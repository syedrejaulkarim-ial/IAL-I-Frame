<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopFund.aspx.cs" Inherits="iFrames.Gomutualfund.TopFund" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta charset="utf-8"/>
  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
  <meta name="viewport" content="width=device-width, initial-scale=1"/>
  <meta name="description" content=""/>
  <meta name="author" content=""/>
  <link rel="shortcut icon" href="assets/ico/favicon.ico"/>

  <title>Top fund</title>

  <!-- CSS Plugins -->
  <link rel="stylesheet" href="font-awesome-4.7.0/css/font-awesome.min.css"/>
  <link rel="stylesheet" href="css/pe-icon-7-stroke.css"/>
  <!-- CSS Global -->
  <link rel="stylesheet" href="css/style.css"/>

    <link rel='stylesheet'  href='http://fonts.googleapis.com/css?family=Open+Sans%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic%7CRoboto%3A100%2C100italic%2C200%2C200italic%2C300%2C300italic%2C400%2C400italic%2C500%2C500italic%2C600%2C600italic%2C700%2C700italic%2C800%2C800italic%2C900%2C900italic&#038;subset=latin%2Clatin-ext&#038;ver=1.0.0' type='text/css' media='all' />

     <!-- JS Global -->
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>

    <!-- JS Plugins -->
    <script src="js/perfect-scrollbar.jquery.min.js"></script>
    <script src="js/bootstrap-slider.min.js"></script>

    <!-- JS Custom -->
    <script src="js/theme.min.js"></script>
    <script src="js/kite.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var mySlider = $("#ex2").slider();
            mySlider.slider('setValue', $("#HiddenMinimumInvesment").val());
            var mySlider1 = $("#ex8").slider();
            mySlider1.slider('setValue', $("#HiddenMinimumSIReturn").val());
            $("#ex2").change(function () {
                // alert($(this).val());
                $("#HiddenMinimumInvesment").val($(this).val());
            });
            $("#ex8").change(function () {
                // alert($(this).val());
                $("#HiddenMinimumSIReturn").val($(this).val());
            });
            $('#rdbOption').find('td:first').after('<td><label></label></td>');
        });
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!-- NAVBAR
    ================================================== -->


  <!-- WRAPPER
    ================================================== -->
  <div class="wrapper">

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
      <div class="row">
        <div class="col-xs-12 col-lg-10" style="margin:0 auto;">

          <!-- Basic form -->
          <div class="panel panel-default">
            <div class="panel-heading"> </div>
            <div class="panel-body">
              <div class="form-horizontal">
                  <div class="form-group">
                      <label class="col-sm-2 control-label-select">Category</label>
                      <div class="col-sm-5">
                          <asp:DropDownList ID="ddlCategory" class="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                      </div>
                      <div class="col-sm-5"></div>
                  </div>
                <div class="form-group">
                  <label class="col-sm-2 control-label-select">Sub-Category</label>
                  <div class="col-sm-5">
                    <asp:DropDownList class="form-control" ID="ddlSubCategory" runat="server"></asp:DropDownList>
                  </div>
                  <div class="col-sm-5"></div>
                </div>
                <div class="form-group">
                  <label class="col-sm-2 control-label-select">Type</label>
                  <div class="col-sm-5">
                     <asp:DropDownList class="form-control" ID="ddlType" runat="server"></asp:DropDownList>
                  </div>
                  <div class="col-sm-5"></div>
                </div>
                <div class="form-group">
                  <label for="inputPassword3" class="col-sm-2 control-label-select">Option</label>
                  <div class="col-sm-10">
                   <%-- <asp:RadioButtonList ID="rdbOption" runat="server" class="radio" RepeatDirection="Horizontal" Style="margin-left: 5px; padding-top: 5px;"></asp:RadioButtonList></div>--%>
                      <div class="style-radio2">
                          <asp:RadioButtonList ID="rdbOption" runat="server" class="radio" RepeatDirection="Horizontal" Style="margin-left: 5px; padding-top: 5px;"></asp:RadioButtonList>
                      </div>
                     <%-- <div class="col-sm-2" style="padding-left:0;">
                      <div class="radio">
                        <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1" checked>
                        <label for="optionsRadios1">Growth</label>
                      </div>
                    </div>
                    <div class="col-sm-8">
                      <div class="radio">
                        <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2">
                        <label for="optionsRadios2">Dividend</label>
                      </div>
                    </div>--%>
                  </div>
                </div>

                <div class="form-group">
                  <label class="col-sm-2 control-label-select">Period</label>
                  <div class="col-sm-5">
                      <asp:DropDownList ID="ddlPeriod" runat="server" class="form-control">
                                                <asp:ListItem Value="Per_7_Days" Text="Last 1 Week" />
                                                <asp:ListItem Value="Per_30_Days" Text="Last 1 Month" />
                                                <asp:ListItem Value="Per_91_Days" Text="Last 3 Months" />
                                                <asp:ListItem Value="Per_182_Days" Text="Last 6 Months" />
                                                <asp:ListItem Value="Per_1_Year" Text="Last 12 Months" Selected="True" />
                                                <asp:ListItem Value="Per_3_Year" Text="Last 3 Years" />
                                                <asp:ListItem Value="Per_5_Year" Text="Last 5 Years" />
                                            </asp:DropDownList>
                  </div>
                  <div class="col-sm-5"></div>
                </div>
                <div class="form-group">
                  <label class="col-sm-2 control-label-select">Rank</label>
                  <div class="col-sm-5">
                   <asp:DropDownList ID="ddlRank" runat="server" class="form-control">
                                                <asp:ListItem Text="All" Value="1000" />
                                                <asp:ListItem Text="Top 5" Value="5" />
                                                <asp:ListItem Text="Top 10" Value="10" />
                                                <asp:ListItem Text="Top 15" Value="15" />
                                                <asp:ListItem Text="Top 20" Value="20" />
                                                <asp:ListItem Text="Top 25" Value="25" />
                                            </asp:DropDownList>
                  </div>
                    <div class="col-sm-5"></div>
                </div>
                 &nbsp;
                <div class="form-group">
                  <label class="col-sm-2 control-label-select">Min. Investment</label>
                  <div class="col-sm-5">
                    <input id="ex2" data-slider-id='ex1Slider' type="text" data-slider-min="500" data-slider-max="10000" data-slider-step="1" data-slider-value="500"/>
                     <asp:HiddenField ID="HiddenMinimumInvesment" runat="server" Value="500" />
                    <div class="row">
                      <div class="col-xs-6">
                        <div class="slider__caption"><i class="fa fa-inr" aria-hidden="true" style="color:#252525; margin-top:2px;"></i> 500</div>
                      </div>
                      <div class="col-xs-6 text-right">
                        <div class="slider__caption"><i class="fa fa-inr" aria-hidden="true" style="color:#252525; margin-top:2px;"></i> 10,000</div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <label class="col-sm-2 control-label-select">Min. SI Return (%)</label>
                  <div class="col-sm-5">
                    <input id="ex8" data-slider-id='ex1Slider' type="text" data-slider-min="5" data-slider-max="50" data-slider-step="1" data-slider-value="5"/>
                        <asp:HiddenField ID="HiddenMinimumSIReturn" runat="server" Value="5" />
                    <div class="row">
                      <div class="col-xs-6">
                        <div class="slider__caption"> 5</div>
                      </div>
                      <div class="col-xs-6 text-right">
                        <div class="slider__caption">50</div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="form-group">
                  <div class="col-sm-2"></div>
                  <div class="col-sm-8">
                    <div class="btn-group" role="group" aria-label="Justified button group with nested dropdown">
                      <%--<a href="#" class="btn btn-primary" role="button">Submit</a>
                      <a href="#" class="btn btn-default" role="button">Reset</a>--%>
                         <asp:Button ID="btnSubmit" class="btn btn-primary" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                         <asp:Button ID="btnReset" class="btn btn-primary" runat="server" Text="Reset" OnClick="btnResetClick" />
                    </div>

                  </div>
                </div>
              </div>
            </div>
          </div>
          <!-- Bordered table -->
          <div class="col-xs-12 col-lg-2"></div>
        </div>
        <div class="col-xs-12 col-lg-2"></div>
        <!-- / .row -->
      </div>
      <div class="row" id="Result" runat="server">
        <div class="col-xs-12 col-lg-12">
          <div class="panel panel-default">
            <div class="panel-body">
              <div class="ribbon ribbon_primary">
                <div class="ribbon__title">
                 <asp:HiddenField ID="HiddenDivValue" runat="server" />
                   <asp:Label ID="lbtopText" runat="server" Text=""></asp:Label>
                </div>
              </div>
              <div class="table-responsive">
                <asp:ListView ID="lstResult" runat="server" OnPagePropertiesChanging="lst_PagePropertiesChanging">
                     <LayoutTemplate>
                                    <table class="table table-bordered">
                                         <thead>
                                        <tr class="table-header">
                                            <th style="text-align:center">
                                                <asp:Label ID="lnkRank" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Rank" Text="Sl. No." />
                                            </th>

                                            <th style="text-align:left">
                                                <asp:Label ID="lnkSchName" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="SchName" Text="Scheme Name" />
                                            </th>

                                            <th style="text-align:left">
                                                <asp:Label ID="lnkNature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Nature" Text="Category" />
                                            </th>

                                            <th style="text-align:left">
                                                <asp:Label ID="lnkSubnature" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Subnature" Text="Sub Category" />
                                            </th>
                                            
                                           

                                            <th style="text-align:right">
                                                <asp:Label ID="lnkNav" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="NAV" Text="NAV" />
                                            </th>

                                            <th style="text-align:right">
                                                <asp:Label ID="lnkPeriod" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="<%=ddlPeriod.SelectedValue%>"><%=ddlPeriod.SelectedItem.Text%></asp:Label>
                                            </th>

                                            <th style="text-align:right">
                                                <asp:Label ID="lnkInception" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Inception" Text="Since Inception" />
                                            </th>
                                             <th style="text-align:right">
                                                <asp:Label ID="Label1" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="Fund_Size" Text="Fund Size(Cr)" />
                                            </th>
                                            <%--<th style="text-align:right">
                                                <asp:Label ID="lblWatchList" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="WatchList" Text="Watch List"></asp:Label>
                                            </th>--%>
                                            <th style="text-align:center">
                                                <asp:Label ID="Label3" runat="server" SkinID="lblHeader" CommandName="Sort" CommandArgument="InvestNow" Text="Invest Now" Style="text-align: center"></asp:Label>
                                            </th>
                                        </tr>

                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                        </thead>
                                    </table>
                                    <div style="padding-top: 5px;">
                                    </div>
                                    <div>
                                        <table>
                                            <tr>

                                                <div style="width: 100%; float: left">
                                                    <asp:DataPager ID="dpTopFund" runat="server" PageSize="10" PagedControlID="lstResult">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                                                            <asp:NumericPagerField CurrentPageLabelCssClass="news_header" />
                                                            <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <span style="padding-left: 40px; text-align: right">Page
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
                                               
                                            </tr>
                                        </table>
                                    </div>

                                </LayoutTemplate>
                     <ItemTemplate>
                                    <tr class='<%# Convert.ToBoolean(Container.DataItemIndex % 2) ? "" : "" %>'>
                                        <td>
                                            <asp:Label runat="server" ID="lblType"><%#Eval("Rank") %></asp:Label>
                                        </td>
                                        <td style="width: 35%;">
                                           <%-- <a href="#" ><%#Eval("Sch_Name")%></a>--%>
                                           <a href="/Gomutualfund/Factsheet.aspx?param=<%#Eval("SchemeId")%>" target="_blank"><%#Eval("Sch_Name")%></a>
                                        </td>
                                        <td style="text-align:left">
                                            <asp:Label runat="server" ID="lblNature" Text='<%#Eval("Nature")%>' />
                                        </td>
                                        <td style="text-align:left">
                                            <asp:Label runat="server" ID="lblSubNature" Text='<%#Eval("SubNature")%>' />
                                        </td>
                                      
                                        <td style="text-align:right">
                                            <asp:Label runat="server" ID="lblNav" Text='<%#Eval("Nav")%>' />
                                        </td>
                                        <td style="text-align:right">
                                            <%# Convert.ToDouble(Eval(ddlPeriod.SelectedValue)).ToString("n2") %>
                                        </td>
                                        <td style="text-align:right">
                                            <%# Convert.ToDouble(Eval("Since_Inception")).ToString("n2")%>
                                        </td>
                                        <td style="text-align:right">
                                            <%# Convert.ToDouble(Eval("Fund_Size")).ToString("n2") %>
                                        </td>
                                        <%--<td style="text-align:center">
                                            <img src="images/watch.jpg" style="cursor: pointer" alt="" name="imgAdd2Watch" id='<%#Eval("SchemeId")%>' />                                            
                                            
                                        </td>--%>
                                        <td style="text-align:center">
                                            <a target="_blank" href="http://gomutualfund.com/contact">
                                            <img src="img/rupee.png" style="cursor: pointer" alt="" onclick="callCross('<%#Eval("SchemeId")%>','<%#Eval("Sch_Name")%>','<%#Eval("OptionId")%>','<%#Eval("Nature")%>','<%#Eval("SubNature")%>')" />

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
        <!-- Footer -->
        <footer class="page__footer">
          <div class="row">
            <div class="col-xs-12">
              <span class="page__footer__year"></span> Developed for Gomutualfund by: <a href="https://www.icraanalytics.com" target="_blank" >ICRA Analytics Ltd</a>&nbsp;<a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer" target="_blank" >Disclaimer</a>
            </div>
          </div>
        </footer>

     
      <!-- / .container-fluid -->

    </div>
    <asp:HiddenField ID="HiddenFundRisk" runat="server" Value="-1" />
    <asp:HiddenField ID="HiddenFundRiskStrColor" runat="server" Value="All" />
   <asp:HiddenField ID="hdIsLoad" runat="server" Value="0" />
   <asp:HiddenField ID="Userid" runat="server" Value="asas" />
    </form>
</body>
</html>
