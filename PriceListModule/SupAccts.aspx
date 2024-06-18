<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupAccts.aspx.vb" Inherits="SupAccts"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
     <%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
    <%@ OutputCache location="none" %> 
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="javascript" type="text/javascript" >

<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->


 
function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}
			

    function CallWebMethod(methodType)
    {
        switch(methodType)
        {
            case "postcode":
                var select=document.getElementById("<%=ddlPostCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlPostName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "postname":
                var select=document.getElementById("<%=ddlPostName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlPostCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;

            case "acccode":
                var select=document.getElementById("<%=ddlAccCode.ClientId%>");
                var selectname=document.getElementById("<%=ddlAccName.ClientId%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
           case "accname":
                var select=document.getElementById("<%=ddlAccName.ClientId%>");
                var selectname=document.getElementById("<%=ddlAccCode.ClientId%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;    
          case "accrualcode":
                var select=document.getElementById("<%=ddlAccrualCode.ClientId%>");
                var selectname=document.getElementById("<%=ddlAccrualname.ClientId%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
           case "accrualname":
                var select=document.getElementById("<%=ddlAccrualname.ClientId%>");
                var selectname=document.getElementById("<%=ddlAccrualCode.ClientId%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "partycode":
                var select = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "invoicepostcode":
                var select = document.getElementById("<%=ddlInvPostCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlInvPostName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "invoicepostname":
                var select = document.getElementById("<%=ddlInvPostName.ClientID%>");
                var selectname = document.getElementById("<%=ddlInvPostCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
                //Added invoicepostcode and invoicepostname by Archana on 25/06/2015
        }
    }


</script>
 <style>
 .bgrow
 {
 background-color:white; 

 }

 </style>
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE align=left><TBODY>
                 <tr>
                <td colspan ="20" align ="left" class="bgrow" >
               
                    <whc:hoteltab ID="whotelatbcontrol" runat="server" />
               
                
                </td>
                </tr>
                 <tr>
                <td>
                <div style="margin-top:-6px;margin-left:13px;">
                <table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid;" class="td_cell" align=left>

                <TR><TD style="HEIGHT: 18px" vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier" Width="100%" CssClass="field_heading" ForeColor="White"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>
    &nbsp;</TD><TD class="td_cell" vAlign=top align=left>Code<span class="td_cell" 
            style="COLOR: #ff0000"> * </span><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> 
            &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR>
    <tr>
        <td align="left" valign="top" width="150">
            &nbsp;</td>
        <td align="left" class="td_cell" valign="top">
            <table style="width: 100%">
                <tr>
                    <td>
                        Supplier</td>
                    <td style="width: 231px">
                        <select ID="ddlSuppierCD" runat="server" class="field_input" name="D1" 
                            onchange="CallWebMethod('partycode');" style="WIDTH: 220px">
                            <option selected=""></option>
                        </select></td>
                    <td style="width: 325px">
                        <select ID="ddlSuppierNM" runat="server" class="field_input" name="D2" 
                            onchange="CallWebMethod('partyname');" style="WIDTH: 310px">
                            <option selected=""></option>
                        </select></td>
                    <td>
                        <asp:Button ID="btnfilldetail" runat="server" CssClass="field_button" 
                            Text="Fill Details" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 450px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 656px">
<TBODY><TR>
<TD style="WIDTH: 80px" class="td_cell" colSpan=2>
<asp:Panel id="PanelAccounts" runat="server" Width="699px" GroupingText="Account Details">
<TABLE style="WIDTH: 583px">
<TBODY><TR>
<TD style="WIDTH: 180px; " align=left>Telephone</TD>
<TD style="WIDTH: 475px; HEIGHT: 15px" align=left>
<INPUT style="WIDTH: 400px" id="txtAccTelephone1" class="field_input" tabIndex=41 type=text maxLength=50 runat="server" />
</TD></TR>
<TR><TD style="WIDTH: 180px" align=left></TD>
<TD style="WIDTH: 475px" align=left>
<INPUT style="WIDTH: 400px" id="txtAccTelephone2" class="field_input" tabIndex=42 type=text maxLength=50 runat="server" />
</TD>
</TR>
<TR><TD style="WIDTH: 180px" align=left>Mobile no.</TD>
<TD style="WIDTH: 475px" align=left>
<INPUT style="WIDTH: 400px" id="txtAccmobile" class="field_input" tabIndex=42 type=text maxLength=50 runat="server" />
</TD></TR>
<TR>
<TD style="WIDTH: 180px" align=left>Fax</TD>
<TD style="WIDTH: 475px" align=left><INPUT style="WIDTH: 400px" id="txtAccFax" class="field_input" tabIndex=43 type=text maxLength=50 runat="server" />
</TD>
</TR>
<TR><TD style="WIDTH: 180px" align=left>Contact</TD>
<TD style="WIDTH: 475px" align=left>
<INPUT style="WIDTH: 400px" id="txtAccContact1" class="field_input" tabIndex=44 type=text maxLength=100 runat="server" />
</TD>
</TR>
<TR><TD style="WIDTH: 180px" align=left></TD>
<TD style="WIDTH: 475px" align=left>
<INPUT style="WIDTH: 400px" id="txtAccContact2" class="field_input" tabIndex=45 type=text maxLength=100 runat="server" />
</TD></TR>
<TR>
<TD style="WIDTH: 180px" align=left>E-mail</TD>
<TD style="WIDTH: 475px" align=left>
<INPUT style="WIDTH: 400px" id="txtAccEmail" class="field_input" tabIndex=46 type=text maxLength=100 runat="server" />
</TD></TR>
<TR><TD style="WIDTH: 180px" align=left>Control A/C Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN>
</TD>
<TD style="WIDTH: 475px" align=left><SELECT style="WIDTH: 150px" id="ddlAccCode" class="field_input" tabIndex=61 onchange="CallWebMethod('acccode')" runat="server">
 <OPTION selected></OPTION>
 </SELECT> 
 <SELECT style="WIDTH: 255px" id="ddlAccName" class="field_input" tabIndex=62 onchange="CallWebMethod('accname')" runat="server"> 
 <OPTION selected></OPTION></SELECT> 
 </TD></TR>
  <TR><TD style="WIDTH: 180px" align=left>&nbsp;</TD>
  <TD style="WIDTH: 475px" align=left>
  <INPUT id="ChkCashSup" tabIndex=48 type=checkbox runat="server" />
      <asp:Label ID="lblcashsupagent" runat="server" Height="15px" 
          Text="Cash Supplier Agent" Width="147px"></asp:Label>
      </TD></TR>
  <TR><TD style="WIDTH: 180px" align=left>Credit Days</TD>
  <TD style="WIDTH: 475px" align=left>
  <INPUT style="WIDTH: 150px; TEXT-ALIGN: right" id="TxtAccCreditDays" class="field_input" tabIndex=49 type=text maxLength=5 runat="server" /> &nbsp; &nbsp; Credit Limit&nbsp;&nbsp; 
  <INPUT style="WIDTH: 150px; TEXT-ALIGN: right" id="txtAccCreditLimit" class="field_input" tabIndex=50 type=text maxLength=15 runat="server" />
  </TD></TR>
  <TR><TD style="WIDTH: 180px; " align=left>Post To</TD>
  <TD style="WIDTH: 475px; HEIGHT: 22px" align=left>&nbsp;<SELECT style="WIDTH: 150px" id="ddlPostCode" class="field_input" tabIndex=51 onchange="CallWebMethod('postcode');" runat="server"> 
  <OPTION selected></OPTION></SELECT>&nbsp; 
  <SELECT style="WIDTH: 255px" id="ddlPostName" class="field_input" tabIndex=52 onchange="CallWebMethod('postname');" runat="server"> 
  <OPTION selected></OPTION></SELECT></TD></TR>

        <TR>
        <TD style="WIDTH: 180px" align=left>Invoice Post</TD>
  <TD style="WIDTH: 475px" align=left>
  <INPUT id="chkInvPost" tabIndex=63 type=checkbox runat="server" />(Use this if GL 
      posting instead of supplier posting required)</TD>

    </TR>
   

    <%--Added Invoice Post checkbox by Archana on 25/06/2015--%>

    <TR>
    <TD style="WIDTH: 180px" align=left>Invoice&nbsp;Post A/C</TD>
 <TD style="WIDTH: 475px" align=left>
 <SELECT style="WIDTH: 150px" id="ddlInvPostCode" class="field_input" tabIndex=64 onchange="CallWebMethod('invoicepostcode')" runat="server">
  <OPTION selected></OPTION></SELECT> 
  <SELECT style="WIDTH: 255px" id="ddlInvPostName" class="field_input" tabIndex=65 onchange="CallWebMethod('invoicepostname')" runat="server">
  <OPTION selected></OPTION></SELECT>
  </TD></TR>

  <%--Added ddlInvPostCode and ddlInvPostName by Archana on 25/06/2015--%>
    
    <tr>
        <td align="left" style="height: 0px;">
            &nbsp;</td>
        <td align="left" style="height: 0px;">
            <select ID="ddlAccrualCode" runat="server" class="field_input" name="D4" 
                onchange="CallWebMethod('accrualcode')" style="WIDTH: 150px; height: 0px;" tabindex="61" 
                visible="False">
                <option selected=""></option>
            </select>&nbsp;<select ID="ddlAccrualName" runat="server" class="field_input" 
                name="D5" onchange="CallWebMethod('accrualname')" style="WIDTH: 255px; height: 2px;" 
                tabindex="62" visible="False">
                <option selected=""></option>
            </select>
        </td>
    </tr>
    
    <tr>
        <td align="left" style="height: 0px;">
            <asp:Label ID="lblposttype" runat="server" Height="15px" Text="Post type" 
                Visible="False" Width="95px"></asp:Label>
        </td>
        <td align="left" style="height: 0px;">
            <select ID="ddlPosttype" runat="server" class="field_input" name="D3" 
                style="WIDTH: 420px" visible="False">
                <option selected="" value="[Select]">[Select]</option>
                <option value="1">Post Net Payable</option>
                <option value="2">Post Hotel Cost and Commission separately</option>
            </select></td>
    </tr>
    
    <tr>
        <td align="left" style="height: 0px;">
            <asp:Label ID="lblTRN" runat="server" Height="15px" Text="TRN No" 
                Width="95px"></asp:Label>
        </td>
        <td align="left" style="height: 0px;">
            <INPUT style="WIDTH: 150px; " id="TxtTRN" class="field_input" 
                tabIndex=49 type=text  runat="server" />
        </td>
    </tr>
    <tr>
        <td align="left" style="height: 0px;">
            &nbsp;</td>
        <td align="left" style="height: 0px;">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="left" style="WIDTH: 180px">
            <asp:Button ID="BtnAccSave" runat="server" CssClass="field_button" 
                onclick="BtnAccSave_Click" tabIndex="53" Text="Save" />
        </td>
        <td align="left" style="WIDTH: 475px">
            <asp:Button ID="BtnAccCancel" runat="server" CssClass="field_button" 
                onclick="BtnAccCancel_Click" tabIndex="54" Text="Return To Search" />
            &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" 
                onclick="btnhelp_Click" tabIndex="55" Text="Help" />
        </td>
    </tr>
    </TBODY></TABLE></asp:Panel> &nbsp; </TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel> <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy></DIV></TD></TR>
 </table>
                </div>
                </td>
                </tr>
</TBODY></TABLE>
</contenttemplate>
    
    </asp:UpdatePanel>
</asp:Content>


