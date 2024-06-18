#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_TrfPriceList2
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
    Dim myDataAdapter As SqlDataAdapter
    Dim gvRow1 As GridViewRow
    Dim MyDS As New DataSet
    Dim Table As New DataTable()
    Dim ParameterArray As New ArrayList()
    Private dt As New DataTable
    Private cnt As Long
    Private arr(1) As String
    Private arrRName(1) As String
    Dim GvRow As String

    Dim dpFDate As New TextBox
    Dim dpTDate As New TextBox
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
        End Try
    End Sub
#End Region

#Region "Characters"
    Public Sub Characters(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If Request.QueryString("State") = "New" Then
                Page.Title = Page.Title + " " + "New Transfer Price List"
            ElseIf Request.QueryString("State") = "Copy" Then
                Page.Title = Page.Title + " " + "Copy Transfer Price List"
            ElseIf Request.QueryString("State") = "Edit" Then
                Page.Title = Page.Title + " " + "Edit Transfer Price List"
            ElseIf Request.QueryString("State") = "View" Then
                Page.Title = Page.Title + " " + "View Transfer Price List"
            ElseIf Request.QueryString("State") = "Delete" Then
                Page.Title = Page.Title + " " + "Delete Transfer Price List"
            End If


            Dim s As String = ""
            ViewState.Add("TrfpricelistState2", Request.QueryString("State"))
            ViewState.Add("TrfpricelistRefCode2", Request.QueryString("RefCode"))
            txtconnection.Value = Session("dbconnectionName")
            If IsPostBack = False Then
                Session("PlistSaved") = False 'Changed
                Dim strSPType As String = "select sptypecode,sptypename from sptypemast where active=1 and sptypecode in (select option_selected  from " & _
                                        " reservation_parameters where param_id in (564,1039))  order by sptypecode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "sptypecode", "sptypename", strSPType, True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", strSPType, True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                FillGridMarket("plgrpcode")
                'fillDategrd(grdDates, True)
                'fillsellingCode("sellmast.sellcode")

                ddlGroupCode.SelectedIndex = 0
                If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Edit" Or ViewState("TrfpricelistState2") = "Copy" Then

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Transfer price list?')==false)return false;")

                    btnSave.Attributes.Item("onclick") &= ";javascript:if(confirmMarkUp()==false) return false;"
                    'If ViewState("TrfpricelistState2") <> "Edit" Then
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirmMarkUp()==false) return false;")
                    'End If
                    Dim obj As New EncryptionDecryption
                    gv_SearchResult.Visible = True
                    BtnClearFormula.Visible = True

                    lblHeading.Text = "Add New Transfer Price List"

                    If ViewState("TrfpricelistState2") = "Edit" Then
                        btnSave.Text = "Update"
                    End If

                    If Request.QueryString("SptypeCode") <> Nothing And Request.QueryString("SptypeName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SptypeName")
                        ddlSPType.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SptypeCode")
                        ddlSPTypeName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    If Request.QueryString("SupplierCode") <> Nothing And Request.QueryString("SupplierName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SupplierName")
                        ddlSupplierCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SupplierCode")
                        ddlSupplierName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("SupplierAgentCode") <> Nothing And Request.QueryString("SupplierAgentName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SupplierAgentName")
                        ddlSupplierAgentCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SupplierAgentCode")
                        ddlSupplierAgentName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    Dim marketstr() As String
                    Dim lblcode As Label
                    Dim chksel As CheckBox
                    If Request.QueryString("Market") <> Nothing Then
                        marketstr = Request.QueryString("Market").ToString.Split(";")
                        For i = 0 To marketstr.GetUpperBound(0)
                            For Each Me.gvRow1 In gv_Market.Rows
                                lblcode = gvRow1.FindControl("lblcode")
                                chksel = gvRow1.FindControl("chkSelect")
                                If marketstr(i).Trim = lblcode.Text.Trim Then
                                    chksel.Checked = True
                                End If
                            Next
                        Next

                    End If
                    'If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                    '    s = ""
                    '    s = Request.QueryString("GroupName")
                    '    ddlGroupCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    '    s = ""
                    '    s = Request.QueryString("GroupCode")
                    '    ddlGroupName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    'End If

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

                    If Request.QueryString("Acvtive") <> Nothing Then
                        If Request.QueryString("Acvtive") = "1" Then
                            ChkActive.Checked = True
                        ElseIf Request.QueryString("Acvtive") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If

                    If ViewState("TrfpricelistState2") <> "Copy" Then
                        If Request.QueryString("approved") <> Nothing Then
                            If Request.QueryString("approved") = "1" Then
                                chkapprove.Checked = True
                            ElseIf Request.QueryString("approved") = "0" Then
                                chkapprove.Checked = False
                            End If
                        End If
                    End If

                    If Request.QueryString("conMarkUp") <> Nothing Then
                        If Request.QueryString("conMarkUp") = "1" Then
                            chkConsdierForMarkUp.Checked = True
                        ElseIf Request.QueryString("conMarkUp") = "0" Then
                            chkConsdierForMarkUp.Checked = False
                        End If
                    End If

                    ddlServerType.Items.Clear()
                    ddlServerType.Items.Add("[Select]")
                    ddlServerType.Items.Add("Arrival Borders")
                    ddlServerType.Items.Add("Departure Borders")
                    ddlServerType.Items.Add("Internal Transfer/Excursion")
                    ddlServerType.Items.Add("Arrival/Departure Transfer Borders")

                    If Request.QueryString("transfertype") <> Nothing Then
                        ddlServerType.SelectedIndex = CType(Request.QueryString("transfertype"), Integer)
                    End If

                ElseIf ViewState("TrfpricelistState2") = "View" Then
                    ddlServerType.Items.Clear()
                    ddlServerType.Items.Add("[Select]")
                    ddlServerType.Items.Add("Arrival Borders")
                    ddlServerType.Items.Add("Departure Borders")
                    ddlServerType.Items.Add("Internal Transfer/Excursion")
                    ddlServerType.Items.Add("Arrival/Departure Transfer Borders")

                    If Request.QueryString("transfertype") <> Nothing Then
                        ddlServerType.SelectedIndex = CType(Request.QueryString("transfertype"), Integer)
                    End If

                    lblHeading.Text = "View Transfer Price List"
                    ShowRecord(ViewState("TrfpricelistRefCode2"))
                    ShowMarkets(CType(ViewState("TrfpricelistRefCode2"), String))
                    'ShowDates(CType(ViewState("TrfpricelistRefCode2"), String))
                    'ShowSellingRecord(CType(ViewState("TrfpricelistRefCode2"), String))


                ElseIf ViewState("TrfpricelistState2") = "Delete" Then
                    ddlServerType.Items.Clear()
                    ddlServerType.Items.Add("[Select]")
                    ddlServerType.Items.Add("Arrival Borders")
                    ddlServerType.Items.Add("Departure Borders")
                    ddlServerType.Items.Add("Internal Transfer/Excursion")
                    ddlServerType.Items.Add("Arrival/Departure Transfer Borders")

                    If Request.QueryString("transfertype") <> Nothing Then
                        ddlServerType.SelectedIndex = CType(Request.QueryString("transfertype"), Integer)
                    End If

                    lblHeading.Text = "Delete Transfer Price List"
                    btnSave.Text = "Delete"
                    ShowRecord(ViewState("TrfpricelistRefCode2"))
                    ShowMarkets(CType(ViewState("TrfpricelistRefCode2"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Transfer price list?')==false)return false;")
                End If

                TextLock(txtCurrCode)
                TextLock(txtCurrName)
                DisableAllControls()

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
            Else
                'dt = Session("GV_HotelData")
                dt = Session("GV_TrfPLData")

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
                gv_SearchResult.Visible = True
                gv_SearchResult.DataSource = dt
                'InstantiateIn Grid View
                gv_SearchResult.DataBind()
                BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Clear formula?')==false)return false;")
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")


            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Public Function checkIsPrivilege() As Boolean"
    Public Sub checkIsPrivilege()
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
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try

    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try

                checkIsPrivilege()
                If Session("Showapprove") <> "Yes" Then
                    chkapprove.Visible = False
                End If

                ViewState.Add("TrfpricelistState2", Request.QueryString("State"))
                ViewState.Add("TrfpricelistRefCode2", Request.QueryString("RefCode"))

                fillDategrd(grdDates, True)
                If Request.QueryString("flag") = "selling" Then
                    ViewState.Add("flag", Request.QueryString("flag"))
                Else
                    ViewState.Add("flag", "")
                End If



                fillsellingCode("sellmast.sellcode")

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

                If ViewState("TrfpricelistState2") = "View" Then
                    lblHeading.Text = "View Transfer Price List"
                    'ShowRecord(ViewState("TrfpricelistRefCode2"))
                    'ShowMarkets(CType(ViewState("TrfpricelistRefCode2"), String))
                    ShowDates(CType(ViewState("TrfpricelistRefCode2"), String))
                    ShowSellingRecord(CType(ViewState("TrfpricelistRefCode2"), String))
                End If
                If ViewState("TrfpricelistState2") = "Edit" Then
                    lblHeading.Text = "Edit Transfer Price List"
                    'ShowRecord(ViewState("TrfpricelistRefCode2"))
                    'ShowMarkets(CType(ViewState("TrfpricelistRefCode2"), String))
                    ShowDates(CType(ViewState("TrfpricelistRefCode2"), String))
                    ShowSellingRecord(CType(ViewState("TrfpricelistRefCode2"), String))
                End If
                If ViewState("TrfpricelistState2") = "Delete" Then
                    lblHeading.Text = "Delete Transfer Price List"
                    'ShowRecord(ViewState("TrfpricelistRefCode2"))
                    'ShowMarkets(CType(ViewState("TrfpricelistRefCode2"), String))
                    ShowDates(CType(ViewState("TrfpricelistRefCode2"), String))
                    ShowSellingRecord(CType(ViewState("TrfpricelistRefCode2"), String))
                End If
                If ViewState("TrfpricelistState2") = "Copy" Then
                    ShowDates(CType(ViewState("TrfpricelistRefCode2"), String))
                End If



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
                objUtils.WritErrorLog("TrfPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trfplisth Where tplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("tplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("tplistcode")
                    End If

                    If IsDBNull(mySqlReader("partycode")) = False Then
                        ddlSupplierName.Value = mySqlReader("partycode")
                        ddlSupplierCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("partycode"))

                        ddlSPTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", mySqlReader("partycode"))
                        ddlSPType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", ddlSPTypeName.Value)

                    End If
                    'If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    '    ddlGroupName.Value = mySqlReader("othgrpcode")
                    '    ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
                    'End If

                    'If IsDBNull(mySqlReader("plgrpcode")) = False Then
                    '    ddlMarketName.Value = mySqlReader("plgrpcode")
                    '    ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", mySqlReader("plgrpcode"))
                    'End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If
                    If IsDBNull(mySqlReader("supagentcode")) = False Then
                        ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text = mySqlReader("supagentcode")
                        ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentname", "supagentcode", mySqlReader("supagentcode"))
                    End If


                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        ddlSubSeasName.Value = mySqlReader("subseascode")
                        ddlSubSeasCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    'If IsDBNull(mySqlReader("frmdate")) = False Then
                    '    dpFromDate.Text = Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy")
                    '    Session("sessionFrmdate") = dpFromDate.Text
                    'End If
                    'If IsDBNull(mySqlReader("todate")) = False Then
                    '    dpToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                    '    Session("sessionTodate") = dpToDate.Text
                    'End If

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

                    If IsDBNull(mySqlReader("considerformarkup")) = False Then
                        If mySqlReader("considerformarkup") = 1 Then
                            chkConsdierForMarkUp.Checked = True
                        ElseIf mySqlReader("considerformarkup") = 0 Then
                            chkConsdierForMarkUp.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("Transfertype")) = False Then
                        ddlServerType.SelectedIndex = mySqlReader("Transfertype")
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region


#Region " Private Sub ShowDates(ByVal RefCode As String)"
    Private Sub ShowMarkets(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim chksel As CheckBox
            Dim lblcode As Label
            'Dim mktcode As String = ""
            'Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
            'Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trfplisth_market Where tplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        For Each Me.gvRow1 In gv_Market.Rows
                            chksel = gvRow1.FindControl("chkSelect")
                            lblcode = gvRow1.FindControl("lblcode")
                            If lblcode.Text = mySqlReader("plgrpcode") Then
                                chksel.Checked = True
                                Exit For
                            End If

                        Next
                    End If
                End While
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region " Private Sub ShowDates(ByVal RefCode As String)"
    Private Sub ShowDates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            'Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
            'Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trfplisth_dates Where tplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromDate")
                        dpTDate = gvRow.FindControl("txtToDate")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                dpFDate.Text = Format("U", mySqlReader("frmdate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = Format("U", mySqlReader("todate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

    Private Sub ShowSellingRecord(ByVal RefCode As String)
        Dim ddl As DropDownList
        Dim GvRow As GridViewRow
        Dim txt1 As TextBox

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand("select * from trfplisth_convrates where tplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            For Each GvRow In grdSelling.Rows

                If mySqlReader.Read = True Then
                    'ddl = GvRow.FindControl("ddlFormulaFrom")
                    If GvRow.Cells(0).Text = mySqlReader("sellcode") Then
                        '    If IsDBNull(mySqlReader("frmlfrom")) = False Then
                        '        If mySqlReader("frmlfrom") = 0 Then
                        '            ddl.SelectedValue = "Category"
                        '        ElseIf mySqlReader("frmlfrom") = 1 Then
                        '            ddl.SelectedValue = "Supplier"
                        '        End If
                        '    End If
                        '    If ddl.SelectedValue = "Category" Then
                        '        ddl = GvRow.FindControl("ddlFormulaCD")
                        '        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddl, "frmlcode", "select frmlcode from sellspcath where sellcode='" & GvRow.Cells(0).Text & "'  union all select '[Select]' order by frmlcode", False)
                        '        If IsDBNull(mySqlReader("frmlcode")) = False Then
                        '            ddl.SelectedValue = mySqlReader("frmlcode")
                        '        End If
                        '    ElseIf ddl.SelectedValue = "Supplier" Then
                        '        ddl = GvRow.FindControl("ddlFormulaCD")
                        '        'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddl, "frmlcode", "select frmlcode from sellsph where sellcode='" & GvRow.Cells(0).Text & "'  union all select '[Select]' order by frmlcode", False)
                        '        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddl, "frmlcode", "select frmlcode from sellsph where sellcode='" & GvRow.Cells(0).Text & "'" & _
                        '        " and supagentcode='" & CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String) & "' and partycode='" & CType(ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text, String) & "'  union all select '[Select]' order by frmlcode", False)
                        '        If IsDBNull(mySqlReader("frmlcode")) = False Then
                        '            ddl.SelectedValue = mySqlReader("frmlcode")
                        '        End If
                        '    End If
                        txt1 = GvRow.FindControl("txtconv")
                        txt1.Text = mySqlReader("convrate")
                    End If
                End If
            Next
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            objUtils.WritErrorLog("HeaderInfo1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        btnselling.Visible = False
        chkrecalsprice.Visible = False
        If ViewState("TrfpricelistState2") = "New" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True
            'ddlMarketCode.Disabled = True
            'ddlMarketName.Disabled = True
            ddlGroupCode.Disabled = True
            'ddlGroupName.Disabled = True
            ddlSupplierCode.Disabled = True
            ddlSupplierName.Disabled = True
            ddlSubSeasCode.Disabled = True
            ddlSubSeasName.Disabled = True
            txtRemark.Disabled = True
            ChkActive.Disabled = True
            ddlSupplierAgentCode.Disabled = True
            ddlSupplierAgentName.Disabled = True
            txtCurrCode.Enabled = False
            txtCurrCode.Enabled = False
            gv_Market.Enabled = False

        ElseIf ViewState("TrfpricelistState2") = "Edit" Or ViewState("TrfpricelistState2") = "Copy" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True
            'ddlMarketCode.Disabled = True
            'ddlMarketName.Disabled = True
            ddlGroupCode.Disabled = True
            'ddlGroupName.Disabled = True
            ddlSupplierCode.Disabled = True
            ddlSupplierName.Disabled = True
            ddlSubSeasCode.Disabled = True
            ddlSubSeasName.Disabled = True
            txtRemark.Disabled = True
            ChkActive.Disabled = True
            ddlSupplierAgentCode.Disabled = True
            ddlSupplierAgentName.Disabled = True
            txtCurrCode.Enabled = False
            txtCurrCode.Enabled = False
            gv_Market.Enabled = False


            If ViewState("TrfpricelistState2") = "Edit" Then
                If chkConsdierForMarkUp.Checked = True Then
                    chkConsdierForMarkUp.Enabled = False
                Else
                    chkConsdierForMarkUp.Enabled = True
                End If

                chkrecalsprice.Visible = True

            End If
        ElseIf ViewState("TrfpricelistState2") = "Delete" Or ViewState("TrfpricelistState2") = "View" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True
            'ddlMarketCode.Disabled = True
            'ddlMarketName.Disabled = True
            ddlGroupCode.Disabled = True
            'ddlGroupName.Disabled = True
            ddlSupplierCode.Disabled = True
            ddlSupplierName.Disabled = True
            ddlSubSeasCode.Disabled = True
            ddlSubSeasName.Disabled = True
            txtRemark.Disabled = True
            ChkActive.Disabled = True
            ddlSupplierAgentCode.Disabled = True
            ddlSupplierAgentName.Disabled = True
            txtCurrCode.Enabled = False
            txtCurrCode.Enabled = False
            gv_SearchResult.Enabled = False
            grdDates.Enabled = False
            grdSelling.Enabled = False
            btnAddLines.Visible = False
            BtnClearFormula.Visible = False
            btnSave.Visible = False
            gv_Market.Enabled = False
            chkapprove.Enabled = False
            chkConsdierForMarkUp.Enabled = False
            btnselling.Visible = True
        End If
        If ViewState("TrfpricelistState2") = "Delete" Then
            btnSave.Visible = True
        End If


    End Sub
#End Region

#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        gv_Market.Visible = True
        If gv_Market.PageIndex < 0 Then
            gv_Market.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_Market.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Market.DataBind()

            Else
                gv_Market.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
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
            If ViewState("TrfpricelistState2") = "View" Or ViewState("TrfpricelistState2") = "Delete" Then
                j = 0
                cnt = gv_SearchResult.Columns.Count
                i = 0
                k = 0
                For Each gvrow In gv_SearchResult.Rows
                    If n = 0 Then
                        For i = 0 To cnt - 5
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
            'strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "')and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1 "

            strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "')and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1 and othcatmast.shuttle<>1 "


            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt <= 0 Then
                Session("CheckGridColumn") = "Not Present"
            Else
                Session("CheckGridColumn") = ""
            End If

            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0
            Dim Column As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            'strSqlQry = "SELECT  distinct dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode,othcatmast.grporder FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1  Order by othcatmast.grporder"
            strSqlQry = "SELECT  distinct dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode,othcatmast.grporder FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1  and othcatmast.shuttle <> 1 Order by othcatmast.grporder"

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(Column) = mySqlReader("othcatcode")
                Column = Column + 1
            End While


            'Here in Array store room types
            '-------------------------------------
            Dim tf As New TemplateField
            dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Code", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type Name", GetType(String)))

            'create columns of this room types in data table
            For i = 0 To Column - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next
            'dt.Columns.Add(New DataColumn("From Date", GetType(String)))
            'dt.Columns.Add(New DataColumn("To Date", GetType(String)))
            dt.Columns.Add(New DataColumn("Pkg", GetType(String)))
            'Session("GV_HotelData") = dt
            Session("GV_TrfPLData") = dt


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            If ddlServerType.SelectedValue = "[Select]" Then
                strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "')  and othtypmast.active=1"
            Else
                strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othtypmast.transfertype='" + CType(ddlServerType.SelectedIndex, String) + "' and othtypmast.active=1"
            End If


            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)

            Dim arr_rnkorder(cnt + 1) As String
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            If ddlServerType.SelectedValue = "[Select]" Then
                strSqlQry = "SELECT distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode,rankorder FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "')  and othtypmast.active=1 order by  rankorder"
            Else
                strSqlQry = "SELECT distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode,rankorder FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othtypmast.transfertype='" + CType(ddlServerType.SelectedIndex,string) + "' and othtypmast.active=1 order by  rankorder"
            End If

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
            'Session("GV_HotelData") = dt
            Session("GV_TrfPLData") = dt
            Dim dr As DataRow

            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow
                'For i = 1 To cnt - 1

                ' dr("Sr No") = row + 1   ' 
                ''taken from the rankorder instead of sno due to show the rank order in the pricelists      
                dr("Sr No") = row + 1 'arr_rnkorder(row)
                dr("Service Type Code") = arr_rows(row)
                dr("Service Type Name") = arr_rname(row)
                'dr("From Date") = dpFromDate.Text
                'dr("To Date") = dpToDate.Text
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
        ''''Herer Only Numbers Can enterd in textboxes
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
                    For i = 0 To cnt - 5
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

            For header = 0 To cnt - 5
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
            Next

            Dim a, b As Long


            a = cnt - 7
            j = 0
            b = 0
            m = 0
            n = 0


            For Each gvrow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            'txt = gvrow.FindControl("txt" & b + a + 1)
                            'NumbersDateInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 2)
                            'NumbersDateInt(txt)
                            NumbersInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            'txt = gvrow.FindControl("txt" & b + a + 3)
                            'NumbersInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 30
                        End If
                    Next

                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            'txt = gvrow.FindControl("txt" & b + a + 1)
                            'NumbersDateInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 60
                            txt = gvrow.FindControl("txt" & b + a + 2)
                            NumbersInt(txt) 'NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60
                            'txt = gvrow.FindControl("txt" & b + a + 3)
                            'NumbersInt(txt)
                            'txt.CssClass = "field_input"
                            'txt.Width = 30
                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next

            If ViewState("TrfpricelistState2") <> "New" Then
                ShowDynamicGrid(ViewState("TrfpricelistRefCode2"))
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


    '            StrQry = "select oplist_costd.* from  oplist_costd ,othcatmast,othtypmast  where " & _
    '                     " oplist_costd.othcatcode=othcatmast.othcatcode and oplist_costd.othgrpcode=othcatmast.othgrpcode and " & _
    '                     " oplist_costd.othtypcode=othtypmast.othtypcode and oplist_costd.othgrpcode = othtypmast.othgrpcode and " & _
    '                     " ocplistcode='" & RefCode & "' order by oplist_costd.othgrpcode,oplist_costd.othtypcode,othtypmast.rankorder,othcatmast.grporder"


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
    '                                If IsDBNull(mySqlReader("ocostprice")) = False Then
    '                                    value = mySqlReader("ocostprice")
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
    '                                            'txt = GVROW.FindControl("txt" & b + a + 1)
    '                                            'If IsDBNull(mySqlReader("frmdate")) = False Then
    '                                            '    txt.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
    '                                            'Else
    '                                            '    txt.Text = ""
    '                                            'End If

    '                                            'txt = GVROW.FindControl("txt" & b + a + 2)
    '                                            'If IsDBNull(mySqlReader("todate")) = False Then
    '                                            '    txt.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
    '                                            'Else
    '                                            '    txt.Text = ""
    '                                            'End If
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
    '                                othcatcode = mySqlReader("othcatcode")
    '                                If IsDBNull(mySqlReader("ocostprice")) = False Then
    '                                    value = mySqlReader("ocostprice")
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
    '                                            'txt = GVROW.FindControl("txt" & b + a + 1)
    '                                            'If IsDBNull(mySqlReader("frmdate")) = False Then
    '                                            '    txt.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
    '                                            'Else
    '                                            '    txt.Text = ""
    '                                            'End If

    '                                            'txt = GVROW.FindControl("txt" & b + a + 2)
    '                                            'If IsDBNull(mySqlReader("todate")) = False Then
    '                                            '    txt.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
    '                                            'Else
    '                                            '    txt.Text = ""
    '                                            'End If
    '                                            txt = GVROW.FindControl("txt" & b + a + 3)
    '                                            If IsDBNull(mySqlReader("pkgnights")) = False Then
    '                                                txt.Text = mySqlReader("pkgnights")
    '                                            Else
    '                                                txt.Text = ""
    '                                            End If
    '                                            GoTo goto2
    '                                        End If
    '                                    Next
    '                                End If '--For oth Typecode Checking
    '                            End If
    '                        End If

    'goto2:              Next
    '                End If
    '                n = i
    '                b = i
    '                ' End If
    '            Next


    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
    '        Finally
    '            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
    '            clsDBConnect.dbConnectionClose(mySqlConn)

    '        End Try

    '    End Sub
    Private Sub ShowDynamicGrid(ByVal RefCode As String)
        Try
            cnt = 0
            Dim StrQry As String = ""

            '--------------------------------------------------------
            ' Show Records From Details Table 
            Dim GVROW As GridViewRow
            cnt = gv_SearchResult.Columns.Count
            Dim i As Long = 0
            'Dim n As Long = 0
            'Dim m As Long = 0
            Dim txt1 As TextBox
            Dim header As Long = 0
            Dim heading(cnt + 1) As String

            Dim othcatcode As String
            Dim value As String

            Dim Linno As Integer
            Dim StrQryTemp As String = ""
            Dim myConn As New SqlConnection
            Dim myCmd As New SqlCommand
            Dim myReader As SqlDataReader
            Dim j As Long = 0
            Dim s As Long

            Try
                For header = 0 To cnt - 5
                    txt1 = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                    heading(header) = txt1.Text
                Next
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try


            'StrQry = "select oplist_costd.ocplistcode,oplist_costd.plgrpcode,oplist_costd.othgrpcode,othtypmast.rankorder oclineno,oplist_costd.othtypcode, " & _
            '         "oplist_costd.othcatcode,oplist_costd.ocostprice,oplist_costd.frmdate,oplist_costd.todate,oplist_costd.pkgnights  " & _
            '         "from  oplist_costd ,othcatmast,othtypmast  where " & _
            '         " oplist_costd.othcatcode=othcatmast.othcatcode and oplist_costd.othgrpcode=othcatmast.othgrpcode and " & _
            '         " oplist_costd.othtypcode=othtypmast.othtypcode and oplist_costd.othgrpcode = othtypmast.othgrpcode and " & _
            '         " ocplistcode='" & RefCode & "' order by oplist_costd.othgrpcode,oplist_costd.othtypcode,othtypmast.rankorder,othcatmast.grporder"
            'chnged trfplist_costd.oclineno from othtypmast.rankorder
            StrQry = "select trfplist_costd.tplistcode ,trfplist_costd.plgrpcode,trfplist_costd.othgrpcode,trfplist_costd.oclineno, " & _
                " trfplist_costd.othtypcode, trfplist_costd.othcatcode, trfplist_costd.tcostprice, trfplist_costd.pkgnights " & _
                " from  trfplist_costd ,othcatmast,othtypmast  where  trfplist_costd.othcatcode=othcatmast.othcatcode " & _
                "  and trfplist_costd.othgrpcode=othcatmast.othgrpcode and  trfplist_costd.othtypcode=othtypmast.othtypcode and  " & _
                "  trfplist_costd.othgrpcode = othtypmast.othgrpcode and  tplistcode='" & RefCode & "' order by trfplist_costd.othgrpcode, " & _
                " trfplist_costd.othtypcode, othtypmast.rankorder, othcatmast.grporder "

            Dim txt As TextBox
            Dim headerlabel As New TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()

            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each GVROW In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("oclineno")) = False Then
                            Linno = mySqlReader("oclineno")
                        End If
                        'If GVROW.Cells(1).Text = Linno Then
                        If GVROW.Cells(2).Text = mySqlReader("othtypcode") Then
                            StrQryTemp = "select trfplist_costd.tplistcode ,trfplist_costd.plgrpcode,trfplist_costd.othgrpcode,trfplist_costd.oclineno,  " & _
                                " trfplist_costd.othtypcode, trfplist_costd.othcatcode, trfplist_costd.tcostprice, trfplist_costd.pkgnights  " & _
                                "    from  trfplist_costd ,othcatmast,othtypmast  where  trfplist_costd.othcatcode=othcatmast.othcatcode  " & _
                                "  and trfplist_costd.othgrpcode=othcatmast.othgrpcode and  trfplist_costd.othtypcode=othtypmast.othtypcode and   " & _
                                "  trfplist_costd.othgrpcode = othtypmast.othgrpcode and  tplistcode='" & RefCode & "' and trfplist_costd.oclineno='" & Linno & "' order by  " & _
                                "  trfplist_costd.othgrpcode, trfplist_costd.othtypcode, othtypmast.rankorder, othcatmast.grporder"

                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    othcatcode = myReader("othcatcode")

                                    If IsDBNull(myReader("tcostprice")) = False Then
                                        value = myReader("tcostprice")
                                    Else
                                        value = ""
                                    End If
                                    For j = 0 To cnt - 5
                                        If heading(i) = "From Date" Or heading(i) = "To Date" Or heading(i) = "Pkg" Then
                                        Else
                                            For s = 0 To gv_SearchResult.Columns.Count - 5
                                                headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = othcatcode Then
                                                        If GVROW.RowIndex = 0 Then
                                                            txt = GVROW.FindControl("txt" & s)
                                                        Else
                                                            txt = GVROW.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * GVROW.RowIndex) + s + GVROW.RowIndex)
                                                        End If
                                                        If txt Is Nothing Then
                                                        Else
                                                            If value = "" Then
                                                                txt.Text = ""
                                                            Else
                                                                txt.Text = value
                                                            End If
                                                        End If
                                                        txt = GVROW.FindControl("txt" & ((gv_SearchResult.Columns.Count - 5) * GVROW.RowIndex) + gv_SearchResult.Columns.Count - 5 + GVROW.RowIndex)
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
        End Try

    End Sub
#End Region

    Private Sub fillsellingCode(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim GvRow As GridViewRow
        Dim ddl As DropDownList
        Dim ddltyp As DropDownList
        Dim market As String
        Dim dmarket As String


        If ViewState("flag") = "selling" Then
            market = Request.QueryString("marketdis")
        Else
            'dmarket = chksellingTypes()


            'If dmarket.Length > 0 Then
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selling Types is not Generated for the below markets," & dmarket & "' );", True)

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selling types for the selected currency do not exist in the markets, please recheck' );", True)
            '    Exit Sub

            'End If
            market = getmarket()
        End If

        grdSelling.Visible = True
        If grdSelling.PageIndex < 0 Then
            grdSelling.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = "select sellmast.sellcode,sellmast.currcode,isnull((select currrates.convrate from currrates " _
                        & " where currrates.tocurr=sellmast.currcode and currrates.currcode='" & txtCurrCode.Text & "'),0) convrate from sellmast " _
                        & " where sellmast.plgrpcode in (" & market & ") and sellmast.active='1' "

            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            mySqlAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            mySqlAdapter.Fill(myDS)
            grdSelling.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                grdSelling.DataBind()
            Else
                grdSelling.PageIndex = 0
                grdSelling.DataBind()
            End If
            'For Each GvRow In grdSelling.Rows
            '    ddltyp = GvRow.FindControl("ddlFormulaFrom")
            '    ddltyp.SelectedValue = "Category"
            '    ddl = GvRow.FindControl("ddlFormulaCD")
            '    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddl, "frmlcode", "select frmlcode from sellspcath where sellcode='" & GvRow.Cells(0).Text & "' order by frmlcode", True)
            'Next

        Catch ex As Exception
            objUtils.WritErrorLog("HeaderInfo1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(mySqlAdapter)                          'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try


    End Sub

    Private Function getmarket() As String
        Dim chksel As CheckBox
        Dim mktcode As String = ""
        Dim lblcode As Label
        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            lblcode = gvRow1.FindControl("lblcode")
            If chksel.Checked = True Then
                mktcode = mktcode + "'" + lblcode.Text.Trim + "'" + ","
            End If
        Next
        If mktcode.Length > 0 Then
            mktcode = mktcode.Substring(0, mktcode.Length - 1)
        End If

        Return mktcode
    End Function

#Region "Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClearFormula.Click
        Dim obj As New EncryptionDecryption
        Try

            Dim SptypeCode As String = obj.Encrypt(CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SptypeName As String = obj.Encrypt(CType(ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text, String), "&%#@?,:*")

            'Dim MarketCode As String = obj.Encrypt(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String), "&%#@?,:*")
            'Dim MarketName As String = obj.Encrypt(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim SupplierCode As String = obj.Encrypt(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierName As String = obj.Encrypt(CType(ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")

            Dim GroupCode As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            'Dim GroupName As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim currcode As String = obj.Encrypt(CType(txtCurrCode.Text, String), "&%#@?,:*")
            Dim currname As String = obj.Encrypt(CType(txtCurrName.Text, String), "&%#@?,:*")

            Dim SubSeasCode As String = obj.Encrypt(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SubSeasName As String = obj.Encrypt(CType(ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim SupplierAgentCode As String = obj.Encrypt(CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierAgentName As String = obj.Encrypt(CType(ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim chksel As CheckBox
            Dim marketstr As String = ""
            Dim lblcode As Label

            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                lblcode = gvRow1.FindControl("lblcode")
                If chksel.Checked = True Then
                    marketstr = marketstr + ";" + lblcode.Text
                End If
            Next


            If marketstr.Length > 0 Then
                marketstr = marketstr.Substring(1, marketstr.Length - 1)
            End If
            Session("sessionRemark") = txtRemark.Value
            'Dim frmdate As String = obj.Encrypt(dpFromDate.Text, "&%#@?,:*")
            'Dim todate As String = obj.Encrypt(dpToDate.Text, "&%#@?,:*")
            Response.Redirect("TrfPriceList1.aspx?&State=" & ViewState("TrfpricelistState2") & "&RefCode=" & ViewState("TrfpricelistRefCode2") & "&SptypeCode=" & SptypeCode &
                              "&SptypeName=" & SptypeName & "&SupplierCode=" & SupplierCode & "&SupplierName=" & SupplierName & "&SupplierAgentCode=" & SupplierAgentCode &
                              "&SupplierAgentName=" & SupplierAgentName & "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&Market=" & marketstr &
                              "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName &
                              "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) &
                              "&transfertype=" & ddlServerType.SelectedIndex, False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("sessionRemark") = ""
        'Session("GV_HotelData") = ""
        Session("GV_TrfPLData") = ""
        ViewState("TrfpricelistRefCode2") = ""
        ViewState("TrfpricelistState2") = ""
        'Response.Redirect("OtherServicesCostPriceListSearch.aspx", False)

        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('TrfPriceListWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

#Region " Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not Session("PlistSaved") = True Then ''this variable because response.redirect is causing a postback and saving twice

                Dim GvRow As GridViewRow
                If Page.IsValid = True Then
                    If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Edit" Or ViewState("TrfpricelistState2") = "Copy" Then
                        If ValidatePage() = False Then
                            Exit Sub
                        End If
                        If CheckDecimalInGrid() = False Then
                            Exit Sub
                        End If

                        If chkConsdierForMarkUp.Checked = True Then
                            If ViewState("TrfpricelistState2") = "Edit" And chkConsdierForMarkUp.Enabled = False Then
                            Else
                                If ValidateChkMarkUp() = False Then
                                    Exit Sub
                                End If
                            End If
                        End If


                        'End If

                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                        If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                            Dim optionval As String
                            optionval = objUtils.GetAutoDocNo("TRFPLIST", mySqlConn, sqlTrans)
                            txtPlcCode.Value = optionval.Trim
                            mySqlCmd = New SqlCommand("sp_add_TrfPListh", mySqlConn, sqlTrans)
                        ElseIf ViewState("TrfpricelistState2") = "Edit" Then
                            mySqlCmd = New SqlCommand("sp_mod_TrfPListh", mySqlConn, sqlTrans)
                        End If

                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@shuttle", SqlDbType.VarChar, 20)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)

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


                        If CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text.Trim, String)
                        End If
                        If CType(txtCurrCode.Text.Trim, String) = "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrCode.Text.Trim, String)
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Value.Trim, String)
                        If ChkActive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                        ElseIf ChkActive.Checked = False Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                        End If
                        If chkConsdierForMarkUp.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@chkMarkUp", SqlDbType.Int)).Value = 1
                        ElseIf chkConsdierForMarkUp.Checked = False Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@chkMarkUp", SqlDbType.Int)).Value = 0
                        End If
                        '@chkMarkUp

                        If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        End If

                        If CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text.Trim, String)
                        End If

                        If chkapprove.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int, 9)).Value = 1
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int, 9)).Value = 0
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@Transfertype", SqlDbType.Int)).Value = CType(ddlServerType.SelectedIndex, Integer)

                        mySqlCmd.ExecuteNonQuery()
                        '-----------------------------------------------------------
                        '----------------------------------- Deleting Data From Dates Table
                        mySqlCmd = New SqlCommand("sp_del_trfplisth_dates", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()
                        '----------------------------------- Inserting Data To Dates Table
                        For Each GvRow In grdDates.Rows
                            dpFDate = GvRow.FindControl("txtfromDate")
                            dpTDate = GvRow.FindControl("txtToDate")
                            If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                                mySqlCmd = New SqlCommand("sp_add_trfplisth_dates", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)
                                If CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String) = "[Select]" Then
                                    mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                Else
                                    mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String)
                                End If

                                mySqlCmd.ExecuteNonQuery()
                            End If
                        Next

                        'Dim ddlFromula As DropDownList
                        Dim txt1 As TextBox
                        '-----------------------------------------------------------
                        '           Selling 
                        mySqlCmd = New SqlCommand("sp_del_trfplisth_convrates", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                        '----------------------------------- Inserting Data To Selling
                        For Each GvRow In grdSelling.Rows
                            txt1 = GvRow.FindControl("txtconv")
                            'ddl = gvrow.FindControl("ddlFormulaFrom")
                            'If ddl.SelectedValue <> "[Select]" Then

                            mySqlCmd = New SqlCommand("sp_add_trfplisth_convrates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@sellcode", SqlDbType.VarChar, 10)).Value = CType(GvRow.Cells(0).Text, String)
                            If txt1 Is Nothing Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@sellcurrcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@sellcurrcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(txt1.Text, Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmlfrom", SqlDbType.Int)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmlcode", SqlDbType.VarChar, 10)).Value = DBNull.Value
                            mySqlCmd.ExecuteNonQuery()

                        Next

                        mySqlCmd = New SqlCommand("sp_del_trfplisth_market", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                        '----------------------------------- Inserting Data To market_detail Table
                        Dim chksel1 As CheckBox
                        Dim lblcode1 As Label

                        For Each Me.gvRow1 In gv_Market.Rows
                            chksel1 = gvRow1.FindControl("chkSelect")
                            lblcode1 = gvRow1.FindControl("lblcode")
                            If chksel1.Checked = True Then
                                mySqlCmd = New SqlCommand("sp_add_trfplisth_market", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode1.Text.Trim, String)
                                mySqlCmd.ExecuteNonQuery()
                            End If
                        Next

                        '--------------------------------------------- Inserting values to detail cost table--------

                        mySqlCmd = New SqlCommand("sp_del_TrfplistCostd", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
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
                        For header = 0 To cnt - 5
                            txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                            heading(header) = txt.Text
                        Next
                        '----------------------------------------------------------------------------
                        Dim chksel As CheckBox
                        Dim lblcode As Label
                        For Each Me.gvRow1 In gv_Market.Rows
                            j = 0
                            n = 0
                            'm = 0
                            b = 0
                            chksel = gvRow1.FindControl("chkSelect")
                            lblcode = gvRow1.FindControl("lblcode")
                            If chksel.Checked = True Then

                                Dim m As Long = 0
                                For Each GvRow In gv_SearchResult.Rows
                                    If n = 0 Then
                                        For j = 0 To cnt - 5
                                            If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                            Else
                                                mySqlCmd = New SqlCommand("sp_add_TrfplistCostd", mySqlConn, sqlTrans)
                                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                                mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                                'If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
                                                If CType(lblcode.Text, String) = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String) ' CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                                                End If

                                                If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                                End If
                                                mySqlCmd.Parameters.Add(New SqlParameter("@oclineno", SqlDbType.Int)).Value = CType(GvRow.Cells(1).Text, Long)

                                                mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(2).Text, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)


                                                txt = GvRow.FindControl("txt" & j)
                                                If txt.Text = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@tcostprice", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@tcostprice", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
                                                End If

                                                txt = GvRow.FindControl("txt" & b + a + 2)
                                                If txt.Text = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@pknights", SqlDbType.Int)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@pknights", SqlDbType.Int)).Value = CType(txt.Text, Long)
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
                                                mySqlCmd = New SqlCommand("sp_add_TrfplistCostd", mySqlConn, sqlTrans)
                                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                                mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                                'If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
                                                If CType(lblcode.Text, String) = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String) ' CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                                                End If
                                                If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
                                                End If

                                                mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(2).Text, String)
                                                txt = GvRow.FindControl("txt" & j)
                                                If txt.Text = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@tcostprice", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@tcostprice", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
                                                End If

                                                mySqlCmd.Parameters.Add(New SqlParameter("@oclineno", SqlDbType.Int)).Value = CType(GvRow.Cells(1).Text, Long)
                                                txt = GvRow.FindControl("txt" & b + a + 2)
                                                If txt.Text = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@pknights", SqlDbType.Int, 9)).Value = DBNull.Value
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@pknights", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
                                                End If
                                                mySqlCmd.ExecuteNonQuery()
                                                k = k + 1
                                            End If
                                        Next
                                    End If
                                    b = j
                                    n = j
                                Next
                            End If
                        Next
                        '---------------------------------------------End of detail cost table------------------------

                        'ReCalucating the price in the Edit mode
                        If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                            If chkConsdierForMarkUp.Checked = True Then

                                For Each Me.gvRow1 In gv_Market.Rows
                                    chksel = gvRow1.FindControl("chkSelect")
                                    lblcode = gvRow1.FindControl("lblcode")
                                    If chksel.Checked = True Then
                                        mySqlCmd = New SqlCommand("sp_calculate_trfsellingprices", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure
                                        mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                        If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@frmmode", SqlDbType.Int, 9)).Value = 1
                                        ElseIf ViewState("TrfpricelistState2") = "Edit" Then
                                            mySqlCmd.Parameters.Add(New SqlParameter("@frmmode", SqlDbType.Int, 9)).Value = 2
                                        End If
                                        mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@psellcode", SqlDbType.VarChar, 10)).Value = ""

                                        mySqlCmd.ExecuteNonQuery()
                                    End If
                                Next

                                For Each Me.gvRow1 In gv_Market.Rows
                                    chksel = gvRow1.FindControl("chkSelect")
                                    lblcode = gvRow1.FindControl("lblcode")
                                    mySqlCmd = New SqlCommand("sp_del_trfplist_selling", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text.Trim, String)
                                    mySqlCmd.ExecuteNonQuery()
                                Next

                            End If

                        Else
                            If chkConsdierForMarkUp.Checked = True Then
                                If chkrecalsprice.Checked = True Then
                                    For Each Me.gvRow1 In gv_Market.Rows
                                        chksel = gvRow1.FindControl("chkSelect")
                                        lblcode = gvRow1.FindControl("lblcode")
                                        If chksel.Checked = True Then
                                            mySqlCmd = New SqlCommand("sp_calculate_trfsellingprices", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure
                                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                            If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmmode", SqlDbType.Int, 9)).Value = 1
                                            ElseIf ViewState("TrfpricelistState2") = "Edit" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@frmmode", SqlDbType.Int, 9)).Value = 2
                                            End If
                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@psellcode", SqlDbType.VarChar, 10)).Value = ""

                                            mySqlCmd.ExecuteNonQuery()
                                        End If
                                    Next

                                    For Each Me.gvRow1 In gv_Market.Rows
                                        chksel = gvRow1.FindControl("chkSelect")
                                        lblcode = gvRow1.FindControl("lblcode")
                                        mySqlCmd = New SqlCommand("sp_del_trfplist_selling", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure
                                        mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text.Trim, String)
                                        mySqlCmd.ExecuteNonQuery()
                                    Next
                                End If
                            End If
                            End If
                            '---------------------------------------------End of save/edit/copy------------------------
                    ElseIf ViewState("TrfpricelistState2") = "Delete" Then
                            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                            'first deleting all detail tabl values and thn deleting main table values
                            mySqlCmd = New SqlCommand("sp_del_trfplisth_dates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()

                            mySqlCmd = New SqlCommand("sp_del_trfplisth_convrates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()

                            mySqlCmd = New SqlCommand("sp_del_trfplisth_market", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()

                            mySqlCmd = New SqlCommand("sp_del_TrfplistCostd", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                            'sp_del_Trfplisth

                            mySqlCmd = New SqlCommand("sp_del_trfplist_selld", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()

                            mySqlCmd = New SqlCommand("sp_del_trfplist_othcat_slabs", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()

                            'delete of main tbl to be written
                            mySqlCmd = New SqlCommand("sp_del_Trfplisth", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()

                    End If
                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                    Session("PlistSaved") = True
                    Session("sessionRemark") = Nothing
                    'Session("GV_HotelData") = Nothing
                    Session("GV_TrfPLData") = Nothing
                    'ViewState("TrfpricelistRefCode2") = Nothing
                    'ViewState("TrfpricelistState2") = Nothing
                    Session("SesionPlistCode") = txtPlcCode.Value

                    If chkConsdierForMarkUp.Checked = False Then

                        ViewState("TrfpricelistRefCode2") = Nothing
                        ViewState("TrfpricelistState2") = Nothing
                        Response.Redirect("TrfPriceListSearch.aspx", False)

                    Else
                        If ViewState("TrfpricelistState2") = "Delete" Then
                            'Response.Redirect("PriceList.aspx", False)
                            Dim strscript1 As String = ""
                            strscript1 = "window.opener.__doPostBack('TrfPriceListWindowPostBack', '');window.opener.focus();window.close();"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)

                        Else
                            'check if in trf paxcalc reqd thn pax slab page else selling page

                            Dim strentryQuery As String = "select 1  from othgrpmast where othgrpcode =(select option_selected  from reservation_parameters where param_id=1001)" & _
                                                                " and paxcalcreqd=1"
                            SqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strentryQuery)
                            '-------------------market common for both funtnlities-------------------------------
                            Dim chksel As CheckBox
                            Dim marketstr As String = ""
                            Dim lblcode As Label

                            For Each Me.gvRow1 In gv_Market.Rows
                                chksel = gvRow1.FindControl("chkSelect")
                                lblcode = gvRow1.FindControl("lblcode")
                                If chksel.Checked = True Then
                                    marketstr = marketstr + ";" + lblcode.Text
                                End If
                            Next
                            If marketstr.Length > 0 Then
                                marketstr = marketstr.Substring(1, marketstr.Length - 1)
                            End If
                            '---------------------------------------------
                            If SqlReader.HasRows = True Then
                                'to pax slab sellling pages

                                ViewState.Add("TrfpricelistRefCode2", txtPlcCode.Value)
                                Response.Redirect("TrfSellingRatePaxSlab.aspx?State=" & CType(ViewState("TrfpricelistState2"), String) &
                                                  "&RefCode=" & CType(ViewState("TrfpricelistRefCode2"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
                                                  "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPType.Items(ddlSPType.SelectedIndex).Text &
                                                  "&SupplierTypeName=" & ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text & "&Market=" & marketstr &
                                                  "&SuppierAgent=" & ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text &
                                                  "&SupplierAgentName=" & ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text &
                                                  "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                                                  "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                                                  "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text &
                                                  "&transfertype=" & ddlServerType.SelectedIndex, False)


                            Else
                                'to normal seling pages

                                ViewState.Add("TrfpricelistRefCode2", txtPlcCode.Value)
                                'Response.Redirect("HederRsellingcode1new.aspx?State=" & CType(ViewState("HeaderinfoState1"), String) & "&RefCode=" & CType(ViewState("HeaderinfoRefCode1"), String) & "&supplier=" & ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text & "&suppliername=" & ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text & "&SupplierType=" & ddlSPTypeCD.Items(ddlSPTypeCD.SelectedIndex).Text & "&SupplierTypeName=" & ddlSPTypeNM.Items(ddlSPTypeNM.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text & "&SupplierAgentName=" & ddlSuppierAgentNM.Items(ddlSuppierAgentNM.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCD.Value & "&CurrencyName=" & ddlCurrencyNM.Value & "&SubSeasonCode=" & ddlSubSeas.Items(ddlSubSeas.SelectedIndex).Text & "&SubSeasonName=" & ddlSubSeasNM.Items(ddlSubSeasNM.SelectedIndex).Text & "&RevisionDate=" & dpRevsiondate.Text & "&PListCode=" & txtBlockCode.Value & "&hdnpricelist=" & hdnpricelist.Value & "&pricelist=" & ddlPriceList.SelectedValue & "&week1=" & week1 & "&week2=" & week2 & "&manual=" & manual & "&promotionname=" & ddlPromotion.Items(ddlPromotion.SelectedIndex).Text & "&promotioncode=" & txtPromotionCode.Text, False)


                                Response.Redirect("TrfPricelistSellingRates.aspx?State=" & CType(ViewState("TrfpricelistState2"), String) &
                                                  "&RefCode=" & CType(ViewState("TrfpricelistRefCode2"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
                                                  "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPType.Items(ddlSPType.SelectedIndex).Text &
                                                  "&SupplierTypeName=" & ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text & "&Market=" & marketstr &
                                                  "&SuppierAgent=" & ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text &
                                                  "&SupplierAgentName=" & ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text &
                                                  "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                                                  "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                                                  "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text &
                                                  "&transfertype=" & ddlServerType.SelectedIndex, False)


                                'Response.Redirect("TrfPricelistSellingRates.aspx?State=New&RefCode=TRPL/000015&supplier=T-000002&suppliername=FADI ECRS" & _
                                '                  "&SupplierType=Transfers&SupplierTypeName=Transfers&Market=CIS;&SuppierAgent=WONINF&SupplierAgentName=" & _
                                '                  "&CurrencyCode=AED&CurrencyName=DIRHAM&SubSeasonCode=ALL SEASONS&SubSeasonName=ALL", False)






                            End If

                        End If
                    End If


                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('TrfPriceListWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


    Private Function chksellingTypes() As String
        Dim chksel As CheckBox
        Dim Myds1 As DataSet
        Dim lblcode As Label
        strSqlQry = ""
        Dim dismarket As String = ""
        Try


            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                lblcode = gvRow1.FindControl("lblcode")
                If chksel.Checked = True Then

                    strSqlQry = "select sellmast.sellcode,sellmast.currcode,isnull((select currrates.convrate from currrates " _
                                        & " where currrates.tocurr=sellmast.currcode and currrates.currcode='" & txtCurrCode.Text & "'),0) convrate from sellmast " _
                                        & " where sellmast.plgrpcode in ('" & lblcode.Text.Trim & "') and sellmast.active='1'"

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    mySqlAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    Myds1 = New DataSet
                    mySqlAdapter.Fill(Myds1)
                    Dim dt1 As DataTable
                    dt1 = Myds1.Tables(0)

                    If dt1.Rows.Count = 0 Then
                        If dismarket = "" Then
                            dismarket = lblcode.Text.Trim
                        Else
                            dismarket = dismarket + "," + lblcode.Text.Trim
                        End If
                    End If

                End If
            Next


            Return dismarket

        Catch ex As Exception

        End Try
    End Function
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
            For header = 0 To cnt - 5
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
            '----------------------------------------------------------------------------
            Dim m As Long = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesCostPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Private Function FindFooterControl(ByVal Controlname As String, ByVal value As String)"
    Private Function FindHeaderTextbox(ByVal Controlname As String, ByVal value As String) As String
        Dim footerlabel As New TextBox
        Dim irow As Integer
        For irow = 0 To gv_SearchResult.Controls(0).Controls.Count - 1
            If CType(gv_SearchResult.Controls(0).Controls(irow), GridViewRow).RowType = DataControlRowType.Header Then
                Dim footer As GridViewRow = CType(gv_SearchResult.Controls(0).Controls(irow), GridViewRow)
                footerlabel = footer.FindControl(Controlname)
                Return footerlabel.Text
                Exit For
            End If
        Next
        Return footerlabel.Text
    End Function
#End Region

#Region "Private Function ValidatePage() As Boolean"
    '    Private Function ValidatePage() As Boolean
    '        Dim j As Long = 0
    '        Dim txt As TextBox
    '        Dim cnt As Long
    '        Dim srno As Long = 0
    '        Dim hotelcategory As String = ""
    '        Dim n As Long = 0
    '        Dim k As Long = 0
    '        Dim a As Long = cnt - 7
    '        Dim b As Long = 0
    '        Dim header As Long = 0
    '        Dim GvRow As GridViewRow
    '        Dim FrmDate As Date = ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text)
    '        Dim GrdFrmDate As Date
    '        Dim Flag As Boolean = False
    '        Try
    '            'If dpFromDate.Text <> "" And dpToDate.Text <> "" Then
    '            '    If ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text) Then
    '            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should be grater than From date.');", True)
    '            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpToDate.ClientID + "');", True)
    '            '        ValidatePage = False
    '            '        Exit Function
    '            '    End If
    '            'End If
    '            gv_SearchResult.Visible = True
    '            cnt = gv_SearchResult.Columns.Count
    '            Dim heading(cnt + 1) As String
    '            '----------------------------------------------------------------------------
    '            '           Stoaring heading column values in the array
    '            For header = 0 To cnt - 4
    '                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
    '                heading(header) = txt.Text
    '            Next
    '            '----------------------------------------------------------------------------
    '            Dim m As Long = 0
    '            For Each GvRow In gv_SearchResult.Rows
    '                If n = 0 Then
    '                    For j = 0 To cnt - 4
    '                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
    '                        Else
    '                            txt = GvRow.FindControl("txt" & j)
    '                            If txt.Text <> "" Then
    '                                Flag = True
    '                                GoTo Flag
    '                            End If
    '                        End If
    '                    Next
    '                    m = j
    '                Else
    '                    k = 0
    '                    For j = n To (m + n) - 1
    '                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
    '                        Else

    '                            txt = GvRow.FindControl("txt" & j)
    '                            If txt.Text <> "" Then
    '                                Flag = True
    '                                GoTo Flag
    '                            End If
    '                            k = k + 1
    '                        End If
    '                    Next
    '                End If
    '                b = j
    '                n = j
    '            Next
    'Flag:       If Flag = False Then
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Grid Rates can not be left blank.');", True)
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
    '                ValidatePage = False
    '                Exit Function
    '            End If


    '            '-----------------------------------------------------------------------------
    '            a = cnt - 7
    '            j = 0
    '            b = 0
    '            m = 0
    '            n = 0
    '            For Each GvRow In gv_SearchResult.Rows
    '                If n = 0 Then
    '                    For j = 0 To cnt - 4
    '                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
    '                        Else
    '                            txt = GvRow.FindControl("txt" & b + a + 1)
    '                            If txt.Text = "" Then
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates From date can not be left blank.');", True)
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                ValidatePage = False
    '                                Exit Function
    '                            Else
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From date ( " & txt.Text & " )  Should be Greater Than or Equal to Header From Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From date ( " & txt.Text & " ) Should be less Than or Equal to Header To Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
    '                            End If
    '                            txt = GvRow.FindControl("txt" & b + a + 2)
    '                            If txt.Text = "" Then
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                ValidatePage = False
    '                                Exit Function
    '                            Else
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To date ( " & txt.Text & " ) Should be greater Than or Equal to Header From Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To date ( " & txt.Text & " )  Should be less Than or Equal to Header To Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(GrdFrmDate, Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To date  ( " & txt.Text & " )  Should be greater Than or Equal to   From Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                            End If

    '                            txt = GvRow.FindControl("txt" & b + a + 3)
    '                            If txt.Text = "" Then
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Blank.');", True)
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                ValidatePage = False
    '                                Exit Function
    '                            Else
    '                                If CType(txt.Text, Integer) <= 0 Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Zero.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                            End If
    '                        End If
    '                    Next

    '                    m = j
    '                Else
    '                    k = 0
    '                    For j = n To (m + n) - 1
    '                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
    '                        Else

    '                            txt = GvRow.FindControl("txt" & j)

    '                            txt = GvRow.FindControl("txt" & b + a + 1)
    '                            If txt.Text = "" Then
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates From date can not be left blank.');", True)
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                ValidatePage = False
    '                                Exit Function
    '                            Else
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From date ( " & txt.Text & " )  Should be Greater Than or Equal to Header From Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  From date ( " & txt.Text & " ) Should be less Than or Equal to Header To Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
    '                            End If


    '                            txt = GvRow.FindControl("txt" & b + a + 2)
    '                            If txt.Text = "" Then
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                ValidatePage = False
    '                                Exit Function
    '                            Else
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To date ( " & txt.Text & " ) Should be greater Than or Equal to Header From Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.Text), Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To date ( " & txt.Text & " )  Should be less Than or Equal to Header To Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < CType(GrdFrmDate, Date) Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates  To date  ( " & txt.Text & " )  Should be greater Than or Equal to   From Date.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                            End If
    '                            txt = GvRow.FindControl("txt" & b + a + 3)
    '                            If txt.Text = "" Then
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Blank.');", True)
    '                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                ValidatePage = False
    '                                Exit Function
    '                            Else
    '                                If CType(txt.Text, Integer) <= 0 Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Zero.');", True)
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                            End If
    '                            k = k + 1
    '                        End If
    '                    Next
    '                End If
    '                b = j
    '                n = j
    '            Next
    '            '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
    '            ValidatePage = True
    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
    '            objUtils.WritErrorLog("tTrfPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        End Try
    '    End Function

    '    Private Function ValidatePage() As Boolean
    '        'Dim j As Long = 1
    '        'Dim txt As TextBox, txtnights As TextBox
    '        'Dim cnt As Long
    '        'Dim GvRow As GridViewRow
    '        'Dim GvRowRMType As GridViewRow
    '        'Dim gvrowfrmla As GridViewRow
    '        'Dim srno As Long = 0
    '        'Dim hotelcategory As String = ""
    '        'j = 0
    '        'gv_SearchResult.Visible = True
    '        'cnt = gv_SearchResult.Columns.Count
    '        'Dim n As Long = 0
    '        'Dim k As Long = 0
    '        'Dim a As Long = cnt - 10
    '        'Dim b As Long = 0
    '        Dim header As Long = 0
    '        Dim heading(cnt + 1) As String
    '        'Dim flag As Boolean = False
    '        'Dim lblcode As Label
    '        Try
    '            For header = 0 To cnt - 6
    '                heading(header) = FindHeaderTextbox("txtHead" & header, "")
    '            Next
    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

    '        End Try

    '        Dim m As Long = 0

    '        'Dim GvRow1 As GridViewRow
    '        Try
    '            '--------------------   Date Validations    ---------------------------------------
    '            Dim flgdt As Boolean = False
    '            Dim ToDt As Date = Nothing

    '            Dim dtValidate As New DataTable
    '            'dtValidate = Session("GV_HotelData")
    '            dtValidate = Session("GV_TrfPLData")
    '            Dim cntStartCol As Integer = 5
    '            Dim cntEndCol As Integer = gv_SearchResult.Columns.Count - 1
    '            Dim cntCol As Integer = 5
    '            Dim AllowFlg As Integer
    '            Dim ErrMsg As String
    '            Dim txtRMType As TextBox
    '            Dim cntN As Integer = 0
    '            Dim cntJ As Integer = 0

    '            Dim chksel As CheckBox
    '            Dim flg As Boolean
    '            flg = False
    '            For Each Me.gvRow1 In gv_Market.Rows
    '                chksel = gvRow1.FindControl("chkSelect")
    '                If chksel.Checked = True Then
    '                    flg = True
    '                End If
    '            Next
    '            If flg = False Then
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
    '                ValidatePage = False
    '                Exit Function
    '            End If

    '            For Each Me.gvRow1 In gv_Market.Rows
    '                chksel = gvRow1.FindControl("chkSelect")
    '                If chksel.Checked = True Then
    '                    flg = True
    '                End If
    '            Next

    '            If flg = False Then
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
    '                ValidatePage = False
    '                Exit Function
    '            End If

    '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

    '            For Each GvRow In grdDates.Rows


    '                dpFDate = GvRow.FindControl("txtfromDate")
    '                dpTDate = GvRow.FindControl("txtToDate")
    '                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
    '                    If ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) Then
    '                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
    '                        SetFocus(dpTDate)
    '                        ValidatePage = False
    '                        Exit Function
    '                    End If

    '                    If ToDt <> Nothing Then
    '                        If ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) <= ToDt Then
    '                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date Overlapping.');", True)
    '                            SetFocus(dpFDate)
    '                            ValidatePage = False
    '                            Exit Function
    '                        End If
    '                    End If
    '                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)
    '                    flgdt = True
    '                    'Exit For
    '                    'ElseIf dpFDate.txtDate.Text <> "" Or dpTDate.txtDate.Text <> "" Then
    '                    '    If ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.txtDate.Text) <= ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.txtDate.Text) Then
    '                    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
    '                    '        SetFocus(dpTDate.txtDate.Text)
    '                    '        ValidatePage = False
    '                    '        Exit Function
    '                    '    End If
    '                    'flgdt = False

    '                    If (txtPromotionCode.Text.Trim <> "") Then
    '                        mySqlCmd = New SqlCommand("sp_validate_promotionprice", mySqlConn)
    '                        mySqlCmd.CommandType = CommandType.StoredProcedure
    '                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSuppierCD.Value.Trim, String)
    '                        mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
    '                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
    '                        If CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String) = "[Select]" Then
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
    '                        Else
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String)
    '                        End If
    '                        If (txtPromotionCode.Text.Trim = "") Then
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
    '                        Else
    '                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtPromotionCode.Text.Trim, String)
    '                        End If
    '                        Dim paramAllowFlg As New SqlParameter
    '                        paramAllowFlg.ParameterName = "@allowflg"
    '                        paramAllowFlg.Direction = ParameterDirection.Output
    '                        paramAllowFlg.DbType = DbType.Int64
    '                        paramAllowFlg.Size = 20
    '                        mySqlCmd.Parameters.Add(paramAllowFlg)

    '                        Dim paramErrMsg As New SqlParameter
    '                        paramErrMsg.ParameterName = "@errmsg"
    '                        paramErrMsg.Direction = ParameterDirection.Output
    '                        paramErrMsg.DbType = DbType.String
    '                        paramErrMsg.Size = 100
    '                        mySqlCmd.Parameters.Add(paramErrMsg)

    '                        mySqlCmd.ExecuteNonQuery()

    '                        AllowFlg = paramAllowFlg.Value
    '                        ErrMsg = paramErrMsg.Value

    '                        mySqlCmd = Nothing

    '                        If CType(AllowFlg, String) = "1" Then
    '                            ValidatePage = False
    '                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
    '                            Exit Function
    '                        End If
    '                    End If

    '                End If

    '                '-----------------------------------------------------------------------***
    '                'dpFDate = GvRow.FindControl("FrmDate")
    '                'dpTDate = GvRow.FindControl("ToDate")

    '                'check the formulla
    '                Dim ddlFormulaCD As DropDownList
    '                For Each gvrowfrmla In grdSelling.Rows
    '                    ddlFormulaCD = gvrowfrmla.FindControl("ddlFormulaCD")
    '                    If CType(ddlFormulaCD.SelectedValue, String) = "[Select]" Then
    '                        If chkcnfformula.Checked = False Then
    '                            'chkcnfformula.Visible = True
    '                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Formulas selected, click on Confirm blank formulas below and save');", True)
    '                            Exit Function
    '                        End If
    '                    End If
    '                Next

    '                If dpFDate.Text <> "" And dpTDate.Text <> "" Then

    '                    For Each Me.gvRow1 In gv_Market.Rows
    '                        chksel = gvRow1.FindControl("chkSelect")
    '                        lblcode = gvRow1.FindControl("lblcode")
    '                        If chksel.Checked = True Then

    '                            j = 0
    '                            n = 0
    '                            m = 0
    '                            b = 0
    '                            For Each GvRowRMType In gv_SearchResult.Rows
    '                                If n = 0 Then
    '                                    For j = 0 To cnt - 6
    '                                        If heading(j) = "Y/n" Or heading(j) = "Pkg" Or heading(j) = "Canc Days" Or heading(j) = "Compulsory Nights" Or heading(j) = "Remark" Then
    '                                        Else
    '                                            txt = GvRowRMType.FindControl("txt" & j)
    '                                            If txt Is Nothing Then
    '                                            Else
    '                                                If txt.Text <> "" Then
    '                                                    If CType(Val(txt.Text), String) <> "0" Then
    '                                                        txtRMType = GvRowRMType.FindControl("txtrmtypcode")
    '                                                        If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
    '                                                            mySqlCmd = New SqlCommand("sp_chkplwkdatenew", mySqlConn)
    '                                                        Else
    '                                                            mySqlCmd = New SqlCommand("sp_chkpldatenew", mySqlConn)
    '                                                        End If
    '                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSuppierCD.Value.Trim, String)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtRMType.Text.Trim, String)

    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(gv_SearchResult.Rows(GvRowRMType.RowIndex).Cells(3).Text.Trim, String)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(dtValidate.Columns(cntCol).ColumnName.Trim, String)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
    '                                                        If ViewState("HeaderinfoState1") = "New" Or ViewState("HeaderinfoState1") = "Copy" Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = ""
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(TxtPLCD.Text.Trim, String)
    '                                                        End If
    '                                                        If CType(lblcode.Text, String) = "" Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String)
    '                                                        End If

    '                                                        txtnights = GvRowRMType.FindControl("txt" & b + a + 1)
    '                                                        If Not txtnights Is Nothing Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 20)).Value = CType(txtnights.Text.Trim, Integer)

    '                                                        End If
    '                                                        If CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String) = "[Select]" Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String)
    '                                                        End If

    '                                                        If (txtPromotionCode.Text.Trim = "") Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtPromotionCode.Text.Trim, String)
    '                                                        End If
    '                                                        Dim paramAllowFlg As New SqlParameter
    '                                                        paramAllowFlg.ParameterName = "@allowflg"
    '                                                        paramAllowFlg.Direction = ParameterDirection.Output
    '                                                        paramAllowFlg.DbType = DbType.Int64
    '                                                        paramAllowFlg.Size = 20
    '                                                        mySqlCmd.Parameters.Add(paramAllowFlg)

    '                                                        Dim paramErrMsg As New SqlParameter
    '                                                        paramErrMsg.ParameterName = "@errmsg"
    '                                                        paramErrMsg.Direction = ParameterDirection.Output
    '                                                        paramErrMsg.DbType = DbType.String
    '                                                        paramErrMsg.Size = 100
    '                                                        mySqlCmd.Parameters.Add(paramErrMsg)

    '                                                        mySqlCmd.ExecuteNonQuery()

    '                                                        AllowFlg = paramAllowFlg.Value
    '                                                        ErrMsg = paramErrMsg.Value

    '                                                        mySqlCmd = Nothing

    '                                                        If CType(AllowFlg, String) = "1" Then
    '                                                            ValidatePage = False
    '                                                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
    '                                                            Exit Function
    '                                                        End If
    '                                                    End If
    '                                                End If
    '                                            End If

    '                                        End If
    '                                    Next
    '                                    m = j
    '                                Else
    '                                    k = 0
    '                                    For j = n To (m + n) - 1
    '                                        If heading(k) = "Y/n" Or heading(k) = "Pkg" Or heading(k) = "Canc Days" Or heading(k) = "Compulsory Nights" Or heading(k) = "Remark" Then
    '                                        Else
    '                                            txt = GvRowRMType.FindControl("txt" & j)
    '                                            If txt Is Nothing Then
    '                                            Else
    '                                                If txt.Text <> "" Then
    '                                                    If CType(Val(txt.Text), String) <> "0" Then
    '                                                        txtRMType = GvRowRMType.FindControl("txtrmtypcode")
    '                                                        If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
    '                                                            mySqlCmd = New SqlCommand("sp_chkplwkdatenew", mySqlConn)
    '                                                        Else
    '                                                            mySqlCmd = New SqlCommand("sp_chkpldatenew", mySqlConn)
    '                                                        End If
    '                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSuppierCD.Value.Trim, String)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtRMType.Text.Trim, String)

    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(gv_SearchResult.Rows(GvRowRMType.RowIndex).Cells(3).Text.Trim, String)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(dtValidate.Columns(cntCol).ColumnName.Trim, String)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
    '                                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
    '                                                        If ViewState("HeaderinfoState1") = "New" Or ViewState("HeaderinfoState1") = "Copy" Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = ""
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(TxtPLCD.Text.Trim, String)
    '                                                        End If

    '                                                        If CType(lblcode.Text, String) = "" Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String)
    '                                                        End If

    '                                                        txtnights = GvRowRMType.FindControl("txt" & b + a + 1)
    '                                                        If Not txtnights Is Nothing Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int, 20)).Value = CType(txtnights.Text.Trim, Integer)
    '                                                        End If
    '                                                        If CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String) = "[Select]" Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String)
    '                                                        End If

    '                                                        If (txtPromotionCode.Text.Trim = "") Then
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
    '                                                        Else
    '                                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtPromotionCode.Text.Trim, String)
    '                                                        End If

    '                                                        Dim paramAllowFlg As New SqlParameter
    '                                                        paramAllowFlg.ParameterName = "@allowflg"
    '                                                        paramAllowFlg.Direction = ParameterDirection.Output
    '                                                        paramAllowFlg.DbType = DbType.Int64
    '                                                        paramAllowFlg.Size = 20
    '                                                        mySqlCmd.Parameters.Add(paramAllowFlg)

    '                                                        Dim paramErrMsg As New SqlParameter
    '                                                        paramErrMsg.ParameterName = "@errmsg"
    '                                                        paramErrMsg.Direction = ParameterDirection.Output
    '                                                        paramErrMsg.DbType = DbType.String
    '                                                        paramErrMsg.Size = 100
    '                                                        mySqlCmd.Parameters.Add(paramErrMsg)

    '                                                        mySqlCmd.ExecuteNonQuery()

    '                                                        AllowFlg = paramAllowFlg.Value
    '                                                        ErrMsg = paramErrMsg.Value

    '                                                        mySqlCmd = Nothing

    '                                                        If CType(AllowFlg, String) = "1" Then
    '                                                            ValidatePage = False
    '                                                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
    '                                                            Exit Function
    '                                                        End If
    '                                                    End If
    '                                                End If
    '                                            End If

    '                                        End If
    '                                        k = k + 1
    '                                    Next
    '                                End If
    '                                b = j
    '                                n = j
    '                            Next

    '                        End If
    '                    Next

    '                End If
    '            Next


    '            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
    '            '-----------------------------------------------------------------------***

    '            If flgdt = False Then
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Dates grid should not be blank.');", True)
    '                SetFocus(grdDates)
    '                ValidatePage = False
    '                Exit Function
    '            End If
    '            '------------------------------------------------------------------------------------
    '            b = 0
    '            n = 0
    '            For Each GvRow In gv_SearchResult.Rows
    '                If n = 0 Then
    '                    For j = 0 To cnt - 6
    '                        If heading(j) = "Y/n" Or heading(j) = "Pkg" Or heading(j) = "Canc Days" Or heading(j) = "Compulsory Nights" Or heading(j) = "Remark" Then
    '                        Else
    '                            txt = GvRow.FindControl("txt" & j)
    '                            If txt Is Nothing Then
    '                            Else
    '                                If txt.Text <> "" Then
    '                                    flag = True
    '                                    GoTo Err
    '                                End If
    '                            End If

    '                        End If
    '                    Next
    '                    m = j
    '                Else
    '                    k = 0
    '                    For j = n To (m + n) - 1
    '                        If heading(k) = "Y/n" Or heading(k) = "Pkg" Or heading(k) = "Canc Days" Or heading(k) = "Compulsory Nights" Or heading(k) = "Remark" Then
    '                        Else
    '                            txt = GvRow.FindControl("txt" & j)
    '                            If txt Is Nothing Then
    '                            Else
    '                                If txt.Text <> "" Then
    '                                    flag = True
    '                                    GoTo Err
    '                                End If
    '                            End If

    '                        End If
    '                        k = k + 1
    '                    Next
    '                End If
    '                b = j
    '                n = j
    '            Next

    'Err:        If flag = False Then
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in net cost.');", True)
    '                ValidatePage = False
    '                Exit Function
    '            End If

    '            flag = False
    '            b = 0
    '            n = 0
    '            For Each GvRow In gv_SearchResult.Rows
    '                If n = 0 Then
    '                    For j = 0 To cnt - 6
    '                        If heading(j) = "Y/n" Or heading(j) = "Pkg" Or heading(j) = "Canc Days" Or heading(j) = "Compulsory Nights" Or heading(j) = "Remark" Then
    '                        Else
    '                            txt = GvRow.FindControl("txt" & b + a + 1)
    '                            If txt Is Nothing Then
    '                            Else
    '                                If txt.Text = "" Then
    '                                    'Package days can not be left blank.
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days can not be left blank.');", True)
    '                                    SetFocus(txt)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                Else
    '                                    If GvRow.Cells(b + a).Text = "0" Then
    '                                        If ddlPriceList.SelectedValue = "Weekly Rates 7 Nights" Then
    '                                            If CType(txt.Text, Integer) <> 7 Then
    '                                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days should be 7 for Weekly Rates 7 Nights.');", True)
    '                                                SetFocus(txt)
    '                                                ValidatePage = False
    '                                                Exit Function
    '                                            End If
    '                                        ElseIf ddlPriceList.SelectedValue = "Normal Rates 1 Night" Then
    '                                            If CType(txt.Text, Integer) <> 1 Then
    '                                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days should be 1 for Normal Rates 1 Night.');", True)
    '                                                SetFocus(txt)
    '                                                ValidatePage = False
    '                                                Exit Function
    '                                            End If
    '                                        ElseIf ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Then
    '                                            If CType(txt.Text, Integer) <> 1 Then
    '                                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days should be 1 for Weekend Rates 1 Night.');", True)
    '                                                SetFocus(txt)
    '                                                ValidatePage = False
    '                                                Exit Function
    '                                            End If
    '                                        ElseIf ddlPriceList.SelectedValue = "Normal Rates > 1 Night" Then
    '                                            If CType(txt.Text, Integer) <> 1 Or CType(txt.Text, Integer) <> 7 Then
    '                                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days should not be 1 or 7 for Normal Rates > 1 Night.');", True)
    '                                                SetFocus(txt)
    '                                                ValidatePage = False
    '                                                Exit Function
    '                                            End If
    '                                        ElseIf ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
    '                                            If CType(txt.Text, Integer) <> 1 Or CType(txt.Text, Integer) <> 7 Then
    '                                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days should not be 1 or 7 for Weekend Rates > 1 Night.');", True)
    '                                                SetFocus(txt)
    '                                                ValidatePage = False
    '                                                Exit Function
    '                                            End If
    '                                        End If
    '                                    End If
    '                                    'flag = True
    '                                    'ValidatePage = True
    '                                    'Exit Function
    '                                End If
    '                            End If


    '                            'txt = GvRow.FindControl("txt" & b + a + 2)
    '                            'If txt Is Nothing Then
    '                            'Else
    '                            '    If txt.Text = "" Then
    '                            '    End If
    '                            'End If
    '                            'txt = GvRow.FindControl("txt" & j)
    '                            'If txt Is Nothing Then
    '                            'Else
    '                            '    If txt.Text = "" Then
    '                            '    End If
    '                            'End If

    '                            'txt = GvRow.FindControl("txt" & b + a + 3)
    '                            'If txt Is Nothing Then
    '                            'Else
    '                            '    If txt.Text = "" Then
    '                            '    End If
    '                            'End If
    '                            txt = GvRow.FindControl("txt" & b + a + 3)
    '                            If txt Is Nothing Then
    '                            Else
    '                                If txt.Text = "" Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Compalsory nights can not be left blank.');", True)
    '                                    SetFocus(txt)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                            End If
    '                        End If
    '                    Next
    '                    m = j
    '                Else
    '                    k = 0
    '                    For j = n To (m + n) - 1
    '                        If heading(k) = "Y/n" Or heading(k) = "Pkg" Or heading(k) = "Canc Days" Or heading(k) = "Compulsory Nights" Or heading(k) = "Remark" Then
    '                        Else
    '                            txt = GvRow.FindControl("txt" & b + a + 1)
    '                            If Not txt Is Nothing Then
    '                                If txt.Text = "" Then
    '                                    'Package days can not be left blank.
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days can not be left blank.');", True)
    '                                    SetFocus(txt)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                Else
    '                                    'ValidatePage = True
    '                                    'Exit Function
    '                                End If
    '                            End If

    '                            'txt = GvRow.FindControl("txt" & b + a + 2)
    '                            'If txt Is Nothing Then
    '                            'Else
    '                            '    If txt.Text = "" Then
    '                            '    End If
    '                            'End If

    '                            'txt = GvRow.FindControl("txt" & j)
    '                            'If txt Is Nothing Then
    '                            'Else
    '                            '    If txt.Text = "" Then
    '                            '    End If
    '                            'End If
    '                            txt = GvRow.FindControl("txt" & b + a + 3)
    '                            If txt Is Nothing Then
    '                            Else
    '                                If txt.Text = "" Then
    '                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Compalsory nights can not be left blank.');", True)
    '                                    SetFocus(txt)
    '                                    ValidatePage = False
    '                                    Exit Function
    '                                End If
    '                            End If
    '                            'txt = GvRow.FindControl("txt" & b + a + 4)
    '                            'If txt Is Nothing Then
    '                            'Else
    '                            '    If txt.Text = "" Then
    '                            '    End If
    '                            'End If
    '                        End If
    '                        k = k + 1
    '                    Next
    '                End If
    '                b = j
    '                n = j
    '            Next
    '            ValidatePage = True
    '        Catch ex As Exception
    '            objUtils.WritErrorLog("HeaderInfo1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        End Try
    '    End Function

    Private Function ValidatePage() As Boolean
        Dim j As Long = 1
        Dim txt As TextBox, txtnights As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow
        'Dim GvRowRMType As GridViewRow
        'Dim gvrowfrmla As GridViewRow
        'Dim srno As Long = 0
        'Dim hotelcategory As String = ""
        j = 0
        gv_SearchResult.Visible = True
        cnt = gv_SearchResult.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0
        Dim a As Long = cnt - 7
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False
        Dim lblcode As Label
        Dim ddmarket As String
        Try
            For header = 0 To cnt - 5
                heading(header) = FindHeaderTextbox("txtHead" & header, "")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try


        '-------Selling type checking ----------
        'Try
        '    ddmarket = chksellingTypes()
        '    If ddmarket.Length > 0 Then
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selling types for the selected currency do not exist in the markets, please recheck' );", True)
        '        ValidatePage = False
        '        Exit Function
        '    End If
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        'End Try

        Dim m As Long = 0

        'Dim GvRow1 As GridViewRow
        Try
            '--------------------   Date Validations    ---------------------------------------
            Dim flgdt As Boolean = False
            Dim ToDt As Date = Nothing

            'Dim dtValidate As New DataTable
            'dtValidate = Session("GV_HotelData")
            Dim cntStartCol As Integer = 5
            Dim cntEndCol As Integer = gv_SearchResult.Columns.Count - 1
            Dim cntCol As Integer = 5
            Dim AllowFlg As Integer
            Dim ErrMsg As String
            Dim txtRMType As TextBox
            Dim cntN As Integer = 0
            Dim cntJ As Integer = 0

            Dim chksel As CheckBox
            Dim flg As Boolean
            flg = False
            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                If chksel.Checked = True Then
                    flg = True
                End If
            Next
            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
                ValidatePage = False
                Exit Function
            End If

            If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                If ddlServerType.SelectedValue = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Transfer Type');", True)
                    ValidatePage = False
                    Exit Function
                End If
            End If

            'For Each Me.gvRow1 In gv_Market.Rows
            '    chksel = gvRow1.FindControl("chkSelect")
            '    If chksel.Checked = True Then
            '        flg = True
            '    End If
            'Next

            'If flg = False Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
            '    ValidatePage = False
            '    Exit Function
            'End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            For Each GvRow In grdDates.Rows
                dpFDate = GvRow.FindControl("txtfromDate")
                dpTDate = GvRow.FindControl("txtToDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    If ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(dpTDate)
                        ValidatePage = False
                        Exit Function
                    End If

                    If ToDt <> Nothing Then
                        If ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) <= ToDt Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date Overlapping.');", True)
                            SetFocus(dpFDate)
                            ValidatePage = False
                            Exit Function
                        End If
                    End If
                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)
                    flgdt = True
                    '''''''''''''''
                    ''''''''''''''''''
                End If

                '-----------------------------------------------------------------------***
                'dpFDate = GvRow.FindControl("FrmDate")
                'dpTDate = GvRow.FindControl("ToDate")

                'check the formulla
                'Dim ddlFormulaCD As DropDownList
                'For Each gvrowfrmla In grdSelling.Rows
                '    ddlFormulaCD = gvrowfrmla.FindControl("ddlFormulaCD")
                '    If CType(ddlFormulaCD.SelectedValue, String) = "[Select]" Then
                '        If chkcnfformula.Checked = False Then
                '            'chkcnfformula.Visible = True
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Formulas selected, click on Confirm blank formulas below and save');", True)
                '            Exit Function
                '        End If
                '    End If
                'Next

                If dpFDate.Text <> "" And dpTDate.Text <> "" Then

                    For Each Me.gvRow1 In gv_Market.Rows
                        chksel = gvRow1.FindControl("chkSelect")
                        lblcode = gvRow1.FindControl("lblcode")
                        If chksel.Checked = True Then

                            j = 0
                            n = 0
                            m = 0
                            b = 0
                            For Each GvRowRMType In gv_SearchResult.Rows
                                If n = 0 Then
                                    For j = 0 To cnt - 5
                                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                        Else
                                            txt = GvRowRMType.FindControl("txt" & j)
                                            If txt Is Nothing Then
                                            Else
                                                If txt.Text <> "" Then
                                                    If CType(Val(txt.Text), String) <> "0" Then
                                                        'txtRMType = GvRowRMType.FindControl("txtrmtypcode")
                                                        'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
                                                        '    mySqlCmd = New SqlCommand("sp_chkplwkdatenew", mySqlConn)
                                                        'Else
                                                        mySqlCmd = New SqlCommand("sp_chkTrfpldatenew", mySqlConn)
                                                        'End If
                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierName.Value.Trim, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)

                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtRMType.Text.Trim, String)

                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(gv_SearchResult.Rows(GvRowRMType.RowIndex).Cells(3).Text.Trim, String)
                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(dtValidate.Columns(cntCol).ColumnName.Trim, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
                                                        If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = ""
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                                        End If
                                                        If CType(lblcode.Text, String) = "" Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String)
                                                        End If

                                                        txtnights = GvRowRMType.FindControl("txt" & b + a + 2)
                                                        If Not txtnights Is Nothing Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 20)).Value = CType(txtnights.Text.Trim, Integer)

                                                        End If
                                                        If CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String) = "[Select]" Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String)
                                                        End If

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.Int)).Value = CType(ddlServerType.SelectedIndex, Integer)

                                                        'If (txtPromotionCode.Text.Trim = "") Then
                                                        '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                                                        'Else
                                                        '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtPromotionCode.Text.Trim, String)
                                                        'End If
                                                        Dim paramAllowFlg As New SqlParameter
                                                        paramAllowFlg.ParameterName = "@allowflg"
                                                        paramAllowFlg.Direction = ParameterDirection.Output
                                                        paramAllowFlg.DbType = DbType.Int64
                                                        paramAllowFlg.Size = 20
                                                        mySqlCmd.Parameters.Add(paramAllowFlg)

                                                        Dim paramErrMsg As New SqlParameter
                                                        paramErrMsg.ParameterName = "@errmsg"
                                                        paramErrMsg.Direction = ParameterDirection.Output
                                                        paramErrMsg.DbType = DbType.String
                                                        paramErrMsg.Size = 100
                                                        mySqlCmd.Parameters.Add(paramErrMsg)

                                                        mySqlCmd.ExecuteNonQuery()

                                                        AllowFlg = paramAllowFlg.Value
                                                        ErrMsg = paramErrMsg.Value

                                                        mySqlCmd = Nothing

                                                        If CType(AllowFlg, String) = "1" Then
                                                            ValidatePage = False
                                                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
                                                            Exit Function
                                                        End If
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
                                            txt = GvRowRMType.FindControl("txt" & j)
                                            If txt Is Nothing Then
                                            Else
                                                If txt.Text <> "" Then
                                                    If CType(Val(txt.Text), String) <> "0" Then
                                                        'txtRMType = GvRowRMType.FindControl("txtrmtypcode")
                                                        'If ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Or ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
                                                        '    mySqlCmd = New SqlCommand("sp_chkplwkdatenew", mySqlConn)
                                                        'Else
                                                        mySqlCmd = New SqlCommand("sp_chkTrfpldatenew", mySqlConn)
                                                        'End If
                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierName.Value.Trim, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)

                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtRMType.Text.Trim, String)

                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(gv_SearchResult.Rows(GvRowRMType.RowIndex).Cells(3).Text.Trim, String)
                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(dtValidate.Columns(cntCol).ColumnName.Trim, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
                                                        If ViewState("TrfpricelistState2") = "New" Or ViewState("TrfpricelistState2") = "Copy" Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = ""
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@tplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                                        End If

                                                        If CType(lblcode.Text, String) = "" Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode.Text, String)
                                                        End If

                                                        txtnights = GvRowRMType.FindControl("txt" & b + a + 2)
                                                        If Not txtnights Is Nothing Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 20)).Value = CType(txtnights.Text.Trim, Integer)
                                                        End If
                                                        If CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String) = "[Select]" Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String)
                                                        End If

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.Int)).Value = CType(ddlServerType.SelectedIndex, Integer)

                                                        'If (txtPromotionCode.Text.Trim = "") Then
                                                        '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                                                        'Else
                                                        '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtPromotionCode.Text.Trim, String)
                                                        'End If

                                                        Dim paramAllowFlg As New SqlParameter
                                                        paramAllowFlg.ParameterName = "@allowflg"
                                                        paramAllowFlg.Direction = ParameterDirection.Output
                                                        paramAllowFlg.DbType = DbType.Int64
                                                        paramAllowFlg.Size = 20
                                                        mySqlCmd.Parameters.Add(paramAllowFlg)

                                                        Dim paramErrMsg As New SqlParameter
                                                        paramErrMsg.ParameterName = "@errmsg"
                                                        paramErrMsg.Direction = ParameterDirection.Output
                                                        paramErrMsg.DbType = DbType.String
                                                        paramErrMsg.Size = 100
                                                        mySqlCmd.Parameters.Add(paramErrMsg)

                                                        mySqlCmd.ExecuteNonQuery()

                                                        AllowFlg = paramAllowFlg.Value
                                                        ErrMsg = paramErrMsg.Value

                                                        mySqlCmd = Nothing

                                                        If CType(AllowFlg, String) = "1" Then
                                                            ValidatePage = False
                                                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
                                                            Exit Function
                                                        End If
                                                    End If
                                                End If
                                            End If

                                        End If
                                        k = k + 1
                                    Next
                                End If
                                b = j
                                n = j
                            Next

                        End If
                    Next

                End If
            Next


            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            '-----------------------------------------------------------------------***

            If flgdt = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Dates grid should not be blank.');", True)
                SetFocus(grdDates)
                ValidatePage = False
                Exit Function
            End If
            '------------------------------------------------------------------------------------
            b = 0
            n = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    flag = True
                                    GoTo Err
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
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    flag = True
                                    GoTo Err
                                End If
                            End If

                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next

