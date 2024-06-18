<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="OtherServiceGroups.aspx.vb" Inherits="OtherServiceGroups" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }



        function GetMainGroupCodeFrom() {

            var ddl = document.getElementById("<%=ddlMainGroupName.ClientID%>");
            ddl.selectedIndex = -1;
            // Iterate through all dropdown items.
            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].text ==
			document.getElementById("<%=ddlMainGroupCode.ClientID%>").value) {
                    // Item was found, set the selected index.
                    ddl.selectedIndex = i;
                    return true;
                }
            }
        }
        function GetMainGroupValueFrom() {
            var ddl = document.getElementById("<%=ddlMainGroupCode.ClientID%>");
            ddl.selectedIndex = -1;
            // Iterate through all dropdown items.
            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].text ==
			document.getElementById("<%=ddlMainGroupName.ClientID%>").value) {
                    // Item was found, set the selected index.
                    ddl.selectedIndex = i;
                    return true;
                }
            }
        }

    
           
           

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 228px; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td class="td_cell" style="height:24px" colspan="4">
                            <asp:Label ID="lblHeading"   align="center" runat="server" Text="New Excursion Group" CssClass="field_heading"
                                Width="610px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #ff0000;height:24px;">
                        <td style="width: 135px; height: 4px" class="td_cell">
                            <span style="color: #000000">Group&nbsp;&nbsp;Code</span><span style="color: #ff0000"
                                class="td_cell">&nbsp;*</span>
                        </td>
                        <td style="width: 1%; color: #000000">
                            <input style="width: 196px" id="txtCode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 135px; height: 24px" class="td_cell">
                            Group Name <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td style="width: 1%; color: #000000">
                            <input style="width: 476px" id="txtName" class="field_input" tabindex="2" type="text"
                                maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 135px; height: 7px;Vertical-align:top " class="td_cell">
                            Terms
                        </td>
                        <td style="width: 1%; height: 7px">
                            <asp:TextBox ID="txtterms" runat="server" CssClass="field_input" Enabled="true" Height="60px"
                                TabIndex="8" TextMode="MultiLine" Width="477px"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<TR><TD style="HEIGHT: 12px" class="td_cell" colSpan=3>
<asp:Panel id="Panel1" runat="server" __designer:wfdid="w18" Width="357px" GroupingText="Print Specification" Height="50px">
    <TABLE><TBODY>
    <TR><TD style="WIDTH: 86px; HEIGHT: 22px"><INPUT id="ChkprnGruop" tabIndex=5 type=checkbox CHECKED runat="server" />&nbsp;Print Group</TD>
    <TD style="WIDTH: 100px; HEIGHT: 22px">&nbsp;<INPUT id="ChkPrnType" tabIndex=6 type=checkbox CHECKED runat="server" />&nbsp;Print Type</TD>
    <TD style="WIDTH: 100px; HEIGHT: 22px"><INPUT id="ChkPrnCategory" tabIndex=7 type=checkbox CHECKED runat="server" />&nbsp;Print&nbsp;Category</TD></TR>
   </TBODY></TABLE></asp:Panel></TD>
   
   <TD style="WIDTH: 9px; HEIGHT: 12px" class="td_cell" colSpan=1></TD></TR>--%>
                    <tr>
                        <td style="width: 135px" class="td_cell">
                            Active
                        </td>
                        <td style="width: 1%">
                            <input id="chkActive" tabindex="9" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 135px; height: 7px">
                            <asp:Label ID="lblmaingrpcode" runat="server" Text="Excursion Main Group Code"></asp:Label>
                            <%--<SPAN style="COLOR: red" class="td_cell">*</SPAN>--%>
                        </td>
                        <td style="width: 1%; height: 7px">
                            <select ID="ddlMainGroupCode" runat="server" class="field_input" name="D2" 
                                onchange="GetMainGroupCodeFrom()" style="width: 202px" tabindex="3">
                                <option selected=""></option>
                            </select>&nbsp;
                            <asp:Label ID="Label1" runat="server" Text="Excursion Main Group Name"></asp:Label>
                            <select ID="ddlMainGroupName" runat="server" class="field_input" name="D1" 
                                onchange="GetMainGroupValueFrom()" style="width: 216px" tabindex="4">
                                <option selected=""></option>
                            </select>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td style="width: 135px; height: 22px" class="td_cell">
                        </td>
                        <td style="width: 1%; height: 22px">
                        </td>
                        <tr>
                            <td style="width: 135px; height: 12px">
                                &nbsp;</td>
                            <td style="width: 1%; height: 12px">
                                &nbsp;
                                <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="10" 
                                    Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                    OnClick="btnCancel_Click" TabIndex="11" Text="Return To Search" />
                                <asp:Button ID="btnhelp" runat="server" CssClass="field_button" 
                                    OnClick="btnhelp_Click" TabIndex="12" Text="Help" />
                            </td>
                            <td style="width: 267px; height: 12px">
                                &nbsp;</td>
                            <td style="width: 9px; height: 12px">
                            </td>
                        </tr>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
