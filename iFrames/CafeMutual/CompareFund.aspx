<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="CompareFund.aspx.cs" Inherits="iFrames.CafeMutual.CompareFund" %>

<!DOCTYPE html>

<html class="no-js">

<head>
    <title>Compare Fund</title>
    <!-- Bootstrap -->
    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="css/jquery-ui.css" rel="stylesheet" />

    <link href="css/auto.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/date.js"></script>
    <script src="js/jquery.confirn_box.js"></script>

    <script src="js/AutoComplete.js" type="text/javascript"></script>

    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/DT_bootstrap.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/styles.css" rel="stylesheet" media="screen" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <script src="js/modernizr2.js" type="text/javascript"></script>


    <script src="../Scripts/HighStockChart/highstock.js" type="text/javascript"></script>
    <script src="../Scripts/HighStockChart/exporting.js" type="text/javascript"></script>

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
        })();

    </script>

    <!-- Do not delete It will required for login , Invest now -->
    <script type="text/javascript">
        function callCross(schid, schname, OptionId, Nature, SubNature) {
            if (OptionId == "2")
                var option = "Growth";
            else
                var option = "Devidend";
            var data = { 'url': 'http://www.askmefund.com/transaction.aspx?Scheme_Name=' + schname + ',Option=' + option + ',SchemeId=' + schid + ',Category=' + Nature + ',Sub_Category=' + SubNature };
            top.postMessage(data, 'http://www.askmefund.com/compare-mutual-funds.aspx');
        }
        function Menuclick(url) {
            var data = { 'url': url };
            top.postMessage(data, 'http://www.askmefund.com/compare-mutual-funds.aspx');
        }

        function btnReset() {
            window.location.href = "/CafeMutual/CompareFund.aspx";
        }

        function FnChk() {
            var selected = [];
            $('#chkbxList input:checked').each(function () {
                selected.push($(this).attr('name'));
            });


            if ((selected.length == 0)) {
                $('#GrdCompFund tr').find('td:eq(1),th:eq(1)').show();
                $('#GrdCompFund tr').find('td:eq(2),th:eq(2)').show();
                $('#GrdCompFund tr').find('td:eq(3),th:eq(3)').show();
                $('#GrdCompFund tr').find('td:eq(4),th:eq(4)').show();
                $('#GrdCompFund tr').find('td:eq(5),th:eq(5)').show();
                $('#GrdCompFund tr').find('td:eq(6),th:eq(6)').show();
            }
        };
        $(function () {

            $('#1m').change(function () {
                if (this.checked) {
                    //$('.1m').show();
                    $('#GrdCompFund tr').find('td:eq(1),th:eq(1)').show();
                }
                else {
                    $('#GrdCompFund tr').find('td:eq(1),th:eq(1)').hide();
                }
                // FnChk();
            });
            $('#3m').change(function () {
                if (this.checked) {
                    $('#GrdCompFund tr').find('td:eq(2),th:eq(2)').show();
                }
                else {
                    $('#GrdCompFund tr').find('td:eq(2),th:eq(2)').hide();
                }
                //  FnChk();

            });

            $('#6m').change(function () {
                if (this.checked) {
                    $('#GrdCompFund tr').find('td:eq(3),th:eq(3)').show();
                }
                else {
                    $('#GrdCompFund tr').find('td:eq(3),th:eq(3)').hide();
                }
                //FnChk();

            });
            $('#1y').change(function () {
                if (this.checked) {
                    $('#GrdCompFund tr').find('td:eq(4),th:eq(4)').show();
                }
                else {
                    $('#GrdCompFund tr').find('td:eq(4),th:eq(4)').hide();
                }
                //FnChk();

            });
            $('#nav').change(function () {
                if (this.checked) {
                    $('#GrdCompFund tr').find('td:eq(5),th:eq(5)').show();
                }
                else {
                    $('#GrdCompFund tr').find('td:eq(5),th:eq(5)').hide();
                }
                //FnChk();

            });

            $('#cat').change(function () {
                if (this.checked) {
                    $('#GrdCompFund tr').find('td:eq(6),th:eq(6)').show();
                }
                else {
                    $('#GrdCompFund tr').find('td:eq(6),th:eq(6)').hide();
                }
                //FnChk();

            });

            $('img[name="imgAdd2Watch"]').click(function () {
                var dataToPush = JSON.stringify({
                    schemeId: $(this).attr('id'),
                    user: $('#Userid').val()
                });
                $.ajax({
                    cache: false,
                    dataType: "json",
                    data: dataToPush,
                    url: 'BansalProxy.aspx/Add2Watchlist',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    success: function (dataConsolidated) {
                        var obj = jQuery.parseJSON(dataConsolidated.d);
                        if (obj.d == 1) {
                            alert("Scheme added to your watch list");
                        }
                        else if (obj.d == 2) {
                            alert("You cannot add more than 10 scheme in watch list");
                        }
                        else if (obj.d == 3) {
                            alert("Scheme already exist in your watchlist");
                        }
                        else if (obj.d == 4) {
                            //window.location("javascript:parent.window.location.href='http://localhost:52348/login.aspx'");
                            var data = { 'url': 'http://www.askmefund.com/login.aspx' };
                            top.postMessage(data, 'http://www.askmefund.com/compare-mutual-funds.aspx');
                        }
                        else if (obj.d == 0) {
                            alert("Some Error occured. Please contact askmefund.com");
                        }
                    },
                    error: function (data) {
                        // debugger;
                        //alert(data);
                    }
                });

            });
            btnPlotclick();
        });
        $(function () {

            var selected = [];
            $('#chkbxList input:checked').each(function () {
                selected.push($(this).attr('name'));
            });
            var sele = selected;


        });
        function btnPlotclick() {
            var schIndId = $('#<%=HdSchemes.ClientID %>').val();
            var frmDate = $('#<%=HdFromData.ClientID %>').val();
            var toData = $('#<%=HdToData.ClientID %>').val();

            if ((schIndId == "") || (schIndId == undefined)) {
                //alert("Please select at least one scheme.");
                return;
            }

            //var arrSchId = schIndId.split("#");
            //var flag = true;
            //$.each(arrSchId, function (i, val) {
            //    if (val.match("^s") && val.substring(1) == "") {
            //        flag = false;
            //        return;
            //    }
            //});
            //if (!flag) {
            //    alert("Please select at least one scheme.");
            //    return;
            //}

            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = '{"schIndexid":"' + schIndId + '", "startDate":"' + frmDate + '", "endDate":"' + toData + '"}';
            $.ajax({
                type: "POST",
                url: "NAV.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    // setChart(msg.d);
                    PlotAuto(msg.d, ImageArray);
                },
                error: function (msg) {
                    console.log(msg);
                    alert("Please select at least one scheme.");
                }
            });
        }

