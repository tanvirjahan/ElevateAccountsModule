Imports System.Data.SqlClient
Imports System.Data

Partial Class PriceListModule_OthPricelistSellingRates2
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As SqlDataAdapter
    'Dim Table As New DataTable()
    'Dim ParameterArray As New ArrayList()
    Private dt As New DataTable
    Private cnt As Long
    Private arr(1) As String
    Private arrRName(1) As String
    'Dim GvRow As String
    Dim gvRow1 As GridViewRow
    Dim dpFDate As New TextBox
    Dim dpTDate As New TextBox
#End Region

#Region " Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'ViewState.Add("HederRsellingcodeState1", Request.QueryString("State"))
        'ViewState.Add("HederRsellingcodeCode1", Request.QueryString("RefCode"))
        ViewState.Add("OthPLsellingRate2State", Request.QueryString("State"))
        ViewState.Add("OthPLsellingRate2Code", Request.QueryString("RefCode"))

        ' Me.Title = "Price List - Selling Rates"

        If IsPostBack = False Then
            FillDDL()

            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddlSPCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSPName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSupplierACode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSupplierAName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSubSeasonCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSubSeasonName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If

            'DisableControl()
            'ShowRecord(CType(txtPLCode.Text.Trim, String))

            Session("PlistSaved") = False
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPCode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierACode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
            Dim strqry As String = ""
            If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then
                strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                    " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

            ElseIf Session("OthPListFilter") = "OTH" Then

                strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                    " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027,1028)) order by othgrpcode"
            End If

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strqry, True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupn, "othgrpcode", "othgrpname", strqry, True)




            If Request.QueryString("PListCode") <> Nothing Then
                'txtBlockCode.Value = Request.QueryString("PListCode")
                txtPLCode.Text = Request.QueryString("PListCode")
            End If

            If Request.QueryString("supplier") <> Nothing Then
                ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text = Request.QueryString("supplier")
                'GetWeekEndValues(Request.QueryString("supplier"))
            End If
            If Request.QueryString("suppliername") <> Nothing Then
                ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text = Request.QueryString("suppliername")
            End If
            If Request.QueryString("SupplierType") <> Nothing Then
                ddlSPCode.Items(ddlSPCode.SelectedIndex).Text = Request.QueryString("SupplierType")
            End If
            If Request.QueryString("SupplierTypeName") <> Nothing Then
                ddlSPName.Items(ddlSPName.SelectedIndex).Text = Request.QueryString("SupplierTypeName")
            End If

            If Request.QueryString("SuppierAgent") <> Nothing Then
                ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text = Request.QueryString("SuppierAgent")
            End If
            If Request.QueryString("SupplierAgentName") <> Nothing Then
                ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text = Request.QueryString("SupplierAgentName")
            End If

            If Request.QueryString("CurrencyCode") <> Nothing Then
                ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = Request.QueryString("CurrencyCode")
            End If
            If Request.QueryString("CurrencyName") <> Nothing Then
                ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = Request.QueryString("CurrencyName")
            End If
            If Request.QueryString("SubSeasonCode") <> Nothing Then
                ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text = Request.QueryString("SubSeasonCode")
            End If
            If Request.QueryString("SubSeasonName") <> Nothing Then
                ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text = Request.QueryString("SubSeasonName")
            End If
            ddlGroupCode.SelectedIndex = 0

            FillGridMarket("plgrpcode")

            Dim marketstr() As String
            Dim lblcode As Label
            Dim chksel As CheckBox
            If Request.QueryString("Market") <> Nothing Then
                marketstr = Request.QueryString("Market").ToString.Split(";")
                For i = 0 To marketstr.GetUpperBound(0)
                    For Each Me.gvRow1 In gv_Market.Rows
                        lblcode = gvRow1.FindControl("lblcode")
                        chksel = gvRow1.FindControl("chkSelect")
                        If marketstr(i).Trim = lblcode.Text.Trim Then
                            chksel.Checked = True
                        End If
                    Next
                Next

            End If


            If Request.QueryString("sellcode") <> Nothing Then
                lblSellingCode.Text = Request.QueryString("sellcode")
            End If

            ShowSellingRadioBtns(ViewState("OthPLsellingRate2Code"))
            createdatatable()
            dt = Session("OthGridData")
            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClassPriceList(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClassPriceList(ListItemType.Item, col.ColumnName, fld2)
                    gv_SearchResult.Columns.Add(bfield)

                End If
            Next
            Session("OthGridData") = dt
            gv_SearchResult.Visible = True
            gv_SearchResult.DataSource = dt

            'InstantiateIn Grid View
            gv_SearchResult.DataBind()
        Else
            dt = Session("OthGridData")
            dt = Session("OthGridData")
            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClassPriceList(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClassPriceList(ListItemType.Item, col.ColumnName, fld2)
                    gv_SearchResult.Columns.Add(bfield)

                End If
            Next
            Session("OthGridData") = dt
            gv_SearchResult.Visible = True
            gv_SearchResult.DataSource = dt
            'InstantiateIn Grid View
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region


#Region "Private Sub FillDDL()"
    Private Sub FillDDL()
        strSqlQry = ""
        strSqlQry = "SELECT currcode,currname FROM currmast WHERE active=1 ORDER BY currcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyCode, "currcode", "currname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT currcode,currname FROM currmast WHERE active=1 ORDER BY currname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", strSqlQry, True)

        strSqlQry = ""
        strSqlQry = "SELECT sptypecode,sptypename FROM sptypemast WHERE active=1 ORDER BY sptypecode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPCode, "sptypecode", "sptypename", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT sptypecode,sptypename FROM sptypemast WHERE active=1 ORDER BY sptypename"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPName, "sptypename", "sptypecode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT supagentcode,supagentname FROM supplier_agents WHERE active=1 ORDER BY supagentcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierACode, "supagentcode", "supagentname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT supagentcode,supagentname FROM supplier_agents WHERE active=1 ORDER BY supagentname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAName, "supagentname", "supagentcode", strSqlQry, True)

        strSqlQry = ""
        strSqlQry = "SELECT partycode,partyname FROM partymast WHERE active=1 ORDER BY partycode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT partycode,partyname FROM partymast WHERE active=1 ORDER BY partyname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT subseascode,subseasname FROM subseasmast WHERE active=1 ORDER BY subseascode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonCode, "subseascode", "subseasname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT subseascode,subseasname FROM subseasmast WHERE active=1 ORDER BY subseasname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonName, "subseasname", "subseascode", strSqlQry, True)

    End Sub
#End Region

