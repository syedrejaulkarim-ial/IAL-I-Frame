<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="UploadExcel.aspx.cs" Inherits="iFrames.Chart.UploadExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="css/Chart.css" />
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table width="50%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="right">
                    <asp:LinkButton ID="lnlLogout" runat="server" Visible="false" Font-Bold="true" ForeColor="#067DCD" OnClick="lnlLogout_Click">logout</asp:LinkButton>
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:Panel ID="pnlLogin" runat="server" GroupingText="Log In" Width="485" Font-Bold="true" ForeColor="#067DCD">
            <table width="50%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblMsgError" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                            Font-Size="Small"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        User ID:
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserid" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ControlToValidate="txtUserid" Display="Dynamic" ValidationGroup="login"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                            ControlToValidate="txtPassword" Display="Dynamic" ValidationGroup="login"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnLogin" runat="server" Text="Log In" OnClick="btnLogin_Click" ValidationGroup="login"
                            CssClass="button" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlUpload" runat="server" GroupingText="Upload Excel" Width="450"
            Font-Bold="False" ForeColor="#067DCD">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <b>Upload:</b>
                    </td>
                    <td style="width: 20px;">
                        <asp:FileUpload ID="FUExcell" runat="server" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                            CssClass="button" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
        </asp:Panel>
    </center>
    </form>
</body>
</html>
