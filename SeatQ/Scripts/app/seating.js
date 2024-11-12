var addWaitList = $("#add-waitlist");

function SeatingViewModel() {
    var self = this;
    var cookieValue = $.cookie("rcid");
    if (cookieValue) {
        rcid = cookieValue;
    }

    self.TableStatus = ko.observableArray([]);
    self.Staffs = ko.observableArray([]);
    self.SelectedTable = ko.observable();
    self.SelectedTableName = ko.observable();

    self.LoadList = function () {

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
                        return new TableSeating(item);
                    });
                    self.TableStatus(tables);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })

    };

    self.CloseTable = function (tableid) {

        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/closetable/?RestaurantChainId=" + rcid + "&TableId=" + tableid,
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
                    self.LoadList();
                    NotifySavedMessage("Table closed.");
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })

    };

    self.AssignTable = function (table) {
        $('input:radio[name=staff]:checked').prop('checked', false);
        self.SelectedTable(table.TableId());
        self.SelectedTableName(table.TableNumber());
        $("input[name=staff][value=" + table.AssignedTo() + "]").prop('checked', true);
        addWaitList.modal('show');
    }

    self.LoadStaffs = function () {
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
                    var mappedbusinesses = $.map(data.data, function (item) {
                        return new Staff(item);
                    });
                    self.Staffs(mappedbusinesses);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            });
    };

    self.Assign = function () {
        var checkedStaff = $("input[name='staff']:checked").val();
        if (!checkedStaff) {
            alert('Please select staff to assign table.');
            return false;
        }

        $.ajax({
            type: "POST",
            url: webserviceURL + "restaurant/assigntable",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON({ RestaurantChainId: rcid, TableId: self.SelectedTable(), UserId: checkedStaff }),
            headers: {
                'Authorization': 'Bearer ' + $.cookie("token")
            },
            xhrFields: {
                withCredentials: true
            }
        })
            .done(function (data) {
                if (data.status == "ok") {
                    self.LoadList();
                    NotifySavedMessage("Table assigned.");
                    addWaitList.modal('hide');
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })
    };

    self.LoadList();
    self.LoadStaffs();

    setInterval(function () {
        self.LoadList();
    }, 60000);

}

var seatingViewModel = new SeatingViewModel();
ko.applyBindings(seatingViewModel, document.getElementById("content-wrapper"));


function TableSeating(data) {
    var $table = this;
    $table.TableId = ko.observable(data.TableId);
    $table.TableNumber = ko.observable(data.TableNumber);
    $table.TableType = ko.observable(data.TableType);
    $table.MaxSeating = ko.observable(data.MaxSeating);
    $table.UserName = ko.computed(function () {
        if (data.AssignedTo)
            return data.UserName;
        else
            return 'Assign Staff';
    });
    $table.AssignedTo = ko.observable(data.AssignedTo);
    $table.AvgTime = ko.computed(function () {
        return data.AvgTime + 'min';
    });
    $table.AvgTimeInMinute = ko.observable(data.AvgTimeInMinute);
    $table.TableStatus = ko.observable(data.TableStatus);
    $table.Avg = ko.computed(function () {
        if (data.TableStatus == 'red')
            return data.AvgTimeInMinute + 'min';
        else if (data.TableStatus == 'yellow')
            return data.AvgTimeInMinute + 'min';
        else
            return "";
    });
    $table.AvgClass = ko.computed(function () {
        if (data.TableStatus == 'red')
            return "green-label";
        else if (data.TableStatus == 'yellow')
            return "red-label";
        else
            return "";
    });
    $table.Status = ko.computed(function () {
        if (data.TableStatus == 'green')
            return '<li class="pop-dot green"></li><li class="pop-dot"></li><li class="pop-dot"></li>';
        else if (data.TableStatus == 'red')
            return '<li class="pop-dot"></li><li class="pop-dot"></li><li class="pop-dot red"></li>';
        else
            return '<li class="pop-dot"></li><li class="pop-dot yellow"></li><li class="pop-dot"></li>';
    });

    $table.displayClose = ko.computed(function () {
        if (data.TableStatus == 'red')
            return true;
        else if (data.TableStatus == 'yellow')
            return true;
        else
            return false;
    });
}
function Staff(data) {
    this.UserName = ko.protectedObservable(data.UserName);
    this.UserId = ko.protectedObservable(data.UserId);
    this.Title = ko.protectedObservable(data.Title);
}