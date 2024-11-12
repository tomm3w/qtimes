$('body').addClass('metrics_body');
$('.metrics-icon').parent().parent().addClass('active');
$('.metrics-icon').addClass('show');

var metricsViewModel = new MetricsViewModel();
ko.applyBindings(metricsViewModel);

function MetricsViewModel() {
    var self = this;

    self.Date = ko.observable(moment().format('YYYY-MM-DD'));
    self.StartDate = ko.observable(moment().format('YYYY-MM-DD'));
    self.EndDate = ko.observable(moment().format('YYYY-MM-DD'));
    self.FilterBy = ko.observable('Today');
    self.AvgGroupSize = ko.observable();
    self.Cancelled = ko.observable();
    self.NewGuest = ko.observable();
    self.NoOfPlace = ko.observable();
    self.AvgWaitTime = ko.observable();
    self.BusinessId = ko.computed(function () {
        return $.cookie("businessid");
    });

    self.Load = function () {
        ShowLoader();

        $.ajax({
            type: 'POST',
            url: webserviceURL + "reservation/metrics",
            data: { Date: self.Date(), BusinessDetailId: self.BusinessId(), FilterBy: self.FilterBy(), StartDate: self.StartDate(), EndDate: self.EndDate() },
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
        //var date = $('#select_onday').val();
        //self.Date(moment().subtract(date, 'days').format('YYYY-MM-DD'));
        //self.FilterBy($("#select_onday option:selected").text());
        self.StartDate($('#daterange').data('daterangepicker').startDate.format('YYYY/MM/DD'));
        self.EndDate($('#daterange').data('daterangepicker').endDate.format('YYYY/MM/DD'));
        self.Load();
    };

    //self.Load();
}

$(function () {

    var start = moment();//.subtract(29, 'days');
    var end = moment();

    function cb(start, end) {
        $('#daterange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    }

    $('#daterange').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment()],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    cb(start, end);
    metricsViewModel.Load();

    $('#daterange').on('apply.daterangepicker', function (ev, picker) {
        metricsViewModel.OnChangeDate();
    });

    $('#daterange').on('cancel.daterangepicker', function (ev, picker) {
    });
});