using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class EntityCreationValidationException : Exception
    {
        private readonly Exception _dbEntityValidationException;

        public EntityCreationValidationException(Exception dbEntityValidationException)
        {
            _dbEntityValidationException = dbEntityValidationException;
        }
    }
}