<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="NAV.aspx.cs" Inherits="iFrames.BansalCapital.NAV" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html class="no-js">

<head>
    <title>NAV Graph</title>
    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <script src="js/AutoComplete.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="css/jquery-slider.css" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="js/date.js"></script>
  
    <link href="css/auto.css" rel="stylesheet" type="text/css" />
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

    <script type="text/javascript" language="javascript">
        function Menuclick(url) {
            debugger;
            var data = { 'url': url };
            top.postMessage(data, 'http://www.askmefund.com/mutual-fund-nav-graph.aspx'); 
            
        }
        function ValidateControl() {

            var selectedFund = $('#<%=ddlFundHouse.ClientID %>').find(':selected').val();
               if (selectedFund == 0) {
                   alert('Please select any Fund House.');
                   $('#<%=ddlFundHouse.ClientID %>').focus();
                   return false;
               }
               var selectedValue = $('#<%=ddlSchemes.ClientID%> option:selected').val();
               if (selectedValue == null) {
                   alert('Please select any Scheme.');
                   $('#<%=ddlSchemes.ClientID %>').focus();
                   return false;
               }
               if ($('#<%=txtfromDate.ClientID %>').val() == '') {
                   alert('Please enter From date.');
                   $('#<%=txtfromDate.ClientID %>').focus();
                   return false;
               }
               if ($('#<%=txtToDate.ClientID %>').val() == '') {
                   alert('Please enter End date.');
                   $('#<%=txtToDate.ClientID %>').focus();
                   return false;
               }
               var listCount = CountItemList();
               if (listCount >= 4) {
                   alert("The List can contain maximum 5 Item");
                   return false;
               }
               return true;
           }

           function Listcount() {
               var listCount = CountItemList();
               if (listCount == 5) {
                   alert("The List can contain maximum 5 Item");
                   return false;
               }
               return true;
           }

           $(function () {
               $('#btnReset').click(function (ev) {
                   $("#ddlFundHouse").val(0);
                   $("#ddlCategory").val("-1");
                   $("#ddlSubCategory").val("-1");
                   $("#ddlType").val("-1");
                   $("#ddlIndices").val(0);
                   //$("#ddlSchemes").html('<option value="0">Select</option>');
                    window.location.href = "";
               });
               //            $('#<%=txtfromDate.ClientID %>').val(Date.parse('today').add(-8).days().toString("dd MMM yyyy"));
               //            $('#<%=txtToDate.ClientID %>').val(Date.parse('today').add(-2).days().toString("dd MMM yyyy"));
               //            $('#btplotChart').click(function () {
               //                btnPlotclick();
               //            });
               initdates();
               setDates();
           });

           function initdates() {
               $('#<%=txtfromDate.ClientID %>').datepicker({
                   showOn: "button",
                   buttonImageOnly: true,
                   buttonImage: "img/calenderb.jpg",
                   //                buttonText: "Select Date",
                   dateFormat: 'dd M yy',
                   changeMonth: true,
                   changeYear: true,
                   maxDate: -2
               });

               $('#<%=txtToDate.ClientID %>').datepicker({
                   showOn: "button",
                   buttonImage: "img/calenderb.jpg",
                   buttonImageOnly: true,
                   //                buttonText: "Select Date",
                   dateFormat: 'dd M yy',
                   changeMonth: true,
                   changeYear: true,
                   maxDate: -1
               });
           }
           window.onload = initdates;
           function setDates() {
               var dateOffset = (24 * 60 * 60 * 1000);
               var myDate = new Date();
               var frmdate = new Date(myDate.getTime() - dateOffset * 8);
               var todate = new Date(myDate.getTime() - dateOffset * 2);
               $('#<%=txtfromDate.ClientID %>').datepicker().datepicker('setDate', frmdate);
               $('#<%=txtToDate.ClientID %>').datepicker().datepicker('setDate', todate);
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

        function btnPlotclick() {
           // debugger;
               var regex = new RegExp("^[0-9]{2} (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) [0-9]{4}$", "i");
               if ($('#<%=rbDateRange.ClientID %>').is(":checked")) {
                   if ($('#<%=txtfromDate.ClientID %>').val() == '') {
                       alert('Please enter From date.');
                       $('#<%=txtfromDate.ClientID %>').focus();
                       return false;
                   }
                   if ($('#<%=txtToDate.ClientID %>').val() == '') {
                       alert('Please enter To date.');
                       $('#<%=txtToDate.ClientID %>').focus();
                       return false;
                   }
                   if (!$('#<%=txtfromDate.ClientID %>').val().match(regex)) {
                       alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                       $('#<%=txtfromDate.ClientID %>').focus();
                       return false;
                   }
                   if (Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                       alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                       $('#<%=txtfromDate.ClientID %>').focus();
                       return false;
                   }
                   if (!dtvalid(Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                       alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                       $('#<%=txtfromDate.ClientID %>').focus();
                       return false;
                   }
                   if (!$('#<%=txtToDate.ClientID %>').val().match(regex)) {
                       alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                       $('#<%=txtToDate.ClientID %>').focus();
                       return false;
                   }
                   if (Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                       alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                       $('#<%=txtToDate.ClientID %>').focus();
                       return false;
                   }
                   if (!dtvalid(Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                       alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                       $('#<%=txtToDate.ClientID %>').focus();
                       return false;
                   }
                   var frmdate = converterdate($('#<%=txtfromDate.ClientID %>').val());
                   var todate = converterdate($('#<%=txtToDate.ClientID %>').val());
                   if (!IsValidDate(frmdate, todate)) {
                       alert("From Date should be Less than To Date ");
                       $('#<%=txtToDate.ClientID %>').focus();
                       return false;
                   }
                   if (!LessThanToday(frmdate)) {
                       alert("From Date should be Less than Today ");
                       $('#<%=txtfromDate.ClientID %>').focus();
                       return false;
                   }
                   if (!LessThanToday(todate)) {
                       alert("To Date should be Less than Today ");
                       $('#<%=txtToDate.ClientID %>').focus();
                       return false;
                   }
               }

               var schIndId = $('#<%=hidSchindSelected.ClientID %>').val();
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
                   alert('Please Select at least One Item from the List');
                   return false;
               }
               var vdate = setDateRange();

               var data = {};
               data.minDate = vdate.fromdate;
               data.maxDate = vdate.enddate;
               data.schemeIndexIds = schIndId;
               var val = "{'schIndexid':'" + schIndId + "', startDate:'" + vdate.fromdate + "', endDate:'" + vdate.enddate + "'}";
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
                       // showtest();
                   },
                   error: function (msg) {
                       alert("Please Select at least One Item from the List.");
                   }
               });
           }

           function setChart(val) {

               var ChartData = Plot(val);
               alert(ChartData);
               var plot2 = $.jqplot('divChart', ChartData, {
                   title: val,
                   axesDefaults: {
                       labelRenderer: $.jqplot.CanvasAxisLabelRenderer
                   },
                   axes: {
                       xaxis: {
                           label: "X Axis",
                           min: val.MaxDate,
                           max: val.MinDate,
                           renderer: $.jqplot.DateAxisRenderer,
                           pad: 0
                       },
                       yaxis: {
                           label: "Y Axis"
                       }
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

               return dataPlot;
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

           function saveImg() {
               alert("test");
               var graphicImage = $('#plotted_image_div');
               var divGraph = $('#divChart').jqplotToImageStr({});
               var divElem = $('<img/>').attr('src', divGraph);
               graphicImage.html(divElem);
               $('#plotted_image_div').css('display', 'block');

               // dont delete the statement(2-1)
               var dataToPush = JSON.stringify({
                   baseimg: divGraph
               });

               $.ajax({
                   type: "POST",
                   url: "../DSP/Retirement/WebMethod.aspx/setValueChartimg",
                   async: false,
                   contentType: "application/json",
                   data: dataToPush,
                   dataType: "json",
                   success: function (msg) {
                       return true;
                   },
                   error: function (msg) {
                       // alert("Error! Try again...");
                   }
               });
               // end (2-1)
           }

    </script>

    <script type="text/javascript">
        var dminyear = 1900;
        var dmaxyear = 2200;
        var chsep = "/";
        var DateDiff = {

            inDays: function (d1, d2) {
                var t2 = d2.getTime();
                var t1 = d1.getTime();

                return parseInt((t2 - t1) / (24 * 3600 * 1000));
            },

            inWeeks: function (d1, d2) {
                var t2 = d2.getTime();
                var t1 = d1.getTime();

                return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
            },

            inMonths: function (d1, d2) {
                var d1Y = d1.getFullYear();
                var d2Y = d2.getFullYear();
                var d1M = d1.getMonth();
                var d2M = d2.getMonth();
                return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
            },

            inYears: function (d1, d2) {
                return d2.getFullYear() - d1.getFullYear();
            }
        }

        function checkinteger(str1) {
            var x;
            for (x = 0; x < str1.length; x++) {
                // verify current character is number or not !   
                var cr = str1.charAt(x);
                if (((cr < "0") || (cr > "9")))
                    return false;
            }
            return true;
        }
        function getcharacters(s, chsep1) {
            var x;
            var Stringreturn = "";
            for (x = 0; x < s.length; x++) {
                var cr = s.charAt(x);
                if (chsep.indexOf(cr) == -1)
                    Stringreturn += cr;
            }
            return Stringreturn;
        }
        function februarycheck(cyear) {
            return (((cyear % 4 == 0) && ((!(cyear % 100 == 0)) || (cyear % 400 == 0))) ? 29 : 28);
        }
        function finaldays(nr) {
            for (var x = 1; x <= nr; x++) {
                this[x] = 31
                if (x == 4 || x == 6 || x == 9 || x == 11) {
                    this[x] = 30
                }
                if (x == 2) {
                    this[x] = 29
                }
            }
            return this
        }

        function converterdate(string) {
            var d = string.split(/[ :\s]/);
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'sep', 'Oct', 'Nov', 'Dec'];
            var monthval = 1;

            for (var i = 0; i < months.length; i++) {
                if (months[i] == d[1]) {
                    monthval = parseInt(i + 1);
                    break;
                }
            }

            if (monthval < 10) {
                monthval = '0' + monthval;
            }

            return d[0] + '/' + monthval + '/' + d[2];
        }

        function dtvalid(strdate) {
            var monthdays = finaldays(12)
            var cpos1 = strdate.indexOf(chsep)
            var cpos2 = strdate.indexOf(chsep, cpos1 + 1)
            var daystr = strdate.substring(0, cpos1)
            var monthstr = strdate.substring(cpos1 + 1, cpos2)
            var yearstr = strdate.substring(cpos2 + 1)
            strYr = yearstr
            if (strdate.charAt(0) == "0" && strdate.length > 1) strdate = strdate.substring(1)
            if (monthstr.charAt(0) == "0" && monthstr.length > 1) monthstr = monthstr.substring(1)
            for (var i = 1; i <= 3; i++) {
                if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
            }
            // The parseInt is used to get a numeric value from a string   
            pmonth = parseInt(monthstr)
            pday = parseInt(daystr)
            pyear = parseInt(strYr)
            if (cpos1 == -1 || cpos2 == -1) {
                //alert("The date format must be : dd/mm/yyyy")
                return false
            }
            if (monthstr.length < 1 || pmonth < 1 || pmonth > 12) {
                //alert("Input a valid month")
                return false
            }
            if (daystr.length < 1 || pday < 1 || pday > 31 || (pmonth == 2 && pday > februarycheck(pyear)) || pday > monthdays[pmonth]) {
                //alert("Input a valid day")
                return false
            }
            if (yearstr.length != 4 || pyear == 0 || pyear < dminyear || pyear > dmaxyear) {
                //alert("Input a valid 4 digit year between " + dminyear + " and " + dmaxyear)
                return false
            }
            if (strdate.indexOf(chsep, cpos2 + 1) != -1 || checkinteger(getcharacters(strdate, chsep)) == false) {
                //alert("Input a valid date")
                return false
            }
            return true
        }

        function IsValidDate(str1, str2) {

            var Day = parseInt(str1.substring(0, 2), 10);
            var Mn = parseInt(str1.substring(3, 5), 10);
            var Yr = parseInt(str1.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);

            var Day1 = parseInt(str2.substring(0, 2), 10);
            var Mn1 = parseInt(str2.substring(3, 5), 10);
            var Yr1 = parseInt(str2.substring(6, 10), 10);
            var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;
            var dt1 = new Date(DateVal1);

            if (dt.getDate() != Day) {
                alert('Invalid Date');
                return false;
            }
            else if (dt.getMonth() != Mn - 1) {
                //this is for the purpose JavaScript starts the month from 0
                alert('Invalid Date');
                return false;
            }
            else if (dt.getFullYear() != Yr) {
                alert('Invalid Date');
                return false;
            }


            if (dt1.getDate() != Day1) {
                alert('Invalid Date');
                return false;
            }
            else if (dt1.getMonth() != Mn1 - 1) {
                //this is for the purpose JavaScript starts the month from 0
                alert('Invalid Date');
                return false;
            }
            else if (dt1.getFullYear() != Yr1) {
                alert('Invalid Date');
                return false;
            }
            // alert(dt); alert(dt1);
            if (dt >= dt1) {
                return false;
            }
            return true;
        }


        function LessThanToday(strDate) {
            var Day = parseInt(strDate.substring(0, 2), 10);
            var Mn = parseInt(strDate.substring(3, 5), 10);
            var Yr = parseInt(strDate.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);

            var todaydate = new Date();
            //alert(todaydate);alert(dt);
            var i = DateDiff.inDays(dt, todaydate)
            if (i <= 0) {
                return false;
            }
            return true;
        }

    </script>

    <script type="text/javascript">

        //function PlotAuto(dataConsolidated, ImageArray) {
        //    var max = dataConsolidated.MaxDate;
        //    var min = dataConsolidated.MinDate;
        //    var data = dataConsolidated.SimpleNavReturnModel;
        //    //  alert(data);
        //    var seriesNames = Array();
        //    var dataPlot = [[[]]];
        //    for (var i = 0; i < data.length; i += 1) {
        //        seriesNames.push(data[i].Name);
        //        var points = [];
        //        for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
        //            points.push([data[i].ValueAndDate[j].Date, data[i].ValueAndDate[j].Value]);
        //        }
        //        dataPlot.push(points);
        //    }
        //    dataPlot.shift();




        //    $('#divChart').remove();
        //    if (data.length < 1) {
        //        $('#chartContainer').append('<div style="width: 710px; height:500px; text-align: center; padding-top: 10%;" id="chart">Data not Available for the selected date range</div>');
        //        $('#chartContainer').effect("highlight", {}, 3000);
        //        return;
        //    }
        //    $('#chartContainer').append('<div style="width: 97%; height:500px;" id="divChart" ></div>');

        //    // var CustomSeriesColors = ["#7bf773", "#0031ce", "#ff9494", "#9900ff", "#00ad00", "#ff0000", "#ff9933", "#737373", "#9cc6ff", "#633100", "#0085cc"];
        //    var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

        //    var colorarray = Array();
        //    for (var i = 0; i < ImageArray.length; i += 1) {
        //        colorarray.push(CustomSeriesColors[ImageArray[i]]);
        //    }

        //    var plot2 = $.jqplot('divChart', dataPlot,
        //     {
        //         seriesColors: colorarray,
        //         animate: true,
        //         animateReplot: true,
        //         axes: {
        //             xaxis: {
        //                 min: min,
        //                 max: max,
        //                 renderer: $.jqplot.DateAxisRenderer,
        //                 rendererOptions: { tickRenderer: $.jqplot.CanvasAxisTickRenderer },
        //                 //tickInterval: '7 day',
        //                 tickOptions: {
        //                     formatString: '%b %#d, %y',
        //                     fontSize: '10pt'
        //                 }
        //                 //tickOptions: { formatString: '%#d/%#m/%Y' }
        //             },
        //             yaxis:
        //                 {
        //                     label: 'Value [Rebased]',
        //                     tickOptions: {
        //                         formatString: '%.2f',
        //                         fontSize: '10pt'
        //                     },
        //                     labelRenderer: $.jqplot.CanvasAxisLabelRenderer
        //                 }
        //         },
        //         seriesDefaults: { showMarker: false, lineWidth: 2, rendererOptions: { animation: { speed: 1000 } } },
        //         highlighter: { show: true, sizeAdjust: 7.5 },
        //         cursor: { show: true, zoom: true, showTooltip: false },
        //         legend: {
        //             renderer: $.jqplot.EnhancedLegendRenderer,
        //             show: true,
        //             location: 's',
        //             rendererOptions: {
        //                 numberRows: 4,
        //                 numberColumns: 2
        //             },
        //             placement: 'outsideGrid',
        //             seriesToggle: 'on',
        //             fontSize: '1em',
        //             border: '0px solid black'
        //         },
        //         grid: {
        //             shadow: false,
        //             borderWidth: 0,
        //             background: 'rgba(0,0,0,0)'
        //         }
        //     });
        //    for (var i = 0; i < seriesNames.length; i += 1) {
        //        plot2.series[i].label = seriesNames[i];
        //    }
        //    var legendTable = $($('.jqplot-table-legend')[0]);
        //    legendTable.css('height', '100px');


        //    plot2.replot();
        //    $('#infoChart').removeAttr("style");


        //    var max = dataConsolidated.MaxDate;
        //    var min = dataConsolidated.MinDate;
        //    var data = dataConsolidated.SimpleNavReturnModel;
        //    var tt = [[[]]];
        //    for (var i = 0; i < data.length; i += 1) {

        //        var tt1 = {};
        //        tt1.name = data[i].Name;

        //        var points = [];
        //        for (var j = 0; j < data[i].ValueAndDate.length; j += 1) {
        //            var res = data[i].ValueAndDate[j].Date.split("-");
        //            points.push([Date.UTC(res[0], res[1], res[2]), data[i].ValueAndDate[j].Value]);
        //        }
        //        tt1.data = points;
        //        tt.push(tt1);
        //    }
        //    //04 - 16 - 2016

        //    Highcharts.stockChart('container', {
        //        rangeSelector: {
        //            buttons: [{
        //                type: 'month',
        //                count: 1,
        //                text: '1m'
        //            }, {
        //                type: 'month',
        //                count: 3,
        //                text: '3m'
        //            }, {
        //                type: 'month',
        //                count: 6,
        //                text: '6m'
        //            }, {
        //                type: 'ytd',
        //                text: 'YTD'
        //            }, {
        //                type: 'year',
        //                count: 1,
        //                text: '1y'
        //            }, {
        //                type: 'year',
        //                count: 3,
        //                text: '3y'
        //            }, {
        //                type: 'year',
        //                count: 5,
        //                text: '5y'
        //            }, {
        //                type: 'year',
        //                count: 10,
        //                text: '10y'
        //            }, {
        //                type: 'year',
        //                count: 15,
        //                text: '15y'
        //            }, {
        //                type: 'all',
        //                text: 'All'
        //            }],
        //            selected: 6
        //        },

        //        yAxis: {
        //            labels: {
        //                formatter: function () {
        //                    return (this.value > 0 ? ' + ' : '') + this.value + '%';
        //                }
        //            },
        //            plotLines: [{
        //                value: 0,
        //                width: 2,
        //                color: 'silver'
        //            }]
        //        },

        //        plotOptions: {
        //            series: {
        //                compare: 'percent',
        //                showInNavigator: true
        //            }
        //        },

        //        tooltip: {
        //            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
        //            valueDecimals: 2,
        //            split: true

        //        },

        //        series: tt

        //    }
        //    , function (chart) {

        //        // apply the date pickers
        //        setTimeout(function () {
        //            $('input.highcharts-range-selector', $(chart.container).parent()).datepicker();
        //        }, 0);
        //    }
        //    );
        //}

    </script>

    <script type="text/javascript">

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
//                    var dateVal = {};
//                    dateVal.x = Date.UTC(res[0], res[1], res[2]);
//                    dateVal.y = data[i].ValueAndDate[j].Value;
//                    dateVal.OrginalValue = data[i].ValueAndDate[j].OrginalValue;
//                    points.push(dateVal);

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

                            s = s + '<span style="color:#363535">' + navdate + '</span><br /><br />';

                            for (var i = 0; i < tt.length; i++) {
                                var d = 123;
                                var f = this;

                                for (var k = 0; k < tt[i].data.length; k++) {
                                    if (this.x === tt[i].data[k][0]) {
                                        s = s + '<span style="color:' + colorarray[i] + '">' + tt[i].name + '</span>: <b>' + (tt[i].data[k][2].toString()==-1?"N/A":tt[i].data[k][2].toString()) + '</b><br />';
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

      
        //$.datepicker.setDefaults({
        //    dateFormat: 'yy-mm-dd',
        //    onSelect: function () {
        //        this.onchange();
        //        this.onblur();
        //    }
        //});
    </script>
</head>

<body>

    <form id="Form1" runat="server">
        <div style="width: 960px">
            <div class="navbar" align="center" style="width: 953px; margin-left: 7px; margin-top: -55px;">
                <div class="navbar-inner">
                    <div class="container-fluid">
                        <div class="brand">Mutual Fund Analyzer</div>
                        <div style="float: right; margin-right: 30px; margin-top: 8px;">
                            <img src="images/search.png" style="margin-top: 3px;" /><asp:TextBox ID="txtSearch" runat="server" Width="232px" placeholder="Type your text here"></asp:TextBox>
                            <asp:HiddenField ID="hfCustomerId" runat="server" />

                        </div>
                        <!--/.nav-collapse -->
                    </div>
                </div>
            </div>

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div class="container-fluid">
                <div class="row-fluid">
                    <div class="span3" id="sidebar">
                        <ul class="nav nav-list bs-docs-sidenav nav-collapse collapse" style="margin-left: -10px; padding-top: 0;">
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/top-performing-mutual-funds.aspx')">Top Funds</a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/compare-mutual-funds.aspx')">Compare Funds</a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/recommended-mutual-fund.aspx')">Recommended Funds </a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/recommended-new-fund-offer.aspx')">Recommended NFO </a></li>
                            <li class="active"><a href="#" onclick="Menuclick('http://www.askmefund.com/mutual-fund-nav-graph.aspx')">Funds NAV Graph </a></li>
                            <li><a href="#" onclick="Menuclick('http://www.askmefund.com/schemesearch.aspx')">Scheme Search </a></li>
                            <li><a href="http://www.askmefund.com/mfwatchlist.aspx" target="_blank">MF Watch List </a></li>
                        </ul>
                    </div>

                    <!--/span-->


                    <div class="span9" id="content">
                        <div class="row-fluid" style="margin-top: -5px; margin-left: 25px; width: 703px;">
                            <!-- block -->
                            <div class="block" style="margin-top: 5px;">
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
                                            OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged" CssClass="span6" Style="max-height: 50px;">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="controls">
                                        <p class="span5 lebel-drop">Category</p>
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="span6"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
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

                                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlType_SelectedIndexChanged" CssClass="span6">
                                        </asp:DropDownList>
                                        &nbsp;
                                    </div>
                                    <div class="controls" style="margin-bottom: 10px;">
                                        <div class="span5 lebel-drop">Option</div>
                                        <div class="style-radio">
                                            <asp:RadioButtonList ID="rdbOption" runat="server"
                                                CssClass="radio" RepeatDirection="Horizontal" AutoPostBack="true"
                                                Style="display: inline-block; margin-left: 15px; margin-top: 5px;"
                                                OnSelectedIndexChanged="rdbOption_SelectedIndexChanged">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>


                                    <div class="controls">
                                        <p class="span5 lebel-drop">Choose Scheme</p>
                                        <%-- <select class="span6">
                                              <option>HDFC Annual Interval Fund - Series I - Plan A</option>
                                              <option>HDFC Debt Fund for Cancer Cure 2014 - Reg - 100% Dividend Donation</option>
                                            </select>--%>
                                        <asp:DropDownList ID="ddlSchemes" runat="server" CssClass="span6">
                                            <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <%-- <button class="btn btn-danger btn-mini" style="margin-top:-11px; margin-left:10px;">
                                           <i class="icon-plus icon-white"></i> Add </button>--%>

                                        <asp:ImageButton ID="btnAddScheme" runat="server" CssClass="btn btn-danger btn-mini" Text="Add" OnClick="btnAddScheme_Click"
                                            OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" ImageUrl="images/images/add.png" Style="margin-top: -15px;"></asp:ImageButton>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme"
                                            ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="controls">
                                        <p class="span5 lebel-drop">
                                            Index
                                        </p>
                                        <%-- <select class="span6">
                                              <option>BSC 500</option>
                                              <option>Nifty</option>
                                            </select>
                                            <button class="btn btn-danger btn-mini" style="margin-top:-11px; margin-left:10px;">
                                            <i class="icon-plus icon-white"></i> Add</button>--%>
                                        <asp:DropDownList ID="ddlIndices" runat="server" CssClass="span6">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="btnAddIndices" runat="server" CssClass="btn btn-danger btn-mini" Text="Add"
                                            OnClick="btnAddIndices_Click" ImageUrl="images/images/add.png" ValidationGroup="indices" Style="margin-top: -15px;"
                                            OnClientClick="Javascript:return Listcount();" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="value_input" runat="server"
                                            ErrorMessage="Please Select Indices" ControlToValidate="ddlIndices" Display="Dynamic"
                                            InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <asp:Button ID="btnReset" class="btn-sub btn-large" style="margin-right: 15px;" runat="server" Text="Reset" OnClick="btnReset_Click" />
                        </div>

                    </div>
                </div>
                <div id="DivGridContain" class="row-fluid" runat="server" style="margin-top: 0px; margin-right: 25px; width: 959px;">
                    <!-- block -->
                    <asp:HiddenField ID="hidSchindSelected" runat="server" />
                    <div class="block" style="margin-top: 20px; margin-right: 25px; margin-left: -11px;">
                        <div class="block-content collapse in">
                            <%--<table cellpadding="0" cellspacing="0" border="0" class="table table-striped table-bordered" id="example2" style="margin-top:15px; font-size:12px;">
                                        <thead>
                                            <tr>
                                                <th>Scheme Name</th>
                                                <th>Key</th>
                                                <th><input name="" type="checkbox" value=""> All</th>
                                                <th>Delete</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr class="odd gradeX">
                                                <td>HDFC Annual Interval Fund - Series I - Plan A</td>
                                                <td><img src="images/clr_bx.jpg" width="12" height="12"></td>
                                                <td><input name="" type="checkbox" value=""></td>
                                              <td><a href="#"><img src="images/close.png"></a></td>
                                          </tr>
                                          <tr class="odd gradeX">
                                                <td>HDFC Annual Interval Fund - Series I - Plan A</td>
                                                <td><img src="images/clr_bx1.jpg" width="12" height="12"></td>
                                                <td><input name="" type="checkbox" value=""></td>
                                              <td><a href="#"><img src="images/close.png"></a></td>
                                          </tr>
                                        </tbody>
                                    </table>--%>

                            <asp:DataGrid class="table table-striped table-bordered" Style="margin-top: 15px; font-size: 12px;" ID="dglist" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-Font-Bold="true" Width="100%" BorderColor="#d9d6d6" BorderWidth="1"
                                OnItemCommand="dglist_ItemCommand">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="text">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                            <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                            <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                            <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                        <%-- <HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Left" Width="70%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Key" HeaderStyle-CssClass="text">
                                        <ItemTemplate>
                                            <asp:Image ID="imgKey" runat="server" ImageUrl='<%#DataBinder.Eval(Container.DataItem, "ImgID")%>' />
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="text">
                                        <HeaderTemplate>
                                            <input type="checkbox" name="SelectAllCheckBox" onclick="SelectAll(this)">
                                            &nbsp;ALL
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="hdSchID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>' />
                                            <asp:HiddenField ID="hdIndID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>' />
                                            <asp:HiddenField ID="hdImgID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Auto ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="text">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="~/BansalCapital/images/close.png" OnClientClick="javascript:return confirm('Are you sure to delete?');"
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
                            <table>
                                <tr>
                                    <td align="left" style="padding-left: 30px;">
                                        <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <!-- /block -->
                </div>
                <div id="DivPlotChart" runat="server" class="row-fluid" style="margin-top: 0px; margin-right: 25px; width: 959px;">
                    <!-- block -->
                    <div class="block" style="margin-top: 20px; margin-right: 25px; margin-left: -11px;display:none" >
                        <div class="block-content collapse in" id="divTimePeriod" runat="server" visible="false">

                            <div class="span3">
                                <div style="margin-top: 10px">
                                    <asp:RadioButton ID="rbTime" CssClass="controls" runat="server" GroupName="Time" Checked="true" /></div>
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
                                    <asp:RadioButton ID="rbDateRange" runat="server" GroupName="Time" /></div>

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
                    <input type="button" value="Plot Chart" class="btn-sub btn-large" id="btplotChart" style="margin-right: 33px;" onclick="Javascript: btnPlotclick();" />

                </div>
                <div id="DivLast" runat="server" class="row-fluid" style="margin-top: 0px; margin-right: 25px; width: 958px;display:none">
                    <div class="block" style="margin-top: 20px; margin-right: 25px; margin-left: -10px;">
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
                                            <span id="infoChart" >* Click on any Legend above to un-plot
                                        the corresponding series</span>
                                        </div>
                                        <div class="value_input" style="width: 100%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                                            Developed for Askmefund by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer"
                                                                                                            target="_blank">Disclaimer </a>
                                        </div>
                                    </td>
                                </tr>
                                <%--   <tr>
                            <td>
                                <input type="button" value="Export to Excel" style="" class="value_plot" id="btnExportExcel"
                                    onmouseover="saveImg()" />
                            </td>
                        </tr>--%>

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
            <footer style="font-size: 12px; margin-top: 80px; text-align: center">
                    
                          </footer>
            <!--  <hr>
            <footer>
                <p>&copy; Vincent Gabriel 2013</p>
            </footer>-->
        </div>
     
    </form>

</body>

</html>