#Region "Private Sub createdatatable()"
    'Private Sub createdatatable()
    '    Try

    '        cnt = 0

    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        strSqlQry = "select count(rmcatcode) from partyrmcat where partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'"
    '        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
    '        cnt = mySqlCmd.ExecuteScalar
    '        mySqlConn.Close()

    '        '            Dim arr(cnt + 1) As String
    '        ReDim arr(cnt + 1)
    '        ReDim arrRName(cnt + 1)
    '        Dim i As Long = 0

    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        'strSqlQry = "select rmcatcode from partyrmcat where partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'" ' order by rmcatcode"
    '        strSqlQry = "select p.rmcatcode from partyrmcat p,rmcatmast r where p.rmcatcode=r.rmcatcode and p.partycode='" _
    '                   & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by isnull(r.rankorder,999)"  ' p.rm
    '        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
    '        mySqlReader = mySqlCmd.ExecuteReader()
    '        While mySqlReader.Read = True
    '            arr(i) = mySqlReader("rmcatcode")
    '            i = i + 1
    '        End While
    '        mySqlReader.Close()
    '        mySqlConn.Close()
    '        'select rmcatcode from partyrmcat where partycode='3'
    '        'Here in Array store room types
    '        '-------------------------------------
    '        Dim tf As New TemplateField
    '        Dim dt As New DataTable
    '        dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Room Type Code", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Room Type Name", GetType(String)))
    '        dt.Columns.Add(New DataColumn("M.Plan", GetType(String)))

    '        'create columns of this room types in data table
    '        'createdatarows()
    '        For i = 0 To cnt - 1
    '            dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
    '        Next
    '        dt.Columns.Add(New DataColumn("Y/n", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Pkg", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Canc Days", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Compl Nights", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Remark", GetType(String)))
    '        Session("GV_HotelData") = dt


    '        'Dim fld2 As String = ""
    '        'Dim col As DataColumn
    '        'For Each col In dt.Columns
    '        '    If col.ColumnName <> "Sr No" And col.ColumnName <> "Room Type Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "M.Plan" Then
    '        '        Dim bfield As New TemplateField
    '        '        'Call Function
    '        '        bfield.HeaderTemplate = New ClassPriceList(ListItemType.Header, col.ColumnName, fld2)
    '        '        bfield.ItemTemplate = New ClassPriceList(ListItemType.Item, col.ColumnName, fld2)
    '        '        gv_SearchResult.Columns.Add(bfield)

    '        '    End If

    '        'Next
    '        'Session("GV_HotelData") = dt
    '        'gv_SearchResult.Visible = True
    '        'gv_SearchResult.DataSource = dt



    '        ''InstantiateIn Grid View
    '        'gv_SearchResult.DataBind()


    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Sub createdatatable()
        Try

            cnt = 0
            strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode " & _
                " WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "')and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1 "
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt <= 0 Then
                Session("CheckGridColumn") = "Not Present"
            Else
                Session("CheckGridColumn") = ""
            End If

            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0
            Dim Column As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "SELECT  distinct dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode,othcatmast.grporder FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1 Order by othcatmast.grporder"

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(Column) = mySqlReader("othcatcode")
                Column = Column + 1
            End While


            'Here in Array store room types
            '-------------------------------------
            Dim tf As New TemplateField
            dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Code", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Name", GetType(String)))

            'create columns of this room types in data table
            For i = 0 To Column - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next
            'dt.Columns.Add(New DataColumn("From Date", GetType(String)))
            'dt.Columns.Add(New DataColumn("To Date", GetType(String)))
            dt.Columns.Add(New DataColumn("Pkg", GetType(String)))
            'Session("GV_HotelData") = dt
            Session("OthGridData") = dt


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceslistSellingRates1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

