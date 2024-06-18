<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"  EnableEventValidation="false"  CodeFile="CountryGroupSearch.aspx.vb" Inherits="CountryGroupSearch"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<style>
        .btnExample {
  color: #2D7C8A;
  background: #e7e7e7;
  font-weight: bold;
  border: 1px solid #2D7C8A;
   padding: 5px 5px;
}
 
.btnExample:hover {
  color: #FFF;
  background: #2D7C8A;
}
.autocomplete_completionListElement
        {
	        visibility : hidden;
	        margin : 1px 0px 0px 0px!important;
	        background-color : #FFFFFF;
	        color : windowtext;
	        border : buttonshadow;
	        border-width : 1px;
	        border-style : solid;
	        cursor : 'default';
	        overflow : auto;
	        height : 200px;
            width:100px;
            text-align:left;
            list-style-type: none;
	           font-family:Verdana;
            font-size:small;
    
        }


        /* AutoComplete highlighted item */


        .autocomplete_highlightedListItem
        {
	        background-color:Silver;
	        color: black;
	          margin-left: -35px;
	          font-weight:bold;
        }


        /* AutoComplete item */

        .autocomplete_listItem
        {
	        background-color : window;
            color : windowtext;
	       margin-left: -35px;
        }

        </style>

<script language="javascript" type ="text/javascript" >
function CallWebMethod(methodType)
    {
        
        
         function checkNumber(e)
			{	    
			   
			   if (event.keycode=13 )
			   {
			   return true;
			   } 	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
			function checkCharacter(e)
			{	   
			if (event.keycode=13 )
			   {
			   return true;
			   } 	
			 
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}
				
</script> 
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td style="width: 100%; height: 11px">
                <table style="width: 100%">
                    <tr>
                        <td align="center" class="field_heading" style="width: 100%; height: 1px">
                            <asp:Label ID="Label7" runat="server" Text="Country Group Search"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="td_cell" style="color: blue">
                Type few characters of code or name and click search 
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 872px"><TBODY><TR>
<TD style="HEIGHT: 22px; TEXT-ALIGN: center" colSpan=6>&nbsp; &nbsp;
<asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" 
        Font-Bold="False" CssClass="btnExample" __designer:wfdid="w42" 
        EnableTheming="True" Font-Italic="False"></asp:Button>
&nbsp;<asp:Button id="btnClear" tabIndex=6 runat="server" Text="Clear" 
        CssClass="btnExample" ></asp:Button>&nbsp;
<asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="btnExample" __designer:wfdid="w3"></asp:Button>&nbsp;
<asp:Button id="btnAddNew" tabIndex=7 runat="server" Text="Add\Edit" 
        Font-Bold="False" __designer:dtid="14636698788954128" CssClass="btn" 
        __designer:wfdid="w4"></asp:Button>&nbsp;
</TD>

    </TR><TR><TD style="WIDTH: 62px">
    &nbsp;</TD><TD style="WIDTH: 73px">
        &nbsp;</TD><TD style="WIDTH: 100px">&nbsp;</TD><TD style="WIDTH: 100px">
    &nbsp;</TD><TD style="WIDTH: 63px" >
            &nbsp;</TD><TD style="WIDTH: 100px">&nbsp;</TD></TR>
    <tr>
        <td style="WIDTH: 62px" align="right">
            <asp:Label ID="Label4" runat="server" __designer:wfdid="w44" 
                CssClass="field_caption" Text="Country" Width="72px"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtCountryName" runat="server" AutoPostBack="True" 
                Width="300px"></asp:TextBox>

            <asp:AutoCompleteExtender ID="txtCountryName_AutoCompleteExtender" 
      runat="server" CompletionInterval="10" 
                CompletionListCssClass="autocomplete_completionListElement" 
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                FirstRowSelected="True" MinimumPrefixLength="1" ServiceMethod="GetCountries" 
                TargetControlID="txtCountryName">
            </asp:AutoCompleteExtender>

      <%--    <asp:AutoCompleteExtender ID="txtcountryname_AutoCompleteExtender" 
                runat="server" CompletionInterval="10" 
                CompletionListCssClass="autocomplete_completionListElement" 
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                FirstRowSelected="True" MinimumPrefixLength="1" ServiceMethod="GetCountries" 
                TargetControlID="txtcountryname">--%>

        </td>
        <td style="WIDTH: 63px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
    </tr>
    <tr>
        <td style="WIDTH: 62px">
            &nbsp;</td>
        <td colspan="3">
            <asp:Label ID="Label6" runat="server" 
                Text="Search By: Country,Region and Country Group Tags." 
                Font-Names="Verdana" Font-Size="Small" ForeColor="#990000"></asp:Label>
        </td>
        <td style="WIDTH: 63px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
    </tr>
    <tr>
        <td style="WIDTH: 62px">
            &nbsp;</td>
        <td style="WIDTH: 73px">
            <asp:RadioButton ID="rbtnsearch" runat="server" __designer:wfdid="w40" 
                AutoPostBack="True" Checked="True" CssClass="td_cell" ForeColor="Black" 
                GroupName="GrSearch" OnCheckedChanged="rbtnsearch_CheckedChanged" Text="Search" 
                Visible="False" />
        </td>
        <td style="WIDTH: 100px">
            <asp:RadioButton ID="rbtnadsearch" runat="server" __designer:wfdid="w41" 
                AutoPostBack="True" CssClass="td_cell" ForeColor="Black" GroupName="GrSearch" 
                OnCheckedChanged="rbtnadsearch_CheckedChanged" Text="Advance Search" 
                Visible="False" Width="180px" />
        </td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 63px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
    </tr>
    <tr style="display:none;">
        <td style="WIDTH: 62px">
            &nbsp;</td>
        <td style="WIDTH: 73px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 63px">
            &nbsp;</td>
        <td style="WIDTH: 100px">
            <asp:Button ID="btnPrint" runat="server" tabIndex="9" Text="Report" 
                Visible="False" />
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td style="height: 15px;width:100%">
                            &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                                Text="Export To Excel" TabIndex="8" Visible="False" />
                            </td>
                    </tr>
                    <tr>
                        <td style="height: 15px;width:100%">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gv_SearchResult" runat="server" __designer:wfdid="w79" 
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                                        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                        CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                                        tabIndex="12" Width="100%">
                                        <FooterStyle CssClass="grdfooter" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="ctrycode" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="field_input" 
                                                        Text='<%# Bind("ctrycode") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcntryCode" runat="server" __designer:wfdid="w26" 
                                                        Text='<%# Bind("ctrycode") %>'></asp:Label>
                            <br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="chkSelectAll_CheckedChanged" Text="Select All" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server"></asp:Label>
                            <br />
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ctrycode" HeaderText="Country Code"></asp:BoundField>
                                            <asp:BoundField DataField="country" HeaderText="Country Name"></asp:BoundField>
                                            <asp:BoundField DataField="region" HeaderText="Region"></asp:BoundField>
                                            <asp:BoundField DataField="countrygroups" HeaderText="Country Group">
                                            </asp:BoundField>
                                        </Columns>
                                        <RowStyle CssClass="grdRowstyle" />
                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </td>
                    </tr>
                    <tr>
                        <td style="height: 33px;width:100%">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
                        <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" __designer:wfdid="w51" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
