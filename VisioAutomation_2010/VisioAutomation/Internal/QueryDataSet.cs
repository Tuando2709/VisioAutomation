﻿using System.Collections.Generic;
using System.Linq;
using VA=VisioAutomation;

namespace VisioAutomation.Internal
{
    class QueryDataSet<T>
    {
        readonly int ColumnCount;
        readonly int RowCount;
        private VA.ShapeSheet.Data.TableRowGroupList Groups;
        public VA.ShapeSheet.Data.Table<string> Formulas { get; private set; }
        public VA.ShapeSheet.Data.Table<T> Results { get; private set; }

        internal QueryDataSet(string[] formulas_array, T[] results_array, IList<int> shapeids, int columncount,
                            int rowcount, VA.ShapeSheet.Data.TableRowGroupList groups)
        {
            if (formulas_array == null && results_array == null)
            {
                throw new AutomationException("Both formulas and results cannot be null");
            }

            if (formulas_array != null & results_array != null)
            {
                if (formulas_array.Length != results_array.Length)
                {
                    throw new AutomationException("Formula array and Result array must have the same length");
                }
            }

            if (shapeids.Count != groups.Count)
            {
                throw new AutomationException("The number of shapes must be equal to the number of groups");
            }

            int groupcountsum = groups.Select(g=>g.Count).Sum();
            if (rowcount != groupcountsum)
            {
                throw new AutomationException("The total number of rows must be equal to the sum of the group counts");                
            }

            int totalcells = columncount*rowcount;

            if (formulas_array != null)
            {
                if (totalcells != formulas_array.Length)
                {
                    throw new AutomationException("Invalid number of formulas");
                }                
            }

            if (results_array != null)
            {
                if (totalcells != results_array.Length)
                {
                    throw new AutomationException("Invalid number of formulas");
                }
            }

            this.RowCount = rowcount;
            this.ColumnCount = columncount;
            this.Groups = groups;
            this.Formulas = formulas_array != null ? new VA.ShapeSheet.Data.Table<string>(this.RowCount, this.ColumnCount, this.Groups, formulas_array): null;
            this.Results = results_array != null ? new VA.ShapeSheet.Data.Table<T>(this.RowCount, this.ColumnCount, this.Groups, results_array) : null;
        }

        internal VA.ShapeSheet.Data.Table<VA.ShapeSheet.CellData<T>> CreateMergedTable()
        {
            int n = this.RowCount*this.ColumnCount;
            var array = new VA.ShapeSheet.CellData<T>[n];
            for (int i=0; i<n; i++)
            {
                array[i] = new VA.ShapeSheet.CellData<T>(this.Formulas.RawArray[i], this.Results.RawArray[i]);
            }
            var table = new VA.ShapeSheet.Data.Table<VA.ShapeSheet.CellData<T>>(this.RowCount, this.ColumnCount, this.Groups, array);
            return table;
        }
    }
}