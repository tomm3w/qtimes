function TableViewModel() {
    var self = this;
    
    self.Waiting = ko.observable();
    self.TotalSeated = ko.observable();
    self.C1_4Inline = ko.observable();
    self.C5_6Inline = ko.observable();
    self.C7_8Inline = ko.observable();
    self.C9PlusInline = ko.observable();
    self.C1_4Turnline = ko.observable();
    self.C5_6Turnline = ko.observable();
    self.C7_8Turnline = ko.observable();
    self.C9PlusTurnline = ko.observable();

    self.LoadTableSummary = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + "waitlist/GetTableSummary/?RestaurantChainId=" + rcid,
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
                    self.Waiting(data.data.Waiting);
                    self.TotalSeated(data.data.Seated);
                    self.C1_4Inline(data.data.C1_4Inline);
                    self.C5_6Inline(data.data.C5_6Inline);
                    self.C7_8Inline(data.data.C7_8Inline);
                    self.C9PlusInline(data.data.C9PlusInline);
                    self.C1_4Turnline(data.data.C1_4Turnline);
                    self.C5_6Turnline(data.data.C5_6Turnline);
                    self.C7_8Turnline(data.data.C7_8Turnline);
                    self.C9PlusTurnline(data.data.C9PlusTurnline);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            });
    };
    self.LoadTableSummary();
}
ko.applyBindings(new TableViewModel, document.getElementById("tableSummary"));  