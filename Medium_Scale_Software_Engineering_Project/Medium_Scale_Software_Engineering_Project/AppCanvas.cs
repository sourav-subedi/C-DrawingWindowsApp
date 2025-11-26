using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medium_Scale_Software_Engineering_Project
{
    internal class AppCanvas : ICanvas
    {
        Bitmap AppCanvasBitmap;
        Graphics g;
        private int xPos, yPos,red,blue,green; //pen position when drawing on the canvas
        Pen pen;

        public AppCanvas(int xsize,int ysize) {
            AppCanvasBitmap = new Bitmap(xsize,ysize);
            g = Graphics.FromImage(AppCanvasBitmap);
            Xpos=0; Ypos=0;
            pen = new Pen(Color.Black);
        }
        public int Xpos {
            get => xPos;
            set =>xPos=value; 
        }
        public int Ypos { 
            get => yPos; 
            set => yPos=value; 
        }
        public object PenColour { 
            get => PenColour;
            set => PenColour = value;
        }

        public void Circle(int radius, bool filled)
        {
            g.DrawEllipse(pen,Xpos,Ypos,radius*2,radius*2);
        }

        public void Clear()
        { 
        }

        public void DrawTo(int x, int y)
        {
            x=0; y=0;
        }

        public object getBitmap()
        {
            return AppCanvasBitmap;
        }

        public void MoveTo(int x, int y)
        {
            Xpos = x; Ypos = y;
        }

        public void Rect(int width, int height, bool filled)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
        }

        public void Set(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void SetColour(int red, int green, int blue)
        {
            this.red=red; this.green=green; this.blue=blue;
        }

        public void Tri(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void WriteText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
