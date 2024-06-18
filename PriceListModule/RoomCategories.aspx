<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="RoomCategories.aspx.vb" Inherits="RoomCategories"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>

<script language="javascript" type="text/javascript">
function checkNumber()
{
    if ( event.keyCode < 45 || event.keyCode > 57 )
    {
    return false;
    }
}
function  GetValueFrom()
{

	var ddl = document.getElementById("<%=ddlSPTypeName.ClientID%>");
	ddl.selectedIndex = -1;

		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSPTypeCode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueCode()
{
	var ddl = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSPTypeName.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				
				return true;
			}
		}
}
function ValidationForOrder()
{
    if (document.getElementById("<%=txtOrder.ClientID%>").value<=0)
    {
    alert("Order must be grater than zero.");
    document.getElementById("<%=txtOrder.ClientID%>").focus();
    }
}

function disable()
{

if (document.getElementById("<%=ddlallotmentrequired.clientid%>").value=="Yes")
{
document.getElementById("<%=ddlCalculatebyPax.clientid%>").disabled=false;

document.getElementById("<%=ddlMealPlan.clientid%>").disabled=false;

document.getElementById("<%=lblDefaultNo.clientid%>").style.visibility = "visible";
document.getElementById("<%=txtDefaultAdultNo.clientid%>").style.visibility = "visible";

}
if (document.getElementById("<%=ddlallotmentrequired.clientid%>").value=="No")
{
   
    document.getElementById("<%=ddlCalculatebyPax.clientid%>").selectedIndex=1;
    document.getElementById("<%=ddlCalculatebyPax.clientid%>").disabled=true;
    document.getElementById("<%=ddlMealPlan.clientid%>").disabled=true;

    document.getElementById("<%=ddlMealPlan.clientid%>").selectedIndex = 0;
    document.getElementById("<%=lblDefaultNo.clientid%>").style.visibility = "hidden";
    document.getElementById("<%=txtDefaultAdultNo.clientid%>").value = "";
    document.getElementById("<%=txtDefaultAdultNo.clientid%>").style.visibility = "hidden";
    
    return true;
}
function ValueChange() {
    alert("entering into function");
}
function FormValidation(state)
{
   if ((document.getElementById("<%=txtCode.ClientID%>").value=="")||(document.getElementById("<%=ddlcattype.ClientID%>").value=="0") ||(document.getElementById("<%=ddlSPTypeCode.ClientID%>").value=="[Select]")  || (document.getElementById("<%=txtName.ClientID%>").value=="")||(document.getElementById("<%=txtPrintName.ClientID%>").value=="")||(document.getElementById("<%=txtOrder.ClientID%>").value=="")||(document.getElementById("<%=txtOrder.ClientID%>").value<=0))
       {
       if (document.getElementById("<%=ddlcattype.ClientID%>").value==0)
           {
            document.getElementById("<%=ddlcattype.ClientID%>").focus(); 
             alert("Select Category Type");
            return false;
           }
           else if (document.getElementById("<%=txtCode.ClientID%>").value=="")
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
           else if (document.getElementById("<%=txtPrintName.ClientID%>").value=="") 
           {
           document.getElementById("<%=txtPrintName.ClientID%>").focus();
           alert("Print name field can not be blank");
            return false;
           }
//          else if (document.getElementById("<%=txtNoPerson.ClientID%>").value=="") 
//           {
//           document.getElementById("<%=txtNoPerson.ClientID%>").focus();
//           alert("No. of person field can not be blank");
//            return false;
//           }
           else if (document.getElementById("<%=txtOrder.ClientID%>").value=="")
           {
            document.getElementById("<%=txtOrder.ClientID%>").focus();
            alert("Order field can not be blank");
            return false;
           }
           else if (document.getElementById("<%=ddlSPTypeCode.ClientID%>").value=="[Select]")
            {
           document.getElementById("<%=ddlSPTypeCode.ClientID%>").focus();
           alert("Select Supplier Type");
            return false;
           }
//            else if (document.getElementById("<%=txtNoPerson.ClientID%>").value<=0)
//           {
//             document.getElementById("<%=txtNoPerson.ClientID%>").focus();
//             alert("No. of person must be grater than zero.");
//             return false;
//           }
           else if (document.getElementById("<%=txtOrder.ClientID%>").value<=0)
           {
             document.getElementById("<%=txtOrder.ClientID%>").focus();
             alert("Order must be grater than zero.");
             return false;
           }
       }
       else
       {
       if (state=='New'){if(confirm('Are you sure you want to save room category?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update room category?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete room category?')==false)return false;}
       }
}
</script>
 <script language="javascript" type ="text/javascript" >
     function mealautocompleteselected(source, eventArgs) {
         if (eventArgs != null) {
             document.getElementById('<%=txtmealcode.ClientID%>').value = eventArgs.get_value();
         }
         else {
             document.getElementById('<%=txtmealcode.ClientID%>').value = '';
         }
     }

     function mealautocompleteremove() {
         document.getElementById('<%=txtmealcode.ClientID%>').value = '';
     }

     function AutoCompleteExtenderKeyUp() {
         $("#<%= txtmealname.ClientID %>").bind("change", function () {
             document.getElementById('<%=txtmealcode.ClientID%>').value = '';
         });

     }

</script>

<script type="text/javascript">

    var result_style = document.getElementById('result_tr').style;
    result_style.display = 'table-row';

    function updatetablerows() {
        var result_style = document.getElementById('result_tr').style;
        result_style.display = 'none';

    }
    

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);

    function InitializeRequestUserControl(sender, args) {

    }

    function EndRequestUserControl(sender, args) {
        AutoCompleteExtenderKeyUp();
    }
    </script>
