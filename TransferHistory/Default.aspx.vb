Imports System.Data.SqlClient
Imports System.Data


Partial Class TransferHistory_Default
    Inherits System.Web.UI.Page


    Dim myCommand As SqlCommand
    Dim SqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim objUtils As New clsUtils
    Dim IsDeletedAdult = False
    Dim strTransferType = String.Empty
    Dim issuccess As Boolean = False

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        imgicon.Style("visibility") = "hidden"
        btnSearch.Attributes.Add("onclick", "return validatePage()")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            issuccess = False
            dpFromDate.txtDate.Text = Format(Date.Now, "dd/MM/yyyy")
            issuccess = False


        End If
    End Sub
  
    Public Sub CancellationPolicyTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_cancelhistory", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd") 'Format(dpFromDate.txtDate.Text, "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub PriceListTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_plisttohistory", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd") 'Format(dpFromDate.txtDate.Text, "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub ChildPolicyTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_childpolicynew_history", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
    Public Sub BlockFullSalesTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_blockfullsale_history", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
    Public Sub CompulsoryRemarksTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_compulsoryremarks_history", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub MinimumNightsTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_minnightdetails_history", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub PromotionTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_promotion_history", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub SpecialEventsTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_speicalevents_history", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else

                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub AllotmentsTransfer()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_transfer_allotmenttohistory", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = Format(CDate(dpFromDate.txtDate.Text), "yyyy/MM/dd")
            IsDeletedAdult = myCommand.ExecuteNonQuery()
            If IsDeletedAdult Then
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered Successfully');", True)
                lblMessage.Text = strTransferType & " Transfer Successfully upto " & dpFromDate.txtDate.Text
                issuccess = True
            Else
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Occurred Or Data Not Found' );", True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub AddHistoryHeader()
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_add_transfer_plist_tohistory", SqlConn, sqlTrans)
            myCommand.Parameters.Add(New SqlParameter("@optionselected", SqlDbType.VarChar)).Value = strTransferType
            myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar)).Value = dpFromDate.txtDate.Text
            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar)).Value = Session("GlobalUserName")
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.ExecuteNonQuery()
            sqlTrans.Commit()
            clsDBConnect.dbSqlTransation(sqlTrans)
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Data Transfered successfully But Entry for user not logged.');", True)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        issuccess = False
        If ddlTransferOption.SelectedValue.Equals("1") Then
            strTransferType = "Price List"
            PriceListTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("2") Then
            strTransferType = "Child Policy"
            ChildPolicyTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("3") Then
            strTransferType = "Cancellation Policy"
            CancellationPolicyTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("4") Then
            strTransferType = "Block Full Sales"
            BlockFullSalesTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("5") Then
            strTransferType = "Compulsory Remarks"
            CompulsoryRemarksTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("6") Then
            strTransferType = "Minimum Night"
            MinimumNightsTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("7") Then
            strTransferType = "Promotion"
            PromotionTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("8") Then
            strTransferType = "Special Events Price List"
            SpecialEventsTransfer()
        ElseIf ddlTransferOption.SelectedValue.Equals("9") Then
            strTransferType = "Allotments"
            AllotmentsTransfer()
        Else
            ' Not Implemented.
        End If
        If issuccess = True Then
            AddHistoryHeader()
        End If
    End Sub
End Class
