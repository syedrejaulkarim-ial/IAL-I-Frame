<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundFactSheet2.aspx.cs"
    Inherits="iFrames.Pages.FundFactSheet2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="540px" cellpadding="2" cellspacing="2">
            <tr>
                <td align="justify" colspan="2" style="height: 55px;">
                    <span class="mainHeader">NAV - </span><span class="SubHeader">
                        <%=shortName%></span>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="menu">
                        <ul>
                            <li><a href='FundFactSheet1.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Fund
                                Facts</a></li>
                            <li><a href='FundFactSheet2.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>NAV</a></li>
                            <li><a href='FundFactSheet3.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Risk
                                Return</a></li>
                            <li><a href='FundFactSheet4.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Portfolio</a></li>
                            <li><a href='FundFactSheet5.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>News</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <br style="line-height: 30px;" />
                    <table cellpadding="1" cellspacing="1" border="1" width="100%" style="border-color: Black;
                        border-collapse: collapse;">
                        <tr class="ListtableRow">
                            <td align="justify">
                                <strong>Latest NAV</strong>
                            </td>
                            <td align="justify">
                                <%=navRs%>
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td align="justify">
                                <strong>Benchmark Index<%=ind_name%></strong>
                            </td>
                            <td align="justify">
                                <%=ind_val%>
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td align="justify">
                                <strong>52 - Week High</strong>
                            </td>
                            <td align="justify">
                                <%=MaxNav%>
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td align="justify">
                                <strong>52 - Week Low</strong>
                            </td>
                            <td align="justify">
                                <%=MinNav%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Chart ID="Chart1" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                        ImageType="Png" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom"
                        BackSecondaryColor="White" BorderDashStyle="Solid" BorderColor="26, 59, 105"
                        Height="296px" Width="530px" Palette="None">
                        <Titles>
                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                Text="NAV details for last one month" ForeColor="26, 59, 105">
                            </asp:Title>
                        </Titles>
                        <%--<Legends>
                            <asp:Legend Enabled="False" IsTextAutoFit="False" Name="Default" BackColor="Transparent" LegendStyle="Table" Docking="Bottom"
                                Font="Trebuchet MS, 8.25pt, style=Bold">
                            </asp:Legend>
                        </Legends>--%>
                        <BorderSkin SkinStyle="Emboss"></BorderSkin>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                                BackSecondaryColor="White" BackColor="Gainsboro" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                    WallWidth="0" IsClustered="False"></Area3DStyle>
                                <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