<script type="text/javascript" charset="utf-8">
    $(document).ready(function () {


        AutoCompleteExtenderKeyUp();

    });

</script>



    <asp:UpdatePanel id="UpdatePanel1" runat="server">
     <contenttemplate>
<TABLE style="border: 2px solid gray; WIDTH: 916px; " >
<TBODY>
<TR>
<TD style="HEIGHT: 17px" class="td_cell" align=center colSpan=4>
<asp:Label id="lblHeading" runat="server" Text="Add New Room Category" ForeColor="White" CssClass="field_heading" Width="744px"></asp:Label><SPAN style="COLOR: #ff0000"> </SPAN></TD></TR>
<TR style="COLOR: #ff0000">
<TD style="WIDTH: 89px; HEIGHT: 12px" class="td_cell">
<SPAN style="COLOR: black">Code <SPAN style="COLOR: red" class="td_cell">*</SPAN></SPAN>
</TD>
<TD style="WIDTH: 134px; COLOR: #000000; HEIGHT: 12px">
<INPUT style="WIDTH: 218px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /></TD>
<TD style="WIDTH: 131px; HEIGHT: 12px"></TD>
<TD style="WIDTH: 199px; HEIGHT: 12px"></TD>
</TR>
<TR>
<TD style="WIDTH: 89px; HEIGHT: 10px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="FONT-SIZE: 12pt; WIDTH: 134px; FONT-FAMILY: Times New Roman; HEIGHT: 10px">
<INPUT style="WIDTH: 216px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD>
<TD style="WIDTH: 131px; HEIGHT: 10px" class="td_cell"">
Category Type<SPAN style="COLOR: #ff0000">*</SPAN></SPAN>
</TD>
<TD style="FONT-SIZE: 12pt; WIDTH: 199px; FONT-FAMILY: Times New Roman; HEIGHT: 10px">
<asp:DropDownList id="ddlcattype" AutoPostBack=true  runat="server"  CssClass="field_input"  Width="160px">
<%--<asp:ListItem Value="3">[Select]</asp:ListItem>
<asp:ListItem Value="0"> Adult Accomodation</asp:ListItem>
<asp:ListItem Value="1">Child Accomodation </asp:ListItem>
<asp:ListItem Value="2">Extra</asp:ListItem>--%>
</asp:DropDownList>
</TD>
</TR>
<TR style="FONT-SIZE: 12pt; FONT-FAMILY: Times New Roman;display:none">
<TD style="WIDTH: 89px; HEIGHT: 22px" class="td_cell">Supplier Type Code&nbsp;<SPAN style="COLOR: red" class="td_cell">* </SPAN></TD>
<TD style="WIDTH: 134px; HEIGHT: 22px">
<SELECT style="WIDTH: 224px" id="ddlSPTypeCode" class="field_input" tabIndex=3 onchange="GetValueFrom()" runat="server"> 
<OPTION selected></OPTION>
</SELECT>
</TD>
<TD style="WIDTH: 131px; HEIGHT: 22px" class="td_cell">Supplier Type&nbsp; Name&nbsp;</TD>
<TD style="WIDTH: 233px; HEIGHT: 22px">
<SELECT style="WIDTH: 256px" id="ddlSPTypeName" class="field_input" tabIndex=4 
        onchange="GetValueCode()" runat="server"> <OPTION selected></OPTION></SELECT>
