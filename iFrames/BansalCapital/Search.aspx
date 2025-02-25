<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="iFrames.BansalCapital.Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DSearch for mutual funds - Bansal Capital</title>
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script src="js/topSearch.js"></script>
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

        body {
            margin: 15px auto;
            text-align: center;
        }
    </style>
</head>
<body>

    <div class="search_box_container">
        <input type="text" class="input_control" placeholder="Search here for mutual funds" id="txtTopSearch" />
        <input type="hidden" id="txtTopSearchID" value="0" />
        <button class="btn_search" type="button" id="btnSearch">SEARCH</button>
        <div id="divNoRes" style="width: 100%; text-align: center; color: red; margin: 15px -10px; font-size: 18px;"></div>
    </div>

</body>
</html>
