<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Accnt_trn_amendlog.aspx.vb" Inherits="AccountsModule_Accnt_trn_amendlog" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="javascript" type="text/javascript">

function CloseFormOK()
    {
        window.returnValue = true;
        self.close();
    }

    </script>
<asp:UpdatePanel ID = "updatepanel1" runat ="server">
<ContentTemplate>
<br /><br />
<table  style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell">
<Tbody><TR><TD class="field_heading" align="center" style="width: 526px"><asp:Label id="lblHeading" runat="server" Text="Reasons for Amend" CssClass="field_heading" Width="500px"></asp:Label> </TD>

</TR>


    <tr>
        <td style="height: 101px; width: 426px;">
            <table  width="600px">
                <tbody>
                    <tr>
                        <td class="td_cell">
                            Doc No:</td>
                        <td>
                             
                            <asp:Label ID="lblDocNo" runat="server"   cssClass="field_input" 
                                height="18px" text="Good" Width="63px"></asp:Label>
                        </td>
                        <td class="td_cell">
                            Doc Type:</td>
                        <td>
                             
                            <asp:Label ID="lblDocType" runat="server"   cssClass="field_input" 
                                height="18px" text="The" Width="36px"></asp:Label>
                        </td>
                        <td class="td_cell" style="width: 143px">
                            Transaction Date</td>
                        <td>
                           
                            <asp:Label ID="lblTransdate" runat="server"  
                                cssClass="field_input" height="18px" text="Date" Width="60px"></asp:Label>
                        </td>
                    </tr>

       <tr>
       <td  colspan="6">  </td>
       </tr>             
                 
 
               <tr >
    <td align="center" colspan="6" style="height: 16px">
        <asp:Label ID="lblReason" runat="server" align="center" 
            Text="Reasons for Amend" Width="123px"></asp:Label>
    </td></tr>
    <tr>
        <td align="center" colspan="6" style="height: 34px">
            <asp:TextBox ID="txtreason" runat="server" align="center" 
                CssClass="field_input" TextMode="MultiLine" Height="43px" Width="395px"></asp:TextBox>
        </td>
    </tr>
    
     <tr>
       <td  colspan="6">  </td>
       </tr>  

            <tr>
                <td colspan="6" align="center">
                    <asp:Button ID="btnSave" runat="server"   CssClass="field_button" 
                        Font-Bold="True" tabIndex="8" Text=" OK " />
                     &nbsp;
                    <asp:Button ID="btnCancel" runat="server"   CssClass="field_button"  Visible=false
                        Font-Bold="True" tabIndex="9" Text=" Cancel "  />
                </td>
    </tr>
    </tbody> 
            </table>

    </td>
    </tr>
    </td>
    </tr>
    </Tbody>
    </table>

    <tr>
        <td>
            &nbsp; &nbsp;
        </td>
    </tr>
    </td>
    </TR>
    </Tbody>
    </table>

</ContentTemplate>

</asp:UpdatePanel>
</asp:Content>

