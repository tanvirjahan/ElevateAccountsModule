Imports System.Data
Imports system.data.SqlClient
Partial Class UpdateContactUs
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim mySqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim strappid As String = ""
            Dim strappname As String = ""
            If AppId Is Nothing = False Then
                strappid = AppId.Value
            End If
            If AppName Is Nothing = False Then
                strappname = AppName.Value
            End If



            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            Try
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("select * from webdetails where webdetailid=4", mySqlConn, sqlTrans)
                mySqlReader = mySqlCmd.ExecuteReader()
                If mySqlReader.Read Then
                    Session.Add("State", "Edit")
                    If IsDBNull(mySqlReader("webdetailid")) = False Then
                        txtWebDetailId.Text = mySqlReader("webdetailid")
                    End If
                    If IsDBNull(mySqlReader("detail_text")) = False Then
                        FCKeditor1.Content = mySqlReader("detail_text")
                    End If
                    btnSave.Text = "Update"
                Else
                    Session.Add("State", "Add")
                    txtWebDetailId.Text = "4"
                    btnSave.Text = "Save"
                End If
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("UpdateContactUs.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
            If CType(Session("State"), String) = "Add" Then
                mySqlCmd = New SqlCommand("sp_add_webdetails", mySqlConn, sqlTrans)
            ElseIf CType(Session("State"), String) = "Edit" Then
                mySqlCmd = New SqlCommand("sp_mod_webdetails", mySqlConn, sqlTrans)
            End If
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@webdetailid", SqlDbType.Int, 9)).Value = txtWebDetailId.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@detail_text", SqlDbType.Text)).Value = FCKeditor1.Content
            mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Save Sucessfully.');", True)
            Session.Add("State", "Edit")
            btnSave.Text = "Update"
        Catch ex As Exception
            objUtils.WritErrorLog("UpdateContactUs.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/MainPage.aspx", False)
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UpdateContactUs','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
