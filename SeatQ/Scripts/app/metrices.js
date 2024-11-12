$('body').addClass('metrics_body');

function MetricsViewModel() {
    var self = this;
    self.Seated = ko.observable();
    self.NoShow = ko.observable();
    self.Promotions = ko.observable();
    self.Guest = ko.observable();
    self.GroupSize = ko.observable();
    self.AverageWaitTime = ko.observable(0);
    self.StartDate = ko.observable();
    self.EndDate = ko.observable();

    self.LoadQuery = function () {

        var d = new Date();
        var MetricsType = $('#select_onday').val();
        var StartDate = moment(self.StartDate()).format('YYYY-MM-DD');
        var EndDate = StartDate;

        self.GetData(0, MetricsType, StartDate, EndDate);
    }
    self.LoadCustomQuery = function () {

        if (!self.StartDate() || !self.EndDate())
            return false;

        self.StartDate(moment(self.StartDate()).format('YYYY-MM-DD'));
        self.EndDate(moment(self.EndDate()).format('YYYY-MM-DD'));

        self.GetData(0, "Custom", self.StartDate(), self.EndDate());
        ClearAll();
    }

    self.GetData = function (restaurantChainId, metricsType, startDate, endDate) {
        $.ajax({
            type: "GET",
            url: webserviceURL + 'metrices/?' + 'RestaurantChainId=' + rcid + '&MetricsType=' + metricsType + '&'
                + 'StartDate=' + startDate + '&' + 'EndDate=' + endDate,
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json; charset=utf-8"
        })
            .done(function (data, textStatus, xhr) {
                if (data.status == "ok") {
                    self.Seated(data.data.Seated);
                    self.NoShow(data.data.NoShow);
                    self.Promotions(data.data.Promotions);
                    self.Guest(data.data.Guest);
                    self.GroupSize(data.data.GroupSize);
                    self.AverageWaitTime(data.data.AverageWaitTime);
                    ClearAll();

                }
            }).
            error(function (xhr, status, error) {
                ShowErrorMessage(xhr, status, error)
            });

    }

    self.LoadQuery();
};

var metricsViewModel = new MetricsViewModel();
ko.applyBindings(metricsViewModel, document.getElementById("content-wrapper"));

function ShowFilter() {
    $("#divFilter").show()
}

function ShowCustom() {
    $("#divCustom").show()
}

function ClearAll() {
    $("#divCustom").hide();
}