'------------================--------------=======================------------------================
'   Module Name    :    RptSupplierInvoicesNotRcvd.aspx 
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class RptSupplierInvoicesNotRcvd
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlstr1, sqlstr2 As String
#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                End If
                SetFocus(ddlSupType)
               

                If ddlSupType.Value <> "[Select]" Then
                    sqlstr1 = "select Code,des from view_account where type = '" + ddlSupType.Value + "' order by code"
                    sqlstr2 = "select des,Code from view_account where type = '" + ddlSupType.Value + "' order by des"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplier, "code", "des", sqlstr1, True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplierName, "des", "code", sqlstr2, True)

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplier, "code", "des", sqlstr1, True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplierName, "des", "code", sqlstr2, True)
                Else
                    sqlstr1 = "select top 10  Code,des from view_account order by code"
                    sqlstr2 = "select top 10 des,Code from view_account  order by des"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplier, "code", "des", sqlstr1, True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplierName, "des", "code", sqlstr2, True)

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplier, "code", "des", sqlstr1, True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplierName, "des", "code", sqlstr2, True)
                End If





                '----------------------------- Default Dates
                txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txtToDate.Text = txtFromDate.Text
                'txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
                '-------------------------------------------
                ddlSupType.Attributes.Add("onchange", "javascript:fillSup('" + CType(ddlSupType.ClientID, String) + "')")
                rbSupall.Attributes.Add("onclick", "javascript:rbevent('" + CType(rbSupall.ClientID, String) + "','" + CType(rbSuprange.ClientID, String) + "','A')")
                rbSuprange.Attributes.Add("onclick", "javascript:rbevent('" + CType(rbSupall.ClientID, String) + "','" + CType(rbSuprange.ClientID, String) + "','R')")


                btnLoadreport.Attributes.Add("onclick", "return FormValidation()")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog(" ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
        Dim typ As Type
        typ = GetType(DropDownList)
        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
            ddlSupType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlReportGroup.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlFromSupplier.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlFromSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlToSupplier.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlToSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = True Then
            Try

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                End If
                If rbSupall.Checked = False Then
                    ddlFromSupplier.Disabled = False
                    ddlFromSupplierName.Disabled = False
                    ddlToSupplier.Disabled = False
                    ddlToSupplierName.Disabled = False

                    If ddlSupType.Value <> "[Select]" Then
                        sqlstr1 = "select Code,des from view_account where type = '" + ddlSupType.Value + "' order by code"
                        sqlstr2 = "select des,Code from view_account where type = '" + ddlSupType.Value + "' order by des"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplier, "code", "des", sqlstr1, True, txtFromSupplier.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplierName, "des", "code", sqlstr2, True, txtFromSupplierName.Value)

                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplier, "code", "des", sqlstr1, True, txtToSupplier.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplierName, "des", "code", sqlstr2, True, txtToSupplierName.Value)
                    Else
                        sqlstr1 = "select top 10  Code,des from view_account order by code"
                        sqlstr2 = "select top 10 des,Code from view_account  order by des"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplier, "code", "des", sqlstr1, True, txtFromSupplier.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromSupplierName, "des", "code", sqlstr2, True, txtFromSupplierName.Value)

                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplier, "code", "des", sqlstr1, True, txtToSupplier.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToSupplierName, "des", "code", sqlstr2, True, txtToSupplierName.Value)
                    End If
                Else
                    ddlFromSupplier.Disabled = True
                    ddlFromSupplierName.Disabled = True
                    ddlToSupplier.Disabled = True
                    ddlToSupplierName.Disabled = True
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog(" ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSupplierInvoicesNotRcvd','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub btnLoadreport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ValidatePage() = True Then
            Try

                Dim strfromdate, strtodate, straccttype, strfromcode, strtocode, strgrporder As String

                strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy-MM-dd 00:00:00")
                strtodate = Format(CType(txtToDate.Text, Date), "yyyy-MM-dd 00:00:00")

                straccttype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
                strfromcode = IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
                strtocode = IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")

                strgrporder = ddlReportGroup.SelectedIndex
                Dim strpop As String = ""
                'strpop = "window.open('RptSupplierInvoicesNotRcvdReport.aspx?Pageame=RptSupplierInvoicesNotRcvd&BackPageName=RptSupplierInvoicesNotRcvd.aspx&frmdate=" & strfromdate & "&todate=" & strtodate _
                '& "&acctype=" & straccttype & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&grporder=" & strgrporder & "','RptSupplierInvoicesNotRcvdRep','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                strpop = "window.open('RptSupplierInvoicesNotRcvdReport.aspx?Pageame=RptSupplierInvoicesNotRcvd&BackPageName=RptSupplierInvoicesNotRcvd.aspx&frmdate=" & strfromdate & "&todate=" & strtodate _
                & "&acctype=" & straccttype & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&grporder=" & strgrporder & "','RptSupplierInvoicesNotRcvdRep');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptSalesRegisterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
    Public Function ValidatePage() As Boolean

        If CType(objDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDate.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
            SetFocus(txttoDate)
            ValidatePage = False
            Exit Function
        End If
    End Function
End Class
