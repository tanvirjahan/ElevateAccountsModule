#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class TransportModule_OthPriceList2
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
    Dim otypecode1, otypecode2 As String
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
            txtbox.Attributes.Add("onkeypress", "return checkNumber()")

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
            Dim strqry As String = ""
            Dim strOption As String = ""
            Dim strtitle As String = ""
            Dim strSPType As String = ""

            If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then

                strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthPListFilter"))
                strqry = "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021)"
                Session("plisttype") = strOption
            ElseIf Session("OthPListFilter") = "OTH" Then
                strtitle = "Other Service "
                strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                    " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027,1028)) order by othgrpcode"
            End If

            If Request.QueryString("State") = "New" Then
                Page.Title = Page.Title + " " + "New " + strtitle + " Price List"
                lblHeading.Text = "Add New " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "Copy" Then
                Page.Title = Page.Title + " " + "Copy " + strtitle + " Price List"
                lblHeading.Text = "Copy " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "Edit" Then
                Page.Title = Page.Title + " " + "Edit " + strtitle + " Price List"
                lblHeading.Text = "Edit " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "View" Then
                Page.Title = Page.Title + " " + "View " + strtitle + " Price List"
                lblHeading.Text = "View " + strtitle + " Price List"
            ElseIf Request.QueryString("State") = "Delete" Then
                Page.Title = Page.Title + " " + "Delete " + strtitle + " Price List"
                lblHeading.Text = "Delete " + strtitle + " Price List"
            End If


            Dim s As String = ""
            ViewState.Add("OthpricelistState2", Request.QueryString("State"))
            ViewState.Add("OthpricelistRefCode2", Request.QueryString("RefCode"))
            txtconnection.Value = Session("dbconnectionName")
            If IsPostBack = False Then
                Dim SPType As String = ""
                Session("PlistSaved") = False
                If Session("OthPListFilter") <> "OTH" Then
                    SPType = "select excsellcode,excsellname from excsellmast where active=1"
                Else
                    SPType = "select excsellcode,excsellname from excsellmast where active=1"

                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", "select excsellname,excsellcode from excsellmast where active=1", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCd, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupNm, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpcode", True)
                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCd, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupNm, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)

                'check
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "airportbordercode", "airportbordername", "select airportbordercode,airportbordername from airportbordersmaster where active=1 order by airportbordercode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "airportbordername", "airportbordercode", "select airportbordername,airportbordercode from airportbordersmaster where active=1 order by airportbordername", True)
              
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

                ddlGroupCd.SelectedIndex = 0
                ddlGroupNm.SelectedIndex = 0

                If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Edit" Or ViewState("OthpricelistState2") = "Copy" Then

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save price list?')==false)return false;")

                    'End If
                    Dim obj As New EncryptionDecryption
                    gv_SearchResult.Visible = True
                    BtnClearFormula.Visible = True



                    If ViewState("OthpricelistState2") = "Edit" Then
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

                    If Request.QueryString("exccode") <> Nothing And Request.QueryString("excname") <> Nothing Then


                        s = ""
                        s = Request.QueryString("exccode")
                        Dim aircd As String = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        ddlexcname.Value = aircd
                        s = ""
                        s = Request.QueryString("excname")
                        Dim airgp As String = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        ddlexccode.Value = airgp
                    End If

                   

                    If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("GroupCode")
                        Dim grdcd As String = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        ddlGroupNm.Value = grdcd
                        s = ""
                        s = Request.QueryString("GroupName")
                        Dim grpnm As String = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        ddlGroupCd.Value = grpnm
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

                    If ViewState("OthpricelistState2") <> "Copy" Then
                        If Request.QueryString("approved") <> Nothing Then
                            If Request.QueryString("approved") = "1" Then
                                chkapprove.Checked = True
                            ElseIf Request.QueryString("approved") = "0" Then
                                chkapprove.Checked = False
                            End If
                        End If
                    End If



                ElseIf ViewState("OthpricelistState2") = "View" Then
                    'lblHeading.Text = "View " + strtitle + " Price List"
                    ShowRecord(ViewState("OthpricelistRefCode2"))
                    ShowDates(CType(ViewState("TrfpricelistRefCode2"), String))


                ElseIf ViewState("OthpricelistState2") = "Delete" Then
                    'lblHeading.Text = "Delete " + strtitle + " Price List"
                    btnSave.Text = "Delete"
                    ShowRecord(ViewState("OthpricelistRefCode2"))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete  price list?')==false)return false;")
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
                dt = Session("GV_OthPLData")


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
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

                ViewState.Add("OthpricelistState2", Request.QueryString("State"))
                ViewState.Add("OthpricelistRefCode2", Request.QueryString("RefCode"))

                fillDategrd(grdDates, True)

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

                If ViewState("OthpricelistState2") = "View" Then
                    lblHeading.Text = "View Excursion Price List"
                    ShowRecord(ViewState("OthpricelistRefCode2"))
                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                End If
                If ViewState("OthpricelistState2") = "Edit" Then
                    lblHeading.Text = "Edit Excursion Price List"

                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                End If
                If ViewState("OthpricelistState2") = "Delete" Then
                    lblHeading.Text = "Delete Excursion Price List"
                    ShowRecord(ViewState("TrfpricelistRefCode2"))
                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                End If

                If ViewState("OthpricelistState2") = "Copy" Then
                    ShowDates(CType(ViewState("OthpricelistRefCode2"), String))
                End If


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
                objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from excplist_header Where eplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("eplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("eplistcode")
                    End If

                    If IsDBNull(mySqlReader("esellcode")) = False Then

                        ddlSPTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excplist_header", "esellcode", "eplistcode", mySqlReader("eplistcode"))
                        ddlSPType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excsellmast", "excsellname", "excsellcode", ddlSPTypeName.Value)


                    End If

                    If IsDBNull(mySqlReader("gpcode")) = False Then

                        ddlGroupNm.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excplist_header", "gpcode", "eplistcode", mySqlReader("eplistcode"))
                        ddlGroupCd.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", ddlGroupNm.Value)


                    End If


                    'If IsDBNull(mySqlReader("exccode")) = False Then
                    '    'check
                    '    'ddlexcname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excsellmast", "airportcode", "tplistcode", mySqlReader("eplistcode"))
                    '    'ddlexccode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "airportbordersmaster", "airportbordername", "airportbordercode", ddlexcname.Value)



                    'End If

                   



                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If

                    If mySqlReader("approved") = "1" Then
                        chkapprove.Checked = True

                    End If


                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

