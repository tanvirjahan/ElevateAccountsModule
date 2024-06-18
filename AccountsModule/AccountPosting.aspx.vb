Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq

Partial Class AccountPosting
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlConn As SqlConnection
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim divid As String = CType(Request.QueryString("divid"), String)
                txtTransId.Text = CType(Request.QueryString("ID"), String)

                If divid = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Division code can not be empty' );", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "AccountPosting", "window.close();", True)
                End If
                If txtTransId.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Transaction id can not be empty' );", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "AccountPosting", "window.close();", True)
                End If

                Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                gvCust.Columns(7).HeaderText = "Debit (" + baseCurrency + ")"
                gvCust.Columns(8).HeaderText = "Credit (" + baseCurrency + ")"
                gvProv.Columns(7).HeaderText = "Debit (" + baseCurrency + ")"
                gvProv.Columns(8).HeaderText = "Credit (" + baseCurrency + ")"

                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)
                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                mySqlCmd = New SqlCommand("sp_view_accountposting", sqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = divid
                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtTransId.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = "IN"
                Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                    Using customerDt As New DataTable
                        myDataAdapter.Fill(customerDt)
                        gvCust.DataSource = customerDt
                        gvCust.DataBind()
                        If customerDt.Rows.Count > 0 Then
                            Dim totDebt As Decimal = customerDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("acc_debit"))
                            Dim totCredit As Decimal = customerDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Acc_credit"))
                            Dim totBaseDebt As Decimal = customerDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("acc_base_debit"))
                            Dim totBaseCredit As Decimal = customerDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("acc_base_credit"))
                            gvCust.FooterRow.Cells(0).Text = "Total"
                            gvCust.FooterRow.Cells(0).ColumnSpan = 5
                            gvCust.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Right
                            For i = 1 To 4
                                gvCust.FooterRow.Cells.RemoveAt(i)
                            Next
                            gvCust.FooterRow.Cells(1).Text = Math.Round(totDebt, decimalPlaces)
                            gvCust.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
                            gvCust.FooterRow.Cells(2).Text = Math.Round(totCredit, decimalPlaces)
                            gvCust.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
                            gvCust.FooterRow.Cells(3).Text = Math.Round(totBaseDebt, decimalPlaces)
                            gvCust.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                            gvCust.FooterRow.Cells(4).Text = Math.Round(totBaseCredit, decimalPlaces)
                            gvCust.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                        End If
                    End Using
                End Using
                clsDBConnect.dbCommandClose(mySqlCmd)

                mySqlCmd = New SqlCommand("sp_view_accountposting", sqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = divid
                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtTransId.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = "PR"
                Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                    Using provDt As New DataTable
                        myDataAdapter.Fill(provDt)
                        gvProv.DataSource = provDt
                        gvProv.DataBind()
                        If provDt.Rows.Count > 0 Then
                            Dim totDebt As Decimal = provDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("acc_debit"))
                            Dim totCredit As Decimal = provDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Acc_credit"))
                            Dim totBaseDebt As Decimal = provDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("acc_base_debit"))
                            Dim totBaseCredit As Decimal = provDt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("acc_base_credit"))
                            gvProv.FooterRow.Cells(0).Text = "Total"
                            gvProv.FooterRow.Cells(0).ColumnSpan = 5
                            gvProv.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Right
                            For i = 1 To 4
                                gvProv.FooterRow.Cells.RemoveAt(i)
                            Next
                            gvProv.FooterRow.Cells(1).Text = Math.Round(totDebt, decimalPlaces)
                            gvProv.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
                            gvProv.FooterRow.Cells(2).Text = Math.Round(totCredit, decimalPlaces)
                            gvProv.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
                            gvProv.FooterRow.Cells(3).Text = Math.Round(totBaseDebt, decimalPlaces)
                            gvProv.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                            gvProv.FooterRow.Cells(4).Text = Math.Round(totBaseCredit, decimalPlaces)
                            gvProv.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                        End If
                    End Using
                End Using
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AccountPosting.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvCust_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCust.RowDataBound"
    Protected Sub gvCust_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCust.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim lblAcc_debit As Label = CType(e.Row.FindControl("lblAcc_debit"), Label)
            If IsNumeric(lblAcc_debit.Text) Then
                lblAcc_debit.Text = Math.Round(Convert.ToDecimal(lblAcc_debit.Text), decimalPlaces)
            End If
            Dim lblAccCredit As Label = CType(e.Row.FindControl("lblAcc_credit"), Label)
            If IsNumeric(lblAccCredit.Text) Then
                lblAccCredit.Text = Math.Round(Convert.ToDecimal(lblAccCredit.Text), decimalPlaces)
            End If
            Dim lblAcc_base_debit As Label = CType(e.Row.FindControl("lblacc_base_debit"), Label)
            If IsNumeric(lblAcc_base_debit.Text) Then
                lblAcc_base_debit.Text = Math.Round(Convert.ToDecimal(lblAcc_base_debit.Text), decimalPlaces)
            End If
            Dim lblAcc_base_credit As Label = CType(e.Row.FindControl("lblacc_base_credit"), Label)
            If IsNumeric(lblAcc_base_credit.Text) Then
                lblAcc_base_credit.Text = Math.Round(Convert.ToDecimal(lblAcc_base_credit.Text), decimalPlaces)
            End If
        End If
    End Sub
#End Region

#Region "Protected Sub gvProv_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProv.RowDataBound"
    Protected Sub gvProv_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim lblAcc_debit As Label = CType(e.Row.FindControl("lblAcc_debit"), Label)
            If IsNumeric(lblAcc_debit.Text) Then
                lblAcc_debit.Text = Math.Round(Convert.ToDecimal(lblAcc_debit.Text), decimalPlaces)
            End If
            Dim lblAccCredit As Label = CType(e.Row.FindControl("lblAcc_credit"), Label)
            If IsNumeric(lblAccCredit.Text) Then
                lblAccCredit.Text = Math.Round(Convert.ToDecimal(lblAccCredit.Text), decimalPlaces)
            End If
            Dim lblAcc_base_debit As Label = CType(e.Row.FindControl("lblacc_base_debit"), Label)
            If IsNumeric(lblAcc_base_debit.Text) Then
                lblAcc_base_debit.Text = Math.Round(Convert.ToDecimal(lblAcc_base_debit.Text), decimalPlaces)
            End If
            Dim lblAcc_base_credit As Label = CType(e.Row.FindControl("lblacc_base_credit"), Label)
            If IsNumeric(lblAcc_base_credit.Text) Then
                lblAcc_base_credit.Text = Math.Round(Convert.ToDecimal(lblAcc_base_credit.Text), decimalPlaces)
            End If
        End If
    End Sub
#End Region

End Class
