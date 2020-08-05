using System.Drawing;

namespace rechtsrum_in_Rechthausen_FORMS
{
    class Arrow
    {

        private Point top;
        private Point left;
        private Point right;

        private float offset;

        public Arrow(Point _top, float _offset)
        {
            this.top = _top;
            this.offset = _offset;

            this.left = new Point((_top.X - (int)this.offset), (_top.Y - (int)this.offset));
            this.right = new Point((_top.X + (int)this.offset), (_top.Y - (int)this.offset));
        }

        public void DrawArrow()
        {

        }
    }
}
