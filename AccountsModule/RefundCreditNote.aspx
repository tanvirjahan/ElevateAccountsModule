<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="RefundCreditNote.aspx.vb" Inherits="AccountsModule_RefundCreditNote"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"   TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server" >
    <script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript" >
function CallWebMethod(methodType)
{
    switch(methodType)
    {
        case "Refund":      
            var select=document.getElementById("<%=ddlRefundId.ClientID%>");
            var codeid=select.value;                
            if(codeid != '[Select]')
            {      
            var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
            constr=connstr.value   

                ColServices.clsServices.GetRefundDetailsnew(constr,codeid,FillRefundDetails,ErrorHandler,TimeOutHandler);
            }                    
    }
}
function FillRefundDetails(result)
{
    var txtRequestId = document.getElementById("<%=txtRequestId.ClientID%>"); 		
	var txtInvoiceNo = document.getElementById("<%=txtInvoiceNo.ClientID%>"); 		
    var ddlRefundType = document.getElementById("<%=ddlRefundType.ClientID%>"); 		
	var txtCurrency = document.getElementById("<%=txtCurrency.ClientID%>"); 
	var txtCustomerCode = document.getElementById("<%=txtCustomerCode.ClientID%>"); 		
	var txtCustomerName = document.getElementById("<%=txtCustomerName.ClientID%>");
    var txtRefundSaleValue= document.getElementById("<%=txtRefundSaleValue.ClientID%>");	
    var txtRefundTotal= document.getElementById("<%=txtRefundTotal.ClientID%>");	

    txtRequestId.value=result[0];	
    txtInvoiceNo.value=result[1]; 		
    ddlRefundType.value=result[2];
    txtCurrency.value=result[5];
    txtCustomerCode.value=result[3];
    txtCustomerName.value=result[4];
    txtRefundSaleValue.value=result[6];
    txtRefundTotal.value=result[6];
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
function checkNumberDecimal(evt,txt) 
{
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :((evt.which) ? evt.which : 0));
    if (charCode != 47 && (charCode > 44 && charCode < 58))
    { 
        var value=txt.value;
        var indx=value.indexOf('.');
        var deci=document.getElementById("<%=txtdecimal.ClientID%>");
        var lngLenght =deci.value ;  
        if (indx < 0 )
        {
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
function CalculateRefTotal() 
{
    var txtRefundSaleValue=document.getElementById("<%=txtRefundSaleValue.ClientID%>");
    var txtRefundCharges=document.getElementById("<%=txtRefundCharges.ClientID%>");
    var txtRefundOther=document.getElementById("<%=txtRefundOther.ClientID%>");
    var txtRefundTotal=document.getElementById("<%=txtRefundTotal.ClientID%>");
    var total=0;
    if(txtRefundSaleValue!=null)
    {
        if(txtRefundSaleValue.value!='')
        {
            total=total+parseFloat(txtRefundSaleValue.value)
        }    
    }
    if(txtRefundCharges!=null)
    {
        if( txtRefundCharges.value !='')
        {
            total=total+parseFloat(txtRefundCharges.value)
        }    
    }
    if(txtRefundOther!=null)
    {
        if(txtRefundOther.value!='')
        {
            total=total+parseFloat(txtRefundOther.value)
        }    
    }
    txtRefundTotal.value =DecRound(total)
}
function DecRound(amtToRound) 
{
   var  txtdec= document.getElementById("<%=txtdecimal.ClientID%>");
   nodecround=Math.pow(10,parseInt(txtdec.value));
   var rdamt=Math.round(parseFloat(amtToRound)*nodecround)/nodecround;
   return  parseFloat(rdamt);
}
var ddlctacct = null;
var lblctacct = null;
var hdnacc= null;
function FillAccCode(ddlac,ddln,acctype,ddlCtrlAccount,lblCtrlAccount,hdnac)
{
    ddlACode = document.getElementById(ddlac);
    ddlAName = document.getElementById(ddln);

//    alert(acctype);
    var codeid=ddlACode.options[ddlACode.selectedIndex].text;
    ddlAName.value=codeid;
    fillCotrolAccountCode(codeid,acctype,ddlCtrlAccount,lblCtrlAccount,hdnac);    
}   
function fillCotrolAccountCode(codeid,acctype,ddlCtrlAccount,lblCtrlAccount,hdnac)
{

    var sqlstr1,sqlstr2

    if(codeid != '[Select]')
    {
        if(acctype=='C')
        {
            lblctacct=document.getElementById(lblCtrlAccount);
            hdnacc=document.getElementById(hdnac);
            lblctacct.style.visibility="visible";
            //ddlctacct.style.visibility="hidden";  
            var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
            constr=connstr.value   

            
            ColServices.clsServices.GetCtrlAccCodenew(constr,codeid,FillCtrlAccount,ErrorHandler,TimeOutHandler);
        }
        if(acctype=='S')
        {
            ddlctacct=document.getElementById(ddlCtrlAccount);
            //lblctacct.style.visibility="hidden";
            ddlctacct.style.visibility="visible"; 
            var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
            constr=connstr.value   

         //   sqlstr1=" select distinct partymast.controlacctcode  , partymast.controlacctcode  from partymast where  partymast.partycode='"+ codeid+"' order by partymast.controlacctcode"  
         //   ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillCtrlAccountSupp,ErrorHandler,TimeOutHandler);
          ColServices.clsServices.GetCtrlAccCodeSuppnew(constr,codeid,FillCtrlAccountSupp,ErrorHandler,TimeOutHandler);         
        }
        if(acctype=='A')
        {
            ddlctacct=document.getElementById(ddlCtrlAccount);
            //lblctacct.style.visibility="hidden";
            ddlctacct.style.visibility="visible"; 
            var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
            constr=connstr.value   

         //   sqlstr1=" select distinct supplier_agents.controlacctcode,supplier_agents.controlacctcode  from supplier_agents where   supplier_agents.supagentcode='"+ codeid+"' order by supplier_agents.controlacctcode"  

          //  ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr1,FillCtrlAccountSupp,ErrorHandler,TimeOutHandler);
           ColServices.clsServices.GetCtrlAccCodeSuppnew(constr,codeid,FillCtrlAccountSupp,ErrorHandler,TimeOutHandler);         
        }        
    } 
}
function FillCtrlAccount(result)
{
    lblctacct.innerHTML=result;
    hdnacc.value=result;
}
function FillCtrlAccountSupp(result)
{
    	
    for (var j = ddlctacct.length - 1; j>=0; j--) 
    {
        ddlctacct.remove(j);
    }   
    for(var i=0;i<result.length;i++)
    {
        var option=new Option(result[i].ListText,result[i].ListValue);
        ddlctacct.options.add(option);
    }     
}
function FillAccName(ddlac,ddln,acctype,ddlCtrlAccount,lblCtrlAccount,hdnac)
{
    ddlACode = document.getElementById(ddlac);
    ddlAName = document.getElementById(ddln);
    var codeid=ddlAName.options[ddlAName.selectedIndex].text;
    ddlACode.value=codeid;
    fillCotrolAccountCode(ddlAName.value,acctype,ddlCtrlAccount,lblCtrlAccount,hdnac); 
}
function GetKWDDrCr(txtd,txtc,kwdd,kwdc,er,strVal)
{
    strval=strVal
    txtdamt = document.getElementById(txtd);
    txtcamt = document.getElementById(txtc);
    txtkwddamt = document.getElementById(kwdd);
    txtkwdcamt = document.getElementById(kwdc); 
    txterate = document.getElementById(er); 
    
    var decdTotal=parseFloat(txtdamt.value)*parseFloat(txterate.value);
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
   
    ColServices.clsServices.RoundwithParameter(decdTotal,FillkwddAmount,ErrorHandler,TimeOutHandler);   
     
   // txtkwdcamt.value=DecRound(txtcamt.value)*DecRound(txterate.value);
    var deccTotal=parseFloat(txtcamt.value)*parseFloat(txterate.value);
    ColServices.clsServices.RoundwithParameter(deccTotal,FillkwdcAmount,ErrorHandler,TimeOutHandler);   
}

function ValidateData()
{
    var txtRefundSaleValue=document.getElementById("<%=txtRefundSaleValue.ClientID%>");
    var txtRefundCharges=document.getElementById("<%=txtRefundCharges.ClientID%>");
    var txtRefundOther=document.getElementById("<%=txtRefundOther.ClientID%>");
    var ddlRefundId=document.getElementById("<%=ddlRefundId.ClientID%>");
    
    var txtRequestId=document.getElementById("<%=txtRequestId.ClientID%>");
    var txtInvoiceNo=document.getElementById("<%=txtInvoiceNo.ClientID%>");
    var txtCustomerCode=document.getElementById("<%=txtCustomerCode.ClientID%>");
    if(txtRequestId.value == '')
    {
        alert('Please Enter RequestId');
        return false;
    }    
    if(txtInvoiceNo.value == '')
    {
        alert('Please Enter Invoice No');
        return false;
    } 
    if(txtCustomerCode.value == '')
    {
        alert('Please Enter CustomerCode');
        return false;
    }    
    
    if(ddlRefundId.value == '[Select]')
    {
        alert('Please Select a Request');
        return false;
    }    
    if (txtRefundSaleValue.value == '')
    {
        alert('Refund Sale Value Can not be blank');
        return false;
    }
    /*if (txtRefundCharges.value == '')
    {
        alert('Refund Charges Can not be blank')
        return false;
    }
    if (txtRefundOther.value == '')
    {
        alert('Refund Other Can not be blank')
        return false;
    }*/
    
    return true;
  
}

//**************
 var nodecround =null;
 var txtkwddamt=null;
 var txtkwdcamt=null;
 var strval=null;
// var txttotDr=null;
// var txttotCr=null;
// var txttotDrCP=null;
// var txttotCrCP=null;

function trim(stringToTrim) 
{
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
   

function FillkwddAmount(result)
{
  txtkwddamt.value=result;
  grdTotal(strval); 
}
function FillkwdcAmount(result)
{
  txtkwdcamt.value=result;
  grdTotal(strval);     
}

function grdTotal(str)
{
    var  txtdec=document.getElementById('<%=txtdecimal.ClientID%>');
    nodecround=Math.pow(10,parseInt(txtdec.value));
  
    if (str=='IP')
    {
     var objGridView = document.getElementById('<%=gv_IncomePosting.ClientID%>');
     var txtrowcnt =document.getElementById('<%=txtgridrowsip.ClientID%>');
     var j=0;
     var valtotDr=0;
     var valtotCr=0;
     intRows=txtrowcnt.value;
       
     for(j=1;j<=intRows;j++)
     {
     
        var valDr=objGridView.rows[j].cells[10].children[0].value;
        var valCr=objGridView.rows[j].cells[11].children[0].value;
        var valExchRate=objGridView.rows[j].cells[7].children[0].value;
        if (valDr==''){valDr=0;}
        if (valCr==''){valCr=0;}
        if (isNaN(valDr)==true){valDr=0;}
        if (isNaN(valCr)==true){valCr=0;}
        
       valtotDr=parseFloat(valtotDr)+parseFloat(valDr);
       valtotCr=parseFloat(valtotCr)+parseFloat(valCr);
    }
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
    
    ColServices.clsServices.RoundwithParameter(parseFloat(valtotDr),FilltxttotDr,ErrorHandler,TimeOutHandler);  
    ColServices.clsServices.RoundwithParameter(parseFloat(valtotCr),FilltxttotCr,ErrorHandler,TimeOutHandler);  
    }
   else if (str=='CP')
    {
      var objGridView = document.getElementById('<%=gv_CostPosting.ClientID%>');
      var txtrowcnt=document.getElementById('<%=txtgridrowscp.ClientID%>');
      var j=0;
      var valtotDr=0;
      var valtotCr=0;
      intRows=txtrowcnt.value;

     for(j=1;j<=intRows;j++)
      {
       var valDr=objGridView.rows[j].cells[10].children[0].value;
       var valCr=objGridView.rows[j].cells[11].children[0].value;
       var valExchRate=objGridView.rows[j].cells[7].children[0].value;  
     
       if (valDr==''){valDr=0;}
       if (valCr==''){valCr=0;}
       if (isNaN(valDr)==true){valDr=0;}
       if (isNaN(valCr)==true){valCr=0;}
       
       valtotDr=parseFloat(valtotDr)+parseFloat(valDr);
       valtotCr=parseFloat(valtotCr)+parseFloat(valCr);
       
    }
    ColServices.clsServices.RoundwithParameter(parseFloat(valtotDr),FilltxttotDrCP,ErrorHandler,TimeOutHandler);  
    ColServices.clsServices.RoundwithParameter(parseFloat(valtotCr),FilltxttotCrCP,ErrorHandler,TimeOutHandler);
    }
  }

function FilltxttotDr(result)
{
var txttotDr = document.getElementById('<%=txtTotalKWDDebit.ClientID%>');
txttotDr.value=result;
}

function FilltxttotCr(result)
{
   var txttotCr  = document.getElementById('<%=txtTotalKWDCredit.ClientID%>');
   txttotCr.value=result;
}

function FilltxttotDrCP(result)
{
 var txttotDrCP = document.getElementById('<%=txtTotalKWDDebitCP.ClientID%>');
txttotDrCP.value=result;
}

function FilltxttotCrCP(result)
{
   var txttotCrCP  = document.getElementById('<%=txtTotalKWDCreditCP.ClientID%>');
   txttotCrCP.value=result;
}

function ValidatePage()
{
     
     var objGridView = document.getElementById('<%=gv_IncomePosting.ClientID%>');
     var txtrowcnt = document.getElementById('<%=txtgridrowsip.ClientID%>');
     var hdnSS=document.getElementById("<%=hdnSS.ClientID%>");
     
     intRows=txtrowcnt.value;

     for(j=1;j<=intRows;j++)
     {  
        var valExchRate=objGridView.rows[j].cells[7].children[0].value;
        if (valExchRate<=0)
        {
            alert('Income posting grid exch rate greater than zero.');
            return false;
        }
        var valAcctCode=objGridView.rows[j].cells[3].children[0].value;
        if (valAcctCode=='[Select]')
        {
            alert('Please select account code from income posting grid.');
            return false;
        }
     }
          
    var ddlRefdType  = document.getElementById('<%=ddlRefundType.ClientID%>');
    var txttotCr  = document.getElementById('<%=txtTotalKWDDebit.ClientID%>');
    var txttotbCr = document.getElementById('<%=txtTotalKWDCredit.ClientID%>');
    var txtbasecurr = document.getElementById('<%=txtbasecurr.ClientID%>');

    if(ddlRefdType.value== 0 || ddlRefdType.value== 1)
    {
        if (txttotCr.value != txttotbCr.value)
        {
            alert('Income posting,total debit and  credit must be equal.');
            return false;
        }
    }
    
    var objGridViewcp = document.getElementById('<%=gv_CostPosting.ClientID%>');
    var txtrowcntcp = document.getElementById('<%=txtgridrowscp.ClientID%>');
    intRowscp=txtrowcntcp.value;
     
     for(j=1;j<=intRowscp;j++)
     { 
        var valExchRatecp=objGridViewcp.rows[j].cells[7].children[0].value; 
       if (valExchRatecp<=0)
        {
            alert('Cost posting grid exch rate greater than zero.');
            return false;
        }
        var valAcctCodecp=objGridViewcp.rows[j].cells[3].children[0].value;
        if (valAcctCodecp=='[Select]')
        {
            alert('Please select account code from cost posting grid.');
            return false;
        }
    }
 
      var txttotCrcp  = document.getElementById('<%=txtTotalKWDDebitCP.ClientID%>');
      var txttotbCrcp = document.getElementById('<%=txtTotalKWDCreditCP.ClientID%>');
    if(ddlRefdType.value== 0 || ddlRefdType.value== 2)
    {
        if (parseFloat(txttotCrcp.value)!= parseFloat(txttotbCrcp.value)) {
            if (objGridViewcp != null) {
                if (objGridViewcprows.count > 0) {
                    alert('Cost posting, total  debit and  credit must be equal.');
                    return false;
                }
            }
        }
    }

   hdnSS.value=0; 
   validate_click();
     
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
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid" class="td_cell"><TBODY><TR><TD align=center colSpan=3><asp:Label id="lblHeading" runat="server" Text="Refund Credit Notes Form" Width="1088px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD colSpan=3><TABLE><TBODY><TR><TD style="width: 106px"><asp:Label id="Label1" runat="server" Text="Credit Note No" Width="92px" CssClass="field_caption"></asp:Label></TD><TD style="WIDTH: 100px"><INPUT style="WIDTH: 135px" id="txtCreditNoteNo" class="field_input" readOnly type=text maxLength=20 runat="server" /></TD><TD style="WIDTH: 100px"><asp:Label id="Label9" runat="server" Text="Credit Note Date" Width="130px" CssClass="field_caption"></asp:Label></TD><TD><ews:DatePicker id="dpCreditNoteDate" tabIndex=1 runat="server" Width="225px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy" CssClass="field_input"></ews:DatePicker> </TD>
    <td>
        <asp:Label ID="Label16" runat="server" CssClass="field_caption" Text="Refund Id"
            Width="54px"></asp:Label></td>
    <td style="width: 116px">
    <select style="WIDTH: 180px" id="ddlRefundId" class="field_input" onchange="CallWebMethod('Refund');" runat="server" tabindex="2"> 
   </select>
</td>
</TR><TR><TD style="width: 106px"><asp:Label id="Label3" runat="server" Text="File Number" Width="92px" CssClass="field_caption"></asp:Label></TD><TD><INPUT style="WIDTH: 135px" id="txtRequestId" class="field_input" readOnly type=text maxLength=20 runat="server" /></TD><TD><asp:Label id="Label10" runat="server" Text="Invoice No" Width="130px" CssClass="field_caption"></asp:Label></TD><TD><INPUT id="txtInvoiceNo" class="field_input" readOnly type=text runat="server" style="width: 160px" /></TD>
    <td>
    </td>
    <td style="width: 116px">
    </td>
</TR><TR><TD style="width: 106px"><asp:Label id="Label4" runat="server" Text="Refund Type" Width="92px" CssClass="field_caption"></asp:Label></TD><TD>
    <select style="WIDTH: 138px" id="ddlRefundType" class="field_input" runat="server" disabled="disabled"> 
        <option value="0" selected>Both</option>
        <option value="1">Customer Only</option>
        <option value="2">Supplier Only</option>
    </select>
    </TD><TD><asp:Label id="Label6" runat="server" Text="Currency" Width="130px" CssClass="field_caption"></asp:Label></TD><TD><INPUT style="WIDTH: 160px" id="txtCurrency" class="field_input" readOnly type=text maxLength=20 runat="server" /></TD>
    <td>
    </td>
    <td style="width: 116px">
    </td>
</TR>
    <tr>
        <td style="width: 106px">
            <asp:Label ID="Label11" runat="server" CssClass="field_caption" Text="Customer Code"
                Width="92px"></asp:Label></td>
        <td>
            <INPUT style="WIDTH: 135px" id="txtCustomerCode" class="field_input" readOnly type=text maxLength=20 runat="server" /></td>
        <td>
            <asp:Label ID="Label17" runat="server" CssClass="field_caption" Text="Customer Name"
                Width="130px"></asp:Label></td>
        <td>
            <INPUT style="WIDTH: 160px" id="txtCustomerName" class="field_input" readOnly type=text maxLength=20 runat="server" /></td>
        <td>
        </td>
        <td style="width: 116px">
        </td>
    </tr>
    <TR>
<TD style="width: 106px">
    <asp:Label id="Label2" runat="server" Text="Refund Sale Value" Width="92px" CssClass="field_caption"></asp:Label>
</TD>
<TD>
    <INPUT id="txtRefundSaleValue" class="field_input" onchange="CalculateRefTotal()" onkeypress ="return checkNumberDecimal(event,this)"  type=text maxLength=20 runat="server" tabindex="3" />
</TD>
<TD>
    <asp:Label ID="Label14" runat="server" CssClass="field_caption" Text="Refund Charges"
        Width="130px"></asp:Label>
</TD>
<TD>
    <INPUT id="txtRefundCharges" class="field_input" onchange="CalculateRefTotal()" onkeypress ="return checkNumberDecimal(event,this)" type=text maxLength=20 runat="server" tabindex="4" />
</TD>
<td>
    <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="Other Charges"
            Width="82px"></asp:Label>
</td>
<td style="width: 116px">
    <INPUT id="txtRefundOther" class="field_input" onchange="CalculateRefTotal()" onkeypress ="return checkNumberDecimal(event,this)" type=text maxLength=20 runat="server" tabindex="5" />
</td>
</TR>
<TR>
<TD style="width: 106px">
<asp:Label id="Label7" runat="server" Text="Refund Total" Width="92px" CssClass="field_caption"></asp:Label>
</TD><TD>
<INPUT id="txtRefundTotal" class="field_input" readOnly type=text maxLength=20 runat="server" />
</TD><TD></TD><TD>
    <asp:Button ID="btndisPos" runat="server" CssClass="btn" OnClick="btndisPos_Click"
        TabIndex="6" Text="Display Posting" OnClientClick="return ValidateData();" /></TD>
    <td>
    </td>
    <td style="width: 116px">
    </td>
</TR><TR><TD style="width: 106px"></TD><TD></TD><TD></TD><TD></TD>
    <td>
    </td>
    <td style="width: 116px">
    </td>
</TR><TR><TD style="width: 106px"></TD><TD style="WIDTH: 100px; HEIGHT: 18px"></TD><TD></TD><TD></TD>
    <td>
    </td>
    <td style="width: 116px">
    </td>
</TR><TR><TD style="width: 106px"></TD><TD style="WIDTH: 100px"></TD><TD></TD><TD></TD>
    <td>
    </td>
    <td style="width: 116px">
    </td>
</TR></TBODY></TABLE></TD></TR><TR><TD align=center colSpan=3><asp:Label id="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana" Width="155px" CssClass="field_caption" ForeColor="Green" Font-Bold="True" BackColor="#E0E0E0" Visible="False"></asp:Label></TD></TR><TR><TD style="HEIGHT: 170px" colSpan=3><DIV style="HEIGHT: 150px" class="container"><asp:GridView id="gv_IncomePosting" tabIndex=7 runat="server" CssClass="grdstyle" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="requesttype" HeaderText="Request Type"></asp:BoundField>
<asp:BoundField DataField="requestlineno" HeaderText="S No"></asp:BoundField>
<asp:BoundField DataField="acc_type" HeaderText="Account Type"></asp:BoundField>
<asp:TemplateField HeaderText="Account code">
    <ItemTemplate>
        <select id="ddlAcctCode" runat="server" class="field_input" style="width: 96px">
            <option selected="selected"></option>
        </select>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Account Name">
    <ItemTemplate>
        <select id="ddlAcctName" runat="server" class=" field_input" style="width: 176px">
            <option selected="selected"></option>
        </select>               
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Control Account Code">
    <ItemTemplate>
        <select id="ddlCtrlAName" runat="server" class=" field_input" style="width: 120px">
            <option selected="selected"></option>
        </select>  
        <asp:Label ID="lblControlAcctCode" runat="server" Text='<%# Bind("controlcode") %>'></asp:Label>
        <asp:HiddenField ID="hdnCtrlAccCode" runat="server" Value='<%# Bind("controlcode") %>' />
    </ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="currcode" HeaderText="Currency Code"></asp:BoundField>
<asp:TemplateField HeaderText="Exch Rate">
    <ItemTemplate>
        <input id="txtExchRate" value='<%# bind("convrate") %>' runat="server" class="field_input" maxlength="12"
                                    style="width: 56px; text-align: right;" type="text" />
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Debit">
    <ItemTemplate>
        <input id="txtDebit" value='<%# bind("debit") %>' runat="server" class="field_input" type="text" style="width: 72px; text-align: right;" />
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Credit">
    <ItemTemplate>
        <input id="txtCredit" value='<%# bind("credit") %>' runat="server" class="field_input" type="text" style="width: 72px; text-align: right;" />
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Debit">
    <ItemTemplate>
        <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDDebit" class="field_input" disabled type=text value='<%# bind("basedebit") %>' runat="server" /> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Credit">
    <ItemTemplate>
        <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDCredit" class="field_input" disabled type=text value='<%# bind("basecredit") %>' runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="narration" HeaderText="Narration"></asp:BoundField>
<asp:TemplateField Visible="False" HeaderText="Supplier Name">
    <ItemTemplate>
        <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Check In">
    <ItemTemplate>
        <asp:Label ID="lblCheckIn" runat="server" Text='<%# Bind("datein") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Check Out">
    <ItemTemplate>
        <asp:Label ID="lblCheckOut" runat="server" Text='<%# Bind("dateout") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="ReconfNo">
    <ItemTemplate>
        <asp:Label ID="lblReConfNo" runat="server" Text='<%# Bind("reconfno") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Acc_Lineno">
    <ItemTemplate>
        <asp:Label ID="lblAccLineno" runat="server" Text='<%# Bind("acc_lineno") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="rlineno">
    <ItemTemplate>
        <asp:Label ID="lblRlineNo" runat="server" Text='<%# Bind("rlineno") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="slineno">
    <ItemTemplate>
        <asp:Label ID="lblSLineno" runat="server" Text='<%# Bind("slineno") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Account code">
    <ItemTemplate>
        <asp:Label ID="lblAcctCode" runat="server" Text='<%# Bind("acc_code") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Account Name">
    <EditItemTemplate>
        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("acc_name") %>'></asp:TextBox>
    </EditItemTemplate>
    <ItemTemplate>
        <asp:Label ID="lblAcctName" runat="server" Text='<%# Bind("acc_name") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black" Wrap="False"></RowStyle>

<PagerStyle CssClass="grdpagerstyle" Wrap="False"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" Wrap="False"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> </DIV>
<asp:Label id="lblMessageIP" runat="server" Font-Size="9pt" Font-Names="Verdana" 
        Font-Bold="True" Visible="False" CssClass="lblmsg">Records Not Found.</asp:Label></TD></TR><TR><TD><INPUT style="VISIBILITY: hidden; WIDTH: 8px" id="txtgridrowsip" type=text runat="server" /> 
<INPUT style="VISIBILITY: hidden; WIDTH: 8px" id="txtdecimal" type="text"  runat="server" />
    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD></TD><TD align=center><TABLE><TBODY><TR><TD>Total  Debit &nbsp;</TD><TD><INPUT style="TEXT-ALIGN: right" id="txtTotalKWDDebit" class="field_input" readOnly type=text runat="server" /></TD><TD>Total  Credit</TD><TD><INPUT style="TEXT-ALIGN: right" id="txtTotalKWDCredit" class="field_input" readOnly type=text runat="server" /></TD><TD style="WIDTH: 70px"></TD><TD style="WIDTH: 130px"></TD></TR></TBODY></TABLE></TD></TR><TR><TD colSpan=3><DIV style="HEIGHT: 200px" class="container"><asp:GridView id="gv_CostPosting" tabIndex=8 runat="server" CssClass="grdstyle" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="requesttype" HeaderText="Request Type"></asp:BoundField>
<asp:BoundField DataField="requestlineno" HeaderText="S No"></asp:BoundField>
<asp:BoundField DataField="acc_type" HeaderText="Account Type"></asp:BoundField>
<asp:TemplateField HeaderText="Account code"><ItemTemplate>
                            <select id="ddlAcctCode" runat="server" class="field_input" style="width: 96px">
                                <option selected="selected"></option>
                            </select>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Account Name"><ItemTemplate>
                            <select id="ddlAcctName" runat="server" class=" field_input" style="width: 176px">
                                <option selected="selected"></option>
                            </select>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Control Account Code">
    <ItemTemplate>
        <select id="ddlCtrlAName" runat="server" class=" field_input" style="width: 120px">
            <option selected="selected"></option>
        </select>  
        <asp:Label ID="lblControlAcctCode" runat="server" Text='<%# Bind("controlcode") %>'></asp:Label>
        <asp:HiddenField ID="hdnCtrlAccCode" runat="server" Value='<%# Bind("controlcode") %>' />
    </ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="currcode" HeaderText="Currency Code"></asp:BoundField>
<asp:TemplateField HeaderText="Exch Rate"><ItemTemplate>
    <input id="txtExchRate" value='<%# bind("convrate") %>' runat="server" class="field_input" maxlength="12"
                                    style="width: 56px; text-align: right;" type="text" />
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Debit"><ItemTemplate>
                            <input id="txtDebit" value='<%# bind("debit") %>' runat="server" class="field_input" type="text" style="width: 72px; text-align: right;" />
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Credit"><ItemTemplate>
                            <input id="txtCredit" value='<%# bind("credit") %>' runat="server" class="field_input" type="text" style="width: 72px; text-align: right;" />
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Debit"><ItemTemplate>
<INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDDebit" class="field_input" disabled type=text value='<%# bind("basedebit") %>' runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Credit"><ItemTemplate>
<INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtKWDCredit" class="field_input" disabled type=text value='<%# bind("basecredit") %>' runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="narration" HeaderText="Narration"></asp:BoundField>
<asp:TemplateField Visible="False" HeaderText="Supplier Name"><ItemTemplate>
                            <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Check In"><ItemTemplate>
                            <asp:Label ID="lblCheckIn" runat="server" Text='<%# Bind("datein") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Check Out"><ItemTemplate>
                            <asp:Label ID="lblCheckOut" runat="server" Text='<%# Bind("dateout") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="ReconfNo"><ItemTemplate>
                            <asp:Label ID="lblReConfNo" runat="server" Text='<%# Bind("reconfno") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Acc_Lineno"><ItemTemplate>
                            <asp:Label ID="lblAccLineno" runat="server" Text='<%# Bind("acc_lineno") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="rlineno"><ItemTemplate>
                            <asp:Label ID="lblRlineNo" runat="server" Text='<%# Bind("rlineno") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="slineno"><ItemTemplate>
                            <asp:Label ID="lblSLineno" runat="server" Text='<%# Bind("slineno") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Account code"><ItemTemplate>
                            <asp:Label ID="lblAcctCode" runat="server" Text='<%# Bind("acc_code") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Account Name"><EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("acc_name") %>'></asp:TextBox>
                        
</EditItemTemplate>
<ItemTemplate>
                            <asp:Label ID="lblAcctName" runat="server" Text='<%# Bind("acc_name") %>'></asp:Label>
                        
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle" Wrap="False"></RowStyle>

<PagerStyle CssClass="grdpagerstyle" Wrap="False"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" Wrap="False"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> </DIV><asp:Label id="lblMessageCP" runat="server" Font-Size="9pt" 
        Font-Names="Verdana" Font-Bold="True" Visible="False" CssClass="lblmsg">Records Not Found.</asp:Label></TD></TR><TR><TD><INPUT style="VISIBILITY: hidden; WIDTH: 8px" id="txtgridrowscp" type=text runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtbasecurr" type=text maxLength=20 runat="server" /></TD><TD></TD><TD align=center dir="ltr"><TABLE><TBODY><TR><TD>Total  Debit &nbsp;</TD><TD style="WIDTH: 100px"><INPUT style="TEXT-ALIGN: right" id="txtTotalKWDDebitCP" class="field_input" readOnly type=text runat="server" /></TD><TD>Total Credit</TD><TD><INPUT style="TEXT-ALIGN: right" id="txtTotalKWDCreditCP" class="field_input" readOnly type=text runat="server" /></TD><TD style="WIDTH: 70px" class="td_cell" align=center>P/L</TD><TD style="WIDTH: 131px" align=left><INPUT style="TEXT-ALIGN: right" id="txtPL" class="field_input" readOnly type=text runat="server" /></TD></TR></TBODY></TABLE></TD></TR><TR><TD style="height: 22px"></TD><TD style="height: 22px"></TD><TD align=center style="height: 22px"><asp:CheckBox id="chkPost" tabIndex=9 runat="server" Text="Post/UnPost" Font-Size="10px" Font-Names="Verdana" Width="103px" CssClass="field_caption" ForeColor="Black" Font-Bold="True" BackColor="#FFC0C0"></asp:CheckBox> 
    <asp:Button id="BtnPrint" runat="server" Text="Print" CssClass="btn" 
        OnClick="BtnPrint_Click" Visible="False"></asp:Button>
&nbsp;    <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="10" 
        Text="Save" />&nbsp;<asp:Button id="btnCancel" tabIndex=11 runat="server" 
        Text="Exit" CssClass="btn"></asp:Button>&nbsp;
        <asp:Button id="btnReturn" tabIndex=12 runat="server" 
        Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnHelp" tabIndex=13 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="btn"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
    <asp:HiddenField ID="hdnSS" runat="server" Value="0" />

    <asp:HiddenField ID="hdnPI" runat="server" Value="0" />

</asp:Content>
