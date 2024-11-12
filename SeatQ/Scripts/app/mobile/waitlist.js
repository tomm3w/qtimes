$(function () {

    $("input[type='number']").inputSpinner();

    $(".waittime .btn--orng").click(function () {
        $(".waittime").hide();
        $(".waitlist").show();
    });

});

function RestaurantModel() {
    var self = this;
    self.RestaurantChainId = ko.observable();
    self.BusinessName = ko.observable();
    self.Phone = ko.observable();
    self.CityTown = ko.observable();
    self.State = ko.observable();
    self.AverageWaitTime = ko.observable();
    self.LogoPath = ko.observable();
    self.Comments = ko.observable();

    self.EstimatedHours = ko.observable();
    self.EstimatedMins = ko.observable();
    self.EstimatedTime = ko.observable();
    self.ActualHours = ko.observable();
    self.ActualMins = ko.observable();
    self.ActualTime = ko.observable();
    self.GroupSize = ko.observable(1);
    self.GuestName = ko.observable();
    self.MobileNumber = ko.observable();
    self.ActualDateTime = ko.observable();
    self.EstimatedDateTime = ko.observable();
    self.GetEstimatedDateTime = ko.computed(function () {
        var hour = 0;
        var min = 0;
        var date = new Date();
        var time = self.EstimatedHours() + ":" + self.EstimatedMins();
        if (time) {
            hour = time.split(":")[0];
            min = time.split(":")[1];
        }

        if (min == 0 && hour == 0)
            return date;
        else {
            if (min == undefined || min == "" || parseInt(min) == "NaN" || min == "undefined")
                min = 0;
            if (hour == undefined || hour == "" || parseInt(hour) == "NaN" || hour == "undefined")
                hour = 0;

            date.setMinutes(date.getMinutes() + (parseInt(hour) * 60) + parseInt(min))

            if (min == 0 && hour == 0)
                return null;
            else
                return date;
        }
    });

    self.Load = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + "restaurant/GetRestaurantByBusinessId/?BusinessId=" + businessId
        })
            .done(function (data) {
                if (data.status === "ok") {
                    self.RestaurantChainId(data.data.RestaurantChainId);
                    self.BusinessName(data.data.BusinessName);
                    self.CityTown(data.data.CityTown);
                    self.State(data.data.State);
                    self.AverageWaitTime(data.data.AverageWaitTime);
                    self.LogoPath(data.data.LogoPath);

                    var AvgWaitTime = self.AverageWaitTime();
                    var AvgHour = 0;
                    var AvgMin = 0;
                    if (AvgWaitTime === undefined) AvgWaitTime = 0;
                    AvgHour = parseInt(AvgWaitTime / 60);
                    AvgMin = (AvgWaitTime - (AvgHour * 60));
                    self.EstimatedHours(AvgHour < 10 ? '0' + AvgHour : AvgHour);
                    self.EstimatedMins(AvgMin < 10 ? '0' + AvgMin : AvgMin);
                    self.EstimatedTime((AvgHour < 10 ? '0' + AvgHour : AvgHour) + ":" + (AvgMin < 10 ? '0' + AvgMin : AvgMin));

                    var date = new Date();
                    var hours = date.getHours();
                    var minutes = date.getMinutes();
                    var ampm = hours >= 12 ? 'pm' : 'am';
                    hours = hours % 12;
                    hours = hours ? hours : 12;
                    minutes = minutes < 10 ? '0' + minutes : minutes;
                    self.ActualDateTime(date);
                    self.ActualHours(hours);
                    self.ActualMins(minutes);
                    self.ActualTime((hours < 10 ? '0' + hours : hours) + ":" + minutes);
                    //$("#favicon, #apple-touch-icon").attr("href", self.LogoPath);
                }
                else if (data.status === "badrequest") {
                    showErrorMessage(data);
                }
            })
            .error(function (data) {
                alert(data.statusText);
            });
    };

    self.AddWaitlist = function () {

        $("#btnSubmit").attr("disabled", "disabled");
        self.ActualDateTime(new Date());

        $.ajax({
            type: "POST",
            url: webserviceURL + 'restaurant/waitlist/',
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON({
                RestaurantChainId: restaurantModel.RestaurantChainId(),
                GuestName: restaurantModel.GuestName(),
                MobileNumber: restaurantModel.MobileNumber(),
                GroupSize: restaurantModel.GroupSize(),
                GuestTypeId: 3,
                EstimatedDateTime: restaurantModel.GetEstimatedDateTime(),
                ActualDateTime: restaurantModel.ActualDateTime(),
                Comments: restaurantModel.Comments()
            })
        })
            .done(function (data) {
                if (data.status === "ok") {
                    $(".waitlist").hide();
                    $(".thankyou").show();
                }
                else if (data.status === "badrequest") {
                    $("#btnSubmit").removeAttr("disabled");
                    showErrorMessage(data);
                }
            })
            .error(function (data) {
                alert(data.statusText);
            });
    };

    self.Load();
}

var restaurantModel = new RestaurantModel();
ko.applyBindings(restaurantModel);

function showErrorMessage(jsondata) {
    if (jsondata.status === "badrequest") {
        if (jsondata.message) {
            alert(jsondata.message);
        }
    }
}