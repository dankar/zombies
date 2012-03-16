using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CsGL.OpenGL;
using System.Windows.Forms;


namespace Map_Editor
{
    

    [Serializable()]
    public class Material
    {
        public string name="";
        public string type="";
        public MaterialColorRGBA ambient;
        public MaterialColorRGBA specular;
        public MaterialColorRGBA diffuse;
        public MaterialColorRGBA emission;
        public float shininess;

        public string texture_filename = "";
        public bool texture_loaded = false;
        [NonSerialized]
        public OpenGLTexture2D texture;

        public string bumpmap_filename = "";
        public bool bumpmap_loaded = false;
        [NonSerialized]
        public OpenGLTexture2D bumpmap;

        public Material()
        {
            this.ambient.A = 0.3f;
            this.ambient.R = 0.3f;
            this.ambient.G = 0.3f;
            this.ambient.B = 1.0f;
            this.specular.A = 0.3f;
            this.specular.R = 0.3f;
            this.specular.G = 0.3f;
            this.specular.B = 0.3f;
            this.emission.A = 1.0f;
            this.emission.R = 0.0f;
            this.emission.G = 0.0f;
            this.emission.B = 0.0f;
            this.diffuse.A = 1.0f;
            this.diffuse.R = 0.3f;
            this.diffuse.G = 0.3f;
            this.diffuse.B = 0.3f;
            this.shininess = 64.0f;
        }
        public bool LoadTexture(string filename)
        {
            try
            {
                this.texture_filename = filename;
                this.texture = new OpenGLTexture2D(Application.StartupPath + "\\textures\\" + filename);
                this.texture_loaded = true;
            }
            catch
            {
                this.texture_loaded = false;

            }
            return this.texture_loaded;
        }
        public bool LoadBumpMap(string filename)
        {
            try
            {
                this.bumpmap = new OpenGLTexture2D(Application.StartupPath + "\\textures\\" + filename);
                this.bumpmap_filename = filename;
                this.bumpmap_loaded = true;
            }
            catch
            {
                this.bumpmap_loaded = false;

            }
            return this.bumpmap_loaded;
        }
        public void CopyTo(Material m)
        {
            m.diffuse = this.diffuse;
            m.specular = this.specular;
            m.emission = this.emission;
            m.ambient = this.ambient;
            m.shininess = this.shininess;
            m.name = this.name;
            m.texture = this.texture;
            m.texture_filename = this.texture_filename;
            m.texture_loaded = this.texture_loaded;
            m.type = this.type;
            m.bumpmap = this.bumpmap;
            m.bumpmap_filename = this.bumpmap_filename;
            m.bumpmap_loaded = this.bumpmap_loaded;
        }
    }
    [Serializable()]
    public struct MaterialColorRGBA
    {
        public float R;
        public float G;
        public float B;
        public float A;
    }
    
    public class StringListConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
        return true;
        }
        /*public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
        return true; 
        }*/
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> lstMaterialNames = new List<string>();
            foreach (Material m in MapEditor.Default.lstMaterial) lstMaterialNames.Add(m.name);
            {
                return new StandardValuesCollection(lstMaterialNames);
            }
        }
    }

}
