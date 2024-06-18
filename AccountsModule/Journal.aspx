<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Journal.aspx.vb" Inherits="Journal"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"    TagPrefix="ews" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="javascript" type="text/javascript">


function Validaterowno()
{

    var txtNoofRows=document.getElementById("<%=txtNoofRows.ClientID%>");
 
    if(txtNoofRows.value =="")
    {
        alert('Enter valid No of rows');
        return false;    
    }
    else
    { 
    return true;
     
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
var nodecround =null;
var txtgrdcrate=null;
var txtfill1=null;
var txtcrate=null;
var ddlACode=null;
var ddlAName=null;
var ddlAConAcc=null;
var ddlAConAccNm=null;
var txtAConAcc=null;
var txtAConAccNm=null;
var sqlstr= null;
var txtgrdDebAmt = null;
var txtgrdDbBaseAmt = null;
var txtgrdCrAmt = null;
var txtgrdCrBaseAmt = null;
var txtcurrname = null;

var txtctryname = null;
var txtctrycode = null;

 function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g,"");	
}
function DecRound(amtToRound) 
{
  var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
  nodecround=Math.pow(10,parseInt(txtdec.value));
  var rdamt=Math.round(parseFloat(Number(amtToRound))*nodecround)/nodecround;
  return  parseFloat(rdamt);
}
function DecRoundtothree(amtToRound) {
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


    function fill_acountcode(ddltp, AutoCompleteExtender, txtcode, txtname, txtConAccd, txtConAccnm, txtgnarr, txtcostcd, txtcostnm, txtgrdcurr,txtgrdcurrname, txtgrdconvrate, txtgrddebt, txtgrdbsdebt, txtgrdcr, txtgrdbscr, txtgrdsrcctryc, txtgrdsrcctryn) 
    {
        //            alert('dd');

        ddltyp = document.getElementById(ddltp);
        //  AutoCompleteExtender = document.getElementById(AutoCompleteExtender);
        var strtp = ddltyp.value;
        txtcode = document.getElementById(txtcode);
        txtcode.value = '';
        txtname = document.getElementById(txtname);
        txtname.value = '';
        txtConAccd = document.getElementById(txtConAccd);
        txtConAccnm = document.getElementById(txtConAccnm);
        txtConAccd.value = '';
        txtConAccnm.value = '';

        txtgrdcurr = document.getElementById(txtgrdcurr);

        txtgrdcurr.value = '';
         txtgrdcurrname = document.getElementById(txtgrdcurrname);

        txtgrdcurrname.value = '';


        txtgrdconvrate = document.getElementById(txtgrdconvrate);
        txtgrdconvrate.value = '';

        txtgrddebt = document.getElementById(txtgrddebt);
        txtgrddebt.value = '';
        txtgrdbsdebt = document.getElementById(txtgrdbsdebt);
        txtgrdbsdebt.value = '';
        txtgrdcr = document.getElementById(txtgrdcr);
        txtgrdcr.value = '';
        txtgrdbscr = document.getElementById(txtgrdbscr);
        txtgrdbscr.value = '';

        txtgrdsrcctryc = document.getElementById(txtgrdsrcctryc);
        txtgrdsrcctryc.value = '';
        txtgrdsrcctryn = document.getElementById(txtgrdsrcctryn);
        txtgrdsrcctryn.value = '';

        txtgrdsrcctryn.disabled = true;

        txtcostcd = document.getElementById(txtcostcd);
        txtcostnm = document.getElementById(txtcostnm);
        var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");
        var contxt = strtp + '||' + txtdivauto.value;

        $find(AutoCompleteExtender).set_contextKey(contxt);







        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value



        //            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr3, FillControlAcc, ErrorHandler, TimeOutHandler);
        //            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr4, FillControlAccName, ErrorHandler, TimeOutHandler);



       
        txtnarr = document.getElementById("<%=txtnarration.ClientID%>");
        document.getElementById(txtgnarr).value = trim(txtnarr.value);

//        txtcostcd.disabled = false;
//        txtcostnm.disabled = false;
        if (strtp == 'G') {

            txtgrdsrcctryn.disabled = true;
//            txtcostcd.disabled = false;
//            txtcostnm.disabled = false;
        }
        else {
//            txtcostcd.disabled = true;
//            txtcostnm.disabled = true;
        }


//        OnChangeType(txtboxauto, ddlname, ddltp);

    }


 
//    ddltyp = document.getElementById(ddltp);
//    var strtp = ddltyp.value;
//    ddlACode = document.getElementById(ddlcode);
//    ddlAName = document.getElementById(ddlname);
//    
//    ddlAConAcc = document.getElementById(ddlConAccd);
//    ddlAConAccNm = document.getElementById(ddlConAccnm);
//  
//    ddlcostcode = document.getElementById(ddlcostcd);
//    ddlcostname = document.getElementById(ddlcostnm);

//    var divcode = document.getElementById("<%=txtDivCode.ClientID%>");
//    
//     var sqlstr1=null;
//  var sqlstr2=null;
//  var sqlstr3=null;
//  var sqlstr4 =null;
//  
//  if (strtp!='[Select]')
//  {
//    sqlstr1="select Code,des from view_account where div_code='" + divcode.value + "' and type = '"+ strtp +"' order by code";
//    sqlstr2 = "select des,Code from view_account where div_code='" + divcode.value + "' and type = '" + strtp + "' order by des";
//    sqlstr3 = "select isnull(controlacctcode,0),code from view_account where div_code='" + divcode.value + "' and type= '" + strtp + "' order by code";
//    sqlstr4 = " select distinct acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" + divcode.value + "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' order by  acctmast.acctname";
// }
// else
// {
//     sqlstr1 = "select top 10 Code,des from view_account where div_code='" + divcode.value + "' and  order by code";
//     sqlstr2 = "select top 10  des,Code from view_account where div_code='" + divcode.value + "' and   order by des";
//     sqlstr3 = "select top 10  isnull(controlacctcode,0),code from view_account where div_code='" + divcode.value + "' and  order by code";
//     sqlstr4 = " select distinct top 10   acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  acctmast.div_code=view_account.div_code and acctmast.div_code='" + divcode.value + "' and  view_account.controlacctcode= acctmast.acctcode  order by  acctmast.acctname";
// }
//    
//    
//     
//    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
//    constr=connstr.value   


//    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillACodes,ErrorHandler,TimeOutHandler);
//    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr2,FillANames,ErrorHandler,TimeOutHandler);
//    
//    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr3,FillControlAcc,ErrorHandler,TimeOutHandler);
//    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr4,FillControlAccName,ErrorHandler,TimeOutHandler);
//  
//  
//        
//    txtgnarrs = document.getElementById(txtgnarr);
//    txtnarr = document.getElementById("<%=txtnarration.ClientID%>" );
//    txtgnarrs.value=trim(txtnarr.value);
//  
//    ddlcostcode.disabled=false;
//    ddlcostname.disabled=false;
//    if (strtp=='G')
//    {
//     ddlcostcode.disabled=false;
//     ddlcostname.disabled=false;
//    }
//    else
//    {
//     ddlcostcode.disabled=true;
//     ddlcostname.disabled=true;
// }


// OnChangeType(txtboxauto, ddlname, ddltp);









//function FillGACode(ddlIccd, ddlIcnm, txtcurr, txtrate, ddlIContAcc, ddlConAccnm, ddltp, txtaccd, txtacnm, txtcramt, txtcrbaseamt, txtdbamt, txtdbbaseamt, txtcontcd, txtcontnm, txtboxauto)
//{   
// 
//  txtgrdDebAmt = document.getElementById(txtdbamt);
//  txtgrdDbBaseAmt = document.getElementById(txtdbbaseamt);
//  txtgrdCrAmt = document.getElementById(txtcramt);
//  txtgrdCrBaseAmt = document.getElementById(txtcrbaseamt);
//  txtauto = document.getElementById(txtboxauto);

//  
//    ddltyp = document.getElementById(ddltp);
//    var strtp = ddltyp.value;
//    
//    
//    ddlIccode = document.getElementById(ddlIccd);
//    ddlIcname = document.getElementById(ddlIcnm);
//    ddlAConAcc = document.getElementById(ddlIContAcc);  
//    ddlAConAccNm = document.getElementById(ddlConAccnm);  
//    txtAConAcc = document.getElementById(txtcontcd);  
//    txtAConAccNm = document.getElementById(txtcontnm);

//    var codeid=ddlIccode.options[ddlIccode.selectedIndex].text;
//    ddlIcname.value=codeid;
//    
//    txtaccd = document.getElementById(txtaccd);  
//    txtacnm = document.getElementById(txtacnm); 
//    txtaccd.value=ddlIccode.options[ddlIccode.selectedIndex].value;
//    txtacnm.value=codeid;
//    
//   
//    txtfill = document.getElementById(txtcurr);
//    txtgrdcrate = document.getElementById(txtrate);

//    var divcode = document.getElementById("<%=txtDivCode.ClientID%>");
//    
////    sqlstr="select cur,cur from view_account where Code='" + ddlIcname.value + "'" ;
////    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurr,ErrorHandler,TimeOutHandler);
////   
////   
////    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
////    sqlstr="select convrate from currrates ,view_account  where  currrates.currcode=view_account.cur and   view_account.code='"+ ddlIcname.value +"' and Tocurr='"+ txtbase.value +"'"
////    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillGrdCvntRate,ErrorHandler,TimeOutHandler);
//    
//    
//    var sqlstr1,sqlstr2
//     ddlAConAcc.disabled=false;
//     ddlAConAccNm.disabled=false;
//    if (strtp=='C')
//    {
//        sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" + divcode.value + "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
//        sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  acctmast.div_code=view_account.div_code and acctmast.div_code='" + divcode.value + "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  acctmast.acctname";
//    }
//     else if (strtp=='S')
//    {
//        sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" + divcode.value + "' and   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"

//        sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" + divcode.value + "' and   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by acctmast.acctname"  
//  
//   }
//     else if (strtp=='A')
//    {
//        sqlstr1 = " select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where  acctmast.div_code='" + divcode.value + "' and   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"

//        sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where  acctmast.div_code='" + divcode.value + "' and   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' order by acctmast.acctname"  
//    }
//     else if (strtp=='G')
//    {
////     sqlstr1=" select '' as controlacctcode, '' as acctname  "  
//        //     sqlstr2=" select '' as acctname , '' as controlacctcode "
//        ddlAConAcc.value = '[Select]';
//        ddlAConAccNm.value = '[Select]';
//     ddlAConAcc.disabled=true;
//     ddlAConAccNm.disabled=true;
//    }
//    
//    if (strtp !='[Select]')
//    {
//     var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
//    constr=connstr.value
//    if (strtp != 'G'){
//    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillControlAcc,ErrorHandler,TimeOutHandler);
//    ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillControlAccName, ErrorHandler, TimeOutHandler);
//    }
//    FillCustDetails(strtp,codeid);
//     }
//    else
//    {
//        alert('Please Select Account Type');
//        ddlIccode.value = "[Select]";
//        ddlIcname.value = "[Select]";
//        txtauto.value = "";
//    }
//    

//}






//function FiilCustDt(result)
//    {
//    txtfill.value = result[0];
//    txtgrdcrate.value = result[1];
//    if (result[2] != '') {
//        ddlAConAccNm.value = result[2];

//        var codeid = ddlAConAccNm.options[ddlAConAccNm.selectedIndex].text;
//    }
//    else {
//        ddlAConAccNm.value = "[Select]";
//        var codeid = ddlAConAccNm.options[ddlAConAccNm.selectedIndex].text;
//    }
//    ddlAConAcc.value =codeid;    
////    txtAConAcc.value=codeid;
////    txtAConAccNm.value=result[2];
////    
//    GrdExchangeRateChange(result[1]);
//    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
//    
//    if ( trim(txtfill.value)==trim(txtbase.value))
//       {
//        txtgrdcrate.readOnly=true; 
//        txtgrdcrate.disabled=true;
//       }
//       else
//       {
//        txtgrdcrate.readOnly=false;
//        txtgrdcrate.disabled=false;
//       }
//     
//   } 






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
function FillCurr(result)
    {
//    txtfill.value=result;
    } 

    function FillGrdCvntRate(result)
    {
     txtgrdcrate.value=DecRound(result);
     GrdExchangeRateChange(result)
     } 
   
function FillCombotoText(ddlc,txtt)
{
    ddlcs = document.getElementById(ddlc); 
    txtts = document.getElementById(txtt); 
    
    var codeid=ddlcs.options[ddlcs.selectedIndex].text;
    txtts.value=codeid;
}   



 function  GrdExchangeRateChange(result)
 {
  txtCnvtRate = result;
    
    if (trim(txtgrdDebAmt.value)==''){txtgrdDebAmt.value=0;}
    if (isNaN(txtgrdDebAmt.value)==true){txtgrdDebAmt.value=0;} 
    if (trim(txtgrdCrAmt.value)==''){txtgrdCrAmt.value=0;}
    if (isNaN(txtgrdCrAmt.value)==true){txtgrdCrAmt.value=0;}
    
    var amt =DecRound(txtgrdDebAmt.value) *  parseFloat(txtCnvtRate) ;
    txtgrdDbBaseAmt.value=DecRound(amt);
     
    
     var amt1 =DecRound(txtgrdCrAmt.value) *  parseFloat(txtCnvtRate) ;
     txtgrdCrBaseAmt.value=DecRound(amt1);
   
   // grdTotal()
 }

 function  convertInRate(txtdbamt,txtdebbaseamt,txtcramt,txtcrbaseamt,txtcnvRate,pstr)
 {
  
    var txtgrdDebAmt1 = document.getElementById(txtdbamt);
    var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
    var txtgrdCrAmt1 = document.getElementById(txtcramt);
    var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
    var txtCnvtRate1 = document.getElementById(txtcnvRate);
  
     if (trim(txtgrdDebAmt1.value)==''){txtgrdDebAmt1.value=0;}
     if (isNaN(txtgrdDebAmt1.value)==true){txtgrdDebAmt1.value=0;}
     if (trim(txtgrdCrAmt1.value)==''){txtgrdCrAmt1.value=0;}
     if (isNaN(txtgrdCrAmt1.value)==true){txtgrdCrAmt1.value=0;}
          
 
    if (pstr=='Debit')
    {
       if (   Number(txtgrdCrAmt1.value) > 0 )
           {
           
                txtgrdDebAmt1.value = 0 ;
              //  txtgrdDebAmt1.readOnly=true;
           }
       else if (  Number(txtgrdCrAmt1.value) == 0  || txtgrdCrAmt1.value=='' )
          {
           
             // txtgrdCrAmt1.readOnly=true;
           }
   }
   else if (pstr=='Credit')
    {
    
      if ( Number(txtgrdDebAmt1.value) > 0 )
           {
                txtgrdCrAmt1.value= 0 ;
               // txtgrdCrAmt1.readOnly=true;
           }
       else if ( Number(txtgrdDebAmt1.value) == 0  || txtgrdDebAmt1.value=='' )
          {
              //txtgrdDebAmt1.readOnly=true;
           }
       }
    
      
     var amt =DecRound(txtgrdDebAmt1.value) *  parseFloat(txtCnvtRate1.value) ;
     
     txtgrdDbBaseAmt1.value=DecRound(amt);
     
     var amt1 =DecRound(txtgrdCrAmt1.value) *  parseFloat(txtCnvtRate1.value) ;
     txtgrdCrBaseAmt1.value=DecRound(amt1);
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

function grdTotal()
{
    var totCr = 0 ;
    var totDr = 0 ;
    var totbCr = 0 ;
    var totbDr = 0 ;
    
    var objGridView = document.getElementById('<%=grdJournal.ClientID%>');
    var txtrowcnt =document.getElementById('<%=txtgridrows.ClientID%>');
    intRows=txtrowcnt.value;
     
     for(j=1;j<=intRows;j++)
    {
       
//       var valDr=objGridView.rows[j].cells[8].children[0].value;
        //       var valCr=objGridView.rows[j].cells[9].children[0].value;

        var valDr = objGridView.rows[j].cells[6].children[0].value;
        var valCr = objGridView.rows[j].cells[7].children[0].value;
 
//       var valbDr=objGridView.rows[j].cells[10].children[0].value;
        //       var valbCr=objGridView.rows[j].cells[11].children[0].value;

        var valbDr = objGridView.rows[j].cells[8].children[0].value;
        var valbCr = objGridView.rows[j].cells[9].children[0].value;
           
       if (valCr==''){valCr=0;}
       if (valDr==''){valDr=0;}
       if (valbCr==''){valbCr=0;}
       if (valbDr==''){valbDr=0;}
       
       if (isNaN(valCr)==true){valCr=0;}
       if (isNaN(valDr)==true){valDr=0;}
       
       if (isNaN(valbCr)==true){valbCr=0;}
       if (isNaN(valbDr)==true){valbDr=0;}
       
        
       totCr=DecRound(totCr)+DecRound(valCr);
       totDr=DecRound(totDr)+DecRound(valDr);
       
       totbCr=DecRound(totbCr)+DecRound(valbCr);
       totbDr = DecRound(totbDr) + DecRound(valbDr);


   }
 
    var txttotCr  = document.getElementById('<%=txtTotalCredit.ClientID%>');
    var txttotDr = document.getElementById('<%=txtTotalDebit.ClientID%>');
   
    var txttotbCr  = document.getElementById('<%=txtTotBaseCredit.ClientID%>');
    var txttotbDr = document.getElementById('<%=txtTotBaseDebit.ClientID%>');
    
    var txttotbDiff = document.getElementById('<%=txtTotBaseDiff.ClientID%>');

    txttotDr.value=DecRound(totDr);
    txttotCr.value=DecRound(totCr);
        
    txttotbDr.value=DecRound(totbDr);
    txttotbCr.value=DecRound(totbCr);
    
    txttotbDiff.value=DecRound(totbDr-totbCr);
    
 
  }
  function openAdjustBill(ddlACode, ddlconCode, ddltyp, txtTranId, txtlno, txtcurrCode, txtCurRate, txtcreditamt, txtcreditbaseamt, txtoldlno, txtdebitamt, txtdebitbaseamt, txtsource)
 { 
       
        var ddlACode = document.getElementById(ddlACode);
        var ddlContCode = document.getElementById(ddlconCode);
        var ddltyp = document.getElementById(ddltyp);
        var txtTranId = document.getElementById(txtTranId);
        var txtLineNo = document.getElementById(txtlno);
        var txtOLineNo = document.getElementById(txtoldlno);
        var txtCurrsCode = document.getElementById(txtcurrCode);
        var txtCurRate = document.getElementById(txtCurRate);
        var  txtCrAmt = document.getElementById(txtcreditamt);
        var  txtCrBaseAmt = document.getElementById(txtcreditbaseamt);
        var txtDRAmt = document.getElementById(txtdebitamt);
        var txtDRBaseAmt = document.getElementById(txtdebitbaseamt);

        var txtsrcctry = document.getElementById(txtsource); 
        
        var txtgrdtype=document.getElementById("<%=txtGridType.ClientID%>");    
//        
//        if trim(txtCrBaseamt.value) =='' ){txtCrBaseamt.value=0;}
//        if trim(txtDbBaseamt.value) =='' ){txtDbBaseamt.value=0;}
//        
//        if isNaN(txtCrBaseamt.value) =true ){txtCrBaseamt.value=0;}
//        if isNaN(txtDbBaseamt.value) =true ){txtDbBaseamt.value=0;}
        
         var passBaseAmt = 0;
         var passAmt = 0;
         
        var valCr =DecRound(txtCrAmt.value) ;
        var valDr =DecRound(txtDRAmt.value) ;
        var valCrBase =DecRound(txtCrBaseAmt.value) ;
        var valDrBase =DecRound(txtDRBaseAmt.value) ;
          
 
         if (valCr=='' || valCr==0)
           {
            passAmt=valDr;
            passBaseAmt=valDrBase;
            txtgrdtype.value='Debit';
            }
         if (valDr=='' || valDr==0)
           {
           passAmt=valCr;
           passBaseAmt=valCrBase;
           txtgrdtype.value='Credit';
           }
        if (passAmt==0)
        {
        alert("Please enter debit or credit amount.")
        return false;
        }
    if (ddltyp.value == 'C'&& txtsrcctry.value=="") {

        alert("Please enter source country of the client.")
        return false;
        }

    var divcode = document.getElementById("<%=txtDivCode.ClientID%>");
        txttrantype=document.getElementById("<%=txtTranType.ClientID%>");
        var txtrowcnt =document.getElementById("<%=txtgridrows.ClientID%>");
        intRows=txtrowcnt.value;
        txtstate=document.getElementById("<%=txtMode.ClientID%>");
        txtrefcode=document.getElementById("<%=txtDocNo.ClientID%>");
        txtAdjcolcode = document.getElementById("<%=txtAdjcolno.ClientID%>");
        txtDate = document.getElementById("<%=txtTDate.ClientID%>");
        var pass;
        if(ddltyp.value != 'G'){
       // pass="TranType="+ txttrantype.value+"&AccCode=" + ddlACode.value + "&ControlCode=" + ddlContCode.value + "&AccType=" + ddltyp.options[ddltyp.selectedIndex].text  + "&TranId=" + txtTranId.value  + "&lineNo=" + txtLineNo.value + "&currcode=" + txtCurrsCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + passAmt + "&Gridtype=" +txtgrdtype.value+ "&MainGrdCount="+intRows;
        //pass="TranType="+txttrantype.value +"&AccCode=" + ddlACode.value + "&ControlCode=" + ddlContCode.value + "&AccType=" + ddltyp.options[ddltyp.selectedIndex].text  + "&TranId=" + txtTranId.value  + "&lineNo=" + txtLineNo.value + "&currcode=" + txtCurrsCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + passAmt +"&BaseAmount=" + passBaseAmt+ "&Gridtype=" +txtgrdtype.value+ "&MainGrdCount="+intRows+"&OlineNo=" + txtOLineNo.value;

            pass = "TranType=" + txttrantype.value + "&AccCode=" + ddlACode.value + "&divid="+ divcode.value +  "&ControlCode=" + ddlContCode.value + "&AccType=" + ddltyp.value + "&TranId=" + txtTranId.value + "&lineNo=" + txtLineNo.value + "&currcode=" + txtCurrsCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + passAmt + "&BaseAmount=" + passBaseAmt + "&Gridtype=" + txtgrdtype.value + "&MainGrdCount=" + intRows + "&OlineNo=" + txtOLineNo.value + "&State=" + txtstate.value + "&RefCode=" + txtrefcode.value + "&AdjColno=" + txtAdjcolcode.value + "&trandate=" + txtDate.value;
        //window.open('ReceiptsAdjustBills.aspx?' + pass,'ReceiptsAdjustBills');

            //window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,fullscreen=yes');
            //17122014
            window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,left=0,top=0,height=' + screen.height + ',width=' + screen.width);
        return false;
        
        }else
        {
        alert('Account type ‘G’ doesn’t  adjust bill .'); 
        return false;
        }
 }
	function FillCodeName(ddlcode,ddlname)
{
    ddlc = document.getElementById(ddlcode);
    ddln = document.getElementById(ddlname);
    ddln.value=ddlc.options[ddlc.selectedIndex].text;
}

 function DecRoundEightPalces(amtToRound) 
{
  
  var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
  //nodecround=Math.pow(10,parseInt(8));
  nodecround = Math.pow(10, parseInt(txtdec.value));
  var rdamt=Math.round(parseFloat(Number(amtToRound))*nodecround)/nodecround;
  return  parseFloat(rdamt);
}
function  convertRateOnBaseCurrency(typ,txtdbamt,txtdebbaseamt,txtcramt,txtcrbaseamt,txtcnvRate,pstr)
 {
    var  acctyp=document.getElementById(typ);
    var txtgrdDebAmt1 = document.getElementById(txtdbamt);
    var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
    var txtgrdCrAmt1 = document.getElementById(txtcramt);
    var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
    var txtCnvtRate1 = document.getElementById(txtcnvRate);
  
     if (trim(txtgrdDebAmt1.value)==''){txtgrdDebAmt1.value=0;}
     if (isNaN(txtgrdDebAmt1.value)==true){txtgrdDebAmt1.value=0;}
     if (trim(txtgrdCrAmt1.value)==''){txtgrdCrAmt1.value=0;}
     if (isNaN(txtgrdCrAmt1.value)==true){txtgrdCrAmt1.value=0;}
          
  
        if (pstr=='Debit')
        {
            if (   Number(txtgrdCrBaseAmt1.value) > 0 )
               {
               
                    txtgrdDbBaseAmt1.value = 0 ;
                }
          else if ( Number(txtgrdDebAmt1.value) == 0  || txtgrdDebAmt1.value=='' )
              {
                  txtgrdDebAmt1.value= 0 ;
                  //txtgrdDebAmt1.readOnly=true;
               }
           
        }
        else if (pstr=='Credit')
        {
           if ( Number(txtgrdDbBaseAmt1.value) > 0 )
               {
                    txtgrdCrBaseAmt1.value= 0 ;
            
               }
            else if (  Number(txtgrdCrAmt1.value) == 0  || txtgrdCrAmt1.value=='' )
              {
                 txtgrdCrBaseAmt1.value = 0 ;
                 // txtgrdCrAmt1.readOnly=true;
               }
        }
 
  if (acctyp.value!='G')
  {      
          if (pstr=='Debit')
          {
                if(txtgrdDebAmt1.value=='' || txtgrdDebAmt1.value==0 )
                   {
                   var amt =DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtCnvtRate1.value) ;
                   txtgrdDebAmt1.value=DecRoundEightPalces(amt);  
                   }
                else
                  {
                   var amt =DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtgrdDebAmt1.value) ;
                   txtCnvtRate1.value=DecRoundEightPalces(amt); 
                  }   
          }
          else if (pstr=='Credit')
          {
              if(txtgrdCrAmt1.value=='' || txtgrdCrAmt1.value==0 )
               {
                var amt =DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtCnvtRate1.value) ;
                txtgrdCrAmt1.value=DecRoundEightPalces(amt);     
               }
               else
               {
                var amt =DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtgrdCrAmt1.value) ;
                txtCnvtRate1.value=DecRoundEightPalces(amt);     
               }
          }
  }
  else
  {
       if (pstr=='Debit')
          {
                if (parseFloat(txtgrdDbBaseAmt1.value) > 0)
                   {
                   var amt =DecRound(txtgrdDbBaseAmt1.value) /  DecRound(txtCnvtRate1.value) ;
                   txtgrdDebAmt1.value=DecRoundEightPalces(amt);  
                   }
          }
          else if (pstr=='Credit')
        {
            if (parseFloat(txtgrdCrBaseAmt1.value) > 0)
               {
                var amt =DecRound(txtgrdCrBaseAmt1.value) /  DecRound(txtCnvtRate1.value) ;
                txtgrdCrAmt1.value=DecRoundEightPalces(amt);     
               }
         }
 }   
     
      grdTotal()
 }
 

