<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SalesInvoiceManual.aspx.vb" Inherits="AccountsModule_SalesInvoiceManual" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="javascript" type="text/javascript">





    function OpenModalDialog(url, diaHeight) {

        var vReturnValue;
        if (diaHeight == null || diaHeight == "")
            diaHeight = "300";
        if (url != null) {
            vReturnValue = window.showModalDialog(url, "#1", "dialogHeight: " + diaHeight + "px; dialogWidth: 650px; dialogTop: 190px; dialogLeft: 120px;dialogRight:220px; edge: Raised; center: Yes; help: No; resizable: No; status: No;");
        }
        else {
            alert("No URL passed to open");
        }
        if (vReturnValue != null && vReturnValue == true) {

            return vReturnValue
        }
        else {   //alert(vReturnValue);
            //alert(vReturnValue);
            return false;
        }
    }

    //Common
    var nodecround = null;
    function DecRound(amtToRound) {
        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));
        var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
        return parseFloat(rdamt);
    }
    function DecRoundtothree(amtToRound) {
        nodecround = Math.pow(10, 3);
        var rdamt = Math.round(parseFloat(amtToRound) * nodecround) / nodecround;

        return parseFloat(rdamt);
    }

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
            var deci = document.getElementById("<%=txtdecimal.ClientID%>");
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

    function ReprintDoc() {
        if (confirm('Are you sure you want to reprint ?') == false) return false;
    }



    function FormValidation(state) {

        if (document.getElementById("<%=ddlCustomer.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlCustomer.ClientID%>").focus();
            alert("Select " + dvType.innerHTML);
            return false;
        }
        else if (document.getElementById("<%=ddlCustomerName.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlCustomerName.ClientID%>").focus();
            alert("Select " + dvType.innerHTML + " Name");
            return false;
        }
        else if (document.getElementById("<%=ddlSalesman.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlSalesman.ClientID%>").focus();
            alert("Select Sales Man Code");
            return false;
        }
        else if (document.getElementById("<%=ddlSalesmanName.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlSalesmanName.ClientID%>").focus();
            alert("Select Sales Man Name");
            return false;
        }
        else if (Number(document.getElementById("<%=txtConversion.ClientID%>").value) == 0) {
            document.getElementById("<%=txtConversion.ClientID%>").focus();
            alert("Conversion Rate can not be 0");
            return false;
        }
        else {
            //alert(state);
            if (state == 'New') { if (confirm('Are you sure you want to save ?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
            if (state == 'Cancel') { if (confirm('Are you sure you want to Cancel ?') == false) return false; }
            if (state == 'UndoCancel') { if (confirm('Are you sure you want to UndoCancel ?') == false) return false; }

        }
    }

    function CallWebMethod(methodType) {
        switch (methodType) {
            case "customercode":

                var ddltyp = document.getElementById("<%=ddlType.ClientID%>");
                var select = document.getElementById("<%=ddlCustomer.ClientID%>");
                if (ddltyp.value == '[Select]') {
                    alert('Please Select Type');
                    select.value = "[Select]";
                }
                else {

                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlCustomerName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;

                    var tcustcode = document.getElementById("<%=txtcustcode.ClientID%>");
                    tcustcode.value = select.options[select.selectedIndex].value;
                    var tcustname = document.getElementById("<%=txtcustname.ClientID%>");
                    tcustname.value = select.options[select.selectedIndex].text;



                    var typ = ddltyp.options[ddltyp.selectedIndex].value;
                    //sqlstr="select cur from view_account where Code = '"+ codeid +"' and type= '"+ typ +"'";
                    //ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrCodes,ErrorHandler,TimeOutHandler);

                    var txtfrdate = document.getElementById("<%=txtDate.ClientID%>");
                    var strdate = txtfrdate.value;

                    if (strdate != "") {
                        sqlstr = "select crdays from view_account where Code = '" + codeid + "' and type= '" + typ + "'";

                        var connewstr = document.getElementById("<%=txtconnection.ClientID%>");
                        constr = connewstr.value
                        ColServices.clsServices.GetQueryReturnDatenew(constr, sqlstr, strdate, FillDueDate, ErrorHandler, TimeOutHandler);
                        //  ColServices.clsServices.GetQueryReturnDatenew(constr,strdate,FillDueDate,ErrorHandler,TimeOutHandler);
                    }


                    FillControllAcCode();
                    FillCustDetails(typ, codeid)
                }
                break;
            case "customername":
                var ddltyp = document.getElementById("<%=ddlType.ClientID%>");
                var select = document.getElementById("<%=ddlCustomerName.ClientID%>");
                if (ddltyp.value == '[Select]') {
                    alert('Please Select Type');
                    select.value = "[Select]";
                }
                else {

                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlCustomer.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;

                    var tcustcode = document.getElementById("<%=txtcustcode.ClientID%>");
                    tcustcode.value = select.options[select.selectedIndex].text;
                    var tcustname = document.getElementById("<%=txtcustname.ClientID%>");
                    tcustname.value = select.options[select.selectedIndex].value;


                    var typ = ddltyp.options[ddltyp.selectedIndex].value;
                    //sqlstr="select cur from view_account where Code = '"+ codeid +"' and type= '"+ typ +"'";
                    //ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrCodes,ErrorHandler,TimeOutHandler);

                    var txtfrdate = document.getElementById("<%=txtDate.ClientID%>");
                    var strdate = txtfrdate.value;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    sqlstr = "select crdays from view_account where Code = '" + codeid + "' and type= '" + typ + "'";
                    ColServices.clsServices.GetQueryReturnDatenew(constr, sqlstr, strdate, FillDueDate, ErrorHandler, TimeOutHandler);
                    FillControllAcCode();
                    FillCustDetails(typ, codeid)
                }
                break;
            case "salescode":
                var select = document.getElementById("<%=ddlSalesman.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSalesmanName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "salesname":
                var select = document.getElementById("<%=ddlSalesmanName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSalesman.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "controlaccode":
                var select = document.getElementById("<%=ddlConAccCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlConAccName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var tconaccode = document.getElementById("<%=txtConAccCode.ClientID%>");
                tconaccode.value = select.options[select.selectedIndex].value;
                var tconacname = document.getElementById("<%=txtConAccName.ClientID%>");
                tconacname.value = select.options[select.selectedIndex].text;
                break;
            case "controlacname":

                var select = document.getElementById("<%=ddlConAccName.ClientID%>");
                var selectname = document.getElementById("<%=ddlConAccCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                //  alert(select.options[select.selectedIndex].text);

                var tconaccode = document.getElementById("<%=txtConAccCode.ClientID%>");
                tconaccode.value = select.options[select.selectedIndex].text;
                var tconacname = document.getElementById("<%=txtConAccName.ClientID%>");
                tconacname.value = select.options[select.selectedIndex].value;
                break;

        }
    }

    function FillCustDDL(ddltp, lblcustcd, lblcustnm) {

        ddltyp = document.getElementById(ddltp);

        lblcustcode = document.getElementById(lblcustcd);
        lblcustname = document.getElementById(lblcustnm);
        var strcap = ddltyp.options[ddltyp.selectedIndex].text;
        var strtp = ddltyp.value;
        var sqlstr1 = null;
        var sqlstr2 = null;
        var connewstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connewstr.value

        if (ddltyp.value != '[Select]') {
            lblcustcode.innerHTML = strcap + 'Code <font color="Red"> *</font>';
            lblcustname.innerHTML = strcap + 'Name';
            sqlstr1 = "select Code,des from view_account where type = '" + strtp + "' order by code";
            sqlstr2 = "select des,Code from view_account where type = '" + strtp + "' order by des";
        }
        else {
            lblcustcode.innerHTML = 'Code <font color="Red"> *</font>';
            lblcustname.innerHTML = 'Name';
            sqlstr1 = "select top 10 Code,des from view_account   order by code";
            sqlstr2 = "select top 10  des,Code from view_account  order by des";
        }
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillCustCodes, ErrorHandler, TimeOutHandler);
        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillCustNames, ErrorHandler, TimeOutHandler);
    }



    function FillCustDetails(typ, codeid) {
        var crdsqlstr = "select cur,convrate,controlacctcode    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + codeid + "' and type='" + typ + "' ";
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 3, FiilCustDt, ErrorHandler, TimeOutHandler);

    }
    function FiilCustDt(result) {
        txtfill = document.getElementById("<%=txtCurrency.ClientID%>");
        txtgrdcrate = document.getElementById("<%=txtConversion.ClientID%>");
        ddlAConAccNm = document.getElementById("<%=ddlConAccName.ClientID%>");
        ddlAConAcc = document.getElementById("<%=ddlConAccCode.ClientID%>");
        txtAConAcc = document.getElementById("<%=txtConAccCode.ClientID%>");
        txtAConAccNm = document.getElementById("<%=txtConAccName.ClientID%>");

        txtfill.value = result[0];
        txtgrdcrate.value = result[1];
        ddlAConAccNm.value = result[2];
        var codeid = ddlAConAccNm.options[ddlAConAccNm.selectedIndex].text;
        ddlAConAcc.value = codeid;
        txtAConAcc.value = codeid;
        txtAConAccNm.value = result[2];

        // GrdExchangeRateChange(result[1]);
        var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');

        if (trim(txtfill.value) == trim(txtbase.value)) {
            txtgrdcrate.readOnly = true;
            txtgrdcrate.disabled = true;

        }
        else {
            txtgrdcrate.readOnly = false;
            txtgrdcrate.disabled = false;
        }
        chnagerate();
    }





    function FillDueDate(result) {
        var txtddate = document.getElementById("<%=txtDueDate.ClientID%>");

        txtddate.value = result;
    }

    function FillCustCodes(result) {
        var ddl = document.getElementById("<%=ddlCustomer.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCustNames(result) {
        var ddl = document.getElementById("<%=ddlCustomerName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCurrCodes(result) {

        var txt = document.getElementById("<%=txtCurrency.ClientID%>");
        if (result != null) {
            txt.value = result;
        }
        else {
            txt.value = "";
        }

        var sqlstr = "select convrate from currrates where currcode='" + result + "' and tocurr = (select option_selected from reservation_parameters where param_id=457)";
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillConRate, ErrorHandler, TimeOutHandler);


    }
    function FillConRate(result) {
        var txt = document.getElementById("<%=txtConversion.ClientID%>");
        //      	txt.value = "";

        txt.value = Number(result);

    }

    //Debit Note Grid Details

    function FillACCodeName(ddlcode, ddlname, txtgrdnar) {
        ddlc = document.getElementById(ddlcode);
        ddln = document.getElementById(ddlname);
        ddln.value = ddlc.options[ddlc.selectedIndex].text;
        txtgrdnarr = document.getElementById(txtgrdnar);
        txtnarr = document.getElementById("<%=txtNarration.ClientID%>");
        txtgrdnarr.value = txtnarr.value;
    }
    function FillCodeName(ddlcode, ddlname) {
        ddlc = document.getElementById(ddlcode);
        ddln = document.getElementById(ddlname);
        ddln.value = ddlc.options[ddlc.selectedIndex].text;
    }



    function FillPriceChanges(txtkval, txtcval, txtcon) {


        txtdkval = document.getElementById(txtkval);
        txtdcval = document.getElementById(txtcval);
        txtdcon = document.getElementById(txtcon);

        var prval = DecRound(DecRound(txtdcval.value) * parseFloat(txtdcon.value));

        txtdkval.value = prval;

        grdTotal();

    }
    function FillCombotoText(ddlc, txtt) {
        ddlcs = document.getElementById(ddlc);
        txtts = document.getElementById(txtt);

        var codeid = ddlcs.options[ddlcs.selectedIndex].text;
        txtts.value = codeid;
    }
    //------------- Sub Total Calculation
    function calculate_total(textot, arr, cnt) {
        var s = new String;
        var total = 0.0;
        var ctl = new Array();
        var dash = new Array();
        var str = new String;
        var slash = new Array();
        var dot = new Array();

        //alert("done1");
        s = arr;
        ctl = s.split("/");
        //var i=parseInt(cnt);
        //alert("done2");
        for (i = 0; i <= parseInt(cnt) - 1; i++) {
            str = (document.getElementById(ctl[i]).value)
            dash = str.split("-");
            slash = str.split("/");
            dot = str.split(".");
            if (isNaN(parseFloat(document.getElementById(ctl[i]).value))) {
            }
            else if (dash.length > 1) {
                alert("Please enter positive bill amount.");
                document.getElementById(ctl[i]).focus();
            }
            else if (slash.length > 1) {
                alert("Please enter numeric bill amount.");
                document.getElementById(ctl[i]).focus();
            }
            else if (dot.length > 2) {
                alert("Please enter valid bill amount.");
                document.getElementById(ctl[i]).focus();
            }
            else {
                total = total + parseFloat(document.getElementById(ctl[i]).value);
            }

        }
        document.getElementById(textot).value = total.toFixed(2);
    }













    function FillControllAcCode() {

        ddltyp = document.getElementById("<%=ddlType.ClientID%>");
        var strtp = ddltyp.options[ddltyp.selectedIndex].value;
        ddlIccode = document.getElementById("<%=ddlCustomer.ClientID%>");
        var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;

        var sqlstr1, sqlstr2
        sqlstr1 = "";
        sqlstr2 = "";
        if (strtp == 'C') {
            sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
            sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  acctmast.acctname";
        }
        else if (strtp == 'S') {
            sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"

            sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by acctmast.acctname"
        }
        else if (strtp == 'A') {
            sqlstr1 = " select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"

            sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' order by acctmast.acctname"

        }

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillConAccCodes, ErrorHandler, TimeOutHandler);
        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillConAccNames, ErrorHandler, TimeOutHandler);

    }
    function FillConAccCodes(result) {
        var ddl = document.getElementById("<%=ddlConAccCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }
    function FillConAccNames(result) {
        var ddl = document.getElementById("<%=ddlConAccName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    //end Debit Note Details ---------------------------------------------------------------------------------------    

    //-----    Common

    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }



    function grdTotal() {
        var objGridView = document.getElementById('<%=grdDebitNote.ClientID%>');
        var crtot = 0;
        var ktot = 0;
        var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');

        intRows = txtrowcnt.value;

        for (j = 1; j <= intRows; j++) {

            var crval = objGridView.rows[j].cells[5].children[0].value;
            var kval = objGridView.rows[j].cells[6].children[0].value;

            if (crval == '') { crval = 0; }
            if (kval == '') { kval = 0; }

            if (isNaN(crval) == true) { crval = 0; }
            if (isNaN(kval) == true) { kval = 0; }

            crtot = parseFloat(crtot) + parseFloat(crval);
            ktot = parseFloat(ktot) + parseFloat(kval);
        }
        var txtcrtot = document.getElementById('<%=txtCurrTotal.ClientID%>');
        var txtktot = document.getElementById('<%=txtKWDTotal.ClientID%>');

        txtcrtot.value = crtot;
        txtktot.value = ktot;

    }

    function chnagerate() {

        var objGridView = document.getElementById('<%=grdDebitNote.ClientID%>');
        var crtot = 0;
        var ktot = 0;
        var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');
        var txtCnvtR = document.getElementById('<%=txtConversion.ClientID%>');
        intRows = txtrowcnt.value;

        for (j = 1; j <= intRows; j++) {

            var crval = objGridView.rows[j].cells[5].children[0].value;
            var txtkval = objGridView.rows[j].cells[6].children[0];

            var kval = 0
            if (crval == '') { crval = 0; }


            if (isNaN(crval) == true) { crval = 0; }

            kval = DecRound(DecRound(crval) * parseFloat(txtCnvtR.value));
            if (kval == '') { kval = 0; }
            if (isNaN(kval) == true) { kval = 0; }
            txtkval.value = kval;
            crtot = DecRound(DecRound(crtot) + DecRound(crval));
            ktot = DecRound(DecRound(ktot) + DecRound(kval));
        }
        var txtcrtot = document.getElementById('<%=txtCurrTotal.ClientID%>');
        var txtktot = document.getElementById('<%=txtKWDTotal.ClientID%>');

        txtcrtot.value = crtot;
        txtktot.value = ktot;

    }

</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="field_heading" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text=" " Width="642px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD><TABLE><TBODY></TBODY></TABLE><TABLE><TBODY><TR><TD class="td_cell"><asp:Label id="Label5" runat="server" Text="Doc. No." Width="110px" CssClass="filed_caption"></asp:Label></TD><TD colSpan=1><asp:TextBox id="txtDocNo" tabIndex=1 runat="server" Width="194px" CssClass="field_input" ReadOnly="True" Enabled="False"></asp:TextBox></TD><TD class="td_cell"><asp:Label id="Label4" runat="server" Text="Date" Width="110px" CssClass="field_Caption"></asp:Label></TD><TD style="WIDTH: 176px" class="td_cell"><asp:TextBox id="txtDate" tabIndex=1 runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox>&nbsp;<asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;<cc1:MaskedEditValidator id="MskVFromDt" runat="server" Width="23px" CssClass="field_error" ControlExtender="MskFromDate" ControlToValidate="txtDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"></cc1:MaskedEditValidator></TD><TD class="td_cell" colSpan=1><asp:Label id="Label3" runat="server" Text="Type" Width="50px" CssClass="field_caption"></asp:Label></TD><TD class="td_cell" colSpan=1><SELECT style="WIDTH: 106px" id="ddlType" class="field_input" tabIndex=2 runat="server"> 
<OPTION value="[Select]" selected>[Select]</OPTION>
<OPTION value="C">Customer</OPTION>
<%--<OPTION value="S">Supplier</OPTION>
<OPTION value="A">Supplier Agent</OPTION>--%>
</SELECT></TD></TR><TR><TD class="td_cell"><asp:Label id="lblCustCode" runat="server" Text=" Code <font color='Red'> *</font>" Width="110px"></asp:Label></TD><TD colSpan=1><SELECT style="WIDTH: 200px" id="ddlCustomer" class="field_input" tabIndex=3 onchange="CallWebMethod('customercode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell"><asp:Label id="lblCustName" runat="server" Text=" Name" Width="110px" CssClass="field_caption"></asp:Label></TD><TD class="td_cell" colSpan=3><SELECT style="WIDTH: 300px" id="ddlCustomerName" class="field_input" tabIndex=4 onchange="CallWebMethod('customername');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="HEIGHT: 26px" class="td_cell"><asp:Label id="Label11" runat="server" Text="Currency" Width="110px" CssClass="field_Caption"></asp:Label></TD><TD style="HEIGHT: 26px" class="td_cell" colSpan=1><INPUT style="WIDTH: 194px" id="txtCurrency" class="field_input" tabIndex=5 readOnly type=text maxLength=1000 runat="server" /></TD><TD style="HEIGHT: 26px" class="td_cell"><asp:Label id="Label9" runat="server" Text="Conversion" Width="110px" CssClass="filed_caption"></asp:Label></TD><TD style="WIDTH: 176px; HEIGHT: 26px" class="td_cell"><INPUT style="WIDTH: 130px; TEXT-ALIGN: right" id="txtConversion" class="field_input" tabIndex=6 type=text maxLength=100 runat="server" /></TD><TD style="HEIGHT: 26px" class="td_cell"><asp:Label id="Label8" runat="server" Text="Due Date" Width="50px" CssClass="field_caption"></asp:Label></TD><TD style="HEIGHT: 26px" class="td_cell" colSpan=1><asp:TextBox id="txtDueDate" tabIndex=7 runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox>&nbsp;<asp:ImageButton id="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp; <cc1:MaskedEditValidator id="MskVRevDate" runat="server" CssClass="field_error" ControlExtender="MskRevDate" ControlToValidate="txtDueDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR><TR><TD style="HEIGHT: 18px" class="td_cell"><asp:Label id="Label12" runat="server" Text="Control A/C Code" Width="110px" CssClass="field_Caption"></asp:Label></TD><TD style="HEIGHT: 18px" class="td_cell"><SELECT style="WIDTH: 200px" id="ddlConAccCode" class="field_input" tabIndex=8 onchange="CallWebMethod('controlaccode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="HEIGHT: 18px" class="td_cell" colSpan=1><asp:Label id="Label13" runat="server" Text="Control A/C Name" Width="110px" CssClass="field_Caption"></asp:Label></TD><TD style="HEIGHT: 18px" class="td_cell" colSpan=3><SELECT style="WIDTH: 300px" id="ddlConAccName" class="field_input" tabIndex=9 onchange="CallWebMethod('controlacname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD class="td_cell"><asp:Label id="Label10" runat="server" Text="Reference No" Width="110px" CssClass="field_Caption"></asp:Label></TD><TD class="td_cell" colSpan=4><asp:TextBox id="txtReferenceNo" tabIndex=10 runat="server" Width="296px" CssClass="field_input"></asp:TextBox></TD><TD class="td_cell" colSpan=1><asp:TextBox id="txtdt" tabIndex=1000 runat="server" Width="5px" Enabled="False" Height="9px" ForeColor="White" BorderStyle="None" BorderColor="White" EnableTheming="False"></asp:TextBox></TD></TR><TR><TD style="HEIGHT: 22px" class="td_cell"><asp:Label id="Label6" runat="server" Text="Narration" Width="110px" CssClass="field_Caption"></asp:Label></TD><TD style="HEIGHT: 22px" class="td_cell" colSpan=5><SELECT style="WIDTH: 608px" id="ddlNarration" class="field_input" tabIndex=11 runat="server"></SELECT></TD></TR><TR><TD class="td_cell"></TD><TD class="td_cell" colSpan=5><INPUT style="WIDTH: 602px" id="txtNarration" class="field_input" tabIndex=12 type=text maxLength=200 runat="server" /></TD></TR><TR><TD style="HEIGHT: 22px" class="td_cell"><asp:Label id="Label7" runat="server" Text="Salesman Code" Width="110px" CssClass="field_Caption"></asp:Label></TD><TD style="HEIGHT: 22px" class="td_cell" colSpan=1><SELECT style="WIDTH: 200px" id="ddlSalesman" class="field_input" tabIndex=13 onchange="CallWebMethod('salescode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="HEIGHT: 22px" class="td_cell"><asp:Label id="Label1" runat="server" Text="Sales Man Name" Width="110px" CssClass="field_caption"></asp:Label></TD><TD style="HEIGHT: 22px" class="td_cell" colSpan=3><SELECT style="WIDTH: 300px" id="ddlSalesmanName" class="field_input" tabIndex=14 onchange="CallWebMethod('salesname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" align=center colSpan=6><asp:Label id="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana" Width="155px" CssClass="field_caption" ForeColor="Green" Font-Bold="True" BackColor="#E0E0E0"></asp:Label>&nbsp;<asp:Label id="lblcrdrCaption" runat="server" Width="192px" CssClass="field_caption"></asp:Label></TD></TR><TR><TD class="td_cell" colSpan=6><DIV style="WIDTH: 940px; HEIGHT: 200px" class="container"><asp:GridView id="grdDebitNote" tabIndex=15 runat="server" Font-Size="10px" Width="900px" CssClass="td_cell" BorderStyle="None" BorderColor="#999999" BackColor="White" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BorderWidth="1px">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:BoundField DataField="SrNo" Visible="False" HeaderText="LineNo"></asp:BoundField>
<asp:TemplateField Visible="False" HeaderText="LineID"><EditItemTemplate>
<asp:TextBox id="HTextBox1" runat="server" Text='<%# Bind("SrNo") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblLineID" runat="server" Text='<%# Bind("SrNo")  %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Account Code"><ItemTemplate>
&nbsp;<SELECT style="WIDTH: 108px" id="ddlAccountCode" class="field_input" runat="server"></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Account Name"><ItemTemplate>
<input type="text" name="accSearch"  class="field_input MyAutoCompleteClass" style="width:98% ; font " id="accSearch"  runat="server" />
<SELECT style="WIDTH: 128px" id="ddlAccountName" class="field_input MyDropDownListCustValue" runat="server"> <OPTION selected></OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost Center Code"><ItemTemplate>
&nbsp;<SELECT style="WIDTH: 108px" id="ddlCostCode" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost Center Name"><ItemTemplate>
&nbsp;<SELECT style="WIDTH: 128px" id="ddlCostName" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Particulars"><ItemTemplate>
&nbsp;<asp:TextBox id="txtParticulars" runat="server" CssClass="field_input" Width="148px" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Curr. Value"><ItemTemplate>
&nbsp;<asp:TextBox style="TEXT-ALIGN: right" id="txtCurrValue" runat="server" Width="96px" CssClass="field_input"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Value"><ItemTemplate>
&nbsp;<asp:TextBox style="TEXT-ALIGN: right" id="txtKWDValue" runat="server" CssClass="field_input" Width="96px" Enabled="False"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Delete"><ItemTemplate>
<asp:CheckBox id="chckDeletion" runat="server" Width="10px"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle  CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView></DIV></TD></TR><TR><TD class="td_cell" colSpan=6>
        <asp:Button id="btnAddLine" tabIndex=16 onclick="btnAddLine_Click" 
            runat="server" Text="Add Row" CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp; 
        <asp:Button id="btnDelLine" tabIndex=17 onclick="btnDelLine_Click" 
            runat="server" Text="DeleteRow" CssClass="field_button" 
            CausesValidation="False"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Sub Total&nbsp;&nbsp;<INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtCurrTotal" class="field_input" disabled type=text runat="server" /> <INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtKWDTotal" class="field_input" disabled type=text runat="server" /></TD></TR><TR><TD class="td_cell" align=left colSpan=6>
        <asp:Button id="btnAdjustBill" tabIndex=18 onclick="btnAdjustBill_Click" 
            runat="server" Text="Adjust Bill" CssClass="field_button"></asp:Button>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR><TR><TD style="HEIGHT: 26px" class="td_cell" align=right colSpan=6>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
    <INPUT style="VISIBILITY: hidden; WIDTH: 29px" id="txtAdjcolno" type=text maxLength=20 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 9px" id="txtbasecurr" type=text maxLength=20 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtConAccName" type=text maxLength=100 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtConAccCode" type=text maxLength=100 runat="server" />&nbsp;<INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustcode" type=text maxLength=100 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustname" type=text maxLength=200 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtgridrows" type=text maxLength=15 runat="server" /> 
<INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtdecimal" type=text maxLength=15 runat="server" /> <asp:CheckBox id="chkPost" tabIndex=19 runat="server" Text="Post/UnPost" Font-Size="10px" Font-Names="Verdana" Width="103px" CssClass="field_caption" ForeColor="Black" Font-Bold="True" BackColor="#FFC0C0" Checked="true"></asp:CheckBox> 
        <asp:Button id="btnSave" tabIndex=20 onclick="btnSave_Click" runat="server" 
            Text="Save" CssClass="field_button"></asp:Button>&nbsp;
             <asp:Button id="btnPrint" tabIndex=21 onclick="btnPrint_Click" runat="server" Text="Print" CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
            id="btnCancel" tabIndex=22 onclick="btnCancel_Click" runat="server" Text="Exit" 
            CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
            id="btnhelp" tabIndex=23 onclick="btnhelp_Click" runat="server" Text="Help" 
            CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="ClsExFromDate" runat="server" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:CalendarExtender id="ClsExRevDate" runat="server" PopupButtonID="ImgBtnRevDate" TargetControlID="txtDueDate" Format="dd/MM/yyyy"></cc1:CalendarExtender><cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"></cc1:MaskedEditExtender> <cc1:MaskedEditExtender id="MskRevDate" runat="server" TargetControlID="txtDueDate" Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"></cc1:MaskedEditExtender> 
</contenttemplate>
</asp:UpdatePanel>
 <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 
</asp:Content>

