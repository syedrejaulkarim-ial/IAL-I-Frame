<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginHistory.aspx.cs" Inherits="iFrames.DSPApp.LoginHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login History</title>
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
    <link href="css/datepicker.css" rel="stylesheet" />
    <%--<style type="text/css">
        .gvwpaging
        {
        	padding-left:5px;
        	padding-right:5px;
        }
    </style>--%>
    <script language="javascript" type="text/javascript">
        function validate() {
            var ddlBranch = $("#ddlBranch");
            if ($("#ddlBranch :selected").length == 0) {
                alert('Select Branch');
                return false;
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
                    <li id="liUserMngmnt" runat="server" class="parent"><a href="#"><i class="icon-user">
                    </i><span>User Management</span></a>
                        <ul class="sub-menu">
                            <li style="list-style-type: none;"><a href="CreateUser.aspx">User Creation</a> </li>
                            <li class="active" style="list-style-type: none;"><a href="Active.aspx">Active/Inactive
                                Login</a> </li>
                            <li style="list-style-type: none;"><a href="LoginHistory.aspx">Login History</a>
                            </li>
                        </ul>
                    </li>
                    <li class="parent"><a href="#"><i class="icon-magnifier"></i><span>Return Analysis</span></a>
                        <ul class="sub-menu">
                            <li id="liUploadExl" runat="server" style="list-style-type: none;"><a href="upload.aspx">
                                Upload Excel File </a></li>
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
                    Login History</h3>
                <ol class="breadcrumb">
                    <li><a href="#">User Management</a></li>
                    <li class="active.html">Login History</li>
                </ol>
            </div>
            <div class="main-content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default panel-heading-fullwidth panel-borders">
                            <div class="panel-heading">
                                <h3>
                                    Login History</h3>
                            </div>
                            <div class="panel-body">
                                <div style="border-radius: 0px;" class="form-horizontal group-border-dashed">
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Select Branch</label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlBranch" class="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                        <%--<div class="col-sm-4">
                                        </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            From Date</label>
                                        <div class="col-sm-6">
                                            <div data-min-view="2" class='input-group date datetimepicker col-md-10 col-xs-7'
                                                id='datetimepicker1'>
                                                
                                            </div>
                                            <input type='text' id="txtFromDate" runat="server" class="form-control" />
                                        </div>
                                        <%-- <div class="col-sm-4">
                                           
                                        </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            To Date</label>
                                        <div class="col-sm-6">
                                            <div data-min-view="2" class='input-group date datetimepicker col-md-10 col-xs-7'
                                                id='Div2'>
                                               
                                            </div>
                                             <input type='text' id="txtToDate" runat="server" class="form-control" />
                                        </div>
                                        <%-- <div class="col-sm-4">
                                            
                                        </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-3 ">
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" OnClientClick="return validate();"
                                                Text="Search" OnClick="btnSearch_Click" />
                                            <asp:Button ID="btnReset" class="btn btn-primary" runat="server" Text="Reset" OnClick="btnReset_Click" />
                                            <asp:Button ID="btnExport" class="btn btn-primary" runat="server" Text="Export to Excel"
                                                OnClick="btnExport_Click" />
                                        </div>
                                        <%--<div class="col-sm-3">
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" class="row" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript">
                            $(document).ready(function () {
                                var prm = Sys.WebForms.PageRequestManager.getInstance();
                                if (prm != null) {
                                    prm.add_endRequest(function (sender, e) {
                                        if (sender._postBackSettings.panelsToUpdate != null) {
                                            gvwPaging();
                                        }
                                    });
                                }
                               gvwPaging();
                            });

                            function gvwPaging() {
                                if ($("#gvwLoginHis").length > 0) {
                                    $('#gvwLoginHis table td').each(function () {
                                        //alert(this.innerHTML);
                                        var el = $(this);
                                        el.css("font-size", "17px");
                                        el.css("padding-left", "5px");
                                        el.css("padding-right", "5px");
                                    });
                                }
                            }
                        </script>
                        <%-- <div class="row">--%>
                        <!--Responsive table-->
                        <div class="col-sm-12">
                            <div class="widget widget-fullwidth widget-small">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvwLoginHis" CssClass="table table-striped table-fw-widget table-hover"
                                        AutoGenerateColumns="false" GridLines="None" AllowPaging="true" PageSize="7"
                                        EmptyDataText="No Record Found" BorderWidth="0px" runat="server" Width="1000"
                                        OnPageIndexChanging="gvwLoginHis_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Login ID</HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="user-avatar">
                                                        <span class="icon-user"></span>
                                                        <%# Eval("EMAIL_ID")%>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle Width="350" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Last Login</HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("LAST_LOGIN", "{0:dd-MMM-yyyy hh:mm tt}")%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="200" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Login Count</HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("LOGIN_COUNT")%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="150" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Branch Name</HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Branch_Name")%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="150" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First"
                                            LastPageText="Last" />
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
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/lib/jquery.nanoscroller/javascripts/jquery.nanoscroller.min.js"
        type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="js/date.js" type="text/javascript"></script>
    <script src="js/jquery.multiple.select.js" type="text/javascript"></script>
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

         $('#txtFromDate').datepicker({
            format: 'dd M yyyy',
            autoclose: true,
        });
        $('#txtToDate').datepicker({
            format: 'dd M yyyy',
            autoclose: true,
        });

        
    </script>
    </form>
</body>
</html>
