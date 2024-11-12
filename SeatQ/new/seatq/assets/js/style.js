// $(function () {
  $(document).ready(function () {
  $(window).on("load", function () {
    $(".loader").fadeOut(1500, function () {
      $(".loader").fadeIn().addClass("hidden");
    });
  });
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
  $('.nav-tog_btn').on('click', function() {
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
