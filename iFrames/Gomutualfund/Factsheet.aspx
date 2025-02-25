<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Factsheet.aspx.cs" Inherits="iFrames.Gomutualfund.Factsheet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Factsheet</title>
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.min.js"></script>
    <![endif]-->
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script>
    <![endif]-->
   

 <%-- <link rel="stylesheet" href="css/style.css"/>
     <link href="css/jquery-ui.css" rel="stylesheet" />

     <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
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
        <script src="js/modernizr-2.6.2-respond-1.1.0.min.js" type="text/javascript"></script>--%>


     <link rel="stylesheet" href="font-awesome-4.7.0/css/font-awesome.min.css"/>
 <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' /> 
  <link rel="stylesheet" href="css/style.css"/>
    <link rel='stylesheet'  href='http://fonts.googleapis.com/css?family=Open+Sans%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic%7CRoboto%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic&#038;subset=latin%2Clatin-ext&#038;ver=1.0.0' type='text/css' media='all' />

     <link href="css/jquery-ui.css" rel="stylesheet" />

     <script src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script src="js/bootstrap.min.js"></script>
     <script type="text/javascript" src="js/date.js"></script>

   
    <script src="js/perfect-scrollbar.jquery.min.js"></script>
    <script src="js/bootstrap-slider.min.js"></script>

    
    <script src="js/theme.min.js"></script>
    <script src="js/kite.min.js"></script>
     
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
            PullChart(schid);
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
                    alert(data);
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
                    alert(data);
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
                    alert(data);
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
    </script>
