<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false"
    CodeFile="FlightsMasterSearch.aspx.vb" Inherits="FlightsMasterSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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

                //}
            }

            //            if (txtfromDate.value != null) {
            //                var dp = txtfromDate.value.split("/");

            //                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            //                var today = new Date();
            ////                alert(newDt);
            ////                alert(today);
            //                newDt = getFormatedDate(newDt);
            //                alert(newDt);
            //                today = getFormatedDate(today);

            //                newDt = new Date(newDt);
            //                today = new Date(today);
            //             
            //                if (newDt < today) {

            //                    alert('From date should not be less than todays date.');
            //                  //  txtfromDate.value = curDate.value;
            //                    return;
            //                }
            //                else {
            //             
            //                }


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

        $(document).ready(function () {
            visualsearchbox();
        });

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

        function visualsearchbox() {


            var $txtvsprocess = $(document).find('.cs_txtvsprocess');

            $txtvsprocess.val('FLIGHTTYPE:" " FLIGHTNO:" " AIRPORT:" " TEXT:" "');

            window.visualSearch = VS.init({
                container: $('#search_box_container'),
                query: $txtvsprocess.val(),
                showFacets: true,
                readOnly: false,
                unquotable: [
            'text',
            'account',
            'filter',
            'access'
          ],
                placeholder: 'Search for..',
                callbacks: {
                    search: function (query, searchCollection) {
                        var $txtvsprocess = $(document).find('.cs_txtvsprocess');
                        $txtvsprocess.val(visualSearch.searchQuery.serialize());

                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                        $txtvsprocesssplit.val(visualSearch.searchQuery.serializetosplit());

                        var btnvsprocess = document.getElementById("<%=btnvsprocess.ClientID%>");
                        btnvsprocess.click();

                        clearTimeout(window.queryHideDelay);
                        window.queryHideDelay = setTimeout(function () {
                            $query.animate({
                                opacity: 0
                            }, {
                                duration: 1000,
                                queue: false
                            });
                        }, 2000);
                    },
                    valueMatches: function (category, searchTerm, callback) {

                        switch (category) {
                            case 'FLIGHTTYPE':
                                var asSqlqry = '';

                                glcallback = callback;

                                ColServices.clsSectorServices.GetListOfFlighTypeVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'FLIGHTNO':
                                var asSqlqry = '';

                                glcallback = callback;

                                ColServices.clsSectorServices.GetListOfFlightNumberVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'AIRPORT':
                                var asSqlqry = '';

                                glcallback = callback;

                                ColServices.clsSectorServices.GetListOfAirportVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;

                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'FLIGHTTYPE', category: 'FLIGHT' },
                { label: 'FLIGHTNO', category: 'AIRPORT' },
                { label: 'AIRPORT', category: 'AIRPORT' },
                { label: 'TEXT', category: 'FLIGHT' },
              ]);
                    }
                }
            });
        }

        function fnFillSearchVS(result) {
            glcallback(result, {
                preserveOrder: true // Otherwise the selected value is brought to the top
            });
        }
    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        function InitializeRequest(sender, args) {

        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            visualsearchbox();
        }
    </script>
    <script language="javascript" type="text/javascript">
