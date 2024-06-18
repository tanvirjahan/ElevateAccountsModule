<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Countries.aspx.vb" Inherits="Countries" %>

 <%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
   <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

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
           RegionAutoCompleteExtenderKeyUp();
           CurrencyAutoCompleteExtenderKeyUp();
       });

        </script>



        <script type="text/javascript" charset="utf-8">

            function currencyautocompleteselected(source, eventArgs) {
                if (eventArgs != null) {
                    document.getElementById('<%=Txtcurrencycode.ClientID%>').value = eventArgs.get_value();
                }
                else {
                    document.getElementById('<%=Txtcurrencycode.ClientID%>').value = '';
                }

            }


            function regionautocompleteselected(source, eventArgs) {
                if (eventArgs != null) {
                    document.getElementById('<%=TxtRegionCode.ClientID%>').value = eventArgs.get_value();
                }
                else {
                    document.getElementById('<%=TxtRegionCode.ClientID%>').value = '';
                }

            }

            function CurrencyAutoCompleteExtenderKeyUp() {
                $("#<%=TxtCurrencyname.ClientID %>").bind("change", function () {

                    document.getElementById('<%=Txtcurrencycode.ClientID%>').value = '';
                });
            }
            function RegionAutoCompleteExtenderKeyUp() {
                $("#<%=TxtRegionName.ClientID %>").bind("change", function () {
                    document.getElementById('<%=TxtRegionCode.ClientID%>').value = '';
                });
            }           

              </script>
  <script language="javascript" type="text/javascript">

      function checkCharacter(e) {
          if (event.keyCode == 32 || event.keyCode == 46)
              return;
          if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
              return false;
          }
      }
      function GetValueFrom() {

          var ddl = document.getElementById("<%=ddlCurName.ClientID%>");
          ddl.selectedIndex = -1;
          // Iterate through all dropdown items.
          for (i = 0; i < ddl.options.length; i++) {
              if (ddl.options[i].text ==
			document.getElementById("<%=ddlCurCode.ClientID%>").value) {
                  // Item was found, set the selected index.
                  ddl.selectedIndex = i;
                  return true;
              }
          }
      }
      function GetValueCode() {
          var ddl = document.getElementById("<%=ddlCurCode.ClientID%>");
          ddl.selectedIndex = -1;
          // Iterate through all dropdown items.
          for (i = 0; i < ddl.options.length; i++) {
              if (ddl.options[i].text ==
			document.getElementById("<%=ddlCurName.ClientID%>").value) {
                  // Item was found, set the selected index.
                  ddl.selectedIndex = i;
                  return true;
              }
          }
      }
      function GetValueFromMkt() {

          var ddl = document.getElementById("<%=ddlMktName.ClientID%>");
          ddl.selectedIndex = -1;
          // Iterate through all dropdown items.
          for (i = 0; i < ddl.options.length; i++) {
              if (ddl.options[i].text ==
			document.getElementById("<%=ddlMktCode.ClientID%>").value) {
                  // Item was found, set the selected index.
                  ddl.selectedIndex = i;
                  return true;
              }
          }
      }
      function GetValueCodeMkt() {
          var ddl = document.getElementById("<%=ddlMktCode.ClientID%>");
          ddl.selectedIndex = -1;
          // Iterate through all dropdown items.
          for (i = 0; i < ddl.options.length; i++) {
              if (ddl.options[i].text ==
			document.getElementById("<%=ddlMktName.ClientID%>").value) {
                  // Item was found, set the selected index.
                  ddl.selectedIndex = i;
                  return true;
              }
          }
      }


      function GetValueNCode() {

          var ddl = document.getElementById("<%=ddlNationalityName.ClientID%>");
          ddl.selectedIndex = -1;
          // Iterate through all dropdown items.
          for (i = 0; i < ddl.options.length; i++) {
              if (ddl.options[i].text ==
			document.getElementById("<%=ddlNationalityCode.ClientID%>").value) {
                  // Item was found, set the selected index.
                  ddl.selectedIndex = i;
                  return true;
              }
          }
      }
      function GetValueNName() {
          var ddl = document.getElementById("<%=ddlNationalityCode.ClientID%>");
          ddl.selectedIndex = -1;
          // Iterate through all dropdown items.
          for (i = 0; i < ddl.options.length; i++) {
              if (ddl.options[i].text ==
			document.getElementById("<%=ddlNationalityName.ClientID%>").value) {
                  // Item was found, set the selected index.
                  ddl.selectedIndex = i;
                  return true;
              }
          }
      }
    </script>
  <script language="javascript" type="text/javascript">

      function FormValidation(state) {
          //||(document.getElementById("<%=ddlWO2from.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlWO2to.ClientID%>").value=="[Select]")
          if ((document.getElementById("<%=txtcountrycode.ClientID%>").value == "") || (document.getElementById("<%=TxtCountryName.ClientID%>").value == "") || (document.getElementById("<%=Txtcurrencycode.ClientID%>").value == "") || (document.getElementById("<%=TxtRegionCode.ClientID%>").value == "")) {
              

              
              if (document.getElementById("<%=txtcountrycode.ClientID%>").value == "") {
                  document.getElementById("<%=txtcountrycode.ClientID%>").focus();
                  alert("Code field can not be blank");
                  return false;
              }
              else if (document.getElementById("<%=TxtCountryName.ClientID%>").value == "") {
                  document.getElementById("<%=TxtCountryName.ClientID%>").focus();
                  alert("Name field can not be blank");
                  return false;
              }

              else if (document.getElementById("<%=Txtcurrencycode.ClientID%>").value == "") {
                  document.getElementById("<%=Txtcurrencycode.ClientID%>").focus();
                  alert("Select Currency Code");
                  return false;
              }
              else if (document.getElementById("<%=TxtRegionCode.ClientID%>").value == "") {
                  document.getElementById("<%=TxtRegionCode.ClientID%>").focus();
                  alert("Select Region Code");
                  return false;
              }

          }
          else {
             
              if (document.getElementById("<%=txtcountrycode.ClientID%>").value != "") {
                  var val = document.getElementById("<%=txtcountrycode.ClientID%>").value;

                  if (!val.match(/^[a-zA-Z0-9\-\/\_]+$/)) {
                      alert('Only alphabets,digits,-,_,/ are allowed');
                      return false
                  }


                  return true;
              }

              if (state == 'New') { if (confirm('Are you sure you want to save Country type?') == false) return false; }
              if (state == 'Edit') { if (confirm('Are you sure you want to update Country type?') == false) return false; }
              if (state == 'Delete') { if (confirm('Are you sure you want to delete Country type?') == false) return false; }



          }
      }

   
   </script>  
    

