﻿namespace VisioAutomation.Models.DOM
{
    public class Rectangle : BaseShape
    {
        public Drawing.Point P0 { get; private set; }
        public Drawing.Point P1 { get; private set; }

        public Rectangle(double x0, double y0, double x1, double y1)
        {
            this.P0 = new Drawing.Point(x0, y0);
            this.P1 = new Drawing.Point(x1, y1);
        }

        public Rectangle(Drawing.Point p0, Drawing.Point p1)
        {
            this.P0 = p0;
            this.P1 = p1;
        }

        public Rectangle(Drawing.Rectangle r0)
        {
            this.P0 = r0.LowerLeft;
            this.P1 = r0.UpperRight;
        }
    }
}