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
      
namespace Map_Editor
{
    [Serializable()]
	public struct Position3D
	{
		public int X;
		public int Y;
		public int Z;
	}
    [Serializable()]
	public struct Size3D
	{
		public int X;
		public int Y;
		public int Z;
	}
    [Serializable()]
	public class Objects
	{
		public int ObjectID;
	}
}