<script type="text/javascript">

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);



    function EndRequestUserControl(sender, args) {


        CurrencyAutoCompleteExtenderKeyUp();
        regionAutoCompleteExtenderKeyUp();

        // after update occur on UpdatePanel re-init the Autocomplete

    }
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 700px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD style="HEIGHT: 11px" class="td_cell" align=center colSpan=4>
    <asp:Label id="lblHeading" runat="server" Text="Add New Country" 
        CssClass="field_heading" Width="100%"></asp:Label></TD></TR>
        <TR><TD style="WIDTH: 250px; HEIGHT: 14px" class="td_cell">Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
        <TD style="WIDTH: 272px; COLOR: #000000; HEIGHT: 14px">
<INPUT style="WIDTH: 172px" id="txtcountrycode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /></TD>
<TD style="WIDTH: 209px; COLOR: #000000; HEIGHT: 14px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 172px; COLOR: #000000; HEIGHT: 14px">
    <INPUT style="WIDTH: 172px" id="TxtCountryName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR>
<TR>
<TD style="WIDTH: 250px; height: 5px;display:none" class="td_cell">Currency&nbsp;Code<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 272px; HEIGHT: 8px;display:none">
<SELECT style="WIDTH: 178px" id="ddlCurCode" class="field_input" tabIndex=3 onchange="GetValueFrom()" runat="server" OnServerChange="ddlCurCode_ServerChange"> <OPTION selected></OPTION></SELECT>
</TD>
</TR>



                            
                                <tr>
                     <TD class="td_cell">Currency<span
