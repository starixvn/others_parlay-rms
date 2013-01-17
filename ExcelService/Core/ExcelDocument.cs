using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using ParlayRMS.ExcelService.Core.Interfaces;

namespace ParlayRMS.ExcelService.Core
{
    public class ExcelDocument
    {
        private readonly IExcelWriter _writer;

        public ExcelDocument()
            : this(null)
        {
        }

        public ExcelDocument(String fileName, IExcelWriter excelWriter)
        {
            FileName = fileName;
            _writer = excelWriter;
        }

        public ExcelDocument(IExcelWriter excelWriter)
        {
            _writer = excelWriter;
        }

        public String FileName { get; set; }

        public string Render(object model)
        {
            return _writer.Render(model);
        }

        public MemoryStream RenderMemoryStream(object model)
        {
            ExcelModel excelModel = ParseModel(model);
            return _writer.RenderMemoryStream(excelModel);
        }

        private static ExcelModel ParseModel(object p)
        {
            var model = new ExcelModel();
            if (p is IEnumerable)
            {
                int counter = 0;
                foreach (object item in (IEnumerable)p)
                {
                    model.Add(CreateExcelRow(item, counter));
                    counter++;
                }
            }
            else
            {
                model.Add(CreateExcelRow(p, 0));
            }
            return model;
        }

        private static ExcelRow CreateExcelRow(object item, int counter)
        {
            var row = new ExcelRow();
            Type type = item.GetType();
            bool hasMetaDataAttribute = false;
            object[] attributes = type.GetCustomAttributes(typeof(MetadataTypeAttribute), false);
            if (attributes.Length > 0)
            {
                if (counter == 0)
                {
                    TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(type), type);
                }
                hasMetaDataAttribute = true;
            }
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                string displayName = GetDisplayName(type, info, hasMetaDataAttribute);
                if (displayName != null)
                {
                    PropertyInfo valuePropetry = type.GetProperty(info.Name);
                    object value = valuePropetry.GetValue(item, null);
                    row.Add(displayName, value);
                }
            }
            return row;
        }

        private static String GetDisplayName(Type type, PropertyInfo info, bool hasMetaDataAttribute)
        {
            if (!hasMetaDataAttribute)
            {
                object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    var displayName = (DisplayNameAttribute)attributes[0];
                    return displayName.DisplayName;
                }
                return info.Name;
            }
            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(type).Find(info.Name, true);
            DisplayNameAttribute displayAttribute =
                propDesc.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            return displayAttribute != null ? displayAttribute.DisplayName : null;
        }
    }
}