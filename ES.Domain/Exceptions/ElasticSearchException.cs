using System;

namespace ES.Domain.Exceptions
{
    public class ElasticSearchException : Exception
    {
        public ElasticSearchException(string message)
            : base(message)
        { }
    }
}
