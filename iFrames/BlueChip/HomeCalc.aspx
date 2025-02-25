<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeCalc.aspx.cs"
    Inherits="iFrames.BlueChip.HomeCalc" %>

<!DOCTYPE html>

<html class="no-js">

<head runat="server">
    <title>Performance Calculator</title>

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />


    <link rel="stylesheet" href="css/new/jquery-ui.css" />
    <link rel="stylesheet" href="css/new/all.css" />
    <link rel="stylesheet" href="css/new/bootstrap-datepicker.css" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>
    <script src="js/new/new/bootstrap-datepicker.js"></script>
    <script src="https://use.fontawesome.com/ea6a7e4db5.js"></script>

    <script type="text/javascript">
        function checkInvestedAmount() {
            var sipamunt = document.getElementById('txtinstallLs').value;
            if (isNaN(sipamunt)) {
                alert("Please enter Numeric value.");
                document.getElementById('txtinstallLs').value = "";
                document.getElementById('txtinstallLs').focus();
                return false;
            }
            else {
                if (sipamunt < 1000) {
                    alert('Minimum investment amount should be Rs. 1000/- ');
                    document.getElementById('txtinstallLs').value = "";
                    document.getElementById("txtinstallLs").focus();
                    return false;
                }
            }
        }
    </script>

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

    <script type="text/javascript">


        $(function () {

            function GetSchDate() {
                var date2 = $('#SIPSchDt').val();

                if (date2 != null && date2 != '' && date2 != "" && date2 != 0) {
                    var Day2 = parseInt(date2.substring(0, 2), 10);
                    var Mn2 = parseInt(date2.substring(3, 5), 10);
                    var Yr2 = parseInt(date2.substring(6, 10), 10);
                    var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;

                    var dt2 = new Date(DateVal2);

                    $("#txtfromDate").datepicker('option', 'minDate', dt2);
                    $("#txtToDate").datepicker('option', 'minDate', dt2);

                    return dt2;
                }
                else {
                    var startsensexdate = new Date("01/01/1980");
                    $("#txtfromDate").datepicker('option', 'minDate', startsensexdate);
                    $("#txtToDate").datepicker('option', 'minDate', startsensexdate);
                    return startsensexdate;
                }
            };

            $('#txtfromDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                endDate: new Date(),
            });



            $('#txtToDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                endDate: new Date(),
            });

            $(".datepicker-input").attr("autocomplete", "off");
        });
    </script>

    <script type="text/javascript">
        function dateValidation() {

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
            };

            var schN = document.getElementById("ddlSchemes");
            var schName = schN.value;

            if (schName != null && schName != '' && schName != "" && schName != '0' && schName != '-1') {


                var date = $('#txtfromDate').val();
                if (date == null || date == NaN || date == "" || date == '') {
                    document.getElementById('txtfromDate').value = "";
                    document.getElementById('txtfromDate').focus();
                    alert("From Date cannot be blank");
                    return false;
                }

                var Day = parseInt(date.substring(0, 2), 10);
                var Mn = parseInt(date.substring(3, 5), 10);
                var Yr = parseInt(date.substring(6, 10), 10);
                var DateVal = Mn + "/" + Day + "/" + Yr;


                var dt = new Date(DateVal);

                var todaydate = new Date();
                //alert(todaydate);alert(dt);
                var i = DateDiff.inDays(dt, todaydate)
                if (i <= 0) {
                    alert("Start Date should be Less than Today");
                    document.getElementById('txtfromDate').value = "";
                    document.getElementById('txtfromDate').focus();
                    return false;
                }



                var date1 = $('#txtToDate').val();

                if (date1 == null || date1 == NaN || date1 == "" || date1 == '') {
                    document.getElementById('txtToDate').value = "";
                    document.getElementById('txtToDate').focus();
                    alert("End Date cannot be blank");
                    return false;
                }

                var Day1 = parseInt(date1.substring(0, 2), 10);
                var Mn1 = parseInt(date1.substring(3, 5), 10);
                var Yr1 = parseInt(date1.substring(6, 10), 10);
                var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;


                var dt1 = new Date(DateVal1);

                i = DateDiff.inDays(dt1, todaydate)
                if (i <= 0) {
                    alert("Start Date should be Less than Today");
                    document.getElementById('txtToDate').value = "";
                    document.getElementById('txtToDate').focus();
                    return false;
                }

                var date2 = $('#SIPSchDt').val();

                if (date2 != null && date2 != '' && date2 != "" && date2 != 0) {
                    var Day2 = parseInt(date2.substring(0, 2), 10);
                    var Mn2 = parseInt(date2.substring(3, 5), 10);
                    var Yr2 = parseInt(date2.substring(6, 10), 10);
                    var DateVal2 = Mn2 + "/" + Day2 + "/" + Yr2;

                    var dt2 = new Date(DateVal2);

                    if (dt2 > dt) {

                        document.getElementById('txtfromDate').value = "";
                        document.getElementById('txtfromDate').focus();
                        alert("Start Date should be greater than Inception date");
                        return false;
                    }
                }


                if (dt >= dt1) {
                    alert("End date should be greater than Start date");
                    return false;
                }
            }
            else {
                alert("Please Select a scheme");
                document.getElementById('ddlSchemes').focus();
                return false;
            }
        };
    </script>

    <style>
        .card{
            border:none;
        }
        .card-body {
            padding: 0 !important;
        }
        .card-header {
            padding: 10px !important;
        }
        .card-header h5{
            margin:0;
        }
        label {
            margin-bottom: 0.4rem;
            margin-top:0.5rem;
        }

        #rdbOption table tbody tr td {
            margin-right: 20px;
            padding-right: 20px;
        }

        #rdbOption_0 {
            margin-right: 5px;
            margin-top: 8px;
            font-weight: normal !important;
        }

        #rdbOption_1 {
            margin-right: 5px;
            margin-top: 8px;
            font-weight: normal !important;
        }

        .tooltip {
            font-size: 10px;
        }

        /* Style the tab */
        .tab {
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }

            /* Style the buttons inside the tab */
            .tab button {
                background-color: inherit;
                float: left;
                border: none;
                outline: none;
                cursor: pointer;
                padding: 14px 16px;
                transition: 0.3s;
                font-size: 17px;
            }

                /* Change background color of buttons on hover */
                .tab button:hover {
                    background-color: #ddd;
                }

                /* Create an active/current tablink class */
                .tab button.active {
                    background-color: #ccc;
                }

        /* Style the tab content */
        .tabcontent {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }

        label {
            font-size: 12px;
            font-weight: normal;
        }

        .form-control {
            padding: 4px 3px;
            height: 30px;
        }
    </style>
