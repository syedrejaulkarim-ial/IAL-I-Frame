<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="Factsheet.aspx.cs"
    Inherits="iFrames.MutualFundIndia.Factsheet" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <title>Factsheet</title>

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="../Styles/jquery.jqplot.css" rel="stylesheet" type="text/css" />
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.min.js"></script>
    <![endif]-->
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script>
    <![endif]-->
    <script src="https://use.fontawesome.com/ea6a7e4db5.js"></script>
    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <script type="text/javascript" src="js/date.js"></script>
    <script src="js/AutoComplete.js" type="text/javascript"></script>

    <script src="js/modernizr2.js" type="text/javascript"></script>

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
    <script src="js/highcharts-3d.js"></script>

    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Nunito+Sans:wght@300;400;600;700;800&display=swap" rel="stylesheet">

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->
    <script type="text/javascript">
        WebFontConfig = {
            google: { families: ['Open+Sans:400,600,700,300:latin'] }
        };
        (function () {
            var wf = document.createElement('script');
            wf.src = ('https:' == document.location.protocol ? 'https' : 'http') +
                '://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';
            wf.type = 'text/javascript';
            wf.async = 'true';
            var s = document.getElementsByTagName('script')[0];
            s.parentNode.insertBefore(wf, s);
        })(); </script>
    <script type="text/javascript">
        //$('#back').click(function () {
        //    parent.history.back();
        //    return false;
        //});
        $(function () {
            //$.jqplot.config.enablePlugins = true;
            //$("#listboxSchemeName").change(function () {
            //    getGraphs($("#listboxSchemeName").val());
            //});
            //if (($("#listboxSchemeName").val() > 0) && ($("#listboxSchemeName").val() != 'null')) {

            // };

            if ($("#hdSchemeId").val() != null && $("#hdSchemeId").val() != 0) {
                getGraphs($("#hdSchemeId").val());
                GetChart($("#hdSchemeId").val());
            }

        });

        function getGraphs(schid) {
            btnPlotclick();
            //PullChart(schid);
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
                    PlotAssetAllocationHigh(dataConsolidated.d);
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
                seriesColors: ["#da251d", "#ed9c54", "#605d5c", "#e6ab0d", "#49b959"],
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
                legend: {
                    show: true,
                    location: 'w',
                    fontFamily: 'Helvetica,Arial,sans-serif',
                    fontSize: '12px'
                    //labels: ['Equity', 'Debt', 'Others']
                }
            });
        }

        function PlotAssetAllocationHigh(data) {
            if (data == null || data.length < 1) {
                $('#chartAssetAllocationContainer').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="chartAssetAllocation">Data not available for the selected scheme</div>');
                $('#chartAssetAllocationContainer').effect("highlight", {}, 3000);
                return;
            }
            $('#chartAssetAllocationContainer').append('<div style="width: 97%; height:100%;" id="dvMarketCap" ></div>');

            var tt = [];
            var nature = [];
            var tt1 = {};
            tt1.name = "Asset Allocat";
            tt1.data = [];
            for (var i = 0; i < data.length; i += 1) {

                var ttobj = {
                    name: data[i].NatureName,
                    y: data[i].Value
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

            var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

            Highcharts.setOptions({
                colors: CustomSeriesColors
            });

            var CurrChart = Highcharts.chart('chartAssetAllocationContainer', {
                chart: {
                    type: 'pie',
                    //height: 150,
                    spacingTop: -10,
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
                        innerSize: 50,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                            enabled: true,
                            distance: 10,
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

        function PullChart(schid) {
            var dataToPush = JSON.stringify({
                schemeIds: schid
            });
            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                url: 'Factsheet.aspx/NavChartData',
                type: 'POST',
                asynchronus: true,
                contentType: "application/json; charset=utf-8",
                success: function (dataConsolidated) {
                    if (dataConsolidated.Value == 0) {
                        return;
                    }
                    Plot(dataConsolidated.d);
                },
                error: function (data, e) {
                    alert("Something went wrong...!!");
                }
            });
        }

        function Plot(dataConsolidated) {
            var max = dataConsolidated.MaxDate;
            var min = dataConsolidated.MinDate;
            var data = dataConsolidated.SimpleNavReturnModel;
            var seriesNames = Array();
            var dataPlot = [[[]]];
            for (var i = 0; i < data.length; i += 1) {
                seriesNames.push(data[i].Name);
                var points = [];
                for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
                    points.push([data[i].ValueAndDate[j].Date, data[i].ValueAndDate[j].Value]);
                }
                dataPlot.push(points);
            }
            dataPlot.shift();
            $('#chart').remove();
            if (data.length < 1) {
                $('#chartContainer').append('<div style="width: 100%; height:200px; text-align: center; padding-top: 10%;" id="chart">Data not Available for the selected date range</div>');
                $('#chartContainer').effect("highlight", {}, 3000);
                return;
            }
            $('#chartContainer').append('<div style="width: 97%; height:200px;" id="chart" ></div>');

            var plot2 = $.jqplot('chart', dataPlot,
                {
                    seriesColors: ["#da251d", "#ed9c54", "#605d5c", "#e6ab0d", "#49b959", "#ed496c", "#e87500", "#4f8ad3"],
                    animate: true,
                    animateReplot: true,
                    axes: {
                        xaxis: {
                            min: min,
                            max: max,
                            renderer: $.jqplot.DateAxisRenderer,
                            rendererOptions: { tickRenderer: $.jqplot.CanvasAxisTickRenderer },
                            //tickInterval: '7 day',
                            tickOptions: { formatString: '%b %#d, %y', fontFamily: 'Arial,sans-serif,Helvetica', }
                            //tickOptions: { formatString: '%#d/%#m/%Y' }
                        },
                        yaxis:
                        {
                            label: 'Value [Rebased]',
                            tickOptions: { formatString: '%.2f', fontFamily: 'Arial,sans-serif,Helvetica' },
                            labelRenderer: $.jqplot.CanvasAxisLabelRenderer
                        }
                    },
                    seriesDefaults: { showMarker: false, lineWidth: 2, rendererOptions: { animation: { speed: 1000 } } },
                    highlighter: { show: false, sizeAdjust: 7.5 },
                    cursor: { show: false, zoom: true, showTooltip: false },
                    legend: {
                        renderer: $.jqplot.EnhancedLegendRenderer,
                        show: true,
                        location: 's',
                        rendererOptions: {
                            numberRows: 4,
                            numberColumns: 3
                        },
                        placement: 'outsideGrid',
                        seriesToggle: 'on',
                        fontSize: '1em',
                        border: '0px solid black',
                        fontFamily: 'Arial,sans-serif,Helvetica'
                    },
                    grid: {
                        shadow: false,
                        borderWidth: 0,
                        background: 'rgba(0,0,0,0)'
                    }
                });
            for (var i = 0; i < seriesNames.length; i += 1) {
                plot2.series[i].label = seriesNames[i];
            }
            plot2.replot();
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
                    PlotMktChartHigh(dataConsolidated.d);
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
                    rendererOptions: { barMargin: 40 },
                    pointLabels: { show: true, stackedValue: true }

                },
                axesDefaults: {
                    tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                    tickOptions: {
                        angle: -30,
                        fontSize: '10pt'
                    }
                },
                //axes: {
                //    xaxis: {
                //        renderer: $.jqplot.CategoryAxisRenderer,
                //        tickOptions: {
                //            angle: -60
                //        }
                //    }
                //}

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

        function PlotMktChartHigh(data) {
            var maxval = 0;
            var dataPlot = [];
            var dataToInsert = {};
            dataToInsert.name = "AUM";
            dataToInsert.data = [];
            var xAxisData = [];
            for (var i = 0; i < data.length; i += 1) {
                xAxisData.push(data[i].PortDate);
                dataToInsert.data.push(data[i].MatketValue)
            }
            //if (maxval < data[i].MatketValue)
            //    maxval = data[i].MatketValue;

            dataPlot.push(dataToInsert);
            //maxval = maxval * .2 + maxval;

            var CustomSeriesColors = ["#4f8ad3"];

            Highcharts.setOptions({
                colors: CustomSeriesColors
            });

            var CurrChart = Highcharts.chart('chart1', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: xAxisData,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'AUM (In Cr.)'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                credits: {
                    text: 'Source: MFI360'
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: dataPlot
            });
        }

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
                    // debugger;
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

        function MarketCap(dataAll) {
            // $('#dvMarketCap').remove();
            var data = dataAll.LstMarketCap;
            if (data == null || data.length < 1) {
                $('#dvMarketCapContaner').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="dvMarketCap">Data not available for the selected scheme</div>');
                $('#dvMarketCapContaner').effect("highlight", {}, 3000);
                return;
            }
            $('#dvMarketCapContaner').append('<div style="width: 97%; height:100%;" id="dvMarketCap" ></div>');

            //$.jqplot.config.enablePlugins = true;
            var dataPlot = [[]];
            var natures = [];
            var ticks = [];
            var points = [];
            for (var i = 0; i < data.length; i += 1) {
                points.push(data[i].M_Cap);
                natures.push({ label: data[i].Market_Slap });
                ticks.push(data[i].Market_Slap);
            }
            dataPlot.push(points);
            dataPlot.shift();
            var plot1 = $.jqplot('dvMarketCapContaner', dataPlot, {
                animate: !$.jqplot.use_excanvas,
                seriesColors: ["#4f8ad3"],
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    pointLabels: { show: true, location: 'e', edgeTolerance: -15 },
                    //rendererOptions: { fillToZero: true }, //commented by syed
                    rendererOptions: {
                        barDirection: 'horizontal',
                        //dataLabels: 'value',
                        //dataLabelFormatString: '%.4f',

                        fillToZero: true
                    },
                    //pointLabels: { show: false }
                },
                series: natures,
                legend: {
                    show: false,
                    placement: 'outsidegrid'
                },
                axes: {
                    xaxis: {
                        //renderer: $.jqplot.CategoryAxisRenderer,
                        //ticks: ticks
                        pad: 1.05,
                        tickOptions: { formatString: '%.2f' }, // tickOptions: { formatString: '$%d' }// change by syed
                        label: 'Market Cap(%)',
                    },
                    yaxis: {
                        //pad: 1.05,
                        //tickOptions: { formatString: '%.2f' } // tickOptions: { formatString: '$%d' }// change by syed
                        renderer: $.jqplot.CategoryAxisRenderer,
                        ticks: ticks
                    }
                    //xaxis: { renderer: $.jqplot.CategoryAxisRenderer, ticks: ticks }
                },
                highlighter: { show: false }
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

            var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

            Highcharts.setOptions({
                colors: CustomSeriesColors
            });

            var CurrChart = Highcharts.chart('dvMarketCapContaner', {
                chart: {
                    type: 'pie',
                    //height: 150,
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

        function AverageMaturity(dataAll) {
            data = dataAll.LstAverageMaturity;
            // $('#dvAvgMaturity').remove();
            if (data == null || data.length < 1) {
                $('#dvMarketCapContaner').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="dvAvgMaturity">Data not available for the selected scheme</div>');
                $('#dvMarketCapContaner').effect("highlight", {}, 3000);
                return;
            }
            $('#dvMarketCapContaner').append('<div style="width: 97%; height:100%;" id="dvAvgMaturity" ></div>');

            //$.jqplot.config.enablePlugins = true;
            var dataPlot = [[]];
            var natures = [];
            var ticks = [];
            var points = [];
            for (var i = 0; i < data.length; i += 1) {
                points.push(data[i].Average_Maturity);
                natures.push({ label: data[i].MonthYear });
                ticks.push(data[i].MonthYear);
            }
            dataPlot.push(points);
            dataPlot.shift();
            var plot1 = $.jqplot('dvMarketCapContaner', dataPlot, {
                animate: !$.jqplot.use_excanvas,
                seriesColors: ["#4f8ad3"],
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    pointLabels: { show: true, location: 'e', edgeTolerance: -15 },
                    //rendererOptions: { fillToZero: true }, //commented by syed
                    rendererOptions: {
                        barDirection: 'horizontal',
                        //dataLabels: 'value',
                        //dataLabelFormatString: '%.4f',

                        fillToZero: true
                    },
                    //pointLabels: { show: true }
                },
                series: natures,
                legend: {
                    show: false,
                    placement: 'outsidegrid'
                },
                axes: {
                    xaxis: {
                        //renderer: $.jqplot.CategoryAxisRenderer,
                        //ticks: ticks
                        label: 'Average Maturity (in Years)',
                        pad: 1.05,
                        tickOptions: { formatString: '%.2f' } // tickOptions: { formatString: '$%d' }// change by syed

                    },
                    yaxis: {
                        //pad: 1.05,
                        //tickOptions: { formatString: '%.2f' } // tickOptions: { formatString: '$%d' }// change by syed
                        renderer: $.jqplot.CategoryAxisRenderer,
                        ticks: ticks
                    }
                    //xaxis: { renderer: $.jqplot.CategoryAxisRenderer, ticks: ticks }
                },
                highlighter: { show: false }
            });
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
                        //  debugger;
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

        function setDateRange() {
            var date = {};
            date.fromdate = null;
            date.enddate = null;
            if ($('#<%=rbTime.ClientID %>').is(":checked")) { // check if the radio is checked
                var selectedFund = $('#<%=ddlTime.ClientID %>').find(':selected').val();
                date.enddate = Date.parse('today').add(-1).days().toString("dd MMMM yyyy");
                date.fromdate = Date.parse('today').add(parseInt(selectedFund * -1)).days().toString("dd MMMM yyyy");
            }
            else {
                date.fromdate = $('#<%=txtfromDate.ClientID %>').val();
                date.enddate = $('#<%=txtToDate.ClientID %>').val();
            }
            if (date.fromdate == null || date.enddate == null) {
                date.enddate = Date.parse('today').add(-1).days().toString("dd MMMM yyyy");
                date.fromdate = Date.parse('today').add(-30).days().toString("dd MMMM yyyy");
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

    </script>
    <style>
        kbd {
            padding: 2px 7px;
            color: #fff;
            border-radius: 2px;
            white-space: nowrap;
        }

            kbd.green {
                background: #439c33;
            }

            kbd.red {
                background: #dc1414;
            }

        .cstm_class tr th, .cstm_class tr td {
            padding: 4px;
            font-size: 12px;
            white-space: nowrap;
        }

        h5 {
            margin: 0;
        }

        .category-style-03 .category-item {
            margin-right: 7%;
        }

            .category-style-03 .category-item:last-child {
                margin-right: 0%;
            }

        .badge {
            border-radius: 5px;
        }

        #spnMaxDate {
            font-size: 11px;
            font-weight: normal;
        }

        #spnMinDate {
            font-size: 11px;
            font-weight: normal;
        }

        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td {
            vertical-align: middle;
        }

        h6 {
            font-size: 1.2em;
            line-height: 1em;
            margin-bottom: 0px;
        }

        .bg-grey {
            background-color: #f7f7f7 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card">
            <div class="card-body">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="row">
                            <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                <label>Category</label>
                                <asp:DropDownList ID="ddlCategory" runat="server"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                    AutoPostBack="true"
                                    CssClass="form-control form-control-sm">
                                </asp:DropDownList>
                            </div>

                            <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                <label>Sub-Category</label>
                                <asp:DropDownList ID="ddlSubNature" runat="server"
                                    OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged"
                                    AutoPostBack="true"
                                    CssClass="form-control form-control-sm">
                                </asp:DropDownList>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <label>Type</label>
                                    <asp:DropDownList ID="ddlType" runat="server"
                                        OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                        AutoPostBack="true"
                                        class="form-control form-control-sm">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="row">
                            <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                <label>Mutual Fund Name</label>
                                <%-- <select  class="form-control form-control-sm">
                                                            <option selected="selected" value=" Select AMC Name">Select
                                                                AMC Name</option>
                                                            <option value="3">Aditya Birla Sun Life Mutual Fund</option>
                                                            <option value="46">Axis Mutual Fund</option>
                                                            <option value="4">Baroda Mutual Fund</option>
                                                        </select>--%>
                                <asp:DropDownList ID="ddlFundHouse" runat="server"
                                    class="form-control form-control-sm" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlFund_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-8 col-lg-8">
                                <div class="form-group">
                                    <label>Choose Scheme</label>
                                    <asp:DropDownList ID="ddlScheme" runat="server"
                                        class="form-control form-control-sm" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card mt-3" runat="server" visible="false" id="dvFactSheet">
            <div class="card-body">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-8 col-lg-8">
                        <h3>
                            <asp:Label runat="server" ID="lblFundNameBold"></asp:Label>
                        </h3>
                        <span class="badge badge-soft-inverse" style="padding-left: 0px;">
                            <asp:Label runat="server" ID="lblCurrNav"></asp:Label>
                            <img id="imgArrow" src="img/arwup.png" class="pt-0" runat="server" />
                            <asp:Label runat="server" ID="lblIncrNav"></asp:Label>
                        </span>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4">
                        <div class="card">
                            <div class="card-header text-center" style="background: #fff; padding: 0.25rem 0.65rem">
                                <h6>52 Weeks</h6>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 pr-0">
                                        <span class="badge badge-success-inverse">
                                            <strong>
                                                <asp:Label runat="server" ID="lblHigh">--</asp:Label>
                                            </strong>
                                            <br />
                                            (<small><span id="spnMaxDate" runat="server"></span></small>)
                                        </span>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 text-lg-right pl-0">
                                        <span class="badge badge-danger-inverse">
                                            <strong>
                                                <asp:Label runat="server" ID="lblLow">--</asp:Label>
                                            </strong>
                                            <br />
                                            (<small><span id="spnMinDate" runat="server"></span></small>)
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row pt-3">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>Investment Info</h6>
                            </div>
                            <div class="card-body">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table" style="margin-bottom: 0px">
                                        <tr>
                                            <td style="border: none" id="tdFundName" runat="server">
                                                <p class="text-grey" style="margin-bottom: 0px;"><strong>Fund Type</strong></p>
                                                <asp:Label runat="server" ID="lblFundType"></asp:Label>
                                            </td>
                                            <td style="border: none; display: none">
                                                <input type="hidden" id="hdSchemeId" value="0" runat="server" />
                                            </td>
                                            <td style="border: none; display: none" id="tdEntryLoad" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Entry Load</strong></p>
                                                <asp:Label runat="server" ID="lblEntry"></asp:Label>
                                            </td>
                                            <td style="border: none" id="tdAssetSize" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Average Asset Size (₹ cr.)</strong></p>
                                                <asp:Label runat="server" ID="lblAsset"></asp:Label>
                                            </td>
                                            <td style="border: none" id="tdStructure" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Investment Plan</strong></p>
                                                <asp:Label runat="server" ID="lblInvestMent"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="border: none" id="tdExitLoad" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Exit Load</strong></p>
                                                <asp:Label runat="server" ID="lblExit"></asp:Label>
                                            </td>
                                            <td style="border: none" id="tdMinInvest" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Minimum Investment (₹)</strong></p>
                                                <asp:Label runat="server" ID="lblMinIvest"></asp:Label>
                                            </td>
                                            <td style="border: none" id="tdLunchDate" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Launch Date</strong></p>
                                                <asp:Label runat="server" ID="lblIncepDate"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="border: none" id="tdFundMan" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Fund Manager</strong></p>
                                                <asp:Label runat="server" ID="lblFundMan"></asp:Label>
                                            </td>
                                            <td style="border: none" id="tdLastDiv" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Last Dividend</strong></p>
                                                <asp:Label runat="server" ID="lblLastDiv"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border: none; display: none" id="tdBenchMark" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Benchmark</strong></p>
                                                <asp:Label runat="server" ID="lblBench"></asp:Label>
                                            </td>
                                            <td style="border: none; display: none" id="tdEmail" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Email / Website</strong></p>
                                                <asp:Label runat="server" ID="lblWebSite"></asp:Label>
                                            </td>
                                            <td style="border: none; display: none" id="tdBonus" runat="server">
                                                <p style="margin-bottom: 0px;"><strong>Bonus</strong></p>
                                                <asp:Label runat="server" ID="lblBonus"></asp:Label>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                                                        <td class="value_tablerow" id="tdAmcName" runat="server"
                                                                            style="border: none; display: none"
                                                                            colspan="3"></td>
                                                                    </tr>--%>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row pt-3">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>Peer Performance</h6>
                            </div>
                            <div class="card-body">
                                <div class="">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                        <asp:Repeater ID="PeerPerformance" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th rowspan="2">Fund Name
                                                            </th>
                                                            <th rowspan="2" style="text-align: right; white-space: nowrap">AUM (₹ Cr)
                                                            </th>
                                                            <th colspan="7" style="text-align: center; padding-top: 0px; padding-bottom: 0px">Return (%)
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="text-align: right; white-space: nowrap">1M
                                                            </th>
                                                            <th style="text-align: right; white-space: nowrap">3M
                                                            </th>
                                                            <th style="text-align: right; white-space: nowrap">6M
                                                            </th>
                                                            <th style="text-align: right; white-space: nowrap">1Y
                                                            </th>
                                                            <th style="text-align: right; white-space: nowrap">3Y
                                                            </th>
                                                            <th style="text-align: right; white-space: nowrap">5Y
                                                            </th>
                                                            <th style="text-align: right; white-space: nowrap">Since
                                                                            Inception</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr id="dvTrID" runat="server">
                                                    <td>
                                                        <a href="https://mfiframes.mutualfundsindia.com/MutualFundIndia/Factsheet.aspx?param=<%#Eval("SchemeId")%>"><%#Eval("SchemeName")%></a>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label2" runat="server" Text='<%#(Eval("FundSize") != DBNull.Value ? !string.IsNullOrEmpty(Eval("FundSize").ToString()) ? (String.Format("{0:#,0.00}", Convert.ToDouble(Eval("FundSize")))) :  "--" : "--") %>' />
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label4" runat="server" Text='<%#(Eval("Per30Days") != DBNull.Value ? !string.IsNullOrEmpty(Eval("Per30Days").ToString()) ?(String.Format("{0:#,0.00}", Convert.ToDouble(Eval("Per30Days"))))  :  "--" : "--") %>' />
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label6" runat="server" Text='<%#(Eval("Per91Days") != DBNull.Value ? !string.IsNullOrEmpty(Eval("Per91Days").ToString()) ? (String.Format("{0:#,0.00}", Convert.ToDouble(Eval("Per91Days"))))  :  "--" : "--") %>' />
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label8" runat="server" Text='<%#(Eval("Per182Days") != DBNull.Value ? !string.IsNullOrEmpty(Eval("Per182Days").ToString()) ?(String.Format("{0:#,0.00}", Convert.ToDouble(Eval("Per182Days"))))  :  "--" : "--")%>' />
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label9" runat="server" Text='<%#(Eval("Per1Year") != DBNull.Value ? !string.IsNullOrEmpty(Eval("Per1Year").ToString()) ?(String.Format("{0:#,0.00}", Convert.ToDouble(Eval("Per1Year"))))  :  "--" : "--") %>' />
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label10" runat="server" Text='<%#(Eval("Per3Year") != DBNull.Value ? !string.IsNullOrEmpty(Eval("Per3Year").ToString()) ?(String.Format("{0:#,0.00}", Convert.ToDouble(Eval("Per3Year"))))  :  "--" : "--")%>' />
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label7" runat="server" Text='<%#(Eval("Per5Year") != DBNull.Value ? !string.IsNullOrEmpty(Eval("Per5Year").ToString()) ?(String.Format("{0:#,0.00}", Convert.ToDouble(Eval("Per5Year")))) :  "--" : "--")%>' />
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label5" runat="server" Text='<%#Eval("SI") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                                                    </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row pt-3">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>NAV Graph</h6>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div id="DivPlotChart" runat="server">
                                            <!-- block -->
                                            <div style="margin-top: 20px; margin-right: 25px; margin-left: -11px; display: none">
                                                <div id="divTimePeriod" runat="server" visible="false">

                                                    <div class="span3">
                                                        <div style="margin-top: 10px">
                                                            <asp:RadioButton ID="rbTime" CssClass="controls" runat="server"
                                                                GroupName="Time" Checked="true" />
                                                        </div>
                                                        <div class="controls" style="margin-top: -25px; margin-left: 30px;">
                                                            <p class="span5 lebel-drop">Time</p>
                                                            <asp:DropDownList ID="ddlTime" runat="server">
                                                                <asp:ListItem Value="7">7 Days</asp:ListItem>
                                                                <asp:ListItem Value="30">1 Month</asp:ListItem>
                                                                <asp:ListItem Value="90">3 Months</asp:ListItem>
                                                                <asp:ListItem Value="182">6 Months</asp:ListItem>
                                                                <asp:ListItem Value="365" Selected="True">1 Year</asp:ListItem>
                                                                <asp:ListItem Value="1095">3 Years</asp:ListItem>
                                                                <asp:ListItem Value="1825">5 Years</asp:ListItem>
                                                                <asp:ListItem Value="3650">10 Years</asp:ListItem>
                                                                <asp:ListItem Value="5471">15 Years</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div>

                                                        <div style="margin-top: 10px">
                                                            <asp:RadioButton ID="rbDateRange" runat="server" GroupName="Time" />
                                                        </div>

                                                        <div class="controls" style="margin-top: -25px; margin-left: 30px;">
                                                            <div class="lebel-drop">From Date</div>
                                                            <asp:TextBox ID="txtfromDate" runat="server" Style="margin-top: -32px; margin-left: 80px; width: 50%;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div style="margin-left: 55px;">

                                                        <div class="controls" style="margin-top: 5px; margin-left: 30px;">
                                                            <div class="lebel-drop">To Date</div>
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="value_txtbox"
                                                                Style="margin-top: -32px; margin-left: 80px; width: 50%;"></asp:TextBox>


                                                        </div>

                                                    </div>

                                                </div>

                                            </div>
                                            <!-- /block -->
                                            <%--<asp:Button ID="btnPlotChart" runat="server" Text="Plot Chart" class="btn-sub btn-large" />--%>
                                            <%-- <input type="button" value="Plot Chart" class="btn-sub btn-large" id="btplotChart" style="margin-right: 33px;" onclick="Javascript: btnPlotclick();" />--%>
                                        </div>
                                        <div id="DivLast" runat="server" style="display: none">
                                            <div>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <%--height: 500px;--%>
                                                                    <div style="width: 100%;" id="chartContainer">
                                                                        <div style="width: 100%; height: 100%;" id="divChart">
                                                                        </div>
                                                                    </div>

                                                                    <%--new chart--%>
                                                                    <div id="HighContainer">
                                                                    </div>

                                                                </div>
                                                                <div style="text-align: left; font-size: 12px; color: #0c4466;">
                                                                    <span id="infoChart">* The above graph represents Fund
                                                                                    vs Benchmark NAV movement
                                                                    </span>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <div id="plotted_image_div">
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
                        </div>
                    </div>
                </div>
                <div class="row pt-3">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>Top 10 Holdings</h6>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <asp:Repeater ID="TopCompDetails" runat="server">
                                            <HeaderTemplate>
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th style="text-align: left">Company Name </th>
                                                            <th style="text-align: center">Sector Name</th>
                                                            <th style="text-align: right; border-right: none; white-space: nowrap">Asset %</th>
                                                            <th style="border-left: none;">&nbsp;</th>
                                                        </tr>
                                                    </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: justify">
                                                        <asp:Label ID="lblSubject" runat="server" Style="font-weight: bold;"
                                                            Text='<%#Eval("CompName") %>' />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                    </td>
                                                    <td style="text-align: center; border-right: none; vertical-align: middle;">
                                                        <div class="dt0la0-5 eKZcU">
                                                            <div height="10" class="animate-on-scroll sc-1842aaf-0 dAMnLF">
                                                                <div id="dvBarWidth" style="color: #ee9c16; width: 24%"
                                                                    height="10" class="sc-1842aaf-1 eQZPnL" runat="server">
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </td>
                                                    <td style="border-left: none">
                                                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("Net_Asset") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <asp:Repeater ID="TopCompDetails1" runat="server">
                                            <HeaderTemplate>
                                                <table width="100%" border="0" align="left" cellpadding="0"
                                                    cellspacing="0" class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th style="text-align: left">Company Name
                                                            </th>
                                                            <th style="text-align: center">Sector Name
                                                            </th>
                                                            <th style="text-align: right; border-right: none; white-space: nowrap">Asset %</th>
                                                            <th style="border-left: none;">&nbsp;</th>
                                                        </tr>
                                                    </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%--<tr>
                                                                                    <td>
                                                                                        HDFC Bank Ltd.
                                                                                    </td>
                                                                                    <td>
                                                                                        Financial Services
                                                                                    </td>
                                                                                    <td style="text-align:right;">
                                                                                        9.8
                                                                                    </td>
                                                                                </tr>--%>
                                                <tr>
                                                    <td style="text-align: justify">
                                                        <asp:Label ID="Label4" runat="server" Style="font-weight: bold"
                                                            Text='<%#Eval("CompName") %>' />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="Label7" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                    </td>
                                                    <td style="text-align: center; border-right: none; vertical-align: middle;">
                                                        <div class="dt0la0-5 eKZcU">
                                                            <div height="10" class="animate-on-scroll sc-1842aaf-0 dAMnLF">
                                                                <div id="dvBarWidth1" style="color: #ee9c16; width: 24%"
                                                                    height="10" class="sc-1842aaf-1 eQZPnL" runat="server">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="border-left: none; vertical-align: middle;">
                                                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("Net_Asset") %>' />
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
                <div class="row pt-3">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>Statistical Analysis</h6>
                            </div>
                            <div class="card-body pt-3">
                                <div class="category-style-03 text-center justify-content-center" style="text-align: center">
                                    <div class="col-xs-12 col-sm-12 col-md-2 col-lg-2">
                                        <div class="card text-center">
                                            <div class="card-header" style="background: #fff; background: #fff; letter-spacing: 1px; padding: 0.75rem 0.75rem;">
                                                <h6 style="font-family: 'Nunito Sans', sans-serif !important; font-size: 1em; color: #273a93; font-weight: bold;">Sharpe</h6>
                                            </div>
                                            <div class="card-body bg-grey">
                                                <label id="lblSharpe" runat="server"></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-2 col-lg-2">
                                        <div class="card text-center">
                                            <div class="card-header" style="background: #fff; background: #fff; letter-spacing: 1px; padding: 0.75rem 0.75rem;">
                                                <h6 style="font-family: 'Nunito Sans', sans-serif !important; font-size: 1em; color: #273a93; font-weight: bold;">Sortino</h6>
                                            </div>
                                            <div class="card-body bg-grey">
                                                <label id="lblSortino" runat="server"></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4">
                                        <div class="card text-center">
                                            <div class="card-header" style="background: #fff; background: #fff; letter-spacing: 1px; padding: 0.75rem 0.75rem;">
                                                <h6 style="font-family: 'Nunito Sans', sans-serif !important; font-size: 1em; color: #273a93; font-weight: bold;">Standard Deviation</h6>
                                            </div>
                                            <div class="card-body bg-grey">
                                                <label id="lblSdv" runat="server"></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-2 col-lg-2">
                                        <div class="card text-center">
                                            <div class="card-header" style="background: #fff; background: #fff; letter-spacing: 1px; padding: 0.75rem 0.75rem;">
                                                <h6 style="font-family: 'Nunito Sans', sans-serif !important; font-size: 1em; color: #273a93; font-weight: bold;">Beta</h6>
                                            </div>
                                            <div class="card-body bg-grey">
                                                <label id="lblBeta" runat="server"></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-2 col-lg-2">
                                        <div class="card text-center">
                                            <div class="card-header" style="background: #fff; background: #fff; letter-spacing: 1px; padding: 0.75rem 0.45rem;">
                                                <h6 style="font-family: 'Nunito Sans', sans-serif !important; font-size: 1em; color: #273a93; font-weight: bold;">R-Square</h6>
                                            </div>
                                            <div class="card-body bg-grey">
                                                <label id="lblRSqure" runat="server"></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row pt-3">
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>Top 5 Sector Holding</h6>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <asp:Repeater ID="RepTopSector" runat="server">
                                            <HeaderTemplate>
                                                <table width="100%" border="0" align="center" cellpadding="0"
                                                    cellspacing="0" class="table table-bordered">
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
                                                <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                    <td>
                                                        <asp:Label ID="lblSectorName" runat="server" Style="font-weight: bold"
                                                            Text='<%#Eval("Sector_Name") %>' />
                                                    </td>
                                                    <td style="text-align: center; border-right: none; vertical-align: middle;">
                                                        <div class="dt0la0-5 eKZcU">
                                                            <div height="10" class="animate-on-scroll sc-1842aaf-0 dAMnLF">
                                                                <div id="dvBarWidthSect" style="color: #ee9c16; width: 14%"
                                                                    height="10" class="sc-1842aaf-1 eQZPnL" runat="server">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="border-left: none">
                                                        <asp:Label ID="Label9" runat="server" Text='<%#Eval("Scheme_Per") %>' />
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
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>
                                    <asp:Label runat="server" ID="TdMc"></asp:Label>
                                </h6>
                            </div>
                            <div class="card-body">
                                <div class="">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                        <div style="float: left; height: 200px; margin-top: 20px; text-align: center; width: 98%"
                                            id="dvMarketCapContaner">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row pt-3">
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>AUM Movement</h6>
                            </div>
                            <div class="card-body">
                                <div class="">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                        <div style="width: 100%; height: 220px;" id="chart1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <div class="card">
                            <div class="card-header" style="background: #fff">
                                <h6>Asset Allocation</h6>
                            </div>
                            <div class="card-body">
                                <div class="">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div id="chartAssetAllocationContainer" style="height: 220px; font-size: 10px; width: 100%">
                                        </div>
                                    </div>
                                </div>
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
    </form>

</body>
</html>
