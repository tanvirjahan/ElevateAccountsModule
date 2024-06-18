<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Exchange.aspx.vb" Inherits="Exchange"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"    TagPrefix="ews" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript">
//----------------------------------------
var nodecround =null;
var txtgrdcrate=null;
var txtfill=null;
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

function fill_acountcode(ddltp,ddlcode,ddlname,ddlConAccd,ddlConAccnm,txtgnarr,ddlcostcd,ddlcostnm,txthid) {

   
 
    ddltyp = document.getElementById(ddltp);
    var strtp = ddltyp.value;
    ddlACode = document.getElementById(ddlcode);
    ddlAName = document.getElementById(ddlname);
    
    ddlAConAcc = document.getElementById(ddlConAccd);
    ddlAConAccNm = document.getElementById(ddlConAccnm);
  
    ddlcostcode = document.getElementById(ddlcostcd);
    ddlcostname = document.getElementById(ddlcostnm);
    txthid = document.getElementById(txthid);
    
     
     var sqlstr1=null;
  var sqlstr2=null;
  var sqlstr3=null;
  var sqlstr4 =null;
  
  if (strtp!='[Select]')
  {
    sqlstr1="select Code,des from view_account where type = '"+ strtp +"' order by code";
    sqlstr2="select des,Code from view_account where type = '"+ strtp +"' order by des";
    sqlstr3="select isnull(controlacctcode,0),code from view_account where type= '"+ strtp +"' order by code";
    sqlstr4=" select distinct acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '"+ strtp +"' order by  acctmast.acctname";
 }
 else
 {
     sqlstr1="select top 10 Code,des from view_account   order by code";
    sqlstr2="select top 10  des,Code from view_account   order by des";
    sqlstr3="select top 10  isnull(controlacctcode,0),code from view_account   order by code";
    sqlstr4=" select distinct top 10   acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  order by  acctmast.acctname";
 }
    
    
     
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   


    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillACodes,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr2,FillANames,ErrorHandler,TimeOutHandler);
    
    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr3,FillControlAcc,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr4,FillControlAccName,ErrorHandler,TimeOutHandler);
  
  
        
    txtgnarrs = document.getElementById(txtgnarr);
    txtnarr = document.getElementById("<%=txtnarration.ClientID%>" );
    txtgnarrs.value=trim(txtnarr.value);
  
    ddlcostcode.disabled=false;
    ddlcostname.disabled=false;
    if (strtp=='G')
    {
     ddlcostcode.disabled=false;
     ddlcostname.disabled=false;
//     txthid.value='N'
     var GridViewValue = document.getElementById('<%=GVCollection.ClientID%>');

     GridViewValue.style.visibility = "visible";

     
    }
    else
    {
     ddlcostcode.disabled=true;
     ddlcostname.disabled=true;
     var GridViewValue = document.getElementById('<%=GVCollection.ClientID%>');

     GridViewValue.style.visibility = "hidden"; 
//     txthid.value='Y'   
    }
 
