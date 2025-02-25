<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="returnCalc_old.aspx.cs" Inherits="iFrames.Edelweiss.returnCalc_old" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Edelweiss Calculator </title>
    <script src="Script/jquery-1.8.3.js" type="text/javascript"></script>
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
        //Add by koustav 5-apr-2018
        function ChangeRegularPayout4Swp(InitialInvest, Frequency, Percentange) {
           
            var Amount = parseFloat((InitialInvest * (Percentange / 100)) / Frequency);
            $('#txtTransferWithdrawal2').val(Math.round(Amount));
        }
        

    </script>
    <link rel="stylesheet" type="text/css" href="CSS/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="CSS/demos.css" />
    <style type="text/css">
        body
        {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
            font: 12px/20px Verdana;
        }
        .layout
        {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }
        .layout-slider
        {
            width: 100px;
        }
        .layout-slider-settings
        {
            font-size: 12px;
            padding-bottom: 0px;
        }
        .layout-slider-settings pre
        {
            font-family: arial;
        }
        
        .layout1
        {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }
        .layout-slider1
        {
            width: 50px;
        }
        .layout-slider1-settings
        {
            font-size: 12px;
            padding-bottom: 0px;
        }
        .layout-slider1-settings pre
        {
            font-family: arial;
        }
        
        .layout2
        {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }
        .layout-slider2
        {
            width: 100px;
            margin: 2px;
            padding: 0;
        }
        .layout-slider2-settings
        {
            font-size: 12px;
            padding-bottom: 0px;
        }
        .layout-slider2-settings pre
        {
            font-family: arial;
        }
        
        .layout-slider4
        {
            width: 100px;
            margin: 2px;
            padding: 0;
        }
        .lefft
        {
            padding-left: 2px;
        }
        .borderlefft
        {
            border-bottom: #c6c8ca solid 1px;
        }
        
        .borderbottom
        {
            border-bottom: #c6c8ca solid 1px;
        }
        table th {
            font-weight: normal;
            }
        
       </style>
    <link href="CSS/master.css" rel="stylesheet" type="text/css" media="all" />
    <link href="CSS/styles.css" rel="stylesheet" type="text/css" />
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
.layout-slider3 {width: 100px; padding:0; margin:2px;}
        
        .style1
        {
            height: 30px;
        }
        
