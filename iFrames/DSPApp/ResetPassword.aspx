<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs"
    Inherits="iFrames.DSPApp.ResetPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reset Password</title>
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
         <script src="js/jquery.nanoscroller.min.js"></script>
    <![endif]-->
    <link type="text/css" href="css/style.css" rel="stylesheet">
    <link href="css/simple-line-icons.css" rel="stylesheet">
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidatePass() {
            var txtNewPass = $("#txtNewPass");
            var NewPass = txtNewPass.val().trim();
            var txtReNewPass = $("#txtReNewPass");
            var ReNewPass = txtReNewPass.val().trim();

            if (NewPass == '') {
                alert("Enter New Password.");
                txtNewPass.focus();
                return false;
            }

            if (NewPass.length < 6 || NewPass.length > 8) {
                alert("New Password length will be between 6 to 8 char.");
                txtNewPass.focus();
                return false;
            }

            //var password = txtPassword.val();
            var UcaseCount = 0;
            var LcaseCount = 0;
            var SpclCount = 0;
            var char;
            var specialChars = "<>@!#$%^&*()_+[]{}?:;|'\"\\,./~`-=";
            for (var i = 0; i < NewPass.length; i++) {
                char = NewPass.charAt(i);
                if (/[A-Z]/.test(char))
                    UcaseCount++;
                if (/[a-z]/.test(char))
                    LcaseCount++;
                if (specialChars.indexOf(char) >= 0)
                    SpclCount++;
            }

            if (UcaseCount == 0) {
                alert("New Password must contain one Capital character.");
                txtNewPass.focus();
                return false;
            }
            if (LcaseCount == 0) {
                alert("New Password must contain one Small character.");
                txtNewPass.focus();
                return false;
            }
            if (SpclCount == 0) {
                alert("New Password must contain one Special character.");
                txtNewPass.focus();
                return false;
            }

            if (ReNewPass == '') {
                alert("Enter Retype New Password.");
                txtReNewPass.focus();
                return false;
            }
            if (NewPass != ReNewPass) {
                alert("Retype New Password should match with New Password.");
                txtReNewPass.focus();
                return false;
            }
        }
    </script>
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
                                    <img src="img/dspBR_MF_logo.jpg" alt="logo" class="logo-img"><span>Reset your password?</span></div>
                                <div class="panel-body">
                                    <%--<form action="CreateUser.aspx" parsley-validate="" novalidate method="get" class="form-horizontal">--%>
                                    <%--<p class="text-center">
                                        Don't worry, we'll send you an email to reset your password.</p>--%>
                                    <%--<div class="form-group">--%>
                                    <%--<div id="email-handler" class="input-group">
                                            <span class="input-group-addon"><i class="icon s7-mail"></i></span>
                                            <input type="password" name="email" id="txtNewPass" runat="server" parsley-trigger="change"
                                                data-parsley-errors-messages-disabled="true" required placeholder="New Password"
                                                autocomplete="off" class="form-control" />
                                        </div>
                                        <div id="Div1" class="input-group">
                                            <span class="input-group-addon"><i class="icon s7-mail"></i></span>
                                            <input type="password" name="email" id="txtReNewPass" runat="server" parsley-trigger="change"
                                                data-parsley-errors-messages-disabled="true" required placeholder="Retype New Password"
                                                autocomplete="off" class="form-control" />
                                        </div>--%>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"><span class="icon-key"></span></span>
                                            <input type="password" name="email" id="txtNewPass" runat="server" parsley-trigger="change"
                                                data-parsley-errors-messages-disabled="true" required placeholder="New Password"
                                                autocomplete="off" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"><span class="icon-key"></span></span>
                                            <input type="password" name="email" id="txtReNewPass" runat="server" parsley-trigger="change"
                                                data-parsley-errors-messages-disabled="true" required placeholder="Retype New Password"
                                                autocomplete="off" class="form-control" />
                                        </div>
                                    </div>
                                    <%--</div>--%>
                                    <div class="form-group login-submit">
                                        <asp:Button ID="BtnSubmit" class="btn btn-block btn-primary btn-lg" runat="server"
                                            Text="Reset Password" OnClientClick="return ValidatePass();" OnClick="BtnSubmit_Click" />
                                    </div>
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
