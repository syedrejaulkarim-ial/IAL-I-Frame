<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnCalc.aspx.cs" Inherits="iFrames.BloomKite.Return_Calc" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Return Calculator</title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />


    <link rel="stylesheet" href="css/new/jquery-ui.css" />
    <link rel="stylesheet" href="css/new/all.css" />
    <link rel="stylesheet" href="css/new/bootstrap-datepicker.css" />

    <script src="js/jquery.js" type="text/javascript"></script>

    <script src="js/check.js" type="text/javascript"></script>
    <script src="js/new/new/jquery_new.min.js"></script>
    <script src="js/new/new/bootstrap.min.js"></script>
    <script src="js/new/new/bootstrap-datepicker.js"></script>

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
            //var currDate = GetToDate().getFullYear().toString();

            $('#txtvalason').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });


            $('#txtfromDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });



            $('#txtToDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });



            $('#txtIniToDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });


            $('#txtLumpfromDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: GetMinDate(),
                endDate: schemeLaunchDate + ":" + year
                //startDate: '-3d'
            });



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
            /* width: auto !important; */
            margin-right: 10px;
        }

        #rdbPayout {
            width: auto !important;
            margin-right: 10px;
            margin-left: 15px;
        }

        #lmprdbReinvest {
            width: auto !important;
        }

        #lmprdbPayout {
            width: auto !important;
        }

        .radio label, .checkbox label {
            min-height: 20px;
            padding-left: 20px;
            margin-bottom: 0;
            font-weight: 400;
            cursor: pointer;
        }

        .text-label {
            /*font-weight: 400;*/
            color: #000000;
            font-size: 16px;
            font-style: normal;
            margin-bottom: 0.5rem;
        }
        .text-label1 {
            /*font-weight: 400;*/
            color: #000000;
            font-size: 16px;
            font-style: normal;
        }
        .nav-tabs > li > a {
            padding: 8px 12px!important;
            font-size:14px;
            font-weight:600;
            border-radius: 10px;
            color: #251534;
        }
        .nav-tabs > li.active > a, .nav-tabs > li.active > a:hover, .nav-tabs > li.active > a:focus{
            color:#fff ;  
            background-color: #251534;
        }

        .nav-tabs{
            background: #f0f0f0;
            border-bottom: 1px solid #ddd;
        }
        .nav > li > a:hover, .nav > li > a:focus {
            text-decoration: none;
            background-color: #d6d6d6;
}
        .nav-tabs .nav-link.active {
            color: #fff;
            background-color: #251534;
        }

        
        #RadioButtonListCustomerType_0 {
            margin-right: 10px
        }

        #RadioButtonListCustomerType_1 {
            margin-right: 10px
        }

        .text-center {
            text-align: center;
        }

        th {
            text-align: center;
        }
    
        .dspNone {
            display: none;
        }
        .login-btn {
            background: #7142ed;
            padding: 10px 12px;
            font-size: 18px;
            display: inline-block;
            color:#fff;
            margin-right:14px;
        }
        .reset-btn{
            background: #fff;
            padding: 10px 12px;
            font-size: 18px;
            display: inline-block;
            color:#000;
            border: 1px solid #bebec7;
        }
        .login-btn:hover, .login-btn:focus {
            color: #fff;
            text-decoration: none;
            background:#5d30d1;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="returns-calc-form">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <h2 style="text-align: center">Return Calculator</h2>
                    <input type="hidden" id="hdSchAmfiCode" value="0" runat="server" />

                    <div class="card-body">
                        <div class="card">
                            <div class="card-body">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row">
                                        <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                            <label for="exampleInputEmail1">Investment Mode</label>
                                            <asp:DropDownList ID="ddlMode" runat="server"
                                                CssClass="form-control select2" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Selected="False">SIP</asp:ListItem>
                                                <asp:ListItem Selected="True">Lump Sum</asp:ListItem>
                                                <%--<asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>
                                                <asp:ListItem Selected="False">SWP</asp:ListItem>
                                                <asp:ListItem Selected="False">STP</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                            <div id="trCategory" runat="server" visible="true">
                                                <div class="">
                                                    <label for="exampleInputEmail1">Category</label>
                                                    <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true"
                                                        DataTextField="Nature" DataValueField="Nature"
                                                        CssClass="form-control select2" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                            <div id="Div1" runat="server" visible="true">
                                                <div class="">
                                                    <label for="exampleInputEmail1">Sub Category</label>
                                                    <asp:DropDownList ID="ddlSubNature" runat="server"
                                                        OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged"
                                                        AutoPostBack="true"
                                                        CssClass="form-control form-control-sm">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row">
                                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                                            <div id="tr1" runat="server" visible="true">
                                                <label for="exampleInputEmail1">Option</label>
                                                <asp:DropDownList ID="ddlOption" runat="server" AutoPostBack="true"
                                                    CssClass="form-control select2" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                                            <label>Mutual Fund Name</label>
                                            <asp:DropDownList ID="ddlFundHouse" runat="server"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged"
                                                CssClass="form-control form-control-sm">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row">
                                        <div class="form-group col-xs-6 col-sm-6 col-md-10 col-lg-10">
                                            <div runat="server" visible="true">
                                                <%-- <label id="lblSchemeName" runat="server">Scheme Name</label>--%>
                                                <asp:Label ID="lblSchemeName" Text="Scheme Name" runat="server" CssClass="text-label"></asp:Label>
                                                <asp:DropDownList ID="ddlscheme" runat="server" CssClass="form-control form-control-sm"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-6 col-sm-6 col-md-2 col-lg-2">
                                            <div visible="true" id="trInception1" runat="server">
                                                <label id="LabelInception" style="margin-bottom:0px">Inception Date</label>
                                                <asp:TextBox ID="SIPSchDt" runat="server" CssClass="form-control form-control-sm"
                                                    ReadOnly="true" Text=""></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row">
                                        <div class="form-group col-xs-12 col-sm-12 col-md-10 col-lg-10">
                                            <div id="trTransferTo" runat="server" visible="false">
                                                <label class="FieldHead">Transfer To</label>
                                                <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True"
                                                    CssClass="form-control form-control-sm" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group col-xs-12 col-sm-12 col-md-2 col-lg-2">
                                            <div id="trInception" runat="server">
                                                <label class="FieldHead" id="Span1">Inception Date</label>
                                                <asp:TextBox ID="SIPSchDt2" runat="server" CssClass="form-control form-control-sm"
                                                    ReadOnly="true" Text=""></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="from-group col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="row">
                                        <div id="trBenchmark" runat="server">
                                            <label id="Span3" class="text-label">Benchmark</label>
                                            <asp:TextBox ID="txtddlsipbnmark" runat="server" CssClass="form-control form-control-sm" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row">
                                        <div id="trInitialInvst" runat="server" visible="false">
                                            <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                <asp:Label class="text-label" runat="server" ID="lblInitial">Initial Amount (₹)</asp:Label>
                                                <asp:TextBox ID="txtiniAmount" runat="server" CssClass="form-control form-control-sm"
                                                    MaxLength="10" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                <asp:Label ID="lblSIPinit" runat="server" class="text-label">Initial Date </asp:Label>
                                                <asp:TextBox ID="txtIniToDate" runat="server" CssClass="form-control form-control-sm"
                                                    onMouseDown="Javascript: setDate();" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row" id="trSipInvst" runat="server" visible="true">
                                        <div>
                                            <div runat="server" id="SIP_withdrawl">
                                                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <asp:Label ID="lblInstallmentAmt" runat="server" Text="Installment Amount (₹)"
                                                        class="text-label"></asp:Label>
                                                    <asp:TextBox ID="txtinstall" CssClass="form-control form-control-sm" MaxLength="10" Text="" runat="server" ReadOnly="false" onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                                </div>
                                                <div class="" id="dvWithDrawSPdspn" runat="server">
                                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6" id="dvWithDrawSP" runat="server">
                                                        <asp:Label ID="lblTransferWithdrawal" runat="server"
                                                            Text="Withdrawal Amount" class="text-label"></asp:Label>
                                                        <asp:TextBox ID="txtTransferWithdrawal" runat="server" CssClass="form-control form-control-sm" MaxLength="14" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="SWP_STP_withdrawl" runat="server" visible="false">
                                                <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                                    <asp:Label ID="lblTransferWithdrawal2" runat="server"
                                                        Text="Withdrawal Amount" class="text-label"></asp:Label>
                                                    <asp:TextBox ID="txtTransferWithdrawal2" runat="server" CssClass="form-control form-control-sm" MaxLength="14" onChange="Javascript: checkInvestedValue();" ReadOnly="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div runat="server">
                                                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <label style="margin-bottom:0px;">Frequency</label>
                                                    <asp:DropDownList ID="ddPeriod_SIP" runat="server"
                                                        CssClass="form-control form-control-sm">
                                                        <%--<asp:ListItem Value="Monthly" Selected="True">Monthly</asp:ListItem>
                                     <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                     <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <asp:Label ID="lblDiffDate" runat="server" Text="SIP Day"
                                                        class="text-label"></asp:Label>
                                                    <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="form-control form-control-sm">
                                                        <asp:ListItem Value="1">1st</asp:ListItem>
                                                        <asp:ListItem Value="7">7th</asp:ListItem>
                                                        <asp:ListItem Value="14">14th</asp:ListItem>
                                                        <asp:ListItem Value="20">20th</asp:ListItem>
                                                        <asp:ListItem Value="25">25th</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server">
                                            <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                                                <label>From Date</label>
                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control form-control-sm datepicker-input"></asp:TextBox>
                                            </div>
                                            <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                                                <label for="">To Date</label>
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker-input form-control form-control-sm" onChange="Javascript: setDateValueAsOn(); "></asp:TextBox>
                                            </div>
                                        </div>
                                        <div runat="server">
                                            <div class="form-group col-xs-6 col-sm-6 col-md-6 col-lg-6">
                                                <label for="">Current Date</label>
                                                <asp:TextBox ID="txtvalason" runat="server" CssClass="datepicker-input form-control form-control-sm"></asp:TextBox>
                                                <small>('Current Date' should be greater than 'To Date')</small>
                                            </div>
                                            <%-- <fieldset class="form-group col-md-6 col-12">
                                    <label for="">  Current Date</label>
                                    <asp:TextBox ID="txtvalason" runat="server" CssClass="datepicker-input" AutoCompleteType="None"></asp:TextBox>
                                    <small id="passwordHelpBlock" class="form-text text-muted"> ('Current Date' should be greater than 'To Date')</small>
                                </fieldset>--%>
                                        </div>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                            <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6"
                                                id="tr_SIP_Sinc" runat="server">
                                                <div>
                                                    <input type="checkbox" id="chkInception4sip" runat="server"
                                                        onclick="setIncDate();" style="width: auto;" />
                                                    <label class="" for="checkbox-ince">Calculate from Inception</label>
                                                </div>
                                            </div>
                                            <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6" id="tr_Cal_Type" runat="server" visible="false">
                                                <asp:RadioButton Text="Reinvest" ID="rdbReinvest" Checked="true" runat="server" GroupName="rdbCalc" />
                                                <asp:RadioButton Text="Payout" ID="rdbPayout" runat="server" GroupName="rdbCalc" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row">
                                        <div id="trLumpInvst" runat="server" visible="false">
                                            <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                                <label>Investment Amount (₹)</label>
                                                <asp:TextBox ID="txtinstallLs" value="" MaxLength="10"
                                                    runat="server" CssClass="form-control form-control-sm"
                                                    onmousedown="Javascript: setDate();"
                                                    onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                            </div>
                                            <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <label class="FieldHead">
                                                        Start Date</label>
                                                    <asp:TextBox ID="txtLumpfromDate" runat="server" CssClass="form-control form-control-sm datepicker-input"
                                                        onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <label class="FieldHead">
                                                        End Date</label>
                                                    <asp:TextBox ID="txtLumpToDate" runat="server" CssClass="form-control form-control-sm datepicker-input"
                                                        onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <div id="tr_Cal_Incp" runat="server" visible="false">
                                                        <input type="checkbox" id="chkInception" runat="server" onclick="setIncDate();" style="width: auto;" />
                                                        <label class="" for="checkbox-ince">Calculate from Inception</label>
                                                    </div>

                                                    <div class="" id="trCalTypeLmpsm" runat="server" visible="false">
                                                        <asp:RadioButton Text=" Reinvest" ID="lmprdbReinvest" Checked="true" runat="server" GroupName="rdbCalc" />
                                                        <asp:RadioButton Text=" Payout" ID="lmprdbPayout" runat="server" GroupName="rdbCalc" />
                                                    </div>
                                                </div>
                                                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-right pt-3">
                                    <div class="row">

                                        <asp:Button ID="sipbtnshow" runat="server" Text="Calculate" CssClass="btn login-btn" OnClick="sipbtnshow_Click" />
                                        <%--<asp:Button ID="sipbtnshow" runat="server" OnClick="sipbtnshow_Click" CssClass="btn solid-Blue-button"></asp:Button>--%>
                                        <%--<input type="submit" id="sipbtnshow" runat="server" OnClick="sipbtnshow_Click" value="Calculate" class="btn solid-Blue-button" />--%>
                                        <asp:Button ID="sipbtnreset" runat="server" OnClick="sipbtnreset_Click" CssClass="btn reset-btn" Text="Reset"></asp:Button>
                                        <%--<input type="submit" id="sipbtnreset" runat="server" class="btn btn-danger solid-Danger-button" value="Reset" />--%>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="row">
                                        <div id="resultDiv" runat="server" visible="false">
                                            <table width="100%" border="0" align="center" cellpadding="0"
                                                cellspacing="0" style="display: none" class="table table-bordered">
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
                                                        <%--<span class="rslt_text">
                    On 05/07/2012, the value of your total investment Rs 120000
                    would be Rs <strong>131779.37</strong>
                </span>--%>
                                                        <asp:Label ID="lblInvstvalue" CssClass="rslt_text"
                                                            runat="server" Text="On Date C, the Scheme value of your total investment ₹ Y would be ₹ Z">
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
                                            <table width="100%" id="firstTable">
                                                <tr>
                                                    <td>
                                                        <div id="divSummary" runat="server" class="pt-3">
                                                            <asp:GridView ID="gvFirstTable" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" Visible="false" CssClass="table table-bordered">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Scheme") == DBNull.Value) ? "--" : Eval("Scheme").ToString()%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"
                                                                            Width="" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Units Purchased"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_unit") == DBNull.Value) ? "--" : Eval("Total_unit").ToString().Trim() == "0" ? "--" : Math.Round(Convert.ToDouble( Eval("Total_unit")),0).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount Invested<br/> (A)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_amount") == DBNull.Value) ? "--" : Eval("Total_amount").ToString().Trim() == "0" ? "--" : "₹  " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 2).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Investment Value as<br/>  on Date (B)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Present_Value") == DBNull.Value) ? "--" : Eval("Present_Value").ToString().Trim() == "0" ? "--" : "₹" + Math.Round(Convert.ToDouble(Eval("Present_Value")), 2).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dividend Amount (C)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Dividend_Amount") == DBNull.Value || Eval("Dividend_Amount") == "--" || Eval("Dividend_Amount").ToString() == "0") ? "--" : " ₹ " + Math.Round(Convert.ToDouble(Eval("Dividend_Amount")),2).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Profit <br/>(B+C-A)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : Eval("Profit_Sip").ToString().Trim() == "0" ? "--" : "₹" + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 2).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Absolute <br/>Returns"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : Eval("ABSOLUTERETURN").ToString().Trim() == "0" ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Returns*" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString().Trim() == "0" ? "--" : (Eval("Yield").ToString() + "%")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <br />
                                                            <span id="sipDisclaimer" class="FieldHead" style="padding: 4px; margin: 2px; border: none"
                                                                visible="false" runat="server"></span>
                                                            <asp:GridView ID="gvSWPSummaryTable" runat="server" AutoGenerateColumns="False" Width="100%" Visible="false" CssClass="table table-bordered">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="400px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount<BR/> Invested (A)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "₹" + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount<BR/> Withdrawn (B)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "₹" + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Present<BR/> Value (C)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Profit<BR/> (B+C-A)"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# "₹ "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abs. <br/>Returns" HeaderStyle-VerticalAlign="Top"
                                                                        HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Returns *" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <asp:GridView ID="gvSTPToSummaryTable" runat="server" AutoGenerateColumns="False" Width="100%" Visible="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"
                                                                            Width="40%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Amount Invested">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                            <%--Eval("Total_Amount_Invested").ToString()--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="24%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Present Value">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
                                                                            <%--TwoDecimal(Eval("Present_Value").ToString())--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="18%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Yield">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="18%" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <table width="100%" border="0" align="center" cellpadding="0"
                                                                cellspacing="0">
                                                                <tr align="left">
                                                                    <td valign="top" colspan="2" style="border: none; padding: 0">
                                                                        <asp:GridView ID="GridViewLumpSum" runat="server" Width="100%"
                                                                            CssClass="table table-bordered" AutoGenerateColumns="false">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left"
                                                                                        Width="330px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Amount Invested (A)"
                                                                                    HeaderStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("InvestedAmount") == DBNull.Value) ? "--" : "₹  " + Convert.ToDouble(Eval("InvestedAmount")).ToString("n2")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Investment Value<br/>(B)"
                                                                                    HeaderStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "₹ " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Dividend Amount<br/>(C)"
                                                                                    HeaderStyle-VerticalAlign="Top">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("DividendAmount") == "--") ? "--" : "₹ " + Eval("DividendAmount")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Profit from Investment<br/>(B+C-A)"
                                                                                    HeaderStyle-VerticalAlign="Top">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Profit") == DBNull.Value) ? "--" : "₹ " + Convert.ToDouble(Eval("Profit")).ToString("n2")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Return *<br/>" HeaderStyle-VerticalAlign="Top">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Return") == DBNull.Value) ? "--" : Eval("Return").ToString() + ((Eval("Return").ToString() == "N/A") ? "" : "%")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
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
                                            <table width="100%" id="TableRemark" class="table table-bordered" cellpadding="0" cellspacing="0" align="center">
                                                <tr>
                                                    <td>
                                                        <div id="divTab" runat="server" visible="false" style="padding: 0">
                                                            <table width="100%" border="0" align="left" cellpadding="0"
                                                                cellspacing="0" style="padding: 0">
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
                                                                                                                    ImageUrl="~/BlueChip/img/excel.jpg"
                                                                                                                    ToolTip="Download Excel" Text="Show Excel Calculation"
                                                                                                                    Visible="true" OnClick="ExcelCalculation_Click" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                    <div id="DetailDiv" runat="server" class="taB tab-pane show active">
                                                                                                        <%--<div id="country1" class="tabcontent">
                                                                                                        --%>
                                                                                                        <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False" class="table table-bordered">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="Date">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[2]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[1]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="NAV">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Scheme Units">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Scheme_units") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_units")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("SCHEME_UNITS_CUMULATIVE") == DBNull.Value) ? "--" :Math.Round(Convert.ToDouble( Eval("SCHEME_UNITS_CUMULATIVE")),2).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="SIP Amount">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Scheme_cashflow") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_cashflow")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Cumulative Fund Value">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("CUMULATIVE_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMULATIVE_AMOUNT")), 2).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                        <asp:GridView ID="swpGridView" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-bordered">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="Date">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%#  (Eval("DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("DATE").ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="NAV">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2") %>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Cashflow">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("FINAL_INVST_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT")).ToString("n2") %>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <%-- <asp:TemplateField HeaderText="Investment Amount">
                                                                                        <ItemTemplate>
                                                                                            <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("INVST_AMOUNT")).ToString("n2")  %>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle  />
                                                                                    </asp:TemplateField>--%>
                                                                                                                <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("CUMILATIVE_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("CUMILATIVE_UNITS")).ToString("n2") %>
                                                                                                                        <%-- Math.Round(Convert.ToDouble(Eval("")), 2).ToString()--%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Cumulative Amount">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("CUMILATIVE_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("CUMILATIVE_AMOUNT")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                        <div id="divSTP" runat="server" visible="False">
                                                                                                            <br />
                                                                                                            <b>From Scheme:
                                                                                    <%= ddlscheme.SelectedItem.Text %>
                                                                                                            </b>
                                                                                                            <br />
                                                                                                            <asp:GridView ID="stpFromGridview" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-bordered">
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
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("FROM_NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FROM_NAV")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("DIVIDEND_BONUS_FROM") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS_FROM").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS_FROM")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Cashflow">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("FINAL_INVST_AMOUNT_FROM") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT_FROM")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Redeemed Units">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("REDEEM_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("REDEEM_UNITS")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
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
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Investment Value">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("CUMILATIVE_AMOUNT_FROM") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_AMOUNT_FROM")), 2).ToString()%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                            <br />
                                                                                                            <b>To Scheme:
                                                                                    <%= ddlschtrto.SelectedItem.Text %>
                                                                                                            </b>
                                                                                                            <br />
                                                                                                            <asp:GridView ID="stpToGridview" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-bordered">
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
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("TO_NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("TO_NAV")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("DIVIDEND_BONUS_TO") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS_TO").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS_TO")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Cashflow">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("FINAL_INVST_AMOUNT_TO") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT_TO")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="No. of Units">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("NO_OF_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NO_OF_UNITS")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("CUMILATIVE_UNITS_TO") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_UNITS_TO")), 2).ToString()%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Investment Value">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("CUMILATIVE_AMOUNT_TO") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_AMOUNT_TO")), 2).ToString()%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="" HorizontalAlign="Center" />
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
                                                                                                        style="width: 100%;" class="text-center">
                                                                                                        <asp:Chart ID="chrtResult" runat="server" AlternateText="BloomKite"
                                                                                                            Visible="true"
                                                                                                            BorderlineWidth="2" Width="650px" Height="580px"
                                                                                                            IsSoftShadows="false">
                                                                                                            <%--BackGradientStyle="Center"  BorderlineColor="RoyalBlue" BackColor="Gray"--%>
                                                                                                            <Titles>
                                                                                                                <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="BloomKite"
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
                                                                                                        <asp:Chart ID="chrtResultSTPTO" runat="server" AlternateText="BloomKite"
                                                                                                            Visible="false"
                                                                                                            BorderlineWidth="2" Width="650px" Height="580px"
                                                                                                            IsSoftShadows="false">
                                                                                                            <Titles>
                                                                                                                <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="BloomKite"
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

                                                                                                    </div>
                                                                                                </asp:View>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:View ID="View3" runat="server">
                                                                                                    <asp:GridView ID="GridViewSIPResult" runat="server"
                                                                                                        Width="98%"
                                                                                                        HeaderStyle-BackColor=" #2ee9b6"
                                                                                                        HeaderStyle-CssClass="grdHead"
                                                                                                        AllowPaging="false" AutoGenerateColumns="false"
                                                                                                        OnRowDataBound="GridViewSIPResult_RowDataBound"
                                                                                                        Visible="false">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)"%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Left"
                                                                                                                    Width="360px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="1 Year">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString()+((Eval("1 Year").ToString()=="N/A")?"":"%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="3 Years">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="5 Years">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Since Inception">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                    <asp:GridView ID="GridViewSTPTOResult" runat="server"
                                                                                                        Width="98%"
                                                                                                        HeaderStyle-BackColor="#2ee9b6"
                                                                                                        HeaderStyle-CssClass="grdHead"
                                                                                                        AllowPaging="false" AutoGenerateColumns="false"
                                                                                                        Visible="false">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() + " (CAGR)"%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="1 Year">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="3 Years">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="5 Years">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Since Inception">
                                                                                                                <ItemTemplate>
                                                                                                                    <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle CssClass="" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                    <div id="divLsTable" runat="server">
                                                                                                        <br />
                                                                                                        <asp:GridView ID="GridViewResultLS" runat="server"
                                                                                                            Width="100%" BorderColor="Transparent"
                                                                                                            BorderStyle="None" AutoGenerateColumns="false"
                                                                                                            CssClass="table table-bordered">
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
                                                                                                                    <ItemStyle HorizontalAlign="left"
                                                                                                                        Width="360px" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="1 Year">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%#
                                                                                            (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="3 Years">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%#
                                                                                            (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="5 Years">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%#
                                                                                            (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Since Inception">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </div>

                                                                                                </asp:View>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:View ID="View4" runat="server">
                                                                                                    <div id="Showpdfdiv" runat="server" class="pb-3">
                                                                                                        <asp:RadioButtonList ID="RadioButtonListCustomerType" CssClass="dspNone" runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged" TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true">
                                                                                                            <asp:ListItem>Distributor</asp:ListItem>
                                                                                                            <asp:ListItem Selected="true">Not a Distributor</asp:ListItem>
                                                                                                        </asp:RadioButtonList>

                                                                                                        <div id="DistributerDiv" runat="server" visible="false">
                                                                                                            <div class="row pt-3" id="tblDistb">
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
                                                                                                            <div class="col-md-12 pt-4">
                                                                                                                <%--<button type="button" class="btn solid-Blue-button">Generate PDF</button>--%>
                                                                                                                <asp:LinkButton ID="LinkButtonGenerateReport" runat="server"
                                                                                                                    OnClick="LinkButtonGenerateReport_Click"
                                                                                                                    ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();"
                                                                                                                    CssClass="btn login-btn btn-sm ">Generate PDF</asp:LinkButton>
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
                                                                                                            Width="98%" CssClass="table table-bordered">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="NAV Date">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(Eval("Nav_Date")).ToString("dd-MMM-yyyy")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="NAV">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Dividend">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("DIVIDEND") == DBNull.Value) ? "--" : (Eval("DIVIDEND") == string.Empty) ? "--" : Convert.ToDouble(Eval("DIVIDEND")).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Bonus">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("BONUS") == DBNull.Value) ?  "--" : (Eval("BONUS") == string.Empty) ? "--" :  Eval("BONUS").ToString()%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Payout Amount">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("PAYOUT_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("PAYOUT_AMOUNT")), 2).ToString("n2")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="" HorizontalAlign="left" />
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
                            <div style="width: 100%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                                Developed by: <a href="https://www.icraanalytics.com"
                                    target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd
                                </a>, <a style="font-size: 10px; color: #999999"
                                    href="https://icraanalytics.com/home/Disclaimer"
                                    target="_blank">Disclaimer</a>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </form>
</body>
</html>
