<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="amcSnapShot.aspx.cs" Inherits="iFrames.Pages.amcSnapShot" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        function tblShow() {
            document.getElementById("tblBtn").style.display = 'inline';
            document.getElementById("tblschem1").style.visibility = "visible";
            document.getElementById("tblschem1").align = 'center';
            document.getElementById("tblschem2").style.display = 'inline';
            document.getElementById("tblschem3").style.display = 'inline';
        }
        function tblhide() {
            document.getElementById("tblBtn").style.display = 'none';
            document.getElementById("tblschem1").style.display = 'none';
            document.getElementById("tblschem1").align = 'center';
            document.getElementById("tblschem2").style.display = 'none';
            document.getElementById("tblschem3").style.display = 'none';
        }
       
    </script>
</head>
<body>
    <form id="frmamcSnapshot" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
      <asp:UpdateProgress runat="server" id="PageUpdateProgress">
            <ProgressTemplate>
              <img src="../Images/ajax-loader.gif"/>
            </ProgressTemplate>
        </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <table width="550px" cellpadding="3" cellspacing="3" border="0">
        <tr class="mainHeader">
            <td>
                AMC SNAPSHOT
                <hr />
                <br style="line-height: 25px;" />
            </td>
        </tr>        
        <tr  class="content">
            <td valign="top" align="left">
                Brief on all Mutual Fund houses, key personnels, schemes managed by them, corpus
                under management and much more..
                <hr />
                <br style="line-height:15px;" />
            </td>
        </tr>
        
    <tr>
    <td>
     
    <table width="550px" cellpadding="3" cellspacing="3" border="0">
        <tr>
            <td align="center">
                <asp:DropDownList ID="DrpAmc" runat="server" AutoPostBack="true" Width="400px" 
                    onselectedindexchanged="DrpAmc_SelectedIndexChanged">
                </asp:DropDownList>                
            </td>
        </tr>
        <tr>
            <td align="center" >
                  <asp:Button ID="BtnGo" runat="server" Text="GO" onclick="BtnGo_Click" />             
            </td>
        </tr>
    <tr>
    <td>
    <table width="550px" cellpadding="3" cellspacing="3" id="tblBtn" style="display:none">
                <tr>
                    <td align="center">
                        <div id="Amcmenu">
                            <ul>
                                <li><a id="ancContactDetail"  runat="server" href="#">Contact Details</a></li>
                                <li><a id="ancPortfolio"  runat="server" href="#">Portfolio</a></li>
                                <li><a  id="ancDividendRecord"  runat="server" href="#">Dividend Record</a></li>
                                <li><a  id="ancFundManager"  runat="server" href="#">Fund Manager</a></li>
                                <li><a id="ancNews"  runat="server"  href="#">News</a></li>
                                <li><a id="ancNav"  runat="server" href="#">Nav</a></li>
                            </ul>
                        </div>                       
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblAmcName" runat="server" SkinID="lblHeader" ForeColor="Maroon"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblAmcDetal" runat="server"></asp:Label>
                    </td>
                </tr>                     
                </table>
    </td>
    </tr>
    <tr>
    <td>
        <table width="550px"  id="tblschem1" style="visibility:hidden;border-collapse:collapse; border-color:black;" border="1" cellpadding="3" cellspacing="3">
                <tr class="ListtableRow"><td align="left" width="60%">No.of schemes</td><td align="center" width="40%"><asp:Label ID="LblNosheme" runat="server" Text="0" ></asp:Label></td></tr>
                <tr class="AlternateRow"><td align="left" width="60%">No. of schemes including options</td><td align="center" width="40%"><asp:Label ID="LblNoshemeOpt" runat="server" Text="0" ></asp:Label></td></tr>
                <tr class="ListtableRow"><td align="left" width="60%">Equity Schemes </td><td align="center" width="40%"><asp:Label ID="LblEquity" runat="server" Text="0" ></asp:Label></td></tr>
                <tr class="AlternateRow"><td align="left" width="60%">Debt Schemes</td><td align="center" width="40%"><asp:Label ID="LblDebt" runat="server" Text="0" ></asp:Label></td></tr>
                <tr class="ListtableRow"><td align="left" width="60%">Short term debt Schemes</td><td align="center" width="40%"><asp:Label ID="LblShortDebt" runat="server" Text="0" ></asp:Label></td></tr>
                <tr class="AlternateRow"><td align="left" width="60%">Equity & Debt</td><td align="center" width="40%"><asp:Label ID="LblEqDebt" runat="server" Text="0" ></asp:Label></td></tr>
                <tr class="ListtableRow"><td align="left" width="60%">Money Market </td><td align="center" width="40%"><asp:Label ID="LblMoney" runat="server" Text="0" ></asp:Label></td></tr>
                <tr class="AlternateRow"><td align="left" width="60%">Gilt Fund</td><td align="center" width="40%"><asp:Label ID="LblGlit" runat="server" Text="0" ></asp:Label></td></tr>
                </table>
    </td>
    </tr>
    <tr>
    <td>
     <table width="550px" id="tblschem2" style="display:none;" cellpadding="3" cellspacing="3">
                    
                    <tr class="SubHeader"><td align="left">Corpus under management</td></tr>
                    <tr>
                        <td align="left" style="line-height:25px;">
                            <asp:Label ID="LblCormgmt" runat="server"></asp:Label>
                        </td>
                    </tr>                    
                    <tr><td align="left" class="SubHeader">Key Personnel</td></tr>
                    <tr><td align="left">
                        <asp:Label ID="LblKeyPerson" runat="server"></asp:Label>
                        </td>
                    </tr>                    
                    <tr><td align="left" class="SubHeader">Fund Managers</td></tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="LblFundmgr" runat="server"></asp:Label>
                        </td>
                    </tr>                    
                </table>
    </td>
    </tr>
      <tr>
      <td>
      <table width="550px" id="tblschem3" style="display:none"  cellpadding="3" cellspacing="3">
                <tr>
                    <td align="left" class="SubHeader">
                        Open Endede Schemes
                    </td>
                </tr>                
                <tr>
                    <td>
                        <asp:ListView ID="LstopenEndSchme" runat="server" 
                        ItemPlaceholderID="itemPlaceHolder1">
                         <layouttemplate>
                            <table border="1" cellpadding="3" cellspacing="3" width="550px" style="border-color:Black; border-collapse:collapse;">
                              <tr class="ListtableHead">
                               <th align="center" rowspan="2" ><asp:Label ID="lbl1" SkinID="lblHeader" runat="server">Scheme Name</asp:Label></th>
                               <th align="center" colspan="5"><asp:Label ID="Label1" SkinID="lblHeader" runat="server">Performance ( % Return )</asp:Label></th>
                               <th align="center" colspan="2"><asp:Label ID="Label2" SkinID="lblHeader" runat="server">Fund Size</asp:Label></th>
                               </tr>
                               <tr class="ListtableHead">
                               <th align="center"><asp:Label ID="lbl2" SkinID="lblHeader" runat="server">As On</asp:Label></th>
                               <th align="center"><asp:Label ID="lbl3" SkinID="lblHeader" runat="server">30 Days</asp:Label></th>
                               <th align="center"><asp:Label ID="lbl4" SkinID="lblHeader" runat="server">91 Days</asp:Label></th>
                               <th align="center"><asp:Label ID="lbl5" SkinID="lblHeader" runat="server">1 Year</asp:Label></th>
                               <th align="center"><asp:Label ID="lbl6" SkinID="lblHeader" runat="server">3 Year</asp:Label></th>
                               <th align="center"><asp:Label ID="lbl7" SkinID="lblHeader" runat="server">Rs. in Cr.</asp:Label></th>
                               <th align="center"><asp:Label ID="lbl8" SkinID="lblHeader" runat="server">As on</asp:Label></th>
                               <%--<th><asp:Label runat="server" SkinID="lblHeader" ID="lblPubon"><%#Convert.ToDateTime(Eval("PublishedOn")).ToString("dd MMM yyyy")%></asp:Label></th>--%>                         
                              </tr>
                          <asp:PlaceHolder ID="itemPlaceHolder1" runat="server" />
                         </table>
                        </layouttemplate>
                            <ItemTemplate>
                                <tr class='<%# Convert.ToBoolean(Container.DisplayIndex % 2) ? "AlternateRow" : "ListtableRow" %>'>
                                    <td width="30%" style="font-size:small" align="left">
                                    <a href="FundFactSheet1.aspx?sname=<%#Eval("sch_code")%>&comID=<%=this.PropCompID %>"><%#Eval("SchemeName").ToString()%></a>                                        
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("PerAsOn").ToString() != "" ? Convert.ToDateTime(Eval("PerAsOn").ToString()).ToString("MMM dd, yyyy") : "NA"%>   
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("30Days").ToString() != "" ? Eval("30Days"): "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("91Days").ToString() != "" ? Eval("91Days") : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("1Year").ToString() != "" ? Eval("1Year") : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("3Year").ToString() != "" ? Eval("3Year") : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("RsinCr").ToString() != "" ? Eval("RsinCr") : "NA"%>    
                                    </td>
                                    <td width="10%" style="font-size:small" align="center">
                                    <%#Eval("AsOn").ToString() == ""  ? "NA":Convert.ToDateTime(Eval("AsOn").ToString()).ToString("MMM dd,yyyy") %>     
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            Data not Found
                            </EmptyDataTemplate>
                        </asp:ListView> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataPager runat="server" ID="ContactsDataPager" PageSize="3" PagedControlID="LstopenEndSchme">
                            <Fields>
                                <asp:TemplatePagerField>
                                    <PagerTemplate>
                                        <span class="labelsHead">Page
                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                            of
                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
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
                    </td>
                </tr>
            </table>
      </td>
      </tr>      

                
               
            

                </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
