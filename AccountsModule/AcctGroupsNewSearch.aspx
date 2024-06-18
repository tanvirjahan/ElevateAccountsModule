<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="AcctGroupsNewSearch.aspx.vb" Inherits="AcctGroupsNewSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script language="javascript" type="text/javascript">
        function client_OnTreeNodeChecked(event) {
            var TreeNode = event.srcElement || event.target;
            if (TreeNode.tagName == "INPUT" && TreeNode.type == "checkbox") {
                if (TreeNode.checked) {
                    uncheckOthers(TreeNode.id);
                }
            }
        }

        function uncheckOthers(id) {
            var elements = document.getElementsByTagName('input');
            // loop through all input elements in form
            for (var i = 0; i < elements.length; i++) {
                if (elements.item(i).type == "checkbox") {
                    if (elements.item(i).id != id) {
                        elements.item(i).checked = false;
                    }
                }
            }
        }
    </script>
    <table class="td_cell" style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid;">
        <tr>
            <td colspan="2" style="width: 100%; text-align: center">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Account Group New"
                    Width="100%"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" Height="350px" Width="100%" ScrollBars="Auto">
                            <asp:TreeView ID="tvAccgrp" TabIndex="1" runat="server" Font-Size="9pt" __designer:wfdid="w10" ExpandDepth="1" ShowLines="true"
                                ShowExpandCollapse="true" ShowCheckBoxes="All">
                            </asp:TreeView>
                        </asp:Panel>
                        &nbsp; 
                    </ContentTemplate>
                </asp:UpdatePanel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnadd" runat="server" CssClass="btn" Font-Bold="True"
                    Text="Add" TabIndex="2" />&nbsp;
                <asp:Button ID="btnEdit" runat="server" CssClass="btn" Font-Bold="True"
                    Text="Edit" TabIndex="3" />&nbsp;
                <asp:Button ID="btndelete" runat="server" CssClass="btn" Font-Bold="True"
                    Text="Delete" TabIndex="4" />&nbsp;<asp:Button ID="BtnPrint"
                        runat="server" CssClass="btn" Text="Report" TabIndex="5" />&nbsp;<asp:Button ID="btnhelp"
                            runat="server" CssClass="btn" OnClick="btnhelp_Click"
                            TabIndex="8" Text="Help" />&nbsp;
                               <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20"
                                   runat="server" />
            </td>

            <td style="display: none">
                <asp:Button ID="btnView" runat="server" CssClass="btn" Font-Bold="True"
                    Text="print" TabIndex="2" />&nbsp;
                <asp:Button ID="btnExcel" runat="server" CssClass="btn" Font-Bold="True"
                    Text="excel" TabIndex="3" />&nbsp;</td>

            <td align="center">
                <%--<triggers>
                <asp:postbackTrigger controlid="btnSearch" > </asp:postbackTrigger>
                </triggers>--%>
                <table style="width: 479px; height: 19px;">
                    <tr>
                        <td style="width: 122px">
                            <select id="ddlAccountSearchType" runat="server" class="drpdown"
                                style="width: 125px" tabindex="6">
                                <option selected="selected" value="Account Code">Account Code</option>
                                <option value="Account Name">Account Name</option>
                            </select>
                        </td>
                        <td style="width: 241px">Search Text</td>
                        <td style="width: 190px">
                            <asp:TextBox ID="txtSerch" runat="server" CssClass="txtbox" MaxLength="20"
                                TabIndex="7" Width="159px"></asp:TextBox></td>
                        <td style="width: 127px">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn" Font-Bold="True"
                                Text="Search" TabIndex="8" />&nbsp;</td>
                    </tr>
                </table>

            </td>
        </tr>
    </table>
</asp:Content>

