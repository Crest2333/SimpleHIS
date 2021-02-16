using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ABPExample.Application.Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// 将前端传来的Excel文件转为DataTable
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(IFormFile file)
        {

            var ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);

            var workbook = new XSSFWorkbook(ms);
            var sheet = workbook.GetSheetAt(0);
            var startRow = 0;

            var data = new DataTable();

            if (sheet != null)
            {
                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                //将第一行设置为表头
                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell != null)
                    {
                        string cellValue = cell.StringCellValue;
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            var column = new DataColumn(cellValue);
                            data.Columns.Add(column);
                        }
                    }
                }
                startRow = sheet.FirstRowNum + 1;

                //最后一列的标号
                var rowCount = sheet.LastRowNum;

                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　

                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    data.Rows.Add(dataRow);
                }
            }
            ms.Dispose();
            return data;
        }

        public static Stream ToExcel(DataTable dt,string fileName, ExcelType type)
        {
            IWorkbook workbook;
            if (type==ExcelType.XLSX)
                workbook = new XSSFWorkbook();
            else
                workbook = new HSSFWorkbook();
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }

        public enum ExcelType
        {
            XLSX,
            XLS
        }
    }
}
