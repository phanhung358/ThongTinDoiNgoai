var page = 2, isPreviousEventComplete = true, isDataAvailable = true;

var url = location.origin + '/DichVu/ThongTinDoiNgoai/LoadMore.ashx';
var data = {
    page: page,
    WebID: $("#ctl06_WebID").val()
};

$(window).scroll(function () {
    if ($(document).height() - 100 <= $(window).scrollTop() + $(window).height()) {
        if (isPreviousEventComplete && isDataAvailable) {

            isPreviousEventComplete = false;
            $("#ctl06_divDanhSach").append("<div class='loading-mobile'><div style='width: 44px; height: 44px; background: url(" + location.origin + "/Css/Thickbox/fancybox_loading.gif) center center no-repeat; margin: 0 auto;'></div></div>");

            $.ajax({
                type: "GET",
                url: url,
                data: data,
                dataType: 'text',
                success: function (result) {
                    $(".loading-mobile").remove();
                    if (result == '') //When data is not available
                        isDataAvailable = false;
                    else {
                        console.log('Thanh cong');
                        $("#DanhSachTin").append(result);

                        data.page = data.page + 1;
                        isPreviousEventComplete = true;
                    }
                },
                error: function (error) {
                    console.log('Lỗi: ' + error.status + ". " + error.statusText);
                }
            });

        }
    }
});