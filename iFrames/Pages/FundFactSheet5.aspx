<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundFactSheet5.aspx.cs"
    Inherits="iFrames.Pages.FundFactSheet5" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="540px" cellpadding="3" cellspacing="3">
            <tr>
           <td align="justify" colspan="2" style="height: 55px;">
                    <span class="mainHeader">NEWS - </span><span class="SubHeader"><%=shortName%></SPAN>
                </td>
            </tr>
               
                <tr>
                    <td colspan="5">
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td valign="middle">
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
                        </table>
                    </td>
                </tr>
                <tr class="SubHeader">
                    <td colspan="5">
                        <strong>NEWS</strong>
                        <br style="line-height: 5px;" />
                        <hr />
                    </td>
                </tr>
                <tr style="line-height: 45px;">
                    <td>
                        <asp:Label ID="lbl1" runat="server" SkinID="lblHeader" Text="Choose Year:" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DrpYear" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="Choose Month:" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DrpMon" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="BtnGo" runat="server" Text="GO" OnClick="BtnGo_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="center" style="font-family: Verdana; color: black; font-size: 11px;
                        font-weight: normal;">
                        <strong>Following are the news articles for the last six months.</strong>
                        <br style="line-height: 5px;" />
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="font-size: small">
                        <asp:ListView ID="LstFundNews" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                            <LayoutTemplate>
                                <table border="1" style="border-color: Black; border-collapse: collapse;" cellpadding="1"
                                    width="100%">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                    <td width="100%" style="font-size: small">
                                        <asp:Image ID="BulletImage" runat="server" ImageUrl="~/Images/bulletwgif.gif" />
                                        <a href='NewsViwe.aspx?news_headline=<%#HttpUtility.UrlPathEncode(Eval("news_headline").ToString())%>&comID=<%=this.PropCompID %>'>
                                            <%#Eval("news_headline")%></a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                No Data Found</EmptyDataTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="center">
                        <a href="TotNews.aspx?comID=<%=this.PropCompID %>">ALL NEWS</a>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
