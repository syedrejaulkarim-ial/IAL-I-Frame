
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

function isNumber(key) {
    var keycode = (key.which) ? key.which : key.keyCode;
    if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
        return true;
    }
    return false;
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
    if (dt >= dt1) {
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
    //alert(1)
    var ddScheme = document.getElementById("ddlscheme");
    // alert(ddScheme.options[ddScheme.selectedIndex].text);
    // alert(ddScheme.options[ddScheme.selectedIndex].value);
    // alert(ddScheme.options[ddScheme.selectedIndex].title);
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    // alert(id);


    var Day = parseInt(allotdate.substring(0, 2), 10);
    var Mn = parseInt(allotdate.substring(3, 5), 10);
    var Yr = parseInt(allotdate.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);
    var todaydate = new Date();
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)
    //    $(function () {  
    //    $("").datepicker({minDate: -889});
    //    }
    //$("#txtfromDate").datepicker("option", "minDate", new Date(2007, 1 - 1, 1));
    if (document.getElementById("ddlscheme").value != '0') {
        $("#txtfromDate").datepicker("option", "minDate", dt);
        $("#txtIniToDate").datepicker("option", "minDate", dt);
        $("#txtLumpfromDate").datepicker("option", "minDate", dt);
    }
    return true;
}


function validatePrincipal() {
   // alert(document.getElementById("ddlMode").value);
    if (document.getElementById("ddlMode").value == 'SIP') {

        if (!validate_SIP())
            return false;
        else {
            return true;
        }

        
    }
    else if (document.getElementById("ddlMode").value == 'LumpSum')
    {
        if (!validate_LUMPSUM())
            return false;
        else {
            return true;
        }
    }
}






