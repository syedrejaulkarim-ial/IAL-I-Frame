<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="MainPage.aspx.cs" Inherits="iFrames.WiseInvest.MainPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Wise</title>
    <link href="css/template_css.css" rel="stylesheet" type="text/css" media="all" />
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function Encode() {
            //alert(1);
            var txtserch = $('#txtSearchBox').val();
            txtserch = escape(txtserch);
            // alert(txtserch);
        }    
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <act:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </act:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
        <ProgressTemplate>
            <img src="../Images/ajax-loader.gif" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <act:TabContainer ID="MainTab" runat="server" ActiveTabIndex="0" ScrollBars="Horizontal"
                    Width="800px" Height="700px">
                    <act:TabPanel runat="server" HeaderText="NewsPanel" ID="TabPanel1">
                        <HeaderTemplate>
                            <asp:Label ID="newsLabel" runat="server" Text="News"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" align="center" class="main-content">
                                <tr>
                                    <td>
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
                                        <table border="0" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="10%">
                                                </td>
                                                <td class="title_filter">
                                                    Title Filter
                                                    <td>
                                                        <asp:TextBox ID="txtSearchBox" runat="server" autoComplete="off"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearch" runat="server" Text="Go" OnClientClick="Javascript:Encode();"
                                                            OnClick="btnSearch_Click" class="back" />
                                                    </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListView ID="lvMfinews" runat="server" OnPagePropertiesChanging="lvMfinews_PagePropertiesChanging">
                                            <EmptyDataTemplate>
                                                Data not Found
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? "news_altrow" : "news_row" %>'>
                                                    <td>
                                                        <img src="img/news_arw1.png" />&#160;&#160;
                                                    </td>
                                                    <td>
                                                        <a href='NewsDetails.aspx?newsHeadr=<%#Eval("NEWS_HEADLINE")%>'>
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
                                            <LayoutTemplate>
                                                <table id="tbl">
                                                    <tr class="news_header">
                                                        <th>
                                                        </th>
                                                        <th align="left">
                                                            <asp:Label ID="Label1" runat="server">Article Title</asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label2" runat="server">Report Date</asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                        </asp:ListView>
                                        <asp:DataPager ID="dpNews" runat="server" PagedControlID="lvMfinews" EnableViewState="False"
                                            ViewStateMode="Disabled">
                                            <Fields>
                                                <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                                                <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                                <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <span style="padding-left: 100px;">Page
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
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
                        <HeaderTemplate>
                            <asp:Label ID="Label1" runat="server" Text="Top Fund"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            hello
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                        <HeaderTemplate>
                            <asp:Label ID="Label2" runat="server" Text="NFO Update"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            hello
                        </ContentTemplate>
                    </act:TabPanel>
                    <act:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4">
                        <HeaderTemplate>
                            <asp:Label ID="Label3" runat="server" Text="Compare Fund"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            hello
                        </ContentTemplate>
                    </act:TabPanel>
                </act:TabContainer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
