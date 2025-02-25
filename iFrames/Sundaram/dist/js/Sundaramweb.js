$.ajaxSetup({ cache: false });

function CreateXmlHttpRequest() {
  var XmlHttp = null;
  try {
    XmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
  } catch (e) {
    try {
      XmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
    } catch (oc) {
      XmlHttp = null;
    }
  }
  if (!XmlHttp && typeof XMLHttpRequest != "undefined") {
    XmlHttp = new XMLHttpRequest();
  }
  return XmlHttp;
}

//function allLetter(txtCont) {
//  var letters = /^[a-zA-Z \s\.\-]+$/;
//  if (txtCont.match(letters)) {
//    return true;
//  } else {
//    return false;
//  }
//}

function allLetter(txtCont) {
    var letters = /^[a-zA-Z \s\.\-]+$/;
    if (typeof txtCont === "string" && txtCont.match(letters)) {
        return true;
    } else {
        return false;
    }
}
function checkEmail(Email) {
  var re =
    /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  if (Email.match(re)) {
    return true;
  } else {
    return false;
  }
}

function allnumeric(txtnum) {
  //var numbers = /^[0-9 \s\-]{10,14}$/;
  var numbers = /^[0-9]+$/;
  if (txtnum.match(numbers)) {
    return true;
  } else {
    return false;
  }
}

function alphanumeric(txtnum) {
  var letters = /^[a-zA-Z0-9 \s\.\-]+$/;
  if (txtnum.match(letters)) {
    return true;
  } else {
    return false;
  }
}

function find_in_object(my_object, my_criteria) {
  return my_object.filter(function(obj) {
    return Object.keys(my_criteria).every(function(c) {
      return obj[c] == my_criteria[c];
    });
  });
}

function find_in_object_MultiParam(my_object, my_criteria) {
  // my_criteria = { my_criteria };
  return my_object.filter(function(o) {
    return Object.keys(my_criteria).every(function(k) {
      //  alert(my_criteria[k]);
      return my_criteria[k].some(function(f) {
        return o[k] === f;
      });
    });
  });
}

//function find_in_object_M(my_array, my_criteria) {

//    return my_array.filter(function (obj) {
//        return Object.keys(my_criteria).every(function (key) {
//            alert(my_criteria[key]);
//            return ((my_criteria[key].some(function (criteria) {
//                    return (typeof obj[key] === 'string')
//                })) || my_criteria[key].length === 0);
//        });
//    });
//}

function showValidating(bFlag) {
  showAjaxImage("ajax_validating.gif", bFlag);
}

function showLoading(bFlag) {
  showAjaxImage("loading.gif", bFlag);
}

function showWait(bFlag) {
  showAjaxImage("loader1.gif", bFlag);
}

function showAjaxImage(ImageName, bFlag) {
  if ($("#AjaxContainer").length > 0 && bFlag) {
    $("#AjaxContainer").append(
      "<img src='../images/" +
      ImageName +
      "' class='loading_img' class='loading_img' style='display: block;position: fixed;z-index: 9999;background-repeat : no-repeat;background-position : center;top:45%; left:50%;width:8%'>"
    );
  } else {
    $("#AjaxContainer .loading_img").remove();
  }
}

//function showValidating(bFlag) { showAjaxImage("ajax_validating.gif", bFlag); }
//function showLoading(bFlag) { showAjaxImage("loading.gif", bFlag); }
//function showWait(bFlag) { showAjaxImage("heartbeat.gif", bFlag); }

//function showAjaxImage(ImageName, bFlag) {
//    if ($("#AjaxContainer").length > 0 && bFlag) {

//        $("#AjaxContainer").append("<div class='loading'><img src='../images/" + ImageName + "' class='loading-image'   ></div>");
//    }
//    else { $('#AjaxContainer .loading').remove(); }

//}

function showWaitInner(bFlag) {
  showAjaxImageInner("loader4.gif", bFlag);
}

function showAjaxImageInner(ImageName, bFlag) {
  if ($("#ContainerInner").length > 0 && bFlag) {
    $("#ContainerInner").append(
      "<img src='../images/" +
      ImageName +
      "' class='loading_img' class='loading_img' style='display: block;position:absolute;background-repeat : no-repeat;background-position : center;top:50%; left:50%;' '>"
    );
  } else {
    $("#ContainerInner .loading_img").remove();
  }
}




function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    } else
        return true;
}

function isJson(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}

function PANValidation(PAN) {
    var regExp = /[a-zA-z]{5}\d{4}[a-zA-Z]{1}/;

    if (PAN.length == 10) {
        if (!PAN.match(regExp)) {
            return false;
        } else {
            return true;
        }
    } else {
        return false;
    }
}

function checkemailVal(Email) {
    if (Email != "") {
        var emailReg = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
        var valid = emailReg.test(Email);
        if (!valid) {
            return false;
        } else {
            return true;
        }

    }
}

function IsMobileNumber(txtMob) {
    var mob = /^[1-9]{1}[0-9]{9}$/;
    // var txtMobile = document.getElementById(txtMobId);
    if (mob.test(txtMob) == false) {
        return false;
    }
    return true;
}


function FormatPhonewithstar(number) {
    var phone = number.slice(number.length - 10);
    var m = phone.match(/(\d+)(\d{4})/);
    var res = '*'.repeat(m[1].length) + m[2];
    return res;
}

function FormatEmailwithstar(Emailid) {
    var Res = "";
    if (checkemailVal(Emailid)) {
        var sli = Emailid.split('@');
        //var emails = sli[0].match(/(\w{3})(\w+)(\w{0})/);

        var eml = sli[0].replace(".", "_");
        var emails = eml.match(/(\w{3})(\w+)(\w{0})/);  
        Res = emails[1] + '*'.repeat(emails[2].length) + emails[3] + "@" + sli[1];
    } else {
        Res = "Invalid Email Id";
    }


    return Res;
}

function preventBack() { window.history.forward(); }

function myFunction() {
    if ((navigator.userAgent.indexOf("Opera") || navigator.userAgent.indexOf('OPR')) != -1) {
        //  alert('Opera');
    } else if (navigator.userAgent.indexOf("Chrome") != -1) {

        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    } else if (navigator.userAgent.indexOf("Safari") != -1) {
        //  alert('Safari');
    } else if ((navigator.userAgent.indexOf("Firefox") != -1) || (navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) //IF IE > 10
    {

        window.history.pushState(null, "", window.location.href);
        window.onpopstate = function () {
            window.history.pushState(null, "", window.location.href);
        };
    } else {
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    }
}



function BlockHistory() {
    if ((navigator.userAgent.indexOf("Opera") || navigator.userAgent.indexOf('OPR')) != -1) {
        //  alert('Opera');
    } else if (navigator.userAgent.indexOf("Chrome") != -1) {

        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    } else if (navigator.userAgent.indexOf("Safari") != -1) {
        //  alert('Safari');
    } else if ((navigator.userAgent.indexOf("Firefox") != -1) || (navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) //IF IE > 10
    {

        window.history.pushState(null, "", window.location.href);
        window.onpopstate = function () {
            window.history.pushState(null, "", window.location.href);
        };
    } else {
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    }
}


