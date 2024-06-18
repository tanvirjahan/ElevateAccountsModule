<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HotelRemarksTemplate.aspx.vb" Inherits="HotelRemarksTemplate" MasterPageFile="~/SubPageMaster.master" %>

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
                if (state == 'Edit') {
                    if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                        document.getElementById("<%=txtCode.ClientID%>").focus();
                        alert("Remarks Code field can not be blank");
                        return false;
                    }
                }


                else if (document.getElementById("<%=txtCodeName.ClientID%>").value == "") {
                    document.getElementById("<%=txtCodeName.ClientID%>").focus();
                    alert("Remarks Description field can not be blank");
                    return false;
                }
            }
            else {
                if (state == 'New') { if (confirm('Are you sure you want to save Hotel Remarks?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Hotel Remarks?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Hotel Remarks?') == false) return false; }
            }
        }

        
   
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
         <TABLE style="border: 2px solid gray; width: 500px;">
                <tbody>
                    <tr>
                      <TD  colspan ="2"class="td_cell" align="center" >
                      

     <asp:Label id="lblCustCatHead" runat="server" Text="Hotel Remarks Template" ForeColor="White" 
                              Width="480px" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                  <tr>
                        <td class="td_cell" style ="height :24px; width: 17px;">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
           <tr>
                                        <TD style=" HEIGHT: 24px; width: 17px;" class="td_cell">
                                            Code<SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD>
                                        <td  >
                                            <input id="txtCode" class="field_input" style="width: 120px" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                                        </td>
                                      
                                    </tr>
                                      <tr>
                                       <TD style=" HEIGHT: 24px;  vertical-align: top; width: 17px;" class="td_cell">
                                           Description <span class="td_cell" style="COLOR: red">*</span></TD>
                                        <td class="td_cell"  >
                                            <asp:TextBox ID="txtcodename" runat="server" __designer:wfdid="w20" 
                                                CssClass="field_input" TabIndex="2" 
                                                Width="360px"></asp:TextBox>
                                        </td>
                                    
                                    </tr>
                                                    <TR>
    <TD style="height: 16px; width: 17px;" class="td_cell"   >Active</TD>
    <TD style=" HEIGHT: 16px" >
        <input id="chkActive" tabindex="3" type="checkbox" checked runat="server" />
    </TD>
                                  </TR>   
                                              <tr>
                                                  <td class="td_cell" style=" HEIGHT: 16px; width: 17px;">
                                                      &nbsp;</td>
                                                  <td style=" HEIGHT: 16px">
                                                      &nbsp;</td>
                    </tr>
                    <tr>
                        <td  align="right" style="width: 17px;">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="4" 
                                Text="Save" />
                        </td>
                        <td>
                             &nbsp;&nbsp;  <asp:Button ID="btnCancel" runat="server" CssClass="btn" tabIndex="5" 
                                Text="Return To Search" Width="146px" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w9" CssClass="btn" 
                                onclick="btnhelp_Click" tabIndex="6" Text="Help" />
                        </td>
                    </tr>
                                              <tr>
                                                  <td style="width: 17px">
                                                      &nbsp;</td>
                                                  <td>
                                                      &nbsp;</td>
                    </tr>
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
