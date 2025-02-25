//Retirement Javascript file

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

function isNumeric(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


function kpvalidateNumber(num) {
    //alert(num.value);
    var chkDecimal = /^-?[1-9][0-9]*(.[0-9]+)?$/.test(num.value);
    if (!chkDecimal) {
        $('#' + num.id.toString()).val('');
        return false;
    }
    return true;
}



function validateNumber(num) {
    //    if (checkplaceholdervalue()) {
    //        return true;
    //    }
    //var chkDecimal = /^-?[1-9][0-9]*(.[0-9]+)?$/.test(num.value);
    // var chkDecimal = /^\+?(0|[1-9]\d*)$/.test(num.value);
    //  var chkDecimal = /^(?=.*[1-9])\d*(?:\.\d{1,2})?$/.test(num.value);

    var chkDecimal = /[0-9 -()+]+$/.test(num.value);
    if (!chkDecimal) {
        //alert("Please Enter Proper value.");
        $('#' + num.id.toString()).val('');
        //$('#' + num.id.toString()).focus();
        //return false;
    }
    //return true;
}

function validateNumberOverloaded(num) {
    //    if (checkplaceholdervalue()) {
    //        return true;
    //    }
    var chkDecimal = /^-?[1-9][0-9]*(.[0-9]+)?$/.test(num.value);
    if (!chkDecimal) {
        // alert("Please Enter Proper value.");
        $('#' + num.id.toString()).val('');
        // $('#' + num.id.toString()).focus();
        return false;
    }
    return true;
}


function validateDecimalLimit(num) {

    var chkDecimal = /^-?[1-9][0-9]*(.[0-9]+)?$/.test(num.value);
    if (!chkDecimal) {       
        $('#' + num.id.toString()).val('');
        // $('#' + num.id.toString()).focus();
        return false;
    }
    var numb = Number(num.value);
    if (numb >= 1000) {
        //alert("More than 1000");
        $('#' + num.id.toString()).val('');
        return false;
    }

    var chkVal = num.value;

    if (chkVal.indexOf(".") != -1) {
        var nsplit = chkVal.split(".");
        var bfrDecmal = Number(nsplit[0]);
        var aftrDecmal = Number(nsplit[1]);
        if (bfrDecmal >= 1000 || aftrDecmal >= 100) {
            $('#' + num.id.toString()).val('');
            return false;
        }
    }

    return true;
}

function checkplaceholdervalue() {
    if ($('#txtUserName').val() == 'Type Your Name Here' || $('#txtAge').val() == 'Type Your Age Here' || $('#txtWishRetireAge').val() == 'Age at Which You Wish to Retire' || $('#txtRatePreRetire').val() == 'Annual rate of return pre retirement (?)' || $('#txtRatePostRetire').val() == 'Annual rate of return post retirement (?)' || $('#txtRateEstInflation').val() == 'Estimated rate of Inflation') {
        return true;
    }
    else {
        return false;
    }
}
function checkAuthentication() {
    Validation();
}

function checkAuthen() {
    if (!Validation())
        $('#divDisplay').attr("display", "none");
    else
        $('#divDisplay').show();   //$('#divDisplay').attr("display", "");
}


function SetErrorMsg(msg) {
    $('#lbspanErrorText').text(msg);
    $('#divError_text').show("slow");
}


function SuppresErrorMsg() {
    $('#lbspanErrorText').text('');
    $('#divError_text').hide("slow");
}


function Validation() {
    //    if ($('#txtUserName').val() == 'Type Your Name Here') {
    //        // alert("asas");
    //        return true;

    //    }
    //ResetAllFinalvalue();
    if ($('#txtUserName').val() == '' || $('#txtUserName').val() == 'Type Your Name Here') {
        //alert("Please Enter UserName");
        SetErrorMsg("Please Enter Your Name");
        return false;
    }

    //alert(jQuery.trim($('#txtUserName').val()).length);

    if ($('#txtAge').val() == '' || $('#txtAge').val() == 'Type Your Age Here') { //!CheckNull(CurrentAge) &&
        //alert("Please Enter User Current Age");
        SetErrorMsg("Please Enter Your Current Age");
        return false;
    }

    //    if ($('#txtUserEmail').val() == '') {// !CheckNull(UserEmailID) &&
    //        //alert("Please Enter User  Email ID "); 
    //        SetErrorMsg("Please Enter User  Email ID");
    //        return false;
    //    }

    if (!CheckEmail($('#txtUserEmail').val())) {
        return false;
    }

    if ($('#txtWishRetireAge').val() == '' || $('#txtWishRetireAge').val() == 'Age at Which You Wish to Retire') { //!CheckNull(ExpRetireAge) && 
        //alert("Please Enter ExpRetireAge"); 
        SetErrorMsg("Please Enter Expected Retirement Age");
        return false;
    }


    if ($('#txtRatePreRetire').val() == '' || $('#txtRatePreRetire').val() == 'Annual rate of return pre retirement (?)') {//!CheckNull(ReturnRatePreRetire) &&
        //alert("Please Enter Return Rate Pre Retire");
        SetErrorMsg("Please Enter Return Rate for Pre Retirement");
        return false;
    }

    if ($('#txtRatePostRetire').val() == '' || $('#txtRatePostRetire').val() == 'Annual rate of return post retirement (?)') {//!CheckNull(ReturnRatePostRetire) &&
        //alert("Please Enter User Return Rate Post Retire"); 
        SetErrorMsg("Please Enter Return Rate for Post Retirement");
        return false;
    }

    if ($('#txtRateEstInflation').val() == '' || $('#txtRateEstInflation').val() == 'Estimated rate of Inflation') {// !CheckNull(RateEstInflation) &&
        //alert("Please Enter Rate Est Inflation"); 
        SetErrorMsg("Please Enter Rate for Estimated Inflation");
        return false;
    }


    if (Number(AgeDiff(Number($('#txtWishRetireAge').val()), Number($('#txtAge').val())) <= 0)) {
        SetErrorMsg("Expected Retirement Age should greater than Current Age");
        return false;
    }

    SuppresErrorMsg();
    return true;

}

function CheckEmail(mailId) {

    if (mailId == "") {
        SetErrorMsg("Please Enter Your Email ID");
        // alert('Please Enter Email Id.');
        // $("#txtUserEmail").focus();
        return false;
    }

    var pattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    if (!pattern.test(mailId)) {
        //alert("Please Enter Valid Email Address"); $("#txtUserEmail").focus(); 
        SetErrorMsg("Please Enter Proper Email ID");
        return false;
    }
    else
        return true;

}


function ContainsLetter(uname) {
    var letters = /^[A-Za-z ]+$/;
    if (uname.value.match(letters)) {
        if (uname.value.length < 3) {
            //alert('Name Conatins atleast 5 Character'); uname.focus();
           // SetErrorMsg("Your Name Contains atleast 3 Character");
            return false;
        }
        else {
            SuppresErrorMsg();
            return true;
        }
    }
    else {
        //alert('Name must have alphabet characters only');
      //  SetErrorMsg("Name must have alphabet characters only");
        uname.value = "";
        // uname.focus();
        return false;
    }
}

function ContainsLetterWithMessage(uname) {
    var letters = /^[A-Za-z ]+$/;
    if (uname.value.match(letters)) {
        if (uname.value.length < 3) {
            alert('Name Conatins atleast 3 Character'); uname.focus();
            //SetErrorMsg("User Name Contains atleast 3 Character");
            return false;
        }
        else {
            SuppresErrorMsg();
            return true;
        }
    }
    else {
        alert('Name must have alphabet characters only');
        //SetErrorMsg("Name must have alphabet characters only");
        uname.value = "";
        // uname.focus();
        return false;
    }
}


function AgeDiff(agefirst, ageSecond) {
    return (agefirst - ageSecond);
}


function CheckNull(obj) {
    if (obj == "") {
        return false;
    }
    else
        return true;
}


function CalculateCompundInterest(p, r, y, n) {
    var ret;
    ret = Math.pow((1 + (r / (100 * n))), n * y);
    ret = p * ret;
    return Math.round(ret);
}

function CalculateSimpleInterest(p, r, t) {
    var ret;
    ret = (p * r * t) / 100;
    ret = p + ret;
    return Math.round(ret);
}









function setvalue() {
    // alert(1);
    Username = $('#txtUserName').val();
    CurrentAge = $('#txtAge').val();
    ExpRetireAge = $('#txtWishRetireAge').val();
    UserEmailID = $('#txtUserEmail').val();
    ReturnRatePreRetire = $('#txtRatePreRetire').val();
    ReturnRatePostRetire = $('#txtRatePostRetire').val();
    RateEstInflation = $('#txtRateEstInflation').val();
    AgeDifference = AgeDiff(ExpRetireAge, CurrentAge);
    // alert("value are initialze");
}

//button 1
function FetchRetiremeentMonthly() {

    if (!Validation())
        return false;
    setvalue();
    var CurrentMonthlyExp = $('#txtcurrentMonthlyExp').val();

    if (CurrentMonthlyExp == '' || CurrentMonthlyExp == 'What are your monthly expenses currently?') {
        $('#lbtxtExpectedMonthlyExpRetire').text('');
        return false;
    }

    var ExpectedMonthlyExp = CalculateCompundInterest(CurrentMonthlyExp, RateEstInflation, AgeDifference, 1);
    //alert(ExpectedMonthlyExp);
    //$('#txtExpectedMonthlyExpRetire').val(ExpectedMonthlyExp);
    $('#HidlbtxtExpectedMonthlyExpRetire').val(ExpectedMonthlyExp);
    $('#lbtxtExpectedMonthlyExpRetire').text(ExpectedMonthlyExp);
    //add by syed
    if ($('#txtMonthlyIncomePostRetire').val() == '' || $('#txtMonthlyIncomePostRetire').text() == '') {
        $('#txtMonthlyIncomePostRetire').val(ExpectedMonthlyExp);
        //$('#txtMonthlyIncomePostRetire').text(ExpectedMonthlyExp);
    }
}




function GetMonthlyReturn(Amount, InterestRate) {
    var returnAmount = 0;
    returnAmount = (Number(Amount) * Number(InterestRate)) / (100 * 12);
    return returnAmount;
}

function GetTotalAmountMOnthly(Amount, Interest) {
    var IntrestAmount = GetMonthlyReturn(Amount, Interest);
    return Number(Amount) + Number(IntrestAmount);
}

function CheckBoxFunc(chkBxid, divId) {

    $('#' + chkBxid.toString()).change(function () {
        if (this.checked)
            $('#' + divId.toString()).fadeIn('slow');
        else
            $('#' + divId.toString()).fadeOut('slow');
    });
}


function HideDiv(divId) {
    $('#' + divId.toString()).fadeOut('slow');
}

function ShowHideDiv() {

    SuppresErrorMsg();

    HideDiv("divTopUp");
    HideDiv("divMainAddedSip");
    HideDiv("divMainAddedLS");

    CheckBoxFunc("cbMainSipS", "divSipMain");
    CheckBoxFunc("cbMainLsS", "divLsMain");
    CheckBoxFunc("cbTopUpS", "divTopUp");
    CheckBoxFunc("cbAddSipS", "divMainAddedSip");
    CheckBoxFunc("cbAddLsS", "divMainAddedLS");
}


//#region call WebMethod by Ajax json
//button 2
function RetiremeentCorpusLast() {

    if (!Validation())
        return false;
    setvalue();

    var EstimatedRetirementCorpus = $('#txtEstimatedRetirementCorpus').val();
    var MonthlyIncomePostRetire = $('#txtMonthlyIncomePostRetire').val();
    var MonthlyIncrementPostRetire = $('#txtMonthlyIncrementPostRetire').val();

    if (EstimatedRetirementCorpus == '' || EstimatedRetirementCorpus == 'What is your estimated retirement corpus?' || MonthlyIncomePostRetire == 'What monthly income would you like to get post retirement (from the retirement corpus)?' || MonthlyIncomePostRetire == 'Would you want an increase in monthly income every year post retirement? If yes, how much?' || !Validation()) {
        $('#lbtxtRetireCorpusLast').text('');
        return false;
    }


    if (MonthlyIncomePostRetire == '' || !Validation()) {
        $('#lbtxtRetireCorpusLast').text('');
        return false;
    }



    var YearsLast = 0;
    if (MonthlyIncrementPostRetire == '' || (!(/^-?[1-9][0-9]*(.[0-9]+)?$/.test(MonthlyIncrementPostRetire))) || MonthlyIncrementPostRetire == 'Would you want an increase in monthly income every year post retirement? If yes, how much?')
        MonthlyIncrementPostRetire = 0;


    //    if (!validateNumberOverloaded(MonthlyIncrementPostRetire))
    //        MonthlyIncrementPostRetire = 0;
    //    YearsLast = RetirementCorpusLeft(EstimatedRetirementCorpus, MonthlyIncomePostRetire, RateEstInflation, ReturnRatePostRetire, MonthlyIncrementPostRetire);
    //    alert(YearsLast);


    var val = "{'Corpus':'" + EstimatedRetirementCorpus + "', MonthlyExpense:'" + MonthlyIncomePostRetire + "',  InfRate:'" + RateEstInflation + "', InterestRate:'" + ReturnRatePostRetire + "',ExpenseRate:'" + MonthlyIncrementPostRetire + "' }";


    $.ajax({
        type: "POST",
        url: "WebMethod.aspx/GetRequiredRetirementCorpus",
        async: false,
        contentType: "application/json",
        data: val,
        dataType: "json",
        success: function (msg) {
            YearsLast = msg.d;
        },
        error: function (msg) {
            alert("Error! Try again...");
        }
    });

    // alert(YearsLast);
    //$('#txtRetireCorpusLast').val(YearsLast);


    $('#HidlbtxtRetireCorpusLast').val(YearsLast);
    $('#lbtxtRetireCorpusLast').text(YearsLast);
    //Add by syed
    if ($('#txtExpectedMonthlyIncomePostRetire').val() == '' || $('#txtExpectedMonthlyIncomePostRetire').text() == '') {
        $('#txtExpectedMonthlyIncomePostRetire').val($('#txtMonthlyIncomePostRetire').val());
        //$('#txtExpectedMonthlyIncomePostRetire').text($('#txtMonthlyIncomePostRetire').val());
    }
    if ($('#txtMonthlyIncrementPostRetire4Stage3').val() == '' || $('#txtMonthlyIncrementPostRetire4Stage3').text() == '') {
        $('#txtMonthlyIncrementPostRetire4Stage3').val($('#txtMonthlyIncrementPostRetire').val());
        //$('#txtMonthlyIncrementPostRetire4Stage3').text($('#txtMonthlyIncrementPostRetire').val());
    }
}

//button 3
function GetRequiredRetirementCorpus() {
    if (!Validation())
        return false;
    var ExpectedMonthlyIncomePostRetire = $('#txtExpectedMonthlyIncomePostRetire').val();
    var ExpectdRetireCorpusLast = $('#txtExpectdRetireCorpusLast').val();
    //changed by syed
    //var MonthlyIncrementPostRetire = $('#txtMonthlyIncrementPostRetire').val();
    var MonthlyIncrementPostRetire = $('#txtMonthlyIncrementPostRetire4Stage3').val();
    if (ExpectedMonthlyIncomePostRetire == '' || ExpectedMonthlyIncomePostRetire == 'What monthly income would you like to get post retirement (from the retirement corpus)?' || ExpectdRetireCorpusLast == '' || ExpectdRetireCorpusLast == 'How long do you wish for your retirement corpus to last? (post retirement)' || !Validation()) {
        $('#lbtxtPlannedRetireCorpus').text('');
        return false;
    }

    setvalue();
    var ReqAmount = 0;
    if (MonthlyIncrementPostRetire == '' || MonthlyIncrementPostRetire == 'Would you want an increase in monthly income every year post retirement? If yes, how much?')
        MonthlyIncrementPostRetire = 0;

    //Currentlty fixing it to 0
    //commented by syed
    //MonthlyIncrementPostRetire = 0;

    //    ReqAmount = RetirementCorpusAmount(ExpectdRetireCorpusLast, ExpectedMonthlyIncomePostRetire, RateEstInflation, ReturnRatePostRetire, MonthlyIncrementPostRetire);
    //    alert(ReqAmount);

    var val = "{'Year':'" + ExpectdRetireCorpusLast + "', MonthlyExpense:'" + ExpectedMonthlyIncomePostRetire + "',  InfRate:'" + RateEstInflation + "', InterestRate:'" + ReturnRatePostRetire + "',ExpenseRate:'" + MonthlyIncrementPostRetire + "' }";


    $.ajax({
        type: "POST",
        url: "WebMethod.aspx/RetirementCorpusAmount",
        async: false,
        contentType: "application/json",
        data: val,
        dataType: "json",
        success: function (msg) {
            ReqAmount = msg.d;
        },
        error: function (msg) {
            alert("Error! Try again...");
        }
    });

    //    alert(ReqAmount);
    //    $('#txtPlannedRetireCorpus').val(ReqAmount);
    $('#HidlbtxtPlannedRetireCorpus').val(ReqAmount);
    $('#lbtxtPlannedRetireCorpus').text(ReqAmount);

    //Add by syed
    if ($('#txtEstRetCorp').val() == '' || $('#txtEstRetCorp').text() == '') {
        $('#txtEstRetCorp').val($('#lbtxtPlannedRetireCorpus').text());
        //$('#txtEstRetCorp').text($('#lbtxtPlannedRetireCorpus').text());
    }
}

function CalculateTimeRetrCorps() {
    if (!Validation())
        return false;

    if ($('#txtEstRetCorp').val() == '' || $('#txtEstRetCorp').val() == 'What is your estimated retirement corpus?') {
        // alert('Please enter Amount.');
        // $('#txtEstRetCorp').focus();
        return false;
    }

    if ($('#txtRetDate').val() == '') {
        // alert('Please enter Start date.');
        // $('#txtRetDate').focus();
        return false;
    }

    var EstRetCorpus = $('#txtEstRetCorp').val();
    var startDate = $('#txtRetDate').val();
    var IntrestRate = $('#txtRatePreRetire').val();

    var SIPMonthly = ($('#txtMonthlySipAmount').val() == '' || $('#txtMonthlySipAmount').val() == 'Monthly Investment amount') ? "0" : $('#txtMonthlySipAmount').val();
    var yearSIP = ($('#txtSIPYearly').val() == '' || $('#txtSIPYearly').val() == 'How Many Yrs') ? "100" : $('#txtSIPYearly').val();
    var LsInvestMain = ($('#txtLsInvestAmount').val() == '' || $('#txtLsInvestAmount').val() == 'Investment Amount') ? "0" : $('#txtLsInvestAmount').val();

    EstRetCorpus = Number(EstRetCorpus);
    IntrestRate = Number(IntrestRate);
    SIPMonthly = Number(SIPMonthly);
    yearSIP = Number(yearSIP);
    LsInvestMain = Number(LsInvestMain);

    // check which one is selected

    if (!$('#cbMainSipS').is(':checked')) {
        SIPMonthly = 0;
        yearSIP = 0;
    }
    if (!$('#cbMainLsS').is(':checked')) {
        LsInvestMain = 0;
    }

    //alert($("input[id$='cbMainLs']").attr("checked", true));
    //alert($("input[id='cbMainSipS']:checked"));

    // alert($('#cbMainSipS').is(':checked'));

    if (!$('#cbMainLsS').is(':checked') && !$('#cbMainSipS').is(':checked'))
        return false;
    //    else {
    //        if ($('#txtMonthlySipAmount').val() == '' || $('#txtLsInvestAmount').val() == '' || $('#txtEstRetCorp').val() == '')
    //            return false;
    //    }

    var vMainSipMonthly = {};
    vMainSipMonthly.SipAmount = SIPMonthly;
    vMainSipMonthly.TotalYear = yearSIP;

    var Json_Add_Sip_List = new Array();
    var Json_Add_Ls_List = new Array();


    //alert($AddSipArray.length);
    //alert($AddLsArray.length);

    var TrackSipAdded = "";
    var TrackLsAdded = "";

    if ($('#cbTopUpS').is(':checked')) {
        if ($('#cbAddSipS').is(':checked')) {
            for (i = 0; i < $AddSipArray.length; i++) {
                var Json_Item = new Object();
                Json_Item.SipAmount = ($('#ntxtSip' + $AddSipArray[i].toString()).val() == '') ? "0" : $('#ntxtSip' + $AddSipArray[i].toString()).val();
                Json_Item.AfterYear = ($('#ntxtAfterYear' + $AddSipArray[i].toString()).val() == '') ? "0" : $('#ntxtAfterYear' + $AddSipArray[i].toString()).val();
                Json_Item.TotalYear = ($('#ntxtYearlast' + $AddSipArray[i].toString()).val() == '') ? "1000" : $('#ntxtYearlast' + $AddSipArray[i].toString()).val();
                Json_Add_Sip_List.push(Json_Item);
                TrackSipAdded += $AddSipArray[i].toString() + '|' + Json_Item.AfterYear.toString() + '|' + Json_Item.SipAmount.toString() + '|' + Json_Item.TotalYear.toString() + '#';
            }
        }
        if ($('#cbAddLsS').is(':checked')) {
            for (i = 0; i < $AddLsArray.length; i++) {
                var Json_Item = new Object();
                Json_Item.InvestmentAmount = ($('#ntxtLsInvstmnt' + $AddLsArray[i].toString()).val() == '') ? "0" : $('#ntxtLsInvstmnt' + $AddLsArray[i].toString()).val();
                Json_Item.AfterYear = ($('#ntxtAfterYearLs' + $AddLsArray[i].toString()).val() == '') ? "0" : $('#ntxtAfterYearLs' + $AddLsArray[i].toString()).val();
                Json_Add_Ls_List.push(Json_Item);
                TrackLsAdded += $AddLsArray[i].toString() + '|' + Json_Item.AfterYear.toString() + '|' + Json_Item.InvestmentAmount.toString() + '#';
            }
        }
    }


    // alert(TrackSipAdded + '-sip- ls' + TrackLsAdded);
    $('#HidAddSipRec').val(TrackSipAdded);
    $('#HidAddLsRec').val(TrackLsAdded);

    // startDate = Date.parse(startDate).toString("dd MMM yyyy");

    var jstartDate = getJsDate(startDate);

    //    jstartDate = Date.parse(jstartDate).toString("dd MMMM yyyy");
    //    var ddate = new Date(parseInt(jstartDate.substr(6)));
    //    alert(jstartDate + '' + ddate);


    var val = "{'ExpectedCorpus':'" + EstRetCorpus + "', IntrestRate:'" + IntrestRate + "',  startDate:'" +
     jstartDate + "', objMainSipMonthly:{ 'SipAmount':'" + SIPMonthly + "', 'TotalYear':'" +
      yearSIP + "'}, objMainLsInvestment : {'InvestmentAmount':'" + LsInvestMain + "'},'objAddedSip':'" +
       JSON.stringify(Json_Add_Sip_List) + "' ,'objAddedLs':'" + JSON.stringify(Json_Add_Ls_List) + "' }";

    $.ajax({
        type: "POST",
        url: "WebMethod.aspx/getExpectdMonthRetCorp",
        async: false,
        contentType: "application/json",
        data: val,
        dataType: "json",
        success: function (msg) {
            setResult(msg.d);
        },
        error: function (msg) {
            alert("Error! Try again...");
        }
    });

}


function getJsDate(SelectedDate) {
    var Day2 = parseInt(SelectedDate.substring(0, 2), 10);
    var Mn2 = parseInt(SelectedDate.substring(3, 5), 10);
    var Yr2 = parseInt(SelectedDate.substring(6, 10), 10);
    var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;
    var dateSelected = new Date(DateVal2).toString('dd/MM/yyyy');
    //alert(dateSelected);
    return dateSelected;
}

function setResult(ValueReturn) {
    var TotalMonth = ValueReturn.split('#');

    var month = new Array();
    month[0] = "January";
    month[1] = "February";
    month[2] = "March";
    month[3] = "April";
    month[4] = "May";
    month[5] = "June";
    month[6] = "July";
    month[7] = "August";
    month[8] = "September";
    month[9] = "October";
    month[10] = "November";
    month[11] = "December";
    //var n = month[d.getMonth()];

    var endTime = TotalMonth[1];

    var Day2 = parseInt(endTime.substring(0, 2), 10);
    var Mn2 = parseInt(endTime.substring(3, 5), 10);
    var Yr2 = parseInt(endTime.substring(6, 10), 10);
    var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;
    var enddateSelected = new Date(DateVal2);


    var strddate = month[enddateSelected.getMonth()].toString() + ' ' + enddateSelected.getFullYear().toString();

    $('#HidlbTotalTimeReqRetire').val(TotalMonth[0]);
    $('#HidlbExpTimeReqRetire').val(strddate);

    $('#lbTotalTimeReqRetire').text(TotalMonth[0]);
    $('#lbExpTimeReqRetire').text(strddate);



    var totalyearTime = TotalMonth[0].split('Years')[0];
    totalyearTime = Number($.trim(totalyearTime));


    //alert(totalyearTime);
    CurrentAge = $('#txtAge').val();
    ExpRetireAge = $('#txtWishRetireAge').val();

    var SelectedDate = $('#txtRetDate').val();
    var currentDate = new Date().toString('dd/MM/yyyy');

    var yearDifffromCurrent = 0;

    if (SelectedDate != '') {
        var dtTo = getJsDate(SelectedDate);
        //  yearDifffromCurrent = DateDiff.inYears(currentDate, dtTo);
        yearDifffromCurrent = AgeDiff(Number(currentDate.split('/')[2]), Number(dtTo.split('/')[2]));
    }

    totalyearTime = totalyearTime + Number(yearDifffromCurrent);




    AgeDifference = AgeDiff(ExpRetireAge, CurrentAge);

    var retireDate = new Date();
    retireDate.setFullYear(retireDate.getFullYear() + Number(AgeDifference));

    // alert(retireDate.toString('dd/MM/yyyy') + '#' + TotalMonth[1]);

    var d = DateDifference(TotalMonth[1], retireDate.toString('dd/MM/yyyy'))
    //alert(d);

    // var lastMonthTime = Number(TotalMonth[0].split('Years')[1].trim().split('Months')[0].split(' ')[1].trim());

    // if (Number(totalyearTime) < Number(AgeDifference)) {
    if (Number(d) >= 0) {

//        $('#divOutputbluetext').show("slow");
        //        $('#divOutputredtext').hide("slow");
        $('#res4').attr("class", "result_txt");
        // $('#txtSpanRes').val("Congratulations, you will achieve your retirement corpus before your retirement age!");
        $('#txtSpanRes').html("Congratulations, you will achieve your retirement corpus before your retirement age!");
    }
    else {
//        $('#divOutputbluetext').hide("slow");
        //        $('#divOutputredtext').show("slow");
        $('#res4').attr("class", "result_txt_brown");
        //$('#txtSpanRes').val("Sorry, You will need to re-look at your retirement plan, because you will not be able to achieve your retirement corpus before your retirement age!");
        $('#txtSpanRes').html("Sorry, You will need to re-look at your retirement plan, because you will not be able to achieve your retirement corpus before your retirement age!");

    }

}


function DateDifference(Fdate, Sdate) {
    var Day1 = parseInt(Fdate.substring(0, 2), 10);
    var Mn1 = parseInt(Fdate.substring(3, 5), 10);
    var Yr1 = parseInt(Fdate.substring(6, 10), 10);
    var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;
    var fDat = new Date(DateVal1);

    var Day2 = parseInt(Sdate.substring(0, 2), 10);
    var Mn2 = parseInt(Sdate.substring(3, 5), 10);
    var Yr2 = parseInt(Sdate.substring(6, 10), 10);
    var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;
    var SDat = new Date(DateVal2);

    var DayDiff = DateDiff.inDays(fDat, SDat);
    return DayDiff;
}

//#endregion  

//#region Add Remove Line

function nAddSipLine() {
    if ($TotalSipcounter < 3) {
        $('#tblMainAddedSip').append('<tr id="trdivSipAdd' + $counter + '"><td colspan="4"><div id="divSipAdd' + $counter + '"><table width="100%" border="0" align="left" cellpadding="0" cellspacing="0"><tr><td style="padding-top: 5px; width: 180px; border-left: 1px solid #dddddd;" class="rupee">'
        + 'Rs. <input name="" type="text" id="ntxtSip' + $counter + '" onkeypress="return isNumeric(event)" class="row_txtbx" maxlength="10" runat="server" style="width: 170px; height: 15px;" />' +
        ' </td><td style="padding-top: 5px; width: 100px;"><input name="" type="text" id="ntxtAfterYear' + $counter + '" maxlength="2" onkeypress="return isNumeric(event)" runat="server" style="width: 100px;height: 15px;" /></td><td style="padding-top: 5px; width: 100px;"> ' +
        '<input name="" type="text" id="ntxtYearlast' + $counter + '" runat="server" maxlength="2" onkeypress="return isNumeric(event)" style="width: 100px;height: 15px;" /></td><td style="padding-top: 5px; width: 70px;"><div class="span01">'
        + '<input id="nbuttonAddSip' + $counter + '" name="" onclick="nAddSipLine(this.id)" type="button" style="background: url(images/add.png) no-repeat;height: 16px; width: 16px;" /></div><div class="span01">' +
        '<input id="nbuttonRemoveSip' + $counter + '" onclick="nRemoveSipLine(this.id)" name="" type="button" style="background: url(images/remove.png) no-repeat;height: 16px; width: 16px;" /></div></td></tr></table></div></td></tr>'
         );
        AddinArrayfunc($AddSipArray, $counter);
        $counter++;
        $TotalSipcounter++;
        $('.row_txtbx').number(true, 0);
        // $('#buttonRemoveSip' + $counter.toString()).setAttribute("onclick", "RemoveSipLine");

    } else {
        alert('You cannot add more than 3 Sip investment');
    }
}


function nRemoveSipLine(counter) {
    counter = counter.split('Sip')[1].toString();
    //alert("Remove" + counter);
    if (confirm("Are you sure want to delete this row")) {
        $("#trdivSipAdd" + counter).remove();
        $AddSipArray = RemoveinArrayfunc($AddSipArray, counter);
        $TotalSipcounter--;
    }
}


function nAddLsLine() {
    if ($TotalLscounter < 3) {

        $('#tblMainAddedLS').append('<tr id="trdivLsAdd' + $Lscounter + '"><td colspan="3"><div id="divLsAdd' + $Lscounter + '"><table width="100%" border="0" align="left" cellpadding="0" cellspacing="0"><tr><td style="padding-top: 5px; width: 200px; border-left: 1px solid #dddddd;" class="rupee">' +
                    'Rs. <input name="" id="ntxtLsInvstmnt' + $Lscounter + '" onkeypress="return isNumeric(event)" class="row_txtbx" maxlength="10" runat="server" type="text" style="width: 170px; height: 15px;" /></td><td style="padding-top: 5px; width: 180px;"><input name=""id="ntxtAfterYearLs' + $Lscounter + '" onkeypress="return isNumeric(event)" maxlength="2" type="text" style="width: 170px; height: 15px;" /></td>' +
                    '<td style="padding-top: 5px; width: 70px;"><div class="span01"><input id="nbuttonAddLs' + $Lscounter + '" onclick="nAddLsLine(this.id)" name="" type="button" style="background: url(images/add.png) no-repeat;height: 16px; width: 16px;" />' +
                    '</div><div class="span01"><input id="nbuttonRemoveLs' + $Lscounter + '" onclick="nRemoveLsLine(this.id)" name="" type="button"style="background: url(images/remove.png) no-repeat; height: 16px; width: 16px;" /></div></td></tr></table></div></td></tr>');


        AddinArrayfunc($AddLsArray, $Lscounter);
        $Lscounter++;
        $TotalLscounter++;
        $('.row_txtbx').number(true, 0);
        //$('#buttonRemoveLs' + $Lscounter.toString()).setAttribute("onclick", "RemoveLsLine");
    } else {
        alert('You cannot add more than 3 Lump sum investment');
    }
}


function nRemoveLsLine(counter) {
    counter = counter.split('Ls')[1].toString();
    //alert("Remove" + counter);
    if (confirm("Are you sure want to delete this row")) {
        $("#trdivLsAdd" + counter).remove();
        $AddLsArray = RemoveinArrayfunc($AddLsArray, counter);
        $TotalLscounter--;
    }
}



//#endregion 


//#region Common Method

function ResetAllFinalvalue() {
    //<summary> Main Function
    //<para>
    //This Method will Reset all sections result Output on Page</para>
    //</summary>
    //alert('Resetting value');
    $('#lbtxtExpectedMonthlyExpRetire').text(''); $('#lbtxtRetireCorpusLast').text('');
    $('#lbtxtPlannedRetireCorpus').text(''); $('#lbTotalTimeReqRetire').text(''); $('#lbExpTimeReqRetire').text('');

}



function AllFilled() {

    if ($('#lbtxtExpectedMonthlyExpRetire').text() == '' || $('#lbtxtRetireCorpusLast').text() == '' ||
    $('#lbtxtPlannedRetireCorpus').text() == '' || $('#lbTotalTimeReqRetire').text() == ''
    || $('#lbExpTimeReqRetire').text() == '')
        return false;
    else
        true;
}

function CallAllMethod() {
    //    if (checkplaceholdervalue()) {
    //        return true;
    //    }
    //alert("test");
    //<summary> Main Function
    //<para>
    //This Method call all  Output section result method on Page</para>
    //</summary>
    if (!Validation())
        return false;
    // alert('Start Calculate');
    setvalue();
    FetchRetiremeentMonthly();
    RetiremeentCorpusLast();
    GetRequiredRetirementCorpus();
    CalculateTimeRetrCorps();

    if (CheckOutputFieldStatus()) {
        $('#btnDownload').show();
        $('#spanbtnDownload').show();
        $('#lblPdfReporttext').attr("style", "display:none");
    }
    else {
        $('#btnDownload').attr("display", "none");
        $('#spanbtnDownload').attr("display", "none");

        $('#lblPdfReporttext').show();
    }


    ShowFilledBox();


    $('.txtLeft').number(true, 0);
    //$('.txtLeft').number(true, 0);

}

function ShowFilledBox() {
    if ($('#lbtxtExpectedMonthlyExpRetire').text() != '' && $('#lbtxtExpectedMonthlyExpRetire').text() != '0') {
        $('#res1').show();
        $('#btnNext1').show();
        $('#Td1').attr("style", "background-image: url('../Retirement/images/stp1a.jpg')");
        $('#lb1').attr("class", "stp_txt1");
    }
    else {
        $('#Td1').attr("style", "background-image: url('../Retirement/images/stp1.jpg')");
        $('#res1').attr("style", "display:none");
        $('#btnNext1').attr("style", "display:none");
        $('#lb1').attr("class", "stp_txt");
        $('#lbtxtExpectedMonthlyExpRetire').text() == '';
    }


    if ($('#lbtxtRetireCorpusLast').text() != '') {
        $('#res2').show();
        $('#btnNext2').show();
        $('#Td2').attr("style", "background-image: url('../Retirement/images/stp1a.jpg')");
        $('#lb2').attr("class", "stp_txt1");
        if ($('#lbtxtExpectedMonthlyExpRetire').text() != '' && $('#lbtxtExpectedMonthlyExpRetire').text() != '0')
            $('#Td1').attr("style", "background-image: url('../Retirement/images/stp1b.jpg')");
        else
            $('#Td1').attr("style", "background-image: url('../Retirement/images/stp1c.jpg')");
    }
    else {
        $('#Td2').attr("style", "background-image: url('../Retirement/images/stp1.jpg')");
        $('#res2').attr("style", "display:none");
        $('#btnNext2').attr("style", "display:none");
        $('#lb2').attr("class", "stp_txt");
    }

    if ($('#lbtxtPlannedRetireCorpus').text() != '' && $('#lbtxtPlannedRetireCorpus').text() != '0') {
        $('#res3').show();
        $('#btnNext3').show();
        $('#Td3').attr("style", "background-image: url('../Retirement/images/stp1a.jpg')");
        $('#lb3').attr("class", "stp_txt1");
        if ($('#lbtxtRetireCorpusLast').text() != '')
            $('#Td2').attr("style", "background-image: url('../Retirement/images/stp1b.jpg')");
        else
            $('#Td2').attr("style", "background-image: url('../Retirement/images/stp1c.jpg')");
    }
    else {
        $('#Td3').attr("style", "background-image: url('../Retirement/images/stp1.jpg')");
        $('#res3').attr("style", "display:none");
        $('#btnNext3').attr("style", "display:none");
        $('#lbtxtPlannedRetireCorpus').text() == '';
        $('#lb3').attr("class", "stp_txt");
    }

    if ($('#lbTotalTimeReqRetire').text() != '') {
        $('#res4').show();
        $('#lb4').attr("class", "stp_txt1");
        $('#Td4').attr("style", "background-image: url('../Retirement/images/stp4.jpg')");

        if ($('#lbtxtPlannedRetireCorpus').text() != '' && $('#lbtxtPlannedRetireCorpus').text() != '0')
            $('#Td3').attr("style", "background-image: url('../Retirement/images/stp1b.jpg')");
        else
            $('#Td3').attr("style", "background-image: url('../Retirement/images/stp1c.jpg')");
    }
    else {
        $('#Td4').attr("style", "background-image: url('../Retirement/images/stp4a.jpg')");
        $('#res4').attr("style", "display:none");
        $('#lb4').attr("class", "stp_txt");
    }
}


function CheckOutputFieldStatus() {

    if ($('#lbtxtExpectedMonthlyExpRetire').text() == '' || $('#lbtxtRetireCorpusLast').text() == '' || $('#lbtxtPlannedRetireCorpus').text() == ''
     || $('#lbTotalTimeReqRetire').text() == '' || $('#lbExpTimeReqRetire').text() == '') {
        // alert("Please fill all section before generating the Report.");
        return false;
    }
    else
        return true;
}


function CheckValdPDFGenerate() {
    //<summary> Generate Pdf Function
    //<para>
    //This Method Generate Pdf for all result method on Page</para>
    //</summary>
    if (!CheckOutputFieldStatus()) {
        return false;
    }
    else {
        // alert("Pdf is getting Loaded");
        ShowPdfSection();
        return true;
    }



}

function CalculateTimeRetrCorpsPage() {

    if ($('#txtEstRetCorp').val() == '' || $('#txtEstRetCorp').val() == 'What is your estimated retirement corpus?') {
        alert('Please enter amount.');
        // $('#txtEstRetCorp').focus();
        return false;
    }

    if ($('#txtRetDate').val() == '') {
        alert('Please enter start date.');
        // $('#txtRetDate').focus();
        return false;
    }

    if (!$('#cbMainLsS').is(':checked') && !$('#cbMainSipS').is(':checked')) {
        alert("Please specify investment mode");
        return false;
    }
    else {
        if ($('#txtMonthlySipAmount').val() == '' || $('#txtMonthlySipAmount').val() == 'Monthly Investment amount' || $('#txtLsInvestAmount').val() == '' || $('#txtLsInvestAmount').val() == 'Investment Amount' || $('#txtEstRetCorp').val() == '' || $('#txtEstRetCorp').val() == 'What is your estimated retirement corpus?') {
            if (($('#txtMonthlySipAmount').val() == '' || $('#txtMonthlySipAmount').val() == 'Monthly Investment amount') && $('#cbMainSipS').is(':checked')) {
                alert("Please enter monthly SIP installment amount ");
                return false;
            }
            if (($('#txtLsInvestAmount').val() == '' || $('#txtLsInvestAmount').val() == 'Investment Amount') && $('#cbMainLsS').is(':checked')) {
                alert("Please enter your one time investment amount");
                return false;
            }
            if ($('#txtEstRetCorp').val() == '' || $('#txtEstRetCorp').val() == 'What is your estimated retirement corpus?') {
                alert("Please specify investment mode");
                return false;
            }
        }
    }

    //    CalculateTimeRetrCorps();

    //    if (CheckOutputFieldStatus()) {
    //        $('#btnDownload').show();
    //        $('#lblPdfReporttext').attr("style", "display:none");
    //    }
    //    else {
    //        $('#btnDownload').attr("display", "none");
    //        $('#lblPdfReporttext').show();
    //    }

    CallAllMethod()
    return true;
}


//#endregion 


//#region 
function setLabelValue() {
    /// <summary>This Function will set Label values from Hidden field as it are not reflected when changed by jquery.So We have to set those value in Hidden field
    /// <param name="name" type="String">Description</param>
    /// <param name="name" type="String">Description</param>
    //</summary>




}

//#endregion 

function myfunction() {
    /// <summary>Description
    /// <param name="name" type="String">Description</param>
    /// <param name="name" type="String">Description</param>
    //</summary>

}


function RemoveinArrayfunc(filters, removeItem) {
    /// <summary>Description
    /// <param name="name" type="String">Description</param>
    /// <param name="name" type="String">Description</param>
    //</summary>

    //    var y = [1, 2, 3]
    //    var removeItem = 2;

    filters = jQuery.grep(filters, function (value) {
        return value != removeItem;
    });

    return filters;
}

function AddinArrayfunc(filters, newFilter) {
    /// <summary>Description
    /// <param name="name" type="String">Description</param>
    /// <param name="name" type="String">Description</param>
    //</summary>

    filters.push(newFilter);
}

function ShowPdfSection() {


    if ($('#Showpdfdiv').css('display') == 'none') {
        $('#Showpdfdiv').show('slow');
    } else {
        $('#Showpdfdiv').hide('slow');
    }


    return false;
}


function pdfcheck() {

    var radiolist = document.getElementById("<%=RadioButtonListCustomerType.ClientID%>");

   // var selectedRadio = $("input[name='RadioButtonListCustomerType']:checked").val().toString().toUpperCase();
    var selectedRadio = $("#HidDist").val().toString().toUpperCase();

    selectedRadio = $.trim(selectedRadio);

    if (!CheckOutputFieldStatus())
        return false;

    if (selectedRadio != "DISTRIBUTOR") {
        return true;
    }

    if ($('#txtPreparedby').val() == '') {
        alert('Please Enter your Name.');
        $('#txtPreparedby').focus()
        return false;
    }

    if ($('#txtMobile').val() == '') {
        alert('Please Enter your Mobile No.');
        $('#txtMobile').focus()
        return false;
    }

    if ($('#txtPreparedFor').val() == '') {
        alert('Please Enter Prepared for Name.');
        $('#txtPreparedFor').focus()
        return false;
    }

    if ($('#txtEmail').val() == '') {
        alert('Please Enter Email Id.');
        $('#txtEmail').focus()
        return false;
    }

    var pattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    if (!pattern.test($('#txtEmail').val())) {
        alert("Please Enter Valid Email Address"); $("#txtEmail").focus();
        return false;
    }



    return true;
}


//function SetdivVisible() {

//    alert($('#DistributerDiv').valueOf());
//    alert($('#RadioButtonListCustomerType').valueOf());   

//}

function NextMethod(tabno) {
   
    $("#tabsholder").tytabs({
        tabinit: tabno,
        fadespeed: "fast"
    });

    var num = Number(tabno);
    num = num - 1;
   // alert(num);

    $("#tab" + num).removeAttr("class");
}