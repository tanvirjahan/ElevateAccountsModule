<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptExcursionPricelistSearch.aspx.vb" Inherits="NewclientsSearch"  MasterPageFile="~/ExcursionMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %><asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <%--<asp:HiddenField ID="hdncategoryname" runat="server"/>--%><%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>   <%--<asp:HiddenField ID="hdncategoryname" runat="server"/>--%>

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
                var select=document.getElementById("<%=ddlsellcode.ClientID%>");                
                var cat=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlsellname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

               
                break;
        case "catname":
                var select=document.getElementById("<%=ddlsellname.ClientID%>");                
                var cat=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlsellcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
              
                break;
    

      case "airportcode":
          var select = document.getElementById("<%=ddlgroupcode.ClientID%>");
          var cat = select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlgroupname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                ColServices.clsServices.getsubgroupfrommaingroup(constr, cat, fillflightfrom, ErrorHandler, TimeOutHandler);
 
                break; 

      case "airportname":
          var select = document.getElementById("<%=ddlgroupname.ClientID%>");
                 var cat = select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlgroupcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                ColServices.clsServices.getsubgroupfrommaingroup(constr, cat, fillflightfrom, ErrorHandler, TimeOutHandler);
 


                break;

            case "subgroup":
                var select = document.getElementById("<%=ddlsubgroup.ClientID%>");
                var selectname = document.getElementById("<%=ddlsubgroupname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "subgroupname":
                var select = document.getElementById("<%=ddlsubgroupname.ClientID%>");
                var selectname = document.getElementById("<%=ddlsubgroup.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;

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


    function ddlCatName_onclick() {

    }


    function fillflightfrom(result) {
        var ddl = document.getElementById("<%=ddlsubgroup.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }
        

    

</script> <table>
        <tr>
            <td style="width: 362px">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center; width: 751px;">
                            Report - Excursion PriceList</td>
                    </tr>
                    <tr>
                        <td style="text-align: center; width: 751px;">
                            <span class="td_cell" style="color: #ff0000"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 375px;">
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY>
                        <TR><TD><asp:Label id="Label1" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD>
    <TD><asp:TextBox id="txtfromdate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImageButton1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MaskedEditValidator1" runat="server" CssClass="field_input" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
    <TD><asp:Label id="Label2" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD>
    <TD><asp:TextBox id="txttodate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton id="ImageButton2" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MaskedEditValidator2" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" InvalidValueBlurredMessage=" Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
   
    </TR>

    <tr>
        <td>
            <asp:Label ID="lblCategorycode" runat="server" CssClass="td_cell" 
                Text="SellType code" Width="102px" style="height: 13px"></asp:Label>
        </td>
        <td>
            <select id="ddlsellcode" runat="server" class="field_input" 
                onchange="CallWebMethod('catcode')" style="WIDTH: 170px" tabindex="1" 
                visible="true">
                <option selected=""></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblCategoryName" runat="server" CssClass="td_cell" 
                Text="SellType Name" Width="126px"></asp:Label>
        </td>
        <td>
            <select id="ddlsellname" runat="server" class="field_input" 
                onchange="CallWebMethod('catname')" style="WIDTH: 237px" tabindex="2" 
                visible="true" onclick="return ddlCatName_onclick()">
                <option selected=""></option>
            </select>
        </td>
    </tr>
    <TR id ="airport" style="visibility:visible"><TD><asp:Label id="lblairportcode" runat="server" Text="Group Code" 
            CssClass="td_cell"></asp:Label></TD><TD>
            <SELECT style="WIDTH: 170px" 
                id="ddlgroupcode" class="field_input" tabIndex=3 
                onchange="CallWebMethod('airportcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD>
        <asp:Label id="lblairportname" runat="server" Text="Group Name" 
            CssClass="td_cell" Width="122px" Height="16px"></asp:Label></TD><TD>
            <SELECT style="WIDTH: 237px" id="ddlgroupname" class="field_input" tabIndex=4 
                onchange="CallWebMethod('airportname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD></TR>
                        <tr id ="sector" style="visibility:visible">
                            <td>
                                <asp:Label ID="lblsubgroup"  runat="server" CssClass="td_cell" 
                                    Text="Sub Group"></asp:Label>
                            </td>
                            <td>
                                <select id="ddlsubgroup" runat="server" class="field_input" name="D1" 
                                    onchange="CallWebMethod('subgroup')" style="WIDTH: 170px" tabindex="3" 
                                    >
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td>
                                <asp:Label ID="lblsubgroupname"  runat="server" CssClass="td_cell" 
                                    Text="Sub Group Name"></asp:Label>
                            </td>
                            <td>
                                <select id="ddlsubgroupname" runat="server" class="field_input" name="D2" 
                                    onchange="CallWebMethod('subgroupname')" style="WIDTH: 237px" tabindex="4" 
                                   >
                                    <option selected=""></option>
                                </select>
                            </td>
                        </tr>
                      
    <TR>
  


        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
  


    </TR>
   
  <table>

  </table> 

      <TR><TD>&nbsp;</TD><TD colSpan=3>&nbsp;</TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4>
<asp:Button id="BtnClear" tabIndex=13 runat="server" Text="Clear" CssClass="field_button" Width="61px"></asp:Button>&nbsp;
 <asp:Button visible ="false" id="btndisplay" tabIndex=14 runat="server" Text="Display" CssClass="field_button"></asp:Button>&nbsp;
  <asp:Button id="BtnPrint" tabIndex=15 runat="server" Text="Load Report" CssClass="field_button" 
                            onclick="BtnPrint_Click"></asp:Button>&nbsp;
  <asp:Button id="btnhelp" tabIndex=16 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="field_button"></asp:Button>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /><input id="txtCityCode" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" /><input id="txtCityName" runat="server" style="visibility: hidden;
                width: 9px; height: 3px" type="text" /></TD>
                <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
        <asp:Button id="Button1" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
   </td>
   </TR><TR><TD colSpan=4><asp:UpdatePanel id="UpdatePanel2" runat="server"><ContentTemplate>
 
</ContentTemplate>
</asp:UpdatePanel></TD></TR></TBODY></TABLE> <cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date" Enabled="True"></cc1:MaskedEditExtender>
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                </table>
                </td>
        </tr>
    </table>  
    <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

     <%--<asp:HiddenField ID="hdncategoryname" runat="server"/>--%>
    

</asp:Content>