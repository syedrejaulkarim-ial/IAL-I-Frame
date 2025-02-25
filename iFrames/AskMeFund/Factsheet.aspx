<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Factsheet.aspx.cs" Inherits="iFrames.AskMeFund.Factsheet" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Factsheet</title>
    <!-- App favicon -->
    <link rel="shortcut icon" href="assets/images/favicon.ico" />
    <!-- nouisliderribute css -->
    <link rel="stylesheet" href="assets/libs/nouislider/nouislider.min.css" />

    <!-- Bootstrap Css -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- Icons Css -->
    <link href="assets/css/icons.min.css" rel="stylesheet" type="text/css" />
    <!-- App Css-->
    <link href="assets/css/app.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/custom.css" rel="stylesheet" />

    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <script src="js/moment.min.js"></script>

    <script src="../Scripts/HighStockChart/highstock.js" type="text/javascript"></script>
    <script src="../Scripts/HighStockChart/exporting.js" type="text/javascript"></script>

    <script src="../Scripts/jqplot/jquery.jqplot.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.cursor.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.dateAxisRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.highlighter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.canvasTextRenderer.min.js"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.canvasAxisLabelRenderer.min.js"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.pointLabels.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.pieRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.barRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.categoryAxisRenderer.min.js" type="text/javascript"></script>
    <link href="../Styles/jquery.jqplot.css" rel="stylesheet" type="text/css" />


    <script>
        $(function () {

            getGraphs($("#hdSchemeId").val());

            GetChart($("#hdSchemeId").val());

        });

        function GetChart(schid) {

            
            var dataToPush = JSON.stringify({
                SchId: schid
            });

            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                url: 'Factsheet.aspx/McapAndAvgMat',
                type: 'POST',
                asynchronus: true,
                contentType: "application/json; charset=utf-8",
                success: function (dataConsolidated) {
                    
                    if (dataConsolidated.d == null) {
                        return;
                    }
                    if (dataConsolidated.Value == 0) {
                        return;
                    }
                    if (dataConsolidated.d.IsMCap == true) {
                        $("#TdMc").html("Market Capitalisation");
                        MarketCapHigh(dataConsolidated.d);
                    }
                    else {
                        $("#TdMc").html("Average Maturity");

                        AverageMaturityHigh(dataConsolidated.d);
                    }
                },
                error: function (data, e) {
                    alert("Something went wrong..!!");
                }
            });
        }

        function MarketCapHigh(dataAll) {
            var data = dataAll.LstMarketCap;
            if (data == null || data.length < 1) {
                $('#dvMarketCapContaner').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="dvMarketCap">Data not available for the selected scheme</div>');
                $('#dvMarketCapContaner').effect("highlight", {}, 3000);
                return;
            }
            $('#dvMarketCapContaner').append('<div style="width: 97%; height:100%;" id="dvMarketCap" ></div>');

            var tt = [];
            var nature = [];
            var tt1 = {};
            tt1.name = "Mcap";
            tt1.data = [];
            for (var i = 0; i < data.length; i += 1) {

                var ttobj = {
                    name: data[i].Market_Slap,
                    y: data[i].M_Cap
                };
                tt1.data.push(ttobj);

                //tt1.name = data[i].M_Cap;
                ////var datatoPush = [];
                ////nature.push(data[i].Market_Slap);
                //tt1.y = data[i].Market_Slap;
                //tt.push(tt1);
            }
            tt.push(tt1);
            //tt.shift();

            //var myJsonString = JSON.stringify(tt);
            //var jsonArray = JSON.parse(JSON.stringify(tt));

            //var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];
            var CustomSeriesColors = ["#3cb6ff", "#67daf5", "#fcd7bc", "#bee6c4", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

            Highcharts.setOptions({
                colors: CustomSeriesColors
            });

            var CurrChart = Highcharts.chart('dvMarketCapContaner', {
                chart: {
                    type: 'pie',
                    height: 260,
                    //spacingTop: -10,
                    options3d: {
                        enabled: true,
                        alpha: 45,
                        beta: 0
                    }
                },
                title: {
                    text: ''
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                credits: {
                    text: 'Source: MFI360'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        size: 140,
                        cursor: 'pointer',
                        depth: 25,
                        dataLabels: {
                            enabled: true,
                            distance: 8,
                            format: '{point.name}<br/> {point.percentage:.1f}%',
                            style: {
                                fontWeight: 'normal'
                            },
                        },
                        showInLegend: true
                    }
                },
                legend: {
                    symbolRadius: 0,
                    y: 15,
                },
                series: tt
            });

            CurrChart.reflow();
        }

        function AverageMaturityHigh(dataAll) {
            data = dataAll.LstAverageMaturity;
            // $('#dvAvgMaturity').remove();
            if (data == null || data.length < 1) {
                $('#dvMarketCapContaner').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="dvAvgMaturity">Data not available for the selected scheme</div>');
                $('#dvMarketCapContaner').effect("highlight", {}, 3000);
                return;
            }
            $('#dvMarketCapContaner').append('<div style="width: 97%; height:100%;" id="dvAvgMaturity" ></div>');

            //$.jqplot.config.enablePlugins = true;
            var dataPlot = [];
            var ticks = [];
            var dataToInsert = {};
            dataToInsert.name = "Average Maturity(in Years)";
            dataToInsert.data = [];

            for (var i = 0; i < data.length; i += 1) {
                dataToInsert.data.push(data[i].Average_Maturity);
                ticks.push(data[i].MonthYear);
            }
            dataPlot.push(dataToInsert);

            var CustomSeriesColors = ["#4f8ad3"];

            Highcharts.setOptions({
                colors: CustomSeriesColors
            });

            var CurrChart = Highcharts.chart('dvMarketCapContaner', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: ''
                },
                credits: {
                    text: 'Source: MFI360'
                },
                legend: {
                    enabled: false,
                    symbolRadius: 0,
                    symbolHeight: 0,
                },
                xAxis: {
                    categories: ticks
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Average Maturity (in Years)',
                        align: 'middle',
                        style: {
                            color: 'black',
                            fontSize: "15px"
                        }
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series: dataPlot
            });

            CurrChart.reflow();
        }


        function getGraphs(schid) {
            btnPlotclick();

            PullChartAssetAllocation(schid);
            PlotMktValue(schid);
        }

        function PullChartAssetAllocation(schid) {
            var dataToPush = JSON.stringify({
                schemeIds: schid
            });
            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                url: 'Factsheet.aspx/assetAllocaton',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                success: function (dataConsolidated) {
                    if (dataConsolidated.d == null) {
                        return;
                    }
                    if (dataConsolidated.Value == 0) {
                        return;
                    }
                    PlotAssetAllocation(dataConsolidated.d);
                },
                error: function (data) {
                    alert(data);
                }
            });
        }

        function PlotAssetAllocation(data) {
            if (data == null || data.length < 1) {
                $('#chartAssetAllocationContainer').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="chartAssetAllocation">Data not available for the selected scheme</div>');
                $('#chartAssetAllocationContainer').effect("highlight", {}, 3000);
                return;
            }

            var dataPlot = [[]];
            for (var i = 0; i < data.length; i += 1) {
                var TextVal = [];
                TextVal.push(data[i].NatureName);
                TextVal.push(data[i].Value);
                dataPlot.push(TextVal);
            }

            dataPlot.shift();
            var plot1 = $.jqplot('chartAssetAllocationContainer', [dataPlot], {
                //seriesColors: ["#da251d", "#ed9c54", "#605d5c", "#e6ab0d", "#49b959"],
                seriesColors: ["#ff88ae", "#bee6c4", "#605d5c", "#e6ab0d", "#49b959"],
                seriesDefaults: {
                    renderer: jQuery.jqplot.PieRenderer,
                    rendererOptions: {
                        showDataLabels: true,
                        dataLabelFormatString: '%.2f%%',
                        dataLabels: 'value',
                        dataLabelThreshold: 0,
                        dataLabelPositionFactor: 1.19,
                    }
                },
                grid: {
                    //drawGridLines: false,        // wether to draw lines across the grid or not.
                        //gridLineColor: '#cccccc',   // CSS color spec of the grid lines.
                        background: 'transparent',      // CSS color spec for background color of grid.
                        //borderColor: '#999999',     // CSS color spec for border around grid.
                        //borderWidth: 2.0,           // pixel width of border around grid.
                        //shadow: true,               // draw a shadow for grid.
                        //shadowAngle: 45,            // angle of the shadow.  Clockwise from x axis.
                        //shadowOffset: 1.5,          // offset from the line of the shadow.
                        //shadowWidth: 3,             // width of the stroke for the shadow.
                        //shadowDepth: 3
                        borderColor: 'transparent', shadow: false, drawBorder: false, shadowColor: 'transparent'
                }, 
                legend: {
                    show: true,
                    location: 'w',
                    fontFamily: 'Helvetica,Arial,sans-serif',
                    fontSize: '12px'
                    //labels: ['Equity', 'Debt', 'Others']
                }
            });
        }

        function PlotMktValue(schid) {
            var dataToPush = JSON.stringify({
                schemeIds: schid
            });
            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                url: 'Factsheet.aspx/portfolioMKT_Val',
                type: 'POST',
                asynchronus: true,
                contentType: "application/json; charset=utf-8",
                success: function (dataConsolidated) {
                    if (dataConsolidated.d == null) {
                        return;
                    }
                    if (dataConsolidated.Value == 0) {
                        return;
                    }
                    
                    PlotMktChart(dataConsolidated.d);
                },
                error: function (data, e) {
                    alert("Something went wrong...!!");
                }
            });
        }

        function PlotMktChart(data) {
            var seriesNames = Array();
            var maxval = 0;
            var dataPlot = [];
            for (var i = 0; i < data.length; i += 1) {
                seriesNames.push(data[i].PortDate);
                dataPlot.push([data[i].PortDate, data[i].MatketValue]);
                if (maxval < data[i].MatketValue)
                    maxval = data[i].MatketValue;
            }
            maxval = maxval * .2 + maxval;

            var plot1 = $.jqplot('chart1', [dataPlot], {
                seriesColors: ["#4f8ad3"],
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    rendererOptions: { barMargin: 50 },
                    pointLabels: { show: true, stackedValue: true }

                },
                grid: {
                    //drawGridLines: true,        // wether to draw lines across the grid or not.
                        //gridLineColor: '#cccccc',   // CSS color spec of the grid lines.
                        background: 'transparent',      // CSS color spec for background color of grid.
                        //borderColor: '#999999',     // CSS color spec for border around grid.
                        //borderWidth: 2.0,           // pixel width of border around grid.
                        //shadow: true,               // draw a shadow for grid.
                        //shadowAngle: 45,            // angle of the shadow.  Clockwise from x axis.
                        //shadowOffset: 1.5,          // offset from the line of the shadow.
                        //shadowWidth: 3,             // width of the stroke for the shadow.
                        //shadowDepth: 3
                        borderColor: 'transparent', shadow: false, drawBorder: false, shadowColor: 'transparent'
                }, 
                axesDefaults: {
                    tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                    tickOptions: {
                        angle: -30,
                        fontSize: '10pt'
                    }
                },

                axes: {
                    xaxis: {
                        renderer: $.jqplot.CategoryAxisRenderer,
                        //label: 'Warranty Concern',
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                        tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                        tickOptions: {
                            angle: -30,
                            //fontFamily: 'Courier New',
                            //fontSize: '9pt'
                        }
                    },
                    yaxis: {
                        label: 'AUM (in Crs)',
                        labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
                        max: Math.round(maxval)
                    }
                }
            })
        }

        function btnPlotclick() {
            
            var regex = new RegExp("^[0-9]{2} (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) [0-9]{4}$", "i");
            var schIndId = getUrlParameter('param');
            if (typeof schIndId != 'undefined' && schIndId > 0) {
                var ImageArray = Array();

                var vdate = setDateRange();

                var data = {};
                data.minDate = vdate.fromdate;
                data.maxDate = vdate.enddate;
                data.schemeIndexIds = schIndId;
                var val = '{"schIndexid":"' + schIndId + '", "startDate":"' + vdate.fromdate + '", "endDate":"' + vdate.enddate + '"}';
                $.ajax({
                    type: "POST",
                    url: "Factsheet.aspx/getChartData",
                    async: false,
                    contentType: "application/json",
                    data: val,
                    dataType: "json",
                    success: function (msg) {
                        
                        // setChart(msg.d);
                        PlotAuto(msg.d, ImageArray);
                        // showtest();
                    },
                    error: function (msg) {
                        alert("Error! Try again...");
                    }
                });
            }
            else {
                $("#DivLast").html("<div style='width: 100%; height:100%; text-align: center; padding-top: 10%;' id='dvAvgMaturity'>Data not available for the selected scheme</div>");
                $("#DivLast").show();
            }
        }

        function PlotAuto(dataConsolidated, ImageArray) {
            $("#DivLast").show();
            var max = dataConsolidated.MaxDate;
            var min = dataConsolidated.MinDate;
            var data = dataConsolidated.SimpleNavReturnModel;
            var tt = [[]];
            for (var i = 0; i < data.length; i += 1) {

                var tt1 = {};
                tt1.name = data[i].Name;

                var points = [];
                for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
                    var res = data[i].ValueAndDate[j].Date.split("-");
                    points.push([Date.UTC(res[0], res[1] - 1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue, data[i].ValueAndDate[j].IsIndex]);
                }
                tt1.data = points;
                tt.push(tt1);
            }
            tt.shift();
            var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

            var colorarray = Array();
            for (var i = 0; i < dataConsolidated.SimpleNavReturnModel.length; i += 1) {
                colorarray.push(CustomSeriesColors[i]);
            }

            Highcharts.setOptions({
                colors: colorarray
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
                    selected: 4
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
                        var s = '';
                        var d = new Date(this.x);

                        var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
                        var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                        var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate() + ' ,' + d.getFullYear();

                        s = s + '<span style="color:#363535">' + navdate + '</span><br /><br />';

                        for (var i = 0; i < tt.length; i++) {
                            var d = 123;
                            var f = this;

                            for (var k = 0; k < tt[i].data.length; k++) {
                                if (this.x === tt[i].data[k][0]) {
                                    s = s + '<span style="color:' + colorarray[i] + '">' + tt[i].name + '</span>: <b>' + (tt[i].data[k][3] != null ? "N/A" : tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b><br />';
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
                series: tt
            },
                function (chart) {
                    setTimeout(function () {
                        $('input.highcharts-range-selector', $(chart.container).parent()).datepicker({
                            dateFormat: 'yy-mm-dd',
                            changeMonth: true,
                            changeYear: true,
                            maxDate: -2
                        });
                    }, 0);
                }
            );
        }

        function setDateRange() {
            
            var date = {};
            date.fromdate = null;
            date.enddate = null;
            <%--if ($('#<%=rbTime.ClientID %>').is(":checked")) { // check if the radio is checked
                var selectedFund = $('#<%=ddlTime.ClientID %>').find(':selected').val();
                date.enddate = Date.parse('today').add(-1).days().toString("dd MMMM yyyy");
                date.fromdate = Date.parse('today').add(parseInt(selectedFund * -1)).days().toString("dd MMMM yyyy");
            }
            else {
                date.fromdate = $('#<%=txtfromDate.ClientID %>').val();
                date.enddate = $('#<%=txtToDate.ClientID %>').val();
            }--%>
            if (date.fromdate == null || date.enddate == null) {
                //date.enddate = Date.parse('today').add(-1).days().toString("dd MMMM yyyy");
                //date.fromdate = Date.parse('today').add(-30).days().toString("dd MMMM yyyy");
                date.enddate = moment().subtract(1, 'years').format('DD MMM, yyyy');
                date.fromdate = moment().subtract(30, 'days').format('DD MMM, yyyy');
            }
            return date;
        }

        function getUrlParameter(sParam) {
            var sPageURL = window.location.search.substring(1),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
                }
            }
        };


    </script>
    <style>
        .card{
            background:#fff
        }
    </style>

</head>
<body data-sidebar="dark">
    <input type="hidden" id="hdSchemeId" value="0" runat="server" />
    <form id="form1" runat="server">
        <!-- <body data-layout="horizontal"> -->
        <!-- Begin page -->
        <div id="layout-wrapper">
            <!-- ============================================================== -->
            <!-- Start right Content here -->
            <!-- ============================================================== -->
            <div class="main-content" id="ResultFactsheet" runat="server" visible="false">
                <div class="page-content">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <h4 class="font-size-20" runat="server" id="lblFundName"></h4>
                                                <p style="margin-bottom: 0px;">
                                                    <label id="lblPresentNav" runat="server">---</label>
                                                    <label id="ImgArrow" runat="server" class="mdi text-success" visible="false"></label>
                                                    <label runat="server" id="lblIncrNav">---</label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;As on <label runat="server" id="lblCurrNavDate">---</label>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="card">
                                    <div class="card-header bg-transparent border-bottom">
                                        <h5 class="mb-0">Investment Info</h5>
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                <div class="table-responsive">
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table">
                                                        <tbody>
                                                            <tr>
                                                                <td style="padding-top: 0px;">
                                                                     <span style="font-size: 13px;">Fund Type</span>
                                                                    <p class="text-grey mb-0" runat="server" id="tdFundName" style="font-weight: bold;"></p>
                                                                   
                                                                </td>
                                                                <td style="padding-top: 0px;">
                                                                    <span style="font-size: 13px;">Entry Load</span>
                                                                    <p class="text-grey mb-0" runat="server" id="tdEntryLoad" style="font-weight: bold;"></p>
                                                                    
                                                                </td>
                                                                <td style="padding-top: 0px;">
                                                                     <span style="font-size: 13px;">Average Asset Size (Rs cr.)</span>
                                                                    <p class="text-grey mb-0" runat="server" id="tdAssetSize" style="font-weight: bold;"></p>
                                                                   
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td>
                                                                    <span style="font-size: 13px;">Investment Plan</span>
                                                                    <p class="text-grey mb-0" style="font-weight: bold;" runat="server" id="tdStructure"></p>
                                                                    
                                                                </td>
                                                                <td>
                                                                    <span style="font-size: 13px;">Exit Load</span>
                                                                    <p class="text-grey mb-0" style="font-weight: bold;" runat="server" id="tdExitLoad"></p>
                                                                    
                                                                </td>
                                                                <td>
                                                                    <span style="font-size: 13px;">Minimum Investment (Rs.)</span>
                                                                    <p class="text-grey mb-0" runat="server" id="tdMinInvest" style="font-weight: bold;"></p>
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                     <span style="font-size: 13px;">Launch Date</span>
                                                                    <p class="text-grey mb-0" runat="server" id="tdLunchDate" style="font-weight: bold;"></p>
                                                                   
                                                                </td>
                                                                <td>
                                                                     <span style="font-size: 13px;">Fund Manager</span>
                                                                    <p class="text-grey mb-0" runat="server" id="tdFundMan" style="font-weight: bold;"></p>
                                                                   
                                                                </td>
                                                                <%--<td>
                                                                    <p class="text-grey mb-0" runat="server" id="tdLastDiv" style="font-weight: bold;"></p>
                                                                    <span style="font-size: 13px;">Last Dividend(Rs.)</span>
                                                                </td>--%>
                                                                <td>
                                                                    <span style="font-size: 13px;">Benchmark</span>
                                                                    <p class="text-grey mb-0" runat="server" id="tdBenchMark" style="font-weight: bold;"></p>
                                                                    
                                                                </td>
                                                            </tr>
                                                            <%--<tr>--%>
                                                                <%--<td>
                                                                    <p class="text-grey mb-0" runat="server" id="tdBenchMark" style="font-weight: bold;"></p>
                                                                    <span style="font-size: 13px;">Benchmark</span>
                                                                </td>--%>
                                                                <%--<td>
                                                                    <p class="text-grey mb-0" runat="server" id="tdEmail" style="font-weight: bold;"></p>
                                                                    <span style="font-size: 13px;">Email / Website</span>
                                                                </td>--%>
                                                                <%--<td>
                                                                    <p class="text-grey mb-0" runat="server" id="tdBonus" style="font-weight: bold;"></p>
                                                                    <span style="font-size: 13px;">Bonus</span>
                                                                </td>--%>
                                                                <%--<td>
                                                                    <p class="text-grey mb-0" runat="server" id="tdAmcName" style="font-weight: bold;"></p>
                                                                    <span style="font-size: 13px;">AMC Name</span>
                                                                </td>--%>
                                                           <%-- </tr>--%>
                                                            <%--<tr>
            
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                            </tr>--%>
                                                        </tbody>
                                                    </table>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-8 col-lg-8">
                                        <div class="card">
                                            <div class="card-header bg-transparent border-bottom">
                                                <h5 class="mb-0">NAV Graph</h5>
                                            </div>
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                        <div id="HighContainer" style="height: 462px; min-width: 310px; max-width: 1000px"></div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4">
                                        <div>
                                            <div class="card">
                                                <div class="card-header bg-transparent border-bottom">
                                                    <h5 class="mb-0">52 weeks</h5>
                                                </div>
                                                <div class="card-body">
                                                    <div class="row align-items-center">
                                                        <div class="col-sm">
                                                            <p class="text-muted mb-0">High</p>
                                                            <h4 class="text-success">
                                                                <label runat="server" id="lblHigh">--</label>
                                                                <small style="font-size: 14px">(<span id="spnMaxDate" runat="server"></span>)</small></h4>
                                                        </div>
                                                        <div class="col-sm">
                                                            <p class="text-muted mb-0">Low</p>
                                                            <h4 class="text-danger">
                                                                <label runat="server" id="lblLow">--</label><small style="font-size: 14px">(<span id="spnMinDate" runat="server"></span>)</small></h4>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div>
                                            <div class="card">
                                                <div class="card-header">
                                                    <h5 class="card-title mb-0">Funds return</h5>
                                                </div>
                                                <div class="card-body">

                                                    <div class="row align-items-center g-0">
                                                        <table class="table table-bordered" style="margin-bottom: 0px;">
                                                            <tr>
                                                                <th>1 mth</th>
                                                                <td>
                                                                    <label runat="server" id="spn1mth"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>3 mths</th>
                                                                <td>
                                                                    <label runat="server" id="spn3mth"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>6 mths</th>
                                                                <td>
                                                                    <label runat="server" id="spn6mth"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>1 Yr</th>
                                                                <td>
                                                                    <label runat="server" id="spn1yr"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>3 Yrs</th>
                                                                <td>
                                                                    <label runat="server" id="spn3yr"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Since Inception</th>
                                                                <td>
                                                                    <label runat="server" id="spnSinceInception"></label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="card">
                                        <div class="card-header bg-transparent border-bottom">
                                            <h5 class="mb-0">Top 10 Holdings</h5>
                                        </div>
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <asp:Repeater ID="TopCompDetails" OnItemDataBound="TopCompDetails_ItemDataBound" runat="server">
                                                        <HeaderTemplate>
                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-striped">
                                                                <thead class="table-light">
                                                                    <tr>
                                                                        <th style="text-align: left">Company Name
                                                                        </th>
                                                                        <th style="text-align: left">Sector Name
                                                                        </th>
                                                                        <th style="text-align: right; border-right: none; white-space:nowrap">Asset %</th>
                                                                    </tr>
                                                                </thead>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="vertical-align:middle">
                                                                    <asp:Label ID="lblCompany" Text='<%#Eval("CompName") %>' runat="server" Style="font-weight: bold"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align:middle">
                                                                    <asp:Label ID="lblSector" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                                </td>
                                                                <td style="text-align: center; border-right: none; vertical-align: middle; white-space:nowrap">
                                                                    <div class="progress mt-1" style="height: 6px;">
                                                                        <asp:Label CssClass="progress-bar progress-bar bg-primary" runat="server" ID="lblprgbar" role="progressbar" Style="width: 72%" aria-valuenow="52" aria-valuemin="0" aria-valuemax="52">
                                                                        </asp:Label>
                                                                        <asp:HiddenField runat="server" ID="hdfProgressBarWidth" Value='<%# Eval("Bar_Value") %>' />
                                                                    </div>
                                                                    <asp:Label ID="lblNetAsset" runat="server" Text='<%#Eval("Net_Asset") %>' Style="font-size:11px;"></asp:Label>
                                                                </td>
                                                              
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>

                                                </div>
                                                <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                                    <asp:Repeater ID="TopCompDetails1" OnItemDataBound="TopCompDetails1_ItemDataBound" runat="server">
                                                        <HeaderTemplate>
                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-striped">
                                                                <thead class="table-light">
                                                                    <tr>
                                                                        <th style="text-align: left">Company Name
                                                                        </th>
                                                                        <th style="text-align: left">Sector Name
                                                                        </th>
                                                                        <th style="text-align: right; border-right: none; white-space:nowrap">Asset %</th>
                                                                    </tr>
                                                                </thead>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="vertical-align:middle">
                                                                    <asp:Label ID="lblCompany1" Text='<%#Eval("CompName") %>' runat="server" Style="font-weight: bold;"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align:middle">
                                                                    <asp:Label ID="lblSector1" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                                </td>
                                                                <td style="text-align: center; border-right: none; vertical-align: middle;">
                                                                    <div class="progress mt-1" style="height: 6px;">
                                                                        <asp:Label CssClass="progress-bar progress-bar bg-primary" runat="server" ID="lblprgbar1" role="progressbar" Style="width: 72%" aria-valuenow="52" aria-valuemin="0" aria-valuemax="52">
                                                                        </asp:Label>
                                                                        <asp:HiddenField runat="server" ID="hdfProgressBarWidth1" Value='<%# Eval("Bar_Value") %>' />
                                                                    </div>
                                                                     <asp:Label ID="lblNetAsset1" runat="server" Text='<%#Eval("Net_Asset") %>' Style="font-size:11px;"></asp:Label>
                                                                </td>
                                                               
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl col-lg-6 col-md-12 col-sm-12">
                                <div class="row">
                                    <div class="col-xl-2 col-lg-6 col-md-6 col-sm-12">
                                        <!-- Card -->
                                        <div class="card">
                                            <div class="card-body">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <h4 class="mb-2">
                                                            <label runat="server" id="lblSharpe"></label>
                                                        </h4>
                                                        <p class="text-muted mb-0">Sharpe</p>
                                                    </div>
                                                    <div class="avatar-md">
                                                        <span class="avatar-title bg-soft-primary rounded">
                                                            <span class="mdi mdi-chart-line font-size-24 text-primary"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl col-sm-6">
                                        <!-- Card -->
                                        <div class="card">
                                            <div class="card-body">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <h4 class="mb-2">
                                                            <label runat="server" id="lblSortino"></label>
                                                        </h4>
                                                        <p class="text-muted mb-0">Sortino</p>
                                                    </div>
                                                    <div class="avatar-md">
                                                        <span class="avatar-title bg-soft-primary rounded">
                                                            <span class="mdi mdi-chart-pie font-size-24 text-primary"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl col-sm-6">
                                        <!-- Card -->
                                        <div class="card">
                                            <div class="card-body">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <h4 class="mb-2">
                                                            <label runat="server" id="lblSdv"></label>
                                                        </h4>
                                                        <p class="text-muted mb-0">Standard Deviation</p>
                                                    </div>
                                                    <div class="avatar-md">
                                                        <span class="avatar-title bg-soft-primary rounded">
                                                            <span class="mdi mdi-square-root font-size-24 text-primary"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl col-sm-6">
                                        <!-- Card -->
                                        <div class="card">
                                            <div class="card-body">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <h4 class="mb-2">
                                                            <label runat="server" id="lblBeta"></label>
                                                        </h4>
                                                        <p class="text-muted mb-0">Beta</p>
                                                    </div>
                                                    <div class="avatar-md">
                                                        <span class="avatar-title bg-soft-primary rounded">
                                                            <span class="mdi mdi-beta font-size-24 text-primary"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl col-sm-6">
                                        <!-- Card -->
                                        <div class="card">
                                            <div class="card-body">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <h4 class="mb-2">
                                                            <label runat="server" id="lblRSqure"></label>
                                                        </h4>
                                                        <p class="text-muted mb-0">R-Square</p>
                                                    </div>
                                                    <div class="avatar-md">
                                                        <span class="avatar-title bg-soft-primary rounded">
                                                            <span class="mdi mdi-finance font-size-24 text-primary"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                        <div class="card">
                                            <div class="card-header bg-transparent border-bottom">
                                                <h5 class="mb-0">Top 5 Sector Holding</h5>
                                            </div>
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                        <asp:Repeater ID="RepTopSector" runat="server">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="text-align: left">Sector Name
                                                                    </th>
                                                                            <th style="text-align: right; border-right: none">% Allocation
                                                                    </th>
                                                                            <th style="border-left: none;">&nbsp;</th>
                                                                        </tr>
                                                                    </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="value_tablerow">
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblSectorName" Style="font-weight: bold" Text='<%#Eval("Sector_Name") %>'></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: center; border-right: none; vertical-align: middle;">
                                                                        <div class="progress mt-1" style="height: 6px;">
                                                                            <asp:Label CssClass="progress-bar progress-bar bg-primary" runat="server" ID="lblprgbarTop5" role="progressbar" Style="width: 72%" aria-valuenow="52" aria-valuemin="0" aria-valuemax="52">
                                                                            </asp:Label>
                                                                            <asp:HiddenField runat="server" ID="hdfProgressBarWidthTop5" Value='<%# Eval("Scheme_Per") %>' />
                                                                        </div>
                                                                    </td>
                                                                    <td style="border-left: none">
                                                                        <asp:Label ID="lblSchemePer" runat="server" Text='<%#Eval("Scheme_Per") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                        <div class="card">
                                            <div class="card-header bg-transparent border-bottom">
                                                <h5 class="mb-0">
                                                    <asp:Label runat="server" ID="TdMc"></asp:Label></h5>
                                            </div>
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                        <%--<div class="card">--%>
                                                            <%--<div class="card-header" style="background: #fff">
                                    <h5>
                                        <asp:Label runat="server" ID="TdMc"></asp:Label>
                                    </h5>
                                </div>--%>
                                                            <%--<div class="card-body">--%>
                                                                <div class="">
                                                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                                        <div class="table-responsive">
                                                                            <div style="float: left; height: 260px; margin-top: 20px; text-align: center; width: 98%" id="dvMarketCapContaner">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            <%--</div>--%>
                                                        <%--</div>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                        <div class="card">
                                            <div class="card-header bg-transparent border-bottom">
                                                <h5 class="mb-0">AUM Movement</h5>
                                            </div>
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                        <div class="table-responsive">
                                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <div style="width: 100%; height: 220px;" id="chart1">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                        <div class="card">
                                            <div class="card-header bg-transparent border-bottom">
                                                <h5 class="mb-0">Asset Allocation</h5>
                                            </div>
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                                        <div class="table-responsive">
                                                            <div id="chartAssetAllocationContainer" style="height: 220px; font-family: Arial; font-size: 10px; width: 100%">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="font-size: 12px">
                                Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing. Past performance of the schemes do not indicate the future performance.
                            </div>
                            <div class="value_btm_text" style="font-size: 10px; color: #A7A7A7; text-align: right; margin-top: 10px;">
                                Developed for Askmefund by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END layout-wrapper -->
    </form>

    <!-- JAVASCRIPT -->
    <script src="assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="assets/libs/metismenujs/metismenujs.min.js"></script>
    <script src="assets/libs/simplebar/simplebar.min.js"></script>
    <script src="assets/libs/feather-icons/feather.min.js"></script>

    <script src="assets/js/app.js"></script>

    <!-- nouisliderribute js -->
    <script src="assets/libs/nouislider/nouislider.min.js"></script>
    <script src="assets/libs/wnumb/wNumb.min.js"></script>

    <!-- range slider init -->
    <script src="assets/js/range-sliders.init.js"></script>
    <script>
        $('.nav-tabs li').click(function () {
            $('.nav-tabs li.active').removeClass('active');
            $(this).addClass('active');
        });

        $("ul.nav-tabs a").click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
        var hidWidth;
        var scrollBarWidths = 40;

        var widthOfList = function (Id) {
            var itemsWidth = 0;
            $('#' + Id + ' .list li').each(function () {
                var itemWidth = $(this).outerWidth();
                itemsWidth += itemWidth;
            });
            return itemsWidth;
        };

        //var widthOfHidden = function (Id) {
        //    return (($('#' + Id + ' .wrapper').outerWidth()) - widthOfList(Id) - getLeftPosi(Id)) - scrollBarWidths;
        //};

        //var getLeftPosi = function (Id) {
        //    return $('#' + Id + ' .list').position().left;
        //};
        var widthOfHidden = function (Id) {

            var ww = 0 - $('#' + Id + ' .wrapper').outerWidth();
            var hw = (($('#' + Id + ' .wrapper').outerWidth()) - widthOfList(Id) - getLeftPosi(Id)) - scrollBarWidths;

            if (ww > hw) {
                return ww;
            }
            else {
                return hw;
            }

        };

        var getLeftPosi = function (Id) {

            var ww = 0 - $('#' + Id + ' .wrapper').outerWidth();
            var lp = $('#' + Id + ' .list').position().left;

            if (ww > lp) {
                return ww;
            }
            else {
                return lp;
            }
        };

        var reAdjust = function (Id) {
            // alert(Id);
            if (($('#' + Id + ' .wrapper').outerWidth()) < widthOfList(Id)) {
                $('#' + Id + ' .scroller-right').show();
            }
            else {
                $('#' + Id + ' .scroller-right').hide();
            }

            if (getLeftPosi(Id) < 0) {
                $('#' + Id + ' .scroller-left').show();
            }
            else {
                $('#' + Id + ' .item').animate({ left: "-=" + getLeftPosi(Id) + "px" }, 'slow');
                $('#' + Id + ' .scroller-left').hide();
            }
        }

        reAdjust("Equity");

        //$(window).on('resize', function (e) {
        //    reAdjust("Equity");
        //});

        $('.scroller-right').click(function () {
            var Id = (this).parentElement.parentElement.parentElement.id;
            $('#' + Id + ' .scroller-left').fadeIn('slow');
            $('#' + Id + ' .scroller-right').fadeOut('slow');
            $('#' + Id + ' .list').animate({ left: "+=" + widthOfHidden(Id) + "px" }, 'slow', function () {
                reAdjust(Id);
            });
        });

        $('.scroller-left').click(function () {
            var Id = (this).parentElement.parentElement.parentElement.id;

            $('#' + Id + ' .scroller-right').fadeIn('slow');
            $('#' + Id + ' .scroller-left').fadeOut('slow');

            $('#' + Id + ' .list').animate({ left: "-=" + getLeftPosi(Id) + "px" }, 'slow', function () {
                reAdjust(Id);
            });
        });

    </script>
</body>
</html>
