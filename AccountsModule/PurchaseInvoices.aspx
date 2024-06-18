<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="PurchaseInvoices.aspx.vb" Inherits="AccountsModule_PurchaseInvoices" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/accounts.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

<script language="javascript" type="text/javascript">

var ddldiffAcc=null;
var ddldiffAccNm=null;

function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g,"");	
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

 	function DecRound(amtToRound) 
{
  
  var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
  nodecround=Math.pow(10,parseInt(txtdec.value));
  //var amtToRound=Number(amtToRound1);
  var rdamt=Math.round( parseFloat( Number(amtToRound)  ) *nodecround)/nodecround;
  return  parseFloat(rdamt);
}
function DecFormat(value)   
{
          var rdamt=null;
          var indx=value.indexOf('.');
           var deci=document.getElementById("<%=txtdecimal.ClientID%>");
           var lngLenght =deci.value;  
           if (indx < 0 )
           {
             rdamt=value+".000"; 
             return rdamt;
             }
              var digit=value.substring(indx+1);
              if(digit.length>lngLenght-1)
                { 
                  rdamt=value;
                  return rdamt;}
                else
                 {
                  var nozeros=parseInt(lngLenght)-parseInt(digit.length);
                 
                  if (nozeros==1)
                       {rdamt=value+"0";}
                    else if (nozeros==2)
                       {rdamt=value+"00";}
                   else if (nozeros==3)
                      {rdamt=value+"000";}
                   else
                       { return value;}
                    return rdamt;   
                  }
                  return rdamt;
    }



    function Filldiffac(accdiffcode, accdiffname) {       

        ddldiffAcc = document.getElementById(accdiffcode);
        ddldiffAccNm = document.getElementById(accdiffname);

        ddldiffAcc.value = ddldiffAccNm.options[ddldiffAccNm.selectedIndex].text;
      
    }

   
       	
function FillSupplier()
{
    var ddlTyp=document.getElementById("<%=ddlType.ClientID%>");
    var strQryCode="";
    var strQryName="";
    var strQryPostToCode="";
    var strQryPostToName="";
    lblcustcode = document.getElementById("<%=lblCustCode.ClientID%>");
    lblcustname = document.getElementById("<%=lblCustName.ClientID%>");
    var strcap=ddlTyp.options[ddlTyp.selectedIndex].text;
    if (ddlTyp.value=="S")
    {
      lblcustcode.innerHTML=strcap+'Code <font color="Red"> *</font>';
    lblcustname.innerHTML=strcap+'Name';
    strQryCode="select Code,des from view_account where type ='S' order by code ";
    strQryName="select des,Code from view_account where type ='S'order by des ";
//    strQryPostToCode="select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'";
//    strQryPostToName="select distinct partymast.partyname,view_account.postaccount from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'";
// 
    strQryPostToCode="select top 10 Code as postaccount,des as partyname  from view_account where type ='S' order by code ";
    strQryPostToName="select top 10  des as partyname ,Code as postaccount  from view_account where type ='S'order by des ";

    }
    else if (ddlTyp.value=="A")
    {
      lblcustcode.innerHTML=strcap+'Code <font color="Red"> *</font>';
     lblcustname.innerHTML=strcap+'Name';
    strQryCode="select Code,des from view_account where type ='A'";
    strQryName="select des,Code from view_account where type ='A'";
    strQryPostToCode="select top 10 Code as postaccount,des as partyname  from view_account where type ='A' order by code ";
    strQryPostToName="select top 10  des as partyname ,Code as postaccount  from view_account where type ='A' order by des ";

//    strQryPostToCode="select distinct view_account.postaccount,supplier_agents.supagentname from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'";
//    strQryPostToName="select distinct supplier_agents.supagentname,view_account.postaccount from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'";
    }
    else
    {
    lblcustcode.innerHTML='Code <font color="Red"> *</font>';
    lblcustname.innerHTML='Name';
    }
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
    
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryCode,FillSupplierCode,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryName,FillSupplierName,ErrorHandler,TimeOutHandler);
    
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToCode,FillPostToCode,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToName,FillPostToName,ErrorHandler,TimeOutHandler);
    
}

function FillPostAccs()
{

var ddlTyp=document.getElementById("<%=ddlType.ClientID%>");
var ddlSCode=document.getElementById("<%=ddlSupplierCode.ClientID%>");
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   

 if ( (ddlTyp.value !='[Select]') && (ddlSCode.value!='[Select]'))
    {
    strQryPostToCode="select  Code as postaccount,des as partyname  from view_account where type ='"+ddlTyp.value +"' and code <> '"+ ddlSCode.options[ddlSCode.selectedIndex].text +"' order by code ";
    strQryPostToName="select  des as partyname ,Code as postaccount  from view_account where type ='"+ddlTyp.value +"' and code <> '"+ ddlSCode.options[ddlSCode.selectedIndex].text +"'  order by des ";
    }
    else
    {
    strQryPostToCode="select top 10 Code as postaccount,des as partyname  from view_account where type ='"+ddlTyp.value +"' order by code ";
    strQryPostToName="select top 10  des as partyname ,Code as postaccount  from view_account where  type ='"+ddlTyp.value +"' order by des ";
    }
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToCode,FillPostToCode,ErrorHandler,TimeOutHandler);
    ColServices.clsServices.GetQueryReturnStringListnew(constr,strQryPostToName,FillPostToName,ErrorHandler,TimeOutHandler);
}


function FillSupplierCode(result)
    {
      var ddlSCode=document.getElementById("<%=ddlSupplierCode.ClientID%>");
        RemoveAll(ddlSCode)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlSCode.options.add(option);
        }
        ddlSCode.value="[Select]";
    }

function FillSupplierName(result)
    {
      var ddlSName=document.getElementById("<%=ddlSpplierName.ClientID%>");
        RemoveAll(ddlSName)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlSName.options.add(option);
        }
        ddlSName.value="[Select]";
    }

    
    function FillPostToCode(result)
    {
      var ddlPCode=document.getElementById("<%=ddlPostToCode.ClientID%>");
        RemoveAll(ddlPCode)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlPCode.options.add(option);
        }
        ddlPCode.value="[Select]";
    }

