'------------================--------------=======================------------------================
'   Module Name    :    Suppliers.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    26 June 2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class Suppliers
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim MyDS As New DataSet
    Dim MyAdapter As SqlDataAdapter
    Dim GvRow As GridViewRow
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim GVRow As GridViewRow
        Dim txt As HtmlInputText
        Dim RefCode As String
        If IsPostBack = False Then

            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            txtconnection.Value = Session("dbconnectionName")

            PanelMain.Visible = True
            PanelReservation.Visible = False
            PanelSales.Visible = False
            PanelAccount.Visible = False
            PanelRoomType.Visible = False
            PanelCategories.Visible = False
            PanelMealPlan.Visible = False
            PanelAllotment.Visible = False
            PanelOtherServices.Visible = False
            PanelGeneral.Visible = False
            PanelSpEvent.Visible = False
            PanelInfoForWEb.Visible = False
            PanelEmail.Visible = False
            '-----------------------------------------------------------------------

            '--------=======    For Main Details Fill DropDownList
            ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select * from sptypemast where active=1 order by sptypecode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where active=1 order by scatcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1 order by scatname", True)

            ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCurrency, "currcode", "select * from currmast where active=1  order by currcode", True)
            ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountry, "ctrycode", "select * from ctrymast where active=1  order by ctrycode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrCode, "currcode", "currname", "select currcode,currname from currmast where active=1  order by currcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrName, "currname", "currcode", "select currname,currcode  from currmast where active=1  order by currname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1  order by sectorcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1  order by sectorname", True)


            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", "select acctcode,acctname from acctmast where upper(controlyn)='Y' order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", "select  acctname,acctcode from acctmast where upper(controlyn)='Y'  order by acctname", True)

            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostCode, "supagentcode", "supagentname", "select * from supplier_agents where active=1  order by supagentcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostName, "supagentname", "supagentcode", "select * from supplier_agents where active=1  order by supagentname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1  order by partycode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostName, "partyname", "partycode", "select  partyname,partycode from partymast where active=1  order by partyname", True)


            ''objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)
            objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select * from othgrpmast where active=1 order by othgrpname", True)
            objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select * from othgrpmast where active=1 order by othgrpcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1  order by othgrpcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1  order by othgrpname", True)

            '-------------  Main Details    -------------------
            Numbers(txtOrder)
            charcters(txtName)
            charcters(txtCode)
            '-------------  Reservation Details    -------------------
            charcters(txtResAddress1)
            charcters(txtResAddress2)
            charcters(txtResAddress3)
            telepphone(txtResPhone1)
            telepphone(txtResPhone2)
            Numbers(txtResFax)
            charcters(txtResContact1)
            charcters(txtResContact2)
            charcters(txtResDistanceFrom)
            Numbers(txtResKM)
            'charcters(txtWeekend1)
            '-------------  Sales Details    -------------------
            telepphone(txtSaleTelephone1)
            telepphone(txtSaleTelephone2)
            charcters(txtSaleContact1)
            charcters(txtSaleContact2)
            telepphone(txtSaleFax)
            '-------------  Account Details    -------------------
            telepphone(txtAccTelephone1)
            telepphone(txtAccTelephone2)
            charcters(txtAccContact1)
            charcters(txtAccContact2)
            telepphone(txtAccFax)
            Numbers(TxtAccCreditDays)
            Numbers(txtAccCreditLimit)
            '-------------------------------------------
            ' FillRommTypePanel()
            ' FillCategorisPanel()
            ' FillMealPlanPanel()
            'FillAllotmentMarketPanel()
            'FillSpecialPanel()
            '  FillGroupPanel()
            ' FillInfoWebDisplay()


            '-----------    Room Type    ----------------------
            'No need for validation of special characters
            'charcterssrvctrl(txtRooms)
            'charcterssrvctrl(txtLocation)
            'charcterssrvctrl(txtFacilities)
            'charcterssrvctrl(txtRestaurants)
            'Numbers(txtStarNo)
            '---------------    Emai;   ----------------------------
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                txt = GVRow.FindControl("txtContactNo")
                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next

            If CType(Session("State"), String) = "New" Then
                SetFocus(txtCode)
                fillgrd(gv_Email, True)
                lblHeading.Text = "Add New Supplier"
                btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")

                BtnSaveRoomType.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnSaveCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnSaveMeal.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnSaveMarket.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnSaveOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnSaveSpecialEve.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                BtnSaveInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")


            ElseIf CType(Session("State"), String) = "Edit" Then
                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtCode)
                FillGridInEditSession()
                lblHeading.Text = "Edit Supplier"
                btnSave_Main.Text = "Update"
                BtnResSave.Text = "Update"
                BtnSaleSave.Text = "Update"
                BtnAccSave.Text = "Update"
                BtnSaveRoomType.Text = "Update"
                BtnSaveCategory.Text = "Update"
                BtnSaveMeal.Text = "Update"
                BtnSaveMarket.Text = "Update"
                BtnSaveOther.Text = "Update"
                BtnGeneralSave.Text = "Update"
                BtnSaveSpecialEve.Text = "Update"
                BtnSaveInfoWeb.Text = "Update"
                BtnEmailSave.Text = "Update"
                btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaveRoomType.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaveCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaveMeal.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaveMarket.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaveOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaveSpecialEve.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")
                BtnSaveInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update supplier?')==false)return false;")


            ElseIf CType(Session("State"), String) = "View" Then
                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                FillGridInEditSession()
                lblHeading.Text = "View Supplier"
                btnSave_Main.Visible = False
                BtnResSave.Visible = False
                BtnSaleSave.Visible = False
                BtnAccSave.Visible = False
                BtnSaveRoomType.Visible = False
                BtnSaveCategory.Visible = False
                BtnSaveMeal.Visible = False
                BtnSaveMarket.Visible = False
                BtnSaveOther.Visible = False
                BtnGeneralSave.Visible = False
                BtnSaveSpecialEve.Visible = False
                BtnSaveInfoWeb.Visible = False
                BtnEmailSave.Visible = False

            ElseIf CType(Session("State"), String) = "Delete" Then
                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                FillGridInEditSession()
                DisableControl()

                lblHeading.Text = "Delete Supplier"
                btnSave_Main.Text = "Delete"
                BtnResSave.Text = "Delete"
                BtnSaleSave.Text = "Delete"
                BtnAccSave.Text = "Delete"
                BtnSaveRoomType.Text = "Delete"
                BtnSaveCategory.Text = "Delete"
                BtnSaveMeal.Text = "Delete"
                BtnSaveMarket.Text = "Delete"
                BtnSaveOther.Text = "Delete"
                BtnGeneralSave.Text = "Delete"
                BtnSaveSpecialEve.Text = "Delete"
                BtnSaveInfoWeb.Text = "Delete"
                BtnEmailSave.Text = "Delete"
                btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete suppliert?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaveRoomType.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaveCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaveMeal.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaveMarket.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaveOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaveSpecialEve.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
                BtnSaveInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")

            End If
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                ddlCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCatName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlTName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSellingCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSectorCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSectorName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            End If
            btnCancel_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnResCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnSaleCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnAccCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnGeneralCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnEmailCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnCancelRoom.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnCancelCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnMealCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnCancelAlotment.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnCancelOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnCancelSpecailEe.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnCancelInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            'FileUpload1.Attributes.Add("onchange", "return checkFileExtension(this);")
        End If


    End Sub
    Private Sub FillGridInEditSession()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox

        Try
            '----------------   Room Type Fill Grid      ------------------
            'strSqlQry = "select * from rmtypmast where rmtypcode   in  ( select rmtypcode from partyrmtyp ) union " & _
            '            " select * from rmtypmast where rmtypcode  not in ( select rmtypcode from partyrmtyp) order by rmtypcode "
            'Dim MyRoomDS As New DataSet
            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            'MyAdapter.Fill(MyRoomDS, "rmtypmast")
            'GV_Market.DataSource = MyRoomDS
            'GV_Market.DataBind()
            'MyAdapter.Dispose()
            'mySqlConn.Close()
            'Dim arrRoomType(MyRoomDS.Tables(0).Rows.Count + 1) As String
            'Dim i As Long = 0
            'strSqlQry = "select rmtypcode from partyrmtyp"
            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            'mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            'While mySqlReader.Read = True
            '    arrRoomType(i) = mySqlReader("rmtypcode")
            '    i = i + 1
            'End While
            'MyAdapter.Dispose()
            'mySqlConn.Close()

            'i = 0
            'For i = 0 To Gv_RoomType.Rows.Count
            '    For Each GvRow In GV_Market.Rows
            '        chk = GvRow.FindControl("ChkSelect")
            '        If arrRoomType(i) = GvRow.Cells(1).Text Then
            '            chk.Checked = True
            '        End If
            '    Next
            'Next
            '-------------------------------------------------------------------------------------
            '----------------   Allotment Market Fill Grid      ------------------
            strSqlQry = "select * from plgrpmast where plgrpcode   in  ( select plgrpcode from partyallot) union " & _
                        " select * from plgrpmast where plgrpcode  not in ( select plgrpcode from partyallot) order by plgrpcode "
            Dim MyGroupDS As New DataSet
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyGroupDS, "plgrpmast")
            GV_Market.DataSource = MyGroupDS
            GV_Market.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()
            Dim arrMarket(MyGroupDS.Tables(0).Rows.Count + 1) As String
            Dim i As Long = 0
            strSqlQry = "select plgrpcode from partyallot"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While mySqlReader.Read = True
                arrMarket(i) = mySqlReader("plgrpcode")
                i = i + 1
            End While
            MyAdapter.Dispose()
            mySqlConn.Close()

            i = 0
            For i = 0 To GV_Market.Rows.Count
                For Each GvRow In GV_Market.Rows
                    chk = GvRow.FindControl("ChkSelect")
                    If arrMarket(i) = GvRow.Cells(1).Text Then
                        chk.Checked = True
                    End If
                Next
            Next
            '-------------------------------------------------------------------------------------

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
    Private Sub FillInfoWebDisplay()
        Try
            Dim dt As DataTable
            Dim dr As DataRow

            dt = New DataTable
            dt.Columns.Add(New DataColumn("Desc", GetType(String)))
            dr = dt.NewRow()
            dr(0) = "In House Doctor"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Helth Club"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Swimming Pool"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Shuttle Service"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Ball Room"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Water Sports"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Squash"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Spa Pool"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Children Pool"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Business Center"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Ayurvedic"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Bar"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Pub"
            dt.Rows.Add(dr)
            'return a DataView to the DataTable
            Gv_InfoForWeb.DataSource = dt
            Gv_InfoForWeb.DataBind()
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "charcterssrvctrl"
    Public Sub charcterssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region
    'othgrpmast

    Private Sub FillGroupPanel()
        Dim GvRow As GridViewRow
        Dim chk As CheckBox
        Dim GrupCode As String = ""
        Try
            'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupCode, "othgrpcode", "othgrpname", "select * from othgrpmast where active=1  order by othgrpcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupName, "othgrpname", "othgrpcode", "select * from othgrpmast where active=1  order by othgrpname", True)


            strSqlQry = "select * from partyothtyp where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyGroupDS As New DataSet
                strSqlQry = "select * from othgrpmast where active=1"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyGroupDS, "othgrpmast")
                GV_Group.DataSource = MyGroupDS
                GV_Group.DataBind()
                MyAdapter.Dispose()
            Else
                mySqlConn.Close()
                strSqlQry = "select * from othgrpmast where   othgrpcode   in  ( select othgrpcode from partyothgrp where partycode='" & txtCode.Value.Trim & "' ) union " & _
                          "select *  from othgrpmast where   othgrpcode not in ( select othgrpcode from partyothgrp where partycode='" & txtCode.Value.Trim & "') "
                Dim MyGroupDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyGroupDS, "othgrpmast")
                GV_Group.DataSource = MyGroupDS
                GV_Group.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrGroupType(MyGroupDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select othgrpcode from partyothgrp where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrGroupType(i) = mySqlReader("othgrpcode")
                    If GrupCode = "" Then
                        GrupCode = mySqlReader("othgrpcode")
                    Else
                        GrupCode = GrupCode & "," & mySqlReader("othgrpcode")
                    End If

                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To GV_Group.Rows.Count - 1
                    For Each GvRow In GV_Group.Rows
                        chk = GvRow.FindControl("ChkGroup")
                        If arrGroupType(i) = GvRow.Cells(1).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
                '--------------------------------------------
                'NOw Fill Other Type
                Dim MyDS_Type As New DataSet
                strSqlQry = "select * from othtypmast where othgrpcode in (" & GrupCode & ")"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Type, "othtypmast")
                GvTypes.DataSource = MyDS_Type
                GvTypes.DataBind()
                MyAdapter.Dispose()
                MyDS_Type.Clear()
                mySqlConn.Close()

                Dim chk1 As HtmlInputCheckBox

                Dim arrType(MyDS_Type.Tables(0).Rows.Count + 1) As String
                i = 0
                strSqlQry = "select othtypcode from partyothtyp where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrType(i) = mySqlReader("othtypcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()
                Try
                    i = 0
                    For i = 0 To GvTypes.Rows.Count
                        For Each GvRow In GvTypes.Rows
                            chk1 = GvRow.FindControl("ChkSelect")
                            If arrType(i) = GvRow.Cells(1).Text Then
                                chk1.Checked = True
                            End If
                        Next
                    Next

                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

                End Try


                Dim MyDS_category As New DataSet
                '--------------------------------------------------
                strSqlQry = "select * from othcatmast where othgrpcode in (" & GrupCode & ")"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_category, "othcatmast")
                Gv_Category.DataSource = MyDS_category
                Gv_Category.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()

                Dim arrCategory(MyDS_category.Tables(0).Rows.Count + 1) As String
                i = 0
                strSqlQry = "select othcatcode from partyothcat where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrCategory(i) = mySqlReader("othcatcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To Gv_Category.Rows.Count - 1
                    For Each GvRow In Gv_Category.Rows
                        chk1 = GvRow.FindControl("ChkSelect")
                        If arrCategory(i) = GvRow.Cells(1).Text Then
                            chk1.Checked = True
                        End If
                    Next
                Next
            End If
            ''---------------------------------------------------------------------=-=============



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub
    Private Sub FillOtherGroups()
        Dim GrupCode As String = ""
        Try
            objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select * from othgrpmast where active=1  order by othgrpname", True)
            objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select * from othgrpmast where active=1  order by othgrpcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupCode, "othgrpcode", "othgrpname", "select * from othgrpmast where active=1  order by othgrpcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupName, "othgrpname", "othgrpcode", "select * from othgrpmast where active=1  order by othgrpname", True)

            Dim MyGroupDS As New DataSet
            strSqlQry = "select 0 othselected,o.othgrpcode,o.othgrpname from othgrpmast o where o.active = 1 "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyGroupDS, "othgrpmast")
            GV_Group.DataSource = MyGroupDS
            GV_Group.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()


            strSqlQry = "select p.othgrpcode from partyothgrp p where p.partycode='" & txtCode.Value & "'"
            Dim gvrow As GridViewRow
            Dim chk As CheckBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While mySqlReader.Read = True
                For Each gvrow In GV_Group.Rows
                    If gvrow.Cells(1).Text = mySqlReader("othgrpcode") Then
                        chk = gvrow.FindControl("CheckBox1")
                        chk.Checked = True
                        Exit For
                    End If
                Next
            End While
            MyAdapter.Dispose()
            mySqlConn.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub

    Private Sub FillRommTypePanel()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select rmtypcode from partyrmtyp where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                If ddlType.Value <> "[Select]" Then
                    strSqlQry = "select * from rmtypmast where active=1 and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & "'"
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    MyAdapter.Fill(MyDS, "rmtypmast")
                    Gv_RoomType.DataSource = MyDS
                    Gv_RoomType.DataBind()
                    MyAdapter.Dispose()
                    mySqlConn.Close()

                End If
            Else
                mySqlConn.Close()

                'strSqlQry = "select * from rmtypmast where rmtypcode   in  ( select rmtypcode from partyrmtyp ) union " & _
                '            " select * from rmtypmast where rmtypcode  not in ( select rmtypcode from partyrmtyp) order by rmtypcode "

                strSqlQry = "select  rmtypcode, rmtypname, rankorder,active from rmtypmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "' and  rmtypcode   in  ( select rmtypcode from partyrmtyp where partycode='" & txtCode.Value.Trim & "' ) union " & _
                            "select  rmtypcode , rmtypname, rankorder,active  from rmtypmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "'  and  rmtypcode not in ( select rmtypcode from partyrmtyp where partycode='" & txtCode.Value.Trim & "') "
                Dim MyRoomDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyRoomDS, "rmtypmast")
                Gv_RoomType.DataSource = MyRoomDS
                Gv_RoomType.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrRoomType(MyRoomDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select rmtypcode from partyrmtyp where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrRoomType(i) = mySqlReader("rmtypcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To Gv_RoomType.Rows.Count
                    For Each GvRow In Gv_RoomType.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrRoomType(i) = GvRow.Cells(2).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally

        End Try
    End Sub
    Private Sub FillSpecialPanel()

        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select * from party_splevents where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                If ddlType.Value <> "[Select]" Then
                    Dim MyDS_Special As New DataSet
                    strSqlQry = "select * from spleventsmast where active=1 and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & "'"
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    MyAdapter.Fill(MyDS_Special, "spleventsmast")
                    GV_SpecialEvent.DataSource = MyDS_Special
                    GV_SpecialEvent.DataBind()
                    MyAdapter.Dispose()
                    mySqlConn.Close()
                End If

            Else
                mySqlConn.Close()

                strSqlQry = "select  * from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "' and  spleventcode   in  ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "' ) union " & _
                            "select *  from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "'  and  spleventcode not in ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "') "
                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "spleventsmast")
                GV_SpecialEvent.DataSource = MyCatDS
                GV_SpecialEvent.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select spleventcode from party_splevents where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrCategory(i) = mySqlReader("spleventcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To GV_SpecialEvent.Rows.Count
                    For Each GvRow In GV_SpecialEvent.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrCategory(i) = GvRow.Cells(1).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub
    Private Sub FillCategorisPanel()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyDS_Category As New DataSet
                strSqlQry = "select * from rmcatmast where active=1"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Category, "rmcatmast")
                Gv_Categories.DataSource = MyDS_Category
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
            Else
                mySqlConn.Close()

                strSqlQry = "select  * from rmcatmast where  rmcatcode   in  ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' ) union  " & _
                            "select  * from rmcatmast where   rmcatcode not in ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "')"
                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "rmcatmast")
                Gv_Categories.DataSource = MyCatDS
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrCategory(i) = mySqlReader("rmcatcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To Gv_Categories.Rows.Count
                    For Each GvRow In Gv_Categories.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrCategory(i) = GvRow.Cells(2).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub
    Private Sub FillMealPlanPanel()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select * from partymeal where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyDS_Category As New DataSet
                Dim MyDS_Meal As New DataSet
                strSqlQry = "select * from mealmast where active=1"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Meal, "mealcode")
                Gv_MealPlan.DataSource = MyDS_Meal
                Gv_MealPlan.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
            Else
                mySqlConn.Close()

                strSqlQry = "select  * from mealmast where  mealcode   in  ( select mealcode from partymeal where partycode='" & txtCode.Value & "' ) union  " & _
                            "select  * from mealmast where   mealcode not in ( select mealcode from partymeal where partycode='" & txtCode.Value & "')"
                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "mealcode")
                Gv_MealPlan.DataSource = MyCatDS
                Gv_MealPlan.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrMeal(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select mealcode from partymeal where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrMeal(i) = mySqlReader("mealcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To Gv_MealPlan.Rows.Count
                    For Each GvRow In Gv_MealPlan.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrMeal(i) = GvRow.Cells(2).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        Finally
            mySqlConn.Close()
        End Try
    End Sub
    Private Sub FillAllotmentMarketPanel()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select * from partyallot where partycode='" & txtCode.Value & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyDS_Market As New DataSet
                strSqlQry = "select * from plgrpmast where active=1"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Market, "plgrpmast")
                GV_Market.DataSource = MyDS_Market
                GV_Market.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
            Else

                mySqlConn.Close()
                '-------------------------------------------------------------------------------------
                '----------------   Allotment Market Fill Grid      ------------------
                strSqlQry = "select * from plgrpmast where  plgrpcode   in  ( select plgrpcode from partyallot where partycode='" & txtCode.Value & "' ) union " & _
                            " select * from plgrpmast where plgrpcode  not in ( select plgrpcode from partyallot where partycode='" & txtCode.Value & "' ) order by plgrpcode "
                Dim MyGroupDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyGroupDS, "plgrpmast")
                GV_Market.DataSource = MyGroupDS
                GV_Market.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrMarket(MyGroupDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select plgrpcode from partyallot where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrMarket(i) = mySqlReader("plgrpcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To GV_Market.Rows.Count
                    For Each GvRow In GV_Market.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrMarket(i) = GvRow.Cells(1).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
                '-------------------------------------------------------------------------------------
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
    Protected Sub BtnMainDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = True
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        ShowRecord(Session("RefCode"))
        'SetFocus(ddlTypeCode)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlType.ClientID + "');", True)

    End Sub

    Protected Sub BtnReservation_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = True
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        ShowRecord(Session("RefCode"))
        SetFocus(txtResAddress1)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResAddress1.ClientID + "');", True)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = True
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        ShowRecord(Session("RefCode"))
        'SetFocus(txtSaleTelephone1)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtSaleTelephone1.ClientID + "');", True)
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = True
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        ShowRecord(Session("RefCode"))
        'SetFocus(txtAccTelephone1)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtAccTelephone1.ClientID + "');", True)
    End Sub

    Protected Sub BtnRoomType_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = True
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        FillRommTypePanel()
        'SetFocus(Gv_RoomType)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + Gv_RoomType.ClientID + "');", True)
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = True
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        FillCategorisPanel()
        'SetFocus(Gv_Categories)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + Gv_Categories.ClientID + "');", True)
    End Sub

    Protected Sub BtnMealPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = True
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        FillMealPlanPanel()
        'SetFocus(Gv_MealPlan)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + Gv_MealPlan.ClientID + "');", True)
    End Sub

    Protected Sub BtnAllotment_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = True
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        FillAllotmentMarketPanel()
        'SetFocus(GV_Market)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + GV_Market.ClientID + "');", True)

    End Sub


    Protected Sub BtnOtherService_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = True
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        If Session("State") = "Delete" Then
            Btn_DelOthGrp.Visible = False
        End If
        'FillGroupPanel()
        FillOtherGroups()
        'SetFocus(ddlGroup)

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlGroupCode.ClientID + "');", True)
    End Sub

    Protected Sub BtnGeneral_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = True
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        '        SetFocus(txtGeneral)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtGeneral.ClientID + "');", True)
    End Sub

    Protected Sub BtnSpEvents_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = True
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = False
        FillSpecialPanel()
        'SetFocus(GV_SpecialEvent)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + GV_SpecialEvent.ClientID + "');", True)
    End Sub

    Protected Sub BtnInfoWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = True
        PanelEmail.Visible = False
        ShowRecord(Session("RefCode"))
        FillInfoWebDisplay()
        DisplayInfoForWEB()
        'SetFocus(txtRooms)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtRooms.ClientID + "');", True)
    End Sub

    Private Sub DisplayInfoForWEB()
        Dim chk As HtmlInputCheckBox
        Try

            strSqlQry = "select * from partyinfo where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = True Then
                If IsDBNull(mySqlReader("rooms")) = False Then
                    txtRooms.Text = mySqlReader("rooms")
                End If
                If IsDBNull(mySqlReader("location")) = False Then
                    txtLocation.Text = mySqlReader("location")
                End If
                If IsDBNull(mySqlReader("restaurants")) = False Then
                    txtRestaurants.Text = mySqlReader("restaurants")
                End If
                If IsDBNull(mySqlReader("facilities")) = False Then
                    txtFacilities.Text = mySqlReader("facilities")
                End If
                If IsDBNull(mySqlReader("star")) = False Then
                    ddlStarNo.SelectedValue = mySqlReader("star")
                End If



                If IsDBNull(mySqlReader("subimage1")) = False Then
                    txtimg1.Text = mySqlReader("subimage1")
                End If
                If IsDBNull(mySqlReader("subimage2")) = False Then
                    txtimg2.Text = mySqlReader("subimage2")
                End If

                If IsDBNull(mySqlReader("subimage3")) = False Then
                    txtimg3.Text = mySqlReader("subimage3")
                End If

                If IsDBNull(mySqlReader("subimage4")) = False Then
                    txtimg4.Text = mySqlReader("subimage4")
                End If
                If IsDBNull(mySqlReader("mainimage")) = False Then
                    txtimg5.Text = mySqlReader("mainimage")
                End If




                For Each GvRow In Gv_InfoForWeb.Rows

                    If GvRow.Cells(1).Text = "In House Doctor" Then
                        If mySqlReader("inhouse_doctor") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If

                    'If GvRow.Cells(1).Text = mySqlReader("Helth Club") Then
                    '    chk = GvRow.FindControl("ChkSelect")
                    '    chk.Checked = True
                    'End If
                    'If GvRow.Cells(1).Text = mySqlReader("Swimming Pool") Then
                    '    chk = GvRow.FindControl("ChkSelect")
                    '    chk.Checked = True
                    'End If
                    If GvRow.Cells(1).Text = "Shuttle Service" Then
                        If mySqlReader("shuttle_service") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If
                    If GvRow.Cells(1).Text = "Ball Room" Then
                        If mySqlReader("ball_room") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If
                    If GvRow.Cells(1).Text = "Water Sports" Then
                        If mySqlReader("water_sports") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If

                    If GvRow.Cells(1).Text = "Squash" Then
                        If mySqlReader("squash") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If
                    If GvRow.Cells(1).Text = "Spa Pool" Then
                        If mySqlReader("spa_pool") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If

                    If GvRow.Cells(1).Text = "Children Pool" Then
                        If mySqlReader("children_pool") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If
                    If GvRow.Cells(1).Text = "Business Center" Then
                        If mySqlReader("business_centre") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If
                    If GvRow.Cells(1).Text = "Ayurvedic" Then
                        If mySqlReader("ayurvedic") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If

                    If GvRow.Cells(1).Text = "Bar" Then
                        If mySqlReader("bar") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If

                    If GvRow.Cells(1).Text = "Pub" Then
                        If mySqlReader("pub") = 1 Then
                            chk = GvRow.FindControl("ChkSelect")
                            chk.Checked = True
                        End If
                    End If

                Next

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

    End Sub

    Protected Sub BtnEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = True
        FilEmail()
        'SetFocus(gv_Email)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_Email.ClientID + "');", True)
    End Sub

    Private Sub FilEmail()
        Try
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txt As HtmlInputText
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from partymast_mulltiemail Where partycode='" & CType(Session("RefCode"), String) & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from partymast_mulltiemail Where partycode='" & CType(Session("RefCode"), String) & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In gv_Email.Rows
                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("contactperson")) = False Then
                            txt = GVRow.FindControl("txtPerson")
                            txt.Value = mySqlReader("contactperson")
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txt = GVRow.FindControl("txtEmail")
                            txt.Value = mySqlReader("email")
                        End If
                        If IsDBNull(mySqlReader("contactno")) = False Then
                            txt = GVRow.FindControl("txtContactNo")
                            txt.Value = mySqlReader("contactno")
                        End If
                    End If
                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            Else
                fillgrd(gv_Email, True)
            End If
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                txt = GVRow.FindControl("txtContactNo")
                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub

    Public Sub sdf()
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccount.Visible = False
        PanelRoomType.Visible = False
        PanelCategories.Visible = False
        PanelMealPlan.Visible = False
        PanelAllotment.Visible = False
        PanelOtherServices.Visible = False
        PanelGeneral.Visible = False
        PanelSpEvent.Visible = False
        PanelInfoForWEb.Visible = False
        PanelEmail.Visible = True
    End Sub


