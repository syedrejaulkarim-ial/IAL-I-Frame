<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchemeUndergone.aspx.cs" Inherits="iFrames.Pages.SchemeUndergone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="scriptManager" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <table width="550px" border="0" cellpadding="3" cellspacing="3" style="height: 400px;
            float: inherit;">
            <tr>
                <td valign="top">
                    <table style="margin-top: 30px;">
                        <tr class="mainHeader">
                            <td>
                                SCHEMES UNDERGONE NAME CHANGE
                                <hr />
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        <tr class="SubHeader">
                            <td>
                                
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        <tr class="content" style="line-height: 45px;">
                            <td align="center">
                                <strong>Enter Scheme Name or Scheme Code</strong>
                                <br />
                                <asp:TextBox ID="txtSchemes" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                               
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:RequiredFieldValidator ControlToValidate="txtSchemes" ID="RequiredFieldValidator1"
                                    runat="server" ErrorMessage="Please provide Scheme Code or Scheme Name" Font-Size="11px"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="SubHeader">
                <td>
                    Following is the list of Schemes whose name has been changed.
                    <br style="line-height: 15px;" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListView ID="lstConvertedScheme" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table cellpadding="3" border="1" cellspacing="3" style="border-color: Black; border-collapse: collapse;">
                                <tr class="ListtableHead">
                                    <th id="sname" runat="server">
                                       Old Name
                                    </th>
                                    
                                    <th id="sdiv" runat="server">
                                     New Name
                                    </th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder">
                                </tr>
                            </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstConvertedScheme">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <span class="lblHeader">Page
                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                                of
                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling ((double)Container.TotalRowCount / Container.PageSize) %>" />
                                                (
                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                                records)
                                                <br />
                                            </span>
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
                        <ItemTemplate>
                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                <td>
                                    <%#Eval("Fund_Text").ToString()%>
                                </td>
                                
                                <td>
                                    <%#Eval("short_name").ToString() != "" ? Eval("short_name").ToString() : "NA"%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            Your Scheme or Fund name is not matched.
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
