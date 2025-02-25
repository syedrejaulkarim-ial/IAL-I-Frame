<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrincipalCalc.aspx.cs"
    EnableViewState="true" EnableViewStateMac="true" ViewStateEncryptionMode="Always"
    Inherits="iFrames.Pages.PrincipleCalc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%--<LINK rel="stylesheet" type="text/css" href="../Styles/jquery-ui-1.8.14.custom.css">

		<script language="javascript" src="../Styles/jquery-1.6.2.min.js" ></script><!-- "jquery-1.6.2.min.js"-->
		<script language="javascript" src="../Styles/jquery-ui-1.8.14.custom.min.js"></script>--%>
<head runat="server">
    <title></title>
    <%--<link href="../../Styles/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Styles/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/dhtmlgoodies_calendar.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.14.custom.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
    var DateDiff = {
 
    inDays: function(d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();
 
        return parseInt((t2-t1)/(24*3600*1000));
    },
 
    inWeeks: function(d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();
 
        return parseInt((t2-t1)/(24*3600*1000*7));
    },
 
    inMonths: function(d1, d2) {
        var d1Y = d1.getFullYear();
        var d2Y = d2.getFullYear();
        var d1M = d1.getMonth();
        var d2M = d2.getMonth();
 
        return (d2M+12*d2Y)-(d1M+12*d1Y);
    },
 
    inYears: function(d1, d2) {
        return d2.getFullYear()-d1.getFullYear();
    }
}


        function sipValidation() {
        //debugger;
            var sipStartdata = document.getElementById('<%=txtfrmDate.ClientID%>').value;
            var sipEnddata = document.getElementById('<%=txtToDate.ClientID%>').value;
            var sipAsondate = document.getElementById('<%=txtAsOn.ClientID%>').value;
            //var sipAmunt = document.getElementById("sipAmount").value;
            var sipAmunt = document.getElementById('<%=sipAmount.ClientID%>').value;
            //document.getElementById('<%=sipAmount.ClientID%>').value;

            if (sipStartdata == "" || sipEnddata == "" || sipAsondate == "" || sipAmunt == "") {

                if (sipStartdata == "") {
                    alert("Start Date cannot be Blank");                    
                    document.getElementById('<%=txtfrmDate.ClientID%>').focus();
                    return false;
                }
                else if (sipEnddata == "") {
                    alert("End Date cannot be Blank");                    
                    document.getElementById('<%=txtToDate.ClientID%>').focus();
                    return false;
                }
                else if (sipAsondate == "") {
                    alert("As on Date cannot be Blank");                    
                    document.getElementById('<%=txtAsOn.ClientID%>').focus();
                    return false;
                }
                else {
                    alert("Amount cannot be Blank");
                    document.getElementById('<%=sipAmount.ClientID%>').focus();
                    return false;
                }

            }

            var bool = IsValidDate(sipStartdata,sipEnddata,11);
            if(bool == false)
            return false;

            var allot_date = "<%=ViewState["alotedate"]%>";            
            var bool = IsValidDate(sipStartdata,allot_date,32);
            if(bool == false)
            return false;

//            var fromdatemodfied = "<%=ViewState["fromDatemodified"]%>"; 
//            var bool12 = IsValidDate(fromdatemodfied,sipEnddata,12);
//            if(bool12 == false)
//            return false;

//            if (Date.parse(sipEnddata) <= Date.parse(sipStartdata)) {
//                alert("End Date should be Greater than the Start Date");
//                document.getElementById("txtToDate").value = "";
//                document.forms(0).txtToDate.focus();
//                return false;
//            }

            var bool1 = IsValidDate(sipEnddata,sipAsondate,3);
             if(bool1 == false)
                        return false;

//            if (Date.parse(sipEnddata) >= Date.parse(sipAsondate)) {
//                alert("As on Date should be Greater than or Equal to the End Date");
//                document.getElementById("txtAsOn").value = "";
//                document.forms(0).txtAsOn.focus();
//                return false;
//            }
              var bool3 =  asonDatechk(sipAsondate)
             if(bool3 == false)
                        return false;





            if (isNaN(sipAmunt)) {
                alert("Amount is not a number, Please enter a valid Amount");
                document.getElementById('<%=sipAmount.ClientID%>').value = "";
                document.getElementById('<%=sipAmount.ClientID%>').focus();
                return false;

            }

            return(true);
        }

//        function CheckDateEalier(sender, args) {
//            if (sender._selectedDate > new Date()) {
//                alert("You cannot select a day after today!");
//                sender._selectedDate = new Date();
//                // set the date back to the today
//                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
//            }
//        }


