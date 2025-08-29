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
            this.buttonActivatePlayer = new System.Windows.Forms.Button();
            this.buttonStartPlayer = new System.Windows.Forms.Button();
            this.buttonStopPlayer = new System.Windows.Forms.Button();
            this.buttonDeactivatePlayer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEthereumAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDataConnectString = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxEthereumKey = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerName = new System.Windows.Forms.TextBox();
            this.textBoxGamePlayerStreet = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPlayerIdentifier = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerCity = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerState = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerZipcode = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelServiceName = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerElectricCo = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.labelCluster = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxPlayerTimeZone = new System.Windows.Forms.TextBox();
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
            this.installServiceMenuItem.Size = new System.Drawing.Size(160, 22);
            this.installServiceMenuItem.Text = "Install Service";
            this.installServiceMenuItem.Click += new System.EventHandler(this.installServiceMenuItem_Click);
            // 
            // startServiceMenuItem
            // 
            this.startServiceMenuItem.Name = "startServiceMenuItem";
            this.startServiceMenuItem.Size = new System.Drawing.Size(160, 22);
            this.startServiceMenuItem.Text = "Start Service";
            this.startServiceMenuItem.Click += new System.EventHandler(this.startServiceMenuItem_Click);
            // 
            // stopServiceMenuItem
            // 
            this.stopServiceMenuItem.Name = "stopServiceMenuItem";
            this.stopServiceMenuItem.Size = new System.Drawing.Size(160, 22);
            this.stopServiceMenuItem.Text = "Stop Service";
            this.stopServiceMenuItem.Click += new System.EventHandler(this.stopServiceMenuItem_Click);
            // 
            // uninstallServiceMenuItem
            // 
            this.uninstallServiceMenuItem.Name = "uninstallServiceMenuItem";
            this.uninstallServiceMenuItem.Size = new System.Drawing.Size(160, 22);
            this.uninstallServiceMenuItem.Text = "Uninstall Service";
            this.uninstallServiceMenuItem.Click += new System.EventHandler(this.uninstallServiceMenuItem_Click);
            // 
            // menuStrip3
            // 
            this.menuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.serviceMenuItem});
            this.menuStrip3.Location = new System.Drawing.Point(0, 0);
            this.menuStrip3.Name = "menuStrip3";
            this.menuStrip3.Size = new System.Drawing.Size(985, 24);
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
            this.exitMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // buttonActivatePlayer
            // 
            this.buttonActivatePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonActivatePlayer.Location = new System.Drawing.Point(52, 103);
            this.buttonActivatePlayer.Name = "buttonActivatePlayer";
            this.buttonActivatePlayer.Size = new System.Drawing.Size(193, 37);
            this.buttonActivatePlayer.TabIndex = 1;
            this.buttonActivatePlayer.Text = "Activate Player";
            this.buttonActivatePlayer.UseVisualStyleBackColor = true;
            this.buttonActivatePlayer.Click += new System.EventHandler(this.buttonActivatePlayer_Click);
            // 
            // buttonStartPlayer
            // 
            this.buttonStartPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStartPlayer.Location = new System.Drawing.Point(278, 103);
            this.buttonStartPlayer.Name = "buttonStartPlayer";
            this.buttonStartPlayer.Size = new System.Drawing.Size(190, 37);
            this.buttonStartPlayer.TabIndex = 2;
            this.buttonStartPlayer.Text = "Start Playing";
            this.buttonStartPlayer.UseVisualStyleBackColor = true;
            this.buttonStartPlayer.Click += new System.EventHandler(this.buttonStartPlayer_Click);
            // 
            // buttonStopPlayer
            // 
            this.buttonStopPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStopPlayer.Location = new System.Drawing.Point(500, 103);
            this.buttonStopPlayer.Name = "buttonStopPlayer";
            this.buttonStopPlayer.Size = new System.Drawing.Size(190, 37);
            this.buttonStopPlayer.TabIndex = 3;
            this.buttonStopPlayer.Text = "Stop Playing";
            this.buttonStopPlayer.UseVisualStyleBackColor = true;
            this.buttonStopPlayer.Click += new System.EventHandler(this.buttonStopPlayer_Click);
            // 
            // buttonDeactivatePlayer
            // 
            this.buttonDeactivatePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeactivatePlayer.Location = new System.Drawing.Point(719, 103);
            this.buttonDeactivatePlayer.Name = "buttonDeactivatePlayer";
            this.buttonDeactivatePlayer.Size = new System.Drawing.Size(208, 37);
            this.buttonDeactivatePlayer.TabIndex = 4;
            this.buttonDeactivatePlayer.Text = "Deactivate Player";
            this.buttonDeactivatePlayer.UseVisualStyleBackColor = true;
            this.buttonDeactivatePlayer.Click += new System.EventHandler(this.buttonDeactivatePlayer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(338, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Current For Carbon, The Game";
            // 
            // textBoxEthereumAddress
            // 
            this.textBoxEthereumAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEthereumAddress.Location = new System.Drawing.Point(246, 395);
            this.textBoxEthereumAddress.Name = "textBoxEthereumAddress";
            this.textBoxEthereumAddress.ReadOnly = true;
            this.textBoxEthereumAddress.Size = new System.Drawing.Size(639, 26);
            this.textBoxEthereumAddress.TabIndex = 8;
            this.textBoxEthereumAddress.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(79, 395);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ethereum Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(78, 462);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Data Connect String";
            // 
            // textBoxDataConnectString
            // 
            this.textBoxDataConnectString.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDataConnectString.Location = new System.Drawing.Point(246, 459);
            this.textBoxDataConnectString.Name = "textBoxDataConnectString";
            this.textBoxDataConnectString.ReadOnly = true;
            this.textBoxDataConnectString.Size = new System.Drawing.Size(639, 26);
            this.textBoxDataConnectString.TabIndex = 12;
            this.textBoxDataConnectString.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(80, 430);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Ethereum Key";
            // 
            // textBoxEthereumKey
            // 
            this.textBoxEthereumKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEthereumKey.Location = new System.Drawing.Point(246, 427);
            this.textBoxEthereumKey.Name = "textBoxEthereumKey";
            this.textBoxEthereumKey.ReadOnly = true;
            this.textBoxEthereumKey.Size = new System.Drawing.Size(639, 26);
            this.textBoxEthereumKey.TabIndex = 16;
            this.textBoxEthereumKey.TabStop = false;
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
            // textBoxGamePlayerName
            // 
            this.textBoxGamePlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerName.Location = new System.Drawing.Point(247, 163);
            this.textBoxGamePlayerName.Name = "textBoxGamePlayerName";
            this.textBoxGamePlayerName.ReadOnly = true;
            this.textBoxGamePlayerName.Size = new System.Drawing.Size(638, 26);
            this.textBoxGamePlayerName.TabIndex = 30;
            this.textBoxGamePlayerName.TabStop = false;
            // 
            // textBoxGamePlayerStreet
            // 
            this.textBoxGamePlayerStreet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerStreet.Location = new System.Drawing.Point(246, 195);
            this.textBoxGamePlayerStreet.Name = "textBoxGamePlayerStreet";
            this.textBoxGamePlayerStreet.ReadOnly = true;
            this.textBoxGamePlayerStreet.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerStreet.TabIndex = 28;
            this.textBoxGamePlayerStreet.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(79, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 20);
            this.label7.TabIndex = 29;
            this.label7.Text = "Player Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(78, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 20);
            this.label8.TabIndex = 31;
            this.label8.Text = "Player Street";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(78, 291);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 20);
            this.label5.TabIndex = 33;
            this.label5.Text = "Player Identifier";
            // 
            // textBoxPlayerIdentifier
            // 
            this.textBoxPlayerIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerIdentifier.Location = new System.Drawing.Point(246, 291);
            this.textBoxPlayerIdentifier.Name = "textBoxPlayerIdentifier";
            this.textBoxPlayerIdentifier.ReadOnly = true;
            this.textBoxPlayerIdentifier.Size = new System.Drawing.Size(639, 26);
            this.textBoxPlayerIdentifier.TabIndex = 32;
            this.textBoxPlayerIdentifier.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(78, 227);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "Player City";
            // 
            // textBoxGamePlayerCity
            // 
            this.textBoxGamePlayerCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerCity.Location = new System.Drawing.Point(246, 227);
            this.textBoxGamePlayerCity.Name = "textBoxGamePlayerCity";
            this.textBoxGamePlayerCity.ReadOnly = true;
            this.textBoxGamePlayerCity.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerCity.TabIndex = 34;
            this.textBoxGamePlayerCity.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(78, 259);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 20);
            this.label10.TabIndex = 37;
            this.label10.Text = "Player State";
            // 
            // textBoxGamePlayerState
            // 
            this.textBoxGamePlayerState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerState.Location = new System.Drawing.Point(246, 259);
            this.textBoxGamePlayerState.Name = "textBoxGamePlayerState";
            this.textBoxGamePlayerState.ReadOnly = true;
            this.textBoxGamePlayerState.Size = new System.Drawing.Size(259, 26);
            this.textBoxGamePlayerState.TabIndex = 36;
            this.textBoxGamePlayerState.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(521, 259);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 20);
            this.label11.TabIndex = 39;
            this.label11.Text = "Player Zipcode";
            // 
            // textBoxGamePlayerZipcode
            // 
            this.textBoxGamePlayerZipcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerZipcode.Location = new System.Drawing.Point(664, 259);
            this.textBoxGamePlayerZipcode.Name = "textBoxGamePlayerZipcode";
            this.textBoxGamePlayerZipcode.ReadOnly = true;
            this.textBoxGamePlayerZipcode.Size = new System.Drawing.Size(221, 26);
            this.textBoxGamePlayerZipcode.TabIndex = 38;
            this.textBoxGamePlayerZipcode.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(399, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(213, 37);
            this.label12.TabIndex = 40;
            this.label12.Text = "Game Player";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(23, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 24);
            this.label13.TabIndex = 41;
            this.label13.Text = "Service";
            // 
            // labelServiceName
            // 
            this.labelServiceName.AutoSize = true;
            this.labelServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServiceName.Location = new System.Drawing.Point(98, 59);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(39, 24);
            this.labelServiceName.TabIndex = 42;
            this.labelServiceName.Text = "text";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(21, 35);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 24);
            this.label14.TabIndex = 43;
            this.label14.Text = "Version";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(98, 35);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(39, 24);
            this.labelVersion.TabIndex = 44;
            this.labelVersion.Text = "text";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(77, 326);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(132, 20);
            this.label15.TabIndex = 45;
            this.label15.Text = "Player Electric Co";
            // 
            // textBoxGamePlayerElectricCo
            // 
            this.textBoxGamePlayerElectricCo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerElectricCo.Location = new System.Drawing.Point(246, 323);
            this.textBoxGamePlayerElectricCo.Name = "textBoxGamePlayerElectricCo";
            this.textBoxGamePlayerElectricCo.ReadOnly = true;
            this.textBoxGamePlayerElectricCo.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerElectricCo.TabIndex = 46;
            this.textBoxGamePlayerElectricCo.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(21, 11);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 24);
            this.label18.TabIndex = 55;
            this.label18.Text = "Cluster";
            // 
            // labelCluster
            // 
            this.labelCluster.AutoSize = true;
            this.labelCluster.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCluster.Location = new System.Drawing.Point(96, 11);
            this.labelCluster.Name = "labelCluster";
            this.labelCluster.Size = new System.Drawing.Size(65, 24);
            this.labelCluster.TabIndex = 56;
            this.labelCluster.Text = "cluster";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(79, 359);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(131, 20);
            this.label19.TabIndex = 57;
            this.label19.Text = "Player Time Zone";
            // 
            // textBoxPlayerTimeZone
            // 
            this.textBoxPlayerTimeZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerTimeZone.Location = new System.Drawing.Point(247, 359);
            this.textBoxPlayerTimeZone.Name = "textBoxPlayerTimeZone";
            this.textBoxPlayerTimeZone.ReadOnly = true;
            this.textBoxPlayerTimeZone.Size = new System.Drawing.Size(639, 26);
            this.textBoxPlayerTimeZone.TabIndex = 58;
            this.textBoxPlayerTimeZone.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 613);
            this.Controls.Add(this.textBoxPlayerTimeZone);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.labelCluster);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBoxGamePlayerElectricCo);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxGamePlayerZipcode);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxGamePlayerState);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxGamePlayerCity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxPlayerIdentifier);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxGamePlayerStreet);
            this.Controls.Add(this.textBoxGamePlayerName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxEthereumKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxDataConnectString);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxEthereumAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDeactivatePlayer);
            this.Controls.Add(this.buttonStopPlayer);
            this.Controls.Add(this.buttonStartPlayer);
            this.Controls.Add(this.buttonActivatePlayer);
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
        private System.Windows.Forms.Button buttonActivatePlayer;
        private System.Windows.Forms.Button buttonStartPlayer;
        private System.Windows.Forms.Button buttonStopPlayer;
        private System.Windows.Forms.Button buttonDeactivatePlayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEthereumAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDataConnectString;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxEthereumKey;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxGamePlayerName;
        private System.Windows.Forms.TextBox textBoxGamePlayerStreet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPlayerIdentifier;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxGamePlayerCity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxGamePlayerState;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxGamePlayerZipcode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelServiceName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxGamePlayerElectricCo;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label labelCluster;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxPlayerTimeZone;
    }
}

