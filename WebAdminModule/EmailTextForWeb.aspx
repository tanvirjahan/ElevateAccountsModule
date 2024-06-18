<%@ Page Language="VB" MasterPageFile="~/WebAdminMaster.master" AutoEventWireup="false"
    ValidateRequest="false" Strict="true" CodeFile="EmailTextForWeb.aspx.vb" Inherits="EmailTextForWeb" %>

<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">
        function checkNumber(e) {
            if (event.keyCode < 45 || event.keyCode > 57) {
                return false;
            }
        }

        function FormValidation(state) {
            if ((document.getElementById("<%=txtSubject.ClientID%>").value == "") || (document.getElementById("<%=txtEmailText.ClientID%>").value == "") || (document.getElementById("<%=txtFooterText.ClientID%>").value == "")) {
                if (document.getElementById("<%=txtSubject.ClientID%>").value == "") {
                    document.getElementById("<%=txtSubject.ClientID%>").focus();
                    alert("Subject field can not be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtEmailText.ClientID%>").value == "") {
                    document.getElementById("<%=txtEmailText.ClientID%>").focus();
                    alert("Email Text field can not be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtFooterText.ClientID%>").value == "") {
                    document.getElementById("<%=txtFooterText.ClientID%>").focus();
                    alert("Footer Text field can not be blank");
                    return false;
                }
            }
            else {
                if (state == 'Edit') { if (confirm('Are you sure you want to update CustomerEmailText?') == false) return false; }
            }
        }


    

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border: 2px solid gray; width: 823px; height: 452px;">
                <tbody>
                    <tr>
                        <td style="height: 18px" class="td_cell" align="center" colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Email  Text" ForeColor="White"
                                CssClass="field_heading" Width="720px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #ff0000">
                        <td style="width: 105px">
                            <span class="td_cell" style="color: red"><span style="color: black">Email Text For</span>
                            </span>
                        </td>
                        <td style="width: 384px">
                            <asp:DropDownList ID="ddlemailtextfor" runat="server" AutoPostBack="true" 
                                TabIndex="1">
                                <asp:ListItem Value="0">Login Credentials</asp:ListItem>
                                <asp:ListItem Value="1">Contract </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="color: #ff0000">
                        <td style="width: 105px; height: 27px" class="td_cell">
                            <span style="color: #ff0000" class="td_cell"><span style="color: black">Subject</span>*</span>
                        </td>
                        <td style="width: 384px; color: #000000; height: 27px">
                            <asp:TextBox ID="txtSubject" TabIndex="2" runat="server" Height="16px" CssClass="field_input"
                                Width="626px" MaxLength="500"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 14px" class="td_cell">
                            <span style="color: red" class="td_cell"><span style="color: black">Email Text</span>*</span>
                        </td>
                        <td style="width: 384px; height: 14px">

                                        <HTMLEditor:Editor ID="txtEmailText" runat="server" Height="200px" Width="100%" 
                                            AutoFocus="true" TabIndex="3" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 14px" class="td_cell">
                            Footer Text
                        </td>
                        <td style="width: 384px; height: 14px">
                         <%--   <asp:TextBox ID="txtFooterText" TabIndex="3" runat="server" Height="100px" CssClass="field_input"
                                Width="626px" MaxLength="1250" TextMode="MultiLine"></asp:TextBox>--%>
                                       
                
                    <HTMLEditor:Editor ID="txtFooterText" runat="server" Height="200px" Width="100%" 
                                AutoFocus="true" TabIndex="4" />
           
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 20px;" class="td_cell">
                            From Email ID
                        </td>
                        <td style="width: 384px; height: 20px;">
                            <asp:TextBox ID="txtFromEmailId" TabIndex="5" runat="server" Height="16px" CssClass="field_input"
                                Width="626px" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 22px">
                            &nbsp;</td>
                        <td style="width: 384px; height: 22px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 22px">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="7" 
                                Text="Save" />
                        </td>
                        <td style="width: 384px; height: 22px">
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" 
                                OnClick="btnCancel_Click" TabIndex="8" Text="Exit" />
                            &nbsp;<asp:Button ID="btnHelp" runat="server" CssClass="btn" OnClick="btnHelp_Click" 
                                TabIndex="9" Text="Help" />
                            &nbsp;
                            <asp:Button ID="btnClear" runat="server" CssClass="btn" 
                                OnClick="btnClear_Click" TabIndex="9" Text="Clear" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <%--onclick="btnSave_Click"--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
