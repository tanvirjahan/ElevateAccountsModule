<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="ChangeUserPassword.aspx.vb" Inherits="ChangeUserPassword" %>
 

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
       <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
        <table align="center" style="width:600px; height: 300px;" border="4"  bordercolor="#06788B"  bordercolordark="#06788B" bordercolorlight="#06788B">
          <tr>
                <td align="center"  class="field_heading">
                    Change Password</td>
            </tr>
          
          <tr>           
                <td >
                  
        <table bgcolor="#FFFFFF" border="0" bordercolor="scrollbar" cellpadding="2" cellspacing="0" style="width:600px;  border-right: scrollbar 1px solid; border-top: scrollbar 1px solid; border-left: scrollbar 1px solid; border-bottom: scrollbar 1px solid;" align="center">
                    
            <tr>
            <td  class="td_cell"  >User Code </td>
                <td class="td_cell" >
                    <asp:TextBox ID="txtUserCode"   runat="server"  Enabled ="false"  class="field_input"   ></asp:TextBox>
                </td>
             

                <td  class="td_cell" >User Name </td>
                <td class="td_cell">
                    <asp:TextBox ID="txtUserName" runat="server" Enabled ="false" class="field_input"  ></asp:TextBox></td>
            </tr>
            <tr>
                <td   class="td_cell">Original Password </td> 
                <td   class="td_cell"  colspan="2">
                    <asp:TextBox ID="txtOriginalPassword" runat="server" width="100%" class="field_input" TextMode="Password" TabIndex="1"></asp:TextBox></td>
                    <td  class="td_cell" >&nbsp;
                </td>
               <%-- <td  class="td_cell" >&nbsp;
                </td>--%>
            </tr>
            <tr>
                <td   class="td_cell">New Password</td>
                <td   class="td_cell" colspan="2" >
                    <asp:TextBox ID="txtNewPassword" runat="server" width="100%" class="field_input" TextMode="Password" TabIndex="2"></asp:TextBox></td>
                    <td  class="td_cell" >&nbsp;
                </td>
                <%--<td  class="td_cell" >&nbsp;
                </td>--%>
            </tr>
             <tr>
                <td   class="td_cell">Re-Type New Password </td>
                <td   class="td_cell" colspan="2">
                    <asp:TextBox ID="txtReNewPassword" runat="server" width="100%" class="field_input" TextMode="Password" TabIndex="2"></asp:TextBox></td>
                    <td  class="td_cell" >&nbsp;
                </td>
               <%-- <td  class="td_cell" >&nbsp;
                </td>--%>
            </tr>
            
            <tr>
                <td   class="td_cell" colspan="4">&nbsp;
                </td>
               
            </tr>
             <tr>
        <td   colspan="4">
            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                  tabIndex="13" Text="Save" />&nbsp;
        
            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                 tabIndex="14" Text="Cancel" />
           <%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnhelp" runat="server" __designer:wfdid="w8" 
                CssClass="field_button" onclick="btnhelp_Click" tabIndex="15" Text="Help" />--%>
        </td>
    </tr>
             <tr>
                <td   class="td_cell" colspan="4">&nbsp;
                </td>
               
            </tr>
        </table>
      </td> 
      </tr>
      </table> 
     </ContentTemplate>
                    </asp:UpdatePanel>
    </div>


     </form>
</body>
</html>
