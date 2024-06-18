
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

Partial Class OtherServicesPriceList1
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

        End Try
    End Sub
#End Region
#Region "HTMLNumbers"
    Public Sub HTMLNumbers(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "Characters"
    Public Sub Characters(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
        Catch ex As Exception

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
            Dim s As String = ""
            ViewState.Add("OtherServpricelist1State", Request.QueryString("State"))
            ViewState.Add("OtherServpricelist1RefCode", Request.QueryString("RefCode"))

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

            If IsPostBack = False Then
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If

                txtconnection.Value = Session("dbconnectionName")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellTypeCode, "othsellcode", "othsellname", "select othsellcode, othsellname from othsellmast where active=1 order by othsellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellTypeName, "othsellname", "othsellcode", "select othsellname,othsellcode from othsellmast where active=1 order by othsellname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)


                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If ViewState("OtherServpricelist1State") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption
                    SetFocus(ddlMarketCode)
                    lblHeading.Text = "Add New Other Services Price List"

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


                    If Request.QueryString("Acvtive") <> Nothing Then
                        If Request.QueryString("Acvtive") = "1" Then
                            ChkActive.Checked = True
                        ElseIf Request.QueryString("Acvtive") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If
                    If Request.QueryString("frmdate") <> Nothing Then
                        s = ""
                        s = Request.QueryString("frmdate")
                        dpFromdate.txtDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("todate") <> Nothing Then
                        s = ""
                        s = Request.QueryString("todate")
                        dpTodate.txtDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OtherServpricelist1State") = "Edit" Or ViewState("OtherServpricelist1State") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    SetFocus(ddlMarketCode)

                    btnGenerate.Visible = True
                    ShowRecord(ViewState("OtherServpricelist1RefCode"))
                    lblHeading.Text = "Edit Other Services Price List"
                End If
                If ViewState("OtherServpricelist1State") = "Copy" Then
                    txtPlcCode.Value = ""

                End If
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
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If ViewState("OtherServpricelist1State") = "New" Then

        ElseIf ViewState("OtherServpricelist1State") = "Edit" Or ViewState("OtherServpricelist1State") = "Copy" Then
            ddlGroupCode.Disabled = True
            ddlGroupName.Disabled = True

            'ddlMarketCode.Disabled = True
            'ddlMarketName.Disabled = True
            'txtCurrCode.Enabled = False
            'txtCurrName.Enabled = False
            'ddlSellTypeCode.Disabled = True
            'ddlSellTypeName.Disabled = True
            'ddlSubSeasCode.Disabled = True
            'ddlSubSeasName.Disabled = True
            'txtRemark.Disabled = True
            'ChkActive.Disabled = True
            'dpFromdate.txtDate.Enabled = False
            'dpTodate.txtDate.Enabled = False
        End If
    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then

                ViewState.Add("OtherServpricelist1State", Request.QueryString("State"))
                ViewState.Add("OtherServpricelist1RefCode", Request.QueryString("RefCode"))

                DisableAllControls()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
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
                    If ViewState("OtherServpricelist1State") <> "Copy" Then
                        If IsDBNull(mySqlReader("frmdate")) = False Then
                            dpFromdate.txtDate.Text = Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy")
                            Session("sessionFrmdate") = dpFromdate.txtDate.Text
                        End If
                        If IsDBNull(mySqlReader("todate")) = False Then
                            dpTodate.txtDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                            Session("sessionTodate") = dpTodate.txtDate.Text
                        End If
                    End If


                    If IsDBNull(mySqlReader("active")) = False Then
                        If mySqlReader("active") = "1" Then
                            ChkActive.Checked = True
                        ElseIf mySqlReader("active") = "0" Then
                            ChkActive.Checked = False
                        End If
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


        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region
#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As New EncryptionDecryption
        Try
            If dpFromdate.txtDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field should not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpFromdate.ClientID + "');", True)
                Exit Sub
            End If
            If dpTodate.txtDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field should not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpTodate.ClientID + "');", True)
                Exit Sub
            End If
            If dpFromdate.txtDate.Text <> "" And dpTodate.txtDate.Text <> "" Then
                If ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(dpTodate.txtDate.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should be grater than from date.');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpTodate.ClientID + "');", True)
                    Exit Sub
                End If
            End If
            '----------------------------------------------------------------
            'sp_chkopldate
            'ValidateDateRange()
            'mySqlCmd = New SqlCommand("sp_chkopldate", mySqlConn)
            'mySqlCmd.CommandText = "sp_chkopldate"
            'mySqlCmd.CommandType = CommandType.StoredProcedure
            'mySqlCmd.Connection = mySqlConn
            'mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar))
            '*********************************          **********************      ***************************
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
            Dim frmdate As String = obj.Encrypt(dpFromdate.txtDate.Text, "&%#@?,:*")
            Dim todate As String = obj.Encrypt(dpTodate.txtDate.Text, "&%#@?,:*")
            Session("sessionRemark") = txtRemark.Value
            Response.Redirect("OtherServicesPriceList2.aspx?&State=" & ViewState("OtherServpricelist1State") & "&RefCode=" & ViewState("OtherServpricelist1RefCode") & "&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&SellTypeCode=" & SellTypeCode & "&SellTypeName=" & SellTypeName & "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName & "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&approved=" & IIf(chkapprove.Checked = True, 1, 0) & "&stopweb=" & IIf(chkshowweb.Checked = True, 1, 0) & "&frmdate=" & frmdate & "&todate=" & todate, False)
            '*********************************          **********************      ***************************

        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("sessionRemark") = ""
        Session("GV_HotelData") = ""
        ViewState("OtherServpricelist1State") = ""
        ViewState("OtherServpricelist1RefCode") = ""
        'Response.Redirect("OtherServicesPriceListSearch.aspx")

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesPriceList1','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class


'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)

'ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")

'If Request.QueryString("remarks") <> Nothing Then
'End If

'DisableAllControls()
'btnGenerate.Visible = False

' ddlCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'ddlCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = mySqlReader("currcode")
'ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", mySqlReader("currcode"))

' dpFromdate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)

'dpTodate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
'If CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text.Trim, String) = "" Then
'    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'Else
'    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text.Trim, String)
'End If

'ddlCurrencyCode.Disabled = True
'ddlCurrencyName.Disabled = True

'Dim currcode As String = obj.Encrypt(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String), "&%#@?,:*")
'Dim currname As String = obj.Encrypt(CType(ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text, String), "&%#@?,:*")


'#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


'    Try


'        Dim s As String = ""
'        If IsPostBack = False Then
'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellTypeCode, "othsellcode", "othsellname", "select othsellcode, othsellname from othsellmast where active=1 order by othsellcode", True)
'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellTypeName, "othsellname", "othsellcode", "select othsellname,othsellcode from othsellmast where active=1 order by othsellname", True)

'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)

'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
'            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)


'            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")


'            If Session("State") = "New" Then
'                btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
'                Dim obj As New EncryptionDecryption
'                gv_SearchResult.Visible = True
'                SetFocus(ddlMarketCode)
'                BtnClearFormula.Visible = False
'                lblHeading.Text = "Add New Other Services Price List"
'                If Request.QueryString("MarketCode") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("MarketCode")
'                    ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("MarketName") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("MarketName")
'                    ddlMarketName.Items(ddlMarketName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("SellTypeCode") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("SellTypeCode")
'                    ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("SellTypeName") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("SellTypeName")
'                    ddlSellTypeName.Items(ddlSellTypeName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If


'                txtRemark.Value = CType(Session("sessionRemark"), String)


'                If Request.QueryString("PlcCode") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("PlcCode")
'                    txtPlcCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("GroupCode") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("GroupCode")
'                    ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("GroupName") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("GroupName")
'                    ddlGroupName.Items(ddlGroupName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("currcode") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("currcode")
'                    txtCurrCode.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")

'                End If
'                If Request.QueryString("currname") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("currname")
'                    txtCurrName.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If

'                If Request.QueryString("SubSeasCode") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("SubSeasCode")
'                    ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If

'                If Request.QueryString("SubSeasName") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("SubSeasName")
'                    ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("Acvtive") <> Nothing Then
'                    If Request.QueryString("Acvtive") = "1" Then
'                        ChkActive.Checked = True
'                    ElseIf Request.QueryString("Acvtive") = "0" Then
'                        ChkActive.Checked = False
'                    End If
'                End If
'                If Request.QueryString("frmdate") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("frmdate")
'                    dpFromdate.txtDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                If Request.QueryString("todate") <> Nothing Then
'                    s = ""
'                    s = Request.QueryString("todate")
'                    dpTodate.txtDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                End If
'                btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
'            ElseIf Session("State") = "Edit" Or Session("State") = "Copy" Then
'                btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
'                SetFocus(ddlMarketCode)

'                btnGenerate.Visible = True
'                BtnClearFormula.Visible = False
'                ShowRecord(Session("RefCode"))
'                lblHeading.Text = "Edit Other Services Price List"
'            ElseIf Session("State") = "View" Then
'                'btnGenerate.Visible = False
'                'SetFocus(ddlMarketCode)
'                'BtnClearFormula.Visible = False
'                'ShowRecord(Session("RefCode"))
'                'DisableAllControls()
'                'lblHeading.Text = "View Other Services Price List"
'            ElseIf Session("State") = "Delete" Then
'                'btnSave.Visible = True
'                'btnGenerate.Visible = False
'                'btnGenerate.Attributes.Add("onclick", "return FormValidation('Delete')")
'                'lblHeading.Text = "Delete Other Services Price List"
'                ''btnSave.Text = "Delete"
'                'ShowRecord(Session("RefCode"))
'                'BtnClearFormula.Visible = False
'                'DisableAllControls()
'                'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete other services price list?')==false)return false;")
'            End If
'            If Session("State") = "Copy" Then
'                txtPlcCode.Value = ""
'            End If
'            BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear prices?')==false)return false;")
'            createdatatable()

