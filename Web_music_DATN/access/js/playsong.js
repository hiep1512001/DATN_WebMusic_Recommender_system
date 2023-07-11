$(document).ready(function () {
    loadDataPlaylist();
    loadData();
});
const song = document.getElementById("song");
const playBtn = document.querySelector(".player-inner");
const nextBtn = document.querySelector(".play-forward");
const prevBtn = document.querySelector(".play-back");
const durationTime = document.querySelector(".duration");
const remainingTime = document.querySelector(".remaining");
const rangeBar = document.querySelector(".range");
const musicName = document.querySelector(".music-name");
const musicThumbnail = document.querySelector(".music-thumb");
const musicImage = document.querySelector(".music-thumb img");
const playRepeat = document.querySelector(".play-repeat");
const playlist = $(".playlist");
const cd = $(".cd");
const cdWidth = cd.offsetWidth;
const playRandom = document.querySelector(".play-infinite");
var liPlaylist = ``;
var idSonglist = 0;
let idSong = 0;
let isPlaying = true;
let indexSong = 0;
let isRepeat = false;
let isRandom = false;
// const musics = ["holo.mp3", "summer.mp3", "spark.mp3", "home.mp3"];
/**
 * Music
 * id: 1
 * title: Holo
 * file: holo.mp3
 * image: unsplash
 */
let timer;
let repeatCount = 0;

const musics = [

];
function Add(){
    var max = rangeBar.max;
    var value = rangeBar.value;
    var id = idSong;
    var data = new FormData;
    data.append("max", max)
    data.append("value", value)
    data.append("idSong", id)
    $.ajax({
        url: "/PlaySong/AddRating",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataPlaylist() {
    $.ajax({
        url: '/Playlist/LoadData',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result)
            $.each(result, function (key, item) {
                liPlaylist += `<li><a class="dropdown-item" onclick="AddSongList( `+ item.id +`)">Thêm vào ` + item.name + `</a></li>`
            });          
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function AddSongList(id){
    var data = new FormData;
    data.append("idsong", idSonglist)
    data.append("idplaylist", id)
    $.ajax({
        url: "/PlaySong/AddSongList",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {

            alert(result.msd)
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function getID(id) {
    idSonglist = id;
    console.log(id);
};

function loadData() {
    $.ajax({
        url: '/PlaySong/LoadData',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result);
            for (var i = 0; i < result.length+1; i++){
        musics.pop();
            }
            var html = "";
            var data_index = 0;
            $.each(result, function (key, item) {
                html += `
                        <div class="song ${data_index == indexSong ? "active" : ""
                    }" >
                            <div class="thumb"
                                style="background-image: url('${item.image}')">
                            </div>
                            <div class="body" id=" ` + data_index + `"onclick='select_song(this)' >
                                <h3 class="title">`+ item.name + `</h3>
                                <p class="author">${item.tencasi}</p>
                            </div>
                             <div class="option">
                                <i class="fas fa-ellipsis-h" data-bs-toggle="dropdown" onclick="getID(`+ item.id +`)" ></i>
                                  <ul class="dropdown-menu">`;
                html += liPlaylist;
                            html+=`</ul>
                            </div>
                    </div>
                    `;
                data_index = data_index + 1;
                var obj = {
                    id: item.id,
                    name: item.name,
                    singer: item.tencasi,
                    file: item.song,
                    image: item.image,
                    tencasi: item.tencasi
                }
                musics.push(obj);
            });
            $('.playlist').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
playRepeat.addEventListener("click", function () {
    if (isRepeat) {
        isRepeat = false;
        playRepeat.removeAttribute("style");
    } else {
        isRepeat = true;
        playRepeat.style.color = "#ffb86c";
    }
});
playRandom.addEventListener("click", function () {
    if (isRandom) {
        isRandom = false;
        playRandom.removeAttribute("style");
    } else {
        isRandom = true;
        playRandom.style.color = "#ffb86c";
    }
});
nextBtn.addEventListener("click", function () {
    Add();
    if (isRandom) {
        changeSongRandom();
    }
    else {
        changeSong(1);
    } 
    loadData();

});
prevBtn.addEventListener("click", function () {
    Add();
    if (isRandom) {
        changeSongRandom();
    }
    else {
        changeSong(-1);
    }   
    loadData();
});
song.addEventListener("ended", handleEndedSong);
function handleEndedSong() {
    if (isRepeat) {
        Add();
        // handle repeat song
        isPlaying = true;
        playPause();
    } else {
        Add();
        if (isRandom) {
            changeSongRandom();
        }
        else {
            changeSong(1);
        }
        loadData();
    }
}
function changeSongRandom() {
    var i = indexSong
    do {
        indexSong = Math.floor(Math.random() * musics.length)
    }
    while (indexSong===i)
    isPlaying = true;
    init(indexSong);
    playPause();
}
function changeSong(dir) {
    if (dir === 1) {
        // next song
        indexSong++;
        repeatCount++;
        if (indexSong >= musics.length) {
            indexSong = 0;
        }
        isPlaying = true;
    } else if (dir === -1) {
        // prev song
        indexSong--;
        repeatCount--;
        if (indexSong < 0) {
            indexSong = musics.length - 1;
        }
        isPlaying = true;
    }
    init(indexSong);
    // song.setAttribute("src", `./music/${musics[indexSong].file}`);
    playPause();
}
playBtn.addEventListener("click", playPause);
function playPause() {
    if (isPlaying) {
        musicThumbnail.classList.add("is-playing");
        song.play();
        playBtn.innerHTML = `<ion-icon name="pause-circle"></ion-icon>`;
        isPlaying = false;
        timer = setInterval(displayTimer, 500);
    } else {
        musicThumbnail.classList.remove("is-playing");
        song.pause();
        playBtn.innerHTML = `<ion-icon name="play"></ion-icon>`;
        isPlaying = true;
        clearInterval(timer);
    }
}
function displayTimer() {
    const { duration, currentTime } = song;
    rangeBar.max = duration;
    rangeBar.value = currentTime;
    remainingTime.textContent = formatTimer(currentTime);
    if (!duration) {
        durationTime.textContent = "00:00";
    } else {
        durationTime.textContent = formatTimer(duration);
    }
}
function formatTimer(number) {
    const minutes = Math.floor(number / 60);
    const seconds = Math.floor(number - minutes * 60);
    return `${minutes < 10 ? "0" + minutes : minutes}:${seconds < 10 ? "0" + seconds : seconds
        }`;
}
rangeBar.addEventListener("change", handleChangeBar);
function handleChangeBar() {
    song.currentTime = rangeBar.value;
}
function init(indexSong) {
    
    song.setAttribute("src", `./${musics[indexSong].file}`);
    musicImage.setAttribute("src","./"+musics[indexSong].image);
    musicName.textContent = musics[indexSong].name +"-"+ musics[indexSong].tencasi;
    idSong = musics[indexSong].id;
}
function select_song(e) {
    Add();
    indexSong = parseInt(e.id);
    repeatCount = parseInt(e.id);
    isPlaying = true;
    displayTimer();
    init(indexSong);
    playPause();
    loadData()
}
/*document.onscroll = function () {
    const scrollTop = window.scrollY || document.documentElement.scrollTop;
    const newCdWidth = cdWidth - scrollTop;
    console.log("1");
    cd.style.width = newCdWidth > 0 ? newCdWidth + "px" : 0;
    cd.style.opacity = newCdWidth / cdWidth;
};*/
displayTimer();
init(indexSong);
function logout() {
    var ans1 = confirm("Bạn có chắc muốn đăng xuất?");
    if (ans1) {
        window.location = "/Logout/Index"
    }
}