Err:        If flag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in net cost.');", True)
                ValidatePage = False
                Exit Function
            End If

            flag = False
            b = 0
            n = 0
            For Each GvRow In gv_SearchResult.Rows
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If txt Is Nothing Then
                            Else
                                If txt.Text = "" Then
                                    'Package days can not be left blank.
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days can not be left blank.');", True)
                                    SetFocus(txt)
                                    ValidatePage = False
                                    Exit Function
                                    'Else
                                    '    If GvRow.Cells(b + a).Text = "0" Then
                                    '' pricelistddl fnctns
                                    'End If
                                    '    'flag = True
                                    '    'ValidatePage = True
                                    '    'Exit Function
                                End If
                            End If


                            'txt = GvRow.FindControl("txt" & b + a + 2)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If
                            'txt = GvRow.FindControl("txt" & j)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If

                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If
                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Compalsory nights can not be left blank.');", True)
                            '        SetFocus(txt)
                            '        ValidatePage = False
                            '        Exit Function
                            '    End If
                            'End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                        Else
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If Not txt Is Nothing Then
                                If txt.Text = "" Then
                                    'Package days can not be left blank.
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package days can not be left blank.');", True)
                                    SetFocus(txt)
                                    ValidatePage = False
                                    Exit Function
                                Else
                                    'ValidatePage = True
                                    'Exit Function
                                End If
                            End If

                            'txt = GvRow.FindControl("txt" & b + a + 2)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If

                            'txt = GvRow.FindControl("txt" & j)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '    End If
                            'End If
                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            'If txt Is Nothing Then
                            'Else
                            '    If txt.Text = "" Then
                            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Compalsory nights can not be left blank.');", True)
                            '        SetFocus(txt)
                            '        ValidatePage = False
                            '        Exit Function
                            '    End If
                            'End If
                            ''txt = GvRow.FindControl("txt" & b + a + 4)
                            ''If txt Is Nothing Then
                            ''Else
                            ''    If txt.Text = "" Then
                            ''    End If
                            ''End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ValidatePage = True
    End Function
