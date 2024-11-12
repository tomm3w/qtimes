$('body').addClass('waittime_reserve_part intro_body');
$.ajax({
    type: 'POST',
    url: webserviceURL + "reservation/business/" + id,
    data: { Id: id, Date: moment().format('YYYY-MM-DD') },
    success: function (data) {
        $('.businessName').text(data.data.BusinessName);
        $('#businessAddress').text(data.data.FullAddress);
        $('#logo').attr('src', data.data.LogoFullPath);
    },
    error: function (xhr, status, error) {
        ShowErrorMessage(xhr, status, error);
    }
});