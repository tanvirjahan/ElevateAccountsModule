

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class InterDeptTransferPostingSearch
    Inherits System.Web.UI.Page


    Dim objUtils As New clsUtils
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

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('InterDeptTransferPosting.aspx?PostingPageState=New&TransPostType=TRANPOST','InterDeptTransferPosting','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = True Then
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "TransferPostWindowPostBack") Then
                'btnSearch_Click(sender, e)
                LoadInterDeptTarnsferPost(False)
                BindGrid()
            End If
        End If

        If Not Page.IsPostBack Then
            LoadInterDeptTarnsferPost(False)
            BindGrid()
        End If
        upnlInterDeptPost.Update()
    End Sub

    Private Sub LoadInterDeptTarnsferPost(ByVal IsSearch As Boolean)
        Try
            Dim mStr As String = "SELECT *,adddate as convadddate,moddate as convmoddate FROM transfer_posting_header "
            If IsSearch Then
                If txtTransPostID.Text = "" And txtJvNo.Text = "" And txtReqId.Text = "" And txtExcId.Text = "" Then
                Else
                    mStr &= " Where "
                    mStr = BindSearchText(mStr)                    
                End If
            End If

            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mStr)
            ViewState("InterDeptTransferPostSearch") = ds
        Catch ex As Exception
        End Try
    End Sub

    Private Function BindSearchText(ByVal mStr As String) As String
        Try
            Dim mArray As New ArrayList
            mArray.Add(txtTransPostID)
            mArray.Add(txtReqId)
            mArray.Add(txtJvNo)
            mArray.Add(txtExcId)
            Dim mCount As Integer = 0
            For i As Integer = 0 To mArray.Count - 1
                Dim mSrhtext As TextBox = DirectCast(mArray(i), TextBox)
                If mSrhtext.Text <> "" Then
                    If mCount = 0 Then
                        If i = 0 Then mStr += " TransPostingID Like '%" + mSrhtext.Text + "%'"
                        If i = 1 Then mStr += " TranId Like '%" + mSrhtext.Text + "%'"
                        If i = 2 Then mStr += " JvNo Like '%" + mSrhtext.Text + "%'"
                        If i = 3 Then mStr += " ExcursionId Like '%" + mSrhtext.Text + "%'"
                    Else
                        If i = 0 Then mStr += " And TransPostingID Like '%" + mSrhtext.Text + "%'"
                        If i = 1 Then mStr += " And TranId Like '%" + mSrhtext.Text + "%'"
                        If i = 2 Then mStr += " And JvNo Like '%" + mSrhtext.Text + "%'"
                        If i = 3 Then mStr += " And ExcursionId Like '%" + mSrhtext.Text + "%'"
                    End If
                    mCount += 1
                End If
            Next
        Catch ex As Exception

        End Try
        Return mStr
    End Function

    Private Sub BindGrid()
        Try
            If Not ViewState("InterDeptTransferPostSearch") Is Nothing Then
                With grdTransferPost
                    .DataSource = ViewState("InterDeptTransferPostSearch")
                    .DataBind()
                End With
            Else
                LoadInterDeptTarnsferPost(False)
                Call BindGrid()
            End If
            upnlInterDeptPost.Update()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            LoadInterDeptTarnsferPost(True)
            BindGrid()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub grdTransferPost_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTransferPost.RowCommand
        Try
            Dim strpop As String = ""
            Dim actionstr As String
            actionstr = ""


            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As String
            'Dim lbltrantype As String

            lblId = e.CommandArgument.ToString

            If e.CommandName = "EditRow" Then
                strpop = "window.open('InterDeptTransferPosting.aspx?PostingPageState=Edit&TransPostId=" + lblId + "&TransPostType=TRANPOST','InterDeptTransferPosting','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                Dim mDsColName As New DataSet
                Dim mGetColumnStr As String = "SELECT a.*,b.* FROM transfer_posting_detail a INNER JOIN transfer_posting_header b On a.TranPostingId = b.TransPostingId Where a.TranPostingId='" + lblId + "'"
                mDsColName = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mGetColumnStr)

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = SqlConn.BeginTransaction
                For Each item As DataRow In mDsColName.Tables(0).Rows
                    myCommand = New SqlCommand("[dbo].[sp_InsertDelTransPostingLog]", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    Dim mTotal As Decimal = 0.0
                    If Not IsDBNull(item("Total")) Then
                        mTotal = CDec(Val(item("Total")))
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@TransPostingId", SqlDbType.VarChar, 50)).Value = lblId
                    myCommand.Parameters.Add(New SqlParameter("@jvno", SqlDbType.VarChar, 50)).Value = item("jvno")
                    If Not IsDBNull(item("FromDate")) Then
                        myCommand.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.DateTime)).Value = Format(CType(item("FromDate"), Date), "dd-MMM-yyyy")
                    End If

                    If Not IsDBNull(item("ToDate")) Then                        
                        myCommand.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.DateTime)).Value = Format(CType(item("ToDate"), Date), "dd-MMM-yyyy")
                    End If


                    myCommand.Parameters.Add(New SqlParameter("@Total", SqlDbType.Decimal, 50)).Value = mTotal
                    myCommand.Parameters.Add(New SqlParameter("@TranId", SqlDbType.VarChar, 50)).Value = item("TranId")
                    myCommand.Parameters.Add(New SqlParameter("@ExcursionId", SqlDbType.VarChar, 50)).Value = item("ExcursionId")

                    If Not IsDBNull(item("TransferDate")) Then                        
                        myCommand.Parameters.Add(New SqlParameter("@TransferDate", SqlDbType.DateTime)).Value = Format(CType(item("TransferDate"), Date), "dd-MMM-yyyy")
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@PickUp", SqlDbType.VarChar, 50)).Value = item("PickUp")

                    'If Not IsDBNull(item("PickUpdate")) Then                        
                    '    myCommand.Parameters.Add(New SqlParameter("@PickUpdate", SqlDbType.DateTime)).Value = Format(CType(item("PickUpdate"), Date), "dd-MMM-yyyy")
                    'End If

                    If Not IsDBNull(item("TranType")) Then
                        myCommand.Parameters.Add(New SqlParameter("@TranType", SqlDbType.Int)).Value = CType(Val(item("TranType")), Integer)
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@DropOff", SqlDbType.VarChar, 50)).Value = item("DropOff")
                    'If Not IsDBNull(item("DropOff")) Then                        
                    '    myCommand.Parameters.Add(New SqlParameter("@DropOff", SqlDbType.DateTime)).Value = Format(CType(item("DropOff"), Date), "dd-MMM-yyyy")
                    'End If

                    myCommand.Parameters.Add(New SqlParameter("@CarType", SqlDbType.VarChar, 50)).Value = item("CarType")
                    myCommand.Parameters.Add(New SqlParameter("@SupplierCode", SqlDbType.VarChar, 50)).Value = item("SupplierCode")
                    myCommand.Parameters.Add(New SqlParameter("@SuplierName", SqlDbType.VarChar, 50)).Value = item("SuplierName")
                    myCommand.Parameters.Add(New SqlParameter("@InhouseSupplier", SqlDbType.VarChar, 50)).Value = item("InhouseSupplier")
                    myCommand.Parameters.Add(New SqlParameter("@costvalue", SqlDbType.Decimal)).Value = item("costvalue")

                    If Not IsDBNull(item("adddate")) Then
                        myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = Format(CType(item("adddate"), Date), "dd-MMM-yyyy")
                    End If

                    If Not IsDBNull(item("moddate")) Then
                        myCommand.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = Format(CType(item("moddate"), Date), "dd-MMM-yyyy")
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = item("adduser")
                    myCommand.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 50)).Value = item("moduser")
                    myCommand.ExecuteNonQuery()
                Next

                myCommand = New SqlCommand("[dbo].[sp_DeleteTransPostingHeaderAndDetail]", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@TransPostingId", SqlDbType.VarChar, 50)).Value = lblId
                myCommand.ExecuteNonQuery()

                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                'strpop = "window.open('InterDeptTransferPosting.aspx?PostingPageState=Delete&TransPostId=" + lblId + "&TransPostType=TRANPOST','InterDeptTransferPosting','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

                LoadInterDeptTarnsferPost(False)
                BindGrid()

            ElseIf e.CommandName = "View" Then
                strpop = "window.open('InterDeptTransferPosting.aspx?PostingPageState=View&TransPostId=" + lblId + "&TransPostType=TRANPOST','InterDeptTransferPosting','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
            'lbltrantype = e.CommandArgument.ToString.Split("|")(1).ToString

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtExcId.Text = ""
        txtJvNo.Text = ""
        txtReqId.Text = ""
        txtTransPostID.Text = ""
        LoadInterDeptTarnsferPost(False)
        BindGrid()    
    End Sub
End Class

