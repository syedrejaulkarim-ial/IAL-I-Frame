<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="Factsheet.aspx.cs" Inherits="iFrames.ValueInvest.Factsheet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Factsheet</title>

    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.min.js"></script>
    <![endif]-->
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script>
    <![endif]-->


    <script type='text/javascript' src='js/jquery-1.9.1.js'></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
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
    <%--<style type="text/css">
     #chartAssetAllocationContainer.jqplot-point-label {
border: 1.5px solid #aaaaaa;
padding: 1px 3px;
background-color: #eeccdd;
}
#chartAssetAllocationContainer
{
    color:Green;
    font-weight:bold;
}

    </style>--%>
    

    

    <script type="text/javascript">
        $(function () {
            //$.jqplot.config.enablePlugins = true;
            //$("#listboxSchemeName").change(function () {
            //    getGraphs($("#listboxSchemeName").val());
            //});
            if (($("#listboxSchemeName").val() > 0) && ($("#listboxSchemeName").val() != 'null')) {
                getGraphs($("#listboxSchemeName").val());
            };
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
                    if (dataConsolidated.d==null) {
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
                        dataLabelPositionFactor : 1.19,
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
            var maxval=0;
            var dataPlot = [];
            for (var i = 0; i < data.length; i += 1) {
                seriesNames.push(data[i].PortDate);
                dataPlot.push([data[i].PortDate, data[i].MatketValue]);
                if (maxval<data[i].MatketValue)
                maxval=data[i].MatketValue;
            }
            maxval=maxval*.2+maxval;

            var plot1 = $.jqplot('chart1', [dataPlot], {
                seriesColors: ["#da251d", "#ed9c54", "#605d5c", "#e6ab0d", "#49b959", "#ed496c", "#e87500", "#4f8ad3"],
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    rendererOptions:{barMargin: 40},
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
                        max : Math.round(maxval)
                    }
                }
            })
        }
    </script>
</head>


