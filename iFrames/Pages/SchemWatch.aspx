<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchemWatch.aspx.cs" Inherits="iFrames.Pages.SchemWatch" %>
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
                SCHEME WATCH
                <hr />
                <br style="line-height: 15px;"/>
            </td>
        </tr>
       
        <tr>
            <td class="SubHeader" valign="top" align="left">
               Our Research Analyst views on selected schemes. 
               <br style="line-height: 15px;"/>
            </td>
        </tr>
        
    </table>
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr>
            <td>
                <asp:ListView ID="LstSchemeWatch" runat="server"  GroupItemCount="2"  
                    onpagepropertieschanging="LstSchemeWatch_PagePropertiesChanging" 
                    >
               
                    <LayoutTemplate>
                        <table border="1" cellpadding="2" width="550px" style="border-color:Black;border-collapse:collapse;">
                            <tr runat="server" id="groupPlaceholder" >
                            </tr>
                            
                        </table>
                        <asp:DataPager runat="server" ID="SchemeWatchDataPager" PageSize="40" PagedControlID="LstSchemeWatch">
                            <Fields>
                                <asp:TemplatePagerField>
                                    <PagerTemplate>
                                        <span class="labelsHead">Page
                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                            of
                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                            (
                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                            records)</span>
                                            <br />
                                       
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
                    </LayoutTemplate>
                    <GroupTemplate>
                        <tr runat="server" id="productRow" width="100%">
                            <td runat="server" id="itemPlaceholder">
                            </td>
                        </tr>
                          
                    </GroupTemplate>
                    <ItemTemplate>
                        <td id="Td1" valign="top" align="left" style="width: 100" runat="server" class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? GetAlterColor(Container.DisplayIndex) : GetAlterColor(Container.DisplayIndex) %>'>
                             <asp:HyperLink ID="ProductLink" runat="server" Target="_blank" Text='<% #Eval("Nature")%>'
                             NavigateUrl='<%#Eval("Data")%>'/>
                    </td>
                    </ItemTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
