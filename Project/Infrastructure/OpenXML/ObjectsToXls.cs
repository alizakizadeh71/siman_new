using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace OpenXml
{
    public class ObjectsToXls
    {
        protected XLSWorker xWorker;

        public ObjectsToXls()
        {
            xWorker = new XLSWorker();
        }

        public ObjectsToXls(string saveTo)
        {
            xWorker = XLSWorker.Create(saveTo, true);
        }


        private PropertyDescriptorCollection objectProperties;

        public void AddSheet(IEnumerable<object> objectCollection, string sheetName)
        {
            AddSheet(objectCollection, sheetName, null, null);
        }
        public void AddSheetReport(IEnumerable<List<Dictionary<string, object>>> objectCollection, string sheetName)
        {
            AddSheetReport(objectCollection, sheetName, null, null);
        }

        public void AddSheet(IEnumerable<object> objectCollection, string sheetName, List<string> includeProperties, List<string> resourcesName)
        {
            var objType1 = objectCollection.FirstOrDefault();
            var objType = objectCollection.FirstOrDefault().GetType();
            this.objectProperties = TypeDescriptor.GetProperties(objType);

            if (includeProperties == null)
            {
                includeProperties = new List<string>();
                foreach (PropertyDescriptor p in this.objectProperties)
                {
                    if (ObjectHelpers.NativeTypes.Contains(p.PropertyType))
                    {
                        includeProperties.Add(p.Name);
                    }
                }
            }

            xWorker.AddNewSheet(sheetName);

            xWorker.AddHeaderRow(includeProperties, 1, resourcesName);

            ReadInRecords(objectCollection, includeProperties);
        }


        public List<object> DataTableToJSONWithJavaScriptSerializer(DataTable table, string sheetName)
        {
            var fieldcolumns = Infrastructure.Utility.GetFieldColumnsINfo(table);
            var resourcesName = Infrastructure.Utility.GetresourcesName(table);

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            var result = new List<object>();
            xWorker.AddNewSheet(sheetName);
            xWorker.AddHeaderRow(fieldcolumns, 1, resourcesName);

            var JSONString = new System.Text.StringBuilder();
            var post = new Common.KendoGridPost();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString = new System.Text.StringBuilder();
                    JSONString.Append("{");
                    int counter = 0;
                    var xRow = xWorker.AddNewRow();
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        string tablerow = table.Rows[i][j].ToString();
                        xRow[fieldcolumns[counter]].Value = tablerow;
                        counter++;
                    }
                }
            }

            return result;

            //var strList = jsSerializer.Serialize(parentRow);

            //var ppp = jsSerializer.DeserializeObject(strList);
            //return jsSerializer.DeserializeObject(strList) as List<object>;
        }

        public void AddSheetReport(IEnumerable<List<Dictionary<string, object>>> objectCollection, string sheetName, List<string> includeProperties, List<string> resourcesName)
        {
            var objType1 = objectCollection.FirstOrDefault();
            var objType = objectCollection.FirstOrDefault().GetType();
            this.objectProperties = TypeDescriptor.GetProperties(objType);

            if (includeProperties == null)
            {
                includeProperties = new List<string>();
                foreach (PropertyDescriptor p in this.objectProperties)
                {
                    if (ObjectHelpers.NativeTypes.Contains(p.PropertyType))
                    {
                        includeProperties.Add(p.Name);
                    }
                }
            }

            xWorker.AddNewSheet(sheetName);

            xWorker.AddHeaderRow(includeProperties, 1, resourcesName);

            ReadInRecordsReport(objectCollection, includeProperties);
        }
        private void ReadInRecordsReport(IEnumerable<List<Dictionary<string, object>>> objectCollection, List<string> props)
        {
            foreach (List<Dictionary<string, object>> row in objectCollection)
            {

                var xRow = xWorker.AddNewRow();
                int counter = 0;
                foreach (var objProp in props)
                {
                    try
                    {
                        Dictionary<string, object> objProprow = row[counter] as Dictionary<string, object>;
                        xRow[objProp].Value = objProprow.FirstOrDefault().Value == null ? string.Empty : objProprow.FirstOrDefault().Value;
                        counter++;
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }

                }
            }
        }

        private void ReadInRecords(IEnumerable<object> objectCollection, List<string> props)
        {
            foreach (List<Dictionary<string, object>> row in objectCollection)
            {

                var xRow = xWorker.AddNewRow();
                int counter = 0;
                foreach (var objProp in props)
                {
                    Dictionary<string, object> objProprow = row[counter] as Dictionary<string, object>;
                    xRow[objProp].Value = objProprow.FirstOrDefault().Value == null ? string.Empty : objProprow.FirstOrDefault().Value;
                    counter++;

                }
            }
        }

        public void Save()
        {
            xWorker.Save();
        }

        public void SaveAs(string path)
        {
            xWorker.SaveAs(path);
        }

        public Stream GetFileStream()
        {
            return xWorker.SaveToMemoryStream();
        }

        public void WriteToHttpResponse(string fileName)
        {
            xWorker.WriteToHttpResponse(fileName);
        }
    }
}