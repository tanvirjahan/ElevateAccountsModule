<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Countrygroup.ascx.vb" Inherits="PriceListModule_Countrygroup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="../../css/Styles.css" rel="stylesheet" type="text/css" />
<%--<script src="../../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>--%>
<link href="../css/StyleCountryUserControl.css" rel="stylesheet" type="text/css" />


<style type="text/css">
    .style1
    {
        width: 422px;
        text-align: right;
    }
    .styleDatalist
    {
        width: 100%;
    }
    
        .FixedHeader {  
            position: absolute;
        }  

        

        
</style>
    <%--<script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>--%>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
//        $("[id*=chkAll]").live("click", function () {
//            var chkHeader = $(this);
//            var grid = $(this).closest("table");
//            $("input[type=checkbox]", grid).each(function () {
//                if (chkHeader.is(":checked")) {
//                    $(this).attr("checked", "checked");
//                    $("td", $(this).closest("tr")).addClass("selected");
//                } else {
//                    $(this).removeAttr("checked");
//                    $("td", $(this).closest("tr")).removeClass("selected");
//                }
//            });
//        });
//        $("[id*=chk2]").live("click", function () {
//            var grid = $(this).closest("table");
//            var chkHeader = $("[id*=chkAll]", grid);
//            if (!$(this).is(":checked")) {
//                $("td", $(this).closest("tr")).removeClass("selected");
//                chkHeader.removeAttr("checked");
//            } else {
//                $("td", $(this).closest("tr")).addClass("selected");
//                if ($("[id*=chk2]", grid).length == $("[id*=chk2]:checked", grid).length) {
//                    chkHeader.attr("checked", "checked");
//                }
//            }
//        });

        function fnConfirm(chk) {

            var chkbox = document.getElementById(chk);
            var hdStatus = document.getElementById('<%=hdStatus.ClientID%>');
            var r = confirm("Are you sure to remove agents selected for this country group? Or for this country?  ");
            if (r == true) {
                hdStatus.value = 'Ok';
                chkbox.checked = false;
            } 
            else {
                hdStatus.value = 'Cancel';
                chkbox.checked = true
            }

            var btnHidProcessCheckBox = document.getElementById('<%=btnHidProcessCheckBox.ClientID%>');
            btnHidProcessCheckBox.click();

        }
        function fnConfirmCountry(chk) {

            var chkbox = document.getElementById(chk);
            var hdStatus = document.getElementById('<%=hdCountryStatus.ClientID%>');
            var r = confirm("Are you sure to remove agents selection for this country? ");
            if (r == true) {
                hdStatus.value = 'Ok';
                chkbox.checked = false;
            }
            else {
                hdStatus.value = 'Cancel';
                chkbox.checked = true
            }

            var btnHidProcessCheckBox = document.getElementById('<%=btnHidProcessCheckBox_Country.ClientID%>');
            btnHidProcessCheckBox.click();

        }

        function fnConfirmCountryAll(chk) {
            var chkbox = document.getElementById(chk);
            var hdStatus = document.getElementById('<%=hdCountryStatus.ClientID%>');
            var r = confirm("Are you sure to remove all country & its agents selection? ");
            if (r == true) {
                hdStatus.value = 'Ok';
                chkbox.checked = false;
            }
            else {
                hdStatus.value = 'Cancel';
                chkbox.checked = true
            }

            var btnHidProcessCheckBox_Country_All = document.getElementById('<%=btnHidProcessCheckBox_Country_All.ClientID%>');
            btnHidProcessCheckBox_Country_All.click();

        }

        function fnConfirmCountryList() {
            var hdStatus = document.getElementById('<%=hdCountryStatus.ClientID%>');
            var r = confirm("Are you sure to remove country & its agents seletion for this search category? ");
            if (r == true) {
                hdStatus.value = 'Ok';
            }
            else {
                hdStatus.value = 'Cancel';
            }

            var btnHidProcessCheckBox_Country_List = document.getElementById('<%=btnHidProcessCheckBox_Country_List.ClientID%>');
            btnHidProcessCheckBox_Country_List.click();

        }