#Region " Private Sub createdatarows()"
    'Private Sub createdatarows()
    '    Dim k As Long = 0
    '    Try
    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        strSqlQry = "SELECT count(partyrmtyp.rmtypcode) FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode where(partyrmtyp.rmtypcode = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) and partyrmtyp.partycode=partymeal.partycode and partyrmtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'"

    '        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
    '        cnt = mySqlCmd.ExecuteScalar
    '        mySqlConn.Close()

    '        Dim arr_rows(cnt + 1) As String
    '        Dim arr_rname(cnt + 1) As String
    '        Dim arr_meal(cnt + 1) As String
    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '        strSqlQry = ""

    '        'strSqlQry = "SELECT partyrmtyp.rmtypcode, rmtypmast.rmtypname, partymeal.mealcode,rmtypmast.remarks " _
    '        '           & " FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode " _
    '        '           & " INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode " _
    '        '           & "    where(partyrmtyp.rmtypcode " _
    '        '           & " = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) and partyrmtyp.partycode=partymeal.partycode and " _
    '        '           & "partyrmtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' and partymeal.partycode='" _
    '        '           & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by  isnull(partyrmtyp.rankord,999)"
    '        If txtPromotionCode.Text.Trim = "" Or txtPromotionCode.Text.Trim = "[Select]" Then
    '            strSqlQry = "SELECT partyrmtyp.rmtypcode, rmtypmast.rmtypname, partymeal.mealcode,rmtypmast.remarks " _
    '                         & " FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode " _
    '                         & " INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode " _
    '                         & " INNER JOIN mealmast on partymeal.mealcode=mealmast.mealcode    where(partyrmtyp.rmtypcode " _
    '                         & " = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) and partyrmtyp.partycode=partymeal.partycode and " _
    '                         & "partyrmtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' and partymeal.partycode='" _
    '                         & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by isnull(mealmast.rankorder,999),mealcode, isnull(partyrmtyp.rankord,999)"

    '        Else
    '            strSqlQry = "SELECT partyrmtyp.rmtypcode, rmtypmast.rmtypname, partymeal.mealcode,rmtypmast.remarks " _
    '          & " FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode " _
    '          & " INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode " _
    '          & " INNER JOIN mealmast on partymeal.mealcode=mealmast.mealcode    where(partyrmtyp.rmtypcode " _
    '          & " = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) and partyrmtyp.partycode=partymeal.partycode and " _
    '          & "partyrmtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' and partymeal.partycode='" _
    '          & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  and partyrmtyp.rmtypcode in (select distinct rmtypcode from promotion_detail " _
    '          & " where promotionid='" & txtPromotionCode.Text & "')  and  partymeal.mealcode in (select distinct mealcode from promotion_detail " _
    '          & " where promotionid='" & txtPromotionCode.Text & "') order by isnull(mealmast.rankorder,999),mealcode,isnull(partyrmtyp.rankord,999)"

    '        End If

    '        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
    '        mySqlReader = mySqlCmd.ExecuteReader()
    '        While mySqlReader.Read = True
    '            arr_rows(k) = mySqlReader("rmtypcode")
    '            arr_rname(k) = mySqlReader("rmtypname")
    '            arr_meal(k) = mySqlReader("mealcode")
    '            k = k + 1
    '        End While
    '        mySqlReader.Close()
    '        mySqlConn.Close()

    '        'Here add rows.....
    '        'Now Get Rows From sellmast Based on SPType
    '        dt = CType(Session("GV_HotelData"), DataTable)
    '        Dim dr As DataRow

    '        Dim row As Long
    '        For row = 0 To k - 1
    '            dr = dt.NewRow
    '            'For i = 1 To cnt - 1
    '            dr("Sr No") = row + 1   ' 
    '            dr("Room Type Code") = arr_rows(row)
    '            dr("Room Type Name") = arr_rname(row)
    '            dr("M.Plan") = arr_meal(row)
    '            dr("Y/n") = "0"
    '            dr("Canc Days") = "0"
    '            If ddlPriceList.SelectedValue = "Weekly Rates 7 Nights" Then
    '                dr("Pkg") = 7
    '            ElseIf ddlPriceList.SelectedValue = "Normal Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
    '                dr("Pkg") = 1
    '            ElseIf ddlPriceList.SelectedValue = "Normal Rates > 1 Night" Then
    '                dr("Pkg") = 2
    '            ElseIf ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
    '                dr("Pkg") = 2
    '            End If
    '            dr("Compl Nights") = "1"
    '            dt.Rows.Add(dr)
    '        Next

    '        gv_SearchResult.Visible = True
    '        gv_SearchResult.DataSource = dt
    '        'InstantiateIn Grid View
    '        gv_SearchResult.DataBind()
    '    Catch ex As Exception

    '    End Try

    '    'showDynamic Frid
    '    'ShowDynamicGrid()
    'End Sub

    Private Sub createdatarows()
        Dim i As Long
        Dim k As Long = 0
        Try

            strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othtypmast.active=1"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)

            Dim arr_rnkorder(cnt + 1) As String
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "SELECT distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode,rankorder FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othtypmast.active=1 order by  rankorder"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr_rnkorder(k) = mySqlReader("rankorder")
                arr_rows(k) = mySqlReader("othtypcode")
                arr_rname(k) = mySqlReader("othtypname")
                k = k + 1
            End While


            'Here add rows.....
            'Now Get Rows From sellmast Based on SPType
            'Session("GV_HotelData") = dt
            Session("OthGridData") = dt
            Dim dr As DataRow

            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow
                'For i = 1 To cnt - 1

                ' dr("Sr No") = row + 1   ' 
                ''taken from the rankorder instead of sno due to show the rank order in the pricelists      
                dr("Sr No") = row + 1 'arr_rnkorder(row)
                dr("Service Type Code") = arr_rows(row)
                dr("Service Type Name") = arr_rname(row)
                'dr("From Date") = dpFromDate.Text
                'dr("To Date") = dpToDate.Text
                dr("Pkg") = 1
                'Next
                dt.Rows.Add(dr)
            Next


            gv_SearchResult.Visible = True
            gv_SearchResult.DataSource = dt
            'InstantiateIn Grid View
            gv_SearchResult.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceslistSellingRates1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
        '-======================||||||||||||||||||||||||||||||||||||||||==========================!!!!!!!!!!!!!!!
        ''''Herer Only Numbers Can enterd in textboxes
        Dim j As Long = 1
        Dim txt As TextBox
        Dim gvrow As GridViewRow
        j = 0
        cnt = gv_SearchResult.Columns.Count
        Dim n As Long = 0
        Dim m As Long = 0
        i = 0
        k = 0
        Try
            For Each gvrow In gv_SearchResult.Rows
                If n = 0 Then
                    For i = 0 To cnt - 4
                        txt = gvrow.FindControl("txt" & i)
                        Numbers(txt)
                        txt.CssClass = "field_input"
                        txt.Width = 60
                    Next
                    m = i
                Else
                    k = 0
                    For i = n To (m + n) - 1
                        txt = gvrow.FindControl("txt" & i)
                        Numbers(txt)
                        txt.CssClass = "field_input"
                        txt.Width = 60
                        k = k + 1
                    Next

                End If
                n = i
            Next

            ''''Herer Only Interger Numbers Can enterd in textboxes from and todate


            Dim header As Long = 0
            cnt = gv_SearchResult.Columns.Count
            Dim heading(cnt + 1) As String
            '----------------------------------------------------------------------------
            '           Stoaring heading column values in the array

            For header = 0 To cnt - 4
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
                If txt.Text = "From Date" Or txt.Text = "To Date" Then
                    txt.Width = 60
                ElseIf txt.Text = "Pkg" Then
                    txt.Width = 30
                Else
                    'If Len(txt.Text) < 20 Then
                    '    txt.Columns = 0
                    '    txt.Width = 100
                    'Else
                    txt.Columns = Len(txt.Text)
                    'txt.Width = Len(txt.Text) * 10
                    ' End If
                End If
                txt.CssClass = "field_input"
            Next

            Dim a, b As Long


            a = cnt - 7
            j = 0
            b = 0
            m = 0
            n = 0


            For Each gvrow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 4
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            'txt = gvrow.FindControl("txt" & b + a + 1)
                            'NumbersDateInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 3)
                            'NumbersDateInt(txt)
                            Numbers(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            'txt = gvrow.FindControl("txt" & b + a + 3)
                            'NumbersInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 30
                        End If
                    Next

                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            'txt = gvrow.FindControl("txt" & b + a + 1)
                            'NumbersDateInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 3)
                            Numbers(txt) 'NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            'txt = gvrow.FindControl("txt" & b + a + 3)
                            'NumbersInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 30
                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next

            'If ViewState("TrfpricelistState2") <> "New" Then
            '    'ShowDynamicGrid()
            'End If
            'ViewState("TrfpricelistRefCode2")
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Private Function FindFooterControl(ByVal Controlname As String, ByVal value As String)"
    Private Function FindHeaderTextbox(ByVal Controlname As String, ByVal value As String) As String
        Dim footerlabel As New TextBox
        Dim irow As Integer
        For irow = 0 To gv_SearchResult.Controls(0).Controls.Count - 1
            If CType(gv_SearchResult.Controls(0).Controls(irow), GridViewRow).RowType = DataControlRowType.Header Then
                Dim footer As GridViewRow = CType(gv_SearchResult.Controls(0).Controls(irow), GridViewRow)
                footerlabel = footer.FindControl(Controlname)
                Return footerlabel.Text
                Exit For
            End If
        Next
        Return footerlabel.Text
    End Function
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                End If
                lblProfitPer.Text = "Selling price lower than profit " & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 525) & "%"

                createdatarows()
                fillDategrd(grdDates, True)
                ShowDates(ViewState("OthPLsellingRate2Code"))

                ' DisableControl()
                'btnSave.Visible = False
                'txtPLCode.Text = Session("PLcode")
                Dim k As Long
                Dim j As Long = 1
                Dim txt As TextBox
                Dim GvRow As GridViewRow
                Dim srno As Long = 0
                Dim hotelcategory As String = ""
                j = 0
                Dim m As Long
                Dim n As Long = 0
                Dim cnt As Long = gv_SearchResult.Columns.Count
                Dim a As Long = cnt - 10
                Dim b As Long = 0
                Dim header As Long = 0
                Dim heading(cnt + 1) As String
                Dim flag As Boolean = False

                Try
                    For Each GvRow In gv_SearchResult.Rows
                        If n = 0 Then
                            For j = 0 To cnt - 4
                                If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                Else
                                    txt = GvRow.FindControl("txt" & b + a + 1)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            'Numbers(txt)
                                            txt.ReadOnly = True
                                        End If
                                    End If

                                    txt = GvRow.FindControl("txt" & b + a + 2)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.ReadOnly = True
                                        End If
                                    End If
                                    txt = GvRow.FindControl("txt" & j)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            Numbers(txt)
                                        End If
                                    End If
                                    txt = GvRow.FindControl("txt" & b + a + 3)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.ReadOnly = True
                                        End If
                                    End If
                                    txt = GvRow.FindControl("txt" & b + a + 4)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.ReadOnly = True
                                        End If
                                    End If
                                End If
                            Next
                            m = j
                        Else
                            k = 0
                            For j = n To (m + n) - 1
                                If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                                Else
                                    txt = GvRow.FindControl("txt" & b + a + 1)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            ' Numbers(txt)
                                            txt.ReadOnly = True
                                        End If
                                    End If
                                    txt = GvRow.FindControl("txt" & b + a + 2)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.ReadOnly = True
                                            'Numbers(txt)
                                        End If
                                    End If
                                    txt = GvRow.FindControl("txt" & j)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            Numbers(txt)
                                        End If
                                    End If

                                    txt = GvRow.FindControl("txt" & b + a + 3)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.ReadOnly = True
                                        End If
                                    End If
                                    txt = GvRow.FindControl("txt" & b + a + 4)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.ReadOnly = True
                                        End If
                                    End If
                                End If
                                k = k + 1
                            Next
                        End If
                        b = j
                        n = j
                    Next

                    Dim strOption As String = ""
                    Dim strtitle As String = ""
                    Dim strSPType As String = ""

                    If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then

                        strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthPListFilter"))
                        Select Case strOption
                            Case "CAR RENTAL"
                                strtitle = "Car Rental"
                                'strSPType = "1031"
                                'lblHeading.Text = "Car Rental Selling Formula"
                            Case "VISA"
                                strtitle = "Visa "
                                'strSPType = "1032"
                                'lblHeading.Text = "Visa Selling Formula"
                            Case "EXC"
                                strtitle = "Excursion  "
                                'strSPType = "1033"
                                'lblHeading.Text = "Excursion Selling Formula"
                            Case "MEALS"
                                strtitle = "Restaurant  "
                                'strSPType = "1034"
                                'lblHeading.Text = "Restaurant Selling Formula"
                            Case "GUIDES"
                                strtitle = "Guide  "
                                'strSPType = "1035"
                                'lblHeading.Text = "Guide Selling Formula"
                            Case "ENTRANCE"
                                strtitle = "Entrance "
                                'strSPType = "1036"
                                'lblHeading.Text = "Guide Selling Formula"
                            Case "JEEPWADI"
                                strtitle = "Jeepwadi "
                                'strSPType = "1037"
                                'lblHeading.Text = "Guide Selling Formula"
                            Case "HFEES"
                                strtitle = "Handling Fee"
                        End Select
                        'strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                        '    " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

                    ElseIf Session("OthPListFilter") = "OTH" Then
                        strtitle = "Other Service "
                        'strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                        '    " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025)) order by othgrpcode"
                    End If




                    If ViewState("OthPLsellingRate2State") = "New" Then
                        lblHeading.Text = strtitle + " Price List - Selling Rates"
                        Page.Title = Page.Title + " " + strtitle + " Price List - Selling Rates"
                        ''If ChkBManual.Checked = True Then
                        'btnSave.Visible = True
                        'btnSave.Text = "Save"
                        'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save price list?')==false)return false;")
                        ShowDynamicGrid()
                        gv_SearchResult.Enabled = False
                        'Else
                        '    btnSave.Visible = False
                        '    ShowDynamicGrid()
                        '    gv_SearchResult.Enabled = False
                        'End If
                    ElseIf ViewState("OthPLsellingRate2State") = "Edit" Then
                        lblHeading.Text = strtitle + " Price List - Selling Rates"
                        Page.Title = Page.Title + " " + strtitle + " Price List - Selling Rates"
                        ''btnGenerate.Visible = False
                        ''If ChkBManual.Checked = True Then
                        'btnSave.Visible = True
                        'btnSave.Text = "Update"
                        'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update price list?')==false)return false;")
                        ShowDynamicGrid()
                        gv_SearchResult.Enabled = False
                        'Else
                        '    btnSave.Visible = False
                        '    ShowDynamicGrid()
                        '    gv_SearchResult.Enabled = False
                        'End If
                    ElseIf ViewState("OthPLsellingRate2State") = "View" Then
                        lblHeading.Text = strtitle + " Price List - Selling Rates"
                        Page.Title = Page.Title + " " + strtitle + " Price List - Selling Rates"
                        ShowDynamicGrid()
                        btnSave.Visible = False
                        gv_SearchResult.Enabled = False
                    ElseIf ViewState("OthPLsellingRate2State") = "Copy" Then
                        lblHeading.Text = strtitle + " Price List - Selling Rates"
                        Page.Title = Page.Title + " " + strtitle + " Price List - Selling Rates"
                        ''btnGenerate.Visible = False
                        ''If ChkBManual.Checked = True Then
                        'btnSave.Visible = True
                        'btnSave.Text = "Save"
                        'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update price list?')==false)return false;")
                        ShowDynamicGrid()
                        gv_SearchResult.Enabled = False
                        'Else
                        '    btnSave.Visible = False
                        '    ShowDynamicGrid()
                        '    gv_SearchResult.Enabled = False
                        'End If
                    End If
                    tbnClose.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close?')==false)return false;")
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                    objUtils.WritErrorLog("HederRsellingcode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
                End Try
                ' FillSellingCode()
                ' ShowSellingRadioBtns(ViewState("HederRsellingcodeCode1"))
                'gv_SearchResult.Columns(gv_SearchResult.Columns.Count - 5).Visible = False
                'gv_SearchResult.Columns(gv_SearchResult.Columns.Count - 3).Visible = False
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OthPricelistSellingRates1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

    Private Sub ShowDynamicGrid()
        '        **************
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long
        Dim gvrow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        gv_SearchResult.Visible = True
        cnt = gv_SearchResult.Columns.Count
        Dim s As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        For header = 0 To cnt - 4
            txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
            If txt.Text <> Nothing Then
                heading(header) = txt.Text
            End If
        Next

        Dim m As Long = 0
        Dim othcatcode As String = ""
        Dim value As String = ""
        Dim Linno As Integer
        Dim StrQry As String
        Dim headerlabel As New TextBox
        Dim myConn As New SqlConnection
        Dim myCmd As New SqlCommand
        Dim myReader As SqlDataReader
        Dim StrQryTemp As String
        Dim cvalue As String = ""
        Dim profitperc As Long = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 525)
        Dim convrate As Decimal
        Try
            'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
            '    StrQry = "select distinct clineno from cplistdwknew where plistcode='" & txtBlockCode.Value & "' and selltype='" & lblSellingCode.Text.Trim & "'"
            'Else
            StrQry = "select distinct oclineno from othplist_selld where oplistcode='" & txtPLCode.Text & "' and selltype='" & lblSellingCode.Text.Trim & "'"
            'End If
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("oclineno")) = False Then
                            Linno = mySqlReader("oclineno")
                        End If
                        If gvrow.Cells(0).Text = Linno Then
                            convrate = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "othplisth_convrates", "convrate", "oplistcode='" & txtPLCode.Text & "' and sellcode='" & lblSellingCode.Text.Trim & "'")
                            'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
                            '    StrQryTemp = "select * from cplistdwknew where plistcode='" & txtBlockCode.Value & "' and selltype='" & lblSellingCode.Text.Trim & "' and clineno='" & Linno & "'"
                            'Else
                            StrQryTemp = "select * from othplist_selld where oplistcode='" & txtPLCode.Text & "' and selltype='" & lblSellingCode.Text.Trim & "'and oclineno='" & Linno & "'"
                            'End If
                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("othcatcode")) = False Then
                                        othcatcode = myReader("othcatcode")
                                    End If
                                    If IsDBNull(myReader("tprice")) = False Then
                                        value = myReader("tprice")
                                    Else
                                        value = ""
                                    End If
                                    If IsDBNull(myReader("tcostprice")) = False Then
                                        cvalue = myReader("tcostprice")
                                    Else
                                        cvalue = ""
                                    End If
                                    For j = 0 To cnt - 4
                                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                        Else
                                            For s = 0 To gv_SearchResult.Columns.Count '- 6
                                                headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = othcatcode Then
                                                        If gvrow.RowIndex = 0 Then
                                                            txt = gvrow.FindControl("txt" & s)
                                                        Else
                                                            txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 4) * gvrow.RowIndex) + s + gvrow.RowIndex) '5
                                                        End If

                                                        If txt Is Nothing Then
                                                        Else
                                                            If value = "" Then
                                                                txt.Text = ""
                                                            Else
                                                                txt.Text = value
                                                            End If
                                                        End If
                                                        txt.BackColor = Drawing.Color.White

                                                        If CDbl(value) > 0 Then
                                                            If Math.Round(cvalue * convrate) > CDbl(value) Then
                                                                If txt Is Nothing Then
                                                                Else
                                                                    txt.BackColor = Drawing.Color.LightCoral
                                                                End If
                                                            ElseIf Math.Round(cvalue * convrate * (100 + profitperc) / 100) > CDbl(value) Then
                                                                If txt Is Nothing Then
                                                                Else
                                                                    txt.BackColor = Drawing.Color.LightBlue
                                                                End If
                                                            End If
                                                        End If

                                                        'txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 8 + gvrow.RowIndex)
                                                        'If txt Is Nothing Then
                                                        'Else
                                                        '    If IsDBNull(myReader("nights")) = False Then
                                                        '        txt.Text = myReader("nights")
                                                        '    Else
                                                        '        txt.Text = ""
                                                        '    End If
                                                        'End If
                                                        'txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 7 + gvrow.RowIndex)
                                                        'If txt Is Nothing Then
                                                        'Else
                                                        '    If IsDBNull(myReader("confdays")) = False Then
                                                        '        txt.Text = myReader("confdays")
                                                        '    Else
                                                        '        txt.Text = ""
                                                        '    End If
                                                        'End If
                                                        'txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 6 + gvrow.RowIndex)
                                                        'If txt Is Nothing Then
                                                        'Else
                                                        '    If IsDBNull(myReader("compalsorynights")) = False Then
                                                        '        txt.Text = myReader("compalsorynights")
                                                        '    Else
                                                        '        txt.Text = ""
                                                        '    End If
                                                        'End If

                                                        'txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 5 + gvrow.RowIndex)
                                                        'If txt Is Nothing Then
                                                        'Else
                                                        '    If IsDBNull(myReader("pkgrmks")) = False Then
                                                        '        txt.Text = myReader("pkgrmks")
                                                        '    Else
                                                        '        txt.Text = ""
                                                        '    End If
                                                        'End If
                                                        txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 4) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 4 + gvrow.RowIndex) '55
                                                        If txt Is Nothing Then
                                                        Else
                                                            If IsDBNull(myReader("pkgnights")) = False Then
                                                                txt.Text = myReader("pkgnights")
                                                            Else
                                                                txt.Text = ""
                                                            End If
                                                        End If
                                                        GoTo go1
                                                    End If
                                                End If
                                            Next
                                        End If
                                    Next
