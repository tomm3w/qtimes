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

    self.GuestTypeId = ko.observable();
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
    self.SeatMap = ko.observable();

    self.SetSizeValue = function (data, event) {
        self.Size($(event.currentTarget).text());

        $(event.currentTarget).parent().siblings().removeClass('active');
        $(event.currentTarget).parent().addClass('active');
        self.AddGuests(self.Size() - 1);
        $('.selectpicker').selectpicker('refresh');
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

        for (var i = 1; i <= self.SpotValue(); i++) {
            spots.append(new Option(i, i));
        }

        self.SeatNo(1);

    };

    self.AddReservation = function () {

        $.ajax({
            type: 'POST',
            url: webserviceURL + "concertreservation/addconcertreservation",
            data: {
                ConcertId: id,
                GuestTypeId: self.GuestTypeId(),
                Size: self.Size(),
                Guests: self.Guests(),
                MainGuestName: self.GuestName(),
                MainGuestMobileNo: self.MobileNumber(),
                MainGuestEmail: self.Email(),
                MainGuestSeatName: self.SpotName(),
                MainGuestSeatNo: self.SeatNo()
            },
            success: function (data) {
                $.notify("Reservation added successfully.", "success");
                window.location.href = '/m/reservation/thankyou/' + id;
            },
            error: function (xhr, status, error) {
                ShowErrorMessage(xhr, status, error);
            }
        });

    };

    self.LoadSeatings = function () {
        $.post(webserviceURL + "concertreservation/GetConcertSeatings", { ConcertId: id }, function (data) {
            var tables = $.map(data.data.Seatings, function (item) {
                return new Seating(item)
            });
            self.Seatings(tables);
            self.SeatMap(data.data.SeatMapFullPath);
            self.LimitSize(data.data.MaxGroupSize);
            $('#size1').parent().addClass('active');
        }).always(function () {
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
            for (var i = 1; i <= $guest.SpotValue(); i++) {
                spots.append(new Option(i, i));
            }
            $('.selectpicker').selectpicker('refresh');
        }
    };
}

function Seating(data) {
    var $spot = this;
    $spot.Name = ko.observable(data.Name);
    $spot.Spots = ko.observable(data.Spots);
    $spot.SeatsPerSpot = ko.observable(data.SeatsPerSpot);
    $spot.CheckInTime = ko.observable(data.CheckInTime);
}