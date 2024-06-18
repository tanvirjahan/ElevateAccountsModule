Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class ExcursionModule_ExcursionUserControls_wucExcursionRequesySubEntry
    Inherits System.Web.UI.UserControl
    Dim msgError As String
    Private IsNew As Boolean = False
    Dim mObjUtilities As New clsUtils
    Dim mDDLSuplierFlg As String = String.Empty
    Dim mExcID As String = String.Empty
    Dim mRLineNo As String = String.Empty
    Public ExcursionType As String = String.Empty

    Public Adult As Integer = 0
    Public Child As Integer = 0
    Public TourDate As String = String.Empty

    Public isGroupReserv As Boolean = False
    Dim dtMC As DataTable

    Dim myCommand As SqlCommand
    Dim SqlConn As SqlConnection


    Dim ddlSupplier As DropDownList
    Dim ddlExcType As DropDownList
    Dim txtAdultCostRate As TextBox
    Dim txtChildCostRate As TextBox
    Dim txtCostCurrency As TextBox
    Dim txtConversionRate As TextBox
    Dim txtCostValue As TextBox
    Dim txtTourDate As TextBox
    Dim txtRemarks As TextBox

    Dim hdnTourDate As HiddenField
    Dim hdnRemarks As HiddenField
    Dim hdnRowId As HiddenField
    Dim hdnSupplier As HiddenField
    Dim hdnExcType As HiddenField
    Dim hdnAdultCostRate As HiddenField
    Dim hdnChildCostRate As HiddenField
    Dim hdnCostCurrency As HiddenField
    Dim hdnConversionRate As HiddenField
    Dim hdnCostValue As HiddenField

    Dim txtAdult As TextBox
    Dim txtChild As TextBox
    Dim txtnoofunits As TextBox

    Dim hdnAdult As HiddenField
    Dim hdnChild As HiddenField
    Dim hdnnoofunits As HiddenField
    Public IsGridUpdated As Boolean = False


    Public Event PassTotalValue(ByVal Totalval As String)
    Private CurState As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                'BindGrid()
            End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub

    Public Sub BindGrid(ByVal excID As String, ByVal rlineNo As String, Optional ByVal state As String = "")
        Try
            mExcID = excID
            mRLineNo = rlineNo
            CurState = state
            hdnDefaultExcType.Value = ExcursionType
            If isGroupReserv Then
                hdnIsGroupReservation.Value = "1"
            Else
                hdnIsGroupReservation.Value = "0"
            End If

            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim ds2 As New DataSet
            Dim strQry As String = "SELECT partycode,partyname FROM partymast Where sptypecode NOT IN (SELECT option_selected FROM reservation_parameters Where param_id =458)"
            Dim strQry1 As String = "select othtypname,othtypcode from othtypmast"
            ds = mObjUtilities.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry)
            ds2 = mObjUtilities.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry1)


            hdnTrnsfrType.Value = mObjUtilities.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1124)

            ViewState("SourceForddlSupplier") = ds
            ViewState("SourceForddlExcType") = ds2

            If state <> "AddRow" Then
                If rlineNo = "" Then
                    Dim rlinenotemp As Integer = 0
                    rlinenotemp = CType(mObjUtilities.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select max(rlineno)+1 rlineno from excursions_detail where excid='" & excID & "'"), Integer)
                    Session("ExcursionRequestSubEntryLineNo") = rlinenotemp
                End If
            End If

            If Not Session("TempMultiCostGrid") Is Nothing Then
                grdMultipleCost.DataSource = Nothing
                grdMultipleCost.DataBind()
                grdTempRowsbind()                
            Else
                ds1 = mObjUtilities.ExecuteQuerySqlnew(Session("dbconnectionName"), "SELECT RowId,excid,rlineno,othtypcode,Supplier, CAST(AdultCostRate AS DECIMAL(18,2)) AdultCostRate,CAST(ChildCostRate AS DECIMAL(18,2)) ChildCostRate,CostCurrency,ConversionRate,CAST(CostValue AS DECIMAL(18,2)) AS CostValue,noofunits,remarks FROM excursions_cost_detail_temp Where excid ='" & excID & "' AND rlineno='" & rlineNo & "' ")

                If ds1.Tables(0).Rows.Count = 0 Then
                    IsNew = True
                    grdEmptyRowbind(3)
                Else
                    IsNew = False
                    With grdMultipleCost
                        .DataSource = ds1
                        .DataBind()
                    End With
                End If
            End If
            ''' Added shahul 13/10/2015
            Dim adultcost As Double
            Dim childcostvalue As Double

            For Each row As GridViewRow In grdMultipleCost.Rows
                txtAdultCostRate = CType(row.FindControl("txtAdultCostRate"), TextBox)
                txtChildCostRate = CType(row.FindControl("txtChildCostRate"), TextBox)
                txtChildCostRate = CType(row.FindControl("txtChildCostRate"), TextBox)
                txtCostCurrency = CType(row.FindControl("txtCostCurrency"), TextBox)
                txtConversionRate = CType(row.FindControl("txtConversionRate"), TextBox)
                txtCostValue = CType(row.FindControl("txtCostValue"), TextBox)
                txtAdult = CType(row.FindControl("txtAdult"), TextBox)
                txtChild = CType(row.FindControl("txtChild"), TextBox)
                txtnoofunits = CType(row.FindControl("txtnoofunits"), TextBox)
                ddlExcType = CType(row.FindControl("ddlExcType"), DropDownList)
                If hdnTrnsfrType.Value = ddlExcType.Text Then
                    txtCostValue.Text = CType(Val(txtAdultCostRate.Text), Double) * CType(Val(txtnoofunits.Text), Double)
                Else
                    adultcost = CType(Val(txtAdult.Text), Double) * CType(Val(txtAdultCostRate.Text), Double) * CType(Val(txtnoofunits.Text), Double)
                    childcostvalue = CType(Val(txtChild.Text), Double) * CType(Val(txtChildCostRate.Text), Double) * CType(Val(txtnoofunits.Text), Double)
                    txtCostValue.Text = Val(adultcost) + Val(childcostvalue)
                End If
                
            Next
            '''''''''''''''''''''''''''
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "CalCulateTotalValue", "CalculateTotal();", True)



        Catch ex As Exception
            msgError = "In Bind Grid WUC " & ex.ToString()
        End Try
    End Sub


    Public Sub BindGroupReservGrid(ByVal excID As String, ByVal rlineNo As String, Optional ByVal state As String = "")
        Try
            mExcID = excID
            mRLineNo = rlineNo
            CurState = state
            hdnDefaultExcType.Value = ExcursionType
            If isGroupReserv Then
                hdnIsGroupReservation.Value = "1"
            Else
                hdnIsGroupReservation.Value = "0"
            End If

            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim ds2 As New DataSet
            Dim strQry As String = "SELECT partycode,partyname FROM partymast Where sptypecode NOT IN (SELECT option_selected FROM reservation_parameters Where param_id =458)"
            Dim strQry1 As String = "select othtypname,othtypcode from othtypmast"
            ds = mObjUtilities.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry)
            ds2 = mObjUtilities.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry1)


            hdnTrnsfrType.Value = mObjUtilities.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1124)

            ViewState("SourceForddlSupplier") = ds
            ViewState("SourceForddlExcType") = ds2

            Session("ExcursionRequestSubEntryLineNo") = rlineNo
            If state <> "AddRow" Then                
                If rlineNo = "" Then
                    Dim rlinenotemp As Integer = 0
                    rlinenotemp = CType(mObjUtilities.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select max(rlineno)+1 rlineno from excursions_detail where excid='" & excID & "'"), Integer)
                    Session("ExcursionRequestSubEntryLineNo") = rlinenotemp
                End If
            End If

            If Not Session("TempMultiCostGrid") Is Nothing Then
                If DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(0).FindControl("hdnrlineno"), HiddenField).Value = rlineNo Then
                    grdMultipleCost.DataSource = Nothing
                    grdMultipleCost.DataBind()
                    grdTempRowsbind()
                Else
                    GoTo chkDstables
                End If
            Else
