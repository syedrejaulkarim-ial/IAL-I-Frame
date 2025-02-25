<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundFactSheet.aspx.cs" Inherits="iFrames.Pages.FundFactSheet" %>

<%@ Register Src="~/UserControl/DropdownCustomControl.ascx" TagName="Control" TagPrefix="DCC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="425px" cellpadding="2" cellspacing="2">
            <tr>
                <td align="center">
                    <DCC:Control ID="UControl" runat="Server" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
