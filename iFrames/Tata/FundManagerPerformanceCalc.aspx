<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundManagerPerformanceCalc.aspx.cs"
     Inherits="iFrames.Tata.FundManagerPerformanceCalc" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="assets/ico/favicon.ico">

    <title>Kite: Admin</title>

    <!-- CSS Plugins -->
    <link rel="stylesheet" href="font-awesome-4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="css/pe-icon-7-stroke.css">
    <!-- CSS Global -->
    <link rel="stylesheet" href="css/style.css">
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/QuotWaiting.js"></script>
    <!-- JS Plugins -->
    <script src="assets/plugins/perfect-scrollbar/js/perfect-scrollbar.jquery.min.js"></script>

    <!-- JS Custom -->
    <script src="assets/js/theme.min.js"></script>
    <script src="assets/js/kite.min.js"></script>
    <script src="assets/js/custom.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
          //  debugger;
          /*  $("#BtnExcle").click(function (e) {
                var dataToPush = JSON.stringify({
                    schemeId: $("#ddlScheme").val(),
                    Year: $("#ddlYearEnd").val(),
                    Day: $("#ddlQtrEnd").val()
                });
                $.ajax({
                    cache: false,
                    data: dataToPush,
                    dataType: "json",
                    url: 'FundManagerPerformanceCalc.aspx/ExportToExcel',
                    type: 'POST',
                    asynchronus: true,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {                       
                        $('#loader').show();                       
                    },
                    success: function (dataConsolidated) {
                        $('#loader').hide();
                        var SchData = JSON.parse(dataConsolidated.d)
                        if (SchData[1] == 'Success') {
                            //  debugger;
                            window.location.href = SchData[0];
                        }
                        else {
                            alert('Something went worng!!!')
                            return;
                        }
                    },
                    error: function (data, e) {
                        //debugger;
                        alert(data);
                    }
                });
            });*/

            $("#BtnReset").click(function (e) {

                window.location = window.location.href;
            });
            $("#BtnSubmit").click(function (e) {
                var dataToPush = JSON.stringify({
                    schemeId: $("#ddlScheme").val(),
                    Year: $("#ddlYearEnd").val(),
                    Day: $("#ddlQtrEnd").val()
                });

                $("#tblSchemeRtn").html('');
                $("#FM").html('');

                $.ajax({
                    cache: false,
                    data: dataToPush,
                    dataType: "json",
                    url: 'FundManagerPerformanceCalc.aspx/ShowSchemeWiseReport',
                    type: 'POST',
                    asynchronus: true,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $('#loader1').show();
                        //  waitingDialog.show();
                        $('#BtnSubmit').prop('disabled', true);
                        $('#BtnReset').prop('disabled', true);
                    },
                    success: function (dataConsolidated) {
                        $('#loader1').hide();
                      //  waitingDialog.hide();
                        var SchData = JSON.parse(dataConsolidated.d)
                        if (SchData == null) {
                            $('#BtnSubmit').prop('disabled', false);
                            $('#BtnReset').prop('disabled', false);
                            alert("Data Not Found !!!")
                            return;
                        }

                        if (SchData.PageDataReturnHeader.length > 0) {

                            var HeadRow1 = "<thead><tr class='table-header'>'<th></th>'";

                            for (i = 0; i < SchData.PageDataReturnHeader.length; i++) {
                                HeadRow1 = HeadRow1 + '<th colspan=2 style="text-align:center">' + SchData.PageDataReturnHeader[i] + '</th>';
                            }
                            HeadRow1 = HeadRow1 + '<th></th></tr></thead>';
                            $("#tblSchemeRtn").append(HeadRow1);
                        }

                        if (SchData.PageDataStructure.length > 0) {

                            //change
                            $('#DivTbldata').show();
                            $("#HdnScheme").val($("#ddlScheme").val());
                            $("#HdnYearEnd").val($("#ddlYearEnd").val());
                            $("#HdnQtrEnd").val($("#ddlQtrEnd").val());
                            $('#BtnExport').show();
                            //

                            var HeadRow = "<thead><tr class='table-header'>";

                            for (i = 0; i < SchData.PageDataStructure.length; i++) {
                                var ColName = SchData.PageDataStructure[i];
                                if (SchData.PageDataStructure[i].indexOf("Scheme_Name") >= 0) {
                                    ColName = "Scheme / Benchmark";
                                }
                                if (SchData.PageDataStructure[i].indexOf("Inception_Date") >= 0) {
                                    ColName = "Inception Date";
                                }
                                if (SchData.PageDataStructure[i].indexOf("_Rtn") >= 0) {
                                    ColName = "Return %";
                                }
                                if (SchData.PageDataStructure[i].indexOf("_Amt") >= 0) {
                                    ColName = "Amount (<i class='fa fa-inr' aria-hidden='true'></i>)";
                                }
                                HeadRow = HeadRow + '<th>' + ColName + '</th>';
                            }
                            HeadRow = HeadRow + '</tr></thead>';

                            $("#tblSchemeRtn").append(HeadRow);
                        }
                        if (SchData.PageData.length > 0) {
                            var Body = "<tbody>";
                            for (t = 0; t < SchData.PageData.length; t++) {
                                Body = Body + "<tr>";
                                for (j = 0; j < SchData.PageDataStructure.length; j++) {
                                    Body = Body + '<td>' + SchData.PageData[t][j] + '</td>';
                                }
                                Body = Body + "</tr>";
                            }
                            Body = Body + "</tbody";
                            $("#tblSchemeRtn").append(Body);

                            GenerateTable();
                        }
                    },
                    error: function (data, e) {
                        //debugger;
                        $('#BtnSubmit').prop('disabled', false);
                        $('#BtnReset').prop('disabled', false);
                        $('#loader1').hide();
                        alert(data);
                       
                    }
                });
            });


            function GenerateTable() {

                var dataToPush = JSON.stringify({
                    schemeId: $("#ddlScheme").val(),
                    Year: $("#ddlYearEnd").val(),
                    Day: $("#ddlQtrEnd").val()
                });
                $.ajax({
                    cache: false,
                    data: dataToPush,
                    dataType: "json",
                    url: 'FundManagerPerformanceCalc.aspx/ShowFundManagerSchemeWiseReport',
                    type: 'POST',
                    asynchronus: true,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $('#loader').show();
                        //  waitingDialog.show();
                        $('#BtnSubmit').prop('disabled', true);
                    },
                    success: function (dataConsolidated) {
                        //debugger;
                        var FMArr = JSON.parse(dataConsolidated.d);
                        $('#loader').hide();
                        $('#BtnSubmit').prop('disabled', false);
                        $('#BtnReset').prop('disabled', false);
                       // waitingDialog.hide();
                        for (f = 0; f < FMArr.length; f++) {
                            var SchemesArr = FMArr[f];
                            var InnerHTML = "<div class='panel panel-primary'>";
                            InnerHTML = InnerHTML + "<div class='panel-heading' role='tab' id='headingTwo3' style='padding:8px;'>";
                            InnerHTML = InnerHTML + "<div class='panel-title' style='font-size:13px;'>";
                            InnerHTML = InnerHTML + "<a role='button' data-toggle='collapse' data-parent='#accordion2' href='#collapseTwo" + f + "' aria-expanded='true' aria-controls='collapseTwo'>Performance of other schemes managed by the " + SchemesArr.FundManagerName + "</a></div></div>";
                            InnerHTML = InnerHTML + "<div id='collapseTwo" + f + "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='headingTwo3' aria-expanded='true'>";
                            InnerHTML = InnerHTML + "<div class='panel-body' id='PB'" + f + " style='padding-top:10px;'>";

                            var ObjScheme = SchemesArr.LstDataStructure;
                            for (q = 0; q < ObjScheme.length; q++) {

                                InnerHTML = InnerHTML + "<div class='table-responsive'>	<table id='TblMFSch" + q + "' class='table table-bordered'>";

                                var SchData = ObjScheme[q];
                                if (SchData.PageDataReturnHeader.length > 0) {

                                    var HeadRow1 = "<thead><tr class='table-header'><th></th>";

                                    for (i = 0; i < SchData.PageDataReturnHeader.length; i++) {
                                        HeadRow1 = HeadRow1 + '<th colspan=2 style="text-align:center">' + SchData.PageDataReturnHeader[i] + '</th>';
                                    }
                                    HeadRow1 = HeadRow1 + '<th></th></tr></thead>';
                                    InnerHTML = InnerHTML + HeadRow1;
                                   // $("#TblMFSch" + i).append(HeadRow1);
                                }

                                if (SchData.PageDataStructure.length > 0) {
                                   // debugger;
                                    var HeadRow = "<thead><tr class='table-header'>";

                                    //for (i = 0; i < SchData.PageDataStructure.length; i++) {
                                    //    HeadRow = HeadRow + '<th>' + SchData.PageDataStructure[i] + '</th>';
                                    //}

                                    for (i = 0; i < SchData.PageDataStructure.length; i++) {
                                        var ColName = SchData.PageDataStructure[i];
                                        if (SchData.PageDataStructure[i].indexOf("Scheme_Name") >= 0) {
                                            ColName = "Scheme / Benchmark";
                                        }
                                        if (SchData.PageDataStructure[i].indexOf("Inception_Date") >= 0) {
                                            ColName = "Inception Date";
                                        }
                                        if (SchData.PageDataStructure[i].indexOf("_Rtn") >= 0) {
                                            ColName = "Return %";
                                        }
                                        if (SchData.PageDataStructure[i].indexOf("_Amt") >= 0) {
                                            ColName = "Amount(Rs.)";
                                        }
                                        HeadRow = HeadRow + '<th>' + ColName + '</th>';
                                    }

                                    HeadRow = HeadRow + '</tr></thead>';

                                    InnerHTML = InnerHTML + HeadRow
                                    // $("#tblSchemeRtn").append(HeadRow);
                                }

                                if (SchData.PageData.length > 0) {
                                    var Body = "<tbody>";
                                    for (t = 0; t < SchData.PageData.length; t++) {
                                        Body = Body + "<tr>";
                                        for (j = 0; j < SchData.PageDataStructure.length; j++) {
                                            Body = Body + '<td>' + SchData.PageData[t][j] + '</td>';
                                        }
                                        Body = Body + "</tr>";
                                    }
                                    Body = Body + "</tbody>";
                                    // $("#tblSchemeRtn").append(Body);
                                }
                                InnerHTML = InnerHTML + Body + "</tbody></table></div>";
                            }
                            InnerHTML = InnerHTML + "</div></div>";

                            $("#FM").append(InnerHTML);
                        }
                    },
                    error: function (data, e) {
                        //debugger;
                        $('#BtnSubmit').prop('disabled', false);
                        $('#BtnReset').prop('disabled', false);
                        $('#loader').hide();
                        alert(data);
                    }
                });
            }
        });

      
    </script>
