<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HDFCLumpSumCalc.aspx.cs" Inherits="iFrames.HDFC_SIP.HDFCLumpSumCalc" %>

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
        var chartHeaders = {};
        function GetMinDate() {

            var ddScheme = document.getElementById('SIPSchDt');
            var allotdate = ddScheme.value;
            var Day = parseInt(allotdate.substring(0, 2), 10);
            var Mn = parseInt(allotdate.substring(3, 5), 10);
            var Yr = parseInt(allotdate.substring(6, 10), 10);
            //var DateVal = Mn + "/" + Day + "/" + Yr;
            var DateVal = Mn + "/" + Day + "/" + Yr;
            dt = new Date(DateVal);

            return dt;
            //return ddScheme.value;
        }
        function SplitDate(val) {
            return (val.getMonth() + 1) + '/' + val.getDate() + '/' + val.getFullYear();
        }

        $(function () {

            var hdnIsoverNight = $("#hdnIsoverNight").val();
            $(".non-over-night").show();
            $(".over-night").hide();
            if (hdnIsoverNight == "1") {
                $(".over-night").show();
                $(".non-over-night").hide();
                chartHeaders = {
                    buttons: [
                        {
                            type: 'day',
                            count: 7,
                            text: '7d'
                        },
                        {
                            type: 'day',
                            count: 15,
                            text: '15d'
                        },
                        {
                            type: 'month',
                            count: 1,
                            text: '1m'
                        },
                        //{
                        //    type: 'month',
                        //    count: 3,
                        //    text: '3m'
                        //}, {
                        //    type: 'month',
                        //    count: 6,
                        //    text: '6m'
                        //}, {
                        //    type: 'ytd',
                        //    text: 'YTD'
                        //},
                        {
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
                            type: 'all',
                            text: 'All'
                        }],
                    selected: 6
                }
            }
            else {
                $(".non-over-night").show();
                $(".over-night").hide();
                chartHeaders = {
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
                }
            }

            var year = new Date().getFullYear().toString();
            var schemeLaunchDate = GetMinDate().getFullYear().toString();
            $("#FromDate").datepicker({
                // dateFormat: 'dd/MMM/yyyy',
                changeMonth: true,
                changeYear: true,
                //maxDate: 0,
                minDate: GetMinDate(),
                yearRange: schemeLaunchDate + ":" + year

            });
            $("#ToDate").datepicker({
                //dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                //maxDate: -1,
                minDate: GetMinDate(),
                yearRange: schemeLaunchDate + ":" + year

            });
            // $("#ddlscheme").change(function () {
            //   btnPlotclick();
            //  });
            //btnPlotclick();

            $('#ddlscheme').change(function () { $("#loader").show(); });
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
            $('#demo-panel-w-switch').change(function () { $("#loader").show(); window.location.href = "/HDFC_SIP/HDFCSipCalc.aspx?SchemeId=" + $('#<%=ddlscheme.ClientID %>').val() + "&NatureId=" + $('#<%=ddlNature.ClientID %>').val() + ""; });

        });
        $(document).ready(function () {
            $("#loader").show();
            //CalculateSip();
            btnPlotclick();
            //loader();
        });

        function CalculateSip() {

            //$("body").fadeIn();
            //pop();
            $("#loader").show();
            var NatureId = $('#<%=ddlNature.ClientID %>').val();
            var schIndId = $('#<%=ddlscheme.ClientID %>').val();
            var frmDate = $('#<%=FromDate.ClientID %>').val();
            var toData = $('#<%=ToDate.ClientID %>').val();
            var Amount = $('#<%=txtiniAmount.ClientID %>').val();
            if ((NatureId == 0) || (NatureId == "") || (NatureId == undefined)) {
                NatureId = "";
            }
            if ((schIndId == 0) || (schIndId == "") || (schIndId == undefined)) {
                //alert('Select Scheme!!!');
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
            if ((Amount < 10000)) {
                $("#loader").hide();
                alert('Amount should be equal or grater than 10000 !!');

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
            if (new Date(frmDate) > new Date()) {
                alert('From date can not grater than current date !!!');
                $('#<%=FromDate.ClientID %>').val('');
                $("#loader").hide();
                return;
            }

            if (new Date(frmDate) >= new Date(toData)) {
                alert('From date can not grater than  or equal To date!!!');
                $('#<%=FromDate.ClientID %>').val('');
                $("#loader").hide();
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
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "', Amount:'" + Amount + "', NatureId:'" + NatureId + "'}";
            $.ajax({
                type: "POST",
                url: "HDFCLumpSumCalc.aspx/getCalculatedate",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    //  var value = msg.d.split('#');
                    $('#<%=InitialAmt.ClientID %>').val(msg.d);
                    $("#loader").hide();
                    btnPlotclick();
                    //  debugger;

                },
                error: function (msg) {
                    alert("Error! Try again...");
                    $("#loader").hide();
                }
            });
        }
        function btnPlotclick() {
            var schIndId = $('#<%=ddlscheme.ClientID %>').val();
            //var schIndId = $('#<%=HdSchemes.ClientID %>').val();
            var frmDate = $('#<%=FromDate.ClientID %>').val();
            var toData = $('#<%=ToDate.ClientID %>').val();
            var Amount = $('#<%=txtiniAmount.ClientID %>').val();
            //var frmDate = $('#<%=HdFromData.ClientID %>').val();
           // var toData = $('#<%=HdToData.ClientID %>').val();
            if ((schIndId == 0) || (schIndId == "") || (schIndId == undefined)) {
                $("#loader").hide();
                return;
            }
            if ((schIndId == 0) || (schIndId == "") || (schIndId == undefined)) {
                alert('Select Scheme!!!');
                $(".loader-container").hide();
                return;
            }
            if ((Amount == 0) || (Amount == "") || (Amount == undefined)) {
                $("#loader").hide();
                alert('Please Give Amount!!!');
                return;
            }
            if (Amount != "" && Amount != null) {
                if (!$.isNumeric(Amount)) {
                    alert('Please Give Proper Amount!!!');
                    $("#loader").hide();
                    return;
                }
            }
            if ((Amount < 10000)) {
                alert('Amount should be equal or greater than 10000 !!');
                $("#loader").hide();
                $('#<%=txtiniAmount.ClientID %>').val(10000);
                return;
            }
            if ((frmDate == "") || (frmDate == undefined)) {

                alert('Select from Date!!!');
                $("#loader").hide();
                return;
            }
            if ((toData == "") || (toData == undefined)) {

                alert('Select To Data!!!');
                $("#loader").hide();
                return;
            }
            if (new Date(frmDate) > new Date(toData)) {
                alert('From date can not greater than To date!!!');
                $("#loader").hide();
                return;
            }
            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "', Amount:'" + Amount + "'}";
            $.ajax({
                type: "POST",
                url: "HDFCLumpSumCalc.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    // setChart(msg.d);
                    $("#loader").hide();
                    $('#<%=InitialAmt.ClientID %>').val(msg.d.InvestedValue);
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
                rangeSelector: chartHeaders,


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
                        //            var Perchange = 0;
                        //            s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>:<span>Amount:<b>'
                        //                + (tt[i].data[k][3].toString() == 0 ? "N/A" : tt[i].data[k][3].toString()) + ',</b></span><span>Nav</span>: <b>'
                        //                + (tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b>(' + this.points[i].point.change.toFixed(2) + '%)<br />';
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
                                        s = s + '<span style="color:#c5b47f">' + this.points[i].point.series.name + '</span>:<span>Amount:<b>'
                                            + (dataplot[dataIndex].data[k][3].toString() == 0 ? "N/A" : dataplot[dataIndex].data[k][3].toString()) + ',</b></span><span>Nav</span>: <b>'
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
                            dateFormat: 'yy-mm-dd',
                            changeMonth: false,
                            changeYear: false,
                            maxDate: -2,
                            inputEnabled:false
                        });
                    }, 0);
                }
            );

            $(".highcharts-input-group").hide();
        }

        // #region
        //PlotAuto Code Off
        //function PlotAuto(dataConsolidated, ImageArray) {

        //    var max = dataConsolidated.MaxDate;
        //    var min = dataConsolidated.MinDate;
        //    var data = dataConsolidated.SimpleNavReturnModel;
        //    var tt = [[]];
        //    for (var i = 0; i < data.length; i += 1) {

        //        var tt1 = {};
        //        tt1.name = data[i].Name;

        //        var points = [];
        //        for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
        //            var res = data[i].ValueAndDate[j].Date.split("-");

        //            points.push([Date.UTC(res[0], res[1] - 1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue, data[i].ValueAndDate[j].InvestAmount]);

        //        }
        //        tt1.data = points;
        //        tt.push(tt1);
        //    }
        //    tt.shift();
        //    //  debugger;
        //    var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

        //    var colorarray = Array();
        //    for (var i = 0; i < ImageArray.length; i += 1) {
        //        colorarray.push(CustomSeriesColors[ImageArray[i]]);
        //    }

        //    Highcharts.setOptions({
        //        useUTC: false
        //    });

        //    Highcharts.stockChart('HighContainer', {

        //        legend: {
        //            enabled: true,
        //            symbolWidth: 40

        //        },
        //        rangeSelector: chartHeaders,


        //        yAxis: {
        //            title: {
        //                min: 0,
        //                text: 'Value',
        //                style: {
        //                    fontWeight: 'bold',
        //                    color: 'black',
        //                    fontSize: "15px"
        //                }
        //            },
        //            labels: {
        //                formatter: function () {
        //                    return (this.value > 0 ? ' + ' : '') + this.value + '%';
        //                }
        //            },
        //            plotLines: [{
        //                value: 0,
        //                width: 2,
        //                color: 'silver'
        //            }]
        //        },

        //        plotOptions: {
        //            series: {
        //                compare: 'percent',
        //                showInNavigator: true
        //            }
        //        },

        //        tooltip: {
        //            shared: true,
        //            backgroundColor: '#FCFFC5',

        //            formatter: function () {
        //                var s = '';

        //                var d = new Date(this.x);

        //                var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        //                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        //                var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate() + ' ,' + d.getFullYear();

        //                s = s + '<span style="color:#839557">' + navdate + '</span><br /><br />';

        //                for (var i = 0; i < tt.length; i++) {

        //                    for (var k = 0; k < tt[i].data.length; k++) {
        //                        if (this.x === tt[i].data[k][0]) {
        //                            var Perchange = 0;

        //                            s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>:<span>Amount:<b>' + (tt[i].data[k][3].toString() == 0 ? "N/A" : tt[i].data[k][3].toString()) + ',</b></span><span>Nav</span>: <b>' + (tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b>(' + this.points[i].point.change.toFixed(2) + '%)<br />';
        //                            break;
        //                        }
        //                    }

        //                }

        //                return s;

        //            }

        //        },
        //        credits: {
        //            enabled: false
        //        },
        //        series: tt

        //    }
        //        , function (chart) {
        //            setTimeout(function () {
        //                //alert('qweiu');
        //                //$('.highcharts-button highcharts-button-disabled').toggleClass('highcharts-button highcharts-button-normal');
        //                $('.highcharts-button highcharts-button-disabled').addClass('highcharts-button highcharts-button-normal').removeClass('highcharts-button highcharts-button-disabled');
        //                $('input.highcharts-range-selector', $(chart.container).parent()).datepicker({
        //                    dateFormat: 'yy-mm-dd',
        //                    changeMonth: false,
        //                    changeYear: false,
        //                    maxDate: -2,
        //                    inputEnabled: false
        //                });
        //            }, 0);
        //        }
        //    );

        //    $(".highcharts-input-group").hide();
        //}
        // #end region

    </script>
    <style>
        body {
            font-family: 'Raleway', sans-serif !important;
        }

        .panel-title {
            font-family: 'Raleway', sans-serif;
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
                                <div class="panel-control" id="rdSwitchPanel" runat="server">
                                    <span class="">Lumpsum</span>
                                    <input id="demo-panel-w-switch" class="toggle-switch" type="checkbox">
                                    <label for="demo-panel-w-switch"></label>
                                    <span class="">SIP</span>
                                </div>
                                <h3 class="panel-title">Lumpsum</h3>
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
                                    <li>from</li>
                                    <li style="width: 30%">
                                        <div id="demo-dp-range" style="top: 13px; position: relative">
                                            <div class="input-daterange input-group">
                                                <fieldset class="material">
                                                    <%--<input type="text" class="datepicker" name="start" />--%>
                                                    <asp:TextBox ID="FromDate" class="datepicker" runat="server" />
                                                    <hr />
                                                </fieldset>
                                                <span class="input-group-addon" style="top: -2px; position: relative">to</span>
                                                <fieldset class="material">
                                                    <%--<input type="text" class="datepicker" name="end" />--%>
                                                    <asp:TextBox ID="ToDate" class="datepicker" runat="server" />
                                                    <hr />
                                                </fieldset>
                                            </div>
                                        </div>
                                    </li>
                                    <li>investment value would have been</li>
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

                                </ul>
                            </div>
                        </div>
                        <div class="row">
                            <div class="panel-body" style="padding: 15px 15px 25px;">
                                <div class="tabbable-panel" style="padding: 0;">
                                    <div class="tabbable-line">
                                        <ul class="nav nav-tabs ">
                                            <li class="active">
                                                <a href="#tab_default_1" data-toggle="tab" style="padding: 0">
                                                    <h3 class="panel-title">Graph</h3>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="#tab_default_2" data-toggle="tab" style="padding: 0">
                                                    <h3 class="panel-title">Performance in Standard Format</h3>
                                                </a>
                                            </li>
                                        </ul>
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="tab_default_1" align="left">
                                                <div id="HighContainer" style="height: 600px; min-width: 310px; max-width: 1000px" runat="server"></div>
                                                <br />

                                            </div>
                                            <div class="tab-pane" id="tab_default_2" runat="server">
                                                <div class="col-md-9" style="padding-left: 0;"><small style="margin-left: 10px; margin-top: 10px;" id="txtPerformanceTitle" runat="server"></small></div>
                                                <div class="col-md-3 text-right"><small style="margin-left: 10px; margin-top: 10px;"> As of <span id="lblDetailsDate" runat="server"></span></small></div>
                                                <div class="table table-responsive">
                                                    <asp:Repeater ID="RptCommonSipResult" runat="server" Visible="true">
                                                        <HeaderTemplate>
                                                            <table class="table table-bordered">
                                                                <thead>
                                                                    <tr>
                                                                        <th rowspan="2">Scheme / Benchmark</th>
                                                                        <th colspan="2" align="center" style="text-align: center;" class="over-night">7D</th>
                                                                        <th colspan="2" align="center" style="text-align: center;" class="over-night">15D</th>
                                                                        <th colspan="2" align="center" style="text-align: center;" class="over-night">1M</th>
                                                                        <th colspan="2" align="center" style="text-align: center;" class="non-over-night">3M</th>
                                                                        <th colspan="2" align="center" style="text-align: center" class="non-over-night">6M</th>
                                                                        <th colspan="2" align="center" style="text-align: center">1Y</th>
                                                                        <th colspan="2" align="center" style="text-align: center">3Y</th>
                                                                        <th colspan="2" align="center" style="text-align: center">5Y</th>
                                                                        <th id="trTen" colspan="2" align="center" style="text-align: center;" runat="server" class="non-over-night">10Y</th>
                                                                        <th colspan="2" align="center" style="text-align: center" class="non-over-night">15Y</th>
                                                                        <th colspan="2" align="center" style="text-align: center">Since Inception</th>

                                                                    </tr>
                                                                    <tr>
                                                                        <th colspan="1" style="text-align: right;" class="over-night">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="over-night">Amount<br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right;" class="over-night">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="over-night">Amount<br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                         <th colspan="1" style="text-align: right;" class="over-night">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="over-night">Amount<br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right;" class="non-over-night">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="non-over-night">Amount<br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="non-over-night">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="non-over-night">Amount<br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right">Amount
                                                                            <br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right">Amount
                                                                            <br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right;">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right">Amount
                                                                            <br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right;" class="non-over-night">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="non-over-night">Amount
                                                                            <br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right;" class="non-over-night">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right" class="non-over-night">Amount
                                                                            <br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right;">Return (%)
                                                                        </th>
                                                                        <th colspan="1" style="text-align: right">Amount
                                                                            <br>
                                                                            (<i class="fa fa-rupee"></i>)
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr style="border-width: 1px; border-style: solid; height: 30px;">
                                                                <td>
                                                                    <%#Eval("Scheme_Index") %>
                                                                </td>
                                                                <td align="right" class="over-night">
                                                                    <%# Eval("7DReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("7DReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="over-night">
                                                                    <%# Eval("7DAmount")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("7DAmount")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="over-night">
                                                                    <%# Eval("15DReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("15DReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="over-night">
                                                                    <%# Eval("15DAmount")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("15DAmount")),2)).ToString() %>
                                                                </td>

                                                                <td align="right" class="over-night">
                                                                    <%# Eval("15DReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("1MReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="over-night">
                                                                    <%# Eval("15DAmount")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("1MAmount")),2)).ToString() %>
                                                                </td>


                                                                <td align="right" class="non-over-night">
                                                                    <%# Eval("3MReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("3MReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="non-over-night">
                                                                    <%# Eval("3MAmount")=="--"?"--" :(Math.Round(Convert.ToDouble(Eval("3MAmount")))).ToString() %>
                                                                </td>
                                                                <td align="right" class="non-over-night">
                                                                    <%# Eval("6MReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("6MReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="non-over-night">
                                                                    <%# Eval("6MAmount")=="--"?"--" :(Math.Round(Convert.ToDouble(Eval("6MAmount")))).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("1YReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("1YReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%#Eval("1YAmount")=="--"?"--" : (Math.Round(Convert.ToDouble(Eval("1YAmount")))).ToString() %>
                                                                </td>

                                                                <td align="right">
                                                                    <%# Eval("3YReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("3YReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("3YAmount")=="--"?"--" :(Math.Round(Convert.ToDouble(Eval("3YAmount")))).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("5YReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("5YReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("5YAmount")=="--"?"--" :(Math.Round(Convert.ToDouble(Eval("5YAmount")))).ToString() %>
                                                                </td>
                                                                <td align="right" class="non-over-night">
                                                                    <%# Eval("10YReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("10YReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="non-over-night">
                                                                    <%# Eval("10YAmount")=="--"?"--" :(Math.Round(Convert.ToDouble(Eval("10YAmount")))).ToString() %>
                                                                </td>
                                                                <td align="right" class="non-over-night">
                                                                    <%#  Eval("15YReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("15YReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right" class="non-over-night">
                                                                    <%# Eval("15YAmount")=="--"?"--" :(Math.Round(Convert.ToDouble(Eval("15YAmount")))).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("SIReturn")=="--"?"--" :(Math.Round(Convert.ToDecimal(Eval("SIReturn")),2)).ToString() %>
                                                                </td>
                                                                <td align="right">
                                                                    <%# Eval("SIAmount")=="--"?"--" :(Math.Round(Convert.ToDouble(Eval("SIAmount")))).ToString() %>
                                                                </td>


                                                            </tr>
                                                        </ItemTemplate>


                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>

                                                    <div class="col-md-12" id="divClientDisclaimer" runat="server"></div>
                                                </div>
                                            </div>

                                            <div class="col-md-12 text-right" style="padding-bottom: 9px;">
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
            <%-- <footer id="footer">
                <div class="show-fixed pull-right">
                    You have <a href="#" class="text-bold text-main"><span class="label label-danger">3</span> pending action.</a>
                </div>
                <p class="pad-lft">&#0169; 2017 Your Company</p>
            </footer>--%>
            <button class="scroll-top btn">
                <i class="pci-chevron chevron-up"></i>
            </button>
        </div>
        <input id="HdSchemes" type="hidden" runat="server" />
        <input id="HdToData" type="hidden" runat="server" />
        <input id="HdFromData" type="hidden" runat="server" />
        <input id="hdnIsoverNight" type="hidden" runat="server" />
        <input id="AddlBenchmarkVal" type="hidden" runat="server" />
    </form>
</body>
</html>
