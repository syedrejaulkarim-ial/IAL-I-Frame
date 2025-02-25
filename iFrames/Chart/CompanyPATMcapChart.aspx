<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="CompanyPATMcapChart.aspx.cs"
    Inherits="iFrames.Chart.CompanyPATMcapChart" %>

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
    <center>
        <table class="style1" width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr style="height: 50px;">
                <td style="width: 10%; background-color: #007ac2;">
                </td>
                <td style="width: 70%; margin-left: 45%;">
                    <p class="Default">
                        <b><span style="font-size: 30.0pt; font-family: color:Gray; color: #007ac2;">Market
                            Cap follows Earnings Growth </span></b>
                    </p>
                </td>
                <td style="width: 20%;">
                    <asp:image id="Image1" runat="server" imageurl="~/Images/pr_logo.gif" />
                </td>
            </tr>
        </table>
        <br />
        <asp:label id="lblMsgError" runat="server" text="" forecolor="Red" font-bold="true">
        </asp:label>
        <asp:panel id="pnlControls" runat="server" groupingtext="" width="700" font-names="verdana"
            font-size="9">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <div>
                            <table width="100%" cellspacing="1" style="background-color: #007AC2; height: 30px;">
                                <tr>
                                    <td>
                                        <span style="color: White; padding-left: 5px;"><b>Company Name:</b></span>
                                    </td>
                                    <td>
                                        <asp:dropdownlist id="ddlCompany" runat="server" onselectedindexchanged="ddlCompany_SelectedIndexChanged"
                                            autopostback="true">
                                        </asp:dropdownlist>
                                    </td>
                                    <td>
                                        <asp:button id="btnShow" runat="server" text="Show Chart" onclick="btnShow_Click"
                                            validationgroup="CompanyCheck" cssclass="button" visible="false" />
                                    </td>
                                    <td align="right">
                                        <span style="color: White; padding-left: 5px;"><b>Distributor Name:</b></span>
                                        <asp:textbox id="txtDistributor" runat="server" text="">
                                        </asp:textbox>
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
                                            <img alt="" src="images/printer.jpeg" width="16" height="16" style="border:0px;" /></a>
                                    </td>
                                    <td>
                                        <a href="#" onclick="Print();" style="color: #007AC2; text-decoration: none; font-family: Verdana;
                                            font-size: smaller;"><%--<b>Print</b>--%></a>
                                    </td>
                                    <td align="left" style="display:none;">
                                        <asp:linkbutton id="lnkbtnDownload" runat="server" onclick="lnkbtnDownload_Click" visible="false">
                                            <img src="images/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:linkbutton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:panel>
        <br />
        <asp:label id="lblMsg" runat="server" text="" forecolor="Red" font-bold="true"></asp:label>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center">
                    <div>
                        <table width="95%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td id="tdCompName" runat="server" align="left" style="height: 25px; padding-left: 5px;">
                                    <asp:label id="lblCompName" runat="server" text="" forecolor="White" font-bold="true"
                                        font-names="Verdana" font-size="9"></asp:label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;">
                                </td>
                            </tr>
                            <tr>
                                <td id="tdLegend" visible="false" runat="server" align="center" style="height: 25px;">
                                    <div>
                                        <table width="75%" cellpadding="0" cellspacing="0" border="0" style="height: 25px;
                                            border: 1px solid #B8E4F9;">
                                            <tr>
                                                <td style="padding-left: 5px; width: 35%; font-family: Verdana; font-size: small">
                                                    <asp:label id="lblGrowth" runat="server" text="" forecolor="Black" font-bold="true"
                                                        font-names="Verdana"></asp:label>
                                                </td>
                                                <td id="tdEps" runat="server" style="width: 20%;">
                                                    <b style="padding-left: 5px; color: #FFFFFF; font-family: Verdana; font-size: small">
                                                        EPS </b>
                                                    <asp:label id="lblEps" runat="server" text="" forecolor="#FFFFFF" font-bold="true"
                                                        font-names="Verdana" font-size="9"></asp:label>
                                                </td>
                                                <td id="tdMcap" runat="server" style="width: 20%;">
                                                    <b style="padding-left: 5px; color: #FFFFFF; font-family: Verdana; font-size: small">
                                                        M Cap </b>
                                                    <asp:label id="lblMcap" runat="server" text="" forecolor="#FFFFFF" font-bold="true"
                                                        font-names="Verdana" font-size="9"></asp:label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <asp:panel id="pnlChart" runat="server" visible="false" width="100%">
                            <asp:chart id="chrtCompanyPrice" runat="server" height="230px" palette="none" width="975px"
                                imagelocation="~/TempImages/ChartPic_#SEQ(300,3)" backcolor="Silver" backgradientstyle="Center"
                                borderdashstyle="Solid" borderwidth="2">
                                <legends>
                                    <asp:Legend Enabled="False" IsTextAutoFit="true" Name="Legend1" BackColor="Transparent"
                                        Alignment="Center" LegendStyle="Row" Docking="Top" Font="Trebuchet MS, 8.25pt, style=Bold">
                                    </asp:Legend>
                                </legends>
                                <chartareas>
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
                                </chartareas>
                            </asp:chart>
                        </asp:panel>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:panel id="pnlgvCompanyShare" runat="server" visible="false">
                        <asp:gridview id="gvCompanyShare" runat="server" width="95%" autogeneratecolumns="False"
                            font-size="Small" borderwidth="1px" backcolor="White" cellpadding="4" borderstyle="None"
                            bordercolor="#3366CC" gridlines="Both" onpageindexchanging="gvCompanyShare_PageIndexChanging">
                            <pagerstyle forecolor="White" horizontalalign="right" backcolor="#007AC2"></pagerstyle>
                            <headerstyle forecolor="White" font-bold="True" backcolor="#007AC2" font-names="verdana"
                                font-size="9"></headerstyle>
                            <footerstyle forecolor="#003399" backcolor="#99CCCC"></footerstyle>
                            <columns>
                                <asp:TemplateField HeaderText="FY End" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <%#FomatDate(DataBinder.Eval(Container.DataItem, "FY_End"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EPS" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "EPS")%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="YoY EPS Growth (%)" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "EPSGrowth"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" M Cap" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "M_Cap"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="YoY M Cap Growth (%)" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "M_CapGrowth"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PE Ratio" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "PE"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Share Price" HeaderStyle-Width="20%" Visible="false">
                                    <ItemTemplate>
                                        <%#CheckBlank(DataBinder.Eval(Container.DataItem, "SharePrice"))%>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </columns>
                        </asp:gridview>
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
                                        is rebased to 100.</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5px;" align="left" valign="middle">
                                    <img src="images/bullet.jpeg" alt="" width="5" height="5" />
                                </td>
                                <td align="left">
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">PE-Price to Earnings
                                        ratio </span>
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
                                    <span style="color: Black; font-family: Verdana; font-size: 9pt">Standalone EPS is adjusted
                                        as per the corporate action. </span>
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
                    </asp:panel>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
