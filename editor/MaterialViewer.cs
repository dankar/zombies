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
    public class MaterialViewer : OpenGLControl
    {

        EventHandler<TickEventArgs> tEventHandler;
        bool isConnected=false;
        double ir = 0;
        private Material material = new Material();
        public void Connect()
		{
            tEventHandler = new EventHandler<TickEventArgs>(OnTick);
            this.isConnected = true;
            SdlDotNet.Core.Events.Tick += tEventHandler;
		}
        public MaterialViewer()
        {

        }
        ~MaterialViewer()
        {
            SdlDotNet.Core.Events.Tick -= tEventHandler;
            tEventHandler = null;
        }
        public void SetMaterial(Material m)
        {
            this.material = m;
        }
        public void OnTick(object sender, SdlDotNet.Core.TickEventArgs e)
        {
            this.InvokeRefresh();
            Thread.Sleep(10);
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
                GL.gluLookAt(10.0f, 0.0f, 0.0f, 0f, 0f, 0f, 0, 1.0f, 0);

                float[] lightPosition = new float[] { 10.0f, 10.0f, 0.0f, 0.00f };
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, lightPosition);	

                GL.glRotatef((float)ir,0.1f,0.0f,0.0f);
                GL.glRotatef((float)ir, 0.0f, 0.1f, 0.0f);

                glDrawBlock(material);
                Thread.Sleep(0);

                GL.glFlush();
                ir += 0.1f;
            }
            Thread.Sleep(0);
        }
        private void glDrawBlock(Material mat)
        {

            float[] diffuse = new float[] { mat.diffuse.R, mat.diffuse.G, mat.diffuse.B, mat.diffuse.A };
            float[] ambient = new float[] { mat.ambient.R, mat.ambient.G, mat.ambient.B, mat.ambient.A };
            float[] specular = new float[] { mat.specular.R, mat.specular.G, mat.specular.B, mat.specular.A };
            float[] emission = new float[] { mat.emission.R, mat.emission.G, mat.emission.B, mat.emission.A };
            float shininess = mat.shininess;

            if (mat.texture_loaded)
            {
                mat.texture.Bind();
            }
            GL.glMaterialfv(GL.GL_FRONT,GL.GL_DIFFUSE, diffuse);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, specular);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, ambient);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, emission);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_SHININESS, ref shininess);

            GL.glutSolidSphere(2.5f, 50, 50);
        }
        protected override void InitGLContext()
        {
            GL.glClearColor(0.0f, 0.0f, 0.0f, 0.5f); 
            GL.glClearDepth(1.0f);                   
            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_TEXTURE_2D);                                     
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

    }
}