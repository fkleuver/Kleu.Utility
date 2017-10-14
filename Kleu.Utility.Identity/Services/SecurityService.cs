using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.EntityFramework.Entities;
using Kleu.Utility.Identity.Repositories;
using Kleu.Utility.Logging;
using Microsoft.AspNet.Identity.EntityFramework;
using TokenUsage = IdentityServer3.Core.Models.TokenUsage;
using TokenExpiration = IdentityServer3.Core.Models.TokenExpiration;
using AccessTokenType = IdentityServer3.Core.Models.AccessTokenType;
using Constants = IdentityServer3.Core.Constants;
using ScopeType = IdentityServer3.Core.Models.ScopeType;
using Flows = IdentityServer3.Core.Models.Flows;
using Sha256 = IdentityServer3.Core.Models.HashExtensions;

namespace Kleu.Utility.Identity.Services
{
    public sealed class SecurityService : ISecurityService
    {
        private static readonly string[] IdentityScopes =
        {
            Constants.StandardScopes.OpenId,
            Constants.StandardScopes.Profile,
            Constants.StandardScopes.Email,
            Constants.StandardScopes.Address,
            Constants.StandardScopes.OfflineAccess,
            Constants.StandardScopes.AllClaims,
            Constants.StandardScopes.Roles
        };

        private readonly ILog _logger;
        private readonly ISecurityRepository _securityRepository;
        private readonly IApplicationUserManager _userManager;

        public SecurityService(
            ILog logger,
            ISecurityRepository securityRepository,
            IApplicationUserManager userManager)
        {
            _logger = logger;
            _securityRepository = securityRepository;
            _userManager = userManager;
        }

        public async Task<bool> CreateHybridClientIfNotExists(string id, string name, string secret)
        {
            var exists = await _securityRepository.Exists<Client>(c => c.ClientId == id);
            if (exists)
            {
                Log(true, nameof(Client), id);
            }
            else
            {
                Log(false, nameof(Client), id);
                var client = CreateClient(id, name, Flows.Hybrid);
                await _securityRepository.AddClient(client);

                var clientScopes = IdentityScopes.Select(CreateClientScope);
                foreach (var clientScope in clientScopes)
                {
                    clientScope.Client = client;
                    await _securityRepository.AddClientScope(clientScope);
                }

                var clientSecret = CreateClientSecret(secret);
                clientSecret.Client = client;
                await _securityRepository.AddClientSecret(clientSecret);
            }

            return true;
        }

        public async Task<bool> CreateResourceOwnerClientIfNotExists(string id, string name, string secret, params string[] permissions)
        {
            var exists = await _securityRepository.Exists<Client>(c => c.ClientId == id);
            if (exists)
            {
                Log(true, nameof(Client), id);
            }
            else
            {
                Log(false, nameof(Client), id);
                var client = CreateClient(id, name, Flows.ResourceOwner);
                await _securityRepository.AddClient(client);

                var permissionsToAdd = AppendPermissionsDistinct(permissions, IdentityScopes);
                var clientScopes = permissionsToAdd.Select(CreateClientScope);
                foreach (var clientScope in clientScopes)
                {
                    clientScope.Client = client;
                    await _securityRepository.AddClientScope(clientScope);
                }

                var clientSecret = CreateClientSecret(secret);
                clientSecret.Client = client;
                await _securityRepository.AddClientSecret(clientSecret);
            }

            return true;
        }

        public async Task EnsureScopesPresent(params string[] scopeNames)
        {
            var scopes = scopeNames.Select(CreateResourceScope).ToList();
            scopes.AddRange(IdentityScopes.Select(CreateIdentityScope));


            foreach (var scope in scopes)
            {
                var exists = await _securityRepository.Exists<Scope>(c => c.Name == scope.Name);
                if (exists)
                {
                    Log(true, nameof(Scope), scope.Name);
                }
                else
                {
                    Log(false, nameof(Scope), scope.Name);
                    await _securityRepository.AddScope(scope);
                }
            }
        }