#End Region

#Region " ValidateChkMarkUp "
    Private Function ValidateChkMarkUp() As Boolean
        ValidateChkMarkUp = True
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        Dim AllowFlg As Integer
        Dim ErrMsg As String
        Dim GvRow As GridViewRow
        Dim gvMarketRow As GridViewRow
        Dim chksel As CheckBox
        Dim lblcode As Label

        If chkConsdierForMarkUp.Checked = True Then
            'check whether any other supplier is already marked up for same time period
            For Each GvRow In grdDates.Rows
                dpFDate = GvRow.FindControl("txtfromDate")
                dpTDate = GvRow.FindControl("txtToDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    For Each gvMarketRow In gv_Market.Rows
                        chksel = gvMarketRow.FindControl("chkSelect")
                        lblcode = gvMarketRow.FindControl("lblcode")
                        If chksel.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_chkExistMarkUpSuplier", mySqlConn)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text.Trim), Date)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text.Trim), Date)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = lblcode.Text.Trim
                            mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.Int)).Value = CType(ddlServerType.SelectedIndex, Integer)

                            Dim paramAllowFlg As New SqlParameter
                            paramAllowFlg.ParameterName = "@allowflg"
                            paramAllowFlg.Direction = ParameterDirection.Output
                            paramAllowFlg.DbType = DbType.Int64
                            paramAllowFlg.Size = 20
                            mySqlCmd.Parameters.Add(paramAllowFlg)

                            Dim paramErrMsg As New SqlParameter
                            paramErrMsg.ParameterName = "@errmsg"
                            paramErrMsg.Direction = ParameterDirection.Output
                            paramErrMsg.DbType = DbType.String
                            paramErrMsg.Size = 100
                            mySqlCmd.Parameters.Add(paramErrMsg)

                            mySqlCmd.ExecuteNonQuery()

                            AllowFlg = paramAllowFlg.Value
                            ErrMsg = paramErrMsg.Value

                            mySqlCmd = Nothing

                            If CType(AllowFlg, String) = "1" Then
                                ValidateChkMarkUp = False
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
                                Exit Function
                            End If
                        End If
                    Next
                        End If
                    Next
                End If

    End Function

