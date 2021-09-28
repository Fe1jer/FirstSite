function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}
// проверяем, есть ли у нас cookie, с которой мы не показываем окно и если нет, запускаем показ
var alertwin = getCookie("alertwin");
if (alertwin != "no" & loggedIn == 'true') {
    var summ;
    $(document).mouseleave(function (e) {
        if (e.clientY < 10) {
            $(".exitblock").fadeIn("fast");
            // записываем cookie на 1 день, с которой мы не показываем окно
            var date = new Date;
            date.setDate(date.getDate() + 3660);
            document.cookie = "alertwin=no; path=/; expires=" + date.toUTCString();
        }
    });
    $('input[name="rating"]').on('click', function () {
        summ = $(this).val();
    })
    $(document).click(function (e) {
        if (($(".exitblock").is(':visible')) && (!$(e.target).closest(".exitblock .modaltext").length)) {
            $(".exitblock").remove();
            if (summ != undefined) {
                $.ajax({
                    type: 'POST',
                    url: '/SiteRating/AddRating',
                    data: {
                        rating: summ
                    },
                    success: function () {
                    },
                    error: function () {
                        alert("error");
                    }
                });
            }
        }
    });
}

