    <%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SuppAgentMain.aspx.vb" Inherits="SuppAgentMain"  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
  <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
  <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>



<script type="text/javascript" charset="utf-8">
    $(document).ready(function () {

        countryautoCompleteExtenderKeyUp();

        TypeNameAutoCompleteExtenderKeyUp();
        CategoryAutoCompleteExtenderKeyUp();
        CurrencyNameAutoCompleteExtenderKeyUp();
        CityNameAutoCompleteExtenderKeyUp();
        SectorNameAutoCompleteExtenderKeyUp();
        ControlAccAutoCompleteExtenderKeyUp();
    });

        </script>

<script language="javascript" type="text/javascript" >
function CallWebMethod(methodType)
    {
        switch(methodType)
        {
         case "sptype":
                var select=document.getElementById("<%=ddlType.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlTName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,codeid,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,codeid,FillCatNames,ErrorHandler,TimeOutHandler);
                
             break;
           
           case "sptypename":
                var select=document.getElementById("<%=ddlTName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlType.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,codeid,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,codeid,FillCatNames,ErrorHandler,TimeOutHandler);
                

                break;
        case "catcode":
                var select=document.getElementById("<%=ddlCCode.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCatName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        case "catname":
                var select=document.getElementById("<%=ddlCatName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
      
        case "currcode":
            var select = document.getElementById("<%=TextCurrencyCode.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCurrName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       
         case "currname":
                var select=document.getElementById("<%=ddlCurrName.ClientID%>");
                var selectname = document.getElementById("<%=TextCurrencyCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       
         case "ctrycode":
                var select=document.getElementById("<%=ddlContCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlcontName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSectorCodeListnew(constr,codeid,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,codeid,null,FillSectorNames,ErrorHandler,TimeOutHandler);                
 
                
                break;
            case "ctryname":
                var select=document.getElementById("<%=ddlcontName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlContCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
 
                ColServices.clsServices.GetSectorCodeListnew(constr,codeid,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,codeid,null,FillSectorNames,ErrorHandler,TimeOutHandler);                
 
                break;
            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var selectctry=document.getElementById("<%=ddlContCode.ClientID%>");
                var codeidctry=selectctry.options[selectctry.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSectorCodeListnew(constr,codeidctry,codeid,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,codeidctry,codeid,FillSectorNames,ErrorHandler,TimeOutHandler);                
                break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                var selectctry=document.getElementById("<%=ddlContCode.ClientID%>");
                var codeidctry=selectctry.options[selectctry.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSectorCodeListnew(constr,codeidctry,codeid,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,codeidctry,codeid,FillSectorNames,ErrorHandler,TimeOutHandler);                
                break; 
            case "sectorcode":
                var select=document.getElementById("<%=ddlSectorCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlSectorName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 
            case "sectorname":
                var select=document.getElementById("<%=ddlSectorName.ClientID%>");
                var selectname=document.getElementById("<%=ddlSectorCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break; 
                  case "ctrlcode":
                var select=document.getElementById("<%=cmbctrlcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=cmbctrlname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "ctrlname":
                var select=document.getElementById("<%=cmbctrlname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=cmbctrlcode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "accrualcode":
                var select=document.getElementById("<%=cmbaccrualcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=cmbaccrualname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "accrualname":
                var select=document.getElementById("<%=cmbaccrualname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=cmbaccrualcode.ClientID%>");
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
function FillSectorCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlSectorCode.ClientID%>");
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
			    if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }

}

function TypeNameautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=txtCode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=txtCode.ClientID%>').value = '';
    }

}
function CategoryNameautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=TextCode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=TextCode.ClientID%>').value = '';
    }

}

function CurrencyNameautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=TextCurrencyCode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=TextCurrencyCode.ClientID%>').value = '';
    }

}

function countryautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
    }
}


function Cityautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=txtcitycode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=txtcitycode.ClientID%>').value = '';
    }
}

function Sectorautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=txtsectorcode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=txtsectorcode.ClientID%>').value = '';
    }
}

function controlautocompleteselected(source, eventArgs) {
    if (eventArgs != null) {
        document.getElementById('<%=txtcontrolacccode.ClientID%>').value = eventArgs.get_value();
    }
    else {
        document.getElementById('<%=txtcontrolacccode.ClientID%>').value = '';
    }
}

function TypeNameAutoCompleteExtenderKeyUp() {
    $("#<%=TxtTypeName.ClientID %>").bind("change", function () {
        document.getElementById('<%=txtCode.ClientID%>').value = '';
    });
}
function CategoryAutoCompleteExtenderKeyUp() {
    $("#<%=TxtCategoryName.ClientID %>").bind("change", function () {
        document.getElementById('<%=textCode.ClientID%>').value = '';
    });
}
function CurrencyNameAutoCompleteExtenderKeyUp() {
    $("#<%=TxtCurrencyName.ClientID %>").bind("change", function () {
        document.getElementById('<%=TextCurrencyCode.ClientID%>').value = '';
    });
}
function countryautoCompleteExtenderKeyUp() {
    $("#<%=TxtCountryName.ClientID %>").bind("change", function () {
        document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
    });
}
function CityNameAutoCompleteExtenderKeyUp() {
    $("#<%=txtcityname.ClientID %>").bind("change", function () {
        document.getElementById('<%=txtcitycode.ClientID%>').value = '';
    });
}
function SectorNameAutoCompleteExtenderKeyUp() {
    $("#<%=txtsectorname.ClientID %>").bind("change", function () {
        document.getElementById('<%=txtsectorcode.ClientID%>').value = '';
    });
}

function ControlAccAutoCompleteExtenderKeyUp() {
    $("#<%=txtcontrolaccname.ClientID %>").bind("change", function () {
        document.getElementById('<%=txtcontrolacccode.ClientID%>').value = '';
    });
}
//function  GetValueFrom()
//{

//	var ddl = document.getElementById("<%=ddlTName.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlType.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}
//function  GetValueCode()
//{
//	var ddl = document.getElementById("<%=ddlType.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlTName.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}


//function  GetValueCategoryFrom()
//{

//	var ddl = document.getElementById("<%=ddlCatName.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlCCode.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}
//function  GetValueCategoryCode()
//{
//	var ddl = document.getElementById("<%=ddlCCode.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlCatName.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}

//function  GetValueSellingFrom()
//{



//function  GetValueCurrencyFrom()
//{

//	var ddl = document.getElementById("<%=ddlCurrName.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlCurrCode.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}
//function  GetValueCurrencyCode()
//{
//	var ddl = document.getElementById("<%=ddlCurrCode.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlCurrName.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}


//function  GetValueCountryFrom()
//{

//	var ddl = document.getElementById("<%=ddlcontName.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlContCode.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}
//function  GetValueCountryCode()
//{
//	var ddl = document.getElementById("<%=ddlContCode.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlcontName.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}

//function  GetValueCityFrom()
//{

//	var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlCityCode.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}
//function  GetValueCityCode()
//{
//	var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlCityName.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}

//function  GetValueSectorFrom()
//{

//	var ddl = document.getElementById("<%=ddlSectorName.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlSectorCode.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}
//function  GetValueSectorCode()
//{
//	var ddl = document.getElementById("<%=ddlSectorCode.ClientID%>");
//	ddl.selectedIndex = -1;
//		// Iterate through all dropdown items.
//		for (i = 0; i < ddl.options.length; i++) 
//		{
//			if (ddl.options[i].text == 
//			document.getElementById("<%=ddlSectorName.ClientID%>").value)
//			{
//				// Item was found, set the selected index.
//				ddl.selectedIndex = i;
//				return true;
//			}
//		}
//}

function load() {
    //    added by sribish
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formmodecheck);
}

function formmodecheck() {
    var vartxtcode = document.getElementById("<%=txtCode.ClientID%>");
   
    if (vartxtcode.value == '') {
        doLinks(false);
    }
    else {
        doLinks(true);
    }


}

function doLinks(how) {
    for (var l = document.links, i = l.length - 1; i > -1; --i)
        if (!how)
            l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
        else
            l[i].onclick = function () { return true; };
}
		

</script>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
   //prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        countryautoCompleteExtenderKeyUp();
        TypeNameAutoCompleteExtenderKeyUp();
        CategoryAutoCompleteExtenderKeyUp();
        CurrencyNameAutoCompleteExtenderKeyUp();
        CityNameAutoCompleteExtenderKeyUp();
        SectorNameAutoCompleteExtenderKeyUp();
        ControlAccAutoCompleteExtenderKeyUp();
    }
</script>
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier Agents" Width="927px" CssClass="field_heading" ForeColor="White"></asp:Label></TD></TR><TR><TD vAlign=top align=left width="50px">Code<SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
<TD class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtSuppCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" />Name&nbsp; 
    <INPUT style="WIDTH: 198px" id="txtSuppName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR>


        <tr style="display:none">  <td> Type <span class="td_cell" style="display:none;COLOR: #ff0000">*</span></td>
           
           
                <td align="left" class="td_cell" valign="top">
                    <select ID="ddlType" runat="server" class="drpdown" 
                        onchange="CallWebMethod('sptype')" style="display:none; WIDTH: 198px" 
                        tabindex="3">
                        <option selected=""></option>
                    </select> Name&nbsp;
                    <select ID="ddlTName" runat="server" class="drpdown" 
                        onchange="CallWebMethod('sptypename')" style="display:none;WIDTH: 198px" 
                        tabindex="4">
                        <option selected=""></option>
                    </select>
                </td>
            </tr>





            <tr>
                <td align="left" style="WIDTH: 88pxpx">
                    <asp:Label ID="TypeName" runat="server" Text="Type" width="70px"></asp:Label><span style="COLOR: #ff0000">*</span></td>
                <td align="left" colspan="2" valign="top" width="300px">
                    <asp:TextBox ID="TxtTypeName" runat="server" AutoPostBack="True" 
                        CssClass="field_input" MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                    <asp:TextBox ID="txtCode" runat="server" style="display:none"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:AutoCompleteExtender ID="TypeName_AutoCompleteExtender" runat="server" 
                        CompletionInterval="10" 
                        CompletionListCssClass="autocomplete_completionListElement" 
                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                        DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                        FirstRowSelected="false" MinimumPrefixLength="0" 
                        OnClientItemSelected="TypeNameautocompleteselected" ServiceMethod="GetTypeName" 
                        TargetControlID="TxtTypeName">
                    </asp:AutoCompleteExtender>
                    <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                    <input style="display:none" id="Text4" class="field_input" type="text"
                            runat="server" />
             </td>
        </tr>
            <tr>
           
                <td align="left" valign="top" width="150">
                    &nbsp;<uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                </td>
                <td style="WIDTH: 100px" valign="top">
                    <div ID="iframeINF" runat="server" style="WIDTH: 824px; HEIGHT: 450px">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table style="WIDTH: 656px">
                                    <tbody>
                                        <tr>
                                            <td class="td_cell" style="WIDTH: 80px">
                                                <asp:Panel ID="PanelMain" runat="server" GroupingText="Main Details" 
                                                    Width="699px">
                                                    <table style="WIDTH: 647px">
                                                        <tbody>
                                                            <tr>
                                                                <td align="left" style="WIDTH: 140px">
                                                                </td>
                                                                <td align="left" style="WIDTH: 88px">
                                                                </td>
                                                                <td align="left" style="WIDTH: 100px">
                                                                </td>
                                                            </tr> 
                                                            <tr style="display:none">

                                                                <td align="left" style="WIDTH: 140px">
                                                                    Category <span class="td_cell" style="COLOR: #ff0000">*</span>
                                                                    <span style="COLOR: #ff0000"></span>
                                                                </td>
                                                                <td align="left" style="WIDTH: 88px">
                                                                    <select ID="ddlCCode" runat="server" class="field_input" 
                                                                        onchange="CallWebMethod('catcode')" style="WIDTH: 223px" tabindex="5">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                </td>



                                                                <td align="left" style="WIDTH: 100px">
                                                                    <select ID="ddlCatName" runat="server" class="field_input" 
                                                                        onchange="CallWebMethod('catname')" style="WIDTH: 237px" tabindex="6">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                </td>
                                                            </tr>


            
   


                                                                       <tr>  <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblcategory" runat="server" Text="Category" Width="70px"></asp:Label><span style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="TxtCategoryName" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="TextCode" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncategorycode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="CategoryAutoCompleteExtender" runat="server"
                                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                                FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetCategoryName"
                                                                                TargetControlID="TxtCategoryName" OnClientItemSelected="CategoryNameautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>   
                            
                                                         <tr style="display:none">
                                                                <td align="left" style="WIDTH: 140px">
                                                                    Currency <span class="td_cell" style="COLOR: #ff0000">*</span>
                                                                </td>
                                                                <td align="left" style="WIDTH: 88px">
                                                                    <select ID="ddlCurrCode" runat="server" class="field_input" 
                                                                        onchange="CallWebMethod('currcode')" style="WIDTH: 223px" tabindex="7">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                </td>
                                                                <td align="left" style="WIDTH: 100px">
                                                                    <select ID="ddlCurrName" runat="server" class="field_input" 
                                                                        onchange="CallWebMethod('currname')" style="WIDTH: 237px" tabindex="8">
                                                                        <option selected=""></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
           
                                                                       <tr> 
                                                                       <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblCurrency" runat="server" Text="Currency" Width="70px"></asp:Label><span style="color: #ff0000">*</span>
                                                                <td align="left" valign="top" colspan="3" width="300px">
                                                                            <asp:TextBox ID="TxtCurrencyName" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="TextCurrencyCode" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:HiddenField ID="HiddenField4" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="CurrencyNameAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                                ServiceMethod="GetCurrencyName" TargetControlID="TxtCurrencyName" OnClientItemSelected="CurrencyNameautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text13" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text14" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
       

                                                            <caption>
                                                                &nbsp;<tr style="display:none">
                                                                    <td align="left" style="WIDTH: 140px">
                                                                        Country <span class="td_cell" style="COLOR: #ff0000">*</span>
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 88px">
                                                                        <select ID="ddlContCode" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('ctrycode')" style="WIDTH: 223px" tabindex="9">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 100px">
                                                                        <select ID="ddlcontName" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('ctryname')" style="WIDTH: 237px" tabindex="10">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 88px">
                                                                        <asp:Label ID="lblcountry" runat="server" Text="Country" Width="70px"></asp:Label><span style="color: #ff0000">*</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" valign="top" width="300px">
                                                                        <asp:TextBox ID="txtcountryname" runat="server" AutoPostBack="True" 
                                                                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                        <asp:TextBox ID="txtcountrycode" runat="server" style="display:none"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdncountry" runat="server" />
                                                                        <asp:AutoCompleteExtender ID="Country_AutoCompleteExtender" runat="server" 
                                                                            CompletionInterval="10" 
                                                                            CompletionListCssClass="autocomplete_completionListElement" 
                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                                                            DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                                                            FirstRowSelected="false" MinimumPrefixLength="0" 
                                                                            OnClientItemSelected="countryautocompleteselected" 
                                                                            ServiceMethod="Getcountrylist" TargetControlID="txtcountryname">
                                                                        </asp:AutoCompleteExtender>
                                                                        <input style="display: none" id="Text9" class="field_input" type="text" runat="server" />
                                                                        <input style="display: none" id="Text10" class="field_input" type="text" runat="server" />
                                                                    </td>
                                                                </tr>
<tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="Label2" runat="server" Text="City" Width="70px"></asp:Label><span  style="color: #ff0000">*</span>
                                                                <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtcityname" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcitycode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncity" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="City_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getcitylist" TargetControlID="txtcityname" OnClientItemSelected="Cityautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text7" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text8" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblsector" runat="server" Text="Sector" Width="70px"></asp:Label><span style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtsectorname" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtsectorcode" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnsector" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="Sector_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getsectorlist" TargetControlID="txtsectorname" OnClientItemSelected="Sectorautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text11" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text12" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                <tr style="display:none">
                                                                    <td align="left" style="WIDTH: 140px">
                                                                        City <span class="td_cell" style="COLOR: #ff0000">*</span>
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 88px">
                                                                        <select ID="ddlCityCode" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('citycode')" style="WIDTH: 223px" tabindex="11">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 100px">
                                                                        <select ID="ddlCityName" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('cityname')" style="WIDTH: 237px" tabindex="12">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display:none">
                                                                    <td align="left" style="WIDTH: 140px">
                                                                        Sector <span class="td_cell" style="COLOR: #ff0000">*</span>
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 88px">
                                                                        <select ID="ddlSectorCode" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('sectorcode')" style="WIDTH: 223px" tabindex="13">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 100px">
                                                                        <select ID="ddlSectorName" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('sectorname')" style="WIDTH: 237px" tabindex="14">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="WIDTH: 140px">
                                                                        Active</td>
                                                                    <td align="left" style="WIDTH: 88px">
                                                                        <INPUT id="chkActive" tabIndex=15 type=checkbox CHECKED runat="server" />
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 100px">
                                                                    </td>
                                                                </tr>
                                                                <tr style= "display:none">
                                                                    <td align="left" style="WIDTH: 140px">
                                                                        Control A/C Code <span class="td_cell" style="COLOR: #ff0000">*</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <select ID="cmbctrlcode" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('ctrlcode');" style="WIDTH: 182px" tabindex="16">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                    <td align="left">
                                                                        <select ID="cmbctrlname" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('ctrlname');" style="WIDTH: 246px" tabindex="17">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr> <td style="width: 88px" align="left">
                                                                            <asp:Label ID="Label1" runat="server" Text="Control A/C" Width="70px"></asp:Label><span style="color: #ff0000">*</span>
                                                                        </td>
                                                                    <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtcontrolaccname" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcontrolacccode" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncontrolacccode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="ControlAccAutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getcontrolacclist" TargetControlID="txtcontrolaccname" OnClientItemSelected="controlautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text5" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text6" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                <tr>
                                                                    <td align="left" style="height: 0px;">
                                                                        &nbsp;</td>
                                                                    <td align="left" style="height: 0px">
                                                                        <select ID="cmbaccrualcode" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('accrualcode');" style="WIDTH: 182px" tabindex="18" 
                                                                            visible="False">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                    <td align="left" style="height: 0px;">
                                                                        <select ID="cmbaccrualname" runat="server" class="field_input" 
                                                                            onchange="CallWebMethod('accrualname');" style="WIDTH: 246px" tabindex="19" 
                                                                            visible="False">
                                                                            <option selected=""></option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="WIDTH: 140px">
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                                                            onclick="btnSave_Click" tabIndex="20" Text="Save" />
                                                                    </td>
                                                                    <td align="left" style="WIDTH: 88px" colspan="2">
                                                                        <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                                                            onclick="btnCancel_Click" tabIndex="21" Text="Return To Search" />
                                                                   
                                                                        <asp:Button ID="btnhelp" runat="server" CssClass="field_button" 
                                                                            onclick="btnhelp_Click" tabIndex="22" Text="Help" />
                                                                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;height: 9px" type="text" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 150px" align="left">
                                                                            <asp:Button ID="btnCategory" TabIndex="27" OnClick="btnCategory_Click" 
                                                                                runat="server" Text="Add New Category"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                        </td>
                                                                      <td style="width: 150px" align="left" colspan="2">
                                                                            <asp:Button ID="btnCurrency" TabIndex="27" OnClick="btnCurrency_Click" 
                                                                                runat="server" Text="Add New Currency"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                  
                                                                            <asp:Button ID="btnCountry" TabIndex="27" OnClick="btnCountry_Click" 
                                                                                runat="server" Text="Add New Country"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                    <td style="width: 150px" align="left">
                                                                            <asp:Button ID="btnCity" TabIndex="27" OnClick="btnCity_Click" 
                                                                                runat="server" Text="Add New City"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                        </td>
                                                                         <td style="width: 150px" align="left" colspan="2">
                                                                            <asp:Button ID="btnSector" TabIndex="27" OnClick="btnSector_Click" 
                                                                                runat="server" Text="Add New Sector"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                                </td></tr>
                                                            </caption>
                                                        </tbody>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            </TBODY>
        </caption>
    </caption>
            </TABLE>

    <script language="javascript">
        formmodecheck();
        load();
</script>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
    </asp:Content>

