<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllSchemes.aspx.cs" Inherits="iFrames.Pages.AllSchemes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <table width="550px" cellpadding="3" cellspacing="3">
            <tr class="mainHeader">
                <td colspan="3">
                    ALL SCHEMES
                    <hr />
                    <br style="line-height: 15px;" />
                </td>
            </tr>
            <tr class="SubHeader" align="center">
                <td>
                    Sales during the month of &nbsp;&nbsp;<%=Convert.ToDateTime(ReqDate.ToString()).ToString("MMM, yyyy")%>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="550px">
                        <tr>
                            <td style="font-family: Verdana; font-size: 9.5px; text-align: right;">
                                Amount in Rs. Crores
                            </td>
                        </tr>
                        <tr>
                            <asp:ListView ID="lstAllSchemes" runat="server" >
                                <LayoutTemplate>
                                    <table cellpadding="3" width="550px" cellspacing="3" border="1" style="border-color: black;
                                        border-collapse: collapse;">
                                        <tr class="ListtableHead">
                                            <th rowspan="4">
                                                Nature
                                            </th>
                                        </tr>
                                        <tr class="ListtableHead">
                                            <th colspan="7">
                                                Structure
                                            </th>
                                        </tr>
                                        <tr class="ListtableHead">
                                            <th colspan="2">
                                                Open Ended
                                            </th>
                                            <th colspan="2">
                                                Close Ended
                                            </th>
                                            <th colspan="2">
                                                Total
                                            </th>
                                        </tr>
                                        <tr class="ListtableHead">
                                            <th>
                                                No. of Schemes
                                            </th>
                                            <th>
                                                Amount
                                            </th>
                                            <th>
                                                No. of Schemes
                                            </th>
                                            <th>
                                                Amount
                                            </th>
                                            <th>
                                                No. of Schemes
                                            </th>
                                            <th>
                                                Amount
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                    <%--<asp:DataPager runat="server" ID="dp" PageSize="10" PagedControlID="lstAllSchemes">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <span class="labelsHead">Page
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                                        of
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                                        (
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                                        records)</span>
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowNextPageButton="false"
                                                ShowPreviousPageButton="false" />
                                            <asp:NumericPagerField PreviousPageText="&lt; Prev 10" NextPageText="Next 10 &gt;"
                                                ButtonCount="10" />
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowNextPageButton="false"
                                                ShowPreviousPageButton="false" />
                                        </Fields>
                                    </asp:DataPager>--%>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                        <td>
                                            <%#Eval("Nature").ToString() == "Total" ? "<strong>" + Eval("Nature").ToString() + "</strong>" : Eval("Nature").ToString()%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToInt32(Eval("All_sch_open")) != 0 ? Eval("All_sch_open") : "--"%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToDouble(Eval("All_sch_amt_open")) != 0 ? Eval("All_sch_amt_open") : "--"%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToInt32(Eval("All_sch_close")) != 0 ? Eval("All_sch_close").ToString() : "--"%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToDouble(Eval("All_sch_amt_close")) != 0 ? Eval("All_sch_amt_close").ToString() : "--"%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToInt32(Eval("TotSch")) != 0 ? Eval("TotSch").ToString() : "--"%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToDouble(Eval("TotAmt")) != 0 ? Eval("TotAmt").ToString() : "--"%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    No data Found
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
