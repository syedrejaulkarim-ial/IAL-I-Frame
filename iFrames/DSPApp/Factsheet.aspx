<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always"
    EnableViewStateMac="true" CodeBehind="Factsheet.aspx.cs" Inherits="iFrames.DSPApp.Factsheet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Factsheet</title>
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.min.js"></script>
    <![endif]-->
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script>
    <![endif]-->
    <script type='text/javascript' src="js/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <script type="text/javascript" src="js/date.js"></script>
    <script src="js/AutoComplete.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/jquery-slider.css" />

    <link href='http://fonts.googleapis.com/css?family=Roboto' rel='stylesheet' type='text/css' />
    <link rel="stylesheet" href="font-awesome-4.5.0/css/font-awesome.min.css" />
    <link href='http://fonts.googleapis.com/css?family=Shadows+Into+Light' rel='stylesheet' type='text/css' />
    <!--bootstrap-->
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <link href="css/ie10-viewport-bug-workaround.css" rel="stylesheet" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <!--core css-->
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <!--responsive css-->
    <link href="css/responsive.css" type="text/css" rel="stylesheet" />


    <script src="js/modernizr-2.6.2-respond-1.1.0.min.js" type="text/javascript"></script>
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
        $(function () {
            //$.jqplot.config.enablePlugins = true;
            //$("#listboxSchemeName").change(function () {
            //    getGraphs($("#listboxSchemeName").val());
            //});
            if (($("#listboxSchemeName").val() > 0) && ($("#listboxSchemeName").val() != 'null')) {
                getGraphs($("#listboxSchemeName").val());
            };
            if ($("#hdSchemeId").val() > 0) {
                getGraphs($("#hdSchemeId").val());
            }
        });

        function getGraphs(schid) {
            //  PullChart(schid);
            PullChartAssetAllocation(schid);
            PlotMktValue(schid);
            GetChart(schid);
            GetExpense(schid);
            GetCrdtRatInsBreakup(schid);
        }


        function GetCrdtRatInsBreakup(schid) {

            var dataToPush = JSON.stringify({
                schemeIds: schid
            });

            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                url: 'Factsheet.aspx/CreditRatingInsBreakup',
                type: 'POST',
                contentType: "application/json; charset=utf-8",

                success: function (dataConsolidated) {
                    //debugger;
                    if (dataConsolidated.d == null) {
                        return;
                    }
                    if (dataConsolidated.d == 0) {
                        return;
                    }
                    //dvCreditRating
                    var HtmlExp = "<table class='table table-bordered'><tr style='background:#f6f4f4'><th class='value_tableheader' style='text-align:center'>Rating Name</th><th class='value_tableheader' style='text-align:center'>Percentage Allocation</th></tr>";
                    var htmlDateTr = "";

                    HtmlExp = HtmlExp + htmlDateTr;
                    htmlDateTr = "";
                    for (var i = 0; i < dataConsolidated.d.LstCreditrating.length; i += 1) {
                        htmlDateTr = htmlDateTr + "<tr><td align='center'>" + dataConsolidated.d.LstCreditrating[i].DataHead + "</td><td align='center'>" + dataConsolidated.d.LstCreditrating[i].Data + "</td></tr>";
                    }
                    HtmlExp = HtmlExp + htmlDateTr + "</table>";
                    $("#dvCreditRating").html(HtmlExp);


                    //--------ins break

                    HtmlExp = "<table class='table table-bordered'><tr style='background:#f6f4f4'><th class='value_tableheader' style='text-align:center'>Instrument Name</th><th class='value_tableheader' style='text-align:center'>Percentage Allocation</th></tr>";
                    htmlDateTr = "";

                    HtmlExp = HtmlExp + htmlDateTr;
                    htmlDateTr = "";
                    for (var i = 0; i < dataConsolidated.d.LstInsBreakup.length; i += 1) {
                        htmlDateTr = htmlDateTr + "<tr><td align='center'>" + dataConsolidated.d.LstInsBreakup[i].DataHead + "</td><td align='center'>" + dataConsolidated.d.LstInsBreakup[i].Data + "</td><tr>";
                    }
                    HtmlExp = HtmlExp + htmlDateTr + "</table>";
                    $("#dvInsBreak").html(HtmlExp);
                },
                error: function (data) {
                    alert(data);
                }
            });
        }
        function GetExpense(schid) {

            var dataToPush = JSON.stringify({
                schemeIds: schid
            });

            $.ajax({
                cache: false,
                data: dataToPush,
                dataType: "json",
                //url: 'https://www.mutualfundindia.com/MF/Factsheet/Details/Factsheet.aspx/ExpanseRation',
                url: 'Factsheet.aspx/ExpanseRatio',
                type: 'POST',
                contentType: "application/json; charset=utf-8",

                success: function (dataConsolidated) {
                    // debugger;
                    if (dataConsolidated.d == null) {
                        return;
                    }
                    if (dataConsolidated.Value == 0) {
                        return;
                    }
                    var HtmlExp = "<h4>Expense Ratio</h4><table class='table table-bordered'><tr style='background:#f6f4f4'>";
                    var htmlDateTr = "";
                    for (var i = 0; i < dataConsolidated.d.length; i += 1) {
                        htmlDateTr = htmlDateTr + "<th class='value_tableheader' style='text-align:center'>" + dataConsolidated.d[i].StrDate + "</th>";
                    }
                    HtmlExp = HtmlExp + htmlDateTr + "</tr><tr>";
                    htmlDateTr = "";
                    for (var i = 0; i < dataConsolidated.d.length; i += 1) {
                        htmlDateTr = htmlDateTr + "<td align='center'>" + dataConsolidated.d[i].StrValue + "</td>";
                    }
                    HtmlExp = HtmlExp + htmlDateTr + "</tr></table>";
                    $("#collExpRatio").html(HtmlExp);
                },
                error: function (e,data) {
                    alert(data);
                }
            });
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
                TextVal.push(data[i].NatureName + "&nbsp");
                TextVal.push(data[i].Value);
                dataPlot.push(TextVal);
            }

            dataPlot.shift();
            var plot1 = $.jqplot('chartAssetAllocationContainer', [dataPlot], {
                seriesColors: ["#da251d", "#ed9c54", "#605d5c", "#e6ab0d", "#49b959"],
                gridPadding: { top: 0, bottom: 0, left: 0, right: 0 },
                seriesDefaults: {
                    renderer: jQuery.jqplot.PieRenderer,
                    rendererOptions: {
                        showDataLabels: true,
                        dataLabelFormatString: '%.2f%%',
                        dataLabels: 'value',
                        dataLabelThreshold: 0,
                        dataLabelPositionFactor: 1.4,
                    },
                    shadow:false,
                },
                legend: {
                    show: true,
                    location: 's',
                    fontFamily: 'Helvetica,Arial,sans-serif',
                    fontSize: '12px',
                    rendererOptions: {
                        numberRows: 1,
                        numberColumns: 3
                    },
                    marginTop: '0px',
                    border: '0px solid white',
                    //marginButton: '0px',

                    //labels: ['Equity', 'Debt', 'Others']
                },
                grid: {
                    drawGridlines: false,
                    borderColor: 'transparent',
                    shadow: false,
                    drawBorder: false,
                    shadowColor: 'transparent',
                    background: 'transparent',
                    fontFamily: 'Open Sans,sans-serif',
                }
            });
        }


        //function charttest() {
        //    var data = JSON.parse('{"d":{"__type":"iFrames.BLL.ChartNavReturnModel","getSimpleNavReturnModel":[{"Name":"BNP Paribas Dividend Yield Fund - Growth","Id":0,"IsIndex":false,"ValueAndDate":[{"Date":"11/13/2013","Value":100},{"Date":"11/14/2013","Value":100.62578222778473}],"MaxDate":null,"MinDate":null},{"Name":"S\u0026P BSE Sensex","Id":0,"IsIndex":false,"ValueAndDate":[{"Date":"11/13/2013","Value":100},{"Date":"11/14/2013","Value":"NaN"}],"MaxDate":null,"MinDate":null}],"MaxDate":"11/14/2013","MinDate":"11/13/2013","MaxVal":100.62578222778473,"MinVal":0}}');
        //    }

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
                    alert(data);
                }
            });
        }

        function Plot(dataConsolidated) {
            var RowCnt = 1;
            var ColCnt = 2;
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                RowCnt = 2;
                ColCnt = 1;
            }

            var max = dataConsolidated.MaxDate;
            var min = dataConsolidated.MinDate;
            var data = dataConsolidated.SimpleNavReturnModel;
            var seriesNames = Array();
            var dataPlot = [[[]]];
            for (var i = 0; i < data.length; i += 1) {
                seriesNames.push("&nbsp;" + data[i].Name + "&nbsp;");
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
                            numberRows: RowCnt,
                            numberColumns: ColCnt
                        },
                        placement: 'outsideGrid',
                        seriesToggle: 'on',
                        fontSize: '1em',
                        border: '0px solid black',
                        fontFamily: 'Arial,sans-serif,Helvetica',
                        marginLeft: 10,
                        border: '0px solid white',
                        grid: {
                            shadow: false,
                            borderWidth: 0,
                            background: 'rgba(0,0,0,0)',
                            marginLeft: 10,
                        }
                    },
                    grid: {
                        shadow: false,
                        borderWidth: 0,
                        background: 'rgba(0,0,0,0)',
                        marginLeft: 10,
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
                    PlotMktChart(dataConsolidated.d);
                },
                error: function (data, e) {
                    alert(data);
                }
            });
        }

        function PlotMktChart(data) {
            //debugger;
            var BarMargin = 50;
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                // alert("tst");
                BarMargin = 5;
                // $("#MfIframeQA").css('overflow', 'scroll');
            }
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
                seriesColors: ["#da251d", "#ed9c54", "#605d5c", "#e6ab0d", "#49b959", "#ed496c", "#e87500", "#4f8ad3"],
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    rendererOptions: { barMargin: BarMargin },
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
                        MarketCap(dataConsolidated.d);
                    }
                    else {
                        $("#TdMc").html("Average Maturity");

                        AverageMaturity(dataConsolidated.d);
                    }
                    if (dataConsolidated.d.isSchemeEquity == false) {
                        $("#DivCreditQualityLabel").show();
                        $("#DivMcapLabel").hide();
                        $("#SpanLowercaption4StyleBox").html("Average Maturity");

                    }
                    else {
                        $("#DivCreditQualityLabel").hide();
                        $("#DivMcapLabel").show();
                        $("#SpanLowercaption4StyleBox").html("P/E");

                    }
                    StyleBox(dataConsolidated.d.StyleBoxImgName);
                },
                error: function (data, e) {
                    alert(data);
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
            $('#dvStyleBox').prepend('<img id="theImg" src="../Images/FundStyleBox/'
                                     + data
                                     + '" />');
        }

        function MarketCap(dataAll) {
            var BarMargin = 35;
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                BarMargin = 5;
            }
            var data = dataAll.LstMarketCap;
            if (data == null || data.length < 1) {
                $('#dvMarketCapContaner').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="dvMarketCap">Data not available for the selected scheme</div>');
                $('#dvMarketCapContaner').effect("highlight", {}, 3000);
                return;
            }
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
                seriesColors: ["#da251d"],
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    pointLabels: { show: true, location: 'e', edgeTolerance: -15 },
                    rendererOptions: {
                        barDirection: 'horizontal',
                        //dataLabels: 'value',
                        //dataLabelFormatString: '%.4f',

                        fillToZero: true,
                        barMargin: BarMargin
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
                        min: 0
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

        function AverageMaturity(dataAll) {
            data = dataAll.LstAverageMaturity;
            // $('#dvAvgMaturity').remove();
            if (data == null || data.length < 1) {
                $('#dvMarketCapContaner').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="dvAvgMaturity">Data not available for the selected scheme</div>');
                $('#dvMarketCapContaner').effect("highlight", {}, 3000);
                return;
            }
            //$('#dvMarketCapContaner').append('<div style="width: 97%; height:100%;" id="dvAvgMaturity" ></div>');

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
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="am-wrapper">
            <nav class="navbar navbar-default navbar-fixed-top am-top-header">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <div class="page-title"><span>Dashboard</span></div>
                        <a href="#" class="am-toggle-left-sidebar navbar-toggle collapsed"><span class="icon-bar"><span></span><span></span><span></span></span></a><a href="#" class="navbar-brand"></a>
                    </div>
                    <a href="#" class="am-toggle-right-sidebar"><span class="icon s7-menu2"></span></a><a href="#" data-toggle="collapse" data-target="#am-navbar-collapse" class="am-toggle-top-header-menu collapsed"><i class="fa fa-angle-down"></i></a>
                    <div id="am-navbar-collapse" class="collapse navbar-collapse">
                        <ul style="display: none;" class="nav navbar-nav navbar-right am-user-nav">
                            <li class="dropdown">
                                <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle">Samantha.amaretti@gmail.com
              
             <i class="fa fa-angle-down"></i></a>
                                <ul role="menu" class="dropdown-menu">
                                    <li><a href="forgetpaswrd.html"><span class="icon-key"></span>Reset Password</a></li>

                                    <li><a href="#"><span class="icon-logout"></span>Sign Out</a></li>
                                </ul>
                            </li>
                        </ul>
                        <!--<ul class="nav navbar-nav am-nav-right">
              <li><a href="#">Home</a></li>
              <li><a href="#">About</a></li>
              <li class="dropdown"><a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle">Services <span class="angle-down s7-angle-down"></span></a>
                <ul role="menu" class="dropdown-menu">
                  <li><a href="#">UI Consulting</a></li>
                  <li><a href="#">Web Development</a></li>
                  <li><a href="#">Database Management</a></li>
                  <li><a href="#">Seo Improvement</a></li>
                </ul>
              </li>
              <li><a href="#">Support</a></li>
            </ul>-->

                    </div>
                </div>
            </nav>
            <div class="am-left-sidebar">
                <div class="content">
                    <div class="am-logo"></div>
                    <ul style="display: none;" class="sidebar-elements">
                        <li class="parent"><a href="#"><i class="icon-user"></i><span>User Management</span></a>
                            <ul class="sub-menu">
                                <li style="list-style-type: none;"><a href="index.html">User Creation</a>
                                </li>
                                <li class="active" style="list-style-type: none;"><a href="active.html">Active/Inactive Login</a>
                                </li>
                            </ul>
                        </li>
                        <li class="parent"><a href="#"><i class="icon-magnifier"></i><span>Return Analysis</span></a>
                            <ul class="sub-menu">
                                <li style="list-style-type: none;"><a href="upload.html">Upload Excel File </a>
                                </li>
                                <li style="list-style-type: none;"><a href="viewreport.html">View Report</a>
                                </li>
                            </ul>
                        </li>

                    </ul>
                    <!--Sidebar bottom content-->
                </div>
            </div>
            <div class="am-content">
                <div class="page-head">
                    <h3>View Report </h3>
                    <ol class="breadcrumb">
                        <li><a href="#">Return Analysis</a></li>
                        <li class="active.html">Factsheet</li>
                    </ol>
                </div>
                <div class="main-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default panel-heading-fullwidth panel-borders">
                                <div class="panel-heading">
                                    <h3 style="font-size: 22px;"><strong>Factsheet </strong></h3>
                                    <span id="dvTest" runat="server"></span>
                                </div>
                                <div class="panel-body">
                                    <div style="padding-left: 0px; margin-left: 0px; padding-right: 0px; margin-right: 0px">
                                        <div class="col-sm-12 col-xs-12 col-md-12">
                                            <div class="sip-left">
                                                <div class="calculator">
                                                    <div class="form-area" style="padding-left: 0px; margin-left: 0px; padding-right: 0px; margin-right: 0px; display: none">
                                                        <div class="form-group col-sm-8 col-xs-12" style="padding-left: 0px; margin-left: 0px; padding-right: 0px; margin-right: 0px">
                                                            <asp:DropDownList ID="ddlFundHouse" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged" runat="server" class="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group col-sm-8 col-xs-12" style="padding-left: 0px; margin-left: 0px; padding-right: 0px; margin-right: 0px">
                                                            <asp:DropDownList ID="listboxSchemeName" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged" runat="server" class="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group col-sm-8 col-xs-12" style="padding-left: 0px; margin-left: 0px; padding-right: 0px; margin-right: 0px">
                                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary pull-right" OnClick="btnSubmit_Click" />
                                                        </div>
                                                    </div>

                                                    <div class="sip-left col-sm-12 col-xs-12 col-md-12" id="tblResult_div" runat="server" visible="false" style="padding-left: 0px; margin-left: 0px; padding-right: 0px; margin-right: 0px">
                                                        <h3><strong>
                                                            <asp:Label runat="server" ID="lblFundName"></asp:Label></strong></h3>
                                                        <span>&nbsp;<asp:Label runat="server" ID="lblPresentNav"></asp:Label>&nbsp;<img id="imgArrow"
                                                            src="img/arw_red.png" runat="server" visible="false" />
                                                            <asp:Label runat="server" ID="lblIncrNav"></asp:Label></span><br />

                                                        <div class="table-responsive">
                                                            <h4>Investment Info</h4>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table table-striped table-bordered">
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="3"><strong>Investment Objective</strong></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" runat="server" id="tdObjtive"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><strong>Fund Type</strong></td>
                                                                        <td><strong>Entry Load</strong></td>
                                                                        <td><strong>Average Asset Size (Rs cr.)</strong></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdFundType" runat="server"></td>
                                                                        <td id="tdEntryLoad" runat="server"></td>
                                                                        <td id="tdAssetSize" runat="server"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><strong>Investment Plan </strong></td>
                                                                        <td><strong>Exit Load </strong></td>
                                                                        <td><strong>Minimum Investment (Rs.) </strong></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdInvestmentPlan" runat="server"></td>
                                                                        <td id="tdExitLoad" runat="server"></td>
                                                                        <td id="tdMinInvest" runat="server"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><strong>Launch Date</strong></td>
                                                                        <td><strong>Fund Manager</strong></td>
                                                                        <td><strong>Last Dividend</strong></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdLunchDate" runat="server"></td>
                                                                        <td id="tdFundMan" runat="server"></td>
                                                                        <td id="tdLastDiv" runat="server"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><strong>Benchmark</strong></td>
                                                                        <td><strong>Email / Website</strong></td>
                                                                        <td><strong>Bonus</strong></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdBenchMark" runat="server"></td>
                                                                        <td id="tdEmail" runat="server"></td>
                                                                        <td id="tdBonus" runat="server"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><strong>AMC Name</strong></td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdAmcName" runat="server" colspan="3"></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <h4>SIP Performance</h4>
                                                        For monthly investment of Rs.10,000 on 1st day of every month
                                                        <div class="table-responsive">
                                                            <asp:Repeater ID="RptCommonSipResult" runat="server">
                                                                <HeaderTemplate>
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-bordered">
                                                                        <%--<tr style="background:#f6f4f4">
                                                <td colspan="8" class="gridtd" align="center">
                                                    <asp:Label ID="lblReturnSch" runat="server"></asp:Label>
                                                </td>
                                            </tr>--%>
                                                                        <tr style="background: #f6f4f4; font-size: 12px;">
                                                                            <td><strong>Particulars</strong></td>
                                                                            <td align="center"><strong>Total Amount Invested (in Rs.)</strong></td>
                                                                            <td align="center"><strong>Scheme Returns Yield (%)</strong></td>
                                                                            <td align="center"><strong>Scheme Market Value (in Rs.)</strong></td>
                                                                            <td align="center"><strong>Bechmark (<asp:Label ID="lblReturnIndex" runat="server"></asp:Label>) Returns Yield * (%)</strong></td>
                                                                            <td align="center"><strong>Benchmark Market Value (in Rs.)</strong></td>
                                                                            <td align="center"><strong>Additional Bechmark (<asp:Label ID="lblReturnAddIndex" runat="server"></asp:Label>) Return Yield * (%)</strong></td>
                                                                            <td align="center"><strong>Additional Benchmark Market Value (in Rs.)</strong></td>
                                                                        </tr>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td style="background: #f6f4f4">
                                                                            <%#Eval("Particulars") %>
                                                                        </td>
                                                                        <td align="center"><%#Eval("Total_Amount_Invest") %></td>
                                                                        <td align="center"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Scheme_Return_Yield")))? "": Convert.ToString(Math.Round(Convert.ToDecimal(Eval("Scheme_Return_Yield")),2)) %> </td>
                                                                        <td align="center"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Scheme_Market_value")))?"": Convert.ToString(Math.Round(Convert.ToDecimal(Eval("Scheme_Market_value")),2)) %> </td>
                                                                        <td align="center"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Bechmark_return_yield")))?"": Convert.ToString(Math.Round(Convert.ToDecimal(Eval("Bechmark_return_yield")),2)) %> </td>
                                                                        <td align="center"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Benchmark_Market_value")))?"": Convert.ToString(Math.Round(Convert.ToDecimal(Eval("Benchmark_Market_value")),2)) %></td>
                                                                        <td align="center"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Additional_Bechmark_return_yield")))?"": Convert.ToString(Math.Round(Convert.ToDecimal(Eval("Additional_Bechmark_return_yield")),2)) %></td>
                                                                        <td align="center"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Additional_Benchmark_Market_value")))?"": Convert.ToString(Math.Round(Convert.ToDecimal(Eval("Additional_Benchmark_Market_value")),2)) %></td>

                                                                    </tr>
                                                                </ItemTemplate>

                                                                <%--<AlternatingItemTemplate>
                                        <tr class="gridaltrow" style="border-width: 1px; border-style: solid; height: 30px;">
                                            <td class="gridtd" align="center">
                                                <%#Eval("Particulars") %>
                                            </td>
                                            <td class="gridtd" align="center">
                                                <%#Eval("Total_Amount_Invest") %>
                                            </td>
                                            <td class="gridtd" align="center">
                                                <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Return_Yield")),2) %>
                                            </td>
                                            <td class="gridtd" align="center">
                                                <%# Math.Round(Convert.ToDecimal(Eval("Scheme_Market_value")),2) %>
                                            </td>
                                            <td class="gridtd" align="center">
                                                <%# Math.Round(Convert.ToDecimal(Eval("Bechmark_return_yield")),2) %>
                                            </td>
                                            <td class="gridtd" align="center">
                                                <%# Math.Round(Convert.ToDecimal(Eval("Benchmark_Market_value")),2) %>
                                            </td>
                                            <td class="gridtd" align="center">
                                                <%# Math.Round(Convert.ToDecimal(Eval("Additional_Bechmark_return_yield")),2) %>
                                            </td>
                                            <td class="gridtd" align="center">
                                                <%# Math.Round(Convert.ToDecimal(Eval("Additional_Benchmark_Market_value")),2) %>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>--%>

                                                                <FooterTemplate>
                                                                    <tr>
                                                                        <td colspan="8">As on last available nav date</td>
                                                                    </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>

                                                        <div id="collExpRatio" class="col-sm-12" style="padding: 0;"></div>

                                                        <div class="col-sm-12 col-xs-12 col-md-12" style="padding: 0;">
                                                            <div class="col-sm-6" style="padding-left: 0;">
                                                                <h4>Detailed Portfolio</h4>
                                                                    <div class="table-responsive" style="padding-right: 0px; padding-left: 0px;">

                                                                        <asp:Repeater ID="TopCompDetails" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                                                    <tr style="background: #f6f4f4">
                                                                                        <th>Company Name</th>
                                                                                        <th>Sector Name</th>
                                                                                        <th>Asset %&nbsp;</th>
                                                                                    </tr>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("CompName") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("Sector_Name") %>' /></td>
                                                                                    <td>
                                                                                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("Net_Asset") %>' /></td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>

                                                                    </div>
                                                                    <div class="col-sm-6" style="padding-left: 4px;">
                                                                        <div class="col-sm-12 table-responsive" style="padding: 0;">
                                                                           <%-- <asp:Repeater ID="TopCompDetails1" runat="server">
                                                                                <HeaderTemplate>
                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                                                        <tr style="background: #f6f4f4">
                                                                                            <th>Company Name</th>
                                                                                            <th style="width: 30%">Sector Name</th>
                                                                                            <th style="width: 15%">Asset %</th>
                                                                                        </tr>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("CompName") %>' /></td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("Sector_Name") %>' /></td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("Net_Asset") %>' /></td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    </table>
                                                                                </FooterTemplate>
                                                                            </asp:Repeater>--%>
                                                                        </div>

                                                                    </div>
                                                                    <div style="padding: 0;">
                                                                     <h4>Other Parameters</h4>
                                                                     <table class="table table-bordered">
                                                                         <tr id="TrPTR">
                                                                             <td style="background:#f6f4f4">
                                                                                 <strong>PTR</strong>
                                                                             </td>
                                                                             <td>
                                                                                 <asp:Label ID="LblPTRDate" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                              <td>
                                                                                 <asp:Label ID="LblPTRValue" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                         </tr>
                                                                         <tr id="TrYTM">
                                                                             <td style="background:#f6f4f4">
                                                                                  <strong>YTM</strong>
                                                                             </td>
                                                                             <td>
                                                                                 <asp:Label ID="LblYTMDate" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                              <td>
                                                                                 <asp:Label ID="LblYTMValue" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                         </tr>
                                                                         <tr id="TrPE">
                                                                             <td style="background:#f6f4f4">
                                                                                  <strong>PE </strong>
                                                                             </td>
                                                                             <td>
                                                                                 <asp:Label ID="LblPEDate" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                              <td>
                                                                                 <asp:Label ID="LblPEValue" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                         </tr>
                                                                         <tr id="TrPB">
                                                                             <td style="background:#f6f4f4">
                                                                                  <strong>PB</strong>
                                                                             </td>
                                                                             <td>
                                                                                 <asp:Label ID="LblPBDate" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                              <td>
                                                                                 <asp:Label ID="LblPBValue" runat="server" Text="--"></asp:Label>
                                                                             </td>
                                                                         </tr>
                                                                     </table>
                                                                 </div>
                                                            </div>
                                                            

                                                             <div class="col-sm-6">
                                                             <div class="col-sm-12" style="padding: 0;">
                                                                    <div class="table-responsive" style="padding: 0;">
                                                                        <h4>Top 5 Sector Holding</h4>
                                                                        <asp:Repeater ID="RepTopSector" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                                                    <tr style="background: #f6f4f4">

                                                                                        <th class="value_tableheader" style="width: 6%;">Sector Name
                                                                                        </th>
                                                                                        <th class="value_tableheader" style="width: 6%;">% Allocation
                                                                                        </th>
                                                                                    </tr>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                                    <td class="value_tablerow" style="width: 34%;">
                                                                                        <asp:Label ID="lblSectorName" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                                                    </td>
                                                                                    <td class="value_tablerow" style="width: 10%;">
                                                                                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("Scheme") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </div>
                                                             <div class="col-sm-12" style="padding: 0;">
                                                                <div class="table-responsive" style="padding: 0;">
                                                                 
                                                                 <h4 id="TdMc">Market Capitalisation</h4>
                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="width: 100%; height: 220px; font-family: Roboto;" id="dvMarketCapContaner">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                             </div>
                                                            </div>
                                                            <div class="table-responsive" style="padding: 0;">
                                                                 
                                                                <h4>AUM Movement</h4>
                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="table table-bordered">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="width: 100%; height: 220px; font-family: Roboto;" id="chart1">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                             </div>  
                                                            <div class="col-sm-6" style="padding-left: 0;">
                                                                      <div class="table-responsive" style="padding-left: 0;">
                                                                <h4>Asset Allocation</h4>
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" paddinh="0"
                                                                        class="table table-bordered">
                                                                        <tr>
                                                                            <td>
                                                                                <div id="chartAssetAllocationContainer" style="width: 100%; height: 160px;font-family: Roboto; font-size: 10px;">
                                                                                </div>
                                                                                <input type="hidden" id="hdSchemeId" value="0" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                  </div>
                                                            <div class="col-sm-6" style="padding-right: 0;">
                                                              <div class="table-responsive">
                                                                    <h4> Style Box</h4>
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" padding: 0" class="table table-bordered table-responsive" >
                                                                        <tr>
                                                                            <td>
                                                                               <div class="col-sm-12">
                                                                                   <div class="col-sm-7" id="dvStyleBox" style="padding-left: 40px; margin-top:35px;"></div>
                                                                                    <div class="col-sm-5" align="right"> <div id="DivMcapLabel"> <img src="/Images/styleBoxMarketCapLabel.png" /></div>
                                                                                    <div id="DivCreditQualityLabel"><img src="/Images/styleBoxCreditQualityLabel.png"  style="margin-top:20px;"/></div>
                                                                                    <div class="col-sm-7"></div>
                                                                                    </div>
                                                                               </div>
                                                                               <div class="col-sm-12" style="padding-bottom:22px;padding-left: 57px;"><span id="SpanLowercaption4StyleBox"></span></div>
                                                                            </td>

                                                                        </tr>
                                                                     
                                                                      </table>
                                                              </div>
                                                                    </div>

                                                             <div id="dvCreditRatingInsBrkUp">
                                                                 <div class="col-sm-12" style="padding-left: 0; padding-right: 0;">
                                                                       <h4>Instrument Breakup</h4>
                                                                 <div id="dvInsBreak"></div>

                                                                       </div>
                                                                 <div class="col-sm-12" style="padding-left: 0; padding-right: 0;">
                                                                         <h4>Credit Rating</h4>
                                                                         <div id="dvCreditRating">
                                                                         </div>
                                                                     </div>
                                                              </div>
                                                              </div>
                                                            </div>
                                                        </div>


                                                       
                                                             <div class="col-sm-12 col-xs-12 col-md-12" style="padding: 0;">
                                                             <div>

                                                                 <div class="col-sm-12 col-xs-12 col-md-12" style="padding: 0;">
                                                                     
                                                                
                                                                   
                                                                </div>
                                                             </div>
                                                             </div>
                                                             <div class="col-sm-12 col-xs-12 col-md-12" style="padding: 0;">
                                                               

                                                             </div>
                                                         </div>
                                                        <div class="col-sm-12 col-xs-12 col-md-12" style="padding: 0">
                                <div class="sip-left small">
                                    <br /> <br />
                                    Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing. Past performance of the schemes do not indicate the future performance.
                                    <br />
                                    <br />
                                    Developed by: <a href="https://www.icraanalytics.com">ICRA Analytics Ltd</a>
                                </div>
                            </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>


        </div>


    </form>
</body>
</html>
