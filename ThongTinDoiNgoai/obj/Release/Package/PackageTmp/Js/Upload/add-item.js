function Upload() {

    //--------------- fileUpload events
    $(".file-input").on('change', function () {

        var fileAcceptedExtension = ['exe', 'dll', 'bat'];

        console.log("CHECK TYPE = " + $.inArray($(this).val().split('.').pop().toLowerCase(), fileAcceptedExtension));

        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileAcceptedExtension) != -1) {
            //alert("Chỉ chấp nhận các định dạng file : " + fileAcceptedExtension.join(', '));
            alert("Định dạng file không cho phép đính kèm!");
        } else {
            //hvnhan
            var arr = $(this).attr('id').split("_");
            var DonVi_Nam_Quy = arr[1];
            //end hvnhan
            var numberId = arr[2];
            console.log("ID clicked= " + $(this).attr('id'));
            console.log("numId= " + numberId);

            //Thông tin File
            var file = $(this).prop('files')[0];
            var filesizeRoundFormat = CalculateFileSize(file.size);
            var filename = $('#multiFileUpload_' + DonVi_Nam_Quy + "_" + numberId).val().replace(/.*(\/|\\)/, '');

            // Finding total number of elements added
            var total_element = $("#divMultiFileUpload_" + numberId + " .element").length;

            // last <div> with element class id
            var lastid = $("#divMultiFileUpload_" + numberId + " .element:last").attr("id");
            var split_id = lastid.split("_");
            var nextindex = Number(split_id[2]) + 1;

            //set MAX item=====
            var max = 10;

            // Check total number elements
            if (total_element <= max) {

                // Adding new div container after last occurance of element class
                $("#divMultiFileUpload_" + numberId + " .element:last").after("<div class='element' id='divElement_" + numberId + "_" + nextindex + "'></div>");

                //var appendItem2 = "<div class='fileUploadContainer'><div id='fileUploadName_" + numberId + "_" + nextindex + "'  class='fileUploadName'>" + filename + "</div><div class='fileUploadSize'>(" + filesizeRoundFormat + ")</div><div id='fileClose_" + numberId + "_" + nextindex + "' class='fileClose'>&times;</div><div id='myBar_" + numberId + "_" + nextindex + "' class='myBar'><progress id='myProgress_" + numberId + "_" + nextindex + "' class='progress is-full is-primary' value='0' max='100'></progress></div></div>";
                var id_ = numberId + "10_" + nextindex.toString();

                var appendItem2 = "<div class='fileUploadContainer Dong_File' id='" + id_ + "'><img src='/images/txt.gif'/><span class='TenFile'>Ten file</span><img src='/images/delete.gif' class='Xoa_File' onclick=CallXoaFile('" + DonVi_Nam_Quy + "-" + numberId + "','" + filename + "','" + id_ + "');" + "></div>";
                $("#divElement_" + numberId + "_" + nextindex).append(appendItem2);

                //load animation progress bar
                $("#myProgress_" + nextindex).load(move(numberId, nextindex, file, DonVi_Nam_Quy));
            }
        }
    });
}

//----- Round File size
function CalculateFileSize(FileInBytes) {
    var strSize = "00";
    if (FileInBytes < 1024)
        strSize = FileInBytes + "B";//Byte
    else if (FileInBytes > 1024 & FileInBytes < 1048576)
        strSize = (FileInBytes / 1024).toFixed(2) + "KB";//Kilobyte
    else if (FileInBytes > 1048576 & FileInBytes < 1073741824)
        strSize = ((FileInBytes / 1024) / 1024).toFixed(2) + "MB";//Megabyte
    else if (FileInBytes > 1073741824 & FileInBytes < 1099511627776)
        strSize = (((FileInBytes / 1024) / 1024) / 1024).toFixed(2) + "GB";//Gigabyte
    else
        strSize = ((((FileInBytes / 1024) / 1024) / 1024) / 1024).toFixed(2) + "TB";//Terabyte
    return strSize;
}
