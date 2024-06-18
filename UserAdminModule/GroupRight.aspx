<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="GroupRight.aspx.vb" Inherits="GroupRight"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
     <script language="javascript" type="text/javascript">
     function SelectAllChildNodes()
          {
        //debugger;
        var obj = window.event.srcElement;
        var treeNodeFound = false;
        var checkedState;
      
        if (obj.tagName == "INPUT" && obj.type == "checkbox")
        {  
           var treeNode = obj;
            checkedState = treeNode.unchecked;
            do
            {
                obj = obj.parentElement;
            } while (obj.tagName != "TABLE")
            
            var parentTreeLevel = obj.rows[0].cells.length;            
            var parentTreeNode = obj.rows[0].cells[0];
            var tables = obj.parentElement.getElementsByTagName("TABLE");
            var numTables = tables.length;

           
           
            if(confirm('Select/Unselect all the submenus under this menu?')==false)return;
            if (numTables >= 0)
            {
                for (iCount=0; iCount < numTables; iCount++)
                {
                    if (tables[iCount] == obj)
                    {
                        treeNodeFound = true;
                        iCount++;
                        if (iCount == numTables)
                        {
                            return;
                        }
                    }
                    if (treeNodeFound == true)
                    {
                     
                        var childTreeLevel = tables[iCount].rows[0].cells.length;
                        if (childTreeLevel > parentTreeLevel)
                        {   
                            
                            var cell = tables[iCount].rows[0].cells[childTreeLevel - 1];
                            var inputs = cell.getElementsByTagName("INPUT");
                          
                            if(inputs[0].checked=='true')
                             {
                               checkedState = treeNode.unchecked;
                             }
                             else{
                               checkedState = treeNode.checked;
                             }
                            inputs[0].checked = checkedState;
                           
                            
                        }
                        else
                        {
                            return;
                        }
                    }
                }
             }
             
        }
      
    }
    
    
    
     </script>
     <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<TABLE style="HEIGHT: 521px" class="td_cell"><TBODY><TR><TD style="HEIGHT: 3px; TEXT-ALIGN: center" colSpan=2><asp:Label id="lblHeading" runat="server" Text="Group Rights" Width="932px" CssClass="field_heading"></asp:Label> </TD><TD style="HEIGHT: 3px; TEXT-ALIGN: center" colSpan=1></TD></TR><TR><TD style="WIDTH: 565px"></TD><TD style="WIDTH: 532px"></TD><TD style="WIDTH: 532px"></TD></TR><TR><TD style="WIDTH: 565px" align=center>Group&nbsp; <asp:DropDownList id="ddlGroup" tabIndex=1 runat="server" Width="131px" CssClass="field_input" AutoPostBack="True">
</asp:DropDownList></TD><TD align=center>Application&nbsp; <asp:DropDownList id="ddlApplication" tabIndex=2 runat="server" Width="131px" CssClass="field_input" AutoPostBack="True"></asp:DropDownList> <asp:TextBox id="txtMenuId" runat="server" Width="1px" Visible="False"></asp:TextBox></TD><TD align=center></TD></TR>


<TR><TD style="WIDTH: 565px" rowSpan=2><asp:Panel id="Panel1" runat="server" Height="500px" Width="300px" ScrollBars="Both"><asp:TreeView id="tvMenu" tabIndex=3 onclick="SelectAllChildNodes()" runat="server" ShowLines="True" ExpandDepth="0" ShowCheckBoxes="All"></asp:TreeView> </asp:Panel></TD><TD vAlign=top colSpan=1><asp:Panel id="pnlfunction" runat="server" Height="225px" Width="412px" ScrollBars="Auto">

<asp:GridView id="grdFunctional" tabIndex=4 runat="server" Font-Size="10px" Height="88px" BackColor="White" CssClass="td_cell" Width="305px" Enabled="False" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" PageSize="20">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:BoundField Visible="False" HeaderText="Sr No"></asp:BoundField>
<asp:TemplateField HeaderText="Select">
<ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle Width="60px" HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="fncode" HeaderText="Functional Code">
<ItemStyle Width="400px"></ItemStyle>

<HeaderStyle Width="400px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="fnRight" HeaderText="Functional Rights">
<ItemStyle Width="1000px"></ItemStyle>

<HeaderStyle Width="1000px"></HeaderStyle>
</asp:BoundField>
</Columns>

<RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> </asp:Panel> <TABLE style="WIDTH: 183px"><TBODY><TR><TD style="HEIGHT: 29px">
<asp:Button id="btnEditRight" tabIndex=5 runat="server" Text="Edit Rights" 
            CssClass="field_button"></asp:Button> </TD>
<TD style="WIDTH: 6px; HEIGHT: 29px"><asp:Button id="btnSaveRights" tabIndex=6 
        runat="server" Text="Save Rights" CssClass="field_button"></asp:Button></TD>
<TD style="WIDTH: 8px; HEIGHT: 29px"><asp:Button id="btnCancelRight" tabIndex=7 
        runat="server" Text="Cancel" CssClass="field_button"></asp:Button></TD></TR>
      
   
        
        </TBODY></TABLE></TD>