<!--
        // WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
        // -->
        function CallWebMethod(methodType) {
            switch (methodType) {

                case "airlinecode":
                    var select = document.getElementById("<%=ddlAirline .ClientID%>");
                    var selectname = document.getElementById("<%=ddlAirlineName .ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "airlinename":
                    var select = document.getElementById("<%=ddlAirlineName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlAirline.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }

        function FillCodeName(type) {

            switch (type) {
                case "Code":

                    var select = document.getElementById("<%=ddlAirBorCode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlAirBorName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "Name":
                    var select = document.getElementById("<%=ddlAirBorName.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlAirBorCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }




        function FillArrivalCodeName(type) {

            switch (type) {
                case "Code":
                    var select = document.getElementById("<%=ddlarvlairports.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlarvlairportname.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "Name":
                    var select = document.getElementById("<%=ddlarvlairportname.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlarvlairports.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }


    </script>
    <table>
        <tr>
            <td style="width: 100%">
                <table style="width: 100%;">
                    <tr align="center">
                        <td class="field_heading" colspan="1" style="text-align: center;" align="center">
                            Flight Master List&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="td_cell" colspan="1" style="color: blue;">
                            &nbsp;&nbsp; Type few characters of code or name and click search
                        </td>
                        <table style="width: 816px;">
                            <tbody>
                                <tr>
                                    <td align="center" colspan="6">
                                        <asp:Button ID="cmdhelp" TabIndex="8" OnClick="cmdhelp_Click" runat="server" Text="Help"
                                            Font-Bold="False" CssClass="search_button" _></asp:Button>&nbsp;
                                        <asp:Button ID="btnAddNew" TabIndex="9" runat="server" Text="Add New" Font-Bold="False"
                                            CssClass="btn"></asp:Button>&nbsp;<asp:Button ID="btnPrint" TabIndex="11" runat="server"
                                                Text="Report" CssClass="btn" __designer:wfdid="w6"></asp:Button>
                                    </td>
                                    <td style="display: none">
                                        <asp:RadioButton ID="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell"
                                            OnCheckedChanged="rbtnsearch_CheckedChanged" AutoPostBack="True" GroupName="GrSearch"
                                            Checked="True"></asp:RadioButton>&nbsp;<asp:RadioButton ID="rbtnadsearch" runat="server"
                                                Text="Advance Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged"
                                                Visible="false" AutoPostBack="True" GroupName="GrSearch"></asp:RadioButton>&nbsp;&nbsp;
                                        <asp:Button ID="btnSearch" TabIndex="8" runat="server" Text="Search" Font-Bold="False"
                                            CssClass="search_button"></asp:Button>&nbsp;
                                        <asp:Button ID="btnClear" TabIndex="9" OnClick="btnClear_Click" runat="server" Text="Clear"
                                            Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
                                        <asp:RadioButton ID="RBtnBrief" runat="server" Text="Brief" ForeColor="Black" CssClass="td_cell"
                                            AutoPostBack="True" GroupName="GrREport" Checked="True"></asp:RadioButton>
                                        &nbsp;<asp:RadioButton ID="RBtndetail" runat="server" Text="Detail" ForeColor="Black"
                                            CssClass="td_cell" AutoPostBack="True" GroupName="GrREport"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display: none">
                                        <asp:Panel ID="PnlFlightMaster" runat="server">
                                            <table style="width: 813px">
                                                <tbody>
                                                    <asp:DropDownList ID="ddlFlightType" runat="server" AutoPostBack="True" class="field_input"
                                                        CssClass="drpdown" TabIndex="1" Width="186px">
                                                        <asp:ListItem>[Select]</asp:ListItem>
                                                        <asp:ListItem>Arrival</asp:ListItem>
                                                        <asp:ListItem>Departure</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label3" runat="server" CssClass="field_caption" Text="Order By" Width="50px"></asp:Label>
                                                    <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="True" CssClass="drpdown"
                                                        OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" Width="134px">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblAirline" runat="server" Text="AirlineCode" Width="62px"></asp:Label>
                                                    <select id="ddlAirline" runat="server" class="drpdown" name="D2" onchange="CallWebMethod('airlinecode')"
                                                        style="width: 184px" tabindex="4" visible="True">
                                                        <option selected=""></option>
                                                    </select>
                                                    <asp:Label ID="lblAirName" runat="server" Text="AirlineName" Width="60px"></asp:Label>
                                                    <select id="ddlAirlineName" runat="server" class="drpdown" name="D1" onchange="CallWebMethod('airlinename')"
                                                        style="width: 251px" tabindex="5">
                                                        <option selected=""></option>
                                                    </select>
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="field_input" Width="80px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                                        PopupPosition="Right" TargetControlID="txtfromDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                        TargetControlID="txtfromDate">
                                                    </cc1:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                    <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                                        ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                    </cc1:MaskedEditValidator>
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px">
                                                    </asp:TextBox>
                                                    <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                                        PopupPosition="Right" TargetControlID="txtToDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                        TargetControlID="txtToDate">
                                                    </cc1:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                    <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                                        ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                    </cc1:MaskedEditValidator>
                                                    <asp:TextBox ID="TxtFlightno" runat="server" CssClass="field_input" MaxLength="20"
                                                        TabIndex="1" Width="179px"></asp:TextBox>
                                                    <asp:DropDownList ID="DDLstatus" runat="server" CssClass="drpdown" Width="186px">
                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                        <asp:ListItem Value="1">In active</asp:ListItem>
                                                        <asp:ListItem Value="2">Active</asp:ListItem>
                                                    </asp:DropDownList>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Pnldeparture" runat="server" BorderStyle="Double" BorderWidth="1px"
                                            TabIndex="5" Width="656px">
                                            <table>
                                                <select id="ddlAirBorCode" runat="server" class="field_input" onchange="FillCodeName('Code');"
                                                    style="width: 193px; height: 25;" tabindex="6">
                                                    <option selected=""></option>
                                                </select>
                                                <select id="ddlAirBorName" runat="server" class="field_input" onchange="FillCodeName('Name');"
                                                    style="width: 193px; margin-left: 0px; margin-bottom: 0px; height: 25;" tabindex="7">
                                                    <option selected=""></option>
                                                </select>&nbsp;&nbsp;
                                                <input style="width: 193px" id="txtdep" class="field_input" tabindex="8" type="text"
                                                    maxlength="50" runat="server" />
                                                &nbsp;<select id="ddlCityDeparture" runat="server" class="field_input" name="D3"
                                                    onchange="CallWebMethod('citynameDep')" style="width: 189px; margin-bottom: 0px;"
                                                    tabindex="14">
                                                    <option selected=""></option>
                                                </select>
                                        </asp:Panel>
                                        <asp:Panel ID="PanelArrival" runat="server" BorderStyle="Double" BorderWidth="1px"
                                            TabIndex="5" Width="656px">
                                            <table>
                                                <input style="width: 193px" id="txtarrvl" class="field_input" tabindex="6" type="text"
                                                    maxlength="50" runat="server" />
                                                <select id="ddlCityArrival" runat="server" class="field_input" name="D4" onchange="CallWebMethod('citynamearr')"
                                                    style="width: 189px" tabindex="14">
                                                    <option selected=""></option>
                                                </select>
                                                <select id="ddlarvlairports" runat="server" class="field_input" onchange="FillArrivalCodeName('Code');"
                                                    style="width: 193px; margin-bottom: 0px; height: 25px;" tabindex="8">
                                                    <option selected=""></option>
                                                </select>
                                                <select id="ddlarvlairportname" runat="server" class="field_input" onchange="FillArrivalCodeName('Name');"
                                                    style="width: 193px; margin-left: 0px; margin-bottom: 0px; height: 25;" tabindex="9">
                                                    <option selected=""></option>
                                                </select>&nbsp;&nbsp;
                                            </table>
                                        </asp:Panel>
                            </tbody>
                        </table>
                    
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <div style="width: 100%">
                    <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0;">
                        <div id="VS" class="container" style="border: 0px;">
                            <div id="search_box_container">
                            </div>
                            <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                            <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                        </div>
                    </div>
                    <div style="width: 18%; display: inline-block; vertical-align: top;">
                        <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" Font-Bold="False"
                            TabIndex="4" Text="Reset Search" /></div>
                </div>
                <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <table class="styleDatalist" style="border: 0px;">
                            <tr style="">
                                <td style="border: 0px;">
                                    <asp:Button ID="lnkCode" runat="server" class="button button4" Style="display: none"
                                        Text='<%# Eval("Code") %>' />
                                    <asp:Button ID="lnkValue" runat="server" class="button button4" Style="display: none"
                                        Text='<%# Eval("Value") %>' />
                                    <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" Font-Bold="False"
                                        Font-Size="Small" ForeColor="#000099" OnClientClick="return false;" Text='<%# Eval("CodeAndValue") %>' />
                                    <asp:Button ID="lbClose" runat="server" class="buttonClose button4" OnClick="lbClose_Click"
                                        Text="X" />
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <table class="style1">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" CssClass="field_caption" Text="Filter By "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOrder" runat="server">
                                <asp:ListItem Value="C">Created Date</asp:ListItem>
                                <asp:ListItem Value="M">Modified Date</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" onchange="filltodate(this);" Width="75px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="3" />
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                MaskType="Date" TargetControlID="txtFromDate">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MeFromDate"
                                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" onchange="ValidateChkInDate();" Width="75px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="3" />
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                MaskType="Date" TargetControlID="txtToDate">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MeToDate"
                                ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" TabIndex="4"
                                Text="Search by Date" />
                            &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False"
                                TabIndex="4" Text="Reset Dates" />
                        </td>
                        <td>
                            <asp:Label ID="RowSelectcs" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                            <asp:DropDownList ID="RowsPerPageCS" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </TBODY></table>
             
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel"
                    TabIndex="10" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_SearchResult" TabIndex="12" runat="server" Font-Size="10px"
                            Width="100%" CssClass="grdstyle" __designer:wfdid="w29" GridLines="Vertical"
                            CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False"
                            AllowSorting="True" AllowPaging="True">
                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderText="Tran Id">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("flight_tranid") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblflight_tranid" runat="server" Text='<%# Bind("flight_tranid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="flight_tranid" SortExpression="flight_tranid" HeaderText="Tran Id">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="flightcode" SortExpression="flightcode" HeaderText="Flight Number">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="type" SortExpression="type" HeaderText="Arrival Departure">
                                </asp:BoundField>
                                <asp:BoundField DataField="frmdate" SortExpression="frmdate" HeaderText="From Date">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="todate" SortExpression="todate" HeaderText="To Date">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="origin" SortExpression="origin" HeaderText="origin"></asp:BoundField>
                                <asp:BoundField DataField="Destination" SortExpression="Destination" HeaderText="Destination">
                                </asp:BoundField>
                                <asp:BoundField DataField="airlinename" SortExpression="airlinename" Visible="false"
                                    HeaderText="Airlines"></asp:BoundField>
                                <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                    DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                    DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
       

                                <asp:ButtonField HeaderText="Action" Text="Copy" CommandName="CopyRow">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                            </Columns>
                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                        </asp:GridView>
                        <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="900px"
                            __designer:wfdid="w30" Visible="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </td> </tr> </table>
    <br />
    <br />
    <br />
    </table> </tr>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
