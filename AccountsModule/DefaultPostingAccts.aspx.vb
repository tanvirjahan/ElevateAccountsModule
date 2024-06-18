Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Linq
Imports System.IO

Partial Class DefaultPostingAccts
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUser As New clsUser
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Shared HotelCode As String
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
            objUtils.WritErrorLog("StopSale.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("StopSale.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                If lsCountryList <> "" Then
                    'strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
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

#Region "Web Services Methods"

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAccounts(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim bstrSqlQry As String = ""
        Dim myDS As New DataSet
        Dim AcctsName As New List(Of String)
        Dim splitContext() As String = contextKey.Split("|")
        Dim divCode As String = ""
        Dim acctOrder As String = ""
        If splitContext.Count = 2 Then
            divCode = splitContext(0)
            acctOrder = splitContext(1)
        End If
        Try
            bstrSqlQry = "select acctcode,acctname from view_acctmast(nolock) where div_code='" & divCode & "' and acctorder=" & acctOrder & " and  acctname like  '" & Trim(prefixText) & "%'  order by acctname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            myDataAdapter = New SqlDataAdapter(bstrSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    AcctsName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                Next
            End If
            Return AcctsName
        Catch ex As Exception
            Return AcctsName
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetControlAccounts(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim bstrSqlQry As String = ""
        Dim myDS As New DataSet
        Dim AcctsName As New List(Of String)
        Try
            bstrSqlQry = "select acctcode,acctname from acctmast(nolock) where controlyn='Y' and div_code='" & contextKey & "' and  acctname like  '" & Trim(prefixText) & "%'  order by acctname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            myDataAdapter = New SqlDataAdapter(bstrSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    AcctsName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                Next
            End If
            Return AcctsName
        Catch ex As Exception
            Return AcctsName
        End Try
    End Function

#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                txtDivCode.Text = Request.QueryString("divid")
                'acctorder
                txtIncome.Text = 3
                txtVatPayable.Text = 1 '2 
                txtCostSale.Text = 4 '6 
                txtProvVatCredit.Text = 2 '1 '2  'changed by mohamed on 26/05/2021
                txtVatCredit.Text = 2 '1 '2 'changed by mohamed on 26/05/2021
                txtSalesDiff.Text = 4 '6 

                If Convert.ToString(Request.QueryString("State")) = "View" Then
                    Session("DefaultPostingAcctState") = "View"
                    Page.Title = "View Default Posting Accounts"
                    lblHeading.Text = "View Default Posting Accounts"
                ElseIf Convert.ToString(Request.QueryString("State")) = "Edit" Then
                    Session("DefaultPostingAcctState") = "Edit"
                    Page.Title = "Edit Default Posting Accounts"
                    lblHeading.Text = "Edit Default Posting Accounts"
                Else
                    Session("DefaultPostingAcctState") = "New"
                    Page.Title = "New Default Posting Accounts"
                    lblHeading.Text = "New Default Posting Accounts"
                End If

                Session("DsClassify") = Nothing
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                HFshowctry_agent.Value = CType(objUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id=5507"), String)
                If HFshowctry_agent.Value = "N" Then
                    iframeINF.Style.Add("display", "none")
                End If
                If Convert.ToString(Session("DefaultPostingAcctState")) = "Edit" Then
                    Dim postingId As String
                    postingId = CType(Request.QueryString("ID"), String)
                    FillEntry(postingId)

                ElseIf Convert.ToString(Session("DefaultPostingAcctState")) = "View" Then
                    Dim postingId As String
                    postingId = CType(Request.QueryString("ID"), String)
                    FillEntry(postingId)
                    DisableControl()
                Else
                    wucCountrygroup.sbSetPageState("", "DefaultPostingAccts", CType(Session("DefaultPostingAcctState"), String))
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    Dim dtService As DataTable = GenerateGridColumns("BeLoad")
                    gvServices.DataSource = dtService
                    gvServices.DataBind()
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DefaultPostingAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Protected Sub FillEntry(ByVal postingId As String)"
    Protected Sub FillEntry(ByVal postingId As String)
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        txtPostingId.Text = postingId
        Try
            strSqlQry = "select distinct applicableTo from InvoicePostingAccounts where divCode='" + txtDivCode.Text.Trim + "' and postingId='" + txtPostingId.Text.Trim + "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCmd = New SqlCommand(strSqlQry, mySqlConn)
            myReader = myCmd.ExecuteReader()
            If myReader.Read() Then
                If Not IsDBNull(myReader("applicableTo")) Then
                    txtApplicableTo.Text = CType(myReader("applicableTo"), String)
                Else
                    txtApplicableTo.Text = ""
                End If
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)

            wucCountrygroup.sbSetPageState(postingId, "DefaultPostingAccts", CType(Session("DefaultPostingAcctState"), String))
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()

            Dim dtService As DataTable = GenerateGridColumns("Select")
            gvServices.DataSource = dtService
            gvServices.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
#End Region

#Region "Protected Sub DisableControl()"
    Protected Sub DisableControl()
        If CType(Session("DefaultPostingAcctState"), String) = "View" Then
            txtPostingId.Enabled = False
            txtApplicableTo.Enabled = False
            btnvsprocess.Enabled = False
            dlList.Enabled = False
            wucCountrygroup.Disable(False)
            Dim txt As TextBox
            txt = TryCast(wucCountrygroup.FindControl("txtSearchAgent"), TextBox)
            txt.Enabled = False
            gvServices.Enabled = False
            btnSave.Visible = False
        ElseIf CType(Session("DefaultPostingAcctState"), String) = "Edit" Then
            txtPostingId.Enabled = False
        End If
    End Sub
#End Region

#Region "Protected Function GenerateGridColumns(ByVal lsMode As String, Optional ByVal selectDt As DataTable = Nothing) As DataTable"
    Protected Function GenerateGridColumns(ByVal lsMode As String, Optional ByVal selectDt As DataTable = Nothing) As DataTable

        Dim dtService As New DataTable
        dtService.Columns.Add(New DataColumn("ServiceId", GetType(String)))
        dtService.Columns.Add(New DataColumn("ServiceName", GetType(String)))
        dtService.Columns.Add(New DataColumn("IncomeAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("IncomeAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatPayableAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatPayableAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvVatCrAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvVatCrAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatCrAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatCrAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesDiffAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesDiffAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvCrCtrlAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvCrCtrlAcctCode", GetType(String)))
        Dim dc As DataColumn = New DataColumn("Classification", GetType(Boolean))
        dc.DefaultValue = False
        dtService.Columns.Add(dc)

        If lsMode = "BeLoad" Then
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("select sNo as ServiceId, ServiceCategory as ServiceName from servicecat order by SNo", mySqlConn)
            mySqlCmd.CommandType = CommandType.Text
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                myDataAdapter.Fill(dtService)
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        ElseIf lsMode = "Select" Then
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("sel_InvoicePostingAccounts", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@postingId", SqlDbType.VarChar, 20)).Value = txtPostingId.Text.Trim
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                myDataAdapter.Fill(dtService)
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Else
            Dim lblServiceId As New Label
            Dim lblServiceName As New Label
            Dim txtIncomeAcct As New TextBox
            Dim txtIncomeAcctCode As New TextBox
            Dim txtVatPayableAcct As New TextBox
            Dim txtVatPayableAcctCode As New TextBox
            Dim txtcostsalesAcct As New TextBox
            Dim txtcostsalesAcctCode As New TextBox
            Dim txtProvVatCrAcct As New TextBox
            Dim txtProvVatCrAcctCode As New TextBox
            Dim txtVatCrAcct As New TextBox
            Dim txtVatCrAcctCode As New TextBox
            Dim txtCostSalesDiffAcct As New TextBox
            Dim txtCostSalesDiffAcctCode As New TextBox
            Dim txtProvCrCtrlAcct As New TextBox
            Dim txtProvCrCtrlAcctCode As New TextBox
            Dim chkClassify As New CheckBox

            For Each gvr As GridViewRow In gvServices.Rows
                Dim dr As DataRow = dtService.NewRow
                dr("ServiceId") = CType(gvr.FindControl("lblServiceId"), Label).Text.Trim
                dr("ServiceName") = CType(gvr.FindControl("lblServiceName"), Label).Text.Trim
                dr("IncomeAcct") = CType(gvr.FindControl("txtIncomeAcct"), TextBox).Text.Trim
                dr("IncomeAcctCode") = CType(gvr.FindControl("txtIncomeAcctCode"), TextBox).Text.Trim
                dr("VatPayableAcct") = CType(gvr.FindControl("txtVatPayableAcct"), TextBox).Text.Trim
                dr("VatPayableAcctCode") = CType(gvr.FindControl("txtVatPayableAcctCode"), TextBox).Text.Trim
                dr("CostSalesAcct") = CType(gvr.FindControl("txtCostSalesAcct"), TextBox).Text.Trim
                dr("CostSalesAcctCode") = CType(gvr.FindControl("txtCostSalesAcctCode"), TextBox).Text.Trim
                dr("ProvVatCrAcct") = CType(gvr.FindControl("txtProvVatCrAcct"), TextBox).Text.Trim
                dr("ProvVatCrAcctCode") = CType(gvr.FindControl("txtProvVatCrAcctCode"), TextBox).Text.Trim
                dr("VatCrAcct") = CType(gvr.FindControl("txtVatCrAcct"), TextBox).Text.Trim
                dr("VatCrAcctCode") = CType(gvr.FindControl("txtVatCrAcctCode"), TextBox).Text.Trim
                dr("CostSalesDiffAcct") = CType(gvr.FindControl("txtCostSalesDiffAcct"), TextBox).Text.Trim
                dr("CostSalesDiffAcctCode") = CType(gvr.FindControl("txtCostSalesDiffAcctCode"), TextBox).Text.Trim
                dr("ProvCrCtrlAcct") = CType(gvr.FindControl("txtProvCrCtrlAcct"), TextBox).Text.Trim
                dr("ProvCrCtrlAcctCode") = CType(gvr.FindControl("txtProvCrCtrlAcctCode"), TextBox).Text.Trim
                dr("Classification") = CType(gvr.FindControl("chkClassify"), CheckBox).Checked
                dtService.Rows.Add(dr)
            Next
        End If

        'Dim findDr = (From n In dtService.AsEnumerable Where n.Field(Of String)("ServiceName") = "Hotel Rooms" Select n)
        'If findDr.Count > 0 Then
        '    Dim cDr As DataRow = findDr(0)
        '    cDr("Classification") = True
        '    dtService.AcceptChanges()
        'End If

        GenerateGridColumns = dtService

    End Function
#End Region

#Region "Protected Function BindClassification(ByVal serviceName As String, ByVal ServiceId As String, ByVal lsMode As String, Optional ByVal gvClassify As GridView = Nothing) As DataTable"
    Protected Function BindClassification(ByVal serviceName As String, ByVal ServiceId As String, ByVal lsMode As String, Optional ByVal gvClassify As GridView = Nothing) As DataTable

        Dim dtService As New DataTable
        dtService.Columns.Add(New DataColumn("ServiceId", GetType(String)))
        dtService.Columns.Add(New DataColumn("ClassCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("ClassName", GetType(String)))
        dtService.Columns.Add(New DataColumn("IncomeAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("IncomeAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatPayableAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatPayableAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvVatCrAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvVatCrAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatCrAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("VatCrAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesDiffAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("CostSalesDiffAcctCode", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvCrCtrlAcct", GetType(String)))
        dtService.Columns.Add(New DataColumn("ProvCrCtrlAcctCode", GetType(String)))

        If lsMode = "BeLoad" Then
            Dim strsql As String = ""
            If serviceName = "Tours" Then
                strsql = "select classificationcode as classCode,classificationname  as className from excclassification_header where active=1 order by classificationcode"
            ElseIf serviceName = "Transfers" Then
                strsql = "select othcatcode as classcode,othcatname as className from othcatmast where othgrpcode in (Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') and active=1 order by othcatcode"
            ElseIf serviceName = "Airport MA" Then
                strsql = "select othtypcode as classCode,othtypname as className from othtypmast where othgrpcode='AIRPORTMA' and active=1 order by othtypcode"
            ElseIf serviceName = "Visa" Then
                strsql = "select othtypcode as classCode,othtypname as className from othtypmast where othgrpcode='VISA' order by othtypcode"
            ElseIf serviceName = "Other Services" Then
                strsql = "select othgrpcode as classCode,othgrpname as ClassName from view_otherservices where active=1 order by othgrpcode"
            End If
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand(strsql, mySqlConn)
            mySqlCmd.CommandType = CommandType.Text
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                myDataAdapter.Fill(dtService)
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        ElseIf lsMode = "Select" Then
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("sel_InvoicePostingAcctClassification", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@postingId", SqlDbType.VarChar, 20)).Value = txtPostingId.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@serviceId", SqlDbType.VarChar, 20)).Value = ServiceId
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                myDataAdapter.Fill(dtService)
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Else
            For Each gvr As GridViewRow In gvClassify.Rows
                Dim dr As DataRow = dtService.NewRow
                dr("ServiceId") = ServiceId.Trim
                dr("ClassCode") = CType(gvr.FindControl("lblClassCode"), Label).Text.Trim
                dr("ClassName") = CType(gvr.FindControl("lblClassName"), Label).Text.Trim
                dr("IncomeAcct") = CType(gvr.FindControl("txtIncomeAcct"), TextBox).Text.Trim
                dr("IncomeAcctCode") = CType(gvr.FindControl("txtIncomeAcctCode"), TextBox).Text.Trim
                dr("VatPayableAcct") = CType(gvr.FindControl("txtVatPayableAcct"), TextBox).Text.Trim
                dr("VatPayableAcctCode") = CType(gvr.FindControl("txtVatPayableAcctCode"), TextBox).Text.Trim
                dr("CostSalesAcct") = CType(gvr.FindControl("txtCostSalesAcct"), TextBox).Text.Trim
                dr("CostSalesAcctCode") = CType(gvr.FindControl("txtCostSalesAcctCode"), TextBox).Text.Trim
                dr("ProvVatCrAcct") = CType(gvr.FindControl("txtProvVatCrAcct"), TextBox).Text.Trim
                dr("ProvVatCrAcctCode") = CType(gvr.FindControl("txtProvVatCrAcctCode"), TextBox).Text.Trim
                dr("VatCrAcct") = CType(gvr.FindControl("txtVatCrAcct"), TextBox).Text.Trim
                dr("VatCrAcctCode") = CType(gvr.FindControl("txtVatCrAcctCode"), TextBox).Text.Trim
                dr("CostSalesDiffAcct") = CType(gvr.FindControl("txtCostSalesDiffAcct"), TextBox).Text.Trim
                dr("CostSalesDiffAcctCode") = CType(gvr.FindControl("txtCostSalesDiffAcctCode"), TextBox).Text.Trim
                dr("ProvCrCtrlAcct") = CType(gvr.FindControl("txtProvCrCtrlAcct"), TextBox).Text.Trim
                dr("ProvCrCtrlAcctCode") = CType(gvr.FindControl("txtProvCrCtrlAcctCode"), TextBox).Text.Trim
                dtService.Rows.Add(dr)
            Next
        End If

        BindClassification = dtService

    End Function
#End Region

#Region "Protected Sub gvServices_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvServices.RowDataBound"
    Protected Sub gvServices_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvServices.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblserviceName As Label = CType(e.Row.FindControl("lblServiceName"), Label)
            Dim chk As CheckBox = CType(e.Row.FindControl("chkClassify"), CheckBox)
            Dim classGrid As GridView = CType(e.Row.FindControl("gvClass"), GridView)
            Dim trClass As HtmlTableRow = CType(e.Row.FindControl("trClass"), HtmlTableRow)
            Dim btnFill As Button = CType(e.Row.FindControl("btnFill"), Button)
            Dim txtIncomeAcct As TextBox = CType(e.Row.FindControl("txtIncomeAcct"), TextBox)
            Dim txtIncomeAcctCode As TextBox = CType(e.Row.FindControl("txtIncomeAcctCode"), TextBox)
            txtIncomeAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtIncomeAcctCode.ClientID + "')")
            Dim txtVatPayableAcct As TextBox = CType(e.Row.FindControl("txtVatPayableAcct"), TextBox)
            Dim txtVatPayableAcctCode As TextBox = CType(e.Row.FindControl("txtVatPayableAcctCode"), TextBox)
            txtVatPayableAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtVatPayableAcctCode.ClientID + "')")
            Dim txtcostsalesAcct As TextBox = CType(e.Row.FindControl("txtcostsalesAcct"), TextBox)
            Dim txtcostsalesAcctCode As TextBox = CType(e.Row.FindControl("txtcostsalesAcctCode"), TextBox)
            txtcostsalesAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtcostsalesAcctCode.ClientID + "')")
            Dim txtProvVatCrAcct As TextBox = CType(e.Row.FindControl("txtProvVatCrAcct"), TextBox)
            Dim txtProvVatCrAcctCode As TextBox = CType(e.Row.FindControl("txtProvVatCrAcctCode"), TextBox)
            txtProvVatCrAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtProvVatCrAcctCode.ClientID + "')")
            Dim txtVatCrAcct As TextBox = CType(e.Row.FindControl("txtVatCrAcct"), TextBox)
            Dim txtVatCrAcctCode As TextBox = CType(e.Row.FindControl("txtVatCrAcctCode"), TextBox)
            txtVatCrAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtVatCrAcctCode.ClientID + "')")
            Dim txtCostSalesDiffAcct As TextBox = CType(e.Row.FindControl("txtCostSalesDiffAcct"), TextBox)
            Dim txtCostSalesDiffAcctCode As TextBox = CType(e.Row.FindControl("txtCostSalesDiffAcctCode"), TextBox)
            txtCostSalesDiffAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtCostSalesDiffAcctCode.ClientID + "')")
            Dim txtProvCrCtrlAcct As TextBox = CType(e.Row.FindControl("txtProvCrCtrlAcct"), TextBox)
            Dim txtProvCrCtrlAcctCode As TextBox = CType(e.Row.FindControl("txtProvCrCtrlAcctCode"), TextBox)
            txtProvCrCtrlAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtProvCrCtrlAcctCode.ClientID + "')")
            If lblserviceName.Text.Trim = "Hotel Rooms" Then
                chk.Checked = False
                chk.Enabled = False
                classGrid.DataSource = Nothing
                classGrid.DataBind()
                trClass.Visible = False
                Dim panClass As Panel = CType(e.Row.FindControl("panClass"), Panel)
                panClass.Visible = False
                btnFill.Enabled = False
                btnFill.BackColor = Gray
            Else
                If chk.Checked = True Then
                    trClass.Visible = True
                    Dim serviceId As Label = CType(e.Row.FindControl("lblServiceId"), Label)
                    Dim ds = CType(Session("DsClassify"), DataSet)
                    Dim dt As New DataTable
                    If Not ds Is Nothing Then
                        If ds.Tables.Contains(lblserviceName.Text.Trim) Then
                            dt = ds.Tables(lblserviceName.Text.Trim)
                        Else
                            If Convert.ToString(Session("DefaultPostingAcctState")) = "New" Then
                                dt = BindClassification(lblserviceName.Text.Trim, serviceId.Text.Trim, "BeLoad")
                            Else
                                dt = BindClassification(lblserviceName.Text.Trim, serviceId.Text.Trim, "Select")
                            End If
                        End If
                    Else
                        If Convert.ToString(Session("DefaultPostingAcctState")) = "New" Then
                            dt = BindClassification(lblserviceName.Text.Trim, serviceId.Text.Trim, "BeLoad")
                        Else
                            dt = BindClassification(lblserviceName.Text.Trim, serviceId.Text.Trim, "Select")
                        End If
                    End If
                    classGrid.DataSource = dt
                    classGrid.DataBind()
                    btnFill.Enabled = True
                Else
                    classGrid.DataSource = Nothing
                    classGrid.DataBind()
                    trClass.Visible = False
                    Dim panClass As Panel = CType(e.Row.FindControl("panClass"), Panel)
                    panClass.Visible = False
                    btnFill.Enabled = False
                    btnFill.BackColor = Gray
                End If
            End If
        End If
    End Sub
#End Region

#Region "Protected Sub gvClass_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)"
    Protected Sub gvClass_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim txtIncomeAcct As TextBox = CType(e.Row.FindControl("txtIncomeAcct"), TextBox)
            Dim txtIncomeAcctCode As TextBox = CType(e.Row.FindControl("txtIncomeAcctCode"), TextBox)
            txtIncomeAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtIncomeAcctCode.ClientID + "')")
            Dim txtVatPayableAcct As TextBox = CType(e.Row.FindControl("txtVatPayableAcct"), TextBox)
            Dim txtVatPayableAcctCode As TextBox = CType(e.Row.FindControl("txtVatPayableAcctCode"), TextBox)
            txtVatPayableAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtVatPayableAcctCode.ClientID + "')")
            Dim txtcostsalesAcct As TextBox = CType(e.Row.FindControl("txtcostsalesAcct"), TextBox)
            Dim txtcostsalesAcctCode As TextBox = CType(e.Row.FindControl("txtcostsalesAcctCode"), TextBox)
            txtcostsalesAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtcostsalesAcctCode.ClientID + "')")
            Dim txtProvVatCrAcct As TextBox = CType(e.Row.FindControl("txtProvVatCrAcct"), TextBox)
            Dim txtProvVatCrAcctCode As TextBox = CType(e.Row.FindControl("txtProvVatCrAcctCode"), TextBox)
            txtProvVatCrAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtProvVatCrAcctCode.ClientID + "')")
            Dim txtVatCrAcct As TextBox = CType(e.Row.FindControl("txtVatCrAcct"), TextBox)
            Dim txtVatCrAcctCode As TextBox = CType(e.Row.FindControl("txtVatCrAcctCode"), TextBox)
            txtVatCrAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtVatCrAcctCode.ClientID + "')")
            Dim txtCostSalesDiffAcct As TextBox = CType(e.Row.FindControl("txtCostSalesDiffAcct"), TextBox)
            Dim txtCostSalesDiffAcctCode As TextBox = CType(e.Row.FindControl("txtCostSalesDiffAcctCode"), TextBox)
            txtCostSalesDiffAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtCostSalesDiffAcctCode.ClientID + "')")
            Dim txtProvCrCtrlAcct As TextBox = CType(e.Row.FindControl("txtProvCrCtrlAcct"), TextBox)
            Dim txtProvCrCtrlAcctCode As TextBox = CType(e.Row.FindControl("txtProvCrCtrlAcctCode"), TextBox)
            txtProvCrCtrlAcct.Attributes.Add("onkeyup", "ClearCode(this,'" + txtProvCrCtrlAcctCode.ClientID + "')")
        End If
    End Sub