function FillPostToName(result)
    {
      var ddlPName=document.getElementById("<%=ddlPostToName.ClientID%>");
        RemoveAll(ddlPName)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddlPName.options.add(option);
        }
        ddlPName.value="[Select]";
    }


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

    
    
function FillAllDataOnSupplier()
{

  var ddlTyp=document.getElementById("<%=ddlType.ClientID%>");
  var ddlSCode=document.getElementById("<%=ddlSupplierCode.ClientID%>");
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
  
    if ((ddlTyp.value!='[Select]') &&  (ddlSCode.value!='[Select]'))
    {
 // var crdsqlstr="select postaccount,accrualacctcode,controlacctcode,cur,convrate  from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '"+  ddlSCode.options[ddlSCode.selectedIndex].text +"' and type='"+ddlTyp.value  +"' ";
  var crdsqlstr="select isnull(postaccount,'[Select]') as postaccount  ,isnull(accrualacctcode,'[Select]') as accrualacctcode ,isnull(controlacctcode,'[Select]') as controlacctcode,isnull(cur,'') as cur, isnull(convrate,0) as convrate  from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '"+  ddlSCode.options[ddlSCode.selectedIndex].text +"' and type='"+ddlTyp.value  +"' ";
  ColServices.clsServices.GetQueryReturnStringArraynew(constr,crdsqlstr,5,FillValue,ErrorHandler,TimeOutHandler);
  }
  else
  {
  FillValueClear();
  }
  
  
  //ColServices.clsServices.GetQueryReturnStringArraynew(constr,sqlstr1,4,FillValueCurr,ErrorHandler,TimeOutHandler);

 }  
function FillValue(result)
{

    var i=0;
    var ddlPCode=document.getElementById("<%=ddlPostToCode.ClientID%>");
    var ddlPName=document.getElementById("<%=ddlPostToName.ClientID%>");
    ddlPName.value = result[0];

    ddlPCode.value= ddlPName.options[ddlPName.selectedIndex].text;
    
    var txtPCode=document.getElementById("<%=txtPostCode.ClientID%>");
    var txtPName=document.getElementById("<%=txtPostName.ClientID%>");
    txtPCode.value=result[0];
    txtPName.value= ddlPName.options[ddlPName.selectedIndex].text;
    
    
    
     
     var ddlACode=document.getElementById("<%=ddlAccrualCode.ClientID%>");
     var ddlAName=document.getElementById("<%=ddlAccuralName.ClientID%>");
     ddlAName.value=result[1];
     ddlACode.value= ddlAName.options[ddlAName.selectedIndex].text;
     var ddlCCode=document.getElementById("<%=ddlControlCode.ClientID%>");
     var ddlCName=document.getElementById("<%=ddlControlName.ClientID%>");
   
     ddlCName.value=result[2];
     ddlCCode.value= ddlCName.options[ddlCName.selectedIndex].text;
     
     var txtCurr=document.getElementById("<%=txtCurrency.ClientID%>");
     txtCurr.value=result[3];
     var txtERate=document.getElementById("<%=txtExchRate.ClientID%>");
     txtERate.value=result[4];
     var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
    if ( trim(txtCurr.value)==trim(txtbase.value))
       {
        txtERate.readOnly=true;
        txtERate.disabled=true;
       }
       else
       {
        txtERate.readOnly=false;
        txtERate.disabled=false;
       }
} 

function FillValueClear()
{
    
    var ddlPCode=document.getElementById("<%=ddlPostToCode.ClientID%>");
    var ddlPName=document.getElementById("<%=ddlPostToName.ClientID%>");
    ddlPName.value='[Select]';
    ddlPCode.value='[Select]';
    
    var txtPCode=document.getElementById("<%=txtPostCode.ClientID%>");
    var txtPName=document.getElementById("<%=txtPostName.ClientID%>");
    txtPCode.value='[Select]';
    txtPName.value='[Select]';
     
     var ddlACode=document.getElementById("<%=ddlAccrualCode.ClientID%>");
     var ddlAName=document.getElementById("<%=ddlAccuralName.ClientID%>");
     ddlAName.value='[Select]';
     ddlACode.value= ddlAName.options[ddlAName.selectedIndex].text;
     var ddlCCode=document.getElementById("<%=ddlControlCode.ClientID%>");
     var ddlCName=document.getElementById("<%=ddlControlName.ClientID%>");
   
     ddlCName.value='[Select]';
     ddlCCode.value='[Select]';
     
     var txtCurr=document.getElementById("<%=txtCurrency.ClientID%>");
     txtCurr.value="";
     var txtERate=document.getElementById("<%=txtExchRate.ClientID%>");
     txtERate.value="";
     var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
    
    if ( trim(txtCurr.value)==trim(txtbase.value))
       {
        txtERate.readOnly=true;
        txtERate.disabled=true;
         
       }
       else
       {
        txtERate.readOnly=false;
        txtERate.disabled=false;
       }
}

function changesupplier()
{
var txtxtrequestid=document.getElementById("<%=txtxtrequestid.ClientID%>");

var connstr = document.getElementById("<%=txtconnection.ClientID%>");
constr = connstr.value   

var strQry=null
var strQry1=null
if (txtxtrequestid.value!='')
{
strQry=" select distinct d.partycode,p.partyname from reservation_detailnew d,partymast p where d.partycode=p.partycode and d.requestid='"+ txtxtrequestid.value +"' union all "
strQry=strQry + " select distinct d.partycode,p.partyname from reservation_other_subdetail d,partymast p  where d.partycode=p.partycode and d.requestid='"+ txtxtrequestid.value +"'"

strQry1=" select distinct p.partyname,d.partycode from reservation_detailnew d,partymast p where d.partycode=p.partycode and d.requestid='"+ txtxtrequestid.value +"' union all "
strQry1=strQry1 + " select distinct p.partyname,d.partycode from reservation_other_subdetail d,partymast p  where d.partycode=p.partycode and d.requestid='"+ txtxtrequestid.value +"'"
}
else
{
strQry="select partycode,partyname from partymast where active=1"
strQry1="select partyname,partycode from partymast where active=1"
}
ColServices.clsServices.GetQueryReturnStringListnew(constr,strQry,FillSupplierCode,ErrorHandler,TimeOutHandler);
ColServices.clsServices.GetQueryReturnStringListnew(constr,strQry1,FillSupplierName,ErrorHandler,TimeOutHandler);
} 

