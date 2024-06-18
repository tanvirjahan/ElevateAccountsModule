<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="SupWebInfo.aspx.vb" Inherits="SupWebInfo" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script type="text/javascript">

       
        function PopUpImageView(code) {
             
            var FileName = document.getElementById("<%=hdnFileName.ClientID%>");
            var lblfilename = document.getElementById("<%=lblimage.ClientID%>");
            if (FileName.value == "") {
                FileName.value = code;
            }
            if (lblfilename.innerText != "") {
              
                popWin = open('../PriceListModule/ImageViewWindow.aspx?code=' + FileName.value, 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
                FileName.value = "";
                return false
                          
            }
            else {
                           
                popWin = open('../PriceListModule/ImageViewWindow.aspx?', 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
            }
        }


        function CallWebMethod(methodType) {
            switch (methodType) {
                case "partycode":
                    var select = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;

                    var hdnSupplierCode = document.getElementById("<%=hdnSupplierCode.ClientID%>");
                    hdnSupplierCode.value = selectname.options[selectname.selectedIndex].value;
                    break;
                case "partyname":
                    var select = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;

                    var hdnSupplierCode = document.getElementById("<%=hdnSupplierCode.ClientID%>");
                    hdnSupplierCode.value = select.options[select.selectedIndex].value;
                    break;
            }
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


        function FormValidation(state) {
            // var lblimage1= document.getElementById(lblimage);

            var lblimage = document.getElementById("<%=lblimage.ClientID%>");


            if (state == '8') {
                if (lblimage.innerHTML == '') {
                    alert('Blank image cannot remove...')
                }
            }

        }


    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSaveInfoWeb" />
        </Triggers>
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                              <tr>
                <td colspan ="20" align ="left" class="bgrow" >
               
                    <whc:hoteltab ID="whotelatbcontrol" runat="server" />
               
                
                </td>
                </tr>
                    <tr>
                        <td>
                            
                                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                                    border-bottom: gray 2px solid;" class="td_cell" align="left">
                                    <tr>
                                        <td align="center" colspan="5" valign="top">
                                            <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" ForeColor="White"
                                                Text="Supplier " Width="1000px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="150">
                                            Code <span class="td_cell" style="color: #ff0000">*</span>
                                        </td>
                                        <td align="left" class="td_cell" style="width: 827px" valign="top">
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
                                        <td align="left" class="td_cell" style="width: 827px" valign="top">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        Supplier
                                                    </td>
                                                    <td>
                                                        <select id="ddlSuppierCD" runat="server" class="field_input" name="D1" onchange="CallWebMethod('partycode');"
                                                            style="width: 120px">
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="150">
                                            <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                                        </td>
                                        <td style="width: 827px" valign="top">
                                            <div id="iframeINF" runat="server" style="width: 824px; height: 100%">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <table style="width: 656px">
                                                            <tbody>
                                                                <tr>
                                                                    <td class="td_cell" colspan="2" style="width: 80px;">
                                                                        <asp:Panel ID="PanelInfoForWEb1" runat="server" GroupingText="Info For Web Display"
                                                                            Width="809px">
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td width="30px">
                                                                                            Select Main Image
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:FileUpload ID="FileUpload" runat="server" accept="image/*" CssClass="field_input" Width="240px" />
                                                                                            (624 X464)
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2" style="height: 31px">
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            <asp:Label ID="lblimage" runat="server" ForeColor="Blue" Width="220px"></asp:Label>
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            <asp:Button ID="btnViewimage" runat="server" CssClass="field_button" Text="View"
                                                                                                Width="64px" />
                                                                                            &nbsp;<asp:Button ID="Btnrmv7" runat="server" CssClass="field_button" Text="Remove"
                                                                                                Width="77px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td id="lblimage1" colspan="2" style="height: 20px" text="fdsfasdF">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="30px">
                                                                                            Other Images
                                                                                        </td>
                                                                                        <td>
                                                                                        
                                                                                         <asp:FileUpload ID="FileUpload2" AllowMultiple="true" accept="image/*" runat="server" CssClass="field_input" Width="240px" />    
                                                                                         (Max 5 Images)
                                                                                         
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td style="height: 31px;" align="left">
                                                                                            <style>
                                                                                                .oimg
                                                                                                {
                                                                                                    border: 1px solid #ddd;
                                                                                                    border-radius: 4px;
                                                                                                    padding: 5px;
                                                                                                    width: 60px;
                                                                                                    height: 60px;
                                                                                                    border-width: 1px !important;
                                                                                                    
                                                                                                }
                                                                                                img:hover
                                                                                                {
                                                                                                    box-shadow: 0 0 2px 1px rgba(0, 140, 186, 0.5);
                                                                                                }
                                                                                                .oclose
                                                                                                {
                                                                                                    margin-right:10px;
                                                                                                }
                                                                                            </style>
                                                                                            <asp:Panel ID="PnlOtherImage" runat="server" Style="width: 600px; overflow: hidden;
                                                                                                max-height: 100px;">
                                                                                                <asp:Repeater ID="RepeaterImages" runat="server">
                                                                                                    <ItemTemplate>
                                                                                                        
                                                                                                        <a href='<%# Container.DataItem %>' target="_blank">
                                                                                                            <asp:Image ID="Image" CssClass="oimg" runat="server" ImageUrl='<%# Container.DataItem %>' />
                                                                                                        </a>
                                                                                                        <asp:ImageButton ID="imgBRemoveOImage" CssClass="oclose" runat="server" 
                                                                                                         OnClick="imgBRemoveOImage_Click" ToolTip="Click to remove this image." CommandArgument="<%# Container.DataItem %>"  ImageUrl="../images/close.jpg"/>
                                                                                                    </ItemTemplate>
                                                                                                </asp:Repeater>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            Hotel Text
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtHOteltxt" runat="server" CssClass="field_input" Height="54px"
                                                                                                TabIndex="104" TextMode="MultiLine" Width="597px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 112px">
                                                                                            Latitude &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtLatitude" runat="server" CssClass="field_input" TabIndex="105"
                                                                                                Width="200px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 112px">
                                                                                            Longitude &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtLongitude" runat="server" CssClass="field_input" TabIndex="105"
                                                                                                Width="200px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 112px">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                       
                                                                                            <asp:Button ID="BtnSaveInfoWeb" runat="server" CssClass="field_button" OnClick="BtnSaveInfoWeb_Click"
                                                                                                TabIndex="113" Text="Save" Width="66px" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="BtnCancelInfoWeb" runat="server" CssClass="field_button" OnClick="BtnCancelInfoWeb_Click"
                                                                                                TabIndex="114" Text="Return to Search" />
                                                                                            &nbsp;&nbsp; &nbsp;&nbsp;
                                                                                            <asp:Button ID="btnhelp" runat="server" CssClass="field_button" OnClick="btnhelp_Click"
                                                                                                TabIndex="116" Text="Help" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:TextBox ID="hdnFileName" Text="" runat="server" Style="display: none" />
    <asp:HiddenField ID="hdnSupplierCode" runat="server" />
</asp:Content>
