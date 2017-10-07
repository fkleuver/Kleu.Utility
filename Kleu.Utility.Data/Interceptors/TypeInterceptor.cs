using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Kleu.Utility.Data.Interceptors
{
    public abstract class TypeInterceptor : IInterceptor
    {
        public Type TargetType { get; }

        protected TypeInterceptor(Type targetType)
        {
            TargetType = targetType;
        }

        public virtual bool IsTargetEntity(DbEntityEntry item)
        {
            return item.State != EntityState.Detached &&
                   TargetType.IsInstanceOfType(item.Entity);
        }

        public void Before(InterceptionContext context)
        {
            foreach (var entry in context.Entries)
                Before(entry);
        }

        public void After(InterceptionContext context)
        {
            foreach (var entryWithState in context.EntriesByState)
            {
                foreach (var entry in entryWithState)
                {
                    After(entry, entryWithState.Key);
                }
            }
        }

        private void Before(DbEntityEntry item)
        {
            if (IsTargetEntity(item))
                OnBefore(item);
        }

        protected abstract void OnBefore(DbEntityEntry item);

        private void After(DbEntityEntry item, EntityState state)
        {
            if (IsTargetEntity(item))
                OnAfter(item, state);
        }

        protected abstract void OnAfter(DbEntityEntry item, EntityState state);
    }
}
