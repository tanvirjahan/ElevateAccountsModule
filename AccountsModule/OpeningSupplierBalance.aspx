<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OpeningSupplierBalance.aspx.vb" Inherits="OpeningSupplierBalance"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript">
//----------------------------------------
var txtfill = null;  
var txtrate = null;
var nodecround =null;
var txtduedts ;
var grdduedts ;
var txtcrday=null;
//var objGridView = document.getElementById('<%=grdRecord.ClientID%>');
//var j;
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



    function custautocompleteselected(source, eventArgs)
{
    if (eventArgs != null) {
        document.getElementById('<%=txtsuppcode.ClientID%>').value = eventArgs.get_value();



        var type=document.getElementById('<%=txttype.ClientID%>').value
          
   
   
    var sqlstr;
    var sqlstr1;
        
    var stname;
    var strfiled;
    
    if (type=='S')
    {
    sqlstr="select currcode,currcode from partymast where Partycode='" + eventArgs.get_value()+ "'" ;
    sqlstr1="select isnull(crdays,0) crdays from partymast where Partycode='" + eventArgs.get_value() + "'" ;
    stname="partymast";
    strfiled="Partycode";
    }
    if (type=='C')
    {
    sqlstr="select currcode,currcode from agentmast where agentcode='" + eventArgs.get_value() + "'" ;
    sqlstr1=="select  isnull(crdays,0) crdays from agentmast where agentcode='" + eventArgs.get_value() + "'" ;
    stname="agentmast";
    strfiled="agentcode";
    }
    if (type=='A')
    {
    sqlstr="select currcode,currcode from supplier_agents where supagentcode='" + eventArgs.get_value() + "'" ;
    sqlstr1=="select  isnull(crdays,0) crdays from supplier_agents where supagentcode='" +  eventArgs.get_value()+ "'" ;
    stname="supplier_agents";
    strfiled="supagentcode";
    }
    
    //changeSupplier(stname,strfiled)
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value
    txtfill = document.getElementById('<%=TxtCurrCode.ClientID%>');
    txtcrday = document.getElementById('<%=txtCrDay.ClientID%>');
  
    ColServices.clsServices.GetQueryReturnStringListnew(constr,sqlstr,FillCurr,ErrorHandler,TimeOutHandler);
//    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr1,FiilCrdays,ErrorHandler,TimeOutHandler);
//    txtcrday = document.getElementById('<%=txtCrDay.ClientID%>');

}

else {


    document.getElementById('<%=txtsuppcode.ClientID%>').value = '';
}

}


 
function FillCurr(result)
    {
    txtfill.value=result[0].ListText;
    
      
    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
     txtrate=document.getElementById('<%=txtConvRate.ClientID%>');
    if ( trim(txtfill.value)==trim(txtbase.value))
       {
        txtrate.readOnly=true; 
        txtrate.disabled=true;
       }
       else
       {
        txtrate.readOnly=false;
        txtrate.disabled=false;
       }
  
    
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
    
    var cursqlstr="select convrate,convrate from currrates where currcode='"+ txtfill.value +"' and Tocurr='"+ txtbase.value +"'"
    ColServices.clsServices.GetQueryReturnStringListnew(constr,cursqlstr,FillCvntRate,ErrorHandler,TimeOutHandler);
     
   
   } 
function FillCvntRate(result)
    {
        txtrate.value=result[0].ListText;
   } 
function FillDueDate(txtdt,txtduedt,stname,strfiled)
    {
    
    var txtdts =document.getElementById(txtdt);
    txtduedts=document.getElementById(txtduedt);

    ddlsup = document.getElementById('<%=txtsuppcode.ClientID%>');
    var codeid=ddlsup.value;
    
    var dtvl =null;
    if (txtdts.value !='')
    {   
    var datearray=txtdts.value.split("/");
    var dateval = datearray[2]+"/"+ datearray[1]+"/"+ datearray[0];
    dtvl=dateval;
    } 
    var crdsqlstr="select convert(varchar(10), Dateadd(day,isnull(crdays,0),'"+dtvl +"'),103) Duedate  from "+stname+" where " +strfiled+  " = '"+ codeid +"'";
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
    
    ColServices.clsServices.GetQueryReturnStringValuenew(constr,crdsqlstr,FiilDueDt,ErrorHandler,TimeOutHandler);
    
   } 
