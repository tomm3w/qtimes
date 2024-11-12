$('body').addClass('waittime_reserve_part intro_body splash_part');
$.ajax({
    type: 'POST',
    url: webserviceURL + "reservation/business/" + id,
    data: { Id: id, Date: moment().format('YYYY-MM-DD') },
    success: function (data) {
        $('.businessName').text(data.data.BusinessName);
        $('.businessDescription').text(data.data.Description);
        $('#businessAddress').text(data.data.FullAddress);
        $('#logo').attr('src', data.data.LogoFullPath);

        if (data.data.EnablePrivacyPolicy && data.data.PrivacyPolicyFilePath)
            $('#privayPolicy').attr('href', data.data.PrivacyPolicyFileFullPath);
        else
            $('#privayPolicy').remove();

        if (data.data.EnableServiceTerms && data.data.ServiceTermsFilePath)
            $('#termsService').attr('href', data.data.ServiceTermsFileFullPath);
        else
            $('#termsService').remove();

        if (data.data.EnableCommunityGuidelines && data.data.CummunityGuidelinesFilePath)
            $('#communityGuidelines').attr('href', data.data.CummunityGuidelinesFileFullPath);
        else
            $('#communityGuidelines').remove();
    },
    error: function (xhr, status, error) {
        ShowErrorMessage(xhr, status, error);
    }
});