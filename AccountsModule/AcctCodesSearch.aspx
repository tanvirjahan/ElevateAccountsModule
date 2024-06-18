<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="AcctCodesSearch.aspx.vb" Inherits="AcctCodesSearch"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
function client_OnTreeNodeChecked(event)
{
    var TreeNode = event.srcElement || event.target ;
    if (TreeNode.tagName == "INPUT" && TreeNode.type == "checkbox")
    {
    if(TreeNode.checked)
        {
            uncheckOthers(TreeNode.id);
        }
    }
}

function uncheckOthers(id)
{
    var elements = document.getElementsByTagName('input');
// loop through all input elements in form
    for(var i = 0; i < elements.length; i++)
    {
        if(elements.item(i).type == "checkbox")
        {
        if(elements.item(i).id!=id)
            {
                elements.item(i).checked= false;
            }
        }
    }
}
     </script>
    <table class="td_cell" style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid; ">
        <tr>
            <td colspan="2" align="center" class="field_heading">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Account Code"
                    Width="100%"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<asp:Panel id="Panel1" runat="server" Height="400px" Width="100%" ScrollBars="Vertical"><asp:TreeView id="tvAccgrp" tabIndex=1 runat="server" Font-Size="9pt" ShowCheckBoxes="All" ShowExpandCollapse="true" ShowLines="true" ExpandDepth="1"></asp:TreeView></asp:Panel> 
</contenttemplate>
                </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td >
                <asp:Button ID="btnadd" runat="server" CssClass="btn" Font-Bold="True" 
                    Text="Add" TabIndex="2" />&nbsp;
                <asp:Button ID="btnEdit" runat="server" CssClass="btn" Font-Bold="True" 
                    Text="Edit" TabIndex="3" />&nbsp;
                <asp:Button ID="btndelete" runat="server" CssClass="btn" Font-Bold="True" 
                    Text="Delete" TabIndex="4" />&nbsp;
                <asp:Button ID="btnview" runat="server" CssClass="btn" Font-Bold="True" 
                    Text="View" TabIndex="5" />&nbsp;
                    <asp:Button ID="btnhelp" runat="server" CssClass="btn" OnClick="btnhelp_Click"
                        TabIndex="6" Text="Help" />
                          <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20"
                                                                          runat="server" />
                        </td>

<td style="display:none"><asp:Button ID="btnPrint" runat="server" CssClass="btn" Font-Bold="True" 
                    Text="Add" TabIndex="2" />&nbsp;
                <asp:Button ID="btnExcel" runat="server" CssClass="btn" Font-Bold="True" 
                    Text="Edit" TabIndex="3" />&nbsp;</td>


            <td align="right" >
                <%--<triggers>
                <asp:postbackTrigger controlid="btnSearch" > </asp:postbackTrigger>
                </triggers>--%>
                <table>
                    <tr>
                        <td>
                            <select id="ddlAccountSearchType" runat="server" class="drpdown" 
                                style="width: 125px" tabindex="6">
                                <option selected="selected" value="Account Code">Account Code</option>
                                <option value="Account Name">Account Name</option>
                            </select>
                        </td>
                        <td>
                            Search Text</td>
                        <td>
                            <asp:TextBox ID="txtSerch" runat="server" CssClass="txtbox" MaxLength="20" TabIndex="7"
                                Width="150px"></asp:TextBox></td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn" Font-Bold="True" 
                                Text="Search" TabIndex="8" />&nbsp;<asp:Button 
                                ID="btnReport" runat="server"
                                    CssClass="btn" Font-Bold="True" Text="Report" TabIndex="9" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

