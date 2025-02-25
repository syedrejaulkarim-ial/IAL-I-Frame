<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="iFrames.Canara.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="css\bootstrap.min.css" rel="stylesheet" />
    <style>
        .content-area-right {
           margin-top: 10%;
        }
        .headingtitle{
            text-align: center;
            text-decoration: underline;
            font-weight: 600;
        }
    </style>
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
<body>
    <form id="form1" runat="server">
        <div id="content" class="interior-page">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div class="content-area-right">
                            <h1 class="headingtitle">Canara Client</h1>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4"></div>
                            <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <span>Reset your password?</span>
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><span class="icon-key"></span></span>
                                                <input type="password" name="email" id="txtNewPass" runat="server" parsley-trigger="change"
                                                    data-parsley-errors-messages-disabled="true" required placeholder="New Password"
                                                    autocomplete="off" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group" style="margin: 10px 0px;">
                                            <div class="input-group">
                                                <span class="input-group-addon"><span class="icon-key"></span></span>
                                                <input type="password" name="email" id="txtReNewPass" runat="server" parsley-trigger="change"
                                                    data-parsley-errors-messages-disabled="true" required placeholder="Retype New Password"
                                                    autocomplete="off" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group login-submit" style="text-align: center;">
                                            <asp:Button ID="Button1" class="btn btn-block btn-primary btn-lg" runat="server"
                                                Text="Reset Password" OnClientClick="return ValidatePass();" OnClick="BtnSubmit_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-4 col-lg-4"></div>
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
