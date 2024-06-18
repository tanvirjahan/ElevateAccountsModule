<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptSupplierpoliciesSearch.aspx.vb" Inherits="rptSupplierpoliciesSearch"  MasterPageFile="~/PricelistMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%><%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>
   <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>

<script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
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
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsptypecode.ClientID%>").value = sptype;      
                
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
                document.getElementById("<%=hdnsptypecode.ClientID%>").value = sptype;   
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
                document.getElementById("<%=hdncategory.ClientID%>").value = cat;
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
                document.getElementById("<%=hdncategory.ClientID%>").value = cat;
                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);
                
                break;
         case "sellcatcode":
                var select=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=select.options[select.selectedIndex].text;                       
                var selectname=document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsellcatcode.ClientID%>").value = sellcat;
               


                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;

                var selectcatcode=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=selectcatcode.options[selectcatcode.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, sellcat, null, null, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, sellcat, null, null, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
          case "sellcatname":
                var select=document.getElementById("<%=ddlSellingName.ClientID%>");      
                var sellcat=select.options[select.selectedIndex].value;                                                 
                var selectname=document.getElementById("<%=ddlSellingCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsellcatcode.ClientID%>").value = sellcat;
                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                
                var selectcatcode=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=selectcatcode.options[selectcatcode.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, scatcat, null, null, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, scatcat, null, null, FillSupplierNames, ErrorHandler, TimeOutHandler);

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
            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var city=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncitycode.ClientID%>").value = null;
                document.getElementById("<%=hdncitycode.ClientID%>").value = city;

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
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncitycode.ClientID%>").value = null;
                document.getElementById("<%=hdncitycode.ClientID%>").value = city;
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
                var select = document.getElementById("<%=ddlPartyCode.ClientID%>");
                var party = select.options[select.selectedIndex].text;            
                var selectname=document.getElementById("<%=ddlPartyName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value = party;
                var txtcode = document.getElementById("<%=txtSupplierCode.ClientID%>");
                txtcode.value = party;

                var txtname = document.getElementById("<%=txtSupplierName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].value;


                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var ddlcontName = document.getElementById("<%=ddlcontName.ClientID%>");
                var ctry = ddlcontName.options[ddlcontName.selectedIndex].value;

                ColServices.clsServices.GetSellingCategoryCodeBySupplier(constr, party, FillSellingCategoryCodeAndName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCatCityCode(constr, txtcode.value, FillCatCity, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, party, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, party, FillRoomTypeNames, ErrorHandler, TimeOutHandler);

                break;

            case "suppliername":
                var select = document.getElementById("<%=ddlPartyName.ClientID%>");
                var party = select.options[select.selectedIndex].value;                               
                var selectname=document.getElementById("<%=ddlPartyCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value = party;
                var txtname = document.getElementById("<%=txtSupplierName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].value;


                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var ddlcontName = document.getElementById("<%=ddlcontName.ClientID%>");
                var ctry = ddlcontName.options[ddlcontName.selectedIndex].value;

                ColServices.clsServices.GetSellingCategoryCodeBySupplier(constr, party, FillSellingCategoryCodeAndName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCatCityCode(constr, party, FillCatCity, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, party, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, party, FillRoomTypeNames, ErrorHandler, TimeOutHandler);


                break;
            case "rmtypcode":
                var select = document.getElementById("<%=ddlRmtypeCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlRmtypename.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnroomtypecode.ClientID%>").value = selectname.value;
                var txtcode = document.getElementById("<%=txtRoomTypeCode.ClientID%>");
                txtcode.value = select.options[select.selectedIndex].value;

                var txtname = document.getElementById("<%=txtRoomTypeName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].value;


                break;

            case "rmtypname":
                var select = document.getElementById("<%=ddlRmtypename.ClientID%>");
                var selectname = document.getElementById("<%=ddlRmtypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnroomtypecode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                var txtcode = document.getElementById("<%=txtRoomTypeCode.ClientID%>");
                txtcode.value = selectname.options[selectname.selectedIndex].value;

                var txtname = document.getElementById("<%=txtRoomTypeName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].text;

                break;
            case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnmarketcode.ClientID%>").value = selectname.value;
                break; 
            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnmarketcode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                break;

        }
        }

function FillSellingCategoryCodeAndName(result) {
    var ddlscatcode = document.getElementById("<%=ddlSellingCode.ClientID%>");
    var ddlscatname = document.getElementById("<%=ddlSellingName.ClientID%>");

//    document.getElementById("<%=hdnsellcatcode.ClientID%>").value = result[0].ListValue;

    
    for (var i = 0; i < ddlscatcode.length - 1; i++) {
        if (ddlscatcode.options[i].text == result) {
            ddlscatcode.selectedIndex = i;
            ddlscatname.selectedIndex = i;
            break;
        }
    }
}

function FillCatCity(result) {
    var ddlCityName = document.getElementById("<%=ddlCityName.ClientID%>");
    var ddlCityCode = document.getElementById("<%=ddlCityCode.ClientID%>");
    var ddlCCode = document.getElementById("<%=ddlCCode.ClientID%>");
    var ddlCatName = document.getElementById("<%=ddlCatName.ClientID%>");
    document.getElementById("<%=hdncitycode.ClientID%>").value = null;
   
    
   
    var txtcitycode = document.getElementById("<%=txtCityCode.ClientID%>");
    var txtCityName = document.getElementById("<%=txtCityName.ClientID%>");

    ddlCityName.value = result[0].ListValue;
    ddlCatName.value = result[0].ListText;
    document.getElementById("<%=hdncitycode.ClientID%>").value = result[0].ListValue;
   

    for (var i = 0; i < ddlCityCode.length - 1; i++) {
        if (ddlCityCode.options[i].text == result[0].ListValue) {
            ddlCityCode.selectedIndex = i;
            break;
        }
    }
    for (var i = 0; i < ddlCCode.length - 1; i++) {
        if (ddlCCode.options[i].text == result[0].ListText) {
            ddlCCode.selectedIndex = i;
            break;
        }
    }

    txtcitycode.value = ddlCityName.value;
    txtCityName.value = ddlCityCode.value;

    //        ddlCityCode.value = ddlCityName.options[ddlCityName.selectedIndex].text;
    //        ddlCCode.value = ddlCatName.options[ddlCatName.selectedIndex].text;

}

function FillRoomTypeCodes(result) {
    var ddl = document.getElementById("<%=ddlRmtypeCode.ClientID%>");
    RemoveAll(ddl)
    for (var i = 0; i < result.length; i++) {
        var option = new Option(result[i].ListText, result[i].ListValue);
        ddl.options.add(option);
    }
    ddl.value = "[Select]";
}
function FillRoomTypeNames(result) {
    var ddl = document.getElementById("<%=ddlRmtypename.ClientID%>");
    RemoveAll(ddl)
    for (var i = 0; i < result.length; i++) {
        var option = new Option(result[i].ListText, result[i].ListValue);
        ddl.options.add(option);
    }
    ddl.value = "[Select]";
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
            <td style="width: 100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Supplier Policies Report</td>
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
<TABLE style="WIDTH: 85%"><TBODY><TR><TD style="WIDTH: 100px"><asp:Label id="lblSupplierTypeCode" runat="server" Text="Supplier Type Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 223px" id="ddlSPTypeCode" class="drpdown" tabIndex=1 onchange="CallWebMethod('sptype')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px"><asp:Label id="lblsuppliertypename" runat="server" Text="Supplier Type Name" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 238px" id="ddlSpTypeName" class="drpdown" tabIndex=2 onchange="CallWebMethod('sptypename')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblCountryCode" runat="server" Text="Country Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 223px" id="ddlContCode" class="drpdown" tabIndex=3 onchange="CallWebMethod('ctrycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px"><asp:Label id="lblcountryname" runat="server" Text="Country Name" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 237px" id="ddlcontName" class="drpdown" tabIndex=4 onchange="CallWebMethod('ctryname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblCityCode" runat="server" Text="City Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 223px" id="ddlCityCode" class="drpdown" tabIndex=5 onchange="CallWebMethod('citycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px"><asp:Label id="lblcityname" runat="server" Text="City Name" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 237px" id="ddlCityName" class="drpdown" tabIndex=6 onchange="CallWebMethod('cityname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblCategorycode" runat="server" Text="Category Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 223px" id="ddlCCode" class="drpdown" tabIndex=7 onchange="CallWebMethod('catcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px"><asp:Label id="lblCategoryName" runat="server" Text="Category Name" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 237px" id="ddlCatName" class="drpdown" tabIndex=8 onchange="CallWebMethod('catname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblSellingCategoryCode" runat="server" Text="Selling Category Code" CssClass="td_cell" Width="123px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 223px" id="ddlSellingCode" class="drpdown" tabIndex=9 onchange="CallWebMethod('sellcatcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px"><asp:Label id="lblsellingcategoryname" runat="server" Text="Selling Category Name" CssClass="td_cell" Width="124px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 237px" id="ddlSellingName" class="drpdown" tabIndex=10 onchange="CallWebMethod('sellcatname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblSupplierCode" runat="server" Text="Supplier Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 223px" id="ddlPartyCode" class="drpdown" tabIndex=11 onchange="CallWebMethod('suppliercode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px"><asp:Label id="lblSupplierName" runat="server" Text="Supplier Name" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 237px" id="ddlPartyName" class="drpdown" tabIndex=12 onchange="CallWebMethod('suppliername')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px">
    <asp:Label id="Label2" runat="server" Text="Room Type Code" CssClass="td_cell"></asp:Label></TD><TD style="WIDTH: 100px">
        <select id="ddlRmtypeCode" runat="server" class="drpdown" name="D1" 
            onchange="CallWebMethod('rmtypcode')" style="WIDTH: 223px" tabindex="12" 
            visible="true">
            <option selected=""></option>
        </select>
    </TD><TD style="WIDTH: 100px">
        <asp:Label ID="Label3" runat="server" CssClass="td_cell" Text="Room Type Name" 
            Width="120px"></asp:Label>
    </TD><TD style="WIDTH: 100px">
        <select id="ddlRmtypename" runat="server" class="drpdown" name="D2" 
            onchange="CallWebMethod('rmtypname')" style="WIDTH: 237px" tabindex="13" 
            visible="true">
            <option selected=""></option>
        </select>
    </TD></TR><TR><TD style="WIDTH: 100px"><asp:Label id="lblMarketCode" runat="server" 
            Text="Market Code" CssClass="td_cell"></asp:Label></TD>
        <TD style="WIDTH: 100px">
            <select id="ddlMarketCode" runat="server" class="drpdown" 
                onchange="CallWebMethod('marketcode')" style="WIDTH: 223px" tabindex="13" 
                visible="true">
                <option selected=""></option>
            </select>
        </TD><TD style="WIDTH: 100px"><asp:Label id="lblMarketName" runat="server" 
                Text="Market Name" CssClass="td_cell"></asp:Label></TD>
        <TD style="WIDTH: 100px">
            <select id="ddlMarketName" runat="server" class="drpdown" 
                onchange="CallWebMethod('marketname')" style="WIDTH: 237px" tabindex="14" 
                visible="true">
                <option selected=""></option>
            </select>
        </TD></TR><TR>
    <TD>
        <asp:Label ID="lblFromDate" runat="server" CssClass="td_cell" Text="From Date"></asp:Label>
    </TD><TD>
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" 
                ControlExtender="MEFromDate" ControlToValidate="txtFromDate" 
                CssClass="field_error" Display="Dynamic" 
                EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format">
            </cc1:MaskedEditValidator>
    </TD><TD>
            <asp:Label ID="lblTodate" runat="server" CssClass="td_cell" Text="To Date"></asp:Label>
        </TD><TD>
            <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
            <asp:ImageButton ID="ImgBtnToDate" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            <cc1:MaskedEditValidator ID="MEVToDate" runat="server" 
                ControlExtender="METoDate" ControlToValidate="txtToDate" CssClass="field_error" 
                Display="Dynamic" EmptyValueBlurredText="Date is required" 
                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" 
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format">
            </cc1:MaskedEditValidator>
        </TD></TR><TR><TD>
    <asp:Label ID="lblstatus" runat="server" CssClass="td_cell" Text="Status"></asp:Label>
    </TD>
    <td>
        <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="drpdown" 
            Width="191px">
            <asp:ListItem Value="1">Approved</asp:ListItem>
            <asp:ListItem Value="0">Unapproved</asp:ListItem>
            <asp:ListItem Value="2">All</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td colspan="2">
        <asp:CheckBox ID="chkproviderfilter" runat="server" CssClass="chkbox" 
            Text="Show Selected Providers for price list only " Width="250px" />
        </td>
    </TR>
    <tr>
        <td style="WIDTH: 100px">
            <asp:Label ID="Label1" runat="server" CssClass="td_cell" Text="Updated as on"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtUpdateAsOn" runat="server" CssClass="fiel_input" 
                Width="80px"></asp:TextBox>
            <asp:ImageButton ID="imgbtnUpdateAsOn" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" />
            <cc1:MaskedEditValidator ID="MEVUpdatedAsOn" runat="server" 
                ControlExtender="MEUpdatedAsOn" ControlToValidate="txtUpdateAsOn" 
                CssClass="field_error" Display="Dynamic" 
                EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format">
            </cc1:MaskedEditValidator>
        </td>
        <td style="WIDTH: 100px">
            <asp:Label ID="lblpolicyselect" runat="server" CssClass="td_cell" 
                Text="Policy Select"></asp:Label>
        </td>
        <td>
            <asp:RadioButton ID="rball" runat="server" AutoPostBack="True" 
                BorderColor="#404040" Checked="True" CssClass="search_button" ForeColor="Black" 
                GroupName="GrSearch" tabIndex="18" Text="All" wfdid="w6" Width="110px" />
            <asp:RadioButton ID="rbparticular" runat="server" AutoPostBack="True" 
                BorderColor="#404040" CssClass="search_button" ForeColor="Black" 
                GroupName="GrSearch" tabIndex="19" Text="Particular" Width="110px" />
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="4">
            <table style="WIDTH: 100%">
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lblremarks" runat="server" CssClass="td_cell" Text="Remarks" 
                                Visible="False"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbpromotion" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" 
                                tabIndex="22" Text="Promotional Rates " Visible="False" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbpromotionrmks" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="22" 
                                Text="Promotion Remarks" Visible="False" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbmaxaccomadation" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="22" 
                                Text="Maximum accomodation" Visible="False" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbspecialevent" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="28" 
                                Text="Special Event Remarks" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbminimumnights" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="27" 
                                Text="Minimum Nights" Visible="False" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbblocksales" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="26" 
                                Text="Block Sales" Visible="False" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbcompulsory" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="24" 
                                Text="Compulsory Remarks" Visible="False" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbchild" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="21" 
                                Text="Child Policy" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbcancellation" runat="server" AutoPostBack="True" 
                                Checked="True" CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" 
                                tabIndex="20" Text="Cancellation Policy" Visible="False" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbremarks" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="25" 
                                Text="General Remarks" Visible="False" />
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:RadioButton ID="rbearlybird" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="23" 
                                Text="Early Bird Promotion" Visible="False" />
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="rbpromotionandearly" runat="server" AutoPostBack="True" 
                                CssClass="td_cell" ForeColor="Black" GroupName="RemarkSearch" tabIndex="29" 
                                Text="Promotion &amp; Early Bird Promotions" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="TEXT-ALIGN: center">
                            <asp:Button ID="BtnClear" runat="server" CssClass="btn" tabIndex="30" 
                                Text="Clear" />
                            &nbsp;
                            <asp:Button ID="BtnPrint" runat="server" CssClass="btn" tabIndex="31" 
                                Text="Load Report" />
                            &nbsp;
                            <asp:Button ID="btnhelp" runat="server" CssClass="btn" onclick="btnhelp_Click" 
                                tabIndex="32" Text="Help" Width="46px" />
                            &nbsp;
                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                            height: 9px" type="text" />
                            <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSupplierCode" 
                                type=text runat="server" />
                            <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSupplierName" 
                                type=text runat="server" />
                            <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtRoomTypeCode" 
                                type=text runat="server" />
                            <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtRoomTypeName" 
                                type=text runat="server" />
                            <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtCityCode" 
                                type=text runat="server" />
                            <INPUT style="VISIBILITY: hidden;WIDTH: 9px; HEIGHT: 3px" id="txtCityName" 
                                type=text runat="server" />
                            <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSellingTypeCode" 
                                type=text runat="server" />
                            <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSellingTypeName" 
                                type=text runat="server" />
                        </td>
                         <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
         <asp:Button id="Button1" tabIndex=16 runat="server" 
        CssClass="field_button"></asp:Button></td>

<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>

                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    </TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEUpdatedAsOn" runat="server" TargetControlID="txtUpdateAsOn" PopupButtonID="imgbtnUpdateAsOn" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEUpdatedAsOn" runat="server" TargetControlID="txtUpdateAsOn" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> 
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                </table>
                </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
                <CR:CrystalReportViewer ID="CRVSupplierPolicies" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" />
      <asp:HiddenField ID="hdnsptypecode" runat="server"/>
      <asp:HiddenField ID="hdncategory" runat="server"/>
      <asp:HiddenField ID="hdnsellcatcode" runat="server"/>
       <asp:HiddenField ID="hdnctrycode" runat="server"/>
       <asp:HiddenField ID="hdncitycode" runat="server"/>
       <asp:HiddenField ID="hdnsuppliercode" runat="server"/>
       <asp:HiddenField ID="hdnroomtypecode" runat="server"/>
       <asp:HiddenField ID="hdnmarketcode" runat="server"/>
     
</asp:Content>