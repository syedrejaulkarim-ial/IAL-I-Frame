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




function validate_SIP() {
    // ****************Created By Mukesh *****************

    //alert("hi");   

    if (document.getElementById("ddlscheme").value == '0') {
        alert('Please select any Scheme.');
        document.getElementById("ddlscheme").focus();
        return false;
    }

    //    if (document.getElementById("ddlsipbnmark").value == '0') {
    //        alert('Please select any Benchmark.');
    //        document.getElementById("ddlsipbnmark").focus();
    //        return false;
    //    }

    if (document.getElementById("ddPeriod_SIP").value == '0') {
        alert('Please select Frequency.');
        document.getElementById("ddPeriod_SIP").focus();
        return false;
    }

    var sipamunt = document.getElementById('txtinstall').value; // document.getElementById('<%=txtinstall.ClientID%>').value;
    //alert(sipamunt);
    if (sipamunt == "") {
        alert("Value of Investment cannot be Blank");
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
        else if (sipamunt >= 1000) {
            if (sipamunt % 100 != 0) {
                alert('Minimum SIP amount should be Rs.1000/- and in multiples of Rs.100/- thereafter');
                document.getElementById("txtinstall").focus();
                return false;
            }
        }
        else {
            alert('Minimum  amount is should be Rs. 1000/- ');
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

    var todaydate = new Date();
    //alert(todaydate);alert(dt);
    var i = DateDiff.inDays(dt, todaydate)
    if (i <= 0) {
        alert("From Date should be Less than Today");
        document.getElementById('txtfromDate').value = "";
        document.getElementById('txtfromDate').focus();
        return false;
    }

     Day = parseInt(sipAsondata.substring(0, 2), 10);
     Mn = parseInt(sipAsondata.substring(3, 5), 10);
     Yr = parseInt(sipAsondata.substring(6, 10), 10);
     DateVal = Mn + "/" + Day + "/" + Yr;

     var dt2 = new Date(DateVal);

     var i = DateDiff.inDays(dt2, todaydate)
     if (i <= 0) {
         alert("Value as on Date should be Less than Today");
         document.getElementById('txtvalason').value = "";
         document.getElementById('txtvalason').focus();
         return false;
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

//    alert(document.getElementById("CheckBoxChart").checked);
//    alert(document.getElementById("divshowChart"));
//    if (document.getElementById("CheckBoxChart").checked) {       
//        document.getElementById("divshowChart").style.display = "inline";
//    }
//    else {
//        alert(document.getElementById("divshowChart").style.display);
//        document.getElementById("divshowChart").style.display = "none";
//    }


   // var ddScheme = document.getElementById("ddlscheme");
    //alert(ddScheme.options[ddScheme.selectedIndex].text);
    // alert(ddScheme.options[ddScheme.selectedIndex].value);
    //alert(ddScheme.options[ddScheme.selectedIndex].title);
//    var allotdate = ddScheme.options[ddScheme.selectedIndex].title;
//    //alert(allotdate);
//    if (!IsValidDate(allotdate, sipStartdata)) {
//        alert("From Date should be greater than allotment Date " + allotdate);
//        document.getElementById('txtfromDate').value = "";
//        document.getElementById('txtfromDate').focus();
//        return false;
//    }
    pop();
    return true;
}

