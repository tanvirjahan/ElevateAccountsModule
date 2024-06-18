<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcursionDaysoftheweek.aspx.vb" Inherits="Other_Services_Selling_Types"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

    <%@ OutputCache location="none" %> 

     
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

 	 <script language="javascript" src="../TADDScript.js" type="text/javascript"></script>
<style type="text/css" >
            .ModalPopupBG
            {
                background-color: gray;
                filter: alpha(opacity=50);
                opacity: 0.7;
            }

            .HellowWorldPopup
            {
                min-width:200px;
                min-height:150px;
                background:white;
                font-size: 10pt;
	            font-weight: bold;
	            border-bottom-style:double;
	            border-width:medium;

	
            }
      
        *{
	outline:none;
           
        }
    .style4
    {
        width: 111px;
    }
    .style5
    {
        width: 124px;
    }
    .NoDisplay
    {
       display:none; 
    }
</style>


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
if (document.getElementById("<%=ddlexccode.ClientID%>").value=="[Select]")
{
   
  //  ("<%=ddlexcname.ClientID%>").value=="[Select]") 
     {
           document.getElementById("<%=ddlexccode.ClientID%>").focus();
           alert("Select Excursion Code");
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
       
	var ddl = document.getElementById("<%=ddlexcname.ClientID%>");
	ddl.selectedIndex = -1;
	// Iterate through all dropdown items.
	
		for (var i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlexccode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}




function  GetValueCode()
{
	var ddl = document.getElementById("<%=ddlexccode.ClientID%>");
	ddl.selectedIndex = -1;
	//alert(document.getElementById("<%=ddlexcname.ClientID%>").value);
		// Iterate through all dropdown items.
		for (var i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == document.getElementById("<%=ddlexcname.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}

function selectall() {

    var mygrid = document.getElementById("<%=gv_days.ClientID%>");
    var mygrid = document.getElementById("<%=chkselectall.ClientID%>");
    for (i = 1; i < 7; i++) {
        //if (mygrid.elements[i].type == "checkbox") {
            mygrid.elements[i].checked = document.getElementById(chkselectall).checked;
       // }
    }

   
}


   
</script>




    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="border: 2px solid gray; WIDTH: 878px; " ><TBODY><TR><TD class="td_cell" align=center colSpan=4>
    <asp:Label id="lblHeading" runat="server" Text=" Excursion Days of the Week" 
        CssClass="field_heading" Width="719px" Height="18px"></asp:Label></TD>

    </TR>
    <TR style="COLOR: #ff0000">
        <td style="height: 24px; width: 827px;"></td></td></TR><TR>
    <TD style="WIDTH: 827px; HEIGHT: 23px" class="td_cell">
        Excursion Code</TD>
    <TD style="COLOR: #000000; width: 504px; height: 23px;">
        <select id="ddlexccode" runat="server" class="drpdown" name="D1" 
            onchange="GetValueFrom()" style="WIDTH: 189px" tabindex="3">
            <option selected=""></option>
        </select>
    </TD>
    <TD style="WIDTH: 827px; HEIGHT: 23px" class="td_cell">
        Excursion Name</TD>
    <td style="height: 23px; width: 856px">
    <select id="ddlexcname" runat="server" class="drpdown" name="D2" 
        onchange="GetValueCode()" style="WIDTH: 201px" tabindex="4">
        <option selected=""></option>
    </select>
    </td></TR><TR>
    <TD style="WIDTH: 827px; HEIGHT: 22px" class="td_cell">&nbsp;</TD>
   
    

    <TD style="height: 29px">&nbsp;</TD>

             <td class="td_cell" style="width: 856px; height: 29px;">
                 &nbsp;</td>
        <td style="height: 22px; width: 294px;">
            &nbsp;</TD>
            </TR>
</table>
<!-- gridview -->
   
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px"    GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"  AutoGenerateColumns="False" AllowSorting="True" AllowPaging="false">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>



<asp:TemplateField Visible="False" HeaderText="Nationality Code">          

<ItemTemplate>
      <asp:Label ID="lblothtypcode" runat="server" Text='<%# Bind("nationalitycode") %>'></asp:Label>

    
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField  DataField="nationalitycode" SortExpression="nationalitycode" HeaderText="Nationality Code">
<ItemStyle HorizontalAlign="Left" CssClass="NoDisplay"  ></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="nationlanguage" SortExpression="nationlanguage" HeaderText=" Language">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="daysofweek"  SortExpression="daysofweek" HeaderText=" Days of the Week">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:ButtonField HeaderText="Action" Text="Edit"  CommandName="Editrow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>




</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>


</asp:GridView>


   <!--gridvew -->  

   <!--checkbox grid -->


    <table>
   
   <tr>
       <td style="height: 27px;">
           <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="6" Text="Save" 
               Width="50px" />
       </td>
      
           <td style="width: 504px; height: 27px;">
               <asp:Button ID="btnCancel" runat="server" CssClass="btn" 
                   onclick="btnCancel_Click" tabIndex="7" Text="Return To Search" />
             &nbsp 
               <asp:Button ID="btnhelp" runat="server" CssClass="btn" 
                   onclick="btnhelp_Click" tabIndex="8" Text="Help" />
               <td style="width: 856px">
               </td>
              
               <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
                   Text="Webserviceerror"></asp:Label>
           </td>
      
    </tr>
    </table>








    <div id ="daysgrid" style="display:none;" runat ="server">

     <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="visibility:hidden" />
                    <input id="btnOkay" type="button" value="OK" style="visibility:hidden"  />
                    <input id="btnCancel1" type="button" value="Cancel" style="visibility:hidden" />





<Ajax:modalpopupextender id="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
	cancelcontrolid="btnCancel1" okcontrolid="btnOkay" 
	targetcontrolid="btnInvisibleGuest" popupcontrolid="daysgrid"
	popupdraghandlecontrolid="PopupHeader" drag="true" 
	backgroundcssclass="ModalPopupBG">
</Ajax:modalpopupextender>

            <asp:GridView ID="gv_days" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#999999" CssClass="td_cell" tabIndex="37">
                <FooterStyle CssClass="grdfooter" />
                <Columns>
                    <asp:TemplateField HeaderText="Nationality Code" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lbldayswkcode" runat="server" Text='<%# Bind("dayswkcode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <input id="chkdays" type="checkbox" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="dayswkname" HeaderText="Days of the Week">
                    </asp:BoundField>
                </Columns>
                <RowStyle CssClass="grdRowstyle" />
                <SelectedRowStyle CssClass="grdselectrowstyle" />
                <HeaderStyle CssClass="grdheader" />
            </asp:GridView>

            
<table>
<tr>
<td><asp:CheckBox id="chkselectall" tabIndex=38  runat="server" Text="Select All" CssClass="chkbox" Width="100px" AutoPostBack="True" ></asp:CheckBox></TD>
<td><asp:Button ID ="save" Text ="Save" runat ="server" CssClass ="btn"></asp:button></td>

<td>&nbsp<asp:Button ID ="btnexit" Text ="Exit" runat ="server" CssClass ="btn"></asp:button></td>
</tr>
</table>

               

            </div>

</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

