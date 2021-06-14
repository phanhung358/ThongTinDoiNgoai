function move(numberId, id, file, DonVi_Quy_Nam) {
    var myBarItem = document.getElementById("myBar_" + numberId + "_" + id);
    var fileUploadName = document.getElementById("fileUploadName_" + numberId + "_" + id);

    var elem = document.getElementById("myProgress_" + numberId + "_" + id);

    //send file
    callAjaxSendFile(numberId, id, file, DonVi_Quy_Nam);

    //Running...
    var id = setInterval(frame, 100);
    function frame() {
        //Running complete
        if (elem.value >= 100) {
            clearInterval(id);
            //hidden
            hiddenItem(myBarItem);
            //change style
            changeTextStyle(fileUploadName);
        }
    }
}

function hiddenItem(myBarItem) {
    myBarItem.style.display = "none";
}

function changeTextStyle(fileUploadName) {
    fileUploadName.style.maxWidth = "315px";
    fileUploadName.style.color = "#1155CC";
}

function callAjaxSendFile(numberId, id, file, DonVi_Quy_Nam) {
    var data = new FormData();
    data.append("UploadedImage", file);

    var href = window.location.href.split('/');
    var url = "";
    if (href.length == 5)
        url = href[0] + '//' + href[2] + '/' + href[3];
    else
        url = href[0] + '//' + href[2]

    var ajaxRequest = $.ajax({
        type: "POST",
        url: url + "/api/DocumentUpload/UploadFile/" + DonVi_Quy_Nam + '-' + numberId.toString(),
        contentType: false,
        processData: false,
        data: data,
        xhr: function () {
            var xhr = $.ajaxSettings.xhr();
            xhr.upload.onprogress = function (e) {
                if (e.lengthComputable) {
                    console.log(e.loaded / e.total);
                    //UI update percent loading progress bar
                    $("#myProgress_" + numberId + "_" + id).attr("value", (e.loaded * 100 / e.total).toFixed());
                }
            };
            return xhr;
        }
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus == 'success') {
            if (responseData != null) {
                if (responseData.Key) {
                    //Delete file upload
                    $("#fileClose_" + numberId + "_" + id).unbind("click");
                    $("#fileClose_" + numberId + "_" + id).bind("click", function () {
                        // Remove <div> with id
                        $("#divElement_" + numberId + "_" + id).remove();

                        //DELETE file
                        var fileName = responseData.Value;
                        callAjaxDeleteFile(numberId, fileName);
                        console.log("Deleted");
                    });
                } else {
                    alert("NotContainKey=" + responseData.Value);
                }
            }
        } else {
            alert('Failed' + responseData.Value);
        }
    });

    //Cancel Upload
    $("#fileClose_" + numberId + "_" + id).bind("click", function () {
        $("#divElement_" + numberId + "_" + id).remove();

        ajaxRequest.abort();//Abort upload
        console.log("Canceled");

        //Reset file Upload
        $("#multiFileUpload_" + numberId).val("");
    });
}

function callAjaxDeleteFile(numberId, fileName) {
    var delData = { 'fileName': fileName };

    var href = window.location.href.split('/');
    var url = "";
    if (href.length == 5)
        url = href[0] + '//' + href[2] + '/' + href[3];
    else
        url = href[0] + '//' + href[2]

    var ajaxDeleteRequest = $.ajax({
        type: "POST",
        url: url + "/api/DocumentUpload/DeleteFile?fileName=" + fileName,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        processData: false,
        data: JSON.stringify(delData),
        success: function (response) {
            //alert("Xóa thành công");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Lỗi khi xóa file: " + XMLHttpRequest.toString() + "\n\nStatus: " + fileName);
        }
    });
    //Reset file Upload
    $("#multiFileUpload_" + numberId).val("");
}


function CallXoaFile(Nam_Thang_Quy_ChiTieu, fileName, divID) {

    if (confirm('Bạn có chắc chắn xóa file này không ?') == true) {
        var delData = { 'fileName': fileName };

        var href = window.location.href.split('/');
        var url = "";
        if (href.length == 5)
            url = href[0] + '//' + href[2] + '/' + href[3];
        else
            url = href[0] + '//' + href[2]

        var ajaxDeleteRequest = $.ajax({
            type: "POST",
            url: url + "/api/DocumentUpload/XoaFile?fileName=" + fileName + "&id=" + Nam_Thang_Quy_ChiTieu,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            processData: false,
            data: JSON.stringify(delData),
            success: function (response) {
                if (response != "")
                    alert(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Lỗi khi xóa file: " + XMLHttpRequest.toString() + "\n\nStatus: " + fileName);
            }

        });
        var e = document.getElementById(divID);
        if (e != null)
            e.style.display = "none";
    }
}