//        function PlotAuto(dataConsolidated, ImageArray) {
//            var max = dataConsolidated.MaxDate;
//            var min = dataConsolidated.MinDate;
//            var data = dataConsolidated.SimpleNavReturnModel;
//            var seriesNames = Array();
//            var dataPlot = [[[]]];
//            for (var i = 0; i < data.length; i += 1) {
//                seriesNames.push(data[i].Name);
//                var points = [];
//                for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
//                    points.push([data[i].ValueAndDate[j].Date, data[i].ValueAndDate[j].Value]);
//                }
//                dataPlot.push(points);
//            }

//            dataPlot.shift();
//            $('#divChart').remove();
//            if (data.length < 1) {
//                $('#chartContainer').append('<div style="width: 710px; height:500px; text-align: center; padding: 100px;" id="chart">Data not Available for the selected date range</div>');
//                $('#chartContainer').effect("highlight", {}, 3000);
//                return;
//            }
//            $('#chartContainer').append('<div style="width: 97%; height:500px;" id="divChart" ></div>');

//            // var CustomSeriesColors = ["#7bf773", "#0031ce", "#ff9494", "#9900ff", "#00ad00", "#ff0000", "#ff9933", "#737373", "#9cc6ff", "#633100", "#0085cc"];
//            var CustomSeriesColors = ["#cc0001", "#4eaf12", "#7a2eed"];

//            var colorarray = Array();
//            for (var i = 0; i < ImageArray.length; i += 1) {
//                colorarray.push(CustomSeriesColors[ImageArray[i]]);
//            }

