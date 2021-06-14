function DisplayCombo(id) {
    var e = document.getElementById(id);
    if (e.style.display == "none")
        e.style.display = "";
    else
        e.style.display = "none";
}
function SelectItem(chkAll, txt, name) {
    var eAll = document.getElementById(chkAll);
    eAll.checked = false;

    var frm = document.forms[0];
    var sl = 0;
    for (j = 0; j < frm.length; j++) {
        e = frm.elements[j];
        if (e.type == 'checkbox' && e.name.indexOf(name) != -1 && e.checked) {
            sl = sl + 1;
        }
    }
    if (sl == 0) {
        document.getElementById(txt).value = "[Tất cả]";
        eAll.checked = true;
    }
    else
        document.getElementById(txt).value = "Chọn " + sl.toString() + ' loại';
}

function SelectAll(chkAll, txt, name) {
    var all = document.getElementById(chkAll);
    var frm = document.forms[0];
    for (j = 0; j < frm.length; j++) {
        e = frm.elements[j];
        if (e.type == 'checkbox' && e.name.indexOf(name) != -1) {
            e.checked = all.checked;
        }
    }
    document.getElementById(txt).value = "[Tất cả]";
}

function ThucHien(obj) {
    var obj = document.getElementById(obj);
    setTimeout(function () {
        obj.click();
    }, 200);
}
function getUrl()
{
    var href = window.location.href.split('/');
    var url = "";
    if (href.length == 5)
        url = href[0] + '//' + href[2] + '/' + href[3];
    else
        url = href[0] + '//' + href[2]
    url = url.replace("/phananh", "");
    return url;
}

function AnHienTK(ctr, Dongid, count) {
    var e = document.getElementById(ctr + "_" + Dongid + "_0");
    if (e.style.display == "none") {
        e.style.display = "";
    }
    else {
        e.style.display = "none";
    }

    for (j = 1; j < count; j++) {
        var e1 = document.getElementById(ctr + "_" + Dongid + "_" + j.toString());

        e1.style.display = e.style.display
    }
}
function EnterNextFocus(id, e) {
    var control = document.getElementById(id);
    var key = e.which;
    if (key == 13) {
        control.focus();
    }
}
function SendEmail(sUrl) {
    $.ajax
    ({
        type: 'POST',
        url: sUrl,
        data: '',
        success: function (msg) {
        }
    });
}

function LoadingScreen() {
    $.fancybox.showLoading();
}
function Tab(id1, id2) {
    document.getElementById(id1).style.display = "";
    if (document.getElementById(id2) != null)
        document.getElementById(id2).style.display = "none";
}

function Tab(id1, id2, csstab, csstabselect) {
    document.getElementById(id1).style.display = "";
    document.getElementById(id1.replace("TienIch", "Tab")).className = csstabselect;
    if (document.getElementById(id2) != null) {
        document.getElementById(id2).style.display = "none";
        document.getElementById(id2.replace("TienIch", "Tab")).className = csstab;
    }
}
function Dem(loaidem) {
    $.ajax
        ({

            type: "POST",
            url: "CaMera/DemLuot.aspx?loaidem=" + loaidem,
            data: "",
            success: function (msg) {
            }
        });
}
function heightIframe(id) {
    var dodai_trang = document.getElementById(id).contentWindow.document.body.scrollHeight;
    document.getElementById(id).height = dodai_trang;
}
function resizeIframe(iframeID) {


    var FramePageHeight = bo.scrollHeight + 10; /* framePage 
is the ID of the framed page's BODY tag. The added 10 pixels prevent an 
unnecessary scrollbar. */


    parent.document.getElementById(iframeID).style.height = FramePageHeight;
}



