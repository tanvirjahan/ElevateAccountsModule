<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="Changeyear.aspx.vb" Inherits="AccountsModule_Changeyear"  %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">







</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
                <tr>
                    <td align="center" class="field_heading" style="width: 100px; height: 21px">
                        <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Change Year"
                            Width="100%"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 65px;">
            <table style="width: 671px">
                <tr>
                    <td style="width: 573px; height: 24px;">
                        </td>
                    <td style="width: 2041px; height: 24px;">
                        &nbsp; &nbsp;
                    </td>
                    <td style="width: 190px; height: 24px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 573px" class="td_cell">
                        Change Year</td>
                    <td style="width: 2041px">
                        &nbsp;<asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged"
                            TabIndex="5" Width="175px" CssClass="drpdown">
                        </asp:DropDownList></td>
                    <td style="width: 190px">
                        &nbsp;</td>
                </tr>
            </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <table style="width: 444px">
                            <tr>
                                <td style="width: 100px">
                                </td>
                                <td style="width: 99px">
                                    </td>
                                <td style="width: 81px">
                                    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
                                    <input id="txtDivCode" runat="server" maxlength="20" style="visibility: hidden; width: 5px"
                                        type="text" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                    </td>
                </tr>
            </table>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

