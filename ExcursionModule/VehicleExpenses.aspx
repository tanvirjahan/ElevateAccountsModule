<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="VehicleExpenses.aspx.vb" Inherits="Other_Services_Selling_Types"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
     <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript " type ="text/javascript" >
function checkNumber()
{
   if (event.keyCode < 45 || event.keyCode > 57)
    {
         return false;
    }
}

function FormValidation(state)
{
if ((document.getElementById("<%=txtexpid.ClientID%>").value=="")||(document.getElementById("<%=ddldcode.ClientID%>").value=="[Select]"))
{
    
      if (document.getElementById("<%=ddldcode.ClientID%>").value=="[Select]") 
     {
           document.getElementById("<%=ddldcode.ClientID%>").focus();
           alert("Select Driver ");
            return false;
     }
             
 }
 else
 {
       if (state=='New'){if(confirm('Are you sure you want to Save Handling Fees Selling Types?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to Update Handling Fees Selling Types?')==false)return false;}
       if (state == 'Delete') { if (confirm('Are you sure you want to Delete Handling Fees Selling Types?') == false) return false; }
 }
}  
   function  GetValueFrom()
{

    var ddl = document.getElementById("<%=ddldname.ClientID%>");
    var drpdn = document.getElementById("<%=grdrecord.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddldcode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}




function  GetValueCode()
{
	var ddl = document.getElementById("<%=ddldcode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddldname.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


function FillNamenew(CodeId, CodeName) {


    var select = document.getElementById(CodeId);
    var selectname = document.getElementById(CodeName);
    selectname.value = select.options[select.selectedIndex].text;


}
function FillNamenew1(CodeId, CodeName) {

    var select = document.getElementById(CodeId);
    var selectname = document.getElementById(CodeName);
    select.value = selectname.options[selectname.selectedIndex].text;


}
var ddlCommon;
//04082014
function fillvehbontype(ddlexptype,VehicleID) {

      var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

    var mytype = document.getElementById(ddlexptype);
        ddlCommon = document.getElementById(VehicleID);
    ColServices.clsServices.fillvehibontype(constr, mytype.value, fillmyvehcombo, ErrorHandler, TimeOutHandler);

}

function fillmyvehcombo(result) {


    RemoveAll(ddlCommon)
    for (var i = 0; i < result.length; i++) {
        var option = new Option(result[i].ListText, result[i].ListValue);
        ddlCommon.options.add(option);
    }
    ddlCommon.value = "[Select]";

}



function grdTotalnew() {


    var objGridView = document.getElementById('<%=grdRecord.ClientID%>');

    //var intRows=parseInt('<%=grdRecord.Rows.Count %>')
    var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');
    var tot = document.getElementById('<%=txttot.ClientID%>');
    var tot2 = 0;
    var tot3 = 0;
    //intRows=txtrowcnt.value;
    var j = 0;

    if (txtrowcnt != undefined || txtrowcnt != null) {
        for (j = 1; j <= parseInt(txtrowcnt.value); j++) {


            var tot1 = objGridView.rows[j].cells[5].children[0].value;

       
            if (tot1 == '') { tot1 = 0; }


            if (isNaN(tot1) == true) { tot1 = 0; }


            if (isNaN(parseFloat(tot1)) == false) {
                tot2 = parseFloat(tot1);

            }

            tot3 = tot3 + tot2



        }

        tot.value = tot3

    }



}

function findreqid() {

}

function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 46 || charCode > 57))
        return false;

    return true;
}

function TimeOutHandler(result) {
    alert("Timeout :" + result);
}

function ErrorHandler(result) {
    var msg = result.get_exceptionType() + "\r\n";
    msg += result.get_message() + "\r\n";
    msg += result.get_stackTrace();
    alert(msg);
}

</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="border: 2px solid gray; WIDTH: 1000px; "><TBODY><TR><TD class="td_cell" align=center colSpan=4>
    <asp:Label id="lblHeading" runat="server" Text="Add New Vehicle Expenses" 
        CssClass="field_heading" Width="903px" Height="18px"></asp:Label></TD>

    </TR>
    <TR style="COLOR: #ff0000">
    <TD style="WIDTH: 148px; height: 24px;" class="td_cell"><SPAN style="COLOR: #000000">
        Exp.Id </SPAN><SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
    <TD style="COLOR: #000000; height: 24px; width: 86px;">
        <asp:TextBox ID="txtexpid" locked =true; runat="server" CssClass="fiel_input" 
                 Width="110px"></asp:TextBox>
             
        </TD>
        <td style="width: 147px"></td>
        <td style="height: 24px; width: 205px;">
              <asp:TextBox ID="txtDate" runat="server" CssClass="fiel_input" 
                 Width="100px"></asp:TextBox>
               
               
                <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                 PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" 
            TargetControlID="txtDate">
             </cc1:CalendarExtender>

             <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                 MaskType="Date" TargetControlID="txtDate">
             </cc1:MaskedEditExtender>

             <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                 ImageUrl="~/Images/Calendar_scheduleHS.png" />
          
          
             <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                 ControlExtender="MeFromDate" ControlToValidate="txtDate" 
                 CssClass="field_error" Display="Dynamic" 
                 EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                 ErrorMessage="" InvalidValueBlurredMessage="Invalid Date" 
                 InvalidValueMessage="Invalid Date" 
                 TooltipMessage="Input a Date in Date/Month/Year">
             </cc1:MaskedEditValidator>


        
        </td></td></TR><TR>
    <TD style="WIDTH: 148px; HEIGHT: 22px" class="td_cell">Driver Code</TD>
   
    <TD style="HEIGHT: 22px; width: 86px;" align=left>
       <%-- <SELECT 
            onchange="GetValueFrom()" style="WIDTH: 197px" id="ddldcode" class="drpdown" 
            tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT>  
            
            <asp:DropDownList ID="ddldcode" runat="server" Width ="197px" CssClass="drpdown" AutoPostBack="true" onchange="GetValueFrom()"></asp:DropDownList>--%>

                        <SELECT style="WIDTH: 197px" id="ddldcode" class="drpdown" tabIndex=3 onchange="GetValueFrom()" runat="server">
<OPTION selected></OPTION></SELECT>
            </td> 

             <td class="td_cell" style="width: 147px; height: 29px;">
                 Driver&nbsp; Name</td>
        <td style="height: 22px; width: 181px;">
    <%--    <SELECT 
            onchange="GetValueCode()" style="WIDTH: 201px" id="ddldname" class="drpdown" 
            tabIndex=4 runat="server"> <OPTION selected></OPTION></SELECT> 
            
            <asp:DropDownList ID="ddldname" runat="server" Width ="201px"  CssClass="drpdown" AutoPostBack="true" onchange="GetValueCode()"></asp:DropDownList> --%>
            
            <SELECT style="WIDTH: 201px" id="ddldname" class="drpdown" tabIndex=4 onchange="GetValueCode()" runat="server">
<OPTION selected></OPTION></SELECT>

  
            </TD>
            <td><asp:Button id="btnloadgrid"  runat="server" Width="96px" Text="Load Grid"   CssClass="field_button"  ></asp:Button></td></TR>
   <table>
   <!--gridview -->
   <TR><TD style="WIDTH: 1000px" colSpan=3><DIV style="WIDTH: 992px; HEIGHT: 300px" 
           id="container" ><asp:GridView id="grdRecord" tabIndex=7 runat="server" 
           Width="989px" Font-Size="10px" CssClass="td_cell" BackColor="White" 
           OnSelectedIndexChanged="grdRecord_SelectedIndexChanged" GridLines="Vertical" 
           CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" 
           AutoGenerateColumns="False">

<RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="LineNo" Visible="False"><EditItemTemplate>
<asp:TextBox id="TextBox8" runat="server" Text="" __designer:wfdid="w5"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblLineNo" runat="server" Text="" __designer:wfdid="w4"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Date" ><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>

  <asp:TextBox ID="txtgriddate" runat="server" CssClass="fiel_input" 
                 Width="70px"></asp:TextBox>
                 <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                 PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" 
            TargetControlID="txtgriddate">
             </cc1:CalendarExtender>
             <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                 MaskType="Date" TargetControlID="txtgriddate">
             </cc1:MaskedEditExtender>
             <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                 ImageUrl="~/Images/Calendar_scheduleHS.png" />
             <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                 ControlExtender="MeFromDate" ControlToValidate="txtgriddate" 
                 CssClass="field_error" Display="Dynamic" 
                 EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                 ErrorMessage="" InvalidValueBlurredMessage="Invalid Date" 
                 InvalidValueMessage="Invalid Date" 
                 TooltipMessage="Input a Date in Date/Month/Year">
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</cc1:MaskedEditValidator>

</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="code"><EditItemTemplate>
<asp:TextBox id="txtcode" runat="server" Text="" __designer:wfdid="w5"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 100px" id="ddlcode"  class="field_input" tabIndex=3 runat="server" Visible="true"> <OPTION selected></OPTION></SELECT><BR />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Expense Name"><EditItemTemplate>
<asp:TextBox id="txtexpname" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 100px" id="ddlexpname"  class="field_input" tabIndex=3 runat="server" Visible="true"> <OPTION selected></OPTION></SELECT><BR />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Expense Type."><EditItemTemplate>
<asp:TextBox id="txtexptype" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 100px" id="ddlexptype"  class="field_input" tabIndex=3 runat="server" Visible="true">
 <option value="0">Transfers</option>
  <option value="1">Safari</option>
  <option value="2">Other</option>
 <OPTION selected></OPTION></SELECT><BR />

</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="requestid"><EditItemTemplate>
<asp:TextBox id="txtreqid" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:DropDownList style="WIDTH: 100px" id="ddlreqid" runat="server"  > </asp:DropDownList>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Amount"><EditItemTemplate>
<asp:TextBox id="txtamt" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<INPUT style="WIDTH: 65px;TEXT-ALIGN: right" id="txtamount"  onkeypress="return isNumberKey(event)" class="field_input" HorizontalAlign="right"  tabIndex=0 type=text maxLength=20 runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Remarks."><EditItemTemplate>
<asp:TextBox id="txtrmks" runat="server"></asp:TextBox>  

</EditItemTemplate>
<ItemTemplate>

<TEXTAREA  id="txtremarks"  class="field_input" 
                tabIndex=18 runat="server"></TEXTAREA>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Vehicle Code"><EditItemTemplate>
<asp:TextBox id="txtvehcode" runat="server"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 100px" id="ddlvehcode"  class="field_input" tabIndex=3 runat="server" Visible="true"> <OPTION selected></OPTION></SELECT><BR />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Delete"><ItemTemplate>
<asp:CheckBox id="chkDel" runat="server" Width="17px" __designer:wfdid="w24"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>

</Columns>



<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>

<PagerStyle HorizontalAlign="Center" BackColor="#999999" ForeColor="Black"></PagerStyle>

<SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

<HeaderStyle  backcolor="#454580"  Font-Bold="True" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle BackColor="Transparent" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView></DIV></TD></TR><tr><td><INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtgridrows" type=text maxLength=15 runat="server" /></td></tr>


    
    </table>
   <!--gridview -->
   <table> 
   <TR>
   <td>
     <input id="txtconnection" runat="server" 
                         style="visibility: hidden; width: 12px; height: 9px" type="text" />
   </td>
   <TD colSpan=3>
   <asp:Button id="btnAdd"  runat="server" Width="96px" Text="Add Row"   CssClass="field_button"  Font-Bold="False"></asp:Button>&nbsp;
   <asp:Button id="btnDelete"  runat="server" Width="96px" Text="Delete Row"   CssClass="field_button" Font-Bold="False"></asp:Button>
       &nbsp;</TD>

       
           
   </TR>
   <tr><td></td></tr>
   <tr>
       <td class="td_cell" style="WIDTH: 100px; HEIGHT: 22px">
           <asp:Label ID="lblrmks" runat="server" Text="Remarks"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txtremks" runat="server" Width="421px"></asp:TextBox>
       </td>

       <td class="td_cell" style="WIDTH: 96px; HEIGHT: 22px">
           <asp:Label ID="lbltot" runat="server" Text="Total"></asp:Label>
       </td>
       <td>
           <asp:TextBox ID="txttot" style="TEXT-ALIGN: right" runat="server" Width="80px"></asp:TextBox>
       </td>
       </tr>
       <tr><td></td></tr>
   </table>

  

     <table >
        <TR >
        <TD style=" height: 27px;"><asp:Button id="btnSave" tabIndex=6 runat="server" Text="Save" CssClass="btn" Width="46px"></asp:Button></TD>
                         
                 <td> <asp:Button id="btnprint"  runat="server" Width="96px" Text="Print"   CssClass="btn"  ></asp:Button></td>
                        <td style="width: 54px; height: 27px;">
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" 
                                onclick="btnCancel_Click" tabIndex="7" Text="Return To Search" />
                            </asp:button></td>
                           
                                                            <td>
                                    <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w20" CssClass="btn" 
                                        onclick="btnhelp_Click" tabIndex="8" Text="Help" />
                                    </asp:button>
                                </td>

                                <td>
                                    <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
                                        Text="Webserviceerror"></asp:Label>
                                </td>
              
              
            
            </TR>
    </table>     
        
        </TBODY></TABLE>
        <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>


                <asp:HiddenField ID="DropDriverCode" runat="server" />
                 <asp:HiddenField ID="GridFirstRowDrop" runat="server" />
                  <asp:HiddenField ID="NewModeCheck" runat="server" />

</contenttemplate>
    </asp:UpdatePanel>
   
    

</asp:Content>

