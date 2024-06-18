Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Net
Imports System.Collections

Partial Class CustomerWhiteLabelling
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim tt As String = ""
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                tt = System.AppDomain.CurrentDomain.BaseDirectory
                ViewState.Add("State", Request.QueryString("State"))
                ViewState.Add("RefCode", Request.QueryString("RefCode"))
                chkactive.Checked = True
                loadoperator()
                AddAttributes()
                If ViewState("State") = "New" Then
                    SetFocus(txtmhotel)
                    lblHeading.Text = "Add New Customer White Labelling"
                    btnSave.Text = "Save"
                    DisableControl()
                    ShowRecord(CType(ViewState("RefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save ?')==false)return false;")
                ElseIf ViewState("State") = "Edit" Then
                    SetFocus(txtmhotel)
                    lblHeading.Text = "Edit  Customer White Labelling"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("RefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update ?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")

                ElseIf ViewState("State") = "View" Then
                    SetFocus(txtmhotel)
                    lblHeading.Text = "View  Customer White Labelling"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Search"
                    DisableControl()
                    btnSave.Visible = False
                    ShowRecord(CType(ViewState("RefCode"), String))

                ElseIf ViewState("State") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete  Customer White Labelling"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("RefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete?')==false)return false;")
                    ' btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                'objUtils.WritErrorLog("UploadBannerAdds.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            If txtmhotel.Text.Trim <> "" And ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Value <> "[Select]" Then
                txthotelstr.Text = ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmhotel.Text))
            End If
            If txtmtansfer.Text.Trim <> "" And ddltransferopr.Items(ddltransferopr.SelectedIndex).Value <> "[Select]" Then
                txttransferstr.Text = ddltransferopr.Items(ddltransferopr.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmtansfer.Text))
            End If

            If txtmcar.Text.Trim <> "" And ddlcaropr.Items(ddlcaropr.SelectedIndex).Value <> "[Select]" Then
                txtcarstr.Text = ddlcaropr.Items(ddlcaropr.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmcar.Text))
            End If
            If txtmvisa.Text.Trim <> "" And ddlvisa.Items(ddlvisa.SelectedIndex).Value <> "[Select]" Then
                txtvisastr.Text = ddlvisa.Items(ddlvisa.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmvisa.Text))
            End If
            If txtmexcursion.Text.Trim <> "" And ddlexcursion.Items(ddlexcursion.SelectedIndex).Value <> "[Select]" Then
                txtexcursionstr.Text = ddlexcursion.Items(ddlexcursion.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmexcursion.Text))
            End If
            If txtmguide.Text.Trim <> "" And ddlguide.Items(ddlguide.SelectedIndex).Value <> "[Select]" Then
                txtgudiestr.Text = ddlguide.Items(ddlguide.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmguide.Text))
            End If
            If txtmentrance.Text.Trim <> "" And ddlentrance.Items(ddlentrance.SelectedIndex).Value <> "[Select]" Then
                txtentrancestr.Text = ddlentrance.Items(ddlentrance.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmentrance.Text))
            End If


            If txtmjeep.Text.Trim <> "" And ddljeep.Items(ddljeep.SelectedIndex).Value <> "[Select]" Then
                txtjeepstr.Text = ddljeep.Items(ddljeep.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmjeep.Text))
            End If
            If txtmmeal.Text.Trim <> "" And ddlmeal.Items(ddlmeal.SelectedIndex).Value <> "[Select]" Then
                txtmealstr.Text = ddlmeal.Items(ddlmeal.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmmeal.Text))
            End If

            If txtmothers.Text.Trim <> "" And ddlothers.Items(ddlothers.SelectedIndex).Value <> "[Select]" Then
                txtothersstr.Text = ddlothers.Items(ddlothers.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmothers.Text))
            End If
            If txtmairmeet.Text.Trim <> "" And ddlairmeet.Items(ddlairmeet.SelectedIndex).Value <> "[Select]" Then
                txtairmeetstr.Text = ddlairmeet.Items(ddlairmeet.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmairmeet.Text))
            End If

            If txtmhandlingfee.Text.Trim <> "" And ddlhandlingfee.Items(ddlhandlingfee.SelectedIndex).Value <> "[Select]" Then
                txthandlingfeestr.Text = ddlhandlingfee.Items(ddlhotelopr.SelectedIndex).Value & String.Format("{0:0.0000}", Val(txtmhandlingfee.Text))
            End If
            

        End If
    End Sub
