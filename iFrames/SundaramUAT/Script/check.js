
var DateDiff = {

    inDays: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000));
    },

    inWeeks: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
    },

    inMonths: function (d1, d2) {
        var d1Y = d1.getFullYear();
        var d2Y = d2.getFullYear();
        var d1M = d1.getMonth();
        var d2M = d2.getMonth();
        return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
    },

    inYears: function (d1, d2) {
        return d2.getFullYear() - d1.getFullYear();
    }
}



function mouseEnter2(controlName, staticLength) {
    //alert(staticLength);
    var maxlength = 0;
    var mySelect = document.getElementById(controlName);
    maxlength = mySelect.value.length;
    maxlength = maxlength * 6;
    if (maxlength < 160)
        maxlength = 160;
    mySelect.style.width = maxlength;
}

function mouseEnter(controlName, staticLength) {
    //alert(staticLength);
    var maxlength = 0;
    var mySelect = document.getElementById(controlName);
    for (var i = 0; i < mySelect.options.length; i++) {
        if (mySelect[i].text.length > maxlength) {
            maxlength = mySelect[i].text.length;
        }
    }

    var ddlwidth = maxlength * 6;
    if (ddlwidth < 160)
        ddlwidth = 160;
    // alert(ddlwidth);

    if (maxlength != 0)
        mySelect.style.width = ddlwidth;

}
function focusOut(controlName, staticLength) {
    // alert(staticLength);
    var mySelect = document.getElementById(controlName);
    if (staticLength < 160)
        staticLength = 160;
    //alert(mySelect.style.width);
    mySelect.style.width = staticLength;
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


function IsValidDateEqual(str1, str2) {
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

function setDate() {
    // alert(1);
    var ddScheme = document.getElementById("ddlscheme");
    // alert(ddScheme.options[ddScheme.selectedIndex].text);
    // alert(ddScheme.options[ddScheme.selectedIndex].value);
    // alert(ddScheme.options[ddScheme.selectedIndex].title);
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    // alert(id);
    //alert(allotdate);



    var Day = parseInt(allotdate.substring(0, 2), 10);
    var Mn = parseInt(allotdate.substring(3, 5), 10);
    var Yr = parseInt(allotdate.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);


    if (document.getElementById("ddlMode").value == 'STP') {
        var ddSchemeto = document.getElementById("ddlschtrto");
        var allotdateto = ddSchemeto.options[ddSchemeto.selectedIndex].title;

        if (allotdateto != null) {
            var Day2 = parseInt(allotdateto.substring(0, 2), 10);
            var Mn2 = parseInt(allotdateto.substring(3, 5), 10);
            var Yr2 = parseInt(allotdateto.substring(6, 10), 10);
            var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;
            var dtTo = new Date(DateVal2);

            var i = DateDiff.inDays(dt, dtTo);
            if (i >= 0) {
                dt = dtTo;
            }
        } else {

            alert("Inception date not found");
        }

    }

    if (document.getElementById("ddlscheme").value != '0') {
        $("#txtfromDate").datepicker("option", "minDate", dt);
        $("#txtIniToDate").datepicker("option", "minDate", dt);
        $("#txtLumpfromDate").datepicker("option", "minDate", dt);
    }


    return true;
}




function setIncDate() {

    // $("#txtLumpfromDate").val($("#SIPSchDt").val());
    //$("input[name='RadioButtonListCustomerType']:checked")

    if ($('#chkInception').is(':checked')) {
        $("#txtLumpfromDate").val($("#SIPSchDt").val());
       // $("#txtLumpfromDate").attr("readonly", false);
       // $('#txtLumpfromDate').attr("disabled", true);
    } else {
        $("#txtLumpfromDate").attr("readonly", true);
    }

    if ($('#chkInception4sip').is(':checked')) {
        $("#txtfromDate").val($("#SIPSchDt").val());
    } else {
        $("#txtfromDate").attr("readonly", true);
    }
}






function setDateValueAsOn() {
    if (document.getElementById("txtToDate").value != '') {
        var Valuedate = document.getElementById("txtToDate").value;
        if (Valuedate != null) {
            var Day = parseInt(Valuedate.substring(0, 2), 10);
            var Mn = parseInt(Valuedate.substring(3, 5), 10);
            var Yr = parseInt(Valuedate.substring(6, 10), 10);
            Day = Day + 1;
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dtTo = new Date(DateVal);
            $("#txtvalason").datepicker("option", "minDate", dtTo);
            //alert(5);
            checkDateRange();
        }

    }

    return true;

}


function checkInvestedValue() {
    var mode = document.getElementById('ddlMode').value;
    if (mode == 'Lump Sum') {
        // alert(1);
        var sipamunt = document.getElementById('txtinstallLs').value;
        if (isNaN(sipamunt)) {
            alert("Please enter Numeric value.");
            document.getElementById('txtinstallLs').value = "";
            document.getElementById('txtinstallLs').focus();
            return false;
        }
        else {
            if (sipamunt < 5000) {
                alert('Minimum investment amount should be Rs. 5000/- ');
                document.getElementById('txtinstallLs').value = "";
                document.getElementById("txtinstallLs").focus();
                return false;
            }
            //            else {
            //                if (sipamunt >= 500) {
            //                    if (sipamunt % 100 != 0) {
            //                        alert('Minimum amount should be Rs.500/- and in multiples of Rs.100/- thereafter');
            //                        document.getElementById('txtinstallLs').value = "";
            //                        document.getElementById("txtinstallLs").focus();
            //                        return false;
            //                    }
            //                }
            //            }
        }

    }
    else if (mode == 'SWP' || mode == 'STP') {
        var text;
        if (mode == 'SWP') text = 'withdrawal';
        else text = 'transfer';

        var withdrwAmt = document.getElementById("txtTransferWithdrawal2").value;
        if (isNaN(withdrwAmt)) {
            alert("Please enter Numeric value.");
            document.getElementById('txtTransferWithdrawal2').value = "";
            document.getElementById('txtTransferWithdrawal2').focus();
            return false;
        }
        else {
            if (parseFloat(document.getElementById("txtTransferWithdrawal2").value) < 500) {
                alert('Minimum ' + text + ' amount should be Rs.500/-');
                document.getElementById('txtTransferWithdrawal2').value = "";
                document.getElementById("txtTransferWithdrawal2").focus();
                return false;
            }
            else {
                //if (parseFloat(document.getElementById("txtTransferWithdrawal").value) >= 500) {
                var amt;
                amt = document.getElementById("txtTransferWithdrawal2").value;
                if (amt % 100 != 0) {
                    alert('Minimum ' + text + ' amount should be Rs.500/- and in multiples of Rs.100/- thereafter');
                    document.getElementById('txtTransferWithdrawal2').value = "";
                    document.getElementById("txtTransferWithdrawal2").focus();
                    return false;
                }
            }
        }
    }
    else {
        var sipamunt = document.getElementById('txtinstall').value;
        if (isNaN(sipamunt)) {
            alert("Please enter Numeric value.");
            document.getElementById('txtinstall').value = "";
            document.getElementById('txtinstall').focus();
            return false;
        }
        else {
            if (sipamunt < 500) {
                alert('Minimum installment amount should be Rs. 500/- ');
                document.getElementById('txtinstall').value = "";
                document.getElementById("txtinstall").focus();
                return false;
            }
            else {
                if (sipamunt >= 500) {
                    if (sipamunt % 100 != 0) {
                        alert('Minimum installment amount should be Rs.500/- and in multiples of Rs.100/- thereafter');
                        document.getElementById('txtinstall').value = "";
                        document.getElementById("txtinstall").focus();
                        return false;
                    }
                }
            }
        }

    }

    return true;
}




function checkDateRange() {
    ///// date part /////
    var sipStartdata = document.getElementById('txtfromDate').value;
    var sipEnddata = document.getElementById('txtToDate').value;
    // var sipAsondata = document.getElementById('txtvalason').value;

    //alert(sipStartdata);
    var Day = parseInt(sipStartdata.substring(0, 2), 10);
    var Mn = parseInt(sipStartdata.substring(3, 5), 10);
    var Yr = parseInt(sipStartdata.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);


    //alert(sipStartdata);
    Day = parseInt(sipEnddata.substring(0, 2), 10);
    Mn = parseInt(sipEnddata.substring(3, 5), 10);
    Yr = parseInt(sipEnddata.substring(6, 10), 10);
    var DateVal2 = Mn + "/" + Day + "/" + Yr;
    var dt2 = new Date(DateVal2);

    var sipDate = document.getElementById("ddSIPdate").value;
    var mode = document.getElementById('ddlMode').value;
    // new calculation

    //

    var k = DateDiff.inMonths(dt, dt2);
    var dtdf = DateDiff.inDays(dt, dt2);

    var period = document.getElementById("ddPeriod_SIP").value;
    var ind = document.getElementById('ddlscheme').selectedIndex;


    //alert(mode);

    if (ind > 0) {
        var schemeid = document.getElementById('ddlscheme').options[ind].value;
        var addMonth = 0; addMonth = 1;

        var newLimitDate = dt;
        newLimitDate.setMonth(newLimitDate.getMonth() + addMonth);
        var day = newLimitDate.getDate();
        if (day <= sipDate)
            newLimitDate.setDate(sipDate);
        else
            newLimitDate.setMonth(newLimitDate.getMonth() + 1, sipDate);

        var ddiff = DateDiff.inDays(dt2, newLimitDate);

        if (mode == 'STP' || mode == 'SWP') {
            if (k < 1 && period == "Monthly") {//ddiff > 0 
                alert(mode + " is allowed for minimum 1 Installment");
                document.getElementById('txtToDate').value = "";
                document.getElementById('txtToDate').focus();
                return false;
            }
            else if (k < 3 && period == "Quarterly") {
                alert(mode + " is allowed for minimum 1 Installment");
                document.getElementById('txtToDate').value = "";
                document.getElementById('txtToDate').focus();
                return false;
            }
        }
        else {

            if ((ddiff > 0) && period == "Monthly") { //k < 12 || dtdf <= 365
                alert("SIP is allowed for minimum 1 Installment");
                document.getElementById('txtToDate').value = "";
                document.getElementById('txtToDate').focus();
                return false;
            }
            else if ((k < 3 || dtdf <= 365) && period == "Quarterly") {
                alert("SIP is allowed for minimum 1 Installment");
                document.getElementById('txtToDate').value = "";
                document.getElementById('txtToDate').focus();
                return false;
            }


        }
    }
}


function pdfcheck() {
    //alert(5);


    var radiolist = document.getElementById("<%=RadioButtonListCustomerType.ClientID%>");

    var selectedRadio = $("input[name='RadioButtonListCustomerType']:checked").val().toString().toUpperCase();
    selectedRadio = $.trim(selectedRadio);

    if (selectedRadio != "DISTRIBUTOR") {
        return true;
    }





    if (document.getElementById("txtPreparedby").value == "") {
        alert('Please Enter your Name.');
        document.getElementById('txtPreparedby').focus();
        return false;
    }
    if (document.getElementById("txtMobile").value == "") {
        alert('Please Enter your Mobile No.');
        document.getElementById('txtMobile').focus();
        return false;
    }

    //    if (document.getElementById("txtMobile").value.length != 10) {
    //        alert('Please Enter your proper Mobile No.');
    //        document.getElementById('txtMobile').focus();
    //        return false;
    //    }



    if (document.getElementById("txtPreparedFor").value == "") {
        alert('Please Enter Prepared for Name.');
        document.getElementById('txtPreparedFor').focus();
        return false;
    }


    var Preparedby = document.getElementById("txtPreparedby");
    if (!allLetter(Preparedby))
        return false;

    var PreparedFor = document.getElementById("txtPreparedFor");
    if (!allLetter(PreparedFor))
        return false;




    if (document.getElementById("txtEmail").value == "") {
        alert('Please Enter Email Id.');
        document.getElementById('txtEmail').focus();
        return false;
    }


    //var RegX = /^[A-Za-z.'' ]{1,200}$/;
    var field = document.getElementById("txtEmail").value;

    //var ck_advisor = /^[A-Za-z0-9.'' ]{1,200}$/;

    var pattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    if (!pattern.test(field)) {
        alert("Please Enter Valid Email Address"); return false;
    }


    return true;
}

function allLetter(uname) {
    var letters = /^[A-Za-z ]+$/;
    if (uname.value.match(letters)) {
        return true;
    }
    else {
        alert('Name must have alphabet characters only');
        uname.value = "";
        uname.focus();
        return false;
    }
}

function validateDSP() {
    if (document.getElementById("ddlMode").value == 'SWP') {
        //alert("swp");

        if (!validate_SWP())
            return false;
        else {
            pop();
            return true;
        }
    }
    else if (document.getElementById("ddlMode").value == 'STP') {

        if (!validate_STP())
            return false;
        else {
            pop();
            return true;
        }
    }
    else {
        // alert("sip");
        if (!validate_SIP())
            return false;
        else {
            pop();
            return true;
        }
    }
}




function validate_SIP() {
    // ****************Created By Mukesh *****************

    //alert("hi");   

    //    alert(document.getElementById("ddlMode").value)

    if (document.getElementById("ddlscheme").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlscheme").focus();
        return false;
    }

    if (document.getElementById("ddlMode").value == 'Lump Sum') {

        var sipamunt = document.getElementById('txtinstallLs').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
        //alert(sipamunt);
        if (sipamunt == "") {
            alert("Value of Investment Amount cannot be Blank");
            document.getElementById('txtinstallLs').focus();
            return false;
        }

        if (sipamunt.indexOf(".") > -1) {
            alert("Please Enter Integral value");
            document.getElementById('txtinstallLs').value = "";
            document.getElementById('txtinstallLs').focus();
            return false;
        }

        if (isNaN(sipamunt)) {
            alert("Please enter Numeric value.");
            document.getElementById('txtinstallLs').value = "";
            document.getElementById('txtinstallLs').focus();
            return false;
        }
        else {

            if (sipamunt == 0) {
                alert("Please enter Valid Numeric value.");
                document.getElementById('txtinstallLs').value = "";
                document.getElementById('txtinstallLs').focus();
                return false;
            }
            if (sipamunt < 5000) {
                alert('Minimum  installment amount should be Rs. 5000/- ');
                document.getElementById('txtinstallLs').value = "";
                document.getElementById("txtinstallLs").focus();
                return false;
            }
            //            else if (sipamunt >= 500) {
            //                if (sipamunt % 500 != 0) {
            //                    alert('Minimum Amount should be Rs.500/- and in multiples of Rs.500/- thereafter');
            //                    document.getElementById('txtinstallLs').value = "";
            //                    document.getElementById("txtinstallLs").focus();
            //                    return false;
            //                }
            //            }
            //            else

        }



        if (document.getElementById("txtLumpfromDate").value == '') {
            alert('Please enter Start date.');
            document.getElementById("txtLumpfromDate").focus();
            return false;
        }

        if (document.getElementById("txtLumpToDate").value == '') {
            alert('Please enter End date.');
            document.getElementById("txtLumpToDate").focus();
            return false;
        }
        var LumpStartdata = document.getElementById('txtLumpfromDate').value;
        var LumpEnddata = document.getElementById('txtLumpToDate').value;

        var Day = parseInt(LumpStartdata.substring(0, 2), 10);
        var Mn = parseInt(LumpStartdata.substring(3, 5), 10);
        var Yr = parseInt(LumpStartdata.substring(6, 10), 10);
        var DateVal = Mn + "/" + Day + "/" + Yr;
        var dt = new Date(DateVal);

        Day = parseInt(LumpEnddata.substring(0, 2), 10);
        Mn = parseInt(LumpEnddata.substring(3, 5), 10);
        Yr = parseInt(LumpEnddata.substring(6, 10), 10);
        DateVal = Mn + "/" + Day + "/" + Yr;

        var dtLSend = new Date(DateVal);







        var todaydate = new Date();
        //alert(todaydate);alert(dt);
        var i = DateDiff.inDays(dt, todaydate)
        if (i <= 0) {
            alert("Start Date should be Less than Today");
            document.getElementById('txtLumpfromDate').value = "";
            document.getElementById('txtLumpfromDate').focus();
            return false;
        }

        i = DateDiff.inDays(dtLSend, todaydate)
        if (i <= 0) {
            alert("End Date should be Less than Today");
            document.getElementById('txtLumpToDate').value = "";
            document.getElementById('txtLumpToDate').focus();
            return false;
        }



        if (!IsValidDate(LumpStartdata, LumpEnddata)) {
            alert("Start Date should be Less than End Date");
            document.getElementById('txtLumpfromDate').value = "";
            document.getElementById('txtLumpfromDate').focus();
            return false;
        }

        return true;
    }







    //    if (document.getElementById("ddlsipbnmark").value == '0') {
    //        alert('Please select any Benchmark.');
    //        document.getElementById("ddlsipbnmark").focus();
    //        return false;
    //    }

    //    if (document.getElementById("ddPeriod_SIP").value == '0') {
    //        alert('Please select Frequency.');
    //        document.getElementById("ddPeriod_SIP").focus();
    //        return false;
    //    }

    //    if (document.getElementById("txtddSIPdate").value == '0') {
    //        alert('Please select SIP Date.');
    //        document.getElementById("txtddSIPdate").focus();
    //        return false;
    //    }

    //    if (isNaN(document.getElementById("txtddSIPdate").value)) {
    //        alert("Please enter Numeric value.");
    //        document.getElementById('txtddSIPdate').value = "";
    //        document.getElementById('txtddSIPdate').focus();
    //        return false;
    //    }

    //    if (document.getElementById("txtddPeriod_SIP").value != 'Monthly' && document.getElementById("txtddPeriod_SIP").value != 'Quarterly' && document.getElementById("txtddPeriod_SIP").value != 'Weekly') {
    //        document.getElementById("txtddPeriod_SIP").value = '';
    //        alert('Please select any Period.');
    //        document.getElementById("txtddPeriod_SIP").focus();
    //        return false;
    //    }

    //    var sipdate = document.getElementById("txtddSIPdate").value;
    //    if (sipdate != '7' && sipdate != '14' && sipdate != '21' && sipdate != '28') {
    //        document.getElementById("txtddSIPdate").value = '';
    //        alert('Please select valid SIP Date.');
    //        document.getElementById("txtddSIPdate").focus();
    //        return false;
    //    }


    if (document.getElementById("ddlMode").value == 'SIP with Initial Investment') {

        if (document.getElementById("txtIniToDate").value == '') {
            alert('Please enter Initial date.');
            document.getElementById("txtIniToDate").focus();
            return false;
        }

        var sipamunt = document.getElementById('txtiniAmount').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
        //alert(sipamunt);
        if (sipamunt == "") {
            alert("Value of Initial Amount cannot be Blank");
            document.getElementById('txtiniAmount').focus();
            return false;
        }

        if (sipamunt.indexOf(".") > -1) {
            alert("Please Enter Integral value");
            document.getElementById('txtiniAmount').value = "";
            document.getElementById('txtiniAmount').focus();
            return false;
        }

        if (isNaN(sipamunt)) {
            alert("Please enter Numeric value.");
            document.getElementById('txtiniAmount').value = "";
            document.getElementById('txtiniAmount').focus();
            return false;
        }
        else {

            if (sipamunt == 0) {
                alert("Please enter Valid Numeric value.");
                document.getElementById('txtiniAmount').value = "";
                document.getElementById('txtiniAmount').focus();
                return false;
            }
            if (sipamunt < 500) {
                alert('Minimum  initial amount should be Rs. 500/- ');
                document.getElementById('txtiniAmount').value = "";
                document.getElementById("txtiniAmount").focus();
                return false;
            }
        }




        var iniStartdata = document.getElementById('txtIniToDate').value;

        if (document.getElementById("txtfromDate").value == '') {
            alert('Please enter From date.');
            document.getElementById("txtfromDate").focus();
            return false;
        }

        var sipStartdata = document.getElementById('txtfromDate').value;


        var Day = parseInt(iniStartdata.substring(0, 2), 10);
        var Mn = parseInt(iniStartdata.substring(3, 5), 10);
        var Yr = parseInt(iniStartdata.substring(6, 10), 10);
        var DateVal = Mn + "/" + Day + "/" + Yr;
        var dt = new Date(DateVal);

        var todaydate = new Date();
        //alert(todaydate);alert(dt);
        var i = DateDiff.inDays(dt, todaydate)
        if (i <= 0) {
            alert("initial Date should be Less than Today");
            document.getElementById('txtIniToDate').value = "";
            document.getElementById('txtIniToDate').focus();
            return false;
        }

        if (!IsValidDateEqual(iniStartdata, sipStartdata)) {
            alert("Initial Date should be Less or Equal to Start Date");
            document.getElementById('txtIniToDate').value = "";
            document.getElementById('txtIniToDate').focus();
            return false;
        }

        var ddScheme = document.getElementById("ddlscheme");
        var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
        if (!IsValidDate(allotdate, iniStartdata)) {
            alert("Initial Date should be greater than or Equal to Inception Date " + allotdate);
            document.getElementById('txtIniToDate').value = "";
            document.getElementById('txtIniToDate').focus();
            return false;
        }



    }





    var sipamunt = document.getElementById('txtinstall').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
    //alert(sipamunt);
    if (sipamunt == "") {
        alert("Value of Installment cannot be Blank");
        document.getElementById('txtinstall').focus();
        return false;
    }

    if (sipamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value");
        document.getElementById('txtinstall').value = "";
        document.getElementById('txtinstall').focus();
        return false;
    }

    if (isNaN(sipamunt)) {
        alert("Please enter Numeric value.");
        document.getElementById('txtinstall').value = "";
        document.getElementById('txtinstall').focus();
        return false;
    }
    else {

        if (sipamunt == 0) {
            alert("Please enter Valid Numeric value.");
            document.getElementById('txtinstall').value = "";
            document.getElementById('txtinstall').focus();
            return false;
        }
        if (sipamunt < 500) {
            alert('Minimum  installment amount should be Rs. 500/- ');
            document.getElementById('txtinstall').value = "";
            document.getElementById("txtinstall").focus();
            return false;
        }
    }


    if (document.getElementById("txtfromDate").value == '') {
        alert('Please enter From date.');
        document.getElementById("txtfromDate").focus();
        return false;
    }


    if (document.getElementById("txtToDate").value == '') {
        alert('Please enter To date.');
        document.getElementById("txtToDate").focus();
        return false;
    }

    if (document.getElementById("txtvalason").value == '') {
        alert('Please enter value as on date.');
        document.getElementById("txtvalason").focus();
        return false;
    }


    ///// date part /////
    var sipStartdata = document.getElementById('txtfromDate').value;
    var sipEnddata = document.getElementById('txtToDate').value;
    var sipAsondata = document.getElementById('txtvalason').value;

    //alert(sipStartdata);
    var Day = parseInt(sipStartdata.substring(0, 2), 10);
    var Mn = parseInt(sipStartdata.substring(3, 5), 10);
    var Yr = parseInt(sipStartdata.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);


    var Day2 = parseInt(sipEnddata.substring(0, 2), 10);
    var Mn2 = parseInt(sipEnddata.substring(3, 5), 10);
    var Yr2 = parseInt(sipEnddata.substring(6, 10), 10);
    var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;
    var dt2 = new Date(DateVal2);


    Day = parseInt(sipAsondata.substring(0, 2), 10);
    Mn = parseInt(sipAsondata.substring(3, 5), 10);
    Yr = parseInt(sipAsondata.substring(6, 10), 10);
    DateVal = Mn + "/" + Day + "/" + Yr;
    var dt3 = new Date(DateVal);



    var todaydate = new Date();
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)
    if (i <= 0) {
        alert("From Date should be Less than Today");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    i = DateDiff.inDays(dt3, todaydate)
    if (i <= 0) {
        alert("Value as on Date should be Less than Today");
        document.getElementById('txtvalason').value = "";
        document.getElementById('txtvalason').focus();
        return false;
    }
    /////////////

    //////////////////////////
    // alert(dt); alert(dt2);
    var k = DateDiff.inMonths(dt, dt2);
    var dtdf = DateDiff.inDays(dt, dt2);
    // alert(dtdf);

    //alert(k);

    var period = document.getElementById("ddPeriod_SIP").value;
    //alert(period);
    //  alert(ddlscheme.options[ddlscheme.selectedIndex].value);
    // alert(ddScheme.options[ddScheme.selectedIndex].text);
    //    var x = document.getElementById("ddlscheme").selectedIndex;
    //    alert(document.getElementsByTagName("option")[x].value);
    //    var schemeid = 2631 or 2632
    var ind = document.getElementById('ddlscheme').selectedIndex;
    if (ind > 0) {


        var schemeid = document.getElementById('ddlscheme').options[ind].value;

        var addMonth = 1;



        var newLimitDate = dt;
       // newLimitDate.setMonth(newLimitDate.getMonth() + addMonth);
        var day = newLimitDate.getDate();
        var sipDate = document.getElementById("ddSIPdate").value;
        if (day <= sipDate)
            newLimitDate.setDate(sipDate);
        else
            newLimitDate.setMonth(newLimitDate.getMonth() + 1, sipDate);

        var ddiff = DateDiff.inDays(dt2, newLimitDate);




        if (ddiff > 0 && period == "Weekly") {//(k < 11) 12 || dtdf <= 365 
            alert("SIP is allowed for minimum 1 Installment");
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }
        else if (dtdf <= 30 && period == "Monthly") {
            alert("SIP is allowed for minimum 1 Installment");
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }
        else if ((k < 3 || dtdf <= 120) && period == "Quarterly") {
            alert("SIP is allowed for minimum 1 Installment");
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }

    }

    var startsensexdate = new Date("01/01/1980");
    if (dt < startsensexdate) {
        alert("Please select Value as of Date from 1st January 1980");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    if (!IsValidDate(sipStartdata, sipEnddata)) {
        alert("From Date should be Less than End Date");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    if (!IsValidDate(sipEnddata, sipAsondata)) {
        alert("Value as on Date should be greater than End Date ");
        document.getElementById('txtvalason').value = "";
        document.getElementById('txtvalason').focus();
        return false;
    }

    //sipStartdata


    var ddScheme = document.getElementById("ddlscheme");
    //alert(ddScheme.options[ddScheme.selectedIndex].text);
    // alert(ddScheme.options[ddScheme.selectedIndex].value);
    //alert(ddScheme.options[ddScheme.selectedIndex].title);
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    //alert(allotdate);
    if (!IsValidDate(allotdate, sipStartdata)) {
        alert("From Date should be greater than or Equal to Inception Date " + allotdate);
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    // pop();
    return true;
}



function validate_SWP() {
    // ****************Created By Mukesh *****************

    //alert("hi");

    if (document.getElementById("ddlscheme").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlscheme").focus();
        return false;
    }

    //    if (document.getElementById("ddlbnmark").value == '0') {
    //        alert('Please select any Benchmark.');
    //        document.getElementById("ddlbnmark").focus();
    //        return false;
    //    }

    if (document.getElementById("ddPeriod_SIP").value == "") {
        alert('Please select Frequency.');
        document.getElementById("ddPeriod_SIP").focus();
        return false;
    }




    //    if (document.getElementById("txtddwperiod").value != 'Monthly' && document.getElementById("txtddwperiod").value != 'Quarterly') {
    //        document.getElementById("txtddwperiod").value = '';
    //        alert('Please select any Period.');
    //        document.getElementById("txtddwperiod").focus();
    //        return false;
    //    }



    //    if (document.getElementById("txtddSWPdate").value == "") {
    //        alert('Please select SWP Date.');
    //        document.getElementById("txtddSWPdate").focus();
    //        return false;
    //    }

    //    var swpdate = document.getElementById("txtddSWPdate").value;
    //    if (swpdate != '7' && swpdate != '14' && swpdate != '21' && swpdate != '28') {
    //        document.getElementById("txtddSWPdate").value = '';
    //        alert('Please select valid SWP Date.');
    //        document.getElementById("txtddSWPdate").focus();
    //        return false;
    //    }


    var sipamunt = document.getElementById('txtiniAmount').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
    //alert(sipamunt);
    if (sipamunt == "") {
        alert("Value of Initial amount cannot be Blank");
        document.getElementById('txtiniAmount').focus();
        return false;
    }

    if (sipamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value");
        document.getElementById('txtiniAmount').value = "";
        document.getElementById('txtiniAmount').focus();
        return false;
    }

    if (isNaN(sipamunt)) {
        alert("Please enter Numeric value.");
        document.getElementById('txtiniAmount').value = "";
        document.getElementById('txtiniAmount').focus();
        return false;
    }
    else {

        if (sipamunt == 0) {
            alert("Please enter Valid Numeric value.");
            document.getElementById('txtiniAmount').value = "";
            document.getElementById('txtiniAmount').focus();
            return false;
        }
        else if (sipamunt >= 3000) {
            if (sipamunt % 100 != 0) {
                alert('Minimum initial amount should be Rs.3000/- and in multiples of Rs.100/- thereafter');
                document.getElementById("txtiniAmount").focus();
                return false;
            }
        }
        else {
            alert('Minimum  initial amount should be Rs. 3000/- ');
            document.getElementById("txtiniAmount").focus();
            return false;
        }
    }

    if (document.getElementById("txtIniToDate").value == '') {
        alert('Please enter Initial date.');
        document.getElementById("txtIniToDate").focus();
        return false;
    }
    var sipwdamunt = document.getElementById('txtTransferWithdrawal2').value;

    if (sipwdamunt == "") {
        alert("Value of withdrawal amount cannot be Blank");
        document.getElementById('txtTransferWithdrawal2').focus();
        return false;
    }

    if (sipwdamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value in withdrawal amount ");
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById('txtTransferWithdrawal2').focus();
        return false;
    }

    if (isNaN(sipwdamunt)) {
        alert("Please enter Numeric value withdrawal amount.");
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById('txtTransferWithdrawal2').focus();
        return false;
    }
    else {

        if (sipwdamunt == 0) {
            alert("Please enter Valid Numeric value withdrawal amount.");
            document.getElementById('txtTransferWithdrawal2').value = "";
            document.getElementById('txtTransferWithdrawal2').focus();
            return false;
        }
    }


    //    if (parseFloat(document.getElementById("txtwinamt").value) < 25000) {
    //        alert('Initial amount cannot be less than 25,000/- Rs.');
    //        document.getElementById('txtwinamt').value = "";
    //        document.getElementById("txtwinamt").focus();
    //        return false;
    //    }

    if (parseFloat(document.getElementById("txtTransferWithdrawal2").value) < 500) {
        alert('Minimum withdrawal amount should be Rs.500/- and in multiples of Rs.100/- thereafter');
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById("txtTransferWithdrawal2").focus();
        return false;
    }


    //    if (parseFloat(document.getElementById("txtwtramt").value) > 25000) {
    //        alert('Maximum withdrawal amount should be Rs.25000');
    //        document.getElementById('txtwtramt').value = "";
    //        document.getElementById("txtwtramt").focus();
    //        return false;
    //    }


    if (parseFloat(document.getElementById("txtTransferWithdrawal2").value) >= 500) {
        var amt;
        amt = document.getElementById("txtTransferWithdrawal2").value;
        if (amt % 100 != 0) {
            alert('Minimum withdrawal amount should be Rs.500/- and in multiples of Rs.100/- thereafter');
            document.getElementById('txtTransferWithdrawal2').value = "";
            document.getElementById("txtTransferWithdrawal2").focus();
            return false;
        }
    }

    var initamt;
    var withamt;
    initamt = document.getElementById("txtiniAmount").value;
    withamt = document.getElementById("txtTransferWithdrawal2").value;

    if (initamt / withamt < 1) {
        alert('Withdrawal amount cannot be greater than initial amount.');
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById("txtTransferWithdrawal2").focus();
        return false;
    }


    if (initamt / withamt < 6) {
        alert('Enter Withdrawal amount for minimum 1 transaction.');
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById("txtTransferWithdrawal2").focus();
        return false;
    }



    if (document.getElementById("txtfromDate").value == '') {
        alert('Please enter From date.');
        document.getElementById("txtfromDate").focus();
        return false;
    }

    if (document.getElementById("txtToDate").value == '') {
        alert('Please enter To date.');
        document.getElementById("txtToDate").focus();
        return false;
    }

    if (document.getElementById("txtvalason").value == '') {
        alert('Please enter value as on date.');
        document.getElementById("txtvalason").focus();
        return false;
    }


    ///// date part /////
    var swpStartdata = document.getElementById('txtfromDate').value;
    var swpEnddata = document.getElementById('txtToDate').value;
    var swpAsondata = document.getElementById('txtvalason').value;
    var initialdate = document.getElementById("txtIniToDate").value

    //alert(sipStartdata);
    var Day = parseInt(swpStartdata.substring(0, 2), 10);
    var Mn = parseInt(swpStartdata.substring(3, 5), 10);
    var Yr = parseInt(swpStartdata.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);

    Day = parseInt(swpEnddata.substring(0, 2), 10);
    Mn = parseInt(swpEnddata.substring(3, 5), 10);
    Yr = parseInt(swpEnddata.substring(6, 10), 10);
    var DateVal2 = Mn + "/" + Day + "/" + Yr;
    var dt2 = new Date(DateVal2);


    ay = parseInt(initialdate.substring(0, 2), 10);
    Mn = parseInt(initialdate.substring(3, 5), 10);
    Yr = parseInt(initialdate.substring(6, 10), 10);
    var DateValini = Mn + "/" + Day + "/" + Yr;
    var dtini = new Date(DateValini);


    var todaydate = new Date();

    var iini = DateDiff.inDays(dtini, todaydate);
    if (iini <= 0) {
        alert("Initial Date should be Less than Today");
        document.getElementById('txtIniToDate').value = "";
        document.getElementById('txtIniToDate').focus();
        return false;
    }
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)
    if (i <= 0) {
        alert("From Date should be Less than Today");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    var startsensexdate = new Date("01/01/1980");
    if (dt < startsensexdate) {
        alert("Please select Value as of Date from 1st January 1980");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    if (!IsValidDate(initialdate, swpStartdata)) {
        alert("Initial Date should be Less than From Date");
        document.getElementById('txtIniToDate').value = "";
        document.getElementById('txtIniToDate').focus();
        return false;
    }

    if (!IsValidDate(swpStartdata, swpEnddata)) {
        alert("From Date should be Less than End Date");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    if (!IsValidDate(swpEnddata, swpAsondata)) {
        alert("Value as on Date should be greater than End Date ");
        document.getElementById('txtvalason').value = "";
        document.getElementById('txtvalason').focus();
        return false;
    }

    //scheme
    var ddScheme = document.getElementById("ddlscheme");
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    //alert(allotdate); 

    if (!IsValidDate(allotdate, swpStartdata)) {
        alert("From Date should be greater than or Equal to Inception Date " + allotdate);
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    var k = DateDiff.inMonths(dt, dt2);
    var dydf = DateDiff.inDays(dt, dt2);

    var period = document.getElementById("ddPeriod_SIP").value;
    var ind = document.getElementById('ddlscheme').selectedIndex;

    if (ind > 0) {
        var schemeid = document.getElementById('ddlscheme').options[ind].value;

        if (k <= 1 && dydf < 31 && period == "Monthly") {
            alert("SWP is allowed for minimum 1 Installment");
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }
        else if (k < 3 && period == "Quarterly") {
            alert("SWP is allowed for minimum 1 Installment");
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }
    }







    return true;
}





function validate_STP() {
    // ****************Created By Mukesh *****************

    //alert("hi");   

    if (document.getElementById("ddlscheme").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlscheme").focus();
        return false;
    }

    if (document.getElementById("ddlschtrto").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlschtrto").focus();
        return false;
    }

    if (document.getElementById("ddPeriod_SIP").value == "") {
        alert('Please select Frequency.');
        document.getElementById("ddPeriod_SIP").focus();
        return false;
    }

    //    if (document.getElementById("ddbnmark").value == '0') {
    //        alert('Please select any Benchmark.');
    //        document.getElementById("ddbnmark").focus();
    //        return false;
    //    }

    var sipamunt = document.getElementById('txtiniAmount').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
    //alert(sipamunt);
    if (sipamunt == "") {
        alert("Value of Investment Amount cannot be Blank");
        document.getElementById('txtiniAmount').focus();
        return false;
    }

    if (sipamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value");
        document.getElementById('txtiniAmount').value = "";
        document.getElementById('txtiniAmount').focus();
        return false;
    }

    if (isNaN(sipamunt)) {
        alert("Please enter Numeric value.");
        document.getElementById('txtiniAmount').value = "";
        document.getElementById('txtiniAmount').focus();
        return false;
    }
    else {

        if (sipamunt == 0) {
            alert("Please enter Valid Numeric value.");
            document.getElementById('txtiniAmount').value = "";
            document.getElementById('txtiniAmount').focus();
            return false;
        }
        //        else if (sipamunt >= 1000) {
        //            if (sipamunt % 100 != 0) {
        //                alert('Minimum Investment amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
        //                document.getElementById("txtinstall").focus();
        //                return false;
        //            }
        //        }
        //        else {
        //            alert('Minimum Investment amount should be Rs. 1000/- ');
        //            document.getElementById("txtinstall").focus();
        //            return false;
        //        }
    }

    if (document.getElementById("txtIniToDate").value == '') {
        alert('Please enter Initial date.');
        document.getElementById("txtIniToDate").focus();
        return false;
    }
    var sipwdamunt = document.getElementById('txtTransferWithdrawal2').value;

    if (sipwdamunt == "") {
        alert("Value of transfer amount cannot be Blank");
        document.getElementById('txtTransferWithdrawal2').focus();
        return false;
    }

    if (sipwdamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value in transfer amount ");
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById('txtTransferWithdrawal2').focus();
        return false;
    }

    if (isNaN(sipwdamunt)) {
        alert("Please enter Numeric value transfer amount.");
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById('txtTransferWithdrawal2').focus();
        return false;
    }
    else {

        if (sipwdamunt == 0) {
            alert("Please enter Valid Numeric value transfer amount.");
            document.getElementById('txtTransferWithdrawal2').value = "";
            document.getElementById('txtTransferWithdrawal2').focus();
            return false;
        }
    }


    if (parseFloat(document.getElementById("txtiniAmount").value) < 3000) {
        alert('Investment amount cannot be less than Rs.3,000');
        document.getElementById('txtiniAmount').value = "";
        document.getElementById("txtiniAmount").focus();
        return false;
    }

    if (parseFloat(document.getElementById("txtTransferWithdrawal2").value) < 500) {
        alert('Minimum transfer amount should be Rs.500/- and in multiples of Rs.100/- thereafter');
        document.getElementById('txtTransferWithdrawal2').value = "";
        document.getElementById("txtTransferWithdrawal2").focus();
        return false;
    }

//    if (parseFloat(document.getElementById("txtTransferWithdrawal2").value) > 30000) {
//        alert('Maximum transfer amount should be Rs.30000');
//        document.getElementById('txtTransferWithdrawal2').value = "";
//        document.getElementById("txtTransferWithdrawal2").focus();
//        return false;
//    }


    if (parseFloat(document.getElementById("txtTransferWithdrawal2").value) >= 500) {
        var amt;
        amt = document.getElementById("txtTransferWithdrawal2").value;
        if (amt % 100 != 0) {
            alert('Minimum transfer amount should be Rs.500/- and in multiples of Rs.100/- thereafter');
            document.getElementById("txtTransferWithdrawal2").focus();
            return false;
        }
    }

    var initamt;
    var withamt;
    initamt = document.getElementById("txtiniAmount").value;
    withamt = document.getElementById("txtTransferWithdrawal2").value;

    if (initamt / withamt < 1) {
        alert('Transfer amount cannot be greater than initial amount.');
        document.getElementById("txtTransferWithdrawal2").focus();
        return false;
    }

    if (initamt / withamt < 1) {
        alert('Enter Transfer amount for a minimum 1 transaction.');
        document.getElementById("txtTransferWithdrawal2").focus();
        return false;
    }


    //    if (document.getElementById("txtddperiod").value == '') {
    //        alert('Please select any Period.');
    //        document.getElementById("txtddperiod").focus();
    //        return false;
    //    }

    //    if (document.getElementById("txtddperiod").value != 'Monthly' && document.getElementById("txtddperiod").value != 'Quarterly') {
    //        document.getElementById("txtddperiod").value = '';
    //        alert('Please select any Period.');
    //        document.getElementById("txtddperiod").focus();
    //        return false;
    //    }



    //    if (document.getElementById("txtddSTPDate").value == '') {
    //        alert('Please select any STP Date.');
    //        document.getElementById("txtddSTPDate").focus();
    //        return false;
    //    }

    //    var stpdate = document.getElementById("txtddSTPDate").value;
    //    // alert(stpdate);
    //    if (stpdate != '7' && stpdate != '14' && stpdate != '21' && stpdate != '28') {
    //        document.getElementById("txtddSTPDate").value = '';
    //        alert('Please select valid STP Date.');
    //        document.getElementById("txtddSTPDate").focus();
    //        return false;
    //    }

    if (document.getElementById("txtfromDate").value == '') {
        alert('Please enter From date.');
        document.getElementById("txtfromDate").focus();
        return false;
    }

    if (document.getElementById("txtToDate").value == '') {
        alert('Please enter To date.');
        document.getElementById("txtToDate").focus();
        return false;
    }

    if (document.getElementById("txtvalason").value == '') {
        alert('Please enter value as on date.');
        document.getElementById("txtvalason").focus();
        return false;
    }


    ///// date part /////
    var sipStartdata = document.getElementById('txtfromDate').value;
    var sipEnddata = document.getElementById('txtToDate').value;
    var sipAsondata = document.getElementById('txtvalason').value;
    var initialdate=document.getElementById("txtIniToDate").value

    //alert(sipStartdata);
    var Day = parseInt(sipStartdata.substring(0, 2), 10);
    var Mn = parseInt(sipStartdata.substring(3, 5), 10);
    var Yr = parseInt(sipStartdata.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);


    //alert(sipStartdata);
    Day = parseInt(sipEnddata.substring(0, 2), 10);
    Mn = parseInt(sipEnddata.substring(3, 5), 10);
    Yr = parseInt(sipEnddata.substring(6, 10), 10);
    var DateVal2 = Mn + "/" + Day + "/" + Yr;
    var dt2 = new Date(DateVal2);


    Day = parseInt(initialdate.substring(0, 2), 10);
    Mn = parseInt(initialdate.substring(3, 5), 10);
    Yr = parseInt(initialdate.substring(6, 10), 10);
    var DateValini = Mn + "/" + Day + "/" + Yr;
    var dtini = new Date(DateValini);

    var todaydate = new Date();

    var iini = DateDiff.inDays(dtini, todaydate);
    if (iini <= 0) {
        alert("Initial Date should be Less than Today");
        document.getElementById('txtIniToDate').value = "";
        document.getElementById('txtIniToDate').focus();
        return false;
    }

   
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate);
    if (i <= 0) {
        alert("From Date should be Less than Today");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    var startsensexdate = new Date("01/01/1980");
    if (dt < startsensexdate) {
        alert("Please select Value as of Date from 1st January 1980");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    if (!IsValidDate(initialdate, sipStartdata)) {
        alert("Initial Date should be Less than From Date");
        document.getElementById('txtIniToDate').value = "";
        document.getElementById('txtIniToDate').focus();
        return false;
    }

    if (!IsValidDate(sipStartdata, sipEnddata)) {
        alert("From Date should be Less than End Date");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    if (!IsValidDate(sipEnddata, sipAsondata)) {
        alert("Value as on Date should be greater than End Date ");
        document.getElementById('txtvalason').value = "";
        document.getElementById('txtvalason').focus();
        return false;
    }




    var ddScheme = document.getElementById("ddlscheme");
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    if (!IsValidDate(allotdate, sipStartdata)) {
        alert("From Date should be greater than or Equal to Inception Date of From Scheme " + allotdate);
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

    var ddSchemeto = document.getElementById("ddlschtrto");
    var allotdateto = ddSchemeto.options[ddSchemeto.selectedIndex].title;
    //alert(allotdateto);
    if (allotdateto != null) {
        if (!IsValidDate(allotdateto, sipStartdata)) {
            alert("From Date should be greater than or Equal to Inception Date of To Scheme " + allotdateto);
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }
    } else {
        alert("No date is found");
    }


    var k = DateDiff.inMonths(dt, dt2);

    var period = document.getElementById("ddPeriod_SIP").value;
    var ind = document.getElementById('ddlscheme').selectedIndex;
    var dydf = DateDiff.inDays(dt, dt2);
    if (ind > 0) {
        var schemeid = document.getElementById('ddlscheme').options[ind].value;

        if (k <= 1 && dydf < 31 && period == "Monthly") {
            alert("STP is allowed for minimum 1 Installment");
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }
        else if (k < 3 && period == "Quarterly") {
            alert("STP is allowed for minimum 1 Installment");
            document.getElementById('txtfromDate').value = "";
            document.getElementById('txtfromDate').focus();
            return false;
        }
    }


    return true;
}




function setFromDateAfterPostBack() {

    var ddScheme = document.getElementById("ddlscheme");
    if (ddScheme.selectedIndex != 0) {
        var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
        //alert(allotdate);
        var Day = parseInt(allotdate.substring(0, 2), 10);
        var Mn = parseInt(allotdate.substring(3, 5), 10);
        var Yr = parseInt(allotdate.substring(6, 10), 10);
        var DateVal = Mn + "/" + Day + "/" + Yr;
        var dt = new Date(DateVal);
        $("#txtIniToDate").datepicker("option", "minDate", dt);
        $("#txtfromDate").datepicker("option", "minDate", dt);
        $("#txtToDate").datepicker("option", "minDate", dt);
        $("#txtLumpfromDate").datepicker("option", "minDate", dt);
        $("#txtLumpToDate").datepicker("option", "minDate", dt);
        $("#txtvalason").datepicker("option", "minDate", dt);
    }

    if (document.getElementById("ddlMode").value == 'STP') {
        var ddSchemeto = document.getElementById("ddlschtrto");
        //alert(ddSchemeto.selectedIndex);
        if (ddSchemeto.selectedIndex > 0) {
            var allotdateto = ddSchemeto.options[ddSchemeto.selectedIndex].title;
            if (allotdateto != null) {
                var Day2 = parseInt(allotdateto.substring(0, 2), 10);
                var Mn2 = parseInt(allotdateto.substring(3, 5), 10);
                var Yr2 = parseInt(allotdateto.substring(6, 10), 10);
                var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;
                var dtTo = new Date(DateVal2);
                if (dt != null)
                    var i = DateDiff.inDays(dt, dtTo);
                //                alert(i);
                if (i >= 0) {
                    $("#txtfromDate").datepicker("option", "minDate", dtTo);
                    $("#txtToDate").datepicker("option", "minDate", dtTo);
                    $("#txtvalason").datepicker("option", "minDate", dtTo);
                }
            }
        }
    }
}
