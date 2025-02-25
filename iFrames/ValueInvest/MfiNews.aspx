<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" Inherits="iFrames.ValueInvest.MfiNews" CodeBehind="MfiNews.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MFI News</title>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table border="0" cellspacing="0" cellpadding="0" width="900" align="left" class="main-content">
                <tr>
                    <td>
                        <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="value_heading" colspan="2">
                                    <img src="img/arw1.jpg" />MFI News</td>

                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="value_heading1" colspan="2">Title Filter&nbsp;&nbsp;
                                                            <%--<input type="text" name="textfield" id="textfield" />--%>
                                                            <asp:TextBox ID="txtSearchBox" runat="server" autoComplete="off"></asp:TextBox>
                                                            <%--<input name="" type="button" value="Go" class="value_go" />--%>
                                                            <asp:Button ID="btnSearch" runat="server" Text="Go" OnClick="btnSearch_Click" class="value_go" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>
<%--                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="value_news_header">Article Title</td>
                                                                    <td class="value_news_header">Report Date</td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="value_news_row"><a href="#">
                                                                        <img src="img/v_bullet.jpg" />ICICI Prudential Mutual Fund revises exit load under its schemes</a></td>
                                                                    <td class="value_news_row">Report Date</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="value_news_row"><a href="#">
                                                                        <img src="img/v_bullet.jpg" />ICICI Prudential Mutual Fund revises exit load under its schemes</a></td>
                                                                    <td class="value_news_row">Report Date</td>
                                                                </tr>
                                                            </table>--%>

                                                            <asp:ListView ID="lvMfinews" runat="server" ItemPlaceholderID="itemPlaceholder" OnPagePropertiesChanging="lvMfinews_PagePropertiesChanging">
                                                                <%--OnPreRender="dpNews_PreRender"--%>
                                                                <LayoutTemplate>
                                                                    <table id="tbl" style="width: 100%;">
                                                                        <tr class="news_header">
                                                                            <td class="value_news_header"></td>
                                                                            <td align="left" class="value_news_header">
                                                                                <asp:Label ID="Label1" runat="server">Article Title</asp:Label>
                                                                            </td>
                                                                            <td class="value_news_header">
                                                                                <asp:Label ID="Label2" runat="server">Report Date</asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder" />
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <%--<tr class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? "news_altrow" : "news_row" %>'>--%>
                                                                    <tr class="value_news_row">
                                                                        <td>
                                                                            <img src="img/news_arw3.png" />&nbsp;&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <a href="NewsDetails.aspx?newsHeadr=<%#  HttpUtility.UrlEncode(Eval("NEWS_HEADLINE").ToString())%>">
                                                                                <%#Eval("NEWS_HEADLINE")%></a>
                                                                            <%--<asp:LinkButton ID="lnkBtn" runat="server"  Text='<%#Eval("NEWS_HEADLINE")%>' PostBackUrl="~/WiseInvest/news_inner.html" ToolTip="j"></asp:LinkButton>--%>
                                                                            <%--<asp:HyperLink ID="hylink" runat="server" Text='<%#Eval("NEWS_HEADLINE")%>' NavigateUrl="NewsDetails.aspx?newsHeadr=<%#Eval("NEWS_HEADLINE")%>"></asp:HyperLink>--%>
                                                                        </td>
                                                                        <td>
                                                                            <%-- <%#Eval("DISPLAY_DATE")%>--%>
                                                                            <%# Convert.ToDateTime(((System.Data.DataRowView)Container.DataItem)["DISPLAY_DATE"]).ToString("dd MMM yyyy")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    Data not Found
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <div style="padding-top: 5px;">
                    </div>
                    <asp:DataPager ID="dpNews" runat="server" PagedControlID="lvMfinews" PageSize="10"
                        EnableViewState="false" ViewStateMode="Disabled">
                        <%--OnPreRender="dpNews_PreRender"--%>
                        <Fields>
                            <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                            <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                            <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <span style="padding-left: 40px;">Page
                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                        of
                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                        (
                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                        records)
                                        <br />
                                    </span>
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                        </Fields>
                    </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="top_text">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="value_dis">Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing. Past performance of the schemes do not indicate the future performance. 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="value_btm_text">Developed and Maintained by: ICRA Analytics Ltd </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="value_btm_text">&nbsp; </td>
                            </tr>

                        </table>

                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
