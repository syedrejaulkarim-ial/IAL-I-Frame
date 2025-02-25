<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="FundDetails.aspx.cs" Inherits="iFrames.WiseInvest.FundDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fund Details</title>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="710" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="fund_details">
                                        </td>
                                        <td class="fund_details_txt">
                                            Fund Details
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="compare_innerbg">
                                    <tr>
                                        <td class="scheme">
                                            Scheme
                                        </td>
                                        <td class="schemename">
                                            <asp:Label ID="LabelSchemeName" runat="server" Text="DSP"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="compare_leftcol">
                                            Portfolio Date
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelPortfolioDate" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Category Name
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelCategory" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Fund Manager
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelFundManager" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            ICRON Rank (1 Year)
                                        </td>
                                        <td class="compare_rightcol">
                                            <div runat="server" id="divSpanrank">
                                                <img src="img/star.png" width="12" height="12" /></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="comparefund_heading">
                                            Asset Allocation
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Equity
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelEquity" runat="server" Text="%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Debt
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelDebt" runat="server" Text="%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Others
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelOthers" runat="server" Text="%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="comparefund_heading">
                                            Market Cap
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Large Cap
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelLCap" runat="server" Text="%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Mid Cap
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelMCap" runat="server" Text="%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Smalll Cap
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelSCap" runat="server" Text="%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="comparefund_heading_alt1">
                                            Top 5 Holdings
                                        </td>
                                        <td class="comparefund_heading_alt2">
                                            <asp:Label ID="LabelTop5HoldingsTotal" runat="server" Text="%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTop1Hold" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTop1HoldPer" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTop2Hold" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTop2HoldPer" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTop3Hold" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTop3HoldPer" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTop4Hold" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTop4HoldPer" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTop5Hold" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTop5HoldPer" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="comparefund_heading_alt1">
                                            Top 3 Sectors
                                        </td>
                                        <td class="comparefund_heading_alt2">
                                            <asp:Label ID="LabelTop3SecPer" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTopSec1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTopSec1Per" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTopSec2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTopSec2Per" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            <asp:Label ID="LabelTopSec3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelTopSec3Per" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol1">
                                            Portfolio PE (Times)
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelPortfolioPE" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol1">
                                            Average Maturity
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelAverageMat" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol1">
                                            YTM
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelYTM" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol1">
                                            Expense Ratio
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelExpenseRatio" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol1">
                                            Exit load
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelExitload" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="comparefund_heading">
                                            Performance
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            3 Months Return
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelPer3Mon" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            6 Months Return
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelPer6Mon" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            1 Year Return
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelPer1Year" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            3 Years Return
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelPer3Year" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            5 Years Return
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelPer5Year" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="comparefund_heading">
                                            SIP Analysis (Installment Amount Rs 5000 per month for 3 years)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Invested Amount (Rs.)
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelInvestedAmt" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Value of SIP (Rs.)
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelSIPvalue" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Return
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelReturnCAGR" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="comparefund_heading">
                                            Risk Measures
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Sharpe
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelSharpe" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Sortino
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelSortino" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="compare_leftcol">
                                            Standard Deviation
                                        </td>
                                        <td class="compare_rightcol">
                                            <asp:Label ID="LabelStandardDev" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="comparefund_heading1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="disclaimerh">
                                            Disclaimer
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="disclaimer">
                                            <%-- Mutual funds like securities investments are subject to market and other risks.
                                            As with any investments in securities, the NAV of units can go up or down depending
                                            on the factors and forces affecting capital markets.--%>
                                            Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing.  Past performance of the schemes do not indicate the future performance.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="text" align="right">
                                            <span style="text-align: right" class="rslt_text1">Developed and Maintained by:<a
                                                class="text" href="https://www.icraanalytics.com" target="_blank"> ICRA Analytics Ltd </a>
                                            </span>
                                        </td>
                                    </tr>                                   
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
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
