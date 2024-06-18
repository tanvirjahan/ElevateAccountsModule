<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OthPriceList2.aspx.vb" Inherits="TransportModule_OthPriceList2" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<style type="text/css">
.TextRight 
{
    text-align:right;
}
</style>

<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript" >



    function chkTextLock(evt) {
        return false;
    }
    function chkTextLock1(evt) {
        if (evt.keyCode = 9) {
            return true;
        }
        else {
            return false;
        }
        return false;

    }
        function checkNumber(evt) {
       
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
       
        if (charCode != 47 && (charCode > 44 && charCode < 58)) {
            return true;
        }
        return false;
    }

//     function checkNumber(evt)
//      {
//        
//         var charCode = (evt.which) ? evt.which : event.keyCode
//         if (charCode != 47 && (charCode > 44 && charCode < 58))
//            return true;

//         return false;
//      }


//    function checkNumber1(evt) {
//        evt = (evt) ? evt : event;
//        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
//        ((evt.which) ? evt.which : 0));
//        if ((charCode > 47 && charCode < 58)) {
//            //if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
//            return true;
//        }
//        return false;
//    }

//    function checkNumber2(evt) {
//        evt = (evt) ? evt : event;
//        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
//        ((evt.which) ? evt.which : 0));
//        if ((charCode > 46 && charCode < 58)) {

