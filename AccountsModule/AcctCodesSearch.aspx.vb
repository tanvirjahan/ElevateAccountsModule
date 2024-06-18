''--------------------------------------------------------------------------------------------------------
'   Module Name    :    AcctCodesSearch 
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'--------------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class AcctCodesSearch
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

      


        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        ViewState.Add("divcode", divid)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            
            'Dim intAppId As Integer
            'intAppId = objUser.GetAppId(Session("dbconnectionName"), CType(Session("AppName"), String))
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim AppId As String = CType(Request.QueryString("appid"), String)


            If CType(Session("sAppId"), String) Is Nothing = False Then
                strappid = CType(Session("sAppId"), String)
            End If
            'If CType(Session("AppName"), String) Is Nothing = False Then
            '    '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
            '    strappname = Session("AppName")
            '    '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            'End If

            If AppId Is Nothing = False Then
                'If AppId = "4" Then
                '    strappname = AppName.Value
                'Else
                '    strappname = AppName.Value
                'End If
                strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & AppId & "'")
            End If

            'ViewState.Add("divcode", divid)
            ViewState("Appname") = strappname
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")

            ViewState.Add("divcode", divid)
            txtDivcode.Value = ViewState("divcode")


            If Page.IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                  CType(strappname, String), "AccountsModule\AcctCodesSearch.aspx?appid=" + strappid, btnadd, btnEdit, _
                                                  btndelete, btnview, btnExcel, BtnPrint)

                End If

                Session("treeExpnd") = Nothing
                PopulateRootLevel()

                tvAccgrp.Attributes.Add("OnClick", "client_OnTreeNodeChecked(event)")
            End If

        Catch ex As Exception
            mySqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AcctGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AcctcodeWindowPostBack") Then
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
        strQry = "select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup   WHERE div_code='" & txtDivcode.Value & "' and childid=sc.parentID  and accttype=1) childnodecount FROM acctgroup sc where div_code='" & txtDivcode.Value & "' and childid =0 and accttype=1"
        Dim objCommand As New SqlCommand(strQry, mySqlConn)
        Dim da As New SqlDataAdapter(objCommand)
        Dim dt As New DataTable()
        da.Fill(dt)
        PopulateNodes(dt, tvAccgrp.Nodes)
        mySqlConn.Close()
    End Sub

    Private Sub PopulateSubLevel(ByVal parentid As Integer, ByVal parentNode As TreeNode)
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        'If parentid = 1 Then
        '    strQry = " select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup WHERE accttype=1 and  childid=sc.parentID) childnodecount FROM acctgroup sc where   accttype=1 and childid=" & parentid
        'Else

        'End If

        '  strQry = " select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup WHERE accttype=2 and  childid=sc.parentID) childnodecount FROM acctgroup sc where    childid=" & parentid  'accttype=2 and
        If checkFinalGroup(parentid) = False Then
            strQry = " select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup WHERE div_code='" & txtDivcode.Value & "' and accttype=1 and  childid=sc.parentID) childnodecount FROM acctgroup sc where div_code='" & txtDivcode.Value & "' and  accttype=1 and childid=" & parentid
        Else
            strQry = " select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup WHERE div_code='" & txtDivcode.Value & "' and accttype=2 and  childid=sc.parentID) childnodecount FROM acctgroup sc where  div_code='" & txtDivcode.Value & "' and  childid=" & parentid
        End If

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

    Private Function checkFinalGroup(ByVal parentid As Integer) As Boolean
        checkFinalGroup = True
        Dim intchkLeval As String
        Dim intselLeval As Long

        intselLeval = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctlevel from acctgroup where div_code='" & txtDivcode.Value & "' and childid='" & parentid & "'")
        intchkLeval = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "507")

        If (intselLeval = intchkLeval) Or (intselLeval = intchkLeval - 1) Then
            checkFinalGroup = True
            Exit Function
        Else
            checkFinalGroup = False
            Exit Function
        End If
        checkFinalGroup = True
    End Function
