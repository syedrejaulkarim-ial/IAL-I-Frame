<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="iFrames.HDFC_SIP.Services" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var strjsn = '{ UserId: "SSO@dspim.com", Password: "7BB5D3BE-DFFE-4753-B662-3235A5AFF80E",Date:"' + $.now() + '"}';
            var encodedString = btoa(strjsn);
            console.log(JSON.stringify({ Data: encodedString }));
            jQuery.ajax({

                url: 'http://mfiframes.mutualfundsindia.com/DSPApp/Services.aspx/GetToken',
                //url: 'http://localhost:20801/DSPApp/Services.aspx/GetToken',
                //url: 'http://localhost:20801/DSPApp/Services.aspx',

                //url: 'http://mfiframes.mutualfundsindia.com/DSPApp/Services.aspx',
                type: "POST",
                //data: JSON.stringify({ UserId: "SSO@dspim.com", Password: "7BB5D3BE-DFFE-4753-B662-3235A5AFF80E" }),
                data: 'Data=' + encodedString,
               // contentType: "text/plain",

                // contentType: "application/json",
                contentType: "application/x-www-form-urlencoded",

                success: function (data) {
                    // console.log(data.d)
                    //var _data = data;
                    debugger;
                    window.location.href = 'http://mfiframes.mutualfundsindia.com/DSPApp/Login.aspx?Token=' + JSON.parse(data).Token + '&Email=admin@dspim.com';
                    //window.location.href = 'http://localhost:20801/DSPApp/Login.aspx?Token=' + JSON.parse(data).Token + '&Email=admin@dspim.com';
                },
                error: function (d) {
                    debugger;
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
      ============== this portion is not commited =====================
</body>

 


</html>