</TD>
</TR>
<TR >
<TD style="WIDTH: 89px; HEIGHT: 21px" class="td_cell">Print Name <SPAN style="COLOR: #ff0000">*</SPAN></TD>
    <TD style="WIDTH: 134px; HEIGHT: 21px"><INPUT style="WIDTH: 218px" id="txtPrintName" class="field_input" tabIndex=5 type=text maxLength=100 runat="server" /></TD>
<TD style="WIDTH: 131px; HEIGHT: 21px" class="td_cell"><asp:Label ID="lblmealplan" 
        runat="server" Text="Whether Meal Plan"></asp:Label></TD>
        <TD style="WIDTH: 199px; HEIGHT: 21px">
        <asp:DropDownList id="ddlMealPlan" onchange="toggleRow(this);" tabIndex=6 runat="server" CssClass="field_input" Width="52px">
        <asp:ListItem>No</asp:ListItem>
        <asp:ListItem>Yes</asp:ListItem>
        </asp:DropDownList>&nbsp;
                
     
    </TD></TR>
    
<TR id="result_tr" style="display: visible;">
<TD style="empty-cells: hide;"td_cell">&nbsp;</TD>
<TD style="empty-cells: hide;">&nbsp;</TD>
<TD style="empty-cells: hide;"td_cell">
    <asp:Label ID="lbllinkedmeal" runat="server" CssClass="td_cell"  Text="Linked Meal" Visible="False"></asp:Label>
    </TD>
<TD style="empty-cells: hide;">

                    
                        <asp:TextBox ID="txtmealname" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="176px"></asp:TextBox>
                           <asp:TextBox ID="txtmealcode" runat="server" AutoPostBack="True" MaxLength="20"  visible="true" BorderStyle="None" BorderWidth="0px" BackColor="White" BorderColor="White"  ></asp:TextBox>
                          
                        <asp:AutoCompleteExtender ID="txtmealname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="mealautocompleteselected"
                            ServiceMethod="GetMeals" TargetControlID="txtmealname">
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text1" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text2" class="field_input" type="text"
                             runat="server" />
          
                  <%--  <asp:TextBox ID="txtmealcode" runat="server" __designer:wfdid="w147" 
                AutoPostBack="True" color="white" MaxLength="20" 
                tabIndex="1" visible="true" Width="0px" BackColor="White" BorderColor="White" 
                        BorderStyle="None" BorderWidth="0px"></asp:TextBox>--%>

    </TD>
</TR>

<TR>

