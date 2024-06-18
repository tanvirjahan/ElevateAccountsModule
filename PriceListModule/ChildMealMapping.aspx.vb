#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
'#Region "Enum GridCol"
'Enum GridCol
'    FromCurrency = 1
'    ToCurrency = 2
'    Conversion = 3
'    DateCreated = 4
'    UserCreated = 5
'    DateModified = 6
'    UserModified = 7
'End Enum
'#End Region
Partial Class PriceListModule_ChildMealMapping
    Inherits System.Web.UI.Page
#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim sqlTrans As SqlTransaction
    Dim SqlConn As New SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim GvRow As GridViewRow
    Dim txtconvert As HtmlInputText
    Dim ddlPlistCode As HtmlSelect
    Dim ddlPlistName As HtmlSelect
    Dim hdnPlistcode As HiddenField
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
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
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                End If

                Session.Add("strsortExpression", "childmealcode")
                Session.Add("strsortdirection", SortDirection.Ascending)

                FillGrid()

                'fillgrd(gv_SearchResult, True)

               
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CurrencyConversionRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
           
        End If
    End Sub



    Private Sub FillGrid() ' promotions filter grids
        Try

            Dim myDS As New DataSet
            Dim strsql As String = ""
            'fill roomtypes grid
            grdchildmapping.Columns(4).Visible = True
            grdchildmapping.Columns(5).Visible = True

            strsql = "select rc.rmcatcode childmealcode,rc.rmcatname childmealname,ISNULL(cm.mealcode,'') mealcode ," _
             & " ISNULL(m.mealname,'') mealname " _
            & " from rmcatmast rc(nolock) left outer join child_meal_mapping cm(nolock) on rc.rmcatcode=cm.childmealcode left outer join mealmast m on cm.mealcode=m.mealcode " _
            & " where rc.accom_extra='C' and rc.rmcatcode not in (select childebcode from child_extrabed_mapping) order by rc.rmcatname"




            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                grdchildmapping.DataSource = myDS
                grdchildmapping.DataBind()

            Else

            End If


            Dim count As Long
            Dim GVRow As GridViewRow
            

            count = grdchildmapping.Rows.Count

            If count > 0 Then
                'fillgrd(gv_SearchResult, False, count)
                'gv_SearchResult.Columns(2).Visible = True

                For Each GVRow In grdchildmapping.Rows
                   
                    ddlPlistCode = GVRow.FindControl("ddlPlistmealCode")
                    ddlPlistName = GVRow.FindControl("ddlPlistmealName")
                    ddlPlistCode.Value = GVRow.Cells(5).Text
                    ddlPlistName.Value = GVRow.Cells(4).Text
                Next

                grdchildmapping.Columns(4).Visible = False
                grdchildmapping.Columns(5).Visible = False
            End If















        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                SqlConn.Close()
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ChildMealMapping.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub grdchildmapping_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdchildmapping.RowDataBound
       

        Try
            'For Each GvRow As GridViewRow In grdchildmapping.Rows
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                ddlPlistCode = e.Row.FindControl("ddlPlistmealCode")
                ddlPlistName = e.Row.FindControl("ddlPlistmealName")
                

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPlistCode, "mealcode", "mealname", "select mealcode,mealname from mealmast where active=1 order by mealcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPlistName, "mealname", "mealcode", "select mealcode,mealname from mealmast where active=1 order by mealname", True)
              

                ddlPlistCode.Attributes.Add("onchange", "javascript:FillPlistCode('" & ddlPlistCode.ClientID & "','" & ddlPlistName.ClientID & "')")
                ddlPlistName.Attributes.Add("onchange", "javascript:FillPlistName('" & ddlPlistName.ClientID & "','" & ddlPlistCode.ClientID & "')")



                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


                    ddlPlistCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPlistName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
            End If
            'Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ChildMealMapping.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub



    Private Sub SaveRecord()

        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            myCommand = New SqlCommand("sp_add_child_meal_mapping_log", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.ExecuteNonQuery() '-----delete data from child_meal_mapping
            


            '----------------------------------- Inserting Data To child_meal_mapping Table
            For Each GvRow As GridViewRow In grdchildmapping.Rows

                ddlPlistCode = GvRow.FindControl("ddlPlistmealCode")
                ddlPlistName = GvRow.FindControl("ddlPlistmealName")
                If ddlPlistName.Value <> "[Select]" Then
                    myCommand = New SqlCommand("sp_add_child_meal_mapping", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@childmealcode", SqlDbType.VarChar, 20)).Value = GvRow.Cells(0).Text
                    myCommand.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = ddlPlistName.Value
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                End If

                'End If
            Next
            sqlTrans.Commit()                'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("ChildMealMapping.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Handles btnSave.Click
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            myCommand = New SqlCommand("sp_add_child_meal_mapping_log", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.ExecuteNonQuery() '-----delete data from child_meal_mapping



            '----------------------------------- Inserting Data To child_meal_mapping Table
            For Each GvRow As GridViewRow In grdchildmapping.Rows

                ddlPlistCode = GvRow.FindControl("ddlPlistmealCode")
                ddlPlistName = GvRow.FindControl("ddlPlistmealName")
                If ddlPlistName.Value <> "[Select]" Then
                    myCommand = New SqlCommand("sp_add_child_meal_mapping", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@childmealcode", SqlDbType.VarChar, 20)).Value = GvRow.Cells(0).Text
                    myCommand.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = ddlPlistName.Value
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                End If

                'End If
            Next
            sqlTrans.Commit()                'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If


            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ChildMealMapping.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=ChildMealMapping&BackPageName=ChildMealMapping.aspx','RepChildMealMapping','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            objUtils.WritErrorLog("ChildMealMapping.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
End Class
