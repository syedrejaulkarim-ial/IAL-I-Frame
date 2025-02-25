<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stpCalculator.aspx.cs"
    Inherits="iFrames.Pages.stpCalculator" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.14.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CheckDateEalier(sender, args) {
            if (sender._selectedDate > new Date()) {
                alert("You cannot select a day after today!");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>
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

            $("#tabs").tabs();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager CombineScripts="false" ID="ScriptManager1" runat="server">
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
    <script type="text/javascript">
        function SchemeCheckFrom(source, args) {
            var ddlCountry = document.getElementById("<%=ddlSchemeNameTrnFrm.ClientID%>");
            if (ddlCountry.selectedIndex <= 1)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
    </script>
    <table id="tblSTP" cellspacing="1" cellpadding="1" width="600px" border="1" runat="server">
        <tr>
            <td>
                <table id="Table5" width="600px" cellspacing="1" cellpadding="1" border="0" border="0">
                    <tr class="content">
                        <td style="height: 13px;" valign="top">
                            <b>Transfer From</b>
                        </td>
                        <td style="height: 13px" valign="top" colspan="3" align="left">
                            <asp:DropDownList ID="ddlSchemeNameTrnFrm" runat="server" Width="400px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="content">
                        <td valign="top">
                            <b>Transfer To</b>
                        </td>
                        <td style="height: 13px;" valign="top" align="left" colspan="3">
                            <asp:DropDownList ID="ddlSchemeName" runat="server" Width="400px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlSchemeName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="content">
                        <td style="width: 150px">
                            <b>Benchmark</b>
                        </td>
                        <td align="left" colspan="3">
                            <asp:DropDownList ID="ddlBenchMark" runat="server" CssClass="scheme_txtfield" Width="400px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="content">
                        <td style="width: 150px;">
                            <b>Initial Amount</b>
                        </td>
                        <td style="width: 150px; height: 25px;" align="left">
                            <asp:TextBox ID="txtinstallAmt" Width="100px" runat="server" MaxLength="14" CssClass="txtfield_input"></asp:TextBox>
                        </td>
                        <td style="width: 140px;">
                            <b>Transfer Amount</b>
                        </td>
                        <td style="width: 150px; height: 25px;">
                            <asp:TextBox ID="txtTranAmt" runat="server" Width="100px" MaxLength="14" CssClass="txtfield_input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="content">
                        <td valign="top" style="width: 150px;">
                            <b>From Date</b>
                        </td>
                        <td style="width: 150px; height: 25px;" align="left">
                            <asp:TextBox ID="txtfrmDate" runat="server" Width="100px"></asp:TextBox>
                            <%--<P>Date: <INPUT id=datepicker type=text></P>

                            <cc1:CalendarExtender ID="frmDate" runat="server" TargetControlID="txtfrmDate" PopupPosition="BottomLeft"
                                PopupButtonID="Image1" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>--%>
                        </td>
                        <td style="width: 140px;" valign="top">
                            <b>To Date</b>
                        </td>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
                            <%--<cc1:CalendarExtender ID="toDate1" runat="server" TargetControlID="txtToDate" PopupPosition="BottomLeft"
                                OnClientDateSelectionChanged="CheckDateEalier" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>--%>
                        </td>
                    </tr>
                    <tr class="content">
                        <td valign="top">
                            <b>As On Date</b>
                        </td>
                        <td style="width: 150px" align="left">
                            <asp:TextBox ID="txtAsOn" runat="server" Width="100px"></asp:TextBox>
                            <%--<cc1:CalendarExtender ID="txtToDate0_CalendarExtender" runat="server" TargetControlID="txtAsOn"
                                PopupPosition="BottomLeft" OnClientDateSelectionChanged="CheckDateEalier" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>--%>
                        </td>
                        <td style="width: 140px;" valign="top">
                            <asp:Label ID="Label6" runat="server" CssClass="txt2"> <b>Period</b></asp:Label>
                        </td>
                        <td style="width: 150px" valign="top">
                            <asp:DropDownList ID="ddlPeriod" runat="server" Width="100px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="content">
                        <td valign="top">
                            <b>STP Date</b>
                        </td>
                        <td style="width: 150px" valign="top" align="left">
                            <asp:DropDownList ID="ddlShipDate" runat="server" Width="100px">
                                <asp:ListItem Value="7">7th</asp:ListItem>
                                <asp:ListItem Value="10">10th</asp:ListItem>
                                <asp:ListItem Value="15">15th</asp:ListItem>
                                <asp:ListItem Value="25">25th</asp:ListItem>
                                <asp:ListItem Value="-1">Last day of month</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 150px" valign="top">
                        </td>
                        <td style="width: 150px" valign="top">
                            <asp:Button ID="btnCal" runat="server" CssClass="bttn01" OnClick="btnCal_Click" Text="Calculate"
                                ValidationGroup="vgSIP" />
                        </td>
                    </tr>
                    <tr class="content">
                        <td colspan="4" align="center">
                            <asp:ValidationSummary runat="server" ID="vsSIP" ValidationGroup="vgSIP" />
                            <asp:CustomValidator ID="cv1" runat="server" ErrorMessage="Please Select scheme"
                                ControlToValidate="ddlSchemeNameTrnFrm" ClientValidationFunction="SchemeCheckFrom"
                                Display="None" Font-Size="Smaller" ValidationGroup="vgSIP" SetFocusOnError="true"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv2" runat="server" ErrorMessage="Please Select scheme"
                                ControlToValidate="ddlSchemeName" ClientValidationFunction="SchemeCheck" Display="None"
                                Font-Size="Smaller" ValidationGroup="vgSIP" SetFocusOnError="true"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfv4" ControlToValidate="txtinstallAmt" runat="server"
                                Display="None" ErrorMessage="Please enter amount" SetFocusOnError="True" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtTranAmt"
                                runat="server" Display="None" ErrorMessage="Please enter transfer amount" SetFocusOnError="True"
                                Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtfrmDate"
                                runat="server" Display="None" ErrorMessage="Please select from date" SetFocusOnError="True"
                                Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtToDate"
                                runat="server" Display="None" ErrorMessage="Please select to date" SetFocusOnError="True"
                                Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtAsOn"
                                runat="server" Display="None" ErrorMessage="Please select as on date" SetFocusOnError="True"
                                Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="REVToDate0" ForeColor="Red" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                                runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                                SetFocusOnError="True" ControlToValidate="txtfrmDate" Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ForeColor="Red" ID="REVToDate" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                                runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                                SetFocusOnError="True" ControlToValidate="txtToDate" Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ForeColor="Red" ID="PNRswpval" ValidationExpression="[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}"
                                runat="server" Display="None" ErrorMessage="Please Select valid date from Calendar "
                                SetFocusOnError="True" ControlToValidate="txtAsOn" Font-Size="Smaller" ValidationGroup="vgSIP"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ForeColor="Red" ID="CompareValidator3" runat="server"  Display="None" 
                                ErrorMessage="To date can not be Less than From date" ControlToCompare="txtfrmDate"
                                ControlToValidate="txtToDate" Type="Date" Operator="GreaterThanEqual" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:CompareValidator>
                            <asp:CompareValidator ForeColor="Red" ID="CompareValidator4" runat="server"  Display="None" 
                                ErrorMessage="As on date can not be Less than To date" ControlToCompare="txtToDate"
                                ControlToValidate="txtAsOn" Type="Date" Operator="GreaterThanEqual" Font-Size="Smaller"
                                ValidationGroup="vgSIP"></asp:CompareValidator>
                            <asp:CompareValidator ForeColor="Red" ID="CVwithDrow" runat="server" ErrorMessage="Withdrawal Amount can't be grater then Installment Amount"
                                Font-Size="X-Small" ControlToCompare="txtinstallAmt" ControlToValidate="txtTranAmt" Display="None" 
                                Operator="LessThanEqual" Type="Double" ValidationGroup="vgSIP"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="demo" runat="server" id="tcResults" visible="false">
                                <div id="tabs">
                                    <ul>
                                        <li><a href="#tabs-1">Flex STP</a></li>
                                        <li><a href="#tabs-2">Regular STP</a></li></ul>
                                    <div id="tabs-1">
                                        <div>
                                            <div id="div_FromScheme" runat="server" visible="false">
                                                <asp:ListView ID="lst_FromScheme" runat="server" ItemPlaceholderID="itemPlaceHolder1"
                                                    Style="margin-top: 106px; font-size: 8">
                                                    <LayoutTemplate>
                                                        <table border="1" cellpadding="1" style="border-collapse: collapse;" width="600px">
                                                            <tr class="content">
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblFromSchName" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="ListtableHead">
                                                                <th>
                                                                    Date
                                                                </th>
                                                                <th>
                                                                    NAV
                                                                </th>
                                                                <th>
                                                                    Unit
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
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("Unit").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulativeUnits").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulativeAmount").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CashFlow").ToString()%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                            <div id="div_Swp" runat="server" visible="false">
                                                <asp:ListView ID="LstSwp" runat="server" ItemPlaceholderID="itemPlaceHolder1" Style="margin-top: 106px;
                                                    font-size: 8">
                                                    <LayoutTemplate>
                                                        <table border="1" cellpadding="2" style="border-collapse: collapse;" width="600px">
                                                            <tr class="content">
                                                                <td colspan="11">
                                                                    <asp:Label ID="lblToSchSwp" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="ListtableHead">
                                                                <th>
                                                                    Date
                                                                </th>
                                                                <th>
                                                                    NAV
                                                                </th>
                                                                <th>
                                                                    Monthly Return
                                                                </th>
                                                                <%--<th>
                                                                Monthly Target Value
                                                            </th>--%>
                                                                <th>
                                                                    Market Value
                                                                </th>
                                                                <th>
                                                                    Incr amount
                                                                </th>
                                                                <th>
                                                                    Amt Invest
                                                                </th>
                                                                <th>
                                                                    Cumul Amt Invested
                                                                </th>
                                                                <th>
                                                                    Unit Brought
                                                                </th>
                                                                <th>
                                                                    Cumul Units Bought
                                                                </th>
                                                                <th>
                                                                    Net Avg Cost Per Unit
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
                                                                <%# Eval("MonReturn").ToString()%>
                                                            </td>
                                                            <%--<td align="center" style="font-size: small" width="15%">
                                                            <%# Eval("MonTargetVal").ToString()%>
                                                        </td>--%>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("MarketValue").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="10%">
                                                                <%# Eval("IncrAmt").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("AmtInvest").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulAmtInvested").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("UnitBrought").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulUnitsBought").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("NetAvgCostPerUnit").ToString()%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tabs-2">
                                        <div>
                                            <div id="Div_FromSchemeReg" runat="server" visible="false">
                                                <asp:ListView ID="lstFromSchemeReg" runat="server" ItemPlaceholderID="itemPlaceHolder1"
                                                    Style="margin-top: 106px; font-size: 8">
                                                    <LayoutTemplate>
                                                        <table border="1" cellpadding="2" style="border-collapse: collapse;" width="600px">
                                                            <tr class="content">
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblFromSchNameReg" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="ListtableHead">
                                                                <th>
                                                                    Date
                                                                </th>
                                                                <th>
                                                                    NAV
                                                                </th>
                                                                <th>
                                                                    Unit
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
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("Unit").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulativeUnits").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulativeAmount").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CashFlow").ToString()%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                            <div id="div_regular" runat="server" visible="false">
                                                <asp:ListView ID="lstRegular" runat="server" ItemPlaceholderID="itemPlaceHolder1"
                                                    Style="margin-top: 106px; font-size: 8">
                                                    <LayoutTemplate>
                                                        <table border="2" cellpadding="2" style="border-collapse: collapse;" width="100%">
                                                            <tr class="content">
                                                                <td colspan="6">
                                                                    <asp:Label ID="lblToSchSwpReg" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="ListtableHead">
                                                                <th>
                                                                    Date
                                                                </th>
                                                                <th>
                                                                    Amount Invested
                                                                </th>
                                                                <th>
                                                                    Cumulative Amt Invested
                                                                </th>
                                                                <th>
                                                                    Units bought
                                                                </th>
                                                                <th>
                                                                    Cumulative Units Bought
                                                                </th>
                                                                <th>
                                                                    Net Avg Cost Per Unit
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
                                                                <%# Eval("InvestedAmt").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulAmtInvested").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("Unitsbought").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="15%">
                                                                <%# Eval("CumulUnitsBought").ToString()%>
                                                            </td>
                                                            <td align="center" style="font-size: small" width="10%">
                                                                <%# Eval("CostPerUnit").ToString()%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="width: 150px" valign="top" colspan="4">
                            <cc1:TabContainer ID="tcResults" runat="server" ActiveTabIndex="0" Visible="false">
                                <cc1:TabPanel ID="tabFrmSchemeValue" HeaderText="Value STP" runat="server">
                                    <ContentTemplate>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tabTo" HeaderText="Regular STP" runat="server">
                                    <ContentTemplate>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="width: 150px" valign="top" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px" valign="top" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px" valign="top" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px" valign="top" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px" valign="top" colspan="4">
                            <div id="divCal" runat="server" visible="false">
                                <table style="width: 600px">
                                    <tr class="content">
                                        <td style="width: 200">
                                            <strong>Total Units purchased</strong>
                                        </td>
                                        <td style="width: 200">
                                            <asp:Label ID="lbltotUnitsPur" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 200">
                                            <asp:Label ID="lbltotUnitsPur_1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="content">
                                        <td>
                                            <strong>Total Amt Invested</strong>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltotAmtInvest" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltotAmtInvest_1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="content">
                                        <td>
                                            <strong>Final Market Value</strong>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblfinalMarketVal" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblfinalMarketVal_1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
