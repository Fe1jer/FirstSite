function cartErrorResult() {
    var msg = '<h3 class="text-center mt-3" style="white-space: normal;">Что-то пошло не так, и мы не смогли отобразить список продуктов :(</h2>';
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
                $('.order_total').remove();
            }
            changeOrderTotalSum();
        },
        error: function () {
            window.location = '/Account/Login'
        }
    });
};
$(document).ready(function () {

    $('li').each(function () {

        var asd = $(this);


        asd.find('.minus').click(function (event, ui) {

            var data = asd.find('.count').val();
            if (data > 1) {
                asd.find('.count').val(parseInt(data) - 1);
            }
            calculateOrderProductSum(asd);
            changeOrderTotalSum();
            return false

        });
        asd.find('.plus').click(function (event, ui) {

            var data = asd.find('.count').val();
            asd.find('.count').val(parseInt(data) + 1);
            calculateOrderProductSum(asd);
            changeOrderTotalSum();
            return false

        });
    });
});

function calculateOrderProductSum(asd) {
    data = asd.find('.count').val();
    var price = parseInt(asd.find(".price").html());
    var total_price = price * data;
    asd.find("#count_price").html(total_price);
}

function changeOrderTotalSum() {
    var summ = 0;
    $('.count_price').each(function (id, el) {
        summ += parseInt(el.textContent);
    });
    $('.cprice').html(summ);
}

$(document).ready(function () {
    changeOrderTotalSum()
});