// function  convertRateOnBaseCurrency(txtDBAmt,txtCnvtRate,txtCnvtAmt,pstr)
// {
//    
//    txtDBAmt = document.getElementById(txtDBAmt);
//    txtCnvtRate = document.getElementById(txtCnvtRate);
//    txtCnvtAmt = document.getElementById(txtCnvtAmt);  
// if (pstr=='Debit')
//    {
//       if (   Number(txtgrdCrAmt1.value) > 0 )
//           {
//           
//                txtgrdDebAmt1.value = 0 ;
//           
//           }
//       else if (  Number(txtgrdCrAmt1.value) == 0  || txtgrdCrAmt1.value=='' )
//          {
//           
//            
//           }
//   }
//   else if (pstr=='Credit')
//    {
//    
//      if ( Number(txtgrdDebAmt1.value) > 0 )
//           {
//                txtgrdCrAmt1.value= 0 ;
//           
//           }
//       else if ( Number(txtgrdDebAmt1.value) == 0  || txtgrdDebAmt1.value=='' )
//          {
//             
//           }
//       }
//    
//      
//    if (trim(txtDBAmt.value)==''){txtDBAmt.value=0;}
//     var amt = DecRound(txtCnvtAmt.value)/DecRound(txtDBAmt.value);
//     txtCnvtRate.value=DecRoundEightPalces(amt);
//     grdTotal()
// }
 