#Region " Private Sub ShowDates(ByVal RefCode As String)"
    Private Sub ShowDates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from excplist_dates Where eplistcode='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        btnselling.Visible = False
        If ViewState("OthpricelistState2") = "New" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True
            ddlGroupCd.Disabled = True
            ddlGroupnm.Disabled = True
            ddlexccode.Disabled = True
            ddlexcname.Disabled = True
            txtRemark.Disabled = False
            ChkActive.Disabled = True
            txtCurrCode.Enabled = False
            txtCurrCode.Enabled = False
            'ddlsectorcd.Disabled = True
            'ddlsectornm.Disabled = True

        ElseIf ViewState("OthpricelistState2") = "Edit" Or ViewState("OthpricelistState2") = "Copy" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True
            ddlGroupCd.Disabled = True
            ddlGroupnm.Disabled = True
            ddlexccode.Disabled = True
            ddlexcname.Disabled = True
            txtRemark.Disabled = False
            ChkActive.Disabled = True
            txtCurrCode.Enabled = False
            txtCurrCode.Enabled = False
            'chkshf.Disabled = True
            'ddlsectorcd.Disabled = True
            'ddlsectornm.Disabled = True
            txtCurrName.Enabled = False

           
        ElseIf ViewState("OthpricelistState2") = "Delete" Or ViewState("OthpricelistState2") = "View" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True
            ddlGroupCd.Disabled = True
            ddlGroupnm.Disabled = True
            ddlexccode.Disabled = True
            ddlexcname.Disabled = True
            txtRemark.Disabled = True
            ChkActive.Disabled = True
            txtCurrCode.Enabled = False

            gv_SearchResult.Enabled = False
            grdDates.Enabled = False
            btnAddLines.Visible = False
            BtnClearFormula.Visible = False
            btnSave.Visible = False
            chkapprove.Enabled = False
            'chkshf.Disabled = True
            btnselling.Visible = False

            'chkshf.Disabled = True
            'ddlsectorcd.Disabled = True
            'ddlsectornm.Disabled = True
            txtCurrName.Enabled = False
        End If
        If ViewState("OthpricelistState2") = "Delete" Then
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
            If ViewState("OthpricelistState2") = "View" Or ViewState("OthpricelistState2") = "Delete" Then
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
            'strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM othcatmast  WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text & "')and othcatmast.active=1 "
            strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM othcatmast  WHERE othcatmast.othgrpcode in (select option_selected from reservation_parameters where param_id  = '" & Session("OthPListFilter") & "')and othcatmast.active=1 "


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
            strSqlQry = "SELECT  distinct dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode,othcatmast.grporder FROM othcatmast  WHERE othcatmast.othgrpcode in (select option_selected from reservation_parameters where param_id = '" & Session("OthPListFilter") & "')  and othcatmast.active=1 Order by othcatmast.grporder"

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
            dt.Columns.Add(New DataColumn("Pkg", GetType(String)))

            'Session("GV_HotelData") = dt
            Session("GV_OthPLData") = dt


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            'strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM  othtypmast  WHERE (othtypmast.othgrpcode = '" & ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text & "') and othtypmast.active=1"
            strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM  othtypmast inner join othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode   WHERE (othgrpmast.othgrpcode = '" & ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text & "') and othtypmast.active=1"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)



            If cnt = 0 Then
                Session.Add("Anyrow", "False")
            Else
                Session.Add("Anyrow", "True")
            End If


            Dim arr_rnkorder(cnt + 1) As String
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            'strSqlQry = "SELECT distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode,rankorder FROM othtypmast inner join othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode  WHERE (othgrpmast.othgrpcode = '" & ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text & "') and othtypmast.active=1 order by  rankorder"
            strSqlQry = "SELECT distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode,rankorder FROM othtypmast inner join othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode  WHERE (othgrpmast.othgrpcode = '" & ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text & "') and othtypmast.active=1 order by   othtypmast.othtypname"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr_rnkorder(k) = mySqlReader("rankorder")
                arr_rows(k) = mySqlReader("othtypcode")
                arr_rname(k) = mySqlReader("othtypname")
                k = k + 1
            End While


            Session("GV_OthPLData") = dt
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
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                        txt.CssClass = "field_input TextRight"
                        txt.Width = 60
                    Next
                    m = i
                Else
                    k = 0
                    For i = n To (m + n) - 1
                        txt = gvrow.FindControl("txt" & i)
                        Numbers(txt)
                        txt.CssClass = "field_input TextRight"
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

                If heading(header) = "Pkg" Then
                    txt.Visible = False
                End If
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
                            If heading(j) = "Pkg" Then
                                txt = gvrow.FindControl("txt" & b + a + 2)
                                txt.Visible = False
                            End If
                        Else

                            txt = gvrow.FindControl("txt" & b + a + 2)
                            'NumbersDateInt(txt)
                            NumbersInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60

                        End If
                    Next

                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then

                            If heading(k) = "Pkg" Then
                                txt = gvrow.FindControl("txt" & b + a + 2)
                                txt.Visible = False
                            End If

                        Else

                            txt = gvrow.FindControl("txt" & b + a + 2)
                            NumbersInt(txt) 'NumbersDateInt(txt)
                            txt.CssClass = "field_input"
                            txt.Width = 60

                            k = k + 1
                        End If
                    Next
                End If
                b = j
                n = j
            Next

            If ViewState("OthpricelistState2") <> "New" Then
                ShowDynamicGrid(ViewState("OthpricelistRefCode2"))
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Private Sub ShowDynamicGrid()"

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
                    If heading(header) = "pkg" Then
                        txt1.Visible = False
                    End If
                Next
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try





            StrQry = "select distinct excplist_detail.exccode from excplist_detail where excplist_detail.eplistcode = '" & RefCode & "'"

            Dim txt As TextBox
            Dim headerlabel As New TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()


            For Each GVROW In gv_SearchResult.Rows


                StrQryTemp = "select excplist_detail.eplistcode  ,excplist_detail.exccode,excplist_detail.othgrpcode,excplist_detail.adttype, " & _
  " excplist_detail.price,excplist_detail.pkg from  excplist_detail  " & _
  " where   " & _
  " excplist_detail.eplistcode= '" & RefCode & "' and exccode = '" & CType(Replace(GVROW.Cells(2).Text, "amp;", ""), String) & "'"

                myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCmd = New SqlCommand(StrQryTemp, myConn)
                myReader = myCmd.ExecuteReader
                If myReader.HasRows Then
                    While myReader.Read
                        othcatcode = myReader("adttype")

                        If IsDBNull(myReader("price")) = False Then
                            value = myReader("price")
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
                                                If IsDBNull(myReader("pkg")) = False Then
                                                    txt.Text = myReader("pkg")
                                                Else
                                                    txt.Text = 0
                                                End If
                                            End If
                                            GoTo go1
                                        End If
                                    End If
                                Next
                            End If

                        Next
