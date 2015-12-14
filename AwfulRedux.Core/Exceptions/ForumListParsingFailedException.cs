using System;

namespace AwfulRedux.Core.Exceptions
{
    public class ForumListParsingFailedException : Exception
    {
        public ForumListParsingFailedException()
        {
        }

        public ForumListParsingFailedException(string message)
            : base(message)
        {
        }
    }
}
