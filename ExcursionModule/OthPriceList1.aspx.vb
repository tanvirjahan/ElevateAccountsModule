#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
#End Region

Partial Class OthPriceList1
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
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

   
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ViewState.Add("costtype", Request.QueryString("costtype"))
            If IsPostBack = False Then
                'Session ("OthPLFilter")
                Dim strqry As String = ""
                Dim strOption As String = ""
                Dim strtitle As String = ""
                Dim strSpType As String = ""


                'If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then

                '    strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthPListFilter"))

                '    Dim sptype As String = ""
                '    sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", strSpType)
                '    If sptype <> "" Then
                '        ddlSPTypeName.Value = sptype
                '        ddlSPType.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
                '    End If

                '    strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                '        " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

                'ElseIf Session("OthPListFilter") = "OTH" Then
                '    Dim sptypeQry As String = ""
                '    sptypeQry = "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021)"
                '    strtitle = "Other Service "
                '    strqry = "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021)"

                'End If
                grdDates.Visible = True
                fillDategrd(grdDates, True)

                ViewState.Add("pricelistState", Request.QueryString("State"))
                ViewState.Add("RefCode", Request.QueryString("RefCode"))



                If ViewState("costtype") = "N" Then
                    lblsector.Style.Add("display", "none")
                    txtsectorname.Visible = False
                    btnselect.Visible = False
                    'Else
                    '    lblsector.Style.Add("display", "block")
                    '    txtsectorname.Visible = True
                    '    btnselect.Visible = True
                End If

                If Request.QueryString("State") = "New" Then
                    Page.Title = Page.Title + " " + "New " + strtitle + " Excursion Price List"
                    lblHeading.Text = " New Excursion Price List"
                    btnsave.Style.Add("display", "none")
                    btncancel.Style.Add("display", "none")

                    txtCurrCode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =928")
                    txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", txtCurrCode.Text)
                   
                ElseIf Request.QueryString("State") = "Copy" Then
                    Page.Title = Page.Title + " " + "Copy " + strtitle + "  Excursion Price List"
                    lblHeading.Text = " Copy Excursion Price List"
                ElseIf Request.QueryString("State") = "Edit" Then
                    Page.Title = Page.Title + " " + "Edit " + strtitle + " Excursion Price List"
                    lblHeading.Text = " Edit Excursion Price List"
                ElseIf Request.QueryString("State") = "View" Then
                    Page.Title = Page.Title + " " + "View" + strtitle + "  Excursion Price List"
                    lblHeading.Text = " View Excursion Price List"
                ElseIf Request.QueryString("State") = "Delete" Then
                    Page.Title = Page.Title + " " + "Delete " + strtitle + " Excursion Price List"
                    lblHeading.Text = " Delete Excursion Price List"
                End If

                Dim s As String = ""
                
                txtconnection.Value = Session("dbconnectionName")

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othtypcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypname,othtypcode from othtypmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othtypname", True)



                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)



                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If ViewState("pricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption

                    SetFocus(ddlSPType)

                    lblHeading.Text = "Add New " + strtitle + " Price List"

                    'check, becz it gets populated if any value persists in session /qs
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("pricelistState") = "Edit" Or ViewState("pricelistState") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    'SetFocus(ddlMarketCode)
                    ShowRecord(ViewState("RefCode"))
                    lblHeading.Text = "Edit " + strtitle + " Price List"
                ElseIf ViewState("pricelistState") = "View" Then
                    ShowRecord(ViewState("RefCode"))
                ElseIf ViewState("pricelistState") = "Delete" Then
                    ShowRecord(ViewState("RefCode"))
                End If
                If ViewState("pricelistState") = "Copy" Then
                    txtPlcCode.Value = ""
                End If
                DisableAllControls()
                TextLock(txtCurrCode)
                TextLock(txtCurrName)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                  

                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If ViewState("pricelistState") = "New" Then

        ElseIf ViewState("pricelistState") = "Edit" Then

            txtApplicableTo.Enabled = True
            txtRemark.Disabled = False
            txtgroupcode.Enabled = True
            txtCurrName.Enabled = True
            txtgroupname.Enabled = True
            grdDates.Enabled = True
            grdExrates.Enabled = True
            chkapprove.Enabled = True
            txtsectorname.Enabled = True
            txtsectorcode.Enabled = True
        ElseIf ViewState("pricelistState") = "Copy" Then

            txtApplicableTo.Enabled = True
            txtRemark.Disabled = False
            txtgroupcode.Enabled = True
            txtCurrName.Enabled = True
            txtgroupname.Enabled = False
            grdDates.Enabled = True
            grdExrates.Enabled = True
            chkapprove.Enabled = True
            txtsectorname.Enabled = True
            txtsectorcode.Enabled = True

        ElseIf ViewState("pricelistState") = "Delete" Or ViewState("pricelistState") = "View" Then

            txtRemark.Disabled = True
            txtgroupcode.Enabled = False
            txtCurrName.Enabled = False
            txtgroupname.Enabled = False
            grdDates.Enabled = False
            grdExrates.Enabled = False
            chkapprove.Enabled = False
            txtsectorname.Enabled = False
            txtsectorcode.Enabled = False

        End If


    End Sub
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            Dim StrQry As String

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select h.*,c.classificationname from excplist_header h, excclassification_header c Where h.classcode=c.classificationcode and  h.eplistcode='" & RefCode & "'", mySqlConn)
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

                    If IsDBNull(mySqlReader("classcode")) = False Then
                        txtgroupcode.Text = mySqlReader("classcode")
                        txtgroupname.Text = mySqlReader("classificationname")
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
                    'If IsDBNull(mySqlReader("sectorgroupcode")) = False Then
                    '    txtsectorcode.Text = mySqlReader("sectorgroupcode")
                    '    txtsectorname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  othtypname from  othtypmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1001) and  othtypcode='" & txtsectorcode.Text & "'")
                    'End If

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
                    If IsDBNull(mySqlReader("PriceWithTax")) = False Then
                        If mySqlReader("PriceWithTax") = "1" Then
                            chkPriceWithTax.Checked = True
                        Else
                            chkPriceWithTax.Checked = False
                        End If
                    End If

                    '**** <<<<<<<<<<<<<<<<<<<<<<< Added by abin on 20180529 
                End If



            End If


        Catch ex As Exception
            objUtils.WritErrorLog("OTHPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Dim strValue As String = ""
        Try


            If txtApplicableTo.Text = "" Or txtApplicableTo.Text = " " Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable To Should not be Blank');", True)
                ValidatePage = False
                SetFocus(txtApplicableTo)
                Exit Function
            End If


            If txtgroupname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group  can not be blank.');", True)

                ValidatePage = False
                Exit Function
            End If

            If ViewState("costtype") = "S" Then

                If txtsectorname.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sector  can not be blank.');", True)

                    ValidatePage = False
                    Exit Function
                End If

            End If

            Dim tickedornot As Boolean = False

            For Each grdrow In grdDates.Rows
                Dim txtfromdate As TextBox = grdrow.findcontrol("txtfromdate")
                Dim txttodate As TextBox = grdrow.findcontrol("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                    tickedornot = True
                    Exit For
                End If

            Next

            If tickedornot = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please  Enter Dates ');", True)
                ValidatePage = False
                Exit Function
            End If

            ''''''''''' Dates Overlapping

            Dim dtdatesnew As New DataTable
            Dim dsdates As New DataSet
            Dim dr As DataRow
            Dim xmldates As String = ""




            dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
            dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))


            For Each gvRow In grdDates.Rows
                Dim txtfromdate As TextBox = gvRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = gvRow.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    dr = dtdatesnew.NewRow

                    dr("fromdate") = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                    dr("todate") = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                    dtdatesnew.Rows.Add(dr)

                End If

            Next
            dsdates.Clear()
            If dtdatesnew IsNot Nothing Then
                If dtdatesnew.Rows.Count > 0 Then
                    dsdates.Tables.Add(dtdatesnew)
                    xmldates = objUtils.GenerateXML(dsdates)
                End If
            Else
                xmldates = "<NewDataSet />"
            End If

            Dim strMsg As String = ""
            Dim ds As DataSet
            Dim parms As New List(Of SqlParameter)
            Dim parm(1) As SqlParameter

            parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))
           
            parms.Add(parm(0))

            'For i = 0 To 2
            '    parms.Add(parm(i))
            'Next

            ds = New DataSet()
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
                        strMsg = "Dates Are Overlapping Please check " + "\n"
                        For i = 0 To ds.Tables(0).Rows.Count - 1

                            strMsg += "  Date -  " + ds.Tables(0).Rows(i)("fromdateC") + "\n"
                        Next

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            End If


            '''''''''''''''''


            Dim ToDt As Date = Nothing
            Dim flgdt As Boolean = False

            For Each gvrow In grdDates.Rows
                Dim txtfromdate As TextBox = gvrow.Findcontrol("txtfromDate")
                Dim txttodate As TextBox = gvrow.Findcontrol("txtToDate")



                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    If Format(CType(txttodate.Text, Date), "yyyy/MM/dd") <= Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date should be greater than From Date ');", True)
                        txttodate.Text = ""
                        SetFocus(txttodate)
                        ValidatePage = False
                        Exit Function
                    End If

                    ToDt = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                    flgdt = True

                End If



            Next



            ValidatePage = True

        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            ValidatePage = False
        End Try

    End Function
#End Region

#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim obj As New EncryptionDecryption
        Try
            Dim myDS As New DataSet

            'If CType(ViewState("pricelistState"), String) = "New" Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'End If

            Dim strMealPlans1 As String = ""
            Dim strCondition1 As String = ""

            If ViewState("costtype") = "N" Then
                strSqlQry = "select  h.exctypcode,h.exctypname,h.ratebasis,isnull(d.seniorallowed,'') seniorallowed ,0 Adult,0 Child,0 Senior,0 Unit, 0 Half,0 FullDay from view_excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
                    & " where active=1 and classificationcode='" & txtgroupcode.Text & "' and isnull(h.sectorwiserates,'') <>'YES' and isnull(h.multicost,'') <>'YES'"
            Else

                strMealPlans1 = txtsectorcode.Text
                If strMealPlans1.Length > 0 Then
                    Dim mString1 As String() = strMealPlans1.Split(",")
                    For i As Integer = 0 To mString1.Length - 1
                        If strCondition1 = "" Then
                            strCondition1 = "'" & mString1(i) & "'"
                        Else
                            strCondition1 &= ",'" & mString1(i) & "'"
                        End If
                    Next


                End If


                strSqlQry = "select distinct   h.exctypcode,h.exctypname,h.ratebasis,d.seniorallowed,0 Adult,0 Child,0 Senior,0 Unit, 0 Half,0 FullDay from view_excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
              & " inner join  exctypes_sectorgrp es on h.exctypcode =es.exctypcode  where active=1 and h.classificationcode='" & txtgroupcode.Text & "' and es.sectorgrpcode in (" & strCondition1 & ")  and isnull(h.sectorwiserates,'') ='YES' and isnull(h.multicost,'') <>'YES'"


            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdExrates.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                btnGenerate.Style.Add("display", "none")
                btnSave.Style.Add("display", "block")
                btncancel.Style.Add("display", "block")
                grdExrates.DataBind()
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There is no records ');", True)
            

                btnGenerate.Style.Add("display", "block")
                btnSave.Style.Add("display", "none")
                btncancel.Style.Add("display", "none")

                grdExrates.PageIndex = 0
                grdExrates.DataBind()

            End If



            '*********************************          **********************      ***************************
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Public Shared Function Getgrouplist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim groupname As New List(Of String)
        Try

            strSqlQry = "select classificationname othgrpname, classificationcode othgrpcode from  excclassification_header where active=1 and classificationname like  '" & Trim(prefixText) & "%' order by classificationname "

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

                    groupname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othgrpname").ToString(), myDS.Tables(0).Rows(i)("othgrpcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return groupname
        Catch ex As Exception
            Return groupname
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
        dt.Columns.Add(New DataColumn("SeasonName", GetType(String)))
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
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("sessionRemark") = Nothing
        Session("GV_OthHotelData") = Nothing
        ViewState("RefCode") = Nothing
        ViewState("pricelistState") = Nothing
        ' Response.Redirect("OtherServicesCostPriceListSearch.aspx")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Dim strtitle As String = ""
        If IsPostBack() = False Then

            ViewState.Add("pricelistState", Request.QueryString("State"))
            ViewState.Add("RefCode", Request.QueryString("RefCode"))


            Dim supagentcode As String
            supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1021'")

            wucCountrygroup.sbSetPageState("", "EXCPLIST", CType(ViewState("pricelistState"), String))

            If CType(ViewState("pricelistState"), String) <> "New" Then
              
                wucCountrygroup.sbSetPageState(ViewState("RefCode"), Nothing, Nothing)
            End If
            hdndecimal.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters(nolock) where param_id=1140")

            If CType(ViewState("pricelistState"), String) = "New" Then



                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                txtApplicableTo.Focus()
                btnselect.Style.Add("display", "block")

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
                ' btnGenerate.Style.Add("display", "block")
                btnGenerate.Visible = False
                Page.Title = Page.Title + " " + "Copy " + strtitle + "  Excursion Price List"
                lblHeading.Text = " Copy Excursion Price List"
                '  btnselect.Style.Add("display", "none")
                TaxCalculation()
            ElseIf CType(ViewState("pricelistState"), String) = "Edit" Then

                btnSave.Text = "Update"

                RefCode = CType(ViewState("RefCode"), String)
                ShowRecord(RefCode)
                ShowDates(CType(RefCode, String))

                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                wucCountrygroup.sbShowCountry()
                '  btnGenerate_Click(sender, e)
                Showdetailsgrid(CType(RefCode, String))
                btnGenerate.Style.Add("display", "none")
                btnSave.Text = "Update"
                '  btnselect.Style.Add("display", "none")
                TaxCalculation()
            ElseIf CType(ViewState("pricelistState"), String) = "View" Then

                RefCode = CType(ViewState("RefCode"), String)
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
               
                ShowRecord(RefCode)
                ShowDates(CType(RefCode, String))
                Showdetailsgrid(CType(RefCode, String))
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                wucCountrygroup.sbShowCountry()
                btnGenerate.Style.Add("display", "none")
                btnSave.Style.Add("display", "none")
                '   btnselect.Style.Add("display", "none")
                TaxCalculation()

            ElseIf CType(ViewState("pricelistState"), String) = "Delete" Then

                RefCode = CType(ViewState("RefCode"), String)
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))

                ShowRecord(RefCode)
                ShowDates(CType(RefCode, String))
                Showdetailsgrid(CType(RefCode, String))
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                wucCountrygroup.sbShowCountry()
                btnGenerate.Style.Add("display", "none")
                btnSave.Text = "Delete"
                ' btnselect.Style.Add("display", "none")
                TaxCalculation()
            End If
            DisableAllControls()

        Else

       

        End If

    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
    End Function
    Private Sub Showdetailsgrid(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdExrates.Visible = True
            strSqlQry = ""


            Dim strQry As String = ""
            Dim cnt As Integer = 0




            If ViewState("costtype") = "N" Then
                strQry = "select  count(*) from view_excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
               & " where active=1 and classificationcode='" & txtgroupcode.Text & "' and isnull(h.sectorwiserates,'') <>'YES' and isnull(h.multicost,'') <>'YES'"
            Else
                '      strQry = "select  count(*) from view_excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
                '& " inner join  exctypes_sectorgrp es on h.exctypcode =es.exctypcode  where active=1 and h.classificationcode='" & txtgroupcode.Text & "' and es.sectorgrpcode='" & txtsectorcode.Text & "'  and isnull(h.sectorwiserates,'') ='YES' and isnull(h.multicost,'') <>'YES'"

                strQry = "select  count(distinct h.exctypcode) from view_excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
      & " inner join  exctypes_sectorgrp es on h.exctypcode =es.exctypcode  where active=1 and h.classificationcode='" & txtgroupcode.Text & "' and es.sectorgrpcode in (" & Session("sectors") & ")   and isnull(h.sectorwiserates,'') ='YES' and isnull(h.multicost,'') <>'YES'"

            End If

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)

            'fillDategrd(grdExrates, False, cnt)

            If cnt > 0 Then
           
                If ViewState("costtype") = "N" Then

                    strSqlQry = "select  * from (select  h.exctypcode,h.exctypname,h.ratebasis,isnull(d.seniorallowed,'') seniorallowed, 0 Adult,0 Child,0 Senior,0 Unit, 0 Half,0 FullDay from view_excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode  " _
                       & "  where active=1 and classificationcode='" & txtgroupcode.Text & "' and isnull(h.sectorwiserates,'') <>'YES'  and isnull(h.multicost,'') <>'YES'  and h.exctypcode not in (select exccode from excplist_detail where eplistcode='" & RefCode & "') union all " _
                       & "  select  h.exctypcode,h.exctypname,h.ratebasis,isnull(d.seniorallowed,'') seniorallowed ,   max(case when a.adttype ='Adult' then round(a.price,2) else 0 end) Adult,  	max(case when a.adttype ='Child' then a.price else 0 end) Child,  " _
                       & " max(case when a.adttype ='Senior' then a.price else 0 end) Senior, max(case when a.adttype ='Unit' then a.price else 0 end) Unit, max(case when a.adttype ='Half' then a.price else 0 end) Half, max(case when a.adttype ='Full' then a.price else 0 end) FullDay  " _
                       & "  from excplist_header eh, excplist_detail a inner join  view_excursiontypes h (nolock) on a.exccode=h.exctypcode left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode   where  eh.eplistcode=a.eplistcode and isnull(eh.sectoryesno,0)=0 and active=1 and classificationcode='" & txtgroupcode.Text & "'  and " _
                       & " isnull(h.sectorwiserates,'') <>'YES' and isnull(h.multicost,'') <>'YES' and a.eplistcode='" & RefCode & "' group by h.exctypcode,h.exctypname,h.ratebasis,d.seniorallowed) rs  order by  rs.exctypcode "
                Else

                    strSqlQry = "select distinct * from (select  h.exctypcode,h.exctypname,h.ratebasis,isnull(d.seniorallowed,'') seniorallowed, 0 Adult,0 Child,0 Senior,0 Unit, 0 Half,0 FullDay from view_excursiontypes h (nolock)   " _
                      & " left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode  inner join  exctypes_sectorgrp es on h.exctypcode =es.exctypcode " _
                    & "  where active=1 and classificationcode='" & txtgroupcode.Text & "' and es.sectorgrpcode in (" & Session("sectors") & ")   and isnull(h.sectorwiserates,'') ='YES'  and isnull(h.multicost,'') <>'YES'  and h.exctypcode not in (select exccode from excplist_detail where eplistcode='" & RefCode & "') union all " _
                    & "  select  h.exctypcode,h.exctypname,h.ratebasis,isnull(d.seniorallowed,'') seniorallowed ,   max(case when a.adttype ='Adult' then round(a.price,2) else 0 end) Adult,  	max(case when a.adttype ='Child' then a.price else 0 end) Child,  " _
                    & " max(case when a.adttype ='Senior' then a.price else 0 end) Senior, max(case when a.adttype ='Unit' then a.price else 0 end) Unit, max(case when a.adttype ='Half' then a.price else 0 end) Half, max(case when a.adttype ='Full' then a.price else 0 end) FullDay  " _
                    & "  from excplist_header eh cross apply dbo.splitallotmkt(eh.sectorgroupcode,',') esc, excplist_detail a inner join  view_excursiontypes h (nolock) on a.exccode=h.exctypcode left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode   where eh.eplistcode=a.eplistcode and isnull(eh.sectoryesno,0)=1  and   esc.mktcode in (" & Session("sectors") & ") and active=1 and classificationcode='" & txtgroupcode.Text & "'  and " _
                    & " isnull(h.sectorwiserates,'') ='YES' and isnull(h.multicost,'') <>'YES' and a.eplistcode='" & RefCode & "' group by h.exctypcode,h.exctypname,h.ratebasis,d.seniorallowed) rs  order by  rs.exctypcode "

                    'strSqlQry = "select  * from (select  h.exctypcode,h.exctypname,h.ratebasis,isnull(d.seniorallowed,'') seniorallowed, 0 Adult,0 Child,0 Senior,0 Unit, 0 Half,0 FullDay from view_excursiontypes h (nolock)   " _
                    '    & " left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode  inner join  exctypes_sectorgrp es on h.exctypcode =es.exctypcode " _
                    '  & "  where active=1 and classificationcode='" & txtgroupcode.Text & "' and es.sectorgrpcode='" & txtsectorcode.Text & "'   and isnull(h.sectorwiserates,'') ='YES'  and isnull(h.multicost,'') <>'YES'  and h.exctypcode not in (select exccode from excplist_detail where eplistcode='" & RefCode & "') union all " _
                    '  & "  select  h.exctypcode,h.exctypname,h.ratebasis,isnull(d.seniorallowed,'') seniorallowed ,   max(case when a.adttype ='Adult' then round(a.price,2) else 0 end) Adult,  	max(case when a.adttype ='Child' then a.price else 0 end) Child,  " _
                    '  & " max(case when a.adttype ='Senior' then a.price else 0 end) Senior, max(case when a.adttype ='Unit' then a.price else 0 end) Unit, max(case when a.adttype ='Half' then a.price else 0 end) Half, max(case when a.adttype ='Full' then a.price else 0 end) FullDay  " _
                    '  & "  from excplist_header eh, excplist_detail a inner join  view_excursiontypes h (nolock) on a.exccode=h.exctypcode left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode   where eh.eplistcode=a.eplistcode and isnull(eh.sectoryesno,0)=1  and isnull(eh.sectorgroupcode,0)='" & txtsectorcode.Text & "' and active=1 and classificationcode='" & txtgroupcode.Text & "'  and " _
                    '  & " isnull(h.sectorwiserates,'') ='YES' and isnull(h.multicost,'') <>'YES' and a.eplistcode='" & RefCode & "' group by h.exctypcode,h.exctypname,h.ratebasis,d.seniorallowed) rs  order by  rs.exctypcode "


                End If


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS)
                grdExrates.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then

                    grdExrates.DataBind()
                Else

                    grdExrates.PageIndex = 0
                    grdExrates.DataBind()

                End If



                'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                'mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                'mySqlReader = mySqlCmd.ExecuteReader



                'For Each GvRow In grdExrates.Rows

                '   Dim lblexctypecode As Label = GvRow.FindControl("lblexctypecode")
                '    Dim txtadult As TextBox = GvRow.FindControl("txtadult")
                '    Dim txtchild As TextBox = GvRow.FindControl("txtchild")
                '    Dim txtsenior As TextBox = GvRow.FindControl("txtsenior")
                '    Dim txtunit As TextBox = GvRow.FindControl("txtunit")
                '    Dim txthalf As TextBox = GvRow.FindControl("txthalf")
                '    Dim txtfull As TextBox = GvRow.FindControl("txtfull")
                '    Dim lblratebasis As Label = GvRow.Findcontrol("lblratebasis")
                '    Dim lblsenioryn As Label = GvRow.Findcontrol("lblsenioryn")

                '    If mySqlReader.Read = True Then
                '        If lblexctypecode.Text = mySqlReader("exccode") Then

                '            If IsDBNull(mySqlReader("Adult")) = False Then
                '                txtadult.Text = IIf(mySqlReader("Adult") = 0, "", DecRound(mySqlReader("Adult")))

                '            End If

                '            If IsDBNull(mySqlReader("Child")) = False Then
                '                txtchild.Text = IIf(mySqlReader("Child") = 0, "", DecRound(mySqlReader("Child")))

                '            End If
                '            If IsDBNull(mySqlReader("Senior")) = False Then
                '                txtsenior.Text = IIf(mySqlReader("Senior") = 0, "", DecRound(mySqlReader("Senior")))

                '            End If
                '            If IsDBNull(mySqlReader("Unit")) = False Then
                '                txtunit.Text = IIf(mySqlReader("Unit") = 0, "", DecRound(mySqlReader("Unit")))

                '            End If
                '            If IsDBNull(mySqlReader("Half")) = False Then
                '                txthalf.Text = IIf(mySqlReader("Half") = 0, "", DecRound(mySqlReader("Half")))

                '            End If
                '            If IsDBNull(mySqlReader("FullDay")) = False Then
                '                txtfull.Text = IIf(mySqlReader("FullDay") = 0, "", DecRound(mySqlReader("FullDay")))

                '            End If

                '        End If

                '    End If
                'Next


                'mySqlReader.Close()
                'mySqlConn.Close()
            End If




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Othpricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


            strSqlQry = "select count( distinct frmdate) from excplist_dates(nolock) where eplistcode='" & RefCode & "'" '" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            mySqlCmd = New SqlCommand("Select * from excplist_dates(nolock) Where eplistcode='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("costtype") = "N" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcPriceListEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcSectorPriceEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
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
#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
    Protected Sub grdExrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdExrates.RowDataBound

        'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
        '    Dim strGridName As String = grdExrates.ClientID
        '    Dim strRowId As String = e.Row.RowIndex
        '    Dim strFoucsColumnIndex = "2"
        '    e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
        '    e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        'End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblratebasis As Label = e.Row.FindControl("lblratebasis")
            Dim lblexctypecode As Label = e.Row.FindControl("lblexctypecode")
            Dim txtsenior As TextBox = e.Row.FindControl("txtsenior")
            Dim txtunit As TextBox = e.Row.FindControl("txtunit")
            Dim txthalf As TextBox = e.Row.FindControl("txthalf")
            Dim txtfull As TextBox = e.Row.FindControl("txtfull")
            Dim txtchild As TextBox = e.Row.FindControl("txtchild")
            Dim txtadult As TextBox = e.Row.FindControl("txtadult")
            Dim lblsenioryn As Label = e.Row.FindControl("lblsenioryn")

            Dim dtrow As DataRow = GridViewRowToDataRow(CType(e.Row, GridViewRow))

            txtadult.Text = IIf(dtrow("adult") = 0, "", DecRound(dtrow("adult")))
            txtchild.Text = IIf(dtrow("child") = 0, "", DecRound(dtrow("child")))
            txtfull.Text = IIf(dtrow("fullday") = 0, "", DecRound(dtrow("fullday")))
            txthalf.Text = IIf(dtrow("half") = 0, "", DecRound(dtrow("half")))
            txtunit.Text = IIf(dtrow("unit") = 0, "", DecRound(dtrow("unit")))
            txtsenior.Text = IIf(dtrow("senior") = 0, "", DecRound(dtrow("senior")))

            Numberssrvctrl(txtadult)
            Numberssrvctrl(txtchild)
            Numberssrvctrl(txtfull)
            Numberssrvctrl(txthalf)
            Numberssrvctrl(txtunit)
            Numberssrvctrl(txtsenior)

            If lblratebasis.Text = "ACS" Then
                txtfull.Enabled = False
                txtunit.Enabled = False
                txthalf.Enabled = False
                txtadult.Enabled = True
                txtchild.Enabled = True
                txtsenior.Enabled = True
            ElseIf lblratebasis.Text = "HALF" Then
                txtfull.Enabled = False
                txtunit.Enabled = False
                txthalf.Enabled = True
                txtadult.Enabled = False
                txtchild.Enabled = False
                txtsenior.Enabled = False
            ElseIf lblratebasis.Text = "FULL" Then
                txtfull.Enabled = True
                txtunit.Enabled = False
                txthalf.Enabled = False
                txtadult.Enabled = False
                txtchild.Enabled = False
                txtsenior.Enabled = False
            ElseIf lblratebasis.Text = "UNIT" Then
                txtfull.Enabled = False
                txtunit.Enabled = True
                txthalf.Enabled = False
                txtadult.Enabled = False
                txtchild.Enabled = False
                txtsenior.Enabled = False
            End If

            If lblsenioryn.Text = "Yes" And lblratebasis.Text = "ACS" Then
                txtsenior.Enabled = True
            Else
                txtsenior.Enabled = False
            End If

            '*** >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Added by abin on 28/05/2018

            Dim strAdult = "'" + txtadult.ClientID + "'"
            Dim txtAdultTV As TextBox = CType(e.Row.FindControl("txtAdultTV"), TextBox)
            strAdult = strAdult + ",'" + txtAdultTV.ClientID + "'"
            Dim txtAdultVAT As TextBox = CType(e.Row.FindControl("txtAdultVAT"), TextBox)
            strAdult = strAdult + ",'" + txtAdultVAT.ClientID + "'"
            txtadult.Attributes.Add("onblur", "CalculateTax(" & strAdult & ")")

            Dim strChild = "'" + txtchild.ClientID + "'"
            Dim txtChildTV As TextBox = CType(e.Row.FindControl("txtChildTV"), TextBox)
            strChild = strChild + ",'" + txtChildTV.ClientID + "'"
            Dim txtChildVAT As TextBox = CType(e.Row.FindControl("txtChildVAT"), TextBox)
            strChild = strChild + ",'" + txtChildVAT.ClientID + "'"
            txtchild.Attributes.Add("onblur", "CalculateTax(" & strChild & ")")

            Dim strSenior = "'" + txtsenior.ClientID + "'"
            Dim txtSeniorTV As TextBox = CType(e.Row.FindControl("txtSeniorTV"), TextBox)
            strSenior = strSenior + ",'" + txtSeniorTV.ClientID + "'"
            Dim txtSeniorVAT As TextBox = CType(e.Row.FindControl("txtSeniorVAT"), TextBox)
            strSenior = strSenior + ",'" + txtSeniorVAT.ClientID + "'"
            txtsenior.Attributes.Add("onblur", "CalculateTax(" & strSenior & ")")

            Dim strUnit = "'" + txtunit.ClientID + "'"
            Dim txtUnitTV As TextBox = CType(e.Row.FindControl("txtUnitTV"), TextBox)
            strUnit = strUnit + ",'" + txtUnitTV.ClientID + "'"
            Dim txtUnitVAT As TextBox = CType(e.Row.FindControl("txtUnitVAT"), TextBox)
            strUnit = strUnit + ",'" + txtUnitVAT.ClientID + "'"
            txtunit.Attributes.Add("onblur", "CalculateTax(" & strUnit & ")")
            ' txtunit.Attributes.Add("onblur", "CalculateTax()")

            Dim strHalf = "'" + txthalf.ClientID + "'"
            Dim txtHalfTV As TextBox = CType(e.Row.FindControl("txtHalfTV"), TextBox)
            strHalf = strHalf + ",'" + txtHalfTV.ClientID + "'"
            Dim txtHalfVAT As TextBox = CType(e.Row.FindControl("txtHalfVAT"), TextBox)
            strHalf = strHalf + ",'" + txtHalfVAT.ClientID + "'"
            txthalf.Attributes.Add("onblur", "CalculateTax(" & strHalf & ")")

            Dim strFull = "'" + txtfull.ClientID + "'"
            Dim txtFullTV As TextBox = CType(e.Row.FindControl("txtFullTV"), TextBox)
            strFull = strFull + ",'" + txtFullTV.ClientID + "'"
            Dim txtFullVAT As TextBox = CType(e.Row.FindControl("txtFullVAT"), TextBox)
            strFull = strFull + ",'" + txtFullVAT.ClientID + "'"
            txtfull.Attributes.Add("onblur", "CalculateTax(" & strFull & ")")

            '*** <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        End If
    End Sub

    Private Function GridViewRowToDataRow(ByVal gvr As GridViewRow) As DataRow
        Dim di As Object = Nothing
        Dim drv As DataRowView = Nothing
        Dim dr As DataRow = Nothing

        If gvr IsNot Nothing Then
            di = TryCast(gvr.DataItem, System.Object)
            If di IsNot Nothing Then
                drv = TryCast(di, System.Data.DataRowView)
                If drv IsNot Nothing Then
                    dr = TryCast(drv.Row, System.Data.DataRow)
                End If
            End If
        End If

        Return dr
    End Function

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
    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow

        If txtApplicableTo.Text = "" Or txtApplicableTo.Text = " " Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable To Should not be Blank');", True)
            ValidateSave = False
            SetFocus(txtApplicableTo)
            Exit Function
        End If

        If grdExrates.Rows.Count = 0 And (ViewState("pricelistState") = "New" Or ViewState("pricelistState") = "Edit" Or ViewState("pricelistState") = "Copy") Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Generate the Records');", True)
            ValidateSave = False
            Exit Function
        End If


        Dim ratesexists As Boolean = False
        For Each gvRow In grdExrates.Rows
            Dim lblexctypecode As Label = gvRow.FindControl("lblexctypecode")
            Dim txtadult As TextBox = gvRow.FindControl("txtadult")
            Dim txtchild As TextBox = gvRow.FindControl("txtchild")
            Dim txtsenior As TextBox = gvRow.FindControl("txtsenior")
            Dim txtunit As TextBox = gvRow.FindControl("txtunit")
            Dim txthalf As TextBox = gvRow.FindControl("txthalf")
            Dim txtfull As TextBox = gvRow.FindControl("txtfull")

            If ((Val(txtadult.Text) <> 0 And (Val(txtchild.Text) = 0 Or Val(txtsenior.Text) = 0)) Or Val(txtunit.Text) = 0 Or Val(txthalf.Text) = 0 Or Val(txtfull.Text) = 0) Then
                ratesexists = True
                Exit For
            End If

        Next

        If ratesexists = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Rates for One Excursions');", True)
            ValidateSave = False
            Exit Function
        End If


      


        ValidateSave = True

    End Function

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

                  

                    For Each gvRow In grdExrates.Rows

                        Dim ds1 As DataSet
                        Dim parms3 As New List(Of SqlParameter)
                        Dim parm3(13) As SqlParameter


                        Dim lblexctypecode As Label = gvRow.FindControl("lblexctypecode")
                        Dim txtadult As TextBox = gvRow.FindControl("txtadult")
                        Dim txtchild As TextBox = gvRow.FindControl("txtchild")
                        Dim txtsenior As TextBox = gvRow.FindControl("txtsenior")
                        Dim txtunit As TextBox = gvRow.FindControl("txtunit")
                        Dim txthalf As TextBox = gvRow.FindControl("txthalf")
                        Dim txtfull As TextBox = gvRow.FindControl("txtfull")

                        parm3(0) = New SqlParameter("@exctypecode", CType(lblexctypecode.Text, String))
                        parm3(1) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                        parm3(2) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                        parm3(3) = New SqlParameter("@plistcode", CType(txtPlcCode.Value, String))
                        parm3(4) = New SqlParameter("@country", CType(Session("CountryList"), String))
                        parm3(5) = New SqlParameter("@agent", CType(Session("AgentList"), String))
                        parm3(6) = New SqlParameter("@adulttype", IIf(Val(txtadult.Text) <> 0, UCase("Adult"), ""))
                        parm3(7) = New SqlParameter("@childtype", IIf(Val(txtchild.Text) <> 0, UCase("Child"), ""))
                        parm3(8) = New SqlParameter("@seniortype", IIf(Val(txtsenior.Text) <> 0, UCase("Senior"), ""))
                        parm3(9) = New SqlParameter("@unittype", IIf(Val(txtunit.Text) <> 0, UCase("UNIT"), ""))
                        parm3(10) = New SqlParameter("@halftype", IIf(Val(txthalf.Text) <> 0, UCase("Half"), ""))
                        parm3(11) = New SqlParameter("@fulltype", IIf(Val(txtfull.Text) <> 0, UCase("Full"), ""))
                        parm3(12) = New SqlParameter("@sectorcode", CType(txtsectorcode.Text, String))


                        For i = 0 To 12
                            parms3.Add(parm3(i))
                        Next

                        ds1 = New DataSet()
                        If ViewState("costtype") = "N" Then
                            ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkexcplist", parms3)
                        Else
                            ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkexcplist_sector", parms3)
                        End If



                        If ds1.Tables.Count > 0 Then
                            If ds1.Tables(0).Rows.Count > 0 Then
                                If IsDBNull(ds1.Tables(0).Rows(0)("eplistcode")) = False Then

                                    strMsg = "Excursion Price List already exists For this Market " + ds1.Tables(0).Rows(0)("applicableto") + " and   Dates   " + " - Pricelist Id " + ds1.Tables(0).Rows(0)("eplistcode") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")


                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                    FindduplicatePeriod = False
                                    Exit Function
                                End If
                            End If
                        End If
                    Next


                End If

            Next



        Catch ex As Exception
            FindduplicatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
