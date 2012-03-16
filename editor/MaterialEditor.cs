using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Map_Editor
{
    partial class MaterialEditor : Form
    {

        List<Material> lstMaterial;
        List<Block> lstBlocks;
        MaterialViewer mvDefault;
        Material material;
        Material selected_material;

        public MaterialEditor(object ml,object blocks)
        {
            InitializeComponent();

            this.lstMaterial = (List<Material>)ml;
            this.lstBlocks = (List<Block>)blocks;
            this.mvDefault = new MaterialViewer();
            this.mvDefault.Parent = this.gbPreview;
            this.mvDefault.Location = new Point(8, 19);
            this.mvDefault.Size = new Size(266, 254);
            this.mvDefault.Connect();
            this.material = new Material();
        }

        public void ShowMessage()
        {

        }
        private void UpdateMaterialList()
        {
            tsMatSel.Items.Clear();
            foreach (Material m in lstMaterial)
            {
                tsMatSel.Items.Add(m.name);
            }
        }
        private void NewMaterial()
        {
            Material nm = new Material() ;
            bool f = true;
            string name = "New Material";
            int it = 1;
            while (f)
            {
                f = false;
                foreach (Material m in lstMaterial)
                {
                    if (m.name == name && it == 1)
                    {
                        f = true;
                        it++;
                        break;
                    }
                    else if (m.name == name + " " + it.ToString())
                    {
                        f = true;
                        it++;
                        break;
                    }
                }
            }
            if (it < 2)
            {
                nm.name = "New Material";
            }
            else
            {
                nm.name = "New Material " + it.ToString();
            }
            lstMaterial.Add(nm);
            UpdateMaterialList();
            SelectMaterial(nm.name);
        }
        private void SelectMaterial(string name)
        {
            foreach (Material m in lstMaterial)
            {
                if (m.name == name)
                {
                    txtAA.Text = m.ambient.A.ToString();
                    txtAR.Text = m.ambient.R.ToString();
                    txtAG.Text = m.ambient.G.ToString();
                    txtAB.Text = m.ambient.B.ToString();
                    txtSA.Text = m.specular.A.ToString();
                    txtSR.Text = m.specular.R.ToString();
                    txtSG.Text = m.specular.G.ToString();
                    txtSB.Text = m.specular.B.ToString();
                    txtEA.Text = m.emission.A.ToString();
                    txtER.Text = m.emission.R.ToString();
                    txtEG.Text = m.emission.G.ToString();
                    txtEB.Text = m.emission.B.ToString();
                    m.CopyTo(this.material);
                    this.selected_material = m;
                    txtShine.Text = m.shininess.ToString();
                    txtTexture.Text = m.texture_filename;
                    txtBumpMap.Text = m.bumpmap_filename;
                    txtName.Text = m.name;
                    this.mvDefault.SetMaterial(this.material);
                    int i=0;
                    for(i=0;i<tsMatSel.Items.Count;i++){
                         if(tsMatSel.Items[i].ToString() == name){
                             break;
                         }
                    }
                    this.tsMatSel.SelectedIndex = i;
                    break;
                }
            }
            
        }

        private void MaterialEditor_Load(object sender, EventArgs e)
        {
            UpdateMaterialList();

            SelectMaterial(tsMatSel.Items[tsMatSel.Items.Count - 1].ToString());

            // Extract first material
            // Update controls
        }
        private void txtAR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.ambient.R = (float)Convert.ToDouble(txtAR.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtAR.BackColor = Color.White;
            }
            catch
            {
                this.txtAR.BackColor = Color.Red;
            }
        }
        private void txtAG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.ambient.G = (float)Convert.ToDouble(txtAG.Text.Replace(".",","));
                this.mvDefault.SetMaterial(this.material);
                this.txtAG.BackColor = Color.White;
            }
            catch
            {
                this.txtAG.BackColor = Color.Red;
            }
        }
        private void txtAA_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.ambient.A = (float)Convert.ToDouble(txtAA.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtAA.BackColor = Color.White;
            }
            catch
            {
                this.txtAA.BackColor = Color.Red;
            }
        }
        private void txtAB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.ambient.B = (float)Convert.ToDouble(txtAB.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtAB.BackColor = Color.White;
            }
            catch
            {
                this.txtAB.BackColor = Color.Red;
            }
        }
        private void txtSR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.specular.R = (float)Convert.ToDouble(txtSR.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtSR.BackColor = Color.White;
            }
            catch
            {
                this.txtSR.BackColor = Color.Red;
            }
        }
        private void txtSG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.specular.G = (float)Convert.ToDouble(txtSG.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtSG.BackColor = Color.White;
            }
            catch
            {
                this.txtSG.BackColor = Color.Red;
            }
        }
        private void txtSB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.specular.B = (float)Convert.ToDouble(txtSB.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtSB.BackColor = Color.White;
            }
            catch
            {
                this.txtSB.BackColor = Color.Red;
            }
        }
        private void txtSA_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.specular.A = (float)Convert.ToDouble(txtSA.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtSA.BackColor = Color.White;
            }
            catch
            {
                this.txtSA.BackColor = Color.Red;
            }
        }
        private void txtShine_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.shininess = (float)Convert.ToDouble(txtShine.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtShine.BackColor = Color.White;
            }
            catch
            {
                this.txtShine.BackColor = Color.Red;
            }
        }
        private void txtDR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.diffuse.R = (float)Convert.ToDouble(txtDR.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtDR.BackColor = Color.White;
            }
            catch
            {
                this.txtDR.BackColor = Color.Red;
            }
        }
        private void txtDG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.diffuse.G = (float)Convert.ToDouble(txtDG.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtDG.BackColor = Color.White;
            }
            catch
            {
                this.txtDG.BackColor = Color.Red;
            }
        }
        private void txtDB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.diffuse.B = (float)Convert.ToDouble(txtDB.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtDB.BackColor = Color.White;
            }
            catch
            {
                this.txtDB.BackColor = Color.Red;
            }
        }
        private void txtDA_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.diffuse.A = (float)Convert.ToDouble(txtDA.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtDA.BackColor = Color.White;
            }
            catch
            {
                this.txtDA.BackColor = Color.Red;
            }
        }
        private void txtER_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.emission.R = (float)Convert.ToDouble(txtER.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtER.BackColor = Color.White;
            }
            catch
            {
                this.txtER.BackColor = Color.Red;
            }
        }
        private void txtEG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.emission.G = (float)Convert.ToDouble(txtEG.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtEG.BackColor = Color.White;
            }
            catch
            {
                this.txtEG.BackColor = Color.Red;
            }
        }
        private void txtEB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.emission.B = (float)Convert.ToDouble(txtEB.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtEB.BackColor = Color.White;
            }
            catch
            {
                this.txtEB.BackColor = Color.Red;
            }
        }
        private void txtEA_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.material.emission.A = (float)Convert.ToDouble(txtEA.Text.Replace(".", ","));
                this.mvDefault.SetMaterial(this.material);
                this.txtEA.BackColor = Color.White;
            }
            catch
            {
                this.txtEA.BackColor = Color.Red;
            }
        }
        private void tsMatSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectMaterial(tsMatSel.SelectedItem.ToString());
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            NewMaterial();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool mf = false;
            foreach (Material m in lstMaterial) if (m.name.ToLower().Trim() == this.material.name.ToLower().Trim()) { mf = true; break; }
            if (!mf)
            {
                if (this.material.name != this.selected_material.name)
                {
                    foreach (Block b in lstBlocks) if (b.Material == this.selected_material.name) b.Material = this.material.name;
                }
                this.material.CopyTo(this.selected_material);
                this.UpdateMaterialList();
                this.SelectMaterial(this.material.name);
            }
            else
            {
                MessageBox.Show("Could not apply settings, a material " + Environment.NewLine + "with this name already exist.","Failed to apply changes");
            }
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.material.name = txtName.Text;
        }
        private void txtTexture_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.material.LoadTexture(txtTexture.Text.Trim()))
                {
                    this.mvDefault.SetMaterial(this.material);
                    this.txtTexture.BackColor = Color.White;
                }
                else
                {
                    this.txtTexture.BackColor = Color.Red;
                }
            }
            catch
            {
                this.txtTexture.BackColor = Color.Red;
            }
        }
        private void txtBumpMap_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.material.LoadBumpMap(txtTexture.Text.Trim()))
                {
                    this.mvDefault.SetMaterial(this.material);
                    this.txtBumpMap.BackColor = Color.White;
                }
                else
                {
                    this.txtBumpMap.BackColor = Color.Red;
                }
            }
            catch
            {
                this.txtBumpMap.BackColor = Color.Red;
            }
        }
        private void tsMatSel_Click(object sender, EventArgs e)
        {

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // - Delete material
            // Check if more then 1 material is present
            if(lstMaterial.Count>1){
                bool mf = false;
                foreach (Block i in lstBlocks) if (i.Material == this.material.name) { mf = true; break; }
                if (!mf) { lstMaterial.Remove(this.selected_material); } else { MessageBox.Show("Material is used in the scene", "Failed to delete material"); };
                UpdateMaterialList();
                SelectMaterial(tsMatSel.Items[tsMatSel.Items.Count - 1].ToString());
            } else {
                MessageBox.Show("You have to have at least one material left in the inventory.","Delete failed");
            }

        }

    }
}
