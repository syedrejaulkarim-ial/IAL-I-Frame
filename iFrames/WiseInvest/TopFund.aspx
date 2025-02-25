<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.WiseInvest.TopFund"
    MasterPageFile="~/WiseInvest/WiseMain.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBoby" runat="server">
    <script type="text/javascript">
        document.getElementById('lTopFund').setAttribute('class', 'selected');     

       
    </script>
    <body>
        <form id="topform1" runat="server">
        <div>
            <%--<table width="550px" cellpadding="3" cellspacing="3">
                <tr>
                    <td>
                        <strong>Rank</strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRank" runat="server">
                            <asp:ListItem Text="All" Value="1000" />
                            <asp:ListItem Text="Top 5" Value="5" />
                            <asp:ListItem Text="Top 10" Value="10" />
                            <asp:ListItem Text="Top 15" Value="15" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Type</strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Category</strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Period</strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriod" runat="server">
                            <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" Selected="True" />
                            <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                            <asp:ListItem Value="Per_91_Days" Text="Last 3 Month" />
                            <asp:ListItem Value="Per_182_Days" Text="Last 6 Month" />
                            <asp:ListItem Value="Per_1_Year" Text="Last 12 Month" />
                            <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                            <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />                          
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="9">
                       
                    </td>
                </tr>
            </table>--%>
            <table width="710" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="top_icon">
                                    <img src="img/top_fund.png" width="29" height="30" />
                                </td>
                                <td class="top_title">
                                    Top Funds
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="2" class="top_line">
                                    <table width="55%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="top_input">
                                                Rank
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRank" runat="server" CssClass="top_input1">
                                                    <asp:ListItem Text="All" Value="1000" />
                                                    <asp:ListItem Text="Top 5" Value="5" />
                                                    <asp:ListItem Text="Top 10" Value="10" />
                                                    <asp:ListItem Text="Top 15" Value="15" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="top_td">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="top_input">
                                                Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="top_input1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="top_td">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="top_input">
                                                Nature
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="top_input1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="top_td">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="top_input">
                                                Period
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="top_input1">
                                                    <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" Selected="True" />
                                                    <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                                                    <asp:ListItem Value="Per_91_Days" Text="Last 3 Month" />
                                                    <asp:ListItem Value="Per_182_Days" Text="Last 6 Month" />
                                                    <asp:ListItem Value="Per_1_Year" Text="Last 12 Month" />
                                                    <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                                                    <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />
                                                    <%--   <asp:ListItem Value="Since_Inception" Text="Since Inception" />--%>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="top_td">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="top_td">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="top">
                                                <table width="90%" border="0" align="right" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnGo" runat="server" Text=">> Submit" OnClick="btnGo_Click" class="top_button" />&nbsp;
                                                            <%--<input id="btnReset"  type="button" class="top_button" value="<< Reset" onclick="Javascript:document.forms[0].reset();" />--%>
                                                            <asp:Button ID="btnReset" runat="server" Text=">> Reset" OnClick="btnRestClick" class="top_button" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="top_text">
                                    <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                        <LayoutTemplate>
                                            <table width="100%" border="1" cellpadding="3" cellspacing="3" style="border-color: Black;
                                                border-collapse: collapse;">
                                                <tr class="top_tableheader">
                                                    <th align="left">
                                                        <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank"
                                                            Text="Rank" />
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName"
                                                            Text="Scheme Name" />
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lnkNature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                            Text="Nature" />
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lnkDate" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Date"
                                                            Text="Date" />
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV"
                                                            Text="NAV" />
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="<%=ddlPeriod.SelectedValue%>"><%=ddlPeriod.SelectedItem.Text%></asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lnkInception" runat="server" SkinID="lblHeader" CommandName="Sort"
                                                            CommandArgument="Inception" Text="Since Inception" />
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                            <div style="padding-top: 5px;">
                                            </div>
                                            <asp:DataPager ID="dpTopFund" runat="server" PageSize="10" PagedControlID="lstResult">
                                                <Fields>
                                                    <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                                                    <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                                    <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <span style="padding-left: 40px; text-align: right">Page
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
                                                </Fields>
                                            </asp:DataPager>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "top_tablerow" : "top_tablerow" %>'>
                                                <td>
                                                    <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("sch_name")%>' />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Calculation_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                                </td>
                                                <td>
                                                    <%# Convert.ToDouble(Eval(ddlPeriod.SelectedValue)).ToString("n3") %>
                                                </td>
                                                <td>
                                                    <%# Convert.ToDouble(Eval("Since_Inception")).ToString("n3")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            Data not Found
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <%--  <tr>
            <td class="disclaimerh">
                Disclaimer
            </td>
        </tr>
        <tr>
            <td class="disclaimer">
                All Mutual Funds and securities investments are subject to market risks and there
                can be no assurance that the scheme’s object will be achieved and the NAV of the
                schemes can go up or down depending upon the factors and forces affecting the securities
                market. Past performance of the schemes do not indicate the future performances.
                The NAV of the schemes may be affected by changes in Interest Rate, trading volumes,
                settlement periods, transfer procedures and performances of individual securities.
                The NAV will be exposed to price/ Interest rate Risk and Credit Risk.
            </td>
        </tr>--%>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </body>
</asp:Content>
