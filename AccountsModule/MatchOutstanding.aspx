<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MatchOutstanding.aspx.vb" Inherits="MatchOutstanding" MasterPageFile= "~/SubPageMaster.master" strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />
<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript">
var nodecround =null;
function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g,"");	
}
function DecRoundABS(amtToRound) 
{
  
  var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
  nodecround=Math.pow(10,parseInt(txtdec.value));
  //var amtToRound=Number(amtToRound1);
  var rdamt=Math.round( parseFloat( Number(amtToRound)  ) *nodecround)/nodecround;
  return  parseFloat(Math.abs(rdamt));
}

function DecRound(amtToRound) 
{
  
  var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
  nodecround=Math.pow(10,parseInt(txtdec.value));
  //var amtToRound=Number(amtToRound1);
  var rdamt=Math.round( parseFloat( Number(amtToRound)  ) *nodecround)/nodecround;
  return  parseFloat(rdamt);
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
function DecRound1(amtToRound) {
    nodecround = Math.pow(10, 3);
    var rdamt = Math.round(parseFloat(amtToRound) * nodecround) / nodecround;

    return parseFloat(rdamt);
}
function chkTextLock(evt)
	{
         return false;
 	}
	function chkTextLock1(evt)
	{
       if ( evt.keyCode =9 )
        { 
         return true;
        }
        else
        {
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
function checkNumberDecimal(evt,txt) 
    {
      
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
        if (charCode != 47 && (charCode > 44 && charCode < 58)) { 
           var value=txt.value;
           var indx=value.indexOf('.');
           var deci=document.getElementById("<%=txtdecimal.ClientID%>");
           var lngLenght =deci.value;  
           if (indx < 0 ){
           return true;
           }
           
           var digit=value.substring(indx+1);
           if(digit.length>lngLenght-1)
            {
            return false;
                }
            else
            {
            return true;
             }
       }
       return false;
    }


function FormValidation(state)
{
     var img =document.getElementById("<%=imgicon.ClientID %>");
     var hdnSS=document.getElementById("<%=hdnSS.ClientID%>");

      if (document.getElementById("<%=txtDate.ClientID%>").value=="")
	    {
            document.getElementById("<%=txtDate.ClientID%>").focus(); 
            alert("Enter Posting Date");
            return false;
        }
      else if (document.getElementById("<%=ddlType.ClientID%>").value=="[Select]")
	    {
            document.getElementById("<%=ddlType.ClientID%>").focus(); 
            alert("Select Account Type");
            return false;
        }
        else if (document.getElementById("<%=TxtCustCode_auto.ClientID%>").value == "") {
            document.getElementById("<%=TxtcustName_auto.ClientID%>").value == ""
	        document.getElementById("<%=TxtCustCode_auto.ClientID%>").focus(); 

            alert("Select Account");
            return false;
        }
     
      else if (document.getElementById("<%=txtNarration.ClientID%>").value=="") 
	    {
           document.getElementById("<%=txtNarration.ClientID%>").focus();
           alert("Enter Narration");
            return false;
        }
      else if (Number(document.getElementById("<%=txtConversion.ClientID%>").value)==0) 
	    {
           document.getElementById("<%=txtConversion.ClientID%>").focus();
           alert("Conversion Rate can not be 0");
            return false;
        }
      else
        {
           //alert(state);
           if (state=='New'){if(confirm('Are you sure you want to save ?')==false)return false;}
           //if (state=='Edit'){if(confirm('Are you sure you want to update ?')==false)return false;}
           if (state=='Delete'){if(confirm('Are you sure you want to delete ?')==false)return false;}
           if (state=='Cancel'){if(confirm('Are you sure you want to Cancel ?')==false)return false;}
           img.style.visibility="visible";   
        }
        
     hdnSS.value=0;           
     validate_click();
 }

 function exchautocompleteselected(source, eventArgs) {

     if (eventArgs != null) {

         document.getElementById('<%=TxtExchCode_auto.ClientID%>').value = eventArgs.get_value();
        



     }
     else {
         document.getElementById('<%=TxtExchCode_auto.ClientID%>').value = '';


     }


 }
function custautocompleteselected(source, eventArgs) {

    if (eventArgs != null) {

        document.getElementById('<%=TxtCustCode_auto.ClientID%>').value = eventArgs.get_value();
        var ddltyp1 = document.getElementById('<%=ddlType.ClientID%>');
        var typ = ddltyp1.options[ddltyp1.selectedIndex].value;

        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value;
        codeid = eventArgs.get_value();
        var crdsqlstr = "select cur,currrates.convrate,controlacctcode ,acctmast.acctname,currmast.currname   from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457)  left join  acctmast on view_account.controlacctcode =acctmast.acctcode   left join  currmast on view_account.cur=currmast.currcode where code = '" + codeid + "' and type='" + typ + "' ";
        ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 5, FiilCustDt, ErrorHandler, TimeOutHandler);




    }
    else {
        document.getElementById('<%=TxtCustCode_auto.ClientID%>').value = '';

     
    }

   
}


    function FillCustDDL(ddltp, lblcustcd) {
        ddltyp = document.getElementById(ddltp);


        lblcustcode = document.getElementById(lblcustcd);

        var strcap = ddltyp.options[ddltyp.selectedIndex].text;
        var strtp = ddltyp.value;

        if (ddltyp.value == '[Select]') {
            lblcustcode.innerHTML = '<font color="Red"> *</font>';


        }
        else {
            lblcustcode.innerHTML = strcap + ' <font color="Red"> *</font>';






       }



    }

   






function FiilCustDt(result)
    {

        var txtcurrcode = document.getElementById("<%=txtCurrency.ClientID%>");
        var txtcurrname = document.getElementById("<%=txtCurrencyname.ClientID%>");

    var txtconv = document.getElementById("<%=txtConversion.ClientID%>");



   	var ddlconcode = document.getElementById("<%=txtControlacct.ClientID%>");
   	var ddlconname = document.getElementById("<%=txtControlacctname.ClientID%>");


   
    txtcurrcode.value = result[0];
    txtconv.value =result[1];
    ddlconcode.value = result[2];
    ddlconname.value = result[3];
    txtcurrname.value = result[4];


    
    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
    
    if ( trim(txtcurrcode.value)==trim(txtbase.value))
       {
        txtconv.readOnly=true; 
        txtconv.disabled=true;
       }
       else
       {
        txtconv.readOnly=false;
        txtconv.disabled=false;
       }
   } 
 
 
//Match Outstanding Grid Details
function FillCrDr(chksel,txtcrval,txtdrval,txtbal,txtbasecredit,txtbasedebit,rate)
{
   
    chsel = document.getElementById(chksel);
    txtcr = document.getElementById(txtcrval);
    txtdr = document.getElementById(txtdrval);
    txtbalamt = document.getElementById(txtbal);
    txtbasecr = document.getElementById(txtbasecredit);
    txtbasedr = document.getElementById(txtbasedebit);
   
    var txtconvrate = document.getElementById(rate);
    var ddltyp = document.getElementById("<%=ddlType.ClientID%>");
    var typ = ddltyp.options[ddltyp.selectedIndex].value;
    var txtCtot = document.getElementById("<%=txtCreditTotal.ClientID%>");
    var txtDtot = document.getElementById("<%=txtDebitTotal.ClientID%>");
    var txtBaseCtot = document.getElementById("<%=txtBaseCreditTotal.ClientID%>");
    var txtBaseDtot = document.getElementById("<%=txtBaseDebitTotal.ClientID%>");
    
    var txtExchCtot = document.getElementById("<%=exchcredit.ClientID%>");
    var txtExchDtot = document.getElementById("<%=exchdebit.ClientID%>");

    
    var Ctot = txtCtot.value;
    var Dtot = txtDtot.value;
    txtcr.readOnly=false;
    txtdr.readOnly=false;
    txtcr.disabled=true;    
    txtdr.disabled=true;
    if (chsel.checked == true)
    {
     
        if (typ=='C')
          {
            if ((Number(txtbalamt.value) > 0))
               { 
                txtcr.value = DecRound(txtbalamt.value);
                txtdr.value =0;
                txtcr.readOnly=false;
                txtdr.readOnly=true;
                txtcr.disabled=false;
                txtdr.disabled=true;                    
                }   
                else
                {
                    txtcr.value = 0;
                    txtdr.value = DecRoundABS(txtbalamt.value);
                    txtcr.readOnly=true;
                    txtdr.readOnly=false;
                    txtcr.disabled=true;    
                    txtdr.disabled=false;
                 }
          
         } 
       else if (typ=='S' || typ=='A')
       {
            if ((Number(txtbalamt.value) > 0))
            {
               txtdr.value =  DecRound(txtbalamt.value);
               txtcr.value =0;
               txtcr.readOnly=true;
               txtdr.readOnly=false;
               txtcr.disabled=true;    
               txtdr.disabled=false;
             }
             else
             {
              txtcr.value = DecRoundABS(txtbalamt.value);
              txtdr.value = 0;
              txtcr.readOnly=false;
              txtdr.readOnly=true;
              txtcr.disabled=false;    
              txtdr.disabled=true;
            } 
         } 
        else
          {
          txtdr.value = 0;
          txtcr.value = 0;
          txtcr.readOnly=true;
          txtdr.readOnly=true;
          txtcr.disabled=true;    
          txtdr.disabled=true;
          }
//           alert(txtCtot.value);
//           if (txtCtot.value == '') {txtCtot.value=0;}
//           if (isNaN(txtCtot.value)==true) {txtCtot.value=0;}
//           if (txtDtot.value=='') {txtDtot.value=0;}
//           if (isNaN(txtDtot.value)==true) {txtDtot.value=0;} 
 
           
////           alert(Number(txtDtot.value));
//           txtCtot.value = DecRound( DecRound(txtCtot.value) + DecRound(txtcr.value) );
//           txtDtot.value =DecRound( DecRound(txtDtot.value) +  DecRound(txtdr.value) );
//           txtbasecr.value=DecRound(DecRound(txtconvrate.value) * DecRound(txtcr.value));     
//           txtbasedr.value=DecRound(DecRound(txtconvrate.value) * DecRound(txtdr.value));
//           txtBaseCtot.value=DecRound( DecRound(txtBaseCtot.value) + DecRound(txtbasecr.value) );
//           txtBaseDtot.value=DecRound( DecRound(txtBaseDtot.value) + DecRound(txtbasedr.value) );
                
     }
    else
    {
//       txtCtot.value = DecRound( DecRound(txtCtot.value) - DecRound(txtcr.value));
//       txtDtot.value =DecRound (  DecRound(txtDtot.value) - DecRound(txtdr.value));
//       txtBaseCtot.value=DecRound( DecRound(txtBaseCtot.value) - DecRound(txtbasecr.value) );
//       txtBaseDtot.value=DecRound( DecRound(txtBaseDtot.value) - DecRound(txtbasedr.value) );
 
       txtdr.value = 0;
       txtcr.value = 0;
       txtbasecr.value=0;     
       txtbasedr.value=0;
       txtcr.disabled=true;    
       txtdr.disabled=true;
    }
         
           txtbasecr.value=DecRound(parseFloat(txtconvrate.value) * DecRound(txtcr.value));     
           txtbasedr.value=DecRound(parseFloat(txtconvrate.value) * DecRound(txtdr.value));

    GrdTotal();
            
}
function ChangeCr(chksel,txtcrval,txtdrval,txtbal,txtbasecredit,txtbasedebit,rate)
{

     
    chsel = document.getElementById(chksel);
    txtcr = document.getElementById(txtcrval);
    txtdr = document.getElementById(txtdrval);
    txtbalamt = document.getElementById(txtbal);
    txtbasecr = document.getElementById(txtbasecredit);
    txtbasedr = document.getElementById(txtbasedebit);
    var txtconvrate = document.getElementById(rate); 
   if (chsel.checked==true)
    {     
      
      
    if ( DecRound(txtcr.value)==0 ) 
     {
       alert('Credit amount should not be  Zero');
        txtcr.focus();
      }
    else if ( DecRound(txtbalamt.value) < DecRound(txtcr.value)) 
    {
        alert('Credit amount should not be greater than Balance amount.');
        txtcr.focus();
        txtcr.value=DecRound(txtbalamt.value);
        txtdr.value=0;
        //txtbasecr.value=DecRound(DecRound(txtconvrate.value) * DecRound(txtbalamt.value));;;
    }
    
    }
   else
   {
    txtcr.value=0;
    txtdr.value=0;
    txtbasecr.value=0;
    txtbasedr.value=0;
   }
  txtbasecr.value=DecRound(parseFloat(txtconvrate.value) * DecRound(txtcr.value));     
  txtbasedr.value=DecRound(parseFloat(txtconvrate.value) * DecRound(txtdr.value));
     GrdTotal();
}
function ChangeDr(chksel,txtcrval,txtdrval,txtbal,txtbasecredit,txtbasedebit,rate)
{
    chsel = document.getElementById(chksel);
    txtcr = document.getElementById(txtcrval);
    txtdr = document.getElementById(txtdrval);
    txtbalamt = document.getElementById(txtbal);
    txtbasecr = document.getElementById(txtbasecredit);
    txtbasedr = document.getElementById(txtbasedebit);
    var txtconvrate = document.getElementById(rate); 
     
    if (chsel.checked==true)
    {     
   
     if (DecRound(txtdr.value)==0 ) 
       {
         alert('Debit amount should not be  Zero');
         txtcr.focus();
        }
      else if ( DecRoundABS(txtbalamt.value) < DecRoundABS(txtdr.value) ) 
      {
        alert('Debit amount should not be greater than Balance amount.');
        txtdr.value= DecRoundABS(txtbalamt.value);
       // txtbasedr.value=DecRound(DecRound(txtconvrate.value) * DecRoundABS(txtbalamt.value) );;
         txtcr.value=0;
        txtdr.focus();
      }
   }
   else
   {
    txtcr.value=0;
    txtdr.value=0;
    txtbasecr.value=0;
    txtbasedr.value=0;
   }
   
  txtbasecr.value=DecRound(parseFloat(txtconvrate.value) * DecRound(txtcr.value));     
  txtbasedr.value=DecRound(parseFloat(txtconvrate.value) * DecRound(txtdr.value));
     GrdTotal();
}    
//    else if (Number(tbal) < (Number(dramt) + Number(cramt)))
//    {
//         alert('Sum of Debit and Credit amount should not be greater than Balance amount.');
//         txtdramt.focus();
//         
//    }
//    else if ((Number(txtdr.value) == 0) && (Number(txtcr.value) == 0))
//    {
//         alert('You should not be enter Debit and Credit amount greater than zero for same row.');
//         txtdramt.focus();
//    }
//    else
//    {
//    
//    alert('test');
//        var objGridView = document.getElementById('<%=grdMatchOut.ClientID%>');
//        var totCr = 0 ;
//        var totDr = 0 ;
//        var totBaseCr = 0 ;
//        var totBaseDr = 0 ;
//        
//        var j=0;
//        var txtrowcnt =document.getElementById('<%=txtgridrows.ClientID%>');
//        intRows=txtrowcnt.value;
//        
//        for(j=1;j<=intRows;j++)
//        {
//            var valDr=objGridView.rows[j].cells[4].children[0].value;
//            var valCr=objGridView.rows[j].cells[5].children[0].value;
//            var chk=objGridView.rows[j].cells[6].children[0];
//            var valBaseDr=objGridView.rows[j].cells[8].children[0].value;
//            var valBaseCr=objGridView.rows[j].cells[9].children[0].value;
////alert(chk.checked);
//            if (chk.checked==true)
//            {
//                if (valDr==''){valDr=0;}
//                if (valCr==''){valCr=0;}
//                if (isNaN(valDr)==true){valDr=0;}
//                if (isNaN(valCr)==true){valCr=0;}

//                totDr=DecRound(totDr)+DecRound(valDr);
//                totCr=DecRound(totCr)+DecRound(valCr);
//                totBaseDr=DecRound(totBaseDr)+DecRound(valBaseDr);
//                totBaseCr=DecRound(totBaseCr)+DecRound(valBaseCr);
//            }
//            

//        }
//        var txttotDr = document.getElementById('<%=txtDebitTotal.ClientID%>');
//        var txttotCr = document.getElementById('<%=txtCreditTotal.ClientID%>');
//        var txtBaseCtot = document.getElementById("<%=txtBaseCreditTotal.ClientID%>");
//        var txtBaseDtot = document.getElementById("<%=txtBaseDebitTotal.ClientID%>");

//        txttotDr.value=DecRound(totDr);
//        txttotCr.value=DecRound(totCr);
//        txtBaseDtot.value=DecRound(totBaseDr);
//        txtBaseCtot.value=DecRound(totBaseCr);
//        
//    }
//}
function GrdTotal()
{
       var objGridView = document.getElementById('<%=grdMatchOut.ClientID%>');
        var totCr = 0 ;
        var totDr = 0 ;
        var totBaseCr = 0 ;
        var totBaseDr = 0 ;
        
        var j=0;
        var txtrowcnt =document.getElementById('<%=txtgridrows.ClientID%>');
        intRows=txtrowcnt.value;
     
        for(j=1;j<=intRows;j++) {
            
            var valDr=objGridView.rows[j].cells[8].children[0].value;
            var valCr=objGridView.rows[j].cells[9].children[0].value;
            var chk=objGridView.rows[j].cells[10].children[0];
            var valBaseDr=objGridView.rows[j].cells[12].children[0].value;
            var valBaseCr = objGridView.rows[j].cells[13].children[0].value;
             
            if (chk.checked==true)
            {
                if (valDr==''){valDr=0;}
                if (valCr==''){valCr=0;}
                if (isNaN(valDr)==true){valDr=0;}
                if (isNaN(valCr)==true){valCr=0;}

                totDr=DecRound(totDr)+DecRound(valDr);
                totCr=DecRound(totCr)+DecRound(valCr);
                totBaseDr=DecRound(totBaseDr)+DecRound(valBaseDr);
                totBaseCr=DecRound(totBaseCr)+DecRound(valBaseCr);
            }
            

        }
        var txttotDr = document.getElementById('<%=txtDebitTotal.ClientID%>');
        var txttotCr = document.getElementById('<%=txtCreditTotal.ClientID%>');
        var txtBaseCtot = document.getElementById("<%=txtBaseCreditTotal.ClientID%>");
        var txtBaseDtot = document.getElementById("<%=txtBaseDebitTotal.ClientID%>");

        var txtExchCtot = document.getElementById("<%=exchcredit.ClientID%>");
        var txtExchDtot = document.getElementById("<%=exchdebit.ClientID%>");


        txttotDr.value=DecRound(totDr);
        txttotCr.value=DecRound(totCr);
        txtBaseDtot.value=DecRound(totBaseDr);
        txtBaseCtot.value=DecRound(totBaseCr);
        
//Exchange difference        
        if (DecRound(txtBaseDtot.value)>DecRound(txtBaseCtot.value))
        {
            txtExchCtot.value=DecRound(txtBaseDtot.value-txtBaseCtot.value);
            txtExchDtot.value=0;
        }
        else if (DecRound(txtBaseCtot.value)>DecRound(txtBaseDtot.value))
        {
            txtExchDtot.value=DecRound(txtBaseCtot.value-txtBaseDtot.value);
            txtExchCtot.value=0;
        }
        else
        {
           txtExchDtot.value=0;
           txtExchCtot.value=0;
        }

        
}
//function FillCrDr(chksel,txtcrval,txtdrval,txtbal)
//{
//    chsel = document.getElementById(chksel);
//    txtcr = document.getElementById(txtcrval);
//    txtdr = document.getElementById(txtdrval);
//    var tbal = txtbal;
//  
//    var ddltyp = document.getElementById("<%=ddlType.ClientID%>");
//    var typ = ddltyp.options[ddltyp.selectedIndex].value;
//    var txtCtot = document.getElementById("<%=txtCreditTotal.ClientID%>");
//    var Ctot = txtCtot.value;
//    var txtDtot = document.getElementById("<%=txtDebitTotal.ClientID%>");
//    var Dtot = txtDtot.value;
//     
//    if (chsel.checked == true)
//    {
//    
//       if (typ=='C')
//       {
//          if ((Number(tbal) > 0))
//          { 
//            txtcr.value = Number(tbal).toFixed(2);
//            txtdr.value = Number(0).toFixed(2);
//            
//          }   
//          else
//          {
//            txtdr.value = Math.abs(Number(tbal)).toFixed(2);
//            txtcr.value = Number(0).toFixed(2);
//          }
//          
//       } 
//       else if (typ=='S' || typ=='A')
//       {
//          if ((Number(tbal) > 0))
//          {
//            txtdr.value = Number(tbal).toFixed(2);
//            txtcr.value = Number(0).toFixed(2);
//          }
//          else
//          {
//            txtcr.value = Math.abs(Number(tbal)).toFixed(2);
//            txtdr.value = Number(0).toFixed(2);
//          } 
//       } 
//       else
//       {
//          txtdr.value = Number(0).toFixed(2);
//          txtcr.value = Number(0).toFixed(2);  
//       }
//       txtCtot.value = (Number(Ctot) + Number(txtcr.value)).toFixed(2);
//       txtDtot.value = (Number(Dtot) + Number(txtdr.value)).toFixed(2);
//    }
//    else
//    {
//       txtCtot.value = (Number(Ctot) - Number(txtcr.value)).toFixed(2);
//       txtDtot.value = (Number(Dtot) - Number(txtdr.value)).toFixed(2);
//       
//       txtdr.value = Number(0).toFixed(2);
//       txtcr.value = Number(0).toFixed(2);
//    }
//            
//}

//------------- Sub Total Calculation

//function grdTotal(cksel,tcval,tdval,tbal)
//{
// 
// chsel = document.getElementById(cksel);
// txtcramt = document.getElementById(tcval);
// txtdramt = document.getElementById(tdval);
// var cramt = txtcramt.value;
// var dramt = txtdramt.value;
//    
//    if ((Number(tbal) < Number(dramt)) || (Number(tbal) < Number(cramt)))
//    {
//        alert('Debit / Credit amount should not be greater than Balance amount.');
//        txtdramt.focus();
//       
//    }
//    else if (Number(tbal) < (Number(dramt) + Number(cramt)))
//    {
//         alert('Sum of Debit and Credit amount should not be greater than Balance amount.');
//         txtdramt.focus();
//         
//    }
//    else if ((Number(dramt) != 0) && (Number(cramt) != 0))
//    {
//         alert('You should not be enter Debit and Credit amount greater than zero for same row.');
//         txtdramt.focus();
//    }
//    else
//    {
//    
//        var objGridView = document.getElementById('<%=grdMatchOut.ClientID%>');
//        var totCr = 0 ;
//        var totDr = 0 ;
//        var j=0;
//        var txtrowcnt =document.getElementById('<%=txtgridrows.ClientID%>');
//        intRows=txtrowcnt.value;
//        
//        for(j=1;j<=intRows;j++)
//        {
//            var valDr=objGridView.rows[j].cells[4].children[0].value;
//            var valCr=objGridView.rows[j].cells[5].children[0].value;
//            var chk=objGridView.rows[j].cells[6].children[0];
////alert(chk.checked);
//            if (chk.checked==true)
//            {
//                if (valDr==''){valDr=0;}
//                if (valCr==''){valCr=0;}
//                if (isNaN(valDr)==true){valDr=0;}
//                if (isNaN(valCr)==true){valCr=0;}


//        //        alert('A');
//                totDr=parseFloat(totDr)+parseFloat(valDr);
//                totCr=parseFloat(totCr)+parseFloat(valCr);
//            }
//            

//        }
//        var txttotDr = document.getElementById('<%=txtDebitTotal.ClientID%>');
//        var txttotCr = document.getElementById('<%=txtCreditTotal.ClientID%>');

//        txttotDr.value=totDr.toFixed(2);
//        txttotCr.value=totCr.toFixed(2);
//    }
//}

//-----------------------------------

 //---------------------------------------------------------------------------------------    
 
//-----    Common
    
    function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }


 function validate_click()
{
    var hdnSS=document.getElementById("<%=hdnSS.ClientID%>");
    var btnss=document.getElementById("<%=btnsave.ClientID%>");
     
    if(hdnSS.value==0)
    {  
        hdnSS.value=1; 
       // btnss.disabled=true;
       btnss.style.visibility="hidden"; 
        return true;
    }
    else
    {   
        return false;
    }   
}  

</script>


    <asp:UpdatePanel id="UpdatePanel1" runat="server">

            <contenttemplate>
             <script type="text/javascript">
                 var prm = Sys.WebForms.PageRequestManager.getInstance();
                 prm.add_beginRequest(function () {

                 });

                 prm.add_endRequest(function () {
                     MyAutoCustomerFillArray();

                 });




                </script>

<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY>
<TR><TD class="field_heading" align=center colSpan=1>
    <asp:Label id="lblHeading" runat="server" Text="Match Outstanding" 
        Width="704px" CssClass="field_heading" Height="26px"></asp:Label></TD></TR>

<TR><TD><TABLE><TBODY><TR><TD class="td_cell"><asp:Label id="Label2" runat="server" Text="Document No." Width="110px" CssClass="field_Caption"></asp:Label></TD>


<TD colSpan=2>
<table><tr><td><asp:TextBox id="txtDocNo" runat="server" tabIndex=1 Width="194px" CssClass="field_input" ReadOnly="True" Enabled="False"></asp:TextBox></td>

<td class="td_cell"><SPAN style="COLOR: #ff0000"><SPAN style="COLOR: #000000">
<asp:Label ID="Label3" runat="server" Text=" Posting Date &lt;font color='Red'&gt; *&lt;/font&gt;" Width="110px"></asp:Label>
        </SPAN></td>
        <td><asp:TextBox id="txtDate" tabIndex=1 runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox> <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVFromDt" runat="server" Width="23px" CssClass="field_error" ControlExtender="MskFromDate" ControlToValidate="txtDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"></cc1:MaskedEditValidator></td>

</tr></table>

</TD>
        
        
        
        <TD class="td_cell">&nbsp;</TD><TD class="td_cell">
        
        
        </TD><TD class="td_cell" colSpan=1></TD></TR><TR><TD class="td_cell"><SPAN><asp:Label id="Label4" runat="server" Text="Type <font color='Red'> *</font>" Width="110px"></asp:Label></SPAN></TD><TD colSpan=2>

  <asp:DropDownList ID="ddlType" runat="server" TabIndex="2" AutoPostBack="True" Width="121px">
   <asp:ListItem Text="Supplier" Value="S"></asp:ListItem>
    <asp:ListItem Text="Customer" Value="C"></asp:ListItem>
     <asp:ListItem Text="Supplier Agent" Value="A"></asp:ListItem>
                                                       
                                                        </asp:DropDownList>
<asp:Label id="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana" Width="113px" CssClass="field_caption" BackColor="#E0E0E0" Font-Bold="True" ForeColor="Green"></asp:Label></TD><TD class="td_cell" colSpan=1></TD><TD class="td_cell" colSpan=1></TD></TR><tr><td class="td_cell">
    <asp:Label ID="lblCustCode" runat="server" 
        Text=" Supplier &lt;font color='Red'&gt; *&lt;/font&gt;" Width="110px"></asp:Label>
    </td><td>
        <asp:TextBox ID="TxtcustName_auto" runat="server" TabIndex="3" 
            AutoPostBack="false" CssClass="field_input" MaxLength="500"  Width="320px" 
             ></asp:TextBox>
        <asp:TextBox ID="TxtCustCode_auto" runat="server" Style="display: none"></asp:TextBox>
        <asp:HiddenField ID="HiddenField1" runat="server" Visible="False" />
        <asp1:AutoCompleteExtender ID="TxtCustName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" ServiceMethod="Getsupplist" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    TargetControlID="TxtCustName_auto" OnClientItemSelected="custautocompleteselected">
                                                                   </asp1:AutoCompleteExtender></td>
   <td>
        <asp:Label ID="Label12" runat="server"  Text="Control A/C"  
            CssClass="field_caption" Width="82px"></asp:Label>
              <INPUT style="WIDTH: 222px" tabIndex=4
                                id="txtControlacctname" class="field_input" readOnly type=text maxLength=1000 
                                runat="server" />
        <INPUT style="WIDTH: 194px ; " readonly="readonly" id="txtControlacct"  class="field_input"  type=text maxLength=1000 
                                runat="server" />
    </td></tr><TR><TD class="td_cell"><DIV id="dvType" class="td_cell" title="Type"><asp:Label 
            ID="Label6" runat="server" CssClass="field_Caption" Text="Currency" 
            Width="110px"></asp:Label>
        </DIV></TD><TD colSpan=2 class="td_cell">
<INPUT style="WIDTH: 194px" id="txtCurrencyname" class="field_input" tabIndex=5 readOnly type=text maxLength=1000 runat="server" />
<INPUT style="WIDTH: 194px ; display:none" id="txtCurrency" class="field_input" readOnly type=text maxLength=1000 runat="server" />
            <asp:Label ID="Label7" runat="server"   
                Text="Conv. Rate" Width="87px"></asp:Label>
            <INPUT style="WIDTH: 130px; TEXT-ALIGN: right" id="txtConversion" 
                                class="field_input" tabIndex=6 type=text maxLength=20 
                    runat="server" />
</TD><TD class="td_cell">
            &nbsp;</TD><TD class="td_cell" colSpan=2>
            &nbsp;</TD></TR><TR><TD class="td_cell"><span>
    <asp:Label ID="Label5" runat="server" 
        Text="Narration  &lt;font color='Red'&gt; *&lt;/font&gt;" Width="110px"></asp:Label>
    </span></TD><TD colSpan=5>
            <asp:TextBox ID="txtnarration" runat="server" CssClass="field_input" 
                Height="70px" MaxLength="200" TabIndex="7" TextMode="MultiLine" 
                Width="629px"></asp:TextBox>
        </TD></TR><TR><TD class="td_cell">&nbsp;</TD><TD class="td_cell" colSpan=2>
    &nbsp;</TD><TD class="td_cell">&nbsp;</TD><TD class="td_cell">&nbsp;</TD><TD class="td_cell" colSpan=1>
    &nbsp;</TD></TR>
    <tr>
        <td class="td_cell">
            From Date</td>
        <td class="td_cell" colspan="2">

        <table><tr><td><asp:TextBox ID="txtPfromdate" runat="server" CssClass="fiel_input" 
                Width="80px" tabIndex=8></asp:TextBox>
            <cc1:CalendarExtender ID="CEPFromDate" runat="server" Format="dd/MM/yyyy" 
                PopupButtonID="ImgPBtnFrmDt" TargetControlID="txtPFromDate">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MEPFromDate" runat="server" Mask="99/99/9999" 
                MaskType="Date" TargetControlID="txtPFromDate">
            </cc1:MaskedEditExtender>
            <asp:ImageButton ID="ImgPBtnFrmDt" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            <cc1:MaskedEditValidator ID="MEVPFromDate" runat="server" 
                ControlExtender="MEPFromDate" ControlToValidate="txtPfromdate" 
                CssClass="field_error" Display="Dynamic" 
                EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                ErrorMessage="MEPFromDate" InvalidValueBlurredMessage="Invalid Date" 
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format">
            </cc1:MaskedEditValidator></td>
            
            <td class="td_cell"> To Date</td>
            
            <td> 
             <asp:TextBox ID="txtPtodate" tabIndex=9 runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
            <cc1:CalendarExtender ID="CEPToDate" runat="server" Format="dd/MM/yyyy" 
                PopupButtonID="ImgPBtntoDt" TargetControlID="txtPToDate">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MEPToDate" runat="server" Mask="99/99/9999" 
                MaskType="Date" TargetControlID="txtPToDate">
            </cc1:MaskedEditExtender>
            <asp:ImageButton ID="ImgPBtntoDt" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            <cc1:MaskedEditValidator ID="MEVPToDate" runat="server" 
                ControlExtender="MEPToDate" ControlToValidate="txtPToDate" 
                CssClass="field_error" Display="Dynamic" 
                EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                ErrorMessage="MEPToDate" InvalidValueBlurredMessage="Invalid Date" 
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format">
            </cc1:MaskedEditValidator>
            </td>
            <td>
               <asp:Button ID="btnDisplay" runat="server" CssClass="field_button" 
                onclick="btnDisplay_Click" tabIndex="10" Text="Display Unmatched" />
            </td>
            <td>   <asp:Button ID="btnClear" runat="server" CssClass="field_button" 
                onclick="btnClear_Click" tabIndex="11" Text="Refresh" /></td>
            </tr></table>

            




        </td>
        <td class="td_cell">
         
           </td>
        <td class="td_cell">
           
        </td>
        <td class="td_cell" colspan="1">
         
        </td>
    </tr>
    <TR><TD class="td_cell" align=center colSpan=6><IMG id="imgicon" height=25 src="../Images/loading.gif" width=400 runat="server" /></TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" colSpan=6 style="height: 318px"><DIV style="HEIGHT: 300px" class="container">
 <asp:GridView id="grdMatchOut" tabIndex=12 runat="server" Font-Size="10px" Width="900px" CssClass="td_cell" BackColor="White" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999">
<FooterStyle  CssClass="grdfooter" ForeColor="Black"></FooterStyle>
<Columns>
<asp:BoundField DataField="type" HeaderText="Doc. Type"></asp:BoundField>
<asp:BoundField DataField="docno_new" HeaderText="Doc. Number"></asp:BoundField>
<asp:BoundField DataField="field3" HeaderText="Ref. Number"></asp:BoundField>
<%--<asp:BoundField DataField="docno_new" HeaderText="Doc. Number"></asp:BoundField>
<asp:BoundField DataField="DocNo" HeaderText="Ref. Number"></asp:BoundField>--%>
<asp:BoundField DataField="field1" HeaderText="File Number"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="Date" HeaderText="Doc. Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="duedate" HeaderText="Due Date"></asp:BoundField>
<asp:TemplateField HeaderText="Balance">
<ItemStyle HorizontalAlign="Right"></ItemStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<asp:TextBox id="txtBalanceAmt" runat="server" Text='<%# Bind("amount") %>' Width="88px" CssClass="field_input"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Exchage Rate"><ItemTemplate>
<asp:TextBox id="txtCurrRate" runat="server" Text='<%# Bind("currrate") %>' Width="60px" CssClass="field_input"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Debit">
<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<asp:TextBox style="TEXT-ALIGN: right" id="txtDebitAmt" runat="server" Width="80px" CssClass="field_input" MaxLength="25"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Credit">
<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<asp:TextBox style="TEXT-ALIGN: right" id="txtCreditAmt" runat="server" Width="80px" CssClass="field_input" MaxLength="25"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Select"><ItemTemplate>
&nbsp;<INPUT id="chkSelect" class="field_input" onclick="return Checkbox1_onclick()" type=checkbox runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Particulars"><ItemTemplate>
&nbsp;<asp:TextBox id="txtParticulars" runat="server" Width="120px" CssClass="field_input"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Debit"><ItemTemplate>
<asp:TextBox style="TEXT-ALIGN: right" id="txtBaseDebit" runat="server" Width="80px" CssClass="field_input" MaxLength="12"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Credit"><ItemTemplate>
<asp:TextBox style="TEXT-ALIGN: right" id="txtBaseCredit" runat="server" Width="80px" CssClass="field_input" MaxLength="12"></asp:TextBox> 
<asp:TextBox id="txtdocNo1" runat="server" Text='<%# Bind("docno") %>' style="display:none" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="acc_tran_lineno" Visible="False" HeaderText="LineNo"></asp:BoundField>
<asp:TemplateField Visible="False" HeaderText="LineID"><EditItemTemplate>
&nbsp;
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblLineID" runat="server" Text='<%# Bind("acc_tran_lineno") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle  CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView></DIV></TD></TR>
    <tr>
        <td align="right" class="td_cell" colspan="6" style="height: 24px" >
            &nbsp; &nbsp;<asp:Label ID="Label8" runat="server" CssClass="field_caption" Text="Diff Amt Post A/c"
                Width="104px"></asp:Label>
   <asp:TextBox ID="TxtExchName_auto" runat="server" TabIndex="3" 
            AutoPostBack="false" CssClass="field_input" MaxLength="500"  Width="239px"
             ></asp:TextBox>
        <asp:TextBox ID="TxtExchCode_auto" runat="server" Style="display: none"></asp:TextBox>
        <asp:HiddenField ID="HiddenFieldExch" runat="server" Visible="False" />
        <asp1:AutoCompleteExtender ID="ExchCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" ServiceMethod="Getexchglist" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    TargetControlID="TxtExchName_auto" OnClientItemSelected="exchautocompleteselected">
                                                                   </asp1:AutoCompleteExtender>
            <asp:Label ID="Label9" runat="server" CssClass="field_caption" Style="left: 19px;
                top: 4px" Text="Diff Amt "></asp:Label>
            &nbsp;<input id="exchdebit" runat="server" class="field_input" disabled="disabled"
                style="width: 96px; text-align: right" type="text" />
            <input id="exchcredit" runat="server" class="field_input" style="width: 96px; text-align: right"
                type="text" disabled="disabled" /></td>
    </tr>
    <TR><TD class="td_cell" align=right colSpan=6><asp:Label id="Label1" runat="server" 
            Text="Total  Amount" Width="103px" CssClass="field_caption"></asp:Label>&nbsp;&nbsp;<INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtDebitTotal" class="field_input" disabled type=text runat="server" />&nbsp;<INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtCreditTotal" class="field_input" disabled type=text runat="server" /> <asp:Label id="lblBaseTotal" runat="server" Text="Base Total" Width="77px" CssClass="field_caption"></asp:Label>&nbsp;<INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtBaseDebitTotal" class="field_input" disabled type=text runat="server" />&nbsp;<INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtBaseCreditTotal" class="field_input" disabled type=text runat="server" /></TD></TR><TR><TD style="HEIGHT: 26px" class="td_cell" align=right colSpan=6>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
            height: 9px" type="text" />
        <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
     
        <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtgridrows" class="field_input" tabIndex=5000 type=text maxLength=100 runat="server" Visible="true" /> <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtDueDate" type=text maxLength=20 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtbasecurr" type=text maxLength=20 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtDivCode" type=text maxLength=20 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="Text1" type=text maxLength=15 runat="server" /><INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtdecimal" type=text maxLength=15 runat="server" /><INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustcode" type=text maxLength=100 runat="server" /><INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtcustname" type=text maxLength=200 runat="server" /><INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtConAccName" type=text maxLength=100 runat="server" /><INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtConAccCode" type=text maxLength=100 runat="server" /> 
        <asp:CheckBox id="chkPost" tabIndex=13 runat="server" Text="Post/UnPost" Font-Size="10px" Font-Names="Verdana" Width="103px" CssClass="field_caption" BackColor="#FFC0C0" Font-Bold="True" ForeColor="Black" Checked="true"></asp:CheckBox> 
        <asp:Button id="btnSave" tabIndex=14 onclick="btnSave_Click" runat="server" 
            Text="Save" CssClass="field_button" Height="20px"></asp:Button>&nbsp;
            <asp:Button 
            id="btnclientprint" tabIndex=15 onclick="btnclientprint_Click" visible=false runat="server" Text="Client Curreny Print" 
            CssClass="field_button" Height="20px"></asp:Button>&nbsp;
            <asp:Button 
            id="btnPrint" tabIndex=16 onclick="btnPrint_Click" runat="server" Text="Print" 
            CssClass="field_button" Height="20px"></asp:Button>
                        <asp:Button 
            id="btnclientpdf" tabIndex=16 onclick="btnclientpdf_Click" runat="server" Text="Client Currency PdfReport" 
            CssClass="field_button" Height="20px"></asp:Button>
             <asp:Button 
            id="PdfReport" tabIndex=16 onclick="PdfReport_Click" runat="server" Text="PdfReport" 
            CssClass="field_button" Height="20px"></asp:Button>&nbsp;
            &nbsp;<asp:Button 
            id="btnCancel" tabIndex=17 onclick="btnCancel_Click" runat="server" Text="Exit" 
            CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
            id="btnhelp" tabIndex=18 onclick="btnhelp_Click" runat="server" Text="Help" 
            CssClass="field_button" Height="20px"></asp:Button>&nbsp;</TD></TR></TBODY></TABLE>
            <cc1:CalendarExtender id="ClsExFromDate" runat="server" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left" ErrorTooltipEnabled="True"></cc1:MaskedEditExtender> 
                <asp:HiddenField ID="hdnSS" runat="server" Value="0" />
</contenttemplate>
</asp:UpdatePanel>
 <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 

</asp:Content>
