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
    var list = [];
    $('#filterHeader').empty();
    $('.form-check-input:checked').each(function (id) {
        list.push($(this).attr("id"));
        $('#filterHeader').append(showFilter($(this)));
    });
    searchAjax(list, null).then(() => {
        history.pushState(null, null, url);
    });
};

window.onpopstate = history.onpushstate = function () {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const page = urlParams.get('page')

    var list = URLToArray('filters');
    getFilterFromUrl(list);
    searchAjax(list, page);
}

function URLToArray(sParam) {
    url = window.location.search;
    var request = {};
    var arr = [];
    var pairs = url.substring(url.indexOf('?') + 1).split('&');
    for (var i = 0; i < pairs.length; i++) {
        var pair = pairs[i].split('=');
        if (pair[0].includes(sParam)) {
            //check we have an array here - add array numeric indexes so the key elem[] is not identical.
            if (endsWith(decodeURIComponent(pair[0]), '[' + i + ']')) {
                var arrName = decodeURIComponent(pair[0]).substring(0, decodeURIComponent(pair[0]).length - ('[' + i + ']').length);
                if (!(arrName in arr)) {
                    arr.push(arrName);
                    arr[arrName] = [];
                }

                arr[arrName].push(decodeURIComponent(pair[1]));
                request[arrName] = arr[arrName];
            } else {
                request[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
            }
        }
        else {
            pairs.splice(i, 1);
            i--;
        }
    }
    return request[sParam];
}

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function getFilterFromUrl(list) {
    console.log(list);
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

function changePage(page) {
    var list = [];
    $('.form-check-input:checked').each(function () {
        list.push($(this).attr("id"));
    });
    searchAjax(list, page).then(() => {
        history.pushState(null, null, url);
        window.scrollTo(0, 0);
    });
}