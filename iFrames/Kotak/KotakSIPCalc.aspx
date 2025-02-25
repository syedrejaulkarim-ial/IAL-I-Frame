<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="KotakSIPCalc.aspx.cs" Inherits="iFrames.Kotak.KotakSIPCalc"
    EnableEventValidation="false" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kotak SIP Calculator </title>
    <link href="Resource/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery-ui-1.8.14.custom_kotak.css" rel="stylesheet" type="text/css" />
    <script src="Resource/KotakSipVald.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery110.js"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui.min110.js"></script>  
    <script src="Resource/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pop() {
            //            $(function () {
            $('#<%= sipbtnshow.ClientID %>').click(function () {
                $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
            });
            //});
        }
 
 
    </script>
    <script type="text/javascript">

        $(function () {
            $("#txtfromDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
            });

            $("#txtToDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
            });

            $("#txtvalason").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
            });
        });

        function showDiv() {
            // alert("hi");
            //            alert(document.getElementById("divshowChart").style.display);
            //  alert( document.getElementById("CheckBoxChart").checked)
            // alert(document.getElementById("CheckBoxChart"));
            if (document.getElementById("CheckBoxChart").checked) {
                if (document.getElementById('<%=divshowChart.ClientID%>') != null)
                    document.getElementById('<%=divshowChart.ClientID%>').style.display = "inline";
            }
            else {
                if (document.getElementById('<%=divshowChart.ClientID%>') != null)
                    document.getElementById('<%=divshowChart.ClientID%>').style.display = "none";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="690px" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr align="left">
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr align="left" style="background: #da251c">
                            <td valign="top">
                                <img src="KotakImg/logo.jpg" alt="" />
                            </td>
                        </tr>
                        <tr align="left">
                            <td valign="top" style="background: url('KotakImg/bg_b.jpg') repeat-x">
                                &nbsp;
                            </td>
                        </tr>
                        <tr align="left">
                            <td valign="top">
                                <table align="center" width="100%" cellpadding="0" cellspacing="0" style="background: #dddddd">
                                    <tr>
                                        <td style="border: #013974 solid 1px;">
                                            <img src="KotakImg/sip.png" height="24" alt="">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="3" style="background: #fff">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="sipDiv">
                                                <table width="100%" border="0" align="center" cellpadding="7" cellspacing="1" id="sipTable"
                                                    style="background: url('KotakImg/bg.jpg') #dddddd; border: #013974 solid 1px;">
                                                    <tr>
                                                        <td width="179">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 175px" valign="top">
                                                            <span id="Label12" class="FieldHead">Scheme Name</span>
                                                        </td>
                                                        <td style="width: 319px" valign="top" align="left" colspan="2">
                                                            <asp:DropDownList ID="ddlscheme" runat="server" CssClass="ddl" Width="370px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: ">
                                                        <td>
                                                            <span id="Label25" class="FieldHead">Benchmark</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:DropDownList ID="ddlsipbnmark" runat="server" CssClass="ddl" Width="370px" Enabled="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 175px" valign="top">
                                                            <span id="Label13" class="FieldHead">Installment Amount (Rs.)</span>
                                                        </td>
                                                        <td width="168" align="left" valign="top" style="width: 19px">
                                                            <asp:TextBox runat="server" type="text" value="1000" MaxLength="14" ID="txtinstall"
                                                                class="ddl"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="Label28" class="FieldHead">Frequency</span>&nbsp;&nbsp;&nbsp;
                                                            <asp:DropDownList ID="ddPeriod_SIP" runat="server" Width="124px" CssClass="ddl">
                                                                <asp:ListItem Value="0">--</asp:ListItem>
                                                                <asp:ListItem Value="1">Monthly</asp:ListItem>
                                                                <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Label35" class="FieldHead">SIP Date</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="ddl">
                                                                <asp:ListItem Value="0">1st</asp:ListItem>
                                                                <asp:ListItem Value="1">7th</asp:ListItem>
                                                                <asp:ListItem Value="2">14th</asp:ListItem>
                                                                <asp:ListItem Value="3">21st</asp:ListItem>
                                                                <asp:ListItem Value="3">28th</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <span id="Label14" class="FieldHead">From Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="ddl"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <span id="Label15" class="FieldHead">To Date</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:TextBox ID="txtToDate" runat="server" Width="115px" CssClass="ddl"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <span id="Label16" class="FieldHead">Value as on Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtvalason" runat="server" CssClass="ddl"></asp:TextBox>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <label>
                                                                <span class="FieldHead">Show graph</span>
                                                                <%-- <asp:CheckBox ID="chkBoxShowGraph" runat="server" AutoPostBack="true"
                                                                title="Checked to  Show Graph"  Visible="false"
                                                                oncheckedchanged="chkBoxShowGraph_CheckedChanged" />
                                                               <asp:CheckBox ID="CheckBoxChart" runat="server" AutoPostBack="false" onClick="showDiv();" />--%>
                                                                <input type="checkbox" runat="server" name="checkbox" id="CheckBoxChart" onclick="showDiv();" />
                                                                <%--<input type="checkbox" runat="server" name="checkbox" id="checkbox" onclick="return showchecked();">--%>
                                                            </label>
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                                        <td colspan="3" height="2px">
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <asp:ImageButton ID="sipbtnshow" runat="server" ImageUrl="~/Kotak/KotakImg/submit.png"
                                                                OnClick="btnSip_Click" OnClientClick="return validate_SIP();" />
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:ImageButton ID="sipbtnreset" runat="server" ImageUrl="~/Kotak/KotakImg/reset.png"
                                                                OnClick="btnReset_Click" />
                                                        </td>
                                                        <td align="right">
                                                            <input type="image" style="display: none" name="showGraph" id="showGraph" alt="show graph"
                                                                title="show graph" src="KotakImg/line_graph.png" onclick="return Showdiv2();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <tr>
                                                    <td height="3" style="background: #fff">
                                                    </td>
                                                </tr>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td width="100%" valign="top">
                                            <div id="resultDiv" runat="server" visible="false">
                                                <table id="tble" width="100%">
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <div id="divshowChart" runat="server" visible="true" style="display: none">
                                                               <%-- <asp:Chart runat="server" ID="chrtResult" AlternateText="Kotak Sip" Visible="false"
                                                                    BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="700px" Height="330px"
                                                                    BackGradientStyle="Center" BackColor="Gray" IsSoftShadows="false">
                                                                    <titles>
                                                                    <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                        Text="Kotak SIP Chart" ForeColor="26, 59, 105">
                                                                    </asp:Title>
                                                                </titles>
                                                                    <legends>
                                                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold">
                                                                    </asp:Legend>
                                                                </legends>
                                                                    <series><asp:Series Name="Series1"></asp:Series></series>
                                                                    <chartareas>
                                                                 <asp:ChartArea Name="ChartArea1" BorderWidth="2"    BorderColor="" AlignmentStyle="PlotPosition"
                                                                        BackSecondaryColor="White" BackColor="#ECF4F9" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                        BackHatchStyle="None" BorderDashStyle="Solid" BackImageTransparentColor="#CCCCFF">
                                                                    <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="SIP period" >
                                                                            <ScaleBreakStyle Enabled="false" />
                                                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                                                        </AxisX>
                                                                        <AxisY ArrowStyle="None" ToolTip="Amount" LineColor="#013974" 
                                                                           >
                                                                            <ScaleBreakStyle Enabled="True" />
                                                                        </AxisY>
                                                                    </asp:ChartArea></chartareas>
                                                                </asp:Chart>--%>
                                                                <asp:Image ID="ImgchrtResult"  Width="700px" Height="330px" runat="server" />
                                                               <asp:Chart ID="chrtResult" runat="server" AlternateText="Kotak Sip" Visible="false"
                                                                BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="700px" Height="330px"
                                                                BackGradientStyle="Center" BackColor="Gray" IsSoftShadows="false" >
                                                                <Titles>
                                                                    <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                        Text="Kotak SIP Chart" ForeColor="26, 59, 105">
                                                                    </asp:Title>
                                                                </Titles>                                                             
                                                                <Legends>
                                                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold">
                                                                    </asp:Legend>
                                                                </Legends>
                                                                <ChartAreas>
                                                                    <asp:ChartArea Name="ChartArea1" BorderWidth="2"    BorderColor="" AlignmentStyle="PlotPosition"
                                                                        BackSecondaryColor="White" BackColor="#ECF4F9" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                        BackHatchStyle="None" BorderDashStyle="Solid" BackImageTransparentColor="#CCCCFF">
                                                                        <Area3DStyle Enable3D="false" Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                            WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                        <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="SIP period" >
                                                                            <ScaleBreakStyle Enabled="false" />
                                                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                                                        </AxisX>
                                                                        <AxisY ArrowStyle="None" ToolTip="Amount" LineColor="#013974" 
                                                                           >
                                                                            <ScaleBreakStyle Enabled="True" />
                                                                        </AxisY>
                                                                    </asp:ChartArea>
                                                                </ChartAreas>
                                                            </asp:Chart>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="infodiv" runat="server" visible="false">
                                                                <table width="100%" bgcolor="#dddddd">
                                                                    <tr>
                                                                        <td width="2%" height="25" align="left" valign="middle">
                                                                            <img src="KotakImg/arw.gif" width="10" height="10">
                                                                        </td>
                                                                        <td width="84%" height="25" align="left" valign="middle">
                                                                            <asp:Label ID="lblTotalInvst" runat="server" Text="Total Invested Amount" CssClass="rslt_text"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="2%" height="25" align="left" valign="middle">
                                                                            <img src="KotakImg/arw.gif" width="10" height="10">
                                                                        </td>
                                                                        <td width="84%" height="25" align="left" valign="middle">
                                                                            <asp:Label ID="lblInvstvalue" runat="server" CssClass="rslt_text" Text="On Date C, the Scheme value of your total investment Rs Y would be Rs Z"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="2%" height="25" align="left" valign="middle">
                                                                            <img src="KotakImg/arw.gif" width="10" height="10">
                                                                        </td>
                                                                        <td width="84%" height="25" align="left" valign="middle" class="rslt_text">
                                                                            For Return calculation, Check table below:
                                                                        </td>
                                                                        <td width="6%" height="25" align="left" valign="middle">
                                                                            <%--<a href="kotak.xlsx" target="_blank"><img src="excel1.png" alt="Import to Excel" width="24" height="24" border="0" title="Import to Excel"></a>--%>
                                                                            <asp:ImageButton ID="imgbtnExcel" runat="server" title="Import to Excel" ImageUrl="~/Kotak/KotakImg/excel1.png"
                                                                                OnClick="imgbtnExcel_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="resultDiv2" runat="server">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <br />
                                                                            <asp:GridView ID="GridViewSip2" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdAltRow" RowStyle-CssClass="grdRow">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Scheme">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Scheme") == DBNull.Value) ? "--" : Eval("Scheme").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Total unit">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Total_unit") == DBNull.Value) ? "--" : Eval("Total_unit").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Total Amount">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Total_amount") == DBNull.Value) ? "--" : Eval("Total_amount").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Present Value">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Present_Value") == DBNull.Value) ? "--" : Eval("Present_Value").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="SIP Returns (%)">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Profit from SIP">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : Eval("Profit_Sip").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--<asp:TemplateField HeaderText="Profit Onetime">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Profit_Onetime") == DBNull.Value) ? "--" : Eval("Profit_Onetime").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>--%>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <br />
                                                                            <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdAltRow" RowStyle-CssClass="grdRow">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                        <ItemTemplate>
                                                                                            <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Eval("Nav_Date").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("NAV") == DBNull.Value) ? "--" : Eval("NAV").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Scheme Units">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Scheme_units") == DBNull.Value) ? "--" : Eval("Scheme_units").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Cumulative Units">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("SCHEME_UNITS_CUMULATIVE") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("SCHEME_UNITS_CUMULATIVE")), 2).ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="SIP Investment Amount">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Scheme_cashflow") == DBNull.Value) ? "--" : Eval("Scheme_cashflow").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--<asp:TemplateField HeaderText="Invested Amount">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("INVESTMENT_AMOUNT") == DBNull.Value) ? "--" : Eval("INVESTMENT_AMOUNT").ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>--%>
                                                                                    <asp:TemplateField HeaderText="Cumulative Fund Value">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("CUMULATIVE_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble( Eval("CUMULATIVE_AMOUNT")),2).ToString()%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="align: justify; padding-left: 5px; padding-right: 5px">
                                                            <div id="disclaimerDiv" runat="server" class="rslt_text" style="align: justify">
                                                                <ul>
                                                                    <li>"Investment" are shown as negative, as it is a cash outflow for the investor. The
                                                                        investment valued on the value date is positive as that is assumed to be cash inflow
                                                                        for the investor. This is necessary for calculating XIRR (Internal rate of Return.</li>
                                                                    <li>Wherever the cash outflow is "0", it is because the scheme had declared dividend
                                                                        on that day which is getting re-invested, and is not a scheduled investment day.</li>
                                                                    <li>"Scheme units" is equal to Investment divided by NAV, and, is the number of units
                                                                        of the scheme which the investor gets for his investment on a particular day.</li>
                                                                </ul>
                                                                <br />
                                                                <span><b>General Disclaimer: This is only a Concept to demonstrate the concept SIP features
                                                                    and should not be construed as a promise, guarantee or a forecast of any minimum
                                                                    returns. </b>The SIP calculator is only for illustrative purposes and does not purport
                                                                    to be an offer for purchase and sale of mutual fund units.<b> Past performance may or
                                                                        may not be sustained in future. </b>
                                                                    <br />
                                                                    <br />
                                                                    Kotak Mahindra Asset Management Company Ltd or ICRA Analytics Ltd or any associated companies or any employee
                                                                    thereof will not liable for any consequences arising out of financial decisions
                                                                    taken based on this or any other financial tool used. Source: ICRA MFI explorer.<br />
                                                                    <br />
                                                                    <b>Mutual Fund Investments are subject to market risks, read all scheme related documents
                                                                        carefully.</b> </span>
                                                                        <br /><span>Developed and Maintained by: <a href="https://www.icraanalytics.com"
                                                                                                                target="_blank">ICRA Analytics Ltd</a>, <a href="https://icraanalytics.com/home/Disclaimer"
                                                                                                            target="_blank">Disclaimer </a></span>
                                                            </div>
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
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
