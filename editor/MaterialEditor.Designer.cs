using System.Windows;
using System.Windows.Forms;

namespace Map_Editor
{
    partial class MaterialEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsMatSel = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.gbLighting = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtAA = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAG = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAR = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtSA = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtShine = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSG = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSR = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtDA = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDG = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDR = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtEA = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtEB = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtEG = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtER = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtBumpMap = new System.Windows.Forms.TextBox();
            this.txtTexture = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.gbLighting.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tsMatSel,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(557, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(77, 22);
            this.toolStripLabel1.Text = "Select Material";
            // 
            // tsMatSel
            // 
            this.tsMatSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsMatSel.Name = "tsMatSel";
            this.tsMatSel.Size = new System.Drawing.Size(121, 25);
            this.tsMatSel.SelectedIndexChanged += new System.EventHandler(this.tsMatSel_SelectedIndexChanged);
            this.tsMatSel.Click += new System.EventHandler(this.tsMatSel_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::My.Resources.Resources.toolbutton_delmat;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Delete current material";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::My.Resources.Resources.toolbutton_addmat;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "New material";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // gbPreview
            // 
            this.gbPreview.Location = new System.Drawing.Point(7, 32);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Size = new System.Drawing.Size(283, 279);
            this.gbPreview.TabIndex = 2;
            this.gbPreview.TabStop = false;
            this.gbPreview.Text = "Preview";
            // 
            // gbLighting
            // 
            this.gbLighting.Controls.Add(this.tabControl1);
            this.gbLighting.Location = new System.Drawing.Point(296, 168);
            this.gbLighting.Name = "gbLighting";
            this.gbLighting.Size = new System.Drawing.Size(251, 143);
            this.gbLighting.TabIndex = 4;
            this.gbLighting.TabStop = false;
            this.gbLighting.Text = "Lighting";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(6, 19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(239, 119);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtAA);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtAB);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtAG);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtAR);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(231, 93);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Ambient";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtAA
            // 
            this.txtAA.Location = new System.Drawing.Point(151, 9);
            this.txtAA.Name = "txtAA";
            this.txtAA.Size = new System.Drawing.Size(48, 20);
            this.txtAA.TabIndex = 7;
            this.txtAA.Text = "0";
            this.txtAA.TextChanged += new System.EventHandler(this.txtAA_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Alpha";
            // 
            // txtAB
            // 
            this.txtAB.Location = new System.Drawing.Point(48, 61);
            this.txtAB.Name = "txtAB";
            this.txtAB.Size = new System.Drawing.Size(48, 20);
            this.txtAB.TabIndex = 5;
            this.txtAB.Text = "0";
            this.txtAB.TextChanged += new System.EventHandler(this.txtAB_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Blue";
            // 
            // txtAG
            // 
            this.txtAG.Location = new System.Drawing.Point(48, 35);
            this.txtAG.Name = "txtAG";
            this.txtAG.Size = new System.Drawing.Size(48, 20);
            this.txtAG.TabIndex = 3;
            this.txtAG.Text = "0";
            this.txtAG.TextChanged += new System.EventHandler(this.txtAG_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Green";
            // 
            // txtAR
            // 
            this.txtAR.Location = new System.Drawing.Point(48, 9);
            this.txtAR.Name = "txtAR";
            this.txtAR.Size = new System.Drawing.Size(48, 20);
            this.txtAR.TabIndex = 1;
            this.txtAR.Text = "0";
            this.txtAR.TextChanged += new System.EventHandler(this.txtAR_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Red";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtSA);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.txtShine);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtSB);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtSG);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.txtSR);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(231, 93);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Specular";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtSA
            // 
            this.txtSA.Location = new System.Drawing.Point(151, 9);
            this.txtSA.Name = "txtSA";
            this.txtSA.Size = new System.Drawing.Size(48, 20);
            this.txtSA.TabIndex = 25;
            this.txtSA.Text = "0";
            this.txtSA.TextChanged += new System.EventHandler(this.txtSA_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(111, 12);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(34, 13);
            this.label17.TabIndex = 24;
            this.label17.Text = "Alpha";
            // 
            // txtShine
            // 
            this.txtShine.Location = new System.Drawing.Point(151, 35);
            this.txtShine.Name = "txtShine";
            this.txtShine.Size = new System.Drawing.Size(48, 20);
            this.txtShine.TabIndex = 15;
            this.txtShine.Text = "0";
            this.txtShine.TextChanged += new System.EventHandler(this.txtShine_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(111, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Shine";
            // 
            // txtSB
            // 
            this.txtSB.Location = new System.Drawing.Point(48, 61);
            this.txtSB.Name = "txtSB";
            this.txtSB.Size = new System.Drawing.Size(48, 20);
            this.txtSB.TabIndex = 13;
            this.txtSB.Text = "0";
            this.txtSB.TextChanged += new System.EventHandler(this.txtSB_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Blue";
            // 
            // txtSG
            // 
            this.txtSG.Location = new System.Drawing.Point(48, 35);
            this.txtSG.Name = "txtSG";
            this.txtSG.Size = new System.Drawing.Size(48, 20);
            this.txtSG.TabIndex = 11;
            this.txtSG.Text = "0";
            this.txtSG.TextChanged += new System.EventHandler(this.txtSG_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Green";
            // 
            // txtSR
            // 
            this.txtSR.Location = new System.Drawing.Point(48, 9);
            this.txtSR.Name = "txtSR";
            this.txtSR.Size = new System.Drawing.Size(48, 20);
            this.txtSR.TabIndex = 9;
            this.txtSR.Text = "0";
            this.txtSR.TextChanged += new System.EventHandler(this.txtSR_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Red";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtDA);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.txtDB);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.txtDG);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.txtDR);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(231, 93);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Diffuse";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtDA
            // 
            this.txtDA.Location = new System.Drawing.Point(154, 9);
            this.txtDA.Name = "txtDA";
            this.txtDA.Size = new System.Drawing.Size(48, 20);
            this.txtDA.TabIndex = 23;
            this.txtDA.Text = "0";
            this.txtDA.TextChanged += new System.EventHandler(this.txtDA_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(114, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Alpha";
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(48, 61);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(48, 20);
            this.txtDB.TabIndex = 21;
            this.txtDB.Text = "0";
            this.txtDB.TextChanged += new System.EventHandler(this.txtDB_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Blue";
            // 
            // txtDG
            // 
            this.txtDG.Location = new System.Drawing.Point(48, 35);
            this.txtDG.Name = "txtDG";
            this.txtDG.Size = new System.Drawing.Size(48, 20);
            this.txtDG.TabIndex = 19;
            this.txtDG.Text = "0";
            this.txtDG.TextChanged += new System.EventHandler(this.txtDG_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Green";
            // 
            // txtDR
            // 
            this.txtDR.Location = new System.Drawing.Point(48, 9);
            this.txtDR.Name = "txtDR";
            this.txtDR.Size = new System.Drawing.Size(48, 20);
            this.txtDR.TabIndex = 17;
            this.txtDR.Text = "0";
            this.txtDR.TextChanged += new System.EventHandler(this.txtDR_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(27, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Red";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtEA);
            this.tabPage4.Controls.Add(this.label13);
            this.tabPage4.Controls.Add(this.txtEB);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.txtEG);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.txtER);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(231, 93);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Emissive";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtEA
            // 
            this.txtEA.Location = new System.Drawing.Point(154, 9);
            this.txtEA.Name = "txtEA";
            this.txtEA.Size = new System.Drawing.Size(48, 20);
            this.txtEA.TabIndex = 31;
            this.txtEA.Text = "0";
            this.txtEA.TextChanged += new System.EventHandler(this.txtEA_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(114, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Alpha";
            // 
            // txtEB
            // 
            this.txtEB.Location = new System.Drawing.Point(48, 61);
            this.txtEB.Name = "txtEB";
            this.txtEB.Size = new System.Drawing.Size(48, 20);
            this.txtEB.TabIndex = 29;
            this.txtEB.Text = "0";
            this.txtEB.TextChanged += new System.EventHandler(this.txtEB_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 64);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "Blue";
            // 
            // txtEG
            // 
            this.txtEG.Location = new System.Drawing.Point(48, 35);
            this.txtEG.Name = "txtEG";
            this.txtEG.Size = new System.Drawing.Size(48, 20);
            this.txtEG.TabIndex = 27;
            this.txtEG.Text = "0";
            this.txtEG.TextChanged += new System.EventHandler(this.txtEG_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 38);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 26;
            this.label15.Text = "Green";
            // 
            // txtER
            // 
            this.txtER.Location = new System.Drawing.Point(48, 9);
            this.txtER.Name = "txtER";
            this.txtER.Size = new System.Drawing.Size(48, 20);
            this.txtER.TabIndex = 25;
            this.txtER.Text = "0";
            this.txtER.TextChanged += new System.EventHandler(this.txtER_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 12);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(27, 13);
            this.label16.TabIndex = 24;
            this.label16.Text = "Red";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtBumpMap);
            this.groupBox1.Controls.Add(this.txtTexture);
            this.groupBox1.Location = new System.Drawing.Point(296, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 78);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Texture filename";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 50);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(57, 13);
            this.label19.TabIndex = 1;
            this.label19.Text = "Bump map";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 22);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(43, 13);
            this.label18.TabIndex = 1;
            this.label18.Text = "Texture";
            // 
            // txtBumpMap
            // 
            this.txtBumpMap.Location = new System.Drawing.Point(70, 47);
            this.txtBumpMap.Name = "txtBumpMap";
            this.txtBumpMap.Size = new System.Drawing.Size(171, 20);
            this.txtBumpMap.TabIndex = 0;
            this.txtBumpMap.TextChanged += new System.EventHandler(this.txtBumpMap_TextChanged);
            // 
            // txtTexture
            // 
            this.txtTexture.Location = new System.Drawing.Point(70, 19);
            this.txtTexture.Name = "txtTexture";
            this.txtTexture.Size = new System.Drawing.Size(171, 20);
            this.txtTexture.TabIndex = 0;
            this.txtTexture.TextChanged += new System.EventHandler(this.txtTexture_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(442, 317);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 29);
            this.button1.TabIndex = 6;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.SystemColors.Info;
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label20.Location = new System.Drawing.Point(7, 317);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(429, 29);
            this.label20.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Location = new System.Drawing.Point(296, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(251, 46);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Material name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(6, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(235, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // MaterialEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 353);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbLighting);
            this.Controls.Add(this.gbPreview);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MaterialEditor";
            this.Text = "GPME Material Editor";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MaterialEditor_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbLighting.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tsMatSel;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.GroupBox gbPreview;
        private System.Windows.Forms.GroupBox gbLighting;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtAA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAG;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtSA;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtShine;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSG;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSR;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtDA;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDG;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDR;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtEA;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtEB;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtEG;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtER;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox1;
        private Label label19;
        private Label label18;
        private Button button1;
        private Label label20;
        private GroupBox groupBox2;
        private TextBox txtName;
        private TextBox txtBumpMap;
        private TextBox txtTexture;
    }
}