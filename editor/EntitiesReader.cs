using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Map_Editor
{
    public class EntitiesReader
    {
        private List<Entity> lstEntity = new List<Entity>();
        public EntitiesReader()
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(Application.StartupPath + "\\config\\entities.xml");

                reader.MoveToContent();

                while (reader.ReadToFollowing("entity"))
                {
                    ProcessItem(reader.ReadSubtree());
                }
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ProcessItem(XmlReader reader)
        {
            reader.MoveToContent();

            Entity tmpEntity = new Entity();

            string name = reader.GetAttribute("displayname");
            string type = reader.GetAttribute("type");

            tmpEntity.DisplayName = name;
            tmpEntity.Type = type;
            this.lstEntity.Add(tmpEntity);
            //reader.ReadToFollowing("title");
            //string title = reader.ReadElementContentAsString("title", reader.NamespaceURI);
        }
        public List<Entity> GetEntityList()
        {
            return lstEntity;
        }
    }
}
