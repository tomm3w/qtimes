var isAdd = null;
var addBtn = $("#addBtn");
$("[name='my-checkbox']").bootstrapSwitch();
var addWaitList = $("#add-waitlist");

addBtn.click(function () {
    PrepareAddMsg();
});

function ReadyMessage(ReadyMessageId, RestaurantChainId, ReadyMessage1, IsEnabled, IsDeleted) {
    var $message = this;
    $message.ReadyMessageId = ko.observable(ReadyMessageId);
    $message.RestaurantChainId = ko.observable(RestaurantChainId);
    $message.ReadyMessage1 = ko.observable(ReadyMessage1);
    $message.IsEnabled = ko.observable(IsEnabled);
    $message.IsDeleted = ko.observable(IsDeleted);
    $message.IsMsgEnabled = ko.computed(function () {
        if ($message.IsEnabled())
            return 'ON';
        else
            return 'OFF';
    });
    $message.OnOff = function (message) {
        if (message.IsEnabled()) {
            message.IsEnabled(false);
        }
        else {
            message.IsEnabled(true);
        }

    };
    $message.Delete = function (message) {
        if (message.VisitMessageId() != 0)
            message.IsDeleted(true);
        else
            messageListViewModel.ReadyMessages.remove(message);
    };

}
function VisitMessage(VisitMessageId, RestaurantChainId, Visit, VisitMessage1, IsEnabled, IsDeleted) {
    var $message = this;
    $message.VisitMessageId = ko.observable(VisitMessageId);
    $message.RestaurantChainId = ko.observable(RestaurantChainId);
    $message.Visit = ko.observable(Visit);
    $message.VisitMessage1 = ko.observable(VisitMessage1);
    $message.IsEnabled = ko.observable(IsEnabled);
    $message.IsDeleted = ko.observable(IsDeleted);
    $message.IsMsgEnabled = ko.computed(function () {
        if ($message.IsEnabled())
            return 'ON';
        else
            return 'OFF';
    });
    $message.OnOff = function (message) {
        if (message.IsEnabled()) {
            message.IsEnabled(false);
        }
        else {
            message.IsEnabled(true);
        }

    };
    $message.Delete = function (message) {
        if (message.VisitMessageId() != 0)
            message.IsDeleted(true);
        else
            messageListViewModel.VisitMessages.remove(message);
    };
}

$('#chknew').on('switchChange.bootstrapSwitch', function (event, state) {
    messageListViewModel.IsEnabled(state);
});