//            var plot2 = $.jqplot('divChart', dataPlot,
//             {
//                 seriesColors: ["#e11515", "#4bb2c5", "#eaa228", "#c1af68", "#ff9494", "#9900ff", "#ff0000", "#ff9933"],
//                 animate: true,
//                 animateReplot: true,
//                 axes: {
//                     xaxis: {
//                         min: min,
//                         max: max,
//                         renderer: $.jqplot.DateAxisRenderer,
//                         rendererOptions: { tickRenderer: $.jqplot.CanvasAxisTickRenderer },
//                         //tickInterval: '7 day',
//                         tickOptions: {
//                             formatString: '%b %#d, %y',
//                             fontSize: '10pt'
//                         }
//                         //tickOptions: { formatString: '%#d/%#m/%Y' }
//                     },
//                     yaxis:
//                         {
//                             label: 'Value [Rebased]',
//                             tickOptions: {
//                                 formatString: '%.2f',
//                                 fontSize: '10pt'
//                             },
//                             labelRenderer: $.jqplot.CanvasAxisLabelRenderer
//                         }
//                 },
//                 seriesDefaults: { showMarker: false, lineWidth: 2, rendererOptions: { animation: { speed: 1000 } } },
//                 highlighter: { show: true, sizeAdjust: 7.5 },
//                 cursor: { show: true, zoom: true, showTooltip: false },
//                 legend: {
//                     renderer: $.jqplot.EnhancedLegendRenderer,
//                     show: true,
//                     location: 's',
//                     rendererOptions: {
//                         numberRows: 4,
//                         numberColumns: 2
//                     },
//                     placement: 'outsideGrid',
//                     seriesToggle: 'on',
//                     fontSize: '1em',
//                     border: '0px solid black'
//                 },
//                 grid: {
//                     shadow: false,
//                     borderWidth: 0,
//                     background: 'rgba(0,0,0,0)'
//                 }
//             });
//            for (var i = 0; i < seriesNames.length; i += 1) {
//                plot2.series[i].label = seriesNames[i];
//            }
//            var legendTable = $($('.jqplot-table-legend')[0]);
//            legendTable.css('height', '100px');


