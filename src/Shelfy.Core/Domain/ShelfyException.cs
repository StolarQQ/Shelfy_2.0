using System;

namespace Shelfy.Core.Domain
{
    public class ShelfyException : Exception
    {
        public string Code { get; }

        protected ShelfyException()
        {
        }

        protected ShelfyException(string code)
        {
            Code = code;
        }

        protected ShelfyException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        protected ShelfyException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        protected ShelfyException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        protected ShelfyException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