#Region " Private Function ValidateMainDetails() As Boolean"
    Private Function ValidateMainDetails() As Boolean
        ' Dim lbl As Label
        'Dim str As String
        Try
            If txtCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
                '  ScriptManagerProxy1.FindControl("txtSuppCode").Focus()
                ' SetFocus(txtCode)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtName.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                ' ScriptManagerProxy1.FindControl("txtSuppName").Focus()
                'SetFocus(txtName)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtName.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlType.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select type code.');", True)
                'ScriptManagerProxy1.FindControl("ddlTypeCode").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlType.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlCCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select category.');", True)
                '    ScriptManagerProxy1.FindControl("ddlCategory").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlSellingCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select selling category.');", True)
                'SetFocus(ddlSelling)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSellingCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlCurrCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select currency.');", True)
                'SetFocus(ddlCurrency)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCurrCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlContCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select country.');", True)
                'SetFocus(ddlCountry)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlContCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlCityCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select city.');", True)
                'SetFocus(ddlCity)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCityCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlSectorCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select sector.');", True)
                'SetFocus(ddlSector)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSectorCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtOrder.Value.Trim <> "" Then
                If Val(txtOrder.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter order greater than zero.');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtOrder.ClientID + "');", True)
                    ValidateMainDetails = False
                    Exit Function
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter order greater than zero.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtOrder.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            ValidateMainDetails = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)


        End Try
    End Function
