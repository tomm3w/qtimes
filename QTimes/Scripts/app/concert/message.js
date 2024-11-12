$('body').addClass('message2_body concert__event concert_message_body');
$('.message-icon').parent().parent().addClass('active');
$('.message-icon').addClass('show');

$('#MessageType').on("change", function () {
    if ($(this).val() === "Welcome") {
        $('.form_third').hide();
        $('.noticep').hide();
    }
    else {
        $('.form_third').show();
        $('.noticep').show();
    }
});

var messageViewModel = new MessageViewModel();
ko.applyBindings(messageViewModel);

function MessageViewModel() {
    var self = this;

    self.Id = ko.observable();
    self.MessageType = ko.observable();
    self.Value = ko.observable();
    self.ValueType = ko.observable();
    self.BeforeAfter = ko.observable();
    self.InOut = ko.observable();
    self.Message = ko.observable();
    self.ReservationMessages = ko.observableArray([]);
    self.Mode = ko.observable('ADD');
    self.ConcertId = ko.computed(function () {
        return $.cookie("concertid");
    });

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "concertmessagerule/" + self.ConcertId(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var tables = $.map(data.data.Model, function (item) {
                    return new ReservationMessage(item)
                });
                self.ReservationMessages(tables);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.AddMessageRule = function () {
        ShowLoader();
        if (self.MessageType() === "Welcome")
            self.Value(0);

        if (self.Mode() === 'ADD') {
            $.ajax({
                type: 'POST',
                url: webserviceURL + "concertmessagerule",
                data: {
                    MessageType: self.MessageType(),
                    Value: self.Value(),
                    ValueType: self.ValueType(),
                    BeforeAfter: self.BeforeAfter(),
                    InOut: self.InOut(),
                    Message: self.Message(),
                    ConcertId: self.ConcertId()
                },
                success: function (data) {
                    $.notify("Reservation message rule added successfully.", "success");
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
                url: webserviceURL + "concertmessagerule",
                data: {
                    Id: self.Id(),
                    MessageType: self.MessageType(),
                    Value: self.Value(),
                    ValueType: self.ValueType(),
                    BeforeAfter: self.BeforeAfter(),
                    InOut: self.InOut(),
                    Message: self.Message(),
                    ConcertId: self.ConcertId()
                },
                success: function (data) {
                    $.notify("Reservation message rule updated successfully.", "success");
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

    self.EditMessageRule = function (data) {
        self.ClearAll();
        self.Mode('UPDATE');
        $('#add-new-reserve_id').modal('show');
        self.Id(data.Id());
        self.MessageType(data.MessageType());
        $('#MessageType').val(data.MessageType());
        self.Value(data.Value());
        self.ValueType(data.ValueType());
        $('#ValueType').val(data.ValueType());
        self.BeforeAfter(data.BeforeAfter());
        $('#BeforeAfter').val(data.BeforeAfter());
        self.InOut(data.InOut());
        $('#InOut').val(data.InOut());
        self.Message(data.Message());
        $('.selectpicker').selectpicker('refresh');
    }

    self.ClearAll = function () {
        self.Id('');
        self.MessageType('');
        self.Value('');
        self.ValueType('');
        self.BeforeAfter('');
        self.InOut('');
        self.Message('');
        self.Mode('ADD');
    };

    self.Load();
}


function ReservationMessage(data) {
    var $res = this;
    $res.Id = ko.observable(data.Id);
    $res.MessageType = ko.observable(data.MessageType);
    $res.Value = ko.observable(data.Value);
    $res.ValueType = ko.observable(data.ValueType);
    $res.BeforeAfter = ko.observable(data.BeforeAfter);
    $res.InOut = ko.observable(data.InOut);
    $res.Message = ko.observable(data.Message);
    $res.Rules = ko.computed(function () {
        if (data.MessageType === "Welcome")
            return "Welcome Message";
        else
            return data.Value + " " + data.ValueType + " " + data.BeforeAfter + " " + data.InOut;
    });
}
