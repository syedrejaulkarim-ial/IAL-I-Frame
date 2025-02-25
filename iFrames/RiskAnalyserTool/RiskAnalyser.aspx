<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="RiskAnalyser.aspx.cs" Inherits="iFrames.RiskAnalyserTool.RiskAnalyser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.min.js"></script>
    <![endif]-->
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script>
    <![endif]-->

    <script type='text/javascript' src='js/jquery-5.js'></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <script src="../Scripts/jqplot/jquery.jqplot.min.js" type="text/javascript"></script>
    <%--<script src="../Scripts/jqplot/plugins/jqplot.cursor.min.js" type="text/javascript"></script>--%>
<%--    <script src="../Scripts/jqplot/plugins/jqplot.dateAxisRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.highlighter.min.js" type="text/javascript"></script>--%>
    <%--<script src="../Scripts/jqplot/plugins/jqplot.canvasTextRenderer.min.js"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.canvasAxisLabelRenderer.min.js"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js"></script>--%>

    <%--<script src="../Scripts/jqplot/plugins/jqplot.pointLabels.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/jqplot/plugins/jqplot.pieRenderer.min.js" type="text/javascript"></script>
    <%--<script src="../Scripts/jqplot/plugins/jqplot.barRenderer.min.js" type="text/javascript"></script>--%>
    <%--<script src="../Scripts/jqplot/plugins/jqplot.categoryAxisRenderer.min.js" type="text/javascript"></script>--%>
    <link href="../Styles/jquery.jqplot.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $(".testClass").change(function (ev) {
                //debugger;
                var Jparam = jQuery.parseJSON($("#hdJsonParam").val());
                var TotSum = 0;
                var test = $('#container input:radio:checked').map(function () {
                    TotSum = parseInt(TotSum) + parseInt(this.value);
                    return TotSum;
                }).get();
                var modVal = Math.round((TotSum / 156) * 100, 2);
                var InvestProfile = "";
                var DataAssetAllocation = [[]];
                var DataRecAssetAllocation = [[]];
                for (i = 0; i < Jparam.length ; i++) {
                    if (Jparam[i].StartVal <= modVal && Jparam[i].EndVal >= modVal) {
                        InvestProfile = Jparam[i].Name;
                        $("#DvShowProfile").html(InvestProfile + "-(" + modVal + ")");
                        PlotAssetAllocation(Jparam[i].LstAsset);
                        PlotRecAssetAllocation(Jparam[i].LstNature);
                        break;
                    }
                }
            });
        });

        function PlotAssetAllocation(data) {
            if (data == null || data.length < 1) {
                $('#chartAssetAllocationContainer').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="chartAssetAllocation">Data not available for the selected scheme</div>');
                $('#chartAssetAllocationContainer').effect("highlight", {}, 3000);
                return;
            }
            var dataPlot = [[]];
            for (var i = 0; i < data.length; i += 1) {
                var TextVal = [];
                TextVal.push(data[i].Name);
                TextVal.push(parseInt(data[i].Value));
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

        function PlotRecAssetAllocation(data) {
            if (data == null || data.length < 1) {
                $('#chartRecAssetAllocationContainer').append('<div style="width: 100%; height:100%; text-align: center; padding-top: 10%;" id="chartAssetAllocation">Data not available for the selected scheme</div>');
                $('#chartRecAssetAllocationContainer').effect("highlight", {}, 3000);
                return;
            }
            var dataPlot = [[]];
            for (var i = 0; i < data.length; i += 1) {
                var TextVal = [];
                TextVal.push(data[i].Name);
                TextVal.push(parseInt(data[i].Value));
                dataPlot.push(TextVal);
            }
            dataPlot.shift();
            var plot1 = $.jqplot('chartRecAssetAllocationContainer', [dataPlot], {
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <div id="DvShowProfile">

            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ1" runat="server"></asp:Label>
                <asp:RadioButtonList RepeatDirection="Vertical"  CssClass="testClass" RepeatColumns="3" ID="DDLAns1" runat="server"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ2" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="DDLAns2" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ3" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns3" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ4" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns4" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ5" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns5" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ6" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns6" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ7" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns7" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ8" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns8" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ9" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns9" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ10" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns10" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ11" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns11" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ12" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns12" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>

            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ13" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns13" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div>
                <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                    <asp:Label ID="lblQ14" runat="server"></asp:Label>
                    <asp:RadioButtonList runat="server" ID="RdbAns14" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
                </div>
            </div>
            <div style="border-bottom: solid; border-bottom-color: black; border-bottom-width: 1px">
                <asp:Label ID="lblQ15" runat="server"></asp:Label>
                <asp:RadioButtonList runat="server" ID="RdbAns15" CssClass="testClass" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
            </div>
            <div id="chartAssetAllocationContainer">

            </div>
            <div id="chartRecAssetAllocationContainer">

            </div>
            <div>
                <asp:HiddenField ID="hdJsonParam" runat="server" />
            </div>

        </div>
    </form>
</body>
</html>
