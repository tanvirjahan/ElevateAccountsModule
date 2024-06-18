<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"    CodeFile="FlightsMaster.aspx.vb" Inherits="FlightsMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../TimePicker.js" type="text/javascript"></script>
    <script language="javascript " type="text/javascript">

        function checkNumber(e) {
            if (event.keyCode < 45 || event.keyCode > 57) {
                return false;
            }
        }

        function checkNumber2(e) {
            if (event.keyCode > 57 && event.keyCode < 59) {
                return true;
            }

            if (event.keyCode < 45 || event.keyCode > 57) {
                return false;
            }


        }


        function CallWebMethod(methodType) {
            switch (methodType) {
                case "ctrynamedep":
                    var select = document.getElementById("<%=ddlCityDeparture.ClientID%>");
                    var selectname = document.getElementById("<%=hdndepcity.ClientID%>");
                    hdndepcity.value = select.options[select.selectedIndex].text;
                    break;
                case "ctrynamearr":
                    var select = document.getElementById("<%=ddlCityArrival.ClientID%>");
                    var selectname = document.getElementById("<%=hdnarrcity.ClientID%>");
                    hdnarrcity.value = select.options[select.selectedIndex].text;
                    break;
            }
        }


        function FormValidation(state) {
            var chkapply = document.getElementById("<%=chkapplytofuture.ClientID%>");

            if ((document.getElementById("<%=TxtFlightnumber.ClientID%>").value == "") || (document.getElementById("<%=TxtFlightnumber.ClientID%>").value == 0)) {
                if (document.getElementById("<%=TxtFlightnumber.ClientID%>").value == "") {
                    document.getElementById("<%=TxtFlightnumber.ClientID%>").focus();
                    alert("Flight Number field can not be blank");
                    return false;
                }

            }


            if (state == 'New') { if (confirm('Are you sure you want to save Flights Master?') == false) return false; }
            if (state == 'Edit') {
                if (confirm('Are you sure you want to update Flights Master?') == false) return false;

                else {
                    if (chkapply.checked == true) {
                        if (confirm('Are you sure to apply to future bookings ?') == false) {

                            chkapply.checked = false;



                        }


                    }

                }


            }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Flights Master?') == false) return false; }
        }




//        function GetValueFromName() {

//            var ddl = document.getElementById("<%=ddlairlinename.ClientID%>");
//            ddl.selectedIndex = -1;
//            // Iterate through all dropdown items.
//            for (i = 0; i < ddl.options.length; i++) {
//                if (ddl.options[i].text == document.getElementById("<%=ddlAirline.ClientID%>").value) {
//                    // Item was found, set the selected index.
//                    ddl.selectedIndex = i;
//                    return true;
//                }
//            }
//        }

//        function GetValueFromCode() {

//            var ddl = document.getElementById("<%=ddlAirline.ClientID%>");
//            ddl.selectedIndex = -1;
//            // Iterate through all dropdown items.
//            for (i = 0; i < ddl.options.length; i++) {
//                if (ddl.options[i].text == document.getElementById("<%=ddlairlinename.ClientID%>").value) {
//                    // Item was found, set the selected index.
//                    ddl.selectedIndex = i;
//                    return true;
//                }
//            }

