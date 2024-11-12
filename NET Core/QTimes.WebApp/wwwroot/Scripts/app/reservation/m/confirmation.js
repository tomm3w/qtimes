
$.ajax({
    type: 'POST',
    url: webserviceURL + "reservation/business/" + businessId,
    data: { Id: businessId, Date: moment().format('YYYY-MM-DD') },
    success: function (data) {

        loadQrCode(passUrl);

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

function loadQrCode(data) {
    if (data) {
        try {
            let canvas = document.createElement('canvas');
            bwipjs.toCanvas(canvas, {
                bcid: 'qrcode',
                text: data,
                scale: 5
            });
            document.getElementById("qrcode").src = canvas.toDataURL('image/png');
        } catch (e) { }
    }
    else {
        document.getElementById("qrcode").src = '/images/noimage.png';
    }
}