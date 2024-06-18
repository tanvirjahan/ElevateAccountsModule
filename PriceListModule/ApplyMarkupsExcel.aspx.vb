Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Imports System.IO
Imports System.Drawing
Imports System.Configuration

Partial Class PriceListModule_ApplyMarkupsExcel
    Inherits System.Web.UI.Page


#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    'Dim sqlTrans As SqlTransaction
    'Dim SqlConn As New SqlConnection
    Dim myCommand As SqlCommand
#End Region

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                fnViewMarkups()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ViewAdultChildCombinationRateExcel.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            'ModalPopupDays.Hide()
        End Try
    End Sub

    Public Function fnViewMarkups() As String
        Dim ds As DataSet
        ds = Session("ViewMarkupsExcelExport")
        gvSearch.AllowPaging = False
        gvSearch.DataSource = ds.Tables(0)
        gvSearch.DataBind()

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=testexcel.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            gvSearch.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using

        Session("ViewMarkupsExcelExport") = Nothing

        Return "Success"
    End Function
End Class
