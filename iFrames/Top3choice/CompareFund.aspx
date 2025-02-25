<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="CompareFund.aspx.cs" Inherits="iFrames.Top3choice.CompareFund1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html class="no-js">

<head>
    <title>Compare Fund</title>
    <!-- Bootstrap -->
    <script type='text/javascript' src="js/jquery-1.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type='text/javascript' src="js/bootstrap.js"></script>
    <link rel="stylesheet" href="CSS/jquery-ui-1.10.3.custom.min.css" />
    <link rel="stylesheet" href="CSS/jquery-slider.css" />
    <link href="CSS/auto.css" rel="stylesheet" type="text/css" />
    <link href="CSS/dspstyles.css" rel="stylesheet" type="text/css" media="all" />
    <link href="CSS/tabcontent.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/date.js"></script>
    <script src="js/AutoComplete.js" type="text/javascript"></script>

    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/DT_bootstrap.css" rel="stylesheet" media="screen" />
    <link href="bootstrap/css/styles.css" rel="stylesheet" media="screen" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css' />
    <script src="js/modernizr-respond-1.min.js" type="text/javascript"></script>
    <style type="text/css">
        body {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
            background:transparent;
			font-family:Arial, Helvetica, sans-serif;
			font-size:12px;
        }
		a:link{border:none;}

        .layout {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }

        .layout-slider {
            width: 100px;
        }

        .layout-slider-settings {
            font-size: 12px;
            padding-bottom: 0px;
        }

            .layout-slider-settings pre {
                font-family: arial;
            }

        .layout1 {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }

        .layout-slider1 {
            width: 50px;
        }

        .layout-slider1-settings {
            font-size: 12px;
            padding-bottom: 0px;
        }

            .layout-slider1-settings pre {
                font-family: arial;
            }

        .layout2 {
            padding: 0px;
            font-family: Georgia, serif;
            height: 10px;
        }

        .layout-slider2 {
            width: 100px;
            margin: 2px;
            padding: 0;
        }

        .layout-slider2-settings {
            font-size: 12px;
            padding-bottom: 0px;
        }

            .layout-slider2-settings pre {
                font-family: arial;
            }

        .layout-slider4 {
            width: 100px;
            margin: 2px;
            padding: 0;
        }

        .lefft {
            padding-left: 2px;
        }

        .borderlefft {
            border-bottom: #c6c8ca solid 1px;
        }

        .borderbottom {
            border-bottom: #c6c8ca solid 1px;
        }
        .grdHead1
{
    text-align: left;
    font-family: Arial;
    color: #ffffff;
    font-size: 12px;
    font-weight: normal;   
    background: #004fa3;
    height: 25px;
    padding-left:10px;
}

.grddata
{
    padding-left:10px;
}

</style>
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


    <%--<script type="text/javascript" >
            $(function () {
                //var dataToPush = JSON.stringify({
                //    schemeIds: schid
                //});
                $.ajax({
                    cache: false,
                    //data: dataToPush,
                    dataType: "json",
                    url: 'localhost:52348/WebMethod.aspx/CheckSession',
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    success: function (dataConsolidated) {
                        alert(dataConsolidated.d);
                        if (dataConsolidated.d == null) {
                            return;
                        }
                        if (dataConsolidated.Value == 0) {
                            return;
                        }
                        
                    },
                    error: function (data) {
                        debugger;
                        alert(data);
                    }
                });
            });

        </script>  --%>
</head>
<body>
<div id="newDiv">
<form id="Form1" runat="server">
                <table width="670" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tbody><tr align="left">
                        <td valign="top">
                            <div class="pageCont">
                                <div class="">
                                    <div class="">
                                        <div class="">
                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tbody><tr align="left">
                                                    <td valign="top">
                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                            <tbody><tr align="left">
                                                                <td width="1%" valign="top">
                                                                    <img src="IMG/spacer11.gif" width="10" height="1" alt="">
                                                                </td>
                                                                <td width="98%" valign="top">
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tbody><tr>
                                                                            <td>
                                                                                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                    <tbody><tr>
                                                                                        <td align="left" valign="top">
                                                                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                
                                                                                                <tbody><tr id="trCategory" align="left">
	<td height="30" align="left" valign="middle" width="23%">
                                                                                                        <span class="FieldHead">Mutual Fund</span>
                                                                                                  </td>
	<td width="80%" colspan="2" align="left" valign="middle">
                                                                                                         <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlMutualFund_SelectedIndexChanged" class="ddl">
                                        </asp:DropDownList>
                                                                                                  </td>
