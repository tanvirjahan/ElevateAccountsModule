#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
#End Region

Partial Class ExcursionMulticost
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime

    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection

  
    Dim sqlTrans As SqlTransaction

    Dim gvRow1 As GridViewRow
    Dim myDataAdapter As SqlDataAdapter
    Dim otypecode1, otypecode2 As String

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
#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub
    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionMultiCost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionMultiCost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentListSearch(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lsAgentNames As New List(Of String)
        Dim lsCountryList As String
        Try

            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList

            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                'If HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl") IsNot Nothing Then 'changed by mohamed on 03/10/2016 - instead of selected, used all
                'lsCountryList = HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    'strSqlQry += " and a.ctrycode in (" & lsCountryList & ")" 'changed by mohamed on 03/10/2016 -'commented this line to show all the agents.
                End If
            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'lsAgentNames.Add(myDS.Tables(0).Rows(i)("agentname").ToString())
                    lsAgentNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next
            End If

            Return lsAgentNames
        Catch ex As Exception

            Return lsAgentNames
        End Try

    End Function
#End Region
#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If IsPostBack = False Then
                'Session ("OthPLFilter")
                Dim strqry As String = ""
                Dim strOption As String = ""
                Dim strtitle As String = ""
                Dim strSpType As String = ""

                Session("GV_OthPLData") = Nothing

              
                grdDates.Visible = True
                fillDategrd(grdDates, True)

            
                ViewState.Add("pricelistState", Request.QueryString("State"))
                ViewState.Add("RefCode", Request.QueryString("RefCode"))

                If ViewState("pricelistState") = "New" Then
                    Page.Title = Page.Title + " " + "New " + strtitle + " Price List"

                    btnSave.Style.Add("display", "none")
                    btncancel.Style.Add("display", "none")
                    divmarkup1.Style.Add("display", "none")
                    btnAddrow.Style.Add("display", "none")
                    btndeleterow.Style.Add("display", "none")


                    txtCurrCode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =457")
                    txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", txtCurrCode.Text)

                ElseIf ViewState("pricelistState") = "Copy" Then
                    Page.Title = Page.Title + " " + "Copy " + strtitle + "  Price List"
                ElseIf ViewState("pricelistState") = "Edit" Then
                    Page.Title = Page.Title + " " + "Edit " + strtitle + "  Price List"
                   

                ElseIf ViewState("pricelistState") = "View" Then
                    Page.Title = Page.Title + " " + "View" + strtitle + "  Price List"

                ElseIf ViewState("pricelistState") = "Delete" Then
                    Page.Title = Page.Title + " " + "Delete " + strtitle + "  Price List"

                End If

                Dim s As String = ""
             
                txtconnection.Value = Session("dbconnectionName")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othtypcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypname,othtypcode from othtypmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othtypname", True)



                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)
                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)



                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If Viewstate("pricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption

                    SetFocus(ddlSPType)

                    lblHeading.Text = "Add New " + strtitle + " Price List"

                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf Viewstate("pricelistState") = "Edit" Or Viewstate("pricelistState") = "Copy" Then
             
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")




                    lblHeading.Text = "Edit " + strtitle + " Price List"
                ElseIf Viewstate("pricelistState") = "View" Then

                ElseIf Viewstate("pricelistState") = "Delete" Then

                End If
                If ViewState("pricelistState") = "Copy" Then
                  
                    txtPlcCode.Value = ""
                End If
                DisableAllControls()
                TextLock(txtCurrCode)
                TextLock(txtCurrName)

                Numberssrvctrl(txtmarkup)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlSPType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    'ddlexccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlexcname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                    'ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
                btncancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Else
                'If Session("GV_OthPLData") Is Nothing = False Then

                '    dt = Session("GV_OthPLData")

                '    Dim fld2 As String = ""
                '    Dim col As DataColumn
                '    For Each col In dt.Columns
                '        If col.ColumnName <> "Supplier" And col.ColumnName <> "Service Type" And col.ColumnName <> "SupplierCode" Then
                '            Dim bfield As New TemplateField
                '            'Call Function


                '            bfield.HeaderTemplate = New ClassChildPolicy(ListItemType.Header, col.ColumnName, fld2)
                '            bfield.ItemTemplate = New ClassChildPolicy(ListItemType.Item, col.ColumnName, fld2)
                '            gv_ExRates.Columns.Add(bfield)



                '        End If
                '    Next

                '    gv_ExRates.Visible = True
                '    gv_ExRates.DataSource = dt
                '    'InstantiateIn Grid View
                '    gv_ExRates.DataBind()
                'End If
            End If

            enablemarkuptype()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionMulticost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Sub enablemarkuptype()
        If ddlMarkuptype.Value = "Unit" Then
            txtoperator.Disabled = True


            txtoperator.Value = "*"
            lblAdultMarkup.Text = "Markup"
            tdMarkupChild.Style.Add("display", "none")
            tdMarkupSenior.Style.Add("display", "none")
            tdMarkupChild1.Style.Add("display", "none")
            tdMarkupSenior1.Style.Add("display", "none")

        Else
            txtoperator.Disabled = True
            txtoperator.Value = "+"


            lblAdultMarkup.Text = "Markup for Adult"
            tdMarkupChild.Style.Add("display", "block")
            tdMarkupSenior.Style.Add("display", "block")
            tdMarkupChild1.Style.Add("display", "block")
            tdMarkupSenior1.Style.Add("display", "block")
        End If
    End Sub
#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If Viewstate("pricelistState") = "New" Then
        ElseIf Viewstate("pricelistState") = "Edit" Or Viewstate("pricelistState") = "Copy" Then
           

            txtRemark.Disabled = False
            txtvehiclename.Enabled = True
            txtexctypename.Enabled = True
            txtCurrName.Enabled = True
            grdDates.Enabled = True
            gv_ExRates.Enabled = True
            txtApplicableTo.Enabled = True
            txtmarkup.Enabled = True
            ddlMarkuptype.Disabled = False
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            '   wucCountrygroup.Disable(False)
            chkapprove.Enabled = True

            txtsectorname.Enabled = True
            btnSecotorselect.Enabled = True

        ElseIf Viewstate("pricelistState") = "Delete" Or Viewstate("pricelistState") = "View" Then
            txtvehiclename.Enabled = False
            txtexctypename.Enabled = False
            txtCurrName.Enabled = False

            grdDates.Enabled = False
            gv_ExRates.Enabled = False
            txtApplicableTo.Enabled = False
            txtmarkup.Enabled = False
            ddlMarkuptype.Disabled = True
            txtRemark.Disabled = True
            btnAddrow.Enabled = False
            btndeleterow.Enabled = False
            wucCountrygroup.Disable(False)
            chkapprove.Enabled = False
            txtsectorname.Enabled = False
            btnSecotorselect.Enabled = False

        End If

        'txtCurrCode.Enabled = False
        'txtCurrName.Enabled = False

    End Sub
#End Region



#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Dim strValue As String = ""
        Try

            'If ddlSPType.Items(ddlSPType.SelectedIndex).Text = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selling  Type can not be blank.');", True)
            '    '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSPTypeCD.ClientID + "');", True)
            '    Return False
            '    Exit Function
            'End If


            'If ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group Code can not be blank.');", True)
            '    '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSupplierAgent.ClientID + "');", True)
            '    Return False
            '    Exit Function
            'End If

            If txtexctypename.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Excursion  can not be blank.');", True)

                Return False
                Exit Function
            End If

            If txtApplicableTo.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicableto  can not be blank.');", True)

                Return False
                Exit Function
            End If

            If txtvehiclename.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Vehicle  can not be blank.');", True)

                Return False
                Exit Function
            End If
            If txtsectorname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sector  can not be blank.');", True)

                Return False
                Exit Function
            End If
            ValidatePage = True

        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            ValidatePage = False
        End Try

    End Function
#End Region
#Region "Public Sub fillroomgrid(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillroomgrid(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 7
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True

    End Sub
