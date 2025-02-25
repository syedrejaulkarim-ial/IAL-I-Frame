<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetUnderMgmt.aspx.cs"
    Inherits="iFrames.Pages.AssetUnderMgmt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="550px" cellpadding="0" cellspacing="0">
        <tr class="mainHeader">
            <td>
                ASSET UNDER MANAGEMENT
                <hr />
                <br style="line-height: 25px; color: #bd2027;" />
            </td>
        </tr>
        <tr>
            <td class="content" valign="top" align="left">
                Latest Average Asset Under Management for all Mutual Fund houses, increase or decrease
                in corpus, sales & redemption figures..
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
    </table>
    <table width="550px" cellpadding="1">
        <tr>
            <td>
                <asp:ListView ID="LstAum" runat="server" OnPagePropertiesChanging="LstAum_PagePropertiesChanging">
                    <LayoutTemplate>
                        <table border="2" cellpadding="2" width="550px" style="border-collapse: collapse; border-color:#077495;">
                            <tr class="ListtableHead">
                                <th align="left" rowspan="2">
                                    Mutual Fund Name
                                </th>
                                <th align="left" rowspan="2">
                                    No. of Schemes*
                                </th>
                                <th colspan="5" align="center">
                                    Asset Under Management
                                </th>
                            </tr>
                            <tr class="ListtableHead">
                                <th>
                                    As on
                                </th>
                                <th>
                                    Corpus
                                </th>
                                <th>
                                    As on
                                </th>
                                <th>
                                    Corpus
                                </th>
                                <th>
                                    Net inc/dec in corpus
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                        <asp:DataPager runat="server" ID="AsmDataPager" PageSize="10" PagedControlID="LstAum">
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
                            <td width="15%" align="center">
                                <a href="amc_snapshot.asp?amc_name=<%#Eval("amc_code")%>&comID=<%=this.PropCompID %>">
                                    <%#Eval("Mut_Name").ToString()%></a>
                            </td>
                            <td width="10%" align="center">
                                <%#Eval("Total").ToString() != "" ? Eval("Total") : "NA"%>
                            </td>
                            <td width="15%"  align="center">
                                <%#Eval("Date").ToString() != "" ? Convert.ToDateTime(Eval("Date").ToString()).ToString("MMM dd, yyyy") : "NA"%>
                            </td>
                            <td width="15%"  align="center">
                                <%#Eval("Corpus_Crs_").ToString() != "" ? Eval("Corpus_Crs_") : "NA"%>
                            </td>
                            <td width="15%"  align="center">
                                <%#Eval("seconddate").ToString() != "" ? Convert.ToDateTime(Eval("seconddate").ToString()).ToString("MMM dd, yyyy") : "NA"%>
                            </td>
                            <td width="15%" align="center">
                                <%#Eval("secondcorpus").ToString() != "" ? Eval("secondcorpus") : "NA"%>
                            </td>
                            <td width="15%" align="center">
                                <%#Eval("netnic").ToString() != "" ? Math.Round(double.Parse(Eval("netnic").ToString()), 3).ToString() : "NA"%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No data Found
                    </EmptyDataTemplate>
                </asp:ListView>
                <br style="line-height:25px;" />
            </td>
        </tr>        
    </table>
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr>
            <td class="labelsHead">
                MUTUAL FUND DATA FOR THE MONTH ENDED -<asp:Label ID="LblDate" SkinID="lblHeader"
                    runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="550px" cellpadding="3" cellspacing="3">
        <tr>
            <td>
                <asp:ListView ID="LstMutDataGr1" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                    <LayoutTemplate>
                        <table border="1" cellpadding="2" width="550px" style="border-collapse: collapse; border-color:Black;">
                            <tr  class="ListtableHead">
                                <th align="left" rowspan="2">
                                    Category
                                </th>
                                <th align="center" rowspan="2">
                                    No. of new schemes launched during the month
                                </th>
                                <th colspan="3" align="center">
                                    Asset Under Management
                                </th>
                                <th align="center">
                                    Redemption
                                </th>
                                <th colspan="3" align="center">
                                    Asset Under Management
                                </th>
                            </tr>
                            <tr  class="ListtableHead">
                                <th>
                                    New schemes
                                </th>
                                <th>
                                    Existing schemes
                                </th>
                                <th>
                                    Total
                                </th>
                                <th>
                                    Total
                                </th>
                                <th>
                                    as on
                                    <asp:Label ID="LblNextDate" SkinID="labelsHead" runat="server" Text="Label"></asp:Label>
                                </th>
                                <th>
                                    as on
                                    <asp:Label ID="LblPrvDate" SkinID="labelsHead" runat="server" Text="Label"></asp:Label>
                                </th>
                                <th>
                                    Inflow/ Outflow
                                </th>
                            </tr>
                            <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                            <td width="15%" style="font-size:small" align="left">
                                   
                                   <%#Eval("Sector").ToString()%></a>                                        
                                    </td>
                                    <td width="15%" style="font-size:small" align="center">
                                    <%# Eval("New_Launched_Sales").ToString() != "" ? (GetSales(Int64.Parse(Eval("New_Launched_Sales").ToString()))).ToString() : "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    
                                    <%# Eval("New_Schemes_Sales").ToString() != "" ? (GetNew_Schemes(Int64.Parse(Eval("New_Schemes_Sales").ToString()))).ToString() : "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    
                                    <%#Eval("Exist_Schemes_Sales").ToString() != "" ?(GetExist_Schemes(Int64.Parse(Eval("Exist_Schemes_Sales").ToString()))).ToString() : "--"%>     
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    
                                    <%#Eval("TotalSales").ToString() != "" ?(GetTotalSales(Int64.Parse(Eval("TotalSales").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    
                                    <%# Eval("Redemption").ToString() != "" ?(GetRedemption(Int64.Parse(Eval("Redemption").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    
                                    <%# Eval("AUM1").ToString() != "" ?(GetAUM1(Int64.Parse(Eval("AUM1").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                     
                                    <%#Eval("AUMPREV").ToString() != "" ?(GetAUMPREV(Int64.Parse(Eval("AUMPREV").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                     
                                    <%# Eval("IOflow").ToString() != "" ? (GetIOflow(Int64.Parse(Eval("IOflow").ToString()))).ToString(): "--"%>    
                                    </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No data Found
                    </EmptyDataTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <td colspan="9" class="content">
                <strong>Private Sector & Joint Venture :</strong>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListView ID="LstMutDataGr2" runat="server" ItemPlaceholderID="itemPlaceHolder2">
                    <LayoutTemplate>
                        <table border="1" cellpadding="3" width="550px" style="border-collapse: collapse; border-color:Black;">
                            <asp:PlaceHolder ID="itemPlaceHolder2" runat="server" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                            <td width="15%" style="font-size:small" align="left"
                            class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                   <%#Eval("Sector").ToString()%>                                     
                                    </td>
                                    <td width="15%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    <%# Eval("New_Launched_Sales").ToString() != "" ? (GetSales(Int64.Parse(Eval("New_Launched_Sales").ToString()))).ToString() : "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                    <%# Eval("New_Schemes_Sales").ToString() != "" ? (GetNew_Schemes(Int64.Parse(Eval("New_Schemes_Sales").ToString()))).ToString() : "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                    <%#Eval("Exist_Schemes_Sales").ToString() != "" ?(GetExist_Schemes(Int64.Parse(Eval("Exist_Schemes_Sales").ToString()))).ToString() : "--"%>     
                                    </td>
                                    <td width="10%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                    <%#Eval("TotalSales").ToString() != "" ?(GetTotalSales(Int64.Parse(Eval("TotalSales").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                    <%# Eval("Redemption").ToString() != "" ?(GetRedemption(Int64.Parse(Eval("Redemption").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                    
                                    <%# Eval("AUM1").ToString() != "" ?(GetAUM1(Int64.Parse(Eval("AUM1").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                     
                                    <%#Eval("AUMPREV").ToString() != "" ?(GetAUMPREV(Int64.Parse(Eval("AUMPREV").ToString()))).ToString(): "--"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center"
                                    class='<%#Container.DataItemIndex==lastrow-1 ? "SubHeader":"nul"%>'>
                                     
                                    <%# Eval("IOflow").ToString() != "" ? (GetIOflow(Int64.Parse(Eval("IOflow").ToString()))).ToString(): "--"%>  
                                       
                                    </td>
                                   
                        </tr>
                    </ItemTemplate>
                   
                    <%--<InsertItemTemplate> eta on karle (InsertItemPosition=LastItem) listview te dite habe...
                        <tr class="AlternateRow">
                            <td width="15%" style="font-size:small" align="center">
                                    <strong>Grand Total (B+C+D)</strong>
                                                                            
                                    </td>
                                    <td width="15%" style="font-size:small" align="center">
                                   <strong><%#GetTotal()%></strong>   
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <strong><%#GetTotalNew_Schemes()%>  </strong>  
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                  <strong><%#GetTotalExist_Schemes()%></strong>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                   <strong><%#GetTotalTotalSales()%> </strong>  
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <strong><%#GetTotalRedemption()%> </strong>   
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <strong><%#GetTotalAUM1()%> </strong>
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <strong><%#GetTotalAUMPREV()%> </strong>   
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                   <strong><%#GetTotalIOflow()%></strong>   
                                    </td>   
                        </tr>
                    </InsertItemTemplate>--%>
                     <EmptyDataTemplate>
                        No data Found
                    </EmptyDataTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <td colspan="9" align="center">
                <a href="Aums.aspx?comID=<%=this.PropCompID %>">NATUREWISE ASSETS UNDER MANAGEMENT</a>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