style="color: #ff0000">*</span>
</td>
                    <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtCurrencyName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="172px"></asp:TextBox>
                            <asp:TextBox ID="Txtcurrencycode" runat="server" style="display:none"  ></asp:TextBox>
                            <asp:HiddenField ID="HiddenField1" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtCurrencyName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getcurrencylist" TargetControlID="TxtCurrencyName" OnClientItemSelected="currencyautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text4" class="field_input" type="text"
                             runat="server" />
                    </td>

                      <TD class="td_cell">Region Name<span
style="color: #ff0000">*</span>
</td>

  <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtRegionName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="176px"></asp:TextBox>
                           <asp:TextBox ID="TxtRegionCode" runat="server" style="display:none"  ></asp:TextBox>
                            <asp:HiddenField ID="HiddenField2" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtRegionName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="GetRegionlist" TargetControlID="TxtRegionName" OnClientItemSelected="regionautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text1" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text2" class="field_input" type="text"
                             runat="server" />
                    </td>






                   </tr>
                                         <TR>
                      <TD style="WIDTH: 250px; HEIGHT: 18px" align="left" class="td_cell">
                      <asp:Label ID="lblvisaremarks"  width="170px" Text ="Visa Remarks" runat ="server"></asp:Label>
                      </td>
                      <TD colspan="3">
        <asp:textbox id="txtvisaremarks"  TextMode ="MultiLine"    CssClass="field_input" tabIndex=12  width="500px" runat="server" ></asp:textbox> 
        
          
        </TD></TR>
                     
                      <TR>
                      <TD style="WIDTH: 250px; HEIGHT: 18px" align="left" class="td_cell">
        <INPUT id="chkActive" width="200px" tabIndex=12 type=checkbox CHECKED runat="server" />
        Active</TD><TD style="display:none">
            <INPUT id="chkpromotion" tabIndex=13 type=checkbox CHECKED runat="server" />
            Include&nbsp;in&nbsp;existing&nbsp;Promotions</TD><TD style="display:none">
            <INPUT id="chkbirdpromotion" tabIndex=14 type=checkbox 
                    CHECKED runat="server" Visible="false" />
            <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="8pt" 
                Text="Include in existing Early Bird Promotions" Visible="False"></asp:Label>
        </TD></TR>
                     
                     
                     
                     
                     
                         
                            
                          
                         
<tr>
<TD style="WIDTH: 211px; HEIGHT: 8px;display:none">
<SELECT style="WIDTH: 250px" id="ddlCurName" class="field_input" tabIndex=4 onchange="GetValueCode()" runat="server"> <OPTION selected>
</OPTION></SELECT>
</TD>
</TR>
<TR>
<TD style="WIDTH: 250px; HEIGHT: 5px;display:none" class="td_cell">Region &nbsp;Code<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 272px; HEIGHT: 5px;display:none">
<SELECT style="WIDTH: 178px" id="ddlMktCode" class="field_input" tabIndex=5 onchange="GetValueFromMkt()" runat="server"> <OPTION selected>
</OPTION>
</SELECT>
</TD>
</TR>
        
                  
                         
<tr><TD style="WIDTH: 211px; HEIGHT: 5px;display:none"><SELECT style="WIDTH: 250px" id="ddlMktName" class="field_input" tabIndex=6 onchange="GetValueCodeMkt()" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
<TR>
<TD style="WIDTH: 250px; height: 5px;display:none" class="td_cell">Nationality Code<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 272px; HEIGHT: 5px;display:none">
<select ID="ddlNationalityCode" runat="server" class="field_input" name="D1" onchange="GetValueNCode()" style="WIDTH: 178px" tabindex="7">
<option selected=""></option>
        </select>
        </TD>
    <td class="td_cell" style="WIDTH: 209px; HEIGHT: 5px;display:none">
        Nationality Name</td>
    <td style="WIDTH: 211px; HEIGHT: 5px;display:none">
        <select ID="ddlNationalityName" runat="server" class="field_input" name="D2" 
            onchange="GetValueNName()" style="WIDTH: 250px" tabindex="8">
            <option selected=""></option>
        </select></td>
    </TR><TR><TD style="WIDTH: 250px; " class="td_cell">
        &nbsp;</TD>
        <td colspan="3">
