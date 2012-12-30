using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Extensions;
using IVisio=Microsoft.Office.Interop.Visio;
using VA=VisioAutomation;

namespace VisioAutomation.Pages
{
    public static class PageHelper
    {
        public static void Duplicate(
            IVisio.Page src_page,
            IVisio.Page dest_page)
        {
            var app = src_page.Application;
            var doc = src_page.Document;
            short copy_paste_flags = (short)IVisio.VisCutCopyPasteCodes.visCopyPasteNoTranslate;

            // handle the source page
            if (src_page == null)
            {
                throw new System.ArgumentNullException("Source Page is null");
            }

            if (dest_page == null)
            {
                throw new System.ArgumentNullException("Destination Page is null");
            }

            if (dest_page == src_page)
            {
                throw new System.ArgumentNullException("Destination Page cannot be Source Page");
            }


            if (src_page != app.ActivePage)
            {
                throw new System.ArgumentException("Source page must be active page.", "src_page");
            }

            var src_page_shapes = src_page.Shapes;
            int num_src_shapes=src_page_shapes.Count;

            if (num_src_shapes > 0)
            {
                var active_window = app.ActiveWindow;
                active_window.SelectAll();
                var selection = active_window.Selection;
                selection.Copy(copy_paste_flags);
                active_window.DeselectAll();
            }

            var src_pagesheet = src_page.PageSheet;
            var pagecells = VA.Pages.PageCells.GetCells(src_pagesheet);


            // handle the dest page

            // first update all the page cells
            var dest_pagesheet = dest_page.PageSheet;
            var update = new VisioAutomation.ShapeSheet.Update();
            pagecells.Apply(update);
            update.Execute(dest_pagesheet);

            // make sure the new page looks like the old page
            dest_page.Background = src_page.Background;
            
            // then paste any contents from the first page
            if (num_src_shapes>0)
            {
                dest_page.Paste(copy_paste_flags);                
            }
        }

        private static VA.Drawing.Size GetSize(IVisio.Page page)
        {
            if (page == null)
            {
                throw new System.ArgumentNullException("page");
            }

            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_height = query.AddColumn(VA.ShapeSheet.SRCConstants.PageHeight);
            var col_width = query.AddColumn(VA.ShapeSheet.SRCConstants.PageWidth);
            var results = query.GetResults<double>(page.PageSheet);
            double height = results[0, col_height];
            double width = results[0, col_width];
            var s = new VA.Drawing.Size(width, height);
            return s;
        }

        public static void ResizeToFitContents(IVisio.Page page, VA.Drawing.Size bordersize)
        {
            page.ResizeToFitContents();

            if ((bordersize.Width > 0.0) || (bordersize.Height > 0.0))
            {
                var old_size = VA.Pages.PageHelper.GetSize(page);
                var new_size = old_size + bordersize.Multiply(2, 2);

                // Set the page size
                var page_cells = new VA.Pages.PageCells();
                page_cells.PageHeight = new_size.Height;
                page_cells.PageWidth = new_size.Width;
                var pageupdate = new VA.ShapeSheet.Update();
                pageupdate.Execute(page);

                // recenter the contents
                page.CenterDrawing();
            }
        }

        public static short[] DropManyU(
            IVisio.Page page,
            IList<IVisio.Master> masters,
            IEnumerable<VA.Drawing.Point> points)
        {
            if (masters == null)
            {
                throw new System.ArgumentNullException("masters");
            }

            if (masters.Count < 1)
            {
                return new short[0];
            }

            if (points == null)
            {
                throw new System.ArgumentNullException("points");
            }

            // NOTE: DropMany will fail if you pass in zero items to drop
            var masters_obj_array = masters.Cast<object>().ToArray();
            var xy_array = VA.Drawing.Point.ToDoubles(points).ToArray();

            System.Array outids_sa;

            page.DropManyU(masters_obj_array, xy_array, out outids_sa);

            short[] outids = (short[])outids_sa;
            return outids;
        }
    }
}