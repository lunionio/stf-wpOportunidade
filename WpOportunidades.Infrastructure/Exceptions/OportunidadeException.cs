using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WpOportunidades.Infrastructure.Exceptions
{
    public class OportunidadeException : Exception
    {
        public OportunidadeException()
        {
        }

        public OportunidadeException(string message) : base(message)
        {
        }

        public OportunidadeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OportunidadeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