#End Region
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

                        If ViewState("costtype") = "N" Then
                            optionName = "EXCPLIST"
                        Else
                            optionName = "EXCSPLIST"
                        End If
                        optionval = objUtils.GetAutoDocNo(optionName, mySqlConn, sqlTrans)
                        txtPlcCode.Value = optionval.Trim


                        mySqlCmd = New SqlCommand("sp_add_excPListh", mySqlConn, sqlTrans)
                    ElseIf ViewState("pricelistState") = "Edit" Then
                        'Inserting Into Logs
                        mySqlCmd = New SqlCommand("sp_excursionpriceslist_logs", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("sp_mod_excplisthnew", mySqlConn, sqlTrans)
                    End If

                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@eplistcd", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)


                        mySqlCmd.Parameters.Add(New SqlParameter("@esellcd", SqlDbType.VarChar, 20)).Value = DBNull.Value

                        mySqlCmd.Parameters.Add(New SqlParameter("@classcode", SqlDbType.VarChar, 20)).Value = CType(txtgroupcode.Text.Trim, String)
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
                        mySqlCmd.Parameters.Add(New SqlParameter("@PriceWithTax", SqlDbType.Int)).Value = "1"
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@PriceWithTax", SqlDbType.Int)).Value = "0"
                    End If
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()


                        mySqlCmd = New SqlCommand("DELETE FROM excplist_detail  Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("DELETE FROM excplist_dates  Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()


                        mySqlCmd = New SqlCommand("DELETE FROM excplist_countries  Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("DELETE FROM excplist_agents Where eplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
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



                        '--------------------------------------------- Inserting values to detail  table--------

                        Dim k As Integer = 1
                        For Each GvRow In grdExrates.Rows
                            Dim lblexctypecode As Label = GvRow.FindControl("lblexctypecode")
                            Dim txtadult As TextBox = GvRow.FindControl("txtadult")
                            Dim txtchild As TextBox = GvRow.FindControl("txtchild")
                            Dim txtsenior As TextBox = GvRow.FindControl("txtsenior")
                            Dim txtunit As TextBox = GvRow.FindControl("txtunit")
                            Dim txthalf As TextBox = GvRow.FindControl("txthalf")
                            Dim txtfull As TextBox = GvRow.FindControl("txtfull")

                       

                            mySqlCmd = New SqlCommand("sp_add_excplistCostd", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = k
                            mySqlCmd.Parameters.Add(New SqlParameter("@exccode", SqlDbType.VarChar, 20)).Value = CType(lblexctypecode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@adultprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtadult.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@childprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtchild.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@seniorprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtsenior.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@unitprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtunit.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@halfprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txthalf.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@fullprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtfull.Text), Decimal)

                        'Added by Abin on 20180529
                        Dim txtAdultTV As TextBox = CType(GvRow.FindControl("txtAdultTV"), TextBox)
                        Dim txtAdultVAT As TextBox = CType(GvRow.FindControl("txtAdultVAT"), TextBox)

                        Dim txtChildTV As TextBox = CType(GvRow.FindControl("txtChildTV"), TextBox)
                        Dim txtChildVAT As TextBox = CType(GvRow.FindControl("txtChildVAT"), TextBox)

                        Dim txtSeniorTV As TextBox = CType(GvRow.FindControl("txtSeniorTV"), TextBox)
                        Dim txtSeniorVAT As TextBox = CType(GvRow.FindControl("txtSeniorVAT"), TextBox)

                        Dim txtUnitTV As TextBox = CType(GvRow.FindControl("txtUnitTV"), TextBox)
                        Dim txtUnitVAT As TextBox = CType(GvRow.FindControl("txtUnitVAT"), TextBox)

                        Dim txtHalfTV As TextBox = CType(GvRow.FindControl("txtHalfTV"), TextBox)
                        Dim txtHalfVAT As TextBox = CType(GvRow.FindControl("txtHalfVAT"), TextBox)

                        Dim txtFullTV As TextBox = CType(GvRow.FindControl("txtFullTV"), TextBox)
                        Dim txtFullVAT As TextBox = CType(GvRow.FindControl("txtFullVAT"), TextBox)

                        mySqlCmd.Parameters.Add(New SqlParameter("@AdultTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtAdultTV.Text), Decimal)
                        mySqlCmd.Parameters.Add(New SqlParameter("@AdultVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtAdultVAT.Text), Decimal)

                        mySqlCmd.Parameters.Add(New SqlParameter("@ChildTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtChildTV.Text), Decimal)
                        mySqlCmd.Parameters.Add(New SqlParameter("@ChildVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtChildVAT.Text), Decimal)

                        mySqlCmd.Parameters.Add(New SqlParameter("@SeniorTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtSeniorTV.Text), Decimal)
                        mySqlCmd.Parameters.Add(New SqlParameter("@SeniorVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtSeniorVAT.Text), Decimal)

                        mySqlCmd.Parameters.Add(New SqlParameter("@UnitTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtUnitTV.Text), Decimal)
                        mySqlCmd.Parameters.Add(New SqlParameter("@UnitVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtUnitVAT.Text), Decimal)

                        mySqlCmd.Parameters.Add(New SqlParameter("@halfTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtHalfTV.Text), Decimal)
                        mySqlCmd.Parameters.Add(New SqlParameter("@halfVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtHalfVAT.Text), Decimal)

                        mySqlCmd.Parameters.Add(New SqlParameter("@FullTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtFullTV.Text), Decimal)
                        mySqlCmd.Parameters.Add(New SqlParameter("@FullVATValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtFullVAT.Text), Decimal) 'end

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                            k = k + 6
                        Next

                        If wucCountrygroup.checkcountrylist.ToString <> "" Then

                            ''Value in hdn variable , so splting to get string correctly
                            Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                            For i = 0 To arrcountry.Length - 1

                                If arrcountry(i) <> "" Then


                                    mySqlCmd = New SqlCommand("sp_add_excplist_countries", mySqlConn, sqlTrans)
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

                                    mySqlCmd = New SqlCommand("sp_add_excplist_agents", mySqlConn, sqlTrans)
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
                        mySqlCmd = New SqlCommand("sp_excursionpriceslist_logs", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()


                        'delete of main tbl to be written
                        mySqlCmd = New SqlCommand("sp_del_excplisth", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@eplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                    End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OthPriceListWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getsectorlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim sectorname As New List(Of String)
        Try

            strSqlQry = "select othtypcode,othtypname  from  othtypmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1001) and  othtypname like  '" & Trim(prefixText) & "%' order by othtypname "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    sectorname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othtypname").ToString(), myDS.Tables(0).Rows(i)("othtypcode").ToString()))

                Next

            End If

            Return sectorname
        Catch ex As Exception
            Return sectorname
        End Try

    End Function

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Protected Sub btncopyrates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyrates.Click
        For Each GvRow In grdExrates.Rows

            Dim txtadult As TextBox = GvRow.FindControl("txtadult")
            Dim txtsenior As TextBox = GvRow.FindControl("txtsenior")


            Dim txtAdultTV As TextBox = CType(GvRow.FindControl("txtAdultTV"), TextBox)
            Dim txtAdultVAT As TextBox = CType(GvRow.FindControl("txtAdultVAT"), TextBox)
            Dim txtSeniorTV As TextBox = CType(GvRow.FindControl("txtSeniorTV"), TextBox)
            Dim txtSeniorVAT As TextBox = CType(GvRow.FindControl("txtSeniorVAT"), TextBox)

            If txtsenior.Enabled = True Then
                txtsenior.Text = txtadult.Text
                txtSeniorTV.Text = txtAdultTV.Text
                txtSeniorVAT.Text = txtAdultVAT.Text
            End If




        
        Next
    End Sub

    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselect.Click
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

            ChkExistingValdiscount(txtsectorcode.Text)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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

    'Protected Sub gvSectors_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSectors.PageIndexChanging
    '    '  gvSectors.PageIndex = e.NewPageIndex
    '    Fillsectors()
    '    ModalSelectsector.Show()
    'End Sub

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
                       & " and active=1 and  ec.sectorgrpcode='" & lblsector.Text & "'  and  h.classificationcode='" & txtgroupcode.Text & "'    and isnull(h.multicost,'') ='NO'"

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
            For Each row As GridViewRow In grdExrates.Rows


                '*** >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Added by abin on 28/05/2018

                Dim txtadult As TextBox = row.FindControl("txtadult")
                Dim txtchild As TextBox = row.FindControl("txtchild")
                Dim txtsenior As TextBox = row.FindControl("txtsenior")
                Dim txtunit As TextBox = row.FindControl("txtunit")
                Dim txthalf As TextBox = row.FindControl("txthalf")
                Dim txtfull As TextBox = row.FindControl("txtfull")

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

                Dim strHalf = txthalf.Text
                Dim txtHalfTV As TextBox = CType(row.FindControl("txtHalfTV"), TextBox)
                Dim txtHalfVAT As TextBox = CType(row.FindControl("txtHalfVAT"), TextBox)

                Dim strFull = txtfull.Text
                Dim txtFullTV As TextBox = CType(row.FindControl("txtFullTV"), TextBox)
                Dim txtFullVAT As TextBox = CType(row.FindControl("txtFullVAT"), TextBox)

                If txtVAT.Text.Trim = "" Then
                    txtAdultTV.Text = ""
                    txtChildTV.Text = ""
                    txtSeniorTV.Text = ""
                    txtUnitTV.Text = ""
                    txtHalfTV.Text = ""
                    txtFullTV.Text = ""

                    txtAdultVAT.Text = ""
                    txtChildVAT.Text = ""
                    txtSeniorVAT.Text = ""
                    txtUnitVAT.Text = ""
                    txtHalfVAT.Text = ""
                    txtFullVAT.Text = ""

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

                    If txthalf.Text <> "" Then
                        txtHalfTV.Text = Math.Round(Convert.ToDecimal(txthalf.Text), 2)
                        txtHalfVAT.Text = Math.Round(Convert.ToDecimal(txthalf.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If

                    If txtfull.Text <> "" Then
                        txtFullTV.Text = Math.Round(Convert.ToDecimal(txtfull.Text), 2)
                        txtFullVAT.Text = Math.Round(Convert.ToDecimal(txtfull.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
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

                    If txthalf.Text <> "" Then
                        txtHalfTV.Text = Math.Round(Convert.ToDecimal(txthalf.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtHalfVAT.Text = Math.Round(Convert.ToDecimal(txthalf.Text) - Convert.ToDecimal(txtHalfTV.Text), 2)
                    End If

                    If txtfull.Text <> "" Then
                        txtFullTV.Text = Math.Round(Convert.ToDecimal(txtfull.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtFullVAT.Text = Math.Round(Convert.ToDecimal(txtfull.Text) - Convert.ToDecimal(txtFullTV.Text), 2)
                    End If
                End If

                '*** <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Next

        Catch ex As Exception

        End Try
    End Sub

End Class
