$('body').addClass('preference_body accont_coms concert__event concert_preference_body');

var eventSeatingsViewModel = new EventSeatingsViewModel();
ko.applyBindings(eventSeatingsViewModel);

function EventSeatingsViewModel() {
    var self = this;
    self.OpenDayFrom = ko.observable();
    self.OpenDayTo = ko.observable();
    self.OpenHourFrom = ko.observable();
    self.OpenHourTo = ko.observable();

    self.EnableTotalCapacity = ko.observable();
    self.TotalCapacity = ko.observable();
    self.EnableGroupSize = ko.observable();
    self.MinGroupSize = ko.observable();
    self.MaxGroupSize = ko.observable();
    self.EnableTimeExpiration = ko.observable();
    self.TimeExpiration = ko.observable();
    self.IsDLScanEnabled = ko.observable();
    self.IsChargeEnabled = ko.observable();
    self.IsSeatmapEnabled = ko.observable();

    self.ConcertId = ko.computed(function () {
        return $.cookie("concertid");
    });

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "concertreservationsetting/concert/" + self.ConcertId(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.OpenDayFrom(data.data.OpenDayFrom);
                self.OpenDayTo(data.data.OpenDayTo);
                self.OpenHourFrom(data.data.OpenHourFrom);
                self.OpenHourTo(data.data.OpenHourTo);
                self.EnableTotalCapacity(data.data.EnableTotalCapacity);
                self.TotalCapacity(data.data.TotalCapacity);
                self.EnableGroupSize(data.data.EnableGroupSize);
                self.MinGroupSize(data.data.MinGroupSize);
                self.MaxGroupSize(data.data.MaxGroupSize);
                self.EnableTimeExpiration(data.data.EnableTimeExpiration);
                self.TimeExpiration(data.data.TimeExpiration);
                self.IsDLScanEnabled(data.data.IsDLScanEnabled);
                self.IsChargeEnabled(data.data.IsChargeEnabled);
                self.IsSeatmapEnabled(data.data.IsSeatmapEnabled);
                $('.selectpicker').selectpicker('refresh');
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.UpdatePreference = function () {
        ShowLoader();
        $.ajax({
            type: 'PUT',
            url: webserviceURL + "concertreservationsetting/UpdateConcertPreference",
            data: {
                OpenDayFrom: self.OpenDayFrom(),
                OpenDayTo: self.OpenDayTo(),
                OpenHourFrom: self.OpenHourFrom(),
                OpenHourTo: self.OpenHourTo(),
                OpenDayFrom: self.OpenDayFrom(),
                EnableTotalCapacity: self.EnableTotalCapacity(),
                TotalCapacity: self.TotalCapacity(),
                EnableGroupSize: self.EnableGroupSize(),
                MinGroupSize: self.MinGroupSize(),
                MaxGroupSize: self.MaxGroupSize(),
                EnableTimeExpiration: self.EnableTimeExpiration(),
                TimeExpiration: self.TimeExpiration(),
                IsDLScanEnabled: self.IsDLScanEnabled(),
                IsChargeEnabled: self.IsChargeEnabled(),
                IsSeatmapEnabled: self.IsSeatmapEnabled(),
                ConcertId: self.ConcertId()
            },
            success: function (data) {
                $.notify("Business preference updated successfully.", "success");
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.Load();
}