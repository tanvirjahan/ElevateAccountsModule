<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="UpdateSupplierBooking.aspx.vb" Inherits="UpdateSupplierBooking" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>
<script type="text/javascript">

    function supplier_OnClientPopulating(sender, args) {
        var hiddenfieldID = sender.get_id().replace("AutoCompleteExtender_Supplier", "lblServiceType");
        var serviceType = document.getElementById(hiddenfieldID).innerHTML;
        sender.set_contextKey(serviceType + ";" + document.getElementById('<%=chkShowAllSupplier.ClientID%>').checked);        
    }

    function supplierautocompleteselected(source, eventArgs) {
        var hiddenfieldID = source.get_id().replace("AutoCompleteExtender_Supplier", "txtSupplierCode");
        if (eventArgs != null) {
            document.getElementById(hiddenfieldID).value = eventArgs.get_value();
        }
        else {
            document.getElementById(hiddenfieldID).value = '';
        }
    }

    function ClearCode(source, sourceCode) {
        if (source.value == "") {
            document.getElementById(sourceCode).value = "";
        }
    }

    function fnFindCostValue(txtPaxorunitrate, lblNoUnit, txtCostValue) {

          var Paxorunitrate=document.getElementById(txtPaxorunitrate).value;
          var lblNoUnit=document.getElementById(lblNoUnit).innerHTML;
         var noUnits=0;
         var paxrate;
         if(isNaN(Paxorunitrate))
         {
            paxrate=0;
         }
         else
         {
            paxrate=Paxorunitrate;
         }
         if(isNaN(lblNoUnit))
         {
            noUnits=0;
         }
         else
         {
            noUnits=lblNoUnit;
         }
        document.getElementById(txtCostValue).value=noUnits*paxrate;
        txtCostValue.Focus();
           
    }

    function fnInHouseProviderChange(InHouseProvider, Paxorunitrate, CostValue, Complimentary, Supplier, SupplierCode) {
        var chkInHouseProvider = document.getElementById(InHouseProvider);
        var txtPaxorunitrate = document.getElementById(Paxorunitrate);
        var txtCostValue = document.getElementById(CostValue);
        var chkComplimentary = document.getElementById(Complimentary);
        var txtSupplier = document.getElementById(Supplier);
        var txtSupplierCode = document.getElementById(SupplierCode);

        if (chkInHouseProvider.checked) {

            txtPaxorunitrate.value = 0;
            txtPaxorunitrate.disabled = true;
            txtCostValue.value = 0;
            chkComplimentary.checked = false
            chkComplimentary.disabled = true;
            txtSupplier.value = '';
            txtSupplierCode.value = '';
            txtSupplier.disabled = true;

        }
        else {

            txtSupplier.disabled = false;
            txtPaxorunitrate.disabled = false;
            chkComplimentary.disabled = false;
        }


    }
    function fnComplimentaryChange(InHouseProvider, Paxorunitrate, CostValue, Complimentary, Supplier, SupplierCode) {
      
        var chkInHouseProvider = document.getElementById(InHouseProvider);
        var txtPaxorunitrate = document.getElementById(Paxorunitrate);
        var txtCostValue = document.getElementById(CostValue);
        var chkComplimentary = document.getElementById(Complimentary);
        var txtSupplier = document.getElementById(Supplier);
        var txtSupplierCode = document.getElementById(SupplierCode);

        if (chkComplimentary.checked) {
            txtPaxorunitrate.value = 0;
            txtPaxorunitrate.disabled = true;
            txtCostValue.value = 0;
            chkInHouseProvider.checked = false;
            chkInHouseProvider.disabled = true;
        }
        else {
            txtPaxorunitrate.disabled = false
            chkInHouseProvider.disabled = false

        }

    }
</script>


<style type="text/css">
.gv_Title 
{
 	font-family: Arial,Verdana, Geneva, ms sans serif;
	font-size: 12pt;
	font-weight: bold;
	font-style:normal;
	font-variant: normal;
	border-width:1px;
	border-color:#06788B;
	color:#06788B;
	margin-left: 0px;
}

