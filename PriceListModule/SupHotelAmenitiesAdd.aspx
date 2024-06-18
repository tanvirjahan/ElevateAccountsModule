<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupHotelAmenitiesAdd.aspx.vb" Inherits="SupHotelAmenitiesAdd" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript" charset="utf-8">
    $(document).ready(function () {
        txtmainmealAutoCompleteExtenderKeyUp();
    });
 </script>
<script language="javascript" type="text/javascript">
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
    function GetValueFrom() {

        var ddl = document.getElementById("<%=ddlSPTypeName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSPTypeCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetValueCode() {
        var ddl = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSPTypeName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

    function FormValidation(state) {

        var txtcodeval = document.getElementById("<%=txtCode.ClientID%>");
        var txtnameval = document.getElementById("<%=txtName.ClientID%>");
        var AmenityTypecodeValue = document.getElementById("<%=txtAmenityType.ClientID%>");
        var txtorderval = document.getElementById("<%=txtOrder.ClientID%>");

        if (txtnameval.value == '') {
            alert('Name Cannot be blank');
            return false;
        }
        if (txtcodeval.value == '') {
            alert('Please Enter Code');
            return false;
        }
        if (AmenityTypecodeValue.value == '') {
            alert('Please Select Amenity Type');
            return false;
        }

        if (state != 'Delete') {
//            if (txtorderval.value != '') {
//                if (txtorderval.value <= 0) {
//                    alert('Please enter order greater than zero.');
//                    return false;
//                }
//            }
//            if (txtorderval.value == '') {

//                alert('Please enter order greater than zero.');
//                return false;

//            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save ?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
        }
    }


    function txtmainmealautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=tCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=tCode.ClientID%>').value = '';
        }
    }

    function txtmainmealAutoCompleteExtenderKeyUp() {
        $("#<%=txtAmenityType.ClientID %>").bind("change", function () {
            document.getElementById('<%=tCode.ClientID%>').value = '';
        });
    }


    function checkNumber(e) {
        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }
    }
</script>


<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        txtmainmealAutoCompleteExtenderKeyUp()
    }
    </script>  

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 854px; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td style="height: 18px" class="field_heading" align="center" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Amenity" Width="301px" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px; height: 24px" class="td_cell">
                            Code <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td style="width: 57px; color: #000000; height: 24px">
                            <input style="width: 200px" id="txtCode" class="txtbox" tabindex="1" type="text" readonly
                                maxlength="20" runat="server" />
                        </td>
                        <td style="width: 105px; height: 24px" class="td_cell">
                            Amenity Type <span style="color: red" class="td_cell">*</span>
                        </td>
                        <td style="width: 318px; height: 24px">
                            <asp:TextBox ID="txtAmenityType" runat="server" AutoPostBack="True" CssClass="field_input"
                                MaxLength="500" TabIndex="3" Width="176px" ToolTip="Select Amenity Type"></asp:TextBox>
                            <asp:TextBox ID="tCode" runat="server" Style="display: none"></asp:TextBox>
                            <asp:HiddenField ID="hdntcode" runat="server" />
                            <asp:AutoCompleteExtender ID="txAmenityType_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                ServiceMethod="GetAmenityType" TargetControlID="txtAmenityType">
                            </asp:AutoCompleteExtender>
                            <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                            <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                        </td>
                        </td> </TD></tr>
                    <tr>
                        <td style="width: 111px; height: 24px" class="td_cell">
                            Name <span style="color: red" class="td_cell">*</span>
                        </td>
                        <td style="width: 57px">
                            <input style="width: 200px" id="txtName" class="txtbox" tabindex="2" type="text"
                                maxlength="100" runat="server" />
                        </td>
                        <td style="width: 105px">
                        </td>
                        <td style="width: 318px">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 111px; height: 33px; display: none" class="td_cell">
                            Supplier&nbsp;Type&nbsp;Code<span style="color: red" class="td_cell">*</span>
                        </td>
                        <td style="width: 57px; height: 33px; display: none">
                            <select style="width: 200px" id="ddlSPTypeCode" class="drpdown" tabindex="3" onchange="GetValueFrom()"
                                runat="server">
                                <option selected></option>
                            </select>
                        </td>
                        <td style="width: 105px; height: 33px; display: none" class="td_cell">
                            Supplier&nbsp;Type&nbsp;Name
                        </td>
                        <td style="width: 318px; height: 33px">
                            <select style="width: 218px; display: none" id="ddlSPTypeName" class="drpdown" tabindex="4"
                                onchange="GetValueCode()" runat="server">
                                <option selected></option>
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 111px; height: 24px" class="td_cell">
                            Rank Order
                        </td>
                        <td style="width: 57px; height: 24px">
                            <input style="width: 200px" id="txtOrder" class="txtbox" value="0" tabindex="5" type="text"
                                maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px; height: 24px;" class="td_cell">
                            Active
                        </td>
                        <td style="width: 57px; height: 24px;">
                            <input id="chkActive" tabindex="5" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px">
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn" TabIndex="6"
                                Text="Save" />
                        </td>
                        <td style="width: 237px">
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="7" Text="Return To Search" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w1" CssClass="btn" OnClick="btnhelp_Click"
                                TabIndex="8" Text="Help" />
                        </td>
                    </tr>
                </tbody>
            </table>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

 