//    if (txthid.value=='Y')
//    {
//      var objGridView = document.getElementById('<%=GVCollection.ClientID%>');
//      objGridView.style.visibility="hidden"; 
//    }
//    else
//    {      
//          var objGridView = document.getElementById('<%=GVCollection.ClientID%>');
//           objGridView.style.visibility="visible"; 
//    }
}
function FillGACode(ddlIccd,ddlIcnm,txtcurr,txtrate,ddlIContAcc,ddlConAccnm,ddltp,txtaccd,txtacnm,txtcramt,txtcrbaseamt,txtdbamt,txtdbbaseamt,txtcontcd,txtcontnm)
{   
 
  txtacccode = document.getElementById("<%=txtaccCode.ClientID%>" );
  txtacctype  = document.getElementById("<%=txtacctype .ClientID%>" );
  
  txtgrdDebAmt = document.getElementById(txtdbamt);
  txtgrdDbBaseAmt = document.getElementById(txtdbbaseamt);
  txtgrdCrAmt = document.getElementById(txtcramt);
  txtgrdCrBaseAmt = document.getElementById(txtcrbaseamt);

  
    ddltyp = document.getElementById(ddltp);
    var strtp = ddltyp.value;
    txtacctype.value =strtp
    txtacccode.value =ddlIccd.value
    
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
    ddlAConAcc = document.getElementById(ddlIContAcc);  
    ddlAConAccNm = document.getElementById(ddlConAccnm);  
    txtAConAcc = document.getElementById(txtcontcd);  
    txtAConAccNm = document.getElementById(txtcontnm);

    var codeid=ddlIccode.options[ddlIccode.selectedIndex].text;
    ddlIcname.value=codeid;
    
    txtaccd = document.getElementById(txtaccd);  
    txtacnm = document.getElementById(txtacnm); 
    txtaccd.value=ddlIccode.options[ddlIccode.selectedIndex].value;
    txtacnm.value=codeid;
    
   
    txtfill = document.getElementById(txtcurr);
    txtgrdcrate=document.getElementById(txtrate); 
    
//    sqlstr="select cur,cur from view_account where Code='" + ddlIcname.value + "'" ;
//    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurr,ErrorHandler,TimeOutHandler);
//   
//   
//    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
//    sqlstr="select convrate from currrates ,view_account  where  currrates.currcode=view_account.cur and   view_account.code='"+ ddlIcname.value +"' and Tocurr='"+ txtbase.value +"'"
//    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillGrdCvntRate,ErrorHandler,TimeOutHandler);
    
    
    var sqlstr1,sqlstr2
     ddlAConAcc.disabled=false;
     ddlAConAccNm.disabled=false;
    if (strtp=='C')
    {
    sqlstr1=" select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '"+ strtp +"' and view_account.code='"+ codeid+"' order by  view_account.controlacctcode";
    sqlstr2=" select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '"+ strtp +"' and view_account.code='"+ codeid+"' order by  acctmast.acctname";
    txtacctype.value =strtp
    txtacccode.value =codeid   
    }
     else if (strtp=='S')
    {
    sqlstr1=" select distinct partymast.controlacctcode , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"' union all  select distinct partymast.accrualacctcode  controlacctcode , acctmast.acctname  from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"' order by controlacctcode"  
    
    sqlstr2=" select distinct acctmast.acctname ,partymast.controlacctcode   from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"' union all  select distinct acctmast.acctname ,partymast.accrualacctcode controlacctcode   from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"'  order by acctmast.acctname"  
  
   }
     else if (strtp=='A')
    {
    sqlstr1=" select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='"+ codeid+"' union all  select distinct supplier_agents.accrualacctcode controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='"+ codeid+"'  order by controlacctcode"  
    
    sqlstr2=" select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='"+ codeid+"' union all  select distinct acctmast.acctname ,supplier_agents.accrualacctcode controlacctcode from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='"+ codeid+"'  order by acctmast.acctname"  
    }
     else if (strtp=='G')
    {
     sqlstr1=" select ''  as controlacctcode, '' as acctname  "  
     sqlstr2=" select  '' as acctname , '' as controlacctcode "  
     ddlAConAcc.disabled=true;
     ddlAConAccNm.disabled=true;
    }
    
    if (strtp !='[Select]')
    {
     var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   

    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillControlAcc,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr2,FillControlAccName,ErrorHandler,TimeOutHandler);
    FillCustDetails(strtp,codeid);
     }
    else
    {
    alert('Please Select Account Type');
    }
    

}

