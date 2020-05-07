using System;

namespace ES.Domain.Exceptions
{
    public class InvalidArgumentException: Exception
    {
        public InvalidArgumentException(string message)
            : base(message)
        { }
    }
}
