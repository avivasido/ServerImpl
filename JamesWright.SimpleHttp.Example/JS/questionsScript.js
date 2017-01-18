function button2changed() {
    document.getElementById("diagnosis_div").style.display = 'block';
}

function button1changed() {
    document.getElementById("diagnosis_div").style.display = 'none';
}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

function submit_ans() {
    if (document.getElementsByName("norm")[1].checked && $('#diagnosis').tokenize().toArray().length === 0) {
        window.alert("error, if abnormal you must enter diagnosis");
    }
    else {
        var norms = document.getElementsByName("norm");

        for (var i = 0; i < norms.length; i++) {
            if (norms[i].checked == true) {
                selectedNorm = norms[i].value;
            }
        }

        var arr = [];
        var diagnosis = $('#diagnosis').tokenize().toArray();
        for (var i = 0, len = diagnosis.length; i < len; i++) {
            //var temp = document.getElementById(replaceAll(diagnosis[i], " ", "")).toString();
           // window.alert(temp);
            arr.push(document.getElementById("sure_"+replaceAll(diagnosis[i], " ", "")).value);
        }


        var data = JSON.stringify({
            "normalOrNot": selectedNorm.toString(),
            "sure1": document.getElementById("sure1").value.toString(),
            "diagnosis": diagnosis,
            //"sure2": document.getElementById("sure2").value.toString()
            "sure2": arr
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

function outputUpdateForSure1(val) {
    document.querySelector("#slider1").value = val;
}

function outputUpdate(slider, val) {
    var tmp = "#slider_"+slider.toString();
    document.querySelector(tmp).value = val;
}



