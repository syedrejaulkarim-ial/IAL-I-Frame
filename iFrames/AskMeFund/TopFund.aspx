<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.AskMeFund.TopFund" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
     <meta charset="utf-8" />
     <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
     <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Mutual Funds</title>
    <script type='text/javascript' src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom.min.css" />
    <script src="../Scripts/HighStockChart/highstock.js" type="text/javascript"></script>
    <script src="../Scripts/HighStockChart/exporting.js" type="text/javascript"></script>
    <script src="js/moment.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.6.3/css/all.css" /> 
    <link href="assets/css/custom.css" rel="stylesheet" />

    
    <!-- Bootstrap Css -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- Icons Css -->
    <link href="assets/css/icons.min.css" rel="stylesheet" type="text/css" />
    <!-- App Css-->
    <link href="assets/css/app.min.css" rel="stylesheet" type="text/css" />

    <!-- nouisliderribute css -->
    <%--<link rel="stylesheet" href="assets/libs/nouislider/nouislider.min.css" />--%>

    <style>
        .pg-goto {
            background-color: #80838b;
            border: #80838b solid 1px;
            padding: 0.2rem 0.45rem;
            font-size: .9375rem;
            border-radius: 0.2rem;
            font-weight: 400;
            line-height: 1.5;
            color: #fff;
            text-align: center;
            vertical-align: middle;
            margin: 0px 5px;
            cursor: pointer;
        }

        .pg-selected, .pg-normal {
            margin: 0px 5px;
            cursor: pointer;
            padding:5px;
            border-radius:2px;
        }

        .pg-selected{
               background-color: #4467a6;
                border: #80838b solid 1px;
                padding: 0.2rem 0.45rem;
                font-size: .9375rem;
                border-radius: 0.2rem;
                font-weight: 500;
                line-height: 1.5;
                color: #fff;
                text-align: center;
                vertical-align: middle;
                margin: 0px 5px;
        }

        .btn-check:active + .btn-primary, .btn-check:checked + .btn-primary, .btn-primary.active, .btn-primary:active, .show > .btn-primary.dropdown-toggle {
            color: #fff;
            background-color: #4467a6;
            border-color: #4467a6;
        }
        .btn-outline-primary:hover {
             color: #fff;
             background-color: #4467a6;
             border-color: #4467a6;
        }
        .risk-active {
            text-shadow: -1px 1px 1px #565c64;
            font-weight: bold;
        }
        .btn-group>.btn-group:not(:first-child), .btn-group>.btn:not(:first-child) {
            margin-left: 0px!important;
        }
        .radio label, .checkbox label {
            padding-left: 6px;
            padding-top: 10px;
            cursor: pointer;
        }
        /*.w-xs {
            min-width: 70px;
        }*/
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


        /*.btn-header-links {
            padding-top: 5px;
            padding-bottom: 15px;
            overflow-x: scroll;
            display: inline-block;
            white-space: nowrap;
            transition: 1s ease;
        }*/

        .padding-align {
            padding-left: 4em !important;
            padding-right: 4em !important;
        }

        .top-adjust {
            top: 1.4em;
        }

        

        /*scroller parent style*/

       

        /*********************/
        /*button styles*/
        .btn-pin {
            border: 0;
            color: #fff;
            font-weight: normal;
            text-transform: capitalize;
           /* -webkit-box-shadow: 0 2px 5px 0 rgba(0,0,0,.16), 0 2px 10px 0 rgba(0,0,0,.12);
            box-shadow: 0 2px 5px 0 rgba(0,0,0,.16), 0 2px 10px 0 rgba(0,0,0,.12);*/
        }


        .btn-bg10 {
            background-color: #3B78C1 !important;
        }





        /******************** Loader Style ***************************/

        #overlay {
            position: fixed;
            top: 0;
            z-index: 99999;
            width: 100%;
            height: 100%;
            display: none;
            background: rgba(0,0,0,0.6);
        }

        .cv-spinner {
            height: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .spinner {
            width: 40px;
            height: 40px;
            border: 4px #ddd solid;
            border-top: 4px #2e93e6 solid;
            border-radius: 50%;
            animation: sp-anime 0.8s infinite linear;
        }

        @keyframes sp-anime {
            100% {
                transform: rotate(360deg);
            }
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


        function ResultSetData(data) {
            
            var res = "";
            var host = $("#HostAuthority").val();
            var Period = $("#HiddenPeriod").val();
            for (i = 0; i < data.length; i++) {

                for (const [key, value] of Object.entries(data[i])) {

                    if (value == null || value == "0") {
                        data[i][key] = "--";
                    }
                }

                res += "<tr><td>";
                res += '<div class="card"><div class="card-body pb-xl-2"><h4 class="font-size-17 mb-1"><a href="https://askmefund.com/factsheet/' + data[i].Scheme_Id + '" target="_blank" class="text-dark">' + data[i].Sch_Name + '</a></h4>';
                res += '<div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12"><div class="row"><div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-xs-12"><div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12"><p>' + data[i].Curr_Nav + '<span class="';

                if (data[i].Curr_Nav - data[i].Prev_Nav > 0) {
                    res += 'mdi mdi-arrow-up-bold text-success';
                } else {
                    res += 'mdi mdi-arrow-down-bold text-success';
                }

                res += '"></span>' + data[i].Incr_Nav + '</p></div>';
                res += '<div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12"><div class="row"><div class="col"><div><h6 style="margin-bottom: 0px;">' + data[i].LblHigh + ' <small class="text-muted"> (' + data[i].SpnMaxDate + ')</small></h6><p class="text-muted mb-0"><span class="badge  badge-soft-success font-size-12">High Nav</span></p></div></div>';
                res += '<div class="col"><div><h6 style="margin-bottom: 0px;">' + data[i].LblLow + ' <small class="text-muted"> (' + data[i].spnMinDate + ')</small></h6><p class="text-muted mb-0"><span class="badge  badge-soft-danger  font-size-12">Low Nav</span></p></div></div></div></div></div>';

                switch (Period)
                    {
                        case "Per_182_Days":
                            res += '<div class="mt-2 col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xs-12"><div class="row"><div class="col border-end"><div class="text-center"><p class="text-muted mb-0">1 Months</p><h6>' + data[i].Per_30_Days + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">3 Months</p><h6>' + data[i].Per_91_Days + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">6 Months</p><h6>' + data[i].Per_182_Days + '</h6></div></div>';
                            break;
                        case "Per_1_Year":
                            res += '<div class="mt-2 col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xs-12"><div class="row"><div class="col border-end"><div class="text-center"><p class="text-muted mb-0">3 Months</p><h6>' + data[i].Per_91_Days + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">6 Months</p><h6>' + data[i].Per_182_Days + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">12 Months</p><h6>' + data[i].Per_1_Year + '</h6></div></div>';
                            break;
                        case "Per_3_Year":
                            res += '<div class="mt-2 col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xs-12"><div class="row"><div class="col border-end"><div class="text-center"><p class="text-muted mb-0">6 Months</p><h6>' + data[i].Per_182_Days + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">12 Months</p><h6>' + data[i].Per_1_Year + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">3 years</p><h6>' + data[i].Per_3_Year + '</h6></div></div>';
                            break;
                        default:
                            res += '<div class="mt-2 col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xs-12"><div class="row"><div class="col border-end"><div class="text-center"><p class="text-muted mb-0">1 Weeks</p><h6>' + data[i].Per_7_Days + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">1 Months</p><h6>' + data[i].Per_30_Days + '</h6></div></div>';
                            res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">3 Months</p><h6>' + data[i].Per_91_Days + '</h6></div></div>';
                            break;
                    }

                //res += '<div class="mt-2 col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xs-12"><div class="row"><div class="col border-end"><div class="text-center"><p class="text-muted mb-0">6 months</p><h6>' + data[i].Per_182_Days + '</h6></div></div>';
                //res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">1 year</p><h6>' + data[i].Per_1_Year + '</h6></div></div>';
                //res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">3 years</p><h6>' + data[i].Per_3_Year + '</h6></div></div>';



                res += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">Since inception</p><h6>' + data[i].Since_Inception + '</h6></div></div>';
                res += '<div class="col"><div class="text-center"><p class="text-muted mb-0">Fund size (Cr)</p><h6>' + data[i].Fund_Size + '</h6></div></div></div>';
                res += '</div></div>';
                res += '<div class="row"><div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 mt-2" style="text-align: right; padding: 0px;"><a href="https://askmefund.com/factsheet/' + data[i].Scheme_Id + '" target="_blank" class="text-primary fw-semibold"><u>More details</u><i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>';
                res += '</div></div></div></div></div></td></tr>';
            }

            $("#SearchResult").html(res);


            paginationTbl();

        }



        $(document).ready(function () {

            //var closeButtonClicked = false;

            //$('.btn-close').on('click', function () {
            //    closeButtonClicked = true;
            //});

            //$('#CompareModel').on('hide.bs.modal', function (e) {
            //    
            //    if (!closeButtonClicked) {
            //        e.preventDefault();
            //        e.stopPropagation();
            //        return false;
            //    }
            //   closeButtonClicked = false;
            //});

           <%-- $('#<%= rdbOptionCompare.ClientID %> input[type="radio"]').click(function (e) {
                e.stopPropagation();
            })--%>

            //$('#rdbOptionCompare').on('click', function (event) {
            //    e.stopPropagation();
               
            //    //event.preventDefault();
            //}


            $('#CompareModel').on('click', function (event) {
              event.stopPropagation();
              event.preventDefault();
            });

            $( "#ddlSchemesCompare" ).on( "change", function() {
                var SchId = $('#ddlSchemesCompare :selected').val();
                if (SchId == "-1" || SchId == "0") {
                    $('#NoSelectSchemeMsg').show();
                    $('#btnAddSchemeCompares').removeAttr('data-bs-toggle');


                } else {
                    $('#NoSelectSchemeMsg').hide();
                    $('#btnAddSchemeCompares').attr('data-bs-toggle', 'modal');
                }
                
            });

            $( "#ddlSchemesNav" ).on( "change", function() {
               var SchId = $('#ddlSchemesNav :selected').val();
                if (SchId == "-1" || SchId == "0") {
                    $('#NoSelectSchemeMsgNav').show();
                } else {
                    $('#NoSelectSchemeMsgNav').hide();
                }
            });

           

            

            $("#ddlSubCategoryCompare").on("change", function () {
               $('#btnAddSchemeCompares').removeAttr('data-bs-toggle');
            });

            $("#btnSubmit").click(function (Func1) {
                

                var data2 = JSON.stringify({
                    ddlCategory: $('#ddlCategory :selected').val(),
                    ddlType: $('#ddlType :selected').val(),
                    ddlSubCategory: $('#ddlSubCategory :selected').val(),
                    rdbOption: $('#rdbOption :checked').val(),
                    //ddlRank: $('#ddlRank :selected').val(),
                    ddlPeriod: $('#ddlPeriod :selected').val(),
                    HiddenFundRisk: $('#HiddenFundRisk').val(),
                    HiddenMinimumInvesment: $('#HiddenMinimumInvesment').val(),
                    HiddenMinimumSIReturn: $('#HiddenMinimumSIReturn').val()


                });

                $("#HiddenPeriod").val($('#ddlPeriod :selected').val());

                $.ajax({
                    url: "TopFund.aspx/GetSearchResult",
                    type: "POST",
                    data: data2,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $("#overlay").show();
                    },
                    success: function (data) {
                        
                        var RtnData = JSON.parse(data.d);
                        if (RtnData.length > 0)
                            ResultSetData(RtnData);
                        else {

                            var card = '<div class="card"><div class="card-body pb-xl-2">';

                            card += '<h4 class="font-size-17 mb-1">No data found</h4>';

                            card += '</div></div>';

                            $("#SearchResult").html(card);
                        }

                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    },
                    complete: function () {
                        $("#overlay").hide();
                    }

                });


            });




            var cssclass = "btn btn-primary btn-sm  " + $("#HiddenFundRiskStrColor").val() + "risk-active";

            $("#" + $("#HiddenFundRiskStrColor").val().replace("-", "_")).attr("class", cssclass);

            if (FundRiskValue == null) {
                $("#low").click(function (Func1) {
                    
                    FundRiskValue = $("#low").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn w-xs btn-success-soft risk-active");
                    $("#mod_low").attr("class", "btn btn-success w-xs");
                    $("#mod").attr("class", "btn btn-info w-xs");
                    $("#mod_high").attr("class", "btn btn-warning w-xs");
                    $("#high").attr("class", "btn btn-danger w-xs");
                    $("#all").attr("class", "btn btn-primary w-xs");
                    $("#HiddenFundRiskStrColor").val("low");
                });

                $("#mod_low").click(function (Func2) {
                    FundRiskValue = $("#mod_low").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn btn-success-soft w-xs");
                    $("#mod_low").attr("class", "btn btn-success risk-active");
                    $("#mod").attr("class", "btn btn-info w-xs");
                    $("#mod_high").attr("class", "btn btn-warning w-xs");
                    $("#high").attr("class", "btn btn-danger w-xs");
                    $("#all").attr("class", "btn btn-primary w-xs");
                    $("#HiddenFundRiskStrColor").val("mod-low");
                });

                $("#mod").click(function (Func2) {
                    FundRiskValue = $("#mod").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn btn-success-soft w-xs");
                    $("#mod_low").attr("class", "btn btn-success w-xs");
                    $("#mod").attr("class", "btn btn-info risk-active");
                    $("#mod_high").attr("class", "btn btn-warning w-xs");
                    $("#high").attr("class", "btn btn-danger w-xs");
                    $("#all").attr("class", "btn btn-primary w-xs");
                    $("#HiddenFundRiskStrColor").val("mod");
                });

                $("#mod_high").click(function (Func2) {
                    FundRiskValue = $("#mod_high").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn btn-success-soft w-xs");
                    $("#mod_low").attr("class", "btn btn-success w-xs");
                    $("#mod").attr("class", "btn btn-info w-xs");
                    $("#mod_high").attr("class", "btn btn-warning risk-active");
                    $("#high").attr("class", "btn btn-danger w-xs");
                    $("#all").attr("class", "btn btn-primary w-xs");
                    $("#HiddenFundRiskStrColor").val("mod-high");
                });

                $("#high").click(function (Func3) {
                    FundRiskValue = $("#high").attr("data-risk");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn btn-success-soft w-xs");
                    $("#mod_low").attr("class", "btn btn-success w-xs");
                    $("#mod").attr("class", "btn btn-info w-xs");
                    $("#mod_high").attr("class", "btn btn-warning w-xs");
                    $("#high").attr("class", "btn btn-danger risk-active");
                    $("#all").attr("class", "btn btn-primary w-xs");
                    $("#HiddenFundRiskStrColor").val("high");
                });

                $("#all").click(function (Func4) {
                    FundRiskValue = $("#all").attr("for");
                    $("#HiddenFundRisk").val(FundRiskValue);
                    $("#low").attr("class", "btn btn-success-soft w-xs");
                    $("#mod_low").attr("class", "btn btn-success w-xs");
                    $("#mod").attr("class", "btn btn-info w-xs");
                    $("#mod_high").attr("class", "btn btn-warning w-xs");
                    $("#high").attr("class", "btn btn-danger w-xs");
                    $("#all").attr("class", "btn btn-primary risk-active");
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



        });

    </script>
    <script type="text/javascript">


        function BindSchemeList() {
            var data2 = JSON.stringify({
                ddlCategoryCompare: $('#ddlCategoryCompare :selected').val(),
                ddlTypeCompare: $('#ddlTypeCompare :selected').val(),
                ddlSubCategoryCompare: $('#ddlSubCategoryCompare :selected').val(),
                rdbOptionCompare: $('#ddlOptionCompare :selected').val(),
                ddlFundHouseCompare: $('#ddlFundHouseCompare :selected').val()
            });

            $.ajax({
                url: "TopFund.aspx/getSchemesListCompare",
                type: "POST",
                data: data2,
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $("#overlay").show();
                },

                success: function (data) {

                    var s = '<option value="-1">Select</option>';
                    var obj = JSON.parse(data.d);

                    if (obj != null) {
                        $('#NoSchemeAvailabelMsg').hide();
                        $('#ddlSchemesCompareBox').show();
                        $("#btnAddSchemeCompares").prop('disabled', false);
                        for (var i = 0; i < obj.length; i++) {
                            s += '<option value="' + obj[i].Scheme_Id + '">' + obj[i].Sch_Short_Name + '</option>';
                        }
                    } else {
                        $('#NoSelectSchemeMsg').hide();
                        $('#NoSchemeAvailabelMsg').show();
                        $('#ddlSchemesCompareBox').hide();
                        $("#btnAddSchemeCompares").prop('disabled', true);
                    }

                    $("#ddlSchemesCompare").html(s);
                },
                error: function (error) {
                    console.log(`Error ${error}`);
                },

                complete: function () {
                    $("#overlay").hide();
                }

            });
        }

        function BindSchemeListNav() {

            var data2 = JSON.stringify({
                ddlCategoryCompare: $('#ddlCategoryNav :selected').val(),
                //ddlTypeCompare: $('#ddlTypeNav :selected').val(),
                ddlSubCategoryCompare: $('#ddlSubCategoryNav :selected').val(),
                rdbOptionCompare: $('#ddlOptionNav :selected').val(),
                ddlFundHouseCompare: $('#ddlFundHouseNav :selected').val()
            });

            $.ajax({
                url: "TopFund.aspx/getSchemesListCompare",
                type: "POST",
                data: data2,
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $("#overlay").show();
                },
                success: function (data) {

                    $('ddlSchemesNav').empty();
                    var s = '<option value="-1">Select</option>';
                    var obj = JSON.parse(data.d);

                    if (obj != null) {
                        $('#NoSchemeAvailabelMsgNav').hide();
                        $('#ddlSchemesNavBox').show();
                        $("#btnAddSchemeNav").prop('disabled', false);
                        for (var i = 0; i < obj.length; i++) {
                            s += '<option value="' + obj[i].Scheme_Id + '">' + obj[i].Sch_Short_Name + '</option>';
                        }
                    } else {
                        $('#NoSelectSchemeMsgNav').hide();
                        $('#NoSchemeAvailabelMsgNav').show();
                        $('#ddlSchemesNavBox').hide();
                        $("#btnAddSchemeNav").prop('disabled', true);
                    }
                    $("#ddlSchemesNav").html(s);
                },
                error: function (error) {
                    console.log(`Error ${error}`);
                },
                complete: function () {
                    $("#overlay").hide();
                }
            });
        }

        $(document).ready(function () {

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
                $("#" + activeTabId).addClass("active");


            }

            $('#ddlCategory').on('change', function () {
                
                var data2 = JSON.stringify({
                    ddlCategory: $('#ddlCategory :selected').val(),

                });

                $.ajax({
                    url: "TopFund.aspx/LoadSubNature",
                    type: "POST",
                    data: data2,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $("#overlay").show();
                    },
                    success: function (data) {
                        
                        var s = '<option value="-1">All</option>';
                        var obj = JSON.parse(data.d);

                        for (var i = 0; i < obj.length; i++) {
                            s += '<option value="' + obj[i].Sebi_Sub_Nature_ID + '">' + obj[i].Sebi_Sub_Nature + '</option>';
                        }
                        $("#ddlSubCategory").html(s);
                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    },
                    complete: function () {
                        $("#overlay").hide();
                    }
                });
            });


            $('#ddlFundHouseCompare').on('change', function () {
                BindSchemeList();
            });
            $('#ddlCategoryCompare').on('change', function () {


                
                var data2 = JSON.stringify({
                    ddlCategoryCompare: $('#ddlCategoryCompare :selected').val(),

                });

                $.ajax({
                    url: "TopFund.aspx/LoadSubNatureCompare",
                    type: "POST",
                    data: data2,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $("#overlay").show();
                    },

                    success: function (data) {
                        
                        var s = '<option value="-1">All</option>';
                        var obj = JSON.parse(data.d);

                        for (var i = 0; i < obj.length; i++) {
                            s += '<option value="' + obj[i].Sebi_Sub_Nature_ID + '">' + obj[i].Sebi_Sub_Nature + '</option>';
                        }
                        $("#ddlSubCategoryCompare").html(s);
                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    },
                    complete: function () {
                        $("#overlay").hide();
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
            $('#ddlOptionCompare').on('change', function () {
                BindSchemeList();
            });

            $('#ddlFundHouseNav').on('change', function () {
                $("#ddlSchemesNav").prop('disabled', false);
                $('#AllSchemeSelectedMsgNav').hide();
                BindSchemeListNav();
            });
            $('#ddlCategoryNav').on('change', function () {


                
                var data2 = JSON.stringify({
                    ddlCategoryCompare: $('#ddlCategoryNav :selected').val(),

                });

                $.ajax({
                    url: "TopFund.aspx/LoadSubNatureCompare",
                    type: "POST",
                    data: data2,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $("#overlay").show();
                    },
                    success: function (data) {
                        
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
                    },
                    complete: function () {
                        $("#overlay").hide();
                    }
                });

                BindSchemeList();

            });
            //$('#ddlTypeNav').on('change', function () {
            //    BindSchemeListNav();
            //});
            $('#ddlSubCategoryNav').on('change', function () {
                $("#ddlSchemesNav").prop('disabled', false);
                $('#AllSchemeSelectedMsgNav').hide();
                BindSchemeListNav();
            });
            $('#ddlOptionNav').on('change', function () {
                $("#ddlSchemesNav").prop('disabled', false);
                $('#AllSchemeSelectedMsgNav').hide();
                BindSchemeListNav();
            });


            $('#btnAddSchemeCompares').click(function (e) {
                
                e.preventDefault();
                var SchName = "";
                var SchId = "";
                var IndName = "";
                var IndId = "";
                var BtnClickNo = $('#BtnClickedNo').val();

                if (BtnClickNo == "1") {

                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    //if ((SchId == "-1" || SchId == "0") && IndId == "0") {
                    //    alert("Please select valid Scheme name and Index name ");
                    //    return;
                    //} else
                    if (SchId == "-1" || SchId == "0") {
                        //alert("Please select valid Scheme name ");
                        $('#NoSelectSchemeMsg').show();
                        return;
                    } else {
                        $('#NoSelectSchemeMsg').hide();

                    }
                    //} else if (IndId == "0") {
                    //    alert("Please select valid Index name ");
                    //    return;
                    //}

                    $("#FirstAddSchemeH").html('');
                    $("#FirstAddSchemeH").append(SchName);

                    $("#CurrScheme1").val(SchName);
                    $("#CurrSchemeId1").val(SchId);
                    $("#CurrIndex1").val(IndName);
                    $("#CurrIndexId1").val(IndId);

                    $("#ddlSchemesCompare :selected").remove();    /*Delete the selected scheme in the dropdown list*/

                    $("#ddlSchemesCompare").val('-1');   /*select the default item */
                    $("#ddlIndicesCompare").val('0');



                    $("#FirstAddBtn").hide();
                    $("#FirstAddScheme").show();

                }
                else if (BtnClickNo == "2") {

                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    //if ((SchId == "-1" || SchId == "0") && IndId == "0") {
                    //    alert("Please select valid Scheme name and Index name ");
                    //    return;
                    //} else
                    if (SchId == "-1" || SchId == "0") {
                        $('#NoSelectSchemeMsg').show(); return;
                    } else {
                        $('#NoSelectSchemeMsg').hide();
                    }
                    //} else if (IndId == "0") {
                    //    alert("Please select valid Index name ");
                    //    return;
                    //}

                    $("#SecondAddSchemeH").html('');
                    $("#SecondAddSchemeH").append(SchName);

                    $("#CurrScheme2").val(SchName);
                    $("#CurrSchemeId2").val(SchId);
                    $("#CurrIndex2").val(IndName);
                    $("#CurrIndexId2").val(IndId);

                    $("#ddlSchemesCompare :selected").remove();    /*Delete the selected scheme in the dropdown list*/

                    $("#ddlSchemesCompare").val('-1');  /*select the default scheme */
                    $("#ddlIndicesCompare").val('0');



                    $("#SecondAddBtn").hide();
                    $("#SecondAddScheme").show();


                }
                else if (BtnClickNo == "3") {
                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    //if ((SchId == "-1" || SchId == "0") && IndId == "0") {
                    //    alert("Please select valid Scheme name and Index name ");
                    //    return;
                    //} else
                    if (SchId == "-1" || SchId == "0") {
                        $('#NoSelectSchemeMsg').show();
                        return;
                    } else {
                        $('#NoSelectSchemeMsg').hide();
                    }
                    //} else if (IndId == "0") {
                    //    alert("Please select valid Index name ");
                    //    return;
                    //}


                    $("#ThirdAddSchemeH").html('');
                    $("#ThirdAddSchemeH").append(SchName);

                    $("#CurrScheme3").val(SchName);
                    $("#CurrSchemeId3").val(SchId);
                    $("#CurrIndex3").val(IndName);
                    $("#CurrIndexId3").val(IndId);

                    $("#ddlSchemesCompare :selected").remove();    /*Delete the selected scheme in the dropdown list*/

                    $("#ddlSchemesCompare").val('-1');  /*select the default scheme*/
                    $("#ddlIndicesCompare").val('0');

                    //$("#ddlSchemesCompare :selected").remove();

                    $("#ThirdAddBtn").hide();
                    $("#ThirdAddScheme").show();

                }
                else {
                    SchName = $('#ddlSchemesCompare :selected').text();
                    SchId = $('#ddlSchemesCompare :selected').val();
                    IndName = $('#ddlIndicesCompare :selected').text();
                    IndId = $('#ddlIndicesCompare :selected').val();

                    //if ((SchId == "-1" || SchId == "0") && IndId == "0") {
                    //    alert("Please select valid Scheme name and Index name ");
                    //    return;
                    //} else
                    if (SchId == "-1" || SchId == "0") {
                        $('#NoSelectSchemeMsg').show();
                        return;
                    } else {
                        $('#NoSelectSchemeMsg').hide();
                    }
                    //} else if (IndId == "0") {
                    //    alert("Please select valid Index name ");
                    //    return;
                    //}

                    $("#FourthAddSchemeH").html('');
                    $("#FourthAddSchemeH").append(SchName);

                    $("#CurrScheme4").val(SchName);
                    $("#CurrSchemeId4").val(SchId);
                    $("#CurrIndex4").val(IndName);
                    $("#CurrIndexId4").val(IndId);

                    $("#ddlSchemesCompare :selected").remove();    /*Delete the selected scheme in the dropdown list*/

                    $("#ddlSchemesCompare").val('-1'); /*select the default scheme*/
                    $("#ddlIndicesCompare").val('0');
                    
                    //$("#ddlSchemesCompare :selected").remove();

                    $("#FourthAddBtn").hide();
                    $("#FourthAddScheme").show();
                }

                $("#ddlFundHouseCompare").val('0');
                $("#ddlCategoryCompare").val('-1');
                $("#ddlSubCategoryCompare").val('-1');
                $("#ddlTypeCompare").val('-1');
                $("#ddlOptionCompare").val('2');
                $('#btnAddSchemeCompares').removeAttr('data-bs-toggle');


            });

            $('#btnCompareFundReset').click(function (e) {

                $("#CurrScheme1").val("");
                $("#CurrSchemeId1").val("");
                $("#CurrIndex1").val("");
                $("#CurrIndexId1").val("");
                $("#FirstAddBtn").show();
                $("#FirstAddScheme").hide();

                $("#CurrScheme2").val("");
                $("#CurrSchemeId2").val("");
                $("#CurrIndex2").val("");
                $("#CurrIndexId2").val("");
                $("#SecondAddBtn").show();
                $("#SecondAddScheme").hide();

                $("#CurrScheme3").val("");
                $("#CurrSchemeId3").val("");
                $("#CurrIndex3").val("");
                $("#CurrIndexId3").val("");
                $("#ThirdAddBtn").show();
                $("#ThirdAddScheme").hide();

                $("#CurrScheme4").val("");
                $("#CurrSchemeId4").val("");
                $("#CurrIndex4").val("");
                $("#CurrIndexId4").val("");
                $("#FourthAddBtn").show();
                $("#FourthAddScheme").hide();

                $("#ddlFundHouseCompare").val('0');
                $("#ddlCategoryCompare").val('-1');
                $("#ddlSubCategoryCompare").val('-1');
                $("#ddlTypeCompare").val('-1');
                $("#ddlSchemesCompare").val('-1'); /*select the default scheme*/
                $("#ddlIndicesCompare").val('0');
                $("#ddlOptionCompare").val('2');

                $("#DivShowPerformanceCard").hide();

            });


            $('#btnCompareFund').click(function (e) {

                e.preventDefault();

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

                //var value = "#s" + $("#CurrSchemeId1").val() + "#s" + $("#CurrSchemeId2").val() + "#s" + $("#CurrSchemeId3").val() + "#s" + $("#CurrSchemeId4").val() + "#" +
                //    "#i" + $("#CurrIndexId1").val() + "#i" + $("#CurrIndexId2").val() + "#i" + $("#CurrIndexId3").val() + "#i" + $("#CurrIndexId4").val();

                var value = "";

                //if ($("#CurrSchemeId1").val() == "" && $("#CurrSchemeId2").val() == "") {
                //    alert("Please select minimum two scheme");
                //    return;
                //}


                if ($("#CurrSchemeId1").val() != "") {
                    value += "#s" + $("#CurrSchemeId1").val() + "#i" + $("#CurrIndexId1").val();
                }
                if ($("#CurrSchemeId2").val() != "") {
                    value += "#s" + $("#CurrSchemeId2").val() + "#i" + $("#CurrIndexId2").val();
                }
                if ($("#CurrSchemeId3").val() != "") {
                    value += "#s" + $("#CurrSchemeId3").val() + "#i" + $("#CurrIndexId3").val();
                }
                if ($("#CurrSchemeId4").val() != "") {
                    value += "#s" + $("#CurrSchemeId4").val() + "#i" + $("#CurrIndexId4").val();
                }

                $("#HdSchemes").val(value);

                //$("#HdSchemes").val = $("#HdSchemes").val.TrimEnd('i');
                //$("#HdSchemes").val = $("#HdSchemes").val.TrimEnd('s');
                //$("#HdSchemes").val = $("#HdSchemes").val.TrimEnd('#');


                $("#HdToData").val(moment().format('DD MMM, yyyy'));
                //$("#HdFromData").val = DateTime.Today.AddYears(-3).ToString("dd MMM yyyy");
                $("#HdFromData").val(moment().subtract(-3, 'years').format('DD MMM, yyyy'));

                 var host = $("#HostAuthority").val();

                $.ajax({
                    url: "TopFund.aspx/PopulateSchemeIndexCompareFund",
                    type: "POST",
                    data: dataToSend,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $("#overlay").show();
                    },
                    success: function (data) {
                        
                        var obj = JSON.parse(data.d);

                        var table = '<div class="table-responsive"><table id=tblResult  class="table align-middle table - nowrap table - centered mb - 0"><thead class="table-light"><tr> <th  class="fw-bold">Scheme Name</th> <th  class="fw-bold">1 months</th> <th class="fw-bold">6 months</th> <th class="fw-bold">1Yr</th> <th class="fw-bold">3 Yrs</th> <th class="fw-bold">Since Inception</th> <th class="fw-bold">Fund size (Cr)</th> </tr></thead><tbody>';
                        for (var i = 0; i < obj.length; i++) {

                            for (const [key, value] of Object.entries(obj[i])) {

                                if (value == null) {
                                    obj[i][key] = "--";
                                }
                            }

                            var row = "<tr>";

                            if (obj[i].Sch_id == "-1") {
                                row += '<td>' + obj[i].Sch_Short_Name + '</td>';
                            } else {
                                row += '<td><a href="https://askmefund.com/factsheet/' + obj[i].Sch_id + '" target="_blank" class="text-dark"> ' + obj[i].Sch_Short_Name + '</a></td>';
                            }
                            


                            row +='<td style="text-right">' + obj[i].Per_30_Days + '</td><td style="text-right">' + obj[i].Per_182_Days + '</td><td style="text-right">' + obj[i].Per_1_Year + '</td><td style="text-right">' + obj[i].Per_3_Year + '</td><td style="text-right">' + obj[i].Per_Since_Inception + '</td><td style="text-right">' + obj[i].Fund_Size + '</td>';
                            //row += "<td>" + obj[i].CurrentNav + "</td><td>" + obj[i].LblIncrNav + "</td><td>" + obj[i].Nature + "</td><td>" + obj[i].Nav_Rs + "</td><td>" + obj[i].Option_Id + "</td><td>" + obj[i].Per_1_Year + "</td><td>" + obj[i].Per_3_Year + "</td><td>" + obj[i].Per_30_Days + "</td><td>" + obj[i].Per_91_Days + "</td><td>" + obj[i].Per_182_Days + "</td><td>" + obj[i].Per_Since_Inception + "</td><td>" + obj[i].PrevNav + "</td><td>" + obj[i].Sch_Short_Name + "</td><td>" + obj[i].sch_id + "</td><td>" + obj[i].Structure_Name + "</td><td>" + obj[i].Sub_Nature + "</td><td>" + obj[i].status + "</td>";
                            //row += "<td>" + response.d[i].Salary + "</td>";
                            row += '</tr>';
                            table += row;
                        }

                        table += '</tbody></table></div>';
                        //$('#GrdCompFund').append(table);
                        $("#GrdCompFund").html(table);
                        $("#DivShowPerformanceCard").show();
                        $("#DivShowPerformance").show();
                        $("#lbRetrnMsg").show();
                        $("#lblSortPeriod").show();
                        $("#HighContainer").show();
                        btnPlotclick();


                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    },
                    complete: function () {
                        $("#overlay").hide();
                    }
                });



            });


            $('#DltBtnFirstSchm').click(function () {
                
                $("#CurrScheme1").val("");
                $("#CurrSchemeId1").val("");
                $("#CurrIndex1").val("");
                $("#CurrIndexId1").val("");
                $("#FirstAddBtn").show();
                $("#FirstAddScheme").hide();
            });

            $('#DltBtnSecondSchm').click(function () {
                
                $("#CurrScheme2").val("");
                $("#CurrSchemeId2").val("");
                $("#CurrIndex2").val("");
                $("#CurrIndexId2").val("");
                $("#SecondAddBtn").show();
                $("#SecondAddScheme").hide();
            });

            $('#DltBtnThirdSchm').click(function () {
                
                $("#CurrScheme3").val("");
                $("#CurrSchemeId3").val("");
                $("#CurrIndex3").val("");
                $("#CurrIndexId3").val("");
                $("#ThirdAddBtn").show();
                $("#ThirdAddScheme").hide();
            });

            $('#DltBtnFourthSchm').click(function () {
                
                $("#CurrScheme4").val("");
                $("#CurrSchemeId4").val("");
                $("#CurrIndex4").val("");
                $("#CurrIndexId4").val("");
                $("#FourthAddBtn").show();
                $("#FourthAddScheme").hide();
            });


            //btnPlotclick();
        });

        function btnPlotclick() {

            
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
            var val = '{"schIndexid":"' + schIndId + '", "startDate":"' + frmDate + '", "endDate":"' + toData + '"}';
            $.ajax({
                type: "POST",
                url: "TopFund.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                beforeSend: function () {
                    $("#overlay").show();
                },

                success: function (msg) {
                    // setChart(msg.d);

                    PlotAuto(msg.d, ImageArray);
                },
                error: function (msg) {
                    console.log(msg);
                    alert("Please select at least one scheme.");
                },
                complete: function () {
                    $("#overlay").hide();
                }
            });
        }

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
                    points.push([Date.UTC(res[0], res[1] - 1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue, data[i].ValueAndDate[j].IsIndex]);

                }
                tt1.data = points;
                tt.push(tt1);
            }
            tt.shift();
            
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

                                    s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>: <b>' + (tt[i].data[k][3] != null ? "N/A" : tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b><br />';
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

            var flag = false;
            var myArray = schIndId.split("#");
            for (var i = 0; i < myArray.length; i++) {
                if (myArray[i].includes("s")) {
                    flag = true;
                }
            }

            if (!flag) {
                alert("Please select scheme ");
                return;
            }


            var ImageArray = Array();
            var data = {};
            data.minDate = frmDate;
            data.maxDate = toData;
            data.schemeIndexIds = schIndId;
            var val = '{"schIndexid":"' + schIndId + '", "startDate":"' + frmDate + '", "endDate":"' + toData + '"}';
            $.ajax({
                type: "POST",
                url: "TopFund.aspx/getChartData",
                async: false,
                contentType: "application/json",
                data: val,
                dataType: "json",
                beforeSend: function () {
                    $("#overlay").show();
                },

                success: function (msg) {
                    // setChart(msg.d);
                    PlotAutoNav(msg.d, ImageArray);
                    
                },
                error: function (msg) {
                    console.log(msg);
                    alert("Please select at least one scheme.");
                },
                complete: function () {
                    $("#overlay").hide();
                }
            });
        }

        function PlotAutoNav(dataConsolidated, ImageArray) {
            
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
                    points.push([Date.UTC(res[0], res[1] - 1, res[2]), data[i].ValueAndDate[j].Value, data[i].ValueAndDate[j].OrginalValue, data[i].ValueAndDate[j].IsIndex]);

                }
                tt1.data = points;
                tt.push(tt1);
            }
            tt.shift();
            
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
                        var navdate = days[d.getDay()] + ' ,' + months[d.getMonth()] + ' ' + d.getDate() + ' ,' + d.getFullYear();

                        s = s + '<span style="color:#839557">' + navdate + '</span><br /><br />';

                        for (var i = 0; i < tt.length; i++) {
                            for (var k = 0; k < tt[i].data.length; k++) {
                                if (this.x === tt[i].data[k][0]) {

                                    s = s + '<span style="color:#c5b47f">' + tt[i].name + '</span>: <b>' + (tt[i].data[k][3] != null ? "N/A" : tt[i].data[k][2].toString() == -1 ? "N/A" : tt[i].data[k][2].toString()) + '</b><br />';
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
            
            var p = o.parentNode.parentNode;
            var id = p.childNodes[0].innerText;
            var seletedSchemeId = $("#hidSchindSelected").val();
            var newSchme = "";

            var myArray = seletedSchemeId.split("#");
            for (var i = 0; i < myArray.length; i++) {
                if (!myArray[i].includes(id)) {
                    if (myArray[i] != '')
                        newSchme += "#" + myArray[i];
                }
            }

            $("#hidSchindSelected").val(newSchme);


            p.parentNode.removeChild(p);

            var oTable = document.getElementById('tblResultNav');
            var rowLength = oTable.rows.length;
            if (rowLength < 2) {
                //$("#tblResultNav").hide();
                //$("#btnPlotChart").hide();
                $("#DivGridContainCard").hide();
                $("#DivGridContain").hide();
                $("#HighContainerNav").hide();
            }
        }
        $(document).ready(function () {
            var tmp = "";
            $("#btnAddSchemeNav").click(function () {
                
                var SchName = $('#ddlSchemesNav :selected').text();
                var SchId = $('#ddlSchemesNav :selected').val();
                var IndName = $('#ddlIndicesNav :selected').text();
                var IndId = $('#ddlIndicesNav :selected').val();

                //if ((SchId == "-1" || SchId == "0") && IndId == "0") {
                //    alert("Please select valid Scheme name and Index name ");
                //    return;
                //} else
                if (SchId == "-1" || SchId == "0") {
                    //alert("Please select valid Scheme name");
                    $('#NoSelectSchemeMsgNav').show();
                    return;
                } else {
                    $('#NoSelectSchemeMsgNav').hide();
                }
                //} else if (IndId == "0") {
                //    alert("Please select Index name ");
                //    return;
                //}

                var dataToSend = JSON.stringify({
                    SchName: SchName,
                    SchId: SchId,
                    IndName: IndName,
                    IndId: IndId


                });

                tmp = $("#hidSchindSelected").val();

                var value = "#s" + SchId + "#" + "#i" + IndId;
                tmp += value;
                $("#hidSchindSelected").val(tmp);

                var host = $("#HostAuthority").val();

                //$("#HdSchemes").val(value);
                $("#HdToDataNav").val(moment().format('DD MMM, yyyy'));
                $("#HdFromDataNav").val(moment().add(-3, 'y').format('DD MMM, yyyy'));
                $.ajax({
                    url: "TopFund.aspx/btnAddSchemeNav",
                    type: "POST",
                    data: dataToSend,
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $("#overlay").show();
                    },
                    success: function (data) {
                        
                        var obj = JSON.parse(data.d);
                        var flag = false;
                        var indval;
                        var oTable = document.getElementById('tblResultNav');
                        var rowLength = oTable.rows.length;
                        var row = "";
                        for (var i = 0; i < obj.length; i++) {

                            if (rowLength > 0) {

                                if (obj[i].Sch_Short_Name != "") {
                                    row += '<tr><td style="display:none">' + obj[i].SCHEME_ID + '</td><td><a href="https://askmefund.com/factsheet/' + obj[i].SCHEME_ID + '" target="_blank" class="text-dark"> ' + obj[i].Sch_Short_Name + '</a></td>';
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                    row += '<tr><td style="display:none">' + obj[i].INDEX_ID + '</td><td>' + obj[i].INDEX_NAME + '</td>';
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';

                                    indval = "#i" + obj[i].INDEX_ID;
                                    tmp += indval;
                                    $("#hidSchindSelected").val(tmp);
                                } else {
                                    row += '<tr><td style="display:none">' + obj[i].INDEX_ID + '</td><td>' + obj[i].INDEX_NAME + '</td>';
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                }
                                flag = true;


                            } else {
                                var table = '<thead class="table-light"><tr> <th>Scheme Name</th> <th>Delete</th>  </tr></thead><tbody>';

                                if (obj[i].Sch_Short_Name != "") {
                                    row += '<tr><td style="display:none">' + obj[i].SCHEME_ID + '</td><td><a href="https://askmefund.com/factsheet/' + obj[i].SCHEME_ID + '" target="_blank" class="text-dark"> ' + obj[i].Sch_Short_Name + '</a></td>';
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                    row += '<tr><td style="display:none">' + obj[i].INDEX_ID + '</td><td>' + obj[i].INDEX_NAME + '</td>';
                                    row += '<td><button type="button" style="border:none; background: none;" value="Delete Row" onclick="SomeDeleteRowFunction(this)"><img src="images/close.png" /></button></td></tr>';
                                    indval = "#i" + obj[i].INDEX_ID;
                                    tmp += indval;
                                    $("#hidSchindSelected").val(tmp);
                                } else {
                                    row += '<tr><td style="display:none">' + obj[i].INDEX_ID + '</td><td>' + obj[i].INDEX_NAME + '</td>';
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

                        $("#DivGridContainCard").show();
                        $("#DivGridContain").show();
                        $("#btnPlotChart").show();
                        $("#tblResultNav").show();

                        //btnPlotclick();

                        $("#ddlSchemesNav :selected").remove();  /*delete the seleted scheme in dropdown list*/


                        $("#ddlSchemesNav").val('-1');  /*select the default scheme*/
                        $("#ddlIndicesNav").val('0');

                        var optionRemaining = $("#ddlSchemesNav").find('option:not(:selected)').length;
                        if (optionRemaining == 0) {

                            //$("#ddlSchemesNavBox").hide();
                            $("#AllSchemeSelectedMsgNav").show();
                            $("#ddlSchemesNav").prop('disabled', true);
                            $("#btnAddSchemeNav").prop('disabled', true);

                        } else {
                            //$("#ddlSchemesNavBox").show();
                            $("#AllSchemeSelectedMsgNav").hide();
                            $("#ddlSchemesNav").prop('disabled', false);
                            $("#btnAddSchemeNav").prop('disabled', false);
                        }

                    },
                    error: function (error) {
                        console.log(`Error ${error}`);
                    },
                    complete: function () {
                        $("#overlay").hide();
                    }
                });



            });

            

            


            $("#btnPlotChart").click(function () {
                btnPlotclickNav();
            });
        });

    </script>

    <script>

        function getSebiSubNature(natureId) {

            //debugger

            var dataToSend = JSON.stringify({
                SebiNatureId: natureId.toString()

            });

            $.ajax({
                type: "POST",
                url: "TopFund.aspx/LoadSubNatureTopFundListTab",
                async: false,
                contentType: "application/json",
                data: dataToSend,
                dataType: "json",
                beforeSend: function () {
                    $("#overlay").show();
                },
                success: function (data) {
                    
                    var btn = "";
                    var obj = JSON.parse(data.d);

                    for (var i = 0; i < obj.length; i++) {
                        btn += '<button type = button class="btn btn-primary btn-pin"  onclick="FnFetchTopfundData(' + obj[i].Sebi_Sub_Nature_ID.toString() + ', ' + natureId + ')"> ' + obj[i].Sebi_Sub_Nature + '</button> ';

                    }

                    $("#ChildListTopFund").empty();
                    $("#ChildListTopFund").append(btn);

                },
                error: function (msg) {
                    alert("An Error Occured.");
                },
                complete: function () {
                    $("#overlay").hide();
                }
            });
        }

        function FnFetchTopfundData(subNatureId, natureId) {

            var _natureId = natureId;

            if (subNatureId == 41 || subNatureId == 38) {
                _natureId = 6;
            }


            var dataToSend = JSON.stringify({
                SebiNatureId: _natureId.toString(),
                SebiSubNatureId: subNatureId.toString()

            });

            //alert("subnatureID = " + subNatureId + "     natureId = " + natureId);

            $.ajax({
                type: "POST",
                url: "TopFund.aspx/getTopFundData",
                async: false,
                contentType: "application/json",
                data: dataToSend,
                dataType: "json",
                beforeSend: function () {
                    $("#overlay").show();
                },

                success: function (msg) {
                    
                    var RtnData = JSON.parse(msg.d);
                    if (RtnData.length > 0)
                        FNSetData(RtnData);
                    else {

                        var card = '<div class="card"><div class="card-body pb-xl-2">';

                        card += '<h4 class="font-size-17 mb-1">No data found</h4>';

                        card += '</div></div>';

                        $("#ResultTopFund").html(card);
                    }

                },
                error: function (msg) {
                    alert("An Error Occured.");
                },
                complete: function () {
                    $("#overlay").hide();
                }
            });
        }

        function FNSetData(data) {

            var card = "";
            var host = $("#HostAuthority").val();

            for (i = 0; i < data.length; i++) {

                for (const [key, value] of Object.entries(data[i])) {

                    if (value == null || value == "0") {
                        data[i][key] = "--";
                    }
                }

                card += '<div class="card"><div class="card-body pb-xl-2"><h4 class="font-size-17 mb-1"><a href="https://askmefund.com/factsheet/' + data[i].Scheme_Id + '" target="_blank" class="text-dark"> ' + data[i].Sch_Name + '</a></h4>';
                card += '<div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12"><div class="row"><div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-xs-12"><div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12"><p>' + data[i].Curr_Nav + '<span class="';

                if (data[i].Curr_Nav - data[i].Prev_Nav > 0) {
                    card += 'mdi mdi-arrow-up-bold text-success';
                } else {
                    card += 'mdi mdi-arrow-down-bold text-success';
                }

                card += '"></span>' + data[i].Incr_Nav + '</p></div>';
                card += '<div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12"><div class="row"><div class="col"><div><h6 style="margin-bottom: 0px;">' + data[i].LblHigh + '<small class="text-muted"> (' + data[i].SpnMaxDate + ')</small></h6><p class="text-muted mb-0"><span class="badge  badge-soft-success font-size-12">High Nav</span></p></div></div>';
                card += '<div class="col"><div><h6 style="margin-bottom: 0px;">' + data[i].LblLow + '<small class="text-muted"> (' + data[i].spnMinDate + ')</small></h6><p class="text-muted mb-0"><span class="badge  badge-soft-danger  font-size-12">Low Nav</span></p></div></div></div></div></div>';
                card += '<div class="mt-2 col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xs-12"><div class="row"><div class="col border-end"><div class="text-center"><p class="text-muted mb-0">6 months</p><h6>' + data[i].Per_182_Days + '</h6></div></div>';
                card += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">1 year</p><h6>' + data[i].Per_1_Year + '</h6></div></div>';
                card += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">3 years</p><h6>' + data[i].Per_3_Year + '</h6></div></div>';
                card += '<div class="col border-end"><div class="text-center"><p class="text-muted mb-0">Since inception</p><h6>' + data[i].Since_Inception + '</h6></div></div>';
                card += '<div class="col"><div class="text-center"><p class="text-muted mb-0">Fund size (Cr)</p><h6>' + data[i].Fund_Size + '</h6></div></div></div>';
                card += '</div></div>';
                card += '<div class="row"><div class="col-sm-12 mt-2" style="text-align: right; padding: 0px;"><a href="https://askmefund.com/factsheet/' + data[i].Scheme_Id + '" target="_blank" class="text-primary fw-semibold"><u>More details</u><i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>';
                card += '</div></div></div></div></div>';

            }
            $("#ResultTopFund").html(card);
        }



        $(document).ready(function () {
            
            getSebiSubNature(4);
            FnFetchTopfundData(25, 4);

            $("#Equity").click(function () {
                getSebiSubNature(4);
                FnFetchTopfundData(25, 4);
            });

            $("#Debt").click(function () {
                getSebiSubNature(3);
                FnFetchTopfundData(36, 3);
            });

            $("#Hybrid").click(function () {
                getSebiSubNature(1);
                FnFetchTopfundData(6, 1);
            });

            //$("#SolutionOriented").click(function () {
            //    getSebiSubNature(6);
            //    FnFetchTopfundData(41, 6);
            //});

            $("#Other").click(function () {
                getSebiSubNature(5);
                FnFetchTopfundData(48, 5);
            });




        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="TabName" runat="server" Value="TopFund" />
        <asp:HiddenField ID="HostAuthority" runat="server" />
        <asp:HiddenField ID="PageValue" runat="server" />
        <!-- Begin page -->
        <div id="layout-wrapper">
            <div class="main-content">
                <div class="page-content">
                    <div class="container-fluid">
                        <div class="row">
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <%--<div class="card">--%>
                                    <%--<div class="card-header">
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
                                    </div>--%>
                                    <!-- end card header -->

                                    <%--<div class="card-body">--%>
                                        <!-- Tab panes -->
                                        <div class="tab-content text-muted">
                                            <div class="tab-pane active" id="TopFund" role="tabpanel" style="display:none">
                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="card">
                                                        <div class="card-body" style="padding: 0;">
                                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="card">
                                                                    <div class="card-header align-items-center d-flex">

                                                                        <div class="flex-shrink-0">
                                                                            <ul class="nav justify-content-end nav-tabs-custom rounded card-header-tabs" role="tablist">
                                                                                <li class="nav-item">
                                                                                    <a class="nav-link active" data-bs-toggle="tab" id="Equity" role="tab">
                                                                                        <span class="d-block d-sm-none"> Equity   </span>
                                                                                        <span class="d-none d-sm-block">Equity</span>
                                                                                    </a>
                                                                                </li>
                                                                                <li class="nav-item">
                                                                                    <a class="nav-link" data-bs-toggle="tab" id="Debt" role="tab">
                                                                                        <span class="d-block d-sm-none">Debt</span>
                                                                                        <span class="d-none d-sm-block">Debt</span>
                                                                                    </a>
                                                                                </li>
                                                                                <li class="nav-item">
                                                                                    <a class="nav-link" data-bs-toggle="tab" id="Hybrid" role="tab">
                                                                                        <span class="d-block d-sm-none">Hybrid</span>
                                                                                        <span class="d-none d-sm-block">Hybrid</span>
                                                                                    </a>
                                                                                </li>
                                                                                <%--<li class="nav-item">
                                                                                    <a class="nav-link" data-bs-toggle="tab" id="SolutionOriented" role="tab">
                                                                                        <span class="d-block d-sm-none">Solution Oriented</span>
                                                                                        <span class="d-none d-sm-block">Solution Oriented</span>
                                                                                    </a>
                                                                                </li>--%>
                                                                                <li class="nav-item">
                                                                                    <a class="nav-link" data-bs-toggle="tab" id="Other" role="tab">
                                                                                        <span class="d-block d-sm-none">Other</span>
                                                                                        <span class="d-none d-sm-block">Other</span>
                                                                                    </a>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <!-- Nav tabs -->
                                                            <div class="container">
                                                                <div class="row">
                                                                    <div class="col-lg-12 no-pad scroller" style="margin-bottom: -20px;">
                                                                        <div class="left-btn-scroller left-scroll ">
                                                                            <i class="fas fa-chevron-left"></i>
                                                                        </div>
                                                                        <div class="right-btn-scroller right-scroll ">
                                                                            <i class="fas fa-chevron-right"></i>
                                                                        </div>
                                                                        <div class="col-lg-12 no-pad btn-header-links" style=" margin-top: 10px;" id="scroll-div">
                                                                            <%--<asp:Repeater ID="RepeaterEquity" runat="server">
                                                                                <ItemTemplate>
                                                                                    <button type="button" class="btn btn-warning btn-pin btn-bg10 " onclick="FnFetchTopfundData('<%#Eval("Sebi_Sub_Nature_ID").ToString()%>','4')"
                                                                                        role="presentation" <%# Container.ItemIndex == 0 ? " class='active'" : ""%>>
                                                                                        <a><%#Eval("Sebi_Sub_Nature")%></a></button>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>--%>
                                                                            <div id="ChildListTopFund">
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <!-- Tab panes -->
                                                            <div class="tab-content p-3 text-muted" id="ResultTopFund">
                                                            </div>
                                                            <div style="font-size: 12px; padding-left: 10px; padding-bottom: 10px;">Note : Returns less than or equal to 1 year are absolute return and returns more than 1 year are compound annualized.</div>
                                                        </div>
                                                        <div class="value_btm_text" style="font-size: 10px; color: #A7A7A7; text-align: right; padding-right: 10px">
                                                            Developed for Askmefund by: <a href="https://www.icraanalytics.com" target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer </a>
                                                        </div>
                                                        <!-- end card-body -->
                                                    </div>
                                                    <!-- end card -->
                                                </div>
                                                <div class="tab-pane active" id="homeTopFund" role="tabpanel">
                                                </div>

                                            </div>
                                            <div class="tab-pane" id="SearchFunds" role="tabpanel" style="display:none">
                                                <div class="card">
                                                    <div class="card-body">
    <form class="needs-validation" novalidate="novalidate">
        <div class="row">
            <div class="col-md-4">
                <div class="mb-3 position-relative">
                    <label class="form-label"
                        for="validationTooltip01">
                        Category</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"></asp:DropDownList>
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
                    <asp:RadioButtonList ID="rdbOption" runat="server" class="radio" RepeatDirection="Horizontal" Style="margin-left: 5px; padding-top: 5px;" CssClass="" Width="100%"></asp:RadioButtonList>
                </div>

                <div class="invalid-tooltip">
                    Please provide a valid city.
                </div>
            </div>

            <div class="col-md-4">
                <div class="mb-3 position-relative">
                    <label class="form-label"
                        for="validationTooltip02">
                        Period</label>
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-select">
                        <%--<asp:ListItem Value="Per_7_Days" Text="Last 1 Week" />
                        <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />--%>
                        <asp:ListItem Value="Per_91_Days" Text="Last 3 Months" />
                        <asp:ListItem Value="Per_182_Days" Text="Last 6 Months" />
                        <asp:ListItem Value="Per_1_Year" Text="Last 12 Months" Selected="True" />
                        <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                        <%--<asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />--%>
                    </asp:DropDownList>
                    <asp:HiddenField ID="HiddenPeriod" runat="server" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="mb-3 position-relative">
                    <label class="form-label" for="validationTooltip02">
                        Fund Risk</label>
                    <div>
                        <div class="btn-group btn-group-example" role="group">

                            <button type="button" id="low" runat="server" title="Low Riskometer" data-risk="1,6" class="btn btn-success-soft">Low </button>
                            <button type="button" id="mod_low" runat="server" title="Moderately Low Riskometer" data-risk="2" class="btn btn-success w-xs">Mid low </button>
                            <button type="button" id="mod" runat="server" title="Moderate Riskometer" data-risk="3" class="btn btn-info w-xs">Mod </button>
                            <button type="button" id="mod_high" runat="server" title="Moderately High Riskometer" data-risk="4" class="btn btn-warning w-xs">Mod High </button>
                            <button type="button" id="high" runat="server" title="High Riskometer" data-risk="5,10" class="btn btn-danger w-xs">High</button>
                            <button type="button" id="all" runat="server" title="All" class="btn btn-primary risk-active w-xs">All</button>
                        </div>
                        <asp:HiddenField ID="HiddenFundRisk" runat="server" Value="-1" />
                        <asp:HiddenField ID="HiddenFundRiskStrColor" runat="server" Value="All" />
                    </div>
                </div>
                <%-- <div class="mb-3 position-relative">
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
                </div>--%>
            </div>
        </div>

        <div class="row"> 
            <div class="col-md-4">
                <div class="mb-3 position-relative">
                    <label class="form-label"
                        for="validationTooltip03">
                        Minimum SI Return(%)
                    </label>
                    <div class="row">
                        <div class="col-md-12 mt-3" style="padding-left: 10px;">
                            <div class="form-check" style="padding-left: 0px;">
                                <form class="form-horizontal form-pricing" role="form">
                                    <div class="price-slider">
                                        <div class="col-sm-12">
                                            <div id="slider1"></div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="">
                            <div>
                                <span style="color: #989898;" class="slider-min">5</span>
                                <span style="color: #989898;" class="slider-max">50</span>
                            </div>
                        </div>
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
                    <label class="form-label"
                        for="validationTooltip03">
                        Maximum Fund Size (Cr) 
                    </label>
                    <div class="row">
                        <div class="col-md-12 mt-3" style="padding-left: 10px;">
                            <div class="form-check" style="padding-left: 0px;">

                                <%--<div class="slidecontainer">
                                 <input id="sliderMin" class="slider" type="range" min="500" max="10000" step="500"/>
                            </div>--%>

                                <form class="form-horizontal form-pricing" role="form">
                                    <div class="price-slider">
                                        <div class="col-sm-12">
                                            <div id="slider"></div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="">
                            <div>
                                <span style="color: #989898;" class="slider-min">&#8377; 500</span>
                                <span style="color: #989898;" class="slider-max">&#8377; 50,000</span>
                            </div>
                           
                        </div>
                    </div>
                    <div class="invalid-tooltip">
                        Please provide a valid city.
                    </div>
                </div>
                <asp:HiddenField ID="HiddenMinimumInvesment" runat="server" Value="500" />
            </div>
            <div class="col-md-4">
            </div>
        </div>
        <div class="col-sm-12 mt-2" style="text-align: right; padding: 0px;"> 
            <%--<a href="#" class="btn btn-primary">Search</a>--%>
            <button id="btnSubmit" class="btn btn-dark" type="button" style="margin-right: 10px">Search</button>
        </div>
    </form>
</div>
                                                </div>
                                                

                                                <div id="Result" runat="server">
                                                    <div class="muted pull-left" style="color: #cc0000; font-weight: 700;">
                                                        <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                                                        <div class="pull-right"></div>
                                                    </div>
                                                    <%--<asp:Repeater ID="lstResult" runat="server" OnItemDataBound="lstResult_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="card">
                                                                <div class="card-body pb-xl-2">
                                                                    <h4 class="font-size-17 mb-1">
                                                                        <a class="text-dark" href="http://localhost:20801/AskMeFund/Factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank"><%#Eval("Sch_Name")%></a>
                                                                        <asp:HiddenField runat="server" ID="hdfSchemeId" Value='<%# Eval("SchemeId") %>' />
                                                                    </h4>
                                                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                                                        <div class="row">
                                                                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <p>
                                                                                        <asp:Label ID="lblPresentNav" runat="server"><%#Eval("CurrentNav")%></asp:Label>
                                                                                        <asp:Label ID="ImgArrow" runat="server"
                                                                                            CssClass="mdi text-success"></asp:Label>
                                                                                        <asp:Label runat="server" ID="lblIncrNav"><%#Eval("LblIncrNav")%></asp:Label>
                                                                                        <asp:HiddenField runat="server" ID="CurrNav" Value='<%# Eval("CurrentNav") %>' />
                                                                                        <asp:HiddenField runat="server" ID="PrevNav" Value='<%# Eval("PrevNav") %>' />
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                    <div class="row">
                                                                                        <div class="col">
                                                                                            <div>
                                                                                                <h6 style="margin-bottom: 0px;">
                                                                                                    <asp:Label runat="server" ID="lblHigh"><%#Eval("LblHigh")%></asp:Label>
                                                                                                    <small class="text-muted">(<asp:Label ID="spnMaxDate" runat="server"><%#Eval("SpnMaxDate")%></asp:Label>)</small>
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
                                                                                                <h6 style="margin-bottom: 0px;">
                                                                                                    <asp:Label runat="server" ID="lblLow"><%#Eval("LblLow")%></asp:Label><small class="text-muted">
                                                                                                        (<asp:Label ID="spnMinDate" runat="server"><%#Eval("spnMinDate")%></asp:Label>)</small>
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

                                                                            <div class="col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xs-12">
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
                                                                                </div>--%>
                                                    <%--<div class="row">
                                                                                    <div class="col-sm-2 mt-2">
                                                                                        <p class="mt-4 mb-0" style="text-align: right;">
                                                                                            18.09%
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-xl-8 mt-2">
                                                                                        <div class="row align-items-center g-0">
                                                                                            <div class="col-sm-12 mt-2">
                                                                                                <p class="text-muted mb-0">
                                                                                                    Average Category
                                                                                                        Returns(3 yrs)
                                                                                                </p>
                                                                                            </div>

                                                                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                                <div class="progress mt-1"
                                                                                                    style="height: 6px;">
                                                                                                    <asp:Label class="progress-bar progress-bar bg-primary" runat="server" ID="lblprgbar"
                                                                                                        role="progressbar"
                                                                                                        Style="width: 50%"
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
                                                                                </div>--%>
                                                    <%--</div>

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
                                                    </asp:Repeater>--%>
                                                </div>
                                                <table id="SearchResult" class="paginated" style="width: 100%">
                                                </table>

                                            </div>

                                            <div class="tab-pane" id="CompareFunds" role="tabpanel" style="display: none">
                                                <div class="mb-3">
                                                    <div class="card">
                                                        <div class="card-body">
                                                            <div class="row">
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <div class="card-radio-label mb-0">
                                                                            <div class="card-radio text-truncate">
                                                                                <div class="text-center mt-2 mb-2" id="FirstAddBtn" runat="server">
                                                                                    <button type="button" class="btn btn-soft-info waves-effect rounded-circle" data-bs-keyboard="false" data-bs-backdrop="static"
                                                                                        data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg">
                                                                                        <i class="mdi mdi-plus mdi-18px"></i>
                                                                                    </button>
                                                                                    <asp:HiddenField ID="BtnClickedNo" runat="server" />
                                                                                    <h6 class="mt-2">Add Scheme</h6>
                                                                                </div>
                                                                                <asp:HiddenField ID="CurrScheme1" runat="server" />
                                                                                <asp:HiddenField ID="CurrSchemeId1" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndex1" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndexId1" runat="server" />
                                                                                <div id="FirstAddScheme" runat="server" style="display: none">
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
                                                                            </div>
                                                                            <!--  Large modal example -->
                                                                            <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" id="CompareModel" data-bs-backdrop="static"
                                                                                aria-labelledby="myLargeModalLabel" aria-hidden="true">
                                                                                <div class="modal-dialog modal-lg">
                                                                                    <div class="modal-content" data-bs-dismiss="false">
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
                                                                                                                OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChangedCompare">
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
                                                                                                            <%--<asp:RadioButtonList ID="rdbOptionCompare" runat="server" CssClass="" Width="100%"
                                                                                                                class="radio" RepeatDirection="Horizontal" AutoPostBack="false"
                                                                                                                Style="margin-left: 15px;">
                                                                                                               
                                                                                                            </asp:RadioButtonList>--%>

                                                                                                            <asp:DropDownList ID="ddlOptionCompare" runat="server" CssClass="form-select">
                                                                                                            </asp:DropDownList>
                                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Select option."
                                                                                                                ControlToValidate="ddlOptionCompare" Display="Dynamic" InitialValue="0" ValidationGroup="option">
                                                                                                            </asp:RequiredFieldValidator>

                                                                                                            <div class="invalid-tooltip">
                                                                                                                Please provide a valid city.
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
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


                                                                                                </div>
                                                                                                <div class="row" style="text-align: left!important;">
                                                                                                    <div class="col-md-8" >
                                                                                                        <div id="ddlSchemesCompareBox"
                                                                                                            class="mb-3 position-relative">
                                                                                                            <label
                                                                                                                class="form-label"
                                                                                                                for="validationTooltip02">
                                                                                                                Choose Scheme<span class="text-danger"  style="font-size: 1em">*</span>

                                                                                                            </label>
                                                                                                            <asp:DropDownList ID="ddlSchemesCompare" runat="server" CssClass="form-select">
                                                                                                                <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                                                                            </asp:DropDownList>
                                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                                                                                                ControlToValidate="ddlSchemesCompare" Display="Dynamic" InitialValue="0" ValidationGroup="scheme">
                                                                                                            </asp:RequiredFieldValidator>
                                                                                                        </div>
                                                                                                        <div class="pt-lg-3" id="NoSchemeAvailabelMsg" style="display:none"><span  class="text-danger" ><b>(For the specified filter, no scheme is available.)</b></span></div>
                                                                                                        <div><span id="NoSelectSchemeMsg" class="text-danger" style="display:none"><b>Please select valid scheme</b></span></div>
                                                                                                    </div>
                                                                                                    <div class="col-md-4"
                                                                                                        style="text-align: right;">
                                                                                                        <label
                                                                                                            class="form-label"
                                                                                                            for="validationTooltip02">
                                                                                                            &nbsp;</label>
                                                                                                        <div
                                                                                                            class="mt-1 position-relative">
                                                                                                            <button 
                                                                                                                class="btn btn-dark " type="button" id="btnAddSchemeCompares">
                                                                                                                Add</button>

                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </form>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <!--  Large modal example -->
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <div class="card-radio-label mb-0">
                                                                            <div class="card-radio p-2">
                                                                                <div id="SecondAddScheme" runat="server" style="display: none">
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
                                                                                    <button type="button" class="btn btn-soft-info waves-effect rounded-circle" data-bs-keyboard="false" data-bs-backdrop="static"
                                                                                        data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg">
                                                                                        <i class="mdi mdi-plus mdi-18px"></i>
                                                                                    </button>
                                                                                    <h6 class="mt-2">Add Scheme</h6>
                                                                                </div>
                                                                                <asp:HiddenField ID="CurrScheme2" runat="server" />
                                                                                <asp:HiddenField ID="CurrSchemeId2" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndex2" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndexId2" runat="server" />

                                                                            </div>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <div class="card-radio-label mb-0">
                                                                            <div class="card-radio text-truncate">
                                                                                <div class="text-center mt-2 mb-2" id="ThirdAddBtn" runat="server">
                                                                                    <button type="button" class="btn btn-soft-info waves-effect rounded-circle"
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
                                                                                <div id="ThirdAddScheme" runat="server" style="display: none">
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
                                                                            </div>
                                                                        </div>
                                                                        <!-- <div class="edit-btn bg-light rounded">
                                                <a href="#"  data-bs-toggle="tooltip" data-placement="top" title="" data-bs-original-title="Edit">
                                                    <i class="bx bx-pencil font-size-16"></i>
                                                </a>
                                            </div> -->
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 col-sm-6">
                                                                    <div data-bs-toggle="collapse">
                                                                        <div class="card-radio-label mb-0">
                                                                            <div class="card-radio text-truncate">
                                                                                <div id="FourthAddScheme" runat="server" style="display: none">
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
                                                                                    <button type="button" class="btn btn-soft-info waves-effect rounded-circle" data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg">
                                                                                        <i class="mdi mdi-plus mdi-18px"></i>
                                                                                    </button>
                                                                                    <h6 class="mt-2">Add Scheme</h6>
                                                                                </div>
                                                                                <asp:HiddenField ID="CurrScheme4" runat="server" />
                                                                                <asp:HiddenField ID="CurrSchemeId4" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndex4" runat="server" />
                                                                                <asp:HiddenField ID="CurrIndexId4" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <!-- <div class="edit-btn bg-light rounded">
                                                <a href="#"  data-bs-toggle="tooltip" data-placement="top" title="" data-bs-original-title="Edit">
                                                    <i class="bx bx-pencil font-size-16"></i>
                                                </a>
                                            </div> -->
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-12 col-md-12 mt-4 mb-2 mr-2"
                                                                style="text-align: right;">
                                                                <button id="btnCompareFund" type="button" class="btn btn-dark">
                                                                    Show Performance
                                                                </button>
                                                                <button id="btnCompareFundReset" type="button" class="btn btn-light">
                                                                    Reset
                                                                </button>

                                                            </div>
                                                        </div>
                                                       
                                                    </div>

                                                    <div class="row mt-2" id="DivShowPerformanceCard" style="display: none;">
                                                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                            <div class="card">
                                                                <div class="card-body pb-xl-2">
                                                                    <div id="DivShowPerformance" runat="server" style="display: none;">
                                                                        <div>

                                                                            <div id="GrdCompFund">
                                                                                <div class="table-responsive">
                                                                                </div>
                                                                            </div>

                                                                            <label id="lbRetrnMsg" class="gap-left" style="display: none;">
                                                                                <small>*Note:- Returns calculated for less than 1 year are Absolute returns and returns
                                                                                            calculated for more than 1 year are compounded annualized.</small></label>
                                                                        </div>
                                                                        <div style="margin-top: 40px; margin-right: 0px; margin-left: -15px;">
                                                                            <input id="HdSchemes" type="hidden" runat="server" />
                                                                            <input id="HdToData" type="hidden" runat="server" />
                                                                            <input id="HdFromData" type="hidden" runat="server" />

                                                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                                                                <div id="HighContainer" style="height: 600px; display: none;"></div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="tab-pane" id="NavTraker" role="tabpanel" style="display:none">

                                                <form class="needs-validation" novalidate="novalidate">
                                                    <div class="card">
                                                    <div class="card-body">
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
                                                                        for="validationTooltip03">
                                                                        Option
                                                                    </label>
                                                                    <%--<asp:RadioButtonList ID="rdbOptionNav" runat="server" class="radio"
                                                                        CssClass="" RepeatDirection="Horizontal" AutoPostBack="false"
                                                                        OnSelectedIndexChanged="rdbOption_SelectedIndexChangedNav" Width="100%">
                                                                    </asp:RadioButtonList>--%>
                                                                    <asp:DropDownList ID="ddlOptionNav" runat="server" CssClass="form-select" AutoPostBack="false">
                                                                    </asp:DropDownList>
                                                                    <div class="invalid-tooltip">
                                                                        Please provide a valid city.
                                                                    </div>
                                                                </div>



                                                                <%--<div class="mb-3 position-relative">
                                                                    <label class="form-label"
                                                                        for="validationTooltip02">
                                                                        Type</label>
                                                                    <asp:DropDownList ID="ddlTypeNav" runat="server" AutoPostBack="false"
                                                                        OnSelectedIndexChanged="ddlType_SelectedIndexChangedNav" CssClass="form-select">
                                                                    </asp:DropDownList>
                                                                </div>--%>
                                                                
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="mb-3 position-relative" id="ddlSchemesNavBox">
                                                                    <label class="form-label"
                                                                        for="validationTooltip02">
                                                                        Choose Scheme <span class="text-danger" style="font-size: 1em";>*</span> </label>
                                                                    <asp:DropDownList ID="ddlSchemesNav" runat="server" CssClass="form-select">
                                                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Select Scheme"
                                                                        ControlToValidate="ddlSchemesNav" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator>
                                                                </div>
                                                                <div class="pt-lg-5 mb-3" id="NoSchemeAvailabelMsgNav" style="display:none">
                                                                    <span class="text-danger"><b>(For the specified filter, no scheme is available.)</b></span>
                                                                </div>
                                                                <div class="" id="AllSchemeSelectedMsgNav" style="display:none">
                                                                    <span class="" style="color:#4467a6" ><b>(All schemes already added)</b></span>
                                                                </div>
                                                                <div><span id="NoSelectSchemeMsgNav" class="text-danger" style="display:none"><b>Please select valid scheme</b></span></div>
                                                            </div>
                                                            
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
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-8">
                                                                
                                                            </div>
                                                            <div class="col-md-4" style="text-align: right">
                                                                <div class="position-relative">
                                                                    <button type="button" class="btn btn-dark" id="btnAddSchemeNav">Add</button>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    </div>
                                                    <div class="card" id="DivGridContainCard" style="display:none;">
                                                        <div class="card-body">
                                                            <div class="row">
                                                                <div id="DivGridContain" style="display: none">
                                                                    <div class="table-responsive">
                                                                        <table id="tblResultNav" class="table align-middle table-nowrap table-centered mb-0">
                                                                        </table>
                                                                    </div>
                                                                    <div style="text-align: right; margin-top: 8px;">
                                                                        <button type="button" id="btnPlotChart" class="btn btn-dark" style="display: none">Plot Chart</button>
                                                                    </div>


                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="table-responsive">
                                                                    <div style="margin-top: 40px; margin-right: 0px; margin-left: -15px;">
                                                                        <input id="hidSchindSelected" type="hidden" runat="server" />
                                                                        <input id="HdToDataNav" type="hidden" runat="server" />
                                                                        <input id="HdFromDataNav" type="hidden" runat="server" />

                                                                        <div id="HighContainerNav" style="height: 600px; display: none;"></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </form>
                                            </div>

                                        </div>
                                    <%--</div>--%>
                                <%--</div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="overlay">
            <div class="cv-spinner">
                <span class="spinner"></span>
            </div>
        </div>

        <!-- JAVASCRIPT -->
        <script src="assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
        <script src="assets/libs/metismenujs/metismenujs.min.js"></script>
        <script src="assets/libs/simplebar/simplebar.min.js"></script>
        <script src="assets/libs/feather-icons/feather.min.js"></script>
        <script src="assets/js/app.js"></script>
    </form>
</body>

<script>
    $(function () {


        // Hide show id's on the basis of page value  (TF, CF, NT, SF)
        var pageValue = $("#PageValue").val();
        //alert(pageValue);
        if (pageValue.toUpperCase() === "TF") {

            $("#TopFund").show();
            //$("#SearchFunds").hide();
            //$("#CompareFunds").hide();
            //$("#NavTraker").hide();

        } else if (pageValue.toUpperCase() === "CF") {

            //$("#TopFund").hide();
            //$("#SearchFunds").hide();
            $("#CompareFunds").show();
            //$("#NavTraker").hide();

        } else if (pageValue.toUpperCase() === "SF") {

            //$("#TopFund").hide();
            $("#SearchFunds").show();
            //$("#CompareFunds").hide();
            //$("#NavTraker").hide();

        } else if (pageValue.toUpperCase() === "NT") {
            //$("#TopFund").hide();
            //$("#SearchFunds").hide();
            //$("#CompareFunds").hide();
            $("#NavTraker").show();
        } else {
            $("#TopFund").show();
            //$("#SearchFunds").hide();
            //$("#CompareFunds").hide();
            //$("#NavTraker").hide();
        }



        //pin scrooler
        $('.left-scroll').click(function (e) {
            
            e.preventDefault();
            var container = document.getElementById('scroll-div');
            sideScroll(container, 'left', 25, 100, 10);
        });
        $('.right-scroll').click(function (e) {
            
            e.preventDefault();
            var container = document.getElementById('scroll-div');
            sideScroll(container, 'right', 25, 100, 10);
        })

    });


    function sideScroll(element, direction, speed, distance, step) {
        scrollAmount = 0;


        var slideTimer = setInterval(function () {
            if (direction == 'left') {
                element.scrollLeft -= step;

            } else {
                element.scrollLeft += step;


            }
            scrollAmount += step;
            if (scrollAmount >= distance) {
                window.clearInterval(slideTimer);
            }


        }, speed);
    }

    $(document).ready(function () {
        $("#slider").slider({
            range: "min",
            animate: true,
            value: 500,
            min: 500,
            max: 50000,
            step: 500,
            slide: function (event, ui) {
                if (ui.value == "undefined") {
                    update(1, 500);
                }
                update(1, ui.value);
                //changed
            }
        });

        $("#slider1").slider({
            range: "min",
            animate: true,
            value: 5,
            min: 5,
            max: 50,
            step: 1,
            slide: function (event, ui) {
                update1(1, ui.value);
                //changed
            }
        });

        //Added, set initial value.
        $("#amount").val(0);
        $("#amount-label").text(0);


        update(1, 500);
        update1(1, 5);

        //*********************** Pagination for Search Fund Page ******************************



    });

    function paginationTbl() {
        $('table.paginated').each(function () {
            
            var $table = $(this);
            var itemsPerPage = 10;
            var currentPage = 0;
            var pages = Math.ceil($table.find("tr:not(:has(th))").length / itemsPerPage);
            $table.bind('repaginate', function () {

                window.scrollTo({
                  top: 0,
                  left: 0,
                  behavior: 'smooth'
                });

                if (pages > 1) {
                    var pager;
                    if ($table.next().hasClass("pager"))
                        pager = $table.next().empty(); else
                        pager = $('<div class="pager" style="padding-top: 20px; direction:ltr; " align="center"></div>');

                    $('<button class="pg-goto"></button>').text(' « First ').bind('click', function () {
                        currentPage = 0;
                        $table.trigger('repaginate');
                    }).appendTo(pager);

                    $('<button class="pg-goto"> « Prev </button>').bind('click', function () {
                        if (currentPage > 0)
                            currentPage--;
                        $table.trigger('repaginate');
                    }).appendTo(pager);

                    var startPager = currentPage > 2 ? currentPage - 2 : 0;
                    var endPager = startPager > 0 ? currentPage + 3 : 5;
                    if (endPager > pages) {
                        endPager = pages;
                        startPager = pages - 5; if (startPager < 0)
                            startPager = 0;
                    }

                    for (var page = startPager; page < endPager; page++) {
                        $('<span id="pg' + page + '" class="' + (page == currentPage ? 'pg-selected' : 'pg-normal') + '"></span>').text(page + 1).bind('click', {
                            newPage: page
                        }, function (event) {
                            currentPage = event.data['newPage'];
                            $table.trigger('repaginate');
                        }).appendTo(pager);
                    }

                    $('<button class="pg-goto"> Next » </button>').bind('click', function () {
                        if (currentPage < pages - 1)
                            currentPage++;
                        $table.trigger('repaginate');
                    }).appendTo(pager);
                    $('<button class="pg-goto"> Last » </button>').bind('click', function () {
                        currentPage = pages - 1;
                        $table.trigger('repaginate');
                    }).appendTo(pager);

                    if (!$table.next().hasClass("pager"))
                        pager.insertAfter($table);
                    //pager.insertBefore($table);

                }
                //else {
                //    $table.next(".pager").hide();
                //}

                $table.find(
                    'tbody tr:not(:has(th))').hide().slice(currentPage * itemsPerPage, (currentPage + 1) * itemsPerPage).show();
            });

            $table.trigger('repaginate');
        });
    }

    //changed. now with parameter
    function update(slider, val) {
        //changed. Now, directly take value from ui.value. if not set (initial, will use current value.)
        var $amount = slider == 1 ? val : $("#amount").val();

        $("#HiddenMinimumInvesment").val(val);

        $("#amount").val($amount);
        $("#amount-label").text($amount);

        $('#slider a').html('<label>' + $amount + '</label><div class="ui-slider-label-inner"></div>');
    }

    function update1(slider, val) {

        //changed. Now, directly take value from ui.value. if not set (initial, will use current value.)
        var $amount = slider == 1 ? val : $("#amount").val();

        $("#HiddenMinimumSIReturn").val(val);
        $("#amount").val($amount);
        $("#amount-label").text($amount);

        $('#slider1 a').html('<label>' + $amount + '</label><div class="ui-slider-label-inner"></div>');
    }


</script>

</html>
