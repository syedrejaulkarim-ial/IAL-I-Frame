<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropdownCustomControl.ascx.cs"
    Inherits="iFrames.Pages.DropdownCustomControl" %>
<table width="100%">
    <tr>
        <td align="right">
            <asp:Label ID="LblFund" SkinID="lblHeader" runat="server" Text="Label"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="drpDownFund" runat="server" Width="300"
             OnSelectedIndexChanged="drpDownFund_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2"></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="LblScheme" SkinID="lblHeader" runat="server" Text="Label"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="drpDownScheme" runat="server" Width="300">
            </asp:DropDownList>
        </td>
    </tr>
</table>
