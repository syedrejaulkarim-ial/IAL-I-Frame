<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HDFCSipCalc.aspx.cs" Inherits="iFrames.HDFC_SIP.HDFCSipCalc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Raleway:300,400,500,600' rel='stylesheet' type='text/css' />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/nifty.min.v1.css" rel="stylesheet" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="font-awesome-4.6.3/css/font-awesome.css" rel="stylesheet" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/bootstrap-select.min.js"></script>

    <script src="../Scripts/HighStockChart/highstock.js" type="text/javascript"></script>
    <script src="../Scripts/HighStockChart/exporting.js" type="text/javascript"></script>
    <script type="text/javascript">
        function GetMinDate() {
            //debugger;
            var ddScheme = document.getElementById('SIPSchDt');
            var allotdate = ddScheme.value;
            var Day = parseInt(allotdate.substring(0, 2), 10);
            var Mn = parseInt(allotdate.substring(3, 5), 10);
            var Yr = parseInt(allotdate.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            //var DateVal = Day + "/" + Mn + "/" + Yr;
            var dt = new Date(DateVal);
            return dt;
            //return ddScheme.value;
        }

        function SplitDate(val) {
            return (val.getMonth() + 1) + '/' + val.getDate() + '/' + val.getFullYear();
        }

        $(function () {
           // $("#loader").show();
            //$("body").fadeIn();
            //$('.datepicker').datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    startDate: GetMinDate(),
            //    endDate: 'now'
            //});
            //$('#FromDate').datepicker().on('changeDate', function (ev) {
            //    debugger;
            //    var calval = $("#FromDate").datepicker("getDate");
            //    $('#FromDate').val(Date.parse(calval, "dd MMM yyyy").toString("dd MMM yyyy"));
            //    $('#FromDate').datepicker('hide');
            //});
            //$('#ToDate').datepicker().on('changeDate', function (ev) {
            //    debugger;
            //    var calval = $("#ToDate").datepicker("getDate");
            //    $('#ToDate').val(Date.parse(calval, "dd MMM yyyy").toString("dd MMM yyyy"));
            //    $('#ToDate').datepicker('hide');
            //});

            // $('#FromDate').val(Date.parse(SplitDate($('#FromDate').val()), "MM/dd/yyyy"));
            // $('#ToDate').val(Date.parse(SplitDate($('#ToDate').val()), "MM/dd/yyyy"));

            if ($("#hdnNABenchmark").val() == "1") {
                $(".NABenchmark").hide();
            }
            else {
                $(".NABenchmark").show();
            }
            var year = new Date().getFullYear().toString();
            var schemeLaunchDate = GetMinDate().getFullYear().toString();
            $("#FromDate").datepicker({
                //dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                //maxDate: -1,
                minDate: GetMinDate(),
                yearRange:schemeLaunchDate+":"+year

            });
            $("#ToDate").datepicker({
                //dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                //maxDate: -1,
                minDate: GetMinDate(),
                yearRange:schemeLaunchDate+":"+year

            });
            //btnPlotclick();
            $('#ddlscheme').change(function () { $("#loader").show(); });
            $('#ddlFrequency').change(function () {
                $("#loader").show();
                //CalculateSip();
                btnPlotclick();
            });
            $('#txtiniAmount').change(function () {
                $("#loader").show();
                //CalculateSip();
                btnPlotclick();
            });
            $('#FromDate').change(function () {
                $("#loader").show();
                //CalculateSip();
                btnPlotclick();
            });
            $('#ToDate').change(function () {
                $("#loader").show();
                //CalculateSip();
                btnPlotclick();
            });
            $('#demo-panel-w-switch').change(function () { $("#loader").show(); window.location.href = "/HDFC_SIP/HDFCLumpSumCalc.aspx?SchemeId=" + $('#<%=ddlscheme.ClientID %>').val() + "&NatureId=" + $('#<%=ddlNature.ClientID %>').val() + ""; });
        });
        $(document).ready(function () {
            $("#loader").show();
            //CalculateSip();
            btnPlotclick();
        });
        function CalculateSip() {
            
            var schIndId = $('#<%=ddlscheme.ClientID %>').val();
            var frmDate = $('#<%=FromDate.ClientID %>').val();
            var toData = $('#<%=ToDate.ClientID %>').val();
            var Amount = $('#<%=txtiniAmount.ClientID %>').val();
            var Frequency = $('#<%=ddlFrequency.ClientID %> option:selected').val();
            if ((schIndId == 0) || (schIndId == "") || (schIndId == undefined)) {
                // alert('Select Scheme!!!');
                $("#loader").hide();
                return;
            }
            if ((schIndId == 0) || (schIndId == "") || (schIndId == undefined)) {
                alert('Select Scheme!!!');
                $("#loader").hide();
                return;
            }
            if ((Amount == 0) || (Amount == "") || (Amount == undefined)) {
                alert('Give Amount!!!');
                $("#loader").hide();
                return;
            }
            if (Amount != "" && Amount != null) {
                if (!$.isNumeric(Amount)) {
                    alert('Give Proper Amount!!!');
                    $("#loader").hide();
                    return;
                }
            }
            if ((Amount < 500)) {

                alert('Amount should be equal or grater than 500 !!');
                $('#<%=txtiniAmount.ClientID %>').val(500);
                $("#loader").hide();
                return;
            }
            if ((frmDate == "") || (frmDate == undefined)) {
                alert('Select frmDate!!!');
                $("#loader").hide();
                return;
            }
            if ((toData == "") || (toData == undefined)) {
                alert('Select toData!!!');
                $("#loader").hide();
                return;
            }
            if (new Date(frmDate) > new Date(toData)) {
                alert('From date can not grater than Todate!!!');
                $("#loader").hide();
                return;
            }
            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "', Amount:'" + Amount + "', Frequency:'" + Frequency + "'}";
            $.ajax({
                type: "POST",
                url: "HDFCSipCalc.aspx/getCalculatedate",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    
                    var value = msg.d.split('#');
                    $('#<%=InitialAmt.ClientID %>').val(value[0]);
                    $('#<%=TotalInvs.ClientID %>').val(value[1]);                   
                    btnPlotclick();
                    $("#loader").hide();
                },
                error: function (msg) {
                    $("#loader").hide();
                    alert("Error! Try again...");
                }
            });
        }
        function btnPlotclick() {
            var NatureId = $('#<%=ddlNature.ClientID %>').val();
            var schIndId = $('#<%=ddlscheme.ClientID %>').val();
            var frmDate = $('#<%=FromDate.ClientID %>').val();
            var toData = $('#<%=ToDate.ClientID %>').val();
            var Amount = $('#<%=txtiniAmount.ClientID %>').val();
            var Frequency = $('#<%=ddlFrequency.ClientID %> option:selected').val();
            if ((schIndId == 0) || (schIndId == "") || (schIndId == undefined)) {
                $("#loader").hide();
                return;
            }
            if ((schIndId == 0) || (schIndId == "") || (schIndId == undefined)) {
                $("#loader").hide();
                alert('Please Select Scheme!!!');
                return;
            }
            if ((Amount == 0) || (Amount == "") || (Amount == undefined)) {
                alert('Please Give Amount!!!');
                $("#loader").hide();
                return;
            }
            if (Amount != "" && Amount != null) {
                if (!$.isNumeric(Amount)) {
                    alert('Please Give Proper Amount!!!');
                    $("#loader").hide();
                    return;
                }
            }
            if ((Amount < 500)) {
                alert('Amount should be equal or greater than 500 !!');
                $("#loader").hide();
                return;
            }
            if ((frmDate == "") || (frmDate == undefined)) {
                alert('Select from Date!!!');
                $("#loader").hide();
                return;
            }
            if ((toData == "") || (toData == undefined)) {
                alert('Select to Data!!!');
                $("#loader").hide();
                return;
            }
            if (new Date(frmDate) > new Date()) {
                alert('From date can not greater than current date !!!');
                $('#<%=FromDate.ClientID %>').val('');
                $("#loader").hide();
                return;
            }

            if (new Date(frmDate) >= new Date(toData)) {
                alert('From date can not grater than To date!!!');
                $("#loader").hide();
                $('#<%=FromDate.ClientID %>').val('');
                return;
            }
            if (new Date(toData) > new Date()) {
                alert('To date can not grater than current date !!!');
                $('#<%=ToDate.ClientID %>').val('');
                $("#loader").hide();
                return;
            }
           
            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "', Amount:'" + Amount + "', Frequency:'" + Frequency + "'}";
            $.ajax({
                type: "POST",
                url: "HDFCSipCalc.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    
                    // setChart(msg.d);
                     $("#loader").hide();
                     $('#<%=InitialAmt.ClientID %>').val(msg.d.InvestedAmt);
                    $('#<%=TotalInvs.ClientID %>').val(msg.d.InvestedValue);  
                    PlotAuto(msg.d, ImageArray);
                    // CalculateSip();

                },
                error: function (msg) {
                    alert("Error! Try again...");
                    $("#loader").hide();
                },
            });
        }
        function PlotAuto(dataConsolidated, ImageArray) {

            var max = dataConsolidated.MaxDate;
            var min = dataConsolidated.MinDate;
            var data = dataConsolidated.SimpleNavReturnModel;
            var dataplot = [[]];
            for (var i = 0; i < data.length; i += 1) {

                var tt1 = {};
                tt1.name = data[i].Name;

                var points = [];
                for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
                    var res = data[i].ValueAndDate[j].Date.split("-");

                    points.push([Date.UTC(res[0], res[1] - 1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue, data[i].ValueAndDate[j].InvestAmount]);

                }
                tt1.data = points;
                dataplot.push(tt1);
            }
            dataplot.shift();
            //  debugger;
            var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

            var colorarray = Array();
            for (var i = 0; i < ImageArray.length; i += 1) {
                colorarray.push(CustomSeriesColors[ImageArray[i]]);
            }

            Highcharts.setOptions({
                useUTC: false
            });

            Highcharts.stockChart('HighContainer', {

                legend: {
                    enabled: true,
                    symbolWidth: 40

                },
                rangeSelector: {
                    buttons: [{
                        type: 'month',
                        count: 1,
                        text: '1m'
                    }, {
                        type: 'month',
                        count: 3,
                        text: '3m'
                    }, {
                        type: 'month',
                        count: 6,
                        text: '6m'
                    }, {
                        type: 'ytd',
                        text: 'YTD'
                    }, {
                        type: 'year',
                        count: 1,
                        text: '1y'
                    }, {
                        type: 'year',
                        count: 3,
                        text: '3y'
                    }, {
                        type: 'year',
                        count: 5,
                        text: '5y'
                    }, {
                        type: 'year',
                        count: 10,
                        text: '10y'
                    }, {
                        type: 'year',
                        count: 15,
                        text: '15y'
                    }, {
                        type: 'all',
                        text: 'All'
                    }],
                    selected: 9
                },


                yAxis: {
                    title: {
                        min: 0,
                        text: 'Value',
                        style: {
                            fontWeight: 'bold',
                            color: 'black',
                            fontSize: "15px"
                        }
                    },
                    labels: {
                        formatter: function () {
                            return (this.value > 0 ? ' + ' : '') + this.value + '%';
                        }
                    },
                    plotLines: [{
                        value: 0,
                        width: 2,
                        color: 'silver'
                    }]
                },

                plotOptions: {
                    series: {
                        compare: 'percent',
                        showInNavigator: true
                    }
                },

                tooltip: {
                    shared: true,
                    backgroundColor: '#FCFFC5',

                    formatter: function () {
                        //var s = '';
                        //var d = new Date(this.x);
                        //var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
                        //var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                        //var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate() + ' ,' + d.getFullYear();
                        //s = s + '<span style="color:#839557">' + navdate + '</span><br /><br />';
                        //for (var i = 0; i < tt.length; i++) {
                        //    for (var k = 0; k < tt[i].data.length; k++) {
                        //        if (this.x === tt[i].data[k][0]) {
                        //            //s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>:<span>Amount:<b>' + tt[i].data[k][3].toString() + ',</b></span><span>Nav</span>: <b>' + (tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b>(' + this.points[i].point.change.toFixed(2) + '%)<br />';
                        //              s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>:<span>Amount:<b>' + (tt[i].data[k][3].toString() == 0 ? "N/A" : tt[i].data[k][3].toString()) + ',</b></span><span>Nav</span>: <b>' + (tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b>(' + this.points[i].point.change.toFixed(2) + '%)<br />';
                        //            break;
                        //        }
                        //    }
                        //}
                        //return s;


                        var tooltipDecimalPoint = 2;
                        var s = '';
                        var d = new Date(this.x);
                        var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
                        var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                        var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate() + ' ,' + d.getFullYear();
                        s = s + '<span style="color:#839557">' + navdate + '</span><br /><br />';
                        var ValueIndex = 2
                        var dataIndex = 0;
                        for (var i = 0; i < this.points.length; i++) {
                            if (this.points[i] != undefined) {
                                while (this.points[i].point.series.name != dataplot[dataIndex].name) {
                                    dataIndex++;
                                }
                            }
                            for (var k = 0; k < dataplot[dataIndex].data.length; k++) {
                                if (this.x === dataplot[dataIndex].data[k][0]) {
                                    if (this.points[i] != undefined) {
                                        var Perchange = 0;
                                        s = s + '<span style="color:#c5b47f">' + this.points[i].point.series.name + '</span>:<span>Nav</span>: <b>'
                                            + (dataplot[dataIndex].data[k][2].toString() == -1 ? "N/A" : dataplot[dataIndex].data[k][2].toString()) + '</b>(' + this.points[i].point.change.toFixed(2) + '%)<br />';
                                    }
                                    break;
                                }
                            }
                        }
                        return s;
                    }

                },
                credits: {
                    enabled: false
                },
                series: dataplot

            }
                , function (chart) {
                    setTimeout(function () {
                        $('input.highcharts-range-selector', $(chart.container).parent()).datepicker({
                            dateFormat: 'MM/dd/yyyy',
                            changeMonth: true,
                            changeYear: true,
                            maxDate: -2
                        });
                    }, 0);
                }
            );
            $(".highcharts-input-group").hide();
        }
    </script>
    <style>
        body {
            font-family: 'Raleway', sans-serif !important;
        }

        .panel-title {
            font-family: 'Raleway', sans-serif;
        }

        .border-bottom-1 {
            border: none;
            border-bottom: 1px solid #DDD;
        }

            .border-bottom-1:focus {
                border: none;
                border-bottom: 1px solid #DDD;
                box-shadow: none !important;
            }

        @media (min-width:1024px) {
            /*#txtiniAmount, #InitialAmt, #TotalInvs {
                position: relative;
                top: 13px;
            }*/

            /*.material hr {
                content: '';
                display: block;
                position: absolute;
                bottom: -14px;
                left: 0;
                margin: 0;
                padding: 0;
                width: 100%;
                height: 2px;
            }

            i.form-control-feedback {
                line-height: 48px;
            }*/
        }

        .loader-container {
            position: fixed;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;
            background: rgba(0,0,0,.5);
            z-index: 9;
            overflow: hidden;
        }

        .loader-content {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 9999;
            color: #fff;
            font-size: 30px;
        }

        @media (max-width:767px) {
            .bootstrap-select:not([class*="col-"]):not([class*="form-control"]):not(.input-group-btn) {
                width: 100%;
                margin-bottom: 10px;
            }

            .list-inline li {
                width: 100% !important;
                margin-bottom: 6px;
            }

            #demo-dp-range {
                margin-bottom: 15px;
            }

            .panel-title {
                font-weight: 600;
                padding: 0 6px 0 6px;
                font-size: 1em;
                line-height: 40px;
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }
        }

        @media screen and (max-width: 1024px) and (min-width: 768px) {
            .list-inline li:nth-child(4) {
                width: 37% !important;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="loader" class="loader-container">
            <div class="loader-content">
                Please Wait...
            </div>
        </div>
        <div id="container" class="effect aside-float aside-bright mainnav-lg">
            <div class="boxed">
                <div id="content-container">
                    <div id="page-content">
                        <div class="panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">Select</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" DataTextField="Nature" TabIndex="2"
                                            DataValueField="Nature" class="selectpicker" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlscheme" runat="server" class="selectpicker" Width="" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="SIPSchDt" runat="server" />
                        <asp:HiddenField ID="HdnbenchMarkId" runat="server" />
                        <asp:HiddenField ID="HiddenFieldName" runat="server" />
                        <div class="panel">
                            <!--Panel heading-->
                            <div class="panel-heading">
                                <div class="panel-control">
                                    <span class="">Lumpsum</span>
                                    <input id="demo-panel-w-switch" class="toggle-switch" type="checkbox" checked>
                                    <label for="demo-panel-w-switch"></label>
                                    <span class="">SIP</span>
                                </div>
                                <h3 class="panel-title">SIP</h3>
                            </div>

                            <!--Panel body-->
                            <div class="panel-body">
                                <ul class="sip list-inline">
                                    <li style="top: 1px; position: relative">An investment of</li>
                                    <li>
                                        <div class="form-group has-feedback">
                                            <i class="demo-pli-male fa fa-rupee form-control-feedback"></i>
                                            <fieldset class="material">
                                                <%--<input type="text" class="" name="end" placeholder="5,000 " style="padding-left:25px;"/>--%>
                                                <asp:TextBox ID="txtiniAmount" class="" placeholder="5,000" runat="server" Style="padding-left: 25px;" />
                                                <hr>
                                            </fieldset>
                                        </div>
                                    </li>
                                    <li>/
                                        <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="border-bottom-1">
                                            <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                            <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Monthly">Monthly</asp:ListItem>                                           
                                            <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                        </asp:DropDownList>
                                        from</li>
                                    <li style="width: 29%">
                                        <div id="demo-dp-range" style="top: 13px; position: relative">
                                            <div class="input-daterange input-group">
                                                <fieldset class="material">
                                                    <%--<input type="text" class="datepicker" name="start" />--%>
                                                    <asp:TextBox ID="FromDate" class="datepicker" runat="server" />
                                                    <hr style="bottom: 0">
                                                </fieldset>
                                                <span class="input-group-addon" style="top: -2px; position: relative">to</span>
                                                <fieldset class="material">
                                                    <%--<input type="text" class="datepicker" name="end" />--%>
                                                    <asp:TextBox ID="ToDate" class="datepicker" runat="server" />
                                                    <hr style="bottom: 0">
                                                </fieldset>
                                            </div>
                                        </div>
                                    </li>
                                    <li>total investment amount would be</li>
                                    <li>
                                        <div class="form-group">
                                            <fieldset class="material">
                                                <i class="fa fa-rupee form-control-feedback"></i>
                                                <%--<input type="text" placeholder="100000" id="demo-inline-inputmail" style="width: 100px;padding-left:25px;">--%>
                                                <asp:TextBox ID="InitialAmt" class="" runat="server" Style="padding-left: 25px;" />
                                                <hr>
                                            </fieldset>
                                        </div>
                                    </li>                                    
                                    <br class="hidden-xs hidden-sm" />
                                    <br class="hidden-xs hidden-sm" />
                                    <li>and the investment value would have been
                                    </li>
                                    <li>
                                        <fieldset class="material">
                                            <i class="fa fa-rupee form-control-feedback"></i>
                                            <%--<input type="text" placeholder="10,000" id="demo-inline-inputmail" style="padding-left:25px;">--%>
                                            <asp:TextBox ID="TotalInvs" class="" runat="server" Style="padding-left: 25px;" />
                                            <hr>
                                        </fieldset>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="panel">
                            <div class="panel-body" style="padding: 0">
                                <!--Default Tabs (Left Aligned)-->
                                <!--===================================================-->
                                <div class="tabbable-panel" style="padding: 0">
                                    <div class="tabbable-line">
                                        <ul class="nav nav-tabs ">
                                            <li class="active">
                                                <a href="#tab_default_1" data-toggle="tab" style="padding: 0">
                                                    <h3 class="panel-title">Graph</h3>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="#tab_default_2" data-toggle="tab" style="padding: 0">
                                                    <h3 class="panel-title">Performance in Standard Format </h3>
                                                </a>
                                            </li>
                                        </ul>
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="tab_default_1">
                                                <div class="table-responsive">
                                                    <div id="HighContainer" style="height: 600px;margin:0 auto; min-width: 95%; max-width: 95%" runat="server"></div>
                                                </div>
                                                <%--<br>
                                                <p style="margin-left: 10px; margin-top: 10px; font-size: 12px; font-weight: 300;">CAGR / Simple annualized / absolute depending on type of scheme and time period</p>--%>
                                            </div>
                                            <div class="tab-pane" id="tab_default_2" runat="server">                                               
                                                <p style="margin-left: 10px; margin-top: 10px; font-size: 12px; font-weight: 300;">Report as on date
                                                    <asp:Label ID="lblReturnValueDate" runat="server"></asp:Label>
                                                </p>
                                                <br />
                                                <div class="table table-responsive">
                                                    <input type="hidden" id="hdnNABenchmark" runat="server" value="0" />
                                                    <asp:Repeater ID="RptCommonSipResult" runat="server" Visible="false">
                                                        <HeaderTemplate>
                                                            <table cellspacing="0" cellpadding="0" class="table table-bordered">
                                                                <tr class="gridheader">
                                                                    <th colspan="8" align="center">
                                                                        <asp:Label ID="lblReturnSch" runat="server"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr class="gridheader">
                                                                    <th align="left">Particulars
                                                                    </th>
                                                                    <th align="right">Total Amount Invested (<i class="fa fa-rupee"></i>)
                                                                    </th>
                                                                    <th align="right">Scheme Returns Yield (%)
                                                                    </th>
                                                                    <th align="right">Scheme Market Value (<i class="fa fa-rupee"></i>)
                                                                    </th>
                                                                    <th align="right">Bechmark (<asp:Label ID="lblReturnIndex" runat="server"></asp:Label>) Returns Yield (%)
                                                                    </th>
                                                                    <th align="right">Benchmark Market Value (<i class="fa fa-rupee"></i>)
                                                                    </th>
                                                                    <th align="right" class="NABenchmark">Additional Bechmark (<asp:Label ID="lblReturnAddIndex" runat="server"></asp:Label>) Return Yield (%)
                                                                    </th>
                                                                    <th align="right" class="NABenchmark">Additional Benchmark Market Value (<i class="fa fa-rupee"></i>)
                                                                    </th>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr style="border-width: 1px; border-style: solid; height: 30px;">
                                                                <td align="left">
                                                                    <%#Eval("Particulars") %>
                                                                </td>
                                                                <td align="right">
                                                                    <%#Eval("Total_Amount_Invest") %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Return_Yield")),2) %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Math.Round(Convert.ToDouble(Eval("Scheme_Market_value"))) %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("Bechmark_return_yield") == System.DBNull.Value ? "--" : Math.Round(Convert.ToDecimal(Eval("Bechmark_return_yield")),2).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("Benchmark_Market_value") == System.DBNull.Value ? "--" : Math.Round(Convert.ToDouble(Eval("Benchmark_Market_value"))).ToString() %>
                                                                </td>
                                                                <td align="right" class="NABenchmark">
                                                                    <%# Eval("Additional_Bechmark_return_yield") == System.DBNull.Value ? "--" : Math.Round(Convert.ToDecimal(Eval("Additional_Bechmark_return_yield")),2).ToString() %>
                                                                </td>
                                                                <td align="right" class="NABenchmark">
                                                                    <%# Eval("Additional_Benchmark_Market_value") == System.DBNull.Value ? "--" : Math.Round(Convert.ToDouble(Eval("Additional_Benchmark_Market_value"))).ToString() %>
                                                                </td>

                                                            </tr>
                                                        </ItemTemplate>

                                                        <AlternatingItemTemplate>
                                                            <tr style="border-width: 1px; border-style: solid; height: 30px;">
                                                                <td align="left">
                                                                    <%#Eval("Particulars") %>
                                                                </td>
                                                                <td align="right">
                                                                    <%#Eval("Total_Amount_Invest") %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Return_Yield")),2) %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Math.Round(Convert.ToDouble(Eval("Scheme_Market_value"))) %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("Bechmark_return_yield") == System.DBNull.Value ? "--" : Math.Round(Convert.ToDecimal(Eval("Bechmark_return_yield")),2).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("Benchmark_Market_value") == System.DBNull.Value ? "--" : Math.Round(Convert.ToDouble(Eval("Benchmark_Market_value"))).ToString() %>
                                                                </td>
                                                                <td align="right" class="NABenchmark">
                                                                    <%# Eval("Additional_Bechmark_return_yield") == System.DBNull.Value ? "--" : Math.Round(Convert.ToDecimal(Eval("Additional_Bechmark_return_yield")),2).ToString() %>
                                                                </td>
                                                                <td align="right" class="NABenchmark">
                                                                    <%# Eval("Additional_Benchmark_Market_value") == System.DBNull.Value ? "--" :  Math.Round(Convert.ToDouble(Eval("Additional_Benchmark_Market_value"))).ToString() %>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>

                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                                <div class="col-md-12" style="padding-bottom:15px;">
                                                    Assuming Rs. 10,000 invested systematically on the first Business Day of every month over a period of time. CAGR returns are computed after accounting for the cash flow by using XIRR method (investment internal rate of return) for <span id="txtPlan" runat="server"></span> Plan - Growth Option. The above investment simulation is for illustrative purposes only and should not be construed as a promise on minimum returns and safeguard of capital.
                                                    <br /><br />
                                                    <b>Past performance may or may not be sustained in the future.</b> Load is not taken into consideration for computation of performance.
                                                </div>
                                            </div>

                                             <div class="col-md-12 text-right" style="padding-bottom:9px;">
                                                -Powered by <a href="https://www.icraonline.com/" target="_blank">ICRA Analytics Ltd.</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--  <footer id="footer">
                <div class="show-fixed pull-right">
                    You have <a href="#" class="text-bold text-main"><span class="label label-danger">3</span> pending action.</a>
                </div>
                <p class="pad-lft"></p>
            </footer>--%>
            <button class="scroll-top btn">
                <i class="pci-chevron chevron-up"></i>
            </button>
        </div>
        <input id="HdSchemes" type="hidden" runat="server" />
        <input id="HdToData" type="hidden" runat="server" />
        <input id="HdFromData" type="hidden" runat="server" />
    </form>
</body>
</html>
