<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SupplierCategories.aspx.vb" Inherits="SupplierCategories" MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
  <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
  <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script> 
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            AutoCompleteExtenderKeyUp();
            

        });
</script>


<script language="javascript" type="text/javascript">
function checkNumber()
{
   if (event.keyCode < 45 || event.keyCode > 57)
    {
         return false;
    }
}
function FormValidation(state) {
   
    if ((document.getElementById("<%=txtCode.ClientID%>").value=="") || (document.getElementById("<%=txtName.ClientID%>").value=="")||(document.getElementById("<%=ddlSupplierType.ClientID%>").value=="[Select]")||(document.getElementById("<%=txtOrder.ClientID%>").value=="")||(document.getElementById("<%=txtOrder.ClientID%>").value<=0) )
       {
           if (document.getElementById("<%=txtCode.ClientID%>").value=="")
           {
            document.getElementById("<%=txtCode.ClientID%>").focus(); 
             alert("Code field can not be blank");
            return false;
           }
           
           else if (document.getElementById("<%=txtName.ClientID%>").value=="")
            {
           document.getElementById("<%=txtName.ClientID%>").focus();
           alert("Name field can not be blank");
            return false;
           }
        else if (document.getElementById("<%=txthotelcode.ClientID%>").value == "") 
               {
                   document.getElementById("<%=txthotelcode.ClientID%>").focus();
                   alert("Select Supplier Type");
                    return false;
               }
               
              else if (document.getElementById("<%=txtOrder.ClientID%>").value=="")
              {
              alert("Order field can not be blank");
              document.getElementById("<%=txtOrder.ClientID%>").focus();
              return false;
              }
              else if (document.getElementById("<%=txtOrder.ClientID%>").value<=0)
              {
              alert("Order field should be greater than zero");
              document.getElementById("<%=txtOrder.ClientID%>").focus();
              return false;
              }
              else if (document.getElementById("<%=txtOrder.ClientID%>").value<=0)
              {
              alert("Order field should be greater than zero");
              document.getElementById("<%=txtOrder.ClientID%>").focus();
              return false;
              }
          }
       else {
         var val = document.getElementById("<%=txtCode.ClientID%>").value;

         if ((!val.match(/^[a-zA-Z0-9\-\/\_]+$/)) && (state == 'New')) {
           
             alert('Only alphabets,digits,-,_,/ are allowed');
                      return false    
                       }
         
             
                   return true;
                      
       if (state=='New'){if(confirm('Are you sure you want to save supplier category?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update supplier category?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete supplier category?')==false)return false;}
       }
}

function hotelautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=txthotelcode.ClientID%>').value = eventArgs.get_value();
        
    }
    else {
        document.getElementById('<%=txthotelcode.ClientID%>').value = '';
    }

}

function propertytypeautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=txtpropertytypecode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=txtpropertytypecode.ClientID%>').value = '';
    }
}


function propertytypesAutoCompleteExtenderKeyUp() {

    $("#<%= txtpropertytypename.ClientID %>").bind("change", function () {

        if (document.getElementById('<%=txtpropertytypename.ClientID%>').value == '') {

            document.getElementById('<%=txtpropertytypecode.ClientID%>').value = '';
        }

    });

    $("#<%= txtpropertytypename.ClientID %>").keyup(function (event) {

        if (document.getElementById('<%=txtpropertytypename.ClientID%>').value == '') {

            document.getElementById('<%=txtpropertytypecode.ClientID%>').value = '';
        }

    });

} 


