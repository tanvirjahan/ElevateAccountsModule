<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OthPolicy.aspx.vb" Inherits="PriceListModule_OthPolicy" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


<script language="javascript" type="text/javascript" >

    function FormValidation(state) {
        if ((document.getElementById("<%=ddlGroupCode.ClientID%>").value == "[Select]")) {
            if (document.getElementById("<%=ddlGroupCode.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlGroupCode.ClientID%>").focus();
                alert("Please Select Group Code.");
                return false;
            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save  Policy?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update  Policy?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete  Policy?') == false) return false; }
        }
    }
    function GetValueGroupName() {

        var ddl = document.getElementById("<%=ddlGrpName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlGroupCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

    function GetValueGroupCode() {
        var ddl = document.getElementById("<%=ddlGroupCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlGrpName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

 

    
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<table style="border-right : gray thin solid; border-top : gray thin solid; border-left : gray thin solid; WIDTH: 353px; BORDER-BOTTOM: gray thin solid" border=1>
<TBODY>
    <TR>
        <TD style="WIDTH: 774px; TEXT-ALIGN: center" class="field_heading">
            <asp:Label id="lblCustCatHead" runat="server" Text=" Policy" Width="742px"></asp:Label>
        </TD>
    </TR>
    <TR>
        <TD style="WIDTH: 774px" class="td_cell">
            <TABLE style="WIDTH: 650px" class="td_cell">
                <TBODY>
                    <TR>
                        <TD style="WIDTH: 50px; HEIGHT: 22px">ID&nbsp;</TD>
                        <TD style="WIDTH: 208px; COLOR: #000000; HEIGHT: 22px">
                            <INPUT style="WIDTH: 200px" id="txtTransID" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /></TD>
                        <TD style="WIDTH: 64px; COLOR: #000000; HEIGHT: 22px"></TD>
                        <TD style="WIDTH: 264px; COLOR: #000000; HEIGHT: 22px; TEXT-ALIGN: left"></TD>
                   </TR>
                   <TR>
                        <TD style="WIDTH: 50px; HEIGHT: 22px">Group<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
                        <TD style="WIDTH: 208px; COLOR: #000000; HEIGHT: 22px"><SELECT onblur="GetValueGroupName()" style="WIDTH: 206px" id="ddlGroupCode" class="field_input" tabIndex=2 runat="server"> <OPTION selected></OPTION></SELECT></TD>
                        <TD style="WIDTH: 64px; COLOR: #000000; HEIGHT: 22px">Group&nbsp;Name</TD><TD style="WIDTH: 264px; COLOR: #000000; HEIGHT: 22px; TEXT-ALIGN: left"><SELECT onblur="GetValueGroupCode()" style="WIDTH: 270px" id="ddlGrpName" class="field_input" tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT></TD>
                   </TR>
                   <TR style="COLOR: #000000">
                       <TD style="WIDTH: 201px" class="td_cell" align=left colSpan=2>
                      
                                </TD>
                        <%--<TD style="WIDTH: 50px">Market<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
                        <TD style="WIDTH: 208px; COLOR: #000000">
                            <SELECT onblur="GetValueMarketName()" style="WIDTH: 206px" id="ddlMarketCode" class="field_input" tabIndex=4 runat="server"> <OPTION selected></OPTION></SELECT></TD>
                        <TD style="WIDTH: 64px; COLOR: #000000">Market&nbsp;Name</TD>
                        <TD style="WIDTH: 264px; COLOR: #000000; TEXT-ALIGN: left">
                            <SELECT onblur="GetValueMarketCode()" style="WIDTH: 270px" id="ddlMarketName" class="field_input" tabIndex=5 runat="server"> <OPTION selected></OPTION></SELECT></TD>--%>
                   </TR>
                </TBODY>
            </TABLE>
        </TD>
    </TR>
    
    <TR>
        <TD style="WIDTH: 774px">
            <TABLE>
                <TBODY>
                    <TR>
                        <TD style="WIDTH: 302px">
                         <asp:Panel ID="pnlMarket" runat="server" Width="400px">
                                    <table style="WIDTH: 360px; HEIGHT: 189px">
                                        <tbody>
                                            <tr>
                                                <td align="left" class="td_cell" style="WIDTH: 391px; height: 16px;">
                                                    Market</td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="WIDTH: 391px" valign="top">
                                                    <asp:Panel ID="Panel2" runat="server" Height="200px" ScrollBars="Auto">
                                                        <asp:GridView ID="gv_Market" runat="server" AutoGenerateColumns="False" 
                                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                            CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" Width="360px">
                                                            <RowStyle CssClass="grdRowstyle" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server"    />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Market Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcode" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle Width="400px" />
                                                                    <ItemStyle Width="400px" />
                                                                </asp:TemplateField>
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
                                            <tr>
                                                <td align="left" style="WIDTH: 391px">
                                                    &nbsp;<asp:Button ID="btnSelectAll" runat="server" CssClass="btn" tabIndex="20" 
                                                        Text="Select All" />
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="btnUnselectAll" runat="server" CssClass="btn" tabIndex="21" 
                                                        Text="Unselect All" />
                                                    &nbsp;<input id="txtrowcnt" runat="server"   type="text" style="width: 10px ; visibility: hidden; " />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </asp:Panel>
                            </TD>
                          <TD style="WIDTH: 410px">
                            <asp:Panel id="pnlCancellation" runat="server" Font-Size="9pt" Font-Names="Verdana" Font-Bold="False" GroupingText="Cancellation Policy">
                                <TABLE><TBODY><TR>
                                    <TD style="WIDTH: 100px" class="td_cell">Active</TD></TR>
                                   <TR><TD style="WIDTH: 100px; HEIGHT: 36px" class="td_cell">
                                        <TEXTAREA style="WIDTH: 423px; HEIGHT: 91px" id="txtCanActive" class="field_input" tabIndex=6 runat="server"></TEXTAREA></TD>
                                   </TR>
                                   <TR><TD style="WIDTH: 100px" class="td_cell">De-Activate</TD></TR>
                                   <TR><TD style="WIDTH: 100px" class="td_cell"><TEXTAREA style="WIDTH: 421px; HEIGHT: 84px" id="txtCanDeactive" class="field_input" tabIndex=7 runat="server"></TEXTAREA></TD></TR>
                                </TBODY></TABLE>
                             </asp:Panel>
                          </TD>
                      </TR>
                      <TR>
                          <TD style="WIDTH: 302px" class="td_cell">
                                <asp:Panel id="Panel1" runat="server" Font-Size="9pt" Font-Names="Verdana" GroupingText="Child Policy">
                                    <TABLE><TBODY><TR><TD style="WIDTH: 102px" class="td_cell">Active</TD></TR>
                                        <TR><TD style="WIDTH: 100px" class="td_cell">
                                                <TEXTAREA style="WIDTH: 423px; HEIGHT: 91px" id="txtChildActive" class="field_input" tabIndex=10 runat="server"></TEXTAREA></TD></TR>
                                        <TR><TD style="WIDTH: 100px" class="td_cell">De-Activate</TD></TR>
                                        <TR><TD style="WIDTH: 100px" class="td_cell"><TEXTAREA style="WIDTH: 421px; HEIGHT: 91px" id="txtChildDeactive" class="field_input" tabIndex=11 runat="server"></TEXTAREA></TD></TR></TBODY></TABLE>
                                </asp:Panel>
                          </TD>
                            <TD style="WIDTH: 410px" class="td_cell" vAlign=top align=right>
                            
                            <asp:Panel id="pnlRemarks" runat="server" Font-Size="9pt" Font-Names="Verdana" GroupingText="Remarks">
                                <TABLE><TBODY><TR>
                                    <TD style="WIDTH: 108px" class="td_cell">Active</TD></TR>
                                 <TR>
                                    <TD style="WIDTH: 108px; HEIGHT: 73px" class="td_cell">
                                        <TEXTAREA style="WIDTH: 427px; HEIGHT: 91px" id="txtRemarkAct" class="field_input" tabIndex=8 runat="server"></TEXTAREA>
                                    </TD>
                                </TR>
                                 <TR>
                                    <TD style="WIDTH: 108px" class="td_cell">De-Activate</TD>
                                </TR>
                                <TR>
                                    <TD style="WIDTH: 108px; HEIGHT: 35px" class="td_cell">
                                        <TEXTAREA style="WIDTH: 424px; HEIGHT: 91px" id="txtRemarkDeAct" class="field_input" tabIndex=9 runat="server"></TEXTAREA>
                                    </TD>
                                </TR>
                                </TBODY></TABLE></asp:Panel>
                            </TD>
                       </TR>
                       <TR>
                            <TD style="WIDTH: 302px" class="td_cell"><INPUT id="chkActive" tabIndex=12 type=checkbox CHECKED runat="server" />Active</TD>
                            <TD style="WIDTH: 410px" class="td_cell" align=right>&nbsp; </TD>
                       </TR>
                       <TR>
                            <TD style="WIDTH: 302px" class="td_cell"></TD>
                            <TD style="WIDTH: 410px" class="td_cell" align=right>
                                <asp:Button id="btnSave" tabIndex=13 onclick="btnSave_Click" runat="server" Text="Save" CssClass="field_button"></asp:Button>
                                &nbsp; <asp:Button id="btnCancel" tabIndex=14 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>
                                &nbsp;<asp:Button id="btnhelp" tabIndex=15 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" __designer:wfdid="w29"></asp:Button>&nbsp;</TD>
                       </TR></TBODY></TABLE>
                   </TD></TR></TBODY></TABLE>
</contenttemplate>
</asp:UpdatePanel>

</asp:Content>

