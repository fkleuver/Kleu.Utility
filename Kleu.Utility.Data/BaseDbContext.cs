using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kleu.Utility.Common;
using Kleu.Utility.Data.Interceptors;
using Kleu.Utility.Logging;
// ReSharper disable MemberCanBePrivate.Global

namespace Kleu.Utility.Data
{

    public abstract class BaseDbContext : DbContext, IDbContext
    {
        public Guid Id { get; } = GuidGenerator.GenerateTimeBasedGuid();

        private readonly ILog _logger;

        private readonly IInterceptor[] _interceptors;
        private readonly bool _avoidDisposeForTesting;

        protected BaseDbContext() : base("Default")
        {
            _logger = LogProvider.GetCurrentClassLogger();
        }

        protected BaseDbContext(ILog logger, string nameOrConnectionString) : base(nameOrConnectionString)
        {
            _logger = logger;
            _logger.Info($"New {GetType().Name} created");
            Configure();
        }

        protected BaseDbContext(ILog logger, string nameOrConnectionString, params IInterceptor[] interceptors) : this(logger, nameOrConnectionString)
        {
            _interceptors = interceptors;
        }

        protected BaseDbContext(ILog logger, DbConnection connection, bool avoidDisposeForTesting, bool contextOwnsConnection = true) : base(connection, contextOwnsConnection)
        {
            _logger = logger;
            _logger.Info($"New {GetType().Name} created");
            Configure();
            _avoidDisposeForTesting = avoidDisposeForTesting;
        }

        protected BaseDbContext(ILog logger, DbConnection connection, bool avoidDisposeForTesting, params IInterceptor[] interceptors) : this(logger, connection, avoidDisposeForTesting)
        {
            _interceptors = interceptors;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            base.OnModelCreating(modelBuilder);
        }

        public new void Dispose()
        {
            _logger.Info($"{GetType().Name} disposed");
            if (!_avoidDisposeForTesting)
            {
                base.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _logger.Info($"{GetType().Name} disposed with disposing={disposing}");
            base.Dispose(disposing);
        }

        private void Configure()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public override int SaveChanges()
        {
            return SaveChangesInterceptedAsync(() => Task.FromResult(base.SaveChanges())).Result;
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await SaveChangesInterceptedAsync(() => base.SaveChangesAsync());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await SaveChangesInterceptedAsync(() => base.SaveChangesAsync(cancellationToken));
        }

        private async Task<int> SaveChangesInterceptedAsync(Func<Task<int>> saveChanges)
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries().ToList();

            var entriesByState = entries.ToLookup(row => row.State);

            InterceptionContext intercept = null;

            if (_interceptors != null)
            {
                intercept = new InterceptionContext(_interceptors)
                {
                    DatabaseContext = this,
                    ObjectContext = ((IObjectContextAdapter)this).ObjectContext,
                    ObjectStateManager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager,
                    ChangeTracker = ChangeTracker,
                    Entries = entries,
                    EntriesByState = entriesByState,
                };
            }

            intercept?.Before();
            var result = await saveChanges();
            intercept?.After();

            return result;
        }
    }
}
