<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always"
    EnableViewStateMac="true" CodeBehind="returnCalc.aspx.cs" Inherits="iFrames.Sundaram.returnCalcUat" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sundaram Calculator </title>
    <script src="Script/jquery-1.8.3.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jshashtable-2.1_src.js"></script>
    <script type="text/javascript" src="Script/jquery.numberformatter-1.2.3.js"></script>
    <script type="text/javascript" src="Script/tmpl.js"></script>
    <script type="text/javascript" src="Script/jquery.dependClass-0.1.js"></script>
    <script src="Script/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Script/jquery.ui.core.js" type="text/javascript"></script>
    <%--<script src="Script/jquery.ui.datepicker.js" type="text/javascript"></script>--%>
    <script src="Script/check.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>
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

    <link rel="stylesheet" href="css/new/bootstrap.min.css" />
    <link rel="stylesheet" href="css/new/jquery-ui.css" />
    <link rel="stylesheet" href="css/new/all.css" integrity="sha384fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr"
        crossorigin="anonymous" />
    <link rel="stylesheet" href="css/new/bootstrap-datepicker.css" />
    <link href="https://fonts.googleapis.com/css?family=Ubuntu:300,400,500,700" rel="stylesheet" />
    <link href="css/new/select2.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/new/style.css" />

    <script type="text/javascript" language="javascript">

        $(function () {

            function GetMinDate() {
                var date = $('#SIPSchDt').val();
                var Day = parseInt(date.substring(0, 2), 10);
                var Mn = parseInt(date.substring(3, 5), 10);
                var Yr = parseInt(date.substring(6, 10), 10);
                var DateVal = Mn + "/" + Day + "/" + Yr;
                dt = new Date(DateVal);

                return dt;
                //return ddScheme.value;
            }
            var year = new Date().getFullYear().toString();
            var schemeLaunchDate = GetMinDate().getFullYear().toString();
            $('#txtvalason').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });

            //$("#txtfromDate").datepicker({
            //    showOn: "button",
            //    buttonImageOnly: true,
            //    buttonImage: "img/calenderb.jpg",
            //    buttonText: "Select Date",
            //    dateFormat: 'dd/mm/yy',
            //    changeMonth: true,
            //    changeYear: true,
            //    maxDate: -1
            //});

            $('#txtfromDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });

            //$("#txtToDate").datepicker({
            //    showOn: "button",
            //    buttonImage: "img/calenderb.jpg",
            //    buttonImageOnly: true,
            //    buttonText: "Select Date",
            //    dateFormat: 'dd/mm/yy',
            //    changeMonth: true,
            //    changeYear: true,
            //    maxDate: -2
            //    //                 maxDate: 0
            //});


            $('#txtToDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });

            //$("#txtvalason").datepicker({
            //    showOn: "button",
            //    buttonImage: "img/calenderb.jpg",
            //    buttonImageOnly: true,
            //    buttonText: "Select Date",
            //    dateFormat: 'dd/mm/yy',
            //    changeMonth: true,
            //    changeYear: true,
            //    maxDate: -1
            //    //                 maxDate: 0
            //});


            //$("#txtIniToDate").datepicker({
            //    showOn: "button",
            //    buttonImage: "img/calenderb.jpg",
            //    buttonImageOnly: true,
            //    buttonText: "Select Date",
            //    dateFormat: 'dd/mm/yy',
            //    changeMonth: true,
            //    changeYear: true,
            //    maxDate: -1
            //    //                 maxDate: 0
            //});

            $('#txtIniToDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });


            //$("#txtLumpfromDate").datepicker({
            //    showOn: "button",
            //    buttonImage: "img/calenderb.jpg",
            //    buttonImageOnly: true,
            //    buttonText: "Select Date",
            //    dateFormat: 'dd/mm/yy',
            //    changeMonth: true,
            //    changeYear: true,
            //    maxDate: -1
            //    //                 maxDate: 0
            //});

            $('#txtLumpfromDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });

            //$("#txtLumpToDate").datepicker({
            //    showOn: "button",
            //    buttonImage: "img/calenderb.jpg",
            //    buttonImageOnly: true,
            //    buttonText: "Select Date",
            //    dateFormat: 'dd/mm/yy',
            //    changeMonth: true,
            //    changeYear: true,
            //    maxDate: -1
            //    //                 maxDate: 0
            //});

            $('#txtLumpToDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });


            setFromDateAfterPostBack();
            $(".datepicker-input").attr("autocomplete", "off");
        });
        
    </script>
    <style>
        .labelComment {
            align-items: flex-end;
        }

        #rdbReinvest {
            width: auto !important;
        }

        #rdbPayout {
            width: auto !important;
        }

        #lmprdbReinvest {
            width: auto !important;
        }

        #lmprdbPayout {
            width: auto !important;
        }
        /*#countrytabs a:active {
                background-color: #2ee9b6;
                color: #2ee9b6;
                border-bottom: none;
                padding: 3px 10px;
            }*/
    </style>
