// JScript File
jQuery(function($){
    $.datepicker.regional['vi-VN'] = {
        buttonText: "Chọn ngày",
        closeText: "Đóng",
        currentText: "Ngày hiện tại",
        dateFormat: "dd/mm/yy",
        dayNames: [ "Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy" ],
        dayNamesMin: [ "CN", "T2", "T3", "T4", "T5", "T6", "T7" ],
        dayNamesShortType: [ "CN", "T2", "T3", "T4", "T5", "T6", "T7" ],
        firstDay: 1,
        isRTL: false,
        monthNames: [ "Tháng một", "Tháng hai", "Tháng ba", "Tháng tư", "Tháng năm", "Tháng sáu", "Tháng bảy", "Tháng tám", "Tháng chín", "Tháng mười", "Tháng mười một", "Tháng mười hai" ],
        monthNamesShort: [ "Th.1", "Th.2", "Th.3", "Th.4", "Th.5", "Th.6", "Th.7", "Th.8", "Th.9", "Th.10", "Th.11", "Th.12" ],
        nextText: "Sau",
        prevText: "Trước",
        weekHeader: "Tuần"
    };
    $.datepicker.setDefaults($.datepicker.regional['vi-VN']);
});
function popUpCalendar(ctlID, ctl2ID, format, object) {
    //alert('aaaaa');
	var formatChar = "/";
	var formatYear = "yy";
	var formatArray = format.split(formatChar);
	if (formatArray.Length < 3)
		alert("Không đúng định dạng ngày, tháng, năm !");
	if (formatArray[2] == "yyyy")
		formatYear = "yy";
	else if (formatArray[2] == "yy")
		formatYear = "y";
	format = formatArray[0] + "/" + formatArray[1] + "/" + formatYear;
    $( "#" + object ).datepicker({
        buttonImageOnly: true,
		buttonText: "",
        changeMonth: true,
        changeYear: true,
		dateFormat: format,
        showButtonPanel: true,
        showOn: "button",
        yearRange: "c-100:c+100"
    });
	$( "#" + object ).datepicker("show");
	$( '.ui-datepicker-trigger').css('display', 'none');
}