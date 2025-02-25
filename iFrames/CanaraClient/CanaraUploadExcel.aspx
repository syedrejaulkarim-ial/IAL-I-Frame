<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CanaraUploadExcel.aspx.cs" Inherits="iFrames.CanaraClient.CanaraUploadExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index Repord</title>
    <link type="text/css" rel="Stylesheet" href="../Chart/css/Chart.css" />
    <link type="text/css" href="css/style.css" rel="stylesheet">
    <link href="../Styles/jquery-ui-1.8.14.custom2.css" rel="stylesheet" type="text/css" />
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet">
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui.min-1.10.2.js"></script>

    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txttDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
        });

    </script>
    <script type="text/javascript" language="javascript">

        function Validate() {

            var listBoxSelection = document.getElementById('<%=ddlIndex.ClientID%>').value;
            if (listBoxSelection == 0) {
                alert("Please Choose Index");
                return false;
            }


            if (document.getElementById('<%=txttDate.ClientID%>').value == "" || document.getElementById('<%=txttDate.ClientID%>').value == "DD-MMM-YYYY") {
                alert("Date cannot be Blank");
                document.getElementById('<%=txttDate.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%=txtIndexValue.ClientID%>').value == "") {
                alert("Index Value cannot be Blank");
                document.getElementById('<%=txtIndexValue.ClientID%>').focus();
                return false;
            }

            var str = document.getElementById('<%=txtIndexValue.ClientID%>').value;
            if (isNaN(str) || str.indexOf(".") < 0) {
                alert(" Index value should be two fractional positions required");
                return false;
            }
            else {
                var check = 100 * str == parseInt(100 * str, 10);
                if (!check) {
                    alert(" Index value should be two fractional positions required");
                    document.getElementById('<%=txtIndexValue.ClientID%>').value = "";
                    document.getElementById('<%=txtIndexValue.ClientID%>').focus();
                }
                return check;
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;

            if ((keycode >= 46 && keycode <= 58) || keycode == 8 || keycode == 9) {

                if ((keycode >= 46 && keycode <= 58)) {

                }
                return true;
            }
            return false;
        }
    </script>
    <style type="text/css">
        .pagebutton
        {
            background-color: #067DCD;
            color: White;
            font-weight: bold;
            text-align: center;
            border: 1;
            border-width: 1;
            cursor: pointer;
            font-family: Verdana;
        }
        .nav_title{
            color: white;
            /*text-transform: uppercase;*/
            font-weight: 600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="am-wrapper">
            <nav class="navbar navbar-default navbar-fixed-top am-top-header">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <div class="page-title"><span>Dashboard</span></div><a href="#" class="am-toggle-left-sidebar navbar-toggle collapsed"><span class="icon-bar"><span></span><span></span><span></span></span></a>
                                <%--<a href="#" class="navbar-brand"></a>--%>
                        <h1 class="nav_title">Canara Mutual Fund</h1>
                    </div>
                        <a href="#" class="am-toggle-right-sidebar"><span class="icon s7-menu2"></span></a><a href="#" data-toggle="collapse" data-target="#am-navbar-collapse" class="am-toggle-top-header-menu collapsed"> <i class="fa fa-angle-down"></i></a>
                    <div id="am-navbar-collapse" class="collapse navbar-collapse">
                        <ul class="nav navbar-nav navbar-right am-user-nav">
                            <li class="dropdown">
                            <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle"><%= Session["EmailId"] %>
                                <i class="fa fa-angle-down"></i></a>
                            <ul role="menu" class="dropdown-menu">
    	                        <%--<li><a href="ForgetPassword.aspx"> <span class="icon-key"></span>Reset Password</a></li>--%>
                                <li><a href="Login.aspx?param=logout"> <span class="icon-logout"></span>Sign Out</a></li>
                            </ul>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <div class="am-left-sidebar">
                <div class="content">
                    <div class="am-logo">
                    </div>
                    <ul class="sidebar-elements">
                        <li id="liUserMngmnt" runat="server" class="parent"><a href="#"><i class="icon-user"></i></a></li>
                        <li style="list-style-type: none;"><a href="ExcelFileUpload.aspx">Static Record</a> </li>
                        <li class="active" style="list-style-type: none;"><a href="#">Index Record</a> </li>
                    </ul>
                    <!--Sidebar bottom content-->
                </div>
            </div>
            <div class="am-content">
                <div class="page-head">
                    <h3>Index Record</h3>
                </div>
                <div class="main-content">
                    <center>
                        <asp:Panel ID="pnlUpload" runat="server" GroupingText="Upload Excel" Width="450"
                            Style="color: #067DCD; font-family: vredana; font-size: 14px;">
                            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="font-family: verdana; font-size: 12px">
                                        Index:
                                    </td>
                                    <td style="width: 20px;" align="left">
                                        <asp:DropDownList ID="ddlIndex" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="font-family: verdana; font-size: 12px">
                                        Date:
                                    </td>
                                    <td style="width: 20px;" align="left">
                                        <asp:TextBox ID="txttDate" runat="server" CssClass="input2" Width="185px" Height="20px"
                                            placeholder="DD-MMM-YYYY"> </asp:TextBox>
                                        <br />
                                        <span style="font-size: 12px; padding-right: 100px;">(DD-MM-YYYY)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="font-family: verdana; font-size: 12px">
                                        Index Value
                                    </td>
                                    <td style="width: 20px;" align="left">
                                        <asp:TextBox ID="txtIndexValue" runat="server" onkeypress="return isNumber(event)"
                                            Width="185px" Height="20px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="font-family: verdana; font-size: 12px;">
                                        Upload:
                                    </td>
                                    <td style="width: 20px;">
                                        <asp:FileUpload ID="FUExcell" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="pagebutton"
                                                            ValidationGroup="SaveIndex" OnClientClick="return Validate();" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                                                            CssClass="pagebutton" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <a href="ViewIndexValues.aspx" style="font-family: verdana; font-size: 12px; text-decoration: none;
                                            color: #067DCD;">View Uploaded Index Value</a>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div style="height: 5px;">
                        </div>
                        <asp:Panel ID="pnlInstruction" HorizontalAlign="Center" runat="server">
                            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                                <tr>
                                    <td style="width: 52%; font-family: verdana; font-size: 12px; color: #067DCD;" align="right">
                                        Excel Format
                                    </td>
                                    <td align="left" style="font-family: verdana; font-size: 12px; color: #067DCD;">
                                        Instruction
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="font-family: Verdana; font-size: 12px;">
                                        <div>
                                            <table border="1" cellpadding="2" cellspacing="4">
                                                <tr>
                                                    <td>
                                                        Index_Id
                                                    </td>
                                                    <td>
                                                        Date(DD/MM/YYYY)
                                                    </td>
                                                    <td>
                                                        Index Value
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        value
                                                    </td>
                                                    <td>
                                                        value
                                                    </td>
                                                    <td>
                                                        value
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        value
                                                    </td>
                                                    <td>
                                                        value
                                                    </td>
                                                    <td>
                                                        value
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        **Sheet Name should be Upload Index
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td style="font-family: Verdana; font-size: 12px;">
                                        <div>
                                            <table border="1" cellpadding="2" cellspacing="4">
                                                <tr>
                                                    <td>
                                                        index id
                                                    </td>
                                                    <td>
                                                        Index Name
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        1
                                                    </td>
                                                    <td>
                                                        I-Sec Composite
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        2
                                                    </td>
                                                    <td>
                                                        MSCI World Index
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        3
                                                    </td>
                                                    <td>
                                                        MSCI Emerging Market Index
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                 </div>
             </div>
            
        </div>
    </form>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
</body>
</html>
