<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="VisaTypes.aspx.vb" Inherits="PriceListModule_VisaTypes" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
      <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
 <script language ="javascript" type="text/javascript" >

     function Validate(state) {

         if (state == 'New' || state == 'Edit') {
            
//             if (document.getElementById("<%=txtCode.ClientID%>").value == '') {
//                 document.getElementById("<%=txtCode.ClientID%>").focus();
//                 alert('Please Enter Code');
//                 return false;
//             } else
             
              if (document.getElementById("<%=txtName.ClientID%>").value == '') {
                 document.getElementById("<%=txtName.ClientID%>").focus();
                 alert('Please Enter Name');
                 return false;
             } 
             else {
                 if (state == 'New') { if (confirm('Are you sure you want to save this Type?') == false) return false; }
                 if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                 if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
             }

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

     function GetOtherGrpValueFrom() {

         var ddl = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
         ddl.selectedIndex = -1;
         // Iterate through all dropdown items.
         for (i = 0; i < ddl.options.length; i++) {
             if (ddl.options[i].text ==
                document.getElementById("<%=ddlOtherGrpCode.ClientID%>").value) {
                 // Item was found, set the selected index.
                 ddl.selectedIndex = i;
                
                 return true;
             }
         }
         
     }
     function GetOtherGrpValueCode() {
         var ddl = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
         ddl.selectedIndex = -1;
         // Iterate through all dropdown items.
         for (i = 0; i < ddl.options.length; i++) {
             if (ddl.options[i].text ==
			document.getElementById("<%=ddlOtherGrpName.ClientID%>").value) {
                 // Item was found, set the selected index.
                 ddl.selectedIndex = i;
                
                 return true;
             }
         }
         

     }


     function suppnameautocompleteselected(source, eventArgs) {
         if (eventArgs != null) {
             document.getElementById('<%=txtsuppcode.ClientID%>').value = eventArgs.get_value();
         }
         else {
             document.getElementById('<%=txtsuppcode.ClientID%>').value = '';
         }
     }
     
     

     function TimeOutHandler(result) {
         alert("Timeout :" + result);
     }

     function ErrorHandler(result) {
         var msg = result.get_exceptionType() + "\r\n";
         msg += result.get_message() + "\r\n";
         msg += result.get_stackTrace();
         alert(msg);
     }
     

			
</script> 



    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 714px; border-bottom: gray 2px solid; text-align: left">
                <tbody>
                    <tr>
                        <td style="height: 3px; text-align: center" class="td_cell" >
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Visa Types" ForeColor="White"
                                __designer:wfdid="w17" Width="100%" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Code<span style="color: #ff0000" class="td_cell">*</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input id="txtCode" class="field_input" style="width: 196px" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>
                    
                    <tr><td></td></tr>

                        <tr>
                            <td class="td_cell">
                                Name<span style="color: #ff0000" class="td_cell">*</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <input id="txtName" class="field_input" style="width: 300px" tabindex="2" type="text"
                                    maxlength="150" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" id="trTax" visible="false">
                            <td class="td_cell">
                               <label id="lblNonTax" runat="server" >Non Taxable Amount<span style="color: #ff0000" class="td_cell">*</span></label>&nbsp;&nbsp;
                                <label id="Label2" runat="server" >Adult</label>&nbsp;&nbsp;
                                <input id="txtNonTax" class="field_input" style="width: 100px" tabindex="3" type="text"
                                    maxlength="150" runat="server" />
                                     &nbsp;&nbsp;<label id="Label1" runat="server" >Child</label>
                                      <input id="txtNonTax_Child" class="field_input" style="width: 95px" tabindex="3" type="text"
                                    maxlength="150" runat="server" />
                            </td>
                        </tr>
                            <tr>
                                <td style="display: none; width: 88%;">
                                    Group Code &nbsp;<span style="color: red" class="td_cell">*&nbsp;&nbsp;</span>
                                </td>
                                <td style="display: none">
                                    <span style="color: #ff0000">
                                        <select style="width: 232px" id="ddlOtherGrpCode" class="field_input" tabindex="4"
                                            onchange="GetOtherGrpValueFrom()" runat="server">
                                            <option selected></option>
                                        </select></span>
                                </td>
                                <td style="display: none">
                                    Group&nbsp;Name&nbsp;&nbsp;
                                </td>
                                <td style="display: none">
                                    <select style="width: 315px" id="ddlOtherGrpName" class="field_input" tabindex="5"
                                        onchange="GetOtherGrpValueCode()" runat="server">
                                        <option selected></option>
                                    </select>
                                </td>
                            </tr>
                            <%--<TR>
            <TD  class="td_cell" style="HEIGHT: 22px">    Transfer type</TD>
            <td align="left" class="td_cell" style="WIDTH: 2px; HEIGHT: 22px">
                    <select ID="ddlType" runat="server" class="fiel_input" onchange="loadpickdropoff()"  style="WIDTH: 232px">
                      <option selected ="selected"  value="[Select]">[Select]</option>
                      <option value="0">Arrival Borders</option>
                      <option value="1">Departure Borders</option>
                      <option value="2">Internal Transfer/Excursion</option>
                      <option value="3">Arrival/Departure Transfer Borders</option>
                    </select>    </td>
            <td align="left" class="td_cell" style="WIDTH: 3px; HEIGHT: 22px">
            &nbsp;</td>
            <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
            &nbsp;</td>
    </TR>--%>
                            <%--<tr>
        <td class="td_cell" style="HEIGHT: 22px">
            Pick Up Point</td>
        <td align="left" class="td_cell" style="WIDTH: 2px; HEIGHT: 22px">
            <span style="COLOR: #ff0000">
            <select ID="ddlPName" runat="server" class="field_input" name="D2" onchange="setvalue();"
                style="WIDTH: 232px" tabindex="3">
                <option selected=""></option>
            </select></span></td>
        <td align="left" class="td_cell" style="WIDTH: 3px; HEIGHT: 22px">
            &nbsp;</td>
        <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
            <asp:HiddenField ID="hdnP" runat="server" />
        </td>
    </tr>--%>
                            <%--<tr>
        <td class="td_cell" style="HEIGHT: 22px">
            Drop Off Point</td>
        <td align="left" class="td_cell" style="WIDTH: 2px; HEIGHT: 22px">
            <span style="COLOR: #ff0000">
            <select ID="ddlDName" runat="server" class="field_input" name="D4" onchange="setvalue();"
                style="WIDTH: 232px" tabindex="3">
                <option selected=""></option>
            </select></span></td>
        <td align="left" class="td_cell" style="WIDTH: 3px; HEIGHT: 22px">
            &nbsp;</td>
        <td align="left" class="td_cell" style="WIDTH: 4px; HEIGHT: 22px">
            <asp:HiddenField ID="hdnD" runat="server" />
        </td>
    </tr>--%>
                            <tr>
                                <td style="display: none; width: 88%;">
                                    Order<span style="color: #ff0000">*</span>
                                    <input style="width: 226px; text-align: right" id="txtOrder" tabindex="6" type="text"
                                        runat="server" class="field_input" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px; height: 2px" class="td_cell">
                                    Remarks&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtRemark" runat="server" __designer:wfdid="w20" CssClass="field_input"
                                        Height="36px" TabIndex="7" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                
                                    <td style="width: 130px; height: 2px" class="td_cell">
                                        Prefered Supplier<span style="color: red" class="td_cell">*</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                
                                    <asp:TextBox ID="txtsuppname" runat="server" CssClass="field_input" MaxLength="500"
                                        TabIndex="3" Width="300px"></asp:TextBox>
                                    <asp:TextBox ID="txtsuppcode" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="txtsuppname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                        EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="0"
                                        OnClientItemSelected="suppnameautocompleteselected" ServiceMethod="Getsupplierlist"
                                        TargetControlID="txtsuppname">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 1px">
                                    <input id="ChkInactive" tabindex="9" type="checkbox" checked runat="server" />
                                    Active
                                </td>
                            </tr>
                            <tr>
                                <td style="display: none; width: 88%;">
                                    <table border="0" style="width: 538px">
                                        <tbody>
                                            <tr>
                                                <td colspan="2" style="width: 293px; height: 1px">
                                                    <%-- <INPUT id="ChkPrnRemark" tabIndex=10 type=checkbox CHECKED runat="server" />
                            &nbsp;Print Remark</td>--%>
                                                    <input id="ChkAutoCancel" tabindex="12" type="checkbox" checked runat="server" />
                                                    Auto Cancellation Required
                                                </td>
                                                <td style="width: 58px; height: 1px">
                                                    Min Pax <span style="color: #ff0000">*</span>
                                                </td>
                                                <td style="width: 1px; height: 1px">
                                                    <input style="width: 81px; text-align: right" id="txtMinPax" tabindex="7" type="text"
                                                        runat="server" class="field_input" />
                                                </td>
                                                <td colspan="2" style="width: 193px; height: 1px">
                                                    <input id="ChkPakReq" tabindex="8" type="checkbox" checked runat="server" />
                                                    Pax Check Required
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 4px">
                                                    <%-- <INPUT id="ChkPrnConfirm" tabIndex=11 type=checkbox CHECKED runat="server" />
                            Print in Confirmation</td>--%>
                                                    <td colspan="2" style="width: 193px; height: 4px">
                                                        <%--<INPUT id="ChkAutoCancel" tabIndex=12 type=checkbox CHECKED runat="server" />
                            &nbsp;Auto Cancellation Required</td>--%>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 3px; width: 88%;">
                                    <asp:Button ID="btnSave" runat="server" CssClass="field_button" OnClick="btnSave_Click"
                                        TabIndex="13" Text="Save" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="field_button" OnClick="btnCancel_Click"
                                        TabIndex="14" Text="Return To Search" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnhelp" runat="server" __designer:wfdid="w8"
                                        CssClass="field_button" OnClick="btnhelp_Click" TabIndex="15" Text="Help" />
                                </td>
                            </tr>
                       
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>                    
                </tbody>
            </table>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
             <Services>
                 <asp:ServiceReference Path="~/clsServices.asmx" />
             </Services>
         </asp:ScriptManagerProxy>
</asp:Content>