#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=TrfPriceList2','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        Dim cnt As Integer
        If ViewState("TrfpricelistState2") = "New" Then
            cnt = 7
        Else
            If ViewState("TrfpricelistRefCode2") <> Nothing Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                strSqlQry = "select count(*) from trfplisth_dates where tplistcode='" + ViewState("TrfpricelistRefCode2") + "'"
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                cnt = mySqlCmd.ExecuteScalar
                mySqlConn.Close()
            End If
        End If

        If blnload = True Then
            lngcnt = cnt '10
        Else
            lngcnt = count
        End If
        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region

    '#Region "Public Sub fillDateinDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    '    Public Sub fillDateinDategrd(ByVal grd As GridView)
    '        Try
    '            ' Dim dtp As EclipseWebSolutions.DatePicker.DatePicker

    '            'If (txtBookingValidityFrom.Text <> "null") Then
    '            '    If (txtBookingValidityFrom.Text.Trim <> "") Then
    '            '        dtp = grd.Rows(0).Cells(1).Controls(1)
    '            '        dtp.DateValue = txtBookingValidityFrom.Text.Replace("-", "/")
    '            '    End If
    '            'End If
    '            'If (txtBookingValidityTo.Text <> "null") Then
    '            '    If (txtBookingValidityTo.Text.Trim <> "") Then
    '            '        dtp = grd.Rows(0).Cells(2).Controls(1)
    '            '        dtp.DateValue = txtBookingValidityTo.Text.Replace("-", "/")
    '            '    End If
    '            'End If

    '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '            mySqlCmd = New SqlCommand("Select frmdate,todate  from promotion_header Where promotionid='" & txtPromotionCode.Text & "'", mySqlConn)
    '            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
    '            If mySqlReader.HasRows Then
    '                If mySqlReader.Read() = True Then
    '                    If IsDBNull(mySqlReader("frmdate")) = False Then
    '                        dpFDate = grd.Rows(0).Cells(1).Controls(1)
    '                        dpFDate.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)
    '                    End If
    '                    If IsDBNull(mySqlReader("todate")) = False Then
    '                        dpTDate = grd.Rows(0).Cells(2).Controls(1)
    '                        dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
    '                    End If
    '                End If
    '            End If

    '        Catch ex As Exception
    '            objUtils.WritErrorLog("HeaderInfo1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        Finally
    '            clsDBConnect.dbReaderClose(mySqlReader)
    '            clsDBConnect.dbCommandClose(mySqlCmd)
    '            clsDBConnect.dbConnectionClose(mySqlConn)
    '        End Try
    '    End Sub
    '#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("no", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region

    Protected Sub btnAddLines_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLines()
    End Sub

    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim n As Integer = 0

        Try
            For Each GVRow In grdDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                n = n + 1
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub



    Protected Sub btnPreviousRates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreviousRates.Click
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        gv_SearchResult.Visible = True
        cnt = gv_SearchResult.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0

        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 6
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("HeaderInfo1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Try
            For Each GvRow In gv_SearchResult.Rows
                chk = GvRow.FindControl("ChkSelect")
                If chk.Checked = True Then
                    cnt_checked = cnt_checked + 1
                End If
            Next
            If cnt_checked = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row');", True)
                SetFocus(cnt_checked)
                Exit Sub
            End If


            Dim arr(3) As String
            'Dim arr_pkg(3) As String
            'Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            'Dim pkg As Long = 0

            For Each GvRow In gv_SearchResult.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                            Else
                                'If pkg = 0 Then
                                '    txt = GvRow.FindControl("txt" & b + a + 1)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_pkg(pkg) = txt.Text
                                '        End If
                                '    End If

                                '    txt = GvRow.FindControl("txt" & b + a + 2)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_cancdays(pkg) = txt.Text
                                '        End If
                                '    End If

                                'End If
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                'pkg = pkg + 1
                                room = room + 1
                            End If
                        End If
                    Next

                    m = j
                    'a = j
                Else
                    k = 0
                    'pkg = 0
                    For j = n To (m + n) - 1
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                            Else
                                'If pkg = 0 Then
                                '    txt = GvRow.FindControl("txt" & b + a + 1)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_pkg(pkg) = txt.Text
                                '        End If
                                '    End If

                                '    txt = GvRow.FindControl("txt" & b + a + 2)
                                '    If txt Is Nothing Then
                                '    Else
                                '        If txt.Text <> Nothing Then
                                '            arr_cancdays(pkg) = txt.Text
                                '        End If
                                '    End If
                                'End If
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                'pkg = pkg + 1
                                room = room + 1
                            End If
                        End If
                        k = k + 1
                    Next

                End If

                b = j
                n = j
            Next
            '--------------------------------------------------------------------------------------------
            'Noe Fill Record to Cell
            room = 0
            'pkg = 0
            n = 0
            b = 0

            For Each GvRow In gv_SearchResult.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 5
                        If GvRow.RowIndex = row_id + 1 Then
                            If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                            Else
                                'txt = GvRow.FindControl("txt" & b + a + 1)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_pkg(pkg)
                                '    End If
                                'End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    txt.Text = arr_room(room)
                                End If

                                'txt = GvRow.FindControl("txt" & b + a + 2)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_cancdays(pkg)
                                '    End If
                                'End If
                                room = room + 1
                            End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If GvRow.RowIndex = row_id + 1 Then
                            If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                            Else
                                'txt = GvRow.FindControl("txt" & b + a + 1)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_pkg(pkg)
                                '    End If
                                'End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    txt.Text = arr_room(room)
                                End If

                                'txt = GvRow.FindControl("txt" & b + a + 2)
                                'If txt Is Nothing Then
                                'Else
                                '    If txt.Text <> Nothing Then
                                '        txt.Text = arr_cancdays(pkg)
                                '    End If
                                'End If
                                room = room + 1
                            End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("HeaderInfo1new.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnselling_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselling.Click
        Dim strentryQuery As String = "select 1  from othgrpmast where othgrpcode =(select option_selected  from reservation_parameters where param_id=1001)" & _
                                                              " and paxcalcreqd=1"
        SqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strentryQuery)
        '-------------------market common for both funtnlities-------------------------------
        Dim chksel As CheckBox
        Dim marketstr As String = ""
        Dim lblcode As Label

        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            lblcode = gvRow1.FindControl("lblcode")
            If chksel.Checked = True Then
                marketstr = marketstr + ";" + lblcode.Text
            End If
        Next
        If marketstr.Length > 0 Then
            marketstr = marketstr.Substring(1, marketstr.Length - 1)
        End If
        '---------------------------------------------
        If SqlReader.HasRows = True Then
            'to pax slab sellling pages

            ViewState.Add("TrfpricelistRefCode2", txtPlcCode.Value)
            Response.Redirect("TrfSellingRatePaxSlab.aspx?State=" & CType(ViewState("TrfpricelistState2"), String) &
                              "&RefCode=" & CType(ViewState("TrfpricelistRefCode2"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
                              "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPType.Items(ddlSPType.SelectedIndex).Text &
                              "&SupplierTypeName=" & ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text & "&Market=" & marketstr &
                              "&SuppierAgent=" & ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text &
                              "&SupplierAgentName=" & ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text &
                              "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                              "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                              "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text &
                              "&transfertype=" & ddlServerType.SelectedIndex, False)


        Else
            'to normal seling pages

            ViewState.Add("TrfpricelistRefCode2", txtPlcCode.Value)
            'Response.Redirect("HederRsellingcode1new.aspx?State=" & CType(ViewState("HeaderinfoState1"), String) & "&RefCode=" & CType(ViewState("HeaderinfoRefCode1"), String) & "&supplier=" & ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text & "&suppliername=" & ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text & "&SupplierType=" & ddlSPTypeCD.Items(ddlSPTypeCD.SelectedIndex).Text & "&SupplierTypeName=" & ddlSPTypeNM.Items(ddlSPTypeNM.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text & "&SupplierAgentName=" & ddlSuppierAgentNM.Items(ddlSuppierAgentNM.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCD.Value & "&CurrencyName=" & ddlCurrencyNM.Value & "&SubSeasonCode=" & ddlSubSeas.Items(ddlSubSeas.SelectedIndex).Text & "&SubSeasonName=" & ddlSubSeasNM.Items(ddlSubSeasNM.SelectedIndex).Text & "&RevisionDate=" & dpRevsiondate.Text & "&PListCode=" & txtBlockCode.Value & "&hdnpricelist=" & hdnpricelist.Value & "&pricelist=" & ddlPriceList.SelectedValue & "&week1=" & week1 & "&week2=" & week2 & "&manual=" & manual & "&promotionname=" & ddlPromotion.Items(ddlPromotion.SelectedIndex).Text & "&promotioncode=" & txtPromotionCode.Text, False)


            Response.Redirect("TrfPricelistSellingRates.aspx?State=" & CType(ViewState("TrfpricelistState2"), String) &
                              "&RefCode=" & CType(ViewState("TrfpricelistRefCode2"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
                              "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPType.Items(ddlSPType.SelectedIndex).Text &
                              "&SupplierTypeName=" & ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text & "&Market=" & marketstr &
                              "&SuppierAgent=" & ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text &
                              "&SupplierAgentName=" & ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text &
                              "&CurrencyCode=" & txtCurrCode.Text & "&CurrencyName=" & txtCurrName.Text &
                              "&SubSeasonCode=" & ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text &
                              "&SubSeasonName=" & ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text &
                              "&transfertype=" & ddlServerType.SelectedIndex, False)



            'Response.Redirect("TrfPricelistSellingRates.aspx?State=New&RefCode=TRPL/000015&supplier=T-000002&suppliername=FADI ECRS" & _
            '                  "&SupplierType=Transfers&SupplierTypeName=Transfers&Market=CIS;&SuppierAgent=WONINF&SupplierAgentName=" & _
            '                  "&CurrencyCode=AED&CurrencyName=DIRHAM&SubSeasonCode=ALL SEASONS&SubSeasonName=ALL", False)

        End If
    End Sub
End Class



