var monthNames = [
  "January", "February", "March",
  "April", "May", "June", "July",
  "August", "September", "October",
  "November", "December"
];

function ReturnGuestViewModel() {
    var self = this;
    self.businesses = ko.observableArray([]);
    self.pagingdata = ko.observable(new PagingData({ Page: 1, PageSize: 10, TotalData: 0, TotalPages: 0 }));
    var cookieValue = $.cookie("rcid");
    if (cookieValue) {
        rcid = cookieValue;
    }

    self.GuestId = ko.observable();
    self.GuestName = ko.observable();
    self.NewGuestMessage = ko.observable();
    self.GuestMessages = ko.observableArray([]);

    self.LoadList = function () {

        var qs = '&Page=' + self.pagingdata().Page() + '&PageSize=' + self.pagingdata().PageSize + '&TotalPages=' + self.pagingdata().TotalPages() + '&TotalData=' + self.pagingdata().TotalData;

        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/loyalty?RestaurantChainId=" + rcid + qs,
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
            setData(data);
        }
        else if (data.status == "badrequest") {
            showErrorMessage(data);
        }
    });

    };

    setData = function (data) {
        var pagingdata = new PagingData(data.data.businesses.PagingData);
        self.pagingdata(pagingdata);
        var mappedbusinesses = $.map(data.data.businesses.Businesses, function (item) {
            return new Business(item);
        });
        self.businesses(mappedbusinesses);
    };

    self.SendMessage = function (guest) {
        $.ajax({
            type: "POST",
            url: webserviceURL + "waitlist/SendMessage",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON(guest),
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
    .done(function (data) {
        if (data.status == "ok") {
            if (data.data.Message) {
                NotifyErrorMessage(data.data.Message);
            }
            else {
                if (data.data[0].Status != 0) {
                    NotifyErrorMessage(data.data[0].ErrorText);
                }
                else if (data.data[0].Status == 0) {
                    self.LoadList();
                    NotifySavedMessage('Message sent successfully.');
                }
            }
        }
        if (data.status == "badrequest") {
            showErrorMessage(data);
        }
    })
    };

    self.ShowMsg = function (GuestId, GuestName, UnReadMessageCount) {
        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/LoyaltyMessage/?GuestId=" + GuestId,
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
            $('.chat-overlay, .chat-box').fadeIn();
            self.GuestId(GuestId);
            self.GuestName(GuestName);
            self.NewGuestMessage(undefined);
            self.GuestMessages.removeAll();
            for (var i = 0; i < data.data.GuestMessage.length; i++) {
                var message = data.data.GuestMessage[i];
                self.GuestMessages.push(new GuestMessage(message.WaitListId, message.GuestMessageId, message.Message, message.MessageDateTime, message.GuestName, message.MessageFrom));
            }

            if (UnReadMessageCount > 0)
                self.LoadList();

        }
        else if (data.status == "badrequest") {
            showErrorMessage(data);
        }
    })
    .error(function () {
    })
    };

    self.SendMessageToGuest = function (GuestId) {
        $.ajax({
            type: "POST",
            url: webserviceURL + "waitlist/SendLoyaltyMessageToGuest",
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
            data: ko.toJSON({ GuestId: GuestId, RestaurantChainId: rcid, Message: returnGuestViewModel.NewGuestMessage() })
        })
    .done(function (data) {
        if (data.status == "ok") {
            if (data.data.Message) {
                NotifyErrorMessage(data.data.Message);
            }
            else {
                if (data.data[0].Status != 0) {
                    NotifyErrorMessage(data.data[0].ErrorText);
                }
                else if (data.data[0].Status == 0) {
                    self.LoadList();
                    NotifySavedMessage("Message sent successfully.");
                }
            }
            ClearAll();
        }
        else if (data.status == "badrequest") {
            showErrorMessage(data);
        }
    })
    };

    self.LoadList();
   
}

function Business(data) {
    var $super = this;
    $super.RestaurantChainId = ko.protectedObservable(data.RestaurantChainId);
    $super.GuestId = ko.protectedObservable(data.GuestId);
    $super.GuestName = ko.protectedObservable(data.GuestName);
    $super.MobileNumber = ko.protectedObservable(data.MobileNumber);
    $super.NoOfReturn = ko.protectedObservable(data.NoOfReturn);
    $super.LastVisit = ko.computed(function () {
        var date = new Date(data.LastVisit);
        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();

        return 'Last Visit: ' + monthNames[monthIndex] + ' ' + (day < 10 ? '0' + day : day) + ', ' + year;
    });
    $super.Total = ko.protectedObservable(data.Total);
    $super.Week = ko.protectedObservable(data.Week);
    $super.Month = ko.protectedObservable(data.Month);
    $super.Year = ko.protectedObservable(data.Year);
    $super.SendMessage = function (guest) {
        returnGuestViewModel.SendMessage(guest);
    };
    $super.RestaurantChainId = ko.protectedObservable(rcid);
    $super.UnReadMessageCount = ko.protectedObservable(data.UnReadMessageCount);
    $super.IsUnReadMessage = ko.computed(function () {
        if (data.UnReadMessageCount > 0)
            return true;
        else
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
        returnGuestViewModel.LoadList();
    };
}


var returnGuestViewModel = new ReturnGuestViewModel();
ko.applyBindings(returnGuestViewModel, document.getElementById("content-wrapper"));


function GuestMessage(GuestId, LoyaltyMessageId, Message, MessageDateTime, GuestName, MessageFrom) {
    var $message = this;
    $message.GuestId = ko.observable(GuestId);
    $message.LoyaltyMessageId = ko.observable(LoyaltyMessageId);
    $message.Message = ko.observable(Message);
    $message.MessageDateTime = ko.observable(MessageDateTime);
    $message.GuestName = ko.observable(GuestName);
    $message.MessageFrom = ko.observable(MessageFrom);
    $message.MessageTime = ko.computed(function () {

        var date = new Date(MessageDateTime);
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        return (hours < 10 ? '0' + hours : hours) + ":" + minutes;

    });

}

function ClearAll() {
    $('.chat-overlay, .chat-box').fadeOut();
}