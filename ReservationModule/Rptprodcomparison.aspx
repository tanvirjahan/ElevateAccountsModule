<%@ Page Title="" Language="VB" MasterPageFile="~/ReservationMaster.master" AutoEventWireup="false"
    CodeFile="Rptprodcomparison.aspx.vb" Inherits="Rptprodcomparison" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=FromDt_CalendarExtender.ClientID %>");
                var date = calendarBehavior1._selectedDate;
                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtToDt.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=ToDt_CalendarExtender.ClientID %>");
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




        function fillCmptodate(fDate) {
            var txtCmpfromDate = document.getElementById("<%=txtCmpFrmDt.ClientID%>");
            var txtCmpToDate = document.getElementById("<%=txtCmpToDt.ClientID%>");

            if ((txtCmpToDate.value != null) && (txtCmpToDate.value != '')) {
                var dpFrom = txtCmpfromDate.value.split("/");
                var newDt = new Date(dpFrom[2] + "/" + dpFrom[1] + "/" + dpFrom[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                var dpTo = txtCmpToDate.value.split("/");

                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                newDtTo = getFormatedDate(newDtTo);
                newDtTo = new Date(newDtTo);
                if (newDt > newDtTo) {
                    txtCmpToDate.value = txtCmpfromDate.value;
                    DateSelectCalExt();
                    return;
                }
            }

        }


        function filltodate(fDate) {
            var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDt.ClientID%>");

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



        function ValidateDate() {
            var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDt.ClientID%>");
            var txtCmpfromDate = document.getElementById("<%=txtCmpFrmDt.ClientID%>");
            var txtCmpToDate = document.getElementById("<%=txtCmpToDt.ClientID%>");
            if (txtfromDate.value == null || txtfromDate.value == "") {
                txtfromDate.value = txtToDate.value;
            }
            if (txtCmpfromDate.value == null || txtCmpfromDate.value == "") {
                txtCmpfromDate.value = txtCmpToDate.value;
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
                alert("To date should be greater than or equal to From date");
            }


            var dp = txtCmpfromDate.value.split("/");
            var newChkInCmpDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = txtCmpToDate.value.split("/");
            var newChkOutCmpDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newChkInCmpDt = getFormatedDate(newChkInCmpDt);
            newChkOutCmpDt = getFormatedDate(newChkOutCmpDt);

            newChkInCmpDt = new Date(newChkInCmpDt);
            newChkOutCmpDt = new Date(newChkOutCmpDt);
            if (newChkInCmpDt > newChkOutCmpDt) {
                txtCmpToDate.value = txtCmpfromDate.value;
                alert("Compare To date should be greater than or equal to From date");
            }
        }

        function CountryGroupAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = '';
            }
        }

        function CustGroupAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtCustGroupCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtCustGroupCode.ClientID%>').value = '';
            }
        }

        function SourceCtryAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtSourceCtryCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtSourceCtryCode.ClientID%>').value = '';
            }
        }

        function CustCategoryAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtCustCategoryCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtCustCategoryCode.ClientID%>').value = '';
            }
        }

        function CustAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtCustCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtCustCode.ClientID%>').value = '';
            }
        }
        function Txt_HotelChain_Selected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=Txt_HotelChainCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=Txt_HotelChainCode.ClientID%>').value = '';
            }
        }
        function HotelCityAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtHotelCityCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtHotelCityCode.ClientID%>').value = '';
            }
        }

        function HotelCategoryAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtHotelCategoryCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtHotelCategoryCode.ClientID%>').value = '';
            }
        }
        function HotelGroupsAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtHotelGroupCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtHotelGroupCode.ClientID%>').value = '';
            }
        }
        function HotelAutoCompleteSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
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
            AutoCompleteExtender_CountryGroup_KeyUp();
            AutoCompleteExtender_CustGroup_KeyUp();
            AutoCompleteExtender_Country_KeyUp();
            AutoCompleteExtender_CustCategory_KeyUp();
            AutoCompleteExtender_Agent_KeyUp();
            AutoCompleteExtender_HotelCity_KeyUp();
            AutoCompleteExtender_HotelCategory_KeyUp();
            AutoCompleteExtender_Hotel_KeyUp();
            AutoCompleteExtender_HotelGroups_KeyUp();
            HotelChainGroupAutoCompleteExtenderKeyUp();
        }
              
    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoCompleteExtender_CountryGroup_KeyUp();
            AutoCompleteExtender_CustGroup_KeyUp();
            AutoCompleteExtender_Country_KeyUp();
            AutoCompleteExtender_CustCategory_KeyUp();
            AutoCompleteExtender_Agent_KeyUp();
            AutoCompleteExtender_HotelCity_KeyUp();
            AutoCompleteExtender_HotelCategory_KeyUp();
            AutoCompleteExtender_Hotel_KeyUp();
            AutoCompleteExtender_HotelGroups_KeyUp();
            HotelChainGroupAutoCompleteExtenderKeyUp();
        });

        function AutoCompleteExtender_CountryGroup_KeyUp() {
            $("#<%= txtCtryGroup.ClientID %>").bind("change", function () {
                var ctryGroup = document.getElementById('<%=txtCtryGroup.ClientID%>');
                if (ctryGroup.value == '') {
                    document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = '';
                }
            });
            $("#<%= txtCtryGroup.ClientID %>").keyup("change", function () {
                var ctryGroup = document.getElementById('<%=txtCtryGroup.ClientID%>');
                if (ctryGroup.value == '') {
                    document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = '';
                }
            });
        }

        function AutoCompleteExtender_CustGroup_KeyUp() {
            $("#<%=txtCustGroup.ClientID%>").bind("change", function () {
                var CustGroup = document.getElementById('<%=txtCustGroup.ClientID%>');
                if (CustGroup.value == '') {
                    document.getElementById('<%=txtCustGroupCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtCustGroup.ClientID%>").keyup("change", function () {
                var CustGroup = document.getElementById('<%=txtCustGroup.ClientID%>');
                if (CustGroup.value == '') {
                    document.getElementById('<%=txtCustGroupCode.ClientID%>').value = '';
                }
            });
        }

        function AutoCompleteExtender_Country_KeyUp() {
            $("#<%=txtSourceCtry.ClientID%>").bind("change", function () {
                var SourceCtry = document.getElementById('<%=txtSourceCtry.ClientID%>');
                if (SourceCtry.value == '') {
                    document.getElementById('<%=txtSourceCtryCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtSourceCtry.ClientID%>").keyup("change", function () {
                var SourceCtry = document.getElementById('<%=txtSourceCtry.ClientID%>');
                if (SourceCtry.value == '') {
                    document.getElementById('<%=txtSourceCtryCode.ClientID%>').value = '';
                }
            });
        }

        function AutoCompleteExtender_CustCategory_KeyUp() {
            $("#<%=txtCustCategory.ClientID%>").bind("change", function () {
                var CustCategory = document.getElementById('<%=txtCustCategory.ClientID%>');
                if (CustCategory.value == '') {
                    document.getElementById('<%=txtCustCategoryCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtCustCategory.ClientID%>").keyup("change", function () {
                var CustCategory = document.getElementById('<%=txtCustCategory.ClientID%>');
                if (CustCategory.value == '') {
                    document.getElementById('<%=txtCustCategoryCode.ClientID%>').value = '';
                }
            });
        }

        function AutoCompleteExtender_Agent_KeyUp() {
            $("#<%=txtCust.ClientID%>").bind("change", function () {
                var Cust = document.getElementById('<%=txtCust.ClientID%>');
                if (Cust.value == '') {
                    document.getElementById('<%=txtCustCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtCust.ClientID%>").keyup("change", function () {
                var Cust = document.getElementById('<%=txtCust.ClientID%>');
                if (Cust.value == '') {
                    document.getElementById('<%=txtCustCode.ClientID%>').value = '';
                }
            });
        }

        function AutoCompleteExtender_HotelCity_KeyUp() {
            $("#<%=txtHotelCity.ClientID%>").bind("change", function () {
                var HotelCity = document.getElementById('<%=txtHotelCity.ClientID%>');
                if (HotelCity.value == '') {
                    document.getElementById('<%=txtHotelCityCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtHotelCity.ClientID%>").keyup("change", function () {
                var HotelCity = document.getElementById('<%=txtHotelCity.ClientID%>');
                if (HotelCity.value == '') {
                    document.getElementById('<%=txtHotelCityCode.ClientID%>').value = '';
                }
            });
        }

        function AutoCompleteExtender_HotelCategory_KeyUp() {
            $("#<%=txtHotelCategory.ClientID%>").bind("change", function () {
                var HotelCategory = document.getElementById('<%=txtHotelCategory.ClientID%>');
                if (HotelCategory.value == '') {
                    document.getElementById('<%=txtHotelCategoryCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtHotelCategory.ClientID%>").keyup("change", function () {
                var HotelCategory = document.getElementById('<%=txtHotelCategory.ClientID%>');
                if (HotelCategory.value == '') {
                    document.getElementById('<%=txtHotelCategoryCode.ClientID%>').value = '';
                }
            });
        }

        function AutoCompleteExtender_HotelCategory_KeyUp() {
            $("#<%=txtHotelGroups.ClientID%>").bind("change", function () {
                var txtHotelGroups = document.getElementById('<%=txtHotelGroups.ClientID%>');
                if (txtHotelGroups.value == '') {
                    document.getElementById('<%=txtHotelGroupCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtHotelGroups.ClientID%>").keyup("change", function () {
                var txtHotelGroups = document.getElementById('<%=txtHotelGroups.ClientID%>');
                if (txtHotelGroups.value == '') {
                    document.getElementById('<%=txtHotelGroupCode.ClientID%>').value = '';
                }
            });
        }

        function Txt_HotelChain_Selected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=Txt_HotelChainCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=Txt_HotelChainCode.ClientID%>').value = '';
            }
        }
        function HotelChainGroupAutoCompleteExtenderKeyUp() {
            $("#<%= Txt_HotelChain.ClientID %>").bind("change", function () {
                if (document.getElementById('<%=Txt_HotelChain.ClientID%>').value == '') {
                    document.getElementById('<%=Txt_HotelChainCode.ClientID%>').value = '';
                }
            });

            $("#<%= txtHotel.ClientID %>").keyup(function (event) {
                if (document.getElementById('<%=Txt_HotelChain.ClientID%>').value == '') {
                    document.getElementById('<%=Txt_HotelChainCode.ClientID%>').value = '';
                }
            });
        }
        function AutoCompleteExtender_Hotel_KeyUp() {
            $("#<%=txtHotel.ClientID%>").bind("change", function () {
                var Hotel = document.getElementById('<%=txtHotel.ClientID%>');
                if (Hotel.value == '') {
                    document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtHotel.ClientID%>").keyup("change", function () {
                var Hotel = document.getElementById('<%=txtHotel.ClientID%>');
                if (Hotel.value == '') {
                    document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
                }
            });
        }   
    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div style="margin-top: -6px; width: 100%;">
                <table style="border: gray 2px solid; width: 100%;" class="td_cell" align="left">
                    <tr>
                        <td valign="top" align="center" style="width: 100%;">
                            <asp:Label ID="lblHeading" runat="server" Text="Comparative Report" CssClass="field_heading"
                                Width="100%" ForeColor="White" Style="padding: 2px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 100%; padding: 10px 0px 12px 0px" align="center">
                            <asp:Button ID="btnHelp" runat="server" Text="Help" Font-Bold="False" CssClass="search_button"
                                Style="display: none"></asp:Button>
                            &nbsp;&nbsp;<asp:Button ID="btnAddNew" runat="server" Text="Add New" Font-Bold="False"
                                CssClass="btn" Style="display: none"></asp:Button>
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="NewFormat" />
                            <%--          <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Font-Bold="False"
                                Text="Export To Excel" Style="display: none" />--%>
                            <asp:GridView ID="gvSearchResult" runat="server" Style="display: none">
                            </asp:GridView>
                            <asp:TextBox ID="txtRptType" runat="server" Style="display: none"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <table cellpadding="7" width="100%">
                                <tr>
                                    <td style="width: 100%">
                                        <table cellpadding="7" width="100%">
                                            <tr>
                                                <td>
                                                    <label visible="false" runat="server" id="lbldatetype" class="field_caption">
                                                        Date Type</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList Visible="false" ID="ddldatetype" CssClass="field_input" runat="server"
                                                        Width="90%" TabIndex="3">
                                                        <asp:ListItem Text="ARRIVAL" Value="Arrival">
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="DEPARTURE" Value="Departure"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;">
                                                    <asp:Label ID="frmdate" runat="server" class="field_caption" Text="Current Period From"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <asp:TextBox ID="txtFromDt" CssClass="field_input" runat="server" TabIndex="1" onchange="filltodate(this);"
                                                        Width="75px" OnTextChanged="txtFromDt_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <asp:CalendarExtender ID="FromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnFromDt"
                                                        PopupPosition="Right" TargetControlID="txtFromDt"></asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="FromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtFromDt"></asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="MevFromDt" runat="server" ControlExtender="FromDt_MaskedEditExtender"
                                                        ControlToValidate="txtFromDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="FromDt_MaskedEditExtender"
                                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                        TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                                <td style="width: 15%">
                                                    <asp:Label class="field_caption" ID="lbltodate" runat="server" Text=" To  "></asp:Label>
                                                </td>
                                                <td style="width: 35%">
                                                    <asp:TextBox ID="txtToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                                        TabIndex="2" Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="ToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnToDt" PopupPosition="Right"
                                                        TargetControlID="txtToDt"></asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="ToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtToDt"></asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="imgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="MevToDt" runat="server" ControlExtender="ToDt_MaskedEditExtender"
                                                        ControlToValidate="txtToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="ToDt_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;">
                                                    <asp:Label ID="Comparfrmdate" runat="server" class="field_caption" Text="Compare Period From"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <asp:TextBox ID="txtCmpFrmDt" CssClass="field_input" runat="server" TabIndex="3"
                                                        onchange="fillCmptodate(this);" OnTextChanged="txtCmpFrmDt_TextChanged" AutoPostBack="true"
                                                        Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CmpFrmDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnCmpFromDt"
                                                        PopupPosition="Right" TargetControlID="txtCmpFrmDt"></asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="CmpFrmDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtCmpFrmDt"></asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnCmpFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="MevCmpFromDt" runat="server" ControlExtender="FromDt_MaskedEditExtender"
                                                        ControlToValidate="txtCmpFrmDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="CmpFrmDt_MaskedEditExtender"
                                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                        TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                                <td style="width: 15%">
                                                    <asp:Label class="field_caption" ID="lblCmptodate" runat="server" Text=" To  "></asp:Label>
                                                </td>
                                                <td style="width: 35%">
                                                    <asp:TextBox ID="txtCmpToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                                        TabIndex="4" Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CmpToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnCmpToDt"
                                                        PopupPosition="Right" TargetControlID="txtCmpToDt"></asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="CmpToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtCmpToDt"></asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="imgBtnCmpToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="MevCmpToDt" runat="server" ControlExtender="CmpToDt_MaskedEditExtender"
                                                        ControlToValidate="txtCmpToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="ToDt_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Country Group</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCtryGroup" CssClass="field_input" runat="server" TabIndex="5"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtCtryGroupCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCtryGroup" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetCountryGroup" TargetControlID="txtCtryGroup" OnClientItemSelected="CountryGroupAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <%--                 <td>
                                                    <label class="field_caption">
                                                        Booking Status</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBookingStatus" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="6">
                                                    </asp:DropDownList>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Source Country</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSourceCtry" CssClass="field_input" runat="server" TabIndex="7"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtSourceCtryCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteSourceCtry" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetSourceCountry" TargetControlID="txtSourceCtry" OnClientItemSelected="SourceCtryAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Guest Name</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGuestName" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="8">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Supplier</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHotel" CssClass="field_input" runat="server" TabIndex="9" Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtHotelCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteHotel" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetHotels" TargetControlID="txtHotel" OnClientItemSelected="HotelAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Agent</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCust" CssClass="field_input" runat="server" TabIndex="10" Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtCustCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCust" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetCustomers" TargetControlID="txtCust" OnClientItemSelected="CustAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Hotel Category</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHotelCategory" CssClass="field_input" runat="server" TabIndex="11"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtHotelCategoryCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteHotelCategory" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetHotelCategory" TargetControlID="txtHotelCategory" OnClientItemSelected="HotelCategoryAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Agent Category</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustCategory" CssClass="field_input" runat="server" TabIndex="12"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtCustCategoryCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCustCategory" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetCustomerCategory" TargetControlID="txtCustCategory" OnClientItemSelected="CustCategoryAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Hotel Group</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHotelGroups" CssClass="field_input" runat="server" TabIndex="13"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtHotelGroupCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteHotelGroup" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetHotelGroups" TargetControlID="txtHotelGroups" OnClientItemSelected="HotelGroupsAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Agent Group</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustGroup" CssClass="field_input" runat="server" TabIndex="14"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtCustGroupCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCustGroup" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetCustomerGroup" TargetControlID="txtCustGroup" OnClientItemSelected="CustGroupAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Hotel City</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHotelCity" CssClass="field_input" runat="server" TabIndex="15"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtHotelCityCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteHotelCity" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetHotelCity" TargetControlID="txtHotelCity" OnClientItemSelected="HotelCityAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Columns</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlColumnns" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="16">
                                                        <asp:ListItem Text="All(Units,RoomNights,Pax,SaleValue)" Value="All"> </asp:ListItem>
                                                        <asp:ListItem Text="RoomNights" Value="RoomNights"></asp:ListItem>
                                                        <asp:ListItem Text="Units" Value="Units"></asp:ListItem>
                                                        <asp:ListItem Text="Pax" Value="Pax"></asp:ListItem>
                                                        <asp:ListItem Text="SaleValue" Value="SaleValue"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Hotel Chain
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_HotelChain" runat="server" CssClass="field_input" MaxLength="25"
                                                        TabIndex="6" Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="Txt_HotelChainCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender_Txt_HotelChain" runat="server"
                                                        CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                        CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                        FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetHotelChain"
                                                        TargetControlID="Txt_HotelChain" OnClientItemSelected="Txt_HotelChain_Selected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Order</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOrder" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="18">
                                                        <asp:ListItem Text="Alphabetical" Value="0"> </asp:ListItem>
                                                        <asp:ListItem Text="Ascending" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Descending" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Request Type</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlRequestType" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="19">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Hotel" Value="Hotel"></asp:ListItem>
                                                        <asp:ListItem Text="Transfers" Value="Transfers"></asp:ListItem>
                                                        <asp:ListItem Text="Tours" Value="Tours"></asp:ListItem>
                                                        <asp:ListItem Text="AirportMA" Value="AirportMA"></asp:ListItem>
                                                        <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                                        <asp:ListItem Text="Visa" Value="Visa"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Group By</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGroupBy" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="20">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Service Status</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBookingStatus" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="5">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Booking Status</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlbookingstatusdb" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="5">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ChkExclComp" Text="Exclude Agent Complement" runat="server" TabIndex="21" />
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Invoice Status</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlInvStatus" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="17">
                                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Invoiced" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Not Invoiced" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="Chk_StafRoom" Text="Exclude Staff Room" runat="server" TabIndex="21" />
                                                </td>
                                                <td>
                                                    
                                                </td>
                                                <td>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:Button ID="btnLoadReport" runat="server" CssClass="btn" Text="Report" TabIndex="22" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" Text="Reset" TabIndex="23" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnLoadReport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
