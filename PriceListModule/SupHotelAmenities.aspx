<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupHotelAmenities.aspx.vb" Inherits="SupHotelAmenities" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="javascript" type="text/javascript" >

    function CallWebMethod(methodType) {
        switch (methodType) {
            case "partycode":
                var select = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
        }
    }
    function Confirmselection() {
        if (confirm('Are you sure you want to save your changes?') == false) {
            return false;
        }
        else {

        }

    }

</script> 
 <style>
 .bgrow
 {
 background-color:white; 

 }
  .ajax__tab_tab
 {
     font-size :14px; 
     font-weight:700;       
 }
 div.ex1 {
    background-color: lightblue;
    width: 100%;
    height: 300px;
    overflow: scroll;
}
 </style>  
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
            <table align="left">
                <tbody>
                    <tr>
                        <td colspan="20" align="left" class="bgrow">
                            <whc:hoteltab ID="whotelatbcontrol" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="margin-top: -6px; margin-left: 13px;">
                                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                                    border-bottom: gray 2px solid;" class="td_cell" align="left">
                                    <tr>
                                        <td valign="top" align="center" width="150" colspan="2">
                                            <asp:Label ID="lblHeading" runat="server" Text="Supplier " ForeColor="White" Width="100%"
                                                CssClass="field_heading"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" width="150">
                                            Code <span style="color: #ff0000" class="td_cell">*</span>
                                        </td>
                                        <td class="td_cell" valign="top" align="left">
                                            <input style="width: 196px" id="txtCode" class="field_input" disabled tabindex="1"
                                                type="text" maxlength="20" runat="server" />
                                            &nbsp; Name&nbsp;
                                            <input style="width: 213px" id="txtName" class="field_input" disabled tabindex="4"
                                                type="text" maxlength="100" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="150">
                                            &nbsp;
                                        </td>
                                        <td align="left" class="td_cell" valign="top">
                                        <asp:Panel ID="PnlSuplierSel" runat="server">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        Supplier
                                                    </td>
                                                    <td>
                                                        <select id="ddlSuppierCD" runat="server" class="field_input" name="D1" onchange="CallWebMethod('partycode');"
                                                            style="width: 220px">
                                                            <option selected=""></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <select id="ddlSuppierNM" runat="server" class="field_input" name="D2" onchange="CallWebMethod('partyname');"
                                                            style="width: 310px">
                                                            <option selected=""></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnfilldetail" runat="server" CssClass="field_button" Text="Fill Details" />
                                                    </td>
                                                </tr>
                                            </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" width="150">
                                            <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl>
                                        </td>
                                        <td style="width: 100px" valign="top">
                                            <div style="width: 824px; height: 100%" id="iframeINF" runat="server">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>

                                                        <table style="width: 656px">
                                                            <tbody>
                                                                <tr>
                                                                    <td style="width: 80px" class="td_cell" colspan="2">
                                                                        
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 100px; height: 4px">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px">
                                                                                       
                                                                                            <cc1:TabContainer ID="TabAmenities" runat="server" ActiveTabIndex="0" 
                                                                                                Width="617px" Font-Bold="False"  ToolTip="Amenities" >
                                                                                                <cc1:TabPanel ID="TabRoomAmenities" runat="server" HeaderText="Room Amenities">
                                                                                                    <ContentTemplate>
                                                                                                    
                                                                                                        <asp:GridView ID="Gv_RoomAmenities" TabIndex="78" runat="server" Width="100%" __designer:wfdid="w8"
                                                                                                            AutoGenerateColumns="False">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField>
                                                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <input id="ChkSelect" type="checkbox" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="AmenityName" HeaderText="Amenity Name"></asp:BoundField>
                                                                                                                <asp:BoundField DataField="AmenityCode" HeaderText="Amenity Code"></asp:BoundField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                       
                                                                                                    </ContentTemplate>
                                                                                                </cc1:TabPanel>
                                                                                                <cc1:TabPanel ID="TabHotelAmenities" runat="server" HeaderText="Hotel Amenities">
                                                                                                 <ContentTemplate>
                                                                                                
                                                                                                        <asp:GridView ID="Gv_HotelAmenities" TabIndex="78" runat="server" Width="100%" __designer:wfdid="w8"
                                                                                                            AutoGenerateColumns="False">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField>
                                                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <input id="ChkSelect" type="checkbox" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="AmenityName" HeaderText="Amenity Name"></asp:BoundField>
                                                                                                                <asp:BoundField DataField="AmenityCode" HeaderText="Amenity Code"></asp:BoundField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                        
                                                                                                    </ContentTemplate>
                                                                                                </cc1:TabPanel>
                                                                                                <cc1:TabPanel ID="TabRecreationAmenities" runat="server" HeaderText="Recreation Amenities">
                                                                                                 <ContentTemplate>
                                                                                                
                                                                                                        <asp:GridView ID="Gv_RecreationAmenities" TabIndex="78" runat="server" Width="100%" __designer:wfdid="w8"
                                                                                                            AutoGenerateColumns="False">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField>
                                                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <input id="ChkSelect" type="checkbox" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="AmenityName" HeaderText="Amenity Name"></asp:BoundField>
                                                                                                                <asp:BoundField DataField="AmenityCode" HeaderText="Amenity Code"></asp:BoundField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                        
                                                                                                    </ContentTemplate>
                                                                                                </cc1:TabPanel>
                                                                                            </cc1:TabContainer>


                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            <asp:Button ID="BtnSelectAllAmenities" TabIndex="79" OnClick="BtnSelectAllAmenities_Click"
                                                                                                runat="server" Text="Select All" CssClass="field_button" __designer:wfdid="w9">
                                                                                            </asp:Button>&nbsp;<asp:Button ID="BtnDeSelectAllAmenities" TabIndex="80" OnClick="BtnDeSelectAllAmenities_Click"
                                                                                                runat="server" Text="DeSelect All" CssClass="field_button" __designer:wfdid="w10">
                                                                                            </asp:Button>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px">
                                                                                            <table style="width: 592px">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td style="width: 35px; height: 22px">
                                                                                                            <asp:Button ID="btnAddNew" TabIndex="7" runat="server" Text="Add New in Master" Width="200px"
                                                                                                                CssClass="field_button" __designer:wfdid="w5" __designer:dtid="5348024557502480"
                                                                                                                Font-Bold="True"></asp:Button>
                                                                                                        </td>
                                                                                                        <td style="width: 26px; height: 22px">
                                                                                                            <asp:Button ID="BtnSaveAmenity" runat="server" __designer:wfdid="w11" CssClass="field_button"
                                                                                                                OnClick="BtnSaveAmenity_Click" TabIndex="81" Text="Save" Width="100px" />
                                                                                                        </td>
                                                                                                        <td style="width: 100px; height: 22px">
                                                                                                            <asp:Button ID="BtnAmenityCancel" TabIndex="82" OnClick="BtnAmenityCancel_Click" runat="server"
                                                                                                                Text="Return to Search" CssClass="field_button" __designer:wfdid="w12"></asp:Button>
                                                                                                        </td>
                                                                                                        <td style="width: 100px; height: 22px">
                                                                                                            <asp:Button ID="btnhelp" TabIndex="83" OnClick="btnhelp_Click" runat="server" Text="Help"
                                                                                                                CssClass="field_button" __designer:wfdid="w13"></asp:Button>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