var flag = 0;
function loadPopup() {
    $(document).ready(function () {
        tb_init('a.thickbox, area.thickbox, input.thickbox');//pass where to apply thickbox
        imgLoader = new Image();// preload image
        imgLoader.src = tb_pathToImage; return false;
    });
}
//Dang su dung
function windowPopupFile(url, id, width, height) {
    if (width == 0)
        width = window.screen.availWidth - 50;
    if (height == 0)
        height = window.screen.availHeight - 100;
    var left = window.screen.availWidth / 2 - width / 2;
    var top = window.screen.availHeight / 2 - height / 2;
    var resizable = 'yes';
    var scrollbars = 'yes';
    var obj = document.getElementById(id);
    if (obj != null) {
        url += "&file=" + obj.value;
    }
    window.open(url, '', 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top + ',resizable=' + resizable + ',scrollbars=' + scrollbars);
    return false;
}

function ShowMenu(sObj, sClassName) {
    var nLen = sClassName.length;
    var sClsObj = sObj.className;
    var nTmpLen = sClsObj.length;
    if (nLen == nTmpLen) {
        sClassName = sClassName + "Over";
    }
    sObj.className = sClassName;
}

function checkMail(txt) {
    var x = document.getElementById(txt).value;
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (filter.test(x))
        return true;
    else
        return false;
}

function CheckLogin(txt1) {
    if (document.getElementById(txt1).value == "") {
        alert("Chưa nhập tên đăng nhập");
        return false;
    }
    else
        return true;
}

function showdate() {
    d = new Date();
    var a = new Array("Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy");
    s = "<b>" + a[d.getDay()] +
        ", ngày " + (d.getDate() < 10 ? "0" + d.getDate() : d.getDate()) +
        " tháng " + (d.getMonth() + 1 < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1)) +
        " năm " + d.getFullYear() + "</b>";
    document.write(s);
}
function showdateEnglish() {
    d = new Date();
    var a = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
    s = "<b>" + a[d.getDay()] +
        ", " + (d.getMonth() + 1 < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1)) +
        "." + (d.getDate() < 10 ? "0" + d.getDate() : d.getDate()) +
        "." + d.getFullYear() + "</b>";
    document.write(s);
}
function showdateShort() {
    d = new Date();
    var a = new Array("Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy");
    s = "" +//+ a[d.getDay()] + 
        "" + (d.getDate() < 10 ? "0" + d.getDate() : d.getDate()) +
        "." + (d.getMonth() + 1 < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1)) +
        "." + d.getFullYear() + "";

    document.write(s);
}
function ChonTatCa(check) {
    var frm = document.forms[0];
    for (j = 0; j < frm.length; j++) {
        e = frm.elements[j];
        if (e.type == 'checkbox' && !e.disabled) {
            e.checked = check.checked;
        }
    }
}
function checkall(chk) {
    var frm = document.forms[0];
    for (j = 0; j < frm.length; j++) {
        e = frm.elements[j];
        if (e.type == 'checkbox' && e.name.indexOf(chk) != -1 && !e.disabled) {
            if (flag == 0)
                e.checked = true;
            else
                e.checked = false;
        }
    }
    if (flag == 0)
        flag = 1
    else
        flag = 0
}
function subCheck(chkall, chk) {
    var frm = document.forms[0];
    flag = true;
    for (j = 0; j < frm.length; j++) {
        e = frm.elements[j];
        if (e.type == 'checkbox' && e.name.indexOf(chk) != -1 && !e.disabled) {
            if (!e.checked)
                flag = false;
        }
    }
    for (j = 0; j < frm.length; j++) {
        e = frm.elements[j];
        if (e.type == 'checkbox' && e.name.indexOf(chkall) != -1) {
            e.checked = flag;
            break;
        }
    }
}

function Xoa() {
    if (confirm("Bạn có chắc chắn muốn xóa không?\nCác thông tin liên quan cũng bị xóa!"))
        return true;
    else
        return false;
}

