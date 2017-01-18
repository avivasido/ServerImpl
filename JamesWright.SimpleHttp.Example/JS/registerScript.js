function submit_register() {
    var data = JSON.stringify({
        "mail": document.getElementById("uname").value,
        "password": document.getElementById("psw").value,
        "fname": document.getElementById("fname").value,
        "lname": document.getElementById("lname").value,
        "medTraining": document.getElementById("level").value
    });
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "http://localhost:8005/register",
        "method": "POST",
        "xhrFields": {
            "withCredentials": true
        },
        "headers": {
            "content-type": "application/x-www-form-urlencoded",
            "cache-control": "no-cache",
            "postman-token": "87dfacbe-364d-57df-9f22-9bd4752d7b0c"
        },
        "data": data
    }


    $.ajax(settings).done(function (response, status, jqXHR) {
        console.log(response);
        var myHeader = jqXHR.getResponseHeader("answer_status");
        if (myHeader.valueOf() === new String('success').valueOf()) {
            //window.alert( getCookie("test_cookie"));
            window.alert("registration successfull");
            window.location.href = "http://localhost:8005/home";
        }
        else {
            var message = jqXHR.getResponseHeader("error_message");
            window.alert(message);
        }

    });
}

function getCookie(cookieName) {
    var cookieArray = document.cookie.split(';');
    for (var i = 0; i < cookieArray.length; i++) {
        var cookie = cookieArray[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        cookieHalves = cookie.split('=');
        if (cookieHalves[0] == cookieName) {
            return cookieHalves[1];
        }
    }
    return "";
}