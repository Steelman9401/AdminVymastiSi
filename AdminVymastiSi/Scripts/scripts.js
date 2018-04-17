﻿//function playVideo(e) {
//    if (window.innerWidth > 768) {
//        e.load();
//        e.play();
//    }
//}

//function stopVideo(e) {
//    if(window.innerWidth > 768) {
//        e.load();
//    }
//}



$(document).ready(function () {
    $(document).on("click", ".video", function (e) {
        $('#myModal').modal({
            backdrop: 'static',
            keyboard: false
        })
    });


    $(document).on("click", ".source-selector", function (e) {
        $('#source-selection').collapse("toggle")
    });

    $(document).on("click", ".click", function (e) {
        e.currentTarget.getElementsByTagName("button")[0].click();
        if ($(e.currentTarget).hasClass("remove-loader-modal")) {
            $("#loader-modal").css("display", "none");
        }
        if ($(e.currentTarget).hasClass("remove-loader-main")) {
            $("#loader-main").css("display", "none");
        }

    });


    $(document).on("click", "#spider-page", function (e) {
        $('#navbarSupportedContent').collapse("hide");
        $("#spider-page").css("font-weight", "bold");
        $("#database-page").css("font-weight", "normal");
        $("#category-page").css("font-weight", "normal");
    });

    $(document).on("click", "#database-page", function (e) {
        $('#navbarSupportedContent').collapse("hide");
        $("#database-page").css("font-weight", "bold");
        $("#spider-page").css("font-weight", "normal");
        $("#category-page").css("font-weight", "normal");
    });

    $(document).on("click", "#category-page", function (e) {
        $('#navbarSupportedContent').collapse("hide");
        $("#database-page").css("font-weight", "normal");
        $("#spider-page").css("font-weight", "normal");
        $("#category-page").css("font-weight", "bold");
    });


    $(document).on("click", ".close-modal", function (e) {

        $('#myModal').modal('hide');

    });

    $(document).on("click", ".website-collapse", function (e) {
        $('#collapse1').collapse("hide");
    });

    $(document).on("click", ".move-up-modal", function (e) {
        $("#myModal").scrollTop(100);
    });

    $(document).on("click", ".open-modal", function (e) {
        $('#myModal').modal({
            backdrop: 'static',
            keyboard: false
        })
    });


    var didScroll;
    var lastScrollTop = 0;
    var delta = 5;
    var navbarHeight = $('header').outerHeight();

    $(window).scroll(function (event) {
        didScroll = true;
    });

    setInterval(function () {
        if (didScroll) {
            hasScrolled();
            didScroll = false;
        }
    }, 250);

    function hasScrolled() {
        var st = $(this).scrollTop();

        // Make sure they scroll more than delta
        if (Math.abs(lastScrollTop - st) <= delta)
            return;

        // If they scrolled down and are past the navbar, add class .nav-up.
        // This is necessary so you never see what is "behind" the navbar.
        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down
            $('header').removeClass('nav-down').addClass('nav-up');
        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {
                $('header').removeClass('nav-up').addClass('nav-down');
            }
        }

        lastScrollTop = st;
    }
   
});