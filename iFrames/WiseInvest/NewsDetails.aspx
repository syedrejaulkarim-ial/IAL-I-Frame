<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="NewsDetails.aspx.cs" Inherits="iFrames.WiseInvest.NewsDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>News Details</title>
    <link href="css/template_css.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/navigation.js" type="text/javascript"></script>
</head>
<body>
    <table border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
        <tr>
            <td>
                <%--<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="2%" align="center">
                                <img src="img/news_arw2.png" />
                            </td>
                            <td width="98%" align="left">
                                <strong>&nbsp;<asp:Label class="componentheading" ID="newsHeader" runat="server"></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td width="2%" align="center">
                                &nbsp;
                            </td>
                            <td width="98%" align="left">
                                &nbsp;
                                <div align="justify">
                                    <asp:Label ID="detailNew" runat="server"></asp:Label></div>
                                <br />
                                <input name="" type="button" value="Back" class="back" onclick="JavaScript:history.go(-1);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>--%>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="2%" align="center">
                            &nbsp;
                        </td>
                        <td width="98%" align="left" class="text_border">
                            <strong>&nbsp;<span class="news_header1"><asp:Label ID="newsHeader" runat="server"></asp:Label>
                            </span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td width="2%" align="center">
                            &nbsp;
                        </td>
                        <td width="98%" align="left">
                            &nbsp;
                            <div align="justify">
                                <asp:Label ID="detailNew" runat="server"></asp:Label>
                            </div>
                            <br />
                            <input type="button" value="&lt;&lt; back" class="back" onclick="JavaScript:history.go(-1);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="disclaimerh">
                            Disclaimer
                        </td>
                    </tr>
                    <tr>
                        <td class="disclaimer">
                            Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing.  Past performance of the schemes do not indicate the future performance.
                        </td>
                    </tr>
                    <tr>
                        <td class="text" align="right">
                            <span style="text-align: right" class="rslt_text1">Developed and Maintained by:<a
                                class="text" href="https://www.icraanalytics.com" target="_blank"> ICRA Analytics Ltd </a>
                            </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
