<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" EnableEventValidation="false"
    AutoEventWireup="false" CodeFile="UpdatesupplierInvoices.aspx.vb" Inherits="AccountsModule_UpdatesupplierInvoices" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="JavaScript" type="text/javascript">
        window.history.forward(1);  
    </script>
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <link href="css/allStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .wrapper
 {
 	border: 1px solid #06788B;
	font-family: Verdana, Arial, Geneva, ms sans serif;
	font-size: 8pt;
	font-weight: bold;
	font-style: normal;
	font-variant: normal;
	color:white;
	background-color :#2D7C8A;
	margin-top: 0px;
    float:left;
    word-wrap: break-word; /* fix for long text breaking sidebar float in IE */
    height=25; 
    text-align :left;    
         
}
    #sidebar-wrapper
{
        float:right;
        width:272px;
        margin-right: 5px;
        font-size: 12px;
        word-wrap: break-word; /* fix for long text breaking sidebar float in IE */
        overflow: hidden;     /* fix for long non-text content breaking IE sidebar float */
}          
</style>
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
            font-size: 10pt;
            font-weight: bold;
            border-bottom-style: double;
            border-width: medium;
        }
        
        *
        {
            outline: none;
        }
        .style4
        {
            width: 111px;
        }
        .style5
        {
            width: 124px;
        }
    </style>
    <script language="javascript" type="text/javascript">


        var ddldiffAcc = null;
        var ddldiffAccNm = null;

        function trim(stringToTrim) {
            return stringToTrim.replace(/^\s+|\s+$/g, "");
        }

        function chkTextLock(evt) {
            return false;
        }
        function chkTextLock1(evt) {
            if (evt.keyCode = 9) {
                return true;
            }
            else {
                return false;
            }
            return false;

        }
        function checkNumber(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            //if (charCode != 47 && (charCode > 45 && charCode < 58)) {    
            if (charCode != 47 && (charCode > 44 && charCode < 58)) {
                return true;
            }
            return false;
        }
        function checkNumber1(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if ((charCode > 47 && charCode < 58)) {
                //if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
                return true;
            }
            return false;
        }

        function checkNumber2(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if ((charCode > 46 && charCode < 58)) {

                return true;
            }
            return false;
        }
        function checkNumberDecimal(evt, txt) {

            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if (charCode != 47 && (charCode > 44 && charCode < 58)) {
                var value = txt.value;
                var indx = value.indexOf('.');
                var deci = 2
                var lngLenght = deci.value;
                if (indx < 0) {
                    return true;
                }

                var digit = value.substring(indx + 1);
                if (digit.length > lngLenght - 1) {
                    return false;
                }
                else {
                    return true;
                }
            }
            return false;
        }

        function DecRound(amtToRound) {


            nodecround = Math.pow(10, parseInt(txtdec.value));
            //var amtToRound=Number(amtToRound1);
            var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
            return parseFloat(rdamt);
        }
        function DecFormat(value) {
            var rdamt = null;
            var indx = value.indexOf('.');
            var deci = 2
            var lngLenght = deci.value;
            if (indx < 0) {
                rdamt = value + ".000";
                return rdamt;
            }
            var digit = value.substring(indx + 1);
            if (digit.length > lngLenght - 1) {
                rdamt = value;
                return rdamt;
            }
            else {
                var nozeros = parseInt(lngLenght) - parseInt(digit.length);

                if (nozeros == 1)
                { rdamt = value + "0"; }
                else if (nozeros == 2)
                { rdamt = value + "00"; }
                else if (nozeros == 3)
                { rdamt = value + "000"; }
                else
                { return value; }
                return rdamt;
            }
            return rdamt;
        }



        function Filldiffac(accdiffcode, accdiffname) {

            ddldiffAcc = document.getElementById(accdiffcode);
            ddldiffAccNm = document.getElementById(accdiffname);

            ddldiffAcc.value = ddldiffAccNm.options[ddldiffAccNm.selectedIndex].text;

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




        function DispalyValidate() {
            if (document.getElementById("<%=ddlSupplierCode.ClientID%>").value == "[Select]" || document.getElementById("<%=ddlSpplierName.ClientID%>").value == "[Select]") {
                alert("Select supplier code");
                document.getElementById("<%=ddlSupplierCode.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddlControlCode.ClientID%>").value == "[Select]" || document.getElementById("<%=ddlControlName.ClientID%>").value == "[Select]") {
                alert("Select control a/c code");
                document.getElementById("<%=ddlControlCode.ClientID%>").focus();
                return false;
            }

        }

        function CallWebMethod(methodType) {
            switch (methodType) {
                case "partycode":
                    var select = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlSpplierName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    FillAllDataOnSupplier();
                    break;
                case "partyname":
                    var select = document.getElementById("<%=ddlSpplierName.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    FillAllDataOnSupplier();
                    break;
                case "controlcode":
                    var select = document.getElementById("<%=ddlControlCode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlControlName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "controlname":
                    var select = document.getElementById("<%=ddlControlName.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlControlCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }


        function FillAllDataOnSupplier() {

            var ddlTyp = document.getElementById("<%=ddlType.ClientID%>");
            var ddlSCode = document.getElementById("<%=ddlSupplierCode.ClientID%>");
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            if ((ddlTyp.value != '[Select]') && (ddlSCode.value != '[Select]')) {
                // var crdsqlstr="select postaccount,accrualacctcode,controlacctcode,cur,convrate  from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '"+  ddlSCode.options[ddlSCode.selectedIndex].text +"' and type='"+ddlTyp.value  +"' ";
                var crdsqlstr = "select isnull(controlacctcode,'[Select]') as controlacctcode,isnull(cur,'') as cur, isnull(convrate,0) as convrate  from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + ddlSCode.options[ddlSCode.selectedIndex].text + "' and type='" + ddlTyp.value + "' ";
                ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 5, FillValue, ErrorHandler, TimeOutHandler);

                var crdsqlstr = "select sptypecode from partymast where partycode = '" + ddlSCode.options[ddlSCode.selectedIndex].text + "' ";
                ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 3, FillSptype, ErrorHandler, TimeOutHandler);

            }
            else {
                FillValueClear();
            }


            //ColServices.clsServices.GetQueryReturnStringArraynew(constr,sqlstr1,4,FillValueCurr,ErrorHandler,TimeOutHandler);

        }
        function FillValue(result) {

            var i = 0;


            var ddlCCode = document.getElementById("<%=ddlControlCode.ClientID%>");
            var ddlCName = document.getElementById("<%=ddlControlName.ClientID%>");

            ddlCName.value = result[0];
            ddlCCode.value = ddlCName.options[ddlCName.selectedIndex].text;

            var txtCurr = document.getElementById("<%=txtCurrency.ClientID%>");
            txtCurr.value = result[1];
            var txtconvrate = document.getElementById("<%=txtconvrate.ClientID%>");
            txtconvrate.value = result[2];

        }

        function FillSptype(result) {
            var hdnsptype1 = document.getElementById('<%=htnsptype.ClientID%>');
            hdnsptype1.value = result[0];
        }

        function FillValueClear() {


            var ddlCCode = document.getElementById("<%=ddlControlCode.ClientID%>");
            var ddlCName = document.getElementById("<%=ddlControlName.ClientID%>");
            ddlCName.value = '[Select]';
            ddlCCode.value = '[Select]';

            var txtCurr = document.getElementById("<%=txtCurrency.ClientID%>");
            txtCurr.value = "";
            var txtERate = document.getElementById("<%=txtExchRate.ClientID%>");
            txtERate.value = "";
            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');
            var hdnsptype1 = document.getElementById('<%=htnsptype.ClientID%>');
            hdnsptype1.value = ""

            if (trim(txtCurr.value) == trim(txtbase.value)) {
                txtERate.readOnly = true;
                txtERate.disabled = true;

            }
            else {
                txtERate.readOnly = false;
                txtERate.disabled = false;
            }
        }


        function FillSupplierCode(result) {
            var ddlSCode = document.getElementById("<%=ddlSupplierCode.ClientID%>");
            RemoveAll(ddlSCode)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlSCode.options.add(option);
            }
            ddlSCode.value = "[Select]";
        }

        function FillSupplierName(result) {
            var ddlSName = document.getElementById("<%=ddlSpplierName.ClientID%>");
            RemoveAll(ddlSName)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlSName.options.add(option);
            }
            ddlSName.value = "[Select]";
        }



        function validate_click() {
            var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");
            var btnss = document.getElementById("<%=btnsave.ClientID%>");
            hdnSS.value = 0;
            if (hdnSS.value == 0) {
                hdnSS.value = 1;
                // btnss.disabled=true;
                btnss.style.visibility = "hidden";
                return true;
            }
            else {
                return false;
            }
        }


        function btnclick(objbtn, chk) {
            var chk = document.getElementById(chk);
            var objbtn1 = document.getElementById(objbtn);




            objbtn1.click();

        }

        function Changecost(costprice, costvalue, nonights, obj) {
            /*
            var costval = document.getElementById(costvalue);
            var costp = document.getElementById(costprice);
            var nights = document.getElementById(nonights);
            var obj1=document.getElementById(obj)
            costval.innerHTML = parseFloat(costp.value) * parseFloat(nights.innerHTML)
            */
            var obj1 = document.getElementById(obj)
            obj1.click();
        }

        function calculatevalue() {


            units = document.getElementById("<%=txtunits.ClientID%>");
            cprice = document.getElementById("<%=txtcprice.ClientID%>");
            cvalue = document.getElementById("<%=txtcValue.ClientID%>");

            cvalue.value = parseFloat(units.value) * parseFloat(cprice.value);

        }


    </script>
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 920px" class="td_cell">
                            <tbody>
                                <tr>
                                    <td style="text-align: center" colspan="4">
                                        <asp:Label ID="lblHeading" runat="server" Text="Add Update Supplier Invoices" Width="980px"
                                            CssClass="field_heading"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        Doc no.
                                    </td>
                                    <td style="width: 212px">
                                        <input style="width: 128px" id="txtPInvoiceNo" class="field_input" readonly type="text"
                                            runat="server" />
                                    </td>
                                    <td style="width: 118px">
                                        Date
                                    </td>
                                    <td style="width: 270px">
                                        <asp:TextBox ID="txtPInvoiceDate" TabIndex="1" runat="server" Width="88px" CssClass="fiel_input"
                                            ValidationGroup="MKE"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtnPInvoiceDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                        </asp:ImageButton>
                                        <cc1:MaskedEditValidator ID="MEVPInvoiceDate" runat="server" CssClass="field_error"
                                            ControlExtender="MEPInvoiceDate" ControlToValidate="txtPInvoiceDate" Display="Dynamic"
                                            EmptyValueBlurredText="Purchase invoice date is required" InvalidValueBlurredMessage="Invalid purchase invoice date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Enter a date in dd/mm/yyyy format"
                                            ErrorMessage="MEVPInvoiceDate"></cc1:MaskedEditValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        Type
                                    </td>
                                    <td style="width: 212px">
                                        <select id="ddlType" runat="server" class="field_input MyAutoCompleteTypeClass" name="D1"
                                            onchange="FillSupplier()" style="width: 104px" tabindex="2">
                                            <option selected="" value="S">Supplier</option>
                                            <option value="A">Supplier Agent</option>
                                        </select>
                                    </td>
                                    <td style="width: 118px">
                                        &nbsp;
                                    </td>
                                    <td style="width: 270px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        <asp:Label ID="lblCustCode" runat="server" Text="Supplier  Code <font color='Red'> *</font>"
                                            Width="136px"></asp:Label>
                                    </td>
                                    <td style="width: 212px">
                                        <select style="width: 160px" id="ddlSupplierCode" class="field_input" tabindex="3"
                                            onchange="CallWebMethod('partycode');" runat="server">
                                        </select>&nbsp;
                                    </td>
                                    <td style="width: 118px">
                                        <asp:Label ID="lblCustName" runat="server" Text=" Supplier Name" Width="122px" CssClass="field_caption"></asp:Label>
                                    </td>
                                    <td style="width: 270px">
                                        <input type="text" name="accSearch" class="field_input MyAutoCompleteClass" onfocus="MyAutoCustomerFillArray();"
                                            style="width: 98%; font" id="accSearch" runat="server" /><select style="width: 280px"
                                                id="ddlSpplierName" class="field_input MyDropDownListCustValue" tabindex="4"
                                                onchange="CallWebMethod('partyname');" runat="server"></select>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        Control A/C Code
                                    </td>
                                    <td style="width: 212px">
                                        <select style="width: 160px" id="ddlControlCode" class="field_input" tabindex="9"
                                            onchange="CallWebMethod('controlcode');" runat="server">
                                        </select>
                                    </td>
                                    <td style="width: 118px">
                                        Control A/C Name
                                    </td>
                                    <td style="width: 270px">
                                        <select style="width: 280px" id="ddlControlName" class="field_input" tabindex="10"
                                            onchange="CallWebMethod('sptypecode');" runat="server">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        Currency
                                    </td>
                                    <td style="width: 212px">
                                        <input style="width: 128px" id="txtCurrency" class="field_input" tabindex="15" readonly
                                            type="text" runat="server" />
                                    </td>
                                    <td style="width: 118px">
                                        Conv.
                                    </td>
                                    <td style="width: 270px">
                                        <input style="width: 128px" id="txtconvrate" class="field_input" tabindex="15" readonly
                                            type="text" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        From Date
                                    </td>
                                    <td style="width: 212px">
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="fiel_input" TabIndex="17"
                                            ValidationGroup="MKE" Width="88px"></asp:TextBox>
                                        <asp:ImageButton ID="imgbtnFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <cc1:MaskedEditValidator ID="MEVFFromDate" runat="server" ControlExtender="MEVFromDate"
                                            ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueMessage="From date is required"
                                            ErrorMessage="MEVFFromDate" InvalidValueBlurredMessage="Invalid from date" InvalidValueMessage="Invalid Date"
                                            TooltipMessage="Enter a date in dd/mm/yyyy format">
                                        </cc1:MaskedEditValidator>
                                    </td>
                                    <td style="width: 118px">
                                        To Date
                                    </td>
                                    <td style="width: 270px">
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" TabIndex="18" ValidationGroup="MKE"
                                            Width="88px"></asp:TextBox>
                                        <asp:ImageButton ID="imgbtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <cc1:MaskedEditValidator ID="MEVToDate" runat="server" ControlExtender="METoDate"
                                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueMessage="To date is required"
                                            ErrorMessage="MEVToDate" InvalidValueBlurredMessage="Invalid from date" InvalidValueMessage="Invalid Date"
                                            TooltipMessage="Enter a date in dd/mm/yyyy format">
                                        </cc1:MaskedEditValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        Remarks
                                    </td>
                                    <td style="width: 212px">
                                        <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Width="292px"></asp:TextBox>
                                    </td>
                                    <td style="width: 118px">
                                        <asp:Button ID="btnDisplay" runat="server" CssClass="btn" TabIndex="19" Text="Display" />
                                    </td>
                                    <td style="width: 270px">
                                        <asp:Button ID="btnClear" runat="server" CssClass="btn" TabIndex="20" Text="Clear" />
                                        <input style="width: 128px" id="txtxtrequestid" class="field_input" runat="server"
                                            visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" style="width: 212px" title=" ">
                                        <input style="visibility: hidden" id="txtExchRate" class="field_input" tabindex="16"
                                            type="text" runat="server" />
                                        &nbsp;
                                    </td>
                                    <td style="width: 118px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px">
                                    </td>
                                    <td style="width: 212px">
                                        <asp:Label ID="Label9" runat="server" ForeColor="#000099" Text="Please select the lines to update "
                                            Width="300px"></asp:Label>
                                    </td>
                                    <td style="width: 118px">
                                        &nbsp;
                                    </td>
                                    <td style="width: 270px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" DataKeyNames="acc_type,acc_code,acc_gl_code,accname,tranid,acc_tran_lineno,trantype,trandate,fileno,reconfno,particulars,arrdate,depdate,mode,debit,credit,otherref"
                                            CssClass="td_cell" Font-Italic="False" Font-Size="10px" GridLines="Vertical"
                                            TabIndex="21" Width="875px">
                                            <Columns>
                                                <asp:BoundField DataField="sno" HeaderText="Sr No.">
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="trandate" HeaderText="Date">
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="tranid" HeaderText="Voucher no.">
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="acc_tran_lineno" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccTranLineno" runat="server" Text='<%# Bind("acc_tran_lineno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="trantype" HeaderText="Voucher Type">
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fileno" HeaderText="File Number">
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Hotel Invoice No">
                                                    <ItemTemplate>
                                                        <input style="width: 100px; text-align: right" id="txtinvno" class="field_input"
                                                            type="text" runat="server" value='<%# Bind("otherref") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="particulars" HeaderText="Description">
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:BoundField>--%>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox TextMode="MultiLine" id="txtDescription"
                                                        runat="server" class="field_input" Text='<%# Bind("particulars") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="credit" HeaderText="Amount">
                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Modify Reservation">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnreservation" runat="server" CssClass="btn" Text="Reservation"
                                                            OnClick="btnreservation_Click" />
                                                        <asp:HiddenField ID="hdnrlineno" runat="server" />
                                                        <asp:HiddenField ID="hdnamount" runat="server" Value='<%# Bind("credit") %>' />
                                                        <asp:HiddenField ID="hdnvalue" runat="server" />
                                                        <asp:HiddenField ID="hdncprice" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>
                                                 <asp:BoundField DataField="agentname" HeaderText="Customer">
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle" Wrap="False" />
                                            <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center" Wrap="False" />
                                            <HeaderStyle CssClass="grdheader" ForeColor="white" Wrap="False" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                        <asp:Label ID="lblMsg" runat="server" Text="Records not found." Width="152px" Visible="False"
                                            CssClass="lblmsg" Font-Bold="True" Font-Names="Verdana"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4" style="height: 7px">
                                        &nbsp; &nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;<input style="visibility: hidden; width: 9px; height: 3px" id="txtbasecurr"
                                            type="text" maxlength="20" runat="server" />
                                        <input style="visibility: hidden; width: 9px; height: 3px" id="txtPostCode" type="text"
                                            maxlength="20" runat="server" />
                                        <input style="visibility: hidden; width: 9px; height: 3px" id="txtPostName" type="text"
                                            maxlength="20" runat="server" />
                                        <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
                                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                            height: 9px" type="text" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="25" Text="Save" />
                                        &nbsp;
                                        <asp:Button ID="btnPrint" runat="server" CssClass="btn" OnClick="btnPrint_Click"
                                            TabIndex="26" Text="Print" />
                                        &nbsp;<asp:Button ID="btnExit" runat="server" CssClass="btn" TabIndex="27" Text="Exit"
                                            Width="40px" />
                                        &nbsp;
                                        <asp:Button ID="btnHelp" runat="server" CssClass="btn" OnClick="btnHelp_Click" TabIndex="28"
                                            Text="Help" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 144px; height: 16px">
                                    </td>
                                    <td style="width: 220px; height: 16px; text-align: left">
                                        &nbsp;
                                    </td>
                                    <td style="width: 118px; height: 16px">
                                    </td>
                                    <td style="width: 270px; height: 16px">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <cc1:CalendarExtender ID="CEPInvocieDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnPInvoiceDate"
                            TargetControlID="txtPInvoiceDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MEPInvoiceDate" runat="server" TargetControlID="txtPInvoiceDate"
                            ErrorTooltipEnabled="True" MaskType="Date" Mask="99/99/9999">
                        </cc1:MaskedEditExtender>
                        <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnFromDate"
                            TargetControlID="txtFromDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MEVFromDate" runat="server" TargetControlID="txtFromDate"
                            ErrorTooltipEnabled="True" MaskType="Date" Mask="99/99/9999">
                        </cc1:MaskedEditExtender>
                        <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnToDate"
                            TargetControlID="txtToDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="METoDate" runat="server" TargetControlID="txtToDate"
                            ErrorTooltipEnabled="True" MaskType="Date" Mask="99/99/9999">
                        </cc1:MaskedEditExtender>
                        <asp:HiddenField ID="hdnSS" runat="server" Value="0" />
                        <asp:ModalPopupExtender ID="ModalPopuphoteldetail" runat="server" BehaviorID="ModalPopuphoteldetail"
                            CancelControlID="btnCancel" OkControlID="btnOkay" TargetControlID="btnInvisibleGuest"
                            PopupControlID="hoteldetail" PopupDragHandleControlID="hoteldetailHeader" Drag="true"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopuphoteltransfer" runat="server" BehaviorID="ModalPopuphoteltransfer"
                            CancelControlID="btnCancel1" OkControlID="btnOkay1" TargetControlID="btnInvisibleGuest1"
                            PopupControlID="transferdetail" PopupDragHandleControlID="hoteldetail" Drag="true"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:HiddenField ID="htnsptype" runat="server" />
                        <asp:Panel ID="hoteldetail" Style="display: none" runat="server" BorderStyle="Double"
                            BorderWidth="4px">
                            <div class="HellowWorldPopup" style="font-family: Arial, Helvetica, sans-serif">
                                <%--<div class="PopupHeader" id="Div2">--%>
                                <asp:Panel ID="hoteldetailHeader" CssClass="PopupHeader" runat="server">
                                    <center style="background-color: #CCCCCC;">
                                        Hotel Reservation Detail</center>
                                </asp:Panel>                                
                                <%--</div>--%>
                                <div class="PopupBody">
                                    <center>
                                        <asp:Panel ID="Panel1" Height="350px" Width="900px" ScrollBars="Vertical" BorderStyle="Solid"
                                            BorderWidth="1px" runat="server">
                                            <asp:GridView ID="gvRoomDetails" runat="server" CssClass="grdstyle" AutoGenerateColumns="False"
                                                Width="850px">
                                                <RowStyle CssClass="grdRowstyle" ForeColor="Black" Wrap="False" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td align="left" style="width: 100px; height: 15px; background-color: #2D7C8A;">
                                                                        <asp:Label ID="Label6" runat="server" Text="Room Type Code" ForeColor="White" CssClass="Label_heading"
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 150px; height: 15px; background-color: #2D7C8A;">
                                                                        <asp:Label ID="Label7" runat="server" Text="RoomType Name" Width="200px" ForeColor="White"
                                                                            CssClass="Label_heading" Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 100px; height: 15px; background-color: #2D7C8A;">
                                                                        <asp:Label ID="Label8" runat="server" Text="Meal Code" ForeColor="White" CssClass="Label_heading"
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 101px; height: 15px; background-color: #2D7C8A;">
                                                                        <asp:Label ID="Label3" runat="server" Text="Category" ForeColor="White" CssClass="Label_heading"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 100px; height: 15px; background-color: #2D7C8A;">
                                                                        <asp:Label ID="Label5" runat="server" Text="No of Rooms" ForeColor="White" CssClass="Label_heading"
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="height: 25px; background-color: #2D7C8A; width: 50;">
                                                                        <asp:Label ID="lblcvalue" runat="server" Text="CostValue" Width="75px" ForeColor="White"
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 55px; height: 15px; background-color: #2D7C8A">
                                                                        <asp:Label ID="lblsvalue" runat="server" Text="SaleValue" Width="75px" ForeColor="White"
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 70px">
                                                                        <asp:Label ID="lblroomtype" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "rmtypcode") %>'
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="lblrmtypname" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "rmtypname") %>'
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="lblMCode" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "mealcode") %>'
                                                                            Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 101px">
                                                                        <asp:Label ID="lblCat" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "rmcatcode") %>'
                                                                            Font-Bold="True"></asp:Label>
                                                                        <td align="left" style="width: 100px">
                                                                            <asp:Label ID="lblNoroom" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "units") %>'
                                                                                Width="50px" Font-Bold="True"></asp:Label>
                                                                        </td>
                                                                        <td align="left" style="width: 101px">
                                                                            <asp:Label ID="lblcostvalue" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "totcostvalue") %>'
                                                                                Width="50px" Font-Bold="True"></asp:Label>
                                                                        </td>
                                                                        <td align="left" style="width: 100px">
                                                                            <asp:Label ID="lblsalevalue" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "totsalevalue") %>'
                                                                                Width="50px" Font-Bold="True"></asp:Label>
                                                                        </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 70px">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td colspan="7">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <div align="left">
                                                                <asp:GridView ID="gvPrice" runat="server" AutoGenerateColumns="False" CssClass="td_cell"
                                                                    Width="800px" OnRowDataBound="gvPrice_RowDataBound">
                                                                    <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                                                                    <HeaderStyle CssClass="grdheader" Wrap="False" />
                                                                    <RowStyle CssClass="grdRowstyle" Wrap="False" />
                                                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Slineno" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblslineno" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "slineno") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" Width="75px" />
                                                                            <ItemStyle Width="75px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Valid From">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblValidform" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "fromdate") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" Width="75px" />
                                                                            <ItemStyle Width="75px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Valid To">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblValidto" runat="server" CssClass="td_cell" Text='<%# DataBinder.Eval (Container.DataItem, "todate") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="75px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Price">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblpricenight" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "price") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Cost Price">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtcostpricenight" runat="server" CssClass="txtbox" Text='<%# DataBinder.Eval (Container.DataItem, "cprice") %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" />
                                                                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Nights">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblnights" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "nights") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Free">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFree" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "freenights") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Sale Value">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblvalue" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "value") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Cost Value">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCostvalue" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "costvalue") %>'></asp:Label>
                                                                                <asp:Button ID="btncostclick" runat="server" OnClick="btncostclick_Click" CssClass="field_button"
                                                                                    Style="visibility: hidden" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="td_cell" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grdheader" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                                                <HeaderStyle CssClass="grdheader" Wrap="False" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                        </asp:Panel>
                                        <br />
                                        &nbsp;&nbsp;<asp:Button ID="btnMoreSave" runat="server" CssClass="field_button" Text="Save" />
                                        &nbsp;
                                        <asp:Button ID="btnMoreClose" runat="server" CssClass="field_button" Text="Close" />
                                        <asp:HiddenField ID="hdnrlineno" runat="server" />
                                        <asp:HiddenField ID="hdnrequestid" runat="server" />
                                        <asp:HiddenField ID="hdndocno" runat="server" />
                                        <asp:HiddenField ID="hdnrowid1" runat="server" />
                                        <asp:HiddenField ID="hdnpartycode" runat="server" />
                                        <asp:HiddenField ID="hdnsupagentcode1" runat="server" />
                                        <asp:HiddenField ID="ghdnrlineno1" runat="server" />
                                    </center>
                                    <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="Button2" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnInvisibleRemark" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnOkay" type="button" value="OK" style="visibility: hidden" />
                                    <input id="btnCancel" type="button" value="Cancel" style="visibility: hidden" />
                                    <input style="visibility: hidden; width: 12px; height: 9px" id="Text1" type="text"
                                        runat="server" />
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="transferdetail" Style="display: none" runat="server" BorderStyle="Double"
                            BorderWidth="4px">
                            <div class="HellowWorldPopup" style="font-family: Arial, Helvetica, sans-serif">
                                <div class="PopupHeader" id="Div1">
                                    <center style="background-color: #CCCCCC;">
                                        Transfer Detail</center>
                                </div>
                                <div class="PopupBody">
                                    <center>
                                        <asp:Panel ID="Panel3" Height="200px" Width="874px" ScrollBars="Vertical" BorderStyle="Solid"
                                            BorderWidth="1px" runat="server">
                                            <table style="width: 833px; font-weight: bold;">
                                                <tr>
                                                    <td style="text-align: right" class="style5">
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="4" align="left">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style5" style="text-align: right; height: 30px;">
                                                        Transfer Date
                                                    </td>
                                                    <td align="left" colspan="4">
                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="field_input_agent" ReadOnly="True"
                                                            TabIndex="0"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="" class="style5" align="right">
                                                        Route
                                                    </td>
                                                    <td colspan="5" align="left">
                                                        <input id="txtroutes" runat="server" class="field_input_agent" maxlength="3" style="width: 450px;"
                                                            type="text" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 30px;" class="style5" align="right">
                                                        Vehicle Type
                                                    </td>
                                                    <td colspan="4" align="left">
                                                        <input id="txtvehicle" runat="server" class="field_input_agent" maxlength="3" style="width: 225px;"
                                                            type="text" readonly="readonly" />
                                                    </td>
                                                    <td style="text-align: right; width: 250px" class="style4">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 30px;" class="style5" align="right">
                                                        No Of Units
                                                    </td>
                                                    <td align="left">
                                                        <input id="txtUnits" runat="server" class="field_input_agent" type="text" maxlength="2"
                                                            style="width: 40px;" readonly="readonly" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="Button1" runat="server" Style="display: none;" BorderStyle="Outset"
                                                            Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Display Price" Width="100px" />
                                                        Sale Price [<asp:Label ID="lblsprice" runat="server" Text="Label"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <input id="txtprice" runat="server" class="field_input_agent" type="text" readonly="readonly"
                                                            style="width: 75px" />
                                                    </td>
                                                    <td align="right">
                                                        Sale Value [<asp:Label ID="lblsvalue" runat="server" Text="Label"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <input id="txtValue" runat="server" class="field_input_agent" type="text" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style5" style="text-align: right">
                                                        &nbsp;
                                                    </td>
                                                    <td align="left">
                                                        &nbsp;
                                                    </td>
                                                    <td align="right">
                                                        Cost Price [<asp:Label ID="lblcprice" runat="server" Text="Label"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <input id="txtcprice" runat="server" class="field_input_agent" type="text" style="width: 75px" />
                                                    </td>
                                                    <td align="right">
                                                        Cost Value [<asp:Label ID="lblcvalue" runat="server" Text="Label"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <input id="txtcValue" runat="server" class="field_input_agent" type="text" readonly="readonly" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        &nbsp;&nbsp;<asp:Button ID="btntransfersave" runat="server" CssClass="field_button"
                                            Text="Save" />
                                        &nbsp;
                                        <asp:Button ID="btntransferclose" runat="server" CssClass="field_button" Text="Close" />
                                    </center>
                                    <input id="btnInvisibleGuest1" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="Button5" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnInvisibleRemark1" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnOkay1" type="button" value="OK" style="visibility: hidden" />
                                    <input id="btnCancel1" type="button" value="Cancel" style="visibility: hidden" />
                                    <asp:HiddenField ID="hdnrowid" runat="server" />
                                    <asp:HiddenField ID="hdnolineno" runat="server" />
                                    <input style="visibility: hidden; width: 12px; height: 9px" id="Text2" type="text"
                                        runat="server" />
                                </div>
                            </div>
                        </asp:Panel>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <asp:GridView ID="grdInvError" runat="server" AutoGenerateColumns="false" CssClass="grdheader"
                            Visible="False">
                            <Columns>
                                <asp:BoundField DataField="servicedescription" HeaderText="Service Description">
                                    <HeaderStyle Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="errmessage" HeaderText="Error Message">
                                    <HeaderStyle Wrap="true" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="grdstyle1_agent" />
                            <AlternatingRowStyle CssClass="grdstyle1_agent" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                    <Services>
                        <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                    </Services>
                </asp:ScriptManagerProxy>
            </td>
        </tr>
    </table>
</asp:Content>
