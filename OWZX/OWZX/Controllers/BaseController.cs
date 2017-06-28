using OWZXBusiness;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OWZXManage.Controllers
{
    [OWZXManage.Common.UserAuthorize]
    public class BaseController : Controller
    {
        private  string TempFilePath = OWZXTool.AppSettings.Settings["UploadTempPath"];
        /// <summary>
        /// 默认分页Size
        /// </summary>
        protected int PageSize = 10;

        /// <summary>
        /// 登录IP
        /// </summary>
        protected string OperateIP
        {
            get 
            {
                return string.IsNullOrEmpty(Request.Headers.Get("X-Real-IP")) ? Request.UserHostAddress : Request.Headers["X-Real-IP"];
            }
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected OWZXEntity.Manage.M_Users CurrentUser
        {
            get
            {
                if (Session["ClientManager"] == null)
                {
                    return null;
                }
                else
                {
                    return (OWZXEntity.Manage.M_Users)Session["ClientManager"];
                }
            }
            set { Session["ClientManager"] = value; }
        }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        protected Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();
 
          
        /// <summary>
        /// Excel转table 导入 Michaux 20160531
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DataTable ImportExcelToDataTable(HttpPostedFileBase file,Dictionary<string,ExcelFormatter> formatColumn=null)
        {
            Dictionary<int, PicturesInfo> ImgList;
            return ImportExcelBase(file, out ImgList, formatColumn);
        }

        public DataTable ImportExcelToDataTable(HttpPostedFileBase file, out Dictionary<int, PicturesInfo> ImgList,
            Dictionary<string, ExcelFormatter> formatColumn = null)
        {
            return ImportExcelBase(file, out ImgList, formatColumn);
        }

        public DataTable ImportExcelBase(HttpPostedFileBase file, out Dictionary<int, PicturesInfo> ImgList, Dictionary<string, ExcelFormatter> formatColumn = null)
        {
            var datatable = new DataTable();
            ImgList=new Dictionary<int, PicturesInfo>();
            if (file.FileName.IndexOf("xlsx") > -1)
            {
                NPOI.XSSF.UserModel.XSSFWorkbook Upfile = new NPOI.XSSF.UserModel.XSSFWorkbook(file.InputStream);
                var sheet = Upfile.GetSheetAt(0);
                var firstRow = sheet.GetRow(0);
                var buffer = new byte[file.ContentLength];
                for (int cellIndex = firstRow.FirstCellNum; cellIndex < firstRow.LastCellNum; cellIndex++)
                {
                    datatable.Columns.Add(firstRow.GetCell(cellIndex).StringCellValue, typeof(string));
                }
                bool imgForm = true;
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow datarow = datatable.NewRow();
                    var row = sheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    bool con = true;
                    for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                    {
                        if (formatColumn != null  && formatColumn.Values.Where(x => x.ColumnName == firstRow.GetCell(j).StringCellValue).Any())
                        {
                            ExcelFormatter excelFormatter = formatColumn.Values.Where(x => x.ColumnName == firstRow.GetCell(j).StringCellValue).FirstOrDefault();
                            if (excelFormatter.ColumnTrans.Equals(EnumColumnTrans.ConvertImportImage) )
                            {
                                if (imgForm)
                                {
                                    ImgList = NPOIExtendImg.GetAllPictureInfos(sheet);
                                    imgForm = false;
                                } 
                                datarow[j] = GetImgsUrl(ImgList, i-1);
                                continue; 
                            } 
                        }
                        var cell = row.GetCell(j);
                        if (cell == null)
                        {
                            datarow[j] = "";
                            continue;
                        }
                        switch (cell.CellType)
                        {
                            case CellType.Numeric:
                                datarow[j] = cell.NumericCellValue;
                                break;
                            case CellType.String:
                                datarow[j] = cell.StringCellValue;
                                break;
                            case CellType.Blank:
                                datarow[j] = "";
                                break;
                            case CellType.Formula:
                                switch (row.GetCell(j).CachedFormulaResultType)
                                {
                                    case CellType.String:
                                        string strFORMULA = row.GetCell(j).StringCellValue;
                                        if (strFORMULA != null && strFORMULA.Length > 0)
                                        {
                                            datarow[j] = strFORMULA.ToString();
                                        }
                                        else
                                        {
                                            datarow[j] = null;
                                        }
                                        break;
                                    case CellType.Numeric:
                                        datarow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                        break;
                                    case CellType.Boolean:
                                        datarow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                        break;
                                    case CellType.Error:
                                        datarow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                        break;
                                    default:
                                        datarow[j] = "";
                                        break;
                                }
                                break;
                            default:
                                con = false;
                                break;
                        }
                        if (!con)
                        {
                            break;
                        }
                    }
                    if (con)
                    {
                        datatable.Rows.Add(datarow);
                    }
                }
            }
            else
            {
                NPOI.HSSF.UserModel.HSSFWorkbook Upfile = new NPOI.HSSF.UserModel.HSSFWorkbook(file.InputStream);
                var sheet = Upfile.GetSheetAt(0);
                var firstRow = sheet.GetRow(0);
                var buffer = new byte[file.ContentLength];
                for (int cellIndex = firstRow.FirstCellNum; cellIndex < firstRow.LastCellNum; cellIndex++)
                {
                    datatable.Columns.Add(firstRow.GetCell(cellIndex).StringCellValue, typeof(string));
                }
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow datarow = datatable.NewRow();
                    var row = sheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    bool con = true;
                    for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null)
                        {
                            datarow[j] = "";
                            continue;
                        }
                        switch (cell.CellType)
                        {
                            case CellType.Numeric:
                                datarow[j] = cell.NumericCellValue;
                                break;
                            case CellType.String:
                                datarow[j] = cell.StringCellValue;
                                break;
                            case CellType.Blank:
                                datarow[j] = "";
                                break;
                            case CellType.Formula:
                                switch (row.GetCell(j).CachedFormulaResultType)
                                {
                                    case CellType.String:
                                        string strFORMULA = row.GetCell(j).StringCellValue;
                                        if (strFORMULA != null && strFORMULA.Length > 0)
                                        {
                                            datarow[j] = strFORMULA.ToString();
                                        }
                                        else
                                        {
                                            datarow[j] = null;
                                        }
                                        break;
                                    case CellType.Numeric:
                                        datarow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                        break;
                                    case CellType.Boolean:
                                        datarow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                        break;
                                    case CellType.Error:
                                        datarow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                        break;
                                    default:
                                        datarow[j] = "";
                                        break;
                                }
                                break;
                            default:
                                con = false;
                                break;
                        }
                        if (!con)
                        {
                            break;
                        }
                    }
                    if (con)
                    {
                        datatable.Rows.Add(datarow);
                    }
                }
            }
            #region 清除最后的空行
            for (int i = datatable.Rows.Count - 1; i > 0; i--)
            {
                bool isnull = true;
                for (int j = 0; j < datatable.Columns.Count; j++)
                {
                    if (datatable.Rows[i][j] != null)
                    {
                        if (datatable.Rows[i][j].ToString() != "")
                        {
                            isnull = false;
                            break;
                        }
                    }
                }
                if (isnull)
                {
                    datatable.Rows[i].Delete();
                }
            }
            #endregion
            return datatable;
        }

        public string GetExcelModel(string modelName)
        {
            string path = Server.MapPath("~/") +"modules/excelmodel/"+ modelName+".json";
            string jsonStr = "";
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    jsonStr += line.ToString();
                }
            }
            catch (Exception)
            {
                jsonStr = "{\"Error\":\"未找到模板也\",\"Item\":{}}";
                    throw;
            }
            return jsonStr;
        }
        public string GetJsonValue(JEnumerable<JToken> jToken, string key)
        {
            IEnumerator enumerator = jToken.GetEnumerator();
            while (enumerator.MoveNext())
            {
                JToken jc = (JToken)enumerator.Current;
                if (jc is JObject || ((JProperty)jc).Value is JObject)
                {
                    return GetJsonValue(jc.Children(), key);
                }
                else
                {
                    if (((JProperty)jc).Name == key)
                    {
                        return ((JProperty)jc).Value.ToString();
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 导出模版获取
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="formatColumn"></param>
        /// <param name="key"></param>
        /// <param name="test"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public Dictionary<string, ExcelModel> GetColumnForJson(string modelName, ref Dictionary<string, ExcelFormatter> formatColumn, string key = "", string test = "", string clientID = "")
        {
            Dictionary<string, ExcelModel> dic = new Dictionary<string, ExcelModel>();
            string path = Server.MapPath("~/") + "modules/excelmodel/" + modelName + ".json";
            string jsonStr = "";
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "Item";
                }
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    jsonStr += line.ToString();
                }
                JObject jObject = (JObject) JsonConvert.DeserializeObject(jsonStr);
                Dictionary<string, object> jDic =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(jObject[key].ToString());
                foreach (var keyvalue in jDic)
                {                    
                    JObject jChild = (JObject) JsonConvert.DeserializeObject(keyvalue.Value.ToString());
                    ExcelModel excelModel = GetExcelModel(keyvalue.Key.ToLower(),jChild);
                    if (!excelModel.IsHide || (!string.IsNullOrEmpty(test) && test!="export"))
                    {
                        dic.Add(keyvalue.Key.ToLower(), excelModel);
                        int columnType = Convert.ToInt32(jChild[test + "type"]);
                        if (excelModel.IsFomat && columnType > -1)
                        {
                            string dataList = excelModel.DataSource;
                            string[] distType = dataList.Split('|');
                            string dropSource = "";
                            if (distType.Length > 1)
                            {
                                if (distType[0] == "List")
                                {
                                    dropSource = distType[1];
                                }
                                else
                                {
                                    if (Convert.ToInt32(distType[1]) > 0)
                                    {
                                        dropSource =
                                            CommonBusiness.GetDropList(
                                                (DropSourceList) Enum.Parse(typeof (DropSourceList), distType[1]),clientID);
                                    }
                                }
                            }
                            formatColumn.Add(keyvalue.Key.ToLower(), new ExcelFormatter()
                            {
                                ColumnName = excelModel.Title,
                                ColumnTrans =
                                    (EnumColumnTrans)
                                        Enum.Parse(typeof(EnumColumnTrans), columnType.ToString()),
                                DropSource = dropSource
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                dic.Add("Error", new ExcelModel(){Title = "未找到模版"});
                throw;
            }
            return dic;
        }
        /// <summary>
        /// 导入对比模版获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jChild"></param>
        /// <returns></returns>
        public ExcelModel GetExcelModel(string key,JObject jChild)
        {
            ExcelModel excelModel = new ExcelModel();
            excelModel.Title = jChild["title"].ToString();
            excelModel.ColumnName = key;
            excelModel.IsHide = Convert.ToBoolean(jChild["hide"]);
            excelModel.IsFomat = Convert.ToBoolean(jChild["format"]);
            excelModel.Type = Convert.ToInt32(jChild["exporttype"]);
            excelModel.TestType = Convert.ToInt32(jChild["testexporttype"]);
            excelModel.DataSource = jChild["datasource"].ToString();
            excelModel.DefaultText = jChild["defaulttext"].ToString();
            excelModel.DataType = jChild["datatype"].ToString();
            excelModel.ImportType = Convert.ToInt32(jChild["importtype"]);
            excelModel.ImportColumn = jChild["importcolumn"].ToString();
            return excelModel;
        }

        public Dictionary<string, string> GetColumnForJson(string modelName, string key = "Item")
        {
             Dictionary<string, string> dic = new Dictionary<string, string>();
            string path = Server.MapPath("~/") + "modules/excelmodel/" + modelName + ".json";
            string jsonStr = "";
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "Item";
                }
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    jsonStr += line.ToString();
                }
                JObject jObject = (JObject) JsonConvert.DeserializeObject(jsonStr);
                Dictionary<string, object> jDic =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(jObject[key].ToString());
                foreach (var keyvalue in jDic)
                {
                    JObject jChild = (JObject)JsonConvert.DeserializeObject(keyvalue.Value.ToString());
                    if (!Convert.ToBoolean(jChild["hide"]))
                    {
                        dic.Add(jChild["title"].ToString(), keyvalue.Key.ToLower());
                    }
                }
            }
            catch (Exception)
            {
                dic.Add("Error", "未找到模板");
                throw;
            }
            return dic;
        }

        public string GetImgsUrl(Dictionary<int, PicturesInfo> imgList,int key)
        {
            if (imgList.Any() && imgList.ContainsKey(key))
            {
                DirectoryInfo directory = new DirectoryInfo(HttpContext.Server.MapPath(TempFilePath));
                if (!directory.Exists)
                {
                    directory.Create();
                }
                System.IO.MemoryStream ms = new System.IO.MemoryStream(imgList[key].PictureData);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssms") + new Random().Next(1000, 9999).ToString() +
                                  ".jpg";
                img.Save(HttpContext.Server.MapPath(TempFilePath) + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                return TempFilePath + fileName;
            }
            return "";
        }

        public string GetbaseUrl()
        {
            string port = HttpContext.Request.Url.Port.ToString();
            return HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host +
                         (string.IsNullOrEmpty(port) || port == "80" ? "" : ":" + port);
        }
    }
}
