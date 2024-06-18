<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="Receipts.aspx.vb" Inherits="Receipts" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="JavaScript" type="text/javascript">
        window.history.forward(1);  
    </script>
<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

    <style type="text/css">
        .hiddencol
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function ConfirmContinue(msg, btnId) {
            var btn = document.getElementById(btnId);
            var hdn = document.getElementById("<%= hdnValidate.ClientID%>");
            if (confirm(msg) == true) {
                btn.click();
            }
            else {
                hdn.click();
            }
        }



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

        //----------------------------------------
        var nodecround = null;
        var txtgrdcrate = null;
        var txtfill = null;
        var txtcrate = null;
        var ddlACode = null;
        var ddlAName = null;
        var ddlAConAcc = null;
        var ddlAConAccNm = null;
        var txtAConAcc = null;
        var txtAConAccNm = null;
        var sqlstr = null;
        var txtgrdDBAmt = null;
        var txtgrdCnvtAmt = null;
        var sddltyp = null;
        var sddlACode = null;

        function trim(stringToTrim) {
            return stringToTrim.replace(/^\s+|\s+$/g, "");
        }
        function DecRound(amtToRound) {

            var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
            nodecround = Math.pow(10, parseInt(txtdec.value));
            var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
            return parseFloat(rdamt);
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



        function DecFormat(value) {

            var rdamt = null;
            var indx = value.indexOf('.');
            var deci = document.getElementById("<%=txtdecimal.ClientID%>");
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

        function FillCodeName(ddlcode, ddlname) {
            ddlc = document.getElementById(ddlcode);
            ddln = document.getElementById(ddlname);
            ddln.value = ddlc.options[ddlc.selectedIndex].text;
        }

        function filldept(ddldept) {
            ddldept = document.getElementById(ddldept);

            ddldept.value = ddldept.value
        }

        function OnchangeCashBank(ddlC) {
            ddlC = document.getElementById(ddlC);
            FillBankCashDet();
            if (ddlC.value == 'Cash') {
                var ddl = document.getElementById("<%=ddlCustBank.ClientId%>");
                //   ddl.style.visibility = "hidden";
                ddl.style.visibility = "visible";
                var txtchq = document.getElementById("<%=txtChequeNo.ClientId%>");
                //   txtchq.style.visibility = "hidden";
                txtchq.style.visibility = "visible";
                var txtchqdt = document.getElementById("<%=txtChequeDate.ClientId%>");
                //   txtchqdt.style.visibility = "hidden";
                txtchqdt.style.visibility = "visible";

                var img = document.getElementById("<%=ImageButton1.ClientId%>");
                //    img.style.visibility = "hidden";
                img.style.visibility = "Visible";

                var lbl1 = document.getElementById("<%=lblChN.ClientId%>");
                //   lbl1.style.visibility = "hidden";
                lbl1.style.visibility = "Visible";

                var lbl2 = document.getElementById("<%=lblChD.ClientId%>");
                //   lbl2.style.visibility = "hidden";
                lbl2.style.visibility = "Visible";
                var lbl3 = document.getElementById("<%=lblChB.ClientId%>");
                //   lbl3.style.visibility = "hidden";
                lbl3.style.visibility = "Visible";
            }
            else if (ddlC.value == 'Bank') {

                var txtTranTypes = document.getElementById("<%=txtTranType.ClientId%>");
                var txtchq = document.getElementById("<%=txtChequeNo.ClientId%>");
                txtchq.style.visibility = "visible";
                var txtchqdt = document.getElementById("<%=txtChequeDate.ClientId%>");
                txtchqdt.style.visibility = "visible";
                var img = document.getElementById("<%=ImageButton1.ClientId%>");
                img.style.visibility = "visible";
                var lbl1 = document.getElementById("<%=lblChN.ClientId%>");
                lbl1.style.visibility = "visible";
                var lbl2 = document.getElementById("<%=lblChD.ClientId%>");
                lbl2.style.visibility = "visible";
                var ddl = document.getElementById("<%=ddlCustBank.ClientId%>");
                var lbl3 = document.getElementById("<%=lblChB.ClientId%>");
                if (txtTranTypes.value.trim() == 'RV') {
                    ddl.style.visibility = "visible";
                    lbl3.style.visibility = "visible";
                }
                else //if (trim(txtTranTypes.value)=='PV' )
                {
                    ddl.style.visibility = "hidden";
                    lbl3.style.visibility = "hidden";
                }
            }
        }

        function FillBankCashDet() {

            var strSqlQry1;
            var strSqlQry2;
            var strSqlQry3;
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>");
            var ddlcashbank = document.getElementById('<%=ddlCashBank.ClientID%>');
            if (ddlcashbank.value == 'Cash') {
                strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y' and  bank_master_type.cashbanktype='C' order by acctcode ";
                strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code  and  bankyn='Y' and bank_master_type.cashbanktype='C'  order by acctname";
                strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y' and bank_master_type.cashbanktype='C'  order by acctcode";
            }
            else if (ddlcashbank.value == 'Bank') {
                strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y' and  bank_master_type.cashbanktype='B' order by acctcode ";
                strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code  and  bankyn='Y' and bank_master_type.cashbanktype='B'  order by acctname";
                strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y' and bank_master_type.cashbanktype='B'  order by acctcode";
            }
            else {
                strSqlQry1 = "select top 10 acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y'  order by acctcode ";
                strSqlQry2 = "select top 10  acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code  and  bankyn='Y'   order by acctname";
                strSqlQry3 = "select top 10 Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y' order by acctcode";
            }
            ColServices.clsServices.GetQueryReturnStringListnew(constr, strSqlQry1, FillBankCodes, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, strSqlQry2, FillBankNames, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, strSqlQry3, FillCurrCodes, ErrorHandler, TimeOutHandler);

        }

        function FillBankCodes(result) {

            ddlbankcd = document.getElementById('<%=ddlAccCode.ClientID%>');
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";
        }
        function FillBankNames(result) {
            ddlbankcd = document.getElementById('<%=ddlAccName.ClientID%>');
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";
        }
        function FillCurrCodes(result) {
            ddlbankcd = document.getElementById('<%=ddlCurrCode.ClientID%>');
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";
        }





        function FillCode(ddlIccd, ddlIcnm) {

            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;
            ddlIcname.value = codeid;

            var txtacccode = document.getElementById('<%=txtAccCode.ClientID%>');
            var txtaccname = document.getElementById('<%=txtAccName.ClientID%>');
            var txtcurrcode = document.getElementById('<%=txtCurrCode.ClientID%>');
            txtacccode.value = ddlIccode.options[ddlIccode.selectedIndex].value;
            txtaccname.value = ddlIccode.options[ddlIccode.selectedIndex].text;
            txtcurrcode.value = ddlIccode.options[ddlIccode.selectedIndex].text;

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value


            var ddlcur = document.getElementById('<%=ddlCurrCode.ClientID%>');
            ddlcur.value = codeid;
            var currsqlstr
            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');
            txtdivcode = document.getElementById("<%=txtDivCode.ClientId%>");

            //sqlstr="select convrate,convrate from currrates where currcode='"+ ddlcur.options[ddlIcname.selectedIndex].text +"' and Tocurr='"+ txtbase.value +"'"
            currsqlstr = "select convrate  from currrates,acctmast  where acctmast.div_code='" + txtdivcode.value + "' and  currrates.currcode=acctmast.currcode and Tocurr='" + txtbase.value + "' and acctmast.acctcode='" + codeid + "' "
            ColServices.clsServices.GetQueryReturnStringValuenew(constr, currsqlstr, FillCvntRate, ErrorHandler, TimeOutHandler);




            sqlstr = "sp_get_account_balance  '" + txtdivcode.value + "','G','" + codeid + "'";

            ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillBalance, ErrorHandler, TimeOutHandler);

            txtconvrate = document.getElementById("<%=txtConvRate.ClientId%>");
            if (trim(ddlcur.options[ddlcur.selectedIndex].text) == trim(txtbase.value)) {
                txtconvrate.readOnly = true;
                txtconvrate.disabled = true;
            }
            else {
                txtconvrate.readOnly = false;
                txtconvrate.disabled = false;
            }


        }

        function FillBalance(result) {
            txtTranTypes = document.getElementById("<%=txtTranType.ClientID%>");
            txtbalan = document.getElementById("<%=txtBalance.ClientId%>");
            txtmodes = document.getElementById("<%=txtMode.ClientId%>");
            txtOldAmounts = document.getElementById("<%=txtOldAmount.ClientID%>");
            var balance = DecRound(result);


            if (txtmodes.value == 'Edit') {
                if (trim(txtTranTypes.value) == 'RV') {
                    balance = DecRound(DecRound(result) - DecRound(txtOldAmounts.value));
                    //txtbalan.value=DecFormat(balance);
                }
                else //if (trim(txtTranTypes.value)=='PV' )
                {
                    balance = DecRound(DecRound(result) + DecRound(txtOldAmounts.value));
                    //txtbalan.value=DecFormat( balance);
                }
            }
            //txtbalan.value=DecFormat( txtbalan.value);
            lblcrdr = document.getElementById("<%=lblBalCrDr.ClientId%>");
            if (DecRound(balance) > 0) {
                lblcrdr.innerHTML = "Cr";
            }
            else {
                lblcrdr.innerHTML = "Dr";
            }
            //txtbalan.value=Math.abs(balance);
            //alert(String(Math.abs(balance)));
            txtbalan.value = DecFormat(String(Math.abs(balance)));
        }
        //----------------------------------------
        function FillName(ddlIccd, ddlIcnm) {
            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            var codeid = ddlIcname.options[ddlIcname.selectedIndex].value;
            ddlIccode.value = ddlIcname.options[ddlIcname.selectedIndex].text;

            var txtacccode = document.getElementById('<%=txtAccCode.ClientID%>');
            var txtaccname = document.getElementById('<%=txtAccName.ClientID%>');
            var txtcurrcode = document.getElementById('<%=txtCurrCode.ClientID%>');
            txtacccode.value = ddlIcname.options[ddlIcname.selectedIndex].text;
            txtaccname.value = ddlIcname.options[ddlIcname.selectedIndex].value;
            txtcurrcode.value = ddlIcname.options[ddlIcname.selectedIndex].value;



            var ddlcur = document.getElementById('<%=ddlCurrCode.ClientID%>');
            ddlcur.value = ddlIcname.value;

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>");
            var currsqlstr
            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');
            //sqlstr="select convrate,convrate from currrates where currcode='"+ ddlcur.options[ddlIcname.selectedIndex].text +"' and Tocurr='"+ txtbase.value +"'"
            currsqlstr = "select convrate  from currrates,acctmast  where acctmast.div_code='" + txtdiv.value + "' and currrates.currcode=acctmast.currcode and Tocurr='" + txtbase.value + "' and acctmast.acctcode='" + codeid + "' "
            ColServices.clsServices.GetQueryReturnStringValuenew(constr, currsqlstr, FillCvntRate, ErrorHandler, TimeOutHandler);
            txtconvrate = document.getElementById("<%=txtConvRate.ClientId%>");
            if (trim(ddlcur.options[ddlcur.selectedIndex].text) == trim(txtbase.value)) {
                txtconvrate.readOnly = true;
                txtconvrate.disabled = true;
            }
            else {
                txtconvrate.readOnly = false;
                txtconvrate.disabled = false;
            }
        }
        //----------------------------------------

        //----------------------------------------
        function FillGACode(ddlIccd, ddlIcnm, txtcurr, txtrate, ddlIContAcc, ddlConAccnm, ddltp, txtaccd, txtacnm, txtcramt, txtcrbaseamt, txtdbamt, txtdbbaseamt, txtcontcd, txtcontnm, txtboxauto) {

            txtgrdDebAmt = document.getElementById(txtdbamt);
            txtgrdDbBaseAmt = document.getElementById(txtdbbaseamt);
            txtgrdCrAmt = document.getElementById(txtcramt);
            txtgrdCrBaseAmt = document.getElementById(txtcrbaseamt);
            txtauto = document.getElementById(txtboxauto);

            var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");


            ddltyp = document.getElementById(ddltp);
            var strtp = ddltyp.value;


            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);
            ddlAConAcc = document.getElementById(ddlIContAcc);
            ddlAConAccNm = document.getElementById(ddlConAccnm);
            txtAConAcc = document.getElementById(txtcontcd);
            txtAConAccNm = document.getElementById(txtcontnm);

            var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;
            ddlIcname.value = codeid;

            txtaccd = document.getElementById(txtaccd);
            txtacnm = document.getElementById(txtacnm);
            txtaccd.value = ddlIccode.options[ddlIccode.selectedIndex].value;
            txtacnm.value = codeid;


            txtfill = document.getElementById(txtcurr);
            txtgrdcrate = document.getElementById(txtrate);

            //    sqlstr="select cur,cur from view_account where Code='" + ddlIcname.value + "'" ;
            //    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurr,ErrorHandler,TimeOutHandler);
            //   
            //   
            //    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
            //    sqlstr="select convrate from currrates ,view_account  where  currrates.currcode=view_account.cur and   view_account.code='"+ ddlIcname.value +"' and Tocurr='"+ txtbase.value +"'"
            //    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillGrdCvntRate,ErrorHandler,TimeOutHandler);


            var sqlstr1, sqlstr2
            ddlAConAcc.disabled = false;
            ddlAConAccNm.disabled = false;
            if (strtp == 'C') {
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and  view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  acctmast.acctname";
            }
            else if (strtp == 'S') {
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by acctmast.acctname"

            }
            else if (strtp == 'A') {
                sqlstr1 = " select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" + txtdivauto.value + "'   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" + txtdivauto.value + "'  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' order by acctmast.acctname"
            }
            else if (strtp == 'G') {
                //     sqlstr1=" select '' as controlacctcode, '' as acctname  "  
                //     sqlstr2=" select '' as acctname , '' as controlacctcode "
                ddlAConAcc.value = '[Select]';
                ddlAConAccNm.value = '[Select]';
                ddlAConAcc.disabled = true;
                ddlAConAccNm.disabled = true;
            }

            if (strtp != '[Select]') {
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                if (strtp != 'G') {
                    ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillControlAcc, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillControlAccName, ErrorHandler, TimeOutHandler);
                }
                FillCustDetails(strtp, codeid);
            }
            else {
                alert('Please Select Account Type');
                ddlIccode.value = "[Select]";
                ddlIcname.value = "[Select]";
                txtauto.value = "";
            }
        }
        //----------------------------------------
        function FillGAName(ddlIccd, ddlIcnm, txtcurr, txtrate, ddlIContAcc, ddlConAccnm, ddltp, txtaccd, txtacnm, txtcramt, txtcrbaseamt, txtdbamt, txtdbbaseamt, txtcontcd, txtcontnm, txtboxauto) {

            txtgrdDebAmt = document.getElementById(txtdbamt);
            txtgrdDbBaseAmt = document.getElementById(txtdbbaseamt);
            txtgrdCrAmt = document.getElementById(txtcramt);
            txtgrdCrBaseAmt = document.getElementById(txtcrbaseamt);
            txtauto = document.getElementById(txtboxauto);

            var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");

            ddltyp = document.getElementById(ddltp);
            var strtp = ddltyp.value;

            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            ddlAConAcc = document.getElementById(ddlIContAcc);
            ddlAConAccNm = document.getElementById(ddlConAccnm);
            txtAConAcc = document.getElementById(txtcontcd);
            txtAConAccNm = document.getElementById(txtcontnm);

            var codeid = ddlIcname.options[ddlIcname.selectedIndex].value;
            ddlIccode.value = ddlIcname.options[ddlIcname.selectedIndex].text;

            txtaccd = document.getElementById(txtaccd);
            txtacnm = document.getElementById(txtacnm);
            txtaccd.value = ddlIcname.options[ddlIcname.selectedIndex].text;
            txtacnm.value = codeid;


            txtfill = document.getElementById(txtcurr);
            txtgrdcrate = document.getElementById(txtrate);

            //    sqlstr="select cur,cur from view_account where Code='" + ddlIcname.value + "'" ;
            //    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr,FillCurr,ErrorHandler,TimeOutHandler);
            //    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
            //    sqlstr="select convrate from currrates ,view_account  where  currrates.currcode=view_account.cur and   view_account.code='"+ ddlIcname.value +"' and Tocurr='"+ txtbase.value +"'"
            //    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillGrdCvntRate,ErrorHandler,TimeOutHandler);

            var sqlstr1, sqlstr2
            ddlAConAcc.disabled = false;
            ddlAConAccNm.disabled = false;
            if (strtp == 'C') {
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and  view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and  view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  acctmast.acctname";
            }
            else if (strtp == 'S') {
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  and partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  and partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by acctmast.acctname"

            }
            else if (strtp == 'A') {
                sqlstr1 = " select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" + txtdivauto.value + "'  and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where  acctmast.div_code='" + txtdivauto.value + "'  and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' order by acctmast.acctname"
            }
            else if (strtp == 'G') {
                ddlAConAcc.value = '[Select]';
                ddlAConAccNm.value = '[Select]';
                ddlAConAcc.disabled = true;
                ddlAConAccNm.disabled = true;
            }
            if (strtp != '[Select]') {
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                if (strtp != 'G') {
                    ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillControlAcc, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillControlAccName, ErrorHandler, TimeOutHandler);
                }
                FillCustDetails(strtp, codeid);
            }
            else {
                alert('Please Select Account Type');
                ddlIccode.value = "[Select]";
                ddlIcname.value = "[Select]";
                txtauto.value = "";
            }
        }

        function FillCustDetails(typ, codeid) {
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>");


            var crdsqlstr = "select cur,convrate,controlacctcode    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where view_account.div_code='" + txtdiv.value + "' and code = '" + codeid + "' and type='" + typ + "' ";
            ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 3, FiilCustDt, ErrorHandler, TimeOutHandler);

        }
        function FiilCustDt(result) {
            txtfill.value = result[0];
            txtgrdcrate.value = result[1];
            if (result[2] != '') {
                ddlAConAccNm.value = result[2];

                var codeid = ddlAConAccNm.options[ddlAConAccNm.selectedIndex].text;
            }
            else {
                ddlAConAccNm.value = "[Select]";
                var codeid = ddlAConAccNm.options[ddlAConAccNm.selectedIndex].text;
            }
            ddlAConAcc.value = codeid;
            txtAConAcc.value = codeid;
            txtAConAccNm.value = result[2];

            GrdExchangeRateChange(result[1]);
            var txtbase = document.getElementById('ctl00_Main_txtbasecurr');

            if (trim(txtfill.value) == trim(txtbase.value)) {
                txtgrdcrate.readOnly = true;
                txtgrdcrate.disabled = true;
            }
            else {
                txtgrdcrate.readOnly = false;
                txtgrdcrate.disabled = false;
            }

            //ValidateMarketCode(codeid);
        }

        function FillCTCode(ddlIccd, ddlIcnm, txtcd, txtnm) {
            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;
            ddlIcname.value = codeid;

            txtctd = document.getElementById(txtcd);
            txtctnm = document.getElementById(txtnm);
            txtctd.value = ddlIccode.options[ddlIccode.selectedIndex].value;
            txtctnm.value = codeid;

        }

        function FillCTName(ddlIccd, ddlIcnm, txtcd, txtnm) {

            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            var codeid = ddlIcname.options[ddlIcname.selectedIndex].text;
            ddlIccode.value = codeid;

            txtctd = document.getElementById(txtcd);
            txtctnm = document.getElementById(txtnm);
            txtctd.value = codeid;
            txtctnm.value = ddlIcname.options[ddlIcname.selectedIndex].value;

        }


        //        function ValidateMarketCode(ddlAcc) {
        //            var crdsqlstr = "SELECT COUNT(acctcode) FROM acctgroup Where childid IN (115,120) and acctcode='" + ddlAcc + "'";
        //            ColServices.clsServices.GetAcctCodeCount(constr, crdsqlstr, ValidAlertMsg, ErrorHandler, TimeOutHandler);
        //        }

        //        function ValidAlertMsg(result) {
        //            var count = result;
        //        }

        //----------------------------------------
        function FillCurr(result) {

            txtfill.value = result[0].ListText;

            //    sqlstr="select convrate,convrate from currrates where currcode='"+ txtfill.value +"' and Tocurr='"+ txtbase.value +"'"

            //    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCvntRate,ErrorHandler,TimeOutHandler);

        }
        //-----------------------------------------
        function FillCvntRate(result) {
            txtt = document.getElementById('<%=txtConvRate.ClientID%>');
            txtt.value = DecRound(result);
            ExchangeRateChange(result);
        }
        function FillGrdCvntRate(result) {
            txtgrdcrate.value = DecRound(result);
            GrdExchangeRateChange(result)
            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');


            if (trim(txtfill.value) == trim(txtbase.value)) {

                txtgrdcrate.readOnly = true;
            }
            else {

                txtgrdcrate.readOnly = false;
            }

        }


        function FillCombotoText(ddlc, txtt) {
            ddlcs = document.getElementById(ddlc);
            txtts = document.getElementById(txtt);

            var codeid = ddlcs.options[ddlcs.selectedIndex].text;
            txtts.value = codeid;
        }


        function fill_acountcode(ddltp, ddlcode, ddlname, ddlConAccd, ddlConAccnm, txtgnarr, ddlcostcd, ddlcostnm, txtboxauto, txtdiv) {

            ddltyp = document.getElementById(ddltp);
            var strtp = ddltyp.value;
            ddlACode = document.getElementById(ddlcode);
            ddlAName = document.getElementById(ddlname);

            ddlAConAcc = document.getElementById(ddlConAccd);
            ddlAConAccNm = document.getElementById(ddlConAccnm);

            ddlcostcode = document.getElementById(ddlcostcd);
            ddlcostname = document.getElementById(ddlcostnm);
            var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");

            var sqlstr1 = null;
            var sqlstr2 = null;
            var sqlstr3 = null;
            var sqlstr4 = null;

            if (strtp != '[Select]') {
                sqlstr1 = "select Code,des from view_account where div_code='" + txtdivauto.value + "' and type = '" + strtp + "' order by code";
                sqlstr2 = "select des,Code from view_account where div_code='" + txtdivauto.value + "' and type = '" + strtp + "' order by des";
                sqlstr3 = "select isnull(controlacctcode,0),code from view_account where div_code='" + txtdivauto.value + "' and type= '" + strtp + "' order by code";
                sqlstr4 = " select distinct acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and view_account.div_code='" + txtdivauto.value + "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' order by  acctmast.acctname";
            }
            else {
                sqlstr1 = "select top 10 Code,des from view_account where div_code='" + txtdivauto.value + "'    order by code";
                sqlstr2 = "select top 10  des,Code from view_account where div_code='" + txtdivauto.value + "'  order by des";
                sqlstr3 = "select top 10  isnull(controlacctcode,0),code from view_account where div_code='" + txtdivauto.value + "'   order by code";
                sqlstr4 = " select distinct top 10   acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where view_account.div_code=acctmast.div_code and view_account.div_code='" + txtdivauto.value + "' and    view_account.controlacctcode= acctmast.acctcode  order by  acctmast.acctname";
            }



            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value


            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillACodes, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillANames, ErrorHandler, TimeOutHandler);

            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr3, FillControlAcc, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr4, FillControlAccName, ErrorHandler, TimeOutHandler);



            txtgnarrs = document.getElementById(txtgnarr);
            txtnarr = document.getElementById("<%=txtnarration.ClientID%>");
            txtgnarrs.value = trim(txtnarr.value);

            ddlcostcode.disabled = false;
            ddlcostname.disabled = false;
            if (strtp == 'G') {
                ddlcostcode.disabled = false;
                ddlcostname.disabled = false;
            }
            else {
                ddlcostcode.disabled = true;
                ddlcostname.disabled = true;
            }


            OnChangeType(txtboxauto, ddlname, ddltp);

        }

        function FillACodes(result) {
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

        function FillControlAcc(result) {
            RemoveAll(ddlAConAcc)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlAConAcc.options.add(option);
            }
            ddlAConAcc.value = "[Select]";


            if (sddltyp != '[Select]' && sddlACode != null) {
                //    alert(sddltyp + ' ' + sddlACode);
                //    FillCustDetails(sddltyp, sddlACode);
            }
        }
        function FillControlAccName(result) {
            RemoveAll(ddlAConAccNm)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlAConAccNm.options.add(option);
            }
            ddlAConAccNm.value = "[Select]";


            if (sddltyp != '[Select]' && sddlACode != null) {
                //  alert(sddltyp +' ' + sddlACode);
                //   FillCustDetails(sddltyp, sddlACode);
            }
        }
        //---------------------------------------------------

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
        //---------------------------------------------------
        function ExchangeRateChange(result) {
            txtDBAmt = document.getElementById("<%=txtAmount.ClientID%>");
            txtCnvtRate = result;
            txtCnvtAmt = document.getElementById("<%=txtCnvAmount.ClientID%>");

            if (txtDBAmt.value == '') { txtDBAmt.value = 0; }
            var cAmt = DecRound(txtDBAmt.value) * txtCnvtRate;
            txtCnvtAmt.value = DecRound(cAmt);

        }
        // function  ExchangeRateChange(txtDBAmt,txtCnvtRate,txtCnvtAmt)
        // {
        //    var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
        //    nodecround=Math.pow(10,parseInt(txtdec.value));


        //    txtDBAmt = document.getElementById(txtDBAmt);
        //    txtCnvtRate = document.getElementById(txtCnvtRate);
        //    txtCnvtAmt = document.getElementById(txtCnvtAmt);  

        //    if (txtDBAmt.value==''){txtDBAmt.value=0;}
        //    var cAmt=parseFloat(txtDBAmt.value) *  parseFloat(txtCnvtRate.value) ;
        //    txtCnvtAmt.value =Math.round(cAmt*nodecround)/nodecround;
        //    //cAmt.toFixed(3);
        //     
        //    //Call Grd Total 
        //  
        // }
        //---------------------------------------------------
        function GrdExchangeRateChange(result) {
            txtCnvtRate = result;

            if (trim(txtgrdDebAmt.value) == '') { txtgrdDebAmt.value = 0; }
            if (isNaN(txtgrdDebAmt.value) == true) { txtgrdDebAmt.value = 0; }
            if (trim(txtgrdCrAmt.value) == '') { txtgrdCrAmt.value = 0; }
            if (isNaN(txtgrdCrAmt.value) == true) { txtgrdCrAmt.value = 0; }

            var amt = DecRound(txtgrdDebAmt.value) * parseFloat(txtCnvtRate);
            txtgrdDbBaseAmt.value = DecRound(amt);


            var amt1 = DecRound(txtgrdCrAmt.value) * parseFloat(txtCnvtRate);
            txtgrdCrBaseAmt.value = DecRound(amt1);

            // grdTotal()
        }

        function convertInRate(txtdbamt, txtdebbaseamt, txtcramt, txtcrbaseamt, txtcnvRate, pstr) {

            var txtgrdDebAmt1 = document.getElementById(txtdbamt);
            var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
            var txtgrdCrAmt1 = document.getElementById(txtcramt);
            var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
            var txtCnvtRate1 = document.getElementById(txtcnvRate);

            if (trim(txtgrdDebAmt1.value) == '') { txtgrdDebAmt1.value = 0; }
            if (isNaN(txtgrdDebAmt1.value) == true) { txtgrdDebAmt1.value = 0; }
            if (trim(txtgrdCrAmt1.value) == '') { txtgrdCrAmt1.value = 0; }
            if (isNaN(txtgrdCrAmt1.value) == true) { txtgrdCrAmt1.value = 0; }


            if (pstr == 'Debit') {
                if (Number(txtgrdCrAmt1.value) > 0) {

                    txtgrdDebAmt1.value = 0;
                    //  txtgrdDebAmt1.readOnly=true;
                }
                else if (Number(txtgrdCrAmt1.value) == 0 || txtgrdCrAmt1.value == '') {

                    // txtgrdCrAmt1.readOnly=true;
                }
            }
            else if (pstr == 'Credit') {

                if (Number(txtgrdDebAmt1.value) > 0) {
                    txtgrdCrAmt1.value = 0;
                    // txtgrdCrAmt1.readOnly=true;
                }
                else if (Number(txtgrdDebAmt1.value) == 0 || txtgrdDebAmt1.value == '') {
                    //txtgrdDebAmt1.readOnly=true;
                }
            }


            var amt = DecRound(txtgrdDebAmt1.value) * parseFloat(txtCnvtRate1.value);

            txtgrdDbBaseAmt1.value = DecRound(amt);

            var amt1 = DecRound(txtgrdCrAmt1.value) * parseFloat(txtCnvtRate1.value);
            txtgrdCrBaseAmt1.value = DecRound(amt1);
            grdTotal()
        }

        function convertInRateBase(txtdbamt, txtdebbaseamt, txtcramt, txtcrbaseamt, txtcnvRate, pstr) {

            var txtgrdDebAmt1 = document.getElementById(txtdbamt);
            var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
            var txtgrdCrAmt1 = document.getElementById(txtcramt);
            var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
            var txtCnvtRate1 = document.getElementById(txtcnvRate);

            if (trim(txtgrdDebAmt1.value) == '') { txtgrdDebAmt1.value = 0; }
            if (isNaN(txtgrdDebAmt1.value) == true) { txtgrdDebAmt1.value = 0; }
            if (trim(txtgrdCrAmt1.value) == '') { txtgrdCrAmt1.value = 0; }
            if (isNaN(txtgrdCrAmt1.value) == true) { txtgrdCrAmt1.value = 0; }
            if (trim(txtgrdDbBaseAmt1.value) == '') { txtgrdDbBaseAmt1.value = 0; }
            if (isNaN(txtgrdDbBaseAmt1.value) == true) { txtgrdDbBaseAmt1.value = 0; }
            if (trim(txtgrdCrBaseAmt1.value) == '') { txtgrdCrBaseAmt1.value = 0; }
            if (isNaN(txtgrdCrBaseAmt1.value) == true) { txtgrdCrBaseAmt1.value = 0; }

            if (pstr == 'Debit') {
                if (Number(txtgrdCrBaseAmt1.value) > 0) {

                    txtgrdDbBaseAmt1.value = 0;
                    //  txtgrdDebAmt1.readOnly=true;
                }
                //         else if (Number(txtgrdCrBaseAmt1.value) == 0 || txtgrdCrBaseAmt1.value == '') {

                //             // txtgrdCrAmt1.readOnly=true;
                //         }
            }
            else if (pstr == 'Credit') {

                if (Number(txtgrdDbBaseAmt1.value) > 0) {
                    txtgrdCrBaseAmt1.value = 0;
                    // txtgrdCrAmt1.readOnly=true;
                }
                //         else if (Number(txtgrdDebAmt1.value) == 0 || txtgrdDebAmt1.value == '') {
                //             //txtgrdDebAmt1.readOnly=true;
                //         }
            }


            var amt = DecRound(txtgrdDbBaseAmt1.value) / parseFloat(txtCnvtRate1.value);

            txtgrdDebAmt1.value = DecRound(amt);

            var amt1 = DecRound(txtgrdCrBaseAmt1.value) / parseFloat(txtCnvtRate1.value);
            txtgrdCrAmt1.value = DecRound(amt1);
            grdTotal()
        }


        function convertInRateAmount(txtDBAmt, txtCnvtRate, txtCnvtAmt) {

            txtDBAmt = document.getElementById(txtDBAmt);
            txtCnvtRate = document.getElementById(txtCnvtRate);
            txtCnvtAmt = document.getElementById(txtCnvtAmt);

            if (trim(txtDBAmt.value) == '') { txtDBAmt.value = 0; }
            var amt = DecRound(txtDBAmt.value) * txtCnvtRate.value;
            txtCnvtAmt.value = DecRound(amt);
            //Call Grd Total 
            grdTotal()
        }


        function convertInRateBaseAmount(txtDBAmt, txtCnvtRate, txtCnvtAmt) {

            txtDBAmt = document.getElementById(txtDBAmt);
            txtCnvtRate = document.getElementById(txtCnvtRate);
            txtCnvtAmt = document.getElementById(txtCnvtAmt);

            if (trim(txtDBAmt.value) == '') { txtDBAmt.value = 0; }
            var amt = DecRound(txtDBAmt.value) / txtCnvtRate.value;
            txtCnvtAmt.value = DecRound(amt);
            //Call Grd Total 
            grdTotal()
        }

        //---------------------------
        function DecRoundEightPalces(amtToRound) {

            //var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
            nodecround = Math.pow(10, parseInt(8));
            var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
            return parseFloat(rdamt);
        }

        function convertRateOnBaseCurrency(typ, txtdbamt, txtdebbaseamt, txtcramt, txtcrbaseamt, txtcnvRate, pstr) {
            var acctyp = document.getElementById(typ);
            var txtgrdDebAmt1 = document.getElementById(txtdbamt);
            var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
            var txtgrdCrAmt1 = document.getElementById(txtcramt);
            var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
            var txtCnvtRate1 = document.getElementById(txtcnvRate);

            if (trim(txtgrdDebAmt1.value) == '') { txtgrdDebAmt1.value = 0; }
            if (isNaN(txtgrdDebAmt1.value) == true) { txtgrdDebAmt1.value = 0; }
            if (trim(txtgrdCrAmt1.value) == '') { txtgrdCrAmt1.value = 0; }
            if (isNaN(txtgrdCrAmt1.value) == true) { txtgrdCrAmt1.value = 0; }


            if (pstr == 'Debit') {
                if (Number(txtgrdCrBaseAmt1.value) > 0) {

                    txtgrdDbBaseAmt1.value = 0;
                }
                else if (Number(txtgrdDebAmt1.value) == 0 || txtgrdDebAmt1.value == '') {
                    txtgrdDebAmt1.value = 0;
                    //txtgrdDebAmt1.readOnly=true;
                }

            }
            else if (pstr == 'Credit') {
                if (Number(txtgrdDbBaseAmt1.value) > 0) {
                    txtgrdCrBaseAmt1.value = 0;

                }
                else if (Number(txtgrdCrAmt1.value) == 0 || txtgrdCrAmt1.value == '') {
                    txtgrdCrBaseAmt1.value = 0;
                    // txtgrdCrAmt1.readOnly=true;
                }
            }

            if (acctyp.value != 'G') {
                if (pstr == 'Debit') {
                    if (txtgrdDebAmt1.value == '' || txtgrdDebAmt1.value == 0) {
                        var amt = DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdDebAmt1.value = DecRoundEightPalces(amt);
                    }
                    else {
                        var amt = DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtgrdDebAmt1.value);
                        txtCnvtRate1.value = DecRoundEightPalces(amt);
                    }
                }
                else if (pstr == 'Credit') {
                    if (txtgrdCrAmt1.value == '' || txtgrdCrAmt1.value == 0) {
                        var amt = DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdCrAmt1.value = DecRoundEightPalces(amt);
                    }
                    else {
                        var amt = DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtgrdCrAmt1.value);
                        txtCnvtRate1.value = DecRoundEightPalces(amt);
                    }
                }
            }
            else {
                if (pstr == 'Debit') {
                    if (parseFloat(txtgrdDbBaseAmt1.value) > 0) {
                        var amt = DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdDebAmt1.value = DecRoundEightPalces(amt);
                    }
                }
                else if (pstr == 'Credit') {
                    if (parseFloat(txtgrdCrBaseAmt1.value) > 0) {
                        var amt = DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdCrAmt1.value = DecRoundEightPalces(amt);
                    }
                }
            }

            grdTotal()
        }

        //---------------------------------------------------

        function grdTotal() {
            var totCr = 0;
            var totDr = 0;
            var totbCr = 0;
            var totbDr = 0;

            var objGridView = document.getElementById('<%=grdReceipt.ClientID%>');
            var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');
            intRows = txtrowcnt.value;

            for (j = 1; j <= intRows; j++) {

                var valDr = objGridView.rows[j].cells[8].children[0].value;
                var valCr = objGridView.rows[j].cells[9].children[0].value;

                var valbDr = objGridView.rows[j].cells[10].children[0].value;
                var valbCr = objGridView.rows[j].cells[11].children[0].value;

                if (valCr == '') { valCr = 0; }
                if (valDr == '') { valDr = 0; }
                if (valbCr == '') { valbCr = 0; }
                if (valbDr == '') { valbDr = 0; }

                if (isNaN(valCr) == true) { valCr = 0; }
                if (isNaN(valDr) == true) { valDr = 0; }

                if (isNaN(valbCr) == true) { valbCr = 0; }
                if (isNaN(valbDr) == true) { valbDr = 0; }


                totCr = DecRound(totCr) + DecRound(valCr);
                totDr = DecRound(totDr) + DecRound(valDr);

                totbCr = DecRound(totbCr) + DecRound(valbCr);
                totbDr = DecRound(totbDr) + DecRound(valbDr);


            }

            var txttotCr = document.getElementById('<%=txtTotalCredit.ClientID%>');
            var txttotDr = document.getElementById('<%=txtTotalDebit.ClientID%>');

            var txttotbCr = document.getElementById('<%=txtTotBaseCredit.ClientID%>');
            var txttotbDr = document.getElementById('<%=txtTotBaseDebit.ClientID%>');

            var txttotbDiff = document.getElementById('<%=txtTotBaseDiff.ClientID%>');

            txttotDr.value = DecRound(totDr);
            txttotCr.value = DecRound(totCr);

            txttotbDr.value = DecRound(totbDr);
            txttotbCr.value = DecRound(totbCr);

            txttotbDiff.value = DecRound(totbCr - totbDr);
            txttotbDiff.value = Math.abs(txttotbDiff.value);
        }
        //---------------------------------------------------

        var ACode = null;
        var conCode = null;
        var typ = null;
        var tranId = null;
        var lno = null;
        var currCode = null;
        var CurRate = null;
        var creditamt = null;
        var baseamt = null;
        var oldlno = null;
        var reqsid = null;

        function openAdjustBill(ddlACode, ddlconCode, ddltyp, txtTranId, txtlno, txtcurrCode, txtCurRate, txtcreditamt, txtcreditbaseamt, txtoldlno, txtdebitamt, txtdebitbaseamt, reqid) {
            var ddlACode = document.getElementById(ddlACode);
            var ddlContCode = document.getElementById(ddlconCode);
            ddltyp = document.getElementById(ddltyp);
            txtTranId = document.getElementById(txtTranId);
            var txtLineNo = document.getElementById(txtlno);
            var txtOLineNo = document.getElementById(txtoldlno);
            txtCurrsCode = document.getElementById(txtcurrCode);
            txtCurRate = document.getElementById(txtCurRate);
            //            txtBaseAmt = document.getElementById(txtbaseamt);
            //            txtCreditAmt = document.getElementById(txtcreditamt);
            txtrequestid = document.getElementById(reqid);

            ACode = ddlACode.value;
            conCode = ddlContCode.value;
            typ = ddltyp.value;
            tranId = txtTranId.value;
            lno = txtLineNo.value;
            oldlno = txtOLineNo.value;
            currCode = txtCurrsCode.value;
            CurRate = txtCurRate.value;
            //            creditamt = txtCreditAmt.value;
            //            baseamt = txtBaseAmt.value;
            if (txtrequestid != null) {
                if (txtrequestid != undefined) {
                    reqsid = txtrequestid.value;
                    if (reqsid == 0) { reqsid = ''; }
                } else {
                    reqsid = '';
                }
            } else {
                reqsid = '';
            }
            var sqlstr = null;

            txttrantype = document.getElementById("<%=txtTranType.ClientID%>");
            txtgrdtype = document.getElementById("<%=txtGridType.ClientID%>");

            var txtCrAmt = document.getElementById(txtcreditamt);
            var txtCrBaseAmt = document.getElementById(txtcreditbaseamt);
            var txtDRAmt = document.getElementById(txtdebitamt);
            var txtDRBaseAmt = document.getElementById(txtdebitbaseamt);

            var passBaseAmt = 0;
            var passAmt = 0;

            var valCr = DecRound(txtCrAmt.value);
            var valDr = DecRound(txtDRAmt.value);
            var valCrBase = DecRound(txtCrBaseAmt.value);
            var valDrBase = DecRound(txtDRBaseAmt.value);


            if (valCr == '' || valCr == 0) {
                passAmt = valDr;
                passBaseAmt = valDrBase;
                txtgrdtype.value = 'Debit';
            }
            if (valDr == '' || valDr == 0) {
                passAmt = valCr;
                passBaseAmt = valCrBase;
                txtgrdtype.value = 'Credit';
            }
            if (passAmt == 0) {
                alert("Please enter debit or credit amount.")
                return false;
            }



            //            if (txttrantype.value == 'RV') {
            //                txtgrdtype.value = 'Credit';
            //            }
            //            else //if (txttrantype.value=='PV' )
            //            {
            //                txtgrdtype.value = 'Debit';
            //            }

            var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');
            intRows = txtrowcnt.value;
            txtstate = document.getElementById("<%=txtMode.ClientID%>");
            txtrefcode = document.getElementById("<%=txtDocNo.ClientID%>");
            txtAdjcolcode = document.getElementById("<%=txtAdjcolno.ClientID%>");
            txtDate = document.getElementById("<%=txtDate.ClientID%>");

            var txtdiv = document.getElementById('<%=txtDivcode.ClientID%>');

            var pass;

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            if (reqsid != '') {
                if (ddltyp.value != 'G') {
                    if (ddlContCode.value == '[Select]') {
                        alert('Select Control Account..');
                        return false;
                    }

                    sqlstr = "select distinct acc_type,acc_code from reservation_invoice_detail where acc_type='" + typ + "' and requestid='" + reqsid + "' and acc_code='" + ddlACode.value + "'"
                    ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr, 2, validate_supp, ErrorHandler, TimeOutHandler);
                }
                else {
                    alert('Account type ‘G’ doesn’t  adjust bill .');
                    return false;
                }
            }
            else {

                if (ddltyp.value != 'G') {
                    if (ddlContCode.value == '[Select]') {
                        alert('Select Control Account..');
                        return false;
                    }


                    if (txtrequestid.value == 0) { txtrequestid.value = ''; }

                    pass = "TranType=" + txttrantype.value + "&AccCode=" + ddlACode.value + "&ControlCode=" + ddlContCode.value + "&AccType=" + ddltyp.value + "&TranId=" + txtTranId.value + "&lineNo=" + txtLineNo.value + "&divid=" + txtdiv.value + "&currcode=" + txtCurrsCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + passAmt + "&BaseAmount=" + passBaseAmt + "&Gridtype=" + txtgrdtype.value + "&MainGrdCount=" + intRows + "&OlineNo=" + txtOLineNo.value + "&State=" + txtstate.value + "&RefCode=" + txtrefcode.value + "&AdjColno=" + txtAdjcolcode.value + "&trandate=" + txtDate.value + "&Requestid=" + txtrequestid.value;

                    window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,left=0,top=0,height=' + screen.height + ',width=' + screen.width);
                    return false;

                } else {
                    alert('Account type ‘G’ doesn’t  adjust bill .');
                    return false;
                }
            }
        }

        function validate_supp(result) {
            txttrantype = document.getElementById("<%=txtTranType.ClientID%>");
            txtgrdtype = document.getElementById("<%=txtGridType.ClientID%>");
            txtDate = document.getElementById("<%=txtDate.ClientID%>");

            if (txttrantype.value == 'RV') {
                txtgrdtype.value = 'Credit';
            }
            else //if (txttrantype.value=='PV' )
            {
                txtgrdtype.value = 'Debit';
            }

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById('<%=txtDivcode.ClientID%>');

            var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');
            intRows = txtrowcnt.value;
            txtstate = document.getElementById("<%=txtMode.ClientID%>");
            txtrefcode = document.getElementById("<%=txtDocNo.ClientID%>");
            txtAdjcolcode = document.getElementById("<%=txtAdjcolno.ClientID%>");
            var pass;
            if (typ != 'G') {

                if (result[0] != typ || result[1] != ACode) {
                    alert('Booking no. does not matching for this account code');
                    return false;
                }

                //         pass = "TranType=" + txttrantype.value + "&AccCode=" + ddlACode.value + "&ControlCode=" + ddlContCode.value + "&AccType=" + ddltyp.value + "&TranId=" + txtTranId.value + "&lineNo=" + txtLineNo.value + "&currcode=" + txtCurrsCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + txtCreditAmt.value + "&BaseAmount=" + txtBaseAmt.value + "&Gridtype=" + txtgrdtype.value + "&MainGrdCount=" + intRows + "&OlineNo=" + txtOLineNo.value + "&State=" + txtstate.value + "&RefCode=" + txtrefcode.value + "&AdjColno=" + txtAdjcolcode.value + "&Requestid=" + txtrequestid.value;
                pass = "TranType=" + txttrantype.value + "&AccCode=" + ACode + "&ControlCode=" + conCode + "&AccType=" + typ + "&TranId=" + tranId + "&lineNo=" + lno + "&divid=" + txtdiv.value + "&currcode=" + currCode + "&currrate=" + CurRate + "&Amount=" + creditamt + "&BaseAmount=" + baseamt + "&Gridtype=" + txtgrdtype.value + "&MainGrdCount=" + intRows + "&OlineNo=" + oldlno + "&State=" + txtstate.value + "&RefCode=" + txtrefcode.value + "&AdjColno=" + txtAdjcolcode.value + "&trandate=" + txtDate.value + "&Requestid=" + reqsid;

                //window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,fullscreen=yes');

                window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,left=0,top=0,height=' + screen.height + ',width=' + screen.width);

                return false;

            } else {
                alert('Account type ‘G’ doesn’t  adjust bill .');
                return false;
            }
        }

        function validate_click() {
            var txtCredit = document.getElementById("<%=txtTotalCredit.ClientID%>");
            var txtBaseDebit = document.getElementById("<%=txtTotalDebit.ClientID%>");
            var txtBaseCredit = document.getElementById("<%=txtTotBaseCredit.ClientID%>");
            var txtBaseDebit = document.getElementById("<%=txtTotBaseDebit.ClientID%>");

            var chkBlank = document.getElementById("<%=chkBlank.ClientID%>");
            var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");
            var btnss = document.getElementById("<%=btnsave.ClientID%>");

            hdnSS.value = 0;
            /*
            if ((txtBaseDebit.value == '') || (txtBaseDebit.value == '0')) {
            if (chkBlank.checked == false) {
            alert('Please select Allow blank');
            chkBlank.disabled = false;
            return false;
            }
            {
            return true;
            }
            }*/

            if (hdnSS.value == 0) {
                hdnSS.value = 1;
                btnss.style.visibility = "hidden";
                return true;
            }
            else {
                return false;
            }

            //return true;                

        }


        function GetValueFromMarket() {

            var ddl = document.getElementById("<%=ddlSMktName.ClientID%>");
            ddl.selectedIndex = -1;
            // Iterate through all dropdown items.
            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].text ==
			document.getElementById("<%=ddlSMktCode.ClientID%>").value) {
                    // Item was found, set the selected index.
                    ddl.selectedIndex = i;
                    return true;
                }
            }
        }
        function GetValueCodeMarket() {
            var ddl = document.getElementById("<%=ddlSMktCode.ClientID%>");
            ddl.selectedIndex = -1;
            // Iterate through all dropdown items.
            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].text ==
			document.getElementById("<%=ddlSMktName.ClientID%>").value) {
                    // Item was found, set the selected index.
                    ddl.selectedIndex = i;
                    return true;
                }
            }
        }


 
    </script>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table>
                    <tbody>
                        <tr>
                            <td class="field_heading" align="center">
                                <asp:Label ID="lblHeading" runat="server" Text="Recepits" Width="896px" CssClass="field_heading"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 920px; height: 165px" class="td_cell">
                                    <tbody>
                                        <tr>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label5" runat="server" Text="Doc. No." Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="height: 26px">
                                                <input id="txtDocNo" class="field_input" tabindex="4" readonly="readonly" type="text"
                                                    maxlength="50" runat="server" />
                                            </td>
                                            <td>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana"
                                                    Width="155px" CssClass="field_caption" ForeColor="Green" Font-Bold="True" BackColor="#E0E0E0"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label9" runat="server" Text=" Date" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="width: 181px; height: 26px">
                                                <asp:TextBox ID="txtDate" TabIndex="5" runat="server" Width="80px" CssClass="fiel_input"
                                                    ValidationGroup="MKE"></asp:TextBox><asp:ImageButton ID="ImgBtnFrmDt" TabIndex="3"
                                                        runat="server" ImageUrl="..\Images\Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator
                                                            ID="MskVFromDt" runat="server" Width="23px" CssClass="field_error" ValidationGroup="MKE"
                                                            ControlExtender="MskFromDate" ControlToValidate="txtDate" Display="Dynamic" EmptyValueBlurredText="*"
                                                            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                <div style ="padding-left:10px; ">
                                                <asp:Label ID="lbldate" runat="server" Text="Posting date" Font-Size="X-Small" ForeColor="Red" Width="70px" ></asp:Label>
                                                </div>

                                            </td>
                                            <td>
                                            </td>
                                            <td align="center">
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="Cash / Bank" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td>
                                                <select style="width: 104px" id="ddlCashBank" class="field_input" tabindex="1" runat="server">
                                                    <option value="[Select]" selected="selected">[Select]</option>
                                                    <option value="Cash">Cash</option>
                                                    <option value="Bank">Bank</option>
                                                </select>
                                            </td>
                                            <td>
                                            </td>
                                            <td align="center">
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 22px">
                                                <asp:Label ID="Label6" runat="server" Text="Bank" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="height: 22px">
                                                <select style="width: 126px" id="ddlAccCode" class="field_input" tabindex="2" runat="server">
                                                </select>
                                            </td>
                                            <td style="height: 22px">
                                                <asp:Label ID="Label8" runat="server" Text=" Name" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="width: 181px; height: 22px">
                                                <select style="width: 178px" id="ddlAccName" class="field_input" tabindex="3" runat="server">
                                                </select>
                                            </td>
                                            <td style="height: 26px">
                                                <asp:Label ID="lblChN" runat="server" Text="Cheque No" Width="80px" CssClass="field_caption"
                                                    Font-Strikeout="False"></asp:Label>
                                            </td>
                                            <td style="height: 26px">
                                                <input id="txtChequeNo" class="field_input" tabindex="6" type="text" maxlength="50"
                                                    runat="server" />
                                            </td>
                                            <td style="height: 26px">
                                                <asp:Label ID="lblChD" runat="server" Text="Cheque Date" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="height: 26px">
                                                <asp:TextBox ID="txtChequeDate" TabIndex="7" runat="server" Width="80px" CssClass="fiel_input"
                                                    ValidationGroup="MKE"></asp:TextBox>&nbsp;<asp:ImageButton ID="ImageButton1" TabIndex="3"
                                                        runat="server" ImageUrl="..\Images\Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;<cc1:MaskedEditValidator
                                                            ID="MaskedEditValidator1" runat="server" Width="23px" CssClass="field_error"
                                                            ValidationGroup="MKE" ControlExtender="MskChequeDate" ControlToValidate="txtChequeDate"
                                                            Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required"
                                                            ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                                            TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                <div style ="padding-left:10px;">
                                                <asp:Label ID="lbldate1" runat="server" Text="Posting date" Font-Size="X-Small" ForeColor="Red" Width="70px" ></asp:Label>
                                             </div>

                                            </td>
                                            <td style="height: 22px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 22px">
                                                <asp:Label ID="lblCurrCode" runat="server" Text="Curr. code" Width="80px" CssClass="field_caption"
                                                    Font-Strikeout="False"></asp:Label>
                                            </td>
                                            <td style="height: 22px">
                                                <select style="width: 100px" id="ddlCurrCode" class="field_input" disabled="disabled"
                                                    tabindex="0" runat="server">
                                                </select>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text=" Conv. Rate" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td>
                                                <input id="txtConvRate" class="field_input" tabindex="8" type="text" maxlength="50"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="Amount " Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="width: 181px">
                                                <input style="text-align: right" id="txtAmount" class="field_input" tabindex="9"
                                                    type="text" maxlength="50" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBaseAmt" runat="server" Text="Base Amount" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td>
                                                <input style="text-align: right" id="txtCnvAmount" class="field_input" tabindex="10"
                                                    type="text" maxlength="50" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" Style="display: none" runat="server" Text="M.RV" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <input id="txtMRV" style="display: none" class="field_input" tabindex="11" type="text"
                                                    maxlength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td style="width: 74px;  height: 22px; display:none;">
                                                <span style="color: black">Market Code</span>
                                            </td>
                                            <td style="width: 186px;  height: 22px; display:none;" >
                                                <select onchange="GetValueFromMarket()" style="width: 183px" id="ddlSMktCode" class="drpdown"
                                                    tabindex="5" runat="server" enableviewstate="true" visible="true">
                                                    <option selected="selected"></option>
                                                </select>
                                            </td>
                                            <td style="width: 100px;  height: 22px; display:none;">
                                                <span style="color: black">Market Name</span>
                                            </td>
                                            <td style="width: 211px;  height: 22px; display:none;">
                                                <select onchange="GetValueCodeMarket()" style="width: 275px; height: 20px" id="ddlSMktName"
                                                    class="drpdown" tabindex="6" runat="server" enableviewstate="true" visible="true">
                                                    <option selected="selected"></option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 24px">
                                                <asp:Label ID="lblRecfrom" runat="server" Text="Received From " Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td style="height: 24px;" colspan="5">
                                                <select style="width: 172px; display: none;" id="ddlRecveidfrom" class="field_input"
                                                    tabindex="12" runat="server">
                                                </select>
                                                <input style="width: 373px" id="txtReceived" class="field_input" tabindex="13" type="text"
                                                    maxlength="50" runat="server" />
                                            </td>
                                            <td style="height: 24px">
                                                <asp:Label ID="lblChB" runat="server" Text="Customer Bank" Width="50px"></asp:Label>
                                            </td>
                                            <td style="height: 24px">
                                                <select style="width: 136px" id="ddlCustBank" class="field_input" tabindex="14" runat="server">
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Narration" Width="80px" CssClass="field_caption"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtnarration" TabIndex="15" runat="server" Width="545px" CssClass="field_input"
                                                    MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                <select style="width: 556px; display: none" id="ddlNarration" class="field_input"
                                                    tabindex="15" runat="server">
                                                </select>&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text=" Balance" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <input style="text-align: right" id="txtBalance" class="field_input" disabled="disabled"
                                                    type="text" maxlength="50" runat="server" />&nbsp;<asp:Label ID="lblBalCrDr" runat="server"
                                                        Text="--" Width="18px" CssClass="field_caption"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                            </td>
                                            <td valign="top" colspan="6">
                                                <%--<asp:TextBox ID="txtnarration" TabIndex="16" runat="server" Width="545px" CssClass="field_input"
                                                    MaxLength="200" TextMode="MultiLine"></asp:TextBox>--%>
                                            </td>
                                            <td valign="top">
                                            </td>
                                            <td valign="top">
                                                <input style="visibility: hidden; width: 168px" id="txtCurrCode" type="text" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height: 300px" class="container">
                                    <asp:GridView ID="grdReceipt" TabIndex="10" runat="server" Font-Size="10px" CssClass="td_cell"
                                        Width="960px" BackColor="White" AutoGenerateColumns="False" BorderColor="#999999"
                                        BorderStyle="None" CellPadding="3" GridLines="Vertical">
                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Type">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <select id="ddlType" runat="server" class="field_input MyAutoCompleteTypeClass" style="width: 45px"
                                                        tabindex="0">
                                                    </select>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="middle" Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="A/C Code">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <input type="text" name="accCodeSearch" class="field_input MyAutoCompleteClass" tabindex="0"
                                                        style="width: 98%; font" id="accCodeSearch" runat="server" />
                                                    <select style="width: 70px" id="ddlgAccCode" class="field_input" tabindex="0" runat="server">
                                                    </select>
                                                    <select style="width: 70px" id="ddlConAccCode" class="field_input" tabindex="0" runat="server">
                                                    </select>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="bottom" Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="A/C Name">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <input type="text" name="accSearch" class="field_input MyAutoCompleteClass" style="width: 98%;
                                                        font" id="accSearch" runat="server" />
                                                    <select style="width: 225px" id="ddlgAccName" class="field_input MyDropDownListCustValue"
                                                        tabindex="0" runat="server">
                                                    </select>
                                                    <select style="width: 225px" id="ddlConAccName" class="field_input" tabindex="0"
                                                        runat="server">
                                                    </select>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <select style="width: 55px; display: none" id="ddlCostCode" class="field_input" runat="server">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </ItemTemplate>
                                                <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                                <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <select style="width: 60px; display: none" id="ddlCostName" class="field_input" runat="server">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </ItemTemplate>
                                                <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                                <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Narration">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <input style="width: 150px" id="txtgnarration" class="field_input" type="text" maxlength="200"
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Currency">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <input style="width: 40px" id="txtCurrency" class="field_input" readonly="readonly"
                                                        type="text" maxlength="50" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Exchg. Rate">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <input id="txtConvRate" runat="server" class="field_input" maxlength="50" style="width: 45px"
                                                        type="text" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Debit">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="width: 70px; text-align: right" id="txtDebit" class="field_input" type="text"
                                                        maxlength="12" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Credit">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="width: 70px; text-align: right" id="txtCredit" class="field_input"
                                                        type="text" maxlength="12" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BaseCurr Debit">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="width: 78px; text-align: right" id="txtBaseDebit" class="field_input"
                                                        type="text" maxlength="50" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BaseCurr Credit">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="width: 80px; text-align: right" id="txtBaseCredit" class="field_input"
                                                        type="text" maxlength="50" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Market">
                                                <ItemTemplate>
                                                    <select style="width: 100px" id="ddldept" class="field_input" runat="server">
                                                        <option selected></option>
                                                    </select>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Adjust Bill">
                                                <ItemTemplate>
                                                    <input style="width: 28px" id="btnAd" class="field_button" type="button" value="A.B"
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ControlStyle BackColor="White" BorderStyle="None" BorderColor="White"></ControlStyle>
                                                <ItemStyle BackColor="White" BorderStyle="None" BorderColor="White"></ItemStyle>
                                                <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                        border-right-color: #eeeeee" id="txtOldLineno" type="text" runat="server" />
                                                </ItemTemplate>
                                                <FooterStyle BackColor="White" BorderStyle="None" BorderColor="White"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ControlStyle BorderStyle="None"></ControlStyle>
                                                <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                                <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                        border-right-color: #eeeeee" id="txtctrolaccode" class="field_input" readonly
                                                        type="text" maxlength="50" value=" " runat="server" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    &nbsp;
                                                </EditItemTemplate>
                                                <ControlStyle BorderStyle="None"></ControlStyle>
                                                <ItemStyle BackColor="White" BorderStyle="None" Width="1px"></ItemStyle>
                                                <HeaderStyle BackColor="White" BorderStyle="None" Width="1px" BorderColor="White">
                                                </HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 44px; border-top-color: #eeeeee;
                                                        border-right-color: #eeeeee" id="txtlineno" class="field_input" readonly type="text"
                                                        maxlength="50" value='<%# Bind("LineNo") %>' runat="server" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ControlStyle BorderStyle="None"></ControlStyle>
                                                <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                                <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                        border-right-color: #eeeeee" id="txtcontrolacname" class="field_input" readonly
                                                        type="text" maxlength="50" value=" " runat="server" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ControlStyle BorderStyle="None"></ControlStyle>
                                                <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                                <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                        border-right-color: #eeeeee" id="txtacctcode" class="field_input" readonly type="text"
                                                        maxlength="50" value=" " runat="server" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ControlStyle BackColor="White" BorderStyle="None"></ControlStyle>
                                                <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                                <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                                <ItemTemplate>
                                                    <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                        border-right-color: #eeeeee" id="txtacctname" class="field_input" readonly type="text"
                                                        maxlength="50" value=" " runat="server" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                                <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                                <ItemTemplate>
                                                    <input style="width: 75px; display: none" id="txtrequestid" class="field_input" type="text"
                                                        maxlength="20" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="grdRowstyle"></RowStyle>
                                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                        <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center">
                                        </PagerStyle>
                                        <HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                    </asp:GridView>
                                </div>
                                <asp:Button ID="btnAdd" TabIndex="18" runat="server" Text="Add Row" CssClass="field_button"
                                    Font-Bold="True"></asp:Button>&nbsp;
                                <asp:Button ID="btnDelLine" TabIndex="19" OnClick="btnDelLine_Click" runat="server"
                                    Text="DeleteRow" CssClass="field_button" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center" style="font-weight: normal; font-family: 'Times New Roman';
                                padding-top: 15px">
                                <asp:Label ID="Label2" runat="server" Text="Credit/Debit Total" CssClass="filed_caption"
                                    Width="98px"></asp:Label>
                                <input style="width: 80px; text-align: right" id="txtTotalDebit" class="field_input"
                                    readonly="readonly" type="text" maxlength="50" runat="server" tabindex="42" />
                                <input style="width: 80px; text-align: right" id="txtTotalCredit" class="field_input"
                                    readonly="readonly" type="text" maxlength="50" runat="server" tabindex="43" />
                                <asp:Label ID="lblBaseTot" runat="server" Text="Base Total" CssClass="filed_caption"
                                    Width="98px"></asp:Label>
                                <input style="width: 80px; text-align: right" id="txtTotBaseDebit" class="field_input"
                                    readonly="readonly" type="text" maxlength="100" runat="server" tabindex="44" />
                                <input style="width: 80px; text-align: right" id="txtTotBaseCredit" class="field_input"
                                    readonly="readonly" type="text" maxlength="100" runat="server" tabindex="45" />
                                <asp:Label ID="lblBaseDiff" runat="server" CssClass="filed_caption" Text=" Total"
                                    Width="100px"></asp:Label>
                                <input style="width: 60px; text-align: right;" id="txtTotBaseDiff" class="field_input"
                                    readonly="readonly" type="text" maxlength="100" runat="server" tabindex="46" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 15px">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3" style="font-weight: normal; font-family: 'Times New Roman';
                                font-variant: normal">
                                <input style="width: 29px; visibility: hidden;" id="txtAdjcolno" type="text" maxlength="20"
                                    runat="server" />
                                <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                    height: 9px" type="text" />
                                <input style="visibility: hidden; width: 29px" id="txtOldAmount" type="text" maxlength="20"
                                    runat="server" />
                                <input style="visibility: hidden; width: 29px" id="txtMode" type="text" maxlength="20"
                                    runat="server" />&nbsp;<input style="visibility: hidden; width: 29px" id="txtDivCode"
                                        class="field_input MyAutoCompletedivClass" type="text" maxlength="20" runat="server" />
                                <input style="visibility: hidden; width: 29px" id="txtGridType" type="text" maxlength="20"
                                    runat="server" />
                                <input style="visibility: hidden; width: 29px" id="txtTranType" type="text" maxlength="20"
                                    runat="server" />
                                <input style="visibility: hidden; width: 29px" id="txtbasecurr" type="text" maxlength="20"
                                    runat="server" />
                                <input style="visibility: hidden; width: 33px" id="txtdecimal" type="text" maxlength="15"
                                    runat="server" />
                                <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
                                <input style="visibility: hidden; width: 137px" id="txtgridrows" type="text" runat="server" />&nbsp;<input
                                    id="chkBlank" runat="server" disabled="disabled" type="checkbox" visible="true" />
                                Allow Blank &nbsp;<input id="chkPrntInclude" runat="server" type="checkbox" visible="false" /><asp:Label
                                    ID="lblPrntInclude" runat="server" Text="Include 2nd page" Visible="false"></asp:Label>
                                &nbsp;
                                <asp:CheckBox ID="chkPost" TabIndex="20" runat="server" Text="Post/UnPost" Font-Size="10px"
                                    Font-Names="Verdana" Width="103px" CssClass="field_caption" ForeColor="Black"
                                    Font-Bold="True" BackColor="#FFC0C0" Checked="true"></asp:CheckBox>&nbsp;
                                <asp:Button ID="btnSave" TabIndex="21" OnClick="btnSave_Click" runat="server" Text="Save"
                                    CssClass="field_button" Font-Bold="True"></asp:Button>&nbsp;
                                <asp:Button ID="btnPrint" TabIndex="22" runat="server" Text="Print" CssClass="field_button">
                                </asp:Button>
                                <asp:Button ID="btnclientreceipt" TabIndex="23" runat="server" Text="Client Receipt"
                                    CssClass="field_button"></asp:Button>
                                &nbsp;
                                <asp:Button ID="btnExit" TabIndex="24" OnClick="btnExit_Click" runat="server" Text="Exit"
                                    CssClass="field_button" Font-Bold="True"></asp:Button>&nbsp;<asp:Button ID="btnhelp"
                                        TabIndex="24" OnClick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button">
                                    </asp:Button>
                                &nbsp; &nbsp;<asp:CheckBox ID="chkadjust" runat="server" Text="Allow any way" Visible="False"
                                    Width="121px" />
                                &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                    TargetControlID="txtdate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtDate"
                                    AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                                    MaskType="Date" MessageValidatorTip="true">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="ClExChequeDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton1"
                                    TargetControlID="txtChequeDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" TargetControlID="txtChequeDate"
                                    AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                                    MaskType="Date" MessageValidatorTip="true">
                                </cc1:MaskedEditExtender>
                                <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                                    <Services>
                                        <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                                    </Services>
                                </asp:ScriptManagerProxy>
                                <asp:HiddenField ID="hdnss" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input style="visibility: hidden;" id="txtAccCode" type="text" runat="server" />&nbsp;
                                <input style="visibility: hidden;" id="txtAccName" type="text" runat="server" />
                                <asp:Button ID="hdnValidate" runat="server" Style="display:none" OnClick="hdnValidate_Click" />
                            </td> 
                        </tr>
                    </tbody>
                </table>
                &nbsp;</asp:Panel>
            &nbsp;
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
