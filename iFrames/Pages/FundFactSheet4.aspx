<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundFactSheet4.aspx.cs"
    Inherits="iFrames.Pages.FundFactSheet4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="540px" cellpadding="2" cellspacing="2" border="0">
           <tr>
           <td align="justify" colspan="2" style="height: 55px;">
                    <span class="mainHeader">PROTFOLIO - </span><SPAN class="SubHeader"><%=shortName%></SPAN>
                </td>
            </tr>
            
            <tr>
                <td valign="middle" colspan="2">
                    <div id="menu">
                        <ul>
                            <li><a href='FundFactSheet1.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Fund Facts</a></li>
                            <li><a href='FundFactSheet2.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>NAV</a></li>
                            <li><a href='FundFactSheet3.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Risk Return</a></li>
                            <li><a href='FundFactSheet4.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Portfolio</a></li>
                            <li><a href='FundFactSheet5.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>News</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr class="mainHeader">
                <td colspan="2" class="SubHeader" style="height: 55px;">
                    <strong>Portfolio</strong>
                    <br style="line-height: 25px;" />
                    <hr />
                </td>
            </tr>
            <tr>
                <td width="70%">
                    <span class="SubHeader">Portfolio Attributes</span>
                    <br style="line-height: 25px;" />
                    <table width="100%" cellpadding="2" cellspacing="2" border="1" style="border-color: Black;
                        border-width: thin; border-collapse: collapse;">
                        <tr class="ListtableRow">
                            <td>
                                <strong>P/E</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblPE" runat="server" />
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td>
                                <strong>P/B</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblPB" runat="server" />
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td>
                                <strong>Dividend Yield</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblDivYield" runat="server" />
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td>
                                <strong>Market Cap (Rs. in crores)</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblMrkCap" runat="server" />
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td>
                                <strong>Large</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblLarge" runat="server" />
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td>
                                <strong>Mid</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblMid" runat="server" />
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td>
                                <strong>Small</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblSmall" runat="server" />
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td>
                                <strong>Top 5 Holding (%)</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblTop5" runat="server" />
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td>
                                <strong>No. of Stocks</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblStocks" runat="server" />
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td>
                                <strong>Expense Ratio (%)</strong>
                            </td>
                            <td>
                                <asp:Label ID="lblExpRatio" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table border="1" cellpadding="4" cellspacing="4" width="550" style="border-color: Black;
                        border-collapse: collapse;">
                        <tr class="ListtableHead" style="height: 35px;">
                            <td>
                                <strong>Whats In</strong>
                            </td>
                            <td>
                                <strong>Whats Out</strong>
                            </td>
                        </tr>
                        <tr style="height: 45px;">
                            <td align="justify">
                                <asp:Label ID="lblIn" runat="server" />
                                <asp:GridView ID="grdIn" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="c_name" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td align="justify">
                                <asp:Label ID="lblOut" runat="server" />
                                <asp:GridView ID="grdOut" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="c_name" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblTop10" runat="server" />
                    <!--List View -->
                    <asp:ListView ID="lstTop10Holding" runat="server">
                        <LayoutTemplate>
                            <table border="1" cellpadding="2" cellspacing="3" style="border-color: Black; border-collapse: collapse;"
                                width="550px">
                                <tr class="ListtableHead">
                                    <th align="left">
                                        Stock
                                    </th>
                                    <th align="left">
                                        Sector
                                    </th>
                                    <th align="left">
                                        P/E
                                    </th>
                                    <th align="left">
                                        Percentage of Net Assets
                                    </th>
                                    <th align="left">
                                        Qty
                                    </th>
                                    <th align="left">
                                        Value
                                    </th>
                                    <th align="left">
                                        Percentage of Change with Last Month
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                <td>
                                    <asp:Label runat="server" ID="lblType"><%#Eval("c_name")%></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label5"><%#Eval("Sect_name")%></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDate" Text='<%#Eval("PE")%>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblNav" Text='<%#Eval("corpus_per")!="NA"?Math.Round(Convert.ToDecimal(Eval("corpus_per")),2).ToString():"NA"%>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label2" Text='<%#Convert.ToDouble(Eval("No_of_shares"))>0?Math.Round(Convert.ToDecimal(Eval("No_of_shares")),2).ToString():"NA"%>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label3" Text='<%#Eval("Mkt_Value").ToString()=="0"?"NA":Eval("Mkt_Value")%>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("PCM")%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                    <br style="line-height: 25px;" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <span class="SubHeader">Sector Allocation (%)</span>
                    <br style="line-height: 25px;" />
                    <asp:Label ID="lblSecAlot" runat="server" />
                    <asp:GridView ID="grdSecAlot" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                        ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("Sect_name")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("corpus_per")==null?"NA":Math.Round(Convert.ToDecimal(Eval("corpus_per")),2).ToString()%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
                <td valign="top">
                    <span class="SubHeader">Asset Allocation (%)</span>
                    <br style="line-height: 25px;" />
                    <asp:Label ID="lblAssetAlot" runat="server" />
                    <asp:GridView ID="grdAssetAlot" runat="server" AutoGenerateColumns="false" AllowPaging="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Equity">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("Equity")==null?"NA":Math.Round(Convert.ToDecimal(Eval("Equity")),2).ToString()%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Debt">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("Debt")==null?"NA":Math.Round(Convert.ToDecimal(Eval("Debt")),2).ToString()%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cash & Equivalent">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("Cas_MonM")==null?"NA":Math.Round(Convert.ToDecimal(Eval("Cas_MonM")),2).ToString()%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
