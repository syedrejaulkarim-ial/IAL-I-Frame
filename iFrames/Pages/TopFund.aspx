<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.Pages.TopFund" %>
<%--<%@ MasterType VirtualPath ="~/Master Pages/msMutualFund.Master" %>--%>
<%--<asp:Content ContentPlaceHolderID="cpChildContent" runat="server">
 <table width="450px" cellpadding="3" cellspacing="3">
            <tr>
                <td>
                    Rank
                </td>
                <td>
                    <asp:DropDownList ID="ddlRank" runat="server">
                        <asp:ListItem Text="All" Value="0" />
                        <asp:ListItem Text="top 5" Value="5" />
                        <asp:ListItem Text="top 10" Value="10" />
                        <asp:ListItem Text="top 15" Value="15" />
                    </asp:DropDownList>
                </td>
                <td>
                    Type
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Open Ended" Value="Open Ended" />
                        <asp:ListItem Text="Closed Ended" Value="Closed Ended" />
                    </asp:DropDownList>
                </td>
                <td>
                    Category
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server">
                        <asp:ListItem Value="" Text="All" Selected="True" />
                        <asp:ListItem Value="Equity" Text="Equity" />
                        <asp:ListItem Value="Debt" Text="Debt" />
                        <asp:ListItem Value="Gilt" Text="Gilt" />
                        <asp:ListItem Value="ETF" Text="ETF" />
                        <asp:ListItem Value="Fund of Funds" Text="Fund of Funds" />
                        <asp:ListItem Value="Short Term Debt" Text="Liquid" />
                        <asp:ListItem Value="Tax" Text="Equity (Tax Planning)" />
                        <asp:ListItem Value="Diversified" Text="Equity (Diversified)" />
                        <asp:ListItem Value="Sector" Text="Equity (Sector)" />
                        <asp:ListItem Value="Index" Text="Equity (Index)" />
                        <asp:ListItem Value="Short" Text="Debt (Short Term)" />
                        <asp:ListItem Value="Income" Text="Debt (Income)" />
                        <asp:ListItem Value="MIP" Text="Debt (MIP)" />
                        <asp:ListItem Value="Equity & Debt" Text="Balanced" />
                    </asp:DropDownList>
                </td>
                <td>
                    Period
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriod" runat="server">
                        <asp:ListItem Value="per_7days" Text="Last 1 Week" Selected="True" />
                        <asp:ListItem Value="per_30days" Text="Last 1 Month" />
                        <asp:ListItem Value="per_91days" Text="Last 3 Month" />
                        <asp:ListItem Value="per_182days" Text="Last 6 Month" />
                        <asp:ListItem Value="per_1yr" Text="Last 12 Month" />
                        <asp:ListItem Value="per_3yr" Text="Last 3 Years" />
                        <asp:ListItem Value="per_5yr" Text="Last 5 Years" />
                        <asp:ListItem Value="since_incept" Text="Since Inception" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table border="0" cellpadding="2">
                                <tr style="background-color: #E5E5FE">
                                    <th align="left">
                                        <asp:Label ID="lnkRank" runat="server" CommandName="Sort" CommandArgument="Rank"
                                            Text="Rank" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkSchName" runat="server" CommandName="Sort" CommandArgument="SchName"
                                            Text="Scheme Name" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkDate" runat="server" CommandName="Sort" CommandArgument="Date"
                                            Text="Date" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkNav" runat="server" CommandName="Sort" CommandArgument="NAV"
                                            Text="NAV (Rs.)" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkPeriod" runat="server" CommandName="Sort" CommandArgument="<%=ddlPeriod.SelectedValue%>"><%=ddlPeriod.SelectedItem.Text%></asp:Label>
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkInception" runat="server" CommandName="Sort" CommandArgument="Inception"
                                            Text="Since Inception" />
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstResult">
                        <Fields>
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <b>Page
                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                        of
                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling ((double)Container.TotalRowCount / Container.PageSize) %>" />
                                        (
                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                        records)
                                        <br />
                                    </b>
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                            <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<<" ShowNextPageButton="False" NextPageText="<" />
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText=">>" ShowPreviousPageButton="False" PreviousPageText=">" />
                        </Fields>
                    </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                </td>
                                <td>
                                    <a href="fundfactsheet1.asp?sname=<%#Eval("sch_code")%>"><%#Eval("sch_name")%></a>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("date").ToString()).ToString("MMM dd, yyyy")%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav_Rs")%>'/>
                                </td>
                                 <td>
                                    <asp:Label runat="server" ID="Label2" Text='<%#Eval(ddlPeriod.SelectedValue)%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label3" text='<%#Eval("since_incept")%>'/>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>        
