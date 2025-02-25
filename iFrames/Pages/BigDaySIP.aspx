<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BigDaySIP.aspx.cs" Inherits="iFrames.Pages.BigDaySIP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/jquery-ui-1.8.14.custom2.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui-1.8.14.custom.min2.js"></script>
    <link type="text/css" href="../Styles/pramerica.css" rel="Stylesheet" />
    <%--<script src="../Scripts/dhtmlgoodies_calendar.js" type="text/javascript"></script>
    <link href="../Styles/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#StartDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0
            });
        });

    </script>
    <style type="text/css">
        .styleResultTble
        {
            border-style: solid;
            width: 85%;
            font-family: calibri;
            font-size: 11px;
            border-width: 3px;
            border-color: 2px solid #007ac2;
            margin-left: 5%;
            margin-right: 10%;
        }
        .styletxtbox
        {
            height: 22px;
            width: 182px;
            background-image: url(../Images/rndtxtbx.gif);
            border-color: White;
            border-width: 0px;
            text-align: left;
            background-repeat: no-repeat;
            padding-left: 8px;
            padding-right: 8px;
            padding-bottom: 1px;
            padding-top: 5px;
        }
        .styleMid2
        {
            text-align: center;
            border-color: #007ac2;
            border-width: 2px;
        }
        .styleMid3
        {
            text-align: left;
            border-color: #007ac2;
            border-width: 2px;
        }
        .styleJrny
        {
            font-family: Calibri;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            vertical-align: bottom;
            height: 50px;
        }
        .gridHeader
        {
            border-left-color: #007ac2;
            border-width: 2px;
            border-right-color: #007ac2;
        }
        .styleMid
        {
            text-align: center;
            border-color: #007ac2;
            border-width: 1px;
        }
        .styleDisclaimer
        {
            text-align: justify;
            font-family: Calibri;
            font-size: 12px;
            width: 970px;
        }
        .styletd1
        {
            width: 38%;
        }
        .styletd2
        {
            width: 31%;
        }
        .styletd3
        {
            width: 31%;
        }
        .resulttdstyle
        {
            border-bottom-color: #0073D0;
            border-color: #0073D0;
            border-left-color: #0073D0;
            border-right-color: #0073D0;
        }
        .styletxtbox
        {
            height: 22px;
            width: 140px;
            background-image: url(../Images/rndtxtbx.gif);
            border-color: White;
            border-width: 0px;
            text-align: left;
            background-repeat: no-repeat;
            padding-left: 8px;
            padding-right: 8px;
            padding-bottom: 1px;
            padding-top: 5px;
        }
        .styletxtboxRupee
        {
            height: 20px;
            width: 167px;
            background-image: url(../Images/finaltxt.gif);
            border-color: White;
            border-width: 0px;
            text-align: left;
            vertical-align: top;
            background-repeat: no-repeat;
            padding-left: 30px;
            padding-top: 8px;
            padding-bottom: 2px;
        }
        
        .styletxtboxwr
        {
            height: 18px;
            width: 172px;
            background-image: url(../Images/rndtext.gif);
            border-color: White;
            border-width: 0px;
            text-align: left;
            vertical-align: bottom;
            background-repeat: no-repeat;
            padding-left: 25px;
            padding-top: 7px;
            padding-bottom: 3px;
        }
        
        .stylcolorTable
        {
            width: 60%;
            margin-left: 20%;
            margin-right: 20%;
            background-color: #007ac2;
        }
        .listNegativerowcolor
        {
            background-color: Red;
        }
        .listrowcolor
        {
            background-color: White;
        }
        
        .myinputdiv
        {
            width: 99%;
            text-align: center;
            padding: 3.5px;
            border: 2px solid #007ac2;
        }
        
        .myinputOuterdiv
        {
            margin: 0.5in auto;
            width: 86%;
            padding: 2px;
            text-align: center;
            border: 5px solid #007ac2;
        }
        p.Default
        {
            margin-bottom: .0001pt;
            font-size: 12.0pt;
            font-family: "Arial" , "sans-serif";
            color: black;
            margin-left: 30px;
            margin-right: 0in;
            margin-top: 0in;
        }
        .style1
        {
            width: 78%;
        }
        .style4
        {
            height: 67px;
        }
    </style>
    <script type="text/javascript">

        function initialize() {
            var name = document.getElementById('<%=txtAdvisorname.ClientID%>');
            var txt2 = document.getElementById('<%=advisorTxt.ClientID %>');
            //document.getElementById('<%=txtAdvisorname.ClientID%>').value;
            if (name.value == 'Advisor Company Name') {
                name.value = 'Advisor Company Name';
                name.style.color = 'DarkGray';
            }
            if (txt2.value == 'Advisor Name') {
                txt2.value = 'Advisor Name';
                txt2.style.color = 'DarkGray';
            }

        }

        function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;

            if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
                //document.getElementById('<%=Amount.ClientID %>').Value()//keycode == 46 ||
                //alert('test');
                if ((keycode >= 48 && keycode <= 57)) {

                    //document.getElementById('<%=Amount.ClientID%>').value += ' per month'
                }

                //document.getElementById('<%=Amount.ClientID%>').value=document.getElementById('<%=Amount.ClientID%>').value.toString().replace(" per month", "");
                return true;
            }
            return false;
        }


        function addPerMonth() {
            var sipamunt = document.getElementById('<%=Amount.ClientID%>').value;
            var splitamount = sipamunt.split(' ');
            //splitamount[0] = splitamount[0] + ' per month';
            //sipamunt.replace('per month', 'per month');
            if (splitamount[0] != '' && splitamount.length > 0) {
                document.getElementById('<%=Amount.ClientID%>').value = splitamount[0] + ' per month';
            }
        }
        function removetext() {
            document.getElementById('<%=Amount.ClientID%>').value = "";
        }
        function chTextin2() {

            var txt2 = document.getElementById('<%=advisorTxt.ClientID %>');
            if (txt2.value == 'Advisor Name') {
                txt2.value = '';
                txt2.style.color = '';
            }
        }
        function chTextin3() {

            var txt2 = document.getElementById('<%=txtARNNumber.ClientID %>');
            if (txt2.value != '') {
                txt2.value = '';
                txt2.style.color = '';
            }

        }
        function chTextin4() {

            var txt2 = document.getElementById('<%=InvestorName.ClientID %>');
            if (txt2.value == 'Client Name') {
                txt2.value = '';
                txt2.style.color = '';
            }
        }
        function chTextin5() {

            var txt2 = document.getElementById('<%=StartDate.ClientID %>');
            if (txt2.value == 'DD-MMM-YYYY') {
                txt2.value = '';
                txt2.style.color = '';
            }
        }
        function chTextin6() {

            var txt2 = document.getElementById('<%=InvestorName1.ClientID %>');
            if (txt2.value == 'Client Name') {
                txt2.value = '';
                txt2.style.color = '';
            }
        }

        function chTextin() {
            var txt = document.getElementById('<%=txtAdvisorname.ClientID%>');
            if (txt.value == 'Advisor Company Name') {
                txt.value = '';
                txt.style.color = '';
            }
        }


        //        function chkWordLimit() {
        //            debugger;
        //            var txt = document.getElementById('<%=txtAdvisorname.ClientID%>').value;
        //            if (txt != '') {
        //                var spaceCount = test.count(' ');
        //                if (spaceCount > 1) {
        //                    alert('Please enter a Advisor Name Consisting of 2 words.');
        //                    document.getElementById('<%=txtAdvisorname.ClientID%>').value = '';
        //                    document.getElementById('<%=txtAdvisorname.ClientID%>').focus();
        //                }               
        //                
        //            }
        //        }

        function chTextout() {

            var txt = document.getElementById('<%=txtAdvisorname.ClientID%>');

            if (txt.value == '') {
                txt.value = 'Advisor Company Name';
                txt.style.color = 'DarkGray';
            }

            //            var txtt = document.getElementById('<%=txtAdvisorname.ClientID%>').value;
            //            if (txtt != 'Advisor Company Name' && txtt != '') {
            //                var spaceCount = txtt.split(' ');

            //                if (spaceCount.length > 2) {
            //                    alert('Please enter a Advisor Company Name Consisting of 2 words.');
            //                    document.getElementById('<%=txtAdvisorname.ClientID%>').value = '';
            //                    document.getElementById('<%=txtAdvisorname.ClientID%>').focus();
            //                }

            //            }
        }

        function chTextout2() {


            var txt2 = document.getElementById('<%=advisorTxt.ClientID %>');


            if (txt2.value == '') {
                txt2.value = 'Advisor Name';
                txt2.style.color = 'DarkGray';
            }

        }
        function chTextout3() {

            var txt2 = document.getElementById('<%=txtARNNumber.ClientID %>');
            if (txt2.value == '') {
                txt2.value = 'ARN Number';
                txt2.style.color = 'DarkGray';
            }
            else {

                var storeval = document.getElementById('<%=txtARNNumber.ClientID%>').value;
                document.getElementById('<%=txtARNNumber.ClientID%>').value = '';
                document.getElementById('<%=txtARNNumber.ClientID%>').value = 'ARN-' + storeval.replace('ARN-', "");
            }

        }
        function chTextout4() {


            var txt2 = document.getElementById('<%=InvestorName.ClientID %>');
            if (txt2.value == '') {
                txt2.value = 'Client Name';
                txt2.style.color = 'DarkGray';
            }

        }
        function chTextout5() {


            var txt2 = document.getElementById('<%=StartDate.ClientID %>');
            if (txt2.value == '') {
                txt2.value = 'DD-MMM-YYYY';
                txt2.style.color = 'DarkGray';
            }

        }
        function chTextout6() {

            var txt2 = document.getElementById('<%=InvestorName1.ClientID %>');
            if (txt2.value == '') {
                txt2.value = 'Client Name';
                txt2.style.color = 'DarkGray';
            }

        }


    </script>
    <script type="text/javascript">

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



        function date_validate() {
            var sipStartdata = document.getElementById('<%=StartDate.ClientID%>').value;
            //document.getElementById('<%=Amount.ClientID%>').value;
            var sipAmunt = document.getElementById('<%=Amount.ClientID%>').value;

            //var selectedradiolist = document.getElementById("RadioButtonList1");
            // var selectedradiolist = document.getElementsByName("RadioButtonList1");
            //            if (selectedradiolist[1].checked =false && selectedradiolist[2].checked = false )
            //            return false;
            //selectedradiolist.se   
            var RegX = /^[A-Za-z.'' ]{1,200}$/;
            var field = document.getElementById('<%=InvestorName.ClientID%>').value;
            //document.getElementById('<%=InvestorName.ClientID%>').value;

            //var ck_advisorComapnyname = /^[A-Za-z0-9!@#$%^&*()_]{6,20}$/;
            var ck_advisor = /^[A-Za-z.'' ]{1,200}$/;



            if (field == "" || field=="Client Name")
            {
                alert('Please enter a valid Name.');
                document.getElementById('<%=InvestorName.ClientID%>').value = "";
                document.getElementById('<%=InvestorName.ClientID%>').focus();
                return false;
            }


            if (!RegX.test(field)) {
                alert('Please enter a valid Name.');
                document.getElementById('<%=InvestorName.ClientID%>').value = "";
                document.getElementById('<%=InvestorName.ClientID%>').focus();
                return false;
            }

            if (sipStartdata == "" || sipAmunt == "") {
                if (sipStartdata == "") {
                    alert("Big Day cannot be Blank");
                    document.getElementById('<%=StartDate.ClientID%>').focus();
                    return false;
                }

                else {
                    alert("Amount cannot be Blank.");
                    document.getElementById('<%=Amount.ClientID%>').focus();
                    return false;
                }
            }

//            var advisorName = document.getElementById('<%=txtAdvisorname.ClientID%>').value;
//            if (advisorName == "Advisor Company Name") {
//                alert("Please enter Advisor Company Name ");
//                document.getElementById('<%=txtAdvisorname.ClientID%>').focus();
//                return false;
//            }

//            var advisor = document.getElementById('<%=advisorTxt.ClientID%>').value;
//            if (advisor == "Advisor Name") {
//                alert("Please enter Advisor Name ");
//                document.getElementById('<%=advisorTxt.ClientID%>').focus();
//                return false;
//            }

            if (!ck_advisor.test(advisorName)) {
                alert('Please enter a valid Advisor Company Name.');
                document.getElementById('<%=txtAdvisorname.ClientID%>').value = "";
                document.getElementById('<%=txtAdvisorname.ClientID%>').focus();
                return false;
            }

            if (!ck_advisor.test(advisor)) {
                alert('Please enter a valid Advisor Name.');
                document.getElementById('<%=advisorTxt.ClientID%>').value = "";
                document.getElementById('<%=advisorTxt.ClientID%>').focus();
                return false;
            }

            if (isNaN(sipAmunt.split(' ')[0])) {
                alert("Please enter Numeric value.");
                document.getElementById('<%=Amount.ClientID%>').value = "";
                document.getElementById('<%=Amount.ClientID%>').focus();
                return false;
            }
            else {
                if (sipAmunt.split(' ')[0] == 0) {
                    alert("Please enter Valid Numeric value.");
                    document.getElementById('<%=Amount.ClientID%>').value = "";
                    document.getElementById('<%=Amount.ClientID%>').focus();
                    return false;
                }
            }

            //            var validdateformat = /^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity

            //            if (!validdateformat.test(sipStartdata)) {
            //                alert("Invalid Date Format. Please correct and submit again.");
            //                document.getElementById('<%=StartDate.ClientID%>').value = "";
            //                document.getElementById('<%=StartDate.ClientID%>').focus();
            //                return false;
            //            }


            var Day = parseInt(sipStartdata.substring(0, 2), 10);
            var Mn = parseInt(sipStartdata.substring(3, 5), 10);
            var Yr = parseInt(sipStartdata.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);

            //            if ((dt.getMonth() + 1 != Mn) || (dt.getDate() != Day) || (dt.getFullYear() != Yr)) {
            //                alert("Invalid Day, Month, or Year range detected. Please correct and submit again.")
            //                document.getElementById('<%=StartDate.ClientID%>').value = "";
            //                document.getElementById('<%=StartDate.ClientID%>').focus();
            //                return false;
            //            }

            var todaydate = new Date();

            var i = DateDiff.inDays(dt, todaydate)
            if (i <= 0) {
                alert("Start Date should be Less than Today");
                document.getElementById('<%=StartDate.ClientID%>').value = "";
                document.getElementById('<%=StartDate.ClientID%>').focus();
                return false;
            }

            var startsensexdate = new Date("01/01/1980");
            if (dt < startsensexdate) {
                alert("Please select Start Date from 1st January 1980");
                document.getElementById('<%=StartDate.ClientID%>').value = "";
                document.getElementById('<%=StartDate.ClientID%>').focus();
                return false;

            }

            return true;
        }
    </script>
    <script type="text/javascript" language="javascript">
        function Print() {
            window.print();
        }
    </script>
