<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopperPages.aspx.cs" Inherits="iFrames.Pages.TopperPages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table width="650px" cellpadding="2" cellspacing="2">
            <tr class="mainHeader">
                <td align="left">                                  
                            <%=MainHeader%> 
                    <hr />
                    <br style="line-height: 25px;" />                    
                </td>
            </tr>
            <tr class="content">
            <td>
            <strong>  
Infotech Schemes  
 
Ranked on the basis of their performance (%) as on Nov 2, 2010. Click on 'Time Period' to rank funds on a particular period of your choice. 
</strong>
            </td>
            </tr>
            <tr>
                <td>
                     <asp:ListView ID="lstOpenTaxSect" runat="server" OnSorting="lstOpenTaxSect_Sorting" 
                    OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table border="1" cellpadding="3" style="border-collapse:collapse; border-color:Black;">
                                <tr class="ListtableHead">
                                    <th align="left" rowspan="2">
                                         <asp:LinkButton ID="lnkRank" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="Rank"
                                            Text="Rank" />
                                    </th>
                                    <th align="left" rowspan="2">
                                         <asp:LinkButton ID="lnkSchName" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="short_name"
                                            Text="Scheme Name" />
                                    </th>
                                    <th align="left" rowspan="2">
                                         <asp:LinkButton ID="lnkDate" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="Date"
                                            Text="Date" />
                                    </th>
                                    <th colspan="5" align="center">
                                         <asp:label ID="lnkPerformance" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="Performance"
                                            Text="Performance" />
                                    </th>                                    
                                </tr>
                                <tr class="ListtableHead">
                                    <th>  <asp:LinkButton ID="lbl1m" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="per_30days"
                                            Text="1 Mth %" /></th>
                                            <th><asp:LinkButton ID="lbl3m" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="per_91days"
                                            Text="3 Mth %" /></th>
                                            <th>  <asp:LinkButton ID="lbl6m" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="per_182days"
                                            Text="6 Mth %" /></th>
                                            <th>  <asp:LinkButton ID="lbl1y" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="per_1yr"
                                            Text="1 Yr %" /></th>
                                            <th>  <asp:LinkButton ID="lbl3y" runat="server"  Font-Bold="true" ForeColor="White" CommandName="Sort" CommandArgument="per_3yr"
                                            Text="1 Yrs %" /></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstOpenTaxSect">
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
                                    <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                </td>
                                <td>
                                    <a href="fundfactsheet1.asp?sname=<%#Eval("sch_code")%>&comID=<%=this.PropCompID %>"><%#Eval("short_name")%></a>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("date").ToString()).ToString("MMM dd, yyyy")%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbl30d" Text='<%#Eval("per_30days")%>'/>
                                </td>
                                 <td>
                                    <asp:Label runat="server" ID="lbl91d" Text='<%#Eval("per_91days")%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbl182d" text='<%#Eval("per_182days")%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbl1yr" text='<%#Eval("per_1yr")%>'/>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbl3yr" text='<%#Eval("per_3yr")%>'/>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>                   
                </td>
            </tr>
            <tr class="content">
                <td>
                    *Note:- Returns calculated for less than 1 year are Absolute returns and returns calculated for more than 1 year are compounded annualized. 
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
