using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;
using System.Linq;

namespace tstOpenXml
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"E:\_VSProjects\tstOpenXml\testdoc.docx";
            string newValue = "Новый текст в ячейке";
            using (WordprocessingDocument wd = WordprocessingDocument.Open(file, true))
            {
                // Find the first table in the document.
                Table table = wd.MainDocumentPart.Document.Body.Elements<Table>().First();


                //Поиск таблицы по наименованию "table2"
                foreach (var tbl in wd.MainDocumentPart.Document.Body.Elements<Table>())
                {
                    var tblPrs = tbl.Elements<TableProperties>().ElementAt(0);                  
                    if(tblPrs.TableCaption.Val.Value == "table2")
                    {
                        table = tbl;
                        break;
                    }
                }

                var rows = table.Elements<TableRow>().ToList();
                var cloner = (TableRow)rows.Last().Clone();
                rows.Last().Remove();
                for (int i = 1; i < 11; i++)
                {
                    //clone our "reference row"
                    var rowToInsert = (TableRow)cloner.Clone();
                    //get list of cells
                    var listOfCellsInRow = rowToInsert.Descendants<TableCell>().ToList();
                    //just replace every bit of text in cells with row-number for this example
                    foreach (TableCell cl in listOfCellsInRow)
                    {
                        cl.Descendants<Text>().FirstOrDefault().Text = i.ToString();
                    }
                    //add new row to table, after last row in table
                    table.Descendants<TableRow>().Last().InsertAfterSelf(rowToInsert);
                    Console.WriteLine(rowToInsert.InnerText.ToString());
                }
                
                // Find the second row in the table.
                TableRow row = table.Elements<TableRow>().ElementAt(1);

                // Find the third cell in the row.
                TableCell cell = row.Elements<TableCell>().ElementAt(2);

                // Find the first paragraph in the table cell.
                Paragraph p = cell.Elements<Paragraph>().First();

                // Find the first run in the paragraph.
                Run r = p.Elements<Run>().First();

                // Set the text for the run.
                Text t = r.Elements<Text>().First();
                t.Text = newValue;

            }
        }
    }
}

