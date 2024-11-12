$('body').addClass('accountset_body accont_coms concert__event concert_setup_body');

var accountSetupViewModel = new AccountSetupViewModel();
ko.applyBindings(accountSetupViewModel);

function AccountSetupViewModel() {
    var self = this;

    self.UserName = ko.observable();
    self.BusinessName = ko.observable();
    self.FullName = ko.observable();
    self.Email = ko.observable();
    self.Address = ko.observable();
    self.City = ko.observable();
    self.State = ko.observable();
    self.Zip = ko.observable();
    self.CurrentPassword = ko.observable();
    self.NewPassword = ko.observable();
    self.ConfirmPassword = ko.observable();
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
                self.UserName(data.data.UserName);
                self.BusinessName(data.data.BusinessName);
                self.FullName(data.data.FullName);
                self.Email(data.data.Email);
    
                self.Address(data.data.Address);
                self.City(data.data.City);
                self.State(data.data.State);
                self.Zip(data.data.Zip);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.UpdateProfile = function () {
        ShowLoader();

        $.ajax({
            type: 'PUT',
            url: webserviceURL + "concertreservationsetting/UpdateConcertAccount",
            data: {
                BusinessName: self.BusinessName(),
                FullName: self.FullName(),
                Email: self.Email(),
                Address: self.Address(),
                City: self.City(),
                State: self.State(),
                Zip: self.Zip(),
                CurrentPassword: self.CurrentPassword(),
                NewPassword: self.NewPassword(),
                ConfirmPassword: self.ConfirmPassword()
            },
            success: function (data) {
                $.notify("Business account updated successfully.", "success");
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