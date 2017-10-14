using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using Kleu.Utility.Common;
using Kleu.Utility.Logging;

namespace Kleu.Utility.Identity
{
    public sealed class CheckPermissionAttribute : AuthorizeAttribute
    {
        private static readonly string[] EmptyArray = new string[0];

        private string _permission;
        private string[] _permissions = EmptyArray;

        public string Permission
        {
            get => _permission ?? string.Empty;
            set
            {
                _permission = value;
                _permissions = value.SplitClean(',');
            }
        }

        public string[] Permissions
        {
            get => _permissions ?? EmptyArray;
            set
            {
                _permissions = value;
                _permission = string.Join(",", value ?? EmptyArray);
            }
        }
        
        private ILog _logger;
        

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var scope = actionContext.Request.GetDependencyScope().GetRequestLifetimeScope();
            if (_logger == null)
            {
                _logger = scope.Resolve<ILog>();
            }

            var overrideAuthorization = scope.Resolve<IOverrideAuthorization>();
            if (overrideAuthorization.SkipAuthorization)
            {
                return HandleSkipAuthorization(actionContext, scope, overrideAuthorization);
            }
            else
            {
                return HandleDoNotSkipAuthorization(actionContext, scope);
            }

        }

        private bool HandleSkipAuthorization(HttpActionContext actionContext, IComponentContext requestScope, IOverrideAuthorization overrideAuthorization)
        {
            var currentUserFactory = requestScope.Resolve<Func<ICurrentUser>>();

            var currentUser = currentUserFactory?.Invoke();
            if (currentUser != null)
            {
                currentUser.UserName = overrideAuthorization.UserName;
            }

            return true;
        }

        private bool HandleDoNotSkipAuthorization(HttpActionContext actionContext, IComponentContext requestScope)
        {
            var isAuthorized = base.IsAuthorized(actionContext);

            _logger.Info($"[{nameof(CheckPermissionAttribute)}] {nameof(isAuthorized)}: {isAuthorized}");

            if (isAuthorized)
            {
                var securityService = requestScope.Resolve<ISecurityService>();
                var principal = actionContext.RequestContext.Principal;
                isAuthorized = IsAuthorized(securityService, principal).Result;

                if (isAuthorized)
                {
                    var currentUserFactory = requestScope.Resolve<Func<ICurrentUser>>();
                    var currentUser = currentUserFactory.Invoke();
                    currentUser.UserName = GetPrincipalName(principal);
                }
            }


            return isAuthorized;

        }

        private IEnumerable<Claim> GetClaims(IPrincipal principal)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal?.Claims != null && claimsPrincipal.Claims.Any())
            {
                return claimsPrincipal.Claims;
            }
            else
            {
                _logger.Error("Access token is valid, but no claims found on the principal");
                return Enumerable.Empty<Claim>();
            }
        }

        private string GetPrincipalName(IPrincipal principal)
        {
            var name = principal.Identity.Name;

            if (string.IsNullOrEmpty(name))
            {
                var subjectClaim = GetClaims(principal).FirstOrDefault(c => c.Type == "subject");
                if (subjectClaim != null)
                {
                    name = subjectClaim.Value;
                }
                else
                {
                    _logger.Error("Access token is valid and principal has claims, but no subject claim was found");
                }
            }

            return name;
        }

        private async Task<bool> IsAuthorized(ISecurityService securityService, IPrincipal principal)
        {
            var isAuthorized = false;


            if (securityService != null && principal != null)
            {

                var userPermissions = GetClaims(principal)
                    .Where(c => c.Type == "permission")
                    .Select(x => x.Value)
                    .Distinct()
                    .ToArray();

                isAuthorized = userPermissions.Intersect(userPermissions, StringComparer.OrdinalIgnoreCase).Any();

                if (!isAuthorized)
                {
                    _logger.Warn("Requested permission not found in principal claims, checking claims from the claim store");
                    isAuthorized = await securityService.UserHasAnyPermission(GetPrincipalName(principal), null, userPermissions);
                }
            }
            else
            {
                var nullParam = securityService == null ? nameof(securityService) : nameof(principal);
                _logger.Warn($"{nullParam} is null, unable to check permissions");
            }

            var permissions = string.Join(", ", _permissions);
            _logger.Info($"[{nameof(CheckPermissionAttribute)}] has permission to [{permissions}]: {isAuthorized}");




            return isAuthorized;
        }
    }
}