<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundPortfolio.aspx.cs" Inherits="iFrames.Pages.FundPortfolio" %>
<%@ Register TagPrefix="drp" TagName="CustomDrop" Src="~/UserControl/DropdownCustomControl.ascx" %> 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr class="mainHeader">
            <td>
                PORTFOLIOS
                <hr />
                <br style="line-height: 25px;" />
            </td>
        </tr>
        
        <tr>
            <td class="content" valign="top" align="center">                
            Complete Portfolio along with Company, Sector, Asset & Instrument Allocation.            
            </td>
        </tr>        
        <tr>
            <td align="center">
                <drp:CustomDrop ID="DrpCustom" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="BtnGo" runat="server" Text="GO" OnClick="BtnGo_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
