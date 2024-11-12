$('body').addClass('preference_body account_setting_body accont_coms');

var accountSetupViewModel = new AccountSetupViewModel();
ko.applyBindings(accountSetupViewModel);

function AccountSetupViewModel() {
    var self = this;
    self.UserName = ko.observable();
    self.Email = ko.observable();
    self.CurrentPassword = ko.observable();
    self.NewPassword = ko.observable();
    self.ConfirmPassword = ko.observable();

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "reservationsetting",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.UserName(data.data.UserName);
                self.Email(data.data.Email);
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
        $.ajax({
            type: 'PUT',
            url: webserviceURL + "reservationsetting",
            data: {
                Email: self.Email(),
                CurrentPassword: self.CurrentPassword(),
                NewPassword: self.NewPassword(),
                ConfirmPassword: self.ConfirmPassword()
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