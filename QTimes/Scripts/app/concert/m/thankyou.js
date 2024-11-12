$('body').addClass('thakyou2');
$('body').find('.waittime').removeClass('waittime').addClass('thankyou');

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

let passUrl = getCookie('pass');

if (passUrl) {
    $('#pass').attr('href', passUrl);
    $('#pass').text(passUrl);
}
