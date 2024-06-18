<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AgentloginInformation.aspx.vb" Inherits="WebAdminModule_AgentloginInformation"  MasterPageFile="~/WebAdminMaster.master"  Strict="true" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="Main" runat="server" >
    <script language="javascript" type="text/javascript">
function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}

function CallWebMethod(methodType)
    {
       switch(methodType)
        {
            case "countrycode":
                var select=document.getElementById("<%=ddlCountry.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncountry.ClientID%>").value = codeid;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value  
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,codeid,null,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,codeid,null,null,FillCustomerName,ErrorHandler,TimeOutHandler);
                
                break;
             case "countryname":
                var select=document.getElementById("<%=ddlCountryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCountry.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                document.getElementById("<%=hdncountry.ClientID%>").value = codeid;
                constr=connstr.value  
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,codeid,null,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,codeid,null,null,FillCustomerName,ErrorHandler,TimeOutHandler);                
                break;
           case "citycode":
                var select=document.getElementById("<%=ddlCity.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncity.ClientID%>").value = codeid;
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,null,codeid,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,null,codeid,null,FillCustomerName,ErrorHandler,TimeOutHandler);                
                
              break;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                 var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCity.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                document.getElementById("<%=hdncity.ClientID%>").value = codeid;
                ColServices.clsServices.GetCustomerCodeAllListnew(constr,null,null,null,null,null,codeid,null,FillCustomerCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr,null,null,null,null,null,codeid,null,FillCustomerName,ErrorHandler,TimeOutHandler);                
                
               break; 
                       
            case "ddlCustCode":
                var select=document.getElementById("<%=ddlCustCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;                
                var selectname=document.getElementById("<%=ddlCustName.ClientID%>");
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                document.getElementById("<%=hdncustomer.ClientID%>").value = codeid;
                constr=connstr.value             
                selectname.value=select.options[select.selectedIndex].text;
                ColServices.clsServices.GetCustSubCodeListnew(constr,codeid,FillCustSubCodes,ErrorHandler,TimeOutHandler);                
                ColServices.clsServices.GetCustSubNameListnew(constr,codeid,FillCustSubNames,ErrorHandler,TimeOutHandler);                         
                break;
            case "ddlCustName":
                var select=document.getElementById("<%=ddlCustName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCustCode.ClientID%>");
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                document.getElementById("<%=hdncustomer.ClientID%>").value = codeid;
                constr=connstr.value             
                selectname.value=select.options[select.selectedIndex].text;
                ColServices.clsServices.GetCustSubCodeListnew(constr,codeid,FillCustSubCodes,ErrorHandler,TimeOutHandler);                      
                ColServices.clsServices.GetCustSubNameListnew(constr,codeid,FillCustSubNames,ErrorHandler,TimeOutHandler);                     
                break; 
            case "ddlSubUserCode":
                var select=document.getElementById("<%=ddlSubUserCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlSubUserName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var txtsubuser = document.getElementById("<%=txtsubuser.ClientID%>");
                document.getElementById("<%=hdnsubcustomer.ClientID%>").value = selectname.value; 
                txtsubuser.value =selectname.value;
                break;
            case "ddlSubUserName":
                var select=document.getElementById("<%=ddlSubUserName.ClientID%>");
                var selectname=document.getElementById("<%=ddlSubUserCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var txtsubuser = document.getElementById("<%=txtsubuser.ClientID%>");
                document.getElementById("<%=hdnsubcustomer.ClientID%>").value = selectname.value;
                txtsubuser.value =selectname.options[selectname.selectedIndex].text;           
                break;                   

        }
    }
    
    
 function FillCountryCodes(result)
    {
      	var ddl = document.getElementById("<%=ddlCountry.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillCountryNames(result)
    {
        var ddl = document.getElementById("<%=ddlCountryName.ClientID%>");
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
      	var ddl = document.getElementById("<%=ddlCity.ClientID%>");
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
    
    function FillCustomerCodes(result)
    {
   
      	var ddl = document.getElementById("<%=ddlCustCode .ClientID%>");
        RemoveAll(ddl)
 	    for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
         
        }
        ddl.value="[Select]";
       
        
    }

    function FillCustomerName(result)
    {
        var ddl = document.getElementById("<%=ddlCustName.ClientID%>");
 	   RemoveAll(ddl)
 	  for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
          
        }
        ddl.value="[Select]";
       
    }
    
    function FillCustSubCodes(result)
    {    
        var ddl = document.getElementById("<%=ddlSubUserCode.ClientID%>");
        for (var j = ddl.length - 1; j>=0; j--) 
        {
         ddl.remove(j);
        }        
      
        for(var i=0;i<result.length;i++)
        {      
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }        
        ddl.value="[Select]";
    }
    function FillCustSubNames(result)
    {    
	    var ddl = document.getElementById("<%=ddlSubUserName.ClientID%>"); 		
        for (var j = ddl.length - 1; j>=0; j--) 
        {
         ddl.remove(j);
        }
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

</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH:100%; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Agent Login Information" Width="100%" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="COLOR: #000000; HEIGHT: 104px"><TABLE><TBODY><TR><TD style="WIDTH: 68px; height: 21px;" class="td_cell">
    Country</TD><TD style="WIDTH: 196px; height: 21px;" class="td_cell">
    <SELECT style="WIDTH: 189px" id="ddlCountry" class="field_input" onchange="CallWebMethod('countrycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD style="WIDTH: 39px; height: 21px;" class="td_cell">Name</TD><TD style="WIDTH: 274px; height: 21px;" class="td_cell">
    <SELECT style="WIDTH: 315px" id="ddlCountryName" class="field_input" onchange="CallWebMethod('countryname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD></TR>
    <tr>
        <td class="td_cell" style="width: 68px; height: 21px">
            City</td>
        <td class="td_cell" style="width: 196px; height: 21px">
            <SELECT style="WIDTH: 193px" id="ddlCity" class="field_input" onchange="CallWebMethod('citycode');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
        </td>
        <td class="td_cell" style="width: 39px; height: 21px">
            Name</td>
        <td class="td_cell" style="width: 274px; height: 21px">
            <SELECT style="WIDTH: 315px" id="ddlCityName" class="field_input" onchange="CallWebMethod('cityname');" runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION></SELECT>
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 68px; height: 22px">
    Customer</td>
        <td class="td_cell" style="width: 196px; height: 22px">
    <select id="ddlCustCode" runat="server" class="field_input" onchange="CallWebMethod('ddlCustCode');"
        style="width: 189px" tabindex="2">
        <option selected="selected"></option>
    </select>
        </td>
        <td class="td_cell" style="width: 39px; height: 22px">
            Name</td>
        <td class="td_cell" style="width: 274px; height: 22px">
    <select id="ddlCustName" runat="server" class="field_input" onchange="CallWebMethod('ddlCustName');"
            style="width: 315px" tabindex="3">
            <option selected="selected"></option>
        </select>
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 68px; height: 22px">
            Sub User</td>
        <td class="td_cell" style="width: 196px; height: 22px">
            <select id="ddlSubUserCode" runat="server" class="field_input" onchange="CallWebMethod('ddlSubUserCode');"
                style="width: 191px" tabindex="10">
            </select>
        </td>
        <td class="td_cell" style="width: 39px; height: 22px">
            Name</td>
        <td class="td_cell" style="width: 274px; height: 22px">
            <select id="ddlSubUserName" runat="server" class="field_input" onchange="CallWebMethod('ddlSubUserName');"
                style="width: 315px" tabindex="11">
            </select>
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 68px;">
            <asp:Label ID="Label2" runat="server" CssClass="field_caption" Text="From Date" Width="62px"></asp:Label></td>
        <td class="td_cell" style="width: 196px;">
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="fiel_input" TabIndex="2" ValidationGroup="MKE"
                Width="80px"></asp:TextBox>
            <asp:ImageButton ID="ImgBtnFrmDt" runat="server"
                    ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
            <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" ControlExtender="MskFromDate"
                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator></td>
        <td class="td_cell" style="width: 39px;">
            <asp:Label ID="Label1" runat="server" CssClass="field_caption" Text="To Date&#13;&#10;"
                Width="50px"></asp:Label></td>
        <td class="td_cell" style="width: 274px;">
            <asp:TextBox ID="txtTodate" runat="server" CssClass="fiel_input" TabIndex="3" ValidationGroup="MKE"
                Width="80px"></asp:TextBox>
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                TabIndex="5" />
            <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MskChequeDate"
                ControlToValidate="txtTodate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 68px">
            <INPUT style="VISIBILITY: hidden; WIDTH: 12px; HEIGHT: 9px" id="txtconnection" type=text runat="server" />
            <INPUT style="VISIBILITY: hidden; WIDTH: 12px; HEIGHT: 9px" id="txtsubuser" type=text runat="server" /></td>
        <td class="td_cell" style="width: 196px">
            <asp:Button id="btnFillList" onclick="btnFillList_Click" runat="server" 
                Text="Search" CssClass="search_button"></asp:Button>&nbsp;<asp:Button 
                id="btnClear" onclick="btnClear_Click" runat="server" Text="Clear" 
                CssClass="search_button"></asp:Button>
            &nbsp; &nbsp;
            <asp:Button id="btnHelp" onclick="btnHelp_Click" runat="server" Text="Help" 
                CssClass="search_button"></asp:Button></td>
        <td class="td_cell" style="width: 39px">
        </td>
        <td class="td_cell" style="width: 274px">
            <asp:Button ID="btnPrint" runat="server" CssClass="field_button" OnClick="btnPrint_Click"
                TabIndex="14" Text="Report" /></td>
    </tr>
</TBODY></TABLE></TD></TR><TR><TD style="HEIGHT: 24px" class="td_cell">
<asp:GridView id="grdUploadClients" runat="server" Font-Size="10px" BackColor="White" Width="100%" CssClass="td_cell" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"  ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="S. Agent Code"><EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("agentcode") %>'></asp:TextBox>
                            
</EditItemTemplate>
<ItemTemplate>
                                <asp:Label ID="lblagentCode" runat="server" Text='<%# Bind("agentcode") %>'></asp:Label>
                            
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="agentcode" SortExpression="agentcode" HeaderText="Code"></asp:BoundField>
<asp:BoundField DataField="agentname" SortExpression="agentname" HeaderText="Customer Name"></asp:BoundField>
<asp:BoundField DataField="webusername" SortExpression="webusername" HeaderText="Web User"></asp:BoundField>
<asp:BoundField DataField="subusercode" SortExpression="subusercode" HeaderText="Sub User Code"></asp:BoundField>
<asp:BoundField DataField="sub_user_name" SortExpression="sub_user_name" HeaderText="Sub User Name"></asp:BoundField>
<asp:BoundField DataField="logindatetime" SortExpression="logindatetime" HeaderText="Login Date"></asp:BoundField>
<asp:BoundField DataField="ipaddress" SortExpression="ipaddress" HeaderText="IP Address"></asp:BoundField>

</Columns>

<RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="White" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
            CssClass="lblmsg"></asp:Label></TD></TR>
<TR><TD style="HEIGHT: 22px"><asp:Button id="btnExit" onclick="btnExit_Click" 
        runat="server" Text="Exit" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> 
            <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                TargetControlID="txtFromDate">
            </cc1:MaskedEditExtender>
            <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                TargetControlID="txtTodate">
            </cc1:MaskedEditExtender>
            <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                TargetControlID="txtFromDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="ClExChequeDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton1"
                TargetControlID="txtTodate">
            </cc1:CalendarExtender>
</contenttemplate>
    </asp:UpdatePanel>



 <asp:Hiddenfield ID="hdncountry" runat="server"/>
<asp:Hiddenfield ID="hdncity" runat="server"/>

<asp:Hiddenfield ID="hdncustomer" runat="server"/>

<asp:Hiddenfield ID="hdnsubcustomer" runat="server"/>
</asp:Content>