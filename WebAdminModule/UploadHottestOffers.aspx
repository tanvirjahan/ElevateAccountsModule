 
<%@ Page Language="VB"  MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="UploadHottestOffers.aspx.vb" Inherits="UploadHottestOffers" %>
 

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
 
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
    
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
  
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
                       PrefSuppAutoCompleteExtenderKeyUp();

        }
    </script>

    <script type="text/javascript" charset="utf-8">

        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
                var date2 = calendarBehavior2._selectedDate;

                var dp2 = txtfromDate2.value.split("/");
                var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
                newDt2 = getFormatedDate(newDt2);
                newDt2 = new Date(newDt2);
                calendarBehavior2.set_selectedDate(newDt2);
            }

        }
        function getFormatedDate(chkdate) {
            var dd = chkdate.getDate();
            var mm = chkdate.getMonth() + 1; //January is 0!
            var yyyy = chkdate.getFullYear();
            if (dd < 10) { dd = '0' + dd };
            if (mm < 10) { mm = '0' + mm };
            chkdate = mm + '/' + dd + '/' + yyyy;
            return chkdate;
        }

        function filltodate(fDate) {
            var txtfromDate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");

            if ((txtToDate.value != null) && (txtToDate.value != '')) {

                var dpTo = txtToDate.value.split("/");

                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                var today = new Date();

                newDtTo = getFormatedDate(newDtTo);
                today = getFormatedDate(today);
                newDtTo = new Date(newDtTo);
                today = new Date(today);

                if (newDt > newDtTo) {

                    txtToDate.value = txtfromDate.value;
                    DateSelectCalExt();
                    return;
                }


            }



        }

        function ValidateChkInDate() {

            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate.value == null || txtfromDate.value == "") {
                txtToDate.value = "";
                alert("Please select From date.");
            }

            var dp = txtfromDate.value.split("/");
            var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = txtToDate.value.split("/");
            var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newChkInDt = getFormatedDate(newChkInDt);
            newChkOutDt = getFormatedDate(newChkOutDt);

            newChkInDt = new Date(newChkInDt);
            newChkOutDt = new Date(newChkOutDt);
            if (newChkInDt > newChkOutDt) {
                txtToDate.value = txtfromDate.value;
                alert("Todate date should not be greater than From date");
            }
        }


        var glcallback;

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }


        function fnFillSearchVS(result) {
            glcallback(result, {
                preserveOrder: true // Otherwise the selected value is brought to the top
            });
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

                popWin = open('../PriceListModule/ImageViewWindow.aspx?code=' + FileName.value + " &pagename=" + "UploadPopularDeals.aspx", 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
                FileName.value = "";
                return false

            }
            else {

                popWin = open('../PriceListModule/ImageViewWindow.aspx?', 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
            }
        }

        function PrefSuppautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtPrefSupCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtPrefSupCode.ClientID%>').value = '';
            }

        }

        function PrefSuppAutoCompleteExtenderKeyUp() {
            $("#<%= txtPrefSupName.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtPrefSupName.ClientID%>').value == '') {

                    document.getElementById('<%=txtPrefSupCode.ClientID%>').value = '';
                }

            });

        }
        function Validate(state) {

            if (state == 'New' || state == 'Edit') {

                //             if (document.getElementById("<%=txtCode.ClientID%>").value == '') {
                //                 document.getElementById("<%=txtCode.ClientID%>").focus();
                //                 alert('Please Enter Code');
                //                 return false;
                //             } else

                if (document.getElementById("<%=txtPrefSupCode.ClientID%>").value == '') {
                    document.getElementById("<%=txtPrefSupCode.ClientID%>").focus();
                    alert('Please Select Supplier');
                    return false;
                }
                else {
                    if (state == 'New') { if (confirm('Are you sure you want to save this Type?') == false) return false; }
                    if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                    if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
                }

            }
        }

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 714px; border-bottom: gray 2px solid; text-align: left">
                <tbody>
                    <tr>
                        <td style="height: 3px; text-align: center" class="td_cell" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Hottest Offers" ForeColor="White"
                                __designer:wfdid="w17" Width="100%" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style ="height :26px">
                           <asp:Label ID="lblcode" Text="Code" runat ="server"></asp:Label>  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td colspan="3" style="height: 26px">
                            <input id="txtCode" class="field_input" style="width: 182px" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>
                    </TR>
                    <tr>
           
                        <td class="td_cell" style=" height: 30px; ">
                            <asp:Label ID="lblprefsupp" runat="server" Text=" Supplier"></asp:Label>
                            <span style="color: #ff0000">*</span>
                        </td>
                        <td align="left" colspan="3" valign="top">
                            <asp:TextBox ID="txtPrefSupName" runat="server" CssClass="field_input" 
                                MaxLength="500" TabIndex="4" Width="548px"></asp:TextBox>
                            <asp:TextBox ID="txtPrefSupCode" runat="server" Style="display: none"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="txtPrefSupName_AutoCompleteExtender" 
                                runat="server" CompletionInterval="10" 
                                CompletionListCssClass="autocomplete_completionListElement" 
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                FirstRowSelected="false" MinimumPrefixLength="0" 
                                OnClientItemSelected="PrefSuppautocompleteselected" 
                                ServiceMethod="GetPreferSupplist" TargetControlID="txtPrefSupName">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr style ="display:none">
                       <td class="style5">
                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" 
                               Text="Offer Start Date"></asp:Label>
                        
                    </td>
                    <td class="style3" >
                        <asp:TextBox ID="txtfromdate" runat="server" onchange="filltodate(this);" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnFrmDt" 
                            PopupPosition="Right" TargetControlID="txtfromdate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtfromdate">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                            ControlExtender="MeFromDate" ControlToValidate="txtFromDate" 
                            CssClass="field_error" Display="Dynamic" 
                            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                            ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                            InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" 
                            Text="Offer End Date"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txttodate" runat="server" onchange="ValidateChkInDate();" 
                            Width="75px"></asp:TextBox>
                        <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnToDt" 
                            PopupPosition="Right" TargetControlID="txttodate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txttodate">
                        </cc1:MaskedEditExtender>
                        &nbsp;<asp:ImageButton ID="ImgBtnToDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        &nbsp;<cc1:MaskedEditValidator ID="MevToDate" runat="server" 
                            ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                            Display="Dynamic" EmptyValueBlurredText="Date is required" 
                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" 
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year">
                        </cc1:MaskedEditValidator>
                    </td>
                    </tr>
                    <tr style ="display:none">
                    <td colspan="2">
                     <span style=" color: #ff0000;fontsize:7">   ( If dates left blank system will consider the next 3 months for fetching prices)</span>
                
                    </td>

                    </tr>
                    
                    <tr>
                        <td class="td_cell" 
                            style=" text-align :left;vertical-align:top; height: 24px; ">
                            Hotel Image</td>
                        <td colspan="3">
                            <asp:FileUpload ID="hotelimage" runat="server" TabIndex="4" />
