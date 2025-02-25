<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PortFolioView.aspx.cs"
    Inherits="iFrames.Pages.PortFolioView" %>
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
                <br style="line-height: 15px;"/>
            </td>
        </tr>
        <tr>
            <td>
                <table width="550px" cellpadding="3" cellspacing="3" border="2" style="border-color: Black;
                    border-collapse: collapse;">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="LblScheme" SkinID="lblHeader" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="ListtableRow">
                        <td>
                            <strong>Fund Size as on </strong>
                        </td>
                        <td>
                            <asp:Label ID="LblFsizeAson" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternateRow">
                        <td>
                            <strong>Fund Size ( Rs. in crores)</strong>
                        </td>
                        <td>
                            <asp:Label ID="LblFundSize" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr class="ListtableRow">
                        <td>
                            <strong>Asset Allocation as on</strong>
                        </td>
                        <td>
                            <asp:Label ID="LblAssetAson" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternateRow">
                        <td>
                            <strong>Equity</strong>
                        </td>
                        <td>
                            <asp:Label ID="LblEquity" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr class="ListtableRow">
                        <td>
                            <strong>Debt</strong>
                        </td>
                        <td>
                            <asp:Label ID="LblDebt" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr class="AlternateRow">
                        <td>
                            <strong>Others</strong>
                        </td>
                        <td>
                            <asp:Label ID="LblOthers" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </table>
        <br style="line-height:15px;"/>
        <table width="560px" cellpadding="3" cellspacing="3">
            <tr>
                <td align="center" class="content" colspan="2">
                    <strong>Top 10 Holding as on</strong>
                    <asp:Label ID="LbltoptenAson" SkinID="lblHeader" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                            <asp:TabPanel ID="tabDebt" HeaderText="DEBT" runat="server">
                                <ContentTemplate>                                 
                    <asp:ListView ID="LstToptenDebt" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                        <LayoutTemplate>
                            <table border="2" cellpadding="3" cellspacing="3" width="550px" style="border-collapse: collapse;border-color:Black;">                                
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
                                        Percentage of Net Assets<asp:Label ID="LblNextDate" runat="server" SkinID="labelsHead"></asp:Label>
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
                          <asp:ListView ID="LstToptenEquity" runat="server" ItemPlaceholderID="itemPlaceHolder2"
                        InsertItemPosition="LastItem">
                        <LayoutTemplate>
                            <table border="2" cellpadding="3" cellspacing="3" width="550px" style="border-collapse: collapse; border-color:Black;">
                                
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
                                        Percentage of Net Assets<asp:Label ID="LblNextDate" runat="server"></asp:Label>
                                    </th>
                                </tr>
                                <asp:PlaceHolder ID="itemPlaceHolder2" runat="server" />
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
                            <tr class="content">
                                <td colspan="5">
                                    * No. of shares shown above may have been calculated on the basis of percentage
                                    of net assets and market values taking NSE closing prices and not necessarily declared
                                    by fund house.
                                </td>
                            </tr>
                        </InsertItemTemplate>
                    </asp:ListView>
                    </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="tabOthers" HeaderText="OTHERS" runat="server">
                    <ContentTemplate>
                         <asp:ListView ID="LstToptenOther" runat="server" ItemPlaceholderID="itemPlaceHolder3">
                        <LayoutTemplate>
                            <table border="2" cellpadding="2" width="100%" style="border-collapse: collapse;">                                
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
                                        Percentage of Net Assets<asp:Label ID="LblNextDate" runat="server"></asp:Label>
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
            <tr>
                <td align="center">
                    <a id="CompletePort" runat="server">COMPLETE PORTFOLIO</a>
                </td>
                <td  align="center">
                    <a id="InstrumentAlloc" align="left" runat="server"></a>
                </td>
            </tr>         
        </table>       
    </form>
</body>
</html>
