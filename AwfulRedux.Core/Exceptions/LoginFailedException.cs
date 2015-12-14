using System;

namespace AwfulRedux.Core.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException()
        {
        }

        public LoginFailedException(string message)
            : base(message)
        {
        }
    }
}
