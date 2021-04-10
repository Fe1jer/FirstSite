function showFilter(item) {
    var list = '';
    list = list + '<a class="schema-tags__item m-1" onclick="setbutton(this, ' + item.attr("id") + '), filterProducts()" title="Категория товара">' +
        '<p class="navbar-nav schema-tags__text">' + item.attr("id") + '</p>' +
        '</a>';
    return list;
}

function setbutton(me, checkbox) {
    var element = me;
    element.remove();
    checkbox.checked = false;
}