<TD vAlign=top colSpan=1></TD></TR>

   
     
         <input id="btnInvisibleEBGuest1" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnOkayEB1" type="button" value="OK" style="visibility: hidden" />
                                    <input id="btnCancelEB1" type="button" value="Cancel" style="visibility: hidden" />
                                </div>
         <asp:ModalPopupExtender ID="ModalExtraPopup1" runat="server" BehaviorID="ModalExtraPopup1"
                                    CancelControlID="btnCancelEB1" OkControlID="btnOkayEB1" TargetControlID="btnInvisibleEBGuest1"
                                    PopupControlID="pnlPopup" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                </asp:ModalPopupExtender>
        
         <asp:Panel ID="pnlPopup"  CssClass="modalPopup" runat="server" style="overflow: scroll; height: 135px; width: 250px;
                                    border: 3px solid green; background-color: White; display: none">
    <div class="body" align=center >
     <br />
        You  have not Selected any Functional Rights.Do You want to save  anyway
        <br />
        <br />

         <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_Click" CssClass="field_button"/>
        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="field_button" />
       
    </div>
</asp:Panel>


<TR><TD style="HEIGHT: 241px"><asp:Panel id="pnlSubMenu" runat="server" Height="225px" Width="408px" ScrollBars="Auto">

<asp:GridView id="grdSubMenu" tabIndex=8 runat="server" Font-Size="10px" Height="88px" BackColor="White" CssClass="td_cell" Width="306px" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" PageSize="20">
<FooterStyle CssClass="grdfooter"  ForeColor="Black"></FooterStyle>



<Columns>
<asp:BoundField Visible="False" HeaderText="Sr No"></asp:BoundField>
<asp:TemplateField HeaderText="Select">
<ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle Width="60px" HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="submenuid" HeaderText="Sub Menu Id">
<ItemStyle Width="400px"></ItemStyle>

<HeaderStyle Width="400px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="submenuname" HeaderText="Sub Menu Name">
<ItemStyle Width="1000px"></ItemStyle>

<HeaderStyle Width="1000px"></HeaderStyle>
</asp:BoundField>
</Columns>

<RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> </asp:Panel> 


<TABLE style="WIDTH: 183px"><TBODY><TR><TD style="HEIGHT: 29px"></TD><TD style="WIDTH: 6px; HEIGHT: 29px">
        <asp:Button id="btnSaveSubMenu" tabIndex=9 runat="server" 
            Text="Save Sub Menu  Rights" CssClass="field_button"></asp:Button></TD>
<TD style="WIDTH: 8px; HEIGHT: 29px"><asp:Button id="BtnSubMenuCancel" tabIndex=10 
        runat="server" Text="Cancel" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE></TD><TD style="HEIGHT: 241px"></TD></TR><TR><TD style="HEIGHT: 17px" colSpan=2><TABLE style="WIDTH: 930px"><TBODY>
<TR><TD style="WIDTH: 85px; HEIGHT: 24px"><asp:Label id="lblFunRights" runat="server" Text="Functional Rights    " CssClass="field_caption" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 88px; HEIGHT: 24px"><SELECT style="WIDTH: 165px" id="ddlGrpFunCode" class="field_input" tabIndex=11  runat="server" Visible="true"> <OPTION selected></OPTION></SELECT></TD><TD colSpan=3>
    <asp:Button id="BtnMenuClear" tabIndex=12 onclick="BtnMenuClear_click" 
        runat="server" Text="Clear Menu" __designer:dtid="562949953421323" 
        CssClass="field_button" __designer:wfdid="w3"></asp:Button>
 &nbsp;<asp:Button id="btnSaveRightsAll" tabIndex=13 
        onclick="btnSaveRightsAll_Click" runat="server" 
        Text="Add Rights For Selected Menu" CssClass="field_button" 
        __designer:wfdid="w4"></asp:Button>
&nbsp;<asp:Button id="btnRemoveRightsAll" tabIndex=14 
        onclick=" btnRemoveRightsAll_Click" runat="server" 
        Text="Remove Rights For  Selected Menu" CssClass="field_button" 
        __designer:wfdid="w5"></asp:Button>&nbsp;&nbsp; </TD></TR></TBODY></TABLE></TD><TD style="HEIGHT: 17px" colSpan=1></TD></TR>
<TR><TD align=right colSpan=2><asp:Button id="btnSave" tabIndex=15 runat="server" 
        Text="Save Group" __designer:dtid="562949953421320" CssClass="field_button" 
        __designer:wfdid="w5"></asp:Button>&nbsp;
 <asp:Button id="btnClearGroup" tabIndex=16 runat="server" Text="Clear Group" 
        __designer:dtid="562949953421323" CssClass="field_button" __designer:wfdid="w6"></asp:Button>&nbsp;
  <asp:Button id="btnExit" tabIndex=17 runat="server" Text="Return to Search" 
        __designer:dtid="562949953421326" CssClass="field_button" __designer:wfdid="w7"></asp:Button>&nbsp;
  <asp:Button id="btnHelp" tabIndex=18 onclick="btnHelp_Click" runat="server" 
        Text="Help" __designer:dtid="1688858450198528" CssClass="field_button" 
        __designer:wfdid="w4"></asp:Button></TD><TD style="HEIGHT: 17px" colSpan=1></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel> 
</asp:Content>