</head>
<body style="font-size: 12px!important">
    <form id="formHome" runat="server">
        <div style="width: 350px; height: 350px;">
            <div class="card" style="border: none">
                <div class="card-body">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="card">
                                <div class="card-header bg-header">
                                    <h5>Mutual Fund Calculator</h5>
                                </div>
                                <div class="card-body">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-2 p-0">
                                        <label>Mutual Fund Name</label>
                                        <asp:DropDownList ID="ddlFundHouse" runat="server"
                                            OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                            AutoPostBack="true"
                                            class="form-control form-control-sm">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0 pt-2"
                                        style="display: none">

                                        <label>Fund Name</label>
                                        <asp:DropDownList ID="ddlFundName" runat="server" OnSelectedIndexChanged="ddlFund_SelectedIndexChanged"
                                            AutoPostBack="true"
                                            class="form-control form-control-sm">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0 pt-2">

                                        <label>Scheme Name</label>
                                        <asp:DropDownList ID="ddlSchemes" runat="server" OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged"
                                            AutoPostBack="true"
                                            class="form-control form-control-sm">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0 pt-2">
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6" style="width:180px">
                                                <label>Investment Amount(₹)</label>
                                                <asp:TextBox ID="txtinstallLs" CssClass="form-control form-control-sm"
                                                    MaxLength="10" Text="" runat="server" ReadOnly="false"
                                                    onChange="Javascript: checkInvestedAmount();"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6" style="width:180px">
                                                <label>Inception Date</label>
                                                <asp:TextBox ID="SIPSchDt" runat="server" CssClass="form-control form-control-sm"
                                                    ReadOnly="true" Text="" onchange="Javascript: return GetSchDate();"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0 pt-2">
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6" style="width:180px">
                                                <label>Start Date</label>
                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control form-control-sm datepicker-input"
                                                    autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6" style="width:180px">
                                                <label>End Date</label>
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker-input form-control form-control-sm"
                                                    autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-4 pl-0">

                                        <asp:Button type="submit" name="btnSubmit" value="Calculate"
                                            runat="server" Text="Submit" ID="btnSubmit" OnClientClick="checkInvestedAmount(); return dateValidation();"
                                            OnClick="btnsubmit_Clicked" class="btn btn-danger btn-sm" />
                                        <asp:Button type="submit" name="btnReset" runat="server"
                                            Text="Reset" value="Reset" ID="btnReset" OnClick="btnreset_Clicked"
                                            class="btn btn-light btn-sm" />

                                    </div>
                                    <div style="font-size: 10px; color: #A7A7A7" class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-3">
                                        <div class="row">
                                            Developed by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">
                                                ICRA Analytics Ltd,
                                            </a>
                                        <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer</a>
                                        </div>
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