function FiilDueDt(result)
    {
    txtduedts.value=result;
   } 
   function GrdFillDueDate(strdate)
    {
    
    
    //tcrdays=document.getElementById('<%=txtCrDay.ClientID%>');
    var crdays= crdays=parseInt(txtcrday.value);
    //var crdays=parseInt(tcrdays.value);
    
    var dtvl =null;
    if (trim(strdate) !='')
    {   
    var datearray=strdate.split("/");
    var dateval = datearray[2]+"/"+ datearray[1]+"/"+ datearray[0];
    dtvl=dateval;
    var grdsqlstr="select convert(varchar(10), Dateadd(day,isnull("+crdays +",0),'"+dtvl +"'),103) Duedate ";
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
    
    ColServices.clsServices.GetQueryReturnStringValuenew(constr,grdsqlstr,GrdFiilDueDt,ErrorHandler,TimeOutHandler);
    } 

   } 
   function FiilCrdays(result)
    {
      
      //txtcrday = document.getElementById('<%=txtCrDay.ClientID%>');
      //txtcrday = document.getElementById('<%=txtCrDay.ClientID%>');
      txtcrday.value=result;
   //   changeSupplier()
      
   } 

function GrdFiilDueDt(valtest)
    {
    //   alert(valtest);
  //    alert(result);
     Grdduedts=valtest;  
     //objGridView.rows[j].cells[2].children[0].value=result;  
   // alert(Grdduedts);
   return valtest;
   } 
   function GFillDueDate(strdate,stname,strfiled)
    {
    
//    var txtdts =document.getElementById(txtdt);
//    txtduedts=document.getElementById(txtduedt);

        ddlsup = document.getElementById('<%=txtsuppcode.ClientID%>');
    var codeid=ddlsup.value;
    
    var dtvl =null;
    if (trim(strdate) !='')
    {   
    var datearray=strdate.split("/");
    var dateval = datearray[2]+"/"+ datearray[1]+"/"+ datearray[0];
    dtvl=dateval;
    } 
    var dsqlstr="select convert(varchar(10), Dateadd(day,isnull(crdays,0),'"+dtvl +"'),103) Duedate  from "+stname+" where " +strfiled+  " = '"+ codeid +"'";
//    alert(dsqlstr);
    //var result1=null;
    //result1=
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
    
    var result1= ColServices.clsServices.GetQueryReturnStringValuenew(constr,dsqlstr,GrdFiilDueDt,ErrorHandler,TimeOutHandler);
   alert(result1);
    //return result1;
    
   }
   
  function changeSupplier(stname,strfiled)
{
     //  var grduedate=objGridView.rows[j].cells[2].children[0].value;
    var objGridView = document.getElementById('<%=grdRecord.ClientID%>');
    var txtrowcnt =document.getElementById('<%=txtgridrows.ClientID%>');
    intRows=txtrowcnt.value;
    
     for(j=1;j<=intRows;j++)
    {
       var grdate=objGridView.rows[j].cells[0].children[0].value;
      if (trim(grdate) !='')
         {
             ddlsup = document.getElementById('<%=txtsuppcode.ClientID%>');
         var codeid=ddlsup.value;
    
        var dtvl =null;
        if (trim(grdate) !='')
         {   
            var datearray=grdate.split("/");
            var dateval = datearray[2]+"/"+ datearray[1]+"/"+ datearray[0];
            dtvl=dateval;
          } 
         var dsqlstr="select convert(varchar(10), Dateadd(day,isnull(crdays,0),'"+dtvl +"'),103) Duedate  from "+stname+" where " +strfiled+  " = '"+ codeid +"'";
         
         var result1;
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   
         
         ColServices.clsServices.GetQueryReturnStringValuenew(constr,dsqlstr,GFiilvalue,ErrorHandler,TimeOutHandler);
         result1=grdduedts;
     //if(trim(Grdduedts) !='')
        //{
        // GFillDueDate(grdate,stname,strfiled);
         objGridView.rows[j].cells[2].children[0].value= result1;
        //}
               
         }
       
       
   }
   
   function GFiilvalue(valtest)
    {
     grdduedts=valtest;  
   } 
}
 
