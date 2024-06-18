'------------================--------------=======================------------------================
'   Module Name    :    CreditNotePrint.aspx
'   Developer Name :    Jaffer
'   Date           :    08 Feb 2009
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Collections.Generic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#End Region
Partial Class AccountsModule_CreditNotePrint
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objEmail As New clsEmail
    Dim creditnoteno As String = ""
    Dim strMessage As String = ""
    Dim strSubject As String = ""
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As DataTable
        ViewState.Add("ResState", Request.QueryString("State"))
        ViewState.Add("CreditNoteNo", Request.QueryString("CreditNoteNo"))
        txtconnection.Value = Session("dbconnectionName")

        If Page.IsPostBack = False Then


            If ViewState("CreditNoteNo") Is Nothing = False Then
                creditnoteno = CType(ViewState("CreditNoteNo"), String)
                'If Request.QueryString("requestid") Is Nothing = False Then
                '    requestid = CType(Request.QueryString("requestid"), String)
                'End If
                dt = CreateTabele()
                Dim dr As DataRow
                dr = dt.NewRow()
                Dim strSql As String = "exec sp_get_creditnote_printpage '" + creditnoteno + "'"
                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSql)
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("creditnoteno")) = False Then
                            txtCreditNoteNo.Value = CType(ds.Tables(0).Rows(0)("requestid"), String)
                        End If
                        If IsDBNull(ds.Tables(0).Rows(0)("reqdate")) = False Then
                            txtReqDate.Value = CType(ds.Tables(0).Rows(0)("reqdate"), String)
                        End If
                        'If IsDBNull(ds.Tables(0).Rows(0)("usercode")) = False Then
                        '    txtUserLogged.Value = CType(ds.Tables(0).Rows(0)("usercode"), String)
                        'End If
                        If IsDBNull(ds.Tables(0).Rows(0)("agentcode")) = False Then
                            txtCustcode.Value = CType(ds.Tables(0).Rows(0)("agentcode"), String)
                            dr("agentcode") = CType(ds.Tables(0).Rows(0)("agentcode"), String)
                        Else
                            dr("agentcode") = ""
                        End If
                        If IsDBNull(ds.Tables(0).Rows(0)("agentname")) = False Then
                            txtCustName.Value = CType(ds.Tables(0).Rows(0)("agentname"), String)
                            dr("agentname") = CType(ds.Tables(0).Rows(0)("agentname"), String)
                        Else
                            dr("agentname") = ""
                        End If
                        'If IsDBNull(ds.Tables(0).Rows(0)("plgrpcode")) = False Then
                        '    txtMarketCode.Value = CType(ds.Tables(0).Rows(0)("plgrpcode"), String)
                        'End If
                        'If IsDBNull(ds.Tables(0).Rows(0)("plgrpname")) = False Then
                        '    txtMarketName.Value = CType(ds.Tables(0).Rows(0)("plgrpname"), String)
                        'End If
                        'If IsDBNull(ds.Tables(0).Rows(0)("currcode")) = False Then
                        '    txtCurrCode.Value = CType(ds.Tables(0).Rows(0)("currcode"), String)
                        'End If
                        'If IsDBNull(ds.Tables(0).Rows(0)("convrate")) = False Then
                        '    txtConvertRate.Value = CType(ds.Tables(0).Rows(0)("convrate"), String)
                        'End If
                        'If IsDBNull(ds.Tables(0).Rows(0)("agentref")) = False Then
                        '    txtAgentref.Value = CType(ds.Tables(0).Rows(0)("agentref"), String)
                        'End If
                        'If IsDBNull(ds.Tables(0).Rows(0)("designation")) = False Then
                        '    Session.Add("Desg", ds.Tables(0).Rows(0)("designation"))
                        'End If
                        If IsDBNull(ds.Tables(0).Rows(0)("email")) = False Then
                            dr("email") = CType(ds.Tables(0).Rows(0)("email"), String)
                        Else
                            dr("email") = ""
                        End If

                        If IsDBNull(ds.Tables(0).Rows(0)("attn")) = False Then
                            dr("contact") = CType(ds.Tables(0).Rows(0)("attn"), String)
                        Else
                            dr("contact") = ""
                        End If
                        dt.Rows.Add(dr)
                        dt.AcceptChanges()
                        gvCreditNote.DataSource = dt
                        gvCreditNote.DataBind()
                    End If
                End If
            Else
                'Response.Redirect("ReservationSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ReservationWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        End If
        btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")

    End Sub
#Region "Public Function CreateTabele()"
    Public Function CreateTabele() As DataTable
        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("agentcode")
        dt.Columns.Add("agentname")
        dt.Columns.Add("email")
        dt.Columns.Add("contact")
        dt.AcceptChanges()
        Return dt
    End Function