<body>
    <form id="form1" runat="server">
        <table border="0" cellspacing="0" cellpadding="0" width="900" align="left" class="main-content">
            <tr>
                <td>
                    <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="value_heading" colspan="2">
                                <img src="img/arw1.jpg" />Fund Fact Sheet</td>
                        </tr>
                        <tr>
                            <td class="value_heading" colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="value_input">Fund House</td>
                                        <td>
                                            <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                                CssClass="value_input1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="top_td">
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="value_input">Scheme Name </td>
                                        <td>
                                            <asp:DropDownList ID="listboxSchemeName" runat="server" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                                CssClass="value_input1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr class="top_td">
                                        <td class="auto-style1"></td>
                                        <td class="auto-style1">
                                            <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="value_button" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        
                                    </tr>
                                    <tr class="top_td">
                                        <td class="top" colspan="2">&nbsp;
                  
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table id="tblResult" width="100%" border="0" align="left" cellpadding="0" cellspacing="0" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                   
                                                    <td align="left" class="value_news_header1" style="font-weight:bold;">&nbsp;<asp:Label runat="server" ID="lblFundName"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    
                                                    <td align="left">&nbsp;<asp:Label runat="server" ID="lblPresentNav"></asp:Label>&nbsp;<img id="imgArrow" src="img/down.jpg" runat="server" visible="false" />
                                                        <asp:Label runat="server" ID="lblIncrNav"></asp:Label></td>
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
                                            <table width="95%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="value_fact_header">Investment Info</td>
                                                </tr>
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                            <tr class="value_tableheader">
                                                                <td class="value_tableheader">Fund Type</td>
                                                                <td class="value_tableheader">Entry Load</td>
                                                                <td class="value_tableheader">Average Asset Size (Rs cr.)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="value_tablerow" id="tdFundName" runat="server"></td>
                                                                <td class="value_tablerow" id="tdEntryLoad" runat="server"></td>
                                                                <td class="value_tablerow" id="tdAssetSize" runat="server"></td>
                                                            </tr>
                                                            <tr class="value_tableheader">
                                                                <td class="value_tableheader">Investment Plan</td>
                                                                <td class="value_tableheader">Exit Load</td>
                                                                <td class="value_tableheader">Minimum Investment (Rs.)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="value_tablerow" id="tdStructure" runat="server"></td>
                                                                <td class="value_tablerow" id="tdExitLoad" runat="server"></td>
                                                                <td class="value_tablerow" id="tdMinInvest" runat="server"></td>
                                                            </tr>
                                                            <tr class="value_tableheader">
                                                                <td class="value_tableheader">Launch Date</td>
                                                                <td class="value_tableheader">Fund Manager</td>
                                                                <td class="value_tableheader">Last Dividend</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="value_tablerow" id="tdLunchDate" runat="server"></td>
                                                                <td class="value_tablerow" id="tdFundMan" runat="server"></td>
                                                                <td class="value_tablerow" id="tdLastDiv" runat="server"></td>
                                                            </tr>
                                                            <tr class="value_tableheader">
                                                                <td class="value_tableheader">Benchmark</td>
                                                                <td class="value_tableheader">Email / Website</td>
                                                                <td class="value_tableheader">Bonus</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="value_tablerow" id="tdBenchMark" runat="server"></td>
                                                                <td class="value_tablerow" id="tdEmail" runat="server"></td>
                                                                <td class="value_tablerow" id="tdBonus" runat="server"></td>
                                                            </tr>
                                                            <tr class="value_tableheader">
                                                                <td class="value_tableheader">AMC Name</td>
                                                                <td class="value_tableheader">&nbsp;</td>
                                                                <td class="value_tableheader">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="value_tablerow" id="tdAmcName" runat="server" colspan="3"></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_fact_header">Peer Performance</td>
                                                </tr>
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Repeater ID="PeerPerformance" runat="server" OnItemDataBound="PeerPerformance_ItemDataBound">

                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" align="left"  cellpadding="0" cellspacing="0" class="value_table">
                                                                    <tr class="top_tableheader">
                                                                        <th class="value_tableheader" style="width: 34%;">Scheme Name</th>
                                                                        <th class="value_tableheader" style="width: 10%;">AUM (In crs)</th>
                                                                        <%--<th class="value_tableheader" style="width: 10%;" ><asp:Label ID="lbl_Rank_Head" runat="server" Text="ICRON Rankings" /></th>--%>
                                                                        <th class="value_tableheader" style="width: 6%;" >1 Mths</tH>
                                                                        <th class="value_tableheader" style="width: 6%;" >3 Mths</tH>
                                                                        <th class="value_tableheader" style="width: 6%;" >6 Mths</tH>
                                                                        <th class="value_tableheader" style="width: 6%;" >1 Yr</th>
                                                                        <th class="value_tableheader" style="width: 6%;" >3 Yrs</th>
                                                                        <th class="value_tableheader" style="width: 6%;" >5 Yrs</th>
                                                                        <th class="value_tableheader" style="width: 10%;">Since Inception</th>
                                                                    </tr>
                                                            </HeaderTemplate>

                                                            <%--<HeaderTemplate>
                                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" runat="server" class="value_table" id="t_Rank_Head">
                                                                    <tr class="top_tableheader" id="tr_Rank_Head">
                                                                        <th class="value_tableheader" style="width: 34%;">Scheme Name</th>
                                                                        <th class="value_tableheader" style="width: 10%;">AUM (In crs)</th>
                                                                        <th class="value_tableheader" style="width: 10%;" id="th_Rank_Head" ><asp:Label ID="lbl_Rank_Head" runat="server" Text="ICRON Rankings" /></th>
                                                                        <th class="value_tableheader" style="width: 6%;" >1 Mths</th>
                                                                        <th class="value_tableheader" style="width: 6%;" >3 Mths</th>
                                                                        <th class="value_tableheader" style="width: 6%;" >6 Mths</th>
                                                                        <th class="value_tableheader" style="width: 6%;" >1 Yr</th>
                                                                        <th class="value_tableheader" style="width: 6%;" >3 Yrs</th>
                                                                        <th class="value_tableheader" style="width: 6%;" >5 Yrs</th>
                                                                        <th class="value_tableheader" style="width: 10%;">Since Inception</th>
                                                                    </tr>
                                                            </HeaderTemplate>--%>
                                                            <ItemTemplate>
                                                                <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                    <td class="value_tablerow" style="width: 34%;">
                                                                        <asp:Label ID="lblSchemeName" runat="server" Text='<%#Eval("SchemeName") %>' /></td>
                                                                    <td class="value_tablerow" style="width: 10%;">
                                                                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("FundSize") %>' /></td>
                                                                   <%-- <td class="value_tablerow" style="width: 10%;">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("Rank") %>' /></td>--%>
                                                                    <td class="value_tablerow" style="width: 6%;">
                                                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("Per30Days") %>' /></td>
                                                                    <td class="value_tablerow" style="width: 6%;">
                                                                        <asp:Label ID="Label6" runat="server" Text='<%#Eval("Per91Days") %>' /></td>
                                                                    <td class="value_tablerow" style="width: 6%;">
                                                                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("Per182Days") %>' /></td>
                                                                    <td class="value_tablerow" style="width: 6%;">
                                                                        <asp:Label ID="Label9" runat="server" Text='<%#Eval("Per1Year") %>' /></td>
                                                                    <td class="value_tablerow" style="width: 6%;">
                                                                        <asp:Label ID="Label10" runat="server" Text='<%#Eval("Per3Year") %>' /></td>
                                                                    <td class="value_tablerow" style="width: 10%;">
                                                                        <asp:Label ID="Label7" runat="server" Text='<%#Eval("Per5Year") %>' /></td>
                                                                    <td class="value_tablerow" style="width: 10%;">
                                                                        <asp:Label ID="Label5" runat="server" Text='<%#Eval("SI") %>' /></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_fact_header">NAV Graph</td>
                                                </tr>
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <%--<tr>
                                                    <td class="value_news_header1">NAV Graph</td>
                                                </tr>--%>
                                                <tr>
                                                    <td> <div id="chartContainer"></div></td>
                                                </tr>
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_fact_header">Top 10 Holdings</td>
                                                </tr>
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td  width="49%" style="vertical-align:top">
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="vertical-align:top">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Repeater ID="TopCompDetails" runat="server">
                                                                                    <HeaderTemplate>
                                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                                            <tr class="value_tableheader">
                                                                                                <th class="value_tableheader" style="width: 55%;">Company Name</td>
                                                                                                <th class="value_tableheader" style="width: 30%">Sector Name</td>
                                                                                                <th class="value_tableheader" style="width: 15%">Asset %</td>
                                                                                            </tr>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                                            <td class="value_tablerow" style="width: 55%;">
                                                                                                <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("CompName") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 30%">
                                                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Sector_Name") %>' /></td>
                                                                                            <td class="value_tablerow" style="width: 15%">
                                                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Net_Asset") %>' /></td>
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
                                                                <td width="2%">&nbsp;</td>
                                                                <td width="49%" style="vertical-align:top">
                                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" style="vertical-align:top">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Repeater ID="TopCompDetails1" runat="server">
                                                                                    <HeaderTemplate>
                                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                                                            <tr class="value_tableheader">
                                                                                                <th class="value_tableheader" style="width: 55%;">Company Name</td>
                                                                                                <th class="value_tableheader" style="width: 30%">Sector Name</td>
                                                                                                <th class="value_tableheader" style="width: 15%">Asset %</td>
                                                                                            </tr>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr class='<%# Convert.ToBoolean(Container.ItemIndex % 2) ? "value_tablerow" : "value_tablerow" %>'>
                                                                                            <td class="value_tablerow" style="width: 55%;">
                                                                                                <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("CompName") %>' />
                                                                                            </td>
                                                                                            <td class="value_tablerow" style="width: 30%">
                                                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Sector_Name") %>' /></td>
                                                                                            <td class="value_tablerow" style="width: 15%">
                                                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Net_Asset") %>' /></td>
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
                                                                <td style="width: 49%;">&nbsp;</td>
                                                                <td style="width: 2%;">&nbsp;</td>
                                                                <td style="width: 49%; ">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 49%;" class="value_fact_header"> AUM Movement</td>
                                                                <td style="width: 2%;">&nbsp;</td>
                                                                <td style="width: 49%; " class="value_fact_header"> Asset Allocation</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 48%; height: 220px; font-family:Arial; font-size:10px;">
                                                                    <div style="width: 100%; height: 220px;" id="chart1">
                                                                    </div>
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td style="width: 48%; height: 220px; margin-top:10px; padding-top:10px; ">
                                                                    <div id="chartAssetAllocationContainer" style="width: 100%; height: 220px; font-family:Arial; font-size:10px;">
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
                                        <td class="value_dis">Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing. Past performance of the schemes do not indicate the future performance. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="value_btm_text">Developed and Maintained by: ICRA Analytics Ltd </td>
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
                            <td class="value_btm_text">&nbsp; </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
