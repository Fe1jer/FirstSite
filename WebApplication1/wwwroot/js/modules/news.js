$('#productCheck').change(function () {
    if ($("#productCheck").prop('checked') == true) {
        $("#productList").show();
        $("#textDiv").hide();
    }
    else {
        $("#productList").hide();
        $("#textDiv").show();
    }
});

$('#caruselCheck').change(function () {
    if ($("#caruselCheck").prop('checked') == true) {
        $("#favImgDiv").show();
    }
    else {
        $("#favImgDiv").hide();
    }
});

$(document).ready(function () {
    var xhr;
    var $result = $('#search_box-result');
    $('#search').on('keyup', function () {
        var search = $(this).val();
        if (xhr) {
            xhr.abort();
        }
        if ((search != '') && (search.length > 1)) {
            xhr = $.ajax({
                type: "POST",
                url: "/Catalog/SearchAjax",
                data: { q: search },
                success: function (msg) {
                    $result.html('<ul class="search_result"></ul>')
                    if (JSON.stringify(msg) != '[]') {

                        $.each(msg.slice(0, 8),
                            function (num, item) {
                                $('.search_result').append(ShowProduct(item));
                            });
                        $result.fadeIn();
                    } else {
                        $('.search_result').append(emptyResult());
                        $result.fadeIn();
                    }
                }
            });
        } else {
            $result.html('');
            $result.fadeOut(100);
        }
    });

    $(document).mouseup(function (e) {
        var container = $result;
        if (container.has(e.target).length === 0) {
            container.hide();
        }
    });

    if (document.getElementById('productCheck').checked) {
        $("#productList").show();
        $("#textDiv").hide();
    }

    if (document.getElementById('caruselCheck').checked) {
        $("#favImgDiv").show();
    }
});

function ShowProduct(item) {
    var name = JSON.stringify(item.product.name).slice(1, -1);
    var company = JSON.stringify(item.product.company).slice(1, -1);
    var msg = '<li onclick="setProductId(' + JSON.stringify(item.product.id) + ')" class="my-2 dropdown-item p-0" style="height: 40px; overflow: hidden; display: flex; cursor:pointer">' +
        '<img class="scale" src=' + JSON.stringify(item.product.img) + ' alt=' + name + ' style="width:40px;height:40px;object-fit: contain;" />' +
        '<p class="my-auto ms-2">' + company + ' ' + name + '</p>' +
        '</li>';
    return msg;
}

function setProductId(id) {
    $('#idProduct').val(id);
}

function emptyResult() {
    var msg = '<div class="">' +
        '<p>Нет таких товаров</p>' +
        '</div>';
    return msg;
}