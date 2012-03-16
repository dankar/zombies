using System.Diagnostics;
using System;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Windows;


namespace Map_Editor
{
	public class View2D
    {
        #region Private variable declarations
            private string View; // View = XY, ZY or ZX (Front,Left or Top)
		    private SdlDotNet.Windows.SurfaceControl Control;
		    private Surface Surf;
            private Surface GridSurf;
            private Surface BlockSurf;
            private Surface TempSurf;
            private Surface ItemSurf;
            private int GridSize;
            private int surfaceUpdated;
            private object blocks;
            private object items;
            private int scrollLocalX;
            private int scrollLocalY;
            private bool[] layerStatus;
            private int levelSize;
        #endregion
        #region Public variable declarations

            public bool isPanning = false;
            public bool isDrawingBlock = false;
            public bool isMovingBlock = false;
            public bool isMovingItem = false;

            public bool isResizingBlock = false;

            public bool resizePosX = false;
            public bool resizePosY = false;
            public bool resizeSizeX = false;
            public bool resizeSizeY = false;

            public int block_id = 0;
            public int item_id = 0;
            public int last_x = 0;
            public int last_y = 0;
            public int current_x = 0;
            public int current_y = 0;
            public int original_x = 0;
            public int original_y = 0;

        #endregion
        #region Public functions
            public View2D(SdlDotNet.Windows.SurfaceControl Control, string View, object _blocks, object _items, int _gridsize, object _layers)
		{

			this.View = View;
			this.Control = Control;
            this.layerStatus = (bool[])_layers;

			this.Surf = new Surface(this.Control.Width, this.Control.Height);

            this.GridSurf = new Surface(this.Control.Width, this.Control.Height);
            this.BlockSurf = new Surface(this.Control.Width, this.Control.Height);
            this.TempSurf = new Surface(this.Control.Width, this.Control.Height);

            this.GridSize = _gridsize;

            this.blocks = _blocks;
            this.items = _items;

			SdlDotNet.Core.Events.Tick +=  new EventHandler<TickEventArgs>(OnTick);

            this.levelSize = 256;

            this.scrollLocalX = this.levelSize*this.GridSize/2;
            this.scrollLocalY = this.levelSize*this.GridSize/ 2;

            this.UpdateGrid();
            this.UpdateBlocks();
            this.UpdateTemp();

		}
            public void OnTick(object sender, SdlDotNet.Core.TickEventArgs e)
		    {
                
                try
                {
                    if (this.surfaceUpdated>0)
                    {
                        this.Surf.Fill(Color.FromArgb(255, 64, 64, 64));
                        this.DrawGrid();
                        this.DrawBlocks();
                        this.DrawItems();
                        this.DrawTemp();
                        this.Control.Blit(this.Surf);
                        this.surfaceUpdated -=1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to refresh SDL control (" + this.View.ToString() + "): " + ex.Message);
                    this.surfaceUpdated = 1;
                }
		    }
            public void Resize()
            {
                try
                {
                    this.Surf.Dispose();
                    this.Surf = new Surface(this.Control.Width, this.Control.Height);
                    this.QueueRedraw();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to resize surface" + ex.Message);
                }
            }
            public void SetGridSize(int _gridsize)
            {
                try
                {
                    this.GridSize = _gridsize;
                    this.UpdateGrid();
                    this.UpdateBlocks();
                    this.UpdateItems();
                    this.UpdateTemp();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to set grid size :" + ex.Message);
                }
            }
            public void QueueRedraw()
            {
                this.surfaceUpdated = 2;
            }
            public void RedrawTemp()
            {
                
                try
                {
                    this.UpdateTemp();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to update temp surface: " + ex.Message);
                }
                this.QueueRedraw();
            }
            public void RedrawBlocks()
            {
                try
                {
                    this.UpdateBlocks();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to update block surface: " + ex.Message);
                }
                this.QueueRedraw();
            }
            public void RedrawItems()
            {
                try
                {
                    this.UpdateItems();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to update items surface: " + ex.Message);
                }
                this.QueueRedraw();
            }
            public void Scroll(int add_x, int add_y)
            {
                this.scrollLocalX += add_x;
                this.scrollLocalY += add_y;
                this.QueueRedraw();
            }
            public int GetGridX(int window_x)
            {
                int centerX = (this.levelSize * this.GridSize) / 2;
                int localX = this.GetLocalX(-(this.Control.Width / 2)) + window_x;
                double diffX = localX - centerX;
                return (int)Math.Round(diffX/this.GridSize);
            }
            public int GetGridY(int window_y)
            {
                int centerY = (this.levelSize*this.GridSize) / 2;
                int localY = this.GetLocalY(-(this.Control.Height / 2))+window_y;
                double diffY = centerY-localY;
                return (int)Math.Round(diffY / this.GridSize);
            }
        #endregion
        #region Private functions
            private void DrawGrid()
            {
                Rectangle dest = new Rectangle(0,0,this.Control.Width,this.Control.Height);
                Rectangle source = new Rectangle(this.GetLocalX(-(this.Control.Width / 2)), this.GetLocalY(-(this.Control.Height / 2)), this.Control.Width, this.Control.Height);

                if (this.GridSurf != null)
                {
                    this.Surf.Blit(this.GridSurf, dest, source);
                }
            }
            private void DrawBlocks()
            {
                Rectangle dest = new Rectangle(0, 0, this.Control.Width, this.Control.Height);
                Rectangle source = new Rectangle(this.GetLocalX(-(this.Control.Width / 2)), this.GetLocalY(-(this.Control.Height / 2)), this.Control.Width, this.Control.Height);

                if (this.BlockSurf != null)
                {
                    this.Surf.Blit(this.BlockSurf, dest, source);
                }
            }
            private void DrawTemp()
            {
                    Rectangle dest = new Rectangle(0, 0, this.Control.Width, this.Control.Height);
                    Rectangle source = new Rectangle(this.GetLocalX(-(this.Control.Width / 2)), this.GetLocalY(-(this.Control.Height / 2)), this.Control.Width, this.Control.Height);
                    if (this.TempSurf != null)
                    {
                        this.Surf.Blit(this.TempSurf, dest, source);
                    }
            }
            private void DrawItems()
            {
                Rectangle dest = new Rectangle(0, 0, this.Control.Width, this.Control.Height);
                Rectangle source = new Rectangle(this.GetLocalX(-(this.Control.Width / 2)), this.GetLocalY(-(this.Control.Height / 2)), this.Control.Width, this.Control.Height);
                if (this.ItemSurf != null)
                {
                    this.Surf.Blit(this.ItemSurf, dest, source);
                }
            }
            private void UpdateGrid()
            {
                Console.WriteLine("updategrid");
                //this.GridSurf.Dispose();
                this.GridSurf = null;

                this.GridSurf = new Surface(this.levelSize * this.GridSize, this.levelSize * this.GridSize);

                IPrimitive p1 = new SdlDotNet.Graphics.Primitives.Line((short)(this.levelSize * this.GridSize / 2), 0, (short)(this.levelSize * this.GridSize / 2), (short)(this.levelSize * this.GridSize));
                IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(0, (short)(this.levelSize * this.GridSize / 2),  (short)(this.levelSize * this.GridSize), (short)(this.levelSize * this.GridSize / 2));

                this.GridSurf.Draw(p1, Color.FromArgb(255,64,64,64));
                this.GridSurf.Draw(p2, Color.FromArgb(255,64,64,64));

                for (int x = this.GridSize; x < this.levelSize*this.GridSize; x += this.GridSize)
                {
                    for (int y = this.GridSize; y < this.levelSize * this.GridSize; y += this.GridSize)
                    {
                        Point p = new Point(x, y);
                        this.GridSurf.Draw(p, Color.FromArgb(255,96,96,96));
                    }
                }
                this.surfaceUpdated = 1;
            }
            private void UpdateTemp()
            {
                /* The dispose method of Surface didn't release memory */
                this.TempSurf = null;

                this.TempSurf = new Surface(this.levelSize * this.GridSize, this.levelSize * this.GridSize);
                this.TempSurf.Transparent = true;

                /* Add a temporary block if user is currently drawing */
                if (this.isDrawingBlock)
                {
                    IPrimitive p = new SdlDotNet.Graphics.Primitives.Box(
                        (short)((original_x * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                        (short)((-original_y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                        (short)((current_x * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                        (short)((-current_y * this.GridSize) + (this.levelSize * this.GridSize / 2)));
                    this.TempSurf.Draw(p, Color.Orange);
                }

                /* Draw Selected Blocks */
                
                List<Block> classRef;
                classRef = (List<Block>)this.blocks;

                    for (int i = 0; i < classRef.Count(); i++)
                    {
                        if (classRef.ElementAt(i).selected && layerStatus[(int)classRef.ElementAt(i)._layer])
                        {
                            IPrimitive p;
                            p = new SdlDotNet.Graphics.Primitives.Box(0, 0, 0, 0);

                            if (this.View == "XY") // "Front" view, Axis1 = X, Axis2 = Y
                            {
                                p = new SdlDotNet.Graphics.Primitives.Box(
                                    (short)((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (classRef.ElementAt(i).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (-classRef.ElementAt(i).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));
                            }
                            else if (this.View == "ZY") // "Left" view, Axis1 = Z, Axis2 = Y
                            {
                                p = new SdlDotNet.Graphics.Primitives.Box(
                                    (short)((classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((classRef.ElementAt(i).Position.Z * this.GridSize) + (classRef.ElementAt(i).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (-classRef.ElementAt(i).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));
                            }
                            else if (this.View == "XZ") // "Top" view, Axis1 = X, Axis2 = Z
                            {
                                p = new SdlDotNet.Graphics.Primitives.Box(
                                    (short)((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)((-classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (classRef.ElementAt(i).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((-classRef.ElementAt(i).Position.Z * this.GridSize) + (-classRef.ElementAt(i).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)));
                            }
                            this.TempSurf.Draw(p, Color.LightGreen);
                        }
                }
                if (this.isResizingBlock)
                {
                    
                    if (this.View == "XY") // "Front" view, Axis1 = X, Axis2 = Y
                    {
                        #region ResizeXY
                        if (this.resizePosX)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizeSizeX)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizePosY)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizeSizeY)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        #endregion ResizeXY
                    }
                    
                    else if (this.View == "ZY") // "Left" view, Axis1 = Z, Axis2 = Y
                    {
                        #region ResizeZY
                        if (this.resizePosX)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizeSizeX)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)(((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizePosY)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizeSizeY)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Y * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        #endregion ResizeZY
                    }
                    else if (this.View == "XZ") // "Top" view, Axis1 = X, Axis2 = Z
                    {
                        #region ResizeXZ
                        if (this.resizePosX)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizeSizeX)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizePosY)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        else if (this.resizeSizeY)
                        {
                            IPrimitive p2 = new SdlDotNet.Graphics.Primitives.Line(
                                        (short)((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((classRef.ElementAt(this.block_id).Position.X * this.GridSize) + (classRef.ElementAt(this.block_id).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                        (short)(((-classRef.ElementAt(this.block_id).Position.Z * this.GridSize) + (-classRef.ElementAt(this.block_id).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)));

                            this.TempSurf.Draw(p2, Color.Orange);
                        }
                        #endregion ResizeXY
                    }

                }

                /* Mark selected items */
                List<Item> classRef2;
                classRef2 = (List<Item>)this.items;

                for (int i = 0; i < classRef2.Count(); i++)
                {
                    if (classRef2.ElementAt(i).selected && layerStatus[(int)classRef2.ElementAt(i)._layer])
                    {
                        IPrimitive p;
                        p = new SdlDotNet.Graphics.Primitives.Box(0, 0, 0, 0);

                        if (this.View == "XY") // "Front" view, Axis1 = X, Axis2 = Y
                        {
                            p = new SdlDotNet.Graphics.Primitives.Box(
                                (short)(((classRef2.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((-classRef2.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((classRef2.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)),
                                (short)(((-classRef2.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)));
                        }
                        else if (this.View == "ZY") // "Left" view, Axis1 = Z, Axis2 = Y
                        {
                            p = new SdlDotNet.Graphics.Primitives.Box(
                                (short)(((classRef2.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((-classRef2.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((classRef2.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)),
                                (short)(((-classRef2.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)));
                        }
                        else if (this.View == "XZ") // "Top" view, Axis1 = X, Axis2 = Z
                        {
                            p = new SdlDotNet.Graphics.Primitives.Box(
                                (short)(((classRef2.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((-classRef2.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((classRef2.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)),
                                (short)(((-classRef2.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)));
                        }
                        this.TempSurf.Draw(p, Color.LightGreen);
                    }
                    
                }
                this.surfaceUpdated = 1;
            }
            private void UpdateBlocks()
            {
                Console.WriteLine("Updateblocks");
                this.BlockSurf = null;
                //this.BlockSurf.Dispose();
                this.BlockSurf = new Surface(this.levelSize * this.GridSize, this.levelSize * this.GridSize);
                this.BlockSurf.Transparent = true;

                List<Block> classRef;
                classRef = (List<Block>)this.blocks;

                    for (int i = 0; i < classRef.Count(); i++)
                    {
                        IPrimitive p;
                        p = new SdlDotNet.Graphics.Primitives.Box(0,0,0,0);
                        if (layerStatus[(int)classRef.ElementAt(i)._layer])
                        {
                            if (this.View == "XY") // "Front" view, Axis1 = X, Axis2 = Y
                            {
                                p = new SdlDotNet.Graphics.Primitives.Box(
                                    (short)((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (classRef.ElementAt(i).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (-classRef.ElementAt(i).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));
                            }
                            else if (this.View == "ZY") // "Left" view, Axis1 = Z, Axis2 = Y
                            {
                                p = new SdlDotNet.Graphics.Primitives.Box(
                                    (short)((classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((classRef.ElementAt(i).Position.Z * this.GridSize) + (classRef.ElementAt(i).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (-classRef.ElementAt(i).Size.Y * this.GridSize)) + (this.levelSize * this.GridSize / 2)));
                            }
                            else if (this.View == "XZ") // "Top" view, Axis1 = X, Axis2 = Z
                            {
                                p = new SdlDotNet.Graphics.Primitives.Box(
                                    (short)((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)((-classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (classRef.ElementAt(i).Size.X * this.GridSize)) + (this.levelSize * this.GridSize / 2)),
                                    (short)(((-classRef.ElementAt(i).Position.Z * this.GridSize) + (-classRef.ElementAt(i).Size.Z * this.GridSize)) + (this.levelSize * this.GridSize / 2)));
                            }
                            this.BlockSurf.Draw(p, Color.Green);
                        }
                }
                this.surfaceUpdated = 1;
            }
            private void UpdateItems()
            {
                this.ItemSurf = null;
                //this.BlockSurf.Dispose();
                this.ItemSurf = new Surface(this.levelSize * this.GridSize, this.levelSize * this.GridSize);
                this.ItemSurf.Transparent = true;
                List<Item> classRef;
                classRef = (List<Item>)this.items;

                for (int i = 0; i < classRef.Count(); i++)
                {
                    if (layerStatus[(int)classRef.ElementAt(i)._layer])
                    {

                        IPrimitive p;
                        p = new SdlDotNet.Graphics.Primitives.Box(0, 0, 0, 0);

                        if (this.View == "XY") // "Front" view, Axis1 = X, Axis2 = Y
                        {
                            p = new SdlDotNet.Graphics.Primitives.Box(
                                (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)),
                                (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)));
                        }
                        else if (this.View == "ZY") // "Left" view, Axis1 = Z, Axis2 = Y
                        {
                            p = new SdlDotNet.Graphics.Primitives.Box(
                                (short)(((classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)),
                                (short)(((-classRef.ElementAt(i).Position.Y * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)));
                        }
                        else if (this.View == "XZ") // "Top" view, Axis1 = X, Axis2 = Z
                        {
                            p = new SdlDotNet.Graphics.Primitives.Box(
                                (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((-classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) - (this.GridSize / 2)),
                                (short)(((classRef.ElementAt(i).Position.X * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)),
                                (short)(((-classRef.ElementAt(i).Position.Z * this.GridSize) + (this.levelSize * this.GridSize / 2)) + (this.GridSize / 2)));
                        }
                        this.ItemSurf.Draw(p, Color.YellowGreen);
                    }
                }
                this.surfaceUpdated = 1;
            }
            private void GetObjectAt(int x, int y)
            {

            }
            private int GetLocalX(int x)
            {
                return (this.scrollLocalX) + x;
            }
            private int GetLocalY(int y)
            {
                return (this.scrollLocalY) + y;
            }
        #endregion
    }
	
}
