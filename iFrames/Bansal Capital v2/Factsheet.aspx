<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="Factsheet.aspx.cs" Inherits="iFrames.BansalCapital.Factsheet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Factsheet</title>
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.min.js"></script>
    <![endif]-->
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script>
    <![endif]-->
    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link href="css/auto.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/date.js"></script>
    <script src="js/AutoComplete.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/DT_bootstrap.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/styles.css" rel="stylesheet" media="screen" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
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
            getGraphs($("#hdSchemeId").val());
            // };

            GetChart($("#hdSchemeId").val());

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
                var val = "{'schIndexid':'" + schIndId + "', startDate:'" + vdate.fromdate + "', endDate:'" + vdate.enddate + "'}";
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
                    points.push([Date.UTC(res[0], res[1] - 1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue]);
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
                                    s = s + '<span style="color:' + colorarray[i] + '">' + tt[i].name + '</span>: <b>' + (tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b><br />';
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
            font-size:12px;
            white-space:nowrap;
        }
    </style>
</head>
<body>
    <div style="width: 1058px; margin:0 auto;">
        <form id="form1" runat="server">

            <div class="container-fluid">
                <div class="row-fluid">
                    <%-- <div class="span3" id="sidebar">
                    <ul class="nav nav-list bs-docs-sidenav nav-collapse collapse" style="margin-top:0; padding-top:0;">
                        <li><a href="TopFund.aspx">Top Funds</a></li>
                        <li><a href="CompareFund.aspx">Compare Funds                     </a></li>
                        <li><a href="#">Recommended Funds                               </a></li>
                        <li><a href="#">Recommended NFO                               </a></li>
                        <li><a href="NAV.aspx">Funds NAV Graph                                </a></li>
                        <li><a href="SchemeSearch.aspx">Scheme Search                   </a></li>
                        <li><a href="WatchList.aspx">MF Watch List                         </a></li>                         
                    </ul>
                </div>--%>

                    <!--/span-->
                    <div class="span12" id="content" style="padding: 0;">
                        <div class="row-fluid">
                            <!-- block -->
                            <div class="block">
                                <div class="navbar navbar-inner block-header">
                                    <div class="muted pull-left" style="color: #cc0000; font-weight: 700;">
                                        Factsheet
                                <div class="pull-right">
                                    <%--<button id="back">Go Back</button>--%>
                                </div>
                                    </div>
                                </div>
                                <div class="block-content collapse in">
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%" align="left" class="main-content">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">

                                                    <tr>
                                                        <td colspan="2">
                                                            <table id="tblResult" width="100%" border="0" align="left" cellpadding="0" cellspacing="0"
                                                                runat="server" visible="false">
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="height: 80px">
                                                                            <tr>
                                                                                <td align="left" class="value_news_header1" style="font-weight: bold; width: 32%">&nbsp;<asp:Label runat="server" ID="lblFundName"></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="position: absolute; right: 12px;">
                                                                                    <div style="width: 100%; float: left; border: 1px solid #eaeaea; padding: 5px; background: #fbfbfb;">
                                                                                        <p style="margin: 0 0 3px; border-bottom: 1px solid #eaeaea; padding-bottom: 3px; text-align: center; font-weight: 900;">52 Weeks</p>
                                                                                        <div style="width: 50%; float: left; border-right: 1px solid #eaeaea; text-align: center">
                                                                                            <div style="margin-bottom: 8px; font-weight: 700">High</div>
                                                                                            <kbd class="green">
                                                                                                <asp:Label runat="server" ID="lblHigh">--</asp:Label>
                                                                                                (<small><span id="spnMaxDate" runat="server"></span></small>)</kbd>
                                                                                        </div>
                                                                                        <div style="width: 43%; float: left; padding-left: 9px; text-align: center">
                                                                                            <div style="margin-bottom: 8px; font-weight: 700">Low</div>
                                                                                            <kbd class="red">
                                                                                                <asp:Label runat="server" ID="lblLow">--</asp:Label>
                                                                                                (<small><span id="spnMinDate" runat="server"></span></small>)</kbd>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                                <td align="left" style="position: absolute; width: 320px; right: 344px;">
                                                                                    <table class="table table-bordered cstm_class">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th colspan="6" style="padding: 4px; background: #fbfbfb; color: #222">Funds return</th>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <th>1 mth</th>
                                                                                                <th>3 mths</th>
                                                                                                <th>6 mths</th>
                                                                                                <th>1 Yr</th>
                                                                                                <th>3 Yrs</th>
                                                                                                <th>Since Inception</th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td><span id="spn1mth" runat="server"></span></td>
                                                                                                <td><span id="spn3mth" runat="server"></span></td>
                                                                                                <td><span id="spn6mth" runat="server"></span></td>
                                                                                                <td><span id="spn1yr" runat="server"></span></td>
                                                                                                <td><span id="spn3yr" runat="server"></span></td>
                                                                                                <td><span id="spnSinceInception" runat="server"></span></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">&nbsp;<asp:Label runat="server" ID="lblPresentNav"></asp:Label>&nbsp;<img id="imgArrow"
                                                                                    src="img/down.jpg" runat="server" visible="false" />
                                                                                    <asp:Label runat="server" ID="lblIncrNav"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td class="value_fact_header">Investment Info
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="top_text">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                                        <tr class="value_tableheader">
                                                                                            <td class="value_tableheader">Fund Type
                                                                                            </td>
                                                                                            <td class="value_tableheader">Entry Load
                                                                                            </td>
                                                                                            <td class="value_tableheader">Average Asset Size (Rs cr.)
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="value_tablerow" id="tdFundName" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdEntryLoad" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdAssetSize" runat="server"></td>
                                                                                        </tr>
                                                                                        <tr class="value_tableheader">
                                                                                            <td class="value_tableheader">Investment Plan
                                                                                            </td>
                                                                                            <td class="value_tableheader">Exit Load
                                                                                            </td>
                                                                                            <td class="value_tableheader">Minimum Investment (Rs.)
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="value_tablerow" id="tdStructure" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdExitLoad" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdMinInvest" runat="server"></td>
                                                                                        </tr>
                                                                                        <tr class="value_tableheader">
                                                                                            <td class="value_tableheader">Launch Date
                                                                                            </td>
                                                                                            <td class="value_tableheader">Fund Manager
                                                                                            </td>
                                                                                            <td class="value_tableheader">Last Dividend
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="value_tablerow" id="tdLunchDate" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdFundMan" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdLastDiv" runat="server"></td>
                                                                                        </tr>
                                                                                        <tr class="value_tableheader">
                                                                                            <td class="value_tableheader">Benchmark
                                                                                            </td>
                                                                                            <td class="value_tableheader">Email / Website
                                                                                            </td>
                                                                                            <td class="value_tableheader">Bonus
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="value_tablerow" id="tdBenchMark" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdEmail" runat="server"></td>
                                                                                            <td class="value_tablerow" id="tdBonus" runat="server"></td>
                                                                                        </tr>
                                                                                        <tr class="value_tableheader">
                                                                                            <td class="value_tableheader">AMC Name
                                                                                            </td>
                                                                                            <td class="value_tableheader">&nbsp;
                                                                                            </td>
                                                                                            <td class="value_tableheader">&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="value_tablerow" id="tdAmcName" runat="server" colspan="3"></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td class="top_text">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="value_fact_header">NAV Graph
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>

                                                                                    <div id="DivPlotChart" runat="server" class="row-fluid" style="margin-top: 0px; margin-right: 25px; width: 959px;">
                                                                                        <!-- block -->
                                                                                        <div class="block" style="margin-top: 20px; margin-right: 25px; margin-left: -11px; display: none">
                                                                                            <div class="block-content collapse in" id="divTimePeriod" runat="server" visible="false">

                                                                                                <div class="span3">
                                                                                                    <div style="margin-top: 10px">
                                                                                                        <asp:RadioButton ID="rbTime" CssClass="controls" runat="server" GroupName="Time" Checked="true" />
                                                                                                    </div>
                                                                                                    <div class="controls" style="margin-top: -25px; margin-left: 30px;">
                                                                                                        <p class="span5 lebel-drop">Time</p>
                                                                                                        <asp:DropDownList ID="ddlTime" runat="server" CssClass="span6">
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
                                                                                                <div class="span3">

                                                                                                    <div style="margin-top: 10px">
                                                                                                        <asp:RadioButton ID="rbDateRange" runat="server" GroupName="Time" />
                                                                                                    </div>

                                                                                                    <div class="controls" style="margin-top: -25px; margin-left: 30px;">
                                                                                                        <div class="span6 lebel-drop">From Date</div>
                                                                                                        <asp:TextBox ID="txtfromDate" runat="server" Style="margin-top: -32px; margin-left: 80px; width: 50%;"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="span3" style="margin-left: 55px;">

                                                                                                    <div class="controls" style="margin-top: 5px; margin-left: 30px;">
                                                                                                        <div class="span6 lebel-drop">To Date</div>
                                                                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="value_txtbox" Style="margin-top: -32px; margin-left: 80px; width: 50%;"> </asp:TextBox>


                                                                                                    </div>

                                                                                                </div>

                                                                                            </div>

                                                                                        </div>
                                                                                        <!-- /block -->
                                                                                        <%--<asp:Button ID="btnPlotChart" runat="server" Text="Plot Chart" class="btn-sub btn-large"/>--%>
                                                                                        <%-- <input type="button" value="Plot Chart" class="btn-sub btn-large" id="btplotChart" style="margin-right: 33px;" onclick="Javascript: btnPlotclick();" />--%>
                                                                                    </div>
                                                                                    <div id="DivLast" runat="server" class="row-fluid" style="margin-top: 0px; display: none">
                                                                                        <div class="block" style="margin-top: 20px;">
                                                                                            <div class="block-content collapse in">
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <div>
                                                                                                                <%--height: 500px;--%>
                                                                                                                <div style="width: 100%; float: left;" id="chartContainer">
                                                                                                                    <div style="width: 100%; height: 100%;" id="divChart">
                                                                                                                    </div>
                                                                                                                </div>

                                                                                                                <%--new chart--%>
                                                                                                                <div id="HighContainer" style="height: 600px; min-width: 310px; max-width: 1000px"></div>

                                                                                                            </div>
                                                                                                            <div style="width: 700px; text-align: left; font-size: 12px; color: #0c4466;">
                                                                                                                <span id="infoChart">* Click on any Legend above to un-plot the corresponding series</span>
                                                                                                            </div>
                                                                                                            <div class="value_input" style="width: 100%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                                                                                                                Developed for Askmefund by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer"
                                                                                                                    target="_blank">Disclaimer </a>
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
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="top_text">&nbsp;
                                                                                </td>
                                                                            </tr>

                                                                            <%--  <tr>
                                                                                <td>
                                                                                    <div id="chartContainer">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>--%>
                                                                            <tr>
                                                                                <td class="top_text">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="value_fact_header">Top 10 Holdings
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="top_text">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td width="49%" style="vertical-align: top">
                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="vertical-align: top">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Repeater ID="TopCompDetails" runat="server">
                                                                                                                <HeaderTemplate>
                                                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                                                                        <tr class="value_tableheader">
                                                                                                                            <th class="value_tableheader" style="width: 55%;">Company Name</td>
                                                                                                                            <th class="value_tableheader" style="width: 30%">Sector Name</td>
                                                                                                                                <th class="value_tableheader" style="width: 15%">
                                                                                                                            Asset %</td>
                                                                                                                        </tr>
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                                                                        <td class="value_tablerow" style="width: 55%;">
                                                                                                                            <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("CompName") %>' />
                                                                                                                        </td>
                                                                                                                        <td class="value_tablerow" style="width: 30%">
                                                                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                                                                                        </td>
                                                                                                                        <td class="value_tablerow" style="width: 15%">
                                                                                                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("Net_Asset") %>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </ItemTemplate>
                                                                                                                <FooterTemplate>
                                                                                                                    </table>
                                                                                                                </FooterTemplate>
                                                                                                            </asp:Repeater>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td width="2%">&nbsp;
                                                                                            </td>
                                                                                            <td width="49%" style="vertical-align: top">
                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="vertical-align: top">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Repeater ID="TopCompDetails1" runat="server">
                                                                                                                <HeaderTemplate>
                                                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                                                                        <tr class="value_tableheader">
                                                                                                                            <th class="value_tableheader" style="width: 55%;">Company Name</td>
                                                                                                                            <th class="value_tableheader" style="width: 30%">Sector Name</td>
                                                                                                                                <th class="value_tableheader" style="width: 15%">
                                                                                                                            Asset %</td>
                                                                                                                        </tr>
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                                                                        <td class="value_tablerow" style="width: 55%;">
                                                                                                                            <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("CompName") %>' />
                                                                                                                        </td>
                                                                                                                        <td class="value_tablerow" style="width: 30%">
                                                                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                                                                                        </td>
                                                                                                                        <td class="value_tablerow" style="width: 15%">
                                                                                                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("Net_Asset") %>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </ItemTemplate>
                                                                                                                <FooterTemplate>
                                                                                                                    </table>
                                                                                                                </FooterTemplate>
                                                                                                            </asp:Repeater>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="top_text">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="value_fact_header">Statistical Analysis
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="top_text">&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="value_table">
                                                                                        <tbody>
                                                                                            <tr class="top_tableheader">
                                                                                                <th class="value_tableheader">Sharpe
                                                                                                </th>
                                                                                                <th class="value_tableheader">Sortino
                                                                                                </th>
                                                                                                <th class="value_tableheader">Standard Deviation
                                                                                                </th>
                                                                                                <th class="value_tableheader">Beta
                                                                                                </th>
                                                                                                <th class="value_tableheader">R-Square
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr class="value_tablerow">
                                                                                                <td class="value_tablerow" width="20%">
                                                                                                    <label id="lblSharpe" runat="server"></label>
                                                                                                </td>
                                                                                                <td class="value_tablerow" width="20%">
                                                                                                    <label id="lblSortino" runat="server"></label>
                                                                                                </td>
                                                                                                <td class="value_tablerow" width="25%">
                                                                                                    <label id="lblSdv" runat="server"></label>
                                                                                                </td>
                                                                                                <td class="value_tablerow" width="15%">
                                                                                                    <label id="lblBeta" runat="server"></label>
                                                                                                </td>
                                                                                                <td class="value_tablerow" width="45%">
                                                                                                    <label id="lblRSqure" runat="server"></label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>



                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">

                                                                                        <tr>
                                                                                            <td style="width: 49%;">&nbsp;
                                                                                            </td>
                                                                                            <td style="width: 2%;">&nbsp;
                                                                                            </td>
                                                                                            <td style="width: 49%;">&nbsp;
                                                                                            </td>
                                                                                        </tr>


                                                                                        <tr>
                                                                                            <td style="width: 49%;" class="value_fact_header">Top 5 Sector Holding
                                                                                            </td>
                                                                                            <td style="width: 2%;">&nbsp;
                                                                                            </td>
                                                                                            <td id="TdMc" style="width: 49%;" class="value_fact_header">Market Capitalisation
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; padding-top: 20px;">
                                                                                                <asp:Repeater ID="RepTopSector" runat="server">
                                                                                                    <HeaderTemplate>
                                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="value_table">
                                                                                                            <tr class="top_tableheader">

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
                                                                                            </td>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <div style="float: left; height: 200px; margin-top: 20px; text-align: center; width: 98%" id="dvMarketCapContaner">
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 49%;">&nbsp;
                                                                                            </td>
                                                                                            <td style="width: 2%;">&nbsp;
                                                                                            </td>
                                                                                            <td style="width: 49%;">&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 49%;" class="value_fact_header">AUM Movement
                                                                                            </td>
                                                                                            <td style="width: 2%;">&nbsp;
                                                                                            </td>
                                                                                            <td style="width: 49%;" class="value_fact_header">Asset Allocation
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td style="width: 48%; height: 220px; font-family: Arial; font-size: 10px;">
                                                                                                <div style="width: 100%; height: 220px;" id="chart1">
                                                                                                </div>
                                                                                            </td>
                                                                                            <td>
                                                                                                <input type="hidden" id="hdSchemeId" value="0" runat="server" />
                                                                                            </td>
                                                                                            <td style="height: 220px; margin-top: 0px; padding-top: 0px;">
                                                                                                <div id="chartAssetAllocationContainer" style="height: 220px; font-family: Arial; font-size: 10px; width: 100%">
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>

                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="value_dis">Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme
                                                                    related documents carefully before investing. Past performance of the schemes do
                                                                    not indicate the future performance.
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="value_btm_text" style="font-size: 10px; color: #A7A7A7">Developed for Askmefund by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer"
                                                                        target="_blank">Disclaimer </a>
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
                                                <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="value_btm_text">&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
