function validateEdelweissInput() {
    var returnVal;
    if (document.getElementById("txtfrdt").value == '') {
        alert('Please Enter From date.');
        document.getElementById("txtfrdt").focus();
        returnVal = false;
        return returnVal;
    }

    if (document.getElementById("txttodt").value == '') {
        alert('Please Enter To date.');
        document.getElementById("txttodt").focus();
        returnVal = false;
        return returnVal;
    }

    if (document.getElementById("txtcurdt").value == '') {
        alert('Current Date Can Not Be Blank.');
        document.getElementById("txtcurdt").focus();
        returnVal = false;
        return returnVal;
    }

    if (document.getElementById("txtSwitchAmt") != null) {
        if (document.getElementById("txtSwitchAmt").value > 199000) {
            alert('Switch Amount Should Not Be Greater Than Rs. 1,99,000.');
            document.getElementById("txtInitialInvestment").focus();
            returnVal = false;
            return returnVal;
        }
    }

    if (document.getElementById("txtInitialInvestment") != null) {
        if (document.getElementById("txtInitialInvestment").value == '') {
            alert('Please Provide Initial Investment.');
            document.getElementById("txtInitialInvestment").focus();
            returnVal = false;
            return returnVal;
        }
    }

    returnVal = ChkInvesmentAmt();
    if (!returnVal)
        return returnVal;

    if (document.getElementById("ddlSIPType").value == 'Prepaid Plus') {
        var values = [];
        $("input[name='txtDynSwitchAmt']").each(function () {
            if ($(this).val() == '') {
                alert('Switch Amount Can Not Be Blank');
                returnVal = false;
                return returnVal;
            }
            if ($(this).val() > 199000) {
                alert('Switch Amount Can Not Be Greater Than 199000');
                returnVal = false;
                return returnVal;
            }
        });

    }


    //    if (document.getElementById("ddlSIPType").value == 'Prepaid Plus') {
    //        if (document.getElementsByName("txtDynSwitchAmt") != null) {
    //            if (document.getElementsByName("txtDynSwitchAmt").value == '') {
    //                alert('Please Provide Switch Amount.');
    //                document.getElementsByName("txtDynSwitchAmt").focus();
    //                return false;
    //            }
    //        }
    //        if (document.getElementsByName("txtDynSwitchAmt") != null) {
    //            if (document.getElementsByName("txtDynSwitchAmt").value != '') {
    //                
    //                var switchAmt = document.getElementsByName("txtDynSwitchAmt")[0].value;
    //                if (switchAmt > 199000) {
    //                    alert($(this).index());
    //                    document.getElementsByName("txtDynSwitchAmt")[0].focus();
    //                    return false;
    //                }
    //            }
    //        }
    //    }

    if (document.getElementById("txtSwitchAmt") != null) {
        if (document.getElementById("txtSwitchAmt").value != '') {
            var switchAmt = document.getElementById("txtSwitchAmt").value;
            if (switchAmt % 1000 != 0) {
                alert('Please Provide Switch Amount in multiple of 1000 (s)');
                document.getElementById("txtSwitchAmt").focus();
                returnVal = false;
                return returnVal;
            }
        }
    }
    return returnVal;
}


function ChkInvesmentAmt() {
    var flag = true;
    var SIPType = $("#ddlSIPType").val();
    if (SIPType == "Power STP") {
        if (parseFloat($("#txtInitialInvestment").val()) < 25000 || parseFloat($("#txtInitialInvestment").val()) > 1800000) {
            alert("Amount Invested should between 25000 and 18 lacs .");
            $("#rvInitialInvestment").html("Amount Invested should between 25000 and 18 lacs .");
            flag = false;
        }
    }
    if (SIPType == "Gain Switch Mechanism") {
        if (parseFloat($("#txtInitialInvestment").val()) < 25000 || parseFloat($("#txtInitialInvestment").val()) > 1000000000) {
            alert("Amount Invested should between 25000 or above .");
            $("#rvInitialInvestment").html("Amount Invested should between 25000 or above .");
            flag = false;
        }
    }
    if (SIPType == "Power SIP") {
        if (parseFloat($("#txtInitialInvestment").val()) < 25000 || parseFloat($("#txtInitialInvestment").val()) > 1800000) {
            alert("Amount Invested shpold between 25000 and 18 lacs .");
            $("#rvInitialInvestment").html("* Amount Invested should between 25000 and 18 lacs .");
            flag = false;
        }
    }
    return flag;
}

function setFocus() {

    document.getElementById("txtfrdt").focus();
}

function ValidateFromDate() {
    var ddScheme = document.getElementById("ddlFromScheme");

    var iniStartdata = document.getElementById("txtfrdt").value;
    if (document.getElementById("txtfrdt").value == '') {
        return false;
    }
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    //alert(allotdate);
    if (!IsValidDate(allotdate, iniStartdata)) {
        alert("From Date should be greater than or Equal to Inception Date " + allotdate);
        document.getElementById('txtfrdt').focus();
        return false;
    }
}


function IsValidDate(str1, str2) {
    var Day = parseInt(str1.substring(0, 2), 10);
    var Mn = parseInt(str1.substring(3, 5), 10);
    var Yr = parseInt(str1.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);

    var Day1 = parseInt(str2.substring(0, 2), 10);
    var Mn1 = parseInt(str2.substring(3, 5), 10);
    var Yr1 = parseInt(str2.substring(6, 10), 10);
    var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;
    var dt1 = new Date(DateVal1);

    if (dt.getDate() != Day) {
        alert('Invalid Date');
        return false;
    }
    else if (dt.getMonth() != Mn - 1) {
        //this is for the purpose JavaScript starts the month from 0
        alert('Invalid Date');
        return false;
    }
    else if (dt.getFullYear() != Yr) {
        alert('Invalid Date');
        return false;
    }


    if (dt1.getDate() != Day1) {
        alert('Invalid Date');
        return false;
    }
    else if (dt1.getMonth() != Mn1 - 1) {
        //this is for the purpose JavaScript starts the month from 0
        alert('Invalid Date');
        return false;
    }
    else if (dt1.getFullYear() != Yr1) {
        alert('Invalid Date');
        return false;
    }
    // alert(dt); alert(dt1);
    if (dt > dt1) {
        return false;
    }
    return true;
}

function isNumber(key) {
    var keycode = (key.which) ? key.which : key.keyCode;
    if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
        return true;
    }
    return false;
}

    

