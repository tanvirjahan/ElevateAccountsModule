''--------------------------------------------------------------------------------------------------------
'   Module Name    :    AcctGroupsSearch.aspx
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'--------------------------------------------------------------------------------------------------------
 #Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class AcctGroupsSearch
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
    Dim dtExpand As DataTable
    Dim strappid As String = ""
    Dim strappname As String = ""

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)


        If appid Is Nothing = False Then
            'If appid = "4" Then
            '    strappname = AppName.Value
            'Else
            '    strappname = AppName.Value
            'End If
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
        End If
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")



        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        '   Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        ViewState.Add("divcode", divid)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
           
           

            txtDivcode.Value = ViewState("divcode")

            strappid = CType(Request.QueryString("appid"), String) ' CType(Session("sAppId"), String)
           
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
            strappname = ViewState("Appname") ' Session("AppName")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            If Page.IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub

                Else

                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                  CType(strappname, String), "AccountsModule\AcctGroupsSearch.aspx?appid=" + strappid, btnadd, btnEdit, _
                                                  btndelete, btnView, btnExcel, BtnPrint)
                End If


                'Dim intAppId As Integer
                'intAppId = objUser.GetAppId(Session("dbconnectionName"), CType(Session("AppName"), String))


                'ViewState.Add("divcode", divid)


                Session("treeExpnd") = Nothing
                PopulateRootLevel()

                tvAccgrp.Attributes.Add("OnClick", "client_OnTreeNodeChecked(event)")
            End If

        Catch ex As Exception
            mySqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AcctGroupsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AcctgrpWindowPostBack") Then
            CheckTreeExpanded(tvAccgrp.Nodes)
            tvAccgrp.Nodes.Clear()
            PopulateRootLevel()
            SetTreeExpanded(tvAccgrp.Nodes)
        End If


    End Sub
#End Region

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

#Region "Fill Tree View"
    Private Sub PopulateRootLevel()
        'Dim objConn As New SqlConnection("data source=jmserver\jmserver;persist security info=True;initial catalog=WondersOnline;user id=sa;pwd=mis")
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        strQry = "select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup   WHERE div_code='" & ViewState("divcode") & "' and childid=sc.parentID  and accttype=1) childnodecount FROM acctgroup sc where  div_code='" & ViewState("divcode") & "' and childid =0 and accttype=1" 'or  childid =0"
        Dim objCommand As New SqlCommand(strQry, mySqlConn)
        Dim da As New SqlDataAdapter(objCommand)
        Dim dt As New DataTable()
        da.Fill(dt)
        PopulateNodes(dt, tvAccgrp.Nodes)
        mySqlConn.Close()
    End Sub

    Private Sub PopulateSubLevel(ByVal parentid As Integer, ByVal parentNode As TreeNode)
        '    Dim strDelimiter As String
        '    strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", "123")

        'Dim objConn As New SqlConnection("data source=jmserver\jmserver;persist security info=True;initial catalog=WondersOnline;user id=sa;pwd=mis")
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        strQry = " select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup WHERE div_code='" & ViewState("divcode") & "' and accttype=1 and  childid=sc.parentID) childnodecount FROM acctgroup sc where div_code='" & ViewState("divcode") & "' and  accttype=1 and childid=" & parentid
        Dim objCommand As New SqlCommand(strQry, mySqlConn)
        objCommand.Parameters.Add("@parentID", SqlDbType.Int).Value = parentid
        Dim da As New SqlDataAdapter(objCommand)
        Dim dt As New DataTable()
        da.Fill(dt)
        PopulateNodes(dt, parentNode.ChildNodes)
        mySqlConn.Close()
    End Sub

    Private Sub PopulateNodes(ByVal dt As DataTable, ByVal nodes As TreeNodeCollection)
        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")
        For Each dr As DataRow In dt.Rows
            Dim tn As New TreeNode()
            tn.Text = dr("acctcode").ToString() & strDelimiter & dr("acctname").ToString()
            tn.Value = dr("id").ToString()
            nodes.Add(tn)
            'If node has child nodes, then enable on-demand populating
            tn.PopulateOnDemand = (CInt(dr("childnodecount")) > 0)
        Next

    End Sub
    Protected Sub tvAccgrp_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles tvAccgrp.TreeNodePopulate
        PopulateSubLevel(CInt(e.Node.Value), e.Node)
    End Sub
