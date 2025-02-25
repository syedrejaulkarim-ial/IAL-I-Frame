<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="LatestNav.aspx.cs" Inherits="iFrames.ValueInvest.LatestNav" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Latest Nav</title>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
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
</head>
<body>
    <form id="frm" runat="server">
        <table border="0" cellspacing="0" cellpadding="0" width="900" align="left" class="main-content">
            <tr>
                <td>
                    <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="value_heading" colspan="2">
                                <img src="img/arw1.jpg" />Latest NAV</td>

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
                                                    <td class="value_input">Fund House</td>
                                                    <td>
                                                        <%--<select name="" class="value_input1">
                                                            <option>Axis Mutual Fund</option>
                                                            <option>Baroda Pioneer Balance Fund</option>
                                                            <option>Birla Sun Life 95- Dividend</option>
                                                        </select>--%>
                                                                 <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                            CssClass="value_input1">
                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_input">Scheme Name </td>
                                                    <td>
                                                        <%--<select name="" class="value_input1">
                                                            <option>Axis Banking Debt Fund - Direct - Dly Dividend</option>
                                                            <option>Baroda Pioneer Balance Fund - Direct - Dividend</option>
                                                            <option>Birla Sun Life 95- Dividend</option>
                                                        </select>--%>
                                                        <asp:ListBox ID="lbSchemeName" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="lbSchemeName_SelectedIndexChanged"
                                            CssClass="value_input1" style="height:100px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">

                                                            <tr>
                                                                <td colspan="5">
                                                                    <%--<input name="" type="button" class="value_button" value="Submit" />--%>
                                                                    <asp:Button ID="btnGo" runat="server" Text="Submit" OnClick="btnGo_Click" CssClass="value_button" />
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
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<table width="90%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                            <tr class="value_tableheader">
                                                                <td class="value_tableheader">Scheme Name</td>
                                                                <td class="value_tableheader">NAV Date</td>
                                                                <td class="value_tableheader">NAV</td>
                                                                <td class="value_tableheader">Last 3 Months</td>
                                                                <td class="value_tableheader">Last 6 Months</td>
                                                                <td class="value_tableheader">Last 1 Year</td>
                                                                <tr>
                                                                    <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                    <td class="value_tablerow">Mar 19, 2013</td>
                                                                    <td class="value_tablerow">1132.7608</td>
                                                                    <td class="value_tablerow">3.3989</td>
                                                                    <td class="value_tablerow">1132.7608</td>
                                                                    <td class="value_tablerow">1132.7608</td>
                                                                </tr>
                                                            <tr>
                                                                <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                                <td class="value_tablerow">Mar 19, 2013</td>
                                                                <td class="value_tablerow">1132.7608</td>
                                                                <td class="value_tablerow">3.3989</td>
                                                                <td class="value_tablerow">1132.7608</td>
                                                                <td class="value_tablerow">1132.7608</td>
                                                            </tr>
                                                        </table>--%>

                                                        <asp:GridView Width="100%" runat="server"  ID="gvNavDetail" AutoGenerateColumns="false" class="value_table">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Scheme Name"  HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%#(Eval("Sch_Short_Name") == DBNull.Value) ? "--" : Eval("Sch_Short_Name").ToString()%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nav Date" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%# Convert.ToDateTime(Eval("Nav_Date")).ToString("dd MMM yyyy")%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nav" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%# Eval("Nav").ToString()  %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Last 3 Month" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%# Eval("Per_91_Days").ToString()%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Last 6 Month" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%# Eval("Per_182_Days").ToString()%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Last 1 year" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%# Eval("Per_1_Year").ToString()%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                Data Not Found
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
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

    </form>
</body>
</html>
