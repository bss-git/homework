using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends
{
    public class FriendManager
    {
        private IFriendOfferRepository _offerRepository;
        private IFriendLinkRepository _friendLinkRepository;

        public FriendManager(IFriendOfferRepository offerRepository, IFriendLinkRepository friendLinkRepository)
        {
            _offerRepository = offerRepository;
            _friendLinkRepository = friendLinkRepository;
        }

        public Task SendOfferAsync(Guid from, Guid to)
        {
            var offer = new FriendOffer(Guid.NewGuid(), from, to);
            return _offerRepository.SaveAsync(offer);
        }

        internal async Task AcceptOfferAsync(Guid from, Guid to)
        {
            var offer = await _offerRepository.GetAsync(from, to);
            if (offer == null)
                throw new FriendOfferNotFoundException();

            await _offerRepository.DeleteAync(offer.Id);
            await _friendLinkRepository.SaveAsync(from, to);
        }
    }
}
