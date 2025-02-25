<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelFileUpload.aspx.cs" Inherits="iFrames.CanaraClient.ExcelFileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <%--<script src="assets/lib/jquery.nanoscroller/javascripts/jquery.nanoscroller.min.js" type="text/javascript"></script>--%>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="js/date.js" type="text/javascript"></script>
    <%--<script src="assets/lib/theme-switcher/theme-switcher.min.js" type="text/javascript"></script>--%>
    <script src="js/jquery.multiple.select.js" type="text/javascript"></script>
    <%--<script src="js/jquery.nanoscroller.min.js"></script>--%>
    <script type='text/javascript'>
        function validate() {
            if (document.getElementById("EventDate").value == "") {
                alert("Please select Event Date");
                document.getElementById("EventDate").focus();
                return false;
            }

            if (document.getElementById("ddlCategory").value == "--- Select Category----") {
                alert("Please select Category");
                document.getElementById("ddlCategory").focus();
                return false;
            }
        }
    </script>
    <title>Static Record</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="assets/img/favicon.png">
    <link type="text/css" href="css/style.css" rel="stylesheet">
    <link href="css/simple-line-icons.css" rel="stylesheet">
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet">
    <link href="css/datepicker.css" rel="stylesheet" />
    <link href="css/multiSelectDropdown/multiple-select.css" rel="stylesheet" />
    <style type="text/css">
        .gridheader
        {
            cursor: hand;
        }
        th.sortasc a  
        {
            display:block; padding:0 4px 0 15px; 
            background:url(img/asc.gif) no-repeat;  
        }

        th.sortdesc a 
        {
            display:block; padding:0 4px 0 15px; 
            background:url(img/desc.gif) no-repeat;
        }
        .nav_title{
            color: white;
            /*text-transform: uppercase;*/
            font-weight: 600;
        }
        /*Loder*/
        

        #cover-spin {
    position:fixed;
    width:100%;
    left:0;right:0;top:0;bottom:0;
    background-color: rgba(255,255,255,0.7);
    z-index:9999;
    display:none;
}

@-webkit-keyframes spin {
	from {-webkit-transform:rotate(0deg);}
	to {-webkit-transform:rotate(360deg);}
}

@keyframes spin {
	from {transform:rotate(0deg);}
	to {transform:rotate(360deg);}
}

#cover-spin::after {
    content:'';
    display:block;
    position:absolute;
    left:48%;top:40%;
    width:40px;height:40px;
    border-style:solid;
    border-color:black;
    border-top-color:transparent;
    border-width: 4px;
    border-radius:50%;
    -webkit-animation: spin .8s linear infinite;
    animation: spin .8s linear infinite;
}
    </style>
    <script type="text/javascript">
        function disableControl() {
            document.getElementById('<%= Button1.ClientID %>').disabled = true;
            document.getElementById('<%= Button1.ClientID %>').disabled = true;
        }
        function enableControl() {
            document.getElementById('<%= Button1.ClientID %>').disabled = false;
            document.getElementById('<%= Button1.ClientID %>').disabled = false;
        }
        function showLoader() {
            document.getElementById("cover-spin").style.display = "block";
        }

        function hideLoader() {
            document.getElementById("cover-spin").style.display = "none";
        }
       <%-- function validateFileSize() {
            alert('call');
            var fileInput = document.getElementById('<%= FileUpload1.ClientID %>');
            console.log(fileInput);
            var filePath = fileInput.value;
            var fileSize = fileInput.files[0].size; // Size in bytes
            var maxSize = 5 * 1024 * 1024; // 5MB in bytes

            if (fileSize > maxSize) {
                alert("File size must be less than 5MB.");
                return false;
            }
            return true;
        }--%>
    </script>
