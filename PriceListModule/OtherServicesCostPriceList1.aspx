<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OtherServicesCostPriceList1.aspx.vb" Inherits="OtherServicesCostPriceList1"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type ="text/javascript" >
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
        
        case "sptypecode":
                var select=document.getElementById("<%=ddlSPType.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSPTypeName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSupplierCodeListnew(constr,codeid,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameListnew(constr,codeid,FillSupplierNames,ErrorHandler,TimeOutHandler);
                
               break;
         case "sptypename":
                var select=document.getElementById("<%=ddlSPTypeName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSPType.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSupplierCodeListnew(constr,codeid,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameListnew(constr,codeid,FillSupplierNames,ErrorHandler,TimeOutHandler);
                break;
                
                
         case "PartyCode":
                var select=document.getElementById("<%=ddlSupplierCode.ClientID%>");
                codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSupplierName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
            
                ColServices.clsServices.GetSupOthGroupCodeListnew(constr,codeid,FillGroupCodesGroup,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupOthGroupNameListnew(constr,codeid,FillGroupNamesGroup,ErrorHandler,TimeOutHandler);
               
                ColServices.clsServices.GetPartyCurrNamenew(constr,codeid,FillSupplierCurrName,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetPartyCurrCodenew(constr,codeid,FillSupplierCurrCode,ErrorHandler,TimeOutHandler);
               
                break;
                
         case  "PartyName":
               var select=document.getElementById("<%=ddlSupplierName.ClientID%>");
               codeid=select.options[select.selectedIndex].text;
               var selectname=document.getElementById("<%=ddlSupplierCode.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
                codeid=select.options[select.selectedIndex].value;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSupOthGroupCodeListnew(constr,codeid,FillGroupCodesGroup,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupOthGroupNameListnew(constr,codeid,FillGroupNamesGroup,ErrorHandler,TimeOutHandler);
               
                ColServices.clsServices.GetPartyCurrNamenew(constr,codeid,FillSupplierCurrName,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetPartyCurrCodenew(constr,codeid,FillSupplierCurrCode,ErrorHandler,TimeOutHandler);
                 
         case  "SupplierAgentCode":
               var select=document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
               var selectname=document.getElementById("<%=ddlSupplierAgentName.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
         case  "SupplierAgentName":
               var select=document.getElementById("<%=ddlSupplierAgentName.ClientID%>");
               var selectname=document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
            
         case  "marketcode":
               var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
               var codeid=select.options[select.selectedIndex].text;
               var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
         case  "marketname":
               var select=document.getElementById("<%=ddlMarketName.ClientID%>");
               var codeid=select.options[select.selectedIndex].text;
               var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
            
         case  "GroupCode":
               var select=document.getElementById("<%=ddlGroupCode.ClientID%>");                
               var selectname=document.getElementById("<%=ddlGroupName.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
         case  "GroupName":
               var select=document.getElementById("<%=ddlGroupName.ClientID%>");
               var selectname=document.getElementById("<%=ddlGroupCode.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
            
         case  "SeasCode":
               var select=document.getElementById("<%=ddlSubSeasCode.ClientID%>");
               var selectname=document.getElementById("<%=ddlSubSeasName.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
         case  "SeasName":
               var select=document.getElementById("<%=ddlSubSeasName.ClientID%>");
               var selectname=document.getElementById("<%=ddlSubSeasCode.ClientID%>");
               selectname.value=select.options[select.selectedIndex].text;
               break;
         
        }
    }
    
    
     function FillSupplierCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlSupplierCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillSupplierNames(result)
    {
        var ddl = document.getElementById("<%=ddlSupplierName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
    
    
    function FillGroupCodesGroup(result)
    {
      	var ddl = document.getElementById("<%=ddlGroupCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillGroupNamesGroup(result)
    {
        var ddl = document.getElementById("<%=ddlGroupName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

   function FillSupplierCurrCode(result)
    {
      	var txt = document.getElementById("<%=txtCurrCode.ClientID%>");
        txt.value=result;
    }

   function FillSupplierCurrName(result)
    {
      	var txt = document.getElementById("<%=txtCurrName.ClientID%>");
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
if (document.getElementById("<%=ddlSPType.ClientID%>").value=="[Select]")
	    {
            document.getElementById("<%=ddlSPType.ClientID%>").focus(); 
            alert("Select Supplier Type Code.");
            return false;
       }
  
  else if (document.getElementById("<%=ddlSupplierCode.ClientID%>").value == "[Select]")
	     {
	           document.getElementById("<%=ddlSupplierCode.ClientID%>").focus();
                alert("Select Supplier Code.");
                return false;
           
          }
           else if (document.getElementById("<%=ddlSupplierAgentCode.ClientID%>").value == "[Select]")
	     {
	           document.getElementById("<%=ddlSupplierAgentCode.ClientID%>").focus();
                alert("Select Supplier Agent Code.");
                return false;
           
          }
          
          else if (document.getElementById("<%=ddlMarketCode.ClientID%>").value=="[Select]")
	    {
            document.getElementById("<%=ddlMarketCode.ClientID%>").focus(); 
            alert("Select market Code.");
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
           if (state=='New'){if(confirm('Are you sure you want to generate other services cost price list?')==false)return false;}
           if (state=='Edit'){if(confirm('Are you sure you want to generate other services cost price list?')==false)return false;}
           if (state=='Delete'){if(confirm('Are you sure you want to generate other services cost price list?')==false)return false;}
           
           }       
}



</script>
  <asp:UpdatePanel id="UpdatePanel1" runat="server">
                   <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2pt solid; BORDER-TOP: gray 2pt solid; BORDER-LEFT: gray 2pt solid; BORDER-BOTTOM: gray 2pt solid"><TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" class="field_heading" align=left colSpan=4><asp:Label id="lblHeading" runat="server" Text="Add New Other Services Cost price List" CssClass="field_heading" Width="744px"></asp:Label></TD></TR><TR><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">PL Code</SPAN></TD><TD style="WIDTH: 122px"><INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD><TD style="WIDTH: 190px" class="td_cell" align=left></TD><TD></TD></TR><TR><TD style="WIDTH: 201px; HEIGHT: 3px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier &nbsp;Type Code&nbsp;<SPAN style="COLOR: #ff0000">* </SPAN></SPAN></TD><TD style="WIDTH: 122px; HEIGHT: 3px"><SELECT style="WIDTH: 200px" id="ddlSPType" class="field_input" tabIndex=2 onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 190px; HEIGHT: 3px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier &nbsp;Type Name</SPAN></TD><TD style="HEIGHT: 3px"><SELECT style="WIDTH: 300px" id="ddlSPTypeName" class="field_input" tabIndex=3 onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Code </SPAN><SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSupplierCode" class="field_input" tabIndex=4 onchange="CallWebMethod('PartyCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Name</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlSupplierName" class="field_input" tabIndex=5 onchange="CallWebMethod('PartyName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Agent Code&nbsp; <SPAN style="COLOR: #ff0000">*</SPAN></SPAN></TD><TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSupplierAgentCode" class="field_input" tabIndex=6 onchange="CallWebMethod('SupplierAgentCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Agent Name</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlSupplierAgentName" class="field_input" tabIndex=7 onchange="CallWebMethod('SupplierAgentName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 201px" class="td_cell" align=left>Market Code <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlMarketCode" class="field_input" tabIndex=8 onchange="CallWebMethod('marketcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 190px" class="td_cell" align=left>Market Name</TD><TD><SELECT style="WIDTH: 300px" id="ddlMarketName" class="field_input" tabIndex=9 onchange="CallWebMethod('marketname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Group Code</SPAN> <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Group Name</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlGroupName" class="field_input" tabIndex=11 onchange="CallWebMethod('GroupName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 201px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Currency Code</SPAN> </SPAN></TD><TD style="WIDTH: 122px"><asp:TextBox id="txtCurrCode" tabIndex=12 runat="server" CssClass="field_input" Width="194px"></asp:TextBox></TD><TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Currency Name</SPAN></TD><TD><asp:TextBox id="txtCurrName" tabIndex=13 runat="server" CssClass="field_input" Width="294px"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 201px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN> <SPAN style="COLOR: #ff0000; FONT-FAMILY: Arial">*</SPAN></SPAN></TD><TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="field_input" tabIndex=14 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="field_input" tabIndex=15 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">From Date </SPAN></TD><TD style="WIDTH: 122px"><ews:DatePicker id="dpFromdate" tabIndex=16 runat="server" CssClass="field_input" Width="180px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD><TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">To Date </SPAN></TD><TD><ews:DatePicker id="dpToDate" tabIndex=17 runat="server" CssClass="field_input" Width="180px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD></TR><TR><TD style="WIDTH: 201px; HEIGHT: 36px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Remark</SPAN></TD><TD style="WIDTH: 183px; HEIGHT: 36px" align=left colSpan=3><TEXTAREA style="WIDTH: 607px; HEIGHT: 59px" id="txtRemark" class="field_input" tabIndex=18 runat="server"></TEXTAREA></TD></TR><TR><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Active</SPAN></TD><TD style="WIDTH: 183px" align=left colSpan=3><INPUT id="ChkActive" tabIndex=19 type=checkbox CHECKED runat="server" />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
    <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
        Visible="False" /></TD></TR><TR><TD style="HEIGHT: 22px" class="td_cell" align=right colSpan=4>
    &nbsp;<asp:Button id="btnGenerate" tabIndex=20 onclick="btnGenerate_Click" runat="server" Text="Generate" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnCancel" tabIndex=21 onclick="btnCancel_Click" 
            runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnhelp" tabIndex=22 onclick="btnhelp_Click" runat="server" 
            Text="Help" Height="20px" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
                  <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>
</asp:Content>