</head>

<body>

    <!-- NAVBAR
    ================================================== -->


    <!-- WRAPPER
    ================================================== -->
    <div class="wrapper">
        <form id="Form1" runat="server">

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
                <div class="row" style="margin: 0 auto;">
                    <div class="col-xs-12 col-lg-2"></div>
                    <div class="col-xs-12 col-lg-8">

                        <!-- Basic form -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                            </div>
                            <div class="panel-body">
                                 <div class="col-lg-1" align="center"></div>
                                 <div class="col-lg-10" align="left">
                                     <div class="form-horizontal">
                                    <div class="form-group">
                                        <label for="inputPassword3" class="col-sm-3 control-label"></label>
                                        <div class="col-sm-9">
                                            <div class="col-sm-3" style="padding-left: 0;">
                                                <div class="radio">
                                                    <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1" checked>
                                                    <label for="optionsRadios1">
                                                        Scheme wise
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="radio">
                                                    <input type="radio" name="optionsRadios" id="Radio1" value="option1">
                                                    <label for="optionsRadios1">
                                                        Fund Manager wise
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-3 control-label-select">Category</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlCategory" runat="server" class="form-control"
                                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                                           
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-3 control-label-select">Option</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlOption" runat="server" class="form-control"
                                                OnSelectedIndexChanged="ddlOption_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                                           </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-3 control-label-select">Select Scheme</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlScheme" runat="server" class="form-control"
                                                 OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-3 control-label-select">Benchmark</label>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="ddlBenchMark" runat="server" class="form-control"></asp:DropDownList>

                                            <%--<input type="text" class="form-control" placeholder="CRISIL Balanced Fund - Aggressive Index" disabled>--%>
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-3 control-label-select">Value of Investment</label>
                                        <div class="col-sm-5">
                                            <div class="prepend-icon">
                                                <input type="text" class="form-control" placeholder="10,000" style="padding-left: 16px;" disabled><i class="fa fa-inr" aria-hidden="true"></i>
                                            </div>
                                        </div>
                                        <div class="col-sm-5"></div>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputEmail3" class="col-sm-3 control-label-select">Value as on Date</label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlQtrEnd" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:DropDownList ID="ddlYearEnd" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="ddlYearEnd_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-offset-3 col-sm-9">
                                            <div class="btn-group" role="group">
                                                <button type="button" id="BtnSubmit" class="btn btn-primary">Submit</button>
                                                <button type="button" id="BtnReset" class="btn btn-default">Reset</button>
                                                <asp:Button ID="BtnExport" runat="server" Text="Export to Excel" class="btn btn-success" style="display:none" OnClick="BtnExport_Click" />
                                               <%-- <button type="button" id="BtnExcle" class="btn btn-success">Download Excel</button>
                                                <label id="loader" style="display:none"><img  src="../Images/ajax-loader.gif" alt="Lodding.." /></label>--%>
                                                 <asp:HiddenField ID="HdnScheme" runat="server" />
                                                 <asp:HiddenField ID="HdnYearEnd" runat="server" />
                                                 <asp:HiddenField ID="HdnQtrEnd" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                 </div>
                                
                            </div>
                        </div>

                        <!-- Bordered table -->
                        <div class="panel panel-default" id="DivTbldata" style="display:none">
                            <div class="panel-body" >
                                 
                                <br>
                               
                                <div class="table-responsive" id="divSchemeRtn">
                                    <div class="col-lg-12" align="center">
                                        <label id="loader1" style="display:none;text-align:center"><img src="../Images/loading.gif" alt="Lodding.." /></label>
                                    </div>
                                     
                                    <table id="tblSchemeRtn" class="table table-bordered" ></table>
                                </div>
                                <div class="col-lg-12" align="center">
                                    <label id="loader" style="display:none;text-align:center"><img  src="../Images/loading.gif" alt="Lodding.." /></label>
                                </div>
                                <br>
                                <div id="FM">
                             
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-lg-2"></div>
                    </div>
                    <!-- / .row -->

                    <!-- Footer -->
                  <%--  <footer class="page__footer">
                        <div class="row">
                            <div class="col-xs-12">

                                <span class="page__footer__year"></span>&copy; Kite theme by <a href="http://simpleqode.com/">Simpleqode.com</a> | Purchase via <a href="https://wrapbootstrap.com/theme/kite-responsive-admin-template-WB0J0800M">Wrapbootstrap.com</a>
                            </div>
                        </div>
                    </footer>--%>

                </div>
                <!-- / .container-fluid -->

            </div>
            <!-- / .wrapper -->


            <!-- JavaScript
    ================================================== -->

            <!-- JS Global -->
        </form>

    </div>

</body>
</html>
