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
using System.ComponentModel;

namespace Map_Editor
{
    [Serializable()]
    [DefaultPropertyAttribute("Name")]
	public class Block : Objects
	{

		public bool selected;
        public bool hover;

        /* Private properties */
        private string pLocalName;
        private string pMaterial;

        public LayerEnum _layer = LayerEnum.Layer1;

        /* Propertygrid connection */
        [CategoryAttribute("Item Settings"), DescriptionAttribute("Name")]
        public string Name
        {
            get
            {
                return pLocalName;
            }
            set
            {
                bool bf = false;
                foreach (Block b in MapEditor.Default.lstBlocks)
                {
                    if (b.Name == value && pLocalName != value)
                    {
                        bf=true;
                        break;
                    }
                }
                if (bf)
                {
                    MessageBox.Show("You selected a non-unique name, try again", "Error");
                }
                else
                {
                    pLocalName = value;
                }
            }
        }

        [TypeConverter(typeof(StringListConverter))]
        public string Material
        {
            get
            {
                return pMaterial;
            }
            set
            {
                pMaterial = value;
            }
        }

        [CategoryAttribute("Position"), DescriptionAttribute("This blocks X position")]
        public int PosX
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = value;
            }
        }
        [CategoryAttribute("Position"), DescriptionAttribute("This blocks Y position")]
        public int PosY
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = value;
            }
        }
        [CategoryAttribute("Position"), DescriptionAttribute("This blocks Z position")]
        public int PosZ
        {
            get
            {
                return Position.Z;
            }
            set
            {
                Position.Z = value;
            }
        }
        [CategoryAttribute("Size"), DescriptionAttribute("This blocks size in the x-axis")]
        public int SizeX
        {
            get
            {
                return Size.X;
            }
            set
            {
                Size.X = value;
            }
        }
        [CategoryAttribute("Size"), DescriptionAttribute("This blocks size in the y-axis")]
        public int SizeY
        {
            get
            {
                return Size.Y;
            }
            set
            {
                Size.Y = value;
            }
        }
        [CategoryAttribute("Size"), DescriptionAttribute("This blocks size in the z-axis")]
        public int SizeZ
        {
            get
            {
                return Size.Z;
            }
            set
            {
                Size.Z = value;
            }
        }

        [TypeConverter(typeof(LayerEnumConverter))]
        public string Layer
        {
            get
            {
                if (this._layer == LayerEnum.Layer1) { return "Layer 1"; }
                else if (this._layer == LayerEnum.Layer2) { return "Layer 2"; }
                else if (this._layer == LayerEnum.Layer3) { return "Layer 3"; }
                else if (this._layer == LayerEnum.Layer4) { return "Layer 4"; }
                else if (this._layer == LayerEnum.Layer5) { return "Layer 5"; }
                else { return "Layer 1"; }
            }
            set
            {
                if (value == "Layer 1") { this._layer = LayerEnum.Layer1; }
                else if (value == "Layer 2") { this._layer = LayerEnum.Layer2; }
                else if (value == "Layer 3") { this._layer = LayerEnum.Layer3; }
                else if (value == "Layer 4") { this._layer = LayerEnum.Layer4; }
                else if (value == "Layer 5") { this._layer = LayerEnum.Layer5; }
                else { this._layer = LayerEnum.Layer1; }
            }
        }

        public Position3D Position; // Position(X,Y,Z)
        public Size3D Size; // Size(X,Y,Z)

		public Block(string View, int Axis1_Pos, int Axis2_Pos, int Axis1_Size, int Axis2_Size, string block_name)
		{

            this.selected = false;
            this.hover = true;
            this.pLocalName = "";
            this.pMaterial = null;
            this.Material = MapEditor.Default.lstMaterial[0].name;

			if (View == "XY") // "Front" view, Axis1 = X, Axis2 = Y
			{
				
				Position.X = Axis1_Pos;
				Position.Y = Axis2_Pos;
				Position.Z = 0;
				
				Size.X = Axis1_Size;
				Size.Y = Axis2_Size;
				Size.Z = 1;
				
			}
			else if (View == "ZY") // "Left" view, Axis1 = Z, Axis2 = Y
			{
				
				Position.X = 0;
				Position.Y = Axis2_Pos;
				Position.Z = Axis1_Pos;
				
				Size.X = 1;
				Size.Y = Axis2_Size;
				Size.Z = Axis1_Size;
				
			}
			else if (View == "XZ") // "Top" view, Axis1 = X, Axis2 = Z
			{
				
				Position.X = Axis1_Pos;
				Position.Y = 0;
				Position.Z = Axis2_Pos;
				
				Size.X = Axis1_Size;
				Size.Y = 1;
				Size.Z = Axis2_Size;
				
			}
			else
			{
				
				MessageBox.Show("Item Created with invalid view");
				
			}
            this.pLocalName = block_name;
		}

	}
}
