<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchemeChoose.aspx.cs" Inherits="iFrames.Pages.SchemeChoose" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="550px" border="0" cellpadding="3" cellspacing="3" style="height: 400px;
            float: inherit;">
            <tr>
                <td valign="top">
                    <table style="margin-top: 30px;">
                        <tr class="mainHeader">
                            <td>
                                CHOOSE A SCHEME
                                <hr />
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        <tr class="SubHeader">
                            <td>
                                Find funds using criteria important to YOU. Select the parameters important to you
                                and proceed. You can use a Simple search or an Advanced search to pick equity schemes.
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        
                    </table>
                </td>
                
            </tr>
            <tr>
                <td valign="top">
                <asp:TreeView ID="TreeView1" runat="server">
                    <Nodes >
                        <asp:TreeNode Text="Choose a Debt Fund" Value="Choose a Debt Fund"  ImageUrl="~/Images/bullet.gif"  NavigateUrl="~/Pages/ChooseDebtScheme.aspx" >
                        </asp:TreeNode>
                        <asp:TreeNode Text="Choose an Equity Fund" Value="Choose an Equity Fund" ImageUrl="~/Images/bullet.gif" >
                            <asp:TreeNode Text="Simple Search" Value="Simple Search" ImageUrl="~/Images/bullet.gif"></asp:TreeNode>
                            <asp:TreeNode Text="Advanced Search" Value="Advanced Search" ImageUrl="~/Images/bullet.gif">
                                <asp:TreeNode Text="&nbsp;Company Wise Search " Value="Company Wise Search " ImageUrl="~/Images/bullet.gif" >
                                </asp:TreeNode>
                                <asp:TreeNode Text="&nbsp;Sector Wise Search" Value="Sector Wise Search" ImageUrl="~/Images/bullet.gif">
                                </asp:TreeNode>
                            </asp:TreeNode>
                        </asp:TreeNode>
                        
                    </Nodes>
                </asp:TreeView>
                </td>
            </tr>
            
        </table>
    </div>
    </form>
</body>
</html>
