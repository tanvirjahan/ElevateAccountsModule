<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" EnableEventValidation="false"  AutoEventWireup="false"
    CodeFile="MarkupFormula1.aspx.vb" Inherits="MarkupFormula1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<style type="text/css">
     td.MarkupValue
        {
            background-color:#FBEEE6;

        }
         td.MarkupACI
        {
            background-color: #FCF3CF;

        }
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
            CurrencyNameAutoCompleteExtenderKeyUp();

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

            
        }
        
        function FormValidation(state) {
            if ((document.getElementById("<%=txtName.ClientID%>").value == "")) {

                if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert("Markup Formula Name field can not be blank");
                    return false;
                }
            }
            else if ((document.getElementById("<%=txtDescription.ClientID%>").value == "")) {

                if (document.getElementById("<%=txtDescription.ClientID%>").value == "") {
                    document.getElementById("<%=txtDescription.ClientID%>").focus();
                    alert("Description field can not be blank");
                    return false;
                }
            }
            else if ((document.getElementById("<%=TxtCurrencyName.ClientID%>").value == "")) {

                if (document.getElementById("<%=TxtCurrencyName.ClientID%>").value == "") {
                    document.getElementById("<%=TxtCurrencyName.ClientID%>").focus();
                    alert("Currency Name field can not be blank");
                    return false;
                }
            }
            else {
                if (state == 'New') { if (confirm('Are you sure you want to save Markup Formula?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Markup Formula?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Markup Formula?') == false) return false; }
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

        function CurrencyNameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=TextCurrencyCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=TextCurrencyCode.ClientID%>').value = '';
            }

        }
        function CurrencyNameAutoCompleteExtenderKeyUp() {
            $("#<%=TxtCurrencyName.ClientID %>").bind("change", function () {
                document.getElementById('<%=TextCurrencyCode.ClientID%>').value = '';
            });
        }
        function validateDigitOnly(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
          
            var regex = /[0-9]/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }
        function validateDecimalOnly(evt, txt) {

 
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            if (key == 13) {
               // alert('test');
//                var btnAddRow = document.getElementById("<%=btnAddRow.ClientID%>");
//                btnAddRow.click();
            }
            else {
                key = String.fromCharCode(key);

                var regex = /[0-9]/;
                if (!regex.test(key)) {
                    theEvent.returnValue = false;
                    if (theEvent.preventDefault) theEvent.preventDefault();
                }

                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode == 46) {
                    var inputValue = txt.value
                    if (inputValue.indexOf('.') < 1) {
                        txt.value = txt.value + '.';
                        return true;
                    }
                    else {
                        return false;
                    }
                }

            }

        }

        function validateOperator(evt, txt) {


            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
      //    alert(key);
            if (key == 37 || key == 42 || key == 43 || key == 45 || key == 47) {
                return true;
            }
            else {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }

        }


    </script>
    <script type="text/javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
            LowerBound = 0;
            SelectedRowIndex = -1;
        }

        function SelectRow(CurrentRow, RowIndex) {
            var gridView = document.getElementById("<%=grdCommFormula.ClientID %>");// *********** Change gridview name
            UpperBound = gridView.getElementsByTagName("tr").length - 2;
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
                return;

            if (SelectedRow != null) {
                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#FFCC99';
                CurrentRow.style.color = 'Black';
                var txtFrm = CurrentRow.cells[1].getElementsByTagName("input")[0];
                txtFrm.focus();
                //alert(txtFrm.value);
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
            if (KeyCode == 40)
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38)
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

            // return false;
        }
        function LastSelectRow(CurrentRow, RowIndex) {
            var row = document.getElementById(CurrentRow);
            SelectRow(row, RowIndex);

        }


        function enableACI(ddlcharge, txtadult, txtchild, txtextrabed, rowind) {
            rw = parseInt(rowind);
            var ddlcharge1 = document.getElementById(ddlcharge);
            var txtvalue1 = document.getElementById(txtadult);

            var txtperc1 = document.getElementById(txtchild);
            var txtchargetype1 = document.getElementById(txtextrabed);

            if (ddlcharge1.value == '*' || ddlcharge1.value == '/') {
                txtvalue1.value = '';
                txtperc1.value = '';
                txtchargetype1.value = '';
                txtvalue1.disabled = false;
                txtperc1.disabled = true;
                txtchargetype1.disabled = true;
            }
            else {
                txtvalue1.disabled = false;

                txtperc1.disabled = false;
                txtchargetype1.disabled = false;

            }
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
                        <td class="td_cell" align="center">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Markup Formula" Width="100%"
                                CssClass="field_heading"></asp:Label>
                                   <asp:Hiddenfield id="hdNewForm" runat="server"></asp:Hiddenfield>
                              <asp:Hiddenfield id="hdAddinalFields" runat="server"></asp:Hiddenfield>
                        </td>
                    </tr>
                    <tr><td align="center">
                    <table style="width:70%; padding:0px; position:relative;">
                    <tr>
                        <td class="td_cell">
                            &nbsp;</td>
                        <td style="color: #000000;width:90%">
                            &nbsp;</td>                        
                    </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                <span style="color: black">Markup Formula Code</span> <span class="td_cell" 
                                    style="color: red">*</span>
                            </td>
                            <td style="color: #000000;width:90%" align="left">
                                <input onblur="chgvalue()" id="txtcode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" readonly="readonly" style="width: 220px" />
                            </td>
                        </tr>
                    <tr>
                        <td style="height:24px;width:28%" class="td_cell" align="left">
                            Markup Formula Name<span style="color: red" class="td_cell">*</span>
                        </td>
                        <td style="width:72%">
                            <input id="txtname" class="field_input" tabindex="2" type="text" maxlength="1000"
                                style="width:100%" runat="server" />
                        </td>                        
                    </tr>
                        <tr>
                            <td align="left" class="td_cell" style="height:24px;width:28%">
                                Formula Type<span class="td_cell" style="color: red">*</span>
                            </td>
                            <td align="left" style="width:72%">
                                <asp:DropDownList ID="ddlFormulaType" class="field_input" runat="server" AutoPostBack="True">
                                    <asp:ListItem>Hotel</asp:ListItem>
                                    <asp:ListItem>Country</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_cell" style="height:24px;width:28%" align="left">
                                Markup Formula Description <span class="td_cell" style="color: red">*</span>
                            </td>
                            <td style="width:72%">
                            <asp:TextBox runat="server" id="txtDescription" class="field_input" tabindex="3" TextMode="MultiLine" maxlength="2000" 
                                style="width:100%;height:50px;" ></asp:TextBox>
                              
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell" style="height:24px;width:28%">
                                <asp:Label ID="label2" runat="server" CssClass="td_ce" Text="Currency" 
                                    ViewStateMode="Enabled" Width="44px"></asp:Label>
                            </td>
                            <td align="left" style="width: 72%">
                                <asp:TextBox ID="TxtCurrencyName" runat="server" AutoPostBack="True" CssClass="field_input"
                                    MaxLength="500" TabIndex="3" Width="220px"></asp:TextBox>
                                <asp:TextBox ID="TextCurrencyCode" runat="server" Style="display: none"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="CurrencyNameAutoCompleteExtender" runat="server" CompletionInterval="10"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                    ServiceMethod="GetCurrencyName" TargetControlID="TxtCurrencyName" OnClientItemSelected="CurrencyNameautocompleteselected">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                    <tr>
                        <td class="td_cell"" align="left">
                            <asp:Label ID="label1" runat="server" CssClass="td_ce" Text="Active" ViewStateMode="enabled"
                                Width="44px"></asp:Label>
                        </td>
                        <td align="left">
                            <input id="chkActive" tabindex="3" type="checkbox" checked="checked" runat="server" />
                        </td>                        
                    </tr>
                    </table>
                    </td></tr>
                    <tr>
                       <td valign="top" style="width:70%" align="center">
                           <table style="width:100%">
                               <tbody>
                                   <tr>
                                       <td class="td_cell"  valign="top">
                                           <strong>Markup Formula</strong>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td style="width:100% " valign="top">
                                       <div style="width:100%;overflow:auto;padding-bottom:10px;">
                                           <asp:GridView ID="grdCommFormula" runat="server" AllowSorting="True" 
                                               AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" 
                                               CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" 
                                               TabIndex="12" Width="120%">
                                               <FooterStyle CssClass="grdfooter" />
                                               <Columns>
                                                   <asp:TemplateField HeaderText="Serial No" ItemStyle-Width="30px">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblFLineNo" runat="server" Text='<%# Bind("fLineNo") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" />
                                                       <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="From (Cost)" ItemStyle-Width="55px">
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="txtFrom" runat="server" Text="0"  onkeypress='validateDigitOnly(event)' CssClass="fiel_input" Width="50px"></asp:TextBox>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="To (Cost)" ItemStyle-Width="55px">
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="txtTo" runat="server"   onkeypress='validateDigitOnly(event)' CssClass="fiel_input" Width="50px"></asp:TextBox>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                             <asp:TemplateField HeaderText="Room (Comm.)"  ItemStyle-Width="75px">
                                                           <ItemTemplate>
                                                              
                                                          <asp:TextBox ID="txtRoomCommOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                               <asp:AutoCompleteExtender ID="txtRoomCommOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtRoomCommOp"> 
                                                               </asp:AutoCompleteExtender><asp:TextBox ID="txtRoomComm" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                       </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Room (Non Comm.)" ItemStyle-Width="75px">
                                                                 <ItemTemplate >
                                                             
                                                                <asp:TextBox ID="txtRoomNonCommOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                      <asp:AutoCompleteExtender ID="txtRoomNonCommOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtRoomNonCommOp">
                                                               </asp:AutoCompleteExtender>  <asp:TextBox ID="txtRoomNonComm" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>

                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Tax"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                             
                                                              <asp:TextBox ID="txtTaxOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>

                                                                             <asp:AutoCompleteExtender ID="txtTaxOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtTaxOp"> 
                                                               </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtTax" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>

                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Breakfast"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                              
                                                                <asp:TextBox ID="txtBreakfastOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>

                                                                       <asp:AutoCompleteExtender ID="txtBreakfastOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtBreakfastOp">
                                                               </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtBreakfast" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>

                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Adult EB"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                             
                                                            <asp:TextBox ID="txtAdultEBOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                    <asp:AutoCompleteExtender ID="txtAdultEBOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtAdultEBOp">
                                                               </asp:AutoCompleteExtender>
                                                                 <asp:TextBox ID="txtAdultEB" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Child Sharing"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                             
                                                                <asp:TextBox ID="txtChildSharingOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                            <asp:AutoCompleteExtender ID="txtChildSharingOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtChildSharingOp">
                                                               </asp:AutoCompleteExtender>
                                                                 <asp:TextBox ID="txtChildSharing" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Child EB"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                              
                                                                <asp:TextBox ID="txtChildEBOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                             <asp:AutoCompleteExtender ID="txtChildEBOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtChildEBOp">
                                                               </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtChildEB" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Exhibition Supplements"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                        
                                                                <asp:TextBox ID="txtExhSupOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                    <asp:AutoCompleteExtender ID="txtExhSupOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtExhSupOp">
                                                               </asp:AutoCompleteExtender>
                                                                      <asp:TextBox ID="txtExhSup" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Adult Meal Supplements"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                              
                                                              <asp:TextBox ID="txtAdultMealsSuplOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                     <asp:AutoCompleteExtender ID="txtAdultMealsSuplOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtAdultMealsSuplOp">
                                                               </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtAdultMealsSupl" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Child Meal Supplements"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                         
                                                            <asp:TextBox ID="txtChildMealsSuplOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                    <asp:AutoCompleteExtender ID="txtChildMealsSuplOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtChildMealsSuplOp">
                                                               </asp:AutoCompleteExtender>
                                                                     <asp:TextBox ID="txtChildMealsSupl" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Adult Special Events"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                        
                                                               <asp:TextBox ID="txtAdultSpclEventsOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                   <asp:AutoCompleteExtender ID="txtAdultSpclEventsOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtAdultSpclEventsOp">
                                                               </asp:AutoCompleteExtender>
                                                                      <asp:TextBox ID="txtAdultSpclEvents" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Child Special Events"  ItemStyle-Width="75px">
                                                                 <ItemTemplate>
                                                              
                                                              <asp:TextBox ID="txtChildSpclEventsOp" runat="server" CssClass="fiel_input"  onkeypress="validateOperator(event,this)" 
                                                                   Width="30px"></asp:TextBox>
                                                                     <asp:AutoCompleteExtender ID="txtChildSpclEventsOpAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                   CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                   CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                   EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                   ServiceMethod="GetOperators" TargetControlID="txtChildSpclEventsOp">
                                                               </asp:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtChildSpclEvents" runat="server" CssClass="fiel_input" onkeypress="validateDecimalOnly(event,this)"
                                                                   Width="40px"></asp:TextBox>
                                                           </ItemTemplate>
                                                           </asp:TemplateField>
                                          
                                                   <asp:TemplateField HeaderText="Adult"  Visible="false" >
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="txtAdults" runat="server" CssClass="fiel_input" 
                                                               onkeypress="validateDecimalOnly(event,this)" Width="10px"></asp:TextBox>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Child"  Visible="false" >
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="txtChild" runat="server" CssClass="fiel_input" 
                                                               onkeypress="validateDecimalOnly(event,this)" Width="10px"></asp:TextBox>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                         
                                                       <asp:TemplateField HeaderText="Action"  ItemStyle-Width="50px">
                                                      <ItemTemplate>
                                                           <asp:Button ID="btnAddRow1" runat="server" CssClass="btnAddRow" 
                                                               onclick="btnAddRow_Click1" Text="Add Row" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Delete"  ItemStyle-Width="50px">
                                                      
                                                       <ItemTemplate>
                                                           <asp:CheckBox ID="chckDeletion" runat="server" Width="10px" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                                                                      <asp:TemplateField HeaderText="Operator for Unit" Visible="false" >
                                                       <ItemTemplate>
                                                           <asp:DropDownList ID="ddlOperator"   runat="server" CssClass="fiel_input" 
                                                               Width="80px">
                                                               <asp:ListItem Value="[Select]">[Select]</asp:ListItem>
                                                               <asp:ListItem Value="*">*</asp:ListItem>
                                                               <asp:ListItem Value="/">/</asp:ListItem>
                                                               <asp:ListItem Value="+">+</asp:ListItem>
                                                               <asp:ListItem Value="-">-</asp:ListItem>
                                                           </asp:DropDownList>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Unit"  Visible="false" >
                                                    
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="txtValue" runat="server" CssClass="fiel_input" 
                                                               onkeypress="validateDecimalOnly(event,this)" Width="60px"></asp:TextBox>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="Operator for ACI"  Visible="false" >
                                                    <ItemTemplate>
                                                    <asp:DropDownList ID="ddlOperatorACI"   runat="server" CssClass="fiel_input" 
                                                               Width="80px" >
                                                               <asp:ListItem Value="[Select]">[Select]</asp:ListItem>
                                                               <asp:ListItem Value="+">+</asp:ListItem>
                                                               <asp:ListItem Value="-">-</asp:ListItem>
                                                                <asp:ListItem Value="*">*</asp:ListItem>
                                                                 <asp:ListItem Value="/">/</asp:ListItem>
                                                           </asp:DropDownList></ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Extra Bed"  Visible="false" >
                                                   
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="txtExtraBed" runat="server" CssClass="fiel_input" 
                                                               onkeypress="validateDecimalOnly(event,this)" Width="60px"></asp:TextBox>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>
                                               <RowStyle CssClass="grdRowstyle" />
                                               <SelectedRowStyle CssClass="grdselectrowstyle" />
                                               <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                               <HeaderStyle CssClass="grdheader" />
                                               <AlternatingRowStyle CssClass="grdAternaterow" />
                                           </asp:GridView>
                                           </div>
                                       </td>
                                   </tr>
                                   <tr align="center">
                                       <td style="height: 22px">
                                           <asp:Button ID="btnAddRow" runat="server" CssClass="btn" 
                                               OnClick="btnAddRow_Click" TabIndex="75" Text="Add Row" />
                                           &nbsp;
                                           <asp:Button ID="btnDeleteRow" runat="server" CssClass="btn" 
                                               OnClick="btnDeleteRow_Click" TabIndex="76" Text="Delete Row" />
                                           &nbsp;<asp:Button ID="btnCopy" runat="server" CssClass="btn" TabIndex="26" 
                                               Text="Copy Selected To Next Row" Visible="False" />
                                       </td>
                                   </tr>
                               </tbody>
                           </table>
                       </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr align="center">
                        <td class="td_cell" style="height: 23px;">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="6" Text="Save" />&nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="7" Text="Return to Search" />&nbsp;
                            <asp:Button ID="btnHelp" runat="server" CssClass="btn" TabIndex="8" Text="Help" OnClick=btnHelp_Click />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
