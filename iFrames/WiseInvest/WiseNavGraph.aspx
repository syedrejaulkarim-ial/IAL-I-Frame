<%@ Page Title="" Language="C#" MasterPageFile="~/WiseInvest/WiseMain.Master" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="WiseNavGraph.aspx.cs" Inherits="iFrames.WiseInvest.WiseNavGraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBoby" runat="server">
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.min.js"></script>
    <![endif]-->
    <!--[if IE]><script language="javascript" type="text/javascript" src="../Scripts/jqplot/excanvas.js"></script>
    <![endif]-->
    <script src="js/jquery-1.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/date.js"></script>
    <script src="../Scripts/jqplot/jquery.jqplot.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.cursor.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.dateAxisRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.highlighter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.canvasTextRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.canvasAxisLabelRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot/plugins/jqplot.dateAxisRenderer.min.js" type="text/javascript"></script>
    <script src="~/Scripts/jqplot/plugins/jqplot.barRenderer.min.js"></script>
    <script src="~/Scripts/jqplot/plugins/jqplot.categoryAxisRenderer.min.js"></script>
    <script src="~/Scripts/jqplot/plugins/jqplot.pointLabels.min.js"></script>
    <%--  <script src="../Scripts/jquery.jqplot.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot.barRenderer.js" type="text/javascript"></script>
    <script src="../Scripts/jqplot.categoryAxisRenderer.js" type="text/javascript"></script>--%>
    <link href="../Styles/jquery.jqplot.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">

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
                window.location.reload(true);
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

            //            $('input:checkbox[id$=chkItem]:checked', datagrid).each(function (item, index) {

            //                var Schid = $(this).next('input:hidden[id$=hdSchID]').val();
            //                var Indid = $(this).next().next('input:hidden[id$=hdIndID]').val();
            //                var ImgId = $(this).next().next().next('input:hidden[id$=hdImgID]').val();
            //                //alert(Schid); alert(Indid);
            //                if (Schid != '')
            //                    tempStr += ('s' + Schid + '#');
            //                if (Indid != '')
            //                    tempStr += ('i' + Indid + '#');
            //                ImageArray.push(ImgId);
            //            });


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




            //alert(tempStr);alert(schIndId);

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
                url: "WiseNavGraph.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                success: function (msg) {
                    // setChart(msg.d);
                    PlotAuto(msg.d, ImageArray);

                },
                error: function (msg) {
                    alert("Error! Try again...");
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

        //        function Plot2(dataConsolidated) {


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
        //            //$('#chart').remove();


        //            var plot2 = $.jqplot('#chart', dataPlot,
        //        {
        //            axes: {
        //                xaxis: {
        //                    //label: 'Date',
        //                    min: min,
        //                    max: max,
        //                    renderer: $.jqplot.DateAxisRenderer
        //                },
        //                yaxis: {

        //                }
        //            }
        //        });

        //        }

        function PlotAuto(dataConsolidated, ImageArray) {
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
            $('#divChart').remove();
            if (data.length < 1) {
                $('#chartContainer').append('<div style="width: 710px; height:500px; text-align: center; padding-top: 10%;" id="chart">Data not Available for the selected date range</div>');
                $('#chartContainer').effect("highlight", {}, 3000);
                return;
            }
            $('#chartContainer').append('<div style="width: 97%; height:500px;" id="divChart" ></div>');

            // var CustomSeriesColors = ["#7bf773", "#0031ce", "#ff9494", "#9900ff", "#00ad00", "#ff0000", "#ff9933", "#737373", "#9cc6ff", "#633100", "#0085cc"];
            var CustomSeriesColors = ["#4bb2c5", "#c5b47f", "#eaa228", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc"];

            var colorarray = Array();
            for (var i = 0; i < ImageArray.length; i += 1) {
                colorarray.push(CustomSeriesColors[ImageArray[i]]);
            }

            var plot2 = $.jqplot('divChart', dataPlot,
                {
                    seriesColors: colorarray,
                    animate: true,
                    animateReplot: true,
                    axes: {
                        xaxis: {
                            min: min,
                            max: max,
                            renderer: $.jqplot.DateAxisRenderer,
                            rendererOptions: { tickRenderer: $.jqplot.CanvasAxisTickRenderer },
                            //tickInterval: '7 day',
                            tickOptions: { formatString: '%b %#d, %y',
                                fontSize: '10pt'
                            }
                            //tickOptions: { formatString: '%#d/%#m/%Y' }
                        },
                        yaxis:
                            {
                                label: 'Value [Rebased]',
                                tickOptions: { formatString: '%.2f',
                                    fontSize: '10pt'
                                },
                                labelRenderer: $.jqplot.CanvasAxisLabelRenderer
                            }
                    },
                    seriesDefaults: { showMarker: false, lineWidth: 2, rendererOptions: { animation: { speed: 1000}} },
                    highlighter: { show: true, sizeAdjust: 7.5 },
                    cursor: { show: true, zoom: true, showTooltip: false },
                    legend: {
                        renderer: $.jqplot.EnhancedLegendRenderer,
                        show: true,
                        location: 's',
                        rendererOptions: {
                            numberRows: 4,
                            numberColumns: 2
                        },
                        placement: 'outsideGrid',
                        seriesToggle: 'on',
                        fontSize: '1em',
                        border: '0px solid black'
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
            var legendTable = $($('.jqplot-table-legend')[0]);
            legendTable.css('height', '100px');


            plot2.replot();
            $('#infoChart').removeAttr("style");
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
                    
    </script>
    <script type="text/javascript">
        document.getElementById('lNavGraph').setAttribute('class', 'selected');
    </script>
    <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="710" border="0" align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="top_icon">
                            <img src="img/nav-graph.png" width="29" height="29" />
                        </td>
                        <td class="top_title">
                            Nav Graph
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" class="top_line">
                            <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_inputa">
                                        Mutual Funds
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFundHouse" runat="server" Width="450px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="top_td">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_inputa">
                                        Scheme Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSchemes" runat="server" Width="450px">
                                            <asp:ListItem Value="0" Selected="True">-Select Scheme-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme"
                                            ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btnAddScheme" runat="server" CssClass="top_button3" Text="Add" OnClick="btnAddScheme_Click"
                                            OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" />
                                    </td>
                                </tr>
                                <tr class="top_td">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="top_inputa">
                                        Indices
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlIndices" runat="server" Width="450px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices"
                                            ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btnAddIndices" runat="server" CssClass="top_button3" Text="Add" OnClick="btnAddIndices_Click"
                                            ValidationGroup="indices" OnClientClick="Javascript:return Listcount();" />
                                    </td>
                                </tr>
                                <tr class="top_td">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
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
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                <asp:HiddenField ID="hidSchindSelected" runat="server" />
                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:DataGrid class="top_table" ID="dglist" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-Font-Bold="true" Width="100%" BorderColor="#4879AC" BorderWidth="1"
                                OnItemCommand="dglist_ItemCommand">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                            <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                            <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                            <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                        <%-- <HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                        <ItemStyle CssClass="top_tablerow" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Key" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <asp:Image ID="imgKey" runat="server" ImageUrl='<%#DataBinder.Eval(Container.DataItem, "ImgID")%>' />
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle CssClass="top_tablerow" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="top_tableheader">
                                        <HeaderTemplate>
                                            <input type="checkbox" name="SelectAllCheckBox" onclick="SelectAll(this)">ALL
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="hdSchID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>' />
                                            <asp:HiddenField ID="hdIndID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>' />
                                            <asp:HiddenField ID="hdImgID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle CssClass="top_tablerow" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Auto ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle CssClass="top_tablerow" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="top_tableheader">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="~/Images/Delete.jpeg" OnClientClick="javascript:return confirm('Are you sure to delete?');"
                                                CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                        <ItemStyle CssClass="top_tablerow" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td class="top_td">
                            <table>
                                <tr>
                                    <td align="left" style="padding-left: 30px;">
                                        <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divTimePeriod" runat="server" visible="false">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tr_width1">
                                            <label>
                                                <asp:RadioButton ID="rbTime" runat="server" GroupName="Time" Checked="true" />
                                            </label>
                                        </td>
                                        <td class="top_inpute">
                                            Time
                                        </td>
                                        <td class="tr_width3">
                                            <asp:DropDownList ID="ddlTime" runat="server" Width="100%" CssClass="ddlClass">
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
                                        </td>
                                        <td class="tr_width1">
                                            <label style="margin-left: 11px;">
                                                <asp:RadioButton ID="rbDateRange" runat="server" GroupName="Time" />
                                                &nbsp;</label>
                                        </td>
                                        <td class="top_input_navg">
                                            From Date
                                        </td>
                                        <td class="tr_width1">
                                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="top_txtbox"></asp:TextBox>
                                        </td>
                                        <td class="top_inputf">
                                            To Date
                                        </td>
                                        <td class="tr_widthnavg">
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="top_txtbox"> </asp:TextBox>
                                        </td>
                                        <td class="tr_width1" style="margin-left: 0px;" align="right">
                                            <input type="button" value=">> Plot Chart" class="top_button1" id="btplotChart" onclick="Javascript:btnPlotclick();" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%" border="0" align="right" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tr_width2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="top_td">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 710px;">
                    <%--height: 500px;--%>
                    <div style="width: 100%; float: left;" id="chartContainer">
                        <div style="width: 97%; height: 100%;" id="divChart">
                        </div>
                    </div>
                </div>
                <div style="width: 700px; text-align: left; font-family: Arial, Helvetica, sans-serif;
                    font-size: 12px; color: #0c4466;">
                    <span id="infoChart" style="visibility: hidden;">* Click on any Legend above to un-plot
                        the corresponding series</span>
                </div>
            </td>
        </tr>
        <%--<tr>
       <td>
       <table>
        <tr>
            <td class="disclaimerh">
                Disclaimer
            </td>
        </tr>
        <tr>
            <td class="disclaimer">
                All Mutual Funds and securities investments are subject to market risks and there
                can be no assurance that the scheme’s object will be achieved and the NAV of the
                schemes can go up or down depending upon the factors and forces affecting the securities
                market. Past performance of the schemes do not indicate the future performances.
                The NAV of the schemes may be affected by changes in Interest Rate, trading volumes,
                settlement periods, transfer procedures and performances of individual securities.
                The NAV will be exposed to price/ Interest rate Risk and Credit Risk.
            </td>
        </tr>
       </table>
       </td>
       </tr>--%>
    </table>
    </form>
</asp:Content>
