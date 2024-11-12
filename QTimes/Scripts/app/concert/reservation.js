$('body').addClass('reservation_body concert__event concert_reservation_body');
$('.rev-icon').parent().parent().addClass('active');
$('.rev-icon').addClass('show');


//var input = document.querySelector("#mob_id");
//var iti = window.intlTelInput(input, {
//    separateDialCode: true,
//    utilsScript: "/content/intl-tel-input/js/utils.js"
//});

//var input2 = document.querySelector("#mob_id2");
//var iti2 = window.intlTelInput(input, {
//    separateDialCode: true,
//    utilsScript: "/content/intl-tel-input/js/utils.js"
//});

//var input3 = document.querySelector("#mob_id3");
//var iti3 = window.intlTelInput(input, {
//    separateDialCode: true,
//    utilsScript: "/content/intl-tel-input/js/utils.js"
//});

function activeDiv(className) {

    $('.over_images_abs').each(function () {

        var thisEl = $(this);

        if (thisEl.hasClass(className)) {
            thisEl.addClass('active');
        } else {
            thisEl.removeClass('active');
        }
    });
}

$(".reserve_absolute1 .btn").click(function (e) {
    e.preventDefault();
    activeDiv('reserve_absolute2');
});
$(".reserve_absolute2 .btn").click(function (e) {
    e.preventDefault();
    //activeDiv('reserve_absolute3');
});

$(".reserve_absolute3 .backward").click(function (e) {
    e.preventDefault();
    activeDiv('reserve_absolute2');
});

$(".reserve_absolute2 a.front_arrow").click(function (e) {
    e.preventDefault();
    activeDiv('reserve_absolute1');
});


var reservationViewModel = new ReservationViewModel();
ko.applyBindings(reservationViewModel);

