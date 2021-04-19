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

        public async Task SendOfferAsync(Guid from, Guid to)
        {
            var offer = await _offerRepository.GetAsync(from, to);
            if (offer != null)
                throw new FriendOfferException("Предложение уже отправлено");

            offer = await _offerRepository.GetAsync(to, from);
            if (offer != null)
                throw new FriendOfferException("Пользователь уже отправил вам предложение");

            offer = new FriendOffer(Guid.NewGuid(), from, to);
            await _offerRepository.SaveAsync(offer);
        }

        public async Task AcceptOfferAsync(Guid from, Guid to)
        {
            var offer = await _offerRepository.GetAsync(from, to);
            if (offer == null)
                throw new FriendOfferException("Предложение држубы не найдено");

            await _offerRepository.DeleteAync(offer.Id);
            await _friendLinkRepository.SaveAsync(from, to);
        }

        public async Task<FriendStatus> GetFriendStatusAsync (Guid from, Guid to)
        {
            var offer = await _offerRepository.GetAsync(from, to);
            if (offer != null)
                return FriendStatus.OfferSent;

            offer = await _offerRepository.GetAsync(to, from);
            if (offer != null)
                return FriendStatus.OfferReceived;

            var friendIds = await _friendLinkRepository.GetFriendIdsAsync(from);
            if (friendIds.Contains(to))
                return FriendStatus.Friend;

            return FriendStatus.Unset;
        }
    }
}