'            'fill controls from previous form
'            ' Now  Bind Column Dynamically 
'            dt = Session("GV_HotelData")
'            Dim fld2 As String = ""
'            Dim col As DataColumn
'            For Each col In dt.Columns
'                If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
'                    Dim bfield As New TemplateField
'                    'Call Function
'                    bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
'                    bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
'                    gv_SearchResult.Columns.Add(bfield)

'                End If

'            Next
'            gv_SearchResult.Visible = True
'            gv_SearchResult.DataSource = dt
'            'InstantiateIn Grid View
'            gv_SearchResult.DataBind()


'            Dim typ As Type
'            typ = GetType(DropDownList)

'            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
'                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

'                ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'                ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'                ddlSellTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'                ddlSellTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'                ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'                ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'                ddlSubSeasCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'                ddlSubSeasName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'            End If
'            TextLock(txtCurrCode)
'            TextLock(txtCurrName)

'        Else
'            dt = Session("GV_HotelData")
'            ' createdatatable()

'            'fill controls from previous form
'            ' Now  Bind Column Dynamically 

'            Dim fld2 As String = ""
'            Dim col As DataColumn
'            For Each col In dt.Columns
'                If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
'                    Dim bfield As New TemplateField
'                    'Call Function
'                    bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
'                    bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
'                    gv_SearchResult.Columns.Add(bfield)

