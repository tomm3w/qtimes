$.ajaxSetup({
    beforeSend: function (xhr) {
        xhr.setRequestHeader('Authorization', 'Bearer ' + $.cookie("token"));
    }
});
var limitSize = 0;

function LoadSummary() {
    var businessId = $.cookie("businessid");
    if (!businessId || window.location.href.indexOf("admin/accounts") > -1) return false;

    $.ajax({
        type: 'POST',
        url: webserviceURL + "reservation/getsummary",
        data: { Date: moment().format('YYYY-MM-DD'), BusinessDetailId: businessId, CurrentTime: moment().format('HH:mm:ss') },
        success: function (data) {
            $('#businessName').text(data.data.BusinessName);
            $('#totalSpots').text(data.data.TotalSpots);
            $('#available').text(data.data.Available);
            $('#waiting').text(data.data.Waiting);
            $('#inPlace').text(data.data.InPlace);
            limitSize = data.data.LimitSize;

            if (!data.data.EnableGroupSize)
                Notify("Warning: Limit Size Per Group is not enabled.");

            if (data.data.MaxGroupSize === 0)
                Notify("Warning: Limit Size Per Group is not defined.");

            $('.notifyjs-bootstrap-warn').parents('.notifyjs-corner').css('width', '100%').css('text-align', 'center');

        },
        error: function (xhr, status, error) {
            $('.loading-screen').removeClass('show');
            ShowErrorMessage(xhr, status, error);
        }
    });
}

LoadSummary();

(function () {
    function checkTime(i) {
        return (i < 10) ? "0" + i : i;
    }
    function startTime() {
        if (document.getElementById('time')) {
            var today = new Date(),
                h = checkTime(today.getHours()),
                m = checkTime(today.getMinutes()),
                s = checkTime(today.getSeconds());
            var ampm = h >= 12 ? 'pm' : 'am';
            h = h % 12;
            h = h ? h : 12;
            document.getElementById('time').innerHTML = '<span class="time-icon"></span> ' + h + ":" + m + ":" + s + ' ' + ampm;
            t = setTimeout(function () {
                startTime()
            }, 1000);
        }
    }
    startTime();
})();