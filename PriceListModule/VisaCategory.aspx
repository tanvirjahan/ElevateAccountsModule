<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="VisaCategory.aspx.vb" Inherits="Visa_Selling_Types" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <script language="javascript " type="text/javascript">
        function checkNumber() {
            if (event.keyCode < 45 || event.keyCode > 57) {
                return false;
            }
        }

        function FormValidation(state) {
            if ((document.getElementById("<%=TxtOtherServiceCode.ClientID%>").value == "") || (document.getElementById("<%=txtOtherServiceSelling.ClientID%>").value == "")) {
                if (document.getElementById("<%=TxtOtherServiceCode.ClientID%>").value == "") {
                    document.getElementById("<%=TxtOtherServiceCode.ClientID%>").focus();
                    alert("Code field can not be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtOtherServiceSelling.ClientID%>").value == "") {
                    document.getElementById("<%=txtOtherServiceSelling.ClientID%>").focus();
                    alert("Name field can not be blank");
                    return false;
                }                
            }
            else {
                if (state == 'New') { if (confirm('Are you sure you want to Save Visa Category Types?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to Update Visa Category Types?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to Delete Visa Category Types?') == false) return false; }
            }
        }
        
   
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 656px; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td class="td_cell" align="center" colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Visa  Selling Types" CssClass="field_heading"
                                Width="645px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #ff0000">
                        <td style="width: 209px" class="td_cell">
                            <span style="color: #000000">Code </span><span style="color: #ff0000" class="td_cell">
                                *</span>
                        </td>
                        <td style="color: #000000">
                            <input style="width: 196px" id="TxtOtherServiceCode" class="txtbox" tabindex="1"
                                type="text" maxlength="150" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 209px; height: 24px" class="td_cell">
                            Name <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td style="color: #000000">
                            <input style="width: 196px" id="txtOtherServiceSelling" class="txtbox" tabindex="2"
                                type="text" maxlength="150" runat="server" />
                        </td>
                    </tr>           
                    <tr>
                        <td style="width: 209px; height: 24px" class="td_cell">
                            Price <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td style="color: #000000">
                            <input style="width: 196px" id="txtPrice" class="txtbox" tabindex="2"
                                type="text" maxlength="150" runat="server" onkeypress="return checkNumber();" />
                        </td>
                    </tr>         
                    <tr>
                        <td style="width: 209px; height: 24px" class="td_cell">
                            Active
                        </td>
                        <td style="height: 24px">
                            <input id="chkActive" tabindex="5" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 209px">
                            <asp:Button ID="btnSave" TabIndex="6" runat="server" Text="Save" CssClass="btn" Width="46px">
                            </asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="btnCancel" TabIndex="7" OnClick="btnCancel_Click" runat="server"
                                Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;&nbsp;
                            <asp:Button ID="btnhelp" TabIndex="8" OnClick="btnhelp_Click" runat="server" Text="Help"
                                CssClass="btn" __designer:wfdid="w20"></asp:Button>
                            <asp:Label ID="lblwebserviceerror" runat="server" Style="display: none" Text="Webserviceerror"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
