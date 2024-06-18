<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="ExcInfoForOnlineMaster.aspx.vb" Inherits="ExcInfoForOnlineMaster" %>

<%@ Register Src="../PriceListModule/SubMenuUserControl.ascx" TagName="SubMenuUserControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">
        function checkNumber() {
            if (event.keyCode < 45 || event.keyCode > 57) {
                return false;
            }
        }
        function formmodecheck() {
            var vartxtcode = document.getElementById("<%=txtCode.ClientID%>");
            if (vartxtcode.value == '') {
                doLinks(false);
            }
            else {
                doLinks(true);
            }
        }

        function doLinks(how) {
            for (var l = document.links, i = l.length - 1; i > -1; --i)
                if (!how)
                    l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
                else
                    l[i].onclick = function () { return; };
        }
         
    

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <Triggers> 
        <asp:PostBackTrigger ControlID="btnSave" /> 
    </Triggers> 
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 800px; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Excursion -Info For Online" ForeColor="White"
                                Width="800px" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" valign="top" align="left">
                            <span style="color: #ff0000" class="td_cell"></span>
                        </td>
                        <td style="width: 85%;" class="td_cell" valign="top" align="left">
                            Code <span style="color: #ff0000" class="td_cell">&nbsp; </span>
                            <asp:TextBox ID="txtCode" ReadOnly="true" runat="server" Width="190px" Height="16px"></asp:TextBox>
                            &nbsp; Name&nbsp;
                            <asp:TextBox ID="txtName" TabIndex="1" runat="server" CssClass="field_input" MaxLength="100"
                                Width="275px" Height="16px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" valign="top" align="left">
                            <span style="color: #ff0000" class="td_cell"></span>
                        </td>
                        <td style="width: 85%;" class="td_cell" valign="top" align="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%" valign="top">
                            &nbsp;<uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                        </td>
                        <td style="width: 85%" valign="top">
                            <div id="iframeINF" runat="server" style="width: 705px">
                                <asp:Panel ID="PanelMain" runat="server" GroupingText="Excursion-Info For Online"
                                    Width="652px">
                                    <table style="width: 644px; margin-right: 0px;">
                                        <tbody>
                                            <tr>
                                                <td class="td_cell" style="width: 130px; height: 21px;">
                                                    Select Image
                                                </td>
                                                <td colspan="4" style="height: 21px">
                                                    <asp:FileUpload ID="fileImage" runat="server" /><span class="td_cell">(203 X 151)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 130px; height: 2px" class="td_cell">
                                              
                                                </td>
                                                <td style="width: 1%; height: 2px">
                                                    <input style="width: 329px" id="txtimg" tabindex="10" type="text" maxlength="30"
                                                        runat="server" />
                                                      
                                                               &nbsp;&nbsp;
                                               

                                                </td>
                                            </tr>
                                            <tr>
                                            <td></td>
                                            <td>     <asp:Button ID="btnremoveimg" runat="server" __designer:wfdid="w14" CssClass="btn"  
                                                        TabIndex="13" Text="Remove Image" />

                                                          <asp:Button ID="btnviewimage" runat="server" CssClass="btn" TabIndex="12" Text="View Image" />
                                            </td></tr>

                                                             <tr>  <td class="td_cell" style="width: 130px; height: 21px; vertical-align: top"></td><td>
                                            
                                             <asp:Panel ID="pnlimage" runat="server" Text="View Image">
                                      <asp:Image ID="imginfo" runat="server" Height="190px" Width="200px"  />
                                      </asp:Panel> 
                                                    
                                    </td>
                                            </tr>
                                            <tr>
                                                <td class="td_cell" style="width: 130px; height: 21px; vertical-align: top">
                                                    <asp:Label ID="lbldesc" runat="server" Width="163px" Style="vertical-align: top;"
                                                        Text=" Breif Description" Height="16px"></asp:Label>
                                                </td>
                                                <td colspan="4" style="height: 21px">
                                                    <asp:TextBox ID="txtdesc" runat="server" CssClass="field_input" Rows="2" TabIndex="7"
                                                        Style="margin: 0px; height: 120px; width: 400px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                           
                                            <tr>
                                                <td class="td_cell" style="height: 22px;">
                                                 
                                                </td>
                                                <td style="height: 22px;">
                                                    &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="12" Text="Save" />
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="btnhelp0" runat="server" __designer:wfdid="w14" CssClass="btn" OnClick="btnhelp_Click"
                                                        TabIndex="13" Text="Help" />
                                                           <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="14" Text="Return To Search" />   
                                                </td>
                                                <td>
                                                
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 22px">
                                                    &nbsp;
                                                </td>
                                                <td style="height: 22px">
                                                    &nbsp;
                                                </td>
                                                <td style="height: 22px">
                                                    &nbsp;
                                                </td>
                                                <td style="height: 22px">
                                                </td>
                        </td>
                        <script language="javascript">
                            load();
                            formmodecheck();
                        </script>
                    </tr>
                    </tr>
                    <tr>
                        <td style="height: 16px" class="td_cell" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            </asp:Panel> </div> </td> </tbody> </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
