<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="DSPCalc.aspx.cs" Inherits="iFrames.DSP.DSPCalc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title>DSP Calculator </title>
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <script type="text/javascript" src="Script/jquery.js"></script>
    <script type="text/javascript" src="Script/jshashtable-2.1_src.js"></script>
    <script type="text/javascript" src="Script/jquery.numberformatter-1.2.3.js"></script>
    <script type="text/javascript" src="Script/tmpl.js"></script>
    <script type="text/javascript" src="Script/jquery.dependClass-0.1.js"></script>
    <script src="Script/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Script/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Script/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="Script/check.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>
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

      

    </script>
    <link rel="stylesheet" type="text/css" href="CSS/jquery-ui.css" />
    <%--<link rel="stylesheet" type="text/css" href="CSS/demos.css" />--%>
    <%-- <style type="text/css">
        body {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
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

        .borderlefft {
            border-bottom: #c6c8ca solid 1px;
        }

        .borderbottom {
            border-bottom: #c6c8ca solid 1px;
        }


        -- >
    </style>
    <link href="CSS/master.css" rel="stylesheet" type="text/css" media="all" />
    <link href="CSS/dspstyles.css" rel="stylesheet" type="text/css" media="all" />
    <style type="text/css">
        <!--
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

        -->
    </style>
    <link href="CSS/tabcontent.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="Script/tabcontent.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="font-awesome-4.6.3/css/font-awesome.min.css" />
    <!-- END CSS FRAMEWORK -->
    <!-- BEGIN CSS PLUGIN -->
    <link rel="stylesheet" href="css/pace-theme-minimal.css" />
    <link href="css/awesome-bootstrap-checkbox.css" rel="stylesheet" />
    <!-- END CSS PLUGIN -->
    <!-- BEGIN CSS TEMPLATE -->
    <link rel="stylesheet" href="css/main.css" />
    <link href="CSS/tabcontent.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        $(function () {
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

            //            $("#txtfromDate").datepicker({
            //                showOn: "button",
            //                buttonImageOnly: true,
            //                buttonImage: "img/calenderb.jpg",
            //                buttonText: "Select Date",
            //                dateFormat: 'dd/mm/yy',
            //                changeMonth: true,
            //                changeYear: true,
            //                maxDate: -1
            //            });

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
</head>
<body>
    <form id="form1" runat="server">
    <div id="Div1" class="row" runat="server">
        <div class="col-md-12">
            <div class="col-md-3">
            </div>
            <div class="col-md-6" style="border-bottom: 2px solid #0f8ccf; border-top: 6px solid #0f8ccf;
                -moz-box-shadow: 0px 0px 8px #666; -webkit-box-shadow: 0px 0px 8px #666; box-shadow: 0px 0px 8px #666;">
                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-3">
                        Investment Mode</div>
                    <div class="col-lg-3 ">
                        <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="True" CssClass="ddl"
                            OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                            <asp:ListItem Selected="True">SIP</asp:ListItem>
                            <asp:ListItem Selected="False">Lump Sum</asp:ListItem>
                            <asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>
                            <asp:ListItem Selected="False">SWP</asp:ListItem>
                            <asp:ListItem Selected="False">STP</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="True" CssClass="ddl">
                                    <asp:ListItem Selected="True">SIP</asp:ListItem>
                                    <asp:ListItem Selected="False">Lump Sum</asp:ListItem>
                                    <asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>
                                    <asp:ListItem Selected="False">SWP</asp:ListItem>
                                    <asp:ListItem Selected="False">STP</asp:ListItem>
                                </asp:DropDownList>--%>
                    </div>
                    <div class="col-lg-5">
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;" id="trCategory" runat="server" visible="true">
                    <div class="col-lg-3">
                        Category</div>
                    <div class="col-lg-2">
                        <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" DataTextField="Nature"
                            DataValueField="Nature" CssClass="ddl" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%--<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" DataTextField="Nature"
                                     DataValueField="Nature" CssClass="ddl" >
                                 </asp:DropDownList>--%>
                    </div>
                    <div class="col-lg-2">
                    </div>
                    <div class="col-lg-5">
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-3">
                        <asp:Label ID="lblSchemeName" runat="server" Text="Scheme Name"></asp:Label></div>
                    <div class="col-lg-2">
                        <asp:DropDownList ID="ddlscheme" runat="server" CssClass="ddl-1" Width="" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%-- <asp:DropDownList ID="DropDownList2" runat="server" CssClass="ddl" Width="" AutoPostBack="True"></asp:DropDownList>--%>
                    </div>
                    <div class="col-lg-2">
                    </div>
                    <div class="col-lg-5">
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;" id="trInception1" runat="server">
                    <div class="col-lg-3">
                        <asp:Label ID="LabelInception" runat="server" CssClass="FieldHead" Text="Inception Date"></asp:Label>
                    </div>
                    <div class="col-lg-2">
                        <asp:TextBox ID="SIPSchDt" runat="server" CssClass="ddl_3" ReadOnly="true" Text=""></asp:TextBox>
                    </div>
                    <div class="col-lg-2">
                    </div>
                    <div class="col-lg-5">
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;" id="trTransferTo" runat="server" visible="false">
                    <div class="col-lg-3">
                        Transfer To</div>
                    <div class="col-lg-2">
                        <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True" CssClass="ddl"
                            Width="360px" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%--  <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True" CssClass="ddl">
                                 </asp:DropDownList>--%>
                    </div>
                    <div class="col-lg-2">
                    </div>
                    <div class="col-lg-5">
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;" id="trInception" runat="server">
                    <div class="col-lg-3">
                        <asp:Label ID="Label1" runat="server" CssClass="FieldHead" Text="Inception Date"></asp:Label>
                    </div>
                    <div class="col-lg-2">
                        <asp:TextBox ID="SIPSchDt2" runat="server" CssClass="ddl_3" ReadOnly="true" Text=""></asp:TextBox>
                    </div>
                    <div class="col-lg-2">
                    </div>
                    <div class="col-lg-5">
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;" id="trBenchmark" runat="server" visible="true">
                    <div class="col-lg-3">
                        Benchmark</div>
                    <div class="col-lg-2">
                        <asp:TextBox ID="txtddlsipbnmark" runat="server" CssClass="ddl_3" Text="" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-lg-2">
                    </div>
                    <div class="col-lg-5">
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;" id="trInitialInvst" runat="server" visible="false">
                    <div class="col-lg-3">
                        Initial Amount (Rs.)</div>
                    <div class="col-lg-2">
                        <asp:TextBox ID="txtiniAmount" runat="server" CssClass="ddl_3" MaxLength="10" ReadOnly="false"
                            Text=""></asp:TextBox>
                    </div>
                    <div class="col-lg-1"></div>
                    <div class="col-lg-2">
                        Initial Date</div>
                    <div class="col-lg-2" style="width:200px">
                        <asp:TextBox ID="txtIniToDate" runat="server"  CssClass="txtbx-brdr" 
                            onMouseDown="Javascript: setDate();">
                        </asp:TextBox>
                    </div>
                    
                </div>
                <div id="trSipInvst" runat="server" visible="true">
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-lg-3">
                            <asp:Label ID="lblInstallmentAmt" runat="server" Text="Installment Amount (Rs.)"
                                class="FieldHead"></asp:Label>
                        </div>
                        <div class="col-lg-2">
                            <asp:TextBox ID="txtinstall" CssClass="ddl_3" MaxLength="10" Text="" runat="server"
                                ReadOnly="false" onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                        </div>
                        <div class="col-lg-1">
                        </div>
                        <div class="col-lg-3">
                            <%--style="padding-left:5px; padding-right:0;"--%>
                            <asp:Label ID="lblTransferWithdrawal" runat="server" class="FieldHead" Text="Withdrawal Amount"></asp:Label>
                        </div>
                        <div class="col-lg-2">
                            <asp:TextBox ID="txtTransferWithdrawal" runat="server" CssClass="ddl_3" MaxLength="14"
                                onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false"></asp:TextBox>
                        </div>
                        <div class="col-lg-1">
                        </div>
                    </div>
                    <div class="row" style="margin-top: 20px; line-height:30px">
                        <div class="col-lg-3">
                            Frequency</div>
                        <div class="col-lg-2">
                            <asp:DropDownList ID="ddPeriod_SIP" runat="server" CssClass="ddl_3">
                                <asp:ListItem Value="Monthly" Selected="True">Monthly</asp:ListItem>
                                <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-1">
                        </div>
                        <div class="col-lg-3">
                            <asp:Label ID="lblDiffDate" runat="server" Text="SIP Date"></asp:Label>
                        </div>
                        <div class="col-lg-2">
                            <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="ddl_3">
                                <asp:ListItem Value="1">1st</asp:ListItem>
                                <asp:ListItem Value="7">7th</asp:ListItem>
                                <asp:ListItem Value="14">14th</asp:ListItem>
                                <asp:ListItem Value="21">21st</asp:ListItem>
                                <asp:ListItem Value="28">28th</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-1">
                        </div>
                    </div>
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-lg-3">
                            From Date</div>
                        <div class="col-lg-2" style="width:200px">
                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="txtbx-brdr"></asp:TextBox>
                        </div>
                        
                        <div class="col-lg-2" style="width:100px">
                            To Date
                        </div>
                        <div class="col-lg-2" style="width:200px">
                            <asp:TextBox ID="txtToDate" runat="server"  onChange="Javascript: setDateValueAsOn(); " CssClass="txtbx-brdr"></asp:TextBox>
                        </div>
                        
                    </div>
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-lg-3">
                            Value as on Date</div>
                        <div class="col-lg-2" style="width:200px">
                            <asp:TextBox ID="txtvalason" runat="server"  CssClass="txtbx-brdr"></asp:TextBox>
                        </div>
                        
                        <div class="col-lg-5" style="font-size:11px; padding-left:0;">
                            ('Value as on Date' should be greater than 'To Date')</div>
                    </div>
                </div>
                <div id="trLumpInvst" runat="server" visible="false">
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-lg-3">
                            Investment Amount (Rs.)</div>
                        <div class="col-lg-2">
                            <asp:TextBox ID="txtinstallLs" value="" MaxLength="10" runat="server" CssClass="ddl_3"
                                onmousedown="Javascript: setDate();" onChange="Javascript: checkInvestedValue();">
                            </asp:TextBox>
                        </div>
                        <div class="col-lg-7">
                        </div>
                    </div>
                    <div class="row" runat="server" style="margin-top: 20px;">
                        <div class="col-lg-3" >
                            Start Date</div>
                        <div class="col-lg-2" style="width:200px">
                            <asp:TextBox ID="txtLumpfromDate" runat="server" onMouseDown="Javascript: setDate();" CssClass="txtbx-brdr"></asp:TextBox>
                        </div>
                        
                        <div class="col-lg-2"  style="width:100px">
                            End Date
                        </div>
                        <div class="col-lg-2" style="width:200px">
                            <asp:TextBox ID="txtLumpToDate" runat="server" CssClass="txtbx-brdr"></asp:TextBox>
                        </div>
                        
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-3">
                    </div>
                    <div class="col-lg-9">
                        <asp:ImageButton ID="sipbtnshow" runat="server" ImageUrl="~/DSP/IMG/calculate.jpg"
                            OnClick="sipbtnshow_Click"></asp:ImageButton>
                        <span style="padding-left: 10px;">
                            <asp:ImageButton ID="sipbtnreset" runat="server" ImageUrl="~/DSP/IMG/reset.jpg" OnClick="sipbtnreset_Click">
                            </asp:ImageButton></span>
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-12" style="background-color: #4395cd; height: 5px; width: 100%">
                    </div>
                </div>
                <div class="row">
                    <img style="margin-bottom: 10px; margin-left: 85px" src="IMG/down-pointer-image.png"
                        height="11" width="14" alt="" />
                </div>
                <div class="row" style="padding: 0; margin: 0">
                    <div class="table-responsive" style="padding: 0; margin: 0">
                        <div id="resultDiv" runat="server" visible="false">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="display: none">
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
                            <table width="100%" id="firstTable">
                                <tr>
                                    <td>
                                        <div id="divSummary" runat="server">
                                            <asp:GridView ID="gvFirstTable" runat="server" AutoGenerateColumns="False" Width="100%"
                                                HeaderStyle-CssClass="grdHead" Visible="false" AlternatingRowStyle-CssClass="grdRow"
                                                RowStyle-CssClass="grdRow">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <%# (Eval("Scheme") == DBNull.Value) ? "--" : Eval("Scheme").ToString()%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="220px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Units<br/> Purchased">
                                                        <ItemTemplate>
                                                            <%# (Eval("Total_unit") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble( Eval("Total_unit")),0).ToString("n0")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderlefft" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount<br/> Invested (A)">
                                                        <ItemTemplate>
                                                            <%# (Eval("Total_amount") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 0).ToString("n0")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderlefft" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Investment Value<br/> as on Date (B)">
                                                        <ItemTemplate>
                                                            <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderlefft" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Profit<br/> (B-A)">
                                                        <ItemTemplate>
                                                            <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 0).ToString("n0")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderlefft" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Abs. <br/>Returns">
                                                        <ItemTemplate>
                                                            <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderlefft" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Returns*" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Top">
                                                        <ItemTemplate>
                                                            <%# (Eval("Yield") == DBNull.Value) ? "--" :( Eval("Yield").ToString() + "%")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderlefft" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:GridView ID="gvSWPSummaryTable" runat="server" AutoGenerateColumns="False" Width="100%"
                                                HeaderStyle-CssClass="grdHead" Visible="false" AlternatingRowStyle-CssClass="grdRow"
                                                RowStyle-CssClass="grdRow">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="210px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount<BR/> Invested (A)">
                                                        <ItemTemplate>
                                                            <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderbottom" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount<BR/> Withdrawn (B)">
                                                        <ItemTemplate>
                                                            <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderbottom" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Present<BR/> Value (C)">
                                                        <ItemTemplate>
                                                            <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderbottom" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Profit<BR/> (B+C-A)">
                                                        <ItemTemplate>
                                                            <%# "<img src='img/rimg.png' style='vertical-align:middle;'> "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="borderbottom" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Abs. <br/>Returns">
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
                                            <asp:GridView ID="gvSTPToSummaryTable" runat="server" AutoGenerateColumns="False"
                                                Width="100%" HeaderStyle-CssClass="grdHead" Visible="false" AlternatingRowStyle-CssClass="grdRow"
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
                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr align="left">
                                                    <td valign="top" colspan="2">
                                                        <asp:GridView ID="GridViewLumpSum" runat="server" Width="100%" CssClass="grdRow"
                                                            HeaderStyle-BackColor=" #4395cd" RowStyle-CssClass="grdRow" AlternatingRowStyle-CssClass="grdRow"
                                                            HeaderStyle-CssClass="grdHead" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="230px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount Invested <br/>(A)">
                                                                    <ItemTemplate>
                                                                        <%# (Eval("InvestedAmount") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedAmount")).ToString("n2")%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="borderbottom" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Investment Value<br/>(B)">
                                                                    <ItemTemplate>
                                                                        <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="borderbottom" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Profit from Investment<br/>(B-A)">
                                                                    <ItemTemplate>
                                                                        <%# (Eval("Profit") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("Profit")).ToString("n2")%>
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
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" id="TableRemark">
                                <tr>
                                    <td>
                                        <div id="divTab" runat="server" visible="false">
                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr align="left">
                                                                <td align="left" valign="top">
                                                                    <ul id="countrytabs" class="shadetabs">
                                                                        <li>
                                                                            <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click">View Detail Report</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click">View Graph</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="lnkTab3" runat="server" OnClick="lnkTab3_Click">View Historical Performance</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="lnkTab4" runat="server" OnClick="lnkTab4_Click">View PDF Report</asp:LinkButton></li>
                                                                    </ul>
                                                                    <div>
                                                                        <asp:MultiView ID="MultiView1" runat="server">
                                                                            <table width="100%" cellpadding="2" cellspacing="5">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:View ID="View1" runat="server">
                                                                                            <div id="DetailDiv" runat="server">
                                                                                                <%--<div id="country1" class="tabcontent">--%>
                                                                                                <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                                    HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow" RowStyle-CssClass="grdRow">
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
                                                                                                            <ItemStyle CssClass="borderlefft" />
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
                                                                                                <asp:GridView ID="swpGridView" runat="server" AutoGenerateColumns="False" Width="98%"
                                                                                                    HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow" RowStyle-CssClass="grdRow">
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
                                                                                                    <asp:GridView ID="stpFromGridview" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                                        HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow" RowStyle-CssClass="grdRow">
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
                                                                                                    <br />
                                                                                                    <asp:GridView ID="stpToGridview" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                                        HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow" RowStyle-CssClass="grdRow">
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
                                                                                                <br />
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td align="right" style="padding-right: 20px">
                                                                                                            <asp:ImageButton ID="btnExcelCalculation" runat="server" ImageUrl="~/DSP/IMG/excell.png"
                                                                                                                ToolTip="Download Excel" Text="Show Excel Calculation" Visible="true" OnClick="ExcelCalculation_Click" UseSubmitBehavior="false"/>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </asp:View>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:View ID="View2" runat="server">
                                                                                            <%--  <div id="country2" class="tabcontent">--%>
                                                                                            <div id="divshowChart" runat="server" visible="true" style="width: 100%;">
                                                                                                <asp:Chart ID="chrtResult" runat="server" AlternateText="DSP" Visible="true" BorderlineWidth="2"
                                                                                                    Width="650px" Height="580px" IsSoftShadows="false">
                                                                                                    <%--BackGradientStyle="Center"  BorderlineColor="RoyalBlue" BackColor="Gray"--%>
                                                                                                    <Titles>
                                                                                                        <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="DSP" ForeColor="26, 59, 105">
                                                                                                        </asp:Title>
                                                                                                    </Titles>
                                                                                                    <Legends>
                                                                                                        <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                                            Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold">
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
                                                                                                            <AxisY ArrowStyle="None" ToolTip="Value(Rs.)" TextOrientation="Horizontal" LineColor="#013974">
                                                                                                                <ScaleBreakStyle Enabled="True" />
                                                                                                                <MajorGrid Enabled="false" />
                                                                                                            </AxisY>
                                                                                                        </asp:ChartArea>
                                                                                                    </ChartAreas>
                                                                                                </asp:Chart>
                                                                                                <br />
                                                                                                <asp:Chart ID="chrtResultSTPTO" runat="server" AlternateText="DSP" Visible="false"
                                                                                                    BorderlineWidth="2" Width="650px" Height="580px" IsSoftShadows="false">
                                                                                                    <Titles>
                                                                                                        <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="DSP" ForeColor="26, 59, 105">
                                                                                                        </asp:Title>
                                                                                                    </Titles>
                                                                                                    <Legends>
                                                                                                        <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                                            Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold">
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
                                                                                                            <AxisY ArrowStyle="None" ToolTip="Value(Rs.)" TextOrientation="Horizontal" LineColor="#013974">
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
                                                                                                                                                                Text="DSP SIP Chart" ForeColor="26, 59, 105">
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
                                                                                            <%--   <asp:Image runat="server" ID="imgchrtResult"  DescriptionUrl="~/DSP/IMG/excel.png" />--%>
                                                                                            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                                                                                        </asp:View>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:View ID="View3" runat="server">
                                                                                            <%--<div id="country3" class="tabcontent">--%>
                                                                                            <asp:GridView ID="GridViewSIPResult" runat="server" Width="100%" RowStyle-CssClass="grdRow"
                                                                                                HeaderStyle-BackColor=" #4395cd" AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                                                AllowPaging="false" AutoGenerateColumns="false" OnRowDataBound="GridViewSIPResult_RowDataBound">
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
                                                                                                HeaderStyle-BackColor=" #4395cd" AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                                                AllowPaging="false" AutoGenerateColumns="false">
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
                                                                                                <asp:GridView ID="GridViewResultLS" runat="server" Width="100%" CssClass="grdRow"
                                                                                                    RowStyle-CssClass="grdRow" HeaderStyle-BackColor=" #4395cd" AlternatingRowStyle-CssClass="grdRow"
                                                                                                    HeaderStyle-CssClass="grdHead" AutoGenerateColumns="false">
                                                                                                    <Columns>
                                                                                                        <%--<asp:TemplateField
                                                                                                                    HeaderText="Type"> <ItemTemplate> <%# (Eval("Type") == DBNull.Value) ? "--" : Eval("Type").ToString()%>
                                                                                                                    </ItemTemplate> <ItemStyle HorizontalAlign="Left" /> </asp:TemplateField>--%>
                                                                                                        <asp:TemplateField HeaderText="Scheme Name">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Scheme_Name") == DBNull.Value)
                                                                                                                                ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)" %>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="360px" />
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
                                                                                                <br />
                                                                                            </div>
                                                                                            <%--</div>--%>
                                                                                        </asp:View>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:View ID="View4" runat="server">
                                                                                            <div id="Showpdfdiv" runat="server" class="FieldHead" style="border: 1px solid #569fd3">
                                                                                                <%-- <b>Please select your Credential:</b>--%>
                                                                                                <table width="100%" style="padding-top: 20px;">
                                                                                                    <tr>
                                                                                                        <td align="left" width="100%" style="padding-left: 8px;">
                                                                                                            <asp:RadioButtonList ID="RadioButtonListCustomerType" runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged"
                                                                                                                TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true" BorderColor="#569fd3"
                                                                                                                BorderWidth="0" Width="250px" CssClass="lefft">
                                                                                                                <asp:ListItem>Distributor</asp:ListItem>
                                                                                                                <asp:ListItem Selected="true">Not a Distributor</asp:ListItem>
                                                                                                            </asp:RadioButtonList>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <div id="DistributerDiv" runat="server" visible="false" style="margin: 0; padding: 0;">
                                                                                                    <table id="tblDistb" width="60%" align="left" style="padding: 0px; margin: 0">
                                                                                                        <tr>
                                                                                                            <td width="40%" align="left" style="padding-left: 30px;">
                                                                                                                ARN No
                                                                                                            </td>
                                                                                                            <td width="60%" align="left">
                                                                                                                <%--<asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>--%>
                                                                                                                <asp:TextBox ID="txtArn" CssClass="ddl_3" runat="server" MaxLength="30"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                Prepared By
                                                                                                            </td>
                                                                                                            <td width="68%" align="left">
                                                                                                                <asp:TextBox ID="txtPreparedby" CssClass="ddl_3" runat="server" MaxLength="40"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                Contact No(Mobile)
                                                                                                            </td>
                                                                                                            <td width="68%" align="left">
                                                                                                                <asp:TextBox ID="txtMobile" CssClass="ddl_3" runat="server" MaxLength="14"></asp:TextBox>
                                                                                                                <%--onkeypress="return isNumber(event)"--%>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                Email
                                                                                                            </td>
                                                                                                            <td width="68%" align="left">
                                                                                                                <asp:TextBox ID="txtEmail" CssClass="ddl_3" runat="server" MaxLength="30"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                Prepared For
                                                                                                            </td>
                                                                                                            <td width="68%" align="left">
                                                                                                                <asp:TextBox ID="txtPreparedFor" CssClass="ddl_3" runat="server" MaxLength="40"></asp:TextBox>
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
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td width="22%" align="left" style="padding-left: 8px;">
                                                                                                            Generate PDF Report:
                                                                                                        </td>
                                                                                                        <td width="68%" align="left">
                                                                                                            <asp:LinkButton ID="LinkButtonGenerateReport" runat="server" OnClick="LinkButtonGenerateReport_Click"
                                                                                                                ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();" UseSubmitBehavior="false"><img src="IMG/downloadPDF.jpg" style="border: 0;" alt=""  /></asp:LinkButton>
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
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div id="disclaimerDiv" runat="server">
                            <table width="100%">
                                <tr align="left">
                                    <td valign="top" class="rslt_text1">
                                        <div align="justify">
                                            <b>Disclaimer:</b>
                                            <br />
                                            <asp:Label ID="LSDisc" runat="server" Text="<b><br/>* Returns here denote the Extended Internal Rate of Return (XIRR).  </br></b>"
                                                Visible="true"></asp:Label>
                                            <asp:Label ID="LSDisc1" runat="server" Text="<b><br/>* For Time Periods > 1 yr, CAGR Returns have been shown. For Time Periods < 1 yr, Absolute Returns have been shown. </br></b>"
                                                Visible="false"></asp:Label>
                                            <b>• Since Inception return of the benchmark is calculated from the scheme inception
                                                date.</b>
                                            <br />
                                            Past performance may or may not be sustained in the future and should not be used
                                            as a basis for comparison with other investments.
                                        </div>
                                    </td>
                                </tr>
                                <tr align="left">
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
                                            The return calculator has been developed and is maintained by ICRA Analytics Limited.
                                            DSP Investment Managers Pvt. Ltd./Trustees do not endorse the authenticity
                                            or accuracy of the figures based on which the returns are calculated; nor shall
                                            they be held responsible or liable for any error or inaccuracy or for any losses
                                            suffered by any investor as a direct or indirect consequence of relying upon the
                                            data displayed by the calculator.
                                        </div>
                                        <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="left">
                                            <tr>
                                                <td class="text" align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text" align="right">
                                                    <span style="text-align: right" class="rslt_text1">Developed by:<a class="text" href="https://www.icraanalytics.com"
                                                        target="_blank"> ICRA Analytics Ltd</a>, <a class="text" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer </a></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
