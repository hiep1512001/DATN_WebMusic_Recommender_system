//Load Data in Table when documents is ready
$(document).ready(function () {

    loadData(TuKhoa, Trang);
});
var MaxTrang = 1;
var Trang = 1;
var TuKhoa = "";

var idComposer = 0;
var nameComposer = "";
var idAlbum = 0;
var idSinger = 0;
var idGenre = 0;
var nameAlbum = "";
var nameSinger = "";
var nameGenre = "";
$('#search').keyup(function () {
    var searchText = $('#search').val().toLowerCase();
    Trang = 1;
    TuKhoa = searchText;
    /*    if (searchText == "") {
            let toremovesong = document.querySelector("#bodymusic");
            toremovesong.parentNode.removeChild(toremovesong)
            let toremovesinger = document.querySelector("#bodysinger");
            toremovesinger.parentNode.removeChild(toremovesinger)
        }
        else {
            var html = '';
            html += '<div class="row" id="bodymusic"></div>';
            html += '<div class="row" id="bodysinger"></div>';
            $('#content').html(html);*/
    loadData(TuKhoa, Trang);
    /*        loadDataSinger(searchText);*/
    /*    }*/
})

//Load Data function  
function loadData(searchText, trang) {
    $.ajax({
        url: '/Song/LoadData',
        type: 'get',
        data: {
            tukhoa: searchText,
            trang: trang,
            layhet: "no"
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result.ds, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.id + '</td>';
                html += '<td>' + item.name + '</td>';
                html += '<td>' + item.tencasi + '</td>';
                html += '<td>' + item.tentacgia + '</td>';
                html += '<td>' + item.tentheloai + '</td>';
                html += '<td>' + item.tenalbum + '</td>';
                html += '<td><img  src="../' + item.image + '" class="img-small"/></td>';
                html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.id + ')">Sửa</a> <a href="#" class="btn btn-danger" onclick="Delele(' + item.id + ')">Xóa</a></td>';
                html += '</tr>';
            });
            MaxTrang = result.sotrang;
            $("#phantrang").empty();
            if (result.sotrang > 1) {
                let li1 = '<li class="page-item disabled" onclick="PagePrevios();"><a class="page-link">Previous</a></li>'
                $("#phantrang").append(li1);
            }          
            if (result.sotrang > 1) {
                for (i = 1; i <= result.sotrang; i++) {
                    let li;
                    if (Trang == i) {
                        li = '<li class="page-item disabled" style="cursor: default;" onclick="ChuyenTrang(' + i + ')" ><a class="page-link">' + i + '</a></li>'
                    }
                    else {
                        li = '<li class="page-item"  style="cursor: default;" onclick="ChuyenTrang(' + i + ')" ><a class="page-link">' + i + '</a></li>'
                    }
                    $("#phantrang").append(li);
                }
            }
            if (result.sotrang > 1) {
                let li2 = '<li class="page-item disabled" onclick="PageNext();" ><a class="page-link">Next</a></li>'
                $("#phantrang").append(li2);
            }

            $('#tbody').html(html);
            loadDataSinger();
            loadDataGenre();
            loadDataAlbum();
            loadDataComposer();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function ChuyenTrang(i) {
    Trang = i;
    loadData(TuKhoa, Trang);
}
function PageNext() {
    if (Trang <= MaxTrang - 1) {
        Trang = Trang + 1;
        loadData(TuKhoa, Trang)
    }
}
function PagePrevios() {
    if (Trang >= 2) {
        Trang = Trang - 1;
        loadData(TuKhoa, Trang)
    }
}
function loadDataSinger() {

    $.ajax({
        url: '/Singer/LoadData',
        type: 'get',
        data: {
            tukhoa: "",
            trang: 1,
            layhet: "yes"
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            if (idSinger != 0) {
                html += '<option value=' + idSinger + ' >' + nameSinger + '</option>';
            }
            $.each(result, function (key, item) {
                if (item.id != idSinger) {
                    html += '<option value=' + item.id + ' >' + item.name + '</option>';
                }
            });
            $('#select-casi').html(html);
            var select_box_element = document.querySelector('#select-casi');
            dselect(select_box_element, {
                search: true
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataComposer() {

    $.ajax({
        url: '/Composer/LoadData',
        type: 'get',
        data: {
            tukhoa: "",
            trang: 1,
            layhet: "yes"
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            
            if (idComposer != 0) {
                html += '<option value=' + idComposer + ' >' + nameComposer + '</option>';
            }
            $.each(result, function (key, item) {
                if (item.id != idComposer) {
                    html += '<option value=' + item.id + ' >' + item.name + '</option>';
                }
            });
            $('#select-tacgia').html(html);
            var select_box_element = document.querySelector('#select-tacgia');
            dselect(select_box_element, {
                search: true
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataGenre() {
    $.ajax({
        url: '/Genre/LoadData',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';

            if (idGenre != 0) {
                html += '<option value=' + idGenre + ' >' + nameGenre + '</option>';
            }
            $.each(result, function (key, item) {
                if (item.id != idGenre) {
                    html += '<option value=' + item.id + ' >' + item.name + '</option>';
                }              
            });
            $('#select-theloai').html(html);
            var select_box_element = document.querySelector('#select-theloai');
            dselect(select_box_element, {
                search: true
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loadDataAlbum() {
    $.ajax({
        url: '/Album/LoadData',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';           
            if (idAlbum == null) {
                idAlbum = 0;
            }
            if (idAlbum != 0) {
                html += '<option value=' + idAlbum + ' >' + nameAlbum + '</option>';
            }
            html += '<option value=0 >None</option>';
            $.each(result, function (key, item) {
                if (item.id != idAlbum) {
                    html += '<option value=' + item.id + ' >' + item.name + '</option>';
                }

            });
            $('#select-album').html(html);
            var select_box_element = document.querySelector('#select-album');
            dselect(select_box_element, {
                search: true
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
/*Add Data Function */
/*function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var singerObj = {
        name: $('#Name').val(),
        image: $('#Image'),
    };
    $.ajax({
        url: "/Singer/Add",
        type: "POST",
        data: JSON.stringify(singerObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.code = 200) {
                alert(result.msd)
                loadData();
                $('#myModal').modal('hide');
            }
            else if (result.code = 300) {
                alert(result.msd)
            }
            else {
                alert(result.msd)
            }

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}*/
function Add() {
    var res = validateAdd();
    if (res == false) {
        return false;
    }
    var name = $('#Name').val();
    var idcasi = $('#select-casi').val();
    var idtheloai = $('#select-theloai').val();
    var idtacgia = $('#select-tacgia').val();
    var idalbum = $('#select-album').val();
    var fileimage = $('#image').get(0).files;
    var filesong = $('#song').get(0).files;
    var data = new FormData;
    data.append("ImageFile", fileimage[0]);
    data.append("SongFile", filesong[0]);
    data.append("name", name)
    data.append("idcasi", idcasi)
    data.append("idalbum", idalbum)
    data.append("idtheloai", idtheloai)
    data.append("idtacgia", idtacgia)
    $.ajax({
        url: "/Song/Add",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            loadData(TuKhoa, Trang);
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Function for getting the Data Based upon Employee ID  
function getbyID(EmpID) {
    $('#Name').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Song/GetbyID/" + EmpID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ID').val(result.id);
            $('#Name').val(result.name);
            nameSinger = result.tencasi;
            idSinger = result.idcasi;
            idAlbum = result.idalbum;
            nameAlbum = result.tenalbum;
            idGenre = result.idtheloai;
            nameGenre = result.tentheloai;
            idComposer = result.idtacgia;
            nameComposer = result.tentacgia;
            console.log(idComposer + nameComposer)

            loadDataSinger();
            loadDataGenre();
            loadDataAlbum();
            loadDataComposer();
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
            $('#myModalLabelupdate').show();
            $('#myModalLabeladd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record  
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var idtacgia = $('#select-tacgia').val();
    var id = $('#ID').val();
    var name = $('#Name').val();
    var idcasi = $('#select-casi').val();
    var idtheloai = $('#select-theloai').val();
    var idalbum = $('#select-album').val();
    var fileimage = $('#image').get(0).files;
    var filesong = $('#song').get(0).files;
    var data = new FormData;
    data.append("ImageFile", fileimage[0]);
    data.append("SongFile", filesong[0]);
    data.append("name", name)
    data.append("idcasi", idcasi)
    data.append("idalbum", idalbum)
    data.append("idtheloai", idtheloai)
    data.append("idtacgia", idtacgia)
    data.append("id", id);
    $.ajax({
        url: "/Song/Update",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            loadData(TuKhoa, Trang);
            $('#myModal').modal('hide');
            $('#ID').val("");
            $('#Name').val("");
            $('#image').val("");
            $('#song').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record  
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: " /Song/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData(TuKhoa, Trang);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    $('#ID').val("");
    $('#Name').val("");
    $('#image').val("");
    $('#btnUpdate').hide();
    $('#song').val("");
    $('#btnAdd').show();
    $('#myModalLabelupdate').hide();
    $('#myModalLabeladd').show();
    $('#Name').css('border-color', 'lightgrey');
    $('#song').css('border-color', 'lightgrey');
}
//Valdidation using jquery  
function validateAdd() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }
    if ($('#song').val().trim() == "") {
        $('#song').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#song').css('border-color', 'lightgrey');
    }
    return isValid;
}
function validate() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }
    return isValid;
}
function logout() {
    var ans1 = confirm("Bạn có chắc muốn đăng xuất?");
    if (ans1) {
        window.location = "/Logout/Index"
    }
}