go1:                End While
                End If
                clsDBConnect.dbConnectionClose(myConn)
                clsDBConnect.dbCommandClose(myCmd)
                clsDBConnect.dbReaderClose(myReader)
                ' End If
            Next
            '    End While
            'End If


            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & " ');", True)
        End Try

    End Sub
#End Region

#Region "Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClearFormula.Click
        Dim obj As New EncryptionDecryption
        Try
            clearmygrid()

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All Prices Are Cleared.');", True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("sessionRemark") = ""
        'Session("GV_HotelData") = ""
        Session("GV_OthPLData") = ""
        ViewState("OthpricelistRefCode2") = ""
        ViewState("OthpricelistState2") = ""
        'Response.Redirect("OtherServicesCostPriceListSearch.aspx", False)

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Private Function GetNoGenName(ByVal prm_type As String) As String
        GetNoGenName = ""
        Dim strOption As String = ""

        If prm_type <> "OTH" Then
            strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", prm_type)
            Select Case strOption
                Case "CAR RENTAL"
                    GetNoGenName = "CARPLIST"
                    Exit Function
                Case "TRFPLIST"
                    GetNoGenName = "PLT"
                    Exit Function
                Case "VISA"
                    GetNoGenName = "VISAPLIST"
                    Exit Function
                Case "TRFS"
                    GetNoGenName = "TRFPLIST"
                    Exit Function
                Case "EXC"
                    GetNoGenName = "EXCPLIST"
                    Exit Function
                Case "MEALS"
                    GetNoGenName = "MEALSPLIST"
                    Exit Function
                Case "GUIDES"
                    GetNoGenName = "GUIDEPLIST"
                    Exit Function
                Case "ENTRANCE"
                    GetNoGenName = "ENTRPLIST"
                    Exit Function
                Case "JEEPWADI"
                    GetNoGenName = "JEEPWPLIST"
                    Exit Function
                Case "HFEES"
                    GetNoGenName = "HFEESPLIST"
                    Exit Function
                Case "AIRPORTMA"
                    GetNoGenName = "AIRMAPLIST"
                    Exit Function
            End Select
        Else
            GetNoGenName = "OTHPLIST"

        End If
    End Function

