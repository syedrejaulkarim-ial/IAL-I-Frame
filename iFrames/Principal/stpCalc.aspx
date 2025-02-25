<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="stpCalc.aspx.cs" Inherits="iFrames.Principal.stpCalc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>STP Calculator</title>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="Resource/Principalcheck.js" type="text/javascript"></script>
    <script src="Resource/jquery-4.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Resource/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txtfrdt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
            $("#txttodt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#txtvalue").datepicker({
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
                if (document.getElementById('<%=divToSchemeshowChart.ClientID%>') != null)
                    document.getElementById('<%=divToSchemeshowChart.ClientID%>').style.display = "inline";
                if (document.getElementById('<%=divFromSchemeshowChart.ClientID%>') != null)
                    document.getElementById('<%=divFromSchemeshowChart.ClientID%>').style.display = "inline";
            }
            else {
                if (document.getElementById('<%=divToSchemeshowChart.ClientID%>') != null)
                    document.getElementById('<%=divToSchemeshowChart.ClientID%>').style.display = "none";
                if (document.getElementById('<%=divFromSchemeshowChart.ClientID%>') != null)
                    document.getElementById('<%=divFromSchemeshowChart.ClientID%>').style.display = "none";
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
                                                    <img src="img/stp.jpg" width="151" height="34" alt="" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td valign="top" class="tr_outer_border">
                                                    <div>
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
                                                                                   <%-- <asp:ListItem>--</asp:ListItem>--%>
                                                                                    <asp:ListItem>SIP</asp:ListItem>
                                                                                    <asp:ListItem>LumpSum</asp:ListItem>
                                                                                    <asp:ListItem>SWP</asp:ListItem>
                                                                                    <asp:ListItem Selected="True">STP</asp:ListItem>
                                                                                </asp:DropDownList>                                                                               
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style1">
                                                                                Transfer From
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlschtrf" runat="server" AutoPostBack="true" CssClass="dropdown_details"
                                                                                    Width="450px" OnSelectedIndexChanged="ddlschtrf_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td class="td_style1">
                                                                                Transfer To
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="false" CssClass="dropdown_details"
                                                                                    Width="450px" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged">
                                                                                </asp:DropDownList>
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
                                                                                <asp:TextBox ID="txtiniamt" runat="server" CssClass="textbox1" MaxLength="10" Text=""
                                                                                    onkeypress="return isNumber(event)" ReadOnly="false">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                Transfer Amount
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txttranamt" runat="server" CssClass="textbox1" MaxLength="10" Text=""
                                                                                    onkeypress="return isNumber(event)" ReadOnly="false">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left" class="td_style2">
                                                                                Frequency
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="txtddperiod" runat="server" CssClass="dropdown_details1">
                                                                                    <asp:ListItem Value="1" Selected="True">Monthly</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                STP Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="txtddSTPDate" runat="server" CssClass="dropdown_details1">
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
                                                                                <asp:TextBox ID="txtfrdt" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                To Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txttodt" runat="server" CssClass="textbox1">
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
                                                                                <asp:TextBox ID="txtvalue" runat="server" CssClass="textbox1">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="td_style2">
                                                                                Show Graph
                                                                            </td>
                                                                            <td align="left">
                                                                                <input type="checkbox" runat="server" name="checkbox" id="CheckBoxChart" onclick="showDiv();" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td valign="top">
                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td align="left" width="30%">
                                                                            </td>
                                                                            <td align="left" width="100">
                                                                                <asp:ImageButton ID="stpbtnshow" runat="server" ImageUrl="img/show.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="stpbtnshow_Click"></asp:ImageButton>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:ImageButton ID="stpbtnreset" runat="server" ImageUrl="img/reset.jpg" Width="80"
                                                                                    Height="25" border="0" OnClick="stpbtnreset_Click"></asp:ImageButton>
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
                                                    </div>
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
                                                                <td valign="middle" class="td_style4" style="height: 40px; vertical-align: top; padding-top: 10px;">
                                                                    <img src="img/bullet.jpg" width="10" height="8" alt="" />
                                                                    Summary Table
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" class="table_format">
                                                                    <asp:GridView ID="gvFromSchemeSummary" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                        Width="99%" CssClass="gridrow" BorderWidth="0px" AlternatingRowStyle-CssClass="gridaltrow"
                                                                        RowStyle-Height="30px" AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Name"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("FromSchemeName"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Total Amount Invested"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("TotalAmtInvst"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Total Amount Withdrawn"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("TotalAmtWidrn"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Present Value"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("PresentValue"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Yield" HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("Yield"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" valign="top">
                                                                    <img src="img/spacer11.gif" width="1" height="10" alt="" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" class="table_format">
                                                                    <asp:GridView ID="gvToSchemeSummary" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                        Width="99%" CssClass="gridrow" BorderWidth="0px" AlternatingRowStyle-CssClass="gridaltrow"
                                                                        RowStyle-Height="30px" AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Name"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("ToSchemeName"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Total Amount Invested"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("TotalAmtInvst"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Present Value"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("PresentValue"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Yield" HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("Yield"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td colspan="2" valign="middle">
                                                                    <img src="img/spacer11.gif" height="10" alt="" />
                                                                </td>
                                                            </tr>
                                                            <%--<div class="graph_div">--%>
                                                            <tr align="left">
                                                                <td colspan="2" align="center">
                                                                    <div id="divFromSchemeshowChart" runat="server" visible="false" style="width: 100%;
                                                                        display: none">
                                                                        <asp:Chart ID="chrtFromSchemeResult" runat="server" AlternateText="Principal swp"
                                                                            Visible="true" BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="751px"
                                                                            Height="338px" BackGradientStyle="None" BackColor="243, 249, 255" IsSoftShadows="false">
                                                                            <Titles>
                                                                                <%--<asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                            Text="Switch Out (Principal Debt Opportunities Fund - Conservative Plan - Growth)"
                                                                            ForeColor="26, 59, 105">
                                                                        </asp:Title>--%>
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
                                                                                    <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="STP period">
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
                                                            <tr align="left">
                                                                <td colspan="2" align="center">
                                                                    <div id="divToSchemeshowChart" runat="server" visible="false" style="width: 100%;
                                                                        display: none">
                                                                        <asp:Chart ID="chrtToSchemeResult" runat="server" AlternateText="Principal swp" Visible="true"
                                                                            BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="751px" Height="338px"
                                                                            BackGradientStyle="None" BackColor="243, 249, 255" IsSoftShadows="false">
                                                                            <Titles>
                                                                                <%-- <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                            Text=" Switch In (Principal Balanced Fund - Dividend)" ForeColor="26, 59, 105">
                                                                        </asp:Title>--%>
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
                                                            <%-- </div>--%>
                                                            <%--<div class="detailed_div">--%>
                                                            <tr>
                                                                <td align="left" class="td_style4">
                                                                    <img src="img/bullet.jpg" width="10" height="8" alt="" />&nbsp;Detailed Calculation
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" class="table_format">
                                                                    <asp:GridView ID="gvSTPDetails1" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                        Width="99%" CssClass="gridrow" BorderWidth="0px" AlternatingRowStyle-CssClass="gridaltrow"
                                                                        RowStyle-Height="30px" AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="From Scheme"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("FROM_SCHEME_NAME"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Date" HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%--<%#  (Eval("FROM_DATE") == DBNull.Value) ? "--" : Eval("FROM_DATE").ToString()%>--%>
                                                                                    <%#  (Eval("FROM_DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV" HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("FROM_NAV") == DBNull.Value) ? "--" : Eval("FROM_NAV").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cash Flow" HeaderStyle-CssClass="gridtd">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("CashFlow") == DBNull.Value) ? "--" : Eval("CashFlow").ToString()%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                    </asp:TemplateField>--%>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Redeemed Units"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("REDEEM_UNITS") == DBNull.Value) ? "--" : Eval("REDEEM_UNITS").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cumilative Units"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("CUMILATIVE_UNITS_FROM") == DBNull.Value) ? "--" : Eval("CUMILATIVE_UNITS_FROM").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Investment Value"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%--<%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : TwoDecimal(Eval("INVST_AMOUNT").ToString())%>--%>
                                                                                    <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Eval("INVST_AMOUNT").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" valign="top">
                                                                    <img src="img/spacer11.gif" width="1" height="10" alt="" />
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:GridView ID="gvSTPDetails2" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                        Width="99%" CssClass="gridrow" BorderWidth="0px" AlternatingRowStyle-CssClass="gridaltrow"
                                                                        RowStyle-Height="30px" AlternatingRowStyle-Height="30px" HeaderStyle-CssClass="gridheader">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="To Scheme" HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("TO_SCHEME_NAME"))%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Date" HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%#  (Eval("TO_DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV" HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("TO_NAV") == DBNull.Value) ? "--" : Eval("TO_NAV").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cash Flow" HeaderStyle-CssClass="gridtd">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("CashFlow") == DBNull.Value) ? "--" : Eval("CashFlow").ToString()%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                    </asp:TemplateField>--%>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="No Of Units"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("NO_OF_UNITS") == DBNull.Value) ? "--" : Eval("NO_OF_UNITS").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cumilative Units"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("CUMILATIVE_UNITS_TO") == DBNull.Value) ? "--" : Eval("CUMILATIVE_UNITS_TO").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Investment Value"
                                                                                HeaderStyle-CssClass="gridtd">
                                                                                <ItemTemplate>
                                                                                    <%# (Eval("AMOUNT") == DBNull.Value) ? "--" : Eval("AMOUNT").ToString()%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" CssClass="gridtd" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <%--</div>--%>
                                                            <tr>
                                                                <td colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <div>
                                    <tr align="left">
                                        <td valign="top">
                                            <img src="img/spacer11.gif" width="1" height="10" alt="" />
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td class="tr_outer_border">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                </div>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
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
                                            STP is only available after the lock in period i.e. 3 years.
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
                                <%-- * Assistant Fund Manager
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
    </form>
</body>
</html>
