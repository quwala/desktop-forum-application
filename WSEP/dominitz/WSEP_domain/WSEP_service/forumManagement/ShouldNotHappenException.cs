using System;
using System.Runtime.Serialization;

namespace WSEP_service.forumManagement
{
    [Serializable]
    internal class ShouldNotHappenException : Exception
    {
        public ShouldNotHappenException()
        {
        }

        public ShouldNotHappenException(string message) : base(message)
        {
        }

        public ShouldNotHappenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ShouldNotHappenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}