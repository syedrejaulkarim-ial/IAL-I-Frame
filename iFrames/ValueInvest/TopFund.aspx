<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.ValueInvest.TopFund" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Top Fund</title>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" media="all" />
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
</head>
<body>
    <form id="topform1" runat="server">
        <div>
            <table border="0" cellspacing="0" cellpadding="0" width="900" align="left" class="main-content">
                <tr>
                    <td>
                        <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="value_heading" colspan="2">
                                    <img src="img/arw1.jpg" />Top Funds</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="value_input">Rank</td>
                                                        <td>
                                                            <%--<select name="" class="value_input1">
                                                                <option>All</option>
                                                                <option>Top 5</option>
                                                                <option>Top 10</option>
                                                                <option>Top 15</option>
                                                            </select>--%>
                                                            <asp:DropDownList ID="ddlRank" runat="server" CssClass="value_input1">
                                                                <asp:ListItem Text="All" Value="1000" />
                                                                <asp:ListItem Text="Top 5" Value="5" />
                                                                <asp:ListItem Text="Top 10" Value="10" />
                                                                <asp:ListItem Text="Top 15" Value="15" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="top_td">
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="value_input">Type</td>
                                                        <td>
                                                            <%-- <select name="select" class="value_input1">
                                                                <option>All</option>
                                                                <option>Open Ended</option>
                                                            </select>--%>
                                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="value_input1">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="top_td">
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="value_input">Nature</td>
                                                        <td>
                                                            <%-- <select name="" class="value_input1">
                                                                <option>All</option>
                                                                <option>Equity</option>
                                                                <option>Debt</option>
                                                            </select>--%>
                                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="value_input1">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="top_td">
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="value_input">Period</td>
                                                        <td>
                                                            <%--<select name="" class="value_input1">
                                                                <option>Last 1 week</option>
                                                                <option>Last 1 month</option>
                                                            </select>--%>
                                                          <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="value_input1">
                                                    <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" Selected="True" />
                                                    <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                                                    <asp:ListItem Value="Per_91_Days" Text="Last 3 Month" />
                                                    <asp:ListItem Value="Per_182_Days" Text="Last 6 Month" />
                                                    <asp:ListItem Value="Per_1_Year" Text="Last 12 Month" />
                                                    <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                                                    <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />
                                                    <%--   <asp:ListItem Value="Since_Inception" Text="Since Inception" />--%>
                                                </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="top_td">
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td colspan="5">
                                                                        <%--<input name="" type="button" class="value_button2" value="Submit" />&nbsp;
                                                                        <input name="" type="button" class="value_reset" value="Reset" />--%>

                                                                        <asp:Button ID="btnGo" runat="server" Text="Submit" OnClick="btnGo_Click" class="value_button_top" />&nbsp;
                                                            <%--<input id="btnReset"  type="button" class="top_button" value="<< Reset" onclick="Javascript:document.forms[0].reset();" />--%>
                                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnRestClick" class="value_reset" />

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr class="top_td">
                                                        <td class="top" colspan="2">&nbsp;
                  
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <%--<tr>
                                                        <td class="valuef_heading1">Top 5 Funds -- Period (Last 1 Week)</td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td class="valuef_heading1" style="border:none">
                                                            <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%--<table width="90%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                <tr class="value_tableheader">
                                                                    <td class="value_tableheader">Rank</td>
                                                                    <td class="value_tableheader">Scheme Name</td>
                                                                    <td class="value_tableheader">Nature</td>
                                                                    <td class="value_tableheader">Date</td>
                                                                    <td class="value_tableheader">NAV</td>
                                                                    <td class="value_tableheader">Return</td>
                                                                    <td class="value_tableheader">SI</td>
                                                                    <tr>
                                                                        <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                        <td class="value_tablerow">Mar 19, 2013</td>
                                                                        <td class="value_tablerow">Debt</td>
                                                                        <td class="value_tablerow">Nov 19, 2013</td>
                                                                        <td class="value_tablerow">10.222</td>
                                                                        <td class="value_tablerow">5.04</td>
                                                                        <td class="value_tablerow">0.64</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                        <td class="value_tablerow">Mar 19, 2013</td>
                                                                        <td class="value_tablerow">Debt</td>
                                                                        <td class="value_tablerow">Nov 19, 2013</td>
                                                                        <td class="value_tablerow">10.222</td>
                                                                        <td class="value_tablerow">5.04</td>
                                                                        <td class="value_tablerow">0.64</td>
                                                                    </tr>
                                                            </table>--%>
                                                            <asp:ListView ID="lstResult"  runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                                                <LayoutTemplate>
                                                                    <table width="100%" border="0" class="value_table" cellpadding="0" cellspacing="0"  border-collapse: collapse;">
                                                                        <tr class="value_tableheader">
                                                                            <th align="left" class="value_tableheader">
                                                                                <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank"
                                                                                    Text="Rank" />
                                                                            </th>
                                                                            <th align="left" class="value_tableheader">
                                                                                <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                                                                    Text="Scheme Name" />
                                                                            </th>
                                                                            <th align="left" class="value_tableheader">
                                                                                <asp:Label ID="lnkNature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                                                    Text="Nature" />
                                                                            </th>
                                                                            <th align="left" class="value_tableheader">
                                                                                <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date"
                                                                                    Text="Date" />
                                                                            </th>
                                                                            <th align="left" class="value_tableheader">
                                                                                <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                                                    Text="NAV" />
                                                                            </th>
                                                                            <th align="left" class="value_tableheader">
                                                                                <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="<%=ddlPeriod.SelectedValue%>"><%=ddlPeriod.SelectedItem.Text%></asp:Label>
                                                                            </th>
                                                                            <th align="left" class="value_tableheader">
                                                                                <asp:Label ID="lnkInception" runat="server" SkinID="lblHeader" CommandName="Sort"
                                                                                    CommandArgument="Inception" Text="Since Inception" />
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                    <div style="padding-top: 5px;">
                                                                    </div>
                                                                    <div>
                                                                        <table>
                                                                            <tr >
                                                                                <td class="value_input" style="width:90%">
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
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                        <td class="value_tablerow">
                                                                            <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                                                        </td>
                                                                        <td class="value_tablerow">
                                                                            <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("sch_name")%>' />
                                                                        </td>
                                                                        <td class="value_tablerow">
                                                                            <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                                                        </td>
                                                                        <td class="value_tablerow">
                                                                            <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Calculation_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                                                        </td>
                                                                        <td class="value_tablerow">
                                                                            <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                                                        </td>
                                                                        <td class="value_tablerow">
                                                                            <%# Convert.ToDouble(Eval(ddlPeriod.SelectedValue)).ToString("n3") %>
                                                                        </td>
                                                                        <td class="value_tablerow">
                                                                            <%# Convert.ToDouble(Eval("Since_Inception")).ToString("n3")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    Data not Found
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="value_dis">Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing. Past performance of the schemes do not indicate the future performance. 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="value_btm_text">Developed and Maintained by: ICRA Analytics Ltd </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="value_btm_text">&nbsp; </td>
                            </tr>

                        </table>

                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>

</html>
