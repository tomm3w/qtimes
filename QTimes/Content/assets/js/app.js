$(function () {

    $('.sidebar-toggle').click(function () {
        $('.content-wrapper').toggleClass('in');
        $('#sidebar').toggleClass('out');
    });

    $('.message-link').click(function () {
        $('.chat-overlay, .chat-box').fadeIn();
    });
    $('.chat-close').click(function () {
        $('.chat-overlay, .chat-box').fadeOut();
    });

    //$('.poplink').click(function () {
    //    $('.popup-overlay, .popup-box').fadeIn();
    //});
    //$('.popup-close').click(function () {
    //    $('.popup-overlay, .popup-box').fadeOut();
    //});

    $('.add_staff .right_coner li').click(function () {
        $('.add_staff .right_coner li.active').removeClass('active');
        $(this).addClass('active');
        $('.add_staff .input_chk').removeClass('tick');
    });
    $('.checkbox').click(function () {
        $(this).toggleClass('on');
    });
    $('.setlabel').click(function () {
        $(this).toggleClass('on');
    });

    $('.navigation_lft li').click(function () {
        $('.navigation_lft li.active').removeClass('active');
        $(this).addClass('active');
    });

    $('.sidebar-menu_li').click(function () {
        $('.sidebar-menu_li.active').removeClass('active');
        $(this).addClass('active');
        $('.sidebar-menu_li.active').find('span').removeClass('show');
        $('.sidebar-menu_li.active').find('span').addClass('show');
    });

});


function HideLoader() {
    $('.loader').addClass('hidden');
}

function ShowLoader() {
    $('.loader').removeClass('hidden');
}

$(document).ready(function () {
    $(".con_p").click(function () {
        //$(this)
        //    .parents("li")
        //    .find(".rt_doc")
        //    .toggleClass("activation");
        if ($(this).find('input[type="checkbox"]').is(":checked"))
            $(this).parents("li").find(".rt_doc").addClass("activation");
        else
            $(this).parents("li").find(".rt_doc").removeClass("activation");

    });
});

$(function () {

    $(".navbar ul li").click(function () {
        $(".navbar ul li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".nav-link").click(function () {
        var hreflink = $(this).attr("href");
        console.log(hreflink);
        $("html, body").animate(
            {
                scrollTop: $(hreflink).offset().top - 100,
            },
            1000
        );
        $('.navbar').removeClass('show_navi');
    });
    $('.nav-tog_btn').on('click', function () {
        $(this).next('.navbar').toggleClass('show_navi');
        $('.updated_hot').toggleClass('on');
    });
    $(".seatQ_link ul li").click(function () {
        $(".seatQ_link ul li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".seatQ_sign ul li").click(function () {
        $(".seatQ_sign ul li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec1_txt_ul li").click(function () {
        $(".sec1_txt_ul li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec2_txt_ul li").click(function () {
        $(".sec2_txt_ul li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec4_txt_ul li").click(function () {
        $(".sec4_txt_ul li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec5_txt_ul li").click(function () {
        $(".sec5_txt_ul li.active").removeClass("active");
        $(this).addClass("active");
    });

    $(".orange--btn").click(function () {
        $(".orange--btn.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec7_txt_ul_1 li").click(function () {
        $(".sec7_txt_ul_1 li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec7_txt_ul_2 li").click(function () {
        $(".sec7_txt_ul_2 li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec7_txt_ul_3 li").click(function () {
        $(".sec7_txt_ul_3 li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".sec7_txt_ul_4 li").click(function () {
        $(".sec7_txt_ul_4 li.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".btn_submit").click(function () {
        $(".btn_submit.active").removeClass("active");
        $(this).addClass("active");
    });
    $(".footer_terms").click(function () {
        $(".footer_terms.active").removeClass("active");
        $(this).addClass("active");
    });

    $(".items_menu li").click(function () {
        $(".items_menu li.active").removeClass("active");
        $(this).addClass("active");
    });

    $(document).ready(function () {
        $("a.screenOne").click(function (e) {
            e.preventDefault();
            $("#over_lay1").css({
                opacity: 1,
            });
            $("#over_lay2").css({
                opacity: 0,
            });
            $("#over_lay3").css({
                opacity: 0,
            });
        });
        $("a.screenTwo").click(function (e) {
            e.preventDefault();
            console.log("clicked");
            $("#over_lay1").css({
                opacity: 0,
            });
            $("#over_lay2").css({
                opacity: 1,
            });
            $("#over_lay3").css({
                opacity: 0,
            });
        });
        $("a.screenThree").click(function (e) {
            e.preventDefault();
            $("#over_lay1").css({
                opacity: 0,
            });
            $("#over_lay2").css({
                opacity: 0,
            });
            $("#over_lay3").css({
                opacity: 1,
            });
        });
    });

});