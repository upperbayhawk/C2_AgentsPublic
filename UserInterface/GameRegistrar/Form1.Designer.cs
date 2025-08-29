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
            this.buttonRegisterPlayer = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxContractAddress = new System.Windows.Forms.TextBox();
            this.labelContractAddress = new System.Windows.Forms.Label();
            this.buttonCreatePlayerID = new System.Windows.Forms.Button();
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
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelServiceName = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerElectricCo = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerPhone = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerEmail = new System.Windows.Forms.TextBox();
            this.buttonSavePlayer = new System.Windows.Forms.Button();
            this.buttonRestorePlayer = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxGamePlayerNumber = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxMqttUserName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxMqttPassword = new System.Windows.Forms.TextBox();
            this.labelCluster = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.textBoxTimezone = new System.Windows.Forms.TextBox();
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
            this.menuStrip3.Size = new System.Drawing.Size(980, 24);
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
            this.buttonActivatePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonActivatePlayer.Location = new System.Drawing.Point(69, 104);
            this.buttonActivatePlayer.Name = "buttonActivatePlayer";
            this.buttonActivatePlayer.Size = new System.Drawing.Size(193, 37);
            this.buttonActivatePlayer.TabIndex = 1;
            this.buttonActivatePlayer.Text = "Activate Player";
            this.buttonActivatePlayer.UseVisualStyleBackColor = true;
            this.buttonActivatePlayer.Click += new System.EventHandler(this.buttonActivatePlayer_Click);
            // 
            // buttonStartPlayer
            // 
            this.buttonStartPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStartPlayer.Location = new System.Drawing.Point(295, 104);
            this.buttonStartPlayer.Name = "buttonStartPlayer";
            this.buttonStartPlayer.Size = new System.Drawing.Size(190, 37);
            this.buttonStartPlayer.TabIndex = 2;
            this.buttonStartPlayer.Text = "Start Playing";
            this.buttonStartPlayer.UseVisualStyleBackColor = true;
            this.buttonStartPlayer.Click += new System.EventHandler(this.buttonStartPlayer_Click);
            // 
            // buttonStopPlayer
            // 
            this.buttonStopPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStopPlayer.Location = new System.Drawing.Point(517, 104);
            this.buttonStopPlayer.Name = "buttonStopPlayer";
            this.buttonStopPlayer.Size = new System.Drawing.Size(190, 37);
            this.buttonStopPlayer.TabIndex = 3;
            this.buttonStopPlayer.Text = "Stop Playing";
            this.buttonStopPlayer.UseVisualStyleBackColor = true;
            this.buttonStopPlayer.Click += new System.EventHandler(this.buttonStopPlayer_Click);
            // 
            // buttonDeactivatePlayer
            // 
            this.buttonDeactivatePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeactivatePlayer.Location = new System.Drawing.Point(736, 104);
            this.buttonDeactivatePlayer.Name = "buttonDeactivatePlayer";
            this.buttonDeactivatePlayer.Size = new System.Drawing.Size(193, 37);
            this.buttonDeactivatePlayer.TabIndex = 4;
            this.buttonDeactivatePlayer.Text = "Deactivate Player";
            this.buttonDeactivatePlayer.UseVisualStyleBackColor = true;
            this.buttonDeactivatePlayer.Click += new System.EventHandler(this.buttonDeactivatePlayer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(333, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Current For Carbon, The Game";
            // 
            // textBoxEthereumAddress
            // 
            this.textBoxEthereumAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEthereumAddress.Location = new System.Drawing.Point(188, 561);
            this.textBoxEthereumAddress.Name = "textBoxEthereumAddress";
            this.textBoxEthereumAddress.Size = new System.Drawing.Size(639, 26);
            this.textBoxEthereumAddress.TabIndex = 14;
            this.textBoxEthereumAddress.TextChanged += new System.EventHandler(this.textBoxEthereumAddress_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 564);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ethereum Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 628);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Data Connect String";
            // 
            // textBoxDataConnectString
            // 
            this.textBoxDataConnectString.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDataConnectString.Location = new System.Drawing.Point(188, 625);
            this.textBoxDataConnectString.Name = "textBoxDataConnectString";
            this.textBoxDataConnectString.Size = new System.Drawing.Size(639, 26);
            this.textBoxDataConnectString.TabIndex = 16;
            this.textBoxDataConnectString.TextChanged += new System.EventHandler(this.textBoxDataConnectString_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 596);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Ethereum Key";
            // 
            // textBoxEthereumKey
            // 
            this.textBoxEthereumKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEthereumKey.Location = new System.Drawing.Point(188, 593);
            this.textBoxEthereumKey.Name = "textBoxEthereumKey";
            this.textBoxEthereumKey.Size = new System.Drawing.Size(639, 26);
            this.textBoxEthereumKey.TabIndex = 15;
            this.textBoxEthereumKey.TextChanged += new System.EventHandler(this.textBoxEthereumKey_TextChanged);
            // 
            // buttonRegisterPlayer
            // 
            this.buttonRegisterPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRegisterPlayer.Location = new System.Drawing.Point(517, 360);
            this.buttonRegisterPlayer.Name = "buttonRegisterPlayer";
            this.buttonRegisterPlayer.Size = new System.Drawing.Size(171, 37);
            this.buttonRegisterPlayer.TabIndex = 22;
            this.buttonRegisterPlayer.Text = "Register Player";
            this.buttonRegisterPlayer.UseVisualStyleBackColor = true;
            this.buttonRegisterPlayer.Click += new System.EventHandler(this.buttonRegisterPlayer_Click);
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
            // textBoxContractAddress
            // 
            this.textBoxContractAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxContractAddress.Location = new System.Drawing.Point(188, 721);
            this.textBoxContractAddress.Name = "textBoxContractAddress";
            this.textBoxContractAddress.ReadOnly = true;
            this.textBoxContractAddress.Size = new System.Drawing.Size(639, 26);
            this.textBoxContractAddress.TabIndex = 17;
            this.textBoxContractAddress.TextChanged += new System.EventHandler(this.textBoxContractAddress_TextChanged);
            // 
            // labelContractAddress
            // 
            this.labelContractAddress.AutoSize = true;
            this.labelContractAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContractAddress.Location = new System.Drawing.Point(20, 724);
            this.labelContractAddress.Name = "labelContractAddress";
            this.labelContractAddress.Size = new System.Drawing.Size(133, 20);
            this.labelContractAddress.TabIndex = 20;
            this.labelContractAddress.Text = "Contract Address";
            // 
            // buttonCreatePlayerID
            // 
            this.buttonCreatePlayerID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCreatePlayerID.Location = new System.Drawing.Point(314, 360);
            this.buttonCreatePlayerID.Name = "buttonCreatePlayerID";
            this.buttonCreatePlayerID.Size = new System.Drawing.Size(171, 37);
            this.buttonCreatePlayerID.TabIndex = 21;
            this.buttonCreatePlayerID.Text = "Create PlayerID";
            this.buttonCreatePlayerID.UseVisualStyleBackColor = true;
            this.buttonCreatePlayerID.Click += new System.EventHandler(this.buttonCreatePlayerID_Click);
            // 
            // textBoxGamePlayerName
            // 
            this.textBoxGamePlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerName.Location = new System.Drawing.Point(188, 168);
            this.textBoxGamePlayerName.Name = "textBoxGamePlayerName";
            this.textBoxGamePlayerName.Size = new System.Drawing.Size(638, 26);
            this.textBoxGamePlayerName.TabIndex = 5;
            this.textBoxGamePlayerName.TextChanged += new System.EventHandler(this.textBoxGamePlayerName_TextChanged);
            // 
            // textBoxGamePlayerStreet
            // 
            this.textBoxGamePlayerStreet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerStreet.Location = new System.Drawing.Point(187, 200);
            this.textBoxGamePlayerStreet.Name = "textBoxGamePlayerStreet";
            this.textBoxGamePlayerStreet.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerStreet.TabIndex = 6;
            this.textBoxGamePlayerStreet.TextChanged += new System.EventHandler(this.textBoxGamePlayerStreet_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 20);
            this.label7.TabIndex = 29;
            this.label7.Text = "Player Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(19, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 20);
            this.label8.TabIndex = 31;
            this.label8.Text = "Player Street";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 496);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 20);
            this.label5.TabIndex = 33;
            this.label5.Text = "Player Identifier";
            // 
            // textBoxPlayerIdentifier
            // 
            this.textBoxPlayerIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerIdentifier.Location = new System.Drawing.Point(188, 493);
            this.textBoxPlayerIdentifier.Name = "textBoxPlayerIdentifier";
            this.textBoxPlayerIdentifier.Size = new System.Drawing.Size(641, 26);
            this.textBoxPlayerIdentifier.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(19, 232);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "Player City";
            // 
            // textBoxGamePlayerCity
            // 
            this.textBoxGamePlayerCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerCity.Location = new System.Drawing.Point(187, 232);
            this.textBoxGamePlayerCity.Name = "textBoxGamePlayerCity";
            this.textBoxGamePlayerCity.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerCity.TabIndex = 7;
            this.textBoxGamePlayerCity.TextChanged += new System.EventHandler(this.textBoxGamePlayerCity_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(19, 264);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 20);
            this.label10.TabIndex = 37;
            this.label10.Text = "Player State";
            // 
            // textBoxGamePlayerState
            // 
            this.textBoxGamePlayerState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerState.Location = new System.Drawing.Point(187, 264);
            this.textBoxGamePlayerState.Name = "textBoxGamePlayerState";
            this.textBoxGamePlayerState.Size = new System.Drawing.Size(259, 26);
            this.textBoxGamePlayerState.TabIndex = 8;
            this.textBoxGamePlayerState.TextChanged += new System.EventHandler(this.textBoxGamePlayerState_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(462, 267);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 20);
            this.label11.TabIndex = 39;
            this.label11.Text = "Player Zipcode";
            // 
            // textBoxGamePlayerZipcode
            // 
            this.textBoxGamePlayerZipcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerZipcode.Location = new System.Drawing.Point(581, 264);
            this.textBoxGamePlayerZipcode.Name = "textBoxGamePlayerZipcode";
            this.textBoxGamePlayerZipcode.Size = new System.Drawing.Size(245, 26);
            this.textBoxGamePlayerZipcode.TabIndex = 9;
            this.textBoxGamePlayerZipcode.TextChanged += new System.EventHandler(this.textBoxGamePlayerZipcode_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(373, 37);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(256, 37);
            this.label13.TabIndex = 42;
            this.label13.Text = "Game Registrar";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(18, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 24);
            this.label14.TabIndex = 43;
            this.label14.Text = "Service";
            // 
            // labelServiceName
            // 
            this.labelServiceName.AutoSize = true;
            this.labelServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServiceName.Location = new System.Drawing.Point(88, 66);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(164, 24);
            this.labelServiceName.TabIndex = 44;
            this.labelServiceName.Text = "labelServiceName";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(18, 37);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 24);
            this.label15.TabIndex = 45;
            this.label15.Text = "Version";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(87, 37);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(71, 24);
            this.labelVersion.TabIndex = 60;
            this.labelVersion.Text = "version";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(21, 529);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(132, 20);
            this.label16.TabIndex = 47;
            this.label16.Text = "Player Electric Co";
            // 
            // textBoxGamePlayerElectricCo
            // 
            this.textBoxGamePlayerElectricCo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerElectricCo.Location = new System.Drawing.Point(188, 526);
            this.textBoxGamePlayerElectricCo.Name = "textBoxGamePlayerElectricCo";
            this.textBoxGamePlayerElectricCo.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerElectricCo.TabIndex = 13;
            this.textBoxGamePlayerElectricCo.TextChanged += new System.EventHandler(this.textBoxGamePlayerElectricCo_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(20, 296);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(102, 20);
            this.label17.TabIndex = 49;
            this.label17.Text = "Player Phone";
            // 
            // textBoxGamePlayerPhone
            // 
            this.textBoxGamePlayerPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerPhone.Location = new System.Drawing.Point(187, 296);
            this.textBoxGamePlayerPhone.Name = "textBoxGamePlayerPhone";
            this.textBoxGamePlayerPhone.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerPhone.TabIndex = 11;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(20, 328);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(95, 20);
            this.label18.TabIndex = 51;
            this.label18.Text = "Player Email";
            // 
            // textBoxGamePlayerEmail
            // 
            this.textBoxGamePlayerEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerEmail.Location = new System.Drawing.Point(187, 328);
            this.textBoxGamePlayerEmail.Name = "textBoxGamePlayerEmail";
            this.textBoxGamePlayerEmail.Size = new System.Drawing.Size(639, 26);
            this.textBoxGamePlayerEmail.TabIndex = 12;
            // 
            // buttonSavePlayer
            // 
            this.buttonSavePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSavePlayer.Location = new System.Drawing.Point(850, 667);
            this.buttonSavePlayer.Name = "buttonSavePlayer";
            this.buttonSavePlayer.Size = new System.Drawing.Size(92, 37);
            this.buttonSavePlayer.TabIndex = 61;
            this.buttonSavePlayer.Text = "Save";
            this.buttonSavePlayer.UseVisualStyleBackColor = true;
            this.buttonSavePlayer.Click += new System.EventHandler(this.buttonSavePlayer_Click);
            // 
            // buttonRestorePlayer
            // 
            this.buttonRestorePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestorePlayer.Location = new System.Drawing.Point(850, 710);
            this.buttonRestorePlayer.Name = "buttonRestorePlayer";
            this.buttonRestorePlayer.Size = new System.Drawing.Size(92, 37);
            this.buttonRestorePlayer.TabIndex = 62;
            this.buttonRestorePlayer.Text = "Restore Player";
            this.buttonRestorePlayer.UseVisualStyleBackColor = true;
            this.buttonRestorePlayer.Click += new System.EventHandler(this.buttonRestorePlayer_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(21, 461);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 20);
            this.label12.TabIndex = 63;
            this.label12.Text = "Player Number";
            // 
            // textBoxGamePlayerNumber
            // 
            this.textBoxGamePlayerNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGamePlayerNumber.Location = new System.Drawing.Point(188, 458);
            this.textBoxGamePlayerNumber.Name = "textBoxGamePlayerNumber";
            this.textBoxGamePlayerNumber.Size = new System.Drawing.Size(641, 26);
            this.textBoxGamePlayerNumber.TabIndex = 64;
            this.textBoxGamePlayerNumber.TextChanged += new System.EventHandler(this.textBoxGamePlayerNumber_TextChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(21, 660);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(130, 20);
            this.label19.TabIndex = 65;
            this.label19.Text = "MQTT Username";
            // 
            // textBoxMqttUserName
            // 
            this.textBoxMqttUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMqttUserName.Location = new System.Drawing.Point(188, 657);
            this.textBoxMqttUserName.Name = "textBoxMqttUserName";
            this.textBoxMqttUserName.Size = new System.Drawing.Size(639, 26);
            this.textBoxMqttUserName.TabIndex = 66;
            this.textBoxMqttUserName.TextChanged += new System.EventHandler(this.textBoxMqttUserName_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(21, 692);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(125, 20);
            this.label20.TabIndex = 67;
            this.label20.Text = "MQTT Password";
            // 
            // textBoxMqttPassword
            // 
            this.textBoxMqttPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMqttPassword.Location = new System.Drawing.Point(188, 689);
            this.textBoxMqttPassword.Name = "textBoxMqttPassword";
            this.textBoxMqttPassword.Size = new System.Drawing.Size(639, 26);
            this.textBoxMqttPassword.TabIndex = 68;
            this.textBoxMqttPassword.TextChanged += new System.EventHandler(this.textBoxMqttPassword_TextChanged);
            // 
            // labelCluster
            // 
            this.labelCluster.AutoSize = true;
            this.labelCluster.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCluster.Location = new System.Drawing.Point(88, 10);
            this.labelCluster.Name = "labelCluster";
            this.labelCluster.Size = new System.Drawing.Size(65, 24);
            this.labelCluster.TabIndex = 69;
            this.labelCluster.Text = "cluster";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(19, 10);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(68, 24);
            this.label21.TabIndex = 70;
            this.label21.Text = "Cluster";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(21, 427);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(131, 20);
            this.label22.TabIndex = 71;
            this.label22.Text = "Player Time Zone";
            // 
            // textBoxTimezone
            // 
            this.textBoxTimezone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTimezone.Location = new System.Drawing.Point(188, 424);
            this.textBoxTimezone.Name = "textBoxTimezone";
            this.textBoxTimezone.Size = new System.Drawing.Size(641, 26);
            this.textBoxTimezone.TabIndex = 72;
            this.textBoxTimezone.TextChanged += new System.EventHandler(this.textBoxTimezone_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 762);
            this.Controls.Add(this.textBoxTimezone);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.labelCluster);
            this.Controls.Add(this.textBoxMqttPassword);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.textBoxMqttUserName);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.textBoxGamePlayerNumber);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.buttonRestorePlayer);
            this.Controls.Add(this.buttonSavePlayer);
            this.Controls.Add(this.textBoxGamePlayerEmail);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBoxGamePlayerPhone);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBoxGamePlayerElectricCo);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
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
            this.Controls.Add(this.buttonCreatePlayerID);
            this.Controls.Add(this.labelContractAddress);
            this.Controls.Add(this.textBoxContractAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonRegisterPlayer);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
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
        private System.Windows.Forms.Button buttonRegisterPlayer;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxContractAddress;
        private System.Windows.Forms.Label labelContractAddress;
        private System.Windows.Forms.Button buttonCreatePlayerID;
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
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelServiceName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxGamePlayerElectricCo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxGamePlayerPhone;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxGamePlayerEmail;
        private System.Windows.Forms.Button buttonSavePlayer;
        private System.Windows.Forms.Button buttonRestorePlayer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxGamePlayerNumber;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxMqttUserName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxMqttPassword;
        private System.Windows.Forms.Label labelCluster;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBoxTimezone;
    }
}

