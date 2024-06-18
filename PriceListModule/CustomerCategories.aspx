<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CustomerCategories.aspx.vb" Inherits="CustomerCategories" MasterPageFile="~/SubPageMaster.master" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%--<title>CustomerCategories</title>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">

        function checkNumber(e) {
            if (event.keyCode == 46)
                return true

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }
     
        function FormValidation(state) {
            if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtCode.ClientID%>").value == 0) || (document.getElementById("<%=txtCodeName .ClientID%>").value == "")) {
                if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    alert("Code field can not be blank");
                    return false;
                }

                else if (document.getElementById("<%=txtCode.ClientID%>").value == 0) {
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    alert("Name field can not be zero");
                    return false;
                }

                else if (document.getElementById("<%=txtCodeName.ClientID%>").value == "") {
                    document.getElementById("<%=txtCodeName.ClientID%>").focus();
                    alert("Name field can not be blank");
                    return false;
                }
      }
            else {
                if (state == 'New') { if (confirm('Are you sure you want to save Customer Category?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Customer Category?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Customer Category?') == false) return false; }
            }
        }

        
   
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
         <TABLE style="border: 2px solid gray; width: 320px;">
                <tbody>
                    <tr>
                      <TD  colspan ="2"class="td_cell" align="center" style="width: 49px">
                      

     <asp:Label id="lblCustCatHead" runat="server" Text="Customer Categories" ForeColor="White" 
                              Width="310px" CssClass="field_heading" Height="21px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                      
                                       <TD style="WIDTH: 117px" class="td_cell"><SPAN>Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
                                       <TD style="COLOR: #000000; width: 355px;" >
                             
                                            <input style="width: 234px"  id="txtCode" class="field_input" tabindex="1" type="text"
                                             maxLength=20    runat="server" />
                                        </td>
</tr>
           <tr>
                                        <TD style="WIDTH: 117px; HEIGHT: 24px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
                                        <td  style="width: 355px" class="td_cell"  >
                                            <input style="width: 234px" id="txtCodeName" class="field_input" tabindex="2" type="text"
                                                runat="server" />
                                        </td>
                                      
                                    </tr>
                                      <tr>
                                       <TD style="WIDTH: 117px; HEIGHT: 16px" class="td_cell">Active</TD>
                                        <td style="width: 355px; HEIGHT: 16px"  >
                                            <input id="chkActive" tabindex="3" type="checkbox" checked runat="server" />
                                        </td>
                                    
                                    </tr>
                                                    <TR>
    <TD style="WIDTH: 117px"><asp:Button id="btnSave" tabIndex=4 runat="server" Text="Save" CssClass="btn"></asp:Button>    </TD>
    <TD style="width: 355px"><asp:Button id="btnCancel" tabIndex=5 runat="server" 
            Text="Return To Search" CssClass="btn" Width="146px"></asp:Button>&nbsp;&nbsp; 
        <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w9" CssClass="btn" 
            onclick="btnhelp_Click" tabIndex="7" Text="Help" />
    </TD>
                                  </TR>   
                                              </tbody>
                                            </table>
                                            
         <TABLE style=" width: 474px;">
                <tbody>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </td>
                    </tr>
                    <caption>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </caption>
                                                </tbody>
                                            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