function validate_SIP() {
    if (document.getElementById("ddlscname").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlscname").focus();
        return false;
    }

    var sipStartdata = document.getElementById('txtstartDate').value;
    var sipEnddata = document.getElementById('txtendDate').value;
    var sipAsondate = document.getElementById('txtvalDate').value;
    //var sipAmunt = document.getElementById("sipAmount").value;
    var sipAmunt = document.getElementById('txtsipAmount').value;
    //document.getElementById('<%=sipAmount.ClientID%>').value;

    if (sipStartdata == "" || sipEnddata == "" || sipAsondate == "" || sipAmunt == "") {

        if (sipStartdata == "") {
            alert("Start Date cannot be Blank");
            document.getElementById('txtstartDate').focus();
            return false;
        }
        else if (sipEnddata == "") {
            alert("End Date cannot be Blank");
            document.getElementById('txtendDate').focus();
            return false;
        }
        else if (sipAsondate == "") {
            alert("Value As on Date cannot be Blank");
            document.getElementById('txtvalDate').focus();
            return false;
        }
        else {
            alert("Amount cannot be Blank");
            document.getElementById('txtsipAmount').focus();
            return false;
        }

    }

    if (sipAmunt.indexOf(".") > -1) {
        alert("Please Enter Integral value");
        document.getElementById('txtsipAmount').value = "";
        document.getElementById('txtsipAmount').focus();
        return false;
    }

    if (isNaN(sipAmunt)) {
        alert("Please enter Numeric value.");
        document.getElementById('txtsipAmount').value = "";
        document.getElementById('txtsipAmount').focus();
        return false;
    }
    else {

        if (sipAmunt == 0) {
            alert("Please enter Valid Numeric value.");
            document.getElementById('txtsipAmount').value = "";
            document.getElementById('txtsipAmount').focus();
            return false;
        }
    }

    if (parseFloat(sipAmunt) < 1000) {
        alert('Minimum amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
        document.getElementById('txtsipAmount').value = "";
        document.getElementById("txtsipAmount").focus();
        return false;
    }
    else {
        if (sipAmunt % 100 != 0) {
            alert('Minimum amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
            document.getElementById('txtsipAmount').value = "";
            document.getElementById("txtsipAmount").focus();
            return false;
        }
    }



    //alert(sipStartdata);
    var Day = parseInt(sipAsondate.substring(0, 2), 10);
    var Mn = parseInt(sipAsondate.substring(3, 5), 10);
    var Yr = parseInt(sipAsondate.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);

    Day = parseInt(sipStartdata.substring(0, 2), 10);
    Mn = parseInt(sipStartdata.substring(3, 5), 10);
    Yr = parseInt(sipStartdata.substring(6, 10), 10);
    var DateVal1 = Mn + "/" + Day + "/" + Yr;
    var startdt = new Date(DateVal1);

    var todaydate = new Date();
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)
    if (i <= 0) {
        alert("Value as on Date should be Less than Today");
        document.getElementById('txtvalDate').value = "";
        document.getElementById('txtvalDate').focus();
        return false;
    }

    var i2 = DateDiff.inDays(startdt, todaydate)
    if (i2 <= 0) {
        alert("Start Date should be Less than Today");
        document.getElementById('txtstartDate').value = "";
        document.getElementById('txtstartDate').focus();
        return false;
    }

    var startsensexdate = new Date("01/01/1980");
    if (startdt < startsensexdate) {
        alert("Please select Value as of Date from 1st January 1980");
        document.getElementById('txtstartDate').value = "";
        document.getElementById('txtstartDate').focus();
        return false;
    }

    if (!IsValidDate(sipStartdata, sipEnddata)) {
        alert("From Date should be Less than End Date");
        document.getElementById('txtstartDate').value = "";
        document.getElementById('txtstartDate').focus();
        return false;
    }

    if (!IsValidDate(sipEnddata, sipAsondate)) {
        alert("Value as on Date should be greater than End Date ");
        document.getElementById('txtvalDate').value = "";
        document.getElementById('txtvalDate').focus();
        return false;
    }


    var ddScheme = document.getElementById("ddlscname");
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    if (!IsValidDate(allotdate, sipStartdata)) {
        alert("From Date should be greater than allotment Date" + allotdate);
        document.getElementById('txtstartDate').value = "";
        document.getElementById('txtstartDate').focus();
        return false;
    }




    return true;
}


function validate_LUMPSUM() {

    if (document.getElementById("ddlscname").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlscname").focus();
        return false;
    }

    var LsStartdata = document.getElementById('LumpStartDate').value;
    var LsEnddata = document.getElementById('LumpEndDate').value;
    var LsAmunt = document.getElementById('LumpAmount').value;


    if (LsStartdata == "" || LsEnddata == "" || LsAmunt == "") {

        if (LsStartdata == "") {
            alert("Start Date cannot be Blank");
            document.getElementById('LumpStartDate').focus();
            return false;
        }
        else if (LsEnddata == "") {
            alert("Value as of Date cannot be Blank");
            document.getElementById('LumpEndDate').focus();
            return false;
        }        
        else {
            alert("Amount cannot be Blank");
            document.getElementById('LumpAmount').focus();
            return false;
        }
    }

    if (isNaN(LsAmunt)) {
        alert("Please enter Numeric value.");
        document.getElementById('LumpAmount').value = "";
        document.getElementById('LumpAmount').focus();
        return false;
    }
    else {

        if (LsAmunt == 0) {
            alert("Please enter Valid Numeric value.");
            document.getElementById('LumpAmount').value = "";
            document.getElementById('LumpAmount').focus();
            return false;
        }
    }

    if (parseFloat(LsAmunt) < 1000) {
        alert('Minimum amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
        document.getElementById('LumpAmount').value = "";
        document.getElementById("LumpAmount").focus();
        return false;
    }
    else {
        if (LsAmunt % 100 != 0) {
            alert('Minimum amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
            document.getElementById('LumpAmount').value = "";
            document.getElementById("LumpAmount").focus();
            return false;
        }
    }


    var Day = parseInt(LsStartdata.substring(0, 2), 10);
    var Mn = parseInt(LsStartdata.substring(3, 5), 10);
    var Yr = parseInt(LsStartdata.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);

    var todaydate = new Date();
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)
    if (i <= 0) {
        alert("Start Date should be Less than Today");
        document.getElementById('LumpStartDate').value = "";
        document.getElementById('LumpStartDate').focus();
        return false;
    }

    if (!IsValidDate(LsStartdata, LsEnddata)) {
        alert("Start Date should be Less than Value as of Date");
        document.getElementById('LumpStartDate').value = "";
        document.getElementById('LumpStartDate').focus();
        return false;
    }


    var ddScheme = document.getElementById("ddlscname");
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    if (!IsValidDate(allotdate, LsStartdata)) {
        alert("From Date should be greater than allotment Date " + allotdate);
        document.getElementById('LumpStartDate').value = "";
        document.getElementById('LumpStartDate').focus();
        return false;
    }

    return true;
}





function validate_SWP() {
    // ****************Created By Mukesh *****************

    // alert("hi");   

    if (document.getElementById("ddlscname").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlscname").focus();
        return false;
    }

    if (document.getElementById("ddlbnmark").value == '0') {
        alert('Please select any Benchmark.');
        document.getElementById("ddlbnmark").focus();
        return false;
    }

    if (document.getElementById("ddwperiod").value == "") {
        alert('Please select Frequency.');
        document.getElementById("ddwperiod").focus();
        return false;
    }




    var swpdate = document.getElementById("ddSWPdate").value;
    if (swpdate != '1' && swpdate != '7' && swpdate != '14' && swpdate != '21' && swpdate != '28') {
        document.getElementById("ddSWPdate").value = '';
        alert('Please select valid SWP Date.');
        document.getElementById("ddSWPdate").focus();
        return false;
    }


    var sipamunt = document.getElementById('txtwinamt').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
    //alert(sipamunt);
    if (sipamunt == "") {
        alert("Value of Intial amount cannot be Blank");
        document.getElementById('txtwinamt').focus();
        return false;
    }

    if (sipamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value");
        document.getElementById('txtwinamt').value = "";
        document.getElementById('txtwinamt').focus();
        return false;
    }

    if (isNaN(sipamunt)) {
        alert("Please enter Numeric value.");
        document.getElementById('txtwinamt').value = "";
        document.getElementById('txtwinamt').focus();
        return false;
    }
    else {

        if (sipamunt == 0) {
            alert("Please enter Valid Numeric value.");
            document.getElementById('txtwinamt').value = "";
            document.getElementById('txtwinamt').focus();
            return false;
        }
        //        else if (sipamunt >= 1000) {
        //            if (sipamunt % 100 != 0) {
        //                alert('Minimum Initial amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
        //                document.getElementById("txtwinamt").focus();
        //                return false;
        ////            }
        //        }
        //        else {
        //            alert('Minimum Initial amount should be Rs. 1000/- ');
        //            document.getElementById("txtwinamt").focus();
        //            return false;
        //        }
    }

    var sipwdamunt = document.getElementById('txtwtramt').value;

    if (sipwdamunt == "") {
        alert("Value of withdrawal amount cannot be Blank");
        document.getElementById('txtwtramt').focus();
        return false;
    }

    if (sipwdamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value in withdrawal amount ");
        document.getElementById('txtwtramt').value = "";
        document.getElementById('txtwtramt').focus();
        return false;
    }

    if (isNaN(sipwdamunt)) {
        alert("Please enter Numeric value withdrawal amount.");
        document.getElementById('txtwtramt').value = "";
        document.getElementById('txtwtramt').focus();
        return false;
    }
    else {

        if (sipwdamunt == 0) {
            alert("Please enter Valid Numeric value withdrawal amount.");
            document.getElementById('txtwtramt').value = "";
            document.getElementById('txtwtramt').focus();
            return false;
        }
    }


    if (parseFloat(document.getElementById("txtwinamt").value) < 12000) {
        alert('Initial amount cannot be less than 12,000/- Rs.');
        document.getElementById('txtwinamt').value = "";
        document.getElementById("txtwinamt").focus();
        return false;
    }

    if (parseFloat(document.getElementById("txtwtramt").value) < 1000) {
        alert('Minimum withdrawal amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
        document.getElementById('txtwtramt').value = "";
        document.getElementById("txtwtramt").focus();
        return false;
    }


    //    if (parseFloat(document.getElementById("txtwtramt").value) > 25000) {
    //        alert('Maximum withdrawal amount should be Rs.25000');
    //        document.getElementById('txtwtramt').value = "";
    //        document.getElementById("txtwtramt").focus();
    //        return false;
    //    }


    if (parseFloat(document.getElementById("txtwtramt").value) >= 1000) {
        var amt;
        amt = document.getElementById("txtwtramt").value;
        if (amt % 100 != 0) {
            alert('Minimum withdrawal amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
            document.getElementById('txtwtramt').value = "";
            document.getElementById("txtwtramt").focus();
            return false;
        }
    }

    var initamt;
    var withamt;
    initamt = document.getElementById("txtwinamt").value;
    withamt = document.getElementById("txtwtramt").value;

    if (initamt / withamt < 1) {
        alert('Withdrawal amount cannot be greater than initial amount.');
        document.getElementById("txtwtramt").focus();
        return false;

    }




    if (document.getElementById("txtwfrdt").value == '') {
        alert('Please enter From date.');
        document.getElementById("txtwfrdt").focus();
        return false;
    }

    if (document.getElementById("txtwtdt").value == '') {
        alert('Please enter To date.');
        document.getElementById("txtwtdt").focus();
        return false;
    }

    if (document.getElementById("txtwvaldate").value == '') {
        alert('Please enter value as on date.');
        document.getElementById("txtwvaldate").focus();
        return false;
    }


    ///// date part /////
    var swpStartdata = document.getElementById('txtwfrdt').value;
    var swpEnddata = document.getElementById('txtwtdt').value;
    var swpAsondata = document.getElementById('txtwvaldate').value;

    //alert(sipStartdata);
    var Day = parseInt(swpAsondata.substring(0, 2), 10);
    var Mn = parseInt(swpAsondata.substring(3, 5), 10);
    var Yr = parseInt(swpAsondata.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;
    var dt = new Date(DateVal);

    var todaydate = new Date();
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)
    if (i <= 0) {
        alert("Value as on Date should be Less than Today");
        document.getElementById('txtwvaldate').value = "";
        document.getElementById('txtwvaldate').focus();
        return false;
    }

    var startsensexdate = new Date("01/01/1980");
    if (dt < startsensexdate) {
        alert("Please select Value as of Date from 1st January 1980");
        document.getElementById('txtwfrdt').value = "";
        document.getElementById('txtwfrdt').focus();
        return false;
    }

    if (!IsValidDate(swpStartdata, swpEnddata)) {
        alert("From Date should be Less than End Date");
        document.getElementById('txtwfrdt').value = "";
        document.getElementById('txtwfrdt').focus();
        return false;
    }

    if (!IsValidDate(swpEnddata, swpAsondata)) {
        alert("Value as on Date should be greater than End Date ");
        document.getElementById('txtwvaldate').value = "";
        document.getElementById('txtwvaldate').focus();
        return false;
    }

    //scheme
    var ddScheme = document.getElementById("ddlscname");
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    if (!IsValidDate(allotdate, swpStartdata)) {
        alert("From Date should be greater than allotment Date" + allotdate);
        document.getElementById('txtwfrdt').value = "";
        document.getElementById('txtwfrdt').focus();
        return false;
    }

    if (initamt / withamt < 6) {
        alert('Initial amount should be 6 time greater than withdrawl amount.');
        document.getElementById('txtwinamt').value = "";
        document.getElementById("txtwinamt").focus();
        return false;

    }

    return true;
}





function validate_STP() {
    // ****************Created By Mukesh *****************

    //alert("hi");   

    if (document.getElementById("ddlschtrf").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlschtrf").focus();
        return false;
    }

    if (document.getElementById("ddlschtrto").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlschtrto").focus();
        return false;
    }

    //    if (document.getElementById("ddbnmark").value == '0') {
    //        alert('Please select any Benchmark.');
    //        document.getElementById("ddbnmark").focus();
    //        return false;
    //    }

    var sipamunt = document.getElementById('txtiniamt').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
    //alert(sipamunt);
    if (sipamunt == "") {
        alert("Value of Initial Amount cannot be Blank");
        document.getElementById('txtiniamt').focus();
        return false;
    }

    if (sipamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value");
        document.getElementById('txtiniamt').value = "";
        document.getElementById('txtiniamt').focus();
        return false;
    }

    if (isNaN(sipamunt)) {
        alert("Please enter Numeric value.");
        document.getElementById('txtiniamt').value = "";
        document.getElementById('txtiniamt').focus();
        return false;
    }
    else {

        if (sipamunt == 0) {
            alert("Please enter Valid Numeric value.");
            document.getElementById('txtiniamt').value = "";
            document.getElementById('txtiniamt').focus();
            return false;
        }
        //        else if (sipamunt >= 1000) {
        //            if (sipamunt % 100 != 0) {
        //                alert('Minimum Initial amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
        //                document.getElementById("txtiniamt").focus();
        //                return false;
        //            }
        //        }
        //        else {
        //            alert('Minimum Initial amount is should be Rs. 1000/- ');
        //            document.getElementById("txtiniamt").focus();
        //            return false;
        //        }
    }

    var sipwdamunt = document.getElementById('txttranamt').value;

    if (sipwdamunt == "") {
        alert("Value of transfer amount cannot be Blank");
        document.getElementById('txttranamt').focus();
        return false;
    }

    if (sipwdamunt.indexOf(".") > -1) {
        alert("Please Enter Integral value in transfer amount ");
        document.getElementById('txttranamt').value = "";
        document.getElementById('txttranamt').focus();
        return false;
    }

    if (isNaN(sipwdamunt)) {
        alert("Please enter Numeric value transfer amount.");
        document.getElementById('txttranamt').value = "";
        document.getElementById('txttranamt').focus();
        return false;
    }
    else {

        if (sipwdamunt == 0) {
            alert("Please enter Valid Numeric value transfer amount.");
            document.getElementById('txttranamt').value = "";
            document.getElementById('txttranamt').focus();
            return false;
        }
    }


    if (parseFloat(document.getElementById("txtiniamt").value) < 12000) {
        alert('Initial amount cannot be less than 12,000/- Rs.');
        document.getElementById('txtiniamt').value = "";
        document.getElementById("txtiniamt").focus();
        return false;
    }

    if (parseFloat(document.getElementById("txttranamt").value) < 1000) {
        alert('Minimum transfer amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
        document.getElementById('txttranamt').value = "";
        document.getElementById("txttranamt").focus();
        return false;
    }

    //    if (parseFloat(document.getElementById("txttranamt").value) > 30000) {
    //        alert('Maximum transfer amount should be Rs.30000');
    //        document.getElementById('txttranamt').value = "";
    //        document.getElementById("txttranamt").focus();
    //        return false;
    //    }


    if (parseFloat(document.getElementById("txttranamt").value) >= 1000) {
        var amt;
        amt = document.getElementById("txttranamt").value;
        if (amt % 100 != 0) {
            alert('Minimum transfer amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
            document.getElementById("txttranamt").focus();
            return false;
        }
    }

    var initamt;
    var withamt;
    initamt = document.getElementById("txtiniamt").value;
    withamt = document.getElementById("txttranamt").value;

    if (initamt / withamt < 1) {
        alert('transfer amount cannot be greater than initial amount.');
        document.getElementById("txttranamt").focus();
        return false;

    }
    if (initamt / withamt < 6) {
        alert('Initial amount should be 6 time greater than Transfer amount.');
        document.getElementById("txtiniamt").focus();
        return false;

    }



    var txtddperiod_val = document.getElementById("txtddperiod").value;
    if (txtddperiod_val == "") {
        alert('Please select any Period.');
        document.getElementById("txtddperiod").focus();
        return false;
    }

    //    if (document.getElementById("txtddperiod").value != 'Monthly' && document.getElementById("txtddperiod").value != 'Quarterly') {
    //        document.getElementById("txtddperiod").value = '';
    //        alert('Please select any Period.');
    //        document.getElementById("txtddperiod").focus();
    //        return false;
    //    }


    var stpdate = document.getElementById("txtddSTPDate").value;
    if (stpdate == "") {
        alert('Please select any STP Date.');
        document.getElementById("txtddSTPDate").focus();
        return false;
    }
    if (stpdate != '1' && stpdate != '7' && stpdate != '14' && stpdate != '21' && stpdate != '28') {
        document.getElementById("txtddSTPDate").value = '';
        alert('Please select valid STP Date.');
        document.getElementById("txtddSTPDate").focus();
        return false;
    }

    if (document.getElementById("txtfrdt").value == '') {
        alert('Please enter From date.');
        document.getElementById("txtfrdt").focus();
        return false;
    }

    if (document.getElementById("txttodt").value == '') {
        alert('Please enter To date.');
        document.getElementById("txttodt").focus();
        return false;
    }

    if (document.getElementById("txtvalue").value == '') {
        alert('Please enter value as on date.');
        document.getElementById("txtvalue").focus();
        return false;
    }


    ///// date part /////
    var sipStartdata = document.getElementById('txtfrdt').value;
    var sipEnddata = document.getElementById('txttodt').value;
    var sipAsondata = document.getElementById('txtvalue').value;


    //alert(sipStartdata);
    var Day = parseInt(sipStartdata.substring(0, 2), 10);
    var Mn = parseInt(sipStartdata.substring(3, 5), 10);
    var Yr = parseInt(sipStartdata.substring(6, 10), 10);
    var DateVal = Mn + "/" + Day + "/" + Yr;

    var dt = new Date(DateVal);


    var todaydate = new Date();
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)

    if (i <= 0) {
        alert("From Date should be Less than Today");
        document.getElementById('txtfrdt').value = "";
        document.getElementById('txtfrdt').focus();
        return false;
    }

    var startsensexdate = new Date("01/01/1980");

    if (dt < startsensexdate) {
        alert("Please select Value as of Date from 1st January 1980");
        document.getElementById('txtfrdt').value = "";
        document.getElementById('txtfrdt').focus();
        return false;
    }

    if (!IsValidDate(sipStartdata, sipEnddata)) {
        alert("From Date should be Less than End Date");
        document.getElementById('txtfrdt').value = "";
        document.getElementById('txtfrdt').focus();
        return false;
    }

    if (!IsValidDate(sipEnddata, sipAsondata)) {
        alert("Value as on Date should be greater than End Date ");
        document.getElementById('txtvalue').value = "";
        document.getElementById('txtvalue').focus();
        return false;
    }

    var ddScheme = document.getElementById("ddlschtrf");
    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
    if (!IsValidDate(allotdate, sipStartdata)) {
        alert("From Date should be greater than allotment Date" + allotdate);
        document.getElementById('txtfrdt').value = "";
        document.getElementById('txtfrdt').focus();
        return false;
    }

    var ddSchemeto = document.getElementById("ddlschtrto");
    allotdate = ddSchemeto.options[ddSchemeto.selectedIndex].title;
    if (!IsValidDate(allotdate, sipStartdata)) {
        alert("From Date should be greater than allotment Date" + allotdate);
        document.getElementById('txtfrdt').value = "";
        document.getElementById('txtfrdt').focus();
        return false;
    }


    return true;
}