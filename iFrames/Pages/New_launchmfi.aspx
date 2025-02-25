<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New_launchmfi.aspx.cs"
    Inherits="iFrames.Pages.New_launchmfi" %>

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
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr class="mainHeader">
            <td>
                NFO UPDATE
                <hr />
                <br style="line-height: 15px;" />
            </td>
        </tr>
        <tr>
            <td class="content" valign="top" align="left">
                Welcome to the 'NFO Update' division of Mutual Funds India. The following is the
                list of ongoing new schemes launched by various mutual funds in india. In order
                to view the details of the scheme or to invest in the scheme, Click on the Scheme
                Name.
            </td>
        </tr>
    </table>
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr>
            <td>
                <asp:ListView ID="lstVwLaunch" runat="server" DataKeyNames="sch_code" OnItemCommand="lstVwLaunch_ItemCommand"
                    OnItemDataBound="lstVwLaunch_ItemDataBound" ItemPlaceholderID="itemPlaceHolder1">
                    <LayoutTemplate>
                        <table border="1" cellpadding="3" width="550px" style="border-color: Black; border-collapse: collapse;">
                            <tr class="ListtableHead">
                                <th align="left">
                                    <asp:Label ID="lbl1" SkinID="lblHeader" runat="server">Scheme Name</asp:Label>
                                </th>
                                <th align="left">
                                    <asp:Label ID="lbl2" SkinID="lblHeader" runat="server">Mutual Fund</asp:Label>
                                </th>
                                <th align="left">
                                    <asp:Label ID="lbl3" SkinID="lblHeader" runat="server">Close Date</asp:Label>
                                </th>
                                <th>
                                </th>
                            </tr>
                            <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? "AlternateRow":"ListtableRow" %>'>
                            <td>
                                <asp:Label runat="server" ID="lblId" Text='<%#Eval("sch_name") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblName" Text='<%#Eval("mut_name")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblType" Text='<%#Convert.ToDateTime(Eval("close_date")).ToString("dd MMM yyyy")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="viewItem" CommandName="view" OnClientClick="javascript:return false;">View</asp:LinkButton>
                            </td>
                        </tr>
                        <tr id="Tr1" runat="server" style="height: 0px;">
                            <td colspan="4">
                                <asp:Panel runat="server" ID="pnlExpender">
                                    <table border="1" cellpadding="3" width="500px" style="border-color: Black; border-collapse: collapse;">
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstVwLaunchinfo" runat="server" ItemPlaceholderID="itemPlaceHolder2">
                                                    <LayoutTemplate>
                                                        <asp:PlaceHolder ID="itemPlaceHolder2" runat="server" />
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <table border="0" cellpadding="3" width="540px" style="border-color: Black; border-collapse: collapse;">
                                                            <tr>
                                                                <td>
                                                                    <table width="540px">
                                                                        <tr class="ListtableHead">
                                                                            <td>
                                                                                <strong>Offer Open:</strong>
                                                                            </td>
                                                                            <td>
                                                                                <strong>Offer Close:</strong>
                                                                            </td>
                                                                            <td>
                                                                                <strong>Structure:</strong>
                                                                            </td>
                                                                            <td>
                                                                                <strong>Nature:</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="ListtableRow">
                                                                            <td>
                                                                                <asp:Label runat="server" ID="lblIdd" Text='<%#Convert.ToDateTime(Eval("Iss_date")).ToString("MMM dd, yyyy")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label runat="server" ID="Label1" Text='<%#Convert.ToDateTime(Eval("close_date")).ToString("MMM dd, yyyy")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label runat="server" ID="Label2" Text='<%#Eval("Type1")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label runat="server" ID="Label3" Text='<%#Eval("nature")%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="540px">
                                                                        <tr class="content">
                                                                            <td>
                                                                                <strong>Scheme Objective</strong>
                                                                                <br style="line-height: 25px;" />
                                                                                <%#Eval("object")%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="540px">
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <hr />
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="ListtableHead">
                                                                            <td valign="top" width="40%">
                                                                                Mutual Fund:
                                                                            </td>
                                                                            <td width="15%">
                                                                                Minimum Investment:
                                                                            </td>
                                                                            <td width="15%">
                                                                                Incremental Investment:
                                                                            </td>
                                                                            <td width="15%">
                                                                                SIP allowed:
                                                                            </td>
                                                                            <td width="15%">
                                                                                NRI Investment :
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <hr />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="labels" width="40%">
                                                                                <asp:Label runat="server" ID="Label4" Text='<%#Eval("Mut_Name")+" </br>"+Eval("Reg_Add1")+" </br>"+Eval("Reg_Add2")+"</br>"+Eval("Reg_city")+" </br>"+Eval("Reg_Phone1") %>'></asp:Label>
                                                                            </td>
                                                                            <td style="font-size: small" width="15%">
                                                                                <asp:Label runat="server" ID="Label5" Text='<%#Eval("min_invt")%>'></asp:Label>
                                                                            </td>
                                                                            <td style="font-size: small" width="15%">
                                                                                <asp:Label runat="server" ID="Label7" Text='<%#Eval("incr_invt")%>'></asp:Label>
                                                                            </td>
                                                                            <td style="font-size: small" width="15%">
                                                                                <asp:Label runat="server" ID="Label6" Text='<%#Eval("sip")%>'></asp:Label>
                                                                            </td>
                                                                            <td style="font-size: small" width="15%">
                                                                                <asp:Label runat="server" ID="Label8" Text='<%#Eval("nri")%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <cc1:CollapsiblePanelExtender Collapsed="True" ID="cpe" runat="Server" TargetControlID="pnlExpender"
                            ExpandControlID="viewItem" CollapseControlID="viewItem">
                        </cc1:CollapsiblePanelExtender>
                    </ItemTemplate>
                    <%-- <alternatingitemtemplate>
                      <tr style="background-color:#EFEFEF">
                       <td><asp:Label runat="server" ID="lblId" Text='<%#Eval("sch_name") %>'></asp:Label></td>
                       <td><asp:Label runat="server" ID="lblName" Text='<%#Eval("mut_name")%>'></asp:Label></td>
                       <td><asp:Label runat="server" ID="lblType" Text='<%#Convert.ToDateTime(Eval("close_date")).ToString("dd MMM yyyy")%>'></asp:Label></td>
                       <td><asp:LinkButton runat="server" ID="viewItem" OnClientClick="javascript:return false;">View</asp:LinkButton></td>
                      </tr>
                      <tr id="Tr1" runat="server" style="height: 0px;">
                            <td colspan="4">
                                <asp:Panel runat="server" ID="pnlExpender" BackColor="Bisque">
                                    <table border="0" cellpadding="1" width="100%">
                                        <tr style="font-size:small;background-color:#e5eef9">
                                            <td >
                                                
                                                        <asp:ListView ID="lstVwLaunchinfo" runat="server" ItemPlaceholderID="itemPlaceHolder2">
                                                    <LayoutTemplate>
                                                        <asp:PlaceHolder ID="itemPlaceHolder2" runat="server" />
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                    <table width="100%">
                                                    <tr>
                                                    <td>
                                                        <div style="float: left; text-align: left; width: 20%">
                                                            <table>
                                                                <tr>
                                                                    <td style="font-size: medium">
                                                                        Offer Open:
                                                                    </td>
                                                                    <td style="font-size: small">
                                                                        <asp:Label runat="server" ID="lblIdd" Text='<%#Convert.ToDateTime(Eval("Iss_date")).ToString("MMM dd, yyyy")%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-size: medium">
                                                                        Offer Close:
                                                                    </td>
                                                                    <td style="font-size: small">
                                                                        <asp:Label runat="server" ID="Label1" Text='<%#Convert.ToDateTime(Eval("close_date")).ToString("MMM dd, yyyy")%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-size: medium">
                                                                        Structure:
                                                                    </td>
                                                                    <td style="font-size: small">
                                                                        <asp:Label runat="server" ID="Label2" Text='<%#Eval("Type1")%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-size: medium">
                                                                        Nature:
                                                                    </td>
                                                                    <td style="font-size: small">
                                                                        <asp:Label runat="server" ID="Label3" Text='<%#Eval("nature")%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="float: left; text-align: left; width: 50%">
                                                            <table>
                                                                <tr>
                                                                    <td style="font-size: medium">
                                                                        Scheme Objective
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <%#Eval("object")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                       </td>
                                                       </tr>
                                                       </table>
                                                       <table width="100%">
                                                       <tr>
                                                       <td>
                                                       <div style="float: left; text-align: left; width:100%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="5">
                                                                    <hr />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                <td style="font-size: medium" valign="top" width="40%">
                                                                        Mutual Fund:
                                                                    </td>
                                                                    <td style="font-size: medium" width="15%">
                                                                        Minimum Investment:
                                                                    </td>
                                                                    
                                                                    <td style="font-size: medium" width="15%">
                                                                        Incremental Investment:
                                                                    </td>
                                                                    <td style="font-size: medium" width="15%">
                                                                        SIP allowed:
                                                                    </td>
                                                                    <td style="font-size: medium" width="15%">
                                                                        NRI Investment :
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5">
                                                                    <hr />
                                                                    </td>
                                                                </tr>
                                                                   
                                                                    <td style="font-size: small" width="40%">
                                                                        <asp:Label runat="server" ID="Label4" Text='<%#Eval("Mut_Name")+" </br>"+Eval("Reg_Add1")+" </br>"+Eval("Reg_Add2")+"</br>"+Eval("Reg_city")+" </br>"+Eval("Reg_Phone1") %>'></asp:Label>
                                                                    </td>
                                                                    <td style="font-size: small" width="15%">
                                                                        <asp:Label runat="server" ID="Label5" Text='<%#Eval("min_invt")%>'></asp:Label>
                                                                    </td>
                                                                    <td style="font-size: small" width="15%">
                                                                        <asp:Label runat="server" ID="Label7" Text='<%#Eval("incr_invt")%>'></asp:Label>
                                                                    </td>
                                                                    <td style="font-size: small" width="15%">
                                                                        <asp:Label runat="server" ID="Label6" Text='<%#Eval("sip")%>'></asp:Label>
                                                                    </td>
                                                                    <td style="font-size: small" width="15%">
                                                                        <asp:Label runat="server" ID="Label8" Text='<%#Eval("nri")%>'></asp:Label>
                                                                    </td>
                                                                </tr>                                                                
                                                            </table>
                                                        </div>
                                                       </td>
                                                       </tr>
                                                       </table> 
                                                    </ItemTemplate>
                                                </asp:ListView>
                                                       
                                            </td>
                                            
                                            
                                        </tr>                                       
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                       <cc1:CollapsiblePanelExtender Collapsed="True" ID="cpe" runat="Server" TargetControlID="pnlExpender"
                            ExpandControlID="viewItem" CollapseControlID="viewItem">
                        </cc1:CollapsiblePanelExtender> 
                </alternatingitemtemplate>--%>
                    <EmptyDataTemplate>
                        No DataFound
                    </EmptyDataTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="labels">
            <td>
                <asp:DataPager runat="server" ID="ContactsDataPager" PageSize="3" PagedControlID="lstVwLaunch">
                    <Fields>
                        <asp:TemplatePagerField>
                            <PagerTemplate>
                                <b>Page
                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                    of
                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                    (
                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                    records)
                                    <br />
                                </b>
                            </PagerTemplate>
                        </asp:TemplatePagerField>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowNextPageButton="false"
                            ShowPreviousPageButton="false" />
                        <asp:NumericPagerField PreviousPageText="&lt; Prev 10" NextPageText="Next 10 &gt;"
                            ButtonCount="10" />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowNextPageButton="false"
                            ShowPreviousPageButton="false" />
                    </Fields>
                </asp:DataPager>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
