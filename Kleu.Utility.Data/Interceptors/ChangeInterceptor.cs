using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Kleu.Utility.Data.Interceptors
{
    public class ChangeInterceptor<T> : TypeInterceptor
    {
        public ChangeInterceptor() : base(typeof(T))
        {

        }

        protected override void OnBefore(DbEntityEntry item)
        {
            var tItem = (T)item.Entity;
            switch (item.State)
            {
                case EntityState.Added:
                    OnBeforeInsert(item, tItem);
                    break;
                case EntityState.Deleted:
                    OnBeforeDelete(item, tItem);
                    break;
                case EntityState.Modified:
                    OnBeforeUpdate(item, tItem);
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnAfter(DbEntityEntry item, EntityState state)
        {
            var tItem = (T)item.Entity;
            switch (state)
            {
                case EntityState.Added:
                    OnAfterInsert(item, tItem);
                    break;
                case EntityState.Deleted:
                    OnAfterDelete(item, tItem);
                    break;
                case EntityState.Modified:
                    OnAfterUpdate(item, tItem);
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public virtual void OnBeforeInsert(DbEntityEntry entry, T item)
        {
        }

        public virtual void OnAfterInsert(DbEntityEntry entry, T item)
        {
        }

        public virtual void OnBeforeUpdate(DbEntityEntry entry, T item)
        {
        }

        public virtual void OnAfterUpdate(DbEntityEntry entry, T item)
        {
        }

        public virtual void OnBeforeDelete(DbEntityEntry entry, T item)
        {
        }

        public virtual void OnAfterDelete(DbEntityEntry entry, T item)
        {
        }
    }
}
