<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transaction.aspx.cs" Inherits="iFrames.Pages.Transaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="550px" cellpadding="3" cellspacing="3">
            <tr class="mainHeader">
                <td colspan="3">
                    FII & MF TRANSACTIONS
                    <hr />
                    <br style="line-height: 15px;" />
                </td>
            </tr>
            <tr class="SubHeader" align="center">
                <td>
                    Actions by FIIs & MFs
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="550px">
                        <tr>
                            <asp:ListView ID="lstTransaction" runat="server" OnItemDataBound="lst_ItemDataBound"
                                ItemPlaceholderID="itemPlaceHolder1" DataKeyNames="org">
                                <LayoutTemplate>
                                    <table cellpadding="3" width="550px" cellspacing="3" border="1" style="border-color: black;
                                        border-collapse: collapse;">
                                        <tr class="ListtableHead">
                                        </tr>
                                        <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                                    </table>
                                </LayoutTemplate>
                                <%--<GroupTemplate>
                                    <tr id="groupPlaceholder" runat="server"></tr>
                                </GroupTemplate> --%>
                                <ItemTemplate>
                                    <tr id="GroupRow" runat="server">
                                        <td align="left" style="font-family: Verdana; font-size: 9.5px;">                                            
                                                <%#"By " + Eval("org") + "s as on " + Convert.ToDateTime(Eval("date").ToString()).ToString("MMM dd, yyyy")%>
                                        </td>
                                        <td  align="right" style="font-family: Verdana; font-size: 9.5px;">
                                            Amount in Rs. Crores
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ListView ID="lstItems" runat="server" ItemPlaceholderID="itemPlaceHolder2">
                                                <LayoutTemplate>
                                                    <table cellpadding="3" width="550px" cellspacing="3" border="1" style="border-color: black;
                                                        border-collapse: collapse;" >
                                                        <tr class="ListtableHead">
                                                            <th>
                                                                Nature
                                                            </th>
                                                            <th>
                                                                Gross Pur.
                                                            </th>
                                                            <th>
                                                                Gross Sale
                                                            </th>
                                                            <th>
                                                                Net
                                                            </th>
                                                        </tr>
                                                        <asp:PlaceHolder ID="itemPlaceHolder2" runat="server" />                                                        
                                                    </table>                                                    
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                                        <td>
                                                            <%#Eval("Nature").ToString() == "Total" ? "<strong>" + Eval("Nature").ToString() + "</strong>" : Eval("Nature").ToString()%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Convert.ToDouble(Eval("pur")) != 0 ? Eval("pur") : "--"%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Convert.ToDouble(Eval("sale")) != 0 ? Eval("sale").ToString() : "--"%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Convert.ToDouble(Eval("net")) != 0 ? Math.Round(Convert.ToDecimal(Eval("net").ToString()),2).ToString() : "--"%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    No data Found
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
