#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class AccountsModule_RptProfitability
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
    Dim month, lastday As String
    Dim max As Integer
    Dim sqlTrans As SqlTransaction
    Dim day As Date
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            Try

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If


                hidecontrols()
                '----------------------------- Default Dates
                txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txttoDate.Text = txtFromDate.Text


                '-------------------------------------------
                'txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
                'txttoDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                ''    rptall.Checked = True
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomercode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomername, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomercodeto, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomernameto, "agentname", "agentcode", " select agentname,agentcode from agentmast where active=1 order by agentname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupcode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupname, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupcodeto, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupnameto, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketcode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketname, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketcodeto, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketnameto, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlacccodefrom, "acctcode", "acctname", "select acctcode,acctname from acctgroup where  childid in (115,120)  order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlaccnamefrom, "acctname", "acctcode", "select acctname,acctcode from acctgroup where  childid in (115,120)   order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlacccodeto, "acctcode", "acctname", "select acctcode,acctname from acctgroup where  childid in (115,120)  order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlaccnameto, "acctname", "acctcode", "select acctname,acctcode from acctgroup where  childid in (115,120)  order by acctname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsalesfrom, "usercode", "username", "select UserCode,UserName from UserMaster where active=1  order by UserCode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsalesnamefrom, "username", "usercode", "select UserName,UserCode from UserMaster where  active=1  order by UserName", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsalesto, "usercode", "username", "select UserCode,UserName from UserMaster where active=1  order by UserCode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsalenameto, "username", "usercode", "select UserName,UserCode from UserMaster where  active=1  order by UserName", True)


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlcustomercode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcustomername.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcustomercodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcustomernameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlsupcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlsupname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlsupcodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlsupnameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlmarketcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlmarketname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlmarketcodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlmarketnameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlacccodefrom.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlaccnamefrom.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlacccodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlaccnameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlsalesfrom.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlsalenameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlsalesto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlsalesnamefrom.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If


                rbmarketall.Attributes.Add("onclick", "rbevent(this,'" & rbmarketrange.ClientID & "','A','Market')")
                rbmarketrange.Attributes.Add("onclick", "rbevent(this,'" & rbmarketall.ClientID & "','R','Market')")
                rbCustall.Attributes.Add("onclick", "rbevent(this,'" & rbcustrange.ClientID & "','A','Customer')")
                rbcustrange.Attributes.Add("onclick", "rbevent(this,'" & rbCustall.ClientID & "','R','Customer')")
                rbsupall.Attributes.Add("onclick", "rbevent(this,'" & rbsuprange.ClientID & "','A','Supplier')")
                rbsuprange.Attributes.Add("onclick", "rbevent(this,'" & rbsupall.ClientID & "','R','Supplier')")
                rbaccountsall.Attributes.Add("onclick", "rbevent(this,'" & rbaccountsrange.ClientID & "','A','Accounts')")
                rbaccountsrange.Attributes.Add("onclick", "rbevent(this,'" & rbaccountsall.ClientID & "','R','Accounts')")

                rbsalesall.Attributes.Add("onclick", "rbevent(this,'" & rbsalesrange.ClientID & "','A','Sales')")
                rbsalesrange.Attributes.Add("onclick", "rbevent(this,'" & rbsalesall.ClientID & "','R','Sales')")

                If ddlrpttype.SelectedIndex = 0 Then
                    ddlbooktype.Style("display") = "none"
                    lblBookType.Style("display") = "none"
                ElseIf ddlrpttype.SelectedIndex = 1 Then
                    ddlbooktype.Style("display") = "block"
                    lblBookType.Style("display") = "block"
                End If

                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepProfWindowPostBack") Then
                    btnLoadreport_Click(sender, e)
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptTrialBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            If rbmarketall.Checked Then
                ddlmarketcode.Style("visibility") = "hidden"
                ddlmarketcodeto.Style("visibility") = "hidden"
                ddlmarketname.Style("visibility") = "hidden"
                ddlmarketnameto.Style("visibility") = "hidden"
                ddlmarketcode.Disabled = True
                ddlmarketcodeto.Disabled = True
                ddlmarketname.Disabled = True
                ddlmarketnameto.Disabled = True
                ddlmarketcode.Value = "[Select]"
                ddlmarketcodeto.Value = "[Select]"
                ddlmarketname.Value = "[Select]"
                ddlmarketnameto.Value = "[Select]"
            End If
            If rbmarketrange.Checked Then
                ddlmarketcode.Style("visibility") = "visible"
                ddlmarketcodeto.Style("visibility") = "visible"
                ddlmarketname.Style("visibility") = "visible"
                ddlmarketnameto.Style("visibility") = "visible"
                ddlmarketcode.Disabled = False
                ddlmarketcodeto.Disabled = False
                ddlmarketname.Disabled = False
                ddlmarketnameto.Disabled = False
            End If

            If rbCustall.Checked Then
                ddlcustomercode.Style("visibility") = "hidden"
                ddlcustomercodeto.Style("visibility") = "hidden"
                ddlcustomername.Style("visibility") = "hidden"
                ddlcustomernameto.Style("visibility") = "hidden"
                ddlcustomercode.Disabled = True
                ddlcustomercodeto.Disabled = True
                ddlcustomername.Disabled = True
                ddlcustomernameto.Disabled = True
                ddlcustomercode.Value = "[Select]"
                ddlcustomercodeto.Value = "[Select]"
                ddlcustomername.Value = "[Select]"
                ddlcustomernameto.Value = "[Select]"
            End If
            If rbcustrange.Checked Then
                ddlcustomercode.Style("visibility") = "visible"
                ddlcustomercodeto.Style("visibility") = "visible"
                ddlcustomername.Style("visibility") = "visible"
                ddlcustomernameto.Style("visibility") = "visible"
                ddlcustomercode.Disabled = False
                ddlcustomercodeto.Disabled = False
                ddlcustomername.Disabled = False
                ddlcustomernameto.Disabled = False
            End If

          
            If rbsupall.Checked Then
                ddlsupcode.Style("visibility") = "hidden"
                ddlsupcodeto.Style("visibility") = "hidden"
                ddlsupname.Style("visibility") = "hidden"
                ddlsupnameto.Style("visibility") = "hidden"
                ddlsupcode.Disabled = True
                ddlsupcodeto.Disabled = True
                ddlsupname.Disabled = True
                ddlsupnameto.Disabled = True
                ddlsupcode.Value = "[Select]"
                ddlsupcodeto.Value = "[Select]"
                ddlsupname.Value = "[Select]"
                ddlsupnameto.Value = "[Select]"
            End If
            If rbsuprange.Checked Then
                ddlsupcode.Style("visibility") = "visible"
                ddlsupcodeto.Style("visibility") = "visible"
                ddlsupname.Style("visibility") = "visible"
                ddlsupnameto.Style("visibility") = "visible"
                ddlsupcode.Disabled = False
                ddlsupcodeto.Disabled = False
                ddlsupname.Disabled = False
                ddlsupnameto.Disabled = False
            End If

            If rbaccountsall.Checked Then
                ddlacccodefrom.Style("visibility") = "hidden"
                ddlacccodeto.Style("visibility") = "hidden"
                ddlaccnamefrom.Style("visibility") = "hidden"
                ddlaccnameto.Style("visibility") = "hidden"
                ddlacccodefrom.Disabled = True
                ddlacccodeto.Disabled = True
                ddlaccnamefrom.Disabled = True
                ddlaccnameto.Disabled = True
                ddlacccodefrom.Value = "[Select]"
                ddlacccodeto.Value = "[Select]"
                ddlaccnamefrom.Value = "[Select]"
                ddlaccnameto.Value = "[Select]"
            End If
            If rbaccountsrange.Checked Then
                ddlacccodefrom.Style("visibility") = "visible"
                ddlacccodeto.Style("visibility") = "visible"
                ddlaccnamefrom.Style("visibility") = "visible"
                ddlaccnameto.Style("visibility") = "visible"
                ddlacccodefrom.Disabled = False
                ddlacccodeto.Disabled = False
                ddlaccnamefrom.Disabled = False
                ddlaccnameto.Disabled = False
            End If

            If rbsalesall.Checked Then
                ddlsalesfrom.Style("visibility") = "hidden"
                ddlsalesnamefrom.Style("visibility") = "hidden"
                ddlsalesto.Style("visibility") = "hidden"
                ddlsalenameto.Style("visibility") = "hidden"
                ddlsalesfrom.Disabled = True
                ddlsalesnamefrom.Disabled = True
                ddlsalenameto.Disabled = True
                ddlsalesto.Disabled = True
                ddlsalesfrom.Value = "[Select]"
                ddlsalesnamefrom.Value = "[Select]"
                ddlsalenameto.Value = "[Select]"
                ddlsalesto.Value = "[Select]"
            End If

            If rbsalesrange.Checked Then
                ddlsalesfrom.Style("visibility") = "visible"
                ddlsalesnamefrom.Style("visibility") = "visible"
                ddlsalesto.Style("visibility") = "visible"
                ddlsalenameto.Style("visibility") = "visible"
                ddlsalesfrom.Disabled = False
                ddlsalesnamefrom.Disabled = False
                ddlsalenameto.Disabled = False
                ddlsalesto.Disabled = False
            End If

        End If

        If ddlrpttype.Value = 1 Then
            btnLoadgrid.Attributes.Add("style", "display:block")
        Else
            btnLoadgrid.Attributes.Add("style", "display:none")
        End If

        btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")

    End Sub

   Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~\MainPage.aspx", False)
    End Sub

    Protected Sub btnLoadreport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            BindDataAndTransfer(1)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptProfitability.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Private Sub BindDataAndTransfer(ByVal Isreport As Integer)
        Try

            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""

            Dim strplgrpcode As String = ""
            Dim strplgrpcodeto As String = ""

            Dim stragentcode As String = ""
            Dim stragentcodeto As String = ""

            Dim strsupcode As String = ""
            Dim strsupcodeto As String = ""

            Dim stracccode As String = ""
            Dim stracccodeto As String = ""

            Dim strsalescode As String = ""
            Dim strsalescodeto As String = ""


            Dim strrepfilter As String = ""
            Dim strrpttype As Integer = 0


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strrepfilter = "From Date: " & Format(CType(txtFromDate.Text, Date), "dd/MM/yyyy")
            strrepfilter = strrepfilter & " - To Date: " & Format(CType(txttoDate.Text, Date), "dd/MM/yyyy")


            strplgrpcode = IIf(UCase(ddlmarketcode.Items(ddlmarketcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlmarketcode.Items(ddlmarketcode.SelectedIndex).Text, "")
            strplgrpcodeto = IIf(UCase(ddlmarketcodeto.Items(ddlmarketcodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlmarketcodeto.Items(ddlmarketcodeto.SelectedIndex).Text, "")


            strsalescode = IIf(UCase(ddlsalesfrom.Items(ddlsalesfrom.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsalesfrom.Items(ddlsalesfrom.SelectedIndex).Text, "")
            strsalescodeto = IIf(UCase(ddlsalesto.Items(ddlsalesto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsalesto.Items(ddlsalesto.SelectedIndex).Text, "")


            If ddlmarketname.Items(ddlmarketname.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " ; From Market: " & ddlmarketname.Items(ddlmarketname.SelectedIndex).Text
            End If
            If ddlmarketnameto.Items(ddlmarketnameto.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " - To Market: " & ddlmarketnameto.Items(ddlmarketnameto.SelectedIndex).Text
            End If

            If ddlsalesnamefrom.Items(ddlsalesnamefrom.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " ; From Sales Person: " & ddlsalesnamefrom.Items(ddlsalesnamefrom.SelectedIndex).Text
            End If
            If ddlsalenameto.Items(ddlsalenameto.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " - To Sales Person: " & ddlsalenameto.Items(ddlsalenameto.SelectedIndex).Text
            End If

            stragentcode = IIf(UCase(ddlcustomercode.Items(ddlcustomercode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustomercode.Items(ddlcustomercode.SelectedIndex).Text, "")
            stragentcodeto = IIf(UCase(ddlcustomercodeto.Items(ddlcustomercodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustomercodeto.Items(ddlcustomercodeto.SelectedIndex).Text, "")
            If ddlcustomername.Items(ddlcustomername.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " ; From Customer: " & ddlcustomername.Items(ddlcustomername.SelectedIndex).Text
            End If
            If ddlcustomernameto.Items(ddlcustomernameto.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " - To Customer: " & ddlcustomernameto.Items(ddlcustomernameto.SelectedIndex).Text
            End If

            strsupcode = IIf(UCase(ddlsupcode.Items(ddlsupcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsupcode.Items(ddlsupcode.SelectedIndex).Text, "")
            strsupcodeto = IIf(UCase(ddlsupcodeto.Items(ddlsupcodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsupcodeto.Items(ddlsupcodeto.SelectedIndex).Text, "")
            If ddlsupname.Items(ddlsupname.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " ; From Supplier Agent: " & ddlsupname.Items(ddlsupname.SelectedIndex).Text
            End If
            If ddlsupnameto.Items(ddlsupnameto.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " - To Supplier Agent: " & ddlsupnameto.Items(ddlsupnameto.SelectedIndex).Text
            End If

            stracccode = IIf(UCase(ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text, "")
            stracccodeto = IIf(UCase(ddlacccodeto.Items(ddlacccodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlacccodeto.Items(ddlacccodeto.SelectedIndex).Text, "")
            If ddlaccnamefrom.Items(ddlaccnamefrom.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " ; From Account: " & ddlaccnamefrom.Items(ddlaccnamefrom.SelectedIndex).Text
            End If
            If ddlaccnameto.Items(ddlaccnameto.SelectedIndex).Text <> "[Select]" Then
                strrepfilter = strrepfilter & " - To Account: " & ddlaccnameto.Items(ddlaccnameto.SelectedIndex).Text
            End If
            If ddlrpttype.SelectedIndex = 0 Then
                ddlbooktype.Style("display") = "none"
                lblBookType.Style("display") = "none"
            ElseIf ddlrpttype.SelectedIndex = 1 Then
                ddlbooktype.Style("display") = "block"
                lblBookType.Style("display") = "block"
            End If

            Dim strpop As String = ""
            If Isreport = 1 Then
                'strpop = "window.open('rptProfitabilityReportsearch.aspx?fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&supcode=" & strsupcode & "&supcodeto=" & strsupcodeto _
                '& "&plgrpcode=" & strplgrpcode & "&plgrpcodeto=" & strplgrpcodeto _
                '& "&salescodefrom=" & strsalescode & "&salescodeto=" & strsalescodeto _
                '& "&acccode=" & stracccode & "&acccodeto=" & stracccodeto _
                '& "&compliment=" & stragentcode & "&booktype=" & ddlbooktype.SelectedIndex & "&rpttype=" & ddlrpttype.SelectedIndex & "&groupby=" & ddlgroupby.SelectedIndex & "&agentcode=" & stragentcode & "&agentcodeto=" & stragentcodeto & "','Profitability','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"



                strpop = "window.open('rptProfitabilityReportsearch.aspx?fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&supcode=" & strsupcode & "&supcodeto=" & strsupcodeto _
                & "&plgrpcode=" & strplgrpcode & "&plgrpcodeto=" & strplgrpcodeto _
                & "&salescodefrom=" & strsalescode & "&salescodeto=" & strsalescodeto _
                & "&acccode=" & stracccode & "&acccodeto=" & stracccodeto _
                & "&compliment=" & stragentcode & "&booktype=" & ddlbooktype.SelectedIndex & "&rpttype=" & ddlrpttype.SelectedIndex & "&groupby=" & ddlgroupby.SelectedIndex & "&agentcode=" & stragentcode & "&agentcodeto=" & stragentcodeto & "','Profitability');"
            Else
                '    strpop = "window.open('ProfitabilityDetails.aspx?fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&supcode=" & strsupcode & "&supcodeto=" & strsupcodeto _
                '& "&plgrpcode=" & strplgrpcode & "&plgrpcodeto=" & strplgrpcodeto _
                ' & "&salescodefrom=" & strsalescode & "&salescodeto=" & strsalescodeto _
                '& "&acccode=" & stracccode & "&acccodeto=" & stracccodeto _
                '& "&compliment=" & stragentcode & "&booktype=" & ddlbooktype.SelectedIndex & "&rpttype=" & ddlrpttype.SelectedIndex & "&groupby=" & ddlgroupby.SelectedIndex & "&agentcode=" & stragentcode & "&agentcodeto=" & stragentcodeto & "','Profitability','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ProfitabilityDetails.aspx?fromdate=" & strfromdate & "&todate=" & strtodate _
            & "&supcode=" & strsupcode & "&supcodeto=" & strsupcodeto _
            & "&plgrpcode=" & strplgrpcode & "&plgrpcodeto=" & strplgrpcodeto _
             & "&salescodefrom=" & strsalescode & "&salescodeto=" & strsalescodeto _
            & "&acccode=" & stracccode & "&acccodeto=" & stracccodeto _
            & "&compliment=" & stragentcode & "&booktype=" & ddlbooktype.SelectedIndex & "&rpttype=" & ddlrpttype.SelectedIndex & "&groupby=" & ddlgroupby.SelectedIndex & "&agentcode=" & stragentcode & "&agentcodeto=" & stragentcodeto & "','Profitability');"

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub hidecontrols()
        ddlmarketcode.Style("visibility") = "hidden"
        ddlmarketname.Style("visibility") = "hidden"
        ddlmarketcodeto.Style("visibility") = "hidden"
        ddlmarketnameto.Style("visibility") = "hidden"

        ddlcustomercode.Style("visibility") = "hidden"
        ddlcustomername.Style("visibility") = "hidden"
        ddlcustomercodeto.Style("visibility") = "hidden"
        ddlcustomernameto.Style("visibility") = "hidden"

        ddlsupcode.Style("visibility") = "hidden"
        ddlsupname.Style("visibility") = "hidden"
        ddlsupcodeto.Style("visibility") = "hidden"
        ddlsupnameto.Style("visibility") = "hidden"

        ddlacccodefrom.Style("visibility") = "hidden"
        ddlaccnamefrom.Style("visibility") = "hidden"
        ddlacccodeto.Style("visibility") = "hidden"
        ddlaccnameto.Style("visibility") = "hidden"


        ddlsalesfrom.Style("visibility") = "hidden"
        ddlsalesnamefrom.Style("visibility") = "hidden"
        ddlsalenameto.Style("visibility") = "hidden"
        ddlsalesto.Style("visibility") = "hidden"

        lblmarketcode.Style("visibility") = "hidden"
        lblmarketname.Style("visibility") = "hidden"
        lblmarketcodeto.Style("visibility") = "hidden"
        lblmarketnameto.Style("visibility") = "hidden"

        lblcustomercode.Style("visibility") = "hidden"
        lblcustomername.Style("visibility") = "hidden"
        lblcustomercodeto.Style("visibility") = "hidden"
        lblcustomernameto.Style("visibility") = "hidden"


        lblsupcode.Style("visibility") = "hidden"
        lblsupname.Style("visibility") = "hidden"
        lblsupcodeto.Style("visibility") = "hidden"
        lblsupnameto.Style("visibility") = "hidden"

        lblaccountscode.Style("visibility") = "hidden"
        lblaccountsname.Style("visibility") = "hidden"
        lblaccountscodeto.Style("visibility") = "hidden"
        lblaccountsnameto.Style("visibility") = "hidden"


        lblsalescodefrom.Style("visibility") = "hidden"
        lblsalesnamefrom.Style("visibility") = "hidden"
        lblsalescodeto.Style("visibility") = "hidden"
        lblsalesnameto.Style("visibility") = "hidden"
    End Sub

    Public Function ValidatePage() As Boolean
        Try
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter from date');", True)
                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter to date');", True)
                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                ValidatePage = False
                Exit Function
            End If
            ValidatePage = True
        Catch ex As Exception

        End Try
    End Function

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        ''If Page.IsPostBack = False Then

        'If Request.Params("Type") = "I" Then
        '    Me.MasterPageFile = "~/AccountsMaster.master"

        '    'Else
        '    '    Me.MasterPageFile = "~/ReservationMaster.master"
        'End If



        ''End If

        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   'changed by mohamed on 27/08/2018
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

  
    Protected Sub btnLoadgrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadgrid.Click
        Try
            BindDataAndTransfer(0)
        Catch ex As Exception

        End Try
    End Sub
End Class
