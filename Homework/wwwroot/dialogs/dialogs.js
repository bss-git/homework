window.onload = () => {
    document.getElementById('send_button').addEventListener("click", function () {
        let div = document.createElement('div');
        let text = document.getElementById('send_text').value;
        div.className = 'message my_message';
        div.innerHTML = text;
        document.getElementsByClassName('message_container')[0].append(div);
    })
}