function popupWindow(url, w, h, bt) {
    var width = w;
    var height = h;
    var resizable = 'no';
    var scrollbars = 'yes';
    var left = ((document.body.clientWidth - width) / 2) + window.screenLeft;
    var top = (((document.body.clientHeight - height) / 2)) + window.screenTop;
    window.open(url, '', 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top + ',resizable=' + resizable + ',scrollbars=' + scrollbars);
    document.getElementById(bt).disabled = true;
}
function popupWindows(url, w, h) {
    if (w == 0)
        w = document.body.clientWidth - 100;
    if (h == 0)
        h = document.body.clientHeight - 200;

    var resizable = 'no';
    var scrollbars = 'yes';
    var left = ((document.body.clientWidth - w) / 2);// + window.screenLeft;
    var top = ((document.body.clientHeight - h) / 2);// + window.screenTop;
    window.open(url, '', 'width=' + w + ',height=' + h + ',left=' + left + ',top=' + top + ',resizable=' + resizable + ',scrollbars=' + scrollbars);
}
function BatTat(obj, o, i) {
    if (i == 0)
        document.getElementById(obj).disabled = o;
    else {
        window.opener.document.getElementById(obj).disabled = o;
    }
}
function IsCheckCha(ckbCha, ckbCon) {
    var f = document.forms[0];
    for (i = 0; i < f.length; i++) {
        e = f.elements[i];
        if (e.type == 'checkbox' && e.name.indexOf(ckbCha) != -1) {
            if (e.checked) {
                for (j = 0; j < f.length; j++) {
                    sub = f.elements[j];
                    if (sub.type == 'checkbox' && sub.name.indexOf(ckbCon) != -1 && sub.id != e.id)
                        sub.checked = true;
                }
            }
            else {
                for (j = 0; j < f.length; j++) {
                    sub = f.elements[j];
                    if (sub.type == 'checkbox' && sub.name.indexOf(ckbCon) != -1 && sub.id != e.id)
                        sub.checked = false;
                }
            }
        }
    }
}

function IsCheckCon(ckbCha, ckbCon) {
    var f = document.forms[0];
    var icheck = true;
    //---------------------       
    for (i = 0; i < f.length; i++) {
        e = f.elements[i];
        if (e.type == 'checkbox' && e.id.indexOf(ckbCon) != -1) {
            if (e.checked) {
                for (j = 0; j < f.length; j++) {
                    cha = f.elements[j];
                    if (cha.type == 'checkbox' && cha.id.indexOf(ckbCha) != -1) {
                        cha.checked = true;
                        //   Alert("Check cha");                       
                        break;
                    }
                }
            }
            else {
                for (j = 0; j < f.length; j++) {
                    sub = f.elements[j];
                    if (sub.type == 'checkbox' && sub.id.indexOf(ckbCon) != -1)
                        icheck = sub.checked;
                }
                for (t = 0; t < f.length; t++) {
                    b = f.elements[t];
                    if (b.type == 'checkbox' && b.id.indexOf(ckbCha) != -1 && b.id != e.id) {
                        b.checked = icheck;
                        break;
                    }
                }
            }
        }
    }
}
function KiemTraCheck_Chon() {
    var f = document.forms[0];
    var bChon = false;
    for (j = 0; j < f.length; j++) {
        ckbCheckChon = f.elements[j];
        if (ckbCheckChon.type == "checkbox") {
            if (ckbCheckChon.checked) {
                bChon = true;
                break;
            }
        }
    }
    return bChon
}
//Dùng trong lịch công tác
function Vao() {
    $(".active").attr("class", '');
}

function Ra(id) {
    var e = document.getElementById(id);
    if (e != null)
        $(e).attr("class", "active");
}

function SetCssDongChon(row) {
    var b = document.getElementById("LuuCssDongChon");
    var b1 = document.getElementById("LuuCssTruocDo");
    if (b.value != "")
        $(".Dong_Chon").attr("class", b.value);
    b.value = b1.value
    $(row).attr("class", "Dong_Chon");
}

