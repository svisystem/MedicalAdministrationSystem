using DevExpress.Xpf.Printing;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using MedicalAdministrationSystem.Models.Billing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }
        protected internal MemoryStream ExaminationPlanTemplate()
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
            DocumentRange textRange = subdoc.AppendText("Kezelési Terv");
            CharacterProperties cp1 = subdoc.BeginUpdateCharacters(textRange);
            cp1.Bold = true;
            cp1.Italic = true;
            cp1.FontSize = 18;
            subdoc.EndUpdateCharacters(cp1);
            subdoc.Paragraphs[0].Alignment = ParagraphAlignment.Center;
            subdoc.Paragraphs[0].LineSpacingType = ParagraphLineSpacing.Sesquialteral;
            doc.Sections[0].EndUpdateHeader(subdoc);

            doc.Protect("admin");

            using (MemoryStream ms = new MemoryStream())
            {
                reds.SaveDocument(ms, DocumentFormat.OpenXml);
                return ms;
            }
        }
        protected internal MemoryStream ExaminationPlan(MemoryStream doc, string name, string details = null, string price = null)
        {
            reds.LoadDocument(new MemoryStream(doc.ToArray()), DocumentFormat.OpenXml);
            reds.Document.Unit = DevExpress.Office.DocumentUnit.Point;
            Table table = reds.Document.Tables.Create(reds.Document.Range.End,
                details == null ? (price == null ? 1 : 2) : (price == null ? 2 : 3), 1);
            table.TableLayout = TableLayoutType.Fixed;
            table.PreferredWidth = 5000;
            table.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table.Borders.InsideHorizontalBorder.LineStyle = TableBorderLineStyle.None;
            table.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table.Borders.Right.LineStyle = TableBorderLineStyle.None;
            table.Borders.Bottom.LineStyle = TableBorderLineStyle.None;
            table.TableCellSpacing = 2;

            table[details == null ? (price == null ? 0 : 1) : (price == null ? 1 : 2), 0].BottomPadding = 5;
            DocumentRange range1 = reds.Document.InsertText(table[0, 0].Range.Start, name);
            CharacterProperties cp2 = reds.Document.BeginUpdateCharacters(range1);
            cp2.Bold = true;
            cp2.FontSize = 16;
            reds.Document.EndUpdateCharacters(cp2);

            if (details != null)
            {
                Table table2 = reds.Document.Tables.Create(table[1, 0].Range.Start, 1, 1);
                table2.TableLayout = TableLayoutType.Fixed;
                table2.PreferredWidth = 5000;
                table2.PreferredWidthType = WidthType.FiftiethsOfPercent;
                table2.Borders.Left.LineStyle = TableBorderLineStyle.None;
                table2.Borders.Right.LineStyle = TableBorderLineStyle.None;
                table2.Borders.Bottom.LineStyle = TableBorderLineStyle.None;
                table2.Borders.Top.LineStyle = TableBorderLineStyle.None;
                reds.Document.InsertText(table2[0, 0].Range.Start, details);

                ParagraphProperties props = reds.Document.BeginUpdateParagraphs(table2.Rows[0].Cells[0].Range);
                props.Alignment = ParagraphAlignment.Justify;
                reds.Document.EndUpdateParagraphs(props);
            }

            if (price != null)
            {
                DocumentRange range2 = reds.Document.InsertText(table[details == null ? 1 : 2, 0].Range.Start, "Összeg: " + price + " Ft");
                CharacterProperties cp3 = reds.Document.BeginUpdateCharacters(range2);
                cp3.Italic = true;

                ParagraphProperties props = reds.Document.BeginUpdateParagraphs(table[details == null ? 1 : 2, 0].Range);
                props.Alignment = ParagraphAlignment.Right;
                reds.Document.EndUpdateParagraphs(props);
            }

            reds.Document.Protect("admin");

            using (MemoryStream ms = new MemoryStream())
            {
                reds.SaveDocument(ms, DocumentFormat.OpenXml);
                return ms;
            }
        }
        protected internal MemoryStream Template(bool Type,
            string companyName,
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

            SubDocument subdoc = doc.Sections[0].BeginUpdateHeader(HeaderFooterType.Primary);
            DocumentRange textRange;
            if (Type)
                textRange = subdoc.AppendText("Vizsgálati Lap");
            else
                textRange = subdoc.AppendText("Státusz");
            CharacterProperties cp1 = subdoc.BeginUpdateCharacters(textRange);
            cp1.Bold = true;
            cp1.Italic = true;
            cp1.FontSize = 18;
            subdoc.EndUpdateCharacters(cp1);
            subdoc.Paragraphs[0].Alignment = ParagraphAlignment.Center;
            subdoc.Paragraphs[0].LineSpacingType = ParagraphLineSpacing.Sesquialteral;
            doc.Sections[0].EndUpdateHeader(subdoc);

            SubDocument subdoc2 = doc.Sections[0].BeginUpdateFooter(HeaderFooterType.Primary);
            Table table2 = subdoc2.Tables.Create(subdoc2.Range.Start, 1, 2);
            table2.TableLayout = TableLayoutType.Fixed;
            table2.PreferredWidth = 5000;
            table2.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table2.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table2.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table2.Borders.Right.LineStyle = TableBorderLineStyle.None;
            table2.Borders.Bottom.LineStyle = TableBorderLineStyle.None;

            subdoc2.InsertText(table2[0, 0].Range.Start, DateTime.Now.ToString("yyyy. MMMM d.", new CultureInfo("hu-HU")));
            DocumentRange range = subdoc2.InsertText(table2[0, 0].Range.Start, "Dátum: ");
            CharacterProperties cp = subdoc2.BeginUpdateCharacters(range);
            cp.Bold = true;
            subdoc2.Paragraphs[0].SpacingBefore = 3;

            subdoc2.InsertText(table2[0, 1].Range.Start, examinationCode);
            DocumentRange r = subdoc2.InsertText(table2[0, 1].Range.Start, "Azonosító: ");
            CharacterProperties c = subdoc2.BeginUpdateCharacters(r);
            c.Bold = true;
            subdoc2.Paragraphs[1].Alignment = ParagraphAlignment.Right;
            subdoc2.Paragraphs[1].SpacingBefore = 3;
            doc.Sections[0].EndUpdateFooter(subdoc2);

            Table table = doc.Tables.Create(doc.CaretPosition, 2, 2);
            table.TableLayout = TableLayoutType.Fixed;
            table.PreferredWidth = 5000;
            table.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table.Borders.Right.LineStyle = TableBorderLineStyle.None;

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

            if (Type)
            {
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
            }
            doc.Paragraphs.Append();
            doc.Paragraphs[Type ? 16 : 15].SpacingBefore = 0;
            doc.Paragraphs[Type ? 16 : 15].LineSpacingType = ParagraphLineSpacing.Single;
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
        DocumentRange range6;
        DocumentRange range7;
        protected internal MemoryStream Billing(string Code, CreateBillM.CompanyData from, CreateBillM.CompanyData to,
            ObservableCollection<CreateBillM.PrintItem> PrintList, int PriceWithoutVat, int Vat, int PriceWithVat)
        {
            Document doc = reds.Document;
            doc.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
            doc.DefaultCharacterProperties.FontSize = 11;
            doc.Unit = DevExpress.Office.DocumentUnit.Centimeter;
            doc.Sections[0].Margins.Bottom = 2;
            doc.Sections[0].Margins.Top = 2;
            doc.Sections[0].Margins.Left = 2;
            doc.Sections[0].Margins.Right = 2;
            doc.Sections[0].Margins.FooterOffset = 0.8F;

            doc.Unit = DevExpress.Office.DocumentUnit.Point;

            Section firstSection = doc.Sections[0];
            SubDocument subdoc = firstSection.BeginUpdateHeader(HeaderFooterType.Primary);
            DocumentRange textRange = subdoc.AppendText("Számla");
            CharacterProperties cp1 = subdoc.BeginUpdateCharacters(textRange);
            cp1.Bold = true;
            cp1.Italic = true;
            cp1.FontSize = 20;
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
            table2.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table2.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table2.Borders.Right.LineStyle = TableBorderLineStyle.None;
            table2.Borders.Bottom.LineStyle = TableBorderLineStyle.None;

            subdoc2.InsertText(table2[0, 0].Range.Start, DateTime.Now.ToString("yyyy. MMMM d.", new CultureInfo("hu-HU")));
            DocumentRange range = subdoc2.InsertText(table2[0, 0].Range.Start, "Kiállítás dátuma: ");
            CharacterProperties cp = subdoc2.BeginUpdateCharacters(range);
            cp.Bold = true;
            subdoc2.Paragraphs[0].SpacingBefore = 3;

            subdoc2.InsertText(table2[0, 1].Range.Start, Code);
            DocumentRange r = subdoc2.InsertText(table2[0, 1].Range.Start, "Számla azonosító: ");
            CharacterProperties c = subdoc2.BeginUpdateCharacters(r);
            c.Bold = true;
            subdoc2.Paragraphs[1].Alignment = ParagraphAlignment.Right;
            subdoc2.Paragraphs[1].SpacingBefore = 3;
            doc.Sections[0].EndUpdateFooter(subdoc2);

            Table table = doc.Tables.Create(doc.Range.Start, 2, 2);
            table.TableLayout = TableLayoutType.Fixed;
            table.PreferredWidth = 5000;
            table.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table.Borders.Right.LineStyle = TableBorderLineStyle.None;
            table.Borders.Top.LineStyle = TableBorderLineStyle.None;

            DocumentRange range2 = reds.Document.InsertText(table[0, 0].Range.Start, "Számla kiállító adatai");
            CharacterProperties cp2 = reds.Document.BeginUpdateCharacters(range2);
            cp2.FontSize = 16;
            cp2.Bold = true;

            DocumentRange range3 = reds.Document.InsertText(table[0, 1].Range.Start, "Vevő adatai");
            CharacterProperties cp3 = reds.Document.BeginUpdateCharacters(range3);
            cp3.FontSize = 16;
            cp3.Bold = true;

            ParagraphProperties props = reds.Document.BeginUpdateParagraphs(table[0, 0].Range);
            props.SpacingAfter = 2;

            Table left = doc.Tables.Create(table[1, 0].Range.Start, 1, 1);
            left.TableLayout = TableLayoutType.Fixed;
            left.PreferredWidth = 5000;
            left.PreferredWidthType = WidthType.FiftiethsOfPercent;
            left.Borders.Left.LineStyle = TableBorderLineStyle.None;
            left.Borders.Right.LineStyle = TableBorderLineStyle.None;
            left.Borders.Top.LineStyle = TableBorderLineStyle.None;
            left.Borders.Bottom.LineStyle = TableBorderLineStyle.None;

            Table right = doc.Tables.Create(table[1, 1].Range.Start, 1, 1);
            right.TableLayout = TableLayoutType.Fixed;
            right.PreferredWidth = 5000;
            right.PreferredWidthType = WidthType.FiftiethsOfPercent;
            right.Borders.Left.LineStyle = TableBorderLineStyle.None;
            right.Borders.Right.LineStyle = TableBorderLineStyle.None;
            right.Borders.Top.LineStyle = TableBorderLineStyle.None;
            right.Borders.Bottom.LineStyle = TableBorderLineStyle.None;

            bool exist1 = false;
            bool exist2 = false;

            if (from.WebPage != null)
                if (!exist1)
                {
                    range6 = doc.InsertText(left[0, 0].Range.Start, "WEB: " + from.WebPage);
                    exist1 = true;
                }
                else doc.InsertText(left[0, 0].Range.Start, "WEB: " + from.WebPage);
            if (to.WebPage != null)
                if (!exist2)
                {
                    range7 = doc.InsertText(right[0, 0].Range.Start, "WEB: " + to.WebPage);
                    exist2 = true;
                }
                else doc.InsertText(right[0, 0].Range.Start, "WEB: " + to.WebPage);
            if (from.Email != null)
                if (!exist1)
                {
                    range6 = doc.InsertText(left[0, 0].Range.Start, "Email: " + from.Email);
                    exist1 = true;
                }
                else doc.InsertText(left[0, 0].Range.Start, "Email: " + from.Email + "\n");
            if (to.Email != null)
                if (!exist2)
                {
                    range7 = doc.InsertText(right[0, 0].Range.Start, "Email: " + to.Email);
                    exist2 = true;
                }
                else doc.InsertText(right[0, 0].Range.Start, "Email: " + to.Email + "\n");
            if (from.Phone != null)
                if (!exist1)
                {
                    range6 = doc.InsertText(left[0, 0].Range.Start, "Telefon: " + from.Phone);
                    exist1 = true;
                }
                else doc.InsertText(left[0, 0].Range.Start, "Telefon: " + from.Phone + "\n");
            if (to.Phone != null)
                if (!exist2)
                {
                    range7 = doc.InsertText(right[0, 0].Range.Start, "Telefon: " + to.Phone);
                    exist2 = true;
                }
                else doc.InsertText(right[0, 0].Range.Start, "Telefon: " + to.Phone + "\n");

            if (from.InvoiceNumber != null)
                if (!exist1)
                {
                    range6 = doc.InsertText(left[0, 0].Range.Start, "Bankszámlaszám: " + from.InvoiceNumber);
                    exist1 = true;
                }
                else doc.InsertText(left[0, 0].Range.Start, "Bankszámlaszám: " + from.InvoiceNumber + "\n");
            if (to.InvoiceNumber != null)
                if (!exist2)
                {
                    range7 = doc.InsertText(right[0, 0].Range.Start, "Bankszámlaszám: " + to.InvoiceNumber);
                    exist2 = true;
                }
                else doc.InsertText(right[0, 0].Range.Start, "Bankszámlaszám: " + to.InvoiceNumber + "\n");
            if (from.RegistrationNumber != null)
                if (!exist1)
                {
                    range6 = doc.InsertText(left[0, 0].Range.Start, "Cégjegyzékszám: " + from.RegistrationNumber);
                    exist1 = true;
                }
                else doc.InsertText(left[0, 0].Range.Start, "Cégjegyzékszám: " + from.RegistrationNumber + "\n");
            if (to.RegistrationNumber != null)
                if (!exist2)
                {
                    range7 = doc.InsertText(right[0, 0].Range.Start, "Cégjegyzékszám: " + to.RegistrationNumber);
                    exist2 = true;
                }
                else doc.InsertText(right[0, 0].Range.Start, "Cégjegyzékszám: " + to.RegistrationNumber + "\n");
            if (!exist1)
            {
                range6 = doc.InsertText(left[0, 0].Range.Start, "Adószám: " + from.TaxNumber);
                exist1 = true;
            }
            else doc.InsertText(left[0, 0].Range.Start, "Adószám: " + from.TaxNumber + "\n");
            if (to.TaxNumber != null)
                if (!exist2)
                {
                    range7 = doc.InsertText(right[0, 0].Range.Start, "Adószám: " + to.TaxNumber);
                    exist2 = true;
                }
                else doc.InsertText(right[0, 0].Range.Start, "Adószám: " + to.TaxNumber + "\n");


            doc.InsertText(left[0, 0].Range.Start, from.Address + "\n");
            if (!exist2)
            {
                range7 = doc.InsertText(right[0, 0].Range.Start, to.Address);
                exist2 = true;
            }
            else doc.InsertText(right[0, 0].Range.Start, to.Address + "\n");
            doc.InsertText(left[0, 0].Range.Start, from.ZipCode + " " + from.Settlement + "\n");
            doc.InsertText(right[0, 0].Range.Start, to.ZipCode + " " + to.Settlement + "\n");

            DocumentRange range4 = doc.InsertText(left[0, 0].Range.Start, from.Name + "\n");
            ParagraphProperties props2 = reds.Document.BeginUpdateParagraphs(range4);
            props2.SpacingBefore = 5;
            props2.SpacingAfter = 3;
            CharacterProperties cp4 = reds.Document.BeginUpdateCharacters(range4);
            cp4.Bold = true;
            cp4.FontSize = 14;

            DocumentRange range5 = doc.InsertText(right[0, 0].Range.Start, to.Name + "\n");
            ParagraphProperties props3 = reds.Document.BeginUpdateParagraphs(range5);
            props3.SpacingBefore = 5;
            props3.SpacingAfter = 3;
            CharacterProperties cp5 = reds.Document.BeginUpdateCharacters(range5);
            cp5.Bold = true;
            cp5.FontSize = 14;

            ParagraphProperties props4 = reds.Document.BeginUpdateParagraphs(range6);
            ParagraphProperties props5 = reds.Document.BeginUpdateParagraphs(range7);
            props4.SpacingAfter = 5;
            props5.SpacingAfter = 5;

            doc.AppendText("\n\n");

            Table table1 = doc.Tables.Create(doc.Range.End, 1, 7);
            table1.TableLayout = TableLayoutType.Fixed;
            table1.PreferredWidth = 5000;
            table1.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table1.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table1.Borders.Right.LineStyle = TableBorderLineStyle.None;
            table1.Borders.Top.LineStyle = TableBorderLineStyle.None;
            table1.Borders.Bottom.LineStyle = TableBorderLineStyle.None;

            table1[0, 0].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1[0, 0].PreferredWidth = 1250;
            table1[0, 0].VerticalAlignment = TableCellVerticalAlignment.Center;
            table1[0, 1].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1[0, 1].PreferredWidth = 500;
            table1[0, 1].VerticalAlignment = TableCellVerticalAlignment.Center;
            table1[0, 2].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1[0, 2].PreferredWidth = 688;
            table1[0, 2].VerticalAlignment = TableCellVerticalAlignment.Center;
            table1[0, 3].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1[0, 3].PreferredWidth = 688;
            table1[0, 3].VerticalAlignment = TableCellVerticalAlignment.Center;
            table1[0, 4].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1[0, 4].PreferredWidth = 500;
            table1[0, 4].VerticalAlignment = TableCellVerticalAlignment.Center;
            table1[0, 5].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1[0, 5].PreferredWidth = 688;
            table1[0, 5].VerticalAlignment = TableCellVerticalAlignment.Center;
            table1[0, 6].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table1[0, 6].PreferredWidth = 688;
            table1[0, 6].VerticalAlignment = TableCellVerticalAlignment.Center;

            DocumentRange r0 = doc.InsertText(table1[0, 0].Range.Start, "Megnevezés");
            CharacterProperties c0 = reds.Document.BeginUpdateCharacters(r0);
            c0.Bold = true;
            c0.FontSize = 10;
            DocumentRange r1 = doc.InsertText(table1[0, 1].Range.Start, "Menny.");
            CharacterProperties c1 = reds.Document.BeginUpdateCharacters(r1);
            c1.Bold = true;
            c1.FontSize = 10;
            ParagraphProperties p1 = doc.BeginUpdateParagraphs(r1);
            p1.Alignment = ParagraphAlignment.Right;
            DocumentRange r2 = doc.InsertText(table1[0, 2].Range.Start, "Egységár");
            CharacterProperties c2 = reds.Document.BeginUpdateCharacters(r2);
            c2.Bold = true;
            c2.FontSize = 10;
            ParagraphProperties p2 = doc.BeginUpdateParagraphs(r2);
            p2.Alignment = ParagraphAlignment.Right;
            DocumentRange r3 = doc.InsertText(table1[0, 3].Range.Start, "Nettó ár");
            CharacterProperties c3 = reds.Document.BeginUpdateCharacters(r3);
            c3.Bold = true;
            c3.FontSize = 10;
            ParagraphProperties p3 = doc.BeginUpdateParagraphs(r3);
            p3.Alignment = ParagraphAlignment.Right;
            DocumentRange r4 = doc.InsertText(table1[0, 4].Range.Start, "ÁFA");
            CharacterProperties c4 = reds.Document.BeginUpdateCharacters(r4);
            c4.Bold = true;
            c4.FontSize = 10;
            ParagraphProperties p4 = doc.BeginUpdateParagraphs(r4);
            p4.Alignment = ParagraphAlignment.Right;
            DocumentRange r5 = doc.InsertText(table1[0, 5].Range.Start, "Áfa érték");
            CharacterProperties c5 = reds.Document.BeginUpdateCharacters(r5);
            c5.Bold = true;
            c5.FontSize = 10;
            ParagraphProperties p5 = doc.BeginUpdateParagraphs(r5);
            p5.Alignment = ParagraphAlignment.Right;
            DocumentRange r6 = doc.InsertText(table1[0, 6].Range.Start, "Bruttó ár");
            CharacterProperties c6 = reds.Document.BeginUpdateCharacters(r6);
            c6.Bold = true;
            c6.FontSize = 10;
            ParagraphProperties p6 = doc.BeginUpdateParagraphs(r6);
            p6.Alignment = ParagraphAlignment.Right;

            Table table3 = doc.Tables.Create(doc.Range.End, PrintList.Count, 7);
            table3.TableLayout = TableLayoutType.Fixed;
            table3.PreferredWidth = 5000;
            table3.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table3.Borders.InsideHorizontalBorder.LineStyle = TableBorderLineStyle.None;
            table3.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table3.Borders.Right.LineStyle = TableBorderLineStyle.None;

            table3[0, 0].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3[0, 0].PreferredWidth = 1250;
            table3[0, 1].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3[0, 1].PreferredWidth = 500;
            table3[0, 2].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3[0, 2].PreferredWidth = 688;
            table3[0, 3].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3[0, 3].PreferredWidth = 688;
            table3[0, 4].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3[0, 4].PreferredWidth = 500;
            table3[0, 5].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3[0, 5].PreferredWidth = 688;
            table3[0, 6].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table3[0, 6].PreferredWidth = 688;

            List<DocumentRange> dr = new List<DocumentRange>();
            List<ParagraphProperties> ppr = new List<ParagraphProperties>();
            List<CharacterProperties> cpr = new List<CharacterProperties>();

            for (int i = 0; i < PrintList.Count; i++)
            {
                if (i % 2 == 0) for (int column = 0; column < 7; column++) table3[i, column].BackgroundColor = ColorTranslator.FromHtml("#E6E6E6");
                dr.Add(doc.InsertText(table3[i, 0].Range.Start, PrintList[i].Name));
                ppr.Add(doc.BeginUpdateParagraphs(dr[dr.Count - 1]));
                cpr.Add(doc.BeginUpdateCharacters(dr[dr.Count - 1]));
                cpr[cpr.Count - 1].FontSize = 10;
                table3[i, 0].VerticalAlignment = TableCellVerticalAlignment.Center;

                dr.Add(doc.InsertText(table3[i, 1].Range.Start, PrintList[i].Quantity.ToString()));
                ppr.Add(doc.BeginUpdateParagraphs(dr[dr.Count - 1]));
                ppr[ppr.Count - 1].Alignment = ParagraphAlignment.Right;
                cpr.Add(doc.BeginUpdateCharacters(dr[dr.Count - 1]));
                cpr[cpr.Count - 1].FontSize = 10;
                table3[i, 1].VerticalAlignment = TableCellVerticalAlignment.Center;

                dr.Add(doc.InsertText(table3[i, 2].Range.Start, Grouping(PrintList[i].QuantityPrice)));
                ppr.Add(doc.BeginUpdateParagraphs(dr[dr.Count - 1]));
                ppr[ppr.Count - 1].Alignment = ParagraphAlignment.Right;
                cpr.Add(doc.BeginUpdateCharacters(dr[dr.Count - 1]));
                cpr[cpr.Count - 1].FontSize = 10;
                table3[i, 2].VerticalAlignment = TableCellVerticalAlignment.Center;

                dr.Add(doc.InsertText(table3[i, 3].Range.Start, Grouping(PrintList[i].PriceWithoutVat)));
                ppr.Add(doc.BeginUpdateParagraphs(dr[dr.Count - 1]));
                ppr[ppr.Count - 1].Alignment = ParagraphAlignment.Right;
                cpr.Add(doc.BeginUpdateCharacters(dr[dr.Count - 1]));
                cpr[cpr.Count - 1].FontSize = 10;
                table3[i, 3].VerticalAlignment = TableCellVerticalAlignment.Center;

                dr.Add(doc.InsertText(table3[i, 4].Range.Start, PrintList[i].Vat.ToString() + " %"));
                ppr.Add(doc.BeginUpdateParagraphs(dr[dr.Count - 1]));
                ppr[ppr.Count - 1].Alignment = ParagraphAlignment.Right;
                cpr.Add(doc.BeginUpdateCharacters(dr[dr.Count - 1]));
                cpr[cpr.Count - 1].FontSize = 10;
                table3[i, 4].VerticalAlignment = TableCellVerticalAlignment.Center;

                dr.Add(doc.InsertText(table3[i, 5].Range.Start, Grouping(PrintList[i].VatPrice)));
                ppr.Add(doc.BeginUpdateParagraphs(dr[dr.Count - 1]));
                ppr[ppr.Count - 1].Alignment = ParagraphAlignment.Right;
                cpr.Add(doc.BeginUpdateCharacters(dr[dr.Count - 1]));
                cpr[cpr.Count - 1].FontSize = 10;
                table3[i, 5].VerticalAlignment = TableCellVerticalAlignment.Center;

                dr.Add(doc.InsertText(table3[i, 6].Range.Start, Grouping(PrintList[i].PriceWithVat)));
                ppr.Add(doc.BeginUpdateParagraphs(dr[dr.Count - 1]));
                ppr[ppr.Count - 1].Alignment = ParagraphAlignment.Right;
                cpr.Add(doc.BeginUpdateCharacters(dr[dr.Count - 1]));
                cpr[cpr.Count - 1].FontSize = 10;
                table3[i, 6].VerticalAlignment = TableCellVerticalAlignment.Center;

                dr.Clear();
                ppr.Clear();
                cpr.Clear();
            }

            Table table4 = doc.Tables.Create(doc.Range.End, 1, 7);
            table4.TableLayout = TableLayoutType.Fixed;
            table4.PreferredWidth = 5000;
            table4.PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;
            table4.Borders.Left.LineStyle = TableBorderLineStyle.None;
            table4.Borders.Right.LineStyle = TableBorderLineStyle.None;
            table4.Borders.Bottom.LineStyle = TableBorderLineStyle.None;
            table4.Borders.Top.LineStyle = TableBorderLineStyle.None;

            table4[0, 0].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4[0, 0].PreferredWidth = 1250;
            table4[0, 0].VerticalAlignment = TableCellVerticalAlignment.Bottom;
            table4[0, 1].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4[0, 1].PreferredWidth = 500;
            table4[0, 1].VerticalAlignment = TableCellVerticalAlignment.Bottom;
            table4[0, 2].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4[0, 2].PreferredWidth = 688;
            table4[0, 2].VerticalAlignment = TableCellVerticalAlignment.Bottom;
            table4[0, 3].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4[0, 3].PreferredWidth = 688;
            table4[0, 3].VerticalAlignment = TableCellVerticalAlignment.Bottom;
            table4[0, 4].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4[0, 4].PreferredWidth = 500;
            table4[0, 4].VerticalAlignment = TableCellVerticalAlignment.Bottom;
            table4[0, 5].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4[0, 5].PreferredWidth = 688;
            table4[0, 5].VerticalAlignment = TableCellVerticalAlignment.Bottom;
            table4[0, 6].PreferredWidthType = WidthType.FiftiethsOfPercent;
            table4[0, 6].PreferredWidth = 688;
            table4[0, 6].VerticalAlignment = TableCellVerticalAlignment.Bottom;

            DocumentRange rq0 = doc.InsertText(table4[0, 0].Range.Start, "Összesen:");
            CharacterProperties cq0 = reds.Document.BeginUpdateCharacters(rq0);
            cq0.Bold = true;
            cq0.FontSize = 10;
            DocumentRange rq3 = doc.InsertText(table4[0, 3].Range.Start, Grouping(PriceWithoutVat));
            CharacterProperties cq3 = reds.Document.BeginUpdateCharacters(rq3);
            cq3.Bold = true;
            cq3.FontSize = 10;
            ParagraphProperties pq3 = doc.BeginUpdateParagraphs(rq3);
            pq3.Alignment = ParagraphAlignment.Right;
            pq3.SpacingBefore = 4;
            DocumentRange rq5 = doc.InsertText(table4[0, 5].Range.Start, Grouping(Vat));
            CharacterProperties cq5 = reds.Document.BeginUpdateCharacters(rq5);
            cq5.Bold = true;
            cq5.FontSize = 10;
            ParagraphProperties pq5 = doc.BeginUpdateParagraphs(rq5);
            pq5.Alignment = ParagraphAlignment.Right;
            DocumentRange rq6 = doc.InsertText(table4[0, 6].Range.Start, Grouping(PriceWithVat));
            CharacterProperties cq6 = reds.Document.BeginUpdateCharacters(rq6);
            cq6.Bold = true;
            cq6.FontSize = 10;
            ParagraphProperties pq6 = doc.BeginUpdateParagraphs(rq6);
            pq6.Alignment = ParagraphAlignment.Right;

            doc.AppendText("\n\n");

            DocumentRange last = doc.AppendText("Fizetendő: " + Grouping(PriceWithVat) + " Ft");
            CharacterProperties lastcp = doc.BeginUpdateCharacters(last);
            lastcp.Bold = true;
            lastcp.FontSize = 16;
            ParagraphProperties lastpp = doc.BeginUpdateParagraphs(last);
            lastpp.Alignment = ParagraphAlignment.Right;

            using (MemoryStream ms = new MemoryStream())
            {
                reds.ExportToPdf(ms);
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
            dpc = new DocumentPreviewControl();
            dpc.DocumentSource = link;
            link.CreateDocument();

            dpc.PrintCommand.Execute(null);
        }

        protected internal void WordPrint(string text)
        {
            reds.RtfText = text;

            LegacyPrintableComponentLink link = new LegacyPrintableComponentLink(reds);
            dpc = new DocumentPreviewControl();
            dpc.DocumentSource = link;
            link.CreateDocument();

            dpc.PrintCommand.Execute(null);
        }
        private string Grouping(int price)
        {
            string temp = null;
            string priceInString = price.ToString();
            int lenght = priceInString.Length;
            for (int i = 0; i < lenght % 3; i++)
            {
                temp += priceInString[0];
                priceInString = priceInString.Remove(0, 1);
            }
            lenght -= lenght % 3;
            for (int i = 0; i < lenght; i++)
            {
                if (priceInString.Length % 3 == 0 && temp != null) temp += " ";
                temp += priceInString[0];
                priceInString = priceInString.Remove(0, 1);
            }
            return temp;
        }
    }
}
