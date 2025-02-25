<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true"
    CodeBehind="SipCalc.aspx.cs" Inherits="iFrames.Edelweiss.SipCalc" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>SIP Calculator</title>

    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/nifty.min.v1.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />

    <script src="script/jquery.min.js"></script>
    <script src="script/jquery-ui.js"></script>
    <script src="script/bootstrap.min.js"></script>
    <script src="script/EdelweissChecks.js"></script>

    <script type="text/javascript" language="javascript">

        $(function () {
            var values = eval('<%=Values%>');
            var values1 = eval('<%=SelectedValues%>');
            if (values == null) {
                addOnLoadHtml();
            }

            if (values != null) {
                var html = "";
                var i = 0;
                $(values).each(function () {
                    var div = $("<div />");
                    var p = GettxtDynSwitchAmt(this);
                    var selValues = String(values1).split(",");
                    p = p.replace('<option value="' + selValues[i] + '">', '<option value="' + selValues[i] + '" selected="selected">');
                    div.html(p);
                    $("#TextBoxContainer").append(div);
                    i++;

                });
            }
            $("#btnAdd").bind("click", function () {
                //addButtonClick();
            });
            $("body").on("click", ".remove", function () {
                $(this).parent('div').parent('div').remove();
                var count = $('input[name*="txtDynSwitchAmt"]').length;
                if (count < 1) { addOnLoadHtml(); }
                ChangePlaceHolder(document.getElementById("ddlSIPType").value)
            });

            $("body").on("click", ".add", function () {
                var count = $('input[name*="txtDynSwitchAmt"]').length;

                if (count == 1) {
                    var Element = document.getElementById("btnRemove");
                    if (Element != null) { Element.style.visibility = 'visible'; }
                }
                if (count < 3) { addButtonClick(); }
                ChangePlaceHolder(document.getElementById("ddlSIPType").value)
            });

            $("#txtfrdt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1,
                minDate: mdate()
            });

            function mdate() {

                var ddScheme = document.getElementById("ddlFromScheme");

                if (ddScheme === null) {
                    var ddToScheme = document.getElementById("ddlToScheme");
                    var Process = document.getElementById("ddlSIPType");
                    if (ddToScheme === null) { return; }
                    if (Process.value == 'Power SIP') {
                        var allotdate = ddToScheme.options[ddToScheme.selectedIndex].title;
                        var Day = parseInt(allotdate.substring(0, 2), 10);
                        var Mn = parseInt(allotdate.substring(3, 5), 10);
                        var Yr = parseInt(allotdate.substring(6, 10), 10);
                        var DateVal = Mn + "/" + Day + "/" + Yr;
                        var dt = new Date(DateVal);
                        return dt;
                    }
                    else { return; }
                }
                var allotdate = ddScheme.options[ddScheme.selectedIndex].title;

                var Day = parseInt(allotdate.substring(0, 2), 10);
                var Mn = parseInt(allotdate.substring(3, 5), 10);
                var Yr = parseInt(allotdate.substring(6, 10), 10);
                var DateVal = Mn + "/" + Day + "/" + Yr;
                var dt = new Date(DateVal);


                return dt;
            }
            $("#ddlFromScheme").change(function () {
                //debugger;
                $("#txtfrdt").datepicker("destroy");
                $("#txtfrdt").datepicker({
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    maxDate: -1,
                    minDate: mdate()

                });

            });
            $("#txttodt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1,
                minDate: mdate()
                //                 maxDate: 0
            });

            $("#txtcurdt").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: -1,
                minDate: mdate()
                //                 maxDate: 0
            });
            var Process = document.getElementById("ddlSIPType");
            //if (Process.value == 'Power SIP') {
            //    $('input[name*="txtDynSwitchAmt"]').attr("placeholder", "SIP Amount");
            //    $('#btnRemove').hide();
            //    $('#btnAdd').hide();
            //    $('#btnAdd4Add').hide();
            //    $('#btnRemove4Add').hide();
            //    //$('select[name*="dynTriggerAmt"]').css("width", "80");
            //    //$('input[name*="txtDynSwitchAmt"]').css("width", "80");
            //    $('#TrNoInvestmentSIP').show();
            //    $('#TrTriggerGain').hide();
            //    //$('#NoOfInvestmenttxt').css("width", "30%");
            //}
            if (Process.value == 'Power STP' ) {
                $('#btnRemove').show();
                $('#btnAdd').show();
                $('#btnAdd4Add').show();
                $('#btnRemove4Add').show();
                $('#TrNoInvestmentSIP').hide();
                $('#TrTriggerGain').hide();
                $('input[name*="txtDynSwitchAmt"]').attr("placeholder", "Switch Amount");
            }
            if (Process.value == 'Power SIP') {
                 $('#btnRemove').show();
                $('#btnAdd').show();
                $('#btnAdd4Add').show();
                $('#btnRemove4Add').show();
                $('#TrNoInvestmentSIP').show();
                $('#TrTriggerGain').hide();
                $('input[name*="txtDynSwitchAmt"]').attr("placeholder", "SIP Amount");
            }
            if (Process.value == 'Gain Switch Mechanism') {
                $('#TrTriggerGain').show();
                $('#TrNoInvestmentSIP').hide();
            }
            //$('#NoOfInvestmenttxt').css("width","30");
        });

        function ChangePlaceHolder(value) {
            switch (value) {
                case "Power STP":
                    $('input[name*="txtDynSwitchAmt"]').attr("placeholder", "Switch Amount");
                    break;
                case "Gain Switch Mechanism":
                    break;
                case "Power SIP":
                    $('input[name*="txtDynSwitchAmt"]').attr("placeholder", "SIP Amount");
                    break;
            }
        }

        function showTab(e,selected, total) {
            $('.tab-btns-list').find('input').each(function (i, value) {
                $(this).removeClass('tablinks-active');
            })
            $(e).addClass('tablinks-active');
            for (i = 1; i <= total; i += 1) {
                document.getElementById('country-' + i).style.display = 'none';
                $("#" + 'country-' + i).removeClass('active in');
            }

            document.getElementById('country-' + selected).style.display = 'block';
            $("#" + 'country-' + selected).addClass('active in');
        }

        function addOnLoadHtml() {
            var div = $("<div style='' />");
            var p = GettxtDynSwitchAmtOnLoad("");
            div.html(p);
            $("#TextBoxContainer").append(div);
        }

        function GettxtDynSwitchAmt(value) {
            var html = '';
            html += '<div style="width:100%; float: left; padding-bottom:10px;"><div class="col-xs-12 col-md-5 col-lg-5 col-sm-5">'
                + '<select name="dynTriggerAmt" class="form-control">'
                + '<option value="-0.5">-0.5 %</option>'
                + '<option value="-1.0">-1.0 %</option>'
                + '<option value="-2.0">-2.0 %</option>'
                + '</select> </div>'
                + '  <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5" style="text-align: center; vertical-align: bottom; ">'
                + '<div class="input-group">'
                + '<span class="input-group-addon">₹</span>'
                + '<input type="text" name="txtDynSwitchAmt" placeholder="Switch Amount" class="form-control" onkeypress="return isNumber(event)" value = "' + value + '" >'
                + '</div>'
                + '</div>'
                + '<div class="col-xs-12 col-md-2 col-lg-2 col-sm-2" style="margin-top:2px">'
                + '<a href="javascript:void(0)"  id="btnAdd4Add" class="add">'
                + '<img src="IMG/add.png" title="Add" /></a>'
                + '<a href="javascript:void(0)" id="btnRemove4Add" class="remove" >&nbsp;'
                + '<img src="IMG/remove.png" title="Remove" /></a>'
                + '</div></div>';
            //+ '<div class="col-xs-12 col-md-5 col-lg-5 col-sm-5"></div>';
            return html;
        }
        function GettxtDynSwitchAmtOnLoad(value) {
            var html = '';
            html += '<div style="width:100%; float: left; padding-bottom:10px;"><div class="col-xs-12 col-md-5 col-lg-5 col-sm-5">'
                + '<select name = "dynTriggerAmt" class="form-control" > '
                + '<option value="-0.5">-0.5 %</option>'
                + '<option value="-1.0">-1.0 %</option>'
                + '<option value="-2.0">-2.0 %</option>'
                + '</select></div>'
                + '  <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5" style="text-align: center; vertical-align: bottom">'
                + '<div class="input-group">'
                + '<span class="input-group-addon">₹</span>'
                + '<input type="text" name="txtDynSwitchAmt" onkeypress="return isNumber(event)" placeholder="Switch Amount" class="form-control" value = "' + value + '" >'
                + '</div>'
                + '</div>'
                + '<div class="col-xs-12 col-md-2 col-lg-2 col-sm-2" style="margin-top:2px">'
                + '<a href="javascript:void(0)"  id="btnAdd4Add" class="add">'
                + '<img src="IMG/add.png" title="Add" /></a>'
                + '<a href="javascript:void(0)" id="btnRemove" class="remove" style="visibility:hidden">&nbsp;'
                + '<img src="IMG/remove.png" title="Remove" /></a>'
                + '</div></div>';
            //+ '<div class="col-xs-12 col-md-5 col-lg-5 col-sm-5"></div>';
            return html;
        }

        function addButtonClick() {

            var div = $("<div />");
            div.html(GettxtDynSwitchAmt(""));
            $("#TextBoxContainer").append(div);
            var count = $('input[name*="txtDynSwitchAmt"]').length;
            //if (count == 3) { $("#btnAdd").prop('disabled', true); }
        }

    </script>
    <style type="text/css">
        .table_format {
            margin-left: 40px;
            border-bottom:none;
        }

        .table-header-text {
            text-align: right;
        }

       .table th {
            color: #212529;
            font-family: 'Helvetica';
            font-size:14px;
        }
        .table tr td {
            font-size:12px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <%--<div id="loader" class="loader-container">
            <div class="loader-content">
                Please Wait...
            </div>
        </div>--%>
        <div id="" class="effect aside-float aside-bright mainnav-lg">
            <div class="boxed">
                <div id="content-container">
                    <div id="page-content">
                        <div class="panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">
                                    <img src="IMG/calc.png" style="margin: 10px 10px 0 10px" />CALCULATE AND INVEST THE RIGHT AMOUNT</h3>
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group">
                                            <div class="well well-sm row">
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">Investment Mode</label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                    <asp:DropDownList ID="ddlSIPType" runat="server" AutoPostBack="true" class="form-control"
                                                        OnSelectedIndexChanged="ddlSIPType_SelectedIndexChanged">
                                                        <asp:ListItem>Power STP</asp:ListItem>
                                                        <asp:ListItem>Gain Switch Mechanism</asp:ListItem>
                                                        <asp:ListItem>Power SIP</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                     <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group" id="sourceSchemeselection" runat="server">
                                            <div class="well well-sm row">
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">Source Scheme</label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                    <asp:DropDownList ID="ddlFromScheme" class="form-control" runat="server"
                                                        OnSelectedIndexChanged="ddlFromScheme_SelectedIndexChanged" AutoPostBack="True"
                                                        OnPreRender="ddlFromScheme_PreRender">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group">
                                            <div class="well well-sm row">
                                                 <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">Target Scheme</label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                    <asp:DropDownList ID="ddlToScheme" CssClass="form-control" runat="server" OnPreRender="ddlToScheme_PreRender"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlToScheme_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0" runat="server" id="IntInvtRow">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group has-feedback" >
                                            <div class="well well-sm row" style="margin-bottom:0">
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">Amount Invested (₹)</label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                    <asp:TextBox ID="txtInitialInvestment" onkeypress="return isNumber(event)" runat="server" class="form-control"
                                                            placeholder="Amount" />
                                                </div>
                                            </div>
                                             <div class="col-xs-12 col-md-5 col-lg-5 col-sm-5"></div>
                                             <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                  <asp:RangeValidator ID="rvInitialInvestment" runat="server" ControlToValidate="txtInitialInvestment" MinimumValue="25000" MaximumValue="1800000" Enabled="false" Type="Integer" Style="font-size: 11px; margin: 0px 0px 0px 5px; color: red"></asp:RangeValidator>
                                             </div>
                                        </div>
                                    </div>
                                   
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group" id="TrTriggerGain" runat="server">
                                            <div class="well well-sm row">
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5" id="benchmarkChangeText" runat="server">
                                                    Trigger<small id="changetype" runat="server"> (benchmark change %)</small></label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7" id="triggarAmt" runat="server">
                                                    <asp:DropDownList ID="ddlTriggerAmt" class="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                              
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5" style="display: none;">Benchmark Index</label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7" style="display: none">
                                                    <asp:TextBox ID="txtIndex" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnIndexID" runat="server" />
                                                    <asp:HiddenField ID="HdnIndexName" runat="server" />
                                                    <asp:HiddenField ID="HdnIndexNameWithOutTRI" runat="server" />
                                                    <asp:HiddenField ID="HdnAddBenchId" runat="server" />
                                                    <asp:HiddenField ID="HdnAddBenchName" runat="server" />
                                                </div>
                                               
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="TrNoInvestmentSIP" runat="server">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <div class="well well-sm row">
                                                    <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5" id="NoOfInvestmenttxt" runat="server">No of Investment</label>
                                                    <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7" id="NoOfInvestmentId" runat="server">
                                                        <asp:DropDownList ID="ddlNoOfInvestment" class="form-control" runat="server">
                                                            <asp:ListItem Value="1" Text="">1</asp:ListItem>
                                                            <asp:ListItem Value="3" Text="">3</asp:ListItem>
                                                            <asp:ListItem Value="5" Text="">5</asp:ListItem>
                                                            <asp:ListItem Value="7" Text="">7</asp:ListItem>
                                                            <asp:ListItem Value="0" Text="">Infinite</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0; display:none;">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <div class="well well-sm row">
                                                    <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5" id="Label1" runat="server">Additional Benchmark Index</label>
                                                    <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7" id="Div1" runat="server">
                                                        <asp:DropDownList ID="ddlAdditionalBenchmark" class="form-control" runat="server">
                                                            <asp:ListItem Value="Crisil Liquid Fund Index" Text="">Crisil Liquid Fund Index</asp:ListItem>
                                                            <asp:ListItem Value="Crisil MIP Blended Index" Text="">Crisil MIP Blended Index</asp:ListItem>
                                                            <asp:ListItem Value="Crisil Balanced Fund - Aggressive Index" Text="">Crisil Balanced Fund - Aggressive 

Index</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div id="TrNoInvestmentSIP" runat="server">
                                    <div class="form-group row">
                                        <label class="col-xs-12 col-md-3 col-lg-3 col-sm-3" id="NoOfInvestmenttxt" runat="server">No Of Investment</label>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6" id="NoOfInvestmentId" runat="server">
                                            <asp:DropDownList ID="ddlNoOfInvestment" class="form-control" runat="server">
                                                <asp:ListItem Value="3" Text="">3</asp:ListItem>
                                                <asp:ListItem Value="5" Text="">5</asp:ListItem>
                                                <asp:ListItem Value="7" Text="">7</asp:ListItem>
                                                <asp:ListItem Value="0" Text="">Infinite</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-xs-12 col-md-3 col-lg-3 col-sm-3" id="Label1" runat="server">Additional Benchmark Index</label>
                                        <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6" id="Div1" runat="server">
                                            <asp:DropDownList ID="ddlAdditionalBenchmark" class="form-control" runat="server">
                                                <asp:ListItem Value="Crisil Liquid Fund Index" Text="">Crisil Liquid Fund Index</asp:ListItem>
                                                <asp:ListItem Value="Crisil MIP Blended Index" Text="">Crisil MIP Blended Index</asp:ListItem>
                                                <asp:ListItem Value="Crisil Balanced Fund - Aggressive Index" Text="">Crisil Balanced Fund - Aggressive 

Index</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                    </div>
                                </div>--%>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                            <div class="well well-sm row">
                                                <div class="form-group has-feedback">
                                                    <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">From Date</label>
                                                    <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtfrdt" runat="server" type="text" class="form-control" placeholder="Select date" />
                                                            <span class="input-group-addon"><img src="IMG/calendar.png" /></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-lg-2 col-md-2 col-sm-2 col-xs-12"></div>
                                        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                                            <div class="well well-sm row">
                                                <div class="form-group has-feedback">
                                                    <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">To Date</label>
                                                    <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txttodt" runat="server" type="text" class="form-control" placeholder="Select date" />
                                                            <span class="input-group-addon">
                                                                <img src="IMG/calendar.png" /></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group has-feedback" runat="server" id="Div3">
                                            <div class="well well-sm row" style="margin-bottom:0">
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">Current Date</label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtcurdt" runat="server" class="form-control" placeholder="Select date" />
                                                        <span class="input-group-addon">
                                                            <img src="IMG/calendar.png" /></span>
                                                    </div>
                                                    <span style="font-size:11px;">('Current Date' should be greater than 'To Date')</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group has-feedback" id="switchAmtSIPPlus" runat="server">
                                            <div class="well well-sm row">
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5">
                                                    Trigger
                                                     <small id="Span1" runat="server">(benchmark change %)</small></label>
                                                <div id="TextBoxContainer" class="col-xs-12 col-md-7 col-lg-7 col-sm-7" style="padding-left: 0px;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="padding: 0">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group" id="switchamountid" runat="server">
                                            <div class="well well-sm row">
                                                <label class="col-xs-12 col-md-5 col-lg-5 col-sm-5" id="switchText" runat="server">Switch Amount</label>
                                                <div class="col-xs-12 col-md-7 col-lg-7 col-sm-7" id="Div2" runat="server">
                                                    <asp:TextBox ID="txtSwitchAmt" runat="server" class="form-control" placeholder="Amount" onChange="Javascript: setFocus(); " />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="form-group row" id="switchamountid" runat="server">
                                    <label class="col-xs-12 col-md-3 col-lg-3 col-sm-3" id="switchText" runat="server">Switch Amount</label>
                                    <div class="col-xs-12 col-md-6 col-lg-6 col-sm-6" id="Div2" runat="server">
                                        <asp:TextBox ID="txtSwitchAmt" runat="server" class="form-control" placeholder="Amount" onChange="Javascript: setFocus(); " />
                                    </div>
                                    <div class="col-xs-12 col-md-3 col-lg-3 col-sm-3"></div>
                                </div>--%>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:Button ID="btnCalculate" OnClientClick="return validateEdelweissInput();" runat="server" class="btn btn-info"
                                            Text="Calculate" OnClick="btnCalculate_Click" />
                                        <button class="btn btn-default" type="reset" id="btnReset" onclick="javascript:window.location.assign(window.location.href);">
                                            Reset</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel" id="reportPanel" runat="server">
                            <div class="panel-heading">
                                <div class="panel-control">
                                    <asp:Button ID="btnExport" class="btn btn-success btn-sm" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />
                                </div>
                                <h3 class="panel-title">Report</h3>
                            </div>

                            <div class="panel-body">
                                <div class="table-responsive">
                                    <asp:GridView ID="gridSummary" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Visible="true">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="">
                                                <ItemTemplate>
                                                    <%# (Eval("Return"))%>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left" CssClass="gridtd1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="ARF">
                                                <ItemTemplate>
                                                    <%# (Eval("ARF"))%>
                                                </ItemTemplate>
                                                <%--<HeaderTemplate>Edge</HeaderTemplate>--%>
                                                <HeaderStyle CssClass="" />
                                                <ControlStyle></ControlStyle>
                                                <HeaderStyle HorizontalAlign="Center" CssClass="gridtd"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="EDGE">
                                                <ItemTemplate>
                                                    <%# (Eval("Edge"))%>
                                                </ItemTemplate>
                                                <%--<HeaderTemplate>Edge</HeaderTemplate>--%>
                                                <HeaderStyle CssClass="" />
                                                <HeaderStyle HorizontalAlign="right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="Power STP">
                                                <ItemTemplate>
                                                    <%# (Eval("PrepaidSIP"))%>
                                                </ItemTemplate>
                                                <%--<HeaderTemplate>Power SIP</HeaderTemplate>--%>
                                                <HeaderStyle CssClass="table-header-text" />
                                                <HeaderStyle HorizontalAlign="right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="Monthly STP >">
                                                <ItemTemplate>
                                                    <%# (Eval("MonthlySIP"))%>
                                                </ItemTemplate>
                                               <%-- <HeaderTemplate>Monthly SIP</HeaderTemplate>--%>
                                                <HeaderStyle CssClass="table-header-text" />
                                                <HeaderStyle HorizontalAlign="right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="Benchmark @">
                                                <ItemTemplate>
                                                    <%# (Eval("Benchmark"))%>
                                                </ItemTemplate>
                                                <%--<HeaderTemplate>Benchmark @</HeaderTemplate>--%>
                                                <HeaderStyle CssClass="table-header-text" />
                                                <HeaderStyle HorizontalAlign="right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="Additional Benchmark $">
                                                <ItemTemplate>
                                                    <%# (Eval("AdditionalBenchmark"))%>
                                                </ItemTemplate>
                                                <%--<HeaderTemplate>Additional Benchmark $</HeaderTemplate>--%>
                                                <HeaderStyle CssClass="table-header-text" />
                                                <HeaderStyle HorizontalAlign="right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="grdRow" />
                                    </asp:GridView>

                                    <div id="div_text" runat="server" visible="false" style="font-size: 11px;">
                                        <small>* Returns are ABSOLUTE for &lt;1 yr and COMPOUND ANNUALIZED for &gt;=1 yr</small>
                                    </div>

                                    <div id="benchmarkfull" runat="server" visible="false" style="font-size: 11px;"><small>@ Benchmark = CRISIL Composite Bond Fund Index</small></div>
                                    <div id="addbenchmarkfull" runat="server" visible="false" style="font-size: 11px;"><small>$ Additional Benchmark= Nifty 50 TRI</small></div>
                                    <div id="MonthlySIPid" runat="server" visible="false" style="font-size: 11px;">
                                        <small>&gt; STP is a systematic investment from the source scheme to the target scheme on the first of every month</small>
                                    </div>
                                    <div runat="server" id="fromschemefull" visible="false" style="font-size: 11px;">
                                    </div>
                                    <div runat="server" id="toschemefull" visible="false" style="font-size: 11px;">
                                    </div>
                                </div>

                                <div style="margin-top:20px;" id="Div_tab" visible="false" runat="server">
                                    <div class="tab-btns-list">
                                        <input type="button" onclick="showTab(this,1,2)" class="tab-button tablinks tab-br-tl tablinks-active" value="Detail Report" >
                                        <input type="button" onclick="showTab(this,2,2)" class="tab-button tablinks tab-br-rb" value="View Graph">

                                    </div>
                                    <!--Nav Tabs-->
                                    <%--<ul class="nav nav-tabs" runat="server" id="Div_tab" visible="false">
                                        <li class="active">
                                            <a data-toggle="tab" href="#demo-lft-tab-1" rel="country1" class="tab-br-tl">Detail Report</a>
                                        </li>
                                        <li>
                                            <a data-toggle="tab" href="#demo-lft-tab-2" rel="country2" class="tab-br-rb">View Graph</a>
                                        </li>
                                    </ul>--%>

                                    <!--Tabs Content-->
                                    <div class="tab-content">
                                        <div id="country-1" class="tab-pane fade active in">
                                            <div class="table-responsive">
                                                <table class="table" id="tblFromScheme" runat="server">
                                                    <tr>
                                                        <td class="FieldHead1" style="border:none;color: #f7941d; font-size: 15px; font-weight: 600;">From Scheme :
                                                            <span id="lblFromScheme" runat="server" style="font-weight: bold"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style=" border-bottom:none;">
                                                            <asp:GridView ID="gridDetailsFrom" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Visible="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Date"))%>
                                                                        </ItemTemplate>

                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="NAV">

                                                                        <ItemTemplate>
                                                                            <%# (Eval("Nav"))%>
                                                                        </ItemTemplate>
                                                                        
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="Index Change (%)">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Index_Change"))%>
                                                                        </ItemTemplate>
                                                                        <%--<HeaderTemplate>
                                                                            Index Change (%)
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="SIP Amount (₹)" HeaderStyle-CssClass="">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("SIP_Amount"))%>
                                                                        </ItemTemplate>
                                                                       <%-- <HeaderTemplate>
                                                                            SIP Amount (₹)
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="Scheme Units">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Scheme_Units"))%>
                                                                        </ItemTemplate>
                                                                       <%-- <HeaderTemplate>
                                                                            Scheme Units
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="right" HeaderText="Fund Value (₹)">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Cumulative_Fund_Value"))%>
                                                                        </ItemTemplate>
                                                                        <%--<HeaderTemplate>
                                                                            Fund Value (₹)
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="grdRow" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="table-responsive">
                                                <table class="table">
                                                    <tr>
                                                        <td class="FieldHead1" colspan="2" style="border:none;color: #f7941d; font-size: 15px; font-weight: 600;">To Scheme :<span id="lblToScheme" runat="server" style="font-weight: bold"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left" class="table_format" style=" border-bottom:none;">
                                                            <asp:GridView ID="gridDetailsTo" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Visible="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Date"))%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="left" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="NAV">
                                                                       <%-- <HeaderTemplate>
                                                                            NAV
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Nav"))%>
                                                                        </ItemTemplate>

                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Index Change (%)">
                                                                       <%-- <HeaderTemplate>
                                                                            Index Change (%)
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Index_Change"))%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="SIP Amount (₹)">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("SIP_Amount"))%>
                                                                        </ItemTemplate>
                                                                        <%--<HeaderTemplate>
                                                                            SIP Amount (₹)
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />

                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Units">

                                                                        <ItemTemplate>
                                                                            <%# (Eval("Scheme_Units"))%>
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            Scheme Units
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Fund Value (₹)">
                                                                        <ItemTemplate>
                                                                            <%# (Eval("Cumulative_Fund_Value"))%>
                                                                        </ItemTemplate>
                                                                       <%-- <HeaderTemplate>
                                                                            Fund Value (₹)
                                                                        </HeaderTemplate>--%>
                                                                        <HeaderStyle CssClass="table-header-text" />
                                                                        <ItemStyle HorizontalAlign="right" CssClass="gridtd2" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="grdRow" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div id="country-2" class="tab-pane fade">
                                            <div class="table-responsive">
                                                <table class="table table-bordered" >
                                                    
                                                    <tr>
                                                        <td height="30" class="FieldHead1" id="charid" runat="server" colspan="4" style="border:none;color: #f7941d; font-size: 15px; font-weight: 600;">Power SIP Performance Chart
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center" style="text-align:center; border-bottom:none; background:#fff" >
                                                            <asp:Chart ID="chartSIP" Visible="false" runat="server" BorderlineColor="128, 128, 255">
                                                                <Series>
                                                                    <asp:Series Name="Series1" ChartType="Line" YValuesPerPoint="6">
                                                                    </asp:Series>
                                                                </Series>
                                                                <ChartAreas>
                                                                    <asp:ChartArea Name="ChartArea1">
                                                                        <AxisY>
                                                                            <MajorGrid LineWidth="0" />
                                                                        </AxisY>
                                                                        <AxisX>
                                                                            <MajorGrid LineWidth="0" />
                                                                        </AxisX>
                                                                    </asp:ChartArea>
                                                                </ChartAreas>
                                                            </asp:Chart>
                                                            <asp:Image ID="ImgChrt" runat="server" BorderlineColor="128, 128, 255"  />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="text-align: justify; font-size: 11px;">
                            <strong>Disclaimer:</strong><br />
                            Past Performance may or may not be sustained in future. The calculator is meant only for illustration purpose and should not be construed as an investment advice. The return calculator has been developed and is maintained by ICRA Analytics Limited. Edelweiss Asset Management Limited (EAML) / ICRA Analytics Ltd. do not endorse the suitability, authenticity and/or accuracy of the information, figures based on which the returns are calculated; nor shall they be held responsible or liable for any error or inaccuracy or for any losses suffered by any investor as a direct or indirect consequence of relying upon the data displayed by the calculator. In the preparation of the calculator, information that is publicly available and information developed in-house has been used. It is designed to assist you in determining the appropriate amount of prospective investments. This calculator alone is not sufficient and shouldn’t be used for the development or implementation of any investment strategy. The various taxes, fees, load, expenses and /or any charges are not considered in the calculation provided by the calculator. SIP does not assure a profit or guarantee protection against loss in a declining market. EAML does not take the responsibility / liability nor does it undertake the authenticity of the figures calculated therein. EAML makes no warranty about the accuracy of the calculators/reckoners. ICRA Analytics Ltd. is not involved nor has any opinion on any recommendation(s) made on this site and hereby disclaims all warranties and conditions with regard to this information, recommendation, including all implied warranties and conditions of merchantability, fitness for a particular purpose, title and non-infringement. You assume the sole risk of making use and/or relying on the materials made available on this site. The examples do not claim to represent the performance of any security or investments. In view of individual nature of tax consequences, each investor is advised to consult his/ her own professional tax advisor before making any investment decisions on the basis of the results provided through the use of this calculator. Mutual Fund investments are subject to market risks, read all scheme related documents carefully.
                            <br />
                            <br />

                            Developed by: <a class="text" href="https://www.icraanalytics.com" target="_blank">ICRA Analytics Ltd</a>, <a class="text" href="https://icraanalytics.com/home/Disclaimer" target="_blank">Disclaimer</a>.
                        </div>

                    </div>

                </div>

            </div>
        </div>
    </form>
</body>
</html>
