Imports System.Data

Partial Class SalesInvoice
    Inherits System.Web.UI.Page
    Protected Sub lbClose_Click(sender As Object, e As System.EventArgs)

    End Sub
    Protected Sub rbtnadsearch_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbtnadsearch.CheckedChanged

    End Sub

    Protected Sub btnhelp_Click(sender As Object, e As System.EventArgs) Handles btnhelp.Click

    End Sub

    Protected Sub ddlOrder_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlOrder.SelectedIndexChanged

    End Sub

    Protected Sub ddlOrder_TextChanged(sender As Object, e As System.EventArgs) Handles ddlOrder.TextChanged

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("Invoiceno", GetType(String)))
            dt.Columns.Add(New DataColumn("Status", GetType(String)))
            dt.Columns.Add(New DataColumn("Invoicedate", GetType(String)))

            dt.Columns.Add(New DataColumn("FileNumber", GetType(String)))
            dt.Columns.Add(New DataColumn("Customercode", GetType(String)))
            dt.Columns.Add(New DataColumn("Customername", GetType(String)))
            dt.Columns.Add(New DataColumn("Customerref", GetType(String)))
            dt.Columns.Add(New DataColumn("Currency", GetType(String)))
            dt.Columns.Add(New DataColumn("Amount", GetType(String)))
            dt.Columns.Add(New DataColumn("Salesamount", GetType(String)))
            dt.Columns.Add(New DataColumn("Datecreated", GetType(String)))
            dt.Columns.Add(New DataColumn("Usercreated", GetType(String)))
            dt.Columns.Add(New DataColumn("Datemodified", GetType(String)))
            dt.Columns.Add(New DataColumn("Usermodified", GetType(String)))


            Dim dr As DataRow
            Dim drdatarow1 As DataRow
            drdatarow1 = dt.NewRow()
            drdatarow1("Invoiceno") = "174865"
            drdatarow1("Status") = "Posted"
            drdatarow1("Invoicedate") = "16/02/2018"
            drdatarow1("FileNumber") = "87895465"
            drdatarow1("Customercode") = "465465"
            drdatarow1("Customername") = "ELEVATE TOURISM L.L.C"
            drdatarow1("Customerref") = "2344234"
            drdatarow1("Currency") = "AED"
            drdatarow1("Amount") = "234"
            drdatarow1("Salesamount") = "523"
            drdatarow1("Datecreated") = "16/02/2018"
            drdatarow1("Usercreated") = "test"
            drdatarow1("Datemodified") = ""
            drdatarow1("Usermodified") = ""
  


            ''Dim drdatarow2 As DataRow
            ''drdatarow2 = dt.NewRow()
            ''drdatarow2("Customer") = "ELEVATE TOURISM L.L.C"
            ''drdatarow2("RequestDate") = "11/4/2018"
            ''drdatarow2("RequestID") = "EXC/002"
            ''drdatarow2("TourDate") = "13/04/2018"
            ''drdatarow2("Adult") = "1"
            ''drdatarow2("Child") = "1"
            ''drdatarow2("SeniorCitizens") = "0"
            ''drdatarow2("Total") = "2"
            ''drdatarow2("Units") = "0"
            ''drdatarow2("GuestName") = "Mr.Varun"
            ''drdatarow2("Hotel") = "Atlantis the Palm"
            ''drdatarow2("Pickuptime") = "12:30"
            ''drdatarow2("ServiceRequested") = "Dhow Cruise"

            dt.Rows.Add(drdatarow1)
            ''dt.Rows.Add(drdatarow2)
            For i = 1 To 5

                dr = dt.NewRow()
                dt.Rows.Add(dr)
            Next i

            gv_SearchResult.DataSource = dt
            gv_SearchResult.DataBind()



        End If
    End Sub
End Class
