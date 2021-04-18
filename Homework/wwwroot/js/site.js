async function SendFriendOffer(to) {
    let response = await fetch('/api/friends/sendOffer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(to)
    });

    if (response.ok)
        alert("Предложение отправлено")
    else
        alert(await response.text())
}

async function AcceptFriendOffer(from) {
    let response = await fetch('/api/friends/acceptOffer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(from)
    });

    if (response.ok)
        alert("Предложение принято")
    else
        alert(await response.text())
}

$(document).ready(function(e) {
    $('#send-offer').click(e => {
        let userId = $(e.target).data('userId')
        SendFriendOffer(userId)
    })

    $('.accept-offer').click(e => {
        let userId = $(e.target).data('userId')
        AcceptFriendOffer(userId)
    })
})