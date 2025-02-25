<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always"
    EnableViewStateMac="true"
    CodeBehind="SIPCalc.aspx.cs" Inherits="iFrames.CafeMutual.SIPCalc" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Return Calculator</title>
    <script type="text/javascript" src="Script/jquery5.js"></script>
    <script type="text/javascript" src="Script/jshashtable8.js"></script>
    <script type="text/javascript" src="Script/jquery7.js"></script>
    <script type="text/javascript" src="Script/tmpl.js"></script>
    <script type="text/javascript" src="Script/jquery4.js"></script>
    <script src="Script/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Script/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Script/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="Script/check.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/jquery.confirn_box.js"></script>
    <script type="text/javascript">
        function pop() {
            $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
        }

        function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
                return true;
            }
            return false;
        }



    </script>
    <link rel="stylesheet" type="text/css" href="CSS/jquery-ui.css" />
    <%-- <link rel="stylesheet" type="text/css" href="CSS/demos.css" />--%>
    <style type="text/css">
        body {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
            font-family: 'Roboto', sans-serif;
        }

        .layout {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }

        .layout-slider {
            width: 100px;
        }

        .layout-slider-settings {
            font-size: 12px;
            padding-bottom: 0px;
        }

            .layout-slider-settings pre {
                font-family: arial;
            }

        .layout1 {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }

        .layout-slider1 {
            width: 50px;
        }

        .layout-slider1-settings {
            font-size: 12px;
            padding-bottom: 0px;
        }

            .layout-slider1-settings pre {
                font-family: arial;
            }

        .layout2 {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }

        .layout-slider2 {
            width: 100px;
            margin: 2px;
            padding: 0;
        }

        .layout-slider2-settings {
            font-size: 12px;
            padding-bottom: 0px;
        }

            .layout-slider2-settings pre {
                font-family: arial;
            }

        .layout-slider4 {
            width: 100px;
            margin: 2px;
            padding: 0;
        }

        .lefft {
            padding-left: 2px;
        }

        /*.borderlefft {
            border-bottom: #c6c8ca solid 1px;
        }

        .borderbottom {
            border-bottom: #c6c8ca solid 1px;
        }*/
                #confirmOverlay {
            width: 100%;
            height: 100%;
            position: fixed;
            top: 0;
            left: 0;
            background-color: rgba(0, 0, 0, 0.7);
            z-index: 100000;
            color: #000;
        }

        #confirmBox {
            background: #fff;
            width: 300px;
            height: auto;
            position: fixed;
            left: 50%;
            top: 50%;
            margin: -130px 0 0 -135px;
            border: 1px solid rgba(0,0,0,.2);
            border-radius: 2px;
            box-shadow: 0 3px 9px rgba(0,0,0,.5);
        }

            #confirmBox h1 {
                letter-spacing: 0.3px;
                color: #888;
                border-bottom: 1px solid #ededed;
                margin-top: 5px;
            }

            #confirmBox h1, #confirmBox p {
                font-size: 18px;
                font-weight: 400;
                background: #fff;
                padding: 5px 10px;
                color: #666;
            }

            #confirmBox p {
                background: none;
                font-size: 12px;
                line-height: 1.4;
                padding-top: 0px;
            }

        #confirmButtons {
            padding: 5px 0 5px;
            text-align: center;
            border-top: 1px solid #ededed;
            background-color: #f9f9f9;
        }
    </style>

    <style type="text/css">
        a:link {
            color: #000000;
            text-decoration: none;
        }

        a:visited {
            color: #000000;
            text-decoration: none;
        }

        a:hover {
            color: #3399FF;
            text-decoration: none;
        }

        a:active {
            color: #000000;
            text-decoration: none;
        }

        .layout3 {
            height: 10px;
        }

        .layout-slider3 {
            width: 100px;
            padding: 0;
            margin: 2px;
        }
    </style>
    <%--<link href="CSS/tabcontent.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="Script/tabcontent.js" type="text/javascript"></script>--%>
    <script type="text/javascript" language="javascript">
        function checkChange() {
            var curState = $("#chkCapApp").is(':checked');
            if (curState) {

                $("#txtTransferWithdrawal").val(0);
                //$("#txtTransferWithdrawal").prop("readonly", true);
                $("#lblTransferWithdrawal").attr("style", "visibility: hidden");
                $("#txtTransferWithdrawal").attr("style", "visibility: hidden");
            }
            else {
                //$("#txtTransferWithdrawal").prop("readonly", false);
                $("#lblTransferWithdrawal").attr("style", "visibility: visible");
                $("#txtTransferWithdrawal").attr("style", "visibility: visible");
            }
        }

         function OpenAlert(messageBody) {
            debugger;
             $.confirm({
                 'title': 'Message',
                 'message': messageBody,
                 'buttons': {
                   
                     'OK': {
                         'class': 'button',
                         'action': function () { }	
                     }
                 }
             });
             return false;
        };
        $(function () {


            checkChange();
            $("#chkCapApp").change(checkChange);


            $("#txtfromDate").datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "img/calenderb.jpg",
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
            });

            $("#txtToDate").datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -2
                //                 maxDate: 0
            });

            $("#txtvalason").datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#txtIniToDate").datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#txtLumpfromDate").datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            $("#txtLumpToDate").datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });

            setFromDateAfterPostBack();

        });
    </script>
    <style type="text/css">
        .navbar {
            *position: relative;
            *z-index: 2;
            margin-bottom: 20px;
            overflow: visible;
        }

        .top_bar {
            margin-bottom: 5px;
            margin-top: 10px;
            top: 0px;
            left: 0px;
        }

        .container-fluid {
            padding-right: 20px;
            padding-left: 20px;
            *zoom: 1;
        }

            .container-fluid:before, .container-fluid:after {
                display: table;
                line-height: 0;
                content: "";
            }

        .top_bar .inner {
            background: #1c75bc;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
            padding: 3px 10px;
            text-align: left;
            color: #fff;
            font-family: 'Roboto', sans-serif;
        }

            .top_bar .inner .left {
                font-size: 20px;
                font-weight: 700;
                line-height: 20px;
                padding: 10px;
            }

        .top_bar .bottom {
            border: 1px solid #dbdbdb;
            border-top: 0;
            margin-bottom: 10px;
            padding: 10px;
        }

            .top_bar .bottom .left {
                width: 30%;
                float: left;
                text-align: left;
            }

            .top_bar .bottom .right {
                width: 70%;
                float: right;
                text-align: right;
            }

        body {
            margin: 0;
            font-family: 'Roboto', sans-serif;
            font-size: 15px;
            line-height: 20px;
            color: #333;
            background-color: #fff;
        }

        select {
            width: 100%;
            background-color: #fff;
            border-radius: 2px;
            border: 1px solid #d5d5d5;
            font-size: 15px;
            padding: 5px;
        }

        p {
            margin: 0 0 10px;
        }

        select, input[type="file"] {
            height: 32px;
            *margin-top: 4px;
            line-height: 30px;
            font-size: 15px;
        }

        select, textarea, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input {
            margin-bottom: 10px;
        }

        textarea, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input {
            background-color: #fff;
            border: 1px solid #ccc;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,0.075);
            -moz-box-shadow: inset 0 1px 1px rgba(0,0,0,0.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,0.075);
            -webkit-transition: border linear .2s,box-shadow linear .2s;
            -moz-transition: border linear .2s,box-shadow linear .2s;
            -o-transition: border linear .2s,box-shadow linear .2s;
            transition: border linear .2s,box-shadow linear .2s;
        }

        .block1 {
            /* border: 1px solid #ccc; */
            background: #efefef70;
            /* margin-left: 5px; */
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            padding: 10px;
            /* width: 340px; */
        }

        input {
            height: 20px;
            padding: 5px;
            width: 99%;
        }

        .table th {
            font-weight: bold;
            color: #428bca;
            background: #ffffff;
            white-space: nowrap;
            font-size: 12px;
            background: #f9f9f9;
            padding: 6px;
        }

        .table td {
            color: #333333;
            font-size: 12px;
            padding: 6px;
        }
    </style>