#End Region
#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim obj As New EncryptionDecryption
        Try
            Dim myDS As New DataSet

            'If CType(Viewstate("pricelistState"), String) = "New" Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'End If
            fillroomgrid(gv_ExRates, True)
            'createdatatable()
            btnSave.Style.Add("display", "block")
            btncancel.Style.Add("display", "block")
            divmarkup1.Style.Add("display", "block")
            btnAddrow.Style.Add("display", "block")
            btndeleterow.Style.Add("display", "block")

            'strSqlQry = "select  h.exctypcode,h.exctypname,h.ratebasis,d.seniorallowed from excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
            '    & " inner join  excursiontypes_suppliers es on h.exctypcode =es.exctypcode  where active=1 and h.classificationcode='" & txtexctypecode.Text & "' and es.partycode='" & txtexctypecode.Text & "'   and isnull(h.multicost,'') ='NO'"

            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            'myDataAdapter.Fill(myDS)
            'grdExrates.DataSource = myDS

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    btnGenerate.Style.Add("display", "none")
            '    btnsave.Style.Add("display", "block")
            '    btncancel.Style.Add("display", "block")
            '    grdExrates.DataBind()
            'Else
            '    btnGenerate.Style.Add("display", "block")
            '    btnsave.Style.Add("display", "none")
            '    btncancel.Style.Add("display", "none")

            '    grdExrates.PageIndex = 0
            '    grdExrates.DataBind()

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('For This Supplier There is No  Supplier Linked');", True)



            'End If


        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Private Sub createdatatable()"
    Private Sub createdatatable()
        Try

            Dim dt As New DataTable
            Dim dr As DataRow

            Dim gvRow As GridViewRow

            cnt = 0

            'strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM othcatmast  WHERE othcatmast.othgrpcode in (select option_selected from reservation_parameters where param_id  = '" & Session("OthPListFilter") & "')and othcatmast.active=1 "

            'cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            'If cnt <= 0 Then
            '    Session("CheckGridColumn") = "Not Present"
            'Else
            '    Session("CheckGridColumn") = ""
            'End If

            Dim strvehiclecount As String = ""
            strvehiclecount = hdnvehicle.Value
            Dim mString As String() = strvehiclecount.Split(",")

            cnt = mString.Length


            Dim strCondition As String = ""
            If hdnvehicle.Value <> "" Then
                strvehiclecount = hdnvehicle.Value

            End If

            If strvehiclecount.Length > 0 Then
                Dim mString1 As String() = strvehiclecount.Split(",")
                For k As Integer = 0 To mString1.Length - 1
                    If strCondition = "" Then
                        strCondition = "'" & mString1(k) & "'"
                    Else
                        strCondition &= ",'" & mString1(k) & "'"
                    End If
                Next
            End If




            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "SELECT  distinct othcatcode, othcatmast.grporder FROM othcatmast  WHERE othcatcode IN (" & strCondition & ") and  " _
                 & " othcatmast.othgrpcode in (select option_selected from reservation_parameters where param_id = '564')  and othcatmast.active=1 Order by othcatmast.grporder"

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(i) = mySqlReader("othcatcode")
                i = i + 1
            End While
            mySqlReader.Close()
            mySqlConn.Close()




            'Here in Array store room types
            '-------------------------------------
            Dim tf As New TemplateField
            dt = New DataTable

            'dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
            dt.Columns.Add(New DataColumn("Supplier", GetType(String)))
            dt.Columns.Add(New DataColumn("Service Type", GetType(String)))
            dt.Columns.Add(New DataColumn("SupplierCode", GetType(String)))

            'create columns of this room types in data table
            For i = 0 To cnt - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next



            Session("GV_OthPLData") = dt

            For i = gv_ExRates.Columns.Count - 1 To 3 Step -1
                gv_ExRates.Columns.RemoveAt(i)
            Next



            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Supplier" And col.ColumnName <> "Service Type" And col.ColumnName <> "SupplierCode" Then
                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
                    gv_ExRates.Columns.Add(bfield)


                End If
            Next
            gv_ExRates.Visible = True



            For Each GvRow In gv_ExRates.Rows

                dr = dt.NewRow

                Dim chkSelect As CheckBox = GvRow.FindControl("chkSelect")

                Dim txtservicetype As TextBox = gvRow.FindControl("txtservicetype")
                Dim txtsuppname As TextBox = gvRow.FindControl("txtsuppname")
                Dim txtsuppcode As TextBox = gvRow.FindControl("txtsuppcode")
                Dim lblCLineno As Label = gvRow.FindControl("lblCLineno")



                '   dr("Sr No") = txtservicetype.Text
                dr("Supplier") = txtsuppname.Text
                dr("Service Type") = txtservicetype.Text
                dr("SupplierCode") = txtsuppcode.Text


                dt.Rows.Add(dr)



            Next


            gv_ExRates.DataSource = dt
            gv_ExRates.DataBind()


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionMulticost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getcurrlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim currname As New List(Of String)
        Try

            strSqlQry = "select currcode,currname  from  currmast where active=1 and convrate <>0  and currname like  '" & Trim(prefixText) & "%' order by currname "

            '   strSqlQry = "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where  " _
            '      & " othmaingrpcode in(select option_selected from reservation_parameters where param_id in (1104,1105)) and active=1 and othgrpname like '" & Trim(prefixText) & "%' order by othgrpname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    currname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))

                Next

            End If

            Return currname
        Catch ex As Exception
            Return currname
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getvehiclelist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim vehiclename As New List(Of String)
        Try

            strSqlQry = "select othcatname, othcatcode  from  othcatmast where active=1 and  othgrpcode in (select option_selected from reservation_parameters where param_id=564) and othcatname like  '" & Trim(prefixText) & "%' order by othcatname "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    vehiclename.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othcatname").ToString(), myDS.Tables(0).Rows(i)("othcatcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return vehiclename
        Catch ex As Exception
            Return vehiclename
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetExcursionlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Excursionname As New List(Of String)
        Try

            strSqlQry = "select exctypname, exctypcode  from  view_excursiontypes where active=1 and isnull(multicost,'')='YES' and exctypname like  '" & Trim(prefixText) & "%' order by exctypname "

            '   strSqlQry = "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where  " _
            '      & " othmaingrpcode in(select option_selected from reservation_parameters where param_id in (1104,1105)) and active=1 and othgrpname like '" & Trim(prefixText) & "%' order by othgrpname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Excursionname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("exctypname").ToString(), myDS.Tables(0).Rows(i)("exctypcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return Excursionname
        Catch ex As Exception
            Return Excursionname
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplierlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim partyname As New List(Of String)
        Try

            strSqlQry = "select partycode, partyname  from  partymast where active=1 and  sptypecode not in (select option_selected from reservation_parameters where param_id=458) and  partyname like  '" & Trim(prefixText) & "%' order by partyname "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    partyname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))

                Next

            End If

            Return partyname
        Catch ex As Exception
            Return partyname
        End Try

    End Function
    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        'Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("DELETE", row.RowIndex)
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try


        Dim count As Integer

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)
        Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        count = gvchildage.Rows.Count + 1

        Dim GVRow As GridViewRow
        '  count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdDates.Rows
                '  chkSelect = GVRow.FindControl("chkSelect")
                'If chkSelect.Checked = False Then
                If k <> row.RowIndex Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

                k = k + 1
            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdDates.Rows.Count > 1 Then
                fillDategrd(grdDates, False, grdDates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdDates, False, grdDates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If GVRow.RowIndex < count Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpFDate.Text = fDate(n)
                    dpTDate = GVRow.FindControl("txtToDate")
                    dpTDate.Text = tDate(n)
                    'lblseason = GVRow.FindControl("lblseason")
                    'lblseason.Text = excl(n)
                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try


    End Sub
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))

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
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        'Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("ADD", 0)
        '    row.FindControl("imgStayAdd").Visible = False
        '    Dim txtfromdate As TextBox
        '    txtfromdate = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromdate"), TextBox)
        '    txtfromdate.Focus()
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try


        Dim count As Integer
        Dim GVRow As GridViewRow

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

        count = gvchildage.Rows.Count + 1

        'count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Try
            For Each GVRow In grdDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                'lblseason = GVRow.FindControl("lblseason")
                'excl(n) = CType(lblseason.Text, String)
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
                'lblseason = GVRow.FindControl("lblseason")
                'lblseason.Text = excl(n)

                n = n + 1
            Next

            'For Each GVRow In grdDates.Rows
            '    lblseason = GVRow.FindControl("lblseason")
            '    If lblseason.Text = "" Then
            '        lblseason.Text = txtseasonname.Text
            '    End If
            'Next

            Dim txtStayFromDt As TextBox
            txtStayFromDt = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
            txtStayFromDt.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Session("sessionRemark") = Nothing
        Session("GV_OthHotelData") = Nothing
        ViewState("OthpricelistRefCode") = Nothing
        Viewstate("pricelistState") = Nothing
        ' Response.Redirect("OtherServicesCostPriceListSearch.aspx")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim RefCode As String
        If IsPostBack() = False Then


            ViewState.Add("pricelistState", Request.QueryString("State"))
            ViewState.Add("RefCode", Request.QueryString("RefCode"))


            Dim supagentcode As String
            supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1021'")

            wucCountrygroup.sbSetPageState("", "EXCMULTICOST", CType(ViewState("pricelistState"), String))

            'Added by abin on 30/05/2018
            Dim strmarkupAdult = "'" + txtmarkup.ClientID + "'"
            strmarkupAdult = strmarkupAdult + ",'" + txtMarkupAdultTV.ClientID + "'"
            strmarkupAdult = strmarkupAdult + ",'" + txtMarkupAdultVAT.ClientID + "'"
            txtmarkup.Attributes.Add("onblur", "CalculateTax(" & strmarkupAdult & ")")

            Dim strmarkupChild = "'" + txtChildMarkup.ClientID + "'"
            strmarkupChild = strmarkupChild + ",'" + txtMarkupChildTV.ClientID + "'"
            strmarkupChild = strmarkupChild + ",'" + txtMarkupChildVAT.ClientID + "'"
            txtChildMarkup.Attributes.Add("onblur", "CalculateTax(" & strmarkupChild & ")")

            Dim strmarkupSenior = "'" + txtSeniorMarkup.ClientID + "'"
            strmarkupSenior = strmarkupSenior + ",'" + txtMarkupSeniorTV.ClientID + "'"
            strmarkupSenior = strmarkupSenior + ",'" + txtMarkupSeniorVAT.ClientID + "'"
            txtSeniorMarkup.Attributes.Add("onblur", "CalculateTax(" & strmarkupSenior & ")")
            'end


            If CType(ViewState("pricelistState"), String) = "New" Then


                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                txtApplicableTo.Focus()

                chkPriceWithTax.Checked = True
                txtVAT.Text = "5"
            ElseIf CType(ViewState("pricelistState"), String) = "Copy" Then
                txtApplicableTo.Focus()


                txtPlcCode.Disabled = True
                RefCode = CType(ViewState("RefCode"), String)
                ShowRecord(RefCode)
                ShowDates(CType(RefCode, String))
                Showdetailsgrid(CType(RefCode, String))
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                wucCountrygroup.sbShowCountry()

                txtPlcCode.Value = ""
                btnGenerate.Style.Add("display", "none")

                chkPriceWithTax.Checked = True
                txtVAT.Text = "5"
                TaxCalculation()
            ElseIf CType(ViewState("pricelistState"), String) = "Edit" Then

                btnSave.Text = "Update"

                RefCode = CType(ViewState("RefCode"), String)
                ShowRecord(RefCode)
                ShowDates(CType(RefCode, String))

                '  btnGenerate_Click(sender, e)
                Showdetailsgrid(CType(RefCode, String))

                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                wucCountrygroup.sbShowCountry()

                btnGenerate.Style.Add("display", "none")
                btnSave.Text = "Update"
                TaxCalculation()
            ElseIf CType(ViewState("pricelistState"), String) = "View" Then

                RefCode = CType(ViewState("RefCode"), String)

                ShowRecord(RefCode)
                ShowDates(CType(RefCode, String))
                Showdetailsgrid(CType(RefCode, String))
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                wucCountrygroup.sbShowCountry()

                btnGenerate.Style.Add("display", "none")
                btnSave.Style.Add("display", "none")
                TaxCalculation()

            ElseIf CType(ViewState("pricelistState"), String) = "Delete" Then

                RefCode = CType(ViewState("RefCode"), String)

                ShowRecord(RefCode)
                ShowDates(CType(RefCode, String))
                Showdetailsgrid(CType(RefCode, String))
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                wucCountrygroup.sbShowCountry()

                btnGenerate.Style.Add("display", "none")
                btnSave.Text = "Delete"
                TaxCalculation()
            End If
            DisableAllControls()
            enablemarkuptype()
        Else

            'If ddlexccode.Value <> "[Select]" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "airportbordercode", "airportbordername", "select airportbordercode , airportbordername from airportbordersmaster where active=1 order by airportbordercode", True, ddlexccode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "airportbordername", "airportbordercode", "select airportbordername , airportbordercode from airportbordersmaster where active=1 order by airportbordername", True, ddlexcname.Value)
            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "airportbordercode", "airportbordername", "select airportbordercode , airportbordername from airportbordersmaster where active=1 order by airportbordercode", True, ddlexccode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "airportbordername", "airportbordercode", "select airportbordername , airportbordercode from airportbordersmaster where active=1 order by airportbordername", True, ddlexcname.Value)
            'End If



            'If ddlSPType.Value <> "[Select]" Then
            '    strSqlQry = "select excsellcode,excsellname from excsellmast where active=1 order by excsellcode"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", strSqlQry, True, ddlSPType.Value)

            '    strSqlQry = "select excsellcode,excsellname from excsellmast where active=1 order by excsellname "
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", strSqlQry, True, ddlSPTypeName.Value)
            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1 order by excsellcode", True, ddlSPType.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True, ddlSPTypeName.Value)
            'End If

            'If ddlGroupCode.Value <> "[Select]" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) and othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' order by othgrpname", True, ddlGroupCode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) order by othgrpcode", True, ddlGroupName.Value)
            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) order by othgrpname", True, ddlGroupCode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) order by othgrpcode", True, ddlGroupName.Value)
            'End If



        End If

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcMultiCost','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function getFillRateType(ByVal prefixText As String) As List(Of String)
        Dim promotionlist As New List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Try


            strSqlQry = "select distinct namecode from extracodes  where type=2  and namecode like '" & Trim(prefixText) & "%' order by namecode "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter

            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    promotionlist.Add(myDS.Tables(0).Rows(i)("namecode").ToString())
                Next

            End If



            Return promotionlist
          

            Return promotionlist
        Catch ex As Exception
            Return promotionlist
        End Try

    End Function

    

    Protected Sub btnselectvehicle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselectvehicle.Click
        FillVehicles()
        ModalSelectVehicle.Show()
    End Sub
    Private Sub FillVehicles()
        Try


            Dim dt As New DataTable
            Dim strSqlQry As String = ""

          


            '''''''''''

           
            Dim myDS As New DataSet

            strSqlQry = "select  othcatcode,othcatname from othcatmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=564) " ' "sp_showapplydiscount'" & CType(hdnpartycode.Value, String) & "' , '" & Format(CType(mindate, Date), "yyyy/MM/dd") & "' ,'" & Format(CType(maxdate, Date), "yyyy/MM/dd") & "','" & ViewState("CountryList") & "','" & txtpromotionid.Text & "', '" & ddlapplydiscount.Value & "'"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gvSelectVehicle.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gvSelectVehicle.DataBind()

            Else
                gvSelectVehicle.PageIndex = 0
                gvSelectVehicle.DataBind()

            End If



            ChkExistingValdiscount(hdnvehicle.Value)








        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub ChkExistingValdSector(ByVal prm_strChkdVal As String)


        Dim arrString As String()
        Dim arrlist As String()
        Dim j As Integer = 0 'string in the form "a,b;c,d;...."

        If prm_strChkdVal <> "" Then
            arrString = prm_strChkdVal.Split(",") 'spliting for ';' 1st





            For k = 0 To arrString.Length - 1
                If arrString(k) <> "" Then

                    'Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                    'For Each grdRow In gvSectors.Rows
                    For Each grdRow As DataListItem In gvSectors.Items
                        Dim chkairportSelect As CheckBox = CType(grdRow.FindControl("chksectorSelect"), CheckBox)
                        Dim lblairportcode As Label = grdRow.FindControl("lblsector")



                        If (arrString(k) = lblairportcode.Text) Then
                            chkairportSelect.Checked = True

                        End If

                    Next
                End If
            Next


        End If
    End Sub

    Private Sub ChkExistingValdiscount(ByVal prm_strChkdVal As String)


        Dim arrString As String()
        Dim arrlist As String()
        Dim j As Integer = 0 'string in the form "a,b;c,d;...."

        If prm_strChkdVal <> "" Then
            arrString = prm_strChkdVal.Split(",") 'spliting for ';' 1st



            For k = 0 To arrString.Length - 1
                If arrString(k) <> "" Then

                    'Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                    For Each grdRow In gvSelectVehicle.Rows
                        Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkvehicleSelect"), CheckBox)
                        Dim lblvehiclecode As Label = grdRow.FindControl("lblvehiclecode")



                        If (arrString(k) = lblvehiclecode.Text) Then
                            chkmarkupSelect.Checked = True

                        End If

                    Next
                End If
            Next


        End If
    End Sub
   

    Protected Sub gvSelectVehicle_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSelectVehicle.PageIndexChanging
        gvSelectVehicle.PageIndex = e.NewPageIndex
        FillVehicles()
        ModalSelectVehicle.Show()
    End Sub

    Protected Sub btnokvehicle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnokvehicle.Click
        Dim applies As String = ""
        Dim tickedornot As Boolean
        tickedornot = False
        For Each grdRow In gvSelectVehicle.Rows
            Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkvehicleSelect"), CheckBox)
            chkmarkupSelect = grdRow.FindControl("chkvehicleSelect")
            If chkmarkupSelect.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next

        If tickedornot = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any ');", True)
            ModalSelectVehicle.Show()
            Exit Sub
        End If


        For Each grdRow In gvSelectVehicle.Rows
            Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkvehicleSelect"), CheckBox)
            Dim lblvehiclecode As Label = grdRow.FindControl("lblvehiclecode")

            If chkmarkupSelect.Checked = True Then
                If lblvehiclecode.Text <> "" Then
                    applies = applies + "," + lblvehiclecode.Text
                End If
               

            End If
        Next

        If applies.Length > 0 Then
            hdnvehicle.Value = Right(applies, Len(applies) - 1)

        End If

        ModalSelectVehicle.Hide()
    End Sub
#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
    Protected Sub gv_ExRates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_ExRates.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim txtperAdult As TextBox = CType(e.Row.FindControl("txtperAdult"), TextBox)
            Dim txtperChild As TextBox = CType(e.Row.FindControl("txtperChild"), TextBox)
            Dim txtperSenior As TextBox = CType(e.Row.FindControl("txtperSenior"), TextBox)
            Dim txtchildfreeupto As TextBox = CType(e.Row.FindControl("txtchildfreeupto"), TextBox)

            Dim txtperunit As TextBox = CType(e.Row.FindControl("txtperunit"), TextBox)
            Numberssrvctrl(txtperAdult)
            Numberssrvctrl(txtperunit)
            Numberssrvctrl(txtperChild)
            Numberssrvctrl(txtperSenior)
            Numberssrvctrl(txtchildfreeupto)

            '*** >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Added by abin on 28/05/2018

            Dim strAdult = "'" + txtperAdult.ClientID + "'"
            Dim txtAdultTV As TextBox = CType(e.Row.FindControl("txtAdultTV"), TextBox)
            strAdult = strAdult + ",'" + txtAdultTV.ClientID + "'"
            Dim txtAdultVAT As TextBox = CType(e.Row.FindControl("txtAdultVAT"), TextBox)
            strAdult = strAdult + ",'" + txtAdultVAT.ClientID + "'"
            txtperAdult.Attributes.Add("onblur", "CalculateTax(" & strAdult & ")")

            Dim strChild = "'" + txtperChild.ClientID + "'"
            Dim txtChildTV As TextBox = CType(e.Row.FindControl("txtChildTV"), TextBox)
            strChild = strChild + ",'" + txtChildTV.ClientID + "'"
            Dim txtChildVAT As TextBox = CType(e.Row.FindControl("txtChildVAT"), TextBox)
            strChild = strChild + ",'" + txtChildVAT.ClientID + "'"
            txtperChild.Attributes.Add("onblur", "CalculateTax(" & strChild & ")")

            Dim strSenior = "'" + txtperSenior.ClientID + "'"
            Dim txtSeniorTV As TextBox = CType(e.Row.FindControl("txtSeniorTV"), TextBox)
            strSenior = strSenior + ",'" + txtSeniorTV.ClientID + "'"
            Dim txtSeniorVAT As TextBox = CType(e.Row.FindControl("txtSeniorVAT"), TextBox)
            strSenior = strSenior + ",'" + txtSeniorVAT.ClientID + "'"
            txtperSenior.Attributes.Add("onblur", "CalculateTax(" & strSenior & ")")

            Dim strUnit = "'" + txtperunit.ClientID + "'"
            Dim txtUnitTV As TextBox = CType(e.Row.FindControl("txtUnitTV"), TextBox)
            strUnit = strUnit + ",'" + txtUnitTV.ClientID + "'"
            Dim txtUnitVAT As TextBox = CType(e.Row.FindControl("txtUnitVAT"), TextBox)
            strUnit = strUnit + ",'" + txtUnitVAT.ClientID + "'"
            txtperunit.Attributes.Add("onblur", "CalculateTax(" & strUnit & ")")
            ' txtunit.Attributes.Add("onblur", "CalculateTax()")



            '*** <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        End If
    End Sub

    Protected Sub btnAddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddrow.Click

        Dim count As Integer
        Dim GVRow As GridViewRow


        count = gv_ExRates.Rows.Count + 1

        Dim n As Integer = 0

        Dim chkSelect As CheckBox

        Dim txtsuppname As TextBox
        Dim txtsuppcode As TextBox
        Dim txtservicetype As TextBox
        Dim txtperadult As TextBox
        Dim txtperunit As TextBox
        Dim txtperChild As TextBox
        Dim txtperSenior As TextBox
        Dim txtchildfreeupto As TextBox


        Dim suppname(count) As String
        Dim suppcode(count) As String
        Dim servicetype(count) As String
        Dim perunit(count) As String
        Dim peradult(count) As String
        Dim perchild(count) As String
        Dim persenior(count) As String
        Dim childfreeupto(count) As String



        '   CopyRow = 0


        Try

            For Each GVRow In gv_ExRates.Rows
                txtsuppname = GVRow.FindControl("txtsuppname")
                txtsuppcode = GVRow.FindControl("txtsuppcode")
                txtservicetype = GVRow.FindControl("txtservicetype")
                txtperunit = GVRow.FindControl("txtperunit")
                txtperadult = GVRow.FindControl("txtperAdult")
                txtperChild = GVRow.FindControl("txtperChild")
                txtperSenior = GVRow.FindControl("txtperSenior")
                txtchildfreeupto = GVRow.FindControl("txtchildfreeupto")
              

                chkSelect = GVRow.FindControl("chkSelect")

                If chkSelect.Checked = True Then
                    ' CopyRow = n
                End If

                suppname(n) = CType(txtsuppname.Text, String)
                suppcode(n) = CType(txtsuppcode.Text, String)
                servicetype(n) = CType(txtservicetype.Text, String)
                perunit(n) = CType(txtperunit.Text, String)
                peradult(n) = CType(txtperadult.Text, String)
             
                perchild(n) = CType(txtperChild.Text, String)
                perSenior(n) = CType(txtperSenior.Text, String)
                childfreeupto(n) = CType(txtchildfreeupto.Text, String)


                n = n + 1
            Next
            fillroomgrid(gv_ExRates, False, gv_ExRates.Rows.Count + 1)




            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_ExRates.Rows
                If n = i Then

                    Exit For
                End If
                txtsuppname = GVRow.FindControl("txtsuppname")
                txtsuppcode = GVRow.FindControl("txtsuppcode")
                txtservicetype = GVRow.FindControl("txtservicetype")
                txtperunit = GVRow.FindControl("txtperunit")
                txtperadult = GVRow.FindControl("txtperAdult")
                txtperChild = GVRow.FindControl("txtperChild")
                txtperSenior = GVRow.FindControl("txtperSenior")
                txtchildfreeupto = GVRow.FindControl("txtchildfreeupto")
                chkSelect = GVRow.FindControl("chkSelect")
               

                txtsuppcode.Text = suppcode(n)
                txtsuppname.Text = suppname(n)
                txtservicetype.Text = servicetype(n)
                txtperunit.Text = perunit(n)
                txtperadult.Text = peradult(n)
                txtperChild.Text = perchild(n)
                txtperSenior.Text = persenior(n)
                txtchildfreeupto.Text = childfreeupto(n)


                n = n + 1
            Next

         

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btndeleterow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleterow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_ExRates.Rows.Count + 1


        Dim n As Integer = 0

        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0


         Dim txtsuppname As TextBox
        Dim txtsuppcode As TextBox
        Dim txtservicetype As TextBox
        Dim txtperadult As TextBox
        Dim txtperunit As TextBox
        Dim txtperChild As TextBox
        Dim txtperSenior As TextBox
        Dim txtchildfreeupto As TextBox


        Dim suppname(count) As String
        Dim suppcode(count) As String
        Dim servicetype(count) As String
        Dim perunit(count) As String
        Dim peradult(count) As String
        Dim perchild(count) As String
        Dim persenior(count) As String
        Dim childfreeupto(count) As String



        Try
            For Each GVRow In gv_ExRates.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then

                    txtsuppname = GVRow.FindControl("txtsuppname")
                    txtsuppcode = GVRow.FindControl("txtsuppcode")
                    txtservicetype = GVRow.FindControl("txtservicetype")
                    txtperunit = GVRow.FindControl("txtperunit")
                    txtperadult = GVRow.FindControl("txtperAdult")
                    txtperChild = GVRow.FindControl("txtperChild")
                    txtperSenior = GVRow.FindControl("txtperSenior")
                    txtchildfreeupto = GVRow.FindControl("txtchildfreeupto")


                    suppname(n) = CType(txtsuppname.Text, String)
                    suppcode(n) = CType(txtsuppcode.Text, String)
                    servicetype(n) = CType(txtservicetype.Text, String)
                    perunit(n) = CType(txtperunit.Text, String)
                    txtperadult.Text = peradult(n)
                    txtperChild.Text = perchild(n)
                    txtperSenior.Text = persenior(n)
                    txtchildfreeupto.Text = childfreeupto(n)


                    n = n + 1

                Else
                    deletedrow = deletedrow + 1

                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillroomgrid(gv_ExRates, False, count)
            'If gv_Filldata.Rows.Count > 1 Then
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count - deletedrow)
            'Else
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count)
            'End If


            Dim i As Integer = n
            n = 0

            For Each GVRow In gv_ExRates.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then


                    txtsuppname = GVRow.FindControl("txtsuppname")
                    txtsuppcode = GVRow.FindControl("txtsuppcode")
                    txtservicetype = GVRow.FindControl("txtservicetype")
                    txtperunit = GVRow.FindControl("txtperunit")
                    txtperadult = GVRow.FindControl("txtperAdult")
                    txtperChild = GVRow.FindControl("txtperChild")
                    txtperSenior = GVRow.FindControl("txtperSenior")
                    txtchildfreeupto = GVRow.FindControl("txtchildfreeupto")


                   txtsuppcode.Text = suppcode(n)
                    txtsuppname.Text = suppname(n)
                    txtservicetype.Text = servicetype(n)
                    txtperunit.Text = perunit(n)
                    txtperadult.Text = peradult(n)
                    txtperChild.Text = perchild(n)
                    txtperSenior.Text = persenior(n)
                    txtchildfreeupto.Text = childfreeupto(n)


                    n = n + 1
                End If
            Next


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow

        If txtApplicableTo.Text = "" Or txtApplicableTo.Text = " " Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable To Should not be Blank');", True)
            ValidateSave = False
            SetFocus(txtApplicableTo)
            Exit Function
        End If


        If Val(txtmarkup.Text) = 0 And Val(txtChildMarkup.Text) = 0 And Val(txtSeniorMarkup.Text) = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Markup');", True)
            ValidateSave = False
            SetFocus(txtApplicableTo)
            Exit Function
        End If



        Dim ratesexists As Boolean = False
        For Each gvRow In gv_ExRates.Rows
             Dim txtsuppcode As TextBox = GvRow.FindControl("txtsuppcode")
            Dim txtservicetype As TextBox = gvRow.FindControl("txtservicetype")
            Dim txtperadult As TextBox = gvRow.FindControl("txtperadult")
            Dim txtperchild As TextBox = gvRow.FindControl("txtperchild")
            Dim txtpersenior As TextBox = gvRow.FindControl("txtpersenior")
            Dim txtchildfreeupto As TextBox = gvRow.FindControl("txtchildfreeupto")
            Dim txtperunit As TextBox = gvRow.FindControl("txtperunit")

            If txtsuppcode.Text <> "" Then
                If Val(txtperadult.Text) <> 0 Or Val(txtperchild.Text) <> 0 Or Val(txtpersenior.Text) <> 0 Or Val(txtperunit.Text) <> 0 Then
                    ratesexists = True
                    Exit For
                End If
            End If

            

        Next

        If ratesexists = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Rates for One Suppliers');", True)
            ValidateSave = False
            Exit Function
        End If
        Dim lnCnt As Integer = 0
        For Each gvRow In gv_ExRates.Rows
            lnCnt += 1
            Dim txtsuppcode As TextBox = gvRow.FindControl("txtsuppcode")
            Dim txtservicetype As TextBox = gvRow.FindControl("txtservicetype")
            Dim txtperadult As TextBox = gvRow.FindControl("txtperadult")
            Dim txtperchild As TextBox = gvRow.FindControl("txtperchild")
            Dim txtpersenior As TextBox = gvRow.FindControl("txtpersenior")
            Dim txtchildfreeupto As TextBox = gvRow.FindControl("txtchildfreeupto")
            Dim txtperunit As TextBox = gvRow.FindControl("txtperunit")
            Dim txtsuppname As TextBox = gvRow.FindControl("txtsuppname")

            If txtsuppcode.Text <> "" Then
                If Val(txtperadult.Text) = 0 And Val(txtperchild.Text) = 0 And Val(txtpersenior.Text) = 0 And Val(txtperunit.Text) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Per Adult/Child/Senior and Per Unit Should not be Blank :" & txtsuppname.Text & " and Line no :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If
            End If



        Next


        ValidateSave = True

    End Function
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
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select h.*,e.exctypname from excmulticplist_header h,view_excursiontypes e Where h.exctypcode=e.exctypcode and  h.eplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then


                    If IsDBNull(mySqlReader("eplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("eplistcode")
                    End If

                    'If IsDBNull(mySqlReader("esellcode")) = False Then
                    '    ddlSPTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excplist_header", "esellcode", "eplistcode", mySqlReader("eplistcode"))
                    '    ddlSPType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excsellmast", "excsellname", "excsellcode", ddlSPTypeName.Value)

                    'End If

                    If IsDBNull(mySqlReader("exctypcode")) = False Then
                        txtexctypecode.Text = mySqlReader("exctypcode")
                        txtexctypename.Text = mySqlReader("exctypname")
                        ' txtgroupname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  classificationname from excclassification_header where classificationcode='" & txtgroupcode.Text & "'")


                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  currname from currmast where currcode='" & txtCurrCode.Text & "'")
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    If IsDBNull(mySqlReader("vehiclecode")) = False Then
                        txtvehiclecode.Text = mySqlReader("vehiclecode")
                        txtvehiclename.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  othcatname from  othcatmast where active=1 and othcatcode='" & txtvehiclecode.Text & "'")
                    End If
                    If IsDBNull(mySqlReader("markupvalue")) = False Then
                        txtmarkup.Text = mySqlReader("markupvalue")
                    End If
                    If IsDBNull(mySqlReader("markupvalue_child")) = False Then
                        txtChildMarkup.Text = mySqlReader("markupvalue_child")
                    End If
                    If IsDBNull(mySqlReader("markupvalue_senior")) = False Then
                        txtSeniorMarkup.Text = mySqlReader("markupvalue_senior")
                    End If
                    If IsDBNull(mySqlReader("markupoperator")) = False Then
                        txtoperator.Value = mySqlReader("markupoperator")
                    End If

                    If IsDBNull(mySqlReader("markuptype")) = False Then
                        ddlMarkuptype.Value = mySqlReader("markuptype")
                    End If

                    If IsDBNull(mySqlReader("sectorgroupcode")) = False Then
                        txtsectorcode.Text = mySqlReader("sectorgroupcode")

                        Dim strMealPlans1 As String = ""
                        Dim strCondition1 As String = ""
                        strMealPlans1 = mySqlReader("sectorgroupcode")
                        If strMealPlans1.Length > 0 Then
                            Dim mString1 As String() = strMealPlans1.Split(",")
                            For i As Integer = 0 To mString1.Length - 1
                                If strCondition1 = "" Then
                                    strCondition1 = "'" & mString1(i) & "'"
                                Else
                                    strCondition1 &= ",'" & mString1(i) & "'"
                                End If
                            Next

                            Session("sectors") = strCondition1
                        End If
                        Dim StrQry As String
                        StrQry = "select distinct stuff((select  ',' + u.othtypname   from othtypmast u(nolock)  where u.othtypcode =othtypcode and u.othtypcode in (" & strCondition1 & ")   group by othtypname    for xml path('')),1,1,'')  othtypname " _
                              & " from othtypmast where  othtypcode in (" & strCondition1 & ") "

                        txtsectorname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrQry)
                    End If

                    If mySqlReader("approved") = "1" Then
                        chkapprove.Checked = True
                    Else
                        chkapprove.Checked = False
                    End If





                    If ddlexccode.Value <> "[Select]" Then


                    End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    '**** >>>>>>>>>>>>>>>>>>> Added by abin on 20180529 
                    If IsDBNull(mySqlReader("VATPerc")) = False Then
                        txtVAT.Text = mySqlReader("VATPerc")
                    End If
                    If IsDBNull(mySqlReader("CostWithTax")) = False Then
                        If mySqlReader("CostWithTax") = "1" Then
                            chkPriceWithTax.Checked = True
                        Else
                            chkPriceWithTax.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("markuptype")) = False Then
                        If mySqlReader("markuptype") = "Unit" Then
                            If IsDBNull(mySqlReader("UnitMarkupTaxableValue")) = False Then
                                txtMarkupAdultTV.Text = mySqlReader("UnitMarkupTaxableValue")
                            End If
                            If IsDBNull(mySqlReader("UnitMarkupVATValue")) = False Then
                                txtMarkupAdultVAT.Text = mySqlReader("UnitMarkupVATValue")
                            End If
                        End If
                    Else
                        If IsDBNull(mySqlReader("AdultMarkupTaxableValue")) = False Then
                            txtMarkupAdultTV.Text = mySqlReader("AdultMarkupTaxableValue")
                            txtMarkupAdultVAT.Text = mySqlReader("AdultMarkupVATValue")
                        End If
                        If IsDBNull(mySqlReader("ChildMarkupTaxableValue")) = False Then
                            txtMarkupChildTV.Text = mySqlReader("ChildMarkupTaxableValue")
                            txtMarkupChildVAT.Text = mySqlReader("ChildMarkupVATValue")
                        End If
                        If IsDBNull(mySqlReader("SeniorMarkupTaxableValue")) = False Then
                            txtMarkupSeniorTV.Text = mySqlReader("SeniorMarkupTaxableValue")
                            txtMarkupSeniorVAT.Text = mySqlReader("SeniorMarkupVATValue")
                        End If
                    End If
                End If

                '**** <<<<<<<<<<<<<<<<<<<<<<< Added by abin on 20180529 





            End If


        Catch ex As Exception
            objUtils.WritErrorLog("ExcursionMulticost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region
    Private Sub Showdetailsgrid(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            gv_ExRates.Visible = True
            strSqlQry = ""
            Dim gvrow As GridViewRow

            Dim strQry As String = ""
            Dim cnt As Integer = 0


            strQry = "select count( distinct elineno) from excmulticplist_detail(nolock) where eplistcode='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(gv_ExRates, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from excmulticplist_detail d  where   d.eplistcode='" & RefCode & "' order by elineno "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader



                For Each GvRow In gv_ExRates.Rows

                     Dim txtsuppcode As TextBox = GvRow.FindControl("txtsuppcode")
                    Dim txtservicetype As TextBox = GvRow.FindControl("txtservicetype")

                    Dim txtperunit As TextBox = gvrow.FindControl("txtperunit")
                    Dim txtsuppname As TextBox = gvrow.FindControl("txtsuppname")
                    Dim txtperAdult As TextBox = gvrow.FindControl("txtperAdult")
                    Dim txtperChild As TextBox = gvrow.FindControl("txtperChild")
                    Dim txtperSenior As TextBox = gvrow.FindControl("txtperSenior")
                    Dim txtchildfreeupto As TextBox = gvrow.FindControl("txtchildfreeupto")

                    If mySqlReader.Read = True Then



                        If IsDBNull(mySqlReader("partycode")) = False Then
                            txtsuppcode.Text = mySqlReader("partycode")
                            txtsuppname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  partyname from  partymast where active=1 and partycode='" & txtsuppcode.Text & "'")

                        End If


                        If IsDBNull(mySqlReader("servicedecription")) = False Then
                            txtservicetype.Text = mySqlReader("servicedecription")

                        End If
                        If IsDBNull(mySqlReader("perpax")) = False Then
                            txtperAdult.Text = mySqlReader("perpax")

                        End If
                        If IsDBNull(mySqlReader("perchild")) = False Then
                            txtperChild.Text = mySqlReader("perchild")

                        End If
                        If IsDBNull(mySqlReader("persenior")) = False Then
                            txtperSenior.Text = mySqlReader("persenior")

                        End If
                        If IsDBNull(mySqlReader("childfreeupto")) = False Then
                            txtchildfreeupto.Text = mySqlReader("childfreeupto")

                        End If
                        If IsDBNull(mySqlReader("perunitpax")) = False Then
                            txtperunit.Text = mySqlReader("perunitpax")

                        End If
                    

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionMultcost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub ShowDates(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            strSqlQry = "select count( distinct frmdate) from excmulticplist_dates(nolock) where eplistcode='" & RefCode & "'" '" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            mySqlCmd = New SqlCommand("Select * from excmulticplist_dates(nolock) Where eplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromdate")
                        dpTDate = gvRow.FindControl("txttodate")
                        '     Dim lblseason As Label = gvRow.FindControl("lblseason")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                dpFDate.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If
            '  txtseasonname.Enabled = False


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionMulticost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            'If Not Session("PlistSaved") = True Then ''this variable because response.redirect is causing a postback and saving twice

            Dim vatPerc As Decimal
            If IsNumeric(txtVAT.Text) And Val(txtVAT.Text) <> "0" Then
                vatPerc = Convert.ToDecimal(txtVAT.Text)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Vat Percentage can not be empty or zero.');", True)
                Exit Sub
            End If
            Dim GvRow As GridViewRow
            If Page.IsValid = True Then
                If ViewState("pricelistState") = "New" Or ViewState("pricelistState") = "Edit" Or ViewState("pricelistState") = "Copy" Then
                    If ValidateSave() = False Then
                        Exit Sub
                    End If
                    If txtsectorname.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sector  can not be blank.');", True)

                        Exit Sub
                    End If
                    If FindduplicatePeriod() = False Then

                        Exit Sub
                    End If

                    If Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("pricelistState") = "New" Or ViewState("pricelistState") = "Copy" Then
                        Dim optionval As String
                        Dim optionName As String

                     
                        optionName = "EXMULTIPL"




                        optionval = objUtils.GetAutoDocNo(optionName, mySqlConn, sqlTrans)
                        txtPlcCode.Value = optionval.Trim


                        mySqlCmd = New SqlCommand("sp_add_excmulticplist", mySqlConn, sqlTrans)
                    ElseIf ViewState("pricelistState") = "Edit" Then
                        'Inserting Into Logs
                        mySqlCmd = New SqlCommand("sp_excursionmulticostpriceslist_logs", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("sp_mod_excmulticplist", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcd", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)


                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtexctypecode.Text.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@vehiclecode", SqlDbType.VarChar, 20)).Value = CType(txtvehiclecode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 8000)).Value = CType(txtApplicableTo.Text.Trim, String)


                    If CType(txtCurrCode.Text.Trim, String) = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcd", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcd", SqlDbType.VarChar, 20)).Value = CType(txtCurrCode.Text.Trim, String)
                    End If

                    '**************************** Setting the MaX DATE and Min Date fromt the GRID CONTROL ***********************
                    Call SetDate()

                    mySqlCmd.Parameters.Add(New SqlParameter("@frmdt", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(ViewState("TempFrmDt"))
                    mySqlCmd.Parameters.Add(New SqlParameter("@todt", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(ViewState("TempToDt"))
                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@markuptype", SqlDbType.VarChar, 20)).Value = CType(ddlMarkuptype.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@Markupoperator", SqlDbType.VarChar, 10)).Value = CType(txtoperator.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@markupvalue", SqlDbType.Decimal)).Value = CType(Val(txtmarkup.Text), Decimal)
                    mySqlCmd.Parameters.Add(New SqlParameter("@markupvalue_child", SqlDbType.Decimal)).Value = CType(Val(txtChildMarkup.Text), Decimal)
                    mySqlCmd.Parameters.Add(New SqlParameter("@markupvalue_senior", SqlDbType.Decimal)).Value = CType(Val(txtSeniorMarkup.Text), Decimal)

                    If ViewState("pricelistState") = "New" Or ViewState("pricelistState") = "Copy" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If
                    If chkapprove.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0
                    End If
                    If txtsectorcode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@sectoryesno", SqlDbType.Int)).Value = 1
                        mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupcode", SqlDbType.Text)).Value = CType(txtsectorcode.Text, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@sectoryesno", SqlDbType.Int)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupcode", SqlDbType.Text)).Value = DBNull.Value

                    End If
                    If txtVAT.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 18, 3)).Value = txtVAT.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If
                    If chkPriceWithTax.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@CostWithTax", SqlDbType.Int)).Value = "1"
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@CostWithTax", SqlDbType.Int)).Value = "0"
                    End If

                    If txtMarkupAdultTV.Text <> "" And ddlMarkuptype.Value <> "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@AdultMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupAdultTV.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@AdultMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If
                    If txtMarkupChildTV.Text <> "" And ddlMarkuptype.Value <> "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@ChildMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupChildTV.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@ChildMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If
                    If txtMarkupSeniorTV.Text <> "" And ddlMarkuptype.Value <> "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@SeniorMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupSeniorTV.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@SeniorMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If

                    If txtMarkupAdultTV.Text <> "" And ddlMarkuptype.Value = "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@UnitMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupAdultTV.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@UnitMarkupTaxableValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If


                    If txtMarkupAdultVAT.Text <> "" And ddlMarkuptype.Value <> "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@AdultMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupAdultVAT.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@AdultMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If

                    If txtMarkupChildVAT.Text <> "" And ddlMarkuptype.Value <> "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@ChildMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupChildVAT.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@ChildMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If
                    If txtMarkupSeniorVAT.Text <> "" And ddlMarkuptype.Value <> "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@SeniorMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupSeniorVAT.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@SeniorMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If
                    If txtMarkupAdultVAT.Text <> "" And ddlMarkuptype.Value = "Unit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@UnitMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtMarkupAdultVAT.Text), Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@UnitMarkupVATValue", SqlDbType.Decimal, 18, 3)).Value = DBNull.Value
                    End If


                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd.Dispose()


                    mySqlCmd = New SqlCommand("DELETE FROM excmulticplist_countries  Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM excmulticplist_agents  Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM excmulticplist_detail  Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM excmulticplist_dates  Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()






                    '----------------------------------- Inserting Data To Dates Table
                    For Each GvRow In grdDates.Rows
                        dpFDate = GvRow.FindControl("txtfromDate")
                        dpTDate = GvRow.FindControl("txtToDate")

                        If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_excmulticplist_dates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)

                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    '----- update the header table with the min max date of dates table 
                    mySqlCmd = New SqlCommand("sp_upd_min_max_dates_excmulticplist", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()



                    '--------------------------------------------- Inserting values to detail  table--------

                    Dim k As Integer = 1
                    For Each GvRow In gv_ExRates.Rows
                        Dim txtsuppcode As TextBox = GvRow.FindControl("txtsuppcode")
                        Dim txtservicetype As TextBox = GvRow.FindControl("txtservicetype")
                        Dim txtperadult As TextBox = GvRow.FindControl("txtperadult")

                        Dim txtperchild As TextBox = GvRow.FindControl("txtperchild")
                        Dim txtpersenior As TextBox = GvRow.FindControl("txtpersenior")
                        Dim txtchildfreeupto As TextBox = GvRow.FindControl("txtchildfreeupto")

                        Dim txtperunit As TextBox = GvRow.FindControl("txtperunit")

                        If txtsuppcode.Text <> "" Then

                            mySqlCmd = New SqlCommand("sp_add_excmulticplist_detail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = k
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtsuppcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@servicedecription", SqlDbType.VarChar, 8000)).Value = CType(txtservicetype.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@peradult", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtperadult.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@perchild", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtperchild.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@persenior", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtpersenior.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@childfreeupto", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtchildfreeupto.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@perunitpax", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtperunit.Text), Decimal)

                            'Added by Abin on 20180529
                            Dim txtAdultTV As TextBox = CType(GvRow.FindControl("txtAdultTV"), TextBox)
                            Dim txtAdultVAT As TextBox = CType(GvRow.FindControl("txtAdultVAT"), TextBox)

                            Dim txtChildTV As TextBox = CType(GvRow.FindControl("txtChildTV"), TextBox)
                            Dim txtChildVAT As TextBox = CType(GvRow.FindControl("txtChildVAT"), TextBox)

                            Dim txtSeniorTV As TextBox = CType(GvRow.FindControl("txtSeniorTV"), TextBox)
                            Dim txtSeniorVAT As TextBox = CType(GvRow.FindControl("txtSeniorVAT"), TextBox)

                            Dim txtUnitTV As TextBox = CType(GvRow.FindControl("txtUnitTV"), TextBox)
                            Dim txtUnitVAT As TextBox = CType(GvRow.FindControl("txtUnitVAT"), TextBox)


                            mySqlCmd.Parameters.Add(New SqlParameter("@AdultCostTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtAdultTV.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@ChildCostTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtChildTV.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@SeniorCostTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtSeniorTV.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@UnitCostTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtUnitTV.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@AdultCostVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtAdultVAT.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@ChildCostVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtChildVAT.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@SeniorCostVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtSeniorVAT.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@UnitCostVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtUnitVAT.Text), Decimal)

                            'end


                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                            k = k + 1
                        End If
                    Next

                    If wucCountrygroup.checkcountrylist.ToString <> "" Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then


                                mySqlCmd = New SqlCommand("sp_add_excmulticplist_countries", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next

                    End If

                    If wucCountrygroup.checkagentlist.ToString <> "" Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                        For i = 0 To arragents.Length - 1

                            If arragents(i) <> "" Then

                                mySqlCmd = New SqlCommand("sp_add_excmulticplist_agents", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next
                    End If



                    '---------------------------------------------End of save/edit/copy------------------------
                ElseIf ViewState("pricelistState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    'SQL  Trans start

                    'Inserting Into Logs
                    mySqlCmd = New SqlCommand("sp_excursionmulticostpriceslist_logs", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()


                    'delete of main tbl to be written
                    mySqlCmd = New SqlCommand("sp_del_excmulticplist", mySqlConn, sqlTrans)
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
            strscript = "window.opener.__doPostBack('ExcursionwiseCostPriceListWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Excursionsuppliercostpricelist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Public Function FindduplicatePeriod() As Boolean



        Dim strMsg As String = ""

        FindduplicatePeriod = True
        Try

            '   CopyRow = 0

            Dim weekdaystr As String = ""

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing
            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            For Each gvRow1 In grdDates.Rows

                Dim txtfromdate As TextBox = gvRow1.FindControl("txtfromdate")
                Dim txttodate As TextBox = gvRow1.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    Dim ds1 As DataSet
                    Dim parms3 As New List(Of SqlParameter)
                    Dim parm3(8) As SqlParameter


                    parm3(0) = New SqlParameter("@exctypecode", CType(txtexctypecode.Text, String))
                    parm3(1) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm3(2) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm3(3) = New SqlParameter("@plistcode", CType(txtPlcCode.Value, String))
                    parm3(4) = New SqlParameter("@country", CType(Session("CountryList"), String))
                    parm3(5) = New SqlParameter("@agent", CType(Session("AgentList"), String))
                    parm3(6) = New SqlParameter("@vehiclecode", CType(txtvehiclecode.Text, String))
                    parm3(7) = New SqlParameter("@sectorcode", CType(txtsectorcode.Text, String))

                    For i = 0 To 7
                        parms3.Add(parm3(i))
                    Next

                    ds1 = New DataSet()
                    ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkexc_Multicostplist", parms3)

                    If ds1.Tables.Count > 0 Then
                        If ds1.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds1.Tables(0).Rows(0)("eplistcode")) = False Then

                                strMsg = "Excursion Multi cost Price List already exists For this  Dates   " + " - Pricelist Id " + ds1.Tables(0).Rows(0)("eplistcode") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")


                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                FindduplicatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If

                   


                End If

            Next



        Catch ex As Exception
            FindduplicatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionMulticost.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

    Protected Sub btnSecotorselect_Click(sender As Object, e As System.EventArgs) Handles btnSecotorselect.Click
        Fillsectors()
        ModalSelectsector.Show()
    End Sub

    Private Sub Fillsectors()
        Try


            Dim dt As New DataTable
            Dim strSqlQry As String = ""
            '''''''''''
            Dim myDS As New DataSet

            Label3.Text = "Sector Selection"
            strSqlQry = "SELECT distinct othtypmast.othtypcode sectorcode, othtypmast.othtypname sectorname FROM othtypmast  WHERE othgrpcode in (select option_selected from reservation_parameters where param_id=564)  and othtypmast.active=1  order by  othtypname"



            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gvSectors.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then

                gvSectors.DataBind()

            Else
                ' gvSectors.PageIndex = 0
                gvSectors.DataBind()

            End If

            ChkExistingValdSector(txtsectorcode.Text)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub btnoksector_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnoksector.Click
        Dim applies As String = ""
        Dim tickedornot As Boolean
        tickedornot = False
        ' For Each grdRow In gvSectors.Rows
        For Each grdRow As DataListItem In gvSectors.Items
            Dim chkairportSelect As CheckBox = CType(grdRow.FindControl("chksectorSelect"), CheckBox)
            chkairportSelect = grdRow.FindControl("chksectorSelect")
            If chkairportSelect.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next

        If tickedornot = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any ');", True)
            ModalSelectsector.Show()
            Exit Sub
        End If

        Dim strQry As String = ""
        Dim cnt As Integer = 0
        If (CType(ViewState("pricelistState"), String) = "Edit" Or CType(ViewState("pricelistState"), String) = "Copy") Then

            For Each grdRow As DataListItem In gvSectors.Items
                Dim chkairportSelect As CheckBox = CType(grdRow.FindControl("chksectorSelect"), CheckBox)
                Dim lblsector As Label = grdRow.FindControl("lblsector")
                Dim lblsectorname As Label = grdRow.FindControl("lblsectorname")

                If chkairportSelect.Checked = True Then
                    strQry = "select count(distinct h.exctypcode)  from view_excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
                       & "  inner join excplist_detail e on h.exctypcode=e.exccode inner join exctypes_sectorgrp ec on h.exctypcode=ec.exctypcode  where  isnull(h.sectorwiserates,'')='YES' " _
                       & " and active=1 and  ec.sectorgrpcode='" & lblsector.Text & "'     and isnull(h.multicost,'') ='NO'"
                    'and  h.classificationcode='" & txtgroupcode.Text & "' 
                    cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)

                    If cnt = 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Sector Not Assigned update Entry Excursion For this  " + lblsectorname.Text + "');", True)
                        ModalSelectsector.Show()
                        Exit Sub
                    End If
                End If
            Next

        End If


        txtsectorname.Text = ""

        '  For Each grdRow In gvSectors.Rows
        For Each grdRow As DataListItem In gvSectors.Items
            Dim chkairportSelect As CheckBox = CType(grdRow.FindControl("chksectorSelect"), CheckBox)
            Dim lblsector As Label = grdRow.FindControl("lblsector")
            Dim lblsectorname As Label = grdRow.FindControl("lblsectorname")

            If chkairportSelect.Checked = True Then
                If lblsector.Text <> "" Then
                    applies = applies + "," + lblsector.Text
                    txtsectorname.Text = txtsectorname.Text + "," + lblsectorname.Text
                End If


            End If
        Next

        If applies.Length > 0 Then
            txtsectorcode.Text = Right(applies, Len(applies) - 1)

            txtsectorname.Text = Right(txtsectorname.Text, Len(txtsectorname.Text) - 1)
        End If

        ModalSelectsector.Hide()
    End Sub
    Protected Sub txtVAT_TextChanged(sender As Object, e As System.EventArgs) Handles txtVAT.TextChanged
        TaxCalculation()

    End Sub
    Protected Sub chkPriceWithTax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkPriceWithTax.CheckedChanged
        TaxCalculation()
    End Sub

    Private Sub TaxCalculation()
        Try

            Dim vatPerc As Decimal
            If IsNumeric(txtVAT.Text) Then
                vatPerc = Convert.ToDecimal(txtVAT.Text)
            Else
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Vat Percentage can not be empty.');", True)
                'Exit Sub
                txtVAT.Text = "0"
            End If
            For Each row As GridViewRow In gv_ExRates.Rows


                '*** >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Added by abin on 28/05/2018

                Dim txtadult As TextBox = row.FindControl("txtperAdult")
                Dim txtchild As TextBox = row.FindControl("txtperChild")
                Dim txtsenior As TextBox = row.FindControl("txtperSenior")
                Dim txtunit As TextBox = row.FindControl("txtperunit")
                Dim txtchildfreeupto As TextBox = row.FindControl("txtchildfreeupto")


                Dim strAdult = txtadult.Text
                Dim txtAdultTV As TextBox = CType(row.FindControl("txtAdultTV"), TextBox)
                Dim txtAdultVAT As TextBox = CType(row.FindControl("txtAdultVAT"), TextBox)

                Dim strChild = txtchild.Text
                Dim txtChildTV As TextBox = CType(row.FindControl("txtChildTV"), TextBox)
                Dim txtChildVAT As TextBox = CType(row.FindControl("txtChildVAT"), TextBox)

                Dim strSenior = txtsenior.Text
                Dim txtSeniorTV As TextBox = CType(row.FindControl("txtSeniorTV"), TextBox)
                Dim txtSeniorVAT As TextBox = CType(row.FindControl("txtSeniorVAT"), TextBox)

                Dim strUnit = txtunit.Text
                Dim txtUnitTV As TextBox = CType(row.FindControl("txtUnitTV"), TextBox)
                Dim txtUnitVAT As TextBox = CType(row.FindControl("txtUnitVAT"), TextBox)

               


                If txtVAT.Text.Trim = "" Then
                    txtAdultTV.Text = ""
                    txtChildTV.Text = ""
                    txtSeniorTV.Text = ""
                    txtUnitTV.Text = ""



                    txtAdultVAT.Text = ""
                    txtChildVAT.Text = ""
                    txtSeniorVAT.Text = ""
                    txtUnitVAT.Text = ""
                   
                    txtMarkupAdultTV.Text = ""
                    txtMarkupAdultVAT.Text = ""
                    txtMarkupChildTV.Text = ""
                    txtMarkupChildVAT.Text = ""
                    txtMarkupSeniorTV.Text = ""
                    txtMarkupSeniorVAT.Text = ""
            

                ElseIf chkPriceWithTax.Checked = False Then
                    If txtadult.Text <> "" Then
                        txtAdultTV.Text = Math.Round(Convert.ToDecimal(txtadult.Text), 2)
                        txtAdultVAT.Text = Math.Round(Convert.ToDecimal(txtadult.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If

                    If txtchild.Text <> "" Then
                        txtChildTV.Text = Math.Round(Convert.ToDecimal(txtchild.Text), 2)
                        txtChildVAT.Text = Math.Round(Convert.ToDecimal(txtchild.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If

                    If txtsenior.Text <> "" Then
                        txtSeniorTV.Text = Math.Round(Convert.ToDecimal(txtsenior.Text), 2)
                        txtSeniorVAT.Text = Math.Round(Convert.ToDecimal(txtsenior.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If

                    If txtunit.Text <> "" Then
                        txtUnitTV.Text = Math.Round(Convert.ToDecimal(txtunit.Text), 2)
                        txtUnitVAT.Text = Math.Round(Convert.ToDecimal(txtunit.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If


                    If txtmarkup.Text <> "" Then
                        txtMarkupAdultTV.Text = Math.Round(Convert.ToDecimal(txtmarkup.Text), 2)
                        txtMarkupAdultVAT.Text = Math.Round(Convert.ToDecimal(txtmarkup.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If

                    If txtChildMarkup.Text <> "" Then
                        txtMarkupChildTV.Text = Math.Round(Convert.ToDecimal(txtChildMarkup.Text), 2)
                        txtMarkupChildVAT.Text = Math.Round(Convert.ToDecimal(txtChildMarkup.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If

                    If txtSeniorMarkup.Text <> "" Then
                        txtMarkupSeniorTV.Text = Math.Round(Convert.ToDecimal(txtSeniorMarkup.Text), 2)
                        txtMarkupSeniorVAT.Text = Math.Round(Convert.ToDecimal(txtSeniorMarkup.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If

                Else
                    If txtadult.Text <> "" Then
                        txtAdultTV.Text = Math.Round(Convert.ToDecimal(txtadult.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtAdultVAT.Text = Math.Round(Convert.ToDecimal(txtadult.Text) - Convert.ToDecimal(txtAdultTV.Text), 2)
                    End If

                    If txtchild.Text <> "" Then
                        txtChildTV.Text = Math.Round(Convert.ToDecimal(txtchild.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtChildVAT.Text = Math.Round(Convert.ToDecimal(txtchild.Text) - Convert.ToDecimal(txtChildTV.Text), 2)
                    End If

                    If txtsenior.Text <> "" Then
                        txtSeniorTV.Text = Math.Round(Convert.ToDecimal(txtsenior.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtSeniorVAT.Text = Math.Round(Convert.ToDecimal(txtsenior.Text) - Convert.ToDecimal(txtSeniorTV.Text), 2)
                    End If

                    If txtunit.Text <> "" Then
                        txtUnitTV.Text = Math.Round(Convert.ToDecimal(txtunit.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtUnitVAT.Text = Math.Round(Convert.ToDecimal(txtunit.Text) - Convert.ToDecimal(txtUnitTV.Text), 2)
                    End If

                    If txtmarkup.Text <> "" Then
                        txtMarkupAdultTV.Text = Math.Round(Convert.ToDecimal(txtmarkup.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtMarkupAdultVAT.Text = Math.Round(Convert.ToDecimal(txtmarkup.Text) - Convert.ToDecimal(txtMarkupAdultTV.Text), 2)
                    End If
                    If txtChildMarkup.Text <> "" Then
                        txtMarkupChildTV.Text = Math.Round(Convert.ToDecimal(txtChildMarkup.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtMarkupChildVAT.Text = Math.Round(Convert.ToDecimal(txtChildMarkup.Text) - Convert.ToDecimal(txtMarkupChildTV.Text), 2)
                    End If
                    If txtSeniorMarkup.Text <> "" Then
                        txtMarkupSeniorTV.Text = Math.Round(Convert.ToDecimal(txtSeniorMarkup.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtMarkupSeniorVAT.Text = Math.Round(Convert.ToDecimal(txtSeniorMarkup.Text) - Convert.ToDecimal(txtMarkupSeniorTV.Text), 2)
                    End If
                End If

                '*** <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Next
            If ddlMarkuptype.Value = "Unit" Then
                txtoperator.Disabled = True


                txtoperator.Value = "*"
                lblAdultMarkup.Text = "Markup"
                tdMarkupChild.Style.Add("display", "none")
                tdMarkupSenior.Style.Add("display", "none")
                tdMarkupChild1.Style.Add("display", "none")
                tdMarkupSenior1.Style.Add("display", "none")

            Else
                txtoperator.Disabled = True
                txtoperator.Value = "+"


                lblAdultMarkup.Text = "Markup for Adult"
                tdMarkupChild.Style.Add("display", "block")
                tdMarkupSenior.Style.Add("display", "block")
                tdMarkupChild1.Style.Add("display", "block")
                tdMarkupSenior1.Style.Add("display", "block")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
