<%@ Page Title="" Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="HistNav.aspx.cs" Inherits="iFrames.BajajCapital.HistNav" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>

<head>
    <title>Historical Nav</title>
    <link href="css/template_css.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" />
    <script src="js/jqueryNew.js" type="text/javascript"></script>
    <script src="js/navigation.js" type="text/javascript"></script>

    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/date.js"></script>
    <script type="text/javascript" language="javascript">

        function ValidateControl() {

            var selectedFund = $('#<%=ddlFundHouse.ClientID %>').find(':selected').val();
            if (selectedFund == 0) {
                alert('Please select any Fund House.');
                $('#<%=ddlFundHouse.ClientID %>').focus();
                return false;
            }

            var selectedValue = $('#<%=listboxSchemeName.ClientID%> option:selected').val();
            if (selectedValue == null) {
                alert('Please select any Scheme.');
                $('#<%=listboxSchemeName.ClientID %>').focus();
                return false;
            }


            var regex = new RegExp("^[0-9]{2} (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) [0-9]{4}$", "i");

            if ($('#<%=txtfromDate.ClientID %>').val() == '') {
                alert('Please enter From date.');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if ($('#<%=txtToDate.ClientID %>').val() == '') {
                alert('Please enter To date.');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            if (!$('#<%=txtfromDate.ClientID %>').val().match(regex)) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }
            if (Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!dtvalid(Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!$('#<%=txtToDate.ClientID %>').val().match(regex)) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }
            if (Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            if (!dtvalid(Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            var frmdate = converterdate($('#<%=txtfromDate.ClientID %>').val());
            var todate = converterdate($('#<%=txtToDate.ClientID %>').val());


            if (!IsValidDate(frmdate, todate)) {
                alert("From Date should be Less than To Date ");
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }


            if (!LessThanToday(frmdate)) {
                alert("From Date should be Less than Today ");
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!LessThanToday(todate)) {
                alert("To Date should be Less than Today ");
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }





            return true;
        }

        function GetMinDate() {

            var ddScheme = document.getElementById('SIPSchDt');
            var allotdate = ddScheme.value;
            var Day = parseInt(allotdate.substring(0, 2), 10);
            var Mn = parseInt(allotdate.substring(3, 5), 10);
            var Yr = parseInt(allotdate.substring(6, 10), 10);
            var DateVal = Day + "/" + Mn + "/" + Yr;
            var dt = new Date(DateVal);
            return dt;
        }
        $(function () {


            $('#<%=txtfromDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "img/calenderb.jpg",
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                //setDate: "-1 d",
                maxDate: -1,
                minDate: GetMinDate()
            });

            $('#<%=txtToDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                //setDate: '-1d',
                maxDate: -1,
                minDate: GetMinDate()
            });

            $('#btnReset').click(function (ev) {
                window.location.reload(true);
            });

            //setDates();

            //            $('#<%=txtfromDate.ClientID %>').val(Date.parse('today').add(-8).days().toString("dd MMM yyyy"));
            //            $('#<%=txtToDate.ClientID %>').val(Date.parse('today').add(-2).days().toString("dd MMM yyyy"));
        });

        function setDates() {
            var dateOffset = (24 * 60 * 60 * 1000);
            var myDate = new Date();
            var frmdate = new Date(myDate.getTime() - dateOffset * 8);
            var todate = new Date(myDate.getTime() - dateOffset * 2);

            $('#<%=txtfromDate.ClientID %>').datepicker().datepicker('setDate', frmdate);
            $('#<%=txtToDate.ClientID %>').datepicker().datepicker('setDate', todate);
        }
    </script>
    <script type="text/javascript">
        var dminyear = 1900;
        var dmaxyear = 2200;
        var chsep = "/";
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

        function checkinteger(str1) {
            var x;
            for (x = 0; x < str1.length; x++) {
                // verify current character is number or not !   
                var cr = str1.charAt(x);
                if (((cr < "0") || (cr > "9")))
                    return false;
            }
            return true;
        }
        function getcharacters(s, chsep1) {
            var x;
            var Stringreturn = "";
            for (x = 0; x < s.length; x++) {
                var cr = s.charAt(x);
                if (chsep.indexOf(cr) == -1)
                    Stringreturn += cr;
            }
            return Stringreturn;
        }
        function februarycheck(cyear) {
            return (((cyear % 4 == 0) && ((!(cyear % 100 == 0)) || (cyear % 400 == 0))) ? 29 : 28);
        }
        function finaldays(nr) {
            for (var x = 1; x <= nr; x++) {
                this[x] = 31
                if (x == 4 || x == 6 || x == 9 || x == 11) {
                    this[x] = 30
                }
                if (x == 2) {
                    this[x] = 29
                }
            }
            return this
        }

        function converterdate(string) {
            var d = string.split(/[ :\s]/);
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'sep', 'Oct', 'Nov', 'Dec'];
            var monthval = 1;

            for (var i = 0; i < months.length; i++) {
                if (months[i] == d[1]) {
                    monthval = parseInt(i + 1);
                    break;
                }
            }

            if (monthval < 10) {
                monthval = '0' + monthval;
            }

            return d[0] + '/' + monthval + '/' + d[2];
        }

        function dtvalid(strdate) {
            var monthdays = finaldays(12)
            var cpos1 = strdate.indexOf(chsep)
            var cpos2 = strdate.indexOf(chsep, cpos1 + 1)
            var daystr = strdate.substring(0, cpos1)
            var monthstr = strdate.substring(cpos1 + 1, cpos2)
            var yearstr = strdate.substring(cpos2 + 1)
            strYr = yearstr
            if (strdate.charAt(0) == "0" && strdate.length > 1) strdate = strdate.substring(1)
            if (monthstr.charAt(0) == "0" && monthstr.length > 1) monthstr = monthstr.substring(1)
            for (var i = 1; i <= 3; i++) {
                if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
            }
            // The parseInt is used to get a numeric value from a string   
            pmonth = parseInt(monthstr)
            pday = parseInt(daystr)
            pyear = parseInt(strYr)
            if (cpos1 == -1 || cpos2 == -1) {
                //alert("The date format must be : dd/mm/yyyy")
                return false
            }
            if (monthstr.length < 1 || pmonth < 1 || pmonth > 12) {
                //alert("Input a valid month")
                return false
            }
            if (daystr.length < 1 || pday < 1 || pday > 31 || (pmonth == 2 && pday > februarycheck(pyear)) || pday > monthdays[pmonth]) {
                //alert("Input a valid day")
                return false
            }
            if (yearstr.length != 4 || pyear == 0 || pyear < dminyear || pyear > dmaxyear) {
                //alert("Input a valid 4 digit year between " + dminyear + " and " + dmaxyear)
                return false
            }
            if (strdate.indexOf(chsep, cpos2 + 1) != -1 || checkinteger(getcharacters(strdate, chsep)) == false) {
                //alert("Input a valid date")
                return false
            }
            return true
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


        function LessThanToday(strDate) {
            var Day = parseInt(strDate.substring(0, 2), 10);
            var Mn = parseInt(strDate.substring(3, 5), 10);
            var Yr = parseInt(strDate.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);

            var todaydate = new Date();
            //alert(todaydate);alert(dt);
            var i = DateDiff.inDays(dt, todaydate)
            if (i <= 0) {
                return false;
            }
            return true;
        }
    </script>
    <style type="text/css">
        form {
            margin: 0;
        }

        body {
            font-family: 'Open Sans', sans-serif !important;
            font-size: 80% !important;
        }

        .top_input_nav {
            width: 470px !important;
        }

        input, textarea, .uneditable-input {
            width: 457px;
        }

        .ui-datepicker-trigger {
            margin-left: 430px;
        }

        .ui-datepicker-trigger {
            position: relative;
            top: -12px;
        }

        .news_header {
            background: rgb(251, 201, 0) !important;
        }

        #lstResult_dpTopFund {
            float: right;
            margin: 10px 20px 15px;
        }

            #lstResult_dpTopFund a {
                background: rgb(165, 139, 34);
                padding: 4px 10px;
                border-radius: 5px;
                cursor: pointer;
                text-decoration: none;
                color: #ecebeb;
                font-size: 12px;
            }

            #lstResult_dpTopFund .news_header {
                background: rgb(88, 75, 19) !important;
                padding: 4px 10px;
                border-radius: 5px;
                cursor: pointer;
                text-decoration: none;
                color: #ecebeb;
                font-size: 12px;
            }

        input[type="text"] {
            margin-bottom: 30px;
        }

        .top_inputb {
            white-space: nowrap;
            margin-right: 10px;
            vertical-align: top;
        }

        .tr_txtbox {
            padding: 0 0 20px;
        }

        body {
            background: #fff;
        }

        .top_button {
            background: #E77817;
            color: #fff;
            padding: 3px 10px;
            border: 0;
            border-radius: 5px;
        }

        .top_inputa {
            width: 130px;
            vertical-align: top;
        }

        .table-bordered td {
            border-left: 0px solid #dddddd;
        }

        .table-bordered.ss td {
            border-left: 1px solid #dddddd;
        }

        .top_tableheader {
            background: rgb(251, 201, 0) !important
        }

        select {
            margin-bottom: -3px;
        }

        input[type="text"] {
            margin-bottom: 16px;
        }

        .ui-datepicker-trigger {
            position: relative;
            top: 2px;
        }

        a:link {
            color: #0088cc;
        }
    </style>
