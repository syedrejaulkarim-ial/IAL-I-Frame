<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundRedeemedRecently.aspx.cs"
    Inherits="iFrames.Pages.FundRedeemedRecently" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        String.prototype.trim = function () {
            a = this.replace(/^\s+/, '');
            return a.replace(/\s+$/, '');
        };

        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="scriptManager" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <table width="550px" border="0" cellpadding="3" cellspacing="3" style="height: 400px;
            float: inherit;">
            <tr>
                <td valign="top">
                    <table style="margin-top: 30px;">
                        <tr class="mainHeader">
                            <td>
                                Funds Redeemed Recently
                                <hr />
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        <tr class="SubHeader">
                            <td>
                                Enter mutual fund house or scheme name to know the schemes redeemed till date.
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        <tr class="content" style="line-height: 45px;">
                            <td align="center">
                                <strong>Enter Scheme Name or Scheme Code</strong>
                                <br />
                                <asp:TextBox id="txtSchemes" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click"  />
                            </td>
                        </tr>
                        <tr>
                            <td align="Center">
                                <asp:RequiredFieldValidator ControlToValidate="txtSchemes" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please provide Scheme Code or Scheme Name" Font-Size="11px" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:PlaceHolder id="PlaceHolder1" runat="server" Visible="false">
                        <ajaxToolkit:TabContainer ID="tabContainerScheme" runat="server">
                            <cc1:TabPanel ID="tabInYear" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblinYear" SkinID="lblHeader" runat="server" />
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:ListView ID="lstInYear" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                        <LayoutTemplate>
                                            <table cellpadding="3" border="1" cellspacing="3" style="border-color:Black; border-collapse:collapse;">
                                                <tr class="ListtableHead">
                                                    <th id="sname" runat="server">
                                                        Scheme Name
                                                    </th>
                                                    <th id="sdate" runat="server">
                                                        Fund Type
                                                    </th>
                                                    <th id="sdiv" runat="server">
                                                        Date
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstInYear">
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
                                                    <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<--" ShowNextPageButton="False"
                                                        NextPageText="<" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText="-->" ShowPreviousPageButton="False"
                                                        PreviousPageText=">" />
                                                </Fields>
                                            </asp:DataPager>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td>
                                                    <%#Eval("short_name").ToString()%>
                                                </td>
                                                <td>
                                                    <%#Eval("type1").ToString() != "" ? Eval("type1").ToString() : "NA"%>
                                                </td>
                                                <td>
                                                    <%#Eval("redmpdate").ToString() != "" ? Convert.ToDateTime(Eval("redmpdate").ToString()).ToString("MMM dd, yyyy") : "NA"%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            Your Scheme or Fund name is not matched.
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tabTillDate" runat="server">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTillDate" runat="server" SkinID="lblHeader" Text="SHEMES REDEEMED TILL DATE" />
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:ListView ID="lstTillDate" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                        <LayoutTemplate>
                                            <table cellpadding="3" border="1" cellspacing="3" style="border-color:Black; border-collapse:collapse;">
                                                <tr class="ListtableHead">
                                                    <th id="sname" runat="server">
                                                        Scheme Name
                                                    </th>
                                                    <th id="sdate" runat="server">
                                                        Fund Type
                                                    </th>
                                                    <th id="sdiv" runat="server">
                                                        Date
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstTillDate">
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
                                                    <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<--" ShowNextPageButton="False"
                                                        NextPageText="<" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText="-->" ShowPreviousPageButton="False"
                                                        PreviousPageText=">" />
                                                </Fields>
                                            </asp:DataPager>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td>
                                                    <%#Eval("short_name").ToString()%>
                                                </td>
                                                <td>
                                                    <%#Eval("type1").ToString() != "" ? Eval("type1").ToString() : "NA"%>
                                                </td>
                                                <td>
                                                    <%#Eval("redmpdate").ToString() != "" ? Convert.ToDateTime(Eval("redmpdate").ToString()).ToString("MMM dd, yyyy") : "NA"%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            Your Scheme or Fund name is not matched.
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
