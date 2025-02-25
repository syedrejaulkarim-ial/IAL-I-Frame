<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="iFrames.DSPApp.CreateUser" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>User Creation</title>
     <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="assets/img/favicon.png">
    <title>DSP Blackrock</title>
    <!--    <link rel="stylesheet" type="text/css" href="css/style.css"/>
    <link rel="stylesheet" type="text/css" href="assets/lib/jquery.nanoscroller/css/nanoscroller.css"/>-->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link type="text/css" href="css/style.css" rel="stylesheet">
    <link href="css/simple-line-icons.css" rel="stylesheet">
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet">
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>

    <%--<script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>--%>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#btnCreateUser").click(function () {
                try {
                    var txtEmailId = $("#txtEmailId");
                    var EmailId = txtEmailId.val().trim();

                    if (EmailId == '') {
                        alert("Enter Email Id.");
                        txtEmailId.focus();
                        return false;
                    }

                    var ddlBranch = $("#ddlBranch");
                   // if ($("#ddlBranch :selected").length == 0)
                      if ($("#ddlBranch").val() == "Select Branch")
                    //if ($('#ddlBranch').attr('selectedIndex') == 0)
                    {
                        alert('Select Branch');
                        return false;
                    }

                    var BranchName = ddlBranch.val();

                    //                    if (EmailId.indexOf(' ') >= 0) {
                    //                        alert("Space not allowed in Email Id.");
                    //                        txtEmailId.focus();
                    //                        return false;
                    //                    }

                    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                    if (regex.test(EmailId) == false) {
                        alert("Invalid Email Id.");
                        txtEmailId.focus();
                        return false;
                    }

                    if (EmailId.indexOf("@dspim.com") == -1) {
                        alert("Email Id should end with '@dspim.com'.");
                        txtEmailId.focus();
                        return false;
                    }

                    var txtPassword = $("#txtPassword");
                    if (txtPassword.val().trim() == '') {
                        alert("Enter Password.");
                        txtPassword.focus();
                        return false;
                    }
                    if (txtPassword.val().trim().length < 6 || txtPassword.val().trim().length > 8) {
                        alert("Password length will be between 6 to 8 char.");
                        txtPassword.focus();
                        return false;
                    }

                    var password = txtPassword.val();
                    var UcaseCount = 0;
                    var LcaseCount = 0;
                    var SpclCount = 0;
                    var char;
                    var specialChars = "<>@!#$%^&*()_+[]{}?:;|'\"\\,./~`-="
                    for (var i = 0; i < password.length; i++) {
                        char = password.charAt(i);
                        if (/[A-Z]/.test(char))
                            UcaseCount++;
                        if (/[a-z]/.test(char))
                            LcaseCount++;
                        if (specialChars.indexOf(char) >= 0)
                            SpclCount++;
                    }

                    if (UcaseCount == 0) {
                        alert("Password must contain one Capital character.");
                        txtPassword.focus();
                        return false;
                    }
                    if (LcaseCount == 0) {
                        alert("Password must contain one Small character.");
                        txtPassword.focus();
                        return false;
                    }
                    if (SpclCount == 0) {
                        alert("Password must contain one Special character.");
                        txtPassword.focus();
                        return false;
                    }

                    var txtRetypePassword = $("#txtRetypePassword");
                    if (txtRetypePassword.val().trim() == '') {
                        alert("Enter Retype Password.");
                        txtRetypePassword.focus();
                        return false;
                    }
                    if (txtPassword.val().trim() != txtRetypePassword.val().trim()) {
                        alert("Retype Password should match with Password.");
                        txtRetypePassword.focus();
                        return false;
                    }

                    var dataToPush = JSON.stringify({
                        EmailId: EmailId
                    });

                    $.ajax({
                        cache: false,
                        data: dataToPush,
                        dataType: "json",
                        url: 'CreateUser.aspx/checkUserExist',
                        type: 'POST',
                        asynchronus: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //alert(data.d);
                            if (data.d == false) {
                                createUser(EmailId, txtPassword.val(), BranchName);
                                return false;
                            }
                            else {
                                alert("'" + txtEmailId.val() + "' already exists.");
                                return false;
                            }
                        },
                        error: function (data, e) {
                            alert("err1 " + data.responseText);
                            return false;
                        }
                    });

                    return false;
                }
                catch (e) {
                    alert(e.Message);
                }
            });

            $("#btnReset").click(function () {
                Reset();
                return false;
            });

            function Reset() {
                $("#txtEmailId").val("");
                $("#txtPassword").val("");
                $("#txtRetypePassword").val("");
                $("#ddlBranch").prop('selectedIndex', 0);

                return false;
            }

            function createUser(EmailId, Password, BranchName) {
                var dataToPush = JSON.stringify({
                    EmailId: EmailId,
                    Password: Password,
                    BranchName: BranchName
                });

                $.ajax({
                    cache: false,
                    data: dataToPush,
                    dataType: "json",
                    url: 'CreateUser.aspx/createUser',
                    type: 'POST',
                    asynchronus: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        //alert(data.d);
                        if (data.d == '2') {
                            alert('User created successfully.');
                            Reset();
                            return false;
                        }
                        else if (data.d == '1') {
                            $(location).attr('href', 'Login.aspx')
                        }
                        else {
                            alert(data.d);
                        }
                    },
                    error: function (data, e) {
                        alert("err1 " + data.responseText);
                    }
                });
            }
        });


        function validate() {         

            if (document.getElementById("FileUpload1").value == "") {
                alert("Please select File");
                document.getElementById("FileUpload1").focus();
                return false;
            }
        }
    </script>

    </script>
