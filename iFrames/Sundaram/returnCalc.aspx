<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always"
    EnableViewStateMac="true" CodeBehind="returnCalc.aspx.cs" Inherits="iFrames.Sundaram.returnCalc" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>
        Returns Calculator | Sundaram Asset Management Company Limited
    </title>

    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=0, shrink-to-fit=no" name="viewport" />
    <meta http-equiv="x-ua-compatible" content="IE=edge" />
    <link rel="shortcut icon" href="dist/img/favicon.png" type="image/x-icon" />

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Lato:ital,wght@0,100;0,300;0,400;0,700;0,900;1,100;1,300;1,400;1,700;1,900&display=swap" rel="stylesheet" />

    <!-- Common CSS Files -->
    <link rel="stylesheet" href="Assets/plugins/bootstrap-5.3.3/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Assets/plugins/fontawesome-6.7.1/css/all.min.css" />
    <link rel="stylesheet" href="dist/css/vendors.css" />
    <link rel="stylesheet" href="dist/css/main.css" />
    <link rel="stylesheet" href="Assets/plugins/datepicker/css/datepicker.min.css" />
    <link rel="stylesheet" href="Assets/plugins/select2-4.1.0/css/select2.min.css" />
    <link rel="stylesheet" href="Assets/plugins/select2-4.1.0/css/select2-bootstrap-5-theme.min.css" />

    <!-- Common CSS Files End -->
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="Script/check.js" type="text/javascript"></script>
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
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function validateEmail(input) {
            var email = input.value;
            var regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!regex.test(email)) {
                alert("Please enter a valid email address.");
                input.focus();
                return false;
            }
            return true;
        }
    </script>
   <%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sundaram Calculator </title>
    <script src="Script/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jshashtable-2.1_src.js"></script>
    <script type="text/javascript" src="Script/jquery.numberformatter-1.2.3.js"></script>
    <script type="text/javascript" src="Script/tmpl.js"></script>
    <script type="text/javascript" src="Script/jquery.dependClass-0.1.js"></script>
    <script src="Script/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Script/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Script/check.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>--%>
    <%--<script type="text/javascript">
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

    
    <style>
        .labelComment{
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
    </style>--%>
</head>
<body id="nav-page" class="preloader-visible" data-barba="wrapper">
    <!-- preloader start -->
<div class="preloader js-preloader">
    <div class="preloader__bg">
        <!-- <img src="dist/img/loading.gif" class="img-fluid" /> -->
    </div>
</div>
<!-- preloader end -->
    <form id="form1" runat="server">
        <div id="Ajaxcont" class="wrapper">
            <main class="main-content">
                <div class="content-wrapper  js-content-wrapper">
                     <!-- Header Section - Start-->
                    <section class="layout-pt-sm">
                        <div class="container-fluid px-100 sm:px-20">
                            <div class="page-header__content">
                                <div class="row">
                                    <div class="col-12">
                                        <div data-anim="slide-right delay-1">
                                            <h1 class="page-header__title">Returns Calculator</h1>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                    <!-- Header Section - End-->
                    <!-- Page Content Section - Start -->
                    <section class="layout-pt-sm layout-pb-md">
                        <div class="container-fluid px-100 sm:px-20">
                            <div class="col-md-12 p-4 shadow-5 rounded-8 y-gap-20">
                                <div class="row y-gap-20">
                                    <div class="col-md-4">
                                        <div class="form-floating">
                                            <asp:HiddenField ID="HdFaceValue" Value="0" runat="server" />
                                            <asp:HiddenField ID="HdFaceValueToSch" Value="0" runat="server" />
                                            <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="True"
                                                CssClass="form-select select2" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                                                <asp:ListItem Selected="True">SIP</asp:ListItem>
                                                <asp:ListItem Selected="False">Lump Sum</asp:ListItem>
                                                <asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>
                                                <asp:ListItem Selected="False">SWP</asp:ListItem>
                                                <asp:ListItem Selected="False">STP</asp:ListItem>
                                            </asp:DropDownList>
                                            <label for="ddlMode">Investment Mode</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="trCategory" runat="server" visible="true">
                                        <div class="form-floating" >
                                            <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true"
                                                DataTextField="Nature" DataValueField="Nature"
                                                CssClass="form-select select2" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <label for="ddlNature">Category</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-floating">
                                             <asp:DropDownList ID="ddlOption" runat="server" AutoPostBack="true"
                                                 CssClass="form-select select2" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                             </asp:DropDownList>
                                            <label for="ddlOption">Option</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-floating">
                                              <asp:DropDownList ID="ddlscheme" runat="server" CssClass="form-control select2"
                                                 AutoPostBack="True" OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                             </asp:DropDownList>
                                            <asp:label for="ddlscheme" id="lblSchemeName" runat="server" CssClass="labelname">Scheme Name</asp:label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-floating">
                                            <asp:TextBox ID="SIPSchDt" runat="server" CssClass="form-control datepicker readonly" ReadOnly="true" Text="" Enabled="false"></asp:TextBox>
                                            <label for="SIPSchDt" class="readonlyLabel">Inception Date</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="trTransferTo" runat="server" visible="false">
                                        <div class="form-floating" >
                                              <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True" CssClass="form-control select2" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged"></asp:DropDownList>
                                            <label for="ddlschtrto">Transfer To</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="trInception" runat="server">
                                        <div class="form-floating" >
                                            <asp:TextBox ID="SIPSchDt2" runat="server" CssClass="form-control datepicker readonly" ReadOnly="true" Text="" Enabled="false"></asp:TextBox>
                                            <label for="SIPSchDt2" class="readonlyLabel">Inception Date</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="trBenchmark" runat="server" visible="true">
                                        <div class="form-floating" >
                                            <asp:TextBox ID="txtddlsipbnmark" runat="server" CssClass="form-control readonly" ReadOnly="true" Enabled="false"></asp:TextBox>
                                            <label for="txtddlsipbnmark" class="readonlyLabel">Benchmark</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="trInitialInvst" runat="server" visible="false">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtiniAmount" runat="server" CssClass="form-control" MaxLength="10" ReadOnly="false"></asp:TextBox>
                                            <label for="txtiniAmount">Initial Amount (₹)</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="trInitialInvst1" runat="server" visible="false">
                                        <div class="form-floating">
                                        <asp:TextBox ID="txtIniToDate" runat="server" CssClass="form-control datepicker" onMouseDown="Javascript: setDate();" autocomplete="off"></asp:TextBox>
                                        <label for="txtIniToDate">Initial Date</label>
                                        </div>
                                    </div>
                                    <div class="col-md-12" id="trSipInvst" runat="server" visible="true">
                                        <div class="row y-gap-20">
                                            <div class="col-md-4" runat="server" id="SIP_withdrawl">
                                                <div class="form-floating" >
                                                    <asp:TextBox ID="txtinstall" CssClass="form-control" MaxLength="10" Text="" runat="server" ReadOnly="false"
                                                        onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                                     <asp:Label ID="lblInstallmentAmt" for="txtinstall" runat="server" Text="Installment Amount (₹)" CssClass="labelname"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SIP_withdrawl1">
                                                <div class="form-floating" >
                                                    <asp:TextBox ID="txtTransferWithdrawal" CssClass="form-control" MaxLength="10" Text="" runat="server" ReadOnly="false"
                                                        onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                                      <asp:Label ID="lblTransferWithdrawal" for="txtTransferWithdrawal" runat="server" class="labelname" Text="Withdrawal Amount"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-4" id="SWP_STP_withdrawl" runat="server" visible="false">
                                                <div class="form-floating">
                                                    <asp:TextBox ID="txtTransferWithdrawal2" CssClass="form-control" MaxLength="10" Text="" runat="server" ReadOnly="false"
                                                        onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                                      <asp:Label ID="lblTransferWithdrawal2" runat="server" class="labelname" Text="Withdrawal Amount"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                    <asp:DropDownList ID="ddPeriod_SIP" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                    <label for="ddPeriod_SIP">Frequency</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                    <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="form-control select2">
                                                        <asp:ListItem Value="1">1st</asp:ListItem>
                                                        <asp:ListItem Value="7">7th</asp:ListItem>
                                                        <asp:ListItem Value="14">14th</asp:ListItem>
                                                        <asp:ListItem Value="20">20th</asp:ListItem>
                                                        <asp:ListItem Value="25">25th</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblDiffDate" runat="server" Text="SIP Date" CssClass="labelname"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control datepicker" autocomplete="off"></asp:TextBox>
                                                    <label for="txtfromDate">From Date</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control datepicker" autocomplete="off" onChange="Javascript: setDateValueAsOn(); "></asp:TextBox>
                                                    <label for="txtToDate">To Date</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                     <asp:TextBox ID="txtvalason" runat="server" CssClass="form-control datepicker" autocomplete="off"></asp:TextBox>
                                                    <label for="txtvalason">Current Date</label>
                                                </div>
                                                <small>('Current Date' should be greater than 'To Date')</small>
                                            </div>
                                            <div class="col-md-12 mt-15">
                                                <div class="form-check form-check-inline" id="tr_SIP_Sinc" runat="server">
                                                    <input type="checkbox" class="form-check-input" id="chkInception4sip" runat="server" onclick="setIncDate();" />
                                                    <label class="form-check-label" for="chkInception4sip">Calculate from Inception</label>
                                                </div>
                                            </div>
                                            <div class="col-md-12 mt-15"  id="tr_Cal_Type" runat="server" visible="false">
                                                <div class="form-check form-check-inline">
                                                    <asp:RadioButton Text="Reinvest" ID="rdbReinvest" Checked="true" runat="server" GroupName="rdbCalc" />&nbsp;&nbsp;
                                                    <asp:RadioButton Text="Payout" ID="rdbPayout" runat="server" GroupName="rdbCalc" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    
                                    <div class="col-md-12" id="trLumpInvst" runat="server" visible="false">
                                        <div class="row y-gap-20">
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                    <asp:TextBox ID="txtinstallLs" value="" MaxLength="10" runat="server" CssClass="form-control" onmousedown="Javascript: setDate();" onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                                    <label for="ddlNature">Investment Amount (₹)</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                    <asp:TextBox ID="txtLumpfromDate" runat="server" CssClass="form-control datepicker" autocomplete="off" onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();"></asp:TextBox>
                                                    <label for="txtLumpfromDate">Start Date</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-floating">
                                                    <asp:TextBox ID="txtLumpToDate" runat="server" CssClass="form-control datepicker" autocomplete="off" onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();"></asp:TextBox>
                                                    <label for="txtLumpfromDate">End Date</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4" id="tr_Cal_Incp" runat="server" visible="false">
                                                <div class="form-floating">
                                                    <input type="checkbox" id="chkInception" runat="server" onclick="setIncDate();" style="width: auto;" />
                                                    <label for="txtLumpfromDate">Calculate from Inception</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4" id="trCalTypeLmpsm" runat="server" visible="false">
                                                <div class="form-check form-check-inline">
                                                    <asp:RadioButton Text=" Reinvest" ID="lmprdbReinvest" Checked="true" runat="server" GroupName="rdbCalc" />
                                                    <asp:RadioButton Text=" Payout" ID="lmprdbPayout" runat="server" GroupName="rdbCalc" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12 d-flex x-gap-10 y-gap-10">
                                       <asp:LinkButton ID="sipbtnshow" runat="server" CssClass="button -sm -sm-blue text-white mr-15" OnClick="sipbtnshow_Click"><i class="fa-solid fa-calculator mr-10"></i>Calculate</asp:LinkButton>
 
                                    <asp:LinkButton ID="sipbtnreset" runat="server" OnClick="sipbtnreset_Click" CssClass="button -sm -sm-orange text-white"><i class="fa-solid fa-close mr-10"></i>Reset</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 mt-25" id="resultDiv" runat="server" visible="false">
                                <div class="row y-gap-20">
                                    <div class="col-12" >
                                        <div class="table-responsive">
                                            <table width="100%" id="firstTable">
                                                <tr>
                                                    <td style="padding: 0">
                                                        <div id="divSummary" runat="server">
                                                            <asp:GridView ID="gvFirstTable" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" CssClass="table table-bordered text-center table-striped"
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
                                                                    <asp:TemplateField HeaderText="Amount  Invested (A)"
                                                                        HeaderStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_amount") == DBNull.Value) ? "--" : Eval("Total_amount").ToString().Trim() == "0" ? "--" : "₹  " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 2).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Investment Value as on Date (B)"
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
                                                                    <asp:TemplateField HeaderText="Total Profit (B+C-A)"
                                                                        HeaderStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : Eval("Profit_Sip").ToString().Trim() == "0" ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 2).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="borderlefft" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Absolute Returns"
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
                                                                AutoGenerateColumns="False" Width="100%" CssClass="table table-bordered text-center table-striped"
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
                                                                    <asp:TemplateField HeaderText="Amount Invested (A)"
                                                                        HeaderStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount Withdrawn (B)"
                                                                        HeaderStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "₹  " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Present Value (C)"
                                                                        HeaderStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "₹ " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Profit (B+C-A)"
                                                                        HeaderStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <%# "₹ "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Abs. Returns" HeaderStyle-VerticalAlign="Top">
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
                                                                AutoGenerateColumns="False" CssClass="table table-bordered text-center table-striped"
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
                                                                           
                                                                            RowStyle-CssClass="grdRow" AlternatingRowStyle-CssClass="grdRow"
                                                                            HeaderStyle-CssClass="grdHead"
                                                                            AutoGenerateColumns="false" CssClass="grdRow table table-bordered text-center table-striped">
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
                                                                                <asp:TemplateField HeaderText="Investment Value (B)"
                                                                                    HeaderStyle-VerticalAlign="Top">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "₹ " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Dividend Amount (C)"
                                                                                    HeaderStyle-VerticalAlign="Top">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("DividendAmount") == "--") ? "--" : "₹ " + Eval("DividendAmount")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Profit from Investment (B+C-A)"
                                                                                    HeaderStyle-VerticalAlign="Top">
                                                                                    <ItemTemplate>
                                                                                        <%# (Eval("Profit") == DBNull.Value) ? "--" : "₹ " + Convert.ToDouble(Eval("Profit")).ToString("n2")%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="borderbottom" HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Return *" HeaderStyle-VerticalAlign="Top">
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
                                        </div>
                                    </div>
                                     <div class="col-12">
                                         <div class="tabs -active-blue-1 mt-30 js-tabs" id="divTab" runat="server" visible="false">
                                             <div class="tabs__controls d-flex items-center js-tabs-controls">
                                                <div>
                                                    <%--<button class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button is-active" data-tab-target=".-tab-item-1" type="button">View Detail Report</button>--%>
                                                    <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click" class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button is-active" type="button">View Detail Report</asp:LinkButton>
                                                </div>
                                                <div>
                                                    <%--<button class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button " data-tab-target=".-tab-item-2" type="button">View Graph</button>--%>
                                                    <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click" class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button" type="button">View Graph</asp:LinkButton>
                                                </div>
                                                <div>
                                                    <%--<button class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button " data-tab-target=".-tab-item-3" type="button">View Historical Performance</button>--%>
                                                    <asp:LinkButton ID="lnkTab3" runat="server" Visible="false" OnClick="lnkTab3_Click" class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button " type="button">View Historical Performance</asp:LinkButton>
                                                </div>
                                                 <div>
                                                    <%--<button class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button " data-tab-target=".-tab-item-3" type="button">View PDF Report</button>--%>
                                                     <asp:LinkButton ID="lnkTab4" runat="server" OnClick="lnkTab4_Click" class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button " type="button">View PDF Report</asp:LinkButton>
                                                </div>
                                                  <div>
                                                    <%--<button class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button " data-tab-target=".-tab-item-3" type="button">View Dividend History</button>--%>
                                                    <asp:LinkButton ID="lnkTab5" runat="server" Visible="false" OnClick="lnkTab5_Click" class="text-light-1 lh-12 tabs__button mr-30 js-tabs-button" type="button">View Dividend History</asp:LinkButton>
                                                </div>
                                            </div>
                                         </div>
                                     </div>
                                </div>
                            </div>
                            <div class="col-md-12 mt-25">
                                <div class="row">
                                    <div  class="table-responsive returnCalc">
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
                                        
                                        <table width="100%" id="TableRemark" class="table" cellpadding="0" cellspacing="0" align="center" style="padding: 0; border-bottom:#fff solid">
                                            <tr>
                                                <td>
                                                    <div  style="padding: 0">
                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="padding: 0">
                                                            <tr align="left">
                                                                <td align="left" valign="top" style="padding: 0">
                                                                    <%--<ul id="countrytabs" class="nav nav-tabs">
                                                                        <li class="nav-item">
                                                                            
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            
                                                                        </li>
                                                                    </ul>--%>
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
                                                                                                                ImageUrl="~/Sundaram/IMG/excel.jpg"
                                                                                                                ToolTip="Download Excel" Text="Show Excel Calculation"
                                                                                                                Visible="true" OnClick="ExcelCalculation_Click" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <div id="DetailDiv" runat="server" class="taB tab-pane fade show active">
                                                                                                    <%--<div id="country1" class="tabcontent">
                                                                                                    --%>
                                                                                                    <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False"
                                                                                                        Width="98%" CssClass="table table-bordered text-center table-striped"
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
                                                                                                        RowStyle-CssClass="grdRow" CssClass="table table-bordered text-center table-striped">
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
                                                                                                            RowStyle-CssClass="grdRow" CssClass="table table-bordered text-center table-striped">
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
                                                                                                            RowStyle-CssClass="grdRow" CssClass="table table-bordered text-center table-striped">
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
                                                                                                   
                                                                                                </div>
                                                                                            </asp:View>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:View ID="View3" runat="server">
                                                                                                <asp:GridView ID="GridViewSIPResult" runat="server"
                                                                                                    Width="98%" RowStyle-CssClass="grdRow" CssClass="table table-bordered text-center table-striped"
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
                                                                                                    Width="98%" RowStyle-CssClass="grdRow" CssClass="table table-bordered text-center table-striped"
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
                                                                                                    <asp:GridView ID="GridViewResultLS" runat="server" CssClass="table table-bordered text-center table-striped" Width="100%" AutoGenerateColumns="false" HeaderStyle-CssClass="grdHead">
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
                                                                                      
                                                                                            </asp:View>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:View ID="View4" runat="server">
                                                                                                <div id="Showpdfdiv" runat="server" class="FieldHead">                                                                    
                                                                                                    <div class="col-lg-12">
                                                                                                            
                                                                                                                <asp:RadioButtonList ID="RadioButtonListCustomerType" runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged" TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="form-check-inline">
                                                                                                                    <asp:ListItem>Distributor </asp:ListItem>
                                                                                                                    <asp:ListItem Selected="true">Not a Distributor</asp:ListItem>
                                                                                                                </asp:RadioButtonList>
                                                                                                            </div>
                                                                                                    <div id="DistributerDiv" runat="server" visible="false">
                                                                                                        <div class="row" id="tblDistb">
                                                                                                            <div class="col-lg-12 y-gap-20">
                                                                                                                <div class="row y-gap-10 x-gap-20">
                                                                                                                    <div class="col-md-2 col">
                                                                                                                        <div class="input-group">
                                                                                                                            <span class="arn input-group-text">ARN-</span>
                                                                                                                            <div class="form-floating">
                                                                                                                                <asp:TextBox ID="txtArn" CssClass="form-control" runat="server" MaxLength="30"></asp:TextBox><label for="ARNcode">ARN Code</label>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </div>

                                                                                                                    <div class="col-md-2 col">
                                                                                                                        <div class="form-floating">
                                                                                                                            
                                                                                                                            <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server" MaxLength="14" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                                            <label for="mobile">Mobile<span class="require">*</span></label>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="col-md-2 col">
                                                                                                                        <div class="form-floating">
                                                                                                                            
                                                                                                                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" MaxLength="30"></asp:TextBox>
                                                                                                                            <label for="Email">Email<span class="require">*</span></label>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="col-md-2 col">
                                                                                                                        <div class="form-floating">
                                                                                                                            
                                                                                                                            <asp:TextBox ID="txtPreparedby" CssClass="form-control" runat="server" MaxLength="40"></asp:TextBox>
                                                                                                                            <label for="Prepared by">Prepared by<span class="require">*</span></label>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="col-md-2 col">
                                                                                                                       <div class="form-floating">
                                                                                                                            
                                                                                                                            <asp:TextBox ID="txtPreparedFor" CssClass="form-control" runat="server" MaxLength="40"></asp:TextBox>
                                                                                                                           <label for="Prepared for">Prepared for<span class="require">*</span></label>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col-lg-12 y-gap-20 mt-20">
                                                                                                    <div class="row y-gap-20">
                                                                                                        <div class="col-md-12 text-left mt-15">
                                                                                                            <%--<button type="button" class="btn solid-Blue-button">Generate PDF</button>--%>
                                                                                                            <asp:LinkButton ID="LinkButtonGenerateReport" runat="server" OnClick="LinkButtonGenerateReport_Click" ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();" CssClass="button -sm -sm-blue text-white display"><i class="fa-regular fa-file-pdf text-22 mr-10"></i> Generate PDF</asp:LinkButton>
                                                                                                        </div>
                                                                                                       </div> 
                                                                                                    </div>
                                                                                                </div>
                                                                                            </asp:View>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:View ID="View5" runat="server">
                                                                                                <div id="showDivHistory" runat="server">
                                                                                                    <br>
                                                                                                    <asp:GridView ID="gridDivHistory" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered text-center table-striped"
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
                                                   
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>



                        </div>
                    </section>
                    <section class="layout-pt-sm layout-pb-sm bg-sm-grey-rgba" id="disclaimerDiv" runat="server">
                        <div class="container-fluid px-100 sm:px-20">
                            <div class="row">
                                <div class=" col-12  return-calc-disclaimer">
                                    <p class="text-16 text-dark-2 text-justify">
                                        <b>Disclaimer:</b><br />
                                        <asp:Label ID="LSDisc" runat="server" Text=""
                                            Visible="true">
                                            * Returns here denote the Extended Internal Rate of Return (XIRR).<br />
                                        </asp:Label>
                                        <asp:Label ID="LSDisc1" runat="server" Text="<b>* For Time Periods > 1 yr, CAGR Returns have been shown. For Time Periods < 1 yr, Absolute Returns have been shown. </br></b>"
                                         Visible="false"></asp:Label> 
                                        Benchmark Returns are based on Total Return Index.
                                    </p>
                                    <p class="text-16 text-dark-2 text-justify"><b>Since Inception return of the benchmark is calculated from the scheme inception date.</b></p>
                                    <p class="text-16 text-dark-2 text-justify">
                                        Past performance may or may not be sustained in the future and should not be used as a basis for comparison with other investments.
                                    </p>
                                    <p class="text-16 text-dark-2 text-justify">
                                        <asp:Label ID="lblDisclaimer" runat="server"></asp:Label>
                                    </p>
                                    <p class="text-16 text-dark-2 text-justify">
                                        The return calculator has been developed and is maintained by <a class="text-sm-orange underline" href="https://www.icraanalytics.com" target="_blank">ICRA Analytics Limited.</a> Sundaram Asset Management Company Limited/ Trustees do not endorse the authenticity or accuracy of the figures based on which the returns are calculated; nor shall they be held responsible or liable for any error or inaccuracy or for any losses suffered by any investor
                                        as a direct or indirect consequence of relying upon the data displayed by the calculator.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </main>
        </div>

    </form>
    <!-- Common JS Files -->
    <script type="text/javascript" src="Assets/plugins/bootstrap-5.3.3/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="dist/js/vendors.js"></script>
    <script type="text/javascript" src="dist/js/main.js"></script>
    <script src="dist/js/custom.js"></script>
    <script type="text/javascript" src="Assets/plugins/datepicker/js/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="Assets/plugins/select2-4.1.0/js/select2.min.js"></script>
    <!-- Common JS Files End -->

   <%-- <script>
        // Datepicker
        $('.datepicker').datepicker({
            autoclose: true,
            format: 'dd-m-yyyy'
        });
    </script>--%>
    <%--<script src="Script/new/jquery_new.min.js" ></script>
    <script src="Script/new/bootstrap.min.js"></script>
    <script src="Script/new/popper.min.js"></script>
    <script src="Script/new/bootstrap-datepicker.js"></script>
    <script src="Script/new/underscore.js"></script>--%>

</body>
</html>
