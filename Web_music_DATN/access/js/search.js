$('#search').keyup(function () {
    var searchText = $('#search').val().trim().toLowerCase();
    if (searchText == "") {
        let toremovesong = document.querySelector("#bodymusic");
        toremovesong.parentNode.removeChild(toremovesong)
        let toremovesinger = document.querySelector("#bodysinger");
        toremovesinger.parentNode.removeChild(toremovesinger)
        let toremovealbum = document.querySelector("#bodyalbum");
        toremovealbum.parentNode.removeChild(toremovealbum)
    }
    else {
        var html = '';
        html+= '<div class="row" id="bodymusic"></div>';
        html += '<div class="row" id="bodysinger"></div>';
        html += '<div class="row" id="bodyalbum"></div>';
        $('#content').html(html);
        loadDataSong(searchText);
        loadDataSinger(searchText);
        loadDataAlbum(searchText);
    }  
})
function loadDataSong(searchtext) {
    $.ajax({
        url: '/Search/Song',
        type: 'get',
        data: {
            tukhoa:searchtext
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<h3 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Bài hát</h3>';
            console.log(result)
            $.each(result, function (key, item) {
                html += '<a href="/Search/PlaySong?id=' + item.id + '">'
                html += `
                        <div class="songSearch">
                            <div class="thumb"
                                style="background-image: url(../`+ item.image+`)">
                            </div>
                            <div class="body">
                                <h3 class="title searchtext" ">${item.name}</h3>
                                <p class="author searchtext" ">${item.tencasi}</p>
                            </div>
                            <div class="option">
                                <p style="color: black;">Lượt nghe: `+ item.views +`</p>
                            </div>
                        </div>
                    `
                html += '</a>';
            });
            $('#bodymusic').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataSinger(searchtext) {
    $.ajax({
        url: '/Search/Singer',
        type: 'get',
        data: {
            tukhoa: searchtext
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<h3 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Ca sĩ</h3>';
            $.each(result, function (key, item) {            
                html += '<div class="col-2">';
                html += '<a href="/Search/PlaySongSinger?id=' + item.id + '">'
                html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                html += '<img src="/' + item.image +'" class="align-self-center thumb" style="padding-top:10px">';
                html += '<p class="text-center p-card" style="width:100%; margin-top:4px">' + item.name + '</p>';
                html += '</div>';
                html += '</a>'
                html += '</div>';
/*                <div class="col-2">
                    <div class="card" style="width: 100%;">
                        <img src="/Image/ha_anh_tuan.jpg" class="card-img-top" alt="...">
                            <div class="card-body">
                                <h5 class="card-title">Cứ thế</h5>
                                <p class="card-text">Hà Anh Tuấn</p>
                            </div>
                        </div>
                    </div>*/
            });
            $('#bodysinger').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataAlbum(searchtext) {
    $.ajax({
        url: '/Search/Album',
        type: 'get',
        data: {
            tukhoa: searchtext
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<h3 style="box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">Album</h3>';
            $.each(result, function (key, item) {
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
            $('#bodyalbum').html(html);
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
