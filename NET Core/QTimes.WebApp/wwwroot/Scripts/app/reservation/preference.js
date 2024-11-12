$('body').addClass('preference_body accont_coms');

var preferenceViewModel = new PreferenceViewModel();
ko.applyBindings(preferenceViewModel);

function PreferenceViewModel() {
    var self = this;

    self.OpenDayFrom = ko.observable();
    self.OpenDayTo = ko.observable();
    self.OpenHourFrom = ko.observable();
    self.OpenHourTo = ko.observable();
    self.IsLimitSizePerHour = ko.observable();
    self.LimitSizePerHour = ko.observable();
    self.IsLimitTimePerReservation = ko.observable();
    self.MinLimitTimePerReservation = ko.observable();
    self.MaxLimitTimePerReservation = ko.observable();
    self.IsLimitSizePerGroup = ko.observable();
    self.LimitSizePerGroup = ko.observable();
    self.IsChargeEnabled = ko.observable();
    self.IsDLScanEnabled = ko.observable();
    self.IsSeatmapEnabled = ko.observable();
    self.NoOfSeatRows = ko.observable();
    self.NoOfSeatColumns = ko.observable();
    self.BusinessId = ko.computed(function () {
        return $.cookie("businessid");
    });

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "reservationsetting/getbusiness/" + self.BusinessId(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.OpenDayFrom(data.data.OpenDayFrom);
                self.OpenDayTo(data.data.OpenDayTo);
                self.OpenHourFrom(data.data.OpenHourFrom);
                self.OpenHourTo(data.data.OpenHourTo);
                self.IsLimitSizePerHour(data.data.IsLimitSizePerHour);
                self.LimitSizePerHour(data.data.LimitSizePerHour);
                self.IsLimitTimePerReservation(data.data.IsLimitTimePerReservation);
                self.MinLimitTimePerReservation(data.data.MinLimitTimePerReservation);
                self.MaxLimitTimePerReservation(data.data.MaxLimitTimePerReservation);
                self.IsLimitSizePerGroup(data.data.IsLimitSizePerGroup);
                self.LimitSizePerGroup(data.data.LimitSizePerGroup);
                self.IsChargeEnabled(data.data.IsChargeEnabled);
                self.IsDLScanEnabled(data.data.IsDLScanEnabled);
                self.IsSeatmapEnabled(data.data.IsSeatmapEnabled);
                self.NoOfSeatRows(data.data.NoOfSeatRows);
                self.NoOfSeatColumns(data.data.NoOfSeatColumns);
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
            url: webserviceURL + "reservationsetting/UpdatePreference",
            data: {
                OpenDayFrom: self.OpenDayFrom(),
                OpenDayTo: self.OpenDayTo(),
                OpenHourFrom: self.OpenHourFrom(),
                OpenHourTo: self.OpenHourTo(),
                IsLimitSizePerHour: self.IsLimitSizePerHour(),
                LimitSizePerHour: self.LimitSizePerHour(),
                IsLimitTimePerReservation: self.IsLimitTimePerReservation(),
                MinLimitTimePerReservation: self.MinLimitTimePerReservation(),
                MaxLimitTimePerReservation: self.MaxLimitTimePerReservation(),
                IsLimitSizePerGroup: self.IsLimitSizePerGroup(),
                LimitSizePerGroup: self.LimitSizePerGroup(),
                IsChargeEnabled: self.IsChargeEnabled(),
                IsDLScanEnabled: self.IsDLScanEnabled(),
                IsSeatmapEnabled: self.IsSeatmapEnabled(),
                NoOfSeatRows: self.NoOfSeatRows(),
                NoOfSeatColumns: self.NoOfSeatColumns(),
                Id: self.BusinessId()
            },
            success: function (data) {
                $.notify("Preference updated successfully.", "success");
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