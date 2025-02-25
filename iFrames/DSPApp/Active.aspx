<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Active.aspx.cs" Inherits="iFrames.DSPApp.Active" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Active/Inactive Login</title>
    <script type='text/javascript' src="js/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="assets/img/favicon.png">
    <!--    <link rel="stylesheet" type="text/css" href="css/style.css"/>
    <link rel="stylesheet" type="text/css" href="assets/lib/jquery.nanoscroller/css/nanoscroller.css"/>-->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link type="text/css" href="css/style.css" rel="stylesheet">
    <link href="css/simple-line-icons.css" rel="stylesheet">
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet">
    <%-- <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>--%>
    <script type="text/javascript" language="javascript">
        function UpdateAction(obj, userid) {
            var isChecked = obj.checked;
            if (isChecked) {
                if (confirm('Do you want to activate?')) {
                    //alert('activated');
                    UpdateUserStatus(userid, isChecked, 'activated');
                }
                else
                    obj.checked = !isChecked;
            }
            else {
                if (confirm('Do you want to deactivate?')) {
                    //alert('deactivated');
                    UpdateUserStatus(userid, isChecked, 'deactivated');
                }
                else
                    obj.checked = !isChecked;
            }
        }

        function UpdateUserStatus(UserId, status, msg) {
            try {
                var dataToPush = JSON.stringify({
                    UserId: UserId,
                    Status: status
                });

                $.ajax({
                    cache: false,
                    data: dataToPush,
                    dataType: "json",
                    url: 'Active.aspx/UpdateUserStatus',
                    type: 'POST',
                    asynchronus: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var retval = data.d;
                        if (retval)
                            alert('User ' + msg + ' successfully.');

                        return false;
                    },
                    error: function (data, e) {
                        alert("err1 " + data.responseText);
                        return false;
                    }
                });
            }
            catch (e) {
                alert(e.Message);
            }
        }       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="am-wrapper">
        <nav class="navbar navbar-default navbar-fixed-top am-top-header">
        <div class="container-fluid">
          <div class="navbar-header">
            <div class="page-title"><span>Dashboard</span></div><a href="#" class="am-toggle-left-sidebar navbar-toggle collapsed"><span class="icon-bar"><span></span><span></span><span></span></span></a><a href="index.php" class="navbar-brand"></a>
          </div><a href="#" class="am-toggle-right-sidebar"><span class="icon s7-menu2"></span></a><a href="#" data-toggle="collapse" data-target="#am-navbar-collapse" class="am-toggle-top-header-menu collapsed"> <i class="fa fa-angle-down"></i></a>
          <div id="am-navbar-collapse" class="collapse navbar-collapse">
            <ul class="nav navbar-nav navbar-right am-user-nav">
              <li class="dropdown">
              <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle"><%= Session["EmailId"] %>
              
             <i class="fa fa-angle-down"></i></a>
                <ul role="menu" class="dropdown-menu">
                	 <li><a href="ForgetPassword.aspx"> <span class="icon-key"></span>Reset Password</a></li>
                 
                  <li><a href="Login.aspx?param=logout"> <span class="icon-logout"></span>Sign Out</a></li>
                </ul>
              </li>
            </ul>
            <!--<ul class="nav navbar-nav am-nav-right">
              <li><a href="#">Home</a></li>
              <li><a href="#">About</a></li>
              <li class="dropdown"><a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle">Services <span class="angle-down s7-angle-down"></span></a>
                <ul role="menu" class="dropdown-menu">
                  <li><a href="#">UI Consulting</a></li>
                  <li><a href="#">Web Development</a></li>
                  <li><a href="#">Database Management</a></li>
                  <li><a href="#">Seo Improvement</a></li>
                </ul>
              </li>
              <li><a href="#">Support</a></li>
            </ul>-->
            
          </div>
        </div>
      </nav>
        <div class="am-left-sidebar">
            <div class="content">
                <div class="am-logo">
                </div>
                <ul class="sidebar-elements">
                    <li id="liUserMngmnt" runat="server" class="parent"><a href="#"><i class="icon-user"></i><span>User Management</span></a>
                        <ul class="sub-menu">
                            <li style="list-style-type: none;"><a href="CreateUser.aspx">User Creation</a> </li>
                            <li class="active" style="list-style-type: none;"><a href="Active.aspx">Active/Inactive
                                Login</a> </li>
                            <li style="list-style-type: none;"><a href="LoginHistory.aspx">Login History</a> </li>
                        </ul>
                    </li>
                    <li class="parent"><a href="#"><i class="icon-magnifier"></i><span>Return Analysis</span></a>
                        <ul class="sub-menu">
                            <li id="liUploadExl" runat="server" style="list-style-type: none;"><a href="upload.aspx">Upload Excel File </a></li>
                            <li style="list-style-type: none;"><a href="viewreport.aspx">View Report</a> </li>
                        </ul>
                    </li>
                </ul>
                <!--Sidebar bottom content-->
            </div>
        </div>
        <div id="dvContent" runat="server" class="am-content">
            <div class="page-head">
                <h3>
                    Active/Inactive Login</h3>
                <ol class="breadcrumb">
                    <li><a href="#">User Management</a></li>
                    <li class="active.html">Active/Inactive Login</li>
                </ol>
            </div>
            <div class="main-content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default panel-heading-fullwidth panel-borders">
                            <div class="panel-heading">
                                <h3>
                                    Inactive Login</h3>
                            </div>
                            <div class="panel-body">
                                <div style="border-radius: 0px;" class="form-horizontal group-border-dashed">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Select User</label>
                                    <div class="col-sm-5">
                                        <asp:DropDownList ID="ddlUser" class="form-control" runat="server">
                                        </asp:DropDownList>
                                        <%-- <select class="form-control">
                                            <option>abc@icraonline.com</option>
                                            <option>xxxxxyyyyyzzzz@icraonline.com</option>
                                        </select>--%>
                                    </div>
                                    <div class="col-sm-4"></div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-3 "> </div>
                                    <div class="col-sm-6">
                                        <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnReset" class="btn btn-primary" runat="server" Text="Reset" OnClick="btnReset_Click" />
                                    </div>
                                    <div class="col-sm-3"></div>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" class="row" runat="server">
                    <ContentTemplate>
                       <%-- <div class="row">--%>
                            <!--Responsive table-->
                            <div class="col-sm-12">
                                <div class="widget widget-fullwidth widget-small">
                                    <div class="table-responsive">
                                        <%--<asp:Repeater ID="rptUser" Visible="false" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-fw-widget table-hover">
                                            <thead>
                                                <tr>
                                                    <th width="60%">
                                                        Login
                                                    </th>
                                                    <th width="25%">
                                                        Last Login
                                                    </th>
                                                    <th width="10%">
                                                        Action
                                                    </th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody class="no-border-x">
                                            <tr>
                                                <td class="user-avatar">
                                                    <span class="icon-user"></span>
                                                    <%# DataBinder.Eval(Container, "DataItem.Email_Id")%>
                                                </td>
                                                <td>
                                                    <%# DataBinder.Eval(Container, "DataItem.Last_Login_Date", "{0:dd-MMM-yyyy hh:mm tt}")%>
                                                </td>
                                                <td>
                                                    <div class="switch-button switch-button-success switch-button-sm">
                                                        <input type="checkbox" value="<%# DataBinder.Eval(Container,"DataItem.IsActive") %>"
                                                            checked="<%# DataBinder.Eval(Container,"DataItem.IsActive") %>" onclick="return UpdateAction(this,'<%#DataBinder.Eval(Container, "DataItem.User_Id") %>');"
                                                            name="<%# DataBinder.Eval(Container, "DataItem.User_Id")%>" id="<%# DataBinder.Eval(Container, "DataItem.User_Id")%>" />
                                                        <span>
                                                            <label for="<%# DataBinder.Eval(Container, "DataItem.User_Id")%>">
                                                            </label>
                                                        </span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>--%>
                                        <asp:GridView ID="gvwUser" CssClass="table table-striped table-fw-widget table-hover"
                                            AutoGenerateColumns="false" GridLines="None" AllowPaging="true" PageSize="5"
                                            BorderWidth="0px" runat="server" OnPageIndexChanging="gvwUser_PageIndexChanging"
                                            OnRowDataBound="gvwUser_RowDataBound" Width="1000">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnUserId" runat="server" Value='<%# Eval("User_Id")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Login</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="user-avatar">
                                                            <span class="icon-user"></span>
                                                            <%# Eval("Email_Id")%>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="350" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Last Login</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# Eval("Last_Login_Date", "{0:dd-MMM-yyyy hh:mm tt}")%>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="200" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Action</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="switch-button switch-button-success switch-button-sm">
                                                            <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Eval("IsActive")%>' />
                                                            <%--<input type="checkbox" value="<%# Eval("IsActive") %>"
                                                                checked="<%# Eval("IsActive") %>" onclick="return UpdateAction(this,'<%#Eval("User_Id") %>');"
                                                                id="<%# Eval("User_Id")%>" />--%>
                                                            <span>
                                                                <label runat="server" id="lblStatus">
                                                                </label>
                                                            </span>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="150" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />
                                            <PagerStyle Font-Size="13" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        <%--</div>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/lib/jquery.nanoscroller/javascripts/jquery.nanoscroller.min.js"
        type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="assets/lib/theme-switcher/theme-switcher.min.js" type="text/javascript"></script>
     <script src="js/jquery.nanoscroller.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //initialize the javascript
            App.init();
        });
      
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            App.livePreview();
        });
      
    </script>
    <script type="text/javascript">
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
      m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-68396117-1', 'auto');
        ga('send', 'pageview'); 
    </script>
</body>
</html>
