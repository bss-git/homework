export async function getFriendStatus(userId) {
    let response = await fetch(`/api/friends/${userId}/status`, {
        method: 'Get'
    });

    return (await response.json()).status
}

export async function sendFriendOffer(userId) {
    let response = await fetch('/api/friends/offer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(userId)
    });
    if (response.ok) {
        alert("Предложение отправлено")
        return true
    }
    else {
        alert(await response.text())
        return false
    }
}

export async function acceptFriendOffer(userId) {
    let response = await fetch('/api/friends/accept', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(userId)
    });

    if (response.ok) {
        alert("Предложение принято")
        return true
    }
    else {
        alert(await response.text())
        return false
    }
}
