function BusinessViewModel() {
    var self = this;
    self.Regions = ko.observableArray([]);
    self.RegionName = ko.observable();
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
        if (data.Regions.length > 0)
        {
            var regions = [];
            $.map(data.Regions, function (item) {
                regions.push({ "text": item.Name, "value": item.Name });
            });
            self.Regions(regions);
        }
    });
    };

    self.LoadRegions();
}

var businessViewModel = new BusinessViewModel();
ko.applyBindings(businessViewModel, document.getElementById("header"));