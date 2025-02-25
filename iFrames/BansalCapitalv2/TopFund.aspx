<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.BansalCapitalv2.TopFund" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Top Fund</title>

    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <script src="../Scripts/HighStockChart/highstock.js" type="text/javascript"></script>
    <script src="../Scripts/HighStockChart/exporting.js" type="text/javascript"></script>
    <script src="js/moment.min.js"></script>
    <!-- Bootstrap Css -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- Icons Css -->
    <link href="assets/css/icons.min.css" rel="stylesheet" type="text/css" />
    <!-- App Css-->
    <link href="assets/css/app.min.css" rel="stylesheet" type="text/css" />

    <!-- nouisliderribute css -->
    <%--<link rel="stylesheet" href="assets/libs/nouislider/nouislider.min.css" />--%>

    <style>
        .slider {
          -webkit-appearance: none;
          width: 100%;
          height: 15px;
          border-radius: 5px;  
          background: #d3d3d3;
          outline: none;
          opacity: 0.7;
          -webkit-transition: .2s;
          transition: opacity .2s;
        }

        .slider::-webkit-slider-thumb {
          -webkit-appearance: none;
          appearance: none;
          width: 25px;
          height: 25px;
          border-radius: 50%; 
          background: #04AA6D;
          cursor: pointer;
        }

        .slider::-moz-range-thumb {
          width: 25px;
          height: 25px;
          border-radius: 50%;
          background: #04AA6D;
          cursor: pointer;
        }
    </style>

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

     <%--Region: Value stores in HiddenField from Web.Config for FundRisk Button and fixed background-color when button is clicked--%>
    <script type="text/javascript">

        var FundRiskValue;

        $(document).ready(function () {

            var cssclass = "btn_nowrap btn-" + $("#HiddenFundRiskStrColor").val() + " active";

            $("#" + $("#HiddenFundRiskStrColor").val().replace("-", "_")).attr("class", cssclass);

            if (FundRiskValue == null) {
                $("#low").click(function (Func1) {
                    debugger;
                    FundRiskValue = $("#low").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap active");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("low");
                });

                $("#mod_low").click(function (Func2) {
                    FundRiskValue = $("#mod_low").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap active");
                    $("#mod").attr("class", "btn-mod btn_nowrap");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("mod-low");
                });

                $("#mod").click(function (Func2) {
                    FundRiskValue = $("#mod").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap active");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("mod");
                });

                $("#mod_high").click(function (Func2) {
                    FundRiskValue = $("#mod_high").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap active");
                    $("#high").attr("class", "btn-high btn_nowrap");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("mod-high");
                });

                $("#high").click(function (Func3) {
                    FundRiskValue = $("#high").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap ");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap active");
                    $("#all").attr("class", "btn-all btn_nowrap");
                    $("#HiddenFundRiskStrColor").val("high");
                });

                $("#all").click(function (Func4) {
                    FundRiskValue = $("#all").attr("for");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn-low btn_nowrap");
                    $("#mod_low").attr("class", "btn-mod-low btn_nowrap");
                    $("#mod").attr("class", "btn-mod btn_nowrap ");
                    $("#mod_high").attr("class", "btn-mod-high btn_nowrap");
                    $("#high").attr("class", "btn-high btn_nowrap ");
                    $("#all").attr("class", "btn-all btn_nowrap active");
                    $("#HiddenFundRiskStrColor").val("all");
                });
            }

            $("#FirstAddBtn").click(function (Fun1) {
                $("#BtnClickedNo").val("1");
            });
            $("#SecondAddBtn").click(function (Fun2) {
                $("#BtnClickedNo").val("2");
            });
            $("#ThirdAddBtn").click(function (Fun3) {
                $("#BtnClickedNo").val("3");
            });
            $("#FourthAddBtn").click(function (Fun4) {
                $("#BtnClickedNo").val("4");
            });

            var slider = document.getElementById("sliderMin");
            slider.oninput = function () {
                debugger;
                var value = $("#sliderMin").val();
                $("#HiddenMinimumInvesment").val(value);
            }                      

            var sliderSI = document.getElementById("sliderSI");

            sliderSI.oninput = function () {
                debugger;
                var value = $("#sliderSI").val();
                $("#HiddenMinimumSIReturn").val(value);
            }
                        
        });

    </script>
    <script type="text/javascript">


        function BindSchemeList() {
            var data2 = JSON.stringify({
                ddlCategoryCompare: $('#ddlCategoryCompare :selected').val(),
                ddlTypeCompare: $('#ddlTypeCompare :selected').val(),
                ddlSubCategoryCompare: $('#ddlSubCategoryCompare :selected').val(),
                rdbOptionCompare: $('#rdbOptionCompare :checked').val(),
                ddlFundHouseCompare: $('#ddlFundHouseCompare :selected').val()
            });

            $.ajax({
                url: "TopFund.aspx/getSchemesListCompare",
                type: "POST",
                data: data2,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var s = '<option value="-1">Select</option>';
                    var obj = JSON.parse(data.d);

                    for (var i = 0; i < obj.length; i++) {
                        s += '<option value="' + obj[i].Scheme_Id + '">' + obj[i].Sch_Short_Name + '</option>';
                    }
                    $("#ddlSchemesCompare").html(s);
                },
                error: function (error) {
                    console.log(`Error ${error}`);
                }
            });
        }

        function BindSchemeListNav() {
            var data2 = JSON.stringify({
                ddlCategoryCompare: $('#ddlCategoryNav :selected').val(),
                ddlTypeCompare: $('#ddlTypeNav :selected').val(),
                ddlSubCategoryCompare: $('#ddlSubCategoryNav :selected').val(),
                rdbOptionCompare: $('#rdbOptionNav :checked').val(),
                ddlFundHouseCompare: $('#ddlFundHouseNav :selected').val()
            });

            $.ajax({
                url: "TopFund.aspx/getSchemesListCompare",
                type: "POST",
                data: data2,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    $('ddlSchemesNav').empty();
                    var s = '<option value="-1">Select</option>';
                    var obj = JSON.parse(data.d);

                    for (var i = 0; i < obj.length; i++) {
                        s += '<option value="' + obj[i].Scheme_Id + '">' + obj[i].Sch_Short_Name + '</option>';
                    }
                    $("#ddlSchemesNav").html(s);
                },
                error: function (error) {
                    console.log(`Error ${error}`);
                }
            });
        }

        $(document).ready(function () {
            debugger;
            $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                sessionStorage.setItem('activeTabHref', $(e.target).attr('href'));
                sessionStorage.setItem('activeTabId', $(e.target).attr('id'));
            });
            var activeTabHref = sessionStorage.getItem('activeTabHref');
            var activeTabId = sessionStorage.getItem('activeTabId');

            if (activeTabHref != null && activeTabId != null) {
                $("#TopFundTab").removeClass("active");
                $("#TopFund").removeClass("active");
                $(activeTabHref).addClass("active");
                $("#"+activeTabId).addClass("active");
                debugger;

            }

            $('#ddlFundHouseCompare').on('change', function () {
                BindSchemeList();
            });
            $('#ddlCategoryCompare').on('change', function () {


                debugger;
                var data2 = JSON.stringify({
                    ddlCategoryCompare: $('#ddlCategoryCompare :selected').val(),

                });

                $.ajax({
                    url: "TopFund.aspx/LoadSubNatureCompare",
                    type: "POST",
                    data: data2,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        //$('#ddlSubCategoryCompare').empty();
                    },
                    success: function (data) {
                        debugger;
                        var s = '<option value="-1">All</option>';
                        var obj = JSON.parse(data.d);

                        for (var i = 0; i < obj.length; i++) {
                            s += '<option value="' + obj[i].Sebi_Sub_Nature_ID + '">' + obj[i].Sebi_Sub_Nature + '</option>';
                        }
                        $("#ddlSubCategoryCompare").html(s);
                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    }
                });

                BindSchemeList();

            });
            $('#ddlTypeCompare').on('change', function () {
                BindSchemeList();
            });
            $('#ddlSubCategoryCompare').on('change', function () {
                BindSchemeList();
            });
            $('#rdbOptionCompare').on('change', function () {
                BindSchemeList();
            });

            $('#ddlFundHouseNav').on('change', function () {
                BindSchemeListNav();
            });
            $('#ddlCategoryNav').on('change', function () {


                debugger;
                var data2 = JSON.stringify({
                    ddlCategoryCompare: $('#ddlCategoryNav :selected').val(),

                });

                $.ajax({
                    url: "TopFund.aspx/LoadSubNatureCompare",
                    type: "POST",
                    data: data2,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        //$('#ddlSubCategoryCompare').empty();
                    },
                    success: function (data) {
                        debugger;
                        $('ddlSubCategoryNav').empty();
                        var s = '<option value="-1">All</option>';
                        var obj = JSON.parse(data.d);

                        for (var i = 0; i < obj.length; i++) {
                            s += '<option value="' + obj[i].Sebi_Sub_Nature_ID + '">' + obj[i].Sebi_Sub_Nature + '</option>';
                        }
                        $("#ddlSubCategoryNav").html(s);
                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    }
                });

                BindSchemeList();

            });
            $('#ddlTypeNav').on('change', function () {
                BindSchemeListNav();
            });
            $('#ddlSubCategoryNav').on('change', function () {
                BindSchemeListNav();
            });
            $('#rdbOptionNav').on('change', function () {
                BindSchemeListNav();
            });

            $('#btnAddSchemeCompares').click(function (e) {
                debugger;
                e.preventDefault();
                var SchName = "";
                var SchId = "";
                var IndName = "";
                var IndId = "";
                var BtnClickNo = $('#BtnClickedNo').val();
                
                if ( BtnClickNo == "1") {
               
                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    $("#FirstAddSchemeH").html('');
                    $("#FirstAddSchemeH").append(SchName) ;

                    $("#CurrScheme1").val(SchName);
                    $("#CurrSchemeId1").val(SchId);
                    $("#CurrIndex1").val(IndName);
                    $("#CurrIndexId1").val(IndId);

                    $("#FirstAddBtn").hide();
                    $("#FirstAddScheme").show();

                } else if (BtnClickNo == "2") {

                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    $("#SecondAddSchemeH").html('');
                    $("#SecondAddSchemeH").append(SchName) ;

                    $("#CurrScheme2").val(SchName);
                    $("#CurrSchemeId2").val(SchId);
                    $("#CurrIndex2").val(IndName);
                    $("#CurrIndexId2").val(IndId);

                    $("#SecondAddBtn").hide();
                    $("#SecondAddScheme").show();


                } else if (BtnClickNo == "3") {
                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    $("#ThirdAddSchemeH").html('');
                    $("#ThirdAddSchemeH").append(SchName) ;

                    $("#CurrScheme3").val(SchName);
                    $("#CurrSchemeId3").val(SchId);
                    $("#CurrIndex3").val(IndName);
                    $("#CurrIndexId3").val(IndId);

                    $("#ThirdAddBtn").hide();
                    $("#ThirdAddScheme").show();

                } else {
                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    $("#FourthAddSchemeH").html('');
                    $("#FourthAddSchemeH").append(SchName) ;

                    $("#CurrScheme4").val(SchName);
                    $("#CurrSchemeId4").val(SchId);
                    $("#CurrIndex4").val(IndName);
                    $("#CurrIndexId4").val(IndId);

                    $("#FourthAddBtn").hide();
                    $("#FourthAddScheme").show();
                }
            });

            $('#btnCompareFund').click(function (e) {

                e.preventDefault();
                debugger;
                var dataToSend = JSON.stringify({
                    CurrSchemeId1: $("#CurrSchemeId1").val(),
                    CurrSchemeId2: $("#CurrSchemeId2").val(),
                    CurrSchemeId3: $("#CurrSchemeId3").val(),
                    CurrSchemeId4: $("#CurrSchemeId4").val(),

                    CurrIndexId1: $("#CurrIndexId1").val(),
                    CurrIndexId2: $("#CurrIndexId2").val(),
                    CurrIndexId3: $("#CurrIndexId3").val(),
                    CurrIndexId4: $("#CurrIndexId4").val()

                });


                //HdSchemes.Value = string.Join("#", SelectList[0].Split(',').Select(x => "s" + x.ToString())) + "#" +
                //    string.Join("#", SelectList[1].Split(',').Select(x => "i" + x.ToString()));

                var value = "#s" + $("#CurrSchemeId1").val() + "#s" + $("#CurrSchemeId2").val() + "#s" + $("#CurrSchemeId3").val() + "#s" + $("#CurrSchemeId4").val() + "#" +
                    "#i" + $("#CurrIndexId1").val() + "#i" + $("#CurrIndexId2").val() + "#i" + $("#CurrIndexId3").val() + "#i" + $("#CurrIndexId4").val();

                $("#HdSchemes").val(value);

                //$("#HdSchemes").val = $("#HdSchemes").val.TrimEnd('i');
                //$("#HdSchemes").val = $("#HdSchemes").val.TrimEnd('s');
                //$("#HdSchemes").val = $("#HdSchemes").val.TrimEnd('#');


                $("#HdToData").val(moment().format('DD MMM, yyyy'));
                //$("#HdFromData").val = DateTime.Today.AddYears(-3).ToString("dd MMM yyyy");
                $("#HdFromData").val(moment().add(-3,'y').format('DD MMM, yyyy'));

                $.ajax({
                    url: "TopFund.aspx/PopulateSchemeIndexCompareFund",
                    type: "POST",
                    data: dataToSend,
                    contentType: "application/json; charset=utf-8",

                    success: function (data) {
                        debugger;
                        var obj = JSON.parse(data.d);

                        var table = "<table id=tblResult><thead><tr> <th>Scheme Name</th> <th>6 months</th> <th>1Yr</th> <th>3 Yrs</th> <th>Since Inception</th> <th>Fund size (Cr)</th> </tr></thead><tbody>";
                         for (var i = 0; i < obj.length; i++) {
                             var row = "<tr>";



                             row += "<td>" + obj[i].Sch_Short_Name + "</td><td>" + obj[i].Per_182_Days + "</td><td>" + obj[i].Per_1_Year + "</td><td>" + obj[i].Per_3_Year + "</td><td>" + obj[i].Per_Since_Inception + "</td><td>" + obj[i].Per_Since_Inception + "</td>";
                             //row += "<td>" + obj[i].CurrentNav + "</td><td>" + obj[i].LblIncrNav + "</td><td>" + obj[i].Nature + "</td><td>" + obj[i].Nav_Rs + "</td><td>" + obj[i].Option_Id + "</td><td>" + obj[i].Per_1_Year + "</td><td>" + obj[i].Per_3_Year + "</td><td>" + obj[i].Per_30_Days + "</td><td>" + obj[i].Per_91_Days + "</td><td>" + obj[i].Per_182_Days + "</td><td>" + obj[i].Per_Since_Inception + "</td><td>" + obj[i].PrevNav + "</td><td>" + obj[i].Sch_Short_Name + "</td><td>" + obj[i].sch_id + "</td><td>" + obj[i].Structure_Name + "</td><td>" + obj[i].Sub_Nature + "</td><td>" + obj[i].status + "</td>";
                             //row += "<td>" + response.d[i].Salary + "</td>";
                             row += "</tr>";
                             table += row;
                         }

                         table += "</tbody></table>";
                         //$('#GrdCompFund').append(table);
                        $("#GrdCompFund").html(table);
                                                                     
                        $("#DivShowPerformance").show();
                        $("#lbRetrnMsg").show();
                        $("#lblSortPeriod").show();
                        $("#HighContainer").show();
                        btnPlotclick();
                        
                        
                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    }
                });

                

            });


            $('#DltBtnFirstSchm').click(function () {
                debugger;
                $("#FirstAddBtn").show();
                $("#FirstAddScheme").hide();
            });

            $('#DltBtnSecondSchm').click(function () {
                debugger;
                $("#SecondAddBtn").show();
                $("#SecondAddScheme").hide();
            });

            $('#DltBtnThirdSchm').click(function () {
                debugger;
                $("#ThirdAddBtn").show();
                $("#ThirdAddScheme").hide();
            });

            $('#DltBtnFourthSchm').click(function () {
                debugger;
                $("#FourthAddBtn").show();
                $("#FourthAddScheme").hide();
            });

            debugger;
            //btnPlotclick();
        });

        function btnPlotclick() {
            debugger;
            var schIndId = $('#<%=HdSchemes.ClientID %>').val();
            var frmDate = $('#<%=HdFromData.ClientID %>').val();
            var toData = $('#<%=HdToData.ClientID %>').val();
           
            if ((schIndId == "") || (schIndId == undefined)) {
                //alert("Please select at least one scheme.");
                return;
            }
            else {
                $("#HighContainer").show();
            }

            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "'}";
            $.ajax({
                type: "POST",
                url: "NAV.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    // setChart(msg.d);
                    debugger;
                    PlotAuto(msg.d, ImageArray);
                },
                error: function (msg) {
                    console.log(msg);
                    alert("Please select at least one scheme.");
                }
            });
        }

        function PlotAuto(dataConsolidated, ImageArray) {
            debugger;
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
                     points.push([Date.UTC(res[0], res[1]-1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue]);

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
                        var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate()+ ' ,' + d.getFullYear();

                        s = s + '<span style="color:#839557">' + navdate + '</span><br /><br />';

                        for (var i = 0; i < tt.length; i++)
                         {
                            for (var k = 0; k < tt[i].data.length; k++) 
                            {
                                if (this.x === tt[i].data[k][0]) 
                                {

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

        function btnPlotclickNav() {
            debugger;
            var schIndId = $('#<%=hidSchindSelected.ClientID %>').val();
            var frmDate = $('#<%=HdFromDataNav.ClientID %>').val();
            var toData = $('#<%=HdToDataNav.ClientID %>').val();
           
            if ((schIndId == "") || (schIndId == undefined)) {
                //alert("Please select at least one scheme.");
                return;
            }
            else {
                $("#HighContainerNav").show();
            }

            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "'}";
            $.ajax({
                type: "POST",
                url: "NAV.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    // setChart(msg.d);
                    debugger;
                    PlotAutoNav(msg.d, ImageArray);
                },
                error: function (msg) {
                    console.log(msg);
                    alert("Please select at least one scheme.");
                }
            });
        }

        function PlotAutoNav(dataConsolidated, ImageArray) {
            debugger;
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
                     points.push([Date.UTC(res[0], res[1]-1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue]);

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

            Highcharts.stockChart('HighContainerNav', {

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
                        var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate()+ ' ,' + d.getFullYear();

                        s = s + '<span style="color:#839557">' + navdate + '</span><br /><br />';

                        for (var i = 0; i < tt.length; i++)
                         {
                            for (var k = 0; k < tt[i].data.length; k++) 
                            {
                                if (this.x === tt[i].data[k][0]) 
                                {

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
    
    <script>
        function SomeDeleteRowFunction(o) {
            //no clue what to put here?
            var p = o.parentNode.parentNode;
            p.parentNode.removeChild(p);
        }
        $(document).ready(function () {
            var tmp = "";
            $("#btnAddSchemeNav").click(function () {
                debugger;
                var SchName = $('#ddlSchemesNav :selected').text();
                var SchId = $('#ddlSchemesNav :selected').val();
                var IndName = $('#ddlIndicesNav :selected').text();
                var IndId = $('#ddlIndicesNav :selected').val();

                var dataToSend = JSON.stringify({
                    SchName : SchName,
                    SchId : SchId,
                    IndName : IndName,
                    IndId : IndId


                });

                

                var value = "#s" + SchId + "#" + "#i" + IndId;
                tmp += value;
                $("#hidSchindSelected").val(tmp);

                //$("#HdSchemes").val(value);
                $("#HdToDataNav").val( moment().format('DD MMM, yyyy'));
                $("#HdFromDataNav").val(moment().add(-3, 'y').format('DD MMM, yyyy'));
                $.ajax({
                    url: "TopFund.aspx/btnAddSchemeNav",
                    type: "POST",
                    data: dataToSend,
                    contentType: "application/json; charset=utf-8",

                    success: function (data) {
                        debugger;
                        var obj = JSON.parse(data.d);
                        var flag = false;
                        var indval;
                        var oTable = document.getElementById('tblResultNav');
                        var rowLength = oTable.rows.length;
                       var row = "";
                        for (var i = 0; i < obj.length; i++) {

                            if (rowLength > 0) {
                                
                                if (obj[i].Sch_Short_Name != "") {
                                    row += "<tr><td>" + obj[i].Sch_Short_Name + "</td>";
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                    row += "<tr><td>" + obj[i].INDEX_NAME + "</td>";
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                    
                                    indval = "#i" + obj[i].INDEX_ID;
                                    tmp += indval;
                                    $("#hidSchindSelected").val(tmp);
                                } else {
                                    row += "<tr><td>" + obj[i].INDEX_NAME + "</td>";
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                }
                                flag = true;
                                

                            } else {
                                var table = "<thead><tr> <th>Scheme Name</th> <th>Delete</th>  </tr></thead><tbody>";
                                
                                if (obj[i].Sch_Short_Name != "") {
                                    row += "<tr><td>" + obj[i].Sch_Short_Name + "</td>";
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                    row += "<tr><td>" + obj[i].INDEX_NAME + "</td>";
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                    indval = "#i" + obj[i].INDEX_ID;
                                    tmp += indval;
                                    $("#hidSchindSelected").val(tmp);
                                } else {
                                    row += "<tr><td>" + obj[i].INDEX_NAME + "</td>";
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                }

                                
                                table += row;
                                table += "</tbody>";
                                
                            }
                           
                        }
                        if (flag) {
                            $("#tblResultNav").append(row);
                        } else {
                            $("#tblResultNav").append(table);
                        }
                        
                        $("#DivGridContain").show();
                        $("#btnPlotChart").show();
                        //btnPlotclick();


                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    }
                });



            });

            $("#btnPlotChart").click(function () {
                btnPlotclickNav();
            });
        });

    </script>

    
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="TabName" runat="server" Value="TopFund" />
        <!-- Begin page -->
        <div id="layout-wrapper">

            <!-- ============================================================== -->
            <!-- Start right Content here -->
            <!-- ============================================================== -->
            
             <div class="main-content">
                        <div class="page-content">
                            <div class="container-fluid">
                                <div class="row">
                                </div>
                                <div class="row">
                                    <div class="col-xl-12">
                                        <div class="card">
                                            <div class="card-header">
                                                <div>
                                                    <ul class="nav nav-pills nav-justified bg-light m-3 rounded" role="tablist" id="myTab">
                                                        <li class="nav-item waves-effect waves-light">
                                                            <a class="nav-link active" id="TopFundTab" data-bs-toggle="tab" href="#TopFund"
                                                                role="tab">
                                                                <!-- <span class="d-block d-sm-none"><i class="fas fa-home"></i></span> -->
                                                                <span class="d-block d-sm-block">Top Fund</span>
                                                        
                                                            </a>
                                                        </li>
                                                        <li class="nav-item waves-effect waves-light">
                                                            <a class="nav-link " id="SearchFundTab" data-bs-toggle="tab" href="#SearchFunds" role="tab">
                                                                <!-- <span class="d-block d-sm-none"><i class="far fa-user"></i></span> -->
                                                                <span class="d-block d-sm-block">Search Funds</span>
                                                            </a>
                                                        </li>
                                                        <li class="nav-item waves-effect waves-light">
                                                            <a class="nav-link" id="CompareFundTab" data-bs-toggle="tab" href="#CompareFunds"
                                                                role="tab">
                                                                <!-- <span class="d-block d-sm-none"><i class="far fa-envelope"></i></span> -->
                                                                <span class="d-block d-sm-block">Compare Funds</span>
                                                            </a>
                                                        </li>
                                                        <li class="nav-item waves-effect waves-light">
                                                            <a class="nav-link" id="NavTrackerTab" data-bs-toggle="tab" href="#NavTraker" role="tab">
                                                                <!-- <span class="d-block d-sm-none"><i class="far fa-envelope"></i></span> -->
                                                                <span class="d-block d-sm-block">Nav Tracker</span>
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <!-- end card header -->

                                            <div class="card-body">
                                                <!-- Tab panes -->
                                                <div class="tab-content text-muted">
                                                    <div class="tab-pane active" id="TopFund" role="tabpanel">
                                                        <div class="col-xl-12">
                                                            <div class="card">
                                                                <div class="card-body" style="padding: 0;">
                                                                    <!-- Nav tabs -->
                                                                    <ul class="nav nav-tabs nav-tabs-custom nav-justified"
                                                                        role="tablist">
                                                                        <li class="nav-item">
                                                                            <a class="nav-link active" data-bs-toggle="tab"
                                                                                href="#home1" role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="fas fa-home"></i></span>
                                                                                <span class="d-none d-sm-block">Small cap
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab"
                                                                                href="#profile1" role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="far fa-user"></i></span>
                                                                                <span class="d-none d-sm-block">Mid cap</span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab"
                                                                                href="#messages1" role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="far fa-envelope"></i></span>
                                                                                <span class="d-none d-sm-block">Large cap</span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab"
                                                                                href="#settings1" role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="fas fa-cog"></i></span>
                                                                                <span class="d-none d-sm-block">Multicap</span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab"
                                                                                href="#Flexicap1" role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="fas fa-cog"></i></span>
                                                                                <span class="d-none d-sm-block">Flexi cap</span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab" href="#Hybrid1"
                                                                                role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="fas fa-cog"></i></span>
                                                                                <span class="d-none d-sm-block">Hybrid</span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab" href="#Debt1"
                                                                                role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="fas fa-cog"></i></span>
                                                                                <span class="d-none d-sm-block">Debt</span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab"
                                                                                href="#Thematic1" role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="fas fa-cog"></i></span>
                                                                                <span class="d-none d-sm-block">Thematic</span>
                                                                            </a>
                                                                        </li>
                                                                        <li class="nav-item">
                                                                            <a class="nav-link" data-bs-toggle="tab" href="#ELSS1"
                                                                                role="tab">
                                                                                <span class="d-block d-sm-none"><i
                                                                                    class="fas fa-cog"></i></span>
                                                                                <span class="d-none d-sm-block">ELSS</span>
                                                                            </a>
                                                                        </li>
                                                                    </ul>

                                                                    <!-- Tab panes -->
                                                                    <div class="tab-content p-3 text-muted">
                                                                        <div class="tab-pane active" id="home1" role="tabpanel">
                                                                            <div class="card">
                                                                                <div class="card-body pb-xl-2">
                                                                                    <h4 class="font-size-20 mb-1"><a href="#"
                                                                                        class="text-dark">Axis Growth
                                                                                        Opportunities Fund-Growth
                                                                                    </a></h4>
                                                                                    <div class="col-xl-12">
                                                                                        <div class="row">
                                                                                            <div class="col-xl-4">
                                                                                                <div class="col-xl-12">
                                                                                                    <p>
                                                                                                        12.953 <span class="mdi mdi-arrow-up-bold text-success"></span>+ .0156 ( +1.19%)
                                                                                                    </p>
                                                                                                </div>
                                                                                                <div class="col-xl-12">
                                                                                                    <div class="row">
                                                                                                        <div class="col">
                                                                                                            <div>
                                                                                                                <h6 style="margin-bottom: 0px;">13.45 <small class="text-muted">(20 Jan 2023)</small>
                                                                                                                </h6>
                                                                                                                <p class="text-muted mb-0">
                                                                                                                    <span class="badge  badge-soft-success font-size-12">High Nav</span>
                                                                                                                </p>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col">
                                                                                                            <div>
                                                                                                                <h6 style="margin-bottom: 0px;">11.23 <small class="text-muted">(28 Oct 2023)</small>
                                                                                                                </h6>
                                                                                                                <p
                                                                                                                    class="text-muted mb-0">
                                                                                                                    <span class="badge  badge-soft-danger  font-size-12">Low Nav</span>
                                                                                                                </p>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>

                                                                                            <div class="col-xl-8">
                                                                                                <div class="row">
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">6 months</p>
                                                                                                            <h6>12.09%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">1 year</p>
                                                                                                            <h6>14.52%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">3 years</p>
                                                                                                            <h6>24.82%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">Since inception</p>
                                                                                                            <h6>16.78%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">Fund size (Cr)</p>
                                                                                                            <h6>2,324.14</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="row">
                                                                                                    <div class="col-sm-2 mt-2">
                                                                                                        <p class="mt-4 mb-0" style="text-align: right;">18.09% </p>
                                                                                                    </div>
                                                                                                    <div class="col-xl-8 mt-2">
                                                                                                        <div class="row align-items-center g-0">
                                                                                                            <div class="col-sm-12 mt-2">
                                                                                                                <p class="text-muted mb-0">
                                                                                                                    <b>Average Category Returns(3  yrs)</b>
                                                                                                                </p>
                                                                                                            </div>

                                                                                                            <div class="col-sm-12">
                                                                                                                <div class="progress progress-md mt-1">
                                                                                                                    <div class="progress-bar progress-bar-striped bg-primary"
                                                                                                                        role="progressbar"
                                                                                                                        style="width: 72%"
                                                                                                                        aria-valuenow="52"
                                                                                                                        aria-valuemin="0"
                                                                                                                        aria-valuemax="52">
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col-sm-2 mt-2">
                                                                                                        <p class="mt-4 mb-0" style="text-align: left;">27.56%</p>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-sm-12 mt-2" style="text-align: right; padding: 0px;">
                                                                                                <a href="#" class="text-primary fw-semibold">
                                                                                                    <u>More details</u>
                                                                                                    <i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>   
                                                                            </div>
                                                                            <div class="card">
                                                                                <div class="card-body pb-xl-2">
                                                                                    <h4 class="font-size-20 mb-1"><a href="#" class="text-dark">Kotak Mutual Multicap Fund-Growth </a></h4>
                                                                                    <div class="col-xl-12">
                                                                                        <div class="row">
                                                                                            <div class="col-xl-4">
                                                                                                <div class="col-xl-12">
                                                                                                    <p>
                                                                                                        12.953 <span class="mdi mdi-arrow-up-bold text-success"></span>+ .0156 ( +1.19%)
                                                                                                    </p>
                                                                                                </div>
                                                                                                <div class="col-xl-12">
                                                                                                    <div class="row">
                                                                                                        <div class="col">
                                                                                                            <div>
                                                                                                                <h6 style="margin-bottom: 0px;">13.45 <small class="text-muted">(20 Jan 2023)</small>
                                                                                                                </h6>
                                                                                                                <p class="text-muted mb-0">
                                                                                                                    <span class="badge badge-soft-success font-size-12">High Nav</span>
                                                                                                                </p>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col">
                                                                                                            <div>
                                                                                                                <h6 style="margin-bottom: 0px;">11.23 <small class="text-muted">(28 Oct 2023)</small>
                                                                                                                </h6>
                                                                                                                <p class="text-muted mb-0">
                                                                                                                    <span class="badge badge-soft-danger font-size-12">Low Nav</span>
                                                                                                                </p>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>

                                                                                            <div class="col-xl-8">
                                                                                                <div class="row">
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">6 months</p>
                                                                                                            <h6>12.09%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">
                                                                                                                1 year
                                                                                                            </p>
                                                                                                            <h6>14.52%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">3 years</p>
                                                                                                            <h6>24.82%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col border-end">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">Since inception</p>
                                                                                                            <h6>16.78%</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col">
                                                                                                        <div class="text-center">
                                                                                                            <p class="text-muted mb-0">
                                                                                                                Fund size (Cr)
                                                                                                            </p>
                                                                                                            <h6>2,324.14</h6>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="row">
                                                                                                    <div class="col-sm-2 mt-2">
                                                                                                        <p class="mt-4 mb-0" style="text-align: right;">18.09% </p>
                                                                                                    </div>
                                                                                                    <div class="col-xl-8 mt-2">
                                                                                                        <div class="row align-items-center g-0">
                                                                                                            <div class="col-sm-12 mt-2">
                                                                                                                <p class="text-muted mb-0">
                                                                                                                    <b>Average Category Returns(3
                                                                                                                    yrs)</b>
                                                                                                                </p>
                                                                                                            </div>

                                                                                                            <div class="col-sm-12">
                                                                                                                <div class="progress progress-md mt-1">
                                                                                                                    <div class="progress-bar progress-bar-striped bg-primary"
                                                                                                                        role="progressbar"
                                                                                                                        style="width: 72%"
                                                                                                                        aria-valuenow="52"
                                                                                                                        aria-valuemin="0"
                                                                                                                        aria-valuemax="52">
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col-sm-2 mt-2">
                                                                                                        <p class="mt-4 mb-0" style="text-align: left;">27.56%</p>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-sm-12 mt-2" style="text-align: right; padding: 0px;">
                                                                                        <a href="#" class="text-primary fw-semibold">
                                                                                            <u>More details</u>
                                                                                            <i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="tab-pane" id="profile1" role="tabpanel">
                                                                            <p class="mb-0"></p>
                                                                        </div>
                                                                        <div class="tab-pane" id="messages1" role="tabpanel">
                                                                            <p class="mb-0">
                                                                                Etsy mixtape wayfarers, ethical wes anderson tofu
                                                                            before they
                                                                            sold out mcsweeney's organic lomo retro fanny pack
                                                                            lo-fi
                                                                            farm-to-table readymade. Messenger bag gentrify
                                                                            pitchfork
                                                                            tattooed craft beer, iphone skateboard locavore
                                                                            carles etsy
                                                                            salvia banksy hoodie helvetica. DIY synth PBR banksy
                                                                            irony.
                                                                            Leggings gentrify squid 8-bit cred pitchfork.
                                                                            Williamsburg banh
                                                                            mi whatever gluten-free carles.
                                                                            </p>
                                                                        </div>
                                                                        <div class="tab-pane" id="settings1" role="tabpanel">
                                                                            <p class="mb-0">
                                                                                Trust fund seitan letterpress, keytar raw denim
                                                                            keffiyeh etsy
                                                                            art party before they sold out master cleanse
                                                                            gluten-free squid
                                                                            scenester freegan cosby sweater. Fanny pack portland
                                                                            seitan DIY,
                                                                            art party locavore wolf cliche high life echo park
                                                                            Austin. Cred
                                                                            vinyl keffiyeh DIY salvia PBR, banh mi before they
                                                                            sold out
                                                                            farm-to-table VHS viral locavore cosby sweater. Lomo
                                                                            wolf viral,
                                                                            mustache readymade keffiyeh craft.
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- end card-body -->
                                                            </div>
                                                            <!-- end card -->
                                                        </div>
                                                    </div>
                                                    <div class="tab-pane" id="SearchFunds" role="tabpanel">
                                                        <p class="mb-0">
                                                            <div class="card-body">
                                                                <form class="needs-validation" novalidate="novalidate">
                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip01">
                                                                                    Category</label>
                                                                                <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select"></asp:DropDownList>
                                                                                <div class="valid-tooltip">
                                                                                    Looks good!
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip02">
                                                                                    Sub-Category</label>
                                                                                <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-select"></asp:DropDownList>
                                                                                <div class="valid-tooltip">
                                                                                    Looks good!
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip02">
                                                                                    Type</label>
                                                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip03">
                                                                                    Option
                                                                                </label>
                                                                                <asp:RadioButtonList ID="rdbOption" runat="server" class="radio" RepeatDirection="Horizontal" Style="margin-left: 5px; padding-top: 5px;" CssClass="form-check"></asp:RadioButtonList></div>

                                                                                <div class="invalid-tooltip">
                                                                                    Please provide a valid city.
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip02">
                                                                                    Period</label>
                                                                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-select">
                                                                                    <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" />
                                                                                    <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                                                                                    <asp:ListItem Value="Per_91_Days" Text="Last 3 Months" />
                                                                                    <asp:ListItem Value="Per_182_Days" Text="Last 6 Months" />
                                                                                    <asp:ListItem Value="Per_1_Year" Text="Last 12 Months" Selected="True" />
                                                                                    <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                                                                                    <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip02">
                                                                                    Rank</label>
                                                                                <asp:DropDownList ID="ddlRank" runat="server" CssClass="form-select">
                                                                                    <asp:ListItem Text="All" Value="1000" />
                                                                                    <asp:ListItem Text="Top 5" Value="5" />
                                                                                    <asp:ListItem Text="Top 10" Value="10" />
                                                                                    <asp:ListItem Text="Top 15" Value="15" />
                                                                                    <asp:ListItem Text="Top 20" Value="20" />
                                                                                    <asp:ListItem Text="Top 25" Value="25" />
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                </form>
                                                            </div>
                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip03">
                                                                                    Minimum Investment
                                                                                </label>
                                                                                <div class="row">
                                                                                    <div class="col-md-12" style="padding-left: 10px;">
                                                                                        <div class="form-check mb-2"
                                                                                            style="padding-left: 0px;">
                                                                                            
                                                                                            <div class="slidecontainer">
                                                                                                <input id="sliderMin" class="slider" type="range" min="500" max="10000" step="500"/>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div style="color: #989898; margin-bottom: 25px; margin-left: 12px; padding-top: 8px;">
                                                                                    <img src="images/rupee.png" width="12" height="16" alt="Rs." />500
                                                                                </div>

                                                                                <div align="right" style="color: #989898; margin-top: -45px; margin-right: 10px;">
                                                                                    <img src="images/rupee.png" width="12" height="16" alt="Rs." />10,000
                                                                                </div>
                                                                                <div class="invalid-tooltip">
                                                                                    Please provide a valid city.
                                                                                </div>
                                                                            </div>
                                                                            <asp:HiddenField ID="HiddenMinimumInvesment" runat="server" Value="500" />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label"
                                                                                    for="validationTooltip03">
                                                                                    Minimum SI Return(%)
                                                                                </label>
                                                                                <div class="row">
                                                                                    <div class="col-md-12" style="padding-left: 10px;">
                                                                                        <div class="form-check mb-2"
                                                                                            style="padding-left: 0px;">
                                                                                    
                                                                                            <div class="slidecontainer">
                                                                                                <input id="sliderSI" type="range" class="slider" min="5" max="50" step="5"/>
                                                                                            </div>                                                                                                            
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div style="color: #989898; margin-bottom: 25px; margin-left: 12px; padding-top: 8px;">
                                                                                    5
                                                                                </div>

                                                                                <div align ="right" style="color: #989898; margin-top: -45px; margin-right: 10px;">
                                                                                    50
                                                                                </div>
                                                                                <div class="invalid-tooltip">
                                                                                    Please provide a valid city.
                                                                                </div>
                                                                            </div>
                                                                            <asp:HiddenField ID="HiddenMinimumSIReturn" runat="server" Value="5" />
                                                                            <asp:HiddenField ID="hdIsLoad" runat="server" Value="0" />
                                                                            <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <div class="mb-3 position-relative">
                                                                                <label class="form-label" for="validationTooltip02">
                                                                                    Fund
                                                                            Risk</label>
                                                                                <div class="">
                                                                                    <div class="btn-group btn-group-example btn-group-sm mb-3" role="group">

                                                                                        <button type="button" id="low" runat="server" title="Low Riskometer" data-risk="1,6" class="btn btn-primary btn-success-soft"> Low </button>
                                                                                        <button type="button" id="mod_low" runat="server" title="Moderately Low Riskometer" data-risk="2" class="btn btn-success w-xs"> Mid low </button>
                                                                                        <button type="button" id="mod" runat="server" title="Moderate Riskometer" data-risk="3" class="btn btn-info w-xs"> Mod </button>
                                                                                        <button type="button" id="mod_high" runat="server" title="Moderately High Riskometer" data-risk="4" class="btn btn-warning w-xs"> Mod High </button>
                                                                                        <button type="button" id="high" runat="server" title="High Riskometer" data-risk="5,10" class="btn btn-danger w-xs"> High</button>
                                                                                        <button type="button" id="all" runat="server" title="All" class="btn btn-danger w-xs"> All</button>
                                                                                        <asp:HiddenField ID="HiddenFundRisk" runat="server" Value="-1" />
                                                                                        <asp:HiddenField ID="HiddenFundRiskStrColor" runat="server" Value="All" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-12 mt-2" style="text-align: right; padding: 0px;">
                                                                        <%--<a href="#" class="btn btn-primary">Search</a>--%>
                                                                        <asp:Button ID="btnSubmit" class="btn btn-primary" Style="margin-right: 10px" runat="server" Text="Search" OnClick="btnSubmit_Click" />
                                                                    </div>
                                                        
                                                    
                                                            <div id="Result" runat="server">
                                                                <div class="muted pull-left" style="color: #cc0000; font-weight: 700;">
                                                                    <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                                                                    <div class="pull-right"></div>
                                                                </div>
                                                                <asp:Repeater ID="lstResult" runat="server" OnItemDataBound="lstResult_ItemDataBound">
                                                            <ItemTemplate>
                                                                <div class="card">
                                                                    <div class="card-body pb-xl-2">
                                                                        <h4 class="font-size-20 mb-1">
                                                                            <a class="text-dark" href="http://www.askmefund.com/factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank"><%#Eval("Sch_Name")%></a>
                                                                            <asp:HiddenField runat="server" ID="hdfSchemeId" Value='<%# Eval("SchemeId") %>' />
                                                                        </h4>
                                                                        <div class="col-xl-12">

                                                                            <div class="row">
                                                                                <div class="col-xl-4">
                                                                                    <div class="col-xl-12">
                                                                                        <p>
                                                                                            <asp:Label ID="lblPresentNav" runat="server"><%#Eval("CurrentNav")%></asp:Label> <asp:Label id="ImgArrow" runat="server"
                                                                                                class="mdi text-success"></asp:Label>
                                                                                            <asp:Label runat="server" ID="lblIncrNav"><%#Eval("LblIncrNav")%></asp:Label>
                                                                                            <asp:HiddenField runat="server" ID="CurrNav" Value='<%# Eval("CurrentNav") %>' />
                                                                                            <asp:HiddenField runat="server" ID="PrevNav" Value='<%# Eval("PrevNav") %>' />
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-xl-12">
                                                                                        <div class="row">
                                                                                            <div class="col">
                                                                                                <div>
                                                                                                    <h6 style="margin-bottom: 0px;"><asp:Label runat="server" ID="lblHigh"><%#Eval("LblHigh")%></asp:Label> <small class="text-muted">
                                                                                                        (<asp:Label id="spnMaxDate" runat="server"><%#Eval("SpnMaxDate")%></asp:Label>)</small>
                                                                                                    </h6>
                                                                                                    <p class="text-muted mb-0">
                                                                                                        <span
                                                                                                            class="badge badge-soft-success font-size-12">High
                                                                                                            Nav</span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col">
                                                                                                <div>
                                                                                                    <h6 style="margin-bottom: 0px;"><asp:Label runat="server" ID="lblLow"><%#Eval("LblLow")%></asp:Label><small class="text-muted">
                                                                                                        (<asp:Label id="spnMinDate" runat="server"><%#Eval("spnMinDate")%></asp:Label>)</small>
                                                                                                    </h6>
                                                                                                    <p class="text-muted mb-0">
                                                                                                        <span
                                                                                                            class="badge badge-soft-danger font-size-12">Low
                                                                                                            Nav</span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-xl-8">
                                                                                    <div class="row">
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">
                                                                                                    6 months
                                                                                                </p>
                                                                                                <h6><%#Eval("Per_182_Days")%></h6>

                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">
                                                                                                    1
                                                                                                    year
                                                                                                </p>
                                                                                                <h6><%#Eval("Per_1_Year")%></h6>

                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">
                                                                                                    3
                                                                                                    years
                                                                                                </p>
                                                                                                <h6><%#Eval("Per_3_Year")%></h6>

                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">
                                                                                                    Since inception
                                                                                                </p>
                                                                                                <h6><%#Eval("Since_Inception")%></h6>

                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">
                                                                                                    Fund size (Cr)
                                                                                                </p>
                                                                                                <h6><%#Eval("Fund_Size")%></h6>

                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-sm-2 mt-2">
                                                                                            <p class="mt-4 mb-0" style="text-align: right;">
                                                                                                18.09%
                                                                                            </p>
                                                                                        </div>
                                                                              
                                                                                        <div class="col-xl-8 mt-2" >
                                                                                            <div class="row align-items-center g-0" >
                                                                                                <div class="col-sm-12 mt-2">
                                                                                                    <p class="text-muted mb-0">
                                                                                                        Average Category
                                                                                                        Returns(3 yrs)
                                                                                                    </p>
                                                                                                </div>

                                                                                                <div class="col-sm-12">
                                                                                                    <div class="progress mt-1"
                                                                                                        style="height: 6px;">
                                                                                                        <asp:Label class="progress-bar progress-bar bg-primary" runat="server" id="lblprgbar"
                                                                                                            role="progressbar"
                                                                                                            style="width: 50%"
                                                                                                            aria-valuenow="0"
                                                                                                            aria-valuemin="0"
                                                                                                            aria-valuemax="52">

                                                                                                        </asp:Label>

                                                                                                        <asp:HiddenField runat="server" ID="hdfProgressBarWidth" Value='<%# Eval("CategoryAverage") %>' />
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-sm-2 mt-2">
                                                                                            <p class="mt-4 mb-0" style="text-align: left;">
                                                                                                27.56%
                                                                                            </p>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                            </div>
                                                                            <div class="col-sm-12 mt-2"
                                                                                style="text-align: right; padding: 0px;">
                                                                                <a href="http://www.askmefund.com/factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank" class="text-primary fw-semibold"><u>More details</u>
                                                                                    <i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                            </div>
                                                            
                                                            <div style="text-align:center">  
                                                                <asp:DataList ID="PgnData" runat="server" OnItemCommand="PgnData_ItemCommand" OnItemDataBound="PgnData_ItemDataBound">  
                                                                    <ItemTemplate>  
                                                                        <asp:LinkButton ID="lnkbtnPaging"  
                                                                            Style="padding: 8px; margin: 2px; background: lightgray; border: solid 1px #666; color: black; font-weight: bold"  
                                                                            CommandName="Paging" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageIndex") %>' runat="server" Font-Bold="True">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>  
                                                                </asp:DataList>  
                                                                <asp:Button id="_lbtnFirst" Text="First" runat="server" OnClick="_lbtnFirst_Click"></asp:Button>
                                                                <asp:Button id="_lbtnPrevious" Text="Previous" runat="server" OnClick="_lbtnPrevious_Click"></asp:Button>
                                                                <asp:Button id="_lbtnNext" Text="Next" runat="server" OnClick="_lbtnNext_Click"></asp:Button>
                                                                <asp:Button id="_lbtnLast" Text="Last" runat="server" OnClick="_lbtnLast_Click"></asp:Button>
                                                                <asp:Label runat="server" ID="lblPageInfo"></asp:Label>
                                                            </div> 
                                                        </p>
                                                        </div>
                                            
                                                    <div class="tab-pane" id="CompareFunds" role="tabpanel">
                                                        <div class="mb-3">
                                                            <div class="row">
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <label class="card-radio-label mb-0" >
                                                                            <span class="card-radio text-truncate">
                                                                                <div class="text-center mt-2 mb-2" id="FirstAddBtn" runat="server">
                                                                                    <button type="button" class="btn btn-success waves-effect rounded-circle" data-bs-keyboard="false" data-bs-backdrop="static"
                                                                                        data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg">
                                                                                        <i class="mdi mdi-plus"></i>
                                                                                    </button>
                                                                                    <asp:HiddenField ID="BtnClickedNo" runat="server" />
                                                                                    <h6 class="mt-2">Add Scheme</h6> 
                                                                                </div>
                                                                                <asp:HiddenField ID="CurrScheme1" runat="server" />
                                                                                <asp:HiddenField ID="CurrSchemeId1" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndex1" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndexId1" runat="server" />
                                                                                <div id="FirstAddScheme" runat="server" style="display:none">
                                                                                    <div class="text-center mt-2 mb-2" style="white-space: normal;">
                                                                                        <h6 id="FirstAddSchemeH">Aditya Birla Sun Life PSU Equity Fund Direct - Growth</h6>
                                                                                    </div>
                                                                                    <div class="edit-btn rounded">
                                                                                        <a id="DltBtnFirstSchm" data-bs-toggle="tooltip"
                                                                                            data-placement="top" title=""
                                                                                            data-bs-original-title="delete"
                                                                                            class="text-danger bg-light">
                                                                                            <i class="mdi mdi-delete-circle font-size-16 mt-2"></i>
                                                                                        </a>
                                                                                    </div>
                                                                                </div>
                                                                            </span>
                                                                            <!--  Large modal example -->
                                                                            <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
                                                                                        aria-labelledby="myLargeModalLabel" aria-hidden="true">
                                                                                        <div class="modal-dialog modal-lg">
                                                                                            <div class="modal-content">
                                                                                                <div class="modal-header">
                                                                                                    <h5 class="modal-title" id="myLargeModalLabel">Add Scheme</h5>
                                                                                                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                                                                                                        aria-label="Close">
                                                                                                    </button>
                                                                                                </div>
                                                                                                <div class="modal-body">
                                                                                                    <form class="needs-validation" novalidate="novalidate">
                                                                                                        <div class="row" style="text-align: left!important;">
                                                                                                            <div class="col-md-4">
                                                                                                                <div class="mb-3 position-relative">
                                                                                                                    <label class="form-label" for="validationTooltip01">
                                                                                                                        Mutual
                                                                                                                    Funds</label>
                                                                                                                    <asp:DropDownList ID="ddlFundHouseCompare" runat="server" AutoPostBack="false" CssClass="form-select"
                                                                                                                        OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChangedCompare" >
                                                                                                                    </asp:DropDownList>
                                                                                                                    <div class="valid-tooltip">Looks good! </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="col-md-4">
                                                                                                                <div class="mb-3 position-relative">
                                                                                                                    <label class="form-label" for="validationTooltip01">Category</label>
                                                                                                                    <asp:DropDownList ID="ddlCategoryCompare" runat="server" AutoPostBack="false" CssClass="form-select"
                                                                                                                        OnSelectedIndexChanged="ddlCategory_SelectedIndexChangedCompare">    
                                                                                                                    </asp:DropDownList>
                                                                                                                    <div class="valid-tooltip">Looks good! </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="col-md-4">
                                                                                                                <div class="mb-3 position-relative">
                                                                                                                    <label class="form-label" for="validationTooltip02">Sub-Category</label>
                                                                                                                    <asp:DropDownList ID="ddlSubCategoryCompare" runat="server" CssClass="form-select" AutoPostBack="false"
                                                                                                                        OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChangedCompare">
                                                                                                                    </asp:DropDownList>
                                                                                                                    <div class="valid-tooltip">Looks good! </div>
                                                                                                                </div>
                                                                                                            </div>

                                                                                                        </div>
                                                                                                        <div class="row"
                                                                                                            style="text-align: left!important;">
                                                                                                            <div class="col-md-4">
                                                                                                                <div
                                                                                                                    class="mb-3 position-relative">
                                                                                                                    <label
                                                                                                                        class="form-label"
                                                                                                                        for="validationTooltip02">
                                                                                                                        Type</label>
                                                                                                                    <asp:DropDownList ID="ddlTypeCompare" runat="server" AutoPostBack="false" CssClass="form-select"
                                                                                                                        OnSelectedIndexChanged="ddlType_SelectedIndexChangedCompare">
                                                                                                                    </asp:DropDownList>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="col-md-4">
                                                                                                                <div
                                                                                                                    class="mb-3 position-relative">
                                                                                                                    <label
                                                                                                                        class="form-label"
                                                                                                                        for="validationTooltip03">
                                                                                                                        Option
                                                                                                                    </label>
                                                                                                                    <asp:RadioButtonList ID="rdbOptionCompare" runat="server" CssClass="form-check"
                                                                                                                    class="radio" RepeatDirection="Horizontal" AutoPostBack="false"
                                                                                                                    Style="margin-left: 15px;"
                                                                                                                    OnSelectedIndexChanged="rdbOption_SelectedIndexChangedCompare">
                                                                                                                    </asp:RadioButtonList>
                                                                                                                    <div
                                                                                                                        class="invalid-tooltip">
                                                                                                                        Please
                                                                                                                    provide a
                                                                                                                    valid city.
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="col-md-4">
                                                                                                                <div
                                                                                                                    class="mb-3 position-relative">
                                                                                                                    <label
                                                                                                                        class="form-label"
                                                                                                                        for="validationTooltip02">
                                                                                                                        Choose
                                                                                                                    Scheme</label>
                                                                                                                    <asp:DropDownList ID="ddlSchemesCompare" runat="server" CssClass="form-select" >
                                                                                                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                                                                                                        ControlToValidate="ddlSchemesCompare" Display="Dynamic" InitialValue="0" ValidationGroup="scheme">
                                                                                                                    </asp:RequiredFieldValidator>
                                                                                                                </div>
                                                                                                            </div>

                                                                                                        </div>
                                                                                                        <div class="row"
                                                                                                            style="text-align: left!important;">
                                                                                                            <div class="col-md-4">
                                                                                                                <div
                                                                                                                    class="mb-3 position-relative">
                                                                                                                    <label
                                                                                                                        class="form-label"
                                                                                                                        for="validationTooltip02">
                                                                                                                        Index</label>
                                                                                                                     <asp:DropDownList ID="ddlIndicesCompare" runat="server" CssClass="form-select">
                                                                                                                    </asp:DropDownList>
                                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                                                                                                        ControlToValidate="ddlIndicesCompare" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="col-md-4">
                                                                                                            </div>


                                                                                                            <div class="col-md-4"
                                                                                                                style="text-align: right;">
                                                                                                                <label
                                                                                                                    class="form-label"
                                                                                                                    for="validationTooltip02">
                                                                                                                    &nbsp;</label>
                                                                                                                <div
                                                                                                                    class="mt-1 position-relative">
                                                                                                                    <button data-bs-toggle="modal"
                                                                                                                        data-bs-target=".add-new"
                                                                                                                        class="btn btn-success btn-md waves-effect waves-light" type="button" id="btnAddSchemeCompares" >Add</button>
                                                                                                        
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </form>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                            <!--  Large modal example -->
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <label class="card-radio-label mb-0">
                                                                            <div class="card-radio p-2">
                                                                                <div id="SecondAddScheme" runat="server" style="display:none">
                                                                                    <div class="text-center mt-2 mb-2" style="white-space: normal;">
                                                                                        <h6 id="SecondAddSchemeH">Aditya Birla Sun Life PSU Equity Fund Direct - Growth</h6>
                                                                                    </div>
                                                                                    <div class="edit-btn rounded">
                                                                                        <a id="DltBtnSecondSchm" data-bs-toggle="tooltip"
                                                                                            data-placement="top" title=""
                                                                                            data-bs-original-title="delete"
                                                                                            class="text-danger bg-light">
                                                                                            <i class="mdi mdi-delete-circle font-size-16 mt-2"></i>
                                                                                        </a>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="text-center mt-2 mb-2" id="SecondAddBtn" runat="server">
                                                                                    <button type="button" class="btn btn-success waves-effect rounded-circle" data-bs-keyboard="false" data-bs-backdrop="static"
                                                                                        data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg">
                                                                                        <i class="mdi mdi-plus"></i>
                                                                                    </button>
                                                                                    <h6 class="mt-2">Add Scheme</h6> 
                                                                                </div>
                                                                                <asp:HiddenField ID="CurrScheme2" runat="server" />
                                                                                <asp:HiddenField ID="CurrSchemeId2" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndex2" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndexId2" runat="server" />
                                                                                
                                                                            </div>

                                                                        </label>

                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <label class="card-radio-label mb-0">
                                                                            <span class="card-radio text-truncate">
                                                                                <div class="text-center mt-2 mb-2" id="ThirdAddBtn" runat="server">
                                                                                    <button type="button" class="btn btn-success waves-effect rounded-circle"
                                                                                        data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg">
                                                                                        <i
                                                                                            class="mdi mdi-plus"></i>
                                                                                    </button>
                                                                                    <h6 class="mt-2">Add Scheme</h6>
                                                                                </div>
                                                                                <asp:HiddenField ID="CurrScheme3" runat="server" />
                                                                                <asp:HiddenField ID="CurrSchemeId3" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndex3" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndexId3" runat="server" />
                                                                                <div id="ThirdAddScheme" runat="server" style="display:none">
                                                                                    <div class="text-center mt-2 mb-2" style="white-space: normal;">
                                                                                        <h6 id="ThirdAddSchemeH">Aditya Birla Sun Life PSU Equity Fund Direct - Growth</h6>
                                                                                    </div>
                                                                                    <div class="edit-btn rounded">
                                                                                        <a id="DltBtnThirdSchm" data-bs-toggle="tooltip"
                                                                                            data-placement="top" title=""
                                                                                            data-bs-original-title="delete"
                                                                                            class="text-danger bg-light">
                                                                                            <i class="mdi mdi-delete-circle font-size-16 mt-2"></i>
                                                                                        </a>
                                                                                    </div>
                                                                                </div>
                                                                            </span>
                                                                        </label>
                                                                        <!-- <div class="edit-btn bg-light rounded">
                                                                        <a href="#"  data-bs-toggle="tooltip" data-placement="top" title="" data-bs-original-title="Edit">
                                                                            <i class="bx bx-pencil font-size-16"></i>
                                                                        </a>
                                                                    </div> -->
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <label class="card-radio-label mb-0">
                                                                            <span class="card-radio text-truncate">
                                                                                <div id="FourthAddScheme" runat="server" style="display:none">
                                                                                    <div class="text-center mt-2 mb-2" style="white-space: normal;">
                                                                                        <h6 id="FourthAddSchemeH">Aditya Birla Sun Life PSU Equity Fund Direct - Growth</h6>
                                                                                    </div>
                                                                                    <div class="edit-btn rounded">
                                                                                        <a id="DltBtnFourthSchm" data-bs-toggle="tooltip"
                                                                                            data-placement="top" title=""
                                                                                            data-bs-original-title="delete"
                                                                                            class="text-danger bg-light">
                                                                                            <i class="mdi mdi-delete-circle font-size-16 mt-2"></i>
                                                                                        </a>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="text-center mt-2 mb-2" id="FourthAddBtn" runat="server">
                                                                                    <button type="button" class="btn btn-success waves-effect rounded-circle"
                                                                                        data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg">
                                                                                        <i
                                                                                            class="mdi mdi-plus"></i>
                                                                                    </button>
                                                                                    <h6 class="mt-2">Add Scheme</h6>
                                                                                </div>
                                                                                <asp:HiddenField ID="CurrScheme4" runat="server" />
                                                                                <asp:HiddenField ID="CurrSchemeId4" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndex4" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndexId4" runat="server" />
                                                                            </span>
                                                                        </label>
                                                                        <!-- <div class="edit-btn bg-light rounded">
                                                                        <a href="#"  data-bs-toggle="tooltip" data-placement="top" title="" data-bs-original-title="Edit">
                                                                            <i class="bx bx-pencil font-size-16"></i>
                                                                        </a>
                                                                    </div> -->
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-sm-12 mt-2 mb-3"
                                                                    style="text-align: right;">
                                                                    <button id="btnCompareFund" class="btn btn-primary">
                                                                        Show Performance
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="row mt-2">
                                                                <div class="col-sm-12">
                                                                    <div class="card">
                                                                        <div class="card-body pb-xl-2">
                                                                            <div id="DivShowPerformance" runat="server" style="display:none;">
                                                                                <div>
                                                                                    <div>
                                                                                        <label id="lblSortPeriod"  style="display:none;" class="gap-left">Click on 'Time Period' to rank funds on a particular period of your choice.
                                                                                        </label>
                                                                                    </div>
                                                                                    <%--<div>
                                                                                        <div class="table-responsive">--%>
                                                                                            <%--<asp:GridView ID="GrdCompFund" runat="server" Style="font-size: 12px;" Width="100%" AutoGenerateColumns="false"
                                                                                                OnRowCommand="GrdCompFund_RowCommand" OnRowDataBound="GrdCompFund_RowDataBound">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="Scheme Name"  HeaderStyle-CssClass="fw-bold">
                                                                                                        <ItemTemplate>
                                                                                                            <div>
                                                                                                                <h5 class="text-truncate font-size-14 mb-1"><%# SetHyperlinkFundDetail(Eval("Sch_id").ToString(), Eval("Sch_Short_Name").ToString())%>
                                                                                                                </h5>
                                                                                                                <%# !string.IsNullOrEmpty(Eval("CurrentNav").ToString()) ? Eval("CurrentNav") : ""%>  
                                                                                                                <asp:Label id="ImgArrowCompare" runat="server" CssClass="mdi text-success"></asp:Label>
                                                                                                                <%# !string.IsNullOrEmpty(Eval("LblIncrNav").ToString()) ? Eval("LblIncrNav") : ""%>
                                                                                                                <asp:HiddenField runat="server" ID="CurrNavCompare" Value='<%# !string.IsNullOrEmpty(Eval("CurrentNav").ToString()) ? Eval("CurrentNav") : "" %>' />
                                                                                                                <asp:HiddenField runat="server" ID="PrevNavCompare" Value='<%# !string.IsNullOrEmpty(Eval("PrevNav").ToString()) ? Eval("PrevNav") : "" %>' />
                                                                                                            </div>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            <asp:LinkButton ID="Lnk6mth" runat="server" Font-Bold="true" Text="6 months"
                                                                                                                Font-Overline="false" CommandName="Per_182_Days" CssClass="text fw-bold"></asp:LinkButton>
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lbl6Mth" runat="server">
                                                                                                                    <%# !string.IsNullOrEmpty(Eval("Per_182_Days").ToString()) ? Eval("Per_182_Days") : "NA"%> 
                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            <asp:LinkButton ID="Lnk1yr" runat="server" Font-Bold="true" Text="1Yr"
                                                                                                                Font-Overline="false" CommandName="Per_1_Year" CssClass="text fw-bold"></asp:LinkButton>
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lbl1yr" runat="server">
                                                                                                                <%# !string.IsNullOrEmpty(Eval("Per_1_Year").ToString()) ? Eval("Per_1_Year") : "NA"%> 

                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            <asp:LinkButton ID="Lnk3yr" runat="server" Font-Bold="true" Text="3 Yrs"
                                                                                                                Font-Overline="false" CommandName="Per_3_Year" CssClass="text fw-bold"></asp:LinkButton>
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lbl3yr" runat="server">
                                                                                                                <%# !string.IsNullOrEmpty(Eval("Per_3_Year").ToString()) ? Eval("Per_3_Year") : "NA"%> 

                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            <asp:LinkButton ID="LnkSI" runat="server" Font-Bold="true" Text="Since Inception"
                                                                                                                Font-Overline="false" CommandName="Per_Since_Inception" CssClass="text fw-bold"></asp:LinkButton>
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblSI" runat="server">
                                                                                                                <%# !string.IsNullOrEmpty(Eval("Per_Since_Inception").ToString()) ? Eval("Per_Since_Inception") : "NA"%>

                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            <asp:LinkButton ID="LnkFund" runat="server" Font-Bold="true" Text="Fund size (Cr)"
                                                                                                                Font-Overline="false" CommandName="Per_Since_Inception" CssClass="text fw-bold"></asp:LinkButton>
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblFund" runat="server">
                                                                                                                <%# !string.IsNullOrEmpty(Eval("Per_Since_Inception").ToString()) ? Eval("Per_Since_Inception") : "NA"%>

                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                <EmptyDataTemplate>
                                                                                                    No Data Found
                                                                                                </EmptyDataTemplate>
                                                                                            </asp:GridView>--%>
                                                                                        <%--</div>
                                                                                    </div>--%>
                                                                                    <div id="GrdCompFund">

                                                                                    </div>
                                                                                    
                                                                                        <label id="lbRetrnMsg" class="gap-left" style="display:none;">
                                                                                            *Note:- Returns calculated for less than 1 year are Absolute returns and returns
                                                                                            calculated for more than 1 year are compounded annualized.</label>
                                                                                </div>
                                                                                <div style="margin-top: 40px; margin-right: 0px; margin-left: -15px;">
                                                                                    <input id="HdSchemes" type="hidden" runat="server" />
                                                                                    <input id="HdToData" type="hidden" runat="server" />
                                                                                    <input id="HdFromData" type="hidden" runat="server" />

                                                                                    
                                                                                      
                                                                                    <div id="HighContainer" style="height: 600px; display:none; min-width: 310px; max-width: 1000px"></div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="tab-pane" id="NavTraker" role="tabpanel">
                                                        <div class="card-body">
                                                            <form class="needs-validation" novalidate="novalidate">
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3 position-relative">
                                                                            <label class="form-label"
                                                                                for="validationTooltip01">
                                                                                Mutual Funds</label>
                                                                            <asp:DropDownList ID="ddlFundHouseNav" runat="server" AutoPostBack="false" 
                                                                                OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChangedNav" CssClass="form-select" Style="max-height: 50px;">
                                                                            </asp:DropDownList>
                                                                            <div class="valid-tooltip">
                                                                                Looks good!
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3 position-relative">
                                                                            <label class="form-label"
                                                                                for="validationTooltip01">
                                                                                Category</label>
                                                                            <asp:DropDownList ID="ddlCategoryNav" runat="server" CssClass="form-select"
                                                                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChangedNav" AutoPostBack="false">
                                                                            </asp:DropDownList>
                                                                            <div class="valid-tooltip">
                                                                                Looks good!
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3 position-relative">
                                                                            <label class="form-label"
                                                                                for="validationTooltip02">
                                                                                Sub-Category</label>
                                                                            <asp:DropDownList ID="ddlSubCategoryNav" runat="server" CssClass="form-select" AutoPostBack="false"
                                                                                OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChangedNav">
                                                                            </asp:DropDownList>
                                                                            <div class="valid-tooltip">
                                                                                Looks good!
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3 position-relative">
                                                                            <label class="form-label"
                                                                                for="validationTooltip02">
                                                                                Type</label>
                                                                            <asp:DropDownList ID="ddlTypeNav" runat="server" AutoPostBack="false"
                                                                                OnSelectedIndexChanged="ddlType_SelectedIndexChangedNav" CssClass="form-select">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3 position-relative">
                                                                            <label class="form-label"
                                                                                for="validationTooltip03">
                                                                                Option
                                                                            </label>
                                                                            <asp:RadioButtonList ID="rdbOptionNav" runat="server"
                                                                            CssClass="form-check" RepeatDirection="Horizontal" AutoPostBack="false"
                                                                            OnSelectedIndexChanged="rdbOption_SelectedIndexChangedNav">
                                                                        </asp:RadioButtonList>
                                                                            <div class="invalid-tooltip">
                                                                                Please provide a valid city.
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3 position-relative">
                                                                            <label class="form-label"
                                                                                for="validationTooltip02">
                                                                                Choose Scheme</label>
                                                                            <asp:DropDownList ID="ddlSchemesNav" runat="server" CssClass="form-select">
                                                                                <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Select Scheme"
                                                                            ControlToValidate="ddlSchemesNav" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3 position-relative">
                                                                            <label class="form-label"
                                                                                for="validationTooltip02">
                                                                                Index</label>
                                                                            <asp:DropDownList ID="ddlIndicesNav" runat="server" CssClass="form-select">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Select Index"
                                                                            ControlToValidate="ddlIndicesNav" Display="Dynamic" InitialValue="0" ValidationGroup="Index"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <label class="form-label"
                                                                            for="validationTooltip02">
                                                                            &nbsp;</label>
                                                                        <div class="mt-1 position-relative">
                                                                            <button type="button"
                                                                                class="btn btn-success btn-md waves-effect waves-light" id="btnAddSchemeNav">Add</button>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-4">
                                                                    </div>
                                                                </div>
                                                                <div id="DivGridContain" style="display:none">
                                                                    <table id="tblResultNav">

                                                                    </table>
                                                                    <button type="button" id="btnPlotChart" class="btn btn-info btn-md waves-effect waves-light" style="display:none">Plot Chart</button>
                                                                </div>
                                                                <div style="margin-top: 40px; margin-right: 0px; margin-left: -15px;">
                                                                    <input id="hidSchindSelected" type="hidden" runat="server" />
                                                                    <input id="HdToDataNav" type="hidden" runat="server" />
                                                                    <input id="HdFromDataNav" type="hidden" runat="server" />
                                                                    


                                                                                    
                                                                                      
                                                                    <div id="HighContainerNav" style="height: 600px; display:none; min-width: 310px; max-width: 1000px"></div>
                                                                </div>
                                                            </form>
                                                        </div>
                                                    </div>

                                                    











                                                </div>
                                            </div>
                                            <!-- end card-body -->
                                        </div>
                                        <!-- end card -->
                                    </div>
                                    <!-- end col -->
                                </div>
                                <!-- end row -->


                            </div>
                            <!-- container-fluid -->
                        </div>
                        <!-- End Page-content -->


                    </div>
             <!-- end main content-->
                

        </div>
        <!-- END layout-wrapper -->


        <!-- JAVASCRIPT -->
        <script src="assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
        <script src="assets/libs/metismenujs/metismenujs.min.js"></script>
        <script src="assets/libs/simplebar/simplebar.min.js"></script>
        <script src="assets/libs/feather-icons/feather.min.js"></script>

        <script src="assets/js/app.js"></script>

        <!-- nouisliderribute js -->
        <%--<script src="assets/libs/nouislider/nouislider.min.js"></script>
        <script src="assets/libs/wnumb/wNumb.min.js"></script>--%>

        <!-- range slider init -->
        <%--<script src="assets/js/range-sliders.init.js"></script>--%>

    </form>


</body>
</html>