////---------------------------------------------------
//function  openAdjustBill(ddlACode,ddltyp,txtTranId,txtlno,txtCrCode,txtCurRate,txtCAmt,txtDAmt)
// { 
////        var ddlACode = document.getElementById(ddlACode);
////        ddltyp = document.getElementById(ddltyp);
////        txtTranId = document.getElementById(txtTranId);
////        var txtLienNo = document.getElementById(txtlno);
////        txtCrCode = document.getElementById(txtCrCode);
////        txtCurRate = document.getElementById(txtCurRate);
////       
////        txtCAmt = document.getElementById(txtCAmt);
////        txtDAmt = document.getElementById(txtDAmt);
////        
////       var pass;
////      var passAmt = 0;
//// 
////         var valCr =txtCAmt.value ;
////         var valDr =txtDAmt.value ;
//// 
////         if (valCr=='' || valCr==0)
////         {passAmt=valDr;}
////         if (valDr=='' || valDr==0)
////         {passAmt=valCr;}
////                  
////        if (passAmt==0)
////        {
////        alert("Please enter debit or credit amount.")
////        return false;
////        }
////        
////        if(ddltyp.options[ddltyp.selectedIndex].text != 'G'){
////        pass="AccCode=" + ddlACode.value + "&AccType=" + ddltyp.options[ddltyp.selectedIndex].text  + "&TranId=" + txtTranId.value  + "&linNo=" + txtLienNo.value + "&crcode=" + txtCrCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + passAmt
////        window.open('JournalAdjustBills.aspx?' + pass,'popupwindow');
////        return false;
////        
////        }else
////        {
////        alert('Account type ‘G’ doesn’t  adjust bill .'); 
////        return false;
////        }
// }
function ValidatePage()
{
    var txtBaseDebit=document.getElementById("<%=txtTotBaseDebit.ClientID%>");
    var txtBaseCredit=document.getElementById("<%=txtTotBaseCredit.ClientID%>");
    
     var hdnSS=document.getElementById("<%=hdnSS.ClientID%>");
       
   if((txtBaseDebit.value == '' && txtBaseCredit.value == '')||(txtBaseDebit.value == '0' && txtBaseCredit.value == '0'))
   {
      
       return true;    
      
          
   }
       
     hdnSS.value=0;           
     validate_click();                     
}


