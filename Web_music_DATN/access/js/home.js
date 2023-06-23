
$(document).ready(function () {

    loadDataSongtrend();
    loadDataSingerTrend();
    loadDataAlbumtrend();
});

function loadDataSongtrend() {
    $.ajax({
        url: '/HomeUser/LoadDataSongTrend',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<h4 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Bài hát có lượt truy cập nhiều nhất</h4>';
            $.each(result, function (key, item) {
                html += '<div class="col-2">';
                html += '<a href="#" onclick="openModal()">'
                html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                html += '<img src="/' + item.image + '" class="align-self-center thumb" style="padding-top:10px">';
                html += '<h5 class="card-text text-center p-card" style="width:100%;">' + item.name + '</h5>';
                html += '<p class="card-text text-center p-card" style="width:100%;">Cs. ' + item.tencasi + '</p>';
                html += '</div>';
                html += '</a>'
                html += '</div>';
            });
            $('#song_trend').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataSingerTrend() {
    $.ajax({
        url: '/HomeUser/LoadDataSingerTrend',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<h4 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Ca sĩ có lượt truy cập nhiều nhất</h4>';
            $.each(result, function (key, item) {
                html += '<div class="col-2">';
                html += '<a href="#" onclick="openModal()">'
                html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                html += '<img src="/' + item.image + '" class="align-self-center thumb" style="padding-top:10px">';
                html += '<p class="text-center p-card " style="width:100%; margin-top:4px">Cs. ' + item.name + '</p>';
                html += '</div>';
                html += '</a>'
                html += '</div>';
            });
            $('#singer_trend').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataAlbumtrend() {
    $.ajax({
        url: '/HomeUser/Albumtrend',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result)
            var html = '<h4 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Album có lượt truy cập nhiều nhất</h4>';
            $.each(result.ds, function (key, item) {
                html += '<div class="col-2">';
                html += '<a href="#" onclick="openModal()">'
                html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                html += '<img src="/Image/playlist.jpg" class="align-self-center thumb" style="padding-top:10px">';
                html += '<div class="container-fluid">'
                html += '<div class="row">'
                html += '<h5 class="card-text text-center p-card" style="width:100%;">' + item.name + '</h5>';
                html += '</div>';
                html += '<div class="row">'
                html += '<p class="card-text text-center p-card" style="width:100%;">Cs. ' + item.tencasi + '</p>';
                html += '</div>';
                html += '</div>';
                html += '</div>';
                html += '</a>'
                html += '</div>';
            });
            $('#album_trend').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function logout() {
    var ans1 = confirm("Bạn có chắc muốn đăng xuất?");
    if (ans1) {
        window.location = "/Logout/Index"
    }
}
function openModal(){
    $('#myModal').modal('show');
}