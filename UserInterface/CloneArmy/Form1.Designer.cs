//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
namespace CurrentForCarbon
{
    partial class Form1
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
            this.serviceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip3 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonActivateClones = new System.Windows.Forms.Button();
            this.buttonStartClones = new System.Windows.Forms.Button();
            this.buttonStopClones = new System.Windows.Forms.Button();
            this.buttonDeactivateClones = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.buttonClone = new System.Windows.Forms.Button();
            this.textBoxStartingNumber = new System.Windows.Forms.TextBox();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonRestore = new System.Windows.Forms.Button();
            this.labelCluster = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelServiceName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // serviceMenuItem
            // 
            this.serviceMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installServiceMenuItem,
            this.startServiceMenuItem,
            this.stopServiceMenuItem,
            this.uninstallServiceMenuItem});
            this.serviceMenuItem.Name = "serviceMenuItem";
            this.serviceMenuItem.Size = new System.Drawing.Size(56, 20);
            this.serviceMenuItem.Text = "Service";
            // 
            // installServiceMenuItem
            // 
            this.installServiceMenuItem.Name = "installServiceMenuItem";
            this.installServiceMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // startServiceMenuItem
            // 
            this.startServiceMenuItem.Name = "startServiceMenuItem";
            this.startServiceMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // stopServiceMenuItem
            // 
            this.stopServiceMenuItem.Name = "stopServiceMenuItem";
            this.stopServiceMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // uninstallServiceMenuItem
            // 
            this.uninstallServiceMenuItem.Name = "uninstallServiceMenuItem";
            this.uninstallServiceMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // menuStrip3
            // 
            this.menuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.serviceMenuItem});
            this.menuStrip3.Location = new System.Drawing.Point(0, 0);
            this.menuStrip3.Name = "menuStrip3";
            this.menuStrip3.Size = new System.Drawing.Size(976, 24);
            this.menuStrip3.TabIndex = 2;
            this.menuStrip3.Text = "menuStrip3";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // buttonActivateClones
            // 
            this.buttonActivateClones.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonActivateClones.Location = new System.Drawing.Point(48, 357);
            this.buttonActivateClones.Name = "buttonActivateClones";
            this.buttonActivateClones.Size = new System.Drawing.Size(205, 37);
            this.buttonActivateClones.TabIndex = 3;
            this.buttonActivateClones.Text = "Activate Clones";
            this.buttonActivateClones.UseVisualStyleBackColor = true;
            this.buttonActivateClones.Click += new System.EventHandler(this.buttonActivateClones_Click);
            // 
            // buttonStartClones
            // 
            this.buttonStartClones.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStartClones.Location = new System.Drawing.Point(276, 357);
            this.buttonStartClones.Name = "buttonStartClones";
            this.buttonStartClones.Size = new System.Drawing.Size(190, 37);
            this.buttonStartClones.TabIndex = 4;
            this.buttonStartClones.Text = "Start Clones";
            this.buttonStartClones.UseVisualStyleBackColor = true;
            this.buttonStartClones.Click += new System.EventHandler(this.buttonStartClones_Click);
            // 
            // buttonStopClones
            // 
            this.buttonStopClones.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStopClones.Location = new System.Drawing.Point(485, 357);
            this.buttonStopClones.Name = "buttonStopClones";
            this.buttonStopClones.Size = new System.Drawing.Size(190, 37);
            this.buttonStopClones.TabIndex = 5;
            this.buttonStopClones.Text = "Stop Clones";
            this.buttonStopClones.UseVisualStyleBackColor = true;
            this.buttonStopClones.Click += new System.EventHandler(this.buttonStopClones_Click);
            // 
            // buttonDeactivateClones
            // 
            this.buttonDeactivateClones.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeactivateClones.Location = new System.Drawing.Point(692, 357);
            this.buttonDeactivateClones.Name = "buttonDeactivateClones";
            this.buttonDeactivateClones.Size = new System.Drawing.Size(232, 37);
            this.buttonDeactivateClones.TabIndex = 6;
            this.buttonDeactivateClones.Text = "Deactivate Clones";
            this.buttonDeactivateClones.UseVisualStyleBackColor = true;
            this.buttonDeactivateClones.Click += new System.EventHandler(this.buttonDeactivateClones_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(322, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Current For Carbon, The Game";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(395, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(194, 37);
            this.label13.TabIndex = 42;
            this.label13.Text = "Clone Army";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(102, 45);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(71, 24);
            this.labelVersion.TabIndex = 46;
            this.labelVersion.Text = "version";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(322, 106);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(188, 29);
            this.label14.TabIndex = 47;
            this.label14.Text = "Starting Number";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(322, 150);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 29);
            this.label16.TabIndex = 48;
            this.label16.Text = "Count";
            // 
            // buttonClone
            // 
            this.buttonClone.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClone.Location = new System.Drawing.Point(161, 203);
            this.buttonClone.Name = "buttonClone";
            this.buttonClone.Size = new System.Drawing.Size(174, 48);
            this.buttonClone.TabIndex = 49;
            this.buttonClone.Text = "CLONE";
            this.buttonClone.UseVisualStyleBackColor = true;
            this.buttonClone.Click += new System.EventHandler(this.buttonClone_Click);
            // 
            // textBoxStartingNumber
            // 
            this.textBoxStartingNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStartingNumber.Location = new System.Drawing.Point(539, 103);
            this.textBoxStartingNumber.Name = "textBoxStartingNumber";
            this.textBoxStartingNumber.Size = new System.Drawing.Size(63, 35);
            this.textBoxStartingNumber.TabIndex = 50;
            this.textBoxStartingNumber.Text = "100";
            this.textBoxStartingNumber.TextChanged += new System.EventHandler(this.textBoxStartingNumber_TextChanged);
            // 
            // textBoxCount
            // 
            this.textBoxCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCount.Location = new System.Drawing.Point(539, 144);
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(63, 35);
            this.textBoxCount.TabIndex = 51;
            this.textBoxCount.TabStop = false;
            this.textBoxCount.Text = "4";
            this.textBoxCount.TextChanged += new System.EventHandler(this.textBoxCount_TextChanged);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemove.Location = new System.Drawing.Point(644, 203);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(174, 48);
            this.buttonRemove.TabIndex = 52;
            this.buttonRemove.Text = "REMOVE";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpdate.Location = new System.Drawing.Point(402, 203);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(174, 48);
            this.buttonUpdate.TabIndex = 53;
            this.buttonUpdate.Text = "UPDATE";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(296, 274);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(174, 48);
            this.buttonSave.TabIndex = 54;
            this.buttonSave.Text = "SAVE CONFIG";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonRestore
            // 
            this.buttonRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestore.Location = new System.Drawing.Point(506, 274);
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.Size = new System.Drawing.Size(224, 48);
            this.buttonRestore.TabIndex = 55;
            this.buttonRestore.Text = "RESTORE CONFIG";
            this.buttonRestore.UseVisualStyleBackColor = true;
            this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
            // 
            // labelCluster
            // 
            this.labelCluster.AutoSize = true;
            this.labelCluster.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCluster.Location = new System.Drawing.Point(100, 21);
            this.labelCluster.Name = "labelCluster";
            this.labelCluster.Size = new System.Drawing.Size(65, 24);
            this.labelCluster.TabIndex = 62;
            this.labelCluster.Text = "cluster";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(25, 21);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 24);
            this.label18.TabIndex = 61;
            this.label18.Text = "Cluster";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 24);
            this.label4.TabIndex = 59;
            this.label4.Text = "Version";
            // 
            // labelServiceName
            // 
            this.labelServiceName.AutoSize = true;
            this.labelServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServiceName.Location = new System.Drawing.Point(102, 69);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(39, 24);
            this.labelServiceName.TabIndex = 58;
            this.labelServiceName.Text = "text";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(27, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 24);
            this.label5.TabIndex = 57;
            this.label5.Text = "Service";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 423);
            this.Controls.Add(this.labelCluster);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonRestore);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.textBoxStartingNumber);
            this.Controls.Add(this.buttonClone);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDeactivateClones);
            this.Controls.Add(this.buttonStopClones);
            this.Controls.Add(this.buttonStartClones);
            this.Controls.Add(this.buttonActivateClones);
            this.Controls.Add(this.menuStrip3);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Form1";
            this.Text = "CurrentForCarbon, The Real SaveThePlanet Game";
            this.menuStrip3.ResumeLayout(false);
            this.menuStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem serviceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installServiceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startServiceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopServiceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallServiceMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.Button buttonActivateClones;
        private System.Windows.Forms.Button buttonStartClones;
        private System.Windows.Forms.Button buttonStopClones;
        private System.Windows.Forms.Button buttonDeactivateClones;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button buttonClone;
        private System.Windows.Forms.TextBox textBoxStartingNumber;
        private System.Windows.Forms.TextBox textBoxCount;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonRestore;
        private System.Windows.Forms.Label labelCluster;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelServiceName;
        private System.Windows.Forms.Label label5;
    }
}