//function FillValueCurr(result)
//{
//   var txtCurr=document.getElementById("<%=txtCurrency.ClientID%>");
//     txtCurr.value=result[0];
//     FillCurrRate();
//}

//function FillCurrRate()
//{
// var txtCurr=document.getElementById("<%=txtCurrency.ClientID%>");
// var strSqlQry="select convrate from currrates where currcode='"+ txtCurr.value +"' and tocurr in (select option_selected from reservation_parameters where param_id=457)" 
// ColServices.clsServices.GetQueryReturnStringArraynew(constr,strSqlQry,1,FillCuurate,ErrorHandler,TimeOutHandler);
//} 
//function FillCuurate(result)
//{
//   var txtERate=document.getElementById("<%=txtExchRate.ClientID%>");
//    txtERate.value=result[0];
//}

function DispalyValidate()
{
    if (document.getElementById("<%=txtSInvoiceNo.ClientID%>").value=="")
    {
        alert("Please enter supplier invoice no.");
        document.getElementById("<%=txtSInvoiceNo.ClientID%>").focus();
        return false;
    }
    else if (document.getElementById("<%=ddlSupplierCode.ClientID%>").value=="[Select]" ||document.getElementById("<%=ddlSpplierName.ClientID%>").value=="[Select]")
    {
        alert("Select supplier code");
        document.getElementById("<%=ddlSupplierCode.ClientID%>").focus();
        return false;
    }
    else if (document.getElementById("<%=ddlAccrualCode.ClientID%>").value=="[Select]" || document.getElementById("<%=ddlAccuralName.ClientID%>").value=="[Select]")
    {
        alert("Select accrual a/c code");
        document.getElementById("<%=ddlAccrualCode.ClientID%>").focus();
        return false;
    }
    else if (document.getElementById("<%=ddlControlCode.ClientID%>").value=="[Select]" ||document.getElementById("<%=ddlControlName.ClientID%>").value=="[Select]")
    {
        alert("Select control a/c code");
        document.getElementById("<%=ddlControlCode.ClientID%>").focus();
        return false;
    }  
          
}

