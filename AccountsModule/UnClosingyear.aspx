<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="unclosingyear.aspx.vb" Inherits="AccountsModule_unclosingyear"  %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">



	function disp_confirm()
    {
        var r=confirm("Are you sure to close the year?");
        return r
    }


</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
                <tr>
                    <td align="center" class="field_heading" style="width: 100px; height: 21px">
                        <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="UnClosing Year"
                            Width="388px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px;">
            <table style="width: 671px">
                <tr>
                    <td style="width: 397px" class="td_cell">
                        Revise Year</td>
                    <td style="width: 2041px">
                        &nbsp;<select id="ddlyearclosing" runat="server" class="drpdown" 
                            style="width: 268px">
                            <option selected="selected"></option>
                        </select></td>
                    <td style="width: 190px">
                        &nbsp;</td>
                </tr>
            </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <table style="width: 668px">
                            <tr>
                                <td style="width: 100px; height: 128px;">
                                    <div id="DIV1" runat="server" 
                                        style="width: 663px; height: 112px; background-color: lavender" class="td_cell">
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 444px">
                            <tr>
                                <td style="width: 100px">
                                </td>
                                <td style="width: 100px">
                                    <asp:Button ID="btnseal" runat="server" CssClass="btn" Text="UnClose Year" OnClick="btnseal_Click" 
                                        OnClientClick="return disp_confirm();" /></td>
                                <td style="width: 100px">
                                    <asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="1px"></asp:TextBox>
                                    <input id="txtDivCode" runat="server" maxlength="20" style="visibility: hidden; width: 5px"
                                        type="text" /></td>
                            </tr>
                        </table>
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