#Region " Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'If Not Session("PlistSaved") = True Then ''this variable because response.redirect is causing a postback and saving twice

            Dim GvRow As GridViewRow
            If Page.IsValid = True Then
                If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Edit" Or ViewState("OthpricelistState2") = "Copy" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If CheckDecimalInGrid() = False Then
                        Exit Sub
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Copy" Then
                        Dim optionval As String
                        Dim optionName As String

                        optionName = "TRFPLIST"
                        optionval = objUtils.GetAutoDocNo(optionName, mySqlConn, sqlTrans)
                        txtPlcCode.Value = optionval.Trim
                        mySqlCmd = New SqlCommand("sp_add_excPListh", mySqlConn, sqlTrans)
                    ElseIf ViewState("OthpricelistState2") = "Edit" Then
                        'Inserting Into Logs
                        mySqlCmd = New SqlCommand("sp_excursionpriceslist_logs", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("sp_mod_excplisthnew", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcd", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)


                    mySqlCmd.Parameters.Add(New SqlParameter("@esellcd", SqlDbType.VarChar, 20)).Value = CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@gpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text.Trim, String)


                    If CType(txtCurrCode.Text.Trim, String) = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcd", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcd", SqlDbType.VarChar, 20)).Value = CType(txtCurrCode.Text.Trim, String)
                    End If

                    '**************************** Setting the MaX DATE and Min Date fromt the GRID CONTROL ***********************
                    Call SetDate()

                    mySqlCmd.Parameters.Add(New SqlParameter("@frmdt", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(ViewState("TempFrmDt"))
                    mySqlCmd.Parameters.Add(New SqlParameter("@todt", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(ViewState("TempToDt"))


                    'If CType(ddlexccode.Items(ddlexccode.SelectedIndex).Text.Trim, String) = "[Select]" Then

                    '    mySqlCmd.Parameters.Add(New SqlParameter("@airportcd", SqlDbType.VarChar, 20)).Value = DBNull.Value

                    'Else
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@airportcd", SqlDbType.VarChar, 20)).Value = CType(ddlexccode.Items(ddlexccode.SelectedIndex).Text.Trim, String)
                    'End If



                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Value.Trim, String)

                    If ViewState("OthpricelistState2") = "New" Or ViewState("OthpricelistState2") = "Copy" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If




                    If chkapprove.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0
                    End If



                    '@chkMarkUp

                    mySqlCmd.ExecuteNonQuery()
                    '-----------------------------------------------------------
                    '----------------------------------- Deleting Data From Dates Table
                    mySqlCmd = New SqlCommand("sp_del_excplisth_dates", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    '----------------------------------- Inserting Data To Dates Table
                    For Each GvRow In grdDates.Rows
                        dpFDate = GvRow.FindControl("txtfromDate")
                        dpTDate = GvRow.FindControl("txtToDate")

                        If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_excplisth_dates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)

                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    '----- update the header table with the min max date of dates table 
                    mySqlCmd = New SqlCommand("sp_upd_min_max_dates_exc", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                    'Dim ddlFromula As DropDownList
                    '--------------------------------------------- Inserting values to detail cost table--------

                    mySqlCmd = New SqlCommand("sp_del_excplistCostd", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
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
                    Dim a As Long = cnt - 10
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

                    j = 0
                    n = 0
                    'm = 0
                    b = 0
                    'chksel = gvRow1.FindControl("chkSelect")

                    Dim m As Long = 0
                    For Each GvRow In gv_SearchResult.Rows
                        If n = 0 Then
                            For j = 0 To cnt - 5
                                txt = GvRow.FindControl("txt" & j)
                                If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                                ElseIf txt.Text <> "" Then
                                    mySqlCmd = New SqlCommand("sp_add_excplistCostd", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@exccode", SqlDbType.VarChar, 20)).Value = CType(Replace(GvRow.Cells(2).Text, "amp;", ""), String) 'CType(GvRow.Cells(2).Text, String)

                                    If CType(ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = Session("plisttype") 'CType(ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text.Trim, String)
                                    End If
                                    mySqlCmd.Parameters.Add(New SqlParameter("@adttype", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)
                                    'txt = GvRow.FindControl("txt" & j)
                                    If txt.Text = "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
                                    End If
                                    'pkg
                                    mySqlCmd.Parameters.Add(New SqlParameter("@pkg", SqlDbType.Int)).Value = 1

                                    mySqlCmd.ExecuteNonQuery()
                                End If
                            Next
                            m = j
                        Else
                            k = 0
                            For j = n To (m + n) - 1
                                txt = GvRow.FindControl("txt" & j)
                                If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
                                ElseIf txt.Text <> "" Then
                                    mySqlCmd = New SqlCommand("sp_add_excplistCostd", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@exccode", SqlDbType.VarChar, 20)).Value = CType(Replace(GvRow.Cells(2).Text, "amp;", ""), String) 'CType(GvRow.Cells(2).Text, String)
                                    If CType(ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text.Trim, String) = "[Select]" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = Session("plisttype") 'CType(ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text.Trim, String)
                                    End If

                                    mySqlCmd.Parameters.Add(New SqlParameter("@adttype", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)
                                    'txt = GvRow.FindControl("txt" & j)
                                    If txt.Text = "" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
                                    End If
                                    'pkg
                                    mySqlCmd.Parameters.Add(New SqlParameter("@pkg", SqlDbType.Int)).Value = 1


                                    mySqlCmd.ExecuteNonQuery()

                                End If
                                k = k + 1
                            Next
                        End If
                        b = j
                        n = j
                    Next

                    '---------------------------------------------End of save/edit/copy------------------------
                ElseIf ViewState("OthpricelistState2") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    'SQL  Trans start

                    'Inserting Into Logs
                    mySqlCmd = New SqlCommand("sp_excursionpriceslist_logs", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    'first deleting all detail tabl values and thn deleting main table values
                    mySqlCmd = New SqlCommand("sp_del_excplisth_dates", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_excplistCostd", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    'sp_del_Trfplisth

                    'delete of main tbl to be written
                    mySqlCmd = New SqlCommand("sp_del_excplisth", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                End If

            End If
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('OthPriceListWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region "Private Function SetDate()"
    Public Sub SetDate()
        Try
            Dim gvRow As GridViewRow
            Dim flgdt As Boolean = False
            Dim tempFrmDt As String = ""
            Dim tempToDt As String = ""
            ViewState("TempFrmDt") = ""
            ViewState("TempToDt") = ""
            For Each gvRow In grdDates.Rows
                dpFDate = gvRow.FindControl("txtfromDate")
                dpTDate = gvRow.FindControl("txttoDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    If flgdt = False Then
                        'dpFromdate.txtDate.Text = dpFDate.txtDate.Text
                        tempFrmDt = dpFDate.Text
                        tempToDt = dpTDate.Text
                    Else
                        If CType(dpFDate.Text, Date) < CType(tempFrmDt, Date) Then
                            tempFrmDt = CType(dpFDate.Text, Date)
                        End If

                        If CType(dpTDate.Text, Date) > CType(tempToDt, Date) Then
                            tempToDt = CType(dpTDate.Text, Date)
                        End If
                    End If
                    flgdt = True
                End If
            Next
            ViewState("TempFrmDt") = tempFrmDt
            ViewState("TempToDt") = tempToDt


        Catch ex As Exception
            objUtils.WritErrorLog("PriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Private Function ValidatePage() As Boolean
        Dim j As Long = 1
        Dim txt As TextBox, txtnights As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

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

        If Session("anyrow") = "False" Then
            ValidatePage = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Excursion Pricelist.');", True)
            Exit Function
        End If

        Try
            For header = 0 To cnt - 5
                heading(header) = FindHeaderTextbox("txtHead" & header, "")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

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
            Dim cntN As Integer = 0
            Dim cntJ As Integer = 0

            Dim chksel As CheckBox
            Dim flg As Boolean
            flg = False

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

            Next

            'vij 
            If checkmygrid() = False Then
                ValidatePage = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast One price Should be entered.');", True)
                Exit Function
            End If

            '-----------------------------------------------------------------------***

            If flgdt = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Dates grid should not be blank.');", True)
                SetFocus(grdDates)
                ValidatePage = False
                Exit Function
            End If
            '------------------------------------------------------------------------------------
            'date checking 
            If mycheckdates() = False Then
                ValidatePage = False

                Exit Function
            End If

            '''''''''''''*date checking 



            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

            ''''''''''' *************** Grdid should have at least one value 


            ''''' ********************* pkg Cant blank 


            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ValidatePage = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthPriceList2','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        Dim cnt As Integer
        If ViewState("OthpricelistState2") = "New" Then
            cnt = 7
        Else
            If ViewState("OthpricelistRefCode2") <> Nothing Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                strSqlQry = "select count(*) from excplist_dates where eplistcode='" + ViewState("OthpricelistRefCode2") + "'"
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
            objUtils.WritErrorLog("OthPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    txt.Text = arr_room(room)
                                End If


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

    Private Function clearmygrid()
        Dim j As Integer
        Dim GvRow As GridViewRow
        Dim cnt As Long

        Dim row_id As Long
        Dim k As Integer
        Dim txt As TextBox
        Dim header As Long = 0
        cnt = gv_SearchResult.Columns.Count
        Dim heading(cnt + 1) As String
        Try
            For Header = 0 To cnt - 6
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & Header)
                heading(Header) = txt.Text
            Next

            For Each GvRow In gv_SearchResult.Rows
                For j = 0 To cnt - 5

                    If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
                    Else

                        txt = GvRow.FindControl("txt" & j + k)
                        If txt Is Nothing Then
                        Else
                            txt.Text = ""
                        End If

                    End If

                Next
                k = k + (cnt - 4)
            Next

        Catch ex As Exception
            objUtils.WritErrorLog("HeaderInfo1new.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function


    Private Function checkmygrid() As Boolean
        Dim j As Integer
        Dim GvRow As GridViewRow
        Dim cnt As Long

        Dim row_id As Long
        Dim k As Integer
        Dim txt As TextBox
        Dim header As Long = 0
        cnt = gv_SearchResult.Columns.Count
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 6
                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next

            For Each GvRow In gv_SearchResult.Rows
                For j = 0 To cnt - 5

                    If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Or heading(j) Is Nothing Then
                    Else

                        txt = GvRow.FindControl("txt" & j + k)
                        If txt.Text = "" Then
                        Else
                            checkmygrid = True
                            Exit Function
                        End If

                    End If

                Next
                k = k + cnt - 4
            Next

        Catch ex As Exception
            objUtils.WritErrorLog("HeaderInfo1new.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function

    Private Function mycheckdates() As Boolean
        Dim j As Integer
        Dim GvRow As GridViewRow
        Dim cnt As Long
        Dim p As Integer
        Dim row_id As Long
        Dim k As Integer
        Dim txt As TextBox
        Dim txt1 As TextBox
        Dim header As Long = 0
        cnt = grdDates.Rows.Count
        Dim heading(cnt + 1) As String
        Dim AllowFlg As Integer
        Dim ErrMsg As String

        Try

            For Each GvRow In grdDates.Rows
                dpFDate = GvRow.FindControl("txtfromDate")
                dpTDate = GvRow.FindControl("txtToDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    mySqlCmd = New SqlCommand("sp_chk_dates_exc", mySqlConn, sqlTrans)

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    If CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text.Trim, String) = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@esellcd", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@esellcd", SqlDbType.VarChar, 20)).Value = CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text.Trim, String)
                    End If

                    If CType(ddlexccode.Items(ddlexccode.SelectedIndex).Text.Trim, String) = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@gpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@gpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCd.Items(ddlGroupCd.SelectedIndex).Text.Trim, String)
                    End If

                   
                    mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)

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
                        mycheckdates = False
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg.Replace("'", " ") & "');", True)
                        Exit Function
                    Else
                        mycheckdates = True
                    End If

                End If

            Next

        Catch ex As Exception

        End Try


    End Function
    

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
            Else

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gv_SearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_SearchResult.SelectedIndexChanged

    End Sub

    Protected Sub chkapprove_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkapprove.CheckedChanged

    End Sub

    Protected Sub txtCurrCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCurrCode.TextChanged

    End Sub

    Protected Sub BtnClearFormula_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClearFormula.Click

    End Sub
End Class