go1:                            End While
                            End If
                            clsDBConnect.dbConnectionClose(myConn)
                            clsDBConnect.dbCommandClose(myCmd)
                            clsDBConnect.dbReaderClose(myReader)
                        End If
                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)


        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricelistSellingRates2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othplisth Where oplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("oplistcode")) = False Then
                        'txtBlockCode.Value = mySqlReader("plistcode")
                        txtPLCode.Text = mySqlReader("oplistcode")
                    End If
                    If IsDBNull(mySqlReader("sptypecode")) = False Then
                        ddlSPName.Value = mySqlReader("sptypecode")
                        ddlSPCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))
                    End If
                    If IsDBNull(mySqlReader("supagentcode")) = False Then
                        ddlSupplierAName.Value = mySqlReader("supagentcode")
                        ddlSupplierACode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentname", "supagentcode", mySqlReader("supagentcode"))
                    End If
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        ddlSupplierName.Value = mySqlReader("partycode")
                        ddlSupplierCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("partycode"))
                        'GetWeekEndValues(mySqlReader("partycode"))
                    End If
                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        ddlSubSeasonName.Value = mySqlReader("subseascode")
                        ddlSubSeasonCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        ddlCurrencyName.Value = mySqlReader("currcode")
                        ddlCurrencyCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If
                    'If IsDBNull(mySqlReader("revisiondate")) = False Then
                    '    dpRevDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("revisiondate")), String)
                    'End If
                    'If IsDBNull(mySqlReader("weekend2")) = False Then
                    '    If mySqlReader("weekend2") = 1 Then
                    '        ChkWeek2.Checked = True
                    '    ElseIf mySqlReader("weekend2") = 0 Then
                    '        ChkWeek2.Checked = False
                    '    End If
                    'End If
                    'If IsDBNull(mySqlReader("weekend1")) = False Then
                    '    If mySqlReader("weekend1") = 1 Then
                    '        ChkWeek1.Checked = True
                    '    ElseIf mySqlReader("weekend1") = 0 Then
                    '        ChkWeek1.Checked = False
                    '    End If
                    'End If
                    'If IsDBNull(mySqlReader("plist_mode")) = False Then
                    '    If mySqlReader("plist_mode") = 1 Then
                    '        ChkBManual.Checked = True
                    '        btnSave.Visible = True
                    '        gv_SearchResult.Enabled = True
                    '    ElseIf mySqlReader("plist_mode") = 0 Then
                    '        ChkBManual.Checked = False
                    '        btnSave.Visible = False
                    '        gv_SearchResult.Enabled = False
                    '    End If
                    'End If
                    'If IsDBNull(mySqlReader("plisttype")) = False Then
                    '    If mySqlReader("plisttype") = "0" Then
                    '        ddlPriceList.SelectedValue = "Normal Rates 1 Night"
                    '        ChkWeek1.Visible = False
                    '        ChkWeek2.Visible = False
                    '        lblWEO1.Visible = False
                    '        lblWEO2.Visible = False
                    '    ElseIf mySqlReader("plisttype") = "1" Then
                    '        ddlPriceList.SelectedValue = "Weekly Rates 7 Nights"
                    '        ChkWeek1.Visible = False
                    '        ChkWeek2.Visible = False
                    '        lblWEO1.Visible = False
                    '        lblWEO2.Visible = False
                    '    ElseIf mySqlReader("plisttype") = "2" Then
                    '        ddlPriceList.SelectedValue = "Weekend Rates 1 Night"
                    '        ChkWeek1.Visible = True
                    '        ChkWeek2.Visible = True
                    '        lblWEO1.Visible = False
                    '        lblWEO2.Visible = False
                    '    ElseIf mySqlReader("plisttype") = "3" Then
                    '        ddlPriceList.SelectedValue = "Normal Rates > 1 Night"
                    '        ChkWeek1.Visible = False
                    '        ChkWeek2.Visible = False
                    '        lblWEO1.Visible = False
                    '        lblWEO2.Visible = False
                    '    ElseIf mySqlReader("plisttype") = "4" Then
                    '        ddlPriceList.SelectedValue = "Weekend Rates > 1 Night"
                    '        ChkWeek1.Visible = True
                    '        ChkWeek2.Visible = True
                    '        lblWEO1.Visible = True
                    '        lblWEO2.Visible = True
                    '    End If
                    'End If

                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelistSellingRates2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("no", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region



#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        txtPLCode.Enabled = False
        ddlSPCode.Disabled = True
        ddlSPName.Disabled = True
        ddlCurrencyCode.Disabled = True
        ddlCurrencyName.Disabled = True
        ddlSupplierACode.Disabled = True
        ddlSupplierAName.Disabled = True
        ddlSubSeasonCode.Disabled = True
        ddlSubSeasonName.Disabled = True
        ddlSupplierCode.Disabled = True
        ddlSupplierName.Disabled = True
        btnSave.Visible = False
        'ChkWeek1.Enabled = False
        'ChkWeek2.Enabled = False
        ' ChkBManual.Enabled = False
        'If ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
        '    lblWEO1.Visible = True
        '    lblWEO2.Visible = True
        'Else
        '    lblWEO1.Visible = False
        '    lblWEO2.Visible = False
        'End If
        'ddlPriceList.Enabled = False
        'dpRevDate.Enabled = False
    End Sub
#End Region


    Protected Sub ChkBManual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''If ChkBManual.Checked = True Then
        'btnSave.Visible = True
        ''Else
        ''btnSave.Visible = False
        ''End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim week1 As Long
        Dim week2 As Long
        Dim manual As Long
        'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
        '    If ChkWeek2.Checked = True Then
        '        week2 = 1
        '    Else
        '        week2 = 0
        '    End If
        '    If ChkWeek1.Checked = True Then
        '        week1 = 1
        '    Else
        '        week1 = 0
        '    End If
        'Else
        '    week1 = 0
        '    week2 = 0
        'End If
        'If ChkBManual.Checked = True Then
        '    manual = 1
        'Else
        '    manual = 0
        'End If
        Session("SesionPlistCode") = txtPLCode.Text
        'ViewState.Add("HederRsellingcodeState1", Request.QueryString("State"))
        'ViewState.Add("HederRsellingcodeCode1", Request.QueryString("RefCode"))

        Dim chksel As CheckBox
        Dim marketstr As String = ""
        Dim lblcode As Label

        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            lblcode = gvRow1.FindControl("lblcode")
            If chksel.Checked = True Then
                marketstr = marketstr + ";" + lblcode.Text
            End If
        Next


        If marketstr.Length > 0 Then
            marketstr = marketstr.Substring(1, marketstr.Length - 1)
        End If


        Response.Redirect("OthPricelistSellingRates1.aspx?State=" & CType(ViewState("OthPLsellingRate2State"), String) & "&RefCode=" & CType(ViewState("OthPLsellingRate2Code"), String) &
                          "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text &
                          "&SupplierType=" & ddlSPCode.Items(ddlSPCode.SelectedIndex).Text & "&SupplierTypeName=" & ddlSPName.Items(ddlSPName.SelectedIndex).Text &
                          "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text &
                          "&SupplierAgentName=" & ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCode.Value &
                          "&CurrencyName=" & ddlCurrencyCode.Value & "&SubSeasonCode=" & ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text &
                          "&SubSeasonName=" & ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text &
                          "&PListCode=" & txtPLCode.Text & "&sellcode=" & rdSellingList.SelectedValue, False)
        ' & "&week1=" & week1 &                          "&week2=" & week2 & "&manual=" & manual
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Dim gvrow As GridViewRow
        'Try
        '    Dim j As Long = 1
        '    Dim txt As TextBox
        '    Dim cnt As Long
        '    Dim chksel As CheckBox
        '    Dim lblcode As Label
        '    Dim srno As Long = 0
        '    Dim hotelcategory As String = ""
        '    j = 0
        '    gv_SearchResult.Visible = True
        '    cnt = gv_SearchResult.Columns.Count - 1
        '    Dim n As Long = 0
        '    Dim k As Long = 0
        '    Dim a As Long = cnt - 7
        '    Dim b As Long = 0
        '    Dim header As Long = 0
        '    Dim heading(cnt + 1) As String
        '    Dim strTemp As String = ""
        '    'If btnSave.Text = "Save" Then

        '    'Dim ErrMsg As String
        '    Try
        '        For header = 0 To cnt - 4
        '            heading(header) = FindHeaderTextbox("txtHead" & header, "")
        '        Next
        '    Catch ex As Exception

        '    End Try

        '    Dim m As Long = 0

        '    'Dim GvRow1 As GridViewRow
        '    Try
        '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        '        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

        '        For Each Me.gvRow1 In gv_Market.Rows
        '            chksel = gvRow1.FindControl("chkSelect")
        '            lblcode = gvRow1.FindControl("lblcode")
        '            If chksel.Checked = True Then

        '                j = 0
        '                n = 0
        '                m = 0
        '                b = 0

        '                For Each gvrow In gv_SearchResult.Rows
        '                    If n = 0 Then
        '                        For j = 0 To cnt - 4
        '                            If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
        '                            Else
        '                                mySqlCmd = New SqlCommand("sp_update_cplistdnew", mySqlConn, sqlTrans)
        '                                mySqlCmd.CommandType = CommandType.StoredProcedure
        '                                mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPLCode.Text.Trim, String)

        '                                mySqlCmd.Parameters.Add(New SqlParameter("@oclineno", SqlDbType.Int)).Value = CType(gvrow.Cells(0).Text, Long)
        '                                If CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String) = "[Select]" Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = DBNull.Value
        '                                Else
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)
        '                                End If

        '                                If CType(lblcode.Text, String) = "" Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
        '                                Else
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String)
        '                                End If

        '                                'strTemp = CType(gvrow.Cells(1).Text, String)
        '                                'If strTemp.Contains("M&amp;A") = True Then
        '                                '    strTemp = strTemp.Replace("M&amp;A", "M&A")
        '                                'End If
        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = strTemp 'CType(gvrow.Cells(1).Text, String)

        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)
        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(3).Text, String)
        '                                txt = gvrow.FindControl("txt" & b + a + 1)
        '                                If txt.Text Is Nothing Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 9)).Value = 0
        '                                Else
        '                                    If txt.Text = "" Then
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 9)).Value = 0
        '                                    Else
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
        '                                    End If

        '                                End If

        '                                txt = gvrow.FindControl("txt" & j)
        '                                If txt.Text Is Nothing Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
        '                                Else
        '                                    If txt.Text = "" Then
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
        '                                    Else
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)
        '                                    End If

        '                                End If
        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.VarChar, 20)).Value = CType(lblSellingCode.Text.Trim(), String)



        '                                If chkAllow.Checked = True Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@allow", SqlDbType.Int)).Value = 1
        '                                Else
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@allow", SqlDbType.Int)).Value = 0
        '                                End If


        '                                Dim paramErrMsg As New SqlParameter
        '                                paramErrMsg.ParameterName = "@errmsg"
        '                                paramErrMsg.Direction = ParameterDirection.Output
        '                                paramErrMsg.DbType = DbType.String
        '                                paramErrMsg.Size = 100
        '                                mySqlCmd.Parameters.Add(paramErrMsg)

        '                                mySqlCmd.ExecuteNonQuery()

        '                                If IsDBNull(paramErrMsg.Value) = False Then
        '                                    txtErrMsg.Value = paramErrMsg.Value
        '                                    If CType(txtErrMsg.Value, String) <> "" Then
        '                                        sqlTrans.Rollback()
        '                                        If mySqlConn.State = ConnectionState.Open Then
        '                                            clsDBConnect.dbConnectionClose(mySqlConn)
        '                                        End If
        '                                        FillGridAsPrices()
        '                                        chkAllow.Visible = True
        '                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & txtErrMsg.Value.Replace("'", " ") & "');", True)
        '                                        Exit Sub
        '                                    End If
        '                                End If

        '                            End If
        '                        Next
        '                        m = j
        '                    Else
        '                        k = 0
        '                        For j = n To (m + n) - 1
        '                            ' If heading(k) = "Y/n" Or heading(k) = "Pkg" Or heading(k) = "Canc Days" Or heading(j) = "Compl Nights" Or heading(k) = "Remark" Then
        '                            If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
        '                            Else
        '                                'txt = gvrow.FindControl("txt" & j)
        '                                mySqlCmd = New SqlCommand("sp_update_cplistdnew", mySqlConn, sqlTrans)
        '                                mySqlCmd.CommandType = CommandType.StoredProcedure
        '                                mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPLCode.Text.Trim, String)
        '                                'If ddlPriceList.SelectedValue = "Normal Rates 1 Night" Then
        '                                '    mySqlCmd.Parameters.Add(New SqlParameter("@plisttype", SqlDbType.Int, 9)).Value = 0
        '                                'ElseIf ddlPriceList.SelectedValue = "Weekly Rates 7 Nights" Then
        '                                '    mySqlCmd.Parameters.Add(New SqlParameter("@plisttype", SqlDbType.Int, 9)).Value = 1
        '                                'ElseIf ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Then
        '                                '    mySqlCmd.Parameters.Add(New SqlParameter("@plisttype", SqlDbType.Int, 9)).Value = 2
        '                                'ElseIf ddlPriceList.SelectedValue = "Normal Rates > 1 Night" Then
        '                                '    mySqlCmd.Parameters.Add(New SqlParameter("@plisttype", SqlDbType.Int, 9)).Value = 3
        '                                'ElseIf ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
        '                                '    mySqlCmd.Parameters.Add(New SqlParameter("@plisttype", SqlDbType.Int, 9)).Value = 4
        '                                'End If

        '                                mySqlCmd.Parameters.Add(New SqlParameter("@oclineno", SqlDbType.Int)).Value = CType(gvrow.Cells(0).Text, Long)
        '                                If CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String) = "[Select]" Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = DBNull.Value
        '                                Else
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)
        '                                End If

        '                                If CType(lblcode.Text, String) = "" Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
        '                                Else
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String)
        '                                End If

        '                                'strTemp = CType(gvrow.Cells(1).Text, String)
        '                                'If strTemp.Contains("M&amp;A") = True Then
        '                                '    strTemp = strTemp.Replace("M&amp;A", "M&A")
        '                                'End If
        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = strTemp
        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)
        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(3).Text, String)
        '                                txt = gvrow.FindControl("txt" & b + a + 1)
        '                                If txt.Text Is Nothing Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 9)).Value = 0
        '                                Else
        '                                    If txt.Text = "" Then
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 9)).Value = 0
        '                                    Else
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
        '                                    End If

        '                                End If
        '                                'If txt.Text <> Nothing Then
        '                                '    If txt.Text = "" Then

        '                                '    Else

        '                                '    End If
        '                                'End If
        '                                'txt = gvrow.FindControl("txt" & b + a + 2)
        '                                'If txt.Text = "" Then
        '                                '    mySqlCmd.Parameters.Add(New SqlParameter("@confdays", SqlDbType.Int, 9)).Value = DBNull.Value
        '                                'Else
        '                                '    mySqlCmd.Parameters.Add(New SqlParameter("@confdays", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
        '                                'End If

        '                                txt = gvrow.FindControl("txt" & j)
        '                                If txt.Text Is Nothing Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
        '                                Else
        '                                    If txt.Text = "" Then
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
        '                                    Else
        '                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)
        '                                    End If
        '                                End If


        '                                'mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.VarChar, 20)).Value = CType(lblSellingCode.Text.Trim(), String)

        '                                If chkAllow.Checked = True Then
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@allow", SqlDbType.Int)).Value = 1
        '                                Else
        '                                    mySqlCmd.Parameters.Add(New SqlParameter("@allow", SqlDbType.Int)).Value = 0
        '                                End If


        '                                Dim paramErrMsg As New SqlParameter
        '                                paramErrMsg.ParameterName = "@errmsg"
        '                                paramErrMsg.Direction = ParameterDirection.Output
        '                                paramErrMsg.DbType = DbType.String
        '                                paramErrMsg.Size = 100
        '                                mySqlCmd.Parameters.Add(paramErrMsg)

        '                                mySqlCmd.ExecuteNonQuery()

        '                                If IsDBNull(paramErrMsg.Value) = False Then
        '                                    txtErrMsg.Value = paramErrMsg.Value
        '                                    If CType(txtErrMsg.Value, String) <> "" Then
        '                                        sqlTrans.Rollback()
        '                                        If mySqlConn.State = ConnectionState.Open Then
        '                                            clsDBConnect.dbConnectionClose(mySqlConn)
        '                                        End If
        '                                        FillGridAsPrices()
        '                                        chkAllow.Visible = True
        '                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & txtErrMsg.Value.Replace("'", " ") & "');", True)
        '                                        Exit Sub
        '                                    End If
        '                                End If

        '                            End If
        '                            k = k + 1
        '                        Next
        '                    End If
        '                    b = j
        '                    n = j
        '                    '   m = j
        '                Next

        '            End If
        '        Next


        '        sqlTrans.Commit()    'SQl Tarn Commit
        '        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
        '        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
        '        clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
        '        Session("SesionPlistCode") = txtPLCode.Text

        '    Catch ex As Exception
        '        sqlTrans.Rollback()
        '        If mySqlConn.State = ConnectionState.Open Then
        '            clsDBConnect.dbConnectionClose(mySqlConn)
        '        End If

        '        objUtils.WritErrorLog("HeaderRsellingcode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        '    End Try


        '    'Response.Redirect("HederRsellingcode1.aspx", Fals
        '    Dim marketstr As String = ""


        '    For Each Me.gvRow1 In gv_Market.Rows
        '        chksel = gvRow1.FindControl("chkSelect")
        '        lblcode = gvRow1.FindControl("lblcode")
        '        If chksel.Checked = True Then
        '            marketstr = marketstr + ";" + lblcode.Text
        '        End If
        '    Next


        '    If marketstr.Length > 0 Then
        '        marketstr = marketstr.Substring(1, marketstr.Length - 1)
        '    End If

        '    Response.Redirect("TrfPricelistSellingRates.aspx?State=" & CType(ViewState("TrfPLsellingRate1codeState1"), String) & "&RefCode=" & CType(ViewState("TrfPLsellingRate1codeCode1"), String) &
        '                      "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text &
        '                      "&SupplierType=" & ddlSPCode.Items(ddlSPCode.SelectedIndex).Text & "&SupplierTypeName=" & ddlSPName.Items(ddlSPName.SelectedIndex).Text & "&Market=" & marketstr &
        '                      "&SuppierAgent=" & ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text & "&SupplierAgentName=" & ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text &
        '                      "&CurrencyCode=" & ddlCurrencyCode.Value & "&CurrencyName=" & ddlCurrencyCode.Value & "&SubSeasonCode=" & ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text &
        '                      "&SubSeasonName=" & ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text & "&PListCode=" & txtPLCode.Text, False)
        '    'End If
        'Catch ex As Exception
        '    If mySqlConn.State = ConnectionState.Open Then
        '        clsDBConnect.dbConnectionClose(mySqlConn)
        '    End If
        '    objUtils.WritErrorLog("HeaderRsellingcode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try

    End Sub

    Protected Sub tbnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbnClose.Click
        Session("SesionPlistCode") = ""
        Session("BackPage") = ""
        ' Response.Redirect("PriceList.aspx")
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('OthPriceListWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Private Sub FillSellingCode()
        Dim StrQry As String = "select distinct sellcode from othplisth_convrates  where oplistcode ='" & CType(Session("SesionPlistCode"), String) & "' order by sellcode"
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            rdSellingList.DataSource = mySqlReader
            rdSellingList.DataTextField = mySqlReader("sellcode")
            rdSellingList.DataValueField = mySqlReader("sellcode")
            rdSellingList.DataBind()
            mySqlConn.Close()
        Catch ex As Exception
            mySqlConn.Close()
        End Try
    End Sub

    Private Sub ShowSellingRadioBtns(ByVal RefCode As String)
        Dim mktlist As String = ""
        Dim strQry As String = ""
        'Dim chkSel As CheckBox
        'Dim lblcode As Label
        'For Each GvRow1 In gv_Market.Rows
        '    chkSel = gvRow1.FindControl("chkSelect")
        '    lblcode = gvRow1.FindControl("lblcode")
        '    If chkSel.Checked = True Then
        '        If mktlist = "" Then
        '            mktlist = GvRow1.Cells(1).Text
        '        Else
        '            mktlist = mktlist & "','" & lblcode.Text.Trim
        '        End If
        '    End If
        'Next

        mktlist = getmarket()
        'strQry = "select distinct a.selltype as sellcode,b.plgrpcode as plgrpcode  from cplistdnew  a inner join sellmast b on a.selltype=b.sellcode where  a.plistcode='" & RefCode & "' and a.selltype<>'NET COST' and b.plgrpcode in (" & mktlist & ")  "
        'strQry = strQry & " union all select distinct a.selltype as sellcode,b.plgrpcode as plgrpcode  from cplistdwknew  a inner join sellmast b on a.selltype=b.sellcode where  a.plistcode='" & RefCode & "' and a.selltype<>'NET COST' and b.plgrpcode in (" & mktlist & ")  "
        strQry = "select distinct a.selltype as sellcode,b.plgrpcode as plgrpcode  from othplist_selld  a inner join sellmast b on a.selltype=b.sellcode where  a.oplistcode='" & RefCode & "' and a.selltype<>'NET COST' and b.plgrpcode in (" & mktlist & ")  "
        strQry = strQry & " order by a.selltype"
        Try

            Dim ds As New DataSet
            Dim i As Integer = 0

            Dim oldgrpcode As String = ""
            Dim tr As New TableRow()
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry)
            tbl.EnableViewState = True

            For i = 0 To ds.Tables(0).Rows.Count - 1
                Dim rb As New RadioButton
                rb.EnableViewState = True
                If oldgrpcode <> ds.Tables(0).Rows(i)("plgrpcode").ToString Then
                    tr = New TableRow()
                    oldgrpcode = ds.Tables(0).Rows(i)("plgrpcode").ToString
                End If
                Dim td As New TableCell()
                rb.GroupName = "sellgroup"
                rb.Text = ds.Tables(0).Rows(i)("sellcode").ToString
                td.Controls.Add(rb)
                tr.Cells.Add(td)
                tbl.Rows.Add(tr)

                If Request.QueryString("sellcode") <> Nothing Then
                    If Request.QueryString("sellcode") = ds.Tables(0).Rows(i)("sellcode").ToString Then
                        rb.Checked = True
                    End If
                End If
            Next

            'rdbtnlistSell.DataSource = mySqlReader
            'rdbtnlistSell.DataTextField = ("sellcode")
            'rdbtnlistSell.DataValueField = ("sellcode")
            'rdbtnlistSell.DataBind()
            'mySqlConn.Close()
        Catch ex As Exception
            mySqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    'Private Sub GetWeekEndValues(ByVal PCode As String)

    '    ' Dim strCtryCode As String = ""
    '    Dim strFromValue As String = ""
    '    Dim strToValue As String = ""
    '    'strCtryCode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"partymast", "ctrycode", "partycode", PCode)
    '    'select wkfrmday1,wktoday1,wkfrmday2,wktoday2 from partymast where partycode=

    '    If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday1", "partycode", PCode)) = False Then
    '        strFromValue = " From :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday1", "partycode", PCode)
    '    End If
    '    If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday1", "partycode", PCode)) = False Then
    '        strFromValue = strFromValue & " ,To :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday1", "partycode", PCode)
    '    End If
    '    If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday2", "ctrycode", PCode)) = False Then
    '        strToValue = "From :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday2", "partycode", PCode)
    '    End If
    '    If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday2", "partycode", PCode)) = False Then
    '        strToValue = strToValue & " , To :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday2", "partycode", PCode)
    '    End If
    '    lblWEO1.Text = strFromValue
    '    lblWEO2.Text = strToValue


    '    'lblWeekEnd1.Text = strFromValue
    '    'lblWeekEnd2.Text = strToValue

    'End Sub


    Private Sub ShowDates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othplisth_dates Where oplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromDate")
                        dpTDate = gvRow.FindControl("txtToDate")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                dpFDate.Text = Format("U", mySqlReader("frmdate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = Format("U", mySqlReader("todate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelistSellingRate1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If ViewState("OthPLsellingRate2Code") <> Nothing Then
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select count(*) from othplisth_dates where oplistcode='" + ViewState("OthPLsellingRate2Code") + "'"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            lngcnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()
        End If

        If blnload = True Then
            lngcnt = lngcnt '10
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub


    Private Sub FillGridAsPrices()
        '        **************
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long
        Dim gvrow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        gv_SearchResult.Visible = True
        cnt = gv_SearchResult.Columns.Count
        Dim s As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        For header = 0 To cnt - 5
            txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
            If txt.Text <> Nothing Then
                heading(header) = txt.Text
            End If
        Next

        Dim m As Long = 0
        Dim othcatcode As String = ""
        Dim value As String = ""
        Dim Linno As Integer
        Dim StrQry As String
        Dim headerlabel As New TextBox
        Dim myConn As New SqlConnection
        Dim myCmd As New SqlCommand
        Dim myReader As SqlDataReader
        Dim StrQryTemp As String
        Dim cvalue As String = ""
        'select option_selected from reservation_parameters where param_id=525
        Dim profitperc As Long = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 525)
        Dim convrate As Decimal
        Try
            'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
            '    StrQry = "select distinct clineno from cplistdwknew where plistcode='" & txtBlockCode.Value & "' and selltype='" & lblSellingCode.Text.Trim & "'"
            'Else
            StrQry = "select distinct oclineno from othplist_selld where oplistcode='" & txtPLCode.Text & "' and selltype='" & lblSellingCode.Text.Trim & "'"
            'End If
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("oclineno")) = False Then
                            Linno = mySqlReader("oclineno")
                        End If
                        If gvrow.Cells(0).Text = Linno Then

                            convrate = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "othplisth_convrates", "convrate", "oplistcode='" & txtPLCode.Text & "' and sellcode='" & lblSellingCode.Text.Trim & "'")

                            'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
                            '    StrQryTemp = "select * from cplistdwknew where plistcode='" & txtBlockCode.Value & "' and selltype='" & lblSellingCode.Text.Trim & "' and clineno='" & Linno & "'"
                            'Else
                            '    StrQryTemp = "select * from cplistdnew where plistcode='" & txtBlockCode.Value & "' and selltype='" & lblSellingCode.Text.Trim & "'and clineno='" & Linno & "'"
                            'End If
                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("othcatcode")) = False Then
                                        othcatcode = myReader("othcatcode")
                                    End If

                                    'If IsDBNull(myReader("price")) = False Then
                                    '    value = myReader("price")
                                    'Else
                                    '    value = ""
                                    'End If
                                    If IsDBNull(myReader("tccostprice")) = False Then
                                        cvalue = myReader("tcostprice")
                                    Else
                                        cvalue = ""
                                    End If

                                    For j = 0 To cnt - 5
                                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                        Else
                                            For s = 0 To gv_SearchResult.Columns.Count - 6
                                                headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = othcatcode Then
                                                        If gvrow.RowIndex = 0 Then
                                                            txt = gvrow.FindControl("txt" & s)
                                                        Else
                                                            txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                        End If
                                                        txt.BackColor = Drawing.Color.White
                                                        If Not txt Is Nothing Then
                                                            'If Math.Round(txt.Text * convrate) > value Then
                                                            '    txt.BackColor = Drawing.Color.LightCoral
                                                            'ElseIf Math.Round(txt.Text * convrate * (100 + profitperc)) > value Then
                                                            '    txt.BackColor = Drawing.Color.LightBlue
                                                            'End If
                                                            If CDbl(txt.Text) > 0 Then
                                                                If Math.Round(cvalue * convrate, 0) > CDbl(txt.Text) Then
                                                                    If txt Is Nothing Then
                                                                    Else
                                                                        txt.BackColor = Drawing.Color.LightCoral
                                                                    End If
                                                                ElseIf Math.Round(cvalue * convrate * (100 + profitperc) / 100) > CDbl(txt.Text) Then
                                                                    If txt Is Nothing Then
                                                                    Else
                                                                        txt.BackColor = Drawing.Color.LightBlue
                                                                    End If
                                                                End If

                                                            End If
                                                        End If


                                                        GoTo go1
                                                    End If
                                                End If
                                            Next
                                        End If
                                    Next
