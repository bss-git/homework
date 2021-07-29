function sendUpdate(message) {
    return fetch(`/api/updates/`, {
        method: 'Post',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(message)
    });
}

$(document).ready(async function (e) {
    $('#send-update').click(async e => {
        e.preventDefault()
        let message = $('#message-input').val()
        var response = await sendUpdate(message)
        if (!response.ok) {
            alert(await response.text())
            return
        }
        $('#message-input').val('')
    })
})
