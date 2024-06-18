<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptOtherServiceCost.aspx.vb" Inherits="rptOtherServiceCost"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ OutputCache location="none" %> 
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<%--<script language="javascript" src="js\date-picker.js"></script>  --%>
    <%--<script language="javascript" src="js\datefun.js"></script>--%>
   <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>


<script language="javascript" type="text/javascript" >
function CallWebMethod(methodType)
    {
        switch(methodType)
        {
           case "othergroupcode":
                var select=document.getElementById("<%=ddlOtherGroupCode.ClientID%>"); 
                var party=select.options[select.selectedIndex].text;               
                var selectname=document.getElementById("<%=ddlOtherGroupName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                break;
            case "othergroupname":
                var select=document.getElementById("<%=ddlOtherGroupName.ClientID%>");                
                var party=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlOtherGroupCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;       

         case "sptype":
                var select=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSpTypeName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;        
                
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCatCodeListnew(constr,sptype,FillCatCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCatNameListnew(constr,sptype,FillCatNames,ErrorHandler,TimeOutHandler);
                
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
               
                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,null,ctry,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,null,ctry,null,FillSupplierNames,ErrorHandler,TimeOutHandler);

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
               
                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,null,ctry,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,null,ctry,null,FillSupplierNames,ErrorHandler,TimeOutHandler);

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
               
                var selectcountry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectcountry.options[selectcountry.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,null,ctry,city,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,null,ctry,city,FillSupplierNames,ErrorHandler,TimeOutHandler);

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
               
                var selectcountry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectcountry.options[selectcountry.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,cat,null,ctry,city,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,cat,null,ctry,city,FillSupplierNames,ErrorHandler,TimeOutHandler);
                
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
                break; 

            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var plgrp=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
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
            <td style="width: 100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Other Service Cost Sheet Report</td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <span class="td_cell" style="color: #ff0000"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 313px">
  
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="Label2" runat="server" 
        Text="Other Service Group Code" CssClass="td_cell" Width="160px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlOtherGroupCode" class="drpdown" tabIndex=1 onchange="CallWebMethod('othergroupcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD>
    <asp:Label id="Label3" runat="server" Text="Other Service Group Name" 
        CssClass="td_cell" Width="169px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlOtherGroupName" class="drpdown" tabIndex=2 onchange="CallWebMethod('othergroupname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblMarketCode" runat="server" Text="Market Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlMarketCode" class="drpdown" tabIndex=3 onchange="CallWebMethod('marketcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblMarketName" runat="server" Text="Market Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlMarketName" class="drpdown" tabIndex=4 onchange="CallWebMethod('marketname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="Label4" runat="server" Text="Season Code" CssClass="td_cell"></asp:Label></TD><TD><asp:DropDownList id="ddlseas1code" tabIndex=7 runat="server" CssClass="drpdown" Width="170px" OnSelectedIndexChanged="ddlseas1code_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD><TD><asp:Label id="Label5" runat="server" Text="Season Name" CssClass="td_cell" Width="124px"></asp:Label></TD><TD><asp:DropDownList id="ddlseas1name" tabIndex=8 runat="server" CssClass="drpdown" Width="237px" OnSelectedIndexChanged="ddlseas1name_SelectedIndexChanged"></asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR><TR><TD><asp:Label id="lblSupplierTypeCode" runat="server" Text="Supplier Type Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlSPTypeCode" class="drpdown" tabIndex=13 onchange="CallWebMethod('sptype')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblsuppliertypename" runat="server" Text="Supplier Type Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 238px" id="ddlSpTypeName" class="drpdown" tabIndex=14 onchange="CallWebMethod('sptypename')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblCountryCode" runat="server" Text="Country Code" CssClass="td_cell"></asp:Label><span style="color:Red;">*</span></TD><TD><SELECT style="WIDTH: 170px" id="ddlContCode" class="drpdown" tabIndex=15 onchange="CallWebMethod('ctrycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcountryname" runat="server" Text="Country Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlcontName" class="drpdown" tabIndex=16 onchange="CallWebMethod('ctryname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblCityCode" runat="server" Text="City Code" CssClass="td_cell"></asp:Label><span style="color:Red;">*</span></TD><TD><SELECT style="WIDTH: 170px" id="ddlCityCode" class="drpdown" tabIndex=17 onchange="CallWebMethod('citycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcityname" runat="server" Text="City Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlCityName" class="drpdown" tabIndex=18 onchange="CallWebMethod('cityname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT>&nbsp;</TD></TR><TR><TD><asp:Label id="lblCategorycode" runat="server" Text="Category Code" CssClass="td_cell"></asp:Label><span style="color:Red;">*</span></TD><TD><SELECT style="WIDTH: 170px" id="ddlCCode" class="drpdown" tabIndex=19 onchange="CallWebMethod('catcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblCategoryName" runat="server" Text="Category Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlCatName" class="drpdown" tabIndex=20 onchange="CallWebMethod('catname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblSupplierCode" runat="server" Text="Supplier Code" CssClass="td_cell"></asp:Label><span style="color:Red;">*</span></TD><TD><SELECT style="WIDTH: 170px" id="ddlPartyCode" class="drpdown" tabIndex=21 onchange="CallWebMethod('suppliercode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblSupplierName" runat="server" Text="Supplier Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlPartyName" class="drpdown" tabIndex=22 onchange="CallWebMethod('suppliername')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" CssClass="td_cell" Text="Approved/Unapproved"
                Width="140px"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="drpdown" Width="171px">
                <asp:ListItem Value="2">All</asp:ListItem>
                <asp:ListItem Value="1">Approve</asp:ListItem>
                <asp:ListItem Value="0">Unapprove</asp:ListItem>
            </asp:DropDownList></td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <TR><TD colSpan=4><asp:Label id="lblmsg" runat="server" Text="( * Leave Code Field Blank for Print All )" ForeColor="Red" CssClass="td_cell"></asp:Label> &nbsp;</TD></TR>
    <TR><TD style="TEXT-ALIGN: center" colSpan=4><asp:Button id="BtnClear" tabIndex=24 
            runat="server" Text="Clear" CssClass="btn"></asp:Button> &nbsp;
    <asp:Button id="BtnPrint" tabIndex=23 runat="server" Text="Load Report" CssClass="btn"></asp:Button>&nbsp;
     <asp:Button id="btnhelp" tabIndex=25 onclick="btnhelp_Click" runat="server" 
            Text="Help" CssClass="btn"></asp:Button>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
            height: 9px" type="text" /></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender>
             <cc1:MaskedEditExtender id="MEFromDate" runat="server" Enabled="True" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender>
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
    &nbsp; &nbsp; &nbsp; &nbsp;
    <br />

</asp:Content>