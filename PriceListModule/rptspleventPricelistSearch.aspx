<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptspleventPricelistSearch.aspx.vb" Inherits="rptspleventPricelistSearch"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"    TagPrefix="ews" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <%--<script language="javascript" src="js\date-picker.js"></script>  --%>
    <%--<script language="javascript" src="js\datefun.js"></script>--%>
   <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>


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
                document.getElementById("<%=hdnsellingtypecode.ClientID%>").value = sptype; 
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,sptype,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,sptype,FillCatNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,sptype,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,sptype,FillSellCatNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,null,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,null,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);

             

             break;
           
           case "sptypename":
                var select=document.getElementById("<%=ddlSpTypeName.ClientID%>");
                var sptype=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsellingtypecode.ClientID%>").value = selectname.value; 
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetCatCodeListnew(constr,sptype,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,sptype,FillCatNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,sptype,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,sptype,FillSellCatNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,null,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,null,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);

                
                break;
                
        case "catcode":
                var select=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCatName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                document.getElementById("<%=hdncategory.ClientID%>").value = cat;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;




                constr=connstr.value   

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
        case "catname":
                var select=document.getElementById("<%=ddlCatName.ClientID%>");                
                var cat=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncategory.ClientID%>").value = selectname.value; 
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value
                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;
                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);
                
                break;
       
         case "ctrycode":
         
                var select=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlcontName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnctrycode.ClientID%>").value = ctry; 
                
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
                document.getElementById("<%=hdnctrycode.ClientID%>").value = selectname.value; 
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
                document.getElementById("<%=hdncitycode.ClientID%>").value = city; 
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
               
                var selectcatcode=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=selectcatcode.options[selectcatcode.selectedIndex].text;
               
                var selectsellcatcode=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=selectsellcatcode.options[selectsellcatcode.selectedIndex].text;                       

                var selectcountry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectcountry.options[selectcountry.selectedIndex].text;

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,sellcat,ctry,city,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,sellcat,ctry,city,FillSupplierNames,ErrorHandler,TimeOutHandler);

                break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var city=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncitycode.ClientID%>").value = selectname.value; 
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
            case "suppliercode":
                var select=document.getElementById("<%=ddlPartyCode.ClientID%>"); 
                var party=select.options[select.selectedIndex].text;               
                var selectname=document.getElementById("<%=ddlPartyName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value =party; 
                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>"); 
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                
                break;
            case "suppliername":
                var select=document.getElementById("<%=ddlPartyName.ClientID%>");                
                var party=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlPartyCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value = selectname.value; 
                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;

                

                break;       

            case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var plgrp=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSellCodeListnew(constr,plgrp,FillSellCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellNameListnew(constr,plgrp,FillSellNames,ErrorHandler,TimeOutHandler);

                break; 

            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var plgrp=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSellCodeListnew(constr,plgrp,FillSellCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellNameListnew(constr,plgrp,FillSellNames,ErrorHandler,TimeOutHandler);

                break;

         case "sellcode":
                var select=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=select.options[select.selectedIndex].text;                       
                var selectname=document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsellingtypecode.ClientID%>").value = selectname.value; 
                break;
          case "sellname":
                var select=document.getElementById("<%=ddlSellingName.ClientID%>");      
                var sellcat=select.options[select.selectedIndex].value;                                                 
                var selectname=document.getElementById("<%=ddlSellingCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsellingtypecode.ClientID%>").value = selectname.value; 
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



function FillSeasdate(result)
    {
        alert ('test');
        var d1 =document.getElementById("<%=txtremarks.ClientID%>");
        d1.value = result[0].ListText;
      
        alert ('test1');
        alert(d1.value);

        window.opener.document.getElementById('ctl00_Main_dtpFromDate').txtDate.Text=d1.value;
    window.close();

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


    function ddlPartyCode_onclick() {

    }

</script> 

    <table>
        <tr>
            <td style="width: 100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Special Events
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
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="lblSupplierTypeCode" runat="server" Text="Supplier Type Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 200px" id="ddlSPTypeCode" class="drpdown" tabIndex=1 onchange="CallWebMethod('sptype')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblsuppliertypename" runat="server" Text="Supplier Type Name" CssClass="td_cell"></asp:Label></TD>
<TD><SELECT style="WIDTH: 238px" id="ddlSpTypeName" class="drpdown" tabIndex=2 onchange="CallWebMethod('sptypename')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblCountryCode" runat="server" Text="Country Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px">
<SELECT style="WIDTH: 200px" id="ddlContCode" class="drpdown" tabIndex=3 onchange="CallWebMethod('ctrycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD>
<TD><asp:Label id="lblcountryname" runat="server" Text="Country Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlcontName" class="drpdown" tabIndex=4 onchange="CallWebMethod('ctryname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblCityCode" runat="server" Text="City Code" CssClass="td_cell"></asp:Label></TD>
<TD style="WIDTH: 176px"><SELECT style="WIDTH: 200px" id="ddlCityCode" class="drpdown" tabIndex=5 onchange="CallWebMethod('citycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcityname" runat="server" Text="City Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 117px" id="ddlCityName" class="drpdown" tabIndex=6 onchange="CallWebMethod('cityname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> &nbsp;</TD></TR>
<TR><TD><asp:Label id="lblCategorycode" runat="server" Text="Category Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 200px" id="ddlCCode" class="drpdown" tabIndex=8 onchange="CallWebMethod('catcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblCategoryName" runat="server" Text="Category Name" CssClass="td_cell"></asp:Label></TD>
<TD><SELECT style="WIDTH: 237px" id="ddlCatName" class="drpdown" tabIndex=9 onchange="CallWebMethod('catname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblSupplierCode" runat="server" Text="Supplier Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 200px" id="ddlPartyCode" class="drpdown" tabIndex=10 onchange="CallWebMethod('suppliercode')" runat="server" Visible="true" onclick="return ddlPartyCode_onclick()">
 <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblSupplierName" runat="server" Text="Supplier Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlPartyName" class="drpdown" tabIndex=11 onchange="CallWebMethod('suppliername')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR>
 <TR><TD><asp:Label id="lblMarketCode" runat="server" Text="Market Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 200px" id="ddlMarketCode" class="drpdown" tabIndex=14 onchange="CallWebMethod('marketcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblMarketName" runat="server" Text="Market Name" CssClass="td_cell"></asp:Label></TD>
 <TD><SELECT style="WIDTH: 237px" id="ddlMarketName" class="drpdown" tabIndex=15 onchange="CallWebMethod('marketname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 99px"><asp:Label id="lblSellingCategoryCode" runat="server" Text="Selling Type Code" CssClass="td_cell" Width="123px"></asp:Label></TD><TD style="WIDTH: 176px"><SELECT style="WIDTH: 200px" id="ddlSellingCode" class="drpdown" tabIndex=16 onchange="CallWebMethod('sellcode')" runat="server" Visible="true"> 
 <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblsellingcategoryname" runat="server" Text="Selling Type Name" CssClass="td_cell" Width="124px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlSellingName" class="drpdown" tabIndex=17 onchange="CallWebMethod('sellname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD colSpan=4>
 <asp:Label id="Label8" runat="server" Text="( * Leave Selling Type Code Field as Blank to Print Net Cost)" ForeColor="#C00000" CssClass="td_cell" Width="664px"></asp:Label></TD></TR><TR><TD><asp:Label id="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD>
 <TD style="WIDTH: 176px"><asp:TextBox id="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>
  <cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR>
  <TR><TD><asp:Label id="Label6" runat="server" Text="Sell Type" CssClass="td_cell"></asp:Label></TD><TD><asp:RadioButton id="rbbeach" tabIndex=22 runat="server" Text="Beach" ForeColor="Black" CssClass="search_button" Width="60px" AutoPostBack="True" BorderColor="#404040" GroupName="GrSearch" wfdid="w6"></asp:RadioButton>&nbsp;<asp:RadioButton id="rbcity" tabIndex=23 runat="server" Text="City" ForeColor="Black" CssClass="search_button" Width="60px" AutoPostBack="True" BorderColor="#404040" GroupName="GrSearch"></asp:RadioButton> <asp:RadioButton id="rball" tabIndex=23 runat="server" Text="All" ForeColor="Black" CssClass="search_button" Width="60px" AutoPostBack="True" BorderColor="#404040" GroupName="GrSearch" Checked="True"></asp:RadioButton></TD>
  <TD></TD><TD></TD>
  </TR>
  <tr>
  <TD><asp:Label id="lblatatus" runat="server" 
            Text="Approval Status" CssClass="td_cell" Width="103px"></asp:Label></TD>
            <TD>
            <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="drpdown" 
                Width="191px">
                <asp:ListItem Value="1">Approved</asp:ListItem>
                <asp:ListItem Value="0">Unapproved</asp:ListItem>
                <asp:ListItem Value="2">All</asp:ListItem>
            </asp:DropDownList>
            </TD>
  
  
  </tr>



  <TR><TD><asp:Label id="Label1" runat="server" Text="Updated as on" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 176px"><asp:TextBox id="txtUpdateAsOn" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="imgbtnUpdateAsOn" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVUpdateAsOn" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtUpdateAsOn" ControlExtender="MEUpdaeAsOn" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD></TD><TD>&nbsp;&nbsp;</TD></TR><TR><TD><asp:Label id="Label7" runat="server" Text="Remarks" CssClass="td_cell" Visible="False"></asp:Label></TD><TD colSpan=3><asp:TextBox id="txtremarks" tabIndex=27 runat="server" Height="40px" CssClass="td_cell" Visible="False" Width="532px" TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4>
  <asp:Button id="BtnClear" tabIndex=29 runat="server" Text="Clear" CssClass="btn"></asp:Button> &nbsp;
  <asp:Button id="BtnPrint" tabIndex=28 runat="server" Text="Load Report" CssClass="btn"></asp:Button>&nbsp; 
    <asp:Button id="btnhelp" tabIndex=30 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="btn"></asp:Button>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> 
        <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> 
        <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
         <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEUpdateAsOn" runat="server" TargetControlID="txtUpdateAsOn" PopupButtonID="imgbtnUpdateAsOn" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEUpdaeAsOn" runat="server" TargetControlID="txtUpdateAsOn" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> 
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
<asp:ServiceReference Path="../clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
                <CR:CrystalReportViewer ID="CRVSupplierPolicies" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="images/crystaltoolbar/" />
    &nbsp;&nbsp;&nbsp;
    <select id="ddlSeasoncode" runat="server" class="drpdown" onchange="CallWebMethod('seascode')"
        style="width: 170px" tabindex="18" visible="false">
        <option selected="selected"></option>
    </select>
    <select id="ddlSeasonName" runat="server" class="drpdown" onchange="CallWebMethod('seasname')"
        style="width: 237px" tabindex="19" visible="false">
        <option selected="selected"></option>
    </select>
    <br />
     <asp:HiddenField ID="hdnsellingtypecode" runat="server"/>
     <asp:HiddenField ID="hdnsptypecode" runat="server"/>
      <asp:HiddenField ID="hdncategory" runat="server"/>
      <asp:HiddenField ID="hdnsellcatcode" runat="server"/>
       <asp:HiddenField ID="hdnctrycode" runat="server"/>
       <asp:HiddenField ID="hdncitycode" runat="server"/>
       <asp:HiddenField ID="hdnsuppliercode" runat="server"/>

</asp:Content>