<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" EnableEventValidation="false"  AutoEventWireup="false"
    CodeFile="Commission.aspx.vb" Inherits="Commission" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<style type="text/css">
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }        
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
            font-size: 10pt;
            font-weight: bold;
            border-bottom-style: double;
            border-width: medium;
            border-color:Blue;
        }
        
        *
        {
            outline: none;
        }
        
        .fmhead 
        {
        	display: block;
         text-align: center;
         
        }
        
          .ModalPopupBG
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
        
           .ModalPopupBGmeal
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {


            Term1AutoCompleteExtenderKeyUp();
            Term2AutoCompleteExtenderKeyUp();
            ResultTermAutoCompleteExtenderKeyUp();

        });

        function Term1AutoCompleteExtenderKeyUp() {
            $("#<%=grdCommFormula.ClientID%> tr input[id*='txtTerm1']").each(function () {
                $(this).change(function (event) {
                    var hiddenfieldID = $(this).attr("id").replace("txtTerm1", "txtTerm1Code");
                    $get(hiddenfieldID).value = '';
                });
            });
        }
        function Term2AutoCompleteExtenderKeyUp() {
            $("#<%=grdCommFormula.ClientID%> tr input[id*='txtTerm2']").each(function () {
                $(this).change(function (event) {
                    var hiddenfieldID = $(this).attr("id").replace("txtTerm2", "txtTerm2Code");
                    $get(hiddenfieldID).value = '';
                });
            });
        }
        function ResultTermAutoCompleteExtenderKeyUp() {
            $("#<%=grdCommFormula.ClientID%> tr input[id*='txtResultTerm']").each(function () {
                $(this).change(function (event) {
                    var hiddenfieldID = $(this).attr("id").replace("txtResultTerm", "txtResultTermCode");
                    $get(hiddenfieldID).value = '';
                });
            });
        }
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 ||  event.keyCode > 122)) {
                return false;
            }
        }

        function checkNumber(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if (charCode != 47 && (charCode > 45 && charCode < 58)) {
                //alert("Enter numerals only in this field. "+ charCode);
                return true;
            }
            return false;
        }

        function ValidationTerm() {

            if ((document.getElementById("<%=txtNewTermCode.ClientID%>").value == "")) {
                document.getElementById("<%=txtNewTermCode.ClientID%>").focus();
                alert("Term Code field can not be blank");
                return false;
            }
            if ((document.getElementById("<%=txtNewTermName.ClientID%>").value == "")) {
                document.getElementById("<%=txtNewTermName.ClientID%>").focus();
                alert("Term Name field can not be blank");
                return false;
           }
           return true;
        }
        
        function FormValidation(state) {
            if ((document.getElementById("<%=txtName.ClientID%>").value == "")) {

                if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert("Commission Formula Name field can not be blank");
                    return false;
                }
            }
            else {
                if (state == 'New') { if (confirm('Are you sure you want to save Commission?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Commission?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Commission?') == false) return false; }
            }
        }
        function MoveUpDown(state) {
                if (state == 'up') { if (confirm('Are you sure you want to move up?') == false) return false; }
                if (state == 'down') { if (confirm('Are you sure you want to move down?') == false) return false; }            
            
        }
        function Term1autocompleteselected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteExtender2", "txtTerm1Code");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }

        function Term2autocompleteselected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteExtender3", "txtTerm2Code");
            $get(hiddenfieldID).value = eventArgs.get_value();           
        }

        function ResultTermautocompleteselected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteExtender4", "txtResultTermCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }
    </script>
    <style>
    .field_heading1
    {
	    font-family: Verdana, Arial, Geneva, ms sans serif;
	    font-size: 9pt;
	    font-weight: bold;
	    background-color: #06788B;
	    color: #ffffff;
	    padding:3px;
    }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid" class="td_cell" width="100%">
                <tbody>
                    <tr>
                        <td class="td_cell" align="center" colspan="3">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Commission" Width="100%"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr><td colspan="3" align="center">
                    <table style="width:70%; padding:0px; position:relative;">
                    <tr>
                        <td class="td_cell">
                            <span style="color: black">Commission Scheme code</span> <span style="color: red"
                                class="td_cell">*</span>
                        </td>
                        <td style="color: #000000;width:90%">
                            <input onblur="chgvalue()" id="txtcode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" readonly="readonly" style="width: 220px" />
                        </td>                        
                    </tr>
                    <tr>
                        <td style="height:24px;width:28%" class="td_cell">
                            Commission Scheme name<span style="color: red" class="td_cell">*</span>
                        </td>
                        <td style="width:72%">
                            <input id="txtname" class="field_input" tabindex="2" type="text" maxlength="1000"
                                style="width:100%" runat="server" />
                        </td>                        
                    </tr>
                    <tr>
                        <td class="td_cell"">
                            Remarks</td>
                        <td>
                         <asp:TextBox ID="txtremarks" runat="server" CssClass="field_input" TabIndex="3"
                           Rows="2" Style="margin: 0px; height: 48px; width: 400px;" TextMode="MultiLine"></asp:TextBox>

                        </td>                        
                    </tr>
                        <tr>
                            <td class="td_cell">
                                <asp:Label ID="label1" runat="server" CssClass="td_ce" Text="Active" 
                                    ViewStateMode="enabled" Width="44px"></asp:Label>
                            </td>
                            <td>
                                <input id="chkActive" tabindex="4" type="checkbox" checked="checked" runat="server" />
                            </td>
                        </tr>
                    </table>
                    </td></tr>
                    <tr>
                       <td valign="top" style="width: 30%">
                        <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%" class="td_cell" valign="top" >
                                            <strong>Commission Terms</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView ID="grdCommTerms" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px"
                                                GridLines="Vertical" TabIndex="12" Width="100%" >
                                                <FooterStyle CssClass="grdfooter" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False" HeaderText="Line No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFLineNo" runat="server" Text='<%# Bind("RankOrder") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Term Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTermCode" runat="server" Text='<%# Bind("TermCode") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" />
                                                        <ItemStyle Wrap="true" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Term Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTermName" runat="server" Text='<%# Bind("TermName") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="false" Width="70%" HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                        <ItemStyle Wrap="true" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False" HeaderText="System Value">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSystemValue" runat="server" Text='<%# Bind("SystemValue") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                       <ItemTemplate>
                                                           <asp:LinkButton ID="lbtnEdit" runat="server" OnClick="CommissionTermEdit">Edit</asp:LinkButton>
                                                       </ItemTemplate>
                                                    <ItemStyle ForeColor="Blue" HorizontalAlign="Center" />
                                                    <HeaderStyle Wrap="false" Width="10%" HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                    </asp:TemplateField>                                                    
                                                    </Columns>
                                               <RowStyle CssClass="grdRowstyle" />
                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView> 
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td style="height: 22px">
                                            <asp:Button ID="btnAddTerm" TabIndex="26" OnClick="btnAddTerm_Click" runat="server"
                                                Text="Add Another Term" CssClass="btn"></asp:Button>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <div id="ShowNewTerms" runat="server" style="overflow:auto; height: 230px; width: 450px;
                                                                        border: 3px solid green; background-color: White; display: none">
                                                                        <table style="float: left; width:100%; height:90%;">  
                                                                        <tr >
                                                                            <td class="td_cell" align="center" colspan="2" style="padding-left:3px; padding-right: 10px; padding-bottom:5px">
                                                                                <asp:Label ID="lblTermTitle" runat="server" Text="New Commission Term" Width="100%"
                                                                                    CssClass="field_heading1" ></asp:Label>
                                                                            </td>
                                                                        </tr>                                                                          
                                                                        <tr>
                                                                                <td class="td_cell" style="padding-left:3px; padding-bottom:5px">
                                                                                    <span style="color: black">Commission Term code</span> <span style="color: red"
                                                                                        class="td_cell">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <input id="txtNewTermCode" class="field_input" tabindex="1" type="text"
                                                                                        maxlength="20" runat="server" style="width: 220px" />
                                                                                </td>                        
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="td_cell" style="padding-left:3px; padding-bottom:5px">
                                                                                    Commission Term name<span style="color: red" class="td_cell">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <input id="txtNewTermName" class="field_input" tabindex="2" type="text" maxlength="100"
                                                                                        style="width: 220px" runat="server" />
                                                                                </td>                        
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="td_cell" style="padding-left:3px; padding-bottom:5px">
                                                                                    Calculation Type<span style="color: red" class="td_cell">*</span>
                                                                                </td>
                                                                                <td>                                                                                  
                                                                                   <asp:DropDownList ID="ddlCalculateType" CssClass="field_input" runat="server" Width="220px" TabIndex="3">
                                                                                        <asp:ListItem Value="[Select]">[Select]</asp:ListItem>
                                                                                        <asp:ListItem Value="P">Percentage</asp:ListItem>
                                                                                        <asp:ListItem Value="V">Value</asp:ListItem>                                                                                    
                                                                                    </asp:DropDownList> 
                                                                                </td>                        
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="td_cell" style="padding-left:3px; padding-bottom:5px">
                                                                                    <asp:Label ID="label3" runat="server" CssClass="td_cell" Text="Active" ViewStateMode="enabled"
                                                                                        Width="44px"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <input id="chkTermActive" tabindex="4" type="checkbox" checked="checked" runat="server" />
                                                                                </td>                        
                                                                            </tr>
                                                                              <tr>
                                                                               <td class="td_cell" style="padding-left:3px; padding-bottom:5px">
                                                                                    <span style="color: black">Calculate By Pax</span> </td>
                                                                                <td>
                                                                                    <input id="chkcalcpax" tabindex="5" type="checkbox"  runat="server" />
                                                                                </td>                        
                                                                            </tr>
                                                                            <tr align="center">
                                                                                <td colspan="2">
                                                                                    <asp:Button ID="btnSaveTerm" runat="server" CssClass="field_button" Text="Save" Width="80px" OnClientClick="return ValidationTerm()"  />&nbsp;&nbsp;
                                                                                    <asp:Button ID="btnClearTerm" runat="server" CssClass="field_button" Text="Close" Width="80px" />
                                                                                </td>
                                                                             </tr>
                                                                        </table>
                                                                        <asp:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                                                            CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                                                            PopupControlID="ShowNewTerms" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                                            BackgroundCssClass="ModalPopupBG">
                                                                        </asp:ModalPopupExtender>
                                                                        <input id="btnInvisibleEBGuest" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                                                        <input id="btnOkayEB" type="button" value="OK" style="visibility: hidden" />
                                                                        <input id="btnCancelEB" type="button" value="Cancel" style="visibility: hidden" />
                                                                    </div>
 
                                    </td>
                                    </tr>
                                 </tbody>
                         </table>
                       </td>
                        <td valign="top" colspan="2" style="width: 70%">
                            <table style="width: 100%" >
                                <tbody>
                                    <tr>
                                        <td style="width: 100%" class="td_cell" valign="top">
                                            <strong>Commission Formula</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100% " valign="top">
                                            <asp:GridView ID="grdCommFormula" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px"
                                                GridLines="Vertical" TabIndex="12" Width="100%" >
                                                <FooterStyle CssClass="grdfooter" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Serial No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFLineNo" runat="server" Text='<%# Bind("fLineNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Term 1">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTerm1" runat="server" CssClass="fiel_input"  Width="100%" >
                                                            </asp:TextBox>
                                                            <asp:TextBox ID="txtTerm1Code" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="Term1autocompleteselected" ServiceMethod="GetTermlist" TargetControlID="txtTerm1">
                                                                                                </asp:AutoCompleteExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle"  Width="25%" />
                                                        <ItemStyle Wrap="false" Width="25%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Operator 1">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlOperator1" CssClass="fiel_input" runat="server" Width="80px">
                                                            <asp:ListItem Value="[Select]">[Select]</asp:ListItem>
                                                            <asp:ListItem Value="*">*</asp:ListItem>
                                                            <asp:ListItem Value="/">/</asp:ListItem>
                                                            <asp:ListItem Value="+">+</asp:ListItem>
                                                            <asp:ListItem Value="-">-</asp:ListItem>
                                                            
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <HeaderStyle VerticalAlign="Middle" />
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Term 2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTerm2" runat="server" CssClass="fiel_input" Width="100%">
                                                            </asp:TextBox>
                                                            <asp:TextBox ID="txtTerm2Code" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="10"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="Term2autocompleteselected" ServiceMethod="GetTermlist" TargetControlID="txtTerm2">
                                                                                                </asp:AutoCompleteExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="False" Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Equal To">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEqualTo" runat="server" Text="=" ></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="true"  VerticalAlign="Middle" HorizontalAlign="Center"/>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Result Term">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtResultTerm" runat="server" CssClass="fiel_input" Width="100%">
                                                            </asp:TextBox>
                                                            <asp:TextBox ID="txtResultTermCode" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="10"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="ResultTermautocompleteselected" ServiceMethod="GetTermlist" TargetControlID="txtResultTerm">
                                                                                                </asp:AutoCompleteExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="False" Width="25%" VerticalAlign="Middle" HorizontalAlign="Center"/>
                                                        <ItemStyle Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Move">
                                                        <ItemTemplate>                                             
                                                            <asp:ImageButton ID="ImgBtnUp" OnClientClick="return MoveUpDown('up')" CommandArgument = "up" runat="server" Height="12px" Width ="12px" ImageUrl="~/Images/ArrowUp.png" OnClick="ChangePreference" />
                                                            <asp:ImageButton ID="ImgBtnDown" OnClientClick="return MoveUpDown('down')" CommandArgument = "down" runat="server" Height="12px" Width ="12px" ImageUrl="~/Images/ArrowDown.png" OnClick="ChangePreference" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" />
                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td style="height: 22px">
                                            <asp:Button ID="btnAddRow" TabIndex="25" OnClick="btnAddRow_Click" runat="server"
                                                Text="Add Row" CssClass="btn"></asp:Button>&nbsp;
                                            <asp:Button ID="btnDeleteRow" TabIndex="26" OnClick="btnDeleteRow_Click" runat="server"
                                                Text="Delete Row" CssClass="btn"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr align="center">
                        <td class="td_cell" style="height: 23px;" colspan="3">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="6" Text="Save" />&nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="7" Text="Return to Search" />&nbsp;
                            <asp:Button ID="btnHelp" runat="server" CssClass="btn" TabIndex="8" Text="Help" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
