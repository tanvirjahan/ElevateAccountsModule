<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="OtherBankMaster.aspx.vb" Inherits="OtherBankMaster" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">

        function FormValidation(state) {

            if ((document.getElementById("<%=txtName.ClientID%>").value == "")) {
                if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert("Name field can not be blank");
                    return false;
                }


            }
            else {

                if (state == 'New') { if (confirm('Are you sure you want to save customer Bank Master?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update customer Bank Master?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete customer Bank Master?') == false) return false; }
            }
        }

        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 456px; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td align="center" colspan="5">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Cutomer Bank Master" Width="471px"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 145px" class="td_cell">
               
                            <span style="FONT-SIZE: 8pt;color: #000000" class="td_cell">Customer&nbsp;Bank Code <span style="color: red"
                                class="td_cell">*</span></span>
                        </td>
                        <td style="color: #000000">
                            <input id="txtCode" class="field_input" tabindex="0" onkeypress="return checkNumber()"
                                type="text" maxlength="10" size="25" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style=" FONT-SIZE: 8pt;width: 145px; height: 24px" class="td_cell">
                            Customer&nbsp;Bank Name <span style="color: red" class="td_cell">*</span>
                        </td>
                        <td>
                            <input id="txtName" class="field_input" tabindex="0" type="text" maxlength="25" size="25"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="FONT-SIZE: 8pt;width: 145px; height: 24px" class="td_cell">
                            Active
                        </td>
                        <td style="height: 24px">
                            <input id="chkActive" tabindex="0" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 145px">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="field_button"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Return To Search" CssClass="field_button">
                            </asp:Button>&nbsp;<asp:Button ID="btnhelp" OnClick="btnhelp_Click" runat="server"
                                Text="Help" CssClass="field_button" __designer:wfdid="w10"></asp:Button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