function checkNumber1(thi,e)
{
    if ( (event.keyCode < 47 || event.keyCode > 57) )
	{
	    return false;
    } 
    if(thi.value >9)
    {
        return false;
    }  
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




function acctautocompleteselected(source, eventArgs) {


    var hiddenfieldID = source.get_id().replace("txtacct_AutoCompleteExtender", "txtacctCode");
    var hiddenfieldID1 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtacct");
    var hiddenfieldID2 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtcurrencycode");
    var hiddenfieldID3 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtConvRate");
    var hiddenfieldID4 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtcontrolacctcode");
    var hiddenfieldID5 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtcontrolacct");
    var hiddenfieldID6 = source.get_id().replace("txtacct_AutoCompleteExtender", "ddlType");

    var hiddenfieldID9 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtCredit");
    var hiddenfieldID10 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtBaseCredit");
    var hiddenfieldID11 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtDebit");
    var hiddenfieldID12 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtBaseDebit");

    var hiddenfieldID16 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtSrcCtryName_AutoCompleteExtender");
    var hiddenfieldID17 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtsourcectrycode");
    var hiddenfieldID18 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtsource");
    var hiddenfieldID19 = source.get_id().replace("txtacct_AutoCompleteExtender", "txtcurrencyname");


    $get(hiddenfieldID).value = eventArgs.get_value();
    var txtdivauto = document.getElementById("<%=txtDivCode.ClientID%>");
    var strtp = document.getElementById(hiddenfieldID6).value;
    var codeid = eventArgs.get_value();
    var sqlstr1;

    if (strtp == 'C') {
        var contxt = strtp + '||' + codeid;

        $find(hiddenfieldID16).set_contextKey(contxt);
    }

    if (strtp != '[Select]') {
            txtAConAccNm = hiddenfieldID5;
            txtAConAcc = hiddenfieldID4;
            txtfill1 = hiddenfieldID2;
            txtcurrname = hiddenfieldID19;
            txtgrdcrate = hiddenfieldID3;
            txtctryname = hiddenfieldID18;
            txtctrycode = hiddenfieldID17;
   
           
            FillCustDetails(strtp, codeid);


        }

        // Auto Calculate Forex / Provision Cash Collection  Param 04/01/2020
        if (strtp == 'G') {

            var divisionCode = document.getElementById("<%=txtDivCode.ClientId%>");
            var sqlstr1;
            sqlstr1 = "select isnull(acctcode,'') as acctcode from AcctAutomatCalculate where acctCode='" + codeid + "' and div_code ='" + divisionCode.value + "'";

            ColServices.clsServices.GetQueryReturnStringnew(constr, sqlstr1, fillAutoCalculate, ErrorHandler, TimeOutHandler);

            var TotalBaseDebit = document.getElementById("<%=txtTotBaseDebit.ClientId%>");
            var TotalBaseCredit = document.getElementById("<%=txtTotBaseCredit.ClientId%>");
            var TotalBaseDiff = document.getElementById("<%=txtTotBaseDiff.ClientId%>");

            function fillAutoCalculate(result) {

                if (result == codeid) {
                    $get(hiddenfieldID9).value = 0;
                    $get(hiddenfieldID10).value = 0;
                    $get(hiddenfieldID11).value = 0;
                    $get(hiddenfieldID12).value = 0;
                    grdTotal()
                    if ((TotalBaseDebit.value != TotalBaseCredit.value) && (parseFloat(TotalBaseDebit.value) != 0.0 || parseFloat(TotalBaseCredit.value) != 0.0)) {                        
                        if (TotalBaseDebit.value > TotalBaseCredit.value) {
                            $get(hiddenfieldID11).value = 0;
                            $get(hiddenfieldID12).value = 0;
                            $get(hiddenfieldID9).value = TotalBaseDiff.value ;
                            $get(hiddenfieldID10).value = TotalBaseDiff.value;
                            TotalBaseCredit.value = TotalBaseDebit.value;                          
                        }
                        else {
                            $get(hiddenfieldID11).value = TotalBaseDiff.value * -1;
                            $get(hiddenfieldID12).value = TotalBaseDiff.value * -1;
                            $get(hiddenfieldID9).value = 0;
                            $get(hiddenfieldID10).value = 0;
                            TotalBaseDebit.value = TotalBaseCredit.value;
                        }
                        TotalBaseDiff.value = '';
                    }
                }
            }
        }
    }



//           FillCustDetails(strtp1, codeid1);

//           txtfill1 = hiddenfieldID2;
//           document.getElementById(txtfill1).value = 'AED';
          
//           FillGAName(hiddenfieldID, hiddenfieldID1, hiddenfieldID2, hiddenfieldID3, hiddenfieldID4, hiddenfieldID5, hiddenfieldID6, hiddenfieldID9, hiddenfieldID10, hiddenfieldID11, hiddenfieldID12, hiddenfieldID16, hiddenfieldID17, hiddenfieldID18, hiddenfieldID19);

//                if (document.getElementById(hiddenfieldID6).value != 'G') {

//                    GetCountryDetails(eventArgs.get_value(), hiddenfieldID6, hiddenfieldID16, hiddenfieldID17, hiddenfieldID18);
//                }


          


            function GetCountryDetails(CustCode, Acctype, SrcCtryautoComp, srcctrycode, srcname) {

                Acctype = document.getElementById(Acctype).value;


                $.ajax({
                    type: "POST",
                    url: "ReceiptsNew.aspx/GetCountryDetails",
                    data: '{CustCode:  "' + CustCode + '",Acctype: "' + Acctype + '",SrcCtryautoComp:" ' + SrcCtryautoComp + '",srcctrycode :"' + srcctrycode + '",srcname :"' + srcname + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var xmlDoc = $.parseXML(response.d);
                        var xml = $(xmlDoc);

                        var Countries = xml.find("Countries");

                        var rowCount = Countries.length;


                        if (rowCount == 1) {

                            $.each(Countries, function () {
                                document.getElementById(srcname).value = ''
                                document.getElementById(srcctrycode).value = '';
                                document.getElementById(srcname).value = $(this).find("ctryname").text();
                                document.getElementById(srcctrycode).value = $(this).find("ctrycode").text();
                                document.getElementById(srcname).setAttribute("readonly", true);

                                //                            $find(SrcCtryautoComp).setAttribute("Enabled", false);

                            });
                        }
                        else {

                            //                        $find(SrcCtryautoComp).setAttribute("Enabled", true);
                            //                        $find('txtSrcCtryName_AutoCompleteExtender').setAttribute("Enabled", true);

                            document.getElementById(srcname).value = ''
                            document.getElementById(srcctrycode).value = '';
                            document.getElementById(srcname).removeAttribute("disabled");
                            document.getElementById(srcname).removeAttribute("readonly");
                        }

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
            }
            

            function FillGAName(ddlIccd, ddlIcnm, txtcurr, txtrate, ddlIContAcc, ddlConAccnm, ddltp, txtcramt, txtcrbaseamt, txtdbamt, txtdbbaseamt, txtSrcCtryName_AutoCompleteExtender, txtSrcctrycode, txtsrcctryname,txtcurrname1) {
               
                txtgrdDebAmt = txtdbamt;
                txtgrdDbBaseAmt = txtdbbaseamt;
                txtgrdCrAmt = txtcramt;
                txtgrdCrBaseAmt =txtcrbaseamt;
                txtfill1 =txtcurr;
                txtgrdcrate =txtrate;
                ddlIccode =document.getElementById(ddlIccd);
                ddlIcname =document.getElementById(ddlIcnm);
                txtAConAcc =ddlIContAcc;
                txtAConAccNm =ddlConAccnm;
                ddltyp =document.getElementById(ddltp);
              
                txtcurrname =txtcurrname1;

               
                var txtdivauto = document.getElementById("<%=txtDivCode.ClientID%>");
             
                var strtp = ddltyp.value;
                var codeid = ddlIccode.value;
          
                var sqlstr1, sqlstr2

                txtsrcctryname.disabled = false;
       
                if (strtp == 'C') {

                    sqlstr1 = " select  view_account.controlacctcode, acctmast.acctname   from view_account  join acctmast on  view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and  view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
                      var contxt = strtp + '||' + codeid;
             
                    $find(txtSrcCtryName_AutoCompleteExtender).set_contextKey(contxt);


                }

                else if (strtp == 'S') {

                    sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  and partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"

}
                else if (strtp == 'A') {
                    sqlstr1 = " select distinct supplier_agents.controlacctcode, acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" + txtdivauto.value + "'  and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"
      
                    //                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where  acctmast.div_code='" + txtdivauto.value + "'  and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' order by acctmast.acctname"
}
                else if (strtp == 'G') {

//                    txtsrcctryname.disabled = true;
//                    ddlIContAcc.value = '';
//                    ddlConAccnm.value = ' ';
//                    ddlIContAcc.disabled = true;
//                    ddlConAccnm.disabled = true;

 }


                if (strtp != '[Select]') {

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value
                    if (strtp != 'G') {

                        // ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr1,2, FillControlAcc, ErrorHandler, TimeOutHandler);
                        strtp1 = strtp;
                        codeid1 = codeid;
                        setTimeout1();

    }
                    //
                    FillCustDetails(strtp1, codeid1);
                    
                }
                else {
                    alert('Please Select Account Type');

                    ddlIccode.value = "";
                    ddlIcname.value = "";

                }
            }



        function FillControlAcc(result) {
         
            if (result != '') {
                //                

                document.getElementById(txtAConAcc).value = result[0];

                
//                txtAConAcc.disabled = true;


                document.getElementById(txtAConAccNm).value = result[1];
            
//                txtAConAccNm.disabled = true;
//             
            }
            else {
                txtAConAccNm.value = '';
                txtAConAcc.value = '';
            }
            
        }
         function FillCustDetails(typ, codeid) {
        
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value
            var crdsqlstr;
            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>");

            if (typ == 'G') {
                crdsqlstr = "select cur,currrates.convrate,'',currmast.currname,'' ,'',''    from  view_account   left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) left join currmast on view_account.cur=currmast.currcode  where   view_account.div_code='" + txtdiv.value + "' and code = '" + codeid + "' and type='" + typ + "' ";
            }
            else {
               crdsqlstr = "select cur,currrates.convrate,controlacctcode,currmast.currname,acctmast.acctname ,view_account.ctrycode,ctrymast.ctryname    from  view_account  left join ctrymast on view_account.ctrycode=ctrymast.ctrycode left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) left join currmast on view_account.cur=currmast.currcode  left join acctmast  on view_account.controlacctcode=acctmast.acctcode and view_account.div_code=acctmast.div_code where   view_account.div_code='" + txtdiv.value + "' and code = '" + codeid + "' and type='" + typ + "' ";
            }
            ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 7, FiilCustDt, ErrorHandler, TimeOutHandler);


            if (typ == 'C') {

                var sqlstr = "select count(agentcode) From agentmast_countries where agentcode='" + codeid + "' group by agentcode"

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr, "1", fnFillcustomer, ErrorHandler, TimeOutHandler);



            }

        }

        function fnFillcustomer(result) {
            if (result[0] != 1) {
                document.getElementById(txtctryname).value = '';
                document.getElementById(txtctrycode).value = '';

                document.getElementById(txtctryname).disabled = false;

            }

            else {
                document.getElementById(txtctryname).disabled = true;
            }
        }

        function changedate(trandate, chkdate) {

            trandate = document.getElementById(trandate);

            chkdate = document.getElementById(chkdate);

            chkdate.value = trandate.value;
        }

        function FiilCustDt(result) {
    
              document.getElementById(txtAConAccNm).value = result[4];
             document.getElementById(txtAConAcc).value = result[2];
             document.getElementById(txtfill1).value = result[0];
             document.getElementById(txtcurrname).value = result[3];
             document.getElementById(txtgrdcrate).value = result[1];
             document.getElementById(txtctryname).value = result[6];
             document.getElementById(txtctrycode).value = result[5];      
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

//            ValidateMarketCode(codeid);
        }

        function SrcCtryautocompleteselectedControl(source, eventArgs) {


            var hiddenfieldID = source.get_id().replace("txtSrcCtryName_AutoCompleteExtender", "txtsourcectrycode");


            $get(hiddenfieldID).value = eventArgs.get_value();
            //              alert(eventArgs.get_value(1));


        }

</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(function () {

                });

                prm.add_endRequest(function () {
                    MyAutoCustomerFillArray();

                });




            </script>
            <table class="td_cell">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" colspan="5">
                            <asp:Label ID="lblHeading" runat="server" Text="Journal Voucher" CssClass="field_heading"
                                Width="731px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="height: 4px; width: 93px;">
                                            <asp:Label ID="Label4" runat="server" Text=" JournalNo" 
                                                CssClass="field_caption" Width="64px"></asp:Label>
                                        </td>
                                        <td style="height: 4px; " colspan=5>

                                        <table>
                                        <tr>
                                        <td><input  id="txtDocNo" class="field_input" readonly type="text"
                                                maxlength="50" width="171px" runat="server" /></td>
                                                <td>  
                                                    <asp:Label ID="Label5" runat="server" class="field_input"
                                                Text=" Tran Type" Width="70px"></asp:Label></td>
                                                <td><asp:TextBox ID="txtTranType" runat="server" class="field_input" Enabled="False" Width="41px"></asp:TextBox></td>
                                                <td> 
                                                    <asp:Label ID="Label3" runat="server" CssClass="field_caption" 
                                                Text=" Journal Date" Width="76px" ></asp:Label></td>
                                                <td>
                                                <asp:TextBox ID="txtJDate" runat="server" CssClass="fiel_input" TabIndex="1" 
                                                ValidationGroup="MKE" Width="80px"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" 
                                                DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" 
                                                MaskType="Date" MessageValidatorTip="true" TargetControlID="txtJDate" />
                                            <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" 
                                                PopupButtonID="ImgBtnFrmDt" TargetControlID="txtJDate" />
                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="2" />
                                            <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" 
                                                ControlExtender="MskFromDate" ControlToValidate="txtJDate" 
                                                CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
                                                InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" 
                                                InvalidValueMessage="Invalid Date" 
                                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" 
                                                Width="23px">
                                            </cc1:MaskedEditValidator>
                                                
                                                
                                                </td>


                                                <td>&nbsp <asp:Label ID="Label6" runat="server" CssClass="field_caption" 
                                                Text=" Transaction Date" ></asp:Label></td>
                                                <td>
                                                
                                                <asp:TextBox ID="txtTDate" runat="server" CssClass="fiel_input" TabIndex="3" 
                                                ValidationGroup="MKE" Width="80px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="ClExChequeDate" runat="server" Format="dd/MM/yyyy" 
                                                PopupButtonID="ImageButton1" TargetControlID="txtTdate" />
                                            <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" AcceptNegative="Left" 
                                                DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" 
                                                MaskType="Date" MessageValidatorTip="true" TargetControlID="txtTdate" />
                                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                                            <cc1:MaskedEditValidator ID="MskVToDt" runat="server" 
                                                ControlExtender="MskChequeDate" ControlToValidate="txtTdate" 
                                                CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
                                                InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
                                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" 
                                                Width="23px">
                                            </cc1:MaskedEditValidator>
                                            <div style ="padding-left:10px;"><asp:Label ID="lbldate" runat="server" Text="Posting date" Font-Size="X-Small" ForeColor="Red" Width="70px" ></asp:Label>
                                             </div>
                                                
                                                </td>
                                        
                                        </tr>
                                        
                                        </table>

                                           
                                          
                                            
                                           
                                            


                                        </td>
                                        <td style="height: 4px">
                                            &nbsp;</td>
                                        <td style="height: 4px">
                                            
                                        </td>
                                        <td style="height: 4px">
                                            
                                        </td>
                                        <td style="height: 4px">
                                             &nbsp;</td>
                                        <td style="height: 4px">
                                            &nbsp;</td>
                                        <td style="height: 4px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 24px; width: 93px;">
                                            <asp:Label ID="Label2" runat="server" Text=" Reference " CssClass="field_caption"
                                                Width="75px"></asp:Label>
                                        </td>
                                        <td style="height: 24px" colspan="4">
                                            <input style="width: 171px" id="txtReference"  class="field_input" tabindex="5" type="text"
                                                maxlength="50" runat="server" />
                                        </td>
                                        <td style="height: 24px">
                                            &nbsp;</td>
                                        <td style="height: 24px">
                                            &nbsp;</td>
                                        <td style="height: 24px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr >
                                        <td style="width: 93px; height: 22px">
                                            <asp:Label ID="Label1" runat="server" CssClass="field_caption" Height="16px" 
                                                Text=" Narration" Width="62px"></asp:Label>
                                        </td>
                                        <td style="height: 22px" colspan="3">
                                           
                                                 <asp:TextBox ID="txtnarration" TextMode="MultiLine" runat="server" CssClass="field_input"
                                                            Enabled="True" MaxLength="200"  Height="80px" TabIndex="6" Width="568px"></asp:TextBox>
                                                             <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtnarration" ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{0,200}$" runat="server" ErrorMessage="Maximum 200 characters allowed."></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="width: 211px; height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 211px; height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 211px; height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 211px; height: 22px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 1px; width: 93px;">
                                            &nbsp;</td>
                                        <td style="height: 1px; text-align: center;" colspan="3">
                                            <asp:Label ID="lblPostmsg" runat="server" BackColor="#E0E0E0" 
                                                CssClass="field_caption" Font-Bold="True" Font-Names="Verdana" Font-Size="12px" 
                                                ForeColor="Green" Text="UnPosted" Width="155px"></asp:Label>
                                        </td>
                                        <td style="height: 1px">
                                            &nbsp;</td>
                                        <td style="height: 1px">
                                            &nbsp;</td>
                                        <td style="height: 1px">
                                            &nbsp;</td>
                                        <td style="height: 1px">
                                            &nbsp;</td>
                                    </tr>
                                  
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="container">
                                <asp:GridView ID="grdJournal" TabIndex="7" runat="server" Font-Size="10px" CssClass="td_cell"
                                    Width="600px" BackColor="White" AutoGenerateColumns="False" BorderColor="#999999"
                                    BorderStyle="None" CellPadding="3" GridLines="Vertical">
                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Type">
                                           
                                            <ItemTemplate>
                                                <select id="ddlType" runat="server" class="field_input MyAutoCompleteTypeClass" style="width: 55px"
                                                    tabindex="0">
                                                </select>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle VerticalAlign="middle" Width="4%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Account"  >
                                          
                                            <ItemTemplate>
                                              <asp:TextBox ID="txtacct" runat="server" AutoPostBack="false" class="field_input"
                                                                    MaxLength="500" TabIndex="14"    height="20px"  Width="215px" ></asp:TextBox>
                                                                <asp:TextBox ID="txtacctCode" style="display:none;" runat="server"  ></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:AutoCompleteExtender ID="txtacct_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="GetGAcclist" contextkey=true TargetControlID="txtacct" OnClientItemSelected="acctautocompleteselected">
                                                                </asp:AutoCompleteExtender><br />
                                                                 <asp:Label ID="Lblcontrol" runat="server"  Text="Control A/c:" CssClass="field_Caption"></asp:Label>
                                                                 <br />
                                                                 <asp:TextBox ID="txtcontrolacctcode"  style="display:none;"   runat="server"></asp:TextBox>
                                             
                                                                   <asp:TextBox ID="txtcontrolacct"   Width="215px"  class="field_input"  runat="server"></asp:TextBox>
                                               
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="bottom" Width="5%" />
                                        </asp:TemplateField>
                            
                                        
                                        <asp:TemplateField HeaderText="Cost Center" Visible ="false" >
                                            <ItemTemplate>
                                                 <asp:TextBox ID="txtcostcentercode" runat="server"></asp:TextBox>
                                                 <asp:TextBox ID="txtcostcentername" CssClass="field_input" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle Width="40px" />
                                            <ItemStyle Width="40px" />
                                        </asp:TemplateField>
                                         
                                        <asp:TemplateField HeaderText="Narration">
                                           
                                            <ItemTemplate>
                                               <asp:TextBox ID="txtnarr" Width="250px"   TextMode="MultiLine"   MaxLength="200" CssClass="field_input" runat="server"    height="60px" 
        ></asp:TextBox>
        <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtnarr" ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{0,200}$" runat="server" ErrorMessage="Maximum 200 characters allowed."></asp:RegularExpressionValidator>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="BookingNo">
                                            
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtbookingno" style="width: 80px" CssClass="field_input" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        

                                       
                                        <asp:TemplateField HeaderText="Currency">
                                         
                                            <ItemTemplate> 
                                             <input id="txtcurrencycode" runat="server" class="field_input" disabled="true"   maxlength="50" style="width: 80px"
                                                    type="text" />                                               
                                                
                                                    <asp:TextBox ID="txtcurrencyname"  Style="display: none"   runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exchg. Rate">
                                          
                                            <ItemTemplate>
                                                <input id="txtConvRate" runat="server" class="field_input" maxlength="50" style="width: 75px"
                                                    type="text" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                         
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 100px; text-align: right" id="txtDebit" class="field_input" type="text"
                                                    maxlength="12" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                           
                                         
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 100px; text-align: right" id="txtCredit" class="field_input"
                                                    type="text" maxlength="12" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BaseCurr Debit">
                                         
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 100px; text-align: right" id="txtBaseDebit" class="field_input"
                                                    type="text" maxlength="50" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BaseCurr Credit">
                                           
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 100px; text-align: right" id="txtBaseCredit" class="field_input"
                                                    type="text" maxlength="50" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Source Country">
                                                <ItemTemplate>
                                                  <asp:TextBox ID="txtsourcectrycode" runat="server"  style=" display: none"   ></asp:TextBox> <%--"--%>
                                                  <asp:TextBox ID="txtsource" runat="server"  Enabled="false" CssClass="field_input" 
                                                                                                    Width="100px" style="text-transform:uppercase;" onclick="javascript: this.select();" ></asp:TextBox>
                                                                                                     <asp:AutoCompleteExtender   ID="txtSrcCtryName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                                                    UseContextKey ="True" ServiceMethod="GetSrcCtrylist" 
                                                                                                    TargetControlID="txtsource" ServicePath ="" OnClientItemSelected="SrcCtryautocompleteselectedControl">  
                                                                                                    </asp:AutoCompleteExtender>
        
                                                   <%-- <select style="width: 100px" id="ddldept" class="field_input" runat="server">
                                                        <option selected></option>
                                                    </select>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Adjust Bill">
                                            <ItemTemplate>
                                                <input style="width: 35px" id="btnAd" class="field_button" type="button" value="A.B"
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chckDeletion" runat="server" Width="5px"></asp:CheckBox>
                                                <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtOldLineno" type="text" runat="server" />
                                                    <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 44px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtlineno" class="field_input" readonly type="text"
                                                    maxlength="50" value='<%# Bind("LineNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                     
                                       
                                  
                                       
                                    </Columns>
                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                    <SelectedRowStyle BackColor="#008A8C" Font-Size="10px" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center">
                                    </PagerStyle>
                                    <HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                            <asp:Button ID="btnAdd" TabIndex="8" runat="server" Text="Add Row" CssClass="field_button"
                                Font-Bold="True"></asp:Button>&nbsp;<asp:Button ID="btnDelLine" TabIndex="9" OnClick="btnDelLine_Click"
                                    runat="server" Text="DeleteRow" CssClass="field_button" >
                                </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="height: 89px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td colspan="2" style="height: 32px">
                                            <input style="visibility: hidden; width: 67px" id="txtdecimal" type="text" maxlength="15"
                                                runat="server" />
                                            <input style="visibility: hidden; width: 29px" id="txtbasecurr" type="text" maxlength="20"
                                                runat="server" />
                                            <input style="visibility: hidden; width: 47px" id="txtgridrows" type="text" runat="server" />
                                            <input style="visibility: hidden; width: 29px" id="txtMode" type="text" maxlength="20"
                                                runat="server" />
                                   
                                                 <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtDivCode" type=text maxLength=15 runat="server" />
                                            <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
                                            <input style="visibility: hidden; width: 65px" id="txtGridType" type="text" maxlength="20"
                                                runat="server" />
                                        </td>
                                        <td style="height: 32px">
                                            <asp:Label ID="Label7" runat="server" Text=" Total" CssClass="filed_caption" Width="98px"></asp:Label>
                                        </td>
                                        <td style="width: 100px; height: 32px">
                                            <input style="width: 80px; text-align: right" id="txtTotalDebit" class="field_input"
                                                readonly type="text" maxlength="50" runat="server" tabindex="42" />
                                        </td>
                                        <td style="width: 100px; height: 32px">
                                            <input style="width: 80px; text-align: right" id="txtTotalCredit" class="field_input"
                                                readonly type="text" maxlength="50" runat="server" tabindex="43" />
                                        </td>
                                        <td style="width: 100px; height: 32px">
                                            <asp:Label ID="lblBaseTot" runat="server" Text=" Total" CssClass="filed_caption"
                                                Width="98px"></asp:Label>
                                        </td>
                                        <td style="width: 100px; height: 32px">
                                            <input style="width: 113px; text-align: right" id="txtTotBaseDebit" class="field_input"
                                                readonly type="text" maxlength="100" runat="server" tabindex="44" />
                                        </td>
                                        <td style="width: 100px; height: 32px">
                                            <input style="width: 113px; text-align: right" id="txtTotBaseCredit" class="field_input"
                                                readonly type="text" maxlength="100" runat="server" tabindex="45" />
                                        </td>
                                        <td style="width: 114px; height: 32px" align="right" class="td_cell">
                                            <asp:Label ID="lblBaseDiff" runat="server" CssClass="filed_caption" Text=" Total"></asp:Label>
                                        </td>
                                        <td style="width: 100px; height: 32px">
                                            <input style="width: 113px; text-align: right" id="txtTotBaseDiff" class="field_input"
                                                readonly type="text" maxlength="100" runat="server" tabindex="46" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                        <td style="width: 100px; height: 26px">
                                        </td>
                                        <td style="width: 100px; height: 26px">
                                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                                height: 9px" type="text" />
                                        </td>
                                        <td>
                                            <input style="visibility: hidden; width: 29px" id="txtAdjcolno" type="text" maxlength="20"
                                                runat="server" />
                                        </td>
                                        <td style="width: 100px; height: 26px">
                                        </td>
                                        <td style="width: 100px; height: 26px">
                                        </td>
                                        <td style="width: 114px; height: 26px">
                                        </td>
                                        <td style="width: 100px; height: 26px">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5" style="height: 30px">
                            <table>
                                <tr>
                                    <td style="font-weight: bold; width: 326px; color: black; height: 26px">
                                        &nbsp;</td>
                                    <td style="width: 100px; height: 26px">
                                        <asp:CheckBox ID="chkadjust" runat="server" CssClass="filed_caption" Font-Bold="True"
                                            Text="Allow any way" Visible="False" Width="121px" TabIndex="48" />
                                    </td>
                                    <td style="width: 100px; height: 26px">
                                        <asp:CheckBox ID="chkPost" TabIndex="10" runat="server" Text="Post/UnPost" Font-Size="10px"
                                            Font-Names="Verdana" CssClass="field_caption" Width="103px" BackColor="#FFC0C0"
                                            Font-Bold="True" ForeColor="Black"></asp:CheckBox>
                                    </td>
                                    <td style="width: 58px; height: 26px">
                                        <asp:Button ID="btnSave" TabIndex="11" runat="server" Text="Save" OnClientClick="return ValidatePage();"
                                            CssClass="field_button" Font-Bold="True"></asp:Button>
                                    </td>
                                    <td style="width: 47px; height: 26px">
                                        <asp:Button ID="btnPrint" TabIndex="12" runat="server" Text="Print" CssClass="field_button">
                                        </asp:Button>
                                    </td>
                                                                        <td style="width: 47px; height: 26px">
                                        <asp:Button ID="btnPdfReport" TabIndex="12" runat="server" Text="Pdf Report" CssClass="field_button">
                                        </asp:Button>
                                    </td>
                                    <td style="width: 44px; height: 26px">
                                        <asp:Button ID="btnExit" TabIndex="13" runat="server" Text="Exit" CssClass="field_button"
                                            Font-Bold="True"></asp:Button>
                                    </td>
                                    <td style="width: 51px; height: 26px">
                                        <asp:Button ID="btnhelp" TabIndex="53" OnClick="btnhelp_Click" runat="server" Text="Help"
                                            CssClass="field_button"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                            &nbsp; &nbsp; &nbsp;
                        </td>
                    </tr>
                      <tr>
                                        <td style="width: 93px;">
                                            <asp:Label ID="lblNoofRows" runat="server" CssClass="field_caption" 
                                                Text="Enter No Of Rows" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <input id="txtNoofRows" class="field_input" onkeypress="return checkNumber1(this,event);"
                                                tabindex="8" type="text" maxlength="50" runat="server" size="5" style="text-align: right;
                                                width: 55px;" visible="false" />
                                            <asp:Button ID="btnGenGrid" runat="server" CssClass="btn" 
                                                 OnClientClick=" return Validaterowno();" TabIndex="9" 
                                                Text="Genarate Grid" Visible="False" />
                                        </td>
                                    </tr>
                </tbody>
            </table>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                </Services>
            </asp:ScriptManagerProxy>
            <asp:HiddenField ID="hdnRows" runat="server" />
            <asp:HiddenField ID="hdnSS" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

