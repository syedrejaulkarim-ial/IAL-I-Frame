<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NFO.aspx.cs"
    Inherits="iFrames.BlueChip.NFO" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>NFO Update</title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />


    <link rel="stylesheet" href="css/new/jquery-ui.css" />
    <link rel="stylesheet" href="css/new/all.css" />
    <link rel="stylesheet" href="css/new/bootstrap-datepicker.css" />
    <link type="text/css" rel="stylesheet" href="css/bootstrap-multiselect.css" />

    <script type="text/javascript">
        function pop() {
            $.blockUI({ message: '<div style="font-size:16px; font-weight:bold">Please wait...</div>' });
        }

        function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
                return true;
            }
            return false;
        }
    </script>





    <script type="text/javascript">
        function dateValidation() {
            var date = $('#txtfromDate').val();
            var Day = parseInt(date.substring(0, 2), 10);
            var Mn = parseInt(date.substring(3, 5), 10);
            var Yr = parseInt(date.substring(6, 10), 10);
            var DateVal = Mn + "/" + Day + "/" + Yr;

            //if (!isNaN(Date.parse(DateVal))) {
            //    document.getElementById('txtfromDate').focus();
            //    alert("Invalid format in from date");
            //    return false;
            //}

            var dt = new Date(DateVal);

            var date1 = $('#txtToDate').val();
            var Day1 = parseInt(date1.substring(0, 2), 10);
            var Mn1 = parseInt(date1.substring(3, 5), 10);
            var Yr1 = parseInt(date1.substring(6, 10), 10);
            var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;

            //if (!isNaN(Date.parse(DateVal1))) {
            //    document.getElementById('txttoDate').focus();
            //    alert("Invalid format in To date");
            //    return false;
            //}

            var dt1 = new Date(DateVal1);

            if (dt > dt1) {
                alert("End date should be greater than Start date");
                return false;
            }
        };
    </script>

    <style>
        .labelComment {
            align-items: flex-end;
        }

        #rdbReinvest {
            /* width: auto !important; */
            margin-right: 10px;
        }

        #rdbPayout {
            width: auto !important;
            margin-right: 10px;
            margin-left: 15px;
        }

        #lmprdbReinvest {
            width: auto !important;
        }

        #lmprdbPayout {
            width: auto !important;
        }

        .radio label, .checkbox label {
            min-height: 20px;
            padding-left: 20px;
            margin-bottom: 0;
            font-weight: 400;
            cursor: pointer;
        }

        .text-label {
            font-weight: bold;
            color: #e46812;
            font-size: 14px;
        }

        .nav-tabs .nav-link.active {
            color: #fff;
            background-color: #e46812;
        }

        .nav-tabs .nav-link.active {
            background-color: #e46812;
            color: #fff;
        }

        #RadioButtonListCustomerType_0 {
            margin-right: 10px
        }

        #RadioButtonListCustomerType_1 {
            margin-right: 10px
        }

        .text-center {
            text-align: center;
        }

        .ui-datepicker-trigger {
            position: absolute;
            top: 31px;
            right: 20px;
        }

        .popover__title {
            font-size: 24px;
            line-height: 36px;
            text-decoration: none;
            color: rgb(228, 68, 68);
            text-align: center;
        }

        .popover__wrapper {
            position: relative;
            /*margin-top: 1.5rem;*/
            display: inline-block;
        }

        .popover__content {
            opacity: 0;
            visibility: hidden;
            position: absolute;
            left: -190px;
            transform: translate(0, 10px);
            background-color: #fff;
            padding: 1.5rem;
            box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.26);
            width: 450px;
        }

            .popover__content:before {
                position: absolute;
                z-index: -1;
                content: "";
                left: calc(40% - 48px);
                top: -7px;
                border-style: solid;
                border-width: 0 10px 10px 10px;
                border-color: transparent transparent #fff transparent;
                transition-duration: 0.3s;
                transition-property: transform;
            }

        .popover__wrapper:hover .popover__content {
            z-index: 10;
            opacity: 1;
            visibility: visible;
            transform: translate(0, -20px);
            transition: all 0.5s cubic-bezier(0.75, -0.02, 0.2, 0.97);
        }

        .popover__message {
            text-align: left;
        }

        .dropdown-menu > .active > a, .dropdown-menu > .active > a:hover, .dropdown-menu > .active > a:focus {
            color: #fff;
            text-decoration: none;
            background-color: transparent;
            outline: 0;
        }

        .multiselect-container {
            padding: 5px;
            height: 300px;
            overflow-y: scroll;
            overflow-x: hidden;
        }

        .checkbox {
            position: relative;
            display: block;
            margin-top: 1px;
            margin-bottom: 1px;
        }

        .btn-group-justified > .btn, .btn-group-justified > .btn-group {
            display: table-cell;
            float: none;
            width: 100%;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="card">
                <div class="card-header bg-grey">
                    <p>NFO Update</p>
                </div>
                <div class="card-body">
                    <div class="card">
                        <div class="card-body">

                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0" style="display: none">
                                <div class="row">
                                    <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                        <label id="">Category</label>
                                        <%--  <select id="ddlNature" class="form-control form-control-sm" runat="server" onselectedindexchanged="ddlNature_SelectedIndexChanged" autopostback="True" datatextfield="Select Nature Name">
                                                            <option selected="selected" value=" Select AMC Name">Select
                                                                Nature Name
                                                            </option>
                                                            <option value="3">Aditya Birla Sun Life Mutual Fund</option>
                                                            <option value="46">Axis Mutual Fund</option>
                                                            <option value="4">Baroda Mutual Fund</option>
                                                        </select>--%>
                                        <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                            AutoPostBack="true" class="form-control form-control-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                        <label>Sub Category</label>
                                        <asp:DropDownList ID="ddlSubNature" runat="server"
                                            class="form-control form-control-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                        <label>Type</label>
                                        <%--<select id="ddlType" class="form-control form-control-sm" runat="server">
                                                            <option selected="selected" value=" Select Type Name">Select
                                                                Type Name
                                                            </option>
                                                            <option value="3">Aditya Birla Sun Life Mutual Fund</option>
                                                            <option value="46">Axis Mutual Fund</option>
                                                            <option value="4">Baroda Mutual Fund</option>
                                                        </select>--%>
                                        <asp:DropDownList ID="ddlType" runat="server"
                                            class="form-control form-control-sm">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdIsLoad" runat="server" Value="0" />
                                        <asp:HiddenField ID="Userid" runat="server" Value="asas" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0">
                                    <div class="row">
                                        <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                            <label>Mutual Fund Name</label>
                                            <%--<select id="ddlFundHouse" class="form-control form-control-sm" runat="server">
                                                                <option selected="selected" value=" Select AMC Name">Select AMC Name
                                                                </option>
                                                                <option value="3">Aditya Birla Sun Life Mutual Fund</option>
                                                                <option value="46">Axis Mutual Fund</option>
                                                                <option value="4">Baroda Mutual Fund</option>
                                                            </select>--%>
                                            <div>
                                                <asp:ListBox ID="ddlFundHouse" runat="server"
                                                    class="form-control form-control-sm" SelectionMode="multiple"></asp:ListBox>
                                            </div>

                                        </div>
                                        <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                            <label>Start Date</label>
                                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control form-control-sm datepicker-input" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-xs-6 col-sm-6 col-md-4 col-lg-4">
                                            <label>End Date</label>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker-input form-control form-control-sm" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 p-0" style="display: none">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-7 col-lg-7">
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-5 col-lg-5 text-right">
                                        <asp:Button ID="btnSubmit" class="btn btn-success"
                                            Style="margin-right: 10px"
                                            OnClientClick="javascript: return dateValidation();"
                                            runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                        <asp:Button ID="btnReset" class="btn btn-light" Style="margin-right: 15px;"
                                            runat="server" Text="Reset" OnClick="btnReset_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 pt-4"
                                id="Result" runat="server">
                                <div class="row">
                                    <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                                        <LayoutTemplate>
                                            <table class="table table-bordered" width="100%" cellpadding="0"
                                                cellspacing="0" border="0">
                                                <thead>
                                                    <tr>
                                                        <th>

                                                            <asp:Label ID="lnkFundName" runat="server" SkinID="lblHeader"
                                                                CommandName="Sort" CommandArgument="FundName"
                                                                Text="Fund Name" /></th>
                                                        <th style="text-align: center">
                                                            <asp:Label ID="Label1" runat="server" SkinID="lblHeader"
                                                                CommandName="Sort" CommandArgument="FundName"
                                                                Text="Category" /></th>
                                                        <th style="text-align: center">
                                                            <asp:Label ID="Label2" runat="server" SkinID="lblHeader"
                                                                CommandName="Sort" CommandArgument="FundName"
                                                                Text="Open Date" /></th>
                                                        <th style="text-align: center">
                                                            <asp:Label ID="Label3" runat="server" SkinID="lblHeader"
                                                                CommandName="Sort" CommandArgument="FundName"
                                                                Text="Close Date" /></th>
                                                        <th style="text-align: center">
                                                            <asp:Label ID="Label4" runat="server" SkinID="lblHeader"
                                                                CommandName="Sort" CommandArgument="FundName"
                                                                Text="Fund Manager" /></th>
                                                        <th style="text-align: center">
                                                            <asp:Label ID="Label5" runat="server" SkinID="lblHeader"
                                                                CommandName="Sort" CommandArgument="FundName"
                                                                Text="Fund Objective" /></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div>
                                                <table>
                                                    <tr>

                                                        <div style="width: 100%; float: left">
                                                            <asp:DataPager ID="dpTopFund" runat="server" PageSize="10"
                                                                PagedControlID="lstResult">
                                                                <Fields>
                                                                    <asp:NextPreviousPagerField ShowFirstPageButton="True"
                                                                        ShowNextPageButton="False" />
                                                                    <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                                                    <asp:NextPreviousPagerField ShowLastPageButton="True"
                                                                        ShowPreviousPageButton="False" />
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <span style="padding-left: 40px; text-align: center">Page
                                                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
                                                                                of
                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                                                                (
                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                                                                records)
                                    <br />
                                                                            </span>
                                                                        </PagerTemplate>

                                                                    </asp:TemplatePagerField>

                                                                </Fields>

                                                            </asp:DataPager>
                                                        </div>
                                                        <div style="font-size: 10px; color: #A7A7A7">
                                                            Developed by: <a href="https://www.icraanalytics.com"
                                                                target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd</a>, <a style="font-size: 10px; color: #999999"
                                                                    href="https://icraanalytics.com/home/Disclaimer"
                                                                    target="_blank">Disclaimer </a>
                                                        </div>
                                                    </tr>
                                                </table>
                                            </div>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdSchID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FUND_ID")%>' />
                                            <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "" : "" %>'>
                                                <%--<td>
                                                                        <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                                                    </td>--%>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td class="clsImgAddObjective" data-toggle="collapse"
                                                                    data-target="#ID_<%#Eval("FUND_ID")%>" style="border: none; background: transparent;"><%#Eval("FUNDNAME")%>
                                                                    <div class="popover__wrapper">
                                                                        <a href="#">
                                                                            <img class="popover__title clsImgAddScheme" src="Images/Add.png"
                                                                                data-original-title="">
                                                                        </a>
                                                                        <div class="popover__content">
                                                                            <asp:Repeater ID="innerRepeater" runat="server">
                                                                                <HeaderTemplate>
                                                                                    <table width="100%" cellpadding="0" cellspacing="0"
                                                                                        class="table table-bordered">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <th width="90%">Scheme Name</th>
                                                                                                <th style="text-align: right" width="10%">Min Investment
                                                                                                    (₹)
                                                                                                </th>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                        <tbody>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label7" runat="server" Text='<%#Eval("Sch_Short_Name") %>' />
                                                                                        </td>
                                                                                        <td style="text-align: right;">
                                                                                            <asp:Label ID="Label6" runat="server" Text='<%#Eval("Min_Investment") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    </tbody>
                                                                                            </table>
                                                                                </FooterTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                                <td style="border-top: transparent; background: transparent; display: none">
                                                                    <div class="clsSchemes" id="ID_<%#Eval("FUND_ID")%>">
                                                                        <table width="100%" cellpadding="0" cellspacing="0"
                                                                            class="table table-bordered">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <th width="70%">Scheme Name
                                                                                    </th>
                                                                                    <th width="30%">Min Invetsment (₹)
                                                                                    </th>
                                                                                </tr>
                                                                            </tbody>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td>Aditya Birla Sun Life FTP - Series TC - Dir - Dividend
                                                                                    </td>
                                                                                    <td style="text-align: right; padding-right: 50px;">1000
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>

                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <%--<asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />--%>

                                                    <%#Eval("Sebi_Nature").ToString() != "" ? Eval("Sebi_Nature") : ""%> - <%#Eval("SEBI_SUB_NATURE").ToString() != "" ? Eval("SEBI_SUB_NATURE") : ""%>
                                                </td>
                                                <td align="center">
                                                    <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                    <%#Eval("ISSUEOPEN").ToString() != "" ? Eval("ISSUEOPEN", "{0:dd/MM/yyyy}") : "NA"%>
                                                </td>
                                                <td align="center" style="text-align: center">
                                                    <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                    <%#Eval("ISSUECLOSE").ToString() != "" ?  Eval("ISSUECLOSE", "{0:dd/MM/yyyy}")  : "NA"%>
                                                </td>
                                                <td align="center">
                                                    <%--<asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />--%>

                                                    <%#Eval("FUND_MANAGER").ToString() != "" ? Eval("FUND_MANAGER") : "NA"%>
                                                </td>
                                                <td style="text-align: center;">
                                                    <%--<div>
                                                        <a href="#">
                                                            <img class="clsImgAddObjective" src="Images/Add.png">&nbsp;View
                                                        </a>
                                                        <div class="popover__content">
                                                            <div class="clsFundObjective">
                                                                <div width="100%">
                                                                    <%#Eval("OBJECTIVE").ToString() != "" ? Eval("OBJECTIVE") : "NA"%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>--%>
                                                     <a data-container="body"
                                                        data-toggle="popover" data-trigger="hover" data-placement="bottom" data-content="<%#Eval("OBJECTIVE").ToString() != "" ? Eval("OBJECTIVE") : "NA"%>" data-animation="true" style="cursor: pointer">
                                                         <img class="clsImgAddObjective" src="Images/Add.png">&nbsp;View
                                                    </a>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            Data not Found
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="width: 100%; float: right; text-align: right; font-size: 10px; color: #A7A7A7">
                Developed by: <a href="https://www.icraanalytics.com"
                    target="_blank" style="font-size: 10px; color: #999999">ICRA Analytics Ltd
                </a>, <a style="font-size: 10px; color: #999999"
                    href="https://icraanalytics.com/home/Disclaimer"
                    target="_blank">Disclaimer</a>
            </div>
        </div>
    </form>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>
    <script>
        $(function () {
            $('[data-toggle="popover"]').popover()
        })

        //$(document).ready(function () {

        //    $('[data-toggle="popover"]').popover({
        //        placement: 'bottom',
        //        delay: {
        //            "hide": 100
        //        }
        //    });

        //    $('[data-toggle="popover"]').click(function () {

        //        setTimeout(function () {
        //            $('.popover').fadeOut('slow');
        //        }, 5000);

        //    });

        //});

        $("#popover").popover({ trigger: "hover" });
    </script>

</body>
</html>