#End Region

    Private Sub AddAttributes()
        Try
            txtmhotel.Attributes.Add("onchange", "GetSellingStr(" + txtmhotel.ClientID + "," + ddlhotelopr.ClientID + "," + txthotelstr.ClientID + ");")
            txtmcar.Attributes.Add("onchange", "GetSellingStr(" + txtmcar.ClientID + "," + ddlcaropr.ClientID + "," + txtcarstr.ClientID + ");")
            txtmexcursion.Attributes.Add("onchange", "GetSellingStr(" + txtmexcursion.ClientID + "," + ddlexcursion.ClientID + "," + txtexcursionstr.ClientID + ");")
            txtmentrance.Attributes.Add("onchange", "GetSellingStr(" + txtmentrance.ClientID + "," + ddlentrance.ClientID + "," + txtentrancestr.ClientID + ");")
            txtmmeal.Attributes.Add("onchange", "GetSellingStr(" + txtmmeal.ClientID + "," + ddlmeal.ClientID + "," + txtmealstr.ClientID + ");")
            txtmairmeet.Attributes.Add("onchange", "GetSellingStr(" + txtmairmeet.ClientID + "," + ddlairmeet.ClientID + "," + txtairmeetstr.ClientID + ");")
            txtmtansfer.Attributes.Add("onchange", "GetSellingStr(" + txtmtansfer.ClientID + "," + ddltransferopr.ClientID + "," + txttransferstr.ClientID + ");")
            txtmvisa.Attributes.Add("onchange", "GetSellingStr(" + txtmvisa.ClientID + "," + ddlvisa.ClientID + "," + txtvisastr.ClientID + ");")
            txtmguide.Attributes.Add("onchange", "GetSellingStr(" + txtmguide.ClientID + "," + ddlguide.ClientID + "," + txtgudiestr.ClientID + ");")
            txtmjeep.Attributes.Add("onchange", "GetSellingStr(" + txtmjeep.ClientID + "," + ddljeep.ClientID + "," + txtjeepstr.ClientID + ");")
            txtmothers.Attributes.Add("onchange", "GetSellingStr(" + txtmothers.ClientID + "," + ddlothers.ClientID + "," + txtothersstr.ClientID + ");")
            txtmhandlingfee.Attributes.Add("onchange", "GetSellingStr(" + txtmhandlingfee.ClientID + "," + ddlhandlingfee.ClientID + "," + txthandlingfeestr.ClientID + ");")

            ddlhotelopr.Attributes.Add("onchange", "GetSellingStr(" + txtmhotel.ClientID + "," + ddlhotelopr.ClientID + "," + txthotelstr.ClientID + ");")
            ddlcaropr.Attributes.Add("onchange", "GetSellingStr(" + txtmcar.ClientID + "," + ddlcaropr.ClientID + "," + txtcarstr.ClientID + ");")
            ddlexcursion.Attributes.Add("onchange", "GetSellingStr(" + txtmexcursion.ClientID + "," + ddlexcursion.ClientID + "," + txtexcursionstr.ClientID + ");")
            ddlentrance.Attributes.Add("onchange", "GetSellingStr(" + txtmentrance.ClientID + "," + ddlentrance.ClientID + "," + txtentrancestr.ClientID + ");")
            ddlmeal.Attributes.Add("onchange", "GetSellingStr(" + txtmmeal.ClientID + "," + ddlmeal.ClientID + "," + txtmealstr.ClientID + ");")
            ddlairmeet.Attributes.Add("onchange", "GetSellingStr(" + txtmairmeet.ClientID + "," + ddlairmeet.ClientID + "," + txtairmeetstr.ClientID + ");")
            ddltransferopr.Attributes.Add("onchange", "GetSellingStr(" + txtmtansfer.ClientID + "," + ddltransferopr.ClientID + "," + txttransferstr.ClientID + ");")
            ddlvisa.Attributes.Add("onchange", "GetSellingStr(" + txtmvisa.ClientID + "," + ddlvisa.ClientID + "," + txtvisastr.ClientID + ");")
            ddlguide.Attributes.Add("onchange", "GetSellingStr(" + txtmguide.ClientID + "," + ddlguide.ClientID + "," + txtgudiestr.ClientID + ");")
            ddljeep.Attributes.Add("onchange", "GetSellingStr(" + txtmjeep.ClientID + "," + ddljeep.ClientID + "," + txtjeepstr.ClientID + ");")
            ddlothers.Attributes.Add("onchange", "GetSellingStr(" + txtmothers.ClientID + "," + ddlothers.ClientID + "," + txtothersstr.ClientID + ");")
            ddlhandlingfee.Attributes.Add("onchange", "GetSellingStr(" + txtmhandlingfee.ClientID + "," + ddlhandlingfee.ClientID + "," + txthandlingfeestr.ClientID + ");")


            txtmhotel.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmhotel.ClientID + ");")
            txtmcar.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmcar.ClientID + ");")
            txtmexcursion.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmexcursion.ClientID + ");")
            txtmentrance.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmentrance.ClientID + ");")
            txtmmeal.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmmeal.ClientID + ");")
            txtmairmeet.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmairmeet.ClientID + ");")
            txtmtansfer.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmtansfer.ClientID + ");")
            txtmvisa.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmvisa.ClientID + ");")
            txtmguide.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmguide.ClientID + ");")
            txtmjeep.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmjeep.ClientID + ");")
            txtmothers.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmothers.ClientID + ");")
            txtmhandlingfee.Attributes.Add("onkeypress", "return checkNumberDecimal1(event," + txtmhandlingfee.ClientID + ");")



        Catch ex As Exception

        End Try

    End Sub
    Private Sub loadoperator()
        Try
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlhotelopr, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcaropr, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcursion, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlentrance, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmeal, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlairmeet, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltransferopr, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvisa, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlguide, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddljeep, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlothers, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlhandlingfee, "operatorsymbol", "operatorsymbol", " select operatorsymbol from operatormast", True)


        Catch ex As Exception

        End Try
    End Sub

    Private Sub DisableControl()
        If ViewState("State") = "New" Then
            txtthumpnail.Visible = False
            txtmissinghotel.Visible = False
            txtreport.Visible = False
            txtother.Visible = False
            pnlview.Visible = False
            lblmsg.Visible = False
            txthotelstr.Enabled = False
            txttransferstr.Enabled = False
            txtcarstr.Enabled = False
            txtvisastr.Enabled = False
            txtexcursionstr.Enabled = False
            txtgudiestr.Enabled = False
            txtentrancestr.Enabled = False
            txtjeepstr.Enabled = False
            txtmealstr.Enabled = False
            txtothersstr.Enabled = False
            txtairmeetstr.Enabled = False
            txthandlingfeestr.Enabled = False
        ElseIf ViewState("State") = "Edit" Then
            'txtthumpnail.Visible = True
            'txtmissinghotel.Visible = True
            'txtreport.Visible = True
            'txtother.Visible = True
            'txtthumpnail.Enabled = False
            'txtmissinghotel.Enabled = False
            'txtreport.Enabled = False
            'txtother.Enabled = False
            pnlview.Visible = True
            lblmsg.Visible = True

            txthotelstr.Enabled = False
            txttransferstr.Enabled = False
            txtcarstr.Enabled = False
            txtvisastr.Enabled = False
            txtexcursionstr.Enabled = False
            txtgudiestr.Enabled = False
            txtentrancestr.Enabled = False
            txtjeepstr.Enabled = False
            txtmealstr.Enabled = False
            txtothersstr.Enabled = False
            txtairmeetstr.Enabled = False
            txthandlingfeestr.Enabled = False
        Else

            fpthumpnail.Visible = False
            fpmissinghotel.Visible = False
            fpreport.Visible = False
            fpother.Visible = False
            pnlview.Visible = True
            pnlfileupload.Visible = False

            txtmhotel.Enabled = False
            txtmtansfer.Enabled = False
            txtmcar.Enabled = False
            txtmvisa.Enabled = False
            txtmexcursion.Enabled = False
            txtmguide.Enabled = False
            txtmentrance.Enabled = False
            txtmjeep.Enabled = False
            txtmmeal.Enabled = False
            txtmothers.Enabled = False
            txtmairmeet.Enabled = False
            txtmhandlingfee.Enabled = False

            ddlhotelopr.Disabled = True
            ddltransferopr.Disabled = True
            ddlcaropr.Disabled = True
            ddlvisa.Disabled = True
            ddlexcursion.Disabled = True
            ddlguide.Disabled = True
            ddlentrance.Disabled = True
            ddljeep.Disabled = True
            ddlmeal.Disabled = True
            ddlothers.Disabled = True
            ddlairmeet.Disabled = True
            ddlhandlingfee.Disabled = True

            txthotelstr.Enabled = False
            txttransferstr.Enabled = False
            txtcarstr.Enabled = False
            txtvisastr.Enabled = False
            txtexcursionstr.Enabled = False
            txtgudiestr.Enabled = False
            txtentrancestr.Enabled = False
            txtjeepstr.Enabled = False
            txtmealstr.Enabled = False
            txtothersstr.Enabled = False
            txtairmeetstr.Enabled = False
            txthandlingfeestr.Enabled = False
            lblmsg.Visible = True
        End If

    End Sub

    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            Dim str As String = ""
            'str = "select a.agentcode,a.agentname,case  isnull(w.whitelable,0) when 1 then 'Yes' when 0 then 'No' end whitelable,w.Markuphotel,w.MarkupTransfer,w.MarkupCar, w.MarkupVisa," & _
            '             "  w.MarkupExcursion, w.MarkupGuide, w.MarkupEntrance, w.MarkupJeep, w.MarkupMeal, w.MarkupOthers, w.MarkupAirmeet, w.MarkupHandlingfee,w.thumpnailimage,w.missinghotelimage,w.reportimage,w.otheimage " & _
            '             "  from agentmast a left outer join  agentmastwl w on a.agentcode=w.agentcode where a.active=1 and a.agentcode='" & RefCode & "'"

            str = "select a.agentcode,a.agentname,case  isnull(w.whitelable,0) when 1 then 'Yes' when 0 then 'No' end whitelable,w.*   from agentmast a left outer join  agentmastwl w on a.agentcode=w.agentcode where a.active=1 and a.agentcode='" & RefCode & "'"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description  " + Server.MapPath("UploadedLogo/") & "' );", True)
            objUtils.WritErrorLog("CustomerWhiteLabelling.aspx", Server.MapPath("ErrorLog.txt"), Server.MapPath("UploadedLogo/"), Session("GlobalUserName"))


            mySqlCmd = New SqlCommand(str, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentcode")) = False Then
                        txtagentcode.Text = CType(mySqlReader("agentcode"), String)
                    Else
                        txtagentcode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        txtagentname.Text = CType(mySqlReader("agentname"), String)
                    Else
                        txtagentname.Text = ""
                    End If

                    If IsDBNull(mySqlReader("Markuphotel")) = False Then
                        If CType(mySqlReader("Markuphotel"), String) = "0.0000" Then
                            txtmhotel.Text = ""
                        Else
                            txtmhotel.Text = CType(mySqlReader("Markuphotel"), String)
                        End If

                    Else
                        txtmhotel.Text = ""
                    End If

                    If IsDBNull(mySqlReader("Markuphotelopt")) = False Then
                        ddlhotelopr.Value = CType(mySqlReader("Markuphotelopt"), String)
                    End If
                    If IsDBNull(mySqlReader("Markuphotelstring")) = False Then
                        txthotelstr.Text = CType(mySqlReader("Markuphotelstring"), String)
                    Else
                        txthotelstr.Text = ""
                    End If

                    If IsDBNull(mySqlReader("MarkupTransfer")) = False Then

                        If CType(mySqlReader("MarkupTransfer"), String) = "0.0000" Then
                            txtmtansfer.Text = ""
                        Else
                            txtmtansfer.Text = CType(mySqlReader("MarkupTransfer"), String)
                        End If

                    Else
                        txtmtansfer.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupTransferopt")) = False Then
                        ddltransferopr.Value = CType(mySqlReader("MarkupTransferopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupTransferstring")) = False Then
                        txttransferstr.Text = CType(mySqlReader("MarkupTransferstring"), String)
                    Else
                        txttransferstr.Text = ""
                    End If




                    If IsDBNull(mySqlReader("MarkupCar")) = False Then
                        If CType(mySqlReader("MarkupCar"), String) = "0.0000" Then
                            txtmcar.Text = ""
                        Else
                            txtmcar.Text = CType(mySqlReader("MarkupCar"), String)
                        End If

                    Else
                        txtmcar.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupCaropt")) = False Then
                        ddlcaropr.Value = CType(mySqlReader("MarkupCaropt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupCarstring")) = False Then
                        txtcarstr.Text = CType(mySqlReader("MarkupCarstring"), String)
                    Else
                        txtcarstr.Text = ""
                    End If


                    If IsDBNull(mySqlReader("MarkupVisa")) = False Then
                        If CType(mySqlReader("MarkupVisa"), String) = "0.0000" Then
                            txtmvisa.Text = ""
                        Else
                            txtmvisa.Text = CType(mySqlReader("MarkupVisa"), String)
                        End If

                    Else
                        txtmvisa.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupVisaopt")) = False Then
                        ddlvisa.Value = CType(mySqlReader("MarkupVisaopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupVisastring")) = False Then
                        txtvisastr.Text = CType(mySqlReader("MarkupVisastring"), String)
                    Else
                        txtvisastr.Text = ""
                    End If


                    If IsDBNull(mySqlReader("MarkupExcursion")) = False Then
                        If CType(mySqlReader("MarkupExcursion"), String) = "0.0000" Then
                            txtmexcursion.Text = ""
                        Else
                            txtmexcursion.Text = CType(mySqlReader("MarkupExcursion"), String)
                        End If

                    Else
                        txtmexcursion.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupExcursionopt")) = False Then
                        ddlexcursion.Value = CType(mySqlReader("MarkupExcursionopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupExcursionstring")) = False Then
                        txtexcursionstr.Text = CType(mySqlReader("MarkupExcursionstring"), String)
                    Else
                        txtexcursionstr.Text = ""
                    End If

                    If IsDBNull(mySqlReader("MarkupGuide")) = False Then
                        If CType(mySqlReader("MarkupGuide"), String) = "0.0000" Then
                            txtmguide.Text = ""
                        Else
                            txtmguide.Text = CType(mySqlReader("MarkupGuide"), String)
                        End If

                    Else
                        txtmguide.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupGuideopt")) = False Then
                        ddlguide.Value = CType(mySqlReader("MarkupGuideopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupGuidestring")) = False Then
                        txtgudiestr.Text = CType(mySqlReader("MarkupGuidestring"), String)
                    Else
                        txtgudiestr.Text = ""
                    End If


                    If IsDBNull(mySqlReader("MarkupEntrance")) = False Then
                        If CType(mySqlReader("MarkupEntrance"), String) = "0.0000" Then
                            txtmentrance.Text = ""
                        Else
                            txtmentrance.Text = CType(mySqlReader("MarkupEntrance"), String)
                        End If

                    Else
                        txtmentrance.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupEntranceopt")) = False Then
                        ddlentrance.Value = CType(mySqlReader("MarkupEntranceopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupEntrancestring")) = False Then
                        txtentrancestr.Text = CType(mySqlReader("MarkupEntrancestring"), String)
                    Else
                        txtentrancestr.Text = ""
                    End If


                    If IsDBNull(mySqlReader("MarkupJeep")) = False Then
                        If CType(mySqlReader("MarkupJeep"), String) = "0.0000" Then
                            txtmjeep.Text = ""
                        Else
                            txtmjeep.Text = CType(mySqlReader("MarkupJeep"), String)
                        End If

                    Else
                        txtmjeep.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupJeepopt")) = False Then
                        ddljeep.Value = CType(mySqlReader("MarkupJeepopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupJeepstring")) = False Then
                        txtjeepstr.Text = CType(mySqlReader("MarkupJeepstring"), String)
                    Else
                        txtjeepstr.Text = ""
                    End If

                    If IsDBNull(mySqlReader("MarkupMeal")) = False Then
                        If CType(mySqlReader("MarkupMeal"), String) = "0.0000" Then
                            txtmmeal.Text = ""
                        Else
                            txtmmeal.Text = CType(mySqlReader("MarkupMeal"), String)
                        End If

                    Else
                        txtmmeal.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupMealopt")) = False Then
                        ddlmeal.Value = CType(mySqlReader("MarkupMealopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupMealstring")) = False Then
                        txtmealstr.Text = CType(mySqlReader("MarkupMealstring"), String)
                    Else
                        txtmealstr.Text = ""
                    End If

                    If IsDBNull(mySqlReader("MarkupOthers")) = False Then
                        If CType(mySqlReader("MarkupOthers"), String) = "0.0000" Then
                            txtmothers.Text = ""
                        Else
                            txtmothers.Text = CType(mySqlReader("MarkupOthers"), String)
                        End If

                    Else
                        txtmothers.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupOthersopt")) = False Then
                        ddlothers.Value = CType(mySqlReader("MarkupOthersopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupOthersstring")) = False Then
                        txtothersstr.Text = CType(mySqlReader("MarkupOthersstring"), String)
                    Else
                        txtothersstr.Text = ""
                    End If

                    If IsDBNull(mySqlReader("MarkupAirmeet")) = False Then
                        If CType(mySqlReader("MarkupAirmeet"), String) = "0.0000" Then
                            txtmairmeet.Text = ""
                        Else
                            txtmairmeet.Text = CType(mySqlReader("MarkupAirmeet"), String)
                        End If

                    Else
                        txtmairmeet.Text = ""
                    End If
                    If IsDBNull(mySqlReader("MarkupAirmeetopt")) = False Then
                        ddlairmeet.Value = CType(mySqlReader("MarkupAirmeetopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupAirmeetstring")) = False Then
                        txtairmeetstr.Text = CType(mySqlReader("MarkupAirmeetstring"), String)
                    Else
                        txtairmeetstr.Text = ""
                    End If


                    If IsDBNull(mySqlReader("MarkupHandlingfee")) = False Then
                        If CType(mySqlReader("MarkupHandlingfee"), String) = "0.0000" Then
                            txtmhandlingfee.Text = ""
                        Else
                            txtmhandlingfee.Text = CType(mySqlReader("MarkupHandlingfee"), String)
                        End If

                    Else
                        txtmhandlingfee.Text = ""
                    End If

                    If IsDBNull(mySqlReader("MarkupHandlingfeeopt")) = False Then
                        ddlhandlingfee.Value = CType(mySqlReader("MarkupHandlingfeeopt"), String)
                    End If
                    If IsDBNull(mySqlReader("MarkupHandlingfeestring")) = False Then
                        txthandlingfeestr.Text = CType(mySqlReader("MarkupHandlingfeestring"), String)
                    Else
                        txthandlingfeestr.Text = ""
                    End If



                    'If Directory.Exists(tt & "\WebAdminModule\UploadedLogo\") = False Then
                    '    Directory.CreateDirectory(tt & "\WebAdminModule\UploadedLogo\")
                    'End If

                    'If IsDBNull(mySqlReader("thumpnailimage")) = False Then
                    '    Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory

                    '    If File.Exists((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Thumpnailimage.bmp")) Then
                    '        File.Delete((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Thumpnailimage.bmp"))
                    '    End If
                    '    File.WriteAllBytes(t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Thumpnailimage.bmp", mySqlReader("thumpnailimage"))
                    '    Imgthumpnail.ImageUrl = "~\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Thumpnailimage.bmp"
                    '    Imgthumpnail.Visible = True
                    '    lblImgthumpnail.Visible = False

                    'Else
                    '    Imgthumpnail.ImageUrl = ""
                    '    Imgthumpnail.Visible = False
                    '    lblImgthumpnail.Visible = True
                    '    lblImgthumpnail.Text = "No Image Found"

                    'End If

                    'If IsDBNull(mySqlReader("missinghotelimage")) = False Then
                    '    Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory

                    '    If File.Exists((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Missinghotelimage.bmp")) Then
                    '        File.Delete((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Missinghotelimage.bmp"))
                    '    End If
                    '    File.WriteAllBytes(t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Missinghotelimage.bmp", mySqlReader("missinghotelimage"))
                    '    Imgmissinghotel.ImageUrl = "~\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Missinghotelimage.bmp"
                    '    Imgmissinghotel.Visible = True
                    '    lblImgmissinghotel.Visible = False

                    'Else
                    '    Imgmissinghotel.ImageUrl = ""
                    '    Imgmissinghotel.Visible = False
                    '    lblImgmissinghotel.Visible = True
                    '    lblImgmissinghotel.Text = "No Image Found"

                    'End If

                    'If IsDBNull(mySqlReader("reportimage")) = False Then
                    '    Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                    '    If File.Exists((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Reportimage.bmp")) Then
                    '        File.Delete((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Reportimage.bmp"))
                    '    End If
                    '    File.WriteAllBytes(t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Reportimage.bmp", mySqlReader("reportimage"))
                    '    Imagreport.ImageUrl = "~\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Reportimage.bmp"
                    '    Imagreport.Visible = True
                    '    lblImagreport.Visible = False

                    'Else
                    '    Imagreport.ImageUrl = ""
                    '    Imagreport.Visible = False
                    '    lblImagreport.Visible = True
                    '    lblImagreport.Text = "No Image Found"

                    'End If

                    'If IsDBNull(mySqlReader("otheimage")) = False Then
                    '    Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                    '    If File.Exists((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Otheimage.bmp")) Then
                    '        File.Delete((t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Otheimage.bmp"))
                    '    End If
                    '    File.WriteAllBytes(t & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Otheimage.bmp", mySqlReader("otheimage"))
                    '    Imgother.ImageUrl = "~\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Otheimage.bmp"
                    '    Imgother.Visible = True
                    '    lblImgother.Visible = False

                    'Else
                    '    Imgother.ImageUrl = ""
                    '    Imgother.Visible = False
                    '    lblImgother.Visible = True
                    '    lblImgother.Text = "No Image Found"

                    'End If
                    '--------------------------



                    If Directory.Exists(Server.MapPath("UploadedLogo/")) = False Then
                        Directory.CreateDirectory(Server.MapPath("UploadedLogo/"))
                    End If

                    If IsDBNull(mySqlReader("thumpnailimage")) = False Then
                        'Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory

                        If File.Exists(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Thumpnailimage.bmp")) Then
                            File.Delete(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Thumpnailimage.bmp"))
                        End If
                        File.WriteAllBytes(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Thumpnailimage.bmp"), mySqlReader("thumpnailimage"))
                        Imgthumpnail.ImageUrl = "~/WebAdminModule/UploadedLogo/" & txtagentcode.Text & "Thumpnailimage.bmp"
                        Imgthumpnail.Visible = True
                        lblImgthumpnail.Visible = False

                    Else
                        Imgthumpnail.ImageUrl = ""
                        Imgthumpnail.Visible = False
                        lblImgthumpnail.Visible = True
                        lblImgthumpnail.Text = "No Image Found"

                    End If

                    If IsDBNull(mySqlReader("missinghotelimage")) = False Then
                        Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory

                        If File.Exists(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Missinghotelimage.bmp")) Then
                            File.Delete(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Missinghotelimage.bmp"))
                        End If
                        File.WriteAllBytes(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Missinghotelimage.bmp"), mySqlReader("missinghotelimage"))
                        Imgmissinghotel.ImageUrl = "~/WebAdminModule/UploadedLogo/" & txtagentcode.Text & "Missinghotelimage.bmp"
                        Imgmissinghotel.Visible = True
                        lblImgmissinghotel.Visible = False

                    Else
                        Imgmissinghotel.ImageUrl = ""
                        Imgmissinghotel.Visible = False
                        lblImgmissinghotel.Visible = True
                        lblImgmissinghotel.Text = "No Image Found"

                    End If

                    If IsDBNull(mySqlReader("reportimage")) = False Then
                        'Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                        If File.Exists(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Reportimage.bmp")) Then
                            File.Delete(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Reportimage.bmp"))
                        End If
                        File.WriteAllBytes(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Reportimage.bmp"), mySqlReader("reportimage"))
                        Imagreport.ImageUrl = "~/WebAdminModule/UploadedLogo/" & txtagentcode.Text & "Reportimage.bmp"
                        Imagreport.Visible = True
                        lblImagreport.Visible = False

                    Else
                        Imagreport.ImageUrl = ""
                        Imagreport.Visible = False
                        lblImagreport.Visible = True
                        lblImagreport.Text = "No Image Found"

                    End If

                    If IsDBNull(mySqlReader("otheimage")) = False Then
                        'Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                        If File.Exists(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Otheimage.bmp")) Then
                            File.Delete(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Otheimage.bmp"))
                        End If
                        File.WriteAllBytes(Server.MapPath("UploadedLogo/" & txtagentcode.Text & "Otheimage.bmp"), mySqlReader("otheimage"))
                        Imgother.ImageUrl = "~/WebAdminModule/UploadedLogo/" & txtagentcode.Text & "Otheimage.bmp"
                        Imgother.Visible = True
                        lblImgother.Visible = False

                    Else
                        Imgother.ImageUrl = ""
                        Imgother.Visible = False
                        lblImgother.Visible = True
                        lblImgother.Text = "No Image Found"

                    End If


                    If IsDBNull(mySqlReader("active")) = False Then
                        If mySqlReader("active") = 1 Then
                            chkactive.Checked = True
                        Else
                            chkactive.Checked = False
                        End If

                    Else
                        If ViewState("State") = "New" Then
                            chkactive.Checked = True
                        Else
                            chkactive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("address1")) = False Then
                        txtaddress.Text = CType(mySqlReader("address1"), String)
                    Else
                        txtaddress.Text = ""
                    End If

                    If IsDBNull(mySqlReader("address2")) = False Then
                        txttelephone.Text = CType(mySqlReader("address2"), String)
                    Else
                        txttelephone.Text = ""
                    End If

                    If IsDBNull(mySqlReader("address3")) = False Then
                        txtfax.Text = CType(mySqlReader("address3"), String)
                    Else
                        txtfax.Text = ""
                    End If

                    If IsDBNull(mySqlReader("address4")) = False Then
                        txtemail.Text = CType(mySqlReader("address4"), String)
                    Else
                        txtemail.Text = ""
                    End If

                    If IsDBNull(mySqlReader("address5")) = False Then
                        txtweb.Text = CType(mySqlReader("address5"), String)
                    Else
                        txtweb.Text = ""
                    End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerWhiteLabelling.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim logothump, logohotel, logoreport, logoother As Byte()
        Dim strthump, strhotel, strreport, strother As String

        logothump = Nothing
        logohotel = Nothing
        logoreport = Nothing
        logoother = Nothing


        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Dim PassArry As String = ""
        Dim strPassQry As String = "False"
        Dim frmmode As Integer

        Dim sptypecode As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='459'")

        Try
            If Page.IsValid = True Then

                If ViewState("State") = "New" Or ViewState("State") = "Edit" Then
                    frmmode = 2
                    If ValidateSave() = False Then
                        Exit Sub
                    End If

                    If sptypecode = "UAE" Then
                        If CheckImage() = False Then
                            Exit Sub
                        End If
                    End If

                    If sptypecode = "JOR" Then
                        If UploadImage() = False Then
                            Exit Sub
                        End If
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction


                    If ViewState("State") = "New" Then

                        mySqlCmd = New SqlCommand("AddMod_agentmastwl", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@flag", 1))
                        PassArry = "1" + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", CType(ViewState("RefCode"), String)))
                        PassArry = PassArry + ViewState("RefCode") + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@whitelable", 1))
                        PassArry = PassArry + "1" + ","

                        mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotel", Val(txtmhotel.Text.Trim())))
                        PassArry = PassArry + Val(txtmhotel.Text.Trim()).ToString + ","
                        If ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotelopt", ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Text))
                            PassArry = PassArry + ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Text + ","

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotelopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotelstring", txthotelstr.Text))
                        PassArry = PassArry + txthotelstr.Text + ","

                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransfer", Val(txtmtansfer.Text.Trim())))
                        PassArry = PassArry + Val(txtmtansfer.Text.Trim()).ToString() + ","
                        If ddlhotelopr.Items(ddltransferopr.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransferopt", ddlhotelopr.Items(ddltransferopr.SelectedIndex).Text))
                            PassArry = PassArry + ddlhotelopr.Items(ddltransferopr.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransferopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransferstring", txttransferstr.Text))
                        PassArry = PassArry + txttransferstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCar", Val(txtmcar.Text.Trim())))
                        PassArry = PassArry + Val(txtmcar.Text.Trim()).ToString() + ","
                        If ddlcaropr.Items(ddlcaropr.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCaropt", ddlcaropr.Items(ddlcaropr.SelectedIndex).Text))
                            PassArry = PassArry + ddlcaropr.Items(ddlcaropr.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCaropt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCarstring", txtcarstr.Text))
                        PassArry = PassArry + txtcarstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisa", Val(txtmvisa.Text.Trim())))
                        PassArry = PassArry + Val(txtmvisa.Text.Trim()).ToString() + ","
                        If ddlvisa.Items(ddlvisa.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisaopt", ddlvisa.Items(ddlvisa.SelectedIndex).Text))
                            PassArry = PassArry + ddlvisa.Items(ddlvisa.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisaopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisastring", txtvisastr.Text))
                        PassArry = PassArry + txtvisastr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursion", Val(txtmexcursion.Text.Trim())))
                        PassArry = PassArry + Val(txtmexcursion.Text.Trim()).ToString() + ","
                        If ddlexcursion.Items(ddlexcursion.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursionopt", ddlexcursion.Items(ddlexcursion.SelectedIndex).Text))
                            PassArry = PassArry + ddlexcursion.Items(ddlexcursion.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursionopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursionstring", txtexcursionstr.Text))
                        PassArry = PassArry + txtexcursionstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuide", Val(txtmguide.Text.Trim())))
                        PassArry = PassArry + Val(txtmguide.Text.Trim()).ToString() + ","
                        If ddlguide.Items(ddlguide.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuideopt", ddlguide.Items(ddlguide.SelectedIndex).Text))
                            PassArry = PassArry + ddlguide.Items(ddlguide.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuideopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuidestring", txtgudiestr.Text))
                        PassArry = PassArry + txtgudiestr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntrance", Val(txtmentrance.Text.Trim())))
                        PassArry = PassArry + Val(txtmentrance.Text.Trim()).ToString() + ","
                        If ddlentrance.Items(ddlentrance.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntranceopt", ddlentrance.Items(ddlentrance.SelectedIndex).Text))
                            PassArry = PassArry + ddlentrance.Items(ddlentrance.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntranceopt", ""))
                            PassArry = PassArry + "" + ","

                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntrancestring", txtentrancestr.Text))
                        PassArry = PassArry + txtentrancestr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeep", Val(txtmjeep.Text.Trim())))
                        PassArry = PassArry + Val(txtmjeep.Text.Trim()).ToString() + ","
                        If ddljeep.Items(ddljeep.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeepopt", ddljeep.Items(ddljeep.SelectedIndex).Text))
                            PassArry = PassArry + ddljeep.Items(ddljeep.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeepopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeepstring", txtjeepstr.Text))
                        PassArry = PassArry + txtjeepstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMeal", Val(txtmmeal.Text.Trim())))
                        PassArry = PassArry + Val(txtmmeal.Text.Trim()).ToString() + ","
                        If ddlmeal.Items(ddlmeal.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMealopt", ddlmeal.Items(ddlmeal.SelectedIndex).Text))
                            PassArry = PassArry + ddlmeal.Items(ddlmeal.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMealopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMealstring", txtmealstr.Text))
                        PassArry = PassArry + txtmealstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthers", Val(txtmothers.Text.Trim())))
                        PassArry = PassArry + Val(txtmothers.Text.Trim()).ToString() + ","
                        If ddlothers.Items(ddlothers.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthersopt", ddlothers.Items(ddlothers.SelectedIndex).Text))
                            PassArry = PassArry + ddlothers.Items(ddlothers.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthersopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthersstring", txtothersstr.Text))
                        PassArry = PassArry + txtothersstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeet", Val(txtmairmeet.Text.Trim())))
                        PassArry = PassArry + Val(txtmairmeet.Text.Trim()).ToString() + ","
                        If ddlairmeet.Items(ddlairmeet.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeetopt", ddlairmeet.Items(ddlairmeet.SelectedIndex).Text))
                            PassArry = PassArry + ddlairmeet.Items(ddlairmeet.SelectedIndex).Text + ","

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeetopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeetstring", txtairmeetstr.Text))
                        PassArry = PassArry + txtairmeetstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfee", Val(txtmhandlingfee.Text.Trim())))
                        PassArry = PassArry + Val(txtmhandlingfee.Text.Trim()).ToString() + ","
                        If ddlhandlingfee.Items(ddlhandlingfee.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfeeopt", ddlhandlingfee.Items(ddlhandlingfee.SelectedIndex).Text))
                            PassArry = PassArry + ddlhandlingfee.Items(ddlhandlingfee.SelectedIndex).Text + ","

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfeeopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfeestring", txthandlingfeestr.Text))
                        PassArry = PassArry + txthandlingfeestr.Text + ","


                        If fpthumpnail.HasFile = False Then
                            logothump = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@thumpnailimage", SqlDbType.Image)).Value = DBNull.Value
                            ' PassArry = PassArry + DBNull.Value + ","
                        Else
                            strthump = fpthumpnail.FileName
                            mySqlCmd.Parameters.Add(New SqlParameter("@thumpnailimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, fpthumpnail.FileBytes)
                            logothump = fpthumpnail.FileBytes
                            ' PassArry = PassArry + IIf(strthump = "", DBNull.Value, fpthumpnail.FileBytes) + ","
                        End If

                        If fpmissinghotel.HasFile = False Then
                            logohotel = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@missinghotelimage", SqlDbType.Image)).Value = DBNull.Value
                            ' PassArry = PassArry + DBNull.Value + ","
                        Else
                            strhotel = fpmissinghotel.FileName
                            mySqlCmd.Parameters.Add(New SqlParameter("@missinghotelimage", SqlDbType.Image)).Value = IIf(strhotel = "", DBNull.Value, fpmissinghotel.FileBytes)
                            logohotel = fpmissinghotel.FileBytes
                            'PassArry = PassArry + IIf(strhotel = "", DBNull.Value, fpmissinghotel.FileBytes) + ","
                        End If

                        If fpreport.HasFile = False Then
                            logoreport = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@reportimage", SqlDbType.Image)).Value = DBNull.Value
                            'PassArry = PassArry + DBNull.Value + ","
                        Else
                            strreport = fpreport.FileName
                            mySqlCmd.Parameters.Add(New SqlParameter("@reportimage", SqlDbType.Image)).Value = IIf(strreport = "", DBNull.Value, fpreport.FileBytes)
                            logoreport = fpreport.FileBytes
                            'PassArry = PassArry + IIf(strreport = "", DBNull.Value, fpreport.FileBytes) + ","

                        End If

                        If fpother.HasFile = False Then
                            logoother = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@otheimage", SqlDbType.Image)).Value = DBNull.Value
                            ' PassArry = PassArry + DBNull.Value + ","
                        Else
                            strother = fpother.FileName
                            mySqlCmd.Parameters.Add(New SqlParameter("@otheimage", SqlDbType.Image)).Value = IIf(strother = "", DBNull.Value, fpother.FileBytes)
                            logoother = fpother.FileBytes
                            ' PassArry = PassArry + IIf(strother = "", DBNull.Value, fpother.FileBytes) + ","

                        End If

                        If chkactive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", 1))
                            PassArry = PassArry + "1" + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", 0))
                            PassArry = PassArry + "0" + ","
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@address1", txtaddress.Text))
                        mySqlCmd.Parameters.Add(New SqlParameter("@address2", txttelephone.Text))
                        mySqlCmd.Parameters.Add(New SqlParameter("@address3", txtfax.Text))
                        mySqlCmd.Parameters.Add(New SqlParameter("@address4", txtemail.Text))
                        mySqlCmd.Parameters.Add(New SqlParameter("@address5", txtweb.Text))

                        mySqlCmd.Parameters.Add(New SqlParameter("@TittltIcon", SqlDbType.VarChar)).Value = DBNull.Value

                        mySqlCmd.ExecuteNonQuery()


                    ElseIf ViewState("State") = "Edit" Then
                        frmmode = 3
                        '------------ save into log file -----------------------

                        mySqlCmd = New SqlCommand("Add_agentmastwl_log", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 200)).Value = CType(ViewState("RefCode"), String)
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("AddMod_agentmastwl", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@flag", 2))
                        PassArry = "2" + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", CType(ViewState("RefCode"), String)))
                        PassArry = PassArry + ViewState("RefCode") + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@whitelable", 1))
                        PassArry = PassArry + "1" + ","

                        mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotel", Val(txtmhotel.Text.Trim())))
                        PassArry = PassArry + Val(txtmhotel.Text.Trim()).ToString + ","
                        If ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotelopt", ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Text))
                            PassArry = PassArry + ddlhotelopr.Items(ddlhotelopr.SelectedIndex).Text + ","

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotelopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@Markuphotelstring", txthotelstr.Text))
                        PassArry = PassArry + txthotelstr.Text + ","

                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransfer", Val(txtmtansfer.Text.Trim())))
                        PassArry = PassArry + Val(txtmtansfer.Text.Trim()).ToString() + ","
                        If ddlhotelopr.Items(ddltransferopr.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransferopt", ddlhotelopr.Items(ddltransferopr.SelectedIndex).Text))
                            PassArry = PassArry + ddlhotelopr.Items(ddltransferopr.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransferopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupTransferstring", txttransferstr.Text))
                        PassArry = PassArry + txttransferstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCar", Val(txtmcar.Text.Trim())))
                        PassArry = PassArry + Val(txtmcar.Text.Trim()).ToString() + ","
                        If ddlcaropr.Items(ddlcaropr.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCaropt", ddlcaropr.Items(ddlcaropr.SelectedIndex).Text))
                            PassArry = PassArry + ddlcaropr.Items(ddlcaropr.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCaropt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupCarstring", txtcarstr.Text))
                        PassArry = PassArry + txtcarstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisa", Val(txtmvisa.Text.Trim())))
                        PassArry = PassArry + Val(txtmvisa.Text.Trim()).ToString() + ","
                        If ddlvisa.Items(ddlvisa.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisaopt", ddlvisa.Items(ddlvisa.SelectedIndex).Text))
                            PassArry = PassArry + ddlvisa.Items(ddlvisa.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisaopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupVisastring", txtvisastr.Text))
                        PassArry = PassArry + txtvisastr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursion", Val(txtmexcursion.Text.Trim())))
                        PassArry = PassArry + Val(txtmexcursion.Text.Trim()).ToString() + ","
                        If ddlexcursion.Items(ddlexcursion.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursionopt", ddlexcursion.Items(ddlexcursion.SelectedIndex).Text))
                            PassArry = PassArry + ddlexcursion.Items(ddlexcursion.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursionopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupExcursionstring", txtexcursionstr.Text))
                        PassArry = PassArry + txtexcursionstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuide", Val(txtmguide.Text.Trim())))
                        PassArry = PassArry + Val(txtmguide.Text.Trim()).ToString() + ","
                        If ddlguide.Items(ddlguide.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuideopt", ddlguide.Items(ddlguide.SelectedIndex).Text))
                            PassArry = PassArry + ddlguide.Items(ddlguide.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuideopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupGuidestring", txtgudiestr.Text))
                        PassArry = PassArry + txtgudiestr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntrance", Val(txtmentrance.Text.Trim())))
                        PassArry = PassArry + Val(txtmentrance.Text.Trim()).ToString() + ","
                        If ddlentrance.Items(ddlentrance.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntranceopt", ddlentrance.Items(ddlentrance.SelectedIndex).Text))
                            PassArry = PassArry + ddlentrance.Items(ddlentrance.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntranceopt", ""))
                            PassArry = PassArry + "" + ","

                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupEntrancestring", txtentrancestr.Text))
                        PassArry = PassArry + txtentrancestr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeep", Val(txtmjeep.Text.Trim())))
                        PassArry = PassArry + Val(txtmjeep.Text.Trim()).ToString() + ","
                        If ddljeep.Items(ddljeep.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeepopt", ddljeep.Items(ddljeep.SelectedIndex).Text))
                            PassArry = PassArry + ddljeep.Items(ddljeep.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeepopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupJeepstring", txtjeepstr.Text))
                        PassArry = PassArry + txtjeepstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMeal", Val(txtmmeal.Text.Trim())))
                        PassArry = PassArry + Val(txtmmeal.Text.Trim()).ToString() + ","
                        If ddlmeal.Items(ddlmeal.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMealopt", ddlmeal.Items(ddlmeal.SelectedIndex).Text))
                            PassArry = PassArry + ddlmeal.Items(ddlmeal.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMealopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupMealstring", txtmealstr.Text))
                        PassArry = PassArry + txtmealstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthers", Val(txtmothers.Text.Trim())))
                        PassArry = PassArry + Val(txtmothers.Text.Trim()).ToString() + ","
                        If ddlothers.Items(ddlothers.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthersopt", ddlothers.Items(ddlothers.SelectedIndex).Text))
                            PassArry = PassArry + ddlothers.Items(ddlothers.SelectedIndex).Text + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthersopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupOthersstring", txtothersstr.Text))
                        PassArry = PassArry + txtothersstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeet", Val(txtmairmeet.Text.Trim())))
                        PassArry = PassArry + Val(txtmairmeet.Text.Trim()).ToString() + ","
                        If ddlairmeet.Items(ddlairmeet.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeetopt", ddlairmeet.Items(ddlairmeet.SelectedIndex).Text))
                            PassArry = PassArry + ddlairmeet.Items(ddlairmeet.SelectedIndex).Text + ","

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeetopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAirmeetstring", txtairmeetstr.Text))
                        PassArry = PassArry + txtairmeetstr.Text + ","


                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfee", Val(txtmhandlingfee.Text.Trim())))
                        PassArry = PassArry + Val(txtmhandlingfee.Text.Trim()).ToString() + ","
                        If ddlhandlingfee.Items(ddlhandlingfee.SelectedIndex).Text <> "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfeeopt", ddlhandlingfee.Items(ddlhandlingfee.SelectedIndex).Text))
                            PassArry = PassArry + ddlhandlingfee.Items(ddlhandlingfee.SelectedIndex).Text + ","

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfeeopt", ""))
                            PassArry = PassArry + "" + ","
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupHandlingfeestring", txthandlingfeestr.Text))
                        PassArry = PassArry + txthandlingfeestr.Text + ","


                        If fpthumpnail.HasFile = False And Imgthumpnail.ImageUrl = "" Then
                            logothump = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@thumpnailimage", SqlDbType.Image)).Value = DBNull.Value
                        Else
                            If fpthumpnail.HasFile = True Then
                                strthump = fpthumpnail.FileName
                                mySqlCmd.Parameters.Add(New SqlParameter("@thumpnailimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, fpthumpnail.FileBytes)
                                logothump = fpthumpnail.FileBytes
                            Else
                                strthump = Imgthumpnail.ImageUrl
                                Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                                strthump = strthump.Substring(1)
                                strthump = t & strthump

                                mySqlCmd.Parameters.Add(New SqlParameter("@thumpnailimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, File.ReadAllBytes(strthump))
                                logothump = File.ReadAllBytes(strthump)
                            End If

                        End If


                        If fpmissinghotel.HasFile = False And Imgmissinghotel.ImageUrl = "" Then
                            logohotel = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@missinghotelimage", SqlDbType.Image)).Value = DBNull.Value
                        Else
                            If fpmissinghotel.HasFile = True Then
                                strthump = fpmissinghotel.FileName
                                mySqlCmd.Parameters.Add(New SqlParameter("@missinghotelimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, fpmissinghotel.FileBytes)
                                logohotel = fpmissinghotel.FileBytes
                            Else
                                strthump = Imgmissinghotel.ImageUrl
                                Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                                strthump = strthump.Substring(1)
                                strthump = t & strthump

                                mySqlCmd.Parameters.Add(New SqlParameter("@missinghotelimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, File.ReadAllBytes(strthump))
                                logohotel = File.ReadAllBytes(strthump)
                            End If

                        End If

                        If fpreport.HasFile = False And Imagreport.ImageUrl = "" Then
                            logoreport = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@reportimage", SqlDbType.Image)).Value = DBNull.Value
                        Else
                            If fpreport.HasFile = True Then
                                strthump = fpreport.FileName
                                mySqlCmd.Parameters.Add(New SqlParameter("@reportimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, fpreport.FileBytes)
                                logoreport = fpreport.FileBytes
                            Else
                                strthump = Imagreport.ImageUrl
                                Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                                strthump = strthump.Substring(1)
                                strthump = t & strthump
                                mySqlCmd.Parameters.Add(New SqlParameter("@reportimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, File.ReadAllBytes(strthump))
                                logoreport = File.ReadAllBytes(strthump)
                            End If

                        End If

                        If fpother.HasFile = False And Imgother.ImageUrl = "" Then
                            logoother = Nothing
                            mySqlCmd.Parameters.Add(New SqlParameter("@otheimage", SqlDbType.Image)).Value = DBNull.Value
                        Else
                            If fpother.HasFile = True Then
                                strthump = fpother.FileName
                                mySqlCmd.Parameters.Add(New SqlParameter("@otheimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, fpother.FileBytes)
                                logoother = fpother.FileBytes
                            Else
                                strthump = Imgother.ImageUrl
                                Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                                strthump = strthump.Substring(1)
                                strthump = t & strthump

                                mySqlCmd.Parameters.Add(New SqlParameter("@otheimage", SqlDbType.Image)).Value = IIf(strthump = "", DBNull.Value, File.ReadAllBytes(strthump))
                                logoother = File.ReadAllBytes(strthump)
                            End If

                        End If

                        If chkactive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", 1))
                            PassArry = PassArry + "1" + ","
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", 0))
                            PassArry = PassArry + "0" + ","
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@address1", txtaddress.Text))
                        PassArry = PassArry + txtaddress.Text + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@address2", txttelephone.Text))
                        PassArry = PassArry + txttelephone.Text + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@address3", txtfax.Text))
                        PassArry = PassArry + txtfax.Text + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@address4", txtemail.Text))
                        PassArry = PassArry + txtemail.Text + ","
                        mySqlCmd.Parameters.Add(New SqlParameter("@address5", txtweb.Text))
                        PassArry = PassArry + txtweb.Text + ","

                        mySqlCmd.Parameters.Add(New SqlParameter("@TittltIcon", SqlDbType.VarChar)).Value = DBNull.Value
                        mySqlCmd.ExecuteNonQuery()
                        DeleteAgentImages()
                    End If



                ElseIf ViewState("State") = "Delete" Then
                    frmmode = 1
                    '------------ save into log file -----------------------

                    PassArry = PassArry + "del" + ","
                    PassArry = PassArry + CType(ViewState("RefCode"), String) + ","
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction
                    mySqlCmd = New SqlCommand("Add_agentmastwl_log", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 200)).Value = CType(ViewState("RefCode"), String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("Del_agentmastwl", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 200)).Value = CType(ViewState("RefCode"), String)
                    mySqlCmd.ExecuteNonQuery()
                    DeleteAgentImages()

                End If

                '------------------------ Reverse add using webservice



                'Dim cmn As New Customer_WebService.WebService
                strPassQry = ""
                'strPassQry = cmn.UpdateAgentmastwl(CType(Session("dbconnectionName"), String), frmmode, strUsrnm, strPwd, PassArry, logothump, logohotel, logoreport, logoother)



                'If strPassQry <> "" Then
                '    If mySqlConn.State = ConnectionState.Open Then
                '        sqlTrans.Rollback()
                '    End If
                '    strPassQry = "Error from WebService -- " & strPassQry.Replace("""", " ").Replace("'", " ")

                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + strPassQry + "');", True)
                '    Exit Sub
                'End If


                If strPassQry = "" Then

                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('CustWhiteLabelWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                End If

            End If

        Catch ex As Exception
            'sqlTrans.Rollback()
            'If mySqlConn.State = ConnectionState.Open Then
            '    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            'End If
            'objUtils.WritErrorLog("CustomerWhiteLabelling.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()



                'If frmmode > 0 And strPassQry = "" Then
                '    strPassQry = ""
                '    Dim cmn As New Customer_WebService.WebService
                '    strPassQry = cmn.Reverse_UpdateAgentmastwl(CType(Session("dbconnectionName"), String), frmmode, strUsrnm, strPwd, txtagentcode.Text.Trim())
                '    If strPassQry <> "" Then
                '        strPassQry = " Service Couldnt Reverse!! -- " & strPassQry.Replace("""", " ").Replace("'", " ")
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + strPassQry + "');", True)
                '    End If
                'End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerWhiteLabelling.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub


    Public Function ValidateSave() As Boolean
        Try
            If fpthumpnail.HasFile = False And Imgthumpnail.ImageUrl = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Thumpmail Image');", True)
                ValidateSave = False
                Exit Function
            End If
            If fpmissinghotel.HasFile = False And Imgmissinghotel.ImageUrl = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select MissingHotel Image');", True)
                ValidateSave = False
                Exit Function
            End If
            If fpreport.HasFile = False And Imagreport.ImageUrl = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Report Image');", True)
                ValidateSave = False
                Exit Function
            End If

            If txtaddress.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Address should not blank');", True)
                ValidateSave = False
                Exit Function
            End If

            If txttelephone.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Telephone should not blank');", True)
                ValidateSave = False
                Exit Function
            End If

            If txtemail.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email should not blank');", True)
                ValidateSave = False
                Exit Function
            End If

            ValidateSave = True
        Catch ex As Exception
            objUtils.WritErrorLog("CustomerWhiteLabelling.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            ValidateSave = False
        End Try
    End Function

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Try
            DeleteAgentImages()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Catch ex As Exception
            objUtils.WritErrorLog("CustomerWhiteLabelling.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Function CheckImagethumnail(ByVal strfilename As String) As Boolean
        If File.Exists(Server.MapPath("UploadedLogo/" + strfilename)) = True Then
            Dim imageurl As String = Server.MapPath("UploadedLogo/" + strfilename.Trim)
            Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)

            Dim width As Integer = 64 'objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=517")  ' fullSizeImg.Width
            Dim height As Integer = 70 'objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=518") 'fullSizeImg.Height

            If fullSizeImg.Height > height Then
                fullSizeImg.Dispose()
                CheckImagethumnail = False
                Exit Function
            Else
                CheckImagethumnail = True

            End If
            If fullSizeImg.Width > width Then
                fullSizeImg.Dispose()
                CheckImagethumnail = False
                Exit Function
            Else
                CheckImagethumnail = True
                fullSizeImg.Dispose()
            End If
        End If
        CheckImagethumnail = True
    End Function

    Private Function CheckImagemissing(ByVal strfilename As String) As Boolean
        If File.Exists(Server.MapPath("UploadedLogo/" + strfilename)) = True Then
            Dim imageurl As String = Server.MapPath("UploadedLogo/" + strfilename.Trim)
            Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)

            Dim width As Integer = 64 'objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=517")  ' fullSizeImg.Width
            Dim height As Integer = 70 'objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=518") 'fullSizeImg.Height

            If fullSizeImg.Height > height Then
                fullSizeImg.Dispose()
                CheckImagemissing = False
                Exit Function
            Else
                CheckImagemissing = True

            End If
            If fullSizeImg.Width > width Then
                fullSizeImg.Dispose()
                CheckImagemissing = False
                Exit Function
            Else
                CheckImagemissing = True
                fullSizeImg.Dispose()
            End If
        End If
        CheckImagemissing = True
    End Function
    Private Function CheckImagereport(ByVal strfilename As String) As Boolean
        If File.Exists(Server.MapPath("UploadedLogo/" + strfilename)) = True Then
            Dim imageurl As String = Server.MapPath("UploadedLogo/" + strfilename.Trim)
            Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)

            Dim width As Integer = 331 'objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=517")  ' fullSizeImg.Width
            Dim height As Integer = 106 'objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=518") 'fullSizeImg.Height

            If fullSizeImg.Height < height And fullSizeImg.Height > 108 Then
                fullSizeImg.Dispose()
                CheckImagereport = False
                Exit Function
            Else
                CheckImagereport = True

            End If
            If fullSizeImg.Width < width Or fullSizeImg.Width > 336 Then
                fullSizeImg.Dispose()
                CheckImagereport = False
                Exit Function
            Else
                CheckImagereport = True
                fullSizeImg.Dispose()
            End If
        End If
        CheckImagereport = True
    End Function
    Public Function UploadImage() As Boolean

        Dim strFileName As String = ""
        Dim strsmallimage As String = ""
        Dim strFilemissing As String = ""
        Dim strFilereport As String = ""

        Try

            Dim ext As String
            If fpthumpnail.HasFile Then
                strFileName = txtagentcode.Text.Trim + fpthumpnail.FileName.Trim
                ext = strFileName.Substring(strFileName.LastIndexOf(".") + 1)
                'ext == "gif" || ext == "GIF" || ext == "JPEG" || ext == "jpeg" || ext == "jpg" || ext == "JPG")
                If ext.ToUpper = "bmp" Or ext.ToUpper = "BMP" Or ext.ToUpper = "jpg" Or ext.ToUpper = "JPG" Then
                    'If File.Exists(Server.MapPath("UploadedImages/" + strFileName)) = True Then


                    fpthumpnail.PostedFile.SaveAs(Server.MapPath("UploadedLogo/" + strFileName.Trim))
                    strsmallimage = Server.MapPath("UploadedLogo/" + strFileName.Trim)

                    If CheckImagethumnail(strFileName.Trim) = False Then
                        If File.Exists(Server.MapPath("UploadedLogo/" + strFileName.Trim)) = True Then
                            File.Delete(Server.MapPath("UploadedLogo/" + strFileName.Trim))
                        End If

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                        Return False
                    End If

                    If File.Exists(Server.MapPath("UploadedLogo/" + strFileName.Trim)) = True Then
                        File.Delete(Server.MapPath("UploadedLogo/" + strFileName.Trim))
                    End If

                    'ImgBanner.ImageUrl = txtImage.Text
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select image file.' );", True)
                    Return False
                End If


            End If

            If fpmissinghotel.HasFile Then
                strFilemissing = txtagentcode.Text.Trim + fpmissinghotel.FileName.Trim
                ext = strFilemissing.Substring(strFilemissing.LastIndexOf(".") + 1)
                'ext == "gif" || ext == "GIF" || ext == "JPEG" || ext == "jpeg" || ext == "jpg" || ext == "JPG")
                If ext.ToUpper = "bmp" Or ext.ToUpper = "BMP" Or ext.ToUpper = "jpg" Or ext.ToUpper = "JPG" Then
                    'If File.Exists(Server.MapPath("UploadedImages/" + strFilemissing)) = True Then

                    fpthumpnail.PostedFile.SaveAs(Server.MapPath("UploadedLogo/" + strFilemissing.Trim))
                    strsmallimage = Server.MapPath("UploadedLogo/" + strFilemissing.Trim)

                    If CheckImagemissing(strFilemissing.Trim) = False Then
                        If File.Exists(Server.MapPath("UploadedLogo/" + strFilemissing.Trim)) = True Then
                            File.Delete(Server.MapPath("UploadedLogo/" + strFilemissing.Trim))
                        End If

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                        Return False
                    End If

                    If File.Exists(Server.MapPath("UploadedLogo/" + strFilemissing.Trim)) = True Then
                        File.Delete(Server.MapPath("UploadedLogo/" + strFilemissing.Trim))
                    End If

                    'ImgBanner.ImageUrl = txtImage.Text
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select image file.' );", True)
                    Return False
                End If
            End If

            If fpreport.HasFile Then
                strFilereport = txtagentcode.Text.Trim + fpreport.FileName.Trim
                ext = strFilereport.Substring(strFilereport.LastIndexOf(".") + 1)
                'ext == "gif" || ext == "GIF" || ext == "JPEG" || ext == "jpeg" || ext == "jpg" || ext == "JPG")
                If ext.ToUpper = "bmp" Or ext.ToUpper = "BMP" Or ext.ToUpper = "jpg" Or ext.ToUpper = "JPG" Then
                    'If File.Exists(Server.MapPath("UploadedImages/" + strFilereport)) = True Then

                    fpreport.PostedFile.SaveAs(Server.MapPath("UploadedLogo/" + strFilereport.Trim))
                    strsmallimage = Server.MapPath("UploadedLogo/" + strFilereport.Trim)

                    If CheckImagereport(strFilereport) = False Then
                        If File.Exists(Server.MapPath("UploadedLogo/" + strFilereport.Trim)) = True Then
                            File.Delete(Server.MapPath("UploadedLogo/" + strFilereport.Trim))
                        End If

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                        Return False
                    End If

                    If File.Exists(Server.MapPath("UploadedLogo/" + strFilereport.Trim)) = True Then
                        File.Delete(Server.MapPath("UploadedLogo/" + strFilereport.Trim))
                    End If


                    'ImgBanner.ImageUrl = txtImage.Text
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select image file.' );", True)
                    Return False
                End If
            End If
        Catch ex As Exception
            If File.Exists(Server.MapPath("UploadedLogo/" + strFileName.Trim)) = True Then
                File.Delete(Server.MapPath("UploadedLogo/" + strFileName.Trim))
            End If

            If File.Exists(Server.MapPath("UploadedLogo/" + strFilemissing.Trim)) = True Then
                File.Delete(Server.MapPath("UploadedLogo/" + strFilemissing.Trim))
            End If

            If File.Exists(Server.MapPath("UploadedLogo/" + strFilereport.Trim)) = True Then
                File.Delete(Server.MapPath("UploadedLogo/" + strFilereport.Trim))
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
        UploadImage = True
    End Function

    Private Function CheckImage() As Boolean
        Try

            If fpthumpnail.HasFile = True Then
                Dim imageurl As String = fpthumpnail.PostedFile.FileName
                Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)

                If fullSizeImg.Height > 70 Then

                    fullSizeImg.Dispose()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Thumpnail Image Size Should be (64x70 Pixels)');", True)
                    CheckImage = False
                    Exit Function
                Else

                    CheckImage = True
                End If

                If fullSizeImg.Width > 64 Then
                    fullSizeImg.Dispose()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Thumpnail Image Size Should be (64x70 Pixels)');", True)
                    CheckImage = False
                    Exit Function
                Else

                    CheckImage = True
                End If
            End If
            If fpmissinghotel.HasFile = True Then
                Dim imageurl As String = fpmissinghotel.PostedFile.FileName
                Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)

                If fullSizeImg.Height > 70 Then

                    fullSizeImg.Dispose()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Thumpnail Image Size Should be (64x70 Pixels)');", True)
                    CheckImage = False
                    Exit Function
                Else

                    CheckImage = True
                End If

                If fullSizeImg.Width > 64 Then
                    fullSizeImg.Dispose()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Thumpnail Image Size Should be (64x70 Pixels)');", True)
                    CheckImage = False
                    Exit Function
                Else

                    CheckImage = True
                End If
            End If
            If fpreport.HasFile = True Then
                Dim imageurl As String = fpreport.PostedFile.FileName
                Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)

                If fullSizeImg.Height < 106 And fullSizeImg.Height > 108 Then
                    fullSizeImg.Dispose()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Report Image Height Should be (108 Pixels)');", True)
                    CheckImage = False
                    Exit Function
                Else

                    CheckImage = True
                End If


                If fullSizeImg.Width < 331 And fullSizeImg.Width > 336 Then
                    fullSizeImg.Dispose()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Report Image width Should be (331 Pixels)');", True)
                    CheckImage = False
                    Exit Function
                Else

                    CheckImage = True
                End If
            End If


            CheckImage = True
        Catch ex As Exception
            CheckImage = False
        End Try
    End Function

    Public Sub DeleteAgentImages()
        Try
            If File.Exists((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Thumpnailimage.bmp")) Then
                File.Delete((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Thumpnailimage.bmp"))
            End If
            If File.Exists((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Missinghotelimage.bmp")) Then
                File.Delete((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Missinghotelimage.bmp"))
            End If
            If File.Exists((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Reportimage.bmp")) Then
                File.Delete((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Reportimage.bmp"))
            End If
            If File.Exists((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Otheimage.bmp")) Then
                File.Delete((tt & "\WebAdminModule\UploadedLogo\ " & txtagentcode.Text & "Otheimage.bmp"))
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
