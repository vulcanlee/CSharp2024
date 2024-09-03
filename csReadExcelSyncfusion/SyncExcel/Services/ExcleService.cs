using DataModel.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncExcel.Services;

public class ExcleService
{
    public void ReadExcel()
    {
        NextGenerationSportsHealthManagementModel healthManagementModel = new();
        //New instance of ExcelEngine is created 
        //Equivalent to launching Microsoft Excel with no workbooks open
        //Instantiate the spreadsheet creation engine
        using (ExcelEngine excelEngine = new ExcelEngine())
        {
            //Instantiate the Excel application object
            IApplication application = excelEngine.Excel;

            //Assigns default application version
            application.DefaultVersion = ExcelVersion.Xlsx;

            //A existing workbook is opened.             
            using (FileStream sampleFile = new FileStream("sample.xlsx", FileMode.Open))
            {
                IWorkbook workbook = application.Workbooks.Open(sampleFile);

                //Access first worksheet from the workbook.
                IWorksheet worksheetCT身體組成 = workbook.Worksheets["CT身體組成"];
                IWorksheet worksheetBIA身體組成 = workbook.Worksheets["BIA身體組成(初版)"];

                //Access a cell value from Excel
                healthManagementModel.HomePageModel.性別 = worksheetCT身體組成.Range["E2"].Value;
                healthManagementModel.HomePageModel.年齡 = worksheetCT身體組成.Range["D2"].Value;
                healthManagementModel.HomePageModel.身高 = worksheetBIA身體組成.Range["C2"].Value;
                healthManagementModel.HomePageModel.體重 = worksheetBIA身體組成.Range["C3"].Value;
                healthManagementModel.HomePageModel.BMI = worksheetBIA身體組成.Range["C5"].Value;

                ShowInformation(healthManagementModel);
            }
        }

    }
    public void ShowInformation(NextGenerationSportsHealthManagementModel healthManagementModel)
    {
        Console.WriteLine($"性別 {healthManagementModel.HomePageModel.性別}");
        Console.WriteLine($"年齡 {healthManagementModel.HomePageModel.年齡}");
        Console.WriteLine($"身高 {healthManagementModel.HomePageModel.身高}");
        Console.WriteLine($"體重 {healthManagementModel.HomePageModel.體重}");
        Console.WriteLine($"BMI {healthManagementModel.HomePageModel.BMI}");
    }
}
