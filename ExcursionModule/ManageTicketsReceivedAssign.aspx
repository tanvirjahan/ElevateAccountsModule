<%@ Page Title="Manage Tickets Received Assign" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ManageTicketsReceivedAssign.aspx.vb" Inherits="ExcursionModule_ManageTicketsReceivedAssign" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="JavaScript" type="text/javascript" >
        window.history.forward(1);  
</script>

<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>

<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />

 <script language="javascript" src="../TADDScript.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {
        $("input[id $= 'chkCopy']").click(function () {
            if (this.checked == true) {
                $("input[id $= 'chkCopy']").removeAttr("checked");
                this.checked = true;
            }
        });

        MyAutoCustomer();
    });



    function MyAutoCustomer() {


        jQuery(".MyAutoCompleteClass").autocomplete({
            source: function (request, response) {

                var CustomerId = this.element.attr('CustomerId');

                jQuery.ajax({
                    url: "../ClsServices.asmx/CustomerAutoCompleteExcursion",

                    data: "{ para1:'" + request.term + "',para2:''}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        data = data.d;
                        if (data.length > 0) {
                            response(jQuery.map(data, function (item) {
                                return {
                                    value: item.Name,
                                    text: item.Id,
                                    CustomerId: CustomerId
                                }
                            }))

                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }

                });


            },
            minLength: 1,
            select: function (event, ui) {

                jQuery("#" + ui.item.CustomerId).val(ui.item.text);

            }







        });








       

              

    }



</script>



