<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MF_News.aspx.cs"
    Inherits="iFrames.MF_News" %>

<!DOCTYPE html>

<html class="no-js">

<head>
    <title>Mutual Fund News</title>

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/IAL_style.css" rel="stylesheet" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="js/new/bootstrap.js"></script>
    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>

        <![endif]-->
    <!-- Do not delete It will required for login , Invest now -->

    <style>
        a:hover, a:focus {
            color: #e46812;
            text-decoration: underline;
        }
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
        .blog-post-title {
            margin-bottom: 5px;
            margin-top:5px;
            font-size: 12px;
            text-align: justify;
        }

        .blog-post-meta {
            padding-bottom: 4px;
            color: #999;
            font-size: 12px;
        }

        .blog-post {
            border-bottom: 1px solid rgba(0, 0, 0, 0.125);
        }

            .blog-post:last-child {
                border-bottom: none;
            }

        /*Custom Scroll*/
        /*.scrollbar {
            overflow-y: scroll;
        }

        .force-overflow {
            height: 350px;
        }

        #style-3::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            background-color: #F5F5F5;
        }

        #style-3::-webkit-scrollbar {
            width: 6px;
            background-color: #F5F5F5;
        }

        #style-3::-webkit-scrollbar-thumb {
            background-color: #898989;
        }*/

    </style>
</head>
<body>
    <div style="width: 350px; height:350px">
        <div class="card" style="border: none">
            <div class="card-body">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="card" style="border: none">
                            <div class="card-header bg-header">
                                <h5>Mutual Fund News</h5>
                            </div>
                            <div class="card-body">
                                <div class="pt-2">

                                    <asp:Repeater ID="news" runat="server">
                                        <HeaderTemplate></HeaderTemplate>
                                        <ItemTemplate>
                                            <table width="100%" class="blog-post">
                                                <tr>
                                                    <td style="width:76%">
                                                        <h5 class="blog-post-title"><a href="#" title="<%#Eval("Title")%>"
                                                            data-toggle="modal"
                                                            data-target="#myModal<%#Eval("rowid")%>"><%#Eval("Title").ToString().Length>44?Eval("Title").ToString().Substring(0,42)+"...":Eval("Title").ToString()%></a>
                                                        </h5>
                                                    </td>
                                                    <td style="width:20%; text-align:right">
                                                        <p class="blog-post-meta">
                                                            <%#Eval("PublishedOn","{0:dd MMM yyyy}")%>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </ItemTemplate>
                                        <FooterTemplate></FooterTemplate>
                                    </asp:Repeater>
                                    <div style="font-size: 10px; color: #A7A7A7" class="pt-2">
                                        Developed by: <a href="https://www.icraanalytics.com"
                                            target="_blank" style="font-size: 10px; color: #999999">
                                            ICRA Analytics Ltd</a>,
                                    <a style="font-size: 10px; color: #999999" href="https://icraanalytics.com/home/Disclaimer"
                                        target="_blank">Disclaimer</a>
                                    </div>
                                </div>
                                
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
                    
    <!-- Modal -->

    <asp:Repeater ID="newsptr" runat="server">
        <HeaderTemplate></HeaderTemplate>
        <ItemTemplate>

            <div class="modal fade" id="myModal<%#Eval("rowid")%>"
                tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"
                                aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel"><%#Eval("Title")%>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <%#Eval("HtmlBody")%>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </asp:Repeater>

</body>

</html>
