using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Drawing;
using OWZXEntity;
using OWZXEnum;
using NPOI.HSSF.UserModel;
//using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.POIFS.NIO;
using NPOI.SS.Util;

namespace OWZXBusiness
{
   /// <summary>
   /// Excel 导出类
   /// </summary>
    public class ExcelWriter
    {
        public ExcelWriter()
        {
            Maps = new List<ExcelPopertyMap>();
        }

        public List<ExcelPopertyMap> Maps { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        /// <param name="key"> DataTable中的列名 此处不区分大小写</param>
        /// <param name="colName">Excel的列名</param>
        public void Map(string key, string colName)
        {
            string info = key;           
            Maps.Add(new ExcelPopertyMap(info)
            {
                Name = colName
            });
        }
        /// <summary>
        /// 导出Excel List 
        /// </summary> 
        /// <param name="records"></param>    
        /// <returns></returns>
        public byte[] Write(IEnumerable<Dictionary<string, string>> records)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var workBook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            var sheet = workBook.CreateSheet();
            var headerRow = sheet.CreateRow(0);
            var cellIndex = 0;
            foreach (var map in Maps)
            {
                headerRow.CreateCell(cellIndex, CellType.String).SetCellValue(map.Name);
                cellIndex++;
            }
            var rowIndex = 1;
            foreach (var record in records)
            {
                var dr = sheet.CreateRow(rowIndex);
                for (int i = 0; i < Maps.Count; i++)
                {
                    var cellValue = record[Maps[i].Info];
                    dr.CreateCell(i).SetCellValue(cellValue ?? "");
                }
                rowIndex++;
            }
            workBook.Write(ms);
            byte[] buffer = ms.ToArray();
            ms.Close();
            sw.Stop();
            return buffer;
        }
        /// <summary>
        /// 导出Excel 运行时解析 
        /// </summary>
        /// <param name="records">records必须都为字符串</param>   
        /// <returns></returns>
        public byte[] Write(IEnumerable<dynamic> records)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var workBook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            var sheet = workBook.CreateSheet();
            var headerRow = sheet.CreateRow(0);
            var cellIndex = 0;
            foreach (var map in Maps)
            {
                headerRow.CreateCell(cellIndex, CellType.String).SetCellValue(map.Name);
                cellIndex++;
            }
            if (records.Any())
            {
                var rowIndex = 1;
                var first = records.First();
                Type t = first.GetType();
                PropertyInfo[] propertysInfos = t.GetProperties();
                foreach (var record in records)
                {
                    var dr = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < Maps.Count; i++)
                    {
                        var propertysInfo = propertysInfos.Where(x => x.Name == Maps[i].Info).FirstOrDefault();
                        if (propertysInfo == null)
                        {
                            continue;
                        }
                        var cellValue = propertysInfo.GetValue(record);
                        dr.CreateCell(i).SetCellValue(cellValue ?? "");
                    }
                    rowIndex++;
                }
            }
            workBook.Write(ms);
            byte[] buffer = ms.ToArray();
            ms.Close();
            sw.Stop();
            return buffer;
        }

        
        /// <summary>
        /// 导出Excel DataTable
        /// </summary> 
        /// <param name="records">records必须都为DataTable</param>   
        /// <param name="formatter">Dictionary key:DataTable中的列明此处必须小写 value:EnumColumnTrans</param>
        /// <returns></returns>
        public byte[] Write(DataTable records, Dictionary<string, ExcelFormatter> formatter = null,string imgBasepath="")
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var workBook = new XSSFWorkbook(); 
            MemoryStream ms = new MemoryStream();
            var sheet = workBook.CreateSheet();
            var headerRow = sheet.CreateRow(0);
            var cellIndex = 0;
            Color lightGrey = Color.FromArgb(221, 221, 221); 
            ICellStyle cstyle = workBook.CreateCellStyle();
            cstyle.Alignment = HorizontalAlignment.Center; 
            cstyle.IsLocked = true;
           // cstyle.FillForegroundColor = new XSSFColor(lightGrey).Indexed;
            cstyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            foreach (var map in Maps)
            {
                var hcell = headerRow.CreateCell(cellIndex, CellType.String);
                hcell.CellStyle = cstyle;
                hcell.SetCellValue(map.Name); 
                cellIndex++;
            }
            IDataValidationHelper dvHelper = sheet.GetDataValidationHelper();
            var rowIndex = 1;
            IDrawing patriarch = sheet.CreateDrawingPatriarch();
            bool isimg = false;
            foreach (DataRow record in records.Rows)
            {
                var dr = sheet.CreateRow(rowIndex);

                for (int i = 0; i < Maps.Count; i++)
                {
                    string drValue = record[Maps[i].Info.ToString()].ToString();
                    ICell cell= dr.CreateCell(i);
                    if (formatter.Any() && formatter.ContainsKey(Maps[i].Info.ToLower()) && formatter[Maps[i].Info.ToLower()]!=null)
                    {
                        ExcelFormatter excelFormatter = formatter[Maps[i].Info.ToLower()];
                        if (!string.IsNullOrEmpty(drValue))
                        {
                            if (excelFormatter!=null && excelFormatter.ColumnTrans == EnumColumnTrans.ConvertDownList)
                            {
                                cell.SetCellValue(drValue);
                                XSSFDataValidationConstraint dvConstraint = (XSSFDataValidationConstraint)dvHelper.CreateExplicitListConstraint(excelFormatter.DropSource.Split(','));
                                CellRangeAddressList regions = new CellRangeAddressList(1, 65535, i, i);
                                XSSFDataValidation dataValidate = (XSSFDataValidation)dvHelper.CreateValidation(dvConstraint, regions);
                                sheet.AddValidationData(dataValidate);
                            }
                            else if (excelFormatter != null && excelFormatter.ColumnTrans == EnumColumnTrans.ConvertImage)
                            {
                                if (File.Exists(@"" + imgBasepath + drValue))
                                {
                                    if (!isimg)
                                    {
                                        sheet.SetColumnWidth(i, 256*20);
                                        isimg = true;
                                    }
                                    dr.HeightInPoints = 90;
                                    byte[] bytes = System.IO.File.ReadAllBytes(@"" + imgBasepath + drValue);
                                    int pictureIdx = workBook.AddPicture(bytes, XSSFWorkbook.PICTURE_TYPE_PNG);
                                    IClientAnchor anchor = new XSSFClientAnchor(100, 50, 0, 0, i, rowIndex, i + 1,
                                        rowIndex + 1);
                                    IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
                                    pict.Resize(0.3);
                                }
                                else
                                {
                                    cell.SetCellValue("");
                                }
                            }
                            else
                            {
                                cell.SetCellValue(FormatterCoulumn(drValue, excelFormatter.ColumnTrans));
                            }
                        }
                        else { cell.SetCellValue(drValue); }
                    }
                    else
                    {
                        switch (records.Columns[Maps[i].Info].DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                cell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                cell.SetCellValue(dateV);
                                //cell.CellStyle =
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                cell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                cell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                cell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                cell.SetCellValue("");
                                break;
                            default:
                                cell.SetCellValue("");
                                break;
                        }
                    }
                }
                rowIndex++;
            }
            workBook.Write(ms);
            byte[] buffer = ms.ToArray();
            ms.Close();
            sw.Stop();
            return buffer;
        }
        public string FormatterCoulumn(string coulumnValue, EnumColumnTrans eumnType,string clientID="")
        {
            switch (eumnType)
            {
                case EnumColumnTrans.ConvertTime:
                    return Convert.ToDateTime(coulumnValue).ToString("yyyy-MM-dd");
                case EnumColumnTrans.ConvertStatus:
                    return CommonBusiness.GetEnumDesc<EnumClientOrderStatus>((EnumClientOrderStatus)Enum.Parse(typeof(EnumClientOrderStatus), coulumnValue));
                case EnumColumnTrans.ConvertOrderStatus:
                    return CommonBusiness.GetEnumDesc<EnumOrderStatus>((EnumOrderStatus)Enum.Parse(typeof(EnumOrderStatus), coulumnValue));
                case EnumColumnTrans.ConvertSendStatus:
                    return CommonBusiness.GetEnumDesc<EnumSendStatus>((EnumSendStatus)Enum.Parse(typeof(EnumSendStatus), coulumnValue));
                case EnumColumnTrans.ConvertOrderPay:
                    return CommonBusiness.GetEnumDesc<EnumClientOrderPay>((EnumClientOrderPay)Enum.Parse(typeof(EnumClientOrderPay), coulumnValue));
                case EnumColumnTrans.ConvertReturnStatus:
                    return CommonBusiness.GetEnumDesc<EnumReturnStatus>((EnumReturnStatus)Enum.Parse(typeof(EnumReturnStatus), coulumnValue));
                case EnumColumnTrans.ConvertExpressType:
                    return CommonBusiness.GetEnumDesc<EnumExpressType>((EnumExpressType)Enum.Parse(typeof(EnumExpressType), coulumnValue));
                case EnumColumnTrans.ConvertCustomerExtent:
                    return  CommonBusiness.GetEnumDesc<EnumCustomerExtend>((EnumCustomerExtend) Enum.Parse(typeof (EnumCustomerExtend), coulumnValue));
                case EnumColumnTrans.ConvertCustomerType:
                    return coulumnValue == "1" ? "企业" : "个人";
                case EnumColumnTrans.ConvertIsorNo:
                    switch (coulumnValue)
                    {
                        case "1": case "true":return "是";
                        case "0": case "false": return "否";
                        default: return "否";
                    }  
              
                case EnumColumnTrans.ConvertCustomerExtentType:
                    return CommonBusiness.GetEnumindexByDesc<EnumCustomerExtend>(EnumCustomerExtend.Huge, coulumnValue).ToString();
                default:
                    return coulumnValue;
            }
        }

        public int PaserByValueOrName(string dvalue,object propname )
        {   
            try
            {
                return Convert.ToInt32(propname);
                
            }catch
            {
                return Convert.ToInt32(dvalue);
            }
        }

        public T GetProductByDataRow<T>(DataRow dr, Dictionary<string, ExcelModel> listColumn, T entity,Dictionary<string,ExcelFormatter> formatters,string clientID="")
        {
            
            foreach (var property in entity.GetType().GetProperties())
            {
                var propertyname = property.Name.ToLower();
                var defaulvalue = listColumn.FirstOrDefault(q => q.Value.ImportColumn.ToLower() == propertyname); 
                if (default(KeyValuePair<string, ExcelModel>).Equals(defaulvalue))
                {
                    defaulvalue = listColumn.FirstOrDefault(q => q.Value.ColumnName.ToLower() == propertyname);
                }
                if (!default(KeyValuePair<string, ExcelModel>).Equals(defaulvalue))
                {
                    var propname = defaulvalue.Value.Title;
                    var drvalue = dr[propname] != null && dr[propname] != DBNull.Value ? dr[propname].ToString() : "";
                    if (formatters != null && formatters.Count > 0 && formatters.ContainsKey(defaulvalue.Value.ColumnName))
                    {
                        ExcelFormatter excelFormatter = formatters[defaulvalue.Value.ColumnName];
                        ///图片获取必须在Excel转Datatable之前获取到 此处不处理
                        if (!excelFormatter.ColumnTrans.Equals(EnumColumnTrans.ConvertImportImage))
                        {
                            drvalue = FormatterCoulumn(drvalue, excelFormatter.ColumnTrans, clientID);
                        }
                    }
                    switch (defaulvalue.Value.DataType)
                    {
                        case "int":
                            property.SetValue(entity,string.IsNullOrEmpty(drvalue)?-1: drvalue == "是" ? 1 : drvalue == "否" ? 0 :
                            PaserByValueOrName(drvalue, dr[propname]),
                                null);
                            break;
                        case "string":
                            object[] objArray = property.GetCustomAttributes(false);
                            if (objArray.Length > 0)
                            {
                                if ((objArray[0] as Property).Value.ToLower() == "lower")
                                {
                                    property.SetValue(entity,
                                       string.IsNullOrEmpty(drvalue) ? "" : drvalue.ToString().ToLower().Replace("\"", "“"),
                                        null);
                                }
                                else if ((objArray[0] as Property).Value.ToLower() == "upper")
                                {
                                    property.SetValue(entity,
                                       string.IsNullOrEmpty(drvalue) ? "" : drvalue.ToString().ToUpper().Replace("\"", "“"),
                                        null);
                                }
                            }
                            else
                            {
                                property.SetValue(entity,
                                     string.IsNullOrEmpty(drvalue) ? "" : drvalue.ToString().Replace("\"", "“"),
                                    null);
                            }
                            break;
                        case "decimal":
                            property.SetValue(entity,
                               !string.IsNullOrEmpty(drvalue) ? Convert.ToDecimal(drvalue): 0,
                                null);
                            break;
                        case "datetime":
                            property.SetValue(entity,
                                !string.IsNullOrEmpty(drvalue) ? Convert.ToDateTime(drvalue) : DateTime.MinValue,
                                null);
                            break;
                        case "bool":
                            property.SetValue(entity,
                                 !string.IsNullOrEmpty(drvalue) ? Convert.ToBoolean(drvalue)  : false,
                               null);
                            break;
                        default:
                            property.SetValue(entity, drvalue, null);
                            break;
                    }
                }
            }
            return entity; ;
        }

    }
    /// <summary>
    /// Column特殊列格式化方法选择 Status PayStatus OrderStatus CreateTime 等
    /// </summary>
    public enum EnumColumnTrans
    {
        /// <summary>
        /// 转为日期
        /// </summary>
        ConvertTime =0,
        /// <summary>
        /// 状态转为文字
        /// </summary>
        ConvertStatus = 1,
        /// <summary>
        /// 订单状态转换
        /// </summary>
        ConvertOrderStatus = 2,
        /// <summary>
        /// 出库状态
        /// </summary>
        ConvertSendStatus = 3,
        /// <summary>
        /// 付款状态
        /// </summary>
        ConvertOrderPay=4,
        /// <summary>
        /// 退货状态
        /// </summary>
        ConvertReturnStatus = 5,
        /// <summary>
        /// 配送状态
        /// </summary>
        ConvertExpressType=6,
        /// <summary>
        /// 下拉框
        /// </summary>
        ConvertDownList=7,
        /// <summary>
        /// 公司规模
        /// </summary>
        ConvertCustomerExtent = 8,
        /// <summary>
        /// 公司行业
        /// </summary>
        ConvertIndustry = 9,
        /// <summary>
        /// 导出图片
        /// </summary>
        ConvertImage = 10,
        /// <summary>
        /// 格式化单位
        /// </summary>
        ConvertUnitName=11,
        /// <summary>
        /// 格式化单位ID
        /// </summary>
        ConvertUnitID=12,
        /// <summary>
        /// 格式化品牌ID
        /// </summary>
        ConvertBrandID=13,
        /// <summary>
        /// 格式化分类ID
        /// </summary>
        ConvertCategoryID = 14,
        /// <summary>
        /// 客户行业
        /// </summary>
        ConvertClientIndustry = 15, 
        /// <summary>
        /// 客户类型
        /// </summary>
        ConvertCustomerType= 16,
        /// <summary>
        /// 格式化客户行业
        /// </summary>
        ConvertClientIndustryID = 17,
        /// <summary>
        /// 格式化品牌ID
        /// </summary>
        ConvertCustomerExtentType = 18,
        /// <summary>
        /// 导入图片获取
        /// </summary>
        ConvertImportImage = 19,
        /// <summary>
        /// 导出显示是否
        /// </summary>
        ConvertIsorNo = 20

    }

    public enum DropSourceList
    {
        /// <summary>
        /// 公司行业
        /// </summary>
        Industry = 1,
        /// <summary>
        /// 省
        /// </summary>
        Province = 2,
        /// <summary>
        /// 产品类别
        /// </summary>
        ProductCategory = 3,
        /// <summary>
        /// 产品品牌
        /// </summary>
        ProductBrand = 4,
        /// <summary>
        /// 产品单位
        /// </summary>
        ProductUnit = 5,
        /// <summary>
        /// 客户行业
        /// </summary>
        ClientIndustry = 6
    }

    public class ExcelFormatter
    {

        public string ColumnName { get; set; }
        public EnumColumnTrans ColumnTrans { get; set; }
        public string DropSource { get; set; }
    }

    public class ExcelModel
    {
        /// <summary>
        /// Excel列名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 对应数据库列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 导出是否隐藏 对于模版下载此列开关无效
        /// </summary>
        public bool IsHide { get; set; }
        /// <summary>
        /// 是否格式化  为true时 下面Type才生效
        /// </summary>
        public bool IsFomat { get; set; }
        /// <summary>
        /// 导出时格式化类型 参考EnumColumnTrans
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 模版下载时对应格式化类型 参考EnumColumnTrans
        /// </summary>
        public int TestType { get; set; }
        /// <summary>
        /// 导入Excel对应格式化类型
        /// </summary>
        public int ImportType { get; set; }
        /// <summary>
        /// 导入时此列实际指向的列  默认为空
        /// </summary>
        public string ImportColumn { get; set; }
        /// <summary>
        /// 格式化类型 为下拉框时  默认的数据源 类型{"List|数据1,数据2,数据3,..." ,"DropSourceList| 枚举DropSourceList的枚举值"}
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 导入时列的类型  int string boole datetime decimal
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 模版下载 此列默认的文本
        /// </summary>
        public string DefaultText { get; set; }
    }

    public class ExcelPopertyMap
    {
        public readonly string Info;
        public ExcelPopertyMap(string propertyInfo)
        {
            Info = propertyInfo;
        }
        public string Name { get; set; }
    }

    public class CommonUntil {

        public static List<Dictionary<string, string>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, string> rowItem = new Dictionary<string, string>();
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    rowItem.Add(dt.Columns[i].ColumnName, row.ItemArray[i].ToString());
                }
                list.Add(rowItem);
            }
            return list;
        }
    }
    /// <summary>
    /// Excel图片实体类
    /// </summary>
    public class PicturesInfo
{
    public int MinRow { get;set; }
    public int MaxRow { get;set; }
    public int MinCol { get;set; }
    public int MaxCol { get;set; }
    public Byte[] PictureData { get; private set; }
 
