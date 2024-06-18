<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PricequerySearch.aspx.vb" Inherits="PricequerySearch"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

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
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,null,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,null,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetRoomTypeCodeListnew(constr,sptype,null,FillRoomTypeCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr,sptype,null,FillRoomTypeNames,ErrorHandler,TimeOutHandler);
                break;
           case "sptypename":
                var select=document.getElementById("<%=ddlSpTypeName.ClientID%>");
                var sptype=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsptypecode.ClientID%>").value = sptype;  

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSupplierCodeAllListnew(constr,sptype,null,null,null,null,FillSupplierCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSupplierNameAllListnew(constr,sptype,null,null,null,null,FillSupplierNames,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetRoomTypeCodeListnew(constr,sptype,null,FillRoomTypeCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr,sptype,null,FillRoomTypeNames,ErrorHandler,TimeOutHandler);
                break;
            case "suppliercode":
                var select=document.getElementById("<%=ddlPartyCode.ClientID%>"); 
                var party=select.options[select.selectedIndex].text;               
                var selectname=document.getElementById("<%=ddlPartyName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value = party;
                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetRoomTypeCodeListnew(constr,sptype,party,FillRoomTypeCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr,sptype,party,FillRoomTypeNames,ErrorHandler,TimeOutHandler);
                break;
            case "suppliername":
                var select=document.getElementById("<%=ddlPartyName.ClientID%>");                
                var party=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlPartyCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnsuppliercode.ClientID%>").value = party;
                var selectsptype=document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                var sptype=selectsptype.options[selectsptype.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetRoomTypeCodeListnew(constr,sptype,party,FillRoomTypeCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetRoomTypeNameListnew(constr,sptype,party,FillRoomTypeNames,ErrorHandler,TimeOutHandler);
                break;       
            case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var plgrp=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnmarketcode.ClientID%>").value = selectname.value;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSellCodeListnew(constr,plgrp,FillSellCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellNameListnew(constr,plgrp,FillSellNames,ErrorHandler,TimeOutHandler);
                break; 
            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var plgrp=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnmarketcode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   

                ColServices.clsServices.GetSellCodeListnew(constr,plgrp,FillSellCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetSellNameListnew(constr,plgrp,FillSellNames,ErrorHandler,TimeOutHandler);
                break;
         case "sellcode":
                var select=document.getElementById("<%=ddlSellingCode.ClientID%>");         
                var sellcat=select.options[select.selectedIndex].text;                       
                var selectname=document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnselltype.ClientID%>").value = sellcat;
                break;
          case "sellname":
                var select=document.getElementById("<%=ddlSellingName.ClientID%>");      
                var sellcat=select.options[select.selectedIndex].value;                                                 
                var selectname=document.getElementById("<%=ddlSellingCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnselltype.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                break;       
            case "rmtypcode":
                var select=document.getElementById("<%=ddlRmtypeCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlRmtypename.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnroomtypecode.ClientID%>").value = selectname.value;
                break; 

            case "rmtypname":
                var select=document.getElementById("<%=ddlRmtypename.ClientID%>");
                var selectname=document.getElementById("<%=ddlRmtypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnroomtypecode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                break;

            case "mealcode":
                var select = document.getElementById("<%=ddlMealCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlMealName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnselltype.ClientID%>").value = selectname.value;
                break; 

            case "mealname":
                var select=document.getElementById("<%=ddlMealName.ClientID%>");
                var selectname=document.getElementById("<%=ddlMealCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdnselltype.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                break;
               
        }
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

function FillRoomTypeCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlRmtypeCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }
function FillRoomTypeNames(result)
    {
    	var ddl = document.getElementById("<%=ddlRmtypename.ClientID%>");
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
                            Query on Prices</td>
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
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR><TR><TD><asp:Label id="lblSupplierTypeCode" runat="server" Text="Supplier Type Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlSPTypeCode" class="drpdown" tabIndex=3 onchange="CallWebMethod('sptype')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblsuppliertypename" runat="server" Text="Supplier Type Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 238px" id="ddlSpTypeName" class="drpdown" tabIndex=4 onchange="CallWebMethod('sptypename')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblSupplierCode" runat="server" Text="Supplier Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlPartyCode" class="drpdown" tabIndex=5 onchange="CallWebMethod('suppliercode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblSupplierName" runat="server" Text="Supplier Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlPartyName" class="drpdown" tabIndex=6 onchange="CallWebMethod('suppliername')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="Label2" runat="server" Text="Room Type Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlRmtypeCode" class="drpdown" tabIndex=7 onchange="CallWebMethod('rmtypcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="Label3" runat="server" Text="Room Type Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlRmtypename" class="drpdown" tabIndex=8 onchange="CallWebMethod('rmtypname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblCountryCode" runat="server" Text="Meal Plan Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlMealCode" class="drpdown" tabIndex=9 onchange="CallWebMethod('mealcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblcountryname" runat="server" Text="Meal Plan Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlMealName" class="drpdown" tabIndex=10 onchange="CallWebMethod('mealname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblMarketCode" runat="server" Text="Market Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlMarketCode" class="drpdown" tabIndex=11 onchange="CallWebMethod('marketcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblMarketName" runat="server" Text="Market Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlMarketName" class="drpdown" tabIndex=12 onchange="CallWebMethod('marketname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblSellingCategoryCode" runat="server" Text="Selling Type Code" CssClass="td_cell" Width="123px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlSellingCode" class="drpdown" tabIndex=13 onchange="CallWebMethod('sellcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblsellingcategoryname" runat="server" Text="Selling Type Name" CssClass="td_cell" Width="124px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlSellingName" class="drpdown" tabIndex=14 onchange="CallWebMethod('sellname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4>
<asp:Button id="BtnClear" tabIndex=17 runat="server" Text="Clear" CssClass="btn" Width="61px"></asp:Button>&nbsp;
<asp:Button id="btndisplay" tabIndex=15 runat="server" Text="Display" CssClass="btn"></asp:Button>&nbsp;
 <asp:Button id="BtnPrint" tabIndex=16 runat="server" Text="Load Report" CssClass="btn"></asp:Button>&nbsp;
<asp:Button id="btnhelp" tabIndex=17 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="btn"></asp:Button>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD></TR><TR><TD colSpan=4><asp:UpdatePanel id="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv_SearchResult" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical"
                        TabIndex="18" Width="100%">
                        <FooterStyle CssClass="grdfooter"/>
                        <Columns>
                            <asp:BoundField DataField="ratetype" HeaderText="Rate Type">
                                <ItemStyle CssClass="field_input" HorizontalAlign="Left" Width="15%" Wrap="False" />
                                <HeaderStyle CssClass="field_input" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="rmcatcode" HeaderText="Room Category" SortExpression="rmcatcode" >
                                <ItemStyle Width="25%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price" >
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="frmdate" HeaderText="From Date" SortExpression="frmdate" >
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="todate" HeaderText="To Date" SortExpression="todate" >
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="nights" HeaderText="Nights" HtmlEncode="False"
                                SortExpression="nights" >
                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle CssClass="grdRowstyle" />
                        <SelectedRowStyle CssClass="grdselectrowstyle"/>
                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="grdheader" ForeColor="White"/>
                        <AlternatingRowStyle CssClass="grdAternaterow"/>
                    </asp:GridView>
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Verdana" 
                        Font-Size="8pt" Text="Records not found. Please redefine search criteria" Visible="False"
                        Width="357px" CssClass="lblmsg"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel> </TD></TR></TBODY></TABLE> <cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender>
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
      
       <asp:HiddenField ID="hdnselltype" runat="server"/>
       <asp:HiddenField ID="hdnmealtype" runat="server"/>
       <asp:HiddenField ID="hdnsuppliercode" runat="server"/>
       <asp:HiddenField ID="hdnroomtypecode" runat="server"/>
       <asp:HiddenField ID="hdnmarketcode" runat="server"/>
     

</asp:Content>