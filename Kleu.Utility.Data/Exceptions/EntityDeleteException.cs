using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class EntityDeleteException : Exception
    {
        private readonly Exception _exception;

        public EntityDeleteException(Exception exception)
        {
            _exception = exception;
        }
    }
}