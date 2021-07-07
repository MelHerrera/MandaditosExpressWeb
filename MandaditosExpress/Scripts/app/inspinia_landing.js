// INSPINIA Landing Page Custom scripts
$(document).ready(function () {

    $('body').scrollspy({
        target: '.navbar-fixed-top',
        offset: 80
    });

    // Page scrolling feature .slice(1) to quit / added by ASP
    $('a.page-scroll').bind('click', function (event) {
        var link = $(this);
        $('html, body').stop().animate({
            scrollTop: $(link.attr('href').slice(1)).offset().top - 50
        }, 500);
        event.preventDefault();
        $('#navbar').collapse('hide');
    });
});

// Activate WOW.js plugin for animation on scrol
new WOW().init();

var cbpAnimatedHeader = (function () {
    var docElem = document.documentElement,
        header = document.querySelector('.navbar-default'),
        didScroll = false,
        changeHeaderOn = 200;
    function init() {
        window.addEventListener('scroll', function (event) {
            if (!didScroll) {
                didScroll = true;
                setTimeout(scrollPage, 250);
            }
        }, false);
    }
    function scrollPage() {
        var sy = scrollY();
        if (sy >= changeHeaderOn) {
            $(header).addClass('navbar-scroll')
        }
        else {
            $(header).removeClass('navbar-scroll')
        }
        didScroll = false;
    }
    function scrollY() {
        return window.pageYOffset || docElem.scrollTop;
    }
    init();
})();