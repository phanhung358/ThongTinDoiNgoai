    $(function() {

        var appendthis = ("<div class='modal-overlay js-modal-close'></div>");

        $('[data-modal-id]').click(function(e) {
            e.preventDefault();
            $("body").append(appendthis);
            $(".modal-overlay").fadeTo(500, 0.7);
            //$(".js-modalbox").fadeIn(500);
            var modalBox = $(this).attr('data-modal-id');
            $('#' + modalBox).fadeIn($(this).data());
        });

        $(".js-modal-close, .modal-overlay").click(function() {
            $(".modal-box, .modal-overlay").fadeOut(500, function() {
                $(".modal-overlay").remove();
            });
        });

        $(window).resize(function() {
            $(".modal-box").css({
                //top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
                //left: ($(window).width() - $(".modal-box").outerWidth()) / 2
                top: ($(window).height())/10,
                left: ($(window).width() - $(".modal-box").outerWidth()) / 2
            });
        });
        $(window).resize();
    });
    /**/
    var _gaq = _gaq || [];
    _gaq.push(['_setAccount', 'UA-36251023-1']);
    _gaq.push(['_setDomainName', 'jqueryscript.net']);
    _gaq.push(['_trackPageview']);

    (function() {
        var ga = document.createElement('script');
        ga.type = 'text/javascript';
        ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0];
        s.parentNode.insertBefore(ga, s);
    })();