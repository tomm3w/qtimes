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
    self.ConcertEvents = ko.observableArray([]);
    self.CurrentConcertEvent = ko.observable();
    self.ConcertId = ko.computed(function () {
        return $.cookie("concertid");
    });

    self.Load = function () {

        var qs = '';
        if (self.CurrentConcertEvent()) {
            qs = `?concertEventId=${self.CurrentConcertEvent()}`
        }
        else
            return false;

        ShowLoader();
        $.post({
            type: 'POST',
            url: webserviceURL + `concerts/${self.ConcertId()}/metrics` + qs,
            data: { Date: self.Date(), ConcertId: self.ConcertId() },
            success: function (data) {
                self.TotalGuests(parseInt(data.data.TotalGuests));
                self.TotalEvents(data.data.TotalEvents);
                self.TotalReservations(data.data.TotalReservations);
                HideLoader();
                RefreshSummary();
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

    self.OnChangeEvent = function (data, event) {
        $.cookie('eventid', data.CurrentConcertEvent());
        this.Load();
    };

    var qs = '?Page=1' + '&PageSize=65535';
    $.ajax({
        type: 'GET',
        url: webserviceURL + `concerts/${self.ConcertId()}/events` + qs,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            self.ConcertEvents(data.data.Model);
            $('.selectpicker').selectpicker('refresh');
            self.Load();
        },
        error: function (xhr, status, error) {
            ShowErrorMessage(xhr, status, error);
        }
    });
}