        public async Task<bool> CreateUserIfNotExists(string username, string password, params string[] permissions)
        {
            var exists = await _securityRepository.Exists<ApplicationUser>(u => u.UserName == username);
            if (exists)
            {
                Log(true, nameof(ApplicationUser), username);
            }
            else
            {
                Log(false, nameof(ApplicationUser), username);
                var user = CreateUser(username, password, false);
                await _securityRepository.AddUser(user);

                var permissionsToAdd = AppendPermissionsDistinct(permissions, IdentityScopes);
                var claims = permissionsToAdd.Select(permission => CreateClaim("permission", permission)).ToList();
                claims.Add(CreateClaim("subject", user.UserName));

                foreach (var claim in claims)
                {
                    claim.UserId = user.Id;
                    await _securityRepository.AddClaim(claim);
                }

            }

            return true;
        }

        public async Task<bool> CreateAdminUserIfNotExists(string username, string password)
        {
            var exists = await _securityRepository.Exists<ApplicationUser>(u => u.UserName == username);
            if (exists)
            {
                Log(true, nameof(ApplicationUser), username);
            }
            else
            {
                Log(false, nameof(ApplicationUser), username);
                var user = CreateUser(username, password, true);
                await _securityRepository.AddUser(user);
                
                var claims = IdentityScopes.Select(permission => CreateClaim("permission", permission)).ToList();
                claims.Add(CreateClaim("subject", user.UserName));

                foreach (var claim in claims)
                {
                    claim.UserId = user.Id;
                    await _securityRepository.AddClaim(claim);
                }

            }

            return true;
        }

        public async Task<bool> UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds)
        {
            if (permissionIds == null)
            {
                throw new ArgumentNullException(nameof(permissionIds));
            }

            var user = await _userManager.FindByNameAsync(userName);

            var result = user != null;

            if (result && user.IsAdministrator)
            {
                return true;
            }


            if (result)
            {
                var userPermissions = user.Claims
                    .Where(c => c.ClaimType == "permission")
                    .Select(x => x.ClaimValue)
                    .Distinct();

                result = userPermissions.Intersect(permissionIds, StringComparer.OrdinalIgnoreCase).Any();
            }

            return result;
        }

        public async Task AllowCorsOriginIfNotYetAllowed(string clientId, string origin)
        {
            var exists = await _securityRepository.Exists<ClientCorsOrigin>(o => o.Client.ClientId == clientId && o.Origin == origin);
            if (exists)
            {
                Log(true, nameof(ClientCorsOrigin), origin, nameof(Client), clientId);
            }
            else
            {
                Log(false, nameof(ClientCorsOrigin), origin, nameof(Client), clientId);
                var corsOrigin = CreateOrigin(origin);
                await _securityRepository.AddOrigin(corsOrigin, clientId);
            }
        }

        public async Task AddRedirectUrisIfNotExists(string clientId, string postLogoutRedirectUri, params string[] redirectUris)
        {
            var exists = await _securityRepository.Exists<ClientPostLogoutRedirectUri>(u => u.Client.ClientId == clientId && u.Uri == postLogoutRedirectUri);
            if (exists)
            {
                Log(true, nameof(ClientPostLogoutRedirectUri), postLogoutRedirectUri, nameof(Client), clientId);
            }
            else
            {
                Log(false, nameof(ClientPostLogoutRedirectUri), postLogoutRedirectUri, nameof(Client), clientId);
                await _securityRepository.AddPostLogoutRedirectUri(CreatePostLogoutRedirectUri(postLogoutRedirectUri), clientId);
            }

            foreach (var uri in redirectUris)
            {
                exists = await _securityRepository.Exists<ClientRedirectUri>(u => u.Client.ClientId == clientId && u.Uri == uri);
                if (exists)
                {
                    Log(true, nameof(ClientRedirectUri), uri, nameof(Client), clientId);
                }
                else
                {
                    Log(false, nameof(ClientRedirectUri), uri, nameof(Client), clientId);
                    await _securityRepository.AddRedirectUri(CreateRedirectUri(uri), clientId);
                }
            }
        }