    public PicturesInfo(int minRow, int maxRow, int minCol, int maxCol,Byte[] pictureData)
    {
        this.MinRow = minRow;
        this.MaxRow = maxRow;
        this.MinCol = minCol;
        this.MaxCol = maxCol;
        this.PictureData = pictureData;
    }
}
 
    /// <summary>
    /// Excel导入获取图片类
    /// </summary>
    public static class NPOIExtendImg
    {
        public static Dictionary<int, PicturesInfo> GetAllPictureInfos(this ISheet sheet)
        {
            return sheet.GetAllPictureInfos(null,null,null,null);
        }

        public static Dictionary<int, PicturesInfo> GetAllPictureInfos(this ISheet sheet, int? minRow, int? maxRow, int? minCol, int? maxCol, bool onlyInternal = true)
        {
            if (sheet is HSSFSheet)
            {
                return GetAllPictureInfos((HSSFSheet)sheet,minRow,maxRow,minCol,maxCol,onlyInternal);
            }
            else if (sheet is XSSFSheet)
            {
                return GetAllPictureInfos((XSSFSheet)sheet, minRow, maxRow, minCol, maxCol, onlyInternal);
            }
            else
            {
                throw new Exception("未处理类型，没有为该类型添加：GetAllPicturesInfos()扩展方法！");
            }
        }

        private static Dictionary<int, PicturesInfo> GetAllPictureInfos(HSSFSheet sheet, int? minRow, int? maxRow, int? minCol, int? maxCol, bool onlyInternal)
        {
            Dictionary<int, PicturesInfo> picturesInfoList = new Dictionary<int, PicturesInfo>();
 
            var shapeContainer = sheet.DrawingPatriarch as HSSFShapeContainer;
            if (null != shapeContainer)
            {
                var shapeList = shapeContainer.Children;
                foreach (var shape in shapeList)
                {
                    if (shape is HSSFPicture && shape.Anchor is HSSFClientAnchor)
                    {
                        var picture = (HSSFPicture)shape;
                        var anchor = (HSSFClientAnchor)shape.Anchor;

                        if (IsInternalOrIntersect(minRow, maxRow, minCol, maxCol, anchor.Row1, anchor.Row2, anchor.Col1, anchor.Col2, onlyInternal) && !picturesInfoList.ContainsKey((anchor.Row1 - 1)))
                        {
                            picturesInfoList.Add(anchor.Row1-1,new PicturesInfo(anchor.Row1, anchor.Row2, anchor.Col1, anchor.Col2, picture.PictureData.Data));
                        }
                    }
                }
            }
 
            return picturesInfoList;
        }

        private static Dictionary<int, PicturesInfo> GetAllPictureInfos(XSSFSheet sheet, int? minRow, int? maxRow, int? minCol, int? maxCol, bool onlyInternal)
        {
            Dictionary<int, PicturesInfo> picturesInfoList = new Dictionary<int, PicturesInfo>();
 
            var documentPartList = sheet.GetRelations(); 
            foreach (var documentPart in documentPartList)
            {
                if (documentPart is XSSFDrawing)
                {
                    var drawing = (XSSFDrawing)documentPart; 
                    var shapeList = drawing.GetShapes();
                    foreach (var shape in shapeList)
                    {
                        if (shape is XSSFPicture)
                        {
                            var picture = (XSSFPicture)shape;
                            var anchor = picture.GetPreferredSize();

                            if (IsInternalOrIntersect(minRow, maxRow, minCol, maxCol, anchor.Row1, anchor.Row2, anchor.Col1, anchor.Col2, onlyInternal) && !picturesInfoList.ContainsKey((anchor.Row1 - 1)))
                            {
                                picturesInfoList.Add(anchor.Row1-1,new PicturesInfo(anchor.Row1, anchor.Row2, anchor.Col1, anchor.Col2, picture.PictureData.Data));
                            }
                        }
                    }
                }
            }
 
            return picturesInfoList;
        }

        private static bool IsInternalOrIntersect(int? rangeMinRow, int? rangeMaxRow, int? rangeMinCol, int? rangeMaxCol,
            int pictureMinRow, int pictureMaxRow, int pictureMinCol, int pictureMaxCol, bool onlyInternal)
        {
            int _rangeMinRow = rangeMinRow ?? pictureMinRow;
            int _rangeMaxRow = rangeMaxRow ?? pictureMaxRow;
            int _rangeMinCol = rangeMinCol ?? pictureMinCol;
            int _rangeMaxCol = rangeMaxCol ?? pictureMaxCol;
 
            if (onlyInternal)
            {
                return (_rangeMinRow <= pictureMinRow && _rangeMaxRow >= pictureMaxRow &&
                        _rangeMinCol <= pictureMinCol && _rangeMaxCol >= pictureMaxCol);
            }
            else
            {
                return ((Math.Abs(_rangeMaxRow - _rangeMinRow) + Math.Abs(pictureMaxRow - pictureMinRow) >= Math.Abs(_rangeMaxRow + _rangeMinRow - pictureMaxRow - pictureMinRow)) &&
                (Math.Abs(_rangeMaxCol - _rangeMinCol) + Math.Abs(pictureMaxCol - pictureMinCol) >= Math.Abs(_rangeMaxCol + _rangeMinCol - pictureMaxCol - pictureMinCol)));
            }
        }
    }
   
}
