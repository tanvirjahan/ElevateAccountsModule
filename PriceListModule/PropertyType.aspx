<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="PropertyType.aspx.vb" Inherits="PropertyType" %>

  
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %> <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script type="text/jscript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js">
    </script>
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
    
<script language ="javascript" type="text/javascript">
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

    function FormValidation(state) {

        if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtName.ClientID%>").value == "")) {
            if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }
        }




        else {
            if (document.getElementById("<%=txtCode.ClientID%>").value != "") {

                var val = document.getElementById("<%=txtCode.ClientID%>").value;
                if (!val.match(/^[a-zA-Z0-9\-\/\_]+$/)) {
                    alert('Only alphabets,digits,-,_,/ are allowed');
                    return false
                }
                return true
            }
            if (state == 'New') { if (confirm('Are you sure you want to save property type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update property type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete  propertytype type?') == false) return false; }
        }

    }

  

 

    

</script>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {

    }
</script>



<head>
<style >
   .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .8em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left:10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
          width: 150px !important;    
        }
        #divwidth div
       {
        width: 150px !important;   
       }

</style>
</head>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 680px; BORDER-BOTTOM: gray 2px solid">
<TBODY>
<TR>
<TD style="HEIGHT: 5px" class="td_cell" align=center colSpan=4>
<asp:Label id="lblHeading" runat="server" Text="Add New Cites" Width="100%" CssClass="field_heading"></asp:Label></TD>
</TR>
<TR>
<TD  class="td_cell">Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 37px; COLOR: #000000">
<INPUT  id="txtCode" class="field_input" style="width:196px"  tabIndex=1 type=text maxLength=20 runat="server" /> </TD>
<TD style="COLOR: #000000"></TD>
<TD style="WIDTH: 279px; COLOR: #000000"></TD>
</TR>















<TR>
<TD  class="td_cell">Name<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD><TD style="WIDTH: 37px; HEIGHT: 8px">
<INPUT style="WIDTH: 196px" id="txtName" class="txtbox" tabIndex=2 type=text maxLength=100 runat="server" /></TD>
<TD style="HEIGHT: 8px"></TD>
<TD style="HEIGHT: 8px"></TD>
</TR>


 










 
<TR>
<TD  class="td_cell">Active</TD>
<TD style="WIDTH: 37px">
<INPUT id="chkActive" tabIndex=6 type=checkbox CHECKED runat="server" /></TD>
<TD style="WIDTH: 855px"></TD><TD style="WIDTH: 279px"></TD>
</TR>
 
<TR>
<TD class="td_cell"><asp:Button id="btnSave" tabIndex=8 runat="server" Text="Save" CssClass="btn"></asp:Button></TD>
<TD style="WIDTH: 37px"><asp:Button id="btnCancel" tabIndex=9 onclick="btnCancel_Click" runat="server" Text="Return To Search" Width="196px" CssClass="btn"></asp:Button></TD>
<TD class="td_cell"><asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="btn" __designer:wfdid="w4"></asp:Button></TD>
<TD><asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></TD></TR>


 

</TBODY>



</TABLE>

<div ID="divwidth"></div>

<asp:TextBox ID="txtAutoComplete" runat="server" onkeyup = "SetContextKey()"  Width="252px" Visible="False"></asp:TextBox>   
<div ID="div1">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </services>
    </asp:ScriptManagerProxy>
            </div>
 
 

</contenttemplate>


    </asp:UpdatePanel>

     </asp:Content>

 