</head>
<body>
    <%--<body onload="initialize();">--%>
    <form id="form1" runat="server">
    <div align="center">
        <table width="975" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="9%" height="89" bgcolor="#006bc4">
                    &nbsp;
                </td>
                <td width="9%">
                    &nbsp;
                </td>
                <td width="57%" class="header_top">
                    <asp:Label runat="server" ID="headerId" Text="Big Day SIP" Font-Bold="true" ForeColor="#006bc4"></asp:Label>
                </td>
                <td width="25%" align="right">
                    <img src="../Images/logo.jpg" width="234" height="86" alt="" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                </td>
            </tr>
        </table>
        <div id="inputDiv" runat="server">
            <table width="975" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <table width="915" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%-- <table width="715" height="435" border="0" align="center" cellpadding="0" cellspacing="0">--%>
                                    <table width="715" height="400" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <%--<td height="435" align="center" valign="top" background="../Images/bg_input.jpg"
                                                style="background-repeat: no-repeat">--%>
                                            <td height="400" align="center" valign="top" background="../Images/bg_input.jpg"
                                                style="background-repeat: no-repeat">
                                                <asp:Panel ID="pnlTop" runat="server" DefaultButton="ImageButton1">
                                                    <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="97%">
                                                                &nbsp;
                                                            </td>
                                                            <td width="3%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <div>
                                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                                        <tr>
                                                                            <td align="left" background="../Images/input1.jpg" style="background-repeat: no-repeat;
                                                                                width: 40%;">
                                                                                <asp:TextBox ID="txtAdvisorname" runat="server" CssClass="input2" onfocus="chTextin();"
                                                                                    onblur="chTextout();" Width="227px" ForeColor="DarkGray"> </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" background="../Images/input12.jpg" style="background-repeat: no-repeat;
                                                                                width: 21%;">
                                                                                <asp:TextBox ID="txtARNNumber" runat="server" CssClass="input2" onfocus="chTextin3();"
                                                                                    onblur="chTextout3();" Width="100px" Text="ARN Number" ForeColor="DarkGray"> </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" background="../Images/input1.jpg" style="background-repeat: no-repeat;
                                                                                width: 40%; height: 44px;">
                                                                                <asp:TextBox ID="advisorTxt" runat="server" CssClass="input2" onfocus="chTextin2();"
                                                                                    onblur="chTextout2();" Width="211px" ForeColor="DarkGray"> </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <%-- <tr>
                                                        <td align="left" style="background-repeat: no-repeat">
                                                            <table width="253" height="44" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" background="../Images/input1.jpg" style="background-repeat: no-repeat">
                                                                        <asp:TextBox ID="txtAdvisorname" runat="server" CssClass="input2" onfocus="chTextin();"
                                                                            onblur="chTextout();" Width="227px">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <table width="253" height="44" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" background="../Images/input1.jpg" style="background-repeat: no-repeat">
                                                                        <asp:TextBox ID="advisorTxt" runat="server" CssClass="input2" onfocus="chTextin2();"
                                                                            onblur="chTextout2();" Width="211px">
                                                                        </asp:TextBox>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left" class="header" style="background-repeat: no-repeat">
                                                            &nbsp;
                                                        </td>
                                                    </tr>--%>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr width="100%">
                                                            <td colspan="2">
                                                                <table width="100%">
                                                                    <tr width="100%">
                                                                        <td width="75%">
                                                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="40%" height="60" class="header">
                                                                                        Big Day for
                                                                                    </td>
                                                                                    <td height="60" class="style1">
                                                                                        <table width="100%" style="height: 47px">
                                                                                            <tr>
                                                                                                <td background="../Images/input2.gif" align="left" style="background-repeat: no-repeat;"
                                                                                                    class="style1">
                                                                                                    <table border="0" cellspacing="0" cellpadding="0" style="height: 33px; width: 174px">
                                                                                                        <tr>
                                                                                                            <td width="4%">
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                            <td width="96%">
                                                                                                                <asp:TextBox ID="InvestorName" runat="server" CssClass="input2" Width="230px" Height="20px"
                                                                                                                    onfocus="chTextin4();" onblur="chTextout4();" Text="Client Name" ForeColor="DarkGray"> </asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="60" class="header" style="padding-right: 30px;">
                                                                                        Big Day
                                                                                    </td>
                                                                                    <td align="left" style="background-repeat: no-repeat" class="style1">
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td width="100%" valign="top" background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height: 45px">
                                                                                                        <tr>
                                                                                                            <td width="4%">
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                            <td width="96%">
                                                                                                                <%--<asp:TextBox ID="StartDate" runat="server" CssClass="input2" Width="178px" Height="27px"
                                                                                                                Text="DD-MMM-YYYY" onfocus="chTextin5();" onblur="chTextout5();">
                                                                                                            </asp:TextBox>--%>
                                                                                                                <asp:TextBox ID="StartDate" runat="server" CssClass="input2" Width="178px" Height="27px"
                                                                                                                    Text="DD-MMM-YYYY"> </asp:TextBox>
                                                                                                                <%--
                                                                                                onmouseover="if (timeoutId) clearTimeout(timeoutId);window.status='Show Calendar';return true;"
                                                                                                    onmouseout="if (timeoutDelay) calendarTimeout();window.status='';" onclick="g_Calendar.show(event,'form1.StartDate',true,'dd/mm/yyyy'); return false;"></asp:TextBox>
                                                                                                onfocus="displayCalendar(document.forms[0].StartDate,'dd/mm/yyyy',this)"--%>
                                                                                                                <%--<input name="textfield22" type="text" class="input3" value="Date" /> onfocus="pickDate(this,document.forms[0].StartDate);" -- --%>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="60" class="header">
                                                                                        SIP Amount
                                                                                    </td>
                                                                                    <td height="60" class="style1">
                                                                                        <table width="100%" style="height: 47px">
                                                                                            <tr>
                                                                                                <td background="../Images/input2.gif" style="background-repeat: no-repeat" align="left">
                                                                                                    <table border="0" cellspacing="0" cellpadding="0" style="width: 80%; margin-left: 0px">
                                                                                                        <tr align="center">
                                                                                                            <td align="right" width="10%">
                                                                                                                <img src="../Images/rimg.png" />
                                                                                                            </td>
                                                                                                            <td width="90%" align="left">
                                                                                                                <asp:TextBox ID="Amount" onclick="removetext();" onkeypress="return isNumber(event)"
                                                                                                                    onblur="addPerMonth();" runat="server" CssClass="input2" MaxLength="10" Width="184px">XXXX per month</asp:TextBox>
                                                                                                            </td>
                                                                                                            <%--<td align="left" width="52%" style="color: Gray; font-family: Calibri; font-size: large">
                                                                                                            per month
                                                                                                        </td>--%>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td width="25%" align="left">
                                                                            <table width="160" height="66" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="160" background="../Images/input3.jpg" style="background-repeat: no-repeat">
                                                                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" Font-Size="15px" CssClass="input2"
                                                                                            Height="40px">
                                                                                            <asp:ListItem>Birth Date</asp:ListItem>
                                                                                            <asp:ListItem Selected="true">Wedding Date</asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="29%">
                                                                        </td>
                                                                        <td width="71%" align="left">
                                                                            &nbsp;
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/btn.png" ForeColor="White"
                                                                                OnClientClick="return date_validate();" OnClick="btnSipclick" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <%-- <tr>
                                                            <td colspan="2" align="right">
                                                            </td>
                                                        </tr>--%>
                                                        <%--  <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                        <%-- <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                        <%--<tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td colspan="2" align="right">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>--%>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <img src="../Images/avatar.jpg" alt="" width="180" height="100" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="OutputDiv" runat="server">
            <table width="975" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <table width="915" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="915" height="880" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right" valign="bottom">
                                                <%--<table width="20%">
                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <a href="#" onclick="Print();">
                                                                <img alt="" src="../Chart/images/printer.jpeg" width="16" height="16" /></a>
                                                            <a href="#" onclick="Print();" style="color: #007AC2; text-decoration: none; font-family: Verdana;
                                                                font-size: smaller;"><b>Print</b></a>
                                                        </td>
                                                        <td style="width: 50%;" align="left">
                                                            <asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="lnkbtnDownload_Click">
                                                                <img src="../Chart/images/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="header" align="center">
                                                <asp:Label ID="lblAdvisorHeading" runat="server" Text="Advisor" Font-Size="18px" Visible="false"></asp:Label>
                                                <%--<span style="font-size: 18px;">Advisor</span>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblAdvisorName" runat="server" Text="" Font-Bold="true" ForeColor="#006bc4"
                                                    Font-Size="25px" Font-Names="Calibri"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblAdvisorCompanyName" runat="server" Text="" Font-Bold="true" ForeColor="#006bc4"
                                                    Font-Size="25px" Font-Names="Calibri"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblAdvisorARNNumber" runat="server" Text="" Font-Bold="true" ForeColor="#006bc4"
                                                    Font-Size="25px" Font-Names="Calibri"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td align="left" valign="top" height="722" background="../Images/bgg.jpg" style="background-repeat: no-repeat">--%>
                                            <td align="left" valign="top" height="760" background="../Images/bg.jpg" style="background-repeat: no-repeat">
                                                <table width="80%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="style4" valign="bottom" style="padding-right: 20px;" colspan="3">
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td style="width: 80%; height: 85px;" align="right">
                                                                    </td>
                                                                    <td style="width: 60%;" align="center" valign="bottom">
                                                                        <asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="lnkbtnDownload_Click">
                                                                <img src="../Chart/images/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:LinkButton>
                                                                    </td>
                                                                    <%-- <td align="right" class="style3" style="display: none; width: 20%;">
                                                                        <a href="#" onclick="Print();">
                                                                            <img alt="" src="../Chart/images/printer.jpeg" width="16" height="16" style="border: 0px;" /></a>
                                                                        <a href="#" onclick="Print();" style="color: #007AC2; text-decoration: none; font-family: Verdana;
                                                                            font-size: smaller;"></a>
                                                                    </td>--%>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td class="header" colspan="3" align="center">
                                                            <%=logo%>
                                                        </td>
                                                    </tr>--%>
                                                    <%--<tr>
                                                        <td colspan="3" align="center">
                                                            <div id="dvAdvisorName">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="right" style="width: 50%;">
                                                                            <span style="font-family: Calibri; font-size: 19px; color: #000000; text-decoration: none;">
                                                                                Advisor Name :&nbsp</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblAdvisorName" runat="server" Text="" Font-Bold="true" ForeColor="#006bc4"
                                                                                Font-Size="19px" Font-Names="Calibri"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>--%>
                                                    <%--<tr>
                                                        <td style="width:40%;">
                                                            <div id="dvAdvisorName">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="left" style="width:30%;">
                                                                            <span style="font-family: Calibri; font-size: 14px; color: #000000; text-decoration: none;">
                                                                                Advisor Name:</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblAdvisorName" runat="server" Text="" Font-Bold="true" ForeColor="#006bc4"
                                                                                Font-Size="15px" Font-Names="Calibri"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td style="width:20%;">
                                                            <div id="dvARNNumber">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="left" style="width:40%;">
                                                                            <span style="font-family: Calibri; font-size: 15px; color: #000000; text-decoration: none;">
                                                                                ARN No :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblAdvisorARNNumber" runat="server" Text="" Font-Bold="true" ForeColor="#006bc4"
                                                                                Font-Size="15px" Font-Names="Calibri"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td style="width:40%;">
                                                            <div id="dvAdvisorCompany">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="left" style="width:40%;">
                                                                            <span style="font-family: Calibri; font-size: 14px; color: #000000; text-decoration: none;">
                                                                                Advisor Company:</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblAdvisorCompanyName" runat="server" Text="" Font-Bold="true" ForeColor="#006bc4"
                                                                                Font-Size="15px" Font-Names="Calibri"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>--%>
                                                    <%-- <tr>
                                                        <td align="left" style="background-repeat: no-repeat;" class="style7">
                                                            Advisor Company Name
                                                            <table width="253" height="44" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" background="../../Images/input1.jpg" style="background-repeat: no-repeat">
                                                                        <asp:Label runat="server" ID="txtAdvisorname1" CssClass="input2" Text="" Font-Bold="true"
                                                                            Font-Size="Large"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="background-repeat: no-repeat;" class="header">
                                                            Prepared By
                                                            <table width="253" height="44" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" background="../../Images/input1.jpg" style="background-repeat: no-repeat">
                                                                        <asp:Label runat="server" ID="advisorTxt11" CssClass="input2" Text="" Font-Bold="true"
                                                                            Font-Size="Large"></asp:Label>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left" class="header" style="background-repeat: no-repeat">
                                                            &nbsp;
                                                        </td>
                                                    </tr>--%>
                                                    <%-- <tr>
                                                        <td colspan="3" style="height: 20px;">
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td colspan="3">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td width="80%">
                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                            <%-- <tr>
                                                                                <td width="45%" height="50" class="header">
                                                                                    Advisor Company Name
                                                                                </td>
                                                                                <td width="55%" background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                    <table width="50%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="4%">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td width="96%">
                                                                                                <asp:Label runat="server" ID="txtAdvisorname1" CssClass="input2" Text="" Font-Bold="true"
                                                                                                    Font-Size="Large"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="45%" height="50" class="header">
                                                                                    Prepared By
                                                                                </td>
                                                                                <td width="55%" background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                    <table width="50%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="4%">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td width="96%">
                                                                                                <asp:Label runat="server" ID="advisorTxt11" CssClass="input2" Text="" Font-Bold="true"
                                                                                                    Font-Size="Large"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>--%>
                                                                            <tr>
                                                                                <td width="45%" height="50" class="header">
                                                                                    Big Day for
                                                                                </td>
                                                                                <td width="55%" background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                    <table width="50%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="4%">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td width="96%">
                                                                                                <asp:TextBox ID="InvestorName1" runat="server" CssClass="input2" Width="230px" ReadOnly="True"
                                                                                                    Text="Client Name" onfocus="chTextin6();" onblur="chTextout6();">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height="50" class="header">
                                                                                    Big Day (<%=DependOn %>)
                                                                                </td>
                                                                                <td width="55%" background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                    <table width="50%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="4%">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td width="96%">
                                                                                                <asp:TextBox ID="StartDate1" runat="server" ReadOnly="True" CssClass="input2" Width="230px"
                                                                                                    Text="DD-MMM-YYYY">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height="50" class="header">
                                                                                    SIP Amount
                                                                                </td>
                                                                                <td background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                    <table border="0" cellspacing="0" cellpadding="0" width="80%">
                                                                                        <tr>
                                                                                            <td align="right" width="11%">
                                                                                                <img src="../Images/rimg.png" />
                                                                                            </td>
                                                                                            <td width="89%">
                                                                                                <asp:TextBox ID="Amount1" runat="server" Width="186px" ReadOnly="True" CssClass="input2">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                            <%--<td width="44%" style="color: Gray; font-family: Calibri; font-size: 17px" align="left">
                                                                                                per month
                                                                                            </td>--%>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height="50" class="header_bold">
                                                                                    Total Amount Invested till date
                                                                                </td>
                                                                                <td background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                    <table width="80%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td align="right" width="10%">
                                                                                                <img src="../Images/rimg.png" />
                                                                                            </td>
                                                                                            <td width="90%" align="left">
                                                                                                <asp:TextBox ID="investedAmountTotal" runat="server" ReadOnly="True" CssClass="input2"
                                                                                                    Width="130px">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height="50" class="header_bold">
                                                                                    Period of Investment
                                                                                </td>
                                                                                <td background="../Images/input2.gif" style="background-repeat: no-repeat">
                                                                                    <table width="50%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="4%">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td width="96%">
                                                                                                <asp:TextBox ID="investmentPeriod" runat="server" ReadOnly="True" CssClass="input2">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="20%">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="display: none;">
                                                                                    <table style="margin-left: 5%" width="157" height="66" border="0" cellpadding="0"
                                                                                        cellspacing="0">
                                                                                        <tr>
                                                                                            <td width="165" background="../Images/input3.gif" style="background-repeat: no-repeat">
                                                                                                <asp:RadioButtonList ID="RadioButtonList2" readonly="True" runat="server" Font-Size="15px"
                                                                                                    CssClass="input2" Height="40px">
                                                                                                    <asp:ListItem>Birth Date</asp:ListItem>
                                                                                                    <asp:ListItem>Wedding Date</asp:ListItem>
                                                                                                </asp:RadioButtonList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <%-- <td colspan="2" class="style5">
                                                        </td>--%>
                                                        <td colspan="2" style="height: 80px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="150px">
                                                            <asp:GridView ID="SipGridView" Width="100%" runat="server" BorderWidth="2px" BackColor="White"
                                                                CellPadding="4" BorderStyle="Solid" BorderColor="#007AC2" GridLines="Both" AutoGenerateColumns="False"
                                                                ShowFooter="false" OnRowCreated="SipGridViewRowCreated" HeaderStyle-BorderColor="#007AC2">
                                                                <RowStyle BackColor="#CEE9FF" Font-Names="calibri" BorderColor="#007AC2" BorderWidth="2px" />
                                                                <AlternatingRowStyle BackColor="White" Font-Names="calibri" BorderColor="#007AC2"
                                                                    BorderWidth="2px" />
                                                                <HeaderStyle Font-Bold="True" Font-Names="calibri" Font-Size="12" BorderColor="#007AC2"
                                                                    BorderWidth="2px" CssClass="gridHeader"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25%" HeaderStyle-BorderColor="#007AC2"
                                                                        HeaderStyle-BorderWidth="2px" ControlStyle-CssClass="gridHeader" ControlStyle-Font-Bold="true">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("HeaderColumn") == DBNull.Value) ? "--" : Eval("HeaderColumn").ToString()%>
                                                                        </ItemTemplate>
                                                                        <%--<HeaderStyle Width="65px" />--%>
                                                                        <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid3" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Value of SIP in Sensex (<img src='../Images/rsymbol.JPG' style='vertical-align:middle;'/> in Lakhs )"
                                                                        HeaderStyle-BorderColor="#007AC2" HeaderStyle-BorderWidth="2px" HeaderStyle-Width="22%">
                                                                        <ItemTemplate>
                                                                            <%-- <%# (Eval("SipValue") == DBNull.Value) ? "--" : Eval("SipValue").ToString()%>--%>
                                                                            <%--<%# (Eval("SipValue") == DBNull.Value) ? "--" : Math.Round((Convert.ToDouble(Eval("SipValue")) / 100000.00), 2).ToString()%>--%>
                                                                            <%# (Eval("SipValue") == DBNull.Value) ? "--" : TwoDecimal(Math.Round((Convert.ToDouble(Eval("SipValue")) / 100000.00), 2).ToString())%>
                                                                        </ItemTemplate>
                                                                        <%-- <HeaderStyle Width="65px" />--%>
                                                                        <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns(%)" HeaderStyle-BorderColor="#007AC2"
                                                                        ControlStyle-CssClass="gridHeader" HeaderStyle-BorderWidth="2px" HeaderStyle-Width="22%">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("SipCagrValue") == DBNull.Value) ? "--" : TwoDecimal(Eval("SipCagrValue").ToString())%>
                                                                        </ItemTemplate>
                                                                        <%-- <HeaderStyle Width="65px" />--%>
                                                                        <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid2" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Value of SIP in Fixed Return Product (<img src='../Images/rsymbol.JPG' style='vertical-align:middle;'/> in Lakhs )"
                                                                        HeaderStyle-BorderColor="#007AC2" HeaderStyle-BorderWidth="2px" ControlStyle-CssClass="gridHeader"
                                                                        HeaderStyle-Width="22%">
                                                                        <ItemTemplate>
                                                                            <%--<%# (Eval("FdValue") == DBNull.Value) ? "--" : Eval("FdValue").ToString()%>--%>
                                                                            <%# (Eval("FdValue") == DBNull.Value) ? "--" : TwoDecimal(Math.Round((Convert.ToDouble(Eval("FdValue")) / 100000.00), 2).ToString())%>
                                                                        </ItemTemplate>
                                                                        <%-- <HeaderStyle Width="65px" />--%>
                                                                        <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid2" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns(%)" ControlStyle-CssClass="styleMid2"
                                                                        HeaderStyle-Width="22%">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("FdCagrValue") == DBNull.Value) ? "--" : TwoDecimal(Eval("FdCagrValue").ToString())%>
                                                                        </ItemTemplate>
                                                                        <%-- <HeaderStyle Width="65px" />--%>
                                                                        <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid2" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="10px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" colspan="3">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td width="22%" style="font-family: Calibri; font-size: 16; display: none;">
                                                                        Advisor Company Name:
                                                                    </td>
                                                                    <td width="78%" align="left" style="font-weight: bold;">
                                                                        <%--<asp:Label runat="server" ID="txtAdvisorname1" CssClass="input2" Text=""></asp:Label>--%>
                                                                    </td>
                                                                    <%--<td  style="background-repeat: no-repeat;">
                                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 186px; height: 43px;">
                                                                            <tr>
                                                                                <td align="left" background="../Images/input4.gif" class="header" style="background-repeat: no-repeat">
                                                                                
                                                                                    -<asp:TextBox ID="txtAdvisorname1" runat="server" ReadOnly="True" CssClass="input2" Width="166px"></asp:TextBox>--                                                                             </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td width="22%" align="left" style="font-family: Calibri; font-size: 16; display: none;">
                                                                        Prepared By:
                                                                    </td>
                                                                    <td align="left" width="78%" style="font-weight: bold;">
                                                                        <%--<asp:Label runat="server" ID="advisorTxt11" CssClass="input2" Text=""></asp:Label>--%>
                                                                    </td>
                                                                    <%--<td align="left" width="66%" height="43px" background="../Images/input4.gif" class="header"
                                                                        style="background-repeat: no-repeat;">
                                                                        
                                                                        <%--<asp:TextBox ID="advisorTxt11" ReadOnly="True" runat="server" CssClass="input2" Width="166px"></asp:TextBox>
                                                                    </td>--%>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel runat="server" ID="SourcePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="5%">
                                                            </td>
                                                            <td width="95%" align="left" style="font-family: calibri; font-size: 15px; text-align: justify;">
                                                                <ul>
                                                                    <li>Returns are in CAGR.</li>
                                                                    <li>Source of Fixed Income Return: Till March
                                                                        <asp:Label ID="yearLabel" runat="server" Text=""></asp:Label>
                                                                        Commercial Bank Deposit rates as per RBI Website and from April
                                                                        <asp:Label ID="yearLabel1" runat="server" Text=""></asp:Label>
                                                                        till date returns are assumed as 10 Year Government of India Bonds + 100bps.</li>
                                                                    <li>Powered by ICRA Analytics Ltd.</li>
                                                                </ul>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="right" width="96%">
                                                            <asp:ImageButton ID="backImageButton" OnClick="btnLinkBack_Click" ImageUrl="~/Images/backbtn.jpg"
                                                                runat="server" />
                                                        </td>
                                                        <td width="4%">
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--<asp:LinkButton ID="btnLinkBack" CssClass="backlink" ForeColor="#006bc4" PostBackUrl="~/Pages/BigDaySIP.aspx"
                                                    runat="server" OnClick="btnLinkBack_Click">Back</asp:LinkButton>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="showResultDiv" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleJrny">
                                                    JOURNEY OF YOUR INVESTMENT
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <asp:GridView ID="PRIndexListViewGrid" runat="server" CssClass="styleResultTble"
                                                        Font-Size="Small" BorderWidth="2px" CellPadding="4" BorderStyle="Solid" BorderColor="#007AC2"
                                                        GridLines="Both" AutoGenerateColumns="False" ShowFooter="false" OnRowDataBound="changeCellColor">
                                                        <HeaderStyle Font-Bold="True" Font-Names="calibri" Font-Size="12"></HeaderStyle>
                                                        <FooterStyle ForeColor="#003399"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Dates" HeaderStyle-BorderColor="#007AC2" HeaderStyle-Width="22%">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("INVESTED_DATE")).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                </ItemTemplate>
                                                                <%-- <HeaderStyle Width="65px" />--%>
                                                                <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-BorderColor="#007AC2" HeaderText="Total Cumulative Investment(<img src='../Images/rsymbol.JPG' style='vertical-align:middle;'/> in Lakhs )"
                                                                HeaderStyle-Width="25%">
                                                                <ItemTemplate>
                                                                    <%# (Eval("INVESTED_AMNT") == DBNull.Value) ? "--" : TwoDecimal(Math.Round((Convert.ToDouble(Eval("INVESTED_AMNT")) / 100000.00), 2).ToString())%>
                                                                </ItemTemplate>
                                                                <%-- <HeaderStyle Width="75px" />--%>
                                                                <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-BorderColor="#007AC2" HeaderText="Value of SIP in Sensex(<img src='../Images/rsymbol.JPG' style='vertical-align:middle;'/> in Lakhs)"
                                                                HeaderStyle-Width="26%">
                                                                <ItemTemplate>
                                                                    <%--<%#   (Eval("SIP_CMPD_AMNT") == DBNull.Value) ? "--" : indiarupeeformat(Convert.ToInt64(Eval("SIP_CMPD_AMNT")).ToString())%>--%>
                                                                    <%# (Eval("SIP_CMPD_AMNT") == DBNull.Value) ? "--" : TwoDecimal(Math.Round((Convert.ToDouble(Eval("SIP_CMPD_AMNT")) / 100000.00), 2).ToString())%>
                                                                </ItemTemplate>
                                                                <%--<HeaderStyle Width="75px" />--%>
                                                                <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-BorderColor="#007AC2" HeaderText="Value of SIP in Fixed Return Product(<img src='../Images/rsymbol.JPG' style='vertical-align:middle;'/> in Lakhs )"
                                                                HeaderStyle-Width="27%">
                                                                <ItemTemplate>
                                                                    <%--<%# (Eval("FD_SIP_CMPD_AMNT") == DBNull.Value) ? "--" : indiarupeeformat( Convert.ToInt64(Eval("FD_SIP_CMPD_AMNT")).ToString())%>--%>
                                                                    <%# (Eval("FD_SIP_CMPD_AMNT") == DBNull.Value) ? "--" : TwoDecimal(Math.Round((Convert.ToDouble(Eval("FD_SIP_CMPD_AMNT")) / 100000.00), 2).ToString())%>
                                                                </ItemTemplate>
                                                                <%--<HeaderStyle Width="75px" />--%>
                                                                <ItemStyle Font-Names="calibri" Font-Size="12pt" CssClass="styleMid" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <%-- <RowStyle ForeColor="#007AC2" BackColor="#F5F5F3"></RowStyle>--%>
                                                    </asp:GridView>
                                                    <br />
                                                    <asp:Panel runat="server" ID="highlightPanel">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="3%" align="left">
                                                                    <table align="left">
                                                                        <tr>
                                                                            <td style="background-color: Red; height: 4px; width: 4px;">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="92%" align="left" style="font-family: calibri; font-size: 15px;">
                                                                    Red highlight shows when the value of investments fell over the previous year.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <br />
                                                    <div id="disclamerDiv" align="left" runat="server" class="styleDisclaimer">
                                                        <strong>Important Disclosures</strong>: &nbsp;This calculator has been created to
                                                        illustrate the benefit of investing in equities over a long term along with the
                                                        power of compounding on monthly investments. This calculator is meant to be used
                                                        for illustrative purposes only. The various taxes, fees, expenses and /or any charges,
                                                        that may be applicable for making investments, are not considered in the calculation
                                                        provided by the calculator. The results of the calculation cannot be construed to
                                                        be entirely accurate / comprehensive and Pramerica Asset Managers Private Limited/
                                                        Pramerica Mutual Fund are not liable for any financial decisions arising out of
                                                        the use of this calculator and also they do not take the responsibility, liability
                                                        or authenticity of the figures calculated on the basis of calculator provided herein
                                                        for calculations towards prospective investments. The above equity investment simulation
                                                        is based on S&P BSE SENSEX, as proxy of equity investments. Investments in equities
                                                        may involve higher degree of risks compared to investments in fixed return products
                                                        and hence may not be suitable for all investors. This calculator is developed and
                                                        maintained by ICRA Analytics Limited and the data content provided is obtained from
                                                        sources considered to be authentic and reliable. Pramerica Asset Managers Private
                                                        Limited/ Pramerica Mutual Fund are not responsible for any error or inaccuracy or
                                                        for any losses suffered on account of information. You are advised to consult your
                                                        Tax and Professional Advisors in regard to tax implications before making any decision
                                                        based on the results provided by the calculator.<br />
                                                        &nbsp;<strong>Risk Factors: - MUTUAL FUND INVESTMENTS ARE SUBJECT TO MARKET RISKS, READ
                                                            ALL SCHEME RELATED DOCUMENTS CAREFULLY.</strong>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