#End Region

#Region "Protected Sub chkClassify_CheckedChanged(sender As Object, e As System.EventArgs)"
    Protected Sub chkClassify_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim ds As New DataSet
            ds = MakeDataSet()
            Session("DsClassify") = ds
            Dim dtService As DataTable = GenerateGridColumns("Update")
            gvServices.DataSource = dtService
            gvServices.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function MakeDataSet() As DataSet"
    Protected Function MakeDataSet() As DataSet
        Try
            Dim ds As New DataSet
            For Each gvr As GridViewRow In gvServices.Rows
                Dim chkClassify As CheckBox = CType(gvr.FindControl("chkClassify"), CheckBox)
                Dim classGrid As GridView = CType(gvr.FindControl("gvClass"), GridView)
                Dim trClass As HtmlTableRow = CType(gvr.FindControl("trClass"), HtmlTableRow)
                Dim lblserviceName As Label = CType(gvr.FindControl("lblServiceName"), Label)
                Dim lblserviceId As Label = CType(gvr.FindControl("lblServiceId"), Label)
                If chkClassify.Checked = True And lblserviceName.Text.Trim <> "Hotel Rooms" Then
                    Dim servMode As String = ""
                    Dim dt As New DataTable
                    If classGrid.Rows.Count > 0 Then
                        servMode = "Update"
                        dt = BindClassification(lblserviceName.Text.Trim, lblserviceId.Text.Trim, servMode, classGrid)
                    Else
                        servMode = "BeLoad"
                        dt = BindClassification(lblserviceName.Text.Trim, lblserviceId.Text.Trim, servMode)
                    End If
                    dt.TableName = lblserviceName.Text.Trim
                    ds.Tables.Add(dt)
                End If
            Next
            MakeDataSet = ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Sub btnFill_Click(sender As Object, e As System.EventArgs)"
    Protected Sub btnFill_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btnFill As Button = CType(sender, Button)
            Dim gvr As GridViewRow = CType(btnFill.NamingContainer, GridViewRow)
            Dim chkClassify As CheckBox = CType(gvr.FindControl("chkClassify"), CheckBox)
            If chkClassify.Checked = True Then
                Dim lblserviceName As Label = CType(gvr.FindControl("lblServiceName"), Label)
                Dim txtIncomeAcct As TextBox = CType(gvr.FindControl("txtIncomeAcct"), TextBox)
                Dim txtIncomeAcctCode As TextBox = CType(gvr.FindControl("txtIncomeAcctCode"), TextBox)
                Dim txtVatPayableAcct As TextBox = CType(gvr.FindControl("txtVatPayableAcct"), TextBox)
                Dim txtVatPayableAcctCode As TextBox = CType(gvr.FindControl("txtVatPayableAcctCode"), TextBox)
                Dim txtcostsalesAcct As TextBox = CType(gvr.FindControl("txtcostsalesAcct"), TextBox)
                Dim txtcostsalesAcctCode As TextBox = CType(gvr.FindControl("txtcostsalesAcctCode"), TextBox)
                Dim txtProvVatCrAcct As TextBox = CType(gvr.FindControl("txtProvVatCrAcct"), TextBox)
                Dim txtProvVatCrAcctCode As TextBox = CType(gvr.FindControl("txtProvVatCrAcctCode"), TextBox)
                Dim txtVatCrAcct As TextBox = CType(gvr.FindControl("txtVatCrAcct"), TextBox)
                Dim txtVatCrAcctCode As TextBox = CType(gvr.FindControl("txtVatCrAcctCode"), TextBox)
                Dim txtCostSalesDiffAcct As TextBox = CType(gvr.FindControl("txtCostSalesDiffAcct"), TextBox)
                Dim txtCostSalesDiffAcctCode As TextBox = CType(gvr.FindControl("txtCostSalesDiffAcctCode"), TextBox)
                Dim txtProvCrCtrlAcct As TextBox = CType(gvr.FindControl("txtProvCrCtrlAcct"), TextBox)
                Dim txtProvCrCtrlAcctCode As TextBox = CType(gvr.FindControl("txtProvCrCtrlAcctCode"), TextBox)
                If txtIncomeAcct.Text.Trim <> "" And txtcostsalesAcctCode.Text.Trim <> "" Or txtVatPayableAcct.Text.Trim <> "" And txtVatPayableAcctCode.Text.Trim <> "" Or
                    txtcostsalesAcct.Text.Trim <> "" And txtcostsalesAcctCode.Text.Trim <> "" Or txtProvVatCrAcct.Text.Trim <> "" And txtProvVatCrAcctCode.Text.Trim <> "" Or
                    txtVatCrAcct.Text.Trim <> "" And txtVatCrAcctCode.Text.Trim <> "" Or txtCostSalesDiffAcct.Text.Trim <> "" And txtCostSalesDiffAcctCode.Text.Trim <> "" Or
                    txtProvCrCtrlAcct.Text.Trim <> "" And txtProvCrCtrlAcctCode.Text.Trim <> "" Then
                    Dim ds As New DataSet
                    ds = MakeDataSet()
                    If ds.Tables.Contains(lblserviceName.Text.Trim) Then
                        Dim dt As DataTable = ds.Tables(lblserviceName.Text.Trim)
                        For Each dr As DataRow In dt.Rows
                            dr("IncomeAcct") = txtIncomeAcct.Text.Trim
                            dr("IncomeAcctCode") = txtIncomeAcctCode.Text.Trim
                            dr("VatPayableAcct") = txtVatPayableAcct.Text.Trim
                            dr("VatPayableAcctCode") = txtVatPayableAcctCode.Text.Trim
                            dr("CostSalesAcct") = txtcostsalesAcct.Text.Trim
                            dr("CostSalesAcctCode") = txtcostsalesAcctCode.Text.Trim
                            dr("ProvVatCrAcct") = txtProvVatCrAcct.Text.Trim
                            dr("ProvVatCrAcctCode") = txtProvVatCrAcctCode.Text.Trim
                            dr("VatCrAcct") = txtVatCrAcct.Text.Trim
                            dr("VatCrAcctCode") = txtVatCrAcctCode.Text.Trim
                            dr("CostSalesDiffAcct") = txtCostSalesDiffAcct.Text.Trim
                            dr("CostSalesDiffAcctCode") = txtCostSalesDiffAcctCode.Text.Trim
                            dr("ProvCrCtrlAcct") = txtProvCrCtrlAcct.Text.Trim
                            dr("ProvCrCtrlAcctCode") = txtProvCrCtrlAcctCode.Text.Trim
                        Next
                        dt.AcceptChanges()
                    End If
                    Session("DsClassify") = ds
                    Dim dtService As DataTable = GenerateGridColumns("Update")
                    gvServices.DataSource = dtService
                    gvServices.DataBind()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function Validation() As Boolean"
    Protected Function Validation() As Boolean
        Try
            If txtApplicableTo.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable To can not be empty');", True)
                txtApplicableTo.Focus()
                Validation = False
                Exit Function
            End If

            If HFshowctry_agent.Value = "Y" Then


                If wucCountrygroup.checkcountrylist.ToString = "" And wucCountrygroup.checkagentlist.ToString = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select any one of the Country or Agent');", True)
                    wucCountrygroup.Focus()
                    Validation = False
                    Exit Function
                End If

                Dim countriesExist As String = ""
                Dim agentsExist As String = ""
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                mySqlCmd = New SqlCommand("findCountriesAgents", mySqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@postingId", SqlDbType.VarChar, 20)).Value = txtPostingId.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@countries", SqlDbType.VarChar)).Value = wucCountrygroup.checkcountrylist.ToString.Trim()
                mySqlCmd.Parameters.Add(New SqlParameter("@agents", SqlDbType.VarChar)).Value = wucCountrygroup.checkagentlist.ToString.Trim()
                Using ds As New DataSet
                    Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                        myDataAdapter.Fill(ds)
                    End Using
                    countriesExist = ds.Tables(0).Rows(0)(0)
                    agentsExist = ds.Tables(1).Rows(0)(0)
                End Using
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(mySqlConn)

                If countriesExist <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Posting already created following countries " + countriesExist + "');", True)
                    wucCountrygroup.Focus()
                    Validation = False
                    Exit Function
                End If

                If agentsExist <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Posting already created following agents" + agentsExist + "');", True)
                    wucCountrygroup.Focus()
                    Validation = False
                    Exit Function
                End If
            End If

            Dim lblServiceId As New Label
            Dim lblServiceName As New Label
            Dim chkClassify As New CheckBox

            For Each gvr As GridViewRow In gvServices.Rows
                lblServiceName = CType(gvr.FindControl("lblServiceName"), Label)
                Dim StatusGridRow As Boolean = CheckGridRow(gvr)
                If StatusGridRow = False Then
                    Validation = False
                Else
                    chkClassify = CType(gvr.FindControl("chkClassify"), CheckBox)
                    If chkClassify.Checked = True And lblServiceName.Text.Trim <> "Hotel Rooms" Then
                        Dim gvClass As GridView = CType(gvr.FindControl("gvClass"), GridView)
                        Dim Gridstatus As Boolean = False
                        For Each gvrClass As GridViewRow In gvClass.Rows
                            Dim StatusClassRow As Boolean = CheckGridRow(gvrClass)
                            If StatusClassRow = False Then
                                Validation = False
                                Exit Function
                            End If
                        Next
                        If CheckGrid(gvClass) = False Then
                            Dim str As String = "Select accounts in " + lblServiceName.Text + " Classification List OR Uncheck classification"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + str + "');", True)
                            Validation = False
                            Exit Function
                        End If
                    End If
                End If
            Next
            Validation = True
        Catch ex As Exception
            Validation = False
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Function CheckGridRow(ByVal gvr As GridViewRow) As Boolean"
    Protected Function CheckGridRow(ByVal gvr As GridViewRow) As Boolean
        Dim lblServiceId As New Label
        Dim lblServiceName As New Label
        Dim txtIncomeAcct As New TextBox
        Dim txtIncomeAcctCode As New TextBox
        Dim txtVatPayableAcct As New TextBox
        Dim txtVatPayableAcctCode As New TextBox
        Dim txtcostsalesAcct As New TextBox
        Dim txtcostsalesAcctCode As New TextBox
        Dim txtProvVatCrAcct As New TextBox
        Dim txtProvVatCrAcctCode As New TextBox
        Dim txtVatCrAcct As New TextBox
        Dim txtVatCrAcctCode As New TextBox
        Dim txtCostSalesDiffAcct As New TextBox
        Dim txtCostSalesDiffAcctCode As New TextBox
        Dim txtProvCrCtrlAcct As New TextBox
        Dim txtProvCrCtrlAcctCode As New TextBox
        txtIncomeAcct = CType(gvr.FindControl("txtIncomeAcct"), TextBox)
        txtIncomeAcctCode = CType(gvr.FindControl("txtIncomeAcctCode"), TextBox)
        If txtIncomeAcct.Text <> "" And txtIncomeAcctCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Income Account');", True)
            txtIncomeAcct.Focus()
            CheckGridRow = False
            Exit Function
        End If
        txtVatPayableAcct = CType(gvr.FindControl("txtVatPayableAcct"), TextBox)
        txtVatPayableAcctCode = CType(gvr.FindControl("txtVatPayableAcctCode"), TextBox)
        If txtVatPayableAcct.Text <> "" And txtVatPayableAcctCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check VAT Payable Account');", True)
            txtVatPayableAcct.Focus()
            CheckGridRow = False
            Exit Function
        End If
        txtcostsalesAcct = CType(gvr.FindControl("txtcostsalesAcct"), TextBox)
        txtcostsalesAcctCode = CType(gvr.FindControl("txtcostsalesAcctCode"), TextBox)
        If txtcostsalesAcct.Text <> "" And txtcostsalesAcctCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Cost of Sales Account');", True)
            txtcostsalesAcct.Focus()
            CheckGridRow = False
            Exit Function
        End If
        txtProvVatCrAcct = CType(gvr.FindControl("txtProvVatCrAcct"), TextBox)
        txtProvVatCrAcctCode = CType(gvr.FindControl("txtProvVatCrAcctCode"), TextBox)
        If txtProvVatCrAcct.Text <> "" And txtProvVatCrAcctCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Provisional VAT Input Credit Account');", True)
            txtProvVatCrAcct.Focus()
            CheckGridRow = False
            Exit Function
        End If
        txtVatCrAcct = CType(gvr.FindControl("txtVatCrAcct"), TextBox)
        txtVatCrAcctCode = CType(gvr.FindControl("txtVatCrAcctCode"), TextBox)
        If txtVatCrAcct.Text <> "" And txtVatCrAcctCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check VAT Input Credit Account');", True)
            txtVatCrAcct.Focus()
            CheckGridRow = False
            Exit Function
        End If
        txtCostSalesDiffAcct = CType(gvr.FindControl("txtCostSalesDiffAcct"), TextBox)
        txtCostSalesDiffAcctCode = CType(gvr.FindControl("txtCostSalesDiffAcctCode"), TextBox)
        If txtCostSalesDiffAcct.Text <> "" And txtCostSalesDiffAcctCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Cost of Sales difference Account');", True)
            txtCostSalesDiffAcct.Focus()
            CheckGridRow = False
            Exit Function
        End If
        txtProvCrCtrlAcct = CType(gvr.FindControl("txtProvCrCtrlAcct"), TextBox)
        txtProvCrCtrlAcctCode = CType(gvr.FindControl("txtProvCrCtrlAcctCode"), TextBox)
        If txtProvCrCtrlAcct.Text <> "" And txtProvCrCtrlAcctCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Provisional Creditors Control Account');", True)
            txtProvCrCtrlAcct.Focus()
            CheckGridRow = False
            Exit Function
        End If
        CheckGridRow = True
    End Function
