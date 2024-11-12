$('body').addClass('waitlist2');

var reservationViewModel = new ReservationViewModel();
ko.applyBindings(reservationViewModel);

function ReservationViewModel() {
    var self = this;
    self.GuestName = ko.observable();
    self.Size = ko.observable();
    self.Comments = ko.observable();
    self.MobileNumber = ko.observable();
    self.SelectedDate = ko.observable();
    self.TimeFrom = ko.observable();
    self.TimeTo = ko.observable();
    self.Times = ko.observableArray([]);

    self.Load = function () {
        $.ajax({
            type: 'GET',
            url: webserviceURL + "reservation/business/" + id,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var tables = $.map(data.data.TimeSlots, function (item) {
                    return new TimeSlot(item)
                });
                self.Times(tables);
            },
            error: function (xhr, status, error) {
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.SetSizeValue = function (data, event) {
        self.Size($(event.currentTarget).text());
        $(event.currentTarget).parent().siblings().removeClass('active');
        $(event.currentTarget).parent().addClass('active');
    }

    self.Add = function () {
        $.ajax({
            type: 'POST',
            url: webserviceURL + "reservation/addreservation",
            data: {
                ReservationBusinessId: id,
                Name: self.GuestName(),
                MobileNo: self.MobileNumber(),
                GuestTypeId: 3,
                Size: self.Size(),
                Date: self.SelectedDate(),
                TimeFrom: self.TimeFrom(),
                TimeTo: self.TimeTo(),
                Comments: self.Comments()
            },
            success: function (data) {
                location.href = '/reservation/m/thankyou/' + id;
            },
            error: function (xhr, status, error) {
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.Load();
}

function TimeSlot(data) {
    var $time = this;
    $time.OpenHourFrom = ko.observable(data.OpenHourFrom);
    $time.OpenHourTo = ko.observable(data.OpenHourTo);
    $time.SpotLeft = ko.observable(data.SpotLeft);
    $time.Time = ko.computed(function () {
        return moment('1990-01-01 ' + data.OpenHourFrom).format("hh:mm A") + ' - ' + moment('1990-01-01 ' + data.OpenHourTo).format("hh:mm A");
    });
    $time.SetTimeSlot = function (data, event) {
        $(event.currentTarget).siblings().removeClass('active');
        $(event.currentTarget).addClass('active');
        $('.input_chk').removeClass('tick');
        $(event.currentTarget).find('.input_chk').addClass('tick');
        reservationViewModel.TimeFrom(data.OpenHourFrom());
        reservationViewModel.TimeTo(data.OpenHourTo());
    }
}