$('body').addClass('preference_body  concert__event concert_account_body concert_events');

var accountSetupViewModel = new AccountSetupViewModel();
ko.applyBindings(accountSetupViewModel);

function AccountSetupViewModel() {
    var self = this;
    self.BusinessAccounts = ko.observableArray([]);
    self.BusinessName = ko.observable();
    self.BusinessTypeId = ko.observable();

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "concertreservationsetting/businessaccounts",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var tables = $.map(data.data.BusinessAccounts, function (item) {
                    return new BusinessAccount(item);
                });
                self.BusinessAccounts(tables);

                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.AddBusiness = function () {
        ShowLoader();
        $.ajax({
            type: 'POST',
            url: webserviceURL + "business",
            data: {
                Name: self.BusinessName(),
                BusinessTypeId: self.BusinessTypeId()
            },
            success: function (data) {
                $.notify("Business added successfully.", "success");
                $('#add-new-reserve_id').modal('hide');
                HideLoader();
                self.Load();
                self.ClearAll();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.ClearAll = function () {
        self.BusinessName('');
        self.BusinessTypeId(1);
    }

    self.Load();
}

function BusinessAccount(data) {
    var $res = this;
    $res.ReservationBusinessId = ko.observable(data.ReservationBusinessId);
    $res.BusinessName = ko.observable(data.BusinessName);
    $res.LogoFullPath = ko.computed(function () {
        if (data.LogoPath)
            return data.LogoFullPath;
        else
            return '/content/assets/images/Group 727@2x.png';
    });
    $res.ConcertId = ko.observable(data.ConcertId);
    $res.BusinessId = ko.observable(data.BusinessId);
    $res.BusinessType = ko.observable(data.BusinessType);
    $res.ButtonText = ko.computed(function () {
        var selectedConcert = $.cookie("concertid");
        if (data.ConcertId === selectedConcert)
            return "Current";
        else
            return "Switch";
    });
    $res.Href = ko.computed(function () {
        if (data.BusinessType != "Event/Concert")
            return "/admin/reservation/?id=" + data.BusinessId;
        else
            return "/concert/reservation/?id=" + data.ConcertId;
    });
}