#End Region

#Region "Protected Function CheckGrid(ByVal gvClass As GridView) As Boolean"
    Protected Function CheckGrid(ByVal gvClass As GridView) As Boolean
        Dim txtIncomeAcctCode As New TextBox
        Dim txtVatPayableAcctCode As New TextBox
        Dim txtcostsalesAcctCode As New TextBox
        Dim txtProvVatCrAcctCode As New TextBox
        Dim txtVatCrAcctCode As New TextBox
        Dim txtCostSalesDiffAcctCode As New TextBox
        Dim txtProvCrCtrlAcctCode As New TextBox
        Dim Gridstatus As Boolean = False
        For Each gvr As GridViewRow In gvClass.Rows
            txtIncomeAcctCode = CType(gvr.FindControl("txtIncomeAcctCode"), TextBox)
            txtVatPayableAcctCode = CType(gvr.FindControl("txtVatPayableAcctCode"), TextBox)
            txtcostsalesAcctCode = CType(gvr.FindControl("txtcostsalesAcctCode"), TextBox)
            txtProvVatCrAcctCode = CType(gvr.FindControl("txtProvVatCrAcctCode"), TextBox)
            txtVatCrAcctCode = CType(gvr.FindControl("txtVatCrAcctCode"), TextBox)
            txtCostSalesDiffAcctCode = CType(gvr.FindControl("txtCostSalesDiffAcctCode"), TextBox)
            txtProvCrCtrlAcctCode = CType(gvr.FindControl("txtProvCrCtrlAcctCode"), TextBox)

            If txtIncomeAcctCode.Text.Trim <> "" Or txtVatPayableAcctCode.Text.Trim <> "" Or txtcostsalesAcctCode.Text.Trim <> "" Or txtProvVatCrAcctCode.Text.Trim <> "" Or
               txtVatCrAcctCode.Text.Trim <> "" Or txtCostSalesDiffAcctCode.Text.Trim <> "" Or txtProvCrCtrlAcctCode.Text.Trim <> "" Then
                Gridstatus = True
                Exit For
            End If
        Next
        CheckGrid = Gridstatus
    End Function
