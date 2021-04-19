import { getFriendStatus, acceptFriendOffer, sendFriendOffer } from './friendOfferApi.js';

$(document).ready(async function (e) {
    let friendStatusElement = $('#friend-status');
    if (!friendStatusElement.length)
        return;

    let userId = $(friendStatusElement).data('userId')

    let status = await getFriendStatus(userId)

    switch (status) {
        case 0:
            setOfferButton(friendStatusElement, userId)
            break
        case 1:
            setOfferSentMessage(friendStatusElement)
            break
        case 2:
            setOfferReceivedButton(friendStatusElement, userId)
            break
        case 3:
            setFriendMessage(friendStatusElement)
            break
    }
})

function setOfferButton(element, userId) {
    element.html(`<button class="btn btn-outline-success">Предложить дружбу</button>`)
    element.click(async e => {
        if (await sendFriendOffer(userId))
            setOfferSentMessage(element)
    })
}

function setOfferSentMessage(element) {
    element.html(`<span class="badge badge-primary">Отправлено преджложение дружбы</button>`)
}

function setOfferReceivedButton(element, userId) {
    element.html(`<button class="btn btn-outline-success">Принять дружбу</button>`)
    element.click(async e => {
        if (await acceptFriendOffer(userId))
            setFriendMessage(element)
    })
}

function setFriendMessage(element) {
    element.html(`<span class="badge badge-success">В друзьях</button>`)
}