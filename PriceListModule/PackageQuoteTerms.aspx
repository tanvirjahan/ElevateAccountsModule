<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PackageQuoteTerms.aspx.vb" Inherits="PackageQuoteTerms"  MasterPageFile="~/PriceListMaster.master" Strict="true" ValidateRequest="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>
<asp:Content ContentPlaceHolderID="Main" runat="server" >
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
        <div>
        
        </div>
            <div>
                <table style="width: 100%">
                    <tbody>
                        <tr>
                            <td style="font-weight: bold; font-size: 12px; font-family: Verdana" class="field_heading">
                                Package Quote Terms and Conditions :
                            </td>
                        </tr>
                           <tr>
                            <td>
                                                     &nbsp;
                            </td>
                            </tr>
                             <tr>
                            <td>
                            Package Type : 
                                <asp:DropDownList ID="ddlPackage" runat="server" AutoPostBack="True">
                               
                                    <asp:ListItem Value="VIH">Visa Information Header</asp:ListItem>
                                         <asp:ListItem Value="TDF">Tourism Dirham Fee</asp:ListItem>
                                               <asp:ListItem Value="BC">Bank Charges</asp:ListItem>
                                                            <asp:ListItem Value="VIF">Visa Information Footer</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            </tr>
                             <tr>
                            <td>
                          &nbsp;
                            </td>
                            </tr> 
                        <tr>
                            <td>
                                <HTMLEditor:Editor ID="Editor1" runat="server" Height="300px" Width="100%" AutoFocus="true" />
                            </td>
                        </tr>
                                    <tr>
                            <td>
                          &nbsp;
                            </td>
                            </tr> 
                        <tr>
                            <td style="text-align: left">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"></asp:Button>&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn"></asp:Button>&nbsp;<asp:Button
                                    ID="btnHelp" OnClick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528"
                                    Width="45px" CssClass="btn" __designer:wfdid="w4"></asp:Button>
                                &nbsp;
                                <asp:TextBox ID="txtPkTermsId" runat="server" Width="80px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
</contenttemplate>
    </asp:UpdatePanel>




</asp:Content>
