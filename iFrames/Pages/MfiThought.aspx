<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MfiThought.aspx.cs"  Inherits="iFrames.Pages.MfiThought" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="550px" cellpadding="0" cellspacing="0">
            <tr class="mainHeader">
            <td>
                MFI Thoughts
                <hr />
                <br style="line-height: 15px;"/>
            </td>
        </tr>
            <tr class="SubHeader">
                <td>
                        Our research experts lay their views on mutual fund industry trends, results and
                        actions.
                        <br style="line-height:25px;" />
                </td>
            </tr>            
            <tr>
                <td>
                    <asp:ListView ID="lstResult" runat="server" GroupItemCount="2" OnPagePropertiesChanging="lst_PagePropertiesChanging"
                    ItemPlaceholderID="Td1">
                        <LayoutTemplate>
                            <table border="1" cellpadding="3" width="550px" style="border-color:Black;border-collapse:collapse;">
                            <tr runat="server" id="groupPlaceholder">
                            </tr>                            
                        </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="20" PagedControlID="lstResult">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <span class="labelsHead">Page
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
                                    <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText="<--" ShowNextPageButton="False"
                                        NextPageText="<" />
                                    <asp:NumericPagerField />
                                    <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText="-->" ShowPreviousPageButton="False"
                                        PreviousPageText=">" />
                                </Fields>
                            </asp:DataPager>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <tr runat="server" id="productRow" >
                                <td runat="server" id="Td1">                                
                                </td>
                            </tr>
                        </GroupTemplate>
                        <ItemTemplate>                           
                                <td id="Td1" valign="top" align="left" style="width: 100" runat="server" class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? GetAlterColor(Container.DisplayIndex) : GetAlterColor(Container.DisplayIndex) %>'>                              
                                    <%#Eval("d_line")%>.....<a href="mfi_thoughts_disp.asp?news_headline=<%#HttpUtility.UrlEncode(Eval("html").ToString())%>&comID=<%=this.PropCompID %>">(more)</a>
                                </td>
                           
                        </ItemTemplate>
                        
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
