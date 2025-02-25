<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="iFrames.CanaraClient.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log in</title>
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="assets/img/favicon.png" />
    <link type="text/css" href="css/style.css" rel="stylesheet" />
    <link href="css/simple-line-icons.css" rel="stylesheet" />
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet" />
    <script type="text/javascript" language="javascript">
        function ValidateLogin() {
            var txtUserName = $("#username");
            if (txtUserName.val().trim() == '') {
                alert("Enter Username.");
                $("#username").focus();
                return false;
            }
            var txtpassword = $("#password");
            if (txtpassword.val().trim() == '') {
                alert("Enter Password.");
                txtpassword.focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ContentTemplate>
    <div class="am-wrapper am-login">
        <div class="am-content">
            <div class="main-content">
                <div class="login-container">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                             <h1 style="font-size: 35px;text-transform: uppercase;">Login</h1>
                            <span>Please enter your user information.</span>
                        </div>
                        <div class="panel-body">
                            <%--<form action="index.php" method="get" class="form-horizontal">--%>
                            <div class="login-form">
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon"><span class="icon-user"></span></span>
                                        <input id="username" type="text" runat="server" placeholder="Username" autocomplete="off"
                                            class="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon"><span class="icon-key"></span></span>
                                        <input id="password" runat="server" type="password" placeholder="Password" class="form-control" />
                                    </div>
                                </div>
                                <div class="form-group login-submit">
                                    <%--<button data-dismiss="modal" type="submit" class="btn btn-primary btn-lg">
                            Log me in</button>--%>
                                    <asp:Button ID="btnLogin" runat="server" Text="Log me in" class="btn btn-primary btn-lg"
                                        OnClick="btnLogin_Click" />
                                </div>
                                <%--<div class="form-group footer row">
                                    <div class="col-xs-6"><a href="ForgetPassword.aspx">Forgot Password?</a></div>
                                    <div class="col-xs-6 remember"></div>
                                </div>--%>
                            </div>
                            <%--</form>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</ContentTemplate>
    </form>

    <%--<div class="container-fluid ps-md-0">
      <div class="row g-0">
        <div class="d-none d-md-flex col-md-4 col-lg-6 bg-image">
        </div>
        <div class="col-md-8 col-lg-6">
          <div class="login d-flex align-items-center py-5">
            <div class="container">
              <div class="row">
                <div class="col-md-9 col-lg-8 mx-auto">
                    <h1>Canara Client</h1>
                    <h3 class="login-heading mb-4">Welcome back!</h3>

                  <!-- Sign In Form -->
                  <form id="form1" runat="server">
                    <div class="form-floating mb-3">
                      <input  type="email" class="form-control" id="username" runat="server" placeholder="">
                      <label for="floatingInput">Email address</label>
                    </div>
                    <div class="form-floating mb-3">
                      <input type="password" class="form-control" id="password" runat="server" placeholder="">
                      <label for="floatingPassword">Password</label>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnLogin" runat="server" Text="Log in" class="btn btn-primary btn-lg"
        OnClick="btnLogin_Click" />
                      <div class="text-center">
                        <a class="large" href="ForgetPassword.aspx">Forgot password?</a>
                      </div>
                    </div>

                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>--%>
</body>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <%--<script src="assets/lib/theme-switcher/theme-switcher.min.js" type="text/javascript"></script>--%>
    <%--<script src="assets/lib/jquery.nanoscroller/javascripts/jquery.nanoscroller.min.js" type="text/javascript"></script>--%>
    <script src="js/main.js" type="text/javascript"></script>
    <%--<script src="js/jquery.nanoscroller.min.js"></script>--%>
   <%-- <script type="text/javascript">
        $(document).ready(function () {
            //initialize the javascript
            App.init();
            //$('.nano').nanoScroller();
        });
    </script>--%>
</html>