function isNumber(key) {
            var keycode = (key.which) ? key.which : key.keyCode;

            if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
                return true;
            }
            return false;
        }

        function isNumberDate(key) {
            var keycode = (key.which) ? key.which : key.keyCode;

            if ((keycode >= 48 && keycode <= 57) || keycode == 8 || keycode == 9) {
                return true;
            }
            return false;
        }

        function lump_validate() {            
            var lumpStartDate = document.getElementById('<%=LumpStartDate.ClientID%>').value;
            var lumpEndDate = document.getElementById('<%=LumpEndDate.ClientID%>').value;
            var lumpAmunt = document.getElementById('<%=LumpAmount.ClientID%>').value;
            //debugger;   document.getElementById('<%=LumpAmount.ClientID%>').value                    


            if (lumpStartDate == "" || lumpEndDate == "" || lumpAmunt == "") 
            {

                if (lumpStartDate == "") {
                    alert("Start Date cannot be Blank");
                    document.getElementById('<%=LumpStartDate.ClientID%>').focus();
                    return false;
                }
                else if (lumpEndDate == "") {
                    alert("End Date cannot be Blank");                    
                    document.getElementById('<%=LumpEndDate.ClientID%>').focus();
                    return false;
                }
                else {
                    alert("Amount cannot be Blank");                    
                    document.getElementById('<%=LumpAmount.ClientID%>').focus();
                    return false;
                }
            }
             
//            if (Date.parse(lumpEndDate) <= Date.parse(lumpStartDate)) {
////                // allot_date.setFullYear(1998, 09, 28)
////                //var today_date = new Date();                
////                alert("End Date should be Greater than the Start Date");
////                return false;
//            }


            var bool = IsValidDate(lumpStartDate,lumpEndDate,1);
            if(bool == false)
            return false;
            else {
                //debugger;
//                // var allot_date = document.forms(0).Hidden1.value;
//                //var allot_date = document.getElementById(Hidden1).value;

                var allot_date = "<%=ViewState["alotedate"]%>";
                var navdate = "<%=ViewState["navDate"]%>";
                 
//                
                //allot_date.setFullYear(1998, 09, 28)
                // var today_date = new Date();

                if(!IsValidDate(lumpEndDate,navdate,2) || !IsValidDate(allot_date,lumpStartDate,2))
                {                
                    return false;
                }

//                if (Date.parse(lumpEndDate) > Date.parse(navdate) || Date.parse(lumpStartDate) < Date.parse(allot_date)) {
//                    
//                }
            }





            if (isNaN(lumpAmunt)) {
                alert("Amount is not a number, Please enter a valid Amount");
                document.getElementById('<%=LumpAmount.ClientID%>').value = "";
                document.getElementById('<%=LumpAmount.ClientID%>').focus();
                return false;

            }

            return true;
        }

        function asonDatechk(str1)
        {
        //debugger;
          var dt = new Date();
          var Day  = parseInt(str1.substring(0,2),10); 
            var Mn = parseInt(str1.substring(3,5),10);
            var Yr  = parseInt(str1.substring(6,10),10); 
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dtt = new Date(DateVal);
            var i = DateDiff.inDays(dtt, dt)
                       if(i <= 0){
                        alert("As on Date should be Less than today");                        
                        document.getElementById('<%=txtAsOn.ClientID%>').value = "";
                        document.getElementById('<%=txtAsOn.ClientID%>').focus();
                        return false;
                        }
                        return true;
        }




        //function IsValidDate(Day,Mn,Yr)
        function IsValidDate(str1,str2,bool){
            var Day  = parseInt(str1.substring(0,2),10); 
            var Mn = parseInt(str1.substring(3,5),10);
            var Yr  = parseInt(str1.substring(6,10),10); 
            var DateVal = Mn + "/" + Day + "/" + Yr;
            var dt = new Date(DateVal);

            var Day1  = parseInt(str2.substring(0,2),10); 
            var Mn1 = parseInt(str2.substring(3,5),10);
            var Yr1  = parseInt(str2.substring(6,10),10); 
            var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;
            var dt1 = new Date(DateVal1);

                if(dt.getDate()!=Day){
                    alert('Invalid Date');
                    return false;
                    }
                else if(dt.getMonth()!=Mn-1){
                //this is for the purpose JavaScript starts the month from 0
                    alert('Invalid Date');
                    return false;
                    }
                else if(dt.getFullYear()!=Yr){
                    alert('Invalid Date');
                    return false;
                    }


                    if(dt1.getDate()!=Day1){
                    alert('Invalid Date');
                    return false;
                    }
                else if(dt1.getMonth()!=Mn1-1){
                //this is for the purpose JavaScript starts the month from 0
                    alert('Invalid Date');
                    return false;
                    }
                else if(dt1.getFullYear()!=Yr1){
                    alert('Invalid Date');
                    return false;
                    }

                    if(bool == 1)
                    {
                    if(dt >= dt1)
                       {
                           alert("End Date should be Greater than the Start Date");                                                      
                           document.getElementById('<%=LumpEndDate.ClientID%>').value = "";
                           document.getElementById('<%=LumpEndDate.ClientID%>').focus();  
                           return false; 
                       } 
                       }
                       if(bool == 11)
                    {
                            if(dt >= dt1)
                               {
                                   alert("End Date should be Greater than the Start Date");                     
                                   document.getElementById('<%=txtToDate.ClientID%>').value = "";                            
                                   document.getElementById('<%=txtToDate.ClientID%>').focus();  
                                   return false; 
                               } 
//                               
//                               debugger
//                       var allot_date = '<%=ViewState["alotedate"]%>';
//                         var navdate = '<%=ViewState["navDate"]%>';
//                       var scheme_name = document.getElementById("ddlSchemeList").value;
//                    //alert("Date range for the " + scheme_name + " is From Date " + allot_date.getUTCDate() + "/" + allot_date.getUTCMonth() + "/" + allot_date.getYear() + " and " + today_date.getUTCDate() + "/" + today_date.getUTCMonth() + "/" + today_date.getYear() + " to date");
//                    alert("Date range for the " + scheme_name + " is From Date " + allot_date + " and " + navdate + " to date");
//                    document.getElementById('<%=txtfrmDate.ClientID%>').value = "";
//                     document.getElementById('<%=txtfrmDate.ClientID%>').focus();
//                    return false;
//                   

                       }

                       if(bool == 12)
                       {
                       if(dt >= dt1){

                        alert("Sip Nav Date should be Greater than or Equal to the End Date");
                        document.getElementById('<%=txtfrmDate.ClientID%>').value = "";
                        document.getElementById('<%=txtfrmDate.ClientID%>').focus();
                        return false;
                        }
                        }



                       if(bool == 2)
                       {
                       if(dt > dt1){
                       var allot_date = '<%=ViewState["alotedate"]%>';
                var navdate = '<%=ViewState["navDate"]%>';
                       var scheme_name = document.getElementById("ddlSchemeList").value;
                    //alert("Date range for the " + scheme_name + " is From Date " + allot_date.getUTCDate() + "/" + allot_date.getUTCMonth() + "/" + allot_date.getYear() + " and " + today_date.getUTCDate() + "/" + today_date.getUTCMonth() + "/" + today_date.getYear() + " to date");
                    alert("Date range for the " + scheme_name + " is From Date " + allot_date + " and " + navdate + " to date");
                    document.getElementById('<%=LumpEndDate.ClientID%>').value = "";
                     document.getElementById('<%=LumpEndDate.ClientID%>').focus();
                    return false;
                    }
                      }

                      if(bool == 32)
                       {
                       if(dt <= dt1){
                       var allot_date = '<%=ViewState["alotedate"]%>';
                var navdate = '<%=ViewState["navDate"]%>';
                       var scheme_name = document.getElementById("ddlSchemeList").value;
                    //alert("Date range for the " + scheme_name + " is From Date " + allot_date.getUTCDate() + "/" + allot_date.getUTCMonth() + "/" + allot_date.getYear() + " and " + today_date.getUTCDate() + "/" + today_date.getUTCMonth() + "/" + today_date.getYear() + " to date");
                    alert("Date range for the " + scheme_name + " is From Date " + allot_date + " and " + navdate + " to date");
                    document.getElementById('<%=txtfrmDate.ClientID%>').value = "";
                     document.getElementById('<%=txtfrmDate.ClientID%>').focus();
                    return false;
                    }
                      }

                       if(bool == 3)
                       {
                       if(dt > dt1){

                        alert("As on Date should be Greater than or Equal to the End Date");
                        document.getElementById('<%=txtAsOn.ClientID%>').value = "";
                        document.getElementById('<%=txtAsOn.ClientID%>').focus();
                        return false;
                        }
                        }


                      


                    return true;
        }       
  
    </script>
    <%--<script type="text/javascript">
        $.datepicker.setDefaults({ dateFormat: "dd/mm/yy" });
        $(document).ready(function () {
            var date = new Date();
            var currentMonth = date.getMonth();
            var currentDate = date.getDate();
            var currentYear = date.getFullYear();

            $('#txtfrmDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //            $('#txtfrmDate').datepicker('option', 'dateFormat', 'dd/mm/yy');
            //            $('#txtfrmDate').datepicker('option', 'altFormat', 'dd/mm/yy');

            $('#txtToDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //            $('#txtToDate').datepicker('option', 'dateFormat', 'dd/mm/yy');
            //            $('#txtToDate').datepicker('option', 'altFormat', 'dd/mm/yy');

            $('#txtAsOn').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //            $('#txtAsOn').datepicker('option', 'dateFormat', 'dd/mm/yy');
            //            $('#txtAsOn').datepicker('option', 'altFormat', 'dd/mm/yy');
            $('#LumpStartDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //            $('#txtToDate').datepicker('option', 'dateFormat', 'dd/mm/yy');
            //            $('#txtToDate').datepicker('option', 'altFormat', 'dd/mm/yy');

            $('#LumpEndDate').datepicker({ maxDate: new Date(currentYear, currentMonth, currentDate), onSelect: function () { } });
            //            $('#txtAsOn').datepicker('option', 'dateFormat', 'dd/mm/yy');
            //            $('#txtAsOn').datepicker('option', 'altFormat', 'dd/mm/yy');



        });
    </script>--%>
    <script type="text/javascript">

        function checkValidDate(yr, mmx, dd) {
            if (yr < 1910 || yr > 2009) { // you may want to change 2009 to some other year!
                alert("Impossible Year Of Birth!")
                return false;
            }
        }

        function mhover(id) {
            debugger;
            //            this.backgroundColor = '#2172A1';
            // document.forms(0).Resetbtn.style.backgroundColor = "#cc66000";
            document.getElementById(id).style.backgroundColor = "#003366";
            //document.forms(0).Resetbtn.style.backgroundColor = '#003366';
        }

        //        function DisplayDate() {
        //            //var lstartdate = document.getElementById('LumpStartDate');
        //            //document.getElementById('chartDisplay').style.visibility = "hidden";
        //            //document.getElementById('trLS').style.visibility = "visibile";
        //           // trLS
        //            //document.getElementById('LumpStartDate').setAttribute("readOnly", "true");
        ////            document.getElementById('<%=LumpEndDate.ClientID%>').setAttribute("readOnly", "true");
        ////            document.getElementById('<%=txtAsOn.ClientID%>').setAttribute("readOnly", "true");
        ////            document.getElementById('<%=txtfrmDate.ClientID%>').setAttribute("readOnly", "true");
        ////            document.getElementById('<%=txtToDate.ClientID%>').setAttribute("readOnly", "true");
        //        }
    </script>
    <style type="text/css">
        
        .backgrdcolor
        {
            
            background:url(../images/principalbg.jpg) repeat-x #95c3f2;
        }
        
        
        .paneltext{margin:0 0 0 10px; width:95%;}
     
        .sipGrid2
        {
            left: auto;
            margin-left: 8%;
            width: 84%;
            border: 1px solid #99CCFF;
            font-family: Arial;
            font-size: 11px;
        }
        .CssTableNote
        {
            text-align : justify;
            font-family: Arial;
            font-size: 12px;
            width: 84%;
            margin-left: 8%;
            margin-right: 8%;
        }
        .collapsibleContainer
        {
            font-family:Arial;
        }
        .style1
        {
            width: 84%;
            font-family: Arial;
            font-size: 12px;
            border: 1px solid #99CCFF;
            height: 53px;
            margin-left: 8%;
            margin-right:8%;
            top: -50px;
            left: -70px;            
        }
        .ListRow
        {
            background-color: #A0C0DF;
            font-size: 12px;
        }
        
        .AltRow
        {
            background-color: #f4fbff;
            font-size: 12px;
        }
        
        .headerCss
        {
            background-color: #cc6600;            
            color: White;
        }
        
        
        
        
        
        
        .Ldivstyle
        {
            border: 1px solid #99CCFF;
            background-color: #99CCFF;
        }
        
        
        #SipCalDiv
        {
            height: 92px;
        }
        .txtboxstyle
        {
            width: 290px;
            font-size:12px;
            background-color:#f4fbff;           
        }
        .tdstyle1
        {
            padding-left:12px;
            text-align: left;
            font-size:11px;
            width : 20%;
            vertical-align: middle;            
            <%--height: 40px;--%>
            
        }
        .tdstyle2
        {
            
            text-align: left;
            width : 30%;
            font-size:11px;
            vertical-align: middle;
           <%-- height: 40px;--%>
            
        }
        .ddlstyle
        {
            width: 300px;
            font-family:Arial;
            font-size:9pt;
            height:20px;
            background-color:#f4fbff;
            
        }
           
        .pnlCSS{
        font-weight: bold;
        cursor: pointer;
        border: solid 1px #c0c0c0;
        width:30%;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font: bold 11px auto "Trebuchet MS", Arial;
            font-size: 12px;
            cursor: pointer;
            width:450px;
            height:18px;
            padding: 4px;            
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Arial;
            border: 1px gray;                
            width:450px;
            padding: 4px;
            padding-top: 7px;
        }       

        .style2
        {
            width: 268435280px;
        }
        

    </style>
    <%--<script language="javascript" type="text/javascript">


        $(function () {
            $.datepicker.setDefaults({ dateFormat: "mm/dd/yy" });
            $("#TextBox3").datepicker();
            $("#TextBox4").datepicker();
            $("#TextBox5").datepicker();

        });
	</script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <div style="width:100%;">
        <table class="style1">
            <tr>
                <td align="left" width="50%">
                    <img src="../Images/Principal MF logo new.jpg" style="height: 80px; width: 120px;" />
                </td>
                <td align="right" width="50%" style="vertical-align: middle;padding-right:10px;">
                    <img src="../Images/Online Returns Calci Logo.gif" style="height: 70px; width: 270px;" />
                </td>
            </tr>
        </table>
        <table class="style1 backgrdcolor" cellspacing="0px">
            <thead>
                <tr>
                    <td colspan="4" style="background-color: #003063; color: White; text-align: center;
                        vertical-align: middle; padding-top: 0px; height: 23px; font-size: 9pt; font-family: Arial;">
                        <b>Principal Mutual Fund </b>
                    </td>
                </tr>
            </thead>
            <tr>
                <td colspan="4" style="height: 8px;">
                    <img src="../Images/section-div-gradient.gif" width="100%" />
                </td>
            </tr>
            <tr>
                <td colspan="4" style="height: 6px;">
                </td>
            </tr>
            <tr>
                <td class="tdstyle1">
                    <b>Scheme Type</b>
                </td>
                <td class="tdstyle2">
                    <asp:DropDownList ID="ddlNature" runat="server" AutoPostBack="true" DataTextField="Nature"
                        DataValueField="Nature" CssClass="ddlstyle" OnSelectedIndexChanged="ShowAllScheme">
                    </asp:DropDownList>
                </td>
                <td class="tdstyle1">
                    <b>Option of the Investment</b>
                </td>
                <td class="tdstyle2">
                    <asp:DropDownList ID="ddlOption" runat="server" OnSelectedIndexChanged="SchemeListforNature"
                        AutoPostBack="true" CssClass="ddlstyle">
                        <asp:ListItem Selected="True">--</asp:ListItem>
                        <asp:ListItem Value="2">Growth </asp:ListItem>
                        <asp:ListItem Value="3">Dividend Reinvestment</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdstyle1">
                    <span id="Label28" class="txt2"><b>Name of the Scheme</b></span>
                </td>
                <td class="tdstyle2">
                    <asp:DropDownList ID="ddlSchemeList" runat="server" DataTextField="Sch_Short_Name"
                        DataValueField="Sch_Short_Name" OnSelectedIndexChanged="SetBenchmarkSelScheme"
                        AutoPostBack="true" CssClass="ddlstyle">
                    </asp:DropDownList>
                </td>
                <td class="tdstyle1">
                    <span id="Label25" class="txt2"><b>Scheme Benchmark</b></span>
                </td>
                <td class="tdstyle2">
                    <asp:DropDownList ID="ddBenchMark" runat="server" CssClass="ddlstyle">
                        <asp:ListItem Selected="True">--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdstyle1">
                    <span id="Span1" class="txt2"><b>Fund Manager</b></span>
                </td>
                <td>
                    <asp:TextBox ID="FundmanegerText" class="txtboxstyle" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
                <td class="tdstyle1">
                    <span id="Span2" class="txt2"><b>Investment Mode </b></span>
                </td>
                <td class="tdstyle2">
                    <asp:DropDownList ID="ddlMode" runat="server" OnSelectedIndexChanged="ShowRelativeDiv"
                        AutoPostBack="True" CssClass="ddlstyle">
                        <asp:ListItem Selected="True">--</asp:ListItem>
                        <asp:ListItem>SIP</asp:ListItem>
                        <asp:ListItem>LumpSum</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <%--<td class="tdstyle1"  >
                    <span id="Label30" class="txt2"><b>Investment Mode </b></span>
                </td>--%>
                <%--<td class="tdstyle2" >
                    <asp:DropDownList ID="ddlMode1" runat="server" OnSelectedIndexChanged="ShowRelativeDiv"
                        AutoPostBack="True" CssClass="ddlstyle">
                        <asp:ListItem Selected="True">--</asp:ListItem>
                        <asp:ListItem>SIP</asp:ListItem>
                        <asp:ListItem>LumpSum</asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
                <td colspan="2" class="tdstyle1">
                    <div style="float: left">
                        <asp:Button ID="Resetbtn" runat="server" Text="Refresh" onMouseOut="document.form1.Resetbtn.style.backgroundColor = '#cc6600';"
                            onMouseOver="document.form1.Resetbtn.style.backgroundColor = '#003366';" OnClick="ResetForm"
                            BackColor="#cc6600" ForeColor="White" /></div>
                    <div style="float: left; margin-left: 10px; color: Red; font-weight: bold">
                        <asp:Label Style="margin-top: 12px; vertical-align: middle;" ID="lblMessage" runat="server"
                            Text="Label"></asp:Label></div>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr runat="server" id="trLS">
                <td colspan="4" style="height: 16px; vertical-align: middle;">
                    <div id="Lumpsumdiv" runat="server" class="collapsibleContainer">
                        <table style="width: 100%;" cellspacing="0px">
                            <tr>
                                <td colspan="4" style="height: 16px; vertical-align: middle;">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdstyle1">
                                    <b>Start Date</b>
                                </td>
                                <td class="tdstyle2">
                                    <input id="LumpStartDate" type="text" class="txtboxstyle" runat="server" onfocus="displayCalendar(document.forms[0].LumpStartDate,'dd/mm/yyyy',this)"
                                        readonly="readonly" />
                                    <%--<asp:TextBox ID="LumpStartDate" class="txtboxstyle" runat="server" 
                                        onfocus="displayCalendar(document.forms[0].LumpStartDate,'dd/mm/yyyy',this)"></asp:TextBox>--%>
                                    <%--<a href="#" style="text-decoration:none;border-width:0px;" onclick="displayCalendar(document.forms[0].LumpStartDate,'dd/mm/yyyy',this)"><img src="../../Images/stars/Green/1-star-grn.jpg"  style="WIDTH: 15px; HEIGHT: 15px;marign-top:-10px;" align="middle"></a>--%>
                                </td>
                                <td class="tdstyle1">
                                    <b>Value as of Date</b>
                                </td>
                                <td class="tdstyle2">
                                    <%--<asp:TextBox ID="LumpEndDate" runat="server" class="txtboxstyle" 
                                        onfocus="displayCalendar(document.forms[0].LumpEndDate,'dd/mm/yyyy',this)"></asp:TextBox>--%>
                                    <input type="text" id="LumpEndDate" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].LumpEndDate,'dd/mm/yyyy',this)"
                                        readonly="readonly" />
                                    <%--<a href="#" style="text-decoration:none;border-width:0px;" onfocus="displayCalendar(document.forms[0].LumpEndDate,'dd/mm/yyyy',this)"><img src="../../Images/stars/Green/1-star-grn.jpg"  style="WIDTH: 15px; HEIGHT: 15px;marign-top:-10px;" align="middle"></a>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdstyle1">
                                    <b>Amount Invested (Rs.)</b>
                                </td>
                                <td class="tdstyle2" colspan="3">
                                    <asp:TextBox ID="LumpAmount" runat="server" onkeypress="return isNumber(event)" MaxLength="10"
                                        class="txtboxstyle"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" class="tdstyle1" style="height: 40px; vertical-align: bottom">
                                    <asp:Button ID="LumpCalButton" runat="server" Text=" Calculate Lump Sum " onMouseOut="document.form1.LumpCalButton.style.backgroundColor = '#cc6600';"
                                        onMouseOver="document.form1.LumpCalButton.style.backgroundColor = '#003366';"
                                        OnClick="LumpSumCalcBtn_Click" OnClientClick="return lump_validate();" BackColor="#cc6600"
                                        ForeColor="White" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr runat="server" id="trSIP">
                <td colspan="4" style="height: 16px; vertical-align: middle;">
                    <div id="SipDiv" runat="server" class="collapsibleContainer">
                        <table style="width: 100%" cellspacing="0px">
                            <tr>
                                <td colspan="4" style="height: 16px; vertical-align: middle;">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdstyle1">
                                    <span id="Label14" class="txt2"><b>Start Date</b></span>
                                </td>
                                <td class="tdstyle2">
                                    <%--<asp:TextBox ID="txtfrmDate" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].txtfrmDate,'dd/mm/yyyy',this)"></asp:TextBox>--%>
                                    <input type="text" id="txtfrmDate" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].txtfrmDate,'dd/mm/yyyy',this)"
                                        readonly="readonly" />
                                </td>
                                <td class="tdstyle1">
                                    <span id="Label15" class="txt2"><b>End Date</b></span>
                                </td>
                                <td class="tdstyle2">
                                    <%--<asp:TextBox ID="txtToDate" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].txtToDate,'dd/mm/yyyy',this)"></asp:TextBox>--%>
                                    <input type="text" id="txtToDate" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].txtToDate,'dd/mm/yyyy',this)"
                                        readonly="readonly" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdstyle1">
                                    <span id="Label37" class="txt2"><b>Value As on Date</b></span>
                                </td>
                                <td class="tdstyle2">
                                    <%--<asp:TextBox ID="txtAsOn" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].txtAsOn,'dd/mm/yyyy',this)"></asp:TextBox>--%>
                                    <input type="text" id="txtAsOn" runat="server" class="txtboxstyle" onfocus="displayCalendar(document.forms[0].txtAsOn,'dd/mm/yyyy',this)"
                                        readonly="readonly" />
                                </td>
                                <td class="tdstyle1">
                                    <span id="Label35" class="txt2"><b>SIP Date</b></span>
                                </td>
                                <td class="tdstyle2">
                                    <asp:DropDownList ID="ddSipIntialDate" runat="server" class="ddlstyle">
                                        <asp:ListItem Value="1">1st</asp:ListItem>
                                        <asp:ListItem Value="5">5th</asp:ListItem>
                                        <asp:ListItem Value="10">10th</asp:ListItem>
                                        <asp:ListItem Value="15">15th</asp:ListItem>
                                        <asp:ListItem Value="20">20th</asp:ListItem>
                                        <asp:ListItem Value="25">25th</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdstyle1">
                                    <span id="Label36" class="txt2"><b>Frequency</b></span>
                                </td>
                                <td class="tdstyle2">
                                    <asp:DropDownList ID="ddPeriod" runat="server" class="ddlstyle">
                                        <asp:ListItem>Monthly</asp:ListItem>
                                        <asp:ListItem>Quarterly</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="tdstyle1">
                                    <span id="Label13" class="txt2"><b>Amount (Rs.)</b></span>
                                </td>
                                <td class="tdstyle2">
                                    <asp:TextBox ID="sipAmount" runat="server" onkeypress="return isNumber(event)" class="txtboxstyle"
                                        MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tdstyle1" style="height: 40px; vertical-align: bottom">
                                    <asp:Button ID="SipCalcButton" runat="server" Text="Calculate SIP" OnClick="SipCalcBtn_Click"
                                        ValidationGroup="SipValidation" onMouseOut="document.form1.SipCalcButton.style.backgroundColor = '#cc6600';"
                                        onMouseOver="document.form1.SipCalcButton.style.backgroundColor = '#003366';"
                                        OnClientClick="return sipValidation();" BackColor="#cc6600" ForeColor="White" />
                                </td>
                                <td colspan="2">
                                    <div style="float: left; margin-left: 10px; color: Red; font-weight: bold">
                                        <asp:Label Style="margin-top: 12px; vertical-align: middle;" ID="lbmsg" runat="server"></asp:Label></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="height: 12px; vertical-align: middle;">
                </td>
            </tr>
        </table>
        <br />
        <br />
        <div id="showResult" runat="server">
            <div id="SipResultDiv" runat="server" class="collapsibleContainer">
                <br />
                <br />
                <asp:GridView ID="SipResultGrid" runat="server" CellPadding="4" AutoGenerateColumns="false"
                    EnableModelValidation="True" ForeColor="#333333" Width="84%" Font-Size="12px"
                    Font-Names="Arial" CssClass="sipGrid2">
                    <AlternatingRowStyle BackColor="#f4fbff" BorderColor="#66CCFF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#cc6600" BorderColor="#FF66CC" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#A0C0DF" Font-Names="Arial" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Sr No." HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("Id") == DBNull.Value) ? "--" : Eval("Id").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV Date" HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("Nav Date") == DBNull.Value) ? "--" : Eval("Nav Date").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="NAV Value of the Scheme as of NAV Date"
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("Nav") == DBNull.Value) ? "--" : Eval("Nav").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Outflow in Rs."
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("Scheme Cashflow") == DBNull.Value) ? "--" : Eval("Scheme Cashflow").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Scheme Units Purchased as of NAV Date"
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("Scheme Units") == DBNull.Value) ? "--" : Eval("Scheme Units").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <br />
                <asp:GridView ID="SPGridViewtbl1" runat="server" CssClass="sipGrid2" BorderWidth="2px"
                    CellPadding="4" AutoGenerateColumns="False" Font-Size="12px">
                    <AlternatingRowStyle BackColor="#f4fbff" Font-Names="Arial" />
                    <RowStyle BackColor="#A0C0DF" Font-Names="Arial" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Total Outflow in Rs."
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("TotalOutflow") == DBNull.Value) ? "--" : Eval("TotalOutflow").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Total Units Purchased"
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("SumUnit") == DBNull.Value) ? "--" : Eval("SumUnit").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Value as of Date "
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("ValueasofDate") == DBNull.Value) ? "--" : Eval("ValueasofDate").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </div>
            <input id="Hidden1" type="hidden" runat="server" />
            <input id="Hidden2" type="hidden" runat="server" />
            <div id="LsResultDiv" runat="server" class="collapsibleContainer">
                <asp:GridView ID="LsGridViewtbl1" runat="server" CssClass="style1" BorderWidth="2px"
                    CellPadding="4" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount Invested "
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# (Eval("AmountInvested") == DBNull.Value) ? "--" : Eval("AmountInvested").ToString()%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Value as of Date"
                            HeaderStyle-CssClass="headerCss">
                            <ItemTemplate>
                                <%# ( Eval("ValueasofDate") == DBNull.Value) ? "--" : Eval ("ValueasofDate").ToString() %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </div>
            <br />
            <asp:GridView ID="CommonResultGridView" runat="server" CssClass="style1" BorderWidth="2px"
                CellPadding="4" AutoGenerateColumns="False" OnRowDataBound="GV_RowDataBound"
                OnDataBound="gv_DataBound">
                <AlternatingRowStyle BackColor="#f4fbff" />
                <RowStyle BackColor="#A0C0DF" />
                <Columns>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Period" HeaderStyle-CssClass="headerCss">
                        <ItemTemplate>
                            <asp:Literal ID="LitQtrPrd" runat="server" Text='<%# (Eval("qtryear") == DBNull.Value) ? "--" : Eval("qtryear").ToString()%>'></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns in %"
                        HeaderStyle-CssClass="headerCss">
                        <ItemTemplate>
                            <%# (Eval("navReturn") == DBNull.Value) ? "--" : Eval("navReturn").ToString()%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<sup>$</sup>PTP Returns"
                        HeaderStyle-CssClass="headerCss">
                        <ItemTemplate>
                            <%# (Eval("navp2pReturn") == DBNull.Value) ? "--" : Eval("navp2pReturn").ToString()%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns in %"
                        HeaderStyle-CssClass="headerCss">
                        <ItemTemplate>
                            <%# (Eval("indexReturn") == DBNull.Value) ? "--" : Eval("indexReturn").ToString()%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<sup>$</sup>PTP Returns"
                        HeaderStyle-CssClass="headerCss">
                        <ItemTemplate>
                            <%# (Eval("indexp2pReturn") == DBNull.Value) ? "--" : Eval("indexp2pReturn").ToString()%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Returns in %"
                        HeaderStyle-CssClass="headerCss">
                        <ItemTemplate>
                            <%# (Eval("addlindexReturn") == DBNull.Value) ? "--" : Eval("addlindexReturn").ToString()%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<sup>$</sup>PTP Returns"
                        HeaderStyle-CssClass="headerCss">
                        <ItemTemplate>
                            <%# (Eval("addlindexp2pReturn") == DBNull.Value) ? "--" : Eval("addlindexp2pReturn").ToString()%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Panel runat="server" ID="TableNote" CssClass="CssTableNote " Visible="false">
                <asp:Label Style="margin-top: 11px; vertical-align: middle; color: Red; font-family: Arial;
                    font-weight: bold" Text="" ID="lblerrmsg" runat="server"></asp:Label>
                <%--<ul>
                    <li>Past performance may or may not be sustained in future. Since Inception Returns
                        (in %) are calculated on Compounded Annualised Basis.</li>
                    <li>$ PTP (Point to Point) Returns is based on standard investment of 10,000/- made
                        at the beginning of relevant period.</li>
                </ul>--%>
                <br />
                Past Performance may or may not be sustained in future. Since Inception returns
                (in %) are calculated on Compounded Annualised Basis.<br />
                <sup>$</sup> PTP (Point to Point) returns is based on standard investment of Rs.
                10,000/- made at the beginning of the relevant period. In case of Dividend Reinvestment
                Option all dividend payouts during the respective period are assumed to be reinvested
                in the units of the scheme at the then prevailing NAV.
                <br />
                Please also refer to performance details of other schemes of Principal Mutual Fund
                managed by the Fund Manager(s) of this Scheme. To know the schemes managed by Fund
                Manager(s) please refer the table below:
                <br />
                <br />
                <table border="2" cellpadding="2" cellspacing="2" width="100%" style="border-color: Black;">
                    <thead>
                        <tr>
                            <td width="25%">
                                <b>Name of the Fund Manager </b>
                            </td>
                            <td width="75%">
                                <b>Scheme(s) Managed by the Fund Manager </b>
                            </td>
                        </tr>
                    </thead>
                    <tr>
                    <td width="25%">
                        Mr. Rajat Jain
                    </td>
                    <td width="75%">
                        Principal Global Opportunities Fund
                    </td>
                </tr>
                <tr>
                    <td>
                        Mr. P.V.K. Mohan
                    </td>
                    <td>
                        Principal Growth Fund, Principal Retail Equity Savings Fund, Principal Tax Savings Fund
                    </td>
                </tr>
                <tr>
                    <td>
                        Mr. Dhimant Shah
                    </td>
                    <td>
                        Principal Emerging Bluechip Fund, Principal Dividend Yield Fund
                    </td>
                </tr>
                <tr>
                    <td>
                        Mr. Anupam Tiwari
                    </td>
                    <td>
                        Principal Large Cap Fund, Principal Smart Equity
                        Fund, Principal Personal Tax Saver Fund
                    </td>
                </tr>
                <tr>
                    <td>
                        Mr. Gurvinder Singh Wasan
                    </td>
                    <td>
                        Principal Bank CD Fund, Principal Income Fund - Short Term Plan
                    </td>
                </tr>
                <tr>
                    <td>
                        Mr. Pankaj Jain
                    </td>
                    <td>
                        Principal Debt Savings Fund - Monthly Income Plan, Principal Debt
                        Opportunities Fund - Corporate Bond Plan, Principal Cash Management Fund,
                        Principal Income Fund - Long Term Plan,Principal Retail
                        Money Manager, Principal Debt Opportunities Fund – Conservative Plan, Principal Debt Savings Fund - Retail Plan
                    </td>
                </tr>
					<tr>
                                                            <td>
                                                                Ms. Rupali Pandit
                                                            </td>
                                                            <td>
                                                                Principal Index Fund
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td>
                                                                Ms. Bekxy Kuriakose
                                                            </td>
                                                            <td>
                                                                Principal G Sec Fund
                                                            </td>
                                                        </tr>
                </table>
                <br />
             <%--   * Assistant Fund Manager<br />--%>
                The return calculator has been developed and maintained by ICRA Analytics Limited.
                Principal Pnb Asset Management Company Pvt. Ltd. / Principal Trustee Company Pvt.
                Ltd. does not endorse the authenticity or accuracy of the figures based on which
                the returns are calculated, nor shall they be held responsible or liable for any
                error or inaccuracy or for any losses suffered by any investor as a direct or indirect
                consequence of relying upon the data displayed by the calculator.
                <br />
                <br />
                The calculator, based on assumed rate of returns, is meant for illustration purposes
                only. The calculations are not based on any judgments of the future return of the
                debt and equity markets / sectors or of any individual security and should not be
                construed as a promise, guarantee or forecast on minimum returns. Protection against
                loss in a declining market is not guaranteed. Applicable taxes, fees, expenses and/or
                any other charges have not been considered in calculations and where the same if
                taken into consideration may reduce the returns on your actual investments. Dividend
                declarations, if any and Inflation have not been considered for calculation of returns.
                Please consult your tax/investment advisor before investing.
                <br />
                <br />
                <strong>Statutory Details:</strong> Principal Mutual Fund has been constituted as
                a trust with Principal Financial Group (Mauritius) Limited, Punjab National Bank
                and Vijaya Bank as the co-settlors. <strong>Sponsor</strong>: Principal Financial
                Services Inc., USA [acting through its wholly owned subsidiary Principal Financial
                Group (Mauritius) Ltd.]. <strong>Trustee</strong>: Principal Trustee Company Private
                Limited. <strong>Investment Manager:</strong> Principal Pnb Asset Management Company
                Private Limited (AMC). <strong>Risk Factors: Mutual funds and securities investments
                    are subject to market risks and there can be no assurance and no guarantee that
                    a scheme&#39;s objective can be achieved. As with any investment in securities,
                    the NAV of the units issued under a scheme can go up or down, depending upon the
                    factors and forces affecting the capital markets</strong>. Past performance
                of the Sponsor and any of its associates, co-settlors and/or AMC/ Mutual Fund does
                not indicate or guarantee the future performance of the Schemes of Principal Mutual
                Fund. <strong>Investors are urged to read the Scheme Information Document (SID), Statement
                    of Additional Information (SAI) &amp; Key Information Memorandum (KIM) carefully,
                    and consult their tax/ investment advisor before they invest in the scheme(s)</strong>.
                The Sponsor and any of their associates including co-settlors are not responsible
                or liable for any loss resulting from the operations of the Principal Mutual Fund
                beyond the initial contribution of an amount of Rs 25 lakhs towards setting up Principal
                Mutual Fund. Investors are not being offered a guaranteed or assured rate of return
                or monthly or regular/periodical income distribution, and the actual returns and/or
                periodical income distribution to an investor will be based on the actual NAV, which
                may go up or down, depending on the market conditions.
            </asp:Panel>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