//            plot2.replot();
//            $('#infoChart').removeAttr("style");
//        }
    </script>


    <script type="text/javascript">

        function PlotAuto(dataConsolidated, ImageArray) {

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
                    //                    var dateVal = {};
                    //                    dateVal.x = Date.UTC(res[0], res[1], res[2]);
                    //                    dateVal.y = data[i].ValueAndDate[j].Value;
                    //                    dateVal.OrginalValue = data[i].ValueAndDate[j].OrginalValue;
                    //                    points.push(dateVal);

                    points.push([Date.UTC(res[0], res[1] - 1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue]);

                }
                tt1.data = points;
                tt.push(tt1);
            }
            tt.shift();
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
                    //pointFormat: '<span >{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
                    //valueDecimals: 2,
                    shared: true,
                    backgroundColor: '#FCFFC5',

                    formatter: function () {
                        var s = '';

                        var d = new Date(this.x);

                        var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
                        var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                        var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate() + ' ,' + d.getFullYear();

                        s = s + '<span style="color:#839557">' + navdate + '</span><br /><br />';

                        for (var i = 0; i < tt.length; i++) {
                            for (var k = 0; k < tt[i].data.length; k++) {
                                if (this.x === tt[i].data[k][0]) {

                                    //s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>: <b>' + (tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b><br />';
                                    //break;

                                    s = s + '<span style="color:' + CustomSeriesColors[i] + '">' + tt[i].name + '</span>: <b>' + '</b> (' + this.points[i].point.change.toFixed(2) + '%)<br />';
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

            }
                , function (chart) {

                    // apply the date pickers
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

    <script type="text/javascript" language="javascript">

        function ValidateControl() {
            var selectedFund = $('#<%=ddlCategory.ClientID %>').find(':selected').val();
            if (selectedFund == 0) {
                alert('Please select any Category.');
                $('#<%=ddlCategory.ClientID %>').focus();
                return false;
            }

            var selectedValue = $('#<%=ddlSchemes.ClientID%> option:selected').val();
            if (selectedValue == null) {
                alert('Please select any Scheme.');
                $('#<%=ddlSchemes.ClientID %>').focus();
                return false;
            }

            var bool = Listcount();
            if (bool == false)
                return false;

            return true;

        }

        function OpenAlert(messageBody) {
            debugger;
            $.confirm({
                'title': 'Message',
                'message': messageBody,
                'buttons': {

                    'OK': {
                        'class': 'button',
                        'action': function () { }
                    }
                }
            });
            return false;
        }

        function Listcount() {
            var listCount = CountItemList();
            if (listCount == 3) {
                debugger;
                OpenAlert("You can compare maximum 3 schemes at a time");
                return false;
            }
            return true;
        }

        function CountItemList() {

            var schemeCount = 0;
            $('#<%=dglist.ClientID %>').find("input:checkbox").each(function () {
                if (this.id != '') {
                    schemeCount++;
                }
            });

            return schemeCount;
        }

        function validateList() {
            var listCount = 0;
            var datagrid = $('#<%=dglist.ClientID %>');

            //            $('input:checkbox[id$=chkItem]:checked', datagrid).each(function (item, index) {
            //                listCount++;
            //            });

            $('#<%=dglist.ClientID %>').find("input:checkbox").each(function () {
                if (this.checked && this.id != '') {
                    listCount++;
                }
            });




            if (listCount == 0) {
                alert('Please select at least one Item from List.');
                $('#<%=dglist.ClientID %>').focus();
                return false;
            }

            var vlbRetrnMsg = $('#<%=lbRetrnMsg.ClientID %>');
            //btnPlotclick();

            return true;
        }


        $(function () {

        });



        function SelectAll(CheckBoxControl) {

            if (CheckBoxControl.checked == true) {

                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    //alert(document.forms[0].elements[i].type);
                    //alert(document.forms[0].elements[i].name.indexOf('dgdept_details'));
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('dglist') > -1)) {

                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf('dglist') > -1)) {

                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function pop() {
            //            $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            setTimeout($.unblockUI, 15000);
            // window.location = "http://www.google.com";
            return true;
        }




    </script>


    <%--<script type="text/javascript" >
            $(function () {
                //var dataToPush = JSON.stringify({
                //    schemeIds: schid
                //});
                $.ajax({
                    cache: false,
                    //data: dataToPush,
                    dataType: "json",
                    url: 'localhost:52348/WebMethod.aspx/CheckSession',
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    success: function (dataConsolidated) {
                        alert(dataConsolidated.d);
                        if (dataConsolidated.d == null) {
                            return;
                        }
                        if (dataConsolidated.Value == 0) {
                            return;
                        }
                        
                    },
                    error: function (data) {
                        debugger;
                        alert(data);
                    }
                });
            });

        </script>  --%>

    <style>
        select {
            padding: 5px;
        }

        #confirmOverlay {
            width: 100%;
            height: 100%;
            position: fixed;
            top: 0;
            left: 0;
            background-color: rgba(0, 0, 0, 0.7);
            z-index: 100000;
            color: #000;
        }

        #confirmBox {
            background: #fff;
            width: 300px;
            height: auto;
            position: fixed;
            left: 50%;
            top: 50%;
            margin: -130px 0 0 -135px;
            border: 1px solid rgba(0,0,0,.2);
            border-radius: 2px;
            box-shadow: 0 3px 9px rgba(0,0,0,.5);
        }

            #confirmBox h1 {
                letter-spacing: 0.3px;
                color: #888;
                border-bottom: 1px solid #ededed;
                margin-top: 5px;
            }

            #confirmBox h1, #confirmBox p {
                font-size: 18px;
                font-weight: 400;
                background: #fff;
                padding: 5px 10px;
                color: #666;
            }

            #confirmBox p {
                background: none;
                font-size: 12px;
                line-height: 1.4;
                padding-top: 0px;
            }

        #confirmButtons {
            padding: 5px 0 5px;
            text-align: center;
            border-top: 1px solid #ededed;
            background-color: #f9f9f9;
        }
    </style>
</head>

