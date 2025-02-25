<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="CompanyPAT_ShareChart.aspx.cs"
    Inherits="iFrames.Chart.CompanyPAT_ShareChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="css/Chart.css" />
    <script type="text/javascript" language="javascript">
        function Print() {
            window.print();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <table class="style1" width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr style="height: 50px;">
                <td style="width: 10%; background-color: #007ac2;">
                </td>
                <td style="width: 70%; margin-left: 45%;">
                    <p class="Default">
                        <b><span style="font-size: 30.0pt; font-family: color:Gray; color: #007ac2;">Share Price
                            follows Earnings Growth </span></b>
                    </p>
                </td>
                <td style="width: 20%;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/pr_logo.gif" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblMsgError" runat="server" Text="" ForeColor="Red" Font-Bold="true">
        </asp:Label>
        <asp:Panel ID="pnlControls" runat="server" GroupingText="" Width="700" Font-Names="verdana"
            Font-Size="9">
            <table width="100%" cellpadding="0" cellspacing="1" border="0">
                <tr>
                    <td>
                        <div>
                            <table width="100%" cellspacing="1" style="background-color: #007AC2; height: 30px;">
                                <tr>
                                    <td>
                                        <span style="color: White; padding-left: 5px;"><b>Company Name:</b></span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCompany" runat="server" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnShow" runat="server" Text="Show Chart" OnClick="btnShow_Click"
                                            ValidationGroup="CompanyCheck" CssClass="button" Visible="false" />
                                    </td>
                                    <td align="right">
                                        <span style="color: White; padding-left: 5px;"><b>Distributor Name:</b></span>
                                        <asp:TextBox ID="txtDistributor" runat="server" Text="">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td align="right" style="display: block;">
                        <div>
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <a href="#" onclick="Print();">
                                            <img alt="" src="images/printer.jpeg" width="16" height="16" style="border: 0px;" /></a>
                                    </td>
                                    <td>
                                        <a href="#" onclick="Print();" style="color: #007AC2; text-decoration: none; font-family: Verdana;
                                            font-size: smaller;">
                                            <%--<b>Print</b>--%></a>
                                    </td>
                                    <td align="left" style="display:none;">
                                        <asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="lnkbtnDownload_Click">
                                            <img src="images/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center">
                    <div>
                        <table width="95%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td id="tdCompName" runat="server" align="left" style="height: 25px; padding-left: 5px;"
                                    colspan="4">
                                    <asp:Label ID="lblCompName" runat="server" Text="" ForeColor="White" Font-Bold="true"
                                        Font-Names="Verdana" Font-Size="9"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td id="tdLegend" visible="false" runat="server" align="center" style="height: 25px;">
                                    <div>
                                        <table width="75%" cellpadding="0" cellspacing="0" style="height: 25px; border: 1px solid #B8E4F9;">
                                            <tr>
                                                <td style="padding-left: 5px; width: 35%; font-family: Verdana; font-size: small">
                                                    <asp:Label ID="lblGrowth" runat="server" Text="" ForeColor="Black" Font-Bold="true"
                                                        Font-Names="Verdana"></asp:Label>
                                                </td>
                                                <td id="tdEps" runat="server" style="width: 20%;">
                                                    <b style="padding-left: 5px; color: #FFFFFF; font-family: Verdana; font-size: small">
                                                        EPS </b>
                                                    <asp:Label ID="lblEps" runat="server" Text="" ForeColor="#FFFFFF" Font-Bold="true"
                                                        Font-Names="Verdana" Font-Size="9"></asp:Label>
                                                </td>
                                                <td id="tdShare" runat="server" style="width: 20%;">
                                                    <b style="padding-left: 5px; color: #FFFFFF; font-family: Verdana; font-size: small">
                                                        Share Price </b>
                                                    <asp:Label ID="lblShare" runat="server" Text="" ForeColor="#FFFFFF" Font-Bold="true"
                                                        Font-Names="Verdana" Font-Size="9"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <asp:Panel ID="pnlCompanyShare" runat="server" Visible="false" Width="100%">
                            <asp:Chart ID="chrtCompanyShare" runat="server" Height="230px" Palette="none" Width="975px"
                                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" BackColor="Silver" BackGradientStyle="Center"
                                borderdashstyle="Solid" BorderWidth="2">
                                <Legends>
                                    <asp:Legend Enabled="False" IsTextAutoFit="true" Name="Legend1" BackColor="Transparent"
                                        Alignment="Center" LegendStyle="Row" Docking="Top" Font="Trebuchet MS, 8.25pt, style=Bold">
                                    </asp:Legend>
                                </Legends>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                                        BackSecondaryColor="White" BackColor="#ECF4F9" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                        <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                            WallWidth="0" IsClustered="False"></Area3DStyle>
                                        <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </AxisY>
                                        <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <%--<MajorGrid LineColor="64, 64, 64, 64" />--%>
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </asp:Panel>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlgvCompanyShare" runat="server" Visible="false">
                        <asp:GridView ID="gvCompanyShare" runat="server" Width="95%" AutoGenerateColumns="False"
                            Font-Size="Small" BorderWidth="1px" BackColor="White" CellPadding="4" BorderStyle="None"
                            BorderColor="#3366CC" GridLines="Both" OnPageIndexChanging="gvCompanyShare_PageIndexChanging">
                            <PagerStyle ForeColor="White" HorizontalAlign="Right" BackColor="#007AC2" CssClass="pager">
                            </PagerStyle>
                            <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#007AC2" Font-Names="Verdana"
                                Font-Size="9"></HeaderStyle>
                            <FooterStyle ForeColor="#003399" BackColor="#84AFC8"></FooterStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="FY End" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <%#FomatDate(DataBinder.Eval(Container.DataItem, "FY_End"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EPS" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "EPS")%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="YoY EPS Growth (%)" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "EPSGrowth"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actual M Cap" HeaderStyle-Width="10%" Visible="false">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "M_Cap"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Market Cap Growth %" HeaderStyle-Width="10%" ItemStyle-Font-Names="verdana"
                                    ItemStyle-Font-Size="10" Visible="false">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "M_CapGrowth"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Share Price" HeaderStyle-Width="10%" ItemStyle-Font-Names="verdana"
                                    ItemStyle-Font-Size="10">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "SharePrice"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="YoY Share Price Growth (%) " HeaderStyle-Width="15%"
                                    ItemStyle-Font-Names="verdana" ItemStyle-Font-Size="10">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "SharePriceGrowth"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dividend Yield (%) " HeaderStyle-Width="10%" ItemStyle-Font-Names="verdana"
                                    ItemStyle-Font-Size="10">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "Dividend_Yield")%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PE Ratio" HeaderStyle-Width="10%" ItemStyle-Font-Names="verdana"
                                    ItemStyle-Font-Size="10">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "PE"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <table width="95%" cellpadding="0" cellspacing="4" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <span style="color: Black; font-family: Verdana; font-weight: bold; font-size: 9pt;">
                                        Disclaimer:</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5px;" align="left" valign="middle">
                                    <img src="images/bullet.jpeg" alt="" width="5" height="5" />
                                </td>
                                <td align="left">
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">Data for the graph
                                        is rebased to 100. </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5px;" align="left" valign="middle">
                                    <img src="images/bullet.jpeg" alt="" width="5" height="5" />
                                </td>
                                <td align="left">
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">PE-Price to Earnings
                                        ratio.</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5px;" align="left" valign="middle">
                                    <img src="images/bullet.jpeg" alt="" width="5" height="5" />
                                </td>
                                <td align="left">
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">EPS-Earning per share
                                        in Rs. </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5px;" align="left" valign="middle">
                                    <img src="images/bullet.jpeg" alt="" width="5" height="5" />
                                </td>
                                <td align="left">
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">Standalone EPS and
                                        share price is adjusted as per the corporate action. </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5px;" align="left" valign="middle">
                                    <img src="images/bullet.jpeg" alt="" width="5" height="5" />
                                </td>
                                <td align="left">
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">CAGR calculated from
                                        the first positive value. </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5px;" align="left" valign="middle">
                                    <img src="images/bullet.jpeg" alt="" width="5" height="5" />
                                </td>
                                <td align="left">
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">Powered by ICRA Analytics
                                        Ltd.</span>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
