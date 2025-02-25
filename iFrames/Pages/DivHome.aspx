<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DivHome.aspx.cs" Inherits="iFrames.Pages.DivHome" %>
<%@ Register Src="~/UserControl/DropdownCustomControl.ascx" TagName="Control" TagPrefix="DCC" %>

<%--<asp:Content ContentPlaceHolderID="cpChildContent" runat="server">
<table width="450px" cellpadding="2" cellspacing="2">
        <tr>
            <td align="center">
                <DCC:Control id="UControl" runat="Server"/>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblHead" runat="server"/>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="grdResult" runat="server" AllowPaging="false" AutoGenerateColumns="false">
                <Columns>                    
                    <asp:TemplateField HeaderText="Record Date">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Convert.ToDateTime(Eval("record_date").ToString()).ToString("MMM dd, yyyy") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Rate of Dividend" DataField="divid_pt" />
                    <asp:BoundField HeaderText="(Bonus/Rights)" DataField="div" />
                </Columns>
                </asp:GridView>
            </td>
        </tr> 
        <tr>
            <td align="center">
            <br style="line-height:30px;" />
            <a href="DivDec.aspx" target="_blank" style="text-decoration:none;">Latest Dividend</a>
            </td>
        </tr>
    </table>
</asp:Content>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="450px" cellpadding="2" cellspacing="2">
        <tr>
            <td align="center">
                <DCC:Control id="UControl" runat="Server"/>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" Class="mainHeader">               
                <%=PageHead%>              
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView AllowPaging="true" PageSize="10" ID="grdResult" runat="server" 
                    AutoGenerateColumns="false" onpageindexchanging="grdResult_PageIndexChanging">                
                <Columns>                    
                    <asp:TemplateField HeaderText="Record Date">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Convert.ToDateTime(Eval("record_date").ToString()).ToString("MMM dd, yyyy") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Rate of Dividend" DataField="divid_pt" />
                    <asp:BoundField HeaderText="(Bonus/Rights)" DataField="div" />
                </Columns>
                </asp:GridView>
            </td>
        </tr> 
        <tr>
            <td align="center">
            <br style="line-height:30px;" />
            <a href="DivDec.aspx?comID=<%=this.PropCompID %>" target="_blank" style="text-decoration:none;">Latest Dividend</a>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