        public async Task AddNameClaimToScopeIfNotExists(string scope)
        {
            var exists = await _securityRepository.Exists<ScopeClaim>(s => s.Name == "name" && s.Scope.Name == scope);
            if (exists)
            {
                Log(true, nameof(ScopeClaim), "name", nameof(Scope), scope);
            }
            else
            {
                Log(false, nameof(ScopeClaim), "name", nameof(Scope), scope);
                await _securityRepository.AddScopeClaim(CreateScopeClaim("name"), scope);
            }
        }

        private void Log(bool exists, string type1, string val1, string type2 = null, string val2 = null)
        {
            var sb = new StringBuilder();
            sb.Append($"{type1} [{val1}] ");
            if (exists)
            {
                sb.Append("already exists");
            }
            else
            {
                sb.Append("does not yet exist");
            }

            if (!string.IsNullOrEmpty(type2))
            {
                sb.Append($" for {type2} [{val2}]");
            }

            if (!exists)
            {
                sb.Append(", adding it now");
            }

            _logger.Info(sb.ToString());
        }

        private static string[] AppendPermissionsDistinct(IEnumerable<string> initialList, params string[] additional)
        {
            var fullList = initialList.ToList();
            fullList.AddRange(additional);
            return fullList.Distinct().ToArray();
        }

        
        private static Client CreateClient(string id, string name, Flows flow)
        {

            return new Client
            {
                ClientId = id,
                ClientName = name,
                Enabled = true,
                RequireConsent = false,
                EnableLocalLogin = true,
                Flow = flow,

                PrefixClientClaims = true,
                AllowAccessTokensViaBrowser = flow == Flows.Hybrid,

                // 5 minutes
                AuthorizationCodeLifetime = 300,
                IdentityTokenLifetime = 300,

                // one hour
                AccessTokenLifetime = 3600,

                // 30 days
                AbsoluteRefreshTokenLifetime = 2592000,

                // 15 days
                SlidingRefreshTokenLifetime = 1296000,

                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute,

                AccessTokenType = AccessTokenType.Jwt,
            };
        }

        private static ClientScope CreateClientScope(string scope)
        {
            return new ClientScope
            {
                Scope = scope
            };

        }
        private static Scope CreateResourceScope(string scope)
        {
            return new Scope
            {
                Name = scope,
                Type = (int)ScopeType.Resource,
                IncludeAllClaimsForUser = true,
                Enabled = true,
                ShowInDiscoveryDocument = true,
                AllowUnrestrictedIntrospection = true,
            };

        }
        private static Scope CreateIdentityScope(string scope)
        {
            return new Scope
            {
                Name = scope,
                Type = (int)ScopeType.Identity,
                IncludeAllClaimsForUser = true,
                Enabled = true,
                ShowInDiscoveryDocument = true,
                AllowUnrestrictedIntrospection = true,
            };
        }

        private static IdentityUserClaim CreateClaim(string type, string value)
        {
            var entity = new IdentityUserClaim
            {
                ClaimType = type,
                ClaimValue = value
            };

            return entity;
        }

        private static ApplicationUser CreateUser(string username, string password, bool isAdmin)
        {
            return new ApplicationUser
            {
                UserName = username,
                PasswordHash = new ApplicationPasswordHasher().HashPassword(password),
                IsAdministrator = isAdmin
            };
        }

        private static ClientCorsOrigin CreateOrigin(string origin)
        {
            return new ClientCorsOrigin
            {
                Origin = origin
            };
        }

        private static ClientRedirectUri CreateRedirectUri(string uri)
        {
            return new ClientRedirectUri
            {
                Uri = uri
            };
        }

        private static ClientPostLogoutRedirectUri CreatePostLogoutRedirectUri(string uri)
        {
            return new ClientPostLogoutRedirectUri
            {
                Uri = uri
            };
        }

        private static ScopeClaim CreateScopeClaim(string claimName)
        {
            return new ScopeClaim
            {
                Name = claimName,
                AlwaysIncludeInIdToken = true
            };
        }

        private static ClientSecret CreateClientSecret(string secret)
        {
            return new ClientSecret { Value = Sha256.Sha256(secret) };
        }

    }
}
