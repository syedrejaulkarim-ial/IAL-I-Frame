<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmcDirectory.aspx.cs" Inherits="iFrames.Pages.AmcDirectory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">        
    </script>
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
                            <td colspan="3">
                                AMC DIRECTORY
                                <hr />
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        <tr class="SubHeader" height="45px">
                            <td colspan="3">
                                Mutual Fund houses contacts
                            </td>
                        </tr>
                        <tr class="content">
                            <td>
                                FUND HOUSE
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFundHouse" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                                <br style="line-height: 25px;" />                                
                            </td>
                        </tr>                       
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblMutName" runat="server" SkinID="lblHeader" ForeColor="Maroon" />
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tblCorp" runat="server" width="550px" cellpadding="3" cellspacing="3" border="1" style="border-collapse: collapse;
                        border-color: Black;">
                        <tr class="ListtableHead">
                            <th>
                                Fund name
                            </th>
                            <th>
                                Details
                            </th>
                        </tr>
                        <tr class="ListtableRow">
                            <td valign="top">
                                <%=FundName %>
                            </td>
                            <td>
                                <%=CorpAddr%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListView ID="lstBranchOff" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table runat="server" cellpadding="3"  width="550px" cellspacing="3" border="1" style="border-color: black;
                                border-collapse: collapse;">
                                <tr class="ListtableHead">
                                    <th>
                                        Office Type
                                    </th>
                                    <th>
                                        Details
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                            <asp:DataPager runat="server" ID="dp" PageSize="5" PagedControlID="lstBranchOff">
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
                                    <asp:NumericPagerField PreviousPageText="&lt; Prev 5" NextPageText="Next 5 &gt;"
                                        ButtonCount="5" />
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowNextPageButton="false"
                                        ShowPreviousPageButton="false" />
                                </Fields>
                            </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                <td valign="top" width="35%">
                                    <%#Eval("br_name")%>
                                </td>
                                <td  width="65%">
                                    <%#Eval("BR_ADD1") == null | Eval("BR_ADD1").ToString().Trim() == "" ? "" : Eval("BR_ADD1").ToString()%>
                                    <br />
                                    <%#Eval("BR_ADD2") == null | Eval("BR_ADD2").ToString().Trim() == "" ? "" : Eval("BR_ADD2").ToString()%>
                                    <br />
                                    <%#Eval("BR_CITY") == null | Eval("BR_CITY").ToString().Trim() == "" ? "" : Eval("BR_CITY").ToString()%>
                                    &nbsp;-&nbsp;
                                    <%#Eval("BR_PIN") == null | Eval("BR_PIN").ToString().Trim() == "" ? "" : Eval("BR_PIN").ToString()%>
                                    <br />
                                    <%#Eval("Br_Phone1") == null | Eval("Br_Phone1").ToString().Trim() == "" ? "" : "Tel.- " + Eval("Br_Phone1").ToString()%>
                                    <%#Eval("Br_Phone2") == null | Eval("Br_Phone2").ToString().Trim() == "" ? "" : ", " + Eval("Br_Phone2").ToString()%>
                                    <%#Eval("Br_Phone3") == null | Eval("Br_Phone3").ToString().Trim() == "" ? "" : ", " + Eval("Br_Phone3").ToString()%>
                                    <%#Eval("BR_FAX") == null | Eval("BR_FAX").ToString().Trim() == "" ? "" : " Fax - " + Eval("BR_FAX").ToString()%>
                                    <br />
                                    <%#Eval("BR_CONPER") == null | Eval("BR_CONPER").ToString().Trim() == "" ? "" : "Contact Person- " + Eval("BR_CONPER").ToString()%>
                                                                       
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            No data Found
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script type="text/javascript">
        var tbl = document.getElementById('tblCorp');
        function validateForm() {
            var ddl = document.getElementById('ddlFundHouse');
            if (ddl.value == '') {
                alert('Please select any Fund Name.');
                return false;
            } else {
                return true;
            }
        }    
    </script>
</body>
</html>