//        21/12/2016
        function fnConfirmCountryListDataList() {
            var hdStatus = document.getElementById('<%=hdCountryStatus.ClientID%>');
            var r = confirm("Are you sure to remove country & its agents seletion?");
            if (r == true) {
                hdStatus.value = 'Ok';
            }
            else {
                hdStatus.value = 'Cancel';
            }

            var btnHidProcess_Country_DataList = document.getElementById('<%=btnHidProcess_Country_DataList.ClientID%>');
            btnHidProcess_Country_DataList.click();

        }

        function fnBtnDummyProcess() {
            var BtnDummyProcess = document.getElementById('<%=BtnDummyProcess.ClientID%>');
            BtnDummyProcess.click();
        }

        function txtSearchAgent_itemselect(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtSearchAgentCtryCode.ClientID%>').value = eventArgs.get_value();
                var btnSearchAgentNext1 = document.getElementById('<%=btnSearchAgentNext.ClientID%>');
                btnSearchAgentNext1.click();
            }
            else {
                document.getElementById('<%=txtSearchAgentCtryCode.ClientID%>').value = '';
            }

            
        }

        function AutoCompleteExtenderUserControlKeyUp() {
            $("#<%= txtSearchAgent.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtSearchAgentCtryCode.ClientID%>').value = '';
            });

//            $(".ctrynotselectedclss").click(function () {
//                $(".ctrynotselectedclss_row").toggle();
            //            });

            $(".ctryshowall").click(function () {
                //$(".ctrynotselectedclss_row").slideDown();
                $(".ctrynotselectedclss_row").show();
            });
            $(".ctryhideall").click(function () {
                $(".ctrynotselectedclss_row").hide();
            });

//            $(".agentshowall").click(function () {
//                $(".agentnotselectedclss_row").show();
//            });
//            $(".agenthideall").click(function () {
//                $(".agentnotselectedclss_row").hide();
//            });

//            $(".agentpopcheckboxheader").click(function () {
//                var chkHeader = $(this);
//                alert(chkHeader.is(":checked"));
//                $(document).find(".agentpopcheckbox").each(function () {

//                    if (chkHeader.is(":checked")) {
//                        alert('test1');
//                        $(this).attr("checked", "checked");
//                    } else {
//                        alert('test2');
//                        $(this).removeAttr("checked");
//                    }
//                })
//            });

//            $(".ctryhideallgrid").click(function (t) {
//                var imgsrc = $(this).attr("src");
//                if (imgsrc.toString().indexOf("plus.gif") >= 0) {
//                    $(".ctrynotselectedclss_row").show();
//                    $(this).attr("src", "../Images/minus.gif");
//                }
//                else {
//                    $(".ctrynotselectedclss_row").hide();
//                    $(this).attr("src", "../Images/plus.gif");
//                }
//            });