</head>
<body>
    <form id="form1" class="form-horizontal group-border-dashed" runat="server">
        <div class="am-wrapper">
            <nav class="navbar navbar-default navbar-fixed-top am-top-header">
                <div class="container-fluid">
                <div class="navbar-header">
                    <div class="page-title"><span>Dashboard</span></div><a href="#" class="am-toggle-left-sidebar navbar-toggle collapsed"><span class="icon-bar"><span></span><span></span><span></span></span></a>
                            <%--<a href="#" class="navbar-brand"></a>--%>
                    <h1 class="nav_title">Canara Mutual Fund</h1>
                </div><a href="#" class="am-toggle-right-sidebar"><span class="icon s7-menu2"></span></a><a href="#" data-toggle="collapse" data-target="#am-navbar-collapse" class="am-toggle-top-header-menu collapsed"> <i class="fa fa-angle-down"></i></a>
                <div id="am-navbar-collapse" class="collapse navbar-collapse">
                    <ul class="nav navbar-nav navbar-right am-user-nav">
                    <li class="dropdown">
                    <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle"><%= Session["EmailId"] %>
        
                    <i class="fa fa-angle-down"></i></a>
                        <ul role="menu" class="dropdown-menu">
             	            <%--<li><a href="ForgetPassword.aspx"> <span class="icon-key"></span>Reset Password</a></li>--%>
                            <li><a href="Login.aspx?param=logout"> <span class="icon-logout"></span>Sign Out</a></li>
                        </ul>
                    </li>
                    </ul>
                </div>
                </div>
            </nav>
            <div class="am-left-sidebar">
                <div class="content">
                    <div class="am-logo">
                    </div>
                    <ul class="sidebar-elements">
                        <li id="liUserMngmnt" runat="server" class="parent"><a href="#"><i class="icon-user"></i></a></li>
                        <li class="active" style="list-style-type: none;"><a href="#">Static Record</a> </li>
                        <li style="list-style-type: none;"><a href="CanaraUploadExcel.aspx">Index Record</a> </li>
                    </ul>
                    <!--Sidebar bottom content-->
                </div>
            </div>
            <div class="am-content">
                <div class="page-head">
                    <h3>Static Record</h3>
                </div>
                <div class="main-content">
                    
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <span>Upload Your FIle. Only `.xlsx` allow</span>
                            
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-3" style="padding-top: 30px;">
                                    <asp:Button ID="Button3" class="btn btn-primary" style="margin-right: 20px;" runat="server" OnClick="btnTmpDownload_Click" Text="Download Template" />
                                    <%--<a href="exceltemplate\Template.xlsx" style="font-weight: 700;font-size: large;margin: 0 40px;">
                                        <i class="fa fa-download" aria-hidden="true"></i>
                                        <span>Download Template</span>
                                    </a>--%>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group" style="margin: 10px 0px;">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-upload" aria-hidden="true"></i></span>
                                            <asp:FileUpload ID="FileUpload1" class="form-control" runat="server" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3" style="padding-top: 15px;">
                                    <div class="form-group login-submit" style="">
                                        <asp:Button ID="Button1" class="btn btn-primary" style="margin-right: 20px;" runat="server" OnClick="btnUpload_Click" Text="Upload and Preview" OnClientClick="$('#cover-spin').show(0);"/>
                                        <asp:Button ID="Button4" class="btn btn-danger" runat="server" OnClick="btnrefresh_Click" Text="Refresh" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Panel ID="pnlExport" runat="server">
                                        <div style="text-align:right;margin-bottom: 5px;padding-right: 25px;"> 
                                            <asp:Button ID="Button2" class="btn btn-primary" runat="server" OnClick="btnExport_Click" Text="Export to Excel" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlGridview" runat="server" Visible="false">
                                        <div style="height:auto;overflow:scroll;max-height: 60%;">
                                            <asp:GridView ID="grvExcelData" runat="server" CssClass="GridviewTable"></asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="cover-spin"></div>
    </form>

    <script src="js/main.js" type="text/javascript"></script>
   <%-- <script type="text/javascript">
        $(document).ready(function () {
            //initialize the javascript
            App.init();
        });

    </script>--%>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            App.livePreview();
        });
    </script>--%>
</body>
    
</html>
