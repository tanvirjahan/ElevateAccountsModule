<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReceiptsAdjustBills.aspx.vb"
    Inherits="ReceiptsAdjustBills" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        .container
        {
            overflow: auto;
            border: 1px solid black;
        }
        
        .container table th
        {
            position: relative;
            font-weight: bold;
            border-bottom: solid 1px #CCCCCC;
            text-align: left;
        }
        
        .container table tbody
        {
            overflow-x: hidden;
        }
        
        .container table tbody tr td
        {
            border-bottom: solid 1px #CCCCCC;
            text-align: left;
        }
    </style>
    <title></title>
    <link rel="stylesheet" href="../CSS/Styles.css" type="text/css" />
    <script language="javascript" src="../js/date-picker.js" type="text/javascript"></script>
    <script language="javascript" src="../js/datefun.js" type="text/javascript"></script>
    <script language="javascript" src="../FillDropDown.js" type="text/javascript"></script>

   
    <script src="../Scripts/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>

</head>
<script language="javascript" type="text/javascript">
    var nodecround = null;

   

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }


    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
    }
    function DecRound(amtToRound) {

   
        //changed by mohamed on 14/10/2021
        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));

    //sharfudeen 31/10/2022
     // var amtToRound1 = parseFloat(amtToRound).toFixed(parseInt(txtdec.value)+1);
    var amtToRound1 = parseFloat(amtToRound).toFixed(parseInt(txtdec.value));

        var rdamt = Math.round(parseFloat(amtToRound1) * nodecround) / nodecround;
        return parseFloat(rdamt);

 /* sharfudeen 02/11/2022
      var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));
         var rdamt=amtToRound;

         $.ajax({
                type: "POST",
                url: "ReceiptsAdjustBills.aspx/DecRoundMethod",
               data: '{Ramt: "' + amtToRound + '",intmdecimal:  "' + parseInt(txtdec.value) + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                  //  alert(response.d);
                    rdamt=response.d;
                },
                failure: function (response) {
                    alert('failure');
                    alert(response.d);
                },
                error: function (response) {
                    alert('error');
                    alert(response.d);
                }
            });
*/
        
    }

 function DecRoundajax(amtToRound) 
 {
         var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));
         var rdamt=amtToRound;

    //    var rdamt = Math.round(parseFloat(amtToRound) * nodecround) / nodecround;


            $.ajax({
                type: "POST",
                url: "ReceiptsAdjustBills.aspx/DecRoundMethod",
               data: '{Ramt: "' + amtToRound + '",intmdecimal:  "' + parseInt(txtdec.value) + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert(response.d);
                    rdamt=response.d;
                },
                failure: function (response) {
                    alert('failure');
                    alert(response.d);
                },
                error: function (response) {
                    alert('error');
                    alert(response.d);
                }
            });
             return parseFloat(rdamt);
        }

        
      

     function DecRoundtest(amtToRound) 
     {
        //changed by mohamed on 14/10/2021
        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));

        var amtToRound1 = parseFloat(amtToRound).toFixed(parseInt(txtdec.value)+1);
                  alert(parseFloat(amtToRound1));

        var rdamt = Math.round(parseFloat(amtToRound1) * nodecround) / nodecround;
      
        return parseFloat(rdamt);
    }

    function DecRound88(amtToRound) {
        //changed by mohamed on 14/10/2021
        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        //alert(amtToRound);
        //alert((parseInt(txtdec.value)+1).toString());
        //var amtdec= Number((Math.round(amtToRound + "e3")  + "e-3"));
        //var rdamt= Number((Math.round(amtToRound + "e" + (parseInt(txtdec.value)+2).toString() )  + "e-" + (parseInt(txtdec.value)+2).toString() ) );
        //var rdamt= Number((Math.round(amtToRound + "e" + (parseInt(txtdec.value)+1).toString() )  + "e-" + (parseInt(txtdec.value)+1).toString() ) );
        var rdamt3dec=  parseInt(amtToRound*1000);
        var rdamt3dec10amt=  parseInt(rdamt3dec/10)*10;
        var rdamt3digit=  rdamt3dec-rdamt3dec10amt;
        var rdamt2digit = rdamt3dec10amt / 1000;
        if (rdamt3digit>=5){
            alert(rdamt3digit);
            rdamt2digit= (rdamt2digit + 0.01).toFixed(parseInt(txtdec.value));
        }
        //alert(rdamt);
        //var rdamt= Number((Math.round(rdamt3dec + "e" + (parseInt(txtdec.value)+1).toString() )  + "e-" + (parseInt(txtdec.value)+1).toString() ) );
        //nodecround = Math.pow(10, parseInt(txtdec.value));
        //var rdamt = Math.round(parseFloat(amtToRound) + "e" + txtdec.value ) + "e-" + txtdec.value ;
        //var rdamt= Number((Math.round(rdamt + "e" + txtdec.value)  + "e-" + txtdec.value));
        var rdamt = rdamt2digit;
        return parseFloat(rdamt);
    }
    
    function DecRound7(amtToRound) {
        //return +(Math.round(amtToRound + "e+2") + "e-2");

        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        //nodecround = Math.pow(10, parseInt(txtdec.value));
        //return Math.ceil(Math.abs(amtToRound)) * Math.sign(amtToRound);
        var decimalValue = 0;
        var startValue = parseFloat(amtToRound); 
        digits = parseInt(txtdec.value) //digits || 0;
        startValue *=  parseFloat(Math.pow(10, (digits + 1)));
        decimalValue = parseInt(Math.floor(startValue)) - (Math.floor(startValue / 10) * 10);
        startValue = Math.floor(startValue / 10);
        if (decimalValue >= 5) {
            startValue += 1;
        }
        startValue /=  parseFloat(Math.pow(10, (digits)));
        return startValue;
    }


    function DecRound2(amtToRound) {
    var num=parseFloat(amtToRound);
    var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
    var d = parseInt(txtdec.value) //decimalPlaces || 0;
    var m = Math.pow(10, d);
    var n = +(d ? num * m : num).toFixed(8); // Avoid rounding errors
    var i = Math.floor(n), f = n - i;
    var e = 1e-8; // Allow for rounding errors in f
    var r = (f > 0.5 - e && f < 0.5 + e) ?
                ((i % 2 == 0) ? i : i + 1) : Math.round(n);
    var rdamt = d ? r / m : r;
     return parseFloat(rdamt);
    }
    
     function DecRound1(amtToRound) {
        
        rdamountonsuccess = 0;
        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));
        var rdamt = Math.round(parseFloat(amtToRound) * nodecround) / nodecround;
        //alert('a');
        //sleep(2000).then(() => { console.log("World!"); });

       // PageMethods.DecRoundMethod(parseFloat(amtToRound), parseInt(txtdec.value), DecRoundOnSuccess);

//        PageMethods.DecRoundMethod(parseFloat(amtToRound), parseInt(txtdec.value), function (response) {
//            rdamountonsuccess = parseFloat(rdamountonsuccess);
//            return parseFloat(response);
//        });

//        ReceiptsAdjustBills.DecRoundMethod(parseFloat(amtToRound), parseInt(txtdec.value), function (response) {
//            rdamountonsuccess = parseFloat(rdamountonsuccess);
//            return parseFloat(response);
//        });

        return parseFloat(rdamt);
        //alert('c');

