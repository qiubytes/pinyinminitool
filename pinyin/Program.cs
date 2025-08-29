// See https://aka.ms/new-console-template for more information
using Common;
using NPOI.XSSF.UserModel;
using PinyinHelper;
using System.Data;

Console.WriteLine("Hello, World!");
//数据库：
//MySQLDBHelper helper = new MySQLDBHelper();
//DataTable dtFact = helper.ExecuteQueryDataTable("select *  from  `生产厂商_最终` ");
//foreach (DataRow row in dtFact.Rows)
//{
//    string ID = row["ID"].ToString();
//    string py= PinyinConverter.ConvertToPinyinInitials(row["厂商名称"].ToString()).ToUpper();
//    string strsql = $@" UPDATE `生产厂商_最终` SET `别名`='{py}' where ID='{ID}' ";
//    MySQLDBHelper.ExecuteNonQuery(strsql);
//}
//Excel:
string filePath = @"C:\Users\Lenovo\Downloads\xxxx.xlsx";
using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
{
    var workbook = new XSSFWorkbook(fs);
    var sheet = workbook.GetSheetAt(0); // 假设你要操作的是第一个工作表

    for (int row = 2; row <= sheet.LastRowNum; row++) // 从第三行开始
    {
        var fCell = sheet.GetRow(row).GetCell(2); // 索引从0开始
        var fCell2 = sheet.GetRow(row).GetCell(7); // 索引从0开始
        if (fCell != null)
        {
            //获取第3列的值，转换为拼音首字母，放到第4列
            var gCell = sheet.GetRow(row).CreateCell(3); // 第四列
            gCell.SetCellValue(PinyinConverter.ConvertToPinyinInitials(fCell.ToString()).ToUpper() );
        }
        if (fCell2 != null)
        {
            //获取第8列的值，转换为拼音首字母，放到第9列
            var gCell2 = sheet.GetRow(row).CreateCell(8); // 第九列
            gCell2.SetCellValue(PinyinConverter.ConvertToPinyinInitials(fCell2.ToString()).ToUpper());
        }
    }

    // 保存到新文件
    using (var fsOut = new FileStream("UpdatedFile.xlsx", FileMode.Create, FileAccess.Write))
    {
        workbook.Write(fsOut);
    }
}

Console.WriteLine("处理完毕");
//string chinese = "中华人民共和国abc.())（）";
//string initials = PinyinConverter.ConvertToPinyinInitials(chinese).ToUpper();
//Console.WriteLine(initials); // 输出 "zhrmghn"