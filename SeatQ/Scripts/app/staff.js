var isAdd = null;
var addWaitList = $("#add-waitlist");

$("#addList").click(function () {
    PrepareAdd();
});

function ClearAll() {
    isAdd = null;
    addWaitList.modal('hide');
    hostessViewModel.isActive("true");
    hostessViewModel.UserName("");
    hostessViewModel.Password("");
    hostessViewModel.Email("");
    hostessViewModel.UserId("");
    $('#username').removeAttr('disabled');
    $('#email').removeAttr('disabled');
}

function PrepareAdd() {
    ClearAll();
    isAdd = true;
    $("#myModalLabel").text("Add Staff");
    $("#btnSubmit").text("Add Staff");
}

function PrepareUpdate() {
    $("#myModalLabel").text("Edit Staff");
    $("#btnSubmit").text("Update Staff");
    isAdd = false;
    $('#username').attr('disabled', 'disabled');
    $('#email').attr('disabled', 'disabled');
    addWaitList.modal('show');
}


function HostessViewModel() {
    var self = this;
    self.RestaurantChainId = ko.observable();
    self.UserName = ko.observable();
    self.Password = ko.observable();
    self.Email = ko.observable();
    self.isActive = ko.observable("true");
    self.UserId = ko.observable();
    self.StaffType = ko.observableArray([]);
    self.StaffTypeId = ko.observable([]);

    self.businesses = ko.observableArray([]);

    self.LoadList = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + 'hostess/?RestaurantChainId=' + rcid,
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
    self.Add = function () {
        if (isAdd) {
            self.RestaurantChainId(rcid);
            $.ajax({
                type: "POST",
                url: webserviceURL + 'hostess',
                data: ko.toJSON(self),
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
                        NotifySavedMessage("Saved successfully.");
                        ClearAll();
                        self.LoadList();
                    }
                    else if (data.status == "badrequest") {
                        showErrorMessage(data);
                    }
                    else if (data.status = "conflict") {
                        NotifyErrorMessage(data.message);
                    }
                }).
                error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
        else if (isAdd == false) {
            $.ajax({
                type: "PUT",
                url: webserviceURL + "hostess",
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(self),
                headers: {
                    'Authorization': 'Bearer ' + $.cookie("token")
                },
                xhrFields: {
                    withCredentials: true
                }
            })
                .done(function (data) {
                    if (data.status == "ok") {
                        NotifySavedMessage("Saved successfully.");
                        ClearAll()
                        self.LoadList();
                    }
                    else if (data.status == "badrequest") {
                        showErrorMessage(data);
                    }
                }).
                error(function (xhr, status, error) {
                    ShowErrorMessage(xhr, status, error)
                });
        }
    }
    self.Edit = function (UserId) {
        $.ajax({
            type: "GET",
            url: webserviceURL + "users/?userid=" + UserId,
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
                    self.RestaurantChainId(data.data.RestaurantChainId);
                    self.UserName(data.data.UserName);
                    self.Email(data.data.Email);
                    self.Password(null);
                    self.isActive(data.data.isActive.toString());
                    self.UserId(data.data.UserId);
                    self.StaffTypeId(data.data.StaffTypeId);
                    $('.selectpicker').selectpicker('refresh');
                    PrepareUpdate();
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    };close

    setData = function (data) {
        var mappedbusinesses = $.map(data.data, function (item) {
            return new Business(item);
        });
        self.businesses(mappedbusinesses);
    };

   
    self.LoadStaffTypes = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + "stafftypes",
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
                    var type = [];
                    $.map(data.data.StaffTypeItems, function (item) {
                        type.push({ "text": item.Title, "value": item.StaffTypeId });
                    });
                    self.StaffType(type);
                    $('.selectpicker').selectpicker('refresh');
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            });
    }
    self.LoadList();
    self.LoadStaffTypes();
}

var hostessViewModel = new HostessViewModel();
ko.applyBindings(hostessViewModel, document.getElementById("content-wrapper"));

function Business(data) {
    var $super = this;
    this.UserName = ko.protectedObservable(data.UserName);
    this.Email = ko.protectedObservable(data.Email);
    this.isActive = ko.protectedObservable(data.isActive);
    this.UserId = ko.protectedObservable(data.UserId);
    this.CreateDate = ko.protectedObservable(data.CreateDate);
    this.LastAccessDateTime = ko.protectedObservable(data.LastAccessDateTime);
    this.StaffTypeId = ko.protectedObservable(data.StaffTypeId);
    this.Title = ko.protectedObservable(data.Title);
    this.MemberSince = ko.computed(function () {
        var d = new Date(data.CreateDate);
        return ("00" + (d.getMonth() + 1)).slice(-2) + "/" +
            ("00" + d.getDate()).slice(-2) + "/" +
            d.getFullYear() + " " +
            ("00" + d.getHours()).slice(-2) + ":" +
            ("00" + d.getMinutes()).slice(-2) + ":" +
            ("00" + d.getSeconds()).slice(-2)
    });
    this.LastAccess = ko.computed(function () {
        if (data.LastAccessDateTime) {
            var d = new Date(data.LastAccessDateTime);
            return ("00" + (d.getMonth() + 1)).slice(-2) + "/" +
                ("00" + d.getDate()).slice(-2) + "/" +
                d.getFullYear() + " " +
                ("00" + d.getHours()).slice(-2) + ":" +
                ("00" + d.getMinutes()).slice(-2) + ":" +
                ("00" + d.getSeconds()).slice(-2)
        }
        else
            return "";
    });
    this.StatusClass = ko.computed(function () {
        if (data.isActive)
            return "status active";
        else
            return "status deactive";
    });
}
