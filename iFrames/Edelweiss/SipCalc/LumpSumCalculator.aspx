<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LumpSumCalculator.aspx.cs" Inherits="iFrames.Edelweiss.LumpSumCalculator" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Lumpsum Calculator</title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/nifty.min.v1.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="script/jquery.min.js"></script>
    <script src="script/jquery-ui.js"></script>
    <script src="script/bootstrap.min.js"></script>
    <script src="script/check.js"></script>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
    <%--<script src="Script/tabcontent.js" type="text/javascript"></script>--%>

    <style>
        body {
            font-family: 'Roboto', sans-serif;
        }

        .panel-control {
            float: none;
            padding: 0;
        }

        .form-group .toggle-switch + *, .form-group .toggle-switch + ::before, .form-group .toggle-switch + ::after {
            margin-top: -8px !important;
        }

        .toggle-switch + label::after {
            top: 8px;
        }

        .table th {
            color: #212529;
            font-size: 14px;
        }

        .table tr td {
            font-size: 14px;
        }

        .table-header-text {
            text-align: right;
        }

        a.tablinks-active {
            border-bottom: #8dc63f solid 3px;
        }

        a.tab-button:hover {
            border-bottom: #8dc63f solid 3px;
        }

        #page-content .ui-datepicker-trigger {
            padding: 3px 4px;
            font-size: 14px;
            font-weight: 400;
            line-height: 1;
            color: #555;
            text-align: center;
            background-color: transparent;
            border: 0px;
            border-radius: 0;
            position: absolute;
            margin-left: -26px;
            z-index: 99;
            top: 16px;
        }

        .input-group-addon {
            position: relative;
            top: 16px;
        }

        .disabled_select {
            pointer-events: none;
            opacity: .5;
        }
    </style>

    <script type="text/javascript">
        function pop() {
            // alert("test");
            //            //            $(function () {
            //            $('#<%= sipbtnshow.ClientID %>').click(function () {
            //                $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
            //            });
            //});
            $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
        }

        function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
                return true;
            }
            return false;
        }
        //Add by koustav 5-apr-2018
        function ChangeRegularPayout4Swp(InitialInvest, Frequency, Percentange) {

            var Amount = parseFloat((InitialInvest * (Percentange / 100)) / Frequency);
            $('#txtTransferWithdrawal2').val(Math.round(Amount));
        }


    </script>

    <script type="text/javascript" language="javascript">

        $(function () {

            $("#txtfromDate").datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "img/calendar.png",
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
            });

            $("#txtToDate").datepicker({
                showOn: "button",
                buttonImage: "img/calendar.png",
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
                buttonImage: "img/calendar.png",
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
                buttonImage: "img/calendar.png",
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
                buttonImage: "img/calendar.png",
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
                buttonImage: "img/calendar.png",
                buttonImageOnly: true,
                buttonText: "Select Date",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1
                //                 maxDate: 0
            });
            //Add by koustav 5-apr-2018
            setFromDateAfterPostBack();
            $('#txtiniAmount').change(function () {
                if ($('#ddlMode option:selected').val() == "SWP") {
                    var frequency = $('#ddPeriod_SIP option:selected').val();
                    var dy;
                    if (frequency == "Daily") { dy = 365; }
                    if (frequency == "Weekly") { dy = 52; }
                    if (frequency == "Monthly") { dy = 12; }
                    if (frequency == "Quarterly") { dy = 4; }
                    if ($('#txtiniAmount').val() != "" && $('#txtiniAmount').val() != null) {
                        $('#txtTransferWithdrawal2').val('');
                        var InitialAmt = parseFloat($('#txtiniAmount').val());
                        var PayoutPercentage = parseFloat($('#Swp_Persentage_payout option:selected').val());
                        ChangeRegularPayout4Swp(InitialAmt, dy, PayoutPercentage);
                    }
                }
            });
            $("#ddPeriod_SIP").change(function () {

                var frequency = $('option:selected', this).val();
                var dy;
                if (frequency == "Daily") { dy = 365; }
                if (frequency == "Weekly") { dy = 52; }
                if (frequency == "Monthly") { dy = 12; }
                if (frequency == "Quarterly") { dy = 4; }
                if ($('#ddlMode option:selected').val() == "SWP") {
                    if ($('#txtiniAmount').val() != "" && $('#txtiniAmount').val() != null) {
                        $('#txtTransferWithdrawal2').val('');
                        ChangeRegularPayout4Swp(parseFloat($('#txtiniAmount').val()), dy, parseFloat($('#Swp_Persentage_payout option:selected').val()));
                    }
                }
            });
            $("#Swp_Persentage_payout").change(function () {

                var frequency = $('#ddPeriod_SIP option:selected').val();
                var dy;
                if (frequency == "Daily") { dy = 365; }
                if (frequency == "Weekly") { dy = 52; }
                if (frequency == "Monthly") { dy = 12; }
                if (frequency == "Quarterly") { dy = 4; }
                if ($('#ddlMode option:selected').val() == "SWP") {
                    if ($('#txtiniAmount').val() != "" && $('#txtiniAmount').val() != null) {
                        $('#txtTransferWithdrawal2').val('');
                        var InitialAmt = parseFloat($('#txtiniAmount').val());
                        var PayoutPercentage = parseFloat($('#Swp_Persentage_payout option:selected').val());
                        ChangeRegularPayout4Swp(InitialAmt, dy, PayoutPercentage);
                    }
                }
            });
            //End
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <%--<div id="loader" class="loader-container">
            <div class="loader-content">
                Please Wait...
            </div>
        </div>--%>
        <div id="" class="effect aside-float aside-bright mainnav-lg">
            <div class="boxed" id="newDiv" runat="server">
                <div id="content-container">
                    <div id="page-content">
                        <%--<div class="panel-heading">
                                <h3 class="panel-title">Calculate & invest the right amount</h3>
                            </div>--%>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0;">
                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <div class="form-group">
                                    <label>Investment Mode</label>
                                    <div id="divMode">
                                        <div id="_mode" class="">
                                            <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true" class="form-control  disabled_select">
                                                <asp:ListItem Value="SIP">SIP</asp:ListItem>
                                                <asp:ListItem Value="Lump Sum">Lump Sum</asp:ListItem>
                                                <%-- <asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>--%>
                                                <asp:ListItem Value="SWP">SWP/Regular Payout Facility</asp:ListItem>
                                                <asp:ListItem Value="STP">STP</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="padding: 0" id="trCategory" runat="server" visible="true">
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <div class="form-group">
                                        <label>Category</label>
                                        <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <div class="form-group">
                                    <label>Option </label>
                                    <asp:DropDownList ID="ddlOption" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <div class="form-group">
                                    <asp:Label ID="lblSchemeName" runat="server" Text=" Scheme Name" CssClass="lebell">Scheme Name </asp:Label>
                                    <asp:DropDownList ID="ddlscheme" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="trLumpInvst" runat="server" visible="true" style="padding: 0">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                <div id="trInception1" runat="server">
                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" style="padding: 0">
                                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                            <div class="form-group has-feedback">
                                                <asp:Label ID="LabelInception" runat="server" Text="Inception Date" CssClass="lebell"></asp:Label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="SIPSchDt" runat="server" ReadOnly="true" Text="" class="form-control" placeholder="Select Date" />
                                                    <span class="input-group-addon">
                                                        <img src="../IMG/calendar.png" /></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12"></div>
                                        
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                            <div class="form-group has-feedback">
                                                <div id="trBenchmark" runat="server" visible="true">
                                                    <label id="Span3" style="text-align: left; vertical-align: bottom">Benchmark</label>
                                                    <asp:TextBox ID="txtddlsipbnmark" runat="server" class="form-control" placeholder="Benchmark" />
                                                </div>
                                            </div>
                                        </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                        <div class="form-group has-feedback" runat="server" id="Div3">
                                            <label>Investment Amount (₹)</label>
                                            <asp:TextBox ID="txtinstallLs" value="" MaxLength="10" runat="server" class="form-control" onmousedown="Javascript: setDate();" onChange="Javascript: checkInvestedValue();" />
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" style="padding: 0; padding-bottom: 10px;">
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                            <div class="form-group has-feedback">
                                                <label id="Label2">Start Date</label>
                                                <div class="input-group" style="display: block;">
                                                    <asp:TextBox ID="txtLumpfromDate" runat="server" class="form-control" onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                            <div class="form-group has-feedback">
                                                <label>End Date</label>
                                                <div class="input-group" style="display: block;">
                                                    <asp:TextBox ID="txtLumpToDate" runat="server" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding-left: 0" id="trCalTypeLmpsm" runat="server" visible="false">
                                <div class="form-group has-feedback col-lg-6 col-md-6 col-sm-12 col-xs-12" style="padding-left: 5px">
                                        <%--<asp:Label ID="Label4" runat="server" Text="" class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></asp:Label>--%>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12" style="padding: 0">
                                            &nbsp;
                                            <asp:RadioButton ID="lmprdbReinvest" Checked="true" runat="server" GroupName="rdbCalc" />&nbsp;Reinvest
                                        </div>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                            &nbsp;
                                            <asp:RadioButton ID="lmprdbPayout" runat="server" GroupName="rdbCalc" />&nbsp;Payout
                                        </div>
                                    </div>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding-left: 15px;">
                                <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12" id="tr_Cal_Incp" runat="server">
                                    <label class="container1">
                                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                        <input type="checkbox" id="chkInception" runat="server" onclick="setIncDate();" />
                                        <span class="checkmark"></span>
                                        <span style="margin-left: 10px;">Calculate from Inception</span>
                                    </label>
                                </div>
                                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12"></div>
                            </div>
                        </div>


                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0" id="trTransferTo" runat="server" visible="false">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="form-group">
                                    <div class="row">
                                        <label>Transfer To (₹)</label>
                                        <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0" id="trInception" runat="server" visible="false">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="form-group has-feedback">
                                        <label>Inception Date</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="SIPSchDt2" runat="server" ReadOnly="true" Text="" class="form-control" placeholder="Select Date" />
                                            <span class="input-group-addon">
                                                <img src="../IMG/calendar.png" /></span>
                                        </div>
                                    </div>
                            </div>
                        </div>

                        <div id="trInitialInvst" runat="server" visible="false">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5">
                                        <div class="form-group has-feedback">
                                            <div class="row">
                                                <label>Initial Amount (₹)</label>
                                                <asp:TextBox ID="txtiniAmount" runat="server" ReadOnly="false" Text="" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-md-2 col-lg-2 col-sm-2"></div>
                                    <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5">
                                        <div class="form-group has-feedback">
                                            <div class="row">
                                                <label>Initial Date</label>
                                                <div class="input-group" style="display: block;">
                                                    <asp:TextBox ID="txtIniToDate" runat="server" Text="" class="form-control" onMouseDown="Javascript: setDate();" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="trSipInvst" runat="server" visible="false">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0" runat="server" id="SIP_withdrawl">
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <div class="form-group">
                                        <asp:Label ID="lblInstallmentAmt" runat="server" Text="Investment Amount (₹)" CssClass="lebell"></asp:Label>
                                        <div>
                                            <asp:TextBox ID="txtinstall" runat="server" class="form-control" placeholder="Amount" ReadOnly="false" onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                        <div class="form-group has-feedback">
                                            <label>Start Date</label>
                                            <div class="input-group" style="display: block;">
                                                <asp:TextBox ID="txtfromDate" runat="server" class="form-control" placeholder="Select Date" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                        <div class="form-group has-feedback">
                                            <label>End Date</label>
                                            <div class="input-group" style="display: block;">
                                                <asp:TextBox ID="txtToDate" runat="server" class="form-control" placeholder="Select Date" onChange="Javascript: setDateValueAsOn(); " />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0" runat="server">

                                <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12" style="padding: 0; display: none">
                                    <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                        <div class="form-group has-feedback">
                                            <label>Frequency </label>
                                            <asp:DropDownList ID="ddPeriod_SIP" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                        <div class="form-group has-feedback lebell">
                                            <asp:Label ID="lblDiffDate" runat="server" Text="SIP Date" Style="text-align: left;" CssClass=""></asp:Label>
                                            <div style="margin-top: 19px;">
                                                <asp:DropDownList ID="ddSIPdate" runat="server" class="form-control">
                                                    <asp:ListItem Value="1">1st</asp:ListItem>
                                                    <asp:ListItem Value="2">2nd</asp:ListItem>
                                                    <asp:ListItem Value="3">3rd</asp:ListItem>
                                                    <asp:ListItem Value="4">4th</asp:ListItem>
                                                    <asp:ListItem Value="5">5th</asp:ListItem>
                                                    <asp:ListItem Value="6">6th</asp:ListItem>
                                                    <asp:ListItem Value="7">7th</asp:ListItem>
                                                    <asp:ListItem Value="8">8th</asp:ListItem>
                                                    <asp:ListItem Value="9">9th</asp:ListItem>
                                                    <asp:ListItem Value="10">10th</asp:ListItem>
                                                    <asp:ListItem Value="11">11th</asp:ListItem>
                                                    <asp:ListItem Value="12">12th</asp:ListItem>
                                                    <asp:ListItem Value="13">13th</asp:ListItem>
                                                    <asp:ListItem Value="14">14th</asp:ListItem>
                                                    <asp:ListItem Value="15">15th</asp:ListItem>
                                                    <asp:ListItem Value="16">16th</asp:ListItem>
                                                    <asp:ListItem Value="17">17th</asp:ListItem>
                                                    <asp:ListItem Value="18">18th</asp:ListItem>
                                                    <asp:ListItem Value="19">19th</asp:ListItem>
                                                    <asp:ListItem Value="20">20th</asp:ListItem>
                                                    <asp:ListItem Value="21">21th</asp:ListItem>
                                                    <asp:ListItem Value="22">22th</asp:ListItem>
                                                    <asp:ListItem Value="23">23th</asp:ListItem>
                                                    <asp:ListItem Value="24">24th</asp:ListItem>
                                                    <asp:ListItem Value="25">25th</asp:ListItem>
                                                    <asp:ListItem Value="26">26th</asp:ListItem>
                                                    <asp:ListItem Value="27">27th</asp:ListItem>
                                                    <asp:ListItem Value="28">28th</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" id="divTransferWithdrawal" runat="server" visible="false">
                                    <div class="row">
                                        <div class="form-group">
                                            <asp:Label ID="lblTransferWithdrawal" runat="server" Text=" Withdrawal Amount (₹)"></asp:Label>
                                            <asp:TextBox ID="txtTransferWithdrawal" runat="server" class="form-control" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--<div >
                                        <asp:Label ID="lblTransferWithdrawal" runat="server" Text="Withdrawal Amount" class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></asp:Label>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6">
                                            <div class="input-group">
                                                <span class="input-group-addon">₹</span>
                                                <asp:TextBox ID="txtTransferWithdrawal" runat="server" class="form-control" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                    </div>--%>



                            <div class="form-group has-feedback" id="SWP_STP_withdrawl" runat="server" visible="false">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="row">
                                        <div class="form-group has-feedback">
                                            <label class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblTransferWithdrawal2" runat="server"
                                                    Text=" Withdrawal Amount (₹)"></asp:Label>
                                            </label>
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group" style="padding: 0">

                                            <div id="Swp_Persentage_payout" class="col-xs-12 col-md-5 col-lg-5 col-sm-12" runat="server">
                                                <asp:DropDownList ID="DropDn4PayoutSwp" runat="server" class="form-control">
                                                    <asp:ListItem Value="6">6%</asp:ListItem>
                                                    <asp:ListItem Value="6.5">6.5%</asp:ListItem>
                                                    <asp:ListItem Value="7">7%</asp:ListItem>
                                                    <asp:ListItem Value="7.5">7.5%</asp:ListItem>
                                                    <asp:ListItem Value="8">8%</asp:ListItem>
                                                    <asp:ListItem Value="8.5">8.5%</asp:ListItem>
                                                    <asp:ListItem Value="9">9%</asp:ListItem>
                                                    <asp:ListItem Value="9.5">9.5%</asp:ListItem>
                                                    <asp:ListItem Value="10">10%</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-xs-12 col-md-5 col-lg-5 col-sm-12">
                                                <div id="divTransferWithdrawal2" runat="server">
                                                    <asp:TextBox ID="txtTransferWithdrawal2" runat="server" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-12 col-xs-12">
                                                <div style="font-size: 12px; margin-top: 22px;" id="Swp_Persentage_payout_message" runat="server" visible="false">
                                                    <small>(For regular payout facility per annum)</small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0; margin-bottom: 12px; display: none">

                                <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                    <div class="form-group has-feedback" runat="server" id="Div2">
                                        <label>Current Date</label>
                                        <div class="input-group" style="display: block;">
                                            <asp:TextBox ID="txtvalason" runat="server" class="form-control" placeholder="Select Date" />
                                        </div>
                                        <span style="font-size: 11px;">('Current Date' should be greater than 'To Date')</span>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12"></div>
                            </div>



                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12" id="tr_Cal_Type" runat="server" visible="false" style="padding: 0">
                                    <div class="form-group has-feedback" style="margin-bottom: 0px">
                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                            <asp:RadioButton ID="rdbReinvest" Checked="true" runat="server"
                                                GroupName="rdbCalc" />&nbsp;Reinvest
                                        </div>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12">
                                            <asp:RadioButton ID="rdbPayout" runat="server" GroupName="rdbCalc" />&nbsp;Payout
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12"></div>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0; margin-top: 18px">
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <div class="form-group has-feedback" id="tr_SIP_Sinc" runat="server">
                                        <div id="Div6" runat="server">
                                            <label class="container1">
                                                <input type="checkbox" id="chkInception4sip" runat="server" onclick="setIncDate();" />
                                                <span class="checkmark"></span>
                                                <span style="margin-left: 20px;">Calculate return from inception </span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-md-6 col-lg-6 col-sm-12"></div>
                            </div>

                            <%--<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding-left: 0"  runat="server" visible="false">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5">
                                                <div class="row">
                                                    
                                                </div>
                                            </div>
                                            <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5"></div>
                                        </div>
                                    </div>--%>
                        </div>
                        <%--<div class="form-group has-feedback row" id="SWP_STP_withdrawl" runat="server" visible="false">
                                        <asp:Label ID="lblTransferWithdrawal2" runat="server" Text="Withdrawal Amount" class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></asp:Label>
                                        <div class="col-xs-12 col-md-2 col-lg-2 col-sm-3" id="Swp_Persentage_payout" runat="server" visible="false">
                                            <asp:DropDownList ID="DropDn4PayoutSwp" runat="server" class="form-control">
                                                <asp:ListItem Value="6">6%</asp:ListItem>
                                                <asp:ListItem Value="6.5">6.5%</asp:ListItem>
                                                <asp:ListItem Value="7">7%</asp:ListItem>
                                                <asp:ListItem Value="7.5">7.5%</asp:ListItem>
                                                <asp:ListItem Value="8">8%</asp:ListItem>
                                                <asp:ListItem Value="8.5">8.5%</asp:ListItem>
                                                <asp:ListItem Value="9">9%</asp:ListItem>
                                                <asp:ListItem Value="9.5">9.5%</asp:ListItem>
                                                <asp:ListItem Value="10">10%</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                        <div id="Swp_Persentage_payout_message" runat="server" visible="false">
                                            <asp:Label ID="Label4SwpPayout" runat="server" Text="(For regular payout facility per annum)" class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></asp:Label>
                                        </div>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6">
                                            <asp:TextBox ID="txtTransferWithdrawal2" runat="server" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false" class="form-control" />
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                    </div>--%>
                        <%--<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0" id="SWP_STP_withdrawl" runat="server" visible="false">
                                    <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                                        <div class="form-group" id="switchamountid" runat="server">
                                            <div class="row">
                                                <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5">
                                                    <asp:Label ID="lblTransferWithdrawal2" runat="server" Text="Withdrawal Amount"></asp:Label>
                                                </div>
                                                <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3" id="Swp_Persentage_payout" runat="server" visible="false">
                                                    <asp:DropDownList ID="DropDn4PayoutSwp" runat="server" class="form-control">
                                                        <asp:ListItem Value="6">6%</asp:ListItem>
                                                        <asp:ListItem Value="6.5">6.5%</asp:ListItem>
                                                        <asp:ListItem Value="7">7%</asp:ListItem>
                                                        <asp:ListItem Value="7.5">7.5%</asp:ListItem>
                                                        <asp:ListItem Value="8">8%</asp:ListItem>
                                                        <asp:ListItem Value="8.5">8.5%</asp:ListItem>
                                                        <asp:ListItem Value="9">9%</asp:ListItem>
                                                        <asp:ListItem Value="9.5">9.5%</asp:ListItem>
                                                        <asp:ListItem Value="10">10%</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-xs-12 col-md-4 col-lg-4 col-sm-4">
                                                    <asp:TextBox ID="txtTransferWithdrawal2" runat="server" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12" id="Swp_Persentage_payout_message" runat="server" visible="false">(For regular payout facility per annum)</div>
                                </div>--%>

                        
                        <%--<div class="form-group has-feedback row" runat="server" id="Div3">
                                        <label class="col-xs-12 col-md-3 col-lg-3 col-sm-3">Investment Amount (₹)</label>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtinstallLs" value="" MaxLength="10" runat="server" class="form-control" onmousedown="Javascript: setDate();" onChange="Javascript: checkInvestedValue();" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>

                                        <asp:Label ID="Label2" runat="server" Text="Start Date" class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></asp:Label>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6">
                                            <div class="input-group">
                                                <span class="input-group-addon">₹</span>
                                                <asp:TextBox ID="txtLumpfromDate" runat="server" class="form-control" onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                    </div>--%>
                        <%--<div class="form-group has-feedback row" id="tr_Cal_Incp" runat="server">
                                        <label class="col-xs-12 col-md-3 col-lg-3 col-sm-3">End Date</label>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6">
                                            <div class="input-group">
                                                <span class="input-group-addon">₹</span>
                                                <asp:TextBox ID="txtLumpToDate" runat="server" class="form-control" />
                                            </div>
                                        </div>
                                        <asp:Label ID="Label3" runat="server" Text="" class="col-xs-12 col-md-2 col-lg-2 col-sm-2" Style="text-align: right;"></asp:Label>
                                        <div class="col-xs-12 col-md-2 col-lg-2 col-sm-3">
                                            <input type="checkbox" id="chkInception" runat="server" onclick="setIncDate();" />
                                            Calculate from Inception
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                    </div>--%>

                        

                        <div class="row" style="padding-left: 15px">
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12" style="padding-top: 30px;">
                                <asp:Button ID="sipbtnreset" class="btn btn-default" Text="Reset" runat="server" OnClick="sipbtnreset_Click"></asp:Button>
                                <asp:Button ID="sipbtnshow" runat="server" class="btn btn-info" Text="Calculate" OnClick="sipbtnshow_Click"></asp:Button>

                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12"></div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div style="border-bottom: #8dc63f solid 3px; margin-top: 20px;"></div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                            <div class="" id="resultDiv" runat="server" visible="false" style="background: #f7f9fc; padding: 12px;">
                                <h3 style="margin-left: 6px; color: #034ea2;">Report</h3>
                                <div class="table-responsive">
                                    <table class="table" style="display: none" cellspacing="0" cellpadding="0" border="0">
                                        <tr align="left">
                                            <td width="3%" height="20" align="center" valign="middle">
                                                <img src="IMG/arw.gif" width="4" height="8" />
                                            </td>
                                            <td width="97%" height="25" valign="middle">
                                                <%-- <span class="rslt_text">Investment amount per month : Rs<strong> 5000</strong></span>--%>
                                                <asp:Label ID="lblInvestment" CssClass="rslt_text" runat="server" Text="Investment amount per month"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td width="3%" align="center" valign="middle">
                                                <img src="IMG/arw.gif" width="4" height="8" />
                                            </td>
                                            <td height="25" align="left" valign="middle">
                                                <%--<span class="rslt_text">Total Investment Amount : Rs <strong>120000</strong></span>--%>
                                                <asp:Label ID="lblTotalInvst" CssClass="rslt_text" runat="server" Text="Total Investment Amount"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="3%" height="25" align="center" valign="middle">
                                                <img src="IMG/arw.gif" width="4" height="8" />
                                            </td>
                                            <td align="left" valign="middle">
                                                <%--<span class="rslt_text">On 05/07/2012, the value of your total investment Rs 120000
                                                                                        would be Rs <strong>131779.37</strong></span>--%>
                                                <asp:Label ID="lblInvstvalue" CssClass="rslt_text" runat="server" Text="On Date C, the Scheme value of your total investment Rs Y would be Rs Z">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="3%" height="25" align="center" valign="middle">
                                                <img src="IMG/arw.gif" width="4" height="8" />
                                            </td>
                                            <td align="left" valign="middle">
                                                <asp:Label ID="lblAbsoluteReturn" CssClass="rslt_text" runat="server" Text="Absolute return from Date  to Date  is X%">
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
                                                <asp:Label ID="lblCagrReturn" CssClass="rslt_text" runat="server" Text="XIRR return from Date  to Date  is X%">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="3%" height="25" align="center" valign="middle">
                                                <img src="IMG/arw.gif" width="4" height="8" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIfInvested" CssClass="rslt_text" runat="server" Text="Had you invested Rs Y at Date A, the total value of this investment at Date C would have become Q">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="25" colspan="2" align="center" valign="middle">
                                                <div align="left">
                                                    &nbsp;&nbsp;&nbsp;<b>View Historical Fund Performance below:<b />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table" id="firstTable" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td style="border: none">
                                                <div id="divSummary" runat="server" style="border: none">
                                                    <asp:GridView ID="gvFirstTable" runat="server" AutoGenerateColumns="False" class="table" BorderWidth="0">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Scheme") == DBNull.Value) ? "--" : Eval("Scheme").ToString()%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="left" CssClass="leftal" Width="280px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Units Purchased" HeaderStyle-VerticalAlign="Top">
                                                                <HeaderStyle CssClass="table-header-text" />
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_unit") == DBNull.Value) ? "--" : Eval("Total_unit").ToString().Trim() == "0" ? "--" : Math.Round(Convert.ToDouble( Eval("Total_unit")),0).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                <HeaderStyle CssClass="table-header-text" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount Invested(A)" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_amount") == DBNull.Value) ? "--" : Eval("Total_amount").ToString().Trim() == "0" ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                <HeaderStyle CssClass="table-header-text" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Investment Value as on Date (B)" HeaderStyle-VerticalAlign="Top">
                                                                <HeaderStyle CssClass="table-header-text" />
                                                                <ItemTemplate>
                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : Eval("Present_Value").ToString().Trim() == "0" ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                <HeaderStyle CssClass="table-header-text" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dividend Amount( C )" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Dividend_Amount") == DBNull.Value || Eval("Dividend_Amount") == "--" || Eval("Dividend_Amount").ToString() == "0") ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Dividend_Amount")),2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                <HeaderStyle CssClass="table-header-text" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Profit (B-A)" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : Eval("Profit_Sip").ToString().Trim() == "0" ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                <HeaderStyle CssClass="table-header-text" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Abs. Returns" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : Eval("ABSOLUTERETURN").ToString().Trim() == "0" ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                <HeaderStyle CssClass="table-header-text" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Returns*" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString().Trim() == "0" ? "--" : (Eval("Yield").ToString() + "%")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                <HeaderStyle CssClass="table-header-text" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>

                                                    <span id="sipDisclaimer" class="FieldHead" style="padding: 4px; margin: 2px" visible="false"
                                                        runat="server"></span>
                                                    <asp:GridView ID="gvSWPSummaryTable" runat="server" AutoGenerateColumns="False" class="table" BorderWidth="0">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="310px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount<BR/> Invested (A)" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount<BR/> Withdrawn (B)" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Present<BR/> Value (C)" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Profit<BR/> (B+C-A)" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# "<img src='' style='vertical-align:middle;'> "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Abs. <br/>Returns" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Returns *" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="gvSTPToSummaryTable" runat="server" AutoGenerateColumns="False" class="table table-bordered">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="40%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Amount Invested">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                    <%--Eval("Total_Amount_Invested").ToString()--%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" Width="24%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Present Value">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
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
                                                    <table class="table">
                                                        <tr align="left">
                                                            <td valign="top" colspan="2" style="border: none">
                                                                <asp:GridView ID="GridViewLumpSum" runat="server" class="table"
                                                                    AutoGenerateColumns="false" BorderWidth="0">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="370px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Amount Invested <br/>(A)" HeaderStyle-VerticalAlign="Top">
                                                                            <HeaderStyle />
                                                                            <ItemTemplate>
                                                                                <%# (Eval("InvestedAmount") == DBNull.Value) ? "--" : "<img src='' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedAmount")).ToString("n2")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Investment Value<br/>(B)" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "<img src='' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Dividend Amount<br/>(C)" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("DividendAmount") == "--") ? "--" : "<img src='' style='vertical-align:middle;'> " + Eval("DividendAmount")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Profit from Investment<br/>(B+C-A)" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("Profit") == DBNull.Value) ? "--" : "<img src='' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("Profit")).ToString("n2")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Return *<br/>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("Return") == DBNull.Value) ? "--" : Eval("Return").ToString() + ((Eval("Return").ToString() == "N/A") ? "" : "%")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border: none; border-bottom: #ccc solid 1px; line-height: 44px;">
                                                <div class="tab-btns-list">
                                                    <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click" class="tab-button tablinks"><img src="../IMG/report.png" /> View Detail Report</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click" class="tab-button tablinks"><img src="../IMG/graph.png" /> View Graph</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkTab3" runat="server" Visible="false" OnClick="lnkTab3_Click" class="tab-button tablinks"><img src="../IMG/report.png" /> View Historical Performance</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkTab5" runat="server" Visible="false" OnClick="lnkTab5_Click" class="tab-button tablinks"><img src="../IMG/report.png" /> View Dividend History</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkTab4" runat="server" OnClick="lnkTab4_Click" class="tab-button tablinks"><img src="../IMG/pdf1.png" /> View PDF Report</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table" id="TableRemark" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td style="border-bottom: none">
                                                <div id="divTab" runat="server" visible="false">
                                                    <div class="panel-control" style="text-align: right; margin-bottom: 10px;">
                                                        <asp:Button ID="btnExcelCalculation" class="btn btn-success btn-sm" runat="server" Text="Export To Excel" OnClick="ExcelCalculation_Click" />
                                                    </div>
                                                    <asp:MultiView ID="MultiView1" runat="server">
                                                        <table class="table" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:View ID="View1" runat="server">
                                                                        <div id="DetailDiv" runat="server">
                                                                            <%--<div id="country1" class="tabcontent">--%>
                                                                            <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False" class="table" BorderWidth="0">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                        <ItemTemplate>
                                                                                            <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[2]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[1]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Dividend(%)">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Scheme Units">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Scheme_units") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_units")).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Cumulative Units">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("SCHEME_UNITS_CUMULATIVE") == DBNull.Value) ? "--" :Math.Round(Convert.ToDouble( Eval("SCHEME_UNITS_CUMULATIVE")),2).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="SIP Amount">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Scheme_cashflow") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_cashflow")).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Cumulative Fund Value">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("CUMULATIVE_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMULATIVE_AMOUNT")), 2).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                            <asp:GridView ID="swpGridView" runat="server" AutoGenerateColumns="False" class="table" BorderWidth="0">
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
                                                                                        <ItemStyle CssClass="borderbottom" HorizontalAlign="right" />
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
                                                                                <b>From Scheme: </b>
                                                                                <%= ddlscheme.SelectedItem.Text %>

                                                                                <br />

                                                                                <asp:GridView ID="stpFromGridview" runat="server" AutoGenerateColumns="False" class="table" BorderWidth="0">
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
                                                                                                <%# (Eval("FINAL_INVST_AMOUNT_FROM") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT_FROM")).ToString("n2")%>
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
                                                                                <b>To Scheme: </b>
                                                                                <%= ddlschtrto.SelectedItem.Text %>

                                                                                <br />

                                                                                <asp:GridView ID="stpToGridview" runat="server" AutoGenerateColumns="False" class="table" BorderWidth="0">
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
                                                                                                <%# (Eval("FINAL_INVST_AMOUNT_TO") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT_TO")).ToString("n2")%>
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

                                                                            <%--<table width="100%">
                                                                                <tr>
                                                                                    <td align="right" style="padding-right: 20px; border: none">
                                                                                        <asp:ImageButton ID="btnExcelCalculation" runat="server" ImageUrl="~/Edelweiss/IMG/excell.png" ToolTip="Download Excel" Text="Show Excel Calculation" Visible="true" OnClick="ExcelCalculation_Click" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>--%>
                                                                        </div>
                                                                    </asp:View>
                                                                </td>
                                                                <td>
                                                                    <asp:View ID="View2" runat="server">
                                                                        <%--  <div id="country2" class="tabcontent">--%>
                                                                        <div id="divshowChart" runat="server" visible="true" style="width: 100%; text-align: center" class="table-responsive">
                                                                            <asp:Chart ID="chrtResult" runat="server" AlternateText="Edelweiss" Visible="true"
                                                                                BorderlineWidth="2" Width="650px" Height="580px" IsSoftShadows="false">
                                                                                <Titles>
                                                                                    <asp:Title Font="Roboto, 14pt, style=Bold" Text="Edelweiss" ForeColor="26, 59, 105">
                                                                                    </asp:Title>
                                                                                </Titles>
                                                                                <Legends>
                                                                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Roboto, 8.25pt, style=Bold">
                                                                                    </asp:Legend>
                                                                                </Legends>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="" AlignmentStyle="PlotPosition"
                                                                                        BackSecondaryColor="White" BackColor="White" ShadowColor="Transparent" BackGradientStyle="Center"
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
                                                                                        <AxisY ArrowStyle="None" ToolTip="Value(₹)" TextOrientation="Horizontal" LineColor="#013974">
                                                                                            <ScaleBreakStyle Enabled="True" />
                                                                                            <MajorGrid Enabled="false" />
                                                                                        </AxisY>
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                            <br />
                                                                            <asp:Chart ID="chrtResultSTPTO" runat="server" AlternateText="Edelweiss" Visible="false"
                                                                                BorderlineWidth="2" Width="650px" Height="580px" IsSoftShadows="false">
                                                                                <Titles>
                                                                                    <asp:Title Font="Roboto, 14pt, style=Bold" Text="Edelweiss" ForeColor="26, 59, 105">
                                                                                    </asp:Title>
                                                                                </Titles>
                                                                                <Legends>
                                                                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Roboto, 8.25pt, style=Bold">
                                                                                    </asp:Legend>
                                                                                </Legends>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="" AlignmentStyle="PlotPosition"
                                                                                        BackSecondaryColor="White" BackColor="White" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                                        BackHatchStyle="None" BorderDashStyle="Solid">
                                                                                        <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="Time period">
                                                                                            <LabelStyle Format="yyyy" />
                                                                                            <ScaleBreakStyle Enabled="false" />
                                                                                            <ScaleView SizeType="Years" />
                                                                                            <MajorGrid Enabled="false" />
                                                                                        </AxisX>
                                                                                        <AxisY ArrowStyle="None" ToolTip="Value(₹)" TextOrientation="Horizontal" LineColor="#013974">
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
                                                                                                                                                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                                                                                                                Text="SUNDARAM SIP Chart" ForeColor="26, 59, 105">
                                                                                                                                                            </asp:Title>
                                                                                                                                                        </Titles>
                                                                                                                                                        <Legends>
                                                                                                                                                            <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                                                                                                Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold">
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
                                                                    </asp:View>
                                                                </td>
                                                                <td>
                                                                    <asp:View ID="View3" runat="server">
                                                                        <%--<div id="country3" class="tabcontent">--%>
                                                                        <asp:GridView ID="GridViewSIPResult" runat="server" Width="98%" RowStyle-CssClass="grdRow"
                                                                            HeaderStyle-BackColor=" #02509b" AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                            AllowPaging="false" AutoGenerateColumns="false" OnRowDataBound="GridViewSIPResult_RowDataBound"
                                                                            Visible="false">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)"%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="360px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="1 Year">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString()+((Eval("1 Year").ToString()=="N/A")?"":"%")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="borderlefft" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="3 Year">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="borderlefft" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="5 Year">
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
                                                                        <asp:GridView ID="GridViewSTPTOResult" runat="server" Width="98%" RowStyle-CssClass="grdRow"
                                                                            HeaderStyle-BackColor="#02509b" AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                            AllowPaging="false" AutoGenerateColumns="false" Visible="false">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() + " (CAGR)"%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="1 Year">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="borderlefft" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="3 Year">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="borderlefft" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="5 Year">
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
                                                                            <asp:GridView ID="GridViewResultLS" runat="server" Width="100%" AutoGenerateColumns="false" BorderWidth="0" CssClass="table">
                                                                                <Columns>
                                                                                    <%--<asp:TemplateField
                                                                                                                    HeaderText="Type"> <ItemTemplate> <%# (Eval("Type") == DBNull.Value) ? "--" : Eval("Type").ToString()%>
                                                                                                                    </ItemTemplate> <ItemStyle HorizontalAlign="Left" /> </asp:TemplateField>--%>
                                                                                    <asp:TemplateField HeaderText="Scheme Name">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Scheme_Name") == DBNull.Value)
                                                                                                                                ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)" %>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" CssClass="borderbottom" Width="360px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="1 Year">
                                                                                        <ItemTemplate>
                                                                                            <%#
                                                                                                                            (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="3 Year">
                                                                                        <ItemTemplate>
                                                                                            <%#
                                                                                                                            (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="5 Year">
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
                                                                        <div id="Showpdfdiv" runat="server" class="FieldHead">
                                                                            <%-- <b>Please select your Credential:</b>--%>
                                                                            <table class="table table-bordered">
                                                                                <tr>
                                                                                    <td align="left" width="50%" style="border: none">
                                                                                        <asp:RadioButtonList ID="RadioButtonListCustomerType" runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged" TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true" BorderWidth="0" Width="250px" CssClass="lefft">
                                                                                            <asp:ListItem>&nbsp;Distributor</asp:ListItem>
                                                                                            <asp:ListItem Selected="true">&nbsp;Not a Distributor</asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                        <br />
                                                                                    </td>
                                                                                    <td align="left" width="50%" style="border: none">&nbsp;</td>
                                                                                </tr>
                                                                            </table>

                                                                            <table id="tblDistb" width="50%" align="left">
                                                                                <div id="DistributerDiv" runat="server" visible="false">
                                                                                    <tr>
                                                                                        <td align="left">ARN No</td>
                                                                                        <td align="left" style="padding-bottom: 10px;">
                                                                                            <%--<asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>--%>
                                                                                            <asp:TextBox ID="txtArn" CssClass="ddl_3" runat="server" MaxLength="30"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 10px;">Prepared By</td>
                                                                                        <td align="left" style="padding-bottom: 10px;">
                                                                                            <asp:TextBox ID="txtPreparedby" CssClass="ddl_3" runat="server" MaxLength="40"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 10px;">Contact No(Mobile)</td>
                                                                                        <td align="left" style="padding-bottom: 10px;">
                                                                                            <asp:TextBox ID="txtMobile" CssClass="ddl_3" runat="server" MaxLength="14"></asp:TextBox>
                                                                                            <%--onkeypress="return isNumber(event)"--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 10px;">Email</td>
                                                                                        <td align="left" style="padding-bottom: 10px;">
                                                                                            <asp:TextBox ID="txtEmail" CssClass="ddl_3" runat="server" MaxLength="30"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 10px;">Prepared For</td>
                                                                                        <td align="left" style="padding-bottom: 10px;">
                                                                                            <asp:TextBox ID="txtPreparedFor" CssClass="ddl_3" runat="server" MaxLength="40"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </div>
                                                                                <tr>
                                                                                    <td width="25%" align="left">Generate PDF Report: </td>
                                                                                    <td align="left">
                                                                                        <asp:LinkButton ID="LinkButtonGenerateReport" runat="server"
                                                                                            OnClick="LinkButtonGenerateReport_Click"
                                                                                            ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();"><img src="IMG/downloadPDF.png" style="border: 0;" /></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>


                                                                        </div>
                                                                    </asp:View>
                                                                </td>
                                                                <td>
                                                                    <asp:View ID="View5" runat="server">
                                                                        <div id="showDivHistory" runat="server">
                                                                            <asp:GridView ID="gridDivHistory" runat="server" AutoGenerateColumns="False" class="table" BorderWidth="0">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="NAV DATE">
                                                                                        <ItemTemplate>
                                                                                            <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(Eval("Nav_Date")).ToString("dd-MMM-yyyy")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" />

                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Dividend">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Dividend") == DBNull.Value) ? "--" : (Eval("DIVIDEND") == string.Empty) ? "--" : Convert.ToDouble(Eval("DIVIDEND")).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Bonus">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Bonus") == DBNull.Value) ?  "--" : (Eval("BONUS") == string.Empty) ? "--" :  Eval("BONUS").ToString()%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Payout Amount">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("PAYOUT_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("PAYOUT_AMOUNT")), 2).ToString("n2")%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="right" />
                                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
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
                                </div>
                            </div>
                            <div id="disclaimerDiv" runat="server" style="text-align: justify; margin-top: 20px; font-size: 13px; text-align: justify; color: #8a8787;">
                                <table cellpadding="0" cellspacing="0" align="left" border="0" width="100%">
                                    <tr>
                                        <td valign="top">
                                            <span style="color: #034ea2; font-size: 14px; font-weight: 600;">Disclaimer:</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: justify; color: #8a8787; font-size: 13px;">
                                            <asp:Label ID="LSDisc" runat="server" Text="<b><br/>* Returns here denote the Extended Internal Rate of Return (XIRR).  </br></b>"
                                                Visible="true"></asp:Label>
                                            <asp:Label ID="LSDisc1" runat="server" Text="<b><br/>* For Time Periods > 1 yr, CAGR Returns have been shown. For Time Periods < 1 yr, Absolute Returns have been shown. </br></b>"
                                                Visible="false"></asp:Label>
                                            <b>• Since Inception return of the benchmark is calculated from the scheme inception date.</b>
                                            <br />
                                            <%-- <b>• The returns for funds with the dividend option are calculated assuming that the
                                                                                                    dividends are reinvested into the fund.</b>--%>
                                        Past performance may or may not be sustained in the future and should not be used as a basis for comparison with other investments.
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td valign="top" class="rslt_text1">
                                            <div align="justify" style="text-align: justify; color: #8a8787; font-size: 13px;">
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
                                        <td valign="top" style="text-align: justify; color: #8a8787; font-size: 13px;">The return calculator has been developed and is maintained by ICRA Analytics Limited.
                                            Edelweiss Asset Management Limited (EAML)  do not endorse the authenticity
                                            or accuracy of the figures based on which the returns are calculated; nor shall
                                            they be held responsible or liable for any error or inaccuracy or for any losses
                                            suffered by any investor as a direct or indirect consequence of relying upon the
                                            data displayed by the calculator.<br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="Table2" class="table" cellpadding="0" cellspacing="0" align="left" border="0" width="100%">
                                                <tr>
                                                    <td class="text" align="right" style="border-bottom: none">
                                                        <span style="text-align: justify; font-size: 11px;">Developed by:<a class="text" href="https://www.icraanalytics.com"
                                                            target="_blank"> ICRA Analytics Ltd </a>, <a class="text" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer </a></span>
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
            </div>
        </div>
    </form>
</body>
</html>
