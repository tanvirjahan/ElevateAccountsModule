<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="UserMaster.aspx.vb" Inherits="UserMaster" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ register assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
        namespace="System.Web.UI" tagprefix="asp" %>
    <%@ outputcache location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
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

            DeptAutoCompleteExtenderKeyUp();
            UserGrpAutoCompleteExtenderKeyUp;
        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);



        function EndRequestUserControl(sender, args) {


            DeptAutoCompleteExtenderKeyUp();
            UserGrpAutoCompleteExtenderKeyUp;


            // after update occur on UpdatePanel re-init the Autocomplete

        }
    </script>

 
        <script language="javascript" type="text/javascript">
            function PopUpImageView(code) {

                var FileName = document.getElementById("<%=hdnFileName.ClientID%>");
                var lblfilename = document.getElementById("<%=txtimg.ClientID%>");



                if (FileName.value == "") {
                    FileName.value = code;
                }

                if (lblfilename != "") {

                    popWin = open('../PriceListModule/ImageViewWindow.aspx?code=' + FileName.value + " &pagename=" + "UserMaster.aspx", 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                    popWin.focus();
                    FileName.value = "";
                    return false

                }
                else {

                    popWin = open('../PriceListModule/ImageViewWindow.aspx?', 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                    popWin.focus();
                }
            }

            function txtimage() {
                document.getElementById("<%=txtSmall.ClientID%>").value = document.getElementById("<%=FileUpload1.ClientID%>").value;
            }

            function CallWebMethod(methodType) {
                switch (methodType) {

                
                }
            }
        

            function deptautocompleteselected(source, eventArgs) {
                if (eventArgs != null) {
                    document.getElementById('<%=txtdeptcode.ClientID%>').value = eventArgs.get_value();
                }
                else {
                    document.getElementById('<%=txtdeptcode.ClientID%>').value = '';
                }

            }

            function DeptAutoCompleteExtenderKeyUp() {
                $("#<%= txtdeptname.ClientID %>").bind("change", function () {

                    if (document.getElementById('<%=txtdeptname.ClientID%>').value == '') {

                        document.getElementById('<%=txtdeptcode.ClientID%>').value = '';
                    }

                });

                $("#<%= txtdeptname.ClientID %>").keyup(function (event) {

                    if (document.getElementById('<%=txtdeptname.ClientID%>').value == '') {

                        document.getElementById('<%=txtdeptcode.ClientID%>').value = '';
                    }

                });
            }

            function usergrpautocompleteselected(source, eventArgs) {
                if (eventArgs != null) {
                    document.getElementById('<%=txtusergrpcode.ClientID%>').value = eventArgs.get_value();
                }
                else {
                    document.getElementById('<%=txtusergrpcode.ClientID%>').value = '';
                }

            }

            function UserGrpAutoCompleteExtenderKeyUp() {
                $("#<%= txtusergrpname.ClientID %>").bind("change", function () {

                    if (document.getElementById('<%=txtusergrpname.ClientID%>').value == '') {

                        document.getElementById('<%=txtusergrpcode.ClientID%>').value = '';
                    }

                });

                $("#<%= txtusergrpname.ClientID %>").keyup(function (event) {

                    if (document.getElementById('<%=txtusergrpname.ClientID%>').value == '') {

                        document.getElementById('<%=txtusergrpcode.ClientID%>').value = '';
                    }

                });
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
                if ((document.getElementById("<%=txtUserCode.ClientID%>").value == "") || (document.getElementById("<%=txtUserCode.ClientID%>").value <= 0) || (document.getElementById("<%=TxtUserName.ClientID%>").value == "")  || (document.getElementById("<%=txtusergrpcode.ClientID%>").value == "[Select]")) {
                    if (document.getElementById("<%=txtUserCode.ClientID%>").value == "") {
                        document.getElementById("<%=txtUserCode.ClientID%>").focus();
                        alert("Code field can not be blank.");
                        return false;
                    }
                    else if (document.getElementById("<%=txtUserCode.ClientID%>").value <= 0) {
                        alert("Code must be greater than zero.");
                        document.getElementById("<%=txtUserCode.ClientID%>").focus();
                        return false;
                    }
                    else if (document.getElementById("<%=TxtUserName.ClientID%>").value == "") {
                        document.getElementById("<%=TxtUserName.ClientID%>").focus();
                        alert("Name field can not be blank.");
                        return false;
                    }
                    else if (document.getElementById("<%=txtdeptcode.ClientID%>").value == "") {
                        document.getElementById("<%=txtdeptcode.ClientID%>").focus();
                        alert("Select Department Code.");
                        return false;
                    }
                    else if (document.getElementById("<%=txtusergrpcode.ClientID%>").value == "[Select]") {
                        document.getElementById("<%=txtusergrpcode.ClientID%>").focus();
                        alert("Select User Group.");
                        return false;
                    }

                }
                else if (document.getElementById("<%=TxtEmailID.ClientID %>").value != "") {
                    var emailPat = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                    var emailid = document.getElementById("<%=TxtEmailID.ClientID %>").value;
                    if (emailPat.test(emailid) == false) {
                        document.getElementById("<%=TxtEmailID.ClientID %>").focus();
                        alert('Invalid Email Address!Please enter valid Email i.e.(abc@abc.com).');
                        return false;
                    }
                    else {
                        if (state == 'New') { if (confirm('Are you sure you want to save User ?') == false) return false; }
                        if (state == 'Edit') { if (confirm('Are you sure you want to update User ?') == false) return false; }
                        if (state == 'Delete') { if (confirm('Are you sure you want to delete User ?') == false) return false; }
                    }
                }
                else {
                    //       alert(state);
                    if (state == 'New') { if (confirm('Are you sure you want to save User ?') == false) return false; }
                    if (state == 'Edit') { if (confirm('Are you sure you want to update User ?') == false) return false; }
                    if (state == 'Delete') { if (confirm('Are you sure you want to delete User ?') == false) return false; }
                }
            }


            function checkNumber(e) {

                if ((event.keyCode < 47 || event.keyCode > 57)) {
                    return false;
                }

            }

        </script>
          <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
        <td>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <%--  <Triggers>
        <asp:PostBackTrigger ControlID="btnSave"/>
    </Triggers>--%>
            <ContentTemplate>
                <table style="width: 100%; height: 203px;">
                    <tbody>
                        <tr>
                            <td style="text-align: center" colspan="4">
                                <asp:Label ID="lblHeading" runat="server" Text="Add User Master" CssClass="field_heading"
                                    Width="100%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 98px" >
                                <span style="font-size: 8pt; font-family: Arial">User Code <span style="color: #ff0066">
                                    *</span></span>
                            </td>
                            <td style="width: 267px"  >
                                <input style="width: 195px" id="txtUserCode" class="field_input" type="text" maxlength="10"
                                    runat="server"  TabIndex="1" />
                            </td>
                            <td style="width: 124px" >
                                <span style="font-size: 8pt; font-family: Arial">User Name <span style="color: #ff0066">
                                    *</span></span>
                            </td>
                            <td >
                                <input style="width: 195px" id="TxtUserName" class="field_input" type="text" maxlength="40"
                                    runat="server" TabIndex="2" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 98px" >
                                <span style="font-size: 8pt; font-family: Arial">Password <span style="color: #ff0000">
                                    *</span></span>
                            </td>
                            <td style="width: 267px" >
                                <asp:TextBox ID="TxtPassword" runat="server" CssClass="field_input" Width="195px"
                                    MaxLength="10" TextMode="Password" __designer:wfdid="w36" TabIndex="3" ></asp:TextBox>
                            </td>
                            <td 
                                <span style="font-size: 8pt; font-family: Arial; width: 124px;">Re-enter Password </span>
                            </td>
                            <td >
                                <asp:TextBox ID="TxtRPassword" runat="server" CssClass="field_input" Width="193px"
                                    MaxLength="10" TextMode="Password" __designer:wfdid="w37" TabIndex="4" ></asp:TextBox>
                            </td>
                        </tr>
                     
                           <tr>
                          <td style=" height: 24px; width: 98px;">
                                <span style="font-size: 8pt; font-family: Arial">Department <span style="color: #ff0000">
                                    *</span></span>
                            </td>
                    <td style="color: #000000; width: 267px;">
                        <asp:TextBox ID="TxtdeptName" runat="server"  
                            CssClass="field_input" MaxLength="500" TabIndex="5" Width="195px"
                            Height="16px"></asp:TextBox>
                            <asp:TextBox ID="TxtdeptCode" runat="server"   style="display:none"  ></asp:TextBox>
                        <asp:AutoCompleteExtender ID="TxtdeptName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getdeptslist" TargetControlID="TxtdeptName" OnClientItemSelected="deptautocompleteselected" >
                        </asp:AutoCompleteExtender>
           
                    </td>
</tr>
                        <tr>
                            <td style=" height: 1px; width: 98px;">
                                <span style="font-size: 8pt; font-family: Arial">Designation</span>
                            </td>
                            <td style=" height: 1px; width: 267px;">
                                <input style="width: 195px" id="TxtDesignation" TabIndex="6" class="field_input" type="text" maxlength="40"
                                    runat="server" />
                            </td>
                            <td style=" height: 1px; width: 124px;">
                            </td>
                            <td style=" height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 98px" >
                                <span style="font-size: 8pt; font-family: Arial">Email ID</span>
                            </td>
                            <td style="width: 267px" > 
                                <input style="width: 195px" id="TxtEmailID" TabIndex="7" class="field_input" type="text" maxlength="100"
                                    runat="server" />
                            </td>
                            <td style="width: 124px" >
                            </td>
                            <td >
                            </td>
                        </tr>
                      
                          <tr>
                        <td style=" height: 21px; width: 98px;">
                                <span style="font-size: 8pt; font-family: Arial">User Group <span style="color: #ff0000">
                                    *</span></span>
                            </td>
                    <td style="width: 267px; color: #000000">
                        <asp:TextBox ID="TxtusergrpName" runat="server"  
                            CssClass="field_input" MaxLength="500" TabIndex="8" Width="195px"
                            Height="16px"></asp:TextBox>
                            <asp:TextBox ID="TxtusergrpCode" runat="server" style="display:none"   ></asp:TextBox>
                 
                        <asp:AutoCompleteExtender ID="TxtusergrpName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getusergrpslist" TargetControlID="TxtusergrpName" OnClientItemSelected="usergrpautocompleteselected" >
                        </asp:AutoCompleteExtender>

                    </td>
</tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </td>
        </tr>
        <tr>
        <td>
        
        <table>
            <tr>
                <td style="width:98px">
                    <span style="font-size: 8pt; font-family: Arial">Signature </span>
                </td>
                <td style="width: 196px">
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="fiel_input" onchange="return txtimage();"
                        Width="242px" TabIndex="9" />
                    <input id="txtSmall" runat="server" type="text" class="field_input" readonly="readonly"
                        style="visibility: hidden" />
                </td>
                <td colspan="2">
                    <asp:Label ID="txtSignature" runat="server" __designer:wfdid="w23" CssClass="field_input"
                        Width="280px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
            <ContentTemplate>
                <table>
                    <tbody>
                        <tr>
                            <td style="width: 98px; height: 19px;" class="field_input">
                                <span style="font-size: 8pt; font-family: Arial">Mobile No</span></td>
                            <td style="width: 267px; height: 19px;padding-left:0px;">
                                <input id="txtMobile" type="text" runat="server" class="field_input" maxlength="25"
                                   TabIndex="10"   onkeypress="return checkNumber()" style="width: 195px" />
                            </td>
                            <td style="width: 124px; font-size: 8pt; font-family: Arial; height: 19px;">
                                View All bookings
                            </td>
                            <td style="width: 58px; height: 19px;">
                                <input id="chkresstatus" type="checkbox" checked runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="field_input" style="width: 98px; height: 19px;">
                            </td>
                            <td style="width: 209px; height: 19px;">
                                <input id="txtMobile1" type="text" runat="server" class="field_input" maxlength="25"
                                  TabIndex="11"  onkeypress="return checkNumber()" style="width: 195px" />
                            </td>
                            <td style="width: 101px; font-size: 8pt; font-family: Arial; height: 19px;">
                            </td>
                            <td style="width: 58px; height: 19px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="field_input" style="width: 98px">
                                &nbsp;</td>
                            <td style="width: 209px;">
                                <input id="txtMobile2" type="text" runat="server" class="field_input" maxlength="25"
                                TabIndex="12"    onkeypress="return checkNumber()" style="width: 195px" />
                            </td>
                            <td style="width: 101px; font-size: 8pt; font-family: Arial;">
                                &nbsp;</td>
                            <td style="width: 58px">
                                &nbsp;</td>
                        </tr>
         
                        <tr>
                            <td class="field_input" style="width: 98px">
                         <span style="font-size: 8pt; font-family: Arial"> Upload Photo</span> 
                            </td>
                            <td colspan="2" style=" padding-top:10px">
                                                         <asp:FileUpload ID="UserImage" runat="server" TabIndex="4" />
<span style="font-size: 9pt; font-family: Arial">(400*400)</span>  &nbsp; 
                            </td>
                         
                            <td style="width: 58px">
                                &nbsp;
                            </td>
                        </tr> 
                                             <tr>

                    <td class="field_input"  >
                        &nbsp;</td>
                    <td  colspan="4" style="width:450px;padding-top:10px;">
                        <input style="width: 375px" id="txtimg"  readonly ="true"
    TabIndex="22"  type="text" maxlength="30" runat="server" />
                        <asp:Button ID="btnViewimage" runat="server" CssClass="field_button" 
                            TabIndex="5" Text="View" Width="64px" />
                       
                    </td>
                    <td style="padding-top:10px;"> <asp:Button ID="Btnremove" runat="server" CssClass="field_button" TabIndex="6" 
                            Text="Remove" Width="77px" />
                    </td>
                    </tr> 
                        <tr>
                            <td class="field_input" style="width: 98px">
                                Active
                            </td>
                            <td style="width: 209px;">
                                <input id="chkActive" type="checkbox"   TabIndex="13"  checked runat="server" />
                            </td>
                            <td style="width: 101px; font-size: 8pt; font-family: Arial;">
                                &nbsp;
                            </td>
                            <td style="width: 58px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="field_input" style="width: 98px">
                                <span style="font-size: 8pt; font-family: Arial">Email User Name</span></td>
                            <td style="width: 209px;">
                                <input style="width: 195px" id="txtemailusername" class="field_input" type="text" maxlength="100"
                                    runat="server"  TabIndex="1" />
                            </td>
                            <td style="width: 101px; font-size: 8pt; font-family: Arial;">
                                Email Password</td>
                            <td style="width: 58px">
                                <asp:TextBox ID="txtemailpassword" runat="server" __designer:wfdid="w37" 
                                    CssClass="field_input" MaxLength="100" TabIndex="4" TextMode="Password" 
                                    Width="193px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 86px">
                                <asp:Button ID="btnSave" runat="server" __designer:wfdid="w9"  TabIndex="14" CssClass="field_button"
                                    Text="Save" Width="62px" />
                            </td>
                            <td style="width: 209px; text-align: left">
                                <asp:Button ID="btnCancel" runat="server" __designer:wfdid="w10" TabIndex="15" CssClass="field_button"
                                    Text="Return To Search" />
                                &nbsp;
                                         <asp:Button ID="btnHelp" runat="server" __designer:dtid="1688858450198528" __designer:wfdid="w3"
                                    CssClass="field_button" TabIndex="16" OnClick="btnHelp_Click" Text="Help" Width="55px" />
                            </td>
                            <td style="width: 101px">
                       
                            </td>
                            <td style="width: 58px">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        
      <asp:TextBox ID="hdnFileName" Text="" runat="server" Style="display: none" />
        </td>
        </tr>
        </table> 
    </asp:Content>
