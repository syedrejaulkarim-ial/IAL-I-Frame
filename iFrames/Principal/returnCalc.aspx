<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="returnCalc.aspx.cs" Inherits="iFrames.Principal.returnCalc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Principal Calculator</title>
    <script src="Resource/Principalcheck.js" type="text/javascript"></script>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="Resource/jquery-3.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txtstartDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
            $("#txtendDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#txtvalDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#LumpStartDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#LumpEndDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
        });

    </script>
    <script type="text/javascript" language="javascript">
        function showDiv() {

            if (document.getElementById("CheckBoxChart").checked) {
                if (document.getElementById('<%=divshowChart.ClientID%>') != null)
                    document.getElementById('<%=divshowChart.ClientID%>').style.display = "inline";
            }
            else {
                if (document.getElementById('<%=divshowChart.ClientID%>') != null)
                    document.getElementById('<%=divshowChart.ClientID%>').style.display = "none";
            }

        }

        function tooltip(id) {
            // alert(id);
            id.title = id.options[id.selectedIndex].text;
        }

        function mouseEnter(controlName, staticLength) {
            var maxlength = 0;
            var mySelect = document.getElementById(controlName);
            for (var i = 0; i < mySelect.options.length; i++) {
                if (mySelect[i].text.length > maxlength) {
                    maxlength = mySelect[i].text.length;
                }
            }

            var ddlwidth = maxlength * 7;
            if (ddlwidth < 300)
                ddlwidth = 300;


            if (maxlength != 0)
                mySelect.style.width = ddlwidth;

        }
        function focusOut(controlName, staticLength) {

            var mySelect = document.getElementById(controlName);
            if (staticLength < 300)
                staticLength = 300;

            mySelect.style.width = staticLength;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divStart" runat="server">
        <table width="900" border="0" align="center" cellpadding="0" cellspacing="0" class="table_outer_border">
            <tr align="left">
                <td valign="top">
                    <table width="900" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr align="left">
                            <td valign="top">
                                <img src="img/prinHeader.jpg" width="900" height="92" alt="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: #003063; color: White; text-align: center; vertical-align: middle;
                                padding-top: 0px; height: 23px; font-size: 9pt; font-family: Verdana, Arial, Helvetica, sans-serif;">
                                <b>Principal Mutual Fund </b>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 12px;">
                                <img src="../Images/section-div-gradient.gif" width="100%" alt="" />
                            </td>
                        </tr>
                        <tr align="left">
                            <td valign="top">
                                <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr align="left">
                                        <td valign="top">
                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr align="left">
                                                    <td valign="top">
                                                        <img src="img/spacer11.gif" height="40" alt="" />
                                                    </td>
                                                </tr>
                                                <tr align="left">
                                                    <td valign="top">
                                                        <img src="img/sip.jpg" id="logocalc" runat="server" alt="" />
                                                    </td>
                                                </tr>
                                                <tr align="left">
                                                    <td valign="top" class="tr_outer_border">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr align="left">
                                                                <td valign="top">
                                                                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="4">
                                                                        <tr align="left">
                                                                            <td colspan="2">
                                                                                <img src="img/spacer11.gif" width="1" height="10" alt="" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style1">
                                                                                Investment Mode
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlMode" runat="server" Width="160px" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged"
                                                                                    AutoPostBack="True" CssClass="dropdown_details">
                                                                                    <%--<asp:ListItem>--</asp:ListItem>--%>
                                                                                    <asp:ListItem Selected="True">SIP</asp:ListItem>
                                                                                    <asp:ListItem>LumpSum</asp:ListItem>
                                                                                    <asp:ListItem>SWP</asp:ListItem>
                                                                                    <asp:ListItem>STP</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="td_style1">
                                                                                Scheme Type
                                                                            </td>
                                                                            <td class="tdstyle2">
                                                                                <asp:DropDownList ID="ddlNature" runat="server" Width="160px" AutoPostBack="true"
                                                                                    DataTextField="Nature" DataValueField="Nature" CssClass="dropdown_details" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="td_style1">
                                                                                Option of the Investment
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlOption" runat="server" Width="160px" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged"
                                                                                    AutoPostBack="true" CssClass="dropdown_details">
                                                                                    <asp:ListItem Selected="True">--</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Growth </asp:ListItem>
                                                                                    <asp:ListItem Value="3">Dividend Reinvestment</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style1">
                                                                                Scheme
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlscname" runat="server" AutoPostBack="true" CssClass="dropdown_details"
                                                                                    Width="450px" OnSelectedIndexChanged="ddlscname_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <%--onmouseover="tooltip(this);"--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td class="td_style1">
                                                                                Benchmark
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlbnmark" runat="server" CssClass="dropdown_details" Width="240px"
                                                                                    Enabled="True">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="td_style1">
                                                                                Fund Manager
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="FundmanegerText" class="textbox1" Width="150px" runat="server" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <!--<tr align="left">
    <td colspan="2" valign="top">&nbsp;</td>
  </tr>-->
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left" runat="server" id="trSIP" visible="true">
                                                                <td valign="top">
                                                                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="4">
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style3">
                                                                                Start Date
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="txtstartDate" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                End Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtendDate" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style3">
                                                                                Value As On Date
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="txtvalDate" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                SIP Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="dropdown_details1">
                                                                                    <asp:ListItem Selected="True">1</asp:ListItem>
                                                                                    <asp:ListItem>7</asp:ListItem>
                                                                                    <asp:ListItem>14</asp:ListItem>
                                                                                    <asp:ListItem>21</asp:ListItem>
                                                                                    <asp:ListItem>28</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style3">
                                                                                Frequency
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddperiod" runat="server" CssClass="dropdown_details1">
                                                                                    <asp:ListItem Value="1" Selected="True">Monthly</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                Amount
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="txtsipAmount" runat="server" CssClass="textbox1" MaxLength="10"
                                                                                    Text="" onkeypress="return isNumber(event)" ReadOnly="false">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style3">
                                                                                Show Graph
                                                                            </td>
                                                                            <td align="left" colspan="3">
                                                                                <input type="checkbox" runat="server" name="checkbox" id="CheckBoxChart" onclick="showDiv();" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left" runat="server" id="trlumpsum" visible="false">
                                                                <td valign="top">
                                                                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="4">
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style3">
                                                                                Start Date
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="LumpStartDate" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                Value As Of Date
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="LumpEndDate" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="td_style3">
                                                                                Amount Invested
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="LumpAmount" runat="server" CssClass="textbox1" MaxLength="10" Text=""
                                                                                    onkeypress="return isNumber(event)" ReadOnly="false">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td colspan="2">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td valign="top">
                                                                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="4" style="display: none">
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style2">
                                                                                Show Graph
                                                                            </td>
                                                                            <td align="left">
                                                                                <input type="checkbox" runat="server" name="checkbox" id="CheckBoxChart1" onclick="showDiv();" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left" style="display: none">
                                                                            <td height="27" align="left" valign="middle" class="td_style2">
                                                                                <span id="Label58" class="FieldHead">Scheme Inception Date</span>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:Label ID="SWPSchDt" runat="server" CssClass="td_style2"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td valign="top">
                                                                    <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td align="left" width="30%">
                                                                            </td>
                                                                            <td align="left" width="100">
                                                                                <asp:ImageButton ID="sipbtnshow" runat="server" ImageUrl="img/show.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="sipbtnshow_Click"></asp:ImageButton>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:ImageButton ID="sipbtnreset" runat="server" ImageUrl="img/reset.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="sipbtnreset_Click"></asp:ImageButton>
                                                                            </td>
                                                                            <td align="left">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td valign="top">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td valign="top">
                                            <img src="img/spacer11.gif" width="1" height="10" alt="" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td valign="top">
                                            <div id="resultDiv" runat="server" visible="false">
                                                <table width="100%" class="tr_outer_borderlower">
                                                    <tr>
                                                        <td>
                                                            <table width="96%" border="0" align="center" cellpadding="0" cellspacing="4">
                                                                <tr align="left">
                                                                    <td valign="middle" class="td_style4">
                                                                        <img src="img/bullet.jpg" width="10" height="8" alt="" />
                                                                        Summary Table
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="left" class="table_format">
                                                                        <asp:GridView ID="SPGridViewtbl1" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                            Width="100%" CssClass="gridrow" BorderWidth="0px" AlternatingRowStyle-CssClass="gridaltrow"
                                                                            RowStyle-Height="30px" AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Total Outflow in Rs."
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("TotalOutflow") == DBNull.Value) ? "--" : Eval("TotalOutflow").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Total Units Purchased"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("SumUnit") == DBNull.Value) ? "--" : Eval("SumUnit").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Value as of Date "
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("ValueasofDate") == DBNull.Value) ? "--" : Eval("ValueasofDate").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        
                                                                        <asp:GridView ID="LsGridViewtbl1" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                            CssClass="gridrow" BorderWidth="0px" AlternatingRowStyle-CssClass="gridaltrow"
                                                                            RowStyle-Height="30px" AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount Invested "
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("AmountInvested") == DBNull.Value) ? "--" : Eval("AmountInvested").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd"  />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Value as of Date"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# ( Eval("ValueasofDate") == DBNull.Value) ? "--" : Eval ("ValueasofDate").ToString() %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd"  />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr align="left">
                                                                    <td colspan="2" valign="middle">
                                                                        <img src="img/spacer11.gif" height="20" alt="" />
                                                                    </td>
                                                                </tr>
                                                                <tr align="left">
                                                                    <td colspan="2" align="center">
                                                                        <div id="divshowChart" runat="server" visible="false" style="width: 100%; display: none">
                                                                            <asp:Chart ID="chrtResult" runat="server" AlternateText="Principal SIP" Visible="true"
                                                                                BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="751px" Height="338px"
                                                                                BackGradientStyle="None" BackColor="243, 249, 255" IsSoftShadows="false">
                                                                                <Titles>
                                                                                    <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                                        Text="SIP Chart" ForeColor="26, 59, 105">
                                                                                    </asp:Title>
                                                                                </Titles>
                                                                                <Legends>
                                                                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold">
                                                                                    </asp:Legend>
                                                                                </Legends>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="" AlignmentStyle="PlotPosition"
                                                                                        BackSecondaryColor="White" BackColor="#ECF4F9" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                                        BackHatchStyle="None" BorderDashStyle="Solid" BackImageTransparentColor="#CCCCFF">
                                                                                        <Area3DStyle Enable3D="false" Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                                            WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                                        <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="SWP period">
                                                                                            <ScaleBreakStyle Enabled="false" />
                                                                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                                                                        </AxisX>
                                                                                        <AxisY ArrowStyle="None" ToolTip="Amount" LineColor="#013974">
                                                                                            <ScaleBreakStyle Enabled="True" />
                                                                                            <MajorGrid Enabled="false" />
                                                                                        </AxisY>
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="td_style4">
                                                                        <img src="img/bullet.jpg" width="10" height="8" alt="" />&nbsp;Detailed Calculation
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="left" class="table_format">
                                                                    <div id="divSipResultGrid" runat="server">
                                                                        <asp:GridView ID="SipResultGrid" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                            CssClass="gridrow" BorderWidth="0px" CellPadding="0" GridLines="Both" RowStyle-BorderWidth="1px"
                                                                            RowStyle-Height="30px" AlternatingRowStyle-CssClass="gridaltrow" HeaderStyle-CssClass="gridheader">
                                                                            <Columns>
                                                                                <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Sr No." HeaderStyle-CssClass="">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Id") == DBNull.Value) ? "--" : Eval("Id").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd"  />
                                                                                </asp:TemplateField>--%>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV Date" HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%--<%# (Eval("Nav Date") == DBNull.Value) ? "--" : Eval("Nav Date").ToString()%>--%>
                                                                                        <%#  (Eval("Nav Date") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("Nav Date").ToString().Split('/')[2]),Convert.ToInt32(Eval("Nav Date").ToString().Split('/')[1]),Convert.ToInt32(Eval("Nav Date").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV Value of the Scheme as of NAV Date"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Nav") == DBNull.Value) ? "--" : Eval("Nav").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Outflow in Rs."
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Scheme Cashflow") == DBNull.Value) ? "--" : Eval("Scheme Cashflow").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Units Purchased as of NAV Date"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Scheme Units") == DBNull.Value) ? "--" : Eval("Scheme Units").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <br /> <br /> 
                                                                         </div>                                                                      
                                                                        <asp:GridView ID="CommonResultGridView" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                            OnRowDataBound="GV_RowDataBound" OnDataBound="gv_DataBound" Width="100%" CssClass="gridrow"
                                                                            BorderWidth="1px" AlternatingRowStyle-CssClass="gridaltrow" RowStyle-Height="30px"
                                                                            AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Period" HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="LitQtrPrd" runat="server" Text='<%# (Eval("qtryear") == DBNull.Value) ? "--" : Eval("qtryear").ToString()%>'></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" Width="150px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns in %"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("navReturn") == DBNull.Value) ? "--" : Eval("navReturn").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<sup>$</sup>PTP Returns"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("navp2pReturn") == DBNull.Value) ? "--" : Eval("navp2pReturn").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns in %"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("indexReturn") == DBNull.Value) ? "--" : Eval("indexReturn").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<sup>$</sup>PTP Returns"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("indexp2pReturn") == DBNull.Value) ? "--" : Eval("indexp2pReturn").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns in %"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("addlindexReturn") == DBNull.Value) ? "--" : Eval("addlindexReturn").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<sup>$</sup>PTP Returns"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("addlindexp2pReturn") == DBNull.Value) ? "--" : Eval("addlindexp2pReturn").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>

                                                                        <asp:Repeater ID="RptCommonSipResult" runat="server" Visible="false">
                                                                            <HeaderTemplate>
                                                                                <table style="border-width: 0px; width: 100%; border-collapse: collapse;" border="0" rules="all" cellspacing="0" cellpadding="0" class="gridrow">
                                                                                    <tr class="gridheader" style="background:#659EC7;">
                                                                                        <th colspan="8" class="gridtd" align="center">
                                                                                            <asp:Label ID="lblReturnSch" runat="server"></asp:Label>

                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr class="gridheader">
                                                                                        <th class="gridtd" align="center" style="width:15%;border-right:#C6DEFF solid 1px;">Particulars
                                                                                        </th>
                                                                                        <th class="gridtd" align="center" style="border-right:#C6DEFF solid 1px;">Total Amount Invested (in Rs.)
                                                                                        </th>
                                                                                        <th class="gridtd" align="center"style="border-right:#C6DEFF solid 1px;">Scheme Returns Yield (%)
                                                                                        </th>
                                                                                        <th class="gridtd" align="center"style="border-right:#C6DEFF solid 1px;">Scheme Market Value (in Rs.)
                                                                                        </th>
                                                                                        <th class="gridtd" align="center"style="border-right:#C6DEFF solid 1px;">Bechmark (<asp:Label ID="lblReturnIndex" runat="server"></asp:Label>) Returns Yield * (%)
                                                                                        </th>
                                                                                        <th class="gridtd" align="center"style="border-right:#C6DEFF solid 1px;">Benchmark Market Value (in Rs.)
                                                                                        </th>
                                                                                        <th class="gridtd" align="center"style="border-right:#C6DEFF solid 1px;" >Additional Bechmark (<asp:Label ID="lblReturnAddIndex" runat="server"></asp:Label>) Return Yield * (%)
                                                                                        </th>
                                                                                        <th class="gridtd" align="center">Additional Benchmark Market Value (in Rs.)
                                                                                        </th>
                                                                                    </tr>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tr style="border-width: 1px; border-style: solid; height: 30px;">
                                                                                    <td class="gridtd" align="center">
                                                                                        <%#Eval("Particulars") %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%#Eval("Total_Amount_Invest") %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Return_Yield")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Market_value")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Bechmark_return_yield")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Benchmark_Market_value")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Additional_Bechmark_return_yield")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Additional_Benchmark_Market_value")),2) %>
                                                                                    </td>

                                                                                </tr>
                                                                            </ItemTemplate>

                                                                            <AlternatingItemTemplate>
                                                                                <tr class="gridaltrow" style="border-width: 1px; border-style: solid; height: 30px;">
                                                                                    <td class="gridtd" align="center">
                                                                                        <%#Eval("Particulars") %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%#Eval("Total_Amount_Invest") %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Return_Yield")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Market_value")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Bechmark_return_yield")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Benchmark_Market_value")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Additional_Bechmark_return_yield")),2) %>
                                                                                    </td>
                                                                                    <td class="gridtd" align="center">
                                                                                        <%# Math.Round(Convert.ToDecimal(Eval("Additional_Benchmark_Market_value")),2) %>
                                                                                    </td>
                                                                                </tr>
                                                                            </AlternatingItemTemplate>

                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; padding-top: 5px;">
                                <asp:Panel runat="server" ID="TableNote" CssClass="CssTableNote " Visible="false">
                                    <span id="lblerrmsg" style="margin-top: 11px; margin-left: 10px; vertical-align: middle;
                                        color: Red; font-family: Arial; font-weight: bold"></span>
                                    <asp:Label Style="margin-top: 11px; vertical-align: middle; color: Red; font-family: Arial;
                                        font-weight: bold" Text="" ID="Label1" runat="server"></asp:Label>
                                    <table style="display: none">
                                        <tr>
                                            <td valign="middle">
                                                <img src="img/bullet.jpeg" alt="" width="5" height="5" />
                                                For the schemes “Principal Personal Tax Saver Fund” and “Principal Tax Savings Fund”
                                                SWP is only available after the lock in period i.e. 3 years.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <img src="img/bullet.jpeg" alt="" width="5" height="5" />
                                                Principal Tax Savings Fund is closed for subscription.
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    Past Performance may or may not be sustained in future. Since Inception returns
                                    (in %) are calculated on Compounded Annualised Basis.
                                    <br />
                                    <sup>$</sup> PTP (Point to Point) returns is based on standard investment of Rs.
                                    10,000/- made at the beginning of the relevant period. In case of Dividend Reinvestment
                                    Option all dividend payouts during the respective period are assumed to be reinvested
                                    in the units of the scheme at the then prevailing NAV. Please also refer to performance
                                    details of other schemes of Principal Mutual Fund managed by the Fund Manager(s)
                                    of this Scheme. To know the schemes managed by Fund Manager(s) please refer the
                                    table below:
                                    <br />
                                    <br />
                                    <table width="100%" cellspacing="2" cellpadding="2" border="2" style="border-color: Black;">
                                        <thead>
                                            <tr>
                                                <td width="25%">
                                                    <b>Name of the Fund Manager </b>
                                                </td>
                                                <td width="75%">
                                                    <b>Scheme(s) Managed by the Fund Manager </b>
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td width="25%">
                                                    Mr. Rajat Jain
                                                </td>
                                                <td width="75%">
                                                    Principal Global Opportunities Fund, Principal Asset Allocation Fund of Funds, Principal Index Fund - Midcap, Principal Smart Equity Fund, Principal Large Cap Fund, Principal Index Fund - Nifty, Principal Personal Taxsaver, Principal Arbitrage Fund
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mr. P.V.K. Mohan
                                                </td>
                                                <td>
                                                    Principal Growth Fund,Principal Balanced Fund, Principal Tax Savings
                                                    Fund,Principal Equity Savings Fund
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mr. Dhimant Shah
                                                </td>
                                                <td>
                                                    Principal Emerging Bluechip Fund, Principal Dividend Yield Fund
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td>
                                                    Mr. Gurvinder Singh Wasan
                                                </td>
                                                <td>
                                                    Principal Bank CD Fund, Principal Short Term Income Fund
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mr. Pankaj Jain
                                                </td>
                                                <td>
                                                    Principal Low Duration Fund,Principal Retail Money Manager Fund, Principal Cash Management Fund, Principal Debt Savings Fund - Retail Plan
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td>
                                                    Ms. Bekxy Kuriakose
                                                </td>
                                                <td>
                                                    Principal G Sec Fund,Principal Dynamic Bond Fund, Principal Credit Opportunities Fund,Principal Balanced Fund
                                                </td>
                                            </tr>                                            
                                        </tbody>
                                    </table>
                                    <br />
                                    <%--  * Assistant Fund Manager
                                <br />--%>
                                    The return calculator has been developed and maintained by <a style="color: #333333;" href="https://www.icraanalytics.com"
                                                                                                            target="_blank">ICRA Analytics Limited</a>-<a style="color: #333333;" href="https://icraanalytics.com/home/Disclaimer"
                                                                                                            target="_blank">Disclaimer</a>.
                                                                                                            
                                    Principal Pnb Asset Management Company Pvt. Ltd. / Principal Trustee Company Pvt.
                                    Ltd. does not endorse the authenticity or accuracy of the figures based on which
                                    the returns are calculated, nor shall they be held responsible or liable for any
                                    error or inaccuracy or for any losses suffered by any investor as a direct or indirect
                                    consequence of relying upon the data displayed by the calculator.
                                    <br />
                                    <br />
                                    The calculator, based on assumed rate of returns, is meant for illustration purposes
                                    only. The calculations are not based on any judgments of the future return of the
                                    debt and equity markets / sectors or of any individual security and should not be
                                    construed as a promise, guarantee or forecast on minimum returns. Protection against
                                    loss in a declining market is not guaranteed. Applicable taxes, fees, expenses and/or
                                    any other charges have not been considered in calculations and where the same if
                                    taken into consideration may reduce the returns on your actual investments. Dividend
                                    declarations, if any and Inflation have not been considered for calculation of returns.
                                    Please consult your tax/investment advisor before investing.
                                    <br />
                                    <br />
                                    <strong>Statutory Details:</strong> Principal Mutual Fund has been constituted as
                                    a trust with Principal Financial Group (Mauritius) Limited, Punjab National Bank
                                    and Vijaya Bank as the co-settlors. <strong>Sponsor</strong> : Principal Financial
                                    Services Inc., USA [acting through its wholly owned subsidiary Principal Financial
                                    Group (Mauritius) Ltd.]. <strong>Trustee</strong> : Principal Trustee Company Private
                                    Limited. <strong>Investment Manager:</strong> Principal Pnb Asset Management Company
                                    Private Limited (AMC). <strong>Risk Factors: Mutual funds and securities investments
                                        are subject to market risks and there can be no assurance and no guarantee that
                                        a scheme's objective can be achieved. As with any investment in securities, the
                                        NAV of the units issued under a scheme can go up or down, depending upon the factors
                                        and forces affecting the capital markets</strong> . Past performance of the
                                    Sponsor and any of its associates, co-settlors and/or AMC/ Mutual Fund does not
                                    indicate or guarantee the future performance of the Schemes of Principal Mutual
                                    Fund. <strong>Investors are urged to read the Scheme Information Document (SID), Statement
                                        of Additional Information (SAI) & Key Information Memorandum (KIM) carefully, and
                                        consult their tax/ investment advisor before they invest in the scheme(s)</strong>
                                    . The Sponsor and any of their associates including co-settlors are not responsible
                                    or liable for any loss resulting from the operations of the Principal Mutual Fund
                                    beyond the initial contribution of an amount of Rs 25 lakhs towards setting up Principal
                                    Mutual Fund. Investors are not being offered a guaranteed or assured rate of return
                                    or monthly or regular/periodical income distribution, and the actual returns and/or
                                    periodical income distribution to an investor will be based on the actual NAV, which
                                    may go up or down, depending on the market conditions.
                                    <br />
                                    <br />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
