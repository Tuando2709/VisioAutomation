using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using VisioAutomation.Extensions;

namespace VisioAutomation.Text
{
    public class TextBlockFormatCells : VA.ShapeSheet.CellDataGroup
    {
        public VA.ShapeSheet.CellData<double> BottomMargin { get; set; }
        public VA.ShapeSheet.CellData<double> LeftMargin { get; set; }
        public VA.ShapeSheet.CellData<double> RightMargin { get; set; }
        public VA.ShapeSheet.CellData<double> TopMargin { get; set; }

        public VA.ShapeSheet.CellData<double> DefaultTabStop { get; set; }
        
        public VA.ShapeSheet.CellData<int> TextBkgnd { get; set; }
        public VA.ShapeSheet.CellData<double> TextBkgndTrans { get; set; }
        
        public VA.ShapeSheet.CellData<int> TextDirection { get; set; }
        
        public VA.ShapeSheet.CellData<int> VerticalAlign { get; set; }

        protected override void _Apply(VA.ShapeSheet.CellDataGroup.ApplyFormula func)
        {
            func(VA.ShapeSheet.SRCConstants.BottomMargin, this.BottomMargin.Formula);
            func(VA.ShapeSheet.SRCConstants.LeftMargin, this.LeftMargin.Formula);
            func(VA.ShapeSheet.SRCConstants.RightMargin, this.RightMargin.Formula);
            func(VA.ShapeSheet.SRCConstants.TopMargin, this.TopMargin.Formula);
            func(VA.ShapeSheet.SRCConstants.DefaultTabStop, this.DefaultTabStop.Formula);
            func(VA.ShapeSheet.SRCConstants.TextBkgnd, this.TextBkgnd.Formula);
            func(VA.ShapeSheet.SRCConstants.TextBkgndTrans, this.TextBkgndTrans.Formula);
            func(VA.ShapeSheet.SRCConstants.TextDirection, this.TextDirection.Formula);
            func(VA.ShapeSheet.SRCConstants.VerticalAlign, this.VerticalAlign.Formula);
        }

        internal static IList<TextBlockFormatCells> GetCells(IVisio.Page page, IList<int> shapeids)
        {
            var query = new TextBlockFormatQuery();
            return VA.ShapeSheet.CellDataGroup._GetObjectsFromRows(page, shapeids, query, get_cells_from_row);
        }

        internal static TextBlockFormatCells GetCells(IVisio.Shape shape)
        {
            var query = new TextBlockFormatQuery();
            return VA.ShapeSheet.CellDataGroup._GetObjectFromSingleRow(shape, query, get_cells_from_row);
        }

        private static TextBlockFormatCells get_cells_from_row(TextBlockFormatQuery query, VA.ShapeSheet.Query.QueryDataRow<double> qds)
        {
            var cells = new TextBlockFormatCells();
            cells.BottomMargin = qds.GetItem(query.BottomMargin);
            cells.LeftMargin= qds.GetItem(query.LeftMargin);
            cells.RightMargin = qds.GetItem(query.RightMargin);
            cells.TopMargin = qds.GetItem(query.TopMargin);
            cells.DefaultTabStop = qds.GetItem(query.DefaultTabStop);
            cells.TextBkgnd = qds.GetItem(query.TextBkgnd).ToInt();
            cells.TextBkgndTrans = qds.GetItem(query.TextBkgndTrans);
            cells.TextDirection = qds.GetItem(query.TextDirection).ToInt();
            cells.VerticalAlign = qds.GetItem(query.VerticalAlign).ToInt();
            return cells;
        }

        class TextBlockFormatQuery : VA.ShapeSheet.Query.CellQuery
        {
            public VA.ShapeSheet.Query.CellQueryColumn BottomMargin { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn LeftMargin { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn RightMargin { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn TopMargin { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn DefaultTabStop { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn TextBkgnd { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn TextBkgndTrans { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn TextDirection { get; set; }
            public VA.ShapeSheet.Query.CellQueryColumn VerticalAlign { get; set; }

            public TextBlockFormatQuery() :
                base()
            {
                BottomMargin = this.AddColumn(VA.ShapeSheet.SRCConstants.BottomMargin, "BottomMargin");
                LeftMargin = this.AddColumn(VA.ShapeSheet.SRCConstants.LeftMargin, "LeftMargin");
                RightMargin = this.AddColumn(VA.ShapeSheet.SRCConstants.RightMargin, "RightMargin");
                TopMargin = this.AddColumn(VA.ShapeSheet.SRCConstants.TopMargin, "TopMargin");


                DefaultTabStop = this.AddColumn(VA.ShapeSheet.SRCConstants.DefaultTabStop, "DefaultTabStop");
                TextBkgnd = this.AddColumn(VA.ShapeSheet.SRCConstants.TextBkgnd, "TextBkgnd");
                TextBkgndTrans = this.AddColumn(VA.ShapeSheet.SRCConstants.TextBkgndTrans, "TextBkgndTrans");
                TextDirection = this.AddColumn(VA.ShapeSheet.SRCConstants.TextDirection, "TextDirection");
                VerticalAlign = this.AddColumn(VA.ShapeSheet.SRCConstants.VerticalAlign, "VerticalAlign");
            }
        }

    }
}