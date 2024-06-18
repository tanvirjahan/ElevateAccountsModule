
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class AccountsModule_Accnt_trn_amendlog
    Inherits System.Web.UI.Page


    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objdate As New clsDateTime
    Dim strQry As String

    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If


            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub

            End If
            ViewState.Add("Docno", Request.QueryString("tid"))
            ViewState.Add("Doctype", Request.QueryString("ttype"))
            ViewState.Add("Transdate", Request.QueryString("tdate"))
            lblDocNo.Text = ViewState("Docno")
            lblDocType.Text = ViewState("Doctype")
            lblTransdate.Text = ViewState("Transdate")


        Catch
        End Try


        

    End Sub
#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If validatePage() = False Then
            Exit Sub

        End If

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        sqlTrans = mySqlConn.BeginTransaction
       

        mySqlCmd = New SqlCommand("sp_add_accounttransaction", mySqlConn, sqlTrans)

        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(lblDocNo.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 20)).Value = CType(lblDocType.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = CType(lblTransdate.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)


        mySqlCmd.Parameters.Add(New SqlParameter("@description", SqlDbType.VarChar, 200)).Value = CType(txtreason.Text.Trim, String)



        mySqlCmd.ExecuteNonQuery()

        sqlTrans.Commit()
        clsDBConnect.dbSqlTransation(sqlTrans)
        clsDBConnect.dbCommandClose(mySqlCmd)
        clsDBConnect.dbConnectionClose(mySqlConn)
      


        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

    End Sub

#End Region

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Public Function validatePage() As Boolean
        validatePage = True
        Try
            If txtreason.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid reasons');", True)
                SetFocus(txtreason)
                validatePage = False
                Exit Function

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
End Class