function BoCheck() {
    var f = document.forms[0];
    for (j = 0; j < f.length; j++) {
        ckbCheckChon = f.elements[j];
        if (ckbCheckChon.type == "checkbox") {
            ckbCheckChon.checked = false;
        }
    }
}
function SetCssDongChon_Check(row, chkID) {
    var e = window.document.getElementById(chkID);
    var b = document.getElementById("LuuCssDongChon");
    var b1 = document.getElementById("LuuCssTruocDo");
    if (e != null && !e.checked) {
        BoCheck();
        e.checked = true;
        if (b.value != "")
            $(".Dong_Chon").attr("class", b.value);
        b.value = b1.value;
        $(row).attr("class", "Dong_Chon");
    }
    else {
        e.checked = false;
        if (b.value != "")
            $(".Dong_Chon").attr("class", b.value);

    }

}
function Css_Mouse_ON(row) {
    var b = document.getElementById("LuuCssTruocDo");
    b.value = row.className;
    row.className = "Dong_Chuot";
}
function Css_Mouse_OUT(row) {
    if (row.className == "Dong_Chuot") {
        var b = document.getElementById("LuuCssTruocDo");
        row.className = b.value;
    }
}

//====================
function HienThongTinCV(id, soDong) {
    var k = document.getElementById("txtSoDong").value;

    var i = 0;
    for (i = 1; i <= k; i++) {
        var txt = document.getElementById("txtID").value + i.toString();
        if (document.getElementById(txt) != null) {
            document.getElementById(txt).style.display = "none";
        }
    }
    for (i = 1; i <= soDong; i++) {
        var txt1 = id + i.toString();

        if (document.getElementById(txt1) != null) {
            document.getElementById(txt1).style.display = "";
        }
        document.getElementById("txtID").value = id;
        document.getElementById("txtSoDong").value = soDong.toString();
    }
}
function changeSource_TT(divMedia, strSource, cssClass) {
    var s = "";
    s = "<object id='oplayer' width='640' height='100' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0' classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000'>";
    s = s + "<param value='http://static.mp3.zdn.vn/skins/mp3_main/flash/zing_mp3_player_v3.1.swf' name='movie'>";
    s = s + "<param value='high' name='quality'>";
    s = s + "<param value='transparent' name='wmode'>";
    s = s + "<param value='always' name='allowscriptaccess'>";
    s = s + "<param value='true' name='allowfullscreen'>";
    s = s + "<param value='songid=0&autostart=true&xmlURL=http://mp3.zing.vn/xml/song-xml/kmJHykmazQLXZWgtLvJyvmLG&textad=http://mp3.zing.vn/xml/adv/song' name='flashvars'>";
    s = s + "<embed id='player' width='640' height='100' flashvars='songid=0&autostart=true&xmlURL=http://mp3.zing.vn/xml/song-xml/kmJHykmazQLXZWgtLvJyvmLG&textad=http://mp3.zing.vn/xml/adv/song' src='http://static.mp3.zdn.vn/skins/mp3_main/flash/zing_mp3_player_v3.1.swf' allowfullscreen='true' allowscriptaccess='always' wmode='transparent'>";
    s = s + "</object>";
    document.getElementById(divMedia).innerHTML = s;
}