&nbsp;</td>
    </TR><TR style="display:none"><TD class="td_cell" colSpan=4 style="WIDTH: 84px; HEIGHT: 72px">
        <asp:Panel id="Panel1" runat="server" Height="50px" Width="577px" 
            GroupingText="Weekend Option 1 &lt;font color=&quot;Red&quot;&gt;*&lt;/Font&gt;"><TABLE><TBODY><TR>
                <TD style="WIDTH: 39px; height: 8px;">From</TD>
                <TD style="WIDTH: 126px; height: 8px;"><asp:DropDownList id="ddlWO1from" tabIndex=8 
                        runat="server" CssClass="field_input" Width="126px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem Value="Sunday">Sunday</asp:ListItem>
<asp:ListItem Value="Monday">Monday</asp:ListItem>
<asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
<asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
<asp:ListItem Value="Thursday">Thursday</asp:ListItem>
<asp:ListItem Value="Friday">Friday</asp:ListItem>
<asp:ListItem Value="Saturday">Saturday</asp:ListItem>
</asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD style="WIDTH: 46px; height: 8px;">To</TD>
                <TD style="WIDTH: 100px; height: 8px;"><asp:DropDownList id="ddlWO1to" tabIndex=9 
                        runat="server" CssClass="field_input" Width="126px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem Value="Sunday">Sunday</asp:ListItem>
<asp:ListItem Value="Monday">Monday</asp:ListItem>
<asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
<asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
<asp:ListItem Value="Thursday">Thursday</asp:ListItem>
<asp:ListItem Value="Friday">Friday</asp:ListItem>
<asp:ListItem Value="Saturday">Saturday</asp:ListItem>
</asp:DropDownList></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR style="display:none">
<TD class="td_cell" colspan="4">
    <asp:Panel ID="Panel2" runat="server" GroupingText="Weekend Option 2 " 
        Height="50px" Width="577px">
        <table>
            <tbody>
                <tr>
                    <td style="WIDTH: 39px">
                        From</td>
                    <td style="WIDTH: 126px">
                        <asp:DropDownList ID="ddlWO2from" runat="server" CssClass="field_input" 
                            tabIndex="10" Width="126px">
                            <asp:ListItem>[Select]</asp:ListItem>
                            <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                            <asp:ListItem Value="Monday">Monday</asp:ListItem>
                            <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="Friday">Friday</asp:ListItem>
                            <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="WIDTH: 46px">
                        To</td>
                    <td style="WIDTH: 100px">
                        <asp:DropDownList ID="ddlWO2to" runat="server" CssClass="field_input" 
                            tabIndex="11" Width="126px">
                            <asp:ListItem>[Select]</asp:ListItem>
                            <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                            <asp:ListItem Value="Monday">Monday</asp:ListItem>
                            <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                            <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                            <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                            <asp:ListItem Value="Friday">Friday</asp:ListItem>
                            <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
        </TD>
    </TR>
   

       
    <tr>
        <td style="WIDTH: 250px; HEIGHT: 2px">
            <asp:Button ID="btnSave" runat="server" CssClass="field_button" tabIndex="15" 
                Text="Save" />
        </td>
        <td style="WIDTH: 272px; HEIGHT: 2px">
            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" tabIndex="16" 
                Text="Return To Search" />
        </td>
        <td style="WIDTH: 209px; HEIGHT: 2px">
            <asp:Button ID="cmdhelp" runat="server" CssClass="field_button" 
                onclick="cmdhelp_Click" tabIndex="17" Text="Help" />
        </td>
        <td ><asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label>
        </td>
    </tr>
    <tr>
    <td></td>
    </tr>

     <tr>
<td style="width: 150px" align="left" colspan="2">
<asp:Button ID="btnCurrency" TabIndex="27" OnClick="btnCurrency_Click" runat="server" Text="Add New Currency" CssClass="field_button" Width="147px"></asp:Button>
<asp:Button ID="btnRegion" TabIndex="28" OnClick="btnRegion_Click"  runat="server" Text="Add New Region" CssClass="field_button" Width="147px"></asp:Button>
</td>
   </tr>


    
    </TBODY></TABLE>
     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>


    </asp:ScriptManagerProxy>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
 