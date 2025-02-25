<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TotNews.aspx.cs" Inherits="iFrames.Pages.TotNews" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="550px" cellpadding="2" cellspacing="2">
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlMutName" runat="server" />                
            </td>
            <td align="center">
                <asp:DropDownList ID="ddlYear" runat="server" />                
            </td>
            <td align="center">
                <asp:DropDownList ID="ddlMonth" runat="server" />                
            </td>
            <td align="center">
                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click"/>                
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <hr style="color:Gray; border:1.5px; width:425px;" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="justify">
                 <%--<%=News%>--%>   
                 <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table border="1" cellpadding="2" cellspacing="2" style="border-color:Black; border-collapse:collapse;">
                                <tr class="ListtableHead">
                                    <th align="left">News Headline                                       
                                    </th>
                                    <th align="Center">Date 
                                    </th>                                    
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstResult">
                        <Fields>
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <span class="lblHeader">Page
                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                        of
                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling ((double)Container.TotalRowCount / Container.PageSize) %>" />
                                        (
                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                        records)</span>
                                        <br />
                                    
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                            <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<<" ShowNextPageButton="False" NextPageText="<" />
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText=">>" ShowPreviousPageButton="False" PreviousPageText=">" />
                        </Fields>
                    </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr  class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                <td>
                                    <a href="news_viwe.asp?news_headline=<%#HttpUtility.UrlEncode(Eval("news_headline").ToString())%>@<%#Eval("newspaper_name")%>&comID=<%=this.PropCompID %> "><%#Eval("news_headline") %></a>
                                </td>
                                <td>
                                    <asp:Label ID="lblDate" runat="server" Text='<%#Convert.ToDateTime(Eval("DATE").ToString()).ToString("MMM dd, yyyy")%>'></asp:Label>
                                </td>                                
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>No Data Found</EmptyDataTemplate>
                    </asp:ListView>    
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