</tr>

                                                                                                <tr align="left">
                                                                                                    <td height="30" align="left" valign="middle" width="23%">
                                                                                                        <span class="FieldHead">
                                                                                                            <span id="lblSchemeName">Category</span></span>
                                                                                                    </td>
                                                                                                    <td width="80%" colspan="2" align="left" valign="middle">
                                                                                                        <asp:DropDownList ID="ddlCategory" runat="server"  class="ddl" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr id="trInception1" align="left">
	<td height="27" align="left" valign="middle">
                                                                                                        
                                                                                                        <span id="LabelInception" class="FieldHead">Sub-Category</span>
                                                                                                  </td>
	<td colspan="3" align="left" valign="middle">
                             <asp:DropDownList ID="ddlSubCategory" runat="server"  class="ddl" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                                                                                  </td>
</tr>

                                                                                                
                                                                                                
                                                                                                <tr id="trBenchmark" align="left">
	<td height="30" align="left" valign="middle" width="23%">
                                                                                                        <span id="Span3" class="FieldHead">Type</span>
                                                                                                  </td>
	<td width="80%" colspan="2" align="left" valign="middle">
                           <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true"  class="ddl"
                                            OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                        </asp:DropDownList>                                         
                                                                                                  </td>
</tr>

                                                                                            </tbody></table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    
                                                                                    <tr id="trSipInvst" align="left">
	<td valign="top">
                                                                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                                                <tbody><tr align="left">
                                                                                                    <td height="30" valign="middle" width="23%">
                                                                                                            <span id="option" class="FieldHead">Option</span>
                                                                                                    </td>
                                                                                                    <td width="77%" valign="middle" colspan="3" class="FieldHead">
                                                                                                   <div class="style-radio">
                                                                                                    <asp:RadioButtonList ID="rdbOption" runat="server"
                                                                                                        class="radio" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                                                        Style="display: inline-block;"
                                                                                                        OnSelectedIndexChanged="rdbOption_SelectedIndexChanged">
                                                                                                    </asp:RadioButtonList>
                                                                                                </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr align="left">
                                                                                                    <td height="30" valign="middle" width="23%">
                                                                                                        <span class="FieldHead">Choose Scheme</span>
                                                                                                    </td>
                                                                                                    <td width="73%" valign="middle" colspan="3">
                                                                                                      <asp:DropDownList ID="ddlSchemes" runat="server" class="ddl">
                                            <asp:ListItem Value="0" Selected="True">-Select Scheme-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="btnAddScheme" runat="server" AlternateText="Add" OnClick="btnAddScheme_Click"
                                            OnClientClick="Javascript:return ValidateControl();" ValidationGroup="scheme" ImageUrl="IMG/add.png" Style="margin-top: -10px;" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Scheme."
                                            ControlToValidate="ddlSchemes" Display="Dynamic" InitialValue="0" ValidationGroup="scheme"></asp:RequiredFieldValidator></td>
                                                                                                    
                                                                                                </tr>
                                                                                                <tr align="left">
                                                                                                    <td height="30" valign="middle" width="23%">
                                                                                                        <span class="FieldHead">Index</span>
                                                                                                    </td>
                                                                                                    <td width="73%" valign="middle" colspan="3">
                                                                                                       <asp:DropDownList ID="ddlIndices" runat="server" class="ddl">
                                                                                                       <asp:ListItem Value="0" Selected="True">-Select Index-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="btnAddIndices" runat="server" AlternateText="Add" ImageUrl="IMG/add.png" OnClick="btnAddIndices_Click"
                                            OnClientClick="Javascript:return Listcount();" ValidationGroup="indices" Style="margin-top: -10px;" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Indices."
                                            ControlToValidate="ddlIndices" Display="Dynamic" InitialValue="0" ValidationGroup="indices"></asp:RequiredFieldValidator></td>
                                                                                                    
                                                                                                </tr>
                                                                                                
                                                                                            </tbody></table>
                                                                                            
                                                                                      </td>
