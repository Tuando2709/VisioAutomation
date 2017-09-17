﻿using System.Collections.Generic;
using VisioAutomation.ShapeSheet.CellGroups;
using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.ShapeSheet;
using VisioAutomation.ShapeSheet.Query;

namespace VisioAutomation.Shapes
{
    public class CustomPropertyCells : CellGroupMultiRow
    {
        public CellValueLiteral Ask { get; set; }
        public CellValueLiteral Calendar { get; set; }
        public CellValueLiteral Format { get; set; }
        public CellValueLiteral Invisible { get; set; }
        public CellValueLiteral Label { get; set; }
        public CellValueLiteral LangID { get; set; }
        public CellValueLiteral Prompt { get; set; }
        public CellValueLiteral SortKey { get; set; }
        public CellValueLiteral Type { get; set; }
        public CellValueLiteral Value { get; set; }

        public CustomPropertyCells()
        {

        }

        public override IEnumerable<SrcValuePair> SrcValuePairs
        {
            get
            {
                yield return SrcValuePair.Create(SrcConstants.CustomPropLabel, this.Label);
                yield return SrcValuePair.Create(SrcConstants.CustomPropValue, this.Value);
                yield return SrcValuePair.Create(SrcConstants.CustomPropFormat, this.Format);
                yield return SrcValuePair.Create(SrcConstants.CustomPropPrompt, this.Prompt);
                yield return SrcValuePair.Create(SrcConstants.CustomPropCalendar, this.Calendar);
                yield return SrcValuePair.Create(SrcConstants.CustomPropLangID, this.LangID);
                yield return SrcValuePair.Create(SrcConstants.CustomPropSortKey, this.SortKey);
                yield return SrcValuePair.Create(SrcConstants.CustomPropInvisible, this.Invisible);
                yield return SrcValuePair.Create(SrcConstants.CustomPropType, this.Type);
                yield return SrcValuePair.Create(SrcConstants.CustomPropAsk, this.Ask);
            }
        }

        public static List<List<CustomPropertyCells>> GetCells(IVisio.Page page, IList<int> shapeids, CellValueType type)
        {
            var query = lazy_query.Value;
            return query.GetCells(page, shapeids, type);
        }

        public static List<CustomPropertyCells> GetCells(IVisio.Shape shape, CellValueType type)
        {
            var query = lazy_query.Value;
            return query.GetCells(shape, type);
        }

        private static readonly System.Lazy<CustomPropertyCellsReader> lazy_query = new System.Lazy<CustomPropertyCellsReader>();


        public class CustomPropertyCellsReader : ReaderMultiRow<CustomPropertyCells>
        {
            public SectionQueryColumn SortKey { get; set; }
            public SectionQueryColumn Ask { get; set; }
            public SectionQueryColumn Calendar { get; set; }
            public SectionQueryColumn Format { get; set; }
            public SectionQueryColumn Invis { get; set; }
            public SectionQueryColumn Label { get; set; }
            public SectionQueryColumn LangID { get; set; }
            public SectionQueryColumn Prompt { get; set; }
            public SectionQueryColumn Value { get; set; }
            public SectionQueryColumn Type { get; set; }

            public CustomPropertyCellsReader()
            {
                var sec = this.query.SectionQueries.Add(IVisio.VisSectionIndices.visSectionProp);


                this.SortKey = sec.Columns.Add(SrcConstants.CustomPropSortKey, nameof(this.SortKey));
                this.Ask = sec.Columns.Add(SrcConstants.CustomPropAsk, nameof(this.Ask));
                this.Calendar = sec.Columns.Add(SrcConstants.CustomPropCalendar, nameof(this.Calendar));
                this.Format = sec.Columns.Add(SrcConstants.CustomPropFormat, nameof(this.Format));
                this.Invis = sec.Columns.Add(SrcConstants.CustomPropInvisible, nameof(this.Invis));
                this.Label = sec.Columns.Add(SrcConstants.CustomPropLabel, nameof(this.Label));
                this.LangID = sec.Columns.Add(SrcConstants.CustomPropLangID, nameof(this.LangID));
                this.Prompt = sec.Columns.Add(SrcConstants.CustomPropPrompt, nameof(this.Prompt));
                this.Type = sec.Columns.Add(SrcConstants.CustomPropType, nameof(this.Type));
                this.Value = sec.Columns.Add(SrcConstants.CustomPropValue, nameof(this.Value));

            }

