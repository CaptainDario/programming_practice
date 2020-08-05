using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace rechtsrum_in_Rechthausen_FORMS
{
    public class DrawMap
    {
        //the drawing size of the crossings
        float crossingSize;

        //the max x\y value in this map
        float xMax;
        float yMax;

        float imageSize;

        //the scaling factor to fit the map in the picture
        float scale;


        public DrawMap(float _xMax, float _yMax)
        {
            this.crossingSize = 10f;

            this.xMax = _xMax;
            this.yMax = _yMax;

            this.imageSize = 490;

            this.scale = this.imageSize / Math.Max(this.xMax, this.yMax);
            
        }

        //Draws the WHOLE map
        //including shortestpath, crossings and streets
        public void DrawWholeMap(Dictionary<String, Crossing> _crs, List<Street> _streets, Dictionary<String, int> _crossingNumbers,
                                Tuple<float, List<string>>[] _shortestPaths, PictureBox _mapImage, bool _measurement, String _start = "", String _end = "", bool _shortest = false)
        {

            Bitmap map = new Bitmap(_mapImage.Size.Width, _mapImage.Size.Height);
            Graphics g = Graphics.FromImage(map);

            Size crossingSize = new Size((int)this.crossingSize, (int)this.crossingSize);

            //CROSSINGS
            foreach (KeyValuePair<String, Crossing> item in _crs)
            {
                //Crossing
                float x;
                float y = this.imageSize - (item.Value.y * scale);
                Point pos = new Point((int)(item.Value.x * scale), (int)y);
               

                Rectangle rect = new Rectangle(pos, crossingSize);

                g.DrawEllipse(Pens.White, rect);

            }
            //STREETS
            for (int i = 0; i < _streets.Count; i++)
            {
                float y1 = this.imageSize - (_streets[i].start.y * scale - this.crossingSize / 2);
                float y2 = this.imageSize - (_streets[i].end.y * scale - this.crossingSize / 2);

                Point start = new Point((int)(_streets[i].start.x * scale + this.crossingSize / 2),
                                        (int)(y1));
                Point end = new Point((int)(_streets[i].end.x * scale + this.crossingSize / 2),
                                      (int)(y2));

                g.DrawLine(Pens.Red, start, end);
            }

            //SHORTEST PATH
            if (_shortest)
            {
                if (_measurement)
                {
                    //pen for the shortest path
                    Pen green = new Pen(Color.FromArgb(255, 0, 255, 0), 5);
                    green.Alignment = PenAlignment.Center;
                    for (int i = 0; i < _shortestPaths[_crossingNumbers[_end]].Item2.Count; i++)
                    {
                        Point pos1;
                        Point pos2;
                        //draws the startpoint and endpoint
                        if (i == 0)
                        {
                            Pen start = new Pen(Color.Red, 10);
                            Pen end = new Pen(Color.Green, 10);
                            start.Alignment = PenAlignment.Center;

                            Point startPoint = new Point((int)(_crs[_start].x * this.scale),
                                                        (int)(this.imageSize - (_crs[_start].y * this.scale)));

                            Point endPoint = new Point((int)(_crs[_end].x * this.scale),
                                                        (int)(this.imageSize - (_crs[_end].y * this.scale)));

                            g.DrawEllipse(start, new Rectangle(startPoint, crossingSize));
                            g.DrawEllipse(end, new RectangleF(endPoint, crossingSize));

                            //release the resources from drawing
                            start.Dispose();
                            end.Dispose();
                        }
                        //if the goal isnt reached
                        if (i != _shortestPaths[_crossingNumbers[_end]].Item2.Count - 1)
                        {
                            pos1 = new Point((int)(_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].x * this.scale + this.crossingSize / 2),
                                               (int)(this.imageSize - (_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].y * this.scale - this.crossingSize / 2)));

                            pos2 = new Point((int)(_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i + 1]].x * this.scale + this.crossingSize / 2),
                                               (int)(this.imageSize - (_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i + 1]].y * this.scale - this.crossingSize / 2)));
                        }
                        //the last point from the shortest path was reached        
                        else
                        {
                            pos1 = new Point((int)(_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].x * this.scale + this.crossingSize / 2),
                                               (int)(this.imageSize - (_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].y * this.scale - this.crossingSize / 2)));

                            pos2 = new Point((int)(_crs[_end].x * this.scale + this.crossingSize / 2),
                                            (int)(this.imageSize - (_crs[_end].y * this.scale - this.crossingSize / 2)));
                        }
                        //draws the current shortestpath piece 
                        g.DrawLine(green, pos1, pos2);
                    }

                    //release the resources from drawing
                    green.Dispose();

                }

                if (!_measurement)
                {
                    //DRAW SHORTEST PATH CROSSINGS AS MEASUREMENT
                    //pen for the shortest path
                    Pen green = new Pen(Color.FromArgb(255, 255, 255, 0), 5);
                    Pen cyan = new Pen(Color.Cyan, 5);
                    green.Alignment = PenAlignment.Center;
                    for (int i = 0; i < _shortestPaths[_crossingNumbers[_end]].Item2.Count; i++)
                    {
                        Point pos1;
                        Point pos2;
                        //draws the startpoint and endpoint
                        if (i == 0)
                        {
                            Pen start = new Pen(Color.Red, 10);
                            Pen end = new Pen(Color.Green, 10);
                            start.Alignment = PenAlignment.Center;

                            Point startPoint = new Point((int)(_crs[_start].x * this.scale),
                                                        (int)(this.imageSize - (_crs[_start].y * this.scale)));

                            Point endPoint = new Point((int)(_crs[_end].x * this.scale),
                                                        (int)(this.imageSize - (_crs[_end].y * this.scale)));

                            g.DrawEllipse(start, new Rectangle(startPoint, crossingSize));
                            g.DrawEllipse(end, new RectangleF(endPoint, crossingSize));

                            //release the resources from drawing
                            start.Dispose();
                            end.Dispose();
                        }
                        //if the goal isnt reached
                        if (i != _shortestPaths[_crossingNumbers[_end]].Item2.Count - 1)
                        {
                            pos1 = new Point((int)(_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].x * this.scale + this.crossingSize / 2),
                                               (int)(this.imageSize - (_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].y * this.scale - this.crossingSize / 2)));

                            pos2 = new Point((int)(_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i + 1]].x * this.scale + this.crossingSize / 2),
                                               (int)(this.imageSize - (_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i + 1]].y * this.scale - this.crossingSize / 2)));
                        }
                        //the last point from the shortest path was reached        
                        else
                        {
                            pos1 = new Point((int)(_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].x * this.scale + this.crossingSize / 2),
                                               (int)(this.imageSize - (_crs[_shortestPaths[_crossingNumbers[_end]].Item2[i]].y * this.scale - this.crossingSize / 2)));

                            pos2 = new Point((int)(_crs[_end].x * this.scale + this.crossingSize / 2),
                                            (int)(this.imageSize - (_crs[_end].y * this.scale - this.crossingSize / 2)));
                        }
                        //draws the current shortestpath piece 
                        g.DrawLine(green, pos1, pos2);
                    }

                    //release the resources from drawing
                    green.Dispose();

                }
            }
            
                
            //assign the drwan image to the picture box
            _mapImage.Image = map;

        }


        /// <summary>
        /// Updates the picZoom image to show the portion of the main image
        /// the mouse is currently over.
        /// </summary>
        public void UpdateZoomedImage(MouseEventArgs e, PictureBox picZoom, PictureBox pictureBox1, int zoomFactor)
        {
            // Calculate the width and height of the portion of the image we want
            // to show in the picZoom picturebox. This value changes when the zoom
            // factor is changed.
            int zoomWidth = picZoom.Width / zoomFactor;
            int zoomHeight = picZoom.Height / zoomFactor;

            // Calculate the horizontal and vertical midpoints for the crosshair
            // cursor and correct centering of the new image
            int halfWidth = zoomWidth / 2;
            int halfHeight = zoomHeight / 2;

            // Create a new temporary bitmap to fit inside the picZoom picturebox
            Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);

            // Create a temporary Graphics object to work on the bitmap
            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            // Clear the bitmap with the selected backcolor
            bmGraphics.Clear(Color.Black);

            // Set the interpolation mode
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the portion of the main image onto the bitmap
            // The target rectangle is already known now.
            // Here the mouse position of the cursor on the main image is used to
            // cut out a portion of the main image.
            bmGraphics.DrawImage(pictureBox1.Image,
                                 new Rectangle(0, 0, zoomWidth, zoomHeight),
                                 new Rectangle(e.X - halfWidth, e.Y - halfHeight, zoomWidth, zoomHeight),
                                 GraphicsUnit.Pixel);

            // Draw the bitmap on the picZoom picturebox
            picZoom.Image = tempBitmap;

            // Draw a crosshair on the bitmap to simulate the cursor position
            bmGraphics.DrawLine(Pens.White, halfWidth + 1, halfHeight - 4, halfWidth + 1, halfHeight - 1);
            bmGraphics.DrawLine(Pens.White, halfWidth + 1, halfHeight + 6, halfWidth + 1, halfHeight + 3);
            bmGraphics.DrawLine(Pens.White, halfWidth - 4, halfHeight + 1, halfWidth - 1, halfHeight + 1);
            bmGraphics.DrawLine(Pens.White, halfWidth + 6, halfHeight + 1, halfWidth + 3, halfHeight + 1);

            // Dispose of the Graphics object
            bmGraphics.Dispose();

            // Refresh the picZoom picturebox to reflect the changes
            picZoom.Refresh();
        }
    }
}
