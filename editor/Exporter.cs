using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Map_Editor
{
    public class Exporter
    {
        private List<Material> materials;
        private List<Entity> entities;
        private List<Item> items;
        private List<Block> blocks;
        private LevelInformation levelinfo;

        public Exporter(object _blocks, object _items, object _entities, object _materials, object _levelinfo)
        {
            try
            {
                this.materials = (List<Material>)_materials;
                this.entities = (List<Entity>)_entities;
                this.items = (List<Item>)_items;
                this.blocks = (List<Block>)_blocks;
                this.levelinfo = (LevelInformation)_levelinfo;
            } catch(Exception ex) {
                MessageBox.Show(MapEditor.Default, "Failed to initialize exporter: " + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
            }
        }

        public void ExportToXML(string filePath)
        {
            try
            {
                FileInfo f = new FileInfo(filePath);
                StreamWriter swOut = f.CreateText();

                swOut.WriteLine("<level name=\""+levelinfo.LevelName+"\" author=\""+levelinfo.Author+"\" email=\""+levelinfo.Email+"\" datemodified=\""+levelinfo.DateModified+"\"  >");
                swOut.WriteLine("   <light name=\"GlobalAmbient\" r=\"" + levelinfo.Red.ToString().Replace(",", ".") + "\" g=\"" + levelinfo.Green.ToString().Replace(",", ".") + "\" b=\"" + levelinfo.Blue.ToString().Replace(",", ".") + "\" a=\"" + levelinfo.Alpha.ToString().Replace(",", ".") + "\"  />");

                foreach(Material m in this.materials){
                    swOut.WriteLine("   <material name=\""+m.name+"\" type=\"" + m.type + "\">");
                    swOut.WriteLine("      <ambient r=\"" + m.ambient.R.ToString().Replace(",", ".") + "\" g=\"" + m.ambient.G.ToString().Replace(",", ".") + "\" b=\"" + m.ambient.B.ToString().Replace(",", ".") + "\" a=\"" + m.ambient.A.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("      <specular r=\"" + m.specular.R.ToString().Replace(",", ".") + "\" g=\"" + m.specular.G.ToString().Replace(",", ".") + "\" b=\"" + m.specular.B.ToString().Replace(",", ".") + "\" a=\"" + m.specular.A.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("      <diffuse r=\"" + m.diffuse.R.ToString().Replace(",", ".") + "\" g=\"" + m.diffuse.G.ToString().Replace(",", ".") + "\" b=\"" + m.diffuse.B.ToString().Replace(",", ".") + "\" a=\"" + m.diffuse.A.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("      <emissive r=\"" + m.emission.R.ToString().Replace(",", ".") + "\" g=\"" + m.emission.G.ToString().Replace(",", ".") + "\" b=\"" + m.emission.B.ToString().Replace(",", ".") + "\" a=\"" + m.emission.A.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("      <shininess value=\"" + m.shininess.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("      <texture file=\"" + m.texture + "\" />");
                    swOut.WriteLine("   </material>");
                }

                foreach (Block b in this.blocks)
                {
                    swOut.WriteLine("   <block name=\"" + b.Name + "\" material=\"" + b.Material + "\">");
                    swOut.WriteLine("      <size x=\"" + b.Size.X.ToString().Replace(",", ".") + "\" y=\"" + b.Size.Y.ToString().Replace(",", ".") + "\" z=\"" + b.Size.Z.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("      <position x=\"" + b.Position.X.ToString().Replace(",", ".") + "\" y=\"" + b.Position.Y.ToString().Replace(",", ".") + "\" z=\"" + b.Position.Z.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("   </block>");
                }

                foreach (Item i in this.items)
                {
                    swOut.WriteLine("   <item name=\"" + i.Name + "\" type=\"" + i.iItemType + "\">");
                    swOut.WriteLine("      <position x=\"" + i.Position.X.ToString().Replace(",", ".") + "\" y=\"" + i.Position.Y.ToString().Replace(",", ".") + "\" z=\"" + i.Position.Z.ToString().Replace(",", ".") + "\" />");
                    swOut.WriteLine("   </item>");
                }

                swOut.WriteLine("</level>");

                swOut.Write(swOut.NewLine);
                swOut.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MapEditor.Default, "Failed to export to xml file: " + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        public void ExportToTSV(string filePath)
        {

        }
    }
}
