<%@ Page Title="Excursion Request Entry" Language="VB" MasterPageFile="~/SubPageMaster.master"
    AutoEventWireup="false" CodeFile="ExcursionRequestEntry.aspx.vb" Inherits="ExcursionModule_ExcursionRequestEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <style type="text/css">
        .TextBoxRightAlign
        {
            text-align: right;
        }
    </style>
    <script type="text/javascript">

        //Added by riswan to bind auto complete text box and enable/disable the currency dropdown depends on the basecurrency

        $(document).ready(function () {
            MyAutoCustomer();
        });
        function MyAutoCustomer() {
            var type = jQuery(".MyAutoCompleteTypeClass").val();
            jQuery.ajax({
                url: "../ClsServices.asmx/CustomerAutoCompleteExcursionRequest",
                data: "{ para1:'" + type + "',para2:''}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    if (data.d.length > 0) {
                        var result = data.d;
                        // alert(result.length);

                        if (result.length == 0)
                            return;

                        jQuery(".MyAutoCompleteClass").autocomplete({
                            source: jQuery.map(result, function (item) {
                                return {
                                    value: item.Name,
                                    text: item.Id

                                }

                            }),
                            minLength: 1,
                            select: function (event, ui) {

                                jQuery(".MyDropDownListCustValue").val(ui.item.text);
                                jQuery(".MyDropDownListCustValue").change();

                            }


                        });

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
        
        function BaseCurrency(ddlCurrency) {
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            $.ajax({
                type: "POST",
                url: "ExcursionRequestEntry.aspx/GetBaseCurrency",
                data: '{"constr":"' + connstr.value + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d == ddlCurrency.value) {
                        ddlCurrency.disabled = false;
                    } else {
                        ddlCurrency.disabled = true;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }

        function FormValidation(state) {

            var ddlMarket = document.getElementById("<%=ddlMarket.ClientID%>");
            var ddlSellingType = document.getElementById("<%=ddlSellingType.ClientID%>");

            var ddlConcierge = document.getElementById("<%=ddlConcierge.ClientID%>");
            var ddlSalesExpert = document.getElementById("<%=ddlSalesExpert.ClientID%>");
            var ddlPayment = document.getElementById("<%=ddlPayment.ClientID%>");

            var txtCreditCardNo = document.getElementById("<%=txtCreditCardNo.ClientID%>");
            var txtConvRate = document.getElementById("<%=txtConvRate.ClientID%>");
            var txtTicketNo = document.getElementById("<%=txtTicketNo.ClientID%>");

            var ddlLanguage = document.getElementById("<%=ddlLanguage.ClientID%>");

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");

            if (ddlMarket.value == "[Select]" || ddlMarket.value == "") {
                ddlMarket.focus();
                alert("Please select Market");
                return false;
            }

            if (ddlSellingType.value == "[Select]" || ddlSellingType.value == "") {
                ddlSellingType.focus();
                alert("Please select Selling Type");
                return false;
            }

            if (ddlConcierge.value == "[Select]" || ddlConcierge.value == "") {
                //ddlConcierge.focus();
                alert("Please select Concierge");
                return false;
            }

            if (ddlSalesExpert.value == "[Select]" || ddlSalesExpert.value == "") {
                ddlSalesExpert.focus();
                alert("Please select Sales Expert");
                return false;
            }

            if (ddlPayment.value == "[Select]" || ddlPayment.value == "") {
                ddlPayment.focus();
                alert("Please select Payment Mode");
                return false;
            }

            if (ddlPayment.value != "[Select]" && txtCreditCardNo.value == "") {
                var codeid = ddlPayment.options[ddlPayment.selectedIndex].value;
                if (codeid == "CRE") {
                    alert("Please enter Cedit Card Number");
                    return false;
                }
            }


            if (txtConvRate.value == "0") {
                txtConvRate.focus();
                alert("Conversion Rate cannot be 0");
                return false;
            }


            if (ddlLanguage.value == "[Select]" || ddlLanguage.value == "") {
                ddlLanguage.focus();
                alert("Please select Language");
                return false;
            }

            if (txtTicketNo.value == "") {
                if (confirm('Do you want to enter Ticket No?') == true) {
                    var ticketNo = window.prompt("Please Enter Ticket No", "");
                    if (ticketNo != null) {
                        txtTicketNo.value = ticketNo;
                    }
                }
                return true;
            }

        }




        function CallWebMethod(methodType) {

            switch (methodType) {

                case "FillAgentDetails":

                    var select = document.getElementById("<%=ddlCustomer.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    var txtAgent = document.getElementById("<%=txtAgent.ClientID%>");

                    var ddltouroperator = document.getElementById("<%=touroperator.ClientID%>");
                    //var txtoperator = document.getElementById("<%=txtoperator.ClientID%>");
                    var lbloperator = document.getElementById("<%=lbloperator.ClientID%>");

                    var hdntouroperator = document.getElementById("<%=hdntouroperator.ClientID%>");

                    txtAgent.value = select.options[select.selectedIndex].text;
                    constr = connstr.value;

                    //16082014

                    if (select.value == hdntouroperator.value) {
                        ddltouroperator.style.display = "block";
                        // txtoperator.style.display = "block";
                        lbloperator.style.display = "block";
                        ddltouroperator.value = "[Select]"
                    }
                    else {
                        ddltouroperator.style.display = "none";
                        // txtoperator.style.display = "none";
                        lbloperator.style.display = "none";
                        ddltouroperator.value = "[Select]"
                    }

                    if (codeid != '[Select]') {
                        // ColServices.clsServices.GetMarketNameListnew(constr, codeid, FillMarket, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetMarketExcursionCustomer(constr, codeid, FillMarket, ErrorHandler, TimeOutHandler);

                        // ColServices.clsServices.GetSellingTypeExcursion(constr, codeid, FillSellingType, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetSellingTypeExcursionCustomer(constr, codeid, FillSellingType, ErrorHandler, TimeOutHandler);

                        ColServices.clsServices.GetCurrencyExcursionCustomer(constr, codeid, FillCurrency, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetExchRate4Reservation(constr, codeid, SetCurrency, ErrorHandler, TimeOutHandler);
                        //17082014
                        ColServices.clsServices.getconceirgedet(constr, codeid, setconceirge, ErrorHandler, TimeOutHandler)
                        ColServices.clsServices.getsalesexpdet(constr, codeid, setsalesexpt, ErrorHandler, TimeOutHandler)
                    }

                    break;


                case "ShowTextBox":
                    var ddlPayment = document.getElementById("<%=ddlPayment.ClientID%>");
                    var txtCreditCardNo = document.getElementById("<%=txtCreditCardNo.ClientID%>");
                    var lblCreditCardNo = document.getElementById("<%=lblCreditCardNo.ClientID%>");

                    var codeid = ddlPayment.options[ddlPayment.selectedIndex].value;
                    if (codeid == "CRE") {
                        txtCreditCardNo.style.display = "block";
                        lblCreditCardNo.style.display = "block";
                    }
                    else {

                        txtCreditCardNo.style.display = "none";
                        lblCreditCardNo.style.display = "none";
                    }
                    break;


                case "ChangeConvRate":
                    var ddlCustomer = document.getElementById("<%=ddlCustomer.ClientID%>");
                    var codeid = ddlCustomer.options[ddlCustomer.selectedIndex].value;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value;

                    var ddlCurrency = document.getElementById("<%=ddlCurrency.ClientID%>");
                    var CurrCodeId = ddlCurrency.options[ddlCurrency.selectedIndex].value;
                    var hdnCurrencyCode = document.getElementById("<%=hdnCurrencyCode.ClientID%>");
                    hdnCurrencyCode.value = ddlCurrency.options[ddlCurrency.selectedIndex].value;

                    //if (codeid != '[Select]') {
                    //ColServices.clsServices.GetExchRate4Reservation(constr, codeid, SetCurrency, ErrorHandler, TimeOutHandler);
                    //}

                    if (codeid != '[Select]' && CurrCodeId != '[Select]') {
                        ColServices.clsServices.GetExchRate4Currencynew(constr, codeid, CurrCodeId, SetCurrency, ErrorHandler, TimeOutHandler);
                    }

                    break;


                case "SetMarketCode":
                    var ddlMarket = document.getElementById("<%=ddlMarket.ClientID%>");
                    var hdnMarketCode = document.getElementById("<%=hdnMarketCode.ClientID%>");
                    hdnMarketCode.value = ddlMarket.options[ddlMarket.selectedIndex].value;
                    break;

                case "SetSellingTypeCode":
                    var ddlSellingType = document.getElementById("<%=ddlSellingType.ClientID%>");
                    var hdnSellingTypeCode = document.getElementById("<%=hdnSellingTypeCode.ClientID%>");
                    hdnSellingTypeCode.value = ddlSellingType.options[ddlSellingType.selectedIndex].value;
                    break;


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



        function FillMarket(result) {

            if (result != null) {
                var ddlMarket = document.getElementById("<%=ddlMarket.ClientID%>");
                var hdnMarketCode = document.getElementById("<%=hdnMarketCode.ClientID%>");
                ddlMarket.value = result;
                hdnMarketCode.value = result;
            }
        }

        function FillSellingType(result) {

            if (result != null) {
                var ddlSellingType = document.getElementById("<%=ddlSellingType.ClientID%>");
                var hdnSellingTypeCode = document.getElementById("<%=hdnSellingTypeCode.ClientID%>");
                ddlSellingType.value = result;
                hdnSellingTypeCode.value = result;
            }

        }

        function FillCurrency(result) {

            if (result != null) {
                var ddlCurrency = document.getElementById("<%=ddlCurrency.ClientID%>");
                var hdnCurrencyCode = document.getElementById("<%=hdnCurrencyCode.ClientID%>");
                ddlCurrency.value = result;
                hdnCurrencyCode.value = result;
                BaseCurrency(ddlCurrency);
            }
        }

        function setconceirge(result) {

            if (result != null) {
                var ddlConcierge = document.getElementById("<%=ddlConcierge.ClientID%>");
                //'var hdnCurrencyCode = document.getElementById("<%=hdnCurrencyCode.ClientID%>");
                ddlConcierge.value = result;
                //hdnCurrencyCode.value = result;

            }

            if (result == null || result == "") {

                ddlConcierge.value = "[Select]"
            }
        }

        function setsalesexpt(result) {

            if (result != null) {
                var ddlSalesExpert = document.getElementById("<%=ddlSalesExpert.ClientID%>");
                //'var hdnCurrencyCode = document.getElementById("<%=hdnCurrencyCode.ClientID%>");
                ddlSalesExpert.value = result;
                //hdnCurrencyCode.value = result;

            }

            if (result == null || result == "") {

                ddlSalesExpert.value = "[Select]"
            }
        }




        function SetCurrency(result) {
            var txtConvRate = document.getElementById("<%=txtConvRate.ClientID%>");
            var hdnAgentConvRate = document.getElementById("<%=hdnAgentConvRate.ClientID%>");
            if (result != null && result != NaN) {
                txtConvRate.value = result;
            }
            else {
                txtConvRate.value = 0;
            }
            hdnAgentConvRate.value = txtConvRate.value
        }


        function OnlyNumber(evt) {

            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
    ((evt.which) ? evt.which : 0));

            if (charCode != 47 && (charCode > 45 && charCode < 58)) {
                return true;
            }
            if (charCode == 8) {
                return true;
            }

            return false;

        }

        function alphanumeric(evt) {

            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
    ((evt.which) ? evt.which : 0));

            if (charCode != 47 && (charCode > 45 && charCode < 58)) {
                return true;
            }
            if (charCode == 8) {
                return true;
            }

            if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)) {
                return true;
            }
            return false;

        }

        function showcollectamt() {
            var txtamtcollect = document.getElementById("<%=txtcollectamt.ClientID%>");
            var lblamtcollect = document.getElementById("<%=lblcollectamt.ClientID%>");
            var ddlPayment = document.getElementById("<%=ddlPayment.ClientID%>");

            var codeid = ddlPayment.options[ddlPayment.selectedIndex].value;
            if (codeid == "COL") {
                txtamtcollect.style.display = "block";
                lblamtcollect.style.display = "block";
            }
            else {

                txtamtcollect.style.display = "none";
                lblamtcollect.style.display = "none";
            }


        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);



                function InitializeRequest(sender, args) {

                }

                function EndRequest(sender, args) {
                    // after update occur on UpdatePanel re-init the Autocomplete
                    MyAutoCustomer();
                }
            </script>

            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="divProgress" style="position: fixed; width: 100%; height: 100%; left: 0;
                        right: 0; top: 0; bottom: 0; z-index: 9999; background-color: #222; opacity: 0.8;">
                        <div style="display: block; position:absolute; vertical-align: middle; width: 100%; height: 100%; left: 40%;
                            top: 50%;">
                            <img src="../Images/loader.gif" height="30" width="30" /><span style="margin-left: 20px;
                                color: White; margin-top: 5px; position: absolute;">Please wait while we are processing
                                your request..</span>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <table style="border: gray 2px solid; width: 700px">
                <tbody>
                    <tr>
                        <td class="field_heading" colspan="4" align="center">
                            <asp:Label ID="lblHeading" runat="server" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Excursion ID
                        </td>
                        <td>
                            <asp:TextBox ID="txtExcursionID" runat="server" Enabled="false" CssClass="field_input"
                                Width="150px" />
                        </td>
                        <td class="td_cell">
                            Date
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="txtbox" TabIndex="6" ValidationGroup="MKE"
                                Width="80px" />
                            <asp:ImageButton ID="ImgBtnDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" />
                            <cc1:MaskedEditValidator ID="MEVDate" runat="server" ControlExtender="MEEDate" ControlToValidate="txtDate"
                                CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required"
                                ErrorMessage="" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" Width="23px">
    
                            </cc1:MaskedEditValidator>
                            <cc1:MaskedEditExtender ID="MEEDate" TargetControlID="txtDate" runat="server" AcceptNegative="Left"
                                DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date"
                                MessageValidatorTip="true">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CLFDate" TargetControlID="txtDate" PopupButtonID="ImgBtnDate"
                                runat="server" Format="dd/MM/yyyy" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Ticket No
                        </td>
                        <td>
                            <asp:TextBox ID="txtTicketNo" runat="server" MaxLength="30" CssClass="field_input"
                                Width="195px" />
                        </td>
                        <td class="td_cell">
                            User
                        </td>
                        <td>
                            <asp:TextBox ID="txtUser" ReadOnly="true" runat="server" CssClass="field_input" Width="195px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Customer
                        </td>
                        <td>
                            <input id="txtAgent" type="text" class="field_input MyAutoCompleteClass" style="width: 195px"
                                runat="server" /><br />
                            <select style="width: 200px" id="ddlCustomer" class="field_input MyDropDownListCustValue"
                                onchange="CallWebMethod('FillAgentDetails')" runat="server">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell">
                            Market
                        </td>
                        <td>
                            <select style="width: 200px" id="ddlMarket" class="field_input" runat="server" onchange="CallWebMethod('SetMarketCode')">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Selling Type
                        </td>
                        <td>
                            <select style="width: 200px" id="ddlSellingType" class="field_input" runat="server"
                                onchange="CallWebMethod('SetSellingTypeCode')">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell">
                            Currency
                        </td>
                        <td>
                            <select style="width: 200px" id="ddlCurrency" onchange="CallWebMethod('ChangeConvRate')"
                                class="field_input" runat="server">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Conversion Rate
                        </td>
                        <td>
                            <asp:TextBox ID="txtConvRate" ReadOnly="true" runat="server" CssClass="field_input"
                                Width="150px" />
                        </td>
                        <td class="td_cell">
                            <asp:Label ID="lbloperator" Style="display: none" runat="server" Text="Operator" />
                        </td>
                        <td>
                            <!--<input id="txtoperator" type="text" class="field_input MyAutoCompleteClass" style="width:195px;display:none" runat="server" /><br />-->
                            <select style="width: 200px; display: none" id="touroperator" class="field_input "
                                runat="server">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Payment Mode
                        </td>
                        <td>
                            <select style="width: 200px" id="ddlPayment" class="field_input" runat="server" onchange="CallWebMethod('ShowTextBox');showcollectamt()">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell">
                            <asp:Label ID="lblCreditCardNo" Style="display: none" runat="server" Text="Credit Card No" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreditCardNo" Style="display: none" runat="server" CssClass="field_input"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Concierge
                        </td>
                        <td>
                            <select style="width: 200px" id="ddlConcierge" class="field_input" runat="server">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell">
                            Sales Expert
                        </td>
                        <td>
                            <select style="width: 200px" id="ddlSalesExpert" class="field_input" runat="server">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Language
                        </td>
                        <td>
                            <select style="width: 200px" id="ddlLanguage" class="field_input" runat="server">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell">
                            <asp:Label ID="lblcollectamt" Style="display: none" runat="server" Text="Collect Amount" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtcollectamt" Style="display: none" runat="server" CssClass="field_input"
                                Width="150px" />
                        </td>
                        <td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Invoice No
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceNo" ReadOnly="true" runat="server" CssClass="field_input"
                                Width="150px" />
                        </td>
                        <td class="td_cell">
                            Credit Note No
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreditNoteNo" ReadOnly="true" runat="server" CssClass="field_input"
                                Width="150px" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <cc1:TabContainer runat="server" ActiveTabIndex="0">
                <cc1:TabPanel HeaderText="Booking Posting" ID="BookingTabPanel" runat="server">
                    <ContentTemplate>
                        <div id="Wrapper" runat="server" style="width: 900px; overflow: scroll">
                            <asp:GridView ID="gvExcursionRequest" runat="server" Width="800px" Font-Size="10px"
                                CssClass="td_cell " GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"
                                BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True"
                                BackColor="White" DataKeyNames="tourdate,othtypcode,othtypname,guestname,adults,child,rateadults,ratechild,amount,amountAED,amend,cancel,costRateAdult,costRateChild,costAmount,costAmountAED,cancelReason,complimentcust,complimentprov,comReduceAdultPer,comReduceChildPer,comReduceAmount,commpayperc,commpayamount,hotel,roomNo,providerCode,attn,conf,confno,flighttype,flightno,flighttime,airport,pickuptime,remarks,arrival,departure,locked,spersoncomm,partycode,guide,trf_required,trf_amount,trf_supplier,total_amount,incominginvno,debitnoteno,confirmed,exctime,supconf,trfno,supconfno,protktno"
                                PageSize="25">
                                <Columns>
                                    <asp:TemplateField Visible="False" HeaderText="LineNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lbRLineNo" runat="server" Text='<%# Bind("rlineno") %>'></asp:Label>
                                            <asp:Label ID="lblRowID" runat="server"></asp:Label>
                                            <asp:Label ID="lblAmend" runat="server" Text='<%# Bind("amend") %>'></asp:Label>
                                            <asp:Label ID="lblCancel" runat="server" Text='<%# Bind("cancel") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#06788B" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Copy">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkselect" runat="server" class="field_input" Width="50px"></asp:CheckBox>
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#06788B" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" CommandName="AddRow" Text="Select" CommandArgument='<%#Eval("rlineno") %>'
                                                runat="server" class="field_input" Width="50px"></asp:Button>
                                            <asp:HiddenField ID="hdnLineNo" runat="server" Value='<%#Eval("rlineno") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#06788B" />
                                    </asp:TemplateField>
                                    <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="tourdate"
                                        HeaderText="Tour Date">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="othtypcode" SortExpression="othtypcode" HeaderText="Exc Code">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="othtypname" SortExpression="othtypname" HeaderText="Exc Name">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="guestname" SortExpression="guestname" HeaderText="Guest Name">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="adults" SortExpression="adults" HeaderText="Adults">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="child" SortExpression="child" HeaderText="Child">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="rateadults" SortExpression="rateadults" HeaderText="Rate Adults">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ratechild" SortExpression="ratechild" HeaderText="Rate Child">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="amount" SortExpression="amount" HeaderText="Amount">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="amountAED" SortExpression="amountAED" HeaderText="Amount AED">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="amend" SortExpression="amend" HeaderText="Amend">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cancel" SortExpression="cancel" HeaderText="Cancel">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="costRateAdult" SortExpression="costRateAdult" HeaderText="Cost Rate Adult">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="costRateChild" SortExpression="costRateChild" HeaderText="Cost Rate Child">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="costAmount" SortExpression="costAmount" HeaderText="Cost Amount">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="costAmountAED" SortExpression="costAmountAED" HeaderText="Cost Amount AED">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cancelReason" SortExpression="cancelReason" HeaderText="Cancel Reason">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="complimentcust" SortExpression="complimentcust" HeaderText="Complimentary to Customer">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="complimentprov" SortExpression="complimentprov" HeaderText="Complimentary to Supplier">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="comReduceAdultPer" SortExpression="comReduceAdultPer"
                                        HeaderText="Commission Recd Adult %">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="comReduceChildPer" SortExpression="comReduceChildPer"
                                        HeaderText="Commission Recd Child %">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="comReduceAmount" SortExpression="comReduceAmount" HeaderText="Commission Recd Amount %">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="commpayperc" SortExpression="commpayperc" HeaderText="Commission Pay">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="commpayamount" SortExpression="commpayamount" HeaderText="Commission Pay Amount">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="hotel" SortExpression="hotel" HeaderText="Hotel">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="roomNo" SortExpression="roomNo" HeaderText="Room No">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="providerCode" SortExpression="providerCode" HeaderText="Provide Code">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="providerCode" SortExpression="providerCode" HeaderText="Provide Name">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="attn" SortExpression="attn" HeaderText="Attn">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="conf" SortExpression="conf" HeaderText="Conf">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="confno" SortExpression="confno" HeaderText="Conf No">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="flighttype" SortExpression="flighttype" HeaderText="Flight Type">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="flightno" SortExpression="flightno" HeaderText="Flight No">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="flighttime" SortExpression="flighttime" HeaderText="Flight Time">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="airport" SortExpression="airport" HeaderText="Airport">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pickuptime" SortExpression="pickuptime" HeaderText="Pick Up Time">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="remarks" SortExpression="remarks" HeaderText="Remarks">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="arrival" DataFormatString="{00:dd/MM/yyyy}" SortExpression="arrival"
                                        HeaderText="Arrival">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="departure" DataFormatString="{00:dd/MM/yyyy}" SortExpression="departure"
                                        HeaderText="Departure">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="locked" SortExpression="locked" HeaderText="Locked">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="spersoncomm" SortExpression="spersoncomm" HeaderText="Sperson Commant">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="partycode" SortExpression="partycode" HeaderText="Party Code">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="guide" SortExpression="guide" HeaderText="Guide">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="trf_required" SortExpression="trf_required" HeaderText="Transfered Required">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="trf_amount" SortExpression="trf_amount" HeaderText="Transfered Amount">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="trf_supplier" SortExpression="trf_supplier" HeaderText="Transfered Supplier">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="total_amount" SortExpression="total_amount" HeaderText="Total Amount">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="incominginvno" SortExpression="incominginvno" HeaderText="Incoming Invoice No">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="debitnoteno" SortExpression="debitnoteno" HeaderText="Debit Note No">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="confirmed" SortExpression="confirmed" HeaderText="Confirmed">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="supconf" SortExpression="supconf" HeaderText="Sup.Conf">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="exctime" SortExpression="exctime" HeaderText="Exc.Time">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="supconfno" SortExpression="supconfno" HeaderText="Sup.Conf No.">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="protktno" SortExpression="protktno" HeaderText="Provider Ticket No.">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="reminderdt" SortExpression="reminderdt" HeaderText="Reminder Date.">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                <AlternatingRowStyle BackColor="Transparent" Font-Size="10px"></AlternatingRowStyle>
                            </asp:GridView>
                            <table style="float: left">
                                <tr>
                                    <td class="td_cell">
                                        Total Sale Amount
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalSaleAmount" ReadOnly="True" Text="0" runat="server" CssClass="field_input TextBoxRightAlign"
                                            Width="80px" />
                                    </td>
                                    <td class="td_cell">
                                        Total Sale Amount(AED)
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalSaleAmountAED" ReadOnly="True" Text="0" runat="server" CssClass="field_input TextBoxRightAlign"
                                            Width="80px" />
                                    </td>
                                    <td class="td_cell">
                                        Total Cost Amount
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalCostAmount" ReadOnly="True" Text="0" runat="server" CssClass="field_input TextBoxRightAlign"
                                            Width="80px" />
                                    </td>
                                    <td class="td_cell">
                                        Total Cost Amount(AED)
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalCostAmountAED" ReadOnly="True" Text="0" runat="server" CssClass="field_input TextBoxRightAlign"
                                            Width="80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <span style="width: 20px; height: 10px; background-color: #FF934A">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        <asp:Label ID="lblAmendColor" Text="Amend" runat="server" />
                                        <span style="width: 20px; height: 10px; background-color: #ff3300">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        <asp:Label ID="lblCancel" Text="Cancel" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel HeaderText="Accounts Posting" ID="AccountTabPanel" runat="server">
                </cc1:TabPanel>
            </cc1:TabContainer>
            <asp:Button ID="btncopy" runat="server" CssClass="btn" Font-Bold="False" Text="Copy Line"
                Width="80px" />&nbsp;
            <asp:Button ID="btnSave" Text="Final Save" CssClass="field_button" runat="server"
                Width="100px" />&nbsp;
            <asp:Button ID="btnExit" Text="Exit" CssClass="field_button" runat="server" Width="50px" />&nbsp;
            <asp:Button ID="Button1" Text="Check_PostBack" CssClass="field_button" runat="server"
                Visible="false" Width="50px" />
            <asp:HiddenField ID="txtconnection" runat="server" />
            <asp:HiddenField ID="hdnPaymentMode" runat="server" />
            <asp:HiddenField ID="hdnAgentConvRate" runat="server" />
            <asp:HiddenField ID="hdnExcursionID" runat="server" />
            <asp:HiddenField ID="hdnMarketCode" runat="server" />
            <asp:HiddenField ID="hdnSellingTypeCode" runat="server" />
            <asp:HiddenField ID="hdnCurrencyCode" runat="server" />
            <asp:HiddenField ID="txtdecimal" runat="server" />
            <asp:HiddenField ID="hdntouroperator" runat="server" />
            <asp:HiddenField ID="hdntouroptemp" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