#End Region

    Protected Sub gvInvoice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Try
            creditnoteno = CType(ViewState("CreditNoteNo"), String)
            If e.CommandName = "Print" Then
                Dim rowindex As Integer = CInt(e.CommandArgument)
                Dim row As GridViewRow = gvCreditNote.Rows(rowindex)
                Dim lblSupAgentCode As HtmlInputText = DirectCast(row.FindControl("lblSupAgentCode"), HtmlInputText)
                Dim strpop As String = ""
                strpop = "window.open('rptCreditNote.aspx?CreditNo=" + creditnoteno + " ','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
            If e.CommandName = "Mail" Then
                Dim rowindex As Integer = CInt(e.CommandArgument)
                Dim row As GridViewRow = gvCreditNote.Rows(rowindex)
                Dim lblSupAgentCode As HtmlInputText = DirectCast(row.FindControl("lblSupAgentCode"), HtmlInputText)
                Dim lblAvailable As Label = DirectCast(row.FindControl("lblAvailable"), Label)
                Dim txtEmail As HtmlInputText = CType(row.FindControl("txtEmail"), HtmlInputText)
                Dim txtContact As HtmlInputText = CType(row.FindControl("txtContact"), HtmlInputText)
                If txtEmail.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Email Id' );", True)
                    Return
                End If
                If objUtils.ValidateEmail(txtEmail.Value) = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid Email Id' );", True)
                    Return
                End If
                Dim requestid As String = CType(Session("RequestId"), String)
                SendMailInvoice(txtCreditNoteNo.Value, txtContact.Value, txtEmail.Value)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("InvoicePrint.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvInvoice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim strSql As String = ""
            Dim lnkMail As LinkButton = CType(e.Row.FindControl("lnkMail"), LinkButton)
            Dim lblSupAgentCode As HtmlInputText = CType(e.Row.FindControl("lblSupAgentCode"), HtmlInputText)
            Dim ddlcontact As HtmlSelect = CType(e.Row.FindControl("ddlAgnet"), HtmlSelect)

            Dim txtEmail As HtmlInputText = CType(e.Row.FindControl("txtEmail"), HtmlInputText)
            Dim txtContact As HtmlInputText = CType(e.Row.FindControl("txtContact"), HtmlInputText)

            strSql = "select contact1 from view_agentmast_multiemail where agentcode='" + lblSupAgentCode.Value + "'"

            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSql)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    ddlcontact.DataSource = ds.Tables(0)
                    ddlcontact.DataValueField = "contact1"
                    ddlcontact.DataTextField = "contact1"
                    ddlcontact.DataBind()
                    ddlcontact.Value = txtContact.Value
                End If
            End If
            ddlcontact.Attributes.Add("onchange", "javascript:FillEmailId('" + CType(lblSupAgentCode.ClientID, String) + "','" + CType(ddlcontact.ClientID, String) + "','C','" + CType(txtEmail.ClientID, String) + "','" + CType(txtContact.ClientID, String) + "')")
            lnkMail.Attributes.Add("onclick", "javascript:if(confirm('Are you sure to send mail?')==false)return false;")
        End If
    End Sub
#Region "Public Function SendMailInvoice()"
    Public Function SendMailInvoice(ByVal requestid As String, ByVal ContactPerson As String, ByVal ToAdd As String)
        Dim rnd As Random = New Random
        Dim strFilename As String = ""
        Dim strFullpath As String = ""
        Try

            strFilename = rnd.Next.ToString()
            strFullpath = Server.MapPath(".")
            strFullpath += "\\SavedReports\" + strFilename + ".pdf"

            Dim rep As New ReportDocument
            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue
            Dim ConnInfo As New ConnectionInfo
            'With ConnInfo
            '    .ServerName = ConfigurationManager.AppSettings("dbServerName")
            '    .DatabaseName = ConfigurationManager.AppSettings("dbDatabaseName")
            '    .UserID = ConfigurationManager.AppSettings("dbUserName")
            '    .Password = ConfigurationManager.AppSettings("dbPassword")
            'End With
            With ConnInfo
                .ServerName = Session("dbServerName")
                .DatabaseName = Session("dbDatabaseName")
                .UserID = Session("dbUserName")
                .Password = Session("dbPassword")
            End With

            rep.Load(Server.MapPath("~\Report\rptInvoice.rpt"))

            Dim RepTbls As Tables = rep.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            pnames = rep.DataDefinition.ParameterFields

            pname = pnames.Item("@requestid")
            paramvalue.Value = requestid
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("CompanyName")
            paramvalue.Value = CType(Session("CompanyName"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            '****************added for report footer- common for above  reports****************************
            pname = pnames.Item("rptComId")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1050)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine1")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1051)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine2")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1052)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine3")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            '************************************************************************************************



            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            rep.ExportToDisk(ExportFormatType.PortableDocFormat, strFullpath)

            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "alert", "alert('Are You Sure to Send Mail to ');", True)
            ''   strSubject = "Booking Ref.no:" + CType(Session("RequestId"), String)

            strSubject = "Invoice for Booking ref:" + CType(txtCreditNoteNo.Value, String)
            strMessage = "Dear " + ContactPerson + "&nbsp;<br /><br />&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;"
            strMessage += "Please find attached Invoice for Booking ref.no  : " + CType(txtCreditNoteNo.Value, String) + "<br /><br />"
            strMessage += "<br />Regards<br />" + +"<br />" + CType(Session("Desg"), String) + "<br />" + CType(Session("ComapnyName"), String)
            Dim from_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
            If objEmail.SendEmail(from_email, ToAdd, strSubject, strMessage, strFullpath) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + ToAdd + "');", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + ToAdd + "');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("InvoicePrint.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            Dim thefile As FileInfo = New FileInfo(strFullpath)
            If thefile.Exists Then
                File.Delete(strFullpath)
            End If
        End Try
        SendMailInvoice = ""
    End Function
#End Region

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ReservationWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Dim strscript As String = ""
        'strscript = "window.opener.__doPostBack('ChildWindowPostBack', '');window.opener.focus();window.close();"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Response.Redirect("ReservationInvoiceSearch.aspx", False)
    End Sub

    Protected Sub gvInvoice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
End Class