//-----------------------------------------

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
//-----------------------------------------
//-----------------------------------------


//---------------------------------------------------
 function  convertInRate(txtCD,txtDBAmt,txtCnvtRate,txtCnvtAmt)
 {
    
    txtDBAmt = document.getElementById(txtDBAmt);
    txtCD = document.getElementById(txtCD);
    if(txtDBAmt.value>0)
    {
  
     txtCD.readOnly=true; 
      }else
      {
     txtCD.readOnly=false;
    }
    
    txtCnvtRate = document.getElementById(txtCnvtRate);
    txtCnvtAmt = document.getElementById(txtCnvtAmt);  
 
    if (txtDBAmt.value==''){txtDBAmt.value=0;}
    var totamt= DecRound(txtDBAmt.value) *  parseFloat(txtCnvtRate.value) ;
    txtCnvtAmt.value =DecRound(totamt); 
    //Call Grd Total 
    grdTotal()
 }
//---------------------------------------------------

function grdTotal()
{
    var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
    nodecround=Math.pow(10,parseInt(txtdec.value));
 
 
    var objGridView = document.getElementById('<%=grdRecord.ClientID%>');
    var totCr = 0 ;
    var totDr = 0 ;
    var totbCr = 0 ;
    var totbDr = 0 ;
    //var intRows=parseInt('<%=grdRecord.Rows.Count %>')
       var txtrowcnt =document.getElementById('<%=txtgridrows.ClientID%>');
      txtrate=document.getElementById('<%=txtConvRate.ClientID%>');
      intRows=txtrowcnt.value;
      
     for(j=1;j<=intRows;j++)
    {
       
       var valDr=objGridView.rows[j].cells[4].children[0].value;
      
       var valCr=objGridView.rows[j].cells[5].children[0].value;
       
       var valbDr=objGridView.rows[j].cells[6].children[0].value;
       var valbCr=objGridView.rows[j].cells[7].children[0].value;

    
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
    var txttotCr  = document.getElementById('<%=txtTotCredit.ClientID%>');
    var txttotDr = document.getElementById('<%=txtTotDebit.ClientID%>');
   
    var txttotbCr  = document.getElementById('<%=txtTotBaseCredit.ClientID%>');
    var txttotbDr = document.getElementById('<%=txtTotBaseDebit.ClientID%>');




    txttotDr.value=DecRound(totDr);
    txttotCr.value=DecRound(totCr);
        
    txttotbDr.value=DecRound(totbDr);
    txttotbCr.value=DecRound(totbCr);
    
    //Net Bal
    var txNetBal  = document.getElementById('<%=txtNetBal.ClientID%>');
    var txNetBalBase = document.getElementById('<%=txtNetBalBase.ClientID%>');
    
    var totbal= DecRound(txttotCr.value) - DecRound(txttotDr.value)
    txNetBal.value =DecRound(totbal); 
    var totBbal=DecRound(txttotbCr.value) - DecRound(txttotbDr.value)
    txNetBalBase.value  =DecRound(totBbal);
    
    
  }
//---------------------------------------------------
function changeRate()
{
 //Call Grd Total 
    
    var objGridView = document.getElementById('<%=grdRecord.ClientID%>');
    var totCr = 0 ;
    var totDr = 0 ;
    var totbCr = 0 ;
    var totbDr = 0 ;
    //var intRows=parseInt('<%=grdRecord.Rows.Count %>')
    var txtrowcnt =document.getElementById('<%=txtgridrows.ClientID%>');
    
    var  txtCnvtR = document.getElementById('<%=txtConvRate.ClientID%>');
    
    intRows=txtrowcnt.value;
     for(j=1;j<=intRows;j++)
    {
       
       var valDr=objGridView.rows[j].cells[4].children[0].value;
       var valCr=objGridView.rows[j].cells[5].children[0].value;
       
       var valbDr=objGridView.rows[j].cells[6].children[0] ;
       var valbCr=objGridView.rows[j].cells[7].children[0] ;
      
       if (valCr==''){valCr=0;}
       if (valDr==''){valDr=0;}
       
       if (isNaN(valCr)==true){valCr=0;}
       if (isNaN(valDr)==true){valDr=0;}
  
       
      if(valCr!=0)
      {
             var vbCr = DecRound(valCr) *  parseFloat(txtCnvtR.value);
             valbCr.value =DecRound(vbCr);
       }
       if(valDr!=0)
      {
       var vbDr = DecRound(valDr) *  parseFloat(txtCnvtR.value);
       valbDr.value =DecRound(vbDr);
       }
   }
   //Call Grd Total 
    grdTotal()
}





//---------------------------------------------------

</script>
        <asp:UpdatePanel id="UpdatePanel1" runat="server" ScrollBars="Auto">
                    <contenttemplate>
<TABLE class="td_cell" style="WIDTH: 80%" ><TBODY><TR><TD style="WIDTH: 100%" class="field_heading" align=center colSpan=3>
    <asp:Label id="lblHeading" runat="server" Text="Opening Supplier Balance" 
        CssClass="field_heading" Width="838px" Height="17px"></asp:Label></TD></TR><TR><TD style="WIDTH: 966px" colSpan=3><TABLE><TBODY><TR>
        <TD style="height: 28px">
            <asp:Label id="Label3" runat="server" Text=" Doc No." 
                CssClass="field_caption" Width="66px"></asp:Label></TD>
        <TD style="height: 28px"><INPUT style="WIDTH: 194px" id="txtDocNo" class="field_input" tabIndex=1 readOnly type=text maxLength=50 runat="server" /></TD>
        <TD style="height: 28px; text-align: right;"><asp:Label id="Label4" runat="server" 
                Text=" Date" CssClass="field_caption" Width="41px"></asp:Label></TD>
        <TD style="height: 28px">
       
        
        
         <asp:TextBox ID="txtDocDate" runat="server" CssClass="fiel_input" TabIndex="2" 
                                                ValidationGroup="MKE" Width="80px"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" 
                                                DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" 
                                                MaskType="Date" MessageValidatorTip="true" TargetControlID="txtDocDate" />
                                            <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" 
                                                PopupButtonID="ImgBtnFrmDt" TargetControlID="txtDocDate" />
                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="2" />
                                            <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" 
                                                ControlExtender="MskFromDate" ControlToValidate="txtDocDate" 
                                                CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
                                                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
                                                InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" 
                                                InvalidValueMessage="Invalid Date" 
                                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" 
                                                Width="23px">
                                            </cc1:MaskedEditValidator>
        
        </TD></TR><TR><TD>
            <asp:Label id="lblcode" runat="server" Text=" " CssClass="field_caption" 
                Width="75px"></asp:Label></TD>
            <TD colspan="3">
 
 
   
                             <asp:TextBox ID="txtsuppname" runat="server" TabIndex="3" AutoPostBack="false" CssClass="field_input"
                                                                    MaxLength="500"  Width="432px" 
                                 Height="16px" ></asp:TextBox>
                                                                <asp:TextBox ID="txtsuppcode" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" Visible="False" />
                                                                <asp:AutoCompleteExtender ID="TxtCustName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    ServiceMethod="" TargetControlID="txtsuppname" OnClientItemSelected="custautocompleteselected">
                                                                   
                                                                </asp:AutoCompleteExtender>
                            </TD></TR><TR><TD><asp:Label id="Label1" runat="server" 
                Text=" Currency" CssClass="field_caption" Width="67px"></asp:Label></TD><TD><INPUT style="WIDTH: 194px" id="TxtCurrCode" class="field_input" tabIndex=4 readOnly type=text maxLength=20 runat="server" /></TD><TD><asp:Label id="Label2" runat="server" Text=" Conv. Rate" CssClass="field_caption" Width="122px"></asp:Label></TD><TD>
            <INPUT style="WIDTH: 106px" id="txtConvRate" class="field_input" tabIndex=6 type=text maxLength=15 runat="server" /> <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtbasecurr" type=text maxLength=20 runat="server" /> <asp:Label id="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana" CssClass="field_caption" Width="73px" BackColor="#E0E0E0" Font-Bold="True" ForeColor="Green"></asp:Label></TD></TR></TBODY></TABLE><BR /></TD></TR><TR><TD style="WIDTH: 966px" colSpan=3>
   
        <asp:GridView id="grdRecord" tabIndex=7 runat="server" Font-Size="10px" 
            CssClass="td_cell" Width="100%" BackColor="White" AutoGenerateColumns="False" 
            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            GridLines="Vertical">
<FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="LineNo"><EditItemTemplate>
<asp:TextBox id="TextBox8" runat="server" Text='<%# Bind("tran_lineno") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblLineNo" runat="server" Text='<%# Bind("tran_lineno") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Date">
<FooterTemplate>
<ews:DatePicker id="dtDate1" runat="server" Width="174px" CalendarPosition="DisplayRight" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker> 
</FooterTemplate>
<ItemTemplate>
<asp:TextBox id="txtDate" runat="server" CssClass="fiel_input" Width="80px" ValidationGroup="MKE"></asp:TextBox><asp:ImageButton id="ImgBtnDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;<cc1:MaskedEditValidator id="MaskEdValidDate" runat="server" CssClass="field_error" Width="1px" Height="16px" ValidationGroup="MSK" Display="Dynamic" ControlExtender="MskEdExtendDate" ControlToValidate="txtDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator><BR /><cc1:CalendarExtender id="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnDt" TargetControlID="txtDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskEdExtendDate" runat="server" TargetControlID="txtDate" AcceptNegative="Left" DisplayMoney="Left" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> 
</ItemTemplate>
    <HeaderStyle Width="50px" />
    <ItemStyle Width="50px" Wrap="False" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Bill Type"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 65px" id="txtBillType" class="field_input" tabIndex=0 type=text maxLength=10 runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Due Date">
<FooterTemplate>
<ews:DatePicker id="dtDueDate1" runat="server" Width="174px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker> 
</FooterTemplate>
<ItemTemplate>
<asp:TextBox id="txtDueDate" runat="server" width="80px" CssClass="fiel_input" ></asp:TextBox>
<asp:ImageButton id="ImgBtnDueDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> 
<cc1:MaskedEditValidator id="MaskEdValidDueDate" runat="server" ValidationGroup="MSk" ControlExtender="MskEdExtendDueDate" ControlToValidate="txtDueDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator> <cc1:CalendarExtender id="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnDueDate" TargetControlID="txtDueDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskEdExtendDueDate" runat="server" TargetControlID="txtDueDate" AcceptNegative="Left" DisplayMoney="Left" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> 
</ItemTemplate>
  <HeaderStyle Width="50px" />
    <ItemStyle Width="50px" Wrap="False" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Document  No"><EditItemTemplate>
<asp:TextBox id="TextBox9" runat="server" Width="94px"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 87px" id="txtDocNo" class="field_input" tabIndex=0 type=text maxLength=20 runat="server" />
<INPUT id="txtOtherRef"  style="display:none" class="field_input" tabIndex=0 type=text maxLength=200 runat="server" />
</ItemTemplate>
</asp:TemplateField>


   
<asp:TemplateField HeaderText="Debit"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("Debit") %>' Width="79px"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 80px; TEXT-ALIGN: right" id="txtDebit" class="field_input" tabIndex=0 type=text maxLength=10 runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Credit"><EditItemTemplate>
<asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("Credit") %>' Width="86px"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 80px; TEXT-ALIGN: right" id="txtCredit" class="field_input" tabIndex=0 type=text maxLength=10 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Debit"><EditItemTemplate>
<asp:TextBox id="TextBox6" runat="server" Text='<%# Bind("BaseDebit") %>' Width="57px"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtBaseDebit" class="field_input" tabIndex=0 readOnly type=text maxLength=20 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Base Credit"><EditItemTemplate>
<asp:TextBox id="TextBox7" runat="server" Text='<%# Bind("basecredit") %>' Width="109px"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 96px; TEXT-ALIGN: right" id="txtbaseCredit" class="field_input" tabIndex=0 readOnly type=text maxLength=15 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>

    <asp:TemplateField HeaderText="FIELD1"><EditItemTemplate>
    <asp:TextBox id="txtfield1" runat="server" Text='<%# Bind("field1") %>' Width="50px"></asp:TextBox> 
    </EditItemTemplate>
    <ItemTemplate>
    <INPUT style="WIDTH: 50px; TEXT-ALIGN: right" id="txtfield1" class="field_input" tabIndex=0  type=text maxLength=15 runat="server" /> 
    </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="FIELD2"><EditItemTemplate>
    <asp:TextBox id="txtfield2" runat="server" Text='<%# Bind("field2") %>' Width="50px"></asp:TextBox> 
    </EditItemTemplate>
    <ItemTemplate>
    <INPUT style="WIDTH: 50px; TEXT-ALIGN: right" id="txtfield2" class="field_input" tabIndex=0  type=text maxLength=15 runat="server" /> 
    </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="FIELD3"><EditItemTemplate>
    <asp:TextBox id="txtfield3" runat="server" Text='<%# Bind("field3") %>' Width="50px"></asp:TextBox> 
    </EditItemTemplate>
    <ItemTemplate>
    <INPUT style="WIDTH: 50px; TEXT-ALIGN: right" id="txtfield3" class="field_input" tabIndex=0  type=text maxLength=15 runat="server" /> 
    </ItemTemplate>
    </asp:TemplateField>

<asp:TemplateField HeaderText="Delete"><ItemTemplate>
<asp:CheckBox id="chkDel" runat="server" Width="17px"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="#084573"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView>
</TD></TR><TR><TD style="WIDTH: 966px" colSpan=3>&nbsp;<asp:Button 
            id="btnAdd" tabIndex=8 runat="server" Text="Add Row" CssClass="field_button" 
            Font-Bold="True"></asp:Button>&nbsp;<asp:Button id="btnDelete" tabIndex=9 
            runat="server" Text="Delete Row" CssClass="field_button" Font-Bold="True"></asp:Button></TD></TR><TR><TD style="WIDTH: 966px" align=right colSpan=3><TABLE style="WIDTH: 574px"><TBODY><TR><TD style="WIDTH: 376px">Total</TD><TD style="WIDTH: 100px"><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtTotDebit" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 100px"><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtTotCredit" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 100px"><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtTotBaseDebit" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 100px"><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtTotBaseCredit" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 376px; HEIGHT: 16px">Net &nbsp; Balance</TD><TD style="WIDTH: 100px; HEIGHT: 16px"><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtNetBal" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD><TD></TD><TD><INPUT style="WIDTH: 113px; TEXT-ALIGN: right" id="txtNetBalBase" class="field_input" readOnly type=text maxLength=100 runat="server" /></TD><TD></TD></TR><TR><TD>
        &nbsp;</TD><TD>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
    <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txttype" type=text maxLength=15 runat="server" /></TD>
        <caption>
            <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtCrDay" type=text maxLength=15 runat="server" />
            <INPUT style="VISIBILITY: hidden; WIDTH: 5px" 
                            id="txtgridrows" type=text maxLength=15 runat="server" />
            <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtdecimal" type=text maxLength=15 runat="server" />
        </caption>
        </TD>
        <tr>
            <td>
                <td colspan="2">
                    &nbsp;</td>
                <td colspan="2" style="text-align: right">
                    <asp:CheckBox ID="chkPost" runat="server" BackColor="#FFC0C0" 
                        CssClass="field_caption" Font-Bold="True" Font-Names="Verdana" Font-Size="10px" 
                        ForeColor="Black" tabIndex="10" Text="Post/UnPost" Width="103px" />
                    <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                        Font-Bold="True" tabIndex="11" Text="Save" />
                </td>
                <td>
                    <asp:Button ID="btnExit" runat="server" CssClass="field_button" 
                        Font-Bold="True" tabIndex="12" Text="Exit" />
                </td>
                <td>
                    <asp:Button ID="btnhelp" runat="server" CssClass="field_button" 
                        onclick="btnhelp_Click" tabIndex="13" Text="Help" />
                </td>
            </td>
        </tr>
    </TR></TBODY></TABLE></TD></TR></TBODY></TABLE>&nbsp; 
</contenttemplate>
                </asp:UpdatePanel> 
                <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>
    <br />
</asp:Content>

