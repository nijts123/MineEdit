﻿/**
 * Copyright (c) 2010, Rob "N3X15" Nelson <nexis@7chan.org>
 *  All rights reserved.
 *
 *  Redistribution and use in source and binary forms, with or without 
 *  modification, are permitted provided that the following conditions are met:
 *
 *    * Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 *    * Redistributions in binary form must reproduce the above copyright 
 *      notice, this list of conditions and the following disclaimer in the 
 *      documentation and/or other materials provided with the distribution.
 *    * Neither the name of MineEdit nor the names of its contributors 
 *      may be used to endorse or promote products derived from this software 
 *      without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenMinecraft;

namespace MineEdit
{
    public class dlgTerrainGen : Form
    {
        IMapHandler mh = null;
        private GroupBox groupBox1;
        private CheckBox chkTrees;
        private CheckBox chkDungeons;
        private CheckBox chkGenOres;
        private CheckBox chkGenWater;
        private CheckBox chkCaves;
        private CheckBox chkRegen;
        public PropertyGrid pgMapGen;
        private ComboBox cmbMapGenSel;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label1;
        private Button cmdCancel;
        private GroupBox grpMaterials;
        private Label lblPresets;
        private ComboBox cmbMatPreset;
        private Button cmdOK;
        private int TemperatureOffset;
        private MapGenMaterials mMaterials=new MapGenMaterials();
        public dlgTerrainGen(IMapHandler _mh)
        {
            mh = _mh;
            InitializeComponent();
            cmbMapGenSel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cmbMapGenSel.DrawItem += new DrawItemEventHandler(cmbMapGenSel_DrawItem);
            LockCheckboxes(true);
            DoPreset();
        }

        void cmbMapGenSel_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            if (e.Index >= 0)
            {
                e.DrawBackground();
                KeyValuePair<string,string> k = (KeyValuePair<string,string>)cmbMapGenSel.Items[e.Index];
                g.DrawString(k.Value, this.Font, new SolidBrush(Color.Black), e.Bounds);
            }
        }

        private void dlgTerrainGen_Load(object sender, EventArgs e)
        {
            MapGenerators.Init();
            cmbMapGenSel.Items.Clear();
            Dictionary<string, string> items = MapGenerators.GetList();
            foreach (KeyValuePair<string, string> k in items)
            {
                cmbMapGenSel.Items.Add(k);
            }
        }

        private void cmbMapGenSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMapGenSel.SelectedItem != null)
            {
                pgMapGen.SelectedObject = MapGenerators.Get(((KeyValuePair<string, string>)cmbMapGenSel.SelectedItem).Key, mh.RandomSeed, mMaterials);
                LockCheckboxes(false);
            }
            else
                LockCheckboxes(true);
            ResetEverything();
        }

        private void LockCheckboxes(bool p)
        {
            chkCaves.Enabled = !p;
            chkDungeons.Enabled = !p;
            chkGenOres.Enabled = !p;
            chkGenWater.Enabled = !p;
            chkRegen.Enabled = !p;
            chkTrees.Enabled = !p;
            pgMapGen.Enabled = !p;
        }

        private void ResetEverything()
        {
            if (pgMapGen.SelectedObject == null) 
                return;
            chkCaves.Checked=(pgMapGen.SelectedObject as IMapGenerator).GenerateCaves;
            chkDungeons.Checked = (pgMapGen.SelectedObject as IMapGenerator).GenerateDungeons;
            chkGenOres.Checked = (pgMapGen.SelectedObject as IMapGenerator).GenerateOres;
            chkGenWater.Checked = (pgMapGen.SelectedObject as IMapGenerator).GenerateWater;
            chkRegen.Checked = (pgMapGen.SelectedObject as IMapGenerator).NoPreservation;
            chkTrees.Checked = (pgMapGen.SelectedObject as IMapGenerator).GenerateTrees;
        }

        private void chkRegen_CheckedChanged(object sender, EventArgs e)
        {
            if (pgMapGen.SelectedObject == null)
                return;
            (pgMapGen.SelectedObject as IMapGenerator).NoPreservation = chkRegen.Checked;
        }

        private void chkCaves_CheckedChanged(object sender, EventArgs e)
        {
            if (pgMapGen.SelectedObject == null)
                return;
            (pgMapGen.SelectedObject as IMapGenerator).GenerateCaves = chkCaves.Checked;
        }

        private void chkGenWater_CheckedChanged(object sender, EventArgs e)
        {
            if (pgMapGen.SelectedObject == null)
                return;
            (pgMapGen.SelectedObject as IMapGenerator).GenerateWater = chkGenWater.Checked;
        }

        private void chkGenOres_CheckedChanged(object sender, EventArgs e)
        {
            if (pgMapGen.SelectedObject == null)
                return;
            (pgMapGen.SelectedObject as IMapGenerator).GenerateOres = chkGenOres.Checked;
        }

        private void chkDungeons_CheckedChanged(object sender, EventArgs e)
        {
            if (pgMapGen.SelectedObject == null)
                return;
            (pgMapGen.SelectedObject as IMapGenerator).GenerateDungeons = chkDungeons.Checked;
        }

        private void chkTrees_CheckedChanged(object sender, EventArgs e)
        {
            if (pgMapGen.SelectedObject == null)
                return;
            (pgMapGen.SelectedObject as IMapGenerator).GenerateTrees = chkTrees.Checked;
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTrees = new System.Windows.Forms.CheckBox();
            this.chkDungeons = new System.Windows.Forms.CheckBox();
            this.chkGenOres = new System.Windows.Forms.CheckBox();
            this.chkGenWater = new System.Windows.Forms.CheckBox();
            this.chkCaves = new System.Windows.Forms.CheckBox();
            this.chkRegen = new System.Windows.Forms.CheckBox();
            this.pgMapGen = new System.Windows.Forms.PropertyGrid();
            this.cmbMapGenSel = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.grpMaterials = new System.Windows.Forms.GroupBox();
            this.lblPresets = new System.Windows.Forms.Label();
            this.cmbMatPreset = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpMaterials.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkTrees);
            this.groupBox1.Controls.Add(this.chkDungeons);
            this.groupBox1.Controls.Add(this.chkGenOres);
            this.groupBox1.Controls.Add(this.chkGenWater);
            this.groupBox1.Controls.Add(this.chkCaves);
            this.groupBox1.Controls.Add(this.chkRegen);
            this.groupBox1.Location = new System.Drawing.Point(12, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 197);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Common Settings";
            // 
            // chkTrees
            // 
            this.chkTrees.AutoSize = true;
            this.chkTrees.Enabled = false;
            this.chkTrees.Location = new System.Drawing.Point(6, 173);
            this.chkTrees.Name = "chkTrees";
            this.chkTrees.Size = new System.Drawing.Size(75, 17);
            this.chkTrees.TabIndex = 5;
            this.chkTrees.Text = "Add Trees";
            this.chkTrees.UseVisualStyleBackColor = true;
            this.chkTrees.CheckedChanged += new System.EventHandler(this.chkTrees_CheckedChanged);
            // 
            // chkDungeons
            // 
            this.chkDungeons.AutoSize = true;
            this.chkDungeons.Enabled = false;
            this.chkDungeons.Location = new System.Drawing.Point(6, 150);
            this.chkDungeons.Name = "chkDungeons";
            this.chkDungeons.Size = new System.Drawing.Size(122, 17);
            this.chkDungeons.TabIndex = 4;
            this.chkDungeons.Text = "Generate Dungeons";
            this.chkDungeons.UseVisualStyleBackColor = true;
            this.chkDungeons.CheckedChanged += new System.EventHandler(this.chkDungeons_CheckedChanged);
            // 
            // chkGenOres
            // 
            this.chkGenOres.AutoSize = true;
            this.chkGenOres.Enabled = false;
            this.chkGenOres.Location = new System.Drawing.Point(6, 127);
            this.chkGenOres.Name = "chkGenOres";
            this.chkGenOres.Size = new System.Drawing.Size(135, 17);
            this.chkGenOres.TabIndex = 3;
            this.chkGenOres.Text = "Generate Ores/Springs";
            this.chkGenOres.UseVisualStyleBackColor = true;
            this.chkGenOres.CheckedChanged += new System.EventHandler(this.chkGenOres_CheckedChanged);
            // 
            // chkGenWater
            // 
            this.chkGenWater.AutoSize = true;
            this.chkGenWater.Enabled = false;
            this.chkGenWater.Location = new System.Drawing.Point(6, 65);
            this.chkGenWater.Name = "chkGenWater";
            this.chkGenWater.Size = new System.Drawing.Size(194, 56);
            this.chkGenWater.TabIndex = 2;
            this.chkGenWater.Text = "Generate Water\r\n\r\n(Will still add random springs unless \r\n\"Generate Ores\" is unch" +
                "ecked)";
            this.chkGenWater.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkGenWater.UseVisualStyleBackColor = true;
            this.chkGenWater.CheckedChanged += new System.EventHandler(this.chkGenWater_CheckedChanged);
            // 
            // chkCaves
            // 
            this.chkCaves.AutoSize = true;
            this.chkCaves.Enabled = false;
            this.chkCaves.Location = new System.Drawing.Point(6, 42);
            this.chkCaves.Name = "chkCaves";
            this.chkCaves.Size = new System.Drawing.Size(103, 17);
            this.chkCaves.TabIndex = 1;
            this.chkCaves.Text = "Generate Caves";
            this.chkCaves.UseVisualStyleBackColor = true;
            this.chkCaves.CheckedChanged += new System.EventHandler(this.chkCaves_CheckedChanged);
            // 
            // chkRegen
            // 
            this.chkRegen.AutoSize = true;
            this.chkRegen.Enabled = false;
            this.chkRegen.Location = new System.Drawing.Point(6, 19);
            this.chkRegen.Name = "chkRegen";
            this.chkRegen.Size = new System.Drawing.Size(158, 17);
            this.chkRegen.TabIndex = 0;
            this.chkRegen.Text = "Regenerate EVERYTHING.";
            this.chkRegen.UseVisualStyleBackColor = true;
            this.chkRegen.CheckedChanged += new System.EventHandler(this.chkRegen_CheckedChanged);
            // 
            // pgMapGen
            // 
            this.pgMapGen.Location = new System.Drawing.Point(294, 73);
            this.pgMapGen.Name = "pgMapGen";
            this.pgMapGen.Size = new System.Drawing.Size(370, 313);
            this.pgMapGen.TabIndex = 2;
            // 
            // cmbMapGenSel
            // 
            this.cmbMapGenSel.FormattingEnabled = true;
            this.cmbMapGenSel.Location = new System.Drawing.Point(12, 73);
            this.cmbMapGenSel.Name = "cmbMapGenSel";
            this.cmbMapGenSel.Size = new System.Drawing.Size(276, 21);
            this.cmbMapGenSel.TabIndex = 3;
            this.cmbMapGenSel.SelectedIndexChanged += new System.EventHandler(this.cmbMapGenSel_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 67);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::MineEdit.Properties.Resources.Terragen_Logo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(73, 67);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Create your own terrain. Fight the power etc.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(86, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Terrain Generation Setup";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(589, 407);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(508, 407);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // grpMaterials
            // 
            this.grpMaterials.Controls.Add(this.lblPresets);
            this.grpMaterials.Controls.Add(this.cmbMatPreset);
            this.grpMaterials.Location = new System.Drawing.Point(12, 304);
            this.grpMaterials.Name = "grpMaterials";
            this.grpMaterials.Size = new System.Drawing.Size(276, 82);
            this.grpMaterials.TabIndex = 6;
            this.grpMaterials.TabStop = false;
            this.grpMaterials.Text = "Materials";
            // 
            // lblPresets
            // 
            this.lblPresets.AutoSize = true;
            this.lblPresets.Location = new System.Drawing.Point(36, 22);
            this.lblPresets.Name = "lblPresets";
            this.lblPresets.Size = new System.Drawing.Size(45, 13);
            this.lblPresets.TabIndex = 1;
            this.lblPresets.Text = "Presets:";
            // 
            // cmbMatPreset
            // 
            this.cmbMatPreset.FormattingEnabled = true;
            this.cmbMatPreset.Location = new System.Drawing.Point(90, 19);
            this.cmbMatPreset.Name = "cmbMatPreset";
            this.cmbMatPreset.Size = new System.Drawing.Size(180, 21);
            this.cmbMatPreset.TabIndex = 0;
            this.cmbMatPreset.SelectedIndexChanged += new System.EventHandler(this.cmbMatPreset_SelectedIndexChanged);
            // 
            // dlgTerrainGen
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(676, 442);
            this.Controls.Add(this.grpMaterials);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmbMapGenSel);
            this.Controls.Add(this.pgMapGen);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgTerrainGen";
            this.ShowIcon = false;
            this.Text = "Terrain Generation Setup";
            this.Load += new System.EventHandler(this.dlgTerrainGen_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpMaterials.ResumeLayout(false);
            this.grpMaterials.PerformLayout();
            this.ResumeLayout(false);

        }

        private void cmbMatPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoPreset();
        }

        private void DoPreset()
        {
            mMaterials=new MapGenMaterials();
            if (cmbMatPreset.SelectedIndex != -1)
            {
                switch (cmbMatPreset.SelectedText)
                {
                    case "Hell Mode":
                        mMaterials.Grass=mMaterials.Soil;
                        mMaterials.Sand=mMaterials.Gravel=49; // Obsidian
                        mMaterials.Water=mMaterials.Lava;
                        mMaterials.Snow=0; // FUCK snow.
                        TemperatureOffset = 30;
                        break;
                    default:
                        break;
                }
            }
            if (pgMapGen.SelectedObject != null)
            {
                (pgMapGen.SelectedObject as IMapGenerator).Materials = mMaterials;
            }
        }
    }
}