<script language="javascript" type="text/javascript">

      function chkTextLock(e) {
        return false;
    }

    function OnlyNumber(evt) {

        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : ((evt.which) ? evt.which : 0));

        if (charCode != 47 && (charCode > 45 && charCode < 58)) {
            return true;
        }
        if (charCode == 8) {
            return true;
        }

        return false;

    }  


    function checkCharacter(e) {
        if (event.keyCode == 32 || event.keyCode == 46)
            return;
        if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
            return false;
        }

    }

    function CallWebMethod(methodType) {
        switch (methodType) {

            case "ExGrpCode":
                var select = document.getElementById("<%=ddlExGrpCode.ClientID%>");
                codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlExGrpName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "ExGrpName":
                var select = document.getElementById("<%=ddlExGrpName.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlExGrpCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "ExTypeCode":
                var select = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "ExTypeName":
                var select = document.getElementById("<%=ddlExTypeName.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

        }
    }

    function ValidateTicketRange() {
        var txtFromNo = document.getElementById("<%=txtFromNo.ClientID%>");
        var txtToNo = document.getElementById("<%=txtToNo.ClientID%>");

        if (txtFromNo.value == "") {
            alert("From Ticket Number Cannot be Empty");
            return false;
        }

        if (txtToNo.value == "") {
            alert("To Ticket Number Cannot be Empty");
            return false;
        }

        if (parseInt(txtToNo.value) < parseInt(txtFromNo.value)) {
            alert("From Ticket Number should be less than To Ticket Number");
            return false;
        }

        return true;
        
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



    function FormValidation(state) {

        if (document.getElementById("<%=ddlExGrpCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlExGrpCode.ClientID%>").focus();
            alert("Select Excursion Group Code.");
            return false;
        }

        else if (document.getElementById("<%=ddlExTypeCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlExTypeCode.ClientID%>").focus();
            alert("Select Excursion Type Code.");
            return false;

        }


        else {

            if (state == 'New') { if (confirm('Are you sure you want to save Excursion Tickets ?') == false) return false; }
            if (state == 'EditRow') { if (confirm('Are you sure you want to update Excursion Tickets?') == false) return false; }
            if (state == 'DeleteRow') { if (confirm('Are you sure you want to Delete Excursion Tickets?') == false) return false; }

        }
    }

    function CheckTicketNo(fromticketno, toticketno) {
        txt = document.getElementById(fromticketno);
        txt1 = document.getElementById(toticketno);


        if (parseInt(txt1.value) <= parseInt(txt.value)) {
            var datearray = txt.value.split("/");
            alert("FromTicketNo should be less than ToTicketNo.");
            txt1.value = '';
            txt1.focus();
            return false;

        }
        return true;
    }

    function DateAdd(timeU, byMany, dateObj) {
        var millisecond = 1;
        var second = millisecond * 1000;
        var minute = second * 60;
        var hour = minute * 60;
        var day = hour * 24;
        var year = day * 365;

        var newDate;
        var dVal = dateObj.valueOf();
        switch (timeU) {
            case "ms": newDate = new Date(dVal + millisecond * byMany); break;
            case "s": newDate = new Date(dVal + second * byMany); break;
            case "mi": newDate = new Date(dVal + minute * byMany); break;
            case "h": newDate = new Date(dVal + hour * byMany); break;
            case "d": newDate = new Date(dVal + day * byMany); break;
            case "y": newDate = new Date(dVal + year * byMany); break;
        }
        return newDate;
    }

    function Left(str, n) {
        if (n <= 0)
            return "";
        else if (n > String(str).length)
            return str;
        else
            return String(str).substring(0, n);
    }

    function Right(str, n) {
        if (n <= 0)
            return "";
        else if (n > String(str).length)
            return str;
        else {
            var iLen = String(str).length;
            return String(str).substring(iLen, iLen - n);
        }
    }
    
</script>

<asp:UpdatePanel id="UpdatePanel1" runat="server">
<contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell">
<TBODY>
<TR>
<TD class="field_heading" align=center colSpan=5>
<asp:Label id="lblHeading" runat="server" Text="Assign Tickets To Agents" CssClass="field_heading" Width="300px"></asp:Label></TD>
</TR>
<TR>
<TD class="td_cell" colSpan=5>
<TABLE class="td_cell">
<TBODY>
<TR>
<TD class="td_cell">Excursion Group Code<SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD class="td_cell">  
    <SELECT style="WIDTH: 200px" id="ddlExGrpCode" 
class="field_input" tabIndex=0  onchange="CallWebMethod('ExGrpCode');" 
runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="td_cell">Excursion Group Name</TD><TD class="td_cell">  
<SELECT style="WIDTH: 300px" id="ddlExGrpName" class="field_input" tabIndex=2   
onchange="CallWebMethod('ExGrpName');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
</TR>

<TR>
<TD class="td_cell">Excursion Type Code<SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD class="td_cell"> <SELECT style="WIDTH: 200px" id="ddlExTypeCode" 
class="field_input" tabIndex=3   onchange="CallWebMethod('ExTypeCode');" 
runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="td_cell">Excursion Type Name</TD>
<TD>  <SELECT style="WIDTH: 300px" id="ddlExTypeName" class="field_input" 
tabIndex=4   onchange="CallWebMethod('ExTypeName');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
</TR>

<tr>
<TD class="td_cell">Ticket ID</TD>
<TD><INPUT id="txtAllotmentID" disabled type=text runat="server" tabindex="0" />
</TD>
<td class="td_cell">Date Received<SPAN style="COLOR: #ff0000">*</SPAN></td>
<td><ews:DatePicker id="dpDateRecieved" runat="server" CssClass="field_input" 
        Width="200px" DateRegularExpression="\d{1,2}\/\d{1,2}\/\d{4}" 
        RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" 
        DateFormatString="dd/MM/yyyy"></ews:DatePicker></td>   
</TR>
    

<TR>
<TD class="td_cell">Remarks</TD>
<TD colSpan="3">
    
    
   
<asp:TextBox runat="server" id="txtRemark"      class="field_input" 
MaxLength="1000" TextMode="MultiLine" Width="500px"  ></asp:TextBox></TD>
</TR>


<tr>
<td><asp:Button ID="btnFillAllTickets" CssClass="field_button" runat="server" Text="Fill All Tickets" /> </td>
<td class="td_cell" colspan="3">From Ticket No <asp:TextBox runat="server" id="txtFromNo" style="text-align:right" class="field_input" MaxLength="10"  Width="100px"  />
To Ticket No <asp:TextBox runat="server" id="txtToNo" style="text-align:right"  class="field_input" MaxLength="10"  Width="100px"  />
 <asp:Button ID="btnFillRangeTickets" CssClass="field_button" runat="server" Text="Fill Range of Tickets" Width="160px" /></td>

</tr>
    
<tr>
<td colspan="4">
<asp:Panel ID="Pnldategrid" runat="server" TabIndex="1">
<asp:GridView id="gv_row"   runat="server" Width="510px" Font-Size="10px" CssClass="td_cell " GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="False" BackColor="White">
<Columns>
<asp:TemplateField   Visible="false" HeaderText="LineNo" HeaderStyle-BackColor="#06788B"  >
<ItemTemplate  >
<asp:Label id="lblLineNo" runat="server" Text='<%# Bind("tlineno") %>'></asp:Label> 
<asp:Label id="lblTicketNo" runat="server" Text='<%# Bind("ticketno") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
                                          
                                            
<asp:TemplateField HeaderText="Select"  HeaderStyle-BackColor="#06788B" >
<ItemTemplate>
<asp:CheckBox id="chkCopy" runat="server" Width="17px"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>

 

<asp:TemplateField  HeaderText="Ticket No" HeaderStyle-BackColor="#06788B">
<ItemTemplate>
<asp:TextBox id="txtticketno" Readonly="True" Text='<%# Bind("ticketno") %>'   runat="server" class="field_input" maxlength="100" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>  



<asp:TemplateField  HeaderText="Ticket Date"  HeaderStyle-BackColor="#06788B" >
<ItemTemplate>
<asp:TextBox id="txtTicketDate" Readonly="True" Text='<%# Bind("ticketdate","{0:dd/MM/yyyy }") %>'   runat="server" CssClass="fiel_input" Width="80px" ValidationGroup="MKE" ></asp:TextBox>&nbsp;

</ItemTemplate>
</asp:TemplateField>  
                                            
                                             
<asp:TemplateField  HeaderText="Assigned To" HeaderStyle-BackColor="#06788B">
<ItemTemplate>
<input type="text"  name="accSearch"  class="field_input MyAutoCompleteClass"   style="width:98% ; font " id="accSearch"  runat="server" />
<SELECT style="WIDTH: 200px" id="ddlCustomer" class="field_input MyDropDownListCustValue"   runat="server"> <OPTION selected></OPTION></SELECT>
</ItemTemplate>
</asp:TemplateField>  

              
                                                 
</Columns>
<RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="Transparent" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> 
<asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" CssClass="lblmsg"></asp:Label> 
    &nbsp;&nbsp<asp:Button id="btnCopy" Width="20%" TabIndex="2" Visible="false"  runat="server" Text="Copy To All Rows" 
                        CssClass="field_button"></asp:Button>   &nbsp;&nbsp
                        <asp:Button id="btnRemove" Width="20%" TabIndex="3" Visible="false"  runat="server" Text="Remove All Rows" 
                        CssClass="field_button"></asp:Button> 
</asp:Panel>
</td>
</tr>


</TBODY></TABLE></TD></TR><TR><TD colSpan=5>&nbsp;</TD></TR><TR><TD align=right colSpan=5>
<input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
height: 9px" type="text" />
    &nbsp; &nbsp;
        
<asp:Button id="btnSave"  tabIndex="4" runat="server" 
Text="Save" CssClass="field_button"></asp:Button>&nbsp;
<asp:Button id="btnExit" tabIndex="5" onclick="btnExit_Click" runat="server" 
Text="Exit" CssClass="field_button"></asp:Button>&nbsp;
<asp:Button id="btnHelp"  onclick="btnHelp_Click" runat="server" 
        Text="Help" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
</asp:UpdatePanel>


<script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        // Place here the first init of the autocomplete
        MyAutoCustomer();
    });

    function InitializeRequest(sender, args) {

    }

    function EndRequest(sender, args) {
        // after update occur on UpdatePanel re-init the Autocomplete
        MyAutoCustomer();
    }
 </script>
</asp:Content>
