<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="NarrationTemplate.aspx.vb" Inherits="NarrationTemplate" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">

        function FormValidation(state) {

            if ((document.getElementById("<%=txtName.ClientID%>").value == "")) {
                if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert("Narration field can not be blank");
                    return false;
                }


            }
            else {
                //alert(state);
                if (state == 'New') { if (confirm('Are you sure you want to save Narration Template ?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Narration Template ?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Narration Template ?') == false) return false; }
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
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 456px; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td align="center" colspan="5">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Narration Template" CssClass="field_heading"
                                Width="622px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 78px" class="td_cell">
                            <span style="color: #000000" class="td_cell">Code <span style="color: red">*</span></span>
                        </td>
                        <td style="color: #000000">
                            <input style="width: 196px" id="txtCode" class="field_input" tabindex="0" onkeypress="return checkNumber()"
                                type="text"  runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 78px;vertical-align: top;" class="td_cell">
                            <span style="color: black ;" class="td_cell">Narration <span style="color: red">*</span></span>
                        </td>
                        <td>
                                     <asp:TextBox ID="txtName" runat="server" CssClass="field_input" Rows="2"  
                                      TabIndex="8"  
                                                                                    
                                      TextMode="MultiLine" Height="68px" Width="530px"      ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 78px" class="td_cell">
                            Active
                        </td>
                        <td>
                            <input id="chkActive" tabindex="0" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 78px; height: 32px;">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="field_button">
                            </asp:Button>
                        </td>
                        <td style="height: 32px">
                            <asp:Button ID="btnCancel" runat="server" Text="Return To Search" CssClass="field_button">
                            </asp:Button>&nbsp;<asp:Button ID="btnhelp" OnClick="btnhelp_Click" runat="server"
                                Text="Help" CssClass="field_button" __designer:wfdid="w17"></asp:Button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
