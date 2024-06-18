<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupOthers.aspx.vb" Inherits="SupOthers"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="javascript" type="text/javascript" >

    function CallWebMethod(methodType) {
        switch (methodType) {
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
        }
    }


</script>   
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier " Width="900px" CssClass="field_heading" ForeColor="White"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" disabled tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtName" class="field_input" disabled tabIndex=4 type=text maxLength=100 runat="server" /></TD></TR><TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 100%" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 656px"><TBODY><TR><TD style="WIDTH: 80px" class="td_cell" colSpan=2>&nbsp;<asp:Panel 
        id="PanelOtherServices" runat="server" Width="773px" 
        GroupingText="Other Services"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 15px"><STRONG>Group</STRONG></TD><TD style="WIDTH: 63px; HEIGHT: 15px"><asp:DropDownList id="ddlGroupCode" runat="server" Width="136px" CssClass="field_input" AutoPostBack="True" OnSelectedIndexChanged="ddlGroupCode_SelectedIndexChanged">
                    </asp:DropDownList></TD><TD style="HEIGHT: 15px" colSpan=2><asp:DropDownList id="ddlGroupName" runat="server" Width="248px" CssClass="field_input" AutoPostBack="True" OnSelectedIndexChanged="ddlGroupName_SelectedIndexChanged">
                    </asp:DropDownList></TD><TD style="WIDTH: 82px; HEIGHT: 15px"></TD><TD style="WIDTH: 100px; HEIGHT: 15px">&nbsp;</TD></TR><TR>
        <TD style="HEIGHT: 15px; width: 100px;"><strong> Supplier</strong></TD>
        <TD style="WIDTH: 63px; HEIGHT: 15px">
            <select ID="ddlSuppierCD" runat="server" class="field_input" name="D1" 
                onchange="CallWebMethod('partycode');" style="WIDTH: 190px">
                <option selected=""></option>
            </select></TD>
        <td colspan="2" style="HEIGHT: 15px">
            <select ID="ddlSuppierNM" runat="server" class="field_input" name="D2" 
                onchange="CallWebMethod('partyname');" style="WIDTH: 245px">
                <option selected=""></option>
            </select></td>
        <td style="WIDTH: 82px; HEIGHT: 15px">
            <asp:Button ID="btnfilldetail" runat="server" CssClass="field_button" 
                Text="Fill Details" />
        </td>
        <td style="WIDTH: 100px; HEIGHT: 15px">
            &nbsp;</td>
        </TR>
             <TR>
           <TD>&nbsp;</TD>
            <TD> &nbsp;</TD>
             <TD> &nbsp;</TD>
              <TD> &nbsp;</TD>
               <TD> &nbsp;</TD>
               </TR> 
        
        <TR><TD style="height: 147px;" align="left" valign="top"><STRONG>Group Selected</STRONG></TD>
            <TD colSpan=5 style="WIDTH: 100px; HEIGHT: 147px">
                <asp:GridView ID="GV_Group" runat="server" AutoGenerateColumns="False" 
                    Enabled="False" tabIndex="89" Width="607px" BorderColor="Black">
                    <Columns>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("othselected") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="othgrpcode" HeaderText="Oth. Srv. Grp. Code" 
                            ReadOnly="True" />
                        <asp:BoundField DataField="othgrpname" HeaderText="Oth. Srv. Grp. Name" 
                            ReadOnly="True" />
                    </Columns>
                </asp:GridView>
            </TD></TR>
            
           <TR>
           <TD>&nbsp;</TD>
            <TD> &nbsp;</TD>
             <TD> &nbsp;</TD>
              <TD> &nbsp;</TD>
               <TD> &nbsp;</TD>
               </TR> 
                    <TR>
           <TD>&nbsp;</TD>
            <TD> &nbsp;</TD>
             <TD> &nbsp;</TD>
              <TD> &nbsp;</TD>
               <TD> &nbsp;</TD>
               </TR> 

            
            <TR><TD style="WIDTH: 100px; "><strong>Types</strong> </TD>
            <TD colSpan=2>
                <asp:Button ID="BtnSelectothtype" runat="server" __designer:wfdid="w5" 
                    CssClass="field_button" onclick="BtnSelectothtype_Click" tabIndex="69" 
                    Text="Select All" />
                &nbsp;
                <asp:Button ID="BtnDeSelectothtype" runat="server" __designer:wfdid="w6" 
                    CssClass="field_button" onclick="BtnDeSelectothtype_Click" tabIndex="70" 
                    Text="DeSelect All" />
            </TD>
            <td style="WIDTH: 59px">
                <strong>Categories</strong>
            </td>
            <td style="WIDTH: 82px">
                <asp:Button ID="BtnSelectothcat" runat="server" __designer:wfdid="w2" 
                    CssClass="field_button" onclick="BtnSelectothcat_Click" tabIndex="69" 
                    Text="Select All" />
            </td>
            <td style="WIDTH: 100px">
                <asp:Button ID="BtnDeSelectothcat" runat="server" __designer:wfdid="w7" 
                    CssClass="field_button" onclick="BtnDeSelectothcat_Click" tabIndex="70" 
                    Text="DeSelect All" />
            </td>
        </TR>
        
        
        
        <TR><TD style="WIDTH: 100px; HEIGHT: 15px" align="left" colspan="3" 
                valign="top">
            <asp:GridView ID="GvTypes" runat="server" AutoGenerateColumns="False" 
                tabIndex="90" Width="397px">
                <Columns>
                    <asp:TemplateField>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("othtypselected") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="othtypcode" HeaderText="Type Code" 
                        HtmlEncode="False" />
                    <asp:BoundField DataField="othtypname" HeaderText="Type Name" />
                </Columns>
            </asp:GridView>
            </TD><TD style="WIDTH: 100px; HEIGHT: 15px" align="left" colspan="3" 
                valign="top">
                <asp:GridView ID="Gv_Category" runat="server" AutoGenerateColumns="False" 
                    tabIndex="91" Width="321px">
                    <Columns>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("othcatselected") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox3" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="othcatcode" HeaderText="Cat Code" />
                        <asp:BoundField DataField="othcatname" HeaderText="Cat Name" />
                    </Columns>
                </asp:GridView>
            </TD></TR><TR><TD style="WIDTH: 100px; height: 4px;">
            </TD><TD style="WIDTH: 63px; height: 4px;">
            </TD><TD style="WIDTH: 100px; height: 4px;"></TD>
            <TD style="WIDTH: 59px; height: 4px;">
            </TD><TD style="WIDTH: 82px; height: 4px;"></TD>
            <TD style="WIDTH: 100px; height: 4px;"></TD></TR><TR><TD style="WIDTH: 100px">
        <asp:Button ID="BtnSaveOther" runat="server" CssClass="field_button" 
            onclick="BtnSaveOther_Click" tabIndex="92" Text="Save" />
        </TD><TD style="WIDTH: 63px">
            <asp:Button ID="Btn_DelOthGrp" runat="server" CssClass="field_button" 
                onclick="Btn_DelOthGrp_Click" Text="Delete Selected Group" />
        </TD><TD style="WIDTH: 100px">
            <asp:Button ID="BtnCancelOther" runat="server" CssClass="field_button" 
                onclick="BtnCancelOther_Click" tabIndex="93" Text="Return to Search" />
        </TD><TD style="WIDTH: 59px">
            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w15" 
                CssClass="field_button" Height="19px" onclick="btnhelp_Click" tabIndex="94" 
                Text="Help" Width="37px" />
        </TD><TD style="WIDTH: 82px"></TD><TD style="WIDTH: 100px"></TD></TR>
        <tr>
            <td style="WIDTH: 100px">
            </td>
            <td style="WIDTH: 63px">
            </td>
            <td style="WIDTH: 100px">
            </td>
            <td style="WIDTH: 59px">
            </td>
            <td style="WIDTH: 82px">
            </td>
            <td style="WIDTH: 100px">
            </td>
        </tr>
        </TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

