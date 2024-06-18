<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="CustomersRegistrationSearch.aspx.vb" Inherits="CustomersRegistrationSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
   
    
    
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
       
   function CallWebMethod(methodType)
    {
        switch(methodType)
        {
            case "ctrycode":
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                var select = document.getElementById("<%=ddlCountrycode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
               
                constr=connstr.value

                ColServices.clsServices.GetwebCityNameListnew(constr, codeid, FillCityCodes, ErrorHandler, TimeOutHandler);
                break;
               
          
       
}
    }

    function FillCityCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
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

    <table width="910" style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td class="field_heading" style="text-align: center; height: 17px;">
                Registration List</td>
        </tr>
        <tr>
            <td class="td_cell" style="color: blue; text-align: center">
                Type few characters of code or name and click search</td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE width=800><TBODY><TR><TD style="TEXT-ALIGN: center" class="td_cell" colSpan=6>
<asp:RadioButton id="rbsearch" tabIndex=16 runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbsearch_CheckedChanged" Checked="True"></asp:RadioButton>&nbsp;
<asp:RadioButton id="rbnadsearch" tabIndex=17 runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbnadsearch_CheckedChanged" wfdid="w6"></asp:RadioButton>&nbsp;
<asp:Button id="btnSearch" tabIndex=10 runat="server" Text="Search" 
        Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;<asp:Button 
        id="btnClear" tabIndex=11 runat="server" Text="Clear" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;
<asp:Button id="btnHelp" tabIndex=15 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button>&nbsp;<asp:Button 
        id="btnAddNew" style="display:none" tabIndex=12 runat="server" Text="Add New" Font-Bold="False" 
        CssClass="field_button"></asp:Button>&nbsp;
<asp:Button id="btnPrint" tabIndex=14 runat="server" Text="Report" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="HEIGHT: 24px" class="td_cell">
    Client &nbsp;Name&nbsp;</TD><TD style="WIDTH: 230px; HEIGHT: 24px">
    <asp:TextBox ID="txtcustomername" runat="server" CssClass="field_input" MaxLength="100"
        TabIndex="2" Width="219px"></asp:TextBox></TD><TD style="HEIGHT: 24px" class="td_cell">
            &nbsp;</TD><TD style="HEIGHT: 24px">&nbsp;</TD><TD style="HEIGHT: 24px"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"></asp:Label></TD><TD style="HEIGHT: 24px"><asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" CssClass="field_input" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR>
    <tr>
        <td class="td_cell" style="height: 24px">
            Register Id</td>
        <td style="width: 230px; height: 24px">
            <asp:TextBox ID="txtregno" runat="server" CssClass="field_input" MaxLength="100"
                TabIndex="2" Width="219px"></asp:TextBox></td>
        <td class="td_cell" style="height: 24px">
            Status</td>
        <td style="height: 24px">
            <select id="ddlStatus" runat="server" class="drpdown" style="width: 219px" 
                tabindex="4">
                <option selected="selected" value="[Select]">[Select]</option>
                <option value="A">Approved</option>
                <option value="U">Unapproved</option>
            </select>
        </td>
        <td style="height: 24px">
        </td>
        <td style="height: 24px">
        </td>
    </tr>
    <TR><TD style="WIDTH: 80px"><asp:Label id="lblCtryCode" runat="server" 
            Text="Country" ForeColor="Black" Width="76px" CssClass="field_caption" 
            Visible="False"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 219px" id="ddlCountryCode" class="field_input" tabIndex=7 onchange="CallWebMethod('ctrycode');" runat="server" visible="false"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 79px">
        <asp:Label ID="lblcity" runat="server" CssClass="td_cell" Text="City " 
            Visible="False" Width="144px"></asp:Label>
        </TD><TD style="WIDTH: 100px">&nbsp;<select id="ddlCitycode" runat="server" 
                class="field_input" name="D1"  
                style="WIDTH: 219px" tabindex="9" visible="false">
            <option selected=""></option>
            </select>
&nbsp;</TD></TR><TR><TD>
        <asp:Label ID="Label2" runat="server" CssClass="field_caption" Text="From Date"
            Width="120px"></asp:Label></TD><TD style="WIDTH: 230px">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="field_input" 
                    TabIndex="2" ValidationGroup="MKE"
                    Width="80px"></asp:TextBox>
                <asp:ImageButton ID="ImgBtnFrmDt" runat="server"
                        ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />
                <cc1:MaskedEditValidator
                            ID="MskVFromDt" runat="server" ControlExtender="MskFromDate" ControlToValidate="txtFromDate"
                            CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required"
                            ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                            TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator></TD><TD>
                <asp:Label ID="Label1" runat="server" CssClass="field_caption" Text="To Date&#13;&#10;"
                    Width="120px"></asp:Label></TD><TD>
                        <asp:TextBox ID="txtTodate" runat="server" CssClass="field_input" TabIndex="3" ValidationGroup="MKE"
                            Width="80px"></asp:TextBox>&nbsp;<asp:ImageButton ID="ImageButton1" runat="server"
                                ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="5" /><cc1:MaskedEditValidator
                                    ID="MaskedEditValidator1" runat="server" ControlExtender="MskChequeDate" ControlToValidate="txtTodate"
                                    CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required"
                                    ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                    TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator></TD></TR></TBODY></TABLE>
                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                            height: 9px" type="text" />
                <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                    ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                    TargetControlID="txtFromDate">
                </cc1:MaskedEditExtender>
                        <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                            ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                            TargetControlID="txtTodate">
                        </cc1:MaskedEditExtender>
                <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                    TargetControlID="txtFromDate">
                </cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="ClExChequeDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton1"
                            TargetControlID="txtTodate">
                        </cc1:CalendarExtender>
</contenttemplate>
                </asp:UpdatePanel>
                &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
</td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="field_button" TabIndex="13"
                    Text="Export To Excel" />
                </td>
        </tr>
        <tr>
            <td style="width: 100px">
    <asp:UpdatePanel id="UpdatePanel3" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=15 runat="server" Font-Size="10px" BackColor="White" Width="910px" CssClass="td_cell" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField SortExpression="regno" Visible="False" HeaderText="Regno"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("regno") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("regno") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField   HeaderText="Regno."><ItemTemplate>
<asp:Label ID="lblregno" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "regno") %>'></asp:Label>
</ItemTemplate>
    <HeaderStyle HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>


