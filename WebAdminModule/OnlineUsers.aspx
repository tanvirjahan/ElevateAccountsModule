<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OnlineUsers.aspx.vb" Inherits="WebAdminModule_OnlineUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Users</title>
        <script src="Js/Jquery-1.7.2.min.js" type="text/javascript"></script>
        <link href="Css/TableCss.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">

            jQuery(document).ready(function () {

                jQuery('.LogOffUserClass').click(function () {

                    LogOffUser(jQuery(this).att("LoginUserId"));

                });

            });

            function LogOffUser(ProfileId) {
                jQuery.ajax({
                    type: "post",
                    url: '../MyChatWebService.asmx/LogOffChat',
                    dataType: "json",
                    data: "{'ProfileId': '" + ProfileId + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (status) {
                        var result = status.d;
                    },
                    error: function (x) {
                    }
                });
            }

        </script>

  
</head>
<body>
    <form id="form1" runat="server">
    <a href='javascript:void(0)' onclick="window.close();" style='text-align:right'>Close</a>
   
    
    <div id="ChatUsers" runat="server" class="ChatUserClass">
    
    </div>
    </form>
</body>
</html>
