using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class EntityUpdateValidationException : Exception
    {
        private readonly Exception _dbEntityValidationException;

        public EntityUpdateValidationException(Exception dbEntityValidationException)
        {
            _dbEntityValidationException = dbEntityValidationException;
        }
    }
}