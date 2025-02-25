<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="iFrames.DSPApp.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="assets/img/favicon.png" />
    <!--    <link rel="stylesheet" type="text/css" href="css/style.css"/>
    <link rel="stylesheet" type="text/css" href="assets/lib/jquery.nanoscroller/css/nanoscroller.css"/>-->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link type="text/css" href="css/style.css" rel="stylesheet" />
    <link href="css/simple-line-icons.css" rel="stylesheet" />
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet" />
    <%--    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>--%>
    <script type="text/javascript" language="javascript">
        function ValidateLogin() {
            var txtUserName = $("#username");
            if (txtUserName.val().trim() == '') {
                alert("Enter Username.");
                $("#username").focus();
                return false;
            }

            //            if (txtUserName.val().indexOf("@dspblackrock.com") == -1) {
            //                alert("Invalid Email Id.");
            //                txtUserName.focus();
            //                return false;
            //            }

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
<body class="am-splash-screen">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--<asp:HiddenField ID="hdnWrongPwdAttmeptCount" Value="0" runat="server" />--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="am-wrapper am-login">
                    <div class="am-content">
                        <div class="main-content">
                            <div class="login-container">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <img src="img/dspBR_MF_logo.png" alt="logo" class="logo-img"><span>Please enter your
                                        user information.</span>
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
                                            <div class="form-group footer row">
                                                <div class="col-xs-6">
                                                    <a href="ForgetPassword.aspx">Forgot Password?</a>
                                                </div>
                                                <div class="col-xs-6 remember">
                                                </div>
                                            </div>
                                        </div>
                                        <%--</form>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="assets/lib/theme-switcher/theme-switcher.min.js" type="text/javascript"></script>
    <script src="assets/lib/jquery.nanoscroller/javascripts/jquery.nanoscroller.min.js" type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
    <script src="js/jquery.nanoscroller.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //initialize the javascript
            App.init();
            //$('.nano').nanoScroller();
        });

    </script>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            App.livePreview();
        });
        function getParentUrl() {
           // debugger;
            var isInIframe = (parent !== window),
                parentUrl = null;

            if (isInIframe) {
                parentUrl = document.referrer;
                if (parentUrl.indexOf("http://172.17.3.2") == 0) {
                    alert(parentUrl.indexOf("http://172.17.3.2"));
                    alert(parentUrl);
                    //return true;
                }
                else {
                    return null;
                }
            }

            return parentUrl;
        }
    </script>--%>
    <%--<script type="text/javascript">
        getParentUrl();

        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
      m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-68396117-1', 'auto');
        ga('send', 'pageview');
      
      
    </script>--%>
</body>
</html>