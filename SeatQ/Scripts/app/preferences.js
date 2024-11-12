$('body').addClass('preference_body account_setting_body accont_coms');

function PreferenceViewModel() {
    var self = this;
    self.RestaurantChainId = ko.observable();
    self.OpeningHour = ko.observable();
    self.ClosingHour = ko.observable();
    self.WaittimeInterval = ko.observable();
    self.ClosedDay = ko.observableArray([]);

    self.Load = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + "restaurant/?RestaurantChainId=" + rcid,
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data) {
                if (data.status == "ok") {
                    self.RestaurantChainId(data.data.RestaurantChainId);
                    self.OpeningHour(data.data.OpeningHour);
                    self.ClosingHour(data.data.ClosingHour);
                    self.WaittimeInterval(data.data.WaittimeInterval);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            });
    };
    self.Update = function () {
        $.ajax({
            type: "PUT",
            url: webserviceURL + "preferences",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON(self),
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data) {
                if (data.status == "ok") {
                    NotifySavedMessage("Saved successfully.");
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            }).error(function (xhr, status, error) {
                ShowErrorMessage(xhr, status, error)
            });
    }

    self.Load();
}

var preferenceViewModel = new PreferenceViewModel();
ko.applyBindings(preferenceViewModel, document.getElementById("content-wrapper"));