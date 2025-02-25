<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewreport.aspx.cs"
    Inherits="iFrames.DSPApp.Viewreport" EnableEventValidation="false"%>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/lib/jquery.nanoscroller/javascripts/jquery.nanoscroller.min.js"
        type="text/javascript"></script>
    <%--<script src="js/main.js" type="text/javascript"></script>--%>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="js/date.js" type="text/javascript"></script>
    <script src="assets/lib/theme-switcher/theme-switcher.min.js" type="text/javascript"></script>
    <script src="js/jquery.multiple.select.js" type="text/javascript"></script>
    <script src="js/jquery.nanoscroller.min.js"></script>
    <script type='text/javascript'>
        function validate() {
            if (document.getElementById("EventDate").value == "") {
                alert("Please select Event Date");
                document.getElementById("EventDate").focus();
                return false;
            }

            if (document.getElementById("ddlCategory").value == "--- Select Category----") {
                alert("Please select Category");
                document.getElementById("ddlCategory").focus();
                return false;
            }
        }
    </script>
    <title>View Report</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="assets/img/favicon.png">
    <!--    <link rel="stylesheet" type="text/css" href="css/style.css"/>
    <link rel="stylesheet" type="text/css" href="assets/lib/jquery.nanoscroller/css/nanoscroller.css"/>-->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link type="text/css" href="css/style.css" rel="stylesheet">
    <link href="css/simple-line-icons.css" rel="stylesheet">
    <link href="font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet">
    <link href="css/datepicker.css" rel="stylesheet" />
    <link href="css/multiSelectDropdown/multiple-select.css" rel="stylesheet" />
    <style type="text/css">
        .gridheader
        {
            cursor: hand;
        }
        th.sortasc a  
        {
            display:block; padding:0 4px 0 15px; 
            background:url(img/asc.gif) no-repeat;  
        }

        th.sortdesc a 
        {
            display:block; padding:0 4px 0 15px; 
            background:url(img/desc.gif) no-repeat;
        }
    </style>
