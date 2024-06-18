<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustAcctDet.aspx.vb" Inherits="CustAcctDet" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

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



           ContAccAutoCompleteExtenderKeyUp();
           BankAccAutoCompleteExtenderKeyUp();
           PostAccAutoCompleteExtenderKeyUp();
 
       });

        </script>

<script type="text/javascript">

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);



    function EndRequestUserControl(sender, args) {

 
        ContAccAutoCompleteExtenderKeyUp();
        BankAccAutoCompleteExtenderKeyUp();
        PostAccAutoCompleteExtenderKeyUp();
        // after update occur on UpdatePanel re-init the Autocomplete


        // after update occur on UpdatePanel re-init the Autocomplete

    }

   
    function ContAccAutoCompleteExtenderKeyUp() {
        $("#<%=TxtControlAccName.ClientID %>").bind("change", function () {

            document.getElementById('<%=TxtControlAccName.ClientID%>').value = '';
        });
    }

    function controlaccautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtControlAccCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtControlAccCode.ClientID%>').value = '';
        }

    }
    function BankAccAutoCompleteExtenderKeyUp() {
        $("#<%=TxtBankAccName.ClientID %>").bind("change", function () {

            document.getElementById('<%=TxtBankAccName.ClientID%>').value = '';
        });
    }

    function Bankaccautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtBankAccCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtBankAccCode.ClientID%>').value = '';
        }

    }
    function PostAccAutoCompleteExtenderKeyUp() {
        $("#<%=TxtPostAccName.ClientID %>").bind("change", function () {

            document.getElementById('<%=TxtPostAccName.ClientID%>').value = '';
        });
    }

    function postaccautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtPostAccCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtPostAccCode.ClientID%>').value = '';
        }

    }
    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

</script>

<script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >


        
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
			
		
function checkNumber(e)
{
    if ( event.keyCode < 45 || event.keyCode > 57 )
    {
    return false;
    }
}

function FormValidationMainDetail(state) {

    if ((document.getElementById("<%=TxtControlAccName.ClientID%>").value == "")||(document.getElementById("<%=TxtControlAccCode.ClientID%>").value == ""))
    {
      alert("Select Main A/C Code.");
      document.getElementById("<%=TxtControlAccName.ClientID%>").focus();
      return false;
  }
  else if ((document.getElementById("<%=TxtBankAccName.ClientID%>").value == "") || (document.getElementById("<%=TxtBankAccCode.ClientID%>").value == "")) {
      alert("Select Bank A/c code.");
      document.getElementById("<%=TxtBankAccName.ClientID%>").focus();
      return false;
  }
  else if (document.getElementById("<%=txtAccTelephone1.ClientID%>").value == "") {
      document.getElementById("<%=txtAccTelephone1.ClientID%>").focus();
      alert("Telephone fieldcannot be blank");
      return false;
  }
     else
       {
       if (state=='New'){if(confirm('Are you sure you want to save customer account details?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update customer account details?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete customer account details?')==false)return false;}
       }

}




</script>

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; 
    BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left>
<TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" Width="800px" CssClass="field_heading" ForeColor="White"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; 
    <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=1 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD style="WIDTH: 85%" vAlign=top>
        <DIV style="WIDTH: 706px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelAccounts" runat="server" Width="669px" 
        GroupingText="Accounts  Details" style="margin-right: 0px"><TABLE><TBODY><TR>
<TD style="WIDTH: 160px; HEIGHT: 15px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Telephone</SPAN>
    &nbsp; <span class="td_cell" style="COLOR: #ff0000">*&nbsp;</span>
</TD><TD style="WIDTH: 337px; HEIGHT: 15px" align=left>
            <INPUT style="WIDTH: 430px" id="txtAccTelephone1" class="field_input" tabIndex=2 type=text maxLength=50 runat="server" /></TD>
<TD style="HEIGHT: 15px" align=left></TD></TR><TR><TD style="WIDTH: 160px" align=left></TD><TD style="WIDTH: 337px" align=left>
<INPUT style="WIDTH: 430px" id="txtAccTelephone2" class="field_input" tabIndex=3 type=text maxLength=50 runat="server" /></TD>
        <TD align=left></TD></TR>
