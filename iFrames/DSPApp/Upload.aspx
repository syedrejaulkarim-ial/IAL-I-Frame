<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="iFrames.DSPApp.Upload"  EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head>

    <script type='text/javascript'>
        function validate() {
            if (document.getElementById("EventDate").value == "") {
                alert("Please select Event Date");
                document.getElementById("EventDate").focus();
                return false;
            }

            if (document.getElementById("FileUpload1").value == "") {
                alert("Please select File");
                document.getElementById("FileUpload1").focus();
                return false;
            }
        }
    </script>

    <script type='text/javascript'>
        function setHourglass() {
            document.body.style.cursor = 'wait';
        }
    </script>

    <title>Upload Excel File</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="assets/img/favicon.png">
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->

    <link type="text/css" href="css/style.css" rel="stylesheet">
    <link href="css/simple-line-icons.css" rel="stylesheet">
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet">
    <link href="css/datepicker.css" rel="stylesheet" />
    <link href="css/multiSelectDropdown/multiple-select.css" rel="stylesheet" />
     <script src="js/jquery.nanoscroller.min.js"></script>
    <%--<link href="../Styles/jquery-ui.css" rel="stylesheet" />--%>
</head>

<body onbeforeunload="setHourglass();" onunload="setHourglass();">
    <form id="form1" class="form-horizontal group-border-dashed" runat="server">

        <div class="am-wrapper">
            <nav class="navbar navbar-default navbar-fixed-top am-top-header">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <div class="page-title"><span>Dashboard</span></div>
                        <a href="#" class="am-toggle-left-sidebar navbar-toggle collapsed"><span class="icon-bar"><span></span><span></span><span></span></span></a><a href="index.php" class="navbar-brand"></a>
                    </div>
                    <a href="#" class="am-toggle-right-sidebar"><span class="icon s7-menu2"></span></a><a href="#" data-toggle="collapse" data-target="#am-navbar-collapse" class="am-toggle-top-header-menu collapsed"><i class="fa fa-angle-down"></i></a>
                    <div id="Div1" class="collapse navbar-collapse">
                        <ul class="nav navbar-nav navbar-right am-user-nav">
                            <li class="dropdown">
                                <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle"><%= Session["EmailId"] %>

                                    <i class="fa fa-angle-down"></i></a>
                                <ul role="menu" class="dropdown-menu">
                                    <li><a href="ForgetPassword.aspx"><span class="icon-key"></span>Reset Password</a></li>

                                    <li><a href="Login.aspx?param=logout"><span class="icon-logout"></span>Sign Out</a></li>
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
                    <div class="am-logo"></div>
                    <ul class="sidebar-elements">
                        <li id="liUserMngmnt" runat="server" class="parent"><a href="#"><i class="icon-user"></i><span>User Management</span></a>
                            <ul class="sub-menu">
                                <li style="list-style-type: none;"><a href="CreateUser.aspx">User Creation</a>
                                </li>
                                <li class="active" style="list-style-type: none;"><a href="Active.aspx">Active/Inactive Login</a>
                                </li>
                                <li style="list-style-type: none;"><a href="LoginHistory.aspx">Login History</a>
                            </li>
                            </ul>
                        </li>
                        <li class="parent"><a href="#"><i class="icon-magnifier"></i><span>Return Analysis</span></a>
                            <ul class="sub-menu">
                                <li id="liUploadExl" runat="server" style="list-style-type: none;"><a href="upload.aspx">Upload Excel File </a>
                                </li>
                                <li style="list-style-type: none;"><a href="viewreport.aspx">View Report</a>
                                </li>
                            </ul>
                        </li>

                    </ul>
                    <!--Sidebar bottom content-->
                </div>
            </div>
            <div id="dvContent" runat="server" class="am-content">
                <div class="page-head">
                    <h3>Upload Excel File </h3>
                    <ol class="breadcrumb">
                        <li><a href="#">Return Analysis</a></li>
                        <li class="active.html">Upload Excel File </li>
                    </ol>
                </div>
                <div class="main-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default panel-heading-fullwidth panel-borders">
                                <div class="panel-heading">
                                    <h3>Upload Excel File </h3>
                                </div>
                                <div class="panel-body">
                                    <form action="#" style="border-radius: 0px;" class="form-horizontal group-border-dashed">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Select Date</label>
                                            <div class="col-sm-6">
                                               
                                                <div data-min-view="2" class='input-group date datetimepicker col-md-10 col-xs-7' id='datetimepicker1'>
                                                    <input type='text' id="EventDate" runat="server" class="form-control" />
                                                    <%--<span class="input-group-addon">
                                                        <i class="fa fa-calendar" id="iconCalenderEventDate" title="Select event date" style="cursor: pointer;"></i>
                                                    </span>--%>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Select File</label>
                                            <div class="col-sm-5">
                                                <!-- <input name=""  type="file"  class="form-control">-->
                                                <asp:FileUpload ID="FileUpload1" class="form-control" runat="server" />
                                            </div>
                                        </div>


                                        <div class="form-group">
                                            <label class="col-sm-3 control-label"></label>
                                            <div class="col-sm-6">
                                                <asp:Button ID="btnUpload" class="btn btn-primary" runat="server" OnClick="btnUpload_Click" Text="Upload" />
                                                <asp:Button ID="btnReset" class="btn btn-primary" runat="server" OnClick="btnReset_Click" Text="Reset" />
                                            </div>
                                        </div>

                                    </form>
                                </div>
                            </div>
                        </div>


                    </div>

                </div>
            </div>


        </div>

        <script src="js/jquery.min.js" type="text/javascript"></script>
        <%--<script src="Script/jquery.ui.core.js"></script>--%>
        <script src="js/main.js" type="text/javascript"></script>
        <script src="js/bootstrap.min.js" type="text/javascript"></script>
        <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
        <script src="js/date.js" type="text/javascript"></script>
        <script src="js/jquery.multiple.select.js" type="text/javascript"></script>

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

            $('#EventDate').datepicker({
                format: 'dd M yyyy',
                autoclose: true,
            });


            //$('#EventDate').datepicker().on('changeDate', function (ev) {
            //    $('#EventDate').val(Date.parse($('#EventDate').data('date'), "MM/dd/yyyy").toString("dd MMM yyyy"));
            //    $('#EventDate').datepicker('hide');
            //});


        </script>

    </form>
</body>
</html>
