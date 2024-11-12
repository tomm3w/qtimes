function SeatingViewModel() {
    var self = this;
    var cookieValue = $.cookie("rcid");
    if (cookieValue) {
        rcid = cookieValue;
    }

    self.TableStatus = ko.observableArray([]);

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
                if (data.status === "ok") {
                    var tables = $.map(data.data, function (item) {
                        var table = $('#table-' + item.TableNumber);
                        table.removeClass('avaiable occupid leave not-avai');
                        table.removeAttr("style");
                        table.removeAttr("onclick");
                        if (item.TableStatus === 'green') {
                            table.addClass('avaiable');
                        }
                        else if (item.TableStatus === 'red') {
                            table.addClass('occupid');
                            table.attr("style", "cursor:pointer");
                            table.attr("onclick", "seatingViewModel.CloseTable(" + item.TableId + ")");
                        }
                        else if (item.TableStatus === 'yellow') {
                            table.addClass('leave');
                            table.attr("style", "cursor:pointer");
                            table.attr("onclick", "seatingViewModel.CloseTable(" + item.TableId + ")");
                        }
                        else
                            table.addClass('not-avai');
                    });
                    self.TableStatus(tables);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            })

    };

    self.CloseTable = function (tableid) {
        if (confirm('Are you sure to close this table?')) {
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
        }
    };

    self.LoadList();

    setInterval(function () {
        self.LoadList();
    }, 60000);

}

var seatingViewModel = new SeatingViewModel();
ko.applyBindings(seatingViewModel, document.getElementById("content-wrapper"));