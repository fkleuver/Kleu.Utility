using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class DbContextResolutionException : Exception
    {
        private readonly Exception _exception;

        public DbContextResolutionException(Exception exception)
        {
            _exception = exception;
        }
    }
}