function changeSource_TT123(divMedia, strSource, cssClass) {
    var s;
    s = "<object id='winMediaPlayerID' class='" + cssClass + "'";
    s += "codeBase='http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=6,4,5,715' ";
    s += "type='application/x-oleobject'";
    s += "standby='Loading Microsoft Windows Media Player components...' ";
    s += "width='100%' classid='CLSID:6BF52A52-394A-11D3-B153-00C04F79FAA6' ";
    s += "name='winMediaPlayer'>";
    s += "<param name='URL' value='" + strSource + "'/> ";
    s += "<param name='rate' value='1'/>";
    s += "<param name='balance' value='0'/>";
    s += "<param name='currentPosition' value='0'/>";
    s += "<param name='defaultFrame' value='0'/>";
    s += "<param name='playCount' value='1'/><param name='CursorType' value='-1'/>";
    s += "<param name='autoStart' value='0'/><param name='currentMarker' value='0'/><param name='invokeURLs' value='-1'/>";
    s += "<param name='volume' value='50'/>";
    s += "<param name='mute' value='0'/><param name='stretchToFit' value='-1'/>";
    s += "<param name='windowlessVideo' value='0'/><param name='enabled' value='1'/>";
    s += "<param name='fullScreen' value='0'/><param name='enableErrorDialogs' value='0'/>";
    s += "<embed class='TT_TinChiTiet_Media' type='application/x-mplayer2' pluginspage='http://www.microsoft.com/windows/windowsmedia/download/' filename='" + strSource + "' src='" + strSource + "' Name='MediaPlayerTV' ";
    s += "<\/embed> ";
    s += "<\/object>";
    document.getElementById(divMedia).innerHTML = s;
}
function changeSource(divMedia, strSource, cssClass) {
    var s;
    s = "<object id='winMediaPlayerID' class='" + cssClass + "'";
    s += "codeBase='http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=6,4,5,715' ";
    s += "type='application/x-oleobject'";
    s += "standby='Loading Microsoft Windows Media Player components...' ";
    s += "width='100%' classid='CLSID:6BF52A52-394A-11D3-B153-00C04F79FAA6' ";
    s += "name='winMediaPlayer'>";
    s += "<param name='URL' value='" + strSource + "'/> ";
    s += "<param name='rate' value='1'/>";
    s += "<param name='balance' value='0'/>";
    s += "<param name='currentPosition' value='0'/>";
    s += "<param name='defaultFrame' value='0'/>";
    s += "<param name='playCount' value='1'/><param name='CursorType' value='-1'/>";
    s += "<param name='autoStart' value='0'/><param name='currentMarker' value='0'/><param name='invokeURLs' value='-1'/>";
    s += "<param name='volume' value='50'/>";
    s += "<param name='mute' value='0'/><param name='stretchToFit' value='-1'/>";
    s += "<param name='windowlessVideo' value='0'/><param name='enabled' value='1'/>";
    s += "<param name='fullScreen' value='0'/><param name='enableErrorDialogs' value='0'/>";
    s += "<embed class='TinhKhuc_embed' type='application/x-mplayer2' pluginspage='http://www.microsoft.com/windows/windowsmedia/download/' filename='" + strSource + "' src='" + strSource + "' Name='MediaPlayerTV' ";
    s += "width='224px' ";
    s += "height='450' ";
    s += "AutoSize='1' ";
    s += "AutoStart='0' ";
    s += "ClickToPlay='0' ";
    s += "DisplaySize='1' ";
    s += "EnableContextMenu='1' ";
    s += "EnableFullScreenControls='1' ";
    s += "EnableTracker='1' ";
    s += "Mute='0' ";
    s += "PlayCount='1' ";
    s += "ShowControls='1' ";
    s += "ShowAudioControls='1' ";
    s += "ShowDisplay='0' ";
    s += "ShowGotoBar='0' ";
    s += "ShowPositionControls='1' ";
    s += "ShowStatusBar='1' ";
    s += "ShowTracker='1'> ";
    s += "<\/embed> ";
    s += "<\/object>";
    document.getElementById(divMedia).innerHTML = s;
}
//Dùng cho video media

function changeSource1(strSource) {
    var s;
    s = "<object id='winMediaPlayerID'";
    s += "codeBase='http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=6,4,5,715' ";
    s += "type='application/x-oleobject' height='400' ";
    s += "standby='Loading Microsoft Windows Media Player components...' ";
    s += "width='100%' classid='CLSID:6BF52A52-394A-11D3-B153-00C04F79FAA6' ";
    s += "name='winMediaPlayer'>";
    s += "<param name='URL' value='" + strSource + "'/> ";
    s += "<param name='rate' value='1'/>";
    s += "<param name='balance' value='0'/>";
    s += "<param name='currentPosition' value='0'/>";
    s += "<param name='defaultFrame' value='0'/>";
    s += "<param name='playCount' value='1'/><param name='CursorType' value='-1'/>";
    s += "<param name='autoStart' value='1'/><param name='currentMarker' value='0'/><param name='invokeURLs' value='-1'/>";
    s += "<param name='volume' value='50'/>";
    s += "<param name='mute' value='0'/><param name='stretchToFit' value='-1'/>";
    s += "<param name='windowlessVideo' value='0'/><param name='enabled' value='1'/>";
    s += "<param name='fullScreen' value='0'/><param name='enableErrorDialogs' value='0'/>";
    s += "<embed type='application/x-mplayer2' pluginspage='http://www.microsoft.com/windows/windowsmedia/download/' filename='" + strSource + "' src='" + strSource + "' Name='MediaPlayerTV' ";
    s += "width='440' ";
    s += "height='450' ";
    s += "AutoSize='1' ";
    s += "AutoStart='1' ";
    s += "ClickToPlay='1' ";
    s += "DisplaySize='1' ";
    s += "EnableContextMenu='1' ";
    s += "EnableFullScreenControls='1' ";
    s += "EnableTracker='1' ";
    s += "Mute='0' ";
    s += "PlayCount='1' ";
    s += "ShowControls='1' ";
    s += "ShowAudioControls='1' ";
    s += "ShowDisplay='0' ";
    s += "ShowGotoBar='0' ";
    s += "ShowPositionControls='1' ";
    s += "ShowStatusBar='1' ";
    s += "ShowTracker='1'> ";
    s += "<\/embed> ";
    s += "<\/object>";

    document.getElementById("MediaPlayer").innerHTML = s;
}
function open_window(boxmenu) {
    var url = boxmenu[boxmenu.selectedIndex].value;
    if (url != '') {
        window.open(url);
    }
}

