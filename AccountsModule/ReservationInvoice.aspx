<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReservationInvoice.aspx.vb" Inherits="ReservationInvoice"  MasterPageFile ="~/SubPageMaster.master"  Strict="true" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"   TagPrefix="ews" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="Main" runat="server" >
<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>

<script language ="javascript" type="text/javascript" >

function CallWebMethod(methodType)
{
    switch(methodType)
    {
        case "ddlCustomerCode":
            var select=document.getElementById("<%=ddlCustomerCode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;   
            var selectname=document.getElementById("<%=ddlCustomerName.ClientID%>");
            var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
            constr=connstr.value             
            selectname.value=select.options[select.selectedIndex].text; 
            ColServices.clsServices.GetCustSubCodeListnew(constr,codeid,FillCustSubCodes,ErrorHandler,TimeOutHandler);                
            ColServices.clsServices.GetCustSubNameListnew(constr,codeid,FillCustSubNames,ErrorHandler,TimeOutHandler);
            break;       
        case "ddlCustomerName":
            var select=document.getElementById("<%=ddlCustomerName.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;   
            var selectname=document.getElementById("<%=ddlCustomerCode.ClientID%>");
            var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
            constr=connstr.value             
            selectname.value=select.options[select.selectedIndex].text;
            ColServices.clsServices.GetCustSubCodeListnew(constr,codeid,FillCustSubCodes,ErrorHandler,TimeOutHandler);                      
            ColServices.clsServices.GetCustSubNameListnew(constr,codeid,FillCustSubNames,ErrorHandler,TimeOutHandler);
            break;
        case "ddlSupplierCode":
            var select=document.getElementById("<%=ddlSupplierCode.ClientID%>");
            var selectname=document.getElementById("<%=ddlSupplierName.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text; 
            break;       
        case "ddlSupplierName":
            var select=document.getElementById("<%=ddlSupplierName.ClientID%>");
            var selectname=document.getElementById("<%=ddlSupplierCode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "ddlSupplierAgentCode":
            var select=document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
            var selectname=document.getElementById("<%=ddlSupplerAgentName.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text; 
            break;       
        case "ddlSupplerAgentName":
            var select=document.getElementById("<%=ddlSupplerAgentName.ClientID%>");
            var selectname=document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "ddlSubUserCode":
            var select=document.getElementById("<%=ddlSubUserCode.ClientID%>");
            var selectname=document.getElementById("<%=ddlSubUserName.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text; 
            break;       
        case "ddlSubUserName":
            var select=document.getElementById("<%=ddlSubUserName.ClientID%>");
            var selectname=document.getElementById("<%=ddlSubUserCode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "ddlUserCode":
            var select=document.getElementById("<%=ddlUserCode.ClientID%>");
            var selectname=document.getElementById("<%=ddlUserName.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text; 
            break;       
        case "ddlUserName":
            var select=document.getElementById("<%=ddlUserName.ClientID%>");
            var selectname=document.getElementById("<%=ddlUserCode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
    }
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
    
function FormValidation()
{
    var img =document.getElementById("<%=imgicon.CLientID %>");
    if (document.getElementById("<%=ddlCustomerCode.ClientID%>").value=="[Select]")
       {
            document.getElementById("<%=ddlCustomerCode.ClientID%>").focus(); 
             alert("Please select customer.");
            return false;
        }
        
     img.style.visibility="visible";
     return true; 
}
</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid" class="td_cell"><TBODY><TR><TD style="TEXT-ALIGN: center" colSpan=4><asp:Label id="lblHeading" runat="server" Text="Reservation Invoice" Width="944px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 111px">File Number </TD><TD style="WIDTH: 395px"><INPUT style="WIDTH: 184px" id="txtRequestId" class="txtbox" type=text runat="server" /></TD><TD></TD>
<TD style="WIDTH: 257px"></TD></TR><TR><TD style="WIDTH: 111px">Customer</TD><TD><SELECT style="WIDTH: 136px" id="ddlCustomerCode" class="drpdown" tabIndex=7 onchange="CallWebMethod('ddlCustomerCode');" runat="server"> </SELECT> <SELECT style="WIDTH: 220px" id="ddlCustomerName" class="drpdown" tabIndex=8 onchange="CallWebMethod('ddlCustomerName');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD>User</TD><TD lang=" "><SELECT style="WIDTH: 136px" id="ddlUserCode" class="drpdown" tabIndex=5 onchange="CallWebMethod('ddlUserCode');" runat="server"> <OPTION selected></OPTION></SELECT>
 <SELECT style="WIDTH: 220px" id="ddlUserName" class="drpdown" tabIndex=6 onchange="CallWebMethod('ddlUserName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 111px">Supplier</TD><TD><SELECT style="WIDTH: 136px" id="ddlSupplierCode" class="drpdown" tabIndex=9 onchange="CallWebMethod('ddlSupplierCode');" runat="server"> <OPTION selected></OPTION></SELECT> <SELECT style="WIDTH: 220px" id="ddlSupplierName" class="drpdown" tabIndex=10 onchange="CallWebMethod('ddlSupplierName');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD>Supplier Agnet</TD>
 <TD><SELECT style="WIDTH: 136px" id="ddlSupplierAgentCode" class="drpdown" tabIndex=11 onchange="CallWebMethod('ddlSupplierAgentCode');" runat="server"> <OPTION selected></OPTION></SELECT> <SELECT style="WIDTH: 220px" id="ddlSupplerAgentName" class="drpdown" tabIndex=12 onchange="CallWebMethod('ddlSupplerAgentName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 111px">Sub User Code</TD><TD><SELECT style="WIDTH: 136px" id="ddlSubUserCode" class="drpdown" tabIndex=13 onchange="CallWebMethod('ddlSubUserCode');" runat="server"> <OPTION selected></OPTION></SELECT>
  <SELECT style="WIDTH: 220px" id="ddlSubUserName" class="drpdown" tabIndex=14 onchange="CallWebMethod('ddlSubUserName');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD>Customer Ref</TD><TD><INPUT style="WIDTH: 176px" id="txtCustRef" class="txtbox" tabIndex=15 onkeypress="return checkNumber(event);" type=text maxLength=20 runat="server" /></TD></TR><TR><TD style="WIDTH: 111px; HEIGHT: 23px">Guest First Name</TD><TD style="HEIGHT: 23px"><INPUT style="WIDTH: 176px" id="txtFirstName" class="txtbox" tabIndex=16 type=text maxLength=20 runat="server" /></TD>
  <TD style="HEIGHT: 23px">Guest&nbsp;Last&nbsp;Name</TD><TD style="HEIGHT: 23px"><INPUT style="WIDTH: 176px" id="txtLastName" class="txtbox" tabIndex=17 type=text maxLength=20 runat="server" /></TD></TR><TR><TD style="TEXT-ALIGN: center" colSpan=4><TABLE style="WIDTH: 656px"><TBODY>
  <TR><TD style="WIDTH: 106px">From&nbsp;Check&nbsp;In&nbsp;Date</TD><TD style="TEXT-ALIGN: left"><ews:DatePicker id="dpFromCheckindate" tabIndex=18 runat="server" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD>
  <TD style="WIDTH: 106px">To Check In Date</TD><TD style="TEXT-ALIGN: left"><ews:DatePicker id="dpToCheckindate" tabIndex=19 runat="server" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD></TR>
  <TR><TD style="WIDTH: 106px">From&nbsp;Check&nbsp;Out&nbsp;Date</TD><TD style="TEXT-ALIGN: left"><ews:DatePicker id="dpFromCheckOut" tabIndex=20 runat="server" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD><TD style="WIDTH: 106px">To&nbsp;Check&nbsp;Out&nbsp;Date</TD>
  <TD style="TEXT-ALIGN: left"><ews:DatePicker id="dpTocheckOut" tabIndex=21 runat="server" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD></TR><TR><TD style="WIDTH: 106px">From&nbsp;Request&nbsp;Date</TD>
  <TD style="TEXT-ALIGN: left"><ews:DatePicker id="dpFromReqDate" tabIndex=22 runat="server" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD><TD style="WIDTH: 106px">To Request Date</TD>
  <TD style="TEXT-ALIGN: left"><ews:DatePicker id="dpToReqDate" tabIndex=23 runat="server" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD></TR></TBODY></TABLE></TD></TR><TR><TD style="WIDTH: 111px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 365px"></TD>
        <TD style="WIDTH: 97px">
           </TD><TD style="WIDTH: 415px">
            <asp:Button id="btnDisplay" runat="server" 
        Text=" Display Pending for Invoicing" CssClass="btn" Width="200px"></asp:Button>&nbsp;
         <asp:Button id="btnCancel" runat="server" Text="Return To Search" 
        CssClass="btn" Width="118px"></asp:Button>&nbsp;<asp:Button id="btnHelp" onclick="btnHelp_Click" runat="server" Text="Help" CssClass="btn"></asp:Button></TD></TR>
         <TR><TD style="TEXT-ALIGN: center" colSpan=4><IMG id="imgicon" height=25 src="../Images/loading.gif" width=400 runat="server" /></TD></TR><TR><TD style="HEIGHT: 147px" colSpan=4><DIV id="divRes" class="container" runat="server"><asp:GridView id="gvSearchResult" runat="server" CssClass="grdstyle" AutoGenerateColumns="False"><Columns>
<asp:TemplateField HeaderText="RequestId">
<ItemStyle Width="65px" Wrap="True" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="65px" Wrap="True"></HeaderStyle>
<ItemTemplate>
<asp:Label id="lblReqid" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "requestid") %>' Width="25px"></asp:Label> 
</ItemTemplate>

<FooterStyle Wrap="True"></FooterStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Status">
<ItemStyle Width="50px" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="50px"></HeaderStyle>
<ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "status") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="amended" HeaderText="Amended">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataFormatString=" {00:dd/MM/yyyy}" DataField="requestdate" HeaderText="Request Date"></asp:BoundField>
<asp:BoundField DataField="agentref" HeaderText="Customer Ref">
<ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle Width="50px" HorizontalAlign="Center"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="customer" HeaderText="Customer">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="supplier" HeaderText="Supplier">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="supagent" HeaderText="Supplier Agent">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataFormatString="{00:dd/MM/yyyy}" DataField="datein" HeaderText="Check In"></asp:BoundField>
<asp:BoundField DataFormatString="{00:dd/MM/yyyy}" DataField="dateout" HeaderText="Check Out"></asp:BoundField>
<asp:BoundField DataField="guestname" HeaderText="GuestName">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="usercode" HeaderText="Sales Person">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="market" HeaderText="Market">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="sellcode" HeaderText="Sell Type">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:ButtonField Text="Invoice" CommandName="Invoice">
<ControlStyle ForeColor="Blue"></ControlStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle" Wrap="False"></RowStyle>

<PagerStyle CssClass="grdpagerstyle" Wrap="False"></PagerStyle>

<HeaderStyle CssClass="grdheader" Wrap="False"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView></DIV>
<asp:Label id="lblMessage" runat="server" Font-Size="9pt" Font-Names="Verdana" 
        Font-Bold="True" Visible="False" CssClass="lblmsg">Records Not Found.</asp:Label></TD></TR><TR><TD style="WIDTH: 111px"></TD><TD style="WIDTH: 365px"></TD><TD style="WIDTH: 97px"></TD><TD style="WIDTH: 257px"></TD></TR><TR><TD style="WIDTH: 111px"></TD><TD style="WIDTH: 365px"></TD><TD style="WIDTH: 97px"></TD><TD style="WIDTH: 257px"></TD></TR></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
    <Services>
        <asp:ServiceReference Path="~/clsServices.asmx" />
    </Services>
</asp:ScriptManagerProxy> 
</contenttemplate>
    </asp:UpdatePanel>

</asp:Content>