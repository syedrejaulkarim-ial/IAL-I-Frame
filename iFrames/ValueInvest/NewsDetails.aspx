<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="NewsDetails.aspx.cs" Inherits="iFrames.ValueInvest.NewsDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>News Details</title>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/navigation.js" type="text/javascript"></script>
</head>
<body>
    
<table border="0" cellSpacing="0" cellPadding="0" width="900" align="left" class="main-content">
  <tr>
    <td>
    	<table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
  <tr>
    <td align="center">&nbsp;</td>
    <td align="left" class="value_border"><strong>&nbsp;<span class="value_news_header1">
        <asp:Label ID="newsHeader" runat="server"></asp:Label></span></strong></td>
  </tr>
  <tr>
    <td align="center">&nbsp;</td>
    <td align="left">&nbsp;
      <div align="justify" class="value_news"><asp:Label ID="detailNew" runat="server"></asp:Label></div>
      <br />
          <%--<input type="button" value="Back" class="value_go" />--%>
        <input type="button" value="Back" class="value_go" onclick="JavaScript: history.go(-1);" />
    </td>
  </tr>
</table>

    </td>
  </tr>
</table>
</body>
</html>

