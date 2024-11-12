$.ajax({
    type: "GET",
    url: webserviceURL + "restaurant/restaurantlist",
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
            var s = $('#selectCompany');
            $.map(data.data, function (item) {
                $("<option />", { value: item.RestaurantChainId, text: item.BusinessName }).appendTo(s);
            });

            s.change(function () {
                rcid = this.value;
                $.cookie("rcid", rcid, { expires: 1 });
                if (typeof metricsViewModel != "undefined")
                    metricsViewModel.LoadQuery();
                else if (typeof returnGuestViewModel != "undefined")
                    returnGuestViewModel.LoadList();
                else if (typeof messageListViewModel != "undefined")
                    messageListViewModel.Load();
                else if (typeof waitListViewModel != "undefined")
                    waitListViewModel.LoadList();
                else if (typeof restaurantInfoViewModel != "undefined")
                    restaurantInfoViewModel.Load();
                else if (typeof accountInfoViewModel != "undefined")
                    accountInfoViewModel.Load();
                else if (typeof preferenceViewModel != "undefined")
                    preferenceViewModel.Load();
                else if (typeof hostessViewModel != "undefined")
                    hostessViewModel.LoadList();
                else if (typeof tableViewModel != "undefined")
                    tableViewModel.LoadList();
                else if (typeof seatingViewModel != "undefined")
                    seatingViewModel.LoadList();
            });

            s.show();
            var cookieValue = $.cookie("rcid");
            if (cookieValue) {
                s.val(cookieValue);
                rcid = cookieValue;
                s.trigger("change");
            }
        }
        else if (data.status == "badrequest") {
            showErrorMessage(data);
        }
    });
