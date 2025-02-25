<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Comparefund.aspx.cs" Inherits="iFrames.PrismAdvisory.Comparefund" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta charset="utf-8"/>
  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
  <meta name="viewport" content="width=device-width, initial-scale=1"/>
  <meta name="description" content=""/>
  <meta name="author" content=""/>
  <link rel="shortcut icon" href="assets/ico/favicon.ico"/>

  <title>Prism Advisory</title>

  
  <link rel="stylesheet" href="font-awesome-4.7.0/css/font-awesome.min.css"/>
 <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' /> 
  <link rel="stylesheet" href="css/style.css"/>
    <link rel='stylesheet'  href='http://fonts.googleapis.com/css?family=Open+Sans%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic%7CRoboto%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic&#038;subset=latin%2Clatin-ext&#038;ver=1.0.0' type='text/css' media='all' />

     <link href="css/jquery-ui.css" rel="stylesheet" />

     <script src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script src="js/bootstrap.min.js"></script>
     <script type="text/javascript" src="js/date.js"></script>

    <!-- JS Plugins -->
    <script src="js/perfect-scrollbar.jquery.min.js"></script>
    <script src="js/bootstrap-slider.min.js"></script>

    <!-- JS Custom -->
    <script src="js/theme.min.js"></script>
    <script src="js/kite.min.js"></script>
     
    <script src="js/modernizr-2-respond.min.js" type="text/javascript"></script>

    
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
            //$('img[name="imgAdd2Watch"]').click(function () {
            //    var dataToPush = JSON.stringify({
            //        schemeId: $(this).attr('id'),
            //        user: $('#Userid').val()
            //    });
            //    $.ajax({
            //        cache: false,
            //        dataType: "json",
            //        data: dataToPush,
            //        url: 'BansalProxy.aspx/Add2Watchlist',
            //        type: 'POST',
            //        contentType: "application/json; charset=utf-8",
            //        success: function (dataConsolidated) {
            //            var obj = jQuery.parseJSON(dataConsolidated.d);
            //            if (obj.d == 1) {
            //                alert("Scheme added to your watch list");
            //            }
            //            else if (obj.d == 2) {
            //                alert("You cannot add more than 10 scheme in watch list");
            //            }
            //            else if (obj.d == 3) {
            //                alert("Scheme already exist in your watchlist");
            //            }
            //            else if (obj.d == 4) {                            
            //                var data = { 'url': 'http://www.askmefund.com/login.aspx' };
            //                top.postMessage(data, 'http://www.askmefund.com/compare-mutual-funds.aspx');
            //            }
            //            else if (obj.d == 0) {
            //                alert("Some Error occured. Please contact askmefund.com");
            //            }
            //        },
            //        error: function (data) {
            //            // debugger;
            //            //alert(data);
            //        }
            //    });

            //});
            $('#rdbOption').find('td:first').after('<td><label></label></td>');
            btnPlotclick();

        });

        function btnPlotclick() {          
            var schIndId = $('#<%=HdSchemes.ClientID %>').val();
            var frmDate = $('#<%=HdFromData.ClientID %>').val();
            var toData = $('#<%=HdToData.ClientID %>').val();
            if ((schIndId == "") || (schIndId == undefined)) {

                return;
            }
            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = "{'schIndexid':'" + schIndId + "', startDate:'" + frmDate + "', endDate:'" + toData + "'}";
            $.ajax({
                type: "POST",
                url: "Comparefund.aspx/getChartData",
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


   

</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">

    <!-- SIDEBAR
      ================================================== -->


    <!-- MAIN CONTENT
      ================================================== -->
    <div class="container-fluid">
      <div class="row">
        <div class="col-xs-12">
          <h3></h3>
        </div>
      </div>
      <!-- / .row -->
      <div class="row">
        <div class="col-xs-12 col-lg-10" style="margin:0 auto;">

          <!-- Basic form -->
          <div class="panel panel-default">
            <div class="panel-heading"> </div>
            <div class="panel-body">
              <div class="form-horizontal">
                  <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label-select">Mutual Fund</label>
                  <div class="col-sm-5">                   
                      <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" class="form-control"
                                            OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged">
                                        </asp:DropDownList>
                  </div>
                  <div class="col-sm-5"></div>
                </div>
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label-select">Category</label>
                  <div class="col-sm-5">
                   <asp:DropDownList ID="ddlCategory" runat="server" class="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                  </div>
                  <div class="col-sm-5"></div>
                </div>
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label-select">Sub-Category</label>
                  <div class="col-sm-5">
                  <asp:DropDownList ID="ddlSubCategory" runat="server" class="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                  </div>
                  <div class="col-sm-5"></div>
                </div>
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label-select">Type</label>
                  <div class="col-sm-5">
                  <asp:DropDownList ID="ddlType" runat="server" class="form-control" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                        </asp:DropDownList>
                  </div>
                  <div class="col-sm-5"></div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label-select">Option</label>
                  <div class="col-sm-10">
                      <div class="style-radio2">
                          <asp:RadioButtonList ID="rdbOption" runat="server"
                                                class="radio" RepeatDirection="Horizontal" AutoPostBack="true"
                                                Style="display: inline-block; margin-left: 15px; margin-top: 5px;"
                                                OnSelectedIndexChanged="rdbOption_SelectedIndexChanged">
                                            </asp:RadioButtonList>
                        </div>
                    <%--<div class="col-sm-2" style="padding-left:0;">
                      <div class="radio">
                        <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1" checked>
                        <label for="optionsRadios1">Growth</label>
                      </div>
                    </div>
                    <div class="col-sm-8">
                      <div class="radio">
                        <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2">
                        <label for="optionsRadios2">Dividend</label>
                      </div>
                    </div>--%>
                  </div>
                </div>

                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label-select">Choose Scheme</label>
                  <div class="col-sm-5">
                <asp:DropDownList ID="ddlSchemes" class="form-control"  runat="server" >
                                            <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        </asp:DropDownList>
                  </div>
                  <div class="col-sm-5">
                      
                       <asp:Button ID="btnAddScheme" class="btn btn-success btn-sm" style="margin-top:1px;" runat="server" Text="Add" OnClick="btnAddScheme_Click" OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" />                      
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                        ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                  </div>
                </div>
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-2 control-label-select">Index</label>
                  <div class="col-sm-5">
                     <asp:DropDownList ID="ddlIndices" runat="server" class="form-control">
                                        </asp:DropDownList>
                  </div>
                <div class="col-sm-5">
                    
                     <asp:Button ID="btnAddIndices" class="btn btn-success btn-sm" style="margin-top:1px;" runat="server" Text="Add" OnClick="btnAddIndices_Click" OnClientClick="Javascript:return Listcount();" ValidationGroup="indices" />                                          
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                            ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator>
                  </div>
                </div>
                
                <div class="form-group">
                  <div class="col-sm-2"></div>
                  <div class="col-sm-4">
                    
                       <asp:Button ID="btnReset" class="btn btn-primary" runat="server" Text="Reset" OnClick="btnResetClick" />
                        <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                    
                  </div>
                </div>
              </div>
            </div>
          </div>
          <!-- Bordered table -->
          <div class="col-xs-12 col-lg-2"></div>
        </div>
        <div class="col-xs-12 col-lg-2"></div>
        <!-- / .row -->
      </div>
      
      <div class="row">
        <div class="col-xs-12 col-lg-12">
          <div class="panel panel-default">
            <div class="panel-body"  id="DivFundShow" runat="server">
              <div class="table-responsive">
               <%-- <table class="table table-bordered">
                  <thead>
                    <tr class="table-header">
                      <th style="text-align:left">Name</th>
                      <th style="text-align:center">
                            <div style="text-align:center">
                                <div class="checkbox" align="center">
                                    <input type="checkbox" id="checkbox_1">
                                    <label for="checkbox_1">All</label>
                                </div>
                            </div>
                      </th>
                      <th style="text-align:center">Delete</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td>Axis Long Term Equity Fund - Growth</td>
                      <td style="text-align:center">
                          <div>
                                <div class="checkbox">
                                    <input type="checkbox" id="checkbox_2">
                                    <label for="checkbox_2"></label>
                                </div>
                          </div>
                      </td>
                      <td style="text-align:center"><i class="fa fa-times" aria-hidden="true" style="color:crimson; font-size:16px;"></i></td>
                    </tr>
                     <tr>
                      <td>Nifty 50</td>
                      <td style="text-align:right">
                          <div>
                                <div class="checkbox">
                                    <input type="checkbox" id="checkbox_3">
                                    <label for="checkbox_2"></label>
                                </div>
                          </div>
                      </td>
                      <td style="text-align:center">
                          <i class="fa fa-times" aria-hidden="true" style="color:crimson; font-size:16px;"></i>
                      </td>
                    </tr>
                  </tbody>
                </table>--%>
                   <asp:DataGrid class="table table-bordered"  ID="dglist" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-Font-Bold="true" Width="100%" OnItemCommand="dglist_ItemCommand">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="table-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                            <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                            <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                            <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                        </ItemTemplate>
                                     
                                        <ItemStyle HorizontalAlign="Left" Width="70%" BackColor="#ffffff"></ItemStyle>
                                    </asp:TemplateColumn>
                                     <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="table-header">
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
                                        <ItemStyle HorizontalAlign="left" Width="10%"  BackColor="#ffffff"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Auto ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                        </ItemTemplate>
                                      
                                        <ItemStyle HorizontalAlign="Left" Width="5%" BackColor="#ffffff"></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="table-header">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="img/close.png" OnClientClick="javascript:return confirm('Are you sure to delete?');"
                                                CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                        </ItemTemplate>
                                        
                                        <ItemStyle HorizontalAlign="Center" Width="5%" BackColor="#ffffff"></ItemStyle>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </asp:DataGrid>
                  <div>
                       <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                      </div>
                <p style="text-align:right">                    
                     <asp:Button ID="btnCompareFund" runat="server" Text="Show Performance" class="btn btn-primary btn-sm" 
                            OnClientClick="return validateList();" OnClick="btnCompareFund_Click" Visible="false" />
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="row">
        <div class="col-xs-12 col-lg-12">
          <div class="panel panel-default">
            <div class="panel-body" id="DivShowPerformance" runat="server">
              <div class="table-responsive">
               <%-- <table class="table table-bordered">
                  <thead>
                    <tr class="table-header">
                      <th style="text-align:left">Scheme Name</th>
                      <th style="text-align:right">1 mth</th>
                      <th style="text-align:right">3 mths</th>
                      <th style="text-align:right">6 mths</th>
                      <th style="text-align:right">1Yr</th>
                      <th style="text-align:right">3 Yrs</th>
                      <th style="text-align:right">Since Inception</th>
                      <th style="text-align:right">NAV</th>
                      <th style="text-align:left">Category</th>
                      <th style="text-align:left">Structure</th>
                      <th style="text-align:center">Invest Now</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td>Axis Long Term Equity Fund - Growth</td>
                      <td style="text-align:right">-0.48</td>
                      <td style="text-align:right">5.69</td>
                      <td style="text-align:right">11.98</td>
                      <td style="text-align:right">16.72</td>
                      <td style="text-align:right">15.65</td>
                      <td style="text-align:right">19.21</td>
                      <td style="text-align:right">39.29</td>
                      <td style="text-align:left">Equity</td>
                      <td style="text-align:left">Open Ended</td>
                      <td style="text-align:center"><a href="#"> <img src="img/rupee.png"></a></td>
                    </tr>
                     <tr>
                      <td>Nifty 50</td>
                      <td style="text-align:right">-0.48</td>
                      <td style="text-align:right">5.69</td>
                      <td style="text-align:right">11.98</td>
                      <td style="text-align:right">16.72</td>
                      <td style="text-align:right">15.65</td>
                      <td style="text-align:right">19.21</td>
                      <td style="text-align:right">NA</td>
                      <td style="text-align:left">NA</td>
                      <td style="text-align:left">NA</td>
                      <td style="text-align:center"><a href="#"> <img src="img/rupee.png"></a></td>
                    </tr>
                  </tbody>
                </table>--%>
                   <asp:GridView ID="GrdCompFund" runat="server" class="table table-bordered "  AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" Width="100%"
                                OnRowCommand="GrdCompFund_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="table-header">
                                        <ItemTemplate>                                          
                                            <%# SetHyperlinkFundDetail(Eval("Sch_id").ToString(), Eval("Sch_Short_Name").ToString())%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="30%"  BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderStyle-CssClass="table-header" >
                                        <HeaderTemplate>
                                           <%-- <asp:LinkButton ID="Lnk1mth" runat="server" Text="1 mth" CssClass="table-header" Font-Overline="false" CommandName="Per_30_Days"
                                                Font-Bold="true" />--%>
                                            <asp:Label ID="Lnk1mth" runat="server" SkinID="lblHeader" Text="1 mth" CssClass="table-header" ></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1Mth" runat="server" Text='<%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days") : "NA"%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right"  BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right"  HeaderStyle-CssClass="table-header">
                                        <HeaderTemplate>
                                            <%--<asp:LinkButton ID="Lnk3mth" runat="server" Font-Bold="true"
                                                Text="3 mths" Font-Overline="false" CommandName="Per_91_Days" CssClass="table-header"></asp:LinkButton>--%>
                                             <asp:Label ID="Lnk3mth" runat="server" SkinID="lblHeader" Text="3 mths" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3Mth" runat="server">
                                <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right"  BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right"  HeaderStyle-CssClass="table-header">
                                        <HeaderTemplate>
                                           <%-- <asp:LinkButton ID="Lnk6mth" runat="server" Font-Bold="true" Text="6 mths"
                                                Font-Overline="false" CommandName="Per_182_Days" CssClass="table-header"></asp:LinkButton>--%>
                                            <asp:Label ID="Lnk6mth" runat="server" SkinID="lblHeader" Text="6 mths" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl6Mth" runat="server">
                                <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right"  BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderStyle-CssClass="table-header" >
                                        <HeaderTemplate>
                                           <%-- <asp:LinkButton ID="Lnk1yr" runat="server" Font-Bold="true" Text="1Yr"
                                                Font-Overline="false" CommandName="Per_1_Year" CssClass="table-header"></asp:LinkButton>--%>
                                            <asp:Label ID="Lnk1yr" runat="server" SkinID="lblHeader" Text="1Yr" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1yr" runat="server">
                                <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right"  BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right"  HeaderStyle-CssClass="table-header">
                                        <HeaderTemplate>
                                           <%-- <asp:LinkButton ID="Lnk3yr" runat="server" Font-Bold="true" Text="3 Yrs"
                                                Font-Overline="false" CommandName="Per_3_Year" CssClass="table-header"></asp:LinkButton>--%>
                                             <asp:Label ID="Lnk3yr" runat="server" SkinID="lblHeader" Text="3 Yrs" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3yr" runat="server">
                                <%#Eval("Per_3_Year").ToString() != "" ? Eval("Per_3_Year") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" BackColor="#ffffff" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderStyle-CssClass="table-header" >
                                        <HeaderTemplate>
                                           <%-- <asp:LinkButton ID="LnkSI" runat="server" Font-Bold="true" Text="Since Inception"
                                                Font-Overline="false" CommandName="Per_Since_Inception" CssClass="table-header"></asp:LinkButton>--%>
                                             <asp:Label ID="LnkSI" runat="server" SkinID="lblHeader" Text="Since Inception" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSI" runat="server">
                                <%#Eval("Per_Since_Inception").ToString() != "" ? Eval("Per_Since_Inception") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right"  BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right"  HeaderStyle-CssClass="table-header">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="NAV" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNav" runat="server">
                                <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right"  BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left"  HeaderStyle-CssClass="table-header">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label2" runat="server" SkinID="lblHeader" Text="Category" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCat" runat="server">
                                <%#Eval("Nature").ToString() != "" ? Eval("Nature") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" BackColor="#ffffff" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left"  HeaderStyle-CssClass="table-header">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Structure" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblStruct" runat="server">
                                <%#Eval("Structure_Name").ToString() != "" ? Eval("Structure_Name") : "NA"%> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" BackColor="#ffffff"/>
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Watch List"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <img src="images/watch.jpg" style="cursor: pointer" alt="" name="imgAdd2Watch" id='<%#Eval("Sch_id")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderStyle-CssClass="table-header">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader" Text="Invest Now" CssClass="table-header"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                        
                                            <img src="img/rupee.png" style="cursor: pointer;" alt="" onclick="callCross('<%#Eval("Sch_id")%>','<%#Eval("Sch_Short_Name")%>','<%#Eval("Option_Id")%>','<%#Eval("Nature")%>','<%#Eval("Sub_Nature")%>')" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" BackColor="#ffffff" />
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    No Data Found
                                </EmptyDataTemplate>
                            </asp:GridView>

               
                   <asp:Label ID="lbRetrnMsg" Visible="false" runat="server" CssClass="gap-left">
                         <small>*Note:- Returns calculated for less than 1 year are Absolute returns and returns calculated for more than 1 year are compounded annualized.</small></asp:Label>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="row">
        <div class="col-xs-12 col-lg-12">
          <div class="panel panel-default" id="DivChart" runat="server">
            <div class="panel-body">
                <div class="ribbon ribbon_primary">
                    <div class="ribbon__title">
                        <asp:Label ID="lblSortPeriod" runat="server" Visible="false" >Click on 'Time Period' to rank funds on a particular period of your choice.
                            </asp:Label>                     
                    </div>
                </div>
                <div>
                    <div style="width: 97.5%; float: left;" id="chartContainer" class="block">
                            <div style="width: 98%; height: 100%;" id="divChart">
                            </div>
                        </div>

                          <%--new chart--%>
                            <div id="HighContainer" style="height: 600px; min-width: 310px; max-width: 1000px"></div>
                </div>
            </div>
          </div>
        </div>
      </div>
    
        <!-- Footer -->
        <footer class="page__footer">
          <div class="row">
            <div class="col-xs-12">
              <span class="page__footer__year"></span> Developed for Prism Advisory by: <a href="https://www.icraanalytics.com" target="_blank" >ICRA Analytics Ltd</a>&nbsp;<a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank" >Disclaimer</a>
            </div>
          </div>
        </footer>

     
      <!-- / .container-fluid -->
 <input id="HdSchemes" type="hidden" runat="server" />
 <input id="HdToData" type="hidden" runat="server" />
 <input id="HdFromData" type="hidden" runat="server" />
    </div>
    </div>
    </form>
</body>
</html>