'                End If

'            Next
'            gv_SearchResult.Visible = True
'            gv_SearchResult.DataSource = dt
'            'InstantiateIn Grid View
'            gv_SearchResult.DataBind()
'            BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Clease formula?')==false)return false;")
'            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

'        End If
'    Catch ex As Exception
'        objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'    End Try
'End Sub
'#End Region


'ElseIf Session("State") = "View" Then
''btnGenerate.Visible = False
''SetFocus(ddlMarketCode)
''BtnClearFormula.Visible = False
''ShowRecord(Session("RefCode"))
''DisableAllControls()
''lblHeading.Text = "View Other Services Price List"
'                ElseIf Session("State") = "Delete" Then
'btnSave.Visible = True
'btnGenerate.Visible = False
'btnGenerate.Attributes.Add("onclick", "return FormValidation('Delete')")
'lblHeading.Text = "Delete Other Services Price List"
''btnSave.Text = "Delete"
'ShowRecord(Session("RefCode"))
'BtnClearFormula.Visible = False
'DisableAllControls()
'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete other services price list?')==false)return false;")


'#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

'    Try
'        If IsPostBack = False Then

'            If Session("CheckGridColumn") = "Not Present" Then
'                Panel1.Visible = False
'                lblError.Visible = True
'                Panel1.Visible = False
'                lblError.Text = "Rates are not present of this selected Group Code. Click Clear Prices for Changing Group."
'            Else
'                Panel1.Visible = True
'                createdatarows()
'                lblError.Visible = False
'            End If

