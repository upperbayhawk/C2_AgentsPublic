namespace PowerMan
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
            this.labelServiceName = new System.Windows.Forms.Label();
            this.ServiceName = new System.Windows.Forms.Label();
            this.buttonDeactivate = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonActivate = new System.Windows.Forms.Button();
            this.textBoxPeriodSecs = new System.Windows.Forms.TextBox();
            this.labelPeriodHours = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelClusterName = new System.Windows.Forms.Label();
            this.textBoxPowerManEnabled = new System.Windows.Forms.TextBox();
            this.labelPowerManEnabled = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.ClusterName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelServiceName
            // 
            this.labelServiceName.AutoSize = true;
            this.labelServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServiceName.Location = new System.Drawing.Point(160, 13);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(0, 22);
            this.labelServiceName.TabIndex = 78;
            // 
            // ServiceName
            // 
            this.ServiceName.AutoSize = true;
            this.ServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServiceName.Location = new System.Drawing.Point(28, 13);
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.Size = new System.Drawing.Size(122, 22);
            this.ServiceName.TabIndex = 77;
            this.ServiceName.Text = "ServiceName:";
            // 
            // buttonDeactivate
            // 
            this.buttonDeactivate.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonDeactivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeactivate.Location = new System.Drawing.Point(726, 169);
            this.buttonDeactivate.Name = "buttonDeactivate";
            this.buttonDeactivate.Size = new System.Drawing.Size(146, 46);
            this.buttonDeactivate.TabIndex = 76;
            this.buttonDeactivate.Text = "Deactivate";
            this.buttonDeactivate.UseVisualStyleBackColor = false;
            this.buttonDeactivate.Click += new System.EventHandler(this.buttonDeactivate_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStop.Location = new System.Drawing.Point(520, 169);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(141, 45);
            this.buttonStop.TabIndex = 75;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRun.Location = new System.Drawing.Point(312, 167);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(151, 47);
            this.buttonRun.TabIndex = 74;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = false;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonActivate
            // 
            this.buttonActivate.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonActivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonActivate.Location = new System.Drawing.Point(102, 165);
            this.buttonActivate.Name = "buttonActivate";
            this.buttonActivate.Size = new System.Drawing.Size(151, 50);
            this.buttonActivate.TabIndex = 73;
            this.buttonActivate.Text = "Activate";
            this.buttonActivate.UseVisualStyleBackColor = false;
            this.buttonActivate.Click += new System.EventHandler(this.buttonActivate_Click);
            // 
            // textBoxPeriodSecs
            // 
            this.textBoxPeriodSecs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPeriodSecs.Location = new System.Drawing.Point(526, 351);
            this.textBoxPeriodSecs.Name = "textBoxPeriodSecs";
            this.textBoxPeriodSecs.Size = new System.Drawing.Size(62, 28);
            this.textBoxPeriodSecs.TabIndex = 59;
            this.textBoxPeriodSecs.TextChanged += new System.EventHandler(this.textBoxPeriodSecs_TextChanged);
            // 
            // labelPeriodHours
            // 
            this.labelPeriodHours.AutoSize = true;
            this.labelPeriodHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeriodHours.Location = new System.Drawing.Point(379, 351);
            this.labelPeriodHours.Name = "labelPeriodHours";
            this.labelPeriodHours.Size = new System.Drawing.Size(124, 22);
            this.labelPeriodHours.TabIndex = 58;
            this.labelPeriodHours.Text = "Period (Secs):";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(160, 57);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(0, 22);
            this.labelVersion.TabIndex = 57;
            // 
            // labelClusterName
            // 
            this.labelClusterName.AutoSize = true;
            this.labelClusterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClusterName.Location = new System.Drawing.Point(160, 35);
            this.labelClusterName.Name = "labelClusterName";
            this.labelClusterName.Size = new System.Drawing.Size(0, 22);
            this.labelClusterName.TabIndex = 56;
            // 
            // textBoxPowerManEnabled
            // 
            this.textBoxPowerManEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPowerManEnabled.Location = new System.Drawing.Point(526, 310);
            this.textBoxPowerManEnabled.Name = "textBoxPowerManEnabled";
            this.textBoxPowerManEnabled.Size = new System.Drawing.Size(100, 28);
            this.textBoxPowerManEnabled.TabIndex = 53;
            this.textBoxPowerManEnabled.TextChanged += new System.EventHandler(this.textboxPowerManEnabled);
            // 
            // labelPowerManEnabled
            // 
            this.labelPowerManEnabled.AutoSize = true;
            this.labelPowerManEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPowerManEnabled.Location = new System.Drawing.Point(337, 313);
            this.labelPowerManEnabled.Name = "labelPowerManEnabled";
            this.labelPowerManEnabled.Size = new System.Drawing.Size(166, 22);
            this.labelPowerManEnabled.TabIndex = 52;
            this.labelPowerManEnabled.Text = "PowerManEnabled:";
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version.Location = new System.Drawing.Point(70, 57);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(76, 22);
            this.Version.TabIndex = 45;
            this.Version.Text = "Version:";
            // 
            // ClusterName
            // 
            this.ClusterName.AutoSize = true;
            this.ClusterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClusterName.Location = new System.Drawing.Point(28, 35);
            this.ClusterName.Name = "ClusterName";
            this.ClusterName.Size = new System.Drawing.Size(119, 22);
            this.ClusterName.TabIndex = 44;
            this.ClusterName.Text = "ClusterName:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(402, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 42);
            this.label1.TabIndex = 43;
            this.label1.Text = "PowerMan";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 608);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.ServiceName);
            this.Controls.Add(this.buttonDeactivate);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonActivate);
            this.Controls.Add(this.textBoxPeriodSecs);
            this.Controls.Add(this.labelPeriodHours);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelClusterName);
            this.Controls.Add(this.textBoxPowerManEnabled);
            this.Controls.Add(this.labelPowerManEnabled);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.ClusterName);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelServiceName;
        private System.Windows.Forms.Label ServiceName;
        private System.Windows.Forms.Button buttonDeactivate;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonActivate;
        private System.Windows.Forms.TextBox textBoxPeriodSecs;
        private System.Windows.Forms.Label labelPeriodHours;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelClusterName;
        private System.Windows.Forms.TextBox textBoxPowerManEnabled;
        private System.Windows.Forms.Label labelPowerManEnabled;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Label ClusterName;
        private System.Windows.Forms.Label label1;
    }
}

