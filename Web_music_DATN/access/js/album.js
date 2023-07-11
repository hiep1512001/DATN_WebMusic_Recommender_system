//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();
    loadDataSinger();
});
var nameSinger = "";
var idSinger = 0;
//Load Data function  
function loadData() {
    $.ajax({
        url: '/Album/LoadData',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result);
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.id + '</td>';
                html += '<td>' + item.name + '</td>';
                html += '<td>' + item.tencasi + '</td>';
                html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.id + ')">Sửa</a> <a href="#" class="btn btn-danger" onclick="Delele(' + item.id + ')">Xóa</a> <a href="/Album/GetSong?id=' + item.id + '" class="btn btn-success" >Chi tiết</a></td>';
                html += '</tr>';
            });
            $('#tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
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
    var idcasi = $('#select-casi').val();
    var data = new FormData;
    data.append("name", name)
    data.append("idcasi", idcasi)
    $.ajax({
        url: "/Album/Add",
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
        url: "/Album/GetbyID/" + EmpID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ID').val(result.id);
            $('#Name').val(result.name);
            /*            $('#select-casi').val(result.idcasi);*/
            idSinger = result.idcasi;
            nameSinger = result.tencasi;

            loadDataSinger();
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
    var idcasi = $('#select-casi').val();
    var data = new FormData;
    data.append("name", name);
    data.append("id", id);
    data.append("idcasi",idcasi)
    $.ajax({
        url: "/Album/Update",
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
            url: " /Album/Delete/" + ID,
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