</asp:Content>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="550px" cellpadding="3" cellspacing="3">
            <tr class="content">
                <td>
                    <strong>Rank</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRank" runat="server">
                        <asp:ListItem Text="All" Value="0" />
                        <asp:ListItem Text="top 5" Value="5" />
                        <asp:ListItem Text="top 10" Value="10" />
                        <asp:ListItem Text="top 15" Value="15" />
                    </asp:DropDownList>
                </td>
                <td>
                    <strong>Type</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Open Ended" Value="Open Ended" />
                        <asp:ListItem Text="Closed Ended" Value="Closed Ended" />
                    </asp:DropDownList>
                </td>
                <td>
                    <strong>Category</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server">
                        <asp:ListItem Value="" Text="All" Selected="True" />
                        <asp:ListItem Value="Equity" Text="Equity" />
                        <asp:ListItem Value="Debt" Text="Debt" />
                        <asp:ListItem Value="Gilt" Text="Gilt" />
                        <asp:ListItem Value="ETF" Text="ETF" />
                        <asp:ListItem Value="Fund of Funds" Text="Fund of Funds" />
                        <asp:ListItem Value="Short Term Debt" Text="Liquid" />
                        <asp:ListItem Value="Tax" Text="Equity (Tax Planning)" />
                        <asp:ListItem Value="Diversified" Text="Equity (Diversified)" />
                        <asp:ListItem Value="Sector" Text="Equity (Sector)" />
                        <asp:ListItem Value="Index" Text="Equity (Index)" />
                        <asp:ListItem Value="Short" Text="Debt (Short Term)" />
                        <asp:ListItem Value="Income" Text="Debt (Income)" />
                        <asp:ListItem Value="MIP" Text="Debt (MIP)" />
                        <asp:ListItem Value="Equity & Debt" Text="Balanced" />
                    </asp:DropDownList>
                </td>
                <td>
                    <strong>Period</strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriod" runat="server">
                        <asp:ListItem Value="per_7days" Text="Last 1 Week" Selected="True" />
                        <asp:ListItem Value="per_30days" Text="Last 1 Month" />
                        <asp:ListItem Value="per_91days" Text="Last 3 Month" />
                        <asp:ListItem Value="per_182days" Text="Last 6 Month" />
                        <asp:ListItem Value="per_1yr" Text="Last 12 Month" />
                        <asp:ListItem Value="per_3yr" Text="Last 3 Years" />
                        <asp:ListItem Value="per_5yr" Text="Last 5 Years" />
                        <asp:ListItem Value="since_incept" Text="Since Inception" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table border="1" cellpadding="3" cellspacing="3" style="border-color:Black;border-collapse:collapse;">
                                <tr class="ListtableHead">
                                    <th align="left">
                                        <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank"
                                            Text="Rank" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                            Text="Scheme Name" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date"
                                            Text="Date" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                            Text="NAV (Rs.)" />
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="<%=ddlPeriod.SelectedValue%>"><%=ddlPeriod.SelectedItem.Text%></asp:Label>
                                    </th>
                                    <th align="left">
                                        <asp:Label ID="lnkInception" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Inception"
                                            Text="Since Inception" />
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstResult">
                        <Fields>
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <span class="labelsHead">Page
                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                        of
                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling ((double)Container.TotalRowCount / Container.PageSize) %>" />
                                        (
                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                        records)
                                        <br />
                                        </span>
                                    
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                            <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<<" ShowNextPageButton="False" NextPageText="<" />
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText=">>" ShowPreviousPageButton="False" PreviousPageText=">" />
                        </Fields>
                    </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                <td>
                                    <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                </td>
                                <td>
                                    <a href="fundfactsheet1.asp?sname=<%#Eval("sch_code")%>&comID=<%=this.PropCompID %>"><%#Eval("sch_name")%></a>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("date").ToString()).ToString("MMM dd, yyyy")%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav_Rs")%>'/>
                                </td>
                                 <td>
                                    <asp:Label runat="server" ID="Label2" Text='<%#Eval(ddlPeriod.SelectedValue)%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label3" text='<%#Eval("since_incept")%>'/>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>        
    </div>
    </form>
</body>
</html>
