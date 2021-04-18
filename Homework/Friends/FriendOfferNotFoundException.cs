using System;
using System.Runtime.Serialization;

namespace Homework.Friends
{
    [Serializable]
    internal class FriendOfferNotFoundException : Exception
    {
        public FriendOfferNotFoundException()
        {
        }

        public FriendOfferNotFoundException(string message) : base(message)
        {
        }

        public FriendOfferNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FriendOfferNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}