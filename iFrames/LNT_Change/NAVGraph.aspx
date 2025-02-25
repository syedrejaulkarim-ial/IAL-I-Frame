<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="NAVGraph.aspx.cs" Inherits="iFrames.LNT.Tools.NAVGraph" %>

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



    <script src="js/AutoComplete.js" type="text/javascript"></script>

    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/DT_bootstrap.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/styles.css" rel="stylesheet" media="screen" />
    
    <script src="js/modernizr-respond.min.js" type="text/javascript"></script>

    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;600;700;800&display=swap"rel="stylesheet" />


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
        $(function () {
            $("#BtnReset").click(function () {
                //location.reload(false);
                window.location = window.location;
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

        function btnPlotclick() {
            //debugger;
            var schIndId = $('#<%=HdSchemes.ClientID %>').val();
            var frmDate = $('#<%=HdFromData.ClientID %>').val();
            var toData = $('#<%=HdToData.ClientID %>').val();
            if ((schIndId == "") || (schIndId == undefined)) {

                return;
            }
             for (var x = 0; x < schIndId.split('#').length; x++) {
                if (schIndId.split('#')[x].indexOf('s') >= 0) {
                    if (schIndId.split('#')[x].length == 1) {
                        alert("Please select atleast one scheme to get graph");
                        //break;
                        return false;
                    }
                }
            }
            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "'}";
            $.ajax({
                type: "POST",
                url: "NAVGraph.aspx/getChartData",
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
                title: {
                    text:"Scheme NAV vs Index Performance"
                },
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


    <script type="text/javascript" language="javascript">

        function ValidateControl() {            

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

        function Listcount() {
            var listCount = CountItemList();
            if (listCount == 5) {
                alert("You can compare maximum 5 schemes/indeces at a time");
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


<%--    <style type="text/css">
        .btn.btn-danger.btn-mini {
            background: #ffcb05;
            color: #fff;
            padding: 1px 4px;
            border: 0;
            border-radius: 5px;
            color:#000;
        }

        .btn-sub {
            background-color:#00adee !important;
        }

        .text,.table th {
            color: #151515;
        }
    </style>--%>
    <style type="text/css">
        .btn.btn-danger.btn-mini {
            background: #db0011;
            color: #fff;
            padding: 1px 4px;
            border: 0;
            border-radius: 1px;
            color: #000;
        }

        .btn-sub {
            background-color: #a2a3a3 !important
        }

        .text, .table th {
            color: #151515;
        }

        /* TAB */


        /* Tabs panel */
        .tabbable-panel {
            border: 1px solid #ccc;
            padding: 10px;
        }

            .tabbable-panel .tabbable-line {
                background: #fff;
                padding: 0px 0px 12px 0px;
            }
        /* Default mode */
        .tabbable-line > .nav-tabs {
            border: none;
            margin: 0px;
        }

            .tabbable-line > .nav-tabs > li {
                margin-right: 2px;
            }

                .tabbable-line > .nav-tabs > li > a {
                    border: 0;
                    margin-right: 0;
                    color: #737373;
                }

                    .tabbable-line > .nav-tabs > li > a > i {
                        color: #a6a6a6;
                    }

                .tabbable-line > .nav-tabs > li.open, .tabbable-line > .nav-tabs > li:hover {
                    border-bottom: 4px solid #000;
                }

                    .tabbable-line > .nav-tabs > li.open > a, .tabbable-line > .nav-tabs > li:hover > a {
                        border: 0;
                        background: none !important;
                        color: #333333;
                    }

                        .tabbable-line > .nav-tabs > li.open > a > i, .tabbable-line > .nav-tabs > li:hover > a > i {
                            color: #a6a6a6;
                        }

                    .tabbable-line > .nav-tabs > li.open .dropdown-menu, .tabbable-line > .nav-tabs > li:hover .dropdown-menu {
                        margin-top: 0px;
                    }

                .tabbable-line > .nav-tabs > li.active {
                    border-bottom: 4px solid #db0011;
                    position: relative;
                }

                    .tabbable-line > .nav-tabs > li.active > a {
                        border: 0;
                        color: #333333;
                        background: transparent !important;
                    }

                        .tabbable-line > .nav-tabs > li.active > a > i {
                            color: #404040;
                        }

        .tabbable-line > .tab-content {
            margin-top: -3px;
            background-color: #fff;
            border: 0;
            border-top: 1px solid #eee;
            padding: 15px 0;
        }

        .panel-title {
            font-weight: 600;
            padding: 0 20px 0 20px;
            font-size: 1.2em;
            line-height: 50px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .scheme_nme{
            border-left: #000 solid 4px;
            background: #db0011;
            color: #fff;
            border-top:none!important;
            border-bottom:none!important;
            font-weight:800;
        }
        .dis{
            font-size:12px;
            text-align:justify;
        }
        .block{
            margin-left:0px;
            /*border:none;*/
        }
    </style>
</head>

<body>
    <div style="width: 81%; margin: 0 auto">
        <form id="Form1" runat="server">

           <%-- <div class="navbar" align="center" style="width: 953px; margin-left: 7px; margin-top: -55px; display: none">
                <div class="navbar-inner">
                    <div class="container-fluid">
                        <div class="brand">Mutual Fund Analyzer</div>
                        <div style="float: right; margin-right: 30px; margin-top: 8px;">
                            <img src="images/search.png" style="margin-top: 3px;" /><asp:TextBox ID="txtSearch" runat="server" Width="232px" placeholder="Type your text here"></asp:TextBox>
                            <asp:HiddenField ID="hfCustomerId" runat="server" />

                        </div>
                    </div>
                </div>
            </div>--%>

              <div class="navbar">
                <div class="navbar-inner">
                    <div class="container">
                        <div class="row">
                            <div class="span6">
                                <img src="img/HSBC_Mutual_Fund_BP_RGB_NEG.svg" style="padding-top: 5px; padding-bottom: 5px" />
                            </div>
                            <div class="span4 pull-right" style="text-align: right; padding-top: 35px;">
                                <h4>Performance Calculator</h4>
                            </div>
                        </div>
                        
                    </div>
                </div>
                  <div class="mr-header-border">
                      <div class="mr-header-edge"></div>
                  </div>
            </div>
            <div class="container-fluid" style="padding: 0;">
                <div class="row-fluid">
                    <!--/span-->
                    <div class="span12" id="content">
                        <div class="row-fluid" style="margin-top: 0px;">
                            <div class="span12">
                                <!-- block -->
                                <div class="block">
                                    <div>
                                        <div class="muted pull-left" style="color: #cc0000; font-weight: 700;"></div>
                                        <div class="pull-right">
                                        </div>
                                    </div>
                                    <div class="block-content collapse in">                                       
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
                                                OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" ImageUrl="images/images/add.png" Style="margin-top: -11px;" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                                ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>

                                        </div>
                                        <div class="controls">
                                            <p class="span5 lebel-drop">Index</p>
                                            <asp:DropDownList ID="ddlIndices" runat="server" CssClass="span6">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnAddIndices" runat="server" AlternateText="Add" ImageUrl="images/images/add.png" OnClick="btnAddIndices_Click" class="btn btn-danger btn-mini"
                                                OnClientClick="Javascript:return Listcount();" ValidationGroup="indices" Style="margin-top: -11px;" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                                ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                    <button id="BtnReset" class="btn-sub btn-large" style="margin-right: 15px;">Reset</button>
                                    <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                                    <!-- /block -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--<asp:HiddenField ID="HdDivFundShow" runat="server" value="0" />--%>
                <div id="DivFundShow" class="row-fluid" runat="server">
                    <!-- block -->
                    <div class="block" style="margin-top: 20px;">

                        <div class="block-content collapse in">

                            <asp:DataGrid class="table table-striped table-bordered" Style="margin-top: 15px; font-size: 12px;" ID="dglist" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-Font-Bold="true" Width="100%" OnItemCommand="dglist_ItemCommand">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="text">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                            <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                            <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                            <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                            <asp:Label ID="IsVisible" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "IsVisible")%>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                        <ItemStyle HorizontalAlign="Left" Width="70%"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="">
                                        <HeaderTemplate>
                                            <input type="checkbox" name="SelectAllCheckBox" class="text" onclick="SelectAll(this)">&nbsp;ALL
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox>
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
                                    <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="text">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDel" runat="server" Visible='<%#DataBinder.Eval(Container.DataItem, "IsVisible")%>'  ImageUrl="images/close.png"
                                                OnClientClick="javascript:return confirm('Are you sure to delete?');"
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
                       

                        <asp:Button ID="btnCompareFund" runat="server" Text="Plot Chart" class="btn-sub btn-large" Style="margin-right: 10px;"
                            OnClientClick="return validateList();" OnClick="btnCompareFund_Click" Visible="false" />
                    </div>
                    <!-- /block -->
                </div>
                <!-- New-->
                  <div class="row-fluid" id="DivShowPerformance" runat="server">
                    <div class="panel-body">
                        <div class="tabbable-panel" style="padding: 0;">
                            <div class="tabbable-line">
                                <ul class="nav nav-tabs ">
                                    <li class="active">
                                        <a href="#tab_default_1" data-toggle="tab" style="padding: 0">
                                            <h3 class="panel-title" style="font-family: Raleway, sans-serif; font-size: 1.2em; font-weight: 600; font-style: normal">Performance using Graph</h3>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#tab_default_2" data-toggle="tab" style="padding: 0">
                                            <h3 class="panel-title">Performance in Standard Format</h3>
                                        </a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane active" id="tab_default_1" align="left">

                                        <div  class="row-fluid" style="margin-top: 0px; margin-right: 5px;" runat="server">
                                            <!-- block -->
                                            <div class="span12">
                                                <div class="" style="margin-top: 20px; margin-right: 0px;">

                                                    <div class="block-content collapse in">
                                                        <asp:Label ID="lblSortPeriod" runat="server" Visible="false" CssClass="gap-left">Click on 'Time Period' to rank funds on a particular period of your choice.
                                                        </asp:Label>
                                                    </div>
                                                    <div class="block-content collapse in">

                                                        <asp:GridView ID="GrdCompFund" runat="server" class="table table-striped table-bordered" Style="font-size: 12px;" Width="100%" AutoGenerateColumns="false"
                                                            OnRowCommand="GrdCompFund_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="30%">
                                                                    <ItemTemplate>

                                                                        <%# SetHyperlinkFundDetail(Eval("Sch_id").ToString(), Eval("Sch_Short_Name").ToString())%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:LinkButton ID="Lnk1mth" runat="server" Text="1 mth" CssClass="text" Font-Overline="false" CommandName="Per_30_Days"
                                                                            Font-Bold="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl1Mth" runat="server" Text='<%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days") : "NA"%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:LinkButton ID="Lnk3mth" runat="server" Font-Bold="true"
                                                                            Text="3 mths" Font-Overline="false" CommandName="Per_91_Days" CssClass="text"></asp:LinkButton>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl3Mth" runat="server">
                                <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:LinkButton ID="Lnk6mth" runat="server" Font-Bold="true" Text="6 mths"
                                                                            Font-Overline="false" CommandName="Per_182_Days" CssClass="text"></asp:LinkButton>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl6Mth" runat="server">
                                <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:LinkButton ID="Lnk1yr" runat="server" Font-Bold="true" Text="1Yr"
                                                                            Font-Overline="false" CommandName="Per_1_Year" CssClass="text"></asp:LinkButton>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl1yr" runat="server">
                                <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:LinkButton ID="Lnk3yr" runat="server" Font-Bold="true" Text="3 Yrs"
                                                                            Font-Overline="false" CommandName="Per_3_Year" CssClass="text"></asp:LinkButton>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl3yr" runat="server">
                                <%#Eval("Per_3_Year").ToString() != "" ? Eval("Per_3_Year") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:LinkButton ID="LnkSI" runat="server" Font-Bold="true" Text="Since Inception"
                                                                            Font-Overline="false" CommandName="Per_Since_Inception" CssClass="text"></asp:LinkButton>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSI" runat="server">
                                <%#Eval("Per_Since_Inception").ToString() != "" ? Eval("Per_Since_Inception") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="NAV"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNav" runat="server">
                                <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="Label2" runat="server" SkinID="lblHeader" Text="Category"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCat" runat="server">
                                <%#Eval("Nature").ToString() != "" ? Eval("Nature") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Structure"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStruct" runat="server">
                                <%#Eval("Structure_Name").ToString() != "" ? Eval("Structure_Name") : "NA"%> </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
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


                                                </div>

                                                <div style="margin-top: 40px; margin-right: 0px; margin-left: 0px;">
                                                    <input id="HdSchemes" type="hidden" runat="server" />
                                                    <input id="HdToData" type="hidden" runat="server" />
                                                    <input id="HdFromData" type="hidden" runat="server" />

                                                    <div style="width: 100%; float: left;" id="chartContainer" class="block">
                                                        <div style="width: 98%; height: 100%;" id="divChart">
                                                        </div>
                                                    </div>

                                                    <%--new chart--%>
                                                    <div id="HighContainer" style="height: 600px; min-width: 310px; max-width: 1000px"></div>
                                                </div>
                                                <!-- /block -->
                                            </div>

                                        </div>
                                    </div>
                                    <div class="tab-pane" id="tab_default_2" runat="server">
                                        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                                            <HeaderTemplate>
                                                <table class="table">
                                                    <%--<tr>
                <td colspan="2" align="center" class="title">Student's Report
                </td>
            </tr>--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr class="even">
                                                    <td colspan="3" class="scheme_nme">
                                                        <asp:Label ID="txtSCH_SHORT_NAME" Enabled="false" Text='<%#Eval("SCH_SHORT_NAME") %>'
                                                            runat="server"
                                                            CssClass="txtSmallEntry"></asp:Label>
                                                        <asp:HiddenField ID="txtSCHEME_ID" Value='<%#Eval("SCHEME_ID") %>'
                                                            runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="txtINDEX_IDS" Value='<%#Eval("INDEX_IDS") %>'
                                                            runat="server"></asp:HiddenField>
                                                    </td>
                                                </tr>
                                                <tr class="even">
                                                    <td colspan="2">
                                                        <asp:Repeater ID="innerRepeater" runat="server">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th rowspan="2">Scheme / Benchmark</th>

                                                                            <th colspan="2" align="center" style="text-align: center;"
                                                                                class="non-over-night">1Yr</th>
                                                                            <th colspan="2" align="center" style="text-align: center"
                                                                                class="non-over-night">3 Yrs</th>
                                                                            <th colspan="2" align="center" style="text-align: center"
                                                                                class="non-over-night">5 Yrs</th>
                                                                            <th colspan="2" align="center" style="text-align: center">
                                                                                Since Inception</th>
                                                                            <%-- <th rowspan="2" align="center" style="text-align: center">NAV</th>
                                    <th rowspan="2" align="center" style="text-align: center">Category</th>
                                    <th rowspan="2" align="center" style="text-align: center">Structure</th>--%>
                                                                        </tr>
                                                                        <tr>
                                                                            <th colspan="1" style="text-align: right;" class="over-night">
                                                                                Return (%)
                                                                            </th>
                                                                            <th colspan="1" style="text-align: right" class="over-night">
                                                                                Amount<br>
                                                                                (₹)
                                                                            </th>
                                                                            <th colspan="1" style="text-align: right;" class="over-night">
                                                                                Return (%)
                                                                            </th>
                                                                            <th colspan="1" style="text-align: right" class="over-night">
                                                                                Amount<br>
                                                                                (₹)
                                                                            </th>
                                                                            <th colspan="1" style="text-align: right;" class="over-night">
                                                                                Return (%)
                                                                            </th>
                                                                            <th colspan="1" style="text-align: right" class="over-night">
                                                                                Amount<br>
                                                                                (₹)
                                                                            </th>
                                                                            <th colspan="1" style="text-align: right;" class="non-over-night">
                                                                                Return (%)
                                                                            </th>
                                                                            <th colspan="1" style="text-align: right" class="non-over-night">
                                                                                Amount<br>
                                                                                (₹)
                                                                            </th>

                                                                        </tr>
                                                                    </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr style="border-width: 1px; border-style: solid;
                                                                    height: 30px;">
                                                                    <td>
                                                                        <%# SetHyperlinkFundDetail(Eval("Sch_Short_Name").ToString(), Eval("Sch_Short_Name").ToString())%>
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="lbl1Mth" runat="server" Text='<%#Eval("Per_1_Year").ToString() == "--" ?"--": (Math.Round(Convert.ToDecimal(Eval("Per_1_Year")),2)).ToString()%>' />
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("1YAmount").ToString() == "--" ?"--": (Math.Round(Convert.ToDecimal(Eval("1YAmount")),2)).ToString() %>' />
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="lbl3Mth" runat="server">
<%#Eval("Per_3_Year").ToString() == "--" ?"--": (Math.Round(Convert.ToDecimal(Eval("Per_3_Year")),2)).ToString() %> </asp:Label>
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="Label2" runat="server">
<%#Eval("3YAmount").ToString() == "--" ? "--" : (Math.Round(Convert.ToDecimal(Eval("3YAmount")),2)).ToString() %> </asp:Label>
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="lbl6Mth" runat="server">
<%#Eval("Per_5_Year").ToString() == "--" ?"--": (Math.Round(Convert.ToDecimal(Eval("Per_5_Year")),2)).ToString() %> </asp:Label>
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="Label3" runat="server">
<%#Eval("5YAmount").ToString() == "--" ?"--" : (Math.Round(Convert.ToDecimal(Eval("5YAmount")),2)).ToString() %> </asp:Label>
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="lbl1yr" runat="server">
<%#Eval("Per_Since_Inception").ToString() == "--" ? "--" : (Math.Round(Convert.ToDecimal(Eval("Per_Since_Inception")),2)).ToString()%> </asp:Label>
                                                                    </td>
                                                                    <td align="right" class="over-night">
                                                                        <asp:Label ID="Label4" runat="server">
<%#Eval("SIAmount").ToString() == "--" ? "--" : (Math.Round(Convert.ToDecimal(Eval("SIAmount")),2)).ToString() %> </asp:Label>
                                                                    </td>
                                                                    <%-- <td align="right" class="over-night">
                                <asp:Label ID="lbl3yr" runat="server">
<%#Eval("Nav_Rs").ToString() == "--" ? "--" : Eval("Nav_Rs") %> </asp:Label>
                            </td>

                            <td align="right" class="over-night">
                                <asp:Label ID="lblCat" runat="server">
<%#Eval("Nature").ToString() != "" ? Eval("Nature") : "NA"%> </asp:Label>
                            </td>
                            <td align="right" class="over-night">
                                <asp:Label ID="lblStruct" runat="server">
<%#Eval("Structure_Name").ToString() != "" ? Eval("Structure_Name") : "NA"%> </asp:Label>
                            </td>--%>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr>
                                                                    <%--                            <td align="right">Total:
<asp:Label ID="lblTotal" CssClass="totalClass" runat="server"></asp:Label>
                            </td>
                            <td class="right">
                                <asp:Label ID="lblResult" runat="server"></asp:Label>
                            </td>--%>
                                                                </tr>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <tr>
                                                </tr>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        
                                    </div>
                                </div>
                               
                            </div>
                        </div>
                        <div class="col-md-12 dis" id="divClientDisclaimer"
                            runat="server"></div>
                    </div>
                </div>

                    <!-- end--->


                


                <footer style="font-size: 12px; margin-top: 10px; text-align: center">
                    <div class="value_input" style="text-align: right; font-size: 10px; color: #A7A7A7">
                                Developed for HSBC Mutual Fund by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd,</a>
                        <a href="https://icraanalytics.com/home/Disclaimer" target="_blank" style="font-size: 10px; color: #999999">Disclaimer</a>
                            </div>
                </footer>
            </div>
        </form>
    </div>
</body>

</html>
