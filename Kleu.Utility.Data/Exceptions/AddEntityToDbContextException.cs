using System;

namespace Kleu.Utility.Data.Exceptions
{
    public class AddEntityToDbContextException : Exception
    {
        private readonly Exception _exception;

        public AddEntityToDbContextException(Exception exception)
        {
            _exception = exception;
        }
    }
}