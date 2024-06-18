<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptPriceExpirySearch.aspx.vb" Inherits="rptPriceExpirySearch"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %>  

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%><%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>
   <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>

<script type="text/javascript">
<!--
// WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
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
                document.getElementById("<%=hdnsptypecode.ClientID%>").value = sptype;    
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCatCodeListnew(constr, sptype, FillCatCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr, sptype, FillCatNames, ErrorHandler, TimeOutHandler);

                ColServices.clsServices.GetSellCatCodeListnew(constr, sptype, FillSellCategoryCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCategoryNames, ErrorHandler, TimeOutHandler);


                var select = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, null, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, null, FillRoomTypeNames, ErrorHandler, TimeOutHandler);

                      
                break;
           case "sptypename":
                var select=document.getElementById("<%=ddlSpTypeName.ClientID%>");
                var sptype=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsptypecode.ClientID%>").value = sptype;   
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCatCodeListnew(constr, sptype, FillCatCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr, sptype, FillCatNames, ErrorHandler, TimeOutHandler);

                ColServices.clsServices.GetSellCatCodeListnew(constr, sptype, FillSellCategoryCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCategoryNames, ErrorHandler, TimeOutHandler);


                var select = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, null, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, null, FillRoomTypeNames, ErrorHandler, TimeOutHandler);

                break;

            case "ctrycode":

                var select = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlcontName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnctrycode.ClientID%>").value = ctry;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCityCodeListnew(constr, ctry, FillCityCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr, ctry, FillCityNames, ErrorHandler, TimeOutHandler);

                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;

            case "ctryname":
                var select = document.getElementById("<%=ddlcontName.ClientID%>");
                var ctry = select.options[select.selectedIndex].value;

                var selectname = document.getElementById("<%=ddlContCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnctrycode.ClientID%>").value = ctry;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCityCodeListnew(constr, ctry, FillCityCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr, ctry, FillCityNames, ErrorHandler, TimeOutHandler);

                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var city=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncitycode.ClientID%>").value = null;
                document.getElementById("<%=hdncitycode.ClientID%>").value = city;
                var txtcode = document.getElementById("<%=txtCityCode.ClientID%>");
                txtcode.value = city;

                var txtname = document.getElementById("<%=txtCityName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].value;

                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);
                
                break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var city=select.options[select.selectedIndex].value;
                 var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncitycode.ClientID%>").value = null;
                document.getElementById("<%=hdncitycode.ClientID%>").value = city;
                var txtcode = document.getElementById("<%=txtCityCode.ClientID%>");
                txtcode.value = city;

                var txtname = document.getElementById("<%=txtCityName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].text;


                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
            case "catcode":
                var select = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCatName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncategory.ClientID%>").value = cat;

                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
            case "catname":
                var select = document.getElementById("<%=ddlCatName.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlCCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncategory.ClientID%>").value = cat;
                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
            case "scatcode":
                var select = document.getElementById("<%=ddlscatcode.ClientID%>");
                var scatcode = select.options[select.selectedIndex].text;

                var selectname = document.getElementById("<%=ddlscatname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsellcatcode.ClientID%>").value = scatcode;

                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, scatcode, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, scatcode, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                // ColServices.clsServices.GetSellCatCodeListnew(constr, sptype,  FillSellCategoryCodes, ErrorHandler, TimeOutHandler);
                // ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCategoryNames, ErrorHandler, TimeOutHandler);


                break;
            case "scatname":

                var select = document.getElementById("<%=ddlscatname.ClientID%>");
                var scatcode = select.value;

                var selectname = document.getElementById("<%=ddlscatcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsellcatcode.ClientID%>").value = scatcode;
                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                var city = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, scatcode, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, scatcode, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;

            case "suppliercode":
                var select=document.getElementById("<%=ddlPartyCode.ClientID%>");        
                var selectname=document.getElementById("<%=ddlPartyName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value = selectname.value;
                var partycode = select.options[select.selectedIndex].text;

                var txtcode = document.getElementById("<%=txtSupplierCode.ClientID%>");
                txtcode.value = partycode;

                var txtname = document.getElementById("<%=txtSupplierName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].value;


                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var ddlcontName = document.getElementById("<%=ddlcontName.ClientID%>");
                var ctry = ddlcontName.options[ddlcontName.selectedIndex].value;

                ColServices.clsServices.GetSellingCategoryCodeBySupplier(constr, partycode, FillSellingCategoryCodeAndName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCatCityCode(constr, txtcode.value, FillCatCity, ErrorHandler, TimeOutHandler);

                break;
            case "suppliername":
                var select=document.getElementById("<%=ddlPartyName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlPartyCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                var partycode = select.options[select.selectedIndex].value;

                var txtcode = document.getElementById("<%=txtSupplierCode.ClientID%>");
                txtcode.value = partycode;

                var txtname = document.getElementById("<%=txtSupplierName.ClientID%>");
                txtname.value = select.options[select.selectedIndex].text;

                var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype = selectsptype.options[selectsptype.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var ddlcontName = document.getElementById("<%=ddlcontName.ClientID%>");
                var ctry = ddlcontName.options[ddlcontName.selectedIndex].value;

                ColServices.clsServices.GetSellingCategoryCodeBySupplier(constr, partycode, FillSellingCategoryCodeAndName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCatCityCode(constr, txtcode.value, FillCatCity, ErrorHandler, TimeOutHandler);

                break;       
            case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
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



        function FillCatCodes(result) {
            var ddl = document.getElementById("<%=ddlCCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillCatNames(result) {
            var ddl = document.getElementById("<%=ddlCatName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSellCategoryCodes(result) {
            var ddl = document.getElementById("<%=ddlscatcode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSellCategoryNames(result) {
            var ddl = document.getElementById("<%=ddlscatname.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillCityCodes(result) {
            var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillCityNames(result) {
            var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSupplierCodes(result) {
            var ddl = document.getElementById("<%=ddlPartyCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillSupplierNames(result) {
            var ddl = document.getElementById("<%=ddlPartyName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
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

        function FillSellingCategoryCodeAndName(result) {
            var ddlscatcode = document.getElementById("<%=ddlscatcode.ClientID%>");
            var ddlscatname = document.getElementById("<%=ddlscatname.ClientID%>");
            for (var i = 0; i < ddlscatcode.length - 1; i++) {
                if (ddlscatcode.options[i].text == result) {
                    ddlscatcode.selectedIndex = i;
                    ddlscatname.selectedIndex = i;
                    break;
                }
            }
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


//function checkTelephoneNumber(e)
//			{	    
//			    	
//				if ( (event.keyCode < 45 || event.keyCode > 57) )
//				{
//					return false;
//	            }   
//	         	
//			}
//function checkNumber(e)
//			{	    
//			    	
//				if ( (event.keyCode < 47 || event.keyCode > 57) )
//				{
//					return false;
//	            }   
//	         	
//			}
//function checkCharacter(e)
//			{	    
//			    if (event.keyCode == 32 || event.keyCode ==46)
//			        return;			
//				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
//				{
//					return false;
//	            }   
//	         	
//			}

</script> 

    <table>
        <tr>
            <td style="width: 100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Price Expiry Report</td>
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
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="lblSupplierTypeCode" runat="server" Text="Supplier Type Code" Width="133px" CssClass="td_cell"></asp:Label></TD>
<TD style="width: 121px"><SELECT style="WIDTH: 170px" id="ddlSPTypeCode" class="drpdown" tabIndex=1 onchange="CallWebMethod('sptype')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD>
<TD><asp:Label id="lblsuppliertypename" runat="server" Text="Supplier Type Name" Width="150px" CssClass="td_cell"></asp:Label></TD>
<TD><SELECT style="WIDTH: 238px" id="ddlSpTypeName" class="drpdown" tabIndex=2 onchange="CallWebMethod('sptypename')" runat="server" Visible="true"> 
<OPTION selected></OPTION></SELECT></TD></TR><TR><TD>
    <asp:Label id="lblctrycode" 
        runat="server" Text="Country Code" CssClass="td_cell"></asp:Label></TD>
<TD style="width: 121px">
    <select id="ddlContCode" runat="server" class="drpdown" name="D1" 
        onchange="CallWebMethod('ctrycode')" style="WIDTH: 170px" tabindex="3" 
        visible="true">
        <option selected=""></option>
    </select>
    </TD>
<TD>
    <asp:Label ID="lblctryname" runat="server" CssClass="td_cell" 
        Text="Country Code"></asp:Label>
    </TD><TD>
        <select id="ddlcontName" runat="server" class="drpdown" name="D2" 
            onchange="CallWebMethod('ctryname')" style="WIDTH: 237px" tabindex="4" 
            visible="true">
            <option selected=""></option>
        </select>
    </TD></TR>
    <tr>
        <td>
            <asp:Label ID="lblCityCode" runat="server" CssClass="td_cell" Text="City Code"></asp:Label>
        </td>
        <td style="width: 121px">
            <select id="ddlCityCode" runat="server" class="drpdown" 
                onchange="CallWebMethod('citycode')" style="WIDTH: 170px" tabindex="5" 
                visible="True">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblcityname" runat="server" CssClass="td_cell" Text="City Name"></asp:Label>
        </td>
        <td>
            <select id="ddlCityName" runat="server" class="drpdown" 
                onchange="CallWebMethod('cityname')" style="WIDTH: 237px" tabindex="6" 
                visible="True">
                <option selected=""></option>
            </select>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCategorycode" runat="server" CssClass="td_cell" 
                Text="Category Code"></asp:Label>
        </td>
        <td style="width: 121px">
            <select id="ddlCCode" runat="server" class="drpdown" name="D3" 
                onchange="CallWebMethod('catcode')" style="WIDTH: 170px" tabindex="8" 
                visible="true">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblCategoryName" runat="server" CssClass="td_cell" 
                Text="Category Name"></asp:Label>
        </td>
        <td>
            <select id="ddlCatName" runat="server" class="drpdown" name="D4" 
                onchange="CallWebMethod('catname')" style="WIDTH: 237px" tabindex="9" 
                visible="true">
                <option selected=""></option>
            </select>
        </td>
    </tr>
    <TR><TD>
        <asp:Label id="lblSupSellingCode" runat="server" Text="Selling Category Code" 
            CssClass="td_cell"></asp:Label></TD>
    <TD style="width: 121px">
        <select ID="ddlscatcode" runat="server" class="drpdown" name="D5" 
            onchange="CallWebMethod('scatcode')" style="WIDTH: 170px" tabindex="10" 
            visible="true">
            <option selected=""></option>
        </select></TD><TD>
            <asp:Label ID="lblSupSellingName" runat="server" CssClass="td_cell" 
                Text="Selling Category Name"></asp:Label>
        </TD><TD>
            <select ID="ddlscatName" runat="server" class="drpdown" name="D6" 
                onchange="CallWebMethod('scatname')" style="WIDTH: 237px" tabindex="11" 
                visible="true">
                <option selected=""></option>
            </select></TD></TR>
    <tr>
        <td>
            <asp:Label ID="lblSupplierCode" runat="server" CssClass="td_cell" 
                Text="Supplier Code"></asp:Label>
        </td>
        <td style="width: 121px">
            <select id="ddlPartyCode" runat="server" class="drpdown" 
                onchange="CallWebMethod('suppliercode')" style="WIDTH: 170px" tabindex="11" 
                visible="True">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblSupplierName" runat="server" CssClass="td_cell" 
                Text="Supplier Name "></asp:Label>
        </td>
        <td>
            <select id="ddlPartyName" runat="server" class="drpdown" 
                onchange="CallWebMethod('suppliername')" style="WIDTH: 237px" tabindex="12" 
                visible="True">
                <option selected=""></option>
            </select>
        </td>
    </tr>
    <TR><TD>
    <asp:Label id="lblMarketCode" runat="server" Text="Market Code" 
        CssClass="td_cell"></asp:Label></TD>
    <TD style="width: 121px"><SELECT style="WIDTH: 170px" id="ddlMarketCode" 
            class="drpdown" tabIndex=17 onchange="CallWebMethod('marketcode')" 
            runat="server" Visible="True"> <OPTION selected></OPTION></SELECT></TD><TD>
    <asp:Label id="lblMarketName" runat="server" Text="Market Name" 
        CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" 
                id="ddlMarketName" class="drpdown" tabIndex=18 
                onchange="CallWebMethod('marketname')" runat="server" Visible="True"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD>
    <asp:Label ID="lblstatus" runat="server" CssClass="td_cell" Text="Status"></asp:Label>
    </TD>
    <TD style="width: 121px">
        <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="drpdown" 
            Width="170px">
            <asp:ListItem Value="1">Approve</asp:ListItem>
            <asp:ListItem Value="0">Unapprove</asp:ListItem>
            <asp:ListItem Value="2">All</asp:ListItem>
        </asp:DropDownList>
    </TD><TD>&nbsp;</TD><TD>&nbsp;</TD></TR><TR><TD><asp:Label id="Label1" runat="server" Text="Updated as on" CssClass="td_cell"></asp:Label></TD>
    <TD style="width: 200px">
        <asp:TextBox ID="dtpason" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
        <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" 
            PopupButtonID="ImgBtnFrmDt" TargetControlID="dtpason">
        </cc1:CalendarExtender>
        <cc1:MaskedEditExtender ID="MEFromDate" runat="server" Mask="99/99/9999" 
            MaskType="Date" TargetControlID="dtpason">
        </cc1:MaskedEditExtender>
        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
            ImageUrl="~/Images/Calendar_scheduleHS.png" />
        <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" 
            ControlExtender="MEFromDate" ControlToValidate="dtpason" 
            CssClass="field_error" Display="Dynamic" 
            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
            TooltipMessage="Input a date in dd/mm/yyyy format">
        </cc1:MaskedEditValidator>
    </TD><TD style="HEIGHT: 7px" colSpan=2><asp:CheckBox id="chkskip" tabIndex=22 runat="server" Text="Skip Room Type & Meal Plan" ForeColor="#0000C0" Width="214px" CssClass="td_cell"></asp:CheckBox></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4>
    <asp:Button id="BtnClear" tabIndex=24 runat="server" Text="Clear" 
        __designer:dtid="21955048183431186" CssClass="btn" __designer:wfdid="w40"></asp:Button>&nbsp;
<asp:Button id="BtnPrint" tabIndex=23 runat="server" Text="Load Report" __designer:dtid="21955048183431187" CssClass="btn" __designer:wfdid="w41"></asp:Button>&nbsp;
 <asp:Button id="btnhelp" tabIndex=25 onclick="btnhelp_Click" runat="server" 
        Text="Help" __designer:dtid="21955048183431188" CssClass="btn" 
        __designer:wfdid="w42"></asp:Button>
                   <INPUT style="VISIBILITY: hidden; WIDTH: 12px; HEIGHT: 9px" id="txtconnection" 
            type=text runat="server" /></TD></TR>
    <tr>
        <td colspan="4" style="TEXT-ALIGN: center">
                    <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtCityCode" type=text runat="server" />
        <INPUT style="VISIBILITY: hidden;WIDTH: 9px; HEIGHT: 3px" id="txtCityName" 
            type=text runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSellingTypeCode" 
            type=text runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSellingTypeName" 
            type=text runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSupplierCode" 
            type=text runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtSupplierName" 
            type=text runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtRoomTypeCode" 
            type=text runat="server" />
        <INPUT style="VISIBILITY: hidden; WIDTH: 9px; HEIGHT: 3px" id="txtRoomTypeName" 
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
    </TBODY></TABLE>
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
    <br />

     <asp:HiddenField ID="hdnsptypecode" runat="server"/>
      <asp:HiddenField ID="hdncategory" runat="server"/>
      <asp:HiddenField ID="hdnsellcatcode" runat="server"/>
       <asp:HiddenField ID="hdnctrycode" runat="server"/>
       <asp:HiddenField ID="hdncitycode" runat="server"/>
       <asp:HiddenField ID="hdnsuppliercode" runat="server"/>
       <asp:HiddenField ID="hdnroomtypecode" runat="server"/>
       <asp:HiddenField ID="hdnmarketcode" runat="server"/>




</asp:Content>