//            $(".agenthideallgrid").click(function (t) {
//                var imgsrc = $(this).attr("src");
//                if (imgsrc.toString().indexOf("plus.gif") >= 0) {
//                    $(".agentnotselectedclss_row").show();
//                    $(this).attr("src", "../Images/minus.gif");
//                }
//                else {
//                    $(".agentnotselectedclss_row").hide();
//                    $(this).attr("src", "../Images/plus.gif");
//                }
//            });
        }

        $("[id*=chkAllNew]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });

    </script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
          <div id="countrygroup" style="width:90%">
                    <table width="100%" >       
                        <tr>
                            <td width="500px" colspan="2"> 
                                <div style="width:90%">
                                    <asp:Label ID="lblAgentSelInfo" runat="server" Text="Select agents only if it is agent specific contracts. If no agents are selected, the contracts applier to all agents of this country" style="font-weight:bold;color:Red"></asp:Label>
                                </div>
                                <div style="float:right;margin-right:30px;vertical-align:top">
                                    <img id="ctryshowall" class="ctryshowall" src="../Images/plus.gif" onclick="return false" alt="" title="Show all the unselected Countries" />
                                    <img id="ctryhideall" class="ctryhideall" src="../Images/minus.gif" onclick="return false" alt="" title="Hide all the unselected Countries" />
                                </div>
                                <%--&nbsp;--%>
                            </td>
                            <td width="300px">
                                <asp:Label ID="lblSearchAgent" runat="server" Text="Search Agent"></asp:Label>
                                <%--<div style="float:right">
                                    <img id="agentshowall" class="agentshowall" src="../Images/plus.gif" onclick="return false" alt="" title="Show all the unselected Agent"/>
                                    <img id="agenthideall" class="agenthideall" src="../Images/minus.gif" onclick="return false" alt="" title="Hide all the unselected Agent"/>
                                </div>--%>
                                <br />
                                <asp:TextBox ID="txtSearchAgent" runat="server" Width="100%" ></asp:TextBox>
                                <asp:TextBox ID="txtSearchAgentCtryCode" runat="server" style="display:none"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txtSearchAgent_AutoCompleteExtender" runat="server" 
                                    CompletionInterval="10" 
                                    CompletionListCssClass="autocomplete_completionListElement" 
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                    DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                    FirstRowSelected="True" MinimumPrefixLength="1" ServiceMethod="GetAgentListSearch" 
                                    TargetControlID="txtSearchAgent" OnClientItemSelected="txtSearchAgent_itemselect">
                                </asp:AutoCompleteExtender>
                                <br />
                                <asp:Button ID="btnSearchAgentNext" runat="server" Text="Next" style="display:none" />
                            </td>
                        </tr>   
                                
                        <tr>
                            <td width="500px" colspan="2">
                                <div ID="Divcountry">
                                    <%--<asp:TextBox ID="txtcountry" runat="server" ></asp:TextBox>--%>
                                    <div ID="ShowCountries" runat="server" style="overflow: scroll; height: 300px;
                                width: 95%; border: 3px solid #2D7C8A; background-color: White;">
                                        <asp:GridView ID="gv_ShowCountries" runat="server" AutoGenerateColumns="False" 
                                            BackColor="White" BorderColor="#099999" CssClass="td_cell" Width="100%" 
                                            TabIndex="8">
                                            <Columns>
                                                <asp:TemplateField HeaderText="ctrycode" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtctrycode" runat="server" Text='<%# Bind("ctrycode") %>'></asp:Label>
                                                        <asp:Label ID="lblctryname" runat="server" Text='<%# Bind("ctryname") %>' style="display:none"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:TemplateField>
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="2%" VerticalAlign="Top" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgExpand" runat="server" ImageUrl="~/Images/minus.gif" class="clsimgexpand" OnClientClick="return false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                --%>

                                                <asp:TemplateField>
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkCtryAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkCtryAll_CheckedChanged"/>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="2%"  />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkCtry2" runat="server" Width="10px" AutoPostBack="True" OnCheckedChanged="chkCtry2_CheckedChanged" />
                                                        <asp:ImageButton ID="imgExpand" runat="server" ImageUrl="~/Images/minus.gif" OnClientClick="return false" style="display:none" />
                                                        <asp:Label ID="lblordertype" runat="server" Text='<%# Bind("ordertype") %>' style="display:none"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="ctrycode" HeaderText="Country Code" 
                                                    SortExpression="ctrycode" >
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="80px" />
                                                </asp:BoundField>

                                                <asp:TemplateField SortExpression="ctryname">
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblctrynamehead" runat="server" Text="Country Name"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblctrynameDisplay" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField SortExpression="plgrpname">
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblplgrpnamehead" runat="server" Text="Region"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"  Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblplgrpnameDisplay" runat="server" Text='<%# Bind("plgrpname") %>'></asp:Label>
                                                    </ItemTemplate> 
                                                </asp:TemplateField>

                                                <asp:TemplateField SortExpression="countrygroupname">
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblcountrygroupnamehead" runat="server" Text="Country Group"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"  Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcountrygroupnameDisplay" runat="server" Text='<%# Bind("countrygroupname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="countrygroupcode" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCountryGroupCode" runat="server" Text='<%# Eval("CountryGroupCode") %>' ></asp:Label>
                                                        <asp:Label ID="lblCountryGroupName" runat="server" Text='<%# Eval("countrygroupname") %>' style="display:none" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                


                                                <asp:TemplateField HeaderText="plgrpcode" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblplgrpcode" runat="server" Text='<%# Eval("plgrpcode") %>' ></asp:Label>
                                                        <asp:Label ID="lblplgrpname" runat="server" Text='<%# Eval("plgrpname") %>' style="display:none" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lnkShowAgent" runat="server" CommandName="cmdShowAgent" CommandArgument='<%# Bind("ctrycode") %>' ImageUrl="~/Images/playverde.jpg" style="width:15px;height:15px" ></asp:ImageButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#454580" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="Transparent" Font-Size="12px" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </td>
                            <td width="400px">
                                <div ID="Divagents">
                                    <%--<asp:TextBox ID="txtagent" runat="server" ></asp:TextBox>--%>
                                    <div ID="showagents" runat="server" style="overflow: scroll; height: 300px;
                                width: 100%; border: 3px solid #2D7C8A; background-color: White;">
                                        <asp:GridView ID="gv_showagents" runat="server" AutoGenerateColumns="False" 
                                            BackColor="White" BorderColor="#099999" CssClass="td_cell" Width="100%" 
                                            TabIndex="10">
                                            <Columns>
                                                <asp:BoundField DataField="ctrycodename" HeaderText="Country" 
                                                    SortExpression="ctrycodename">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Agentcode" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtagentcode" runat="server" Text='<%# Bind("agentcode") %>'></asp:Label>
                                                        <asp:Label ID="lblagentname" runat="server" Text='<%# Bind("agentname") %>' style="display:none"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="2%" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chk2All_agent_checkedchanged" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk2" runat="server" Width="10px" AutoPostBack="true" OnCheckedChanged="chk2_agent_checkedchanged"  />
                                                        <asp:ImageButton ID="imgExpand" runat="server" ImageUrl="~/Images/minus.gif" OnClientClick="return false" style="display:none" />
                                                        <asp:Label ID="lblordertype" runat="server" Text='<%# Bind("ordertype") %>' style="display:none"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="agentcode" HeaderText="Agent Code" 
                                                    SortExpression="agentcode" Visible="false">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="80px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="agentname" HeaderText="Agent Name" 
                                                    SortExpression="agentname">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="countrycode" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCountryCode" Text='<%# Eval("ctrycode") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:BoundField DataField="customergroupname" HeaderText="Customer Group Name" 
                                                    SortExpression="customergroupname">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>--%>

                                                <asp:TemplateField HeaderText="customergroupname" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerGroupNameDisplay" Text='<%# Eval("customergroupname") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="customergroupcode" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerGroupCode" Text='<%# Eval("customergroupcode") %>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblCustomerGroupName" Text='<%# Eval("customergroupname") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#454580" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="Transparent" Font-Size="12px" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:HiddenField ID="hdStatus" runat="server" />
                <asp:HiddenField ID="hdCountryStatus" runat="server" />
                <asp:Button ID="btnHidProcessCheckBox" runat="server" style="display:none" />
                  <asp:Button ID="btnHidProcessCheckBox_Country" runat="server" style="display:none" />
                  <asp:Button ID="btnHidProcessCheckBox_Country_List" runat="server" style="display:none" />
                  <asp:Button ID="btnHidProcess_Country_DataList" runat="server" style="display:none" />
                  <asp:Button ID="btnHidProcessCheckBox_Country_All" runat="server" style="display:none" />
                  <asp:Button ID="BtnDummyProcess" runat="server" style="display:none" />

                
                   <asp:HiddenField ID="hdnPageMode" runat="server" /> 
                   <asp:HiddenField ID="hdnPageName" runat="server" /> 
                   <asp:HiddenField ID="hdnTranId" runat="server" /> 


                   </ContentTemplate>
                  </asp:UpdatePanel>

                  <%-- Agent Selection Modal Poup --%>
                    <asp:Panel runat="server" ID="PanelShowAgent" Style="display: none; overflow: scroll;
                        height: 500px; width: 700px; z-index: -100;" BorderStyle="Double" BorderWidth="6px"
                        BackColor="white">
                        <div style="margin: 5%">
                            <%--<asp:LinkButton ID="lnkCloseShowAgent" runat="server" class="closeiconnew_mhd" Text="X" style="font-size:0px" ></asp:LinkButton>--%>
                            <asp:UpdatePanel ID="UpdatePanelShowAgent" runat="Server">
                                <ContentTemplate>
                                    <span class="aveRooms">Select Agent</span>
                                    <br />
                                    <div>
                                        <asp:GridView ID="gvShowAgentPopup" runat="server" Font-Size="13px" Width="600px" CellPadding="3"
                                            BorderWidth="1px" AutoGenerateColumns="False"
                                            AllowPaging="false" AlternatingRowStyle-BackColor="lightGray">
                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                   <HeaderTemplate>
                                                        <center>
                                                            <div align="center">
                                                                <asp:CheckBox ID="chkAllNew" runat="server" AutoPostBack="false" class="agentpopcheckboxheader"  />
                                                            </div>
                                                        </center>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <center> 
                                                            <div align="center">
                                                             <asp:CheckBox ID="chkselagent" runat="server" AutoPostBack="false" class="agentpopcheckbox" />
                                                            </div>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="20px" />
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agent Code">
                                                    <ItemTemplate>
                                                        <center>
                                                            <div align="center">
                                                             <asp:Label ID="lblagentcode" runat="server" Text='<%# Bind("agentcode") %>'></asp:Label>
                                                             <asp:Label ID="lblctrycode" runat="server" Text='<%# Bind("ctrycode") %>' style="display:none"> </asp:Label>
                                                            </div>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agent Name">
                                                    <ItemTemplate>
                                                        <center>
                                                            <div align="center">
                                                                <asp:Label ID="lblagentname" runat="server" Text='<%# Bind("agentname") %>'></asp:Label>
                                                            </div>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="300px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="align: center">
                                        <center>
                                            <asp:Button ID="btnPopupShowAgentSave" runat="server" Text="Save Selected" />
                                            <asp:Button ID="btnPopupShowAgentClose" runat="server" Text="Close" />
                                        </center>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>

                    <asp:modalpopupextender runat="server" id="ModalPopupShowAgent"
                        targetcontrolid="dummy5" cancelcontrolid="btnPopupShowAgentClose" popupcontrolid="PanelShowAgent"
                        backgroundcssclass="ModalPopupBG" dropshadow="true" />
                
                    <input id="dummy5" type="button" style="display: none" value="Cancel" runat="server" />

                    <%-- Agent Selection Modal Popup End --%>

                    <%-- Couuntry Selection Modal Poup --%>
                    <asp:Panel runat="server" ID="PanelShowCountry" Style="display: none; overflow: scroll;
                        height: 500px; width: 700px; z-index: -100;" BorderStyle="Double" BorderWidth="6px"
                        BackColor="white">
                        <div style="margin: 5%">
                            <%--<asp:LinkButton ID="lnkCloseShowCountry" runat="server" class="closeiconnew_mhd1" Text="X" style="font-size:0px" ></asp:LinkButton>--%>
                            <asp:UpdatePanel ID="UpdatePanelShowCountry" runat="Server">
                                <ContentTemplate>
                                    <asp:label ID="lblCountryPopupHead" class="aveRooms" Text="Select Country" runat="server"> </asp:label>
                                    <asp:label ID="lblCountryPopupType" style="display:none" Text="C" runat="server"> </asp:label>
                                    <br />
                                    <div>
                                        <asp:GridView ID="gvShowCountryPopup" runat="server" Font-Size="13px" Width="600px" CellPadding="3"
                                            BorderWidth="1px" AutoGenerateColumns="False"
                                            AllowPaging="false" AlternatingRowStyle-BackColor="lightGray">
                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                   <HeaderTemplate>
                                                        <center>
                                                            <div align="center">
                                                                <asp:CheckBox ID="chkAllNew" runat="server" AutoPostBack="false" class="countrypopcheckboxheader"  />
                                                            </div>
                                                        </center>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <center> 
                                                            <div align="center">
                                                             <asp:CheckBox ID="chkselcountry" runat="server" AutoPostBack="false" class="countrypopcheckbox" />
                                                            </div>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="20px" />
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Country Code">
                                                    <ItemTemplate>
                                                        <center>
                                                            <div align="center">
                                                             <asp:Label ID="lblctrycode" runat="server" Text='<%# Bind("ctrycode") %>'></asp:Label>
                                                            </div>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Country Name">
                                                    <ItemTemplate>
                                                        <center>
                                                            <div align="center">
                                                                <asp:Label ID="lblcountryname" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                                                            </div>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="300px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="align: center">
                                        <center>
                                            <asp:Button ID="btnPopupShowCountrySave" runat="server" Text="Save Selected" />
                                            <asp:Button ID="btnPopupShowCountryClose" runat="server" Text="Close" />
                                        </center>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>

                    <asp:modalpopupextender runat="server" id="ModalPopupShowCountry"
                        targetcontrolid="dummy6" cancelcontrolid="btnPopupShowCountryClose" popupcontrolid="PanelShowCountry"
                        backgroundcssclass="ModalPopupBG" dropshadow="true" />
                
                    <input id="dummy6" type="button" style="display: none" value="Cancel" runat="server" />

                    <%-- Country Selection Modal Popup End --%>