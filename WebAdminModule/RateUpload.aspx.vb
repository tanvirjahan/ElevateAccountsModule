#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections.Generic

#End Region

Partial Class WebAdminModule_Rate_Upload
    Inherits System.Web.UI.Page
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objdate As New clsDateTime
    Dim strQry As String
    Dim sqlTrans As SqlTransaction
    Dim SqlConn As New SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim dtExpand As DataTable
    Dim tvrup1 As TreeView
    Shared selectednode As String
    Shared newrootnode As TreeNode
    Shared selecteddepth As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not Page.IsPostBack Then
                ' btnupload.Attributes.Add("onclick", "return filenamevalidate('" + FileUpload1.ClientID + "');")
                createtreeview()
            End If
           

        Catch ex As Exception

        End Try

    End Sub
    Public Sub refresh()
        tvrupl.Nodes.Clear()
        Dim rootDir As New DirectoryInfo(Server.MapPath("..\AgentsOnline\RateUpload\"))
        Dim RootNode As TreeNode = RecurseNodes(rootDir)
        tvrupl.Nodes.Add(RootNode)


    End Sub

    Public Sub createtreeview()
        Dim rootDir As New DirectoryInfo(Server.MapPath("..\AgentsOnline\RateUpload\"))
        ' Enter the RecurseNodes function to recursively walk the directory tree. 
        Dim RootNode As TreeNode = RecurseNodes(rootDir)
        'Dim gchild As TreeNode = RecurseNodes(rootDir)

        ' Add this Node hierarchy to the TreeNode control. 
        tvrupl.Nodes.Add(RootNode)
    End Sub
    Private Sub CheckTreeExpanded(ByVal treeNodecol As TreeNodeCollection)
        If dtExpand IsNot Nothing = False Then
            dtExpand = New DataTable
            dtExpand.Columns.Add("Id", GetType(String))
            dtExpand.Columns.Add("IsExpanded", GetType(Integer))
            Session("treeExpnd") = dtExpand
        Else
            dtExpand = Session("treeExpnd")

        End If

        For i = 0 To treeNodecol.Count - 1
            Dim tvNode As TreeNode
            tvNode = treeNodecol.Item(i)
            If tvNode.Expanded = True Then
                dtExpand.Rows.Add(tvNode.Value, 1)
            Else
                dtExpand.Rows.Add(tvNode.Value, 0)
            End If
            Session("treeExpnd") = dtExpand
            If tvNode.Expanded = True Then
                CheckTreeExpanded(tvNode.ChildNodes)
            End If

        Next

    End Sub

    Private Sub SetTreeExpanded(ByVal treeNodecol As TreeNodeCollection)
        If dtExpand IsNot Nothing = False Then
            dtExpand = Session("treeExpnd")
        End If

        For i = 0 To treeNodecol.Count - 1
            Dim tvNode As TreeNode
            Dim dtRow() As DataRow
            tvNode = treeNodecol.Item(i)
            dtRow = dtExpand.Select(String.Format("Id='{0}'", tvNode.Value))
            If dtRow.Length > 0 Then
                If dtRow(0).Item("IsExpanded") = 1 Then
                    tvNode.Expand()
                End If
            End If

            If tvNode.Expanded = True Then
                SetTreeExpanded(tvNode.ChildNodes)
            End If

        Next
    End Sub
    Private Function RecurseNodes(ByVal thisDir As DirectoryInfo) As TreeNode
        Dim thisDirNode As New TreeNode(thisDir.Name, Nothing)

        ' Get all the subdirectories in this Directory. 
        Dim subDirs As DirectoryInfo() = thisDir.GetDirectories()
        For Each subDir As DirectoryInfo In subDirs
            thisDirNode.ChildNodes.Add(RecurseNodes(subDir))
        Next

        ' Now get the files in this Directory. 
        Dim files As FileInfo() = thisDir.GetFiles()
        For Each file As FileInfo In files
            Dim thisFileNode As New TreeNode(file.Name, Nothing)
            thisDirNode.ChildNodes.Add(thisFileNode)
        Next

        Return thisDirNode
    End Function
    Protected Sub tvrupl_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvrupl.SelectedNodeChanged

        If (tvrupl.SelectedNode.Depth >= 2) Then
            lblmsg.Visible = False
            ModalPopupExtender1.Show()
            If (tvrupl.SelectedNode.Depth = 2 Or tvrupl.SelectedNode.Depth = 3) Then
                pnlfileupload.Visible = False
                pnlsubfolder.Visible = True
            Else
               

                If (tvrupl.SelectedNode.Depth > 5) Then
                    pnlfileupload.Visible = False
                    pnlsubfolder.Visible = False
                ElseIf (tvrupl.SelectedNode.Depth > 4) Then
                    'pnlfileupload.Visible = True 'This is commented by Elsitta on 23.04.15
                    'pnlsubfolder.Visible = False
                    pnlfileupload.Visible = False
                    pnlsubfolder.Visible = False

                Else
                    'pnlfileupload.Visible = False 'This is commented by Elsitta on 23.04.15
                    'pnlsubfolder.Visible = True

                    pnlfileupload.Visible = True ' This is added by Elsitta on 23.04.15 
                    pnlsubfolder.Visible = False
                End If
            End If
            End If

    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        ' Dim strnodename As String = tvrupl.SelectedNode.Text
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            myCommand = New SqlCommand("sp_treeviewlog", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@folder", SqlDbType.VarChar, 100)).Value = CType(txtfoldername.Text, String)
            myCommand.Parameters.Add(New SqlParameter("@pfolder", SqlDbType.VarChar, 100)).Value = CType(tvrupl.SelectedNode.Text, String)
            myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()
            sqlTrans.Commit()                'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)

            Dim strparentnode As String = tvrupl.SelectedNode.ValuePath.ToString()
            Dim rootDir As New DirectoryInfo(Server.MapPath("..\AgentsOnline\" + strparentnode + "\" + txtfoldername.Text))
            Directory.CreateDirectory(rootDir.ToString())
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "history.go(0)", True)
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RateUpload.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            tvrupl.SelectedNode.Selected = False
        End Try
    End Sub
    Protected Sub lnkdelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkdelete.Click
        Try
            If Not tvrupl.SelectedNode Is Nothing Then
                Dim strparentnode As String = tvrupl.SelectedNode.ValuePath.ToString()
                Dim rootDir As New DirectoryInfo(Server.MapPath("..\AgentsOnline\" + strparentnode))
                Dim rootfile As New FileInfo(Server.MapPath("..\AgentsOnline\" + strparentnode))
                Dim path As String = System.IO.File.GetAttributes(rootDir.ToString())
                Dim isdir As Boolean = Directory.Exists(rootDir.ToString())
                If isdir = True Then
                    Dim countdir = Directory.GetDirectories(rootDir.ToString()).ToString()
                    Dim countfiles = Directory.GetFiles(rootDir.ToString())
                    If (countfiles.Length > 0 Or tvrupl.SelectedNode.Depth = 2 Or tvrupl.SelectedNode.Depth = 1) Then
                        lblmsg.Visible = True
                        lblmsg.Text = "folder is not allowed to delete"
                        ModalPopupExtender1.Show()
                    Else
                        Directory.Delete(rootDir.ToString())
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "history.go(0)", True)
                    End If

                Else
                    lblmsg.Visible = False
                    File.Delete(rootDir.ToString())
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "history.go(0)", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RateUpload.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            tvrupl.SelectedNode.Selected = False
        End Try
    End Sub
    Dim resultNodes As New Generic.List(Of TreeNode)
    Private Sub GetNodesToRoot(ByVal node As TreeNode)
        If node Is Nothing Then
            Return
        Else
            ' previous node was the root.
            resultNodes.Add(node)
            'resultNodes.Add(node.Parent)
            GetNodesToRoot(node.Parent)
        End If
    End Sub
    Private Function FindRootNode(ByVal treeNode As TreeNode) As TreeNode
        While treeNode.Text IsNot Nothing
            treeNode = treeNode.Parent
        End While
        Return treeNode
    End Function
    Private Sub PrintRecursive(ByVal n As TreeNode)
        Dim aNode As TreeNode
        For Each aNode In n.ChildNodes
            GetNodesToRoot(aNode)
        Next
    End Sub

    ' Call the procedure using the top nodes of the treeview.
    Private Sub CallRecursive(ByVal aTreeView As TreeView)
        Dim n As TreeNode
        For Each n In aTreeView.Nodes
            PrintRecursive(n)
        Next
    End Sub
    Protected Sub btnupload_click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Dim strnodename As String = tvrupl.SelectedNode.Text
        Try
            Dim fullpath As String = String.Empty
            Dim strparentnode As String = tvrupl.SelectedNode.ValuePath.ToString()
            selecteddepth = tvrupl.SelectedNode.Depth
            selectednode = tvrupl.SelectedNode.Text
            CallRecursive(tvrupl)


            Dim filename As String = Path.GetFileName(FileUpload1.FileName)
           


            If FileUpload1.FileName = "" Then
                fullpath = hdnfilename.Value
            End If

            fullpath = FileUpload1.FileName
            If filename = "" Then
                filename = Path.GetFileName(hdnfilename.Value)
                fullpath = hdnfilename.Value
            End If

            If (filename <> "") Then
                'filename = FileUpload1.FileName
                'Else
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "alert", "alert('File Name is invalid');", True)
                '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "history.go(0)", True)
                'Return


                Dim rootDir As String = Server.MapPath("..\AgentsOnline\" + strparentnode + "\" + filename)
                'Dim rootDir As String = Server.MapPath("..\AgentsOnline\" + strparentnode)
                'FileUpload1.SaveAs(rootDir)

                'File.Copy(fullpath, rootDir, True)

                FileUpload1.SaveAs(rootDir)

                'saving in database
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                myCommand = New SqlCommand("sp_fileupload_log", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@filename", SqlDbType.VarChar, 100)).Value = CType(tvrupl.SelectedNode.Text, String)
                myCommand.Parameters.Add(New SqlParameter("@uploadpath", SqlDbType.VarChar, 100)).Value = CType(tvrupl.SelectedNode.ValuePath, String)
                myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                myCommand.ExecuteNonQuery()
                sqlTrans.Commit()                'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)

                If Not tvrupl.SelectedNode Is Nothing Then
                    Dim lbNodefound As Boolean = False
                    For Each lchlnode As TreeNode In tvrupl.SelectedNode.ChildNodes
                        If lchlnode.Text.ToString.Trim.ToUpper = filename.ToString.Trim.ToUpper Then
                            lbNodefound = True
                        End If
                    Next
                    If lbNodefound = False Then
                        Dim thisFileNode As New TreeNode(filename, Nothing)
                        tvrupl.SelectedNode.ChildNodes.Add(thisFileNode)
                    End If
                End If

                '-------------------------------------------
                ' 
                'createtreeview()
                'Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
                ' Response.Redirect("RateUpload.aspx")
                'createtreeview()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File Uploaded Successfully.');", True)
                ' tvrupl.SelectedNodeStyle.ForeColor = Drawing.Color.Orange
                ' tvrupl.SelectedNodeStyle.BackColor = Drawing.Color.Purple

                'refresh()

                'For Each tn As TreeNode In newrootnode.ChildNodes
                '    If tn.Text = selectednode Then
                '        tn.Selected = True
                '        tn.ExpandAll()
                '        Exit For
                '    End If
                'Next
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RateUpload.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            tvrupl.SelectedNode.Selected = False
        End Try


        'tvrupl.CollapseAll()

    End Sub
    Protected Sub btnrename_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnrename.Click
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            myCommand = New SqlCommand("sp_treeviewlog", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@folder", SqlDbType.VarChar, 100)).Value = CType(txtfoldername.Text, String)
            myCommand.Parameters.Add(New SqlParameter("@pfolder", SqlDbType.VarChar, 100)).Value = CType(tvrupl.SelectedNode.Text, String)
            myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()
            myCommand = New SqlCommand("sp_fileupload_log", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@filename", SqlDbType.VarChar, 100)).Value = CType(tvrupl.SelectedNode.Text, String)
            myCommand.Parameters.Add(New SqlParameter("@uploadpath", SqlDbType.VarChar, 100)).Value = CType(tvrupl.SelectedNode.ValuePath, String)
            myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()
            sqlTrans.Commit()                'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)

            Dim strparentnode As String = tvrupl.SelectedNode.ValuePath.ToString()

            Dim rootDir As New DirectoryInfo(Server.MapPath("..\AgentsOnline\" + strparentnode))
            Dim rootfile As New FileInfo(Server.MapPath("..\AgentsOnline\" + strparentnode))

            'Dim path As String = System.IO.File.GetAttributes(rootDir.ToString())
            Dim isdir As Boolean = Directory.Exists(rootDir.ToString())

            If isdir = True Then
                'for folder rename
                '    rootDir.MoveTo(strparentnode.ToString())
                FileIO.FileSystem.RenameDirectory(rootDir.ToString(), txtrename.Text)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "history.go(0)", True)

            Else
                'for file rename

                Dim extension As String = rootfile.Extension
                FileIO.FileSystem.RenameFile(rootfile.ToString(), txtrename.Text + extension)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "history.go(0)", True)
            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RateUpload.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            tvrupl.SelectedNode.Selected = False
        End Try
    End Sub

    Protected Sub lnkcancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkcancel.Click
        Try
            tvrupl.SelectedNode.Selected = False
        Catch ex As Exception

        End Try
    End Sub

    'Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click
    '    Try
    '        tvrupl.SelectedNode.Selected = False
    '    Catch ex As Exception

    '    End Try
    'End Sub

  
End Class
