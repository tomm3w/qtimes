$(function () {
  //mobile device menu
  $(".sidebar-toggle").click(function () {
    $(".content-wrapper").toggleClass("in");
    $("#sidebar").toggleClass("out");
  });

  $(".message-link").click(function () {
    $(".chat-overlay, .chat-box").fadeIn();
  });
  $(".chat-close").click(function () {
    $(".chat-overlay, .chat-box").fadeOut();
  });
  $(".add_staff .right_coner li").click(function () {
    $(".add_staff .right_coner li.active").removeClass("active");
    $(this).addClass("active");
    $(".add_staff .input_chk").removeClass("tick");
  });
  $(".checkbox").click(function () {
    $(this).toggleClass("on");
  });
  $(".setlabel").click(function () {
    $(this).toggleClass("on");
  });

  $(".navigation_lft li").click(function () {
    $(".navigation_lft li.active").removeClass("active");
    $(this).addClass("active");
  });

  $(".sidebar-menu_li").click(function () {
    $(".sidebar-menu_li.active").removeClass("active");
    $(this).addClass("active");
    $(".sidebar-menu_li.active").find("span").removeClass("show");
    $(".sidebar-menu_li.active").find("span").addClass("show");
  });
});

$(document).ready(function () {
  $("#datepicker")
    .datepicker({
      autoclose: true,
      todayHighlight: true,
    })
    .datepicker("update", new Date());

  $(".date_picker_show").click(function () {
    $(".datepicker-days").toggleClass("open");
  });
});

$(window).on("load", function () {
  $(".loader").fadeOut(1500, function () {
    $(".loader").fadeIn().addClass("hidden");
  });
});
$(document).ready(function () {
  $(".con_p").click(function () {
      $(this)
      .parents("li")
      .find(".rt_doc")
      .toggleClass("activation");
  });
});