chkDstables:    If Session("dsTables") Is Nothing Then
                    GoTo lblFromTempTable
                Else
                    'filtering by rlineno
                    Dim dsTables As New DataSet
                    dsTables = CType(Session("dsTables"), DataSet)
                    Dim tmpTble As New DataTable
                    Dim IsFillTable As Boolean = False
                    If Not dsTables Is Nothing Then
                        For i As Integer = 0 To dsTables.Tables.Count - 1
                            If dsTables.Tables(i).Rows.Count > 0 Then
                                If dsTables.Tables(i).Rows(0).Item("rlineno") = rlineNo Then
                                    tmpTble = dsTables.Tables(i).Copy
                                    ds1.Tables.Add(tmpTble)
                                    IsFillTable = True
                                    'GoTo lblSkipFromTempTable
                                End If
                            End If
                        Next
                    End If
                    If IsFillTable Then GoTo lblSkipFromTempTable
                End If

lblFromTempTable:
                ds1 = mObjUtilities.ExecuteQuerySqlnew(Session("dbconnectionName"), "SELECT RowId,excid,rlineno,othtypcode,Supplier, CAST(AdultCostRate AS DECIMAL(18,2)) AdultCostRate,CAST(ChildCostRate AS DECIMAL(18,2)) ChildCostRate,CostCurrency,ConversionRate,CAST(CostValue AS DECIMAL(18,2)) AS CostValue,noofunits,remarks FROM excursions_cost_detail_temp Where excid ='" & excID & "' AND rlineno='" & rlineNo & "' ")

