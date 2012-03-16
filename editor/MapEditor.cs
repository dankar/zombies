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
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualBasic.CompilerServices;


namespace Map_Editor
{
	public partial class MapEditor
	{
        private Thread tSDLRun;
        // Constructor
		public MapEditor()
		{
			lstBlocks = new List<Block>();
			lstItems = new List<Item>();

			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
		
        // Some crap added by VB to C# converter
		#region Default Instance
		
		    private static MapEditor defaultInstance;
    		
		    /// <summary>
		    /// Added by the VB.Net to C# Converter to support default instance behavour in C#
		    /// </summary>
		    public static MapEditor Default
		    {
			    get
			    {
				    if (defaultInstance == null)
				    {
					    defaultInstance = new MapEditor();
					    defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
				    }
    				
				    return defaultInstance;
			    }
		    }
    		
		    static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		    {
			    defaultInstance = null;
		    }
    		
		#endregion
		
        #region Public Variable Declarations

		    public Settings appSettings = new Settings();
            public EntitiesReader entReader;
            public LevelInformation levelInfo = new LevelInformation();
            public List<Block> lstBlocks;
            public List<Item> lstItems;
            public List<Entity> lstEntities;
            public List<Material> lstMaterial;
            public Exporter levelExporter;

            public bool[] layerStatus = new bool[5]{true,true,true,true,true};

		    public string sFileName;
		    public string sFilePath;
		    public bool bFileIsSaved;
            public bool bFileIsChanged;
    		

            public int iBlockCounter = 1;
            public int iItemCounter = 1;

		    public View2D viewXY;
		    public View2D viewZY;
		    public View2D viewXZ;
    		
            /* iToolID indicates which tool is currently in use
             * 0 = Select & Move
             * 1 = Modifier
             * 2 = Create block
             * 3 = Create item
             */
            public int iToolID = 0;

            public string iToolItemID = "";
        
        #endregion

        #region Private Functions
		    private void FileNew()
		    {
                bool drCancel = false;
                if (bFileIsChanged)
                {
                    DialogResult dr = MessageBox.Show("File is changed, do you want to save?", "File is changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        this.FileSave();
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        drCancel = true;
                    }
                }
                if (!drCancel)
                {
                    lstBlocks.Clear();
                    lstItems.Clear();
                    levelInfo.Clear();
                    lstMaterial.Clear();

                    // Read standard materials
                    MaterialReader matReader = new MaterialReader();
                    foreach(Material m in matReader.GetMaterialList()) {
                        this.lstMaterial.Add(m);
                    }

                    sFileName = "untitled";
                    sFilePath = "";
                    bFileIsSaved = false;
                    bFileIsChanged = false;

                    this.RedrawBlocks();
                    this.RedrawItems();
                    this.RedrawTemp();

                    this.UpdateTitle();
                }
                this.UpdateProperties();
		    }
            private bool FileSave()
            {
                if (this.sFilePath == "")
                {
                    this.FileSaveAs();
                }
                try
                {
                    Stream stream = File.Open(this.sFilePath, FileMode.Create);
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, lstBlocks);
                    bFormatter.Serialize(stream, lstItems);
                    bFormatter.Serialize(stream, levelInfo);
                    bFormatter.Serialize(stream, lstMaterial);
                    stream.Close();
                    this.bFileIsSaved = true;
                    this.bFileIsChanged = false;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "The file could not be saved, error: " + Environment.NewLine + ex.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            private void FileSaveAs()
            {
                SaveFileDialog dlgSave = new SaveFileDialog();
                dlgSave.Filter = "GPME map file (*.gpm)|*.gpm|All Files (*.*)|*.*";
                dlgSave.FilterIndex = 1;
                dlgSave.RestoreDirectory = true;
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    this.sFilePath = dlgSave.FileName;
                    this.sFileName = Path.GetFileName(dlgSave.FileName);
                    this.FileSave();
                    this.RedrawAll();
                } 
            }
            private void FileExportXml()
            {
                SaveFileDialog dlgSave = new SaveFileDialog();
                dlgSave.Filter = "Zombies XML map (*.zml)|*.zml|All Files (*.*)|*.*";
                dlgSave.FilterIndex = 1;
                dlgSave.RestoreDirectory = true;
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    levelExporter.ExportToXML(dlgSave.FileName);
                }
            }
            private void FileOpen()
            {
                bool drCancel = false;
                if (bFileIsChanged)
                {
                    DialogResult dr = MessageBox.Show("File is changed, do you want to save?", "File is changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        this.FileSave();
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        drCancel = true;
                    }
                }
                if (!drCancel)
                {
                    OpenFileDialog dlgOpen = new OpenFileDialog();
                    dlgOpen.Filter = "GPME map file (*.gpm)|*.gpm|All Files (*.*)|*.*";
                    dlgOpen.FilterIndex = 1;
                    dlgOpen.RestoreDirectory = true;
                    if (dlgOpen.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (Stream stream = File.Open(dlgOpen.FileName, FileMode.Open))
                            {
                                BinaryFormatter bin = new BinaryFormatter();

                                lstBlocks.Clear();
                                lstItems.Clear();
                                levelInfo.Clear();
                                lstMaterial.Clear();


                                var tlstBlocks = (List<Block>)bin.Deserialize(stream);
                                var tlstItems = (List<Item>)bin.Deserialize(stream);
                                this.levelInfo = (LevelInformation)bin.Deserialize(stream);
                                var tlstMaterial = (List<Material>)bin.Deserialize(stream);

                                foreach (Block b in tlstBlocks)
                                {
                                    lstBlocks.Add(b);
                                }

                                foreach (Item i in tlstItems)
                                {
                                    lstItems.Add(i);
                                }

                                foreach (Material i in tlstMaterial)
                                {
                                    lstMaterial.Add(i);
                                }

                                tlstBlocks.Clear();
                                tlstBlocks = null;
                                tlstItems.Clear();
                                tlstItems = null;
                                tlstMaterial.Clear();
                                tlstMaterial = null;

                                this.sFilePath = dlgOpen.FileName;
                                this.sFileName = Path.GetFileName(dlgOpen.FileName);
                                this.bFileIsSaved = true;
                                this.bFileIsChanged = false;

                                this.RedrawAll();

                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "The file could not be opened, error: " + Environment.NewLine + ex.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                this.UpdateProperties();
            }
            private void ShowSplash()
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(ShowSplash));
                }
                else
                {
                    StartSplash.Default.Owner = this;
                }
                StartSplash.Default.ShowDialog();
                StartSplash.Default.Activate();
                StartSplash.Default.TopMost = true;
            }
            private void RedrawBlocks()
            {
                viewXY.RedrawBlocks();
                viewXZ.RedrawBlocks();
                viewZY.RedrawBlocks();
            }
            private void RedrawTemp()
            {
                viewXY.RedrawTemp();
                viewXZ.RedrawTemp();
                viewZY.RedrawTemp();
            }
            private void RedrawItems()
            {
                viewXY.RedrawItems();
                viewXZ.RedrawItems();
                viewZY.RedrawItems();
            }
            private void RedrawAll()
            {
                this.RedrawBlocks();
                this.RedrawTemp();
                this.RedrawItems();
                this.UpdateTitle();
            }
            private void DeselectAll()
            {
                foreach (Block b in lstBlocks) b.selected = false;
                foreach (Item i in lstItems) i.selected = false;
                viewXY.block_id = 0;
                viewXZ.block_id = 0;
                viewZY.block_id = 0;
            }
            private void UpdateTitle()
            {
                string sUnsaved = "";
                if (this.bFileIsChanged) sUnsaved = " *";
                this.Text = "GMPE! - " + this.sFileName + sUnsaved;
            }
            private void UpdateProperties()
            {
                bool bf = false;
                foreach (Block b in lstBlocks) if (b.selected) { pgProperties.SelectedObject = b; bf = true; break; }
                if (!bf) foreach (Item i in lstItems) if (i.selected) { pgProperties.SelectedObject = i; bf = true; break; }
                if (!bf) pgProperties.SelectedObject = this.levelInfo;
                
                cbItemSelector.Items.Clear();
                
                foreach (Item i in lstItems) cbItemSelector.Items.Add(i.Name);
                foreach (Block b in lstBlocks) cbItemSelector.Items.Add(b.Name);

                cbItemSelector.SelectedItem = 0;

            }
            private void UpdateEntitiesMenu()
            {
                int i = 0;

                foreach (Entity e in this.lstEntities)
                {

                    EntToolStripMenuItem m = new EntToolStripMenuItem();
                    m.Name = "tsmEntity" + i.ToString();
                    m.Size = new System.Drawing.Size(152, 22);
                    m.Text = e.DisplayName;
                    m.entityIndex = lstEntities.IndexOf(e);

                    m.Click += new EventHandler( delegate(System.Object o, EventArgs ea)
                    { EntToolStripMenuItem o2 = (EntToolStripMenuItem)o; this.iToolID = 4; iToolItemID = lstEntities.ElementAt(o2.entityIndex).Type; });
                    i++;
                    tsdEntities.DropDownItems.Add(m);
                } 
            }
            private void SelectObjectFromName(string name)
            {
                bool bf = false;
                foreach (Item i in lstItems)
                {
                    if (i.Name == name)
                    {
                        this.DeselectAll();
                        int tmpI = lstItems.IndexOf(i);
                        i.selected = true;
                        viewXY.item_id = tmpI;
                        viewXZ.item_id = tmpI;
                        viewZY.item_id = tmpI;
                        bf = true;
                        break;
                    }
                }
                if (!bf) foreach (Block b in lstBlocks)
                {
                    if (b.Name == name)
                    {
                        this.DeselectAll();
                        int tmpI = lstBlocks.IndexOf(b);
                        b.selected = true;
                        viewXY.block_id = tmpI;
                        viewXZ.block_id = tmpI;
                        viewZY.block_id = tmpI;
                        bf = true;
                        break;
                    }
                }
                RedrawAll();
                UpdateProperties();
            }
            private void UpdateLayer(int layer)
            {
                this.layerStatus[layer] = !this.layerStatus[layer];
                if(layer == 0){
                    this.tsbLayer1.Checked = !this.layerStatus[layer];
                    this.miLayer1.Checked = !this.layerStatus[layer];
                }else if(layer ==1){
                    this.tsbLayer2.Checked = !this.layerStatus[layer];
                    this.miLayer2.Checked = !this.layerStatus[layer];
                }else if(layer ==2){
                    this.tsbLayer3.Checked = !this.layerStatus[layer];
                    this.miLayer3.Checked = !this.layerStatus[layer];
                }else if(layer ==3){
                    this.tsbLayer4.Checked = !this.layerStatus[layer];
                    this.miLayer4.Checked = !this.layerStatus[layer];
                }else if(layer ==4){
                    this.tsbLayer5.Checked = !this.layerStatus[layer];
                    this.miLayer5.Checked = !this.layerStatus[layer];
                }
                this.RedrawAll();
            }
        #endregion

