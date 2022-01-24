$(document).ready(function () {
    var xhr;
    $('#search').on('keyup', function () {
        var $result = $(this).siblings('#search_box-result');
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
                    $result.html('<ul class="search_result" id="search_result"></ul>')
                    if (JSON.stringify(msg) != '[]') {
                        $.each(msg.slice(0, 8),
                            function (num, item) {
                                $result.children('#search_result').append(searchProduct(item));
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
        var container = $('#search_box');
        if (container.has(e.target).length === 0) {
            container.children('#search_box-result').hide();
        }
    });

    $('#search').mousedown(function (e) {
        $(this).siblings('#search_box-result').show();
    });
});

function searchProduct(item) {
    var name = JSON.stringify(item.product.name).slice(1, -1);
    var company = JSON.stringify(item.product.company).slice(1, -1);
    var msg = '<li class="my-2 dropdown-item p-0" style="height: 40px; overflow: hidden; cursor:pointer">' +
        '<a class="d-flex dropdown-item p-0" href="/Catalog/' + name.replace(/ /g, '-') + '?id=' + JSON.stringify(item.product.id) + '">' +
        '<img src=' + JSON.stringify(item.product.img) + ' alt="' + name + '" style="width:40px;height:40px;object-fit: contain;" />' +
        '<p class="my-auto ms-2">' + company + ' ' + name + '</p>' +
        '</a>' +
        '</li>';
    return msg;
}

function emptyResult() {
    var msg = '<p>Нет таких товаров</p>'
    return msg;
}