//            return true;
//        }
//        return false;
//    }


    function checkTelephoneNumber(e) {

        if ((event.keyCode < 45 || event.keyCode > 57)) {
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

            case "sptypecode":
                var select = document.getElementById("<%=ddlSPType.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeListnew(constr, codeid, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameListnew(constr, codeid, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
            case "sptypename":
                var select = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSPType.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeListnew(constr, codeid, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameListnew(constr, codeid, FillSupplierNames, ErrorHandler, TimeOutHandler);
                break;


            case "Groupcd":
                var select = document.getElementById("<%=ddlGroupcd.ClientID%>");
                codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlGroupnm.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupOthGroupCodeListnew(constr, codeid, FillGroupCodesGroup, ErrorHandler, TimeOutHandler);

                break;

            case "Groupnm":
                var select = document.getElementById("<%=ddlGroupnm.ClientID%>");
                codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlGroupcd.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                codeid = select.options[select.selectedIndex].value;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value


            case "Airportcd":
                var select = document.getElementById("<%=ddlexccode.ClientID%>");
                var selectname = document.getElementById("<%=ddlexcname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "Airportnm":
                var select = document.getElementById("<%=ddlexcname.ClientID%>");
                var selectname = document.getElementById("<%=ddlexccode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            

        }
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

    function chkshft_onclick(chk, ap, sc) {

       
            //var arp = document.getElementById('<%'=airport.ClientID%>');
            //var sec = document.getElementById('<%'=sector.ClientID%>');
            var arp = document.getElementById(ap);
            var sec = document.getElementById(sc);
            if (chk.checked) {
                if (!arp) {
                    alert("Could not find object");
                }
                arp.style.visibility = "hidden";
                sec.style.visibility = "visible";
            }
            else {
                sec.style.visibility = "hidden";
                arp.style.visibility = "visible";
            }
        }

       

</script>

<asp:UpdatePanel id="UpdatePanel1" runat="server">
   <contenttemplate>
   <TABLE style="BORDER-RIGHT: gray 2pt solid; BORDER-TOP: gray 2pt solid; BORDER-LEFT: gray 2pt solid; BORDER-BOTTOM: gray 2pt solid">
     <TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" class="field_heading" align=left colSpan=4>
          <asp:Label id="lblHeading" runat="server" Text="Add New Other Service price List" CssClass="field_heading" Width="744px"></asp:Label></TD></TR>
     
     <tr><td style="width: 442px" class="td_cell" align=left>
        <SPAN style="FONT-FAMILY: Arial">PL Code</SPAN></TD><TD style="WIDTH: 122px">
            <INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD>
            <TD style="WIDTH: 190px" class="td_cell" align=left></TD>
            <TD></TD></TR><TR><TD style="WIDTH: 442px; HEIGHT: 3px" class="td_cell" 
             align=left>
            <SPAN style="FONT-FAMILY: Arial">Selling &nbsp;Type Code&nbsp;<SPAN style="COLOR: #ff0000">* </SPAN></SPAN></TD>
            <td style="WIDTH: 122px; HEIGHT: 3px"><SELECT style="WIDTH: 200px" id="ddlSPType" class="field_input" tabIndex=2 onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
            <td style="WIDTH: 190px; HEIGHT: 3px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">
                Selling &nbsp;Type Name</SPAN></TD>
            <td style="HEIGHT: 3px"><SELECT style="WIDTH: 300px" id="ddlSPTypeName" class="field_input" tabIndex=3 onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
          
      <tr><td style="WIDTH: 442px" class="td_cell" align=left>Group Code<SPAN style="FONT-FAMILY: Arial"> <SPAN style="COLOR: #ff0000">*</SPAN></SPAN></TD>
          <td style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlGroupCd" class="field_input" tabIndex=6 onchange="CallWebMethod('Groupcd');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
          <td style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">
              Group Name</SPAN></TD>
          <td><SELECT style="WIDTH: 300px" id="ddlGroupNm" class="field_input" tabIndex=7 onchange="CallWebMethod('Groupnm');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>

          <tr id="airport" ><td style="WIDTH: 442px" 
                  class="td_cell" align=left> <asp:Label id ="lblexccode" runat ="server" Visible ="false">Excursion Code</asp:Label> </TD>
            <td style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlexccode" visible ="false"
                    class="field_input" tabIndex=4 onchange="CallWebMethod('airportcd');" 
                    runat="server" onclick="return ddlAirportCode_onclick()"> <OPTION selected></OPTION></SELECT></TD>
            <td style="WIDTH: 190px" class="td_cell" align=left><asp:Label ID ="lblexcname" runat ="server" Visible ="false">Excursion Name</asp:Label> </TD><TD>
              <SELECT style="WIDTH: 300px" id="ddlexcname" class="field_input" tabIndex=5 visible ="false"
                  onchange="CallWebMethod('airportnm');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>

    <tr><TD style="WIDTH: 442px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Currency Code</SPAN> </SPAN></TD>
        <TD style="WIDTH: 122px"><asp:TextBox id="txtCurrCode" tabIndex=12 runat="server" CssClass="field_input"  Width="194px"></asp:TextBox></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Currency Name</SPAN></TD>
        <TD><asp:TextBox id="txtCurrName" tabIndex=13 runat="server" CssClass="field_input" Width="294px"></asp:TextBox></TD></TR>
        
    <tr><TD style="WIDTH: 442px" class="td_cell" align=left>&nbsp;</TD>
        <TD>&nbsp;</TD>
         <TD style="WIDTH: 201px; HEIGHT: 36px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Remark</SPAN></TD>  
        <TD style="WIDTH: 183px; HEIGHT: 36px" align=left>
            <TEXTAREA style="WIDTH: 319px; HEIGHT: 29px" id="txtRemark" class="field_input" 
                tabIndex=18 runat="server"></TEXTAREA></TD>
                <td>&nbsp;</td>
                <caption>
                     <input id="txtconnection" runat="server" 
                         style="visibility: hidden; width: 12px; height: 9px" type="text" />
                </caption>
         </TR>

               
  
   <TR><TD style="WIDTH: 442px" class="td_cell" align=left><%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                    <INPUT id="ChkActive" visible =false tabIndex=19 type=checkbox CHECKED runat="server" />
<asp:Label ID='lblactive' Text='Active' Visible =false  runat="server" ></asp:Label></TD><TD style="WIDTH: 183px"  class="td_cell" align=left colSpan=3>
   

    <%--<asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
        Visible="False" />--%>
           &nbsp;</TD></TR><%--<TR><TD style="HEIGHT: 22px" class="td_cell" align=right colSpan=4>
    &nbsp;<asp:Button id="btnGenerate" tabIndex=20 onclick="btnGenerate_Click" runat="server" Text="Generate" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnCancel" tabIndex=21 onclick="btnCancel_Click" 
            runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnhelp" tabIndex=22 onclick="btnhelp_Click" runat="server" 
            Text="Help" Height="20px" CssClass="field_button"></asp:Button></TD></TR>--%>
            
        <%--  </TBODY></TABLE>--%>
        <TR><TD style="HEIGHT: 22px" class="td_cell" align=left colSpan=4>
          <TABLE style="WIDTH: 864px"><TBODY><TR>
            <TD style="WIDTH: 449px" class="td_cell" valign="top">
                <strong>Enter Dates</strong></TD>
            <td style="WIDTH: 351px">
                &nbsp;</td>
            </TR>
            <tr>
                <td style="WIDTH: 300px" valign="top">
                  <asp:Panel ID="Panel4" runat="server" Height="210px" ScrollBars="Auto" >
                    <asp:GridView ID="grdDates" runat="server" AllowSorting="True" 
                        AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" 
                        tabIndex="12" Width="1px">
                        <FooterStyle CssClass="grdfooter" />
                        <Columns>
                            <asp:BoundField DataField="SrNo" HeaderText="Sr No" Visible="False">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="From Date">
                                <ItemTemplate>
                                    <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px">
                                    </asp:TextBox>
                                    <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                                        PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtfromDate">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                                        MaskType="Date" TargetControlID="txtfromDate">
                                    </cc1:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                        ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <br />
                                    <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                                        ControlExtender="MeFromDate" ControlToValidate="txtfromDate" 
                                        CssClass="field_error" Display="Dynamic" 
                                        EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                                        ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                                        InvalidValueMessage="Invalid Date" 
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </cc1:MaskedEditValidator>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Date">
                                <ItemTemplate>
                                    <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px">
                                    </asp:TextBox>
                                    <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" 
                                        PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" 
                                        MaskType="Date" TargetControlID="txtToDate">
                                    </cc1:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                                        ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <br />
                                    <cc1:MaskedEditValidator ID="MevToDate" runat="server" 
                                        ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                                        Display="Dynamic" EmptyValueBlurredText="Date is required" 
                                        EmptyValueMessage="Date is required" ErrorMessage="MeToDate" 
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </cc1:MaskedEditValidator>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="grdRowstyle" />
                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="grdheader" />
                        <AlternatingRowStyle CssClass="grdAternaterow" />
                    </asp:GridView></asp:Panel>
                    <br />
                    <asp:Button ID="btnAddLines" runat="server" CssClass="btn" 
                        onclick="btnAddLines_Click" Text="Add Row" />

                    <br />
                </td>

            </tr>
            <%--<TR><TD style="WIDTH: 449px">
                &nbsp;</TD>
                <td style="WIDTH: 351px">
                    &nbsp;</td>
                <td style="WIDTH: 351px">
                    &nbsp;</td>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 449px" valign="top">
                    <asp:Button ID="btnChkDate" runat="server" CssClass="btn" 
                        Text="Check Duplicate Dates" Visible="False" Width="162px" />
                </td>
            </TR>--%>
            <tr>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 351px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 351px" valign="top">
                    &nbsp;&nbsp;
                </td>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
            </tr>
            </TBODY></TABLE> </TD> </TR> 

        <tr>
                <td style="WIDTH: 442px" class="td_cell" valign="top"><strong>Enter Rates</strong></td></tr> 
                <tr>

                    <td align="left" class="td_cell" colSpan="4" style="HEIGHT: 22px">
                        <asp:Panel ID="Panel1" runat="server"   
                            ScrollBars="Auto" Width="900px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td >
                                            <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                                CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                                                tabIndex="20" >
                                                <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                                                <Columns>
                                                <asp:TemplateField><EditItemTemplate>
                                                <asp:TextBox id="TextBox1" runat="server" CssClass="field_input"></asp:TextBox> 
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                <INPUT id="ChkSelect" type=checkbox name="ChkSelect" runat="server" />
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:BoundField DataField="Sr No" HeaderText="Sr No" ></asp:BoundField>
                                                    <asp:BoundField DataField="Service Type Code" HeaderText="Service Type Code">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Service Type Name"  HeaderText="Service Type Name">
                                                       
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" 
                                                    HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" Font-Bold="True" ForeColor="white" 
                                                    HorizontalAlign="Left" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr> 
                <td style="width: 442px">                     <asp:Button ID="btnPreviousRates" runat="server" CssClass="btn" 
                        onclick="btnPreviousRates_Click" Text="Copy Rates to next line" Width="196px" />
                                  </td>
                </tr>
                <tr>
                    <td align="left" class="td_cell" colSpan="4" style="HEIGHT: 22px">
                        <table style="WIDTH: 951px">
                            <tbody>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Verdana" 
                                            Font-Size="8pt" Visible="False" Width="734px"></asp:Label>
                                    </td>
                                </tr>
          
                                <tr>
                                    <td style="WIDTH: 133px; HEIGHT: 23px">
                                        <table align="left" class="td_cell" 
                                            style="BORDER-RIGHT: blue 1px solid; BORDER-TOP: blue 1px solid; BORDER-LEFT: blue 1px solid; WIDTH: 356px; BORDER-BOTTOM: blue 1px solid">
                                            <tbody>
                                                <tr>
                                                    <td style="WIDTH: 100px">
                                                        -1 Incl</td>
                                                    <td style="WIDTH: 100px">
                                                        -2 N/Incl</td>
                                                    <td style="WIDTH: 100px">
                                                        -3 Free</td>
                                                    <td style="WIDTH: 100px">
                                                        -4 N/A</td>
                                                    <td style="WIDTH: 100px">
                                                        -5 On Request</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td>
                                       
                                                        <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" 
                                                            Text="Approve/Unapprove" />
                                                    
                                    </td>
                                    <td>
                                        <asp:Button ID="BtnClearFormula" runat="server" CssClass="field_button" 
                                            onclick="BtnClearFormula_Click" tabIndex="23" Text="Clear Prices" />
                                        &nbsp;
                                       
                                    </td>
                                </tr>
                                <tr >
                                
                                <td></td>
                                <td></td>
                                <td> <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                            onclick="btnSave_Click" tabIndex="21" Text="Save" />
                                        &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                            onclick="btnCancel_Click" tabIndex="22" Text="Return to Search" />
                                        &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" Height="20px" 
                                            onclick="btnhelp_Click" tabIndex="23" Text="Help" />
                                        &nbsp;<asp:Button ID="btnselling" runat="server" CssClass="btn" tabIndex="18" 
                                                Text="View Selling" Visible="False" /></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
               
         
</contenttemplate>
</asp:UpdatePanel>


</asp:Content>

