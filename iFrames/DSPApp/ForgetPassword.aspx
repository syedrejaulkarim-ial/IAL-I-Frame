<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs"
    Inherits="iFrames.DSPApp.ForgetPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
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
</head>
<body class="am-splash-screen">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="am-wrapper am-login">
                <div class="am-content">
                    <div class="main-content">
                        <div class="login-container forgot-password">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <img src="img/dspBR_MF_logo.png" alt="logo" class="logo-img"><span>Forgot your password?</span></div>
                                <div class="panel-body">
                                    <%--<form action="CreateUser.aspx" parsley-validate="" novalidate method="get" class="form-horizontal">--%>
                                    <p class="text-center">
                                        Don't worry, we'll send you an email to reset your password.</p>
                                    <div class="form-group">
                                        <div id="email-handler" class="input-group">
                                            <span class="input-group-addon"><i class="icon s7-mail"></i></span>
                                            <input type="text" name="email" id="txtEmail" runat="server" parsley-trigger="change"
                                                data-parsley-errors-messages-disabled="true" data-parsley-class-handler="#email-handler"
                                                required placeholder="Your Email" autocomplete="off" class="form-control" />
                                        </div>
                                    </div>
                                    <asp:Button ID="BtnSubmit" class="btn btn-block btn-primary btn-lg" runat="server"
                                        Text="Reset Password" OnClick="BtnSubmit_Click" />
                                    <%-- <button type="submit" class="btn btn-block btn-primary btn-lg">
                                Reset Password</button>--%>
                                    <%-- </form>--%>
                                </div>
                            </div>
                            <%--<div class="text-center out-links">
                                <a href="#">© 2015 Your Company</a>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
