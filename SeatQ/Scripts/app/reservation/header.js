$.ajaxSetup({
    beforeSend: function (xhr) {
        xhr.setRequestHeader('Authorization', 'Bearer ' + $.cookie("token"));
    }
});

$.ajax({
    type: 'POST',
    url: webserviceURL + "reservation/getsummary",
    data: { Date: moment().format('YYYY-MM-DD') },
    success: function (data) {
        $('#businessName').text(data.data.BusinessName);
        $('#totalSpots').text(data.data.TotalSpots);
        $('#available').text(data.data.Available);
        $('#waiting').text(data.data.Waiting);
        $('#inPlace').text(data.data.InPlace);
    },
    error: function (xhr, status, error) {
        $('.loading-screen').removeClass('show');
        ShowErrorMessage(xhr, status, error);
    }
});

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