using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WpOportunidades.Infrastructure.Exceptions
{
    public class EnderecoException : Exception
    {
        public EnderecoException()
        {
        }

        public EnderecoException(string message) : base(message)
        {
        }

        public EnderecoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EnderecoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
