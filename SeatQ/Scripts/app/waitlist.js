$('body').addClass('message2_body');

var isAdd = null;
var addWaitList = $("#add-waitlist");


$("#addList").click(function () {
    PrepareAdd();
});


function ClearAll() {
    addWaitList.modal('hide');
    $('.chat-overlay, .chat-box').fadeOut();
    isAdd = null;
    waitListViewModel.GroupSize(undefined);
    waitListViewModel.GuestName(undefined);
    waitListViewModel.Comments(undefined);
    waitListViewModel.MobileNumber(undefined);
    waitListViewModel.EstimatedHours(undefined);
    waitListViewModel.EstimatedMins(undefined);
    waitListViewModel.ActualHours(undefined);
    waitListViewModel.ActualMins(undefined);
    waitListViewModel.TableNumber(undefined);
    waitListViewModel.TableId(undefined);
    waitListViewModel.RoomNumber(undefined);
    waitListViewModel.EstimatedTime(undefined);
    waitListViewModel.ActualTime(undefined);
    $("#btnSubmit").removeAttr("disabled");
    $("#ActualTime").removeAttr("disabled");
    waitListViewModel.NewGuestMessage(undefined);
    waitListViewModel.GuestTypeId('');
    waitListViewModel.TableId('');
}

function WaitListViewModel() {
    var self = this;
    self.WaitListId = ko.observable();
    self.GroupSize = ko.observable();
    self.GuestTypeId = ko.observable();
    self.GuestName = ko.observable();
    self.Comments = ko.observable();
    self.MobileNumber = ko.observable();
    self.EstimatedHours = ko.observable();
    self.EstimatedMins = ko.observable();
    self.EstimatedTime = ko.observable();
    self.ActualHours = ko.observable();
    self.ActualMins = ko.observable();
    self.ActualDateTime = ko.observable();
    self.ActualTime = ko.observable();
    self.MessageReply = ko.observable();
    self.NoOfReturn = ko.observable();
    self.Visit = ko.observable();
    self.IsSeated = ko.observable();
    self.IsLeft = ko.observable();
    self.IsMsg = ko.observable();
    self.AverageWaitTime = ko.observable();
    self.LateSeating = ko.observable();
    self.RestaurantChainId = ko.observable();
    self.RestaurantNumber = ko.observable();
    self.RoomNumber = ko.observable();
    self.TableNumber = ko.observable();
    self.TableId = ko.observable();
    self.UnReadMessageCount = ko.observable();
    self.BusinessName = ko.observable();
    self.LogoPath = ko.observable();
    self.EstimatedDateTime = ko.observable();
    self.GetEstimatedDateTime = ko.computed(function () {
        var hour = 0;
        var min = 0;
        var date = new Date();
        var time = self.EstimatedHours() + ":" + self.EstimatedMins()//self.EstimatedTime();
        if (time) {
            hour = time.split(":")[0];
            min = time.split(":")[1];
        }

        if (min == 0 && hour == 0)
            return date;
        else {
            if (min == undefined || min == "" || parseInt(min) == "NaN" || min == "undefined")
                min = 0;
            if (hour == undefined || hour == "" || parseInt(hour) == "NaN" || hour == "undefined")
                hour = 0;

            date.setMinutes(date.getMinutes() + (parseInt(hour) * 60) + parseInt(min))

            if (min == 0 && hour == 0)
                return null;
            else
                return date;
        }
    });
    self.ImageSrc = ko.computed(function () {
        if (self.LogoPath())
            return self.LogoPath();
        else
            return '/Content/Uploads/sample.png';
    });

    self.MyGroupSize = ko.observable();
    self.MyGroupSizeFormated = ko.computed(function () {
        return "(" + self.MyGroupSize() + ") Persons";
    });
    self.MyGuestTypeId = ko.observable();
    self.MyEstimatedDateTime = ko.observable();
    self.MyActualDateTime = ko.observable();
    self.MyMobileNumber = ko.observable();
    self.MyGuestName = ko.observable();
    self.MyComments = ko.observable();
    self.MyWaitListId = ko.observable();
    self.MyMessageReply = ko.observable();
    self.MyIsSeated = ko.observable();
    self.MyIsLeft = ko.observable();
    self.MyIsMessageSent = ko.observable();
    self.MyRoomNumber = ko.observable();
    self.MyTableNumber = ko.observable();
    self.MyTableId = ko.observable();
    self.MyWaitingTime = ko.computed(function () {
        if (self.MyActualDateTime()) {
            var date = new Date(self.MyActualDateTime());
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var ampm = hours >= 12 ? 'pm' : 'am';
            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? '0' + minutes : minutes;
            var strTime = hours + ':' + minutes + ' ' + ampm;
            return strTime;
        }
    });
    self.MyEstimatedTime = ko.computed(function () {
        if (self.MyActualDateTime() && self.MyEstimatedDateTime()) {
            var date1 = new Date(self.MyActualDateTime());
            var date2 = new Date(self.MyEstimatedDateTime());
            var diffMs = (date2 - date1);
            //var diffHrs = Math.round((diffMs % 86400000) / 3600000);
            //var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000);
            /*if(diffHrs == 0)
                return diffMins + " mins";
            else
                return diffHrs + " hour " + diffMins + " mins";*/

            return Math.round((diffMs / 1000) / 60) + " mins";

        }

    });
    self.MyStatus = ko.computed(function () {
        if (self.MyMessageReply() == 1 && self.MyIsSeated() == false)
            return "Confirmed";
        else if (self.MyMessageReply() == 2)
            return "Cancelled";
        else if (self.MyIsSeated() == true)
            return "Seated";
        else if (self.MyIsLeft() == true)
            return "Seated";
        else if (self.MyIsMessageSent() == true && self.MyMessageReply() == null)
            return "Message Sent";
        else
            return "Waiting...";
    });
    self.MyShowConfirmed = ko.computed(function () {
        if (self.MyMessageReply() != 1 && self.MyIsSeated() == false)
            return true;
        else
            return false;
    });
    self.MyShowSeated = ko.computed(function () {
        if (self.MyMessageReply() == 1 && self.MyIsSeated() == false)
            return true;
        else
            return false;
    });
    self.MyShowLeft = ko.computed(function () {
        if (self.MyMessageReply() == 1 && self.MyIsSeated() == true)
            return true;
        else
            return false;
    });

    self.EditEstimatedDateTime = ko.observable();
    self.businesses = ko.observableArray([]);
    self.pagingdata = ko.observable(new PagingData({ Page: 1, PageSize: 10, TotalData: 0, TotalPages: 0 }));
    self.GuestType = ko.observableArray([]);
    self.Tables = ko.observableArray([]);
    self.TableStatus = ko.observableArray([]);
    self.NewGuestMessage = ko.observable();
    self.GuestMessages = ko.observableArray([]);

    
    self.LoadList = function () {
        var qs = '&Page=' + self.pagingdata().Page() + '&PageSize=' + self.pagingdata().PageSize + '&TotalPages=' + self.pagingdata().TotalPages() + '&TotalData=' + self.pagingdata().TotalData;

        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/?RestaurantChainId=" + rcid + qs,
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
                    $('.selectpicker').selectpicker('refresh');
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
            .error(function () {
            });

    };

    self.AddWaitlist = function () {
        if (isAdd) {
            if (rcid == 0) {
                $.notify('Please select restaurant.', 'error');
                return;
            }
            $("#btnSubmit").attr("disabled", "disabled");
            self.ActualDateTime(new Date());


            $.ajax({
                type: "POST",
                url: webserviceURL + 'waitlist',
                data: ko.toJSON({
                    RestaurantChainId: rcid,
                    GuestName: waitListViewModel.GuestName(), MobileNumber: waitListViewModel.MobileNumber(), GroupSize: waitListViewModel.GroupSize(), GuestTypeId: waitListViewModel.GuestTypeId(),
                    EstimatedDateTime: waitListViewModel.GetEstimatedDateTime(), ActualDateTime: waitListViewModel.ActualDateTime(),
                    TableId: waitListViewModel.TableId(), RoomNumber: waitListViewModel.RoomNumber(), Comments: waitListViewModel.Comments()
                }),
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
                        ClearAll();
                        NotifySavedMessage("Saved successfully.");
                        self.LoadList();
                    }
                    else if (data.status == "badrequest") {
                        $("#btnSubmit").removeAttr("disabled");
                        showErrorMessage(data);
                    }
                })
                .error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
        else if (isAdd == false) {
            if (rcid == 0) {
                $.notify('Please select restaurant', 'error');
                return;
            }
            $("#btnSubmit").attr("disabled", "disabled");

            $.ajax({
                type: "PUT",
                url: webserviceURL + "waitlist",
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + $.cookie("token")
                },
                xhrFields: {
                    withCredentials: true
                },
                data: ko.toJSON({
                    RestaurantChainId: rcid, WaitListId: waitListViewModel.WaitListId(),
                    GuestName: waitListViewModel.GuestName(), MobileNumber: waitListViewModel.MobileNumber(),
                    GroupSize: waitListViewModel.GroupSize(), GuestTypeId: waitListViewModel.GuestTypeId(), EstimatedDateTime: waitListViewModel.EditEstimatedDateTime(), ActualDateTime: waitListViewModel.ActualDateTime(),
                    TableId: waitListViewModel.TableId(), RoomNumber: waitListViewModel.RoomNumber(), Comments: waitListViewModel.Comments()
                }),
            })
                .done(function (data) {
                    if (data.status == "ok") {
                        ClearAll();
                        NotifySavedMessage("Saved successfully.");
                        self.LoadList();
                    }
                    if (data.status == "badrequest") {
                        $("#btnSubmit").removeAttr("disabled");
                        showErrorMessage(data);
                    }
                })
                .error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
    };

    self.Edit = function (WaitListId) {
        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/?WaitListId=" + WaitListId,
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
                    if (data.data.MyMessageReply == 2) {
                        //NotifyErrorMessage("Guest already cancelled.");
                        //return;
                    }
                    if (data.data.MyIsSeated) {
                        //NotifyErrorMessage("Guest already seated.");
                        //return;
                    }
                    PrepareUpdate();

                    self.WaitListId(data.data.MyWaitListId);
                    self.GroupSize(data.data.MyGroupSize);
                    self.GuestName(data.data.MyGuestName);
                    self.Comments(data.data.MyComments);
                    self.MobileNumber(data.data.MyMobileNumber);
                    self.ActualDateTime(data.data.MyActualDateTime);
                    self.GuestTypeId(data.data.MyGuestTypeId);
                    self.TableNumber(data.data.MyTableNumber);
                    self.TableId(data.data.MyTableId);
                    self.RoomNumber(data.data.MyRoomNumber);
                    $('.selectpicker').selectpicker('refresh');
                    if (data.data.MyActualDateTime) {
                        var date = new Date(data.data.MyActualDateTime);
                        var hours = date.getHours();
                        var minutes = date.getMinutes();
                        var ampm = hours >= 12 ? 'pm' : 'am';
                        hours = hours % 12;
                        hours = hours ? hours : 12; // the hour '0' should be '12'
                        minutes = minutes < 10 ? '0' + minutes : minutes;
                        self.ActualHours(hours);
                        self.ActualMins(minutes);
                        self.ActualTime((hours < 10 ? '0' + hours : hours) + ":" + minutes);
                    }
                    if (data.data.MyEstimatedDateTime && data.data.MyActualDateTime) {
                        var date1 = new Date(data.data.MyActualDateTime);
                        var date2 = new Date(data.data.MyEstimatedDateTime);
                        var diffMs = (date2 - date1);
                        var diffMins = Math.round((diffMs / 1000) / 60);

                        var realmin = diffMins % 60
                        var hours = Math.floor(diffMins / 60)
                        realmin = realmin < 10 ? '0' + realmin : realmin;
                        hours = hours < 10 ? '0' + hours : hours;
                        self.EstimatedMins(realmin);
                        self.EstimatedHours(hours);
                        self.EstimatedTime(hours + ":" + realmin);
                    }
                    self.EditEstimatedDateTime = ko.computed(function () {
                        var date = new Date(data.data.MyActualDateTime);
                        var min = 0;
                        var hour = 0;
                        var time = self.EstimatedHours() + ":" + self.EstimatedMins();
                        if (time) {
                            hour = time.split(":")[0];
                            min = time.split(":")[1];
                        }
                        if (min == 0 && hour == 0)
                            return date;
                        else {
                            if (min == undefined || min == "" || parseInt(min) == "NaN" || min == "undefined")
                                min = 0;
                            if (hour == undefined || hour == "" || parseInt(hour) == "NaN" || min == "undefined")
                                hour = 0;

                            date.setMinutes(date.getMinutes() + (parseInt(hour) * 60) + parseInt(min))

                            return date;
                        }

                    });
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
            .error(function () {
            })
    };

    self.EditWaitlist = function (WaitListId) {
        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/?WaitListId=" + WaitListId,
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
                    if (data.data.MyMessageReply == 2) {
                        NotifyErrorMessage("Guest already cancelled.");
                        return;
                    }
                    if (data.data.MyIsLeft) {
                        NotifyErrorMessage("Guest already left.");
                        return;
                    }
                    ClearAll();

                    self.MyWaitListId(data.data.MyWaitListId);
                    self.MyGroupSize(data.data.MyGroupSize);
                    self.MyGuestName(data.data.MyGuestName);
                    self.MyComments(data.data.MyComments);
                    self.MyMobileNumber(data.data.MyMobileNumber);
                    self.MyEstimatedDateTime(data.data.MyEstimatedDateTime);
                    self.MyActualDateTime(data.data.MyActualDateTime);
                    self.MyMessageReply(data.data.MyMessageReply);
                    self.MyIsSeated(data.data.MyIsSeated);
                    self.MyIsMessageSent(data.data.MyIsMessageSent);


                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
            .error(function () {
            })
    };

    self.SendTextMessage = function (WaitListId) {
        $.ajax({
            type: "POST",
            url: webserviceURL + "waitlist/SendTextMessage",
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
            data: ko.toJSON({ WaitListId: WaitListId })
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

    self.SendLeftMessage = function (WaitListId) {
        $.ajax({
            type: "POST",
            url: webserviceURL + "waitlist/SendLeftMessage",
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
            data: ko.toJSON({ WaitListId: WaitListId })
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
                            NotifySavedMessage("Loyalty message sent successfully.");
                        }
                    }
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    };

    self.SendMessageToGuest = function (WaitListId) {
        $.ajax({
            type: "POST",
            url: webserviceURL + "waitlist/SendMessageToGuest",
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
            data: ko.toJSON({ WaitListId: WaitListId, RestaurantChainId: rcid, Message: waitListViewModel.NewGuestMessage() })
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

    self.Confirmed = function (WaitListId) {
        $.ajax({
            type: "PUT",
            url: webserviceURL + "waitlist/Confirmed",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON({ WaitListId: WaitListId }),
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data) {
                if (data.status == "ok") {
                    NotifySavedMessage("Guest confirmed.");
                    self.LoadList();
                    ClearAll();
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    };

    self.Leave = function (WaitListId) {
        $.ajax({
            type: "PUT",
            url: webserviceURL + "waitlist/Leave",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON({ WaitListId: WaitListId }),
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data) {
                if (data.status == "ok") {
                    NotifySavedMessage("Guest cancelled.");
                    self.LoadList();
                    ClearAll();
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    };

    self.OpenSeated = function (WaitListId) {

        self.WaitListId(WaitListId);

        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/tables/?RestaurantChainId=" + rcid,
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
                    var tables = $.map(data.data, function (item) {
                        return new TableSeating(item.TableId, item.TableNumber, item.AvgTimeInMinute, item.TableStatus, item.MaxSeating)
                    });
                    self.TableStatus(tables);
                    $("#addWaitList").hide();
                    $('.popup-overlay, .popup-box').fadeIn();
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    }
    self.CloseSeated = function () {
        $('.popup-overlay, .popup-box').fadeOut();
    }
    self.Seated = function (TableId, TableStatus) {
        if (TableStatus != 'green') {
            if (confirm('The table is currently not available.\n\nDo you want to override.') == false)
                return;
        }
        WaitListId = self.WaitListId();
        $("#addWaitList").hide();
        $.ajax({
            type: "PUT",
            url: webserviceURL + "waitlist/Seated",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON({ WaitListId: WaitListId, TableId: TableId }),
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
        })
            .done(function (data) {
                if (data.status == "ok") {
                    $('.popup-overlay, .popup-box').fadeOut();
                    NotifySavedMessage("Guest seated.");
                    self.LoadList();
                    ClearAll();
                }
                else if (data.status == "badrequest") {
                    $('.popup-overlay, .popup-box').fadeOut();
                    showErrorMessage(data);
                }
            })
    };

    self.Left = function (WaitListId) {

        $("#addWaitList").hide();
        $.ajax({
            type: "PUT",
            url: webserviceURL + "waitlist/Left",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON({ WaitListId: WaitListId }),
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
        })
            .done(function (data) {
                if (data.status == "ok") {
                    NotifySavedMessage("Guest left.");
                    self.LoadList();
                    ClearAll();
                    self.SendLeftMessage(WaitListId);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    };

    self.ShowMsg = function (WaitListId, GuestName) {
        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/message/?WaitListId=" + WaitListId,
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

                    ClearAll();
                    addWaitList.hide();
                    $('.chat-overlay, .chat-box').fadeIn();
                    self.MyWaitListId(WaitListId);
                    self.MyGuestName(data.data.GuestName);
                    self.GuestMessages.removeAll();
                    for (var i = 0; i < data.data.GuestMessage.length; i++) {
                        var message = data.data.GuestMessage[i];
                        self.GuestMessages.push(new GuestMessage(message.WaitListId, message.GuestMessageId, message.Message, message.MessageDateTime, message.GuestName, message.MessageFrom));
                    }
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
            .error(function () {
            })
    };

    setData = function (data) {
        var pagingdata = new PagingData(data.data.businesses.PagingData);
        self.pagingdata(pagingdata);
        var mappedbusinesses = $.map(data.data.businesses.Businesses, function (item) {
            return new Business(item);
        });
        self.businesses(mappedbusinesses);

        var guesttype = [];
        $.map(data.data.businesses.GuestType, function (item) {
            guesttype.push({ "text": item.GuestType, "value": item.GuestTypeId });
        });
        self.GuestType(guesttype);

        var tables = [];
        $.map(data.data.businesses.Tables, function (item) {
            tables.push({ "text": item.TableNumber, "value": item.TableId });
        });
        self.Tables(tables);

        self.RestaurantChainId(data.data.businesses.RestaurantChainId);
        self.AverageWaitTime(data.data.businesses.AverageWaitTime);
        self.RestaurantNumber(data.data.businesses.RestaurantNumber);
        self.BusinessName(data.data.businesses.BusinessName);
        self.LogoPath(data.data.businesses.LogoPath);
        rcid = data.data.businesses.RestaurantChainId;
        $.cookie("rcid", rcid, { expires: 1 });

    };

    self.LoadList();

    setInterval(function () {
        self.LoadList();
    }, 10000);

    self.Regions = ko.observableArray([]);
    self.RegionName = ko.observable();
    self.Businesses = ko.observableArray([]);
    self.BusinessId = ko.observable();
    self.SelectedRegionBusiness = ko.observable();
    self.DisplayBusiness = ko.observable(false);
    self.SelectedBusiness = ko.observable('Business');
    self.LoadRegions = function () {
        $.ajax({
            type: "GET",
            url: webserviceCoreURL + "regions",
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data, textStatus, xhr) {
                if (data.Regions.length > 0) {
                    var regions = [];
                    $.map(data.Regions, function (item) {
                        regions.push({ "text": item.Name, "value": item.Name });
                    });
                    self.Regions(regions);
                    self.DisplayBusiness(true);
                }
                else
                    self.DisplayBusiness(false);

            });
    };
    self.LoadBusiness = function () {
        $.ajax({
            type: "GET",
            url: webserviceCoreURL + self.RegionName() + '/business',
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data, textStatus, xhr) {
                if (data.data.Businesses.length > 0) {
                    var businesses = [];
                    $.map(data.data.Businesses, function (item) {
                        businesses.push({ "text": item.Name, "value": item.BusinessId + "&" + item.Name });
                    });
                    self.Businesses(businesses);
                }

            });
    }
    self.ResetBusiness = function () {
        self.RegionName(undefined);
        self.Businesses([]);
        self.BusinessId(undefined);
        cswm();
    }
    self.SaveBusiness = function () {
        if (self.SelectedRegionBusiness() == undefined) {
            $.notify('Please select business.', 'error');
            return;
        }

        selectedBusiness = self.SelectedRegionBusiness().split('&');
        self.BusinessId(selectedBusiness[0]);
        self.BusinessName(selectedBusiness[1]);
        self.SelectedBusiness(self.RegionName() + ' : ' + self.BusinessName());

        //Now, save idestn business
        $.ajax({
            type: "POST",
            url: webserviceURL + 'restaurant/addbusiness',
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            },
            data: ko.toJSON({
                BusinessId: self.BusinessId(),
                BusinessName: self.BusinessName()
            }),
        })
            .done(function (data, textStatus, xhr) {
                if (data.status == "ok") {
                    rcid = data.data.RestaurantChainId;
                    $.cookie("rcid", rcid, { expires: 1 });
                    cswm();
                    ClearAll();
                    NotifySavedMessage("Saved successfully.");
                    self.LoadList();
                }
                if (data.status == "badrequest") {
                    $("#btnSubmit").removeAttr("disabled");
                    showErrorMessage(data);
                }

            });
    };

    self.PlusEstimatedWait = function () {
        var min = self.EstimatedMins();
        if (min == "") min = 0;
        min = parseInt(min) + 1;

        if (min > 59) {
            var hours = self.EstimatedHours();
            if (hours == "") hours = 0;
            hours = parseInt(hours) + 1;
            self.EstimatedHours(hours);
            min = 0;
        }

        self.EstimatedMins(min);
    }

    self.MinusEstimatedWait = function () {
        var min = self.EstimatedMins();
        if (min == "") min = 0;
        min = parseInt(min) - 1;

        if (min < 0) min = 0;
        self.EstimatedMins(min);
    }

    self.LoadRegions();
}

function Business(data) {
    var $super = this;
    this.GuestName = ko.protectedObservable(data.GuestName);
    this.Comments = ko.protectedObservable(data.Comments);
    this.MobileNumber = ko.protectedObservable(data.MobileNumber);
    this.GroupSize = ko.protectedObservable(data.GroupSize);
    this.GuestTypeId = ko.protectedObservable(data.GuestTypeId);
    this.ActualDateTime = ko.protectedObservable(data.ActualDateTime);
    this.EstimatedDateTime = ko.protectedObservable(data.EstimatedDateTime);
    this.SeatedDateTime = ko.protectedObservable(data.SeatedDateTime);
    this.WaitListId = ko.protectedObservable(data.WaitListId);
    this.MessageReply = ko.protectedObservable(data.MessageReply);
    this.IsSeated = ko.protectedObservable(data.IsSeated);
    this.GuestMessageCount = ko.protectedObservable(data.GuestMessageCount);
    this.NoOfReturn = ko.protectedObservable(data.NoOfReturn);
    this.Visit = ko.protectedObservable(data.Visit);
    this.TableNumber = ko.protectedObservable(data.TableNumber);
    this.TableId = ko.protectedObservable(data.TableId);
    this.AvgTableTime = ko.protectedObservable(data.AvgTableTime);
    this.UnReadMessageCount = ko.protectedObservable(data.UnReadMessageCount);
    this.IsUnReadMessage = ko.computed(function () {
        if (data.UnReadMessageCount > 0)
            return true;
        else
            return false;

    });
    this.GuestNameWithType = ko.computed(function () {

        var guestType;
        if (data.GuestTypeId == 1)
            guestType = 'CI'
        else if (data.GuestTypeId == 2)
            guestType = 'WI'
        else if (data.GuestTypeId == 3)
            guestType = 'MC'

        return data.GuestName + ' (' + guestType + ', ' + data.Visit + ')';
    });
    this.LateSeating = ko.computed(function () {
        return (data.LateSeating + " mins").replace("-", "+");
    });
    this.LateSeatingClass = ko.computed(function () {
        if (data.LateSeating) {
            if (data.LateSeating > 0)
                return "badge bg-darkgray";
            else
                return "badge bg-darkred";
        }
        else
            return "badge bg-darkgray";

    });
    this.RoomNumber = ko.protectedObservable(data.RoomNumber);
    this.RemainingTime = ko.computed(function () {
        return (data.RemainingTime + " mins").replace("-", "+");
    });
    this.TotalMinuteSat = ko.computed(function () {
        if (data.TotalMinuteSat) {
            return data.TotalMinuteSat + " mins";
        }
        else
            return '--';
    });
    this.RemainingTimeClass = ko.computed(function () {
        if (data.RemainingTime) {
            if (data.RemainingTime > 0)
                return "badge color-green";
            else
                return "badge color-red";
        }

        if (data.RemainingTime == 0)
            return "badge color-green";

    });
    this.SizeClass = ko.computed(function () {
        if (data.RemainingTime) {
            if (data.RemainingTime >= 0)
                return "bg-orange";
            else
                return "bg-red";
        }
        if (data.RemainingTime == 0)
            return "bg-orange";
    });
    this.ReturnGuest = ko.computed(function () {
        if (data.NoOfReturn)
            return data.GuestName + ' (' + data.NoOfReturn + ')';
        else
            return data.GuestName;
    });
    this.TableBooked = ko.computed(function () {
        if (data.TableNumber)
            return data.TableNumber;
        else
            return 'First Available';
    });
    this.ActualTime = ko.computed(function () {
        if (data.ActualDateTime) {
            var date = new Date(data.ActualDateTime);
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var ampm = hours >= 12 ? 'pm' : 'am';
            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? '0' + minutes : minutes;
            var strTime = hours + ':' + minutes + ' ' + ampm;

            var actualDate = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
            var today = new Date();
            var todayDate = today.getFullYear() + "-" + (today.getMonth() + 1) + "-" + today.getDate();

            var date1 = new Date(actualDate);
            var date2 = new Date(todayDate);

            if (date2 > date1) {
                var month = (date.getMonth() + 1);
                var day = date.getDate();
                actualDate = (month < 10 ? '0' + month : month) + "/" + (day < 10 ? '0' + day : day) + "/" + date.getFullYear();
                return actualDate + " " + strTime;
            }
            else
                return strTime;
        }
    });
    this.TimeSat = ko.computed(function () {
        if (data.SeatedDateTime) {
            var date = new Date(data.SeatedDateTime);
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var ampm = hours >= 12 ? 'pm' : 'am';
            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? '0' + minutes : minutes;
            var strTime = hours + ':' + minutes + ' ' + ampm;

            var actualDate = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
            var today = new Date();
            var todayDate = today.getFullYear() + "-" + (today.getMonth() + 1) + "-" + today.getDate();

            var date1 = new Date(actualDate);
            var date2 = new Date(todayDate);

            if (date2 > date1) {
                var month = (date.getMonth() + 1);
                var day = date.getDate();
                actualDate = (month < 10 ? '0' + month : month) + "/" + (day < 10 ? '0' + day : day) + "/" + date.getFullYear();
                return actualDate + " " + strTime;
            }
            else
                return strTime;
        }
        else
            return '--';
    });
    this.Time = ko.computed(function () {
        if (data.ActualDateTime && data.EstimatedDateTime) {
            var date1 = new Date(data.ActualDateTime);
            var date2 = new Date(data.EstimatedDateTime);
            var diffMs = (date2 - date1);

            return Math.round((diffMs / 1000) / 60) + " mins";

        }
    });
    this.SeatedTime = ko.computed(function () {
        if (data.ActualDateTime && data.SeatedDateTime) {
            var date1 = new Date(data.ActualDateTime);
            var date2 = new Date(data.SeatedDateTime);
            var diffMs = (date2 - date1);

            return Math.round((diffMs / 1000) / 60) + " mins";

        }
    });
    this.IsConfirmed = ko.computed(function () {
        if (data.MessageReply == 1 && data.IsSeated == false)
            return true;
        else
            return false;
    });
    this.SeatedClass = ko.computed(function () {
        if (data.IsSeated) {
            var SeatedDate = new Date(data.SeatedDateTime);
            var EstimatedDate = new Date(data.EstimatedDateTime);

            if (SeatedDate > EstimatedDate)
                return "color:#FF0000";
            else
                return "color:#2C8139";
        }
        else {
            if (data.MessageReply == 2) //cancel
                return "color:#F8A001";
        }

    });
    this.Status = ko.computed(function () {
        if (data.MessageReply == 1 && data.IsSeated == false)
            return "Confirmed";
        else if (data.MessageReply == 2)
            return "Cancelled";
        else if (data.IsSeated == true)
            return "Seated";
        else if (data.IsMessageSent == true && data.MessageReply == null)
            return "Message Sent";
    });
    this.StatusClass = ko.computed(function () {
        if (data.MessageReply == 1 && data.IsSeated == false)
            return "btn-confirmed";
        else if (data.MessageReply == 2)
            return "btn-leave";
        else if (data.IsSeated == true && data.IsLeft == null)
            return "btn-seated";
        else if (data.IsMessageSent == true && data.MessageReply == null)
            return "btn-message";
        else if (data.IsLeft == true)
            return "btn-leave";
        else
            return "btn-waiting";
    });
    this.DisabledClass = ko.computed(function () {
        if (data.MessageReply == 2)
            return "disable";
    });
    this.CommentsClass = ko.computed(function () {
        if (data.Comments)
            return "message active";
        else
            return "message inactive";
    });
    this.ReadyMessageClass = ko.computed(function () {
        if (data.IsMessageSent)
            return "background:#ea2c32;font-size: 10px;border-radius: 2px;display: block;color:#fff;font-weight:700;padding: 3px 7px;";
        else
            return "background:#26b074;font-size: 10px;border-radius: 2px;display: block;color:#fff;font-weight:700;padding: 3px 7px;";
    });
    this.IsReturnedGuest = ko.computed(function () {
        if (data.NoOfReturn > 0)
            return true;
        else
            return false;
    });
    this.IsMsg = ko.computed(function () {
        if (data.GuestMessageCount > 0)
            return true;
        else
            return false;
    });
    this.IsCancelled = ko.computed(function () {
        if (data.MessageReply == 2 || data.IsLeft == true)
            return true;
        else
            return false;
    });
    this.ShowConfirmed = ko.computed(function () {
        if (data.MessageReply != 1 && data.IsSeated == false)
            return true;
        else
            return false;
    });
    this.ShowSeated = ko.computed(function () {
        if (data.MessageReply == 1 && data.IsSeated == false)
            return true;
        else
            return false;
    });
    this.ShowLeft = ko.computed(function () {
        if (data.MessageReply == 1 && data.IsSeated == true)
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
        waitListViewModel.LoadList();
    };
}

function PrepareAdd() {
    ClearAll();
    $('.chat-overlay, .chat-box').fadeOut();
    $("#myModalLabel").text("Add New List");
    $("#btnSubmit").text("Add to waitlist");

    var AvgWaitTime = waitListViewModel.AverageWaitTime();
    var AvgHour = 0;
    var AvgMin = 0;
    if (AvgWaitTime == undefined) AvgWaitTime = 0;
    AvgHour = parseInt(AvgWaitTime / 60);
    AvgMin = (AvgWaitTime - (AvgHour * 60));
    waitListViewModel.EstimatedHours(AvgHour < 10 ? '0' + AvgHour : AvgHour);
    waitListViewModel.EstimatedMins(AvgMin < 10 ? '0' + AvgMin : AvgMin);
    waitListViewModel.EstimatedTime((AvgHour < 10 ? '0' + AvgHour : AvgHour) + ":" + (AvgMin < 10 ? '0' + AvgMin : AvgMin));

    isAdd = true;

    var date = new Date();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;

    waitListViewModel.ActualDateTime(date);
    waitListViewModel.ActualHours(hours);
    waitListViewModel.ActualMins(minutes);
    waitListViewModel.ActualTime((hours < 10 ? '0' + hours : hours) + ":" + minutes);
}

function PrepareUpdate() {
    addWaitList.modal('show');
    $('.chat-overlay, .chat-box').fadeOut();
    $("#myModalLabel").text("Edit List");
    $("#btnSubmit").text("UPDATE WAITLIST");
    isAdd = false;
}

var waitListViewModel = new WaitListViewModel();
ko.applyBindings(waitListViewModel, document.getElementById("content-wrapper"));

function HideAdd() {
    $("#addWaitList").hide();
}

function HideUpdate() {
    $("#updateWaitList").hide();
}

$.ajax({
    type: "GET",
    url: webserviceURL + "ispasswordchanged",
    contentType: "application/json; charset=utf-8",
    //data: ko.toJSON({ UserId: 0/*uid*/ }),
    headers: {
        'Authorization': 'Bearer ' + $.cookie("token")
    },
    xhrFields: {
        withCredentials: true
    }
})
    .done(function (data, textStatus, xhr) {
        if (data.status == "ok") {
            if (data.data.IsPasswordChanged)
                NotifyErrorMessage("Please set your new password.");

        }
        if (data.status == "badrequest") {
            showErrorMessage(data);
        }
    });

function GuestMessage(WaitListId, GuestMessageId, Message, MessageDateTime, GuestName, MessageFrom) {
    var $message = this;
    $message.WaitListId = ko.observable(WaitListId);
    $message.GuestMessageId = ko.observable(GuestMessageId);
    $message.Message = ko.observable(Message);
    $message.MessageDateTime = ko.observable(MessageDateTime);
    $message.GuestName = ko.observable(GuestName);
    $message.MessageFrom = ko.observable(MessageFrom);
    /*$message.MessageFrom = ko.computed(function () {
        if (MessageFrom == null)
            return "You";
        else
            return GuestName;
    });*/
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

function TableSeating(TableId, TableNumber, AvgTimeInMinute, TableStatus, MaxSeating) {
    var $table = this;
    $table.TableId = ko.observable(TableId);
    $table.TableNumber = ko.observable(TableNumber);
    $table.AvgTimeInMinute = ko.observable(AvgTimeInMinute);
    $table.TableStatus = ko.observable(TableStatus);
    $table.MaxSeating = ko.observable(MaxSeating);
    $table.Avg = ko.computed(function () {
        if (TableStatus == 'red')
            return AvgTimeInMinute + 'min';
        else if (TableStatus == 'yellow')
            return AvgTimeInMinute + 'min';
        else
            return "";
    });
    $table.AvgClass = ko.computed(function () {
        if (TableStatus == 'red')
            return "green-label";
        else if (TableStatus == 'yellow')
            return "red-label";
        else
            return "";
    });
    $table.Status = ko.computed(function () {
        if (TableStatus == 'green')
            return '<li class="pop-dot green"></li><li class="pop-dot"></li><li class="pop-dot"></li>';
        else if (TableStatus == 'red')
            return '<li class="pop-dot"></li><li class="pop-dot"></li><li class="pop-dot red"></li>';
        else
            return '<li class="pop-dot"></li><li class="pop-dot yellow"></li><li class="pop-dot"></li>';
    });

}