<TR><TD style="WIDTH: 160px" align=left>
<SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
  Mobile No.</SPAN></TD><TD style="WIDTH: 337px" align=left>
        <INPUT style="WIDTH: 430px" id="txtAccMobile" class="field_input" tabIndex=4 type=text maxLength=50 runat="server" /></TD>
<TD align=left></TD></TR><TR><TD style="WIDTH: 160px" class="td_cell" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
Fax</SPAN></TD><TD style="WIDTH: 337px" align=left>
            <INPUT style="WIDTH: 430px" id="txtAccFax" class="field_input" tabIndex=5 type=text maxLength=50 runat="server" /></TD>
<TD align=left></TD></TR><TR><TD style="WIDTH: 160px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
  Contact</SPAN></TD><TD style="WIDTH: 337px" align=left>
            <INPUT style="WIDTH: 430px" id="txtAccContact1" class="field_input" tabIndex=6 type=text maxLength=100 runat="server" /></TD>
<TD align=left></TD></TR><TR><TD style="WIDTH: 160px" align=left></TD><TD style="WIDTH: 337px" align=left>
        <INPUT style="WIDTH: 430px" id="txtAccContact2" class="field_input" tabIndex=7 type=text maxLength=100 runat="server" /></TD>
<TD align=left></TD></TR><TR><TD style="WIDTH: 160px" 
        align=left>&nbsp;</TD><TD style="WIDTH: 337px" align=left>
        <INPUT style="WIDTH: 430px" id="txtAccContact3" class="field_input" tabIndex=8 type=text maxLength=100 runat="server" /></TD>
<TD align=left>&nbsp;</TD></TR>
    <tr>
        <td align="left" style="WIDTH: 160px">
            &nbsp;</td>
        <td align="left" style="WIDTH: 337px">
            <INPUT style="WIDTH: 430px" id="txtAccContact4" class="field_input" tabIndex=9 
                    type=text maxLength=100 runat="server" />
        </td>
        <td align="left">
            &nbsp;</td>
    </tr>
    <TR><TD style="WIDTH: 160px" class="td_cell" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
 E-Mail</SPAN></TD><TD style="WIDTH: 550px" align=left>
        <INPUT style="WIDTH: 430px" id="txtAccEmail" class="field_input" tabIndex=10 type=text maxLength=100 runat="server" /></TD>
<TD align=left></TD></TR>
<TR><TD style="WIDTH: 160px" align=left class="td_cell"> <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
 CC-Mail</SPAN></TD><TD align=left 
        style="WIDTH: 550px">
    <INPUT style="WIDTH: 430px" id="txtAcc_ccEmail" 
                    class="field_input" tabIndex=11 type=text maxLength=500 runat="server" />
    </TD>
    <td align="left">
    </td>
    </TR>


<tr>
<TD style="WIDTH: 160px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">
    Control A/C Code <span class="td_cell" style="COLOR: #ff0000">*</span>  </SPAN></SPAN></TD>
   <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtControlAccName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="297px" 
                            Rows="12"></asp:TextBox>
                            <asp:TextBox ID="TxtControlAccCode" runat="server" style="display:none"  ></asp:TextBox>
                            <asp:HiddenField ID="hdncontacccode" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtControlAccName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getcontrolacclist" TargetControlID="TxtControlAccName" OnClientItemSelected="controlaccautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text15" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text16" class="field_input" type="text"
                             runat="server" />
                    </td>
            <%--            <td align="left" style="WIDTH: 61px">
               
                            </td>--%>
                        </tr>
                        <tr>
