function setbutton(me, idCheckbox) {
    document.getElementById('button').style.display = 'block';
    var element = me;
    element.remove();
    idCheckbox.checked = false;
}

function setCheck() {
    document.getElementById('button').style.display = 'block';
}