$('body').addClass('commonctn waitlist2 waitlist3');
$('[type="date"].date_picker_show').prop('min', function () {
    return new Date().toJSON().split('T')[0];
});


var reservationViewModel = new ReservationViewModel();
ko.applyBindings(reservationViewModel);

function ReservationViewModel() {
    var self = this;
    self.GuestName = ko.observable();
    self.Size = ko.observable(1);
    self.Comments = ko.observable();
    self.MobileNumber = ko.observable();
    self.SelectedDate = ko.observable(moment().format('YYYY-MM-DD'));
    self.TimeFrom = ko.observable();
    self.TimeTo = ko.observable();
    self.Times = ko.observableArray([]);
    self.LimitSize = ko.observable(0);
    self.IsDlscanEnabled = ko.observable(false);
    self.Dob = ko.observable();
    self.FamilyName = ko.observable();
    self.FirstName = ko.observable();
    self.State = ko.observable();
    self.City = ko.observable();
    self.Zip = ko.observable();
    self.Address = ko.observable();
    self.FacePhotoImageBase64 = ko.observable();
    self.Email = ko.observable();
    self.Load = function () {
        var root = this;
        $('.loader').removeClass('hidden');
        $.ajax({
            type: 'POST',
            url: webserviceURL + "reservation/business/" + id,
            data: { Id: id, Date: self.SelectedDate() },
            success: function (data) {
                var tables = $.map(data.data.TimeSlots, function (item) {
                    return new TimeSlot(item)
                });
                self.Times(tables);
                self.LimitSize(data.data.LimitSize == 0 ? 1 : data.data.LimitSize);
                $('.loader').addClass('hidden');
                const refId = getUrlParam('refId');
                const phone = getUrlParam('phone');
                const email = getUrlParam('email');
                root.IsDlscanEnabled(data.data.IsDlscanEnabled);
                if (data.data.IsDlscanEnabled && refId === null) {
                    var url = `${dlScanAppUrl}?callback=${window.location.href}`
                    window.location.replace(url);
                } else if (refId) {
                    $.getJSON(`${webserviceURL}DriverLicense?id=${refId}`, function (data) {
                        root.GuestName(data.data.Document.FullName);
                        root.Dob(data.data.Document.DOB);
                        root.FamilyName(data.data.Document.FamilyName);
                        root.FirstName(data.data.Document.FirstName);
                        root.State(data.data.Document.State);
                        root.City(data.data.Document.City);
                        root.Zip(data.data.Document.Zip);
                        root.Address(data.data.Document.Address);
                        root.FacePhotoImageBase64(data.data.FacePhotoFromDocument);
                        root.MobileNumber(phone);
                        root.Email(email);
                        iti.setNumber('+' + phone);
                    });
                }
            },
            error: function (xhr, status, error) {
                $('.loader').addClass('hidden');
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
        if (!iti.isValidNumber()) {
            NotifyErrorMessage("Invalid Mobile No., Please enter valid number.")
            return false;
        }

        $('.loader').removeClass('hidden');
        $.ajax({
            type: 'POST',
            url: webserviceURL + "reservation/addreservation",
            data: {
                BusinessDetailId: id,
                Name: self.GuestName(),
                MobileNo: iti.getNumber(),
                GuestTypeId: 3,
                Size: self.Size(),
                Date: self.SelectedDate(),
                TimeFrom: self.TimeFrom(),
                TimeTo: self.TimeTo(),
                Comments: self.Comments(),
                Dob: self.Dob,
                FamilyName: self.FamilyName(),
                FirstName: self.FirstName(),
                State: self.State(),
                City: self.City(),
                Zip: self.Zip(),
                Address: self.Address(),
                Email: self.Email(),
                FacePhotoImageBase64: self.FacePhotoImageBase64()

            },
            success: function (data) {
                document.cookie = "pass=" + data.data.PassUrl + "; path=/";
                location.href = '/m/reservation/confirmation/' + data.data.ReservationId;
            },
            error: function (xhr, status, error) {
                $('.loader').addClass('hidden');
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.OnReservationDateChange = function () {
        var date = event.target.value;
        self.SelectedDate(date);
        self.Load();
    };

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
    $time.IsHourPassed = ko.computed(function () {
        var now = new Date();
        var selectedDate = moment(reservationViewModel.SelectedDate());
        var totime = moment(selectedDate.year() + '-' + (selectedDate.month() + 1) + '-' + selectedDate.date() + ' ' + data.OpenHourTo);
        if (now > totime)
            return true;
        else
            false;

    });
}
function getUrlParam(prop) {
    // No parameters
    if (location.search === "") return null;

    const search = location.search.substring(1);

    // Multiple parameters
    if (search.includes("&")) {
        const params = new URLSearchParams(search);
        return params.get(prop);
    }

    // Single parameter
    const parts = search.split("=");
    const key = parts[0];
    const val = parts[1];

    if (key === prop) {
        return val;
    }

    return null;
}