</head>
<body id="nav-page">
    <form id="form1" runat="server">
        <div id="returns-calc-form">
            <section class="mt-2">
                <div class="container">
                    <div class="row">
                        <div class="col-md-12">
                            <h3 class="text-title">Returns Calculator</h3>
                        </div>
                    </div>
                </div>
            </section>
            <section class="nav-divident-form-sec">
                <div class="container">
                    <div class="col-md-12 p-0">
                        <div class="row">
                            <div class="col-md-6 col-12 ">
                                <div class="form-row">
                                    <fieldset class="form-group col-md-6 col-lg-6 col-xl-6 col-12">
                                        <label for="exampleInputEmail1">Investment Mode</label>
                                        <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="True"
                                            CssClass="form-control select2" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">SIP</asp:ListItem>
                                            <asp:ListItem Selected="False">Lump Sum</asp:ListItem>
                                            <asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>
                                            <asp:ListItem Selected="False">SWP</asp:ListItem>
                                            <asp:ListItem Selected="False">STP</asp:ListItem>
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset class="form-group col-md-6 col-lg-6 col-xl-6 col-12">
                                        <div id="trCategory" runat="server" visible="true">
                                            <div class="">
                                                <label for="exampleInputEmail1">Category</label>
                                                <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true"
                                                    DataTextField="Nature" DataValueField="Nature"
                                                    CssClass="form-control select2" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                            <div class="col-md-6 col-12 "></div>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0">
                        <div id="tr1" runat="server" visible="true">
                            <div class="form-row">
                                <fieldset class="form-group col-md-6 col-12">
                                    <div class="">
                                        <label for="exampleInputEmail1">Option</label>
                                        <asp:DropDownList ID="ddlOption" runat="server" AutoPostBack="true"
                                            CssClass="form-control select2" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6 col-12 "></div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0">
                        <div class="form-row" runat="server" visible="true">
                            <fieldset class="col-md-6 col-12 form-group">
                                <asp:Label ID="lblSchemeName" runat="server" Text="Scheme Name" CssClass="lebell"></asp:Label>
                                <asp:DropDownList ID="ddlscheme" runat="server" CssClass="form-control select2"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                </asp:DropDownList>
                            </fieldset>
                            <div class="col-md-6 col-12 "></div>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0">
                        <div class="form-row" visible="true" id="trInception1"
                            runat="server">
                            <fieldset class="col-md-6 col-12 form-group">
                                <asp:Label ID="LabelInception" runat="server" CssClass="FieldHead lebell"
                                    Text="Inception Date"></asp:Label>
                                <asp:TextBox ID="SIPSchDt" runat="server" CssClass="form-control select2"
                                    ReadOnly="true" Text=""></asp:TextBox>
                            </fieldset>
                            <div class="col-md-6 col-12 "></div>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0">
                        <div class="form-row" id="trTransferTo" runat="server" visible="false">
                            <fieldset class="col-md-6 col-12 form-group">
                                <label class="FieldHead">Transfer To</label>
                                <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True" CssClass="form-control select2" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged">
                                </asp:DropDownList>
                            </fieldset>
                            <div class="col-md-6 col-12 "></div>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0">
                        <div class="form-row" id="trInception" runat="server">
                            <fieldset class="col-md-6 col-12 form-group">
                                <label class="FieldHead" id="Span1">Inception Date</label>
                                <asp:TextBox ID="SIPSchDt2" runat="server" CssClass="form-control select2" ReadOnly="true" Text=""></asp:TextBox>
                            </fieldset>
                            <div class="col-md-6 col-12 "></div>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0">
                        <div class="form-row" id="trBenchmark" runat="server" visible="true">
                            <fieldset class="col-md-6 col-12 form-group">
                                <label class="FieldHead" id="Span3">Benchmark</label>
                                <asp:TextBox ID="txtddlsipbnmark" runat="server" CssClass="form-control select2" ReadOnly="true"></asp:TextBox>
                            </fieldset>
                            <div class="col-md-6 col-12 "></div>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0" id="trInitialInvst" runat="server" visible="false">
                        <div class="form-row">
                            <fieldset class="col-md-6 col-12 form-group">
                                <label class="FieldHead">Initial Amount (₹)</label>
                                <asp:TextBox ID="txtiniAmount" runat="server" CssClass="form-control select2" MaxLength="10" ReadOnly="false"></asp:TextBox>
                            </fieldset>
                            <fieldset class="col-md-6 col-12  form-group">
                                <label class="FieldHead"><i class="far fa-calendar-alt"></i>&nbsp;Initial Date </label>
                                <asp:TextBox ID="txtIniToDate" runat="server" CssClass="form-control select2"
                                    onMouseDown="Javascript: setDate();"></asp:TextBox>
                            </fieldset>
                        </div>
                    </div>
                    <div class="col-md-12 col-12 p-0" id="trSipInvst" runat="server" visible="true">
                        <div>
                            <div class="form-row" runat="server" id="SIP_withdrawl">
                                <fieldset class="col-md-6 col-12 form-group">
                                    <asp:Label ID="lblInstallmentAmt" runat="server" Text="Installment Amount (₹)" class="FieldHead lebell"></asp:Label>
                                    <asp:TextBox ID="txtinstall" CssClass="form-control select2" MaxLength="10" Text="" runat="server" ReadOnly="false"
                                        onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                </fieldset>
                                <fieldset class="form-group col-md-6 col-12 form-group">
                                    <asp:Label ID="lblTransferWithdrawal" runat="server"
                                        class="FieldHead" Text="Withdrawal Amount" CssClass="lebell"></asp:Label>
                                    <asp:TextBox ID="txtTransferWithdrawal" runat="server" CssClass="form-control select2" MaxLength="14" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false"></asp:TextBox>
                                </fieldset>
                            </div>
                            <div class="form-row" id="SWP_STP_withdrawl" runat="server" visible="false">
                                <fieldset class="form-group Amountcol-md-6 col-12">
                                    <asp:Label ID="lblTransferWithdrawal2" runat="server" class="FieldHead" Text="Withdrawal Amount" CssClass="lebell"></asp:Label>
                                    <asp:TextBox ID="txtTransferWithdrawal2" runat="server" CssClass="form-control select2" MaxLength="14" onChange="Javascript: checkInvestedValue();" ReadOnly="false"></asp:TextBox>
                                </fieldset>
                                <div class="col-md-6 col-12"></div>
                            </div>
                            <div class="form-row" runat="server">
                                <fieldset class="form-group col-md-6 col-12">
                                    <label class="FieldHead">Frequency</label>
                                    <asp:DropDownList ID="ddPeriod_SIP" runat="server" CssClass="form-control select2">
                                        <%--<asp:ListItem Value="Monthly" Selected="True">Monthly</asp:ListItem>
                                     <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                     <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </fieldset>
                                <fieldset class="form-group col-md-6 col-12">
                                    <asp:Label ID="lblDiffDate" runat="server" Text="SIP Date" CssClass="lebell"></asp:Label>
                                    <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="form-control select2">
                                        <asp:ListItem Value="1">1st</asp:ListItem>
                                        <asp:ListItem Value="7">7th</asp:ListItem>
                                        <asp:ListItem Value="14">14th</asp:ListItem>
                                        <asp:ListItem Value="20">20th</asp:ListItem>
                                        <asp:ListItem Value="25">25th</asp:ListItem>
                                    </asp:DropDownList>
                                </fieldset>
                            </div>
                        </div>
                        <div>
                            <div class="form-row" runat="server">
                                <fieldset class="form-group col-md-6 col-12">
                                    <label for="">
                                        <i class="far fa-calendar-alt"></i>&nbsp;From Date</label>
                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="datepicker-input"></asp:TextBox>
                                </fieldset>
                                <fieldset class="form-group col-md-6 col-12">
                                    <label for=""><i class="far fa-calendar-alt"></i>&nbsp;To Date</label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker-input" onChange="Javascript: setDateValueAsOn(); "></asp:TextBox>
                                </fieldset>
                            </div>
                            <div class="form-row labelComment" runat="server">
                                <fieldset class="form-group col-md-6 col-12">
                                    <label for="">
                                        <i class="far fa-calendar-alt"></i>&nbsp;Current Date</label>
                                    <div>
                                        <asp:TextBox ID="txtvalason" runat="server" CssClass="datepicker-input"></asp:TextBox>

                                    </div>
                                </fieldset>

                                <%--                                <fieldset class="form-group col-md-6 col-12">
                                    <label for=""> <i class="far fa-calendar-alt"></i> Current Date</label>
                                    <asp:TextBox ID="txtvalason" runat="server" CssClass="datepicker-input" AutoCompleteType="None"></asp:TextBox>
                                    <small id="passwordHelpBlock" class="form-text text-muted"> ('Current Date' should be greater than 'To Date')</small>
                                </fieldset>--%>
                                <div class="form-group col-md-6 col-12">
                                    <label>('Current Date' should be greater than 'To Date')</label>
                                </div>
                            </div>
                            <div class="col-md-12 p-0">
                                <div class="row">
                                    <div class="col-md-6 col-12 ">
                                        <div class="form-row">
                                            <fieldset class="col-md-6 col-12" id="tr_SIP_Sinc" runat="server">
                                                <div class="align-self-center">
                                                    <input type="checkbox" id="chkInception4sip" runat="server" onclick="setIncDate();" style="width: auto;" />
                                                    <label class="" for="checkbox-ince">Calculate from Inception</label>
                                                </div>
                                            </fieldset>
                                            <fieldset class="form-group col-md-6 col-12" id="tr_Cal_Type" runat="server" visible="false">
                                                <div class="align-self-center">
                                                    <asp:RadioButton Text="Reinvest" ID="rdbReinvest" Checked="true" runat="server" GroupName="rdbCalc" />&nbsp;&nbsp;
                                                    <asp:RadioButton Text="Payout" ID="rdbPayout" runat="server" GroupName="rdbCalc" />
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12 col-12 p-0" id="trLumpInvst" runat="server" visible="false">
                        <div class="form-row">
                            <fieldset class="form-group col-md-6 col-12">
                                <label class="FieldHead">Investment Amount (₹)</label>
                                <asp:TextBox ID="txtinstallLs" value="" MaxLength="10" runat="server" CssClass="ddl_3" onmousedown="Javascript: setDate();" onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                            </fieldset>
                            <div class="form-group col-md-6 col-12 "></div>
                        </div>
                        <div class="form-row">
                            <fieldset class="form-group col-md-6 col-12">
                                <label class="FieldHead"><i class="far fa-calendar-alt"></i>Start Date</label>
                                <asp:TextBox ID="txtLumpfromDate" runat="server" CssClass="datepicker-input" onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();"></asp:TextBox>
                            </fieldset>
                            <fieldset class="form-group col-md-6 col-12 ">
                                <label class="FieldHead"><i class="far fa-calendar-alt"></i>End Date</label>
                                <asp:TextBox ID="txtLumpToDate" runat="server" CssClass="datepicker-input" onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="form-row">
                            <fieldset class="form-group col-md-6 col-12" id="tr_Cal_Incp" runat="server" visible="false">
                                <div class="align-self-center">
                                    <input type="checkbox" id="chkInception" runat="server" onclick="setIncDate();" style="width: auto;" />
                                    <label class="" for="checkbox-ince">Calculate from Inception</label>
                                </div>
                            </fieldset>
                        </div>
                        <div class="form-row">
                            <fieldset class="form-group col-md-6 col-12" id="trCalTypeLmpsm"
                                runat="server" visible="false">
                                <asp:RadioButton Text=" Reinvest" ID="lmprdbReinvest"
                                    Checked="true" runat="server" GroupName="rdbCalc" />
                                <asp:RadioButton Text=" Payout" ID="lmprdbPayout" runat="server"
                                    GroupName="rdbCalc" />
                            </fieldset>
                        </div>
                    </div>

                    <div class="col-md-12 col-12 p-0">
                        <div class="row">
                            <div class="col-md-6 col-12 action-button">
                                <asp:Button ID="sipbtnshow" runat="server" Text="Calculate"
                                    CssClass="btn solid-Blue-button" OnClick="sipbtnshow_Click" />
                                <%--<asp:Button ID="sipbtnshow" runat="server" OnClick="sipbtnshow_Click" CssClass="btn solid-Blue-button"></asp:Button>--%>
                                <%--<input type="submit" id="sipbtnshow" runat="server" OnClick="sipbtnshow_Click" value="Calculate" class="btn solid-Blue-button" />--%>
                                <asp:Button ID="sipbtnreset" runat="server" OnClick="sipbtnreset_Click" CssClass="btn btn-danger solid-Danger-button" Text="Reset"></asp:Button>
                                <%--<input type="submit" id="sipbtnreset" runat="server" class="btn btn-danger solid-Danger-button" value="Reset" />--%>
                            </div>
                            <div class="col-md-6 col-12 "></div>
                        </div>
                    </div>

                    <div class="col-md-12 col-12">
                        <div class="row">
                                <div id="resultDiv" runat="server" visible="false" class="table-responsive returnCalc">
                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="display: none" class="table">
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
                                                <%--<span class="rslt_text">
                    On 05/07/2012, the value of your total investment Rs 120000
                    would be Rs <strong>131779.37</strong>
                </span>--%>
                                                <asp:Label ID="lblInvstvalue" CssClass="rslt_text" runat="server" Text="On Date C, the Scheme value of your total investment ₹ Y would be ₹ Z">
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
                                                <%--<span class="rslt_text">
                                            XIsRR return of Investment from 01/07/2010 to 01/07/2012 is <strong>
                                                9.17%
                                            </strong>
                                        </span>--%>
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
                                                    runat="server" Text="Had you invested ₹ Y at Date A, the total value of this investment at Date C would have become Q">
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
                                    <table width="100%" id="firstTable" class="table mt-4">
                                        <tr>
                                            <td style="padding: 0">
                                                <div id="divSummary" runat="server">
                                                    <asp:GridView ID="gvFirstTable" runat="server" AutoGenerateColumns="False"
                                                        Width="100%"
                                                        HeaderStyle-CssClass="grdHead" Visible="false"
                                                        AlternatingRowStyle-CssClass="grdRow"
                                                        RowStyle-CssClass="grdRow">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Scheme") == DBNull.Value) ? "--" : Eval("Scheme").ToString()%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" CssClass="leftal"
                                                                    Width="380px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Units Purchased"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_unit") == DBNull.Value) ? "--" : Eval("Total_unit").ToString().Trim() == "0" ? "--" : Math.Round(Convert.ToDouble( Eval("Total_unit")),0).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount  Invested<br/> (A)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_amount") == DBNull.Value) ? "--" : Eval("Total_amount").ToString().Trim() == "0" ? "--" : "₹  " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Investment Value as<br/>  on Date (B)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : Eval("Present_Value").ToString().Trim() == "0" ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dividend Amount (C)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Dividend_Amount") == DBNull.Value || Eval("Dividend_Amount") == "--" || Eval("Dividend_Amount").ToString() == "0") ? "--" : " ₹  " + Math.Round(Convert.ToDouble(Eval("Dividend_Amount")),2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Profit <br/>(B+C-A)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : Eval("Profit_Sip").ToString().Trim() == "0" ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 2).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Absolute <br/>Returns"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : Eval("ABSOLUTERETURN").ToString().Trim() == "0" ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Returns*" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString().Trim() == "0" ? "--" : (Eval("Yield").ToString() + "%")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <br />
                                                    <span id="sipDisclaimer" class="FieldHead" style="padding: 4px; margin: 2px; border: none" visible="false" runat="server"></span>
                                                    <asp:GridView ID="gvSWPSummaryTable" runat="server"
                                                        AutoGenerateColumns="False" Width="100%"
                                                        HeaderStyle-CssClass="grdHead" Visible="false"
                                                        AlternatingRowStyle-CssClass="grdRow"
                                                        RowStyle-CssClass="grdRow">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="210px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount<BR/> Invested (A)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount<BR/> Withdrawn (B)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "₹  " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Present<BR/> Value (C)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Profit<BR/> (B+C-A)"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# "₹ "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Abs. <br/>Returns" HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Returns *" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="gvSTPToSummaryTable" runat="server"
                                                        AutoGenerateColumns="False"
                                                        Width="100%" HeaderStyle-CssClass="grdHead" Visible="false"
                                                        AlternatingRowStyle-CssClass="grdRow"
                                                        RowStyle-CssClass="grdRow">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="40%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Amount Invested">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                    <%--Eval("Total_Amount_Invested").ToString()--%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" Width="24%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Present Value">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
                                                                    <%--TwoDecimal(Eval("Present_Value").ToString())--%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" Width="18%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Yield">
                                                                <ItemTemplate>
                                                                    <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="borderbottom" Width="18%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                        <tr align="left">
                                                            <td valign="top" colspan="2" style="border: none; padding: 0">
                                                                <asp:GridView ID="GridViewLumpSum" runat="server" Width="98%"
                                                                    CssClass="grdRow" HeaderStyle-BackColor="#2ee9b6"
                                                                    RowStyle-CssClass="grdRow" AlternatingRowStyle-CssClass="grdRow"
                                                                    HeaderStyle-CssClass="grdHead"
                                                                    AutoGenerateColumns="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" CssClass="leftal"
                                                                                Width="230px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Amount Invested (A)"
                                                                            HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("InvestedAmount") == DBNull.Value) ? "--" : "₹  " + Convert.ToDouble(Eval("InvestedAmount")).ToString("n2")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Investment Value<br/>(B)"
                                                                            HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "₹ " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Dividend Amount<br/>(C)"
                                                                            HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("DividendAmount") == "--") ? "--" : "₹ " + Eval("DividendAmount")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Profit from Investment<br/>(B+C-A)"
                                                                            HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("Profit") == DBNull.Value) ? "--" : "₹ " + Convert.ToDouble(Eval("Profit")).ToString("n2")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Return *<br/>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%# (Eval("Return") == DBNull.Value) ? "--" : Eval("Return").ToString() + ((Eval("Return").ToString() == "N/A") ? "" : "%")%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" id="TableRemark" class="table" cellpadding="0" cellspacing="0" align="center" style="padding: 0">
                                        <tr>
                                            <td>
                                                <div id="divTab" runat="server" visible="false" style="padding: 0">
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="padding: 0">
                                                        <tr align="left">
                                                            <td align="left" valign="top" style="padding: 0">
                                                                <ul id="countrytabs" class="nav nav-tabs">
                                                                    <li class="nav-item">
                                                                        <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click"
                                                                            class="nav-link active">View Detail Report</asp:LinkButton>
                                                                    </li>
                                                                    <li class="nav-item">
                                                                        <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click"
                                                                            class="nav-link">View Graph</asp:LinkButton>
                                                                    </li>
                                                                    <li class="nav-item">
                                                                        <asp:LinkButton ID="lnkTab3" runat="server" Visible="false"
                                                                            OnClick="lnkTab3_Click" class="nav-link">View Historical Performance</asp:LinkButton>
                                                                    </li>
                                                                    <li class="nav-item">
                                                                        <asp:LinkButton ID="lnkTab4" runat="server" OnClick="lnkTab4_Click"
                                                                            class="nav-link">View PDF Report</asp:LinkButton>
                                                                    </li>
                                                                    <li class="nav-item">
                                                                        <asp:LinkButton ID="lnkTab5" runat="server" Visible="false"
                                                                            OnClick="lnkTab5_Click" class="nav-link">View Dividend History</asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                                <div class="tab-content">
                                                                    <div style="width: 100%; padding: 5px;">
                                                                        <asp:MultiView ID="MultiView1" runat="server">
                                                                            <table width="100%" cellpadding="2" cellspacing="5">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:View ID="View1" runat="server">
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td align="right" style="padding-right: 20px">
                                                                                                        <asp:ImageButton ID="btnExcelCalculation" runat="server"
                                                                                                            ImageUrl="~/SundaramUAT/IMG/excel.jpg"
                                                                                                            ToolTip="Download Excel" Text="Show Excel Calculation"
                                                                                                            Visible="true" OnClick="ExcelCalculation_Click" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <div id="DetailDiv" runat="server" class="taB tab-pane fade show active">
                                                                                                <%--<div id="country1" class="tabcontent">
                                                                                                --%>
                                                                                                <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False"
                                                                                                    Width="98%"
                                                                                                    HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow"
                                                                                                    RowStyle-CssClass="grdRow">
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
                                                                                                            <ItemStyle CssClass="" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Scheme Units">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Scheme_units") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_units")).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("SCHEME_UNITS_CUMULATIVE") == DBNull.Value) ? "--" :Math.Round(Convert.ToDouble( Eval("SCHEME_UNITS_CUMULATIVE")),2).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="SIP Amount">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Scheme_cashflow") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_cashflow")).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Cumulative Fund Value">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("CUMULATIVE_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMULATIVE_AMOUNT")), 2).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                                <asp:GridView ID="swpGridView" runat="server" AutoGenerateColumns="False"
                                                                                                    Width="98%" HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow"
                                                                                                    RowStyle-CssClass="grdRow">
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
                                                                                                    <asp:GridView ID="stpFromGridview" runat="server" AutoGenerateColumns="False"
                                                                                                        Width="98%"
                                                                                                        HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow"
                                                                                                        RowStyle-CssClass="grdRow">
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
                                                                                                    <b>To Scheme:
                                                                                    <%= ddlschtrto.SelectedItem.Text %>
                                                                                                    </b>
                                                                                                    <br />
                                                                                                    <asp:GridView ID="stpToGridview" runat="server" AutoGenerateColumns="False"
                                                                                                        Width="98%" HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow"
                                                                                                        RowStyle-CssClass="grdRow">
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
                                                                                                <%--
                                                                        </div>--%>
                                                                                                <br />

                                                                                            </div>
                                                                                        </asp:View>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:View ID="View2" runat="server">
                                                                                            <%--  <div id="country2" class="tabcontent">
                                                                                            --%>
                                                                                            <div id="divshowChart" runat="server" visible="true"
                                                                                                style="width: 100%;">
                                                                                                <asp:Chart ID="chrtResult" runat="server" AlternateText="SUNDARAM"
                                                                                                    Visible="true"
                                                                                                    BorderlineWidth="2" Width="650px" Height="580px"
                                                                                                    IsSoftShadows="false">
                                                                                                    <%--BackGradientStyle="Center"  BorderlineColor="RoyalBlue" BackColor="Gray"--%>
                                                                                                    <Titles>
                                                                                                        <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="SUNDARAM"
                                                                                                            ForeColor="26, 59, 105">
                                                                                                        </asp:Title>
                                                                                                    </Titles>
                                                                                                    <Legends>
                                                                                                        <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend"
                                                                                                            BackColor="Transparent"
                                                                                                            Alignment="Center" LegendStyle="Row" Docking="Bottom"
                                                                                                            Font="Trebuchet MS, 8.25pt, style=Bold">
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
                                                                                                <asp:Chart ID="chrtResultSTPTO" runat="server" AlternateText="SUNDARAM"
                                                                                                    Visible="false"
                                                                                                    BorderlineWidth="2" Width="650px" Height="580px"
                                                                                                    IsSoftShadows="false">
                                                                                                    <Titles>
                                                                                                        <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="SUNDARAM"
                                                                                                            ForeColor="26, 59, 105">
                                                                                                        </asp:Title>
                                                                                                    </Titles>
                                                                                                    <Legends>
                                                                                                        <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend"
                                                                                                            BackColor="Transparent"
                                                                                                            Alignment="Center" LegendStyle="Row" Docking="Bottom"
                                                                                                            Font="Trebuchet MS, 8.25pt, style=Bold">
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
                                                                                                <%--
                                                                        </div>--%>
                                                                                            </div>
                                                                                        </asp:View>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:View ID="View3" runat="server">
                                                                                            <%--<div id="country3" class="tabcontent">
                                                                                            --%>
                                                                                            <asp:GridView ID="GridViewSIPResult" runat="server"
                                                                                                Width="98%" RowStyle-CssClass="grdRow"
                                                                                                HeaderStyle-BackColor=" #2ee9b6" AlternatingRowStyle-CssClass="grdRow"
                                                                                                HeaderStyle-CssClass="grdHead"
                                                                                                AllowPaging="false" AutoGenerateColumns="false"
                                                                                                OnRowDataBound="GridViewSIPResult_RowDataBound"
                                                                                                Visible="false">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="">
                                                                                                        <ItemTemplate>
                                                                                                            <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)"%>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Left" CssClass="leftal"
                                                                                                            Width="360px" />
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
                                                                                            <asp:GridView ID="GridViewSTPTOResult" runat="server"
                                                                                                Width="98%" RowStyle-CssClass="grdRow"
                                                                                                HeaderStyle-BackColor="#2ee9b6" AlternatingRowStyle-CssClass="grdRow"
                                                                                                HeaderStyle-CssClass="grdHead"
                                                                                                AllowPaging="false" AutoGenerateColumns="false"
                                                                                                Visible="false">
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
                                                                                                <asp:GridView ID="GridViewResultLS" runat="server"
                                                                                                    Width="98%" HeaderStyle-BackColor="#2ee9b6" BorderColor="Transparent"
                                                                                                    BorderStyle="None" AutoGenerateColumns="false">
                                                                                                    <Columns>
                                                                                                        <%--<asp:TemplateField HeaderText="Type">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("Type") == DBNull.Value) ? "--" : Eval("Type").ToString()%>
                                                                                        </ItemTemplate> <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>--%>
                                                                                                        <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Scheme_Name") == DBNull.Value)
                                                                                            ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)" %>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="left" CssClass="leftal"
                                                                                                                Width="360px" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="1 Year">
                                                                                                            <ItemTemplate>
                                                                                                                <%#
                                                                                            (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="3 Year">
                                                                                                            <ItemTemplate>
                                                                                                                <%#
                                                                                            (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="5 Year">
                                                                                                            <ItemTemplate>
                                                                                                                <%#
                                                                                            (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Since Inception">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderbottom" HorizontalAlign="center" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                            <%--
                                                                    </div>--%>
                                                                                        </asp:View>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:View ID="View4" runat="server">
                                                                                            <div id="Showpdfdiv" runat="server" class="FieldHead">
                                                                                                <%-- <b>Please select your Credential:</b>--%>

                                                                                                <%--<div class="row mt-3">
                                                                                                    <div class="col col-lg-2">
                                                                                                        <div class="form-group" >
                                                                                                            <label class="radio-inline">
                                                                                                                <input type="radio" name="Dis" id="DisChk" onclick="nonDis()" checked> Distributor </label>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col col-lg-2">
                                                                                                        <label class="radio-inline">
                                                                                                            <input type="radio" name="Dis" id="NDisChk" onclick="disDet()">Not a Distributor </label>
                                                                                                    </div>
                                                                                                </div>--%>



                                                                                                <table width="100%" style="padding-top: 20px;">
                                                                                                    <tr>
                                                                                                        <td align="left" width="100%" style="padding-left: 0; padding-bottom: 0">
                                                                                                            <asp:RadioButtonList ID="RadioButtonListCustomerType"
                                                                                                                runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged"
                                                                                                                TextAlign="Right" RepeatDirection="Horizontal"
                                                                                                                AutoPostBack="true" CssClass="radio-inline">
                                                                                                                <asp:ListItem>Distributor</asp:ListItem>
                                                                                                                <asp:ListItem Selected="true">Not a Distributor</asp:ListItem>
                                                                                                            </asp:RadioButtonList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <div id="DistributerDiv" runat="server" visible="false">
                                                                                                    <div class="row" id="tblDistb">
                                                                                                        <div class="col-md-2 col">
                                                                                                            <div class="form-group">
                                                                                                                <label>ARN Number</label>
                                                                                                                <asp:TextBox ID="txtArn" CssClass="form-control"
                                                                                                                    runat="server" MaxLength="30"></asp:TextBox>
                                                                                                            </div>
                                                                                                        </div>

                                                                                                        <div class="col-md-2 col">
                                                                                                            <div class="form-group">
                                                                                                                <label>Mobile</label>
                                                                                                                <asp:TextBox ID="txtMobile" CssClass="form-control"
                                                                                                                    runat="server" MaxLength="14"></asp:TextBox>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col-md-2 col">
                                                                                                            <div class="form-group">
                                                                                                                <label>Email</label>
                                                                                                                <asp:TextBox ID="txtEmail" CssClass="form-control"
                                                                                                                    runat="server" MaxLength="30"></asp:TextBox>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col-md-3 col">
                                                                                                            <div class="form-group">
                                                                                                                <label>Prepared by</label>
                                                                                                                <asp:TextBox ID="txtPreparedby" CssClass="form-control"
                                                                                                                    runat="server" MaxLength="40"></asp:TextBox>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col-md-3 col">
                                                                                                            <div class="form-group">
                                                                                                                <label>Prepared for</label>
                                                                                                                <asp:TextBox ID="txtPreparedFor" CssClass="form-control"
                                                                                                                    runat="server" MaxLength="40"></asp:TextBox>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>

                                                                                                </div>
                                                                                                <div class="row">
                                                                                                    <div class="col-md-12 text-center">
                                                                                                        <%--<button type="button" class="btn solid-Blue-button">Generate PDF</button>--%>
                                                                                                        <asp:LinkButton ID="LinkButtonGenerateReport" runat="server"
                                                                                                            OnClick="LinkButtonGenerateReport_Click"
                                                                                                            ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();"
                                                                                                            CssClass="btn solid-Blue-button">Generate PDF</asp:LinkButton>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <%--<table width="100%">
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
                                                                                                </table>--%>
                                                                                            </div>
                                                                                        </asp:View>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:View ID="View5" runat="server">
                                                                                            <div id="showDivHistory" runat="server">
                                                                                                <br>
                                                                                                <asp:GridView ID="gridDivHistory" runat="server" AutoGenerateColumns="False"
                                                                                                    Width="98%" HeaderStyle-BackColor=" #2ee9b6" BorderStyle="None"
                                                                                                    BorderWidth="0" BorderColor="Transparent">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="NAV Date">
                                                                                                            <ItemTemplate>
                                                                                                                <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(Eval("Nav_Date")).ToString("dd-MMM-yyyy")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="NAV">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Dividend">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("DIVIDEND") == DBNull.Value) ? "--" : (Eval("DIVIDEND") == string.Empty) ? "--" : Convert.ToDouble(Eval("DIVIDEND")).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Bonus">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("BONUS") == DBNull.Value) ?  "--" : (Eval("BONUS") == string.Empty) ? "--" :  Eval("BONUS").ToString()%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Payout Amount">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("PAYOUT_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("PAYOUT_AMOUNT")), 2).ToString("n2")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
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
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <%-- <div id="resultDivLS" runat="server" visible="false">
                    <ul id="Ul1" class="shadetabs">
                        <li>
                            <asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnkTab5_Click">View Historical Performance</asp:LinkButton>
                        </li>
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
                        </div>
                    </div>
                </div>
            </section>
            <section class="mt-4" id="disclaimerDiv" runat="server">
                <div class="container">
                    <div class="row">
                        <%--<asp:Label ID="LSDisc1" runat="server"></asp:Label>--%>
                        <div class=" col-12  return-calc-disclaimer">
                            <p>
                                <b>Disclaimer:</b><br />
                                <asp:Label ID="LSDisc" runat="server" Text="* Returns here denote the Extended Internal Rate of Return (XIRR)."
                                    Visible="true"></asp:Label>
                                <asp:Label ID="LSDisc1" runat="server" Text="<b><br/>* For Time Periods > 1 yr, CAGR Returns have been shown. For Time Periods < 1 yr, Absolute Returns have been shown. </br></b>"
                                    Visible="false"></asp:Label>
                                <br />
                                Benchmark Returns are based on Total Return Index.
                            </p>
                            <p><b>Since Inception return of the benchmark is calculated from the scheme inception date.</b></p>
                            <p>
                                Past performance may or may not be sustained in the future and should not be used as a basis for comparison with other investments.
                            </p>
                            <p>
                                <asp:Label ID="lblDisclaimer" runat="server"></asp:Label>
                            </p>
                            <p>
                                The return calculator has been developed and is maintained by <a class="text" href="http://www.icraonline.com" target="_blank">ICRA Online Limited.</a> Sundaram Asset Management Company Limited/ Trustees do not endorse the authenticity or accuracy of the figures based on which the returns are calculated; nor shall they be held responsible or liable for any error or inaccuracy or for any losses suffered by any investor
                                as a direct or indirect consequence of relying upon the data displayed by the calculator.
                            </p>
                            <%--<p>
                                <asp:Label ID="LSDisc" runat="server" Text="* Returns here denote the Extended Internal Rate of Return (XIRR)."
                                    Visible="true"></asp:Label>
                            </p>--%>
                        </div>
                    </div>
                </div>
            </section>
        </div>

    </form>
    <script src="Script/new/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
        crossorigin="anonymous"></script>
    <script src="Script/new/jquery-ui.min.js" integrity="sha256-eGE6blurk5sHj+rmkfsGYeKyZx3M4bG+ZlFyA7Kns7E="
        crossorigin="anonymous"></script>
    <script src="Script/new/bootstrap.min.js"></script>
    <script src="Script/new/popper.min.js"></script>
    <script src="Script/new/bootstrap-datepicker.js"></script>
    <script src="Script/new/underscore.js"></script>
    <script src="Script/new/select2.min.js"></script>



</body>
</html>
