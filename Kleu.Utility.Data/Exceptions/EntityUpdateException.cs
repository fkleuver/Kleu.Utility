using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class EntityUpdateException : Exception
    {
        private readonly Exception _exception;

        public EntityUpdateException(Exception exception)
        {
            _exception = exception;
        }
    }
}