<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="swpCalculator.aspx.cs"
    Inherits="iFrames.Pages.swpCalculator" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.14.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript"> 
        $.datepicker.setDefaults({ dateFormat: "dd/mm/yy" });
        $(function () {
            var date = new Date();
            var currentMonth = date.getMonth();
            var currentDate = date.getDate();
            var currentYear = date.getFullYear();  

            $('#txtfrmDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //$('#txtfrmDate').datepicker('option', 'dateFormat', 'dd/mm/yy');

            $('#txtToDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //$('#txtToDate').datepicker('option', 'dateFormat', 'dd/mm/yy');

            $('#txtAsOn').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //$('#txtAsOn').datepicker('option', 'dateFormat', 'dd/mm/yy');
        });
    </script>
    <style type="text/css">
        .headWidth
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
    <table cellpadding="1" cellspacing="1" width="550px">
        <tr class="content">
            <td class="headWidth">
                <b>Scheme Name: </b>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ddlSchemeName" runat="server" Width="400px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddwscname_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="content">
            <td class="headWidth">
                <strong>Benchmark</strong>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ddlBenchMark" runat="server" Width="400px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="content">
            <td class="headWidth">
                <strong>Initial Amount (Rs.)</strong>
            </td>
            <td style="width: 200px">
                <asp:TextBox ID="txtinstallAmt" runat="server" Width="94px"></asp:TextBox>
            </td>
            <td style="width: 100px">
                <strong>Period</strong>
            </td>
            <td style="width: 300px">
                <asp:DropDownList ID="ddlPeriod" runat="server" Width="157px">
                    <%--   <asp:ListItem>Quaterly</asp:ListItem>  
      <asp:ListItem>Monthly</asp:ListItem>--%>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="content">
            <td class="headWidth">
                <b>From Date</b>
            </td>
            <td>
                <asp:TextBox ID="txtfrmDate" runat="server" Width="142px"></asp:TextBox>
                <%-- <cc1:CalendarExtender ID="frmDate" runat="server" TargetControlID="txtfrmDate" PopupPosition="BottomLeft"
                    Format="dd/MM/yyyy">
                </cc1:CalendarExtender>--%>
            </td>
            <td>
                <b>To Date</b>
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" Width="150px"></asp:TextBox>
                <%--<cc1:CalendarExtender ID="toDate" runat="server" TargetControlID="txtToDate" PopupPosition="BottomLeft"
                    Format="dd/MM/yyyy">
                </cc1:CalendarExtender>--%>
            </td>
        </tr>
        <tr class="content">
            <td class="headWidth">
                <b>SWP Date</b>
            </td>
            <td>
                <asp:DropDownList ID="ddlShipDate" runat="server" Width="143px">
                    <asp:ListItem Value="7">7th</asp:ListItem>
                    <asp:ListItem Value="10">10th</asp:ListItem>
                    <asp:ListItem Value="15">15th</asp:ListItem>
                    <asp:ListItem Value="25">25th</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <b>As on Date</b>
            </td>
            <td>
                <asp:TextBox ID="txtAsOn" runat="server" Width="150px"></asp:TextBox>
                <%--<cc1:CalendarExtender ID="txtToDate0_CalendarExtender" runat="server" TargetControlID="txtAsOn"
                    PopupPosition="BottomLeft" Format="dd/MM/yyyy">
                </cc1:CalendarExtender>--%>
            </td>
        </tr>
        <tr class="content">
            <td class="headWidth">
                <b>Withdrawal Amount</b>
            </td>
            <td>
                <asp:TextBox ID="txtTranAmt" runat="server" MaxLength="14" CssClass="txtfield_input"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Style="margin-left: 85px;" Text="Calculate"
                    OnClick="btnSubmit_Click" ValidationGroup="vgSIP" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:ValidationSummary runat="server" ID="vsSIP" ValidationGroup="vgSIP" />
                <asp:CustomValidator ID="cv1" runat="server" ErrorMessage="Please Select scheme"
                    ControlToValidate="ddlSchemeName" ClientValidationFunction="SchemeCheck" Display="None"
                    Font-Size="Smaller" ValidationGroup="vgSIP" SetFocusOnError="true"></asp:CustomValidator>
                <asp:RequiredFieldValidator ID="rfv4" ControlToValidate="txtinstallAmt" runat="server"
                    Display="None" ErrorMessage="Please enter amount" SetFocusOnError="True" Font-Size="Smaller"
                    ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtfrmDate"
                    runat="server" Display="None" ErrorMessage="Please select from date" SetFocusOnError="True"
                    Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtToDate"
                    runat="server" Display="None" ErrorMessage="Please select to date" SetFocusOnError="True"
                    Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtAsOn"
                    runat="server" Display="None" ErrorMessage="Please select as on date" SetFocusOnError="True"
                    Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtTranAmt"
                    runat="server" Display="None" ErrorMessage="Please enter transfer amount" SetFocusOnError="True"
                    Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CVwithDrow" runat="server" ErrorMessage="Withdrawal Amount can't be grater then Installment Amount"
                    Font-Size="X-Small" ForeColor="#FF3300" ControlToCompare="txtinstallAmt" ControlToValidate="txtTranAmt"
                    Operator="LessThanEqual" Type="Double" ValidationGroup="vgSIP"></asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator3" runat="server" Display="Static" ErrorMessage="To date can not be Less than From date"
                    ControlToCompare="txtfrmDate" ControlToValidate="txtToDate" Type="Date" Operator="GreaterThanEqual"
                    Font-Size="Smaller" ValidationGroup="vgSIP"></asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator4" runat="server" Display="Static" ErrorMessage="As on date can not be Less than To date"
                    ControlToCompare="txtToDate" ControlToValidate="txtAsOn" Type="Date" Operator="GreaterThanEqual"
                    Font-Size="Smaller" ValidationGroup="vgSIP"></asp:CompareValidator>
                <asp:RegularExpressionValidator ID="REVToDate0" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                    runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                    SetFocusOnError="True" ControlToValidate="txtfrmDate" Font-Size="Smaller"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="REVToDate" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                    runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                    SetFocusOnError="True" ControlToValidate="txtToDate" Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="PNRswpval" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                    runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                    SetFocusOnError="True" ControlToValidate="txtAsOn" Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:ListView ID="LstSWP" runat="server" ItemPlaceholderID="itemPlaceHolder1" Style="margin-top: 106px">
                    <LayoutTemplate>
                        <table border="1" cellpadding="2" style="border-collapse: collapse;" width="550px">
                            <tr class="ListtableHead">
                                <th>
                                    Date
                                </th>
                                <th>
                                    NAV
                                </th>
                                <%--<th style="visibility:collapse">
                                    Investment Amount
                                </th>--%>
                                <th>
                                    Units
                                </th>
                                <th>
                                    Cumulative Units
                                </th>
                                <th>
                                    Cumulative Amount
                                </th>
                                <th>
                                    Cash Flow
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
                            <%--<td align="center" style="font-size: small;visibility:collapse" width="15%">
                                <%# Eval("InvestAmount").ToString()%>
                            </td>--%>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("Unit").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("CumulativeUnits").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="10%">
                                <%# Eval("CumulativeAmount").ToString()%>
                            </td>
                            <td align="center" style="font-size: small" width="15%">
                                <%# Eval("CashFlow").ToString()%>
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
