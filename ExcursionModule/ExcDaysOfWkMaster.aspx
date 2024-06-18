<%@ Page Language="VB"  MasterPageFile="~/SubPageMaster.master"  AutoEventWireup="false" CodeFile="ExcDaysOfWkMaster.aspx.vb" Inherits="ExcDaysOfWkMaster" %>


 

<%@ Register Src="../PriceListModule/SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

     <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
     
         <%@ OutputCache location="none" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
      <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker" TagPrefix="ews" %>
     <asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
      <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script> 
    <script language="javascript" type="text/javascript">
 function checkNumber()
{
   if (event.keyCode < 45 || event.keyCode > 57)
    {
            return false;
    }
    }

    function chkendtime(starttime,timeStr, rowind) {
        var txtvalue1 = document.getElementById(timeStr);
        var txtstartime = document.getElementById(starttime);    
          
        var timRegX = /^(\d{1,2}):(\d{2})?$/;
        timevalue = txtvalue1.value;
        starttimevalue=txtstartime.value;
        var startvalue=starttimevalue.match(timRegX);
        var timevalue = timevalue.match(timRegX);

    
        if (timevalue == null) {
                    alert("Time is not in a valid format..");
            txtvalue1.value = '';
            txtvalue1.focus();
            return false;
        }
        hour = timevalue[1];
        minute = timevalue[2];
        if (hour < 0 || hour > 23) {
                alert("Hour must be between 1 and 12. (or 0 and 23 for  time)");
            txtvalue1.value = '';
            txtvalue1.focus();
            return false; chkfrmtime

        }

        if (minute < 0 || minute > 59) {
                alert("Minute must be between 0 and 59.");
            txtvalue1.value = '';
            txtvalue1.focus();
            return false;
        }
        hourstart =startvalue[1];
        if (hourstart > hour) {
            alert("Start Time is Greater than end time.");
        }

        return false;
    }

    function chkstarttime(timeStr, rowind) {

        var txtvalue1 = document.getElementById(timeStr);
              var timRegX = /^(\d{1,2}):(\d{2})?$/;
        timevalue = txtvalue1.value;
        var timevalue = timevalue.match(timRegX);
        hour = timevalue[1];
        minute = timevalue[2];
        if (hour < 0 || hour > 23) {

            alert("Hour must be between 1 and 12. (or 0 and 23 for  time)");
            txtvalue1.value = '';
            txtvalue1.focus();
            return false;
        }

        if (minute < 0 || minute > 59) {
            alert("Minute must be between 0 and 59.");
            txtvalue1.value = '';
            txtvalue1.focus();
            return false;
        }

        return false;
    }

    function disabledbox(ddldurationunit, txtstarttime, txtendtime, txtduration, rowind) {
    
       var rw = parseInt(rowind);
       var ddldurationunit = document.getElementById(ddldurationunit);
       var txtstarttime = document.getElementById(txtstarttime);
       var txtendtime = document.getElementById(txtendtime);
       var txtduration = document.getElementById(txtduration);
       if (ddldurationunit.value == 'Days') {
           txtstarttime.value = '';
           txtduration.value = '';
           txtendtime.value = '';
           txtstarttime.disabled = true;
           txtduration.disabled = true;
           txtendtime.disabled = true;
                }
        else {

            txtstarttime.disabled = false;
            txtendtime.disabled = false;
            txtduration.disabled = false;
        }
    }






    $("[id*=chkswkdaysAll]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");

            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");

            }
        });
    });


</script>

   <asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; 
    BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY>
  
    <TR><TD vAlign=top align=center width=150 colSpan=2>
    <asp:Label id="lblmainheading" runat="server" Text="Excursion Types" ForeColor="White" Width="800px" CssClass="field_heading"></asp:Label>
    </TD></TR>
        
        <TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; 
        <INPUT style="WIDTH: 274px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD style="WIDTH: 85%" vAlign=top>
   <%-- <DIV style="WIDTH: 682px" id="iframeINF" runat="server">--%>
<asp:Panel id="Panelctry" runat="server" Width="498px" GroupingText="WeekDaysDetails">
<TABLE style="WIDTH: 71%" align=center>
<TBODY>

