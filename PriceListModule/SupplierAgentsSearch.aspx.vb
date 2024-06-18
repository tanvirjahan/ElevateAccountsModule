'------------================--------------=======================------------------================
'   Module Name    :    SupplierAgentsSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    19 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupplierAgentsSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Enum GridCol"
    Enum GridCol
        SAgentCodeTCol = 0
        SAgentCode = 1
        SAgentName = 2
        SProvTypeCode = 3
        SProvTypeName = 4
        CategoryCode = 5
        SellingCategoryCode = 6
        Currency = 7
        SectorCode = 8
        Active = 9
        AddressLine1 = 10
        Telephone1 = 11
        Fax = 12
        SalesTelephone = 13
        SalesContactPerson1 = 14
        AccountsTelephone1 = 15
        AccountsContactPerson1 = 16
        ControlACCode = 17
        CreditDays = 18
        CreditLimit = 19
        DMCCode = 20
        DateCreated = 21
        UserCreated = 22
        DateModified = 23
        UserModified = 24
        Edit = 25
        View = 26
        Delete = 27
    End Enum
#End Region


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   'changed by mohamed on 27/08/2018
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub



#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try


                Dim AppId As String = CType(Request.QueryString("appid"), String)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then

                    If AppId = "4" Then

                        strappname = AppName.Value
                    ElseIf AppId = "14" Then

                        strappname = AppName.Value
                    Else
                        strappname = AppName.Value
                    End If
                End If
                RowsPerPageAS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                txtconnection.Value = Session("dbconnectionName")

                SetFocus(txtAgentCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\SupplierAgentsSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If


                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlType, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlTName, "sptypename", "sptypecode", "select * from sptypemast where active=1 order by sptypename", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCode, "catcode", "catname", "select * from catmast where active=1 order by catcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCatName, "catname", "catcode", "select * from catmast where active=1 order by catname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingCode, "scatcode", "scatname", "select * from sellcatmast where active=1 order by scatcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingName, "scatname", "scatcode", "select * from sellcatmast where active=1 order by scatname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlContCode, "ctrycode", "ctryname", "select * from ctrymast where active=1  order by ctrycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlcontName, "ctryname", "ctrycode", "select * from ctrymast where active=1  order by ctryname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSectorCode, "sectorcode", "sectorname", "select * from sectormaster where active=1  order by sectorcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSectorName, "sectorname", "sectorcode", "select * from sectormaster where active=1  order by sectorname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where active=1 order by scatcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1 order by scatname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1  order by sectorcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1  order by sectorname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlControlCode, "controlacctcode", "controlacctcode", "select distinct controlacctcode from supplier_agents where active=1 and controlacctcode is not NULL order by controlacctcode", True)

                Session.Add("strsortExpression", "supagentcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                charcters(txtAgentCode)
                charcters(txtAgentName)
                '' Create a Dynamic datatable ---- Start
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                '--------end
                fillorderby()
                'FillGrid("supplier_agents.supagentname")
                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else
            Try
                If ddlType.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where  sptypecode='" & ddlTName.Value & "' and active=1 order by catcode", True, ddlCCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where sptypecode='" & ddlTName.Value & "' and  active=1 order by catname", True, ddlCatName.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where  sptypecode='" & ddlTName.Value & "' and active=1 order by scatcode", True, ddlSellingCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where sptypecode='" & ddlTName.Value & "' and active=1 order by scatname", True, ddlSellingName.Value)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True, ddlCCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True, ddlCatName.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where active=1 order by scatcode", True, ddlSellingCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1 order by scatname", True, ddlSellingName.Value)
                End If

                If ddlContCode.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where ctrycode='" & ddlcontName.Value & "'and active=1  order by sectorcode", True, ddlSectorCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where  ctrycode='" & ddlcontName.Value & "'and active=1  order by sectorname", True, ddlSectorName.Value)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1  order by sectorcode", True, ddlSectorCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1  order by sectorname", True, ddlSectorName.Value)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCatName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlTName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            'ddlSellingCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            'ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSectorCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSectorName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlControlCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SupagentsWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If

        Page.Title = "SupplierAgents Search"
    End Sub


