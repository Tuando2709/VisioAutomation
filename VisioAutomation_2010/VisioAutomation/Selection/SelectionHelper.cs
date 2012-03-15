using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Extensions;
using IVisio=Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;


namespace VisioAutomation.Selection
{
    public static class SelectionHelper
    {
        /// <summary>
        /// Selects a series of shapes and groups them into one shape
        /// </summary>
        /// <param name="window"></param>
        /// <param name="shapes"></param>
        /// <returns></returns>
        public static IVisio.Shape SelectAndGroup(IVisio.Window window, IEnumerable<IVisio.Shape> shapes)
        {
            if (window == null)
            {
                throw new System.ArgumentNullException("window");
            }

            if (shapes == null)
            {
                throw new System.ArgumentNullException("shapes");
            }

            var selectargs = IVisio.VisSelectArgs.visSelect;
            window.Select(shapes, selectargs);
            var selection = window.Selection;
            var group = selection.Group();
            return group;
        }

        public static IList<IVisio.Shape> GetSelectedShapes(IVisio.Selection selection, ShapesEnumeration enumerationtype)
        {
            if (selection.Count < 1)
            {
                return new List<IVisio.Shape>(0);
            }

            var shapes = selection.AsEnumerable();

            if (enumerationtype == ShapesEnumeration.Flat)
            {
                return shapes.ToList();
            }
            
            if (enumerationtype == ShapesEnumeration.ExpandGroups)
            {
                var shapes_in_groups = VA.ShapeHelper.GetNestedShapes(shapes)
                    .Where(s => s.Type != (short) IVisio.VisShapeTypes.visTypeGroup)
                    .ToList();
                return shapes_in_groups;
            }

            throw new System.ArgumentOutOfRangeException("enumerationtype");
        }

        public static void SetGroupSelectMode(IVisio.Shape shape, VA.Selection.GroupSelectMode mode)
        {
            var src_selectmode = VA.ShapeSheet.SRCConstants.SelectMode;
            var select_mode_cell = shape.CellsSRC[src_selectmode.Section, src_selectmode.Row, src_selectmode.Cell];
            select_mode_cell.FormulaU = ((int)mode).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}