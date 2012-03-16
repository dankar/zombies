using System;
using CsGL.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SdlDotNet.Core;
using System.Threading;
using System.Windows.Forms;


namespace Map_Editor
{
    public class View3D : OpenGLControl
    {

        bool isConnected=false;
        private int GridSize;

        private List<Block> blocks;
        private List<Item> items;
        private List<Material> material;

        private float it = 0;

        private bool[] KeyState = new bool[256];	

        public bool mouse_left;
        public float current_x;
        public float current_y;
        public float last_x;
        public float last_y;
        public bool active;

        private float mouse_sens = 0.5f;

        protected float c_pos_x = -80;
        protected float c_pos_y = -40;
        protected float c_pos_z = -64;

        protected float c_ori_x = 27;
        protected float c_ori_y = -49;
        protected float c_ori_z = 5;

        public void Connect(object _blocks, object _items, int _gridsize,object _material)
		{
            this.GridSize = _gridsize;

            this.blocks = (List<Block>)_blocks;
            this.items = (List<Item>)_items;
            this.material = (List<Material>)_material;

            this.isConnected = true;
            SdlDotNet.Core.Events.Tick += new EventHandler<TickEventArgs>(OnTick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
		}
        public void OnTick(object sender, SdlDotNet.Core.TickEventArgs e)
        {
            if (this.KeyState[(int)Keys.W] && this.active)
            {
                CameraTranslate(0.0f, 0.0f, 0.5f);
            }
            if (this.KeyState[(int)Keys.S] && this.active)
            {
                CameraTranslate(0.0f, 0, -0.5f);
            }
            if (this.KeyState[(int)Keys.A] && this.active)
            {
                CameraTranslate(0.5f, 0, 0);
            }
            if (this.KeyState[(int)Keys.D] && this.active)
            {
                CameraTranslate(-0.5f, 0, 0);
            }
            if (this.mouse_left)
            {
                if (this.last_x == 0)
                {
                    this.last_x = this.current_x;
                    this.last_y = this.current_y;
                }
                else
                {
                    this.CameraRotate((float)(this.current_y - this.last_y)*this.mouse_sens,(float)(this.current_x - this.last_x)*this.mouse_sens);
                    this.last_x = this.current_x;
                    this.last_y = this.current_y;
                }
            }

            this.InvokeRefresh();

            if (!this.active)
            {
                Thread.Sleep(10);
            }
            else
            {
                Thread.Sleep(2);
            }
            

        }
        public void InvokeRefresh() {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(InvokeRefresh));
            }
            else
            {
                this.Refresh();
            }
            Thread.Sleep(0);
        }
        public override void glDraw()
        {
            if(this.isConnected){

                GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
                GL.glMatrixMode(GL.GL_MODELVIEW); 
                GL.glLoadIdentity();              
                GL.glRotatef(c_ori_x, 1.0f, 0, 0);					
                GL.glRotatef(c_ori_y, 0, 1.0f, 0);					

                float[] lightPosition = new float[] { 0.0f, 50.0f, 0.0f, 1.00f };
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, lightPosition);

                GL.glTranslatef(c_pos_x, c_pos_y, c_pos_z);			

                foreach (Block b in this.blocks)
                {
                    glDrawBlock(b.Position.X, b.Position.Y, b.Position.Z, b.Size.X, b.Size.Y, b.Size.Z,b.selected,b.Material);
                    Thread.Sleep(0);
                }
                
                it+=1F;

                GL.glFlush();
            }
            Thread.Sleep(0);
        }
        private void CameraRotate(float angleX, float angleY)
        {
                float newAngleX = (c_ori_x + angleX) % 360.0f;
                float newAngleY = (c_ori_y + angleY) % 360.0f;

                c_ori_x = newAngleX;
                c_ori_y = newAngleY;
        }
        public void CameraTranslate(float dx, float dy, float dz)
        {
            if (dx == 0.0f & dy == 0.0f && dz == 0.0f)
            {
                /*c_pos_x = 0;
                c_pos_y = 0;
                c_pos_z = 0;*/
            }

            double xRot = this.DegreeToRadian(c_ori_x);
            double yRot = this.DegreeToRadian(c_ori_y);

            float x= (float)(dx * Math.Cos(yRot) + dy * Math.Sin(xRot) * Math.Sin(yRot) - dz * Math.Cos(xRot) * Math.Sin(yRot));
            float y= (float)(+dy * Math.Cos(xRot) + dz * Math.Sin(xRot));
            float z= (float)(dx * Math.Sin(yRot) - dy * Math.Sin(xRot) * Math.Cos(yRot) + dz * Math.Cos(xRot) * Math.Cos(yRot));

            c_pos_x += x;
            c_pos_y += y;
            c_pos_z += z;
        }
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        private void glDrawBlock(int px,int py,int pz,int sx,int sy,int sz, bool selected,string m)
        {
            Material mat = new Material();
            this.FindMaterial(m).CopyTo(mat);

            if (selected)
            {
                mat.ambient.R = 0.0f;
                mat.ambient.G = 1.0f;
                mat.ambient.B = 1.0f;
                mat.diffuse.R = 0.0f;
                mat.diffuse.G = 1.0f;
                mat.diffuse.B = 1.0f;
            }

            float[] diffuse = new float[] { mat.diffuse.R, mat.diffuse.G, mat.diffuse.B, mat.diffuse.A };
            float[] ambient = new float[] { mat.ambient.R, mat.ambient.G, mat.ambient.B, mat.ambient.A };
            float[] specular = new float[] { mat.specular.R, mat.specular.G, mat.specular.B, mat.specular.A };
            float[] emissive = new float[] { mat.emission.R, mat.emission.R, mat.emission.R, mat.emission.R };
            float shininess = mat.shininess;

            GL.glBegin(GL.GL_QUADS);
                // Bottom
                if (sy > 0)
                    { GL.glNormal3f(0.0f, -1.0f, 0.0f); }
                else
                    { GL.glNormal3f(0.0f, 1.0f, 0.0f); }
                GL.glMaterialfv(GL.GL_FRONT,GL.GL_DIFFUSE, diffuse);
                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, specular);
                GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, ambient);
                GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, emissive);
                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SHININESS, ref shininess);
                GL.glVertex3f(px, py, pz);
                GL.glVertex3f(px+sx, py, pz);
                GL.glVertex3f(px+sx, py, pz+sz);
                GL.glVertex3f(px, py, pz+sz);
                // Left
                if (sx > 0)
                    { GL.glNormal3f(-1.0f, 0.0f, 0.0f); }
                else
                    { GL.glNormal3f(1.0f, 0.0f, 0.0f); }
                GL.glVertex3f(px, py, pz);
                GL.glVertex3f(px, py+sy, pz);
                GL.glVertex3f(px, py+sy, pz + sz);
                GL.glVertex3f(px, py, pz + sz);
                // Top
                if (sy > 0)
                    {GL.glNormal3f(0.0f, 1.0f, 0.0f);}
                else
                    {GL.glNormal3f(0.0f, -1.0f, 0.0f);}
                GL.glVertex3f(px, py+sy, pz);
                GL.glVertex3f(px + sx, py + sy, pz);
                GL.glVertex3f(px + sx, py + sy, pz + sz);
                GL.glVertex3f(px, py + sy, pz + sz);
                // Right
                if (sx > 0)
                    { GL.glNormal3f(1.0f, 0.0f, 0.0f); }
                else
                    { GL.glNormal3f(-1.0f, 0.0f, 0.0f); }
                GL.glVertex3f(px + sx, py, pz);
                GL.glVertex3f(px + sx, py + sy, pz);
                GL.glVertex3f(px + sx, py + sy, pz + sz);
                GL.glVertex3f(px + sx, py, pz + sz);
                // Back
                if (sz > 0)
                    { GL.glNormal3f(0.0f, 0.0f, -1.0f); }
                else
                    { GL.glNormal3f(0.0f, 0.0f, 1.0f); }	
                GL.glVertex3f(px, py, pz);
                GL.glVertex3f(px, py + sy, pz);
                GL.glVertex3f(px + sx, py + sy, pz);
                GL.glVertex3f(px + sx, py, pz);
                // Front
                if (sz > 0)
                    { GL.glNormal3f(0.0f, 0.0f, 1.0f); }
                else
                    { GL.glNormal3f(0.0f, 0.0f, -1.0f); }		
                GL.glVertex3f(px, py, pz+sz);
                GL.glVertex3f(px, py + sy, pz + sz);
                GL.glVertex3f(px + sx, py + sy, pz + sz);
                GL.glVertex3f(px + sx, py, pz + sz);
            GL.glEnd();
        }
        protected override void InitGLContext()
        {
            GL.glClearColor(0.0f, 0.0f, 0.0f, 0.5f); 
            GL.glClearDepth(1.0f);                   
            GL.glEnable(GL.GL_LIGHTING);                                                                     
            GL.glLightModelf(GL.GL_LIGHT_MODEL_LOCAL_VIEWER,1.0f);
            float[] lightIntensity = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] lightSpecular = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] lightAmbient = new float[] { 0.80f, 0.80f, 0.80f, 1.0f };
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, lightIntensity);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, lightSpecular);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, lightAmbient);       
             GL.glEnable(GL.GL_LIGHT0);
             GL.glDepthFunc(GL.GL_LEQUAL);
             GL.glEnable(GL.GL_DEPTH_TEST);
             GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);
             GL.glEnable(GL.GL_BLEND);
             GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GL.gluPerspective(45.0f, (double)Size.Width / (double)Size.Height, 0.5f, 300.0f);
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
        }
        protected override bool IsInputKey(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                case Keys.A:
                case Keys.S:
                case Keys.D:
                case Keys.Tab:
                    return true;
                default:
                    return base.IsInputKey(key);
            }
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            this.KeyState[e.KeyValue] = false;
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            this.KeyState[e.KeyValue] = true;
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mouse_left = true;
            }
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mouse_left = false;
                this.last_x = 0;
                this.last_y = 0;
            }
        }
        private Material FindMaterial(string m)
        {
            Material retmat = new Material();
            foreach (Material i in this.material) if (i.name == m) { retmat = i; break; };
            return retmat;
        }
    }
}