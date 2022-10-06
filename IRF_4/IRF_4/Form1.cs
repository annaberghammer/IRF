using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace IRF_4
{
    public partial class Form1 : Form
    {
        List<Flat> flats = new List<Flat>();
        RealEstateEntities context = new RealEstateEntities();

        Excel.Application xlApp; // A Microsoft Excel alkalmazás
        Excel.Workbook xlWB; // A létrehozott munkafüzet
        Excel.Worksheet xlSheet; // Munkalap a munkafüzeten belül

        string[] headers = new string[]
        {
            "Kód",
            "Eladó",
            "Oldal",
            "Kerület",
            "Lift",
            "Szobák száma",
            "Alapterület (m2)",
            "Ár (mFt)",
            "Négyzetméter ár (Ft/m2)"
        };

        public Form1()
        {
            InitializeComponent();

            LoadData();
            CreateExcel();
            CreateTable();
            FormatTable();
        }

        void LoadData()
        {
            flats = context.Flats.ToList();
        }

        void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application(); //Excel elindítása és az applikáció objektum betöltése            
                xlWB = xlApp.Workbooks.Add(Missing.Value); //új munkafüzet              
                xlSheet = xlWB.ActiveSheet; //új munkalap

                // Tábla létrehozása
                //CreateTable();

                // Control átadása a felhasználónak
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex) // Hibakezelés a beépített hibaüzenettel
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                // Hiba esetén az Excel applikáció bezárása automatikusan
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }

        void CreateTable()
        {
            

            for (int i = 0; i < headers.Count(); i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];
            }

            object[,] values = new object[flats.Count, headers.Length];

            int counter = 0;
            foreach (var f in flats)
            {
                values[counter, 0] = f.Code;
                values[counter, 1] = f.Vendor;
                values[counter, 2] = f.Side;
                values[counter, 3] = f.District;
                if (f.Elevator == true)
                {
                    values[counter, 4] = "Van";
                }
                else
                {
                    values[counter, 4] = "Nincs";
                }
                values[counter, 5] = f.NumberOfRooms;
                values[counter, 6] = f.FloorArea;
                values[counter, 7] = f.Price;
                values[counter, 8] = "";
                counter++;
            }

            xlSheet.get_Range(
                GetCell(2, 1),
                GetCell(1 + values.GetLength(0), values.GetLength(1)))
                .Value2 = values;

            for (int i = 0; i < flats.Count(); i++)
            {
                xlSheet.Cells[i + 2, 9] = "=" + GetCell(i + 2, 8) + "/" + GetCell(i + 2, 7) + "*1000000";
            }
        }

        void FormatTable()
        {
            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            int lastRowID = xlSheet.UsedRange.Rows.Count;
            Excel.Range tableRange = xlSheet.get_Range(GetCell(2, 1), GetCell(lastRowID, 9));
            tableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            Excel.Range firstColumn = xlSheet.get_Range(GetCell(2, 1), GetCell(lastRowID, 1));

            firstColumn.Font.Bold = true;
            firstColumn.Interior.Color = Color.LightYellow;

            Excel.Range lastColumn = xlSheet.get_Range(GetCell(2, 9), GetCell(lastRowID, 9));
            lastColumn.Interior.Color = Color.LightGreen;
            lastColumn.NumberFormat = "### ### ##0.00";

        }

        string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }
    }
}
