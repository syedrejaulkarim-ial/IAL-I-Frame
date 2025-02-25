$(function() {

    //owl Carousel

    //Banner Image
    $("#owl-carousel-banner").owlCarousel({
        loop: true,
        dots: false,
        nav: true,
        // center: true,
        items: 1,
        animate: true,
        // animateOut: 'slideOutDown',
        animateIn: 'flipInX',
        margin: 0,
        stagePadding: 0,
        smartSpeed: 450,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        lazyLoad: true
    });
    var owl = $("#owl-carousel-banner");
    owl.owlCarousel();
    $(".next-btn").click(function() {
        owl.trigger("next.owl.carousel");
    });
    $(".prev-btn").click(function() {
        owl.trigger("prev.owl.carousel");
    });
    $(".prev-btn").addClass("disabled");
    $(owl).on("translated.owl.carousel", function(event) {
        if ($(".owl-prev").hasClass("disabled")) {
            $(".prev-btn").addClass("disabled");
        } else {
            $(".prev-btn").removeClass("disabled");
        }
        if ($(".owl-next").hasClass("disabled")) {
            $(".next-btn").addClass("disabled");
        } else {
            $(".next-btn").removeClass("disabled");
        }
    });

    //Banner Text
    $("#owl-carousel-text").owlCarousel({
        loop: true,
        dots: true,
        nav: false,
        items: 1,
        animate: true,
        // animateOut: 'slideOutDown',
        animateIn: 'flipInY',
        margin: 30,
        stagePadding: 30,
        smartSpeed: 450,
        lazyLoad: true
    });
    var owl = $("#owl-carousel-text");
    owl.owlCarousel();
    $(".next-btn").click(function() {
        owl.trigger("next.owl.carousel");
    });
    $(".prev-btn").click(function() {
        owl.trigger("prev.owl.carousel");
    });
    $(".prev-btn").addClass("disabled");
    $(owl).on("translated.owl.carousel", function(event) {
        if ($(".owl-prev").hasClass("disabled")) {
            $(".prev-btn").addClass("disabled");
        } else {
            $(".prev-btn").removeClass("disabled");
        }
        if ($(".owl-next").hasClass("disabled")) {
            $(".next-btn").addClass("disabled");
        } else {
            $(".next-btn").removeClass("disabled");
        }
    });

    //FM Slider
    $("#owl-carousel-fm").owlCarousel({
        loop: true,
        dots: true,
        nav: false,
        items: 1,
        animate: true,
        // animateOut: 'slideOutDown',
        animateIn: 'flipInY',
        margin: 30,
        stagePadding: 30,
        smartSpeed: 450,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        lazyLoad: true
    });
    var owl = $("#owl-carousel-fm");
    owl.owlCarousel();
    $(".next-btn").click(function() {
        owl.trigger("next.owl.carousel");
    });
    $(".prev-btn").click(function() {
        owl.trigger("prev.owl.carousel");
    });
    $(".prev-btn").addClass("disabled");
    $(owl).on("translated.owl.carousel", function(event) {
        if ($(".owl-prev").hasClass("disabled")) {
            $(".prev-btn").addClass("disabled");
        } else {
            $(".prev-btn").removeClass("disabled");
        }
        if ($(".owl-next").hasClass("disabled")) {
            $(".next-btn").addClass("disabled");
        } else {
            $(".next-btn").removeClass("disabled");
        }
    });


    //Tab Menu Slider
    $(".owl-carousel-tabmenu").owlCarousel({
        loop: false,
        dots: false,
        nav: false,
        items: 10,
        animate: true,
        // animateOut: 'slideOutDown',
        animateIn: 'flipInY',
        margin: 0,
        stagePadding: 0,
        smartSpeed: 450,
        autoplay: false,
        autoplayTimeout: 10000,
        autoplayHoverPause: false,
        lazyLoad: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 3
            },
            1000: {
                items: 5
            }
        }
    });
    var owl = $(".owl-carousel-tabmenu");
    owl.owlCarousel();
    $(".next-btn").click(function() {
        owl.trigger("next.owl.carousel");
    });
    $(".prev-btn").click(function() {
        owl.trigger("prev.owl.carousel");
    });
    $(".prev-btn").addClass("disabled");
    $(owl).on("translated.owl.carousel", function(event) {
        if ($(".owl-prev").hasClass("disabled")) {
            $(".prev-btn").addClass("disabled");
        } else {
            $(".prev-btn").removeClass("disabled");
        }
        if ($(".owl-next").hasClass("disabled")) {
            $(".next-btn").addClass("disabled");
        } else {
            $(".next-btn").removeClass("disabled");
        }
    });

    //Product Card Slider
    $(".owl-carousel-prdt-card").owlCarousel({
        loop: false,
        dots: false,
        nav: false,
        items: 5,
        animate: true,
        animateOut: 'slideOutDown',
        // animateIn: 'flipInY',
        margin: 15,
        stagePadding: 15,
        smartSpeed: 450,
        autoplay: false,
        autoplayTimeout: 10000,
        autoplayHoverPause: false,
        lazyLoad: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 4
            }
        }
    });
    var owl = $(".owl-carousel-prdt-card");
    owl.owlCarousel();
    $(".next-btn").click(function() {
        owl.trigger("next.owl.carousel");
    });
    $(".prev-btn").click(function() {
        owl.trigger("prev.owl.carousel");
    });
    $(".prev-btn").addClass("disabled");
    $(owl).on("translated.owl.carousel", function(event) {
        if ($(".owl-prev").hasClass("disabled")) {
            $(".prev-btn").addClass("disabled");
        } else {
            $(".prev-btn").removeClass("disabled");
        }
        if ($(".owl-next").hasClass("disabled")) {
            $(".next-btn").addClass("disabled");
        } else {
            $(".next-btn").removeClass("disabled");
        }
    });

    //IE Listen Slider

    //Banner Text
    $(".owl-carousel-watch-listen-read").owlCarousel({
        loop: true,
        dots: false,
        nav: true,
        items: 1,
        animate: true,
        // animateOut: 'slideOutDown',
        animateIn: 'flipInY',
        margin: 30,
        stagePadding: 10,
        smartSpeed: 450,
        lazyLoad: true
    });
    var owl = $(".owl-carousel-watch-listen-read");
    owl.owlCarousel();
    $(".next-btn").click(function() {
        owl.trigger("next.owl.carousel");
    });
    $(".prev-btn").click(function() {
        owl.trigger("prev.owl.carousel");
    });
    $(".prev-btn").addClass("disabled");
    $(owl).on("translated.owl.carousel", function(event) {
        if ($(".owl-prev").hasClass("disabled")) {
            $(".prev-btn").addClass("disabled");
        } else {
            $(".prev-btn").removeClass("disabled");
        }
        if ($(".owl-next").hasClass("disabled")) {
            $(".next-btn").addClass("disabled");
        } else {
            $(".next-btn").removeClass("disabled");
        }
    });


    //Select2
    // $(".select2").select2();

    $('.select2').select2({
        theme: 'bootstrap-5'
    });

    // Datepicker
    $('.datepicker').datepicker({
        autoclose: true,
        format: 'dd-m-yyyy'
    });
    $('#SIPStrtDate').datepicker({
        autoclose: true,
        format: 'dd-M-yyyy',
        startDate: '+10'
    });

    //Page Scroll
    $('a.page-scroll').bind('click', function(event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top - 190,
        }, 500, 'linear');
        event.preventDefault();
    });

    //CustomeTabs
    // [].slice.call(document.querySelectorAll('.tabs')).forEach(function(el) {
    //     new CustomeTabs(el);
    // });
});