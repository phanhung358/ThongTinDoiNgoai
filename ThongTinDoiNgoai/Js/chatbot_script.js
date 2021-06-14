$(document).ready(function () {
    var arrow = $('.chat-head img');
    var textarea = $('.chat-text textarea');
    var chatContent = $('.chat-content');

    var chatIconUp = $('.chat-icon-up');//add
    chatIconUp.on('click', function () {
        chatContent.slideToggle('fast');

        if ($('.msg-receive').length <= 1)
            $('.msg-receive').first().show();
        chatIconUp.hide();
        textarea.css("width", "300px");
        arrow.attr('title', 'Đóng');
    });

    arrow.on('click', function () {
        var src = arrow.attr('src');
        chatContent.slideToggle('fast');
        arrow.attr('title', 'Mở');
        chatIconUp.show();
        //https://maxcdn.icons8.com/windows10/PNG/16/Arrows/angle_down-16.png
    });


    //--- thẻ SPAN button postback click
    $(document).on('click', '.msg-button-postback', function () {
        var text = $(this).text();
        var questionText = $(this).attr('postback-data');

        $('.msg-insert').append("<div class='msg-send'>" + text + "</div>");
        callApi(questionText);
    });


    textarea.keypress(function (event) {
        var $this = $(this);

        if (event.keyCode == 13) {
            var questionText = $this.val();
            console.log('Cau Hoi = ' + questionText);
            if (arrow.attr('title') != 'Đóng') {
                chatContent.slideToggle('fast');
                chatIconUp.hide();//add
                textarea.css("width", "300px");//
            }
            arrow.attr('title', 'Đóng');
            //$('.msg-insert').prepend("<div class='msg-send'>" + msg + "</div>");
            $('.msg-insert').append("<div class='msg-send'>" + questionText + "</div>");

            //send & get data from server
            callApi(questionText);

            //always scroll to bottom
            scrollToBottom();

            //reset ô text và return false để không thêm ký tự 'enter' vào ô text
            $this.val('');
            return false;
        }

    });

    function callApi(questionText) {

        var domain = getUrl(); //"http://localhost:1433"; //
        var jqxhr = $.get(domain + "/api/chatbot/getanswer?q=" + questionText, function () { })
               .done(function (data) {
                   //var obj = jQuery.parseJSON(data);

                   var listAnswer = data.answers;

                   trickDelayAddItem(listAnswer, 1200); // --delay 1200ms

               })
               .fail(function (e) {
                   alert("error" + e);
               });
    }

    function trickDelayAddItem(listAnswer, miliseconds) {
        var index = 0;
        var length = listAnswer.length;

        while (index < length) {
            setTimeout(function () {
                var viTriHienTai = index - length;

                //...CALL FUNCTION HERERE
                var answerItem = listAnswer[viTriHienTai];
                addItem(answerItem);
                //
                index++;
            }, index * miliseconds);

            index++;
        }
    }

    function addItem(answerItem) {
        var typeAnswerItem = answerItem.type;
        console.log(typeAnswerItem);

        if (typeAnswerItem == 'text') {
            var contentAnswerItem = answerItem.text;

            //add tag <a>
            addTagLink(contentAnswerItem);

            //always scroll to bottom
            scrollToBottom();
        }
        else if (typeAnswerItem == 'attachments') {
            //-------add mô tả
            var contentAnswerItem = answerItem.text;
            //console.log(typeof contentAnswerItem);
            //console.log(contentAnswerItem.length);

            //add tag <a>
            if (!isEmpty(contentAnswerItem))
                addTagLink(contentAnswerItem);

            //always scroll to bottom
            scrollToBottom();

            //--------
            var listAttachment = answerItem.attachments;
            $.each(listAttachment, function (index, attachmentItem) {

                var imgTitle = attachmentItem.title;
                var imgSubTitle = attachmentItem.subtitle;
                var imgUrl = attachmentItem.image_url;
                var lstButton = attachmentItem.buttons;

                var allButton = '';

                $.each(lstButton, function (index, buttonItem) {
                    var postbackType = buttonItem.type;
                    var payload = jQuery.parseJSON(buttonItem.payload);
                    var payloadContent = payload.content;
                    //console.log(payload.content);
                    //sự kiện postback và add payloadContent vào button
                    if (postbackType == 'postback')
                        allButton += '<span class="msg-attachments-button msg-button-postback" postback-data="' + payloadContent + '">' + buttonItem.title + '</span>';
                    else
                        allButton += '<span class="msg-attachments-button">' + buttonItem.title + '</span>';

                });
                //add phần tử 
                var pImg = '';
                if (!isEmpty(imgUrl))
                    pImg = '<img class="msg-attachments-img" src="' + imgUrl + '">';

                var pTitle = ''
                if (!isEmpty(imgTitle))
                    pTitle = '<span class="msg-attachments-title">' + imgTitle + '</span>'

                var pSubTitle = ''
                if (!isEmpty(imgSubTitle))
                    pSubTitle = '<span class="msg-attachments-subtitle">' + imgSubTitle + '</span>'

                //div tổng
                var appendDIV = '<div class="msg-attachments">' + pImg + pTitle + pSubTitle + allButton + '</div>';

                //insert message tag
                $('.msg-insert').append(appendDIV);

                //always scroll to bottom
                scrollToBottom();
            });
        }
    }

    function addTagLink(contentAnswerItem) {
        //pattern url
        var pattern = /(http|https|ftp|ftps)\:\/\/[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(\/[^ );]*)?/gm;

        //xử lý add tag <a> link
        var listMatch = contentAnswerItem.match(pattern);
        if (listMatch != null && listMatch.length > 0) {
            $.each(listMatch, function (index, value) {
                contentAnswerItem = contentAnswerItem.replace(value, '<a class="msg-link" target="_blank" href="' + value + '">' + value + '</a> ');

            });
        }
        //insert message tag
        $('.msg-insert').append("<div class='msg-receive'>" + contentAnswerItem + "</div>");
    }

    function scrollToBottom() {
        $(".chat-body").stop().animate({
            scrollTop: $(".chat-body")[0].scrollHeight
        }, 1000);
    }

    function isEmpty(value) {
        return (value == null || value.length === 0);
    }

});