function AnHienObj(idClick, idAnHien, cssHien, cssAn) {
    //Dùng trong liên kết webste dạng list
    var e = document.getElementById(idAnHien);
    if (e != null) {
        if (e.style.display == "none") {
            e.style.display = "";
            idClick.className = cssHien;
        }
        else {
            e.style.display = "none";
            idClick.className = cssAn;
        }
    }
}

//Kiểm tra hợp lệ ngày
function KiemTraNgayHopLe(ctrID) {
    var ok = true;
    var day;
    var month;
    var year;
    var leap = 0;
    var date;
    var ctr = document.getElementById(ctrID);

    if (ctr != null) {

        var objRegExp = /(^(([0-9])|([0-2][0-9])|(3[0-1]))\/(([1-9])|(0[1-9])|(1[0-2]))\/(([0-9][0-9])|([1-2][0,9][0-9][0-9]))$)/;

        if (objRegExp.test(ctr.value)) {

            date = ctr.value;
            var arr = [];
            arr = date.split('/');
            day = arr[0];
            month = arr[1];
            year = arr[2];
        }

        /* Validation leap-year / february / day */
        if ((year % 4 == 0) || (year % 100 == 0) || (year % 400 == 0))
            leap = 1;
        if ((month == 2) && (leap == 1) && (day > 29))
            ok = false;
        if ((month == 2) && (leap != 1) && (day > 28))
            ok = false;
        /* Validation of other months */
        if ((day > 31) && ((month == 1) || (month == 3) || (month == 5) || (month == 7) || (month == 8) || (month == 10) || (month == 12)))
            ok = false;
        if ((day > 30) && ((month == 4) || (month == 6) || (month == 9) || (month == 11)))
            ok = false;

        if (!ok || !objRegExp.test(ctr.value)) {
            alert('Ngày không hợp lệ, vui lòng nhập lại!');
            ctr.focus();
            ctr.select();
            return false;
        }
    }
    return true;
}
function btnXemTinTheoNgay_onclick(str) {
    obj = document.getElementById("txtNgayXemTin");
    sngay = "";

    if (obj != null) {
        str = str + obj.value;
        sngay = obj.value;
    }
    if (sngay == "") {
        alert("Bạn phải chọn ngày cần xem");
        return;
    }
    window.location = str;
}
function windowPopup(url, width, height) {
    if (width == 0)
        width = window.screen.availWidth - 50;
    if (height == 0)
        height = window.screen.availHeight - 100;
    var left = window.screen.availWidth / 2 - width / 2;
    var top = window.screen.availHeight / 2 - height / 2;
    var myWind = window.open(url, '', 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top + ',resizable=no,scrollbars=yes');

    myWind.document.title = 'testing';
   
    myWind.focus();
    return false;
}
function windowPopupFileDinhKem(url, id, width, height) {
    var left = window.screen.availWidth / 2 - width / 2;
    var top = window.screen.availHeight / 2 - height / 2;
    var obj = document.getElementById(id);
    if (obj != null)
        url += "?id=" + id + "&amp;file=" + obj.value;
    myWind = window.open(url, 'subWindow', 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top + ',resizable=no,scrollbars=yes');
    myWind.focus();
    return false;
}

