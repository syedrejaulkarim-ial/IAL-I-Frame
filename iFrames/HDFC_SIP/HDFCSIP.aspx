<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always"
    EnableViewStateMac="true" CodeBehind="HDFCSIP.aspx.cs" Inherits="iFrames.HDFCSIP.HDFCSIP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <!--[if IE]>
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	<![endif]-->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>HDFC Calculator</title>
    <link rel="icon" href="assets/img/favicon.ico">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <!-- BEGIN CSS FRAMEWORK -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="font-awesome-4.6.3/css/font-awesome.min.css" />
    <!-- END CSS FRAMEWORK -->
    <!-- BEGIN CSS PLUGIN -->
    <link rel="stylesheet" href="css/pace-theme-minimal.css" />
    <link href="css/awesome-bootstrap-checkbox.css" rel="stylesheet" />
    <!-- END CSS PLUGIN -->
    <!-- BEGIN CSS TEMPLATE -->
    <link rel="stylesheet" href="css/main.css" />
    <link href="CSS/tabcontent.css" rel="stylesheet" />
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="DatePicker/bootstrap-datepicker.js" type="text/javascript"></script>
    <link href="DatePicker/datepicker.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Script/check.js" type="text/javascript"></script>
    <script src="js/date.js" type="text/javascript"></script>
    <%--<link href="CSS/datepicker.css" rel="stylesheet" type="text/css" />--%>
    <!-- END CSS TEMPLATE -->
    <script type="text/javascript">
        function pop() {
           
            // alert("test");
            //            //            $(function () {
            //            $('#<%= sipbtnshow.ClientID %>').click(function () {
            //                $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
            //            });
            //});
            $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
        }

        function SplitDate(val) {
            return (val.getMonth() + 1) + '/' + val.getDate() + '/' + val.getFullYear();
        }



        function GetMinDate() {
            var ddScheme = document.getElementById('SIPSchDt');
            return ddScheme.value;
        }



        $(document).ready(function () {
            $('#calFromDt').datepicker({
                startDate: GetMinDate(),
                endDate:'now'
            });
            $('#calToDate').datepicker({
                startDate: GetMinDate(),
                endDate: 'now'
            });
            $('#calvalason').datepicker({
                startDate: GetMinDate(),
                endDate: 'now'
            });
            $('#calLumpfromDate').datepicker({
                startDate: GetMinDate(),
                endDate: 'now'
            });
            $('#calLumpToDate').datepicker({
                startDate: GetMinDate(),
                endDate: 'now'
            });
            $('#InitDate').datepicker({
                startDate: GetMinDate(),
                endDate: 'now'
            });
            //


            $('#calFromDt').datepicker().on('changeDate', function (ev) {
                var calval = $("#calFromDt").datepicker("getDate");
                $('#txtfromDate').val(Date.parse(SplitDate(calval), "MM/dd/yyyy").toString("dd/MM/yyyy"));
                $('#calFromDt').datepicker('hide');
            });
            $('#calToDate').datepicker().on('changeDate', function (ev) {
                var calval = $("#calToDate").datepicker("getDate");
                $('#txtToDate').val(Date.parse(SplitDate(calval), "MM/dd/yyyy").toString("dd/MM/yyyy"));
                $('#calToDate').datepicker('hide');
            });
            $('#calvalason').datepicker().on('changeDate', function (ev) {
                var calval = $("#calvalason").datepicker("getDate");
                $('#txtvalason').val(Date.parse(SplitDate(calval), "MM/dd/yyyy").toString("dd/MM/yyyy"));
                $('#calvalason').datepicker('hide');
            });
            //             $('#calToDate').datepicker().on('changeDate', function (ev) {
            //                 var calval = $("#calToDate").datepicker("getDate");
            //                 $('#txtToDate').val(Date.parse(SplitDate(calval), "MM/dd/yyyy").toString("dd/MM/yyyy"));
            //                 $('#calToDate').datepicker('hide');
            //             });
            $('#calLumpfromDate').datepicker().on('changeDate', function (ev) {
                var calval = $("#calLumpfromDate").datepicker("getDate");
                $('#txtLumpfromDate').val(Date.parse(SplitDate(calval), "MM/dd/yyyy").toString("dd/MM/yyyy"));
                $('#calLumpfromDate').datepicker('hide');
            });
            $('#calLumpToDate').datepicker().on('changeDate', function (ev) {
                var calval = $("#calLumpToDate").datepicker("getDate");
                $('#txtLumpToDate').val(Date.parse(SplitDate(calval), "MM/dd/yyyy").toString("dd/MM/yyyy"));
                $('#calLumpToDate').datepicker('hide');

            });
            $('#InitDate').datepicker().on('changeDate', function (ev) {
                var calval = $("#InitDate").datepicker("getDate");
                $('#txtIniToDate').val(Date.parse(SplitDate(calval), "MM/dd/yyyy").toString("dd/MM/yyyy"));
                $('#InitDate').datepicker('hide');

            });
            //
        });
    </script>
