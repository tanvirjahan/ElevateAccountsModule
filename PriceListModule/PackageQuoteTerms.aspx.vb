


Imports System.Data
Imports System.Data.SqlClient
Partial Class PackageQuoteTerms
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
                mySqlCmd = New SqlCommand("select * from packageterms_header where pktype='" & ddlPackage.SelectedValue & "'", mySqlConn, sqlTrans)
                mySqlReader = mySqlCmd.ExecuteReader()
                If mySqlReader.Read Then
                    Session.Add("State", "Edit")
                    If IsDBNull(mySqlReader("pkid")) = False Then
                        txtPkTermsId.Text = mySqlReader("Pkid")
                    End If
                    If IsDBNull(mySqlReader("pkterms")) = False Then
                        Editor1.Content = mySqlReader("pkterms")
                    End If
                    btnSave.Text = "Update"
                Else
                    Session.Add("State", "Add")
                    txtPkTermsId.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select max(pkid)+1 from packageterms_header")
                    btnSave.Text = "Save"
                End If
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PackageQuoteTerms.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                mySqlCmd = New SqlCommand("sp_add_packageterms", mySqlConn, sqlTrans)
            ElseIf CType(Session("State"), String) = "Edit" Then
                mySqlCmd = New SqlCommand("sp_mod_packageterms", mySqlConn, sqlTrans)
            End If
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@pkid", SqlDbType.Int, 9)).Value = txtPkTermsId.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@pktype", SqlDbType.VarChar, 10)).Value = CType(ddlPackage.SelectedValue, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@pkterms", SqlDbType.Text)).Value = Editor1.Content
            If CType(Session("State"), String) = "Edit" Then
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            End If
            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Save Sucessfully.');", True)
            Session.Add("State", "Edit")
            btnSave.Text = "Update"
        Catch ex As Exception
            objUtils.WritErrorLog("UpdateTermsAndConditions.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/MainPage.aspx", False)
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UpdateTermsAndConditions','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlPackage_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPackage.SelectedIndexChanged
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("select * from packageterms_header where pktype='" & ddlPackage.SelectedValue & "'", mySqlConn, sqlTrans)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.Read Then
                Session.Add("State", "Edit")
                If IsDBNull(mySqlReader("pkid")) = False Then
                    txtPkTermsId.Text = mySqlReader("pkid")
                End If
                If IsDBNull(mySqlReader("pkterms")) = False Then
                    Editor1.Content = mySqlReader("pkterms")
                End If
                btnSave.Text = "Update"
            Else
                Session.Add("State", "Add")
                txtPkTermsId.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select max(pkid)+1 from packageterms_header")
                Editor1.Content = ""
                btnSave.Text = "Save"
            End If
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PackageQuoteTerms.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class