function testNumber(obj, e) {
    var keycode = 0;
    if (window.event)
        keycode = window.event.keyCode;
    else
        keycode = e.which;
    if ((keycode < 48 || keycode > 57) && keycode != 8 && keycode != 0) {
        return false;
    }

}
function testFloat(obj, e) {
    var keycode = 0;
    if (window.event)
        keycode = window.event.keyCode;
    else
        keycode = e.which;

    var s = obj.value
    if (s == "0")
        obj.value = "";
    if (s.indexOf(",") != -1 && keycode != 8 && keycode != 0) {
        return false;
    }
    if (s == "0" && keycode == 44)
        obj.value = "0";

    if ((keycode < 48 || keycode > 57) && (keycode != 44) && keycode != 8 && keycode != 0) {
        return false;
    }
}

function testReadOnly(e) {
    return false;
}
function BoDauCham(s) {
    while (s.indexOf(".") != -1)
        s = s.replace(".", "");
    return s;
}
function ChuyenPhayCham(s) {
    s = s.replace(",", ".");
    return s;
}
function ChuyenChamPhay(s) {
    s = s.replace(".", ",");
    return s;
}

function BoDinhDangObj(txt) {
    var s = txt.value;
    while (s.indexOf(".") != -1)
        s = s.replace(".", "");
    txt.value = s;
}
function BoDinhDangStr(s) {
    while (s.indexOf(".") != -1)
        s = s.replace(".", "");
    s = s.replace(",", ".");
    return s;
}
function DinhDangCham(txt) {
    var s = txt.value;
    var thapphan = "";
    if (s.indexOf(",") != -1) {
        thapphan = s.substr(s.indexOf(","), s.length);
        s = s.substr(0, s.indexOf(","));
    }
    var k = 0;
    var len = s.length;
    for (var i = len; i > 0; i--) {
        k = k + 1;
        if (k == 3 && i != 1) {
            s = s.substr(0, i - 1) + "." + s.substr(i - 1, s.length)
            k = 0;
        }
    }
    txt.value = s + thapphan;
}
function NhanEnter(btn, txt) {
    var e1 = document.getElementById(btn);
    $('#' + txt).bind("keydown", function (e) {
        if (e.which == 13 || window.event.keyCode == 13) { //Enter key
            e1.focus();
            //e1.preventDefault(); //to skip default behavior of the enter key
            $('#' + btn).click();
        }
    });
}

function TimMaHoSo(btn, txt) {
    var e1 = document.getElementById(txt);
    $('#' + txt).bind("keyup", function (e) {
        if (e1.value.length == 13) { //Enter key
            $('#' + btn).click();
            //alert('aaa');
        }
    });

}

function TimTaiKhoan(btn, txt) {
    var e1 = document.getElementById(txt);
    $('#' + txt).bind("keyup", function (e) {
        if (e1.value.indexOf("#") != -1) { //Enter key
            $('#' + btn).click();
            //alert('aaa');
        }
    });

}


function KiemTraCheck() {
    var f = document.forms[0];
    var bChon = false;
    for (j = 0; j < f.length; j++) {
        ckbCheckChon = f.elements[j];
        if (ckbCheckChon.type == "checkbox") {
            if (ckbCheckChon.checked) {
                bChon = true;
                break;
            }
        }
    }
    if (bChon == false) {
        alert("Bạn chưa chọn thông tin để xóa");
        return false;
    }
    else {
        return confirm("Bạn có chắc chắn muốn xóa các thông tin đã chọn không?");
    }
}
function isValidDate(str) {
    var datePattern = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;

    var patt = new RegExp(datePattern);
    var res = patt.test(str);
    if (!res) return false;
    else return true;
}
