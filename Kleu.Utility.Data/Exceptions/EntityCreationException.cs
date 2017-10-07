using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class EntityCreationException : Exception
    {
        private readonly Exception _exception;

        public EntityCreationException(Exception exception)
        {
            _exception = exception;
        }
    }
}