<TR>
     <TD style="HEIGHT: 18px" class="td_cell" align="center" colSpan=2>
     <asp:Label id="lblHeading" runat="server" Text="Excursion-Days Of Week Details" 
             ForeColor="White" CssClass="field_heading" Width="426px" Height="16px"></asp:Label></TD></TR>
     <TR style="COLOR: #ff0000">
     <TD style="WIDTH: 145px; HEIGHT: 27px" class="td_cell"  colSpan=2>
         <asp:GridView ID="grdweekdays" runat="server" AutoGenerateColumns="False" BackColor="White"
             BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
             Font-Bold="true" Font-Size="12px" GridLines="Vertical" TabIndex="1" Width="298px">
             <FooterStyle CssClass="grdfooter" />
             <Columns>
                 <asp:TemplateField>
                     <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                     <HeaderTemplate>
                         <asp:CheckBox runat="server" ID="chkswkdaysAll" />
                     </HeaderTemplate>
                     <ItemTemplate>
                         <asp:CheckBox ID="chkwkdays" runat="server" />
                     </ItemTemplate>
                     <HeaderStyle HorizontalAlign="Center" Width="60px" />
                     <ItemStyle HorizontalAlign="Center" Width="60px" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="BlockSale ID" Visible="False">
                     <ItemTemplate>
                         <asp:Label ID="lblSrNo" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:BoundField DataField="wkdays" HeaderText="Days Of Week">
                     <ItemStyle HorizontalAlign="Left" />
                 </asp:BoundField>
                 <asp:TemplateField HeaderText="Start Time">
                     <ItemTemplate>
                         <asp:TextBox ID="txtstarttime" runat="server" CssClass="field_input" Width="80px"></asp:TextBox>
                         <cc1:MaskedEditExtender ID="Mestarttime" runat="server" Mask="99:99" MaskType="Time"
                             AcceptAMPM="false" TargetControlID="txtstarttime">
                         </cc1:MaskedEditExtender>
                     </ItemTemplate>
                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                     <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="End Time">
                     <ItemTemplate>
                         <asp:TextBox ID="txtendtime" runat="server" CssClass="field_input" Width="80px"></asp:TextBox>
                         <cc1:MaskedEditExtender ID="Meendtime" runat="server" Mask="99:99" MaskType="Time"
                             AcceptAMPM="false" TargetControlID="txtendtime">
                         </cc1:MaskedEditExtender>
                     </ItemTemplate>
                     <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                     <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Duration">
                     <ItemTemplate>
                         <asp:TextBox ID="txtduration" runat="server" Style="text-align: right" onkeypress="return checkNumber()"
                             CssClass="field_input" Height="16px" Width="60px"></asp:TextBox>
                     </ItemTemplate>
                 </asp:TemplateField>
                   <asp:TemplateField HeaderText="Min Hour">
                     <ItemTemplate>
                         <asp:TextBox ID="txtMinHour" runat="server" Style="text-align: right" onkeypress="return checkNumber()"
                             CssClass="field_input" Height="16px" Width="60px"></asp:TextBox>
                     </ItemTemplate>
                 </asp:TemplateField>
                   <asp:TemplateField HeaderText="Max Hour">
                     <ItemTemplate>
                         <asp:TextBox ID="txtMaxHour" runat="server" Style="text-align: right" onkeypress="return checkNumber()"
                             CssClass="field_input" Height="16px" Width="60px"></asp:TextBox>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Duration Unit">
                     <ItemTemplate>
                         <asp:DropDownList ID="ddldurationunit" runat="server" TabIndex="3" Width="80px" Height="18px">
                             <%--    <asp:ListItem Text="[Select]">
                                </asp:ListItem>--%>
                             <asp:ListItem Text="Hours" Value="Hours">
                             </asp:ListItem>
                             <asp:ListItem Text="Days" Value="Days">
                             </asp:ListItem>
                         </asp:DropDownList>
                     </ItemTemplate>
                 </asp:TemplateField>
             </Columns>
             <RowStyle CssClass="grdRowstyle" />
             <SelectedRowStyle CssClass="grdselectrowstyle" />
             <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
             <HeaderStyle CssClass="grdheader" />
             <AlternatingRowStyle CssClass="grdAternaterow" />
         </asp:GridView>
                                              <asp:HiddenField ID="hdnMainGridRowid" runat="server" />
                                              
                                                

                                            </td>
                                            </tr>
                                            
            <%--  <td>
                                  
                                          
                                         </TD>--%></TR>
                                         <tr >
                                             <td>
                                                <asp:Button ID="btncopytonextrow" runat="server" CssClass="btn" TabIndex="2" 
                                                     Text="Copy  to Next Row" Width="150px" />
                                                   </td>
                                             <td>
                                                    <asp:Button ID="btncleartimings" runat="server" CssClass="btn" TabIndex="2" 
                                                     Text="Clear Timings" Width="150px" />
                                             </td>
                                             </tr>

   <TR><TD style="WIDTH: 58px" align=left>
          <asp:Button id="BtnSave"   tabIndex="3" runat="server" Text="Save" 
            CssClass="field_button" Width="93px"></asp:Button></TD>
            <TD style="WIDTH: 230px" align=left>
            <asp:Button id="BtnCancel" tabIndex="4" onclick="BtnCancel_Click" runat="server" Text="Return to Search" CssClass="field_button">
            </asp:Button>&nbsp;
             <asp:Button id="btnHelp" tabIndex="5" onclick="btnHelp_Click" runat="server" Text="Help" CssClass="field_button">
             </asp:Button></TD></TR>
                                           <tr>
                                                                <td colspan="4"> 
                                                                    <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
                                                                        Text="Webserviceerror"></asp:Label>
                                                                </td>
                                         </tr>
    </TBODY></TABLE>&quot;</asp:Panel> 
</TD></TR></TBODY></TABLE>  
                     
   
</contenttemplate>
    </asp:UpdatePanel>

   <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>


