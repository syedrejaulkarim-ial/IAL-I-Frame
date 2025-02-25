<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComapreFundRpt.aspx.cs" Inherits="iFrames.Pages.ComapreFundRpt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        function back() {
            history.back(-1);
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr class="mainHeader">
            <td>
                COMPARE FUND
                <hr />
                <br style="line-height: 25px; color: #bd2027;" />
            </td>
        </tr>
        
        <tr>
            <td class="content" valign="top" align="left">                
            Click on 'Time Period' to rank funds on a particular period of your choice....             
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GrdCompFund" runat="server" AutoGenerateColumns="false" 
                    onrowcommand="GrdCompFund_RowCommand" 
                    onrowdatabound="GrdCompFund_RowDataBound" >
                    <Columns>
                        <asp:TemplateField HeaderText="Scheme Name">
                            <ItemTemplate>
                                <asp:Label ID="lblSchName" runat="server" Text=' <%#Eval("short_name").ToString() != "" ? Eval("short_name") : "NA"%> '/>                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="Lnk1mth" runat="server" Text="1 mth" Font-Overline="false"
                                CommandName="per_30days" ForeColor="White" Font-Bold="true" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl1Mth" runat="server" Text='<%#Eval("per_30days").ToString() != "" ? Eval("per_30days") : "NA"%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="Lnk3mth" runat="server"  ForeColor="White" Font-Bold="true" Text="3 mths" Font-Overline="false"
                                 CommandName="per_91days"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl3Mth" runat="server">
                                <%#Eval("per_91days").ToString() != "" ? Eval("per_91days") : "NA"%> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="Lnk6mth" runat="server" ForeColor="White" Font-Bold="true" Text="6 mths" Font-Overline="false"
                                CommandName="per_182days"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl6Mth" runat="server">
                                <%#Eval("per_182days").ToString() != "" ? Eval("per_182days") : "NA"%> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="Lnk1yr" runat="server" ForeColor="White" Font-Bold="true" Text="1Yr" Font-Overline="false"
                                CommandName="per_1yr"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl1yr" runat="server">
                                <%#Eval("per_1yr").ToString() != "" ? Eval("per_1yr") : "NA"%> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="Lnk3yr" runat="server" ForeColor="White" Font-Bold="true" Text="3 Yrs" Font-Overline="false"
                                CommandName="per_3yr"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl3yr" runat="server">
                                <%#Eval("per_3yr").ToString() != "" ? Eval("per_3yr") : "NA"%> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="NAV"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblNav" runat="server">
                                <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs") : "NA"%> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label2" runat="server" SkinID="lblHeader" Text="Category"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCat" runat="server">
                                <%#Eval("nature").ToString() != "" ? Eval("nature") : "NA"%> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                 <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Structure"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStruct" runat="server">
                                <%#Eval("type1").ToString() != "" ? Eval("type1") : "NA"%> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>No Data Found</EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
        <td class="content">
        *Note:- Returns calculated for less than 1 year are Absolute returns 
        and returns calculated for more than 1 year are compounded annualized. 
        </td>
        </tr>
        <tr>
            <td align="left">
            <asp:Button ID="BtnBack" runat="server" Text="BACK" onclick="BtnBack_Click" />
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    
    </form>
</body>
</html>
