<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcursionStopSales.aspx.vb" Inherits="ExcursionModule_ExcursionStopSales" %>

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

 <script language="javascript" src="../TADDScript.js" type="text/javascript"></script>
<script type="text/javascript">
<!--
    //WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript">
    function chkTextLock(e) {
        return false;
    }

    function checkNumber(e) {

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }

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

                var hdnExcursionGroupCode = document.getElementById("<%=hdnExcursionGroupCode.ClientID%>");
                hdnExcursionGroupCode.value = select.options[select.selectedIndex].text;


                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value;

                ColServices.clsServices.GetExcursionTypeCodeByGroupCode(constr, codeid, FillExTypeCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetExcursionTypeNameByGroupCode(constr, codeid, FillExTypeName, ErrorHandler, TimeOutHandler);

                break;

            case "ExGrpName":
                var select = document.getElementById("<%=ddlExGrpName.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlExGrpCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value;

                var hdnExcursionGroupCode = document.getElementById("<%=hdnExcursionGroupCode.ClientID%>");
                hdnExcursionGroupCode.value = selectname.options[selectname.selectedIndex].text;

                ColServices.clsServices.GetExcursionTypeCodeByGroupCode(constr, codeid, FillExTypeCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetExcursionTypeNameByGroupCode(constr, codeid, FillExTypeName, ErrorHandler, TimeOutHandler);
                break;

              

            case "ExTypeCode":
                var select = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var hdnExcursionTypeCode = document.getElementById("<%=hdnExcursionTypeCode.ClientID%>");
                var hdnExcursionTypeName = document.getElementById("<%=hdnExcursionTypeName.ClientID%>");
                hdnExcursionTypeCode.value = select.options[select.selectedIndex].text;
                hdnExcursionTypeName.value = select.options[select.selectedIndex].value;
                break;
            case "ExTypeName":
                var select = document.getElementById("<%=ddlExTypeName.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var hdnExcursionTypeCode = document.getElementById("<%=hdnExcursionTypeCode.ClientID%>");
                var hdnExcursionTypeName = document.getElementById("<%=hdnExcursionTypeName.ClientID%>");
                hdnExcursionTypeCode.value = selectname.options[selectname.selectedIndex].text;
                hdnExcursionTypeName.value = selectname.options[selectname.selectedIndex].value;
                break;

        }
    }

    function FillExTypeCode(result) {
        var ddl = document.getElementById("<%=ddlExTypeCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillExTypeName(result) {
        var ddl = document.getElementById("<%=ddlExTypeName.ClientID%>");

        RemoveAll(ddl)
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
     

        else if (document.getElementById("<%=dpFromdate.ClientID%>").value == "") {
            document.getElementById("<%=dpFromdate.ClientID%>").focus();
            alert("From Date field can not be left blank.");
            return false;

        }
        else if (document.getElementById("<%=dpToDate.ClientID%>").value == "") {
            document.getElementById("<%=dpToDate.ClientID%>").focus();
            alert("To Date field can not be left blank.");
            return false;

        }
        else {

            if (state == 'New') 
            { if (confirm('Are you sure you want to save Excursion stop sale - allotment?') == false) return false; }
            if (state == 'Delete') {
                
             if (confirm('Are you sure you want to Delete Excursion stop sale - allotment?') == false) return false; }
             
            //     if (state=='Generate'){if(confirm('Are you sure you want to generate?')==false)return false;}

        }
 }



    function ChangeDate(fromdate, todate) {
//        txt = document.getElementById(fromdate);
//        txt1 = document.getElementById(todate);
//        var ddl = 0;

//        if (txt.value != '') {
//            var datearray = txt.value.split("/");
//            var mn = datearray[1];
//            mn = mn * 1;
//            mn = mn - 1;
//            var yDate = new Date();

//            yDate.setDate(datearray[0]);
//            yDate.setMonth(mn);
//            yDate.setYear(datearray[2]);

//            var todt = DateAdd('d', ddl, yDate);
//            var nyr = todt.getYear();
//            var nmn = todt.getMonth();
//            nmn = nmn * 1;
//            nmn = nmn + 1;
//            nmn = Right('0' + nmn, 2);
//            var ndy = Right('0' + todt.getDate(), 2);
//            txt1.value = ndy + "/" + nmn + "/" + nyr;

        txt = document.getElementById(fromdate);
        txt1 = document.getElementById(todate);

        if (txt.value != '') {
            txt1.value = txt.value;
           
        }
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
<asp:Label id="lblHeading" runat="server" Text="Add New Stop Sale" CssClass="field_heading" Width="226px"></asp:Label></TD>
</TR>
<TR>
<TD class="td_cell" colSpan=5>
<TABLE class="td_cell">
<TBODY>
<TR>
<TD class="td_cell">Excursion Group Code<SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD class="td_cell">  <SELECT style="WIDTH: 200px" id="ddlExGrpCode" 
        class="field_input" tabIndex=1  onchange="CallWebMethod('ExGrpCode');" 
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
<TD class="td_cell">Stop Sale ID</TD>
<TD><INPUT id="txtAllotmentID" disabled type=text runat="server" />
    <span style="COLOR: #ff0000">
        <asp:Button ID="btnFillmkt" runat="server" CssClass="field_button" 
            onclick="btnFillmkt_Click" Text="Fillmkt" />
    </span></TD>
    
    </TR>
    
    <TR><TD class="td_cell">From Date<SPAN style="COLOR: #ff0000">*</SPAN></TD>
    <TD class=" "><ews:DatePicker id="dpFromdate" tabIndex=12 runat="server" CssClass="field_input" Width="185px" DateRegularExpression="\d{1,2}\/\d{1,2}\/\d{4}" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>&nbsp;&nbsp;&nbsp; </TD>
    <TD class="td_cell">To Date<SPAN style="COLOR: #ff0000">*</SPAN></TD>
    <TD class=" "><ews:DatePicker id="dpToDate" tabIndex=13 runat="server" CssClass="field_input" Width="200px" DateRegularExpression="\d{1,2}\/\d{1,2}\/\d{4}" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD>
    </TR>
    <TR>
    <TD class="td_cell">Remarks</TD>
    <TD colSpan=3><TEXTAREA style="WIDTH: 500px" id="txtRemark" class="field_input" 
            runat="server" tabindex="5"></TEXTAREA></TD>
    </TR>
    <TR>
    <TD colSpan=4>
    <table style="width: 100%">
        <tr>
            <td>
                <asp:Panel ID="pnlMarket" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <strong>Market</strong></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Pnlmkt" runat="server"  ScrollBars="Auto" 
                                    Width="368px"><%--Height="150px"--%>
                                    <asp:GridView ID="gv_Mkt" runat="server" AutoGenerateColumns="False" 
                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                        CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" tabIndex="6" 
                                        Width="360px">
                                        <RowStyle CssClass="grdRowstyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="plgrpcode" HeaderText="Market Code">
                                                <HeaderStyle Width="400px" />
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="plgrpname" HeaderText="Market Name">
                                                <HeaderStyle Width="1000px" />
                                                <ItemStyle Width="1000px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle CssClass="grdfooter" />
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                        <HeaderStyle CssClass="grdheader" />
                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    &nbsp;<br />
                    <asp:CheckBox ID="chkMarket" runat="server" AutoPostBack="True" 
                        CssClass="chkbox" OnCheckedChanged="chkMarket_CheckedChanged" tabIndex="7" 
                        Text="Select / Deselect Market" Width="247px" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlRooms" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td><strong>
                                Rooms</strong></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="PnlRm" runat="server" Height="150px" ScrollBars="Auto" 
                                    Width="368px">
                                    <asp:GridView ID="gv_rm" runat="server" AutoGenerateColumns="False" 
                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                        CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" tabIndex="0" 
                                        Width="360px">
                                        <RowStyle CssClass="grdRowstyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="rmtypcode" HeaderText="Room Code">
                                                <HeaderStyle Width="400px" />
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>--%>

                                            <asp:TemplateField HeaderText="Room Code" Visible=false>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblroomcode" runat="server" Text='<%# Bind("rmtypcode") %>'></asp:Label>
                                                </ItemTemplate>
                                               
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="rmtypname" HeaderText="Room Name">
                                                <HeaderStyle Width="1000px" />
                                                <ItemStyle Width="1000px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle CssClass="grdfooter" />
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                        <HeaderStyle CssClass="grdheader" />
                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    &nbsp;<br />
                    <asp:CheckBox ID="chkRooms" runat="server" AutoPostBack="True" 
                        CssClass="chkbox" OnCheckedChanged="chkrooms_CheckedChanged" tabIndex="0" 
                        Text="Select / Deselect Room" Width="247px" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    </TD></TR>
    <tr>
        <td colspan="4">
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="4">
            <strong>Date</strong></td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Panel ID="Pnldategrid" runat="server" TabIndex="8">
                <asp:GridView ID="grdDates" runat="server" AllowSorting="True" 
                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell" 
                    Font-Size="10px" GridLines="Vertical" tabIndex="13" Width="365px">
                    <FooterStyle CssClass="grdfooter" />
                    <Columns>
                        <asp:BoundField DataField="SrNo" HeaderText="Sr No" Visible="False">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="From Date">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:TextBox ID="txtPfromdate" runat="server" CssClass="fiel_input" 
                                    Width="80px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CEPFromDate" runat="server" Format="dd/MM/yyyy" 
                                    PopupButtonID="ImgPBtnFrmDt" TargetControlID="txtPFromDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MEPFromDate" runat="server" Mask="99/99/9999" 
                                    MaskType="Date" TargetControlID="txtPFromDate">
                                </cc1:MaskedEditExtender>
                                <asp:ImageButton ID="ImgPBtnFrmDt" runat="server" 
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                <cc1:MaskedEditValidator ID="MEVPFromDate" runat="server" 
                                    ControlExtender="MEPFromDate" ControlToValidate="txtPfromdate" 
                                    CssClass="field_error" Display="Dynamic" 
                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                                    ErrorMessage="MEPFromDate" InvalidValueBlurredMessage="Invalid Date" 
                                    InvalidValueMessage="Invalid Date" 
                                    TooltipMessage="Input a date in dd/mm/yyyy format">
                                </cc1:MaskedEditValidator>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Date">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPtodate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CEPToDate" runat="server" Format="dd/MM/yyyy" 
                                    PopupButtonID="ImgPBtntoDt" TargetControlID="txtPToDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MEPToDate" runat="server" Mask="99/99/9999" 
                                    MaskType="Date" TargetControlID="txtPToDate">
                                </cc1:MaskedEditExtender>
                                &nbsp;<asp:ImageButton ID="ImgPBtntoDt" runat="server" 
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                <cc1:MaskedEditValidator ID="MEVPToDate" runat="server" 
                                    ControlExtender="MEPToDate" ControlToValidate="txtPToDate" 
                                    CssClass="field_error" Display="Dynamic" 
                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                                    ErrorMessage="MEPToDate" InvalidValueBlurredMessage="Invalid Date" 
                                    InvalidValueMessage="Invalid Date" 
                                    TooltipMessage="Input a date in dd/mm/yyyy format">
                                </cc1:MaskedEditValidator>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="grdRowstyle" />
                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="grdheader" />
                    <AlternatingRowStyle CssClass="grdAternaterow" />
                </asp:GridView>
                <asp:Button ID="btnAddLines" runat="server" CssClass="btn" tabIndex="9" 
                    Text="Add Row" />
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Button ID="btnGenerate" runat="server" CssClass="field_button" 
                onclick="btnGenerate_Click" tabIndex="10" Text="... Generate" />
            &nbsp;
            <asp:Button ID="btnStopAll" runat="server" CssClass="field_button" 
                onclick="btnStopAll_Click" tabIndex="-1" Text="Stop All" />
            &nbsp;
            <asp:Button ID="btnRemoveStopAll" runat="server" CssClass="field_button" 
                onclick="btnRemoveStopAll_Click" tabIndex="16" Text="Remove Stop All" />
        </td>
    </tr>
    </TBODY></TABLE></TD></TR><TR><TD colSpan=5><asp:Panel id="pnlAllot" runat="server" Visible="False">
    <DIV style="WIDTH: 941px; HEIGHT: 300px" class="container">
    <asp:GridView id="gv_allotment" tabIndex=11 runat="server" Font-Size="10px" 
            CssClass="td_cell" Width="925px" Visible="False" GridLines="Vertical" 
            CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" 
            AutoGenerateColumns="False" BackColor="White">
<FooterStyle CssClass="grdfooter"  ForeColor="Black"></FooterStyle>
<Columns>


<asp:TemplateField Visible="False" HeaderText="Line No"><EditItemTemplate>
<asp:TextBox id="TextBox8" runat="server" Text='<%# Bind("lineno") %>'></asp:TextBox> 
</EditItemTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
<ItemTemplate>
<asp:Label id="lblLineno" runat="server" Text='<%# Bind("lineno") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>


<asp:TemplateField Visible="False" HeaderText="Line No"><EditItemTemplate>
<asp:TextBox id="TextBox8" runat="server" Text='<%# Bind("rowid") %>'></asp:TextBox> 
</EditItemTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
<ItemTemplate>
<asp:Label id="lblrowid" runat="server" Text='<%# Bind("rowid") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>


<asp:BoundField DataField="market" HeaderText="Market">
<HeaderStyle Width="10%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othtype" HeaderText="Excursion Type Code">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othtypname" HeaderText="Excursion Type Name">
<HeaderStyle Width="25%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="allotdate" HeaderText="Allot Date">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:TemplateField HeaderText="Stop Sale">
<HeaderStyle Width="5%"></HeaderStyle>
<ItemTemplate>
&nbsp;<asp:CheckBox id="ChkStopSale" runat="server" Checked='<%# bind("stopsale") %>'></asp:CheckBox>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="stopsales"><ItemTemplate>
<asp:Label id="lblStopSale" runat="server" Text='<%# Bind("stopsale") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="White" HorizontalAlign="Left" 
            Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView></DIV></asp:Panel></TD></TR><TR><TD align=right colSpan=5>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
    <asp:Button id="btnResetAll" tabIndex=20 onclick="btnResetAll_Click" runat="server" 
            Text="Reset All" CssClass="field_button"></asp:Button>
    &nbsp; 
        <asp:Button id="btnResetForHotel" tabIndex=21 
            onclick="btnResetForHotel_Click" runat="server" Text="Reset For Same Hotel" 
            CssClass="field_button" Visible="False"></asp:Button>&nbsp;
     <asp:Button id="btnSave" tabIndex=12 onclick="btnSave_Click" runat="server" 
            Text="Save" CssClass="field_button"></asp:Button>&nbsp;
      <asp:Button id="btnExit" tabIndex=13 onclick="btnExit_Click" runat="server" 
            Text="Exit" CssClass="field_button"></asp:Button>&nbsp;
    <asp:Button id="btnHelp" style="display:none" tabIndex=20 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
    <input id="txtMarketName" runat="server" class="field_input" disabled="disabled"
        maxlength="5000" style="width: 344px; background-color: lightgoldenrodyellow"
        type="text" visible="false" />
     <asp:HiddenField ID="hdnsptypecode" runat="server"/>
      <asp:HiddenField ID="hdnsuppliercode" runat="server"/>
       <asp:HiddenField ID="hdnsupplieragentcode" runat="server"/>

           <asp:HiddenField ID="hdnExcursionTypeCode"  EnableViewState="true" runat="server" />
         <asp:HiddenField ID="hdnExcursionTypeName" EnableViewState="true" runat="server" />
          <asp:HiddenField ID="hdnExcursionGroupCode" EnableViewState="true" runat="server" />
</asp:Content>


