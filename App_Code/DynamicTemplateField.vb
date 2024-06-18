Imports Microsoft.VisualBasic
Imports System.Web.UI

Public Class DynamicTemplateField
    Implements ITemplate

    Public _colName As String
    Sub New(ByVal colName As String)
        _colName = colName
    End Sub

    Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
        Dim gTable As New Table()
        Dim gTableRow As New TableRow()
        Dim gTablecell As New TableCell()
        gTablecell.Style("width") = "55%"
        Dim img As New Image
        img.ImageUrl = "~/Images/bullet-amber.png"
        img.CssClass = "imageStyle"
        img.ID = "imgB2B" + _colName
        img.Visible = False
        gTablecell.Controls.Add(img)
        Dim lbl As New Label
        lbl.ID = "lblB2B" + _colName
        lbl.CssClass = "LabelDynamic"
        lbl.Text = ""
        gTablecell.Controls.Add(lbl)
        Dim brctr As New HtmlGenericControl
        brctr.InnerHtml = "<br/>"
        brctr.ID = "brB2B" + _colName
        brctr.Visible = False
        gTablecell.Controls.Add(brctr)
        Dim img1 As New Image
        img1.ImageUrl = "~/Images/bullet-yellow.ico"
        img1.CssClass = "imageStyle"
        img1.ID = "imgFinancial" + _colName
        img1.Visible = False
        gTablecell.Controls.Add(img1)
        Dim lbl1 As New Label
        lbl1.ID = "lblFinancial" + _colName
        lbl1.CssClass = "LabelDynamic"
        lbl1.Text = ""
        gTablecell.Controls.Add(lbl1)
        Dim brctr1 As New HtmlGenericControl
        brctr1.InnerHtml = "<br/>"
        brctr1.ID = "brFinancial" + _colName
        brctr1.Visible = False
        gTablecell.Controls.Add(brctr1)
        Dim img2 As New Image
        img2.ImageUrl = "~/Images/bullet-Magenta.png"
        img2.CssClass = "imageStyle"
        img2.ID = "imgGeneral" + _colName
        img2.Visible = False
        gTablecell.Controls.Add(img2)
        Dim lbl2 As New Label
        lbl2.ID = "lblGeneral" + _colName
        lbl2.CssClass = "LabelDynamic"
        lbl2.Text = ""
        gTablecell.Controls.Add(lbl2)
        gTableRow.Cells.Add(gTablecell)
        gTablecell = New TableCell()
        gTablecell.Style("width") = "45%"
        gTablecell.HorizontalAlign = HorizontalAlign.Left
        Dim img3 As New Image
        img3.ImageUrl = "~/images/bullet-green.ico"
        img3.CssClass = "imageGreenStyle"
        img3.ID = "imgFreeSale" + _colName
        img3.Visible = False
        gTablecell.Controls.Add(img3)
        Dim lbl3 As New Label
        lbl3.ID = "lblFreeSale" + _colName
        lbl3.CssClass = "LabelDynamic"
        lbl3.Visible = False
        lbl3.Text = ""
        gTablecell.Controls.Add(lbl3)
        Dim brctr2 As New HtmlGenericControl
        brctr2.InnerHtml = "<br/>"
        brctr2.ID = "brFreeSale" + _colName
        brctr2.Visible = False
        gTablecell.Controls.Add(brctr2)
        Dim img4 As New Image
        img4.ImageUrl = "~/images/bullet-red.ico"
        img4.CssClass = "imageStyle"
        img4.ID = "imgStopSale" + _colName
        img4.Visible = False
        gTablecell.Controls.Add(img4)
        Dim lbl4 As New Label
        lbl4.ID = "lblStopSale" + _colName
        lbl4.CssClass = "LabelDynamic"
        lbl4.Visible = False
        lbl4.Text = ""
        gTablecell.Controls.Add(lbl4)
        Dim brctr3 As New HtmlGenericControl
        brctr3.InnerHtml = "<br/>"
        brctr3.ID = "brStopSale" + _colName
        brctr3.Visible = False
        gTablecell.Controls.Add(brctr3)
        gTableRow.Cells.Add(gTablecell)
        gTable.Rows.Add(gTableRow)
        gTable.CssClass = "TableDynamic"
        container.Controls.Add(gTable)
    End Sub
    
End Class
