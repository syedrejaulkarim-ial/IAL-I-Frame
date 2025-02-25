<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PramericaSIP.aspx.cs" Inherits="iFrames.Pages.PramericaSIP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/dhtmlgoodies_calendar.js" type="text/javascript"></script>
    <style>
        .style1
        {
            width: 84%;
            font-family: Verdana;
            font-size: 11px;
            border: 2px solid #3abfef;
            height: 53px;
            margin-left: 8%;
            margin-right: 8%;
            top: -50px;
            left: -70px;
        }
        .trstyle
        {
            border: solid 1px #3abfef;
            border-bottom-color: #3abfef;
            border-right-width: 1px;
            border-top-color: #3abfef;
            border-top-width: 1px;
            border-bottom-width: 1px;
            border-right-color: #3abfef;
            border-left-color: #3abfef;
            border-left-width: 1px;
            font-weight: bold;
        }
        .tdstyle1
        {
            padding-left: 12px;
            text-align: left;
            width: 20%;
            border-color: #3abfef;
            border-bottom-color: #3abfef;
        }
        .tdstyle2
        {
            padding-left: 12px;
            text-align: left;
            width: 30%;
            border-color: #3abfef;
            border-bottom-color: #3abfef;
        }
        .tdstyle11
        {
            padding-left: 12px;
            text-align: left;
            width: 22%;
            border-color: #3abfef;
            border-bottom-color: #3abfef;
        }
        .tdstyle22
        {
            padding-left: 12px;
            text-align: left;
            width: 34%;
            border-color: #3abfef;
            border-bottom-color: #3abfef;
        }
        .style3
        {
            padding-left: 12px;
            text-align: left;
            width: 20%;
            border-color: #3abfef;
            height: 30px;
        }
        .style4
        {
            text-align: left;
            width: 30%;
            border-color: #3abfef;
            height: 30px;
        }
        .style5
        {
            width: 33%;
            text-align:left;            
            padding-left:13px;
            height:25px;
        }
        .trheight
        {
            height:25px;
        }
    </style>
    <script type="text/javascript">
        function date_validate() {
            var sipStartdata = document.getElementById("StartDate").value;
            var sipEnddata = document.getElementById("EndDate").value;
            var sipAmunt = document.forms(0).Amount.value;

            if (sipStartdata == "" || sipEnddata == "" || sipAmunt == "") {
                if (sipStartdata == "") {
                    alert("Start Date cannot be Blank");
                    document.forms(0).StartDate.focus();
                    return false;
                }
                else if (sipEnddata == "") {
                    alert("End Date cannot be Blank");
                    document.forms(0).EndDate.focus();
                    return false;
                }
                else {
                    alert("Amount cannot be Blank.");
                    document.forms(0).Amount.focus();
                    return false;
                }
            }


            if (isNaN(sipAmunt)) {
                alert("Please enter Numeric value.");
                document.getElementById("Amount").value = "";
                document.forms(0).Amount.focus();
                return false;
            }


            var boolVal = IsValidDate(sipStartdata, sipEnddata);
            if (boolVal == false)
                return false;

            var boolchk = asonDatechk(sipEnddata);
            if (boolchk == false)
                return false;




        }

        function asonDatechk(str1) {
            //debugger;
            var dt = new Date();
            var Day = parseInt(str1.substring(0, 2), 10);
            var Mn = parseInt(str1.substring(3, 5), 10);
            var Yr = parseInt(str1.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dtt = new Date(DateVal);
            var i = DateDiff.inDays(dtt, dt)
            if (i <= 0) {
                alert("End Date should be Less than today.");
                document.getElementById("EndDate").value = "";
                document.forms(0).EndDate.focus();
                return false;
            }
            return true;
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


            if (dt >= dt1) {
                alert("End Date should be Greater than the Start Date");
                document.getElementById("EndDate").value = "";
                document.forms(0).EndDate.focus();
                return false;
            }
            //debugger;

            var startsensexdate = new Date("01/02/1980");
            if (dt < startsensexdate) {
                alert("Please select Start Date after 1st January 1980");
                document.getElementById("StartDate").value = "";
                document.forms(0).StartDate.focus();
                return false;

            }

        }

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

    </script>
</head>
<body>
    <br />
    <form id="form1" runat="server">
    <%--<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Convert the excel into XML" />--%>
    <div id="prAmricaDiv">
        <table class="style1" cellspacing="0px" cellpadding="4px">
            <thead>
                <tr class="trstyle">
                    <td colspan="4" style="background-color: #3abfef; color: White; text-align: center;
                        vertical-align: middle; padding-top: 0px; height: 25px; font-size: 13px;">
                        <b>SIP CALCULATOR </b>
                    </td>
                </tr>
            </thead>
            <tr class="trstyle">
                <td colspan="4" style="height: 16px; vertical-align: middle; border-color: #3abfef;">
                    <hr style="background-color: #3abfef;" />
                </td>
            </tr>
            <tr class="trstyle">
                <td class="tdstyle1" >
                    <b>Start Date</b>
                </td>
                <td  class="tdstyle2">
                    <asp:TextBox ID="StartDate" class="txtboxstyle" runat="server" onfocus="displayCalendar(document.forms[0].StartDate,'dd/mm/yyyy',this)"></asp:TextBox>
                </td>
                <td class="tdstyle1" >
                    <b>End Date</b>
                </td>
                <td class="tdstyle2">
                    <asp:TextBox ID="EndDate" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].EndDate,'dd/mm/yyyy',this)"></asp:TextBox>
                </td>
            </tr>
            <tr class="trstyle">
                <td class="tdstyle1">
                    <b>Investment Amount (Rs.)</b>
                </td>
                <td class="tdstyle2" colspan="3">
                    <asp:TextBox ID="Amount" runat="server" class="txtboxstyle" MaxLength="15"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr class="trstyle">
                <td colspan="4" class="tdstyle1" style="vertical-align: bottom">
                    <asp:Button ID="Button3" runat="server" Text=" Calculate Sum " OnClick="sipCalcBtn_Click"
                        OnClientClick="return date_validate();" BackColor="#3abfef" ForeColor="White" />
                </td>
            </tr>
            <tr class="trstyle" id="showSensexDiv" runat="server">
                <td colspan="4" style="height: 10px;">
                    <div >
                        <table style="width: 100%;font-family: Verdana; font-size: 11px; " cellspacing="0px" cellpadding="0">
                        <thead align="center" style =" background-color:#3abfef; font-weight: bold; color: White;font-size: 13px; text-align: center; "   > <tr><td class="style5">
                                </td>
                                <td class="style5">
                                    Date
                                </td>
                                <td class="style5">
                                    Sensex
                                </td> </tr></thead>
                            <%--<tr>
                                <td colspan="4" style="height: 16px; vertical-align: middle;">
                                    <hr />
                                </td>
                            </tr>--%>
                            <tr class="trheight">
                                <td class="style5">
                                    SIP Date (First Investment)
                                </td>
                                <td class="style5">
                                    <asp:Label ID="frstSensexdate" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="style5">
                                    <asp:Label ID="frstSensexValue" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr class="trheight">
                                <td class="style5">
                                    Exit Date
                                </td>
                                <td class="style5">
                                    <asp:Label ID="lastSensexdate" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="style5">
                                    <asp:Label ID="lastSensexValue" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <%--<asp:ListView ID="PRSensexlistview" runat="server">
            <LayoutTemplate>
                <table class="style1" cellpadding="4">
                    <tr style="background-color: #3abfef; font-weight: bold; color: White; text-align: center;
                        font-size: small">                        
                        <td>
                            Sensex Date
                        </td>
                        <td>
                           Sensex Value
                        </td>
                    </tr>
                    <tr id="itemPlaceholder" align="center" runat="server" class="tdstyle1">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AltRow" : "ListRow" %>'>
                    
                    <td align="center">
                        <%#Eval("SensexDate").ToString()%>
                    </td>
                    <td align="center">
                        <%#Eval("SensexValue").ToString()%>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                No data Found
            </EmptyDataTemplate>
        </asp:ListView>--%>
                    </div>
                </td>
            </tr>
            <tr class="trstyle"  id="compareDiv" runat="server">
                <td colspan="4" style="height: 10px;">
                    <div >
                        <table style="width: 100%;" cellspacing="0px">                          
                            <thead style =" background-color:#3abfef; font-weight: bold; color: White; text-align: center;font-size: 13px;  margin-top:10px; margin-bottom:10px; " >
                            <tr style=" height:25px; " >
                                <td class="tdstyle22">
                                </td>
                                <td class="tdstyle11">
                                Date
                                </td>
                                <td class="tdstyle11" >
                                    Index Return
                                </td>
                                <td class="tdstyle11" >
                                    Bank FD Return
                                </td>
                            </tr>
                            </thead>
                            <tr class="trheight">

                                <td class="tdstyle22">
                                SIP Total Investment Amount                                    
                                </td>
                                <td class="tdstyle11">
                                    <asp:Label ID="investedAmountStartDate" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="tdstyle11">
                                    <asp:Label ID="investedAmountSx" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="tdstyle11">
                                    <asp:Label ID="investedAmountFd" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr class="trheight">
                                <td class="tdstyle22">
                                Final Value
                                    
                                </td>
                                <td class="tdstyle11">
                                  <asp:Label ID="investedAmountLastDate" runat="server" Text=""></asp:Label>  
                                </td>
                                <td class="tdstyle11">
                                    <asp:Label ID="returnAmountSx" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="tdstyle11">
                                    <asp:Label ID="returnAmountFd" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr class="trheight" >
                                
                                <td class="tdstyle22">
                                    Returns (CAGR)(with Expenses)
                                </td>
                                <td class="tdstyle11">
                                </td>
                                <td class="tdstyle11">
                                    <asp:Label ID="returnSensexCagr" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="tdstyle11">
                                    <asp:Label ID="returnFdCagr" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            </table>
    </div>
    <div id="showResultDiv" runat="server">
        <asp:ListView ID="PRIndexListView" runat="server">
            <LayoutTemplate>
                <table class="style1" cellpadding="4">
                    <tr style="background-color: #3abfef; font-weight: bold; color: White; text-align: center;
                        font-size: small">
                        <td >
                            Dates
                        </td>
                        <td >
                            SIP Total Investment
                        </td>
                        <td >
                            Value of Investment(Sensex)
                        </td>
                        <td >
                            Value of Investment(FD)
                        </td>
                    </tr>
                    <tr id="itemPlaceholder" align="center" runat="server" class="tdstyle1">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AltRow" : "ListRow" %>'>
                    <td align="left" style="padding-left:40px;" >
                        <%# Convert.ToDateTime(Eval("INVESTED_DATE")).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                    </td>
                    <td align="center">
                        <%# (Eval("INVESTED_AMNT") == DBNull.Value) ? "--" : Convert.ToInt64(Eval("INVESTED_AMNT")).ToString()%>
                    </td>
                    <td align="center">
                        <%# (Eval("SIP_CMPD_AMNT") == DBNull.Value) ? "--" : Convert.ToInt64(Eval("SIP_CMPD_AMNT")).ToString()%>
                    </td>
                    <td align="center">
                        <%# (Eval("FD_SIP_CMPD_AMNT") == DBNull.Value) ? "--" : Convert.ToInt64(Eval("FD_SIP_CMPD_AMNT")).ToString()%>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                No data Found
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
    </form>
</body>
</html>
