<%@ Page Title="" Language="C#" MasterPageFile="~/WiseInvest/WiseMain.Master" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="NfoUpdate.aspx.cs" Inherits="iFrames.WiseInvest.NfoUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBoby" runat="server">
    <script type="text/javascript">
        document.getElementById('lNfoUpdate').setAttribute('class', 'selected');
    </script>
    <form id="form1" runat="server">
    <%--<table>
        <tr>
            <td>
                <strong>Nature</strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server">
                </asp:DropDownList>
            </td>

        </tr>
        <tr>
        <td colspan ="2">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                onclick="btnSubmit_Click" />
        </td>
        </tr>
        <tr>
        <td colspan ="2">
        
        </td>
        </tr>
    </table>--%>
    <table border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
        <tr>
            <td>
                <table width="710" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="top_icon">
                                        <img src="img/nfo_up.png" width="31" height="27" />
                                    </td>
                                    <td class="top_title">
                                        NFO Update
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
                                        <table width="80%" border="0" align="left" cellpadding="0" cellspacing="0">
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
                                                    Nature
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="top_input2">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="btnSubmit" runat="server" Text=">> Submit" OnClick="btnSubmit_Click"
                                                        class="top_button" />
                                                </td>
                                            </tr>
                                            <tr class="top_td">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
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
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListView ID="listVwNFODetail" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                            <layouttemplate>
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="top_table">
                                                    <tr class="top_tableheader">
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
                                                                Text="NFO Close Date" />
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </layouttemplate>
                                            <itemtemplate>
                                                <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "top_tablerow" : "top_tablerow" %>'>
                                                    <td class="top_tablerow">
                                                        <asp:Label runat="server" ID="lblSchName" Text='<%#Eval("Sch_Short_Name")%>' />
                                                    </td>
                                                    <td class="top_tablerow">
                                                        <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                                    </td>
                                                    <td class="top_tablerow">
                                                        <asp:Label runat="server" ID="lblDate" Text='<%#Convert.ToDateTime(Eval("Nfo_Close_Date").ToString()).ToString("MMM dd, yyyy")%>' />
                                                    </td>
                                                </tr>
                                            </itemtemplate>
                                            <emptydatatemplate>
                                                Data not Found
                                            </emptydatatemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <div style="padding-top:5px;"></div>
                                        <asp:DataPager ID="dpNFO" runat="server" PageSize="10" PagedControlID="listVwNFODetail">
                                            <fields>
                            <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                            <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                            <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <span style="padding-left: 40px; text-align:right">Page
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
                        </fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <%-- <tr>
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
            </td>
        </tr>
    </table>
    </form>
</asp:Content>