</style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tr>
                    <td class="td_cell" align="center" style="width: 100%;">
                        <asp:Label ID="lblHeading" runat="server" Text="Update Supplier in Booking" Style="padding: 2px"
                            CssClass="field_heading" Width="100%">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                    <table>
                    <tr>
                    <td><label class="field_caption">Booking No</label></td>
                    <td><asp:TextBox ID="txtBookingNo" CssClass="field_input" runat="server" TabIndex="1" Width="90%" style="background-color:#DCDCDC" ReadOnly="true"></asp:TextBox></td>
                    <td>
                        <asp:Label ID="Label1" runat="server">
                            <asp:CheckBox ID="chkShowAllSupplier" runat="server" />
                            Show All Suppliers </asp:Label>                    
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    </tr>
                    </table>
                    </td>
                </tr>
                <tr>
                <td style="padding-top:10px;">
                <label class="gv_Title">List of Services</label></td>
                </td>
                </tr>
                <tr>                
                <td style="width: 100%">
                    <div id="divGrid" style="min-height: 460px; max-height: 460px; max-width: 96vw; overflow: scroll">
                        <asp:GridView ID="gvUpdateSupplier" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                            Width="100%" Style="padding-top: 2px; padding-bottom: 3px;" TabIndex="10">
                            <Columns>  
                                <asp:TemplateField HeaderText="Service Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceType" runat="server" Text='<%# Bind("serviceType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Service Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceDate" runat="server" Text='<%# Bind("ServiceDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                     
                                <asp:TemplateField HeaderText="Row ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowId" runat="server" Text='<%# Bind("elineno") %>'></asp:Label>
                                        <asp:Label ID="lblRownumber" runat="server" Text='<%# Bind("rownumber") %>' style ="display:none"></asp:Label>
                                        <asp:Label ID="lblSublineno" runat="server" Text='<%# Bind("sublineno") %>' style ="display:none"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Service Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("serviceName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Rate Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRateType" runat="server" Text='<%# Bind("rateType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                                                
                                <asp:TemplateField HeaderText="Pax Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaxType" runat="server" Text='<%# Bind("paxType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="No of Pax or Unit" HeaderStyle-Width="70">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoUnit" runat="server" Text='<%# Bind("noofpaxorunit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Child No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChildNo" runat="server" Text='<%# Bind("childno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Child Age">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChildAge" runat="server" Text='<%# Bind("childAge") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Supplier">
                                    <ItemTemplate>                                        
                                        <asp:TextBox ID="txtSupplier" Width="180"  runat="server" Text='<%# Bind("partyName") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtSupplierCode" runat="server" style="display:none" Text='<%# Bind("partyCode") %>'></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender_Supplier" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="-1"
                                            ServiceMethod="GetSupplier" TargetControlID="txtSupplier" ContextKey="true" 
                                            OnClientPopulating="supplier_OnClientPopulating" OnClientItemSelected="supplierautocompleteselected">
                                        </asp:AutoCompleteExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pax or Unit Rate">
                                    <ItemTemplate>                                        
                                        <asp:TextBox ID="txtPaxorunitrate" runat="server" Width="100" Text='<%# Bind("paxorunitrate") %>' ></asp:TextBox> <%--OnTextChanged="txtPaxorunitrate_TextChanged" AutoPostBack="true"--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Value">
                                    <ItemTemplate>                                        
                                        <asp:TextBox ID="txtCostValue" runat="server" Width="100" Text='<%# Bind("costvalue") %>' ReadOnly="true" style="background-color:#DCDCDC"></asp:TextBox>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Complimentary from Supplier">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkComplimentary" runat="server" Checked='<%# Bind("Complimentary") %>' 
                                       />                 <%-- AutoPostBack="true"   OnCheckedChanged="chkComplimentary_CheckedChanged"  --%>                    
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In House Provider" HeaderStyle-Width="70">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkInHouseProvider" runat="server" Checked='<%# Bind("InHouseProvider") %>' 
                                       />                           <%-- AutoPostBack="true"    OnCheckedChanged="chkInHouseProvider_CheckedChanged"   --%>       
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>   
                                    <asp:TemplateField HeaderText="Purchase Invoice No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseInvoiceNo" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                               
                            </Columns>
                            <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle"
                                ForeColor="White" BorderColor="LightGray" />
                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                            <RowStyle CssClass="grdRowstyle" Wrap="true" />
                            <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                            <FooterStyle CssClass="grdfooter" />
                        </asp:GridView>                        
                    </div>
                </td>
                </tr>
                <tr>
                <td align="center" style="padding-top: 8px;">                    
                    <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="14" Text="Save" />&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="15" Text="Return To Search" />
                </td>
                </tr>
            </table>            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