function FillGAName(ddlIccd,ddlIcnm,txtcurr,txtrate,ddlIContAcc,ddlConAccnm,ddltp,txtaccd,txtacnm,txtcramt,txtcrbaseamt,txtdbamt,txtdbbaseamt,txtcontcd,txtcontnm)
{
  
    txtgrdDebAmt = document.getElementById(txtdbamt);
    txtgrdDbBaseAmt = document.getElementById(txtdbbaseamt);
    txtgrdCrAmt = document.getElementById(txtcramt);
    txtgrdCrBaseAmt = document.getElementById(txtcrbaseamt);
    
    ddltyp = document.getElementById(ddltp);
    var strtp = ddltyp.value;
    
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
    
    ddlAConAcc = document.getElementById(ddlIContAcc);  
    ddlAConAccNm = document.getElementById(ddlConAccnm);  
    txtAConAcc = document.getElementById(txtcontcd);  
    txtAConAccNm = document.getElementById(txtcontnm);
    
    var codeid=ddlIcname.options[ddlIcname.selectedIndex].value;
    ddlIccode.value=ddlIcname.options[ddlIcname.selectedIndex].text;
    
    txtaccd = document.getElementById(txtaccd);  
    txtacnm = document.getElementById(txtacnm); 
    txtaccd.value= ddlIcname.options[ddlIcname.selectedIndex].text;
    txtacnm.value=codeid;
    
    
    txtfill = document.getElementById(txtcurr);
    txtgrdcrate=document.getElementById(txtrate);
    
//    sqlstr="select cur,cur from view_account where Code='" + ddlIcname.value + "'" ;
//    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr,FillCurr,ErrorHandler,TimeOutHandler);
//    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
//    sqlstr="select convrate from currrates ,view_account  where  currrates.currcode=view_account.cur and   view_account.code='"+ ddlIcname.value +"' and Tocurr='"+ txtbase.value +"'"
//    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillGrdCvntRate,ErrorHandler,TimeOutHandler);
    
    var sqlstr1,sqlstr2
     ddlAConAcc.disabled=false;
     ddlAConAccNm.disabled=false;
    if (strtp=='C')
    {
    sqlstr1=" select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '"+ strtp +"' and view_account.code='"+ codeid+"' order by  view_account.controlacctcode";
    sqlstr2=" select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '"+ strtp +"' and view_account.code='"+ codeid+"' order by  acctmast.acctname";
    }
     else if (strtp=='S')
    {
    sqlstr1=" select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"' union all  select distinct partymast.accrualacctcode controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"' order by controlacctcode"  
    
    sqlstr2=" select distinct acctmast.acctname ,partymast.controlacctcode  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"' union all  select distinct acctmast.acctname ,partymast.accrualacctcode controlacctcode  from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='"+ codeid+"' order by acctmast.acctname"  
  
   }
     else if (strtp=='A')
    {
    sqlstr1=" select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='"+ codeid+"' union all  select distinct supplier_agents.accrualacctcode controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='"+ codeid+"' order by controlacctcode"  
    
    sqlstr2=" select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='"+ codeid+"' union all  select distinct acctmast.acctname ,supplier_agents.accrualacctcode controlacctcode     from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='"+ codeid+"' order by acctmast.acctname"  
    }
     else if (strtp=='G')
    {
     sqlstr1=" select ''  as controlacctcode, '' as acctname  "  
     sqlstr2=" select  '' as acctname , '' as controlacctcode "  
     ddlAConAcc.disabled=true;
     ddlAConAccNm.disabled=true;
    }
   if (strtp !='[Select]')
    { 
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   

    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillControlAcc,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr2,FillControlAccName,ErrorHandler,TimeOutHandler);
    FillCustDetails(strtp,codeid);
     }
    else
    {
    alert('Please Select Account Type');
    } 
}




function FillCustDetails(typ,codeid)
    {
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   

    var crdsqlstr="select cur,convrate,controlacctcode    from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '"+ codeid +"' and type='"+typ  +"' ";
    ColServices.clsServices.GetQueryReturnStringArraynew(constr,crdsqlstr,3,FiilCustDt,ErrorHandler,TimeOutHandler);

   } 
function FiilCustDt(result)
    {
    txtfill.value = result[0];
    txtgrdcrate.value =result[1];
    ddlAConAccNm.value =result[2];
    var codeid=ddlAConAccNm.options[ddlAConAccNm.selectedIndex].text;
    ddlAConAcc.value =codeid;    
    txtAConAcc.value=codeid;
    txtAConAccNm.value=result[2];
    
    GrdExchangeRateChange(result[1]);
    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
    
    if ( trim(txtfill.value)==trim(txtbase.value))
       {
        txtgrdcrate.readOnly=true; 
        txtgrdcrate.disabled=true;
       }
       else
       {
        txtgrdcrate.readOnly=false;
        txtgrdcrate.disabled=false;
       }
     
   } 





function FillACodes(result)
    {
     	RemoveAll(ddlACode)
        for(var i=0;i<result.length;i++)
        {
           
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlACode.options.add(option);
        }
        ddlACode.value="[Select]";
    }

function FillANames(result)
    {
       
 		RemoveAll(ddlAName)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlAName.options.add(option);
        }
        ddlAName.value="[Select]";
    }
    
