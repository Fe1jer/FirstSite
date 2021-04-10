function ShowProduct(item, isModer) {
    var name = JSON.stringify(item.product.name).replace(/"/g, "");
    console.log(name);
    var company = JSON.stringify(item.product.company).replace(/"/g, "");
    var shortDesc = JSON.stringify(item.product.shortDesc).replace(/"/g, "");
    var InCart = JSON.stringify(item.isInCart);
    var msg = '<div class="col-lg-4 mt-2 mb-3">' +
        '<div class="card shadow-sm">' +
        '<div class="scale">' +
        '<a href="/Products/Product/' + name + '?id=' + JSON.stringify(item.product.id) + '" >' +
        '<img class="scale card-image w-100" src=' + JSON.stringify(item.product.img) + ' alt=' + name + ' />' +
        '</a>' +
        '</div>' +
        '<div class="card-body">' +
        '<div class="box">' +
        '<div class="h-auto">' +
        '<h5>' + company + ' ' + name + '</h5>' +
        '<p style="font-size: .9375rem; margin-bottom: 0.9rem;">' + shortDesc + '</p>' +
        '</div>' +
        '</div>' +
        '<p><b>Цена: $' + Intl.NumberFormat('en-US').format(JSON.stringify(item.product.price)) + '</b></p>' +
        '<div id="cardButtons" class="d-flex justify-content-between align-items-center">' +
        '<a type="button" class="btn btn-outline-warning" href="/Products/Product/' + name + '?id=' + JSON.stringify(item.product.id) + '" >' +
        '<font style="vertical-align: inherit;">' +
        '<font style="vertical-align: inherit;">' +
        'Подробнее' +
        '</font>' +
        '</font>' +
        '</a>'
    if (isModer == 'True') {
        msg = msg + buttonEditProduct(JSON.stringify(item.product.id));
    }
    if (InCart == 'true') {
        msg = msg + buttonRemoveToCart(JSON.stringify(item.product.id));
    }
    else {
        msg = msg + buttonAddToCart(JSON.stringify(item.product.id));
    }
    msg = msg + '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
    return (msg);
}

function resultProductsEmpty() {
    var msg = '<h2 class="text-center mt-3" style="white-space: normal;">Результат поиска"нет товаров"</h2>';
    return (msg);
}

function resultShopCartEmpty() {
    var msg = '<h2 class="text-center mt-3">Добавьте что-нибудь в корзину, чтобы она не была пустая</h2>';
    return (msg);
}

function buttonAddToCart(id) {
    msg = '<a type="button" onclick="addToCart(this, ' + id + ')" class="text-end btn btn-outline-success">' +
        '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-bag-plus" viewBox="0 0 16 16">' +
        '<path fill-rule="evenodd" d="M8 7.5a.5.5 0 0 1 .5.5v1.5H10a.5.5 0 0 1 0 1H8.5V12a.5.5 0 0 1-1 0v-1.5H6a.5.5 0 0 1 0-1h1.5V8a.5.5 0 0 1 .5-.5z" />' +
        '<path d="M8 1a2.5 2.5 0 0 1 2.5 2.5V4h-5v-.5A2.5 2.5 0 0 1 8 1zm3.5 3v-.5a3.5 3.5 0 1 0-7 0V4H1v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V4h-3.5zM2 5h12v9a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V5z" />' +
        '</svg>' +
        ' В корзину' +
        '</a>'
    return msg;
}

function buttonRemoveToCart(id) {
    msg = '<a type="button" onclick="removeToCart(this, ' + id + ')" class="text-end btn btn-outline-success">' +
        '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-bag-check" viewBox="0 0 16 16">' +
        '<path fill-rule="evenodd" d="M10.854 8.146a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 0 1 .708-.708L7.5 10.793l2.646-2.647a.5.5 0 0 1 .708 0z" />' +
        '<path d="M8 1a2.5 2.5 0 0 1 2.5 2.5V4h-5v-.5A2.5 2.5 0 0 1 8 1zm3.5 3v-.5a3.5 3.5 0 1 0-7 0V4H1v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V4h-3.5zM2 5h12v9a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V5z" />' +
        '</svg>' +
        ' В корзине' +
        '</a>'
    return msg;
}

function buttonEditProduct(id) {
    msg = '<a type = "button nav-link" class="text-end btn btn-outline-success" href="/Products/ChangeProduct?id=' + id + '" >' +
        '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">' +
        '<path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />' +
        '<path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />' +
        '</svg>' +
        '</a>'
    return msg
}

function addToCart(item, id) {
    $.ajax({
        type: 'POST',
        beforeSend: function () {
            $(item).addClass('isDisabled');
        },
        url: '/ShopCart/AddToCart',
        data: { IdProduct: id },
        success: function () {
            $(item).parent("#cardButtons").append(buttonRemoveToCart(id));
            $(item).remove();
        },
        error: function () {
            window.location = '/Account/Login'
        }
    });
};

function removeToCart(item, id) {
    $.ajax({
        type: 'POST',
        beforeSend: function () {
            $(item).addClass('isDisabled');
        },
        url: '/ShopCart/RemoveToCart',
        data: { IdProduct: id },
        success: function () {
            $(item).parent("#cardButtons").append(buttonAddToCart(id));
            $(item).remove();
        },
        error: function () {
            window.location = '/Account/Login'
        }
    });
};

function removeItemToCart(item, id, idParent) {
    $.ajax({
        type: 'POST',
        beforeSend: function () {
            $(item).addClass('isDisabled');
        },
        url: '/ShopCart/RemoveToCart',
        data: { IdProduct: id },
        success: function (count) {
            $(item).parent(idParent).remove();
            if (JSON.stringify(count) === '0') {
                $('#shopCart').append(resultShopCartEmpty());
                $('#orderButton').remove();
            }
        },
        error: function () {
            window.location = '/Account/Login'
        }
    });
};