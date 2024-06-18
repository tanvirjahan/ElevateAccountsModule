<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Freeform_invoice.aspx.vb" Inherits="Freeform_Invoice" MasterPageFile= "~/SubPageMaster.master" strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker" TagPrefix="ews" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript">
    var nodecround = null;
    var txtkwddamt = null;
    var txtkwdcamt = null;
    var strval = null;
    var txtpl1 = null;

    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
    }
 
    //Common
    var nodecround = null;
    function DecRound(amtToRound) {
        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));
        var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
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



    function FormValidation(state) 
    {

     
        if (document.getElementById("<%=ddlCustomer.ClientID%>").value == "[Select]") {
            alert("Select Customer code");
            return false;
        }
        else if (document.getElementById("<%=ddlCustomerName.ClientID%>").value == "[Select]") {
            alert("Select  Customer Name");
            return false;
        }
        else if (Number(document.getElementById("<%=txtConversion.ClientID%>").value) == 0) 
        {
            alert("Conversion Rate can not be 0");
            return false;
        }
        else if (Number(document.getElementById("<%=txtsalevalue.ClientID%>").value) == 0) {
            alert("Sale Price can not be 0");
            return false;
        }
        else if (document.getElementById("<%=txtCurrency.ClientID%>").value == '') 
        {
            alert("Currency can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtGuestName.ClientID%>").value == '') 
        {
            alert("Guest Name can not be blank");
            return false;
        }
        else if (document.getElementById("<%=txtRequestNo.ClientID%>").value == '') {
            alert("Request no. can not be blank");
            return false;
        }

        else if (document.getElementById("<%=txtReferenceNo.ClientID%>").value == '') {
            alert("Reference no. can not be blank");
            return false;
        }

        if (document.getElementById("<%=hdnSS.ClientID%>").value == 1) 
         {
             alert("Purchase value has changed Fill Cost");
             return false;
         }

        else 
        {

              if (state == 'New') { if (confirm('Are you sure you want to save ?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
            if (state == 'Cancel') { if (confirm('Are you sure you want to Cancel ?') == false) return false; }
            if (state == 'UndoCancel') { if (confirm('Are you sure you want to UndoCancel ?') == false) return false; }
        }
    }



    function CallWebMethod(methodType) 
    {
        switch (methodType) 
        {
            case "customercode":
                var select = document.getElementById("<%=ddlCustomer.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCustomerName.ClientID%>");
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                selectname.value = select.options[select.selectedIndex].text;
                var txtcustcode = document.getElementById("<%=txtcustcode.ClientID%>");
                txtcustcode.value = selectname.value

                FillCustDetails_head();
                break;
            case "customername":
                var select = document.getElementById("<%=ddlCustomerName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlCustomer.ClientID%>");
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                selectname.value = select.options[select.selectedIndex].text;
                var txtcustcode = document.getElementById("<%=txtcustcode.ClientID%>");
                txtcustcode.value = select.value

                FillCustDetails_head();
                break;
        }
    }


    function FillCustDDL(lblcustcd, lblcustnm) {

        lblcustcode = document.getElementById(lblcustcd);
        lblcustname = document.getElementById(lblcustnm);
        var sqlstr1 = null;
        var sqlstr2 = null;
        var connewstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connewstr.value


        lblcustcode.innerHTML = strcap + 'Code <font color="Red"> *</font>';
        lblcustname.innerHTML = strcap + 'Name';
        sqlstr1 = "select Code,des from view_account where type = 'C' order by code";
        sqlstr2 = "select des,Code from view_account where type = 'C' order by des";

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillCustCodes, ErrorHandler, TimeOutHandler);
        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillCustNames, ErrorHandler, TimeOutHandler);
    }



    function FillCustDetails(codeid) 
    {
        var crdsqlstr = "select cur,convrate,controlacctcode from view_account left outer join currrates on view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + codeid + "' and type='C' ";
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 3, FillCustDt, ErrorHandler, TimeOutHandler);

    }
    function FillCustDt(result) {
        txtfill = document.getElementById("<%=txtCurrency.ClientID%>");
        txtgrdcrate = document.getElementById("<%=txtConversion.ClientID%>");

        txtfill.value = result[0];
        txtgrdcrate.value = result[1];

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


    function FillCustDetails_head() {
   
         var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value
        var ddlCustomer = document.getElementById("<%=ddlCustomer.ClientID%>");

        var codeid = ddlCustomer.options[ddlCustomer.selectedIndex].text;

        var crdsqlstr = "select cur,convrate    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + codeid + "' and type='C' ";
        ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 3, FiilCustDt_head, ErrorHandler, TimeOutHandler);

    }
    function FiilCustDt_head(result) {

        var txtcurrcode = document.getElementById("<%=txtCurrency.ClientID%>");
        var txtconv = document.getElementById("<%=txtConversion.ClientID%>");

        txtcurrcode.value = result[0];
        txtconv.value = result[1];
        txtconv.readOnly = true;
        txtconv.readOnly = true;

        /*
        if (trim(txtcurrcode.value) == trim(txtbase.value)) 
        {
            txtconv.readOnly = true;
            txtconv.disabled = true;
        }
        else {
            txtconv.readOnly = false;
            txtconv.disabled = false;
        }*/
    }


    var txtCur1 = null;
    var txtExch1 = null;
    function FillPDetails_code(ddlac, ddln, ddltyp, txtCur, txtExch, hdncode1, hdnname1) 
    {
        ddlACode = document.getElementById(ddlac);
        ddlAName = document.getElementById(ddln);
        ddltyp = document.getElementById(ddltyp);
        txtCur1 = document.getElementById(txtCur);
        txtExch1 = document.getElementById(txtExch);

      var hdncode = document.getElementById(hdncode1);        
       var hdnname = document.getElementById(hdnname1);        

        var codeid = ddlACode.options[ddlACode.selectedIndex].text;
        ddlAName.value = codeid;

        hdncode.value = ddlAName.value
       hdnname.value = ddlACode.value;

        var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");
        hdnSS.value = 1;

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value
        var crdsqlstr = "select cur,convrate    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + codeid + "' and type='" + ddltyp.value + "' ";
        ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 2, FiilpurchaseDt, ErrorHandler, TimeOutHandler);

    }

    function FillPDetails_name(ddlac, ddln, ddltyp, txtCur, txtExch, hdncode1, hdnname1) {
        ddlACode = document.getElementById(ddlac);
        ddlAName = document.getElementById(ddln);
        ddltyp = document.getElementById(ddltyp);
        txtCur1 = document.getElementById(txtCur);
        txtExch1 = document.getElementById(txtExch);

        var hdncode = document.getElementById(hdncode1);
        var hdnname = document.getElementById(hdnname1);        


        var codeid = ddlAName.options[ddlAName.selectedIndex].text;
        ddlACode.value = codeid;
        hdncode.value = ddlAName.value;
        hdnname.value = ddlACode.value;

         
        var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");
        hdnSS.value = 1;

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value
        var crdsqlstr = "select cur,convrate    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + ddlAName.value + "' and type='" + ddltyp.value + "' ";
        ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 2, FiilpurchaseDt, ErrorHandler, TimeOutHandler);

    }


    function FiilpurchaseDt(result) 
    {
      if (txtCur1.value != result[0])
    {
    txtCur1.value = result[0];
    txtExch1.value = result[1];
    }
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

    function FillControllAcCode() 
    {

        ddlIccode = document.getElementById("<%=ddlCustomer.ClientID%>");
        var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;

        var sqlstr1, sqlstr2
        sqlstr1 = "";
        sqlstr2 = "";

        sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= 'C' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
        sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= 'C' and view_account.code='" + codeid + "' order by  acctmast.acctname";

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillConAccCodes, ErrorHandler, TimeOutHandler);
        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillConAccNames, ErrorHandler, TimeOutHandler);

    }

