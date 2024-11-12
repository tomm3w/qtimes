$('body').addClass('businessprofile_body accont_coms concert__event concert_profile_body');

var businessProfileViewModel = new BusinessProfileViewModel();
ko.applyBindings(businessProfileViewModel);

function BusinessProfileViewModel() {
    var self = this;

    self.PassTemplateId = ko.observable();
    self.Description = ko.observable();
    self.ImagePath = ko.observable();
    self.ImageFullPath = ko.observable();
    self.VirtualNo = ko.observable();
    self.PhoneNo = ko.observable();
    self.MobileNo = ko.observable();
    self.EnablePrivacyPolicy = ko.observable();
    self.EnableServiceTerms = ko.observable();
    self.EnableCommunityGuidelines = ko.observable();

    self.PrivacyPolicyFilePath = ko.observable();
    self.PrivacyPolicyFileFullPath = ko.observable();
    self.PrivacyPolicyFileName = ko.computed(function () {
        if (self.PrivacyPolicyFilePath())
            return 'Click to view';
        else
            return 'No file chosen';
    });

    self.ServiceTermsFilePath = ko.observable();
    self.ServiceTermsFileFullPath = ko.observable();
    self.ServiceTermsFileName = ko.computed(function () {
        if (self.ServiceTermsFilePath())
            return 'Click to view';
        else
            return 'No file chosen';
    });

    self.CummunityGuidelinesFilePath = ko.observable();
    self.CummunityGuidelinesFileFullPath = ko.observable();
    self.CummunityGuidelinesFileName = ko.computed(function () {
        if (self.CummunityGuidelinesFilePath())
            return 'Click to view';
        else
            return 'No file chosen';
    });

    self.ConcertId = ko.computed(function () {
        return $.cookie("concertid");
    });

    self.Load = function () {
        ShowLoader();
        $.ajax({
            type: 'GET',
            url: webserviceURL + "concertreservationsetting/concert/" + self.ConcertId(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.Description(data.data.Description);
                self.PassTemplateId(data.data.PassTemplateId);
                self.ImagePath(data.data.ImagePath);
                self.ImageFullPath(data.data.ImageFullPath);
                self.EnablePrivacyPolicy(data.data.EnablePrivacyPolicy);
                self.EnableServiceTerms(data.data.EnableServiceTerms);
                self.EnableCommunityGuidelines(data.data.EnableCommunityGuidelines);
                self.PrivacyPolicyFilePath(data.data.PrivacyPolicyFilePath);
                self.PrivacyPolicyFileFullPath(data.data.PrivacyPolicyFileFullPath);
                self.ServiceTermsFilePath(data.data.ServiceTermsFilePath);
                self.ServiceTermsFileFullPath(data.data.ServiceTermsFileFullPath);
                self.CummunityGuidelinesFilePath(data.data.CummunityGuidelinesFilePath);
                self.CummunityGuidelinesFileFullPath(data.data.CummunityGuidelinesFileFullPath);

                self.VirtualNo(data.data.VirtualNo);
                self.PhoneNo(data.data.PhoneNo);
                self.MobileNo(data.data.MobileNo);

                $("#accountlogo").attr("src", data.data.ImageFullPath);
                $('#staffUrl').val(window.location.origin + '/concert/checkin/?id=' + self.ConcertId());
                $('#mobileUrl').val(window.location.origin + '/m/concert/business/' + self.ConcertId());
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
        $.ajax({
            type: 'PUT',
            url: webserviceURL + "concertreservationsetting/UpdateConcertProfile",
            data: {
                Description: self.Description(),
                ImagePath: self.ImagePath(),
                EnablePrivacyPolicy: self.EnablePrivacyPolicy(),
                EnableServiceTerms: self.EnableServiceTerms(),
                EnableCommunityGuidelines: self.EnableCommunityGuidelines(),
                PrivacyPolicyFilePath: self.PrivacyPolicyFilePath(),
                ServiceTermsFilePath: self.ServiceTermsFilePath(),
                CummunityGuidelinesFilePath: self.CummunityGuidelinesFilePath(),
                ConcertId: self.ConcertId(),
                PassTemplateId: self.PassTemplateId(),
                VirtualNo: self.VirtualNo(),
                PhoneNo: self.PhoneNo(),
                MobileNo: self.MobileNo()
            },
            success: function (data) {
                $.notify("Business profile updated successfully.", "success");
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

function FileUpload(e, type) {

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
                    if (type === 'PrivacyPolicy') {
                        businessProfileViewModel.PrivacyPolicyFilePath(result.data.ImageName);
                        businessProfileViewModel.PrivacyPolicyFileFullPath(result.data.ImageFullPath);
                    }
                    else if (type === 'TermsService') {
                        businessProfileViewModel.ServiceTermsFilePath(result.data.ImageName);
                        businessProfileViewModel.ServiceTermsFileFullPath(result.data.ImageFullPath);
                    }
                    else if (type === 'CummunityGuidelines') {
                        businessProfileViewModel.CummunityGuidelinesFilePath(result.data.ImageName);
                        businessProfileViewModel.CummunityGuidelinesFileFullPath(result.data.ImageFullPath);
                    }
                    else {
                        $("#accountlogo").attr("src", result.data.ImageFullPath);
                        businessProfileViewModel.ImagePath(result.data.ImageName);
                    }
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

const over_in_input = document.getElementById('over_in_input');
const show_img = document.getElementById('show_img');
show_img.addEventListener('click', function () {
    event.preventDefault();
    over_in_input.click();
});
over_in_input.addEventListener('change', function (e) {
    if (over_in_input.value) {
        $('#customtxt').innerHTML = over_in_input.value;
        FileUpload(e, "BusinessImage");
    }
    else {
        $('#customtxt').innerHTML = 'No file choosen';
    }
});

const over_in_input1 = document.getElementById('over_in_input1');
const show_img1 = document.getElementById('show_img1');
show_img1.addEventListener('click', function () {
    event.preventDefault();
    over_in_input1.click();
});
over_in_input1.addEventListener('change', function (e) {
    if (over_in_input1.value) {
        $('#customtxt1').innerHTML = over_in_input1.value;
        FileUpload(e, 'PrivacyPolicy');
    }
    else {
        $('#customtxt1').innerHTML = 'No file choosen';
    }
});

const over_in_input2 = document.getElementById('over_in_input2');
const show_img2 = document.getElementById('show_img2');
show_img2.addEventListener('click', function () {
    event.preventDefault();
    over_in_input2.click();
});
over_in_input2.addEventListener('change', function (e) {
    if (over_in_input2.value) {
        $('#customtxt2').innerHTML = over_in_input2.value;
        FileUpload(e, 'TermsService');
    }
    else {
        $('#customtxt2').innerHTML = 'No file choosen';
    }
});

const over_in_input3 = document.getElementById('over_in_input3');
const show_img3 = document.getElementById('show_img3');
show_img3.addEventListener('click', function () {
    event.preventDefault();
    over_in_input3.click();
});
over_in_input3.addEventListener('change', function (e) {
    if (over_in_input3.value) {
        $('#customtxt3').innerHTML = over_in_input3.value;
        FileUpload(e, 'CummunityGuidelines');
    }
    else {
        $('#customtxt3').innerHTML = 'No file choosen';
    }
});