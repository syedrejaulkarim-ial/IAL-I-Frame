<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="KotakSip.aspx.cs" Inherits="iFrames.Kotak.KotakSip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .textboxwidth
        {
            width: 120px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="table1">
            <tr>
                <td>
                    <div id="sipDiv">
                        <table id="sipTable" border="0" cellspacing="1" cellpadding="1" width="100%">
                            <tr>
                                <td style="width: 175px" valign="top">
                                    <asp:Label ID="Label12" runat="server" CssClass="FieldHead" Width="96px" Height="17px"
                                        Font-Name="Vardana"> Scheme Name</asp:Label>
                                </td>
                                <td style="width: 319px" valign="top" align="left" colspan="3">
                                    <asp:DropDownList ID="ddlscheme" runat="server" CssClass="ddl" Width="320px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="display: ">
                                <td>
                                    <asp:Label ID="Label25" runat="server" CssClass="FieldHead" Width="48px" Height="20px"
                                        Font-Name="Vardana">Benchmark</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlsipbnmark" runat="server" CssClass="ddl" Width="320px" Enabled="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 175px" valign="top">
                                    <asp:Label ID="Label13" runat="server" CssClass="FieldHead" Width="175px" Font-Name="Vardana">Installment Amount (Rs.)</asp:Label>
                                </td>
                                <td style="width: 2s 19px" valign="top" align="left">
                                    <asp:TextBox ID="txtinstall" CssClass="textboxwidth" MaxLength="14" Text="1000" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="width: 130px">
                                    <asp:Label ID="Label28" runat="server" CssClass="FieldHead">Frequency</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddPeriod_SIP" runat="server" CssClass="textboxwidth">
                                        <asp:ListItem Value="0">--</asp:ListItem>
                                        <asp:ListItem Value="1">Monthly</asp:ListItem>
                                        <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label35" runat="server" CssClass="FieldHead" Width="56px">SIP Date</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="textboxwidth">
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
                                    <asp:Label ID="Label14" runat="server" CssClass="FieldHead" Font-Name="Vardana">From Date</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="textboxwidth" Font-Name="Vardana"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label15" runat="server" CssClass="FieldHead" Width="56px" Font-Name="Vardana">To Date</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="textboxwidth" Font-Name="Vardana"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px">
                                    <asp:Label ID="Label16" runat="server" CssClass="FieldHead" Font-Name="Vardana">Value as on Date</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtvalason" runat="server" CssClass="textboxwidth" Font-Name="Vardana"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" height="5px">
                                    * Enter date in DD/MM/YYYY format
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:Button ID="sipbtnshow" runat="server" Text="Submit" OnClick="btnSip_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Chart ID="chrtResult" runat="server" AlternateText="Kotak Sip" Visible="false"
                        BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="639px" Height="350px"
                        BackGradientStyle="Center">
                         <Legends>
                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent" 
                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold">
                                    </asp:Legend>
                                </Legends>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="CornflowerBlue" AlignmentStyle="PlotPosition">
                                <Area3DStyle Enable3D="false" />
                                <AxisX ArrowStyle="SharpTriangle"  LineColor="DodgerBlue" ToolTip="SIP period">
                                    <ScaleBreakStyle Enabled="True" />
                                     <MajorGrid LineColor="64, 64, 64, 64" />                                     
                                </AxisX>
                                <AxisY ArrowStyle="SharpTriangle" ToolTip="Amount" LineColor="DodgerBlue">
                                    <ScaleBreakStyle Enabled="True" />
                                </AxisY>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <div id="resultDiv" runat="server" visible="false">
                        <table width="100%">
                            <tr>
                                <td>
                                    <div id="infodiv" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTotalInvst" runat="server" Text="Total Invested Amount"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblInvstvalue" runat="server" Text="On Date C, the Scheme value of your total investment Rs Y would be Rs Z"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    For Return calculation, Check table below:
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(Eval("Nav_Date").ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
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
                                            <asp:TemplateField HeaderText="Scheme Cashflow">
                                                <ItemTemplate>
                                                    <%# (Eval("Scheme_cashflow") == DBNull.Value) ? "--" : Eval("Scheme_cashflow").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <%# (Eval("AMOUNT") == DBNull.Value) ? "--" : Eval("AMOUNT").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cumulative Amount">
                                                <ItemTemplate>
                                                    <%# (Eval("CUMULATIVE_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble( Eval("CUMULATIVE_AMOUNT")),2).ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Index Value">
                                                <ItemTemplate>
                                                    <%# (Eval("Index_Value_amount") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble( Eval("Index_Value_amount")),2).ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    At A glance<br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GridViewSip2" runat="server" AutoGenerateColumns="False" Width="100%">
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
                                            <asp:TemplateField HeaderText="Yield">
                                                <ItemTemplate>
                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Profit Sip">
                                                <ItemTemplate>
                                                    <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : Eval("Profit_Sip").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Profit Onetime">
                                                <ItemTemplate>
                                                    <%# (Eval("Profit_Onetime") == DBNull.Value) ? "--" : Eval("Profit_Onetime").ToString()%>
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
        </table>
    </div>
    </form>
</body>
</html>