<asp:TemplateField Visible ="False"  HeaderText="status"><ItemTemplate>
<asp:Label ID="lblapprove" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "approve") %>'></asp:Label>
</ItemTemplate>
    <HeaderStyle HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:BoundField DataField="agentname" SortExpression="agentname" HeaderText="Client Name"></asp:BoundField>

<asp:TemplateField HeaderText="Status"><ItemTemplate>
<asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "approve") %>'></asp:Label>
</ItemTemplate>
    <HeaderStyle HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>


<asp:BoundField DataField="contact1" SortExpression="contact1" HeaderText="First Name"></asp:BoundField>
<asp:BoundField DataField="designation" SortExpression="designation" HeaderText="Designation">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="ctrycode" SortExpression="ctrycode" HeaderText="Country"></asp:BoundField>
<asp:BoundField DataField="citycode" SortExpression="citycode" HeaderText="City"></asp:BoundField>
<asp:BoundField DataField="add1" SortExpression="add1" HeaderText="Address1"></asp:BoundField>
<asp:BoundField DataField="tel2" SortExpression="tel1" HeaderText="Telephone"></asp:BoundField>
<asp:BoundField DataField="fax" SortExpression="fax" HeaderText="Fax"></asp:BoundField>
<asp:BoundField DataField="email" SortExpression="email" HeaderText="Email"></asp:BoundField>

<asp:TemplateField HeaderText="Register Date"><ItemTemplate>
<asp:Label ID="lblregister" runat="server" Text='<%# FormatDate(DataBinder.Eval (Container.DataItem, "registerdate")) %>'></asp:Label>
</ItemTemplate>
    <HeaderStyle HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:ButtonField HeaderText="Action" Text="Approve" CommandName="Addrow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<%--
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow" Visible="False">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>--%>
<asp:ButtonField HeaderText="Action" Text="View/Show Registration Form" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

<asp:ButtonField HeaderText="Action" Text="Print" CommandName="Print">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>

</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" Visible="False"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel>
                &nbsp; &nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
    &nbsp; &nbsp;
</asp:Content>

