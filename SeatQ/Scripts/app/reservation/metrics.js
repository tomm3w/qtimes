$('body').addClass('metrics_body');
$('.metrics-icon').parent().parent().addClass('active');
$('.metrics-icon').addClass('show');

var metricsViewModel = new MetricsViewModel();
ko.applyBindings(metricsViewModel);

function MetricsViewModel() {
    var self = this;

    self.Date = ko.observable(moment().format('YYYY-MM-DD'));
    self.AvgGroupSize = ko.observable();
    self.Cancelled = ko.observable();
    self.NewGuest = ko.observable();
    self.NoOfPlace = ko.observable();
    self.AvgWaitTime = ko.observable();

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'POST',
            url: webserviceURL + "reservation/metrics",
            data: { Date: self.Date() },
            success: function (data) {
                self.AvgGroupSize(parseInt(data.data.AvgGroupSize));
                self.Cancelled(data.data.Cancelled);
                self.NewGuest(data.data.NewGuest);
                self.NoOfPlace(data.data.NoOfPlace);
                self.AvgWaitTime(parseInt(data.data.AvgWaitTime));
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