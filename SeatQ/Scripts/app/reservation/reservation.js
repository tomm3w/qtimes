$('body').addClass('reservation_body');
$('.resv-icon').parent().parent().addClass('active');
$('.resv-icon').addClass('show');

ShowLoader();

var reservationViewModel = new ReservationViewModel();
ko.applyBindings(reservationViewModel);

function ReservationViewModel() {
    var self = this;

    self.Id = ko.observable();
    self.GuestTypeId = ko.observable();
    self.GuestName = ko.observable();
    self.Size = ko.observable();
    self.Comments = ko.observable();
    self.MobileNumber = ko.observable();
    self.SelectedDate = ko.observable();
    self.TimeFrom = ko.observable();
    self.TimeTo = ko.observable();
    self.Reservations = ko.observableArray([]);
    self.pagingdata = ko.observable(new PagingData({ Page: 1, PageSize: 100, TotalData: 0, TotalPages: 0 }));
    self.SearchText = ko.observable('');
    self.ReservationDate = ko.observable(moment().format('YYYY-MM-DD'));
    self.Message = ko.observable();
    self.ReservationGuestId = ko.observable();
    self.MyGuestName = ko.observable();
    self.MyReservationId = ko.observable();
    self.GuestMessages = ko.observableArray([]);
    self.Mode = ko.observable('NEW');
    self.Times = ko.observableArray([]);

    self.Load = function () {
        ShowLoader();
        var qs = '?Page=' + self.pagingdata().Page() + '&PageSize=' + self.pagingdata().PageSize;
        qs = qs + '&ReservationDate=' + self.ReservationDate();
        if (self.SearchText())
            qs = qs + '&SearchText=' + self.SearchText()

        $.ajax({
            type: 'GET',
            url: webserviceURL + "reservation/" + qs,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var tables = $.map(data.data.Model, function (item) {
                    return new Reservation(item)
                });
                self.Reservations(tables);
                var pagingdata = new PagingData(data.data.PagingModel);
                self.pagingdata(pagingdata);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.LoadTimeSlots = function () {

        $.ajax({
            type: 'POST',
            url: webserviceURL + "reservation/gettimeslots",
            data: { Date: moment().format('YYYY-MM-DD') },
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

    self.AddReservation = function () {
        ShowLoader();
        if (self.Mode() === 'NEW') {
            $.ajax({
                type: 'POST',
                url: webserviceURL + "reservation",
                data: {
                    Name: self.GuestName(),
                    MobileNo: self.MobileNumber(),
                    GuestTypeId: self.GuestTypeId(),
                    Size: self.Size(),
                    Date: self.SelectedDate(),
                    TimeFrom: self.TimeFrom(),
                    TimeTo: self.TimeTo(),
                    Comments: self.Comments()
                },
                success: function (data) {
                    $.notify("Reservation added successfully.", "success");
                    $('#add-new-reserve_id').modal('hide');
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
                url: webserviceURL + "reservation",
                data: {
                    Id: self.Id(),
                    Name: self.GuestName(),
                    MobileNo: self.MobileNumber(),
                    GuestTypeId: self.GuestTypeId(),
                    Size: self.Size(),
                    Date: self.SelectedDate(),
                    TimeFrom: self.TimeFrom(),
                    TimeTo: self.TimeTo(),
                    Comments: self.Comments(),
                    ReservationGuestId: self.ReservationGuestId()
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

    self.TimeInReservation = function (id) {
        ShowLoader();
        $.ajax({
            type: 'POST',
            url: webserviceURL + "/reservation/TimeIn",
            data: {
                Id: id
            },
            success: function (data) {
                $.notify("Reservation Time In.", "success");
                HideLoader();
                self.Load();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.TimeOutReservation = function (id) {
        ShowLoader();
        $.ajax({
            type: 'POST',
            url: webserviceURL + "/reservation/TimeOut",
            data: {
                Id: id
            },
            success: function (data) {
                $.notify("Reservation Time Out.", "success");
                HideLoader();
                self.Load();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.CancelReservation = function (id) {
        ShowLoader();
        $.ajax({
            type: 'POST',
            url: webserviceURL + "/reservation/cancel",
            data: {
                Id: id
            },
            success: function (data) {
                $.notify("Reservation cancelled successfully.", "success");
                HideLoader();
                self.Load();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
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

    self.SetSizeValue = function (data, event) {
        self.Size($(event.currentTarget).text());
        $(event.currentTarget).parent().siblings().removeClass('active');
        $(event.currentTarget).parent().addClass('active');
    }

    self.OnChangeDate = function () {
        var year = $('#select_yrs').val();
        var month = $('#select_month').val();
        var day = $('#select_date').val();
        self.ReservationDate(year + '-' + month + '-' + day);
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
        self.GuestName(data.GuestName());
        self.Size(data.Size());
        $('#size' + data.Size()).parent().siblings().removeClass('active');
        $('#size' + data.Size()).parent().addClass('active');
        self.Comments(data.Comments());
        self.MobileNumber(data.GuestMobileNumber());
        self.TimeFrom(data.TimeFrom());
        self.TimeTo(data.TimeTo());
        self.SelectedDate(moment(data.Date()).format('YYYY-MM-DD'));
        self.ReservationGuestId(data.ReservationGuestId());
        //$('.selectpicker').selectpicker('refresh');
        //$('#datepicker').datepicker('refresh');

        var chkTime = data.TimeFrom() + '-' + data.TimeTo();
        $('.timeslots').siblings().removeClass('active');
        $('.form_fifth').find(':checkbox[value="' + chkTime + '"]').parent().addClass('active');
        $('.timeslots').find('.input_chk').removeClass('tick');
        $('.form_fifth').find(':checkbox[value="' + chkTime + '"]').parent().find('.input_chk').addClass('tick');
        HideLoader();
    }

    self.ShowMsg = function (data) {
        ShowLoader();
        $.ajax({
            type: "POST",
            url: webserviceURL + "reservation/GetMessages",
            data: { ReservationId: data.Id() },
            success: function (response) {
                self.MyReservationId(data.Id());
                self.MyGuestName(data.GuestName());
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
            url: webserviceURL + "reservation/Message",
            data: { Id: self.MyReservationId(), MessageSent: self.Message() },
            success: function (response) {
                $("#txtReply").val('');
                var obj = {};
                obj['ReservationId'] = self.MyReservationId();
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
        self.Size('');
        self.Comments('');
        self.MobileNumber('');
        self.SelectedDate();
        self.TimeFrom();
        self.TimeTo();
        self.SearchText('');
        self.ReservationDate = ko.observable(moment().format('YYYY-MM-DD'));
        self.Message('');
        self.MyGuestName('');
        self.MyReservationId('');
        self.Mode = ko.observable('NEW');
        $('.timeslots').removeClass('active');
        $('.timeslots').find('.input_chk').removeClass('tick');
    };

    //$('#datepicker').datepicker("setDate", moment().format('YYYY-MM-DD'));
    //$('#datepicker').datepicker("refresh");
    self.Load();
    self.LoadTimeSlots();

    setInterval(function () {
        self.Load();
    }, 60 * 1000);

}

function Reservation(data) {
    var $res = this;
    $res.Id = ko.observable(data.Id);
    $res.ReservationGuestId = ko.observable(data.ReservationGuestId);
    $res.GuestTypeId = ko.observable(data.GuestTypeId);
    $res.Size = ko.observable(data.Size);
    $res.Date = ko.observable(data.Date);
    $res.TimeFrom = ko.observable(data.TimeFrom);
    $res.TimeTo = ko.observable(data.TimeTo);
    $res.TimeIn = ko.computed(function () {
        if (data.TimeIn)
            return moment(new Date(data.UniversalDateTimeIn), "HH:mm:ss").format("hh:mm A");
    });
    $res.TimeOut = ko.computed(function () {
        if (data.TimeOut)
            return moment(new Date(data.UniversalDateTimeOut), "HH:mm:ss").format("hh:mm A");
    });
    $res.Comments = ko.observable(data.Comments);
    $res.GuestName = ko.observable(data.GuestName);
    $res.GuestType = ko.observable(data.GuestType);
    $res.GuestMobileNumber = ko.observable(data.GuestMobileNumber);
    $res.IsCancelled = ko.observable(data.IsCancelled);
    $res.GuestTypeCss = ko.computed(function () {
        if (data.GuestTypeId === 1)
            return "callin_check";
        else if (data.GuestTypeId === 2)
            return "walkin_check";
        else if (data.GuestTypeId === 2)
            return "mob_check";
    });
    $res.Reserved = ko.computed(function () {
        if (data.ReservedMinutes) {
            var totMinutes = data.ReservedMinutes;
            if (totMinutes > 59) {
                var hours = totMinutes / 60 + " hours";
                return hours;
            }
            else
                return totMinutes + " min";
        }
        else
            return "0 min";
    });
    $res.Status = ko.computed(function () {
        if (data.IsCancelled !== true) {
            if (data.TimeIn == null)
                return "Waiting";
            else if (data.TimeIn && !data.IsTimeOut) {
                return "In place";
            }
            else if (data.TimeIn && data.IsTimeOut) {
                return "Time out";
            }
        }
        else
            return "Cancelled";
    });
    $res.StatusCss = ko.computed(function () {
        if (data.IsCancelled !== true) {
            if (data.TimeIn == null)
                return "orange_bg";
            else if (data.TimeIn && !data.IsTimeOut) {
                return "blue_bg";
            }
            else if (data.TimeIn && data.IsTimeOut) {
                return "pink_bg";
            }
        }
        else
            return "pink_bg";
    });
    $res.IsTimeIn = ko.computed(function () {
        if (data.TimeIn == null && data.TimeOut == null && data.IsCancelled != true)
            return true;
        else if (data.TimeIn != null && data.TimeOut == null && data.IsCancelled != true)
            return false;
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
    $message.ReservationId = ko.observable(data.ReservationId);
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