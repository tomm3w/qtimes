$('body').addClass('preference_body account_setting_body accont_coms');


function AccountInfoViewModel() {
    var self = this;
    self.UserId = ko.observable();
    self.UserName = ko.observable();
    self.RestaurantChainId = ko.observable();
    self.isActive = ko.observable();
    self.Email = ko.observable();
    self.CurrentPassword = ko.observable();
    self.NewPassword = ko.observable();
    self.ConfirmNewPassword = ko.observable();

    self.Load = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + "users/GetCurrentUser/?RestaurantChainId=" + rcid,
            contentType: "application/json; charset=utf-8",
            //data: ko.toJSON({ UserId: uid })
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data) {
                if (data.status == "ok") {
                    self.UserId(data.data.UserId);
                    self.RestaurantChainId(data.data.RestaurantChainId);
                    self.UserName(data.data.UserName);
                    self.isActive(data.data.isActive.toString());
                    self.Email(data.data.Email);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            });
    };
    self.Update = function () {
        $.ajax({
            type: "PUT",
            url: webserviceURL + "accountinfo",
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
    self.Delete = function () {
        if (confirm('Are you sure you want to delete this account?')) {
            $.ajax({
                type: "DELETE",
                url: webserviceURL + "accountinfo",
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
                        NotifySavedMessage("Account deleted successfully.");
                        window.location.href = "/Account/Logoff";
                    }
                    else if (data.status == "badrequest") {
                        showErrorMessage(data);
                    }
                }).error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
    };

    self.Load();
}

var accountInfoViewModel = new AccountInfoViewModel();
ko.applyBindings(accountInfoViewModel, document.getElementById("content-wrapper"));