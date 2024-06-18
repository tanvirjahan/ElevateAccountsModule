<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" EnableEventValidation="false"  Title=":: ELEVATE TOURISM L.L.C ::"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>:: ELEVATE TOURISM L.L.C ::</title>
     <link rel="stylesheet" href="CSS/Styles.css" type="text/css" />

     	  <script language="javascript" type="text/javascript">
     	      window.moveTo(0, 0)
     	      window.resizeTo(screen.availWidth, screen.availHeight)
            </script>
</head>

<body> 
    <form id="form1" runat="server">
    <div style="text-align: center">
         <table align="center" style="width:600px; height: 300px" border="4" bordercolor="#06788B" bordercolordark="#06788B" bordercolorlight="#06788B" cellspacing="2" id="TABLE1" language="javascript">
            <tr>
                <td align="center"> 
                    <img src="Images/loginlogo.png"  style="max-width :420px; max-height:120px;"/>
                    </td>
                  
            </tr>
            <tr>
            
                <td align="center" style="height: 320px;">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
        <table bgcolor="#FFFFFF" border="0" bordercolor="scrollbar" cellpadding="2" cellspacing="0" style="width: 360px; border-right: scrollbar 1px solid; border-top: scrollbar 1px solid; border-left: scrollbar 1px solid; border-bottom: scrollbar 1px solid;" align="center">
            <tr>
                <td align="center" colspan="2" class="field_heading">
                    Log In</td>
            </tr>
            <tr>
                <td align="center" class="td_cell" >User Name :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="logintxtbox" TabIndex="1"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="td_cell" style="height: 15px">Password :
                </td>
                <td align="left" style="height: 15px">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="logintxtbox" TextMode="Password" TabIndex="2"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="td_cell" colspan="2" style="height: 15px">
                    <asp:CheckBox ID="chkRemember" runat="server"
                        Text="Remember me next time." Width="183px" CssClass="td_cell" TabIndex="3" /></td>
            </tr>
            <tr>
                <td align="center" class="td_cell" style="height: 15px">
                </td>
                <td align="left" style="height: 15px">
                    <select id="ddlDbName" runat="server" 
                        class="logindrpdown"                       style="width: 130px" tabindex="4" 
                        visible="true" name="ddlDbName">
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="height: 23px"><asp:Button ID="btnLogIn" runat="server" Text="Log In" CssClass="btn" BorderStyle="Solid" BorderWidth="1px" TabIndex="5" />&nbsp;
                    <asp:Button ID="btnChangePwd" onclick="btnChangePwd_Click" runat="server" Text="Change Password"  CssClass="btn" BorderStyle="Solid" BorderWidth="1px" TabIndex="6"/>&nbsp;
                    </td>
            </tr>
        </table>
                    <table bgcolor="#ffffff" border="0" bordercolor="scrollbar" cellpadding="2" cellspacing="0" style="width: 360px; border-right: scrollbar 1px solid; border-top: scrollbar 1px solid; border-left: scrollbar 1px solid; border-bottom: scrollbar 1px solid;" align="center">
                        <tr>
                            <td align="center" colspan="2" class="field_heading">
                                Forgot Your Password?</td>
                        </tr>
                        <tr style="font-style: italic; color: #000000;">
                            <td align="center" colspan="2" style="font-size: 9pt; font-family: Verdana; height: 14px">
                                Enter your User Name to receive your password.</td>
                        </tr>
                        <tr style="color: #000000">
                            <td align="center" class="td_cell" style="height: 26px">
                                <span>User Name :</span></td>
                            <td align="left" style="height: 26px">
                                <asp:TextBox ID="txtFUserName" runat="server" Font-Names="Verdana" 
                                    Font-Size="9pt" CssClass="logintxtbox"></asp:TextBox> 
                     &nbsp; &nbsp;<asp:Button ID="btnSubmit" onclick="btnSubmit_Click" runat="server" Text="Submit"  CssClass="btn" BorderStyle="Solid" BorderWidth="1px" /> </td>
                        </tr>
                       <%-- <tr style="color: #000000">
                            <td align="right" colspan="2" style="height: 23px">
                            
                               </td>
                        </tr>--%>
                    </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;</td>
            </tr>
            <tr style="color: #000000">
                <td align="center" style="font-size: 8pt; font-family: Tahoma">
                    Designed and Developed by MAHCE <a id="HyperLink1" href="http://www.mahce.com/" target="_blank">
                        http://www.mahce.com/</a></td>
            </tr>
        </table>
        &nbsp;
    
    </div>
    </form>
</body>
</html>
