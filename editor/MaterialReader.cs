using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Map_Editor
{
    public class MaterialReader
    {
        private List<Material> lstMaterial = new List<Material>();
        public MaterialReader()
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(Application.StartupPath + "\\config\\materials.xml");
                reader.MoveToContent();

                while (reader.ReadToFollowing("material"))
                {
                    ProcessItem(reader.ReadSubtree());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ProcessItem(XmlReader reader)
        {
            reader.MoveToContent();

            Material tmpMaterial = new Material();

            tmpMaterial.name = reader.GetAttribute("displayname");
            tmpMaterial.type = reader.GetAttribute("type");
            
            if (reader.ReadToFollowing("ambient"))
            {
                reader.MoveToContent();
                tmpMaterial.ambient.R = (float)Convert.ToDouble(reader.GetAttribute("r").Replace(".", ","));
                tmpMaterial.ambient.G = (float)Convert.ToDouble(reader.GetAttribute("g").Replace(".", ","));
                tmpMaterial.ambient.B = (float)Convert.ToDouble(reader.GetAttribute("b").Replace(".", ","));
                tmpMaterial.ambient.A = (float)Convert.ToDouble(reader.GetAttribute("a").Replace(".", ","));
            }
            if (reader.ReadToFollowing("specular"))
            {
                reader.MoveToContent();
                tmpMaterial.specular.R = (float)Convert.ToDouble(reader.GetAttribute("r").Replace(".", ","));
                tmpMaterial.specular.G = (float)Convert.ToDouble(reader.GetAttribute("g").Replace(".", ","));
                tmpMaterial.specular.B = (float)Convert.ToDouble(reader.GetAttribute("b").Replace(".", ","));
                tmpMaterial.specular.A = (float)Convert.ToDouble(reader.GetAttribute("a").Replace(".", ","));
            }
            if (reader.ReadToFollowing("diffuse"))
            {
                reader.MoveToContent();
                tmpMaterial.diffuse.R = (float)Convert.ToDouble(reader.GetAttribute("r").Replace(".", ","));
                tmpMaterial.diffuse.G = (float)Convert.ToDouble(reader.GetAttribute("g").Replace(".", ","));
                tmpMaterial.diffuse.B = (float)Convert.ToDouble(reader.GetAttribute("b").Replace(".", ","));
                tmpMaterial.diffuse.A = (float)Convert.ToDouble(reader.GetAttribute("a").Replace(".", ","));
            }
            if (reader.ReadToFollowing("emissive"))
            {
                reader.MoveToContent();
                tmpMaterial.emission.R = (float)Convert.ToDouble(reader.GetAttribute("r").Replace(".", ","));
                tmpMaterial.emission.G = (float)Convert.ToDouble(reader.GetAttribute("g").Replace(".", ","));
                tmpMaterial.emission.B = (float)Convert.ToDouble(reader.GetAttribute("b").Replace(".", ","));
                tmpMaterial.emission.A = (float)Convert.ToDouble(reader.GetAttribute("a").Replace(".", ","));
            }
            if (reader.ReadToFollowing("shininess"))
            {
                reader.MoveToContent();
                tmpMaterial.shininess = (float)Convert.ToDouble(reader.GetAttribute("value").Replace(".", ","));
            }
            if (reader.ReadToFollowing("texture"))
            {
                reader.MoveToContent();
                tmpMaterial.LoadTexture(reader.GetAttribute("file"));
            }
            this.lstMaterial.Add(tmpMaterial);
        }
        public List<Material> GetMaterialList()
        {
            return lstMaterial;
        }
    }
}
