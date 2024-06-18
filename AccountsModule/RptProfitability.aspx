<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptProfitability.aspx.vb" Inherits="AccountsModule_RptProfitability" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">

        function CallWebMethod(methodType) {

            switch (methodType) {

                case "plgrpcode":
                    var select = document.getElementById("<%=ddlmarketcode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlmarketname.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;

                    var selectname2 = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                    selectname2.value = select.options[select.selectedIndex].text;
                    var select1 = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                    select1.value = selectname2.options[selectname2.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value
                    /*
                    ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes1, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName1, ErrorHandler, TimeOutHandler);*/

                    break;
                case "plgrpname":
                    var select = document.getElementById("<%=ddlmarketname.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlmarketcode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var selectname1 = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                    selectname1.value = select.options[select.selectedIndex].text;
                    var select1 = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                    select1.value = selectname1.options[selectname1.selectedIndex].text;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value
                    /*
                    ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes1, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName1, ErrorHandler, TimeOutHandler);*/
                    1
                    break;
                case "plgrpcodeto":
                    var select = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "plgrpnameto":
                    var select = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;

                case "agentcode":
                    var select = document.getElementById("<%=ddlcustomercode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlcustomername.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var selectname2 = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                    selectname2.value = select.options[select.selectedIndex].text;
                    var select1 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                    select1.value = selectname2.options[selectname2.selectedIndex].text;
                    break;
                case "agentname":
                    var select = document.getElementById("<%=ddlcustomername.ClientID%>");
                    var selectname = document.getElementById("<%=ddlcustomercode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var selectname1 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                    selectname1.value = select.options[select.selectedIndex].text;
                    var select1 = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                    select1.value = selectname1.options[selectname1.selectedIndex].text;

                    break;
                case "agentcodeto":
                    var select = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "agentnameto":
                    var select = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;

                case "partycode":
                    var select = document.getElementById("<%=ddlsupcode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsupname.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var selectname2 = document.getElementById("<%=ddlsupnameto.ClientID%>");
                    selectname2.value = select.options[select.selectedIndex].text;
                    var select1 = document.getElementById("<%=ddlsupcodeto.ClientID%>");
                    select1.value = selectname2.options[selectname2.selectedIndex].text;

                    break;

                case "partyname":
                    var select = document.getElementById("<%=ddlsupname.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsupcode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var selectname1 = document.getElementById("<%=ddlsupcodeto.ClientID%>");
                    selectname1.value = select.options[select.selectedIndex].text;
                    var select1 = document.getElementById("<%=ddlsupnameto.ClientID%>");
                    select1.value = selectname1.options[selectname1.selectedIndex].text;

                    break;

                case "partycodeto":
                    var select = document.getElementById("<%=ddlsupcodeto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsupnameto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;

                case "partynameto":
                    var select = document.getElementById("<%=ddlsupnameto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsupcodeto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;

                case "acccodefrom":

                    var select = document.getElementById("<%=ddlacccodefrom.ClientID%>");
                    var selectname = document.getElementById("<%=ddlaccnamefrom.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var select2 = document.getElementById("<%=ddlacccodeto.clientID%>");
                    var selectname2 = document.getElementById("<%=ddlaccnameto.clientID%>");
                    selectname2.value = select.options[select.selectedIndex].text;
                    select2.value = selectname2.options[selectname2.selectedIndex].text;
                    break;

                case "accnamefrom":
                    var select = document.getElementById("<%=ddlacccodefrom.ClientID%>");
                    var selectname = document.getElementById("<%=ddlaccnamefrom.clientID%>");
                    select.value = selectname.options[selectname.selectedIndex].text;
                    var select2 = document.getElementById("<%=ddlacccodeto.clientID%>");
                    var selectname2 = document.getElementById("<%=ddlaccnameto.clientID%>");
                    select2.value = selectname.options[selectname.selectedIndex].text;
                    selectname2.value = select2.options[select2.selectedIndex].text;
                    break;

                case "acccodeto":
                    var select = document.getElementById("<%=ddlacccodeto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlaccnameto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    //    document.getElementById("<%=ddlaccnameto.ClientID%>").value=select.options[select.selectedIndex].text;  
                    break;
                case "accnameto":
                    var select = document.getElementById("<%=ddlaccnameto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlaccnameto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    //    document.getElementById("<%=ddlacccodeto.ClientID%>").value=select.options[select.selectedIndex].text;  
                    break;


                case "salescodefrom":

                    var select = document.getElementById("<%=ddlsalesfrom.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsalesnamefrom.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var select2 = document.getElementById("<%=ddlsalesto.clientID%>");
                    var selectname2 = document.getElementById("<%=ddlsalenameto.clientID%>");
                    selectname2.value = select.options[select.selectedIndex].text;
                    select2.value = selectname2.options[selectname2.selectedIndex].text;
                    break;



                case "salesnamefrom":
                    var select = document.getElementById("<%=ddlsalesfrom.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsalesnamefrom.clientID%>");
                    select.value = selectname.options[selectname.selectedIndex].text;
                    var select2 = document.getElementById("<%=ddlsalesto.clientID%>");
                    var selectname2 = document.getElementById("<%=ddlsalenameto.clientID%>");
                    select2.value = selectname.options[selectname.selectedIndex].text;
                    selectname2.value = select2.options[select2.selectedIndex].text;
                    break;

                case "salescodeto":
                    var select = document.getElementById("<%=ddlsalesto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsalenameto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    //    document.getElementById("<%=ddlaccnameto.ClientID%>").value=select.options[select.selectedIndex].text;  
                    break;


                case "salesnameto":
                    var select = document.getElementById("<%=ddlsalenameto.ClientID%>");
                    var selectname = document.getElementById("<%=ddlsalesto.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;

            }
        }

        //-------------function for dependency dropdown-----for customer----------------------------------------------------
        function FillCustomerCodes(result) {

            var ddl = document.getElementById("<%=ddlcustomercode .ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);

            }
            ddl.value = "[Select]";


        }

        function FillCustomerName(result) {
            var ddl = document.getElementById("<%=ddlcustomername.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);

            }
            ddl.value = "[Select]";

        }

        function FillCustomerCodes1(result) {
            var ddl1 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");

            RemoveAll(ddl1)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);

                ddl1.options.add(option);

            }

            ddl1.value = "[Select]";

        }

        function FillCustomerName1(result) {

            var ddl1 = document.getElementById("<%=ddlcustomernameto.ClientID%>");

            RemoveAll(ddl1)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);

                ddl1.options.add(option);

            }

            ddl1.value = "[Select]";
        }
        //--------------------for Supplier
        function FillSupplierCodes(result) {

            var ddl = document.getElementById("<%=ddlsupcode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);

            }
            ddl.value = "[Select]";


        }

        function FillSupplierName(result) {
            var ddl = document.getElementById("<%=ddlsupname.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);

            }
            ddl.value = "[Select]";

        }

        function FillSupplierCodes1(result) {
            var ddl1 = document.getElementById("<%=ddlsupcodeto.ClientID%>");

            RemoveAll(ddl1)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);

                ddl1.options.add(option);

            }

            ddl1.value = "[Select]";

        }

        function FillSupplierName1(result) {

            var ddl1 = document.getElementById("<%=ddlsupnameto.ClientID%>");

            RemoveAll(ddl1)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);

                ddl1.options.add(option);

            }

            ddl1.value = "[Select]";
        }

        //----------------------------------------------------- 
        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }
        //------------------------------------------------------------------------

        //--------Function for diasabled terue/false-----------------------------
        function rbevent(rb1, rb2, Opt, Group) {
            var rb2 = document.getElementById(rb2);
            rb1.checked = true;
            rb2.checked = false;
            switch (Group) {
                case "Market":
                    var ddlm1 = document.getElementById("<%=ddlmarketcode.ClientID%>");
                    var ddlm2 = document.getElementById("<%=ddlmarketname.ClientID%>");
                    var ddlm3 = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                    var ddlm4 = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                    var lbl1 = document.getElementById("<%=lblmarketcode.ClientID%>");
                    var lbl2 = document.getElementById("<%=lblmarketcodeto.ClientID%>");
                    var lbl3 = document.getElementById("<%=lblmarketname.ClientID%>");
                    var lbl4 = document.getElementById("<%=lblmarketnameto.ClientID%>");

                    break;
                case "Customer":
                    var ddlm1 = document.getElementById("<%=ddlcustomercode.ClientID%>");
                    var ddlm2 = document.getElementById("<%=ddlcustomername.ClientID%>");
                    var ddlm3 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                    var ddlm4 = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                    var lbl1 = document.getElementById("<%=lblcustomercode.ClientID%>");
                    var lbl2 = document.getElementById("<%=lblcustomercodeto.ClientID%>");
                    var lbl3 = document.getElementById("<%=lblcustomername.ClientID%>");
                    var lbl4 = document.getElementById("<%=lblcustomernameto.ClientID%>");

                    break;
                case "Supplier":
                    var ddlm1 = document.getElementById("<%=ddlsupcode.ClientID%>");
                    var ddlm2 = document.getElementById("<%=ddlsupname.ClientID%>");
                    var ddlm3 = document.getElementById("<%=ddlsupcodeto.ClientID%>");
                    var ddlm4 = document.getElementById("<%=ddlsupnameto.ClientID%>");
                    var lbl1 = document.getElementById("<%=lblsupcode.ClientID%>");
                    var lbl2 = document.getElementById("<%=lblsupcodeto.ClientID%>");
                    var lbl3 = document.getElementById("<%=lblsupname.ClientID%>");
                    var lbl4 = document.getElementById("<%=lblsupnameto.ClientID%>");

                    break;
                case "Accounts":
                    var ddlm1 = document.getElementById("<%=ddlacccodefrom.ClientID%>");
                    var ddlm2 = document.getElementById("<%=ddlaccnamefrom.ClientID%>");
                    var ddlm3 = document.getElementById("<%=ddlacccodeto.ClientID%>");
                    var ddlm4 = document.getElementById("<%=ddlaccnameto.ClientID%>");
                    var lbl1 = document.getElementById("<%=lblaccountscode.ClientID%>");
                    var lbl2 = document.getElementById("<%=lblaccountscodeto.ClientID%>");
                    var lbl3 = document.getElementById("<%=lblaccountsname.ClientID%>");
                    var lbl4 = document.getElementById("<%=lblaccountsnameto.ClientID%>");

                    break;

                case "Sales":
                    var ddlm1 = document.getElementById("<%=ddlsalesfrom.ClientID%>");
                    var ddlm2 = document.getElementById("<%=ddlsalesnamefrom.ClientID%>");
                    var ddlm3 = document.getElementById("<%=ddlsalesto.ClientID%>");
                    var ddlm4 = document.getElementById("<%=ddlsalenameto.ClientID%>");
                    var lbl1 = document.getElementById("<%=lblsalescodefrom.ClientID%>");
                    var lbl2 = document.getElementById("<%=lblsalescodeto.ClientID%>");
                    var lbl3 = document.getElementById("<%=lblsalesnamefrom.ClientID%>");
                    var lbl4 = document.getElementById("<%=lblsalesnameto.ClientID%>");

                    break;

            }

            if (Opt == 'A') {
                ddlm1.disabled = true;
                ddlm2.disabled = true;
                ddlm3.disabled = true;
                ddlm4.disabled = true;
                ddlm1.style.visibility = "hidden";
                ddlm2.style.visibility = "hidden";
                ddlm3.style.visibility = "hidden";
                ddlm4.style.visibility = "hidden";

                lbl1.style.visibility = "hidden";
                lbl2.style.visibility = "hidden";
                lbl3.style.visibility = "hidden";
                lbl4.style.visibility = "hidden";
                ddlm1.value = "[Select]";
                ddlm2.value = "[Select]";
                ddlm3.value = "[Select]";
                ddlm4.value = "[Select]";
            }
            else {
                ddlm1.disabled = false;
                ddlm2.disabled = false;
                ddlm3.disabled = false;
                ddlm4.disabled = false;
                ddlm1.style.visibility = "visible";
                ddlm2.style.visibility = "visible";
                ddlm3.style.visibility = "visible";
                ddlm4.style.visibility = "visible";

                lbl1.style.visibility = "visible";
                lbl2.style.visibility = "visible";
                lbl3.style.visibility = "visible";
                lbl4.style.visibility = "visible";

            }

        }

        function hidebooktype() {
            var ddlsumdet = document.getElementById("<%=ddlrpttype.ClientID%>");
            var lblbooktype = document.getElementById("<%=lblbooktype.ClientID%>");
            var booktype = document.getElementById("<%=ddlbooktype.ClientID%>");
            var withprice = document.getElementById("<%=ddlrpttype.ClientID%>");
            var groupby = document.getElementById("<%=ddlgroupby.ClientID%>");

            var btngrd = document.getElementById("<%=btnLoadGrid.ClientID%>");

            if (ddlsumdet.value == 1) {
                lblbooktype.style.display = "block";
                booktype.style.display = "block";
                btngrd.style.display = "block";

            }
            if (ddlsumdet.value == 0) {
                lblbooktype.style.display = "none";
                booktype.style.display = "none";
                btngrd.style.display = "none";
            }
        }


        //-----------------------------------------------------------------------

        //    function ChangeDate() {

        //        var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
        //        var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

        //        if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
        //        else { txttdate.value = txtfdate.value; }


        //    }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                border-bottom: gray 1px solid">
                <tbody>
                    <tr>
                        <td style="text-align: center" class=" field_input" colspan="5">
                            <asp:Label ID="lblHeading" runat="server" Text="Profitability Report" ForeColor="White"
                                Width="100%" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tbody>
                                    <tr>
                                        <td class="td_cell">
                                            <asp:Label ID="lblfromdate" runat="server" Width="104px">From Date</asp:Label>
                                        </td>
                                        <td class="td_cell">
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" Width="96px" TabIndex="1"></asp:TextBox>
                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                TabIndex="2" />
                                            <asp:MaskedEditValidator ID="MskVFromDt" runat="server" ControlExtender="MskFromDate"
                                                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                Width="1px"></asp:MaskedEditValidator>
                                        </td>
                                        <td class="td_cell">
                                            <asp:Label ID="lbltodate" runat="server" Text="To Date" Width="104px"></asp:Label>
                                        </td>
                                        <td class="td_cell">
                                            <asp:TextBox ID="txttoDate" runat="server" CssClass="txtbox" Width="96px" TabIndex="4"></asp:TextBox>
                                            <asp:ImageButton ID="ImgBtntoDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                TabIndex="5" />
                                            <asp:MaskedEditValidator ID="MskVtoDt" runat="server" ControlExtender="MsktoDate"
                                                ControlToValidate="txttoDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                Width="1px"></asp:MaskedEditValidator>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tbody>
                                    <tr>
                                        <td class="td_cell">
                                            <asp:Label ID="lblrptgrp" runat="server" Text="Report Group" Width="100px"></asp:Label>
                                        </td>
                                        &nbsp;
                                        <td>
                                            <select style="width: 120px" id="ddlgroupby" class="field_input" tabindex="5" runat="server">
                                                <option value="Customer">Customer</option>
                                                <option value="Market" selected>Market</option>
                                                <option value="Invoice">Invoice</option>
                                            </select>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBookType" runat="server" Text="Book Type" class="td_cell"></asp:Label>
                                        </td>
                                        <td class="field_input">
                                            <select style="width: 105px;" id="ddlbooktype" class="field_input" tabindex="0" runat="server">
                                                <option value="0" selected>All</option>
                                                <option value="1">Hotel</option>
                                                <option value="2">Transfer</option>
                                                <option value="3">Visa</option>
                                                <option value="4">Excursions</option>
                                                <option value="5">Others</option>
                                                <option value="6">Vehicle Chauffeur</option>
                                                <option value="7">Airport Meet</option>
                                                <option value="8">Golf</option>
                                            </select>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="Report Type" Width="120px" CssClass="field_caption"></asp:Label>
                                        </td>
                                        <td>
                                            <select style="width: 165px" id="ddlrpttype" class="field_input" tabindex="11" onchange="hidebooktype();"
                                                runat="server">
                                                <option value="0" selected>Brief</option>
                                                <option value="1">Detailed</option>
                                            </select>
                                            <%--<asp:DropDownList ID="ddlrpttype" TabIndex="11" runat="server" Width="207px" CssClass="drpdown">
                                                                <asp:ListItem Value="0">Brief</asp:ListItem>
                                                                <asp:ListItem Value="1">Detailed</asp:ListItem>
                                                            </asp:DropDownList>--%>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td class="field_input">
                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                height: 9px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                                border-bottom: gray 1px solid" class="field_input">
                                <tbody>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlmarket" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Market">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbmarketall" type="radio" checked name="Market" runat="server" />
                                                                All&nbsp;
                                                            </td>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbmarketrange" type="radio" name="Market" runat="server" />Range
                                                            </td>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblmarketcode" runat="server" Text="Code From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlmarketcode" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('plgrpcode');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblmarketname" runat="server" Text="Name From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlmarketname" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('plgrpname');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblmarketcodeto" runat="server" Text="Code To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlmarketcodeto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('plgrpcodeto');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblmarketnameto" runat="server" Text="Name To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlmarketnameto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('plgrpnameto');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlcustomer" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Customer">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbCustall" type="radio" checked name="Customer" runat="server" />
                                                                All&nbsp;
                                                            </td>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbcustrange" type="radio" name="Customer" runat="server" />Range
                                                            </td>
                                                            <td style="width: 53px">
                                                                <asp:Label ID="lblcustomercode" runat="server" Text="Code From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlcustomercode" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('agentcode');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 62px">
                                                                <asp:Label ID="lblcustomername" runat="server" Text="Name From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlcustomername" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('agentname');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 53px">
                                                                <asp:Label ID="lblcustomercodeto" runat="server" Text="Code To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlcustomercodeto" class="field_input" disabled
                                                                    tabindex="0" onchange="CallWebMethod('agentcodeto');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 62px">
                                                                <asp:Label ID="lblcustomernameto" runat="server" Text="Name To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlcustomernameto" class="field_input" disabled
                                                                    tabindex="0" onchange="CallWebMethod('agentnameto');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlSupplier" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Supplier"
                                                Style="display: none">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbsupall" type="radio" checked name="Supplier" runat="server" />
                                                                All&nbsp;
                                                            </td>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbsuprange" type="radio" name="Supplier" runat="server" />Range
                                                            </td>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblsupcode" runat="server" Text="Code From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsupcode" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('partycode');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblsupname" runat="server" Text="Name From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsupname" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('partyname');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblsupcodeto" runat="server" Text="Code To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsupcodeto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('partycodeto');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblsupnameto" runat="server" Text="Name To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsupnameto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('partynameto');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="PnlAccounts" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Accounts code">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbaccountsall" type="radio" checked name="Accounts" runat="server" />
                                                                All&nbsp;
                                                            </td>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbaccountsrange" type="radio" name="Accounts" runat="server" />Range
                                                            </td>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblaccountscode" runat="server" Text="Code From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlacccodefrom" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('acccodefrom');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblaccountsname" runat="server" Text="Name From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlaccnamefrom" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('accnamefrom');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblaccountscodeto" runat="server" Text="Code To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlacccodeto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('acccodeto');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblaccountsnameto" runat="server" Text="Name To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlaccnameto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('accnameto');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="Pnlsales" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Sales Person">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbsalesall" type="radio" checked name="Sales" runat="server" />
                                                                All&nbsp;
                                                            </td>
                                                            <td style="width: 100px" rowspan="2">
                                                                <input id="rbsalesrange" type="radio" name="Sales" runat="server" />Range
                                                            </td>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblsalescodefrom" runat="server" Text="Code From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsalesfrom" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('salescodefrom');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblsalesnamefrom" runat="server" Text="Name From" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsalesnamefrom" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('salesnamefrom');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 54px">
                                                                <asp:Label ID="lblsalescodeto" runat="server" Text="Code To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsalesto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('salescodeto');" runat="server">
                                                                </select>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <asp:Label ID="lblsalesnameto" runat="server" Text="Name To" Width="62px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select style="width: 196px" id="ddlsalenameto" class="field_input" disabled tabindex="0"
                                                                    onchange="CallWebMethod('salesnameto');" runat="server">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <table>
                                        <tr style="text-align:center;width:300px">                                            
                                            <td>
                                                <asp:Button ID="btnLoadreport" TabIndex="5" OnClick="btnLoadreport_Click" runat="server"
                                                    Text="Load Report" Width="83px" CssClass="field_button"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnExit" TabIndex="6" OnClick="btnExit_Click" runat="server" Text="Exit"
                                                    Width="46px" CssClass="field_button" CausesValidation="False"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnLoadgrid" TabIndex="7" runat="server" style="display:none" Text="Load Grid"
                                                    CssClass="field_button" CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </tbody>
                            </table>
                            <asp:CalendarExtender ID="ClsExFromDate" runat="server" TargetControlID="txtFromDate"
                                PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:CalendarExtender ID="ClsExtoDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtntoDt"
                                TargetControlID="txttoDate">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                                MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                DisplayMoney="Left" AcceptNegative="Left">
                            </asp:MaskedEditExtender>
                            <asp:MaskedEditExtender ID="MsktoDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                                ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                TargetControlID="txttoDate">
                            </asp:MaskedEditExtender>
                            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                                <Services>
                                    <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                                </Services>
                            </asp:ScriptManagerProxy>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