</head>
<body>
    <div style="width: 100%">
        <form id="form1" runat="server">
            <div class="navbar" align="left" style="margin-left: 7px;">
                <div class="navbar top_bar">
                    <div class="container-fluid">
                        <div class="inner">
                            <div class="left">
                                Return Calculator
                            </div>
                        </div>
                        <div class="bottom">
                            <div class="">
                                <a href="https://mfi360.icrainsights.com/Account/login"
                                    target="_blank">
                                    <img src="img/logo_mfi360.png" /></a>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="row-fluid">
                </div>
            </div>

            <div id="newDiv" runat="server">
                <table width="100%" border="0" align="left" cellpadding="0"
                    cellspacing="0">
                    <tr align="left">
                        <td valign="top">
                            <div class="pageCont">
                                <div class="prdHold">
                                    <div class="blueBox">
                                        <div class="Boxborder">
                                            <table width="100%" border="0" align="left" cellpadding="0"
                                                cellspacing="0">
                                                <tr align="left">
                                                    <td valign="top">
                                                        <table width="100%" border="0" align="left" cellpadding="0"
                                                            cellspacing="0">
                                                            <tr align="left">
                                                                <td width="1%" valign="top">
                                                                    <img src="IMG/spacer11.gif" width="10" height="1" alt="" />
                                                                </td>
                                                                <td width="98%" valign="top">
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                <table width="98%" border="0" align="center" cellpadding="0"
                                                                                    cellspacing="0" class="block1">
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr>
                                                                                                    <td width="19%" height="30">
                                                                                                        <span class="FieldHead">Investment Mode</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="True"
                                                                                                            CssClass="ddl"
                                                                                                            OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                                                                                                            <asp:ListItem Selected="True">SIP</asp:ListItem>
                                                                                                            <asp:ListItem Selected="False">Lump Sum</asp:ListItem>
                                                                                                            <%-- <asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>
                                                                                                        <asp:ListItem Selected="False">SWP</asp:ListItem>
                                                                                                        <asp:ListItem Selected="False">STP</asp:ListItem>--%>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>

                                                                                                <tr>


                                                                                                    <td width="19%" height="30"><span class="FieldHead">Mutual
                                                                                                        Fund</span></td>
                                                                                                    <td>
                                                                                                        <%--<select class="span6">
                                                                                                      <option>HDFC Mutual Fund</option>
                                                                                                      <option>HDFC Mutual Fund</option>
                                                                                                    </select>--%>
                                                                                                        <asp:DropDownList ID="ddlFundHouse" runat="server"
                                                                                                            AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged"
                                                                                                            CssClass="span6" Style="max-height: 50px;">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>

                                                                                                <tr align="left" id="trCategory" runat="server" visible="true">
                                                                                                    <td height="30" align="left" valign="middle" width="23%">
                                                                                                        <span class="FieldHead">Category</span>
                                                                                                    </td>
                                                                                                    <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                        <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true"
                                                                                                            DataTextField="Nature"
                                                                                                            DataValueField="Nature" CssClass="ddl" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="left">
                                                                                                    <td height="30" align="left" valign="middle" width="23%">
                                                                                                        <span class="FieldHead">
                                                                                                            <asp:Label ID="lblSchemeName" runat="server" Text="Scheme Name"></asp:Label></span>
                                                                                                    </td>
                                                                                                    <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                        <%-- <asp:DropDownList ID="ddlscheme"  runat="server" CssClass="ddl"  Width="" AutoPostBack="True"
                                                                                                        OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                                                                                   
                                                                                                    </asp:DropDownList>--%>
                                                                                                        <asp:DropDownList ID="ddlscheme" runat="server" CssClass="ddl"
                                                                                                            AutoPostBack="True"
                                                                                                            OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                                                                                            <asp:ListItem Value="0" Selected="true">Select</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="left" id="trInception1" runat="server">
                                                                                                    <td height="30" align="left" valign="middle">
                                                                                                        <%-- <span id="Span2" class="FieldHead">Scheme Inception Date</span>--%>
                                                                                                        <asp:Label ID="LabelInception" runat="server" CssClass="FieldHead"
                                                                                                            Text="Inception Date"></asp:Label>
                                                                                                    </td>
                                                                                                    <td colspan="2" align="left" valign="middle">
                                                                                                        <%--<asp:Label ID="SIPSchDt" runat="server" CssClass="FieldHead" Width="150px"></asp:Label>--%>
                                                                                                        <asp:TextBox ID="SIPSchDt" runat="server" CssClass="ddl_3"
                                                                                                            ReadOnly="true" Text=""></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="left" id="trTransferTo" runat="server" visible="false">
                                                                                                    <td height="30" valign="middle" width="145">
                                                                                                        <span class="FieldHead">Transfer To</span>
                                                                                                    </td>
                                                                                                    <td width="535" align="left" valign="middle" style="height: 25px">
                                                                                                        <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True"
                                                                                                            CssClass="ddl" Width="360px" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged">
                                                                                                        </asp:DropDownList>
                                                                                                        &nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="left" id="trInception" runat="server">
                                                                                                    <td height="27" align="left" valign="middle">
                                                                                                        <span id="Span1" class="FieldHead">Inception Date</span>
                                                                                                    </td>
                                                                                                    <td colspan="2" align="left" valign="middle">
                                                                                                        <%--<asp:Label ID="SIPSchDt" runat="server" CssClass="FieldHead" Width="150px"></asp:Label>--%>
                                                                                                        <asp:TextBox ID="SIPSchDt2" runat="server" CssClass="ddl_3"
                                                                                                            ReadOnly="true" Text=""></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="left" id="trBenchmark" runat="server">
                                                                                                    <td align="left" valign="middle" width="23%">
                                                                                                        <span id="Span3" class="FieldHead">Benchmark</span>
                                                                                                    </td>
                                                                                                    <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                        <%--<asp:DropDownList ID="ddlsipbnmark" runat="server" CssClass="ddl" Width="190px" Enabled="True">
                                                                                                    </asp:DropDownList>--%>
                                                                                                        <asp:TextBox ID="txtddlsipbnmark" runat="server" CssClass="ddl_3"
                                                                                                            Text="" ReadOnly="true"></asp:TextBox>
                                                                                                        <%-- <b>
                                                                                                        <asp:Label ID="lblddlsipbnmark" runat="server" CssClass="FieldHead" Text=""></asp:Label></b>--%>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" id="trInitialInvst" runat="server"
                                                                                        visible="false">
                                                                                        <td valign="top">
                                                                                            <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr align="left">
                                                                                                    <td width="23%" height="30" class="FieldHead">Initial
                                                                                                        Amount (Rs.)
                                                                                                    </td>
                                                                                                    <td width="22%" valign="middle">
                                                                                                        <asp:TextBox ID="txtiniAmount" runat="server" CssClass="ddl_3"
                                                                                                            MaxLength="10" ReadOnly="false"
                                                                                                            Text="" Style="width: 90px;"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td width="22%" class="FieldHead">Initial Date
                                                                                                    </td>
                                                                                                    <td width="33%" valign="middle" colspan="3">
                                                                                                        <asp:TextBox ID="txtIniToDate" runat="server" CssClass="ddl_3"
                                                                                                            Style="width: 90px;"
                                                                                                            onMouseDown="Javascript: setDate();">
                                                                                                        </asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" id="trSipInvst" runat="server" visible="true">
                                                                                        <td valign="top">
                                                                                            <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr align="left">
                                                                                                    <td colspan="2">
                                                                                                        <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                            cellspacing="0">
                                                                                                            <tr align="left">
                                                                                                                <td height="30" valign="middle" width="23%">
                                                                                                                    <span>
                                                                                                                        <asp:Label ID="lblInstallmentAmt" runat="server" Text="Installment Amount (Rs.)"
                                                                                                                            class="FieldHead"></asp:Label></span>
                                                                                                                </td>
                                                                                                                <td valign="middle" width="80%" colspan="3">
                                                                                                                    <asp:TextBox ID="txtinstall" CssClass="ddl_3" MaxLength="10"
                                                                                                                        Text="" runat="server" ReadOnly="false" onmousedown="Javascript: setDate(); "
                                                                                                                        onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>

                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td height="30" valign="middle" style="display: none">
                                                                                                        <asp:Label ID="lblTransferWithdrawal" Visible="true"
                                                                                                            runat="server" class="FieldHead" Text="Withdrawal Amount"></asp:Label>
                                                                                                    </td>
                                                                                                    <td valign="middle" style="display: none">
                                                                                                        <asp:TextBox ID="txtTransferWithdrawal" Visible="true"
                                                                                                            runat="server" CssClass="ddl_3" MaxLength="14"
                                                                                                            onChange="Javascript: checkInvestedValue();" Text=""
                                                                                                            ReadOnly="false" Style="width: 90px;"></asp:TextBox>&nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <%-- <tr align="left" id="trTransferWithdrawal" runat="server" visible="false">
                                                                                                <td height="30" align="left" valign="middle" width="23%">
                                                                                                    <span class="FieldHead">
                                                                                                        <asp:Label ID="lblTransferWithdrawal" runat="server" Text="Withdrawal Amount"></asp:Label></span>
                                                                                                </td>
                                                                                                <td width="77%" colspan="3" align="left" valign="middle">
                                                                                                    <asp:TextBox ID="txtTransferWithdrawal" runat="server" CssClass="ddl_3" MaxLength="14"
                                                                                                        Text="" ReadOnly="false" Style="width: 90px;"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>--%>
                                                                                                <tr align="left">
                                                                                                    <td colspan="2">
                                                                                                        <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                            cellspacing="0">
                                                                                                            <td height="30" valign="middle" width="22.5%">
                                                                                                                <span class="FieldHead">Frequency</span>
                                                                                                            </td>
                                                                                                            <td width="25%" valign="middle">
                                                                                                                <asp:DropDownList ID="ddPeriod_SIP" runat="server"
                                                                                                                    CssClass="ddl_3" AutoPostBack="True" OnSelectedIndexChanged="ddPeriod_SIP_SelectedIndexChanged">
                                                                                                                    <%-- <asp:ListItem Value="Daily" Selected="True">Daily</asp:ListItem>
                                                                                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>--%>
                                                                                                                    <%-- <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                                                                        <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>--%>
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td width="2%"></td>
                                                                                                            <td class="FieldHead" width="23%" style="text-align: center">
                                                                                                                <asp:Label ID="lblDiffDate" runat="server" Text="SIP Date"></asp:Label>
                                                                                                            </td>
                                                                                                            <td valign="middle" colspan="3" width="25%">
                                                                                                                <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="ddl_3">
                                                                                                                    <%--  <asp:ListItem Value="1">1st</asp:ListItem>
                                                                                                        <asp:ListItem Value="7">7th</asp:ListItem>
                                                                                                        <asp:ListItem Value="14">14th</asp:ListItem>
                                                                                                        <asp:ListItem Value="21">21st</asp:ListItem>
                                                                                                        <asp:ListItem Value="28">28th</asp:ListItem>--%>
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </table>
                                                                                                    </td>

                                                                                                </tr>
                                                                                            </table>
                                                                                            <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                cellspacing="0">
                                                                                                <tr align="left">
                                                                                                    <td colspan="3">
                                                                                                        <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                            cellspacing="0">
                                                                                                            <tr align="left">
                                                                                                                <td>
                                                                                                                    <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                                        cellspacing="0">
                                                                                                                        <tr align="left">
                                                                                                                            <td width="20%" height="30" valign="middle" class="FieldHead">
                                                                                                                                From Date
                                                                                                                            </td>
                                                                                                                            <td width="22%" valign="middle">
                                                                                                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="ddl_3"
                                                                                                                                    Width="82%"> </asp:TextBox>
                                                                                                                                <%-- &nbsp;<img src="IMG/calender2.gif" id="img1" onmousedown="Javascript: setDate();"
                                                                                                        style="vertical-align: middle" />--%>
                                                                                                                            </td>
                                                                                                                            <td width="22%" valign="middle" class="FieldHead" style="text-align: center">
                                                                                                                                To Date
                                                                                                                            </td>
                                                                                                                            <td width="22%" valign="middle">
                                                                                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="ddl_3"
                                                                                                                                    Style="width: 90px;" onChange="Javascript: setDateValueAsOn(); ">
                                                                                                                                </asp:TextBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="left">
                                                                                                    <td>
                                                                                                        <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                            cellspacing="0">
                                                                                                            <tr align="left">
                                                                                                                <td width="20%" height="30" valign="middle" class="FieldHead">
                                                                                                                    Value as on Date
                                                                                                                </td>
                                                                                                                <td width="26%" valign="middle">
                                                                                                                    <asp:TextBox ID="txtvalason" runat="server" CssClass="ddl_3"
                                                                                                                        Width="69%">
                                                                                                                    </asp:TextBox>
                                                                                                                </td>
                                                                                                                <td width="20%" valign="middle" class="FieldHead" colspan="2">
                                                                                                                    <small>('Value as on Date' should be greater than 'To
                                                                                                                        Date')</small>
                                                                                                                </td>
                                                                                                                <td style="width:20%; text-align:right" >
                                                                                                                    <a href="https://mfi360.icrainsights.com/SIP/SIPCalculator" target="_blank"
                                                                                                                        style="color: #cc885a; font-size: 12px; font-weight: bold; text-decoration: underline">More Schemes and Features</a>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" id="trLumpInvst" runat="server" visible="false">
                                                                                        <td align="left" valign="top">
                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td colspan="4">
                                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                            <tr>
                                                                                                                <td width="23%" height="30" class="FieldHead">Investment
                                                                                                                    Amount (Rs.)
                                                                                                                </td>
                                                                                                                <td colspan="3" valign="middle" width="80%">
                                                                                                                    <asp:TextBox ID="txtinstallLs" value="" MaxLength="10"
                                                                                                                        runat="server" CssClass="ddl_3" onmousedown="Javascript: setDate();"
                                                                                                                        onChange="Javascript: checkInvestedValue();">
                                                                                                                    </asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>

                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="4">
                                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                            <tr>
                                                                                                                <td width="20%" height="30" valign="middle" class="FieldHead">
                                                                                                                    Start Date
                                                                                                                </td>
                                                                                                                <td width="22%" valign="middle">
                                                                                                                    <asp:TextBox ID="txtLumpfromDate" runat="server" CssClass="ddl_3"
                                                                                                                        Width="82%" onMouseDown="Javascript: setDate();">
                                                                                                                    </asp:TextBox>
                                                                                                                </td>
                                                                                                                <td width="22%" valign="middle" class="FieldHead" style="text-align: center;">
                                                                                                                    End Date
                                                                                                                </td>
                                                                                                                <td width="23%" valign="middle">
                                                                                                                    <asp:TextBox ID="txtLumpToDate" runat="server" CssClass="ddl_3"
                                                                                                                        Width="82%">
                                                                                                                    </asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>

                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="stpCalcOption" valign="middle" class="FieldHead"
                                                                                        visible="false" runat="server">
                                                                                        <td height="40">
                                                                                            <asp:CheckBox ID="chkCapApp" TextAlign="Left" Text="Capital Appreciation Systematic Transfer Plan"
                                                                                                runat="server" ClientIDMode="Static" />
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr>
                                                                                        <td>&nbsp;
                                                                                        <%--<div style="background-color: #4395cd; height: 5px; width: 100%">
                                                                                            </div>
                                                                                            <img style="margin-bottom: 10px; margin-left: 85px" src="IMG/down-pointer-image.png"
                                                                                                height="11" width="14" alt="" />--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left" valign="middle">
                                                                            <td style="padding-top: 10px;">
                                                                                <table width="99%" border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr align="left">

                                                                                        <td valign="middle" style="text-align: right" colspan="3">
                                                                                            <asp:ImageButton ID="sipbtnshow" runat="server" ImageUrl="img/submit.jpg"
                                                                                                OnClick="sipbtnshow_Click" Width="83" Height="30">
                                                                                            </asp:ImageButton>
                                                                                            <span style="padding-left: 10px;">
                                                                                                <asp:ImageButton ID="sipbtnreset" runat="server" ImageUrl="img/reset.jpg"
                                                                                                    OnClick="sipbtnreset_Click" Width="71" Height="29">
                                                                                                </asp:ImageButton></span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left" valign="top">
                                                                            <td>
                                                                                <div id="resultDiv" runat="server" visible="false">
                                                                                    <table width="100%" border="0" align="center" cellpadding="0"
                                                                                        cellspacing="0" style="display: none">
                                                                                        <tr align="left">
                                                                                            <td width="3%" height="20" align="center" valign="middle">
                                                                                                <img src="IMG/arw.gif" width="4" height="8" />
                                                                                            </td>
                                                                                            <td width="97%" height="25" valign="middle">
                                                                                                <%-- <span class="rslt_text">Investment amount per month : Rs<strong> 5000</strong></span>--%>
                                                                                                <asp:Label ID="lblInvestment" CssClass="rslt_text"
                                                                                                    runat="server" Text="Investment amount per month"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left">
                                                                                            <td width="3%" align="center" valign="middle">
                                                                                                <img src="IMG/arw.gif" width="4" height="8" />
                                                                                            </td>
                                                                                            <td height="25" align="left" valign="middle">
                                                                                                <%--<span class="rslt_text">Total Investment Amount : Rs <strong>120000</strong></span>--%>
                                                                                                <asp:Label ID="lblTotalInvst" CssClass="rslt_text"
                                                                                                    runat="server" Text="Total Investment Amount"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="3%" height="25" align="center" valign="middle">
                                                                                                <img src="IMG/arw.gif" width="4" height="8" />
                                                                                            </td>
                                                                                            <td align="left" valign="middle">
                                                                                                <%--<span class="rslt_text">On 05/07/2012, the value of your total investment Rs 120000
                                                                                        would be Rs <strong>131779.37</strong></span>--%>
                                                                                                <asp:Label ID="lblInvstvalue" CssClass="rslt_text"
                                                                                                    runat="server" Text="On Date C, the Scheme value of your total investment Rs Y would be Rs Z">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="3%" height="25" align="center" valign="middle">
                                                                                                <img src="IMG/arw.gif" width="4" height="8" />
                                                                                            </td>
                                                                                            <td align="left" valign="middle">
                                                                                                <asp:Label ID="lblAbsoluteReturn" CssClass="rslt_text"
                                                                                                    runat="server" Text="Absolute return from Date  to Date  is X%">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="3%" height="25" align="center" valign="middle">
                                                                                                <img src="IMG/arw.gif" width="4" height="8" />
                                                                                            </td>
                                                                                            <td height="25" align="left" valign="middle">
                                                                                                <%--<span class="rslt_text">XIsRR return of Investment from 01/07/2010 to 01/07/2012 is <strong>
                                                                                        9.17%</strong> </span>--%>
                                                                                                <asp:Label ID="lblCagrReturn" CssClass="rslt_text"
                                                                                                    runat="server" Text="XIRR return from Date  to Date  is X%">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="3%" height="25" align="center" valign="middle">
                                                                                                <img src="IMG/arw.gif" width="4" height="8" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblIfInvested" CssClass="rslt_text"
                                                                                                    runat="server" Text="Had you invested Rs Y at Date A, the total value of this investment at Date C would have become Q">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td height="25" colspan="2" align="center" valign="middle">
                                                                                                <div align="left">
                                                                                                    &nbsp;&nbsp;&nbsp;<b>View Historical Fund Performance
                                                                                                        below:<b />
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <table width="100%" id="firstTable" class="table table-striped table-bordered">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div id="divSummary" runat="server">
                                                                                                    <asp:GridView ID="gvFirstTable" runat="server" AutoGenerateColumns="False" Width="100%" HeaderStyle-CssClass="grdHead" Visible="false" AlternatingRowStyle-CssClass="grdRow"
                                                                                                        RowStyle-CssClass="grdRow" BorderWidth="1px" BorderColor="#dddddd"
                                                                                                        HeaderStyle-HorizontalAlign="Right">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="right">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Scheme") == DBNull.Value) ? "--" : Eval("Scheme").ToString()%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Left" CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Units Purchased" ItemStyle-HorizontalAlign="Right">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Total_unit") == DBNull.Value || Eval("Total_unit").ToString() == "0") ? "--" : Math.Round(Convert.ToDouble(Eval("Total_unit")), 0).ToString("n0")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Amount Invested (A)"
                                                                                                                ItemStyle-HorizontalAlign="Right">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Total_amount") == DBNull.Value || Eval("Total_amount").ToString() == "0") ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 0).ToString("n0")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Investment Value as on Date (B)"
                                                                                                                ItemStyle-HorizontalAlign="Right">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Present_Value") == DBNull.Value || Eval("Present_Value").ToString() == "0") ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Total Profit (B-A)"
                                                                                                                ItemStyle-HorizontalAlign="Right">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Profit_Sip") == DBNull.Value || Eval("Profit_Sip").ToString() == "0") ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 0).ToString("n0")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Abs. Returns" ItemStyle-HorizontalAlign="Right">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("ABSOLUTERETURN") == DBNull.Value || Eval("ABSOLUTERETURN").ToString() == "0") ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Returns*" HeaderStyle-HorizontalAlign="right"
                                                                                                                HeaderStyle-VerticalAlign="Top">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Yield") == DBNull.Value || Eval("Yield").ToString() == "0") ? "--" : (Eval("Yield").ToString() + "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                    <asp:GridView ID="gvSWPSummaryTable" runat="server"
                                                                                                        AutoGenerateColumns="False" Width="100%"
                                                                                                        HeaderStyle-CssClass="grdHead" Visible="false"
                                                                                                        AlternatingRowStyle-CssClass="grdRow"
                                                                                                        RowStyle-CssClass="grdRow" BorderWidth="1px" BorderColor="#d52e42">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Left" CssClass="" Width="210px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Amount Invested (A)">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Amount Withdrawn (B)">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Present Value (C)">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Total Profit (B+C-A)">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# "<img src='img/rimg.png' style='vertical-align:middle;'> "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Abs. Returns">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Returns *" HeaderStyle-HorizontalAlign="right"
                                                                                                                HeaderStyle-VerticalAlign="Top">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                    <asp:GridView ID="gvSTPToSummaryTable" runat="server"
                                                                                                        AutoGenerateColumns="False"
                                                                                                        Width="100%" HeaderStyle-CssClass="grdHead" Visible="false"
                                                                                                        AlternatingRowStyle-CssClass="grdRow"
                                                                                                        RowStyle-CssClass="grdRow" BorderWidth="1px" BorderColor="#d52e42">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Left" CssClass="" Width="40%" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Total Amount Invested">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                                                                    <%--Eval("Total_Amount_Invested").ToString()--%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" Width="24%" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Present Value">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
                                                                                                                    <%--TwoDecimal(Eval("Present_Value").ToString())--%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" Width="18%" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Yield">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="borderbottom" Width="18%" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                    <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                        cellspacing="0">
                                                                                                        <tr align="left">
                                                                                                            <td valign="top" colspan="2">
                                                                                                                <asp:GridView ID="GridViewLumpSum" runat="server" Width="100%"
                                                                                                                    CssClass="grdRow" RowStyle-CssClass="grdRow"
                                                                                                                    AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                                                                    AutoGenerateColumns="false"
                                                                                                                    BorderWidth="1px" BorderColor="#dddddd">
                                                                                                                    <Columns>
                                                                                                                        <asp:TemplateField>
                                                                                                                            <ItemTemplate>
                                                                                                                                <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle HorizontalAlign="Left" CssClass="" Width="230px" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="Amount Invested (A)"
                                                                                                                            HeaderStyle-HorizontalAlign="right">
                                                                                                                            <ItemTemplate>
                                                                                                                                <%# (Eval("InvestedAmount") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedAmount")).ToString("n2")%>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Right" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="Investment Value(B)"
                                                                                                                            HeaderStyle-HorizontalAlign="right">
                                                                                                                            <ItemTemplate>
                                                                                                                                <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Right" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="Profit from Investment(B-A)"
                                                                                                                            HeaderStyle-HorizontalAlign="right">
                                                                                                                            <ItemTemplate>
                                                                                                                                <%# (Eval("Profit") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("Profit")).ToString("n2")%>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Right" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="Return *" HeaderStyle-VerticalAlign="Top"
                                                                                                                            HeaderStyle-HorizontalAlign="right">
                                                                                                                            <ItemTemplate>
                                                                                                                                <%# (Eval("Return") == DBNull.Value) ? "--" : Eval("Return").ToString() + ((Eval("Return").ToString() == "N/A") ? "" : "%")%>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Right" />
                                                                                                                        </asp:TemplateField>
                                                                                                                    </Columns>
                                                                                                                </asp:GridView>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="2">&nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <div class="" style="text-align: right; margin-bottom: 10px>
                                                                                        <span style="text-align:right">
                                                                                            <a href="https://mfi360.icrainsights.com/SIP/SIPCalculator" target="_blank" style="color: #cc885a; font-size: 12px; font-weight: bold; text-decoration:underline">Detailed Report</a>
                                                                                        </span>
                                                                                    </>
                                                                                    <table width="100%" id="TableRemark" class="table table-striped table-bordered"cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div id="divTab" runat="server" visible="false">
                                                                                                    <table width="100%" border="0" align="center" cellpadding="0"
                                                                                                        cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table width="100%" border="0" align="left" cellpadding="0"
                                                                                                                    cellspacing="0">
                                                                                                                    <tr align="left">
                                                                                                                        <td align="left" valign="top">
                                                                                                                            <ul runat="server" id="countrytabs" class="shadetabs"
                                                                                                                                visible="false">
                                                                                                                                <li>
                                                                                                                                    <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click">View Detail Report</asp:LinkButton>
                                                                                                                                </li>
                                                                                                                                <li>
                                                                                                                                    <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click">View Graph</asp:LinkButton>
                                                                                                                                </li>
                                                                                                                                <li>
                                                                                                                                    <asp:LinkButton ID="lnkTab3" runat="server" OnClick="lnkTab3_Click">View Historical Performance</asp:LinkButton>
                                                                                                                                </li>
                                                                                                                                <li>
                                                                                                                                    <asp:LinkButton ID="lnkTab4" runat="server" Visible="false"
                                                                                                                                        OnClick="lnkTab4_Click">View PDF Report</asp:LinkButton>
                                                                                                                                </li>
                                                                                                                            </ul>
                                                                                                                            <div style="width: 100%; padding-top: 5px;">
                                                                                                                                <asp:MultiView ID="MultiView1" runat="server">
                                                                                                                                    <table width="100%" class="table table-striped table-bordered">
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <table class="table table-striped table-bordered" style="border-color: #DDDDDD; border-style: solid; width: 100%; border-collapse: collapse;">
                                                                                                                                                    <asp:View ID="View1" runat="server">
                                                                                                                                                        <div id="DetailDiv" runat="server">
                                                                                                                                                            <%--<div id="country1" class="tabcontent">--%>
                                                                                                                                                            <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False"
                                                                                                                                                                Width="100%"
                                                                                                                                                                HeaderStyle-CssClass="grdHead" BorderColor="#dddddd">
                                                                                                                                                                <Columns>
                                                                                                                                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[2]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[1]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderlefft" Width="95px" HorizontalAlign="Center" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="NAV" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Dividend (%)" HeaderStyle-HorizontalAlign="right">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="Right" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Scheme Units" HeaderStyle-HorizontalAlign="right">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("Scheme_units") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_units")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="Right" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cumulative Units" HeaderStyle-HorizontalAlign="right">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("SCHEME_UNITS_CUMULATIVE") == DBNull.Value) ? "--" :Math.Round(Convert.ToDouble( Eval("SCHEME_UNITS_CUMULATIVE")),2).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="Right" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="SIP Amount" HeaderStyle-HorizontalAlign="right">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("Scheme_cashflow") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_cashflow")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="Right" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <%-- <asp:TemplateField HeaderText="Cumulative Amount">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("CUMULATIVE_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMULATIVE_AMOUNT")), 2).ToString("n2")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle CssClass="borderlefft" Width="79px" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="Cumulative Amount <br/>Benchmark">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("Index_Value_amount") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Index_Value_amount")).ToString("n2")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                </asp:TemplateField>--%>
                                                                                                                                                                </Columns>
                                                                                                                                                            </asp:GridView>

                                                                                                                                                            <asp:GridView ID="swpGridView" runat="server" AutoGenerateColumns="False"
                                                                                                                                                                Width="98%"
                                                                                                                                                                HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow"
                                                                                                                                                                RowStyle-CssClass="grdRow"
                                                                                                                                                                BorderWidth="1px" BorderColor="#D52E42">
                                                                                                                                                                <Columns>
                                                                                                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%#  (Eval("DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("DATE").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2") %>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cashflow">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("FINAL_INVST_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT")).ToString("n2") %>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <%-- <asp:TemplateField HeaderText="Investment Amount">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("INVST_AMOUNT")).ToString("n2")  %>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                                                                            </asp:TemplateField>--%>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("CUMILATIVE_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("CUMILATIVE_UNITS")).ToString("n2") %>
                                                                                                                                                                            <%-- Math.Round(Convert.ToDouble(Eval("")), 2).ToString()--%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cumulative Amount">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("CUMILATIVE_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("CUMILATIVE_AMOUNT")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                </Columns>
                                                                                                                                                            </asp:GridView>
                                                                                                                                                            <div id="divSTP" runat="server" visible="False">
                                                                                                                                                                <br />
                                                                                                                                                                <b>From Scheme:
                                                                                                                                                            <%= ddlscheme.SelectedItem.Text %>
                                                                                                                                                                </b>
                                                                                                                                                                <br />
                                                                                                                                                                <br />
                                                                                                                                                                <asp:GridView ID="stpFromGridview" runat="server" AutoGenerateColumns="False"
                                                                                                                                                                    Width="98%"
                                                                                                                                                                    HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow"
                                                                                                                                                                    RowStyle-CssClass="grdRow"
                                                                                                                                                                    BorderWidth="1px" BorderColor="#D52E42" OnInit="stpFromGridview_Init">
                                                                                                                                                                    <Columns>
                                                                                                                                                                        <%-- <asp:TemplateField HeaderText="From Scheme">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%#  (Eval("FROM_SCHEME_NAME") == DBNull.Value) ? "--" : Eval("FROM_SCHEME_NAME").ToString()%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                            </asp:TemplateField>--%>
                                                                                                                                                                        <asp:TemplateField HeaderText="Date">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%-- <%#  (Eval("FROM_DATE") == DBNull.Value) ? "--" : Eval("FROM_DATE").ToString()%>--%>
                                                                                                                                                                                <%#  (Eval("FROM_DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("FROM_NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FROM_NAV")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("DIVIDEND_BONUS_FROM") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS_FROM").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS_FROM")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cashflow">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("INVST_AMOUNT")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Redeemed Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("REDEEM_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("REDEEM_UNITS")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <%--                                                                               <asp:TemplateField HeaderText="Investment Amount">
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Eval("INVST_AMOUNT").ToString()%>
                                                                                                                                            </ItemTemplate>
                                                                                                                                        </asp:TemplateField>--%>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_UNITS_FROM") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_UNITS_FROM")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Investment Value">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_AMOUNT_FROM") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_AMOUNT_FROM")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                    </Columns>
                                                                                                                                                                </asp:GridView>
                                                                                                                                                                <br />
                                                                                                                                                                <b>To Scheme:
                                                                                                                                                            <%= ddlschtrto.SelectedItem.Text %>
                                                                                                                                                                </b>
                                                                                                                                                                <br />
                                                                                                                                                                <br />
                                                                                                                                                                <asp:GridView ID="stpToGridview" runat="server" AutoGenerateColumns="False"
                                                                                                                                                                    Width="98%"
                                                                                                                                                                    HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow"
                                                                                                                                                                    RowStyle-CssClass="grdRow"
                                                                                                                                                                    BorderWidth="1px" BorderColor="#D52E42">
                                                                                                                                                                    <Columns>
                                                                                                                                                                        <%-- <asp:TemplateField HeaderText="To Scheme">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%#  (Eval("TO_SCHEME_NAME") == DBNull.Value) ? "--" : Eval("TO_SCHEME_NAME").ToString()%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                            </asp:TemplateField>--%>
                                                                                                                                                                        <asp:TemplateField HeaderText="Date">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%-- <%#  (Eval("TO_DATE") == DBNull.Value) ? "--" : Eval("TO_DATE").ToString()%>--%>
                                                                                                                                                                                <%#  (Eval("TO_DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("TO_NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("TO_NAV")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("DIVIDEND_BONUS_TO") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS_TO").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS_TO")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cashflow">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("Amount") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Amount")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="No. of Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("NO_OF_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NO_OF_UNITS")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_UNITS_TO") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_UNITS_TO")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Investment Value">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_AMOUNT_TO") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_AMOUNT_TO")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                    </Columns>
                                                                                                                                                                </asp:GridView>
                                                                                                                                                            </div>
                                                                                                                                                            <%--</div>--%>
                                                                                                                                                            <br />
                                                                                                                                                            <%-- <table width="100%">
                                                                                                                                                            <tr>
                                                                                                                                                                <td align="right">
                                                                                                                                                                    <asp:ImageButton ID="btnExcelCalculation" runat="server"
                                                                                                                                                                        ImageUrl="~/BirlaSunlife/IMG/excell.png"
                                                                                                                                                                        ToolTip="Download Excel" Text="Show Excel Calculation"
                                                                                                                                                                        Visible="true" OnClick="ExcelCalculation_Click" />
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                        </table>--%>
                                                                                                                                                        </div>
                                                                                                                                                    </asp:View>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <asp:View ID="View2" runat="server">
                                                                                                                                                    <%--  <div id="country2" class="tabcontent">--%>
                                                                                                                                                    <div id="divshowChart" runat="server" visible="true"
                                                                                                                                                        style="width: 100%;">
                                                                                                                                                        <asp:Chart ID="chrtResult" runat="server" AlternateText="BirlaSunlife"
                                                                                                                                                            Visible="true"
                                                                                                                                                            BorderlineWidth="2" Width="650px" Height="620px"
                                                                                                                                                            IsSoftShadows="false">
                                                                                                                                                            <%--BackGradientStyle="Center"  BorderlineColor="RoyalBlue" BackColor="Gray"--%>
                                                                                                                                                            <Titles>
                                                                                                                                                                <asp:Title Font="Verdana, 14pt, style=Bold">
                                                                                                                                                                </asp:Title>
                                                                                                                                                            </Titles>
                                                                                                                                                            <Legends>
                                                                                                                                                                <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend"
                                                                                                                                                                    BackColor="Transparent"
                                                                                                                                                                    Alignment="Center" LegendStyle="Row" Docking="Bottom"
                                                                                                                                                                    Font="Verdana, 8.25pt, style=Bold">
                                                                                                                                                                </asp:Legend>
                                                                                                                                                            </Legends>
                                                                                                                                                            <ChartAreas>
                                                                                                                                                                <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor=""
                                                                                                                                                                    AlignmentStyle="PlotPosition"
                                                                                                                                                                    BackSecondaryColor="White" BackColor="White" ShadowColor="Transparent"
                                                                                                                                                                    BackGradientStyle="Center"
                                                                                                                                                                    BackHatchStyle="None" BorderDashStyle="Solid">
                                                                                                                                                                    <%--BackImageTransparentColor="#CCCCFF"--%>
                                                                                                                                                                    <%--<Area3DStyle Enable3D="false" Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                                                                                                                    WallWidth="0" IsClustered="False"></Area3DStyle>--%>
                                                                                                                                                                    <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="Time period">
                                                                                                                                                                        <LabelStyle Format="yyyy" />
                                                                                                                                                                        <%--<ScaleBreakStyle Enabled="false" />
                                                                                                                                                                    <ScaleView SizeType="Years" />--%>
                                                                                                                                                                        <MajorGrid Enabled="false" />
                                                                                                                                                                    </AxisX>
                                                                                                                                                                    <AxisY ArrowStyle="None" ToolTip="Value(Rs.)" TextOrientation="Horizontal"
                                                                                                                                                                        LineColor="#013974">
                                                                                                                                                                        <ScaleBreakStyle Enabled="True" />
                                                                                                                                                                        <MajorGrid Enabled="false" />
                                                                                                                                                                    </AxisY>
                                                                                                                                                                </asp:ChartArea>
                                                                                                                                                            </ChartAreas>
                                                                                                                                                        </asp:Chart>
                                                                                                                                                        <br />
                                                                                                                                                        <asp:Chart ID="chrtResultSTPTO" runat="server" AlternateText="BirlaSunlife"
                                                                                                                                                            Visible="false" BorderlineWidth="2" Width="650px"
                                                                                                                                                            Height="580px" IsSoftShadows="false">
                                                                                                                                                            <Titles>
                                                                                                                                                                <asp:Title Font="Verdana, 14pt, style=Bold">
                                                                                                                                                                </asp:Title>
                                                                                                                                                            </Titles>
                                                                                                                                                            <Legends>
                                                                                                                                                                <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend"
                                                                                                                                                                    BackColor="Transparent"
                                                                                                                                                                    Alignment="Center" LegendStyle="Row" Docking="Bottom"
                                                                                                                                                                    Font="Verdana, 8.25pt, style=Bold">
                                                                                                                                                                </asp:Legend>
                                                                                                                                                            </Legends>
                                                                                                                                                            <ChartAreas>
                                                                                                                                                                <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor=""
                                                                                                                                                                    AlignmentStyle="PlotPosition"
                                                                                                                                                                    BackSecondaryColor="White" BackColor="White" ShadowColor="Transparent"
                                                                                                                                                                    BackGradientStyle="Center"
                                                                                                                                                                    BackHatchStyle="None" BorderDashStyle="Solid">
                                                                                                                                                                    <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="Time period">
                                                                                                                                                                        <LabelStyle Format="yyyy" />
                                                                                                                                                                        <ScaleBreakStyle Enabled="false" />
                                                                                                                                                                        <ScaleView SizeType="Years" />
                                                                                                                                                                        <MajorGrid Enabled="false" />
                                                                                                                                                                    </AxisX>
                                                                                                                                                                    <AxisY ArrowStyle="None" ToolTip="Value(Rs.)" TextOrientation="Horizontal"
                                                                                                                                                                        LineColor="#013974">
                                                                                                                                                                        <ScaleBreakStyle Enabled="True" />
                                                                                                                                                                        <MajorGrid Enabled="false" />
                                                                                                                                                                    </AxisY>
                                                                                                                                                                </asp:ChartArea>
                                                                                                                                                            </ChartAreas>
                                                                                                                                                        </asp:Chart>
                                                                                                                                                        <%--<asp:Chart ID="chrt" runat="server" AlternateText="DWS Sip" Visible="false"
                                                                                                                                                        BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="650px" Height="580px"
                                                                                                                                                        BackGradientStyle="Center" BackColor="Gray" IsSoftShadows="false">
                                                                                                                                                        <Titles>
                                                                                                                                                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Verdana, 12pt, style=Bold" ShadowOffset="3"
                                                                                                                                                                Text="DSP SIP Chart" ForeColor="26, 59, 105">
                                                                                                                                                            </asp:Title>
                                                                                                                                                        </Titles>
                                                                                                                                                        <Legends>
                                                                                                                                                            <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                                                                                                Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Verdana, 8.25pt, style=Bold">
                                                                                                                                                            </asp:Legend>
                                                                                                                                                        </Legends>
                                                                                                                                                        <ChartAreas>
                                                                                                                                                            <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="" AlignmentStyle="PlotPosition"
                                                                                                                                                                BackSecondaryColor="White" BackColor="#ECF4F9" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                                                                                                                BackHatchStyle="None" BorderDashStyle="Solid" BackImageTransparentColor="#CCCCFF">
                                                                                                                                                                <Area3DStyle Enable3D="false" Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                                                                                                                    WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                                                                                                                <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="SWP period">
                                                                                                                                                                    <ScaleBreakStyle Enabled="false" />
                                                                                                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                                                                                </AxisX>
                                                                                                                                                                <AxisY ArrowStyle="None" ToolTip="Amount" LineColor="#013974">
                                                                                                                                                                    <ScaleBreakStyle Enabled="True" />
                                                                                                                                                                </AxisY>
                                                                                                                                                            </asp:ChartArea>
                                                                                                                                                        </ChartAreas>
                                                                                                                                                    </asp:Chart>--%>
                                                                                                                                                        <%-- </div>--%>
                                                                                                                                                    </div>
                                                                                                                                                    <%--   <asp:Image runat="server" ID="imgchrtResult"  DescriptionUrl="~/DSP/IMG/excel.png" />--%>
                                                                                                                                                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                                                                                                                                                </asp:View>
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <asp:View ID="View3" runat="server">
                                                                                                                                                    <%--<div id="country3" class="tabcontent">--%>
                                                                                                                                                    <asp:GridView ID="GridViewSIPResult" runat="server"
                                                                                                                                                        Width="98%" RowStyle-CssClass="grdRow"
                                                                                                                                                        AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                                                                                                        AllowPaging="false"
                                                                                                                                                        AutoGenerateColumns="false" OnRowDataBound="GridViewSIPResult_RowDataBound"
                                                                                                                                                        BorderWidth="1px"
                                                                                                                                                        BorderColor="#D52E42">
                                                                                                                                                        <Columns>
                                                                                                                                                            <asp:TemplateField HeaderText="">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)"%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle HorizontalAlign="Left" CssClass="" Width="360px" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="1 Year">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString()+((Eval("1 Year").ToString()=="N/A")?"":"%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="3 Years">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="5 Years">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="Since Inception">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                        </Columns>
                                                                                                                                                    </asp:GridView>
                                                                                                                                                    <asp:GridView ID="GridViewSTPTOResult" runat="server"
                                                                                                                                                        Width="98%" RowStyle-CssClass="grdRow"
                                                                                                                                                        AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                                                                                                        AllowPaging="false"
                                                                                                                                                        AutoGenerateColumns="false" BorderWidth="1px" BorderColor="#D52E42">
                                                                                                                                                        <Columns>
                                                                                                                                                            <asp:TemplateField HeaderText="">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() + " (CAGR)"%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle HorizontalAlign="Left" CssClass="" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="1 Year">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="3 Years">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="5 Years">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="Since Inception">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                        </Columns>
                                                                                                                                                    </asp:GridView>
                                                                                                                                                    <div id="divLsTable" runat="server">
                                                                                                                                                        <br />
                                                                                                                                                        <asp:GridView ID="GridViewResultLS" runat="server"
                                                                                                                                                            Width="98%" CssClass="grdRow"
                                                                                                                                                            RowStyle-CssClass="grdRow" AlternatingRowStyle-CssClass="grdRow"
                                                                                                                                                            HeaderStyle-CssClass="grdHead"
                                                                                                                                                            AutoGenerateColumns="false" BorderWidth="1px" BorderColor="#dee2e6">
                                                                                                                                                            <Columns>
                                                                                                                                                                <%--<asp:TemplateField
                                                                                                                    HeaderText="Type"> <ItemTemplate> <%# (Eval("Type") == DBNull.Value) ? "--" : Eval("Type").ToString()%>
                                                                                                                    </ItemTemplate> <ItemStyle HorizontalAlign="Left" /> </asp:TemplateField>--%>
                                                                                                                                                                <asp:TemplateField HeaderText="Scheme Name">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value)
                                                                                                                                ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)" %>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="" Width="360px" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="1 Year">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%#
                                                                                                                            (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="3 Years">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%#
                                                                                                                            (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="5 Years">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%#
                                                                                                                            (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="Since Inception">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                            </Columns>
                                                                                                                                                        </asp:GridView>
                                                                                                                                                    </div>
                                                                                                                                                    <%--</div>--%>
                                                                                                                                                </asp:View>
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <asp:View ID="View4" runat="server">
                                                                                                                                                    <div id="Showpdfdiv" runat="server" class="FieldHead"
                                                                                                                                                        style="border: 1px solid #569fd3">
                                                                                                                                                        <%-- <b>Please select your Credential:</b>--%>
                                                                                                                                                        <table width="100%" style="padding-top: 20px;">
                                                                                                                                                            <tr>
                                                                                                                                                                <td align="left" width="100%">
                                                                                                                                                                    <asp:RadioButtonList ID="RadioButtonListCustomerType"
                                                                                                                                                                        runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged"
                                                                                                                                                                        TextAlign="Right" RepeatDirection="Horizontal"
                                                                                                                                                                        AutoPostBack="true" BorderColor="#569fd3"
                                                                                                                                                                        BorderWidth="0" Width="250px" CssClass="lefft">
                                                                                                                                                                        <asp:ListItem>Distributor</asp:ListItem>
                                                                                                                                                                        <asp:ListItem Selected="true">Not a Distributor</asp:ListItem>
                                                                                                                                                                    </asp:RadioButtonList>
                                                                                                                                                                    <br />
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                        </table>
                                                                                                                                                        <div id="DistributerDiv" runat="server" visible="false">
                                                                                                                                                            <table id="tblDistb" width="100%" align="center">
                                                                                                                                                                <tr>
                                                                                                                                                                    <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                        ARN No
                                                                                                                                                                    </td>
                                                                                                                                                                    <td width="68%" align="left">
                                                                                                                                                                        <%--<asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>--%>
                                                                                                                                                                        <asp:TextBox ID="txtArn" CssClass="ddl_3" runat="server"
                                                                                                                                                                            MaxLength="30"></asp:TextBox>
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                                <tr>
                                                                                                                                                                    <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                        Prepared By
                                                                                                                                                                    </td>
                                                                                                                                                                    <td width="68%" align="left">
                                                                                                                                                                        <asp:TextBox ID="txtPreparedby" CssClass="ddl_3" runat="server"
                                                                                                                                                                            MaxLength="40"></asp:TextBox>
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                                <tr>
                                                                                                                                                                    <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                        Contact No(Mobile)
                                                                                                                                                                    </td>
                                                                                                                                                                    <td width="68%" align="left">
                                                                                                                                                                        <asp:TextBox ID="txtMobile" CssClass="ddl_3" runat="server"
                                                                                                                                                                            MaxLength="14"></asp:TextBox>
                                                                                                                                                                        <%--onkeypress="return isNumber(event)"--%>
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                                <tr>
                                                                                                                                                                    <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                        Email
                                                                                                                                                                    </td>
                                                                                                                                                                    <td width="68%" align="left">
                                                                                                                                                                        <asp:TextBox ID="txtEmail" CssClass="ddl_3" runat="server"
                                                                                                                                                                            MaxLength="30"></asp:TextBox>
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                                <tr>
                                                                                                                                                                    <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                        Prepared For
                                                                                                                                                                    </td>
                                                                                                                                                                    <td width="68%" align="left">
                                                                                                                                                                        <asp:TextBox ID="txtPreparedFor" CssClass="ddl_3" runat="server"
                                                                                                                                                                            MaxLength="40"></asp:TextBox>
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                                <%--<tr>
                                                                                                                                                                <td>
                                                                                                                                                                    ARN No
                                                                                                                                                                </td>
                                                                                                                                                                <td>
                                                                                                                                                                    <asp:TextBox ID="txtxARNo" runat="server"></asp:TextBox>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>--%>
                                                                                                                                                            </table>
                                                                                                                                                        </div>
                                                                                                                                                        <table width="100%">
                                                                                                                                                            <tr>
                                                                                                                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                    Generate PDF Report:
                                                                                                                                                                </td>
                                                                                                                                                                <td width="68%" align="left">
                                                                                                                                                                    <asp:LinkButton ID="LinkButtonGenerateReport" runat="server"
                                                                                                                                                                        OnClick="LinkButtonGenerateReport_Click"
                                                                                                                                                                        ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();"><img src="IMG/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:LinkButton>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                        </table>
                                                                                                                                                    </div>
                                                                                                                                                </asp:View>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                    </table>
                                                                                                                                </asp:MultiView>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                <%-- <div id="resultDivLS" runat="server" visible="false">
                                                                                                <ul id="Ul1" class="shadetabs">
                                                                                                    <li>
                                                                                                        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnkTab5_Click">View Historical Performance</asp:LinkButton></li>
                                                                                                </ul>
                                                                                                <asp:MultiView ID="MultiView2" runat="server">
                                                                                                    <asp:View ID="View5" runat="server">
                                                                                                    </asp:View>
                                                                                                </asp:MultiView>
                                                                                            </div>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div class="" style="text-align: right; margin-bottom: 10px;
                                                                                    padding-top: 20px;">
                                                                                    <p class="">
                                                                                        <a href="https://mfi360.icrainsights.com/AMC/FundScreener"
                                                                                            target="_blank"
                                                                                            style="color: #cc885a; font-size: 12px; font-weight: bold;
                                                                                            text-decoration: underline">Do More</a>
                                                                                    </p>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="disclaimerDiv" runat="server" style="font-size: 11px;
                                                                                    margin-top: 15px; display:none ">
                                                                                    <table width="100%">
                                                                                        
                                                                                        <tr align="left" display: none>
                                                                                            <td valign="top" class="rslt_text1">
                                                                                                <div align="justify">
                                                                                                    <b>Disclaimer:</b>
                                                                                                    <br />
                                                                                                    <asp:Label ID="LSDisc" runat="server" Text="<b><br/>* Returns here denote the Extended Internal Rate of Return (XIRR).  </br></b>"
                                                                                                        Visible="true"></asp:Label>
                                                                                                    <asp:Label ID="LSDisc1" runat="server" Text="<b><br/>* For Time Periods > 1 yr, CAGR Returns have been shown. For Time Periods < 1 yr, Absolute Returns have been shown. </br></b>"
                                                                                                        Visible="false"></asp:Label>
                                                                                                    <b>• Since Inception return of the benchmark is calculated
                                                                                                        from the scheme inception
                                                                                                    date.</b>
                                                                                                    <br />
                                                                                                    Past performance may or may not be sustained in the
                                                                                                    future and should not be used
                                                                                                as a basis for comparison with other investments.
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                       
                                                                                        <tr align="left" display: none>
                                                                                            <td valign="top" class="rslt_text1">
                                                                                                <div align="justify">
                                                                                                    <asp:Label ID="lblDisclaimer" runat="server" Text=""></asp:Label>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img src="IMG/spacer11.gif" width="1" height="4" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left">
                                                                                            <td align="left" valign="top" class="rslt_text1">
                                                                                                <div align="justify">
                                                                                                    The return calculator has been developed and is maintained
                                                                                                    by ICRA Analytics Limited.
                                                                                                </div>
                                                                                                <table id="Table2" border="0" cellspacing="0" cellpadding="0"
                                                                                                    width="100%" align="left">
                                                                                                    <tr>
                                                                                                        <td class="text" align="right"></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="text" align="right">
                                                                                                            <span style="text-align: right" class="rslt_text1">Developed
                                                                                                                by:<a class="text" href="https://www.icraanalytics.com"
                                                                                                                    target="_blank"> ICRA Analytics Ltd </a>, <a class="text"
                                                                                                                        href="https://icraanalytics.com/home/Disclaimer"
                                                                                                                        target="_blank">Disclaimer </a></span>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr >

                                                                            <td align="right" style="text-align: right; font-size: 10px; color: #A7A7A7">
                                                                                <span style="text-align: right" class="rslt_text1">Developed for Cafemutual by:<a style="text-align: right; font-size: 10px; color: #A7A7A7" 
                                                                                                                    href="https://www.icraanalytics.com"
                                                                                                                    target="_blank"> ICRA Analytics Ltd </a>, 
                                                                                    <a style="text-align: right; font-size: 10px; color: #A7A7A7"
                                                                                                                        href="https://icraanalytics.com/home/Disclaimer"
                                                                                                                        target="_blank">Disclaimer </a></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="1%" valign="top">
                                                                    <img src="IMG/spacer11.gif" width="10" height="1" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

        </form>
    </div>
</body>
</html>
