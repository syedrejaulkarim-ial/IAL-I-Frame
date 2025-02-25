<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Portfolio.aspx.cs" Inherits="iFrames.MutualFundIndia.Portfolio" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <title>Portfolio</title>
    <meta name="viewport" content="width=device-width,initial-scale=1">

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="../Styles/jquery.jqplot.css" rel="stylesheet" type="text/css" />

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
            var a = $("#hdSchemeId").val();
            if ($("#hdSchemeId").val() != null && $("#hdSchemeId").val() != 0) {
                getGraphs($("#hdSchemeId").val());
                GetChart($("#hdSchemeId").val());
            }
        });

        function getGraphs(schid) {
            //btnPlotclick();
            //PullChart(schid);
            PullChartAssetAllocation(schid);
            PlotMktValue(schid);
        }

        function GetChart(schid) {

            var dataToPush = JSON.stringify({
                SchId: schid
            });

            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                url: 'Portfolio.aspx/McapAndAvgMat',
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

                    StyleBox(dataConsolidated.d.StyleBoxImgName);
                },
                error: function (data, e) {
                    alert("Something went wrong..!!");
                }
            });
        }

        function StyleBox(data) {
            if (data == null || data.length < 1) {
                $('#DivMcapLabel').empty();
                $('#DivCreditQualityLabel').empty();
                $('#dvStyleBox').prepend('Data not available for the selected scheme</div>');
                $('#dvStyleBox').effect("highlight", {}, 3000);
                $("#SpanLowercaption4StyleBox").html("");
                return;
            }
            $('#dvStyleBox').prepend('<img id="theImg" src="Images/FundStyleBox/'
                + data
                + '" />');
        }

        function PullChartAssetAllocation(schid) {
            var dataToPush = JSON.stringify({
                schemeIds: schid
            });
            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                url: 'Portfolio.aspx/assetAllocaton',
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
                    y: 8,
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
                url: 'Portfolio.aspx/portfolioMKT_Val',
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
            dataToInsert.name = "Asset Allocation";
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
                        size: 128,
                        cursor: 'pointer',
                        depth: 25,
                        dataLabels: {
                            enabled: true,
                            distance: 5,
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
                    itemStyle: {
                        fontSize: '11px',
                    },
                    y: 4,
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
                    symbolRadius: 0,
                    symbolHeight: 0,
                },
                xAxis: {
                    categories: ticks
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
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

    <script type="text/javascript" language="javascript">
        function ShowMoreButton() {
            var elem = document.getElementById('<%=btnShowMoreButton.ClientID%>');
            var table = document.getElementById("tblPort");
            var tbodyRowCount = table.tBodies[0].rows.length;

            if (elem.value == "Detailed Portfolio") {
                for (var i = 11, row; row = table.tBodies[0].rows[i]; i++) {
                    $(table.tBodies[0].rows[i]).removeClass('dspNone');
                }
                elem.value = "Show Less";
            }
            else if (elem.value == "Show Less") {
                for (var i = 11, row; row = table.tBodies[0].rows[i]; i++) {
                    $(table.tBodies[0].rows[i]).addClass('dspNone');
                }
                elem.value = "Detailed Portfolio";
            }
            return false;
        }
    </script>

    <style>
        .dspNone {
            display: none;
        }
        h6 {
            font-size: 1.2em;
            line-height:1em;
            margin-bottom: 0px;
        }
    </style>

</head>

<body>
    <form id="form1" runat="server">
        <div class="col-lg-9">
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
                <div class="card-body" runat="server" visible="false" id="dvPortFolio">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <h3 style="margin-top: 10px;">
                                <asp:Label runat="server" ID="lblFundNameBold"></asp:Label>
                            </h3>
                            <span class="badge badge-soft-inverse">
                                <asp:Label runat="server" ID="lblCurrNav"></asp:Label>
                                <img id="imgArrow" src="img/arwup.png" class="pt-0" runat="server" />
                                <asp:Label runat="server" ID="lblIncrNav"></asp:Label>
                            </span>
                            <em>1 day change as on
                                        <asp:Label runat="server" ID="lblCurrNavDate"></asp:Label></em>
                        </div>

                    </div>
                    <div class="row pt-3 pb-3">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="card">
                                <div class="card-header" style="background: #fff">
                                    <h6>Top Holding <small>(<asp:Label runat="server" ID="lblPortDate"></asp:Label>)</small></h6>
                                </div>
                                <div class="card-body">
                                    <div class="">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <asp:Repeater ID="TopCompDetails" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered" id="tblPort">

                                                        <tbody>
                                                            <tr>
                                                                <th style="text-align: left">Company Name
                                                                </th>
                                                                <th style="text-align: left">Asset Type
                                                                </th>
                                                                <th style="text-align: right">Percentage Allocation
                                                                </th>
                                                            </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%-- <tr>
                                                                                <td>
                                                                                    <span id="TopCompDetails1_ctl01_Label4">9.95 Food Corporation of India Mar 7 2022</span>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="TopCompDetails1_ctl01_Label7">Debt</span>
                                                                                </td>
                                                                                <td style="text-align: right">
                                                                                    <span id="TopCompDetails1_ctl01_Label8">7.25 %</span>
                                                                                </td>
                                                                                <td style="border: none; display: none">
                                                                                    <input type="hidden" id="hdSchemeId" value="0" runat="server" />
                                                                                </td>
                                                                            </tr>--%>
                                                    <tr id="cmpIdData" runat="server" class="dspNone">
                                                        <td>
                                                            <span id="TopCompDetails1_ctl01_Label4">
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("CompName") %>' /></span>
                                                        </td>
                                                        <td>
                                                            <span id="TopCompDetails1_ctl01_Label7">
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("Sector_Name") %>' /></span>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <span id="TopCompDetails1_ctl01_Label8">
                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Net_Asset") %>' /></span>
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
                    <asp:Button ID="btnShowMoreButton" runat="server" CssClass="btn btn-light btn-sm" BackColor="#db3539" ForeColor="White" BorderStyle="Inset" Text="Detailed Portfolio" Visible="false" OnClientClick="return ShowMoreButton(); return false;" />
                    <%--OnClick="ShowMoreButton_Click" --%>
                    <div class="row pt-3">
                        <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-header" style="background: #fff">
                                    <h6>Top Sector Holding</h6>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <asp:Repeater ID="RepTopSector" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">

                                                        <tbody>
                                                            <tr>
                                                                <th style="text-align: left">Sector Name
                                                                </th>
                                                                <th style="text-align: right; border-right: none">Percentage Allocation
                                                                </th>
                                                                <th style="border-left: none;">&nbsp;</th>
                                                            </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="font-weight: bold">
                                                            <span id="TopCompDetails1_ctl01_Label4">
                                                                <asp:Label ID="lblSectorName" runat="server" Text='<%#Eval("Sector_Name") %>' /></span>
                                                        </td>
                                                        <td style="text-align: center; border-right: none; vertical-align: middle;">
                                                            <div class="dt0la0-5 eKZcU">
                                                                <div height="10" class="animate-on-scroll sc-1842aaf-0 dAMnLF">
                                                                    <div id="dvBarWidthSect" style="color: #ee9c16; width: 24%" height="10" class="sc-1842aaf-1 eQZPnL" runat="server"></div>
                                                                </div>

                                                            </div>
                                                        </td>
                                                        <td style="border-left: none; text-align: center">
                                                            <asp:Label ID="Label9" runat="server" Text='<%#Eval("Scheme_Per") %>' />
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
                        <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-header" style="background: #fff">
                                    <h6>Asset Allocation</h6>
                                </div>
                                <div class="card-body">
                                    <div class="">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div style="float: left; height: 200px; margin-top: 20px; text-align: center; width: 98%"
                                                id="chartAssetAllocationContainer">
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
                                    <h6>
                                        <asp:Label runat="server" ID="TdMc"></asp:Label></h6>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div style="float: left; height: 200px; margin-top: 20px; text-align: center; width: 98%"
                                                id="dvMarketCapContaner">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-header" style="background: #fff">
                                    <h6>Style Box</h6>
                                </div>
                                <div class="card-body">
                                    <div class="">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-28" id="dvStyleBox" style="text-align: center;">
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
                                    <h6>Credit Rating</h6>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <asp:Repeater ID="RepeaterRate" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                        <tbody>
                                                            <tr>
                                                                <th style="text-align: left">Rating Name
                                                                </th>
                                                                <th style="text-align: center; border-right: none">Percentage Allocation
                                                                </th>
                                                                <%--<th style="border-left: none;">&nbsp;</th>--%>
                                                            </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <span id="TopCompDetails1_ctl01_Label4">
                                                                <asp:Label ID="lblSectorName" runat="server" Text='<%#Eval("DataHead") %>' /></span>
                                                        </td>
                                                        <%--<td style="text-align: center; border-right: none; vertical-align: middle;">
                                                                                    <div class="dt0la0-5 eKZcU">
                                                                                        <div height="10" class="animate-on-scroll sc-1842aaf-0 dAMnLF">
                                                                                            <div style="color: #ee9c16; width: 24%" height="10" class="sc-1842aaf-1 eQZPnL"></div>
                                                                                        </div>

                                                                                    </div>
                                                                                </td>--%>
                                                        <td style="border-left: none; text-align: center">
                                                            <asp:Label ID="Label4" runat="server" Text='<%#Eval("Data") %>' />%
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
                        <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-header" style="background: #fff">
                                    <h6>Instrument Breakup</h6>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <asp:Repeater ID="RepeaterInst" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                        <tbody>
                                                            <tr>
                                                                <th style="text-align: left; white-space: nowrap">Instrument Name
                                                                </th>
                                                                <th style="text-align: center; /*border-right: none; */ white-space: nowrap">Percentage Allocation
                                                                </th>
                                                                <%-- <th style="border-left: none;">&nbsp;</th>--%>
                                                            </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <span id="TopCompDetails1_ctl01_Label4">
                                                                <asp:Label ID="lblSectorName" runat="server" Text='<%#Eval("DataHead") %>' /></span>
                                                        </td>
                                                        <%-- <td style="text-align: center; border-right: none; vertical-align: middle;">
                                                                                    <div class="dt0la0-5 eKZcU">
                                                                                        <div height="10" class="animate-on-scroll sc-1842aaf-0 dAMnLF">
                                                                                            <div style="color: #ee9c16; width: 24%" height="10" class="sc-1842aaf-1 eQZPnL"></div>
                                                                                        </div>

                                                                                    </div>
                                                                                </td>--%>
                                                        <td style="border-left: none; text-align: center">
                                                            <asp:Label ID="Label4" runat="server" Text='<%#Eval("Data") %>' />%
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
                                    <h6>Other Parameters</h6>
                                </div>
                                <div class="card-body">
                                    <div class="">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <asp:Repeater ID="ExtraData" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <th style="text-align: left">
                                                            <asp:Label ID="lblSectorName" runat="server" Text='<%#Eval("ColName") %>' />
                                                        </th>
                                                        <td style="text-align: left">
                                                            <asp:Label ID="Label5" runat="server" Text='<%#Eval("ColDate") %>' />
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="Label6" runat="server" Text='<%#Eval("ColData") %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
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
                    </div>
                </div>
            </div>
        </div>

        <div style="width: 0%; display: none">
            <input type="hidden" id="hdSchemeId" value="0" runat="server" style="visibility: hidden" />
        </div>
    </form>
</body>
</html>
