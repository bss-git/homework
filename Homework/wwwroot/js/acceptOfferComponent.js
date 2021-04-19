import { acceptFriendOffer } from './friendOfferApi.js';

$(document).ready(function (e) {
    $('.accept-offer').click(async e => {
        let userId = $(e.target).data('userId')
        if (await acceptFriendOffer(userId))
            $(e.target.parentElement).html('<span class="badge badge-success">Предложение принято</span>')
    })
})