#End Region

    Protected Sub btnSave_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Or Session("State") = "Edit" Then
                    If ValidateMainDetails() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_partymast", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlType.Items(ddlType.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType((ddlCCode.Items(ddlCCode.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scatcode", SqlDbType.VarChar, 20)).Value = CType((ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType((ddlContCode.Items(ddlContCode.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType((ddlCityCode.Items(ddlCityCode.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType((ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    If ChkPreferred.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int)).Value = 1
                    ElseIf ChkPreferred.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@rnkorder", SqlDbType.Int)).Value = CType(Val(txtOrder.Value.Trim), Long)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    deleteuploadimage() 'Beforedeletion
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                '  Session.Add("SessionFirstCheck", "Edit")
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                    Session.Add("State", "Edit")
                    Session.Add("RefCode", txtCode.Value)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    Session.Add("State", "Edit")
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If


                ' Response.Redirect("Suppliers.aspx", False)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Function deleteuploadimage()"
    Private Function deleteuploadimage()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select mainimage ,subimage1,subimage2,subimage3,subimage4 from partyinfo Where partycode='" & txtCode.Value.Trim & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("mainimage")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("mainimage"))
                    End If
                    If IsDBNull(mySqlReader("subimage1")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\\" + mySqlReader("subimage1"))
                    End If
                    If IsDBNull(mySqlReader("subimage2")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("subimage2"))
                    End If
                    If IsDBNull(mySqlReader("subimage3")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("subimage3"))
                    End If
                    If IsDBNull(mySqlReader("subimage4")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("subimage4"))
                    End If
                End If
            End If
            'FileUpload1.PostedFile.SaveAs(Server.MapPath(".") + "//UploadImage/" + strFileName)
            'txtImg.Text = FileUploa
            Return True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("State") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "partymast", "partycode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "partymast", "partyname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("State") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "partymast", "partycode", "partyname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region


#Region " Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean"
    Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean
        Try
            Dim email1length As Integer
            email1length = Len(email.Trim)
            If email1length > 255 Then
                'objcommon.MessageBox("email1 length is too large..please enter valid email exampele(abc@abc.com)..", Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email length is too large..please enter valid email exampele(abc@abc.com).');", True)
                SetFocus(txt)
                Me.Page.SetFocus(txt)
                EmailValidate = False
                Exit Function
            Else
                Dim atpos As String
                Dim dotpos As String
                Dim s1 As String
                Dim s As String
                s1 = email
                atpos = s1.LastIndexOf("@")
                dotpos = s1.LastIndexOf(".")
                s = s1.LastIndexOf(".")
                If atpos < 1 Or dotpos < 2 Or s < 4 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                    SetFocus(txt)
                    EmailValidate = False
                    Exit Function
                Else
                    Dim sp As String()
                    Dim at As String()
                    Dim dot As String()
                    Dim chkcom As String
                    Dim chkyahoo As String
                    Dim test As String
                    Dim t As String
                    sp = s1.Split(".")
                    at = s1.Split("@")
                    chkcom = sp.GetValue(sp.Length() - 1)
                    chkyahoo = at.GetValue(at.Length() - 1)
                    dot = chkyahoo.Split(".")
                    If dot.Length() > 2 Then
                        t = dot.GetValue(dot.Length() - 3)
                        test = sp.GetValue(sp.Length() - 2)
                        If test <> "co" Or chkcom.Length() > 2 Or IsNumeric(t) = True Then
                            'objutil.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    Else
                        t = dot.GetValue(dot.Length() - 2)
                        test = sp.GetValue(sp.Length() - 1)
                        If test.Length < 2 Or IsNumeric(t) = True Or IsNumeric(test) = True Then
                            'objcommon.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    End If
                End If
            End If
            EmailValidate = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)


        End Try
    End Function

#End Region


#Region "Private Function ValidateResrvation() As Boolean"
    Private Function ValidateResrvation() As Boolean
        Try
            If txtResAddress1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Address1 field can not be blank.');", True)
                'SetFocus(txtResAddress1)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResAddress1.ClientID + "');", True)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResPhone1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Telephone1 field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResPhone1.ClientID + "');", True)
                'SetFocus(txtResPhone1)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResFax.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Fax field can not be blank.');", True)
                'SetFocus(txtResFax)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResFax.ClientID + "');", True)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResEmail.Value.Trim <> "" Then
                If EmailValidate(txtResEmail.Value.Trim, txtResEmail) = False Then
                    ValidateResrvation = False
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResEmail.ClientID + "');", True)
                    'SetFocus(txtResEmail)
                    Exit Function
                End If
            End If
            If ChkWeekend1.Checked = False And ChkWeekend2.Checked = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select either Weekend 1 or Weekend 2');", True)
                'SetFocus(txtResFax)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ChkWeekend1.ClientID + "');", True)
                ValidateResrvation = False
                Exit Function
            End If

            If Trim(txtResWebSite.Value) <> "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                'SetFocus(txtResWebSite)
                'ValidateResrvation = False
                'Exit Function
                ' Else
                Dim getstr As String
                Dim dotpos1 As String
                Dim dotpos2 As String
                getstr = Trim(txtResWebSite.Value)
                dotpos1 = getstr.LastIndexOf(".")
                dotpos2 = getstr.LastIndexOf(".")
                If dotpos1 < 1 Or dotpos2 < 2 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                    'SetFocus(txtResWebSite)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
                    ValidateResrvation = False
                    Exit Function
                Else
                    Dim laststr As String
                    Dim atposstr As String()
                    Dim getvaluestr As String
                    Dim tempstr As String
                    atposstr = getstr.Split(".")
                    If atposstr.Length < 3 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                        'SetFocus(txtResWebSite)
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
                        ValidateResrvation = False
                        Exit Function
                    ElseIf atposstr.Length = 3 Then
                        getvaluestr = atposstr.GetValue(atposstr.Length() - 3)
                        tempstr = atposstr.GetValue(atposstr.Length() - 1)
                        If getvaluestr <> "www" Or IsNumeric(tempstr) = True Or tempstr.Length() < 3 Or tempstr.Length() > 3 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            'SetFocus(txtResWebSite)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
                            ValidateResrvation = False
                            Exit Function
                        End If
                    ElseIf atposstr.Length > 3 Then
                        getvaluestr = atposstr.GetValue(atposstr.Length() - 4)
                        tempstr = atposstr.GetValue(atposstr.Length() - 2)
                        laststr = atposstr.GetValue(atposstr.Length() - 1)
                        If laststr = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            ' SetFocus(txtResWebSite)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
                            ValidateResrvation = False
                            Exit Function
                        ElseIf getvaluestr <> "www" Or IsNumeric(tempstr) = True Or IsNumeric(laststr) = True Or tempstr.Length > 2 Or laststr.Length < 2 Or laststr.Length > 2 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            'SetFocus(txtResWebSite)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
                            ValidateResrvation = False
                            Exit Function
                        End If
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
                        ValidateResrvation = False
                        Exit Function
                    End If
                End If
            End If
            ValidateResrvation = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)


        End Try
    End Function
#End Region

    Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    'objUtils.MessageBox("Please Save First Main Details.", Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    If ValidateResrvation() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateres_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updateres_partymast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 100)).Value = CType(txtResAddress1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = CType(txtResAddress2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = CType(txtResAddress3.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(txtResPhone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = CType(txtResPhone2.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(txtResFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = CType(txtResContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = CType(txtResContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtResEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@website", SqlDbType.VarChar, 200)).Value = CType(txtResWebSite.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@place", SqlDbType.VarChar, 100)).Value = CType(txtResDistanceFrom.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@kms", SqlDbType.Int, 4)).Value = CType(Val(txtResKM.Value.Trim), Long)

                    If ddlRescity.SelectedValue = "Airport" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@disttype", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlRescity.SelectedValue = "City Center" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@disttype", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@disttype", SqlDbType.VarChar, 200)).Value = DBNull.Value
                    End If
                    If ddlComunicate.SelectedValue = "Email" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commode", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlComunicate.SelectedValue = "Fax" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commode", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@commode", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    If ddlSell.SelectedValue = "Beach" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlSell.SelectedValue = "City" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If
                    If ddlAutoEmail.SelectedValue = "Yes" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@automail", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlAutoEmail.SelectedValue = "No" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@automail", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@automail", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    If ChkWeekend1.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@weekend1", SqlDbType.Int, 4)).Value = 1
                    ElseIf ChkWeekend1.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@weekend1", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@weekend1", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    If ChkWeekend2.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@weekend2", SqlDbType.Int, 4)).Value = 1
                    ElseIf ChkWeekend2.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@weekend2", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@weekend2", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