</head>
<body>
    <form id="form1" runat="server">
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
                            <li class="active" style="list-style-type: none;"><a href="Active.aspx">Active/Inactive Login</a>
                            </li>
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
                    User Creation
                </h3>
                <ol class="breadcrumb">
                    <li><a href="#">User Management</a></li>
                    <li class="active.html">User Creation</li>
                </ol>
            </div>
            <div class="main-content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default panel-heading-fullwidth panel-borders">
                            <div class="panel-heading">
                                <h3>
                                    User Creation</h3>
                            </div>
                           
                             <div class="panel-body" id ="CreatUser" runat ="server" >
                                <div style="border-radius: 0px;" class="form-horizontal group-border-dashed">
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Email ID</label>
                                        <div class="col-sm-6">
                                            <input type="text" id="txtEmailId" runat="server" maxlength="50" autocomplete="off" class="form-control" placeholder="Email ID"/>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Password</label>
                                        <div class="col-sm-6">
                                            <input type="password" id="txtPassword" runat="server" autocomplete="off" class="form-control" placeholder="Password"/>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Retype Password</label>
                                        <div class="col-sm-6">
                                            <input type="password" id="txtRetypePassword" runat="server" autocomplete="off" placeholder="Retype Password" class="form-control"/>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                            Select Branch</label>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlBranch" class="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>                                     
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">
                                        </label>
                                        <div class="col-sm-6">
                                            <button id="btnCreateUser" class="btn btn-primary">
                                                Create User</button>
                                            <button id="btnReset" class="btn btn-primary">
                                                Reset</button>                                           
                                            <asp:Button ID="Button3" class="btn btn-primary" runat="server" OnClick="btnExcelUpload_Click" Text="Excel Upload" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                             <div class="panel-body" id ="Upload" runat ="server" visible="false">
                                    <div style style="border-radius: 0px;" class="form-horizontal group-border-dashed">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label">Select File</label>
                                            <div class="col-sm-6">                                       
                                                <asp:FileUpload ID="FileUpload1" class="form-control" runat="server" />
                                            </div>
                                        </div>


                                        <div class="form-group">
                                            <label class="col-sm-3 control-label"></label>
                                            <div class="col-sm-6">
                                                <asp:Button ID="Button1" class="btn btn-primary" runat="server" OnClick="btnUpload_Click" Text="Upload" />
                                                <asp:Button ID="Button2" class="btn btn-primary" runat="server" OnClick="btnReset_Click" Text="Close" />
                                            </div>
                                        </div>

                                     </div>
                               </div>                                
               
                        </div>
                    </div>
                </div>
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
