using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Kleu.Utility.Data.Interceptors
{
    public class InterceptionContext
    {
        public BaseDbContext DatabaseContext { get; internal set; }
        public ObjectContext ObjectContext { get; internal set; }
        public ObjectStateManager ObjectStateManager { get; internal set; }
        public DbChangeTracker ChangeTracker { get; internal set; }
        public IEnumerable<DbEntityEntry> Entries { get; internal set; }
        public ILookup<EntityState, DbEntityEntry> EntriesByState { get; internal set; }

        private readonly List<IInterceptor> _interceptors;

        public InterceptionContext(IInterceptor[] interceptors)
        {
            _interceptors = new List<IInterceptor>(interceptors);
        }

        public void Before()
        {
            var interceptors = _interceptors;
            interceptors.ForEach(intercept => intercept.Before(this));
        }

        public void After()
        {
            var interceptors = _interceptors;
            interceptors.ForEach(intercept => intercept.After(this));
        }
    }
}
