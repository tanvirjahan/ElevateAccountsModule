<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="AcctGroupNew.aspx.vb" Inherits="AcctGroupNew" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">
        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid" class="td_cell">
                <tbody>
                    <tr>
                        <td style="width: 539px; height: 15px" align="center">
                            <asp:Label ID="lblHeading" runat="server" Text="Add Account Group" Width="536px"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <table style="width: 539px; height: 93px">
                                <tbody>
                                    <tr>
                                    <td style="width: 154px"  >
                                <span style="FONT-SIZE:9pt; FONT-FAMILY: Arial">Account Code</span>
                                <span style="FONT-SIZE:8pt; COLOR: #ff0000; FONT-FAMILY: Arial">*</span>
                            </td>
                                        <td id="txtAccCode">
                                            &nbsp;<input style="width: 120px" id="txtAccCode" tabindex="1" type="text" maxlength="20"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                       <td style="width: 154px" >
                                <span style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">  Account Name
                                <span style="FONT-SIZE:8pt; COLOR: #ff0000; FONT-FAMILY: Arial">*</span>
                            </td>
                                        <td>
                                            &nbsp;<input style="width: 220px" id="txtAccName" tabindex="2" type="text" maxlength="100"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                     

                                                                  <td style="width: 154px" >
                                <span style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">  Major
                                <span style="COLOR: red">&nbsp;*</span></span>
                            </td>
                                        <td>
                                            &nbsp;<select style="width: 224px" id="ddlMajor" tabindex="3" runat="server">
                                                <option selected></option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                      
                                         <td style="width: 154px" >
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">  Balance Sheet / P & L Group 
                                <span style="COLOR: red">&nbsp;*</span></span>
                            </td>
                                        <td>
                                            &nbsp;<select id="ddlAccMajobSub" runat="server" style="width: 225px;" 
                                                tabindex="4"><option
                                                selected></option>
                                            </select>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 154px">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                    
                                        <td style="height: 24px; width: 202px;" align="center" colspan="2" >
                               
                                            <asp:Button ID="btnSave" TabIndex="5" runat="server" Text="Save" Font-Bold="True"
                                                CssClass="field_button"></asp:Button>&nbsp;
                                            <asp:Button ID="btnExit" TabIndex="6" runat="server" Text="Exit" Font-Bold="True"
                                                CssClass="field_button"></asp:Button>&nbsp;<asp:Button ID="btnhelp" TabIndex="7"
                                                    OnClick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" __designer:wfdid="w10">
                                                </asp:Button>
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
</asp:Content>
