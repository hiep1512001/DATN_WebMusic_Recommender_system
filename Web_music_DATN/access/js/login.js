
$(document).ready(function () {
    loadDataGenre();
});
var i = 0;
function login() {
    $('#successnotify').css('display', 'none')
    $('#notify').css('display', 'none')
    var res = validate();
    if (res == false) {
        return false;
    }
    var name = $('#username').val();
    var password = $('#password').val();
    var data = new FormData;
    data.append("username", name)
    data.append("password", password)
    $.ajax({
        url: "/Login/Kt",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.msd != "No") {
                if (result.msd == "user") {
                    window.location = "/HomeUser/Index"
                }
                else {
                    window.location = "/Song/Index"
                }
            }
            else {
                $('#notify').css('display', 'block')
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function validate() {
    var isValid = true;
    if ($('#username').val().trim() == "") {
        $('#notifyusername').css('display', 'block')
        isValid = false;
    }
    else {
        $('#notifyusername').css('display', 'none')
    }
    if ($('#password').val().trim() == "") {
        $('#notifypassword').css('display', 'block')
        isValid = false;
    }
    else {
        $('#notifypassword').css('display', 'none')
    }
    return isValid;
}
function loadDataGenre() {
    $.ajax({
        url: '/Genre/LoadData',
        type: 'get',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                    html += '<option value=' + item.id + ' >' + item.name + '</option>';
            });
            $('#select-theloai').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function Register() {
    $('#rnotify').css('display', 'none')
    var rres = rvalidate();
    if (rres == false) {
        return false;
    }
    var name = $('#rusername').val();
    var password = $('#rpassword').val();
    var idtheloai = $('#select-theloai').val();
    var data = new FormData;
    data.append("idtheloai", idtheloai)
    data.append("username", name)
    data.append("password", password)
    $.ajax({
        url: "/Login/AddUser",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.msd == "No") {
                $('#rnotify').css('display', 'block')
            }
            else {
                window.location="/HomeUser/Index"              
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function rvalidate() {
    var risValid = true;
    if ($('#rusername').val().trim() == "") {
        $('#rnotifyusername').css('display', 'block')
        risValid = false;
    }
    else {
        $('#rnotifyusername').css('display', 'none')
    }
    if ($('#rpassword').val().trim() == "") {
        $('#rnotifypassword1').css('display', 'block')
        risValid = false;
    }
    else {
        $('#rnotifypassword1').css('display', 'none')
        if ($('#rpassword').val().toString().trim().length < 6) {
            $('#rnotifypassword2').css('display', 'block')
            risValid = false;
        }
        else {
            $('#rnotifypassword2').css('display', 'none')
            if ($('#rpassword2').val().toString().trim() != $('#rpassword').val().toString().trim()) {
                $('#rnotifypassword3').css('display', 'block')
                risValid = false;
            }
            else {
                $('#rnotifypassword3').css('display', 'none')
            }
        }
    }
    return risValid;
}
function LoginFacebook() {
    window.location = "/Facebook/Index"
}