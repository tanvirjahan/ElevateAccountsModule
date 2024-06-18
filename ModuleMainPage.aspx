<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ModuleMainPage.aspx.vb" Inherits="ModuleMainPage" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   
    <script language="javascript" type="text/javascript">
    function divLogOut_onclick()
			{
			  alert("You are successfully logged out.");
			  parent.location.href = 'login.aspx';
		    }
    </script>
<%--   <script language="javascript" type="text/javascript">
        window.moveTo(0, 0)
        window.resizeTo(screen.availWidth, screen.availHeight)
    </script>
--%>    <script type="text/javascript">
        function SetTarget() {
            document.forms[1].target = "_blank";

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table align="center" border="4" bordercolor="#06788B" bordercolordark="#06788B" bordercolorlight="#06788B" cellspacing="2" style="height: 400px; width: 800px;">
            <tr>
                <td style="height:65px" align="center" >
                    <img src="Images/loginlogo.png"  style="max-width :420px; max-height:120px;"/>
                    
                    </td>
            </tr>
            <tr>
                <td align="left" style="width: 40%">
                    <span style="color: #0000ff; text-decoration: underline">
                        </span>
                    <asp:Label ID="lblCurrentDate" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Current Date:"></asp:Label>&nbsp;
                    <asp:Label ID="lblLoggedAs" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Logged As : Admin"></asp:Label>
                    <FONT face="Arial" size="2">|</FONT>
						<DIV language="javascript" id="Divlogout" onmouseover="style.color='#06788B';style.backgroundColor ='white';"
							style="BORDER-RIGHT: gray 1px; BORDER-TOP: gray 1px; DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 8pt; BORDER-LEFT: gray 1px; CURSOR: hand; COLOR:#06788B  ; BORDER-BOTTOM: gray 1px; FONT-FAMILY: Arial; BORDER-COLLAPSE: separate; BACKGROUND-COLOR: white; TEXT-ALIGN: center"
							onclick="divLogOut_onclick()" onmouseout="style.color='#06788B';style.backgroundColor ='white';" >Logout</DIV>
                    
                    </td>
                    
            </tr>
            <tr>
                <td align="left" style="height: 300px; width: 40%;">
                    
                     <table align="center">
                        <tr>
                            <td style="text-align: center; width: 30px">
                                <asp:Label ID="Label1" runat="server" BackColor="white" BorderColor="#06788B"
                                    BorderStyle="Solid"  Font-Bold="True" Font-Size="X-Large" Text="Select Module" ForeColor="#084573"
                                    Width="329px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 30px; height: 45px;">
                                <asp:Table ID="Table1" runat="server" Width="336px" BackColor="Transparent">
                     
                                </asp:Table>
                            </td>
                        </tr>
                    </table>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" style="font-size: 8pt; font-family: Tahoma; width: 40%;">
                    Designed and Developed by MAHCE <a id="HyperLink1" href="http://www.mahce.com/" target="_blank">
                        http://www.mahce.com/</a></td>
            </tr>
        </table>
        &nbsp;&nbsp;
    
    </div>
    </form>
</body>
</html>
