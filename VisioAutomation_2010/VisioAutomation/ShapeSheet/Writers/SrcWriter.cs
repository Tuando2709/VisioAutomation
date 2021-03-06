﻿namespace VisioAutomation.ShapeSheet.Writers
{
    public class SrcWriter : WriterBase
    {
        private WriteCache<Src> _formulaRecords;
        private WriteCache<Src> _resultRecords;

        public SrcWriter()
        {
        }

        public void Clear()
        {
            _formulaRecords?.Clear();
            _resultRecords?.Clear();
        }

        public void Commit(Microsoft.Office.Interop.Visio.Shape shape)
        {
            var surface = new SurfaceTarget(shape);
            this.Commit(surface);
        }

        public void Commit(Microsoft.Office.Interop.Visio.Page page)
        {
            var surface = new SurfaceTarget(page);
            this.Commit(surface);
        }

        public void Commit(VisioAutomation.SurfaceTarget surface)
        {
            this.CommitFormulas(surface);
            this.CommitResults(surface);
        }

        public void SetFormula(Src src, CellValueLiteral formula)
        {
            this.__SetFormulaIgnoreNull(src, formula);
        }
        
        private void __SetFormulaIgnoreNull(Src src, CellValueLiteral formula)
        {
            if (this._formulaRecords == null)
            {
                this._formulaRecords = new WriteCache<Src>();
            }

            if (formula.HasValue)
            {
                this._formulaRecords.Add(src, formula.Value);
            }
        }

        private VisioAutomation.ShapeSheet.Streams.StreamArray buildstream_src(WriteCache<Src> wcs)
        {
            var builder = new VisioAutomation.ShapeSheet.Streams.SrcStreamArrayBuilder(wcs.Count);
            builder.AddRange(wcs.EnumCoords());
            return builder.ToStreamArray();
        }

        private void CommitFormulas(SurfaceTarget surface)
        {
            if ((this._formulaRecords == null || this._formulaRecords.Count < 1))
            {
                return;
            }

            var stream = this.buildstream_src(this._formulaRecords);
            var formulas = this._formulaRecords.BuildValues();

            if (stream.Array.Length == 0)
            {
                throw new VisioAutomation.Exceptions.InternalAssertionException();
            }

            var flags = this.ComputeGetFormulaFlags();

            int c = surface.SetFormulas(stream, formulas, (short)flags);
        }

        public void SetResult(Src src, CellValueLiteral result)
        {
            if (this._resultRecords == null)
            {
                this._resultRecords = new WriteCache<Src>();
            }

            this._resultRecords.Add(src, result.Value);
        }

        private void CommitResults(SurfaceTarget surface)
        {
            if (this._resultRecords == null || this._resultRecords.Count < 1)
            {
                return;
            }

            var stream = this.buildstream_src(this._resultRecords);
            var results = this._resultRecords.BuildValues();
            const object[] unitcodes = null;

            if (stream.Array.Length == 0)
            {
                throw new VisioAutomation.Exceptions.InternalAssertionException();
            }

            var flags = this.ComputeGetResultFlags();
            surface.SetResults(stream, unitcodes, results, (short)flags);
        }
    }
}