//    //end Debit Note Details ---------------------------------------------------------------------------------------    

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


    var ddlACode = null;
    var ddlAName = null;
    function LoadAcc(hdnacctyp, ddlAccType, ddlacccode, ddlaccname) {
        var hdnActype = document.getElementById(hdnacctyp);
        var ddlAccType = document.getElementById(ddlAccType);
        ddlACode = document.getElementById(ddlacccode);
        ddlAName = document.getElementById(ddlaccname);
        hdnActype.value = ddlAccType.value;       
        var strtp = ddlAccType.value;
        /* if(ddlAccType.value=='G')
        {
        ddlACode.disabled=true;
        ddlAName.disabled=true;
        }
        else
        {*/
        ddlACode.disabled = false;
        ddlAName.disabled = false;
        //}*/

        var hndStatus = document.getElementById("<%=hndStatus.ClientID%>");
        var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");
        hdnSS.value = 1;

        if (strtp == 'C') {
            var txtCustomerName = document.getElementById("<%=ddlCustomerName.ClientID%>");
            var txtCustomerCode = document.getElementById("<%=ddlCustomer.ClientID%>");

            if (ddlACode.value == txtCustomerName.value && ddlAName.value == txtCustomerCode.value) {
                hndStatus.value = 1;
            }
            else {
                hndStatus.value = 0;
            }
        }
        else {
            hndStatus.value = 0;
        }

       

        if (strtp != '[Select]') {
            sqlstr1 = "select Code,des from view_account where type = '" + strtp + "' and isnull(controlyn,'')='N' order by code";
            sqlstr2 = "select des,Code from view_account where type = '" + strtp + "' and isnull(controlyn,'')='N' order by des";
        }
        else {
            sqlstr1 = "select top 10 Code,des from view_account   order by code";
            sqlstr2 = "select top 10  des,Code from view_account   order by des";
        }

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillACodes, ErrorHandler, TimeOutHandler);
        ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillANames, ErrorHandler, TimeOutHandler);

    }
    function FillACodes(result) 
    {
        RemoveAll(ddlACode)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddlACode.options.add(option);
        }
        ddlACode.value = "[Select]";
    }

    function FillANames(result) {
        RemoveAll(ddlAName)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddlAName.options.add(option);
        }
        ddlAName.value = "[Select]";
    }



    var ddlConAcode = null;
    var ddlConAname = null;
    var lblCurrcode = null;
    var txtExchrate = null;
    var txtDbit = null;
    var txtKWDDbit = null;
    var hdnCurrcode = null;
    var hdnCAcode = null;
    function FillAccCode(ddlac, ddln, ddlconac, ddlconan, ddltyp, lblCur, txtExch, txtDebit, txtKWDDebit, hdnCurcode, hdnConAccode, index) {
        hdnCAcode = document.getElementById(hdnConAccode);
        hdnCurrcode = document.getElementById(hdnCurcode);
        lblCurrcode = document.getElementById(lblCur);
        txtExchrate = document.getElementById(txtExch);
        txtDbit = document.getElementById(txtDebit);
        txtKWDDbit = document.getElementById(txtKWDDebit);

        ddlACode = document.getElementById(ddlac);
        ddlAName = document.getElementById(ddln);
        ddlConAcode = document.getElementById(ddlconac);
        ddlConAname = document.getElementById(ddlconan);
        var ddltype = document.getElementById(ddltyp);
        var strtp = ddltype.value;

        var txtCustomerCode = document.getElementById("<%=ddlCustomer.ClientID%>");
        var txtCustomerName = document.getElementById("<%=ddlCustomerName.ClientID%>");

        var codeid = ddlACode.options[ddlACode.selectedIndex].text;
        ddlAName.value = codeid;


        var sqlstr1, sqlstr2, sqlstr3
        var sqlstr4, retlist

        ddlConAcode.disabled = false;
        ddlConAname.disabled = false;
        sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
        sqlstr2 = " select '' as acctname , '' as controlacctcode "
        sqlstr3 = "select cur,convrate,controlacctcode    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + codeid + "' and type='" + strtp + "' ";
        sqlstr4 = "select '' as controlacctcode"

        var hndStatus = document.getElementById("<%=hndStatus.ClientID%>");

        if (index == '0') {
            if (strtp == 'C') {
                var txtCustomerName = document.getElementById("<%=ddlCustomer.ClientID%>");
                var txtCustomerCode = document.getElementById("<%=ddlCustomerName.ClientID%>");
                txtCustomerCode.value = codeid;
                txtCustomerName.value = ddlAName.options[ddlAName.selectedIndex].text
                if (ddlACode.value == txtCustomerName.value && ddlAName.value == txtCustomerCode.value) {
                    hndStatus.value = 1;
                }
                else {
                    hndStatus.value = 0;
                }
            }
            else {
                hndStatus.value = 0;
            }
        }



        if (strtp == 'C') {
            sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
            sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  acctmast.acctname";
        }
        else if (strtp == 'S') {
            sqlstr1 = " select distinct partymast.controlacctcode controlacctcode , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"

            sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode controlacctcode      from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by acctmast.acctname"

            sqlstr4 = "select distinct partymast.controlacctcode as controlacctcode   from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "'"

        }
        else if (strtp == 'A') {
            sqlstr1 = " select distinct supplier_agents.controlacctcode controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"

            sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' "

            sqlstr4 = "select distinct supplier_agents.controlacctcode as controlacctcode   from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "'"

        }
        else if (strtp == 'G') {
            sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
            sqlstr2 = " select  '' as acctname , '' as controlacctcode "
            ddlConAcode.disabled = true;
            ddlConAname.disabled = true;
        }

        if (strtp != '[Select]') {
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillControlAcc, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillControlAccName, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr3, 3, FiilCustDt, ErrorHandler, TimeOutHandler);

            ColServices.clsServices.GetQueryReturnStringnew(constr, sqlstr4, Fillcntrlcode, ErrorHandler, TimeOutHandler);
        }
        else {
            alert('Please Select Account Type');
        }
    }


 function FillAccName(ddlac, ddln, ddlconac, ddlconan, ddltyp, lblCur, txtExch, txtDebit, txtKWDDebit, hdnCurcode, hdnConAccode, index) {
        hdnCAcode = document.getElementById(hdnConAccode);
        hdnCurrcode = document.getElementById(hdnCurcode);
        lblCurrcode = document.getElementById(lblCur);
        txtExchrate = document.getElementById(txtExch);
        txtDbit = document.getElementById(txtDebit);
        txtKWDDbit = document.getElementById(txtKWDDebit);

        ddlACode = document.getElementById(ddlac);
        ddlAName = document.getElementById(ddln);
        ddlConAcode = document.getElementById(ddlconac);
        ddlConAname = document.getElementById(ddlconan);
        var ddltype = document.getElementById(ddltyp);
        var txtCustomerCode = document.getElementById("<%=ddlCustomer.ClientID%>");
        var txtCustomerName = document.getElementById("<%=ddlCustomerName.ClientID%>");

        var strtp = ddltype.value;

        var code = ddlAName.options[ddlAName.selectedIndex].text;
        ddlACode.value = code;

        var codeid = ddlAName.value;


        var sqlstr1, sqlstr2, sqlstr3
        ddlConAcode.disabled = false;
        ddlConAname.disabled = false;
        sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
        sqlstr2 = " select '' as acctname , '' as controlacctcode "
        sqlstr3 = "select cur,convrate,controlacctcode    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" + codeid + "' and type='" + strtp + "' ";
        sqlstr4 = "select '' as controlacctcode"

        var hndStatus = document.getElementById("<%=hndStatus.ClientID%>");


        if (index == '0') {
            if (strtp == 'C') {
                var txtCustomerName = document.getElementById("<%=ddlCustomer.ClientID%>");
                var txtCustomerCode = document.getElementById("<%=ddlCustomerName.ClientID%>");

                txtCustomerCode.value = codeid;
                txtCustomerName.value = ddlAName.options[ddlAName.selectedIndex].text

                if (ddlACode.value == txtCustomerName.value && ddlAName.value == txtCustomerCode.value) {
                    hndStatus.value = 1;
                }
                else {
                    hndStatus.value = 0;
                }
            }
            else {
                hndStatus.value = 0;
            }
        }


        if (strtp == 'C') {
            sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
            sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  acctmast.acctname";
        }
        else if (strtp == 'S') {
            sqlstr1 = " select distinct partymast.controlacctcode controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' "

            sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode controlacctcode      from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' "

            sqlstr4 = "select distinct partymast.controlacctcode as controlacctcode   from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "'"

        }
        else if (strtp == 'A') {
            sqlstr1 = " select distinct supplier_agents.controlacctcode controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' "

            sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' "

            sqlstr4 = "select distinct supplier_agents.controlacctcode  as controlacctcode   from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "'"

        }
        else if (strtp == 'G') {
            sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
            sqlstr2 = " select  '' as acctname , '' as controlacctcode "
            ddlConAcode.disabled = true;
            ddlConAname.disabled = true;
        }

        if (strtp != '[Select]') {
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillControlAcc, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillControlAccName, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr3, 3, FiilCustDt, ErrorHandler, TimeOutHandler);

            ColServices.clsServices.GetQueryReturnStringnew(constr, sqlstr4, Fillcntrlcode, ErrorHandler, TimeOutHandler);

        }
        else {
            alert('Please Select Account Type');
        }

    }

    function Fillcntrlcode(result) {
        ddlConAname.value = result;
        var code = ddlConAname.options[ddlConAname.selectedIndex].text;
        hdnCAcode.value = ddlConAname.value
        ddlConAcode.value = code;
        // hdnConAcCode1.value=code;
    }	



    function FillControlAcc(result) {
        RemoveAll(ddlConAcode)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddlConAcode.options.add(option);
        }
        ddlConAcode.value = "[Select]";
    }
    function FillControlAccName(result) {
        RemoveAll(ddlConAname)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddlConAname.options.add(option);
        }
        ddlConAname.value = "[Select]";
    }


    function FiilCustDt(result) {
        if (lblCurrcode.innerHTML != result[0]) {
            lblCurrcode.innerHTML = result[0];
            hdnCurrcode.value = result[0];
            txtExchrate.value = result[1];
            txtDbit.value = parseFloat(txtKWDDbit.value) / parseFloat(txtExchrate.value);
            ddlConAname.value = result[2];
            hdnCAcode.value = result[2];
            var codeid1 = ddlConAname.options[ddlConAname.selectedIndex].text;
            ddlConAcode.value = codeid1;
        }
    }


    function calculatebaseamount(txtd, kwdd, er) {
        txtdamt = document.getElementById(txtd);
        txtkwddamt = document.getElementById(kwdd);
        txterate = document.getElementById(er);


        var decdTotal = parseFloat(txtdamt.value) * parseFloat(txterate.value);
        txtkwddamt.value = decdTotal
    }


    function GetKWDDrCr(txtd, txtc, kwdd, kwdc, er, strVal) {
        strval = strVal
        txtdamt = document.getElementById(txtd);
        txtcamt = document.getElementById(txtc);

        txtkwddamt = document.getElementById(kwdd);
        txtkwdcamt = document.getElementById(kwdc);
        txterate = document.getElementById(er);


        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        var decdTotal = parseFloat(txtdamt.value) * parseFloat(txterate.value);
//        ColServices.clsServices.RoundwithParameter(decdTotal, FillkwddAmount, ErrorHandler, TimeOutHandler);
        txtkwddamt.value = decdTotal

        var deccTotal = parseFloat(txtcamt.value) * parseFloat(txterate.value);
        txtkwdcamt.value = deccTotal
       // ColServices.clsServices.RoundwithParameter(deccTotal, FillkwdcAmount, ErrorHandler, TimeOutHandler);
        grdTotal(strval);
    }

    function FillkwddAmount(result) {
        
        txtkwddamt.value = result;
      //  grdTotal(strval);
    }
    function FillkwdcAmount(result) {
        txtkwdcamt.value = result;
     //   grdTotal(strval);
    }

    function grdTotal(str)
     {
        var txtdec = document.getElementById('<%=txtdecimal.ClientID%>');
        nodecround = Math.pow(10, parseInt(txtdec.value));

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value


        if (str == 'IP') {
            var objGridView = document.getElementById('<%=gv_IncomePosting.ClientID%>');
            var txtrowcnt = document.getElementById('<%=txtgridrowsip.ClientID%>');
            var j = 0;
            var valtotDr = 0;
            var valtotCr = 0;
            intRows = txtrowcnt.value;

          //  alert(intRows);

            for (j = 1; j <= intRows; j++)
             {
    
                var valDr = objGridView.rows[j].cells[10].children[0].value;
                var valCr = objGridView.rows[j].cells[11].children[0].value;
                var valExchRate = objGridView.rows[j].cells[7].children[0].value;
                if (valDr == '') 
                {
                    valDr = 0;
                 }
                if (valCr == '') { valCr = 0; }
                if (isNaN(valDr) == true) { valDr = 0; }
                if (isNaN(valCr) == true) { valCr = 0; }

                valtotDr = parseFloat(valtotDr) + parseFloat(valDr);
                valtotCr = parseFloat(valtotCr) + parseFloat(valCr);

              //  alert(valCr);
            }

           
            
            ColServices.clsServices.RoundwithParameter(parseFloat(valtotDr), FilltxttotDr, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.RoundwithParameter(parseFloat(valtotCr), FilltxttotCr, ErrorHandler, TimeOutHandler);
        }
        else if (str == 'CP') 
        {
            var objGridView = document.getElementById('<%=gv_CostPosting.ClientID%>');
            var txtrowcnt = document.getElementById('<%=txtgridrowscp.ClientID%>');
            var j = 0;
            var valtotDr = 0;
            var valtotCr = 0;
            intRows = txtrowcnt.value;

            for (j = 1; j <= intRows; j++) {
                var valDr = objGridView.rows[j].cells[10].children[0].value;
                var valCr = objGridView.rows[j].cells[11].children[0].value;
                var valExchRate = objGridView.rows[j].cells[7].children[0].value;

                if (valDr == '') { valDr = 0; }
                if (valCr == '') { valCr = 0; }
                if (isNaN(valDr) == true) { valDr = 0; }
                if (isNaN(valCr) == true) { valCr = 0; }

                valtotDr = parseFloat(valtotDr) + parseFloat(valDr);
                valtotCr = parseFloat(valtotCr) + parseFloat(valCr);

            }
            ColServices.clsServices.RoundwithParameter(parseFloat(valtotDr), FilltxttotDrCP, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.RoundwithParameter(parseFloat(valtotCr), FilltxttotCrCP, ErrorHandler, TimeOutHandler);
        }
    

    }





    function FilltxttotDr(result) {
        var txttotDr = document.getElementById('<%=txtTotalKWDDebit.ClientID%>');
        txttotDr.value = result;

        var txtPL = document.getElementById('<%=txtPL.ClientID%>');
        var txttotCrCP = document.getElementById('<%=txtTotalKWDCreditCP.ClientID%>');
        var txttotDr = document.getElementById('<%=txtTotalKWDDebit.ClientID%>');
        var txttotCr = document.getElementById('<%=txtTotalKWDCredit.ClientID%>');

        txtPL.value = ((parseFloat(txttotCr.value) + parseFloat(txttotDr.value)) - (parseFloat(result) + parseFloat(txttotCrCP.value)))/2; 

    }

    function FilltxttotCr(result) {
        var txttotCr = document.getElementById('<%=txtTotalKWDCredit.ClientID%>');
        txttotCr.value = result;

        var txtPL = document.getElementById('<%=txtPL.ClientID%>');
        var txttotCrCP = document.getElementById('<%=txtTotalKWDCreditCP.ClientID%>');
        var txttotDr = document.getElementById('<%=txtTotalKWDDebit.ClientID%>');
        var txttotCr = document.getElementById('<%=txtTotalKWDCredit.ClientID%>');

        txtPL.value = ((parseFloat(txttotCr.value) + parseFloat(txttotDr.value)) - (parseFloat(result) + parseFloat(txttotCrCP.value))) / 2; 

    }

    function FilltxttotDrCP(result) {
        var txttotDrCP = document.getElementById('<%=txtTotalKWDDebitCP.ClientID%>');
        txttotDrCP.value = result;

        var txtPL = document.getElementById('<%=txtPL.ClientID%>');
        var txttotCrCP = document.getElementById('<%=txtTotalKWDCreditCP.ClientID%>');
        var txttotDr = document.getElementById('<%=txtTotalKWDDebit.ClientID%>');
        var txttotCr = document.getElementById('<%=txtTotalKWDCredit.ClientID%>');

        txtPL.value = ((parseFloat(txttotCr.value) + parseFloat(txttotDr.value)) - (parseFloat(result) + parseFloat(txttotCrCP.value))) / 2; 
    }

    function FilltxttotCrCP(result) {
        var txttotCrCP = document.getElementById('<%=txtTotalKWDCreditCP.ClientID%>');
        txttotCrCP.value = result;

        var txtPL = document.getElementById('<%=txtPL.ClientID%>');
        var txttotCrCP = document.getElementById('<%=txtTotalKWDCreditCP.ClientID%>');
        var txttotDr = document.getElementById('<%=txtTotalKWDDebit.ClientID%>');
        var txttotCr = document.getElementById('<%=txtTotalKWDCredit.ClientID%>');

        txtPL.value = ((parseFloat(txttotCr.value) + parseFloat(txttotDr.value)) - (parseFloat(result) + parseFloat(txttotCrCP.value))) / 2; 

    }


</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid; width: 100%;"><TBODY><TR><TD class="field_heading" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text=" " Width="642px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD><TABLE><TBODY></TBODY></TABLE><TABLE><TBODY><TR><TD class="td_cell">
    <asp:Label id="Label5" runat="server" Text="Invoice No." Width="80px" 
        CssClass="filed_caption"></asp:Label></TD><TD colSpan=1>
        <asp:TextBox id="txtDocNo" tabIndex=1 runat="server" Width="120px" 
            CssClass="field_input" ReadOnly="True" Enabled="False"></asp:TextBox></TD><TD class="td_cell">
    <asp:Label id="Label4" runat="server" Text="Invoice Date" Width="80px" 
        CssClass="field_Caption" Height="16px"></asp:Label></TD>
        <TD style="WIDTH: 176px" class="td_cell">
        <asp:TextBox id="txtDate" tabIndex=1 runat="server" Width="80px" CssClass="field_input"></asp:TextBox>&nbsp;
        <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;
            <br />
        <cc1:MaskedEditValidator id="MskVFromDt" runat="server" Width="23px" CssClass="field_error" ControlExtender="MskFromDate" ControlToValidate="txtDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"></cc1:MaskedEditValidator>
        </TD><TD class="td_cell" colSpan=1>
        <asp:Label ID="Label14" runat="server" CssClass="filed_caption" 
            Text="Request No." Width="80px"></asp:Label>
    </TD><TD class="td_cell" colSpan=1>
        <asp:TextBox ID="txtRequestNo" runat="server" CssClass="field_input" 
            tabIndex="1" Width="120px"></asp:TextBox>
    </TD>
    <td class="td_cell">
        <asp:Label ID="Label15" runat="server" CssClass="field_Caption" 
            Text="Request Date"></asp:Label>
    </td>
    <td class="td_cell" style="WIDTH: 176px">
        <ews:DatePicker ID="dpFromReqDate" runat="server" 
            CalendarPosition="DisplayRight" DateFormatString="dd/MM/yyyy" 
            DateValue="" 
            RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" 
            tabIndex="22" />
    </td>
    </TR>
    <tr>
        <td class="td_cell">
            <asp:Label ID="lblCustCode" runat="server" 
                Text=" Code &lt;font color='Red'&gt; *&lt;/font&gt;" Width="80px"></asp:Label>
        </td>
        <td colspan="1">
            <select id="ddlCustomer" runat="server" class="field_input" name="D1" 
                onchange="CallWebMethod('customercode');" style="WIDTH: 150px" 
                tabindex="3">
                <option selected=""></option>
            </select>
        </td>
        <td class="td_cell" colspan="2">
            <select ID="ddlCustomerName" runat="server" class="field_input" name="D2" 
                onchange="CallWebMethod('customername');" style="WIDTH: 240px" tabindex="4">
                <option selected=""></option>
            </select>&nbsp;</td>
        <td class="td_cell" colspan="1">
            <asp:Label ID="Label11" runat="server" CssClass="field_Caption" Text="Currency" 
                Width="80px"></asp:Label>
        </td>
        <td class="td_cell" colspan="1">
            <INPUT style="WIDTH: 120px" id="txtCurrency" class="field_input" tabIndex=5 
                readOnly type=text maxLength=1000 runat="server" />
        </td>
        <td class="td_cell">
            <asp:Label ID="Label9" runat="server" CssClass="filed_caption" 
                Text="Conversion" Width="80px"></asp:Label>
        </td>
        <td class="td_cell">
            <INPUT style="WIDTH: 150px; TEXT-ALIGN: right" id="txtConversion" 
                class="field_input" tabIndex=6 type=text maxLength=100 runat="server" />
        </td>
    </tr>
    <tr>
        <td class="td_cell">
            <asp:Label ID="Label16" runat="server" CssClass="field_Caption" 
                Text="Arrival Date" Width="80px"></asp:Label>
        </td>
        <td colspan="1">
            <ews:DatePicker ID="dpFromCheckindate" runat="server" 
                DateFormatString="dd/MM/yyyy" Font-Size="Medium" 
                RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" 
                tabIndex="18" />
        </td>
        <td class="td_cell">
            <asp:Label ID="Label17" runat="server" CssClass="field_Caption" 
                Text="Departure Date" Width="90px"></asp:Label>
        </td>
        <td class="td_cell">
            <ews:DatePicker ID="dpFromCheckOut" runat="server" 
                DateFormatString="dd/MM/yyyy" DateValue="" 
                RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" 
                tabIndex="20" />
        </td>
        <td class="td_cell">
            <asp:Label ID="Label8" runat="server" CssClass="field_Caption" Text="Amount" 
                Width="50px"></asp:Label>
        </td>
        <td class="td_cell" colspan="3">
            <INPUT id="txtSaleValue" class="field_input" type=text runat="server" style="TEXT-ALIGN: right" />
        </td>
    </tr>
    <tr>
        <td class="td_cell">
            <asp:Label ID="Label10" runat="server" CssClass="field_Caption" Height="16px" 
                Text="Customer Ref" Width="80px"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtReferenceNo" runat="server" CssClass="field_input" 
                tabIndex="10" Width="390px"></asp:TextBox>
        </td>
        <td class="td_cell">
            <asp:Label ID="Label18" runat="server" CssClass="field_Caption" 
                Text="Guest Name" Width="80px"></asp:Label>
        </td>
        <td class="td_cell" colspan="3">
            <asp:TextBox ID="txtGuestName" runat="server" CssClass="field_input" 
                tabIndex="10" Width="360px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="td_cell">
            <asp:Label ID="Label6" runat="server" CssClass="field_Caption" Height="16px" 
                Text="Narration" Width="80px"></asp:Label>
        </td>
        <td colspan="3">
            <select id="ddlNarration" runat="server" class="field_input" name="D3" 
                style="WIDTH: 390px" tabindex="11">
            </select>
        </td>
        <td class="td_cell" colspan="4">
            <INPUT style="WIDTH: 445px" id="txtNarration" 
            class="field_input" tabIndex=12 type=text maxLength=200 runat="server" />
        </td>
    </tr>
    <tr>
        <td class="td_cell">
            <INPUT id="txtpurchaseid" class="field_input" type=text runat="server" 
                style="TEXT-ALIGN: right; width: 106px; visibility: hidden;" readonly="readonly" />
        </td>
        <td colspan="3">
            <asp:Button ID="btnfillincome" runat="server" CausesValidation="False" 
                CssClass="field_button" tabIndex="22" Text="Fill Sales" Width="164px" />
        </td>
        <td class="td_cell" colspan="4">
            <asp:Label ID="lblPostmsg" runat="server" BackColor="#E0E0E0" 
                CssClass="field_caption" Font-Bold="True" Font-Names="Verdana" Font-Size="12px" 
                ForeColor="Green" Text="UnPosted" Width="155px"></asp:Label>
        </td>
    </tr>
    </TBODY></TABLE></TD></TR>
    <tr>
        <td style="HEIGHT: 170px">
            <div class="container" style="HEIGHT: 200px" ID="div_income" runat="server" >
                <asp:GridView ID="gv_IncomePosting" runat="server" AutoGenerateColumns="False" 
                    CssClass="grdstyle" tabIndex="1">
                    <Columns>
                    <asp:TemplateField HeaderText="Request Type">
                            <ItemTemplate>
                                <asp:Label ID="lblrequesttype" runat="server" Text='<%# Bind("requesttype") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblrequestlineno" runat="server" Text='<%# Bind("requestlineno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Account Type">
                            <ItemTemplate>
                                <asp:Label ID="lblAcctype" runat="server" Text='<%# Bind("acc_type") %>'></asp:Label>
                                <select ID="ddlAccType" runat="server" class="drpdown" style="width: 45px">
                                    <option selected="selected"></option>
                                </select>
                                <asp:HiddenField ID="hdnAccType" runat="server" 
                                    value='<%# Bind("acc_type") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account code">
                            <ItemTemplate>
                                <select ID="ddlAcctCode" runat="server" class="drpdown" style="width: 96px">
                                    <option selected="selected"></option>
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Name">
                            <ItemTemplate>
                                <select ID="ddlAcctName" runat="server" class=" drpdown" style="width: 176px">
                                    <option selected="selected"></option>
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Control Account Code">
                            <ItemTemplate>
                                <asp:Label ID="lblCtrlAccCode" runat="server" Text='<%# Bind("controlcode") %>'></asp:Label>
                                <br />
                                <select ID="ddlConAccCode" runat="server" class="drpdown" style="width: 100px" 
                                    tabindex="0">
                                </select>
                                <asp:HiddenField ID="hdnConAccCode" runat="server" 
                                    value='<%# Bind("controlcode") %>' />
                                <br />
                                <select ID="ddlConAccName" runat="server" class="drpdown" style="width: 100px" 
                                    tabindex="0">
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency Code">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrCode" runat="server" Text='<%# Bind("currcode") %>'></asp:Label>
                                <asp:HiddenField ID="hdnCurrCode" runat="server" 
                                    value='<%# Bind("currcode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exch Rate">
                            <ItemTemplate>
                                <input id="txtExchRate" value='<%# bind("convrate") %>' runat="server" class="txtbox" maxlength="12"
                                    style="width: 56px; text-align: right;" type="text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Debit">
                            <ItemTemplate>
                                <input id="txtDebit" value='<%# bind("debit") %>' runat="server" class="txtbox" type="text" style="width: 72px; text-align: right;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit">
                            <ItemTemplate>
                                <input id="txtCredit" value='<%# bind("credit") %>' runat="server" class="txtbox" type="text" style="width: 72px; text-align: right;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base Debit">
                            <ItemTemplate>
                                <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDDebit" class="txtbox" disabled type=text value='<%# bind("basedebit") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base Credit">
                            <ItemTemplate>
                                <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDCredit" class="txtbox" disabled type=text value='<%# bind("basecredit") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Narration">
                            <ItemTemplate>
                                <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtnarration" class="txtbox"  type=text  runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Supplier Name" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierName" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check In" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCheckIn" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check Out" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCheckOut" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ReconfNo" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblReConfNo" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acc_Lineno" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblAccLineno" runat="server" Text='<%# Bind("acc_lineno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="rlineno" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRlineNo" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="slineno" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblSLineno" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account code" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblAcctCode" runat="server" Text='<%# Bind("acc_code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Name" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("acc_name") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAcctName" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actual code" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblActualCode" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete"><ItemTemplate>
                        <asp:CheckBox id="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
                        </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <RowStyle CssClass="grdRowstyle" Wrap="False" />
                    <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                    <HeaderStyle CssClass="grdheader" Wrap="False" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
            </div>
            <asp:Button ID="btnAdd" runat="server" CssClass="field_button" Font-Bold="True" 
                tabIndex="7" Text="Add Row" Visible="False" />
            &nbsp;
            <asp:Button ID="btnDelLine" runat="server" CausesValidation="False" 
                CssClass="field_button"  tabIndex="8" 
                Text="DeleteRow" Visible="False" />
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblMessageIP" runat="server" CssClass="lblmsg" Font-Bold="True" 
                Font-Names="Verdana" Font-Size="9pt" Visible="False">Records Not Found.</asp:Label>
        </td>
    </tr>
    <TR><TD class="td_cell" align=center>
        &nbsp;&nbsp;</TD></TR>
    <tr>
        <td align="center" class="td_cell">
            <table style="width: 100%">
                <tr>
                    <td>
                        <input id="txtgridrowsip" runat="server" 
                        style="VISIBILITY: hidden; WIDTH: 8px" type="text" />
                        <INPUT style="VISIBILITY: hidden; WIDTH: 8px" id="txtgridrowscp" type=text 
                        runat="server" />
                        <INPUT style="VISIBILITY: hidden; WIDTH: 8px" id="txtmaxacclineno" type=text 
                        runat="server" />
                    </td>
                    <td style="width: 65px">
                        <asp:Label ID="lblITot_DB" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <INPUT style="TEXT-ALIGN: right" id="txtTotalKWDDebit" class="txtbox" readOnly 
                type=text runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblItot_Cr" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <INPUT 
                style="TEXT-ALIGN: right" id="txtTotalKWDCredit" class="txtbox" readOnly 
                type=text runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center" class="td_cell">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="left" class="td_cell">
            <div class="container" style="HEIGHT: 150px">
                <asp:GridView ID="gv_purchase_detail" runat="server" AutoGenerateColumns="False" 
                    CssClass="grdstyle" tabIndex="2">
                    <Columns>

                        <asp:TemplateField HeaderText="Services">
                            <ItemTemplate>
                                <asp:Label ID="lblservicetype" runat="server" Visible="False"   Text='<%# Bind("requesttype") %>'></asp:Label>
                            <SELECT style="WIDTH: 109px" id="ddlservices" class="drpdown" runat="server"><OPTION value="H" 
                              selected>Hotel</OPTION><OPTION value="O">Other Services</OPTION></SELECT>
                                <asp:HiddenField ID="hdnserviceType" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" Wrap="True" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Account Type">
                            <ItemTemplate>
                                <asp:Label ID="lblAcctype" runat="server" Visible="False"   Text='<%# Bind("acc_type") %>'></asp:Label>
                                <select ID="ddlAccType" runat="server" class="drpdown" style="width: 70px" >
                                    <option selected="selected"></option>
                                </select>
                                <asp:HiddenField ID="hdnAccType" runat="server" value='<%# Bind("acc_type") %>'/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account code">
                            <ItemTemplate>
                                <select ID="ddlAcctCode" runat="server" class="drpdown" style="width: 96px">
                                    <option selected="selected"></option>
                                </select>
                                <asp:HiddenField ID="hdnAcctCode" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Name">
                            <ItemTemplate>
                                <select ID="ddlAcctName" runat="server" class="drpdown" style="width: 220px">
                                    <option selected="selected"></option>
                                </select>
                                <asp:HiddenField ID="hdnAcctName" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Currency Code">
                            <ItemTemplate>
                                <input id="txtcurrcode"  runat="server" class="txtbox" maxlength="12"
                                    style="width: 56px; text-align: right;" type="text" value='<%# bind("currcode") %>'/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" Wrap="True" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Exch Rate">
                            <ItemTemplate>
                                <input id="txtExchRate"  runat="server" class="txtbox" maxlength="12"
                                    style="width: 56px; text-align: right;" type="text" value='<%# bind("convrate") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <input id="txtamount"  runat="server" class="txtbox" type="text" style="width: 72px; text-align: right;"  value='<%# bind("amount") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base Amount">
                            <ItemTemplate>
                                <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtbaseamount" class="txtbox" disabled type=text  runat="server" value='<%# bind("baseamount") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SrNo" Visible="False" HeaderText="LineNo"></asp:BoundField>
                        <asp:TemplateField Visible="False" HeaderText="LineID"><EditItemTemplate>
                        <asp:TextBox id="HTextBox1" runat="server" Text='<%# Bind("SrNo") %>'></asp:TextBox> 
                        </EditItemTemplate>
                        <ItemTemplate>
                        <asp:Label id="lblsno" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label> 
                        </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Account code" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblAcctCode" runat="server" Text='<%# Bind("acc_code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Account Name" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("accname") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAcctName" runat="server"  ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete"><ItemTemplate>
                        <asp:CheckBox id="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
                        </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <RowStyle CssClass="grdRowstyle" Wrap="False" />
                    <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                    <HeaderStyle CssClass="grdheader" Wrap="False" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
            </div>
        </td>
    </tr>
    <tr>
        <td align="left" class="td_cell">
            <asp:Button ID="btnAdd_det" runat="server" CssClass="field_button" 
                Font-Bold="True" tabIndex="7" Text="Add Row" Visible="False" />
            &nbsp;
            <asp:Button ID="btnDelLine_det" runat="server" CausesValidation="False" 
                CssClass="field_button" tabIndex="8" Text="DeleteRow" Visible="False" />
        </td>
    </tr>
    <tr>
        <td align="center" class="td_cell">
            <asp:Button ID="btnfilldetail" runat="server" CausesValidation="False" 
                CssClass="field_button" tabIndex="22" Text="Fill Cost" Width="100px" />
        </td>
    </tr>
    <tr>
        <td align="center" class="td_cell">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="center" class="td_cell">
            <div class="container" style="HEIGHT: 150px" ID="div_cost" runat="server">
                <asp:GridView ID="gv_CostPosting" runat="server" AutoGenerateColumns="False" 
                    CssClass="grdstyle" tabIndex="2">
                    <Columns>

                        <asp:TemplateField HeaderText="Request Type">
                            <ItemTemplate>
                                <asp:Label ID="lblrequesttype" runat="server" Text='<%# Bind("requesttype") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblrequestlineno" runat="server" Text='<%# Bind("requestlineno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Account Type">
                            <ItemTemplate>
                                <asp:Label ID="lblacctype" runat="server" Text='<%# Bind("acc_type") %>'></asp:Label>
                                <select ID="ddlAccType" runat="server" class="drpdown" style="width: 45px">
                                    <option selected="selected"></option>
                                </select>
                                <asp:HiddenField ID="hdnAccType" runat="server" 
                                    value='<%# Bind("acc_type") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account code">
                            <ItemTemplate>
                                <select ID="ddlAcctCode" runat="server" class="drpdown" style="width: 96px">
                                    <option selected="selected"></option>
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Name">
                            <ItemTemplate>
                                <select ID="ddlAcctName" runat="server" class=" drpdown" style="width: 176px">
                                    <option selected="selected"></option>
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Control Account Code">
                            <ItemTemplate>
                                <asp:Label ID="lblCtrlAccCode" runat="server" Text='<%# Bind("controlcode") %>'></asp:Label>
                                <br />
                                <select ID="ddlConAccCode" runat="server" class="drpdown" style="width: 100px" 
                                    tabindex="0">
                                </select>
                                <asp:HiddenField ID="hdnConAccCode" runat="server" 
                                    value='<%# Bind("controlcode") %>' />
                                <br />
                                <select ID="ddlConAccName" runat="server" class="drpdown" style="width: 100px" 
                                    tabindex="0">
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency Code">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrCode" runat="server" Text='<%# Bind("currcode") %>'></asp:Label>
                                <asp:HiddenField ID="hdnCurrCode" runat="server" 
                                    value='<%# Bind("currcode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exch Rate">
                            <ItemTemplate>
                                <input id="txtExchRate" value='<%# bind("convrate") %>' runat="server" class="txtbox" maxlength="12"
                                    style="width: 56px; text-align: right;" type="text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Debit">
                            <ItemTemplate>
                                <input id="txtDebit" value='<%# bind("debit") %>' runat="server" class="txtbox" type="text" style="width: 72px; text-align: right;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit">
                            <ItemTemplate>
                                <input id="txtCredit" value='<%# bind("credit") %>' runat="server" class="txtbox" type="text" style="width: 72px; text-align: right;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base Debit">
                            <ItemTemplate>
                                <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDDebit" class="txtbox" disabled type=text value='<%# bind("basedebit") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base Credit">
                            <ItemTemplate>
                                <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDCredit" class="txtbox" disabled type=text value='<%# bind("basecredit") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Narration">
                            <ItemTemplate>
                                <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtnarration" class="txtbox"  type=text  runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supplier Name" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierName" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check In" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCheckIn" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check Out" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCheckOut" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ReconfNo" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblReConfNo" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acc_Lineno" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblAccLineno" runat="server" Text='<%# Bind("acc_lineno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="rlineno" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRlineNo" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="slineno" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblSLineno" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account code" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblAcctCode" runat="server" Text='<%# Bind("acc_code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Name" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" ></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAcctName" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actual code" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblActualCode" runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" Visible="false" ><ItemTemplate>
                        <asp:CheckBox id="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
                        </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="grdRowstyle" Wrap="False" />
                    <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                    <HeaderStyle CssClass="grdheader" Wrap="False" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
            </div>
        </td>
    </tr>
    <tr>
        <td align="left" class="td_cell">
            <asp:Button ID="btnadd_CP" runat="server" CssClass="field_button" 
                Font-Bold="True" tabIndex="7" Text="Add Row" Visible="False" />
            &nbsp;
            <asp:Button ID="btnDelLine_cp" runat="server" CausesValidation="False" 
                CssClass="field_button" tabIndex="8" Text="DeleteRow" Visible="False" />
        </td>
    </tr>
    <tr>
        <td align="center" class="td_cell">
            <asp:Label ID="lblCTot_DB" runat="server" Text="Label"></asp:Label>
            &nbsp;<INPUT style="TEXT-ALIGN: right" id="txtTotalKWDDebitCP" class="txtbox" 
                readOnly type=text runat="server" />&nbsp;&nbsp;
            <asp:Label ID="lblCtot_Cr" runat="server" Text="Label"></asp:Label>
            &nbsp;
            <INPUT style="TEXT-ALIGN: right" id="txtTotalKWDCreditCP" class="txtbox" 
                readOnly type=text runat="server" />
            &nbsp;
            <asp:Label ID="lblpl" runat="server" Text="P/L"></asp:Label>
            &nbsp;&nbsp;
            <INPUT style="TEXT-ALIGN: right" id="txtPL" class="txtbox" readOnly 
                type=text runat="server" />
        </td>
    </tr>
    <TR><TD class="td_cell">&nbsp;</TD></TR><TR>
        <TD class="td_cell" align=left>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR><TR>
        <TD style="HEIGHT: 26px" class="td_cell" align=right>
            &nbsp;<input id="txtcustcode" type="text" style="width: 10px; visibility: hidden;" />
    <input id="txtconnection" style="width: 12px; visibility: hidden; height: 9px;" type="text" runat="server" />
    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
            <input 
                id="txtAdjcolno" runat="server" style="visibility: hidden; width: 29px;
        " type="text" maxLength="20" />
    <INPUT style="VISIBILITY: hidden; WIDTH: 9px" id="txtbasecurr" type=text maxLength=20 runat="server" /> 
            <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtConAccName" type=text maxLength=100 runat="server" /> 
            <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtConAccCode" type=text maxLength=100 runat="server" /> 
            &nbsp;<INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustcode" type=text maxLength=100 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustname" type=text maxLength=200 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtgridrows" type=text maxLength=15 runat="server" /> 
<INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtdecimal" type=text maxLength=15 runat="server" /> <asp:CheckBox id="chkPost" tabIndex=19 runat="server" Text="Post/UnPost" Font-Size="10px" Font-Names="Verdana" Width="103px" CssClass="field_caption" ForeColor="Black" Font-Bold="True" BackColor="#FFC0C0"></asp:CheckBox> 
            &nbsp;&nbsp;&nbsp; 
        <asp:Button id="btnSave" tabIndex=20 onclick="btnSave_Click" runat="server" 
            Text="Save" CssClass="field_button"></asp:Button>&nbsp;
             <asp:Button id="btnPrint" tabIndex=21 onclick="btnPrint_Click" runat="server" Text="Print" CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
            id="btnCancel" tabIndex=22 onclick="btnCancel_Click" runat="server" Text="Exit" 
            CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
            id="btnhelp" tabIndex=23 onclick="btnhelp_Click" runat="server" Text="Help" 
            CssClass="field_button"></asp:Button></TD></TR>
    <tr>
        <td align="right" class="td_cell" style="HEIGHT: 26px">
            &nbsp;</td>
    </tr>
    </TBODY></TABLE><cc1:CalendarExtender id="ClsExFromDate" runat="server" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"></cc1:MaskedEditExtender>  
           <asp:HiddenField ID="hndStatus" runat="server" Value="1" />
            <asp:HiddenField ID="hdnSS" runat="server" Value="0" />
</contenttemplate>
</asp:UpdatePanel>
 <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 

</asp:Content>