//        alert('a1');
//        sleep(2000);
//        alert('a');
        //return parseFloat(rdamountonsuccess);
        
    }
    function sleep(milliseconds) {
        const date = Date.now();
        let currentDate = null;
        do {
        currentDate = Date.now();
        } while (currentDate - date < milliseconds);
    }

    function sleep1(delay) {
        var start = new Date().getTime();
        while (new Date().getTime() < start + delay);
    }

    function DecRoundOnSuccess(response) {
        //alert('b');
        var rdamountonsuccess;
        rdamountonsuccess = response;
        return parseFloat(rdamountonsuccess);
    }


//    function sleep2(ms) {
//      return new Promise(resolve => setTimeout(resolve, ms));
//   }

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

    //---------------------------------------------------
    function OnClickOk() {

        var txtBal = document.getElementById('<%=txtBalAdu.ClientID%>');
        var txtBaseBal = document.getElementById('<%=txtBalAduinBase.ClientID%>');

        var valAd = txtBal.value;
        var valBaseAd = txtBaseBal.value;

        if (valAd == '') { valAd = 0; }
        if (isNaN(valAd) == true) { valAd = 0; }

        if (valBaseAd == '') { valBaseAd = 0; }
        if (isNaN(valBaseAd) == true) { valBaseAd = 0; }

        if (valAd == 0) {
            // window.close();
            if (valBaseAd != 0) {
                var strMsg = "Please pass entry for exchange difference of " + valBaseAd + " value";
                alert(strMsg);
            }

        } else {
            var txtfl = document.getElementById('<%=txtflag.ClientID%>');
            if (confirm('Do you want to generate auto advance entry for Balance to Adjust : ' + valAd) == false) {
                txtfl.value = 0;
                //                window.close();
            }
            else {
                txtfl.value = 1;
                //                window.close();
            }

        }
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

    var glb_baseconvrate =0.00000;

    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

    //Added by Ram 21-09-2023

    function OnChangeChkAll() {
        var gridView = document.getElementById('<%= grdAdjustBill.ClientID %>');
        
        if (gridView) {
            var rows = gridView.getElementsByTagName('tr');
            var chkboxall = rows[0].querySelector('[id*="chkBillAll"]');
            for (var i = 1; i < rows.length; i++) {
                var row = rows[i];
                var txtAdujustAmt = row.querySelector('[id*="txtAdujustAmt"]');
                var chkbox = row.querySelector('[id*="chkBill"]');
                var txtBalAmount = row.querySelector('[id*="txtBalAmount"]');
                var txtCurRate = row.querySelector('[id*="txtCurRate"]');
                var txtBaseAmount = row.querySelector('[id*="txtBaseAmount"]');
                var rowIndex = i;
                var hdnCurrCode = row.querySelector('[id*="hdnCurrCode"]');
                var hdnbaseconrate = row.querySelector('[id*="hdnbaseconrate"]');
               // alert(' base amount Rosalin ');

                if (chkboxall.checked === false) {
                    chkbox.checked = false;
                    OnchangeChk(txtAdujustAmt.id, chkbox.id, txtBalAmount.id, txtCurRate.id, txtBaseAmount.id, rowIndex, hdnCurrCode.value, hdnbaseconrate.value);
                }
                else if (chkbox.checked === false) {
                    //console.log('Checkbox State : ' + chkbox.checked);
                    //console.log('Adjust Amount value : ' + txtAdujustAmt.value);
                    //console.log('Hidden Curr Code value : ' + hdnCurrCode.value);
                    chkbox.checked = true;
                    OnchangeChk(txtAdujustAmt.id, chkbox.id, txtBalAmount.id, txtCurRate.id, txtBaseAmount.id, rowIndex, hdnCurrCode.value, hdnbaseconrate.value);
                }
            }
        }
    }

    //End


    function OnchangeChk(txtAdj, chk, txtbamt, txtcurrate, txtbaseamt, tt,flag,baseconvrate) {

        chk = document.getElementById(chk);
        txtAdj = document.getElementById(txtAdj);
        txtbalamt = document.getElementById(txtbamt);

      

        var currcode = getUrlVars()["currcode"];

        var txtBalanceAdj = document.getElementById('<%=txtBalAdu.ClientID%>');
        var txtrAmtAdj = document.getElementById('<%=txtAmountAdjust.ClientID%>');

        var txtrbaseAmtAdj = document.getElementById('<%=txtBaseAmountAdjust.ClientID%>');
        var txtBalanceAdjinbase = document.getElementById('<%=txtBalAduinBase.ClientID%>');
        var txtHeaderbaseAmt = document.getElementById('<%=txtBaseAmount.ClientID%>');

        var txtHeaderconvrate = document.getElementById('<%=txtExchangeRate.ClientID%>'); 

        txtcurRate = document.getElementById(txtcurrate);
        txtbaseAmt = document.getElementById(txtbaseamt);



//        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
//        constr = connstr.value  
//        ColServices.clsServices.GetConvnew(constr, flag, FillCvntRate, ErrorHandler, TimeOutHandler);

                          

        if (chk.checked == true) {
            txtAdj.readOnly = false;
            if (txtBalanceAdj.value != 0) {

               
                if ((DecRound(txtbalamt.value) > DecRound(txtBalanceAdj.value)) || (DecRound(txtbalamt.value) == DecRound(txtBalanceAdj.value))) {

                    txtAdj.value = DecRound(txtBalanceAdj.value);
                    var afadjamt1 = DecRound(txtrAmtAdj.value) + DecRound(txtBalanceAdj.value);
                    txtrAmtAdj.value = DecRound(afadjamt1);
                    txtBalanceAdj.value = 0;
                }
                else {


                    if (currcode == flag) {
                        txtAdj.value = DecRound(txtbalamt.value);
                        var badjamt = DecRound(txtBalanceAdj.value) - DecRound(txtbalamt.value);
                        txtBalanceAdj.value = DecRound(badjamt);
                        var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(txtbalamt.value);
                        txtrAmtAdj.value = DecRound(afadjamt2);
                    }
                    else {
                        txtAdj.value = DecRound(txtbalamt.value);
                        // var badjamt = DecRound(txtBalanceAdj.value) - DecRound(parseFloat(txtcurRate.value) * DecRound(txtbalamt.value));

                        //var badjamt = DecRound(txtBalanceAdj.value) - DecRound(parseFloat(baseconvrate) * txtbalamt.value / parseFloat(txtHeaderconvrate.value)); //changed by mohamed on 14/10/2021 removed headerconvrate
                        //var badjamt = DecRound(txtBalanceAdj.value) - DecRound(((parseFloat(baseconvrate)*10000) * txtbalamt.value)/10000);
                        var badjamt = DecRound(txtBalanceAdj.value) - DecRound(parseFloat(baseconvrate) * txtbalamt.value);
                        txtBalanceAdj.value = DecRound(badjamt);
                        // var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(parseFloat(txtcurRate.value) * DecRound(txtbalamt.value));

                        //var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(parseFloat(baseconvrate) * txtbalamt.value / parseFloat(txtHeaderconvrate.value)); //changed by mohamed on 14/10/2021 removed headerconvrate
                        //var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(((parseFloat(baseconvrate)*10000) * txtbalamt.value)/10000);
                        var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(parseFloat(baseconvrate) * txtbalamt.value);
                        txtrAmtAdj.value = DecRound(afadjamt2);
                    }

                    
//                    txtAdj.value = DecRound(txtbalamt.value);
//                    //var badjamt = DecRound(txtBalanceAdj.value) - DecRound(txtbalamt.value);
//                    var badjamt = DecRound(txtBalanceAdj.value) - DecRound(parseFloat(txtcurRate.value) * DecRound(txtbalamt.value));
//                    txtBalanceAdj.value = DecRound(badjamt);
//                    //var afadjamt2 = DecRound(txtrAmtAdj.value) +  DecRound(txtbalamt.value);
//                    var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(parseFloat(txtcurRate.value) * DecRound(txtbalamt.value));
//                    txtrAmtAdj.value = DecRound(afadjamt2);
                }

            }
            else {
                if (DecRound(txtbalamt.value) < 0) {
                    txtAdj.readOnly = true;
                    txtBalanceAdj.value = -DecRound(txtbalamt.value);
                    txtAdj.value = DecRound(txtbalamt.value)
                    var afadjamt3 = DecRound(txtrAmtAdj.value) + DecRound(txtbalamt.value);
                    txtrAmtAdj.value = DecRound(afadjamt3);
                }
                else {
                    txtAdj.value = 0;
                    txtbaseAmt.value = 0;
                    txtAdj.readOnly = true;
                    chk.checked = false;
                }
            }
 
            //txtrbaseAmtAdj.value = parseFloat(txtrbaseAmtAdj.value) + (((parseFloat(txtcurRate.value)*10000) * DecRound(txtAdj.value))/10000); //changed by mohamed 14/10/2021
         
       
             txtrbaseAmtAdj.value = parseFloat(txtrbaseAmtAdj.value) + (parseFloat(txtcurRate.value) * DecRound(txtAdj.value)); //changed by mohamed 14/10/2021

             // sharfudeen 02/11/2022
          // txtrbaseAmtAdj.value = DecRound(parseFloat(txtrbaseAmtAdj.value)) + DecRound(parseFloat(txtcurRate.value) * DecRound(txtAdj.value));

        //  var decrountajx = DecRoundajax(txtAdj.value);

           // txtcurRate.value = parseFloat(txtrbaseAmtAdj.value) / parseFloat(txtAdj.value);
      

            txtrbaseAmtAdj.value = DecRound(txtrbaseAmtAdj.value);
             

            txtBalanceAdjinbase.value = parseFloat(txtHeaderbaseAmt.value) - parseFloat(txtrbaseAmtAdj.value);
            txtBalanceAdjinbase.value = DecRound(txtBalanceAdjinbase.value);
        }
        else {

            txtAdj.readOnly = true;
            if (currcode == flag) {

                var badjamt = DecRound(txtBalanceAdj.value) + DecRound(txtAdj.value);
            }
            else {
                //var badjamt = DecRound(txtBalanceAdj.value) + DecRound(parseFloat(txtcurRate.value) * (txtAdj.value));
                //var badjamt = DecRound(txtBalanceAdj.value) + DecRound(parseFloat(baseconvrate) * (txtAdj.value) / parseFloat(txtHeaderconvrate.value)); //changed by mohamed on 14/10/2021 removed headerconvrate
                //var badjamt = DecRound(txtBalanceAdj.value) + DecRound(((parseFloat(baseconvrate)*10000) * (txtAdj.value))/10000);
                var badjamt = DecRound(txtBalanceAdj.value) + DecRound(parseFloat(baseconvrate) * (txtAdj.value));
            }
            txtBalanceAdj.value = DecRound(badjamt);
            if (currcode == flag) {
                var afadjamt4 = DecRound(txtrAmtAdj.value) - DecRound(txtAdj.value);
            }
            else {

               // var afadjamt4 = DecRound(txtrAmtAdj.value) - DecRound(parseFloat(txtcurRate.value) * (txtAdj.value));
               
                //var afadjamt4 = DecRound(txtrAmtAdj.value) - DecRound(parseFloat(baseconvrate) * (txtAdj.value) / parseFloat(txtHeaderconvrate.value)); //changed by mohamed on 14/10/2021 removed headerconvrate
                //var afadjamt4 = DecRound(txtrAmtAdj.value) - DecRound(((parseFloat(baseconvrate)*10000) * (txtAdj.value))/10000 );
                var afadjamt4 = DecRound(txtrAmtAdj.value) - DecRound(parseFloat(baseconvrate) * (txtAdj.value) );
            }

            txtrAmtAdj.value = DecRound(afadjamt4);
            //var bamt = DecRound(((parseFloat(txtcurRate.value)*10000) * DecRound(txtAdj.value))/10000); //changed by mohamed on 14/10/2021
            var bamt = DecRound(parseFloat(txtcurRate.value) * DecRound(txtAdj.value)); //changed by mohamed on 14/10/2021 


            txtrbaseAmtAdj.value = parseFloat(txtrbaseAmtAdj.value) - parseFloat(bamt);
            txtrbaseAmtAdj.value = DecRound(txtrbaseAmtAdj.value);
            txtBalanceAdjinbase.value = parseFloat(txtHeaderbaseAmt.value) - parseFloat(txtrbaseAmtAdj.value);
            txtBalanceAdjinbase.value = DecRound(txtBalanceAdjinbase.value);
            txtAdj.value = 0;
        }

        //var baseamount = ((parseFloat(txtcurRate.value)*10000) * DecRound(txtAdj.value))/10000;
        var baseamount = parseFloat(txtcurRate.value) * DecRound(txtAdj.value);
        txtbaseAmt.value = DecRound(baseamount);

                
    }

    function FillCvntRate(result) {
        glb_baseconvrate = DecRound(result[0].ListText);
    } 
    function AdjestBillChange(txtAdj, chk, txtbamt, txtcurrate, txtbaseamt, rowind) {
        var rw = rowind;
        var valTotal = 0;
        var objGridView = document.getElementById('<%=grdAdjustBill.ClientID%>');
        var txtrowcnt = document.getElementById('<%=txtgrdAdjRows.ClientID%>');
        intRows = txtrowcnt.value;

        var basevalTotal = 0;
        for (j = 1; j <= intRows; j++) {
            if (rw != j - 1) {

                if (objGridView.rows[j].cells[0].children[0].checked == true) {
                    var valAd = objGridView.rows[j].cells[8].children[0].value;
                    if (valAd == '') { valAd = 0; }
                    if (isNaN(valAd) == true) { valAd = 0; }
                    valTotal = DecRound(valTotal) + DecRound(valAd);

                    var valAd1 = objGridView.rows[j].cells[9].children[0].value;
                    if (valAd1 == '') { valAd1 = 0; }
                    if (isNaN(valAd1) == true) { valAd1 = 0; }
                    basevalTotal = DecRound(basevalTotal) + DecRound(valAd1);

                }
            }
        }



        var objGridView = document.getElementById('<%=grdAdPay.ClientID%>');
        var txtrowcnt = document.getElementById('<%=txtgrdAdpayRows.ClientID%>');
        var grdtype = document.getElementById('<%=txtGrdType.ClientID%>');
        intRows = txtrowcnt.value;

        for (j = 1; j <= intRows; j++) {
            var valAd = 0;
            var basevalAd = 0;
            if (grdtype.value == 'Debit') {
                valAd = objGridView.rows[j].cells[5].children[0].value;
                basevalAd = objGridView.rows[j].cells[6].children[0].value;
            }
            else if (grdtype.value == 'Credit') {
                valAd = objGridView.rows[j].cells[5].children[0].value;
                basevalAd = objGridView.rows[j].cells[6].children[0].value;
            }
            if (valAd == '') { valAd = 0; }
            if (isNaN(valAd) == true) { valAd = 0; }
            valTotal = DecRound(valTotal) + DecRound(valAd);

            if (basevalAd == '') { basevalAd = 0; }
            if (isNaN(basevalAd) == true) { basevalAd = 0; }
            basevalTotal = DecRound(basevalTotal) + DecRound(basevalAd);

        }

        var txtrAmount = document.getElementById('<%=txtADAmount.ClientID%>');
        var txtrAmtAdj = document.getElementById('<%=txtAmountAdjust.ClientID%>');
        var txtBalanceAdj = document.getElementById('<%=txtBalAdu.ClientID%>');

        var txtrbaseAmtAdj = document.getElementById('<%=txtBaseAmountAdjust.ClientID%>');
        var txtBalanceAdjinbase = document.getElementById('<%=txtBalAduinBase.ClientID%>');
        var txtHeaderbaseAmt = document.getElementById('<%=txtBaseAmount.ClientID%>');

        txtcurRate = document.getElementById(txtcurrate);
        txtbaseAmt = document.getElementById(txtbaseamt);

        var balamt1 = DecRound(txtrAmount.value) - DecRound(valTotal); // adjustBalace
        var afadjamt1 = DecRound(valTotal); // after adjusted Balace
        txtBalanceAdj.value = balamt1;
        txtrAmtAdj.value = afadjamt1;
        txtrbaseAmtAdj.value = DecRound(basevalTotal);
        txtBalanceAdjinbase.value = parseFloat(txtHeaderbaseAmt.value) - parseFloat(txtrbaseAmtAdj.value);

        txtAdj = document.getElementById(txtAdj);
        txtbalamt = document.getElementById(txtbamt);

        if (txtAdj.value == '') { txtAdj.value = 0; }
        if (isNaN(txtAdj.value) == true) { txtAdj.value = 0; }

        if (txtAdj.value != 0) {
            if ((DecRound(txtBalanceAdj.value) > DecRound(txtAdj.value)) || (DecRound(txtBalanceAdj.value) == DecRound(txtAdj.value))) {
                if ((DecRound(txtbalamt.value) > DecRound(txtAdj.value)) || (DecRound(txtbalamt.value) == DecRound(txtAdj.value))) {
                    var balamt2 = DecRound(txtBalanceAdj.value) - DecRound(txtAdj.value);
                    txtBalanceAdj.value = DecRound(balamt2);
                    var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(txtAdj.value);
                    txtrAmtAdj.value = DecRound(afadjamt2);
                }
                else {
                    alert('You have only ' + txtbalamt.value + ' amout to adjust in this line');
                    var balamt3 = DecRound(txtBalanceAdj.value) - DecRound(txtbalamt.value);
                    txtBalanceAdj.value = DecRound(balamt3);
                    var afadjamt3 = DecRound(txtrAmtAdj.value) + DecRound(txtbalamt.value);
                    txtrAmtAdj.value = DecRound(afadjamt3);
                    txtAdj.value = DecRound(txtbalamt.value);
                }
            }
            else {
                alert('You have only ' + txtBalanceAdj.value + ' amout to adjusted');
                if ((DecRound(txtbalamt.value) > DecRound(txtBalanceAdj.value)) || (DecRound(txtbalamt.value) == DecRound(txtBalanceAdj.value))) {
                    var balamt4 = DecRound(txtBalanceAdj.value);
                    var afadjamt4 = DecRound(txtrAmtAdj.value) + DecRound(txtBalanceAdj.value);
                    txtrAmtAdj.value = DecRound(afadjamt4);
                    txtAdj.value = DecRound(balamt4);
                    txtBalanceAdj.value = 0;
                }
                else {
                    alert('You have only ' + txtbalamt.value + ' amout to adjust in this line');
                    var balamt5 = DecRound(txtBalanceAdj.value) - DecRound(txtbalamt.value);
                    txtBalanceAdj.value = DecRound(balamt5);
                    var afadjamt5 = DecRound(txtrAmtAdj.value) + DecRound(txtbalamt.value);
                    txtrAmtAdj.value = DecRound(afadjamt5);
                    txtAdj.value = DecRound(txtbalamt.value);
                }
            }
        }

        //var baseamount = ((parseFloat(txtcurRate.value)*10000) * DecRound(txtAdj.value))/10000; //changed by mohamed on 14/10/2021
        var baseamount = parseFloat(txtcurRate.value) * DecRound(txtAdj.value); //changed by mohamed on 14/10/2021
        txtbaseAmt.value = DecRound(baseamount);

        txtrbaseAmtAdj.value = DecRound(txtrbaseAmtAdj.value) + DecRound(baseamount);
        txtrbaseAmtAdj.value = DecRound(txtrbaseAmtAdj.value);
        txtBalanceAdjinbase.value = parseFloat(txtHeaderbaseAmt.value) - parseFloat(txtrbaseAmtAdj.value);
        txtBalanceAdjinbase.value = DecRound(txtBalanceAdjinbase.value);
    }

     function OnChangeAdvanceno(txtADocNo,hdnADocNo) 
     {
       txtADocNo = document.getElementById(txtADocNo);
       hdnADocNo = document.getElementById(hdnADocNo);

        var btnadvance  = document.getElementById('<%=btnadvance.ClientID%>');
        var  hdnadvancelastno = document.getElementById('<%=hdnadvancelastno.ClientID%>');
        hdnadvancelastno.value = hdnADocNo.value;
      //  alert(hdnadvancelastno.value);
        btnadvance.click();
     }


    function OnChangeAdPay(txtadvAmt, txtbaseAmt, txtrate, rowind) {
        var rw = rowind;
        var valTotal = 0;
        var objGridView = document.getElementById('<%=grdAdjustBill.ClientID%>');
        var txtrowcnt = document.getElementById('<%=txtgrdAdjRows.ClientID%>');
        intRows = txtrowcnt.value;

        var basevalTotal = 0;

        for (j = 1; j <= intRows; j++) {
            if (objGridView.rows[j].cells[0].children[0].checked == true) {
                var valAd = objGridView.rows[j].cells[8].children[0].value;
                if (valAd == '') { valAd = 0; }
                if (isNaN(valAd) == true) { valAd = 0; }
                valTotal = DecRound(valTotal) + DecRound(valAd);

                var valAd1 = objGridView.rows[j].cells[9].children[0].value;
                if (valAd1 == '') { valAd1 = 0; }
                if (isNaN(valAd1) == true) { valAd1 = 0; }
                basevalTotal = DecRound(basevalTotal) + DecRound(valAd1);
            }
        }

        var objGridView = document.getElementById('<%=grdAdPay.ClientID%>');
        var txtrowcnt = document.getElementById('<%=txtgrdAdpayRows.ClientID%>');
        var grdtype = document.getElementById('<%=txtGrdType.ClientID%>');

        intRows = txtrowcnt.value;

        for (j = 1; j <= intRows; j++) {
            if (rw != j - 1) {
                var valAd = 0;
                var basevalAd = 0;

                valAd = objGridView.rows[j].cells[5].children[0].value;
                if (valAd == '') { valAd = 0; }
                if (isNaN(valAd) == true) { valAd = 0; }
                valTotal = DecRound(valTotal) + DecRound(valAd);

                basevalAd = objGridView.rows[j].cells[6].children[0].value;
                if (basevalAd == '') { basevalAd = 0; }
                if (isNaN(basevalAd) == true) { basevalAd = 0; }
                basevalTotal = DecRound(basevalTotal) + DecRound(basevalAd);


            }

        }


        var txtrAmount = document.getElementById('<%=txtADAmount.ClientID%>');
        var txtrAmtAdj = document.getElementById('<%=txtAmountAdjust.ClientID%>');
        var txtBalanceAdj = document.getElementById('<%=txtBalAdu.ClientID%>');

        var txtrbaseAmtAdj = document.getElementById('<%=txtBaseAmountAdjust.ClientID%>');
        var txtBalanceAdjinbase = document.getElementById('<%=txtBalAduinBase.ClientID%>');
        var txtHeaderbaseAmt = document.getElementById('<%=txtBaseAmount.ClientID%>');



        var balamt1 = DecRound(txtrAmount.value) - DecRound(valTotal); // adjustBalace
        var afadjamt1 = DecRound(valTotal); // after adjusted Balace
        txtBalanceAdj.value = DecRound(balamt1);
        txtrAmtAdj.value = afadjamt1;

        //         txtrbaseAmtAdj.value=DecRound(basevalTotal);
        //         txtBalanceAdjinbase.value=parseFloat(txtHeaderbaseAmt.value)- parseFloat(txtrbaseAmtAdj.value);
        //      
        txtAdj = document.getElementById(txtadvAmt);
        txtBaseAmt = document.getElementById(txtbaseAmt);
        txtRate = document.getElementById(txtrate);
        var baseamt;

        txtrbaseAmtAdj.value = DecRound(basevalTotal);
        txtBalanceAdjinbase.value = parseFloat(txtHeaderbaseAmt.value) - parseFloat(txtrbaseAmtAdj.value);

        if (txtAdj.value == '') { txtAdj.value = 0; }
        if (isNaN(txtAdj.value) == true) { txtAdj.value = 0; }

        if (txtAdj.value != 0) {
            if ((DecRound(txtBalanceAdj.value) > DecRound(txtAdj.value)) || (DecRound(txtBalanceAdj.value) == DecRound(txtAdj.value))) {
                var balamt2 = DecRound(txtBalanceAdj.value) - DecRound(txtAdj.value);

                txtBalanceAdj.value = DecRound(balamt2);
                var afadjamt2 = DecRound(txtrAmtAdj.value) + DecRound(txtAdj.value);
                txtrAmtAdj.value = DecRound(afadjamt2);
            }
            else {
                alert('You have only ' + txtBalanceAdj.value + ' amout to adjusted');
                var balamt3 = DecRound(txtBalanceAdj.value);
                var afadjamt3 = DecRound(txtrAmtAdj.value) + DecRound(txtBalanceAdj.value);
                txtrAmtAdj.value = DecRound(afadjamt3);
                txtAdj.value = DecRound(balamt3);
                txtBalanceAdj.value = 0;
            }

        }

        baseamt = DecRound(txtAdj.value) * parseFloat(txtRate.value);
        txtBaseAmt.value = DecRound(baseamt);
        txtrbaseAmtAdj.value = DecRound(txtrbaseAmtAdj.value) + DecRound(baseamt);
        txtrbaseAmtAdj.value = DecRound(txtrbaseAmtAdj.value);
        txtBalanceAdjinbase.value = parseFloat(txtHeaderbaseAmt.value) - parseFloat(txtrbaseAmtAdj.value);
        txtBalanceAdjinbase.value = DecRound(txtBalanceAdjinbase.value);

    }


    function chkvalidate(chk, requestid, guestname, amount) {
        //alert('this is testing');


        var chk = document.getElementById(chk);
        var requestid = document.getElementById(requestid);
        var guestname = document.getElementById(guestname);
        var amount = document.getElementById(amount);

        var gvET = document.getElementById("<%=GVCollection.ClientID%>");
        var objGridView = document.getElementById('<%=grdAdPay.ClientID%>');
        var rCount = gvET.rows.length;
        var rAdvcount = objGridView.rows.length;
        var rowIdx = 1;

        var rowElement;
        var chkBox;
        var i = 0;

        for (rowIdx; rowIdx <= rCount - 1; rowIdx++) {
            rowElement = gvET.rows[rowIdx];
            chkBox = rowElement.cells[0].firstChild;
            if (chkBox.checked) {
                i = i + 1
            }
        }
        if (i > 1) {
            alert('Cannot select more than one');
            chkBox.checked = false
            return false
        }


        var j = 1;
        var valAmount = objGridView.rows[1].cells[5].children[0].value;

        for (j; j <= rAdvcount - 1; j++) {

            if (chk.checked == true) {
                if (valAmount != 0) {
                    objGridView.rows[1].cells[3].children[0].value = requestid.innerHTML;
                    objGridView.rows[1].cells[8].children[0].value = guestname.innerHTML;
                }
                else {
                    alert('Enter the advance amount');
                    chk.checked = false;
                    return false;
                }
            }
            else {
                objGridView.rows[1].cells[3].children[0].value = '';
                objGridView.rows[1].cells[8].children[0].value = '';
            }

        }
        return true;

    }


    //Advance Grid This is used for Credit and Debit text box separately
    //function OnChangeAdPay( txtcrAmt,txtdbBAmt,txtbaseAmt,txtrate,rowind)
    // {
    //       var rw=rowind;
    //        var valTotal=0;
    //        var objGridView = document.getElementById('<%=grdAdjustBill.ClientID%>');
    //        var txtrowcnt =document.getElementById('<%=txtgrdAdjRows.ClientID%>');
    //        intRows=txtrowcnt.value;
    //        for(j=1;j<=intRows;j++)
    //        {
    //              if(objGridView.rows[j].cells[0].children[0].checked == true)
    //                   { 
    //                      var valAd=objGridView.rows[j].cells[8].children[0].value;
    //                      if (valAd==''){valAd=0;}
    //                      if (isNaN(valAd)==true){valAd=0;}
    //                      valTotal=DecRound(valTotal)+DecRound(valAd);
    //                   }
    //         }
    //        
    //       
    //        var objGridView = document.getElementById('<%=grdAdPay.ClientID%>');
    //        var txtrowcnt =document.getElementById('<%=txtgrdAdpayRows.ClientID%>');
    //        var grdtype =document.getElementById('<%=txtGrdType.ClientID%>');
    //        intRows=txtrowcnt.value;
    //  
    //        for(j=1;j<=intRows;j++)
    //        {
    //        if (rw !=j-1 )
    //            {
    //              var valAd = 0;
    //               if (grdtype.value =='Debit')
    //                {
    //                  valAd=objGridView.rows[j].cells[5].children[0].value;                  
    //                  }
    //                else if (grdtype.value =='Credit')
    //                {
    //                 valAd=objGridView.rows[j].cells[6].children[0].value;                 
    //                }
    //                if (valAd==''){valAd=0;}
    //                if (isNaN(valAd)==true){valAd=0;}
    //                valTotal=DecRound(valTotal)+DecRound(valAd);
    //            }
    //             
    //        }
    //           
    //        
    //        var txtrAmount =document.getElementById('<%=txtADAmount.ClientID%>');
    //        var txtrAmtAdj =document.getElementById('<%=txtAmountAdjust.ClientID%>');
    //        var txtBalanceAdj =document.getElementById('<%=txtBalAdu.ClientID%>');
    //        var balamt1=DecRound(txtrAmount.value)- DecRound(valTotal); // adjustBalace
    //        var afadjamt1 =DecRound(valTotal); // after adjusted Balace
    //        txtBalanceAdj.value=DecRound(balamt1);
    //        txtrAmtAdj.value=afadjamt1;

    //  
    //         txtCRAmt=document.getElementById(txtcrAmt);
    //         txtDBAmt=document.getElementById(txtdbBAmt);
    //         txtBaseAmt=document.getElementById(txtbaseAmt);
    //         txtRate=document.getElementById(txtrate);
    //         var baseamt;
    //         var txtAdj=null;
    //          if (grdtype.value =='Debit')
    //          {       
    //               txtAdj = document.getElementById(txtdbBAmt);
    //           }
    //         else if (grdtype.value =='Credit')
    //             {
    //                 txtAdj = document.getElementById(txtcrAmt);
    //              }
    //       
    //          
    //        if (txtAdj.value==''){txtAdj.value=0;}
    //        if (isNaN(txtAdj.value)==true){txtAdj.value=0;}
    //       
    //       
    //         if(txtAdj.value!=0)
    //        {
    //              
    //               if ((DecRound(txtBalanceAdj.value) > DecRound(txtAdj.value)) || (DecRound(txtBalanceAdj.value) == DecRound(txtAdj.value) ) )
    //                     {
    //                         var balamt2=DecRound(txtBalanceAdj.value)- DecRound(txtAdj.value);
    //                      
    //                         txtBalanceAdj.value=DecRound(balamt2);
    //                         var afadjamt2=DecRound(txtrAmtAdj.value)+ DecRound(txtAdj.value);
    //                         txtrAmtAdj.value=DecRound(afadjamt2);
    //                      }
    //                     else
    //                     {
    //                        alert('You have only '+txtBalanceAdj.value+ ' amout to adjusted');
    //                         var balamt3=DecRound(txtBalanceAdj.value);
    //                         var afadjamt3=DecRound(txtrAmtAdj.value)+ DecRound(txtBalanceAdj.value);
    //                         txtrAmtAdj.value=DecRound(afadjamt3);
    //                         txtAdj.value=DecRound(balamt3);
    //                         txtBalanceAdj.value=0;
    //                     }
    //  
    //        }
    //          
    //         baseamt= DecRound(txtAdj.value)* DecRound(txtRate.value) ;     
    //         txtBaseAmt.value=DecRound(baseamt);
    // }

    function OnchangeChk_checked() {
        var hdnchkid1 = document.getElementById('<%=hdnchkid.ClientID%>');
        var hdnreqid1 = document.getElementById('<%=hdnreqid.ClientID%>');
        if (hdnchkid1.value != '') {
            var chk = document.getElementById(hdnchkid1.value);
            if (hdnreqid1.value != '') {
                chk.click();
            }
        }

    }

</script>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="td_cell" style="border-left-color: dimgray; border-bottom-color: dimgray;
                    border-top-style: solid; border-top-color: dimgray; border-right-style: solid;
                    border-left-style: solid; border-right-color: dimgray; border-bottom-style: solid">
                    <tr>
                        <td align="center" class="field_heading">
                            <asp:Label ID="Label2" runat="server" CssClass="field_heading" Text="Adjust Bills"
                                Width="467px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        Account Type
                                    </td>
                                    <td>
                                        <input id="txtAdAccountType" runat="server" class="field_input" readonly="readonly"
                                            type="text" style="width: 194px" tabindex="1" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Account Code
                                    </td>
                                    <td>
                                        <input id="txtAccCode" runat="server" class="field_input" readonly="readonly" type="text"
                                            style="width: 194px" tabindex="2" />
                                    </td>
                                    <td>
                                        Account Name
                                    </td>
                                    <td colspan="3">
                                        <input id="txtAccName" runat="server" class="field_input" readonly="readonly" style="width: 275px"
                                            type="text" tabindex="3" />
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
                                        SalesMan Code
                                    </td>
                                    <td>
                                        <input id="txtSalesManCode" runat="server" class="field_input" readonly="readonly"
                                            type="text" style="width: 194px" tabindex="4" />
                                    </td>
                                    <td>
                                        SalesMan Name
                                    </td>
                                    <td colspan="3">
                                        <input id="txtSalesManName" runat="server" class="field_input" readonly="readonly"
                                            style="width: 276px" type="text" tabindex="5" />
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
                                        Currency Code
                                    </td>
                                    <td>
                                        <input id="txtCurrencyCode" runat="server" class="field_input" readonly="readonly"
                                            type="text" style="width: 194px" tabindex="6" />
                                    </td>
                                    <td>
                                        Exchange Rate
                                    </td>
                                    <td>
                                        <input id="txtExchangeRate" runat="server" class="field_input" readonly="readonly"
                                            style="width: 76px" type="text" tabindex="7" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAmount" runat="server" CssClass="filed_caption" Text="Amount "
                                            Width="84px"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtADAmount" runat="server" class="field_input" readonly="readonly" type="text"
                                            tabindex="8" style="width: 100px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBaseAmtCaption" runat="server" CssClass="filed_caption" Text="Amount "
                                            Width="100px"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtBaseAmount" runat="server" class="field_input" readonly="readonly"
                                            type="text" tabindex="8" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDrCrCaption" runat="server" CssClass="filed_caption" Text="Label"
                                            Width="50px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Search No
                                    </td>
                                    <td>
                                        <select style="width: 172px" id="ddlABSearchNo" class="field_input" tabindex="10"
                                            runat="server">                                               
                                               <option value="1">Invoice No</option>
                                               <option value="2">Booking No</option>
                                               <option value="3">Ticket No</option>
                                        </select>
                                    </td>                                   
                                    <td colspan="3">
                                        <input id="txtABSearchNo" runat="server" class="field_input" 
                                            style="width: 276px" type="text" tabindex="11" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBASearch" runat="server" CssClass="field_button" Font-Bold="True"
                                            TabIndex="12" Text="Search" />&nbsp;
                                    </td>
                                    <td>
                                        <%--<asp:Button ID="btnFindNext" runat="server" CssClass="field_button" Font-Bold="True"
                                            TabIndex="13" Text="Find Next" />&nbsp;--%>
                                    </td>
                                    <td>
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:TabContainer ID="adjustBillTab" runat="server" ActiveTabIndex="0" EnableTheming="False"
                                Width="950px" AutoPostBack="True">
                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                                    <HeaderTemplate>
                                        Adjust Bills
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        &nbsp;<div class="container" style="height: 300px">
                                            <asp:GridView ID="grdAdjustBill" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                Font-Size="10px" GridLines="Vertical" TabIndex="1" Width="915px">
                                                <HeaderStyle BackColor="#06788B" Font-Bold="True" ForeColor="White" />
                                                <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="#DDD9CF" Font-Size="10px" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="LineNo" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAccTranLineNo" runat="server" Text='<%# Bind("acc_tran_lineno") %>'
                                                                Width="57px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                       <%-- Added by Ram 23102023--%>
                                                        <HeaderTemplate>
                                                            <input id="chkBillAll" title="Check All" runat="server" type="checkbox" onclick="OnChangeChkAll()" />
                                                        </HeaderTemplate>
                                                        <%-- End --%>
                                                        <EditItemTemplate>
                                                            &nbsp;
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <input id="chkBill" runat="server" type="checkbox" />
                                                            <asp:HiddenField ID="hdnCurrCode" runat="server" Value='<%# Bind("currcode") %>' />
                                                                  <%-- Added by Ram --%>
                                                            <asp:HiddenField ID="hdnbaseconrate" runat="server" Value='<%# Bind("baseconvrate") %>' />
                                                             <%-- End --%>
                                       
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="1%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Doc No">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblDoNo" runat="server" Text='<%# Bind("DocNo") %>' CssClass="filed_input"></asp:Label>--%>
                                                            <asp:Label ID="lblDoNoView" runat="server" Text='<%# Bind("DocNo1") %>' CssClass="filed_input"></asp:Label>
                                                            <asp:Label ID="lblDoNo" runat="server" Text='<%# Bind("DocNo") %>' style="display:none" ></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Doc Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocType" runat="server" Text='<%# Bind("Type") %>' CssClass="field_input"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="4%" />
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="field1" HeaderText="Reference no">
                                                        <HeaderStyle Width="4%" />
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Booking No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRefNo" runat="server" Text='<%# Bind("field1") %>' CssClass="filed_input"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="Date"
                                                        HeaderText="Date">
                                                        <HeaderStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="DueDate"
                                                        HeaderText="Due Date">
                                                        <HeaderStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Exchg Rate">
                                                        <ItemTemplate>
                                                            &nbsp;<input id="txtCurRate" runat="server" class="field_input" maxlength="12" style="width: 40px;
                                                                text-align: right;" type="text" value='<%#Bind("currrate")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Amount">
                                                        <ItemTemplate>
                                                            <input id="txtBalAmount" runat="server" class="field_input" maxlength="12" style="width: 75px;
                                                                text-align: right;" type="text" value='<%#Bind("amount")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount To Adjust">
                                                        <EditItemTemplate>
                                                            &nbsp;
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <input id="txtAdujustAmt" runat="server" class="field_input" maxlength="12" style="width: 75px;
                                                                text-align: right;" type="text" readonly="readOnly" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Amount">
                                                        <ItemTemplate>
                                                            <input id="txtBaseAmount" runat="server" class="field_input" maxlength="12" style="width: 75px;
                                                                text-align: right;" type="text" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            &nbsp;
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Field 2">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltktno" runat="server" Text='<%# Bind("field2") %>' CssClass="filed_input"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="field3" HeaderText="Field 3">
                                                        <HeaderStyle Width="5%" />
                                                        <ControlStyle Width="5%" />
                                                        <ItemStyle Width="5%" />
                                                        <FooterStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="field4" HeaderText="Field 4">
                                                        <HeaderStyle Width="5%" />
                                                        <ControlStyle Width="5%" />
                                                        <ItemStyle Width="5%" />
                                                        <FooterStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="field5" HeaderText="Field 5">
                                                        <HeaderStyle Width="5%" />
                                                        <ControlStyle Width="5%" />
                                                        <ItemStyle Width="5%" />
                                                        <FooterStyle Width="5%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle BackColor="White" ForeColor="Black" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                                    <HeaderTemplate>
                                        Advanced Payment
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="container" style="height: 300px">
                                            <asp:GridView ID="grdAdPay" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                Font-Size="10px" GridLines="Vertical" TabIndex="2" Width="915px">
                                                <AlternatingRowStyle BackColor="#DDD9CF" Font-Size="10px" />
                                                <Columns>
                                                    <asp:BoundField DataField="LineNo" HeaderText="LineNo" Visible="False" />
                                                    <asp:TemplateField HeaderText="Doc No">
                                                        <ItemTemplate>
                                                            <input id="txtADocNo" class="field_input" maxlength="20" style="width: 60px" type="text"
                                                                runat="server" />
                                                                 <asp:HiddenField ID="hdnADocNo" runat="server" />  <%--Sharfudeen 31/07/2023--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDate" runat="server" CssClass="fiel_input" ValidationGroup="MKE"
                                                                Width="68px"></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <cc1:MaskedEditValidator ID="MaskEdValidDate" runat="server" ControlExtender="MskEdExtendDate"
                                                                ControlToValidate="txtDate" CssClass="field_error" Display="Dynamic" Height="16px"
                                                                InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                                ValidationGroup="MSK" Width="1px"></cc1:MaskedEditValidator>
                                                            <cc1:CalendarExtender ID="CalExDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnDt"
                                                                TargetControlID="txtDate">
                                                            </cc1:CalendarExtender>
                                                            <cc1:MaskedEditExtender ID="MskEdExtendDate" runat="server" AcceptNegative="Left"
                                                                DisplayMoney="Left" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDate">
                                                            </cc1:MaskedEditExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Doc Type">
                                                        <ItemTemplate>
                                                            <input id="txtDocType" class="field_input" maxlength="10" style="width: 39px" type="text"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="4%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reference no">
                                                        <ItemTemplate>
                                                            <input id="txtRefNo" class="field_input" maxlength="100" style="width: 55px" type="text"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDueDate" runat="server" CssClass="fiel_input" Width="68px"></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnDueDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <cc1:MaskedEditValidator ID="MaskEdValidDueDate" runat="server" ControlExtender="MskEdExtendDueDate"
                                                                ControlToValidate="txtDueDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MSk"></cc1:MaskedEditValidator>
                                                            <cc1:CalendarExtender ID="CalExDueDt" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnDueDate"
                                                                TargetControlID="txtDueDate">
                                                            </cc1:CalendarExtender>
                                                            <cc1:MaskedEditExtender ID="MskEdExtendDueDate" runat="server" AcceptNegative="Left"
                                                                DisplayMoney="Left" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDueDate">
                                                            </cc1:MaskedEditExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Advanced Payment Amount ">
                                                        <ItemTemplate>
                                                            <input id="txtAdvPayAmount" class="field_input" maxlength="12" style="width: 77px;
                                                                text-align: right;" type="text" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Amount">
                                                        <ItemTemplate>
                                                            <input id="txtBaseAmount" class="field_input" maxlength="12" style="text-align: right;
                                                                width: 87px;" type="text" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Field 2">
                                                        <ItemTemplate>
                                                            <input id="txtField2" class="field_input" maxlength="100" style="width: 60px" type="text"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Field 3">
                                                        <ItemTemplate>
                                                            <input id="txtField3" class="field_input" maxlength="100" style="width: 60px" type="text"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Field 4">
                                                        <ItemTemplate>
                                                            <input id="txtfield4" class="field_input" maxlength="100" style="width: 71px" type="text"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Field 5">
                                                        <ItemTemplate>
                                                            <input id="txtField5" class="field_input" maxlength="100" style="width: 60px" type="text"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="field_input" Width="73px" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="2%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                                                <HeaderStyle BackColor="#06788B" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                <RowStyle BackColor="White" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                        </div>
                                        <div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="field_button" Font-Bold="True" OnClick="btnAdd_Click"
                                                            TabIndex="3" Text="Add Row" CausesValidation="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDelLine" runat="server" CausesValidation="False" CssClass="field_button"
                                                            OnClick="btnDelLine_Click" TabIndex="4" Text="Clear" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            
                            

                                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                                    <HeaderTemplate>
                                        Future Bookings
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        &nbsp;<div class="container" style="height: 300px">
                                            <asp:GridView ID="grdFutureBooking" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                Font-Size="10px" GridLines="Vertical" TabIndex="1" Width="915px">
                                                <HeaderStyle BackColor="#06788B" Font-Bold="True" ForeColor="White" />
                                                <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="#DDD9CF" Font-Size="10px" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="LineNo1" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAccTranLineNo1" runat="server" Text='<%# Bind("acc_tran_lineno") %>'
                                                                Width="57px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            &nbsp;
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <input id="chkBill1" runat="server" type="checkbox" />
                                                            <asp:HiddenField ID="hdnCurrCode1" runat="server" Value='<%# Bind("currcode") %>' />
                                                                          <%-- Added by Ram --%>
                                                            <asp:HiddenField ID="hdnbaseconrate1" runat="server" Value='<%# Bind("baseconvrate") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="1%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Doc No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoNo1" runat="server" Text='<%# Bind("DocNo") %>' CssClass="filed_input"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Doc Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocType1" runat="server" Text='<%# Bind("Type") %>' CssClass="field_input"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="4%" />
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="field11" HeaderText="Reference no">
                                                        <HeaderStyle Width="4%" />
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Booking No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRefNo1" runat="server" Text='<%# Bind("field1") %>' CssClass="filed_input"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="Date"
                                                        HeaderText="Date">
                                                        <HeaderStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="DueDate"
                                                        HeaderText="Due Date">
                                                        <HeaderStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Exchg Rate">
                                                        <ItemTemplate>
                                                            &nbsp;<input id="txtCurRate1" runat="server" class="field_input" maxlength="12" style="width: 40px;
                                                                text-align: right;" type="text" value='<%#Bind("currrate")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Amount">
                                                        <ItemTemplate>
                                                            <input id="txtBalAmount1" runat="server" class="field_input" maxlength="12" style="width: 75px;
                                                                text-align: right;" type="text" value='<%#Bind("amount")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount To Adjust">
                                                        <EditItemTemplate>
                                                            &nbsp;
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <input id="txtAdujustAmt1" runat="server" class="field_input" maxlength="12" style="width: 75px;
                                                                text-align: right;" type="text" readonly="readOnly" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Amount">
                                                        <ItemTemplate>
                                                            <input id="txtBaseAmount1" runat="server" class="field_input" maxlength="12" style="width: 75px;
                                                                text-align: right;" type="text" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            &nbsp;
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Field 2">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltktno1" runat="server" Text='<%# Bind("field2") %>' CssClass="filed_input"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="field3" HeaderText="Field 3">
                                                        <HeaderStyle Width="5%" />
                                                        <ControlStyle Width="5%" />
                                                        <ItemStyle Width="5%" />
                                                        <FooterStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="field4" HeaderText="Field 4">
                                                        <HeaderStyle Width="5%" />
                                                        <ControlStyle Width="5%" />
                                                        <ItemStyle Width="5%" />
                                                        <FooterStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="field5" HeaderText="Field 5">
                                                        <HeaderStyle Width="5%" />
                                                        <ControlStyle Width="5%" />
                                                        <ItemStyle Width="5%" />
                                                        <FooterStyle Width="5%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle BackColor="White" ForeColor="Black" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>


                            
                            
                            
                            
                            
                            
                            </cc1:TabContainer>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 195px">
                                    </td>
                                    <td style="width: 313px">
                                    </td>
                                    <td style="width: 322px">
                                        <asp:HiddenField ID="hdnreqid" runat="server" />
                                    </td>
                                    <td style="width: 347px">
                                        <asp:HiddenField ID="hdnchkid" runat="server" />
                                    </td>
                                    <td style="width: 167px">
                                    </td>
                                    <td style="width: 100px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 195px; height: 34px">
                                        <asp:Label ID="lblAmountAdjusted" runat="server" CssClass="field_caption" Text=" Amount Adjusted"
                                            Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 313px; height: 34px">
                                        <asp:Label ID="lblBaseAmountAdj" runat="server" CssClass="field_caption" Text=" Amount Adjusted"
                                            Width="144px"></asp:Label>
                                    </td>
                                    <td style="width: 322px; height: 34px">
                                        <asp:Label ID="lblBalancetoAdjust" runat="server" CssClass="field_caption" Text=" Balance to Adjust"
                                            Width="144px"></asp:Label>
                                    </td>
                                    <td style="width: 347px; height: 34px">
                                        <asp:Label ID="lblBaseAmtBalance" runat="server" CssClass="field_caption" Text=" Balance to Adjust"
                                            Width="144px"></asp:Label>
                                    </td>
                                    <td style="width: 167px; height: 34px">
                                    </td>
                                    <td style="width: 100px; height: 34px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 195px; height: 21px">
                                        <input id="txtAmountAdjust" runat="server" class="field_input" type="text" disabled="disabled" />
                                    </td>
                                    <td style="width: 313px; height: 21px">
                                        <input id="txtBaseAmountAdjust" runat="server" class="field_input" type="text" disabled="disabled"
                                            value="0" />
                                    </td>
                                    <td style="width: 322px; height: 21px">
                                        <input id="txtBalAdu" runat="server" class="field_input" type="text" disabled="disabled" />
                                    </td>
                                    <td style="width: 347px; height: 21px">
                                        <input id="txtBalAduinBase" runat="server" class="field_input" type="text" disabled="disabled"
                                            value="0" />
                                    </td>
                                    <td style="width: 167px; height: 21px">
                                    </td>
                                    <td style="width: 100px; height: 21px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 195px; height: 29px">
                                        <input id="txtflag" runat="server" style="visibility: hidden; width: 24px" type="text" />&nbsp;
                                        <input id="txtdecimal" runat="server" maxlength="15" style="visibility: hidden; width: 33px"
                                            type="text" />
                                    </td>
                                    <td style="width: 313px; height: 29px">
                                        <input id="txtgrdAdjRows" runat="server" style="visibility: hidden; width: 18px"
                                            type="text" />
                                        <input id="txtgrdAdjRows1" runat="server" style="visibility: hidden; width: 18px"
                                            type="text" />
                                            
                                            &nbsp;
                                        <input id="txtGrdType" runat="server" style="visibility: hidden; width: 18px" type="text" />
                                    </td>
                                    <td style="width: 322px; height: 29px">
                                        <input id="txtgrdAdpayRows" runat="server" style="visibility: hidden; width: 117px"
                                            type="text" />
                                    </td>
                                    <td style="width: 347px; height: 29px">
                                        <asp:Button ID="btnOk" runat="server" CssClass="field_button" Font-Bold="True" TabIndex="5"
                                            Text="Ok" />&nbsp;
                                        <asp:Button ID="btnAExit" OnClick="btnAExit_Click" runat="server" CssClass="field_button"
                                            Font-Bold="True" TabIndex="6" Text="Exit" />
                                    </td>
                                    <td style="width: 167px; height: 29px">
                                         <asp:Button ID="btnRecalculate" width="1px" style="visibility:hidden" runat="server" CssClass="field_button"
                                            Font-Bold="True" TabIndex="6" Text="Recalculate" />
                                    </td>
                                    <td style="width: 100px; height: 29px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;<div style="width: 973px; height: 150px; display: none;" class="container">
                                &nbsp;<asp:GridView ID="GVCollection" runat="server" AutoGenerateColumns="False"
                                    CssClass="grdstyle" Height="33px" Width="400px" OnRowDataBound="GVCollection_RowDataBound">
                                    <RowStyle BackColor="white" ForeColor="Black" Wrap="False" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSel" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Request no">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestno" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "requestid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Guest Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblguestname" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "guestname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblamount" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "amount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle Wrap="False" />
                                    <HeaderStyle BackColor="#06788B" BorderStyle="None" ForeColor="White" Wrap="False" />
                                    <AlternatingRowStyle BackColor="#DDD9CF" CssClass="grdAternaterow" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                   <input id="txtSealDate" type="text" runat="server" style="visibility:hidden" />
                  <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
        <asp:HiddenField ID="hdnadvancepayment" runat="server"  /> <%--Sharfudeen 29/07/2023--%>
         <asp:HiddenField ID="hdnadvancelastno" runat="server"  /> <%--Sharfudeen 31/07/2023--%>

          <asp:Button ID="btnadvance" width="1px" style="visibility:display" runat="server" CssClass="field_button"
                                            Font-Bold="True" TabIndex="6" Text="advance" />

                <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                    <Services>
                        <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                    </Services>
                </asp:ScriptManagerProxy>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        OnchangeChk_checked()   
    </script>
    </form>
</body>
</html>