'            'If Session("State") <> "New" Then
'            '    ShowDynamicGrid(Session("RefCode"))
'            '    gv_SearchResult.Visible = True
'            '    gv_SearchResult.Enabled = False
'            '    Panel1.Visible = True

'            'Else
'            '    lblError.Visible = False
'            '    Panel1.Visible = False
'            '    gv_SearchResult.Visible = False
'            'End If

'            lblError.Visible = False
'            Panel1.Visible = False
'            gv_SearchResult.Visible = False


'        End If
'        ' dpFromdate.txtDate.Text = Session("sessionFrmdate")
'        '/'dpTodate.txtDate.Text = Session("sessionTodate")
'    Catch ex As Exception
'        objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'    End Try



'End Sub
'#End Region



'#Region " Private Sub createdatatable()"
'    Private Sub createdatatable()
'        Try

'            cnt = 0

'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            strSqlQry = "select count(othcatcode) from  othcatmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            cnt = mySqlCmd.ExecuteScalar
'            mySqlConn.Close()
'            If cnt > 0 Then
'                Session("CheckGridColumn") = ""
'            Else
'                Session("CheckGridColumn") = "Not Present"
'            End If
'            ReDim arr(cnt + 1)
'            ReDim arrRName(cnt + 1)
'            Dim i As Long = 0

'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            strSqlQry = "select othcatcode from  othcatmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            mySqlReader = mySqlCmd.ExecuteReader()
'            While mySqlReader.Read = True
'                arr(i) = mySqlReader("othcatcode")
'                i = i + 1
'            End While
'            mySqlReader.Close()
'            mySqlConn.Close()
'            'select rmcatcode from partyrmcat where partycode='3'
'            'Here in Array store room types
'            '-------------------------------------
'            Dim tf As New TemplateField
'            dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
'            dt.Columns.Add(New DataColumn("Service Type Code", GetType(String)))
'            dt.Columns.Add(New DataColumn("Service Type Name", GetType(String)))

'            'create columns of this room types in data table
'            For i = 0 To cnt - 1
'                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
'            Next
'            dt.Columns.Add(New DataColumn("From Date", GetType(String)))
'            dt.Columns.Add(New DataColumn("To Date", GetType(String)))
'            dt.Columns.Add(New DataColumn("Pkg", GetType(String)))
'            Session("GV_HotelData") = dt



'        Catch ex As Exception
'            If mySqlConn.State = ConnectionState.Open Then
'                mySqlConn.Close()
'            End If
'            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Sub
'#End Region

'#Region "Private Sub createdatarows()"
'    Private Sub createdatarows()
'        Dim i As Long = 0
'        Dim k As Long = 0
'        Try
'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            strSqlQry = "select count(othtypcode) from othtypmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            cnt = mySqlCmd.ExecuteScalar
'            mySqlConn.Close()

'            Dim arr_rows(cnt + 1) As String
'            Dim arr_rname(cnt + 1) As String

'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            strSqlQry = "select othtypcode,othtypname from othtypmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            mySqlReader = mySqlCmd.ExecuteReader()
'            While mySqlReader.Read = True
'                arr_rows(k) = mySqlReader("othtypcode")
'                arr_rname(k) = mySqlReader("othtypname")
'                k = k + 1
'            End While
'            mySqlReader.Close()
'            mySqlConn.Close()


'            'Here add rows.....
'            'Now Get Rows From sellmast Based on SPType
'            Session("GV_HotelData") = dt
'            Dim dr As DataRow

'            Dim row As Long

'            For row = 0 To k - 1
'                dr = dt.NewRow
'                'For i = 1 To cnt - 1
'                dr("Sr No") = row + 1   ' 
'                dr("Service Type Code") = arr_rows(row)
'                dr("Service Type Name") = arr_rname(row)
'                'dr(arr(i)) = ""
'                'Next
'                dt.Rows.Add(dr)
'            Next


