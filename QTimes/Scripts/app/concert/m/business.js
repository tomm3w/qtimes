﻿$('body').addClass('waittime_reserve_part intro_body splash_part');
$('.waittime').removeClass('waittime').addClass('waittime_reserve');

$.ajax({
    type: 'GET',
    url: webserviceURL + "concertreservation/concert/" + id,
    contentType: "application/json; charset=utf-8",
    success: function (data) {
        $('.businessName').text(data.data.Name);
        $('#businessDesc').text(data.data.Description);
        $('#logo').attr('src', data.data.ImageFullPath);

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