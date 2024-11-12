$('body').addClass('businessprofile_body accont_coms ');
$('#timezone').timezones();
const over_in_input = document.getElementById('over_in_input');
const show_img = document.getElementById('show_img');
show_img.addEventListener('click', function () {
    event.preventDefault();
    over_in_input.click();
});
over_in_input.addEventListener('change', function (e) {
    if (over_in_input.value) {
        $('#customtxt').innerHTML = over_in_input.value;
        FileUpload(e);
    }
    else {
        $('#customtxt').innerHTML = 'No file choosen';
    }
});
function downloadImage() {
    var a = document.createElement('a');
    a.href = document.getElementById("qrcode").src;
    a.download = "qrcode.png";
    document.body.appendChild(a);
    a.click();
}
function loadQrCode(data) {
    if (data) {
        try {
            let canvas = document.createElement('canvas');
            bwipjs.toCanvas(canvas, {
                bcid: 'qrcode',
                text: data,
                scale: 5
            });
            document.getElementById("qrcode").src = canvas.toDataURL('image/png');
        } catch (e) { }
    }
    else {
        document.getElementById("qrcode").src = '/images/noimage.png';
    }
}


var businessProfileViewModel = new BusinessProfileViewModel();
ko.applyBindings(businessProfileViewModel);

function BusinessProfileViewModel() {
    var self = this;

    self.PassTemplateId = ko.observable();
    self.BusinessName = ko.observable();
    self.FullName = ko.observable();
    self.Email = ko.observable();
    self.LogoPath = ko.observable();
    self.Address = ko.observable();
    self.City = ko.observable();
    self.State = ko.observable();
    self.Zip = ko.observable();
    self.VirtualNo = ko.observable();
    self.PhoneNo = ko.observable();
    self.MobileNo = ko.observable();
    self.TimezoneOffset = ko.observable();
    self.TimezoneOffsetValue = ko.observable();
    self.ShortUrl = ko.observable();
    self.MobilePageUrl = ko.observable();
    self.BusinessId = ko.computed(function () {
        return $.cookie("businessid");
    });

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "reservationsetting/getbusiness/" + self.BusinessId(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.PassTemplateId(data.data.PassTemplateId);
                self.BusinessName(data.data.BusinessName);
                self.FullName(data.data.FullName);
                self.Email(data.data.Email);
                self.LogoPath(data.data.LogoPath);
                self.Address(data.data.Address);
                self.City(data.data.City);
                self.State(data.data.State);
                self.Zip(data.data.Zip);
                self.VirtualNo(data.data.VirtualNo);
                self.PhoneNo(data.data.PhoneNo);
                self.MobileNo(data.data.MobileNo);
                self.TimezoneOffset(data.data.TimezoneOffset);
                self.TimezoneOffsetValue(data.data.TimezoneOffsetValue);
                self.ShortUrl(data.data.ShortUrl);
                self.MobilePageUrl(window.location.origin + '/m/reservation/business/' + self.BusinessId());
                $("#accountlogo").attr("src", data.data.LogoFullPath);
                $('#timezone').val(data.data.TimezoneOffsetValue);
                loadQrCode(data.data.ShortUrl);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.UpdateProfile = function () {
        ShowLoader();
        self.TimezoneOffset($('#timezone option:selected').attr('data-offset'));
        self.TimezoneOffsetValue($('#timezone').val());
        $.ajax({
            type: 'PUT',
            url: webserviceURL + "reservationsetting/UpdateProfile",
            data: {
                BusinessName: self.BusinessName(),
                FullName: self.FullName(),
                Email: self.Email(),
                LogoPath: self.LogoPath(),
                Address: self.Address(),
                City: self.City(),
                State: self.State(),
                Zip: self.Zip(),
                VirtualNo: self.VirtualNo(),
                PhoneNo: self.PhoneNo(),
                MobileNo: self.MobileNo(),
                TimezoneOffset: self.TimezoneOffset(),
                TimezoneOffsetValue: self.TimezoneOffsetValue(),
                ShortUrl: self.ShortUrl(),
                PassTemplateId: self.PassTemplateId(),
                Id: self.BusinessId()
            },
            success: function (data) {
                $.notify("Business profile updated successfully.", "success");
                loadQrCode(self.ShortUrl());
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.Load();
}

function FileUpload(e) {

    var files = e.target.files;
    var uploaderId = e.target.id;

    if (files.length > 0) {
        if (window.FormData !== undefined) {

            ShowLoader();

            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
            }

            $.ajax({
                type: "POST",
                url: webserviceURL + "reservationsetting/upload",
                contentType: false,
                processData: false,
                data: data,
                headers: {
                    'Authorization': 'Bearer ' + $.cookie("token")
                },
                success: function (result) {
                    $("#accountlogo").attr("src", result.data.ImageFullPath);
                    businessProfileViewModel.LogoPath(result.data.ImageName);
                    HideLoader();
                },
                error: function (xhr, status, p3) {
                    HideLoader();
                    var err = "Error " + " " + status + " " + p3;
                    if (xhr.responseText && xhr.responseText[0] === "{") {
                        err = JSON.parse(xhr.responseText).message;
                        $.notify(err, "error");
                    }
                    else
                        ShowErrorMessage(xhr, status, p3);
                }
            });
        }
    }
}