#Region "Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    If txtSaleEmail.Value.Trim <> "" Then
                        If EmailValidate(txtSaleEmail.Value.Trim, txtSaleEmail) = False Then
                            '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Fax field can not be blank.');", True)
                            Exit Sub
                        End If
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatesal_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updatesal_partymast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel1", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel2", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sfax", SqlDbType.VarChar, 50)).Value = CType(txtSaleFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact1", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact2", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@semail", SqlDbType.VarChar, 100)).Value = CType(txtSaleEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast ", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If


            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


    Protected Sub BtnAccSave_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    '-----------    Validate Page   ---------------
                    If ddlAccCode.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Control A/C Code field can not be blank.');", True)
                        Exit Sub
                    End If
                    If txtAccEmail.Value.Trim <> "" Then
                        If EmailValidate(txtAccEmail.Value.Trim, txtAccEmail) = False Then
                            SetFocus(txtAccEmail)
                            Exit Sub
                        End If
                    End If
                    '---------------------------------------------------

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_partymast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel1", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel2", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@afax", SqlDbType.VarChar, 50)).Value = CType(txtAccFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact1", SqlDbType.VarChar, 100)).Value = CType(txtAccContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact2", SqlDbType.VarChar, 100)).Value = CType(txtAccContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@aemail", SqlDbType.VarChar, 100)).Value = CType(txtAccEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crdays", SqlDbType.Int, 4)).Value = CType(Val(TxtAccCreditDays.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crlimit", SqlDbType.Int, 4)).Value = CType(Val(txtAccCreditLimit.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text.Trim, String)
                    If ChkCashSup.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashsupp", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkCashSup.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashsupp", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = CType(ddlPostCode.Items(ddlPostCode.SelectedIndex).Text, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnSaveRoomType_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp 
            Dim flag As Boolean = False
            Dim GVRow As GridViewRow
            Dim CHK As HtmlInputCheckBox
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    ' Dim chk As HtmlInputCheckBox
                    For Each GVRow In Gv_RoomType.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            flag = True
                        End If
                    Next
                    If flag = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one room type.');", True)
                        Exit Sub
                    End If
                    '---------------------------------------------------


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp    ", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                    For Each GVRow In Gv_RoomType.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyrmtyp", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(2).Text, String)
                            CHK = GVRow.FindControl("ChkInactive")
                            If CHK.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@inactive", SqlDbType.Int, 4)).Value = 1
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@inactive", SqlDbType.Int, 4)).Value = 0
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@rankord", SqlDbType.Int, 4)).Value = CType(GVRow.Cells(3).Text, Long)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next




                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnSaveCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp
            Dim flag As Boolean = False
            Dim GVRow As GridViewRow
            Dim CHK As HtmlInputCheckBox

            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    For Each GVRow In Gv_Categories.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            flag = True
                            Exit For
                        End If
                    Next
                    If flag = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one category type.');", True)
                        Exit Sub
                    End If

                    '--------------------------------------------------------------------------------
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    'sp_update_crosstab_headernew
                    mySqlCmd = New SqlCommand("sp_update_crosstab_headernew", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    For Each GVRow In Gv_Categories.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyrmcat", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(2).Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@showmain", SqlDbType.Int, 4)).Value = 0
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next



                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnSaveMeal_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp
            Dim flag As Boolean = False
            Dim GVRow As GridViewRow
            Dim CHK As HtmlInputCheckBox


            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    For Each GVRow In Gv_MealPlan.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            flag = True
                            Exit For
                        End If
                    Next
                    If flag = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one mean plan type.');", True)
                        Exit Sub
                    End If
                    '-------------------------------------------------------------------------


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                    For Each GVRow In Gv_MealPlan.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partymeal", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(2).Text, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next


                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnSaveMarket_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp
            Dim flag As Boolean = False
            Dim GVRow As GridViewRow
            Dim CHK As HtmlInputCheckBox

            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    'Dim GVRow As GridViewRow
                    'Dim CHK As HtmlInputCheckBox
                    For Each GVRow In GV_Market.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            flag = True
                            Exit For
                        End If
                    Next
                    If flag = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one market type.');", True)
                        Exit Sub
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                    For Each GVRow In GV_Market.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyallot", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next


                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updategen_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updategen_partymast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@general", SqlDbType.Text)).Value = CType(txtGeneral.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
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