go1:                            End While
                            End If
                            clsDBConnect.dbConnectionClose(myConn)
                            clsDBConnect.dbCommandClose(myCmd)
                            clsDBConnect.dbReaderClose(myReader)
                        End If
                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)


        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricelistSellingRates2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub

#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        gv_Market.Visible = True
        If gv_Market.PageIndex < 0 Then
            gv_Market.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
            strSqlQry = strSqlQry & " and plgrpcode in (select plgrpcode from othplist_selld  where oplistcode ='" + txtPLCode.Text.Trim + "') ORDER BY " & strorderby & " " & strsortorder
            'strSqlQry = strSqlQry & " union all select plgrpcode from cplistdwknew  where plistcode ='" + txtPLCode.Text.Trim + "')    " 
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_Market.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Market.DataBind()

            Else
                gv_Market.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricelistSellingRates1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
    End Sub
#End Region
    Private Function getmarket() As String
        Dim chksel As CheckBox
        Dim mktcode As String = ""
        Dim lblcode As Label
        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            lblcode = gvRow1.FindControl("lblcode")
            If chksel.Checked = True Then
                mktcode = mktcode + "'" + lblcode.Text.Trim + "'" + ","
            End If
        Next
        If mktcode.Length > 0 Then
            mktcode = mktcode.Substring(0, mktcode.Length - 1)
        End If

        Return mktcode
    End Function

    Protected Sub btnCheckPrices_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckPrices.Click
        FillGridAsPrices()
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=HeaderRsellingcode','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
