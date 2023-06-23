//Load Data in Table when documents is ready

$(document).ready(function () {
    loadData(TuKhoa, Trang);

});
var MaxTrang = 1;
var Trang = 1;
var TuKhoa = "";

$('#search').keyup(function () {
    TuKhoa = $('#search').val().toLowerCase();
    Trang = 1;
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

    loadData(TuKhoa, 1);
/*        loadDataSinger(searchText);*/
/*    }*/
})
//Load Data function  
function loadData(searchText, trang) {
    $.ajax({
        url: '/Singer/LoadData',
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
                html += '<td><img  src="../'+ item.image+'" class="img-small"/></td>';
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
    if (Trang <= MaxTrang-1) {
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
    var res = validate();
    if (res == false) {
        return false;
    }
    var name = $('#Name').val();
    var file = $('#image').get(0).files;
    var data = new FormData;
    data.append("ImageFile", file[0]);
    data.append("name",name)
    $.ajax({
        url: "/Singer/Add",
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
        url: "/Singer/GetbyID/" + EmpID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ID').val(result.id);
            $('#Name').val(result.name);

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
    var id = $('#ID').val();
    var name = $('#Name').val();
    var file = $('#image').get(0).files;
    var data = new FormData;
    data.append("ImageFile", file[0]);
    data.append("name", name);
    data.append("id", id);
    $.ajax({
        url: "/Singer/Update",
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
            url: " /Singer/Delete/" + ID,
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
    $('#btnAdd').show();
    $('#myModalLabelupdate').hide();
    $('#myModalLabeladd').show();
    $('#Name').css('border-color', 'lightgrey');
}
//Valdidation using jquery  
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