#Region "Private Sub AddLines()"
    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Email.Rows.Count + 1
        Dim txt As HtmlInputText
        Dim name(count) As String
        Dim email(count) As String
        Dim contact(count) As String
        'Dim chk(count) As Boolean
        Dim n As Integer = 0
        Try
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                name(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtEmail")
                email(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtContactNo")
                contact(n) = CType(Trim(txt.Value), String)
                n = n + 1
            Next
            fillgrd(gv_Email, False, gv_Email.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_Email.Rows
                If n = i Then
                    Exit For
                End If
                'txtPerson txtEmail txtContactNo
                txt = GVRow.FindControl("txtPerson")
                txt.Value = name(n)
                txt = GVRow.FindControl("txtEmail")
                txt.Value = email(n)
                txt = GVRow.FindControl("txtContactNo")
                txt.Value = contact(n)
                n = n + 1
            Next
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                ' txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtContactNo")
                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#End Region

    Protected Sub BtnAdd_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLines()
    End Sub

    Protected Sub ChkGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'ChkGroup
        Dim GrupCode As String = ""
        Dim GVRow As GridViewRow
        Dim chk As CheckBox
        Try
            For Each GVRow In GV_Group.Rows
                chk = GVRow.FindControl("ChkGroup")
                If chk.Checked = True Then
                    If GrupCode = "" Then
                        GrupCode = " '" & GVRow.Cells(1).Text & "'"
                    Else
                        GrupCode = GrupCode & " ,'" & GVRow.Cells(1).Text & "'"
                    End If
                End If
            Next
            Dim MyDS_Type As New DataSet
            strSqlQry = "select * from othtypmast where othgrpcode in (" & GrupCode & ")"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyDS_Type, "othtypmast")
            GvTypes.DataSource = MyDS_Type
            GvTypes.DataBind()
            MyAdapter.Dispose()
            MyDS_Type.Clear()
            mySqlConn.Close()
            '--------------------------------------------------
            strSqlQry = "select * from othcatmast where othgrpcode in (" & GrupCode & ")"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyDS_Type, "othtypmast")
            Gv_Category.DataSource = MyDS_Type
            Gv_Category.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub BtnSaveSpecialEve_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp
            Dim flag As Boolean = False
            Dim GVRow As GridViewRow
            Dim CHK As HtmlInputCheckBox

            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    'Dim GVRow As GridViewRow
                    'Dim CHK As HtmlInputCheckBox
                    For Each GVRow In GV_SpecialEvent.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            flag = True
                            Exit For
                        End If
                    Next
                    If flag = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one special event type.');", True)
                        Exit Sub
                    End If
                    '-----------------------------------------------------------------------------------

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                    For Each GVRow In GV_SpecialEvent.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partysplevents", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next


                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnSaveInfoWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim strpath_logo As String = ""
            Dim strpath_logo1 As String = ""
            Dim strpath_logo2 As String = ""
            Dim strpath_logo3 As String = ""
            Dim strpath_logo4 As String = ""
            Dim strpath As String = ""
            Dim strpath1 As String = ""
            Dim strpath2 As String = ""
            Dim strpath3 As String = ""
            Dim strpath4 As String = ""
            Dim flag As Boolean = False
            Dim GvRow As GridViewRow
            Dim chk As HtmlInputCheckBox
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    For Each GvRow In Gv_InfoForWeb.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If chk.Checked = True Then
                            flag = True
                            Exit For
                        End If
                    Next
                    If flag = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one info web description.');", True)
                        Exit Sub
                    End If
                    If FileUpload5.FileName <> "" Then
                        strpath_logo = FileUpload5.FileName
                        strpath_logo = txtCode.Value & "_" & strpath_logo
                        strpath = Server.MapPath("UploadedImages\" & strpath_logo)
                        FileUpload5.PostedFile.SaveAs(strpath)
                        txtimg5.Text = strpath_logo
                    End If
                    If FileUpload1.FileName <> "" Then
                        strpath_logo1 = FileUpload1.FileName
                        strpath_logo1 = txtCode.Value & "_" & strpath_logo1
                        strpath1 = Server.MapPath("UploadedImages\" & strpath_logo1)
                        FileUpload1.PostedFile.SaveAs(strpath1)
                        txtimg1.Text = strpath_logo1
                    End If
                    If FileUpload2.FileName <> "" Then
                        strpath_logo2 = FileUpload2.FileName
                        strpath_logo2 = txtCode.Value & "_" & strpath_logo2
                        strpath2 = Server.MapPath("UploadedImages\" & strpath_logo2)
                        FileUpload2.PostedFile.SaveAs(strpath2)
                        txtimg2.Text = strpath_logo2
                    End If
                    If FileUpload3.FileName <> "" Then
                        strpath_logo3 = FileUpload3.FileName
                        strpath_logo3 = txtCode.Value & "_" & strpath_logo3
                        strpath3 = Server.MapPath("UploadedImages\" & strpath_logo3)
                        FileUpload3.PostedFile.SaveAs(strpath3)
                        txtimg3.Text = strpath_logo3
                    End If
                    If FileUpload4.FileName <> "" Then
                        strpath_logo4 = FileUpload4.FileName
                        strpath_logo4 = txtCode.Value & "_" & strpath_logo4
                        strpath4 = Server.MapPath("UploadedImages\" & strpath_logo4)
                        FileUpload4.PostedFile.SaveAs(strpath4)
                        txtimg4.Text = strpath_logo4
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_add_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rooms", SqlDbType.VarChar, 100)).Value = CType(txtRooms.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@location", SqlDbType.VarChar, 1000)).Value = CType(txtLocation.Text.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@restaurants", SqlDbType.VarChar, 100)).Value = CType(txtRestaurants.Text.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@facilities", SqlDbType.VarChar, 100)).Value = CType(txtFacilities.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@restaurants", SqlDbType.VarChar, Len(CType(txtRestaurants.Text.Trim, String)))).Value = CType(txtRestaurants.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@facilities", SqlDbType.VarChar, Len(CType(txtFacilities.Text.Trim, String)))).Value = CType(txtFacilities.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@star", SqlDbType.Int, 4)).Value = CType(ddlStarNo.SelectedValue.Trim, Long)

                    For Each GvRow In Gv_InfoForWeb.Rows
                        If GvRow.Cells(1).Text = "In House Doctor" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@inhouse_doctor", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@inhouse_doctor", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Helth Club" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                'mySqlCmd.Parameters.Add(New SqlParameter("@star", SqlDbType.Char, 4)).Value = "1"
                            Else
                                ' mySqlCmd.Parameters.Add(New SqlParameter("@star", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Swimming Pool" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                'mySqlCmd.Parameters.Add(New SqlParameter("@star", SqlDbType.Char, 4)).Value = "1"
                            Else
                                'mySqlCmd.Parameters.Add(New SqlParameter("@star", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Shuttle Service" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@shuttle_service", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@shuttle_service", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Ball Room" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@ball_room", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@ball_room", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Water Sports" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@water_sports", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@water_sports", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Squash" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@squash", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@squash", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Spa Pool" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@spa_pool", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@spa_pool", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Children Pool" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@children_pool", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@children_pool", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Business Center" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@business_centre", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@business_centre", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Ayurvedic" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@ayurvedic", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@ayurvedic", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Bar" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@bar", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@bar", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                        If GvRow.Cells(1).Text = "Pub" Then
                            chk = GvRow.FindControl("ChkSelect")
                            If chk.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@pub", SqlDbType.Char, 4)).Value = "1"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@pub", SqlDbType.Char, 4)).Value = "0"
                            End If
                        End If
                    Next
                    'mySqlCmd.Parameters.Add(New SqlParameter("@mainimage", SqlDbType.VarChar, 100)).Value = CType(strpath_logo, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@subimage1", SqlDbType.VarChar, 100)).Value = CType(strpath_logo1, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@subimage2", SqlDbType.VarChar, 100)).Value = CType(strpath_logo2, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@subimage3", SqlDbType.VarChar, 100)).Value = CType(strpath_logo3, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@subimage4", SqlDbType.VarChar, 100)).Value = CType(strpath_logo4, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@mainimage", SqlDbType.VarChar, 100)).Value = CType(txtimg5.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@subimage1", SqlDbType.VarChar, 100)).Value = CType(txtimg1.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@subimage2", SqlDbType.VarChar, 100)).Value = CType(txtimg2.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@subimage3", SqlDbType.VarChar, 100)).Value = CType(txtimg3.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@subimage4", SqlDbType.VarChar, 100)).Value = CType(txtimg4.Text, String)

                    ''catcode
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType(ddlCCode.Items(ddlCCode.SelectedIndex).Text, String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    deleteuploadimage()
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnEmailSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    If ValidateEmail() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In gv_Email.Rows
                        txtName = GvRow.FindControl("txtPerson")
                        txtEmail = GvRow.FindControl("txtEmail")
                        txtContact = GvRow.FindControl("txtContactNo")
                        If CType(txtName.Value, String) <> "" And CType(txtEmail.Value, String) <> "" And CType(txtContact.Value, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_partymast_mulltiemail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactperson", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtEmail.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactno", SqlDbType.VarChar, 50)).Value = CType(txtContact.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region " Private Function ValidateEmail() As Boolean"
    Private Function ValidateEmail() As Boolean
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GVRow As GridViewRow
        Dim FLAG As Boolean = False
        Try
            For Each GVRow In gv_Email.Rows
                txtName = GVRow.FindControl("txtPerson")
                txtEmail = GVRow.FindControl("txtEmail")
                txtContact = GVRow.FindControl("txtContactNo")
                If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                    FLAG = True
                End If
            Next

            If FLAG = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter at least one email details.');", True)
                ValidateEmail = False
                Exit Function
            Else

                For Each GVRow In gv_Email.Rows
                    txtName = GVRow.FindControl("txtPerson")
                    txtEmail = GVRow.FindControl("txtEmail")
                    txtContact = GVRow.FindControl("txtContactNo")
                    If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                        If txtName.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact Person field can not be blank.');", True)
                            'SetFocus(txtName)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtName.ClientID + "');", True)
                            ValidateEmail = False
                            Exit Function
                        End If
                        If txtEmail.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email field can not be blank.');", True)
                            'SetFocus(txtEmail)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtEmail.ClientID + "');", True)
                            ValidateEmail = False
                            Exit Function
                        Else
                            If EmailValidate(txtEmail.Value.Trim, txtEmail) = False Then
                                'SetFocus(txtEmail)
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtEmail.ClientID + "');", True)
                                ValidateEmail = False
                                Exit Function
                            End If
                        End If
                        If txtContact.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact no field can not be blank.');", True)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtContact.ClientID + "');", True)
                            'SetFocus(txtContact)
                            ValidateEmail = False
                            Exit Function
                        End If

                    End If
                Next
            End If
            ValidateEmail = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)


        End Try
    End Function
