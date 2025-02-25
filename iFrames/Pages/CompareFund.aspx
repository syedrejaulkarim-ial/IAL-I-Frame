<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompareFund.aspx.cs" Inherits="iFrames.Pages.CompareFund" %>

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
            <table width="550px" cellpadding="3" cellspacing="3">
                <tr>
                    <td align="center" class="content" colspan="3">
                        <strong>1.Choose Category </strong>&nbsp;&nbsp;
                        <asp:DropDownList ID="DrpCat" runat="server" AutoPostBack="true" Width="400px" OnSelectedIndexChanged="DrpCat_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td >
                        <span class="content"><strong>2.Choose Fund From the Fund List </strong></span>
                        <br style="line-height: 25px;" />
                        <asp:ListBox ID="LstBxFund" runat="server" Width="350" Height="150" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="BtnForward" runat="server" Text=">>" OnClick="BtnForward_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td width="45%">
                                    <asp:Button ID="BtnBackward" runat="server" Text="<<" OnClick="BtnBackward_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <span class="content"><strong>3.Choose compare fund for Result</strong></span><br
                            style="line-height: 25px;" />
                        <asp:ListBox ID="LstBxCompFund" runat="server" Width="350" Height="150" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="left">
                        <span class="content"><strong>4:Choose Index</strong></span><br style="line-height: 25px;" />
                        <asp:ListBox ID="LstBxIndex" runat="server" Width="350" Height="150" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="left">
                        <asp:Button ID="BtnCompare" runat="server" Text="COMPARE" OnClick="BtnCompare_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
