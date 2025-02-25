<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComSchemeChoose.aspx.cs"
    Inherits="iFrames.Pages.ComSchemeChoose" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSearch" defaultfocus="Search">
    <div>
        <table cellpadding="3" cellspacing="3" width="550px">
            <tr class="mainHeader">
                <td colspan="3">
                    CHOOSE A SCHEME
                    <hr />
                    <br style="line-height: 15px;" />
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="3" cellspacing="3">
                        <tr id="trCompPortfolio" runat="server" class="content">
                            <td style="width: 167px;">
                                <strong>Company Portfolio</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCompPortfolio" runat="server" Width="344px" Height="15px"></asp:TextBox><br />
                                <center><asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label></center>
                            </td>
                        </tr>
                        <tr id="trCompName" runat="server" class="content" visible="false">
                            <td style="width: 167px;">
                                <strong>Choose Company</strong>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompany" runat="server" Width="349px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trPercent" runat="server" class="content" visible="false">
                            <td style="width: 167px;">
                                <strong>Choose Percentage</strong>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPercentage" runat="server" Width="349px">
                                    <asp:ListItem Value="0" Text="Choose %Age" />
                                    <asp:ListItem Value="5" Text="Not less than 5%" />
                                    <asp:ListItem Value="10" Text="Not less than 10%" />
                                    <asp:ListItem Value="25" Text="Not less than 25%" />
                                    <asp:ListItem Value="50" Text="Not less than 50%" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="return validateText();" />
                                &nbsp;
                                <asp:Button ID="btnClear" runat="server" Text="Clear" 
                                    onclick="btnClear_Click" />
                                <script type="text/javascript">
                                    String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ''); };
                                    function validateText() {
                                    var ddl=document.getElementById("<%=ddlCompany.ClientID%>");
                                    var formChecker="<%=flag%>";
                                    if (formChecker=="N") 
                                        {
                                            if (document.getElementById("<%=txtCompPortfolio.ClientID%>").value.trim() == "") 
                                            {
                                                alert("Please enter some text.");
                                                document.getElementById("<%=txtCompPortfolio.ClientID%>").focus();
                                                return false;
                                            }
                                            else { return true; }
                                        }
                                        else 
                                        {
                                            if (ddl.value == "Select") {
                                                alert("Please select an option.");
                                                ddl.focus();
                                                return false;
                                            }
                                            else if (document.getElementById("<%=ddlPercentage.ClientID%>").value == "0") {
                                                alert("Please select an option.");
                                                document.getElementById("<%=ddlPercentage.ClientID%>").focus();
                                                return false;
                                            }
                                            else { return true; }
                                            }
                                    }

                                </script>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
            <td>
            
                <asp:ListView ID="lstSchemes" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <table cellpadding="3" width="550px" cellspacing="3" border="1" style="border-color: black;
                                        border-collapse: collapse;">
                                        <tr class="ListtableHead" style="text-align:center;">
                                            <td rowspan="2">Scheme Name</td>
                                            <td colspan="2">NAV</td>
                                            <td rowspan="2">Structure</td>
                                            <td rowspan="2">Type</td>
                                            <td colspan="2">Equity</td>
                                            <td colspan="4">Performance in %</td>
                                        </tr>
                                        <tr style="text-align:center;" class="ListtableHead">
                                            <td>As on</td>
                                            <td>Rs.</td>
                                            <td>As on</td>
                                            <td>%</td>
                                            <td>91 Days</td>
                                            <td>1 Yr</td>
                                            <td> 3 Yrs</td>
                                            <td>As On</td>
                                        </tr> 
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                    <asp:DataPager runat="server" ID="dp" PageSize="10" PagedControlID="lstSchemes">
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
                                <ItemTemplate>
                                    <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                        <td>
                                            <a href='FundFactSheet1.aspx?sch=<%#Eval("sch_code")%>&comID=<%=this.PropCompID %>'><%#Eval("short_name").ToString()%></a>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToDateTime(Eval("LDate").ToString()).ToString("MMM dd, yyyy")%>
                                        </td>
                                        <td align="center">
                                            <%#Math.Round(Convert.ToDecimal(Eval("Nav_Rs").ToString()),2).ToString()%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("type1").ToString().ToLower() == "open ended" ? "O" : "C"%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("type3").ToString().ToLower() == "income/dividend" ? "I" : "G"%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToDateTime(Eval("netDate").ToString()).ToString("MMM dd, yyyy")%>
                                        </td>
                                        <td align="center">
                                            <%#Math.Round(Convert.ToDecimal(Eval("corpus_per").ToString()), 2).ToString()%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("per_91days") == null | Eval("per_91days").ToString()==""? "NA" : Math.Round(Convert.ToDecimal(Eval("per_91days").ToString()), 2).ToString()%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("per_1yr") == null | Eval("per_1yr").ToString()==""? "NA" : Math.Round(Convert.ToDecimal(Eval("per_1yr").ToString()), 2).ToString()%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("per_3yr") == null | Eval("per_3yr").ToString()=="" ? "NA" : Math.Round(Convert.ToDecimal(Eval("per_3yr").ToString()), 2).ToString()%>
                                        </td>
                                        <td align="center">
                                            <%#Convert.ToDateTime(Eval("TDate").ToString()).ToString("MMM dd, yyyy")%>
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
</body>
</html>
