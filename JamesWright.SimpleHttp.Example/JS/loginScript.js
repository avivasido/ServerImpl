function submit_login() {
    var data = JSON.stringify({
        "mail": document.getElementById("uname").value,
        "password": document.getElementById("psw").value
    });
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "http://localhost:8005/login",
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
            //window.alert(response);
            //localStorage.setItem("tempPassword", response);
            //localStorage.setItem("mail", document.getElementById("uname").value);
            window.location.href = "http://localhost:8005/main";
        }
        else {
            var message = jqXHR.getResponseHeader("error_message");
            window.alert(message);
        }
        
    });
}

function forgot() {
    var data = JSON.stringify({
        "mail": document.getElementById("uname").value
    });
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "http://localhost:8005/forgotpass",
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
            window.location.href = "http://localhost:8005/main";
        }
        else {
            var message = jqXHR.getResponseHeader("error_message");
            window.alert(message);
        }

    });
}