</head>
<body>
    <form runat="server" id="form1">
        <asp:HiddenField ID="SIPSchDt" runat="server" />
        <table width="650" border="0" align="left" cellpadding="0" cellspacing="0" class="table-bordered">
            <tr>
                <td>
                    <table class="" style="border-bottom: 1px solid #eaeaea; width: 100%;">
                        <tr>
                            <td class="top_icon" style="padding: 6px 15px;">
                                <img src="img/history_nav.png" width="31" height="32" />
                                &nbsp;Historical Nav
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" class="top_line">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="margin-left: 20px">
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="top_inputa">Fund House
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                                CssClass="top_input_nav">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="top_td">
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="top_inputa">Scheme Name
                                        </td>
                                        <td>
                                            <%-- <asp:ListBox ID="listboxSchemeName" runat="server" SelectionMode="Single" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                            CssClass="top_input3"></asp:ListBox>--%>
                                            <asp:DropDownList ID="listboxSchemeName" runat="server" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                                CssClass="top_input_nav" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="top_td">
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="top_inputb">From Date
                                        </td>
                                        <td style="width: 127px;">
                                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="top_txtbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="top_inputb">To Date
                                        </td>
                                        <td style="width: 127px;">
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="top_txtbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td class="ui-datepicker-trigger"></td>--%>
                                        <td>&nbsp;</td>
                                        <td class="text-right">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="Javascript:return ValidateControl();"
                                                OnClick="btnSubmit_Click" CssClass="top_button" Style="margin-left: 157px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="top_text">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:GridView runat="server" ID="gvNavHistDetail" Width="95%" AutoGenerateColumns="false"
                                    CssClass="top_table" AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="10">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-CssClass="top_tableheader">
                                            <ItemTemplate>
                                                <%#              
                (Eval("Sch_Short_Name") == DBNull.Value) ? "--" : Eval("Sch_Short_Name").ToString()
                                                %>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="top_tablerow" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nav Date" HeaderStyle-CssClass="top_tableheader">
                                            <ItemTemplate>
                                                <%# Convert.ToDateTime(Eval("Nav_Date")).ToString("dd MMM yyyy")%>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="top_tablerow" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nav" HeaderStyle-CssClass="top_tableheader">
                                            <ItemTemplate>
                                                <%# Eval("Nav").ToString()  %>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="top_tablerow" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        Data Not Found
                                    </EmptyDataTemplate>
                                </asp:GridView>--%>

                                <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                    <LayoutTemplate>
                                        <table class="table table-bordered ss" style="width: 93%; margin: 0 auto">
                                            <tr class="top_tableheader">
                                                <th align="left">
                                                    <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                                        Text="Scheme Name" />
                                                </th>
                                                <th align="left">
                                                    <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date"
                                                        Text="Date" />
                                                </th>
                                                <th align="left">
                                                    <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                        Text="NAV" />
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                        <div style="padding-top: 5px;">
                                        </div>
                                        <asp:DataPager ID="dpTopFund" runat="server" PageSize="10" PagedControlID="lstResult">
                                            <Fields>
                                                <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                                                <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                                <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <span style="padding-left: 40px; text-align: right">Page
                                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                                            of
                                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                                            (
                                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                                            records)
                                                                <br />
                                                        </span>
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "top_tablerow" : "top_tablerow" %>'>
                                            <td>
                                                <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("Sch_Short_Name")%>' />
                                            </td>

                                            <td style="white-space:nowrap">
                                                <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Nav_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        Data not Found
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr>
                            <td class="disclaimerh" style="margin-left: 20px"></td>
                        </tr>
                        <tr>
                            <td class="disclaimer" style="padding-left: 20px; text-align: right; padding: 10px 25px 5px;">
                                <small style="text-align: right" class="rslt_text1">Developed for Bajaj Capital by:<a
                                    class="text" href="https://www.icraanalytics.com" target="_blank"> ICRA Analytics Ltd,</a>
                                    <a
                                        class="text" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer</a>
                                </small>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