#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtAgentCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(supplier_agents.supagentcode) LIKE '" & Trim(txtAgentCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(supplier_agents.supagentcode) LIKE '" & Trim(txtAgentCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtAgentName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(supplier_agents.supagentname) LIKE '" & Trim(txtAgentName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(supplier_agents.supagentname) LIKE '" & Trim(txtAgentName.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlType.Items(ddlType.SelectedIndex).Text <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " supplier_agents.sptypecode = '" & Trim(ddlType.Items(ddlType.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND supplier_agents.sptypecode = '" & Trim(ddlType.Items(ddlType.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlTName.Items(ddlTName.SelectedIndex).Text <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " sptypemast.sptypename = '" & Trim(ddlTName.Items(ddlTName.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND sptypemast.sptypename = '" & Trim(ddlTName.Items(ddlTName.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlCCode.Items(ddlCCode.SelectedIndex).Text <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " supplier_agents.catcode = '" & Trim(ddlCCode.Items(ddlCCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND supplier_agents.catcode = '" & Trim(ddlCCode.Items(ddlCCode.SelectedIndex).Text) & "'"
            End If
        End If
        'If ddlCatName.Items(ddlCatName.SelectedIndex).Text <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " catmast.catname = '" & Trim(ddlCatName.Items(ddlCatName.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND catmast.catname = '" & Trim(ddlCatName.Items(ddlCatName.SelectedIndex).Text) & "'"
        '    End If
        'End If

        'If ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " supplier_Agents.scatcode = '" & Trim(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND supplier_Agents.scatcode = '" & Trim(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) & "'"
        '    End If
        'End If

        'If ddlSellingName.Items(ddlSellingName.SelectedIndex).Text <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " sellcatmast.scatname = '" & Trim(ddlSellingName.Items(ddlSellingName.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND sellcatmast.scatname = '" & Trim(ddlSellingName.Items(ddlSellingName.SelectedIndex).Text) & "'"
        '    End If
        'End If


        If ddlContCode.Items(ddlContCode.SelectedIndex).Text <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " ctrymast.ctrycode = '" & Trim(ddlContCode.Items(ddlContCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND ctrymast.ctrycode = '" & Trim(ddlContCode.Items(ddlContCode.SelectedIndex).Text) & "'"
            End If
        End If

        If ddlcontName.Items(ddlcontName.SelectedIndex).Text <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " ctrymast.ctryname = '" & Trim(ddlcontName.Items(ddlcontName.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND ctrymast.ctryname = '" & Trim(ddlcontName.Items(ddlcontName.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " supplier_agents.sectorcode = '" & Trim(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND supplier_agents.sectorcode = '" & Trim(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text) & "'"
            End If
        End If
        'If ddlSectorName.Items(ddlSectorName.SelectedIndex).Text <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " sectormaster.sectorname = '" & Trim(ddlSectorName.Items(ddlSectorName.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND sectormaster.sectorname = '" & Trim(ddlSectorName.Items(ddlSectorName.SelectedIndex).Text) & "'"
        '    End If
        'End If
        If ddlControlCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " supplier_Agents.controlacctcode = '" & Trim(ddlControlCode.Value) & "'"
            Else
                strWhereCond = strWhereCond & " AND supplier_Agents.controlacctcode = '" & Trim(ddlControlCode.Value) & "'"
            End If
        End If

        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblagentname As Label = e.Row.FindControl("lblagentname")

            Dim lsAgentName As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsAgentName = ""

                        If "AGENTNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsAgentName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsAgentName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsAgentName.Trim <> "" Then
                            lblagentname.Text = Regex.Replace(lblagentname.Text.Trim, lsAgentName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                    Next
                End If
            End If



        End If




    End Sub
#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            'strSqlQry = "SELECT supplier_agents.supagentcode, supplier_agents.supagentname, supplier_agents.sptypecode, " & _
            '            " sptypemast.sptypename, supplier_agents.catcode, catmast.catname,   " & _
            '            " supplier_agents.currcode, supplier_agents.ctrycode, ctrymast.ctryname, supplier_agents.sectorcode, sectormaster.sectorname, " & _
            '            " supplier_agents.add1, supplier_agents.tel1, supplier_agents.fax, supplier_agents.stel1, " & _
            '            " supplier_agents.scontact1, supplier_agents.atel1, supplier_agents.acontact1, supplier_agents.controlacctcode,  " & _
            '            " supplier_agents.crdays, supplier_agents.crlimit, supplier_agents.agtcode, supplier_agents.adddate, supplier_agents.adduser, " & _
            '            " supplier_agents.moddate, supplier_agents.moduser,  " & _
            '            " case when isnull(supplier_agents.active,0)=1 then 'Active' when isnull(supplier_agents.active,0)=0 then 'InActive'  end as  Active" & _
            '            " FROM supplier_agents INNER JOIN  " & _
            '            " sptypemast ON supplier_agents.sptypecode = sptypemast.sptypecode INNER JOIN  " & _
            '            " catmast ON supplier_agents.catcode = catmast.catcode INNER JOIN  " & _
            '            " ctrymast ON supplier_agents.ctrycode = ctrymast.ctrycode INNER JOIN  " & _
            '            " sectormaster ON supplier_agents.sectorcode = sectormaster.sectorcode  "
            strSqlQry = "SELECT supplier_agents.supagentcode, supplier_agents.supagentname, supplier_agents.sptypecode, " & _
                                                " sptypemast.sptypename, supplier_agents.catcode, catmast.catname,supplier_agents.scatcode,  " & _
                                                " supplier_agents.currcode, supplier_agents.ctrycode, ctrymast.ctryname, supplier_agents.sectorcode, sectormaster.sectorname, " & _
                                                " supplier_agents.add1, supplier_agents.tel1, supplier_agents.fax, supplier_agents.stel1, " & _
                                                " supplier_agents.scontact1, supplier_agents.atel1, supplier_agents.acontact1, supplier_agents.controlacctcode,  " & _
                                                " supplier_agents.crdays, supplier_agents.crlimit, supplier_agents.agtcode, supplier_agents.adddate, supplier_agents.adduser, " & _
                                                " supplier_agents.moddate, supplier_agents.moduser,  " & _
                                                " case when isnull(supplier_agents.active,0)=1 then 'Active' when isnull(supplier_agents.active,0)=0 then 'InActive'  end as  Active" & _
                                                " FROM supplier_agents left JOIN  " & _
                                                " sptypemast ON supplier_agents.sptypecode = sptypemast.sptypecode left JOIN  " & _
                                                " catmast ON supplier_agents.catcode = catmast.catcode left JOIN  " & _
                                                            " ctrymast ON supplier_agents.ctrycode = ctrymast.ctrycode left JOIN  " & _
                                                " sectormaster ON supplier_agents.sectorcode = sectormaster.sectorcode  "



            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Session.Add("SupagentsState", "New")
        ' Response.Redirect("SupplierAgents.aspx", False)
        'Response.Redirect("SuppAgentMain.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New','Supagents','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New','Supagents');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region


#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("supagentcode")
        'Select Case ddlOrderBy.SelectedIndex
        '    Case 0
        '        FillGrid("supplier_agents.supagentname")
        '    Case 1
        '        FillGrid("supplier_agents.supagentcode")
        '    Case 2
        '        FillGrid("sptypemast.sptypecode")
        '    Case 3
        '        FillGrid("sellcatmast.scatcode")

        'End Select
        FillGridNew()
    End Sub

#End Region


#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblagentCode")

            If e.CommandName = "EditRow" Then
                Session.Add("SupagentsState", "Edit")
                Session.Add("SupagentsRefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SuppAgentMain.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supagents','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supagents');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Session.Add("SupagentsState", "View")
                Session.Add("SupagentsRefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SuppAgentMain.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supagents','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supagents');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Session.Add("SupagentsState", "Delete")
                Session.Add("SupagentsRefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SuppAgentMain.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supagents','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('SuppAgentMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supagents');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("supagentcode")
        Try


            Select Case ddlOrderBy.SelectedIndex
                Case 0
                    FillGrid("supplier_agents.supagentname")
                Case 1
                    FillGrid("supplier_agents.supagentcode")
                Case 2
                    FillGrid("sptypemast.sptypecode")
                Case 3
                    'FillGrid("sellcatmast.scatcode")

            End Select
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtAgentName.Text = ""
        txtAgentCode.Text = ""
        ddlType.Value = "[Select]"
        ddlTName.Value = "[Select]"
        ddlCCode.Value = "[Select]"
        ddlCatName.Value = "[Select]"
        'ddlSellingCode.Value = "[Select]"
        'ddlSellingName.Value = "[Select]"
        ddlContCode.Value = "[Select]"
        ddlcontName.Value = "[Select]"
        ddlSectorCode.Value = "[Select]"
        ddlSectorName.Value = "[Select]"
        ddlControlCode.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        FillGrid("supplier_agents.supagentname")
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        'FillGrid(e.SortExpression, e.SortDirection)
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                'Response.ContentType = "application/vnd.ms-excel"
                'Response.Charset = ""
                'Me.EnableViewState = False

                'Dim tw As New System.IO.StringWriter()
                'Dim hw As New System.Web.UI.HtmlTextWriter(tw)
                'Dim frm As HtmlForm = New HtmlForm()
                'Me.Controls.Add(frm)
                'frm.Controls.Add(gv_SearchResult)
                'frm.RenderControl(hw)
                'Response.Write(tw.ToString())
                'Response.End()
                'Response.Clear()
                strSqlQry = "SELECT supagentcode AS [SupplierAgentCode],supagentname as [SupplierAgentName],supplier_agents.sptypecode as [SupplierTypeCode],sptypemast.sptypename as [SupplierTypeName],supplier_agents.catcode as [CategoryCode],supplier_agents.scatcode as [SellingCategoryCode],supplier_agents.currcode as [Currency],supplier_agents.sectorcode as [Sector Code],supplier_agents.add1 as [AddressLine1]," & _
                            "supplier_Agents.fax as [Fax],supplier_agents.stel1 as [SalesTelephone],supplier_agents.scontact1 as [SalesContactPerson1],supplier_agents.atel1 as [AccountTelephone1],supplier_agents.acontact1 as [AccountontactPerons1],supplier_agents.controlacctcode as [Control A/C Code]," & _
                            "supplier_agents.crdays as [CreditDays],supplier_agents.crlimit as [CreditLimit],supplier_agents.agtcode as [DMC Code]," & _
                            "[Active]=case when supplier_agents.active=1 then 'Active' when supplier_agents.active=0 then 'InActive' end, supplier_agents.adduser as [User Created], (Convert(Varchar, Datepart(DD,supplier_agents.adddate))+ '/'+ Convert(Varchar, Datepart(MM,supplier_agents.adddate))+ '/'+ Convert(Varchar, Datepart(YY,supplier_agents.adddate)) + ' ' + Convert(Varchar, Datepart(hh,supplier_agents.adddate))+ ':' + Convert(Varchar, Datepart(m,supplier_agents.adddate))+ ':'+ Convert(Varchar, Datepart(ss,supplier_agents.adddate))) as [Date Created],supplier_agents.adduser as [User Created] ,(Convert(Varchar, Datepart(DD,supplier_agents.moddate))+ '/'+ Convert(Varchar, Datepart(MM,supplier_agents.moddate))+ '/'+ Convert(Varchar, Datepart(YY,supplier_agents.moddate)) + ' ' + Convert(Varchar, Datepart(hh,supplier_agents.moddate))+ ':' + Convert(Varchar, Datepart(m,supplier_agents.moddate))+ ':'+ Convert(Varchar, Datepart(ss,supplier_agents.moddate))) as [Date Modified],supplier_agents.moduser as [User Modified]" & _
                            "FROM supplier_agents left JOIN sptypemast ON supplier_agents.sptypecode = sptypemast.sptypecode "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY supagentcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY supagentcode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "supplier_agents")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            ' strpop = "window.open('rptReportNew.aspx?Pageame=Supplier Agent&BackPageName=SupplierAgentsSearch.aspx&SupagentCode=" & txtAgentCode.Text.Trim & "&SupagentName=" & txtAgentName.Text.Trim & "&SuptypeCode=" & Trim(ddlType.Items(ddlType.SelectedIndex).Text) & "&SuptypeName=" & Trim(ddlTName.Items(ddlTName.SelectedIndex).Text) & "&SupCatCode=" & Trim(ddlCCode.Items(ddlCCode.SelectedIndex).Text) & "&SupcatName=" & Trim(ddlCatName.Items(ddlCatName.SelectedIndex).Text) & "&SellcatCode=" & Trim(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) & "&SellcatName=" & Trim(ddlSellingName.Items(ddlSellingName.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlContCode.Items(ddlContCode.SelectedIndex).Text) & "&CtryName=" & Trim(ddlcontName.Items(ddlcontName.SelectedIndex).Text) & "&SectCode=" & Trim(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text) & "&SectName=" & Trim(ddlSectorName.Items(ddlSectorName.SelectedIndex).Text) & "&CtrlaccCode=" & Trim(ddlControlCode.Items(ddlControlCode.SelectedIndex).Text) & "','RepSupAgent','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Supplier Agent&BackPageName=SupplierAgentsSearch.aspx&SupagentCode=" & txtAgentCode.Text.Trim & "&SupagentName=" & txtAgentName.Text.Trim & "&SuptypeCode=" & Trim(ddlType.Items(ddlType.SelectedIndex).Text) & "&SuptypeName=" & Trim(ddlTName.Items(ddlTName.SelectedIndex).Text) & "&SupCatCode=" & Trim(ddlCCode.Items(ddlCCode.SelectedIndex).Text) & "&SupcatName=" & Trim(ddlCatName.Items(ddlCatName.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlContCode.Items(ddlContCode.SelectedIndex).Text) & "&CtryName=" & Trim(ddlcontName.Items(ddlcontName.SelectedIndex).Text) & "&SectCode=" & Trim(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text) & "&SectName=" & Trim(ddlSectorName.Items(ddlSectorName.SelectedIndex).Text) & "&CtrlaccCode=" & Trim(ddlControlCode.Items(ddlControlCode.SelectedIndex).Text) & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Supplier Agent")
            'Session.Add("BackPageName", "SupplierAgentsSearch.aspx")

            'If txtAgentCode.Text.Trim <> "" Then
            '    strReportTitle = "Agent Code : " & txtAgentCode.Text.Trim
            '    strSelectionFormula = "{supplier_agents.supagentcode} LIKE '" & txtAgentCode.Text.Trim & "*'"
            'End If
            'If txtAgentName.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Agent Name : " & txtAgentName.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} LIKE '" & txtAgentName.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Agent Name : " & txtAgentName.Text.Trim
            '        strSelectionFormula = "{supplier_agents.supagentname} LIKE '" & txtAgentName.Text.Trim & "*'"
            '    End If
            'End If
            'If ddlType.Items(ddlType.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Supplier Type Code : " & ddlType.Items(ddlType.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {supplier_agents.sptypecode} = '" & ddlType.Items(ddlType.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Supplier Type  Code: " & ddlType.Items(ddlType.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{supplier_agents.sptypecode} = '" & ddlType.Items(ddlType.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'If ddlTName.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Supplier Type Name : " & ddlTName.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & ddlTName.Value.Trim & "'"
            '    Else
            '        strReportTitle = "Supplier Type Name: " & ddlTName.Value.Trim
            '        strSelectionFormula = "{sptypemast.sptypename} = '" & ddlTName.Value.Trim & "'"
            '    End If
            'End If
            'If ddlCCode.Items(ddlCCode.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Category Code : " & ddlCCode.Items(ddlCCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {supplier_agents.catcode} = '" & ddlCCode.Items(ddlCCode.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Category Code: " & ddlCCode.Items(ddlCCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{supplier_agents.catcode} = '" & ddlCCode.Items(ddlCCode.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'If ddlCatName.Items(ddlCatName.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Category Name : " & ddlCatName.Items(ddlCatName.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {catmast.catname} = '" & ddlCatName.Items(ddlCatName.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Category Name: " & ddlCatName.Items(ddlCatName.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{catmast.catname} = '" & ddlCatName.Items(ddlCatName.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'If ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Selling Category Code : " & ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {supplier_Agents.scatcode} = '" & ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Selling Category Code: " & ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{supplier_Agents.scatcode} = '" & ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If
            'If ddlSellingName.Items(ddlSellingName.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Selling Category Name : " & ddlSellingName.Items(ddlSellingName.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sellcatmast.scatname} = '" & ddlSellingName.Items(ddlSellingName.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Selling Category Name: " & ddlSellingName.Items(ddlSellingName.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{sellcatmast.scatname} = '" & ddlSellingName.Items(ddlSellingName.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If
            'If ddlContCode.Items(ddlContCode.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country Code : " & ddlContCode.Items(ddlContCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {ctrymast.ctrycode} = '" & ddlContCode.Items(ddlContCode.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Country Code: " & ddlContCode.Items(ddlContCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{ctrymast.ctrycode} = '" & ddlContCode.Items(ddlContCode.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If
            'If ddlcontName.Items(ddlcontName.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country Name : " & ddlcontName.Items(ddlcontName.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & ddlcontName.Items(ddlcontName.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Country Name: " & ddlcontName.Items(ddlcontName.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{ctrymast.ctryname} = '" & ddlcontName.Items(ddlcontName.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If
            'If ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Sector Code : " & ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorcode} = '" & ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Sector Code: " & ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{sectormaster.sectorcode} = '" & ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If
            'If ddlSectorName.Items(ddlSectorName.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Sector Name : " & ddlSectorName.Items(ddlSectorName.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorname} = '" & ddlSectorName.Items(ddlSectorName.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Sector Name: " & ddlSectorName.Items(ddlSectorName.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{sectormaster.sectorname} = '" & ddlSectorName.Items(ddlSectorName.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If
            'If ddlControlCode.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Control Code : " & ddlControlCode.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {supplier_Agents.controlacctcode} = '" & ddlControlCode.Value.Trim & "'"
            '    Else
            '        strReportTitle = "Control Code: " & ddlControlCode.Value.Trim
            '        strSelectionFormula = "{supplier_Agents.controlacctcode} = '" & ddlControlCode.Value.Trim & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        'Dim lsSearchTxt As String = ""
        'lsSearchTxt = txtvsprocesssplit.Text '.Replace(": """, ":""")
        'Dim lsProcessCity As String = ""
        'Dim lsProcessCountry As String = ""
        ''txtvsprocessCity.Text = ""
        ''txtvsprocessCountry.Text = ""
        'Dim lsMainArr As String()
        ''lsMainArr = lsSearchTxt.Split("|~,")
        'lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        'For i = 0 To lsMainArr.GetUpperBound(0)
        '    Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
        '        Case "CITY"
        '            'txtvsprocessCity.Text = Mid(lsMainArr(i).Split(":")(1), 2, lsMainArr(i).Split(":")(1).ToString.Length - 2)
        '            'txtvsprocessCity.Text = lsMainArr(i).Split(":")(1)
        '            lsProcessCity = lsMainArr(i).Split(":")(1)
        '        Case "COUNTRY"
        '            'txtvsprocessCountry.Text = Mid(lsMainArr(i).Split(":")(1), 2, lsMainArr(i).Split(":")(1).ToString.Length - 2)
        '            'txtvsprocessCountry.Text = lsMainArr(i).Split(":")(1)
        '            lsProcessCountry = lsMainArr(i).Split(":")(1)
        '    End Select
        'Next
        'txtcitycode.Text = ""
        'txtcityname.Text = ""
        ''ddlsccode.SelectedIndex = -1
        ''ddlscname.SelectedIndex = -1
        'objUtils.sbSetSelectedValueForHTMLSelect("[Select]", ddlsccode)
        'objUtils.sbSetSelectedValueForHTMLSelect("[Select]", ddlscname)
        'If lsProcessCity.Trim <> "" Then
        '    txtcityname.Text = lsProcessCity.Trim
        'End If

        'If lsProcessCountry.Trim <> "" Then
        '    objUtils.sbSetSelectedValueForHTMLSelect(lsProcessCountry, ddlscname)
        'End If

        'Select Case ddlOrderBy.SelectedIndex
        '    Case 0
        '        FillGrid("cityname")
        '    Case 1
        '        FillGrid("citycode")
        '    Case 2
        '        FillGrid("ctrycode")


        'End Select
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbsearch.CheckedChanged
        'PnlSupplierAgent.Visible = False
        lblCategoryCode.Visible = False
        lblcategoryname.Visible = False
        lblcontrolACCode.Visible = False
        lblCountryCode.Visible = False
        lblcountryname.Visible = False
        lblSectorCode.Visible = False
        lblSectorName.Visible = False
        'lblSellingCategoryCode.Visible = False
        'lblSellingCategoryName.Visible = False
        lblSupplierTypeCode.Visible = False
        lblSupplierTypeName.Visible = False

        ddlType.Visible = False
        ddlTName.Visible = False
        ddlCCode.Visible = False
        ddlCatName.Visible = False
        'ddlSellingCode.Visible = False
        'ddlSellingName.Visible = False
        ddlContCode.Visible = False
        ddlcontName.Visible = False
        ddlSectorCode.Visible = False
        ddlSectorName.Visible = False
        ddlControlCode.Visible = False

    End Sub
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsAgentCategory As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "AGENTNAME"
                    lsAgentCategory = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("AGENTNAME", lsAgentCategory, "AGENTNAME")

                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub

    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function
    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub rbnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbnadsearch.CheckedChanged
        '        PnlSupplierAgent.Visible = True
        lblCategoryCode.Visible = True
        lblcategoryname.Visible = True
        lblcontrolACCode.Visible = True
        lblCountryCode.Visible = True
        lblcountryname.Visible = True
        lblSectorCode.Visible = True
        lblSectorName.Visible = True
        'lblSellingCategoryCode.Visible = True
        'lblSellingCategoryName.Visible = True
        lblSupplierTypeCode.Visible = True
        lblSupplierTypeName.Visible = True

        ddlType.Visible = True
        ddlTName.Visible = True
        ddlCCode.Visible = True
        ddlCatName.Visible = True
        'ddlSellingCode.Visible = True
        'ddlSellingName.Visible = True
        ddlContCode.Visible = True
        ddlcontName.Visible = True
        ddlSectorCode.Visible = True
        ddlSectorName.Visible = True
        ddlControlCode.Visible = True

        ddlType.Value = "[Select]"
        ddlTName.Value = "[Select]"
        ddlCCode.Value = "[Select]"
        ddlCatName.Value = "[Select]"
        'ddlSellingCode.Value = "[Select]"
        'ddlSellingName.Value = "[Select]"
        ddlContCode.Value = "[Select]"
        ddlcontName.Value = "[Select]"
        ddlSectorCode.Value = "[Select]"
        ddlSectorName.Value = "[Select]"
        ddlControlCode.Value = "[Select]"
    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("SupplierAgent Name")
        ddlOrderBy.Items.Add("SupplierAgent Code")
        ddlOrderBy.Items.Add("Supplier Type code")
        ddlOrderBy.Items.Add("Category Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("supplier_agents.supagentname")
            Case 1
                FillGrid("supplier_agents.supagentcode")
            Case 2
                FillGrid("sptypemast.sptypecode")
                'Case 3
                '    FillGrid("sellcatmast.scatcode")

        End Select
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Function getRowpage() As String
        Dim rowpageas As String
        If RowsPerPageAS.SelectedValue = "20" Then
            rowpageas = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpageas = RowsPerPageAS.SelectedValue

        End If
        Return rowpageas
    End Function
    Protected Sub RowsPerPageSCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageAS.SelectedIndexChanged
        FillGridNew()
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupplierAgentsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Private Function BuildConditionNew(ByVal strAgentValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strAgentValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(supplier_agents.supagentname) IN (" & Trim(strAgentValue.Trim.ToUpper) & ")"
                'Else
                '    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            End If
        End If
        If strTextValue <> "" Then
            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then
                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = "(upper(supplier_agents.supagentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
                        'Else
                        'strWhereCond1 = strWhereCond1 & " OR  (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    End If
                End If
            Next



            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If

        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),supplier_agents.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),supplier_agents.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),supplier_agents.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),supplier_agents.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function
    Private Sub FillGridNew()
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strAgentValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "AGENTNAME" Then
                        If strAgentValue <> "" Then
                            strAgentValue = strAgentValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strAgentValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                    End If
                Next
            End If
            Dim pagevaluesas = RowsPerPageAS.SelectedValue
            strBindCondition = BuildConditionNew(strAgentValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"
            strSqlQry = "SELECT supplier_agents.supagentcode, supplier_agents.supagentname, supplier_agents.sptypecode, " & _
                                                " sptypemast.sptypename, supplier_agents.catcode, catmast.catname,supplier_agents.scatcode,  " & _
                                                " supplier_agents.currcode, supplier_agents.ctrycode, ctrymast.ctryname, supplier_agents.sectorcode, sectormaster.sectorname, " & _
                                                " supplier_agents.add1, supplier_agents.tel1, supplier_agents.fax, supplier_agents.stel1, " & _
                                                " supplier_agents.scontact1, supplier_agents.atel1, supplier_agents.acontact1, supplier_agents.controlacctcode,  " & _
                                                " supplier_agents.crdays, supplier_agents.crlimit, supplier_agents.agtcode, supplier_agents.adddate, supplier_agents.adduser, " & _
                                                " supplier_agents.moddate, supplier_agents.moduser,  " & _
                                                " case when isnull(supplier_agents.active,0)=1 then 'Active' when isnull(supplier_agents.active,0)=0 then 'InActive'  end as  Active" & _
                                                " FROM supplier_agents left JOIN  " & _
                                                " sptypemast ON supplier_agents.sptypecode = sptypemast.sptypecode left JOIN  " & _
                                                " catmast ON supplier_agents.catcode = catmast.catcode left JOIN  " & _
                                                            " ctrymast ON supplier_agents.ctrycode = ctrymast.ctrycode left JOIN  " & _
                                                " sectormaster ON supplier_agents.sectorcode = sectormaster.sectorcode"
            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            gv_SearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = pagevaluesas
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()

    End Sub
End Class
