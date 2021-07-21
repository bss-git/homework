$(document).ready(async function (e) {

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/api/updates/hub")
        .build()

    hubConnection.on("NewUpdate", function (data) {
        let elem = document.createElement("div")
        elem.className = "list-group-item flex-column align-items-start"
        elem.innerHTML = `<div class="d-flex w-100 justify-content-between">
                <h5 class="mb-1">${data.userName}</h5>
                <small>${data.timestamp}</small>
            </div>
            <p class="mb-1">${data.message}</p>`
        let firstElem = document.getElementById("updates-list").firstChild
        document.getElementById("updates-list").insertBefore(elem, firstElem)

    });

    hubConnection.start()
})
