<%@ Page Title="" Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false"
    CodeFile="RptPriceList.aspx.vb" Inherits="RptPriceList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>  
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            AutoCompleteExtenderKeyUp();
            AutoCompleteExtenderCountryKeyUp();
            AutoCompleteExtenderAgentKeyUp();            
        });

        function AutoCompleteExtenderKeyUp() {
            $("#<%= txtHotelName.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
            });
        }

        function AutoCompleteExtenderCountryKeyUp() {
            $("#<%= txtCtryName.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtCtryCode.ClientID%>').value = '';
                document.getElementById('<%=txtAgentCode.ClientID%>').value = '';
                document.getElementById('<%=txtAgentName.ClientID%>').value = '';
            });
        }

        function AutoCompleteExtenderAgentKeyUp() {
            $("#<%= txtAgentName.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtAgentCode.ClientID%>').value = '';
            });
        }

        function HotelAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
            }
            var btnCodeUpdate = document.getElementById("<%=btnCodeUpdate.ClientID%>");
            btnCodeUpdate.click();
        }

        function CountryAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtCtryCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtCtryCode.ClientID%>').value = '';
                document.getElementById('<%=txtAgentCode.ClientID%>').value = '';
                document.getElementById('<%=txtAgentName.ClientID%>').value = '';
            }
            var btnCodeUpdate = document.getElementById("<%=btnCodeUpdate.ClientID%>");
            btnCodeUpdate.click();
        }

        function AgentAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtAgentCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtAgentCode.ClientID%>').value = '';
            }
            var btnCodeUpdate = document.getElementById("<%=btnCodeUpdate.ClientID%>");
            btnCodeUpdate.click();
        }

        function AutoCompleteAgent_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=txtCtryCode.ClientID%>').value);
        }

        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");
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

                var dpFrom = txtfromDate.value.split("/");
                var newDt = new Date(dpFrom[2] + "/" + dpFrom[1] + "/" + dpFrom[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                var dpTo = txtToDate.value.split("/");
                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                newDtTo = getFormatedDate(newDtTo);
                newDtTo = new Date(newDtTo);
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

        function CompleteValidation() {
            if ($.trim(document.getElementById("<%=txtHotelName.ClientID%>").value) == "" || $.trim(document.getElementById("<%=txtHotelCode.ClientID%>").value) == "") {
                alert("Hotel Name can not be empty");
                return false;
            }
            if ($.trim(document.getElementById("<%=txtCtryName.ClientID%>").value) == "" || $.trim(document.getElementById("<%=txtCtryCode.ClientID%>").value) == "") {
                alert("Country can not be empty");
                return false;
            }
            var txtfromDate = document.getElementById("<%=txtFromDate.ClientID%>");
            if (txtfromDate.value == "") {
                alert("From Date can not be empty");
                return false;
            }
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtToDate.value == "") {
                alert("To Date can not be empty");
                return false;
            }
            ShowProgress();
            $.removeCookie('Downloaded', { path: '/' });
            //Check if receive cookie from server by second
            intervalProgress = setInterval("$.checkDownloadFileCompletely()", 1000);
            return true;            
        }

        function ShowProgress() {

            var ModalPopupLoading = $find("ModalPopupLoading");
            ModalPopupLoading.show();            
        }

        function HideProgess() {
            var ModalPopupLoading = $find("ModalPopupLoading");
            ModalPopupLoading.hide(500);            
        }

        $.checkDownloadFileCompletely = function () {
            var cookieValue = $.getCookie('Downloaded');
            console.log(cookieValue + "---> Cookie Value;");
            if (cookieValue == 'True') {                
                $.removeCookie('Downloaded');
                clearInterval(intervalProgress);
                HideProgess();
            }
        }

        /* get cookie from document.cookie */
        $.getCookie = function (cookieName) {
            var cookieValue = document.cookie;
            var c_start = cookieValue.indexOf(" " + cookieName + "=");
            if (c_start == -1) {
                c_start = cookieValue.indexOf(cookieName + "=");
            }
            if (c_start == -1) {
                cookieValue = null;
            }
            else {
                c_start = cookieValue.indexOf("=", c_start) + 1;
                var c_end = cookieValue.indexOf(";", c_start);
                if (c_end == -1) {
                    c_end = cookieValue.length;
                }
                cookieValue = unescape(cookieValue.substring(c_start, c_end));
            }
            return cookieValue;
        }

        /* Remove cookie in document.cookie */
        $.removeCookie = function (cookieName) {
            var cookies = document.cookie.split(";");

            for (var i = 0; i < cookies.length; i++) {
                var cookie = cookies[i];
                var eqPos = cookie.indexOf("=");
                var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
                if (name == cookieName) {
                    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
                }
            }
        }
    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);

        function InitializeRequestUserControl(sender, args) {

        }
       
        function EndRequestUserControl(sender, args) {
            AutoCompleteExtenderKeyUp();
            AutoCompleteExtenderCountryKeyUp();
            AutoCompleteExtenderAgentKeyUp();
        }        
    </script>
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid;" width="50%;">
        <tr>
            <td>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 100%; height: 11px" align="center" class="field_heading">
                            Price List
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table cellpadding="10px" style="width: 100%;" class="td_cell">
                                        <tr>
                                            <td style="width: 20%;">
                                                <label>
                                                    Hotel Name <span style="color: #ff0000" class="td_cell">*</span></label>
                                            </td>
                                            <td class="td_cell" valign="top" align="left" style="width: 70%;">
                                                <asp:TextBox ID="txtHotelName" runat="server" Width="98%" CssClass="field_input"
                                                    MaxLength="250" TabIndex="1"></asp:TextBox>
                                                <asp:TextBox ID="txtHotelCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:Button ID="btnCodeUpdate" runat="server" Style="display: none" />
                                                <asp:AutoCompleteExtender ID="txtHotelName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetHotelsList" TargetControlID="txtHotelName" OnClientItemSelected="HotelAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Country <span style="color: #ff0000" class="td_cell">*</span></label>
                                            </td>
                                            <td class="td_cell" valign="top" align="left">
                                                <asp:TextBox ID="txtCtryName" runat="server" Width="98%" CssClass="field_input" MaxLength="200"
                                                    TabIndex="2"></asp:TextBox>
                                                <asp:TextBox ID="txtCtryCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetCountriesList" TargetControlID="txtCtryName" OnClientItemSelected="CountryAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Agent</label>
                                            </td>
                                            <td class="td_cell" valign="top" align="left">
                                                <asp:TextBox ID="txtAgentName" runat="server" Width="98%" CssClass="field_input"
                                                    MaxLength="250" TabIndex="3"></asp:TextBox>
                                                <asp:TextBox ID="txtAgentCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="txtAgentName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetAgentsList" TargetControlID="txtAgentName" ContextKey="true"
                                                    OnClientPopulating="AutoCompleteAgent_OnClientPopulating" OnClientItemSelected="AgentAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Rate Type</label>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:RadioButton ID="rdoNetPayable" runat="server" CssClass="field_input" Text="Net Payable"
                                                    Checked="true" GroupName="rdoGrpRatetype" TabIndex="4" />
                                                <asp:RadioButton ID="rdoSellingRates" runat="server" CssClass="field_input" Text="Selling Rates"
                                                    GroupName="rdoGrpRatetype" TabIndex="5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    From Date <span style="color: #ff0000" class="td_cell">*</span></label>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:TextBox ID="txtFromDate" runat="server" Style="width: 90px" CssClass="field_input"
                                                    onchange="filltodate(this);" MaxLength="3" TabIndex="6"></asp:TextBox>
                                                <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                <asp:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                                    PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtFromDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                    TargetControlID="txtFromDate">
                                                </asp:MaskedEditExtender>
                                                <asp:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                                    ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                    EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                </asp:MaskedEditValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    To Date <span style="color: #ff0000" class="td_cell">*</span></label>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:TextBox ID="txtToDate" runat="server" Style="width: 90px" onchange="ValidateChkInDate();"
                                                    CssClass="field_input" MaxLength="3" TabIndex="7"></asp:TextBox>
                                                <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                <asp:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                                    PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                    TargetControlID="txtToDate">
                                                </asp:MaskedEditExtender>
                                                <asp:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                                    ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                    EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                </asp:MaskedEditValidator>
                                            </td>
                                        </tr>
                                         <tr>
                                         <td>
                                             <INPUT id="chkcolinExcel" tabIndex=4 type=checkbox  runat="server"  />
            <TD style="HEIGHT: 16px">
            <asp:Label ID="lblgroup" runat="server">
                                                    Group columns by dates </asp:Label></TD>            
            
            </td>
            
            
                          
                                        </tr>
                                        
                                        <tr><td colspan="2" align="center">                           
                                            <asp:Button ID="btnLoadReport" CssClass="btn" runat="server" TabIndex="8" Text="Load Report" OnClientClick="return CompleteValidation();"/>                                            
                                            &nbsp;&nbsp;
                                            <asp:Button ID="BtnClear" runat="server" CssClass="btn" TabIndex="9" Text="Clear" />
                                        </td>
                                    </table>                                    
                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnLoadReport" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>                    
                </table>
            </td>
        </tr>
        <tr><td style="display:none">
               <asp:Button id="btnadd"  runat="server"  
        CssClass="field_button"></asp:Button>
         <asp:Button id="Button1" runat="server" 
        CssClass="field_button"></asp:Button></td></tr>
    </table>  
    <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
     
     <center>
    <div id="Loading1" runat="server" style="height: 150px; width: 500px; vertical-align: middle">
        <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="150" />
        <h2 style="color: #06788B">
            Processing please wait...</h2>
    </div>
    </center>
    <asp:ModalPopupExtender ID="ModalPopupLoading" runat="server" BehaviorID="ModalPopupLoading"
        TargetControlID="btnInvisibleLoading" CancelControlID="btnCloseLoading" PopupControlID="Loading1"
        BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <input id="btnInvisibleLoading" runat="server" type="button" value="Cancel" style="display: none" />
    <input id="btnCloseLoading" runat="server" type="button" value="Cancel" style="display: none" />         
</asp:Content>
