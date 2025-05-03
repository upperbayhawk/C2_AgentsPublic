//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
namespace GameStarter
{
    partial class GameStarter
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
            this.GameNameText = new System.Windows.Forms.TextBox();
            this.CFCLabel = new System.Windows.Forms.Label();
            this.GameStarterLabel = new System.Windows.Forms.Label();
            this.GameNameLabel = new System.Windows.Forms.Label();
            this.GameTypeText = new System.Windows.Forms.TextBox();
            this.GameTypeLabel = new System.Windows.Forms.Label();
            this.PointsPerWattText = new System.Windows.Forms.TextBox();
            this.DollarPerPointText = new System.Windows.Forms.TextBox();
            this.StartTimeText = new System.Windows.Forms.DateTimePicker();
            this.StartTimeLabel = new System.Windows.Forms.Label();
            this.StartGameButton = new System.Windows.Forms.Button();
            this.JsonEventString = new System.Windows.Forms.TextBox();
            this.CreateJsonButton = new System.Windows.Forms.Button();
            this.DurationInMinutesLabel = new System.Windows.Forms.Label();
            this.PointsPerPercentText = new System.Windows.Forms.TextBox();
            this.BonusPointsText = new System.Windows.Forms.TextBox();
            this.DollarPerPointLabel = new System.Windows.Forms.Label();
            this.PointsPerWattLabel = new System.Windows.Forms.Label();
            this.PointsPerPercentLabel = new System.Windows.Forms.Label();
            this.BonusPointLabel = new System.Windows.Forms.Label();
            this.BonusPoolText = new System.Windows.Forms.TextBox();
            this.BonusPoolLabel = new System.Windows.Forms.Label();
            this.GridZoneLabel = new System.Windows.Forms.Label();
            this.GridZoneText = new System.Windows.Forms.TextBox();
            this.comboBoxDurationInMinutes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxGameAwardRank = new System.Windows.Forms.TextBox();
            this.GameIdText = new System.Windows.Forms.TextBox();
            this.GameIdLabel = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.labelCluster = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPreStartAlert = new System.Windows.Forms.TextBox();
            this.GetLMP = new System.Windows.Forms.Button();
            this.labelLMP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GameNameText
            // 
            this.GameNameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameNameText.Location = new System.Drawing.Point(259, 228);
            this.GameNameText.Name = "GameNameText";
            this.GameNameText.Size = new System.Drawing.Size(257, 29);
            this.GameNameText.TabIndex = 2;
            this.GameNameText.Text = "Baccarat";
            // 
            // CFCLabel
            // 
            this.CFCLabel.AutoSize = true;
            this.CFCLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CFCLabel.Location = new System.Drawing.Point(384, 14);
            this.CFCLabel.Name = "CFCLabel";
            this.CFCLabel.Size = new System.Drawing.Size(360, 29);
            this.CFCLabel.TabIndex = 1;
            this.CFCLabel.Text = "CurrentForCarbon, The Game";
            this.CFCLabel.Click += new System.EventHandler(this.CFCLabel_Click);
            // 
            // GameStarterLabel
            // 
            this.GameStarterLabel.AutoSize = true;
            this.GameStarterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameStarterLabel.Location = new System.Drawing.Point(454, 59);
            this.GameStarterLabel.Name = "GameStarterLabel";
            this.GameStarterLabel.Size = new System.Drawing.Size(221, 37);
            this.GameStarterLabel.TabIndex = 2;
            this.GameStarterLabel.Text = "Game Starter";
            // 
            // GameNameLabel
            // 
            this.GameNameLabel.AutoSize = true;
            this.GameNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameNameLabel.Location = new System.Drawing.Point(95, 230);
            this.GameNameLabel.Name = "GameNameLabel";
            this.GameNameLabel.Size = new System.Drawing.Size(112, 24);
            this.GameNameLabel.TabIndex = 3;
            this.GameNameLabel.Text = "GameName";
            // 
            // GameTypeText
            // 
            this.GameTypeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameTypeText.Location = new System.Drawing.Point(259, 271);
            this.GameTypeText.Name = "GameTypeText";
            this.GameTypeText.Size = new System.Drawing.Size(257, 29);
            this.GameTypeText.TabIndex = 4;
            this.GameTypeText.Text = "SHEDPOWER";
            // 
            // GameTypeLabel
            // 
            this.GameTypeLabel.AutoSize = true;
            this.GameTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameTypeLabel.Location = new System.Drawing.Point(95, 274);
            this.GameTypeLabel.Name = "GameTypeLabel";
            this.GameTypeLabel.Size = new System.Drawing.Size(104, 24);
            this.GameTypeLabel.TabIndex = 7;
            this.GameTypeLabel.Text = "GameType";
            // 
            // PointsPerWattText
            // 
            this.PointsPerWattText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PointsPerWattText.Location = new System.Drawing.Point(693, 227);
            this.PointsPerWattText.Name = "PointsPerWattText";
            this.PointsPerWattText.Size = new System.Drawing.Size(76, 29);
            this.PointsPerWattText.TabIndex = 9;
            this.PointsPerWattText.Text = "1.0";
            // 
            // DollarPerPointText
            // 
            this.DollarPerPointText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DollarPerPointText.Location = new System.Drawing.Point(693, 189);
            this.DollarPerPointText.Name = "DollarPerPointText";
            this.DollarPerPointText.Size = new System.Drawing.Size(76, 29);
            this.DollarPerPointText.TabIndex = 8;
            this.DollarPerPointText.Text = ".001";
            // 
            // StartTimeText
            // 
            this.StartTimeText.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartTimeText.Checked = false;
            this.StartTimeText.CustomFormat = "\"MM-dd-yyyy hh:mm:ss tt\"";
            this.StartTimeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartTimeText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartTimeText.Location = new System.Drawing.Point(346, 126);
            this.StartTimeText.Name = "StartTimeText";
            this.StartTimeText.Size = new System.Drawing.Size(263, 35);
            this.StartTimeText.TabIndex = 6;
            this.StartTimeText.Value = new System.DateTime(2021, 7, 1, 16, 28, 2, 0);
            // 
            // StartTimeLabel
            // 
            this.StartTimeLabel.AutoSize = true;
            this.StartTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartTimeLabel.Location = new System.Drawing.Point(118, 128);
            this.StartTimeLabel.Name = "StartTimeLabel";
            this.StartTimeLabel.Size = new System.Drawing.Size(210, 29);
            this.StartTimeLabel.TabIndex = 12;
            this.StartTimeLabel.Text = "Game Start Time";
            // 
            // StartGameButton
            // 
            this.StartGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartGameButton.Location = new System.Drawing.Point(451, 563);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(229, 36);
            this.StartGameButton.TabIndex = 14;
            this.StartGameButton.Text = "Start Game";
            this.StartGameButton.UseVisualStyleBackColor = true;
            this.StartGameButton.Click += new System.EventHandler(this.startGameButton_Click);
            // 
            // JsonEventString
            // 
            this.JsonEventString.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JsonEventString.Location = new System.Drawing.Point(23, 444);
            this.JsonEventString.Multiline = true;
            this.JsonEventString.Name = "JsonEventString";
            this.JsonEventString.Size = new System.Drawing.Size(1053, 103);
            this.JsonEventString.TabIndex = 14;
            this.JsonEventString.TabStop = false;
            // 
            // CreateJsonButton
            // 
            this.CreateJsonButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateJsonButton.Location = new System.Drawing.Point(491, 404);
            this.CreateJsonButton.Name = "CreateJsonButton";
            this.CreateJsonButton.Size = new System.Drawing.Size(142, 34);
            this.CreateJsonButton.TabIndex = 13;
            this.CreateJsonButton.Text = "Create JSON";
            this.CreateJsonButton.UseVisualStyleBackColor = true;
            this.CreateJsonButton.Click += new System.EventHandler(this.creatJsonButton_Click);
            // 
            // DurationInMinutesLabel
            // 
            this.DurationInMinutesLabel.AutoSize = true;
            this.DurationInMinutesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DurationInMinutesLabel.Location = new System.Drawing.Point(622, 128);
            this.DurationInMinutesLabel.Name = "DurationInMinutesLabel";
            this.DurationInMinutesLabel.Size = new System.Drawing.Size(223, 29);
            this.DurationInMinutesLabel.TabIndex = 18;
            this.DurationInMinutesLabel.Text = "DurationInMinutes";
            // 
            // PointsPerPercentText
            // 
            this.PointsPerPercentText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PointsPerPercentText.Location = new System.Drawing.Point(693, 269);
            this.PointsPerPercentText.Name = "PointsPerPercentText";
            this.PointsPerPercentText.Size = new System.Drawing.Size(76, 29);
            this.PointsPerPercentText.TabIndex = 10;
            this.PointsPerPercentText.Text = "0.064";
            // 
            // BonusPointsText
            // 
            this.BonusPointsText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BonusPointsText.Location = new System.Drawing.Point(933, 186);
            this.BonusPointsText.Name = "BonusPointsText";
            this.BonusPointsText.Size = new System.Drawing.Size(76, 29);
            this.BonusPointsText.TabIndex = 11;
            this.BonusPointsText.Text = "10";
            // 
            // DollarPerPointLabel
            // 
            this.DollarPerPointLabel.AutoSize = true;
            this.DollarPerPointLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DollarPerPointLabel.Location = new System.Drawing.Point(529, 192);
            this.DollarPerPointLabel.Name = "DollarPerPointLabel";
            this.DollarPerPointLabel.Size = new System.Drawing.Size(129, 24);
            this.DollarPerPointLabel.TabIndex = 22;
            this.DollarPerPointLabel.Text = "DollarPerPoint";
            // 
            // PointsPerWattLabel
            // 
            this.PointsPerWattLabel.AutoSize = true;
            this.PointsPerWattLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PointsPerWattLabel.Location = new System.Drawing.Point(529, 230);
            this.PointsPerWattLabel.Name = "PointsPerWattLabel";
            this.PointsPerWattLabel.Size = new System.Drawing.Size(126, 24);
            this.PointsPerWattLabel.TabIndex = 23;
            this.PointsPerWattLabel.Text = "PointsPerWatt";
            // 
            // PointsPerPercentLabel
            // 
            this.PointsPerPercentLabel.AutoSize = true;
            this.PointsPerPercentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PointsPerPercentLabel.Location = new System.Drawing.Point(529, 271);
            this.PointsPerPercentLabel.Name = "PointsPerPercentLabel";
            this.PointsPerPercentLabel.Size = new System.Drawing.Size(155, 24);
            this.PointsPerPercentLabel.TabIndex = 25;
            this.PointsPerPercentLabel.Text = "PointsPerPercent";
            // 
            // BonusPointLabel
            // 
            this.BonusPointLabel.AutoSize = true;
            this.BonusPointLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BonusPointLabel.Location = new System.Drawing.Point(796, 189);
            this.BonusPointLabel.Name = "BonusPointLabel";
            this.BonusPointLabel.Size = new System.Drawing.Size(120, 24);
            this.BonusPointLabel.TabIndex = 26;
            this.BonusPointLabel.Text = "Bonus Points";
            // 
            // BonusPoolText
            // 
            this.BonusPoolText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BonusPoolText.Location = new System.Drawing.Point(933, 233);
            this.BonusPoolText.Name = "BonusPoolText";
            this.BonusPoolText.Size = new System.Drawing.Size(76, 29);
            this.BonusPoolText.TabIndex = 12;
            this.BonusPoolText.Text = "10";
            // 
            // BonusPoolLabel
            // 
            this.BonusPoolLabel.AutoSize = true;
            this.BonusPoolLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BonusPoolLabel.Location = new System.Drawing.Point(796, 233);
            this.BonusPoolLabel.Name = "BonusPoolLabel";
            this.BonusPoolLabel.Size = new System.Drawing.Size(107, 24);
            this.BonusPoolLabel.TabIndex = 28;
            this.BonusPoolLabel.Text = "Bonus Pool";
            // 
            // GridZoneLabel
            // 
            this.GridZoneLabel.AutoSize = true;
            this.GridZoneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridZoneLabel.Location = new System.Drawing.Point(97, 192);
            this.GridZoneLabel.Name = "GridZoneLabel";
            this.GridZoneLabel.Size = new System.Drawing.Size(90, 24);
            this.GridZoneLabel.TabIndex = 29;
            this.GridZoneLabel.Text = "GridZone";
            // 
            // GridZoneText
            // 
            this.GridZoneText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridZoneText.Location = new System.Drawing.Point(259, 189);
            this.GridZoneText.Name = "GridZoneText";
            this.GridZoneText.Size = new System.Drawing.Size(257, 29);
            this.GridZoneText.TabIndex = 1;
            this.GridZoneText.Text = "ALL";
            this.GridZoneText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // comboBoxDurationInMinutes
            // 
            this.comboBoxDurationInMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDurationInMinutes.FormattingEnabled = true;
            this.comboBoxDurationInMinutes.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "30",
            "60",
            "120",
            "180",
            "240"});
            this.comboBoxDurationInMinutes.Location = new System.Drawing.Point(851, 125);
            this.comboBoxDurationInMinutes.Name = "comboBoxDurationInMinutes";
            this.comboBoxDurationInMinutes.Size = new System.Drawing.Size(107, 37);
            this.comboBoxDurationInMinutes.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(95, 311);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 24);
            this.label2.TabIndex = 35;
            this.label2.Text = "GameAwardRank";
            // 
            // textBoxGameAwardRank
            // 
            this.textBoxGameAwardRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGameAwardRank.Location = new System.Drawing.Point(259, 309);
            this.textBoxGameAwardRank.Name = "textBoxGameAwardRank";
            this.textBoxGameAwardRank.Size = new System.Drawing.Size(257, 29);
            this.textBoxGameAwardRank.TabIndex = 5;
            this.textBoxGameAwardRank.Text = "GOLD";
            // 
            // GameIdText
            // 
            this.GameIdText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameIdText.Location = new System.Drawing.Point(160, 359);
            this.GameIdText.Name = "GameIdText";
            this.GameIdText.Size = new System.Drawing.Size(896, 29);
            this.GameIdText.TabIndex = 36;
            this.GameIdText.Text = "1234";
            // 
            // GameIdLabel
            // 
            this.GameIdLabel.AutoSize = true;
            this.GameIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameIdLabel.Location = new System.Drawing.Point(59, 362);
            this.GameIdLabel.Name = "GameIdLabel";
            this.GameIdLabel.Size = new System.Drawing.Size(78, 24);
            this.GameIdLabel.TabIndex = 37;
            this.GameIdLabel.Text = "GameID";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(35, 19);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(68, 24);
            this.label21.TabIndex = 76;
            this.label21.Text = "Cluster";
            // 
            // labelCluster
            // 
            this.labelCluster.AutoSize = true;
            this.labelCluster.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCluster.Location = new System.Drawing.Point(104, 19);
            this.labelCluster.Name = "labelCluster";
            this.labelCluster.Size = new System.Drawing.Size(65, 24);
            this.labelCluster.TabIndex = 75;
            this.labelCluster.Text = "cluster";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(103, 46);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(71, 24);
            this.labelVersion.TabIndex = 74;
            this.labelVersion.Text = "version";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(34, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 24);
            this.label15.TabIndex = 73;
            this.label15.Text = "Version";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(529, 311);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 24);
            this.label1.TabIndex = 77;
            this.label1.Text = "PreStartAlert";
            // 
            // textBoxPreStartAlert
            // 
            this.textBoxPreStartAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPreStartAlert.Location = new System.Drawing.Point(692, 309);
            this.textBoxPreStartAlert.Name = "textBoxPreStartAlert";
            this.textBoxPreStartAlert.Size = new System.Drawing.Size(77, 29);
            this.textBoxPreStartAlert.TabIndex = 78;
            this.textBoxPreStartAlert.Text = "true";
            // 
            // GetLMP
            // 
            this.GetLMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetLMP.Location = new System.Drawing.Point(800, 288);
            this.GetLMP.Name = "GetLMP";
            this.GetLMP.Size = new System.Drawing.Size(103, 34);
            this.GetLMP.TabIndex = 79;
            this.GetLMP.Text = "Get LMP";
            this.GetLMP.UseVisualStyleBackColor = true;
            this.GetLMP.Click += new System.EventHandler(this.GetLMP_Click);
            // 
            // labelLMP
            // 
            this.labelLMP.AutoSize = true;
            this.labelLMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLMP.Location = new System.Drawing.Point(929, 292);
            this.labelLMP.Name = "labelLMP";
            this.labelLMP.Size = new System.Drawing.Size(41, 24);
            this.labelLMP.TabIndex = 81;
            this.labelLMP.Text = "lmp";
            this.labelLMP.Click += new System.EventHandler(this.labelLMP_Click);
            // 
            // GameStarter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 618);
            this.Controls.Add(this.labelLMP);
            this.Controls.Add(this.GetLMP);
            this.Controls.Add(this.textBoxPreStartAlert);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.labelCluster);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.GameIdText);
            this.Controls.Add(this.GameIdLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxGameAwardRank);
            this.Controls.Add(this.comboBoxDurationInMinutes);
            this.Controls.Add(this.GridZoneText);
            this.Controls.Add(this.GridZoneLabel);
            this.Controls.Add(this.BonusPoolLabel);
            this.Controls.Add(this.BonusPoolText);
            this.Controls.Add(this.BonusPointLabel);
            this.Controls.Add(this.PointsPerPercentLabel);
            this.Controls.Add(this.PointsPerWattLabel);
            this.Controls.Add(this.DollarPerPointLabel);
            this.Controls.Add(this.BonusPointsText);
            this.Controls.Add(this.PointsPerPercentText);
            this.Controls.Add(this.DurationInMinutesLabel);
            this.Controls.Add(this.CreateJsonButton);
            this.Controls.Add(this.JsonEventString);
            this.Controls.Add(this.StartGameButton);
            this.Controls.Add(this.StartTimeLabel);
            this.Controls.Add(this.StartTimeText);
            this.Controls.Add(this.DollarPerPointText);
            this.Controls.Add(this.PointsPerWattText);
            this.Controls.Add(this.GameTypeLabel);
            this.Controls.Add(this.GameTypeText);
            this.Controls.Add(this.GameNameLabel);
            this.Controls.Add(this.GameStarterLabel);
            this.Controls.Add(this.CFCLabel);
            this.Controls.Add(this.GameNameText);
            this.Name = "GameStarter";
            this.Text = "CurrentForCarbon, The Real SaveThePlanet Game";
            this.TransparencyKey = System.Drawing.Color.White;
            this.Load += new System.EventHandler(this.GameStarter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox GameNameText;
        private System.Windows.Forms.Label GameStarterLabel;
        private System.Windows.Forms.Label GameNameLabel;
        private System.Windows.Forms.TextBox GameTypeText;
        private System.Windows.Forms.Label GameTypeLabel;
        private System.Windows.Forms.TextBox PointsPerWattText;
        private System.Windows.Forms.TextBox DollarPerPointText;
        private System.Windows.Forms.DateTimePicker StartTimeText;
        private System.Windows.Forms.Label StartTimeLabel;
        private System.Windows.Forms.Button StartGameButton;
        private System.Windows.Forms.TextBox JsonEventString;
        private System.Windows.Forms.Label CFCLabel;
        private System.Windows.Forms.Button CreateJsonButton;
        private System.Windows.Forms.Label DurationInMinutesLabel;
        private System.Windows.Forms.TextBox PointsPerPercentText;
        private System.Windows.Forms.TextBox BonusPointsText;
        private System.Windows.Forms.Label DollarPerPointLabel;
        private System.Windows.Forms.Label PointsPerWattLabel;
        private System.Windows.Forms.Label PointsPerPercentLabel;
        private System.Windows.Forms.Label BonusPointLabel;
        private System.Windows.Forms.TextBox BonusPoolText;
        private System.Windows.Forms.Label BonusPoolLabel;
        private System.Windows.Forms.Label GridZoneLabel;
        private System.Windows.Forms.TextBox GridZoneText;
        private System.Windows.Forms.ComboBox comboBoxDurationInMinutes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxGameAwardRank;
        private System.Windows.Forms.TextBox GameIdText;
        private System.Windows.Forms.Label GameIdLabel;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label labelCluster;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPreStartAlert;
        private System.Windows.Forms.Button GetLMP;
        private System.Windows.Forms.Label labelLMP;
    }
}

