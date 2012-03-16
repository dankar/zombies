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
	public class Item : Objects
	{
        private string _name;
        private string _iItemType;
        public LayerEnum _layer = LayerEnum.Layer1;

        [CategoryAttribute("Item ID"), DescriptionAttribute("Name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                bool bf = false;
                foreach (Item i in MapEditor.Default.lstItems)
                {
                    if (i.Name == value && _name != value)
                    {
                        bf=true;
                        break;
                    }
                }
                if (bf)
                {
                    MessageBox.Show("You selected a non-unique item name, try again", "Error");
                }
                else
                {
                    _name = value;
                }
            }
        }
        [CategoryAttribute("Item ID"), DescriptionAttribute("Type")]
        public string iItemType
        {
            get
            {
                return _iItemType;
            }
            set
            {
                _iItemType = value;
            }
        }
        [CategoryAttribute("Position"), DescriptionAttribute("This blocks X position")]
        public int X
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
        public int Y
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
        public int Z
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

        [TypeConverter(typeof(LayerEnumConverter))]
        public string Layer
        {
            get
            {
                if (this._layer == LayerEnum.Layer1) { return "Layer 1"; }
                else if (this._layer == LayerEnum.Layer2) {  return "Layer 2"; }
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

        public Position3D Position;

        public bool selected = false;
        
		public Item(string View, int Axis1, int Axis2, string item_id, string item_name)
		{
			if (View == "XY") // "Front" view, Axis1 = X, Axis2 = Y
			{
				this.Position.X = Axis1;
				this.Position.Y = Axis2;
				this.Position.Z = 0;
			}
			else if (View == "ZY") // "Left" view, Axis1 = Z, Axis2 = Y
			{
				this.Position.X = 0;
				this.Position.Y = Axis2;
				this.Position.Z = Axis1;
			}
			else if (View == "XZ") // "Top" view, Axis1 = X, Axis2 = Z
			{
				this.Position.X = Axis1;
				this.Position.Y = 0;
				this.Position.Z = Axis2;
			}
			else
			{
				MessageBox.Show("Item Created with invalid view");
			}
            this._iItemType = item_id;
            this._name = item_name;
            this._layer = LayerEnum.Layer1;
		}
		
	}
	
}