'            gv_SearchResult.Visible = True
'            gv_SearchResult.DataSource = dt
'            'InstantiateIn Grid View
'            gv_SearchResult.DataBind()
'        Catch ex As Exception
'            If mySqlConn.State = ConnectionState.Open Then
'                mySqlConn.Close()
'            End If
'            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'        '-======================||||||||||||||||||||||||||||||||||||||||==========================!!!!!!!!!!!!!!!
'        ''''Herer Onlu Numbers Can enterd in textboxes
'        'Dim j As Long = 1
'        'Dim txt As TextBox
'        'Dim gvrow As GridViewRow
'        'j = 0
'        'cnt = gv_SearchResult.Columns.Count
'        'Dim n As Long = 0
'        'Dim m As Long = 0
'        'i = 0
'        'k = 0
'        'Try
'        '    For Each gvrow In gv_SearchResult.Rows
'        '        If n = 0 Then
'        '            For i = 0 To cnt - 4
'        '                txt = gvrow.FindControl("txt" & i)
'        '                Numbers(txt)
'        '            Next
'        '            m = i
'        '        Else
'        '            k = 0
'        '            For i = n To (m + n) - 1
'        '                txt = gvrow.FindControl("txt" & i)
'        '                Numbers(txt)
'        '                k = k + 1
'        '            Next

'        '        End If
'        '        n = i
'        '    Next


'        'Catch ex As Exception
'        'objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        'End Try

'    End Sub
'#End Region

'#Region "Private Sub ShowDynamicGrid()"
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

'            End Try

'            StrQry = "select * from oplistd  where oplistcode='" & RefCode & "' order by olineno,othtypcode"
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
'                            k = k + 1
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


'#Region "Private Function ValidatePage() As Boolean"
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
'        Dim FrmDate As Date = ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)
'        Dim GrdFrmDate As Date
'        Dim Flag As Boolean = False
'        Try
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
'                            'txt = GvRow.FindControl("txt" & j)
'                            txt = GvRow.FindControl("txt" & b + a + 1)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates From date can not be left blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            Else
'                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < FrmDate Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the From dates  Should be Greater Than or Equal to Header From Date.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                    ValidatePage = False
'                                    Exit Function
'                                End If
'                            End If
'                            GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'                            txt = GvRow.FindControl("txt" & b + a + 2)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            Else
'                                If CType(GrdFrmDate, Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the To dates  Should be Greater Than or Equal to Grids From Date.');", True)
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
'                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < FrmDate Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the From dates  Should be Greater Than or Equal to Header From Date.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                    ValidatePage = False
'                                    Exit Function
'                                End If
'                            End If

'                            GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'                            txt = GvRow.FindControl("txt" & b + a + 2)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            Else
'                                If CType(GrdFrmDate, Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the To dates  Should be Greater Than or Equal to Grids From Date.');", True)
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
'            Response.Write(ex)
'            objUtils.WritErrorLog("OtherServicesPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Function
'#End Region
'#Region " Validate Date "
'    Public Function ValidateDateRange() As Boolean
'        Try
'            Dim gvRowDT As GridViewRow
'            Dim SqlConn1 As SqlConnection
'            For Each gvRowDT In gv_SearchResult.Rows
'                'dpFDate = gvRowDT.FindControl("FrmDate")
'                'dpTDate = gvRowDT.FindControl("ToDate")

'                'If dpFDate.txtDate.Text <> "" And dpTDate.txtDate.Text <> "" Then
'                'Dim gvRow As GridViewRow
'                Dim Alflg As Long
'                Dim ErrMsg As String

'                'For Each gvRow In gv_SearchResult.Rows
'                'chkSel = gvRow.FindControl("chkSelect")
'                'If chkSel.Checked = True Then
'                SqlConn1 = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'                'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

'                mySqlCmd = New SqlCommand("sp_chkopldate", SqlConn1)
'                mySqlCmd.CommandType = CommandType.StoredProcedure

'                mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtBlockCode.Value.Trim, String)
'                mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplier.Items(ddlSupplier.SelectedIndex).Text, String)
'                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.txtDate.Text)
'                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.txtDate.Text)
'                mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarket.Items(ddlMarket.SelectedIndex).Text, String)
'                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
'                mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = CType(ddlMarket.Items(ddlMarket.SelectedIndex).Text, String)
'                mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarket.Items(ddlMarket.SelectedIndex).Text, String)

'                Dim param1 As SqlParameter
'                Dim param2 As SqlParameter
'                param1 = New SqlParameter
'                param1.ParameterName = "@allowflg"
'                param1.Direction = ParameterDirection.Output
'                param1.DbType = DbType.Int16
'                param1.Size = 9
'                mySqlCmd.Parameters.Add(param1)
'                param2 = New SqlParameter
'                param2.ParameterName = "@errmsg"
'                param2.Direction = ParameterDirection.Output
'                param2.DbType = DbType.String
'                param2.Size = 50
'                mySqlCmd.Parameters.Add(param2)
'                mySqlAdapter = New SqlDataAdapter(mySqlCmd)
'                mySqlCmd.ExecuteNonQuery()

'                Alflg = param1.Value
'                ErrMsg = param2.Value

'                'sqlTrans.Commit()    'SQl Tarn Commit
'                'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'                'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
'                clsDBConnect.dbConnectionClose(SqlConn1)
'                'connection close
'                If Alflg = 1 And ErrMsg <> "" Then
'                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
'                    SetFocus(dpFromdate)
'                    ValidateDateRange = False
'                    Exit Function
'                End If
'                'End If
'                'Next
'                'End If

'            Next

'            ValidateDateRange = True

'        Catch ex As Exception

'        End Try
'    End Function
'#End Region

'#Region "Private Function CheckDecimalInGrid() As Boolean"
'    Private Function CheckDecimalInGrid() As Boolean
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
'        Dim arr() As String
'        Dim GvRow As GridViewRow
'        Try
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
'                                arr = txt.Text.Split(".")
'                                If arr.Length > 2 Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
'                                    CheckDecimalInGrid = False
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
'                            If txt.Text <> "" Then
'                                arr = txt.Text.Split(".")
'                                If arr.Length > 2 Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
'                                    CheckDecimalInGrid = False
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
'            CheckDecimalInGrid = True
'        Catch ex As Exception
'            Response.Write(ex)
'            objUtils.WritErrorLog("OtherServicesPriceList2.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Function
'#End Region


' createdatatable()

'fill controls from previous form
' Now  Bind Column Dynamically 
'dt = Session("GV_HotelData")
'Dim fld2 As String = ""
'Dim col As DataColumn
'For Each col In dt.Columns
'    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
'        Dim bfield As New TemplateField
'        'Call Function
'        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
'        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
'        gv_SearchResult.Columns.Add(bfield)

'    End If

'Next
'gv_SearchResult.Visible = True
'gv_SearchResult.DataSource = dt
''InstantiateIn Grid View
'gv_SearchResult.DataBind()


'dt = Session("GV_HotelData")
'' createdatatable()

''fill controls from previous form
'' Now  Bind Column Dynamically 

'Dim fld2 As String = ""
'Dim col As DataColumn
'For Each col In dt.Columns
'    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
'        Dim bfield As New TemplateField
'        'Call Function
'        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
'        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
'        gv_SearchResult.Columns.Add(bfield)

'    End If

'Next
'gv_SearchResult.Visible = True
'gv_SearchResult.DataSource = dt
''InstantiateIn Grid View
'gv_SearchResult.DataBind()
'BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Clease formula?')==false)return false;")
'btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")


' Else

'mySqlCmd.Dispose()
'mySqlReader.Close()
'If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
'#Region "Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
'    Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)
'        Try
'            gv_SearchResult.Visible = False
'            BtnClearFormula.Visible = False
'            btnGenerate.Visible = True
'            Panel1.Visible = False
'            lblError.Visible = False
'        Catch ex As Exception

'        End Try
'    End Sub
'#End Region


'#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
'    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
'        'Try
'        '    Dim GvRow As GridViewRow
'        '    If Page.IsValid = True Then
'        '        If Session("State") = "New" Or Session("State") = "Edit" Or Session("State") = "Copy" Then
'        '            '------------------------------------------------------------------------------------
'        '            '           Here Checking Validations
'        '            If dpFromdate.txtDate.Text <> "" And dpTodate.txtDate.Text <> "" Then
'        '                If ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(dpTodate.txtDate.Text) Then
'        '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should be grater than from date.');", True)
'        '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpTodate.ClientID + "');", True)
'        '                    Exit Sub
'        '                End If
'        '            End If
'        '            If ValidatePage() = False Then
'        '                Exit Sub
'        '            End If
'        '            If CheckDecimalInGrid() = False Then
'        '                Exit Sub
'        '            End If
'        '            '------------------------------------------------------------------------------------
'        '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'        '            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
'        '            If Session("State") = "New" Or Session("State") = "Copy" Then
'        '                Dim optionval As String
'        '                optionval = objUtils.GetAutoDocNo("PLISTOTH", mySqlConn, sqlTrans)
'        '                txtPlcCode.Value = optionval.Trim
'        '                mySqlCmd = New SqlCommand("sp_add_oplisth", mySqlConn, sqlTrans)
'        '            ElseIf Session("State") = "Edit" Then
'        '                mySqlCmd = New SqlCommand("sp_mod_oplisth", mySqlConn, sqlTrans)
'        '            End If
'        '            mySqlCmd.CommandType = CommandType.StoredProcedure

'        '            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'        '            If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '            Else
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
'        '            End If
'        '            If CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '            Else
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 20)).Value = CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text.Trim, String)
'        '            End If
'        '            If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '            Else
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
'        '            End If
'        '            If CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '            Else
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)
'        '            End If
'        '            If CType(txtCurrCode.Text.Trim, String) = "" Then
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '            Else
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrCode.Text.Trim, String)
'        '            End If

'        '            If dpFromdate.txtDate.Text = "" Then
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
'        '            Else
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text), Date)
'        '            End If
'        '            If dpTodate.txtDate.Text = "" Then
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
'        '            Else
'        '                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(dpTodate.txtDate.Text), Date)
'        '            End If
'        '            mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Value.Trim, String)
'        '            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
'        '            mySqlCmd.ExecuteNonQuery()


'        '            '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$


'        '            mySqlCmd = New SqlCommand("sp_del_oplistd", mySqlConn, sqlTrans)
'        '            mySqlCmd.CommandType = CommandType.StoredProcedure
'        '            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'        '            mySqlCmd.ExecuteNonQuery()

'        '            Dim j As Long = 1
'        '            Dim txt As TextBox
'        '            Dim cnt As Long
'        '            Dim srno As Long = 0
'        '            Dim hotelcategory As String = ""
'        '            j = 0

'        '            gv_SearchResult.Visible = True
'        '            cnt = gv_SearchResult.Columns.Count
'        '            Dim n As Long = 0
'        '            Dim k As Long = 0
'        '            Dim a As Long = cnt - 7
'        '            Dim b As Long = 0
'        '            Dim header As Long = 0
'        '            Dim heading(cnt + 1) As String
'        '            '----------------------------------------------------------------------------
'        '            '           Stoaring heading column values in the array
'        '            For header = 0 To cnt - 4
'        '                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
'        '                heading(header) = txt.Text
'        '            Next
'        '            '----------------------------------------------------------------------------
'        '            Dim m As Long = 0
'        '            For Each GvRow In gv_SearchResult.Rows
'        '                If n = 0 Then
'        '                    For j = 0 To cnt - 4
'        '                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
'        '                        Else
'        '                            mySqlCmd = New SqlCommand("sp_add_oplistd", mySqlConn, sqlTrans)
'        '                            mySqlCmd.CommandType = CommandType.StoredProcedure
'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@olineno", SqlDbType.Int)).Value = CType(GvRow.Cells(0).Text, Long)
'        '                            If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
'        '                            End If
'        '                            If CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)
'        '                            End If
'        '                            If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
'        '                            End If

'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)
'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)

'        '                            txt = GvRow.FindControl("txt" & j)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
'        '                            End If

'        '                            txt = GvRow.FindControl("txt" & b + a + 1)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'        '                            End If
'        '                            txt = GvRow.FindControl("txt" & b + a + 2)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'        '                            End If
'        '                            txt = GvRow.FindControl("txt" & b + a + 3)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
'        '                            End If

'        '                            mySqlCmd.ExecuteNonQuery()

'        '                        End If
'        '                    Next

'        '                    m = j
'        '                Else
'        '                    k = 0
'        '                    For j = n To (m + n) - 1
'        '                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
'        '                        Else
'        '                            mySqlCmd = New SqlCommand("sp_add_oplistd", mySqlConn, sqlTrans)
'        '                            mySqlCmd.CommandType = CommandType.StoredProcedure
'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@olineno", SqlDbType.Int)).Value = CType(GvRow.Cells(0).Text, Long)
'        '                            If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
'        '                            End If
'        '                            If CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text.Trim, String)
'        '                            End If
'        '                            If CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String) = "[Select]" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text.Trim, String)
'        '                            End If

'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)
'        '                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text, String)
'        '                            txt = GvRow.FindControl("txt" & j)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@oprice", SqlDbType.Decimal, 18, 3)).Value = CType(txt.Text.Trim, Decimal)
'        '                            End If
'        '                            txt = GvRow.FindControl("txt" & b + a + 1)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'        '                            End If
'        '                            txt = GvRow.FindControl("txt" & b + a + 2)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'        '                            End If
'        '                            txt = GvRow.FindControl("txt" & b + a + 3)
'        '                            If txt.Text = "" Then
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = DBNull.Value
'        '                            Else
'        '                                mySqlCmd.Parameters.Add(New SqlParameter("@pkgnights", SqlDbType.Int, 9)).Value = CType(txt.Text, Long)
'        '                            End If
'        '                            mySqlCmd.ExecuteNonQuery()
'        '                            k = k + 1
'        '                        End If
'        '                    Next
'        '                End If
'        '                b = j
'        '                n = j
'        '            Next

'        '        ElseIf Session("State") = "Delete" Then
'        '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'        '            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

'        '            mySqlCmd = New SqlCommand("sp_del_oplisth", mySqlConn, sqlTrans)
'        '            mySqlCmd.CommandType = CommandType.StoredProcedure
'        '            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'        '            mySqlCmd.ExecuteNonQuery()
'        '            mySqlCmd = New SqlCommand("sp_del_oplistd", mySqlConn, sqlTrans)
'        '            mySqlCmd.CommandType = CommandType.StoredProcedure
'        '            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'        '            mySqlCmd.ExecuteNonQuery()
'        '        End If
'        '        sqlTrans.Commit()    'SQl Tarn Commit
'        '        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'        '        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
'        '        clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
'        '        Session("sessionRemark") = ""
'        '        Session("GV_HotelData") = ""
'        '        Session("RefCode") = ""
'        '        Session("State") = ""
'        '        Response.Redirect("OtherServicesPriceListSearch.aspx", False)
'        '    End If
'        'Catch ex As Exception
'        '    sqlTrans.Rollback()
'        'End Try
'    End Sub
'#End Region

'If Request.QueryString("MarketCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("MarketCode")
'                      ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("MarketName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("MarketName")
'                      ddlMarketName.Items(ddlMarketName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("SellTypeCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SellTypeCode")
'                      ddlSellTypeCode.Items(ddlSellTypeCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("SellTypeName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SellTypeName")
'                      ddlSellTypeName.Items(ddlSellTypeName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("GroupCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("GroupCode")
'                      ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("GroupName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("GroupName")
'                      ddlGroupName.Items(ddlGroupName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("SubSeasCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SubSeasCode")
'                      ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If

'                  If Request.QueryString("SubSeasName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SubSeasName")
'                      ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If


'If Session("CheckGridColumn") = "Not Present" Then
'    Panel1.Visible = False
'    lblError.Visible = True
'    Panel1.Visible = False
'    lblError.Text = "Rates are not present of this selected Group Code. Click Clear Prices for Changing Group."
'Else
'    Panel1.Visible = True
'    createdatarows()
'    lblError.Visible = False
'End If

'If Session("State") <> "New" Then
'    ShowDynamicGrid(Session("RefCode"))
'    gv_SearchResult.Visible = True
'    gv_SearchResult.Enabled = False
'    Panel1.Visible = True

'Else
'    lblError.Visible = False
'    Panel1.Visible = False
'    gv_SearchResult.Visible = False
'End If

'lblError.Visible = False
'Panel1.Visible = False
'gv_SearchResult.Visible = False

' dpFromdate.txtDate.Text = Session("sessionFrmdate")
'/'dpTodate.txtDate.Text = Session("sessionTodate")