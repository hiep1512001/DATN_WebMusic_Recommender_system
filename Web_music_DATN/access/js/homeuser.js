
$(document).ready(function () {

    loadDataSongtrend();
    loadDataContent();
    loadDataAlbumtrend();
    loadDataSingerTrend();
    loadDataCollab();
});
function loadDataAlbumtrend() {
    $.ajax({
        url: '/HomeUser/Albumtrend',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<h4 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Album có lượt truy cập nhiều nhất</h4>';
            $.each(result.ds, function (key, item) {
                html += '<div class="col-2">';
                html += '<a href="/Search/PlaySongAlbum?id=' + item.id + '">'
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
function loadDataContent() {
    $.ajax({
        url: '/HomeUser/LoadDataContent',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result)
            var html = '';
            if (result.msg == "yes") {
                var html = '<h4 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Gợi ý bài hát dựa vào bài hát đã nghe</h4>';
                $.each(result.ds, function (key, item) {
                    html += '<div class="col-2">';
                    html += '<a href="/HomeUser/PlaySong?id=' + item.id + '">'
                    html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                    html += '<img src="/' + item.image + '" class="align-self-center thumb" style="padding-top:10px">';
                    html += '<h5 class="card-text text-center p-card " style="width:100%;">' + item.name + '</h5>';
                    html += '<p class="card-text text-center p-card" style="width:100%;">Cs. ' + item.tencasi + '</p>';
                    html += '</div>';
                    html += '</a>'
                    html += '</div>';
                });
                $('#goi_y_content').html(html);
            }
            else if (result.msg == "yes1") {
                var html = '<h4 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Có lẽ bạn sẽ thích</h4>';
                $.each(result.ds, function (key, item) {
                    html += '<div class="col-2">';
                    html += '<a href="/HomeUser/PlaySong?id=' + item.id + '">'
                    html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                    html += '<img src="/' + item.image + '" class="align-self-center thumb" style="padding-top:10px">';
                    html += '<h5 class="card-text text-center p-card " style="width:100%;">' + item.name + '</h5>';
                    html += '<p class="card-text text-center p-card" style="width:100%;">Cs. ' + item.tencasi + '</p>';
                    html += '</div>';
                    html += '</a>'
                    html += '</div>';
                });
                $('#goi_y_content').html(html);
            }
            else {
                $('#goi_y_content').css('display', 'none')
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
/*data - bs - toggle="tooltip" data - bs - placement="top"
data - bs - custom - class="custom-tooltip"
data - bs - title="This top tooltip is themed via CSS variables."*/
function loadDataCollab() {
    $.ajax({
        url: '/HomeUser/LoadDataCollab',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            
            if (result.msg == "yes") {
                var html = '<h4 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Gợi ý bài hát dựa vào người nghe tương tự</h4>';
                $.each(result.ds, function (key, item) {
                    html += '<div class="col-2">';
                    html += '<a href="/HomeUser/PlaySong?id=' + item.id + '">'
                    html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                    html += '<img src="/' + item.image + '" class="align-self-center thumb" style="padding-top:10px">';
                    html += '<h5 class="card-text text-center p-card " style="width:100%;">' + item.name + '</h5>';
                    html += '<p class="card-text text-center p-card" style="width:100%;">Cs. ' + item.tencasi + '</p>';
                    html += '</div>';
                    html += '</a>'
                    html += '</div>';
                });
                $('#goi_y_collab').html(html);
            }
            else {
                $('#goi_y_collab').css('display', 'none')
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
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
                html += '<a href="/HomeUser/PlaySong?id=' + item.id + '">'
                html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                html += '<img src="/' + item.image + '" class="align-self-center thumb" style="padding-top:10px">';
                html += '<h5 class="card-text text-center p-card" style="width:100%;" >' + item.name + '</h5>';
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
                html += '<a href="/HomeUser/PlaySongSinger?id=' + item.id + '">'
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
function logout() {
    var ans1 = confirm("Bạn có chắc muốn đăng xuất?");
    if (ans1) {
        window.location = "/Logout/Index"
    }
}