lblSkipFromTempTable:

                If ds1.Tables.Count > 0 Then
                    If ds1.Tables(0).Rows.Count = 0 Then
                        IsNew = True
                        grdEmptyRowbind(3)
                    Else
                        Dim dsTblCount As Integer = ds1.Tables.Count
                        IsNew = False
                        With grdMultipleCost
                            .DataSource = ds1.Tables(dsTblCount - 1)
                            .DataBind()
                        End With
                    End If
                Else
                    IsNew = True
                    grdEmptyRowbind(3)
                End If

            End If

            ''' Added shahul 14/10/2015
            Dim adultcost As Double
            Dim childcostvalue As Double

            For Each row As GridViewRow In grdMultipleCost.Rows
                txtAdultCostRate = CType(row.FindControl("txtAdultCostRate"), TextBox)
                txtChildCostRate = CType(row.FindControl("txtChildCostRate"), TextBox)
                txtChildCostRate = CType(row.FindControl("txtChildCostRate"), TextBox)
                txtCostCurrency = CType(row.FindControl("txtCostCurrency"), TextBox)
                txtConversionRate = CType(row.FindControl("txtConversionRate"), TextBox)
                txtCostValue = CType(row.FindControl("txtCostValue"), TextBox)
                txtAdult = CType(row.FindControl("txtAdult"), TextBox)
                txtChild = CType(row.FindControl("txtChild"), TextBox)
                txtnoofunits = CType(row.FindControl("txtnoofunits"), TextBox)
                ddlExcType = CType(row.FindControl("ddlExcType"), DropDownList)
                If hdnTrnsfrType.Value = ddlExcType.Text Then
                    txtCostValue.Text = CType(Val(txtAdultCostRate.Text), Double) * CType(Val(txtnoofunits.Text), Double)
                Else
                    adultcost = CType(Val(txtAdult.Text), Double) * CType(Val(txtAdultCostRate.Text), Double) * CType(Val(txtnoofunits.Text), Double)
                    childcostvalue = CType(Val(txtChild.Text), Double) * CType(Val(txtChildCostRate.Text), Double) * CType(Val(txtnoofunits.Text), Double)
                    txtCostValue.Text = Val(adultcost) + Val(childcostvalue)
                End If

            Next
            '''''''''''''''''''''''''''

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "CalCulateTotalValue", "CalculateTotal();", True)



        Catch ex As Exception
            msgError = "In Bind Grid WUC " & ex.ToString()
        End Try
    End Sub

    Private Sub grdTempRowsbind()
        Try
            Dim dtMultipleCost As DataTable
            dtMultipleCost = New DataTable
            dtMultipleCost.Columns.Add("RowId")
            dtMultipleCost.Columns.Add("Supplier")
            dtMultipleCost.Columns.Add("AdultCostRate")
            dtMultipleCost.Columns.Add("ChildCostRate")
            dtMultipleCost.Columns.Add("CostCurrency")
            dtMultipleCost.Columns.Add("ConversionRate")
            dtMultipleCost.Columns.Add("CostValue")
            dtMultipleCost.Columns.Add("Adult")
            dtMultipleCost.Columns.Add("Child")
            dtMultipleCost.Columns.Add("Noofunits")
            dtMultipleCost.Columns.Add("TourDate")
            dtMultipleCost.Columns.Add("remarks")
            dtMultipleCost.Columns.Add("rlineno")
            dtMultipleCost.Columns.Add("othtypcode")

            Dim dr As DataRow
            Dim mRowCount As Integer = CType(Session("TempMultiCostGrid"), GridView).Rows.Count
            For i = 1 To mRowCount
                dr = dtMultipleCost.NewRow
                hdnRowId = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("hdnRowID"), HiddenField)
                ddlSupplier = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("ddlSupplier"), DropDownList)
                ddlExcType = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("ddlExcType"), DropDownList)
                txtAdultCostRate = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtAdultCostRate"), TextBox)
                txtChildCostRate = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtChildCostRate"), TextBox)
                txtCostCurrency = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtCostCurrency"), TextBox)
                txtConversionRate = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtConversionRate"), TextBox)
                txtCostValue = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtCostValue"), TextBox)

                txtAdult = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtAdult"), TextBox)
                txtChild = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtChild"), TextBox)
                txtnoofunits = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtnoofunits"), TextBox)
                txtTourDate = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtTourDate"), TextBox)
                txtRemarks = DirectCast(CType(Session("TempMultiCostGrid"), GridView).Rows(i - 1).FindControl("txtRemarks"), TextBox)

                dr("RowId") = hdnRowId.Value
                dr("Supplier") = ddlSupplier.SelectedValue
                dr("othtypcode") = ddlExcType.SelectedValue
                dr("AdultCostRate") = txtAdultCostRate.Text
                dr("ChildCostRate") = txtChildCostRate.Text
                dr("CostCurrency") = txtCostCurrency.Text
                dr("ConversionRate") = txtConversionRate.Text
                dr("CostValue") = txtCostValue.Text
                dr("Adult") = txtAdult.Text
                dr("Child") = txtChild.Text
                dr("Noofunits") = txtnoofunits.Text
                dr("TourDate") = txtTourDate.Text
                dr("remarks") = txtRemarks.Text
                dr("rlineno") = CType(Session("ExcursionRequestSubEntryLineNo"), Integer)
                dtMultipleCost.Rows.Add(dr)
            Next
            dtMultipleCost.AcceptChanges()
            grdMultipleCost.DataSource = dtMultipleCost
            grdMultipleCost.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub grdEmptyRowbind(ByVal rowCount As Integer)
        Try
            Dim dtMultipleCost As DataTable
            dtMultipleCost = New DataTable
            dtMultipleCost.Columns.Add("RowId")
            dtMultipleCost.Columns.Add("Supplier")
            dtMultipleCost.Columns.Add("AdultCostRate")
            dtMultipleCost.Columns.Add("ChildCostRate")
            dtMultipleCost.Columns.Add("CostCurrency")
            dtMultipleCost.Columns.Add("ConversionRate")
            dtMultipleCost.Columns.Add("CostValue")
            dtMultipleCost.Columns.Add("Adult")
            dtMultipleCost.Columns.Add("Child")
            dtMultipleCost.Columns.Add("Noofunits")
            dtMultipleCost.Columns.Add("TourDate")
            dtMultipleCost.Columns.Add("remarks")
            dtMultipleCost.Columns.Add("rlineno")
            dtMultipleCost.Columns.Add("othtypcode")

            Dim dr As DataRow
            If IsNew = False Then
                Dim mRowCount As Integer = grdMultipleCost.Rows.Count + 1
                For i = 1 To mRowCount
                    dr = dtMultipleCost.NewRow
                    If i <> mRowCount Then
                        hdnRowId = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("hdnRowID"), HiddenField)
                        ddlSupplier = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("ddlSupplier"), DropDownList)
                        ddlExcType = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("ddlExcType"), DropDownList)
                        txtAdultCostRate = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtAdultCostRate"), TextBox)
                        txtChildCostRate = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtChildCostRate"), TextBox)
                        txtCostCurrency = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtCostCurrency"), TextBox)
                        txtConversionRate = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtConversionRate"), TextBox)
                        txtCostValue = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtCostValue"), TextBox)

                        txtAdult = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtAdult"), TextBox)
                        txtChild = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtChild"), TextBox)
                        txtnoofunits = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtnoofunits"), TextBox)
                        txtTourDate = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtTourDate"), TextBox)
                        txtRemarks = DirectCast(grdMultipleCost.Rows(i - 1).FindControl("txtRemarks"), TextBox)
                        txtAdult.Text = Adult
                        txtChild.Text = Child
                        txtTourDate.Text = TourDate

                        dr("RowId") = hdnRowId.Value
                        dr("Supplier") = ddlSupplier.SelectedValue
                        dr("othtypcode") = ddlExcType.SelectedValue
                        dr("AdultCostRate") = txtAdultCostRate.Text
                        dr("ChildCostRate") = txtChildCostRate.Text
                        dr("CostCurrency") = txtCostCurrency.Text
                        dr("ConversionRate") = txtConversionRate.Text
                        dr("CostValue") = txtCostValue.Text
                        dr("Adult") = txtAdult.Text
                        dr("Child") = txtChild.Text
                        dr("Noofunits") = txtnoofunits.Text
                        dr("TourDate") = txtTourDate.Text
                        dr("remarks") = txtRemarks.Text
                        dr("rlineno") = CType(Session("ExcursionRequestSubEntryLineNo"), Integer)
                    Else
                        dr("Supplier") = ddlSupplier
                        dr("othtypcode") = ddlExcType
                        dr("noofunits") = "1"
                        dr("TourDate") = TourDate
                        dr("Adult") = Adult
                        dr("Child") = Child
                    End If
                    dtMultipleCost.Rows.Add(dr)
                Next
            Else
                For i = 0 To rowCount
                    dr = dtMultipleCost.NewRow
                    Dim ddlSupplier As New DropDownList
                    Dim ddlExcType As New DropDownList
                    dr("Supplier") = ddlSupplier
                    dr("othtypcode") = ddlExcType
                    dr("noofunits") = "1"
                    dr("TourDate") = TourDate
                    dr("Adult") = Adult
                    dr("Child") = Child
                    dtMultipleCost.Rows.Add(dr)
                Next
            End If
            dtMultipleCost.AcceptChanges()
            grdMultipleCost.DataSource = dtMultipleCost
            grdMultipleCost.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub grdMultipleCost_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMultipleCost.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim hdnSuplr As String = String.Empty
                Dim hdnExcTy As String = String.Empty
                ddlSupplier = CType(e.Row.FindControl("ddlSupplier"), DropDownList)
                ddlExcType = CType(e.Row.FindControl("ddlExcType"), DropDownList)
                txtAdultCostRate = CType(e.Row.FindControl("txtAdultCostRate"), TextBox)
                txtChildCostRate = CType(e.Row.FindControl("txtChildCostRate"), TextBox)
                txtChildCostRate = CType(e.Row.FindControl("txtChildCostRate"), TextBox)
                txtCostCurrency = CType(e.Row.FindControl("txtCostCurrency"), TextBox)
                txtConversionRate = CType(e.Row.FindControl("txtConversionRate"), TextBox)
                txtCostValue = CType(e.Row.FindControl("txtCostValue"), TextBox)
                txtAdult = CType(e.Row.FindControl("txtAdult"), TextBox)
                txtChild = CType(e.Row.FindControl("txtChild"), TextBox)
                txtnoofunits = CType(e.Row.FindControl("txtnoofunits"), TextBox)
                txtTourDate = CType(e.Row.FindControl("txtTourDate"), TextBox)
                txtRemarks = CType(e.Row.FindControl("txtRemarks"), TextBox)

                hdnSupplier = CType(e.Row.FindControl("hdnDDLSupplier"), HiddenField)
                hdnExcType = CType(e.Row.FindControl("hdnExcType"), HiddenField)
                hdnAdultCostRate = CType(e.Row.FindControl("hdnAdultCostRate"), HiddenField)
                hdnChildCostRate = CType(e.Row.FindControl("hdnChildCostRate"), HiddenField)
                hdnCostCurrency = CType(e.Row.FindControl("hdnCostCurrency"), HiddenField)
                hdnConversionRate = CType(e.Row.FindControl("hdnConversionRate"), HiddenField)
                hdnCostValue = CType(e.Row.FindControl("hdnCostValue"), HiddenField)

                hdnAdult = CType(e.Row.FindControl("hdnAdult"), HiddenField)
                hdnChild = CType(e.Row.FindControl("hdnAdult"), HiddenField)
                hdnnoofunits = CType(e.Row.FindControl("hdnnoofunits"), HiddenField)
                hdnTourDate = CType(e.Row.FindControl("hdnTourDate"), HiddenField)

                hdnRemarks = CType(e.Row.FindControl("hdnRemarks"), HiddenField)

                hdnSuplr = hdnSupplier.Value
                hdnExcTy = hdnExcType.Value

                txtAdult.Text = Adult
                txtChild.Text = Child
                txtTourDate.Text = TourDate

                ddlSupplier.Attributes.Add("onchange", "OnChangeddlSupplierWuc('" & Session("dbconnectionName") & "','" & ddlSupplier.ClientID & "','" & txtCostCurrency.ClientID & "','" & txtAdultCostRate.ClientID & "','" & txtConversionRate.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnCostCurrency.ClientID & "','" & hdnConversionRate.ClientID & "','" & hdnCostValue.ClientID & "','" & ddlExcType.ClientID & "','" & txtTourDate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & txtnoofunits.ClientID & "','supplier')")

                ddlExcType.Attributes.Add("onchange", "OnChangeddlSupplierWuc('" & Session("dbconnectionName") & "','" & ddlSupplier.ClientID & "','" & txtCostCurrency.ClientID & "','" & txtAdultCostRate.ClientID & "','" & txtConversionRate.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnCostCurrency.ClientID & "','" & hdnConversionRate.ClientID & "','" & hdnCostValue.ClientID & "','" & ddlExcType.ClientID & "','" & txtTourDate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & txtnoofunits.ClientID & "','ExcType')")

                txtAdultCostRate.Attributes.Add("onkeypress", "return checkNumber(event,this)")
                txtChildCostRate.Attributes.Add("onkeypress", "return checkNumber(event,this)")
                txtAdult.Attributes.Add("onkeypress", "return checkNumber(event,this)")
                txtChild.Attributes.Add("onkeypress", "return checkNumber(event,this)")
                txtConversionRate.Attributes.Add("onkeypress", "return checkNumber(event,this)")
                txtCostValue.Attributes.Add("onkeypress", "return checkNumber(event,this)")

                'txtAdultCostRate.Attributes.Add("onchange", "CalculateCostValue('" & Session("dbconnectionName") & "','" & txtCostCurrency.ClientID & "','" & txtAdultCostRate.ClientID & "','" & txtConversionRate.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnCostCurrency.ClientID & "','" & hdnConversionRate.ClientID & "','" & hdnCostValue.ClientID & "')")

                txtAdultCostRate.Attributes.Add("onblur", "CalculateCostValueByUnits('" & Session("dbconnectionName") & "','" & txtAdultCostRate.ClientID & "','" & txtnoofunits.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnnoofunits.ClientID & "','" & hdnCostValue.ClientID & "','" & txtConversionRate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & ddlExcType.ClientID & "')")

                txtChildCostRate.Attributes.Add("onblur", "CalculateCostValueByUnits('" & Session("dbconnectionName") & "','" & txtAdultCostRate.ClientID & "','" & txtnoofunits.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnnoofunits.ClientID & "','" & hdnCostValue.ClientID & "','" & txtConversionRate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & ddlExcType.ClientID & "')")

                txtAdult.Attributes.Add("onblur", "CalculateCostValueByUnits('" & Session("dbconnectionName") & "','" & txtAdultCostRate.ClientID & "','" & txtnoofunits.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnnoofunits.ClientID & "','" & hdnCostValue.ClientID & "','" & txtConversionRate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & ddlExcType.ClientID & "')")

                txtChild.Attributes.Add("onblur", "CalculateCostValueByUnits('" & Session("dbconnectionName") & "','" & txtAdultCostRate.ClientID & "','" & txtnoofunits.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnnoofunits.ClientID & "','" & hdnCostValue.ClientID & "','" & txtConversionRate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & ddlExcType.ClientID & "')")

                txtnoofunits.Attributes.Add("onblur", "CalculateCostValueByUnits('" & Session("dbconnectionName") & "','" & txtAdultCostRate.ClientID & "','" & txtnoofunits.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnnoofunits.ClientID & "','" & hdnCostValue.ClientID & "','" & txtConversionRate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & ddlExcType.ClientID & "')")

                'txtnoofunits.Attributes.Add("onchange", "CalculateCostValueByUnits('" & Session("dbconnectionName") & "','" & txtAdultCostRate.ClientID & "','" & txtnoofunits.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnnoofunits.ClientID & "','" & hdnCostValue.ClientID & "','" & txtConversionRate.ClientID & "','" & txtChildCostRate.ClientID & "')")

                txtConversionRate.Attributes.Add("onblur", "CalculateCostValueByUnits('" & Session("dbconnectionName") & "','" & txtAdultCostRate.ClientID & "','" & txtnoofunits.ClientID & "','" & txtCostValue.ClientID & "','" & hdnSupplier.ClientID & "','" & hdnAdultCostRate.ClientID & "','" & hdnnoofunits.ClientID & "','" & hdnCostValue.ClientID & "','" & txtConversionRate.ClientID & "','" & txtChildCostRate.ClientID & "','" & hdnChildCostRate.ClientID & "','" & txtAdult.ClientID & "','" & txtChild.ClientID & "','" & ddlExcType.ClientID & "')")

                txtCostValue.Attributes.Add("onblur", "CalculateTotal()")



                If Not ViewState("SourceForddlSupplier") Is Nothing Then
                    ddlSupplier.DataSource = ViewState("SourceForddlSupplier")
                    ddlSupplier.DataTextField = "partyname"
                    ddlSupplier.DataValueField = "partycode"
                    ddlSupplier.DataBind()
                    Dim mListItem As New ListItem
                    mListItem.Text = "[Select]"
                    mListItem.Value = "[Select]"
                    ddlSupplier.Items.Add(mListItem)
                    If hdnSuplr = "System.Web.UI.WebControls.DropDownList" Or hdnSuplr = "" Then
                        ddlSupplier.SelectedValue = "[Select]"
                    Else
                        ddlSupplier.SelectedValue = hdnSuplr
                    End If
                End If

                If Not ViewState("SourceForddlExcType") Is Nothing Then
                    ddlExcType.DataSource = ViewState("SourceForddlExcType")
                    ddlExcType.DataTextField = "othtypname"
                    ddlExcType.DataValueField = "othtypcode"
                    ddlExcType.DataBind()
                    Dim mListItem As New ListItem
                    mListItem.Text = "[Select]"
                    mListItem.Value = "[Select]"
                    ddlExcType.Items.Add(mListItem)
                    If IsNew Then
                        hdnExcTy = ExcursionType
                        'Else
                        '    hdnExcTy = ExcursionType
                    End If

                    If hdnExcTy = "System.Web.UI.WebControls.DropDownList" Or hdnExcTy = "" Then
                        ddlExcType.SelectedValue = "[Select]"
                    Else
                        ddlExcType.SelectedValue = hdnExcTy
                    End If
                End If

                If ddlExcType.SelectedValue = hdnTrnsfrType.Value Then
                    txtnoofunits.Enabled = True
                Else
                    txtnoofunits.Enabled = False
                End If

            End If


        Catch ex As Exception

        End Try

    End Sub

    Public Sub AddRow()
        Try
            Dim rowCount As Integer = grdMultipleCost.Rows.Count
            grdEmptyRowbind(rowCount)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub DeleteRow()
        Dim chk As HtmlInputCheckBox
        Dim mGridTotalVal As Decimal = 0.0

        Dim dtMultipleCost As DataTable
        dtMultipleCost = New DataTable
        dtMultipleCost.Columns.Add("RowId")
        dtMultipleCost.Columns.Add("Supplier")
        dtMultipleCost.Columns.Add("AdultCostRate")
        dtMultipleCost.Columns.Add("ChildCostRate")
        dtMultipleCost.Columns.Add("CostCurrency")
        dtMultipleCost.Columns.Add("ConversionRate")
        dtMultipleCost.Columns.Add("CostValue")
        dtMultipleCost.Columns.Add("Adult")
        dtMultipleCost.Columns.Add("Child")
        dtMultipleCost.Columns.Add("Noofunits")
        dtMultipleCost.Columns.Add("TourDate")
        dtMultipleCost.Columns.Add("remarks")
        dtMultipleCost.Columns.Add("rlineno")
        dtMultipleCost.Columns.Add("othtypcode")

        Dim dr As DataRow

        'Session("OldTempMultiCostGrid") = grdMultipleCost

        If grdMultipleCost.Rows.Count > 1 Then
            If Session("ExcursionRequestSubEntryLineNo") <> "" Then
                For i = 0 To grdMultipleCost.Rows.Count - 1
                    chk = CType(grdMultipleCost.Rows(i).FindControl("chk"), HtmlInputCheckBox)
                    If chk.Checked = False Then

                        hdnRowId = CType(grdMultipleCost.Rows(i).FindControl("hdnRowId"), HiddenField)
                        ddlSupplier = CType(grdMultipleCost.Rows(i).FindControl("ddlSupplier"), DropDownList)
                        ddlExcType = CType(grdMultipleCost.Rows(i).FindControl("ddlExcType"), DropDownList)
                        txtAdultCostRate = CType(grdMultipleCost.Rows(i).FindControl("txtAdultCostRate"), TextBox)
                        txtChildCostRate = CType(grdMultipleCost.Rows(i).FindControl("txtChildCostRate"), TextBox)
                        txtCostCurrency = CType(grdMultipleCost.Rows(i).FindControl("txtCostCurrency"), TextBox)
                        txtConversionRate = CType(grdMultipleCost.Rows(i).FindControl("txtConversionRate"), TextBox)
                        txtCostValue = CType(grdMultipleCost.Rows(i).FindControl("txtCostValue"), TextBox)

                        txtAdult = DirectCast(grdMultipleCost.Rows(i).FindControl("txtAdult"), TextBox)
                        txtChild = DirectCast(grdMultipleCost.Rows(i).FindControl("txtChild"), TextBox)
                        txtnoofunits = DirectCast(grdMultipleCost.Rows(i).FindControl("txtnoofunits"), TextBox)
                        txtTourDate = DirectCast(grdMultipleCost.Rows(i).FindControl("txtTourDate"), TextBox)
                        txtRemarks = DirectCast(grdMultipleCost.Rows(i).FindControl("txtRemarks"), TextBox)

                        hdnSupplier = CType(grdMultipleCost.Rows(i).FindControl("hdnDDLSupplier"), HiddenField)
                        hdnExcType = CType(grdMultipleCost.Rows(i).FindControl("hdnExcType"), HiddenField)
                        hdnAdultCostRate = CType(grdMultipleCost.Rows(i).FindControl("hdnAdultCostRate"), HiddenField)
                        hdnChildCostRate = CType(grdMultipleCost.Rows(i).FindControl("hdnChildCostRate"), HiddenField)
                        hdnCostCurrency = CType(grdMultipleCost.Rows(i).FindControl("hdnCostCurrency"), HiddenField)
                        hdnConversionRate = CType(grdMultipleCost.Rows(i).FindControl("hdnConversionRate"), HiddenField)
                        hdnCostValue = CType(grdMultipleCost.Rows(i).FindControl("hdnCostValue"), HiddenField)

                        hdnAdult = DirectCast(grdMultipleCost.Rows(i).FindControl("hdnAdult"), HiddenField)
                        hdnChild = DirectCast(grdMultipleCost.Rows(i).FindControl("hdnChild"), HiddenField)
                        hdnnoofunits = DirectCast(grdMultipleCost.Rows(i).FindControl("hdnnoofunits"), HiddenField)
                        hdnTourDate = DirectCast(grdMultipleCost.Rows(i).FindControl("hdnTourDate"), HiddenField)
                        hdnRemarks = DirectCast(grdMultipleCost.Rows(i).FindControl("hdnRemarks"), HiddenField)                        
                        txtAdult.Text = Adult
                        txtChild.Text = Child
                        txtTourDate.Text = TourDate

                        dr = dtMultipleCost.NewRow
                        dr("RowId") = hdnRowId.Value
                        dr("Supplier") = ddlSupplier.SelectedValue
                        dr("othtypcode") = ddlExcType.SelectedValue
                        dr("AdultCostRate") = txtAdultCostRate.Text
                        dr("ChildCostRate") = txtChildCostRate.Text
                        dr("CostCurrency") = txtCostCurrency.Text
                        dr("ConversionRate") = txtConversionRate.Text
                        dr("CostValue") = txtCostValue.Text
                        dr("Adult") = txtAdult.Text
                        dr("Child") = txtChild.Text
                        dr("Noofunits") = txtnoofunits.Text
                        dr("TourDate") = txtTourDate.Text
                        dr("remarks") = txtRemarks.Text
                        dr("rlineno") = CType(Session("ExcursionRequestSubEntryLineNo"), Integer)

                        dtMultipleCost.Rows.Add(dr)
                        dtMultipleCost.AcceptChanges()

                        If txtCostValue.Text = "" Then txtCostValue.Text = 0
                        mGridTotalVal = mGridTotalVal + CDec(txtCostValue.Text)
                    End If
                Next

            End If
            If dtMultipleCost.Rows.Count > 0 Then
                grdMultipleCost.DataSource = dtMultipleCost
                grdMultipleCost.DataBind()
                txtTotal.Value = mGridTotalVal
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Atleast one row should be left undeleted!');", True)
            End If
        End If
    End Sub

    Public Function SaveMultipleCost(Optional ByVal IsClose As Boolean = False) As Boolean
        Try
            'If CDec(Val(txtTotal.Value)) > 0 Then
            If IsGridUpdated Then
                If grdMultipleCost.Rows.Count > 0 Then
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    Dim i As Integer = 0

                    Dim flag As Boolean = False
                    Session("TempMultiCostGrid") = grdMultipleCost
                    Session("OldTempMultiCostGrid") = grdMultipleCost
                    flag = True

                    If IsClose = False Then
                        If flag Then
                            RaiseEvent PassTotalValue(txtTotal.Value)
                            Return True
                        Else
                            'RaiseEvent PassTotalValue(txtTotal.Value)
                            Return False
                        End If
                    End If
                End If                
            Else
                If Not Session("OldTempMultiCostGrid") Is Nothing Then
                    Session("TempMultiCostGrid") = Session("OldTempMultiCostGrid")
                    RaiseEvent PassTotalValue(txtTotal.Value)
                End If
            End If
            'End If

        Catch ex As Exception
            RaiseEvent PassTotalValue(txtTotal.Value)
            Return False
        End Try
    End Function


    '    excid, rlineno, othtypcode, Supplier, AdultCostRate, ChildCostRate, CostCurrency, ConversionRate, CostValue, noofunits, remarks
    'FROM         excursions_cost_detail_temp

    Public Function ValidateExcursionCostDetails() As Boolean
        Dim mCount As Integer = 0
        Try
            For i = 0 To grdMultipleCost.Rows.Count - 1
                ddlSupplier = CType(grdMultipleCost.Rows(i).FindControl("ddlSupplier"), DropDownList)
                If ddlSupplier.SelectedValue <> "[Select]" Then
                    mCount = mCount + 1
                    Exit For
                End If
            Next


            For i = 0 To grdMultipleCost.Rows.Count - 1
                ddlSupplier = CType(grdMultipleCost.Rows(i).FindControl("ddlSupplier"), DropDownList)
                ddlExcType = CType(grdMultipleCost.Rows(i).FindControl("ddlExcType"), DropDownList)
                txtAdult = CType(grdMultipleCost.Rows(i).FindControl("txtAdult"), TextBox)
                txtChild = CType(grdMultipleCost.Rows(i).FindControl("txtChild"), TextBox)
                txtAdultCostRate = CType(grdMultipleCost.Rows(i).FindControl("txtAdultCostRate"), TextBox)
                txtChildCostRate = CType(grdMultipleCost.Rows(i).FindControl("txtChildCostRate"), TextBox)
                txtnoofunits = CType(grdMultipleCost.Rows(i).FindControl("txtnoofunits"), TextBox)
                txtCostValue = CType(grdMultipleCost.Rows(i).FindControl("txtCostValue"), TextBox)

                If ddlSupplier.SelectedValue <> "[Select]" And Val(txtAdult.Text) > 0 And Val(txtAdultCostRate.Text) = 0 Then
                    Return False
                End If
                If ddlSupplier.SelectedValue <> "[Select]" And ddlExcType.SelectedValue <> hdnTrnsfrType.Value And Val(txtChild.Text) > 0 And Val(txtChildCostRate.Text) = 0 Then
                    Return False
                End If
                If ddlSupplier.SelectedValue <> "[Select]" And Val(txtnoofunits.Text) > 0 And Val(txtAdultCostRate.Text) = 0 Then
                    Return False
                End If
                If ddlSupplier.SelectedValue = "[Select]" And Val(txtCostValue.Text) > 0 Then
                    Return False
                End If
            Next


            If mCount = 0 Then
                If Val(txtTotal.Value) = 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                If Val(txtTotal.Value) = 0 Then
                    Return False
                Else
                    Return True
                End If
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function


    '<WebMethod()> _
    'Public Shared Function GetExcursionType() As DataSet
    '    ' Dim query As String = "SELECT sptypecode FROM partymast Where partycode ='" & supplier & "'"
    '    Dim ds As New DataSet
    '    Dim mObjUtilities As New clsUtils
    '    'ds = mObjUtilities.ExecuteQuerySqlnew(constr, query)
    '    Return ds
    'End Function
End Class




