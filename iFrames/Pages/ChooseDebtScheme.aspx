<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseDebtScheme.aspx.cs"
    Inherits="iFrames.Pages.ChooseDebtScheme" %>

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
                <td valign="top" colspan="3">
                    <table style="margin-top: 30px;">
                        <tr class="mainHeader">
                            <td>
                                CHOOSE A SCHEME
                                <hr />
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                        <tr class="SubHeader">
                            <td colspan="3">
                                'Choose a scheme' lets you create a list of mutual funds based on the criteria that
                                are important to you, individually. You can select on one parameter or as many as
                                you want from the menu given below and click on 'search'
                                <br style="line-height: 15px;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Structure
                </td>
                <td colspan="2" valign="top" class="labels">
                    <asp:RadioButtonList ID="RadioStructureLst" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">Open Ended</asp:ListItem>
                        <asp:ListItem>Closed Ended</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Nature
                </td>
                <td colspan="2" class="labels">
                    <asp:DropDownList ID="DrpNature" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Any Nature</asp:ListItem>
                        <asp:ListItem>Debt</asp:ListItem>
                        <asp:ListItem>Gilt</asp:ListItem>
                        <asp:ListItem>Short Term</asp:ListItem>
                        <asp:ListItem>Money Market</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Fund Age
                </td>
                <td colspan="2" class="labels">
                    <asp:DropDownList ID="DrpFundAge" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Any Age</asp:ListItem>
                        <asp:ListItem Value="3month">Less than 3 Months</asp:ListItem>
                        <asp:ListItem Value="6month">Not less than 3 Months</asp:ListItem>
                        <asp:ListItem Value="1year">Not less than 1 Year</asp:ListItem>
                        <asp:ListItem Value="3year">Not less than 3 year</asp:ListItem>
                        <asp:ListItem Value="5year">Not less than 5 Year</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Investment Objective
                </td>
                <td colspan="2" valign="top" class="labels">
                    <asp:RadioButtonList ID="RadioIncDiv" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">Income/Dividend </asp:ListItem>
                        <asp:ListItem>Growth</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Min. Invest.
                </td>
                <td colspan="2" class="labels">
                    <asp:DropDownList ID="DrpMinInvst" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Any Investment</asp:ListItem>
                        <asp:ListItem Value="<=,5000">Up to 5000</asp:ListItem>
                        <asp:ListItem Value="<=,10000">Up to 10000</asp:ListItem>
                        <asp:ListItem Value="<=,25000">Up to 25000</asp:ListItem>
                        <asp:ListItem Value=">,25000">More than 25000</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Fund Size (In Crores)
                </td>
                <td colspan="2" class="labels">
                    <asp:DropDownList ID="DrpFundSize" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Any Size</asp:ListItem>
                        <asp:ListItem Value="<,50">Less than 50 Cr.</asp:ListItem>
                        <asp:ListItem Value=">,50">Not less than 50 Cr.</asp:ListItem>
                        <asp:ListItem Value=">,100">Not less than 100 Cr.</asp:ListItem>
                        <asp:ListItem Value=">,500">Not less than 500 Cr.</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="SubHeader">
                    Performance
                    <hr />
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Period
                </td>
                <td width="35%" class="labels">
                    <asp:DropDownList ID="DrpPeriopd" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Select Period</asp:ListItem>
                        <asp:ListItem Value="per_15days">14 days</asp:ListItem>
                        <asp:ListItem Value="per_30days">30 Days</asp:ListItem>
                        <asp:ListItem Value="per_91days">91 Days</asp:ListItem>
                        <asp:ListItem Value="per_182days">182 Days</asp:ListItem>
                        <asp:ListItem Value="per_1yr">1 Year</asp:ListItem>
                        <asp:ListItem Value="per_3yr">3 Year</asp:ListItem>
                        <asp:ListItem Value="per_5yr">5 Year</asp:ListItem>
                        <asp:ListItem Value="since_incept">Since Inception</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="50%" class="labelsHead">
                    And Return <asp:DropDownList ID="DrpReturn" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Any Return</asp:ListItem>
                        <asp:ListItem Value="<,5">Less than 5%</asp:ListItem>
                        <asp:ListItem Value=">,5">Not less than 5%</asp:ListItem>
                        <asp:ListItem Value=">,10">Not less than 10%</asp:ListItem>
                        <asp:ListItem Value=">,20">Not less than 20%</asp:ListItem>
                        <asp:ListItem Value=">,50">Not less than 50% </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Current Dividend Yield
                </td>
                <td colspan="2" class="labels">
                    <asp:DropDownList ID="DrpCurDivYield" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Choose Any</asp:ListItem>
                        <asp:ListItem Value=">=,5">Not Less than 5%</asp:ListItem>
                        <asp:ListItem Value=">=,10">Not Less than 10%</asp:ListItem>
                        <asp:ListItem Value=">=,15">Not Less than 15%</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    NRI Investment
                </td>
                <td colspan="2" valign="top" class="labels">
                    <asp:RadioButtonList ID="RadioNriInvt" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Repatriability
                </td>
                <td colspan="2" valign="top" class="labels">
                    <asp:RadioButtonList ID="RadioRepartial" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Tax Benefits
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="ChkTaxBnft" runat="server" Text="112" />
                </td>
            </tr>
            <tr>
                <td width="15%" class="labelsHead">
                    Rating
                </td>
                <td width="35%" class="labels">
                    <asp:DropDownList ID="DrpRating" runat="server" Width="160px">
                    </asp:DropDownList>
                </td>
                <td width="50%" class="labelsHead">
                    Any Percent <asp:DropDownList ID="DrpPercent" runat="server" Width="160px">
                        <asp:ListItem Value=" ">Any Percent</asp:ListItem>
                        <asp:ListItem Value="<,'5'">Less than 5%</asp:ListItem>
                        <asp:ListItem Value=">,'5'">Not less than 5%</asp:ListItem>
                        <asp:ListItem Value=">,'10'">Not less than 10%</asp:ListItem>
                        <asp:ListItem Value=">,'50'">Not less than 50%</asp:ListItem>
                        <asp:ListItem Value=">,'90'">Not less than 90%</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" class="labels">
                    <asp:Button ID="BtnSearch" runat="server" Text="SEARCH" OnClick="BtnSearch_Click" />
                    <asp:Button ID="BtnClear" runat="server" Text="CLEAR" 
                        onclick="BtnClear_Click" />
                </td>
            </tr>
        </table>
        <table width="550px" id="tblscheme" style="display: inline" cellpadding="3" cellspacing="3">
            <tr>
                <td align="left" class="SubHeader">
                    SEARCH RESULTS
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListView ID="lstChooseScheme" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table cellpadding="3" border="1" cellspacing="3" style="border-color: Black; border-collapse: collapse;">
                                <tr class="ListtableHead">
                                   <th align="center" rowspan="2" ><asp:Label ID="lbl1" SkinID="lblHeader" runat="server">Scheme Name</asp:Label></th>
                                   <th align="center" colspan="2" ><asp:Label ID="Label3" SkinID="lblHeader" runat="server">NAV</asp:Label></th>
                                   <th align="center" rowspan="2" ><asp:Label ID="Label4" SkinID="lblHeader" runat="server">Structure</asp:Label></th>
                                   <th align="center" rowspan="2" ><asp:Label ID="Label5" SkinID="lblHeader" runat="server">Type</asp:Label></th>
                                   <th align="center" rowspan="2" ><asp:Label ID="Label6" SkinID="lblHeader" runat="server">Inception Date</asp:Label></th>
                                   <th align="center" colspan="4"><asp:Label ID="Label1" SkinID="lblHeader" runat="server">Performance in % </asp:Label></th>
                                   
                                   </tr>
                                <tr class="ListtableHead">
                                   <th align="center"><asp:Label ID="lbl2" SkinID="lblHeader" runat="server">As On</asp:Label></th>
                                   <th align="center"><asp:Label ID="lbl3" SkinID="lblHeader" runat="server">Rs.</asp:Label></th>
                                   <th align="center"><asp:Label ID="lbl4" SkinID="lblHeader" runat="server">91 Days</asp:Label></th>
                                   <th align="center"><asp:Label ID="lbl5" SkinID="lblHeader" runat="server">1 Year</asp:Label></th>
                                   <th align="center"><asp:Label ID="lbl6" SkinID="lblHeader" runat="server">3 Year</asp:Label></th>                                   
                                   <th align="center"><asp:Label ID="lbl8" SkinID="lblHeader" runat="server">As on</asp:Label></th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder">
                                </tr>
                            </table>
                            <asp:DataPager ID="dp" runat="server" PageSize="10" PagedControlID="lstChooseScheme">
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
                                <td width="30%" style="font-size:small" align="left">
                                    <a href="FundFactSheet1.aspx?sname=<%#Eval("sch_code")%>&comID=<%=this.PropCompID %>"><%#Eval("short_name").ToString()%></a>                                        
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("navason").ToString() != "" ? Convert.ToDateTime(Eval("navason").ToString()).ToString("MMM dd, yyyy") : "NA"%>   
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("nav_rs").ToString() != "" ? Eval("nav_rs") : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("type1").ToString() == "Open Ended" ? "O": "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("type3").ToString() == "Income/Dividend" ? "I" : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("fundage") %>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("91days").ToString() != "" ? Eval("91days") : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("1yrs").ToString() != "" ? Eval("1yrs") : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("3yrs").ToString() != "" ? Eval("3yrs") : "NA"%>    
                                    </td>                                    
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("TopdtAson").ToString() == "" ? "NA" : Convert.ToDateTime(Eval("TopdtAson").ToString()).ToString("MMM dd,yyyy")%>     
                                    </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            No Data Found
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
