<%@ Page Title="" Language="C#" MasterPageFile="~/WiseInvest/WiseMain.Master" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="LatestNav.aspx.cs" Inherits="iFrames.WiseInvest.LatestNav" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBoby" runat="server">
    <script type="text/javascript">
        document.getElementById('lLatest').setAttribute('class', 'selected');
    </script>
    <form id="frm" runat="server">
    <table width="710" border="0" align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="top_icon">
                            <img src="img/Latest_NAV.png" width="25" height="31" />
                        </td>
                        <td class="top_title">
                            Latest NAV
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
                            <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_inputa">
                                        Fund House
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                            CssClass="top_input3">
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
                                <tr valign="top">
                                    <td>
                                        <table border="0" align="right" cellpadding="0" cellspacing="0">
                                            <tr valign="top">
                                                <td class="top_inputd">
                                                    Scheme Name
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lbSchemeName" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="lbSchemeName_SelectedIndexChanged"
                                            CssClass="top_inputddl"></asp:ListBox>
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
                                <tr class="top_td">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="top">
                                        <table width="29%" border="0" align="right" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnGo" runat="server" Text=">> Submit" OnClick="btnGo_Click" CssClass="top_button" />
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<table width="70%" border="0" align="left" cellpadding="0" cellspacing="0" class="top_table">
                                <tr class="top_tableheader">
                                    <td class="top_tableheader1">
                                        Scheme Name
                                    </td>
                                    <td class="top_tableheader1">
                                        Date
                                    </td>
                                    <td class="top_tableheader1">
                                        Nav (<img src="img/rupee.png" />)
                                    </td>
                                    <td class="top_tableheader">
                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="3" class="top_tableheader2">
                                                    <div align="center">
                                                        Performance % as on Mar 19, 2013</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    91 Days
                                                </td>
                                                <td>
                                                    182 Days
                                                </td>
                                                <td>
                                                    1 Year
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <tr>
                                        <td class="top_tablerow">
                                            HDFC Annual Interval Fund - Series I - Plan A
                                        </td>
                                        <td class="top_tablerow">
                                            Mar 19, 2013
                                        </td>
                                        <td class="top_tablerow">
                                            8.0000
                                        </td>
                                        <td class="top_tablerow">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        NA
                                                    </td>
                                                    <td>
                                                        NA
                                                    </td>
                                                    <td>
                                                        NA
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                            </table>--%>
                            <asp:GridView Width="100%" runat="server" CssClass="top_table" ID="gvNavDetail" AutoGenerateColumns="false">
                                <Columns>
                                    <%-- <asp:TemplateField HeaderText="SchemeId" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%#
                (Eval("Scheme_Id") == DBNull.Value) ? "--" : Eval("Scheme_Id").ToString() 
                                            %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%#              
                (Eval("Sch_Short_Name") == DBNull.Value) ? "--" : Eval("Sch_Short_Name").ToString()
                                            %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nav Date" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%# Convert.ToDateTime(Eval("Nav_Date")).ToString("dd MMM yyyy")%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nav" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%# Eval("Nav").ToString()  %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last 3 Month" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%# Eval("Per_91_Days").ToString()%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last 6 Month" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%# Eval("Per_182_Days").ToString()%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last 1 year" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <%# Eval("Per_1_Year").ToString()%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="top_tablerow" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Data Not Found
                                </EmptyDataTemplate>
                            </asp:GridView>
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
    </form>
</asp:Content>