</tr>

                                                                                    
                                                                                    <tr align="left" valign="middle">
                                                                                        <td>
                                                                                            <%--<table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                <tbody><tr align="left">
                                                                                                    <td width="23%" valign="middle">&nbsp;
                                                                                                    </td>
                                                                                                    <td width="40%" valign="middle" align="left">                                                                                                        
                                                                                                            <input type="image" name="sipbtnreset" id="sipbtnreset" src="IMG/reset.png" style="border-width:0px;" />
                                                                                                    </td>
                                                                                                    <td width="37%" valign="middle">&nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody></table>--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>&nbsp;
                                                                                            
                                                                                      </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td height="2"></td>
                                                                                    </tr>
                                                                                </tbody></table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left" valign="top">
                                                                            <td>
                                                                                
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="DivFundShow"  runat="server">
                                                                                    <asp:DataGrid Style="margin-top: 15px; font-size: 12px;" ID="dglist" runat="server" AutoGenerateColumns="false"
                                                                                        HeaderStyle-Font-Bold="true" Width="100%" OnItemCommand="dglist_ItemCommand">
                                                                                        <Columns>
                                                                                            <asp:TemplateColumn HeaderText="Scheme Name" HeaderStyle-CssClass="grdHead1">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblSchemeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>'></asp:Label>
                                                                                                    <asp:Label ID="lblIndId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>'></asp:Label>
                                                                                                    <asp:Label ID="lblSchemeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Sch_Short_Name")%>'></asp:Label>
                                                                                                    <asp:Label ID="lblIndName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "INDEX_NAME")%>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="80%"></HeaderStyle>--%>
                                                                                                <ItemStyle HorizontalAlign="Left" Width="70%" CssClass="grddata"></ItemStyle>
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="Chart" HeaderStyle-CssClass="grdHead1">
                                                                                                <HeaderTemplate>
                                                                                                    <input type="checkbox" name="SelectAllCheckBox" onclick="SelectAll(this)">&nbsp;ALL
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox>
                                                                                                    <asp:HiddenField ID="hdSchID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SCHEME_ID")%>' />
                                                                                                    <asp:HiddenField ID="hdIndID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "INDEX_ID")%>' />
                                                                                                    <asp:HiddenField ID="hdImgID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                                                                <ItemStyle HorizontalAlign="left" Width="10%" CssClass="grddata"></ItemStyle>
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="Auto ID" Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblAutoID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                                                                <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="img/close.png" OnClientClick="javascript:return confirm('Are you sure to delete?');"
                                                                                                        CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AutoID")%>' />
                                                                                                </ItemTemplate>
                                                                                                <%--<HeaderStyle BackColor="#37689a" ForeColor="White" Width="5%"></HeaderStyle>--%>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                                                            </asp:TemplateColumn>
                                                                                        </Columns>
                                                                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                    </asp:DataGrid>
                                                                                    <div align="right" style="padding-top:10px;">
                                                                                    <div>
                                                                                        <asp:Label ID="lblGridMsg" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                                                        <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                                                                                    </div>
                                                                                  <asp:ImageButton ID="btnCompareFund" runat="server" OnClientClick="return validateList();" ImageUrl="~/Top3choice/IMG/performance.png" OnClick="btnCompareFund_Click" Visible="false" width="114" height="26"></asp:ImageButton>
                                                                                 </div>
                                                                                 </div>
                                                                                 
                                                                     <div id="DivShowPerformance" runat="server">
                                                                       <div>
                                                                        <asp:Label ID="lblSortPeriod" runat="server" Visible="false">Click on 'Time Period' to rank funds on a particular period of your choice.
                                                                        </asp:Label>
                                                                    </div>
                                                                    <div>
                                                                     <asp:GridView ID="GrdCompFund" runat="server"  Style="width:100%;border-collapse:collapse; margin-top:5px;" AutoGenerateColumns="false"
                                                                                            OnRowCommand="GrdCompFund_RowCommand">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="30%" HeaderStyle-CssClass="grdHead1">
                                                                                                    <ItemTemplate>                                          
                                                                                                        <%# SetHyperlinkFundDetail(Eval("Sch_id").ToString(), Eval("Sch_Short_Name").ToString())%>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left"  Width="30%"/>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="Lnk1mth" runat="server" Text="1 mth" CssClass="grdHead" Font-Overline="false" CommandName="Per_30_Days"
                                                                                                            Font-Bold="true" />
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbl1Mth" runat="server" Text='<%#Eval("Per_30_Days").ToString() != "" ? Eval("Per_30_Days") : "NA"%>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="Lnk3mth" runat="server" Font-Bold="true"
                                                                                                            Text="3 mths" Font-Overline="false" CommandName="Per_91_Days" CssClass="grdHead"></asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbl3Mth" runat="server">
                                                                                            <%#Eval("Per_91_Days").ToString() != "" ? Eval("Per_91_Days") : "NA"%> </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="Lnk6mth" runat="server" Font-Bold="true" Text="6 mths"
                                                                                                            Font-Overline="false" CommandName="Per_182_Days" CssClass="grdHead"></asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbl6Mth" runat="server">
                                                                                            <%#Eval("Per_182_Days").ToString() != "" ? Eval("Per_182_Days") : "NA"%> </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="Lnk1yr" runat="server" Font-Bold="true" Text="1Yr"
                                                                                                            Font-Overline="false" CommandName="Per_1_Year" CssClass="grdHead"></asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbl1yr" runat="server">
                                                                                            <%#Eval("Per_1_Year").ToString() != "" ? Eval("Per_1_Year") : "NA"%> </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="Lnk3yr" runat="server" Font-Bold="true" Text="3 Yrs"
                                                                                                            Font-Overline="false" CommandName="Per_3_Year" CssClass="grdHead"></asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbl3yr" runat="server">
                                                                                            <%#Eval("Per_3_Year").ToString() != "" ? Eval("Per_3_Year") : "NA"%> </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="LnkSI" runat="server" Font-Bold="true" Text="Since Inception"
                                                                                                            Font-Overline="false" CommandName="Per_Since_Inception" CssClass="grdHead"></asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblSI" runat="server">
                                                                                            <%#Eval("Per_Since_Inception").ToString() != "" ? Eval("Per_Since_Inception") : "NA"%> </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:Label ID="Label1" runat="server" SkinID="lblHeader" Text="NAV"></asp:Label>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblNav" runat="server">
                                                                                            <%#Eval("Nav_Rs").ToString() != "" ? Eval("Nav_Rs") : "NA"%> </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
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
                                                                                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
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
                                                                                        <div>
                                                                                <div>
                                                                                    <asp:Label ID="lbRetrnMsg" Visible="false" runat="server">
                                                                            *Note:- Returns calculated for less than 1 year are Absolute returns and returns
                                                                            calculated for more than 1 year are compounded annualized.</asp:Label>
                                                                                </div>
                                                                                <div class="value_btm_text" style="text-align: right; font-size: 10px;">
                                                                                    Developed and Maintained by: <a href="https://www.icraanalytics.com"  target="_blank">ICRA Analytics Ltd</a>, <a href="https://icraanalytics.com/home/Disclaimer"
                                                                                                            target="_blank">Disclaimer </a>
                                                                                </div>
                                                                            </div>                  
                                                                              </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                            
                                                                            </td>
                                                                        </tr>
                                                                    </tbody></table>
                                                                </td>
                                                                <td width="1%" valign="top">
                                                                    <img src="IMG/spacer11.gif" width="10" height="1">
                                                                </td>
                                                            </tr>
                                                        </tbody></table>
                                                    </td>
                                                </tr>
                                            </tbody></table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody></table>
                </form>
            </div>
</body>
</html>


