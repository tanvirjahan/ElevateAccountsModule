<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptTicketPriceSearch.aspx.vb" Inherits="rptTicketPriceSearch"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

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
WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
function CallWebMethod(methodType)
    {
        switch(methodType)
        {
         case "sptype":
                var select=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSpTypeName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;        
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,sptype,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,sptype,FillCatNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,sptype,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,sptype,FillSellCatNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSuppAgentCodeListnew(constr,sptype,FillSuppAgentCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSuppAgentNameListnew(constr,sptype,FillSuppAgentNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,null,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,null,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);
             break;
           
           case "sptypename":
                var select=document.getElementById("<%=ddlSpTypeName.ClientID%>");
                var sptype=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetCatCodeListnew(constr,sptype,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,sptype,FillCatNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,sptype,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,sptype,FillSellCatNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSuppAgentCodeListnew(constr,sptype,FillSuppAgentCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSuppAgentNameListnew(constr,sptype,FillSuppAgentNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,null,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,null,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);
                
                break;
                
        case "catcode":
                var select=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCatName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);

                break;
        case "catname":
                var select=document.getElementById("<%=ddlCatName.ClientID%>");                
                var cat=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);
                
                break;
       
         case "ctrycode":
         
                var select=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlcontName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,ctry,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,ctry,FillCityNames,ErrorHandler,TimeOutHandler);

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
               
                var selectcatcode=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=selectcatcode.options[selectcatcode.selectedIndex].text;
               
                var selectsellcatcode=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=selectsellcatcode.options[selectsellcatcode.selectedIndex].text;                       


                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,sellcat,ctry,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,sellcat,ctry,null,FillSupplierNames,ErrorHandler,TimeOutHandler);

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

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
               
                var selectcatcode=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=selectcatcode.options[selectcatcode.selectedIndex].text;
               
                var selectsellcatcode=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=selectsellcatcode.options[selectsellcatcode.selectedIndex].text;                       

                
                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,sellcat,ctry,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,sellcat,ctry,null,FillSupplierNames,ErrorHandler,TimeOutHandler);

                break;
            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var city=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
               
                var selectcatcode=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=selectcatcode.options[selectcatcode.selectedIndex].text;
               
                var selectsellcatcode=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=selectsellcatcode.options[selectsellcatcode.selectedIndex].text;                       

                var selectcountry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectcountry.options[selectcountry.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,sellcat,ctry,city,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,sellcat,ctry,city,FillSupplierNames,ErrorHandler,TimeOutHandler);

                break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var city=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
               
                var selectcatcode=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=selectcatcode.options[selectcatcode.selectedIndex].text;
               
                var selectsellcatcode=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=selectsellcatcode.options[selectsellcatcode.selectedIndex].text;                       

                var selectcountry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectcountry.options[selectcountry.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,sellcat,ctry,city,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,sellcat,ctry,city,FillSupplierNames,ErrorHandler,TimeOutHandler);
                
                break; 
                
            case "supplieragentcode":
                var select=document.getElementById("<%=ddlSupplierAgentCode.ClientID%>"); 
                var party=select.options[select.selectedIndex].text;               
                var selectname=document.getElementById("<%=ddlSupplierAgentName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

            case "supplieragentname":
                var select=document.getElementById("<%=ddlSupplierAgentName.ClientID%>");                
                var party=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;    
                
            case "suppliercode":
                var select=document.getElementById("<%=ddlPartyCode.ClientID%>"); 
                var party=select.options[select.selectedIndex].text;               
                var selectname=document.getElementById("<%=ddlPartyName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

            case "suppliername":
                var select=document.getElementById("<%=ddlPartyName.ClientID%>");                
                var party=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlPartyCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;       

            case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var plgrp=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GettktSellTypenew(constr,'',plgrp,FillSellCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GettktSellTypenamenew(constr,'',plgrp,FillSellNames,ErrorHandler,TimeOutHandler);
                break; 

            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var plgrp=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GettktSellTypenew(constr,'',plgrp,FillSellCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GettktSellTypenamenew(constr,'',plgrp,FillSellNames,ErrorHandler,TimeOutHandler);
                break;

         case "sellcode":
                var select=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=select.options[select.selectedIndex].text;                       
                var selectname=document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

          case "sellname":
                var select=document.getElementById("<%=ddlSellingName.ClientID%>");      
                var sellcat=select.options[select.selectedIndex].value;                                                 
                var selectname=document.getElementById("<%=ddlSellingCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;       

            case "seascode":
                var select=document.getElementById("<%=ddlseas1code.ClientID%>");
                var seascode=select.options[select.selectedIndex].text;                       
                var selectname=document.getElementById("<%=ddlseas1name.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 

            case "seasname":
                var select=document.getElementById("<%=ddlseas1name.ClientID%>");
                var seascode=select.options[select.selectedIndex].value;                       
                var selectname=document.getElementById("<%=ddlseas1code.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

         case "flightclasscode":
                var select=document.getElementById("<%=ddlFlightClassCode.ClientID%>"); 
                var party=select.options[select.selectedIndex].text;               
                var selectname=document.getElementById("<%=ddlFlightClassname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

          case "flightclassname":
                var select=document.getElementById("<%=ddlFlightClassname.ClientID%>");                
                var party=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlFlightClassCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;       
                
        }
        }
        
        
function FillCatCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
function FillCatNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCatName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


function FillSellCatCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlSellingCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
    
function FillSellCatNames(result)
    {
    	var ddl = document.getElementById("<%=ddlSellingName.ClientID%>");
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

function FillSupplierCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlPartyCode.ClientID%>");
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
    	var ddl = document.getElementById("<%=ddlPartyName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

function FillSuppAgentCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
function FillSuppAgentNames(result)
    {
    	var ddl = document.getElementById("<%=ddlSupplierAgentName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


function FillSellCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlSellingCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
function FillSellNames(result)
    {
    	var ddl = document.getElementById("<%=ddlSellingName.ClientID%>");
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


</script> 

    <table>
        <tr>
            <td style="width: 100px">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Ticketing
                            Price List</td>
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
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="lblSupplierTypeCode" runat="server" Text="Supplier Type Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlSPTypeCode" class="field_input" tabIndex=1 onchange="CallWebMethod('sptype')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblsuppliertypename" runat="server" Text="Supplier Type Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 238px" id="ddlSpTypeName" class="field_input" tabIndex=2 onchange="CallWebMethod('sptypename')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblCountryCode" runat="server" Text="Country Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlContCode" class="field_input" tabIndex=3 onchange="CallWebMethod('ctrycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcountryname" runat="server" Text="Country Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlcontName" class="field_input" tabIndex=4 onchange="CallWebMethod('ctryname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblCityCode" runat="server" Text="City Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlCityCode" class="field_input" tabIndex=5 onchange="CallWebMethod('citycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcityname" runat="server" Text="City Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlCityName" class="field_input" tabIndex=6 onchange="CallWebMethod('cityname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT>&nbsp;&nbsp;</TD></TR><TR><TD><asp:Label id="lblCategorycode" runat="server" Text="Category Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlCCode" class="field_input" tabIndex=7 onchange="CallWebMethod('catcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblCategoryName" runat="server" Text="Category Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlCatName" class="field_input" tabIndex=8 onchange="CallWebMethod('catname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="Label9" runat="server" Text="Supplier Agent Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlSupplierAgentCode" class="field_input" tabIndex=9 onchange="CallWebMethod('supplieragentcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="Label10" runat="server" Text="Supplier Agent Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlSupplierAgentName" class="field_input" tabIndex=10 onchange="CallWebMethod('supplieragentname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblSupplierCode" runat="server" Text="Supplier Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlPartyCode" class="field_input" tabIndex=11 onchange="CallWebMethod('suppliercode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblSupplierName" runat="server" Text="Supplier Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlPartyName" class="field_input" tabIndex=12 onchange="CallWebMethod('suppliername')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblMarketCode" runat="server" Text="Market Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlMarketCode" class="field_input" tabIndex=13 onchange="CallWebMethod('marketcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblMarketName" runat="server" Text="Market Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlMarketName" class="field_input" tabIndex=14 onchange="CallWebMethod('marketname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 99px"><asp:Label id="lblSellingCategoryCode" runat="server" Text="Selling Type Code" Width="123px" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlSellingCode" class="field_input" tabIndex=15 onchange="CallWebMethod('sellcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblsellingcategoryname" runat="server" Text="Selling Type Name" Width="124px" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlSellingName" class="field_input" tabIndex=16 onchange="CallWebMethod('sellname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD colSpan=4><asp:Label id="Label8" runat="server" Text="( * Leave Selling Type Code Field as Blank to Print Net Cost)" ForeColor="#C00000" Width="664px" CssClass="td_cell"></asp:Label></TD></TR>
    <tr>
        <td>
            <asp:Label ID="lblatatus" runat="server" CssClass="td_cell" Text="Approval Status"
                Width="103px"></asp:Label></td>
        <td style="width: 176px">
            <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="field_input" Width="167px">
                <asp:ListItem Value="2">All</asp:ListItem>
                <asp:ListItem Value="1">Approve</asp:ListItem>
                <asp:ListItem Value="0">Unapprove</asp:ListItem>
            </asp:DropDownList></td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <TR><TD><asp:Label id="Label6" runat="server" Text="Flight Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlFlightCode" class="field_input" tabIndex=17 runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD></TD><TD></TD></TR><TR><TD><asp:Label id="Label11" runat="server" Text="Flight Class Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 170px" id="ddlFlightClassCode" class="field_input" tabIndex=18 onchange="CallWebMethod('flightclasscode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="Label12" runat="server" Text="Flight Class Name" Width="124px" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlFlightClassname" class="field_input" tabIndex=19 onchange="CallWebMethod('flightclassname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="Label4" runat="server" Text="Sub Season Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><asp:DropDownList id="ddlseas1code" runat="server" Width="200px" OnSelectedIndexChanged="ddlseas1code_SelectedIndexChanged" CssClass="field_input" AutoPostBack="True"></asp:DropDownList></TD><TD><asp:Label id="Label5" runat="server" Text="Sub Season Name" Width="124px" CssClass="td_cell"></asp:Label></TD><TD><asp:DropDownList id="ddlseas1name" runat="server" Width="237px" OnSelectedIndexChanged="ddlseas1name_SelectedIndexChanged" CssClass="field_input" AutoPostBack="True"></asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><asp:TextBox id="txtFromDate" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" ControlExtender="MEFromDate" ControlToValidate="txtFromDate" Display="Dynamic"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" ControlExtender="METoDate" ControlToValidate="txtToDate" Display="Dynamic"></cc1:MaskedEditValidator></TD></TR><TR><TD><asp:Label id="Label1" runat="server" Text="Updated as on" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><asp:TextBox id="txtUpdateAsOn" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox><asp:ImageButton id="imgbtnUpdateAsOn" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVUpdateAsOn" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" ControlExtender="MEUpdateAsOn" ControlToValidate="txtUpdateAsOn" Display="Dynamic"></cc1:MaskedEditValidator></TD><TD><SELECT style="WIDTH: 170px" id="ddlSeasoncode" class="field_input" tabIndex=20 onchange="CallWebMethod('seascode')" runat="server" visible="false"> <OPTION selected></OPTION></SELECT></TD><TD><SELECT style="WIDTH: 237px" id="ddlSeasonName" class="field_input" tabIndex=21 onchange="CallWebMethod('seasname')" runat="server" visible="false"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4>
    <asp:Button id="BtnClear" tabIndex=26 runat="server" Text="Clear" 
        CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="BtnPrint" tabIndex=25 runat="server" Text="Load Report" CssClass="field_button"></asp:Button>&nbsp; 
    <asp:Button id="btnhelp" tabIndex=27 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="field_button"></asp:Button>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
            height: 9px" type="text" /></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDate" TargetControlID="txtToDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEUpdateAsOn" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgbtnUpdateAsOn" TargetControlID="txtUpdateAsOn"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEUpdateAsOn" runat="server" TargetControlID="txtUpdateAsOn" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> 
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            &nbsp; &nbsp;
                        </td>
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
                <CR:CrystalReportViewer ID="CRVSupplierPolicies" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;
    <br />

</asp:Content>