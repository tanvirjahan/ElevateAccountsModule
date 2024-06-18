
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class InterDeptTransferPosting
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils    

    Dim chkItem As HtmlInputCheckBox
    Dim lblTransactionID As Label
    Dim lblExcursionID As Label
    Dim lblTransType As Label
    Dim lblTransferDate As Label

    Dim lblPickupDate As Label
    Dim lblDepdate As Label
    Dim lblCarType As Label
    Dim lblSupplierCode As Label
    Dim lblSupplierName As Label
    Dim lblInhousesuppler As Label
    Dim txtCostRate As TextBox

    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim strTranType As String
    Private TransPostingID As String = String.Empty
    Private IsAddMode As Boolean = False
    Private mTotalCostVal As Decimal = CDec(0.0)

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ViewState("TransPostType") = Request.QueryString("TransPostType")
            ViewState("PostingPageState") = Request.QueryString("PostingPageState")
            If ViewState("PostingPageState") <> "New" Then
                If ViewState("PostingPageState") = "View" Then
                    dpFromDate.Enabled = False
                    dpToDate.Enabled = False
                    btnSave.Visible = False
                    btnDisplay.Visible = False
                    txtfromDate.Enabled = False
                    txtToDate.Enabled = False
                    grdTransferPost.Enabled = False
                ElseIf ViewState("PostingPageState") = "Edit" Then
                    dpFromDate.Enabled = True
                    dpToDate.Enabled = True
                    btnSave.Visible = True
                    btnDisplay.Visible = True
                    txtfromDate.Enabled = True
                    txtToDate.Enabled = True
                    grdTransferPost.Enabled = True
                End If
                ViewState("TransPostId") = Request.QueryString("TransPostId")
                TransPostingID = CType(ViewState("TransPostId"), String)
            Else
                dpFromDate.Enabled = True
                dpToDate.Enabled = True
                btnSave.Enabled = True
                btnDisplay.Enabled = True
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                txtfromDate.Text = Date.Today
                txtToDate.Text = Date.Today
                LoadInterDeptTarnsferPost()
                BindGrid()
            End If
            UpdatePanel1.Update()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadInterDeptTarnsferPost()
        Dim mStr As String = String.Empty
        Try
            If ViewState("PostingPageState") <> "New" Then
                mStr = "SELECT a.*,b.FromDate,b.Todate FROM transfer_posting_detail a INNER JOIN transfer_posting_header b On a.TranPostingId = b.TransPostingId Where a.TranPostingId='" + TransPostingID + "'"
            Else
                mStr = "EXEC sp_loadinterdepttransferpost '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "','" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "'"
            End If

            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mStr)
            If ViewState("PostingPageState") <> "New" Then
                If ds.Tables(0).Rows(0).Item("FromDate").ToString <> "" Then txtfromDate.Text = Format(CType(ds.Tables(0).Rows(0).Item("FromDate").ToString, Date), "dd/MM/yyyy")
                If ds.Tables(0).Rows(0).Item("ToDate").ToString <> "" Then txtToDate.Text = Format(CType(ds.Tables(0).Rows(0).Item("ToDate").ToString, Date), "dd/MM/yyyy")
            End If
            ViewState("InterDeptTransferPost") = ds
        Catch ex As Exception
        End Try
    End Sub

    Private Sub BindGrid()
        Try
            If Not ViewState("InterDeptTransferPost") Is Nothing Then
                Dim ds As DataSet = CType(ViewState("InterDeptTransferPost"), DataSet)
                If ds.Tables(0).Rows.Count > 0 Then
                    With grdTransferPost
                        .DataSource = ViewState("InterDeptTransferPost")
                        .DataBind()
                    End With
                End If                
            Else
                LoadInterDeptTarnsferPost()
                Call BindGrid()
            End If
            UpdatePanel1.Update()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub TemplateFieldBind(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Select Case sender.ID
                Case "lblTransType"
                    If Eval("TranType") = 2 Then
                        sender.text = "Shifting"
                    ElseIf Eval("TranType") = 1 Then
                        sender.text = "Departure"
                    ElseIf Eval("TranType") = 0 Then
                        sender.text = "Arrival"
                    End If

                Case "txtCostValue"
                    Dim mtext As TextBox = CType(sender, TextBox)
                    mtext.Text = Eval("costvalue")

                Case "lblPickupDate"
                    sender.text = Eval("pickup")

                Case "lblTransactionID"
                    sender.text = Eval("TranId")

                Case "lblExcursionID"
                    sender.text = Eval("ExcursionId")

                Case "lblTransferDate"
                    sender.text = Eval("transferdate")

                Case "lblDepdate"
                    sender.text = Eval("DropOff")

                Case "lblCarType"
                    sender.text = Eval("cartype")

                Case "lblSupplierCode"
                    sender.text = Eval("suppliercode")

                Case "lblSupplierName"
                    sender.text = Eval("SuplierName")

                Case "lblInhousesuppler"
                    sender.text = Eval("inhousesuppler")

            End Select
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
            'LoadInterDeptTarnsferPost()
            Dim mStr As String
            mStr = "EXEC sp_loadinterdepttransferpost '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "','" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "'"
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mStr)
            ViewState("InterDeptTransferPost") = ds
            IsAddMode = True
            BindGrid()
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub grdTransferPost_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTransferPost.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                chkItem = CType(e.Row.FindControl("chkItem"), HtmlInputCheckBox)
                lblTransactionID = CType(e.Row.FindControl("lblTransactionID"), Label)
                lblExcursionID = CType(e.Row.FindControl("lblExcursionID"), Label)
                lblTransType = CType(e.Row.FindControl("lblTransType"), Label)
                lblTransferDate = CType(e.Row.FindControl("lblTransferDate"), Label)
                lblPickupDate = CType(e.Row.FindControl("lblPickupDate"), Label)
                lblDepdate = CType(e.Row.FindControl("lblDepdate"), Label)
                lblCarType = CType(e.Row.FindControl("lblCarType"), Label)
                lblSupplierCode = CType(e.Row.FindControl("lblSupplierCode"), Label)
                lblSupplierName = CType(e.Row.FindControl("lblSupplierName"), Label)
                lblInhousesuppler = CType(e.Row.FindControl("lblInhousesuppler"), Label)
                txtCostRate = CType(e.Row.FindControl("txtCostValue"), TextBox)
                txtCostRate.Attributes.Add("onchange", "GetSelectedValue()")
                txtCostRate.Attributes.Add("onkeypress", "return checkNumber(event,this)")

                If ViewState("PostingPageState") <> "New" Then
                    If IsAddMode = False Then
                        chkItem.Checked = True
                        If ViewState("PostingPageState") = "View" Then                            
                            grdTransferPost.Columns(0).Visible = False
                        Else
                            grdTransferPost.Columns(0).Visible = True
                        End If
                    End If
                End If
                If chkItem.Checked Then
                    mTotalCostVal += CDec(txtCostRate.Text)
                End If
                txtTotal.Value = mTotalCostVal
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim txtDocNo As String = String.Empty
        'btnSave.Enabled = False
        Try
            If Page.IsValid = True Then

                If Val(txtTotal.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total value should not be zero');", True)
                    Exit Sub
                End If

                'If ValidateGrid = True Then
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = SqlConn.BeginTransaction

                If ViewState("PostingPageState") = "New" Then
                    Dim optionval As String
                    optionval = objUtils.GetAutoDocNo(CType(ViewState("TransPostType"), String), SqlConn, sqlTrans)
                    txtDocNo = optionval.Trim
                    myCommand = New SqlCommand("[dbo].[sp_InsertTransPostingHeader]", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@TransPostingId", SqlDbType.VarChar, 50)).Value = txtDocNo
                    myCommand.Parameters.Add(New SqlParameter("@jvno", SqlDbType.VarChar, 50)).Value = ""
                    If txtfromDate.Text <> "" Then myCommand.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.DateTime)).Value = Format(CType(txtfromDate.Text, Date), "dd-MMM-yyyy")
                    If txtToDate.Text <> "" Then myCommand.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.DateTime)).Value = Format(CType(txtToDate.Text, Date), "dd-MMM-yyyy")
                    myCommand.Parameters.Add(New SqlParameter("@Total", SqlDbType.Decimal)).Value = CDec(Val(txtTotal.Value))
                    myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = CType(Format(Date.Now, "dd-MMM-yyyy"), Date)
                ElseIf ViewState("PostingPageState") = "Edit" Then
                    txtDocNo = TransPostingID
                    myCommand = New SqlCommand("[dbo].[sp_UpdateTransPostingHeader]", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@TransPostingId", SqlDbType.VarChar, 50)).Value = TransPostingID
                    myCommand.Parameters.Add(New SqlParameter("@jvno", SqlDbType.VarChar, 50)).Value = ""
                    If txtfromDate.Text <> "" Then myCommand.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.DateTime)).Value = Format(CType(txtfromDate.Text, Date), "dd-MMM-yyyy")
                    If txtToDate.Text <> "" Then myCommand.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.DateTime)).Value = Format(CType(txtToDate.Text, Date), "dd-MMM-yyyy")
                    myCommand.Parameters.Add(New SqlParameter("@Total", SqlDbType.VarChar, 50)).Value = CDec(Val(txtTotal.Value))
                    myCommand.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.Parameters.Add(New SqlParameter("@modedate", SqlDbType.DateTime)).Value = CType(Format(Date.Now, "dd-MMM-yyyy"), Date)
                ElseIf ViewState("PostingPageState") = "Delete" Then
                    'txtDocNo = TransPostingID
                    'myCommand = New SqlCommand("[dbo].[sp_DeleteTransPostingHeaderAndDetail]", SqlConn, sqlTrans)
                    'myCommand.CommandType = CommandType.StoredProcedure
                    'myCommand.Parameters.Add(New SqlParameter("@TransPostingId", SqlDbType.VarChar, 50)).Value = TransPostingID
                End If
                myCommand.ExecuteNonQuery()

                If ViewState("PostingPageState") <> "Delete" Then
                    For Each row As GridViewRow In grdTransferPost.Rows
                        chkItem = CType(row.FindControl("chkItem"), HtmlInputCheckBox)
                        lblTransactionID = CType(row.FindControl("lblTransactionID"), Label)
                        lblExcursionID = CType(row.FindControl("lblExcursionID"), Label)
                        lblTransType = CType(row.FindControl("lblTransType"), Label)
                        lblTransferDate = CType(row.FindControl("lblTransferDate"), Label)
                        lblPickupDate = CType(row.FindControl("lblPickupDate"), Label)
                        lblDepdate = CType(row.FindControl("lblDepdate"), Label)
                        lblCarType = CType(row.FindControl("lblCarType"), Label)
                        lblSupplierCode = CType(row.FindControl("lblSupplierCode"), Label)
                        lblSupplierName = CType(row.FindControl("lblSupplierName"), Label)
                        lblInhousesuppler = CType(row.FindControl("lblInhousesuppler"), Label)
                        txtCostRate = CType(row.FindControl("txtCostValue"), TextBox)

                        If chkItem.Checked Then
                            If ViewState("PostingPageState") = "New" Or ViewState("PostingPageState") = "Edit" Then
                                myCommand = New SqlCommand("[dbo].[sp_InsertTransPostingDetail]", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                '@TransPostingId
                                myCommand.Parameters.Add(New SqlParameter("@TransPostingId", SqlDbType.VarChar, 50)).Value = txtDocNo
                                myCommand.Parameters.Add(New SqlParameter("@TranId", SqlDbType.VarChar, 50)).Value = lblTransactionID.Text
                                myCommand.Parameters.Add(New SqlParameter("@ExcursionId", SqlDbType.VarChar, 50)).Value = lblExcursionID.Text
                                If lblTransferDate.Text <> "" Then myCommand.Parameters.Add(New SqlParameter("@TransferDate", SqlDbType.DateTime)).Value = Format(CType(lblTransferDate.Text, Date), "dd-MMM-yyyy")
                                'If lblPickupDate.Text <> "" Then myCommand.Parameters.Add(New SqlParameter("@PickUpdate", SqlDbType.DateTime)).Value = Format(CType(lblPickupDate.Text, Date), "dd-MMM-yyyy")
                                myCommand.Parameters.Add(New SqlParameter("@PickUp", SqlDbType.VarChar, 50)).Value = lblPickupDate.Text

                                myCommand.Parameters.Add(New SqlParameter("@TranType", SqlDbType.Int)).Value = CType(Val(lblTransType.Text), Integer)
                                myCommand.Parameters.Add(New SqlParameter("@DropOff", SqlDbType.VarChar, 50)).Value = lblDepdate.Text
                                myCommand.Parameters.Add(New SqlParameter("@CarType", SqlDbType.VarChar, 50)).Value = lblCarType.Text
                                myCommand.Parameters.Add(New SqlParameter("@SupplierCode", SqlDbType.VarChar, 50)).Value = lblSupplierCode.Text
                                myCommand.Parameters.Add(New SqlParameter("@SuplierName", SqlDbType.VarChar, 50)).Value = lblSupplierName.Text
                                myCommand.Parameters.Add(New SqlParameter("@InhouseSupplier", SqlDbType.VarChar, 50)).Value = lblInhousesuppler.Text
                                myCommand.Parameters.Add(New SqlParameter("@costvalue", SqlDbType.Decimal)).Value = txtCostRate.Text
                                myCommand.ExecuteNonQuery()
                            End If
                        End If
                    Next
                End If

                'Update JV No to the header table
                If ViewState("PostingPageState") = "New" Or ViewState("PostingPageState") = "Edit" Then

                    Dim narration As String = "Transfer Cost Related Excursion from the date " + Format(CType(txtfromDate.Text, Date), "dd-MMM-yyyy") + " to " + Format(CType(txtToDate.Text, Date), "dd-MMM-yyyy") + " for posting id" + txtDocNo
                    myCommand = New SqlCommand("[dbo].[sp_Interdept_autojv]", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@transpostingid", SqlDbType.VarChar, 50)).Value = txtDocNo
                    myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 200)).Value = narration
                    myCommand.Parameters.Add(New SqlParameter("@totalamount", SqlDbType.VarChar, 50)).Value = CDec(Val(txtTotal.Value))
                    myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.Parameters.Add(New SqlParameter("@jvno", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.ExecuteNonQuery()
                End If

                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('TransferPostWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                'End If


            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)                       'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)
            End If

        End Try
    End Sub

    'Private Function ValidateGrid() As Boolean
    '    Dim mRetVal As Boolean = False
    '    For Each row As GridViewRow In grdTransferPost.Rows
    '        chkItem = CType(row.FindControl("chkItem"), HtmlInputCheckBox)
    '        txtCostRate = CType(row.FindControl("txtCostValue"), TextBox)
    '        If txtCostRate.Text = "" Or Val(txtCostRate.Text) = 0 Then
    '            mRetVal = False
    '            Return mRetVal
    '        Else
    '            mRetVal = True                
    '        End If
    '    Next
    '    Return mRetVal
    'End Function

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Try
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('TransferPostWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Catch ex As Exception

        End Try
    End Sub
End Class

 