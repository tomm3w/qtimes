$("#divHeader").text("Manage Hostess");
var overlay = $("#overlay");
var addBtn = $("#addBtn");
addBtn.text("New Hostess");
$("#customDate").hide();
$("#filter").hide();

addBtn.click(function () {
    if ($("#add-hostess").is(':visible')) {
        overlay.hide('fade');
        $("#add-hostess").hide();
        isAdd = null;
    }
    else {
        overlay.show('fade');
        $("#add-hostess").show();
        isAdd = true;
        $("#title").text("Create New Hostess");
        $("#UserName").removeAttr("readonly");
    }

    hostessViewModel.RestaurantChainId(0);
    hostessViewModel.isActive("true");
    hostessViewModel.UserName(undefined);
    hostessViewModel.Password("");
    hostessViewModel.Email(undefined);
    hostessViewModel.UserId(undefined);
});

var isAdd = null;

function HostessViewModel() {
    var self = this;
    self.RestaurantChainId = ko.observable();
    self.UserName = ko.observable();
    self.Password = ko.observable();
    self.Email = ko.observable();
    self.isActive = ko.observable("true");
    self.UserId = ko.observable();
    self.businesses = ko.observableArray([]);
    self.LoadList = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + 'hostess/?RestaurantChainId=' + rcid,
            //data: ko.toJSON({RestaurantChainId:rcid}),
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
                        overlay.hide('fade');
                        $("#add-hostess").hide();
                        isAdd = null;
                        self.LoadList();
                    }
                    else if (data.status == "badrequest") {
                        showErrorMessage(data);
                    }
                    else if (data.status = "conflict") {
                        NotifyErrorMessage(data.message);
                    }
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
                    $("#add-hostess").hide();
                    overlay.hide('fade');
                    self.LoadList();
                    isAdd = null;
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
        }
    }
    self.Edit = function (UserId) {
        $("#add-hostess").show();
        overlay.show('fade');
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
            //data: ko.toJSON({UserId:UserId})
        })
            .done(function (data) {
                if (data.status == "ok") {
                    self.RestaurantChainId(data.data.RestaurantChainId);
                    self.UserName(data.data.UserName);
                    self.Email(data.data.Email);
                    self.Password(null);
                    self.isActive(data.data.isActive.toString());
                    self.UserId(data.data.UserId);
                    isAdd = false;

                    $("#title").text("Edit Hostess");
                    $("#UserName").attr("readonly", "true");
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    };

    setData = function (data) {
        //var pagingdata = new PagingData(data.data.businesses.PagingData);
        //self.pagingdata(pagingdata);
        var mappedbusinesses = $.map(data.data, function (item) {
            return new Business(item);
        });
        self.businesses(mappedbusinesses);
    };

    self.LoadList();

}

function Business(data) {
    var $super = this;
    this.UserName = ko.protectedObservable(data.UserName);
    this.Email = ko.protectedObservable(data.Email);
    this.isActive = ko.protectedObservable(data.isActive);
    this.UserId = ko.protectedObservable(data.UserId);
    this.CreateDate = ko.protectedObservable(data.CreateDate);
    this.LastAccessDateTime = ko.protectedObservable(data.LastAccessDateTime);
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

var hostessViewModel = new HostessViewModel();
ko.applyBindings(hostessViewModel);

function HidePopup() {
    $("#add-hostess").hide();
    overlay.hide('fade');
}

