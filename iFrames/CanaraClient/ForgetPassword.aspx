<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="iFrames.CanaraClient.ForgetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forget Password</title>
    <link type="text/css" href="css\bootstrap.min.css" rel="stylesheet" />
    <style>
        #highlighted {
            position: relative;
            background-color: #DC143C;
        }
        @media (min-width: 992px)
        #highlighted .container-fluid {
            margin-bottom: 2.5rem;
        }
        #highlighted .container-fluid h1, #highlighted .container-fluid p {
            color: #FFF;
        }
        .h1, h1 {
            font-size: 54.93px;
        }
        .h1, h1, h2, h3, h4, h5, h6 {
            font-family: Verlag,museo-sans,'Helvetica Neue',Helvetica,Arial,sans-serif;
            color: #414141;
        }
        .h1, body, h1, h2, h3, h4, h5, h6, html {
            font-weight: 300;
        }
        #content {
            background-position: right bottom;
            background-repeat: no-repeat;
        }
        .interior-page {
            background-color: #FFF;
            padding-bottom: 30px;
        }
        #highlighted+#content.interior-page .interior-page-nav {
            margin-top: -4em;
        }
        #highlighted+#content.interior-page .interior-page-nav, .interior-page .interior-page-nav {
            padding-left: 0;
        }
        .sidebar {
            margin-top: 2em;
        }
        @media (min-width: 1200px)
        .col-lg-2 {
            width: 16.66666667%;
        }
        .content-area-right {
            /*max-width: 1200px;*/
            padding-right: 15px;
            padding-left: 15px;
        }
        .container-fluid>.row h2.crumb-title {
            margin-bottom: 0;
        }
        .page-title {
            min-height: 50px;
        }
        .page-title, ul {
            margin: 0;
            list-style: none;
        }
        .content-crumb-div {
            margin: 5px 0 20px;
        }
        a {
            text-decoration: none;
        }
        .container-fluid .row .modal, .page .modal {
            position: fixed;
            top: 35%;
        }
        #highlighted+#content.interior-page .interior-page-nav, .interior-page .interior-page-nav {
            padding-left: 0;
        }
        #highlighted+#content.interior-page .interior-page-nav {
            margin-top: -4em;
        }
        .dynamicDiv.panel-group {
            border: 1px solid #E7E9E9;
            margin-left: 30px;
        }
        .panel-group {
            margin-bottom: 0;
            background-color: #fff;
        }
        .panel-group .panel {
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0;
            border: none;
            box-shadow: none;
        }
        .panel-group .panel-heading {
            padding: 0;
            border: none;
        }
        .panel-default>.panel-heading {
            color: #333;
            background-color: #f5f5f5;
            border-color: #ddd;
        }
        .panel-group .panel-heading .panel-title {
            font-size: 1.1em;
            font-family: Verlag,museo-sans,'Helvetica Neue',Helvetica,Arial,sans-serif;
        }
        .interior-page-nav .panel-group .panel-heading .panel-title a {
            background: 0 0;
        }
        .panel-group .panel-heading .panel-title a {
            display: block;
            padding: 15px 45px 15px 15px;
            background: url(/resources/images/misc/icon_accordion-open.png) 95% center no-repeat #f6f6f6;
        }
        span.subMenuHighlight, ul.panel-heading li.panel-title a:hover {
            color: #ED3C95;
        }
        .panel-group .panel-heading .panel-title {
            font-size: 1.1em;
            font-family: Verlag,museo-sans,'Helvetica Neue',Helvetica,Arial,sans-serif;
        }
        ul.panel-heading {
            margin-bottom: 1px;
        }
        .panel-group .panel-heading .panel-title a {
            display: block;
            padding: 15px 45px 15px 15px;
            background: url(/resources/images/misc/icon_accordion-open.png) 95% center no-repeat #f6f6f6;
        }
        .panel-group {
            margin-bottom: 0;
            background-color: #fff;
        }
        .label-default {
            background-color: #FFF;
            margin-top: 10px;
        }
        label {
            display: inline-block;
            margin-bottom: 5px;
            font-weight: 700;
        }
        .form-control {
            border-radius: 0;
        }
        .btn-primary {
            color: #fff;
            background-color: #DC143C;
            border-color: #ea3e10;
            width: 100%;
        }
        .btn-block {
            display: block;
        }
        .btn {
            padding: 8px 28px;
            font-weight: 400;
            -webkit-transition: background .3s ease-in;
            transition: background .3s ease-in;
            white-space: normal;
            border-width: 0 0 1px;
        }
        .content-area-right {
           margin-top: 10%;
        }
        .headingtitle{
            text-align: center;
            text-decoration: underline;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="content" class="interior-page">
        <div class="container-fluid">
            <div class="row">
                <!--Content-->
                <div class="col-sm-12 col-md-12 col-lg-12">
                  <div class="content-area-right">
                      <h1 class="headingtitle">Canara Client</h1>
                      <div class="row">
                        <div class="col-md-4 forgot-form"></div>
                        <div class="col-md-4 forgot-form">
                            <div class="panel panel-default">
                                <div class="panel-heading"></div>
                                <div class="panel-body">
                                    <p class="text-center">Don't worry, we'll send you an email to reset your password.</p>
                                    <div class="form-group" style="margin-bottom:10px;">
                                        <div id="email-handler" class="input-group">
                                            <span class="input-group-addon"><i class="icon s7-mail"></i></span>
                                            <input type="text" name="email" id="txtEmail" runat="server" parsley-trigger="change"
                                                data-parsley-errors-messages-disabled="true" data-parsley-class-handler="#email-handler"
                                                required placeholder="Your Email" autocomplete="off" class="form-control" />
                                        </div>
                                    </div>
                                    <asp:Button ID="BtnSubmit" class="btn btn-block btn-primary btn-lg" runat="server" Text="Reset Password" OnClick="BtnSubmit_Click" />
                                </div>
                            </div>
                        <%--<a id="mybad" class="btn btn-primary" role="button">RESET</a>--%>
                        </div>
                        <div class="col-md-4 forgot-form"></div>
                        <%--<div class="col-md-12 forgot-return" style="display:block;">
                        <h3>
                            Reset Password Sent
                        </h3>
                        <p>
                            An email has been sent to your address with a reset password you can use to access your account.
                        </p>
                        </div>--%>
                      </div>
                   </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
<script src="js\jquery.min.js"></script>
<script src="js\bootstrap.min.js"></script>
</html>
