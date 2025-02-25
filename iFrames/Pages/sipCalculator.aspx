<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sipCalculator.aspx.cs"
    Inherits="iFrames.Pages.sipCalculator" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.14.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CheckDateEalier(sender, args) {
            if (sender._selectedDate > new Date()) {
                alert("You cannot select a day after today!");
                sender._selectedDate = new Date();
                // set the date back to the today
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
      
    </script>
    <script type="text/javascript">
        $.datepicker.setDefaults({dateFormat:"dd/mm/yy"});
        $(document).ready(function () {
            var date = new Date();
            var currentMonth = date.getMonth();
            var currentDate = date.getDate();
            var currentYear = date.getFullYear();

            $('#txtfrmDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
//            $('#txtfrmDate').datepicker('option', 'dateFormat', 'dd/mm/yy');
//            $('#txtfrmDate').datepicker('option', 'altFormat', 'dd/mm/yy');

            $('#txtToDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
//            $('#txtToDate').datepicker('option', 'dateFormat', 'dd/mm/yy');
//            $('#txtToDate').datepicker('option', 'altFormat', 'dd/mm/yy');

            $('#txtAsOn').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
//            $('#txtAsOn').datepicker('option', 'dateFormat', 'dd/mm/yy');
//            $('#txtAsOn').datepicker('option', 'altFormat', 'dd/mm/yy');
        });
    </script>
    <style type="text/css">
        .headingWidth
        {
            width: 150px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" CombineScripts="false">
    </cc1:ToolkitScriptManager>
    <script type="text/javascript">
        function SchemeCheck(source, args) {
            var ddlCountry = document.getElementById("<%=ddlSchemeName.ClientID%>");
            if (ddlCountry.selectedIndex <= 1)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
    </script>
    <table style="width: 550px;" cellpadding="0" cellspacing="0">
        <tr class="mainHeader">
            <td>
                SIP CALCULATOR
                <hr />
                <br style="line-height: 15px;" />
            </td>
        </tr>
        <tr>
            <td>
                <table width="550px" cellpadding="1" cellspacing="3" border="0">
                    <tr class="content">
                        <td class="headingWidth">
                            <strong>Scheme Name: </strong>
                        </td>
                        <td colspan="5" align="left">
                            <asp:DropDownList ID="ddlSchemeName" runat="server" OnSelectedIndexChanged="ddlSchemeName_SelectedIndexChanged"
                                Style="width: 400px;" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="content">
                        <td class="headingWidth">
                            <strong>Benchmark</strong>
                        </td>
                        <td colspan="5" align="left">
                            <asp:DropDownList ID="ddlBenchMark" runat="server" Style="width: 400px;">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="content">
                        <td class="headingWidth">
                            <strong>Installment Amount</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="txtinstallAmt" runat="server" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <strong>Period</strong>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriod" runat="server" Width="80px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 80px;">
                            <strong>Sip Date</strong>
                        </td>
                        <td align="right">
                            <asp:DropDownList ID="ddlShipDate" runat="server" Width="86px">
                                <asp:ListItem Value="7">7th</asp:ListItem>
                                <asp:ListItem Value="10">10th</asp:ListItem>
                                <asp:ListItem Value="15">15th</asp:ListItem>
                                <asp:ListItem Value="25">25th</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="content">
                        <td valign="top" class="headingWidth">
                            <b>From Date</b>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtfrmDate" runat="server" Width="80px"></asp:TextBox>
                            <%--<cc1:CalendarExtender ID="frmDate" runat="server" TargetControlID="txtfrmDate" PopupPosition="BottomLeft"
                                 PopupButtonID="Image1" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>--%>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td valign="top">
                            <b>To Date</b>
                        </td>
                        <td valign="top" align="right">
                            <asp:TextBox ID="txtToDate" runat="server" Width="80px"></asp:TextBox>
                            <%--<cc1:CalendarExtender ID="toDate" runat="server" TargetControlID="txtToDate" PopupPosition="BottomLeft"
                                 OnClientDateSelectionChanged="CheckDateEalier"  Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>--%>
                            <br />
                        </td>
                    </tr>
                    <tr class="content">
                        <td valign="top" class="headingWidth">
                            <b>As on Date</b>
                        </td>
                        <td valign="top" colspan="2">
                            <asp:TextBox ID="txtAsOn" runat="server" Width="80px"></asp:TextBox>
                            <%--<cc1:CalendarExtender ID="txtToDate0_CalendarExtender" runat="server" TargetControlID="txtAsOn"
                                PopupPosition="BottomLeft"  OnClientDateSelectionChanged="CheckDateEalier"  Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>--%>
                            <br />
                        </td>
                        <td align="right" colspan="3">
                            <asp:Button ID="btnShow" runat="server" Style="height: 18px;" OnClick="btnShow_Click"
                                Text="Calculate" ValidationGroup="vgSIP" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="content" align="center">
                            <asp:ValidationSummary runat="server" ID="vsSIP" ValidationGroup="vgSIP" />
                            <asp:CustomValidator ID="cv1" runat="server" ErrorMessage="Please Select scheme"
                                ControlToValidate="ddlSchemeName" ClientValidationFunction="SchemeCheck" Display="None"
                                Font-Size="Smaller" ValidationGroup="vgSIP" SetFocusOnError="true"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfv4" ControlToValidate="txtinstallAmt" runat="server"
                                Display="None" ErrorMessage="Please enter amount" SetFocusOnError="True" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfv1" ControlToValidate="txtfrmDate" runat="server"
                                Display="None" ErrorMessage="Please select from date" SetFocusOnError="True"
                                Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfv2" ControlToValidate="txtToDate" runat="server"
                                Display="None" ErrorMessage="Please select to date" SetFocusOnError="True" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfv3" ControlToValidate="txtAsOn" runat="server"
                                Display="None" ErrorMessage="Please select as on date" SetFocusOnError="True"
                                Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" Display="Static" ErrorMessage="To date can not be Less than From date"
                                ControlToCompare="txtfrmDate" ControlToValidate="txtToDate" ForeColor="Red" Type="Date"
                                Operator="GreaterThanEqual" Font-Size="Smaller" ValidationGroup="vgSIP" SetFocusOnError="true"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator4" runat="server" ForeColor="Red" Display="Static"
                                ErrorMessage="As on date can not be Less than To date" ControlToCompare="txtToDate"
                                ControlToValidate="txtAsOn" Type="Date" Operator="GreaterThanEqual" Font-Size="Smaller"
                                SetFocusOnError="true" ValidationGroup="vgSIP"></asp:CompareValidator>
                            <asp:RegularExpressionValidator ID="REVToDate0" ForeColor="Red" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                                runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                                SetFocusOnError="True" ControlToValidate="txtfrmDate" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="REVToDate" ForeColor="Red" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                                runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                                SetFocusOnError="True" ControlToValidate="txtToDate" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="PNRswpval" ForeColor="Red" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                                runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                                SetFocusOnError="True" ControlToValidate="txtAsOn" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:ListView ID="LstSipCal_DateRange" runat="server" ItemPlaceholderID="itemPlaceHolder1"
                    Style="margin-top: 106px">
                    <LayoutTemplate>
                        <table border="1" cellpadding="2" style="border-collapse: collapse;" width="550px;">
                            <tr class="ListtableHead">
                                <th>
                                    Date
                                </th>
                                <th>
                                    NAV
                                </th>
                                <th>
                                    Cum. Units
                                </th>
                                <th>
                                    Cash Flow
                                </th>
                                <th>
                                    Amount
                                </th>
                                <th>
                                    SIP Value
                                </th>
                                <th>
                                    Cum Index Units
                                </th>
                            </tr>
                            <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                            <td align="center" style="font-size: small" width="15%">
                                <%# shortDate(Convert.ToDateTime(Eval("Date")))%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("Nav_Rs").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("Units").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("Cash_Flow").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("Amount").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="10%">
                                <%# Eval("SIP_Value").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("ind_unit").ToString()%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <div id="divReturnValue_Range" visible="False" runat="server">
                    <table>
                        <tr class="content">
                            <td>
                                <strong>Initial Value:</strong>
                                <asp:Label ID="lblInitialValue_Range" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <strong>Scheme Return:</strong>
                                <asp:Label ID="lblReturnValue_Range" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Current Value:</strong>
                                <asp:Label ID="lblCurrentValue_Range" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <strong>Benchmark Return: </strong>
                                <asp:Label ID="lblBenchMrkRtn" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:ListView ID="lstReturn" runat="server" ItemPlaceholderID="itemPlaceHolder1"
                    Style="margin-top: 106px">
                    <LayoutTemplate>
                        <table border="1" cellpadding="2" style="border-collapse: collapse;" width="550px;">
                            <tr class="ListtableHead">
                                <th>
                                    Period
                                </th>
                                <th>
                                    SIP start date
                                </th>
                                <th>
                                    Total Amount Invest
                                </th>
                                <th>
                                    Scheme Market Value
                                </th>
                                <th>
                                    Scheme SIP return
                                </th>
                                <th>
                                    Benchmark Market Value
                                </th>
                                <th>
                                    Benchmark SIP Return
                                </th>
                            </tr>
                            <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("Period")%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# shortDate(Convert.ToDateTime(Eval("SipStartDate")))%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("TotAmtInvest").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("SchMarketVal").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("SchSipRtn").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="10%">
                                <%# Eval("BenchMarkVal").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("BenchMarkSipRtn").ToString()%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
