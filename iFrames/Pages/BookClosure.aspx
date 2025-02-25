<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookClosure.aspx.cs" Inherits="iFrames.Pages.BookClosure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="scriptManager" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <table width="550px" border="0" cellpadding="3" cellspacing="3" style="height: 400px;
            float: inherit;">
            <tr>
                <td valign="top">
                    <table style="margin-top: 30px;">
                        <tr class="mainHeader">
                            <td>
                                BOOK CLOSURES
                                <hr />
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>                     
                 
                    </table>
                </td>
            </tr>
            <tr class="SubHeader">
                <td>
                    Book closures during past one month.
                    <br style="line-height: 15px;" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListView ID="lstBookClosure" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table cellpadding="3" border="1" cellspacing="3" style="border-color: Black; border-collapse: collapse;">
                               <tr class="ListtableHead">
                               <th align="center" rowspan="2" ><asp:Label ID="lbl1" SkinID="lblHeader" runat="server">Scheme Name</asp:Label></th>
                               <th align="center" colspan="2"><asp:Label ID="Label1" SkinID="lblHeader" runat="server">Book Closure</asp:Label></th>
                               <th align="center" colspan="2"><asp:Label ID="Label2" SkinID="lblHeader" runat="server">Purpose</asp:Label></th>
                               </tr>
                               <tr class="ListtableHead">
                               <th align="center"><asp:Label ID="lbl2" SkinID="lblHeader" runat="server">From</asp:Label></th>
                               <th align="center"><asp:Label ID="lbl3" SkinID="lblHeader" runat="server">To</asp:Label></th>                               
                               
                              </tr>
                                <tr runat="server" id="itemPlaceholder">
                                </tr>
                            </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstBookClosure">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <span class="lblHeader">Page
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
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowNextPageButton="false"
                                        ShowPreviousPageButton="false" />
                                    <asp:NumericPagerField PreviousPageText="&lt; Prev 10" NextPageText="Next 10 &gt;"
                                        ButtonCount="10" />
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowNextPageButton="false"
                                        ShowPreviousPageButton="false" />
                                </Fields>
                            </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                <td>
                                     <a href="fund_facts_rpt.aspx?sname=<%#Eval("Sch_code")%>&comID=<%=this.PropCompID %>"><%#Eval("Sch_Name").ToString()%></a> 
                                </td>
                                <td>
                                    <%#Eval("BC_from").ToString() != "" ? Convert.ToDateTime(Eval("BC_from").ToString()).ToString("MMM dd, yyyy") : "NA"%>
                                </td>
                                <td>
                                    <%#Eval("BC_Till").ToString() != "" ? Convert.ToDateTime(Eval("BC_Till").ToString()).ToString("MMM dd, yyyy") : "NA"%>
                                </td>
                                <td>
                                    <%#Eval("Notice").ToString() != "" ? Eval("Notice").ToString() : "NA"%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            There are no Book Closures currently
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
