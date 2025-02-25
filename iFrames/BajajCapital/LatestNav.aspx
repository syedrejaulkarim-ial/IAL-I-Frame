<%@ Page Title="" Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="LatestNav.aspx.cs" Inherits="iFrames.BajajCapital.LatestNav" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>

<head>
    <title>Latest Nav</title>
    <link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" />
    <script src="js/jqueryNew.js" type="text/javascript"></script>
    <%--    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>--%>
    <script src="js/navigation.js" type="text/javascript"></script>
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
            font-size: 80% !important;
        }

        .ui-datepicker-trigger {
            position: relative;
            top: 5px;
        }

        .top_inputa {
            position: relative;
            top: -5px;
            width: 20px;
        }

        .table tr td, .table tr th {
            vertical-align: middle;
            padding: 3px 10px;
        }

        #abc td {
            border: 0;
            white-space: nowrap;
            top: 6px;
            position: relative
        }

        #abc.table.table-bordered {
            border: 0 !important;
        }

            #abc.table.table-bordered td {
                border: 0;
                padding-bottom: 0;
                padding-top: 0;
            }

        .btn_theme {
            background: #E77817;
            color: #fff;
            padding: 3px 10px;
            border: 0;
            border-radius: 5px;
        }

        .top_inputa, .top_inputd {
            text-align: right !important;
        }

        select {
            width: 100% !important;
        }

        .top_icon, .top_title {
            border: 0 !important;
        }

        select[multiple], select[size] {
            height: 150px;
        }

        .top_tableheader {
            background: rgb(251, 201, 0) !important
        }
        .sc tr > td{
            border-bottom:1px solid #ddd;
        }
    </style>
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="frm" runat="server">
        <table width="650" border="0" class="table-bordered">
            <tr>
                <td style="border-bottom: 1px solid #eaeaea; padding: 5px 10px;">
                    <table border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="top_icon">
                                <img src="img/Latest_NAV.png" width="25" height="31" />
                            </td>
                            <td class="top_title">&nbsp;Latest NAV
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="abc" class="table">
                        <tr>
                            <td colspan="2" class="top_line">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="top_inputa">Fund House
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                                CssClass="top_input3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right" class="top_inputa">Scheme Name
                                        </td>
                                        <td>
                                            <asp:ListBox ID="lbSchemeName" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="lbSchemeName_SelectedIndexChanged"
                                                CssClass="top_inputddl"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr class="top_td">
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGo" runat="server" Text="Submit" OnClick="btnGo_Click" CssClass="btn_theme" Style="float: right" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="border: 0">
                    <table class="table text-right" style="margin-bottom: 0;width: 98%;margin-left: 5px;">
                        <tr>
                            <td style="border: 0">
                                <asp:ListView ID="lstResult" runat="server">
                                    <LayoutTemplate>
                                        <table width="100%" class="table table-bordered sc">
                                            <tr class="top_tableheader">
                                                <th align="left" rowspan="2">
                                                    <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                                        Text="Scheme Name" />
                                                </th>
                                                <th align="left" rowspan="2">
                                                    <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date"
                                                        Text="Date" />
                                                </th>
                                                <th align="left" rowspan="2">
                                                    <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                        Text="NAV(Rs.)" />
                                                </th>
                                                <th colspan="3" style="text-align: center; border-bottom: 1px solid #eae3e3;">

                                                    <asp:Label ID="lbl" runat="server">Performance % as on  <%=System.DateTime.Now.AddDays(-1).ToString("MMM dd, yyyy")%> </asp:Label>
                                                </th>


                                            </tr>
                                            <tr class='top_tableheader'>
                                                <th align="left">
                                                    <asp:Label ID="Last3Month" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Last3Month"
                                                        Text="Last 3 Month" Style="white-space: nowrap" />
                                                </th>
                                                <th align="left">
                                                    <asp:Label ID="Last6Month" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Last6Month"
                                                        Text="Last 6 Month" Style="white-space: nowrap" />
                                                </th>
                                                <th align="left">
                                                    <asp:Label ID="Last1year" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Last1year"
                                                        Text="Last 1 year" Style="white-space: nowrap" />
                                                </th>
                                            </tr>


                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                        <div style="padding-top: 5px;">
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblSchName" Text='<%#(Eval("Sch_Short_Name") == DBNull.Value) ? "--" : Eval("Sch_Short_Name").ToString()%>' />
                                            </td>

                                            <td style="white-space:nowrap">
                                                <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Nav_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label1" Text='<%#Eval("Per_91_Days")!= DBNull.Value?(Math.Round(Convert.ToDouble(Eval("Per_91_Days")),2)).ToString():"NA"%>' />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label2" Text='<%#Eval("Per_182_Days")!= DBNull.Value?(Math.Round(Convert.ToDouble(Eval("Per_182_Days")),2)).ToString():"NA"%>' />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label3" Text='<%#Eval("Per_1_Year")!= DBNull.Value?(Math.Round(Convert.ToDouble(Eval("Per_1_Year")),2)).ToString():"NA"%>' />
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
                            <td style="padding-left: 20px;text-align: right;padding: 10px 25px 5px;border: 0;">
                                <small>Developed for Bajaj Capital by:<a
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
