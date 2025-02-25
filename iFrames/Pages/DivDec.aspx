<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DivDec.aspx.cs" Inherits="iFrames.Pages.DivDec" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <center>
        <form id="form1" runat="server">
        <div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">  
        </asp:ToolkitScriptManager>
            <table cellpadding="2" cellspacing="2" width="450px">
                <tr>
                    <td>
                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                            <asp:TabPanel ID="tabEquity" HeaderText="Equity Funds" runat="server">
                                <ContentTemplate> 
                                    <h4>Latest dividends declared in Equity Funds for the last 1 month </h4>                               
                                    <asp:ListView ID="lstEquity" runat="server" onpagepropertieschanging="lst_PagePropertiesChanging">                                        
                                        <LayoutTemplate>
                                            <table cellpadding="2" runat="server" id="tblThought" style="width: 460px; border-color:Black;border-collapse:collapse;" border="1">
                                                <tr class="ListtableHead">
                                                    <th id="sname" runat="server">Scheme Name</th>
                                                    <th id="sdate" runat="server">Record Date</th>
                                                    <th id="sdiv" runat="server">Dividend (in %)</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstEquity">
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
                                                    <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<--" ShowNextPageButton="False"
                                                        NextPageText="<" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText="-->" ShowPreviousPageButton="False"
                                                        PreviousPageText=">" />
                                                </Fields>
                                            </asp:DataPager>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr  class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td align="left"><a href="fundfactsheet1.aspx?sname=<%#Eval("sch_code")%>&comID=<%=ComID%>"><%#Eval("short_name")%></a></td>
                                                <td align="left"><%#Convert.ToDateTime(Eval("Record_date").ToString()).ToString("MMM dd, yyyy")%></td>
                                                <td align="left"><%#Eval("Divid_pt")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <ajaxToolkit:TabPanel ID="tabDebt" HeaderText="Debt Funds"  runat="server">
                            <ContentTemplate>
                            <h4>Latest dividends declared in Debt Funds for the last 1 week </h4>
                                <asp:ListView ID="lstDebt" runat="server" onpagepropertieschanging="lst_PagePropertiesChanging">                                        
                                        <LayoutTemplate>
                                            <table cellpadding="2" runat="server" id="tblThought" style="width: 460px; border-color:Black;border-collapse:collapse;" border="1">
                                                <tr class="ListtableHead">
                                                    <th id="sname" runat="server">Scheme Name</th>
                                                    <th id="sdate" runat="server">Record Date</th>
                                                    <th id="sdiv" runat="server">Dividend (in %)</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstDebt">
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
                                                    <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<--" ShowNextPageButton="False"
                                                        NextPageText="<" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText="-->" ShowPreviousPageButton="False"
                                                        PreviousPageText=">" />
                                                </Fields>
                                            </asp:DataPager>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr  class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td align="left"><a href="fundfactsheet1.aspx?sname=<%#Eval("sch_code")%>&comID=<%=ComID%>"><%#Eval("short_name")%></a></td>
                                                <td align="left"><%#Convert.ToDateTime(Eval("Record_date").ToString()).ToString("MMM dd, yyyy")%></td>
                                                <td align="left"><%#Eval("Divid_pt")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                            </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabBalance" HeaderText="Balanced Funds"  runat="server">
                            <ContentTemplate>                            
                                <asp:ListView ID="lstBalance" runat="server" onpagepropertieschanging="lst_PagePropertiesChanging">                                        
                                        <LayoutTemplate>
                                            <table cellpadding="2" runat="server" id="tblThought" style="width: 460px; border-color:Black;border-collapse:collapse;" border="1">
                                                <tr class="ListtableHead">
                                                    <th id="sname" runat="server">Scheme Name</th>
                                                    <th id="sdate" runat="server">Record Date</th>
                                                    <th id="sdiv" runat="server">Dividend (in %)</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstBalance">
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
                                                    <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<--" ShowNextPageButton="False"
                                                        NextPageText="<" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText="-->" ShowPreviousPageButton="False"
                                                        PreviousPageText=">" />
                                                </Fields>
                                            </asp:DataPager>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr  class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td align="left"><a href="fundfactsheet1.aspx?sname=<%#Eval("sch_code")%>&comID=<%=ComID%>"><%#Eval("short_name")%></a></td>
                                                <td align="left"><%#Convert.ToDateTime(Eval("Record_date").ToString()).ToString("MMM dd, yyyy")%></td>
                                                <td align="left"><%#Eval("Divid_pt")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                            </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </asp:TabContainer>
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </center>
</body>
</html>