-->
</style>
    <link href="CSS/tabcontent.css" rel="stylesheet" type="text/css" />
    <%--<script src="Script/tabcontent.js" type="text/javascript"></script>--%>
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
                        ChangeRegularPayout4Swp(InitialAmt, dy,PayoutPercentage );
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
                        ChangeRegularPayout4Swp(InitialAmt, dy,PayoutPercentage );
                    }
                }
            });
            //End
        });        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="newDiv" runat="server">
            <table width="670" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr align="left">
                    <td valign="top">
                        <div class="pageCont">
                            <div class="prdHold">
                                <div class="blueBox">
                                    <div class="">
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr align="left">
                                                <td valign="top">
                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                        <tr align="left">
                                                            <%--<td width="1%" valign="top">
                                                                <img src="IMG/spacer11.gif" width="10" height="1" alt="" />
                                                            </td>--%>
                                                            <td width="100%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td align="left" valign="top">
                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td width="19%" class="style1">
                                                                                                    <span class="FieldHead">Investment Mode</span>
                                                                                                </td>
                                                                                                <td class="style1">
                                                                                                    <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true" CssClass="ddl" Enabled="False" >
                                                                                                        <asp:ListItem  Value="SIP">SIP</asp:ListItem>
                                                                                                        <asp:ListItem  Value="Lump Sum">Lump Sum</asp:ListItem>
                                                                                                        <%--<asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>--%>
                                                                                                        <asp:ListItem  Value="SWP">SWP/Regular Payout Facility</asp:ListItem>
                                                                                                        <asp:ListItem  Value="STP">STP</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" id="trCategory" runat="server" visible="true">
                                                                                                <td height="30" align="left" valign="middle" width="23%">
                                                                                                    <span class="FieldHead">Category</span>
                                                                                                </td>
                                                                                                <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                    <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" DataTextField="Nature"
                                                                                                        DataValueField="Nature" CssClass="ddl" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" id="tr1" runat="server" visible="true">
                                                                                                <td height="30" align="left" valign="middle" width="23%">
                                                                                                    <span class="FieldHead">Option</span>
                                                                                                </td>
                                                                                                <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                    <asp:DropDownList ID="ddlOption" runat="server" AutoPostBack="true" CssClass="ddl"
                                                                                                        OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left">
                                                                                                <td height="30" align="left" valign="middle" width="23%">
                                                                                                    <span class="FieldHead">
                                                                                                        <asp:Label ID="lblSchemeName" runat="server" Text="Scheme Name"></asp:Label></span>
                                                                                                </td>
                                                                                                <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                    <asp:DropDownList ID="ddlscheme" runat="server" CssClass="ddl" Width="" AutoPostBack="True"
                                                                                                        OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" id="trInception1" runat="server">
                                                                                                <td height="27" align="left" valign="middle">
                                                                                                    <%-- <span id="Span2" class="FieldHead">Scheme Inception Date</span>--%>
                                                                                                    <asp:Label ID="LabelInception" runat="server" CssClass="FieldHead" Text="Inception Date"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3" align="left" valign="middle">
                                                                                                    <%--<asp:Label ID="SIPSchDt" runat="server" CssClass="FieldHead" Width="150px"></asp:Label>--%>
                                                                                                    <asp:TextBox ID="SIPSchDt" runat="server" CssClass="ddl_3" ReadOnly="true" Text=""></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" id="trTransferTo" runat="server" visible="false">
                                                                                                <td height="30" valign="middle" width="145">
                                                                                                    <span class="FieldHead">Transfer To</span>
                                                                                                </td>
                                                                                                <td width="535" align="left" valign="middle" style="height: 25px">
                                                                                                    <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True" CssClass="ddl"
                                                                                                        Width="360px" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" id="trInception" runat="server" visible="false">
                                                                                                <td height="27" align="left" valign="middle">
                                                                                                    <span id="Span1" class="FieldHead">Inception Date</span>
                                                                                                </td>
                                                                                                <td colspan="3" align="left" valign="middle">
                                                                                                    <%--<asp:Label ID="SIPSchDt" runat="server" CssClass="FieldHead" Width="150px"></asp:Label>--%>
                                                                                                    <asp:TextBox ID="SIPSchDt2" runat="server" CssClass="ddl_3" ReadOnly="true" Text=""></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" id="trBenchmark" runat="server" visible="true">
                                                                                                <td height="30" align="left" valign="middle" width="23%">
                                                                                                    <span id="Span3" class="FieldHead">Benchmark</span>
                                                                                                </td>
                                                                                                <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                    <%--<asp:DropDownList ID="ddlsipbnmark" runat="server" CssClass="ddl" Width="190px" Enabled="True">
                                                                                                    </asp:DropDownList>--%>
                                                                                                    <asp:TextBox ID="txtddlsipbnmark" runat="server" CssClass="ddl_3" Width="200px" Text=""
                                                                                                        ReadOnly="true"></asp:TextBox>
                                                                                                    <%-- <b>
                                                                                                        <asp:Label ID="lblddlsipbnmark" runat="server" CssClass="FieldHead" Text=""></asp:Label></b>--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left" id="trInitialInvst" runat="server" visible="false">
                                                                                    <td valign="top">
                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            <tr align="left">
                                                                                                <td width="23%" height="30" class="FieldHead">
                                                                                                    Initial Amount (Rs.)
                                                                                                </td>
                                                                                                <td width="22%" valign="middle">
                                                                                                    <asp:TextBox ID="txtiniAmount" runat="server" CssClass="ddl_3" MaxLength="10" ReadOnly="false"
                                                                                                        Text="" Style="width: 90px;"></asp:TextBox>
                                                                                                </td>
                                                                                                <td width="22%" class="FieldHead">
                                                                                                    Initial Date
                                                                                                </td>
                                                                                                <td width="33%" valign="middle" colspan="3">
                                                                                                    <asp:TextBox ID="txtIniToDate" runat="server" CssClass="ddl_3" Style="width: 90px;"
                                                                                                        onMouseDown="Javascript: setDate();">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr align="left" id="trSipInvst" runat="server" visible="true">
                                                                                    <td valign="top">
                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            <tr align="left" runat="server" id="SIP_withdrawl">
                                                                                                <td height="30" valign="middle" width="23%">
                                                                                                    <span>
                                                                                                        <asp:Label ID="lblInstallmentAmt" runat="server" Text="Installment Amount (Rs.)"
                                                                                                            class="FieldHead"></asp:Label></span>
                                                                                                </td>
                                                                                                <td width="22%" valign="middle">
                                                                                                    <asp:TextBox ID="txtinstall" CssClass="ddl_3" MaxLength="10" Text="" runat="server"
                                                                                                        ReadOnly="false" Style="width: 90px;" onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                                                                                                </td>
                                                                                                <td height="30" valign="middle" width="22%">
                                                                                                    <asp:Label ID="lblTransferWithdrawal" runat="server" class="FieldHead" Text="Withdrawal Amount"></asp:Label>&nbsp;
                                                                                                </td>
                                                                                                <td width="33%" valign="middle">
                                                                                                    <asp:TextBox ID="txtTransferWithdrawal" runat="server" CssClass="ddl_3" MaxLength="14"
                                                                                                        onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false" Style="width: 90px;"></asp:TextBox>&nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left">
                                                                                                <td height="30" valign="middle" width="23%">
                                                                                                    <span class="FieldHead">Frequency</span>
                                                                                                </td>
                                                                                                <td width="20%" valign="middle">
                                                                                                    <asp:DropDownList ID="ddPeriod_SIP" runat="server" Width="95px" CssClass="ddl_3">
                                                                                                        <%--<asp:ListItem Value="Monthly" Selected="True">Monthly</asp:ListItem>
                                                                                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                                                                                        <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>--%>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td width="22%" class="FieldHead">
                                                                                                    <asp:Label ID="lblDiffDate" runat="server" Text="SIP Date"></asp:Label>
                                                                                                </td>
                                                                                                <td width="35%" valign="middle" colspan="3">
                                                                                                    <asp:DropDownList ID="ddSIPdate" runat="server" CssClass="ddl_3">
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
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" id="SWP_STP_withdrawl" runat="server" visible="false">
                                                                                                <td height="30" valign="middle" width="23%">
                                                                                                    <asp:Label ID="lblTransferWithdrawal2" runat="server" class="FieldHead" Text="Withdrawal Amount"></asp:Label>&nbsp;
                                                                                                </td>
                                                                                                <td width="22%" valign="middle" id="Swp_Persentage_payout" runat="server" visible="false">
                                                                                                      <asp:DropDownList ID="DropDn4PayoutSwp" runat="server" Width="95px" CssClass="ddl_3">
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
                                                                                                </td>
                                                                                                <td height="30" valign="middle" width="22%">
                                                                                                    <asp:TextBox ID="txtTransferWithdrawal2" runat="server" CssClass="ddl_3" MaxLength="14"
                                                                                                        onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false" Style="width: 90px;"></asp:TextBox>&nbsp;
                                                                                                </td>
                                                                                                <td width="33%" valign="middle" id="Swp_Persentage_payout_message" runat="server" visible="false">
                                                                                                    <asp:Label ID="Label4SwpPayout" runat="server" class="FieldHead" Text="(For regular payout facility per annum)"></asp:Label>&nbsp;
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
                                                                                            
                                                                                        </table>


                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            <tr align="left">
                                                                                                <td width="23%" height="30" valign="middle" class="FieldHead">
                                                                                                    From Date
                                                                                                </td>
                                                                                                <td width="22%" valign="middle">
                                                                                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="ddl_3" Style="width: 90px;"
                                                                                                        ></asp:TextBox>
                                                                                                    <%-- &nbsp;<img src="IMG/calender2.gif" id="img1" onmousedown="Javascript: setDate();"
                                                                                                        style="vertical-align: middle" />--%>
                                                                                                </td>
                                                                                                <td width="22%" valign="middle" class="FieldHead">
                                                                                                    To Date
                                                                                                </td>
                                                                                                <td width="33%" valign="middle">
                                                                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="ddl_3" Style="width: 90px;"
                                                                                                        onChange="Javascript: setDateValueAsOn(); " >
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left">
                                                                                                <td width="23%" height="30" valign="middle" class="FieldHead">
                                                                                                    Current Date
                                                                                                </td>
                                                                                                <td width="20%" valign="middle">
                                                                                                    <asp:TextBox ID="txtvalason" runat="server" CssClass="ddl_3" Style="width: 90px;"
                                                                                                        >
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                                <td width="57%" valign="middle" class="FieldHead" colspan="2">
                                                                                                    ('Current Date' should be greater than 'To Date')
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr id="tr_SIP_Sinc" runat="server">
                                                                                                <td width="23%" height="30" valign="middle" class="FieldHead">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td width="77%" valign="middle" colspan="3">
                                                                                                    <input type="checkbox" id="chkInception4sip" runat="server" onclick="setIncDate();" />
                                                                                                    Calculate from Inception
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr id="tr_Cal_Type" runat="server" visible="false">
                                                                                                <td width="23%" height="30" valign="middle" class="FieldHead">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td width="77%" valign="middle" colspan="3" class="FieldHead">
                                                                                                    <asp:RadioButton Text=" Reinvest" ID="rdbReinvest" Checked="true" runat="server"
                                                                                                        GroupName="rdbCalc" />
                                                                                                    <asp:RadioButton Text=" Payout" ID="rdbPayout" runat="server" GroupName="rdbCalc" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr align="left" id="trLumpInvst" runat="server" visible="false">
                                                                                    <td align="left" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="23%" height="30" class="FieldHead">
                                                                                                    Investment Amount (Rs.)
                                                                                                </td>
                                                                                                <td colspan="3" valign="middle">
                                                                                                    <asp:TextBox ID="txtinstallLs" value="" MaxLength="10" runat="server" CssClass="ddl_3"
                                                                                                        Style="width: 90px;" onmousedown="Javascript: setDate();" onChange="Javascript: checkInvestedValue();">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="23%" height="30" valign="middle" class="FieldHead">
                                                                                                    Start Date
                                                                                                </td>
                                                                                                <td width="22%" valign="middle">
                                                                                                    <asp:TextBox ID="txtLumpfromDate" runat="server" CssClass="ddl_3" Style="width: 90px;"
                                                                                                         onMouseDown="Javascript: setDate();" onchange="Javascript:setIncDate();">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                                <td width="12%" valign="middle" class="FieldHead">
                                                                                                    End Date
                                                                                                </td>
                                                                                                <td width="43%" valign="middle">
                                                                                                    <asp:TextBox ID="txtLumpToDate" runat="server" CssClass="ddl_3" Style="width: 90px;"
                                                                                                        >
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr id="tr_Cal_Incp" runat="server">
                                                                                                <td width="23%" height="30" valign="middle" class="FieldHead">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td width="77%" valign="middle" colspan="3">
                                                                                                    <input type="checkbox" id="chkInception" runat="server" onclick="setIncDate();" />
                                                                                                    Calculate from Inception
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr id="trCalTypeLmpsm" runat="server" visible="false">
                                                                                                <td width="23%" height="30" valign="middle" class="FieldHead">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td width="77%" valign="middle" colspan="3" class="FieldHead">
                                                                                                    <asp:RadioButton Text=" Reinvest" ID="lmprdbReinvest" Checked="true" runat="server"
                                                                                                        GroupName="rdbCalc" />
                                                                                                    <asp:RadioButton Text=" Payout" ID="lmprdbPayout" runat="server" GroupName="rdbCalc" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr align="left" valign="middle">
                                                                                    <td style="padding-top: 10px;">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr align="left">
                                                                                                <td width="23%" valign="middle">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td width="40%" valign="middle">
                                                                                                    <asp:ImageButton ID="sipbtnshow" runat="server" ImageUrl="~/Edelweiss/SipCalc/IMG/calculate.jpg"
                                                                                                        OnClick="sipbtnshow_Click"></asp:ImageButton>
                                                                                                    <span style="padding-left: 10px;">
                                                                                                        <asp:ImageButton ID="sipbtnreset" runat="server" ImageUrl="~/Edelweiss/SipCalc/IMG/reset.jpg"
                                                                                                            OnClick="sipbtnreset_Click"></asp:ImageButton></span>
                                                                                                </td>
                                                                                                <td width="37%" valign="middle">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                                    <td>
                                                                                        <div style="background-color: #f27641; height: 3px; width: 100%">
                                                                                        </div>
                                                                                        <%--<img style="margin-bottom: 10px; margin-left: 85px" src="IMG/down-pointer-image.png" height="11" width="14" alt="" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                    <tr align="left" valign="top">
                                                                        <td>
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
                                                                                                &nbsp;&nbsp;&nbsp;<b>View Historical Fund Performance below:<b /></div>
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
                                                                                                            <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="180px" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Units<br/> Purchased <br/>" HeaderStyle-VerticalAlign="Top" >
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Total_unit") == DBNull.Value) ? "--" : Eval("Total_unit").ToString().Trim() == "0" ? "--" : Math.Round(Convert.ToDouble( Eval("Total_unit")),0).ToString("n0")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Amount<br/> Invested <br/> (A)" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Total_amount") == DBNull.Value) ? "--" : Eval("Total_amount").ToString().Trim() == "0" ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 2).ToString("n0")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Investment Value<br/> as on Date <br/> (B)" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Present_Value") == DBNull.Value) ? "--" : Eval("Present_Value").ToString().Trim() == "0" ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 2).ToString("n0")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Dividend <br/>Amount<br/>( C )<br/>" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Dividend_Amount") == DBNull.Value || Eval("Dividend_Amount") == "--" || Eval("Dividend_Amount").ToString() == "0") ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Dividend_Amount")),2).ToString("n0")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Total Profit<br/> (B+C-A) <br/>" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : Eval("Profit_Sip").ToString().Trim() == "0" ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 2).ToString("n0")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Abs. <br/>Returns <br/>" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : Eval("ABSOLUTERETURN").ToString().Trim() == "0" ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Returns*" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString().Trim() == "0" ? "--" : (Eval("Yield").ToString() + "%")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderlefft" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                                <br />
                                                                                                <span id="sipDisclaimer" class="FieldHead" style="padding: 4px; margin: 2px" visible="false"
                                                                                                    runat="server"></span>
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
                                                                                                        <asp:TemplateField HeaderText="Amount<BR/> Invested (A)" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderbottom" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Amount<BR/> Withdrawn (B)" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderbottom" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Present<BR/> Value (C)" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="borderbottom" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Total Profit<BR/> (B+C-A)" HeaderStyle-VerticalAlign="Top">
                                                                                                            <ItemTemplate>
                                                                                                                <%# "<img src='img/rimg.png' style='vertical-align:middle;'> "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
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
                                                                                                            <asp:GridView ID="GridViewLumpSum" runat="server" Width="100%" CssClass="grdRow" HeaderStyle-BackColor="#02509b"
                                                                                                                RowStyle-CssClass="grdRow" AlternatingRowStyle-CssClass="grdRow" HeaderStyle-CssClass="grdHead"
                                                                                                                AutoGenerateColumns="false">
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="230px" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Amount Invested <br/>(A)" HeaderStyle-VerticalAlign="Top">
                                                                                                                        <HeaderStyle  />
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("InvestedAmount") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedAmount")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Investment Value<br/>(B)" HeaderStyle-VerticalAlign="Top">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Dividend Amount<br/>(C)" HeaderStyle-VerticalAlign="Top">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%# (Eval("DividendAmount") == "--") ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Eval("DividendAmount")%>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Profit from Investment<br/>(B+C-A)" HeaderStyle-VerticalAlign="Top">
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
                                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                                <tr align="left">
                                                                                                                    <td align="left" valign="top">
                                                                                                                        <ul id="countrytabs" class="shadetabs">
                                                                                                                            <li>
                                                                                                                                <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click">View Detail Report</asp:LinkButton></li>
                                                                                                                            <li>
                                                                                                                                <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click">View Graph</asp:LinkButton></li>
                                                                                                                            <li>
                                                                                                                                <asp:LinkButton ID="lnkTab3" runat="server" Visible="false" OnClick="lnkTab3_Click">View Historical Performance</asp:LinkButton></li>
                                                                                                                            <li>
                                                                                                                                <asp:LinkButton ID="lnkTab4" runat="server" OnClick="lnkTab4_Click">View PDF Report</asp:LinkButton></li>
                                                                                                                            <li>
                                                                                                                                <asp:LinkButton ID="lnkTab5" runat="server" Visible="false" OnClick="lnkTab5_Click">View Dividend History</asp:LinkButton></li>
                                                                                                                        </ul>
                                                                                                                        <div style="width: 100%; padding-left: 1px; padding-right:1px">
                                                                                                                            <asp:MultiView ID="MultiView1" runat="server">
                                                                                                                                <table width="100%" cellpadding="0" cellspacing="5">
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
                                                                                                                                                        <b>From Scheme: </b>
                                                                                                                                                            <%= ddlscheme.SelectedItem.Text %>
                                                                                                                                                       
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
                                                                                                                                                        <b>To Scheme: </b>
                                                                                                                                                            <%= ddlschtrto.SelectedItem.Text %>
                                                                                                                                                       
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
                                                                                                                                                    
                                                                                                                                                    <table width="100%">
                                                                                                                                                        <tr>
                                                                                                                                                            <td align="right" style="padding-right: 20px">
                                                                                                                                                                <asp:ImageButton ID="btnExcelCalculation" runat="server" ImageUrl="~/Edelweiss/IMG/excell.png"
                                                                                                                                                                    ToolTip="Download Excel" Text="Show Excel Calculation" Visible="true" OnClick="ExcelCalculation_Click" />
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
                                                                                                                                                    <asp:Chart ID="chrtResult" runat="server" AlternateText="Edelweiss" Visible="true"
                                                                                                                                                        BorderlineWidth="2" Width="650px" Height="580px" IsSoftShadows="false">
                                                                                                                                                        <%--BackGradientStyle="Center"  BorderlineColor="RoyalBlue" BackColor="Gray"--%>
                                                                                                                                                        <Titles>
                                                                                                                                                            <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="Edelweiss" ForeColor="26, 59, 105">
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
                                                                                                                                                    <asp:Chart ID="chrtResultSTPTO" runat="server" AlternateText="Edelweiss" Visible="false"
                                                                                                                                                        BorderlineWidth="2" Width="650px" Height="580px" IsSoftShadows="false">
                                                                                                                                                        <Titles>
                                                                                                                                                            <asp:Title Font="Trebuchet MS, 14pt, style=Bold" Text="Edelweiss" ForeColor="26, 59, 105">
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
                                                                                                                                                    <asp:GridView ID="GridViewResultLS" runat="server" Width="100%" CssClass="grdRow"
                                                                                                                                                        RowStyle-CssClass="grdRow" HeaderStyle-BackColor="#02509b" AlternatingRowStyle-CssClass="grdRow"
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
                                                                                                                                                            <td align="left" width="100%">
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
                                                                                                                                                    <div id="DistributerDiv" runat="server" visible="false">
                                                                                                                                                        <table id="tblDistb" width="100%" align="center">
                                                                                                                                                            <tr>
                                                                                                                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                    ARN No
                                                                                                                                                                </td>
                                                                                                                                                                <td width="68%" align="left">
                                                                                                                                                                    <%--<asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>--%>
                                                                                                                                                                    <asp:TextBox ID="txtArn" CssClass="ddl_3" runat="server" MaxLength="30"></asp:TextBox>
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
                                                                                                                                                                <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                    Contact No(Mobile)
                                                                                                                                                                </td>
                                                                                                                                                                <td width="68%" align="left">
                                                                                                                                                                    <asp:TextBox ID="txtMobile" CssClass="ddl_3" runat="server" MaxLength="14"></asp:TextBox>
                                                                                                                                                                    <%--onkeypress="return isNumber(event)"--%>
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
                                                                                                                                                        </table>
                                                                                                                                                    </div>
                                                                                                                                                    <table width="100%">
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="32%" align="left" style="padding-left: 30px;">
                                                                                                                                                                Generate PDF Report:
                                                                                                                                                            </td>
                                                                                                                                                            <td width="68%" align="left">
                                                                                                                                                                <asp:LinkButton ID="LinkButtonGenerateReport" runat="server" OnClick="LinkButtonGenerateReport_Click"
                                                                                                                                                                    ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();"><img src="IMG/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:LinkButton>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </div>
                                                                                                                                            </asp:View>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <asp:View ID="View5" runat="server">
                                                                                                                                                <div id="showDivHistory" runat="server">
                                                                                                                                                    <asp:GridView ID="gridDivHistory" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                                                                                        HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdRow" RowStyle-CssClass="grdRow">
                                                                                                                                                        <Columns>
                                                                                                                                                            <asp:TemplateField HeaderText="NAV_DATE">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(Eval("Nav_Date")).ToString("dd-MMM-yyyy")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="Dividend">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("DIVIDEND") == DBNull.Value) ? "--" : (Eval("DIVIDEND") == string.Empty) ? "--" : Convert.ToDouble(Eval("DIVIDEND")).ToString("n2")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="Bonus">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("BONUS") == DBNull.Value) ?  "--" : (Eval("BONUS") == string.Empty) ? "--" :  Eval("BONUS").ToString()%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
                                                                                                                                                            </asp:TemplateField>
                                                                                                                                                            <asp:TemplateField HeaderText="PAYOUT_AMOUNT">
                                                                                                                                                                <ItemTemplate>
                                                                                                                                                                    <%# (Eval("PAYOUT_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("PAYOUT_AMOUNT")), 2).ToString("n2")%>
                                                                                                                                                                </ItemTemplate>
                                                                                                                                                                <ItemStyle CssClass="borderlefft" />
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
                                                                                                <%-- <b>• The returns for funds with the dividend option are calculated assuming that the
                                                                                                    dividends are reinvested into the fund.</b>--%>
                                                                                                <br />
                                                                                                Past performance may or may not be sustained in the future and should not be used
                                                                                                as a basis for comparison with other investments.</div>
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
                                                                                               Edelweiss Asset Management Limited (EAML)  do not endorse the authenticity
                                                                                                or accuracy of the figures based on which the returns are calculated; nor shall
                                                                                                they be held responsible or liable for any error or inaccuracy or for any losses
                                                                                                suffered by any investor as a direct or indirect consequence of relying upon the
                                                                                                data displayed by the calculator.<br /><br />
                                                                                            </div>
                                                                                            <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="left">
                                                                                                <tr>
                                                                                                    <td class="text" align="right">
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="text" align="right">
                                                                                                        <span style="text-align: right" class="rslt_text1">Developed by:<a class="text" href="https://www.icraanalytics.com"
                                                                                                            target="_blank"> ICRA Analytics Ltd </a>, <a class="text" href="https://icraanalytics.com/home/Disclaimer"
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
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <%--<td width="1%" valign="top">
                                                                <img src="IMG/spacer11.gif" width="10" height="1" />
                                                            </td>--%>
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
    </div>
    </form>
</body>
</html>