#End Region

#Region "Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Session("DefaultPostingAcctState") = "New" Or Session("DefaultPostingAcctState") = "Edit" Then
                If Validation() = False Then
                    Exit Sub
                End If
                Dim dtServices As DataTable = GenerateGridColumns("MakeTable")
                'Dim servRow = (From n In dtServices.AsEnumerable() Where n.Field(Of String)("IncomeAcctCode") = "" And n.Field(Of String)("VatPayableAcctCode") = "" And
                '                n.Field(Of String)("CostSalesAcctCode") = "" And n.Field(Of String)("ProvVatCrAcctCode") = "" And n.Field(Of String)("VatCrAcctCode") = "" And
                '                n.Field(Of String)("CostSalesDiffAcctCode") = "" And n.Field(Of String)("ProvCrCtrlAcctCode") = "" Select n)
                'If servRow.Count > 0 Then
                '    Dim servName As String = ""
                '    For Each row In servRow.ToList
                '        If servName = "" Then
                '            servName = row("ServiceName")
                '        Else
                '            servName = servName + ", " + row("ServiceName")
                '        End If
                '    Next
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Accounts for " + servName + "');", True)
                '    Exit Sub
                'End If
                Dim classDs As DataSet = MakeDataSet()
                Dim ClassDt As New DataTable
                If classDs.Tables.Count > 0 Then
                    For Each dt As DataTable In classDs.Tables
                        Dim ClassRow = (From n In dt.AsEnumerable() Where n.Field(Of String)("IncomeAcctCode") = "" And n.Field(Of String)("VatPayableAcctCode") = "" And
                                    n.Field(Of String)("CostSalesAcctCode") = "" And n.Field(Of String)("ProvVatCrAcctCode") = "" And n.Field(Of String)("VatCrAcctCode") = "" And
                                    n.Field(Of String)("CostSalesDiffAcctCode") = "" And n.Field(Of String)("ProvCrCtrlAcctCode") = "" Select n)
                        If ClassRow.Count > 0 Then
                            For Each row In ClassRow.ToList
                                row.Delete()
                            Next
                        End If
                        If dt.Columns.Contains("ClassName") Then
                            dt.Columns.Remove("ClassName")
                        End If
                        If dt.Columns.Contains("IncomeAcct") Then
                            dt.Columns.Remove("IncomeAcct")
                        End If
                        If dt.Columns.Contains("VatPayableAcct") Then
                            dt.Columns.Remove("VatPayableAcct")
                        End If
                        If dt.Columns.Contains("CostSalesAcct") Then
                            dt.Columns.Remove("CostSalesAcct")
                        End If
                        If dt.Columns.Contains("ProvVatCrAcct") Then
                            dt.Columns.Remove("ProvVatCrAcct")
                        End If
                        If dt.Columns.Contains("VatCrAcct") Then
                            dt.Columns.Remove("VatCrAcct")
                        End If
                        If dt.Columns.Contains("CostSalesDiffAcct") Then
                            dt.Columns.Remove("CostSalesDiffAcct")
                        End If
                        If dt.Columns.Contains("ProvCrCtrlAcct") Then
                            dt.Columns.Remove("ProvCrCtrlAcct")
                        End If
                        dt.AcceptChanges()
                    Next
                    ClassDt = classDs.Tables(0).Clone()
                    For Each dt As DataTable In classDs.Tables
                        ClassDt.Merge(dt)
                    Next
                End If
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                sqlTrans = mySqlConn.BeginTransaction
                Dim PostingId As String = ""
                If Session("DefaultPostingAcctState") = "New" Then
                    If txtDivCode.Text.Trim = "01" Then
                        PostingId = objUtils.GetAutoDocNo("POSTACCTRP", mySqlConn, sqlTrans)
                    ElseIf txtDivCode.Text.Trim = "02" Then
                        PostingId = objUtils.GetAutoDocNo("POSTACCTRG", mySqlConn, sqlTrans)
                    Else
                        PostingId = ""
                    End If
                    txtPostingId.Text = PostingId
                End If
                If txtPostingId.Text.Trim = "" Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbSqlTransation(sqlTrans)
                    clsDBConnect.dbConnectionClose(mySqlConn)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Posting ID can not be empty');", True)
                    Exit Sub
                End If

                For Each dr As DataRow In dtServices.Rows
                    If Session("DefaultPostingAcctState") = "New" Then
                        mySqlCmd = New SqlCommand("Add_InvoicePostingAccounts", mySqlConn, sqlTrans)
                    ElseIf Session("DefaultPostingAcctState") = "Edit" Then

                        mySqlCmd = New SqlCommand("mod_InvoicePostingAccounts", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@postingId", SqlDbType.VarChar, 20)).Value = txtPostingId.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivCode.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@serviceId", SqlDbType.VarChar, 20)).Value = dr("ServiceId")
                    mySqlCmd.Parameters.Add(New SqlParameter("@serviceName", SqlDbType.VarChar, 500)).Value = dr("ServiceName")
                    mySqlCmd.Parameters.Add(New SqlParameter("@applicableTo", SqlDbType.VarChar, 1000)).Value = txtApplicableTo.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@incomeAcct", SqlDbType.VarChar, 20)).Value = dr("IncomeAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@VATPayableAcct", SqlDbType.VarChar, 20)).Value = dr("VatPayableAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@costSalesAcct", SqlDbType.VarChar, 20)).Value = dr("CostSalesAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@ProvVATInputCRAcct", SqlDbType.VarChar, 20)).Value = dr("ProvVatCrAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@VATInputCRAcct", SqlDbType.VarChar, 20)).Value = dr("VatCrAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@CostSalesDiffAcct", SqlDbType.VarChar, 20)).Value = dr("CostSalesDiffAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@ProvCreditorsControlAcct", SqlDbType.VarChar, 20)).Value = dr("ProvCrCtrlAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@PostingClassification", SqlDbType.Bit)).Value = IIf(dr("Classification") = True, 1, 0)

                    If HFshowctry_agent.Value = "Y" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@countries", SqlDbType.VarChar)).Value = wucCountrygroup.checkcountrylist.ToString.Trim()
                        mySqlCmd.Parameters.Add(New SqlParameter("@agents", SqlDbType.VarChar)).Value = wucCountrygroup.checkagentlist.ToString.Trim()
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@countries", SqlDbType.VarChar)).Value = System.DBNull.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@agents", SqlDbType.VarChar)).Value = System.DBNull.Value

                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 100)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd.Dispose()
                Next

                If Session("DefaultPostingAcctState") = "Edit" Then
                    mySqlCmd = New SqlCommand("del_InvoicePostingAcctClassification", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivCode.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@postingId", SqlDbType.VarChar, 20)).Value = txtPostingId.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 100)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                If ClassDt.Rows.Count > 0 Then
                    Dim xmlClassification As String = ConvertDatatableToXML(ClassDt)
                    mySqlCmd = New SqlCommand("add_InvoicePostingAcctClassification", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivCode.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@postingId", SqlDbType.VarChar, 20)).Value = txtPostingId.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@xmlClassification", SqlDbType.Xml)).Value = xmlClassification
                    mySqlCmd.ExecuteNonQuery()
                End If
            End If
            sqlTrans.Commit()
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
            Dim msg As String = ""
            If Session("DefaultPostingAcctState") = "New" Then
                msg = "Created"
            ElseIf Session("DefaultPostingAcctState") = "Edit" Then
                msg = "Updated"
            ElseIf Session("DefaultPostingAcctState") = "Delete" Then
                msg = "Deleted"
            End If
            msg = "alert('Default Posting Account " + msg + " Successfully' );"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", msg, True)
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('DefaultPostingAcctPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Catch ex As Exception
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    If Not sqlTrans Is Nothing Then
                        sqlTrans.Rollback()
                    End If
                    clsDBConnect.dbCommandClose(mySqlCmd)
                    clsDBConnect.dbConnectionClose(mySqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Public Function ConvertDatatableToXML(ByVal dt As DataTable) As String"
    Public Function ConvertDatatableToXML(ByVal dt As DataTable) As String
        Dim mstr As New MemoryStream()
        dt.TableName = "Classification"
        dt.WriteXml(mstr, True)
        mstr.Seek(0, SeekOrigin.Begin)
        Dim sr As New StreamReader(mstr)
        Dim xmlstr As String
        xmlstr = sr.ReadToEnd()
        Return (xmlstr)
    End Function
#End Region

End Class
