using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateExcel
{
    public class WriteExcel
    {
        public static void WriteOpenExcel()
        {
            //This creates a spreadsheet with 100 columns and 50000 rows of data. 
            using (SpreadsheetDocument xl = SpreadsheetDocument.Create("C:\\Data\\LargeFile.xlsx", SpreadsheetDocumentType.Workbook))
            {
                List<OpenXmlAttribute> oxa;
                OpenXmlWriter oxw;

                xl.AddWorkbookPart();
                WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

                oxw = OpenXmlWriter.Create(wsp);
                oxw.WriteStartElement(new Worksheet());
                oxw.WriteStartElement(new SheetData());

                for (int i = 1; i <= 50000; ++i)
                {
                    oxa = new List<OpenXmlAttribute>();
                    // this is the row index
                    oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));

                    oxw.WriteStartElement(new Row(), oxa);

                    for (int j = 1; j <= 100; ++j)
                    {
                        oxa = new List<OpenXmlAttribute>();
                        // this is the data type ("t"), with CellValues.String ("str")
                        oxa.Add(new OpenXmlAttribute("t", null, "str"));

                        // it's suggested you also have the cell reference, but
                        // you'll have to calculate the correct cell reference yourself.
                        // Here's an example:
                        //oxa.Add(new OpenXmlAttribute("r", null, "A1"));

                        oxw.WriteStartElement(new Cell(), oxa);

                        oxw.WriteElement(new CellValue(string.Format("R{0}C{1}", i, j)));

                        // this is for Cell
                        oxw.WriteEndElement();
                    }

                    // this is for Row
                    oxw.WriteEndElement();
                }

                // this is for SheetData
                oxw.WriteEndElement();
                // this is for Worksheet
                oxw.WriteEndElement();
                oxw.Close();

                oxw = OpenXmlWriter.Create(xl.WorkbookPart);
                oxw.WriteStartElement(new Workbook());
                oxw.WriteStartElement(new Sheets());

                // you can use object initialisers like this only when the properties
                // are actual properties. SDK classes sometimes have property-like properties
                // but are actually classes. For example, the Cell class has the CellValue
                // "property" but is actually a child class internally.
                // If the properties correspond to actual XML attributes, then you're fine.
                oxw.WriteElement(new Sheet()
                {
                    Name = "Sheet1",
                    SheetId = 1,
                    Id = xl.WorkbookPart.GetIdOfPart(wsp)
                });

                // this is for Sheets
                oxw.WriteEndElement();
                // this is for Workbook
                oxw.WriteEndElement();
                oxw.Close();

                xl.Close();
            }
        }
    }
}
