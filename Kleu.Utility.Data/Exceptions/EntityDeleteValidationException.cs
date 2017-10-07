using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class EntityDeleteValidationException : Exception
    {
        private readonly Exception _dbEntityValidationException;

        public EntityDeleteValidationException(Exception dbEntityValidationException)
        {
            _dbEntityValidationException = dbEntityValidationException;
        }
    }
}