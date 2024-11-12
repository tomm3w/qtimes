$.ajaxSetup({
    beforeSend: function (xhr) {
        xhr.setRequestHeader('Authorization', 'Bearer ' + $.cookie("token"));
    }
});

var data, url;
function RefreshSummary() {

    var qs = '';
    var id = $.cookie("concertid");
    var eventId = $.cookie("eventid");
    if (eventId != undefined) {
        qs = `?eventId=${eventId}`
    }

    url = webserviceURL + `concerts/${id}/summary${qs}`;
    data = { ConcertId: $.cookie("concertid") };

    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: function (data) {
            $('#businessName').text(data.data.BusinessName);
            $('#totalSpots').text(data.data.TotalSpots);
            $('#available').text(data.data.Available);
            $('#waiting').text(data.data.Waiting);
            $('#inPlace').text(data.data.InPlace);

            if (!data.data.EnableGroupSize)
                Notify("Warning: Group Size is not enabled.");

            if (data.data.MaxGroupSize === 0)
                Notify("Warning: Max Group Size is not defined.");

            if (data.data.ConcertEventSeatingCount === 0)
                Notify("Warning: Seatings are not added.");

            $('.notifyjs-bootstrap-warn').parents('.notifyjs-corner').css('width', '100%').css('text-align', 'center');
        },
        error: function (xhr, status, error) {
            $('.loading-screen').removeClass('show');
            ShowErrorMessage(xhr, status, error);
        }
    });

}

RefreshSummary();

(function () {
    function checkTime(i) {
        return (i < 10) ? "0" + i : i;
    }
    function startTime() {
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
    startTime();
})();