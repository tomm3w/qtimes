$('body').addClass('businessprofile_body accont_coms concert__event concert_seating');

var businessProfileViewModel = new BusinessProfileViewModel();
ko.applyBindings(businessProfileViewModel);

function BusinessProfileViewModel() {
    var self = this;

    self.SeatType = ko.observable();
    self.SeatMapPath = ko.observable();
    self.SeatMapFullPath = ko.observable();
    self.AllowCheckInTime = ko.observable();
    self.ConcertSeatings = ko.observableArray([]);
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
                self.SeatType(data.data.SeatType);
                self.SeatMapPath(data.data.SeatMapPath);
                self.SeatMapFullPath(data.data.SeatMapFullPath);
                self.AllowCheckInTime(data.data.AllowCheckInTime);
                var tables = $.map(data.data.ConcertSeatings, function (item) {
                    return new ConcertSeating(item.Id, item.Name, item.Spots, item.SeatsPerSpot, item.CheckInTime);
                });
                self.ConcertSeatings(tables);
                $("#accountlogo").attr("src", data.data.SeatMapFullPath);
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    }

    self.UpdateSeating = function () {
        ShowLoader();
        $.ajax({
            type: 'PUT',
            url: webserviceURL + "concertreservationsetting/UpdateConcertSeating",
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON({
                SeatType: self.SeatType(),
                SeatMapPath: self.SeatMapPath(),
                AllowCheckInTime: self.AllowCheckInTime(),
                ConcertSeatings: self.ConcertSeatings(),
                ConcertId: self.ConcertId()
            }),
            success: function (data) {
                $.notify("Seatings updated successfully.", "success");
                HideLoader();
            },
            error: function (xhr, status, error) {
                HideLoader();
                ShowErrorMessage(xhr, status, error);
            }
        });
    };

    self.AddSeats = function () {
        self.ConcertSeatings.push(new ConcertSeating(0, '', 1, 1, '00:00:00'));
    };

    self.Load();
}

function ConcertSeating(Id, Name, Spots, SeatsPerSpot, CheckInTime) {
    var $res = this;
    $res.Id = ko.observable(Id);
    $res.Name = ko.observable(Name);
    $res.Spots = ko.observable(Spots);
    $res.SeatsPerSpot = ko.observable(SeatsPerSpot);
    $res.CheckInTime = ko.observable(CheckInTime);
    $res.Delete = function (number) {
        businessProfileViewModel.ConcertSeatings.remove(number);
    };
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
                    businessProfileViewModel.SeatMapPath(result.data.ImageName);

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
        FileUpload(e,);
    }
    else {
        $('#customtxt').innerHTML = 'No file choosen';
    }
});