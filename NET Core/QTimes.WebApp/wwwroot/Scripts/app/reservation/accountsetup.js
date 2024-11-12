$('body').addClass('preference_body account_setting_body accont_coms');
$('#timezone').timezones();

var accountSetupViewModel = new AccountSetupViewModel();
ko.applyBindings(accountSetupViewModel);

function AccountSetupViewModel() {
    var self = this;
    self.UserName = ko.observable();
    self.Email = ko.observable();
    self.CurrentPassword = ko.observable();
    self.NewPassword = ko.observable();
    self.ConfirmPassword = ko.observable();

    self.BusinessName = ko.observable();
    self.FullName = ko.observable();
    self.Address = ko.observable();
    self.City = ko.observable();
    self.State = ko.observable();
    self.Zip = ko.observable();
    self.TimezoneOffset = ko.observable();
    self.TimezoneOffsetValue = ko.observable();

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "reservationsetting",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.UserName(data.data.UserName);
                self.Email(data.data.Email);
                self.BusinessName(data.data.BusinessName);
                self.FullName(data.data.FullName);
                self.Address(data.data.Address);
                self.City(data.data.City);
                self.State(data.data.State);
                self.Zip(data.data.Zip);
                self.TimezoneOffset(data.data.TimezoneOffset);
                self.TimezoneOffsetValue(data.data.TimezoneOffsetValue);
                $('#timezone').val(data.data.TimezoneOffsetValue);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.UpdateAccountSetup = function () {
        ShowLoader();
        self.TimezoneOffset($('#timezone option:selected').attr('data-offset'));
        self.TimezoneOffsetValue($('#timezone').val());
        $.ajax({
            type: 'PUT',
            url: webserviceURL + "reservationsetting",
            data: {
                BusinessName: self.BusinessName(),
                FullName: self.FullName(),
                Email: self.Email(),
                CurrentPassword: self.CurrentPassword(),
                NewPassword: self.NewPassword(),
                ConfirmPassword: self.ConfirmPassword(),
                Address: self.Address(),
                City: self.City(),
                State: self.State(),
                Zip: self.Zip(),
                TimezoneOffset: self.TimezoneOffset(),
                TimezoneOffsetValue: self.TimezoneOffsetValue()
            },
            success: function (data) {
                $.notify("Account updated successfully.", "success");
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