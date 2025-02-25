<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="compareFund.aspx.cs"
    Inherits="iFrames.GreenEarth.compareFund" %>

<!DOCTYPE html>

<html class="no-js">

<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="origin-when-cross-origin" name="referrer">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>Compare Fund</title>
    <!-- Bootstrap -->
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />
    <script src="https://use.fontawesome.com/ea6a7e4db5.js"></script>

    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="css/jquery-ui.css" rel="stylesheet" />

    <script type="text/javascript" src="js/date.js"></script>

    <script src="js/AutoComplete.js" type="text/javascript"></script>

    <script src="js/modernizr2.js" type="text/javascript"></script>

    <script src="../Scripts/HighStockChart/highstock.js"
        type="text/javascript"></script>
    <script src="../Scripts/HighStockChart/exporting.js"
        type="text/javascript"></script>

    <link type="text/css" rel="stylesheet" href="css/bootstrap-multiselect.css" />
    <script src="js/bootstrap-multiselect.js"></script>

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
        function UnCheckItem(CnkBx) {
            //alert("wekgu");
            if (!CnkBx.checked) {
                $('input[name="SelectAllCheckBox"]')[0].checked = false;
            }
            else {
                var IsAllChecked = true;
                $('#dglist input:checkbox').each(function () {
                    if (!$(this)[0].checked && $(this)[0].attributes["name"].value != "SelectAllCheckBox") {
                        IsAllChecked = false;
                    }
                });
                if (IsAllChecked)
                    $('input[name="SelectAllCheckBox"]')[0].checked = true;

            }
        }
        function CheckItem() {
            //alert("wekgu");
            setTimeout(function () {
                var IsAllChecked = true;
                $('#dglist input:checkbox').each(function () {
                    if (!$(this)[0].checked && $(this)[0].attributes["name"].value != "SelectAllCheckBox") {
                        IsAllChecked = false;
                    }
                });
                if (IsAllChecked)
                    $('input[name="SelectAllCheckBox"]')[0].checked = true;
            }, 100);

        }
        CheckItem();

        $(function () {
            if ($('#HdShowGraph').val() == "1") {
                btnPlotclick();
                $('#HdShowGraph').val("0");
            }
        });

        function btnPlotclick() {
            var schIndId = $('#<%=HdSchemes.ClientID %>').val();
            var frmDate = $('#<%=HdFromData.ClientID %>').val();
            var toData = $('#<%=HdToData.ClientID %>').val();

            if ((schIndId == "") || (schIndId == undefined)) {
                //alert("Please select at least one scheme..");
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

            var datagrid = $('#<%=dglist.ClientID %>');
            var tempStr = '';
            var ImageArray = Array();


            $('#<%=dglist.ClientID %>').find("input:checkbox").each(function () {
                if (this.checked && this.id != '') {
                    //   alert( $(this).next().val());

                    var Schid = $(this).next().val();
                    var Indid = $(this).next().next().val();
                    var ImgId = $(this).next().next().next().val();
                    //alert(Schid); alert(Indid);
                    if (Schid != '')
                        tempStr += ('s' + Schid + '#');
                    if (Indid != '')
                        tempStr += ('i' + Indid + '#');
                    ImageArray.push(ImgId);
                }
            });

            if (tempStr != '')
                schIndId = tempStr;
            else {
                alert('Please Select at least One Scheme to Plot Chart');
                return false;
            }
            //var vdate = setDateRange();

            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = '{"schIndexid":"' + schIndId + '", "startDate":"' + frmDate + '", "endDate":"' + toData + '"}';
            $.ajax({
                type: "POST",
                url: "compareFund.aspx/getChartData",
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
                    alert("Please Select at least One Scheme to Plot Chart.");
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

                                    s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>: <b>' + (tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b><br />';
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

    <script type="text/javascript">
        $(function () {
            $('#ddlFundHouse').attr("multiple", "multiple");
            $('[id=ddlFundHouse]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                nonSelectedText: 'Select Mutual Fund',
                numberDisplayed: 2,
                filterPlaceholder: 'Search Here..',
                deselectAll: false
            });
            //$("#ddlFundHouse").val([]);
            var chkPostBack = '<%= Page.IsPostBack ? "true" : "false" %>';

            if (chkPostBack == 'false') {
                $('#ddlFundHouse').multiselect('deselectAll', false);
                $('#ddlFundHouse').multiselect('rebuild');
            }
        });
    </script>


    <script type="text/javascript" language="javascript">

        function ValidateControl() {
            var selectedFund = $('#<%=ddlCategory.ClientID %>').find(':selected').val();
              if (selectedFund == 0) {
                  alert('Please select any Category.');
                  $('#<%=ddlCategory.ClientID %>').focus();
                  return false;
              }

              var selectedFund = $('#<%=ddlFundHouse.ClientID %>').find(':selected').val();
              if (selectedFund == 0) {
                  alert('Please select any Mutual Fund.');
                  $('#<%=ddlFundHouse.ClientID %>').focus();
                  return false;
              }

              var selectedValue = $('#<%=ddlSchemes.ClientID%> option:selected').val();
              if (selectedValue == null || selectedValue == 0) {
                  alert('Please select any Scheme.');
                  $('#<%=ddlSchemes.ClientID %>').focus();
                return false;
            }


            var bool = Listcount();
            if (bool == false)
                return false;

            return true;

        }

        function Listcount() {
            var listCount = CountItemList();
            if (listCount >= 5) {
                alert("You can compare maximum 5 schemes/indices at a time");
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


    <style>
        label {
            margin-bottom: 0.5rem;
        }

        /*#rdbOption label {
            font-weight: normal !important;
            font-size:11px;
            padding-right:5px;
        }*/
        .radio, .checkbox {
            display: inline-table;
            top: 0px;
            left: 0px;
        }

            .radio input[type="radio"], .radio-inline input[type="radio"], .checkbox-inline input[type="checkbox"] {
                margin-left: 0px;
            }

        .btn-group-xs > .btn, .btn-xs {
            padding: 1px 5px;
            font-size: 12px;
            line-height: 1.5;
        }

        table th a {
            color: #fff;
        }

        .text-right {
            text-align: right;
        }

        .gap-left {
            font-size: 11px;
        }

        .multiselect-container {
            padding: 5px;
            height: 300px;
            overflow-y: scroll;
            overflow-x: hidden;
        }

        .checkbox {
            position: relative;
            display: block;
            margin-top: 1px;
            margin-bottom: 1px;
        }

        .btn-group-justified > .btn, .btn-group-justified > .btn-group {
            display: table-cell;
            float: none;
            width: 100%;
        }

        .dropdown-menu > .active > a, .dropdown-menu > .active > a:hover, .dropdown-menu > .active > a:focus {
            color: #fff;
            text-decoration: none;
            background-color: transparent;
            outline: 0;
        }
         .card{
            border:none;
        }
        
        .card-header {
            padding: 10px 0px !important;
            border-bottom: 1px solid #ebebeb;
        }
        .card-header h5{
            margin:0;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="card">
                <div class="card-header">
                    <h5>Compare Fund</h5>                               
                     <asp:HiddenField ID="HdShowGraph"  runat="server" Value="0" />
                </div>
                <div class="card-body">
                    <div class="card">
                        <div class="card-body">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div class="row">
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Category</label>
                                        <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                            AutoPostBack="true" class="form-control form-control-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Sub-Category</label>
                                        <asp:DropDownList ID="ddlSubCategory" runat="server"
                                            class="form-control form-control-sm" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div class="row">
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Type</label>
                                        <asp:DropDownList ID="ddlType" runat="server" class="form-control form-control-sm"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Mutual Fund Name</label>
                                        <div>
                                            <asp:ListBox ID="ddlFundHouse" runat="server"
                                                class="form-control form-control-sm" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged"
                                                SelectionMode="multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-xs-10 col-sm-10 col-md-5 col-lg-5">
                                        <label>Choose Scheme</label>
                                        <asp:DropDownList ID="ddlSchemes" runat="server" CssClass="form-control form-control-sm">
                                            <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                            runat="server" ErrorMessage="Please Select Scheme."
                                            ControlToValidate="ddlSchemes" Display="Dynamic"
                                            InitialValue="-1" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-xs-2 col-sm-2 col-md-2 col-lg-1 pt-25 text-right"
                                        style="padding-left: 0">
                                        <asp:ImageButton ID="btnAddScheme" runat="server" AlternateText="Add"
                                            OnClick="btnAddScheme_Click" class="btn btn-success btn-xs"
                                            OnClientClick="Javascript:return ValidateControl();"
                                            ValidationGroup="scheme" ImageUrl="images/images/add.png" />
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <label>Option</label>
                                        <div>
                                            <asp:RadioButtonList ID="rdbOption" runat="server"
                                                class="radio" RepeatDirection="Horizontal" Width="100%"
                                                OnSelectedIndexChanged="rdbOption_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div class="row">

                                    <div class="form-group col-xs-10 col-sm-10 col-md-5 col-lg-5">
                                        <label>Choose Index</label>
                                        <%--<asp:DropDownList ID="ddlRank" runat="server" class="form-control form-control-sm">
                                                            <asp:ListItem Text="All" Value="1000" />
                                                            <asp:ListItem Text="Top 5" Value="5" />
                                                            <asp:ListItem Text="Top 10" Value="10" />
                                                            <asp:ListItem Text="Top 15" Value="15" />
                                                            <asp:ListItem Text="Top 20" Value="20" />
                                                            <asp:ListItem Text="Top 25" Value="25" />
                                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlIndices" runat="server" CssClass="form-control form-control-sm">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                            runat="server" ErrorMessage="Please Select Indices."
                                            ControlToValidate="ddlIndices" Display="Dynamic"
                                            InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-xs-2 col-sm-2 col-md-1 col-lg-1 pt-25 text-right"
                                        style="padding-left: 0">
                                        <asp:ImageButton ID="btnAddIndices" runat="server"
                                            AlternateText="Add" ImageUrl="images/images/add.png"
                                            OnClick="btnAddIndices_Click" class="btn btn-success btn-xs"
                                            OnClientClick="Javascript:return Listcount();"
                                            ValidationGroup="indices" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pr-0 pt-3 p-0"
                                style="text-align: right">
                                <asp:Button ID="btnReset" class="btn btn-light text-right"
                                    Style="margin-right: 0px;"
                                    runat="server" Text="Reset" OnClick="btnReset_Click" />
                                <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                <div id="DivFundShow" runat="server">
                                    <div class="" style="margin-top: 20px;">
                                        <asp:DataGrid class="table table-striped table-bordered"
                                            ID="dglist"
                                            runat="server" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true"
                                            Width="100%" OnItemCommand="dglist_ItemCommand">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="top_tableheader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSchemeId" runat="server" Visible="false"
                                                            Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                                        <asp:Label ID="lblIndId" runat="server" Visible="false"
                                                            Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                                        <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                                        <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                                    <ItemStyle HorizontalAlign="Left" Width="70%"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="top_tableheader">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" name="SelectAllCheckBox"
                                                            onclick="SelectAll(this)" onload="">&nbsp;ALL
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkItem" runat="server" onclick="UnCheckItem(this)">
                                                        </asp:CheckBox>
                                                        <asp:HiddenField ID="hdSchID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>' />
                                                        <asp:HiddenField ID="hdIndID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>' />
                                                        <asp:HiddenField ID="hdImgID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Auto ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                    <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="top_tableheader">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="images/close.png"
                                                            OnClientClick="javascript:return confirm('Are you sure you want to delete this item?');"
                                                            CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                    </ItemTemplate>
                                                    <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                        </asp:DataGrid>
                                        <div>
                                            <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                                        </div>
                                        <%--<button class="btn-sub btn-large" style="margin-right:10px;">Show Performance</button>--%>
                                        <div style="text-align: right; padding-bottom:10px" class="pt-1">
                                            <asp:Button ID="btnCompareFund" runat="server" Text="Show Performance"
                                                class="btn btn-warning btn-xs pull-right" OnClientClick="validateList();"
                                                OnClick="btnCompareFund_Click" Visible="false" />
                                        </div>

                                    </div>
                                    <!-- / -->
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">

                                <div id="DivShowPerformance" runat="server">
                                    <!--  -->
                                    <div class="table-responsive">
                                        <div class="">
                                            <asp:Label ID="lblSortPeriod" runat="server" Visible="false"
                                                CssClass="gap-left">Click on 'Time Period' to rank funds on a particular period of your choice.
                                            </asp:Label>
                                        </div>
                                        <div class="">
                                            <asp:GridView ID="GrdCompFund" runat="server" class="table table-striped table-bordered"
                                                Width="100%" AutoGenerateColumns="false"
                                                OnRowCommand="GrdCompFund_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="center"
                                                        ControlStyle-Width="30%">
                                                        <ItemTemplate>
                                                            <%# SetHyperlinkFundDetail(Eval("Sch_id").ToString(), Eval("Sch_Short_Name").ToString())%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="Lnk1mth" runat="server" Text="1M"
                                                                CssClass="text-right" Font-Overline="false" CommandName="Per_30_Days"
                                                                />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl1Mth" CssClass="text-right" runat="server"
                                                                Text='<%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days", "{0:0.00}") : "--"%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="Lnk3mth" runat="server" 
                                                                Text="3M" Font-Overline="false" CommandName="Per_91_Days"
                                                                CssClass="text-right"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl3Mth" runat="server" CssClass="text-right">
                                <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days", "{0:0.00}") : "--"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="Lnk6mth" runat="server"
                                                                Text="6M"
                                                                Font-Overline="false" CommandName="Per_182_Days"
                                                                CssClass="text-right"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl6Mth" runat="server">
                                <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days", "{0:0.00}") : "--"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="Lnk1yr" runat="server" 
                                                                Text="1Y"
                                                                Font-Overline="false" CommandName="Per_1_Year"
                                                                CssClass="text-right"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl1yr" runat="server">
                                <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year", "{0:0.00}") : "--"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="Lnk3yr" runat="server" 
                                                                Text="3Y"
                                                                Font-Overline="false" CommandName="Per_3_Year"
                                                                CssClass="text-right"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl3yr" runat="server">
                                <%#Eval("Per_3_Year").ToString() != "" ? Eval("Per_3_Year", "{0:0.00}") : "--"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="LnkSI" runat="server" 
                                                                Text="Since Inception"
                                                                Font-Overline="false" CommandName="Per_Since_Inception"
                                                                CssClass="text-right"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSI" runat="server">
                                <%#Eval("Per_Since_Inception").ToString() != "" ? Eval("Per_Since_Inception", "{0:0.00}") : "--"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label1" runat="server" SkinID="lblHeader"
                                                                Text="NAV" CssClass="text-right"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNav" runat="server">
                                <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs", "{0:0.00}") : "NA"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label2" runat="server" SkinID="lblHeader"
                                                                Text="Category"></asp:Label>
                                                        </HeaderTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCat" runat="server">
                                <%#Eval("Nature").ToString() != "" ? Eval("Nature") : "NA"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader"
                                                                Text="Structure"></asp:Label>
                                                        </HeaderTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStruct" runat="server">
                                <%#Eval("Structure_Name").ToString() != "" ? Eval("Structure_Name") : "NA"%> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No Data Found
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                        <div style="border-bottom: none;">
                                            <asp:Label ID="lbRetrnMsg" Visible="false" runat="server"
                                                CssClass="gap-left">
                        *Note:- Returns calculated for less than 1 year are Absolute returns and returns
                        calculated for more than 1 year are compounded annualized.</asp:Label>
                                        </div>
                                        
                                    </div>
                                    <div class="table-responsive">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-2 p-0">
                                            <input id="HdSchemes" type="hidden" runat="server" />
                                            <input id="HdToData" type="hidden" runat="server" />
                                            <input id="HdFromData" type="hidden" runat="server" />

                                            <div id="chartContainer" class="">
                                                <div id="divChart"></div>
                                            </div>

                                            <%--new chart--%>
                                            <div id="HighContainer"></div>
                                        </div>
                                        <div style="text-align: left; font-size: 12px; color: #0c4466;">
                                            <span id="infoChart">* Click on any Legend above to un-plot
                                                the corresponding series</span>
                                        </div>
                                        <div style="width: 100%; float: right; text-align: right;
                                            font-size: 10px; color: #A7A7A7">
                                            Developed by: <a href="https://www.icraanalytics.com"
                                                target="_blank" style="font-size: 10px; color: #999999">
                                                ICRA Analytics Ltd</a>, <a style="font-size: 10px;
                                                    color: #999999"
                                                    href="https://icraanalytics.com/home/Disclaimer"
                                                    target="_blank">Disclaimer </a>
                                        </div>
                                        <!-- / -->
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