function AutoCompleteExtenderKeyUp() {
    $("#<%=txthotelname.ClientID %>").bind("change", function () {
        document.getElementById('<%=txthotelcode.ClientID%>').value = '';
    });
}
function  GetSpTypeValueFrom()
{

	var ddl = document.getElementById("<%=ddlSupplierTypeName.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSupplierType.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetSpTypeValueCode()
{
	var ddl = document.getElementById("<%=ddlSupplierType.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSupplierTypeName.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


</script>


<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        AutoCompleteExtenderKeyUp();
        propertytypesAutoCompleteExtenderKeyUp();
       
    }
</script>











    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 631px; BORDER-BOTTOM: gray 2px solid">
<TBODY>
<TR>
<TD style="HEIGHT: 17px" class="td_cell" align=center colSpan=4><asp:Label id="lblHeading" runat="server" Text="Add New Supplier Category" ForeColor="White" CssClass="field_heading" Width="100%"></asp:Label></TD></TR>
<TR>
<TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell"><SPAN>Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 82px; COLOR: #000000; HEIGHT: 24px">
<INPUT id="txtCode" class="txtbox" tabIndex=1 type=text maxLength=20 runat="server" /> </TD>
<TD style="WIDTH: 147px; HEIGHT: 24px"></TD><TD style="WIDTH: 159px; HEIGHT: 24px"></TD>
</TR>
<TR>
<TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 82px"><INPUT id="txtName" class="txtbox" tabIndex=2 type=text maxLength=100 runat="server" /> </TD>
<TD style="WIDTH: 147px"></TD><TD style="WIDTH: 159px"></TD>
</TR>
<TR style="display:none">
<TD style="WIDTH: 200px; HEIGHT: 16px" class="td_cell">Supplier&nbsp;Type&nbsp;Code<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 82px; HEIGHT: 16px"><SELECT onchange="GetSpTypeValueFrom()" style="WIDTH: 128px" id="ddlSupplierType" class="drpdown" tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT>
</TD>
<TD style="WIDTH: 200px; HEIGHT: 16px" class="td_cell">Supplier&nbsp;Type&nbsp;Name</TD>
<TD style="WIDTH: 159px; HEIGHT: 16px">
<SELECT onchange="GetSpTypeValueCode()" style="WIDTH: 378px" id="ddlSupplierTypeName" class="drpdown" tabIndex=4 runat="server"> 
<OPTION selected></OPTION></SELECT>
</TD>
</TR>
<tr>
<TD style="WIDTH: 200px; HEIGHT: 16px" class="td_cell">Supplier&nbsp;Type&nbsp;Name<span
style="color: #ff0000">*</span>
</td>
<td align="left" valign="top" colspan="2" width="300px">
<asp:TextBox ID="txthotelname" runat="server" AutoPostBack="True" CssClass="field_input"
MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
<asp:TextBox ID="txthotelcode" runat="server" style="display:none"></asp:TextBox>
<asp:HiddenField ID="hdnpartycode" runat="server" />
<asp:AutoCompleteExtender ID="txthotelname_AutoCompleteExtender" runat="server" CompletionInterval="10"
CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
ServiceMethod="Gethoteltypelist" TargetControlID="txthotelname" OnClientItemSelected="hotelautocompleteselected">
</asp:AutoCompleteExtender>
<input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
<input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
</td>
</tr>

<TR>
<TD style="WIDTH: 145px" class="td_cell">Order <SPAN style="COLOR: #ff0033">*</SPAN>
</TD>
<TD style="WIDTH: 82px">
<INPUT style="TEXT-ALIGN: right" id="txtOrder" class="txtbox" tabIndex=5 onkeypress="return checkNumber()" type=text maxLength=100 runat="server" /></TD>
<TD style="WIDTH: 147px"></TD>
<TD style="WIDTH: 159px"></TD>
</TR>
<TR>
<TD style="WIDTH: 145px; HEIGHT: 22px" class="td_cell">Active</TD><TD style="WIDTH: 82px; HEIGHT: 22px">
<INPUT id="chkActive" tabIndex=6 type=checkbox CHECKED runat="server" /></TD><TD style="WIDTH: 147px; HEIGHT: 22px"></TD>
<TD style="WIDTH: 159px; HEIGHT: 22px"></TD>
</TR>
  <tr>
    <td style="width: 88px" align="left" class="td_cell">
        <asp:Label ID="Label1" runat="server" Text="Property Type" Width="120px"></asp:Label>
    </td>
    <td align="left" valign="top" colspan="2" width="300px">
        <asp:TextBox ID="txtpropertytypename" runat="server"  CssClass="field_input"
            MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
        <asp:TextBox ID="txtpropertytypecode" runat="server" style="display:none" ></asp:TextBox>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:AutoCompleteExtender ID="PropertyType_AutoCompleteExtender" runat="server" CompletionInterval="10"
            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
            ServiceMethod="Getpropertytypelist" TargetControlID="txtpropertytypename" OnClientItemSelected="propertytypeautocompleteselected">
        </asp:AutoCompleteExtender>
        <input style="display: none" id="Text23" class="field_input" type="text" runat="server" />
        <input style="display: none" id="Text24" class="field_input" type="text" runat="server" />
    </td>
</tr>
<TR>
<TD style="WIDTH: 145px; HEIGHT: 22px"><asp:Button id="btnSave" tabIndex=7 runat="server" Text="Save" CssClass="btn"></asp:Button></TD>
<TD style="WIDTH: 82px; HEIGHT: 22px"><asp:Button id="btnCancel" tabIndex=8 runat="server" Text="Return To Search" CssClass="btn"></asp:Button></TD>
<TD style="WIDTH: 147px; HEIGHT: 22px"><asp:Button id="btnhelp" tabIndex=9 runat="server" Text="Help" CssClass="btn" __designer:wfdid="w14" OnClick="btnhelp_Click"></asp:Button></TD>
<TD style="WIDTH: 159px; HEIGHT: 22px"></TD>
</TR>
</TBODY>
</TABLE>&nbsp; 
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
