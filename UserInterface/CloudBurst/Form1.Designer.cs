namespace CloudBurst
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
            this.labelSlices = new System.Windows.Forms.Label();
            this.lblSlices = new System.Windows.Forms.Label();
            this.labelSamples = new System.Windows.Forms.Label();
            this.lblSamples = new System.Windows.Forms.Label();
            this.labelServiceName = new System.Windows.Forms.Label();
            this.ServiceName = new System.Windows.Forms.Label();
            this.textBoxSlice = new System.Windows.Forms.TextBox();
            this.labelSlice = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelClusterName = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.ClusterName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GetAllSoilRecords = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.buttonGetDataSlice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSlices
            // 
            this.labelSlices.AutoSize = true;
            this.labelSlices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlices.Location = new System.Drawing.Point(749, 132);
            this.labelSlices.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSlices.Name = "labelSlices";
            this.labelSlices.Size = new System.Drawing.Size(20, 22);
            this.labelSlices.TabIndex = 81;
            this.labelSlices.Text = "0";
            this.labelSlices.Click += new System.EventHandler(this.labelSlices_Click);
            // 
            // lblSlices
            // 
            this.lblSlices.AutoSize = true;
            this.lblSlices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlices.Location = new System.Drawing.Point(682, 132);
            this.lblSlices.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSlices.Name = "lblSlices";
            this.lblSlices.Size = new System.Drawing.Size(63, 22);
            this.lblSlices.TabIndex = 80;
            this.lblSlices.Text = "Slices:";
            // 
            // labelSamples
            // 
            this.labelSamples.AutoSize = true;
            this.labelSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSamples.Location = new System.Drawing.Point(749, 110);
            this.labelSamples.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSamples.Name = "labelSamples";
            this.labelSamples.Size = new System.Drawing.Size(20, 22);
            this.labelSamples.TabIndex = 79;
            this.labelSamples.Text = "0";
            this.labelSamples.Click += new System.EventHandler(this.labelSamples_Click);
            // 
            // lblSamples
            // 
            this.lblSamples.AutoSize = true;
            this.lblSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSamples.Location = new System.Drawing.Point(661, 110);
            this.lblSamples.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSamples.Name = "lblSamples";
            this.lblSamples.Size = new System.Drawing.Size(84, 22);
            this.lblSamples.TabIndex = 78;
            this.lblSamples.Text = "Samples:";
            // 
            // labelServiceName
            // 
            this.labelServiceName.AutoSize = true;
            this.labelServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServiceName.Location = new System.Drawing.Point(62, -75);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(0, 22);
            this.labelServiceName.TabIndex = 77;
            // 
            // ServiceName
            // 
            this.ServiceName.AutoSize = true;
            this.ServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServiceName.Location = new System.Drawing.Point(-70, -75);
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.Size = new System.Drawing.Size(122, 22);
            this.ServiceName.TabIndex = 76;
            this.ServiceName.Text = "ServiceName:";
            // 
            // textBoxSlice
            // 
            this.textBoxSlice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSlice.Location = new System.Drawing.Point(155, 264);
            this.textBoxSlice.Name = "textBoxSlice";
            this.textBoxSlice.Size = new System.Drawing.Size(61, 28);
            this.textBoxSlice.TabIndex = 70;
            this.textBoxSlice.Text = "0";
            this.textBoxSlice.TextChanged += new System.EventHandler(this.textBoxSlice_TextChanged);
            // 
            // labelSlice
            // 
            this.labelSlice.AutoSize = true;
            this.labelSlice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlice.Location = new System.Drawing.Point(80, 267);
            this.labelSlice.Name = "labelSlice";
            this.labelSlice.Size = new System.Drawing.Size(54, 22);
            this.labelSlice.TabIndex = 69;
            this.labelSlice.Text = "Slice:";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(151, 20);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(0, 22);
            this.labelVersion.TabIndex = 56;
            // 
            // labelClusterName
            // 
            this.labelClusterName.AutoSize = true;
            this.labelClusterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClusterName.Location = new System.Drawing.Point(63, -53);
            this.labelClusterName.Name = "labelClusterName";
            this.labelClusterName.Size = new System.Drawing.Size(0, 22);
            this.labelClusterName.TabIndex = 55;
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version.Location = new System.Drawing.Point(61, 20);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(76, 22);
            this.Version.TabIndex = 44;
            this.Version.Text = "Version:";
            this.Version.Click += new System.EventHandler(this.Version_Click);
            // 
            // ClusterName
            // 
            this.ClusterName.AutoSize = true;
            this.ClusterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClusterName.Location = new System.Drawing.Point(-70, -53);
            this.ClusterName.Name = "ClusterName";
            this.ClusterName.Size = new System.Drawing.Size(119, 22);
            this.ClusterName.TabIndex = 43;
            this.ClusterName.Text = "ClusterName:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(324, -75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 38);
            this.label1.TabIndex = 42;
            this.label1.Text = "RainMan";
            // 
            // GetAllSoilRecords
            // 
            this.GetAllSoilRecords.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GetAllSoilRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetAllSoilRecords.Location = new System.Drawing.Point(301, 96);
            this.GetAllSoilRecords.Name = "GetAllSoilRecords";
            this.GetAllSoilRecords.Size = new System.Drawing.Size(292, 87);
            this.GetAllSoilRecords.TabIndex = 41;
            this.GetAllSoilRecords.Text = "GetSoilData";
            this.GetAllSoilRecords.UseVisualStyleBackColor = false;
            this.GetAllSoilRecords.Click += new System.EventHandler(this.GetAllSoilRecords_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(342, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(224, 42);
            this.lblTitle.TabIndex = 82;
            this.lblTitle.Text = "Cloud Burst";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(154, 25);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(0, 22);
            this.lblVersion.TabIndex = 83;
            this.lblVersion.Click += new System.EventHandler(this.lblVersion_Click);
            // 
            // buttonGetDataSlice
            // 
            this.buttonGetDataSlice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetDataSlice.Location = new System.Drawing.Point(330, 254);
            this.buttonGetDataSlice.Name = "buttonGetDataSlice";
            this.buttonGetDataSlice.Size = new System.Drawing.Size(224, 47);
            this.buttonGetDataSlice.TabIndex = 84;
            this.buttonGetDataSlice.Text = "GetDataSlice";
            this.buttonGetDataSlice.UseVisualStyleBackColor = true;
            this.buttonGetDataSlice.Click += new System.EventHandler(this.buttonGetDataSlice_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 335);
            this.Controls.Add(this.buttonGetDataSlice);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.labelSlices);
            this.Controls.Add(this.lblSlices);
            this.Controls.Add(this.labelSamples);
            this.Controls.Add(this.lblSamples);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.ServiceName);
            this.Controls.Add(this.textBoxSlice);
            this.Controls.Add(this.labelSlice);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelClusterName);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.ClusterName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GetAllSoilRecords);
            this.Name = "Form1";
            this.Text = "Soil Data Download";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSlices;
        private System.Windows.Forms.Label lblSlices;
        private System.Windows.Forms.Label labelSamples;
        private System.Windows.Forms.Label lblSamples;
        private System.Windows.Forms.Label labelServiceName;
        private System.Windows.Forms.Label ServiceName;
        private System.Windows.Forms.TextBox textBoxSlice;
        private System.Windows.Forms.Label labelSlice;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelClusterName;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Label ClusterName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GetAllSoilRecords;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button buttonGetDataSlice;
    }
}

