using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;


namespace Map_Editor
{
    public class Entity
    {
        public string Type = "";
        public string DisplayName = "";
    }
    public class EntToolStripMenuItem : ToolStripMenuItem
    {
        public int entityIndex=0;
    }

}
