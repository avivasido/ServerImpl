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
            localStorage.setItem("tempPassword", response);
            localStorage.setItem("mail", document.getElementById("uname").value);
            window.location.href = "http://localhost:8005/main";
        }
        else {
            var message = jqXHR.getResponseHeader("error_message");
            window.alert(message);
        }
        
    });
}

function forgot() {
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
        "data": {
            "mail": document.getElementById("uname").value
        }
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

function button2changed() {
    document.getElementById("diagnosis_div").style.display = 'block';
}

function button1changed() {
    document.getElementById("diagnosis_div").style.display = 'none';
}

function submit_ans() {
    if (document.getElementsByName("norm")[1].checked && $('#diagnosis').tokenize().toArray().length === 0) {
        window.alert("error, if abnormal you must enter diagnosis");
    }
else
{

       // window.alert(document.getElementsByName("norm")[0].value.toString());
       // window.alert($('#diagnosis').tokenize().toArray().length);

    // var diag = $('#diagnosis').tokenize().toArray();
    document.cookie = "tempPassword=" + localStorage.tempPassword;
    document.cookie = "mail=" + localStorage.mail;
    var norms = document.getElementsByName("norm");

    for (var i = 0; i < norms.length; i++) {
        if (norms[i].checked == true) {
            selectedNorm = norms[i].value;
        }
    }
    var data = JSON.stringify({
        "normalOrNot": selectedNorm.toString(),
        "sure1": document.getElementById("sure1").value.toString(),
        "diagnosis": $('#diagnosis').tokenize().toArray(),
        "sure2": document.getElementById("sure2").value.toString()
    });
    
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "http://localhost:8005/ans",
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
        //window.alert(document.getElementById("diagnosis").value);
        if (myHeader.valueOf() === new String('success').valueOf()) {
            window.alert("success!!!!!");
            window.location.href = "http://localhost:8005/main";
        }
        else {
            var message = jqXHR.getResponseHeader("error_message");
            window.alert(message);
            //window.alert(sessionStorage.getItem("tempPass"));
        }

    });
}
}