</head>
<body>
    <form id="form1" class="form-horizontal group-border-dashed" runat="server">

    <div class="am-wrapper">
        <nav class="navbar navbar-default navbar-fixed-top am-top-header">
        <div class="container-fluid">
          <div class="navbar-header">
            <div class="page-title"><span>Dashboard</span></div><a href="#" class="am-toggle-left-sidebar navbar-toggle collapsed"><span class="icon-bar"><span></span><span></span><span></span></span></a><a href="index.php" class="navbar-brand"></a>
          </div><a href="#" class="am-toggle-right-sidebar"><span class="icon s7-menu2"></span></a><a href="#" data-toggle="collapse" data-target="#am-navbar-collapse" class="am-toggle-top-header-menu collapsed"> <i class="fa fa-angle-down"></i></a>
          <div id="am-navbar-collapse" class="collapse navbar-collapse">
            <ul class="nav navbar-nav navbar-right am-user-nav">
              <li class="dropdown">
              <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle"><%= Session["EmailId"] %>
              
             <i class="fa fa-angle-down"></i></a>
                <ul role="menu" class="dropdown-menu">
                	 <li><a href="ForgetPassword.aspx"> <span class="icon-key"></span>Reset Password</a></li>
                 
                  <li><a href="Login.aspx?param=logout"> <span class="icon-logout"></span>Sign Out</a></li>
                </ul>
              </li>
            </ul>
            <!--<ul class="nav navbar-nav am-nav-right">
              <li><a href="#">Home</a></li>
              <li><a href="#">About</a></li>
              <li class="dropdown"><a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="dropdown-toggle">Services <span class="angle-down s7-angle-down"></span></a>
                <ul role="menu" class="dropdown-menu">
                  <li><a href="#">UI Consulting</a></li>
                  <li><a href="#">Web Development</a></li>
                  <li><a href="#">Database Management</a></li>
                  <li><a href="#">Seo Improvement</a></li>
                </ul>
              </li>
              <li><a href="#">Support</a></li>
            </ul>-->
            
          </div>
        </div>
      </nav>
        <div class="am-left-sidebar">
            <div class="content">
                <div class="am-logo">
                </div>
                <ul class="sidebar-elements">
                    <li id="liUserMngmnt" runat="server" class="parent"><a href="#"><i class="icon-user">
                    </i><span>User Management</span></a>
                        <ul class="sub-menu">
                            <li style="list-style-type: none;"><a href="CreateUser.aspx">User Creation</a> </li>
                            <li class="active" style="list-style-type: none;"><a href="Active.aspx">Active/Inactive
                                Login</a> </li>
                            <li style="list-style-type: none;"><a href="LoginHistory.aspx">Login History</a>
                            </li>
                        </ul>
                    </li>
                    <li class="parent"><a href="#"><i class="icon-magnifier"></i><span>Return Analysis</span></a>
                        <ul class="sub-menu">
                            <li id="liUploadExl" runat="server" style="list-style-type: none;"><a href="upload.aspx">
                                Upload Excel File </a></li>
                            <li style="list-style-type: none;"><a href="viewreport.aspx">View Report</a> </li>
                        </ul>
                    </li>
                </ul>
                <!--Sidebar bottom content-->
            </div>
        </div>
        <div class="am-content">
            <div class="page-head">
                <h3>
                    View Report
                </h3>
                <ol class="breadcrumb">
                    <li><a href="#">Return Analysis</a></li>
                    <li class="active.html">View Report </li>
                </ol>
            </div>
            <div class="main-content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default panel-heading-fullwidth panel-borders">
                            <div class="panel-heading">
                                <h3>
                                    View Report</h3>
                            </div>
                            <div class="panel-body">
                                <form action="#" style="border-radius: 0px;" class="form-horizontal group-border-dashed">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Select Date</label>
                                    <div class="col-sm-6">
                                        <!--<div data-min-view="2" data-date-format="yyyy-mm-dd" class="input-group date datetimepicker col-md-10 col-xs-7">
                          <input size="16" type="text" value="" class="form-control"><span class="input-group-addon btn btn-primary"><i class="icon-calendar"></i></span>
                        </div>-->
                                        <div data-min-view="2" class='input-group date datetimepicker col-md-10 col-xs-7'
                                            id='datetimepicker1'>
                                            <input type='text' id="EventDate" runat="server" class="form-control" />
                                            <%-- <span class="input-group-addon">
                                                <i class="fa fa-calendar" id="iconCalenderEventDate"  title="Select event date" style="cursor: pointer;"></i>
                                            </span>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Select Category</label>
                                    <div class="col-sm-5">
                                        <!--  <select class="form-control">
                              <option>LIQUID (D)</option>
                              <option>ULTRA SHORT (D)</option>
                                
                           </select>-->
                                        <asp:DropDownList ID="ddlCategory" runat="server" class="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                        Return Period</label>
                                    <div class="col-sm-5">
                                        <!--<select multiple="multiple" name="MutualFundMultiSel" id="MutualFundMultiSel" style="width: 100%;">
                               <option value="57">1 Day</option>
                               <option value="5">3 Days</option>
                               <option value="11">1 Week</option>
                               <option value="26">3 Weeks</option>
                               <option value="27">1 Month</option>
                               <option value="30">3 Months</option>
                               <option value="31">6 Months</option>
                               <option value="32">1 Year</option>
                               <option value="62">3 Years</option>
                               <option value="45">5 Years</option>
                              </select>  -->
                                        <asp:DropDownList ID="ddlperiod" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">
                                    </label>
                                    <div class="col-sm-6">
                                        <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" OnClick="btnSearch_Click"
                                            Text="Show" OnClientClick="getSelectedPeriod();" />
                                        <asp:Button ID="btnReset" class="btn btn-primary" runat="server" OnClick="btnReset_Click"
                                            Text="Reset" OnClientClick="return resetClick();" />
                                    </div>
                                </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lblrptDate" runat="server" Style="text-align: center"></asp:Label>
                    <!--Responsive table-->
                    <div class="col-sm-12">
                        <div class="widget widget-fullwidth widget-small">
                                    <div class="table-responsive" id="GridContaner" style="overflow-x: hidden;">
                                        <asp:GridView ID="dgViewStatus" runat="server" CssClass="scroll" OnRowDataBound="dgViewStatus_RowDataBound"
                                            CellPadding="4" EnableModelValidation="True" GridLines="None" ForeColor="#333333"
                                            PageSize="15" OnPageIndexChanging="dgViewStatus_PageIndexChanging" 
                                            OnSorting="dgViewStatus_Sorting" EnableViewState="False">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#358399" Font-Size="Small" />
                                            <%--<Columns>
                        <asp:BoundField />
                    </Columns>--%>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#70BDD1" ForeColor="White" Font-Bold="True" />
                                            <HeaderStyle BackColor="#FF808080" Font-Bold="False" ForeColor="White" CssClass="GridHeader" />
                                            <SortedAscendingHeaderStyle CssClass="sortasc" />
                                            <SortedDescendingHeaderStyle CssClass="sortdesc" />
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First"
                                                LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" />
                                            <PagerStyle BorderColor="Black" BorderStyle="None" Font-Size="20px" BackColor="#70BDD1"
                                                ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Small" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <EmptyDataTemplate>
                                                <strong>No Data Found...</strong>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                    <div id="GvwPaging">
                                    </div>
                            <div class="divgrid" id="sumdiv" style="overflow: auto">
                                <asp:GridView ID="GridViewSummary" runat="server" CssClass="GridHeader" OnRowDataBound="GridViewSummary_RowDataBound"
                                    CellPadding="4" EnableModelValidation="True" GridLines="None" ForeColor="#333333"
                                    PageSize="15" EnableViewState="False">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#358399" Font-Size="Small" />
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#70BDD1" ForeColor="White" Font-Bold="True" />
                                    <HeaderStyle BackColor="#FF808080" Font-Bold="False" ForeColor="White" CssClass="GridHeader" />
                                    <PagerStyle BorderColor="Black" BorderStyle="None" BackColor="#70BDD1" ForeColor="White"
                                        HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Small" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <EmptyDataTemplate>
                                        <strong>No Data Found...</strong>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="widget widget-fullwidth widget-small">
                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" runat="server" CssClass="GridHeader" OnRowDataBound="GridView1_RowDataBound"
                                    CellPadding="4" EnableModelValidation="True" GridLines="None" ForeColor="#333333"
                                    PageSize="15" EnableViewState="False">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#358399" Font-Size="Small" />
                                    <Columns>
                                        <asp:BoundField />
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#70BDD1" ForeColor="White" Font-Bold="True" />
                                    <HeaderStyle BackColor="#FF808080" Font-Bold="False" ForeColor="White" CssClass="GridHeader" />
                                    <PagerStyle BorderColor="Black" BorderStyle="None" BackColor="#70BDD1" ForeColor="White"
                                        HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Small" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <EmptyDataTemplate>
                                        <strong>No Data Found...</strong>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidSelectedReturnPeriod" runat="server" />
        <asp:HiddenField ID="hidSelectedReturnPeriodSpan" runat="server" />
        <asp:HiddenField ID="HDGrdColCount" runat="server" />
    </div>
    <script src="js/main.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //initialize the javascript
            App.init();
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            App.livePreview();
        });

    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            alert("sldhl");
            $('#ddlperiod').val("");
            $('#ddlperiod').multipleSelect({
            });

        });

        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
          m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-68396117-1', 'auto');
        ga('send', 'pageview');

        $('#EventDate').datepicker({
            format: 'dd M yyyy',
            autoclose: true,
        });


        $('#ddlperiod').multipleSelect({
        });

         if ($("#ddlCategory").val() != "Summary") {
            if ($("#dgViewStatus").length > 0) {
                
                $('#dgViewStatus table').each(function(){ 
                     $("#GvwPaging").html(this.innerHTML);
                 });
                 $('#GvwPaging td').each(function(){  
                    var el = $(this);
                    el.css("font-size","17px");
                    el.css("padding-left","5px");
                    el.css("padding-right","5px"); 
                 });

                $("#GvwPaging").css("background-color","#70BDD1");
                
                //alert($("#GvwPaging").html());
                $('#dgViewStatus table').hide();

                $("#GridContaner").attr('style',"overflow-x:hidden");              
                $("#dgViewStatus").attr('style', "table-layout: fixed");

                var GVMaintainReceiptMaster = document.getElementById('<%= dgViewStatus.ClientID %>');

                var tab = $("#<%= dgViewStatus.ClientID%>").clone(true);
                var tabfreeze = $("#<%= dgViewStatus.ClientID%>").clone(true);
                var totalwidth = $("#<%= dgViewStatus.ClientID%>").outerWidth();
                var totalwidth4Mob = $("#<%= dgViewStatus.ClientID%>").parent().outerWidth();
               // alert(totalwidth4Mob);
                var firstcolumnwidth = $("#<%= dgViewStatus.ClientID%> th:nth-child(2)").outerWidth();

                firstcolumnwidth = 410;
                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                    firstcolumnwidth = 150;
                }
                
                tabfreeze.width(firstcolumnwidth);
                tab.width(totalwidth - firstcolumnwidth);

                tabfreeze.find("th:gt(1)").remove();
                tabfreeze.find("td:not(:nth-child(2))").remove();
                tab.find("th:nth-child(2)").remove();
                tab.find("td:nth-child(2)").remove();
               
                var ColWidth = totalwidth - 410;
                var MobColWidth = totalwidth4Mob - 150;
                //MobColWidth = MobColWidth * (1 / 3);
                //MobColWidth = MobColWidth - 70;
                var conatiner="";
                conatiner = $('<table border="0" cellpadding="0" cellspacing="0"><tr><td valign="top"><div id="FCol" ></div></td><td valign="top" style="overflow:auto; width:' + ColWidth + 'px"><div id="Col" style="overflow:auto; width:' + ColWidth + 'px" ></div></td></tr></table>');

                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                    conatiner = $('<table border="0" cellpadding="0" cellspacing="0"><tr><td valign="top"><div id="FCol" ></div></td><td valign="top" style="overflow:auto; width:' + MobColWidth + 'px"><div id="Col" style="overflow:auto; width:' + MobColWidth + 'px" ></div></td></tr></table>');
                }
                $("#FCol", conatiner).html($(tabfreeze));
                $("#Col", conatiner).html($(tab));
                $("#GridContaner").html('');
                $("#GridContaner").append(conatiner);

            }
        }
       

    //Aded by sop
    $("input[type='checkbox'][name='selectAllddlperiod']").click(function () {
        var toggle = this.checked;
        $("input[type='checkbox'][name='selectItemddlperiod']").attr("checked", toggle);
    });

    $("input:checkbox[name='selectItemddlperiod']").click(function () {
        var toggle = this.checked;
        $(this).attr('checked', toggle);
    });

    function getSelectedPeriod() {
        //var selectedPeriod = '';
        var allSelectedPeriod = [];
        $("input:checkbox[name='selectItemddlperiod']:checked").each(function () {
            //selectedPeriod = selectedPeriod + $(this).val()+',';
            allSelectedPeriod.push($(this).val());
        });
        $('#hidSelectedReturnPeriod').val(allSelectedPeriod);
        $('#hidSelectedReturnPeriodSpan').val($(".ms-choice").find("span").text());
        return (true);
    }
    if ($('#hidSelectedReturnPeriod').val() != '') {
        var strSlPeriod = $('#hidSelectedReturnPeriod').val();
        var strSlPeriodSpan = $('#hidSelectedReturnPeriodSpan').val();
        var arr = strSlPeriod.split(',');
        $.each(arr, function (index, item) {
            //alert(substr[index]);
            //$("input:checkbox[value=2]").attr("checked", true);
            $("input:checkbox[name='selectItemddlperiod'][value='" + item + "']").attr("checked", true);
        });
        //$(".ms-choice").find("span").text(strSlPeriod);
        $(".ms-choice").find("span").text(strSlPeriodSpan);
    }

    function resetClick() {
        $('#hidSelectedReturnPeriod').val('');
        $('#hidSelectedReturnPeriodSpan').val('');
        $("input:checkbox[name='selectItemddlperiod']").each(function () {
            $("input:checkbox[name='selectItemddlperiod']").attr("checked", false);
        });
        return (true);
    }
    //End
    </script>
    <script type="text/javascript">
        function showFactsheet(EvId) {
            window.open("Factsheet.aspx?param=" + EvId, "", "location=1,status=1,scrollbars=1,resizable=1 ", "");
        }
    </script>
    </form>
</body>
</html>
