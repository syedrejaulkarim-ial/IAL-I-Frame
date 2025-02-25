<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="ViewIndexValues.aspx.cs"
    Inherits="iFrames.Canara.ViewIndexValues" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Uploaded Index</title>
    <link type="text/css" rel="Stylesheet" href="../Chart/css/Chart.css" />
    <link type="text/css" href="css/style.css" rel="stylesheet" />
    <link href="css/simple-line-icons.css" rel="stylesheet" />
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/datepicker.css" rel="stylesheet" />
    <link href="css/multiSelectDropdown/multiple-select.css" rel="stylesheet" />
    <link href="../Styles/jquery-ui-1.8.14.custom2.css" rel="stylesheet" type="text/css" />
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet" />
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-1.10.2.js"></script>

    <%--<script type="text/javascript" language="javascript" src="../Scripts/jquery-ui.min-1.10.2.js"></script>--%>
    <%--<link type="text/css" rel="Stylesheet" href="../Chart/css/Chart.css" />--%>
    <%--<link href="../Styles/jquery-ui-1.8.14.custom2.css" rel="stylesheet" type="text/css" />--%>
    <%--<script type="text/javascript" language="javascript" src="../Scripts/jquery-1.10.2.js"></script>--%>
    <script type="text/javascript" language="javascript" src="../Scripts/jquery-ui.min-1.10.2.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#txtStarttDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
            $("#txtEndDate").datepicker({
                dateFormat: 'dd-MM-yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

        });

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
         .nav_title {
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
                        <div class="page-title"><span>Dashboard</span></div>
                        <a href="#" class="am-toggle-left-sidebar navbar-toggle collapsed"><span class="icon-bar"><span></span><span></span><span></span></span></a>
                        <%--<a href="#" class="navbar-brand"></a>--%>
                        <h1 class="nav_title">Canara Mutual Fund</h1>
                    </div>
                    <a href="#" class="am-toggle-right-sidebar"><span class="icon s7-menu2"></span></a><a href="#" data-toggle="collapse" data-target="#am-navbar-collapse" class="am-toggle-top-header-menu collapsed"><i class="fa fa-angle-down"></i></a>
                    <div id="am-navbar-collapse" class="collapse navbar-collapse">
                        <ul class="nav navbar-nav navbar-right am-user-nav">
                            <li class="dropdown">
                                <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle"><%= Session["EmailId"] %>
                                    <i class="fa fa-angle-down"></i></a>
                                <ul role="menu" class="dropdown-menu">
                                    <%--<li><a href="ForgetPassword.aspx"> <span class="icon-key"></span>Reset Password</a></li>--%>
                                    <li><a href="Login.aspx?param=logout"><span class="icon-logout"></span>Sign Out</a></li>
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
                        <li class="active" style="list-style-type: none;"><a href="CanaraUploadExcel.aspx">Index Record</a> </li>
                    </ul>
                    <!--Sidebar bottom content-->
                </div>
            </div>
            <div class="am-content">
                <div class="page-head">
                    <h3>Uploaded Index</h3>
                </div>
                <div class="main-content">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <span>Uploaded Index</span>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <asp:Panel ID="Panel1" runat="server" GroupingText="View Uploaded Index Values" Font-Bold="False" HorizontalAlign="Center">
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>  
                                    <div class="col-md-12">
                                        <div class="col-md-3">Index:</div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlIndex" runat="server" CssClass="form-control"></asp:DropDownList><br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please choose index." ControlToValidate="ddlIndex" Display="Dynamic" InitialValue="0" ValidationGroup="CheckIndex"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3 xs-mt-10"></div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-3">Start Date:</div>
                                        <div class="col-md-6">
                                             <asp:TextBox ID="txtStarttDate" runat="server" CssClass="form-control" Height="20px" Text="DD-MMM-YYYY" Style="font-size: 12px;"> </asp:TextBox>
                                        </div>
                                        <div class="col-md-3 xs-mt-10"></div>
                                    </div>
                                    <div class="col-md-12" style="margin: 10px 0px;">
                                        <div class="col-md-3">End Date:</div>
                                        <div class="col-md-6">
                                             <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" Height="20px" Text="DD-MMM-YYYY" Style="font-size: 12px;"> </asp:TextBox>
                                        </div>
                                        <div class="col-md-3 xs-mt-10"></div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-3 xs-mt-10"></div>
                                        <div class="col-md-2 xs-mt-10" style="text-align: left;">
                                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="btn btn-primary" ValidationGroup="CheckIndex" />
                                        </div>
                                        <div class="col-md-7 xs-mt-10"></div>
                                    </div>
                                </asp:Panel>
                            </div>
                            
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlList" runat="server" GroupingText="View Uploaded Index Values"
                                            HorizontalAlign="Center"  Width="750" Visible="false">
                                            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="gvIndexValue" runat="server" Width="80%" AutoGenerateColumns="False"
                                                            Font-Size="Small" BorderWidth="1px" BackColor="White" CellPadding="4" BorderStyle="None"
                                                            BorderColor="#3366CC" GridLines="Both" OnPageIndexChanging="gvCompanyShare_PageIndexChanging">
                                                            <PagerStyle ForeColor="White" HorizontalAlign="Right" BackColor="#007AC2" CssClass="pager">
                                                            </PagerStyle>
                                                            <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#007AC2" Font-Names="Verdana"
                                                                Font-Size="9"></HeaderStyle>
                                                            <FooterStyle ForeColor="#003399" BackColor="#84AFC8"></FooterStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Index Id" HeaderStyle-Width="10%" Visible="false">
                                                                    <ItemTemplate>
                                                                        <%#DataBinder.Eval(Container.DataItem, "CUSTOM_INDEX_ID")%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Record date" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <%#FomatDate(DataBinder.Eval(Container.DataItem, "RECORD_DATE"))%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="5%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Index Value" HeaderStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <%#DataBinder.Eval(Container.DataItem, "CUSTOM_INDEX_VALUE")%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Font-Names="verdana" Font-Size="9pt" Width="10%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>




    <div>
       <%-- <center>
            <table width="50%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="lnlLogout" runat="server" Visible="false" Font-Bold="true" ForeColor="#067DCD"
                            OnClick="lnlLogout_Click">logout</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </center>--%>
      <%--  <center>
            
        </center>--%>
    </div>
    </form>
</body>
</html>
