using System;
using System.Runtime.Serialization;

namespace Homework.Friends
{
    [Serializable]
    internal class FriendOfferException : Exception
    {
        public FriendOfferException()
        {
        }

        public FriendOfferException(string message) : base(message)
        {
        }

        public FriendOfferException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FriendOfferException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}