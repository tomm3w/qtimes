$('body').addClass('metrics_body concert__event concert_mertics_body');
$('.metrics-icon').parent().parent().addClass('active');
$('.metrics-icon').addClass('show');

var metricsViewModel = new MetricsViewModel();
ko.applyBindings(metricsViewModel);

function MetricsViewModel() {
    var self = this;

    self.Date = ko.observable(moment().format('YYYY-MM-DD'));
    self.TotalGuests = ko.observable();
    self.TotalEvents = ko.observable();
    self.TotalReservations = ko.observable();
    self.ConcertId = ko.computed(function () {
        return $.cookie("concertid");
    });

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'POST',
            url: webserviceURL + "concertreservation/metrics",
            data: { Date: self.Date(), ConcertId: self.ConcertId() },
            success: function (data) {
                self.TotalGuests(parseInt(data.data.TotalGuests));
                self.TotalEvents(data.data.TotalEvents);
                self.TotalReservations(data.data.TotalReservations);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.OnChangeDate = function () {
        var date = $('#select_onday').val();
        self.Date(moment().subtract(date, 'days').format('YYYY-MM-DD'));
        self.Load();
    };

    self.Load();
}