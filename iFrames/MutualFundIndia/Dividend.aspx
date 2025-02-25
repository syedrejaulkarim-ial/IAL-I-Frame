<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="Dividend.aspx.cs"
    Inherits="iFrames.MutualFundIndia.Dividend" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <title>Dividend</title>
    <meta name="viewport" content="width=device-width,initial-scale=1">

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />

    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/new/bootstrap-datepicker.css" />

    <%--<script src="js/jquery-1.9.1.js" type="text/javascript"></script>--%>
    <script src="js/jquery-ui.js" type="text/javascript"></script>

    <script src="js/new/new/jquery_new.min.js"></script>
    <script src="js/new/new/bootstrap.min.js"></script>
    <script src="js/new/new/bootstrap-datepicker.js"></script>

    <script type="text/javascript">

        function ValidateControl() {

            var selectedFund = $('#<%=ddlFundHouse.ClientID %>').find(':selected').val();
            if (selectedFund == 0) {
                alert('Please select any Fund House.');
                $('#<%=ddlFundHouse.ClientID %>').focus();
                return false;
            }

            var selectedValue = $('#<%=listboxSchemeName.ClientID%> option:selected').val();
            if (selectedValue == -1) {
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

            //if (!dtvalid(Date.parse($('#txtfromDate').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
            //    alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
            //    $('#txtfromDate').focus();
            //    return false;
            //}

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

          <%-- if (!dtvalid(Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }--%>

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
            <%--$('#<%=txtfromDate.ClientID %>').datepicker({
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
            });--%>
            var StartDate = new Date('01/01/1901');

            $('#txtfromDate').datepicker({
                format: 'dd M yyyy',
                autoclose: true,
                startDate: StartDate,
                endDate: '-2d'
                //startDate: '-3d'
            });



            $('#txtToDate').datepicker({
                format: 'dd M yyyy',
                autoclose: true,
                startDate: StartDate,
                endDate: '-2d'
                //startDate: '-3d'
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
            //$('#<%=txtToDate.ClientID %>').datepicker().datepicker('setDate', todate);
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
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
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
        .ui-datepicker-trigger {
            position: absolute;
            top: 25px;
            right: 20px;
        }
    </style>
</head>
<body>
    <form runat="server" id="form1">
        <div class="card">
            <div class="card-body">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="row">


                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                            <label>Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                CssClass="form-control form-control-sm">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                            <label>Sub-Category</label>
                            <asp:DropDownList ID="ddlSubCategory" runat="server"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged"
                                CssClass="form-control form-control-sm">
                            </asp:DropDownList>
                        </div>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="row">
                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                            <label>Mutual Fund Name</label>
                            <asp:DropDownList ID="ddlFundHouse" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                CssClass="form-control form-control-sm">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                            <label>Scheme Name</label>
                            <asp:DropDownList ID="listboxSchemeName" runat="server"
                                OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                CssClass="form-control form-control-sm">
                            </asp:DropDownList>
                        </div>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="row">
                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                            <label>From Date</label>
                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                            <label>To Date</label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-7 col-lg-7">
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-5 col-lg-5 text-right pr-0">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit"
                                OnClientClick="Javascript:return ValidateControl();"
                                OnClick="btnSubmit_Click" CssClass="btn btn-primary" Style="margin-right: 10px" />
                            <asp:Button ID="btnReset" class="btn btn-light"
                                runat="server" Text="Reset" OnClick="btnReset_Click" />
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-3">
                    <div class="row">
                        <asp:ListView ID="listvwDividendDetail" runat="server"
                            OnPagePropertiesChanging="lst_PagePropertiesChanging">
                            <LayoutTemplate>
                                <table width="100%" border="0" align="left" cellpadding="0"
                                    cellspacing="0" class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>
                                                <asp:Label ID="lnkSchName"
                                                    runat="server" SkinID="lblHeader" CommandName="Sort"
                                                    CommandArgument="SchName" Text="Scheme Name" />
                                            </th>
                                            <th style="text-align: center">
                                                <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader"
                                                    CommandName="Sort" CommandArgument="Date"
                                                    Text="Record Date" />
                                            </th>
                                            <th style="text-align: center">
                                                <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader"
                                                    CommandName="Sort" CommandArgument="Dividend" Text="Dividend (%)" />
                                            </th>
                                        </tr>
                                    </thead>

                                    <tr id="itemPlaceholder" runat="server"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="">
                                        <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("Sch_Short_Name")%>' />
                                    </td>
                                    <td class="" style="white-space: nowrap; text-align: center">
                                        <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Record_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                    </td>
                                    <td class="" style="white-space: nowrap; text-align: center">
                                        <%#  Convert.ToDouble(Eval("Div_Ind")).ToString("n2") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                Data not Found
                            </EmptyDataTemplate>
                        </asp:ListView>
                        <asp:DataPager ID="dpDividend" runat="server" PageSize="10"
                            PagedControlID="listvwDividendDetail">
                            <Fields>
                                <asp:NextPreviousPagerField ShowFirstPageButton="True"
                                    ShowNextPageButton="False" />
                                <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                <asp:NextPreviousPagerField ShowLastPageButton="True"
                                    ShowPreviousPageButton="False" />
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
                    </div>
                </div>
                <div>
                    <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-3">
                    <div class="row">
                        <div style="font-size: 10px; color: #A7A7A7">
                            Disclaimer: Mutual Fund investments are subject to
                                        market risks. Read all scheme related documents carefully
                                        before investing. Past performance of the schemes
                                        do not indicate the future performance.
                        </div>
                        <%--<div style="font-size: 10px; color: #A7A7A7;" class="pt-1">
                                                        Developed and Maintained by: <a href="https://www.icraanalytics.com"
                                                            target="_blank" style="font-size: 10px; color: #999999">
                                                        ICRA Analytics Ltd</a>
                                                    </div>--%>
                        <div style="width: 100%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                            Developed by: <a href="https://www.icraanalytics.com"
                                target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999"
                                    href="https://icraanalytics.com/home/Disclaimer"
                                    target="_blank">Disclaimer </a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>

