<%@ Page Title="" Language="VB" MasterPageFile="~/WebAdminMaster.master" AutoEventWireup="false"
    CodeFile="RateUpload.aspx.vb" Inherits="WebAdminModule_Rate_Upload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
   <asp:PostBackTrigger ControlID="btnupload" />
        </Triggers>
        <ContentTemplate>
            <table class="td_cell" style="width: 60px; height: 35px">
                <tr>
                    <td style="width: 100px; text-align: center">
                        <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Rate Upload"
                            Width="946px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Panel ID="Panel1" runat="server" Height="350px" Width="950px" ScrollBars="Auto">
                            <asp:TreeView ID="tvrupl" TabIndex="1" runat="server" Font-Size="9pt" __designer:wfdid="w10"
                                ExpandDepth="3" ShowLines="true" ShowExpandCollapse="true">
                            </asp:TreeView>
                        </asp:Panel>
                        &nbsp;
                        <asp:LinkButton ID="lnkfalse" runat="server"></asp:LinkButton>
                        <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopup" Style="display: none">
                            <div style="text-align: right">
                                <asp:LinkButton ID="lnkcancel" runat="server" Text="X" ForeColor="Red"></asp:LinkButton>
                            </div>

                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlsubfolder" runat="server">
                                            create sub folder<br />
                                            <br />
                                            Enter Subfolder Name<br />
                                            <asp:TextBox ID="txtfoldername" runat="server"></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Button ID="btnsave" runat="server" Text="save" />
                                        </asp:Panel>
                                        <asp:Panel ID="pnlfileupload" runat="server">
                                            upload files
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                            <br />
                                            <br />
                                            <asp:Button ID="btnupload" runat="server" Text="Upload"  OnClick="btnupload_click" />
                                            <br />
                                            <br />
                                        </asp:Panel>
                                        <asp:Button ID="lnkdelete" ForeColor="Red" runat="server" Text="Delete this File/Folder"></asp:Button>
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                    </td>
                                    <td valign="top" style="border-left: 1px solid gray; width: 200px">
                                        Rename this file/folder<br />
                                        <asp:TextBox ID="txtrename" runat="server"></asp:TextBox><br />
                                        <asp:Button ID="btnrename" runat="server" Text="Rename" />
                                        <%--<asp:LinkButton ID="lnkmod" ForeColor="Red" runat="server" Text="Modify this File/Folder"></asp:LinkButton>--%>
                                        
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td colspan="2" align="right" > <asp:Button ID="btnclose" runat="server" Text="Close"/> </td>
                                </tr>--%>
                            </table>
                            <asp:Label ID="lblmsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </asp:Panel>
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                            BackgroundCssClass="modalBackground" TargetControlID="lnkfalse" PopupControlID="pnlpopup">
                        </asp:ModalPopupExtender>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnfilename" runat="server" />
                  
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnSelectedNode" runat="server" />

    <script language="javascript" type="text/javascript">
        function savefilenameintotemp() {
            var hdnfilename = document.getElementById('<%=hdnfilename.ClientID%>');
            var fileupload1 = document.getElementById('<%=FileUpload1.ClientID%>');
            hdnfilename.value = fileupload1.files[0].name //fileupload1.value;

          //  hdnfilename.value = fileupload1.value;
            
        }
    </script>

    <script language="javascript" type="text/javascript">
        function filenamevalidate(fileuploadid) {
            var fileupload11 = document.getElementById(fileuploadid);
           
            if (fileupload11.value == "") {
                alert('File Name is invalid');
                return false;
            }

        }
    </script>

    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border: 1px solid #0DA9D0;
            padding: 10px;
        }
    </style>
</asp:Content>
