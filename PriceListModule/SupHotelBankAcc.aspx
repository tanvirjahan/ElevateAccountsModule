<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupHotelBankAcc.aspx.vb" Inherits="SupHotelBankAcc" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
    <%@ OutputCache location="none" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript" >

<!--
    //WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
    // -->

    function currencyautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcurrencycode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcurrencycode.ClientID%>').value = '';
        }

    }

    function currencyAutoCompleteExtenderKeyUp() {

        $("#<%= txtcurrencyname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtcurrencyname.ClientID%>').value == '') {

                document.getElementById('<%=txtcurrencycode.ClientID%>').value = '';
            }

        });

        $("#<%= txtcurrencyname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtcurrencyname.ClientID%>').value == '') {

                document.getElementById('<%=txtcurrencycode.ClientID%>').value = '';
            }

        });

    }


    function CallWebMethod(methodType) {
        switch (methodType) {
            case "partycode":
                var select = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
        }
    }


    function checkTelephoneNumber(e) {

        if ((event.keyCode < 45 || event.keyCode > 57)) {
            return false;
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
 <style>
 .bgrow
 {
 background-color:white; 

 }

 </style> 
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="" align=left>
<TBODY>
 <tr>
                <td colspan ="20" align ="left" class="bgrow" >
               
                    <whc:hoteltab ID="whotelatbcontrol" runat="server" />
               
                
                </td>
                </tr>
                <tr>
                <td>
                <div style="margin-top:-6px;margin-left:13px;">
                <table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid;" class="td_cell" align=left>
                
                
<TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier" CssClass="field_heading" Width="100%" ForeColor="White"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtName" class="field_input" tabIndex=4 type=text maxLength=100 runat="server" /></TD></TR>
    <tr>
        <td align="left" valign="top" width="150">
            &nbsp;</td>
        <td align="left" class="td_cell" valign="top">
            <table style="width: 100%">
                <tr>
                    <td>
                        Supplier</td>
                    <td>
                        <select ID="ddlSuppierCD" runat="server" class="field_input" name="D1" 
                            onchange="CallWebMethod('partycode');" style="WIDTH: 220px">
                            <option selected=""></option>
                        </select></td>
                    <td>
                        <select ID="ddlSuppierNM" runat="server" class="field_input" name="D2" 
                            onchange="CallWebMethod('partyname');" style="WIDTH: 310px">
                            <option selected=""></option>
                        </select></td>
                    <td>
                        <asp:Button ID="btnfilldetail" runat="server" CssClass="field_button" 
                            Text="Fill Details" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 450px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
        <table style="width: 656px">
            <tbody>
                <tr>
                    <td style="width: 80px" class="td_cell" colspan="2">
                        <asp:Panel ID="PanelHotelAccountDetails" runat="server" Width="699px" GroupingText="Hotel Account Details">
                            <table style="width: 412px" border="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                        </td>
                                        <td style="width: 100px" align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            Account Name
                                        </td>
                                        <td style="width: 100px" align="left">
                                            <input style="width: 295px" id="txtHotAccountName" class="field_input" tabindex="33"
                                                type="text" maxlength="50" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            Account Number
                                        </td>
                                        <td style="width: 100px" align="left">
                                            <input style="width: 295px" id="txtHotAccountNumber" class="field_input" tabindex="34"
                                                type="text" maxlength="50" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            Bank Name
                                        </td>
                                        <td style="width: 100px" align="left">
                                            <input style="width: 295px" id="txtHotAccountBankName" class="field_input" tabindex="35"
                                                type="text" maxlength="50" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            Branch Name
                                        </td>
                                        <td style="width: 100px" align="left">
                                            <input style="width: 295px" id="txtHotAccountBranchName" class="field_input" tabindex="36"
                                                type="text" maxlength="100" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            Swift Code
                                        </td>
                                        <td style="width: 100px" align="left">
                                            <input style="width: 295px" id="txtHotAccountSWIFT" class="field_input" tabindex="37"
                                                type="text" maxlength="100" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; height: 26px" align="left">
                                            IBAN
                                        </td>
                                        <td style="width: 100px; height: 26px" align="left">
                                            <input style="width: 295px" id="txtHotAccountIBAN" class="field_input" tabindex="38"
                                                type="text" maxlength="100" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            Account Currency
                                        </td>
                                        <td style="width: 100px" align="left">
                                            

                                             <asp:TextBox ID="txtcurrencyname" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcurrencycode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncurrency" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="Currency_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getcurrencylist" TargetControlID="txtcurrencyname" OnClientItemSelected="currencyautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text7" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text8" class="field_input" type="text" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            <asp:Button ID="BtnSaleSave" TabIndex="39" runat="server" Text="Save" CssClass="field_button">
                                            </asp:Button>
                                        </td>
                                        <td style="width: 230px" align="left">
                                            <asp:Button ID="BtnSaleCancel" TabIndex="40" OnClick="BtnSaleCancel_Click" runat="server"
                                                Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;
                                            <asp:Button ID="btnhelp" TabIndex="41" OnClick="btnhelp_Click" runat="server" Text="Help"
                                                CssClass="field_button" __designer:wfdid="w10"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </tbody>
        </table>
</ContentTemplate>
</asp:UpdatePanel> </DIV>
</TD></TR>
</table>
                </div>
                </td>
                </tr>


</TBODY>
</TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

