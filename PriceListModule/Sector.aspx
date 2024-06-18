<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Sector.aspx.vb" Inherits="Sector"  %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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
            countryAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            sectorGroupAutoCompleteExtenderKeyUp();
        });
</script>
<script language ="javascript" type="text/javascript" >

    function CallWebMethod(methodType) {
        switch (methodType) {

            case "othtypcode":
                var select = document.getElementById("<%=ddlSectorGroupCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSectorGroupName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value;



                ColServices.clsServices.GetCtryCodeCategoryListnew(constr, codeid, FillCountryCodes, ErrorHandler, TimeOutHandler);






                break;
            case "othtypname":
                var select = document.getElementById("<%=ddlSectorGroupName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSectorGroupCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCtryCodeCategoryListnew(constr, codeid, FillCountryCodes, ErrorHandler, TimeOutHandler);
                break;
            case "ctrycode":
                var select = document.getElementById("<%=ddlCountryCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                ColServices.clsServices.GetCityCodeListnew(constr, codeid, FillCityCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr, codeid, FillCityNames, ErrorHandler, TimeOutHandler);
                break;
            case "ctryname":
                var select = document.getElementById("<%=ddlCountryName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlCountryCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCityCodeListnew(constr, codeid, FillCityCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr, codeid, FillCityNames, ErrorHandler, TimeOutHandler);
                break;
            case "citycode":
                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlCityName.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                selectname.value = select.options[select.selectedIndex].text;
                // ColServices.clsServices.GetCtyCountryCodeListnew(constr,codeid,FillCtryCode,ErrorHandler,TimeOutHandler); 
                //ColServices.clsServices.GetCtyCountryNameListnew(constr,selectname,FillCtryName,ErrorHandler,TimeOutHandler);
            case "cityname":
                var select = document.getElementById("<%=ddlCityName.ClientID%>");
                var selectname = document.getElementById("<%=ddlCityCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                //  ColServices.clsServices.GetCtyCountryNameListnew(constr,codeid,FillCtryName,ErrorHandler,TimeOutHandler);
                //ColServices.clsServices.GetCtyCountryCodeListnew(constr,selectname,FillCtryCode,ErrorHandler,TimeOutHandler);

        }
    }


    function FillCityCodes(result) {
        var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCityNames(result) {
        var ddl = document.getElementById("<%=ddlCityName.ClientID%>");

        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }


    function FillCtryCode(result) {
        var ddl = document.getElementById("<%=ddlCountryCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        //       ddl.value = "[Select]";
    }



    function FillCtryName(result) {
        var ddl = document.getElementById("<%=ddlCountryName.ClientID%>");

        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        //        ddl.value = "[Select]";
    }

    function FillCountryCodes(result) {

        var ddlCtryName = document.getElementById("<%=ddlCountryName.ClientID%>");
        var ddlCtryCode = document.getElementById("<%=ddlCountryCode.ClientID%>");
        var ddlCityCode = document.getElementById("<%=ddlCityCode.ClientID%>");
        var ddlCityName = document.getElementById("<%=ddlCityName.ClientID%>");

        if (result[0].ListValue != '') {


            ddlCityName.value = result[0].ListValue;
            ddlCtryName.value = result[0].ListText;

            for (var i = 0; i < ddlCtryCode.length - 1; i++) {

                if (ddlCtryCode.options[i].text == result[0].ListText) {
                    ddlCtryCode.selectedIndex = i;
                    break;
                }
            }
            for (var i = 0; i < ddlCityCode.length - 1; i++) {

                if (ddlCityCode.options[i].text == result[0].ListValue) {
                    ddlCityCode.selectedIndex = i;
                    break;
                }
            }

        }
        else {


            ddlCtryName.value = "[Select]"
            ddlCtryCode.value = "[Select]"
            ddlCityCode.value = "[Select]"
            ddlCityName.value = "[Select]"
        }

        ddlCtryName.disabled = true;
        ddlCtryCode.disabled = true;
        ddlCityCode.disabled = true;
        ddlCityName.disabled = true;

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
    function FormValidation(state) {
        if ((document.getElementById("<%=txtSectorCode.ClientID%>").value == "") || (document.getElementById("<%=txtSectorName.ClientID%>").value == "") || (document.getElementById("<%=txtcountrycode.ClientID%>").value == "") || (document.getElementById("<%=txtSectorGroupCode.ClientID%>").value == "") || (document.getElementById("<%=txtcitycode.ClientID%>").value == "")) {
            if (document.getElementById("<%=txtSectorCode.ClientID%>").value == "") {
                document.getElementById("<%=txtSectorCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtSectorName.ClientID%>").value == "") {
                document.getElementById("<%=txtSectorName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtSectorGroupCode.ClientID%>").value == "") {
                document.getElementById("<%=txtSectorGroupCode.ClientID%>").focus();
                alert("Select Sector Group Code");
                return false;
            }
            else if (document.getElementById("<%=txtcountrycode.ClientID%>").value == "") {
                document.getElementById("<%=txtcountrycode.ClientID%>").focus();
                alert("Select Country Code");
                return false;
            }

            else if (document.getElementById("<%=txtcitycode.ClientID%>").value == "") {
                document.getElementById("<%=txtcitycode.ClientID%>").focus();
                alert("Select City Code");
                return false;
            }
        }
        else {
            if (document.getElementById("<%=txtSectorCode.ClientID%>").value != "") {

                var val = document.getElementById("<%=txtSectorCode.ClientID%>").value;
                if (!val.match(/^[a-zA-Z0-9\-\/\_]+$/)) {
                    alert('Only alphabets,digits,-,_,/ are allowed');
                    return false
                }
                return true;
            }

            if (state == 'New') { if (confirm('Are you sure you want to save Sector type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Sector type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Sector type?') == false) return false; }
        }
    }
    function countryautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
        }
    }



    function cityautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcitycode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcitycode.ClientID%>').value = '';
        }
    }


    function sectorgroupautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtSectorGroupCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtSectorGroupCode.ClientID%>').value = '';
        }
    }

    function countryAutoCompleteExtenderKeyUp() {
        $("#<%=txtcountryname.ClientID %>").bind("change", function () {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
        });
    }

    function cityAutoCompleteExtenderKeyUp() {
        $("#<%=txtcityname.ClientID %>").bind("change", function () {
            document.getElementById('<%=txtcitycode.ClientID%>').value = '';
        });
    }
    function sectorGroupAutoCompleteExtenderKeyUp() {
        $("#<%=txtsectorgroupname.ClientID %>").bind("change", function () {
            document.getElementById('<%=txtSectorGroupCode.ClientID%>').value = '';
        });
    }
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
            countryAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            sectorGroupAutoCompleteExtenderKeyUp();
        }
    </script>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
 <ContentTemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 742px; BORDER-BOTTOM: gray 2px solid">
<TBODY>
<TR>
<TD class="td_cell" align=center colSpan=4>
<asp:Label id="lblHeading" runat="server" Text="Add New Sector" Width="100%" CssClass="field_heading"></asp:Label></TD>
</TR>
<TR style="COLOR: #ff0000">
<TD style="WIDTH: 176px" class="td_cell">
<SPAN style="COLOR: #000000">Sector Code</SPAN><SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
<TD style="COLOR: #000000">
<INPUT id="txtSectorCode" style="width:250px" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> </TD>
</TR>
<TR>
<TD class="td_cell">Sector Name<SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
<TD style="COLOR: #000000">
<INPUT  id="txtSectorName" style="width:250px" class="field_input" tabIndex=2 type=text maxLength=150 runat="server" /> 
</TD>
</TR>
<tr>
<TD class="td_cell">Sector Group<span
style="color: #ff0000">*</span>
</td>
<td align="left" valign="top" colspan="2" width="300px">
<asp:TextBox ID="txtSectorGroupName" runat="server" AutoPostBack="True" CssClass="field_input"
MaxLength="500" TabIndex="3" Width="250px"></asp:TextBox>
<asp:TextBox ID="txtSectorGroupCode" runat="server" style="display:none"></asp:TextBox>
<asp:HiddenField ID="hdnsectorGroup" runat="server" />
<asp:AutoCompleteExtender ID="Sector_AutoCompleteExtender" runat="server" CompletionInterval="10"
CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
ServiceMethod="Getsectorgrouplist" TargetControlID="txtSectorGroupName" OnClientItemSelected="sectorgroupautocompleteselected">
</asp:AutoCompleteExtender>
<input style="display: none" id="Text11" class="field_input" type="text" runat="server" />
<input style="display: none" id="Text12" class="field_input" type="text" runat="server" />
</td>
</tr>

<tr>
<TD class="td_cell">Country<span
style="color: #ff0000">*</span>
</td>
<td align="left" valign="top" colspan="2" width="300px">
<asp:TextBox ID="txtcountryname" runat="server" AutoPostBack="True" CssClass="field_input"
MaxLength="500" TabIndex="3" Width="250px"></asp:TextBox>
<asp:TextBox ID="txtcountrycode" runat="server" style="display:none" ></asp:TextBox>
<asp:HiddenField ID="hdncountry" runat="server" />
<asp:AutoCompleteExtender ID="Country_AutoCompleteExtender" runat="server" CompletionInterval="10"
CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
ServiceMethod="Getcountrylist" TargetControlID="txtcountryname" OnClientItemSelected="countryautocompleteselected">
</asp:AutoCompleteExtender>
<input style="display: none" id="Text5" class="field_input" type="text" runat="server" />
<input style="display: none" id="Text6" class="field_input" type="text" runat="server" />
</td>
</tr>

 <tr>
<TD class="td_cell">City<span
style="color: #ff0000">*</span>
</td>
<td align="left" valign="top" colspan="2" width="300px">
<asp:TextBox ID="txtcityname" runat="server" AutoPostBack="True" CssClass="field_input"
MaxLength="500" TabIndex="3" Width="250px"></asp:TextBox>
<asp:TextBox ID="txtcitycode" runat="server" style="display:none"></asp:TextBox>
<asp:HiddenField ID="hdncity" runat="server" />
<asp:AutoCompleteExtender ID="City_AutoCompleteExtender" runat="server" CompletionInterval="10"
CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
ServiceMethod="Getcitylist" TargetControlID="txtcityname" OnClientItemSelected="cityautocompleteselected">
</asp:AutoCompleteExtender>
<input style="display: none" id="Text9" class="field_input" type="text" runat="server" />
<input style="display: none" id="Text10" class="field_input" type="text" runat="server" />
</td>
</tr>
<TR style="display:none">
<TD  class="td_cell">Sector Group Code&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD  class="td_cell" rowSpan=1>
<SELECT  id="ddlSectorGroupCode" class="field_input" tabindex=3 onchange="CallWebMethod('othtypcode');" runat="server">
 <OPTION selected></OPTION>
 </SELECT>
 </TD>
 <TD  class="td_cell" >Sector Group Name</TD>
 <TD  class="td_cell" >
 <SELECT style="WIDTH: 306px" id="ddlSectorGroupName" class="field_input" tabIndex=4 onchange="CallWebMethod('othtypname');" runat="server"> <OPTION selected></OPTION></SELECT>
 </TD>
 </TR>
<TR style="display:none">
<TD  class="td_cell">Country Code&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD  class="td_cell" rowSpan=1>
<SELECT id="ddlCountryCode" class="field_input" tabIndex=3 onchange="CallWebMethod('ctrycode');" runat="server"> 
<OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 168px" class="td_cell" rowSpan=1>Country Name</TD>
<TD  class="td_cell" rowSpan=1>
<SELECT  id="ddlCountryName" class="field_input" tabIndex=4 onchange="CallWebMethod('ctryname');" runat="server"> 
<OPTION selected></OPTION></SELECT>
</TD>
</TR>
<TR style="display:none">
<TD  class="td_cell">City&nbsp; Code <SPAN style="COLOR: red" class="td_cell">*</SPAN>
</TD>
<TD  class="td_cell">
<SELECT style="WIDTH: 202px" id="ddlCityCode" class="field_input" tabIndex=5 onchange="CallWebMethod('citycode');" runat="server"> <OPTION selected></OPTION></SELECT>
</TD>
<TD  class="td_cell">City Name</TD>
<TD  class="td_cell">
<SELECT  id="ddlCityName" class="field_input" tabIndex=6 onchange="CallWebMethod('cityname');" runat="server">
 <OPTION selected>
 </OPTION>
 </SELECT>
 </TD>
 </TR>
 <TR>
 <TD  class="td_cell">Active</TD>
 <TD>
 <INPUT id="chkActive" tabIndex=7 type=checkbox CHECKED runat="server" />
 <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;height: 9px" type="text" />
        </TD>
        </TR>
        <tr>
        <td class="td_cell" style ="float:left">Show in Web</td>
        <td> <INPUT id="chkshow" tabIndex=8 type=checkbox CHECKED runat="server" /></td>
        </tr>
        <TR>
        <TD>
        <asp:Button id="btnSave" tabIndex=8 runat="server" Text="Save" CssClass="field_button"></asp:Button>
        </TD>
<TD>
            <asp:Button id="btnCancel" tabIndex=9 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp; 
            <asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" 
                Text="Help" CssClass="field_button"></asp:Button></TD>
                </TR>

                <tr>
                 <td style="width: 150px" align="left">
        <asp:Button ID="btnCountry" TabIndex="27" OnClick="btnCountry_Click" runat="server" Text="Add New Country" CssClass="field_button" Width="147px"></asp:Button></td>
                      <td style="width: 150px" align="left" colspan="2">
                                                                            <asp:Button ID="btnCity" TabIndex="27" OnClick="btnCity_Click" 
                                                                                runat="server" Text="Add New City"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                
                
                
                </tr>
                </TBODY>
                </TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</ContentTemplate>
    </asp:UpdatePanel>
   
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
    </asp:ScriptManagerProxy>
    
</asp:Content>



 