using System;

namespace AwfulRedux.Core.Exceptions
{
    public class WebManagerException : Exception
    {
        public WebManagerException()
        {
        }

        public WebManagerException(string message)
            : base(message)
        {
        }
    }
}