<TD style="WIDTH: 160px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">
    Select Bank details to be printed in Invoice <span class="td_cell" style="COLOR: #ff0000">*</span>  </SPAN></SPAN></TD>
   <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtBankAccName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="13" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtBankAccCode" runat="server" style="display:none" ></asp:TextBox>
                            <asp:HiddenField ID="hdnbankacccode" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtBankAccName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getbankacclist" TargetControlID="TxtBankAccName" OnClientItemSelected="Bankaccautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text1" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text2" class="field_input" type="text"
                             runat="server" />
                    </td>
            <%--            <td align="left" style="WIDTH: 61px">
               
                            </td>--%>
                        </tr>
        <TR><TD style="WIDTH: 160px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
        Credit Days</SPAN>
 </td>
 <td>
     <INPUT id="TxtAccCreditDays" tabIndex=15 type=text runat="server" class="field_input" maxLength="5" style="WIDTH: 172px; TEXT-ALIGN: right" />
     &nbsp;&nbsp;<%--&nbsp;&nbsp;&nbsp;--%><%--</TD>--%><%--<TD style="WIDTH: 550px" align=left>--%><span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Credit Limit</span>&nbsp;&nbsp;<INPUT 
            style="WIDTH: 176px; TEXT-ALIGN: right" id="txtAccCreditLimit" 
            class="field_input" tabIndex=16 type=text runat="server" /></TD>
       <%-- <TD align=left></TD>--%></TR><TR><TD style="WIDTH: 160px" align=left>
    <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Cash Customer</span></TD>
    <TD style="WIDTH: 337px" align=left>
        <INPUT id="ChkCashSup" tabIndex=17 type=checkbox runat="server" /></TD>
 <TD align=left></TD></TR><TR><TD style="WIDTH: 160px; " 
        align=left><SPAN style="FONT-SIZE: 8pt; ">
    <span style="FONT-FAMILY: Arial">Booking Credit Limit
    </span></SPAN></TD><TD style="width: 550px;" align=left>
        <INPUT style="WIDTH: 175px; TEXT-ALIGN: right" id="txtAccBooking" 
                    class="field_input" tabIndex=18 type=text runat="server" />
    </TD>
    <td align="left">
    </td>
    </TR>
    <tr>
<TD style="WIDTH: 160px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">
Post Account 
<%--<span class="td_cell" style="COLOR: #ff0000">*</span>  --%>
</SPAN></SPAN></TD>
  <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtPostAccName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="19" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtPostAccCode" runat="server" style="display:none" ></asp:TextBox>
                            <asp:HiddenField ID="hdnpostacccode" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtPostAccName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getpostacclist" TargetControlID="TxtPostAccName" OnClientItemSelected="postaccautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text4" class="field_input" type="text"
                             runat="server" />
                    </td>
            <%--            <td align="left" style="WIDTH: 61px">
               
                            </td>--%>
                        </tr>
                      
                             
 <TR>
     <TD style="width: 160px;" align=left> <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
   Remarks</SPAN> </TD>
     <td align="left" style="WIDTH: 337px">
         <INPUT style="WIDTH: 427px" id="txtremarks" class="field_input" tabIndex=20 
             type=text runat="server" maxLength="100" />
     </td>
     <TD align=left> </TD>
                            </TR>
                            <TR>
                                <TD align=left style="WIDTH: 160px"> <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
   Client Opinion</SPAN></TD><TD style="WIDTH: 337px" align=left>
    <INPUT id="txtclientop" tabIndex=21 type=text runat="server" class="field_input" style="WIDTH: 426px" /> </TD>
 <TD align=left></TD></TR>
 <TR><TD style="WIDTH: 160px" align=left></TD>
  <TD align=left style="WIDTH: 337px">
      <INPUT id="ChkAccBooking2" tabIndex=22 
                type=checkbox runat="server" />
                <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">
   Booking credit limits need to bechecked or not</SPAN>
     </TD><TD align=left>
     </TD>
                            </TR>
 <TR><TD style="WIDTH: 160px" align=left>
     <asp:Button ID="BtnAccSave" runat="server" CssClass="field_button" 
         tabIndex="23" Text="Save" Width="78px" />
     </TD><TD style="WIDTH: 337px" align=left>
         <asp:Button ID="BtnAccCancel" runat="server" CssClass="field_button" 
             onclick="BtnAccCancel_Click" tabIndex="24" Text="Return to Search" />
         &nbsp;
         <asp:Button ID="btnHelp" runat="server" CssClass="field_button" 
             onclick="btnHelp_Click" tabIndex="25" Text="Help" />
     </TD><TD align=left>
         <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
             Text="Webserviceerror"></asp:Label>
     </TD></TR>

    </TBODY></TABLE></asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

