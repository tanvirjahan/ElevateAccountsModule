<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CreditNotePrint.aspx.vb" Inherits="AccountsModule_CreditNotePrint" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    
<script language="javascript" type="text/javascript" >
var txtEmail1;
var txtCont1;
function FillEmailId(txtsupagnt,ddlCont,type,txtEmail,txtCont)
{
var ddlCont1 = document.getElementById(ddlCont);
   var txtsupagnt1 = document.getElementById(txtsupagnt);
    txtEmail1 = document.getElementById(txtEmail);
    txtCont1 = document.getElementById(txtCont);
  var  type1 = document.getElementById(type);
  
    
   var codeid=ddlCont1.options[ddlCont1.selectedIndex].value;
     //alert(txtEmail1);
    var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
    constr=connstr.value   

    ColServices.clsServices.GetEmailandContactnew(constr,txtsupagnt1.value,codeid,type1.value,FillEmail,ErrorHandler,TimeOutHandler);
    
    ddlCont.value=codeid;
}
function FillEmail(result)
{
//alert(txtEmail1);

    txtEmail1.value=result[0].ListText
    txtCont1.value=result[0].ListValue
}
function TimeOutHandler(result)
{
    alert("Timeout :" + result);
}
function ErrorHandler(result)
{
    var msg=result.get_exceptionType() + "\r\n";
    msg += result.get_message() + "\r\n";
    msg += result.get_stackTrace();
    alert(msg);
}
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 950px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 400px" vAlign=top align=center><TABLE style="WIDTH: 950px"><TBODY><TR><TD class="td_cell" align=center colSpan=9><asp:Label id="lblHeading" runat="server" Text="Print Credit Note" CssClass="field_heading" Width="950px"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 150px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 101px; HEIGHT: 10px" class="td_cell" align=left></TD></TR><TR><TD style="WIDTH: 100px" class="td_cell" align=left>Customer</TD><TD style="WIDTH: 100px" class="td_cell" align=left><INPUT id="txtCustcode" class="txtbox" readOnly type=text maxLength=20 size=10 runat="server" /></TD><TD style="WIDTH: 150px" class="td_cell" align=left><INPUT id="txtCustName" class="txtbox" readOnly type=text maxLength=20 runat="server" /></TD><TD style="WIDTH: 100px" class="td_cell" align=left>
    Credit Note no</TD><TD style="WIDTH: 100px" class="td_cell" align=left><INPUT id="txtCreditNoteNo" class="txtbox" readOnly type=text maxLength=20 size=10 runat="server" /></TD><TD style="WIDTH: 100px" class="td_cell" align=left>ReqDate</TD><TD style="WIDTH: 100px" class="td_cell" align=left><INPUT id="txtReqDate" class="txtbox" readOnly type=text maxLength=20 size=10 runat="server" /></TD><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 101px" class="td_cell" align=left></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
            height: 9px" type="text" /></TD><TD style="WIDTH: 150px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 10px" class="td_cell" align=left></TD><TD style="WIDTH: 101px; HEIGHT: 10px" class="td_cell" align=left></TD></TR><TR><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 150px" class="td_cell" align=left></TD><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 100px" class="td_cell" align=left></TD><TD style="WIDTH: 101px" class="td_cell" align=left></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 150px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 101px; HEIGHT: 20px" class="td_cell" align=left></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 150px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 20px" class="td_cell" align=left></TD><TD style="WIDTH: 101px; HEIGHT: 20px" class="td_cell" align=left></TD></TR><TR><TD style="HEIGHT: 16px" class="td_cell" align=center colSpan=9><TABLE width="100%"><TBODY><TR><TD style="WIDTH: 50px"></TD><TD align=left><asp:GridView id="gvCreditNote" runat="server" CssClass="grdstyle" OnRowCommand="gvInvoice_RowCommand" OnRowDataBound="gvInvoice_RowDataBound" ShowHeader="False" AutoGenerateColumns="False" OnSelectedIndexChanged="gvInvoice_SelectedIndexChanged">
                                                     <RowStyle CssClass="grdRowstyle" />
                                                     <Columns>
                                                         <asp:TemplateField HeaderText="Agent">
                                                             <ItemTemplate>
                                                                 <input id="lblSupAgentCode" runat="server" class="txtbox" maxlength="20" readonly="readOnly"
                                                                     size="10" style="width: 0px; visibility: hidden" type="text" value='<%# DataBinder.Eval (Container.DataItem, "agentcode") %>' />
                                                                 <asp:Label ID="lblSupAgentName" runat="server" CssClass="td_cell" Text='<%# DataBinder.Eval (Container.DataItem, "agentname") %>'></asp:Label>
                                                             </ItemTemplate>
                                                             <HeaderStyle Width="200px" />
                                                             <ItemStyle Width="200px" />
                                                         </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Email">
                                                             <ItemTemplate>
                                                                 <input id="txtEmail" runat="server" class="txtbox" maxlength="100" size="10"
                                                                     style="width: 150px" type="text" value='<%# DataBinder.Eval (Container.DataItem, "email") %>' />
                                                             </ItemTemplate>
                                                             <HeaderStyle Width="150px" />
                                                             <ItemStyle Width="150px" />
                                                         </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Contact">
                                                             <ItemTemplate>
                                                                 <input id="txtContact" runat="server" class="txtbox" maxlength="20" size="10"
                                                                     style="width: 150px" type="text" value='<%# DataBinder.Eval (Container.DataItem, "contact") %>' />
                                                             </ItemTemplate>
                                                             <HeaderStyle Width="150px" />
                                                             <ItemStyle Width="150px" />
                                                         </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Select Agent">
                                                             <ItemTemplate>
                                                                 <select id="ddlAgnet" runat="server" class="drpdown" style="width: 150px" tabindex="6">
                                                                 </select>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Print">
                                                             <ItemTemplate>
                                                                 <asp:LinkButton ID="lnkPrint" runat="server" CommandArgument='<%# Ctype(Container,GridViewRow).RowIndex %>'
                                                                     CommandName="Print" Text="Print"></asp:LinkButton>
                                                             </ItemTemplate>
                                                             <HeaderStyle Width="50px" />
                                                             <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                         </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Email">
                                                             <ItemTemplate>
                                                                 <asp:LinkButton ID="lnkMail" runat="server" CommandArgument='<%# Ctype(Container,GridViewRow).RowIndex %>'
                                                                     CommandName="Mail" Text="Mail"></asp:LinkButton>
                                                             </ItemTemplate>
                                                             <HeaderStyle Width="50px" />
                                                             <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                         </asp:TemplateField>
                                                     </Columns>
                                                     <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                                                     <HeaderStyle CssClass="grdheader" Wrap="false" />
                                                     <AlternatingRowStyle CssClass="grdAternaterow" />
                                                 </asp:GridView> </TD><TD style="WIDTH: 50px"></TD></TR></TBODY></TABLE></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 16px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 16px" class="td_cell" align=left></TD><TD style="WIDTH: 150px; HEIGHT: 16px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 16px" class="td_cell" align=left></TD><TD style="HEIGHT: 16px" class="td_cell" align=left colSpan=2>
    <asp:Button id="btnBack" runat="server" Text="Return To Search" CssClass="btn"></asp:Button></TD><TD style="WIDTH: 100px; HEIGHT: 16px" class="td_cell" align=left></TD><TD style="WIDTH: 100px; HEIGHT: 16px" class="td_cell" align=left></TD><TD style="WIDTH: 101px; HEIGHT: 16px" class="td_cell" align=left></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