<body>
    <div style="width: 100%">
        <form id="Form1" runat="server">

            <div class="navbar" align="left" style="margin-left: 7px; margin-top: -55px;">
                <div class="navbar top_bar">
                    <div class="container-fluid">
                        <div class="inner">
                            <div class="left">
                                Compare Funds
                            </div>
                        </div>
                        <div class="bottom">
                            <div class="">
                                <a href="https://mfi360.icrainsights.com/Account/login"
                                    target="_blank">
                                    <img src="img/logo_mfi360.png" /></a>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="row-fluid">
                    <%--<div class="span3" id="sidebar">
                        <ul class="nav nav-list bs-docs-sidenav nav-collapse collapse" style="margin-left: -10px; padding-top: 0;">
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/top-performing-mutual-funds.aspx')">Top Funds</a></li>
                            <li class="active"><a href="#" onclick="Menuclick('http://www.askmefund.com/compare-mutual-funds.aspx')">Compare Funds                     </a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/recommended-mutual-fund.aspx')">Recommended Funds                               </a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/recommended-new-fund-offer.aspx')">Recommended NFO                               </a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/mutual-fund-nav-graph.aspx')">Funds NAV Graph                                </a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/schemesearch.aspx')">Scheme Search                   </a></li>
                            <li><a href="http://www.askmefund.com/mfwatchlist.aspx" target="_blank">MF Watch List                         </a></li>
                        </ul>
                    </div>--%>

                    <!--/span-->
                    <div class="span12" id="content">
                        <div class="row-fluid">
                            <!-- block -->
                            <div class="block1">
                                <div>
                                    <div class="muted pull-left" style="color: #cc0000; font-weight: 700;"></div>
                                    <div class="pull-right">
                                    </div>
                                </div>
                                <div class="block-content collapse in">
                                    <div class="controls">
                                        <p class="span5 lebel-drop">Mutual Fund</p>
                                        <%--<select class="span6">
                                              <option>HDFC Mutual Fund</option>
                                              <option>HDFC Mutual Fund</option>
                                            </select>--%>
                                        <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged" CssClass="span6">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="controls">
                                        <p class="span5 lebel-drop">Category</p>
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="span6" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="controls">
                                        <p class="span5 lebel-drop">Sub-Category</p>
                                        <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="span6" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="controls">
                                        <p class="span5 lebel-drop">Type</p>

                                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" CssClass="span6"
                                            OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        &nbsp;
                                    </div>
                                    <div class="controls" style="margin-bottom: 10px;">
                                        <div class="span5 lebel-drop">Option</div>
                                        <div class="style-radio">
                                            <asp:RadioButtonList ID="rdbOption" runat="server"
                                                class="radio" RepeatDirection="Horizontal" AutoPostBack="true"
                                                Style="display: inline-block; margin-left: 15px; margin-top: 5px;"
                                                OnSelectedIndexChanged="rdbOption_SelectedIndexChanged">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>

                                    <div class="controls">
                                        <p class="span5 lebel-drop">Choose Scheme</p>
                                        <asp:DropDownList ID="ddlSchemes" runat="server" CssClass="span6">
                                            <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="btnAddScheme" runat="server" AlternateText="Add" OnClick="btnAddScheme_Click" class="btn btn-danger btn-mini"
                                            OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" ImageUrl="images/images/add.png" Style="margin-top: -15px;" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                            ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="controls" style="display: none">
                                        <p class="span5 lebel-drop">Index</p>
                                        <asp:DropDownList ID="ddlIndices" runat="server" CssClass="span6">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="btnAddIndices" runat="server" AlternateText="Add" ImageUrl="images/images/add.png" OnClick="btnAddIndices_Click" class="btn btn-danger btn-mini"
                                            OnClientClick="Javascript:return Listcount();" ValidationGroup="indices" Style="margin-top: -15px;" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                            ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <input class="btn-sub btn-large" value="RESET" onclick="btnReset()" type="button" />
                                <%-- <asp:Button ID="btnReset" class="btn-sub btn-large" runat="server" Text="Reset" OnClick="btnReset_Click" />--%>
                                <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                                <!-- /block -->
                            </div>
                        </div>
                    </div>
                </div>
                <%--<asp:HiddenField ID="HdDivFundShow" runat="server" value="0" />--%>
                <div id="DivFundShow" class="row-fluid" style="margin-top: 0px; margin-right: 25px;" runat="server">
                    <!-- block -->
                    <div class="block">

                        <div class="block-content collapse in">

                            <asp:DataGrid class="table table-striped table-bordered" Style="margin-top: 15px; font-size: 12px; margin-left: 0px;" ID="dglist" runat="server" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" Width="100%" OnItemCommand="dglist_ItemCommand">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="text" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <input type="checkbox" name="SelectAllCheckBox" class="text" onclick="SelectAll(this)" style="text-align: c">&nbsp;ALL
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkItem" Checked="true" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="hdSchID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>' />
                                            <asp:HiddenField ID="hdIndID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>' />
                                            <asp:HiddenField ID="hdImgID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="text">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                            <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                            <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                            <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Left" Width="70%"></ItemStyle>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Auto ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="text" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="images/close.png" OnClientClick="javascript:return confirm('Are you sure to delete?');"
                                                CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </asp:DataGrid>
                        </div>
                        <div>
                            <div class="block-content collapse in">
                                <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                            </div>
                        </div>
                        <%--<button class="btn-sub btn-large" style="margin-right:10px;">Show Performance</button>--%>
                        <asp:Button ID="btnCompareFund" runat="server" Text="Show Performance" class="btn-sub btn-large" Style="margin-right: 0px;"
                            OnClientClick="return validateList();" OnClick="btnCompareFund_Click" Visible="false" />
                    </div>
                    <br />
                    <br />
                    <div class="" style="text-align: right; margin-bottom: 10px; padding-top: 20px;">
                        <p>
                            <a href="https://mfi360.icrainsights.com/AMC/FundScreener" target="_blank" style="color: #cc885a; font-size: 12px; font-weight: bold">Compare more Funds</a>
                        </p>
                    </div>
                    <!-- /block -->
                </div>

                <div id="DivShowPerformance" class="row-fluid" style="margin-top: 0px; margin-right: 25px;" runat="server">
                    <!-- block -->
                    <div class="block" style="margin-top: 20px;">

                        <div class="block-content collapse in" style="display: none;">
                            <asp:Label ID="lblSortPeriod" runat="server" Visible="false" CssClass="gap-left">Click on 'Time Period' to rank funds on a particular period of your choice.
                            </asp:Label>
                        </div>
                        <div class="block-content collapse in">
                            <a href="#myModal" role="button" class="btn-sub-small btn-large"
                                data-toggle="modal" style="color: #fff">Add/ Remove Columns</a>

                            <asp:GridView ID="GrdCompFund" runat="server" class="table table-striped table-bordered" Style="font-size: 12px;" Width="100%" AutoGenerateColumns="false"
                                OnRowCommand="GrdCompFund_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="30%">
                                        <ItemTemplate>

                                            <a href="factsheet.aspx?param=<%#Eval("Sch_id")%>" target="_blank"><%#Eval("Sch_Short_Name")%></a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="30%" Font-Bold="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderText="1 mth" HeaderStyle-CssClass="1m">

                                        <ItemTemplate>
                                            <asp:Label ID="lbl1Mth" HeaderStyle-HorizontalAlign="right"
                                                runat="server" Text='<%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days") : "NA"%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderText="3 mths" HeaderStyle-CssClass="3m">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3Mth" runat="server" HeaderStyle-HorizontalAlign="right">
                                <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderText="6 mths" HeaderStyle-CssClass="6m">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl6Mth" runat="server" HeaderStyle-HorizontalAlign="right">
                                <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderText="1Yr" HeaderStyle-CssClass="1y">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1yr" runat="server" HeaderStyle-HorizontalAlign="right">
                                <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="nav">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="NAV"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNav" runat="server">
                                <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-CssClass="cat">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label2" runat="server" SkinID="lblHeader" Text="Category"></asp:Label>
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCat" runat="server">
                                <%#Eval("Nature").ToString() != "" ? Eval("Nature") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Structure"></asp:Label>
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblStruct" runat="server">
                                <%#Eval("Structure_Name").ToString() != "" ? Eval("Structure_Name") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Watch List"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <img src="img/add-button.png" style="cursor: pointer;" alt="" name="imgAdd2Watch" id="<%#Eval("Sch_id")%>" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <%-- <HeaderTemplate>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Invest Now"></asp:Label>
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <%-- <a href="javascript:parent.window.location.href='http://localhost:52348/InvestNow.aspx?schId=<%#Eval("Sch_id")%>&SchName=<%#Eval("Sch_Short_Name")%>'">
                                                <img src="images/invest.jpg" alt="" /></a>--%>

                                            <%--  <img src="images/invest.jpg" style="cursor: pointer" alt="" onclick="callCross('<%#Eval("Sch_id")%>','<%#Eval("Sch_Short_Name")%>','<%#Eval("Option_Id")%>','<%#Eval("Nature")%>','<%#Eval("Sub_Nature")%>')" />--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    No Data Found
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <div style="float: left; border-bottom: none;">
                            <asp:Label ID="lbRetrnMsg" Visible="false" runat="server" CssClass="gap-left">
                        *Note:- Returns calculated for less than 1 year are Absolute returns and returns
                        calculated for more than 1 year are compounded annualized.</asp:Label>
                        </div>
                        <div style="float: right; margin-bottom: 10px; padding-top: 0px;">
                            <span>
                                <a href="https://mfi360.icrainsights.com/SchemePerformance/Index" target="_blank" style="color: #cc885a; font-size: 12px; font-weight: bold; text-decoration: underline">
                                    Add more periods
                                </a>
                            </span>
                            </div>


                    </div>

                    <div style="margin-top: 40px; margin-right: 0px; margin-left: -15px;">
                        <input id="HdSchemes" type="hidden" runat="server" />
                        <input id="HdToData" type="hidden" runat="server" />
                        <input id="HdFromData" type="hidden" runat="server" />

                        <%--new chart--%>
                        <div id="HighContainer" style="height: 600px; width: auto"></div>
                    </div>
                    <!-- /block -->
                    <div class="" style="text-align: right; margin-bottom: 10px; padding-top: 20px;">
                        <p>
                            <a href="https://mfi360.icrainsights.com/SchemePerformance/Index"
                                target="_blank"
                                style="color: #cc885a; font-size: 12px; font-weight: bold; text-decoration: underline">Add more periods</a>
                        </p>
                    </div>
                    <div class="" style="text-align: right; margin-bottom: 10px; padding-top: 20px;">
                        <p>
                            <a href="https://mfi360.icrainsights.com/AMC/FundScreener"
                                target="_blank"
                                style="color: #cc885a; font-size: 12px; font-weight: bold; text-decoration: underline">Do More</a>
                        </p>
                    </div>
                </div>


                <footer style="font-size: 12px; margin-top: 10px; text-align: center">
                    <div class="value_input" style="text-align: right; font-size: 10px; color: #A7A7A7">
                        Developed for Cafemutual by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer"
                            target="_blank">Disclaimer </a>
                    </div>
                </footer>
            </div>
        </form>
    </div>

    <!-- Modal -->
    <div id="myModal" class="modal hide fade" tabindex="-1"
        role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal"
                aria-hidden="true">
                ×</button>
            <h4 id="myModalLabel">Add/ Remove Columns</h4>
        </div>
        <div class="modal-body" id="chkbxList">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                <tr>
                    <td>
                        <label class="checkbox">
                            <input type="checkbox" id="1m" name="1" checked="checked">
                            1month
                        </label>
                    </td>
                    <td>
                        <label class="checkbox">
                            <input type="checkbox" id="3m" name="2" checked="checked">
                            3months
                        </label>
                    </td>
                    <td>
                        <label class="checkbox">
                            <input type="checkbox" id="6m" name="3" checked="checked">
                            6months
                        </label>
                    </td>

                </tr>
                <tr>
                    <td>
                        <label class="checkbox">
                            <input type="checkbox" id="1y" name="4" checked="checked">
                            1Year
                        </label>
                    </td>
                    <td>
                        <label class="checkbox">
                            <input type="checkbox" id="nav" name="5" checked="checked">
                            NAV
                        </label>
                    </td>
                    <%-- <td>
                        <label class="checkbox" >
                            <input type="checkbox" id="cat" name="7" checked="checked">
                            Since Inception
                        </label>
                    </td>--%>
                    <td>&nbsp; </td>
                </tr>
            </table>

        </div>
        <%--<div class="modal-footer">
            <button class="btn" data-dismiss="modal" aria-hidden="true">
                Close</button>
            <button class="btn btn-primary">Save changes</button>
        </div>--%>
    </div>

</body>

</html>
