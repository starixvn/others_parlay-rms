// --------------------------------------------------------------------------
// <copyright file="ExcelDocument.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Core
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ParlayRMS.ExcelService.Core.Interfaces;

    /// <summary>
    /// Excel Document.
    /// </summary>
    public class ExcelDocument
    {
        private readonly IExcelWriter _writer;

        public ExcelDocument()
            : this(null)
        {
        }

        public ExcelDocument(string fileName, IExcelWriter excelWriter)
        {
            FileName = fileName;
            _writer = excelWriter;
        }

        public ExcelDocument(IExcelWriter excelWriter)
        {
            _writer = excelWriter;
        }

        public string FileName { get; set; }

        /// <summary>
        /// Renders the model to text format.
        /// </summary>
        /// <param name="model">Model contains data needs to be rendered.</param>
        /// <returns>Text represents the rendered model.</returns>
        public string Render(object model)
        {
            return _writer.Render(model);
        }

        /// <summary>
        /// Renders Excel data to a Memory Stream.
        /// </summary>
        /// <param name="model">Model contains data needs to be rendered.</param>
        /// <returns>Memory Stream that contains rendered data.</returns>
        public MemoryStream RenderMemoryStream(object model)
        {
            ExcelModel excelModel = ParseModel(model);
            return _writer.RenderMemoryStream(excelModel);
        }

        /// <summary>
        /// Parses an object to ExcelModel object.
        /// </summary>
        /// <param name="p">Source object.</param>
        /// <returns>Parsed ExcelModel object.</returns>
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

        /// <summary>
        /// Creates an Excel row from an object.
        /// </summary>
        /// <param name="item">Object that its properties will be parsed to Excel cells' value.</param>
        /// <param name="counter">Using to identify that the passed item has its associated metadata.</param>
        /// <returns>ExcelRow instance.</returns>
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

        /// <summary>
        /// Gets display name of a property of a Type.
        /// </summary>
        /// <param name="type">Type that using to retrieve property descriptor.</param>
        /// <param name="info">Property Information.</param>
        /// <param name="hasMetaDataAttribute">Has metadata attribute.</param>
        /// <returns>Display name.</returns>
        private static string GetDisplayName(Type type, PropertyInfo info, bool hasMetaDataAttribute)
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