<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="Customers.aspx.vb" Inherits="Customers"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

 
 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript"  type ="text/javascript" > 

function CallWebMethod(methodType)
    {
        switch(methodType)
        {
        case "agentcatcode":
                var select=document.getElementById("<%=ddlCategory.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCategoryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
        case "agentcataname":
                var select=document.getElementById("<%=ddlCategoryName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCategory.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;  
         case "sellcode":
                var select=document.getElementById("<%=ddlSelling.ClientID%>");                
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSellCurrCodeListnew(constr,codeid,FillCurrCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCurrNameListnew(constr,codeid,FillCurrNames,ErrorHandler,TimeOutHandler);
                break;
          case "sellname":
                var select=document.getElementById("<%=ddlSellingName.ClientID%>");                
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSelling.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSellCurrCodeListnew(constr,codeid,FillCurrCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCurrNameListnew(constr,codeid,FillCurrNames,ErrorHandler,TimeOutHandler);
                
                break;       
        
        case "othsellcode":
                var select=document.getElementById("<%=ddlOtherSell.ClientID%>");                
                var selectname=document.getElementById("<%=ddlOtherSellName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
          case "othsellname":
                var select=document.getElementById("<%=ddlOtherSellName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlOtherSell.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;       
                
         case "tktsellcode":
                var select=document.getElementById("<%=ddlTicketSelling.ClientID%>");                
                var selectname=document.getElementById("<%=ddlTicketSellingName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
          case "tktsellname":
                var select=document.getElementById("<%=ddlTicketSellingName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlTicketSelling.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;                             
        case "currcode":
                var select=document.getElementById("<%=ddlCurrency.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCurrencyName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       
         case "currname":
                var select=document.getElementById("<%=ddlCurrencyName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCurrency.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       
         case "ctrycode":
                var select=document.getElementById("<%=ddlCountry.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
               
                //ColServices.clsServices.GetCustSectorCodeListnew(constr,codeid,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
               // ColServices.clsServices.GetCustSectorNameListnew(constr,codeid,null,FillSectorNames,ErrorHandler,TimeOutHandler);                
               
                break;
            case "ctryname":
                var select=document.getElementById("<%=ddlCountryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCountry.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetCustSectorCodeListnew(constr,codeid,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
               ColServices.clsServices.GetCustSectorNameListnew(constr,codeid,null,FillSectorNames,ErrorHandler,TimeOutHandler);
                break;
            case "citycode":
                var select=document.getElementById("<%=ddlCity.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                 ColServices.clsServices.GetCustSectorCodeListnew(constr,codeid,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustSectorNameListnew(constr,codeid,null,FillSectorNames,ErrorHandler,TimeOutHandler);                
                break;
          case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCity.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCustSectorCodeListnew(constr,codeid,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustSectorNameListnew(constr,codeid,null,FillSectorNames,ErrorHandler,TimeOutHandler);                
                break; 
          case "salepcode":
                var select=document.getElementById("<%=ddlSalesPerson.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSalesPersonName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
          case "salepname":
                var select=document.getElementById("<%=ddlSalesPersonName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSalesPerson.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
                
                
           case "marketcode":
                var select=document.getElementById("<%=ddlMarket.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;  
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCountryCodeListnew(constr,codeid,FillCountryCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCountryNameListnew(constr,codeid,FillCountryNames,ErrorHandler,TimeOutHandler);
             break;
           case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlMarket.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCountryCodeListnew(constr,codeid,FillCountryCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCountryNameListnew(constr,codeid,FillCountryNames,ErrorHandler,TimeOutHandler);
                break;
                       
            case "sectorcode":
                var select=document.getElementById("<%=ddlSector.ClientID%>");
                var selectname=document.getElementById("<%=ddlSectorName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 
            case "sectorname":
                var select=document.getElementById("<%=ddlSectorName.ClientID%>");
                var selectname=document.getElementById("<%=ddlSector.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
                
            case "postcode":
                var select=document.getElementById("<%=ddlPostCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlPostName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;         
                break;
            case "postname":
                var select=document.getElementById("<%=ddlPostName.ClientID%>");
                var selectname=document.getElementById("<%=ddlPostCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;         
                break;    
           case "acccode":
                var select=document.getElementById("<%=ddlAccCode.ClientId%>");
                var selectname=document.getElementById("<%=ddlAccName.ClientId%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
           case "accname":
                var select=document.getElementById("<%=ddlAccName.ClientId%>");
                var selectname=document.getElementById("<%=ddlAccCode.ClientId%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;         
         
         
        }
        }
        
        
function FillCurrCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCurrency.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
function FillCurrNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCurrencyName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


function FillCityCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCity.ClientID%>");
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
function FillSectorCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlSector.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

function FillSectorNames(result)
    {
    	var ddl = document.getElementById("<%=ddlSectorName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
        
        
        function FillCountryCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlCountry.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
function FillCountryNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCountryName.ClientID%>");
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

function FormValidationAcount(state)
{
if ((document.getElementById("<%=ddlAccCode.ClientID%>").value=="[Select]")||(document.getElementById("<%=txtAccEmail.ClientID %>").value!="")||(document.getElementById("<%=txtResFax.ClientID%>").value=="")||(document.getElementById("<%=txtResEmail.ClientID %>").value!=""))
{
 if (document.getElementById("<%=ddlAccCode.ClientID%>").value=="[Select]"){
            document.getElementById("<%=ddlAccCode.ClientID%>").focus(); 
             alert(" Select Control A/C code ");
            return false;
           }
           if(document.getElementById("<%=txtAccEmail.ClientID %>").value!="")
              {
                var emailPat = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                var emailid=document.getElementById("<%=txtAccEmail.ClientID %>").value;
               if(emailPat.test(emailid) == false)
                 {
                  alert('Invalid Email Address!Please enter valid Email i.e.(abc@abc.com)');
                  return false;
                 }  
            }
     
    }
  else
       {
       if (state=='New'){if(confirm('Are you sure you want to save Customer?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update Customer?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete Customer?')==false)return false;}  }
}		
           
           
           
           
function FormValidationReservation(state)
{
if ((document.getElementById("<%=txtResAddress1.ClientID%>").value=="")|| (document.getElementById("<%=txtResPhone1.ClientID%>").value=="")||(document.getElementById("<%=txtResFax.ClientID%>").value=="")||(document.getElementById("<%=txtResEmail.ClientID %>").value!=""))
{
 if (document.getElementById("<%=txtResAddress1.ClientID%>").value==""){
            document.getElementById("<%=txtResAddress1.ClientID%>").focus(); 
             alert("Address  field can not be blank");
            return false;
           }
           else if (document.getElementById("<%=txtResPhone1.ClientID%>").value=="") {
           document.getElementById("<%=txtResPhone1.ClientID%>").focus();
           alert("Telephone field can not be blank");
            return false;
           }
             else if (document.getElementById("<%=txtResFax.ClientID%>").value=="") {
           document.getElementById("<%=txtResFax.ClientID%>").focus();
           alert("Fax field can not be blank");
            return false;
           }
            else if(document.getElementById("<%=txtResEmail.ClientID %>").value!="")
              {
                var emailPat = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                var emailid=document.getElementById("<%=txtResEmail.ClientID %>").value;
               if(emailPat.test(emailid) == false)
                 {
                  alert('Invalid Email Address!Please enter valid Email i.e.(abc@abc.com)');
                  return false;
                 }  
            }
    }
  else
       {
       if (state=='New'){if(confirm('Are you sure you want to save Customer?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update Customer?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete Customer?')==false)return false;}  }
}			
			
function ValidEmail(State)	
{
if(document.getElementById("<%=txtSaleEmail.ClientID %>").value!="")
              {
                var emailPat = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                var emailid=document.getElementById("<%=txtSaleEmail.ClientID %>").value;
               if(emailPat.test(emailid) == false)
                 {
                  alert('Invalid Email Address!Please enter valid Email i.e.(abc@abc.com)');
                  return false;
                 }  
            }
             else
       {
       if (state=='New'){if(confirm('Are you sure you want to save Customer?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update Customer?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete Customer?')==false)return false;} 
        }
            
}
//===================Validation For Main Detail Form======================================================
			
			
function FormValidationMainDetail(state)
{

if ((document.getElementById("<%=txtCustomerCode.ClientID%>").value=="")||(document.getElementById("<%=txtCustomerName.ClientID%>").value=="")||(document.getElementById("<%=txtshortname.ClientID%>").value=="")||(document.getElementById("<%=ddlCategory.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlSelling.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlOtherSell.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlTicketSelling.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlCurrency.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlCountry.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlCity.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlMarket.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlSector.ClientID%>").value=="[Select]")||(document.getElementById("<%=ddlSalesPerson.ClientID%>").value=="[Select]")||(document.getElementById("<%=txtCommission.ClientID%>").value!=""))
{

 if (document.getElementById("<%=txtCustomerCode.ClientID%>").value==""){
            alert("Code field can not be blank");
            document.getElementById("<%=txtCustomerCode.ClientID%>").focus(); 
             return false;
           }
           else if (document.getElementById("<%=txtCustomerName.ClientID%>").value=="") {
           alert("Name field can not be blank");
           document.getElementById("<%=txtCustomerName.ClientID%>").focus();
            return false;
           }
            else if (document.getElementById("<%=txtshortname.ClientID%>").value=="") {
           document.getElementById("<%=txtshortname.ClientID%>").focus();
           alert("Short Name field can not be blank");
            return false;
           }
             else if (document.getElementById("<%=ddlCategory.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlCategory.ClientID%>").focus();
           alert("Select Category Type");
            return false;
           }        
            else if (document.getElementById("<%=ddlSelling.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlSelling.ClientID%>").focus();
           alert("Select Selling Type");
            return false;
           }               
            else if (document.getElementById("<%=ddlOtherSell.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlOtherSell.ClientID%>").focus();
           alert("Select Other selling Type");
            return false;
           }               
            else if (document.getElementById("<%=ddlTicketSelling.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlTicketSelling.ClientID%>").focus();
           alert("Select Ticket selling Type");
            return false;
           } 
           else if (document.getElementById("<%=ddlCurrency.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlCurrency.ClientID%>").focus();
           alert("Select Currency Type");
            return false;
           }    
            else if (document.getElementById("<%=ddlCountry.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlCountry.ClientID%>").focus();
           alert("Select Country Type");
            return false;
           }   
            else if (document.getElementById("<%=ddlCity.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlCity.ClientID%>").focus();
           alert("Select City Type");
            return false;
           } 
           else if (document.getElementById("<%=ddlMarket.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlMarket.ClientID%>").focus();
           alert("Select Market Type");
            return false;
           }  
           else if (document.getElementById("<%=ddlSector.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlSector.ClientID%>").focus();
           alert("Select Sector Type");
            return false;
           }        
           else if (document.getElementById("<%=ddlSalesPerson.ClientID%>").value=="[Select]") {
           document.getElementById("<%=ddlSalesPerson.ClientID%>").focus();
           alert("Select Person Type");
            return false;
           }                                              
//          else if (document.getElementById("<%=txtCommission.ClientID%>").value!="")
//            {
//              if (document.getElementById("<%=txtCommission.ClientID%>").value<=0)
//                {
//                 alert("Please enter positive numbers & grater than zero.");
//                 document.getElementById("<%=txtCommission.ClientID%>").focus();
//                 return false;
//                 }
//             }
    }
  else
       {
       if (state=='New'){if(confirm('Are you sure you want to save Customer?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update Customer?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete Customer?')==false)return false;}
       }
}
		
function checkNumber(e)
{
    if ( event.keyCode < 45 || event.keyCode > 57 )
    {
    return false;
    }
}
</script> 

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 807px; HEIGHT: 59px"><TBODY><TR><TD style="WIDTH: 228px; HEIGHT: 13px" class="field_input" contentEditable=true vAlign=middle align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Add New Customer" Width="899px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 228px; HEIGHT: 13px" class="field_input" contentEditable=true vAlign=middle align=left colSpan=2></TD></TR><TR><TD style="WIDTH: 228px; HEIGHT: 13px" class="field_input" vAlign=middle align=left><SPAN style="COLOR: #000000">Code </SPAN><SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="COLOR: #000000; HEIGHT: 13px" class="field_input" align=left><INPUT style="WIDTH: 228px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp;&nbsp; Name <SPAN style="COLOR: #000000" class="td_cell"><SPAN style="COLOR: #ff0000">* </SPAN>&nbsp; &nbsp;&nbsp; </SPAN>&nbsp; <INPUT style="WIDTH: 228px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 228px; HEIGHT: 382px" vAlign=top align=left><TABLE style="WIDTH: 143px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="BtnMainDetails" tabIndex=92 onclick="BtnMainDetails_Click" runat="server" Text="Main Details" Width="135px" CssClass="field_button" BorderWidth="1px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnReservation" tabIndex=93 onclick="BtnReservation_Click" runat="server" Text="Reservation Details" Width="135px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnSalesDetail" tabIndex=94 onclick="BtnSalesDetail_Click1" runat="server" Text="Sales Details" Width="135px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnAccountDetail" tabIndex=95 onclick="BtnAccountDetail_Click" runat="server" Text="Account Details" Width="135px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnUserEmail" tabIndex=96 onclick="BtnUserEmail_Click1" runat="server" Text="User Email" Width="135px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnWebAppr" tabIndex=97 onclick="BtnWebAppr_Click" runat="server" Text="Web Approval" Width="135px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnVisitFollow" tabIndex=98 onclick="BtnVisitFollow_Click" runat="server" Text="Visit Follow Up" Width="135px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnSurveyForm" tabIndex=10 onclick="BtnSurveyForm_Click" runat="server" Text="Survey Form Received" Width="135px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnGeneral" tabIndex=99 onclick="BtnGeneral_Click" runat="server" Text="General" Width="135px" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD><TD style="HEIGHT: 382px" class="field_input" vAlign=top align=left><asp:Panel id="PanelMain" runat="server" Width="743px" GroupingText="Main Details"><TABLE style="WIDTH: 557px"><TBODY><TR><TD style="WIDTH: 752px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Short Name</SPAN> <SPAN style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial">*</SPAN></TD><TD align=left><INPUT style="WIDTH: 188px" id="txtshortname" class="field_input" tabIndex=3 type=text runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 21px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Category</SPAN> <SPAN style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial" class="td_cell">*</SPAN><SPAN style="COLOR: #ff0000"></SPAN></TD><TD style="HEIGHT: 21px" class="field_input" align=left><SELECT style="WIDTH: 189px" id="ddlCategory" class="field_input" tabIndex=4 onchange="CallWebMethod('agentcatcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 21px" align=left><SELECT style="WIDTH: 189px" id="ddlCategoryName" class="field_input" tabIndex=5 onchange="CallWebMethod('agentcataname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial"><SPAN style="COLOR: #000000">Selli</SPAN>ng Type</SPAN> <SPAN style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial" class="td_cell">*</SPAN></TD><TD align=left><SELECT style="WIDTH: 189px" id="ddlSelling" class="field_input" tabIndex=6 onchange="CallWebMethod('sellcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 189px" id="ddlSellingName" class="field_input" tabIndex=7 onchange="CallWebMethod('sellname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 16px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Other Sell Type</SPAN><SPAN style="COLOR: #ff0000"> <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial" class="td_cell">*</SPAN></SPAN></TD><TD style="COLOR: #000000; HEIGHT: 16px" align=left><SELECT style="WIDTH: 189px" id="ddlOtherSell" class="field_input" tabIndex=8 onchange="CallWebMethod('othsellcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 16px" align=left><SELECT style="WIDTH: 189px" id="ddlOtherSellName" class="field_input" tabIndex=9 onchange="CallWebMethod('othsellname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 24px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">Ticket Selling Tye<SPAN style="COLOR: #000000">p</SPAN><SPAN style="COLOR: #ff0000"> *</SPAN></SPAN></SPAN></TD><TD style="COLOR: #000000; HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlTicketSelling" class="field_input" tabIndex=10 onchange="CallWebMethod('tktsellcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlTicketSellingName" class="field_input" tabIndex=11 onchange="CallWebMethod('tktsellname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 24px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Currency <SPAN style="COLOR: #ff0000">*</SPAN></SPAN></TD><TD style="HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlCurrency" class="field_input" tabIndex=12 onchange="CallWebMethod('currcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlCurrencyName" class="field_input" tabIndex=13 onchange="CallWebMethod('currname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 24px" class="field_input" align=left>Market <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlMarket" class="field_input" tabIndex=20 onchange="CallWebMethod('marketcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlMarketName" class="field_input" tabIndex=21 onchange="CallWebMethod('marketname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 24px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Country</SPAN> <SPAN style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial" class="td_cell">*</SPAN></TD><TD style="HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlCountry" class="field_input" tabIndex=14 onchange="CallWebMethod('ctrycode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 24px" align=left><SELECT style="WIDTH: 189px" id="ddlCountryName" class="field_input" tabIndex=15 onchange="CallWebMethod('ctryname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 27px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">City</SPAN> <SPAN style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial" class="td_cell">*</SPAN></TD><TD style="HEIGHT: 27px" align=left><SELECT style="WIDTH: 189px" id="ddlCity" class="field_input" tabIndex=16 onchange="CallWebMethod('citycode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 27px" align=left><SELECT style="WIDTH: 189px" id="ddlCityName" class="field_input" tabIndex=17 onchange="CallWebMethod('cityname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 27px" class="field_input" align=left>Sector <SPAN style="COLOR: red">*</SPAN></TD><TD style="HEIGHT: 27px" align=left><SELECT style="WIDTH: 189px" id="ddlSector" class="field_input" tabIndex=22 onchange="CallWebMethod('sectorcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 27px" align=left><SELECT style="WIDTH: 189px" id="ddlSectorName" class="field_input" tabIndex=23 onchange="CallWebMethod('sectorname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 27px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Sales Persons <SPAN style="COLOR: red">*</SPAN></SPAN></TD><TD style="HEIGHT: 27px" align=left><SELECT style="WIDTH: 189px" id="ddlSalesPerson" class="field_input" tabIndex=18 onchange="CallWebMethod('salepcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 27px" align=left><SELECT style="WIDTH: 189px" id="ddlSalesPersonName" class="field_input" tabIndex=19 onchange="CallWebMethod('salepname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 752px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Commission %</SPAN></TD><TD align=left><INPUT style="WIDTH: 100px" id="txtCommission" class="field_input" tabIndex=24 type=text runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Lock-Status</SPAN></TD><TD style="WIDTH: 100px" align=left><asp:Label id="lbllockstatus" runat="server" ForeColor="Red" Width="180px" CssClass="field_input"></asp:Label></TD></TR><TR><TD style="WIDTH: 752px" class="field_input" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Active</SPAN></TD><TD align=left><INPUT id="chkActive" tabIndex=25 type=checkbox CHECKED runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 752px; HEIGHT: 22px" class="field_input" align=left><asp:Button id="btnSave_Main" tabIndex=26 runat="server" Text="Save" Width="46px" CssClass="field_button"></asp:Button></TD><TD style="HEIGHT: 22px" align=left><asp:Button id="btnCancel_Main" tabIndex=27 onclick="btnCancel_Main_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 22px" align=left></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelReservstion" runat="server" Height="280px" Width="743px" 
        GroupingText="Reservation Details"><TABLE style="WIDTH: 732px" align=center><TBODY><TR><TD style="WIDTH: 1px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Address</SPAN><SPAN style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial" class="td_cell">*</SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress1" class="field_input" tabIndex=28 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px; HEIGHT: 24px" align=left></TD><TD style="WIDTH: 5px; HEIGHT: 24px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress2" class="field_input" tabIndex=29 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px" align=left></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress3" class="field_input" tabIndex=30 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Telephone<SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></SPAN></SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone1" class="field_input" tabIndex=31 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px" align=left></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone2" class="field_input" tabIndex=32 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px; HEIGHT: 4px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Fax<SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></SPAN></SPAN></TD><TD style="WIDTH: 5px; HEIGHT: 4px" align=left><INPUT style="WIDTH: 297px" id="txtResFax" class="field_input" tabIndex=33 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact</SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResContact1" class="field_input" tabIndex=34 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px" align=left></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResContact2" class="field_input" tabIndex=35 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Email</SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResEmail" class="field_input" tabIndex=36 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 1px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">CommunicateBy</SPAN></TD><TD style="WIDTH: 5px" align=left><asp:DropDownList id="ddlCommunicateBy" tabIndex=37 runat="server" CssClass="field_input" Width="189px" AutoPostBack="True"><asp:ListItem>Email</asp:ListItem>
<asp:ListItem>Fax</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 1px" align=left><asp:Button id="BtnResSave" 
                    tabIndex=38 runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 5px" align=left><asp:Button id="BtnResCancel" tabIndex=39 onclick="BtnResCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelSales" runat="server" Height="270px" Width="743px" 
        GroupingText="Sales Details"><TABLE style="WIDTH: 491px" border=0><TBODY><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Recommended By</SPAN></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleRecommended" class="field_input" tabIndex=40 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Telephone</SPAN></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleTelephone1" class="field_input" tabIndex=41 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleTelephone2" class="field_input" tabIndex=42 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Fax</SPAN></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleFax" class="field_input" tabIndex=43 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact</SPAN></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleContact1" class="field_input" tabIndex=44 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleContact2" class="field_input" tabIndex=45 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">E-mail</SPAN></TD><TD style="WIDTH: 100px; HEIGHT: 26px" align=left><INPUT style="WIDTH: 295px" id="txtSaleEmail" class="field_input" tabIndex=46 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>
            <asp:Button id="BtnSaleSave" tabIndex=47 runat="server" Text="Save" 
                CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px" align=left><asp:Button id="BtnSaleCancel" tabIndex=48 onclick="BtnSaleCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelAccounts" runat="server" Height="375px" Width="743px" 
        GroupingText="Accounts  Details"><TABLE style="WIDTH: 491px" border=0><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 15px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Telephone</SPAN></TD><TD style="WIDTH: 100px; HEIGHT: 15px" align=left><INPUT style="WIDTH: 315px" id="txtAccTelephone1" class="field_input" tabIndex=49 type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 100px; HEIGHT: 15px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 315px" id="txtAccTelephone2" class="field_input" tabIndex=50 type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" class="td_cell" align=left>Fax</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 315px" id="txtAccFax" class="field_input" tabIndex=51 type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>Contact</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 315px" id="txtAccContact1" class="field_input" tabIndex=52 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 315px" id="txtAccContact2" class="field_input" tabIndex=53 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" class="td_cell" align=left>E-mail</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 315px" id="txtAccEmail" class="field_input" tabIndex=54 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">Control A/C Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></SPAN></SPAN></TD><TD align=left colSpan=2><SELECT style="WIDTH: 182px" id="ddlAccCode" class="field_input" tabIndex=55 onchange="CallWebMethod('acccode')" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp; <SELECT style="WIDTH: 246px" id="ddlAccName" class="field_input" tabIndex=56 onchange="CallWebMethod('accname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Credit Days</SPAN></TD><TD align=left><INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="TxtAccCreditDays" class="field_input" tabIndex=57 type=text maxLength=5 runat="server" /> &nbsp; &nbsp; <SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Credit Limit</SPAN> <INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtAccCreditLimit" class="field_input" tabIndex=58 type=text runat="server" /></TD><TD align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Cash Custmer</SPAN></TD><TD style="WIDTH: 100px" align=left><INPUT id="ChkCashSup" tabIndex=59 type=checkbox runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>Booking Credit Limit</TD><TD align=left><INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="txtAccBooking" class="field_input" tabIndex=60 type=text runat="server" /></TD><TD align=left></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px" align=left>Post Account </TD><TD style="HEIGHT: 22px" align=left colSpan=2>&nbsp;<SELECT style="WIDTH: 182px" id="ddlPostCode" class="field_input" tabIndex=61 onchange="CallWebMethod('postcode')" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp; <SELECT style="WIDTH: 246px" id="ddlPostName" class="field_input" tabIndex=62 onchange="CallWebMethod('postname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD align=left><INPUT id="ChkAccBooking2" tabIndex=63 type=checkbox runat="server" /> Booking credit limits need to bechecked or no</TD><TD align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left><asp:Button id="BtnAccSave" tabIndex=64 runat="server" Text="Save" Width="50px" CssClass="field_button"></asp:Button></TD><TD align=left><asp:Button id="BtnAccCancel" tabIndex=65 onclick="BtnAccCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD><TD align=left></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelUser" runat="server" Height="250px" Width="743px" 
        GroupingText="User Details"><TABLE style="WIDTH: 403px"><TBODY><TR><TD style="WIDTH: 355px; TEXT-ALIGN: right" align=right>
            <asp:Button id="BtnUserDetailAdd" tabIndex=66 onclick="BtnUserDetailAdd_Click" 
                runat="server" Text="Add" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 355px" align=left>&nbsp;<asp:GridView id="gv_Email" tabIndex=67 runat="server" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="no" HeaderText="Sr No"></asp:BoundField>
<asp:TemplateField HeaderText="Contect Person Name &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>
                                                <input id="txtPerson" runat="server" class="field_input" maxlength="100" style="width: 215px"
                                                    type="text" />
                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Email Address &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>
                                                <input id="txtEmail" runat="server" class="field_input" maxlength="100" style="width: 220px"
                                                    type="text" />
                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Contact No &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>
                                                <input id="txtContactNo" runat="server" class="field_input" maxlength="15" style="width: 159px"
                                                    type="text" />
                                            
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></TD></TR><TR><TD style="WIDTH: 355px; HEIGHT: 39px" align=left><TABLE><TBODY><TR><TD style="WIDTH: 29px">
                <asp:Button id="BtnUserSave" tabIndex=68 onclick="BtnUserSave_Click" 
                    runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="BtnUserCancel" tabIndex=69 onclick="BtnUserCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelWebApproval" runat="server" Height="275px" Width="743px" 
        GroupingText="Web Approval"><TABLE style="WIDTH: 553px" border=0><TBODY><TR><TD style="WIDTH: 6px" align=left>ApprovalForWeb</TD><TD style="WIDTH: 109px" align=left><INPUT id="ChkWebApprove" tabIndex=70 type=checkbox runat="server" /></TD></TR><TR><TD style="WIDTH: 6px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">UserName</SPAN></TD><TD style="WIDTH: 109px" align=left><INPUT id="txtWebAppUsername" class="field_input" tabIndex=71 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 6px" align=left>Password</TD><TD style="WIDTH: 109px" align=left><INPUT id="txtWebAppPassword" class="field_input" tabIndex=72 readOnly type=text maxLength=50 runat="server" />&nbsp;<asp:Button id="BtnShowPassword" tabIndex=73 onclick="BtnShowPassword_Click" runat="server" Text="Show Password" Width="103px" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 6px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact</SPAN></TD><TD style="WIDTH: 109px" align=left><INPUT id="txtWebAppContact" class="field_input" tabIndex=74 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 6px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Email IDToSend</SPAN></TD><TD style="WIDTH: 109px" align=left><INPUT style="WIDTH: 262px" id="txtWebAppEmail" class="field_input" tabIndex=75 type=text maxLength=100 runat="server" /></TD></TR><TR><TD align=left colSpan=2></TD></TR><TR><TD align=left colSpan=2>
            <asp:Button id="BtnWebInviteCustomer" tabIndex=76 
                onclick="BtnWebInviteCustomer_Click" runat="server" 
                Text="Invite Customer First Time For Web  Access" 
                CssClass="field_button"></asp:Button></TD></TR><TR><TD align=left colSpan=2>
                <asp:Button id="BtnWebResendPasswprd" tabIndex=77 
                    onclick="BtnWebResendPasswprd_Click" runat="server" 
                    Text="Resend Password Email To Customer" CssClass="field_button"></asp:Button></TD></TR><TR><TD align=left colSpan=2></TD></TR><TR><TD style="WIDTH: 6px" align=left>
            <asp:Button id="BtnWebAppSave" tabIndex=78 onclick="BtnWebAppSave_Click" 
                runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 109px" align=left><asp:Button id="BtnWebAppCancel" tabIndex=79 onclick="BtnWebAppCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelVisit" runat="server" Height="275px" Width="743px" 
        GroupingText="Visit Follow Up"><TABLE style="WIDTH: 403px"><TBODY><TR><TD style="TEXT-ALIGN: right" align=left>
            <asp:Button id="btnVisitFollo" tabIndex=80 onclick="btnVisitFollo_Click" 
                runat="server" Text="Add" CssClass="field_button"></asp:Button></TD></TR><TR><TD align=left>&nbsp;<asp:GridView id="gv_VisitFollow" tabIndex=81 runat="server" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="no" HeaderText="Sr No"></asp:BoundField>
<asp:TemplateField HeaderText="Visit Date"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<ews:DatePicker id="dpDateVisit" runat="server" Width="185px" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"></ews:DatePicker> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Sales Person Code"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:DropDownList id="ddlSalesPersonCode" onSelectedIndexChanged="ddlSalesPersonCode_SelectedIndexChanged" AutoPostback="True" runat="server" Width="102px" CssClass="field_input"></asp:DropDownList>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Sales Person Name"><EditItemTemplate>
<asp:TextBox id="TextBox3" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT id="txtSalesPersonName" type=text runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Remark"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT id="txtRemark" type=text runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></TD></TR><TR><TD style="HEIGHT: 39px" align=left><TABLE>
<TBODY><TR><TD style="WIDTH: 29px"><asp:Button id="BtnVisitSave" tabIndex=82 
        onclick="BtnVisitSave_Click" runat="server" Text="Save" CssClass="field_button"></asp:Button></TD>
<TD style="WIDTH: 100px"><asp:Button id="BtnVisitCancel" tabIndex=83 onclick="BtnVisitCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelSurvey" runat="server" Height="250px" Width="743px" 
        GroupingText="Survey From Received">&nbsp;<TABLE style="WIDTH: 403px"><TBODY><TR>
        <TD style="TEXT-ALIGN: right" align=left><asp:Button id="Btnaddsurvey" tabIndex=84 
                onclick="Btnaddsurvey_Click" runat="server" Text="Add" CssClass="field_button"></asp:Button>&nbsp;
         <asp:Button id="btnViewForm" tabIndex=85 runat="server" Text="View Form Submitted" CssClass="field_button"></asp:Button> </TD></TR>
         <TR><TD align=left><asp:GridView id="grvSurvey" tabIndex=86 runat="server" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="no" HeaderText="Sr No"></asp:BoundField>
<asp:TemplateField HeaderText="Survey Date"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<ews:DatePicker id="dpDateSurvey" runat="server" Width="185px" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"></ews:DatePicker> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Submitted By"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" CssClass="field_input"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT id="txtSubmitedBy" type=text runat="server" class="field_input" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Remarks"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT id="txtRemarkSurvey" type=text runat="server" class="field_input" /> 
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> </TD></TR><TR><TD align=left><TABLE><TBODY><TR><TD style="WIDTH: 29px"><asp:Button id="BtnSurveySave" tabIndex=87 onclick="BtnSurveySave_Click" runat="server" Text="Save" Width="60px" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px">
<asp:Button id="BtnSurveyCancel" tabIndex=88 onclick="BtnSurveyCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> 
    <asp:Panel id="PanelGeneral" runat="server" Height="200px" Width="743px" 
        GroupingText="General"><TABLE style="WIDTH: 414px"><TBODY><TR><TD style="WIDTH: 100px" align=left>General Comments</TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><asp:TextBox id="txtGeneral" tabIndex=89 runat="server" Height="100px" CssClass="field_input" Width="389px" TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><TABLE><TBODY><TR><TD style="WIDTH: 100px">
            <asp:Button id="BtnGeneralSave" tabIndex=90 onclick="BtnGeneralSave_Click" 
                runat="server" Text="Save" CssClass="field_button"></asp:Button></TD>
        <TD style="WIDTH: 100px"><asp:Button id="BtnGeneralCancel" tabIndex=91 onclick="BtnGeneralCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> </TD></TR><TR><TD style="WIDTH: 228px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

