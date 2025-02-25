<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.BajajCapital.TopFund" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>

<head>
    <title>Top fund</title>

    <script src="js/jqueryNew.js" type="text/javascript"></script>
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
            font-size:80% !important;
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
            border-left: 0px solid #dddddd !important;
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


        #dpDividend {
            float: right;
            margin: 0px 0 25px;
        }

        a:link {
            color: #0088cc;
        }

        .btn_theme {
            background: #E77817;
            color: #fff;
            padding: 3px 10px;
            border: 0;
            border-radius: 5px;
        }



        #lstResult_dpTopFund {
            float: right;
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

        .table td {
            padding: 5px;
        }

        .top_line.s table tr td:first-child {
            width: 120px;
            position: relative;
            top: -10px;
        }

        .sss td {
            border-left: 1px solid #dddddd !important;
            border-right: 1px solid #dddddd !important;
        }
    </style>
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" />
</head>
<body>
    <form id="topform1" runat="server">        
               <table width="650" border="0" align="left" cellpadding="0" cellspacing="0" class="table-bordered" 
                    <tr>
                        <td style="border-bottom: 1px solid #eaeaea; width: 100%;padding: 3px 17px;">
                            <table>
                                <tr>
                                    <td class="top_icon">
                                        <img src="img/top_fund.png" width="29" height="30" />
                                    </td>
                                    <td class="top_title">Top Funds
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="margin-left:20px">
                                <tr>
                                    <td colspan="2" class="top_line s">
                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Rank</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlRank" runat="server" CssClass="top_input1">
                                                        <asp:ListItem Text="All" Value="1000" />
                                                        <asp:ListItem Text="Top 5" Value="5" />
                                                        <asp:ListItem Text="Top 10" Value="10" />
                                                        <asp:ListItem Text="Top 15" Value="15" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Type</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="top_input1">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Nature</td>
                                                    <td>
                                                   
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="top_input1">
                                                    </asp:DropDownList>
                                                </td>
                                               </tr>
                                            <tr>
                                                    <td>Period</td>
                                                    <td>
                                                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="top_input1">
                                                        <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" Selected="True" />
                                                        <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                                                        <asp:ListItem Value="Per_91_Days" Text="Last 3 Month" />
                                                        <asp:ListItem Value="Per_182_Days" Text="Last 6 Month" />
                                                        <asp:ListItem Value="Per_1_Year" Text="Last 12 Month" />
                                                        <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                                                        <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />
                                                        <asp:ListItem Value="Since_Inception" Text="Since Inception" />
                                                    </asp:DropDownList>
                                                </td>
                                                </tr>
                                            <tr>
                                                <td align="right" colspan="5" style="position: relative;left: -58px;top: 5px;">
                                                    <asp:Button ID="btnGo" runat="server" Text="Submit" OnClick="btnGo_Click" class="btn_theme" />&nbsp;
                                                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnRestClick" class="btn_theme" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
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
                            <table id="TblTopFund" runat="server" visible="false" border="0" align="left" cellpadding="0" cellspacing="0" class="table" style="border-bottom: 0;width: 91%;margin-left: 20px;">
                                <tr>
                                    <td class="top_text">
                                        <asp:Label ID="lbtopText"  runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 0; border: 0;">
                                        <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                            <LayoutTemplate>
                                                <table class="sss" width="100%" border="0" cellpadding="3" cellspacing="3" style="border-color: Black; border-collapse: collapse; border-bottom: 1px solid #ddd;">
                                                    <tr class="top_tableheader">
                                                        <th align="left">
                                                            <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank"
                                                                Text="Rank" />
                                                        </th>
                                                        <th align="left">
                                                            <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                                                Text="Scheme Name" />
                                                        </th>
                                                        <th align="left">
                                                            <asp:Label ID="lnkNature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                                Text="Nature" />
                                                        </th>
                                                        <th align="left">
                                                            <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date"
                                                                Text="Date" />
                                                        </th>
                                                        <th align="left">
                                                            <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                                Text="NAV" />
                                                        </th>
                                                        <th align="left" style="white-space:nowrap">
                                                            <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="<%=ddlPeriod.SelectedValue%>"><%=ddlPeriod.SelectedItem.Text%></asp:Label>
                                                        </th>
                                                        <th align="left" style="white-space:nowrap">
                                                            <asp:Label ID="lnkInception" runat="server" SkinID="lblHeader" CommandName="Sort"
                                                                CommandArgument="Inception" Text="Since Inception" />
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
                                                        <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("sch_name")%>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Calculation_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                                    </td>
                                                    <td>
                                                        <%#Math.Round(Convert.ToDouble(Eval(ddlPeriod.SelectedValue)),2).ToString() %>
                                                    </td>
                                                    <td>
                                                        <%#Math.Round(Convert.ToDouble(Eval("Since_Inception")),2).ToString()%>
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
            
        </tr>
        
                            </table>
                        </td>
                    </tr>
                   <tr>
            <td class="disclaimer" style="padding-left: 20px; text-align: right; padding: 10px 25px 5px;">
                <small style="text-align: right" class="rslt_text1">Developed for Bajaj Capital by:<a
                                            class="text" href="https://www.icraanalytics.com" target="_blank"> ICRA Analytics Ltd,</a>
                                            <a
                                            class="text" href="https://icraanalytics.com/home/Disclaimer" target="_blank"> Disclaimer</a>
                                        </small>
            </td>
        </tr>
                </table>
            
    </form>
</body>
</html>

