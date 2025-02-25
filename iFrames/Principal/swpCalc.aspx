<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="swpCalc.aspx.cs" Inherits="iFrames.Principal.swpCalc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SWP Calculator</title>
    <script src="Resource/Principalcheck.js" type="text/javascript"></script>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="Resource/jquery-4.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txtwfrdt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
            $("#txtwtdt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#txtwvaldate").datepicker({
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
                                                        <img src="img/swp.jpg" alt="" />
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
                                                                            <asp:DropDownList ID="ddlMode" runat="server" Width="450px" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged"
                                                                                    AutoPostBack="True" CssClass="dropdown_details">
                                                                                    <%--<asp:ListItem>--</asp:ListItem>--%>
                                                                                    <asp:ListItem>SIP</asp:ListItem>
                                                                                    <asp:ListItem>LumpSum</asp:ListItem>
                                                                                    <asp:ListItem Selected="True">SWP</asp:ListItem>
                                                                                    <asp:ListItem>STP</asp:ListItem>
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
                                                                        <!--<tr align="left">
    <td colspan="2" valign="top">&nbsp;</td>
  </tr>-->
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td valign="top">
                                                                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="4">
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style2">
                                                                                Initial Amount
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="txtwinamt" runat="server" CssClass="textbox1" MaxLength="10" Text=""
                                                                                    onkeypress="return isNumber(event)" ReadOnly="false">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                Withdrawal Amount
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtwtramt" runat="server" CssClass="textbox1" MaxLength="14" Text=""
                                                                                    onkeypress="return isNumber(event)" ReadOnly="false">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style2">
                                                                                Frequency
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddwperiod" runat="server" CssClass="dropdown_details1">
                                                                                    <asp:ListItem Value="1" Selected="True">Monthly</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                SWP Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddSWPdate" runat="server" CssClass="dropdown_details1">
                                                                                    <asp:ListItem Selected="True">1</asp:ListItem>
                                                                                    <asp:ListItem>7</asp:ListItem>
                                                                                    <asp:ListItem>14</asp:ListItem>
                                                                                    <asp:ListItem>21</asp:ListItem>
                                                                                    <asp:ListItem>28</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style2">
                                                                                From Date
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="txtwfrdt" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                To Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtwtdt" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td valign="top">
                                                                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="4">
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style2">
                                                                                Value As On Date
                                                                            </td>
                                                                            <td align="left" class="td_style3">
                                                                                <asp:TextBox ID="txtwvaldate" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                Show Graph
                                                                            </td>
                                                                            <td align="left">
                                                                                <input type="checkbox" runat="server" name="checkbox" id="CheckBoxChart" onclick="showDiv();" />
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
                                                                                <asp:ImageButton ID="swpbtnshow" runat="server" ImageUrl="img/show.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="swpbtnshow_Click"></asp:ImageButton>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:ImageButton ID="swpbtnreset" runat="server" ImageUrl="img/reset.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="swpbtnreset_Click"></asp:ImageButton>
                                                                            </td>
                                                                            <td align="left">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%--<table width="45%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td colspan="2" height="10px">
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style4">
                                                                                <asp:ImageButton ID="swpbtnshow" runat="server" ImageUrl="img/show.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="swpbtnshow_Click"></asp:ImageButton>
                                                                            </td>
                                                                            <td align="left" class="td_style4">
                                                                                <asp:ImageButton ID="swpbtnreset" runat="server" ImageUrl="img/reset.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="swpbtnreset_Click"></asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>--%>
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
                                                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="0" style="display: none">
                                                                            <tr class="gridheader">
                                                                                <td align="center">
                                                                                    Scheme Name
                                                                                </td>
                                                                                <td align="center">
                                                                                    Total Amount Invested
                                                                                </td>
                                                                                <td align="center">
                                                                                    Total Amount Withdrawn
                                                                                </td>
                                                                                <td align="center">
                                                                                    Present Value
                                                                                </td>
                                                                                <td align="center">
                                                                                    Yield
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="gridrow">
                                                                                <td class="" align="center">
                                                                                    Principal Balanced Fund - Growth
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    100000
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    40000
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    85154.3986
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    8.56
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <asp:GridView ID="GridViewSWPSummary" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                            Width="100%" CssClass="gridrow" BorderWidth="0px" CellPadding="0" GridLines="Both"
                                                                            RowStyle-BorderWidth="1px" RowStyle-Height="30px" AlternatingRowStyle-CssClass=""
                                                                            HeaderStyle-CssClass="gridheader">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Name"
                                                                                    HeaderStyle-CssClass="">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("SCHEME_NAME") == DBNull.Value) ? "--" : Eval("SCHEME_NAME").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="gridtd padding" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount Invested"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("TOTAL_AMOUNT_INVESTED") == DBNull.Value) ? "--" : Eval("TOTAL_AMOUNT_INVESTED").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount Withdrawn"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("TOTAL_AMOUNT_WITHDRAWN") == DBNull.Value) ? "--" : Eval("TOTAL_AMOUNT_WITHDRAWN").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Present Value"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("PRESENT_VALUE") == DBNull.Value) ? "--" : TwoDecimal(Eval("PRESENT_VALUE").ToString())%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Yield" HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("YIELD") == DBNull.Value) ? "--" : Eval("YIELD").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
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
                                                                            <asp:Chart ID="chrtResult" runat="server" AlternateText="Principal swp" Visible="false"
                                                                                BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="751px" Height="338px"
                                                                                BackGradientStyle="None" BackColor="243, 249, 255" IsSoftShadows="false">
                                                                                <Titles>
                                                                                    <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                                        Text="SWP Chart" ForeColor="26, 59, 105">
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
                                                                                        <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="Time period">
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
                                                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="0" style="display: none">
                                                                            <tr class="gridheader">
                                                                                <td align="center">
                                                                                    Date
                                                                                </td>
                                                                                <td align="center">
                                                                                    NAV
                                                                                </td>
                                                                                <td align="center">
                                                                                    Cashflow
                                                                                </td>
                                                                                <td align="center">
                                                                                    Investment Amount
                                                                                </td>
                                                                                <td align="center">
                                                                                    Redeemed Units
                                                                                </td>
                                                                                <td align="center">
                                                                                    Cumulative Units
                                                                                </td>
                                                                                <td align="center">
                                                                                    Cumulative Units
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="gridrow">
                                                                                <td class="gridrow" align="center">
                                                                                    08-06-2009
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    24.66
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    -100000
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    100000
                                                                                </td>
                                                                                <td class="gridrow" align="center">
                                                                                    0
                                                                                </td>
                                                                                <td align="center" class="gridrow">
                                                                                    4055.15
                                                                                </td>
                                                                                <td align="center" class="gridrow">
                                                                                    100000
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="gridaltrow">
                                                                                <td class="gridaltrow" align="center">
                                                                                    09-06-2009
                                                                                </td>
                                                                                <td class="gridaltrow" align="center">
                                                                                    25.08
                                                                                </td>
                                                                                <td class="gridaltrow" align="center">
                                                                                    -1000
                                                                                </td>
                                                                                <td class="gridaltrow" align="center">
                                                                                    99000
                                                                                </td>
                                                                                <td class="gridaltrow" align="center">
                                                                                    39.8724
                                                                                </td>
                                                                                <td align="center" class="gridaltrow">
                                                                                    4015.2776
                                                                                </td>
                                                                                <td align="center" class="gridaltrow">
                                                                                    10703.1622
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <asp:GridView ID="GridViewSWPResult" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                            Width="100%" CssClass="gridrow" BorderWidth="1px" AlternatingRowStyle-CssClass="gridaltrow"
                                                                            RowStyle-Height="30px" AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Date" HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%--  <%# (Eval("Date") == DBNull.Value) ? "--" : Eval("Date").ToString()%>--%>
                                                                                        <%--  <%#  (Eval("Date") == DBNull.Value) ? "--" : Convert.ToDateTime(Eval("Date").ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>--%>
                                                                                        <%#  (Eval("Date") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("Date").ToString().Split('/')[2]),Convert.ToInt32(Eval("Date").ToString().Split('/')[1]),Convert.ToInt32(Eval("Date").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV" HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("NAV") == DBNull.Value) ? "--" : Eval("NAV").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Units" HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("No_of_Units") == DBNull.Value) ? "--" : Eval("No_of_Units").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="CashFlow" HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("FINAL_INVST_AMOUNT") == DBNull.Value) ? "--" : TwoDecimal(Eval("FINAL_INVST_AMOUNT").ToString())%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount">
                                                <ItemTemplate>
                                                    <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Eval("INVST_AMOUNT").ToString()%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cumilative Units"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("CUMILATIVE_UNITS") == DBNull.Value) ? "--" : Eval("CUMILATIVE_UNITS").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cumilative Amount"
                                                                                    HeaderStyle-CssClass="gridtd">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("CUMILATIVE_AMOUNT") == DBNull.Value) ? "--" :TwoDecimal(Eval("CUMILATIVE_AMOUNT").ToString())%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>--%>
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
                                    <table>
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
                                    Past Performance may or may not be sustained in future.
                                    <%-- Since Inception returns
                                (in %) are calculated on Compounded Annualised Basis.
                                <br />
                                <sup>$</sup> PTP (Point to Point) returns is based on standard investment of Rs.
                                10,000/- made at the beginning of the relevant period. In case of Dividend Reinvestment
                                Option all dividend payouts during the respective period are assumed to be reinvested
                                in the units of the scheme at the then prevailing NAV.--%>
                                    Please also refer to performance details of other schemes of Principal Mutual Fund
                                    managed by the Fund Manager(s) of this Scheme. To know the schemes managed by Fund
                                    Manager(s) please refer the table below:
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
                                                    Principal Global Opportunities Fund
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mr. P.V.K. Mohan
                                                </td>
                                                <td>
                                                    Principal Growth Fund, Principal Retail Equity Savings Fund, Principal Tax Savings
                                                    Fund
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
                                                    Mr. Anupam Tiwari
                                                </td>
                                                <td>
                                                    Principal Large Cap Fund, Principal Smart Equity Fund, Principal Personal Tax Saver
                                                    Fund
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mr. Gurvinder Singh Wasan
                                                </td>
                                                <td>
                                                    Principal Bank CD Fund, Principal Income Fund - Short Term Plan
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Mr. Pankaj Jain
                                                </td>
                                                <td>
                                                    Principal Debt Savings Fund - Monthly Income Plan, Principal Debt Opportunities
                                                    Fund - Corporate Bond Plan, Principal Cash Management Fund, Principal Income Fund
                                                    - Long Term Plan,Principal Retail Money Manager, Principal Debt Opportunities Fund
                                                    – Conservative Plan, Principal Debt Savings Fund - Retail Plan
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Ms. Rupali Pandit
                                                </td>
                                                <td>
                                                    Principal Index Fund
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Ms. Bekxy Kuriakose
                                                </td>
                                                <td>
                                                    Principal G Sec Fund
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <br />
                                    <%--  * Assistant Fund Manager
                                <br />--%>
                                    The return calculator has been developed and maintained by ICRA Analytics Limited.
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
