using System;

namespace WSEP_doamin.forumManagementDomain

{
    public class InvalidNameException : Exception
    {
        public InvalidNameException(string message) : base(message)
        {
        }
    }
}