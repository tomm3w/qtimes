

var reservationViewModel = new ReservationViewModel();
ko.applyBindings(reservationViewModel);

function ReservationViewModel() {
    var self = this;
    self.Guests = ko.observableArray([]);
    self.Seatings = ko.observableArray([]);
    self.Reservations = ko.observableArray([]);
    self.pagingdata = ko.observable(new PagingData({ Page: 1, PageSize: 100, TotalData: 0, TotalPages: 0 }));
    self.SearchText = ko.observable('');

    self.Load = function () {
        ShowLoader();
        var qs = '?Page=' + self.pagingdata().Page() + '&PageSize=' + self.pagingdata().PageSize;
        qs = qs + '&ConcertId=' + id;
        if (self.SearchText())
            qs = qs + '&SearchText=' + self.SearchText()

        $.ajax({
            type: 'GET',
            url: webserviceURL + "concertreservation/checkinguest/" + qs,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var tables = $.map(data.data.Model, function (item) {
                    return new Reservation(item)
                });
                self.Reservations(tables);

                var pagingdata = new PagingData(data.data.PagingModel);
                self.pagingdata(pagingdata);

                $('.plus_more_f').click(function () {
                    $('.plus_more_f').show();
                    $('.minus_less_f').hide();
                    $(this).hide();
                    $('.head_tr').removeClass('show');
                    $('.togg').removeClass('show');
                    $(this).parents('.head_tr').next('tr.togg').addClass('show');
                    $(this).parents('.head_tr').closest("tr.togg").addClass('show');
                    $(this).parents('.head_tr').find('.minus_less_f').show();
                });

                $('.minus_less_f').click(function () {
                    $(this).hide();
                    $('.togg.show').removeClass('show');
                    $(this).parents('.head_tr').find('.plus_more_f').show();
                });

                $(".big_grn").click(function () {
                    $(this)
                        .parents("li")
                        .find(".rt_input_gr_txt")
                        .toggleClass("activation");
                });

                HideLoader();

            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.AddReservation = function (data) {
        ShowLoader();

        $.ajax({
            type: 'PUT',
            url: webserviceURL + "concertreservation/updatecheckin",
            data: {
                ConcertGuestId: data.ConcertGuestId(),
                ConcertReservationId: data.ConcertReservationId(),
                Temperature: data.Temperature(),
                IsCheckedIn: data.IsCheckedIn(),
                IsSolo: data.IsSolo()
            },
            success: function (response) {
                HideLoader();
                $.notify("Reservation updated successfully.", "success");
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });

    };

    $(".search_input").on("keydown", function (event) {
        if (event.which === 13)
            self.Load();
    });

    self.Load();
}


function PagingData(data) {
    var $super = this;
    this.Page = ko.observable(data.Page);
    this.PageSize = data.PageSize;
    this.TotalData = data.TotalData;
    this.TotalPages = ko.observable(data.TotalPages);
    this.HasPrev = ko.computed(function () {
        return $super.Page() > 1;
    });
    this.HasNext = ko.computed(function () {
        return $super.Page() < $super.TotalPages();
    });
    this.GoToPage = function (p) {
        $super.Page(p);
        reservationViewModel.LoadList();
    };
}

function Reservation(data) {
    var $res = this;
    $res.Id = ko.observable(data.Id);
    $res.GuestTypeId = ko.observable(data.GuestTypeId);
    $res.Size = ko.observable(data.Size);
    $res.MainGuestName = ko.observable(data.MainGuestName);
    $res.GuestType = ko.observable(data.GuestType);
    $res.DOB = ko.observable(data.DOB);
    $res.MainGuestMobileNo = ko.observable(data.MainGuestMobileNo);
    $res.MainGuestAddress = ko.observable(data.MainGuestAddress);
    $res.MainGuestCity = ko.observable(data.MainGuestCity);
    $res.MainGuestState = ko.observable(data.MainGuestState);
    $res.MainGuestZip = ko.observable(data.MainGuestZip);
    $res.MainGuestEmail = ko.observable(data.MainGuestEmail);
    $res.SeatNo = ko.observable(data.SeatNo);
    $res.ConfirmationNo = ko.observable(data.ConfirmationNo);
    $res.ConcertGuestId = ko.observable(data.ConcertGuestId);
    $res.CheckInTime = ko.computed(function () {
        return moment(new Date(data.CheckInTime), "HH:mm:ss").format("hh:mm A");
    });
    $res.PhotoFullPath = ko.computed(function () {
        if (data.Photo)
            return data.PhotoFullPath;
        else
            return '/content/assets/images/staff_grey.png';
    });

    $res.Guests = ko.computed(function () {
        var tables = $.map(data.Guests, function (item) {
            return new Guest(item)
        });
        return tables;
    });

}

function Guest(data) {
    var $guest = this;
    $guest.ConcertReservationId = ko.observable(data.ConcertReservationId);
    $guest.ConcertGuestId = ko.observable(data.ConcertGuestId);
    $guest.IsCheckedIn = ko.observable(data.IsCheckedIn);
    $guest.Temperature = ko.observable(data.Temperature);
    $guest.Name = ko.observable(data.Name);
    $guest.MobileNo = ko.observable(data.MobileNo);
    $guest.Email = ko.observable(data.Email);
    $guest.SeatNo = ko.observable(data.SeatNo);
    $guest.IsMainGuest = ko.observable(data.IsMainGuest);
    $guest.IsSolo = ko.observable(data.IsSolo);
    $guest.CheckInTime = ko.computed(function () {
        if (data.CheckInTime)
            return moment(new Date(data.CheckInTime)).format("MM/DD/YYYY hh:mm A");
        else
            return "";
    });
}

function HideLoader() {
    $('.loader').addClass('hidden');
}

function ShowLoader() {
    $('.loader').removeClass('hidden');
}