#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        Gv_RoomType.Enabled = False
        Gv_Categories.Enabled = False
        Gv_MealPlan.Enabled = False
        GV_Market.Enabled = False
        Gv_Category.Enabled = False
        GV_Group.Enabled = False
        GvTypes.Enabled = False
        GV_SpecialEvent.Enabled = False
        Gv_InfoForWeb.Enabled = False
        gv_Email.Enabled = False
        BtnAdd.Visible = False


        Me.txtCode.Disabled = True
        Me.txtName.Disabled = True
        ddlType.Disabled = True
        ddlTName.Disabled = True
        ddlCCode.Disabled = True
        ddlCatName.Disabled = True
        ddlSellingCode.Disabled = True
        ddlSellingName.Disabled = True
        ddlCurrCode.Disabled = True
        ddlCurrName.Disabled = True
        ddlContCode.Disabled = True
        ddlcontName.Disabled = True
        ddlCityCode.Disabled = True
        ddlCityName.Disabled = True
        ddlSectorCode.Disabled = True
        ddlSectorName.Disabled = True
        chkActive.Disabled = True
        ChkPreferred.Disabled = True
        txtOrder.Disabled = True
        '------------------------------------------------------
        '-------------- Reservation Details --------------------------
        txtResAddress1.Disabled = True
        txtResAddress2.Disabled = True
        txtResAddress3.Disabled = True
        txtResPhone1.Disabled = True
        txtResPhone2.Disabled = True
        txtResFax.Disabled = True
        txtResContact1.Disabled = True
        txtResContact2.Disabled = True
        txtResEmail.Disabled = True
        ddlComunicate.Enabled = False
        ddlSell.Enabled = False
        txtResWebSite.Disabled = True
        ddlRescity.Enabled = False
        txtResDistanceFrom.Disabled = True
        txtResKM.Disabled = True
        ddlAutoEmail.Enabled = False

        '------------------------END-----------------------------------
        '---------  Sales Details ------------------------------------
        txtSaleTelephone1.Disabled = True
        txtSaleTelephone2.Disabled = True
        txtSaleFax.Disabled = True
        txtSaleContact1.Disabled = True
        txtSaleContact2.Disabled = True
        txtSaleEmail.Disabled = True
        '------------------------END-----------------------------------
        '---------  Account Details ------------------------------------

        txtAccTelephone1.Disabled = True
        txtAccTelephone2.Disabled = True
        txtAccFax.Disabled = True
        txtAccContact1.Disabled = True
        txtAccContact2.Disabled = True
        txtAccEmail.Disabled = True
        TxtAccCreditDays.Disabled = True
        txtAccCreditLimit.Disabled = True
        ChkCashSup.Disabled = True
        ddlAccCode.Disabled = True
        ddlAccName.Disabled = True
        ddlPostCode.Disabled = True
        ddlPostName.Disabled = True
        ddlGroupCode.Enabled = False
        ddlGroupName.Enabled = False
        '------------------------END-----------------------------------
        '---------  General Details ------------------------------------
        txtGeneral.ReadOnly = True
        Dim GVRow As GridViewRow
        Dim txt As HtmlInputText
        For Each GVRow In gv_Email.Rows
            txt = GVRow.FindControl("txtPerson")
            txt.Disabled = True
            txt = GVRow.FindControl("txtEmail")
            txt.Disabled = True
            txt = GVRow.FindControl("txtContactNo")
            txt.Disabled = True
        Next
    End Sub
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from partymast Where partycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        Me.txtCode.Value = mySqlReader("partycode")
                    End If
                    If IsDBNull(mySqlReader("partyname")) = False Then
                        Me.txtName.Value = mySqlReader("partyname")
                    End If
                    'If IsDBNull(mySqlReader("sptypecode")) = False Then
                    '    'ddlTypeCode.SelectedItem.Text = mySqlReader("sptypecode")
                    '    'txtTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sptypemast", "sptypename", "sptypecode", ddlTypeCode.SelectedItem.Text)
                    '    ddlType.Value = mySqlReader("sptypecode")
                    '    ddlTName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sptypemast", "sptypename", "sptypecode", ddlType.Value)

                    'End If
                    ''objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCategory, "catcode", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by catcode", True)
                    ''objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSelling, "scatcode", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by scatcode", True)

                    ''---------- Main Details    -------------------------
                    'If IsDBNull(mySqlReader("catcode")) = False Then
                    '    'ddlCategory.SelectedItem.Text = mySqlReader("catcode")
                    '    'txtCategoryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"catmast", "catname", "catcode", ddlCategory.SelectedItem.Text)
                    '    ddlCCode.Value = mySqlReader("catcode")
                    '    ddlCatName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"catmast", "catname", "catcode", ddlCCode.Value)
                    'End If
                    'If IsDBNull(mySqlReader("scatcode")) = False Then
                    '    'ddlSelling.SelectedItem.Text = mySqlReader("scatcode")
                    '    'txtSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellcatmast", "scatname", "scatcode", ddlSelling.SelectedItem.Text)
                    '    ddlSellingCode.Value = mySqlReader("scatcode")
                    '    ddlSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellcatmast", "scatname", "scatcode", ddlSellingCode.Value)

                    'End If
                    'If IsDBNull(mySqlReader("currcode")) = False Then
                    '    'ddlCurrency.SelectedItem.Text = mySqlReader("currcode")
                    '    'txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", ddlCurrency.SelectedValue)
                    '    ddlCurrCode.Value = mySqlReader("currcode")
                    '    ddlCurrName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", ddlCurrCode.Value)
                    'End If
                    'If IsDBNull(mySqlReader("ctrycode")) = False Then
                    '    'ddlCountry.SelectedItem.Text = mySqlReader("ctrycode")
                    '    'txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCountry.SelectedValue)
                    '    ddlContCode.Value = mySqlReader("ctrycode")
                    '    ddlcontName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlContCode.Value)
                    'End If
                    '' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCity, "citycode", "select * from citymast where active=1  and ctrycode='" & ddlCountry.SelectedItem.Text & "' order by citycode", True)
                    'If IsDBNull(mySqlReader("citycode")) = False Then
                    '    'ddlCity.SelectedItem.Text = mySqlReader("citycode")
                    '    'txtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"citymast", "cityname", "citycode", ddlCity.SelectedItem.Text)
                    '    ddlCityCode.Value = mySqlReader("citycode")
                    '    ddlCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"citymast", "cityname", "citycode", ddlCityCode.Value)
                    'End If
                    ''objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select * from sectormaster where active=1  and citycode='" & ddlCity.SelectedItem.Text & "' order by sectorcode", True)
                    'If IsDBNull(mySqlReader("sectorcode")) = False Then
                    '    'ddlSector.SelectedItem.Text = mySqlReader("sectorcode")
                    '    'txtSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sectormaster", "sectorname", "sectorcode", ddlSector.SelectedItem.Text)
                    '    ddlSectorCode.Value = mySqlReader("sectorcode")
                    '    ddlSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sectormaster", "sectorname", "sectorcode", ddlSectorCode.Value)
                    'End If
                    If IsDBNull(mySqlReader("sptypecode")) = False Then
                        ddlTName.Value = mySqlReader("sptypecode")
                        ddlType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))
                    End If

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & " ' order by catcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1  and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & " ' order by catname", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where active=1  and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & " ' order by scatcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1  and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & " ' order by scatname", True)



                    If IsDBNull(mySqlReader("catcode")) = False Then
                        ddlCatName.Value = mySqlReader("catcode")
                        ddlCCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "catmast", "catname", "catcode", mySqlReader("catcode"))
                    End If
                    If IsDBNull(mySqlReader("scatcode")) = False Then
                        ddlSellingName.Value = mySqlReader("scatcode")
                        ddlSellingCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellcatmast", "scatname", "scatcode", mySqlReader("scatcode"))
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        ddlCurrName.Value = mySqlReader("currcode")
                        ddlCurrCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        ddlcontName.Value = mySqlReader("ctrycode")
                        ddlContCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", mySqlReader("ctrycode"))
                    End If
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "' order by citycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "'  order by cityname", True)

                    If IsDBNull(mySqlReader("citycode")) = False Then
                        ddlCityName.Value = mySqlReader("citycode")
                        ddlCityCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", mySqlReader("citycode"))
                    End If
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "' and citycode='" & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text & "'  order by sectorcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "' and citycode='" & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text & "'  order by sectorname", True)


                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        ddlSectorName.Value = mySqlReader("sectorcode")
                        ddlSectorCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sectormaster", "sectorname", "sectorcode", mySqlReader("sectorcode"))
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("preferred")) = False Then
                        If CType(mySqlReader("preferred"), String) = "1" Then
                            ChkPreferred.Checked = True
                        ElseIf CType(mySqlReader("preferred"), String) = "0" Then
                            ChkPreferred.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("rnkorder")) = False Then
                        txtOrder.Value = mySqlReader("rnkorder")
                    Else
                        txtOrder.Value = ""
                    End If
                    'preferred
                    '------------------------------------------------------
                    '-------------- Reservation Details --------------------------
                    If IsDBNull(mySqlReader("add1")) = False Then
                        txtResAddress1.Value = mySqlReader("add1")
                    Else
                        txtResAddress1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add2")) = False Then
                        txtResAddress2.Value = mySqlReader("add2")
                    Else
                        txtResAddress2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add3")) = False Then
                        txtResAddress3.Value = mySqlReader("add3")
                    Else
                        txtResAddress3.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel1")) = False Then
                        txtResPhone1.Value = mySqlReader("tel1")
                    Else
                        txtResPhone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel2")) = False Then
                        txtResPhone2.Value = mySqlReader("tel2")
                    Else
                        txtResPhone2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("fax")) = False Then
                        txtResFax.Value = mySqlReader("fax")
                    Else
                        txtResFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact1")) = False Then
                        txtResContact1.Value = mySqlReader("contact1")
                    Else
                        txtResContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact2")) = False Then
                        txtResContact2.Value = mySqlReader("contact2")
                    Else
                        txtResContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("email")) = False Then
                        txtResEmail.Value = mySqlReader("email")
                    Else
                        txtResEmail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("commode")) = False Then
                        If mySqlReader("commode") = "1" Then
                            ddlComunicate.SelectedValue = "Email"
                        ElseIf mySqlReader("commode") = "0" Then
                            ddlComunicate.SelectedValue = "Fax"
                        End If
                    End If
                    If IsDBNull(mySqlReader("selltype")) = False Then
                        If mySqlReader("selltype") = "1" Then
                            ddlSell.SelectedValue = "Beach"
                        ElseIf mySqlReader("selltype") = "0" Then
                            ddlSell.SelectedValue = "City"
                        End If
                    End If
                    If IsDBNull(mySqlReader("website")) = False Then
                        txtResWebSite.Value = mySqlReader("website")
                    Else
                        txtResWebSite.Value = ""
                    End If

                    'weekend1
                    txtWeekend1_1.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "wkfrmday1", "ctrycode", mySqlReader("ctrycode"))
                    txtWeekend1_2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "wktoday1", "ctrycode", mySqlReader("ctrycode"))
                    txtWeekend2_1.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "wkfrmday2", "ctrycode", mySqlReader("ctrycode"))
                    txtWeekend2_2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "wktoday2", "ctrycode", mySqlReader("ctrycode"))
                    If IsDBNull(mySqlReader("weekend1")) = False Then
                        If CType(mySqlReader("weekend1"), String) = "1" Then
                            ChkWeekend1.Checked = True
                        ElseIf CType(mySqlReader("weekend1"), String) = "0" Then
                            ChkWeekend1.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("weekend2")) = False Then
                        If CType(mySqlReader("weekend2"), String) = "1" Then
                            ChkWeekend2.Checked = True
                        ElseIf CType(mySqlReader("weekend2"), String) = "0" Then
                            ChkWeekend2.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("place")) = False Then
                        txtResDistanceFrom.Value = mySqlReader("place")
                    Else
                        txtResDistanceFrom.Value = ""
                    End If
                    If IsDBNull(mySqlReader("kms")) = False Then
                        txtResKM.Value = mySqlReader("kms")
                    Else
                        txtResKM.Value = ""
                    End If
                    If IsDBNull(mySqlReader("disttype")) = False Then
                        If CType(mySqlReader("disttype"), String) = "1" Then
                            ddlRescity.SelectedValue = "Airport"
                        ElseIf CType(mySqlReader("disttype"), String) = "0" Then
                            ddlRescity.SelectedValue = "City Center"
                        Else
                            ddlRescity.SelectedValue = "[Select]"
                        End If
                    Else
                        ddlRescity.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("automail")) = False Then
                        If CType(mySqlReader("automail"), String) = "1" Then
                            ddlAutoEmail.SelectedValue = "Yes"
                        ElseIf CType(mySqlReader("automail"), String) = "0" Then
                            ddlAutoEmail.SelectedValue = "No"
                        End If
                    End If

                    '------------------------END-----------------------------------
                    '---------  Sales Details ------------------------------------

                    If IsDBNull(mySqlReader("stel1")) = False Then
                        txtSaleTelephone1.Value = mySqlReader("stel1")
                    Else
                        txtSaleTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("stel2")) = False Then
                        txtSaleTelephone2.Value = mySqlReader("stel2")
                    Else
                        txtSaleTelephone2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("sfax")) = False Then
                        txtSaleFax.Value = mySqlReader("sfax")
                    Else
                        txtSaleFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact1")) = False Then
                        txtSaleContact1.Value = mySqlReader("scontact1")
                    Else
                        txtSaleContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact2")) = False Then
                        txtSaleContact2.Value = mySqlReader("scontact2")
                    Else
                        txtSaleContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("semail")) = False Then
                        txtSaleEmail.Value = mySqlReader("semail")
                    Else
                        txtSaleEmail.Value = ""
                    End If
                    '------------------------END-----------------------------------
                    '---------  Account Details ------------------------------------

                    If IsDBNull(mySqlReader("atel1")) = False Then
                        txtAccTelephone1.Value = mySqlReader("atel1")
                    Else
                        txtAccTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("atel2")) = False Then
                        txtAccTelephone2.Value = mySqlReader("atel2")
                    Else
                        txtAccTelephone2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("afax")) = False Then
                        txtAccFax.Value = mySqlReader("afax")
                    Else
                        txtAccFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact1")) = False Then
                        txtAccContact1.Value = mySqlReader("acontact1")
                    Else
                        txtAccContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact2")) = False Then
                        txtAccContact2.Value = mySqlReader("acontact2")
                    Else
                        txtAccContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("aemail")) = False Then
                        txtAccEmail.Value = mySqlReader("aemail")
                    Else
                        txtAccEmail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crdays")) = False Then
                        TxtAccCreditDays.Value = mySqlReader("crdays")
                    Else
                        TxtAccCreditDays.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crlimit")) = False Then
                        txtAccCreditLimit.Value = mySqlReader("crlimit")
                    Else
                        txtAccCreditLimit.Value = ""
                    End If

                    '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAccPostTo, "supagentcode", "select * from supplier_agents where active=1 and  supagentcode<>'" & txtCode.Value.Trim & "' and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by supagentcode", True)
                    'If IsDBNull(mySqlReader("postaccount")) = False Then
                    '    'ddlAccPostTo.SelectedItem.Text = mySqlReader("postaccount")
                    '    'txtAccPostTo2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"supplier_agents", "supagentname", "supagentcode", ddlAccPostTo.SelectedItem.Text)
                    '    ddlPostName.Value = mySqlReader("postaccount")
                    '    ddlPostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"supplier_agents", "supagentname", "supagentcode", mySqlReader("postaccount"))
                    'End If

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partycode<> '" & txtCode.Value & "' order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostName, "partyname", "partycode", "select  partyname,partycode from partymast where active=1 and partycode<> '" & txtCode.Value & "'   order by partyname", True)
                    If IsDBNull(mySqlReader("postaccount")) = False Then
                        'ddlAccPostTo.SelectedItem.Text = mySqlReader("postaccount")
                        'txtAccPostTo2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"supplier_agents", "supagentname", "supagentcode", ddlAccPostTo.SelectedItem.Text)
                        ddlPostName.Value = mySqlReader("postaccount")
                        ddlPostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("postaccount"))
                    End If


                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        ddlAccName.Value = mySqlReader("controlacctcode")
                        ddlAccCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                    Else
                        ddlAccCode.Value = "[Select]"
                        ddlAccName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("cashsupp")) = False Then
                        If mySqlReader("cashsupp") = 1 Then
                            ChkCashSup.Checked = True
                        Else
                            ChkCashSup.Checked = False
                        End If
                    End If
                    '------------------------END-----------------------------------
                    '---------  General Details ------------------------------------
                    If IsDBNull(mySqlReader("general")) = False Then
                        txtGeneral.Text = mySqlReader("general")
                    Else
                        txtGeneral.Text = ""
                    End If
                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()
            '---------------------------------------------------------------------------------
            'Dim count As Long
            'Dim GVRow As GridViewRow
            'Dim txt As HtmlInputText
            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            'mySqlCmd = New SqlCommand("Select count(*) from partymast_mulltiemail Where partycode='" & RefCode & "'", mySqlConn)
            'count = mySqlCmd.ExecuteScalar
            'mySqlCmd.Dispose()
            'mySqlConn.Close()
            'If count > 0 Then
            '    fillgrd(gv_Email, False, count)
            '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            '    mySqlCmd = New SqlCommand("Select * from partymast_mulltiemail Where partycode='" & RefCode & "'", mySqlConn)
            '    mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            '    For Each GVRow In gv_Email.Rows
            '        If mySqlReader.Read = True Then
            '            If IsDBNull(mySqlReader("contactperson")) = False Then
            '                txt = GVRow.FindControl("txtPerson")
            '                txt.Value = mySqlReader("contactperson")
            '            End If
            '            If IsDBNull(mySqlReader("email")) = False Then
            '                txt = GVRow.FindControl("txtEmail")
            '                txt.Value = mySqlReader("email")
            '            End If
            '            If IsDBNull(mySqlReader("contactno")) = False Then
            '                txt = GVRow.FindControl("txtContactNo")
            '                txt.Value = mySqlReader("contactno")
            '            End If
            '        End If
            '    Next
            '    mySqlCmd.Dispose()
            '    mySqlReader.Close()
            '    mySqlConn.Close()
            'Else
            '    fillgrd(gv_Email, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region



    Protected Sub btnCancel_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnCancelRoom_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnCancelCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnCancelAlotment_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnMealCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnCancelOther_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnCancelInfoWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnEmailCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnCancelSpecailEe_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierSearch.aspx")
    End Sub

    Protected Sub BtnSelectRommType_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_RoomType.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
        Next
    End Sub

    Protected Sub BtnDeSelectRommType_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_RoomType.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
        Next
    End Sub

    Private Function ValidateOtherType() As Boolean
        Dim chk As CheckBox
        Dim flag As Boolean = False
        Try
            'GV_Group   ChkGroup
            'GvTypes
            'For Each GvRow In GV_Group.Rows
            '    ChkServer = GvRow.FindControl("CheckBox1")
            '    If ChkServer.Checked = True Then
            '        flag = True
            '        Exit For
            '    End If
            'Next

            'If flag = False Then
            If ddlGroupCode.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one group type.');", True)
                ValidateOtherType = False
                Exit Function
            End If

            '---------------------------------------------------------------

            flag = False
            For Each GvRow In GvTypes.Rows
                chk = GvRow.FindControl("CheckBox2")
                If chk.Checked = True Then
                    flag = True
                    Exit For
                End If
            Next
            If flag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one type.');", True)
                ValidateOtherType = False
                Exit Function
            End If


            '---------------------------------------------------------------

            flag = False
            For Each GvRow In Gv_Category.Rows
                chk = GvRow.FindControl("CheckBox3")
                If chk.Checked = True Then
                    flag = True
                    Exit For
                End If
            Next
            If flag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one category.');", True)
                ValidateOtherType = False
                Exit Function
            End If
            ValidateOtherType = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
    Protected Sub BtnSaveOther_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp

            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    If ValidateOtherType() = False Then
                        Exit Sub
                    End If
                    Dim GVRow As GridViewRow
                    Dim CHK As CheckBox

                    Dim cnt_grp As Integer = 0

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------            First Delete All Records From Details Table   -----------------------------
                    mySqlCmd = New SqlCommand("sp_del_partyothgrp_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothtyp_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothcat_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("sp_add_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.ExecuteNonQuery()


                    For Each GVRow In GvTypes.Rows
                        CHK = GVRow.FindControl("CheckBox2")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyothtyp", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    '---------------        Type        ---------------------------------------

                    'sp_add_partyothtyp
                    '---------------        Type        ---------------------------------------
                    For Each GVRow In Gv_Category.Rows
                        CHK = GVRow.FindControl("CheckBox3")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyothcat", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    '-----------------------------------------------------------------------------------------
                    sqlTrans.Commit()    'SQl Tarn Commit
                    mySqlConn.Close()

                    'sp_add_partyothcat
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    sqlTrans.Commit()    'SQl Tarn Commit
                End If
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                mySqlConn.Close()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnSelectAllCategories_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_Categories.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
        Next
    End Sub

    Protected Sub BtnDeSelectAllCategories_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_Categories.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
        Next
    End Sub

    Protected Sub BtnDeSelectAllMealPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_MealPlan.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
        Next
    End Sub

    Protected Sub BtnSelectAllMealPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_MealPlan.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
        Next
    End Sub

    Protected Sub BtnSelectAllMarket_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In GV_Market.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
        Next
    End Sub

    Protected Sub BtnDeSelectAllMarket_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In GV_Market.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
        Next
    End Sub

    Protected Sub BtnSelectAllSpEvent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In GV_SpecialEvent.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
        Next
    End Sub

    Protected Sub BtnDeSelectAllSpEvent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In GV_SpecialEvent.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
        Next
    End Sub
    Private Function CheckDeleteRelationShip() As Boolean
        Try

            If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "hotels_construction", "partycode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier  is used in hotel construction ,cannot delete this supplier');", True)
                CheckDeleteRelationShip = False
                Exit Function
            End If

            If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "partycode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You can not delete this code because it is used in party Policy.');", True)
                CheckDeleteRelationShip = False
                Exit Function
            End If


            CheckDeleteRelationShip = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function


    Protected Sub ddlGroupCode_SelectedIndexChanged(Optional ByVal sender As Object = Nothing, Optional ByVal e As System.EventArgs = Nothing)
        If ddlGroupCode.SelectedValue <> "[Select]" Then
            ddlGroupName.SelectedValue = ddlGroupCode.SelectedItem.Text
            GridOthSelect(ddlGroupCode.SelectedItem.Text)
            FillOthTypCat(ddlGroupCode.SelectedItem.Text)
        Else
            'called from delete group button, so refresh the groups grid
            ddlGroupName.SelectedValue = "[Select]"
            Dim gvrow As GridViewRow
            Dim chk As CheckBox
            For Each gvrow In GV_Group.Rows
                chk = gvrow.FindControl("CheckBox1")
                chk.Checked = False
            Next

            strSqlQry = "select p.othgrpcode from partyothgrp p where p.partycode='" & txtCode.Value & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While mySqlReader.Read = True
                For Each gvrow In GV_Group.Rows
                    If gvrow.Cells(1).Text = mySqlReader("othgrpcode") Then
                        chk = gvrow.FindControl("CheckBox1")
                        chk.Checked = True
                        Exit For
                    End If
                Next
            End While
            mySqlReader.Dispose()
            mySqlConn.Close()
            'reset the other types and categories to blank
            Dim MyTypeDS As New DataSet
            Dim MyCatDS As New DataSet
            strSqlQry = "select 0 othtypselected,o.othtypcode,o.othtypname from othtypmast o  where o.active = 1 and o.othgrpcode=''"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyTypeDS, "othtypmast")
            GvTypes.DataSource = MyTypeDS
            GvTypes.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()

            strSqlQry = "select 0 othcatselected,o.othcatcode,o.othcatname from othcatmast o  where o.active = 1 and o.othgrpcode=''"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyCatDS, "othcatmast")
            Gv_Category.DataSource = MyCatDS
            Gv_Category.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()

        End If

    End Sub

    Private Sub FillOthTypCat(ByVal othgrpcode As String)
        Dim MyTypeDS As New DataSet
        Dim MyCatDS As New DataSet
        strSqlQry = "select 0 othtypselected,o.othtypcode,o.othtypname from othtypmast o  where o.active = 1 and o.othgrpcode='" & othgrpcode & "'"

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyTypeDS, "othtypmast")
        GvTypes.DataSource = MyTypeDS
        GvTypes.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()

        strSqlQry = "select p.othtypcode from partyothtyp p where p.partycode='" & txtCode.Value & "' and " _
        & " p.othgrpcode='" & othgrpcode & "'"
        Dim gvrow As GridViewRow
        Dim chk As CheckBox
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        While mySqlReader.Read = True
            For Each gvrow In GvTypes.Rows
                If gvrow.Cells(1).Text = mySqlReader("othtypcode") Then
                    chk = gvrow.FindControl("CheckBox2")
                    chk.Checked = True
                    Exit For
                End If
            Next
        End While
        mySqlReader.Dispose()
        mySqlConn.Close()

        strSqlQry = "select case when p.othcatcode is null then 0 else 1 end othcatselected,o.othcatcode,o.othcatname" _
        & " from othcatmast o left outer join partyothcat p on o.othcatcode=p.othcatcode " _
        & " where(o.active = 1 and o.othgrpcode='" & othgrpcode & "')"
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyCatDS, "othcatmast")
        Gv_Category.DataSource = MyCatDS
        Gv_Category.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()

        strSqlQry = "select p.othcatcode from partyothcat p where p.partycode='" & txtCode.Value & "' and " _
        & " p.othgrpcode='" & othgrpcode & "'"
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        While mySqlReader.Read = True
            For Each gvrow In Gv_Category.Rows
                If gvrow.Cells(1).Text = mySqlReader("othcatcode") Then
                    chk = gvrow.FindControl("CheckBox3")
                    chk.Checked = True
                    Exit For
                End If
            Next
        End While
        MyAdapter.Dispose()
        mySqlConn.Close()

    End Sub

    Protected Sub ddlGroupName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlGroupName.SelectedValue <> "[Select]" Then
            ddlGroupCode.SelectedValue = ddlGroupName.SelectedItem.Text
            FillOthTypCat(ddlGroupName.SelectedValue)
        End If

    End Sub

    Private Sub GridOthSelect(ByVal othgrpcode As String)
        Dim gvrow As GridViewRow
        Dim chk As CheckBox
        For Each gvrow In GV_Group.Rows
            If gvrow.Cells(1).Text = othgrpcode Then
                chk = gvrow.FindControl("CheckBox1")
                chk.Checked = True
                Exit For
            End If
        Next
    End Sub

    Protected Sub Btn_DelOthGrp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    If ddlGroupCode.SelectedValue = "[Select]" Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------            First Delete All Records From Details Table   -----------------------------
                    mySqlCmd = New SqlCommand("sp_del_partyothgrp_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothtyp_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothcat_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.ExecuteNonQuery()


                    '-----------------------------------------------------------------------------------------
                    sqlTrans.Commit()    'SQl Tarn Commit
                    mySqlConn.Close()
                    ddlGroupCode.SelectedValue = "[Select]"
                    ddlGroupCode_SelectedIndexChanged()
                    'sp_add_partyothcat
                End If
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierSearch.aspx", False)
                End If
            End If


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                mySqlConn.Close()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


    Protected Sub BtnimgUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim strpath_logo As String = ""
            Dim strpath_logo1 As String = ""
            Dim strpath_logo2 As String = ""
            Dim strpath_logo3 As String = ""
            Dim strpath_logo4 As String = ""
            Dim strpath As String = ""
            Dim strpath1 As String = ""
            Dim strpath2 As String = ""
            Dim strpath3 As String = ""
            Dim strpath4 As String = ""

            If FileUpload5.FileName <> "" Then
                strpath_logo = FileUpload5.FileName
                strpath_logo = txtCode.Value & "_" & strpath_logo
                strpath = Server.MapPath("UploadedImages\" & strpath_logo)
                FileUpload5.PostedFile.SaveAs(strpath)
                txtimg5.Text = strpath_logo
            End If
            If FileUpload1.FileName <> "" Then
                strpath_logo1 = FileUpload1.FileName
                strpath_logo1 = txtCode.Value & "_" & strpath_logo1
                strpath1 = Server.MapPath("UploadedImages\" & strpath_logo1)
                FileUpload1.PostedFile.SaveAs(strpath1)
                txtimg1.Text = strpath_logo1
            End If
            If FileUpload2.FileName <> "" Then
                strpath_logo2 = FileUpload2.FileName
                strpath_logo2 = txtCode.Value & "_" & strpath_logo2
                strpath2 = Server.MapPath("UploadedImages\" & strpath_logo2)
                FileUpload2.PostedFile.SaveAs(strpath2)
                txtimg2.Text = strpath_logo2
            End If
            If FileUpload3.FileName <> "" Then
                strpath_logo3 = FileUpload3.FileName
                strpath_logo3 = txtCode.Value & "_" & strpath_logo3
                strpath3 = Server.MapPath("UploadedImages\" & strpath_logo3)
                FileUpload3.PostedFile.SaveAs(strpath3)
                txtimg3.Text = strpath_logo3
            End If
            If FileUpload4.FileName <> "" Then
                strpath_logo4 = FileUpload4.FileName
                strpath_logo4 = txtCode.Value & "_" & strpath_logo4
                strpath4 = Server.MapPath("UploadedImages\" & strpath_logo4)
                FileUpload4.PostedFile.SaveAs(strpath4)
                txtimg4.Text = strpath_logo4
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


    End Sub

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "blocksale_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a BlockFullOfSales, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cancel_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a CancellationPolicy, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "child_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a ChildPolicy, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compulsory_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a CompulsoryRemarks, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compare_ratesd", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a CompetitorsRates, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplistdwknew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a PriceList of WeekEnd Rates, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a PriceList, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromotion_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a EarliBirdPromotion, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "flightmast", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a FlightMaster, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "hotels_construction", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a HotelConstruction, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "minnights_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MinimumNights, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costh", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherServiceCostPriceList, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "party_splevents", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SpecialEvents/Extras For Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyallot", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SupplierAllotment, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyinfo", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Supplier Information, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast_mulltiemail", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MultiEmail of Suppliers, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymaxaccomodation", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MaximumAccomodation Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothcat", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherService Category OF Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothgrp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherServiceGroup Of Supplier , cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothtyp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherServiceTypes of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyrmcat", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a RoomCategory Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyrmtyp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a RoomType Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Promotions, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellsph", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SellingFormulaForSupplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a GeneralPolicy, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplistd", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Details Of SpecialEvents/Extras, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SpecialEvents/Extras, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymeal", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Meal Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplistdnew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Detailsof TicketingpriceList, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplistdwknew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a  TicketingpriceListof WeekEndRates, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplisthnew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a  TicketingpriceList, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        End If

        checkForDeletion = True
    End Function
#End Region
End Class

'hotels_construction
'sparty_policy
'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'mySqlCmd = New SqlCommand("Select 't' from hotels_construction where partycode='" & txtCode.Value.Trim & "'", mySqlConn)
'mySqlReader = mySqlCmd.ExecuteReader
'If mySqlReader.Read = True Then
'    mySqlConn.Close()

'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'mySqlCmd = New SqlCommand("Select * from sparty_policy where partycode='" & txtCode.Value.Trim & "'", mySqlConn)
'mySqlReader = mySqlCmd.ExecuteReader
'If mySqlReader.Read = True Then
'    mySqlConn.Close()
'mySqlConn.Close()