        #region Events

            public void tsmEntity_Click(int index)
            {
                MessageBox.Show(index.ToString());
            }
		    public void MapEditor_Load(System.Object sender, System.EventArgs e)
		    {

                //- Show splash in separate thread
                Thread tStartSplash;
                tStartSplash = new Thread(new System.Threading.ThreadStart(this.ShowSplash));
                tStartSplash.Start();


			    //- Load Application Settings
			    appSettings.LoadSettings();
                this.entReader = new EntitiesReader();
                this.lstEntities = entReader.GetEntityList();
                this.UpdateEntitiesMenu();
                MaterialReader matReader = new MaterialReader();
                this.lstMaterial = matReader.GetMaterialList();

                // - Initialize level exporter
                levelExporter = new Exporter(lstBlocks, lstItems, lstEntities, lstMaterial, levelInfo);

			    //- Set application title
			    this.Text = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Title + " - " + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version.Major + "." + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version.Minor;

                //- Connect SDL Surfaces to the classes
                viewXY = new View2D(scXY, "XY", lstBlocks, lstItems, appSettings.GridSize,layerStatus);
                viewZY = new View2D(scZY, "ZY", lstBlocks, lstItems, appSettings.GridSize,layerStatus);
                viewXZ = new View2D(scXZ, "XZ", lstBlocks, lstItems, appSettings.GridSize,layerStatus);

                this.glView1.Connect(lstBlocks, lstItems, appSettings.GridSize,lstMaterial);

			    //- Initialize SDL
                SdlDotNet.Core.Events.Fps = appSettings.FpsLimit;

			    tSDLRun = new Thread(new System.Threading.ThreadStart(SdlDotNet.Core.Events.Run));
			    tSDLRun.IsBackground = true;
			    tSDLRun.Name = "SDL.NET";
			    tSDLRun.Priority = ThreadPriority.Normal;
			    tSDLRun.Start();

                //- Initialize new file
                FileNew();

                tsbSelectMove_Click(this,null);

		    }

            private void Application_Idle(Object sender, EventArgs e)
            {
                this.glView1.Refresh();
            }
		    public void ExitToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		    {
			    ProjectData.EndApp();
		    }
		    public void splitHorizontalRight_SplitterMoved(System.Object sender, System.Windows.Forms.SplitterEventArgs e)
		    {
			    splitHorizontalLeft.SplitterDistance = splitHorizontalRight.SplitterDistance;
		    }
		    public void splitHorizontalLeft_SplitterMoved(System.Object sender, System.Windows.Forms.SplitterEventArgs e)
		    {
			    splitHorizontalRight.SplitterDistance = splitHorizontalLeft.SplitterDistance;
		    }

