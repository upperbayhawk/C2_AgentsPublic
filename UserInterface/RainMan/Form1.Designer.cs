namespace RainMan
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
            this.GetAllSoilRecords = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ClusterName = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.Community = new System.Windows.Forms.TextBox();
            this.lblCommunity = new System.Windows.Forms.Label();
            this.lblLotNumber = new System.Windows.Forms.Label();
            this.TextBoxLotNumber = new System.Windows.Forms.TextBox();
            this.lblZipcode = new System.Windows.Forms.Label();
            this.textBoxZipcode = new System.Windows.Forms.TextBox();
            this.labelRainManEnabled = new System.Windows.Forms.Label();
            this.textBoxRainManEnabled = new System.Windows.Forms.TextBox();
            this.labelRainManChainEnabled = new System.Windows.Forms.Label();
            this.textBoxRainManChainEnabled = new System.Windows.Forms.TextBox();
            this.labelClusterName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelPeriodHours = new System.Windows.Forms.Label();
            this.textBoxPeriodInHours = new System.Windows.Forms.TextBox();
            this.labelChainID = new System.Windows.Forms.Label();
            this.textBoxChainID = new System.Windows.Forms.TextBox();
            this.labelChainContract = new System.Windows.Forms.Label();
            this.textBoxChainContract = new System.Windows.Forms.TextBox();
            this.labelChainKey = new System.Windows.Forms.Label();
            this.textBoxChainKey = new System.Windows.Forms.TextBox();
            this.labelChainServerURL = new System.Windows.Forms.Label();
            this.textBoxChainServerURL = new System.Windows.Forms.TextBox();
            this.labelChainAddress = new System.Windows.Forms.Label();
            this.textBoxChainAddress = new System.Windows.Forms.TextBox();
            this.labelSlice = new System.Windows.Forms.Label();
            this.textBoxSlice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonActivate = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonDeactivate = new System.Windows.Forms.Button();
            this.ServiceName = new System.Windows.Forms.Label();
            this.labelServiceName = new System.Windows.Forms.Label();
            this.lblSamples = new System.Windows.Forms.Label();
            this.labelSamples = new System.Windows.Forms.Label();
            this.lblSlices = new System.Windows.Forms.Label();
            this.labelSlices = new System.Windows.Forms.Label();
            this.buttonGetSlice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetAllSoilRecords
            // 
            this.GetAllSoilRecords.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.GetAllSoilRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetAllSoilRecords.Location = new System.Drawing.Point(372, 91);
            this.GetAllSoilRecords.Name = "GetAllSoilRecords";
            this.GetAllSoilRecords.Size = new System.Drawing.Size(348, 81);
            this.GetAllSoilRecords.TabIndex = 0;
            this.GetAllSoilRecords.Text = "GetSoilData";
            this.GetAllSoilRecords.UseVisualStyleBackColor = false;
            this.GetAllSoilRecords.Click += new System.EventHandler(this.GetAllSoilRecords_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(460, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "RainMan";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ClusterName
            // 
            this.ClusterName.AutoSize = true;
            this.ClusterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClusterName.Location = new System.Drawing.Point(51, 31);
            this.ClusterName.Name = "ClusterName";
            this.ClusterName.Size = new System.Drawing.Size(119, 22);
            this.ClusterName.TabIndex = 2;
            this.ClusterName.Text = "ClusterName:";
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version.Location = new System.Drawing.Point(94, 53);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(76, 22);
            this.Version.TabIndex = 3;
            this.Version.Text = "Version:";
            // 
            // Community
            // 
            this.Community.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Community.Location = new System.Drawing.Point(665, 336);
            this.Community.Name = "Community";
            this.Community.Size = new System.Drawing.Size(255, 28);
            this.Community.TabIndex = 4;
            this.Community.TextChanged += new System.EventHandler(this.Community_TextChanged);
            // 
            // lblCommunity
            // 
            this.lblCommunity.AutoSize = true;
            this.lblCommunity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommunity.Location = new System.Drawing.Point(536, 339);
            this.lblCommunity.Name = "lblCommunity";
            this.lblCommunity.Size = new System.Drawing.Size(104, 22);
            this.lblCommunity.TabIndex = 5;
            this.lblCommunity.Text = "Community:";
            // 
            // lblLotNumber
            // 
            this.lblLotNumber.AutoSize = true;
            this.lblLotNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLotNumber.Location = new System.Drawing.Point(538, 377);
            this.lblLotNumber.Name = "lblLotNumber";
            this.lblLotNumber.Size = new System.Drawing.Size(103, 22);
            this.lblLotNumber.TabIndex = 6;
            this.lblLotNumber.Text = "LotNumber:";
            // 
            // TextBoxLotNumber
            // 
            this.TextBoxLotNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxLotNumber.Location = new System.Drawing.Point(665, 374);
            this.TextBoxLotNumber.Name = "TextBoxLotNumber";
            this.TextBoxLotNumber.Size = new System.Drawing.Size(255, 28);
            this.TextBoxLotNumber.TabIndex = 7;
            this.TextBoxLotNumber.TextChanged += new System.EventHandler(this.TextBoxLotNumber_TextChanged);
            // 
            // lblZipcode
            // 
            this.lblZipcode.AutoSize = true;
            this.lblZipcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZipcode.Location = new System.Drawing.Point(556, 417);
            this.lblZipcode.Name = "lblZipcode";
            this.lblZipcode.Size = new System.Drawing.Size(79, 22);
            this.lblZipcode.TabIndex = 8;
            this.lblZipcode.Text = "Zipcode:";
            // 
            // textBoxZipcode
            // 
            this.textBoxZipcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxZipcode.Location = new System.Drawing.Point(665, 414);
            this.textBoxZipcode.Name = "textBoxZipcode";
            this.textBoxZipcode.Size = new System.Drawing.Size(255, 28);
            this.textBoxZipcode.TabIndex = 9;
            this.textBoxZipcode.TextChanged += new System.EventHandler(this.textBoxZipcode_TextChanged);
            // 
            // labelRainManEnabled
            // 
            this.labelRainManEnabled.AutoSize = true;
            this.labelRainManEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRainManEnabled.Location = new System.Drawing.Point(186, 342);
            this.labelRainManEnabled.Name = "labelRainManEnabled";
            this.labelRainManEnabled.Size = new System.Drawing.Size(152, 22);
            this.labelRainManEnabled.TabIndex = 10;
            this.labelRainManEnabled.Text = "RainManEnabled:";
            // 
            // textBoxRainManEnabled
            // 
            this.textBoxRainManEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRainManEnabled.Location = new System.Drawing.Point(353, 339);
            this.textBoxRainManEnabled.Name = "textBoxRainManEnabled";
            this.textBoxRainManEnabled.Size = new System.Drawing.Size(100, 28);
            this.textBoxRainManEnabled.TabIndex = 11;
            this.textBoxRainManEnabled.TextChanged += new System.EventHandler(this.textBoxRainManEnabled_TextChanged);
            // 
            // labelRainManChainEnabled
            // 
            this.labelRainManChainEnabled.AutoSize = true;
            this.labelRainManChainEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRainManChainEnabled.Location = new System.Drawing.Point(210, 377);
            this.labelRainManChainEnabled.Name = "labelRainManChainEnabled";
            this.labelRainManChainEnabled.Size = new System.Drawing.Size(128, 22);
            this.labelRainManChainEnabled.TabIndex = 12;
            this.labelRainManChainEnabled.Text = "ChainEnabled:";
            // 
            // textBoxRainManChainEnabled
            // 
            this.textBoxRainManChainEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRainManChainEnabled.Location = new System.Drawing.Point(353, 377);
            this.textBoxRainManChainEnabled.Name = "textBoxRainManChainEnabled";
            this.textBoxRainManChainEnabled.Size = new System.Drawing.Size(100, 28);
            this.textBoxRainManChainEnabled.TabIndex = 13;
            this.textBoxRainManChainEnabled.TextChanged += new System.EventHandler(this.textBoxRainManChainEnabled_TextChanged);
            // 
            // labelClusterName
            // 
            this.labelClusterName.AutoSize = true;
            this.labelClusterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClusterName.Location = new System.Drawing.Point(184, 31);
            this.labelClusterName.Name = "labelClusterName";
            this.labelClusterName.Size = new System.Drawing.Size(0, 22);
            this.labelClusterName.TabIndex = 14;
            this.labelClusterName.Click += new System.EventHandler(this.labelClusterName_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(184, 53);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(0, 22);
            this.labelVersion.TabIndex = 15;
            // 
            // labelPeriodHours
            // 
            this.labelPeriodHours.AutoSize = true;
            this.labelPeriodHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeriodHours.Location = new System.Drawing.Point(206, 417);
            this.labelPeriodHours.Name = "labelPeriodHours";
            this.labelPeriodHours.Size = new System.Drawing.Size(132, 22);
            this.labelPeriodHours.TabIndex = 16;
            this.labelPeriodHours.Text = "Period (Hours):";
            // 
            // textBoxPeriodInHours
            // 
            this.textBoxPeriodInHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPeriodInHours.Location = new System.Drawing.Point(353, 417);
            this.textBoxPeriodInHours.Name = "textBoxPeriodInHours";
            this.textBoxPeriodInHours.Size = new System.Drawing.Size(62, 28);
            this.textBoxPeriodInHours.TabIndex = 17;
            this.textBoxPeriodInHours.TextChanged += new System.EventHandler(this.textBoxPeriodInHours_TextChanged);
            // 
            // labelChainID
            // 
            this.labelChainID.AutoSize = true;
            this.labelChainID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChainID.Location = new System.Drawing.Point(754, 628);
            this.labelChainID.Name = "labelChainID";
            this.labelChainID.Size = new System.Drawing.Size(79, 22);
            this.labelChainID.TabIndex = 18;
            this.labelChainID.Text = "ChainID:";
            // 
            // textBoxChainID
            // 
            this.textBoxChainID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChainID.Location = new System.Drawing.Point(863, 628);
            this.textBoxChainID.Name = "textBoxChainID";
            this.textBoxChainID.Size = new System.Drawing.Size(96, 28);
            this.textBoxChainID.TabIndex = 19;
            this.textBoxChainID.TextChanged += new System.EventHandler(this.textBoxChainID_TextChanged);
            // 
            // labelChainContract
            // 
            this.labelChainContract.AutoSize = true;
            this.labelChainContract.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChainContract.Location = new System.Drawing.Point(156, 554);
            this.labelChainContract.Name = "labelChainContract";
            this.labelChainContract.Size = new System.Drawing.Size(135, 22);
            this.labelChainContract.TabIndex = 20;
            this.labelChainContract.Text = "Chain Contract:";
            // 
            // textBoxChainContract
            // 
            this.textBoxChainContract.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChainContract.Location = new System.Drawing.Point(319, 551);
            this.textBoxChainContract.Name = "textBoxChainContract";
            this.textBoxChainContract.Size = new System.Drawing.Size(401, 28);
            this.textBoxChainContract.TabIndex = 21;
            this.textBoxChainContract.TextChanged += new System.EventHandler(this.textBoxChainContract_TextChanged);
            // 
            // labelChainKey
            // 
            this.labelChainKey.AutoSize = true;
            this.labelChainKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChainKey.Location = new System.Drawing.Point(72, 676);
            this.labelChainKey.Name = "labelChainKey";
            this.labelChainKey.Size = new System.Drawing.Size(98, 22);
            this.labelChainKey.TabIndex = 22;
            this.labelChainKey.Text = "Chain Key:";
            // 
            // textBoxChainKey
            // 
            this.textBoxChainKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChainKey.Location = new System.Drawing.Point(182, 673);
            this.textBoxChainKey.Name = "textBoxChainKey";
            this.textBoxChainKey.Size = new System.Drawing.Size(831, 28);
            this.textBoxChainKey.TabIndex = 23;
            this.textBoxChainKey.TextChanged += new System.EventHandler(this.textBoxChainKey_TextChanged);
            // 
            // labelChainServerURL
            // 
            this.labelChainServerURL.AutoSize = true;
            this.labelChainServerURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChainServerURL.Location = new System.Drawing.Point(130, 625);
            this.labelChainServerURL.Name = "labelChainServerURL";
            this.labelChainServerURL.Size = new System.Drawing.Size(161, 22);
            this.labelChainServerURL.TabIndex = 24;
            this.labelChainServerURL.Text = "Chain Server URL:";
            // 
            // textBoxChainServerURL
            // 
            this.textBoxChainServerURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChainServerURL.Location = new System.Drawing.Point(319, 625);
            this.textBoxChainServerURL.Name = "textBoxChainServerURL";
            this.textBoxChainServerURL.Size = new System.Drawing.Size(401, 28);
            this.textBoxChainServerURL.TabIndex = 25;
            this.textBoxChainServerURL.TextChanged += new System.EventHandler(this.textBoxChainServerURL_TextChanged);
            // 
            // labelChainAddress
            // 
            this.labelChainAddress.AutoSize = true;
            this.labelChainAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChainAddress.Location = new System.Drawing.Point(163, 588);
            this.labelChainAddress.Name = "labelChainAddress";
            this.labelChainAddress.Size = new System.Drawing.Size(128, 22);
            this.labelChainAddress.TabIndex = 26;
            this.labelChainAddress.Text = "ChainAddress:";
            // 
            // textBoxChainAddress
            // 
            this.textBoxChainAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChainAddress.Location = new System.Drawing.Point(319, 588);
            this.textBoxChainAddress.Name = "textBoxChainAddress";
            this.textBoxChainAddress.Size = new System.Drawing.Size(401, 28);
            this.textBoxChainAddress.TabIndex = 27;
            // 
            // labelSlice
            // 
            this.labelSlice.AutoSize = true;
            this.labelSlice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlice.Location = new System.Drawing.Point(55, 124);
            this.labelSlice.Name = "labelSlice";
            this.labelSlice.Size = new System.Drawing.Size(54, 22);
            this.labelSlice.TabIndex = 28;
            this.labelSlice.Text = "Slice:";
            // 
            // textBoxSlice
            // 
            this.textBoxSlice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSlice.Location = new System.Drawing.Point(115, 121);
            this.textBoxSlice.Name = "textBoxSlice";
            this.textBoxSlice.Size = new System.Drawing.Size(61, 28);
            this.textBoxSlice.TabIndex = 29;
            this.textBoxSlice.Text = "0";
            this.textBoxSlice.TextChanged += new System.EventHandler(this.textBoxSlice_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(219, 513);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(648, 22);
            this.label2.TabIndex = 30;
            this.label2.Text = "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!TREAD CAREFULLY!!!!!!!!!!!!!!!!!!!" +
    "!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            // 
            // buttonActivate
            // 
            this.buttonActivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonActivate.Location = new System.Drawing.Point(160, 211);
            this.buttonActivate.Name = "buttonActivate";
            this.buttonActivate.Size = new System.Drawing.Size(151, 50);
            this.buttonActivate.TabIndex = 31;
            this.buttonActivate.Text = "Activate";
            this.buttonActivate.UseVisualStyleBackColor = true;
            this.buttonActivate.Click += new System.EventHandler(this.buttonActivate_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRun.Location = new System.Drawing.Point(370, 213);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(151, 47);
            this.buttonRun.TabIndex = 32;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStop.Location = new System.Drawing.Point(579, 215);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(141, 45);
            this.buttonStop.TabIndex = 33;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonDeactivate
            // 
            this.buttonDeactivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeactivate.Location = new System.Drawing.Point(784, 215);
            this.buttonDeactivate.Name = "buttonDeactivate";
            this.buttonDeactivate.Size = new System.Drawing.Size(175, 46);
            this.buttonDeactivate.TabIndex = 34;
            this.buttonDeactivate.Text = "Deactivate";
            this.buttonDeactivate.UseVisualStyleBackColor = true;
            this.buttonDeactivate.Click += new System.EventHandler(this.buttonDeactivate_Click);
            // 
            // ServiceName
            // 
            this.ServiceName.AutoSize = true;
            this.ServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServiceName.Location = new System.Drawing.Point(51, 9);
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.Size = new System.Drawing.Size(122, 22);
            this.ServiceName.TabIndex = 35;
            this.ServiceName.Text = "ServiceName:";
            // 
            // labelServiceName
            // 
            this.labelServiceName.AutoSize = true;
            this.labelServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServiceName.Location = new System.Drawing.Point(183, 9);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(0, 22);
            this.labelServiceName.TabIndex = 36;
            // 
            // lblSamples
            // 
            this.lblSamples.AutoSize = true;
            this.lblSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSamples.Location = new System.Drawing.Point(807, 110);
            this.lblSamples.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSamples.Name = "lblSamples";
            this.lblSamples.Size = new System.Drawing.Size(84, 22);
            this.lblSamples.TabIndex = 37;
            this.lblSamples.Text = "Samples:";
            this.lblSamples.Click += new System.EventHandler(this.lblSamples_Click);
            // 
            // labelSamples
            // 
            this.labelSamples.AutoSize = true;
            this.labelSamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSamples.Location = new System.Drawing.Point(895, 110);
            this.labelSamples.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSamples.Name = "labelSamples";
            this.labelSamples.Size = new System.Drawing.Size(20, 22);
            this.labelSamples.TabIndex = 38;
            this.labelSamples.Text = "0";
            this.labelSamples.Click += new System.EventHandler(this.labelSamples_Click);
            // 
            // lblSlices
            // 
            this.lblSlices.AutoSize = true;
            this.lblSlices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlices.Location = new System.Drawing.Point(828, 130);
            this.lblSlices.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSlices.Name = "lblSlices";
            this.lblSlices.Size = new System.Drawing.Size(63, 22);
            this.lblSlices.TabIndex = 39;
            this.lblSlices.Text = "Slices:";
            // 
            // labelSlices
            // 
            this.labelSlices.AutoSize = true;
            this.labelSlices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlices.Location = new System.Drawing.Point(895, 132);
            this.labelSlices.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSlices.Name = "labelSlices";
            this.labelSlices.Size = new System.Drawing.Size(20, 22);
            this.labelSlices.TabIndex = 40;
            this.labelSlices.Text = "0";
            this.labelSlices.Click += new System.EventHandler(this.labelSlices_Click);
            // 
            // buttonGetSlice
            // 
            this.buttonGetSlice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetSlice.Location = new System.Drawing.Point(182, 119);
            this.buttonGetSlice.Name = "buttonGetSlice";
            this.buttonGetSlice.Size = new System.Drawing.Size(90, 32);
            this.buttonGetSlice.TabIndex = 41;
            this.buttonGetSlice.Text = "GetSlice";
            this.buttonGetSlice.UseVisualStyleBackColor = true;
            this.buttonGetSlice.Click += new System.EventHandler(this.buttonGetSlice_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 712);
            this.Controls.Add(this.buttonGetSlice);
            this.Controls.Add(this.labelSlices);
            this.Controls.Add(this.lblSlices);
            this.Controls.Add(this.labelSamples);
            this.Controls.Add(this.lblSamples);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.ServiceName);
            this.Controls.Add(this.buttonDeactivate);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonActivate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSlice);
            this.Controls.Add(this.labelSlice);
            this.Controls.Add(this.textBoxChainAddress);
            this.Controls.Add(this.labelChainAddress);
            this.Controls.Add(this.textBoxChainServerURL);
            this.Controls.Add(this.labelChainServerURL);
            this.Controls.Add(this.textBoxChainKey);
            this.Controls.Add(this.labelChainKey);
            this.Controls.Add(this.textBoxChainContract);
            this.Controls.Add(this.labelChainContract);
            this.Controls.Add(this.textBoxChainID);
            this.Controls.Add(this.labelChainID);
            this.Controls.Add(this.textBoxPeriodInHours);
            this.Controls.Add(this.labelPeriodHours);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelClusterName);
            this.Controls.Add(this.textBoxRainManChainEnabled);
            this.Controls.Add(this.labelRainManChainEnabled);
            this.Controls.Add(this.textBoxRainManEnabled);
            this.Controls.Add(this.labelRainManEnabled);
            this.Controls.Add(this.textBoxZipcode);
            this.Controls.Add(this.lblZipcode);
            this.Controls.Add(this.TextBoxLotNumber);
            this.Controls.Add(this.lblLotNumber);
            this.Controls.Add(this.lblCommunity);
            this.Controls.Add(this.Community);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.ClusterName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GetAllSoilRecords);
            this.Name = "Form1";
            this.Text = "Soil Moisture Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetAllSoilRecords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ClusterName;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.TextBox Community;
        private System.Windows.Forms.Label lblCommunity;
        private System.Windows.Forms.Label lblLotNumber;
        private System.Windows.Forms.TextBox TextBoxLotNumber;
        private System.Windows.Forms.Label lblZipcode;
        private System.Windows.Forms.TextBox textBoxZipcode;
        private System.Windows.Forms.Label labelRainManEnabled;
        private System.Windows.Forms.TextBox textBoxRainManEnabled;
        private System.Windows.Forms.Label labelRainManChainEnabled;
        private System.Windows.Forms.TextBox textBoxRainManChainEnabled;
        private System.Windows.Forms.Label labelClusterName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelPeriodHours;
        private System.Windows.Forms.TextBox textBoxPeriodInHours;
        private System.Windows.Forms.Label labelChainID;
        private System.Windows.Forms.TextBox textBoxChainID;
        private System.Windows.Forms.Label labelChainContract;
        private System.Windows.Forms.TextBox textBoxChainContract;
        private System.Windows.Forms.Label labelChainKey;
        private System.Windows.Forms.TextBox textBoxChainKey;
        private System.Windows.Forms.Label labelChainServerURL;
        private System.Windows.Forms.TextBox textBoxChainServerURL;
        private System.Windows.Forms.Label labelChainAddress;
        private System.Windows.Forms.TextBox textBoxChainAddress;
        private System.Windows.Forms.Label labelSlice;
        private System.Windows.Forms.TextBox textBoxSlice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonActivate;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonDeactivate;
        private System.Windows.Forms.Label ServiceName;
        private System.Windows.Forms.Label labelServiceName;
        private System.Windows.Forms.Label lblSamples;
        private System.Windows.Forms.Label labelSamples;
        private System.Windows.Forms.Label lblSlices;
        private System.Windows.Forms.Label labelSlices;
        private System.Windows.Forms.Button buttonGetSlice;
    }
}

