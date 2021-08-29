window.onload = () => {
    let sendButton = document.getElementById('send_button')
    let textInput = document.getElementById('send_text')

    sendButton.addEventListener("click", function () {
        let recipient = document.getElementsByClassName('contact_item active')[0].dataset.userId
        sendMessage(recipient, textInput, sendButton)
    })

    textInput.addEventListener('input', (event) => {
        if (textInput.value)
            sendButton.disabled = false
        else
            sendButton.disabled = true
    });

    let contacts = document.getElementsByClassName('contact_item')

    for (let i = 0; i < contacts.length; i++) {
        contacts[i].addEventListener("click", function (e) {
            if (e.currentTarget.classList.contains("active"))
                return

            let contacts = document.getElementsByClassName('contact_item')
            for (let i = 0; i < contacts.length; i++) {
                contacts[i].classList.remove("active");
            }

            e.currentTarget.className += " active"
            let contactName = e.currentTarget.dataset.contactName
            document.getElementById('contact_name').innerText = `Чат с ${contactName}`
            let userId = e.currentTarget.dataset.userId
            startMessageUpdating(userId)
            textInput.disabled = false
        })
    }
}

var messagesUpdateInterval;

async function startMessageUpdating(userId) {
    clearInterval(messagesUpdateInterval)

    if (await loadMessages(userId)) {
    }
    else {
        alert('Не удалось загрузить данные')
    }

    messagesUpdateInterval = setInterval(() => { loadMessages(userId) }, 3000)

    async function loadMessages(userId) {
        let response = await fetch(`/api/dialogs/${userId}`, {
            method: 'GET'
        });

        if (response.ok) {
            let messages = await response.json()
            clearMessages()
            for (let i = 0; i < messages.length; i++) {
                addMessageToList(messages[i].text, messages[i].isMy)
            }

            return true
        }

        return false
    }
}

async function sendMessage(recipient, textInput, sendButton) {
    sendButton.disabled = true
    let text = textInput.value
    let response = await fetch('/api/dialogs/message', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify({ to: recipient, text: text })
    });

    if (response.ok) {
        textInput.value = ''
        addMessageToList(text, true)
        scrollDown()
    }
    else {
        sendButton.disabled = false
        alert(await response.text())
    }
}

function clearMessages() {
    let messages = document.getElementsByClassName('message_container')[0]
    messages.innerHTML = ''
}

function addMessageToList(text, isMy) {
    let div = document.createElement('div')

    if (isMy)
        div.className = 'message my_message'
    else
        div.className = 'message'

    div.innerHTML = text
    let messages = document.getElementsByClassName('message_container')[0]
    messages.append(div)
}

function scrollDown() {
    let messages = document.getElementsByClassName('message_container')[0]
    messages.scrollTop = messages.scrollHeight
}