<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Topper.aspx.cs" Inherits="iFrames.Pages.Topper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="550px" cellpadding="2" cellspacing="2">
            <tr class="content">
                <td>
                    'Toppers & Laggards' gives you a list of all schemes under a particular category
                        with their performance in different periods. You can sort these schemes on a particular
                        period to know about the top performing scheme & the worst performing scheme.                    
                </td>
            </tr>
            <tr class="content">
                <td>
                    <strong>Click on any of the category mentioned below.</strong>
                    <br style="line-height: 25px;" />
                    <ul>
                        <li><a href="TopperPages.aspx?page=OpenTaxSect&comID=<%=this.PropCompID %>" target="_blank">All Open Ended Equity Schemes (Including
                            tax saving &amp; sector funds)</a></li>
                        <li><a href="TopperPages.aspx?page=OpenTaxSectOther&comID=<%=this.PropCompID %>" >All Open Ended Equity Schemes
                            (Excluding tax saving &amp; sector funds)</a></li>
                        <li><a href="TopperPages.aspx?page=SectSpeSchAll&comID=<%=this.PropCompID %>" >Sector Specific Schemes (All)</a></li>
                        <li><a href="TopperPages.aspx?page=InfoScheme&comID=<%=this.PropCompID %>" >Infotech Schemes</a></li>
                        <li><a href="TopperPages.aspx?page=PharmaSch&comID=<%=this.PropCompID %>" >Pharma Schemes </a></li>
                        <li><a href="TopperPages.aspx?page=FmcgSch&comID=<%=this.PropCompID %>" >FMCG Schemes</a></li>
                        <li><a href="TopperPages.aspx?page=OpenTax&comID=<%=this.PropCompID %>" >Open Ended Tax Planning Schemes</a></li>
                        <li><a href="TopperPages.aspx?page=OpenBalance&comID=<%=this.PropCompID %>" >Open Ended Balanced Schemes </a>
                        </li>
                        <li><a href="TopperPages.aspx?page=DebtShort&comID=<%=this.PropCompID %>" >Open Ended Short Term Debt Schemes
                        </a></li>
                        <li><a href="TopperPages.aspx?page=DebtSch&comID=<%=this.PropCompID %>" >Open Ended Debt Schemes </a></li>
                        <li><a href="TopperPages.aspx?page=LiquidSch&comID=<%=this.PropCompID %>" >Liquid Schemes </a></li>
                        <li><a href="TopperPages.aspx?page=GiltSch&comID=<%=this.PropCompID %>" >Gilt Schemes</li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
