$('body').addClass('businessprofile_body accont_coms');


$(function () {
    'use strict';
    // Change this to the location of your server-side upload handler:

    $('#formimageupload').fileupload({
        url: webserviceURL + "restaurant/upload",
        autoUpload: true,
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + $.cookie("token")
        },
        xhrFields: {
            withCredentials: true
        },
        add: function (e, data) {
            data.submit();
        },
        done: function (e, data) {
            if (data.result.status == 'ok') {
                var data = data.result.data;
                $("#logo").attr("src", data.ImagePath);
            }
            else if (data.result.status == "badrequest") {
                showErrorMessage(data.result);
            }
        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled')
    .bind('fileuploadsubmit', function (e, data) {
        data.formData = {
            'RestaurantChainId': rcid
        };
    });

    $('#formtableimageupload').fileupload({
        url: webserviceURL + "restaurant/uploadtablelayout",
        autoUpload: true,
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + $.cookie("token")
        },
        xhrFields: {
            withCredentials: true
        },
        add: function (e, data) {
            data.submit();
        },
        done: function (e, data) {
            if (data.result.status == 'ok') {
                var data = data.result.data;
                $("#tableLayout").attr("src", data.ImagePath);
            }
            else if (data.result.status == "badrequest") {
                showErrorMessage(data.result);
            }
        }
    }).prop('disabled', !$.support.fileInput)
       .parent().addClass($.support.fileInput ? undefined : 'disabled')
   .bind('fileuploadsubmit', function (e, data) {
       data.formData = {
           'RestaurantChainId': rcid
       };
   });
});

function RestaurantInfoViewModel() {
    var self = this;
    self.RestaurantId = ko.observable();
    self.RestaurantChainId = ko.observable();
    self.BusinessName = ko.observable();
    self.LogoPath = ko.observable();
    self.TableLayoutPath = ko.observable();
    self.FullName = ko.observable();
    self.Email = ko.observable();
    self.Address1 = ko.observable();
    self.Address2 = ko.observable();
    self.Phone = ko.observable();
    self.CityTown = ko.observable();
    self.State = ko.observable();
    self.Zip = ko.observable();
    self.RestaurantNumber = ko.observable();

    self.ImageSrc = ko.computed(function () {
        if (self.LogoPath())
            return self.LogoPath();
        else
            return '/images/noimage.png';
    });
    self.TableLayoutSrc = ko.computed(function () {
        if (self.TableLayoutPath())
            return self.TableLayoutPath();
        else
            return '/images/noimage.png';
    });
    self.Load = function () {
        $.ajax({
            type: "GET",
            url: webserviceURL + "restaurant/?RestaurantChainId=" + rcid,
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
                    self.RestaurantId(data.data.RestaurantId);
                    self.RestaurantChainId(data.data.RestaurantChainId);
                    self.BusinessName(data.data.BusinessName);
                    self.LogoPath(data.data.LogoPath);
                    self.TableLayoutPath(data.data.TableLayoutPath);
                    self.FullName(data.data.FullName);
                    self.Email(data.data.Email);
                    self.Address1(data.data.Address1);
                    self.Address2(data.data.Address2);
                    self.Phone(data.data.Phone);
                    self.CityTown(data.data.CityTown);
                    self.State(data.data.State);
                    self.Zip(data.data.Zip);
                    self.RestaurantNumber(data.data.RestaurantNumber);
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            });
    };
    self.Update = function () {
        $.ajax({
            type: "PUT",
            url: webserviceURL + "restaurant",
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
                }
                else if (data.status == "badrequest") {
                    showErrorMessage(data);
                }
            }).error(function (xhr, status, error) {
                ShowErrorMessage(xhr, status, error)
            });
    }

    self.Load();
}

var restaurantInfoViewModel = new RestaurantInfoViewModel();
ko.applyBindings(restaurantInfoViewModel, document.getElementById("content-wrapper"));