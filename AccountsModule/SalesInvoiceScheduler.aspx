<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SalesInvoiceScheduler.aspx.vb" Inherits="SalesInvoiceScheduler" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>

    <style type="text/css">
        .advLink
        {
            font-family: Arial,Verdana, Geneva, ms sans serif;
            font-size: 10pt;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            border-width: 1px;
            border-color: #06788B;
            margin-left: 0px;
        }  
        
        .disablebtn
        {          
            border-radius: 2px;
	        border: 1px solid #06788B;
	        font-family: Verdana, Arial, Geneva, ms sans serif;
	        font-size: 10pt;
	        font-weight: bold;
	        font-style: normal;
	        font-variant: normal;
	        color:white;
	        background-color :Gray;
	        margin-top: 0px;
            height: 24px;
        }
            
    </style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tr>
                    <td class="td_cell" align="center" style="width: 100%;">
                        <asp:Label ID="lblHeading" runat="server" Text="Sales Invoice Scheduler" style="padding:2px"
                            CssClass="field_heading" Width="100%" >
                        </asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table style="width: 100%; padding:2px 4px 0px 4px">
                            <tr>
                             
                                <td colspan="6">
                                    <table>
                                    <tr>
                            <td><img id="ImgAdvanced" src="~/Images/rightArrow.png" alt="image" style="cursor: pointer" runat="server" onclick="AdvancedOption()"/>
                            </td>
                           
                            </tr>        
                                        <tr>
                                            <td>
                                            <div id="div3" style="min-height: 370px; max-height: 370px; max-width:95vw; overflow:scroll">
                                                <asp:GridView ID="gvScheduler" runat="server" AutoGenerateColumns="False" 
                                                    BackColor="White" BorderColor="#999999" CssClass="grdstyle" 
                                                    HeaderStyle-BorderColor="White" Height="158px" ShowHeaderWhenEmpty="true" 
                                                    TabIndex="30" Width="1250px">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Scheduled ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScheduleId" runat="server" Text='<%# Bind("scheduleId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="from Checkin">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFromCheckin" runat="server" 
                                                                    Text='<%# Bind("frmCheckindate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Checkin">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblToCheckIn" runat="server" 
                                                                    Text='<%# Bind("tocheckindate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Invoice state">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblinvoicestate" runat="server" Text='<%# Bind("invoicestate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Added By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbladduser" runat="server" Text='<%# Bind("adduser") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Added Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbladddate" runat="server" Text='<%# Bind("adddate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scheduled Time">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScheduledTime" runat="server" 
                                                                    Text='<%# Bind("processScheduledTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ProcessStatus">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProcessStatus" runat="server" 
                                                                    Text='<%# Bind("ProcessStatus") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ProcessStartTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ProcessStartTime" runat="server" 
                                                                    Text='<%# Bind("ProcessStartTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="processEndTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblprocessEndTime" runat="server" 
                                                                    Text='<%# Bind("processEndTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="ProcessCount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblprocessCount" runat="server" 
                                                                    Text='<%# Bind("processedCount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
														
														 <asp:TemplateField HeaderText="ErrorCount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblerrorCount" runat="server" 
                                                                    Text='<%# Bind("ErrorCount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="Processed Bookings">
                                                       <ItemTemplate>
                                                       <asp:LinkButton ID="lbtnViewProcessed" Text='View' CssClass="field_input" CommandName="Processed" CommandArgument='<%# Container.DisplayIndex %>' 
                                                       ForeColor="Blue" runat="server"></asp:LinkButton>
                                                       </ItemTemplate>
                                                       <HeaderStyle HorizontalAlign="Center" />
                                                       <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                                      </asp:TemplateField> 
                                                       <asp:TemplateField HeaderText="Error Bookings">
                                                       <ItemTemplate>
                                                       <asp:LinkButton ID="lbtnViewError" Text='View' CssClass="field_input" CommandName="Error" CommandArgument='<%# Container.DisplayIndex %>'
                                                       ForeColor="Blue" runat="server"></asp:LinkButton>
                                                       </ItemTemplate>
                                                       <HeaderStyle HorizontalAlign="Center" />
                                                       <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                                      </asp:TemplateField> 
                                        
                                                         <asp:TemplateField HeaderText="process flag" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblprocessflag" runat="server" Text='<%# Bind("processflag") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            </asp:TemplateField> 
                                         
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle" />
                                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                    <PagerStyle CssClass="grdpagerstyle" />
                                                    <HeaderStyle CssClass="grdheader" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                                </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                             
                            </tr>   

                           

                            <tr>
                                <td colspan="6" align="center" style="padding-top:8px;">
                                    <%--<asp:Button ID="btnDisplay" runat="server" CssClass="btn" TabIndex="15" Text="Display Pending for Invoicing" OnClientClick="return validateSearch();"/>&nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" TabIndex="16" Text="Clear" />
                                    <asp:Button ID="btnSchedule" runat="server" CssClass="btn" TabIndex="16" Text="Schedule" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="left" style="padding-top:8px; padding-bottom:8px">
                               <asp:TabContainer ID="TabInvoice" runat="server" ActiveTabIndex="0" TabIndex="17" >
                                <asp:TabPanel ID="panProcess" runat="server" HeaderText="Invoice Processed Details" Font-Bold="true" TabIndex="18">
                                <ContentTemplate>
                                <div id="divGrid" style="min-height: 370px; max-height: 370px; max-width:95vw; overflow:scroll">
                                <asp:GridView ID="gvProcessedInvoice" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    Width="100%" style="padding-top:3px; padding-bottom:3px;" AllowSorting="true" TabIndex="19">                                    
                                    <Columns>                                        
                                     
                                     <asp:TemplateField HeaderText="Schedule ID." SortExpression="scheduleId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblscheduleIdP" runat="server" Text='<%# Bind("scheduleId") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>  

                                        <asp:TemplateField HeaderText="Booking No." SortExpression="RequestId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("RequestId") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                                                               
                                         <asp:BoundField DataField="ProcessStatus" HeaderText="Process Status" SortExpression="ProcessStatus" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>  
                                        
                                        
                                        <asp:TemplateField HeaderText="Process Time" SortExpression="processtime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblprocesstime" runat="server" Text='<%# Bind("processtime") %>'></asp:Label>
                                                                                            
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>
                                                       
                                    <asp:TemplateField HeaderText="Invoice no" SortExpression="invno">
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvno" runat="server" Text='<%# Bind("invno") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                        
                                                                   
                                    </Columns>                                    
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                    <FooterStyle CssClass="grdfooter" />                                    
                                </asp:GridView>
                                <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>  
                                </div>                                
                                </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabError" runat="server" HeaderText="Error Details" TabIndex="20">
                                <ContentTemplate>
                                <div id="div1" style="min-height: 370px; max-height: 370px; max-width:95vw; overflow:scroll">
                                <asp:GridView ID="gvProcessError" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    Width="100%" AllowSorting="true" TabIndex="21">                                    
                                    <Columns>                                        
                                     
                                     <asp:TemplateField HeaderText="S.No" SortExpression="scheduleId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text='<%# Bind("scheduleId") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>  

                                        <asp:TemplateField HeaderText="Booking No." SortExpression="RequestId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestIdE" runat="server" Text='<%# Bind("RequestId") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                                                               
                                         <asp:BoundField DataField="ProcessStatus" HeaderText="Process Status" SortExpression="ProcessStatus" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>  
                                        
                                        
                                        <asp:TemplateField HeaderText="Process Time" SortExpression="processtime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblprocesstimeE" runat="server" Text='<%# Bind("processtime") %>'></asp:Label>
                                              
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>
                                                       
                                    <asp:TemplateField HeaderText="Error" SortExpression="errormessage">
                                            <ItemTemplate>
                                                <asp:Label ID="lblError" runat="server" Text='<%# Bind("errormessage") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                     </Columns>                                        
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                    <FooterStyle CssClass="grdfooter" />                                   
                                </asp:GridView>
                                <asp:Label ID="lblErrorMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>  
                                </div>                                
                                </ContentTemplate>
                                </asp:TabPanel>
                                </asp:TabContainer>                                    
                                </td>
                            </tr>
                           
                        </table>
                    </td>
                </tr>                
            </table>


            <div>
            <center>

                    <div id="Loading1" runat="server" style="height: 150px; width: 500px; vertical-align: middle">
                    <img alt="" id="Img1" runat="server" src="~/Images/loader-progressbar.gif" width="150" />
                    <h2 style="color: #06788B">
                        Processing please wait...</h2>
                </div>                 
                </center>
                <asp:ModalPopupExtender ID="ModalPopupLoading" runat="server" BehaviorID="ModalPopupLoading"
                    TargetControlID="btnInvisibleLoading" CancelControlID="btnCloseLoading" PopupControlID="Loading1"
                    BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>
                <input id="btnInvisibleLoading" runat="server" type="button" value="Cancel" style="display: none" />
                <input id="btnCloseLoading" type="button" value="Cancel" style="display: none" />
            </div>
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>

