<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyPortfolio.aspx.cs"
    Inherits="iFrames.Pages.CompanyPortfolio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr class="mainHeader">
            <td>
                PORTFOLIOS
                <hr />
                <br style="line-height: 25px; color: #bd2027;" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="550" cellpadding="3" cellspacing="3">
                <tr>
                    <td colspan="5" align="center">
                        <asp:Label ID="LblPortScheme" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="content">
                    <td colspan="2" align="center">
                        Portfolio as on
                        <asp:Label ID="LblComportAson" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                            <asp:TabPanel ID="tabDebt" HeaderText="DEBT" runat="server">
                                <ContentTemplate>
                                    <asp:ListView ID="LstComPortDebt" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                                        <LayoutTemplate>
                                            <table border="1" cellpadding="2" width="550px" style="border-collapse: collapse;">                                               
                                                <tr class="ListtableHead">
                                                    <th>
                                                        Company Name
                                                    </th>
                                                    <th>
                                                        Instrument
                                                    </th>
                                                    <th>
                                                        Rating
                                                    </th>
                                                    <th>
                                                        Market Value (Rs. in crores)
                                                    </th>
                                                    <th>
                                                        % Net Assets<asp:Label ID="LblNextDate" SkinID="lblHeader" runat="server"></asp:Label>
                                                    </th>
                                                </tr>
                                                <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td width="35%" align="center">
                                                    <%#Eval("c_name").ToString()%>
                                                </td>
                                                <td width="15%" align="center">
                                                    <%#Eval("Instrument").ToString()!=""?Eval("Instrument").ToString():"NA"%>
                                                </td>
                                                <td width="10%" align="center">
                                                    <%#Eval("Rating").ToString()!=""?Eval("Rating").ToString():"NA"%>
                                                </td>
                                                <td width="20%" align="center">
                                                    <%#Eval("Mkt_Value").ToString() != "" ? Math.Round(double.Parse(Eval("Mkt_Value").ToString()), 2, MidpointRounding.AwayFromZero).ToString() : "NA"%>
                                                </td>
                                                <td width="20%" align="center">
                                                    <%#Eval("corpus_per").ToString() != "" ? Math.Round(double.Parse(Eval("corpus_per").ToString()), 2, MidpointRounding.AwayFromZero).ToString() : "NA"%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="tabEquity" HeaderText="EQUITY" runat="server">
                                <ContentTemplate>
                                    <asp:ListView ID="LstComPortEquity" runat="server" ItemPlaceholderID="itemPlaceHolder2"
                                        InsertItemPosition="LastItem" OnPagePropertiesChanging="LstComPortEquity_PagePropertiesChanging">
                                        <LayoutTemplate>
                                            <table border="1" cellpadding="3" cellspacing="3" width="550px" style="border-collapse: collapse;
                                                border-color: Black;">                                                
                                                <tr class="ListtableHead">
                                                    <th>
                                                        Company Name
                                                    </th>
                                                    <th>
                                                        Instrument
                                                    </th>
                                                    <th>
                                                        No. of Shares
                                                    </th>
                                                    <th>
                                                        Market Value (Rs. in crores)
                                                    </th>
                                                    <th>
                                                        % Net Assets<asp:Label ID="LblNextDate" SkinID="lblHeader" runat="server"></asp:Label>
                                                    </th>
                                                </tr>
                                                <asp:PlaceHolder ID="itemPlaceHolder2" runat="server" />
                                            </table>
                                            <asp:DataPager runat="server" ID="EquityDataPager" PageSize="10" PagedControlID="LstComPortEquity">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <span class="labelsHead">Page
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
                                                <td width="35%"  align="center">
                                                    <%#Eval("c_name").ToString()%>
                                                </td>
                                                <td width="15%" align="center">
                                                    <%#Eval("Instrument").ToString()!=""?Eval("Instrument").ToString():"NA"%>
                                                </td>
                                                <td width="15%" align="center">
                                                    <%#Eval("No_of_shares").ToString()!=""?Math.Round(double.Parse(Eval("No_of_shares").ToString()), 2, MidpointRounding.AwayFromZero).ToString():"NA"%>
                                                </td>
                                                <td width="15%" align="center">
                                                    <%#Eval("Mkt_Value").ToString() != "" ? Math.Round(double.Parse(Eval("Mkt_Value").ToString()), 2, MidpointRounding.AwayFromZero).ToString() : "NA"%>
                                                </td>
                                                <td width="20%" align="center">
                                                    <%#Eval("corpus_per").ToString()!=""?Math.Round(double.Parse(Eval("corpus_per").ToString()), 2, MidpointRounding.AwayFromZero).ToString():"NA"%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <tr>
                                                <td colspan="5" class="content">
                                                    * No. of shares shown above may have been calculated on the basis of percentage
                                                    of net assets and market values taking NSE closing prices and not necessarily declared
                                                    by fund house.
                                                </td>
                                            </tr>
                                        </InsertItemTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="tabOther" HeaderText="OTHER" runat="server">
                                <ContentTemplate>
                                    <asp:ListView ID="LstComPortOther" runat="server" ItemPlaceholderID="itemPlaceHolder3">
                                        <LayoutTemplate>
                                            <table border="1" cellpadding="3" cellspacing="3" width="550px" style="border-collapse: collapse;
                                                border-color: Black;">                                                
                                                <tr class="ListtableHead">
                                                    <th>
                                                        Company Name
                                                    </th>
                                                    <th>
                                                        Instrument
                                                    </th>
                                                    <th>
                                                        Market Value (Rs. in crores)
                                                    </th>
                                                    <th>
                                                        % Net Assets<asp:Label ID="LblNextDate" runat="server" SkinID="lblHeader"></asp:Label>
                                                    </th>
                                                </tr>
                                                <asp:PlaceHolder ID="itemPlaceHolder3" runat="server" />
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td width="35%" align="center">
                                                    <%#Eval("c_name").ToString()%>
                                                </td>
                                                <td width="15%" align="center">
                                                    <%#Eval("Instrument").ToString()!=""?Eval("Instrument").ToString():"NA"%>
                                                </td>
                                                <td width="25%" align="center">
                                                    <%#Eval("Mkt_Value").ToString() != "" ? Math.Round(double.Parse(Eval("Mkt_Value").ToString()), 2, MidpointRounding.AwayFromZero).ToString() : "NA"%>
                                                </td>
                                                <td width="25%" align="center">
                                                    <%#Eval("corpus_per").ToString() != "" ? Math.Round(double.Parse(Eval("corpus_per").ToString()), 2, MidpointRounding.AwayFromZero).ToString() : "NA"%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </td>
                </tr>               
            </table>
            <table width="550px">
                <tr>
                    <td width="50%" align="right">
                        <a id="CompletePort" runat="server">TOP 10 HOLDING</a>
                    </td>
                    <td width="50%">
                        &nbsp;&nbsp;&nbsp;&nbsp;<a id="InstrumentAlloc" align="left" runat="server"></a>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
