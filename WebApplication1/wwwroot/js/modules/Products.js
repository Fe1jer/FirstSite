function productsErrorResult() {
    var msg = '<h3 class="text-center mt-3" style="white-space: normal;">Что-то пошло не так, и мы не смогли отобразить список продуктов :(</h2>';
    return (msg);
}

function resultProductsEmpty() {
    var msg = '<h2 class="text-center mt-3" style="white-space: normal;">Результат поиска"нет товаров"</h2>';
    return (msg);
}

var xhr;
var url;

function searchAjax(list, numPage) {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const searchName = urlParams.get('q')

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

function addAttribute() {
    var msg = '<div class="attribute">' +
        '<p>' + $("#name_attribute").val() + '</p>' +
        '<input type="text" class="attributesValue"  name="Product.AttributeValues[' + $('.attribute').length + '].Value" />' +
        '<input type="hidden" class="categoryName" name="Product.AttributeValues[' + $('.attribute').length + '].Attribute.Name" value="' + $("#name_attribute").val() + '" />' +
        '<input type="button" id="delete_button_attribute" value="Удалить" />' +
        '</div>';
    return msg;
}

$(document).ready(function () {
    $(document).on('click', '#delete_button_attribute', function () {
        $(this).parent(".attribute").remove();
        changingIndices();
    });

    $("#add_button_attribute").click(function () {
        if ($("#name_attribute").val() != '') {
            $('.div_product_attributes').append(addAttribute());
        }
        $("#name_attribute").val('');
    });
})

function changingIndices() {
    $('.attribute').each(function (i) {
        $(this).children(".attributesValue").attr("name", "Product.AttributeValues[" + i + "].Value");
        $(this).children(".categoryName").attr("name", "Product.AttributeValues[" + i + "].Attribute.Name");
        $(this).children(".attributesId").attr("name", "Product.AttributeValues[" + i + "].Id");
    });
}