function FillControlAcc(result)
    {
        RemoveAll(ddlAConAcc)
        for(var i=0;i<result.length;i++)
        {

            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlAConAcc.options.add(option);
        }
        ddlAConAcc.value="[Select]";
    }
    function FillControlAccName(result)
    {
        RemoveAll(ddlAConAccNm)
        for(var i=0;i<result.length;i++)
        {

            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlAConAccNm.options.add(option);
        }
        ddlAConAccNm.value="[Select]";
    }
 

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
    txtfill.value=result;
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
function FillCTCode(ddlIccd,ddlIcnm,txtcd,txtnm)
{
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
    
    var codeid=ddlIccode.options[ddlIccode.selectedIndex].text;
    ddlIcname.value=codeid;
    
    txtctd = document.getElementById(txtcd);  
    txtctnm = document.getElementById(txtnm); 
    txtctd.value= ddlIccode.options[ddlIccode.selectedIndex].value;
    txtctnm.value=codeid;
    
}

function FillCTName(ddlIccd,ddlIcnm,txtcd,txtnm)
{
 
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
    
    var codeid=ddlIcname.options[ddlIcname.selectedIndex].text;
    ddlIccode.value=codeid;
    
    txtctd = document.getElementById(txtcd);  
    txtctnm = document.getElementById(txtnm); 
    txtctd.value= codeid;
    txtctnm.value=ddlIcname.options[ddlIcname.selectedIndex].value;
    
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
 
function baseconvertInRate()
{
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
       
       var valDr=objGridView.rows[j].cells[10].children[0].value;
       var valCr=objGridView.rows[j].cells[11].children[0].value;
 
       var valbDr=objGridView.rows[j].cells[10].children[0].value;
       var valbCr=objGridView.rows[j].cells[11].children[0].value;
           
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
       totbDr=DecRound(totbDr)+DecRound(valbDr);
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

	function FillCodeName(ddlcode,ddlname)
{
    ddlc = document.getElementById(ddlcode);
    ddln = document.getElementById(ddlname);
    ddln.value=ddlc.options[ddlc.selectedIndex].text;
}

 function DecRoundEightPalces(amtToRound) 
{
  
  //var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
  nodecround=Math.pow(10,parseInt(8));
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
     alert('Debit and Credit should not blank...');
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
function Validaterowno()
{
    var txtNoofRows=document.getElementById("<%=txtNoofRows.ClientID%>");
    var hdnRows=document.getElementById("<%=hdnRows.ClientID%>");
    if(txtNoofRows.value =="")
    {
        alert('Enter valid No of rows');
        return false;    
    }
    else
    { 
    return true;
     /*   if(parseInt(txtNoofRows.value) < parseInt(hdnRows.value))
        {
            alert('There are already '+hdnRows.value+' no of row please enter '+hdnRows.value+' or more');
            return false;   
        }
        else
        {
           
        }*/
    }
} 

function chkmultiple(chksel)
{
  var  chk=document.getElementById(chksel);
  var objGridView = document.getElementById('<%=GVCollection.ClientID%>');
   var txtRowCount =document.getElementById('<%=txtRowCount.ClientID%>');
   intRows=txtRowCount.value ;
   var i=0;
   
  
    for(j=1;j<=intRows;j++)
    {
       if(objGridView.rows[j].cells[0].children[0].checked == true)
        {
         i+=1
        } 

    } 
    if (i>1) 
    {
     alert('Only one bill can adjust')
     chk.checked=false;
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
</script>

<asp:UpdatePanel id="UpdatePanel1" runat="server">
<contenttemplate>
<TABLE class="td_cell"><TBODY><TR><TD class="field_heading" align=center colSpan=5><asp:Label id="lblHeading" runat="server" Text="Exchange Difference" CssClass="field_heading" Width="731px"></asp:Label></TD></TR><TR><TD colSpan=5><TABLE><TBODY><TR><TD style="HEIGHT: 4px"><asp:Label id="Label4" runat="server" Text=" Tran id" CssClass="field_caption" Width="110px"></asp:Label></TD><TD style="HEIGHT: 4px"><INPUT style="WIDTH: 210px" id="txtDocNo" class="field_input" readOnly type=text maxLength=50 runat="server" /></TD><TD style="HEIGHT: 4px"><asp:Label id="Label5" runat="server" Text=" Tran Type" CssClass="field_caption" Width="110px"></asp:Label></TD><TD style="HEIGHT: 4px"><asp:TextBox id="txtTranType" runat="server" Width="173px" Enabled="False"></asp:TextBox></TD></TR><TR><TD><asp:Label id="Label3" runat="server" Text="Tran Date" CssClass="field_caption" Width="110px"></asp:Label></TD><TD style="WIDTH: 172px"><asp:TextBox id="txtJDate" tabIndex=1 runat="server" CssClass="fiel_input" Width="80px" ValidationGroup="MKE"></asp:TextBox> <asp:ImageButton id="ImgBtnFrmDt" tabIndex=3 runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVFromDt" runat="server" CssClass="field_error" Width="23px" ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtJDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator></TD><TD><asp:Label id="Label6" runat="server" Text=" Transaction Date" CssClass="field_caption" Width="110px"></asp:Label></TD><TD><asp:TextBox id="txtTDate" tabIndex=2 runat="server" CssClass="fiel_input" Width="80px" ValidationGroup="MKE"></asp:TextBox> <asp:ImageButton id="ImageButton1" tabIndex=3 runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVToDt" runat="server" CssClass="field_error" Width="23px" ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtTdate" ControlExtender="MskChequeDate"></cc1:MaskedEditValidator></TD></TR><TR><TD style="HEIGHT: 24px"><asp:Label id="Label2" runat="server" Text=" Reference " CssClass="field_caption" Width="110px"></asp:Label></TD><TD style="WIDTH: 172px; HEIGHT: 24px"><INPUT style="WIDTH: 210px" id="txtReference" class="field_input" tabIndex=3 type=text maxLength=50 runat="server" /></TD><TD align=center><asp:Label id="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana" CssClass="field_caption" Width="155px" BackColor="#E0E0E0" Font-Bold="True" ForeColor="Green"></asp:Label></TD><TD style="HEIGHT: 24px"></TD></TR><TR><TD style="HEIGHT: 1px"><asp:Label id="Label1" runat="server" Text=" Narration" CssClass="field_caption" Width="110px"></asp:Label></TD><TD style="HEIGHT: 1px" colSpan=3><SELECT style="WIDTH: 556px" id="ddlNarration" class="field_input" tabIndex=4 runat="server"></SELECT></TD></TR><TR><TD style="height: 24px"></TD><TD colSpan=3 style="height: 24px"><INPUT style="WIDTH: 551px" id="txtnarration" class="field_input" tabIndex=5 type=text maxLength=200 runat="server" /></TD></TR>
    <tr>
        <td style="height: 24px">
            <asp:Label ID="lblNoofRows" runat="server" CssClass="field_caption" Text="Enter No Of Rows"
                Width="110px" Visible="False"></asp:Label></td>
        <td colspan="3" style="height: 24px">
            <INPUT id="txtNoofRows" class="field_input" onkeypress="return checkNumber1(this,event);" tabIndex=6 type=text maxLength=50 runat="server" size="5" style="text-align: right" visible="false" />
            &nbsp; &nbsp;
            <asp:Button ID="btnGenGrid" runat="server" CssClass="field_button" OnClick="btnGenGrid_Click"
                TabIndex="7" Text="Genarate Grid" OnClientClick="return Validaterowno();" Visible="False" /></td>
    </tr>
</TBODY></TABLE></TD></TR><TR><TD colSpan=5><DIV class="container">
        <asp:GridView id="grdJournal" tabIndex=6 runat="server" Font-Size="10px" 
            CssClass="td_cell" Width="1430px" BackColor="White" AutoGenerateColumns="False" 
            BorderStyle="None" CellPadding="3" GridLines="Vertical">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField HeaderText="Type"><EditItemTemplate>
                                <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
                                <select id="ddlType" runat="server" class="field_input" style="width: 45px" tabindex="0">
                                </select>
                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 70px" id="ddlgAccCode" class="field_input" tabIndex=0 runat="server"></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="A/C Name">
<ItemTemplate>
<input type="text" name="accSearch"  class="field_input " style="width:98% ; font " id="accSearch"  runat="server" />
   
<SELECT style="WIDTH: 225px" id="ddlgAccName" class="field_input" tabIndex=0 runat="server"></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Control A/C Code"><ItemTemplate>
<SELECT style="WIDTH: 65px" id="ddlConAccCode" class="field_input" tabIndex=0 runat="server"></SELECT>
</ItemTemplate>
    <HeaderStyle Width="60px" />
    <ItemStyle Width="60px" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Control A/C Name"><ItemTemplate>
<SELECT style="WIDTH: 125px" id="ddlConAccName" class="field_input" tabIndex=0 runat="server"></SELECT>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost Center Code"><ItemTemplate>
<SELECT style="WIDTH: 55px" id="ddlCostCode" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT>
</ItemTemplate>
    <HeaderStyle Width="60px" />
    <ItemStyle Width="60px" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost Center Name"><ItemTemplate>
<SELECT style="WIDTH: 60px" id="ddlCostName" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT>
</ItemTemplate>
    <HeaderStyle Width="60px" />
    <ItemStyle Width="60px" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Narration"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 150px" id="txtgnarration" class="field_input" type=text maxLength=200 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Currency"><EditItemTemplate>
&nbsp;
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 40px" id="txtCurrency" class="field_input" readOnly type=text maxLength=50 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Exchg. Rate"><EditItemTemplate>
&nbsp;
</EditItemTemplate>
<ItemTemplate>
                                <input id="txtConvRate" runat="server" class="field_input" maxlength="50" style="width: 45px"
                                    type="text" />
                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Debit" Visible="False"><EditItemTemplate>
&nbsp; 
</EditItemTemplate>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<INPUT style="WIDTH: 70px; TEXT-ALIGN: right" id="txtDebit" class="field_input" type=text maxLength=12 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Credit" Visible="False"><EditItemTemplate>
<asp:TextBox id="TextBox7" runat="server"></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<INPUT style="WIDTH: 70px; TEXT-ALIGN: right" id="txtCredit" class="field_input" type=text maxLength=12 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="BaseCurr Debit"><EditItemTemplate>
&nbsp; 
</EditItemTemplate>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<INPUT style="WIDTH: 78px; TEXT-ALIGN: right" id="txtBaseDebit" class="field_input" type=text maxLength=50 runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="BaseCurr Credit"><EditItemTemplate>
&nbsp; 
</EditItemTemplate>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<INPUT style="WIDTH: 80px; TEXT-ALIGN: right" id="txtBaseCredit" class="field_input" type=text maxLength=50 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Txthid" Visible="False"><EditItemTemplate>
<asp:TextBox id="Txthid"  runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 26px ;VISIBILITY: hidden" id="txthid"  class="field_input" type=text maxLength=200 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>


<asp:TemplateField HeaderText="Adjust Bill" Visible="False"><ItemTemplate>
<INPUT style="WIDTH: 28px" id="btnAd" class="field_button" type=button value="A.B" runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Delete" Visible="False"><ItemTemplate>
<asp:CheckBox id="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>

<ControlStyle BackColor="White" BorderStyle="None" BorderColor="White"></ControlStyle>

<ItemStyle BackColor="White" BorderStyle="None" BorderColor="White"></ItemStyle>

<HeaderStyle  CssClass="grdheader" BorderColor="White"></HeaderStyle>
<ItemTemplate>
<INPUT style="VISIBILITY: hidden; BORDER-BOTTOM-COLOR: #eeeeee; WIDTH: 1px; BORDER-TOP-COLOR: #eeeeee; BORDER-RIGHT-COLOR: #eeeeee" id="txtOldLineno" type=text runat="server" />
</ItemTemplate>

<FooterStyle CssClass="grdfooter" ></FooterStyle>
</asp:TemplateField>
<asp:TemplateField>
<ControlStyle BorderStyle="None"></ControlStyle>

<ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>

<HeaderStyle  CssClass="grdheader" ></HeaderStyle>
<ItemTemplate>
<INPUT style="VISIBILITY: hidden; BORDER-BOTTOM-COLOR: #eeeeee; WIDTH: 1px; BORDER-TOP-COLOR: #eeeeee; BORDER-RIGHT-COLOR: #eeeeee" id="txtctrolaccode" class="field_input" readOnly type=text maxLength=50 value=" " runat="server" />
</ItemTemplate>

<FooterStyle BorderStyle="None"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField><EditItemTemplate>
&nbsp; 
</EditItemTemplate>

<ControlStyle BorderStyle="None"></ControlStyle>

<ItemStyle BackColor="White" BorderStyle="None" Width="1px"></ItemStyle>

<HeaderStyle CssClass="grdheader" ></HeaderStyle>
<ItemTemplate>
<INPUT style="VISIBILITY: hidden; BORDER-BOTTOM-COLOR: #eeeeee; WIDTH: 44px; BORDER-TOP-COLOR: #eeeeee; BORDER-RIGHT-COLOR: #eeeeee" id="txtlineno" class="field_input" readOnly type=text maxLength=50 value='<%# Bind("LineNo") %>' runat="server" /> 
</ItemTemplate>

<FooterStyle CssClass="grdfooter" BorderStyle="None"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField>
<ControlStyle BorderStyle="None"></ControlStyle>

<ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>

<HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
<ItemTemplate>
<INPUT style="VISIBILITY: hidden; BORDER-BOTTOM-COLOR: #eeeeee; WIDTH: 1px; BORDER-TOP-COLOR: #eeeeee; BORDER-RIGHT-COLOR: #eeeeee" id="txtcontrolacname" class="field_input" readOnly type=text maxLength=50 value=" " runat="server" />
</ItemTemplate>

<FooterStyle BorderStyle="None"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField>
<ControlStyle BorderStyle="None"></ControlStyle>

<ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>

<HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
<ItemTemplate>
<INPUT style="VISIBILITY: hidden; BORDER-BOTTOM-COLOR: #eeeeee; WIDTH: 1px; BORDER-TOP-COLOR: #eeeeee; BORDER-RIGHT-COLOR: #eeeeee" id="txtacctcode" class="field_input" readOnly type=text maxLength=50 value=" " runat="server" />
</ItemTemplate>

<FooterStyle BorderStyle="None"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField>
<ControlStyle BackColor="White" BorderStyle="None"></ControlStyle>

<ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>

<HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
<ItemTemplate>
<INPUT style="VISIBILITY: hidden; BORDER-BOTTOM-COLOR: #eeeeee; WIDTH: 1px; BORDER-TOP-COLOR: #eeeeee; BORDER-RIGHT-COLOR: #eeeeee" id="txtacctname" class="field_input" readOnly type=text maxLength=50 value=" " runat="server" />
</ItemTemplate>

<FooterStyle BorderStyle="None"></FooterStyle>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle" ></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"  Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView></DIV><asp:Button id="btnAdd" tabIndex=7 runat="server" Text="Add Row" 
            CssClass="field_button" Font-Bold="True" Visible="False"></asp:Button>&nbsp;<asp:Button id="btnDelLine" tabIndex=8 onclick="btnDelLine_Click" runat="server" Text="DeleteRow" CssClass="field_button" Width="96px" CausesValidation="False" Visible="False"></asp:Button></TD></TR><TR><TD colSpan=5 style="height: 89px"><TABLE><TBODY><TR><TD colSpan=2 style="height: 32px"><INPUT style="VISIBILITY: hidden; WIDTH: 67px" id="txtdecimal" type=text maxLength=15 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 29px" id="txtbasecurr" type=text maxLength=20 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 47px" id="txtgridrows" type=text runat="server" />&nbsp;
    <INPUT style="VISIBILITY: hidden; WIDTH: 29px" id="txtDivCode" type=text maxLength=20 runat="server" /> 
    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
    <INPUT style="VISIBILITY: hidden; WIDTH: 65px" id="txtGridType" type=text maxLength=20 runat="server" /></TD><TD style="height: 32px"><asp:Label id="Label7" runat="server" Text=" Total" CssClass="filed_caption" Width="98px"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 32px"><INPUT style="WIDTH: 80px; TEXT-ALIGN: right" id="txtTotalDebit" class="field_input" readOnly type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 100px; HEIGHT: 32px"><INPUT style="WIDTH: 80px; TEXT-ALIGN: right" id="txtTotalCredit" class="field_input" readOnly type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 100px; HEIGHT: 32px"><asp:Label id="lblBaseTot" runat="server" Text=" Total" CssClass="filed_caption" Width="98px"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 32px"><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtTotBaseDebit" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 100px; HEIGHT: 32px"><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtTotBaseCredit" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD>
    <td style="width: 114px; height: 32px" align="right" class="td_cell">
        <asp:Label ID="lblBaseDiff" runat="server" CssClass="filed_caption" Text=" Total"></asp:Label></td>
    <td style="width: 100px; height: 32px">
        <INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtTotBaseDiff" class="field_input" readOnly type=text maxLength=100 runat="server" /></td>
</TR><TR><TD style="height: 28px; width: 111px;"><asp:Button ID="btngenerate" runat="server" CssClass="field_button" OnClick="btngenerate_Click"
                TabIndex="7" Text="Display Bills" /></TD><TD style="height: 28px">&nbsp;</TD><TD style="height: 28px"></TD><TD style="WIDTH: 100px; HEIGHT: 28px"><INPUT style="WIDTH: 11px; visibility: hidden;" id="txtacccode" type=text runat="server" />
                    <INPUT style="WIDTH: 15px; visibility: hidden;" id="txtacctype" type=text runat="server"  />
                    <input id="txtRowCount" runat="server" style="visibility: hidden; width: 1px" type="text" /></TD><TD style="WIDTH: 100px; HEIGHT: 28px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
                    <INPUT style="VISIBILITY: hidden; WIDTH: 47px" id="txtAdjcolno" type=text runat="server" /></TD><TD style="height: 28px"></TD><TD style="WIDTH: 100px; HEIGHT: 28px"></TD><TD style="WIDTH: 100px; HEIGHT: 28px"></TD>
    <td style="width: 114px; height: 28px">
    </td>
    <td style="width: 100px; height: 28px">
    </td>
</TR></TBODY></TABLE>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 100px">
                <div class="container" style="width: 973px; height: 150px">
                    <asp:GridView ID="GVCollection" runat="server" AutoGenerateColumns="False" Visible ="true"
                        CssClass="grdstyle" Height="33px"
                        Width="400px">
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server" checked='<%# DataBinder.Eval (Container.DataItem, "sel") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tranid">
                                <ItemTemplate>
                                    <asp:Label ID="lbltranid" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "tranid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="lineno">
                                <ItemTemplate>
                                    <asp:Label ID="lbltranlineno" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "acc_tran_lineno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tran type">
                                <ItemTemplate>
                                    <asp:Label ID="lbltrantype" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "trantype") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Debit">
                                <ItemTemplate>
                                    <asp:Label ID="lbldebit" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "debit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Credit">
                                <ItemTemplate>
                                    <asp:Label ID="lblcredit" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "Credit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                        <RowStyle CssClass="grdRowstyle" ForeColor="#084573" Wrap="False" />
                        <PagerStyle Wrap="False" />
                        <HeaderStyle CssClass="grdheader" BorderStyle="None" ForeColor="white" Wrap="False" />
                        <AlternatingRowStyle  CssClass="grdAternaterow" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
</TD></TR><TR><TD align=center colSpan=5 style="height: 30px">
    <table>
        <tr>
            <td style="font-weight: bold; width: 326px; color: black; height: 26px">
                &nbsp;</td>
            <td style="width: 100px; height: 26px">
                <asp:CheckBox ID="chkadjust" runat="server" CssClass="filed_caption" Font-Bold="True"
                    Text="Allow any way" Visible="False" Width="121px" /></td>
            <td style="width: 100px; height: 26px">
                <asp:CheckBox id="chkPost" tabIndex=9 runat="server" Text="Post/UnPost" Font-Size="10px" Font-Names="Verdana" CssClass="field_caption" Width="103px" BackColor="#FFC0C0" Font-Bold="True" ForeColor="Black"></asp:CheckBox></td>
            <td style="width: 58px; height: 26px">
                <asp:Button id="btnSave" tabIndex=10 runat="server" Text="Save" 
                    CssClass="field_button" Font-Bold="True" OnClientClick="return ValidatePage();"></asp:Button></td>
            <td style="width: 47px; height: 26px">
                <asp:Button id="btnPrint" tabIndex=11 runat="server" Text="Print" 
                    CssClass="field_button"></asp:Button></td>
            <td style="width: 44px; height: 26px">
                <asp:Button id="btnExit" tabIndex=12 runat="server" Text="Exit" 
                    CssClass="field_button" Font-Bold="True"></asp:Button></td>
            <td style="width: 51px; height: 26px">
                <asp:Button id="btnhelp" tabIndex=13 onclick="btnhelp_Click" runat="server" 
                    Text="Help" CssClass="field_button"></asp:Button></td>
        </tr>
    </table>
    &nbsp; &nbsp; &nbsp;</TD></TR></TBODY></TABLE><cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtJDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
                </cc1:MaskedEditExtender> <cc1:CalendarExtender id="ClsExFromDate" runat="server" TargetControlID="txtJDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender><cc1:CalendarExtender id="ClExChequeDate" runat="server" TargetControlID="txtTdate" PopupButtonID="ImageButton1" Format="dd/MM/yyyy">
                </cc1:CalendarExtender><cc1:MaskedEditExtender id="MskChequeDate" runat="server" TargetControlID="txtTdate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
                </cc1:MaskedEditExtender><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 
    <asp:HiddenField ID="hdnRows" runat="server" />
    <asp:HiddenField ID="hdnSS" runat="server" Value="0" />
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

