function sendUpdate(message) {
    return fetch(`/api/updates/`, {
        method: 'Post',
        body: message
    });
}

$(document).ready(async function (e) {
    $('#send-update').click(() => {
        let message = $('message-input').val()
        var response = await sendUpdate(message)
        alert(await response.text())
    })
})
