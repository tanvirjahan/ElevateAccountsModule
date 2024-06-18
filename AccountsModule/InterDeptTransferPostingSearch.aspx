<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterDeptTransferPostingSearch.aspx.vb"
    Inherits="InterDeptTransferPostingSearch" MasterPageFile="~/AccountsMaster.master"
    Strict="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    
    <style type="text/css">
        .hiddencol
        {
            display: none;
        }
        
        .spanclass
        {
            text-decoration: none;
            display: inline;
            margin: 0;
            padding: 0px 0px 0px 30px;
            _padding: 0px 0px 0px 30px; /* this did the trick. Only IE6 should 
	padding-left:90px;*/
        }
        
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
            font-size: 10pt;
            font-weight: bold;
            border-bottom-style: double;
            border-width: medium;
        }
        
        #TextArea1
        {
            height: 142px;
            width: 418px;
        }
        #Text1
        {
            width: 91px;
        }
        #Select1
        {
            width: 225px;
        }
        #Select3
        {
            width: 166px;
        }
        #txtNotes
        {
            height: 22px;
            width: 181px;
        }
        
        
        #ddlArrDep
        {
            width: 35px;
        }
        
        
      
        .HideControl
        {
            display: none;
        }
        .style1
        {
            width: 849px;
            text-align: right;
        }
    </style>
    <asp:UpdatePanel ID="upnlInterDeptPost" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="border: 2px solid gray; width: 900px;">
                <tr>
                    <td align="center" class="field_heading" colspan="6">
                        &nbsp;Inter Department Transfer Posting Search
                    </td>
                </tr>
                <tr>
                    <td class="td_cell" align="center" style="color: blue" colspan="6">
                        Type few characters of code or name and click search
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="6">
                        <%--<asp:RadioButton ID="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell"
                            Checked="True" GroupName="GrSearch">
                        </asp:RadioButton>&nbsp;
                        <asp:RadioButton ID="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black"
                            CssClass="td_cell" GroupName="GrSearch" >
                        </asp:RadioButton>
                        &nbsp;&nbsp;--%>
                        <asp:Button ID="btnSearch" TabIndex="13" runat="server" Text="Search" Font-Bold="False"
                            CssClass="search_button"></asp:Button>&nbsp;
                        <asp:Button ID="btnClear" TabIndex="14" runat="server" Text="Clear" Font-Bold="False"
                            CssClass="search_button"></asp:Button>&nbsp;
                        <asp:Button ID="btnhelp" TabIndex="15" runat="server" Text="Help"
                            CssClass="search_button"></asp:Button>
                        &nbsp;
                        <asp:Button ID="btnAddNew" TabIndex="16" runat="server" Text="Add New" Font-Bold="True"
                            CssClass="btn"></asp:Button>&nbsp;
                        <asp:Button ID="btnPrint" TabIndex="18" runat="server" Text="Report" Visible="false" CssClass="btn">
                        </asp:Button>                        
                    </td>
                </tr>

                <tr>
                    <td>
                        <span class="field_caption">Transfer Posting ID:</span><asp:TextBox ID="txtTransPostID" runat="server"></asp:TextBox>
                    </td>
                    <td>
                       <span class="field_caption"> JV No: </span><asp:TextBox ID="txtJvNo" runat="server"></asp:TextBox>
                    </td>
                    <td>
                       <span class="field_caption"> Request Id: </span><asp:TextBox ID="txtReqId" runat="server"></asp:TextBox>
                    </td>
                    <td>
                       <span class="field_caption"> Excursion Id : </span><asp:TextBox ID="txtExcId" runat="server"></asp:TextBox>
                    </td>
                </tr>    
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="grdTransferPost" TabIndex="10" runat="server" Font-Size="10px"
                            CssClass="td_cell" Width="960px" BackColor="White" AutoGenerateColumns="False"
                            BorderColor="#999999" BorderStyle="None" CellPadding="3" GridLines="Vertical">
                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                            <Columns>                                    
                                <asp:BoundField HeaderText="Transfer Posting ID"  DataField="TransPostingId"/>
                                <asp:BoundField HeaderText="JV No"  DataField="jvno"/>
                                <asp:BoundField HeaderText="Date Created" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}" DataField="convadddate"/>
                                <asp:BoundField HeaderText="Add User"  DataField="adduser"/>
                                <asp:BoundField HeaderText="Date Modified"  DataField="convmoddate" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "/>
                                <asp:BoundField HeaderText="Modified User"  DataField="moduser"/>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditRow" Text="Edit" CommandArgument='<%# Bind("TransPostingId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                    <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkView" runat="server" CommandName="View"
                                            Text="View" CommandArgument='<%# Bind("TransPostingId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRow" Text="Delete"
                                            CommandArgument='<%# Bind("TransPostingId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Transfer Posting ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransPostId" runat="server" OnDataBinding="TemplateFieldBind"
                                            Text=""></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="JV No">
                                    <ItemTemplate>
                                        <asp:Label ID="lbljvno" runat="server" OnDataBinding="TemplateFieldBind" Text=""></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Add Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddDate" runat="server" OnDataBinding="TemplateFieldBind" Text=""></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Add User">
                                    <ItemTemplate>
                                        <asp:Label ID="lbladduser" runat="server" OnDataBinding="TemplateFieldBind"
                                            Text=""></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="middle" />
                                </asp:TemplateField> --%>                              
                            </Columns>
                            <FooterStyle CssClass="grdfooter" />
                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnSearch" runat="server" Value="" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