//        }

        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }

        function FillCodeName(type) {

            switch (type) {
                case "Code":

                    var select = document.getElementById("<%=ddlAirBorCode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlAirBorName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "Name":
                    var select = document.getElementById("<%=ddlAirBorName.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlAirBorCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }




        function FillArrivalCodeName(type) {

            switch (type) {
                case "Code":
                    var select = document.getElementById("<%=ddlarvlairports.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlarvlairportname.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                case "Name":
                    var select = document.getElementById("<%=ddlarvlairportname.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlarvlairports.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
            }
        }



        function checkdeciNumber(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46)
                return true;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }







        function checkselect(rownum) {

            var grid = document.getElementById("<%=gvweekdays.ClientID%>");
            rownum = rownum + 1;


            for (var i = 1; i < grid.rows.length; i++) {
                if (i != rownum) {
                    var cntls = grid.rows[i].cells[3].getElementsByTagName('input');
                    cntls[2].checked = false;
                }
            }



        }

        function Keepchanges(txtold, txtnew) {

            var chkapply = document.getElementById("<%=chkapplytofuture.ClientID%>");
            var lbl = document.getElementById("<%=lblapplytofuture.ClientID%>");
            if (txtold.value != txtnew.value) {
                chkapply.style.visibility = "visible";
                lbl.style.visibility = "visible";
                chkapply.enabled = true;
                chkapply.checked = true;
            }


        }



        function checkselect_grddates(rownum) {

            var grid = document.getElementById("<%=grddates.ClientID%>");
            rownum = rownum + 1;

            for (var i = 1; i < grid.rows.length; i++) {
                if (i != rownum) {
                    var cntls = grid.rows[i].cells[0].getElementsByTagName('input');
                    cntls[0].checked = false;
                }
            }



        }


    </script>
    <div>
    <table>
    <tr>
    <td>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 656px; border-bottom: gray 2px solid; height: 293px" id="TABLE1">
              
                    <tr>
                        <td style="height: 17px" class="td_cell" align="center" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Flight Master " Width="645px"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #ff0000">
                        <td style="width: 109px; height: 17px;" class="td_cell">
                            <asp:Label ID="lblflighttran_id" runat="server" Text="Flight TranId" ForeColor="#000040"
                                Width="84px"></asp:Label><span>*</span>
                        </td>
                        <td style="font-size: 12pt; width: 171px; height: 17px;" class="td_cell">
                            <input style="width: 193px" id="TxtFlighttranid" class="field_input" tabindex="0"
                                type="text" maxlength="50" runat="server" readonly="readonly" />
                        </td><td class="td_cell" style="height: 17px; width: 94px;">
                            <asp:Label ID="lblflighttype" runat="server" ForeColor="#000040" 
                                Text="Flight Type" Width="70px"></asp:Label><span >*</span> </td>
                                <td style="height: 17px" class="td_cell">
                            <asp:DropDownList ID="ddlFlightType" class="field_input" runat="server" AutoPostBack="True" 
                                CssClass="drpdown" 
                                TabIndex="1" 
                                Width="193px">
                                <asp:ListItem>[Select]</asp:ListItem>
                                <asp:ListItem>Arrival</asp:ListItem>
                                <asp:ListItem>Departure</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
           
                    <tr> 
                        <td style="display:none">
                            Select Airline
                        </td>
                        <td style="display:none">
                            <select id="ddlAirline" runat="server" class="field_input"  onchange="GetValueFromName();"
                                style="width: 198px; height: 31px;" tabindex="2" >
                                <option selected=""></option>
                            </select>
                        </td>
                        <td style="display:none">
                            Name
                        </td>
                        <td style="display:none">
                            <select id="ddlairlinename" runat="server" class="field_input"
                                style="width:193px" tabindex="3">
                                <option selected="true">test</option>
                             </select>
                        </td>
                    </tr>
                    <tr style="color: #ff0000">
                        <td style="width: 109px; height: 20px;" class="td_cell">
                            <span style="color: black">Flight Number <span style="color: #ff0033">*</span></span>
                        </td>
                        <td style="font-size: 12pt; width: 171px; height: 20px;" class="td_cell">
                            <input style="width: 193px; height: 18px;" id="TxtFlightnumber" class="field_input"
                                tabindex="4" type="text" maxlength="50" runat="server" />
                        </td>

                        <td></td><td></td>
                    </tr>
                    <tr style="color: #ff0000">
                        <td class="td_cell" style="width: 109px; height: 20px;">
                            &nbsp;</td>
                        <td class="td_cell" style="font-size: 12pt; width: 171px; height: 20px;">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr style="color: #000000">
                       
                       <td colspan="4">
                       <asp:Panel ID="Pnldeparture" BorderStyle="Double"  runat="server" Height="80px" 
                               Width="656px" BorderWidth="1px" TabIndex="5">
                           
                           <table> 
                           <thead>
                               <caption>
                                   <strong>Departure Details</strong>
                               </caption>
                               </thead>
                           <tr style="color: #000000">
                           
                        <td style="width: 109px; height: 25px" class="td_cell">
                            Origin Airport
                        </td>
                         <td class="td_cell" style="font-size: 12pt; width: 171px;">
                            <select id="ddlAirBorCode" runat="server" class="field_input"  onchange="FillCodeName('Code');"
                                style="width: 193px;" tabindex="6">
                                <option selected=""></option>
                            </select>
                        </td>

                          <td class="td_cell" style="width: 94px;">
                            Name
                        </td>
                        <td  class="td_cell"style="width: 212px; color: #000000;">
                            <select id="ddlAirBorName" runat="server" class="field_input"  onchange="FillCodeName('Name');"
                                style="width: 193px; margin-left: 0px; margin-bottom: 0px; " 
                                tabindex="7">
                                <option selected=""></option>
                            </select>&nbsp;&nbsp;
                        </td>
                       
                       
                    </tr>
                    <tr style="color: #000000">
                        <td class="td_cell" style="width: 109px;">
                            Destination
                        </td>
                       

                         <td style="font-size: 12pt; width: 171px; height: 25px" class="td_cell">
                            <input style="width: 193px" id="txtdep" class="field_input" tabindex="8" type="text"
                                maxlength="50" runat="server" />
                        </td>
                         <td class="td_cell" style="width: 94px; height: 25px">
                            City
                        </td>
                        <td style="width: 212px; color: #000000; height: 25px">
                            <select ID="ddlCityDeparture" runat="server" class="field_input" name="D1" 
                                onchange="CallWebMethod('citynameDep')" style="WIDTH: 189px; margin-bottom: 0px;" 
                                tabindex="14">
                                <option selected=""></option>
                            </select></td>
                      
                    </tr>                    
                           
                           </table>
                           
                           
                      </asp:Panel>
                 
                             <asp:Panel ID="PanelArrival"  TabIndex="5" BorderWidth="1px" BorderStyle="Double"  runat="server" Height="80px" Width="656px">
                                                      <table > 
                               <thead>
                                   
                                       <caption>
                                           <strong>Arrival Details</strong>
                                       </caption>
                                
                               </thead>
                               <tr>
                        <td style="width: 109px; height: 25px" class="td_cell">
                            Origin Airport
                        </td>
                         <td style="font-size: 12pt; width: 171px; height: 25px" class="td_cell">
                            <input style="width: 193px" id="txtarrvl" class="field_input" tabindex="6" type="text"
                                maxlength="50" runat="server" />
                        </td>
                         <td class="td_cell" style="width: 94px; height: 25px">
                            City
                        </td>
                        <td style="width: 212px; color: #000000; height: 25px">
                            <select ID="ddlCityArrival" runat="server" class="field_input" name="D2" 
                                onchange="CallWebMethod('citynamearr')" style="WIDTH: 189px" tabindex="14">
                                <option selected=""></option>
                            </select></td>
                      
                       
                       
                    </tr>
                    <tr style="color: #000000">
                        <td class="td_cell" style="width: 109px;">
                            Destination
                        </td>

                           <td class="td_cell" style="font-size: 12pt; width: 171px;">
                            <select id="ddlarvlairports" runat="server" class="field_input" onchange="FillArrivalCodeName('Code');"
                                style="width: 193px;" tabindex="8">
                                <option selected=""></option>
                            </select>
                        </td>

                          <td class="td_cell" style="width: 94px;">
                            Name
                        </td>
                        <td  class="td_cell"style="width: 212px; color: #000000;">
                            <select id="ddlarvlairportname" runat="server" class="field_input"  onchange="FillArrivalCodeName('Name');"
                                style="width: 193px; margin-left: 0px; margin-bottom: 0px; " 
                                tabindex="9">
                                <option selected=""></option>
                            </select>&nbsp;&nbsp;
                        </td>

                       

                        
                      
                    </tr>                    
                           
                           </table>
                           
                           
                            </asp:Panel>
                       
                       </td>
                    </tr>
                   
                    <tr style="color: #000000">
                        <td >
                          
                        </td>
                        <td></td>
                        
                        <td ></td>
                        <td></td>
                    </tr>
                    <tr style="color: #000000">
                        <td class="td _cell" colspan="2">
                        <strong> Week's Flight Operations</strong> </td>
                        
                        <td class="td_cell" colspan="2">
                            <strong>Enter Dates</strong></td>
                        
                    </tr>
                    <tr>
                        <td style="width: 109px;vertical-align:Top" colspan="2" class="td_cell"  >                           
                                         
    
    
     
                       <asp:GridView ID="gvweekdays"   runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Names="Verdana" Font-Size="9px"  
                            GridLines="None" Width="300px" TabIndex="10" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="chk" type="checkbox" runat="server"  tabindex="9" />
                                    </ItemTemplate>
                                </asp:TemplateField>                            
                               
                            
                               
                                   <asp:BoundField DataField="days" HeaderText="Days Of Week">
                                                   <ItemStyle HorizontalAlign="Left" />
                    
                    </asp:BoundField>
                                <asp:TemplateField HeaderText="Origin Time">
                                    <ItemTemplate>
                                    <input ID="txtarrtime" runat="server" class="td_cell" maxlength="9" 
                                            ONBLUR="validateDatePicker24hour(this)" size="9" type="text" 
                                            value='<%# Bind("origintime") %>' style ="width :50px;"  onkeypress="return checkdeciNumber(event,this);"></input>
                                    <input ID="txtarrtime_old" runat="server" class="td_cell" maxlength="9" 
                                            ONBLUR="validateDatePicker24hour(this)" size="9" type="text" 
                                            value='<%# Bind("origintime") %>' style ="width :50px; display :none ;"  onkeypress="return checkdeciNumber(event,this);"></input>

                                        
                                               
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Destination Time">
                                    <ItemTemplate>
                                      <input ID="txtdeptime" runat="server" class="td_cell" maxlength="9" 
                                            ONBLUR="validateDatePicker24hour(this)" size="9" type="text" 
                                            value='<%# Bind("destinationtime") %>' style ="width :50px;"  onkeypress="return checkdeciNumber(event,this);"></input>
                                        <input ID="txtdeptime_old" runat="server" class="td_cell" maxlength="9" 
                                            ONBLUR="validateDatePicker24hour(this)" size="9" type="text" 
                                            value='<%# Bind("destinationtime") %>' style ="width :50px; display :none;"  onkeypress="return checkdeciNumber(event,this);"></input>

                                        
                                    <input id="chktime" type="checkbox" runat="server"  />
                                    
                                    </ItemTemplate>
                                    


                                    
                                    <HeaderStyle HorizontalAlign="Center" />
                                    
                                </asp:TemplateField>
               
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#06788B" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#E3EAEB" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>

                  

                      
                        </td>
                  
                        <td style="vertical-align:Top " colspan="2" class="td_cell">
                            <asp:panel id="Panel"  Height="200px" ScrollBars="Vertical" runat="server">
    
   
                             <asp:GridView ID="grdDates" runat="server" AllowSorting="True" 
                        AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" 
                        tabIndex="11" Width="1px">
                        <FooterStyle CssClass="grdfooter" />
                        <Columns>
                         <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="chk" type="checkbox" runat="server"  tabindex="9" />
                                    </ItemTemplate>
                                </asp:TemplateField>  

                
                            <asp:TemplateField HeaderText="From Date">
                                <ItemTemplate>
                                    <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
                                    <asp:TextBox ID="txtfromDate" runat="server"   CssClass="fiel_input" Width="80px" Text ='<%# Bind("frmdate") %>'> </asp:TextBox>
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
                                    <asp:TextBox ID="txtToDate" runat="server"  CssClass="fiel_input" Width="80px" Text='<%# Bind("todate") %>'> </asp:TextBox>
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
                    </asp:GridView>
                     </asp:panel>
 
                        </td>
                       
                    </tr>
                    <tr>
                        <td style="width: 109px;" class="td_cell" colspan="2">
                           <table>
                               <tr>
                                   <td>
                                       <asp:Button ID="Btnselectall" runat="server" CssClass="btn" TabIndex="12" 
                                           Text="SelectAll" />
                                   </td>
                                   <td>
                                       <asp:Button ID="Btnunselectall" runat="server" CssClass="btn" TabIndex="12" 
                                           Text="Un Select" />
                                   </td>
                                      </tr>
                                      <tr>
                                   <td colspan="2">
                                       <asp:Button ID="Btncopy" runat="server" CssClass="btn" TabIndex="12" 
                                           Text="Copy Time to All Rows" Width="193px" />

                                   </td>

                               </tr>
                           </table>
                        </td>
                        
                        
                        <td style="width: 94px; " class="td_cell" colspan="2">
                       <table>
                               <tr>
                                   <td>
                                      
                    <asp:Button ID="btnAddLines" runat="server" CssClass="btn" 
                        onclick="btnAddLines_Click"  TabIndex="12" Text="Add Row" />
                           </td>
                                   <td>
                                       <asp:Button ID="btndeleteLines" runat="server" CssClass="btn" 
                       TabIndex="12" Text="Delete Row" onclick="btndeleteLines_Click" />
                                   </td>
                                      </tr>
                                     
                           </table>
                        </td>
                       
                    </tr>
                 
     
                    <tr>
                        <td style="width: 109px;" class="td_cell">
                            &nbsp;
                        </td>
                        <td style="width: 171px;" class="td_cell">
                            &nbsp;
                        </td>
                        <td style="width: 94px;" class="td_cell">
                        </td>
                        <td style="width: 171px;" class="td_cell">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 109px;" class="td_cell">
                            Active
                        </td>
                        <td style="width: 171px; " class="td_cell">
                            <input id="chkActive" tabindex="13" type="checkbox" checked runat="server" />
                        </td>
                        <td style="width: 94px;" class="td_cell">
                           
                        </td>
                        <td style="width: 171px; " class="td_cell">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                      
                        <td class="td_cell" colspan="4" >
                        <table>
                            <tr>
                                <td>
                                <asp:CheckBox  id="chkapplytofuture"  tabindex="13"  AutoPostBack="true"    runat="server" />
                                     </td>
                                <td>
                                         <asp:Label ID="lblapplytofuture" runat="server" 
                                        Text="Apply to All future bookings"></asp:Label>
                            
                                </td>
                                  </td>
                        <td >
                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                OnClick="btnSave_Click" TabIndex="14" Text="Save" />
                        </td>
                        <td style="width: 94px">
                            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" OnClick="btnCancel_Click"
                                TabIndex="15" Text="Return To Search" />
                        </td>
                        <td style="width: 212px">
                            <asp:Button ID="btnHelp" runat="server" CssClass="field_button" 
                                OnClick="btnHelp_Click" TabIndex="16" Text="Help" />
                        </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:HiddenField ID="hdnarrcity" runat="server" />
                                </td>
                                <td style="width: 94px">
                                    &nbsp;</td>
                                <td style="width: 212px">
                                    <asp:HiddenField ID="hdndepcity" runat="server" />
                                </td>
                    </tr>
                            </table>
                           
                           
             <%--       </tr>
                </tbody>
            </table>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
     </td>
    </tr>
    </table>
    
    </div>
</asp:Content>
