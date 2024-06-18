<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="Suppliers.aspx.vb" Inherits="Suppliers"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
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
                var select=document.getElementById("<%=ddlType.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlTName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,codeid,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,codeid,FillCatNames,ErrorHandler,TimeOutHandler);
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,codeid,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,codeid,FillSellCatNames,ErrorHandler,TimeOutHandler);
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
                
                ColServices.clsServices.GetSellCatCodeListnew(constr,codeid,FillSellCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellCatNameListnew(constr,codeid,FillSellCatNames,ErrorHandler,TimeOutHandler);

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
         case "sellcatcode":
                var select=document.getElementById("<%=ddlSellingCode.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
          case "sellcatname":
                var select=document.getElementById("<%=ddlSellingName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSellingCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;       
        
        case "currcode":
                var select=document.getElementById("<%=ddlCurrCode.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCurrName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       
         case "currname":
                var select=document.getElementById("<%=ddlCurrName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlCurrCode.ClientID%>");
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
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}
			
//checkFileExtension(elem) {
//        var filePath = elem.value;
//        elem.

//        if(filePath.indexOf('.') == -1)
//            return false;
//        
//        var validExtensions = new Array();
//        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
//    
//        validExtensions[0] = 'jpg';
//        validExtensions[1] = 'jpeg';
//        validExtensions[2] = 'bmp';
//        validExtensions[4] = 'gif';  
//        
//        for(var i = 0; i < validExtensions.length; i++) {
//            if(ext == validExtensions[i])
//                return true;
//        }

//        alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
//        return false;
//    }  

			
////function  GetValueFrom()
////{

////	var ddl = document.getElementById("<%=ddlTName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlType.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueCode()
////{
////	var ddl = document.getElementById("<%=ddlType.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlTName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}


////function  GetValueCategoryFrom()
////{

////	var ddl = document.getElementById("<%=ddlCatName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlCCode.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueCategoryCode()
////{
////	var ddl = document.getElementById("<%=ddlCCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlCatName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}

////function  GetValueSellingFrom()
////{

////	var ddl = document.getElementById("<%=ddlSellingName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlSellingCode.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueSellingCode()
////{
////	var ddl = document.getElementById("<%=ddlSellingCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlSellingName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}

////function  GetValueCurrencyFrom()
////{

////	var ddl = document.getElementById("<%=ddlCurrName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlCurrCode.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueCurrencyCode()
////{
////	var ddl = document.getElementById("<%=ddlCurrCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlCurrName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}


////function  GetValueCountryFrom()
////{

////	var ddl = document.getElementById("<%=ddlcontName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlContCode.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueCountryCode()
////{
////	var ddl = document.getElementById("<%=ddlContCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlcontName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}

////function  GetValueCityFrom()
////{

////	var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlCityCode.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueCityCode()
////{
////	var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlCityName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}

////function  GetValueSectorFrom()
////{

////	var ddl = document.getElementById("<%=ddlSectorName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlSectorCode.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueSectorCode()
////{
////	var ddl = document.getElementById("<%=ddlSectorCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlSectorName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}


////function  GetValuePostFrom()
////{

////	var ddl = document.getElementById("<%=ddlPostName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlPostCode.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValuePostCode()
////{
////	var ddl = document.getElementById("<%=ddlPostCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlPostName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}

////function  GetValueGroupFrom()
////{

////	var ddl = document.getElementById("<%=ddlGroupName.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlGroupName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}
////function  GetValueGroupCode()
////{
////	var ddl = document.getElementById("<%=ddlGroupCode.ClientID%>");
////	ddl.selectedIndex = -1;
////		// Iterate through all dropdown items.
////		for (i = 0; i < ddl.options.length; i++) 
////		{
////			if (ddl.options[i].text == 
////			document.getElementById("<%=ddlGroupName.ClientID%>").value)
////			{
////				// Item was found, set the selected index.
////				ddl.selectedIndex = i;
////				return true;
////			}
////		}
////}

</script> 
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<TABLE style="WIDTH: 807px; HEIGHT: 59px"><TBODY><TR><TD style="WIDTH: 228px; HEIGHT: 13px" class="field_input" contentEditable=true vAlign=middle align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier" CssClass="field_heading" Width="899px"></asp:Label></TD></TR><TR><TD style="WIDTH: 228px; HEIGHT: 13px" class="field_input" contentEditable=true vAlign=middle align=left colSpan=2></TD></TR><TR><TD style="WIDTH: 228px; HEIGHT: 13px" class="field_input" vAlign=middle align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="HEIGHT: 13px" class="field_input" align=left><INPUT style="WIDTH: 228px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp;&nbsp; Name <SPAN style="COLOR: #ff0000" class="td_cell">* &nbsp; &nbsp;&nbsp; </SPAN>&nbsp; <INPUT style="WIDTH: 228px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 228px; HEIGHT: 97px" vAlign=top align=left><TABLE style="WIDTH: 143px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="BtnMainDetails" tabIndex=51 onclick="BtnMainDetails_Click" runat="server" Text="Main Details" CssClass="field_button" Width="135px" BorderWidth="1px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnReservation" tabIndex=52 onclick="BtnReservation_Click" runat="server" Text="Reservation Details" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="Button2" tabIndex=53 onclick="Button2_Click" runat="server" Text="Sales Details" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="Button3" tabIndex=54 onclick="Button3_Click" runat="server" Text="Account Details" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnRoomType" tabIndex=55 onclick="BtnRoomType_Click" runat="server" Text="Room Type" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="Button5" tabIndex=56 onclick="Button5_Click" runat="server" Text="Categories" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnMealPlan" tabIndex=57 onclick="BtnMealPlan_Click" runat="server" Text="Meal Plans" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnAllotment" tabIndex=58 onclick="BtnAllotment_Click" runat="server" Text="Allotment Market" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnOtherService" tabIndex=59 onclick="BtnOtherService_Click" runat="server" Text="Other Services" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnGeneral" tabIndex=60 onclick="BtnGeneral_Click" runat="server" Text="General" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnSpEvents" tabIndex=61 onclick="BtnSpEvents_Click" runat="server" Text="Special Events / Extras" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnInfoWeb" tabIndex=62 onclick="BtnInfoWeb_Click" runat="server" Text="Info For Web Display" CssClass="field_button" Width="135px"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnEmail" tabIndex=63 onclick="BtnEmail_Click" runat="server" Text="Multiple Email" CssClass="field_button" Width="135px"></asp:Button></TD></TR></TBODY></TABLE></TD><TD class="field_input" vAlign=top align=left><asp:Panel id="PanelMain" runat="server" Width="743px" GroupingText="Main Details"><TABLE style="WIDTH: 623px"><TBODY><TR><TD style="WIDTH: 100px" class="field_input" align=left>Type <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD style="WIDTH: 88px" align=left><SELECT style="WIDTH: 223px" id="ddlType" class="field_input" tabIndex=3 onchange="CallWebMethod('sptype')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 238px" id="ddlTName" class="field_input" tabIndex=4 onchange="CallWebMethod('sptypename')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left>Category <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN><SPAN style="COLOR: #ff0000"></SPAN></TD><TD style="WIDTH: 88px" align=left><SELECT style="WIDTH: 223px" id="ddlCCode" class="field_input" tabIndex=5 onchange="CallWebMethod('catcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 237px" id="ddlCatName" class="field_input" tabIndex=6 onchange="CallWebMethod('catname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left>Selling Category <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 88px" align=left><SELECT style="WIDTH: 223px" id="ddlSellingCode" class="field_input" tabIndex=7 onchange="CallWebMethod('sellcatcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 237px" id="ddlSellingName" class="field_input" tabIndex=8 onchange="CallWebMethod('sellcatname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left>Curren<SPAN style="FONT-SIZE: 8pt; COLOR: #000000; FONT-FAMILY: Arial">c</SPAN>y <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 88px" align=left><SELECT style="WIDTH: 223px" id="ddlCurrCode" class="field_input" tabIndex=9 onchange="CallWebMethod('currcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 237px" id="ddlCurrName" class="field_input" tabIndex=10 onchange="CallWebMethod('currname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left>Country <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 88px" align=left><SELECT style="WIDTH: 223px" id="ddlContCode" class="field_input" tabIndex=11 onchange="CallWebMethod('ctrycode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 237px" id="ddlcontName" class="field_input" tabIndex=12 onchange="CallWebMethod('ctryname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left>City <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 88px" align=left><SELECT style="WIDTH: 223px" id="ddlCityCode" class="field_input" tabIndex=13 onchange="CallWebMethod('citycode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 237px" id="ddlCityName" class="field_input" tabIndex=14 onchange="CallWebMethod('cityname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left>Sector <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 88px" align=left><SELECT style="WIDTH: 223px" id="ddlSectorCode" class="field_input" tabIndex=15 onchange="CallWebMethod('sectorcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px" align=left><SELECT style="WIDTH: 237px" id="ddlSectorName" class="field_input" tabIndex=16 onchange="CallWebMethod('sectorname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left><STRONG>Active</STRONG></TD><TD style="WIDTH: 88px" align=left><INPUT id="chkActive" tabIndex=17 type=checkbox CHECKED runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left><STRONG>Preferred</STRONG></TD><TD style="WIDTH: 88px" align=left><INPUT id="ChkPreferred" tabIndex=18 type=checkbox CHECKED runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left>Order</TD><TD style="WIDTH: 88px" align=left><INPUT style="WIDTH: 194px; TEXT-ALIGN: right" id="txtOrder" class="field_input" tabIndex=19 type=text maxLength=4 runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" class="field_input" align=left><asp:Button id="btnSave_Main" tabIndex=20 onclick="btnSave_Main_Click" runat="server" Text="Save" CssClass="field_button" Width="46px"></asp:Button></TD><TD style="WIDTH: 88px" align=left><asp:Button id="btnCancel_Main" tabIndex=21 onclick="btnCancel_Main_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px" align=left></TD></TR></TBODY></TABLE>&nbsp;&nbsp; </asp:Panel> <asp:Panel id="PanelReservation" runat="server" Width="743px" GroupingText="Reservation Details" Visible="False"><TABLE style="WIDTH: 732px" align=center><TBODY><TR><TD style="WIDTH: 109px" align=left>Address <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress1" class="field_input" tabIndex=28 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 340px" align=left>Communicate By&nbsp; <asp:DropDownList id="ddlComunicate" tabIndex=41 runat="server" CssClass="field_input" Width="81px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem>Email</asp:ListItem>
<asp:ListItem>Fax</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px; HEIGHT: 24px" align=left></TD><TD style="WIDTH: 329px; HEIGHT: 24px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress2" class="field_input" tabIndex=29 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 340px; HEIGHT: 24px" align=left>Sell Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList id="ddlSell" tabIndex=42 runat="server" CssClass="field_input" Width="82px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem>Beach</asp:ListItem>
<asp:ListItem>City</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px; HEIGHT: 24px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left></TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress3" class="field_input" tabIndex=30 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 340px" align=left>Auto Email&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; <asp:DropDownList id="ddlAutoEmail" tabIndex=43 runat="server" CssClass="field_input" Width="82px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem Selected="True">Yes</asp:ListItem>
<asp:ListItem>No</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left>Telephone <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone1" class="field_input" tabIndex=31 type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 340px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left></TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone2" class="field_input" tabIndex=32 type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 340px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left>Fax <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResFax" class="field_input" tabIndex=33 type=text maxLength=50 runat="server" /></TD><TD style="WIDTH: 340px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left>Contact</TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResContact1" class="field_input" tabIndex=34 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 340px" align=left><INPUT id="ChkWeekend1" tabIndex=44 type=checkbox CHECKED runat="server" />&nbsp; Weekend Option 1</TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left></TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResContact2" class="field_input" tabIndex=35 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 340px" align=left><INPUT style="WIDTH: 136px" id="txtWeekend1_1" class="field_input" disabled tabIndex=38 type=text runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left>E-mail</TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResEmail" class="field_input" tabIndex=36 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 340px" align=left><INPUT style="WIDTH: 136px" id="txtWeekend1_2" class="field_input" disabled tabIndex=38 type=text runat="server" /></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left>Web Site</TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 297px" id="txtResWebSite" class="field_input" tabIndex=37 type=text maxLength=100 runat="server" /></TD><TD style="WIDTH: 340px" align=left><INPUT id="ChkWeekend2" tabIndex=47 type=checkbox CHECKED runat="server" />&nbsp; Weekend Option 2</TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 109px; HEIGHT: 15px" align=left></TD><TD style="WIDTH: 329px; HEIGHT: 15px" align=left></TD><TD style="WIDTH: 340px; HEIGHT: 15px" align=left><INPUT style="WIDTH: 136px" id="txtWeekend2_1" class="field_input" disabled tabIndex=38 type=text runat="server" /></TD><TD style="WIDTH: 100px; HEIGHT: 15px" align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left>Distance From</TD><TD style="WIDTH: 329px" align=left><INPUT style="WIDTH: 98px" id="txtResDistanceFrom" class="field_input" tabIndex=38 type=text runat="server" />&nbsp;<asp:DropDownList id="ddlRescity" tabIndex=39 runat="server" CssClass="field_input" Width="82px"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem>Airport</asp:ListItem>
<asp:ListItem Selected="True">City Center</asp:ListItem>
</asp:DropDownList>&nbsp; <INPUT style="WIDTH: 47px" id="txtResKM" class="field_input" tabIndex=40 type=text maxLength=4 runat="server" /> Kms</TD><TD style="WIDTH: 340px" align=left><INPUT style="WIDTH: 136px" id="txtWeekend2_2" class="field_input" disabled tabIndex=38 type=text runat="server" /></TD><TD align=left></TD></TR><TR><TD style="WIDTH: 109px" align=left><asp:Button id="BtnResSave" tabIndex=50 onclick="BtnResSave_Click" runat="server" Text="Save" CssClass="field_button" Width="48px"></asp:Button></TD><TD style="WIDTH: 329px" align=left><asp:Button id="BtnResCancel" tabIndex=51 onclick="BtnResCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 340px" align=left></TD><TD align=left></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelSales" runat="server" Width="743px" GroupingText="Sales Details"><TABLE style="WIDTH: 412px" border=0><TBODY><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>Telephone</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleTelephone1" class="field_input" tabIndex=47 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleTelephone2" class="field_input" tabIndex=48 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>Fax</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleFax" class="field_input" tabIndex=49 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>Contact</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleContact1" class="field_input" tabIndex=50 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleContact2" class="field_input" tabIndex=51 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px" align=left>E-mail</TD><TD style="WIDTH: 100px; HEIGHT: 26px" align=left><INPUT style="WIDTH: 295px" id="txtSaleEmail" class="field_input" tabIndex=52 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left><asp:Button id="BtnSaleSave" tabIndex=53 onclick="BtnSaleSave_Click" runat="server" Text="Save" CssClass="field_button" Width="44px"></asp:Button></TD><TD style="WIDTH: 100px" align=left><asp:Button id="BtnSaleCancel" tabIndex=54 onclick="BtnSaleCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelAccount" runat="server" Width="743px" GroupingText="Account Details"><TABLE style="WIDTH: 582px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 15px" align=left>Telephone</TD><TD style="WIDTH: 446px; HEIGHT: 15px" align=left><INPUT style="WIDTH: 315px" id="txtAccTelephone1" class="field_input" tabIndex=55 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 446px" align=left><INPUT style="WIDTH: 315px" id="txtAccTelephone2" class="field_input" tabIndex=56 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left>Fax</TD><TD style="WIDTH: 446px" align=left><INPUT style="WIDTH: 315px" id="txtAccFax" class="field_input" tabIndex=57 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left>Contact</TD><TD style="WIDTH: 446px" align=left><INPUT style="WIDTH: 315px" id="txtAccContact1" class="field_input" tabIndex=58 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 446px" align=left><INPUT style="WIDTH: 315px" id="txtAccContact2" class="field_input" tabIndex=59 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left>E-mail</TD><TD style="WIDTH: 446px" align=left><INPUT style="WIDTH: 315px" id="txtAccEmail" class="field_input" tabIndex=60 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left>Control A/C Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 446px"><SELECT style="WIDTH: 182px" id="ddlAccCode" class="field_input" tabIndex=61 onchange="CallWebMethod('acccode')" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;<SELECT style="WIDTH: 246px" id="ddlAccName" class="field_input" tabIndex=62 onchange="CallWebMethod('accname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" align=left>Cash Supplier</TD><TD style="WIDTH: 446px" align=left><INPUT id="ChkCashSup" tabIndex=62 type=checkbox runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left>Credit Days</TD><TD style="WIDTH: 446px" align=left><INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="TxtAccCreditDays" class="field_input" tabIndex=63 type=text maxLength=5 runat="server" />&nbsp;&nbsp;&nbsp;&nbsp; Credit Limit <INPUT style="WIDTH: 144px; TEXT-ALIGN: right" id="txtAccCreditLimit" class="field_input" tabIndex=64 type=text maxLength=15 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px" align=left>Post To</TD><TD style="WIDTH: 446px; HEIGHT: 22px" align=left><SELECT style="WIDTH: 182px" id="ddlPostCode" class="field_input" onchange="CallWebMethod('postcode')" runat="server"> <OPTION selected></OPTION></SELECT> <SELECT style="WIDTH: 246px" id="ddlPostName" class="field_input" onchange="CallWebMethod('postname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 100px" align=left><asp:Button id="BtnAccSave" tabIndex=66 onclick="BtnAccSave_Click1" runat="server" Text="Save" CssClass="field_button" Width="50px"></asp:Button></TD><TD style="WIDTH: 446px" align=left><asp:Button id="BtnAccCancel" tabIndex=67 onclick="BtnAccCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelRoomType" runat="server" Width="743px" GroupingText="Room Types"><TABLE><TBODY><TR><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:GridView id="Gv_RoomType" tabIndex=68 runat="server" Width="660px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkSelect" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="rmtypname" HeaderText="Room Type Name"></asp:BoundField>
<asp:BoundField DataField="rmtypcode" HeaderText="Room Type Code"></asp:BoundField>
<asp:BoundField DataField="rankorder" HeaderText="Order"></asp:BoundField>
<asp:TemplateField HeaderText="Inactive">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
<INPUT id="ChkInactive" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><asp:Button id="BtnSelectRommType" tabIndex=69 onclick="BtnSelectRommType_Click" runat="server" Text="Select All" CssClass="field_button"></asp:Button> <asp:Button id="BtnDeSelectRommType" tabIndex=70 onclick="BtnDeSelectRommType_Click" runat="server" Text="DeSelect All" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><TABLE><TBODY><TR><TD style="WIDTH: 29px; HEIGHT: 22px"><asp:Button id="BtnSaveRoomType" tabIndex=71 onclick="BtnSaveRoomType_Click" runat="server" Text="Save" CssClass="field_button" Width="60px"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:Button id="BtnCancelRoom" tabIndex=72 onclick="BtnCancelRoom_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelCategories" runat="server" Width="743px" GroupingText="Categories"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 4px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:GridView id="Gv_Categories" tabIndex=73 runat="server" Width="607px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkSelect" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="rmcatname" HeaderText="Category Name"></asp:BoundField>
<asp:BoundField DataField="rmcatcode" HeaderText="Category Code"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><asp:Button id="BtnSelectAllCategories" tabIndex=74 onclick="BtnSelectAllCategories_Click" runat="server" Text="Select All" CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="BtnDeSelectAllCategories" tabIndex=75 onclick="BtnDeSelectAllCategories_Click" runat="server" Text="DeSelect All" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><TABLE><TBODY><TR><TD style="WIDTH: 29px; HEIGHT: 22px"><asp:Button id="BtnSaveCategory" tabIndex=76 onclick="BtnSaveCategory_Click" runat="server" Text="Save" CssClass="field_button" Width="60px"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:Button id="BtnCancelCategory" tabIndex=77 onclick="BtnCancelCategory_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelMealPlan" runat="server" Width="743px" GroupingText="Meal Plan"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 4px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:GridView id="Gv_MealPlan" tabIndex=78 runat="server" Width="607px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkSelect" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="mealname" HeaderText="Meal Plan Name"></asp:BoundField>
<asp:BoundField DataField="mealcode" HeaderText="Meal Plan Code"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><asp:Button id="BtnSelectAllMealPlan" tabIndex=79 onclick="BtnSelectAllMealPlan_Click" runat="server" Text="Select All" CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="BtnDeSelectAllMealPlan" tabIndex=80 onclick="BtnDeSelectAllMealPlan_Click" runat="server" Text="DeSelect All" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><TABLE><TBODY><TR><TD style="WIDTH: 29px; HEIGHT: 22px"><asp:Button id="BtnSaveMeal" tabIndex=81 onclick="BtnSaveMeal_Click" runat="server" Text="Save" CssClass="field_button" Width="60px"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:Button id="BtnMealCancel" tabIndex=82 onclick="BtnMealCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelAllotment" runat="server" Width="743px" GroupingText="Allotment Markets"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 4px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:GridView id="GV_Market" tabIndex=83 runat="server" Width="607px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkSelect" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="plgrpcode" HeaderText="Market Code"></asp:BoundField>
<asp:BoundField DataField="plgrpname" HeaderText="Market Name"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><asp:Button id="BtnSelectAllMarket" tabIndex=84 onclick="BtnSelectAllMarket_Click" runat="server" Text="Select All" CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="BtnDeSelectAllMarket" tabIndex=85 onclick="BtnDeSelectAllMarket_Click" runat="server" Text="DeSelect All" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 41px"><TABLE><TBODY><TR><TD style="WIDTH: 29px; HEIGHT: 22px"><asp:Button id="BtnSaveMarket" tabIndex=86 onclick="BtnSaveMarket_Click" runat="server" Text="Save" CssClass="field_button" Width="60px"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:Button id="BtnCancelAlotment" tabIndex=87 onclick="BtnCancelAlotment_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelOtherServices" runat="server" Width="743px" GroupingText="Other Services"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 15px"><STRONG>Group</STRONG></TD><TD style="WIDTH: 110px; HEIGHT: 15px"><asp:DropDownList id="ddlGroupCode" runat="server" CssClass="field_input" Width="136px" AutoPostBack="True" OnSelectedIndexChanged="ddlGroupCode_SelectedIndexChanged"></asp:DropDownList></TD><TD style="WIDTH: 100px; HEIGHT: 15px" colSpan=2><asp:DropDownList id="ddlGroupName" runat="server" CssClass="field_input" Width="248px" OnSelectedIndexChanged="ddlGroupName_SelectedIndexChanged"></asp:DropDownList></TD><TD style="WIDTH: 100px; HEIGHT: 15px">&nbsp;</TD></TR><TR><TD vAlign=top align=left><STRONG>Group Selected</STRONG></TD><TD style="WIDTH: 100px" colSpan=4><asp:GridView id="GV_Group" tabIndex=89 runat="server" Width="607px" AutoGenerateColumns="False" Enabled="False"><Columns>
<asp:TemplateField><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("othselected") %>'></asp:TextBox> 
</EditItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<asp:CheckBox id="CheckBox1" runat="server"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField ReadOnly="True" DataField="othgrpcode" HeaderText="Oth. Srv. Grp. Code"></asp:BoundField>
<asp:BoundField ReadOnly="True" DataField="othgrpname" HeaderText="Oth. Srv. Grp. Name"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD style="WIDTH: 100px"><STRONG>Types</STRONG></TD><TD style="WIDTH: 110px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"><STRONG>Categories</STRONG></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 15px" vAlign=top align=left colSpan=3><asp:GridView id="GvTypes" tabIndex=90 runat="server" Width="397px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("othtypselected") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<asp:CheckBox id="CheckBox2" runat="server"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="othtypcode" HeaderText="Type Code"></asp:BoundField>
<asp:BoundField DataField="othtypname" HeaderText="Type Name"></asp:BoundField>
</Columns>
</asp:GridView></TD><TD style="WIDTH: 100px; HEIGHT: 15px" vAlign=top align=left colSpan=2><asp:GridView id="Gv_Category" tabIndex=91 runat="server" Width="321px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("othcatselected") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<asp:CheckBox id="CheckBox3" runat="server"></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="othcatcode" HeaderText="Cat Code"></asp:BoundField>
<asp:BoundField DataField="othcatname" HeaderText="Cat Name"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 4px"></TD><TD style="WIDTH: 110px; HEIGHT: 4px"></TD><TD style="WIDTH: 100px; HEIGHT: 4px"></TD><TD style="WIDTH: 100px; HEIGHT: 4px"></TD><TD style="WIDTH: 100px; HEIGHT: 4px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnSaveOther" tabIndex=92 onclick="BtnSaveOther_Click" runat="server" Text="Save" CssClass="field_button" Width="56px"></asp:Button></TD><TD style="WIDTH: 110px"><asp:Button id="Btn_DelOthGrp" onclick="Btn_DelOthGrp_Click" runat="server" Text="Delete Selected Group" CssClass="field_button" Width="136px"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="BtnCancelOther" tabIndex=93 onclick="BtnCancelOther_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 110px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelGeneral" runat="server" Width="743px" GroupingText="General"><TABLE style="WIDTH: 414px"><TBODY><TR><TD style="WIDTH: 100px" align=left>General Comments</TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><asp:TextBox id="txtGeneral" tabIndex=94 runat="server" Height="100px" CssClass="field_input" Width="389px" TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="BtnGeneralSave" tabIndex=95 onclick="BtnGeneralSave_Click" runat="server" Text="Save" CssClass="field_button" Width="56px"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="BtnGeneralCancel" tabIndex=96 onclick="BtnGeneralCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelSpEvent" runat="server" Width="743px" GroupingText="Special Events / Extras"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 4px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:GridView id="GV_SpecialEvent" tabIndex=97 runat="server" Width="607px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkSelect" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="spleventcode" HeaderText="Spevent Code"></asp:BoundField>
<asp:BoundField DataField="spleventname" HeaderText="Spevent Name"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><asp:Button id="BtnSelectAllSpEvent" tabIndex=98 onclick="BtnSelectAllSpEvent_Click" runat="server" Text="Select All" CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="BtnDeSelectAllSpEvent" tabIndex=99 onclick="BtnDeSelectAllSpEvent_Click" runat="server" Text="DeSelect All" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><TABLE style="WIDTH: 210px"><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="BtnSaveSpecialEve" tabIndex=100 onclick="BtnSaveSpecialEve_Click" runat="server" Text="Save" CssClass="field_button" Width="61px"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="BtnCancelSpecailEe" tabIndex=101 onclick="BtnCancelSpecailEe_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelInfoForWEb" runat="server" Width="743px" GroupingText="Info For Web Display"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 24px">Rooms</TD><TD style="HEIGHT: 24px" colSpan=3><asp:TextBox id="txtRooms" tabIndex=102 runat="server" Height="46px" CssClass="field_input" Width="599px" TextMode="MultiLine" MaxLength="100"></asp:TextBox>&nbsp;&nbsp; </TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 33px">Location&nbsp;&nbsp; </TD><TD style="HEIGHT: 33px" colSpan=3><asp:TextBox id="txtLocation" tabIndex=103 runat="server" Height="43px" CssClass="field_input" Width="597px" TextMode="MultiLine" MaxLength="1000"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 33px">Restaurants</TD><TD style="HEIGHT: 33px" colSpan=3><asp:TextBox id="txtRestaurants" tabIndex=104 runat="server" Height="43px" CssClass="field_input" Width="597px" TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px">Facilities&nbsp;&nbsp; </TD><TD colSpan=3><asp:TextBox id="txtFacilities" tabIndex=105 runat="server" Height="43px" CssClass="field_input" Width="597px" TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px">Star - Accept&nbsp; nos&nbsp;&nbsp; &nbsp; 1- 6</TD><TD style="WIDTH: 100px"><asp:DropDownList id="ddlStarNo" tabIndex=106 runat="server" Width="49px"><asp:ListItem>1</asp:ListItem>
<asp:ListItem>2</asp:ListItem>
<asp:ListItem>3</asp:ListItem>
<asp:ListItem>4</asp:ListItem>
<asp:ListItem>5</asp:ListItem>
<asp:ListItem>6</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 39px">&nbsp;</TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 147px" colSpan=4><asp:GridView id="Gv_InfoForWeb" tabIndex=107 runat="server" Width="583px" AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkSelect" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Desc" HeaderText="Description"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 15px"></TD><TD style="WIDTH: 100px; HEIGHT: 15px"></TD><TD style="WIDTH: 100px; HEIGHT: 15px"></TD><TD style="WIDTH: 39px; HEIGHT: 15px"></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px">Upload Sub Image 1</TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:FileUpload id="FileUpload1" tabIndex=108 runat="server" CssClass="field_input" Width="251px"></asp:FileUpload></TD><TD style="WIDTH: 100px; HEIGHT: 22px">Uplaod Sub Image 2</TD><TD style="WIDTH: 39px; HEIGHT: 22px"><asp:FileUpload id="FileUpload2" tabIndex=109 runat="server" CssClass="field_input" Width="251px"></asp:FileUpload></TD></TR><TR><TD style="WIDTH: 100px">Upload Sub Image 3</TD><TD style="WIDTH: 100px"><asp:FileUpload id="FileUpload3" tabIndex=110 runat="server" CssClass="field_input" Width="251px"></asp:FileUpload></TD><TD style="WIDTH: 100px">Upload Sub Image 4</TD><TD style="WIDTH: 39px"><asp:FileUpload id="FileUpload4" tabIndex=111 runat="server" CssClass="field_input" Width="251px"></asp:FileUpload></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 19px"></TD><TD style="HEIGHT: 19px" align=left colSpan=3>Upload&nbsp;Main Image&nbsp;&nbsp;&nbsp; <asp:FileUpload id="FileUpload5" tabIndex=112 runat="server" CssClass="field_input" Width="251px"></asp:FileUpload></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 1px"></TD><TD style="WIDTH: 100px; HEIGHT: 1px"><asp:Button id="BtnimgUpload" onclick="BtnimgUpload_Click" runat="server" Text="Upload Images" CssClass="search_button" Width="94px" Visible="False"></asp:Button> <asp:TextBox id="txtimg1" runat="server" Width="17px" Visible="False"></asp:TextBox> <asp:TextBox id="txtimg2" runat="server" Width="17px" Visible="False"></asp:TextBox> <asp:TextBox id="txtimg3" runat="server" Width="17px" Visible="False"></asp:TextBox></TD><TD style="WIDTH: 100px; HEIGHT: 1px">&nbsp;&nbsp; <asp:TextBox id="txtimg4" runat="server" Width="17px" Visible="False"></asp:TextBox> <asp:TextBox id="txtimg5" runat="server" Width="17px" Visible="False"></asp:TextBox></TD><TD style="WIDTH: 39px; HEIGHT: 1px">&nbsp;</TD></TR><TR><TD style="WIDTH: 100px"><asp:Button id="BtnSaveInfoWeb" tabIndex=113 onclick="BtnSaveInfoWeb_Click" runat="server" Text="Save" CssClass="field_button" Width="66px"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="BtnCancelInfoWeb" tabIndex=114 onclick="BtnCancelInfoWeb_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 39px"></TD></TR></TBODY></TABLE></asp:Panel> <asp:Panel id="PanelEmail" runat="server" Width="735px" GroupingText="Multiple Email"><TABLE style="WIDTH: 403px"><TBODY><TR><TD align=left></TD></TR><TR><TD align=right><asp:Button id="BtnAdd" tabIndex=58 onclick="BtnAdd_Click1" runat="server" Text="Add" CssClass="field_button" Width="51px"></asp:Button>&nbsp;</TD></TR><TR><TD align=left><asp:GridView id="gv_Email" tabIndex=115 runat="server" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="no" HeaderText="Sr No"></asp:BoundField>
<asp:TemplateField HeaderText="Contact Person Name &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>
<INPUT style="WIDTH: 215px" id="txtPerson" class="field_input" type=text maxLength=100 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Email Address &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>
<INPUT style="WIDTH: 220px" id="txtEmail" class="field_input" type=text maxLength=100 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Contact No &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>
<INPUT style="WIDTH: 159px" id="txtContactNo" class="field_input" type=text maxLength=15 runat="server" /> 
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><TABLE><TBODY><TR><TD style="WIDTH: 29px; HEIGHT: 22px"><asp:Button id="BtnEmailSave" tabIndex=116 onclick="BtnEmailSave_Click" runat="server" Text="Save" CssClass="field_button" Width="60px"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:Button id="BtnEmailCancel" tabIndex=117 onclick="BtnEmailCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 228px; HEIGHT: 21px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 100px; HEIGHT: 21px"></TD></TR></TBODY></TABLE>
</contenttemplate>
        <triggers>
<asp:PostBackTrigger ControlID="BtnSaveInfoWeb"></asp:PostBackTrigger>
</triggers>
    </asp:UpdatePanel>
    &nbsp; &nbsp;
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

