function ShowProduct(item, isModer) {
    var name = JSON.stringify(item.product.name).slice(1, -1);
    var company = JSON.stringify(item.product.company).slice(1, -1);
    var shortDesc = JSON.stringify(item.product.shortDesc).slice(1, -1);
    var InCart = JSON.stringify(item.isInCart);
    var msg = '<div class="col-lg-4 mt-2 mb-2">' +
        '<div class="card shadow-sm">' +
        '<div class="scale">' +
        '<a href="/Catalog/' + name.replace(/ /g, '-') + '?id=' + JSON.stringify(item.product.id) + '" >' +
        '<img class="scale card-image w-100" src=' + JSON.stringify(item.product.img) + ' alt="' + name + '" />' +
        '</a>' +
        '</div>' +
        '<div class="card-body">' +
        '<div class="box">' +
        '<div class="h-auto">' +
        '<h5>' + company + ' ' + name + '</h5>' +
        '<p style="font-size: .9375rem; margin-bottom: 0.9rem;">' + shortDesc + '</p>' +
        '</div>' +
        '</div>' +
        '<div class="d-flex justify-content-between">' +
        '<p class="mx-0"><b>Цена: $' + Intl.NumberFormat('en-US').format(JSON.stringify(item.product.price)) + '</b></p>'
    if (JSON.stringify(item.product.isFavourite) == 'true') {
        msg = msg + '<p class="mx-0 text-danger">РеCUMендуем</p>'
    }
    msg = msg + '</div >' +
        '<div id="cardButtons" class="d-flex justify-content-between align-items-center">' +
        '<a type="button" class="btn btn-outline-warning" href="/Catalog/' + name.replace(/ /g, '-') + '?id=' + JSON.stringify(item.product.id) + '" >' +
        '<font style="vertical-align: inherit;">' +
        '<font style="vertical-align: inherit;">' +
        'Подробнее' +
        '</font>' +
        '</font>' +
        '</a>'
    if (isModer == 'True') {
        msg = msg + buttonEditProduct(JSON.stringify(item.product.id));
    }
    if (JSON.stringify(item.product.available) == 'true') {
        if (InCart == 'true') {
            msg = msg + buttonRemoveToCart(JSON.stringify(item.product.id));
        }
        else {
            msg = msg + buttonAddToCart(JSON.stringify(item.product.id));
        }
    }
    else {
        msg = msg + '<a class="text-end btn btn-outline-secondary isDisabled">' +
            'Нет в наличии' +
            '</a>'
    }
    msg = msg + '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
    return (msg);
}

function productsErrorResult() {
    var msg = '<h3 class="text-center mt-3" style="white-space: normal;">Что-то пошло не так, и мы не смогли отобразить список продуктов :(</h2>';
    return (msg);
}

function resultProductsEmpty() {
    var msg = '<h2 class="text-center mt-3" style="white-space: normal;">Результат поиска"нет товаров"</h2>';
    return (msg);
}

function buttonEditProduct(id) {
    msg = '<a type = "button nav-link" class="text-end btn btn-outline-success" href="/Catalog/Edit?id=' + id + '" >' +
        '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">' +
        '<path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />' +
        '<path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />' +
        '</svg>' +
        '</a>'
    return msg
}

var xhr;
/*function filterProducts() {
    var ArrFiltersHref = [];
    var list = [];
    $('#filterHeader').empty();
    $('.form-check-input:checked').each(function () {
        ArrFiltersHref.push('&filters=' + $(this).val());
        list.push($(this).val());
        $('#filterHeader').append(showFilter($(this)));
    });
    

    searchAjax();
};*/

function filterProducts() {
    var ArrFiltersHref = [];
    var list = [];
    $('#filterHeader').empty();
    $('.form-check-input:checked').each(function () {
        ArrFiltersHref.push('&filters=' + $(this).val());
        list.push($(this).val());
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

var searchName;

function searchAjax(list) {
    if (xhr) {
        xhr.abort();
    }
    searchName == undefined ? "" : searchName;
    xhr = $.ajax({
        type: 'POST',
        url: '/Catalog/SearchAjax',
        beforeSend: function () {
            $("#loaderDiv").show();
        },
        data: {
            q: searchName,
            filters: list
        },
        success: function (result) {
            setTimeout(showContent, 100, result)
        },
        error: function (err) {
            if (err.statusText != 'abort') {
                $("#loaderDiv").hide();
                $('#content').empty();
                $('#content').append(productsErrorResult());
            }
        }
    });
}

function showContent(result) {
    $("#loaderDiv").hide();
    $('#content').empty();
    if (JSON.stringify(result) != '[]') {
        $.each(result,
            function (num, item) {
                $('#content').append(ShowProduct(item, isAdminOrModer));
            });
    }
    else {
        $('#content').append(resultProductsEmpty());
    }
}