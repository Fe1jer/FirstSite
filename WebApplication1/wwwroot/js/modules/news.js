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
    $('#search__news').on('keyup', function () {
        console.log("sa");
        var $result = $(this).siblings('#search_box-result');
        var search = $(this).val();
        if (xhr) {
            xhr.abort();
        }
        if ((search != '') && (search.length > 1)) {
            xhr = $.ajax({
                type: "POST",
                url: "/Catalog/GetSearchProduct",
                data: { q: search },
                success: function (msg) {
                    $result.html('<ul class="search_result" id="search_result"></ul>')
                    if (JSON.stringify(msg) != '[]') {

                        $.each(msg.slice(0, 8),
                            function (num, item) {
                                $result.children('#search_result').append(ShowProduct(item));
                            });
                        $result.fadeIn();
                    } else {
                        $result.children('#search_result').html(emptyResult());
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
        var container = $('#search_box__news');
        if (container.has(e.target).length === 0) {
            container.children('#search_box-result').hide();
        }
    });

    $('#search__news').mousedown(function (e) {
        $(this).siblings('#search_box-result').show();
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
    var name = JSON.stringify(item.productType.name).slice(1, -1);
    var company = JSON.stringify(item.productType.company).slice(1, -1);
    var a = "'";
    var msg = '<li onclick="setProduct(' + JSON.stringify(item.id) + ',' + a + name + a + ',' + a + company + a + ')" class="my-2 dropdown-item p-0" style="height: 40px; overflow: hidden; display: flex; cursor:pointer">' +
        '<img src=' + JSON.stringify(item.img) + ' alt=' + name + ' style="width:40px;height:40px;object-fit: contain;" />' +
        '<p class="my-auto ms-2">' + company + ' ' + name + '</p>' +
        '</li>';
    return msg;
}

function setProduct(id, name, company) {
    $('#ProductHref').val('/Catalog/' + name.replace(/ /g, '-') + '?id=' + id);
    $('#selectedProduct').html  (company + ' ' + name);
}