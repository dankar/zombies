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
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public partial class MapEditor : System.Windows.Forms.Form
		{
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
			{
			try
			{
				if (disposing && (components != null))
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

        //Required by the Windows Form Designer
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
			{
                this.components = new System.ComponentModel.Container();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
                this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
                this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
                this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.SaveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
                this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.BlablaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
                this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.tsmSelectMove = new System.Windows.Forms.ToolStripMenuItem();
                this.tsmCreate = new System.Windows.Forms.ToolStripMenuItem();
                this.deleteSelectedObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
                this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.settiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
                this.layersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.miLayer1 = new System.Windows.Forms.ToolStripMenuItem();
                this.miLayer2 = new System.Windows.Forms.ToolStripMenuItem();
                this.miLayer3 = new System.Windows.Forms.ToolStripMenuItem();
                this.miLayer4 = new System.Windows.Forms.ToolStripMenuItem();
                this.miLayer5 = new System.Windows.Forms.ToolStripMenuItem();
                this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
                this.tslTool = new System.Windows.Forms.ToolStripStatusLabel();
                this.tslView = new System.Windows.Forms.ToolStripStatusLabel();
                this.tslCoordinates = new System.Windows.Forms.ToolStripStatusLabel();
                this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
                this.tsbSelectMove = new System.Windows.Forms.ToolStripButton();
                this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
                this.tsbCreate = new System.Windows.Forms.ToolStripButton();
                this.tsdEntities = new System.Windows.Forms.ToolStripDropDownButton();
                this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
                this.tsbLayer5 = new System.Windows.Forms.ToolStripButton();
                this.tsbLayer4 = new System.Windows.Forms.ToolStripButton();
                this.tsbLayer3 = new System.Windows.Forms.ToolStripButton();
                this.tsbLayer2 = new System.Windows.Forms.ToolStripButton();
                this.tsbLayer1 = new System.Windows.Forms.ToolStripButton();
                this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
                this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
                this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.splitContainer1 = new System.Windows.Forms.SplitContainer();
                this.splitVertical = new System.Windows.Forms.SplitContainer();
                this.splitHorizontalLeft = new System.Windows.Forms.SplitContainer();
                this.scXY = new SdlDotNet.Windows.SurfaceControl();
                this.scXZ = new SdlDotNet.Windows.SurfaceControl();
                this.splitHorizontalRight = new System.Windows.Forms.SplitContainer();
                this.scZY = new SdlDotNet.Windows.SurfaceControl();
                this.glView1 = new Map_Editor.View3D();
                this.cbItemSelector = new System.Windows.Forms.ComboBox();
                this.pgProperties = new System.Windows.Forms.PropertyGrid();
                this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
                this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
                this.materialEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.MenuStrip1.SuspendLayout();
                this.StatusStrip1.SuspendLayout();
                this.ToolStrip1.SuspendLayout();
                this.contextMenuStrip1.SuspendLayout();
                this.splitContainer1.Panel1.SuspendLayout();
                this.splitContainer1.Panel2.SuspendLayout();
                this.splitContainer1.SuspendLayout();
                this.splitVertical.Panel1.SuspendLayout();
                this.splitVertical.Panel2.SuspendLayout();
                this.splitVertical.SuspendLayout();
                this.splitHorizontalLeft.Panel1.SuspendLayout();
                this.splitHorizontalLeft.Panel2.SuspendLayout();
                this.splitHorizontalLeft.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.scXY)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.scXZ)).BeginInit();
                this.splitHorizontalRight.Panel1.SuspendLayout();
                this.splitHorizontalRight.Panel2.SuspendLayout();
                this.splitHorizontalRight.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.scZY)).BeginInit();
                this.SuspendLayout();
                // 
                // MenuStrip1
                // 
                this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.settiToolStripMenuItem,
            this.HelpToolStripMenuItem});
                this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
                this.MenuStrip1.Name = "MenuStrip1";
                this.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
                this.MenuStrip1.Size = new System.Drawing.Size(790, 24);
                this.MenuStrip1.TabIndex = 0;
                this.MenuStrip1.Text = "MenuStrip1";
                // 
                // FileToolStripMenuItem
                // 
                this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolStripMenuItem,
            this.ToolStripMenuItem1,
            this.openToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveasToolStripMenuItem,
            this.toolStripMenuItem5,
            this.ExportToolStripMenuItem,
            this.ToolStripMenuItem2,
            this.ExitToolStripMenuItem});
                this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
                this.FileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
                this.FileToolStripMenuItem.Text = "&File";
                // 
                // NewToolStripMenuItem
                // 
                this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
                this.NewToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
                this.NewToolStripMenuItem.Text = "&New";
                this.NewToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
                // 
                // ToolStripMenuItem1
                // 
                this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
                this.ToolStripMenuItem1.Size = new System.Drawing.Size(148, 6);
                // 
                // openToolStripMenuItem
                // 
                this.openToolStripMenuItem.Name = "openToolStripMenuItem";
                this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
                this.openToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
                this.openToolStripMenuItem.Text = "&Open";
                this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
                // 
                // SaveToolStripMenuItem
                // 
                this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
                this.SaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
                this.SaveToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
                this.SaveToolStripMenuItem.Text = "&Save";
                this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
                // 
                // SaveasToolStripMenuItem
                // 
                this.SaveasToolStripMenuItem.Name = "SaveasToolStripMenuItem";
                this.SaveasToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
                this.SaveasToolStripMenuItem.Text = "Save &as ...";
                this.SaveasToolStripMenuItem.Click += new System.EventHandler(this.SaveasToolStripMenuItem_Click);
                // 
                // toolStripMenuItem5
                // 
                this.toolStripMenuItem5.Name = "toolStripMenuItem5";
                this.toolStripMenuItem5.Size = new System.Drawing.Size(148, 6);
                // 
                // ExportToolStripMenuItem
                // 
                this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BlablaToolStripMenuItem});
                this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
                this.ExportToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
                this.ExportToolStripMenuItem.Text = "Export";
                // 
                // BlablaToolStripMenuItem
                // 
                this.BlablaToolStripMenuItem.Name = "BlablaToolStripMenuItem";
                this.BlablaToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
                this.BlablaToolStripMenuItem.Text = "Zombies XML map ...";
                this.BlablaToolStripMenuItem.Click += new System.EventHandler(this.BlablaToolStripMenuItem_Click);
                // 
                // ToolStripMenuItem2
                // 
                this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
                this.ToolStripMenuItem2.Size = new System.Drawing.Size(148, 6);
                // 
                // ExitToolStripMenuItem
                // 
                this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
                this.ExitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
                this.ExitToolStripMenuItem.Text = "Exit";
                this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
                // 
                // ToolsToolStripMenuItem
                // 
                this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmSelectMove,
            this.tsmCreate,
            this.deleteSelectedObjectToolStripMenuItem,
            this.toolStripMenuItem8,
            this.materialEditorToolStripMenuItem,
            this.toolStripMenuItem7,
            this.propertiesToolStripMenuItem});
                this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
                this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
                this.ToolsToolStripMenuItem.Text = "Tools";
                // 
                // tsmSelectMove
                // 
                this.tsmSelectMove.Name = "tsmSelectMove";
                this.tsmSelectMove.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
                this.tsmSelectMove.Size = new System.Drawing.Size(210, 22);
                this.tsmSelectMove.Text = "Selector";
                this.tsmSelectMove.Click += new System.EventHandler(this.tsbSelectMove_Click);
                // 
                // tsmCreate
                // 
                this.tsmCreate.Name = "tsmCreate";
                this.tsmCreate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
                this.tsmCreate.Size = new System.Drawing.Size(210, 22);
                this.tsmCreate.Text = "Create block";
                this.tsmCreate.Click += new System.EventHandler(this.tsbCreate_Click);
                // 
                // deleteSelectedObjectToolStripMenuItem
                // 
                this.deleteSelectedObjectToolStripMenuItem.Name = "deleteSelectedObjectToolStripMenuItem";
                this.deleteSelectedObjectToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
                this.deleteSelectedObjectToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
                this.deleteSelectedObjectToolStripMenuItem.Text = "Delete object";
                this.deleteSelectedObjectToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedObjectToolStripMenuItem_Click);
                // 
                // toolStripMenuItem7
                // 
                this.toolStripMenuItem7.Name = "toolStripMenuItem7";
                this.toolStripMenuItem7.Size = new System.Drawing.Size(207, 6);
                // 
                // propertiesToolStripMenuItem
                // 
                this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
                this.propertiesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
                this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
                this.propertiesToolStripMenuItem.Text = "Map properties ...";
                this.propertiesToolStripMenuItem.ToolTipText = "Properties for selected block";
                this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
                // 
                // settiToolStripMenuItem
                // 
                this.settiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridToolStripMenuItem,
            this.toolStripMenuItem4,
            this.layersToolStripMenuItem,
            this.toolStripMenuItem3});
                this.settiToolStripMenuItem.Name = "settiToolStripMenuItem";
                this.settiToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
                this.settiToolStripMenuItem.Text = "View";
                // 
                // gridToolStripMenuItem
                // 
                this.gridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem});
                this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
                this.gridToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.gridToolStripMenuItem.Text = "Grid";
                // 
                // zoomInToolStripMenuItem
                // 
                this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
                this.zoomInToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
                this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
                this.zoomInToolStripMenuItem.Text = "Zoom in";
                this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
                // 
                // zoomOutToolStripMenuItem
                // 
                this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
                this.zoomOutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
                this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
                this.zoomOutToolStripMenuItem.Text = "Zoom out";
                this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
                // 
                // toolStripMenuItem4
                // 
                this.toolStripMenuItem4.Name = "toolStripMenuItem4";
                this.toolStripMenuItem4.Size = new System.Drawing.Size(149, 6);
                // 
                // layersToolStripMenuItem
                // 
                this.layersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLayer1,
            this.miLayer2,
            this.miLayer3,
            this.miLayer4,
            this.miLayer5});
                this.layersToolStripMenuItem.Name = "layersToolStripMenuItem";
                this.layersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.layersToolStripMenuItem.Text = "Hide layers";
                // 
                // miLayer1
                // 
                this.miLayer1.Checked = true;
                this.miLayer1.CheckState = System.Windows.Forms.CheckState.Checked;
                this.miLayer1.Name = "miLayer1";
                this.miLayer1.ShortcutKeys = System.Windows.Forms.Keys.F1;
                this.miLayer1.Size = new System.Drawing.Size(140, 22);
                this.miLayer1.Text = "Layer 1";
                this.miLayer1.Click += new System.EventHandler(this.showHideLayer1ToolStripMenuItem_Click);
                // 
                // miLayer2
                // 
                this.miLayer2.Checked = true;
                this.miLayer2.CheckState = System.Windows.Forms.CheckState.Checked;
                this.miLayer2.Name = "miLayer2";
                this.miLayer2.ShortcutKeys = System.Windows.Forms.Keys.F2;
                this.miLayer2.Size = new System.Drawing.Size(140, 22);
                this.miLayer2.Text = "Layer 2";
                this.miLayer2.Click += new System.EventHandler(this.showHideLayer2ToolStripMenuItem_Click);
                // 
                // miLayer3
                // 
                this.miLayer3.Checked = true;
                this.miLayer3.CheckState = System.Windows.Forms.CheckState.Checked;
                this.miLayer3.Name = "miLayer3";
                this.miLayer3.ShortcutKeys = System.Windows.Forms.Keys.F3;
                this.miLayer3.Size = new System.Drawing.Size(140, 22);
                this.miLayer3.Text = "Layer 3";
                this.miLayer3.Click += new System.EventHandler(this.showHideLayer3ToolStripMenuItem_Click);
                // 
                // miLayer4
                // 
                this.miLayer4.Checked = true;
                this.miLayer4.CheckState = System.Windows.Forms.CheckState.Checked;
                this.miLayer4.Name = "miLayer4";
                this.miLayer4.ShortcutKeys = System.Windows.Forms.Keys.F4;
                this.miLayer4.Size = new System.Drawing.Size(140, 22);
                this.miLayer4.Text = "Layer 4";
                this.miLayer4.Click += new System.EventHandler(this.showHideLayer4ToolStripMenuItem_Click);
                // 
                // miLayer5
                // 
                this.miLayer5.Checked = true;
                this.miLayer5.CheckState = System.Windows.Forms.CheckState.Checked;
                this.miLayer5.Name = "miLayer5";
                this.miLayer5.ShortcutKeys = System.Windows.Forms.Keys.F5;
                this.miLayer5.Size = new System.Drawing.Size(140, 22);
                this.miLayer5.Text = "Layer 5";
                this.miLayer5.Click += new System.EventHandler(this.showHideLayer5ToolStripMenuItem_Click);
                // 
                // HelpToolStripMenuItem
                // 
                this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
                this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
                this.HelpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
                this.HelpToolStripMenuItem.Text = "Help";
                // 
                // AboutToolStripMenuItem
                // 
                this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
                this.AboutToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
                this.AboutToolStripMenuItem.Text = "ToDo: About ...";
                this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
                // 
                // StatusStrip1
                // 
                this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslTool,
            this.tslView,
            this.tslCoordinates});
                this.StatusStrip1.Location = new System.Drawing.Point(0, 447);
                this.StatusStrip1.Name = "StatusStrip1";
                this.StatusStrip1.Size = new System.Drawing.Size(790, 22);
                this.StatusStrip1.TabIndex = 1;
                // 
                // tslTool
                // 
                this.tslTool.Name = "tslTool";
                this.tslTool.Size = new System.Drawing.Size(34, 17);
                this.tslTool.Text = "Tool: ";
                // 
                // tslView
                // 
                this.tslView.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
                this.tslView.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
                this.tslView.Name = "tslView";
                this.tslView.Size = new System.Drawing.Size(37, 17);
                this.tslView.Text = "View:";
                // 
                // tslCoordinates
                // 
                this.tslCoordinates.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
                this.tslCoordinates.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
                this.tslCoordinates.Name = "tslCoordinates";
                this.tslCoordinates.Size = new System.Drawing.Size(47, 17);
                this.tslCoordinates.Text = "Coord: ";
                // 
                // ToolStrip1
                // 
                this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSelectMove,
            this.toolStripSeparator1,
            this.tsbCreate,
            this.tsdEntities,
            this.toolStripSeparator2,
            this.tsbLayer5,
            this.tsbLayer4,
            this.tsbLayer3,
            this.tsbLayer2,
            this.tsbLayer1,
            this.toolStripLabel1});
                this.ToolStrip1.Location = new System.Drawing.Point(0, 24);
                this.ToolStrip1.Name = "ToolStrip1";
                this.ToolStrip1.Size = new System.Drawing.Size(790, 25);
                this.ToolStrip1.TabIndex = 2;
                this.ToolStrip1.Text = "ToolStrip1";
                // 
                // tsbSelectMove
                // 
                this.tsbSelectMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsbSelectMove.Image = global::My.Resources.Resources.toolbutton_move;
                this.tsbSelectMove.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsbSelectMove.Name = "tsbSelectMove";
                this.tsbSelectMove.Size = new System.Drawing.Size(23, 22);
                this.tsbSelectMove.Text = "Select, move and resize (CTRL+1)";
                this.tsbSelectMove.Click += new System.EventHandler(this.tsbSelectMove_Click);
                // 
                // toolStripSeparator1
                // 
                this.toolStripSeparator1.Name = "toolStripSeparator1";
                this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
                // 
                // tsbCreate
                // 
                this.tsbCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsbCreate.Image = global::My.Resources.Resources.toolbutton_new;
                this.tsbCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsbCreate.Name = "tsbCreate";
                this.tsbCreate.Size = new System.Drawing.Size(23, 22);
                this.tsbCreate.Text = "Create block (CTRL+2)";
                this.tsbCreate.Click += new System.EventHandler(this.tsbCreate_Click);
                // 
                // tsdEntities
                // 
                this.tsdEntities.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsdEntities.Image = global::My.Resources.Resources.toolbutton_entities;
                this.tsdEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsdEntities.Name = "tsdEntities";
                this.tsdEntities.Size = new System.Drawing.Size(29, 22);
                this.tsdEntities.Text = "Map entities";
                // 
                // toolStripSeparator2
                // 
                this.toolStripSeparator2.Name = "toolStripSeparator2";
                this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
                // 
                // tsbLayer5
                // 
                this.tsbLayer5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tsbLayer5.CheckOnClick = true;
                this.tsbLayer5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsbLayer5.Image = global::My.Resources.Resources.toolbutton_l5;
                this.tsbLayer5.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsbLayer5.Name = "tsbLayer5";
                this.tsbLayer5.Size = new System.Drawing.Size(23, 22);
                this.tsbLayer5.Text = "toolStripButton1";
                this.tsbLayer5.Click += new System.EventHandler(this.tsbLayer5_Click);
                // 
                // tsbLayer4
                // 
                this.tsbLayer4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tsbLayer4.CheckOnClick = true;
                this.tsbLayer4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsbLayer4.Image = global::My.Resources.Resources.toolbutton_l4;
                this.tsbLayer4.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsbLayer4.Name = "tsbLayer4";
                this.tsbLayer4.Size = new System.Drawing.Size(23, 22);
                this.tsbLayer4.Text = "tsbLayer4";
                this.tsbLayer4.Click += new System.EventHandler(this.tsbLayer4_Click);
                // 
                // tsbLayer3
                // 
                this.tsbLayer3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tsbLayer3.CheckOnClick = true;
                this.tsbLayer3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsbLayer3.Image = global::My.Resources.Resources.toolbutton_l3;
                this.tsbLayer3.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsbLayer3.Name = "tsbLayer3";
                this.tsbLayer3.Size = new System.Drawing.Size(23, 22);
                this.tsbLayer3.Text = "tsbLayer3";
                this.tsbLayer3.Click += new System.EventHandler(this.tsbLayer3_Click);
                // 
                // tsbLayer2
                // 
                this.tsbLayer2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tsbLayer2.CheckOnClick = true;
                this.tsbLayer2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsbLayer2.Image = global::My.Resources.Resources.toolbutton_l2;
                this.tsbLayer2.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsbLayer2.Name = "tsbLayer2";
                this.tsbLayer2.Size = new System.Drawing.Size(23, 22);
                this.tsbLayer2.Text = "tsbLayer2";
                this.tsbLayer2.Click += new System.EventHandler(this.tsbLayer2_Click);
                // 
                // tsbLayer1
                // 
                this.tsbLayer1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tsbLayer1.CheckOnClick = true;
                this.tsbLayer1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsbLayer1.Image = global::My.Resources.Resources.toolbutton_l1;
                this.tsbLayer1.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsbLayer1.Name = "tsbLayer1";
                this.tsbLayer1.Size = new System.Drawing.Size(23, 22);
                this.tsbLayer1.Text = "tsbLayer1";
                this.tsbLayer1.Click += new System.EventHandler(this.tsbLayer1_Click);
                // 
                // toolStripLabel1
                // 
                this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.toolStripLabel1.Name = "toolStripLabel1";
                this.toolStripLabel1.Size = new System.Drawing.Size(63, 22);
                this.toolStripLabel1.Text = "Hide Layers";
                // 
                // contextMenuStrip1
                // 
                this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
                this.contextMenuStrip1.Name = "contextMenuStrip1";
                this.contextMenuStrip1.Size = new System.Drawing.Size(105, 26);
                // 
                // testToolStripMenuItem
                // 
                this.testToolStripMenuItem.Name = "testToolStripMenuItem";
                this.testToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
                this.testToolStripMenuItem.Text = "test";
                // 
                // splitContainer1
                // 
                this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitContainer1.Location = new System.Drawing.Point(0, 49);
                this.splitContainer1.Name = "splitContainer1";
                // 
                // splitContainer1.Panel1
                // 
                this.splitContainer1.Panel1.Controls.Add(this.splitVertical);
                // 
                // splitContainer1.Panel2
                // 
                this.splitContainer1.Panel2.Controls.Add(this.cbItemSelector);
                this.splitContainer1.Panel2.Controls.Add(this.pgProperties);
                this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 4, 4, 0);
                this.splitContainer1.Panel2.Resize += new System.EventHandler(this.splitContainer1_Panel2_Resize);
                this.splitContainer1.Size = new System.Drawing.Size(790, 398);
                this.splitContainer1.SplitterDistance = 638;
                this.splitContainer1.TabIndex = 4;
                // 
                // splitVertical
                // 
                this.splitVertical.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitVertical.Location = new System.Drawing.Point(0, 0);
                this.splitVertical.Name = "splitVertical";
                // 
                // splitVertical.Panel1
                // 
                this.splitVertical.Panel1.Controls.Add(this.splitHorizontalLeft);
                // 
                // splitVertical.Panel2
                // 
                this.splitVertical.Panel2.Controls.Add(this.splitHorizontalRight);
                this.splitVertical.Size = new System.Drawing.Size(638, 398);
                this.splitVertical.SplitterDistance = 338;
                this.splitVertical.TabIndex = 4;
                // 
                // splitHorizontalLeft
                // 
                this.splitHorizontalLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.splitHorizontalLeft.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitHorizontalLeft.Location = new System.Drawing.Point(0, 0);
                this.splitHorizontalLeft.Name = "splitHorizontalLeft";
                this.splitHorizontalLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
                // 
                // splitHorizontalLeft.Panel1
                // 
                this.splitHorizontalLeft.Panel1.Controls.Add(this.scXY);
                // 
                // splitHorizontalLeft.Panel2
                // 
                this.splitHorizontalLeft.Panel2.Controls.Add(this.scXZ);
                this.splitHorizontalLeft.Size = new System.Drawing.Size(338, 398);
                this.splitHorizontalLeft.SplitterDistance = 195;
                this.splitHorizontalLeft.TabIndex = 0;
                this.splitHorizontalLeft.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitHorizontalLeft_SplitterMoved);
                // 
                // scXY
                // 
                this.scXY.AccessibleDescription = "SdlDotNet SurfaceControl";
                this.scXY.AccessibleName = "SurfaceControl";
                this.scXY.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
                this.scXY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                this.scXY.Dock = System.Windows.Forms.DockStyle.Fill;
                this.scXY.Image = ((System.Drawing.Image)(resources.GetObject("scXY.Image")));
                this.scXY.InitialImage = ((System.Drawing.Image)(resources.GetObject("scXY.InitialImage")));
                this.scXY.Location = new System.Drawing.Point(0, 0);
                this.scXY.Name = "scXY";
                this.scXY.Size = new System.Drawing.Size(334, 191);
                this.scXY.TabIndex = 0;
                this.scXY.TabStop = false;
                this.scXY.MouseLeave += new System.EventHandler(this.scXY_MouseLeave);
                this.scXY.MouseMove += new System.Windows.Forms.MouseEventHandler(this.scXY_MouseMove);
                this.scXY.Resize += new System.EventHandler(this.scXY_Resize);
                this.scXY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scXY_MouseDown);
                this.scXY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.scXY_MouseUp);
                // 
                // scXZ
                // 
                this.scXZ.AccessibleDescription = "SdlDotNet SurfaceControl";
                this.scXZ.AccessibleName = "SurfaceControl";
                this.scXZ.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
                this.scXZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                this.scXZ.Dock = System.Windows.Forms.DockStyle.Fill;
                this.scXZ.Image = ((System.Drawing.Image)(resources.GetObject("scXZ.Image")));
                this.scXZ.InitialImage = ((System.Drawing.Image)(resources.GetObject("scXZ.InitialImage")));
                this.scXZ.Location = new System.Drawing.Point(0, 0);
                this.scXZ.Name = "scXZ";
                this.scXZ.Size = new System.Drawing.Size(334, 195);
                this.scXZ.TabIndex = 0;
                this.scXZ.TabStop = false;
                this.scXZ.MouseLeave += new System.EventHandler(this.scXZ_MouseLeave);
                this.scXZ.MouseMove += new System.Windows.Forms.MouseEventHandler(this.scXZ_MouseMove);
                this.scXZ.Resize += new System.EventHandler(this.scXZ_Resize);
                this.scXZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scXZ_MouseDown);
                this.scXZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.scXZ_MouseUp);
                // 
                // splitHorizontalRight
                // 
                this.splitHorizontalRight.BackColor = System.Drawing.Color.Transparent;
                this.splitHorizontalRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.splitHorizontalRight.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitHorizontalRight.Location = new System.Drawing.Point(0, 0);
                this.splitHorizontalRight.Name = "splitHorizontalRight";
                this.splitHorizontalRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
                // 
                // splitHorizontalRight.Panel1
                // 
                this.splitHorizontalRight.Panel1.Controls.Add(this.scZY);
                // 
                // splitHorizontalRight.Panel2
                // 
                this.splitHorizontalRight.Panel2.Controls.Add(this.glView1);
                this.splitHorizontalRight.Size = new System.Drawing.Size(296, 398);
                this.splitHorizontalRight.SplitterDistance = 196;
                this.splitHorizontalRight.TabIndex = 0;
                this.splitHorizontalRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitHorizontalRight_SplitterMoved);
                // 
                // scZY
                // 
                this.scZY.AccessibleDescription = "SdlDotNet SurfaceControl";
                this.scZY.AccessibleName = "SurfaceControl";
                this.scZY.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
                this.scZY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                this.scZY.Dock = System.Windows.Forms.DockStyle.Fill;
                this.scZY.Image = ((System.Drawing.Image)(resources.GetObject("scZY.Image")));
                this.scZY.InitialImage = ((System.Drawing.Image)(resources.GetObject("scZY.InitialImage")));
                this.scZY.Location = new System.Drawing.Point(0, 0);
                this.scZY.Name = "scZY";
                this.scZY.Size = new System.Drawing.Size(292, 192);
                this.scZY.TabIndex = 0;
                this.scZY.TabStop = false;
                this.scZY.MouseLeave += new System.EventHandler(this.scZY_MouseLeave);
                this.scZY.MouseMove += new System.Windows.Forms.MouseEventHandler(this.scZY_MouseMove);
                this.scZY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scZY_MouseDown);
                this.scZY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.scZY_MouseUp);
                this.scZY.SizeChanged += new System.EventHandler(this.scZY_Resize);
                // 
                // glView1
                // 
                this.glView1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.glView1.ImeMode = System.Windows.Forms.ImeMode.On;
                this.glView1.Location = new System.Drawing.Point(0, 0);
                this.glView1.Name = "glView1";
                this.glView1.Size = new System.Drawing.Size(292, 194);
                this.glView1.TabIndex = 0;
                this.glView1.Text = "glView1";
                this.glView1.MouseLeave += new System.EventHandler(this.glView1_MouseLeave);
                this.glView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glView1_MouseMove);
                // 
                // cbItemSelector
                // 
                this.cbItemSelector.Dock = System.Windows.Forms.DockStyle.Top;
                this.cbItemSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.cbItemSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.cbItemSelector.FormattingEnabled = true;
                this.cbItemSelector.Items.AddRange(new object[] {
            "Choose item ..."});
                this.cbItemSelector.Location = new System.Drawing.Point(0, 4);
                this.cbItemSelector.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
                this.cbItemSelector.Name = "cbItemSelector";
                this.cbItemSelector.Size = new System.Drawing.Size(144, 21);
                this.cbItemSelector.TabIndex = 1;
                this.cbItemSelector.SelectedIndexChanged += new System.EventHandler(this.cbItemSelector_SelectedIndexChanged);
                // 
                // pgProperties
                // 
                this.pgProperties.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.pgProperties.Location = new System.Drawing.Point(0, 27);
                this.pgProperties.Name = "pgProperties";
                this.pgProperties.Size = new System.Drawing.Size(144, 371);
                this.pgProperties.TabIndex = 0;
                this.pgProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgProperties_PropertyValueChanged);
                // 
                // toolStripMenuItem3
                // 
                this.toolStripMenuItem3.Name = "toolStripMenuItem3";
                this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
                // 
                // toolStripMenuItem8
                // 
                this.toolStripMenuItem8.Name = "toolStripMenuItem8";
                this.toolStripMenuItem8.Size = new System.Drawing.Size(207, 6);
                // 
                // materialEditorToolStripMenuItem
                // 
                this.materialEditorToolStripMenuItem.Name = "materialEditorToolStripMenuItem";
                this.materialEditorToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
                this.materialEditorToolStripMenuItem.Text = "Material editor ...";
                this.materialEditorToolStripMenuItem.Click += new System.EventHandler(this.materialEditorToolStripMenuItem_Click);
                // 
                // MapEditor
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(790, 469);
                this.Controls.Add(this.splitContainer1);
                this.Controls.Add(this.ToolStrip1);
                this.Controls.Add(this.StatusStrip1);
                this.Controls.Add(this.MenuStrip1);
                this.MainMenuStrip = this.MenuStrip1;
                this.Name = "MapEditor";
                this.Text = "Form1";
                this.Load += new System.EventHandler(this.MapEditor_Load);
                this.MenuStrip1.ResumeLayout(false);
                this.MenuStrip1.PerformLayout();
                this.StatusStrip1.ResumeLayout(false);
                this.StatusStrip1.PerformLayout();
                this.ToolStrip1.ResumeLayout(false);
                this.ToolStrip1.PerformLayout();
                this.contextMenuStrip1.ResumeLayout(false);
                this.splitContainer1.Panel1.ResumeLayout(false);
                this.splitContainer1.Panel2.ResumeLayout(false);
                this.splitContainer1.ResumeLayout(false);
                this.splitVertical.Panel1.ResumeLayout(false);
                this.splitVertical.Panel2.ResumeLayout(false);
                this.splitVertical.ResumeLayout(false);
                this.splitHorizontalLeft.Panel1.ResumeLayout(false);
                this.splitHorizontalLeft.Panel2.ResumeLayout(false);
                this.splitHorizontalLeft.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.scXY)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.scXZ)).EndInit();
                this.splitHorizontalRight.Panel1.ResumeLayout(false);
                this.splitHorizontalRight.Panel2.ResumeLayout(false);
                this.splitHorizontalRight.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.scZY)).EndInit();
                this.ResumeLayout(false);
                this.PerformLayout();

		}
		internal System.Windows.Forms.MenuStrip MenuStrip1;
		internal System.Windows.Forms.StatusStrip StatusStrip1;
		internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripButton tsbSelectMove;
		internal System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem SaveasToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem BlablaToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem2;
		internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private ToolStripMenuItem settiToolStripMenuItem;
        private ToolStripMenuItem gridToolStripMenuItem;
        private ToolStripMenuItem zoomInToolStripMenuItem;
        private ToolStripMenuItem zoomOutToolStripMenuItem;
        private ToolStripButton tsbCreate;
        private ToolStripMenuItem tsmSelectMove;
        private ToolStripMenuItem tsmCreate;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripStatusLabel tslTool;
        private ToolStripStatusLabel tslView;
        private ToolStripStatusLabel tslCoordinates;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem deleteSelectedObjectToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private ToolStripDropDownButton tsdEntities;
        private ToolStripMenuItem testToolStripMenuItem;
        private SplitContainer splitContainer1;
        internal SplitContainer splitVertical;
        internal SplitContainer splitHorizontalLeft;
        internal SdlDotNet.Windows.SurfaceControl scXY;
        internal SdlDotNet.Windows.SurfaceControl scXZ;
        internal SplitContainer splitHorizontalRight;
        internal SdlDotNet.Windows.SurfaceControl scZY;
        private PropertyGrid pgProperties;
        private ComboBox cbItemSelector;
        private ToolStripButton tsbLayer1;
        private ToolStripButton tsbLayer2;
        private ToolStripButton tsbLayer3;
        private ToolStripButton tsbLayer4;
        private ToolStripButton tsbLayer5;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripLabel toolStripLabel1;
        private ToolStripMenuItem layersToolStripMenuItem;
        private ToolStripMenuItem miLayer1;
        private ToolStripMenuItem miLayer2;
        private ToolStripMenuItem miLayer3;
        private ToolStripMenuItem miLayer4;
        private ToolStripMenuItem miLayer5;
        private Map_Editor.View3D glView1;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem materialEditorToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
		
	}
	
}