</head>
<body class="pace-done skin-blue">
    <!-- BEGIN CONTENT -->
    <form id="form1" runat="server">
    <section class="content">
				<div class="row">
                	<div class="col-md-12">
                		<div class="col-lg-2"></div>
                		<div class="col-lg-8" style="border-top:#103880 solid 8px;-moz-box-shadow: 0px 0px 8px #666; -webkit-box-shadow: 0px 0px 8px #666;box-shadow: 0px 0px 8px #666;">	
                         			
                            <div class="row">
                    	    <div class="col-lg-3" style="margin-top:20px;">Investment Mode</div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<select id="ddlMode" class="form-control">
                                   <option>SIP</option>
                                   <option>Balanced</option>
                                   <option>Debt</option>
                                   <option>Dynamic/Asset Allocation</option>
                                   <option>Equity</option>
                               </select>--%>
                        <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="True" class="form-control"  OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                                        <asp:ListItem Selected="True">SIP</asp:ListItem>
                                        <asp:ListItem Selected="False">Lump Sum</asp:ListItem>
                                        <asp:ListItem Selected="False">SIP with Initial Investment</asp:ListItem>
                                        <asp:ListItem Selected="False">SWP</asp:ListItem>
                                        <asp:ListItem Selected="False">STP</asp:ListItem>
                              </asp:DropDownList>
                            </div>
                            <div class="col-lg-1" style="margin-top:15px;"></div>
                            <div class="col-lg-4" style="margin-top:15px;"></div>
                            </div>
                    	    <div class="row" style="margin-top:20px;">
                    	    <div class="col-lg-3" style="margin-top:20px;">Category</div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<select class="form-control">
                                   <option>Equity</option>
                                   <option>Balanced</option>
                                   <option>Debt</option>
                                   <option>Dynamic/Asset Allocation</option>
                                   <option>Equity</option>
                               </select>--%>
                               <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" DataTextField="Nature"
                                DataValueField="Nature" class="form-control" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                            </asp:DropDownList>
                            </div>
                            <div class="col-lg-1" style="margin-top:15px;"></div>
                            <div class="col-lg-4" style="margin-top:15px;"></div>
                        </div>
                    	    <div class="row" style="margin-top:20px;">
                    	    <div class="col-lg-3" style="margin-top:20px;" ID="lblSchemeName" runat="server">Scheme Name</div>
                            <div class="col-lg-5" style="margin-top:15px;">
                                <%--<select class="form-control">
                                   <option>Equity</option>
                                   <option>Balanced</option>
                                   <option>Debt</option>
                                   <option>Dynamic/Asset Allocation</option>
                                   <option>Equity</option>
                               </select>--%>

                               <asp:DropDownList ID="ddlscheme" runat="server"  class="form-control" Width="" AutoPostBack="True"
                                                                                                                OnSelectedIndexChanged="ddlscheme_SelectedIndexChanged">
                                                                                                            </asp:DropDownList>

                            </div>
                            <div class="col-lg-4" style="margin-top:15px;"></div>
                        </div>
                    	    <div class="row" style="margin-top:20px;" runat="server"> 
                    	    <div class="col-lg-3" style="margin-top:20px;">Inception Date</div>
                            <div class="col-lg-3" style="margin-top:15px;">
                        	    <%--<input type="text" class="form-control" id="SIPSchDt" name="SIPSchDt" runat="server" placeholder="Inception Date" />--%>
                            <asp:textbox id="SIPSchDt" placeholder="Inception Date"  class="form-control" runat="server" />
                               

                            </div>
                            <div class="col-lg-1" style="margin-top:15px;"></div>
                            <div class="col-lg-4" style="margin-top:15px;"></div>
                        </div>
                    	    <div class="row" style="margin-top:20px;" id="trTransferTo" runat="server">
                    	    <div class="col-lg-3" style="margin-top:20px;">Transfer To </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<select class="form-control">
                                   <option>Equity</option>
                                   <option>Balanced</option>
                                   <option>Debt</option>
                                   <option>Dynamic/Asset Allocation</option>
                                   <option>Equity</option>
                               </select>--%>
                               <asp:DropDownList ID="ddlschtrto" runat="server" AutoPostBack="True" class="form-control"
                                                                                                                Width="360px" OnSelectedIndexChanged="ddlschtrto_SelectedIndexChanged">
                                                                                                            </asp:DropDownList>
                            </div>
                            <div class="col-lg-1" style="margin-top:15px;"></div>
                            <div class="col-lg-4" style="margin-top:15px;"></div>
                        </div>
                    	    <div class="row" style="margin-top:20px;" id="trInception" runat="server">
                    	    <div class="col-lg-3" style="margin-top:20px;">Inception Date  </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                        	    <div data-date="2014-06-14T05:25:07Z" data-date-format="dd-mm-yyyy HH:ii" data-link-field="dtp_input1">
												    <%--<input type="text" class="form-control">--%>
                                                    <asp:textbox class="form-control" id="SIPSchDt2" runat="server" placeholder="Inception Date"></asp:textbox>
												    <%--<p class="input-group-addon" style="margin-right:-20px;"><i class="fa fa-calendar"></i></p>--%>
											    </div>
                            </div>
                            <div class="col-lg-1" style="margin-top:15px;"></div>
                            <div class="col-lg-4" style="margin-top:15px;"></div>
                        </div>
                    	    <div class="row" style="margin-top:20px;" id="trBenchmark" runat="server">
                    	    <div class="col-lg-3" style="margin-top:20px;">Benchmark </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<input type="text" class="form-control" placeholder="Benchmark ">--%>
                                <asp:TextBox ID="txtddlsipbnmark" runat="server" class="form-control" placeholder="Benchmark " Text="" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-lg-1" style="margin-top:15px;"></div>
                            <div class="col-lg-4" style="margin-top:15px;"></div>
                            </div>
                    	    <div class="row" style="margin-top:20px;" id="trInitialInvst" runat="server">
                    	    <div class="col-lg-3" style="margin-top:20px;">Initial Amount (Rs.)</span> </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<input type="text" class="form-control" placeholder="Initial Amount  (Rs.) ">--%>
                                <asp:TextBox ID="txtiniAmount" runat="server" class="form-control" placeholder="Initial Amount " MaxLength="10" ReadOnly="false"
                                                                                                                Text="" ></asp:TextBox>
                            </div>
                            <div class="col-lg-3" style="margin-top:15px;">Initial Date </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                        	    <div class="input-group date form_datetime" data-date="2014-06-14T05:25:07Z" data-date-format="dd-mm-yyyy HH:ii" data-link-field="dtp_input1">
												    <%--<input type="text" class="form-control">--%>
                                                    <asp:textbox id="txtIniToDate" class="form-control" runat="server" />
												    <p class="input-group-addon" style="margin-right:-20px;"><i class="fa fa-calendar" style="cursor:pointer" id="InitDate"></i></p>
											    </div>
                            </div>
                        </div>
                            <div id="trSipInvst" runat="server" visible="false" >
                    	    <div class="row" style="margin-top:20px;">
                    	    <div class="col-lg-3" style="margin-top:20px;" id="lblInstallmentAmt" runat="server">Installment Amount (Rs.)</span> </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<input type="text" class="form-control" placeholder="Placeholder">--%>
                                <asp:TextBox ID="txtinstall" class="form-control" placeholder="Installment Amt" MaxLength="10" Text="" runat="server" ReadOnly="false"  onmousedown="Javascript: setDate(); " onChange="Javascript: checkInvestedValue();"></asp:TextBox>
                            </div>
                            <div class="col-lg-3" style="margin-top:15px;" id="lblTransferWithdrawal" runat="server">Withdrawal Amount (Rs.)</div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<input type="text" class="form-control" placeholder="Withdrawal Amount">--%>
                                <asp:TextBox ID="txtTransferWithdrawal" runat="server" class="form-control" placeholder="Withdrawal Amount" MaxLength="14" onChange="Javascript: checkInvestedValue();" Text="" ReadOnly="false"></asp:TextBox>
                            </div>
                           </div>
                    	    <div class="row" style="margin-top:20px;">
                    	    <div class="col-lg-3" style="margin-top:20px;" >Frequency  </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<select class="form-control">
                                   <option>Monthly</option>
                                   <option>Balanced</option>
                                   <option>Debt</option>
                                   <option>Dynamic/Asset Allocation</option>
                                   <option>Equity</option>
                               </select>--%>
                               <asp:DropDownList ID="ddPeriod_SIP" runat="server" class="form-control">
                                                                                                                <asp:ListItem Value="Monthly" Selected="True">Monthly</asp:ListItem>
                                                                                                                <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                                                                                            </asp:DropDownList>
                            </div>
                            <div class="col-lg-3" style="margin-top:15px;" id="lblDiffDate" runat="server">SIP Date</div>
                           
                        	     <div class="col-lg-3" style="margin-top:15px;">
                                     <%--<select class="form-control">
                                   <option>Monthly</option>
                                   <option>Balanced</option>
                                   <option>Debt</option>
                                   <option>Dynamic/Asset Allocation</option>
                                   <option>Equity</option>
                               </select>--%>
                              <asp:DropDownList ID="ddSIPdate" runat="server" class="form-control">
                                                                                                                <asp:ListItem Value="1">1st</asp:ListItem>
                                                                                                                <asp:ListItem Value="5">5th</asp:ListItem>
                                                                                                                <asp:ListItem Value="10">10th</asp:ListItem>
                                                                                                                <asp:ListItem Value="15">15th</asp:ListItem>
                                                                                                                <asp:ListItem Value="20">20th</asp:ListItem>
                                                                                                                <asp:ListItem Value="25">25th</asp:ListItem>
                                                                                                            </asp:DropDownList>
                            </div>
                            </div>
                       
                    	    <div class="row" style="margin-top:20px;">
                    	    <div class="col-lg-3" style="margin-top:20px;">From Date  </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                        	    <div class="input-group date form_datetime"id="dp3" data-date="2014-06-14T05:25:07Z" data-date-format="dd-mm-yyyy HH:ii" data-link-field="dtp_input1">
                                                    <%--<input type="text" id="txtLumpToDate" class="form-control">--%>
                                                     <asp:textbox id="txtfromDate" class="form-control" runat="server" onMouseDown="Javascript: setDate();" />
												    <p class="input-group-addon" style="margin-right:-20px;"><i class="fa fa-calendar" id="calFromDt" style="cursor:pointer"></i></p>
											    </div>
											    </div>
                            <div class="col-lg-3" style="margin-top:15px;">To Date </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                        	    <div class="input-group date form_datetime" data-date="2014-06-14T05:25:07Z" data-date-format="dd-mm-yyyy HH:ii" data-link-field="dtp_input1">
                                                    <%--<input type="text" id="txtLumpToDate" class="form-control">--%>
                                                     <asp:textbox id="txtToDate" class="form-control" runat="server" />
												    <p class="input-group-addon" style="margin-right:-20px;"><i class="fa fa-calendar" id="calToDate" style="cursor:pointer"></i></p>
											    </div>
                            </div>
                        </div>
                    	    <div class="row" style="margin-top:20px;">
                    	    <div class="col-lg-3" style="margin-top:20px;">Value as on Date </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                                <%--<input type="text" class="form-control" placeholder="Value as on Date">--%> 
                                
                                 <div class="input-group date form_datetime" data-date="2014-06-14T05:25:07Z" data-date-format="dd-mm-yyyy HH:ii" data-link-field="dtp_input1">
                                                    <%--<input type="text" id="txtLumpToDate" class="form-control">--%>
                                                     <asp:textbox id="txtvalason" class="form-control" runat="server" />
												    <p class="input-group-addon" style="margin-right:-20px;"><i class="fa fa-calendar" id="calvalason" style="cursor:pointer"></i></p>
											    </div>
                                <h6 style="margin-top:2px;">('Value as on Date' should be greater than 'To Date')</h6>
                            </div>
                           
                            <div class="col-lg-5" style="margin-top:15px;"></div>
                        </div>
                            </div>
                            <div id="trLumpInvst" runat="server" visible="false">
                            <div class="row" style="margin-top:30px;" >
                                <div class="col-lg-3" style="margin-top:10px;">Investment Amount (Rs.)</div>
                                <div class="col-lg-3" style="margin-top:10px;">
                                    <%--<input type="text" class="form-control" placeholder="Investment Amount (Rs.)">--%> 
                                    <asp:TextBox ID="txtinstallLs" value="" MaxLength="10" runat="server" class="form-control" placeholder="Investment Amount" onmousedown="Javascript: setDate();" onChange="Javascript: checkInvestedValue();">
                                                                                                            </asp:TextBox>
                                </div>
                                <div class="col-lg-2" style="margin-top:15px;"></div>
                                <div class="col-lg-4" style="margin-top:15px;"></div>
                            </div>
                            <div class="row" style="margin-top:20px;">
                    	    <div class="col-lg-3" style="margin-top:20px;">Start Date </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                        	    <div class="input-group date form_datetime" data-date="2014-06-14T05:25:07Z" data-date-format="dd-mm-yyyy HH:ii" data-link-field="dtp_input1">
												    <asp:textbox runat="server" ID="txtLumpfromDate" class="form-control" />
												    <p class="input-group-addon" style="margin-right:-20px;"><i class="fa fa-calendar" style="cursor:pointer" id="calLumpfromDate"></i></p>
											    </div>
                            </div>
                            <div class="col-lg-3" style="margin-top:15px;">End Date </div>
                            <div class="col-lg-3" style="margin-top:15px;">
                        	    <div class="input-group date form_datetime" data-date="2014-06-14T05:25:07Z" data-date-format="dd-mm-yyyy HH:ii" data-link-field="dtp_input1">
												    <asp:textbox runat="server" ID="txtLumpToDate" class="form-control" />
												    <p class="input-group-addon" style="margin-right:-20px;"><i class="fa fa-calendar"  style="cursor:pointer" id="calLumpToDate"></i></p>
											    </div>
                            </div>
                        </div>
                    	   </div>
                    	    <div class="row" style="margin-top:40px;">
                    		    <div class="col-lg-3" style="margin-top:20px;"></div>
                        	    <div class="col-lg-3" style="margin-top:15px;"></div>
                        	  
                                <div class="col-lg-6" align="right">
                        <asp:Button ID="sipbtnshow" runat="server" Text="Show"  class="btn btn-primary"  OnClick="sipbtnshow_Click"></asp:Button>
                        <asp:Button  ID="sipbtnreset" runat="server" Text="Reset" class="btn btn-default" OnClick="sipbtnreset_Click"></asp:Button>
                                    <%--<button type="button" class="btn btn-primary">Submit</button>
                                    <button type="button" class="btn btn-default">Reset</button>--%>

                                </div>
                        </div>
                            <div class="row">
                    	      <hr class="hr-dashed" />
                              </div>
                    	    <div class="row" style="padding:0; margin:0">
                                 
                    		    <div class="table-responsive" style="padding:0; margin:0">	
                                <div id="resultDiv" runat="server" visible="false" style="padding:0; margin:0">
                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="display: none" class="table table-striped">
                                                                                            <tr align="left">
                                                                                                <td width="3%" height="20" align="center" valign="middle">
                                                                                                    <img src="IMG/arw.gif" width="4" height="8" />
                                                                                                </td>
                                                                                                <td width="97%" height="25" valign="middle">
                                                                                                    <%-- <span class="rslt_text">Investment amount per month : Rs<strong> 5000</strong></span>--%>
                                                                                                    <asp:Label ID="lblInvestment" CssClass="rslt_text" runat="server" Text="Investment amount per month"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left">
                                                                                                <td width="3%" align="center" valign="middle">
                                                                                                    <img src="IMG/arw.gif" width="4" height="8" />
                                                                                                </td>
                                                                                                <td height="25" align="left" valign="middle">
                                                                                                    <%--<span class="rslt_text">Total Investment Amount : Rs <strong>120000</strong></span>--%>
                                                                                                    <asp:Label ID="lblTotalInvst" CssClass="rslt_text" runat="server" Text="Total Investment Amount"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="3%" height="25" align="center" valign="middle">
                                                                                                    <img src="IMG/arw.gif" width="4" height="8" />
                                                                                                </td>
                                                                                                <td align="left" valign="middle">
                                                                                                    <%--<span class="rslt_text">On 05/07/2012, the value of your total investment Rs 120000
                                                                                            would be Rs <strong>131779.37</strong></span>--%>
                                                                                                    <asp:Label ID="lblInvstvalue" CssClass="rslt_text" runat="server" Text="On Date C, the Scheme value of your total investment Rs Y would be Rs Z">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="3%" height="25" align="center" valign="middle">
                                                                                                    <img src="IMG/arw.gif" width="4" height="8" />
                                                                                                </td>
                                                                                                <td align="left" valign="middle">
                                                                                                    <asp:Label ID="lblAbsoluteReturn" CssClass="rslt_text" runat="server" Text="Absolute return from Date  to Date  is X%">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="3%" height="25" align="center" valign="middle">
                                                                                                    <img src="IMG/arw.gif" width="4" height="8" />
                                                                                                </td>
                                                                                                <td height="25" align="left" valign="middle">
                                                                                                    <%--<span class="rslt_text">XIsRR return of Investment from 01/07/2010 to 01/07/2012 is <strong>
                                                                                            9.17%</strong> </span>--%>
                                                                                                    <asp:Label ID="lblCagrReturn" CssClass="rslt_text" runat="server" Text="XIRR return from Date  to Date  is X%">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="3%" height="25" align="center" valign="middle">
                                                                                                    <img src="IMG/arw.gif" width="4" height="8" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblIfInvested" CssClass="rslt_text" runat="server" Text="Had you invested Rs Y at Date A, the total value of this investment at Date C would have become Q">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td height="25" colspan="2" align="center" valign="middle">
                                                                                                    <div align="left">
                                                                                                        &nbsp;&nbsp;&nbsp;<b>View Historical Fund Performance below:<b />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>						
                                    <table width="100%" id="firstTable" style="padding:0; margin:0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div id="divSummary" runat="server" style="padding:0; margin:0">
                                                                                                        <asp:GridView ID="gvFirstTable" runat="server" AutoGenerateColumns="False" Width="100%" Visible="false" HeaderStyle-CssClass="grdHead" RowStyle-CssClass="grdRow" AlternatingRowStyle-CssClass="grdAltRow">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="">
                                                                                                                    <ItemTemplate>
                                                                                                                        <span style="padding-left:3px;"><%# (Eval("Scheme") == DBNull.Value) ? "--" : Eval("Scheme").ToString()%></span>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="220px" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Units<br/> Purchased">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Total_unit") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble( Eval("Total_unit")),0).ToString("n0")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle  />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Amount<br/> Invested (A)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Total_amount") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_amount")), 0).ToString("n0")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle  />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Investment Value<br/> as on Date (B)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle  />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Total Profit<br/> (B-A)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Profit_Sip") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Profit_Sip")), 0).ToString("n0")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle  />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Abs. <br/>Returns">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle  />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Returns*" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Top">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Yield") == DBNull.Value) ? "--" :( Eval("Yield").ToString() + "%")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle  />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>

                                                                                                        <asp:GridView ID="gvSWPSummaryTable" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                                            HeaderStyle-CssClass="grdHead" Visible="false" AlternatingRowStyle-CssClass="grdAltRow"
                                                                                                            RowStyle-CssClass="grdRow">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="210px" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Amount<BR/> Invested (A)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Amount<BR/> Withdrawn (B)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Total_Amount_Withdrawn") == DBNull.Value) ? "N.A." : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Withdrawn")), 0).ToString("n0")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Present<BR/> Value (C)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0") %><%-- TwoDecimal(Eval("").ToString() --%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Total Profit<BR/> (B+C-A)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# "<img src='img/rimg.png' style='vertical-align:middle;'> "+ totalProfit(Eval("Total_Amount_Invested").ToString(), Eval("Total_Amount_Withdrawn").ToString(),Eval("Present_Value").ToString()) %>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Abs. <br/>Returns">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("ABSOLUTERETURN") == DBNull.Value) ? "--" : (Eval("ABSOLUTERETURN").ToString() + "%")%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle  />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Returns *" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Top">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                        <asp:GridView ID="gvSTPToSummaryTable" runat="server" AutoGenerateColumns="False" Width="100%" HeaderStyle-CssClass="grdHead" Visible="false" AlternatingRowStyle-CssClass="grdAltRow" RowStyle-CssClass="grdRow">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="40%" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Total Amount Invested">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Total_Amount_Invested") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Total_Amount_Invested")), 0).ToString("n0")%>
                                                                                                                        <%--Eval("Total_Amount_Invested").ToString()--%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" Width="24%" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Present Value">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Present_Value") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Math.Round(Convert.ToDouble(Eval("Present_Value")), 0).ToString("n0")%>
                                                                                                                        <%--TwoDecimal(Eval("Present_Value").ToString())--%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" Width="18%" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Yield">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%# (Eval("Yield") == DBNull.Value) ? "--" : Eval("Yield").ToString() +"%"%>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle CssClass="borderbottom" Width="18%" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                        <asp:GridView ID="GridViewLumpSum" runat="server" Width="98%" CssClass="grdRow" HeaderStyle-CssClass="grdHead" RowStyle-CssClass ="grdRow" AlternatingRowStyle-CssClass="grdAltRow" AutoGenerateColumns="false">
                                                                                                                        <Columns>
                                                                                                                            <asp:TemplateField>
                                                                                                                                <ItemTemplate>
                                                                                                                                    <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString()%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="230px" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Amount Invested <br/>(A)">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <%# (Eval("InvestedAmount") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedAmount")).ToString("n2")%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Investment Value<br/>(B)">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <%# (Eval("InvestedValue") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("InvestedValue")).ToString("n2")%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Profit from Investment<br/>(B-A)">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <%# (Eval("Profit") == DBNull.Value) ? "--" : "<img src='img/rimg.png' style='vertical-align:middle;'> " + Convert.ToDouble(Eval("Profit")).ToString("n2")%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Return *<br/>" HeaderStyle-VerticalAlign="Top">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <%# (Eval("Return") == DBNull.Value) ? "--" : Eval("Return").ToString() + ((Eval("Return").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle CssClass="borderbottom" />
                                                                                                                            </asp:TemplateField>
                                                                                                                        </Columns>
                                                                                                                    </asp:GridView>
                                                                                                                    </div>
                                                                                                                    </td>
                                                                                                                    </tr>
                                                                                                                    </table>
                                 <%--somabrata removed--%>
                    	    
                            <div class="row">
                    		    <div class="table-responsive">	
                                 <table width="100%" id="TableRemark" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div id="divTab" runat="server" visible="false">
                                                                                                        <br />						
                                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                                        <tr align="left">
                                                                                                                            <td align="left" valign="top">
                                                                                                                                <ul id="countrytabs" class="shadetabs">
                                                                                                                                    <li>
                                                                                                                                        <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click">View Detail Report</asp:LinkButton></li>
                                                                                                                                    <li>
                                                                                                                                        <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click">View Graph</asp:LinkButton></li>
                                                                                                                                    <li>
                                                                                                                                        <asp:LinkButton ID="lnkTab3" runat="server" OnClick="lnkTab3_Click">View Historical Performance</asp:LinkButton></li>
                                                                                                                                    <li>
                                                                                                                                        <asp:LinkButton ID="lnkTab4" runat="server" OnClick="lnkTab4_Click">View PDF Report</asp:LinkButton></li>
                                                                                                                                </ul>
                                                                                                                                <div style="width: 100%; margin-top:15px;" >
                                                                                                                                    <asp:MultiView ID="MultiView1" runat="server">
                                                                                                                                        <table width="100%" cellpadding="2" cellspacing="5">
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <asp:View ID="View1" runat="server">
                                                                                                                                                        <div id="DetailDiv" runat="server">
                                                                                                                                                            <%--<div id="country1" class="tabcontent">--%>
                                                                                                                                                            <asp:GridView ID="sipGridView" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                                                                                                HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdAltRow" RowStyle-CssClass="grdRow">
                                                                                                                                                                <Columns>
                                                                                                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%#  (Eval("Nav_Date") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[2]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[1]), Convert.ToInt32(Eval("Nav_Date").ToString().Split('/')[0])).ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle  />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle  />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle  />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Scheme Units">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("Scheme_units") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_units")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle  />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("SCHEME_UNITS_CUMULATIVE") == DBNull.Value) ? "--" :Math.Round(Convert.ToDouble( Eval("SCHEME_UNITS_CUMULATIVE")),2).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle  />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="SIP Amount">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("Scheme_cashflow") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("Scheme_cashflow")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle  />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cumulative Fund Value">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("CUMULATIVE_AMOUNT") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMULATIVE_AMOUNT")), 2).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle  />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                </Columns>
                                                                                                                                                            </asp:GridView>
                                                                                                                                                            <asp:GridView ID="swpGridView" runat="server" AutoGenerateColumns="False" Width="98%"
                                                                                                                                                                HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdAltRow" RowStyle-CssClass="grdRow">
                                                                                                                                                                <Columns>
                                                                                                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%#  (Eval("DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("DATE").ToString().Split('/')[0])).ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NAV")).ToString("n2") %>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("DIVIDEND_BONUS") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cashflow">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("FINAL_INVST_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT")).ToString("n2") %>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <%-- <asp:TemplateField HeaderText="Investment Amount">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("INVST_AMOUNT")).ToString("n2")  %>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                </asp:TemplateField>--%>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("CUMILATIVE_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("CUMILATIVE_UNITS")).ToString("n2") %>
                                                                                                                                                                            <%-- Math.Round(Convert.ToDouble(Eval("")), 2).ToString()--%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Cumulative Amount">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("CUMILATIVE_AMOUNT") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("CUMILATIVE_AMOUNT")).ToString("n2")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                </Columns>
                                                                                                                                                            </asp:GridView>
                                                                                                                                                            <div id="divSTP" runat="server" visible="False">
                                                                                                                                                                <br />
                                                                                                                                                                <b>From Scheme:
                                                                                                                                                                <%= ddlscheme.SelectedItem.Text %>
                                                                                                                                                                </b>
                                                                                                                                                                <br />
                                                                                                                                                                <br />
                                                                                                                                                                <asp:GridView ID="stpFromGridview" runat="server" AutoGenerateColumns="False" Width="98%"
                                                                                                                                                                    HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdAltRow" RowStyle-CssClass="grdRow">
                                                                                                                                                                    <Columns>
                                                                                                                                                                        <%-- <asp:TemplateField HeaderText="From Scheme">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%#  (Eval("FROM_SCHEME_NAME") == DBNull.Value) ? "--" : Eval("FROM_SCHEME_NAME").ToString()%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                </asp:TemplateField>--%>
                                                                                                                                                                        <asp:TemplateField HeaderText="Date">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%-- <%#  (Eval("FROM_DATE") == DBNull.Value) ? "--" : Eval("FROM_DATE").ToString()%>--%>
                                                                                                                                                                                <%#  (Eval("FROM_DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("FROM_DATE").ToString().Split('/')[0])).ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("FROM_NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FROM_NAV")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("DIVIDEND_BONUS_FROM") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS_FROM").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS_FROM")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cashflow">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("FINAL_INVST_AMOUNT_FROM") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT_FROM")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Redeemed Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("REDEEM_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("REDEEM_UNITS")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <%--                                                                               <asp:TemplateField HeaderText="Investment Amount">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <%# (Eval("INVST_AMOUNT") == DBNull.Value) ? "--" : Eval("INVST_AMOUNT").ToString()%>
                                                                                                                                                </ItemTemplate>
                                                                                                                                            </asp:TemplateField>--%>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_UNITS_FROM") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_UNITS_FROM")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Investment Value">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_AMOUNT_FROM") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_AMOUNT_FROM")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                    </Columns>
                                                                                                                                                                </asp:GridView>
                                                                                                                                                                <br />
                                                                                                                                                                <b>To Scheme:
                                                                                                                                                                <%= ddlschtrto.SelectedItem.Text %>
                                                                                                                                                                </b>
                                                                                                                                                                <br />
                                                                                                                                                                <br />
                                                                                                                                                                <asp:GridView ID="stpToGridview" runat="server" AutoGenerateColumns="False" Width="98%"
                                                                                                                                                                    HeaderStyle-CssClass="grdHead" AlternatingRowStyle-CssClass="grdAltRow" RowStyle-CssClass="grdRow">
                                                                                                                                                                    <Columns>
                                                                                                                                                                        <%-- <asp:TemplateField HeaderText="To Scheme">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%#  (Eval("TO_SCHEME_NAME") == DBNull.Value) ? "--" : Eval("TO_SCHEME_NAME").ToString()%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                </asp:TemplateField>--%>
                                                                                                                                                                        <asp:TemplateField HeaderText="Date">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%-- <%#  (Eval("TO_DATE") == DBNull.Value) ? "--" : Eval("TO_DATE").ToString()%>--%>
                                                                                                                                                                                <%#  (Eval("TO_DATE") == DBNull.Value) ? "--" : Convert.ToDateTime(new DateTime(Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[2]), Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[1]), Convert.ToInt32(Eval("TO_DATE").ToString().Split('/')[0])).ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="NAV">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("TO_NAV") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("TO_NAV")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Dividend(%)">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("DIVIDEND_BONUS_TO") == DBNull.Value) ? "--" : Eval("DIVIDEND_BONUS_TO").ToString().Trim() == "0" ? "--" : Convert.ToDouble(Eval("DIVIDEND_BONUS_TO")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cashflow">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("FINAL_INVST_AMOUNT_TO") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("FINAL_INVST_AMOUNT_TO")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="No. of Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("NO_OF_UNITS") == DBNull.Value) ? "--" : Convert.ToDouble(Eval("NO_OF_UNITS")).ToString("n2")%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Cumulative Units">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_UNITS_TO") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_UNITS_TO")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                        <asp:TemplateField HeaderText="Investment Value">
                                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                                <%# (Eval("CUMILATIVE_AMOUNT_TO") == DBNull.Value) ? "--" : Math.Round(Convert.ToDouble(Eval("CUMILATIVE_AMOUNT_TO")), 2).ToString()%>
                                                                                                                                                                            </ItemTemplate>
                                                                                                                                                                            <ItemStyle  />
                                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                                    </Columns>
                                                                                                                                                                </asp:GridView>
                                                                                                                                                            </div>
                                                                                                                                                            <%--</div>--%>
                                                                                                                                                            <br />
                                                                                                                                                            <table width="100%">
                                                                                                                                                                <tr>
                                                                                                                                                                    <td align="right" style="padding-right: 20px">
                                                                                                                                                                        <asp:ImageButton ID="btnExcelCalculation" runat="server" ImageUrl="~/HDFC_SIP/IMG/excell.png"
                                                                                                                                                                            ToolTip="Download Excel" Text="Show Excel Calculation" Visible="true" OnClick="ExcelCalculation_Click" />
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                            </table>
                                                                                                                                                        </div>
                                                                                                                                                    </asp:View>
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <asp:View ID="View2" runat="server">
                                                                                                                                                        <%--  <div id="country2" class="tabcontent">--%>
                                                                                                                                                        <div id="divshowChart" runat="server" visible="true" style="width: 100%;">
                                                                                                                                                            <asp:Chart ID="chrtResult" runat="server" AlternateText="HDFC" Visible="true" BorderlineWidth="2"
                                                                                                                                                                Width="650px" Height="580px" IsSoftShadows="false">
                                                                                                                                                                <%--BackGradientStyle="Center"  BorderlineColor="RoyalBlue" BackColor="Gray"--%>
                                                                                                                                                                <Titles>
                                                                                                                                                                    <asp:Title Font="Trebuchet MS, 12pt" Text="HDFC" ForeColor="26, 59, 105">
                                                                                                                                                                    </asp:Title>
                                                                                                                                                                </Titles>
                                                                                                                                                                <Legends>
                                                                                                                                                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                                                                                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 7pt, ">
                                                                                                                                                                    </asp:Legend>
                                                                                                                                                                </Legends>
                                                                                                                                                                <ChartAreas>
                                                                                                                                                                    <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="" AlignmentStyle="PlotPosition" BackSecondaryColor="White" BackColor="White" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                                                                                                                        BackHatchStyle="None" BorderDashStyle="Solid">
                                                                                                                                                                        <%--BackImageTransparentColor="#CCCCFF"--%>
                                                                                                                                                                        <%--<Area3DStyle Enable3D="false" Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                                                                                                                        WallWidth="0" IsClustered="False"></Area3DStyle>--%>
                                                                                                                                                                        <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="Time period">
                                                                                                                                                                            <LabelStyle Format="yyyy" />
                                                                                                                                                                            <%--<ScaleBreakStyle Enabled="false" />
                                                                                                                                                                        <ScaleView SizeType="Years" />--%>
                                                                                                                                                                            <MajorGrid Enabled="false" />
                                                                                                                                                                        </AxisX>
                                                                                                                                                                        <AxisY ArrowStyle="None" ToolTip="Value (Rs.)" TextOrientation="Horizontal" LineColor="#013974">
                                                                                                                                                                            <ScaleBreakStyle Enabled="True" />
                                                                                                                                                                            <MajorGrid Enabled="false" />
                                                                                                                                                                        </AxisY>
                                                                                                                                                                    </asp:ChartArea>
                                                                                                                                                                </ChartAreas>
                                                                                                                                                            </asp:Chart>
                                                                                                                                                            <br />
                                                                                                                                                            <asp:Chart ID="chrtResultSTPTO" runat="server" AlternateText="HDFC" Visible="false"
                                                                                                                                                                BorderlineWidth="2" Width="650px" Height="580px" IsSoftShadows="false">
                                                                                                                                                                <Titles>
                                                                                                                                                                    <asp:Title Font="Arial, 7pt," Text="HDFC" ForeColor="17, 55, 129">
                                                                                                                                                                    </asp:Title>
                                                                                                                                                                </Titles>
                                                                                                                                                                <Legends>
                                                                                                                                                                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                                                                                                        Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 7pt, ">
                                                                                                                                                                    </asp:Legend>
                                                                                                                                                                </Legends>
                                                                                                                                                                <ChartAreas>
                                                                                                                                                                    <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="" AlignmentStyle="PlotPosition"
                                                                                                                                                                        BackSecondaryColor="White" BackColor="White" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                                                                                                                        BackHatchStyle="None" BorderDashStyle="Solid">
                                                                                                                                                                        <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="Time period">
                                                                                                                                                                            <LabelStyle Format="yyyy" />
                                                                                                                                                                            <ScaleBreakStyle Enabled="false" />
                                                                                                                                                                            <ScaleView SizeType="Years" />
                                                                                                                                                                            <MajorGrid Enabled="false" />
                                                                                                                                                                        </AxisX>
                                                                                                                                                                        <AxisY ArrowStyle="None" ToolTip="Value (Rs.)"  TextOrientation="Horizontal" LineColor="#013974" >
                                                                                                                                                                            <ScaleBreakStyle Enabled="True" />
                                                                                                                                                                            <MajorGrid Enabled="false" />
                                                                                                                                                                        </AxisY>
                                                                                                                                                                    </asp:ChartArea>
                                                                                                                                                                </ChartAreas>
                                                                                                                                                            </asp:Chart>
                                                                                                                                                            <%--<asp:Chart ID="chrt" runat="server" AlternateText="DWS Sip" Visible="false"
                                                                                                                                                            BorderlineColor="RoyalBlue" BorderlineWidth="2" Width="650px" Height="580px"
                                                                                                                                                            BackGradientStyle="Center" BackColor="Gray" IsSoftShadows="false">
                                                                                                                                                            <Titles>
                                                                                                                                                                <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                                                                                                                                                                    Text="HDFC SIP Chart" ForeColor="26, 59, 105">
                                                                                                                                                                </asp:Title>
                                                                                                                                                            </Titles>
                                                                                                                                                            <Legends>
                                                                                                                                                                <asp:Legend Enabled="true" IsTextAutoFit="true" Name="chrtLegend" BackColor="Transparent"
                                                                                                                                                                    Alignment="Center" LegendStyle="Row" Docking="Bottom" Font="Trebuchet MS, 7pt, ">
                                                                                                                                                                </asp:Legend>
                                                                                                                                                            </Legends>
                                                                                                                                                            <ChartAreas>
                                                                                                                                                                <asp:ChartArea Name="ChartArea1" BorderWidth="2" BorderColor="" AlignmentStyle="PlotPosition"
                                                                                                                                                                    BackSecondaryColor="White" BackColor="#ECF4F9" ShadowColor="Transparent" BackGradientStyle="Center"
                                                                                                                                                                    BackHatchStyle="None" BorderDashStyle="Solid" BackImageTransparentColor="#CCCCFF">
                                                                                                                                                                    <Area3DStyle Enable3D="false" Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                                                                                                                        WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                                                                                                                    <AxisX ArrowStyle="None" LineColor="#013974" ToolTip="SWP period">
                                                                                                                                                                        <ScaleBreakStyle Enabled="false" />
                                                                                                                                                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                                                                                    </AxisX>
                                                                                                                                                                    <AxisY ArrowStyle="None" ToolTip="Amount" LineColor="#013974">
                                                                                                                                                                        <ScaleBreakStyle Enabled="True" />
                                                                                                                                                                    </AxisY>
                                                                                                                                                                </asp:ChartArea>
                                                                                                                                                            </ChartAreas>
                                                                                                                                                        </asp:Chart>--%>
                                                                                                                                                            <%-- </div>--%>
                                                                                                                                                        </div>
                                                                                                                                                        <%--   <asp:Image runat="server" ID="imgchrtResult"  DescriptionUrl="~/HDFC_SIP/IMG/excel.png" />--%>
                                                                                                                                                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                                                                                                                                                    </asp:View>
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <asp:View ID="View3" runat="server">
                                                                                                                                                        <%--<div id="country3" class="tabcontent">--%>
                                                                                                                                                        <asp:GridView ID="GridViewSIPResult" runat="server" Width="100%" RowStyle-CssClass="grdRow" AlternatingRowStyle-CssClass="grdAltRow" HeaderStyle-CssClass="grdHead" AllowPaging="false" AutoGenerateColumns="false" OnRowDataBound="GridViewSIPResult_RowDataBound">
                                                                                                                                                            <Columns>
                                                                                                                                                                <asp:TemplateField HeaderText="">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)"%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="360px" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="1 Year">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString()+((Eval("1 Year").ToString()=="N/A")?"":"%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="3 Years">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="5 Years">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="Since Inception">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                            </Columns>
                                                                                                                                                        </asp:GridView>
                                                                                                                                                        <asp:GridView ID="GridViewSTPTOResult" runat="server" Width="98%"  AlternatingRowStyle-CssClass="grdAltRow" CssClass="grdHead" RowStyle-CssClass="grdRow" AllowPaging="false" AutoGenerateColumns="false">
                                                                                                                                                            <Columns>
                                                                                                                                                                <asp:TemplateField HeaderText="">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("Scheme_Name") == DBNull.Value) ? "--" : Eval("Scheme_Name").ToString() + " (CAGR)"%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="leftal" />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="1 Year">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="3 Year">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="5 Year">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField HeaderText="Since Inception">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                    <ItemStyle  />
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                            </Columns>
                                                                                                                                                        </asp:GridView>
                                                                                                                                                        <div id="divLsTable" runat="server">
                                                                                                                                                            <br />
                                                                                                                                                            <asp:GridView ID="GridViewResultLS" runat="server" Width="98%" AutoGenerateColumns="false" AlternatingRowStyle-CssClass="grdAltRow" CssClass="grdHead" RowStyle-CssClass="grdRow">
                                                                                                                                                                <Columns>
                                                                                                                                                                    <%--<asp:TemplateField
                                                                                                                        HeaderText="Type"> <ItemTemplate> <%# (Eval("Type") == DBNull.Value) ? "--" : Eval("Type").ToString()%>
                                                                                                                        </ItemTemplate> <ItemStyle HorizontalAlign="Left" /> </asp:TemplateField>--%>
                                                                                                                                                                    <asp:TemplateField HeaderText="Scheme Name">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("Scheme_Name") == DBNull.Value)
                                                                                                                                    ? "--" : Eval("Scheme_Name").ToString() +" (CAGR)" %>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle HorizontalAlign="Left" CssClass="leftal" Width="360px" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="1 Year">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%#
                                                                                                                                (Eval("1 Year") == DBNull.Value) ? "--" : Eval("1 Year").ToString() + ((Eval("1 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="3 Year">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%#
                                                                                                                                (Eval("3 Year") == DBNull.Value) ? "--" : Eval("3 Year").ToString() + ((Eval("3 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="5 Year">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%#
                                                                                                                                (Eval("5 Year") == DBNull.Value) ? "--" : Eval("5 Year").ToString() + ((Eval("5 Year").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                    <asp:TemplateField HeaderText="Since Inception">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <%# (Eval("Since Inception") == DBNull.Value) ? "--" : Eval("Since Inception").ToString() + ((Eval("Since Inception").ToString() == "N/A") ? "" : "%")%>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                        <ItemStyle CssClass="borderbottom" />
                                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                                </Columns>
                                                                                                                                                            </asp:GridView>
                                                                                                                                                        </div>
                                                                                                                                                        <%--</div>--%>
                                                                                                                                                    </asp:View>
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <asp:View ID="View4" runat="server">
                                                                                                                                                        <div id="Showpdfdiv" runat="server" class="FieldHead" style="border: 1px solid #113781; padding:5px;">
                                                                                                                                                            <%-- <b>Please select your Credential:</b>--%>
                                                                                                                                                            <table width="100%" style="padding-top: 20px;" >
                                                                                                                                                                <tr>
                                                                                                                                                                    <td align="left" width="100%">
                                                                                                                                                                        <asp:RadioButtonList ID="RadioButtonListCustomerType" runat="server" OnSelectedIndexChanged="RadioButtonListCustomerType_SelectedIndexChanged"
                                                                                                                                                                            TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true" BorderColor="#569fd3"
                                                                                                                                                                            BorderWidth="0" Width="250px" CssClass="lefft">
                                                                                                                                                                            <asp:ListItem>Distributor</asp:ListItem>
                                                                                                                                                                            <asp:ListItem Selected="true">Not a Distributor</asp:ListItem>
                                                                                                                                                                        </asp:RadioButtonList>
                                                                                                                                                                        <br />
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                            </table>
                                                                                                                                                            <div id="DistributerDiv" runat="server" visible="false">
                                                                                                                                                                <table id="tblDistb" width="50%" align="left" cellpadding="0" cellspacing="0">
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td width="32%" align="left">ARN No
                                                                                                                                                                        </td>
                                                                                                                                                                        <td width="68%" align="left">
                                                                                                                                                                            <%--<asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>--%>
                                                                                                                                                                            <asp:TextBox ID="txtArn" CssClass="form-control" runat="server" MaxLength="30" ></asp:TextBox>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td colspan="2">&nbsp;</td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td width="32%" align="left" >Prepared By
                                                                                                                                                                        </td>
                                                                                                                                                                        <td width="68%" align="left">
                                                                                                                                                                            <asp:TextBox ID="txtPreparedby" CssClass="form-control" runat="server" MaxLength="40"></asp:TextBox>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td colspan="2">&nbsp;</td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td width="32%" align="left" >Contact No <small>(Mobile)</small>
                                                                                                                                                                        </td>
                                                                                                                                                                        <td width="68%" align="left">
                                                                                                                                                                            <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server" MaxLength="14"></asp:TextBox>
                                                                                                                                                                            <%--onkeypress="return isNumber(event)"--%>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td colspan="2">&nbsp;</td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td width="32%" align="left" >Email
                                                                                                                                                                        </td>
                                                                                                                                                                        <td width="68%" align="left">
                                                                                                                                                                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" MaxLength="30"></asp:TextBox>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td colspan="2">&nbsp;</td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td width="32%" align="left" >Prepared For
                                                                                                                                                                        </td>
                                                                                                                                                                        <td width="68%" align="left">
                                                                                                                                                                            <asp:TextBox ID="txtPreparedFor" CssClass="form-control" runat="server" MaxLength="40"></asp:TextBox>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <%--<tr>
                                                                                                                                                                    <td>
                                                                                                                                                                        ARN No
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>
                                                                                                                                                                        <asp:TextBox ID="txtxARNo" runat="server"></asp:TextBox>
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>--%>
                                                                                                                                                                </table>
                                                                                                                                                            </div>
                                                                                                                                                            <table width="100%">
                                                                                                                                                                <tr>
                                                                                                                                                                        <td colspan="2">&nbsp;</td>
                                                                                                                                                                    </tr>
                                                                                                                                                                <tr>
                                                                                                                                                                    <td width="20%" align="left">Generate PDF Report:
                                                                                                                                                                    </td>
                                                                                                                                                                    <td width="68%" align="left">
                                                                                                                                                                        <asp:LinkButton ID="LinkButtonGenerateReport" runat="server" OnClick="LinkButtonGenerateReport_Click"
                                                                                                                                                                           ToolTip="Download PDF" OnClientClick="javascript:return pdfcheck();"><img src="IMG/downloadPDF.jpg" style="border: 0;" alt="" width="25" height="25" /></asp:LinkButton>
                                                                                                                                                                    </td>
                                                                                                                                                                </tr>
                                                                                                                                                            </table>
                                                                                                                                                        </div>
                                                                                                                                                    </asp:View>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </table>
                                                                                                                                    </asp:MultiView>
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        </div>
                                                                                                        </td>
                                                                                                        </tr>
                                                                                                        </table>
                                    <%-- <table width="98%" id="Table1" align="center">
                                                                                            <tr>
                                                                                                <td>--%>
                                                                                                    <div id="div1" runat="server" visible="false">
                                                                                                        <br />
                                
						      </div>
                    	        </div>

                                                    <%--Added Somabrata--%>
                                        </div>
                                    </div>
                    		    </div>
                            </div>
                            <div class="row">
                    		    <div class="col-lg-12">
                                <div id="disclaimerDiv" runat="server">
                                   <table>
                                                                                            <tr align="left">
                                                                                                <td valign="top" class="rslt_text1">
                                                                                                    <div align="justify" style="margin-top:15px;">
                                                                                                        <b>Disclaimer:</b>
                                                                                                        <asp:Label ID="LSDisc" runat="server" Text="<b><br/>* Returns here denote the Extended Internal Rate of Return (XIRR).  </br></b>"
                                                                                                            Visible="true"></asp:Label>
                                                                                                        <asp:Label ID="LSDisc1" runat="server" Text="<b><br/>* For Time Periods > 1 yr, CAGR Returns have been shown. For Time Periods < 1 yr, Absolute Returns have been shown. </br></b>"
                                                                                                            Visible="false"></asp:Label>
                                                                                                        <b>• Since Inception return of the benchmark is calculated from the scheme inception
                                                                                                        date.</b>
                                                                                                        <br />
                                                                                                        Past performance may or may not be sustained in the future and should not be used
                                                                                                    as a basis for comparison with other investments.
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left">
                                                                                                <td valign="top" class="rslt_text1">
                                                                                                    <div align="justify">
                                                                                                        <asp:Label ID="lblDisclaimer" runat="server" Text=""></asp:Label>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <img src="IMG/spacer11.gif" width="1" height="4" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left">
                                                                                                <td align="left" valign="top" class="rslt_text1">
                                                                                                    <div align="justify">
                                                                                                        The return calculator has been developed and is maintained by ICRA Analytics Limited.
                                                                                                    HDFC AMC./Trustees do not endorse the authenticity
                                                                                                    or accuracy of the figures based on which the returns are calculated; nor shall
                                                                                                    they be held responsible or liable for any error or inaccuracy or for any losses
                                                                                                    suffered by any investor as a direct or indirect consequence of relying upon the
                                                                                                    data displayed by the calculator.
                                                                                                    </div>
                                                                                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="left">
                                                                                                        <tr>
                                                                                                            <td class="text" align="right"></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="text" align="right">
                                                                                                                <span style="text-align: right" class="rslt_text1">Developed by:<a class="text" href="https://www.icraanalytics.com"
                                                                                                                    target="_blank"> ICRA Analytics Ltd </a>, <a href="https://icraanalytics.com/home/Disclaimer"
                                                                                                            target="_blank">Disclaimer </a></span>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                </div>
                                </div>
                                </div>

                    </div>
                    <div class="col-lg-2"></div>
                    </div>
                </div>
                
			</section>
    <!-- END CONTENT -->
    <!-- BEGIN SCROLL TO TOP -->
    <div class="scroll-to-top">
    </div>
    <!-- END SCROLL TO TOP -->
    <!-- BEGIN JS FRAMEWORK -->
    <%--Somabrata Add Later--%>
    <div class="modal fade" id="modalSuccess" tabindex="-1" role="dialog" aria-labelledby="myModalLabel3"
        aria-hidden="true">
        <div class="modal-wrapper">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;</button>
                        <h4 class="modal-title" id="myModalLabel3">
                            Add Scheme to Compare</h4>
                    </div>
                    <div class="modal-body">
                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Fund House</label>
                            <div class="col-sm-9">
                                <select class="form-control">
                                    <option>HDFC Mutual Fund</option>
                                    <option>Birla Sun Life Mutual Fund</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Structure</label>
                            <div class="col-sm-9">
                                <select name="ddlPeriod" id="ddlPeriod" class="form-control">
                                    <option value="Per_7_Days">Close Ended</option>
                                    <option value="Per_30_Days">Interval</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group" id="trCategory" runat="server">
                            <label class="col-sm-3 control-label">
                                Category</label>
                            <div class="col-sm-9">
                                <select name="ddlPeriod" id="ddlPeriod" class="form-control">
                                    <option value="Per_7_Days">Balanced</option>
                                    <option value="Per_30_Days">Debt</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Sub Category</label>
                            <div class="col-sm-9">
                                <select name="ddlPeriod" id="ddlPeriod" class="form-control">
                                    <option value="Per_7_Days">Select Sub Category</option>
                                    <option value="Per_30_Days">Debt</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Scheme Name</label>
                            <div class="col-sm-9">
                                <select name="ddlPeriod" id="ddlPeriod" class="form-control">
                                    <option value="Per_7_Days">HDFC Balanced Fund - Growth</option>
                                    <option value="Per_30_Days">Debt</option>
                                </select>
                            </div>
                        </div>
                        
                    </div>
                    <div class="modal-footer">
                        <div class="btn-group">
                            <button type="button" class="btn btn-success">
                                Add</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                Reset</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
