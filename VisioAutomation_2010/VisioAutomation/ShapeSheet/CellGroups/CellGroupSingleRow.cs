using VisioAutomation.ShapeSheet.Writers;

namespace VisioAutomation.ShapeSheet.CellGroups
{
    public abstract class CellGroupSingleRow : CellGroupBase
    {
        public void SetFormulas(SrcWriter writer)
        {
            foreach (var pair in this.SrcFormulaPairs)
            {
                writer.SetFormula(pair.Src, pair.Formula);
            }
        }

        public void SetFormulas(SidSrcWriter writer, short shapeid)
        {
            foreach (var pair in this.SrcFormulaPairs)
            {
                writer.SetFormula(shapeid, pair.Src, pair.Formula);
            }
        }
    }
}