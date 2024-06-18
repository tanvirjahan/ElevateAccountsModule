Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic

Partial Class ContractMain
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime
    Dim myDataAdapter As SqlDataAdapter

#End Region

#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub

    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentListSearch(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lsAgentNames As New List(Of String)
        Dim lsCountryList As String
        Try

            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList

            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                'If HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl") IsNot Nothing Then 'changed by mohamed on 03/10/2016 - instead of selected, used all
                'lsCountryList = HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    'strSqlQry += " and a.ctrycode in (" & lsCountryList & ")" 'changed by mohamed on 03/10/2016 -'commented this line to show all the agents.
                End If
            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'lsAgentNames.Add(myDS.Tables(0).Rows(i)("agentname").ToString())
                    lsAgentNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next
            End If

            Return lsAgentNames
        Catch ex As Exception

            Return lsAgentNames
        End Try

    End Function
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            Dim RefCode As String
            If IsPostBack = False Then

                '   PanelMain.Visible = True
                'charcters(txtCode)
                'charcters(txtName)

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If

                'If Session("ContractState") Is Nothing Then
                '    Session("ContractState") = CType(Request.QueryString("ContractState"), String)

                'End If

                wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))

                If CType(Session("ContractState"), String) <> "New" Then
                    'Session("ContractRefCode") = CType(Request.QueryString("contractid"), String)
                    'Session("contractid") = CType(Request.QueryString("contractid"), String)
                    wucCountrygroup.sbSetPageState(Session("contractid"), Nothing, Nothing)
                End If

                txtconnection.Value = Session("dbconnectionName")
                txtfromDate.Text = Now.ToString("dd/MM/yyyy")
                txtToDate.Text = Now.ToString("dd/MM/yyyy")
                hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")
                txtefffromdate.Text = Now.ToString("dd/MM/yyyy")
                txtefftodate.Text = Now.ToString("dd/MM/yyyy")

                If Session("Contractparty") Is Nothing Then
                    ViewState("partycode") = CType(Request.QueryString("partycode"), String)
                    hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                Else
                    hdnpartycode.Value = Session("Contractparty")
                    ViewState("partycode") = hdnpartycode.Value
                End If


                'ViewState("partycode") = CType(Session("partycode"), String)
                'hdnpartycode.Value = CType(Session("partycode"), String)

                Session("Contractparty") = hdnpartycode.Value
                Session("partycode") = hdnpartycode.Value
                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast(nolock) where partycode='" & ViewState("partycode") & "'")
                txthotelname.Enabled = False
                sbGenerateSeasonGridColumns("BLANK", 0)

                txtsupagentcode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")
                txtsupagentname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select supagentname from supplier_agents where supagentcode='" & txtsupagentcode.Text & "'")
                lblstatustext.Visible = False
                lblstatus.Visible = False
                FillTerms()
                '  FillFormula()

                txtservicerate.Enabled = False
                txttaxperc.Enabled = False
                txtcommperc.Enabled = False

                Numberssrvctrl(txtservicerate)
                Numberssrvctrl(txttaxperc)
                Numberssrvctrl(txtcommperc)

                If CType(Session("ContractState"), String) = "New" Then
                    SetFocus(txtName)

                    Page.Title = Page.Title + " " + "New Contracts"
                    txtCode.Disabled = True
                    ' wucCountrygroup.clearsessions()
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()

                    Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(validfrom,'') validfrom ,isnull(validto,'') validto  from Contract_Email(nolock) where  hotelid='" & hdnpartycode.Value & "' and assignedto='" & CType(Session("GlobalUserName"), String) & "' and emailid='" & Session("sEmailCode") & "'")
                    If ds.Tables(0).Rows.Count > 0 Then
                        txtfromDate.Text = ds.Tables(0).Rows(0).Item("validfrom")
                        txtToDate.Text = ds.Tables(0).Rows(0).Item("validto")

                        txtefffromdate.Text = ds.Tables(0).Rows(0).Item("validfrom")
                        txtefftodate.Text = ds.Tables(0).Rows(0).Item("validto")
                    End If





                    txthotelname.Focus()
                    lblHeading.Text = "Add New Contracts - " + txthotelname.Text
                    Page.Title = "Contract Main "

                    'txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rnkorder),0) from partymast") + 1
                    btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Contracts?')==false)return false;")
                ElseIf CType(Session("ContractState"), String) = "Copy" Then
                    txthotelname.Focus()

                    Page.Title = Page.Title + " " + "Copy Contracts"
                    txtCode.Disabled = True
                    RefCode = CType(Session("ContractRefCode"), String)
                    ShowRecord(RefCode)
                    fillseasondates(RefCode)
                    FillTerms()
                    '  FillFormula(RefCode)
                    '  Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    ' wucCountrygroup.sbShowCountry()
                    lblHeading.Text = "Copy Contracts - " + txthotelname.Text
                    txtcontractid.Text = ""
                    'txtCode.Value = ""
                    'txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rnkorder),0) from partymast") + 1
                    btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Contracts?')==false)return false;")
                    Page.Title = "Contract Main "
                ElseIf CType(Session("ContractState"), String) = "Edit" Then

                    btnSave.Text = "Update"

                    RefCode = CType(Session("ContractRefCode"), String)
                    ShowRecord(RefCode)
                    wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("ContractState"), String))
                    ' 
                    '    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    fillseasondates(RefCode)
                    FillTerms()
                    '   FillFormula(RefCode)

                    txtcontractid.Enabled = False
                    txthotelname.Enabled = False
                    lblHeading.Text = "Edit Contracts - " + txthotelname.Text
                    Page.Title = "Contract Main "
                    btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                    '  btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Contracts?')==false)return false;")
                ElseIf CType(Session("ContractState"), String) = "View" Then

                    RefCode = CType(Session("ContractRefCode"), String)
                    wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("ContractState"), String))

                    txtCode.Disabled = True
                    txtName.Disabled = True
                    ShowRecord(RefCode)
                    fillseasondates(RefCode)
                    FillTerms()
                    '   FillFormula(RefCode)

                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    Page.Title = Page.Title + " " + "View Contracts "
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    btnCancel.Focus()
                    txtcontractid.Enabled = False
                    txthotelname.Enabled = False
                    DisableControl()
                    lblHeading.Text = "View Contracts - " + txthotelname.Text
                    Page.Title = "Contract Main "
                ElseIf CType(Session("ContractState"), String) = "PendingView" Then
                    Session.Remove("ContractState")
                    Session.Add("ContractState", "View")

                    Dim contractid As String = Request.QueryString("contractid")

                    Dim partycode As String = Request.QueryString("partycode")
                    Session.Add("ContractRefCode", CType(contractid, String))
                    Session("Contractparty") = partycode
                    Session.Add("contractid", CType(contractid, String))
                    If Session("sDtDynamic") Is Nothing Then
                        Dim dtDynamic = New DataTable()
                        Dim dcCode = New DataColumn("Code", GetType(String))
                        Dim dcValue = New DataColumn("Value", GetType(String))
                        Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                        dtDynamic.Columns.Add(dcCode)
                        dtDynamic.Columns.Add(dcValue)
                        dtDynamic.Columns.Add(dcCodeAndValue)
                        Session("sDtDynamic") = dtDynamic
                    End If
                    RefCode = CType(Session("ContractRefCode"), String)
                    wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("ContractState"), String))

                    txtCode.Disabled = True
                    txtName.Disabled = True
                    ShowRecord(RefCode)
                    fillseasondates(RefCode)
                    FillTerms()
                    ' FillFormula(RefCode)

                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    Page.Title = Page.Title + " " + "View Contracts "
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    btnCancel.Focus()
                    txtcontractid.Enabled = False
                    txthotelname.Enabled = False
                    DisableControl()
                    lblHeading.Text = "View Contracts - " + txthotelname.Text
                    Page.Title = "Contract Main "
                ElseIf CType(Session("ContractState"), String) = "Delete" Then

                    RefCode = CType(Session("ContractRefCode"), String)
                    wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("ContractState"), String))

                    ShowRecord(RefCode)
                    fillseasondates(RefCode)
                    FillTerms()
                    '   FillFormula(RefCode)
                    DisableControl()
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    Page.Title = Page.Title + " " + "Delete Contracts"
                    lblHeading.Text = "Delete Contracts - " + txthotelname.Text
                    btnSave.Text = "Delete"
                    btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                    Page.Title = "Contract Main "
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Contracts?')==false)return false;")

                End If
            Else
                Try

                    Enabletax()

                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                    objUtils.WritErrorLog("ContractsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
                End Try
            End If
            If chkcommission.Checked = True Then
                divcommision.Style.Add("display", "block")
                divterms.Style.Add("display", "block")
            Else
                divcommision.Style.Add("display", "none")
                divterms.Style.Add("display", "none")
            End If


            txtefffromdate.Attributes.Add("onchange", "checkfromdates('" & txtefffromdate.ClientID & "','" & txtefftodate.ClientID & "');")

            chkcommission.Attributes.Add("onChange", "showcommission('" & chkcommission.ClientID & "')")

            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            End If
            Session.Add("submenuuser", "ContractsSearch.aspx")
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
    Private Sub FillTerms()
        Try
            Dim strSqlQry As String
            Dim myDS As New DataSet
            strSqlQry = "select RankOrder,TermCode,TermName from commissionterms order by rankOrder"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdCommTerms.DataSource = myDS
            grdCommTerms.DataBind()
            clsDBConnect.dbAdapterClose(myDataAdapter)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try
    End Sub
    Private Sub FillFormula(Optional ByVal contractid As String = "")
        Dim myDS As New DataSet
        Dim strsql As String = ""
        Dim formulaid As String = ""


        grdcommission.Visible = True

        strsql = "SELECT formulaid,formulaname , (select left(terms,len(terms)-2) from (select (select term1 + ' ' + operator1 + ' ' + term2 + ' = ' + resultterm+ ' | ' from commissionformula_detail where formulaid=commissionformula_header.formulaid order by flineno For XML PATH ('')) as terms) as t) as [Formula]   FROM commissionformula_header where active=1 "


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
        myDataAdapter.Fill(myDS)
        grdcommission.DataSource = myDS
        grdcommission.DataBind()

        If chkcommission.Checked = True Then

            Dim formulacode As String = ""
            If contractid <> "" Then
                formulacode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select formulaid from view_contractcommission(nolock) where contractid='" & contractid & "'")
            End If

            For Each gvrow As GridViewRow In grdcommission.Rows

                Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
                Dim txtformula As Label = gvrow.FindControl("txtformulacode")
                Dim optcomm As RadioButton = gvrow.FindControl("optcomm")


                If txtformula.Text = formulacode Then
                    chkcomm.Checked = True
                    optcomm.Checked = True

                End If

                If optcomm.Checked = True Then
                    formulaid = txtformula.Text
                End If


            Next






        End If

        If CType(Session("ContractState"), String) = "New" Then

            grdcommissiondetail.Visible = True
            myDS.Clear()
            strsql = "SELECT  d.term1,t.termname,'' value, t.rankorder from commissionformula_detail d inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.formulaid='" & formulaid & "' union all  " _
                     & " select d.term2,t.termname,'' value ,t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & formulaid & "' order by 4"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        Else
            grdcommissiondetail.Visible = True
            myDS.Clear()
            strsql = "SELECT  d.term1,t.termname,d.value,t.rankorder   from view_contractcommission d  inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.contractid='" & contractid & "' union all " _
                     & " select d.term2,t.termname ,0,t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & formulaid & "' and d.term2 not in (select term1 from view_contractcommission where contractid='" & contractid & "') order by 4"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        End If
        Enabletax()

    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Gethotelslist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select partyname, partycode from  partymast where isnull(sptypecode,'') in (select option_selected from reservation_parameters where param_id=458) and active=1 and partyname like  '" & prefixText & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupagentlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim supagentname As New List(Of String)
        Try

            strSqlQry = "select supagentname, supagentcode from  supplier_agents where active=1 and supagentname like  '" & Trim(prefixText) & "%' order by supagentname "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    supagentname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("supagentname").ToString(), myDS.Tables(0).Rows(i)("supagentcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return supagentname
        Catch ex As Exception
            Return supagentname
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSeasonlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select distinct subseasname from contracts_seasons"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString())
                Next

            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function
    Private Sub Showseasons(ByVal RefCode As String)
        Try

            Dim myDS As New DataSet


            strSqlQry = ""



            strSqlQry = "select rowid,SeasonName from view_contractseasons(nolock) where contractid='" & CType(RefCode, String) & "' "


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            GvSeasonShow.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                GvSeasonShow.DataBind()

            Else
                GvSeasonShow.DataBind()

            End If




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub fillseasondates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim dtSeason As New DataTable
            Dim drSeason As DataRow
            Dim liRowId As Integer = 0
            Dim liRowIdCurr As Integer = -1

            dtSeason.Columns.Add(New DataColumn("RowId", GetType(Integer)))
            dtSeason.Columns.Add(New DataColumn("SeasonName", GetType(String)))
            dtSeason.Columns.Add(New DataColumn("FromDate", GetType(Date)))
            dtSeason.Columns.Add(New DataColumn("ToDate", GetType(Date)))
            dtSeason.Columns.Add(New DataColumn("MinNight", GetType(String)))
            dtSeason.Columns.Add(New DataColumn("oldSeasonName", GetType(String)))




            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            


            'mySqlCmd = New SqlCommand("Select RowId,SeasonName,convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,MinNight, SeasonName oldSeasonName" _
            '                         & "    from view_contractseasons(nolock) Where contractid='" & txtcontractid.Text & "' ORDER BY  ROW_NUMBER() over (partition by contractid order by contractid,fromdate) ", mySqlConn)

            'Added By rosalin 2019-10-21
            strSqlQry = "exec New_contractseasons'" & CType(txtcontractid.Text, String) & "' "
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    drSeason = dtSeason.NewRow()
                    drSeason("RowId") = (mySqlReader("RowId"))
                    drSeason("SeasonName") = (mySqlReader("SeasonName"))
                    drSeason("FromDate") = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")
                    drSeason("ToDate") = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                    drSeason("MinNight") = (mySqlReader("MinNight"))
                    drSeason("oldSeasonName") = (mySqlReader("oldSeasonName"))

                    dtSeason.Rows.Add(drSeason)
                End While
            End If

            Session("gvSeasonShow_datatable") = dtSeason

            Dim dvSeasonMain As DataView, dtSeasonMain As DataTable
            dvSeasonMain = New DataView(dtSeason)

            dvSeasonMain.RowFilter = ""
            dtSeasonMain = dvSeasonMain.ToTable(True, {"SeasonName", "RowId", "oldSeasonName"})
            GvSeasonShow.DataSource = dtSeasonMain
            GvSeasonShow.DataBind()


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        If CType(Session("ContractState"), String) = "View" Or CType(Session("ContractState"), String) = "Delete" Then
            txtcontractid.Enabled = False
            txthotelname.Enabled = False
            txtApplicableTo.Enabled = False
            txtfromDate.Enabled = False
            txtToDate.Enabled = False
            gvSeasonInput.Enabled = False
            GvSeasonShow.Enabled = False
            wucCountrygroup.Disable(False)
            txtsupagentname.Enabled = False
            ImgBtnFrmDt.Enabled = False
            ImgBtnToDt.Enabled = False
            grdcommission.Enabled = False
            txtservicerate.Enabled = False
            txttaxperc.Enabled = False
            txtcommperc.Enabled = False
            chkcommission.Enabled = False
        Else
            txthotelname.Enabled = False
            txtApplicableTo.Enabled = True
            txtfromDate.Enabled = True
            txtToDate.Enabled = True
            gvSeasonInput.Enabled = True
            GvSeasonShow.Enabled = True
            wucCountrygroup.Disable(True)
            txtsupagentname.Enabled = True
            ImgBtnFrmDt.Enabled = True
            ImgBtnToDt.Enabled = False
            grdcommission.Enabled = True
            chkcommission.Enabled = True
        End If


    End Sub
#End Region
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select a.partyname,c.fromdate,c.todate,c.applicableto,c.contractid,c.partycode,c.supagentcode,c.supagentname,c.status,c.Commissionable , " _
                                      & " c.effectivefrom,c.effectiveto,c.activestate,c.withdraw,c.titlename from view_contracts_search c(nolock),partymast a  Where c.partycode=a.partycode and c.contractid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partyname")) = False Then
                        txthotelname.Text = mySqlReader("partyname")
                        hdnpartycode.Value = mySqlReader("partycode")
                        SubMenuUserControl1.partyval = hdnpartycode.Value
                        If Session("Contractparty") = "" Or Session("Contractparty") Is Nothing Then
                            Session("Contractparty") = hdnpartycode.Value

                        End If
                        If Session("partycode") = "" Or Session("partycode") Is Nothing Then
                            Session("partycode") = hdnpartycode.Value
                        End If

                    End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        txtfromDate.Text = mySqlReader("fromdate")
                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        txtToDate.Text = mySqlReader("todate")
                    End If

                    If IsDBNull(mySqlReader("titlename")) = False Then
                        txttitle.Text = mySqlReader("titlename")

                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)
                        hdnapplicable.Value = CType(Replace(txtApplicableTo.Text, " ", ""), String)
                    End If
                    If IsDBNull(mySqlReader("contractid")) = False Then
                        txtcontractid.Text = mySqlReader("contractid")
                        SubMenuUserControl1.contractval = txtcontractid.Text
                    End If
                    If IsDBNull(mySqlReader("supagentcode")) = False Then
                        txtsupagentcode.Text = mySqlReader("supagentcode")
                        txtsupagentname.Text = mySqlReader("supagentname")
                    End If
                    If IsDBNull(mySqlReader("status")) = False Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = IIf(mySqlReader("status").ToString.ToUpper = "YES", "APPROVED", "UNAPPROVED")


                    End If
                    If IsDBNull(mySqlReader("Commissionable")) = False Then
                        chkcommission.Checked = IIf(mySqlReader("Commissionable") = 1, True, False)
                    End If

                    If IsDBNull(mySqlReader("effectivefrom")) = False Then
                        txtefffromdate.Text = mySqlReader("effectivefrom")
                    End If
                    If IsDBNull(mySqlReader("effectiveto")) = False Then
                        txtefftodate.Text = mySqlReader("effectiveto")
                    End If

                    If IsDBNull(mySqlReader("withdraw")) = False Then
                        'If mySqlReader("activestate") = "" Or mySqlReader("activestate") = "Active" Then
                        '    chkactive.Checked = True
                        'Else
                        '    chkactive.Checked = False
                        'End If
                        chkwithdraw.Checked = mySqlReader("withdraw") 'IIf(mySqlReader("withdraw") = "", True, False)
                    End If


                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region
    Private Sub Enabletax()

        Dim str As String = ""
        Dim sr As Boolean = False
        Dim tax As Boolean = False
        Dim comm As Boolean = False
        For Each gvrow As GridViewRow In grdcommission.Rows

            Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
            Dim lblFormulaName As Label = gvrow.FindControl("lblFormula")
            Dim optcomm As RadioButton = gvrow.FindControl("optcomm")


            If optcomm.Checked = True Then
                str = lblFormulaName.Text

                str = str.Replace("+", " ")
                str = str.Replace("-", " ")
                str = str.Replace("*", " ")
                str = str.Replace("/", " ")
                str = str.Replace("=", " ")
                str = str.Replace("|", " ")


                If lblFormulaName.Text.Contains(" SR ") And lblFormulaName.Text.Contains(" T ") And lblFormulaName.Text.Contains(" C ") Then
                    txtservicerate.Enabled = True
                    txttaxperc.Enabled = True
                    txtcommperc.Enabled = True
                    sr = True
                    tax = True
                    comm = True
                    Exit For
                End If

                If lblFormulaName.Text.Contains(" SR ") And lblFormulaName.Text.Contains(" T ") And lblFormulaName.Text.Contains(" C ") = False Then
                    txtservicerate.Enabled = True
                    txttaxperc.Enabled = True
                    txtcommperc.Enabled = False
                    sr = True
                    tax = True
                    comm = False
                    Exit For
                End If

                If lblFormulaName.Text.Contains(" T ") And lblFormulaName.Text.Contains(" C ") And lblFormulaName.Text.Contains(" SR ") = False Then
                    txtservicerate.Enabled = False
                    txttaxperc.Enabled = True
                    txtcommperc.Enabled = True
                    sr = False
                    tax = True
                    comm = True
                    Exit For
                End If
                If lblFormulaName.Text.Contains(" C ") And lblFormulaName.Text.Contains(" T ") = False Then
                    txtservicerate.Enabled = False
                    txttaxperc.Enabled = False
                    txtcommperc.Enabled = True
                    sr = False
                    tax = False
                    comm = True
                    Exit For
                End If




            End If

        Next

        'For Each gvrow As GridViewRow In grdcommissiondetail.Rows

        '    Dim txtperc As TextBox = gvrow.FindControl("txtperc")
        '    Dim txtterm1 As Label = gvrow.FindControl("txtterm1")

        '    If txtterm1.Text = "SR" And sr = True Then
        '        txtperc.Enabled = True
        '    ElseIf txtterm1.Text = "T" And tax = True Then
        '        txtterm1.Enabled = True
        '    ElseIf txtterm1.Text = "C" And comm = True Then
        '        txtterm1.Enabled = True
        '    Else
        '        txtterm1.Enabled = False
        '    End If

        'Next



    End Sub
    Protected Sub btnshowcomm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillFormula()

    End Sub
    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Enabletax()

    End Sub
    'Private Function checkseasondelete() As Boolean

    '    Dim ds As DataSet
    '    Dim parms2 As New List(Of SqlParameter)
    '    Dim parm2(1) As SqlParameter
    '    Dim strMsg As String = ""

    '    Dim txtSeasonName As TextBox
    '    Dim txtSeasonfromDate As TextBox
    '    Dim txtSeasonToDate As TextBox

    '    Dim GvSeasonShowSub As GridView
    '    Dim lblRowId As Label

    '    Dim dtseasonnew As New DataTable
    '    Dim dsseason As New DataSet
    '    Dim dr As DataRow
    '    Dim xmlseason As String = ""

    '    For Each gvRow As GridViewRow In GvSeasonShow.Rows

    '        lblRowId = gvRow.FindControl("lblRowId")
    '        txtSeasonName = gvRow.FindControl("txtSeasonName")

    '        parm2(0) = New SqlParameter("@season", CType(txtSeasonName.Text, String))
    '        parm2(1) = New SqlParameter("@contractid", CType(txtcontractid.Text, String))
    '        For i = 0 To 1
    '            parms2.Add(parm2(i))
    '        Next
    '        ds = New DataSet()
    '        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkseason_delete", parms2)
    '        If ds.Tables.Count > 0 Then
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
    '                    strMsg = "Missing Season Dates Period  " + "\n"
    '                    For i = 0 To ds.Tables(0).Rows.Count - 1

    '                        strMsg += " From Date -  " + ds.Tables(0).Rows(i)("fromdateC") + " - Todate  " + ds.Tables(0).Rows(i)("Todate") + "\n"
    '                    Next
    '                    ' strMsg = "Missing Season Dates Period  " + " From Date -  " + ds.Tables(0).Rows(0)("fromdateC") + " - Todate  " + ds.Tables(0).Rows(0)("Todate")

    '                    strMsg = strMsg + " Do you want to Continue to Save the contract ? "
    '                    '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
    '                    checkseasondelete = False
    '                    Exit Function
    '                End If
    '            End If
    '        End If

    '    Next

    'End Function


    Private Function CheckMissingdates() As String

        Dim txtSeasonName As TextBox
        Dim txtSeasonfromDate As TextBox
        Dim txtSeasonToDate As TextBox

        Dim GvSeasonShowSub As GridView
        Dim lblRowId As Label

        Dim dtseasonnew As New DataTable
        Dim dsseason As New DataSet
        Dim dr As DataRow
        Dim xmlseason As String = ""

        CheckMissingdates = ""

        dtseasonnew.Columns.Add(New DataColumn("partycode", GetType(String)))
        dtseasonnew.Columns.Add(New DataColumn("season", GetType(String)))
        dtseasonnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
        dtseasonnew.Columns.Add(New DataColumn("todate", GetType(String)))



        For Each gvRow As GridViewRow In GvSeasonShow.Rows

            lblRowId = gvRow.FindControl("lblRowId")
            txtSeasonName = gvRow.FindControl("txtSeasonName")
            GvSeasonShowSub = gvRow.FindControl("GvSeasonShowSub")

            For Each gvRow1 In GvSeasonShowSub.Rows

                txtSeasonfromDate = gvRow1.FindControl("txtSeasonfromDate")
                txtSeasonToDate = gvRow1.FindControl("txtSeasonToDate")



                If GvSeasonShowSub.Rows.Count > 0 And txtSeasonfromDate.Text <> "" And txtSeasonToDate.Text <> "" Then

                    dr = dtseasonnew.NewRow
                    dr("partycode") = CType(hdnpartycode.Value, String)
                    dr("season") = CType(txtSeasonName.Text, String)
                    dr("fromdate") = Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd")
                    dr("todate") = Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd")
                    dtseasonnew.Rows.Add(dr)

                End If
            Next
        Next

        dsseason.Clear()
        If dtseasonnew IsNot Nothing Then
            If dtseasonnew.Rows.Count > 0 Then
                dsseason.Tables.Add(dtseasonnew)
                xmlseason = objUtils.GenerateXML(dsseason)
            End If
        Else
            xmlseason = "<NewDataSet />"
        End If

        Dim parammsg As SqlParameter
        Dim ErrMsg As String = ""

        'mySqlCmd = New SqlCommand
        'mySqlCmd.CommandText = "sp_checkmissingdates"
        'mySqlCmd.Connection = mySqlConn
        'mySqlCmd.Transaction = sqlTrans
        'mySqlCmd.CommandType = CommandType.StoredProcedure
        Dim strMsg As String = ""
        Dim ds As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim parm(2) As SqlParameter

        parm(0) = New SqlParameter("@seasonxml", CType(xmlseason, String))
        parm(1) = New SqlParameter("@contractfromdate", ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text))
        parm(2) = New SqlParameter("@contracttodate", ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text))

        ' parms.Add(parm(0))

        For i = 0 To 2
            parms.Add(parm(i))
        Next


        ds = New DataSet()
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkmissingdates", parms)
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
                    strMsg = "Missing Season Dates Period  " + "\n"
                    For i = 0 To ds.Tables(0).Rows.Count - 1

                        strMsg += " From Date -  " + ds.Tables(0).Rows(i)("fromdateC") + " - Todate  " + ds.Tables(0).Rows(i)("Todate") + "\n"
                    Next
                    ' strMsg = "Missing Season Dates Period  " + " From Date -  " + ds.Tables(0).Rows(0)("fromdateC") + " - Todate  " + ds.Tables(0).Rows(0)("Todate")

                    strMsg = strMsg + " Do you want to Continue to Save the contract ? "
                    '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                    CheckMissingdates = strMsg
                    Exit Function
                End If
            End If
        End If





        'CheckMissingdates = True
    End Function
    Private Function ValidateSave() As Boolean

        Dim txtSeasonName As TextBox
        Dim txtSeasonfromDate As TextBox
        Dim txtSeasonToDate As TextBox
        Dim txtMinNight As TextBox
        Dim GvSeasonShowSub As GridView
        Dim lblRowId As Label

        Dim ToDt As Date = Nothing
        Dim flgdt As Boolean = False

        If txtsupagentname.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Supplier Agent.');", True)

            ValidateSave = False
            Exit Function
        End If
        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Applicable To.');", True)

            ValidateSave = False
            Exit Function
        End If

        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from partymast where partycode='" & hdnpartycode.Value & "'") = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Hotel Not belongs to the Supplier.');", True)

            ValidateSave = False
            Exit Function
        End If

        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from supplier_agents where supagentcode='" & txtsupagentcode.Text & "'") = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Supplier Agent Not belongs to the Supplier Agent Master.');", True)

            ValidateSave = False
            Exit Function
        End If

        'If wucCountrygroup.checkcountrylist.ToString = "" Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Country.');", True)

        '    ValidateSave = False
        '    Exit Function
        'End If



        '------------Validate Commissionable 
        Dim formulacheck As Boolean = False
        Dim formula As String = ""
        Dim formulacount As Integer = 0
        If chkcommission.Checked = True Then

            For Each gvrow As GridViewRow In grdcommission.Rows

                Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
                Dim lblFormulaName As Label = gvrow.FindControl("lblFormulaName")
                Dim optcomm As RadioButton = gvrow.FindControl("optcomm")

                If optcomm.Checked = True Then
                    formulacheck = True

                    formulacount = formulacount + 1
                End If

            Next

            If formulacheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select the Commission Formula ');", True)
                ValidateSave = False
                Exit Function
            End If

            If formulacount > 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select the One Formula ');", True)
                ValidateSave = False
                Exit Function
            End If

            Dim str As String = ""
            For Each gvrow As GridViewRow In grdcommission.Rows

                Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
                Dim lblFormulaName As Label = gvrow.FindControl("lblFormula")
                Dim optcomm As RadioButton = gvrow.FindControl("optcomm")

                If optcomm.Checked = True Then
                    str = lblFormulaName.Text

                    str = str.Replace("+", " ")
                    str = str.Replace("-", " ")
                    str = str.Replace("*", " ")
                    str = str.Replace("/", " ")
                    str = str.Replace("=", " ")
                    str = str.Replace("|", " ")

                    For Each gvRow1 As GridViewRow In grdcommissiondetail.Rows

                        Dim txtperc As TextBox = gvRow1.FindControl("txtperc")
                        Dim lblterm1 As Label = gvRow1.FindControl("txtterm1")

                        If lblFormulaName.Text.Contains(" SR ") = True And lblterm1.Text.Contains("SR") = True And Val(txtperc.Text) = 0 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Formula Contains Service tax Please enter ServiceTax');", True)
                            ValidateSave = False
                            Exit Function
                        End If
                        If lblFormulaName.Text.Contains(" T ") = True And lblterm1.Text.Contains("T") = True And Val(txtperc.Text) = 0 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Formula Contains Service tax Please enter Tax');", True)
                            ValidateSave = False
                            Exit Function
                        End If

                        If lblFormulaName.Text.Contains(" C ") = True And lblterm1.Text.Contains("C") = True And Val(txtperc.Text) = 0 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Formula Contains Service tax Please enter Commission');", True)
                            ValidateSave = False
                            Exit Function
                        End If

                    Next





                End If

            Next



        End If


        '''''''''''''''''

        ''''''''''' Dates Overlapping

        Dim dtdatesnew As New DataTable
        Dim dsdates As New DataSet
        Dim dr As DataRow
        Dim xmldates As String = ""




        dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))


        For Each gvRow As GridViewRow In GvSeasonShow.Rows

            lblRowId = gvRow.FindControl("lblRowId")
            txtSeasonName = gvRow.FindControl("txtSeasonName")
            GvSeasonShowSub = gvRow.FindControl("GvSeasonShowSub")

            For Each gvRow1 In GvSeasonShowSub.Rows

                txtSeasonfromDate = gvRow1.FindControl("txtSeasonfromDate")
                txtSeasonToDate = gvRow1.FindControl("txtSeasonToDate")

                If GvSeasonShowSub.Rows.Count > 0 And txtSeasonfromDate.Text <> "" And txtSeasonToDate.Text <> "" Then

                    dr = dtdatesnew.NewRow

                    dr("fromdate") = Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd")
                    dr("todate") = Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd")
                    dtdatesnew.Rows.Add(dr)

                End If

            Next
        Next
        dsdates.Clear()
        If dtdatesnew IsNot Nothing Then
            If dtdatesnew.Rows.Count > 0 Then
                dsdates.Tables.Add(dtdatesnew)
                xmldates = objUtils.GenerateXML(dsdates)
            End If
        Else
            xmldates = "<NewDataSet />"
        End If

        Dim strMsg As String = ""
        Dim ds As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim parm(1) As SqlParameter

        parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))


        parms.Add(parm(0))


        ds = New DataSet()
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates_contract", parms)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
                    strMsg = "Dates Are Overlapping Please check " + "\n"
                    'For i = 0 To ds.Tables(0).Rows.Count - 1

                    '    strMsg += "  Date -  " + ds.Tables(0).Rows(i)("fromdateC") + "\n"
                    'Next
                    For i = 0 To ds.Tables(0).Rows.Count - 1

                        strMsg += " From Date -  " + ds.Tables(0).Rows(i)("fromdateC") + " - Todate  " + ds.Tables(0).Rows(i)("Todate") + "\n"
                    Next

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                    ValidateSave = False
                    Exit Function
                End If
            End If
        End If


        '''''''''''''''''




        '--------------------------------------------- Validate season Date Grid

        For Each gvRow As GridViewRow In GvSeasonShow.Rows

            lblRowId = gvRow.FindControl("lblRowId")
            txtSeasonName = gvRow.FindControl("txtSeasonName")
            GvSeasonShowSub = gvRow.FindControl("GvSeasonShowSub")

            For Each gvRow1 In GvSeasonShowSub.Rows

                txtSeasonfromDate = gvRow1.FindControl("txtSeasonfromDate")
                txtSeasonToDate = gvRow1.FindControl("txtSeasonToDate")



                If GvSeasonShowSub.Rows.Count > 0 And txtSeasonfromDate.Text <> "" And txtSeasonToDate.Text <> "" Then
                    If txtSeasonfromDate.Text <> "" And txtSeasonToDate.Text <> "" Then
                        'If ObjDate.ConvertDateromTextBoxToDatabase(Format(txtSeasonToDate.Text, "yyyy/MM/dd")) <= ObjDate.ConvertDateromTextBoxToDatabase(Format(txtSeasonfromDate.Text, "yyyy/MM/dd")) Then
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        '    SetFocus(txtSeasonToDate)
                        '    ValidateSave = False
                        '    Exit Function
                        'End If
                        If Left(Right(CType(txtSeasonfromDate.Text, Date), 4), 2) <> "20" Or Left(Right(CType(txtSeasonToDate.Text, Date), 4), 2) <> "20" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century ');", True)
                            ValidateSave = False
                            SetFocus(txtSeasonfromDate)
                            Exit Function
                        End If



                        If Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd") < Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                            SetFocus(txtSeasonToDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        If ToDt <> Nothing Then
                            'If ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text) <= ToDt Then
                            If Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") <= ToDt Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Season Date Overlapping.');", True)
                                SetFocus(txtSeasonfromDate)
                                ValidateSave = False
                                Exit Function
                            End If
                        End If
                        'If (ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)) Then
                        If Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Season From Date Should belongs to the Contracts Period.');", True)
                            SetFocus(txtSeasonfromDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        'If (ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonToDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)) Then
                        If (Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Season To Date Should belongs to the Contracts Period.');", True)
                            SetFocus(txtSeasonToDate)
                            ValidateSave = False
                            Exit Function
                        End If


                        'ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonToDate.Text)
                        ToDt = Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd")
                        flgdt = True

                    ElseIf txtSeasonfromDate.Text <> "" And txtSeasonToDate.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter To Date.');", True)
                        SetFocus(txtSeasonToDate)
                        ValidateSave = False
                        Exit Function
                    ElseIf txtSeasonfromDate.Text = "" And txtSeasonToDate.Text <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date.');", True)
                        SetFocus(txtSeasonfromDate)
                        ValidateSave = False
                        Exit Function
                    End If
                End If
            Next
            ToDt = Nothing
        Next




        ValidateSave = True
    End Function
    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow

        Dim strMsg As String = ""
        FindDatePeriod = True
        Try
            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist

            Dim ds As DataSet
            Dim parms3 As New List(Of SqlParameter)
            Dim parm3(7) As SqlParameter
            parm3(0) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
            parm3(1) = New SqlParameter("@fromdate", Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd"))
            parm3(2) = New SqlParameter("@todate", Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"))
            parm3(3) = New SqlParameter("@mode", CType(Session("ContractState"), String))
            parm3(4) = New SqlParameter("@tranid", CType(txtcontractid.Text, String))
            parm3(5) = New SqlParameter("@country", CType(Session("CountryList"), String))
            parm3(6) = New SqlParameter("@agent", CType(Session("AgentList"), String))

            parms3.Add(parm3(0))
            parms3.Add(parm3(1))
            parms3.Add(parm3(2))
            parms3.Add(parm3(3))
            parms3.Add(parm3(4))
            parms3.Add(parm3(5))
            parms3.Add(parm3(6))



            ds = New DataSet()
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkduplicate_contract", parms3)


            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("contractid")) = False Then
                        strMsg = "Contract already exists For this Hotel " + ds.Tables(0).Rows(0)("contractid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max accomodation already exists For this Supplier Please check this transaction   ');", True)
                        FindDatePeriod = False
                        Exit Function
                    End If
                End If
            End If


            FindDatePeriod = True

        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Private Function checkApplicable(ByVal applicable As String) As String
        ''''''''' Applicable checking
        checkApplicable = ""
        Dim strmsg1 As String = ""

        If Replace(applicable, " ", "") <> hdnapplicable.Value Then
            strmsg1 = "Do you want to update applicable to for entire contract ?"
            checkApplicable = strmsg1

        End If



        '''''''''''''

    End Function

    Private Function checkseasongrid() As String

        checkseasongrid = ""
        Dim strmsg1 As String = ""

        For Each gvrow In gvSeasonInput.Rows
            Dim txtseason As TextBox = gvrow.findcontrol("txtSeasonName")

            If txtseason.Text <> "" Then
                strmsg1 = "You have not saved the season dates Are sure want to continue save ?"
                checkseasongrid = strmsg1
            End If

        Next





        '''''''''''''

    End Function
    Private Function checkseasons(ByVal applicable As String) As String
        ''''''''' Applicable checking
        checkseasons = ""
        Dim strmsg2 As String = ""

        Dim txtSeasonName As TextBox
        Dim txtoldseasonname As TextBox

        Dim GvSeasonShowSub As GridView
        Dim lblRowId As Label

        For Each lRow As GridViewRow In GvSeasonShow.Rows

            lblRowId = lRow.FindControl("lblRowId")
            txtoldseasonname = lRow.FindControl("txtoldseasonname")
            GvSeasonShowSub = lRow.FindControl("GvSeasonShowSub")
            txtSeasonName = lRow.FindControl("txtSeasonName")

            'For Each lsubRow As GridViewRow In GvSeasonShowSub.Rows

            '    lblRowId = lsubRow.FindControl("lblRowId")
            '    txtSeasonName = lsubRow.FindControl("txtSeasonName")
            '    '  txtoldseasonname = lsubRow.FindControl("txtoldseasonname")

            If Trim(txtSeasonName.Text.ToUpper) <> Trim(txtoldseasonname.Text.ToUpper) And txtoldseasonname.Text <> "" Then
                strmsg2 = "Do you want to update Seasons to for entire contract ?"
                checkseasons = txtSeasonName.Text.ToUpper 'strmsg2
                Exit For
            End If

            ' Next
        Next
        'If Replace(applicable, " ", "") <> hdnapplicable.Value Then
        '    strmsg1 = "Do you want to update applicable to for entire contract ?"
        '    checkseasons = strmsg2

        'End If



        '''''''''''''

    End Function
    Protected Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ViewState("checkapplicable") = Nothing
        ViewState("checkseasons") = Nothing
        ViewState("checkMissing") = Nothing
        ViewState("chkseasongrid") = Nothing
    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim strMsg As String = ""
            Dim txtSeasonName As TextBox
            Dim txtSeasonfromDate As TextBox
            Dim txtSeasonToDate As TextBox
            Dim txtMinNight As TextBox
            Dim GvSeasonShowSub As GridView
            Dim lblRowId As Label
            Dim liRowId As Integer = 0
            Dim liRowIdCurr As Integer = -1
            Dim gvRow1 As GridViewRow

            If Page.IsValid = True Then
                If Session("ContractState") = "New" Or Session("ContractState") = "Edit" Or Session("ContractState") = "Copy" Then
                    If ValidateSave() = False Then
                        Exit Sub
                    End If



                    If ViewState("chkseasongrid") <> 1 Then
                        Dim strmsg4 As String = checkseasongrid()
                        If strmsg4 <> "" Then

                            ViewState("chkseasongrid") = 1
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "confirmapplicable('" + strmsg4 + "','" + btnSave.ClientID + "');", True)
                            Exit Sub

                        End If
                    End If

                    If ViewState("checkMissing") <> 1 Then
                        Dim strmsg3 As String = CheckMissingdates()

                        If strmsg3 <> "" Then

                            ViewState("checkMissing") = 1
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "confirmapplicable('" + strmsg3 + "','" + btnSave.ClientID + "');", True)
                            Exit Sub

                        End If
                    End If


                    If ViewState("checkapplicable") <> 1 And Session("ContractState") = "Edit" Then
                        Dim strmsg1 As String = checkApplicable(CType(txtApplicableTo.Text, String))
                        If strmsg1 <> "" Then
                            ViewState("checkapplicable") = 1
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "confirmapplicable('" + strmsg1 + "','" + btnSave.ClientID + "');", True)
                            Exit Sub
                        End If
                    End If


                    If ViewState("checkseasons") <> 1 And Session("ContractState") = "Edit" Then
                        Dim strmsg2 As String = checkseasons(CType(txtApplicableTo.Text, String))
                        Dim strmsg3 = "Do you want to update Seasons to for entire contract ?"
                        If strmsg2 <> "" Then
                            ViewState("checkseasons") = 1
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "confirmapplicable('" + strmsg3 + "','" + btnSave.ClientID + "');", True)
                            Exit Sub
                        End If
                    End If


                    If chkwithdraw.Checked = False Then
                        If FindDatePeriod() = False Then

                            Exit Sub
                        End If

                        If Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                            Exit Sub
                        End If
                    End If






                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("ContractState") = "New" Or Session("ContractState") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("CONTRACT", mySqlConn, sqlTrans)
                        txtcontractid.Text = optionval.Trim



                        mySqlCmd = New SqlCommand("sp_add_edit_contracts", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtsupagentcode.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroups", SqlDbType.VarChar, 5000)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 8000)).Value = CType(txtApplicableTo.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@Commissionable", SqlDbType.Int)).Value = IIf(chkcommission.Checked = True, 1, 0)

                        If txtefffromdate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectivefrom", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectivefrom", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtefffromdate.Text)
                        End If
                        If txtefftodate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectiveto", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectiveto", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtefftodate.Text)
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@activestate", SqlDbType.VarChar, 20)).Value = IIf(chkwithdraw.Checked = False, "Active", "With Drawn")
                        mySqlCmd.Parameters.Add(New SqlParameter("@withdraw", SqlDbType.Int)).Value = IIf(chkwithdraw.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@titlename", SqlDbType.VarChar, 200)).Value = CType(txttitle.Text, String)


                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()

                    ElseIf Session("ContractState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts", mySqlConn, sqlTrans)
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtsupagentcode.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroups", SqlDbType.VarChar, 5000)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 8000)).Value = CType(txtApplicableTo.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@Commissionable", SqlDbType.Int)).Value = IIf(chkcommission.Checked = True, 1, 0)
                        If txtefffromdate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectivefrom", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectivefrom", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtefffromdate.Text)
                        End If
                        If txtefftodate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectiveto", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@effectiveto", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtefftodate.Text)
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@activestate", SqlDbType.VarChar, 20)).Value = IIf(chkwithdraw.Checked = False, "Active", "With Drawn")
                        mySqlCmd.Parameters.Add(New SqlParameter("@withdraw", SqlDbType.Int)).Value = IIf(chkwithdraw.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@titlename", SqlDbType.VarChar, 200)).Value = CType(txttitle.Text, String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()

                    End If

                    '''  User cotrol country saving
                    ''' 

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_countries Where contractid='" & CType(txtcontractid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_agents Where contractid='" & CType(txtcontractid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'If wucCountrygroup.checkcountrylist.ToString <> "" Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                    '    For i = 0 To arrcountry.Length - 1

                    '        If arrcountry(i) <> "" Then




                    '            mySqlCmd = New SqlCommand("sp_add_editcontractcountries", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure

                    '            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)


                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    'End If

                    'If wucCountrygroup.checkagentlist.ToString <> "" Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For i = 0 To arragents.Length - 1

                    '        If arragents(i) <> "" Then

                    '            mySqlCmd = New SqlCommand("sp_add_editcontractagents", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure

                    '            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)


                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    'End If

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_Contracts Where contract_id='" & CType(txtcontractid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_seasons Where contractid='" & CType(txtcontractid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()


                    'Copy existing in the show grid except season name passed
                    Dim lsSeasonName As String = ""
                    Dim k As Integer
                    For Each lRow As GridViewRow In GvSeasonShow.Rows

                        lblRowId = lRow.FindControl("lblRowId")
                        txtSeasonName = lRow.FindControl("txtSeasonName")
                        GvSeasonShowSub = lRow.FindControl("GvSeasonShowSub")

                        If txtSeasonName IsNot Nothing Then

                            If CType(Val(lblRowId.Text), Integer) > liRowId Then
                                liRowId = CType(Val(lblRowId.Text), Integer)
                            End If

                            If GvSeasonShowSub IsNot Nothing Then
                                For Each lSubRow As GridViewRow In GvSeasonShowSub.Rows
                                    txtSeasonfromDate = lSubRow.FindControl("txtSeasonfromDate")
                                    txtSeasonToDate = lSubRow.FindControl("txtSeasonToDate")
                                    txtMinNight = lSubRow.FindControl("txtMinNight")

                                    ' mySqlCmd = New SqlCommand("sp_add_edit_contractsseasons", mySqlConn, sqlTrans)
                                    '  mySqlCmd.CommandType = CommandType.StoredProcedure
                                    If Session("CountryList") = Nothing Then
                                        Session("CountryList") = wucCountrygroup.checkcountrylist
                                    End If

                                    If Session("AgentList") = Nothing Then
                                        Session("AgentList") = wucCountrygroup.checkagentlist
                                    End If


                                    mySqlCmd = New SqlCommand("sp_insert_new_contracts", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandTimeout = 0

                                    mySqlCmd.CommandType = CommandType.StoredProcedure

                                    mySqlCmd.CommandTimeout = 0
                                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.Date)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.Date)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0
                                    mySqlCmd.Parameters.Add(New SqlParameter("@activestate", SqlDbType.VarChar, 20)).Value = IIf(chkwithdraw.Checked = False, "Active", "With Drawn")
                                    mySqlCmd.Parameters.Add(New SqlParameter("@withdraw", SqlDbType.Int)).Value = IIf(chkwithdraw.Checked = True, 1, 0)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@titlename", SqlDbType.VarChar, 200)).Value = CType(txttitle.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.NVarChar, 200)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@SeasonGroup_ID", SqlDbType.NVarChar, 200)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Season_Name", SqlDbType.NVarChar, 200)).Value = CType(txtSeasonName.Text.ToUpper.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Season_start_Date", SqlDbType.Date)).Value = Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") 'ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Season_End_Date", SqlDbType.Date)).Value = Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd") 'ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonToDate.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Contract_Master_ID", SqlDbType.NVarChar, 200)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Excluded_DateFlag", SqlDbType.NVarChar, 20)).Value = "None"
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Promotion_Name", SqlDbType.NVarChar, 400)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Tactical_Flag", SqlDbType.NVarChar, 20)).Value = "None"
                                    mySqlCmd.Parameters.Add(New SqlParameter("@StopSales_flag", SqlDbType.NVarChar, 20)).Value = "None"
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Agent_Country_Id", SqlDbType.NVarChar, 400)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@minnights", SqlDbType.Int)).Value = CType(Val(txtMinNight.Text), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@countrylist", SqlDbType.VarChar, 8000)).Value = CType(Session("CountryList"), String)

                                    If (Session("AgentList") = Nothing) Then

                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentlist", SqlDbType.VarChar, 8000)).Value = ""

                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentlist", SqlDbType.VarChar, 8000)).Value = CType(Session("AgentList"), String)
                                    End If

                                    ' Rosalin 2019-30-10  for edit_contract season
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = CType(liRowId, Integer)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@fromdate1", SqlDbType.DateTime)).Value = Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") 'ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@todate1", SqlDbType.DateTime)).Value = Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd")


                                    mySqlCmd.ExecuteNonQuery()
                                    mySqlCmd.Dispose() 'command disposed

                                Next
                            End If


                        End If
                    Next


                    ' ''''''' to save formula
                    'If chkcommission.Checked = True Then


                    '    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_commission Where contractid='" & CType(txtcontractid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    '    mySqlCmd.CommandType = CommandType.Text
                    '    mySqlCmd.ExecuteNonQuery()

                    '    For Each gvrow As GridViewRow In grdcommission.Rows


                    '        Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
                    '        Dim txtformulacode As Label = gvrow.FindControl("txtformulacode")
                    '        Dim optcomm As RadioButton = gvrow.FindControl("optcomm")


                    '        If optcomm.Checked = True Then
                    '            Dim i As Integer = 1
                    '            For Each gridRow As GridViewRow In grdcommissiondetail.Rows

                    '                Dim txtperc As TextBox = gridRow.FindControl("txtperc")
                    '                Dim txtterm1 As Label = gridRow.FindControl("txtterm1")

                    '                If Val(txtperc.Text) <> 0 Then
                    '                    mySqlCmd = New SqlCommand("sp_add_edit_contractscommision", mySqlConn, sqlTrans)
                    '                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text.Trim, String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@formulaid", SqlDbType.VarChar, 20)).Value = CType(txtformulacode.Text.Trim, String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@term1", SqlDbType.VarChar, 20)).Value = CType(txtterm1.Text.Trim, String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@value", SqlDbType.Decimal)).Value = CType(Val(txtperc.Text.Trim), Decimal)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = i


                    '                    mySqlCmd.ExecuteNonQuery()
                    '                    mySqlCmd.Dispose() 'command disposed
                    '                    i = i + 1
                    '                End If
                    '            Next

                    '        End If

                    '    Next

                    'End If


                    '''''''''''

                    'mySqlCmd = New SqlCommand("sp_add_editpendforapprove", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 30)).Value = "edit_contracts"
                    'mySqlCmd.Parameters.Add(New SqlParameter("@markets", SqlDbType.VarChar, 50)).Value = txtApplicableTo.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 30)).Value = CType(txtcontractid.Text.Trim, String)

                    'mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = txthotelname.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 100)).Value = hdnpartycode.Value
                    'mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@moddate ", SqlDbType.DateTime)).Value = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "yyyy/MM/dd")
                    'mySqlCmd.Parameters.Add(New SqlParameter("@pricecode", SqlDbType.VarChar, 100)).Value = ""
                    'mySqlCmd.ExecuteNonQuery()


                    ''''''' Update Applicable to all option 

                    'If checkApplicable(CType(txtApplicableTo.Text, String)) <> "" And Session("ContractState") = "Edit" Then
                    '    mySqlCmd = New SqlCommand("sp_update_applicable_contracts", mySqlConn, sqlTrans)
                    '    ' mySqlCmd = New SqlCommand("New_update_applicable_contracts", mySqlConn, sqlTrans)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text.Trim, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 8000)).Value = CType(txtApplicableTo.Text, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '    mySqlCmd.ExecuteNonQuery()
                    'End If

                    ''''''''''

                    ''''''' Update Seasons to all option 
                    Dim txtoldseasonname As TextBox
                    If ViewState("checkseasons") = 1 And Session("ContractState") = "Edit" Then
                        'If Session("ContractState") = "Edit" Then
                        For Each lRow As GridViewRow In GvSeasonShow.Rows

                            lblRowId = lRow.FindControl("lblRowId")
                            txtSeasonName = lRow.FindControl("txtSeasonName")
                            txtoldseasonname = lRow.FindControl("txtoldseasonname")

                            If Trim(txtSeasonName.Text.ToUpper.Trim) <> Trim(txtoldseasonname.Text.ToUpper.Trim) Then 'added trim - New_update_applicable_season

                                ' mySqlCmd = New SqlCommand("sp_update_applicable_season", mySqlConn, sqlTrans)
                                mySqlCmd = New SqlCommand("New_update_applicable_season", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@season", SqlDbType.VarChar, 100)).Value = CType(txtSeasonName.Text.ToUpper.Trim, String) 'added trim - 
                                mySqlCmd.Parameters.Add(New SqlParameter("@oldseasoname", SqlDbType.VarChar, 100)).Value = CType(txtoldseasonname.Text.ToUpper.Trim, String) 'added trim -
                                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                mySqlCmd.ExecuteNonQuery()

                            End If

                        Next

                    End If

                    ''''''''''
                    '' With draw need to remove from main price tables added shahul  15/04/18
                    'If chkwithdraw.Checked = True Then

                    '    mySqlCmd = New SqlCommand("sp_withdraw_contracts", mySqlConn, sqlTrans)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text.Trim, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '    mySqlCmd.ExecuteNonQuery()

                    'End If


                    strMsg = "Saved Succesfully!!"
                ElseIf Session("ContractState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_edit_contracts", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(txtcontractid.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    strMsg = "Delete  Succesfully!!"
                End If


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed

                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                Session("ContractState") = ""
                wucCountrygroup.clearsessions()


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ContractMainWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region


#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    Protected Sub imgUpdateToNextGrid_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Add to another grid
        Dim lbCheckFromInput As Boolean = True
        Dim txtSeasonName1 As TextBox
        Dim txtSeasonfromDate As TextBox
        Dim txtSeasonToDate As TextBox
        Dim txtMinNight As TextBox
        Dim GvSeasonShowSub As GridView
        Dim lblRowId As Label
        Dim txtlineno As TextBox
        Dim ToDt As Date = Nothing
        Try
            'Dim imgSclose As ImageButton = CType(sender, ImageButton)
            'Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
            'Dim lb As Label = CType(row.FindControl("txtctrycode"), Label)
            'gvSeasonInput.DeleteRow(row.RowIndex)

            For Each lRow As GridViewRow In gvSeasonInput.Rows
                txtSeasonName1 = lRow.FindControl("txtSeasonName")
                txtSeasonfromDate = lRow.FindControl("txtSeasonfromDate")
                txtSeasonToDate = lRow.FindControl("txtSeasonToDate")
                txtMinNight = lRow.FindControl("txtMinNight")
                If txtSeasonName1 IsNot Nothing Then
                    If txtSeasonfromDate.Text <> "" And txtSeasonToDate.Text <> "" Then

                        If Left(Right(txtSeasonfromDate.Text, 4), 2) <> "20" Or Left(Right(txtSeasonToDate.Text, 4), 2) <> "20" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  ');", True)
                            Exit Sub
                            Exit Sub
                        End If

                        If ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonToDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                            SetFocus(txtSeasonToDate)

                            Exit Sub
                        End If


                        'If ToDt <> Nothing Then
                        '    If ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text) <= ToDt Then
                        '        'If Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") <= ToDt Then

                        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Season Date Overlapping..');", True)
                        '        SetFocus(txtSeasonfromDate)

                        '        Exit Sub
                        '    End If
                        'End If
                        If (ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)) Or (ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)) Then
                            'If Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Season From Date Should belongs to the Contracts Period.');", True)
                            SetFocus(txtSeasonfromDate)

                            Exit Sub
                        End If

                        If (ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonToDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)) Then
                            'If (Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Season To Date Should belongs to the Contracts Period.');", True)
                            SetFocus(txtSeasonToDate)

                            Exit Sub
                        End If


                        ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonToDate.Text)



                    ElseIf txtSeasonfromDate.Text <> "" And txtSeasonToDate.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter To Date.');", True)
                        SetFocus(txtSeasonToDate)

                        Exit Sub
                    ElseIf txtSeasonfromDate.Text = "" And txtSeasonToDate.Text <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date.');", True)
                        SetFocus(txtSeasonfromDate)

                        Exit Sub
                    End If
                End If
            Next


            For Each lrow As GridViewRow In GvSeasonShow.Rows
                Dim imgScancelShow As ImageButton = lrow.FindControl("imgScancelShow")
                If imgScancelShow.Visible = True Then
                    lbCheckFromInput = False
                    Dim txtSeasonName As TextBox = lrow.FindControl("txtSeasonName")
                    Dim txtoldseasonname As TextBox = lrow.FindControl("txtoldseasonname")
                    sbGenerateSeasonShowGridColumns("ADD", txtSeasonName.Text, txtoldseasonname.Text)
                End If
            Next

            If gvSeasonInput.Rows.Count > 0 And lbCheckFromInput = True Then
                Dim txtSeasonName As TextBox = gvSeasonInput.Rows(0).FindControl("txtSeasonName")
                If txtSeasonName IsNot Nothing Then
                    sbGenerateSeasonShowGridColumns("ADD", txtSeasonName.Text, "")
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim imgSclose As ImageButton = CType(sender, ImageButton)
            Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
            'Dim lb As Label = CType(row.FindControl("txtctrycode"), Label)
            'gvSeasonInput.DeleteRow(row.RowIndex)

            sbGenerateSeasonGridColumns("DELETE", row.RowIndex)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnAddRowGvS_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            sbGenerateSeasonGridColumns("ADD", 0)
            Dim txtSeasonfromDate As TextBox
            txtSeasonfromDate = TryCast(gvSeasonInput.Rows(gvSeasonInput.Rows.Count - 1).FindControl("txtSeasonfromDate"), TextBox)
            txtSeasonfromDate.Focus()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvSeasonInput_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSeasonInput.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dtrow As DataRow = objUtils.fnGridViewRowToDataRow(CType(e.Row, GridViewRow))

                'onchange="fillSeasontodate(this);"
                Dim txtSeasonName As TextBox
                Dim txtSeasonfromDate As TextBox
                Dim txtSeasonToDate As TextBox
                Dim txtMinNight As TextBox

                txtSeasonName = e.Row.FindControl("txtSeasonName")
                txtSeasonfromDate = e.Row.FindControl("txtSeasonfromDate")
                txtSeasonToDate = e.Row.FindControl("txtSeasonToDate")
                txtMinNight = e.Row.FindControl("txtMinNight")

                If txtSeasonName IsNot Nothing Then
                    txtSeasonfromDate.Attributes.Add("onchange", "fillSeasontodate('" & txtSeasonfromDate.ClientID & "','" & txtSeasonToDate.ClientID & "')")
                    txtSeasonToDate.Attributes.Add("onchange", "ValidateSeasonChkInDate('" & txtSeasonfromDate.ClientID & "','" & txtSeasonToDate.ClientID & "')")
                End If

                txtSeasonName.Text = dtrow("SeasonName")
                txtSeasonfromDate.Text = dtrow("FromDate")
                txtSeasonToDate.Text = dtrow("ToDate")
                txtMinNight.Text = dtrow("MinNight")

                If e.Row.RowIndex > 0 Then
                    txtSeasonName.Style.Add("display", "none")
                    'txtSeasonName.Enabled = False
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    'Protected Sub gvSeasonInput_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvSeasonInput.RowDeleting
    '    Try
    '        'Dim index As Integer = Convert.ToInt32(e.RowIndex)
    '        'Dim dt As DataTable = TryCast(ViewState("dt"), DataTable)
    '        'dt.Rows(index).Delete()
    '        'ViewState("dt") = dt
    '        'BindGrid()
    '        sbGenerateSeasonGridColumns("DELETE", e.RowIndex)
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try
    'End Sub

    Sub sbGenerateSeasonGridColumns(ByVal lsMode As String, ByVal liRowIndex As Integer)
        Dim dtSeason As New DataTable
        Dim drSeason As DataRow
        dtSeason.Columns.Add(New DataColumn("SeasonName", GetType(String)))
        dtSeason.Columns.Add(New DataColumn("FromDate", GetType(Date)))
        dtSeason.Columns.Add(New DataColumn("ToDate", GetType(Date)))
        dtSeason.Columns.Add(New DataColumn("MinNight", GetType(String)))


        Dim txtSeasonName As TextBox
        Dim txtSeasonfromDate As TextBox
        Dim txtSeasonToDate As TextBox
        Dim txtMinNight As TextBox
        Dim txtlineno As TextBox

        Dim lsSeasonName As String = ""

        Dim lGridViewToProcess As GridView
        If lsMode.Trim.ToUpper = "SHOWEDIT" Then
            lGridViewToProcess = GvSeasonShow.Rows(liRowIndex).FindControl("GvSeasonShowSub")
        Else
            lGridViewToProcess = gvSeasonInput
        End If

        If lsMode.Trim.ToUpper <> "BLANK" Then
            For Each lRow As GridViewRow In lGridViewToProcess.Rows
                txtSeasonName = lRow.FindControl("txtSeasonName")
                txtSeasonfromDate = lRow.FindControl("txtSeasonfromDate")
                txtSeasonToDate = lRow.FindControl("txtSeasonToDate")
                txtMinNight = lRow.FindControl("txtMinNight")

                If txtSeasonName IsNot Nothing Then
                    If lRow.RowIndex = 0 Then
                        lsSeasonName = txtSeasonName.Text
                    End If

                    drSeason = dtSeason.NewRow()
                    drSeason("SeasonName") = lsSeasonName
                    drSeason("FromDate") = txtSeasonfromDate.Text
                    drSeason("ToDate") = txtSeasonToDate.Text
                    drSeason("MinNight") = txtMinNight.Text

                    dtSeason.Rows.Add(drSeason)
                End If
            Next
        End If

        If lsMode.Trim.ToUpper = "DELETE" Then
            dtSeason.Rows(liRowIndex).Delete()
        End If
        If lsMode.Trim.ToUpper = "ADD" Or dtSeason.Rows.Count = 0 Then
            drSeason = dtSeason.NewRow()
            drSeason("SeasonName") = lsSeasonName
            drSeason("FromDate") = Now.Date
            drSeason("ToDate") = Now.Date
            drSeason("MinNight") = "1"

            dtSeason.Rows.Add(drSeason)
        End If
        gvSeasonInput.DataSource = dtSeason
        gvSeasonInput.DataBind()
        'Session("gvSeasonInput_datatable") = dtSeason
    End Sub

    Protected Sub imgSeditShow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim imgSeditshow As ImageButton = CType(sender, ImageButton)
            Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
            Dim imgScancelShow As ImageButton = row.FindControl("imgScancelShow")
            Dim imgScloseShow As ImageButton = row.FindControl("imgScloseShow")
            imgSeditshow.Visible = False
            imgScloseShow.Visible = False
            imgScancelShow.Visible = True
            sbGenerateSeasonGridColumns("SHOWEDIT", row.RowIndex)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub imgScancelShow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim imgScancelShow As ImageButton = CType(sender, ImageButton)
            Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
            Dim imgSeditshow As ImageButton = row.FindControl("imgSeditShow")
            Dim imgScloseShow As ImageButton = row.FindControl("imgScloseShow")
            imgSeditshow.Visible = True
            imgScloseShow.Visible = True
            imgScancelShow.Visible = False
            sbGenerateSeasonGridColumns("BLANK", 0)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub imgScloseShow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim imgScloseShow As ImageButton = CType(sender, ImageButton)
            Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
            Dim txtSeasonName As TextBox = row.FindControl("txtSeasonName")

            Dim ds As DataSet
            Dim parms2 As New List(Of SqlParameter)
            Dim parm2(3) As SqlParameter
            Dim strMsg As String = ""

            If Session("ContractState") = "Edit" Then

                parm2(0) = New SqlParameter("@season", CType(txtSeasonName.Text, String))
                parm2(1) = New SqlParameter("@contractid", CType(txtcontractid.Text, String))
                parm2(2) = New SqlParameter("@approved", IIf(lblstatus.Text.ToUpper = "APPROVED", 1, 0))
                For i = 0 To 2
                    parms2.Add(parm2(i))
                Next
                ds = New DataSet()
                ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkseason_delete", parms2)

                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("contractid")) = False Then
                            strMsg = "We can not Remove this Season Its Already Entered in the below Options " + "\n"
                            For i = 0 To ds.Tables(0).Rows.Count - 1

                                strMsg += ds.Tables(0).Rows(i)("Pagetype") + " - Tranid -  " + ds.Tables(0).Rows(i)("tranid") + "\n"
                            Next
                            strMsg = strMsg + "\n" + "Please Remove From the Above Options then it will allow to Remove  "

                            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)

                            Exit Sub
                        End If
                    Else
                        sbGenerateSeasonShowGridColumns("DELETE", txtSeasonName.Text, "")
                    End If
                Else
                    sbGenerateSeasonShowGridColumns("DELETE", txtSeasonName.Text, "")
                End If
            Else
                sbGenerateSeasonShowGridColumns("DELETE", txtSeasonName.Text, "")
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Sub sbGenerateSeasonShowGridColumns(ByVal lsMode As String, ByVal lsSeasonNameToCopy As String, ByVal oldseasonname As String)
        Dim dtSeason As New DataTable
        Dim drSeason As DataRow
        Dim liRowId As Integer = 0
        Dim liRowIdCurr As Integer = -1
        dtSeason.Columns.Add(New DataColumn("RowId", GetType(Integer)))
        dtSeason.Columns.Add(New DataColumn("SeasonName", GetType(String)))
        dtSeason.Columns.Add(New DataColumn("FromDate", GetType(Date)))
        dtSeason.Columns.Add(New DataColumn("ToDate", GetType(Date)))
        dtSeason.Columns.Add(New DataColumn("MinNight", GetType(String)))
        dtSeason.Columns.Add(New DataColumn("oldSeasonName", GetType(String)))


        Dim txtSeasonName As TextBox
        Dim txtSeasonfromDate As TextBox
        Dim txtSeasonToDate As TextBox
        Dim txtMinNight As TextBox
        Dim GvSeasonShowSub As GridView
        Dim lblRowId As Label
        Dim txtlineno As TextBox
        Dim txtoldseasonname As TextBox

        'Validation for Add Mode
        If lsMode.Trim.ToUpper = "ADD" Then

        End If

        'Copy existing in the show grid except season name passed
        Dim lsSeasonName As String = ""
        Dim i As Integer = 1
        For Each lRow As GridViewRow In GvSeasonShow.Rows

            lblRowId = lRow.FindControl("lblRowId")
            txtSeasonName = lRow.FindControl("txtSeasonName")
            GvSeasonShowSub = lRow.FindControl("GvSeasonShowSub")
            txtoldseasonname = lRow.FindControl("txtoldseasonname")

            If txtSeasonName IsNot Nothing Then

                If CType(Val(lblRowId.Text), Integer) > liRowId Then
                    liRowId = CType(Val(lblRowId.Text), Integer)
                End If

                If lsSeasonNameToCopy.Trim.ToUpper = txtSeasonName.Text.Trim.ToUpper Then
                    liRowIdCurr = CType(Val(lblRowId.Text), Integer)
                Else
                    If GvSeasonShowSub IsNot Nothing Then
                        For Each lSubRow As GridViewRow In GvSeasonShowSub.Rows
                            txtSeasonfromDate = lSubRow.FindControl("txtSeasonfromDate")
                            txtSeasonToDate = lSubRow.FindControl("txtSeasonToDate")
                            txtMinNight = lSubRow.FindControl("txtMinNight")

                            If txtSeasonfromDate IsNot Nothing Then
                                drSeason = dtSeason.NewRow()
                                drSeason("RowId") = lblRowId.Text
                                drSeason("SeasonName") = txtSeasonName.Text
                                drSeason("FromDate") = txtSeasonfromDate.Text
                                drSeason("ToDate") = txtSeasonToDate.Text
                                drSeason("MinNight") = txtMinNight.Text
                                drSeason("oldSeasonName") = txtoldseasonname.Text.ToUpper
                                dtSeason.Rows.Add(drSeason)

                            End If
                        Next
                    End If
                End If
            End If
        Next

        If lsMode.Trim.ToUpper = "ADD" Then
            For Each lRow As GridViewRow In gvSeasonInput.Rows
                txtSeasonName = lRow.FindControl("txtSeasonName")
                txtSeasonfromDate = lRow.FindControl("txtSeasonfromDate")
                txtSeasonToDate = lRow.FindControl("txtSeasonToDate")
                txtMinNight = lRow.FindControl("txtMinNight")
                If txtSeasonName IsNot Nothing Then
                    If lRow.RowIndex = 0 Then
                        lsSeasonName = txtSeasonName.Text
                    End If
                    drSeason = dtSeason.NewRow()
                    drSeason("RowId") = IIf(liRowIdCurr = -1, liRowId + 1, liRowIdCurr)
                    drSeason("SeasonName") = lsSeasonName
                    drSeason("FromDate") = txtSeasonfromDate.Text
                    drSeason("ToDate") = txtSeasonToDate.Text
                    drSeason("MinNight") = txtMinNight.Text
                    drSeason("oldSeasonName") = oldseasonname.ToUpper

                    dtSeason.Rows.Add(drSeason)
                End If
            Next
        End If

        Dim dtview As New DataView(dtSeason)
        dtview.Sort = "fromdate ASC"


        dtSeason = dtview.ToTable()

        Session("gvSeasonShow_datatable") = dtSeason

        Dim dvSeasonMain As DataView, dtSeasonMain As DataTable
        dvSeasonMain = New DataView(dtSeason)
        dvSeasonMain.RowFilter = ""
        dtSeasonMain = dvSeasonMain.ToTable(True, {"SeasonName", "RowId", "oldSeasonName"})
        GvSeasonShow.DataSource = dtSeasonMain
        GvSeasonShow.DataBind()

        'Make input grid blank 
        sbGenerateSeasonGridColumns("BLANK", 0)
    End Sub

    Protected Sub GvSeasonShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvSeasonShow.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dtrow As DataRow = objUtils.fnGridViewRowToDataRow(CType(e.Row, GridViewRow))

                Dim txtSeasonName As TextBox = e.Row.FindControl("txtSeasonName")

                Dim GvSeasonShowSub As GridView = e.Row.FindControl("GvSeasonShowSub")
                Dim dtSeaSub As DataTable = CType(Session("gvSeasonShow_datatable"), DataTable)
                Dim dvSeaSub As DataView
                dvSeaSub = New DataView(dtSeaSub)
                dvSeaSub.RowFilter = "SeasonName='" & txtSeasonName.Text & "'"
                GvSeasonShowSub.DataSource = dvSeaSub.ToTable
                GvSeasonShowSub.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("ContractState") = Nothing
        Session("ContractRefCode") = Nothing
        Session("contractid") = Nothing
        Session("State") = Nothing
        Session("Contractparty") = Nothing

        wucCountrygroup.clearsessions()
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ContractMainWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
    Protected Sub optcomm_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim optcomm As RadioButton = CType(sender, RadioButton)

        Dim row As GridViewRow = CType((CType(sender, RadioButton)).NamingContainer, GridViewRow)
        Dim txtformula As Label = row.FindControl("txtformulacode")


        Dim myDS As New DataSet
        Dim strsql As String = ""
        Dim formulaid As String = ""
        Dim contractid As String = ""

        If CType(Session("ContractState"), String) = "New" Then

            grdcommissiondetail.Visible = True
            myDS.Clear()
            strsql = "SELECT  d.term1,t.termname,'' value, t.rankorder from commissionformula_detail d inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.formulaid='" & txtformula.Text & "' union all  " _
                     & " select d.term2,t.termname,'' value ,t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & txtformula.Text & "' order by 4"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        Else
            grdcommissiondetail.Visible = True
            myDS.Clear()
            strsql = "SELECT  d.term1,t.termname,d.value,t.rankorder   from view_contractcommission d  inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.contractid='" & txtcontractid.Text & "' union all " _
                     & " select d.term2,t.termname ,0,t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & txtformula.Text & "' and d.term2 not in (select term1 from view_contractcommission where contractid='" & txtcontractid.Text & "') order by 4"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        End If
        Enabletax()


        'sbAgentSelectAndSortAndAssign(sender, IIf(chk2_agent.Checked = True, 1, 0), "")
        '

    End Sub

    Protected Sub grdcommission_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdcommission.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim formula As String = ""
                Dim chkcomm As CheckBox
                Dim SR As String = ""
                Dim T As String = ""
                Dim lblformula As Label
                Dim optcomm As RadioButton


                chkcomm = e.Row.FindControl("chkcomm")
                lblformula = e.Row.FindControl("lblFormula")
                optcomm = e.Row.FindControl("optcomm")

                chkcomm.Attributes.Add("onChange", "fillgrid('" & optcomm.ClientID & "')")

                chkcomm.Attributes.Add("onChange", "Enabletax('" & chkcomm.ClientID & "','" & lblformula.Text & "','" + txtservicerate.ClientID + "','" + txttaxperc.ClientID + "','" + txtcommperc.ClientID + "')")





            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub grdcommissiondetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdcommissiondetail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtperc As TextBox = e.Row.FindControl("txtperc")

                Numberssrvctrl(txtperc)


            End If

        Catch ex As Exception

        End Try
    End Sub

    'Private Function wucCountrygroup() As Object
    '    Throw New NotImplementedException
    'End Function

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSave.Command

    End Sub
End Class