</head>
<body>
     <div>
    <form id="form1" runat="server">
        
        <div class="container-fluid" >
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
                <div class="col-lg-10" id="content" style="padding:0; padding-top:15px;">
                    <div class="row">
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
                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td align="left" class="value_news_header1">
                                                                                &nbsp;<asp:Label runat="server" ID="lblFundName"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                &nbsp;<asp:Label runat="server" ID="lblPresentNav"></asp:Label>&nbsp;<img id="imgArrow"
                                                                                    src="img/down.jpg" runat="server" visible="false" />
                                                                                <asp:Label runat="server" ID="lblIncrNav"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="value_fact_header">
                                                                                Investment Info
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="top_text">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                                    <tr class="value_tableheader">
                                                                                        <td class="value_tableheader">
                                                                                            Fund Type
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Entry Load
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Average Asset Size (Rs cr.)
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="value_tablerow" id="tdFundName" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdEntryLoad" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdAssetSize" runat="server">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="value_tableheader">
                                                                                        <td class="value_tableheader">
                                                                                            Investment Plan
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Exit Load
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Minimum Investment (Rs.)
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="value_tablerow" id="tdStructure" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdExitLoad" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdMinInvest" runat="server">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="value_tableheader">
                                                                                        <td class="value_tableheader">
                                                                                            Launch Date
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Fund Manager
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Last Dividend
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="value_tablerow" id="tdLunchDate" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdFundMan" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdLastDiv" runat="server">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="value_tableheader">
                                                                                        <td class="value_tableheader">
                                                                                            Benchmark
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Email / Website
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            Bonus
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="value_tablerow" id="tdBenchMark" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdEmail" runat="server">
                                                                                        </td>
                                                                                        <td class="value_tablerow" id="tdBonus" runat="server">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="value_tableheader">
                                                                                        <td class="value_tableheader">
                                                                                            AMC Name
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td class="value_tableheader">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="value_tablerow" id="tdAmcName" runat="server" colspan="3">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="top_text">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="value_fact_header">
                                                                                Peer Performance
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="top_text">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Repeater ID="PeerPerformance" runat="server" OnItemDataBound="PeerPerformance_ItemDataBound">
                                                                                    <HeaderTemplate>
                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="value_table">
                                                                                            <tr class="top_tableheader">
                                                                                                <th class="value_tableheader" style="width: 34%;">
                                                                                                    Scheme Name
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 10%; text-align:right">
                                                                                                    AUM (In crs)
                                                                                                </th>
                                                                                                <%--<th class="value_tableheader" style="width: 10%;">
                                                                                                    <asp:Label ID="lbl_Rank_Head" runat="server" Text="ICRON Rankings" />
                                                                                                </th>--%>
                                                                                                <th class="value_tableheader" style="width: 6%; text-align:right">
                                                                                                    1 Mths
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 6%; text-align:right">
                                                                                                    3 Mths
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 6%; text-align:right">
                                                                                                    6 Mths
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 6%; text-align:right">
                                                                                                    1 Yr
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 6%; text-align:right">
                                                                                                    3 Yrs
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 6%; text-align:right">
                                                                                                    5 Yrs
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 10%; text-align:right">
                                                                                                    Since Inception
                                                                                                </th>
                                                                                            </tr>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                                            <td class="value_tablerow" style="width: 34%;">
                                                                                                <asp:Label ID="lblSchemeName" runat="server" Text='<%#Eval("SchemeName") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 10%; text-align:right">
                                                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("FundSize") %>' />
                                                                                            </td>
                                                                                           <%-- <td class="value_tablerow" style="width: 10%;">
                                                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("Rank") %>' />
                                                                                            </td>--%>
                                                                                            <td class="value_tablerow" style="width: 6%; text-align:right">
                                                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("Per30Days") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 6%; text-align:right">
                                                                                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("Per91Days") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 6%; text-align:right">
                                                                                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("Per182Days") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 6%; text-align:right">
                                                                                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("Per1Year") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 6%; text-align:right">
                                                                                                <asp:Label ID="Label10" runat="server" Text='<%#Eval("Per3Year") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 10%; text-align:right">
                                                                                                <asp:Label ID="Label7" runat="server" Text='<%#Eval("Per5Year") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 10%; text-align:right">
                                                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("SI") %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        </table>
                                                                                    </FooterTemplate>
                                                                                </asp:Repeater>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="top_text">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="value_fact_header">
                                                                                NAV Graph
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="top_text">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                    <td class="value_news_header1">NAV Graph</td>
                                                </tr>--%>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="chartContainer">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="top_text">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="value_fact_header">
                                                                                Top 10 Holdings
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="top_text">
                                                                                &nbsp;
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
                                                                                                                        <th class="value_tableheader" style="width: 55%;">
                                                                                                                            Company Name</td>
                                                                                                                            <th class="value_tableheader" style="width: 30%">
                                                                                                                                Sector Name</td>
                                                                                                                                <th class="value_tableheader" style="width: 15%; text-align:right">
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
                                                                                                                    <td class="value_tablerow" style="width: 15%; text-align:right">
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
                                                                                        <td width="2%">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td width="49%" style="vertical-align: top">
                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="vertical-align: top">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Repeater ID="TopCompDetails1" runat="server">
                                                                                                            <HeaderTemplate>
                                                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                                                                    <tr class="value_tableheader">
                                                                                                                        <th class="value_tableheader" style="width: 55%;">
                                                                                                                            Company Name</td>
                                                                                                                            <th class="value_tableheader" style="width: 30%">
                                                                                                                                Sector Name</td>
                                                                                                                                <th class="value_tableheader" style="width: 15%; text-align:right">
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
                                                                                                                    <td class="value_tablerow" style="width: 15%; text-align:right">
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
                                                                            <td>
                                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                 <tr>
                                                                                        <td style="width: 49%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 2%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 49%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 49%;" class="value_fact_header">
                                                                                            Top 5 Sector Holding
                                                                                        </td>
                                                                                        <td style="width: 2%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td  id="TdMc" style="width: 49%;" class="value_fact_header">
                                                                                           Market Capitalisation
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                       <td style="vertical-align:top; padding-top:20px;">
                                                                                            <asp:Repeater ID="RepTopSector" runat="server">
                                                                                    <HeaderTemplate>
                                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="value_table">
                                                                                            <tr class="top_tableheader">
                                                                                                
                                                                                                <th class="value_tableheader" style="width: 6%;">
                                                                                                    Sector Name
                                                                                                </th>
                                                                                                <th class="value_tableheader" style="width: 6%; text-align:right">
                                                                                                    % Allocation
                                                                                                </th>
                                                                                            </tr>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                                            <td class="value_tablerow" style="width: 34%;">
                                                                                                <asp:Label ID="lblSectorName" runat="server" Text='<%#Eval("Sector_Name") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 10%; text-align:right">
                                                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Scheme") %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        </table>
                                                                                    </FooterTemplate>
                                                                                </asp:Repeater>
                                                                                        </td>
                                                                                        <td>

                                                                                        </td>
                                                                                         <td>
                                                                                            <div style="width: 100%; float: left; height: 200px; margin-top:20px; text-align: center" id="dvMarketCapContaner">
                                                                                           </div>
                                                                                        </td>                                                                                        
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 49%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 2%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 49%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 49%;" class="value_fact_header">
                                                                                            AUM Movement
                                                                                        </td>
                                                                                        <td style="width: 2%;">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 49%;" class="value_fact_header">
                                                                                            Asset Allocation
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
                                                                                        <td style="width: 50%; height: 220px; margin-top: 0px; padding-top: 0px;">
                                                                                            <div id="chartAssetAllocationContainer" style="width: 100%; height: 220px; font-family: Arial;
                                                                                                font-size: 10px;">
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
                                                                <td class="value_dis" style="padding-top:20px;">
                                                                    <small>Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme
                                                                    related documents carefully before investing. Past performance of the schemes do
                                                                    not indicate the future performance.</small>
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
                                                    <td class="value_btm_text">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <footer class="page__footer">
          <div class="row">
            <div class="col-xs-12">
              <span class="page__footer__year"></span> Developed for Gomutualfund by: <a href="https://www.icraanalytics.com" target="_blank" >ICRA Analytics Ltd</a>&nbsp;<a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank" >Disclaimer</a>
            </div>
          </div>
        </footer>
                </div>
                </div>
                </div>
         <!-- Footer -->
        
    </form>
        </div>
</body>
</html>
