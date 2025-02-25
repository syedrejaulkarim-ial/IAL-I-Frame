<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundFactSheet3.aspx.cs"
    Inherits="iFrames.Pages.FundFactSheet3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="540px" cellpadding="3" cellspacing="3"> 
        <tr>
           <td align="justify" colspan="2" style="height: 55px;">
                    <span class="mainHeader">RISK RETURN - </span><SPAN class="SubHeader"><%=shortName%></SPAN>
                </td>
            </tr>           
            
            <tr>
                <td colspan="2">
                   <div id="menu">
                        <ul>
                            <li><a href='FundFactSheet1.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Fund Facts</a></li>
                            <li><a href='FundFactSheet2.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>NAV</a></li>
                            <li><a href='FundFactSheet3.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Risk Return</a></li>
                            <li><a href='FundFactSheet4.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>Portfolio</a></li>
                            <li><a href='FundFactSheet5.aspx?sch=<%=schCode%>&comID=<%=this.PropCompID %>'>News</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="SubHeader" style="height:55px;">                    
                    <strong>Risk & Return</strong>
                    <br style="line-height: 25px;" />
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table cellpadding="2" width="100%" cellspacing="2" border="1" style="border-collapse: collapse;">
                        <tr class="labelsHead">
                            <td colspan="7" align="left">                                
                                    <%=DateHeader%>                                    
                            </td>
                        </tr>
                        <tr class="ListtableHead">
                            <td align="center">
                               1 Month
                            </td>
                            <td align="center">
                                3 Months
                            </td>
                            <td align="center">
                               6 Months
                            </td>
                            <td align="center">
                                1 Year
                            </td>
                            <td align="center">
                                3 Years
                            </td>
                            <td align="center">
                                5 Years
                            </td>
                            <td align="center">
                                Since Inception
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td align="center">
                                <asp:Label ID="lbl30days" runat="server" />
                                
                            </td>
                            <td align="center">
                                <asp:Label ID="lbl91days" runat="server" />
                                
                            </td>
                            <td align="center">
                            <asp:Label ID="lbl182days" runat="server" />
                                
                            </td>
                            <td align="center">
                            <asp:Label ID="lbl1yr" runat="server" />
                                
                            </td>
                            <td align="center">
                            <asp:Label ID="lbl3yrs" runat="server" />
                                
                            </td>
                            <td align="center">
                            <asp:Label ID="lbl5yrs" runat="server"/>
                                
                            </td>
                            <td align="center">
                            <asp:Label ID="lblSinceInsp" runat="server" />
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="SubHeader" style="height:55px;">
                    Risk
                    <br style="line-height: 5px;" />
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="left"  width="48%">
                    <table cellpadding="2" cellspacing="2" border="1"  width="100%" style="border-collapse: collapse;">
                        <tr class="ListtableRow">
                            <td>
                                <strong>Mean</strong>
                            </td>
                            <td align="right">
                            <asp:Label ID="lblmean" runat="server" />
                                
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td>
                                <strong>Standard Deviation</strong>
                            </td>
                            <td align="right">
                            <asp:Label ID="lblStDiv" runat="server"/>
                                
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td>
                                <strong>Sharpe</strong>
                            </td>
                            <td align="right">
                            <asp:Label ID="lblSharpe" runat="server" />
                                
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td>
                                <strong>Beta</strong>
                            </td>
                            <td align="right">
                            <asp:Label ID="lblBeta" runat="server"/>
                                
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" width="48%">
                    <table cellpadding="2" cellspacing="2" border="1" width="100%" style="border-collapse: collapse;">
                        <tr class="ListtableRow">
                            <td align="left">
                                <strong>Treynor</strong>
                            </td>
                            <td>
                            <asp:Label ID="lblTreynor" runat="server" />
                                
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td align="left">
                                <strong>Sortino</strong>
                            </td>
                            <td>
                            <asp:Label ID="lblSortino" runat="server" />
                                
                            </td>
                        </tr>
                        <tr class="ListtableRow">
                            <td align="left">
                                <strong>Correlation</strong>
                            </td>
                            <td>
                            <asp:Label ID="lblCorrelation" runat="server" />
                                
                            </td>
                        </tr>
                        <tr class="AlternateRow">
                            <td align="left">
                                <strong>Fama</strong>
                            </td>
                            <td>
                            <asp:Label ID="lblFama" runat="server" />
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
