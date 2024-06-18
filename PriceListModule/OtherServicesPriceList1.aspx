<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OtherServicesPriceList1.aspx.vb" Inherits="OtherServicesPriceList1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
     <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script type="text/javascript">
<!--
WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
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
	
function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}

function CallWebMethod(methodType)
    {
       switch(methodType)
        {
            case "SeasCode":
                var select=document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlSubSeasName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            break;
            case "SeasName":
                var select=document.getElementById("<%=ddlSubSeasName.ClientID%>");
                var selectname=document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            break;
            
            case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
              break;
           
            case "SellTypeCode":
                var select=document.getElementById("<%=ddlSellTypeCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlSellTypeName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                var sqlstr;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                sqlstr="select currcode  from othsellmast where active=1 and othsellcode='"+ select.options[select.selectedIndex].text +"'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrCode,ErrorHandler,TimeOutHandler);
                var sqlstr="select currmast.currname from currmast inner join  othsellmast on currmast.currcode=othsellmast.currcode and othsellmast. active=1 and othsellmast.othsellcode='"+ select.options[select.selectedIndex].text +"'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrName,ErrorHandler,TimeOutHandler);
                break;
            
            case "SellTypeName":
                var select=document.getElementById("<%=ddlSellTypeName.ClientID%>");
                var selectname=document.getElementById("<%=ddlSellTypeCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                var sqlstr="select currcode  from othsellmast where active=1 and othsellcode='"+ select.options[select.selectedIndex].value +"'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrCode,ErrorHandler,TimeOutHandler);
                var sqlstr="select currmast.currname from currmast inner join  othsellmast on currmast.currcode=othsellmast.currcode and othsellmast. active=1 and othsellmast.othsellcode='"+ select.options[select.selectedIndex].value +"'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurrName,ErrorHandler,TimeOutHandler);

                break;
            
            case "GroupCode":
                var select=document.getElementById("<%=ddlGroupCode.ClientID%>");
                
                var selectname=document.getElementById("<%=ddlGroupName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "GroupName":
                var select=document.getElementById("<%=ddlGroupName.ClientID%>");
                var selectname=document.getElementById("<%=ddlGroupCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            
        }
    }
    
   function FillCurrCode(result)
   {
   var txt  =  document.getElementById("<%=txtCurrCode.ClientId%>");
   txt.value = result;
   }
 function FillCurrName(result)
   {
   var txt = document.getElementById("<%=txtCurrName.ClientId%>");
   txt.value=result;
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
    
function FormValidation(state)
{
 
  if (document.getElementById("<%=ddlMarketCode.ClientID%>").value=="[Select]")
	    {
            document.getElementById("<%=ddlMarketCode.ClientID%>").focus(); 
            alert("Select market Code.");
            return false;
       }
    else if (document.getElementById("<%=ddlSellTypeCode.ClientID%>").value=="[Select]")
         {
              alert("Select Sell Type Code.");
              document.getElementById("<%=ddlSellTypeCode.ClientID%>").focus();
              return false;
          }
     else if (document.getElementById("<%=ddlGroupCode.ClientID%>").value == "[Select]")
	     {
	           document.getElementById("<%=ddlGroupCode.ClientID%>").focus();
                alert("Select Other Service Group Code.");
                return false;
           
          }

           else if (document.getElementById("<%=ddlSubSeasCode.ClientID%>").value=="[Select]")
	     {
           document.getElementById("<%=ddlSubSeasCode.ClientID%>").focus();
           alert("Select Sub Season Code.");
            return false;
          }
             
        else
           {
           //alert(state);
           if (state=='New'){if(confirm('Are you sure you want to generate other services price list?')==false)return false;}
           if (state=='Edit'){if(confirm('Are you sure you want to generate other services price list?')==false)return false;}
           if (state=='Delete'){if(confirm('Are you sure you want to generate other services price list?')==false)return false;}
           
           }       
}

</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 732px; BORDER-BOTTOM: gray 2px solid; HEIGHT: 351px"><TBODY><TR><TD style="HEIGHT: 4px" class="field_heading" align=center colSpan=4>
<asp:Label id="lblHeading" runat="server" Text="Other Services Price List" Width="358px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 457px; HEIGHT: 10px" class="td_cell" align=left>PL&nbsp;Code</TD>
<TD style="WIDTH: 183px; HEIGHT: 10px"><INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD><TD style="WIDTH: 88px; HEIGHT: 10px" class="td_cell" align=left></TD>
<TD style="HEIGHT: 10px"></TD></TR><TR><TD style="WIDTH: 457px; HEIGHT: 13px" class="td_cell" align=left>Market&nbsp;Code <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 183px; HEIGHT: 13px">
<SELECT style="WIDTH: 200px" id="ddlMarketCode" class="field_input" tabIndex=2 onchange="CallWebMethod('marketcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD style="WIDTH: 88px; HEIGHT: 13px" class="td_cell" align=left>Market&nbsp;Name</TD><TD style="HEIGHT: 13px"><SELECT style="WIDTH: 300px" id="ddlMarketName" class="field_input" tabIndex=3 onchange="CallWebMethod('marketname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 457px; HEIGHT: 13px" class="td_cell" align=left>Sell&nbsp;Type&nbsp;Code <SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD style="WIDTH: 183px; HEIGHT: 13px"><SELECT style="WIDTH: 200px" id="ddlSellTypeCode" class="field_input" tabIndex=4 onchange="CallWebMethod('SellTypeCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 88px; HEIGHT: 13px" class="td_cell" align=left>Sell&nbsp;Type&nbsp;Name</TD><TD style="HEIGHT: 13px"><SELECT style="WIDTH: 300px" id="ddlSellTypeName" class="field_input" tabIndex=5 onchange="CallWebMethod('SellTypeName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR>
<TD style="WIDTH: 457px; HEIGHT: 11px" class="td_cell" align=left>Group&nbsp;Code <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 183px; HEIGHT: 11px"><SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=6 onchange="CallWebMethod('GroupCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 88px; HEIGHT: 11px" class="td_cell" align=left>Group&nbsp;Name</TD><TD style="HEIGHT: 11px"><SELECT style="WIDTH: 300px" id="ddlGroupName" class="field_input" tabIndex=7 onchange="CallWebMethod('GroupName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
<TR><TD style="WIDTH: 457px; HEIGHT: 2px" class="td_cell" align=left>Currency&nbsp;Code <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 183px; HEIGHT: 2px"><asp:TextBox id="txtCurrCode" tabIndex=8 runat="server" Width="194px" CssClass="field_input"></asp:TextBox></TD><TD style="WIDTH: 88px; HEIGHT: 2px" class="td_cell" align=left>Currency&nbsp;Name</TD><TD style="HEIGHT: 2px"><asp:TextBox id="txtCurrName" tabIndex=9 runat="server" Width="294px" CssClass="field_input"></asp:TextBox></TD></TR><TR>
<TD style="WIDTH: 500px" class="td_cell" align=left>Sub&nbsp;Season&nbsp;Code<SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD style="WIDTH: 183px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="field_input" tabIndex=10 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 88px" class="td_cell" align=left>Sub&nbsp;Season&nbsp;Name</TD><TD><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="field_input" tabIndex=11 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 457px" class="td_cell" align=left>From Date&nbsp; <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 183px"><ews:DatePicker id="dpFromdate" tabIndex=12 runat="server" CssClass="field_input" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"></ews:DatePicker></TD><TD style="WIDTH: 88px" class="td_cell" align=left>To Date<SPAN style="COLOR: #ff0000">*</SPAN></TD><TD><ews:DatePicker id="dpTodate" tabIndex=13 runat="server" CssClass="field_input" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"></ews:DatePicker></TD></TR>

<TR><TD style="WIDTH: 457px; HEIGHT: 40px" class="td_cell" align=left>Remark</TD><TD style="WIDTH: 183px; HEIGHT: 40px" align=left colSpan=3>
<TEXTAREA style="WIDTH: 607px; HEIGHT: 59px" id="txtRemark" class="field_input" tabIndex=14 runat="server"></TEXTAREA></TD></TR>

<TR><TD style="WIDTH: 457px; HEIGHT: 9px" class="td_cell" align=left>Active</TD><TD style="WIDTH: 183px; HEIGHT: 9px" align=left colSpan=3><INPUT id="ChkActive" tabIndex=15 type=checkbox CHECKED runat="server" />
    <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
        Visible="False" />
    <asp:CheckBox ID="chkshowweb" runat="server" Font-Bold="False" Text="Stop Web" Visible="False"
        Width="140px" /></TD></TR><TR><TD style="HEIGHT: 4px" align=right colSpan=4>
            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                height: 9px" type="text" />
            <asp:Button id="btnGenerate" tabIndex=16 onclick="btnGenerate_Click" runat="server" Text="....Generate" CssClass="field_button"></asp:Button>
            &nbsp; <asp:Button id="btnCancel" tabIndex=17 onclick="btnCancel_Click" 
                runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>&nbsp;
            <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server" 
                Text="Help" CssClass="field_button"></asp:Button>&nbsp; </TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
        <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

