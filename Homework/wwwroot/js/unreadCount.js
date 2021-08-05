window.onload = async () => {
    let userId = $('#user-status').data('userId')

    let response = await fetch(`http://${window.location.hostname}:5002/api/counters/${userId}`, {
        method: 'GET'
    });

    if (response.ok) {
        var respData = await response.json()
        $('#unread-updates').text = respData.updates
        $('#unread-dialogs').text = respData.dialogs
    }
}