#Region "Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd.Click"
    Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd.Click
        ' Dim i As Long
        Dim intSelPrantid As Long
        Dim intNewParent As Long
        Dim intchkLeval As String
        Dim intselLeval As Long

        If tvAccgrp.CheckedNodes.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account group.');", True)
            Exit Sub
        End If

        Dim tnCollection As TreeNodeCollection = tvAccgrp.CheckedNodes

        If tnCollection(0).Value = 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('User can not add account code  under Root Accounts.');", True)
            Exit Sub
        End If


        Dim strValuPath As String = tnCollection(0).ValuePath   'First chk in array

        Dim strSplitValuPath As String() = strValuPath.Split("/")

        intselLeval = strSplitValuPath.Length

        intSelPrantid = tnCollection(0).Value

        If objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "acctgroup", "parentid", "accttype=2 and parentid=" & intSelPrantid, "div_code", txtDivcode.Value) <> 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('User can not add account code under account code.');", True)
            Exit Sub
        End If

        intNewParent = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select max(isnull(parentid,0))+1 from acctgroup where div_code='" & txtDivcode.Value & "'")

        'Session.Add("ChildId", intSelPrantid)
        'Session.Add("AccLevel", intselLeval)
        'Session.Add("ParentId", intNewParent)
        'Session.Add("Level", chkLevel(tnCollection, intselLeval, intNewParent))

        intchkLeval = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "507")

        If intchkLeval = intselLeval Then
            'Session("State") = "New"

            Dim strSplitcode As String() = tnCollection(0).Text.Split(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123"))
            Dim ord As String

            ord = IIf(strSplitcode(0) = "0", " ", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctorder from acctgroup  where div_code='" & txtDivcode.Value & "' and acctcode='" & strSplitcode(0) & "'"))

            'Response.Redirect("AcctCode.aspx?ord=" & ord, False)
            Dim strpop As String = ""
            strpop = "window.open('AcctCode.aspx?State=New&ord=" & ord & "&divid=" & txtDivcode.Value & "&ChildId=" & intSelPrantid & "&AccLevel=" & intselLeval & "&Parentid=" & intNewParent & "&Level=" & chkLevel(tnCollection, intselLeval, intNewParent) & "','AcctCode','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user cannot create  account  under the " & intselLeval & " Leval');", True)
            Exit Sub
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account code .');", True)
            Exit Sub
        End If

        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")

        Dim tnCollection As TreeNodeCollection = tvAccgrp.CheckedNodes

        Dim strValuPath As String = tnCollection(0).ValuePath
        Dim strSplitValuPath As String() = strValuPath.Split("/")
        intselLeval = strSplitValuPath.Length - 1

        If intselLeval = 0 Or intselLeval = 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user can not edit this group.');", True)
            Exit Sub
        End If

        intSelPrantid = tnCollection(0).Parent.Value

        intNewParent = tnCollection(0).Value 'Current Selected

        If objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "acctgroup", "parentid", "accttype=1 and parentid=" & intNewParent, "div_code", txtDivcode.Value) <> 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user can not edit this group.');", True)
            Exit Sub
        End If

        'Session.Add("ChildId", intSelPrantid)
        'Session.Add("AccLevel", intselLeval)
        'Session.Add("ParentId", intNewParent)
        'Session.Add("Level", chkEditLevel(tnCollection, intselLeval, intNewParent))


        Dim strTemp As String() = tnCollection(0).Text.Split(strDelimiter)

        'Session("RefCode") = strTemp.GetValue(0)

        'Session("State") = "Edit"

        Dim strSplitcode As String() = tnCollection(0).Text.Split(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123"))
        Dim ord As String

        ord = IIf(strSplitcode(0) = "0", " ", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctorder from acctgroup  where div_code='" & txtDivcode.Value & "' and  acctcode='" & strSplitcode(0) & "'"))

        'Response.Redirect("AcctCode.aspx?ord=" & ord, False)
        Dim strpop As String = ""
        strpop = "window.open('AcctCode.aspx?State=Edit&ord=" & ord & "&divid=" & txtDivcode.Value & "&RefCode=" & strTemp.GetValue(0) & "&ChildId=" & intSelPrantid & "&AccLevel=" & intselLeval & "&Parentid=" & intNewParent & "&Level=" & chkEditLevel(tnCollection, intselLeval, intNewParent) & "','AcctCode','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


    End Sub
#End Region
#Region "Protected Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelete.Click"
    Protected Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelete.Click

        Dim intNewParent As Long
        Dim intselLeval As Long

        If tvAccgrp.CheckedNodes.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Account code  .');", True)
            Exit Sub
        End If

        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")

        Dim tnCollection As TreeNodeCollection = tvAccgrp.CheckedNodes
        Dim strValuPath As String = tnCollection(0).ValuePath
        Dim strSplitValuPath As String() = strValuPath.Split("/")
        intselLeval = strSplitValuPath.Length - 1

        If intselLeval = 0 Or intselLeval = 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user cannot delete this group');", True)
            Exit Sub
        End If

        intNewParent = tnCollection(0).Value 'Current Selected

        If objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "acctgroup", "parentid", "accttype=1 and parentid=" & intNewParent, "div_code", txtDivcode.Value) <> 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user can not delete this group.');", True)
            Exit Sub
        End If

        'Session.Add("ParentId", intNewParent)
        Dim strTemp As String() = tnCollection(0).Text.Split(strDelimiter)
        'Session("RefCode") = strTemp.GetValue(0)
        'Session("State") = "Delete"
        Dim strSplitcode As String() = tnCollection(0).Text.Split(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123"))
        Dim ord As String

        ord = IIf(strSplitcode(0) = "0", " ", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctorder from acctgroup  where div_code='" & txtDivcode.Value & "' and  acctcode='" & strSplitcode(0) & "'"))
        Dim strpop As String = ""
        strpop = "window.open('AcctCode.aspx?State=Delete&ord=" & ord & "&divid=" & txtDivcode.Value & "&ParentId=" & intNewParent & "&RefCode=" & strTemp.GetValue(0) & "','AcctCode','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        'Response.Redirect("AcctCode.aspx?ord=" & ord, False)
    End Sub
#End Region
#Region "Sub Search_node(ByVal tnode As TreeNode)"
    Sub Search_node(ByVal tnode As TreeNode)
        tnode.Select()
        tvAccgrp.SelectedNodeStyle.BackColor = Drawing.Color.Bisque
    End Sub
#End Region
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Search Text');", True)
            SetFocus("txtSerch")
            Exit Sub
        End If

        'If ddlAccountSearchType.Value = "Account Name" Then
        '    'mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"),"select * from accrep where accname='" & txtSerch.Text & "'")
        '    mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "select * from acctgroup where acctname LIKE '%" & txtSerch.Text & "%' and accttype =2")
        'ElseIf ddlAccountSearchType.Value = "Account Code" Then
        '    'mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"),"select * from accrep where acccode='" & txtSerch.Text & "'")
        '    mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "select * from acctgroup where acctcode LIKE '%" & txtSerch.Text & "%' and accttype =2")
        'End If
        Dim strQry As String = ""
        If ddlAccountSearchType.Value = "Account Name" Then

            strQry = "select accrep.level1 ,accrep.level2,accrep.level3,accrep.level4,accrep.level5,accrep.level6,accrep.level7," & _
                "accrep.acccode ,accrep.accname from accrep inner join acctgroup on accrep.acccode = acctgroup.acctcode and accrep.divcode=acctgroup.div_code " & _
                " where acctgroup.div_code='" & txtDivcode.Value & "' and accttype =2 and accrep.accname like '%" & txtSerch.Text & "%'"
            mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strQry)

        ElseIf ddlAccountSearchType.Value = "Account Code" Then

            strQry = "select accrep.level1 ,accrep.level2,accrep.level3,accrep.level4,accrep.level5,accrep.level6,accrep.level7," & _
                "accrep.acccode ,accrep.accname from accrep inner join acctgroup on accrep.acccode = acctgroup.acctcode and accrep.divcode=acctgroup.div_code " & _
                " where acctgroup.div_code='" & txtDivcode.Value & "' and accttype =2 and accrep.acccode like '%" & txtSerch.Text & "%'"
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
        End If


        For Each node0 In tvAccgrp.Nodes  '0
            node0.Expand()
            For Each node1 In node0.ChildNodes '1
                If node1.Value = nodevalue1 Then
                    node1.Expand()
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

    Protected Sub btnview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnview.Click

        Dim intNewParent As Long
        Dim intselLeval As Long

        If tvAccgrp.CheckedNodes.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Account code.');", True)
            Exit Sub
        End If

        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")

        Dim tnCollection As TreeNodeCollection = tvAccgrp.CheckedNodes
        Dim strValuPath As String = tnCollection(0).ValuePath
        Dim strSplitValuPath As String() = strValuPath.Split("/")
        intselLeval = strSplitValuPath.Length - 1

        If intselLeval = 0 Or intselLeval = 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user cannot view this group');", True)
            Exit Sub
        End If

        intNewParent = tnCollection(0).Value 'Current Selected

        If objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "acctgroup", "parentid", "accttype=1 and parentid=" & intNewParent, "div_code", txtDivcode.Value) <> 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user can not view this group.');", True)
            Exit Sub
        End If

        'Session.Add("ParentId", intNewParent)
        Dim strTemp As String() = tnCollection(0).Text.Split(strDelimiter)
        'Session("RefCode") = strTemp.GetValue(0)
        'Session("State") = "View"
        'Response.Redirect("AcctCode.aspx", False)

        Dim strpop As String = ""
        strpop = "window.open('AcctCode.aspx?State=View&ParentId=" & intNewParent & "&divid=" & txtDivcode.Value & "&RefCode=" & strTemp.GetValue(0) & "','AcctCode','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            'Session("ColReportParams") = Nothing

            'Session.Add("Pageame", "Accounts Master")
            'Session.Add("BackPageName", "AcctCodesSearch.aspx")

            If txtSerch.Text <> "" Then
                If ddlAccountSearchType.Value = "Account Code" Then
                    '    strReportTitle = "Account Code: " & txtSerch.Text.Trim
                    '    strSelectionFormula = " {acctmast.acctcode} LIKE '" & txtSerch.Text.Trim & "*'"
                    'Else
                    '    strReportTitle = "Account Name: " & txtSerch.Text.Trim
                    '    strSelectionFormula = " {acctmast.acctname} LIKE '" & txtSerch.Text.Trim & "*'"
                    strpop = ""
                    strpop = "window.open('rptReportNew.aspx?Pageame=Accounts Master&BackPageName=AcctCodesSearch.aspx&divid=" & txtDivcode.Value & "&AcctCode = " & txtSerch.Text.Trim & "','AcctcodeMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                Else
                    strpop = ""
                    strpop = "window.open('rptReportNew.aspx?Pageame=Accounts Master&BackPageName=AcctCodesSearch.aspx&divid=" & txtDivcode.Value & "&AcctName=" & txtSerch.Text.Trim & "','AcctcodeMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If
            Else
                strpop = ""
                strpop = "window.open('rptReportNew.aspx?Pageame=Accounts Master&divid=" & txtDivcode.Value & "&BackPageName=AcctCodesSearch.aspx','AcctcodeMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If


        Catch ex As Exception
            objUtils.WritErrorLog("AcctGroupsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=AcctCodeSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
       
    End Sub
End Class

'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('User can not add account group under code.');", True)
