<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="BankTypes.aspx.vb" Inherits="BankTypes" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">

        function FormValidation(state) {

            if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtCode.ClientID%>").value <= 0) || (document.getElementById("<%=txtName.ClientID%>").value == "") || (document.getElementById("<%=ddlCashBank.ClientID%>").value == "[Select]")) {
                if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    alert("Code field can not be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtCode.ClientID%>").value <= 0) {
                    alert("Code must be greater than zero.");
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    return false;
                }
                else if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert("Name field can not be blank");
                    return false;
                }
                else if (document.getElementById("<%=ddlCashBank.ClientID%>").value == "[Select]") {
                    document.getElementById("<%=ddlCashBank.ClientID%>").focus();
                    alert("Please select Cash/Bank ");
                    return false;
                }


            }
            else {
                //alert(state);
                if (state == 'New') { if (confirm('Are you sure you want to save Bank Types ?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Bank Types ?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Bank Types ?') == false) return false; }
            }
        }


        function checkNumber(e) {

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

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                   <table style="border: 2px solid gray; width: 351px; height: 156px;">
                    <tr>
                        <td align="center" colspan="5">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Bank Types" CssClass="field_heading"
                                Width="486px" Height="20px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 181px">
                            Bank Type &nbsp;Code<span style="color: #ff0000"> <span class="td_cell">*</span></span>
                        </td>
                        <td style="width: 392px"  >
                            <input style="width: 108px" id="txtCode" class="field_input" tabindex="1" type="text"
                                maxlength="10" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 181px">
                            Bank Type&nbsp; Name<span style="color: #ff0000"> <span class="td_cell">*</span></span>
                        </td>
                        <td style="width: 392px"  >
                            <input   id="txtName" class="field_input" tabindex="2" type="text"
                                maxlength="25" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 181px">
                            Cash/Bank <span style="color: #ff0000">&nbsp;<span class="td_cell">*</span></span>
                        </td>
                        <td style="width: 392px"  >
                            <select style="width: 114px; height: 20px;" id="ddlCashBank" class="field_input"
                                tabindex="3" runat="server">
                                <option value="[Select]" selected>[Select]</option>
                                <option value="C">Cash</option>
                                <option value="B">Bank</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 181px">
                        </td>
                        <td style="width: 392px" >
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 181px">
                            <asp:Button ID="btnSave" TabIndex="4" runat="server" Text="Save" CssClass="field_button">
                            </asp:Button>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnhelp" runat="server" CssClass="field_button" 
                                OnClick="btnhelp_Click" TabIndex="8" Text="Help" />
                        </td>
                        <td style="width: 392px"  >
                            <asp:Button ID="btnCancel" TabIndex="5" runat="server" Text="Return To Search" 
                                CssClass="field_button" Width="148px">
                            </asp:Button>&nbsp;</td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