(626X464) &nbsp;</td>
                    </tr>
                    <tr>

                    <td class="td_cell" style=" text-align :left;vertical-align:top;">
                        &nbsp;</td>
                    <td align="left" colspan="3" style="height: 22px" valign="top">
                        <input style="width: 375px" id="txtimg"  readonly ="true"
    TabIndex="22"  type="text" maxlength="30" runat="server" />
                        <asp:Button ID="btnViewimage" runat="server" CssClass="field_button" 
                            TabIndex="5" Text="View" Width="64px" />
                        &nbsp;<asp:Button ID="Btnremove" runat="server" CssClass="field_button" TabIndex="6" 
                            Text="Remove" Width="77px" />
                    </td>
                    </tr>

                    <tr>
                        <td class="td_cell" style=" text-align :left;vertical-align:top; height: 24px;">
                            Remarks</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark" runat="server" __designer:wfdid="w20" 
                                CssClass="field_input" Height="116px" TabIndex="7" TextMode="MultiLine" 
                                Width="548px"></asp:TextBox>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td class="td_cell" style="height: 24px; " >
                            &nbsp;Active &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <input id="ChkInactive" tabindex="8" type="checkbox" checked runat="server" />
                        </td>
                        <td class="td_cell" style="float:right">
                            &nbsp;  </td>
                        <td class="td_cell">
                            &nbsp;</td>
                        
                    </tr>
                    <tr>
                        <td >
                            &nbsp;</td>
                        <td colspan="3">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="height: 3px;height: 24px; ">
                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                 TabIndex="9" Text="Save" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td colspan="3">
                            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                OnClick="btnCancel_Click" TabIndex="10" Text="Return To Search" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w8" 
                                CssClass="field_button" OnClick="btnhelp_Click" TabIndex="11" 
                                Text="Help" />
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td >
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
      <asp:TextBox ID="hdnFileName" Text="" runat="server" Style="display: none" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>

