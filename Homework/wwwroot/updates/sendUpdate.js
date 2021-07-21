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
        var update = await response.json()
        $('.list-group').prepend(
        `<div class="list-group-item flex-column align-items-start" >
            <div class= "d-flex w-100 justify-content-between" >
                <h5 class="mb-1">${update.username}</h5>
                <small>${update.timestamp}</small>
            </div >
            <p class="mb-1">${message}</p>
        </div >`)

    })
})
