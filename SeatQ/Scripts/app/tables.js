var isAdd = null;
$('body').addClass('setting_table accont_coms');
var addWaitList = $("#add-waitlist");
$("#addList").click(function () {
    PrepareAdd();
});

function TableViewModel() {
    var self = this;
    self.RestaurantChainId = ko.observable();
    self.TableNumber = ko.observable();
    self.TableTop = ko.observable();
    self.TableType = ko.observableArray([]);
    self.businesses = ko.observableArray([]);
    self.TableTypeId = ko.observable();
    self.TableId = ko.observable();
    self.IsOut = ko.observable();
    self.IsAvailable = ko.observable();

    self.LoadList = function () {
        //var qs = '&Page=' + self.pagingdata().Page() + '&PageSize=' + self.pagingdata().PageSize + '&TotalPages=' + self.pagingdata().TotalPages() + '&TotalData=' + self.pagingdata().TotalData;

        $.ajax({
            type: "GET",
            url: webserviceURL + "tables/?RestaurantChainId=" + rcid,// + qs,
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
            })
            .error(function () {
            });

    };
    self.Add = function () {
        self.RestaurantChainId(rcid);
        self.IsAvailable(!self.IsOut());

        if (isAdd) {
            $.ajax({
                type: "POST",
                url: webserviceURL + 'tables',
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
                    else if (data.status == "conflict") {
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
                url: webserviceURL + "tables",
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
    self.Edit = function (TableId) {
        $.ajax({
            type: "GET",
            url: webserviceURL + "tables/?TableId=" + TableId,
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

                    PrepareUpdate();

                    self.TableId(data.data.TableId);
                    self.TableNumber(data.data.TableNumber);
                    self.TableTypeId(data.data.TableTypeId);
                    self.IsOut(data.data.IsAvailable === false ? true : false);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            }).
            error(function (xhr, status, error) {
                ShowErrorMessage(xhr, status, error)
            });
    };
    setData = function (data) {
        //var pagingdata = new PagingData(data.data.businesses.PagingData);
        //self.pagingdata(pagingdata);
        var mappedbusinesses = $.map(data.data.Businesses, function (item) {
            return new Business(item);
        });
        self.businesses(mappedbusinesses);

        var tabletype = [];
        $.map(data.data.Tables, function (item) {
            tabletype.push({ "text": item.Title, "value": item.TableTypeId });
        });
        self.TableType(tabletype);

    };

    self.LoadList();
}

var tableViewModel = new TableViewModel();
ko.applyBindings(tableViewModel, document.getElementById("content-wrapper"));

function HidePopup() {
    $("#add-table").hide();
    overlay.hide('fade');
}

function Business(data) {
    var $super = this;
    this.TableId = ko.protectedObservable(data.TableId);
    this.TableNumber = ko.protectedObservable(data.TableNumber);
    this.TableTypeId = ko.protectedObservable(data.TableTypeId);
    this.Title = ko.protectedObservable(data.Title);
    this.MaxSeating = ko.protectedObservable(data.MaxSeating);
    this.IsOut = ko.computed(function () {
        if (data.IsAvailable)
            return 'No';
        else
            return 'Yes';
    });
}

function ClearAll() {
    isAdd = null;
    addWaitList.modal('hide');
    tableViewModel.TableNumber(undefined);
    tableViewModel.TableTop(undefined);
}

function PrepareAdd() {
    ClearAll();
    isAdd = true;
    $("#myModalLabel").text("New Table");
    $("#btnSubmit").text("Add");
}

function PrepareUpdate() {
    addWaitList.modal('show');
    $("#myModalLabel").text("Edit Table");
    $("#btnSubmit").text("Update");
    isAdd = false;
    addWaitList.modal('show');
}