<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VisitfollSearch.aspx.vb" Inherits="VisitfollSearch"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<%--<script language="javascript" src="js\date-picker.js"></script>  --%>
    <%--<script language="javascript" src="js\datefun.js"></script>--%>
   <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>

<script type="text/javascript">
<!--
//  WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
function CallWebMethod(methodType)
    {
        switch(methodType)
        {                      
         case "ctrycode":
                var select=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlcontName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,ctry,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,ctry,FillCityNames,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,ctry,null,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,ctry,null,null,FillCustomerName,ErrorHandler,TimeOutHandler);


                break;
                
            case "ctryname":
                var select=document.getElementById("<%=ddlcontName.ClientID%>");
                var ctry=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlContCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                
                ColServices.clsServices.GetCityCodeListnew(constr,ctry,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,ctry,FillCityNames,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,ctry,null,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,ctry,null,null,FillCustomerName,ErrorHandler,TimeOutHandler);
                break;

            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var city=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var selectctry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectctry.options[selectctry.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value                  
                var txtcode=document.getElementById("<%=txtCityCode.ClientID%>");
                txtcode.value=city;
                var txtname=document.getElementById("<%=txtCityName.ClientID%>");
                txtname.value=select.options[select.selectedIndex].value;
                
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,null,city,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,null,city,null,FillCustomerName,ErrorHandler,TimeOutHandler);

                break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var city=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var selectctry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectctry.options[selectctry.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                var txtcode=document.getElementById("<%=txtCityCode.ClientID%>");
                txtcode.value=city;
                var txtname=document.getElementById("<%=txtCityName.ClientID%>");
                txtname.value=select.options[select.selectedIndex].text;
                
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,null,city,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,null,city,null,FillCustomerName,ErrorHandler,TimeOutHandler);


                break; 
          case "agentcode":
                var select=document.getElementById("<%=ddlcustomercode.ClientID%>");
                var selectname=document.getElementById("<%=ddlcustomername.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var txtcode=document.getElementById("<%=txtcustcode.ClientID%>");
                txtcode.value=selectname.value;
                var txtname=document.getElementById("<%=txtcustname.ClientID%>");
                txtname.value=select.options[select.selectedIndex].text;
                
                 break;
          case "agentname":
                var select=document.getElementById("<%=ddlcustomername.ClientID%>");
                var selectname=document.getElementById("<%=ddlcustomercode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                

                var txtcode=document.getElementById("<%=txtcustcode.ClientID%>");
                txtcode.value=select.value;
                var txtname=document.getElementById("<%=txtcustname.ClientID%>");
                txtname.value=select.options[select.selectedIndex].text;
                break;                                 
                
          case "salepcode":
                var select=document.getElementById("<%=ddlSalesPerson.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSalesPersonName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var txtcode=document.getElementById("<%=txtsmancode.ClientID%>");
                txtcode.value=selectname.value;
                var txtname=document.getElementById("<%=txtsmanname.ClientID%>");
                txtname.value=select.options[select.selectedIndex].text;
                
                break;
          case "salepname":
                var select=document.getElementById("<%=ddlSalesPersonName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSalesPerson.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var txtcode=document.getElementById("<%=txtsmancode.ClientID%>");
                txtcode.value=select.value;
                var txtname=document.getElementById("<%=txtsmanname.ClientID%>");
                txtname.value=select.options[select.selectedIndex].text;
                
                break;                                
        }
        }
        
        
        
function FillCityCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

function FillCityNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


function FillCustomerCodes(result)
    {
   
      	var ddl = document.getElementById("<%=ddlcustomercode .ClientID%>");
        RemoveAll(ddl)
 	    for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
         
        }
        ddl.value="[Select]";
       
        
    }

    function FillCustomerName(result)
    {
        var ddl = document.getElementById("<%=ddlcustomername.ClientID%>");
 	   RemoveAll(ddl)
 	  for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
          
        }
        ddl.value="[Select]";
       
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

function ChangeDate()
{
   
     var txtfdate=document.getElementById("<%=txtFromDate.ClientID%>");
     //var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
   
       if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
      else {ColServices.clsServices.GetQueryReturnFromToDate('FromDate',30,txtfdate.value,FillToDate,ErrorHandler,TimeOutHandler);}
}
function FillToDate(result)
    {
       	 var txttdate=document.getElementById("<%=txtToDate.ClientID%>");
      	 txttdate.value=result;
    }


</script> <table>
        <tr>
            <td style="width: 100px">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center; height: 17px;">
                            Report of Visit Follow Up</td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <span class="td_cell" style="color: #ff0000"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
  
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="lblCountryCode" runat="server" Text="Country Code" CssClass="td_cell" Width="110px"></asp:Label></TD><TD><SELECT style="WIDTH: 172px" id="ddlContCode" class="field_input" tabIndex=3 onchange="CallWebMethod('ctrycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcountryname" runat="server" Text="Country Name" CssClass="td_cell" Width="122px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlcontName" class="field_input" tabIndex=4 onchange="CallWebMethod('ctryname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="height: 22px"><asp:Label id="lblCityCode" runat="server" Text="City Code" CssClass="td_cell" Width="111px"></asp:Label></TD><TD style="height: 22px"><SELECT style="WIDTH: 172px" id="ddlCityCode" class="field_input" tabIndex=5 onchange="CallWebMethod('citycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="height: 22px"><asp:Label id="lblcityname" runat="server" Text="City Name" CssClass="td_cell" Width="121px"></asp:Label></TD><TD style="height: 22px"><SELECT style="WIDTH: 237px" id="ddlCityName" class="field_input" tabIndex=6 onchange="CallWebMethod('cityname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR>
    <tr>
        <td>
            <asp:Label ID="lblcustcode" runat="server" CssClass="td_cell" Text="Customer Code"
                Width="111px"></asp:Label></td>
        <td>
            <SELECT style="WIDTH: 172px" id="ddlcustomercode" class="field_input" tabIndex=5 onchange="CallWebMethod('agentcode')" runat="server" Visible="true">
                <OPTION selected></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblcustname" runat="server" CssClass="td_cell" Text="Customer Name"
                Width="111px"></asp:Label></td>
        <td>
            <SELECT style="WIDTH: 237px" id="ddlcustomername" class="field_input" tabIndex=6 onchange="CallWebMethod('agentname')" runat="server" Visible="true">
                <OPTION selected></option>
            </select>
        </td>
    </tr>
    <TR><TD>
    <asp:Label ID="lblspersoncode" runat="server" CssClass="td_cell" Text="Salesman Code" Width="108px"></asp:Label></TD><TD>
    <select id="ddlSalesPerson" runat="server" class="field_input" onchange="CallWebMethod('salepcode')"
        style="width: 173px" tabindex="22">
        <option selected="selected"></option>
    </select>
</TD><TD>
    <asp:Label ID="lblspersonname" runat="server" CssClass="td_cell" Text="Salesman Name" Width="120px"></asp:Label></TD><TD>
    <select id="ddlSalesPersonName" runat="server" class="field_input" onchange="CallWebMethod('salepname')"
        style="width: 235px" tabindex="23">
        <option selected="selected"></option>
    </select>
</TD></TR><TR><TD><asp:Label id="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_input" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" InvalidValueBlurredMessage=" Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR><TR><TD></TD><TD colSpan=3>
    &nbsp;&nbsp;</TD></TR><TR><TD style="TEXT-ALIGN: center; height: 22px;" colSpan=4><asp:Button id="BtnClear" tabIndex=13 runat="server" Text="Clear" CssClass="field_button" Width="61px"></asp:Button>&nbsp;
    <asp:Button id="BtnPrint" tabIndex=15 runat="server" Text="Load Report" CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="btnhelp" tabIndex=16 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" Width="46px"></asp:Button>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /><input id="txtCityCode" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" /><input id="txtCityName" runat="server" style="visibility: hidden;
                width: 9px; height: 3px" type="text" />
    <input id="txtcustcode" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" />
    <input id="txtcustname" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" />
    <input id="txtsmancode" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" />
    <input id="txtsmanname" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" /></TD></TR><TR><TD colSpan=4></TD></TR></TBODY></TABLE> <cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date" Enabled="True"></cc1:MaskedEditExtender>
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                </table>
                </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

</asp:Content>