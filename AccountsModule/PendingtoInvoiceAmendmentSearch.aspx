<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="PendingtoInvoiceAmendment.aspx.vb" Inherits="PendingtoInvoiceAmendment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">

        function CallWebMethod(methodType) {
            switch (methodType) {
                case "ddlCustomerCode":
                    var select = document.getElementById("<%=ddlCustomerCode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlCustomerName.ClientID%>");
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value
                    selectname.value = select.options[select.selectedIndex].text;
                    ColServices.clsServices.GetCustSubCodeListnew(constr, codeid, FillCustSubCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustSubNameListnew(constr, codeid, FillCustSubNames, ErrorHandler, TimeOutHandler);
                    break;
                case "ddlCustomerName":
                    var select = document.getElementById("<%=ddlCustomerName.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlCustomerCode.ClientID%>");
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value
                    selectname.value = select.options[select.selectedIndex].text;
                    ColServices.clsServices.GetCustSubCodeListnew(constr, codeid, FillCustSubCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustSubNameListnew(constr, codeid, FillCustSubNames, ErrorHandler, TimeOutHandler);
                    break;
                case "ddlSupplierCode":
                    var select = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplierName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSupplierName":
                    var select = document.getElementById("<%=ddlSupplierName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSupplierAgentCode":
                    var select = document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplerAgentName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSupplerAgentName":
                    var select = document.getElementById("<%=ddlSupplerAgentName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSubUserCode":
                    var select = document.getElementById("<%=ddlSubUserCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSubUserName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSubUserName":
                    var select = document.getElementById("<%=ddlSubUserName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSubUserCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlUserCode":
                    var select = document.getElementById("<%=ddlUserCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlUserName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlUserName":
                    var select = document.getElementById("<%=ddlUserName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlUserCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }
        function FillCustSubCodes(result) {
            var ddl = document.getElementById("<%=ddlSubUserCode.ClientID%>");
            for (var j = ddl.length - 1; j >= 0; j--) {
                ddl.remove(j);
            }
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillCustSubNames(result) {
            var ddl = document.getElementById("<%=ddlSubUserName.ClientID%>");
            for (var j = ddl.length - 1; j >= 0; j--) {
                ddl.remove(j);
            }
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
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
        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }

        function FormValidation() {
            var img = document.getElementById("<%=imgicon.CLientID %>");
            if (document.getElementById("<%=txtfrmCheckin.ClientID%>").value == "") {
                document.getElementById("<%=txtfrmCheckin.ClientID%>").focus();
                alert("Please Enter From Checkin Date");
                return false;
            }


            if (document.getElementById("<%=txttocheckin.ClientID%>").value == "") {
                document.getElementById("<%=txttoCheckin.ClientID%>").focus();
                alert("Please Enter To Checkin Date");
                return false;
            }


            img.style.visibility = "visible";
            return true;
        }


        var txtchk;

        function hiddenvalchng() {
            var invoice = confirm('Not Ready for Invoicing,Do You want to continue with the rest ?');
            if (invoice == 1) {

                var btn = document.getElementById("<%=btndummy.ClientID%>")
                btn.click();

            }
            else {
                // window.open('ReservationInvoiceSearch.aspx','ReservationWindowPostBack','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');
                window.opener.__doPostBack('ReservationWindowPostBack', ''); window.opener.focus(); window.close();
            }




        }

        function chargedcanceltype() {
            var invoice = confirm('Are you sure to Cancel selected invoices without charge?');
            if (invoice == 1) {

                var btn = document.getElementById("<%=btndmyRemvInv.ClientID%>")
                btn.click();

            }
            else {


            }

        }



        function invoicevalidate() {
            var btn = document.getElementById("<%=btngenerateinvoice.ClientID%>")
            btn.style.visibility = "hidden";

            return true;
        }
   
    </script>
    <%--<center>
             <asp:Label ID="lblHeadingHTL" runat="server" CssClass="field_heading" 
               Text=" Pending to Invoice after amendment " Width="100%"></asp:Label>
               </center>
     <br />  --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                border-bottom: gray 1px solid" class="td_cell">
                <tbody>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <asp:Label ID="lblHeading" runat="server" Text="Pending to Invoice after amendment"
                                Width="100%" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px">
                            File Number
                        </td>
                        <td colspan="6">
                            <table>
                                <tr>
                                    <td>
                                        <input id="txtRequestId" class="txtbox" style="width: 184px" type="text" runat="server" />
                                    </td>
                                    <td>
                                        From&nbsp;Check&nbsp;In&nbsp;Date
                                    </td>
                                    <td style="width: 257px">
                                        <table>
                                            <tr>
                                                <td class="style8" style="text-align: left">
                                                    <asp:TextBox ID="txtfrmCheckin" runat="server" class="field_input_agent" CssClass="field_input"
                                                        TabIndex="5" Width="70px" ValidationGroup="MKE"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtn2"
                                                        TargetControlID="txtfrmCheckin">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtfrmCheckin">
                                                    </cc1:MaskedEditExtender>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:ImageButton ID="imgbtn2" runat="Server" AlternateText="Click to show calendar"
                                                        ImageUrl="~/Content/images/calendar.png" />
                                                    <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                                        ControlToValidate="txtfrmCheckin" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                        EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="dd/mm/yyyy" ValidationGroup="MKE"
                                                        Width="88px"></cc1:MaskedEditValidator>
                                                </td>
                                                <td>
                                                    To&nbsp;Check&nbsp;In&nbsp;Date
                                                </td>
                                                <td class="style8" style="text-align: left">
                                                    <asp:TextBox ID="txttoCheckin" runat="server" class="field_input_agent" CssClass="field_input"
                                                        TabIndex="5" Width="70px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton1"
                                                        TargetControlID="txttoCheckin">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txttoCheckin">
                                                    </cc1:MaskedEditExtender>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:ImageButton ID="ImageButton1" runat="Server" AlternateText="Click to show calendar"
                                                        ImageUrl="~/Content/images/calendar.png" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px">
                            Customer
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 356px">
                                        <input id="accSearch" runat="server" class="field_input MyAutoCompleteClass" name="accSearch"
                                            onfocus="MyAutoCustomer_rptFillArray();" style="width: 98%; font" type="text" />
                                    </td>
                                </tr>
                            </table>
                            <select style="width: 136px" id="ddlCustomerCode" class="drpdown" tabindex="7" onchange="CallWebMethod('ddlCustomerCode');"
                                runat="server">
                            </select>
                            <select style="width: 220px" id="ddlCustomerName" class="drpdown MyDropDownListCustValue"
                                tabindex="8" onchange="CallWebMethod('ddlCustomerName');" runat="server">
                                <option selected></option>
                            </select>
                        </td>
                        <td>
                            User
                        </td>
                        <td lang=" ">
                            <select style="width: 136px" id="ddlUserCode" class="drpdown" tabindex="5" onchange="CallWebMethod('ddlUserCode');"
                                runat="server">
                                <option selected></option>
                            </select>
                            <select style="width: 220px" id="ddlUserName" class="drpdown" tabindex="6" onchange="CallWebMethod('ddlUserName');"
                                runat="server">
                                <option selected></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px">
                            Supplier
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 356px">
                                        <input id="suppsearch" runat="server" class="field_input MyAutosupplierCompleteClass"
                                            name="suppSearch" onfocus="MyAutosupplier_rptFillArray();" style="width: 98%;
                                            font" type="text" />
                                    </td>
                                </tr>
                            </table>
                            <select style="width: 136px" id="ddlSupplierCode" class="drpdown" tabindex="9" onchange="CallWebMethod('ddlSupplierCode');"
                                runat="server">
                                <option selected></option>
                            </select>
                            <select style="width: 220px" id="ddlSupplierName" class="drpdown MyDropDownListsuppValue"
                                tabindex="10" onchange="CallWebMethod('ddlSupplierName');" runat="server">
                                <option selected></option>
                            </select>
                        </td>
                        <td>
                            Supplier Agnet
                        </td>
                        <td>
                            <select style="width: 136px" id="ddlSupplierAgentCode" class="drpdown" tabindex="11"
                                onchange="CallWebMethod('ddlSupplierAgentCode');" runat="server">
                                <option selected></option>
                            </select>
                            <select style="width: 220px" id="ddlSupplerAgentName" class="drpdown" tabindex="12"
                                onchange="CallWebMethod('ddlSupplerAgentName');" runat="server">
                                <option selected></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px">
                            Sub User Code
                        </td>
                        <td>
                            <select style="width: 136px" id="ddlSubUserCode" class="drpdown" tabindex="13" onchange="CallWebMethod('ddlSubUserCode');"
                                runat="server">
                                <option selected></option>
                            </select>
                            <select style="width: 220px" id="ddlSubUserName" class="drpdown" tabindex="14" onchange="CallWebMethod('ddlSubUserName');"
                                runat="server">
                                <option selected></option>
                            </select>
                        </td>
                        <td>
                            Customer Ref
                        </td>
                        <td>
                            <input style="width: 176px" id="txtCustRef" class="txtbox" tabindex="15" onkeypress="return checkNumber(event);"
                                type="text" maxlength="20" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px; height: 23px">
                            Guest First Name
                        </td>
                        <td style="height: 23px">
                            <input style="width: 176px" id="txtFirstName" class="txtbox" tabindex="16" type="text"
                                maxlength="20" runat="server" />
                        </td>
                        <td style="height: 23px">
                            Guest&nbsp;Last&nbsp;Name
                        </td>
                        <td style="height: 23px">
                            <input style="width: 176px" id="txtLastName" class="txtbox" tabindex="17" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 111px; height: 23px">
                            Booking Type
                        </td>
                        <td style="height: 23px">
                            <select style="width: 136px" id="ddlBookingType" class="drpdown" tabindex="13" runat="server">
                                <option value="A">All</option>
                                <option value="H">Hotel</option>
                                <option value="T">Transfer</option>
                                <option value="E">Excursion</option>
                            </select>
                        </td>
                        <td style="height: 23px">
                        </td>
                        <td style="height: 23px">
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 106px">
                            From&nbsp;Check&nbsp;Out&nbsp;Date
                        </td>
                        <td style="text-align: left" colspan="6">
                            <table>
                                <tr>
                                    <td class="style8" style="text-align: left">
                                        <asp:TextBox ID="txtfrmCheckout" runat="server" class="field_input_agent" CssClass="field_input"
                                            TabIndex="5" Width="70px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton2"
                                            TargetControlID="txtfrmCheckout">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtfrmCheckout">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImageButton2" runat="Server" AlternateText="Click to show calendar"
                                            ImageUrl="~/Content/images/calendar.png" />
                                    </td>
                                    <td>
                                        To&nbsp;Check&nbsp;Out&nbsp;Date
                                    </td>
                                    <td class="style8" style="text-align: left">
                                        <asp:TextBox ID="txttoCheckout" runat="server" class="field_input_agent" CssClass="field_input"
                                            TabIndex="5" Width="70px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton3"
                                            TargetControlID="txttoCheckout">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txttoCheckout">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImageButton3" runat="Server" AlternateText="Click to show calendar"
                                            ImageUrl="~/Content/images/calendar.png" />
                                    </td>
                                    <td>
                                        From&nbsp;Request&nbsp;Date
                                    </td>
                                    <td class="style8" style="text-align: left">
                                        <asp:TextBox ID="txtfrmrequestdate" runat="server" class="field_input_agent" CssClass="field_input"
                                            TabIndex="5" Width="70px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton4"
                                            TargetControlID="txtfrmrequestdate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtfrmrequestdate">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="ImageButton4" runat="Server" AlternateText="Click to show calendar"
                                            ImageUrl="~/Content/images/calendar.png" />
                                    </td>
                                    <td>
                                        To Request Date
                                    </td>
                                    <td>
                                        <td class="style8" style="text-align: left">
                                            <asp:TextBox ID="txttorequestdate" runat="server" class="field_input_agent" CssClass="field_input"
                                                TabIndex="5" Width="70px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton5"
                                                TargetControlID="txttorequestdate">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txttorequestdate">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:ImageButton ID="ImageButton5" runat="Server" AlternateText="Click to show calendar"
                                                ImageUrl="~/Content/images/calendar.png" />
                                        </td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 111px">
                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                height: 9px" type="text" />
                        </td>
                        <td style="width: 365px">
                        </td>
                        <td>
                            <asp:Button ID="btnDisplay" runat="server" CssClass="btn" Text=" Display Pending for Invoicing"
                                Width="200px" />
                        </td>
                        <td style="width: 415px">
                            <asp:Button ID="btnCancel" runat="server" Text="Return" CssClass="btn" Width="118px">
                            </asp:Button>&nbsp;<asp:Button ID="btnHelp" OnClick="btnHelp_Click" runat="server"
                                Text="Help" CssClass="btn"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="4">
                            <img id="imgicon" height="25" src="../Images/loading.gif" width="400" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table class="td_cell">
                <tbody>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <asp:Label ID="lblAReady" runat="server" CssClass="field_heading" Text="Amendments Ready to Invoice"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <asp:Panel ID="divRes" runat="server" Height="200px" ScrollBars="Both">
                                <br />
                                <asp:GridView ID="grdAmendmentReady" DataKeyNames="customer" runat="server" CssClass="grdstyle"
                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="100">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <input id="chkSel" runat="server" type="checkbox" class="field_input" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RequestId">
                                            <ItemStyle Width="75px" Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="75px" Wrap="True"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblReqid" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "requestid") %>'
                                                    Width="65px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Wrap="True"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No">
                                            <ItemStyle Width="65px" Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="65px" Wrap="True"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvNo" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "invno") %>'
                                                    Width="25px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Wrap="True"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemStyle Width="50px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="amended" HeaderText="Amended">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataFormatString=" {00:dd/MM/yyyy}" DataField="arrivaldate" HeaderText="Arrival Date">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="agentref" HeaderText="Customer Ref">
                                            <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle Width="50px" HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="customer" HeaderText="Customer">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="guestname" HeaderText="GuestName">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="usercode" HeaderText="Sales Person">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="market" HeaderText="Market">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sellcode" HeaderText="Sell Type">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Preview Invoice">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkpreviewinvoice" runat="server" OnClick="lnkpreviewinvoice_Click"
                                                    CommandArgument='<%# Ctype(Container,GridViewRow).RowIndex %>'>Preview Invoice</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="grdRowstyle" Wrap="False"></RowStyle>
                                    <PagerStyle CssClass="grdpagerstyle" Wrap="False"></PagerStyle>
                                    <HeaderStyle CssClass="grdheader" Wrap="False"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                </asp:GridView>
                                <asp:Label ID="lblMessage" runat="server" Font-Size="9pt" Font-Names="Verdana" Font-Bold="True"
                                    Visible="False" CssClass="lblmsg">Records Not Found.</asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
                                <center>
                                    <asp:GridView ID="grdInvError" runat="server" Visible="False" AutoGenerateColumns="false"
                                        CssClass="grdheader">
                                        <Columns>
                                            <asp:BoundField HeaderText="Service Description" DataField="servicedescription">
                                                <HeaderStyle Wrap="true" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Error Message" DataField="errmessage">
                                                <HeaderStyle Wrap="true" />
                                            </asp:BoundField>
                                              <asp:BoundField DataField="requestid" HeaderText ="Request Id">
                                        <HeaderStyle Wrap="true" />
                                        </asp:BoundField>

                                        </Columns>
                                        <RowStyle CssClass="grdstyle1_agent" />
                                        <AlternatingRowStyle CssClass="grdstyle1_agent" />
                                    </asp:GridView>
                                </center>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <br />
                            <asp:Button ID="Btnselectall" runat="server" CssClass="btn" TabIndex="12" Text="SelectAll"
                                Visible="False" />
                            &nbsp;
                            <asp:Button ID="btnunselect" runat="server" CssClass="btn" TabIndex="12" Text="UnSelectAll"
                                Visible="False" />
                            &nbsp;
                            <asp:Button ID="btngenerateinvoice" runat="server" CssClass="btn" TabIndex="12" Text="Generate Invoice for Selected"
                                Visible="False" />
                            &nbsp;
                            <asp:Button ID="btndummy" Style="display: none" OnClick="btndummy_Click" runat="server"
                                CssClass="btn" TabIndex="12" Text="Gendummy" Width="34px" />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <asp:Label ID="lblCancelH" runat="server" CssClass="field_heading" Text="Cancellations After Invoice"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <asp:Panel ID="pnlCanc" runat="server" Height="200px" ScrollBars="Auto">
                                <br />
                                <asp:GridView ID="grdCancelInv" DataKeyNames="customer,CancelType" runat="server"
                                    CssClass="grdstyle" AutoGenerateColumns="False" AllowPaging="true" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <input id="chkSel" runat="server" type="checkbox" class="field_input" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RequestId">
                                            <ItemStyle Width="65px" Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="65px" Wrap="True"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblReqid" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "requestid") %>'
                                                    Width="25px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Wrap="True"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No">
                                            <ItemStyle Width="65px" Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="65px" Wrap="True"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvNo" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "invno") %>'
                                                    Width="25px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Wrap="True"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemStyle Width="50px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="amended" HeaderText="Amended">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataFormatString=" {00:dd/MM/yyyy}" DataField="arrivaldate" HeaderText="Arrival Date">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="agentref" HeaderText="Customer Ref">
                                            <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle Width="50px" HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="customer" HeaderText="Customer">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="guestname" HeaderText="GuestName">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="usercode" HeaderText="Sales Person">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="market" HeaderText="Market">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sellcode" HeaderText="Sell Type">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CancelType" HeaderText="Cancellation Type">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <%--<asp:TemplateField HeaderText="Preview Invoice">
              <ItemTemplate>
               <asp:LinkButton ID="lnkpreviewinvoice" runat="server" OnClick="lnkpreviewinvoice_Click"   CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>' >Preview Invoice</asp:LinkButton>
               </ItemTemplate>
              </asp:TemplateField>--%>
                                    </Columns>
                                    <RowStyle CssClass="grdRowstyle" Wrap="False"></RowStyle>
                                    <PagerStyle CssClass="grdpagerstyle" Wrap="False"></PagerStyle>
                                    <HeaderStyle CssClass="grdheader" Wrap="False"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                </asp:GridView>
                                <asp:Label ID="lblCancl" runat="server" Font-Size="9pt" Font-Names="Verdana" Font-Bold="True"
                                    Visible="False" CssClass="lblmsg">Records Not Found.</asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <%--<tr> <TD style="TEXT-ALIGN: center" colspan="6">
            <asp:Panel ID="Panel3"  runat="server" Width = "100%" ScrollBars ="Auto">
            <center>
      <asp:GridView ID="GridView2" runat="server" Visible="False"      AutoGenerateColumns ="false"
                                CssClass="grdheader">
                       
                        <Columns>
                            <asp:BoundField HeaderText="Service Description" DataField ="servicedescription"  >
                            <HeaderStyle Wrap ="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Error Message" DataField ="errmessage"  >
                             <HeaderStyle Wrap ="true" />
                            </asp:BoundField>
                        </Columns>
                       
                        <RowStyle CssClass="grdstyle1_agent" />
                        <AlternatingRowStyle CssClass="grdstyle1_agent" />
                    </asp:GridView>

                    </center>
                    </asp:Panel>
         </TD></tr>--%>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <br />
                            <asp:Button ID="btnSelectAllCancel" runat="server" CssClass="btn" TabIndex="12" Text="SelectAll"
                                Visible="False" />
                            &nbsp;
                            <asp:Button ID="btnUnSelectAllCancel" runat="server" CssClass="btn" TabIndex="12"
                                Text="UnSelectAll" Visible="False" />
                            &nbsp;
                            <asp:Button ID="btnRemoveInvoices" runat="server" CssClass="btn" TabIndex="12" Text="Cancel selected without charge "
                                Visible="False" />
                            &nbsp;
                            <asp:Button ID="btndmyRemvInv" Style="display: none" runat="server" CssClass="btn"
                                TabIndex="12" Text="Gendummy" Width="34px" />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="6">
                            <asp:Label ID="lblAPending" runat="server" CssClass="field_heading" Text="Amendments Pending Confirmation"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="divRes1" runat="server" ScrollBars="Auto">
                                <br />
                                <asp:GridView ID="grdAmendemntPending" DataKeyNames="customer" runat="server" CssClass="grdstyle"
                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="20">
                                    <Columns>
                                        <%--<asp:TemplateField>
                                  <ItemTemplate>
                                        <input id="chkSel" runat="server" type="checkbox" class="field_input" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="RequestId">
                                            <ItemStyle Width="65px" Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="65px" Wrap="True"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblReqid" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "requestid") %>'
                                                    Width="65px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Wrap="True"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No">
                                            <ItemStyle Width="65px" Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="65px" Wrap="True"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvNo" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "invno") %>'
                                                    Width="25px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Wrap="True"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemStyle Width="50px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="amended" HeaderText="Amended">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataFormatString=" {00:dd/MM/yyyy}" DataField="arrivaldate" HeaderText="Arrival Date">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="agentref" HeaderText="Customer Ref">
                                            <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle Width="50px" HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="customer" HeaderText="Customer">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="guestname" HeaderText="GuestName">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="usercode" HeaderText="Sales Person">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="market" HeaderText="Market">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sellcode" HeaderText="Sell Type">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <%--<asp:TemplateField HeaderText="Preview Invoice">


                  <ItemTemplate>
               <asp:LinkButton ID="lnkpreviewinvoice" runat="server" OnClick="lnkpreviewinvoice_Click"   CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>' >Preview Invoice</asp:LinkButton>
               </ItemTemplate>
              </asp:TemplateField>  --%>
                                    </Columns>
                                    <RowStyle CssClass="grdRowstyle" Wrap="False"></RowStyle>
                                    <PagerStyle CssClass="grdpagerstyle" Wrap="False"></PagerStyle>
                                    <HeaderStyle CssClass="grdheader" Wrap="False"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
