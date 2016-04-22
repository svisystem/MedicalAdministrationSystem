using DevExpress.Xpf.Printing;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    class DocumentGenerator
    {
        private RichEditDocumentServer reds;
        private DocumentPreviewControl dpc;
        public DocumentGenerator()
        {
            reds = new RichEditDocumentServer();
            dpc = new DocumentPreviewControl();
        }
        protected internal MemoryStream Examination(string companyName,
            string companyZip,
            string companySettlement,
            string companyAddress,
            string doctorName,
            int doctorSealNumber,
            string patientName,
            string motherName,
            DateTime birthDate,
            string TAJ,
            string patientZip,
            string patientSettlement,
            string patientAddress,
            string examination,
            string examinationCode)
        {
            Document doc = reds.Document;
            doc.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
            doc.DefaultCharacterProperties.FontSize = 12;
            doc.Unit = DevExpress.Office.DocumentUnit.Centimeter;
            doc.Sections[0].Margins.Bottom = 2;
            doc.Sections[0].Margins.Top = 2;
            doc.Sections[0].Margins.Left = 2;
            doc.Sections[0].Margins.Right = 2;
            doc.Sections[0].Margins.FooterOffset = 0.8F;

            doc.Unit = DevExpress.Office.DocumentUnit.Point;

            Section firstSection = doc.Sections[0];
            SubDocument subdoc = firstSection.BeginUpdateHeader(HeaderFooterType.Primary);
            DocumentRange textRange = subdoc.AppendText("Vizsgálati Lap");
            CharacterProperties cp1 = subdoc.BeginUpdateCharacters(textRange);
            cp1.Bold = true;
            cp1.Italic = true;
            cp1.FontSize = 18;
            subdoc.EndUpdateCharacters(cp1);
            subdoc.Paragraphs[0].Alignment = ParagraphAlignment.Center;
            subdoc.Paragraphs[0].LineSpacingType = ParagraphLineSpacing.Sesquialteral;
            doc.Sections[0].EndUpdateHeader(subdoc);

            Section section = doc.Sections[0];
            SubDocument subdoc2 = firstSection.BeginUpdateFooter(HeaderFooterType.Primary);
            Table table2 = subdoc2.Tables.Create(subdoc2.Range.Start, 1, 2);
            table2.TableLayout = TableLayoutType.Fixed;
            table2.PreferredWidth = 5000;
            table2.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table2.Borders.InsideVerticalBorder.LineColor = Color.Transparent;
            table2.Borders.Left.LineColor = Color.Transparent;
            table2.Borders.Right.LineColor = Color.Transparent;
            table2.Borders.Bottom.LineColor = Color.Transparent;

            subdoc2.InsertText(table2[0, 0].Range.Start, DateTime.Now.ToString("yyyy. MMMM d.", new CultureInfo("hu-HU")));
            DocumentRange range = subdoc2.InsertText(table2[0, 0].Range.Start, "Dátum: ");
            CharacterProperties cp = subdoc2.BeginUpdateCharacters(range);
            cp.Bold = true;
            subdoc2.Paragraphs[0].SpacingBefore = 6;

            subdoc2.InsertText(table2[0, 1].Range.Start, examinationCode);
            DocumentRange r = subdoc2.InsertText(table2[0, 1].Range.Start, "Azonosító: ");
            CharacterProperties c = subdoc2.BeginUpdateCharacters(r);
            c.Bold = true;
            subdoc2.Paragraphs[1].Alignment = ParagraphAlignment.Right;
            subdoc2.Paragraphs[1].SpacingBefore = 6;
            doc.Sections[0].EndUpdateFooter(subdoc2);

            Table table = doc.Tables.Create(doc.CaretPosition, 2, 2);
            table.TableLayout = TableLayoutType.Fixed;
            table.PreferredWidth = 5000;
            table.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table.Borders.InsideVerticalBorder.LineColor = Color.Transparent;
            table.Borders.Left.LineColor = Color.Transparent;
            table.Borders.Right.LineColor = Color.Transparent;

            doc.InsertText(table[0, 0].Range.Start, "\t" + companyAddress);
            doc.InsertText(table[0, 0].Range.Start, "  Címe: " + companyZip + " " + companySettlement + "\n");
            doc.InsertText(table[0, 0].Range.Start, "  Neve: " + companyName + "\n");
            DocumentRange range1 = doc.InsertText(table[0, 0].Range.Start, "Intézmény\n");
            CharacterProperties cp2 = doc.BeginUpdateCharacters(range1);
            cp2.Bold = true;
            cp2.FontSize = 16;
            doc.EndUpdateCharacters(cp2);
            doc.Paragraphs[0].SpacingBefore = 6;
            doc.Paragraphs[3].LineSpacingType = ParagraphLineSpacing.Sesquialteral;

            doc.InsertText(table[0, 1].Range.Start, "  Pecsétszáma: " + doctorSealNumber);
            doc.InsertText(table[0, 1].Range.Start, "  Neve: " + doctorName + "\n");
            DocumentRange range2 = doc.InsertText(table[0, 1].Range.Start, "Orvos\n");
            CharacterProperties cp3 = doc.BeginUpdateCharacters(range2);
            cp3.Bold = true;
            cp3.FontSize = 16;
            doc.EndUpdateCharacters(cp3);
            doc.Paragraphs[4].SpacingBefore = 6;

            doc.InsertText(table[1, 0].Range.Start, "  Születési ideje: " + birthDate.ToString("yyyy. MMMM d.", new CultureInfo("hu-HU")));
            doc.InsertText(table[1, 0].Range.Start, "  Anyja neve: " + motherName + "\n");
            doc.InsertText(table[1, 0].Range.Start, "  Neve: " + patientName + "\n");
            DocumentRange range3 = doc.InsertText(table[1, 0].Range.Start, "Páciens\n");
            CharacterProperties cp4 = doc.BeginUpdateCharacters(range3);
            cp4.Bold = true;
            cp4.FontSize = 16;
            doc.EndUpdateCharacters(cp4);
            doc.Paragraphs[7].SpacingBefore = 6;
            doc.Paragraphs[10].LineSpacingType = ParagraphLineSpacing.Sesquialteral;

            doc.InsertText(table[1, 1].Range.Start, "\t" + patientAddress);
            doc.InsertText(table[1, 1].Range.Start, "  Lakcíme: " + patientZip + " " + patientSettlement + "\n");
            doc.InsertText(table[1, 1].Range.Start, "  TAJ száma: " + TAJ + "\n");
            DocumentRange range4 = doc.InsertText(table[1, 1].Range.Start, "\n");
            CharacterProperties cp5 = doc.BeginUpdateCharacters(range4);
            cp5.Bold = true;
            cp5.FontSize = 16;
            doc.EndUpdateCharacters(cp5);
            doc.Paragraphs[11].SpacingBefore = 6;

            DocumentRange range5 = doc.AppendText("  Vizsgálat: ");
            CharacterProperties cp6 = doc.BeginUpdateCharacters(range5);
            cp6.Bold = true;
            cp6.FontSize = 16;
            doc.Paragraphs[15].SpacingBefore = 6;
            doc.Paragraphs[15].LineSpacingType = ParagraphLineSpacing.Sesquialteral;

            DocumentRange range6 = doc.AppendText(examination);
            CharacterProperties cp7 = doc.BeginUpdateCharacters(range6);
            cp7.Bold = false;
            cp7.FontSize = 12;

            doc.Paragraphs.Append();
            doc.Paragraphs[16].SpacingBefore = 0;
            doc.Paragraphs[16].LineSpacingType = ParagraphLineSpacing.Single;
            doc.AppendText("          ");

            RangePermissionCollection rpc = doc.BeginUpdateRangePermissions();
            RangePermission rangePermission = new RangePermission(doc.Paragraphs[16].Range);
            rangePermission.UserName = "User1";
            rpc.Add(rangePermission);

            doc.EndUpdateRangePermissions(rpc);

            doc.Protect("admin");

            using (MemoryStream ms = new MemoryStream())
            {
                reds.SaveDocument(ms, DocumentFormat.OpenXml);
                return ms;
            }
        }
        protected internal void PicturePrint(MemoryStream image)
        {
            reds.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reds.Document.Unit = DevExpress.Office.DocumentUnit.Centimeter;

            reds.Document.Sections[0].Paragraphs[0].Alignment = ParagraphAlignment.Center;
            reds.Document.Shapes.InsertPicture(reds.Document.Sections[0].Paragraphs[0].Range.Start, DocumentImageSource.FromStream(new MemoryStream(image.ToArray())));
            reds.Document.Shapes[0].LockAspectRatio = true;
            reds.Document.Shapes[0].HorizontalAlignment = ShapeHorizontalAlignment.Center;
            reds.Document.Shapes[0].VerticalAlignment = ShapeVerticalAlignment.Center;

            float X = (reds.Document.Sections[0].Page.Width - 2) / reds.Document.Shapes[0].Size.Width;
            float Y = (reds.Document.Sections[0].Page.Height - 2) / reds.Document.Shapes[0].Size.Height;

            reds.Document.Shapes[0].ScaleX = reds.Document.Shapes[0].ScaleY = X > Y ? Y : X;

            LegacyPrintableComponentLink link = new LegacyPrintableComponentLink(reds);
            DocumentPreviewControl dpc = new DocumentPreviewControl();
            dpc.DocumentSource = link;
            link.CreateDocument();

            dpc.PrintCommand.Execute(null);
        }

        protected internal void WordPrint(string text)
        {
            reds.RtfText = text;

            LegacyPrintableComponentLink link = new LegacyPrintableComponentLink(reds);
            DocumentPreviewControl dpc = new DocumentPreviewControl();
            dpc.DocumentSource = link;
            link.CreateDocument();

            dpc.PrintCommand.Execute(null);
        }
    }
}
