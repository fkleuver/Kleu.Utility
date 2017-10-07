using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class EntityRetrievalException : Exception
    {
        private readonly Exception _exception;

        public EntityRetrievalException(Exception exception)
        {
            _exception = exception;
        }
    }
}