$('body').addClass('waitlist2 waitlist3 waitlist4 guest-detail_body iti-mobile');

var input = document.querySelector("#mob_id");
var iti = window.intlTelInput(input, {
    separateDialCode: true,
    utilsScript: "/content/intl-tel-input/js/utils.js"
});

$(".reserve_absolute1 .btn").click(function (e) {
    e.preventDefault();
    $('#appHeader').text('GUEST DETAILS');
    $('.reserve_absolute2').show();
    $('.reserve_absolute1').hide();
    $('.reserve_absolute3').hide();
});
$(".reserve_absolute2 .front_arrow").click(function (e) {
    e.preventDefault();
    $('#appHeader').text('NEW RESERVATION');
    $('.reserve_absolute1').show();
    $('.reserve_absolute2').hide();
    $('.reserve_absolute3').hide();
});

$(".seat-cg-btn a").click(function (e) {
    e.preventDefault();
    $('#appHeader').text('Spot Map');
    $('.reserve_absolute1').hide();
    $('.reserve_absolute2').hide();
    $('.reserve_absolute3').show();
});

$(".cross-del").click(function (e) {
    e.preventDefault();
    $('#appHeader').text('NEW RESERVATION');
    $('.reserve_absolute1').show();
    $('.reserve_absolute2').hide();
    $('.reserve_absolute3').hide();
});

var reservationViewModel = new ReservationViewModel();
ko.applyBindings(reservationViewModel);

function ReservationViewModel() {
    var self = this;

    self.GuestTypeId = ko.observable(3);//Mobile connected
    self.Size = ko.observable(1);
    self.Guests = ko.observableArray([]);
    self.Seatings = ko.observableArray([]);
    self.LimitSize = ko.observable(5);
    self.SpotName = ko.observable();
    self.SpotValue = ko.observable();
    self.SeatNo = ko.observable();
    self.GuestName = ko.observable();
    self.MobileNumber = ko.observable();
    self.Email = ko.observable();
    self.IsDlscanEnabled = ko.observable(false);
    self.Dob = ko.observable();
    self.FamilyName = ko.observable();
    self.FirstName = ko.observable();
    self.State = ko.observable();
    self.City = ko.observable();
    self.Zip = ko.observable();
    self.Address = ko.observable();
    self.FacePhotoImageBase64 = ko.observable();
    self.SeatMap = ko.observable();
    self.EnableTimeExpiration = ko.observable();

    self.SetSizeValue = function (data, event) {
        self.Size($(event.currentTarget).text());

        $(event.currentTarget).parent().siblings().removeClass('active');
        $(event.currentTarget).parent().addClass('active');
        self.AddGuests(self.Size() - 1);
    }

    self.AddGuests = function (size) {
        self.Guests([]);
        for (var i = 0; i < size; i++) {
            self.Guests.push(new Guest({ Name: '', MobileNo: '', SeatNo: 1, SeatName: '', IsMainGuest: false }));

            window.intlTelInput(document.querySelector("#mob_id" + (i + 2)), {
                separateDialCode: true
            });
        }
    };

    self.OnChangeSpot = function (data, event) {
        var spots = $(event.currentTarget).siblings('select.spots');
        self.SpotName($(event.currentTarget).find('option:selected').text());
        spots.empty();

        spots.append(new Option("Choose...", ""));
        for (var i = 1; i <= self.SpotValue(); i++) {
            spots.append(new Option(i, i));
        }

    };

    self.OnSeatSelect = function (data, event) {
        if (self.EnableTimeExpiration()) {
            $('.loader').removeClass('hidden');
            self.SeatNo($(event.currentTarget).find('option:selected').text());
            $.get(
                webserviceURL + `concerts/${concertId}/events/${eventId}/seatlocks/${data.SpotName() + '-' + data.SeatNo()}`,
                function (response, status) {
                }).always(function () {
                    $('.loader').addClass('hidden');
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    ShowErrorMessage(jqXHR, textStatus, errorThrown);
                });
        }
    }

    self.AddReservation = function () {
        $('.loader').removeClass('hidden');
        $.ajax({
            type: 'POST',
            url: webserviceURL + `concerts/${concertId}/events/${eventId}/reservations`,
            //url: webserviceURL + "concerteventreservation/addconcertreservation",
            data: {
                ConcertEventId: eventId,
                GuestTypeId: self.GuestTypeId(),
                Size: self.Size(),
                Guests: self.Guests(),
                MainGuestName: self.GuestName(),
                MainGuestMobileNo: self.MobileNumber(),
                MainGuestEmail: self.Email(),
                MainGuestSeatName: self.SpotName(),
                MainGuestSeatNo: self.SeatNo(),
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
                location.href = '/m/concert/confirmation/' + data.data.ConcertEventReservationId;
            },
            error: function (xhr, status, error) {
                $('.loader').addClass('hidden');
                ShowErrorMessage(xhr, status, error);
            }
        });

    };

    self.LoadSeatings = function () {
        $('.loader').removeClass('hidden');
        const root = this;
        var url = webserviceURL + `concerts/${concertId}/events/${eventId}/seatings`;
        $.get(url, function (data) {
            var tables = $.map(data.data.Seatings, function (item) {
                return new Seating(item)
            });
            self.Seatings(tables);
            self.EnableTimeExpiration(data.data.EnableTimeExpiration);
            self.SeatMap(data.data.SeatMapFullPath);
            self.LimitSize(data.data.MaxGroupSize == 0 ? 1 : data.data.MaxGroupSize);
            $('#size1').parent().addClass('active');
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
        }).always(function () {
            $('.loader').addClass('hidden');
        });
    }

    self.LoadSeatings();

}


function Guest(data) {
    var $guest = this;
    $guest.Name = ko.observable(data.Name);
    $guest.MobileNo = ko.observable(data.MobileNo);
    $guest.Email = ko.observable(data.Email);
    $guest.SeatNo = ko.observable(data.SeatNo);
    $guest.SeatName = ko.observable(data.SeatName);
    $guest.IsMainGuest = ko.observable(data.IsMainGuest);
    $guest.SpotValue = ko.observable(data.SeatNo);
    $guest.OnChangeSpot = function (data, event) {
        if (event) {
            var spots = $(event.currentTarget).siblings('select.spots');
            $guest.SeatName($(event.currentTarget).find('option:selected').text())
            spots.empty();
            spots.append(new Option("Choose...", ""));
            for (var i = 1; i <= $guest.SpotValue(); i++) {
                spots.append(new Option(i, i));
            }
        }
    };
    $guest.OnSeatSelect = function (data, event) {
        if (reservationViewModel.EnableTimeExpiration() && event) {
            $('.loader').removeClass('hidden');
            $guest.SeatNo($(event.currentTarget).find('option:selected').text());
            $.get(
                webserviceURL + `concerts/${id}/events/${id}/seatlocks/${data.SeatName() + '-' + data.SeatNo()}`,
                function (response, status) {
                }).always(function () {
                    $('.loader').addClass('hidden');
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    ShowErrorMessage(jqXHR, textStatus, errorThrown);
                });
        }
    }
}

function Seating(data) {
    var $spot = this;
    $spot.Name = ko.observable(data.Name);
    $spot.Spots = ko.observable(data.Spots);
    $spot.SeatsPerSpot = ko.observable(data.SeatsPerSpot);
    $spot.CheckInTime = ko.observable(data.CheckInTime);
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