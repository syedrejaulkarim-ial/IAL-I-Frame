<%@ Page Language="C#" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="HistNav.aspx.cs" Inherits="iFrames.ValueInvest.HistNav" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Historical NAV</title>
    <link href="css/stylesheet.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/date.js"></script>
    <script type="text/javascript" language="javascript">

        function ValidateControl() {

            var selectedFund = $('#<%=ddlFundHouse.ClientID %>').find(':selected').val();
            if (selectedFund == 0) {
                alert('Please select any Fund House.');
                $('#<%=ddlFundHouse.ClientID %>').focus();
                return false;
            }

            var selectedValue = $('#<%=listboxSchemeName.ClientID%> option:selected').val();
            if (selectedValue == null) {
                alert('Please select any Scheme.');
                $('#<%=listboxSchemeName.ClientID %>').focus();
                return false;
            }


            var regex = new RegExp("^[0-9]{2} (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) [0-9]{4}$", "i");

            if ($('#<%=txtfromDate.ClientID %>').val() == '') {
                alert('Please enter From date.');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if ($('#<%=txtToDate.ClientID %>').val() == '') {
                alert('Please enter To date.');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            if (!$('#<%=txtfromDate.ClientID %>').val().match(regex)) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }
            if (Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!dtvalid(Date.parse($('#<%=txtfromDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!$('#<%=txtToDate.ClientID %>').val().match(regex)) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }
            if (Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy") == null) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            if (!dtvalid(Date.parse($('#<%=txtToDate.ClientID %>').val(), "dd MMM yyyy").toString("dd/MM/yyyy"))) {
                alert('Please provide the date in dd MMM yyyy format, e.g. 12 Jan 2012');
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }

            var frmdate = converterdate($('#<%=txtfromDate.ClientID %>').val());
            var todate = converterdate($('#<%=txtToDate.ClientID %>').val());


            if (!IsValidDate(frmdate, todate)) {
                alert("From Date should be Less than To Date ");
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }


            if (!LessThanToday(frmdate)) {
                alert("From Date should be Less than Today ");
                $('#<%=txtfromDate.ClientID %>').focus();
                return false;
            }

            if (!LessThanToday(todate)) {
                alert("To Date should be Less than Today ");
                $('#<%=txtToDate.ClientID %>').focus();
                return false;
            }





            return true;
        }


        $(function () {

            $('#<%=txtfromDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "img/calenderb.jpg",
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                setDate: "-2 d",
                maxDate: -2
            });

            $('#<%=txtToDate.ClientID %>').datepicker({
                showOn: "button",
                buttonImage: "img/calenderb.jpg",
                buttonImageOnly: true,
                //                buttonText: "Select Date",
                dateFormat: 'dd M yy',
                changeMonth: true,
                changeYear: true,
                setDate: '-1d',
                maxDate: -1
            });

            $('#btnReset').click(function (ev) {
                window.location.reload(true);
            });

            setDates();

            //            $('#<%=txtfromDate.ClientID %>').val(Date.parse('today').add(-8).days().toString("dd MMM yyyy"));
            //            $('#<%=txtToDate.ClientID %>').val(Date.parse('today').add(-2).days().toString("dd MMM yyyy"));
        });

        function setDates() {
            var dateOffset = (24 * 60 * 60 * 1000);
            var myDate = new Date();
            var frmdate = new Date(myDate.getTime() - dateOffset * 8);
            var todate = new Date(myDate.getTime() - dateOffset * 2);

            $('#<%=txtfromDate.ClientID %>').datepicker().datepicker('setDate', frmdate);
            $('#<%=txtToDate.ClientID %>').datepicker().datepicker('setDate', todate);
        }
    </script>
      <script type="text/javascript">
          var dminyear = 1900;
          var dmaxyear = 2200;
          var chsep = "/";
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
          }

          function checkinteger(str1) {
              var x;
              for (x = 0; x < str1.length; x++) {
                  // verify current character is number or not !   
                  var cr = str1.charAt(x);
                  if (((cr < "0") || (cr > "9")))
                      return false;
              }
              return true;
          }
          function getcharacters(s, chsep1) {
              var x;
              var Stringreturn = "";
              for (x = 0; x < s.length; x++) {
                  var cr = s.charAt(x);
                  if (chsep.indexOf(cr) == -1)
                      Stringreturn += cr;
              }
              return Stringreturn;
          }
          function februarycheck(cyear) {
              return (((cyear % 4 == 0) && ((!(cyear % 100 == 0)) || (cyear % 400 == 0))) ? 29 : 28);
          }
          function finaldays(nr) {
              for (var x = 1; x <= nr; x++) {
                  this[x] = 31
                  if (x == 4 || x == 6 || x == 9 || x == 11) {
                      this[x] = 30
                  }
                  if (x == 2) {
                      this[x] = 29
                  }
              }
              return this
          }

          function converterdate(string) {
              var d = string.split(/[ :\s]/);
              var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'sep', 'Oct', 'Nov', 'Dec'];
              var monthval = 1;

              for (var i = 0; i < months.length; i++) {
                  if (months[i] == d[1]) {
                      monthval = parseInt(i + 1);
                      break;
                  }
              }

              if (monthval < 10) {
                  monthval = '0' + monthval;
              }

              return d[0] + '/' + monthval + '/' + d[2];
          }

          function dtvalid(strdate) {
              var monthdays = finaldays(12)
              var cpos1 = strdate.indexOf(chsep)
              var cpos2 = strdate.indexOf(chsep, cpos1 + 1)
              var daystr = strdate.substring(0, cpos1)
              var monthstr = strdate.substring(cpos1 + 1, cpos2)
              var yearstr = strdate.substring(cpos2 + 1)
              strYr = yearstr
              if (strdate.charAt(0) == "0" && strdate.length > 1) strdate = strdate.substring(1)
              if (monthstr.charAt(0) == "0" && monthstr.length > 1) monthstr = monthstr.substring(1)
              for (var i = 1; i <= 3; i++) {
                  if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
              }
              // The parseInt is used to get a numeric value from a string   
              pmonth = parseInt(monthstr)
              pday = parseInt(daystr)
              pyear = parseInt(strYr)
              if (cpos1 == -1 || cpos2 == -1) {
                  //alert("The date format must be : dd/mm/yyyy")
                  return false
              }
              if (monthstr.length < 1 || pmonth < 1 || pmonth > 12) {
                  //alert("Input a valid month")
                  return false
              }
              if (daystr.length < 1 || pday < 1 || pday > 31 || (pmonth == 2 && pday > februarycheck(pyear)) || pday > monthdays[pmonth]) {
                  //alert("Input a valid day")
                  return false
              }
              if (yearstr.length != 4 || pyear == 0 || pyear < dminyear || pyear > dmaxyear) {
                  //alert("Input a valid 4 digit year between " + dminyear + " and " + dmaxyear)
                  return false
              }
              if (strdate.indexOf(chsep, cpos2 + 1) != -1 || checkinteger(getcharacters(strdate, chsep)) == false) {
                  //alert("Input a valid date")
                  return false
              }
              return true
          }

          function IsValidDate(str1, str2) {

              var Day = parseInt(str1.substring(0, 2), 10);
              var Mn = parseInt(str1.substring(3, 5), 10);
              var Yr = parseInt(str1.substring(6, 10), 10);
              var DateVal = Mn + "/" + Day + "/" + Yr;
              var dt = new Date(DateVal);

              var Day1 = parseInt(str2.substring(0, 2), 10);
              var Mn1 = parseInt(str2.substring(3, 5), 10);
              var Yr1 = parseInt(str2.substring(6, 10), 10);
              var DateVal1 = Mn1 + "/" + Day1 + "/" + Yr1;
              var dt1 = new Date(DateVal1);

              if (dt.getDate() != Day) {
                  alert('Invalid Date');
                  return false;
              }
              else if (dt.getMonth() != Mn - 1) {
                  //this is for the purpose JavaScript starts the month from 0
                  alert('Invalid Date');
                  return false;
              }
              else if (dt.getFullYear() != Yr) {
                  alert('Invalid Date');
                  return false;
              }


              if (dt1.getDate() != Day1) {
                  alert('Invalid Date');
                  return false;
              }
              else if (dt1.getMonth() != Mn1 - 1) {
                  //this is for the purpose JavaScript starts the month from 0
                  alert('Invalid Date');
                  return false;
              }
              else if (dt1.getFullYear() != Yr1) {
                  alert('Invalid Date');
                  return false;
              }
              // alert(dt); alert(dt1);
              if (dt >= dt1) {
                  return false;
              }
              return true;
          }


          function LessThanToday(strDate) {
              var Day = parseInt(strDate.substring(0, 2), 10);
              var Mn = parseInt(strDate.substring(3, 5), 10);
              var Yr = parseInt(strDate.substring(6, 10), 10);
              var DateVal = Mn + "/" + Day + "/" + Yr;
              var dt = new Date(DateVal);

              var todaydate = new Date();
              //alert(todaydate);alert(dt);
              var i = DateDiff.inDays(dt, todaydate)
              if (i <= 0) {
                  return false;
              }
              return true;
          }

    </script>
</head>
<body>
    <form runat="server" id="form1">

        <table border="0" cellspacing="0" cellpadding="0" width="900" align="left" class="main-content">
            <tr>
                <td>
                    <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="value_heading" colspan="2">
                                <img src="img/arw1.jpg" />Historical NAV</td>

                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table width="90%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_input">Fund House</td>
                                                    <td>
                                                        <%--                                                        <select name="" class="value_input1">
                                                            <option>Axis Mutual Fund</option>
                                                            <option>Baroda Pioneer Balance Fund</option>
                                                            <option>Birla Sun Life 95- Dividend</option>
                                                        </select>--%>
                                                        <asp:DropDownList ID="ddlFundHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFundHouse_SelectedIndexChanged"
                                                            CssClass="value_input1">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="value_input">Scheme Name </td>
                                                    <td>
                                                        <%--<select name="" class="value_input1">
                                                            <option>Axis Banking Debt Fund - Direct - Dly Dividend</option>
                                                            <option>Baroda Pioneer Balance Fund - Direct - Dividend</option>
                                                            <option>Birla Sun Life 95- Dividend</option>
                                                        </select>--%>
                                                        <asp:DropDownList ID="listboxSchemeName" runat="server" OnSelectedIndexChanged="listboxSchemeName_SelectedIndexChanged"
                                                            CssClass="value_input1">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td class="value_input3">From Date</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="value_txtbox"></asp:TextBox></td>
                                                                <td class="value_input3" style="width: 50px">To Date</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="value_txtbox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="Javascript:return ValidateControl();"
                                                                        OnClick="btnSubmit_Click" CssClass="value_button" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr class="top_td">
                                                    <td class="top" colspan="2">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="top_text">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <!--<table width="80%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                            <tr class="top_tableheader">
                                                            <td class="value_tableheader">Scheme Name</td>
                                                            <td class="value_tableheader">Date</td>
                                                            <td class="value_tableheader">Nav (<img src="img/rupee_v.png"/>)</td>
                                                                <td>
                         	                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                    <td colspan="3" class="value_tableheader"><div align="center">Performance % as on Mar 19, 2013</div></td>
                                                                    </tr>
                                                                    <tr>
                                                                    <td align="center" class="value_tableheader">91 Days</td>
                                                                    <td align="center" class="value_tableheader">182 Days</td>
                                                                    <td align="center" class="value_tableheader">1 Year</td>
                                                                    </tr>
                                                                </table>
                                                                </td>
                                                            <tr>
                                                            <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                            <td class="value_tablerow">Mar 19, 2013</td>
                                                            <td class="value_tablerow">8.0000</td>
                                                                <td class="value_tablerow">
                         	                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                    <td width="37%" align="center">NA</td>
                                                                    <td width="35%" align="center">NA</td>
                                                                    <td width="28%" align="center">NA</td>
                                                                    </tr>
                                                                </table>

                                                                </td>
                                                            </tr>
                                                        </table>-->
                                                        <%--<table width="90%" border="0" align="left" cellpadding="0" cellspacing="0" class="value_table">
                                                        <tr class="value_tableheader">
                                                        <td class="value_tableheader">Scheme Name</td>
                                                        <td class="value_tableheader">NAV Date</td>
                                                        <td class="value_tableheader">NAV</td>
                                                        <tr>
                                                        <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                        <td class="value_tablerow">Mar 19, 2013</td>
                                                        <td class="value_tablerow">19.11</td>
                                                        </tr>
                                                        <tr>
                                                        <td class="value_tablerow">HDFC Annual Interval Fund - Series I - Plan A</td>
                                                        <td class="value_tablerow">Mar 19, 2013</td>
                                                        <td class="value_tablerow">19.11</td>
                                                        </tr>
                                                        </table>--%>

                                                        <asp:GridView runat="server" ID="gvNavHistDetail" Width="90%" AutoGenerateColumns="false"
                                                            CssClass="value_table">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%#(Eval("Sch_Short_Name") == DBNull.Value) ? "--" : Eval("Sch_Short_Name").ToString() %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nav Date" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%# Convert.ToDateTime(Eval("Nav_Date")).ToString("dd MMM yyyy")%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nav" HeaderStyle-CssClass="value_tableheader">
                                                                    <ItemTemplate>
                                                                        <%# Eval("Nav").ToString()  %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="value_tablerow" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <tr>
                                                                    <td class="value_tablerow" style="border: none">Data not Found
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="value_dis">Disclaimer: Mutual Fund investments are subject to market risks. Read all scheme related documents carefully before investing. Past performance of the schemes do not indicate the future performance. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="value_btm_text">Developed and Maintained by: ICRA Analytics Ltd </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="70%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="value_btm_text">&nbsp; </td>
                        </tr>

                    </table>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