function ReservationViewModel() {
    var self = this;

    self.Id = ko.observable();
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
    self.EnableTimeExpiration = ko.observable();

    self.ConcertId = ko.computed(function () {
        return $.cookie("concertid");
    });

    self.AddGuests = function (size) {
        self.Guests([]);
        for (var i = 0; i < size; i++) {
            self.Guests.push(new Guest({ Name: '', MobileNo: '', SeatNo: 1, SeatName: '', IsMainGuest: false }));
        }
    };

    self.SetSizeValue = function (data, event) {
        self.Size($(event.currentTarget).text());

        $(event.currentTarget).parent().siblings().removeClass('active');
        $(event.currentTarget).parent().addClass('active');
        self.AddGuests(self.Size() - 1);
        $('.selectpicker').selectpicker('refresh');
    }

    self.LoadSeatings = function () {
        $.post(webserviceURL + "concertreservation/GetConcertSeatings", { ConcertId: self.ConcertId() }, function (data) {
            var tables = $.map(data.data.Seatings, function (item) {
                return new Seating(item)
            });
            self.EnableTimeExpiration(data.data.EnableTimeExpiration);
            self.Seatings(tables);
            self.SeatMap(data.data.SeatMapFullPath);
            self.LimitSize(data.data.MaxGroupSize);
            $('#size1').parent().addClass('active');
            $('.selectpicker').selectpicker('refresh');
        }).always(function () {
            HideLoader();
        }).fail(function (xhr, status, error) {
            ShowErrorMessage(xhr, status, error);
        });
    }

    self.OnChangeSpot = function (data, event) {
        var spots = $(event.currentTarget).siblings('select.spots');
        self.SpotName($(event.currentTarget).find('option:selected').text());
        spots.empty();

        spots.append(new Option("Choose...", ""));
        for (var i = 1; i <= self.SpotValue(); i++) {
            spots.append(new Option(i, i));
        }

        $('.selectpicker').selectpicker('refresh');
    };

    self.OnSeatSelect = function (data, event) {
        if (self.EnableTimeExpiration()) {
            ShowLoader();
            self.SeatNo($(event.currentTarget).find('option:selected').text());
            $.post(
                webserviceURL + "concertreservation/ValidateSeatLock",
                { ConcertId: self.ConcertId(), SeatNo: data.SpotName() + '-' + data.SeatNo() },
                function (response, status) {
                }).always(function () {
                    HideLoader()
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    ShowErrorMessage(jqXHR, textStatus, errorThrown);
                });
        }
    }

    self.Reservations = ko.observableArray([]);
    self.pagingdata = ko.observable(new PagingData({ Page: 1, PageSize: 100, TotalData: 0, TotalPages: 0 }));
    self.SearchText = ko.observable('');
    self.Message = ko.observable();
    self.ReservationGuestId = ko.observable();
    self.MyGuestName = ko.observable();
    self.MyReservationId = ko.observable();
    self.GuestMessages = ko.observableArray([]);
    self.Mode = ko.observable('NEW');

    self.Load = function (hideloader) {
        if (!hideloader) ShowLoader();
        var qs = '?Page=' + self.pagingdata().Page() + '&PageSize=' + self.pagingdata().PageSize;
        qs = qs + '&ConcertId=' + self.ConcertId();
        if (self.SearchText())
            qs = qs + '&SearchText=' + self.SearchText()

        $.ajax({
            type: 'GET',
            url: webserviceURL + "concertreservation/" + qs,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var tables = $.map(data.data.Model, function (item) {
                    return new Reservation(item)
                });
                self.Reservations(tables);

                var pagingdata = new PagingData(data.data.PagingModel);
                self.pagingdata(pagingdata);
                HideLoader();

                $('.plus_more_f').click(function () {
                    event.preventDefault();
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
                    event.preventDefault();
                    $(this).hide();
                    $('.togg.show').removeClass('show');
                    $(this).parents('.head_tr').find('.plus_more_f').show();
                });
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.AddReservation = function () {
        ShowLoader();

        if (self.Mode() === 'NEW') {
            $.ajax({
                type: 'POST',
                url: webserviceURL + "concertreservation",
                data: {
                    ConcertId: self.ConcertId(),
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
                    $('#add-new-reserve_id').modal('hide');
                    self.ClearAll
                    HideLoader();
                    self.Load();
                },
                error: function (xhr, status, error) {
                    HideLoader();
                    ShowErrorMessage(xhr, status, error);
                }
            });
        }
        else {
            $.ajax({
                type: 'PUT',
                url: webserviceURL + "concertreservation",
                data: {
                    Id: self.Id(),
                    ConcertId: self.ConcertId(),
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
                    $.notify("Reservation updated successfully.", "success");
                    $('#add-new-reserve_id').modal('hide');
                    self.ClearAll()
                    HideLoader();
                    self.Load();
                },
                error: function (xhr, status, error) {
                    HideLoader();
                    ShowErrorMessage(xhr, status, error);
                }
            });
        }
    };

    self.MessageGuest = function (id) {
        ShowLoader();
        $.ajax({
            type: 'POST',
            url: webserviceURL + "/reservation/message",
            data: {
                Id: id,
                MessageSent: self.Message()
            },
            success: function (data) {
                $.notify("Reservation cancelled successfully.", "success");
                self.Load();
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.OnChangeDate = function () {
        //var year = $('#select_yrs').val();
        //var month = $('#select_month').val();
        //var day = $('#select_date').val();
        //self.ReservationDate(year + '-' + month + '-' + day);
        var date = event.target.value;
        self.ReservationDate(date);
        self.Load();
    };

    $(".search_input").on("keydown", function (event) {
        if (event.which === 13)
            self.Load();
    });

    $(".chat-input").on("keydown", function (event) {
        if (event.which === 13)
            self.Reply();
    });

    self.Add = function () {
        self.ClearAll();
        self.LoadTimeSlots();
        $('#add-new-reserve_id').modal('show');
    };

    self.Edit = function (data) {
        ShowLoader();
        self.ClearAll();
        $('#add-new-reserve_id').modal('show');
        self.Mode('UPDATE');
        self.Id(data.Id());
        self.GuestTypeId(data.GuestTypeId());
        $('#GuestTypeId').val(data.GuestTypeId());
        self.GuestName(data.MainGuestName());
        self.MobileNumber(data.MainGuestMobileNo());
        self.Email(data.MainGuestEmail());
        self.Size(data.Size());
        $('#size' + data.Size()).parent().siblings().removeClass('active');
        $('#size' + data.Size()).parent().addClass('active');

        //self.OnChangeSpot(null, $('#ddSeatName'));
        //self.SeatNo(data.SeatNo().split('-')[1]);
        var tables = $.map(data.Guests(), function (item) {
            if (!item.IsMainGuest) {
                return new Guest(item)
            }
        });
        self.Guests(tables);
        $('.selectpicker').selectpicker('refresh');
        HideLoader();
    }

    self.ShowMsg = function (data) {
        ShowLoader();
        $.ajax({
            type: "POST",
            url: webserviceURL + "concertreservation/GetConcertMessages",
            data: { ConcertReservationId: data.Id() },
            success: function (response) {
                self.MyReservationId(data.Id());
                self.MyGuestName(data.MainGuestName());
                self.GuestMessages.removeAll();
                var tables = $.map(response.data.Model, function (item) {
                    return new GuestMessage(item)
                });
                self.GuestMessages(tables);
                $('.chat-overlay, .chat-box').fadeIn();
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.Reply = function () {
        ShowLoader();
        $.ajax({
            type: "POST",
            url: webserviceURL + "concertreservation/ConcertMessage",
            data: { Id: self.MyReservationId(), MessageSent: self.Message() },
            success: function (response) {
                $("#txtReply").val('');
                var obj = {};
                obj['ConcertReservationId'] = self.MyReservationId();
                obj['GuestName'] = 'Admin';
                obj['MessageFrom'] = 'Admin';
                obj['Message'] = linkify(self.Message());
                obj['MessageDate'] = getFormatedDate(new Date());
                obj['MessageTime'] = getFormatedTime(new Date());
                self.GuestMessages.unshift(obj);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.ClearAll = function () {
        self.Id('');
        self.GuestTypeId('');
        self.GuestName('');
        self.Email('');
        self.Size(1);
        self.MobileNumber('');
        self.SearchText('');
        self.Message('');
        self.MyGuestName('');
        self.MyReservationId('');
        self.Mode = ko.observable('NEW');
        self.Guests([]);
        activeDiv('reserve_absolute1');
        $('#size1').parent().addClass('active');
    };

    self.Load();
    self.LoadSeatings();

    setInterval(function () {
        self.Load(true);
    }, 60 * 1000);

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
    $res.CheckInTime = ko.computed(function () {
        return moment(new Date(data.CheckInTime), "HH:mm:ss").format("hh:mm A");
    });
    $res.PhotoFullPath = ko.computed(function () {
        if (data.Photo)
            return data.PhotoFullPath;
        else
            return '/content/assets/images/staff_grey.png';
    });
    $res.GuestTypeCss = ko.computed(function () {
        if (data.GuestTypeId === 1)
            return "callin_check";
        else if (data.GuestTypeId === 2)
            return "walkin_check";
        else if (data.GuestTypeId === 2)
            return "mob_check";
    });

    $res.Status = ko.computed(function () {
        if (data.CheckInTime) {
            if (data.TimeIn == null)
                return "In place";
            else if (data.TimeIn && !data.IsTimeOut) {
                return "Waiting";
            }
        }
    });
    $res.StatusCss = ko.computed(function () {
        if (data.CheckInTime)
            return "status_place";
        else
            return "status_wait";

    });
    $res.Guests = ko.computed(function () {
        var tables = $.map(data.Guests, function (item) {
            return new Guest(item)
        });
        return tables;
    });
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

function GuestMessage(data) {
    var $message = this;
    $message.ConcertReservationId = ko.observable(data.ConcertReservationId);
    $message.Message = ko.observable(data.MessageSent);
    $message.GuestName = ko.observable(data.GuestName);
    $message.MessageSentDateTimeUniversal = ko.observable(data.MessageSentDateTimeUniversal);
    $message.MessageFrom = ko.observable(data.MessageSentBy);
    $message.MessageTime = ko.computed(function () {

        var date = new Date(data.MessageSentDateTimeUniversal);
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        return (hours < 10 ? '0' + hours : hours) + ":" + minutes;

    });

}

function Guest(data) {
    var $guest = this;
    $guest.Name = ko.observable(data.Name);
    $guest.MobileNo = ko.observable(data.MobileNo);
    $guest.Email = ko.observable(data.Email);
    $guest.SeatNo = ko.observable(data.SeatNo);
    $guest.SeatName = ko.observable(data.SeatName);
    $guest.Temperature = ko.observable(data.Temperature);
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
            $('.selectpicker').selectpicker('refresh');
        }
    };
    $guest.OnSeatSelect = function (data, event) {
        if (reservationViewModel.EnableTimeExpiration()) {
            ShowLoader();
            $guest.SeatNo($(event.currentTarget).find('option:selected').text());
            $.post(
                webserviceURL + "concertreservation/ValidateSeatLock",
                { ConcertId: reservationViewModel.ConcertId(), SeatNo: data.SeatName() + '-' + data.SeatNo() },
                function (response, status) {
                }).always(function () {
                    HideLoader()
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    ShowErrorMessage(jqXHR, textStatus, errorThrown);
                });
        }
    }
    $guest.CheckInTime = ko.computed(function () {
        if (data.CheckInTime)
            return moment(new Date(data.CheckInTime)).format("MM/DD/YYYY hh:mm A");
        else
            return "";
    });
}

function Seating(data) {
    var $spot = this;
    $spot.Name = ko.observable(data.Name);
    $spot.Spots = ko.observable(data.Spots);
    $spot.SeatsPerSpot = ko.observable(data.SeatsPerSpot);
    $spot.CheckInTime = ko.observable(data.CheckInTime);
}