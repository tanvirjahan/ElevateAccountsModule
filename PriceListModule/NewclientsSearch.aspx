<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NewclientsSearch.aspx.vb" Inherits="NewclientsSearch"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

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
// WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
function CallWebMethod(methodType)
    {
        switch(methodType)
        {
                
        case "catcode":
                var select=document.getElementById("<%=ddlCCode.ClientID%>");                
                var cat=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCatName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                document.getElementById("<%=hdnsptypecode.ClientID%>").value = cat;
                break;
        case "catname":
                var select=document.getElementById("<%=ddlCatName.ClientID%>");                
                var cat=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsptypecode.ClientID%>").value = cat;
                break;
       
         case "ctrycode":
                var select=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlcontName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncountry.ClientID%>").value = ctry;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,ctry,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,ctry,FillCityNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSectorCodeListnew(constr,ctry,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,ctry,null,FillSectorNames,ErrorHandler,TimeOutHandler);
                break;
                
            case "ctryname":
                var select=document.getElementById("<%=ddlcontName.ClientID%>");
                var ctry=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlContCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncountry.ClientID%>").value = ctry;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                
                ColServices.clsServices.GetCityCodeListnew(constr,ctry,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,ctry,FillCityNames,ErrorHandler,TimeOutHandler);

                ColServices.clsServices.GetSectorCodeListnew(constr,ctry,null,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,ctry,null,FillSectorNames,ErrorHandler,TimeOutHandler);
                break;

            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var city=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncitycode.ClientID%>").value = city;
                var selectctry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectctry.options[selectctry.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                var txtcode=document.getElementById("<%=txtCityCode.ClientID%>");
                txtcode.value=city;

                var txtname=document.getElementById("<%=txtCityName.ClientID%>");
                txtname.value=select.options[select.selectedIndex].value;

                ColServices.clsServices.GetSectorCodeListnew(constr,ctry,city,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,ctry,city,FillSectorNames,ErrorHandler,TimeOutHandler);

                break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var city=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncitycode.ClientID%>").value = city;
                var selectctry=document.getElementById("<%=ddlContCode.ClientID%>");
                var ctry=selectctry.options[selectctry.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                var txtcode=document.getElementById("<%=txtCityCode.ClientID%>");
                txtcode.value=city;

                var txtname=document.getElementById("<%=txtCityName.ClientID%>");
                txtname.value=select.options[select.selectedIndex].text;


                ColServices.clsServices.GetSectorCodeListnew(constr,ctry,city,FillSectorCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSectorNameListnew(constr,ctry,city,FillSectorNames,ErrorHandler,TimeOutHandler);

                break; 
            case "sectorcode":
                var select=document.getElementById("<%=ddlSectorCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlSectorName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsectorcode.ClientID%>").value = selectname.value;
                break; 

            case "sectorname":
                var select=document.getElementById("<%=ddlSectorName.ClientID%>");
                var selectname=document.getElementById("<%=ddlSectorCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsectorcode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;

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
            <td style="width: 100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Report of New Clients</td>
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
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="lblCategorycode" runat="server" Text="Category Code" CssClass="td_cell" Width="102px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlCCode" class="field_input" tabIndex=1 onchange="CallWebMethod('catcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblCategoryName" runat="server" Text="Category Name" CssClass="td_cell" Width="126px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlCatName" class="field_input" tabIndex=2 onchange="CallWebMethod('catname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblCountryCode" runat="server" Text="Country Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlContCode" class="field_input" tabIndex=3 onchange="CallWebMethod('ctrycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcountryname" runat="server" Text="Country Name" CssClass="td_cell" Width="122px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlcontName" class="field_input" tabIndex=4 onchange="CallWebMethod('ctryname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblCityCode" runat="server" Text="City Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlCityCode" class="field_input" tabIndex=5 onchange="CallWebMethod('citycode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcityname" runat="server" Text="City Name" CssClass="td_cell" Width="121px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlCityName" class="field_input" tabIndex=6 onchange="CallWebMethod('cityname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblSupplierTypeCode" runat="server" Text="Sector Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlSectorCode" class="field_input" tabIndex=7 onchange="CallWebMethod('sectorcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblsuppliertypename" runat="server" Text="Sector Name" CssClass="td_cell" Width="116px"></asp:Label></TD><TD><SELECT style="WIDTH: 238px" id="ddlSectorName" class="field_input" tabIndex=8 onchange="CallWebMethod('sectorname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_input" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" InvalidValueBlurredMessage=" Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR><TR><TD><asp:Label id="Label6" runat="server" Text="Order By" CssClass="td_cell"></asp:Label></TD><TD colSpan=3><asp:RadioButton id="rbcode" tabIndex=11 runat="server" Text="Client Code" ForeColor="Black" CssClass="search_button" Width="110px" AutoPostBack="True" BorderColor="#404040" Checked="True" GroupName="GrSearch" wfdid="w6"></asp:RadioButton>&nbsp;<asp:RadioButton id="rbname" tabIndex=12 runat="server" Text="Client Name" ForeColor="Black" CssClass="search_button" Width="110px" AutoPostBack="True" BorderColor="#404040" GroupName="GrSearch"></asp:RadioButton>&nbsp;</TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4>
<asp:Button id="BtnClear" tabIndex=13 runat="server" Text="Clear" CssClass="field_button" Width="61px"></asp:Button>&nbsp;
 <asp:Button id="btndisplay" tabIndex=14 runat="server" Text="Display" CssClass="field_button"></asp:Button>&nbsp;
  <asp:Button id="BtnPrint" tabIndex=15 runat="server" Text="Load Report" CssClass="field_button"></asp:Button>&nbsp;
  <asp:Button id="btnhelp" tabIndex=16 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="field_button"></asp:Button>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /><input id="txtCityCode" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" /><input id="txtCityName" runat="server" style="visibility: hidden;
                width: 9px; height: 3px" type="text" /></TD></TR><TR><TD colSpan=4><asp:UpdatePanel id="UpdatePanel2" runat="server"><ContentTemplate>
<asp:GridView id="gv_SearchResult" tabIndex=18 runat="server" Font-Size="10px" BackColor="White" Width="100%" CssClass="td_cell" BorderColor="#999999" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
<Columns>
<asp:BoundField DataField="custcode" HeaderText="Customer Code">
<ItemStyle Width="15%" CssClass="field_input" Wrap="False" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle CssClass="field_input" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="custname" SortExpression="custname" HeaderText="Customer Name">
<ItemStyle Width="25%"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="perincharge" SortExpression="perincharge" HeaderText="Contact Person">
<ItemStyle Width="15%"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="tel" SortExpression="tel" HeaderText="Telephone">
<ItemStyle Width="15%"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="fax" SortExpression="fax" HeaderText="Fax">
<ItemStyle Width="15%"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="email" SortExpression="email" HeaderText="Email">
<ItemStyle Width="10%" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="recommby" SortExpression="recommby" HeaderText="Recommended By">
<ItemStyle Width="10%"></ItemStyle>
</asp:BoundField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
            CssClass="lblmsg"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel></TD></TR></TBODY></TABLE> <cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date" Enabled="True"></cc1:MaskedEditExtender>
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

     <asp:HiddenField ID="hdnsptypecode" runat="server"/>
           
    
     
     <asp:HiddenField ID="hdncategory" runat="server"/>
     <asp:HiddenField ID="hdncountry" runat="server"/>
      <asp:HiddenField ID="hdncitycode" runat="server"/>
        <asp:HiddenField ID="hdnsectorcode" runat="server"/>
     <%--<asp:HiddenField ID="hdncategoryname" runat="server"/>--%>
    

</asp:Content>