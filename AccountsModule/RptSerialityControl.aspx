<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptSerialityControl.aspx.vb" Inherits="RptSerialityControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>

<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);

    function InitializeRequestUserControl(sender, args) {

    }
    function EndRequestUserControl(sender, args) {
        AutoCompleteExtender_From_KeyUp();
        AutoCompleteExtender_To_KeyUp();
    }
              
</script>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        AutoCompleteExtender_From_KeyUp();
        AutoCompleteExtender_To_KeyUp();
    });

    function AutoComplete_OnClientPopulating(sender, args) {
        sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value);
    }
        
    function FromAutoCompleteSelected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtFromCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtFromCode.ClientID%>').value = '';
        }
    }
    function ToAutoCompleteSelected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtToCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtToCode.ClientID%>').value = '';
        }
    }

    function AutoCompleteExtender_From_KeyUp() {
        $("#<%=txtFrom.ClientID%>").bind("change", function () {
            var fromVal = document.getElementById('<%=txtFrom.ClientID%>');
            if (fromVal.value == '') {
                document.getElementById('<%=txtFromCode.ClientID%>').value = '';
            }
        });
        $("#<%=txtFrom.ClientID%>").keyup("change", function () {
            var fromVal = document.getElementById('<%=txtFrom.ClientID%>');
            if (fromVal.value == '') {
                document.getElementById('<%=txtFromCode.ClientID%>').value = '';
            }
        });
    }
    function AutoCompleteExtender_To_KeyUp() {
        $("#<%=txtTo.ClientID%>").bind("change", function () {
            var toVal = document.getElementById('<%=txtTo.ClientID%>');
            if (toVal.value == '') {
                document.getElementById('<%=txtToCode.ClientID%>').value = '';
            }
        });
        $("#<%=txtTo.ClientID%>").keyup("change", function () {
            var toVal = document.getElementById('<%=txtTo.ClientID%>');
            if (toVal.value == '') {
                document.getElementById('<%=txtToCode.ClientID%>').value = '';
            }
        });
    }

    function showProgress() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.show();
        $.removeCookie('DownloadSeriality', { path: '/' });
        //Check if receive cookie from server by second
        intervalProgress = setInterval("$.checkDownloadFileCompletely()", 1000);
        return true;
    }
    function HideProgess() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.hide(500);
    }

    $.checkDownloadFileCompletely = function () {
        var cookieValue = $.getCookie('DownloadSeriality');
        console.log(cookieValue + "---> Cookie Value;");
        if (cookieValue == 'True') {
            $.removeCookie('DownloadSeriality');
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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
        width: 100%; border-bottom: gray 1px solid" class="td_cell" align="left">
        <tbody>
            <tr>
                <td class="field_heading" align="center">
                    <asp:Label ID="lblHeading" runat="server" Text="Seriality Control" CssClass="field_heading"
                        Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 50%">
                    <table style="width: 100%" cellpadding="10px">
                        <tr>
                            <td style="width: 100%">
                                <fieldset style="width:50%">
                                <legend><asp:Label ID="Label1" runat="server" Text="Select Booking" CssClass="field_caption"></asp:Label>
                                </legend>
                                 <table style="width: 100%" cellpadding="10px">
                                        <tr>
                                            <td style="width: 20%" align="center">
                                                <asp:Label ID="lblFrom" runat="server" Text="From" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtFrom" runat="server" CssClass="field_input" TabIndex="1" Width="60%"></asp:TextBox>
                                                <asp:TextBox ID="txtFromCode" runat="server" style="display:none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteFrom" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetRequestId" TargetControlID="txtFrom" UseContextKey="true" OnClientPopulating="AutoComplete_OnClientPopulating"
                                                     OnClientItemSelected="FromAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%" align="center">
                                                <asp:Label ID="lblTo" runat="server" Text="To" CssClass="field_caption"></asp:Label>                                                
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTo" runat="server" CssClass="field_input" TabIndex="2" Width="60%"></asp:TextBox>
                                                <asp:TextBox ID="txtToCode" runat="server" style="display:none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteTo" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetRequestId" TargetControlID="txtTo" UseContextKey="true" OnClientPopulating="AutoComplete_OnClientPopulating" 
                                                    OnClientItemSelected="ToAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPdfReport" TabIndex="9" OnClick="btnPdfReport_Click" runat="server"
                                    Text="Pdf Report" CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;
                                <asp:Button ID="btnExcelReport" TabIndex="10" OnClick="btnExcelReport_Click" OnClientClick="return showProgress();" runat="server"
                                    Text="Excel Report" CssClass="field_button" CausesValidation="False"></asp:Button>                                
                                <asp:Button ID="btnAddNew" TabIndex="2" runat="server" Text="Add New" Font-Bold="False"
                                    CssClass="btn" Style="display: none"></asp:Button>
                                <asp:TextBox ID="txtDivcode" runat="server" Style="display: none;"></asp:TextBox>
                                <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#999999" BorderStyle="None" CssClass="td_cell" Font-Size="10px"
                                    Style="display: none">
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
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
    </ContentTemplate>
    </asp:UpdatePanel>           
</asp:Content>
