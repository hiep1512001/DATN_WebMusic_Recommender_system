//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();
});
var idPlaylist = 0;

//Load Data function  
function loadData() {
    $.ajax({
        url: '/Playlist/LoadData',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<div class="col-2">';
                html += '<div class="card" style="background-color:#e7c0c0; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.1);">';
                html += '<a class="align-self-center" href="/Playlist/GetIdPlaylist?id=' +item.id+ '">'
                html += '<img src="/Image/playlist.jpg" class="align-self-center thumb" style="padding-top:10px">';
                html += '</a>'
                html += '<div class="container-fluid">'
                html += '<div class="row">'
                html += '<h5 class="card-text text-center p-card" style="width:100%;"><i onclick="return getbyID(' + item.id + ')" class="fa-sharp fa-solid fa-pen" style="cursor:pointer;"></i> ' + item.name + '</h5>';
                html += '</div>';
                html += '<div class="row">'
                html += '<p class="card-text text-center p-card" style="width:100%;">Số bài hát: ' + item.sumsong + '</p>';
                html += '</div>';
                html += '</div>';
                html += '<a href="#" onclick="Delele(' + item.id + ')" class="btn btn-danger"><i class="fa-sharp fa-solid fa-trash"></i> Xóa</a>';
                html += '</div>';
                html += '</div>';
            });
            $('#body').html(html);
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
    var res = validate();
    if (res == false) {
        return false;
    }
    var name = $('#Name').val();
    var data = new FormData;
    data.append("name", name)
    $.ajax({
        url: "/Playlist/Add",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            loadData();
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
        url: "/Playlist/GetbyID/" + EmpID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Name').val(result.name);
            idPlaylist = result.id;
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
    var id = idPlaylist;
    var name = $('#Name').val();
    var data = new FormData;
    data.append("name", name);
    data.append("id", id);
    $.ajax({
        url: "/Playlist/Update",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#ID').val("");
            $('#Name').val("");
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
            url: " /Playlist/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    $('#Name').val("");
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