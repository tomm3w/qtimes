$('body').addClass('reservation_body concert__event concert_events');
$('.event-icon').parent().parent().addClass('active');
$('.event-icon').addClass('show');

var eventViewModel = new EventViewModel();
ko.applyBindings(eventViewModel);

function EventViewModel() {
    var self = this;
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Date = ko.observable();
    self.TimeFrom = ko.observable();
    self.TimeTo = ko.observable();
    self.Events = ko.observableArray([]);
    self.Mode = ko.observable('ADD');
    self.ConcertId = ko.computed(function () {
        return $.cookie("concertid");
    });

    self.SearchText = ko.observable('');
    //self.EventDate = ko.observable(moment().format('YYYY-MM-DD'));
    self.FilterBy = ko.observable('All');

    self.Load = function () {
        ShowLoader();
        //List all events, no filter by date
        //var qs = '?EventDate=' + self.EventDate();
        var qs = '?FilterBy=' + self.FilterBy();
        if (self.SearchText())
            qs = qs + '&SearchText=' + self.SearchText();

        $.ajax({
            type: 'GET',
            url: webserviceURL + `concerts/${self.ConcertId()}/events` + qs,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var tables = $.map(data.data.Model, function (item) {
                    return new ConcertEvent(item)
                });
                self.Events(tables);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.Save = function () {
        ShowLoader();

        if (self.Mode() === 'ADD') {
            $.ajax({
                type: 'POST',
                url: webserviceURL + `concerts/${self.ConcertId()}/events`,
                data: {
                    Name: self.Name(),
                    Date: self.Date(),
                    TimeFrom: self.TimeFrom(),
                    TimeTo: self.TimeTo(),
                    ConcertId: self.ConcertId()
                },
                success: function (data) {
                    $.notify("Event added successfully.", "success");
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
        }
        else {
            $.ajax({
                type: 'PUT',
                url: webserviceURL + `concerts/${self.ConcertId()}/events/${self.Id()}`,
                data: {
                    Id: self.Id(),
                    Name: self.Name(),
                    Date: self.Date(),
                    TimeFrom: self.TimeFrom(),
                    TimeTo: self.TimeTo(),
                    ConcertId: self.ConcertId(),
                },
                success: function (data) {
                    $.notify("Event updated successfully.", "success");
                    $('#add-new-reserve_id').modal('hide');
                    self.ClearAll();
                    HideLoader();
                    self.Load();
                },
                error: function (xhr, status, error) {
                    HideLoader();
                    ShowErrorMessage(xhr, status, error);
                }
            });
        }
    }

    self.AddEvent = function (data) {
        self.ClearAll();
        self.Mode('ADD');
        $('#add-new-reserve_id').modal('show');
    }

    self.EditEvent = function (data) {
        self.ClearAll();
        self.Mode('UPDATE');
        $('#add-new-reserve_id').modal('show');
        self.Id(data.Id());
        self.Name(data.Name());
        self.Date(data.Date());
        self.TimeFrom(data.TimeFrom());
        self.TimeTo(data.TimeTo());
    }

    self.DeleteEvent = function (data) {
        if (confirm(`Are you sure you want to delete this event "${data.Name()}"?`)) {
            ShowLoader();
            $.ajax({
                type: 'DELETE',
                url: webserviceURL + `concerts/${self.ConcertId()}/events/${data.Id()}`,
                success: function (data) {
                    self.Load();
                    HideLoader();
                    $.notify(`Concert event "${data.Name()}" deleted successfully.`, "success");
                },
                error: function (xhr, status, error) {
                    HideLoader();
                    ShowErrorMessage(xhr, status, error);
                }
            });
        }
    }

    self.ClearAll = function () {
        self.Id('');
        self.Name('');
        self.Date('');
        self.TimeFrom('');
        self.TimeTo('');
        self.Mode('ADD');
    };

    self.OnChangeEvent = function (data, event) {
        self.FilterBy($('#ddFilterBy').val());
        self.Load();
    };

    self.OnChangeDate = function () {
        var date = event.target.value;
        self.EventDate(date);
        self.Load();
    };

    $(".search_input").on("keydown", function (event) {
        if (event.which === 13)
            self.Load();
    });

    self.Load();
}

function ConcertEvent(data) {
    var $res = this;
    $res.Id = ko.observable(data.Id);
    $res.Name = ko.observable(data.Name);
    $res.Date = ko.observable(data.EventDate);
    $res.DateFormated = ko.observable(moment(new Date(data.EventDate)).format('MM/DD/YYYY'));
    $res.TimeFrom = ko.observable(data.TimeFrom);
    $res.TimeFromFormated = ko.observable(moment('1990-01-01 ' + data.TimeFrom).format("hh:mm A"));
    $res.TimeTo = ko.observable(data.TimeTo);
    $res.TimeToFormated = ko.observable(moment('1990-01-01 ' + data.TimeTo).format("hh:mm A"));
    $res.Preferences = function () {
        location.href = '/concert/events/preferences/' + data.Id;
    }
    $res.Seatings = function () {
        location.href = '/concert/events/seatings/' + data.Id;
    }
    $res.Preferences = function () {
        location.href = '/concert/events/preferences/' + data.Id;
    }
    $res.Business = function () {
        location.href = '/concert/events/business/' + data.Id;
    }
}