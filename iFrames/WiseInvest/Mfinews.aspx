<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="Mfinews.aspx.cs" Inherits="iFrames.WiseInvest.Mfinews" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MFI News</title>
    <link href="css/template_css.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/navigation.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="0" cellpadding="0" width="710" align="left">
            <tr>
                <td style="display: none">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr class="news_head">
                            <td width="2%">
                                <img src="img/news_arw.png" width="10" height="9" />
                            </td>
                            <td width="98%" align="left" valign="middle">
                                <strong>&nbsp;</strong>Mutual Fund News Update
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="top_icon">
                                <img src="img/mfi_news.png" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="15px" colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td width="5%">
                            </td>
                            <td class="title_filter">
                                Title Filter
                            </td>
                            <td style="margin: 0; padding: 0;">
                                <asp:TextBox ID="txtSearchBox" runat="server" autoComplete="off"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Go" OnClick="btnSearch_Click" class="back" />
                                <%--OnClientClick="Javascript:Encode();"--%>
                            </td>
                        </tr>
                        <tr>
                            <td height="15px" colspan="4">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListView ID="lvMfinews" runat="server" ItemPlaceholderID="itemPlaceholder" OnPagePropertiesChanging="lvMfinews_PagePropertiesChanging">
                        <%--OnPreRender="dpNews_PreRender"--%>
                        <LayoutTemplate>
                            <table id="tbl" style="width: 90%;">
                                <tr class="news_header">
                                    <td class="text_border1">
                                    </td>
                                    <td align="left" class="text_border1">
                                        <asp:Label ID="Label1" runat="server">Article Title</asp:Label>
                                    </td>
                                    <td class="text_border1">
                                        <asp:Label ID="Label2" runat="server">Report Date</asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <%--<tr class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? "news_altrow" : "news_row" %>'>--%>
                            <tr class="news_row1">
                                <td>
                                    <img src="../DSP/IMG/news_arw3.png" />&nbsp;&nbsp;
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
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="disclaimerh">
                                Disclaimer
                            </td>
                        </tr>
                        <tr>
                            <td class="disclaimer">
                               Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing.  Past performance of the schemes do not indicate the future performance.
                            </td>
                        </tr>
                        <tr>
                            <td class="text" align="right">
                                <span style="text-align: right" class="rslt_text1">Developed and Maintained by:<a
                                    class="text" href="https://www.icraanalytics.com" target="_blank"> ICRA Analytics Ltd </a>
                                </span>
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
