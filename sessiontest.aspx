<%@ Page Language="VB" AutoEventWireup="false" CodeFile="sessiontest.aspx.vb" Inherits="sessiontest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var sessionTimeout = "<%= Session.Timeout %>";
        function DisplaySessionTimeout() {
            //assigning minutes left to session timeout to Label
            document.getElementById("<%= lblSessionTime.ClientID %>").innerText =
                                                                        sessionTimeout;
            sessionTimeout = sessionTimeout - 1;

            //if session is not less than 0
            if (sessionTimeout >= 0)
            //call the function again after 1 minute delay
                window.setTimeout("DisplaySessionTimeout()", 60000);
            else {
                //show message box
                alert("Your current Session is over.");
            }
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblSessionTime" runat="server" Text="Label"></asp:Label>
    </div>
    
    </form>
</body>
</html>
