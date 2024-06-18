<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptviewProfitnLoss.aspx.vb" Inherits="AccountsModule_RptviewProfitnLoss" %>

<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"   TagPrefix="ews" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language ="javascript" type="text/javascript" >
        function CallWebMethod(methodType) {
            switch (methodType) {
                case "ddlCustomerCode":
                    var select = document.getElementById("<%=ddlCustomerCode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlCustomerName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value
                    ColServices.clsServices.GetCustSubCodeListnew(constr, codeid, FillCustSubCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustSubNameListnew(constr, codeid, FillCustSubNames, ErrorHandler, TimeOutHandler);
                    break;
                case "ddlCustomerName":
                    var select = document.getElementById("<%=ddlCustomerName.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlCustomerCode.ClientID%>");
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value
                    selectname.value = select.options[select.selectedIndex].text;
                    ColServices.clsServices.GetCustSubCodeListnew(constr, codeid, FillCustSubCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCustSubNameListnew(constr, codeid, FillCustSubNames, ErrorHandler, TimeOutHandler);
                    break;
                case "ddlSupplierCode":
                    var select = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplierName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSupplierName":
                    var select = document.getElementById("<%=ddlSupplierName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSupplierAgentCode":
                    var select = document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplerAgentName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSupplerAgentName":
                    var select = document.getElementById("<%=ddlSupplerAgentName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSupplierAgentCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSubUserCode":
                    var select = document.getElementById("<%=ddlSubUserCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSubUserName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlSubUserName":
                    var select = document.getElementById("<%=ddlSubUserName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSubUserCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlUserCode":
                    var select = document.getElementById("<%=ddlUserCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlUserName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "ddlUserName":
                    var select = document.getElementById("<%=ddlUserName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlUserCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }


      
        function FillCustSubCodes(result) {
            var ddl = document.getElementById("<%=ddlSubUserCode.ClientID%>");
            for (var j = ddl.length - 1; j >= 0; j--) {
                ddl.remove(j);
            }
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillCustSubNames(result) {
            var ddl = document.getElementById("<%=ddlSubUserName.ClientID%>");
            for (var j = ddl.length - 1; j >= 0; j--) {
                ddl.remove(j);
            }
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }
        function checkNumber(e) {
            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }
        }

       
</script>

    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;">
        <tr>
            <td align="center" class="field_heading">
                <asp:Label id="lblHeading" runat="server" Text="Reservation List" Width="744px" CssClass="field_heading"></asp:Label></td>
        </tr>
        <tr >
          <td > 
              &nbsp;</td>
        </tr>
      
        <tr>       
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>                   
                    <div id="Divmain" runat="server"  >
                  
<TABLE style="WIDTH: 832px" class="td_cell"><TBODY><TR><TD style="COLOR: blue" align=center colSpan=7>
    &nbsp;</TD></TR>
    <tr>
        <td align="center" colspan="7" style="COLOR: blue">
            Type few characters of code or name and click search
        </td>
    </tr>
    <TR><TD align=center colSpan=7><asp:RadioButton id="rbsearch" tabIndex=1 runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbsearch_CheckedChanged" AutoPostBack="True" GroupName="GrSearch" Checked="True">
                                </asp:RadioButton> <asp:RadioButton id="rbnadsearch" tabIndex=2 runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbnadsearch_CheckedChanged" AutoPostBack="True" GroupName="GrSearch" wfdid="w6">
                                </asp:RadioButton> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnSearch" tabIndex=26 onclick="btnSearch_Click" runat="server" Text="Search" Font-Bold="False" CssClass="search_button" Width="62px"></asp:Button> &nbsp; 
                                 <asp:Button id="btnClear" tabIndex=27 onclick="btnClear_Click" runat="server" Text="Clear" Font-Bold="False" CssClass="search_button" Width="39px">
                                </asp:Button> &nbsp; <asp:Button id="btnHelp" tabIndex=31 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button> 
    &nbsp; &nbsp;</TD></TR>
    <tr>
        <td align="center" colspan="7" style="height: 47px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="None" 
                BorderWidth="0px" Height="30px">
                &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="rbnallbooking" runat="server" Checked="True" 
                    GroupName="Grpbooking" Text="All Bookings" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="rbnbackbooking" runat="server" GroupName="Grpbooking" 
                    Text="Back Office Bookings" />
                &nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="rbnonlinebooking" runat="server" GroupName="Grpbooking" 
                    Text="Online Bookings" />
            </asp:Panel>
        </td>
    </tr>
    <TR><TD><asp:Label id="Label12" runat="server" Text="File Number" Width="100px"></asp:Label></TD><TD><INPUT style="WIDTH: 142px" id="txtReqId" class="txtbox" tabIndex=3 type=text maxLength=20 runat="server" visible="true" /></TD>
        <TD valign="middle"><INPUT id="rdbExact" class=" " type=radio name="grp" runat="server" />&nbsp;Exact<INPUT id="rdbLike" class=" " type=radio CHECKED name="grp" runat="server" />&nbsp;Like</TD>
        <TD align=center valign="middle"><asp:Label id="Label18" runat="server" Text="Filter By" Width="80px"></asp:Label></TD><TD><SELECT style="WIDTH: 150px" id="ddlFilterBy" class="drpdown" tabIndex=24 onchange="CallWebMethod('');" runat="server" visible="true"> <OPTION value="0" selected>All</OPTION> <OPTION value="1">Confirmed</OPTION> <OPTION value="2">Not Confirmed</OPTION> <OPTION value="3">Cancelled</OPTION><OPTION value="4">Pending Reconfirmation</OPTION><OPTION value="5">Reconfirmed all for invoicing</OPTION><OPTION value="6">On Time Limit</OPTION></SELECT></TD><TD><asp:Label id="Label19" runat="server" Text="Order By" Width="64px"></asp:Label> </TD><TD><SELECT style="WIDTH: 120px" id="ddlOrderBy" class="drpdown" tabIndex=25 onchange="CallWebMethod('');" runat="server" visible="true"> <OPTION value="Fileno Desc" selected>Fileno Desc</OPTION> <OPTION value="Fileno Asc">Fileno Asc</OPTION> <OPTION value="Request Date">Request Date</OPTION> <OPTION value="Check in Date">Check in Date</OPTION> <OPTION value="Check out Date">Check out Date</OPTION> <OPTION value="Customer">Customer</OPTION> <OPTION value="Supplier">Supplier</OPTION> <OPTION value="Supplier Agent">Supplier Agent</OPTION> <OPTION value="Sno">Sno</OPTION></SELECT></TD></TR><TR>
    <TD>
        <asp:Label ID="Label20" runat="server" Text="Sno" Width="100px"></asp:Label>
    </TD>
    <td>
        <INPUT style="WIDTH: 142px" id="txtsno" 
                    class="txtbox" tabIndex=3 type=text maxLength=20 runat="server" visible="true" />
    </td>
    <td>
        </td>
    <td align="center">
        
        </td>
    <td>
        &nbsp;</td>
    <td>
        Booking Status</td>
    <td>
       <%-- <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" tabIndex="29" 
            Text="Export To Excel" Width="106px" />--%>
        <select id="ddlBookingStatus" runat="server" class="drpdown" name="D1" 
             style="WIDTH: 120px" tabindex="25" visible="true">
            <option selected="selected"  value="2">All</option>
            <option value="1">Active</option>
            <option value="0">Cancelled</option>
             
        </select>
    </td>
    </TR>
    <tr>
        <td valign="top">
            <asp:Label ID="Label21" runat="server" Text="From CheckIn Date"></asp:Label>
        </td>
        <td valign="top">
           <asp:TextBox ID="txtFromDate" runat="server" CssClass="field_input" 
        Width="80px"></asp:TextBox><asp:ImageButton
        ID="ImgBtnFrmDt" runat="server" 
        ImageUrl="~/Images/Calendar_scheduleHS.png" /><br />
    <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" ControlExtender="MEFromDate"
        ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
        EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
        InvalidValueMessage="Invalid Date" 
        TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
            </td>
        <td valign="top">
            &nbsp;</td>
        <td align="center" valign="top">
            <asp:Label ID="Label22" runat="server" Text="To CheckIn Date"></asp:Label>
            </td>
        <td>  
             <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton
                ID="ImgBtnRevDate" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" /><br />
            <cc1:MaskedEditValidator ID="MEVToDate" runat="server" ControlExtender="METoDate"
                ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                InvalidValueMessage="Invalid Date" 
                TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></td>
               </td>
        <td>
            Invoice Status</td>
        <td>
           <cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender><cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender> 
            <select ID="ddlInvStatus" runat="server" class="drpdown" name="D2" 
                style="WIDTH: 120px" tabindex="25" visible="true">
                <option selected="selected" value="2">All</option>
                <option value="1">Invoiced</option>
                <option value="0">Pending to Invoice</option>
            </select></td>
    </tr>
    <tr>
        <td colspan="7">
            <asp:Panel ID="pnlSearch" runat="server" Visible="False">
                <table>
                    <tbody>
                        <tr>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label1" runat="server" Text="User" Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" style="WIDTH: 203px">
                                <select ID="ddlUserCode" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlUserCode');" style="WIDTH: 150px" tabindex="5" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell">
                                <select ID="ddlUserName" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlUserName');" style="WIDTH: 150px" tabindex="6" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label5" runat="server" Text="Customer " Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell">
                                <select ID="ddlCustomerCode" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlCustomerCode');" style="WIDTH: 150px" tabindex="7" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell">
                                <select ID="ddlCustomerName" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlCustomerName');" style="WIDTH: 150px" tabindex="8" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell" style="height: 23px">
                                <asp:Label ID="Label6" runat="server" Text="Supplier" Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" style="WIDTH: 203px; height: 23px;">
                                <select ID="ddlSupplierCode" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlSupplierCode');" style="WIDTH: 150px" tabindex="9" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell" style="height: 23px">
                                <select ID="ddlSupplierName" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlSupplierName');" style="WIDTH: 300px" tabindex="10" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell" style="height: 23px">
                                <asp:Label ID="Label7" runat="server" Text="Supplier Agent  " Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" style="height: 23px">
                                <select ID="ddlSupplierAgentCode" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlSupplierAgentCode');" style="WIDTH: 150px" 
                                    tabindex="11" visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell" style="height: 23px">
                                <select ID="ddlSupplerAgentName" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlSupplerAgentName');" style="WIDTH: 150px" 
                                    tabindex="12" visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label8" runat="server" Text="Sub User Code" Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" style="WIDTH: 203px">
                                <select ID="ddlSubUserCode" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlSubUserCode');" style="WIDTH: 150px" tabindex="13" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell">
                                <select ID="ddlSubUserName" runat="server" class="drpdown" 
                                    onchange="CallWebMethod('ddlSubUserName');" style="WIDTH: 150px" tabindex="14" 
                                    visible="true">
                                    <option selected=""></option>
                                </select>
                            </td>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label17" runat="server" Text="Customer Ref" Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell">
                                <INPUT style="WIDTH: 142px" id="txtCustRef" class="txtbox" tabIndex=15 type=text maxLength=20 runat="server" visible="true" />
                            </td>
                            <td align="left" class="td_cell">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label15" runat="server" Text="Guest First Name" Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" style="WIDTH: 203px">
                                <INPUT style="WIDTH: 142px" id="txtGFname" class="txtbox" tabIndex=16 type=text maxLength=20 runat="server" visible="true" />
                            </td>
                            <td align="left" class="td_cell">
                            </td>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label16" runat="server" Text="Guest Last Name" Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell">
                                <INPUT style="WIDTH: 142px" id="txtGLname" class="txtbox" tabIndex=17 type=text maxLength=20 runat="server" visible="true" />
                            </td>
                            <td align="left" class="td_cell">
                                <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                        height: 9px" type="text" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                &nbsp;</td>
                            <td align="left" class="td_cell" colspan="2">
                                <ews:DatePicker ID="dpFromCheckindate" runat="server" 
                                    DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="18" 
                                    Width="200px" />
                            </td>
                            <td align="left" class="td_cell">
                                &nbsp;</td>
                            <td align="left" class="td_cell" colspan="2">
                                <ews:DatePicker ID="dpToCheckindate" runat="server" 
                                    DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="19" 
                                    Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label3" runat="server" Text="From Check Out Date" Width="102px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" colspan="2">
                                <ews:DatePicker ID="dpFromCheckOut" runat="server" 
                                    DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="20" 
                                    Width="200px" />
                            </td>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label10" runat="server" Text="To Check Out Date"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" colspan="2">
                                <ews:DatePicker ID="dpTocheckOut" runat="server" DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="21" 
                                    Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label4" runat="server" Text="From Request Date"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" colspan="2">
                                <ews:DatePicker ID="dpFromReqDate" runat="server" DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="22" 
                                    Width="200px" />
                            </td>
                            <td align="left" class="td_cell">
                                <asp:Label ID="Label11" runat="server" Text="To Request Date" Width="102px"></asp:Label>
                            </td>
                            <td align="left" class="td_cell" colspan="2">
                                <ews:DatePicker ID="dpToReqDate" runat="server" DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="23" 
                                    Width="200px" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    </TBODY></TABLE>
      </div>
</ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" tabIndex="29" 
            Text="Export To Excel" Width="106px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
<TABLE><TBODY><TR><TD align=center><asp:Label id="lblMessage" runat="server" ForeColor="Red" CssClass="field_input"></asp:Label></TD></TR><TR><TD align=center><asp:GridView id="gvSearchRes" runat="server" CssClass="td_cell" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvSearchRes_PageIndexChanging" PageSize="25" OnRowCommand="gvSearchRes_RowCommand" OnRowCreated="gvSearchRes_RowCreated" OnRowDataBound="gvSearchRes_RowDataBound">
                                                    <RowStyle CssClass="grdRowstyle" Wrap="False" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sno">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsno" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "sno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReqid" runat="server" 
                                                                    Text='<%# DataBinder.Eval (Container.DataItem, "requestid") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdnPosted" runat="server" />
                                                                <asp:HiddenField ID="hdnInvoiced" runat="server" />
                                                                <asp:HiddenField ID="hdnsealed" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="65px" Wrap="True" />
                                                            <FooterStyle Wrap="True" />
                                                            <HeaderStyle Width="65px" Wrap="True" />
                                                        </asp:TemplateField> 
                                                        
                                                          <%--<asp:TemplateField HeaderText="Request Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblreqdate" runat="server" Text='<%# FormatDate(DataBinder.Eval (Container.DataItem, "requestdate")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                              <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>--%> 
                                                          <asp:TemplateField HeaderText="Arrival Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArrdate" runat="server" Text='<%# FormatDate(DataBinder.Eval (Container.DataItem, "datein")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                              <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Departure Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDepdate" runat="server" Text='<%# FormatDate(DataBinder.Eval (Container.DataItem, "dateout")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                              <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField> 
                                                        <asp:BoundField DataField="agentref" HeaderText="Customer Ref" >
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="agentname" HeaderText="Customer" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                           <asp:BoundField DataField="guestname" HeaderText="GuestName" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="usercode" HeaderText="Sales Person" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="market" HeaderText="Market" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="sellcode" HeaderText="Sell Type" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="statusBkg" HeaderText="Status" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="invcStatus" HeaderText="Invoiced" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="Printpl" CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>' >P/L</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      
                                                      <%--  <asp:TemplateField HeaderText="Invoice">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkInvoice" runat="server" CommandName="RowInvoice" CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>' >Invoice</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Date Created">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreDate" runat="server" Text='<%# FormatDate(DataBinder.Eval (Container.DataItem, "adddate")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                              <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField> 
                                                        <asp:BoundField DataField="adduser" HeaderText="User Created" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Date Modified">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblModDate" runat="server" Text='<%# FormatDate(DataBinder.Eval (Container.DataItem, "moddate")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                              <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField> 
                                                        <asp:BoundField DataField="moduser" HeaderText="User Modified" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField> 
                                                        
                                                    </Columns>
                                                    <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                                                    <HeaderStyle  CssClass="grdheader" Wrap="False" />
                                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                                </asp:GridView> 
                                                   <asp:HiddenField ID="hdnrequestid" runat="server" />
    <asp:HiddenField ID="hdnsno" runat="server" />
           
 
   
    <asp:Label id="lblMsg" runat="server" 
        Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
        Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
        CssClass="lblmsg"></asp:Label> </TD></TR></TBODY></TABLE>
</ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
             <Services>
                 <asp:ServiceReference Path="~/clsServices.asmx" />
             </Services>
</asp:ScriptManagerProxy>           <asp:HiddenField ID="hdnPrivilage" runat="server" />
    </asp:Content>