#End Region

#Region "Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd.Click"
    Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd.Click
        ' Dim i As Long
        Dim intSelPrantid As Long
        Dim intNewParent As Long

        Dim intchkLeval As String
        Dim intselLeval As Long

        If tvAccgrp.CheckedNodes.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Account Group.');", True)
            Exit Sub
        End If

        Dim tnCollection As TreeNodeCollection = tvAccgrp.CheckedNodes

        intchkLeval = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "507")

        Dim strValuPath As String = tnCollection(0).ValuePath   'First chk in array

        Dim strSplitValuPath As String() = strValuPath.Split("/")

        intselLeval = strSplitValuPath.Length

        intSelPrantid = tnCollection(0).Value

        intNewParent = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select max(isnull(parentid,0))+1 from acctgroup where div_code='" & ViewState("divcode") & "'")

        'Session.Add("ChildId", intSelPrantid)
        'Session.Add("AccLevel", intselLeval)
        'Session.Add("ParentId", intNewParent)
        'Session.Add("Level", chkLevel(tnCollection, intselLeval, intNewParent))
        'ViewState.Add("ChildId", intSelPrantid)
        'ViewState.Add("AccLevel", intselLeval)
        'ViewState.Add("ParentId", intNewParent)
        'ViewState.Add("Level", chkLevel(tnCollection, intselLeval, intNewParent))
        If intchkLeval = intselLeval Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user cannot create  group under the " & intselLeval & " level');", True)
            Exit Sub
        Else


            Dim strSplitcode As String() = tnCollection(0).Text.Split(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123"))
            Dim ord As String

            ord = IIf(strSplitcode(0) = "0", "", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctorder from acctgroup  where div_code='" & ViewState("divcode") & "' and acctcode='" & strSplitcode(0) & "'"))

            'Session("State") = "New"
            'Response.Redirect("AcctGroup.aspx?ord=" & ord, False)
            Dim strpop As String = ""
            strpop = "window.open('AcctGroup.aspx?State=New&ord=" & ord & "&divid=" & ViewState("divcode") & "&ChildId=" & intSelPrantid & "&AccLevel=" & intselLeval & "&Parentid=" & intNewParent & "&Level=" & chkLevel(tnCollection, intselLeval, intNewParent) & "','AcctGroup','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        End If
    End Sub
#End Region

#Region "Public Function chkLevel(ByVal tnCollection As TreeNodeCollection, ByVal AccLevel As Integer, ByVal intNewParent As Long) As String"

    Public Function chkLevel(ByVal tnCollection As TreeNodeCollection, ByVal AccLevel As Integer, ByVal intNewParent As Long) As String
        chkLevel = ""
        Select Case AccLevel
            Case 1
                chkLevel = intNewParent         'Leval 1
                chkLevel = chkLevel & "-" & 0   'Leval 2
                chkLevel = chkLevel & "-" & 0   'Leval 3
                chkLevel = chkLevel & "-" & 0   'Leval 4
                chkLevel = chkLevel & "-" & 0   'Leval 5
                chkLevel = chkLevel & "-" & 0   'Leval 6
                chkLevel = chkLevel & "-" & 0   'Leval 7
            Case 2
                chkLevel = tnCollection(0).Value          'Leval 1
                chkLevel = chkLevel & "-" & intNewParent  'Leval 2
                chkLevel = chkLevel & "-" & 0   'Leval 3
                chkLevel = chkLevel & "-" & 0   'Leval 4
                chkLevel = chkLevel & "-" & 0   'Leval 5
                chkLevel = chkLevel & "-" & 0   'Leval 6
                chkLevel = chkLevel & "-" & 0   'Leval 7
            Case 3
                chkLevel = tnCollection(0).Parent.Value             'Leval 1
                chkLevel = chkLevel & "-" & tnCollection(0).Value   'Leval 2
                chkLevel = chkLevel & "-" & intNewParent            'Leval 3
                chkLevel = chkLevel & "-" & 0   'Leval 4
                chkLevel = chkLevel & "-" & 0   'Leval 5
                chkLevel = chkLevel & "-" & 0   'Leval 6
                chkLevel = chkLevel & "-" & 0   'Leval 7
            Case 4
                chkLevel = tnCollection(0).Parent.Parent.Value             'Leval 1
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Value   'Leval 2
                chkLevel = chkLevel & "-" & tnCollection(0).Value          'Leval 3
                chkLevel = chkLevel & "-" & intNewParent                   'Leval 4
                chkLevel = chkLevel & "-" & 0   'Leval 5
                chkLevel = chkLevel & "-" & 0   'Leval 6
                chkLevel = chkLevel & "-" & 0   'Leval 7
            Case 5
                chkLevel = tnCollection(0).Parent.Parent.Parent.Value             'Leval 1
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Parent.Value   'Leval 2
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Value          'Leval 3
                chkLevel = chkLevel & "-" & tnCollection(0).Value                 'Leval 4
                chkLevel = chkLevel & "-" & intNewParent                          'Leval 5
                chkLevel = chkLevel & "-" & 0   'Leval 6
                chkLevel = chkLevel & "-" & 0   'Leval 7
            Case 6
                chkLevel = tnCollection(0).Parent.Parent.Parent.Parent.Value             'Leval 1
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Parent.Parent.Value   'Leval 2
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Parent.Value          'Leval 3
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Value                 'Leval 4
                chkLevel = chkLevel & "-" & tnCollection(0).Value                        'Leval 5
                chkLevel = chkLevel & "-" & intNewParent                                 'Leval 6
                chkLevel = chkLevel & "-" & 0   'Leval 7
            Case 7
                chkLevel = tnCollection(0).Parent.Parent.Parent.Parent.Parent.Value             'Leval 1
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Parent.Parent.Parent.Value   'Leval 2
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Parent.Parent.Value          'Leval 3
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Parent.Value                 'Leval 4
                chkLevel = chkLevel & "-" & tnCollection(0).Parent.Value                        'Leval 5
                chkLevel = chkLevel & "-" & tnCollection(0).Value                               'Leval 6
                chkLevel = chkLevel & "-" & intNewParent                                        'Leval 7

        End Select

    End Function
#End Region

#Region "Public Function chkEditLevel(ByVal tnCollection As TreeNodeCollection, ByVal AccLevel As Integer, ByVal intNewParent As Long) As String"
    Public Function chkEditLevel(ByVal tnCollection As TreeNodeCollection, ByVal AccLevel As Integer, ByVal intNewParent As Long) As String
        chkEditLevel = ""
        Select Case AccLevel
            Case 1
                chkEditLevel = intNewParent         'Leval 1
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 2
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 3
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 4
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 5
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 6
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 7
            Case 2
                chkEditLevel = tnCollection(0).Parent.Value       'Leval 1
                chkEditLevel = chkEditLevel & "-" & intNewParent  'Leval 2
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 3
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 4
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 5
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 6
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 7
            Case 3
                chkEditLevel = tnCollection(0).Parent.Parent.Value                    'Leval 1
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Value      'Leval 2
                chkEditLevel = chkEditLevel & "-" & intNewParent                      'Leval 3
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 4
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 5
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 6
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 7
            Case 4
                chkEditLevel = tnCollection(0).Parent.Parent.Parent.Value                  'Leval 1
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Value    'Leval 2
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Value           'Leval 3
                chkEditLevel = chkEditLevel & "-" & intNewParent                           'Leval 4
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 5
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 6
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 7
            Case 5
                chkEditLevel = tnCollection(0).Parent.Parent.Parent.Parent.Value                 'Leval 1
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Parent.Value   'Leval 2
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Value        'Leval 3
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Value              'Leval 4
                chkEditLevel = chkEditLevel & "-" & intNewParent                              'Leval 5
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 6
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 7
            Case 6
                chkEditLevel = tnCollection(0).Parent.Parent.Parent.Parent.Parent.Value                 'Leval 1
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Parent.Parent.Value   'Leval 2
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Parent.Value          'Leval 3
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Value                 'Leval 4
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Value                        'Leval 5
                chkEditLevel = chkEditLevel & "-" & intNewParent                                        'Leval 6
                chkEditLevel = chkEditLevel & "-" & 0   'Leval 7
            Case 7
                chkEditLevel = tnCollection(0).Parent.Parent.Parent.Parent.Parent.Parent.Value                 'Leval 1
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Parent.Parent.Parent.Value   'Leval 2
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Parent.Parent.Value          'Leval 3
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Parent.Value                 'Leval 4
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Parent.Value                        'Leval 5
                chkEditLevel = chkEditLevel & "-" & tnCollection(0).Parent.Value                               'Leval 6
                chkEditLevel = chkEditLevel & "-" & intNewParent                                               'Leval 7
        End Select
    End Function
#End Region

#Region "Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click"
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim intSelPrantid As Long
        Dim intNewParent As Long

        Dim intselLeval As Long

        If tvAccgrp.CheckedNodes.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Account Group.');", True)
            Exit Sub
        End If

        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")

        Dim tnCollection As TreeNodeCollection = tvAccgrp.CheckedNodes

        Dim strValuPath As String = tnCollection(0).ValuePath
        Dim strSplitValuPath As String() = strValuPath.Split("/")
        intselLeval = strSplitValuPath.Length - 1

        If intselLeval = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user cannot edit this group');", True)
            Exit Sub
        End If

        intSelPrantid = tnCollection(0).Parent.Value

        intNewParent = tnCollection(0).Value 'Current Selected

        'Session.Add("ChildId", intSelPrantid)
        'Session.Add("AccLevel", intselLeval)
        'Session.Add("ParentId", intNewParent)
        'Session.Add("Level", chkEditLevel(tnCollection, intselLeval, intNewParent))

        Dim strTemp As String() = tnCollection(0).Text.Split(strDelimiter)

        'ViewState("AcctgrpRefCode") = strTemp.GetValue(0)


        Dim strSplitcode As String() = tnCollection(0).Text.Split(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123"))
        Dim ord As String

        ord = IIf(strSplitcode(0) = "0", "", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctorder from acctgroup  where div_code='" & ViewState("divcode") & "' and  acctcode='" & strSplitcode(0) & "'"))


        'Session("State") = "Edit"
        'Response.Redirect("AcctGroup.aspx?ord=" & ord, False)
        Dim strpop As String = ""
        strpop = "window.open('AcctGroup.aspx?State=Edit&ord=" & ord & "&divid=" & ViewState("divcode") & "&RefCode=" & strTemp.GetValue(0) & "&ChildId=" & intSelPrantid & "&AccLevel=" & intselLeval & "&Parentid=" & intNewParent & "&Level=" & chkEditLevel(tnCollection, intselLeval, intNewParent) & "','AcctGroup','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region

#Region "Protected Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelete.Click"
    Protected Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelete.Click

        Dim intNewParent As Long
        Dim intselLeval As Long

        If tvAccgrp.CheckedNodes.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Account Group.');", True)
            Exit Sub
        End If

        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")

        Dim tnCollection As TreeNodeCollection = tvAccgrp.CheckedNodes
        Dim strValuPath As String = tnCollection(0).ValuePath
        Dim strSplitValuPath As String() = strValuPath.Split("/")
        intselLeval = strSplitValuPath.Length - 1



        If intselLeval = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user cannot delete this group');", True)
            Exit Sub
        End If

        intNewParent = tnCollection(0).Value 'Current Selected

        If objUtils.GetDBFieldFromLongnewdiv(Session("dbconnectionName"), "acctgroup", "parentid", "childid", intNewParent, "div_code", ViewState("divcode")) <> 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user first delete child account group.');", True)
            Exit Sub
        End If

        'Session.Add("ParentId", intNewParent)
        Dim strTemp As String() = tnCollection(0).Text.Split(strDelimiter)
        'Session("RefCode") = strTemp.GetValue(0)
        'Session("State") = "Delete"


        Dim strSplitcode As String() = tnCollection(0).Text.Split(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123"))
        Dim ord As String

        ord = IIf(strSplitcode(0) = "0", "", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctorder from acctgroup  where div_code='" & ViewState("divcode") & "' and  acctcode='" & strSplitcode(0) & "'"))

        'Response.Redirect("AcctGroup.aspx?ord=" & ord, False)
        Dim strpop As String = ""
        strpop = "window.open('AcctGroup.aspx?State=Delete&ord=" & ord & "&divid=" & ViewState("divcode") & "&ParentId=" & intNewParent & "&RefCode=" & strTemp.GetValue(0) & "','AcctGroup','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region

    Sub Search_node(ByVal tnode As TreeNode)
        tnode.Select()
        tvAccgrp.SelectedNodeStyle.BackColor = Drawing.Color.Bisque
    End Sub

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim node0, node1, node2, node3, node4, node5, node6, node7 As TreeNode
        Dim nodevalue1 As Long
        Dim nodevalue2 As Long
        Dim nodevalue3 As Long
        Dim nodevalue4 As Long
        Dim nodevalue5 As Long
        Dim nodevalue6 As Long
        Dim nodevalue7 As Long
        Dim i, j As Integer
        For i = 0 To tvAccgrp.Nodes.Count - 1
            For j = 0 To tvAccgrp.Nodes.Item(i).ChildNodes.Count - 1
                If tvAccgrp.SelectedNodeStyle.BackColor = Drawing.Color.Bisque Then
                    tvAccgrp.SelectedNodeStyle.BackColor = Drawing.Color.White
                    Exit For
                End If
            Next
        Next
        If txtSerch.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter in search text.');", True)
            Exit Sub
        End If


        Dim strQry As String = ""
        If ddlAccountSearchType.Value = "Account Name" Then
            'mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"),"select * from accrep where accname='" & txtSerch.Text & "'")
            'mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "select * from accrep where accname LIKE '%" & txtSerch.Text & "%'")
            strQry = "select accrep.level1 ,accrep.level2,accrep.level3,accrep.level4,accrep.level5,accrep.level6,accrep.level7," & _
                "accrep.acccode ,accrep.accname from accrep inner join acctgroup on accrep.acccode = acctgroup.acctcode and accrep.divcode=acctgroup.div_code " & _
                " where acctgroup.div_code='" & ViewState("divcode") & "' and accttype =1 and accrep.accname like '%" & txtSerch.Text & "%'"
            mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strQry)

        ElseIf ddlAccountSearchType.Value = "Account Code" Then
            'mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"),"select * from accrep where acccode='" & txtSerch.Text & "'")
            strQry = "select accrep.level1 ,accrep.level2,accrep.level3,accrep.level4,accrep.level5,accrep.level6,accrep.level7," & _
                "accrep.acccode ,accrep.accname from accrep inner join acctgroup on accrep.acccode = acctgroup.acctcode and accrep.divcode=acctgroup.div_code " & _
                " where acctgroup.div_code='" & ViewState("divcode") & "' and acctgroupaccttype =1 and accrep.acccode like '%" & txtSerch.Text & "%'"
            mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strQry)

        End If

        If mySqlReader Is Nothing Then
            Exit Sub
        End If

        If mySqlReader.Read Then
            nodevalue1 = mySqlReader("level1")
            nodevalue2 = mySqlReader("level2")
            nodevalue3 = mySqlReader("level3")
            nodevalue4 = mySqlReader("level4")
            nodevalue5 = mySqlReader("level5")
            nodevalue6 = mySqlReader("level6")
            nodevalue7 = mySqlReader("level7")
        Else
            Exit Sub
            '  tvAccgrp.SelectedNodeStyle.BackColor = Drawing.Color.Bisque
        End If


        For Each node0 In tvAccgrp.Nodes  '0
            node0.Expand()
            For Each node1 In node0.ChildNodes '1
                If node1.Value = nodevalue1 Then
                    node1.ExpandAll()
                    For Each node2 In node1.ChildNodes '2
                        If node2.Value = nodevalue2 Then
                            node2.Expand()
                            For Each node3 In node2.ChildNodes '3
                                If node3.Value = nodevalue3 Then
                                    node3.Expand()
                                    For Each node4 In node3.ChildNodes '4
                                        If node4.Value = nodevalue4 Then
                                            node4.Expand()
                                            For Each node5 In node4.ChildNodes '5
                                                If node5.Value = nodevalue5 Then
                                                    node5.Expand()
                                                    For Each node6 In node5.ChildNodes '6
                                                        If node6.Value = nodevalue6 Then
                                                            node6.Expand()
                                                            For Each node7 In node6.ChildNodes '7
                                                                If node7.Value = nodevalue7 Then
                                                                    'node7.Expand()
                                                                    Search_node(node7)
                                                                    Exit Sub
                                                                End If
                                                            Next
                                                            Search_node(node6)
                                                            Exit Sub
                                                        End If
                                                    Next
                                                    Search_node(node5)
                                                    Exit Sub
                                                End If
                                            Next
                                            Search_node(node4)
                                            Exit Sub
                                        End If
                                    Next
                                    Search_node(node3)
                                    Exit Sub
                                End If
                            Next
                            Search_node(node2)
                            Exit Sub
                        End If
                    Next
                    Search_node(node1)
                    Exit Sub
                End If
            Next
        Next
    End Sub
#End Region

    'Private Function GetNodes(ByVal nodevalue As String) As TreeNode
    '    Dim enode As TreeNode
    '    Dim node0, node1, node2, node3, node4, node5, node6, node7 As TreeNode
    '    Dim strSplitValuPath(10) As String '= nodevalue.Split("/")
    '    strSplitValuPath = nodevalue.Split("/")


    '    For Each node0 In tvAccgrp.Nodes  '0
    '        For Each node1 In node0.ChildNodes '1
    '            If node1.Value = strSplitValuPath(0) Then
    '                If txtParentId.Value = strSplitValuPath(0) Then
    '                    GetNodes = node1
    '                    Exit Function
    '                Else
    '                    node1.Expand()
    '                End If
    '            Else
    '                node1.Expand()
    '                For Each node2 In node1.ChildNodes '2
    '                    If node2.Value = strSplitValuPath(1) Then
    '                        GetNodes = node2
    '                        Exit Function
    '                    Else
    '                        node2.Expand()
    '                        For Each node3 In node2.ChildNodes '3
    '                            If node3.Value = strSplitValuPath(2) Then
    '                                GetNodes = node3
    '                                Exit Function
    '                            Else
    '                                node3.Expand()
    '                                For Each node4 In node3.ChildNodes '4
    '                                    If node4.Value = strSplitValuPath(3) Then
    '                                        GetNodes = node4
    '                                        Exit Function
    '                                    Else
    '                                        node4.Expand()
    '                                        For Each node5 In node4.ChildNodes '5
    '                                            If node5.Value = strSplitValuPath(4) Then
    '                                                GetNodes = node5
    '                                                Exit Function
    '                                            Else
    '                                                node5.Expand()
    '                                                For Each node6 In node5.ChildNodes '6
    '                                                    If node6.Value = strSplitValuPath(5) Then
    '                                                        GetNodes = node6
    '                                                        Exit Function
    '                                                    Else
    '                                                        node6.Expand()
    '                                                        For Each node7 In node6.ChildNodes '7
    '                                                            If node7.Value = strSplitValuPath(6) Then
    '                                                                'node7.Expand()
    '                                                                'Search_node(node7)
    '                                                                GetNodes = node7
    '                                                                Exit Function
    '                                                            End If
    '                                                        Next
    '                                                    End If
    '                                                Next
    '                                            End If
    '                                        Next
    '                                    End If
    '                                Next
    '                            End If
    '                        Next
    '                    End If
    '                Next
    '            End If
    '        Next
    '    Next


    '    Return enode

    'End Function

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try

            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            ''Session("ColReportParams") = Nothing
            ''Session.Add("Pageame", "Account Group")
            ''Session.Add("BackPageName", "AcctGroupsSearch.aspx")

            'If txtSerch.Text <> "" Then
            '    If ddlAccountSearchType.Value = "Account Code" Then
            '        '        strReportTitle = "Account Group Code: " & txtSerch.Text.Trim
            '        '        strSelectionFormula = " {acctgroup.acctcode} LIKE '" & txtSerch.Text.Trim & "*'"
            '        '    Else
            '        '        strReportTitle = "Account Group Name: " & txtSerch.Text.Trim
            '        '        strSelectionFormula = " {acctgroup.acctname} LIKE '" & txtSerch.Text.Trim & "*'"

            '        '    End If
            '        'End If

            '        Dim strpop As String = ""
            '        strpop = "window.open('rptReportNew.aspx?Pageame=Account Group&BackPageName=AcctGroupsSearch.aspx&AcctgrpCode=" & txtSerch.Text.Trim & "&divid=" & ViewState("divcode") & "','AcctgrpMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            '    Else
            '        Dim strpop As String = ""
            '        strpop = "window.open('rptReportNew.aspx?Pageame=Account Group&BackPageName=AcctGroupsSearch.aspx&AcctgrpName=" & txtSerch.Text.Trim & "&divid=" & ViewState("divcode") & "','AcctgrpMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            '    End If
            'Else
            '    Dim strpop As String = ""
            '    strpop = "window.open('rptReportNew.aspx?Pageame=Account Group&BackPageName=AcctGroupsSearch.aspx&divid=" & ViewState("divcode") & "','AcctgrpMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'End If

            Dim DS As New DataSet
            Dim DA As SqlDataAdapter
            Dim con As SqlConnection
            Dim objcon As New clsDBConnect

            Dim strsqlqry As String
            strsqlqry = "select concat(char(30),case acctlevel when 1 then acctcode else '-' end) level1," &
                        "concat( char(30),case acctlevel when 2 then acctcode else '-' end )level2," &
                            "concat(char(30),case acctlevel when 3 then acctcode else '-' end )level3," &
                    "concat(char(30),case acctlevel when 4 then acctcode else '-' end )level4," &
                    "case  when acctlevel>=5 then acctcode else '-' end  Detail,acctname Name" &
                        " from acctgroup where div_code='" & ViewState("divcode") & "' and accttype=1"

            If txtSerch.Text <> "" Then
                If ddlAccountSearchType.Value = "Account Code" Then
                    strsqlqry = strsqlqry & " and acctcode LIKE '" & txtSerch.Text.Trim & "%'"
                Else
                    strsqlqry = strsqlqry & " and acctname LIKE '" & txtSerch.Text.Trim & "%'"
                End If
            End If

            con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strsqlqry, con)
            DA = New SqlDataAdapter(strsqlqry, con)
            DA.Fill(DS, "acctgroup")
            objUtils.ExportToExcelnew(DS, Response, "Accounts Groups")

            con.Close()

        Catch ex As Exception
            objUtils.WritErrorLog("AcctGroupsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=AcctGroupsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
       
    End Sub
End Class
