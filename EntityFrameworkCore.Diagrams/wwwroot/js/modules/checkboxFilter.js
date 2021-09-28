function showFilter(item) {
    var list = '';
    var id = "'" + item.attr('id') + "'";
    list = list + '<a class="schema-tags__item m-1" onclick="setbutton(this, ' + id + '), filterProducts()" title="Категория товара">' +
        '<p class="navbar-nav schema-tags__text">' + item.val() + '</p>' +
        '</a>';
    return list;
}

function setbutton(me, checkbox) {
    me.remove();
    document.getElementById(checkbox).checked = false;
}

function filterProducts() {
    var ArrFiltersHref = [];
    var list = [];
    $('#filterHeader').empty();
    $('.form-check-input:checked').each(function () {
        ArrFiltersHref.push('&filters=' + $(this).attr("id"));
        list.push($(this).attr("id"));
        $('#filterHeader').append(showFilter($(this)));
    });
    if (window.location.pathname == '/Catalog') {
        var filtersHref = '?' + ArrFiltersHref.join('').slice(1);
        var winLocHref = window.location.href.split("?");
        history.pushState(null, null, winLocHref[0] + ((filtersHref == "?") ? "" : filtersHref));
    }
    else {
        var winLocHref = window.location.href.split("&");
        history.pushState(null, null, winLocHref[0] + ArrFiltersHref.join(''));
    }
    searchAjax(list);
};

window.onpopstate = history.onpushstate = function (e) {
    var list = getFilterFromUrl();
    searchAjax(list);
}

function getFilterFromUrl() {
    var list = window.location.search.replace(/\&/g, '').split("filters=");
    list.shift();
    $('#filterHeader').empty();
    $('.form-check-input:checked').each(function () {
        this.checked = false;
    });
    $(list).each(function () {
        $('#filterHeader').append(showFilter($(document.getElementById(this))));
        document.getElementById(this).checked = true;
    });
    return list;
}