<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="DepartmentMaster.aspx.vb" Inherits="DepartmentMaster"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
 
 
   <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript"
       charset="utf-8"></script>
    <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>
 
 
 <script type="text/javascript" charset="utf-8">
     $(document).ready(function () {

   txtDeptnameAutoCompleteExtenderKeyUp();
      
     });
</script>
<script language ="javascript" type ="text/javascript" >

    function txtDeptnameAutoCompleteExtenderKeyUp() {
        $("#<%=txtDeptname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtDeptname.ClientID%>').value == '') {

                document.getElementById('<%=txtDeptcode.ClientID%>').value = '';
            }

        });

        $("#<%=txtDeptname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtDeptname.ClientID%>').value == '') {

                document.getElementById('<%=txtDeptcode.ClientID%>').value = '';
            }

        });
    }

    function txtDeptnameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtDeptcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtDeptcode.ClientID%>').value = '';
        }
    }


 function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }



function FormValidation(state)
{
if ((document.getElementById("<%=txtCode.ClientID%>").value=="")||(document.getElementById("<%=txtCode.ClientID%>").value<=0)||(document.getElementById("<%=txtName.ClientID%>").value==""))
    {
       if (document.getElementById("<%=txtCode.ClientID%>").value=="")
	    {
            document.getElementById("<%=txtCode.ClientID%>").focus(); 
             alert("Code field can not be blank");
            return false;
         }
         else if (document.getElementById("<%=txtCode.ClientID%>").value<=0)
              {
              alert("Code must be greater than zero.");
              document.getElementById("<%=txtCode.ClientID%>").focus();
              return false;
              }
         else if (document.getElementById("<%=txtName.ClientID%>").value=="") 
	     {
           document.getElementById("<%=txtName.ClientID%>").focus();
           alert("Name field can not be blank");
            return false;
         }
               
         }
    

else if (document.getElementById("<%=txtemail.ClientID %>").value!="")
{
            var emailPat = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            var emailid=document.getElementById("<%=txtemail.ClientID %>").value;
            if(emailPat.test(emailid) == false)
            {
              alert('Invalid Email Address!Please enter valid Email i.e.(abc@abc.com)');
              return false;
            }  
            else
           {
               if (state=='New'){if(confirm('Are you sure you want to save Department ?')==false)return false;}
               if (state=='Edit'){if(confirm('Are you sure you want to update Department ?')==false)return false;}
               if (state=='Delete'){if(confirm('Are you sure you want to delete Department ?')==false)return false;}
           }
}
    else
       {
//       alert(state);
       if (state=='New'){if(confirm('Are you sure you want to save Department ?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update Department ?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete Department ?')==false)return false;}
       }
}


function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}




</script>

<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        txtDeptnameAutoCompleteExtenderKeyUp();
       
    }
</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 750px; BORDER-BOTTOM: gray 2px solid; HEIGHT: 319px"><TBODY><TR><TD style="WIDTH: 750px" align=center colSpan=7>
    <asp:Label id="lblHeading" runat="server" Text="Add New Department Master" 
        CssClass="field_heading" Width="750px"></asp:Label></TD></TR>


<TR><TD style="WIDTH: 200px; HEIGHT: 15px" class="td_cell">Department Code<SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD style="WIDTH: 288px; COLOR: #000000; HEIGHT: 15px"><INPUT style="WIDTH: 277px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=10 runat="server" /></TD><TD class=" "></TD><TD></TD></TR><TR><TD style="WIDTH: 182px; HEIGHT: 8px" class="td_cell">Department Name<SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 288px; COLOR: #000000; HEIGHT: 8px"><INPUT style="WIDTH: 278px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD><TD class=" "></TD><TD></TD></TR>



<TR><TD class="td_cell">Show In Reservation</TD><TD style="WIDTH: 288px"><asp:DropDownList id="ddlShowinweb" tabIndex=3 runat="server" CssClass="field_input" Width="52px" __designer:wfdid="w5"><asp:ListItem>Yes</asp:ListItem>
<asp:ListItem>No</asp:ListItem>
</asp:DropDownList></TD><TD class=" "></TD><TD></TD></TR><TR><TD style="WIDTH: 182px; HEIGHT: 10px" class="td_cell">Email Address</TD><TD style="WIDTH: 288px; HEIGHT: 10px"><INPUT style="WIDTH: 277px" id="txtemail" class="field_input" tabIndex=4 type=text maxLength=100 runat="server" /></TD><TD class=" "></TD><TD></TD></TR>

 <tr>
                                                        <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label5" runat="server" Text="Department Head" Width="200px" ></asp:Label>
                                                                
                                                        </td>
                                                        <td align="left" valign="top" colspan="2" width="700px">
                                                            <asp:TextBox ID="txtDeptname" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="3" Width="200px"></asp:TextBox>
                                                            <asp:TextBox ID="txtDeptcode" runat="server" ></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField5" runat="server" />
                                                            <asp:AutoCompleteExtender ID="txtDeptname_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetDeptHead"
                                                                TargetControlID="txtDeptname" OnClientItemSelected="txtDeptnameautocompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>



<TR><TD class="td_cell">Phone</TD><TD style="WIDTH: 288px"><INPUT style="WIDTH: 197px" id="txtphone" class="field_input" tabIndex=7 onkeypress="return checkNumber()" type=text maxLength=20 runat="server" /></TD><TD class="td_cell"></TD><TD style="WIDTH: 262px"></TD></TR><TR><TD class="td_cell">Fax</TD><TD style="WIDTH: 288px"><INPUT style="WIDTH: 197px" id="txtfax" class="field_input" tabIndex=8 onkeypress="return checkNumber()" type=text maxLength=20 runat="server" /></TD><TD class=" "></TD><TD style="WIDTH: 262px"></TD></TR><TR><TD class="td_cell">URL</TD><TD style="WIDTH: 288px"><INPUT style="WIDTH: 197px" id="txturl" class="field_input" tabIndex=9 type=text maxLength=100 runat="server" /></TD><TD class=" "></TD><TD style="WIDTH: 262px"></TD></TR><TR><TD class="td_cell">Active</TD><TD style="WIDTH: 288px"><INPUT id="chkActive" tabIndex=10 type=checkbox CHECKED runat="server" /></TD><TD class=" "></TD><TD style="WIDTH: 262px"></TD></TR><TR><TD style="HEIGHT: 27px"><asp:Button id="btnSave" tabIndex=11 runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 288px; HEIGHT: 27px"><asp:Button id="btnCancel" tabIndex=12 runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnhelp" tabIndex=13 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" __designer:wfdid="w6"></asp:Button>&nbsp; </TD><TD style="HEIGHT: 27px" class=" "></TD><TD style="WIDTH: 262px"></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