function CallWebMethod(methodType)
{
 switch(methodType)
        {
            case "partycode":
                var select=document.getElementById("<%=ddlSupplierCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSpplierName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                FillPostAccs();
                FillAllDataOnSupplier();
                break;
             case "partyname":
                var select=document.getElementById("<%=ddlSpplierName.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSupplierCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                FillPostAccs();
                FillAllDataOnSupplier();
                break;
             case "posttocode":
                var select=document.getElementById("<%=ddlPostToCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlPostToName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var txtPCode=document.getElementById("<%=txtPostCode.ClientID%>");
                var txtPName=document.getElementById("<%=txtPostName.ClientID%>");
                txtPCode.value=select.value;
                txtPName.value=selectname.value;
                break;
             case "posttoname":
                var select=document.getElementById("<%=ddlPostToName.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlPostToCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var txtPCode=document.getElementById("<%=txtPostCode.ClientID%>");
                var txtPName=document.getElementById("<%=txtPostName.ClientID%>");
                txtPCode.value=selectname.value;
                txtPName.value=select.value;
                
                break;
            case "accuralcode":
                var select=document.getElementById("<%=ddlAccrualCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlAccuralName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
             case "accuralname":
                var select=document.getElementById("<%=ddlAccuralName.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlAccrualCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
             case "controlcode":
                var select=document.getElementById("<%=ddlControlCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlControlName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
             case "controlname":
                var select=document.getElementById("<%=ddlControlName.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlControlCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        }
}

 

function FillSInvoiceValue(chksel,txtval,txtsupinvval,txtsupinvvalkwd,diffamtkwd,postto,narr,txtdiff,convrate,posttonm)
{
    chsel = document.getElementById(chksel);
    txtvalue=document.getElementById(txtval);
    txtsinvval = document.getElementById(txtsupinvval);
    txtsinvvalkwd = document.getElementById(txtsupinvvalkwd);
    txtdiffamountkwd = document.getElementById(diffamtkwd);
    ddlpostto = document.getElementById(postto);
    txtnar = document.getElementById(narr);
    txtdiffamt = document.getElementById(txtdiff);
    ddlposttonm = document.getElementById(posttonm);

    txtnarr = document.getElementById("<%=txtNarration.ClientID%>");
    //ddlnarr = document.getElementById("<%=ddlNarration.ClientID%>");
      // if(ddlnarr.value!="[Select]")
        //{
        //txtnar.value=ddlnarr.value;
        //}
        //else if (txtnarr.value!="")
        //{
          txtnar.value=txtnarr.value;
        //}
    txtRate=document.getElementById(convrate);
    if (chsel.checked == true)
    {
        txtsinvval.value=DecRound(txtvalue.value);
        txtsinvval.value=DecFormat(String(txtsinvval.value));
        txtsinvvalkwd.value=DecRound(DecRound(txtsinvval.value)*parseFloat(txtRate.value));
        txtsinvvalkwd.value=DecFormat(String(txtsinvvalkwd.value));
        txtdiffamountkwd.value=DecRound(DecRound(DecRound(txtvalue.value)-DecRound(txtsinvval.value))*parseFloat(txtRate.value));
        txtdiffamountkwd.value=DecFormat(String(txtdiffamountkwd.value));
        txtdiffamt.value=DecRound(DecRound(txtvalue.value)- DecRound(txtsinvval.value));
        
        GrandTotal();
        
        if (parseFloat(txtdiffamountkwd.value)!=0)
        {
          //  ddlpostto.disabled = false;
            ddlposttonm.disabled = false;
        }
        else
        {
         //   ddlpostto.disabled = true;
            ddlposttonm.disabled = true;
        }
    }
    else if (chsel.checked == false)
    {
//        txttotalsinvval.value=DecRound(txttotalsinvval.value) - DecRound(txtsinvval.value);
//        txttotaldiffamtkwd.value=DecRound(txttotaldiffamtkwd.value)-DecRound(txtdiffamountkwd.value);
        txtsinvval.value="";
        txtsinvvalkwd.value="";
        txtdiffamountkwd.value="";
        txtnar.value="";
        ddlpostto.value = "[Select]";
        ddlposttonm.value = "[Select]";
        GrandTotal();
    }
       if (parseFloat(txtdiffamountkwd.value)>0)
        {
            // ddlpostto.disabled=false;
            ddlposttonm.disabled = false;
        }
        else
        {
           // ddlpostto.disabled = true;
            ddlposttonm.disabled = true;
        }
    }

    var ddldiffto = null;
    var ddldifftonm = null;


function OnchangeFillInvoiceValue(chksel, txtval, txtsupinvval, txtsupinvvalkwd, diffamtkwd, postto, txtdiff, convrate, posttonm,invno,hdngrpcode)
{
    chsel = document.getElementById(chksel);
    txtvalue=document.getElementById(txtval);
    txtsinvval = document.getElementById(txtsupinvval);
    txtsinvvalkwd = document.getElementById(txtsupinvvalkwd);
    txtdiffamountkwd = document.getElementById(diffamtkwd);
    ddlpostto1 = document.getElementById(postto);
    txtdiffamt = document.getElementById(txtdiff);
    ddlposttonm = document.getElementById(posttonm);
   
    ddldiffto = document.getElementById(postto);
    ddldifftonm = document.getElementById(posttonm);

    var ddlSCode = document.getElementById("<%=ddlSupplierCode.ClientID%>");
    partycode = ddlSCode.options[ddlSCode.selectedIndex].text;
    


    txtRate=document.getElementById(convrate);
    if (chsel.checked == true)
    {  
        txtsinvval.value=DecRound(txtsinvval.value);
        
        txtsinvvalkwd.value=DecRound(DecRound(txtsinvval.value)*parseFloat(txtRate.value));
        txtsinvvalkwd.value=DecFormat(String(txtsinvvalkwd.value));
          
       //txtdiffamountkwd.value=DecRound((txtvalue.value-txtsinvval.value)*(txtRate.value));
        txtdiffamountkwd.value=DecRound( DecRound( DecRound(txtvalue.value)- DecRound(txtsinvval.value)) *parseFloat(txtRate.value));
        txtdiffamountkwd.value=DecFormat(String(txtdiffamountkwd.value));
        
        txtdiffamt.value= DecRound(DecRound(txtvalue.value)-DecRound(txtsinvval.value));

        GrandTotal();


        if (parseFloat(txtdiffamountkwd.value) != 0) {

           var connstr = document.getElementById("<%=txtConnection.ClientID%>");
           constr = connstr.value
           ColServices.clsServices.Getcostcode(constr, invno, partycode, hdngrpcode, Fillcostcode, ErrorHandler, TimeOutHandler);

         //   ddlpostto.disabled = false;
            ddlposttonm.disabled = false;
 
        }
        else
        {
            ddlposttonm.disabled = true;
            ddlpostto1.value = "[Select]";
            ddlposttonm.value = "[Select]";
        }



    } 
    else
    {
        txtsinvval.value="";
        txtsinvvalkwd.value="";
        txtdiffamountkwd.value="";
        alert("Please select record first by selecting checkbox.");
    }
       
}

function Fillcostcode(result) {
    ddldifftonm.value = result;
    ddldiffto.value = ddldifftonm.options[ddldifftonm.selectedIndex].text;
}

function GrandTotal()
{
    var objGridView = document.getElementById('<%=gvResult.ClientID%>');
    txttotalsinvval = document.getElementById("<%=txtTotalSInvoiceValue.ClientID%>");
    txttotalsinvvalkwd = document.getElementById("<%=txtTotalSInvoiceValueKWD.ClientID%>");
    txttotaldiffamtkwd = document.getElementById("<%=txtTotalDiffAmountKWD.ClientID%>");
    txtInvoiceTot = document.getElementById("<%=txtInvoiceTotal.ClientID%>");
    var invtot=0;
    txtrowcnt = document.getElementById("<%=txtRowCount.ClientID%>");
    intRows=txtrowcnt.value;
    txttotalsinvval.value=0;
    txttotalsinvvalkwd.value=0;
    txttotaldiffamtkwd.value=0;
      for(j=1;j<=intRows;j++)
        {
              ddlpostto =  objGridView.rows[j].cells[11].children[0];
              ddlpostto.disabled=true;
            if(objGridView.rows[j].cells[5].children[0].checked == true)
              { 
                if (objGridView.rows[j].cells[7].children[0].value==''){objGridView.rows[j].cells[7].children[0].value=0;}
                if (isNaN(objGridView.rows[j].cells[7].children[0].value)==true){objGridView.rows[j].cells[7].children[0].value=0;}
                txttotalsinvval.value=DecRound(txttotalsinvval.value)+DecRound(objGridView.rows[j].cells[7].children[0].value);
                    
                if (objGridView.rows[j].cells[8].children[0].value==''){objGridView.rows[j].cells[8].children[0].value=0;}
                if (isNaN(objGridView.rows[j].cells[8].children[0].value)==true){objGridView.rows[j].cells[8].children[0].value=0;}
                txttotalsinvvalkwd.value=DecRound(txttotalsinvvalkwd.value)+DecRound(objGridView.rows[j].cells[8].children[0].value);
                   
                if (objGridView.rows[j].cells[9].children[0].value==''){objGridView.rows[j].cells[9].children[0].value=0;}
                if (isNaN(objGridView.rows[j].cells[9].children[0].value)==true){objGridView.rows[j].cells[9].children[0].value=0;}
                txttotaldiffamtkwd.value=DecRound(txttotaldiffamtkwd.value)+DecRound(objGridView.rows[j].cells[9].children[0].value);
                
                if (objGridView.rows[j].cells[4].children[0].value==''){objGridView.rows[j].cells[4].children[0].value=0;}
                if (isNaN(objGridView.rows[j].cells[4].children[0].value)==true){objGridView.rows[j].cells[4].children[0].value=0;}
               invtot=DecRound(invtot)+DecRound(objGridView.rows[j].cells[4].children[0].value);
                
                
             }
              
              txtInvoiceTot.value=DecRound(invtot);
             if (parseFloat(objGridView.rows[j].cells[9].children[0].value)!=0)
              {
                ddlpostto.disabled=false;
              }
         }
 txttotalsinvval.value=DecFormat(String(txttotalsinvval.value));
 txttotalsinvvalkwd.value=DecFormat(String(txttotalsinvvalkwd.value));
 txttotaldiffamtkwd.value=DecFormat(String(txttotaldiffamtkwd.value));
}

function ChangeValuesOnExchRate()
{
    var objGridView = document.getElementById('<%=gvResult.ClientID%>');
    txtRate=document.getElementById("<%=txtExchRate.ClientID%>");
    txtrowcnt = document.getElementById("<%=txtRowCount.ClientID%>");
    intRows=txtrowcnt.value;
      for(j=1;j<=intRows;j++)
        {
             if(objGridView.rows[j].cells[5].children[0].checked == true)
              { 
                var sinvval=objGridView.rows[j].cells[6].children[0].value;
                var lngvalue=objGridView.rows[j].cells[4].children[0].value;
                objGridView.rows[j].cells[7].children[0].value=DecRound(sinvval)*(txtRate.value);
                objGridView.rows[j].cells[8].children[0].value=(DecRound(lngvalue)-DecRound(sinvval))*(txtRate.value);
              }
        }
    GrandTotal();
}

function SelectAll()
{
    var objGridView = document.getElementById('<%=gvResult.ClientID%>');
    txtrowcnt = document.getElementById("<%=txtRowCount.ClientID%>");
    txtnarr = document.getElementById("<%=txtNarration.ClientID%>");
    ddlnarr = document.getElementById("<%=ddlNarration.ClientID%>");
   // txtRate=document.getElementById("<%=txtExchRate.ClientID%>");
    intRows=txtrowcnt.value;
      for(j=1;j<=intRows;j++)
        {
             txtRate= objGridView.rows[j].cells[6].children[0]
            
             objGridView.rows[j].cells[5].children[0].checked=true;
             var lngvalue=objGridView.rows[j].cells[4].children[0].value;
             objGridView.rows[j].cells[7].children[0].value=DecFormat(lngvalue);
             var lngsinvvalue=objGridView.rows[j].cells[7].children[0].value;
             objGridView.rows[j].cells[8].children[0].value=DecRound(lngsinvvalue)*parseFloat(txtRate.value);
             objGridView.rows[j].cells[9].children[0].value=(DecRound(lngsinvvalue)-DecRound(lngvalue))*parseFloat(txtRate.value);
            
            if(ddlnarr.value!="[Select]")
            {
            objGridView.rows[j].cells[10].children[0].value=ddlnarr.value;
            }
            else if (txtnarr.value!="")
            {
              objGridView.rows[j].cells[10].children[0].value=txtnarr.value;
            }
         }
    GrandTotal();
}
function DeSelectAll()
{
    var objGridView = document.getElementById('<%=gvResult.ClientID%>');
    txtrowcnt = document.getElementById("<%=txtRowCount.ClientID%>");
    intRows=txtrowcnt.value;
      for(j=1;j<=intRows;j++)
        {
             objGridView.rows[j].cells[5].children[0].checked=false;
             objGridView.rows[j].cells[7].children[0].value="";
             objGridView.rows[j].cells[8].children[0].value="";
             objGridView.rows[j].cells[9].children[0].value="";
             objGridView.rows[j].cells[10].children[0].value="";
             objGridView.rows[j].cells[11].children[0].value="[Select]";
        }
    GrandTotal();
}

function FillCombotoText(ddlc,txtt)
{
    ddlcs = document.getElementById(ddlc); 
    txtts = document.getElementById(txtt); 
    
    var codeid=ddlcs.options[ddlcs.selectedIndex].text;
    txtts.value=codeid;
}  
function printRepor()
{
    var Pinvno = String(document.getElementById("<%=txtPInvoiceNo.ClientID%>").value);
    var trans =String(document.getElementById("<%=txtTranType.ClientID%>").value);
   // alert(Pinvno);
  //  alert(trans);
    if(confirm('Are you sure you want to print?')==false)
    {
        return false;
    }
    else
    {
        window.open('../PriceListModule/PrintDocNew.aspx?Pageame=PurchaseInvoiceDoc&BackPageName=~\AccountsModule\PurchaseInvoices.aspx&PInvoiceNo='+Pinvno+'&TranType='+trans+'','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');
        return true;
    }

} 
  
  function validate_click()
{
    var hdnSS=document.getElementById("<%=hdnSS.ClientID%>");
    var btnss=document.getElementById("<%=btnsave.ClientID%>");
    hdnSS.value=0;    
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


    <table>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 920px" class="td_cell"><TBODY><TR><TD style="TEXT-ALIGN: center" colSpan=6>
    <asp:Label id="lblHeading" runat="server" Text="Add Purchase Invoice" 
        Width="980px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 144px">Purchase Invocie No</TD><TD style="WIDTH: 212px"><INPUT style="WIDTH: 128px" id="txtPInvoiceNo" class="field_input" readOnly type=text runat="server" /></TD><TD style="WIDTH: 118px">Purchase Invoice Date</TD><TD style="WIDTH: 270px"><asp:TextBox id="txtPInvoiceDate" tabIndex=1 runat="server" Width="88px" CssClass="fiel_input" ValidationGroup="MKE"></asp:TextBox>
<asp:ImageButton id="ImgBtnPInvoiceDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>
 <cc1:MaskedEditValidator id="MEVPInvoiceDate" runat="server" CssClass="field_error" ControlExtender="MEPInvoiceDate" ControlToValidate="txtPInvoiceDate" Display="Dynamic" EmptyValueBlurredText="Purchase invoice date is required" InvalidValueBlurredMessage="Invalid purchase invoice date" InvalidValueMessage="Invalid Date" TooltipMessage="Enter a date in dd/mm/yyyy format" ErrorMessage="MEVPInvoiceDate"></cc1:MaskedEditValidator></TD><TD style="WIDTH: 56px">Type</TD>
 <TD><SELECT style="WIDTH: 104px" id="ddlType" class="field_input MyAutoCompleteTypeClass" tabIndex=2 onchange="FillSupplier()" runat="server"> <OPTION value="S" selected>Supplier</OPTION><OPTION value="A">Supplier Agent</OPTION></SELECT></TD></TR><TR>
 <TD style="WIDTH: 144px"><asp:Label id="lblCustCode" runat="server" Text="Supplier  Code <font color='Red'> *</font>" Width="136px"></asp:Label></TD>
 <TD style="WIDTH: 212px"><SELECT style="WIDTH: 160px" id="ddlSupplierCode" class="field_input" tabIndex=3 onchange="CallWebMethod('partycode');" runat="server"></SELECT>&nbsp; </TD><TD style="WIDTH: 118px"><asp:Label id="lblCustName" runat="server" Text=" Supplier Name" Width="122px" CssClass="field_caption"></asp:Label></TD><TD style="WIDTH: 270px"><input type="text" name="accSearch"  class="field_input MyAutoCompleteClass" onfocus="MyAutoCustomerFillArray();"  style="width:98% ; font " id="accSearch"  runat="server" /><SELECT style="WIDTH: 280px" id="ddlSpplierName" class="field_input MyDropDownListCustValue" tabIndex=4 onchange="CallWebMethod('partyname');" runat="server"></SELECT> </TD><TD style="WIDTH: 56px"><INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtTranType" type=text value="PI" runat="server" /></TD><TD></TD></TR>
 <TR><TD style="WIDTH: 144px; HEIGHT: 22px">Post To A/C Code</TD><TD style="WIDTH: 212px; HEIGHT: 22px"><SELECT style="WIDTH: 160px" id="ddlPostToCode" class="field_input" tabIndex=5 onchange="CallWebMethod('posttocode');" runat="server"></SELECT></TD><TD style="WIDTH: 118px; HEIGHT: 22px">Post To A/C Name</TD><TD style="WIDTH: 270px; HEIGHT: 22px"><SELECT style="WIDTH: 280px" id="ddlPostToName" class="field_input" tabIndex=6 onchange="CallWebMethod('posttoname');" runat="server"></SELECT></TD><TD style="WIDTH: 56px; HEIGHT: 22px"></TD><TD style="HEIGHT: 22px"></TD></TR><TR><TD style="WIDTH: 144px; HEIGHT: 22px">Accrual A/C Code</TD><TD style="WIDTH: 212px; HEIGHT: 22px"><SELECT style="WIDTH: 160px" id="ddlAccrualCode" class="field_input" tabIndex=7 onchange="CallWebMethod('accuralcode');" runat="server"></SELECT></TD><TD style="WIDTH: 118px; HEIGHT: 22px">Accrual A/C Name</TD><TD style="WIDTH: 270px; HEIGHT: 22px"><SELECT style="WIDTH: 280px" id="ddlAccuralName" class="field_input" tabIndex=8 onchange="CallWebMethod('accuralname');" runat="server"></SELECT></TD><TD style="WIDTH: 56px; HEIGHT: 22px"></TD><TD style="HEIGHT: 22px"></TD></TR><TR><TD style="WIDTH: 144px">Control A/C Code</TD><TD style="WIDTH: 212px"><SELECT style="WIDTH: 160px" id="ddlControlCode" class="field_input" tabIndex=9 onchange="CallWebMethod('controlcode');" runat="server"></SELECT></TD><TD style="WIDTH: 118px">Control A/C Name</TD><TD style="WIDTH: 270px"><SELECT style="WIDTH: 280px" id="ddlControlName" class="field_input" tabIndex=10 onchange="CallWebMethod('sptypecode');" runat="server"></SELECT></TD><TD style="WIDTH: 56px"></TD><TD></TD></TR><TR><TD style="WIDTH: 144px">Supplier Invoice No</TD><TD style="WIDTH: 212px">
 <INPUT style="WIDTH: 128px" id="txtSInvoiceNo" class="field_input" tabIndex=11 type=text runat="server" /></TD><TD style="WIDTH: 118px">Suuplier Invoice Date</TD><TD style="WIDTH: 270px"><asp:TextBox id="txtSInvoiceDate" tabIndex=12 runat="server" Width="88px" CssClass="fiel_input" ValidationGroup="MKE"></asp:TextBox><asp:ImageButton id="imgbtnSInvoiceDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;<cc1:MaskedEditValidator id="MEVSInvoiceDate" runat="server" CssClass="field_error" ControlExtender="MESInvoiceDAte" ControlToValidate="txtSInvoiceDate" Display="Dynamic" InvalidValueBlurredMessage="Invalid supplier invoice date" InvalidValueMessage="Invalid Date" TooltipMessage="Enter a date in dd/mm/yyyy format" ErrorMessage="MEVSInvoiceDate" EmptyValueMessage="Supplier invoice date is required"></cc1:MaskedEditValidator></TD>
 <TD style="WIDTH: 56px"></TD><TD></TD></TR><TR><TD style="WIDTH: 144px">Narration</TD>
 <TD colSpan=2><SELECT style="WIDTH: 288px" id="ddlNarration" class="field_input" tabIndex=13 runat="server"> <OPTION selected></OPTION></SELECT></TD>
 <TD colSpan=2><INPUT style="WIDTH: 344px" id="txtNarration" class="field_input" tabIndex=14 type=text runat="server" /></TD><TD></TD></TR>
 <TR><TD style="WIDTH: 144px">Currency</TD><TD style="WIDTH: 212px" title=" "><INPUT style="WIDTH: 128px" id="txtCurrency" class="field_input" tabIndex=15 readOnly type=text runat="server" /></TD>
 <TD style="WIDTH: 118px"></TD><TD style="WIDTH: 270px"><INPUT style="VISIBILITY: hidden" id="txtExchRate" class="field_input" tabIndex=16 type=text runat="server" /></TD><TD style="WIDTH: 56px"></TD><TD></TD></TR><TR><TD style="WIDTH: 144px">From Date</TD>
 <TD style="WIDTH: 212px"><asp:TextBox id="txtFromDate" tabIndex=17 runat="server" Width="88px" CssClass="fiel_input" ValidationGroup="MKE"></asp:TextBox>
 <asp:ImageButton id="imgbtnFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVFFromDate" runat="server" CssClass="field_error" ControlExtender="MEVFromDate" ControlToValidate="txtFromDate" Display="Dynamic" InvalidValueBlurredMessage="Invalid from date" InvalidValueMessage="Invalid Date" TooltipMessage="Enter a date in dd/mm/yyyy format" ErrorMessage="MEVFFromDate" EmptyValueMessage="From date is required"></cc1:MaskedEditValidator></TD>
 <TD style="WIDTH: 118px">To Date</TD><TD style="WIDTH: 270px"><asp:TextBox id="txtToDate" tabIndex=18 runat="server" Width="88px" CssClass="fiel_input" ValidationGroup="MKE"></asp:TextBox><asp:ImageButton id="imgbtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" ControlExtender="METoDate" ControlToValidate="txtToDate" Display="Dynamic" InvalidValueBlurredMessage="Invalid from date" InvalidValueMessage="Invalid Date" TooltipMessage="Enter a date in dd/mm/yyyy format" ErrorMessage="MEVToDate" EmptyValueMessage="To date is required"></cc1:MaskedEditValidator></TD><TD style="WIDTH: 56px"><INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtdecimal" type=text maxLength=15 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtRowCount" type=text runat="server" /> </TD><TD>&nbsp;</TD></TR><TR><TD style="WIDTH: 144px">
    File Number</TD><TD style="WIDTH: 212px">
        <INPUT style="WIDTH: 128px" 
         id="txtxtrequestid" class="field_input"  runat="server" />
    </TD><TD style="WIDTH: 118px">&nbsp;</TD>
 <TD style="WIDTH: 270px">&nbsp;</TD><TD style="WIDTH: 56px; ">&nbsp;</TD><TD>
    &nbsp;</TD></TR><TR><TD style="WIDTH: 144px"></TD>
        <td style="WIDTH: 212px">
        </td>
        <td style="WIDTH: 118px">
            <asp:Label ID="lblPostmsg" runat="server" BackColor="#E0E0E0" 
                CssClass="field_caption" Font-Bold="True" Font-Names="Verdana" Font-Size="12px" 
                ForeColor="Green" Text="UnPosted" Width="131px"></asp:Label>
        </td>
        <td style="WIDTH: 270px">
        </td>
        <td style="WIDTH: 56px; TEXT-ALIGN: right">
            <asp:Button ID="btnDisplay" runat="server" CssClass="btn" tabIndex="19" 
                Text="Display" />
        </td>
        <td>
            <asp:Button ID="btnClear" runat="server" CssClass="btn" tabIndex="20" 
                Text="Clear" />
        </td>
    </TR><TR><TD colSpan=6>
        <div id="divRes" runat="server" class="container" style="HEIGHT: 300px">
            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                CssClass="td_cell" Font-Italic="False" Font-Size="10px" GridLines="Vertical" 
                tabIndex="21" Width="992px">
                <Columns>
                    <asp:BoundField HeaderText="Sr No.">
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="acc_tran_id" HeaderText="Invoice No">
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="requestid" HeaderText="File Number">
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="particulars" HeaderText="Particulars">
                        <ItemStyle Width="250px" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Value">
                        <ItemTemplate>
                            <INPUT style="WIDTH: 80px; TEXT-ALIGN: right" id="txtvalue" class="field_input" 
type=text runat="server" value='<%# Bind("Value") %>' />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PI Y/N">
                        <ItemTemplate>
                            <INPUT id="chkPIYN" type=checkbox runat="server" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Exch. Rate">
                        <ItemTemplate>
                            <INPUT style="WIDTH: 80px" id="txtConvRate" class="field_input" disabled type=text value='<%# Bind("convrate") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Invoice Value">
                        <ItemTemplate>
                            <INPUT style="WIDTH: 84px; TEXT-ALIGN: right" id="txtSInvoiceValue" class="field_input" type=text runat="server" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Invoice Value (Base)">
                        <ItemTemplate>
                            <INPUT style="WIDTH: 84px; TEXT-ALIGN: right" id="txtSInvoiceValueKWD" class="field_input" readOnly type=text runat="server" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Diff Amount (Base)">
                        <ItemTemplate>
                            <INPUT style="WIDTH: 64px; TEXT-ALIGN: right" id="txtDiffAmountKWD" class="field_input" readOnly type=text runat="server" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Narration">
                        <ItemTemplate>
                            <INPUT id="txtNarration" class="field_input" type=text runat="server" style="width: 128px" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Post Diff A/C">
                        <ItemTemplate>
                    <SELECT style="WIDTH: 220px" id="ddlPostdiffacName" class="field_input" tabIndex=6 
                         runat="server"></SELECT>
                            <select id="ddlPostDiffAC" runat="server" class="field_input" 
                                style="WIDTH: 101px" disabled="disabled">
                                <option selected=""></option>
                            </select>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="acc_tran_type " Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblAccTranType" runat="server" 
                                Text='<%# Bind("[acc_tran_type ]") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="acc_tran_lineno" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblAccTranLineno" runat="server" 
                                Text='<%# Bind("acc_tran_lineno") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <controlstyle backcolor="White" bordercolor="White" borderstyle="None" />
                        <ItemStyle BackColor="White" BorderColor="White" BorderStyle="None" />
                        <HeaderStyle BackColor="White" BorderColor="White" BorderStyle="None" />
                        <ItemTemplate>
                            <INPUT style="VISIBILITY: hidden; WIDTH: 1px; TEXT-ALIGN: right" id="txtDiffAmount" class="field_input" readOnly type=text runat="server" />
                        </ItemTemplate>
                        <FooterStyle BorderColor="White" CssClass="grdfooter" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="othgrpup" Visible="False">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdngroup" runat="server"  Value='<%# Bind("othgrpcode") %>'/>
                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>
                <RowStyle CssClass="grdRowstyle" Wrap="False" />
                <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center" 
                    Wrap="False" />
                <HeaderStyle CssClass="grdheader" ForeColor="white" Wrap="False" />
                <AlternatingRowStyle CssClass="grdAternaterow" />
            </asp:GridView>
        </div>
        <asp:Label id="lblMsg" runat="server" Text="Records not found." Width="152px" 
            Visible="False" CssClass="lblmsg" Font-Bold="True" Font-Names="Verdana"></asp:Label> </TD></TR><TR>
        <TD align="center" colspan="6" style="HEIGHT: 7px">
            <asp:Label ID="Label3" runat="server" Text="Invoice Total" Visible="False" 
                Width="110px"></asp:Label>
            <INPUT style="WIDTH: 100px; text-align: right;" id="txtInvoiceTotal" type=text runat="server" class="field_input" readOnly value="0" Visible="true" /> 
            <asp:Label ID="lblTotal" runat="server" Text="Supplier Invoice Total" 
                Width="134px"></asp:Label>
    <input id="txtTotalSInvoiceValue" runat="server" style="width: 100px;
        text-align: right;" type="text" class="field_input" readOnly value="0" Visible="true" />
            <asp:Label ID="Label1" runat="server" Text="Base Supplier Invoice Total" 
                Width="161px"></asp:Label>
            &nbsp; 
            <INPUT style="WIDTH: 100px; text-align: right;" id="txtTotalSInvoiceValueKWD" type=text runat="server" class="field_input" value="0" /> 
            <asp:Label ID="Label2" runat="server" Text="Base Diff Amount Total" 
                Width="149px"></asp:Label>
            <INPUT style="WIDTH: 100px; text-align: right;" id="txtTotalDiffAmountKWD" type=text runat="server" class="field_input" readOnly value="0" Visible="true" />
        </TD></TR>
    <TR><TD>&nbsp;<INPUT 
                    style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtbasecurr" type=text 
                    maxLength=20 runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtPostCode" 
                    type=text maxLength=20 runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtPostName" 
                    type=text maxLength=20 runat="server" />
        <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
        </TD><TD>
            <INPUT id="btnSelectAll" class="btn" tabIndex=22 onclick="SelectAll()" type=button 
                    value="SelectAll" runat="server" />
            <INPUT id="btnDeSelectAll" class="btn" tabIndex=23 onclick="DeSelectAll()" type=button 
                    value="DeSelectAll" runat="server" />
        </TD><TD></TD><TD colspan="3">
        <asp:CheckBox ID="chkPost" runat="server" BackColor="#FFC0C0" 
            CssClass="field_caption" Font-Bold="True" Font-Names="Verdana" Font-Size="10px" 
            ForeColor="Black" tabIndex="24" Text="Post/UnPost" Width="103px" />
        <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="25" 
            Text="Save" />
        &nbsp;
        <asp:Button ID="btnPrint" runat="server" CssClass="btn" 
            onclick="btnPrint_Click" tabIndex="26" Text="Print" />
        &nbsp;<asp:Button ID="btnExit" runat="server" CssClass="btn" tabIndex="27" 
            Text="Exit" Width="40px" />
        &nbsp;
        <asp:Button ID="btnHelp" runat="server" CssClass="btn" onclick="btnHelp_Click" 
            tabIndex="28" Text="Help" />
        </TD></TR>
    <tr>
        <td style="WIDTH: 144px; HEIGHT: 16px">
        </td>
        <td style="WIDTH: 220px; HEIGHT: 16px; TEXT-ALIGN: left">
        </td>
        <td style="WIDTH: 118px; HEIGHT: 16px">
        </td>
        <td style="WIDTH: 270px; HEIGHT: 16px">
        </td>
        <td style="WIDTH: 56px; HEIGHT: 16px">
        </td>
        <td style="HEIGHT: 16px">
        </td>
    </tr>
    </TBODY></TABLE><cc1:CalendarExtender id="CEPInvocieDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnPInvoiceDate" TargetControlID="txtPInvoiceDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEPInvoiceDate" runat="server" TargetControlID="txtPInvoiceDate" ErrorTooltipEnabled="True" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CESInvocieDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnSInvoiceDate" TargetControlID="txtSInvoiceDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MESInvoiceDAte" runat="server" TargetControlID="txtSInvoiceDate" ErrorTooltipEnabled="True" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender><cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnFromDate" TargetControlID="txtFromDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEVFromDate" runat="server" TargetControlID="txtFromDate" ErrorTooltipEnabled="True" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnToDate" TargetControlID="txtToDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" ErrorTooltipEnabled="True" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> 
                        <asp:HiddenField ID="hdnSS" runat="server" Value="0" />
</contenttemplate>
                </asp:UpdatePanel><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy></td>
        </tr>
    </table>
</asp:Content>
