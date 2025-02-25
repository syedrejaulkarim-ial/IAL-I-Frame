<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mobilisation.aspx.cs" Inherits="iFrames.Pages.Mobilisation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="550px" cellpadding="3" cellspacing="3" style="height: 400px;">
            <tr class="mainHeader">
                <td colspan="3">
                    MF MOBILISATION
                    <hr />
                    <br style="line-height: 15px;" />
                </td>
            </tr>
            <tr class="SubHeader">
                <td>
                    Mutual Fund Mobilisation Figures as released by AMFI.
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <ul class="content" style="list-style: none;">
                        <li>
                            <img src="../Images/bullet.gif" width="10" height="9" />
                            <strong>Sales during the month of</strong>&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlDate" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <ul style="list-style: none;">
                                <li>
                                    <img src="../Images/bullet.gif" width="10" height="9" />
                                    <a href="NewScheme.aspx?dt=<%=ddlDate.SelectedItem.Value%>&comID=<%=this.PropCompID %>" onclick="return validDate();"
                                        target="_self">New Schemes</a></li>
                                <li>
                                    <img src="../Images/bullet.gif" width="10" height="9" />
                                    <a href="ExistSchemes.aspx?dt=<%=ddlDate.SelectedItem.Value%>&comID=<%=this.PropCompID %>" onclick="return validDate();"
                                        target="_self">Existing Schemes</a></li>
                                <li>
                                    <img src="../Images/bullet.gif" width="10" height="9" />
                                    <a href="AllSchemes.aspx?dt=<%=ddlDate.SelectedItem.Value%>&comID=<%=this.PropCompID %>" onclick="return validDate();"
                                        target="_self">All Schemes</a></li>
                            </ul>
                        </li>
                        <li>
                            <img src="../Images/bullet.gif" width="10" height="9" />
                            <a href="RedemptionRepurchase.aspx?dt=<%=ddlDate.SelectedItem.Value%>&comID=<%=this.PropCompID %>" onclick="return validDate();"
                                style="font-weight: bold;" target="_self">Redemption / Repurchase</a></li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script type="text/javascript">
        function validDate() {
            var ddl = document.getElementById("<%=ddlDate.ClientID%>");
            if (ddl.value == "01/01/0001 12:00:00 AM") {
                alert("Please select any valid date.");
                return false;
            }
            else { return true; }
        }
    </script>
</body>
</html>
