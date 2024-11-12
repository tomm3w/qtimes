$('body').addClass('waitime2');
$.ajax({
    type: 'GET',
    url: webserviceURL + "reservation/business/" + id,
    contentType: "application/json; charset=utf-8",
    success: function (data) {
        $('#businessName').text(data.data.BusinessName);
        $('#businessAddress').text(data.data.FullAddress);
        $('#logo').attr('src', data.data.LogoFullPath);
    },
    error: function (xhr, status, error) {
        ShowErrorMessage(xhr, status, error);
    }
});