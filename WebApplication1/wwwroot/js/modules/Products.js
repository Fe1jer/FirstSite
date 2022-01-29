function productsErrorResult() {
    var msg = '<h3 class="text-center mt-3" style="white-space: normal;">Что-то пошло не так, и мы не смогли отобразить список продуктов :(</h2>';
    return (msg);
}

function resultProductsEmpty() {
    var msg = '<h2 class="text-center mt-3" style="white-space: normal;">Результат поиска"нет товаров"</h2>';
    return (msg);
}

var searchName;
var xhr;
var url;

function searchAjax(list, numPage) {
    if (xhr) {
        xhr.abort();
    }
    searchName == undefined ? "" : searchName;
    return xhr = $.ajax({
        type: 'POST',
        url: '/Catalog/GetPartialSearchProduct',
        beforeSend: function () {
            $("#loaderDiv").show();
        },
        data: {
            q: searchName,
            filters: list,
            page: numPage
        },
        success: function (result) {
            $("#loaderDiv").hide();
            $('#content').empty();
            if (JSON.stringify(result) != '[]') {
                $('#content').append(result);
            }
            else {
                $('#content').html(resultProductsEmpty());
            }
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
