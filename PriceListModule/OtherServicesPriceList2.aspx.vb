'
'   Form Name       :       Other Service Price List
'   Developer Name  :       Nilesh Sawant
'   Date            :       2 July 2008
'
'
'
'

#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class OtherServicesPriceList2
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim SqlCmd As SqlCommand
    Dim SqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim StrQry As String
    Dim SqlCon As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
    Dim ObjCon As New clsDBConnect
    Dim MyDS As New DataSet
    Dim Table As New DataTable()
    Dim ParameterArray As New ArrayList()
    Private dt As New DataTable
    Private cnt As Long
    Private arr(1) As String
    Private arrRName(1) As String
    Dim GvRow As String
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "NumbersInt"
    Public Sub NumbersInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "NumbersDateInt"
    Public Sub NumbersDateInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber2(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "HTMLNumbers"
    Public Sub HTMLNumbers(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Characters"
    Public Sub Characters(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            ViewState.Add("OtherServpricelist2State", Request.QueryString("State"))
            ViewState.Add("OtherServpricelist2RefCode", Request.QueryString("RefCode"))

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            If Request.QueryString("State") = "New" Then
                Page.Title = Page.Title + " " + "New Other Services Price List"
            ElseIf Request.QueryString("State") = "Copy" Then
                Page.Title = Page.Title + " " + "Copy Other Services Price List"
            ElseIf Request.QueryString("State") = "Edit" Then
                Page.Title = Page.Title + " " + "Edit Other Services Price List"
            ElseIf Request.QueryString("State") = "View" Then
                Page.Title = Page.Title + " " + "View Other Services Price List"
            ElseIf Request.QueryString("State") = "Delete" Then
                Page.Title = Page.Title + " " + "Delete Other Services Price List"
            End If

            Dim s As String = ""
            If IsPostBack = False Then
                Session("PlistSaved") = False
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellTypeCode, "othsellcode", "othsellname", "select othsellcode, othsellname from othsellmast where active=1 order by othsellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellTypeName, "othsellname", "othsellcode", "select othsellname,othsellcode from othsellmast where active=1 order by othsellname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                If ViewState("OtherServpricelist2State") = "New" Or ViewState("OtherServpricelist2State") = "Edit" Or ViewState("OtherServpricelist2State") = "Copy" Then

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save other services price list?')==false)return false;")
                    Dim obj As New EncryptionDecryption

                    SetFocus(ddlMarketCode)

                    lblHeading.Text = "Add Other Services Price List"

                    If Request.QueryString("MarketCode") <> Nothing And Request.QueryString("MarketName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("MarketName")
                        ddlMarketCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("MarketCode")
                        ddlMarketName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("SellTypeCode") <> Nothing And Request.QueryString("SellTypeName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SellTypeName")
                        ddlSellTypeCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SellTypeCode")

                        ddlSellTypeName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("GroupName")
                        ddlGroupCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("GroupCode")
                        ddlGroupName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("SubSeasCode") <> Nothing And Request.QueryString("SubSeasName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SubSeasName")
                        ddlSubSeasCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SubSeasCode")
                        ddlSubSeasName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If


                    txtRemark.Value = CType(Session("sessionRemark"), String)

                    If Request.QueryString("PlcCode") <> Nothing Then
                        s = ""
                        s = Request.QueryString("PlcCode")
                        txtPlcCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("currcode") <> Nothing Then
                        s = ""
                        s = Request.QueryString("currcode")

                        txtCurrCode.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("currname") <> Nothing Then
                        s = ""
                        s = Request.QueryString("currname")
                        txtCurrName.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If


                    If Request.QueryString("frmdate") <> Nothing Then
                        s = ""
                        s = Request.QueryString("frmdate")
                        dpFromDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("todate") <> Nothing Then
                        s = ""
                        s = Request.QueryString("todate")
                        dpToDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("Acvtive") <> Nothing Then
                        If Request.QueryString("Acvtive") = "1" Then
                            ChkActive.Checked = True
                        ElseIf Request.QueryString("Acvtive") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If

                    If ViewState("OtherServpricelist2State") <> "Copy" Then
                        If Request.QueryString("approved") <> Nothing Then
                            If Request.QueryString("approved") = "1" Then
                                chkapprove.Checked = True
                            ElseIf Request.QueryString("approved") = "0" Then
                                chkapprove.Checked = False
                            End If
                        End If

                        If Request.QueryString("stopweb") <> Nothing Then
                            If Request.QueryString("stopweb") = "1" Then
                                chkshowweb.Checked = True
                            ElseIf Request.QueryString("stopweb") = "0" Then
                                chkshowweb.Checked = False
                            End If
                        End If



                    End If

                ElseIf ViewState("OtherServpricelist2State") = "View" Then
                    lblHeading.Text = "View ther Services Price List"
                    ShowRecord(ViewState("OtherServpricelist2RefCode"))

                ElseIf ViewState("OtherServpricelist2State") = "Delete" Then
                    lblHeading.Text = "Delete Other Services Price List"
                    btnSave.Text = "Delete"

                    ShowRecord(ViewState("OtherServpricelist2RefCode"))

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete other services price list?')==false)return false;")
                End If
                BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear prices?')==false)return false;")
                createdatatable()


                'fill controls from previous form
                ' Now  Bind Column Dynamically 

                Dim fld2 As String = ""
                Dim col As DataColumn
                For Each col In dt.Columns
                    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
                        Dim bfield As New TemplateField
                        'Call Function
                        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
                        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
                        gv_SearchResult.Columns.Add(bfield)

                    End If

                Next
                gv_SearchResult.Visible = True
                gv_SearchResult.DataSource = dt
                'InstantiateIn Grid View
                gv_SearchResult.DataBind()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubSeasCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubSeasName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                TextLock(txtCurrCode)
                TextLock(txtCurrName)
                DisableAllControls()
            Else
                dt = Session("GV_HotelData")
                ' createdatatable()

                'fill controls from previous form
                ' Now  Bind Column Dynamically 

                Dim fld2 As String = ""
                Dim col As DataColumn
                For Each col In dt.Columns
                    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
                        Dim bfield As New TemplateField
                        'Call Function
                        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
                        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
                        gv_SearchResult.Columns.Add(bfield)

                    End If

                Next
                gv_SearchResult.DataSource = dt
                'InstantiateIn Grid View
                gv_SearchResult.DataBind()
                BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Clease formula?')==false)return false;")
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try

                checkIsPrivilege()
                If Session("Showapprove") <> "Yes" Then
                    chkapprove.Visible = False
                End If


                If Session("CheckGridColumn") = "Not Present" Then
                    Panel1.Visible = False
                    lblError.Visible = True
                    lblError.Text = "Rates are not present of this selected Group Code. Click Clear Prices for Changing Group."
                Else
                    Panel1.Visible = True
                    createdatarows()
                    lblError.Visible = False
                End If
                DisableGrid()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
    End Sub
#End Region

#Region "Public Function checkIsPrivilege() As Boolean"
    Public Function checkIsPrivilege() As Boolean
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='8' and appid='1' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Showapprove", "Yes")
            Else
                Session.Add("Showapprove", "No")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try

    End Function
#End Region

#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        ddlMarketCode.Disabled = True
        ddlMarketName.Disabled = True
        ddlGroupCode.Disabled = True
        ddlGroupName.Disabled = True
        ddlSellTypeCode.Disabled = True
        ddlSellTypeName.Disabled = True
        ddlSubSeasCode.Disabled = True
        ddlSubSeasName.Disabled = True
        txtCurrCode.Enabled = False
        txtCurrName.Enabled = False
        txtRemark.Disabled = True
        ChkActive.Disabled = True
        gv_SearchResult.Visible = True
        If ViewState("OtherServpricelist2State") = "New" Or ViewState("OtherServpricelist2State") = "Edit" Or ViewState("OtherServpricelist2State") = "Copy" Then
            BtnClearFormula.Visible = True
            btnSave.Visible = True
        ElseIf ViewState("OtherServpricelist2State") = "View" Or ViewState("OtherServpricelist2State") = "Delete" Then
            BtnClearFormula.Visible = False
            btnSave.Visible = False
            dpFromdate.Enabled = False
            dpToDate.Enabled = False
        End If
        If ViewState("OtherServpricelist2State") = "Delete" Then
            btnSave.Visible = True
        End If

    End Sub
#End Region
#Region "Private Sub DisableGrid()"
    Private Sub DisableGrid()
        Dim i As Long
        Dim k As Long = 0
        Dim j As Long = 1
        Dim txt As TextBox
        Dim gvrow As GridViewRow
        Dim n As Long = 0
        Dim m As Long = 0
        Try
            If ViewState("OtherServpricelist2State") = "View" Or ViewState("OtherServpricelist2State") = "Delete" Then

                ''''Herer Onlu Numbers Can enterd in textboxes

                j = 0
                cnt = gv_SearchResult.Columns.Count
                i = 0
                k = 0

                For Each gvrow In gv_SearchResult.Rows
                    If n = 0 Then
                        For i = 0 To cnt - 4
                            txt = gvrow.FindControl("txt" & i)
                            txt.Enabled = False
                            TextLock(txt)
                        Next
                        m = i
                    Else
                        k = 0
                        For i = n To (m + n) - 1
                            txt = gvrow.FindControl("txt" & i)
                            txt.Enabled = False
                            TextLock(txt)
                            k = k + 1
                        Next

                    End If
                    n = i
                Next

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "Private Sub createdatatable()"
    Private Sub createdatatable()
        Try
            cnt = 0
            strSqlQry = "select count(othcatcode) from  othcatmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt > 0 Then
                Session("CheckGridColumn") = ""
            Else
                Session("CheckGridColumn") = "Not Present"
            End If
            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select othcatcode from  othcatmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1 order by grporder"

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(i) = mySqlReader("othcatcode")
                i = i + 1
            End While


            'Here in Array store room types
            '-------------------------------------
            Dim tf As New TemplateField
            dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Code", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Name", GetType(String)))

            'create columns of this room types in data table
            For i = 0 To cnt - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next
            dt.Columns.Add(New DataColumn("From Date", GetType(String)))
            dt.Columns.Add(New DataColumn("To Date", GetType(String)))
            dt.Columns.Add(New DataColumn("Pkg", GetType(String)))
            Session("GV_HotelData") = dt


        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region
#Region "Private Sub createdatarows()"
    Private Sub createdatarows()
        Dim i As Long
        Dim k As Long = 0
        Try

            strSqlQry = "select count(othtypcode) from othtypmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)

            Dim arr_rnkorder(cnt + 1) As String
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select othtypcode,othtypname,rankorder from othtypmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1 order by  rankorder"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr_rnkorder(k) = mySqlReader("rankorder")
                arr_rows(k) = mySqlReader("othtypcode")
                arr_rname(k) = mySqlReader("othtypname")
                k = k + 1
            End While

            'Here add rows.....
            'Now Get Rows From sellmast Based on SPType
            Session("GV_HotelData") = dt
            Dim dr As DataRow

            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow
                'For i = 1 To cnt - 1
                ' dr("Sr No") = row + 1   '
                'taken from the rankorder instead of sno due to show the rank order in the pricelists      
                dr("Sr No") = arr_rnkorder(row)
                dr("Service Type Code") = arr_rows(row)
                dr("Service Type Name") = arr_rname(row)
                dr("From Date") = dpFromDate.Text
                dr("To Date") = dpToDate.Text
                dr("Pkg") = 1
                'Next
                dt.Rows.Add(dr)
            Next


            gv_SearchResult.Visible = True
            gv_SearchResult.DataSource = dt
            'InstantiateIn Grid View
            gv_SearchResult.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
        '-======================||||||||||||||||||||||||||||||||||||||||==========================!!!!!!!!!!!!!!!
        ''''Herer Onlu Numbers Can enterd in textboxes
        Dim j As Long = 1
        Dim txt As TextBox
        Dim gvrow As GridViewRow
        j = 0
        cnt = gv_SearchResult.Columns.Count
        Dim n As Long = 0
        Dim m As Long = 0
        i = 0
        k = 0
        Try

            For Each gvrow In gv_SearchResult.Rows
                If n = 0 Then
                    For i = 0 To cnt - 4
                        txt = gvrow.FindControl("txt" & i)
                        Numbers(txt)
                        txt.CssClass = "field_input"
                        txt.Width = 60
                    Next
                    m = i
                Else
                    k = 0
                    For i = n To (m + n) - 1
                        txt = gvrow.FindControl("txt" & i)
                        Numbers(txt)
                        txt.CssClass = "field_input"
                        txt.Width = 60
                        k = k + 1
                    Next

                End If
                n = i
            Next

            ''''Herer Only Interger Numbers Can enterd in textboxes from and todate


            Dim header As Long = 0
            cnt = gv_SearchResult.Columns.Count
            Dim heading(cnt + 1) As String
            '----------------------------------------------------------------------------
            '           Stoaring heading column values in the array

            For header = 0 To cnt - 4
                If Not gv_SearchResult.HeaderRow Is Nothing Then
                    txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                    heading(header) = txt.Text
                    If txt.Text = "From Date" Or txt.Text = "To Date" Then
                        txt.Width = 60
                    ElseIf txt.Text = "Pkg" Then
                        txt.Width = 30
                    Else
                        'If Len(txt.Text) < 20 Then
                        '    txt.Columns = 0
                        '    txt.Width = 100
                        'Else
                        txt.Columns = Len(txt.Text)
                        'txt.Width = Len(txt.Text) * 10
                        ' End If
                    End If
                    txt.CssClass = "field_input"
                End If
            Next

            Dim a, b As Long


            a = cnt - 7
            j = 0
            b = 0
            m = 0
            n = 0


            For Each gvrow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 4
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            txt = gvrow.FindControl("txt" & b + a + 1)
                            NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 2)
                            NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 3)
                            NumbersInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 30
                        End If
                    Next

                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            txt = gvrow.FindControl("txt" & b + a + 1)
                            NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 2)
                            NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 3)
                            NumbersInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 30
                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next


            If ViewState("OtherServpricelist2State") <> "New" Then
                ShowDynamicGrid(ViewState("OtherServpricelist2RefCode"))
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from oplisth Where oplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("oplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("oplistcode")
                    End If
                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        ddlMarketName.Value = mySqlReader("plgrpcode")
                        ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", mySqlReader("plgrpcode"))
                    End If
                    If IsDBNull(mySqlReader("othsellcode")) = False Then
                        ddlSellTypeName.Value = mySqlReader("othsellcode")
                        ddlSellTypeCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "othsellname", "othsellcode", mySqlReader("othsellcode"))
                    End If
                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        ddlGroupName.Value = mySqlReader("othgrpcode")
                        ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If

                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        ddlSubSeasName.Value = mySqlReader("subseascode")
                        ddlSubSeasCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    If IsDBNull(mySqlReader("frmdate")) = False Then
                        dpFromDate.Text = Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy")
                        Session("sessionFrmdate") = dpFromDate.Text
                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        dpToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                        Session("sessionTodate") = dpToDate.Text
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If mySqlReader("active") = "1" Then
                            ChkActive.Checked = True
                        ElseIf mySqlReader("active") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("approve")) = False Then
                        If mySqlReader("approve") = 1 Then
                            chkapprove.Checked = True
                        ElseIf mySqlReader("approve") = 0 Then
                            chkapprove.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("showagent")) = False Then
                        If mySqlReader("showagent") = 1 Then
                            chkshowweb.Checked = True
                        ElseIf mySqlReader("showagent") = 0 Then
                            chkshowweb.Checked = False
                        End If
                    End If

                End If
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region


#Region "Private Sub ShowDynamicGrid()"
    '    Private Sub ShowDynamicGrid(ByVal RefCode As String)
    '        Try
    '            cnt = 0
    '            Dim StrQry As String = ""
    '            '--------------------------------------------------------
    '            ' Show Records From Details Table 
    '            Dim GVROW As GridViewRow
    '            cnt = gv_SearchResult.Columns.Count
    '            Dim i As Long = 0
    '            Dim n As Long = 0
    '            Dim m As Long = 0
    '            Dim txt1 As TextBox
    '            Dim header As Long = 0
    '            Dim heading(cnt + 1) As String
    '            Try
    '                For header = 0 To cnt - 4
    '                    txt1 = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
    '                    heading(header) = txt1.Text
    '                Next
    '            Catch ex As Exception
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '            End Try


    '            StrQry = "select oplistd.* from  oplistd ,othcatmast,othtypmast  where " & _
    '            " oplistd.othcatcode=othcatmast.othcatcode and oplistd.othgrpcode=othcatmast.othgrpcode and " & _
    '            " oplistd.othtypcode=othtypmast.othtypcode and oplistd.othgrpcode = othtypmast.othgrpcode and " & _
    '            " oplistcode='" & RefCode & "' order by oplistd.othgrpcode,oplistd.othtypcode,othtypmast.rankorder,othcatmast.grporder"

    '            Dim txt As TextBox
    '            Dim headerlabel As New TextBox
    '            SqlCon = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '            SqlCmd = New SqlCommand(StrQry, SqlCon)
    '            mySqlReader = SqlCmd.ExecuteReader()
    '            Dim s As Long
    '            Dim k As Long = 0
    '            Dim a As Long = cnt - 7
    '            Dim b As Long = 0
    '            Dim othtypcode As String
    '            Dim othcatcode As String
    '            Dim value As String

    '            For Each GVROW In gv_SearchResult.Rows
    '                '  If mySqlReader.Read = True Then
    '                If n = 0 Then
    '                    For i = 0 To cnt - 4
    '                        If heading(i) = "From Date" Or heading(i) = "To Date" Or heading(i) = "Pkg" Then
    '                        Else
    '                            If mySqlReader.Read = True Then
    '                                othtypcode = mySqlReader("othtypcode")
    '                                othcatcode = mySqlReader("othcatcode")
    '                                ' If CType(GVROW.Cells(1).Text, String) = othtypcode Then
    '                                If IsDBNull(mySqlReader("oprice")) = False Then
    '                                    value = mySqlReader("oprice")
    '                                Else
    '                                    value = ""
    '                                End If
    '                                If GVROW.Cells(1).Text = othtypcode Then '--Other Typcode and Grid Typecode 

    '                                    For s = 0 To cnt - 4
    '                                        headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
    '                                        If headerlabel.Text = othcatcode Then
    '                                            txt = GVROW.FindControl("txt" & i)
    '                                            If value = "" Then
    '                                                txt.Text = ""
    '                                            Else
    '                                                txt.Text = value
    '                                            End If
    '                                            txt = GVROW.FindControl("txt" & b + a + 1)
    '                                            If IsDBNull(mySqlReader("frmdate")) = False Then
    '                                                txt.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
    '                                            Else
    '                                                txt.Text = ""
    '                                            End If

    '                                            txt = GVROW.FindControl("txt" & b + a + 2)
    '                                            If IsDBNull(mySqlReader("todate")) = False Then

    '                                                txt.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
    '                                            Else
    '                                                txt.Text = ""
    '                                            End If
    '                                            txt = GVROW.FindControl("txt" & b + a + 3)
    '                                            If IsDBNull(mySqlReader("pkgnights")) = False Then
    '                                                txt.Text = mySqlReader("pkgnights")
    '                                            Else
    '                                                txt.Text = ""
    '                                            End If
    '                                            GoTo go1
    '                                        End If

    '                                    Next
    '                                End If '--For oth Typecode Checking
    '                            End If
    '                        End If

    'go1:                Next
    '                    m = i
    '                Else
    '                    k = 0
    '                    For i = n To (m + n) - 1
    '                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
    '                        Else
    '                            k = k + 1
    '                            If mySqlReader.Read = True Then
    '                                othtypcode = mySqlReader("othtypcode")

    '                                If GVROW.Cells(1).Text = othtypcode Then '--Other Typcode and Grid Typecode 
    '                                    For s = 0 To cnt - 4
    '                                        othcatcode = mySqlReader("othcatcode")
    '                                        If IsDBNull(mySqlReader("oprice")) = False Then
    '                                            value = mySqlReader("oprice")
    '                                        Else
    '                                            value = ""
    '                                        End If
    '                                        headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)

    '                                        If headerlabel.Text = othcatcode Then

    '                                            txt = GVROW.FindControl("txt" & i)
    '                                            If value = "" Then
    '                                                txt.Text = ""
    '                                            Else
    '                                                txt.Text = value
    '                                            End If
    '                                            txt = GVROW.FindControl("txt" & b + a + 1)
    '                                            If IsDBNull(mySqlReader("frmdate")) = False Then
    '                                                txt.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
    '                                            Else
    '                                                txt.Text = ""
    '                                            End If

    '                                            txt = GVROW.FindControl("txt" & b + a + 2)
    '                                            If IsDBNull(mySqlReader("todate")) = False Then
    '                                                txt.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
    '                                            Else
    '                                                txt.Text = ""
    '                                            End If
    '                                            txt = GVROW.FindControl("txt" & b + a + 3)
    '                                            If IsDBNull(mySqlReader("pkgnights")) = False Then
    '                                                txt.Text = mySqlReader("pkgnights")
    '                                            Else
    '                                                txt.Text = ""
    '                                            End If
    '                                            GoTo goto2
    '                                        End If

    '                                    Next
    '                                End If '---Checking For Oth typecode
    '                            End If
    '                        End If
    'goto2:              Next

    '                End If
    '                n = i
    '                b = i
    '                ' End If
    '            Next
    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        Finally
    '            clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
    '            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
    '            clsDBConnect.dbConnectionClose(SqlCon)
    '        End Try
    '    End Sub


    Private Sub ShowDynamicGrid(ByVal RefCode As String)
        '        **************
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long
        Dim gvrow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        gv_SearchResult.Visible = True
        cnt = gv_SearchResult.Columns.Count
        Dim s As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        For header = 0 To cnt - 4
            txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
            If txt.Text <> Nothing Then
                heading(header) = txt.Text
            End If
        Next

        Dim m As Long = 0
        Dim rmcatcode As String = ""
        Dim rmtypecode As String = ""
        Dim value As String = ""
        Dim Linno As Integer
        Dim StrQry As String
        Dim headerlabel As New TextBox
        Dim myConn As New SqlConnection
        Dim myCmd As New SqlCommand
        Dim myReader As SqlDataReader
        Dim StrQryTemp As String

        Try

            StrQry = "select distinct othtypmast.rankorder olineno from  oplistd ,othcatmast,othtypmast  where " & _
                    " oplistd.othcatcode=othcatmast.othcatcode and oplistd.othgrpcode=othcatmast.othgrpcode and " & _
                    " oplistd.othtypcode=othtypmast.othtypcode and oplistd.othgrpcode = othtypmast.othgrpcode and " & _
                    " oplistcode='" & RefCode & "' order by othtypmast.rankorder"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("olineno")) = False Then
                            Linno = mySqlReader("olineno")
                        End If
                        If gvrow.Cells(0).Text = Linno Then

                            StrQryTemp = "select oplistd.oplistcode,othtypmast.rankorder olineno,oplistd.plgrpcode,oplistd.othsellcode," & _
                                        "oplistd.othgrpcode,oplistd.othtypcode,oplistd.othcatcode,oplistd.oprice,oplistd.frmdate,oplistd.todate, " & _
                                         "oplistd.pkgnights from  oplistd ,othcatmast,othtypmast  where " & _
                                         " oplistd.othcatcode=othcatmast.othcatcode and oplistd.othgrpcode=othcatmast.othgrpcode and " & _
                                         " oplistd.othtypcode=othtypmast.othtypcode and oplistd.othgrpcode = othtypmast.othgrpcode and " & _
                                         " oplistcode='" & RefCode & "' and othtypmast.rankorder='" & Linno & "' order by oplistd.othgrpcode,oplistd.othtypcode,othtypmast.rankorder,othcatmast.grporder"

                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("othcatcode")) = False Then
                                        rmcatcode = myReader("othcatcode")
                                    End If
                                    If IsDBNull(myReader("oprice")) = False Then
                                        value = myReader("oprice")
                                    Else
                                        value = ""
                                    End If
                                    For j = 0 To cnt - 5
                                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                        Else
                                            For s = 0 To gv_SearchResult.Columns.Count - 4
                                                headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = rmcatcode Then

                                                        If gvrow.RowIndex = 0 Then
                                                            txt = gvrow.FindControl("txt" & s)
                                                        Else
                                                            txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 4) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                        End If
                                                        If txt Is Nothing Then
                                                        Else
                                                            If value = "" Then
                                                                txt.Text = ""
                                                            Else
                                                                txt.Text = value
                                                            End If
                                                        End If

                                                        'txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 4) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 6 + gvrow.RowIndex)
                                                        'If txt Is Nothing Then
                                                        'Else
                                                        '    If IsDBNull(myReader("frmdate")) = False Then
                                                        '        txt.Text = CType(Format(CType(myReader("frmdate"), Date), "dd/MM/yyyy"), String)
                                                        '    Else
                                                        '        txt.Text = ""
                                                        '    End If
                                                        'End If
                                                        'txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 4) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 5 + gvrow.RowIndex)
                                                        'If txt Is Nothing Then
                                                        'Else
                                                        '    If IsDBNull(myReader("todate")) = False Then
                                                        '        txt.Text = CType(Format(CType(myReader("todate"), Date), "dd/MM/yyyy"), String)
                                                        '    Else
                                                        '        txt.Text = ""
                                                        '    End If
                                                        'End If
                                                        txt = gvrow.FindControl("txt" & ((gv_SearchResult.Columns.Count - 4) * gvrow.RowIndex) + gv_SearchResult.Columns.Count - 4 + gvrow.RowIndex)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If IsDBNull(myReader("pkgnights")) = False Then
                                                                txt.Text = myReader("pkgnights")
                                                            Else
                                                                txt.Text = ""
                                                            End If
                                                        End If
                                                        GoTo go1
                                                    End If
                                                End If
                                            Next
                                        End If
                                    Next
go1:                            End While
                            End If
                            clsDBConnect.dbConnectionClose(myConn)
                            clsDBConnect.dbCommandClose(myCmd)
                            clsDBConnect.dbReaderClose(myReader)
                        End If
                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)


        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("HederRsellingcode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub



#End Region

#Region "Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As New EncryptionDecryption
        Try
            Dim MarketCode As String = obj.Encrypt(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim MarketName As String = obj.Encrypt(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SellTypeCode As String = obj.Encrypt(CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SellTypeName As String = obj.Encrypt(CType(ddlSellTypeName.Items(ddlSellTypeName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")
            Dim GroupCode As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim GroupName As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim currcode As String = obj.Encrypt(CType(txtCurrCode.Text, String), "&%#@?,:*")
            Dim currname As String = obj.Encrypt(CType(txtCurrName.Text, String), "&%#@?,:*")
            Dim SubSeasCode As String = obj.Encrypt(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SubSeasName As String = obj.Encrypt(CType(ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, String), "&%#@?,:*")
            Session("sessionRemark") = txtRemark.Value
            Dim frmdate As String = obj.Encrypt(dpFromDate.Text, "&%#@?,:*")
            Dim todate As String = obj.Encrypt(dpToDate.Text, "&%#@?,:*")
            Session("sessionRemark") = txtRemark.Value

            Response.Redirect("OtherServicesPriceList1.aspx?&State=" & ViewState("OtherServpricelist2State") & "&RefCode=" & ViewState("OtherServpricelist2RefCode") & "&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&SellTypeCode=" & SellTypeCode & "&SellTypeName=" & SellTypeName & "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName & "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&frmdate=" & frmdate & "&todate=" & todate, False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("sessionRemark") = Nothing
        Session("GV_HotelData") = Nothing
        ViewState("OtherServpricelist2RefCode") = Nothing
        ViewState("OtherServpricelist2State") = Nothing
        'Response.Redirect("OtherServicesPriceListSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region " Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not Session("PlistSaved") = True Then ''this variable because response.redirect is causing a postback and saving twice
                Dim GvRow As GridViewRow
                If Page.IsValid = True Then
                    If ViewState("OtherServpricelist2State") = "New" Or ViewState("OtherServpricelist2State") = "Edit" Or ViewState("OtherServpricelist2State") = "Copy" Then
                        If ValidatePage() = False Then
                            Exit Sub
                        End If
                        If CheckDecimalInGrid() = False Then
                            Exit Sub
                        End If
                        If ValidateDateRange() = False Then
                            Exit Sub
                        End If
                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction
                        If ViewState("OtherServpricelist2State") = "New" Or ViewState("OtherServpricelist2State") = "Copy" Then
                            Dim optionval As String
                            optionval = objUtils.GetAutoDocNo("PLISTOTH", mySqlConn, sqlTrans)
                            txtPlcCode.Value = optionval.Trim
                            mySqlCmd = New SqlCommand("sp_add_oplisth", mySqlConn, sqlTrans)
                        ElseIf ViewState("OtherServpricelist2State") = "Edit" Then
                            mySqlCmd = New SqlCommand("sp_mod_oplisth", mySqlConn, sqlTrans)
                        End If

                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                        End If
                        If CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text.Trim, String)
                        End If
                        If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                        End If
                        If CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)
                        End If
                        If CType(txtCurrCode.Text.Trim, String) = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrCode.Text.Trim, String)
                        End If
                        If dpFromDate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = dpFromDate.Text
                        End If
                        If dpToDate.Text = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = dpToDate.Text
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        If chkapprove.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int, 9)).Value = 1
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int, 9)).Value = 0
                        End If

                        If chkshowweb.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@showagent", SqlDbType.Int, 9)).Value = 1
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@showagent", SqlDbType.Int, 9)).Value = 0
                        End If


                        mySqlCmd.ExecuteNonQuery()



                        '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$


                        mySqlCmd = New SqlCommand("sp_del_oplistd", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                        Dim j As Long = 1
                        Dim txt As TextBox
                        Dim cnt As Long
                        Dim srno As Long = 0
                        Dim hotelcategory As String = ""
                        j = 0

                        gv_SearchResult.Visible = True
                        cnt = gv_SearchResult.Columns.Count
                        Dim n As Long = 0
                        Dim k As Long = 0
                        Dim a As Long = cnt - 7
                        Dim b As Long = 0
                        Dim header As Long = 0
                        Dim heading(cnt + 1) As String
                        '----------------------------------------------------------------------------
                        '           Stoaring heading column values in the array
                        For header = 0 To cnt - 4
                            txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                            heading(header) = txt.Text
                        Next
                        '----------------------------------------------------------------------------
                        Dim m As Long = 0
                        For Each GvRow In gv_SearchResult.Rows
                            If n = 0 Then
                                For j = 0 To cnt - 4
                                    If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                    Else
                                        mySqlCmd = New SqlCommand("sp_add_oplistd", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure
                                        mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@olineno", SqlDbType.Int)).Value = CType(GvRow.Cells(0).Text, Long)

                                        If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                                        End If
                                        If CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)
                                        End If
                                        If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                        End If

                                        mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)

                                        txt = GvRow.FindControl("txt" & j)
                                        If Not txt Is Nothing Then
                                            If txt.Text = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
                                            End If
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                                        End If

                                        txt = GvRow.FindControl("txt" & b + a + 1)
                                        If Not txt Is Nothing Then
                                            If txt.Text = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)
                                            End If
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                                        End If

                                        txt = GvRow.FindControl("txt" & b + a + 2)
                                        If Not txt Is Nothing Then
                                            If txt.Text = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)
                                            End If
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                                        End If

                                        txt = GvRow.FindControl("txt" & b + a + 3)
                                        If Not txt Is Nothing Then
                                            If txt.Text = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
                                            End If
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = DBNull.Value
                                        End If
                                        mySqlCmd.ExecuteNonQuery()

                                    End If
                                Next

                                m = j
                            Else
                                k = 0
                                For j = n To (m + n) - 1
                                    If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                                    Else
                                        mySqlCmd = New SqlCommand("sp_add_oplistd", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                        mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@olineno", SqlDbType.Int)).Value = CType(GvRow.Cells(0).Text, Long)
                                        If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                                        End If
                                        If CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)
                                        End If
                                        If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                        End If

                                        mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
                                        txt = GvRow.FindControl("txt" & j)
                                        If txt.Text = "" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
                                        End If
                                        txt = GvRow.FindControl("txt" & b + a + 1)
                                        If Not txt Is Nothing Then
                                            If txt.Text = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)
                                            End If
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                                        End If

                                        txt = GvRow.FindControl("txt" & b + a + 2)
                                        If Not txt Is Nothing Then
                                            If txt.Text = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)
                                            End If
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                                        End If

                                        txt = GvRow.FindControl("txt" & b + a + 3)
                                        If Not txt Is Nothing Then
                                            If txt.Text = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
                                            End If
                                        Else
                                            mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = DBNull.Value
                                        End If

                                        mySqlCmd.ExecuteNonQuery()
                                        k = k + 1
                                    End If
                                Next
                            End If
                            b = j
                            n = j
                        Next

                    ElseIf ViewState("OtherServpricelist2State") = "Delete" Then
                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                        mySqlCmd = New SqlCommand("sp_del_oplisth", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd = New SqlCommand("sp_del_oplistd", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()
                    End If
                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                    Session("PlistSaved") = True
                    Session("sessionRemark") = Nothing
                    Session("GV_HotelData") = Nothing
                    ViewState("OtherServpricelist2RefCode") = Nothing
                    ViewState("OtherServpricelist2State") = Nothing

                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('OtherServicesPriceListWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


                    ' Response.Redirect("OtherServicesPriceListSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'If mySqlConn.State = ConnectionState.Open Then
            '    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            '    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            '    clsDBConnect.dbConnectionClose(mySqlConn)
            'End If
        End Try
    End Sub
#End Region

#Region "Private Function CheckDecimalInGrid() As Boolean"
    Private Function CheckDecimalInGrid() As Boolean
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        Dim n As Long = 0
        Dim k As Long = 0
        Dim a As Long = cnt - 7
        Dim b As Long = 0
        Dim header As Long = 0
        Dim arr() As String
        Dim GvRow As GridViewRow
        Try
            gv_SearchResult.Visible = True
            cnt = gv_SearchResult.Columns.Count
            Dim heading(cnt + 1) As String
            '----------------------------------------------------------------------------
            '           Stoaring heading column values in the array
            For header = 0 To cnt - 4
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
            '----------------------------------------------------------------------------
            Dim m As Long = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 4
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt.Text <> "" Then
                                arr = txt.Text.Split(".")
                                If arr.Length > 2 Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
                                    CheckDecimalInGrid = False
                                    Exit Function
                                End If
                            End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt.Text <> "" Then
                                arr = txt.Text.Split(".")
                                If arr.Length > 2 Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
                                    CheckDecimalInGrid = False
                                    Exit Function
                                End If
                            End If
                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next
            CheckDecimalInGrid = True
        Catch ex As Exception
            'Response.Write(ex)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Private Function ValidatePage() As Boolean"
    Private Function ValidatePage() As Boolean

        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        Dim n As Long = 0
        Dim k As Long = 0
        Dim a As Long = cnt - 7
        Dim b As Long = 0
        Dim header As Long = 0
        Dim GvRow As GridViewRow
        Dim FrmDate As Date = ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text)
        Dim GrdFrmDate As Date
        Dim Flag As Boolean = False
        Try

            If dpFromDate.Text <> "" And dpToDate.Text <> "" Then
                If ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should be grater than from date.');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpToDate.ClientID + "');", True)
                    Exit Function
                End If
            End If



            'gv_SearchResult.Visible = True
            cnt = gv_SearchResult.Columns.Count
            Dim heading(cnt + 1) As String
            '----------------------------------------------------------------------------
            '           Stoaring heading column values in the array
            For header = 0 To cnt - 4
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
            '----------------------------------------------------------------------------
            Dim m As Long = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 4
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If Not txt Is Nothing Then
                                If txt.Text <> "" Then
                                    Flag = True
                                    GoTo Flag
                                End If
                            End If

                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else

                            txt = GvRow.FindControl("txt" & j)
                            If Not txt Is Nothing Then
                                If txt.Text <> "" Then
                                    Flag = True
                                    GoTo Flag
                                End If
                            End If
                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next
Flag:       If Flag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Grid Rates can not be left blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
                ValidatePage = False
                Exit Function
            End If


            '-----------------------------------------------------------------------------
            a = cnt - 7
            j = 0
            b = 0
            m = 0
            n = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 4
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            'txt = GvRow.FindControl("txt" & j)
                            txt = GvRow.FindControl("txt" & b + a + 1)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates From date can not be left blank.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                    ValidatePage = False
                                    Exit Function
                                Else

                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From dates ( " & txt.Text & " )  Should be Greater Than or Equal to Header From Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From dates ( " & txt.Text & " ) Should be less Than or Equal to Header To Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
                                End If
                            End If


                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                    ValidatePage = False
                                    Exit Function
                                Else
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To dates ( " & txt.Text & " ) Should be greater Than or Equal to Header From Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To dates ( " & txt.Text & " )  Should be less Than or Equal to Header To Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(GrdFrmDate, Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To dates  ( " & txt.Text & " )  Should be greater Than or Equal to   From Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If

                                End If
                            End If


                            txt = GvRow.FindControl("txt" & b + a + 3)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Blank.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                    ValidatePage = False
                                    Exit Function
                                Else
                                    If CType(txt.Text, Integer) <= 0 Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Zero.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                End If
                            End If
                        End If

                    Next

                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else

                            'txt = GvRow.FindControl("txt" & j)

                            txt = GvRow.FindControl("txt" & b + a + 1)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates From date can not be left blank.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                    ValidatePage = False
                                    Exit Function
                                Else
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From dates ( " & txt.Text & " )   Should be Greater Than or Equal to Header From Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From dates ( " & txt.Text & " )   Should be less Than or Equal to Header To Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
                                End If
                            End If


                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                    ValidatePage = False
                                    Exit Function
                                Else
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To dates ( " & txt.Text & " )  Should be greater Than or Equal to Header From Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To dates ( " & txt.Text & " )   Should be less Than or Equal to Header To Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                    If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(GrdFrmDate, Date) Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To dates ( " & txt.Text & " )  Should be greater Than or Equal to   From Date.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If

                                End If
                            End If

                            txt = GvRow.FindControl("txt" & b + a + 3)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Blank.');", True)
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                    ValidatePage = False
                                    Exit Function
                                Else
                                    If CType(txt.Text, Integer) <= 0 Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Zero.');", True)
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
                                        ValidatePage = False
                                        Exit Function
                                    End If
                                End If
                            End If

                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next
            '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            ValidatePage = True
        Catch ex As Exception
            'Response.Write(ex)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region " Validate Date "
    Public Function ValidateDateRange() As Boolean
        Dim Alflg As Long
        Dim ErrMsg As String
        Dim SqlConn1 As SqlConnection
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        Dim n As Long = 0
        Dim k As Long = 0
        Dim a As Long = gv_SearchResult.Columns.Count - 7
        Dim b As Long = 0
        Dim header As Long = 0
        Dim GvRow As GridViewRow
        Try
            gv_SearchResult.Visible = True
            cnt = gv_SearchResult.Columns.Count
            Dim heading(cnt + 1) As String
            '----------------------------------------------------------------------------
            '           Stoaring heading column values in the array
            '----------------------------------------------------------------------------
            For header = 0 To cnt - 4
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
            '----------------------------------------------------------------------------
            Dim m As Long = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 4
                        'If heading(j) = "From Date" Then 'Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            SqlCon = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                            mySqlCmd = New SqlCommand("sp_chkopldate", SqlCon)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)
                            txt = GvRow.FindControl("txt" & b + a + 1)
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                            txt = GvRow.FindControl("txt" & b + a + 3)
                            mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = CType(txt.Text, Long)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)

                            Dim param1 As SqlParameter
                            Dim param2 As SqlParameter
                            param1 = New SqlParameter
                            param1.ParameterName = "@allowflg"
                            param1.Direction = ParameterDirection.Output
                            param1.DbType = DbType.Int16
                            param1.Size = 9
                            mySqlCmd.Parameters.Add(param1)
                            param2 = New SqlParameter
                            param2.ParameterName = "@errmsg"
                            param2.Direction = ParameterDirection.Output
                            param2.DbType = DbType.String
                            param2.Size = 50
                            mySqlCmd.Parameters.Add(param2)
                            mySqlAdapter = New SqlDataAdapter(mySqlCmd)
                            mySqlCmd.ExecuteNonQuery()

                            Alflg = param1.Value
                            ErrMsg = param2.Value
                            clsDBConnect.dbCommandClose(mySqlCmd)
                            clsDBConnect.dbConnectionClose(SqlCon)

                            If Alflg = 1 And ErrMsg <> "" Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                                SetFocus(dpFromdate)
                                ValidateDateRange = False
                                Exit Function
                            End If
                            'j = j + 2

                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        'If heading(k) = "From Date" Then 'Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            SqlCon = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

                            mySqlCmd = New SqlCommand("sp_chkopldate", SqlCon)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)
                            txt = GvRow.FindControl("txt" & b + a + 1)

                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)

                            txt = GvRow.FindControl("txt" & b + a + 2)

                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(Format(CType(txt.Text, Date), "yyyy/MM/dd"), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                            txt = GvRow.FindControl("txt" & b + a + 3)
                            mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = CType(txt.Text, Long)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)

                            Dim param1 As SqlParameter
                            Dim param2 As SqlParameter
                            param1 = New SqlParameter
                            param1.ParameterName = "@allowflg"
                            param1.Direction = ParameterDirection.Output
                            param1.DbType = DbType.Int16
                            param1.Size = 9
                            mySqlCmd.Parameters.Add(param1)
                            param2 = New SqlParameter
                            param2.ParameterName = "@errmsg"
                            param2.Direction = ParameterDirection.Output
                            param2.DbType = DbType.String
                            param2.Size = 50
                            mySqlCmd.Parameters.Add(param2)
                            mySqlAdapter = New SqlDataAdapter(mySqlCmd)
                            mySqlCmd.ExecuteNonQuery()

                            Alflg = param1.Value
                            ErrMsg = param2.Value

                            clsDBConnect.dbCommandClose(mySqlCmd)
                            clsDBConnect.dbConnectionClose(SqlCon)

                            If Alflg = 1 And ErrMsg <> "" Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                                SetFocus(dpFromdate)
                                ValidateDateRange = False
                                Exit Function
                            End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
            ValidateDateRange = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlCmd.Connection.State = ConnectionState.Open Then
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(SqlCon)
            End If
        End Try
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesPriceList2','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class


'txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate"))
'txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate"))
'txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate"))
'txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate"))
'txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate"))

'------------------------------------------------------------------------------------
'       Check The Dates Overlapping Using Stored Procedure
'------------------------------------------------------------------------------------
'If ValidateDateRange() = False Then
'    sqlTrans.Rollback()                                 'SQl Tarn Rollback
'    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'    clsDBConnect.dbConnectionClose(mySqlConn)
'    Exit Sub
'End If
'------------------------------------------------------------------------------------


'mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txt.Text)
'mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txt.Text)

'------------------------------------------------------------------------------------
'           Here Checking Validations
'If dpFromdate.txtDate.Text <> "" And dpTodate.txtDate.Text <> "" Then
'    If ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(dpTodate.txtDate.Text) Then
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should be grater than from date.');", True)
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpTodate.ClientID + "');", True)
'        Exit Sub
'    End If
'End If


'------------------------------------------------------------------------------------
'       Check The Dates Overlapping Using Stored Procedure
'------------------------------------------------------------------------------------
'If ValidateDateRange() = False Then
'    sqlTrans.Rollback()                                 'SQl Tarn Rollback
'    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'    clsDBConnect.dbConnectionClose(mySqlConn)
'    Exit Sub
'End If
'------------------------------------------------------------------------------------

'mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text), Date)
'mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTodate.txtDate.Text), Date)

'mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)


'mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)

'mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txt.Text)
'mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txt.Text)

'ddlCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'ddlCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)

'ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")

'ddlCurrencyCode.Disabled = True
'ddlCurrencyName.Disabled = True

'If Request.QueryString("remarks") <> Nothing Then
'End If

'BtnClearFormula.Visible = True
'gv_SearchResult.Visible = True

'ElseIf Session("State") = "Edit" Then
'    SetFocus(ddlMarketCode)
'    DisableAllControls()
'    ShowRecord(Session("RefCode"))
'    lblHeading.Text = "Edit Other Services Price List"

'    SetFocus(ddlMarketCode)
'    BtnClearFormula.Visible = False
'DisableAllControls()

'    btnSave.Text = "Delete"
'    BtnClearFormula.Visible = False

'gv_SearchResult.Visible = True

'If CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text.Trim, String) = "" Then
'    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'Else
'    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text.Trim, String)
'End If

'Dim currcode As String = obj.Encrypt(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String), "&%#@?,:*")
'Dim currname As String = obj.Encrypt(CType(ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text, String), "&%#@?,:*")

'Else
'    gv_SearchResult.Visible = False

'strSqlQry = "select othcatcode from  othcatmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"

'select rmcatcode from partyrmcat where partycode='3'

'   Response.Redirect("OtherServicesPriceList1.aspx?&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&SellTypeCode=" & SellTypeCode & "&SellTypeName=" & SellTypeName & "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName & "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0), False)

'#Region "Private Sub ShowDynamicGrid_old()"
'    Private Sub ShowDynamicGrid_old()
'        Try
'            cnt = 0
'            Dim StrQry As String = ""
'            '--------------------------------------------------------
'            ' Show Records From Details Table 
'            Dim GVROW As GridViewRow
'            cnt = gv_SearchResult.Columns.Count
'            Dim i As Long = 0
'            Dim n As Long = 0
'            Dim m As Long = 0
'            Dim txt1 As TextBox
'            Dim header As Long = 0
'            Dim heading(cnt + 1) As String
'            Try
'                For header = 0 To cnt - 4
'                    txt1 = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
'                    heading(header) = txt1.Text
'                Next
'            Catch ex As Exception

'            End Try

'            StrQry = "select * from oplistd  where oplistcode='" & txtPlcCode.Value & "' order by olineno,othtypcode"
'            Dim txt As TextBox
'            Dim headerlabel As New TextBox
'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
'            mySqlReader = mySqlCmd.ExecuteReader()
'            Dim s As Long
'            Dim k As Long = 0
'            Dim a As Long = cnt - 7
'            Dim b As Long = 0
'            Dim othtypcode As String
'            Dim value As String

'            For Each GVROW In gv_SearchResult.Rows
'                '  If mySqlReader.Read = True Then
'                If n = 0 Then
'                    For i = 0 To cnt - 4
'                        If heading(i) = "From Date" Or heading(i) = "To Date" Or heading(i) = "Pkg" Then
'                        Else
'                            If mySqlReader.Read = True Then
'                                othtypcode = mySqlReader("othcatcode")
'                                If IsDBNull(mySqlReader("oprice")) = False Then
'                                    value = mySqlReader("oprice")
'                                Else
'                                    value = ""
'                                End If

'                                For s = 0 To cnt - 4
'                                    headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
'                                    If headerlabel.Text = othtypcode Then
'                                        txt = GVROW.FindControl("txt" & i)
'                                        If value = "" Then
'                                            txt.Text = ""
'                                        Else
'                                            txt.Text = value
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 1)
'                                        If IsDBNull(mySqlReader("frmdate")) = False Then
'                                            txt.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
'                                        Else
'                                            txt.Text = ""
'                                        End If

'                                        txt = GVROW.FindControl("txt" & b + a + 2)
'                                        If IsDBNull(mySqlReader("todate")) = False Then
'                                            txt.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 3)
'                                        If IsDBNull(mySqlReader("pkgnights")) = False Then
'                                            txt.Text = mySqlReader("pkgnights")
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        GoTo go1
'                                    End If

'                                Next
'                            End If
'                        End If

'go1:                Next
'                    m = i
'                Else
'                    k = 0
'                    For i = n To (m + n) - 1
'                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
'                        Else
'                            If mySqlReader.Read = True Then
'                                othtypcode = mySqlReader("othcatcode")
'                                If IsDBNull(mySqlReader("oprice")) = False Then
'                                    value = mySqlReader("oprice")
'                                Else
'                                    value = ""
'                                End If

'                                For s = 0 To cnt - 4
'                                    headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
'                                    If headerlabel.Text = othtypcode Then
'                                        txt = GVROW.FindControl("txt" & i)
'                                        If value = "" Then
'                                            txt.Text = ""
'                                        Else
'                                            txt.Text = value
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 1)
'                                        If IsDBNull(mySqlReader("frmdate")) = False Then
'                                            txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate"))
'                                        Else
'                                            txt.Text = ""
'                                        End If

'                                        txt = GVROW.FindControl("txt" & b + a + 2)
'                                        If IsDBNull(mySqlReader("todate")) = False Then
'                                            txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate"))
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 3)
'                                        If IsDBNull(mySqlReader("pkgnights")) = False Then
'                                            txt.Text = mySqlReader("pkgnights")
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        GoTo goto2
'                                    End If

'                                Next
'                            End If

'                        End If
'                        k = k + 1
'goto2:              Next
'                End If
'                n = i
'                b = i
'                ' End If
'            Next

'            mySqlReader.Close()
'            mySqlConn.Close()
'            '  gv_SearchResult.Enabled = False
'            '-------------------------------------------
'        Catch ex As Exception
'            If mySqlConn.State = ConnectionState.Open Then
'                mySqlConn.Close()
'            End If
'        End Try
'    End Sub
'#End Region

'If Session("State") = "Copy" Then
'    ShowDynamicGrid(Session("RefCode"))
'End If

'#Region "Private Sub ShowRecord(ByVal RefCode As String)"
'    Private Sub ShowRecord(ByVal RefCode As String)
'        Try
'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'            mySqlCmd = New SqlCommand("Select * from oplisth Where oplistcode='" & RefCode & "'", mySqlConn)
'            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
'            If mySqlReader.HasRows Then
'                If mySqlReader.Read() = True Then
'                    If IsDBNull(mySqlReader("oplistcode")) = False Then
'                        txtPlcCode.Value = mySqlReader("oplistcode")
'                    End If
'                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
'                        ddlMarketName.Value = mySqlReader("plgrpcode")
'                        ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"plgrpmast", "plgrpname", "plgrpcode", mySqlReader("plgrpcode"))
'                    End If
'                    If IsDBNull(mySqlReader("othsellcode")) = False Then
'                        ddlSellTypeName.Value = mySqlReader("othsellcode")
'                        ddlSellTypeCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othsellmast", "othsellname", "othsellcode", mySqlReader("othsellcode"))
'                    End If
'                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
'                        ddlGroupName.Value = mySqlReader("othgrpcode")
'                        ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othsellmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
'                    End If
'                    If IsDBNull(mySqlReader("currcode")) = False Then
'                        ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = mySqlReader("currcode")
'                        ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", mySqlReader("currcode"))
'                    End If

'                    If IsDBNull(mySqlReader("subseascode")) = False Then
'                        ddlSubSeasName.Value = mySqlReader("subseascode")
'                        ddlSubSeasCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
'                    End If
'                    If IsDBNull(mySqlReader("remarks")) = False Then
'                        txtRemark.Value = mySqlReader("remarks")
'                    End If
'                    If IsDBNull(mySqlReader("frmdate")) = False Then
'                        dpFromdate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
'                        Session("sessionFrmdate") = dpFromdate.txtDate.Text
'                    End If
'                    If IsDBNull(mySqlReader("todate")) = False Then
'                        dpTodate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
'                        Session("sessionTodate") = dpTodate.txtDate.Text
'                    End If

'                    If IsDBNull(mySqlReader("active")) = False Then
'                        If mySqlReader("active") = "1" Then
'                            ChkActive.Checked = True
'                        ElseIf mySqlReader("active") = "0" Then
'                            ChkActive.Checked = False
'                        End If
'                    End If
'                End If
'            End If
'            mySqlCmd.Dispose()
'            mySqlReader.Close()

'        Catch ex As Exception
'            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        Finally
'            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
'        End Try
'    End Sub
'#End Region

'strSqlQry = "select othtypcode,othtypname from othtypmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"

'sqlTrans.Commit()    'SQl Tarn Commit
'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed

'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

'sqlTrans.Commit()    'SQl Tarn Commit
'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'clsDBConnect.dbCommandClose(myCommand)      

'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

'connection close

' Else

'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the From dates  Should be Greater Than or Equal to Header From Date.');", True)

' GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTodate.txtDate.Text), Date)

'If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < FrmDate Then
' If CType(GrdFrmDate, Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) Then






'#Region "Private Sub ShowDynamicGrid()"
'Private Sub ShowDynamicGrid(ByVal RefCode As String)
'    Try
'        cnt = 0
'        Dim StrQry As String = ""
'        '--------------------------------------------------------
'        ' Show Records From Details Table 
'        Dim GVROW As GridViewRow
'        cnt = gv_SearchResult.Columns.Count
'        Dim i As Long = 0
'        Dim n As Long = 0
'        Dim m As Long = 0
'        Dim txt1 As TextBox
'        Dim header As Long = 0
'        Dim heading(cnt + 1) As String
'        Try
'            For header = 0 To cnt - 4
'                txt1 = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
'                heading(header) = txt1.Text
'            Next
'        Catch ex As Exception
'            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
'        End Try

'        StrQry = " select oplistd.* from  oplistd ,othcatmast  where  oplistd.othcatcode=othcatmast.othcatcode " & _
'               " and oplistcode='" & RefCode & "' order by oplistd.othgrpcode,oplistd.othtypcode,othcatmast.grporder "
'        'StrQry = "select * from oplistd  where oplistcode='" & RefCode & "' order by olineno,othtypcode"

'        Dim txt As TextBox
'        Dim headerlabel As New TextBox
'        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'        mySqlCmd = New SqlCommand(StrQry, mySqlConn)
'        mySqlReader = mySqlCmd.ExecuteReader()
'        Dim s As Long
'        Dim k As Long = 0
'        Dim a As Long = cnt - 7
'        Dim b As Long = 0
'        Dim othtypcode As String
'        Dim value As String

'        For Each GVROW In gv_SearchResult.Rows
'            '  If mySqlReader.Read = True Then
'            If n = 0 Then
'                For i = 0 To cnt - 4
'                    If heading(i) = "From Date" Or heading(i) = "To Date" Or heading(i) = "Pkg" Then
'                    Else
'                        If mySqlReader.Read = True Then
'                            othtypcode = mySqlReader("othcatcode")
'                            If IsDBNull(mySqlReader("oprice")) = False Then
'                                value = mySqlReader("oprice")
'                            Else
'                                value = ""
'                            End If

'                            For s = 0 To cnt - 4
'                                headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
'                                If headerlabel.Text = othtypcode Then
'                                    txt = GVROW.FindControl("txt" & i)
'                                    If value = "" Then
'                                        txt.Text = ""
'                                    Else
'                                        txt.Text = value
'                                    End If
'                                    txt = GVROW.FindControl("txt" & b + a + 1)
'                                    If IsDBNull(mySqlReader("frmdate")) = False Then
'                                        txt.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
'                                    Else
'                                        txt.Text = ""
'                                    End If

'                                    txt = GVROW.FindControl("txt" & b + a + 2)
'                                    If IsDBNull(mySqlReader("todate")) = False Then

'                                        txt.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
'                                    Else
'                                        txt.Text = ""
'                                    End If
'                                    txt = GVROW.FindControl("txt" & b + a + 3)
'                                    If IsDBNull(mySqlReader("pkgnights")) = False Then
'                                        txt.Text = mySqlReader("pkgnights")
'                                    Else
'                                        txt.Text = ""
'                                    End If
'                                    GoTo go1
'                                End If

'                            Next
'                        End If
'                    End If

'go1:            Next
'                m = i
'            Else
'                k = 0
'                For i = n To (m + n) - 1
'                    If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
'                    Else
'                        k = k + 1
'                        If mySqlReader.Read = True Then
'                            othtypcode = mySqlReader("othcatcode")
'                            If IsDBNull(mySqlReader("oprice")) = False Then
'                                value = mySqlReader("oprice")
'                            Else
'                                value = ""
'                            End If

'                            For s = 0 To cnt - 4
'                                headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
'                                If headerlabel.Text = othtypcode Then
'                                    txt = GVROW.FindControl("txt" & i)
'                                    If value = "" Then
'                                        txt.Text = ""
'                                    Else
'                                        txt.Text = value
'                                    End If
'                                    txt = GVROW.FindControl("txt" & b + a + 1)
'                                    If IsDBNull(mySqlReader("frmdate")) = False Then
'                                        txt.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
'                                    Else
'                                        txt.Text = ""
'                                    End If

'                                    txt = GVROW.FindControl("txt" & b + a + 2)
'                                    If IsDBNull(mySqlReader("todate")) = False Then
'                                        txt.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
'                                    Else
'                                        txt.Text = ""
'                                    End If
'                                    txt = GVROW.FindControl("txt" & b + a + 3)
'                                    If IsDBNull(mySqlReader("pkgnights")) = False Then
'                                        txt.Text = mySqlReader("pkgnights")
'                                    Else
'                                        txt.Text = ""
'                                    End If
'                                    GoTo goto2
'                                End If

'                            Next
'                        End If

'                    End If

'goto2:          Next


'            End If
'            n = i
'            b = i
'            ' End If
'        Next

'        mySqlReader.Close()
'        mySqlConn.Close()
'        '  gv_SearchResult.Enabled = False
'        '-------------------------------------------
'    Catch ex As Exception
'        If mySqlConn.State = ConnectionState.Open Then
'            mySqlConn.Close()
'        End If
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
'    End Try
'End Sub
'#End Region


'Session("sessionRemark") = ""
'Session("GV_HotelData") = ""
'Session("RefCode") = ""
'Session("State") = ""

'Session("sessionRemark") = ""
'Session("GV_HotelData") = ""
'Session("RefCode") = ""
'Session("State") = ""
'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'cnt = mySqlCmd.ExecuteScalar
'mySqlConn.Close()


'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'cnt = mySqlCmd.ExecuteScalar
'mySqlConn.Close()

'If mySqlConn.State = ConnectionState.Open Then
'    mySqlConn.Close()
'End If
'mySqlReader.Close()
'mySqlConn.Close()


'mySqlReader.Close()
'mySqlConn.Close()
'If mySqlConn.State = ConnectionState.Open Then
'    mySqlConn.Close()
'End If


'  mySqlCmd.Dispose()
' mySqlReader.Close()

'Finally
'    If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()

'StrQry = "select * from oplistd  where oplistcode='" & RefCode & "' order by olineno,othtypcode"

'mySqlReader.Close()
'SqlCon.Close.Close()
'  gv_SearchResult.Enabled = False

'If mySqlConn.State = ConnectionState.Open Then
'    mySqlConn.Close()
'End If

'clsDBConnect.dbConnectionClose(SqlConn1)

'mySqlConn.Close()

'If Request.QueryString("MarketCode") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("MarketCode")
'                        ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If
'                    If Request.QueryString("MarketName") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("MarketName")
'                        ddlMarketName.Items(ddlMarketName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If
'                    If Request.QueryString("SellTypeCode") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("SellTypeCode")
'                        ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If
'                    If Request.QueryString("SellTypeName") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("SellTypeName")
'                        ddlSellTypeName.Items(ddlSellTypeName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If

'                    If Request.QueryString("GroupCode") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("GroupCode")
'                        ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If
'                    If Request.QueryString("GroupName") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("GroupName")
'                        ddlGroupName.Items(ddlGroupName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If
'                    If Request.QueryString("SubSeasCode") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("SubSeasCode")
'                        ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If

'                    If Request.QueryString("SubSeasName") <> Nothing Then
'                        s = ""
'                        s = Request.QueryString("SubSeasName")
'                        ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                    End If
