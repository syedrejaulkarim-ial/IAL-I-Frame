<%@ Page Title="" Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="Dividend.aspx.cs" Inherits="iFrames.BajajCapital.Dividend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>

<head>
    <title>Dividend Declared</title>
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="css/template_css.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
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


        $(function () {
            $('#<%=txtfromDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "img/calenderb.jpg",
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -2
            });

            $('#<%=txtToDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
            });

            $('#btnReset').click(function (ev) {
                window.location.reload(true);
            });

            setDates();

            // $('#<%=txtfromDate.ClientID %>').val(Date.parse('today').add(-8).days().toString("dd MMMM yyyy"));
            // $('#<%=txtToDate.ClientID %>').val(Date.parse('today').add(-2).days().toString("dd MMM yyyy"));

        });



        function setDates() {
            var dateOffset = (24 * 60 * 60 * 1000);
            var myDate = new Date();
            var frmdate = new Date(myDate.getTime() - dateOffset * 8);
            var todate = new Date(myDate.getTime() - dateOffset * 2);

            //$('#<%=txtfromDate.ClientID %>').datepicker().datepicker('setDate', frmdate);
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
        body {
            font-family: 'Open Sans', sans-serif !important;
        }

        select:not(.ui-datepicker-month):not(.ui-datepicker-year) {
            width: 470px !important;
            margin-bottom: 16px;
        }

        input, textarea, .uneditable-input {
            width: 457px;
        }

        .ui-datepicker-trigger {
            margin-left: 430px;
        }

        .ui-datepicker-trigger {
            position: relative;
            top: 3px;
        }

        .news_header {
            background: rgb(251, 201, 0) !important;
        }

        #lstResult_dpTopFund {
            float: right;
            margin: 10px;
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

        .top_inputa, .top_inputb {
            white-space: nowrap;
            position: relative;
            top: 5px;
            text-align: left;
            padding-right: 15px;
            padding-left: 15px;
        }

        .tr_txtbox {
            padding: 0;
        }

        input[type="text"] {
            margin-bottom: 16px;
        }

        #dpDividend a {
            background: rgb(165, 139, 34);
            padding: 4px 10px;
            border-radius: 5px;
            cursor: pointer;
            text-decoration: none;
            color: #ecebeb;
            font-size: 12px;
        }

        #dpDividend .news_header {
            background: rgb(88, 75, 19) !important;
            padding: 4px 10px;
            border-radius: 5px;
            cursor: pointer;
            text-decoration: none;
            color: #ecebeb;
            font-size: 12px;
        }

        #dpDividend {
            float: right;
            margin: 0px 0 25px;
        }

        a:link {
            color: #0088cc;
        }
    </style>

</head>

<body>
    <form runat="server" id="form1">
        <%--<div>
   
    Fund name
    <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged">
    </asp:DropDownList>
    <br />
    Schmename:
    <asp:ListBox ID="listboxSchemeName" runat="server" SelectionMode="Single" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
        Height="138px"></asp:ListBox>
    <br />
    From Date :
    <asp:TextBox ID="txtfromDate" runat="server" Style="width: 90px;" Font-Name="Vardana"></asp:TextBox>
    &nbsp; To Date :
    <asp:TextBox ID="txtToDate" runat="server" Style="width: 90px;" Font-Name="Vardana"> </asp:TextBox>
    <br />
    <asp:Button ID="btnSubmit" runat="server" Text="Button" OnClientClick="Javascript:return ValidateControl();"
        OnClick="btnSubmit_Click" />
    <br />
    

     </div>--%>
        <table width="650" border="0" align="left" cellpadding="0" cellspacing="0" class="table-bordered">
            <tr>
                <td style="border-bottom: 1px solid #ddd; padding: 5px 10px;">
                    <table>
                        <tr>
                            <td class="top_icon">
                                <img src="img/dividend_dec.png" width="31" height="32" />
                            </td>
                            <td class="top_title">Dividend Declared
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" class="top_line">
                                <table width="95%" border="0" align="left" cellpadding="0" cellspacing="0">
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
                                                CssClass="top_inputdivi">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="top_inputa">Scheme Name
                                        </td>
                                        <td>
                                            <%--<asp:ListBox ID="listboxSchemeName" runat="server" SelectionMode="Single" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                            CssClass="top_input3"></asp:ListBox>--%>
                                            <asp:DropDownList ID="listboxSchemeName" runat="server" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                                CssClass="top_inputdivi">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="top_inputb">From Date
                                        </td>
                                        <td class="tr_txtbox">
                                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="top_txtbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td class="ui-datepicker-trigger"></td>--%>
                                        <td class="top_inputb">To Date
                                        </td>
                                        <td class="tr_txtbox">
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="top_txtbox"></asp:TextBox>
                                        </td>
                                        <%--<td class="ui-datepicker-trigger"></td>--%>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="text-align: right">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="Javascript:return ValidateControl();"
                                                OnClick="btnSubmit_Click" CssClass="top_button" Style="margin-left: 156px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <table width="93%" style="margin-left: 15px;" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="top_text">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListView ID="listvwDividendDetail" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                    <LayoutTemplate>
                                        <table border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered ss">
                                            <tr class="top_tableheader">
                                                <th align="left">
                                                    <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                                        Text="Scheme Name" />
                                                </th>
                                                <th align="left" style="white-space: nowrap">
                                                    <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date"
                                                        Text="Record Date" />
                                                </th>
                                                <th align="left" style="white-space: nowrap">
                                                    <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Dividend"
                                                        Text="Dividend (%)" />
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "top_tablerow" : "top_tablerow" %>'>
                                            <td class="top_tablerow">
                                                <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("Sch_Short_Name")%>' />
                                            </td>
                                            <td class="top_tablerow" style="white-space: nowrap">
                                                <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Record_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                            </td>
                                            <td class="top_tablerow" style="text-align: center">
                                                <%#  Convert.ToDouble(Eval("Div_Ind")).ToString("n2") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        Data not Found
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="padding-top: 5px;"></div>
                                <asp:DataPager ID="dpDividend" runat="server" PageSize="10" PagedControlID="listvwDividendDetail">
                                    <Fields>
                                        <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                                        <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                        <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                                        <asp:TemplatePagerField>
                                            <PagerTemplate>
                                                <span style="padding-left: 15px; text-align: right">Page
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
                            </td>
                        </tr>
                        <tr>
                            <td class="disclaimerh"></td>
                        </tr>
                        <tr>
                            <td class="disclaimer" style="text-align: right; padding: 0 0 5px 0px;">
                                <small style="text-align: right" class="rslt_text1">Developed for Bajaj Capital by:<a
                                    class="text" href="https://icraanalytics.com" target="_blank"> ICRA Analytics Ltd,</a>
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