            private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (appSettings.GridSize <= 8)
                {
                    appSettings.GridSize += 1;
                    viewXY.SetGridSize(appSettings.GridSize);
                    viewXZ.SetGridSize(appSettings.GridSize);
                    viewZY.SetGridSize(appSettings.GridSize);
                }
                RedrawAll();
            }
            private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (appSettings.GridSize >= 2)
                {
                    appSettings.GridSize -= 1;
                    viewXY.SetGridSize(appSettings.GridSize);
                    viewXZ.SetGridSize(appSettings.GridSize);
                    viewZY.SetGridSize(appSettings.GridSize);
                }
                RedrawAll();
            }
            private void NewToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.FileNew();
            }

            private void scXZ_Resize(object sender, EventArgs e)
            {
                viewXZ.Resize();
            }
            private void scXY_Resize(object sender, EventArgs e)
            {
                viewXY.Resize();
            }
            private void scZY_Resize(object sender, EventArgs e)
            {
                viewZY.Resize();
            }

            private void scXY_MouseDown(object sender, MouseEventArgs e)
            {
                if(e.Button==MouseButtons.Right)
                {
                    viewXY.isPanning = true;
                    viewXY.last_x = e.X;
                    viewXY.last_y = e.Y;
                }
                if (e.Button == MouseButtons.Left && this.iToolID == 2) // LB & Tool2 = Create Box
                {
                    viewXY.original_x = viewXY.GetGridX(e.X);
                    viewXY.original_y = viewXY.GetGridY(e.Y);
                    viewXY.current_x = viewXY.GetGridX(e.X);
                    viewXY.current_y = viewXY.GetGridY(e.Y);
                    viewXY.isDrawingBlock = true;
                }
                if (e.Button == MouseButtons.Left && (this.iToolID == 0 || this.iToolID == 1)) // LB & Tool0 = Select And move
                {
                    int x = viewXY.GetGridX(e.X);
                    int y = viewXY.GetGridY(e.Y);

                    bool bItemSelected = false;
                    /* Firstly, check if a item is under the mousepointer */
                    for (int i = 0; i < lstItems.Count(); i++)
                    {
                        if (x == lstItems.ElementAt(i).Position.X &&
                            y == lstItems.ElementAt(i).Position.Y)
                        {
                            this.DeselectAll();
                            viewXY.isMovingItem = true;
                            lstItems.ElementAt(i).selected = true;
                            viewXY.item_id = i;
                            viewXZ.item_id = i;
                            viewZY.item_id = i;
                            viewXY.original_x = viewXY.GetGridX(e.X) - lstItems.ElementAt(i).Position.X;
                            viewXY.original_y = viewXY.GetGridY(e.Y) - lstItems.ElementAt(i).Position.Y;
                            bItemSelected = true;
                            break;
                        }
                        else
                        {
                            lstItems.ElementAt(i).selected = false;
                        }
                    }

                    /* Secondly, check if a block is under the mousepointer */
                    if (!bItemSelected && lstBlocks.Count() > 0 && (x == lstBlocks.ElementAt(viewXY.block_id).Position.X) &&
                            ((lstBlocks.ElementAt(viewXY.block_id).Size.Y >= 0 && (y >= lstBlocks.ElementAt(viewXY.block_id).Position.Y && y <= lstBlocks.ElementAt(viewXY.block_id).Position.Y + lstBlocks.ElementAt(viewXY.block_id).Size.Y)) ||
                            (lstBlocks.ElementAt(viewXY.block_id).Size.Y < 0 && (y <= lstBlocks.ElementAt(viewXY.block_id).Position.Y && y >= lstBlocks.ElementAt(viewXY.block_id).Position.Y + lstBlocks.ElementAt(viewXY.block_id).Size.Y))) &&
                            (lstBlocks.ElementAt(viewXY.block_id).selected == true) && layerStatus[(int)lstBlocks.ElementAt(viewXY.block_id)._layer])
                        {
                            viewXY.isResizingBlock = true;
                            viewXY.resizePosX = true;
                            viewXY.resizeSizeX = false;
                            viewXY.resizePosY = false;
                            viewXY.resizeSizeY = false;

                            viewXY.original_x = viewXY.GetGridX(e.X) - lstBlocks.ElementAt(viewXY.block_id).Position.X;
                            viewXY.original_y = viewXY.GetGridY(e.Y) - lstBlocks.ElementAt(viewXY.block_id).Position.Y;
                        }
                    else if (!bItemSelected && lstBlocks.Count() > 0 && (x == lstBlocks.ElementAt(viewXY.block_id).Position.X + lstBlocks.ElementAt(viewXY.block_id).Size.X) &&
                            ((lstBlocks.ElementAt(viewXY.block_id).Size.Y >= 0 && (y >= lstBlocks.ElementAt(viewXY.block_id).Position.Y && y <= lstBlocks.ElementAt(viewXY.block_id).Position.Y + lstBlocks.ElementAt(viewXY.block_id).Size.Y)) ||
                            (lstBlocks.ElementAt(viewXY.block_id).Size.Y < 0 && (y <= lstBlocks.ElementAt(viewXY.block_id).Position.Y && y >= lstBlocks.ElementAt(viewXY.block_id).Position.Y + lstBlocks.ElementAt(viewXY.block_id).Size.Y))) &&
                            (lstBlocks.ElementAt(viewXY.block_id).selected == true) && layerStatus[(int)lstBlocks.ElementAt(viewXY.block_id)._layer])
                        {
                            viewXY.isResizingBlock = true;
                            viewXY.resizePosX = false;
                            viewXY.resizeSizeX = true;
                            viewXY.resizePosY = false;
                            viewXY.resizeSizeY = false;

                            viewXY.original_x = viewXY.GetGridX(e.X) - lstBlocks.ElementAt(viewXY.block_id).Size.X;
                            viewXY.original_y = viewXY.GetGridY(e.Y) - lstBlocks.ElementAt(viewXY.block_id).Size.Y;
                        }
                else if (!bItemSelected && lstBlocks.Count() > 0 && (y == lstBlocks.ElementAt(viewXY.block_id).Position.Y) &&
                            ((lstBlocks.ElementAt(viewXY.block_id).Size.X >= 0 && (x >= lstBlocks.ElementAt(viewXY.block_id).Position.X && x <= lstBlocks.ElementAt(viewXY.block_id).Position.X + lstBlocks.ElementAt(viewXY.block_id).Size.X)) ||
                            (lstBlocks.ElementAt(viewXY.block_id).Size.X < 0 && (x <= lstBlocks.ElementAt(viewXY.block_id).Position.X && x >= lstBlocks.ElementAt(viewXY.block_id).Position.X + lstBlocks.ElementAt(viewXY.block_id).Size.X))) &&
                            (lstBlocks.ElementAt(viewXY.block_id).selected == true) && layerStatus[(int)lstBlocks.ElementAt(viewXY.block_id)._layer])
                        {
                            viewXY.isResizingBlock = true;
                            viewXY.resizePosX = false;
                            viewXY.resizeSizeX = false;
                            viewXY.resizePosY = true;
                            viewXY.resizeSizeY = false;

                            viewXY.original_x = viewXY.GetGridX(e.X) - lstBlocks.ElementAt(viewXY.block_id).Position.X;
                            viewXY.original_y = viewXY.GetGridY(e.Y) - lstBlocks.ElementAt(viewXY.block_id).Position.Y;
                        }
                else if (!bItemSelected && lstBlocks.Count() > 0 && (y == lstBlocks.ElementAt(viewXY.block_id).Position.Y + lstBlocks.ElementAt(viewXY.block_id).Size.Y) &&
                          ((lstBlocks.ElementAt(viewXY.block_id).Size.X >= 0 && (x >= lstBlocks.ElementAt(viewXY.block_id).Position.X && x <= lstBlocks.ElementAt(viewXY.block_id).Position.X + lstBlocks.ElementAt(viewXY.block_id).Size.X)) ||
                          (lstBlocks.ElementAt(viewXY.block_id).Size.X < 0 && (x <= lstBlocks.ElementAt(viewXY.block_id).Position.X && x >= lstBlocks.ElementAt(viewXY.block_id).Position.X + lstBlocks.ElementAt(viewXY.block_id).Size.X))) &&
                            (lstBlocks.ElementAt(viewXY.block_id).selected == true) && layerStatus[(int)lstBlocks.ElementAt(viewXY.block_id)._layer])
                        {
                            viewXY.isResizingBlock = true;
                            viewXY.resizePosX = false;
                            viewXY.resizeSizeX = false;
                            viewXY.resizePosY = false;
                            viewXY.resizeSizeY = true;

                            viewXY.original_x = viewXY.GetGridX(e.X) - lstBlocks.ElementAt(viewXY.block_id).Size.X;
                            viewXY.original_y = viewXY.GetGridY(e.Y) - lstBlocks.ElementAt(viewXY.block_id).Size.Y;
                        
                    } else if(!bItemSelected){
                        bool bOneSelected = false;
                        if(lstBlocks.Count() > 0 && ((lstBlocks.ElementAt(viewXY.block_id).Size.X>=0&&(x >= lstBlocks.ElementAt(viewXY.block_id).Position.X&&x <= lstBlocks.ElementAt(viewXY.block_id).Position.X + lstBlocks.ElementAt(viewXY.block_id).Size.X)) ||
                                    (lstBlocks.ElementAt(viewXY.block_id).Size.X<0&&(x <= lstBlocks.ElementAt(viewXY.block_id).Position.X&&x >= lstBlocks.ElementAt(viewXY.block_id).Position.X + lstBlocks.ElementAt(viewXY.block_id).Size.X)))&&
                                    ((lstBlocks.ElementAt(viewXY.block_id).Size.Y>=0&&(y >= lstBlocks.ElementAt(viewXY.block_id).Position.Y&&y <= lstBlocks.ElementAt(viewXY.block_id).Position.Y + lstBlocks.ElementAt(viewXY.block_id).Size.Y)) ||
                                    (lstBlocks.ElementAt(viewXY.block_id).Size.Y < 0 && (y <= lstBlocks.ElementAt(viewXY.block_id).Position.Y && y >= lstBlocks.ElementAt(viewXY.block_id).Position.Y + lstBlocks.ElementAt(viewXY.block_id).Size.Y))) && lstBlocks.ElementAt(viewXY.block_id).selected && layerStatus[(int)lstBlocks.ElementAt(viewXY.block_id)._layer])
                        {
                            viewXY.isMovingBlock = true;
                            viewXY.original_x = viewXY.GetGridX(e.X) - lstBlocks.ElementAt(viewXY.block_id).Position.X;
                            viewXY.original_y = viewXY.GetGridY(e.Y) - lstBlocks.ElementAt(viewXY.block_id).Position.Y;
                            bOneSelected = true;
                        }else{
                            for (int i = 0; i < lstBlocks.Count();i++ )
                            {
                                if (((lstBlocks.ElementAt(i).Size.X>=0&&(x >= lstBlocks.ElementAt(i).Position.X&&x <= lstBlocks.ElementAt(i).Position.X + lstBlocks.ElementAt(i).Size.X)) ||
                                    (lstBlocks.ElementAt(i).Size.X<0&&(x <= lstBlocks.ElementAt(i).Position.X&&x >= lstBlocks.ElementAt(i).Position.X + lstBlocks.ElementAt(i).Size.X)))&&
                                    ((lstBlocks.ElementAt(i).Size.Y>=0&&(y >= lstBlocks.ElementAt(i).Position.Y&&y <= lstBlocks.ElementAt(i).Position.Y + lstBlocks.ElementAt(i).Size.Y)) ||
                                    (lstBlocks.ElementAt(i).Size.Y < 0 && (y <= lstBlocks.ElementAt(i).Position.Y && y >= lstBlocks.ElementAt(i).Position.Y + lstBlocks.ElementAt(i).Size.Y))) && (!lstBlocks.ElementAt(i).selected || viewXY.block_id == i) && !bOneSelected && layerStatus[(int)lstBlocks.ElementAt(i)._layer]) 
                                {
                                        viewXY.isMovingBlock = true;
                                        lstBlocks.ElementAt(i).selected = true;
                                        viewXY.block_id = i;
                                        viewXZ.block_id = i;
                                        viewZY.block_id = i;
                                        viewXY.original_x = viewXY.GetGridX(e.X) - lstBlocks.ElementAt(i).Position.X;
                                        viewXY.original_y = viewXY.GetGridY(e.Y) - lstBlocks.ElementAt(i).Position.Y;
                                        bOneSelected = true;
                                } else {
                                    lstBlocks.ElementAt(i).selected = false;
                                }
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    bool foundSelected = false;
                    bool selectedNew = false;
                    int foundfirst = 0;
                    bool hasfoundfirst = false;
                    int x = viewXY.GetGridX(e.X);
                    int y = viewXY.GetGridY(e.Y);
                    foreach (Block b in lstBlocks)
                    {
                        if (((b.Size.X >= 0 && (x >= b.Position.X && x <= b.Position.X + b.Size.X)) ||
                                (b.Size.X < 0 && (x <= b.Position.X && x >= b.Position.X + b.Size.X))) &&
                                ((b.Size.Y >= 0 && (y >= b.Position.Y && y <= b.Position.Y + b.Size.Y)) ||
                                (b.Size.Y < 0 && (y <= b.Position.Y && y >= b.Position.Y + b.Size.Y))) && (!b.selected) && foundSelected && !selectedNew && layerStatus[(int)b._layer])
                        {
                            this.DeselectAll();
                            b.selected = true;
                            viewXY.block_id = lstBlocks.IndexOf(b);
                            viewXZ.block_id = lstBlocks.IndexOf(b);
                            viewZY.block_id = lstBlocks.IndexOf(b);
                            selectedNew = true;
                            break;
                        }
                        else if (((b.Size.X >= 0 && (x >= b.Position.X && x <= b.Position.X + b.Size.X)) ||
                                (b.Size.X < 0 && (x <= b.Position.X && x >= b.Position.X + b.Size.X))) &&
                                ((b.Size.Y >= 0 && (y >= b.Position.Y && y <= b.Position.Y + b.Size.Y)) ||
                                (b.Size.Y < 0 && (y <= b.Position.Y && y >= b.Position.Y + b.Size.Y))) && (b.selected) && !selectedNew && layerStatus[(int)b._layer])
                        {
                            foundSelected = true;
                        }
                        else if (((b.Size.X >= 0 && (x >= b.Position.X && x <= b.Position.X + b.Size.X)) ||
                                (b.Size.X < 0 && (x <= b.Position.X && x >= b.Position.X + b.Size.X))) &&
                                ((b.Size.Y >= 0 && (y >= b.Position.Y && y <= b.Position.Y + b.Size.Y)) ||
                                (b.Size.Y < 0 && (y <= b.Position.Y && y >= b.Position.Y + b.Size.Y))) && !hasfoundfirst && layerStatus[(int)b._layer])
                        {
                            foundfirst = lstBlocks.IndexOf(b);
                            hasfoundfirst = true;
                        }
                    }
                    if (foundSelected && !selectedNew && lstBlocks.Count() > 0)
                    {
                        this.DeselectAll();
                        lstBlocks.ElementAt(foundfirst).selected = true;
                        viewXY.block_id = foundfirst;
                        viewXZ.block_id = foundfirst;
                        viewZY.block_id = foundfirst;
                    }
                }
                /* Items */
                if (e.Button == MouseButtons.Left && iToolID == 4)
                {
                    lstItems.Add(new Item("XY", viewXY.GetGridX(e.X), viewXY.GetGridY(e.Y), this.iToolItemID.ToString(), "Item"+this.levelInfo.itemiterator));
                    this.levelInfo.itemiterator++;
                }
                this.RedrawItems();
                this.RedrawTemp();
                this.UpdateProperties();
            }
            private void scXY_MouseUp(object sender, MouseEventArgs e)
            {
                viewXY.isPanning = false;
                if (viewXY.isDrawingBlock)
                {
                    Block tmpBlock = new Block("XY", viewXY.original_x, viewXY.original_y, viewXY.current_x - viewXY.original_x, viewXY.current_y - viewXY.original_y, "Block" + this.levelInfo.blockiterator);
                    this.levelInfo.blockiterator++;
                    lstBlocks.Add(tmpBlock);
                    viewXY.isDrawingBlock = false;
                    this.bFileIsChanged = true;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                } else if (viewXY.isMovingBlock) 
                {
                    viewXY.isMovingBlock = false;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                else if (viewXY.isMovingItem)
                {
                    viewXY.isMovingItem = false;
                    this.RedrawItems();
                }
                else if (viewXY.isResizingBlock)
                {
                    viewXY.isResizingBlock = false;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                this.UpdateTitle();
            }
            private void scXY_MouseLeave(object sender, EventArgs e)
            {
                viewXY.isPanning = false;
                viewXY.isDrawingBlock = false;
                viewXY.isResizingBlock = false;
                viewXY.isMovingBlock = false;
                viewXY.isMovingItem = false;

                tslView.Text = "View: None";
                tslCoordinates.Text = "X: N/A Y:N/A";

                //this.RedrawTemp();
                //this.RedrawBlocks();
            }
            private void scXY_MouseMove(object sender, MouseEventArgs e)
            {
                if (viewXY.isPanning == true)
                {
                    viewXY.Scroll(viewXY.last_x - e.X, viewXY.last_y - e.Y);
                    viewXY.last_x = e.X;
                    viewXY.last_y = e.Y;
                } else if (viewXY.isDrawingBlock)
                {
                    viewXY.current_x = viewXY.GetGridX(e.X);
                    viewXY.current_y = viewXY.GetGridY(e.Y);
                    if (viewXY.current_x != viewXY.last_x || viewXY.current_y != viewXY.last_y)
                    {
                        viewXY.last_x = viewXY.GetGridX(e.X);
                        viewXY.last_y = viewXY.GetGridY(e.Y);
                        viewXY.RedrawTemp();
                    }
                } else if (viewXY.isMovingBlock) {
                    viewXY.current_x = viewXY.GetGridX(e.X);
                    viewXY.current_y = viewXY.GetGridY(e.Y);
                    if (viewXY.current_x != viewXY.last_x || viewXY.current_y != viewXY.last_y)
                    {
                        lstBlocks.ElementAt(viewXY.block_id).Position.X = viewXY.GetGridX(e.X) - viewXY.original_x;
                        lstBlocks.ElementAt(viewXY.block_id).Position.Y = viewXY.GetGridY(e.Y) - viewXY.original_y;

                        this.RedrawTemp();

                        this.bFileIsChanged = true;
                        
                        viewXY.last_x = viewXY.GetGridX(e.X);
                        viewXY.last_y = viewXY.GetGridY(e.Y);

                    }
                } else if (viewXY.isResizingBlock) {
                    viewXY.current_x = viewXY.GetGridX(e.X);
                    viewXY.current_y = viewXY.GetGridY(e.Y);
                    if (viewXY.current_x != viewXY.last_x || viewXY.current_y != viewXY.last_y)
                    {
                        if (viewXY.resizePosX)
                        {
                            int diff = lstBlocks.ElementAt(viewXY.block_id).Position.X - (viewXY.GetGridX(e.X) - viewXY.original_x);
                            lstBlocks.ElementAt(viewXY.block_id).Position.X = viewXY.GetGridX(e.X) - viewXY.original_x;
                            lstBlocks.ElementAt(viewXY.block_id).Size.X += diff;
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewXY.resizeSizeX)
                        {
                            lstBlocks.ElementAt(viewXY.block_id).Size.X = -(viewXY.original_x - viewXY.GetGridX(e.X));
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewXY.resizePosY)
                        {
                            int diff = lstBlocks.ElementAt(viewXY.block_id).Position.Y - (viewXY.GetGridY(e.Y) - viewXY.original_y);
                            lstBlocks.ElementAt(viewXY.block_id).Position.Y = viewXY.GetGridY(e.Y) - viewXY.original_y;
                            lstBlocks.ElementAt(viewXY.block_id).Size.Y += diff;
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewXY.resizeSizeY)
                        {
                            lstBlocks.ElementAt(viewXY.block_id).Size.Y = -(viewXY.original_y - viewXY.GetGridY(e.Y));
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        viewXY.last_x = viewXY.GetGridX(e.X);
                        viewXY.last_y = viewXY.GetGridY(e.Y);
                    }
                }
                else if (viewXY.isMovingItem)
                {
                    viewXY.current_x = viewXY.GetGridX(e.X);
                    viewXY.current_y = viewXY.GetGridY(e.Y);
                    if (viewXY.current_x != viewXY.last_x || viewXY.current_y != viewXY.last_y)
                    {
                        lstItems.ElementAt(viewXY.item_id).Position.X = viewXY.GetGridX(e.X) - viewXY.original_x;
                        lstItems.ElementAt(viewXY.item_id).Position.Y = viewXY.GetGridY(e.Y) - viewXY.original_y;

                        this.RedrawTemp();

                        this.bFileIsChanged = true;

                        viewXY.last_x = viewXY.GetGridX(e.X);
                        viewXY.last_y = viewXY.GetGridY(e.Y);

                    }
                }

                tslView.Text = "View: Front (XY)";

                int x = viewXY.GetGridX(e.X);
                int y = viewXY.GetGridY(e.Y);

                tslCoordinates.Text = "X:" + x.ToString() + " Y:" + y.ToString();

            }

            private void scZY_MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                {
                    viewZY.isPanning = true;
                    viewZY.last_x = e.X;
                    viewZY.last_y = e.Y;
                }
                if (e.Button == MouseButtons.Left && this.iToolID == 2) // LB & Tool2 = Create Box
                {
                    viewZY.original_x = viewZY.GetGridX(e.X);
                    viewZY.original_y = viewZY.GetGridY(e.Y);
                    viewZY.current_x = viewZY.GetGridX(e.X);
                    viewZY.current_y = viewZY.GetGridY(e.Y);
                    viewZY.isDrawingBlock = true;
                }
                if (e.Button == MouseButtons.Left && (this.iToolID == 0 || this.iToolID == 1)) // LB & Tool0 = Select And move
                {
                    int x = viewZY.GetGridX(e.X);
                    int y = viewZY.GetGridY(e.Y);

                    bool bItemSelected = false;
                    /* Firstly, check if a item is under the mousepointer */
                    for (int i = 0; i < lstItems.Count(); i++)
                    {
                        if (x == lstItems.ElementAt(i).Position.Z &&
                            y == lstItems.ElementAt(i).Position.Y)
                        {
                            this.DeselectAll();
                            viewZY.isMovingItem = true;
                            lstItems.ElementAt(i).selected = true;
                            viewXY.item_id = i;
                            viewXZ.item_id = i;
                            viewZY.item_id = i;
                            viewZY.original_x = viewZY.GetGridX(e.X) - lstItems.ElementAt(i).Position.Z;
                            viewZY.original_y = viewZY.GetGridY(e.Y) - lstItems.ElementAt(i).Position.Y;
                            bItemSelected = true;
                            break;
                        }
                        else
                        {
                            lstItems.ElementAt(i).selected = false;
                        }
                    }

                    /* Secondly, check if a block is under the mousepointer */
                    if (!bItemSelected && lstBlocks.Count() > 0 && (x == lstBlocks.ElementAt(viewZY.block_id).Position.Z) &&
                            ((lstBlocks.ElementAt(viewZY.block_id).Size.Y >= 0 && (y >= lstBlocks.ElementAt(viewZY.block_id).Position.Y && y <= lstBlocks.ElementAt(viewZY.block_id).Position.Y + lstBlocks.ElementAt(viewZY.block_id).Size.Y)) ||
                            (lstBlocks.ElementAt(viewZY.block_id).Size.Y < 0 && (y <= lstBlocks.ElementAt(viewZY.block_id).Position.Y && y >= lstBlocks.ElementAt(viewZY.block_id).Position.Y + lstBlocks.ElementAt(viewZY.block_id).Size.Y))) &&
                            (lstBlocks.ElementAt(viewZY.block_id).selected == true))
                    {
                        viewZY.isResizingBlock = true;
                        viewZY.resizePosX = true;
                        viewZY.resizeSizeX = false;
                        viewZY.resizePosY = false;
                        viewZY.resizeSizeY = false;

                        viewZY.original_x = viewZY.GetGridX(e.X) - lstBlocks.ElementAt(viewZY.block_id).Position.Z;
                        viewZY.original_y = viewZY.GetGridY(e.Y) - lstBlocks.ElementAt(viewZY.block_id).Position.Y;
                    }
                    else if (!bItemSelected && lstBlocks.Count() > 0 && (x == lstBlocks.ElementAt(viewZY.block_id).Position.Z + lstBlocks.ElementAt(viewZY.block_id).Size.Z) &&
                            ((lstBlocks.ElementAt(viewZY.block_id).Size.Y >= 0 && (y >= lstBlocks.ElementAt(viewZY.block_id).Position.Y && y <= lstBlocks.ElementAt(viewZY.block_id).Position.Y + lstBlocks.ElementAt(viewZY.block_id).Size.Y)) ||
                            (lstBlocks.ElementAt(viewZY.block_id).Size.Y < 0 && (y <= lstBlocks.ElementAt(viewZY.block_id).Position.Y && y >= lstBlocks.ElementAt(viewZY.block_id).Position.Y + lstBlocks.ElementAt(viewZY.block_id).Size.Y))) &&
                            (lstBlocks.ElementAt(viewZY.block_id).selected == true))
                    {
                        viewZY.isResizingBlock = true;
                        viewZY.resizePosX = false;
                        viewZY.resizeSizeX = true;
                        viewZY.resizePosY = false;
                        viewZY.resizeSizeY = false;

                        viewZY.original_x = viewZY.GetGridX(e.X) - lstBlocks.ElementAt(viewZY.block_id).Size.Z;
                        viewZY.original_y = viewZY.GetGridY(e.Y) - lstBlocks.ElementAt(viewZY.block_id).Size.Y;
                    }
                    else if (!bItemSelected && lstBlocks.Count() > 0 && (y == lstBlocks.ElementAt(viewZY.block_id).Position.Y) &&
                            ((lstBlocks.ElementAt(viewZY.block_id).Size.Z >= 0 && (x >= lstBlocks.ElementAt(viewZY.block_id).Position.Z && x <= lstBlocks.ElementAt(viewZY.block_id).Position.Z + lstBlocks.ElementAt(viewZY.block_id).Size.Z)) ||
                            (lstBlocks.ElementAt(viewZY.block_id).Size.Z < 0 && (x <= lstBlocks.ElementAt(viewZY.block_id).Position.Z && x >= lstBlocks.ElementAt(viewZY.block_id).Position.Z + lstBlocks.ElementAt(viewZY.block_id).Size.Z))) &&
                            (lstBlocks.ElementAt(viewZY.block_id).selected == true))
                    {
                        viewZY.isResizingBlock = true;
                        viewZY.resizePosX = false;
                        viewZY.resizeSizeX = false;
                        viewZY.resizePosY = true;
                        viewZY.resizeSizeY = false;

                        viewZY.original_x = viewZY.GetGridX(e.X) - lstBlocks.ElementAt(viewZY.block_id).Position.Z;
                        viewZY.original_y = viewZY.GetGridY(e.Y) - lstBlocks.ElementAt(viewZY.block_id).Position.Y;
                    }
                    else if (!bItemSelected && lstBlocks.Count() > 0 && (y == lstBlocks.ElementAt(viewZY.block_id).Position.Y + lstBlocks.ElementAt(viewZY.block_id).Size.Y) &&
                              ((lstBlocks.ElementAt(viewZY.block_id).Size.Z >= 0 && (x >= lstBlocks.ElementAt(viewZY.block_id).Position.Z && x <= lstBlocks.ElementAt(viewZY.block_id).Position.Z + lstBlocks.ElementAt(viewZY.block_id).Size.Z)) ||
                              (lstBlocks.ElementAt(viewZY.block_id).Size.Z < 0 && (x <= lstBlocks.ElementAt(viewZY.block_id).Position.Z && x >= lstBlocks.ElementAt(viewZY.block_id).Position.Z + lstBlocks.ElementAt(viewZY.block_id).Size.Z))) &&
                                (lstBlocks.ElementAt(viewZY.block_id).selected == true))
                    {
                        viewZY.isResizingBlock = true;
                        viewZY.resizePosX = false;
                        viewZY.resizeSizeX = false;
                        viewZY.resizePosY = false;
                        viewZY.resizeSizeY = true;

                        viewZY.original_x = viewZY.GetGridX(e.X) - lstBlocks.ElementAt(viewZY.block_id).Size.Z;
                        viewZY.original_y = viewZY.GetGridY(e.Y) - lstBlocks.ElementAt(viewZY.block_id).Size.Y;

                    }
                    else if (!bItemSelected)
                    {
                        bool bOneSelected = false;
                        if (lstBlocks.Count() > 0 && ((lstBlocks.ElementAt(viewZY.block_id).Size.Z >= 0 && (x >= lstBlocks.ElementAt(viewZY.block_id).Position.Z && x <= lstBlocks.ElementAt(viewZY.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z)) ||
                           (lstBlocks.ElementAt(viewZY.block_id).Size.Z < 0 && (x <= lstBlocks.ElementAt(viewZY.block_id).Position.Z && x >= lstBlocks.ElementAt(viewZY.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z))) &&
                           ((lstBlocks.ElementAt(viewZY.block_id).Size.Y >= 0 && (y >= lstBlocks.ElementAt(viewZY.block_id).Position.Y && y <= lstBlocks.ElementAt(viewZY.block_id).Position.Y + lstBlocks.ElementAt(viewXZ.block_id).Size.Y)) ||
                           (lstBlocks.ElementAt(viewZY.block_id).Size.Y < 0 && (y <= lstBlocks.ElementAt(viewZY.block_id).Position.Y && y >= lstBlocks.ElementAt(viewZY.block_id).Position.Y + lstBlocks.ElementAt(viewXZ.block_id).Size.Y))) && lstBlocks.ElementAt(viewXZ.block_id).selected)
                        {
                            viewZY.isMovingBlock = true;
                            viewZY.original_x = viewZY.GetGridX(e.X) - lstBlocks.ElementAt(viewZY.block_id).Position.Z;
                            viewZY.original_y = viewZY.GetGridY(e.Y) - lstBlocks.ElementAt(viewZY.block_id).Position.Y;
                            bOneSelected = true;
                        }
                        else
                        {
                            for (int i = 0; i < lstBlocks.Count(); i++)
                            {

                                if (((lstBlocks.ElementAt(i).Size.Z >= 0 && (x >= lstBlocks.ElementAt(i).Position.Z && x <= lstBlocks.ElementAt(i).Position.Z + lstBlocks.ElementAt(i).Size.Z)) ||
                                    (lstBlocks.ElementAt(i).Size.Z < 0 && (x <= lstBlocks.ElementAt(i).Position.Z && x >= lstBlocks.ElementAt(i).Position.Z + lstBlocks.ElementAt(i).Size.Z))) &&
                                    ((lstBlocks.ElementAt(i).Size.Y >= 0 && (y >= lstBlocks.ElementAt(i).Position.Y && y <= lstBlocks.ElementAt(i).Position.Y + lstBlocks.ElementAt(i).Size.Y)) ||
                                    (lstBlocks.ElementAt(i).Size.Y < 0 && (y <= lstBlocks.ElementAt(i).Position.Y && y >= lstBlocks.ElementAt(i).Position.Y + lstBlocks.ElementAt(i).Size.Y))) && (!lstBlocks.ElementAt(i).selected || viewZY.block_id == i) && !bOneSelected)
                                {
                                    viewZY.isMovingBlock = true;
                                    lstBlocks.ElementAt(i).selected = true;
                                    viewXY.block_id = i;
                                    viewXZ.block_id = i;
                                    viewZY.block_id = i;
                                    viewZY.original_x = viewZY.GetGridX(e.X) - lstBlocks.ElementAt(i).Position.Z;
                                    viewZY.original_y = viewZY.GetGridY(e.Y) - lstBlocks.ElementAt(i).Position.Y;
                                    bOneSelected = true;
                                }
                                else
                                {
                                    lstBlocks.ElementAt(i).selected = false;
                                }
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    bool foundSelected = false;
                    bool selectedNew = false;
                    int foundfirst = 0;
                    bool hasfoundfirst = false;
                    int x = viewZY.GetGridX(e.X);
                    int y = viewZY.GetGridY(e.Y);
                    foreach (Block b in lstBlocks)
                    {
                        if (((b.Size.Z >= 0 && (x >= b.Position.Z && x <= b.Position.Z + b.Size.Z)) ||
                                (b.Size.Z < 0 && (x <= b.Position.Z && x >= b.Position.Z + b.Size.Z))) &&
                                ((b.Size.Y >= 0 && (y >= b.Position.Y && y <= b.Position.Y + b.Size.Y)) ||
                                (b.Size.Y < 0 && (y <= b.Position.Y && y >= b.Position.Y + b.Size.Y))) && (!b.selected) && foundSelected && !selectedNew)
                        {
                            this.DeselectAll();
                            b.selected = true;
                            viewXY.block_id = lstBlocks.IndexOf(b);
                            viewXZ.block_id = lstBlocks.IndexOf(b);
                            viewZY.block_id = lstBlocks.IndexOf(b);
                            selectedNew = true;
                            break;
                        }
                        else if (((b.Size.Z >= 0 && (x >= b.Position.Z && x <= b.Position.Z + b.Size.Z)) ||
                                (b.Size.Z < 0 && (x <= b.Position.Z && x >= b.Position.Z + b.Size.Z))) &&
                                ((b.Size.Y >= 0 && (y >= b.Position.Y && y <= b.Position.Y + b.Size.Y)) ||
                                (b.Size.Y < 0 && (y <= b.Position.Y && y >= b.Position.Y + b.Size.Y))) && (b.selected) && !selectedNew)
                        {
                            foundSelected = true;
                        }
                        else if (((b.Size.Z >= 0 && (x >= b.Position.Z && x <= b.Position.Z + b.Size.Z)) ||
                                (b.Size.Z < 0 && (x <= b.Position.Z && x >= b.Position.Z + b.Size.Z))) &&
                                ((b.Size.Y >= 0 && (y >= b.Position.Y && y <= b.Position.Y + b.Size.Y)) ||
                                (b.Size.Y < 0 && (y <= b.Position.Y && y >= b.Position.Y + b.Size.Y))) && !hasfoundfirst)
                        {
                            foundfirst = lstBlocks.IndexOf(b);
                            hasfoundfirst = true;
                        }
                    }
                    if (foundSelected && !selectedNew && lstBlocks.Count() > 0)
                    {
                        this.DeselectAll();
                        lstBlocks.ElementAt(foundfirst).selected = true;
                        viewXY.block_id = foundfirst;
                        viewXZ.block_id = foundfirst;
                        viewZY.block_id = foundfirst;
                    }
                }
                /* Items */
                if (e.Button == MouseButtons.Left && iToolID == 4)
                {
                    lstItems.Add(new Item("ZY", viewZY.GetGridX(e.X), viewZY.GetGridY(e.Y), this.iToolItemID.ToString(), "Item" + this.levelInfo.itemiterator));
                    this.levelInfo.itemiterator++;
                }
                this.RedrawItems();
                this.RedrawTemp();
                this.UpdateProperties();
            }
            private void scZY_MouseUp(object sender, MouseEventArgs e)
            {
                viewZY.isPanning = false;
                if (viewZY.isDrawingBlock)
                {
                    Block tmpBlock = new Block("ZY", viewZY.original_x, viewZY.original_y, viewZY.current_x - viewZY.original_x, viewZY.current_y - viewZY.original_y, "Block" + this.levelInfo.blockiterator);
                    this.levelInfo.blockiterator++;
                    lstBlocks.Add(tmpBlock);
                    viewZY.isDrawingBlock = false;
                    this.bFileIsChanged = true;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                else if (viewZY.isMovingBlock)
                {
                    viewZY.isMovingBlock = false;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                else if (viewZY.isMovingItem)
                {
                    viewZY.isMovingItem = false;
                    this.RedrawItems();
                }
                else if (viewZY.isResizingBlock)
                {
                    viewZY.isResizingBlock = false;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                this.UpdateTitle();
            }
            private void scZY_MouseLeave(object sender, EventArgs e)
            {
                viewZY.isPanning = false;
                viewZY.isDrawingBlock = false;
                viewZY.isResizingBlock = false;
                viewZY.isMovingBlock = false;
                viewZY.isMovingItem = false;
                tslView.Text = "View: None";
                tslCoordinates.Text = "X: N/A Y:N/A";

                //this.RedrawTemp();
                //this.RedrawBlocks();
            }
            private void scZY_MouseMove(object sender, MouseEventArgs e)
            {
                if (viewZY.isPanning == true)
                {
                    viewZY.Scroll(viewZY.last_x - e.X, viewZY.last_y - e.Y);
                    viewZY.last_x = e.X;
                    viewZY.last_y = e.Y;
                }
                else if (viewZY.isDrawingBlock)
                {
                    viewZY.current_x = viewZY.GetGridX(e.X);
                    viewZY.current_y = viewZY.GetGridY(e.Y);
                    if (viewZY.current_x != viewZY.last_x || viewZY.current_y != viewZY.last_y)
                    {
                        viewZY.last_x = viewZY.GetGridX(e.X);
                        viewZY.last_y = viewZY.GetGridY(e.Y);
                        viewZY.RedrawTemp();
                    }
                }
                else if (viewZY.isMovingBlock)
                {
                    viewZY.current_x = viewZY.GetGridX(e.X);
                    viewZY.current_y = viewZY.GetGridY(e.Y);
                    if (viewZY.current_x != viewZY.last_x || viewZY.current_y != viewZY.last_y)
                    {
                        lstBlocks.ElementAt(viewZY.block_id).Position.Z = viewZY.GetGridX(e.X) - viewZY.original_x;
                        lstBlocks.ElementAt(viewZY.block_id).Position.Y = viewZY.GetGridY(e.Y) - viewZY.original_y;

                        this.RedrawTemp();

                        this.bFileIsChanged = true;

                        viewZY.last_x = viewZY.GetGridX(e.X);
                        viewZY.last_y = viewZY.GetGridY(e.Y);

                    }
                }
                else if (viewZY.isResizingBlock)
                {
                    viewZY.current_x = viewZY.GetGridX(e.X);
                    viewZY.current_y = viewZY.GetGridY(e.Y);
                    if (viewZY.current_x != viewZY.last_x || viewZY.current_y != viewZY.last_y)
                    {
                        if (viewZY.resizePosX)
                        {
                            int diff = lstBlocks.ElementAt(viewZY.block_id).Position.Z - (viewZY.GetGridX(e.X) - viewZY.original_x);
                            lstBlocks.ElementAt(viewZY.block_id).Position.Z = viewZY.GetGridX(e.X) - viewZY.original_x;
                            lstBlocks.ElementAt(viewZY.block_id).Size.Z += diff;
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewZY.resizeSizeX)
                        {
                            lstBlocks.ElementAt(viewZY.block_id).Size.Z = -(viewZY.original_x - viewZY.GetGridX(e.X));
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewZY.resizePosY)
                        {
                            int diff = lstBlocks.ElementAt(viewZY.block_id).Position.Y - (viewZY.GetGridY(e.Y) - viewZY.original_y);
                            lstBlocks.ElementAt(viewZY.block_id).Position.Y = viewZY.GetGridY(e.Y) - viewZY.original_y;
                            lstBlocks.ElementAt(viewZY.block_id).Size.Y += diff;
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewZY.resizeSizeY)
                        {
                            lstBlocks.ElementAt(viewZY.block_id).Size.Y = -(viewZY.original_y - viewZY.GetGridY(e.Y));
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        viewZY.last_x = viewZY.GetGridX(e.X);
                        viewZY.last_y = viewZY.GetGridY(e.Y);
                    }
                }
                else if (viewZY.isMovingItem)
                {
                    viewZY.current_x = viewZY.GetGridX(e.X);
                    viewZY.current_y = viewZY.GetGridY(e.Y);
                    if (viewZY.current_x != viewZY.last_x || viewZY.current_y != viewZY.last_y)
                    {
                        lstItems.ElementAt(viewZY.item_id).Position.Z = viewZY.GetGridX(e.X) - viewZY.original_x;
                        lstItems.ElementAt(viewZY.item_id).Position.Y = viewZY.GetGridY(e.Y) - viewZY.original_y;

                        this.RedrawTemp();

                        this.bFileIsChanged = true;

                        viewZY.last_x = viewZY.GetGridX(e.X);
                        viewZY.last_y = viewZY.GetGridY(e.Y);

                    }
                }

                tslView.Text = "View: Left (ZY)";

                int x = viewZY.GetGridX(e.X);
                int y = viewZY.GetGridY(e.Y);

                tslCoordinates.Text = "Z:" + x.ToString() + " Y:" + y.ToString();

            }

            private void scXZ_MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                {
                    viewXZ.isPanning = true;
                    viewXZ.last_x = e.X;
                    viewXZ.last_y = e.Y;
                }
                if (e.Button == MouseButtons.Left && this.iToolID == 2) // LB & Tool2 = Create Box
                {
                    viewXZ.current_x = viewXZ.GetGridX(e.X);
                    viewXZ.current_y = viewXZ.GetGridY(e.Y);              
                    viewXZ.original_x = viewXZ.GetGridX(e.X);
                    viewXZ.original_y = viewXZ.GetGridY(e.Y);
                    viewXZ.isDrawingBlock = true;
                }
                if (e.Button == MouseButtons.Left && (this.iToolID == 0 || this.iToolID == 1)) // LB & Tool0 = Select And move
                {
                    int x = viewXZ.GetGridX(e.X);
                    int y = viewXZ.GetGridY(e.Y);

                    bool bItemSelected = false;
                    /* Firstly, check if a item is under the mousepointer */
                    for (int i = 0; i < lstItems.Count(); i++)
                    {
                        if (x == lstItems.ElementAt(i).Position.X &&
                            y == lstItems.ElementAt(i).Position.Z)
                        {
                            this.DeselectAll();
                            viewXZ.isMovingItem = true;
                            lstItems.ElementAt(i).selected = true;
                            viewXY.item_id = i;
                            viewXZ.item_id = i;
                            viewZY.item_id = i;
                            viewXZ.original_x = viewXZ.GetGridX(e.X) - lstItems.ElementAt(i).Position.X;
                            viewXZ.original_y = viewXZ.GetGridY(e.Y) - lstItems.ElementAt(i).Position.Z;
                            bItemSelected = true;
                            break;
                        }
                        else
                        {
                            lstItems.ElementAt(i).selected = false;
                        }
                    }

                    /* Secondly, check if a block is under the mousepointer */
                    if (!bItemSelected && lstBlocks.Count() > 0 && (x == lstBlocks.ElementAt(viewXZ.block_id).Position.X) &&
                            ((lstBlocks.ElementAt(viewXZ.block_id).Size.Z >= 0 && (y >= lstBlocks.ElementAt(viewXZ.block_id).Position.Z && y <= lstBlocks.ElementAt(viewXZ.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z)) ||
                            (lstBlocks.ElementAt(viewXZ.block_id).Size.Z < 0 && (y <= lstBlocks.ElementAt(viewXZ.block_id).Position.Z && y >= lstBlocks.ElementAt(viewXZ.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z))) &&
                            (lstBlocks.ElementAt(viewXZ.block_id).selected == true))
                    {
                        viewXZ.isResizingBlock = true;
                        viewXZ.resizePosX = true;
                        viewXZ.resizeSizeX = false;
                        viewXZ.resizePosY = false;
                        viewXZ.resizeSizeY = false;

                        viewXZ.original_x = viewXZ.GetGridX(e.X) - lstBlocks.ElementAt(viewXZ.block_id).Position.X;
                        viewXZ.original_y = viewXZ.GetGridY(e.Y) - lstBlocks.ElementAt(viewXZ.block_id).Position.Z;
                    }
                    else if (!bItemSelected && lstBlocks.Count() > 0 && (x == lstBlocks.ElementAt(viewXZ.block_id).Position.X + lstBlocks.ElementAt(viewXZ.block_id).Size.X) &&
                            ((lstBlocks.ElementAt(viewXZ.block_id).Size.Z >= 0 && (y >= lstBlocks.ElementAt(viewXZ.block_id).Position.Z && y <= lstBlocks.ElementAt(viewXZ.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z)) ||
                            (lstBlocks.ElementAt(viewXZ.block_id).Size.Z < 0 && (y <= lstBlocks.ElementAt(viewXZ.block_id).Position.Z && y >= lstBlocks.ElementAt(viewXZ.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z))) &&
                            (lstBlocks.ElementAt(viewXZ.block_id).selected == true))
                    {
                        viewXZ.isResizingBlock = true;
                        viewXZ.resizePosX = false;
                        viewXZ.resizeSizeX = true;
                        viewXZ.resizePosY = false;
                        viewXZ.resizeSizeY = false;

                        viewXZ.original_x = viewXZ.GetGridX(e.X) - lstBlocks.ElementAt(viewXZ.block_id).Size.X;
                        viewXZ.original_y = viewXZ.GetGridY(e.Y) - lstBlocks.ElementAt(viewXZ.block_id).Size.Z;
                    }
                    else if (!bItemSelected && lstBlocks.Count() > 0 && (y == lstBlocks.ElementAt(viewXZ.block_id).Position.Z) &&
                            ((lstBlocks.ElementAt(viewXZ.block_id).Size.X >= 0 && (x >= lstBlocks.ElementAt(viewXZ.block_id).Position.X && x <= lstBlocks.ElementAt(viewXZ.block_id).Position.X + lstBlocks.ElementAt(viewXZ.block_id).Size.X)) ||
                            (lstBlocks.ElementAt(viewXZ.block_id).Size.X < 0 && (x <= lstBlocks.ElementAt(viewXZ.block_id).Position.X && x >= lstBlocks.ElementAt(viewXZ.block_id).Position.X + lstBlocks.ElementAt(viewXZ.block_id).Size.X))) &&
                            (lstBlocks.ElementAt(viewXZ.block_id).selected == true))
                    {
                        viewXZ.isResizingBlock = true;
                        viewXZ.resizePosX = false;
                        viewXZ.resizeSizeX = false;
                        viewXZ.resizePosY = true;
                        viewXZ.resizeSizeY = false;

                        viewXZ.original_x = viewXZ.GetGridX(e.X) - lstBlocks.ElementAt(viewXZ.block_id).Position.X;
                        viewXZ.original_y = viewXZ.GetGridY(e.Y) - lstBlocks.ElementAt(viewXZ.block_id).Position.Z;
                    }
                    else if (!bItemSelected && lstBlocks.Count() > 0 && (y == lstBlocks.ElementAt(viewXZ.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z) &&
                              ((lstBlocks.ElementAt(viewXZ.block_id).Size.X >= 0 && (x >= lstBlocks.ElementAt(viewXZ.block_id).Position.X && x <= lstBlocks.ElementAt(viewXZ.block_id).Position.X + lstBlocks.ElementAt(viewXZ.block_id).Size.X)) ||
                              (lstBlocks.ElementAt(viewXZ.block_id).Size.X < 0 && (x <= lstBlocks.ElementAt(viewXZ.block_id).Position.X && x >= lstBlocks.ElementAt(viewXZ.block_id).Position.X + lstBlocks.ElementAt(viewXZ.block_id).Size.X))) &&
                                (lstBlocks.ElementAt(viewXZ.block_id).selected == true))
                    {
                        viewXZ.isResizingBlock = true;
                        viewXZ.resizePosX = false;
                        viewXZ.resizeSizeX = false;
                        viewXZ.resizePosY = false;
                        viewXZ.resizeSizeY = true;

                        viewXZ.original_x = viewXZ.GetGridX(e.X) - lstBlocks.ElementAt(viewXZ.block_id).Size.X;
                        viewXZ.original_y = viewXZ.GetGridY(e.Y) - lstBlocks.ElementAt(viewXZ.block_id).Size.Z;

                    }
                    else if (!bItemSelected)
                    {
                        bool bOneSelected = false;
                        if (lstBlocks.Count() > 0 && ((lstBlocks.ElementAt(viewXZ.block_id).Size.X >= 0 && (x >= lstBlocks.ElementAt(viewXZ.block_id).Position.X && x <= lstBlocks.ElementAt(viewXZ.block_id).Position.X + lstBlocks.ElementAt(viewXZ.block_id).Size.X)) ||
                           (lstBlocks.ElementAt(viewXZ.block_id).Size.X < 0 && (x <= lstBlocks.ElementAt(viewXZ.block_id).Position.X && x >= lstBlocks.ElementAt(viewXZ.block_id).Position.X + lstBlocks.ElementAt(viewXZ.block_id).Size.X))) &&
                           ((lstBlocks.ElementAt(viewXZ.block_id).Size.Z >= 0 && (y >= lstBlocks.ElementAt(viewXZ.block_id).Position.Z && y <= lstBlocks.ElementAt(viewXZ.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z)) ||
                           (lstBlocks.ElementAt(viewXZ.block_id).Size.Z < 0 && (y <= lstBlocks.ElementAt(viewXZ.block_id).Position.Z && y >= lstBlocks.ElementAt(viewXZ.block_id).Position.Z + lstBlocks.ElementAt(viewXZ.block_id).Size.Z))) && lstBlocks.ElementAt(viewXZ.block_id).selected)
                        {
                            viewXZ.isMovingBlock = true;
                            viewXZ.original_x = viewXZ.GetGridX(e.X) - lstBlocks.ElementAt(viewXZ.block_id).Position.X;
                            viewXZ.original_y = viewXZ.GetGridY(e.Y) - lstBlocks.ElementAt(viewXZ.block_id).Position.Z;
                            bOneSelected = true;
                        }
                        else
                        {
                            for (int i = 0; i < lstBlocks.Count(); i++)
                            {

                                if (((lstBlocks.ElementAt(i).Size.X >= 0 && (x >= lstBlocks.ElementAt(i).Position.X && x <= lstBlocks.ElementAt(i).Position.X + lstBlocks.ElementAt(i).Size.X)) ||
                                    (lstBlocks.ElementAt(i).Size.X < 0 && (x <= lstBlocks.ElementAt(i).Position.X && x >= lstBlocks.ElementAt(i).Position.X + lstBlocks.ElementAt(i).Size.X))) &&
                                    ((lstBlocks.ElementAt(i).Size.Z >= 0 && (y >= lstBlocks.ElementAt(i).Position.Z && y <= lstBlocks.ElementAt(i).Position.Z + lstBlocks.ElementAt(i).Size.Z)) ||
                                    (lstBlocks.ElementAt(i).Size.Z < 0 && (y <= lstBlocks.ElementAt(i).Position.Z && y >= lstBlocks.ElementAt(i).Position.Z + lstBlocks.ElementAt(i).Size.Z))) && (!lstBlocks.ElementAt(i).selected || viewXZ.block_id == i) && !bOneSelected)
                                {
                                    viewXZ.isMovingBlock = true;
                                    lstBlocks.ElementAt(i).selected = true;
                                    viewXY.block_id = i;
                                    viewXZ.block_id = i;
                                    viewZY.block_id = i;
                                    viewXZ.original_x = viewXZ.GetGridX(e.X) - lstBlocks.ElementAt(i).Position.X;
                                    viewXZ.original_y = viewXZ.GetGridY(e.Y) - lstBlocks.ElementAt(i).Position.Z;
                                    bOneSelected = true;
                                }
                                else
                                {
                                    lstBlocks.ElementAt(i).selected = false;
                                }
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    bool foundSelected = false;
                    bool selectedNew = false;
                    int foundfirst = 0;
                    bool hasfoundfirst = false;
                    int x = viewXZ.GetGridX(e.X);
                    int y = viewXZ.GetGridY(e.Y);
                    foreach (Block b in lstBlocks)
                    {
                        if (((b.Size.X >= 0 && (x >= b.Position.X && x <= b.Position.X + b.Size.X)) ||
                                (b.Size.X < 0 && (x <= b.Position.X && x >= b.Position.X + b.Size.X))) &&
                                ((b.Size.Z >= 0 && (y >= b.Position.Z && y <= b.Position.Z + b.Size.Z)) ||
                                (b.Size.Z < 0 && (y <= b.Position.Z && y >= b.Position.Z + b.Size.Z))) && (!b.selected) && foundSelected && !selectedNew)
                        {
                            this.DeselectAll();
                            b.selected = true;
                            viewXY.block_id = lstBlocks.IndexOf(b);
                            viewXZ.block_id = lstBlocks.IndexOf(b);
                            viewZY.block_id = lstBlocks.IndexOf(b);
                            this.RedrawTemp();
                            selectedNew = true;
                            break;
                        }
                        else if (((b.Size.X >= 0 && (x >= b.Position.X && x <= b.Position.X + b.Size.X)) ||
                                (b.Size.X < 0 && (x <= b.Position.X && x >= b.Position.X + b.Size.X))) &&
                                ((b.Size.Z >= 0 && (y >= b.Position.Z && y <= b.Position.Z + b.Size.Z)) ||
                                (b.Size.Z < 0 && (y <= b.Position.Z && y >= b.Position.Z + b.Size.Z))) && (b.selected) && !selectedNew)
                        {
                            foundSelected = true;
                        }
                        else if (((b.Size.X >= 0 && (x >= b.Position.X && x <= b.Position.X + b.Size.X)) ||
                                (b.Size.X < 0 && (x <= b.Position.X && x >= b.Position.X + b.Size.X))) &&
                                ((b.Size.Z >= 0 && (y >= b.Position.Z && y <= b.Position.Z + b.Size.Z)) ||
                                (b.Size.Z < 0 && (y <= b.Position.Z && y >= b.Position.Z + b.Size.Z))) && !hasfoundfirst)
                        {
                            foundfirst = lstBlocks.IndexOf(b);
                            hasfoundfirst = true;
                        }
                    }
                    if (foundSelected && !selectedNew && lstBlocks.Count() > 0)
                    {
                        this.DeselectAll();
                        lstBlocks.ElementAt(foundfirst).selected = true;
                        viewXY.block_id = foundfirst;
                        viewXZ.block_id = foundfirst;
                        viewZY.block_id = foundfirst;
                    }
                }
                /* Items */
                if (e.Button == MouseButtons.Left && iToolID == 4)
                {
                    lstItems.Add(new Item("XZ", viewXZ.GetGridX(e.X), viewXZ.GetGridY(e.Y), this.iToolItemID.ToString(), "Item" + this.levelInfo.itemiterator));
                    this.levelInfo.itemiterator++;
                }
                this.RedrawItems();
                this.RedrawTemp();
                this.UpdateProperties();
            }
            private void scXZ_MouseUp(object sender, MouseEventArgs e)
            {
                viewXZ.isPanning = false;
                if (viewXZ.isDrawingBlock)
                {
                    Block tmpBlock = new Block("XZ", viewXZ.original_x, viewXZ.original_y, viewXZ.current_x - viewXZ.original_x, viewXZ.current_y - viewXZ.original_y, "Block" + this.levelInfo.blockiterator);
                    this.levelInfo.blockiterator++;
                    lstBlocks.Add(tmpBlock);
                    viewXZ.isDrawingBlock = false;
                    this.bFileIsChanged = true;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                else if (viewXZ.isMovingBlock)
                {
                    viewXZ.isMovingBlock = false;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                else if (viewXZ.isMovingItem)
                {
                    viewXZ.isMovingItem = false;
                    this.RedrawItems();
                }
                else if (viewXZ.isResizingBlock)
                {
                    viewXZ.isResizingBlock = false;
                    this.RedrawBlocks();
                    this.RedrawTemp();
                }
                this.UpdateTitle();
            }
            private void scXZ_MouseLeave(object sender, EventArgs e)
            {
                viewXZ.isPanning = false;
                viewXZ.isDrawingBlock = false;
                viewXZ.isResizingBlock = false;
                viewXZ.isMovingBlock = false;
                viewXZ.isMovingItem = false;
                tslView.Text = "View: None";
                tslCoordinates.Text = "X: N/A Y:N/A";

                //this.RedrawTemp();
                //this.RedrawBlocks();
            }
            private void scXZ_MouseMove(object sender, MouseEventArgs e)
            {
                if (viewXZ.isPanning == true)
                {
                    viewXZ.Scroll(viewXZ.last_x - e.X, viewXZ.last_y - e.Y);
                    viewXZ.last_x = e.X;
                    viewXZ.last_y = e.Y;
                }
                else if (viewXZ.isDrawingBlock)
                {
                    viewXZ.current_x = viewXZ.GetGridX(e.X);
                    viewXZ.current_y = viewXZ.GetGridY(e.Y);
                    if (viewXZ.current_x != viewXZ.last_x || viewXZ.current_y != viewXZ.last_y)
                    {
                        viewXZ.last_x = viewXZ.GetGridX(e.X);
                        viewXZ.last_y = viewXZ.GetGridY(e.Y);
                        viewXZ.RedrawTemp();
                    }
                }
                else if (viewXZ.isMovingBlock)
                {
                    viewXZ.current_x = viewXZ.GetGridX(e.X);
                    viewXZ.current_y = viewXZ.GetGridY(e.Y);
                    if (viewXZ.current_x != viewXZ.last_x || viewXZ.current_y != viewXZ.last_y)
                    {
                        lstBlocks.ElementAt(viewXZ.block_id).Position.X = viewXZ.GetGridX(e.X) - viewXZ.original_x;
                        lstBlocks.ElementAt(viewXZ.block_id).Position.Z = viewXZ.GetGridY(e.Y) - viewXZ.original_y;

                        this.RedrawTemp();

                        this.bFileIsChanged = true;

                        viewXZ.last_x = viewXZ.GetGridX(e.X);
                        viewXZ.last_y = viewXZ.GetGridY(e.Y);

                    }
                }
                else if (viewXZ.isResizingBlock)
                {
                    viewXZ.current_x = viewXZ.GetGridX(e.X);
                    viewXZ.current_y = viewXZ.GetGridY(e.Y);
                    if (viewXZ.current_x != viewXZ.last_x || viewXZ.current_y != viewXZ.last_y)
                    {
                        if (viewXZ.resizePosX)
                        {
                            int diff = lstBlocks.ElementAt(viewXZ.block_id).Position.X - (viewXZ.GetGridX(e.X) - viewXZ.original_x);
                            lstBlocks.ElementAt(viewXZ.block_id).Position.X = viewXZ.GetGridX(e.X) - viewXZ.original_x;
                            lstBlocks.ElementAt(viewXZ.block_id).Size.X += diff;
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewXZ.resizeSizeX)
                        {
                            lstBlocks.ElementAt(viewXZ.block_id).Size.X = -(viewXZ.original_x - viewXZ.GetGridX(e.X));
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewXZ.resizePosY)
                        {
                            int diff = lstBlocks.ElementAt(viewXZ.block_id).Position.Z - (viewXZ.GetGridY(e.Y) - viewXZ.original_y);
                            lstBlocks.ElementAt(viewXZ.block_id).Position.Z = viewXZ.GetGridY(e.Y) - viewXZ.original_y;
                            lstBlocks.ElementAt(viewXZ.block_id).Size.Z += diff;
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        else if (viewXZ.resizeSizeY)
                        {
                            lstBlocks.ElementAt(viewXZ.block_id).Size.Z = -(viewXZ.original_y - viewXZ.GetGridY(e.Y));
                            this.bFileIsChanged = true;
                            this.RedrawTemp();
                        }
                        viewXZ.last_x = viewXZ.GetGridX(e.X);
                        viewXZ.last_y = viewXZ.GetGridY(e.Y);
                    }
                }
                else if (viewXZ.isMovingItem)
                {
                    viewXZ.current_x = viewXZ.GetGridX(e.X);
                    viewXZ.current_y = viewXZ.GetGridY(e.Y);
                    if (viewXZ.current_x != viewXZ.last_x || viewXZ.current_y != viewXZ.last_y)
                    {
                        lstItems.ElementAt(viewXZ.item_id).Position.X = viewXZ.GetGridX(e.X) - viewXZ.original_x;
                        lstItems.ElementAt(viewXZ.item_id).Position.Z = viewXZ.GetGridY(e.Y) - viewXZ.original_y;

                        this.RedrawTemp();

                        this.bFileIsChanged = true;

                        viewXZ.last_x = viewXZ.GetGridX(e.X);
                        viewXZ.last_y = viewXZ.GetGridY(e.Y);

                    }
                }

                tslView.Text = "View: Top (XZ)";

                int x = viewXZ.GetGridX(e.X);
                int y = viewXZ.GetGridY(e.Y);

                tslCoordinates.Text = "X:" + x.ToString() + " Z:" + y.ToString();

            }

            private void tsbSelectMove_Click(object sender, EventArgs e)
            {
                this.iToolID = 0;
                this.tsmSelectMove.Checked = true;
                this.tsmCreate.Checked = false;
                this.tslTool.Text = "Tool: Select & Move";
            }
            private void tsbCreate_Click(object sender, EventArgs e)
            {
                this.iToolID = 2;
                this.tsmSelectMove.Checked = false;
                this.tsmCreate.Checked = true;
                this.tslTool.Text = "Tool: Create block";
            }

            private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (this.bFileIsSaved)
                {
                    this.FileSave();
                }
                else
                {
                    this.FileSaveAs();
                }
            }
            private void SaveasToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.FileSaveAs();
            }

            private void openToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.FileOpen();
            }
            private void deleteSelectedObjectToolStripMenuItem_Click(object sender, EventArgs e)
            {
                foreach (Block b in lstBlocks)
                {
                    if (b.selected)
                    {
                        lstBlocks.Remove(b);
                        this.DeselectAll();
                        RedrawBlocks();
                        RedrawTemp();
                        break;
                    }
                }
                foreach (Item b in lstItems)
                {
                    if (b.selected)
                    {
                        lstItems.Remove(b);
                        this.DeselectAll();
                        RedrawItems();
                        RedrawTemp();
                        break;
                    }
                }
            }
            private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
            {
                dlgAbout d;
                d = new dlgAbout();
                d.ShowDialog();
            }
            private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
            {
                pgProperties.Height = splitContainer1.Height - cbItemSelector.Height - 7;
            }
            private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
            {
                this.RedrawAll();
            }

            private void cbItemSelector_SelectedIndexChanged(object sender, EventArgs e)
            {
                this.SelectObjectFromName(cbItemSelector.SelectedItem.ToString());
            }
            private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
            {
                pgProperties.SelectedObject = levelInfo;
            }
            private void BlablaToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.FileExportXml();
            }
            private void showHideLayer1ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(0);
            }
            private void showHideLayer2ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(1);
            }
            private void showHideLayer3ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(2);
            }
            private void showHideLayer4ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(3);
            }
            private void showHideLayer5ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(4);
            }
            private void tsbLayer1_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(0);
            }
            private void tsbLayer2_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(1);
            }
            private void tsbLayer3_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(2);
            }
            private void tsbLayer4_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(3);
            }
            private void tsbLayer5_Click(object sender, EventArgs e)
            {
                this.UpdateLayer(4);
            }
            private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
            {
                //this.ShowAllLayers(1);
            }

        #endregion

            private void glView1_MouseMove(object sender, MouseEventArgs e)
            {
                if (!this.glView1.Focused)
                {
                    this.glView1.Focus();
                    this.glView1.BringToFront();
                }

                this.glView1.active = true;
                glView1.current_x = e.X;
                glView1.current_y = e.Y;
                this.glView1.Refresh();
            }
            private void glView1_MouseLeave(object sender, EventArgs e)
            {
                glView1.active = false;
            }
            private void materialEditorToolStripMenuItem_Click(object sender, EventArgs e)
            {
                MaterialEditor d = new MaterialEditor(lstMaterial,lstBlocks);
                d.ShowDialog();
                //tSDLRun.Interrupt();
                tSDLRun.Abort();
                tSDLRun = null;
                tSDLRun = new Thread(new System.Threading.ThreadStart(SdlDotNet.Core.Events.Run));
                tSDLRun.IsBackground = true;
                tSDLRun.Name = "SDL.NET";
                tSDLRun.Priority = ThreadPriority.Normal;
                tSDLRun.Start();

            }

	}
}
