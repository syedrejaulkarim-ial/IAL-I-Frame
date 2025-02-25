<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="iFrames.AskMeFund.Search" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
 <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
 <meta http-equiv="X-UA-Compatible" content="IE=edge" />
<title>Search</title>
<script type='text/javascript' src="js/jquery.js"></script>
<script type="text/javascript" src="js/jquery-ui.js"></script>
<link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
<%--<script src="js/AutoComplete.js" type="text/javascript"></script>--%>
<script src="js/topSearch.js"></script>

<link href="css/jquery-ui.css" rel="stylesheet" />
<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.6.3/css/all.css" />
<link href="assets/css/custom.css" rel="stylesheet" />

<!-- Bootstrap Css -->
<link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<!-- Icons Css -->
<link href="assets/css/icons.min.css" rel="stylesheet" type="text/css" />
<!-- App Css-->
<link href="assets/css/app.min.css" rel="stylesheet" type="text/css" />
    <style>
        .search_box_container {
            margin: 0 auto;
            /*text-align: center;*/
            display: inline-block;
            /* width: 100%; */
            position: relative;
        }

        .input_control:focus, .btn_search:focus {
            outline: none
        }

        .ui-widget-content {
            border-radius: 5px;
            text-align: left;
            height: 400px;
            overflow: auto;
        }

            .ui-widget-content li {
                text-align: left
            }

        .ui-menu .ui-menu-item a {
            font-size: 13px;
        }

        .input_control {
            border-radius: 17px;
            border: 2px solid #c3c3c3;
            font-size: 13px;
            padding: 6px;
            width: 350px;
            padding-left: 30px;
            padding-right: 87px;
        }

        .btn_search {
            background: #cc0000;
            border-radius: 17px;
            color: #fff;
            padding: 7px 15px;
            position: absolute;
            right: 1px;
            border: 0;
            top: 1px;
            font-size: 13px;
            font-weight: 700;
            letter-spacing: 1px;
            cursor: pointer;
        }

      .form-control{
          border: 1px solid #d1d1d1;
          border-radius: 0.4rem;
          padding: 0.35rem 0.65rem;
          font-size: .75rem;
      }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="layout-wrapper">
    <div class="main-content">
        <div class="page-content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:TextBox ID="txtTopSearch" runat="server" Width="300px" placeholder="Search for Mutual Funds" CssClass="form-control"></asp:TextBox>
                        <asp:HiddenField ID="txtTopSearchID" runat="server" />
                        <div id="divNoRes" style="width: 100%; text-align: center; color: red; margin: 15px -10px; font-size: 18px;"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

       
    </form>
</body>
</html>