function MessageListViewModel() {
    var self = this;
    self.ReadyMessages = ko.observableArray([]);
    self.VisitMessages = ko.observableArray([]);
    self.ReadyMessageId = ko.observable();
    self.VisitMessageId = ko.observable();
    self.ReadyMessage = ko.observable();
    self.VisitMessage = ko.observable();
    self.IsEnabled = ko.observable();
    self.Visit = ko.observable();
    self.MessageType = ko.observable();

    self.EditReadyMessage = function (data) {
        PrepareEditMsg();
        self.MessageType('Ready');
        self.ReadyMessageId(data.ReadyMessageId());
        self.ReadyMessage(data.ReadyMessage1());
        self.IsEnabled(data.IsEnabled());
        if (data.IsEnabled())
            $('#chkedit').bootstrapSwitch('state', true, true);
        else
            $('#chkedit').bootstrapSwitch('state', false, false);

        countChar('txtReadyMessage', 'spanReady');
    };

    self.EditVisitMessage = function (data) {
        PrepareEditMsg();
        self.MessageType('Visit');
        self.VisitMessageId(data.VisitMessageId());
        self.VisitMessage(data.VisitMessage1());
        self.Visit(data.Visit());
        if (data.IsEnabled())
            $('#chknew').bootstrapSwitch('state', true, true);
        else
            $('#chknew').bootstrapSwitch('state', false, false);

        countChar('txtVisitMessage', 'spanVisit')
    };

    self.DeleteVisitMessage = function (data) {
        if (window.confirm('Are you sure to delete this custom message?')) {
            $.ajax({
                type: "DELETE",
                url: webserviceURL + "messages/visit?VisitMessageId=" + data.VisitMessageId(),
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + $.cookie("token")
                },
                xhrFields: {
                    withCredentials: true
                }
            })
                .done(function (data) {
                    if (data.status == "ok") {
                        NotifySavedMessage("Successfully deleted.");
                        ClearAll();
                        self.Load();
                    }
                    else if (data.status == "badrequest") {
                        showErrorMessage(data);
                    }
                }).error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
    };

    self.Save = function () {
        if (isAdd) {
            $.ajax({
                type: "POST",
                url: webserviceURL + "messages/visit",
                data: ko.toJSON({ RestaurantChainId: rcid, VisitMessage1: self.VisitMessage(), Visit: self.Visit(), IsEnabled: self.IsEnabled() }),
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + $.cookie("token")
                },
                xhrFields: {
                    withCredentials: true
                }
            })
                .done(function (data) {
                    if (data.status == "ok") {
                        NotifySavedMessage("Successfully saved.");
                        ClearAll();
                        self.Load();
                    }
                    else if (data.status == "badrequest") {
                        showErrorMessage(data);
                    }
                }).error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
        else if (isAdd == false) {
            var url, data;
            if (self.MessageType() === "Visit") {
                url = webserviceURL + "messages/visit";
                data = ko.toJSON({ RestaurantChainId: rcid, VisitMessageId: self.VisitMessageId(), VisitMessage1: self.VisitMessage(), Visit: self.Visit(), IsEnabled: self.IsEnabled() });
            }
            else if (self.MessageType() === "Ready") {
                url = webserviceURL + "messages/ready";
                data = ko.toJSON({ RestaurantChainId: rcid, ReadyMessage1: self.ReadyMessage(), ReadyMessageId: self.ReadyMessageId(), IsEnabled: self.IsEnabled() });
            }
            else
                return;


            $.ajax({
                type: "PUT",
                url: url,
                data: data,
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + $.cookie("token")
                },
                xhrFields: {
                    withCredentials: true
                }
            })
                .done(function (data) {
                    if (data.status == "ok") {
                        NotifySavedMessage("Successfully updated.");
                        ClearAll();
                        self.Load();
                    }
                    else if (data.status == "badrequest") {
                        showErrorMessage(data);
                    }
                }).error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
    };

    self.Load = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + 'messages/?RestaurantChainId=' + rcid,
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
        })
            .done(function (data) {
                if (data.status == "ok") {
                    self.ReadyMessages.removeAll();
                    for (var i = 0; i < data.data.ReadyMessage.length; i++) {
                        var message = data.data.ReadyMessage[i];
                        self.ReadyMessages.push(new ReadyMessage(message.ReadyMessageId, message.RestaurantChainId, message.ReadyMessage1, message.IsEnabled, message.IsDeleted));
                    }

                    self.VisitMessages.removeAll();
                    for (var i = 0; i < data.data.VisitMessage.length; i++) {
                        var message = data.data.VisitMessage[i];
                        self.VisitMessages.push(new VisitMessage(message.VisitMessageId, message.RestaurantChainId, message.Visit, message.VisitMessage1, message.IsEnabled, message.IsDeleted));
                    }

                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            });
    };

    self.Load();
}

var messageListViewModel = new MessageListViewModel();
ko.applyBindings(messageListViewModel, document.getElementById("content-wrapper"));


function PrepareAddMsg() {
    $("#myModalLabel").text("New Message");
    $("#btnSubmit").val("ADD & SAVE");
    isAdd = true;
    countChar('txtVisitMessage', 'spanVisit')
}
function PrepareEditMsg() {
    addWaitList.modal('show');
    $("#myModalLabel").text("Edit Message");
    $("#btnSubmit").val("UPDATE");
    isAdd = false;
    countChar('txtVisitMessage', 'spanVisit')
}

function ClearAll() {
    addWaitList.modal('hide');
    isAdd = null;
    $("#btnSubmit").removeAttr("disabled");
    messageListViewModel.ReadyMessageId(undefined);
    messageListViewModel.ReadyMessage(undefined);
    messageListViewModel.VisitMessage(undefined);
    messageListViewModel.IsEnabled(undefined);
    messageListViewModel.Visit(undefined);
    messageListViewModel.MessageType(undefined);
}

function countChar(value, label) {
    var txt = $('#' + value);
    var len = txt.val().length;
    if (len >= 160) {
        txt.val(txt.val().substring(0, 160)).change();
        $('#' + label).text(0);
    }
    else {
        $('#' + label).text(160 - len);
    }
};