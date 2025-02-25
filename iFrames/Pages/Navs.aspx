<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Navs.aspx.cs" Inherits="iFrames.Pages.Navs" %>

<%@ Register TagPrefix="drp" TagName="CustomDrop" Src="~/UserControl/DropdownCustomControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- <link href="../App_Themes/ICRATheme/style.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" language="javascript">
        function tdShow() {

            document.getElementById("tddaymonyr").style.visibility = "visible";
        }
        function tblShowLatNav() {
            //alert("ok");
            document.getElementById("tblsch1").style.visibility = 'visible';
            document.getElementById("tblsch2").style.visibility = "visible";
        }
        function tblHideLatNav() {
            document.getElementById("tblsch1").style.visibility = 'hidden';
            document.getElementById("tblsch2").style.visibility = "hidden";
        }
        function tblShowHistNav() {
            //alert("ok");
            document.getElementById("tblHistNav").style.visibility = "visible";

        }
        function tblHideHistNav() {

            document.getElementById("tblHistNav").style.visibility = "hidden";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
        <ProgressTemplate>
            <img src="../Images/ajax-loader.gif" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="550px" cellpadding="3" cellspacing="0" border="0">
                <tr class="content">
                    <td align="left">
                        <drp:CustomDrop ID="DrpCustom" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table>
                            <tr>
                                <td align="left">
                                    <table width="90%" border="0" style="margin-left: 57px; margin-top: -5px;">
                                        <tr class="content">
                                            <td>
                                                <strong>NAV TYPE</strong>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="drpnavType" AutoPostBack="true" OnSelectedIndexChanged="drpnavType_SelectedIndexChanged"
                                                    Width="300">
                                                    <asp:ListItem>---</asp:ListItem>
                                                    <asp:ListItem Value="0">Latest Nav</asp:ListItem>
                                                    <asp:ListItem Value="1">Historical Nav</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="content">
                                            <td colspan="2">
                                                <table id="tddaymonyr" style="visibility: hidden; margin-left: 35px;">
                                                    <tr>
                                                        <td>
                                                            <strong>Day</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpDay" runat="server" Width="50px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <strong>Month</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpMon" runat="server" Width="50px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <strong>Year</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpYr" runat="server" Width="72px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="right">
                                                <asp:Button ID="BtnGo" runat="server" Style="margin-right: 20px;" Text="GO" OnClick="BtnGo_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table id="tblsch1" width="550px" border="1" style="border-collapse: collapse; border-color: black;
                            visibility: hidden">
                            <tr class="ListtableHead">
                                <td width="10%" rowspan="2" align="center" valign="middle">
                                    <strong class="bottomblue">Scheme Code </strong>
                                </td>
                                <td width="38%" rowspan="2" align="center" valign="middle">
                                    <strong>Scheme Name </strong>
                                </td>
                                <td width="11%" rowspan="2" align="center" valign="middle">
                                    Date
                                </td>
                                <td width="10%" rowspan="2" align="center" valign="middle">
                                    NAV (Rs.)
                                </td>
                                <td align="center" valign="top" width="31%">
                                    <table>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="LblperAsondt" SkinID="lblHeader" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="33%">
                                                91 Days
                                            </td>
                                            <td width="33%">
                                                182 Days
                                            </td>
                                            <td width="33%">
                                                1 Year
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="550px"  id="tblsch2" border="1" style="border-collapse: collapse; border-color: black;
                            visibility: hidden">
                            <tr class="ListtableRow">
                                <td width="10%" align="center">
                                    <asp:Label ID="LblSchCode" runat="server"></asp:Label>
                                </td>
                                <td width="38%" align="center">
                                    <a id="AncScode" runat="server">
                                        <asp:Label ID="lblSchName" runat="server"></asp:Label></a>
                                </td>
                                <td width="11%" align="center">
                                    <asp:Label ID="LblNavDt" runat="server"></asp:Label>
                                </td>
                                <td width="10%" align="center">
                                    <asp:Label ID="LblNav" runat="server"></asp:Label>
                                </td>
                                <td width="10%" align="center">
                                    <asp:Label ID="Lbl91" runat="server"></asp:Label>
                                </td>
                                <td width="11%" align="center">
                                    <asp:Label ID="Lbl182" runat="server"></asp:Label>
                                </td>
                                <td width="10%" align="center">
                                    <asp:Label ID="Lbl1yr" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="550px" align="center" style="visibility: hidden" id="tblHistNav">
                            <tr class="content">
                                <td align="center">
                                    <strong>Scheme Name <a id="AncScheme" runat="server">
                                        <asp:Label ID="lblScheme" runat="server"></asp:Label></a></strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView ID="lstVwHistNav" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                                        <LayoutTemplate>
                                            <table border="1" cellpadding="1" width="550px" style="border-color:Black;border-collapse:collapse;">
                                                <tr class="ListtableHead">
                                                    <th align="left">
                                                        <asp:Label ID="lbl1" SkinID="lblHeader" runat="server">Date</asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lbl2" SkinID="lblHeader" runat="server">Nav</asp:Label>
                                                    </th>
                                                </tr>
                                                <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                <td>
                                                    <asp:Label runat="server" ID="lblDate" Text='<%#Eval("Date").ToString() == ""  ? "NA":Convert.ToDateTime(Eval("Date").ToString()).ToString("MMM dd,yyyy") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav_Rs")%>'></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            No data Found
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
