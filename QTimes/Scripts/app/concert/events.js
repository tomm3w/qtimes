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

    self.SearchText = ko.observable('');
    self.EventDate = ko.observable(moment().format('YYYY-MM-DD'));

    self.Load = function () {
        ShowLoader();
        var qs = '?EventDate=' + self.EventDate();
        if (self.SearchText())
            qs = qs + '&SearchText=' + self.SearchText()

        $.ajax({
            type: 'GET',
            url: webserviceURL + "concertevent/" + qs,
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
                url: webserviceURL + "concertevent",
                data: {
                    Name: self.Name(),
                    Date: self.Date(),
                    TimeFrom: self.TimeFrom(),
                    TimeTo: self.TimeTo()
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
                url: webserviceURL + "concertevent",
                data: {
                    Id: self.Id(),
                    Name: self.Name(),
                    Date: self.Date(),
                    TimeFrom: self.TimeFrom(),
                    TimeTo: self.TimeTo()
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

    self.ClearAll = function () {
        self.Id('');
        self.Name('');
        self.Date('');
        self.TimeFrom('');
        self.TimeTo('');
        self.Mode('ADD');
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
}