<TD class="td_cell" style="empty-cells: hide">No. Of Person <SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD style="empty-cells: hide">
<INPUT style="WIDTH: 218px; TEXT-ALIGN: right" id="txtNoPerson" class="field_input" tabIndex=7 onkeypress="return checkNumber()" type=text maxLength=9 runat="server" /></TD>
<TD class="td_cell" style="empty-cells: hide">
    <asp:Label ID="lblallotment" runat="server" Text="Allotment required"></asp:Label>
    </TD>
<TD style="empty-cells: hide">
<asp:DropDownList id="ddlAllotmentRequired" tabIndex=8 runat="server" 
        CssClass="field_input" Width="52px" onchange="disable()">
<asp:ListItem>Yes</asp:ListItem>
<asp:ListItem>No</asp:ListItem>
</asp:DropDownList>
</TD>


</TR>
<TR><TD style="WIDTH: 89px; " class="td_cell">Order <span style="COLOR: #ff0000">*</span></TD>
<TD style="WIDTH: 134px; ">
<INPUT id="txtOrder" tabIndex=9 type=text runat="server" class="field_input" maxLength="9" onkeypress="return checkNumber()" style="WIDTH: 218px; TEXT-ALIGN: right" /></TD>
<TD style="WIDTH: 131px; display: none;" class="td_cell">
    Calculate by Pax</TD>
<TD style="WIDTH: 199px; display: none;">
    <asp:DropDownList ID="ddlCalculatebyPax" runat="server" CssClass="field_input" 
        tabIndex="10" Width="52px">
        <asp:ListItem>Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:DropDownList>
    </TD>
</TR>
<TR>
<TD style="WIDTH: 89px; HEIGHT: 22px" class="td_cell">Active</TD>
    <TD style="HEIGHT: 22px; width: 134px;">
        <INPUT id="chkActive" tabIndex=11 type=checkbox CHECKED runat="server" />
    </TD>
    <td class="td_cell" style="WIDTH: 131px; HEIGHT: 22px">
        <asp:Label ID="lblDefaultNo" runat="server" 
            Text="Default No of adults to show in Reservation"></asp:Label>
    </td>
    <td style="WIDTH: 199px; HEIGHT: 22px">
        <INPUT style="WIDTH: 52px; " id="txtDefaultAdultNo" class="field_input" tabIndex=7 
        onkeypress="return checkNumber()" type=text maxLength=9 runat="server" />
    </td>
    </TR><TR>
    <TD class="td_cell" style="width: 89px; height: 22px;">Display in web</TD>
        <TD colSpan=3 style="HEIGHT: 22px">
            <textarea id="txtDisplayinWeb" runat="server" class="field_input" rows="2" 
                style="WIDTH: 451px" tabindex="12">
            </textarea>
            <asp:Label ID="lblJavaScript" runat="server" Text="Label" Visible="False"></asp:Label>
        </TD></TR><TR>
        <TD class="td_cell" style="width: 89px"></TD><TD colSpan=3>
        <INPUT id="txtUnitName" tabIndex=13 type=text runat="server" Visible="false" class="field_input" maxLength="10" style="WIDTH: 216px" /></TD></TR><TR>
    <TD style="WIDTH: 89px" class="td_cell"></TD>
    <TD colspan="3">
        <INPUT id="chkAutoConfirm" tabIndex=14 
                type=checkbox runat="server" Visible="false" />
        </TD></TR>
    <tr>
        <td style="WIDTH: 89px">
            <asp:Button ID="btnSave" runat="server" CssClass="field_button" tabIndex="15" 
                Text="Save" />
        </td>
        <td style="WIDTH: 134px">
            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" tabIndex="16" 
                Text="Return To Search" />
        </td>
        <td style="WIDTH: 131px">
            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w31" 
                CssClass="field_button" onclick="btnhelp_Click" tabIndex="17" Text="Help" />
        </td>
        <td style="WIDTH: 199px">
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate> 
    </asp:UpdatePanel>
</asp:Content>