            public override CustomPropertyCells ToCellGroup(Utilities.ArraySegment<string> row)
            {
                var cells = new CustomPropertyCells();
                cells.Value = row[this.Value];
                cells.Calendar = row[this.Calendar];
                cells.Format = row[this.Format];
                cells.Invisible = row[this.Invis];
                cells.Label = row[this.Label];
                cells.LangID = row[this.LangID];
                cells.Prompt = row[this.Prompt];
                cells.SortKey = row[this.SortKey];
                cells.Type = row[this.Type];
                cells.Ask = row[this.Ask];
                return cells;
            }
        }

        public static CustomPropertyCells Create(CellValueLiteral value, string type)
        {
            var cp_cells = new CustomPropertyCells();
            cp_cells.Value = value;
            cp_cells.Type = type;
            return cp_cells;
        }

        public static CustomPropertyCells Create(string value, CustomPropertyType type)
        {
            var type_int = CustomPropertyTypeToInt(type);
            var cp_cells = new CustomPropertyCells();
            cp_cells.Value = value;
            cp_cells.Type = type_int;
            return cp_cells;
        }

        public static int CustomPropertyTypeToInt(CustomPropertyType type)
        {
            int type_int = -1;
            if (type == CustomPropertyType.String)
            {
                type_int = 0;
            }
            else if (type == CustomPropertyType.FixedList)
            {
                type_int = 1;
            }
            else if (type == CustomPropertyType.Number)
            {
                type_int = 2;
            }
            else if (type == CustomPropertyType.Boolean)
            {
                type_int = 3;
            }
            else if (type == CustomPropertyType.VariableList)
            {
                type_int = 4;
            }
            else if (type == CustomPropertyType.Date)
            {
                type_int = 5;
            }
            else if (type == CustomPropertyType.Duration)
            {
                type_int = 6;
            }
            else if (type == CustomPropertyType.Currency)
            {
                type_int = 7;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException(nameof(type));
            }
            return type_int;
        }


        public static CustomPropertyCells Create(string value)
        {
            return Create(value, CustomPropertyType.String);
        }

        public static CustomPropertyCells Create(int value)
        {
            var cvl = new CellValueLiteral(value);
            return Create(cvl.Value, CustomPropertyType.Number);
        }

        public static CustomPropertyCells Create(double value)
        {
            var cvl = new CellValueLiteral(value);
            return Create(cvl.Value, CustomPropertyType.Number);
        }

        public static CustomPropertyCells Create(float value)
        {
            var cvl = new CellValueLiteral(value);
            return Create(cvl.Value, CustomPropertyType.Number);
        }

        public static CustomPropertyCells Create(bool value)
        {
            return Create(value ? "TRUE" : "FALSE", CustomPropertyType.Boolean);
        }

        public static CustomPropertyCells Create(System.DateTime value)
        {
            var current_culture = System.Globalization.CultureInfo.CurrentCulture;
            string formatted_dt = value.ToString(current_culture);
            string _Value = string.Format("DATETIME(\"{0}\")", formatted_dt);
            return Create(_Value, CustomPropertyType.Date);
        }

        public static CustomPropertyCells Create(CellValueLiteral value)
        {
            return Create(value.Value, CustomPropertyType.String);
        }

        public void EncodeValues()
        {
            // only quote the value when it is a string (no type specified or type equals zero)
            bool noquote = (!(this.Type.Value == null || this.Type.Value == "0"));
            this.Value = CellValueLiteral.EncodeValue(this.Value.Value, noquote);
            this.Label = CellValueLiteral.EncodeValue(this.Label.Value);
            this.Format = CellValueLiteral.EncodeValue(this.Format.Value);
            this.Prompt = CellValueLiteral.EncodeValue(this.Prompt.Value);
        }

    }
}