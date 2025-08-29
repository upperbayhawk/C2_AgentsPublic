//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Windows.Forms;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Upperbay.Agent.ColonyMatrix;
using Upperbay.Agent.Interfaces;

using Upperbay.Worker.JSON;
using Upperbay.Worker.MQTT;
using Upperbay.Worker.LMP;
using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Assistant;
using System;

namespace GameStarter
{
    public partial class GameStarter : Form
    {
        private GameEventVariable _gameEventVariable = new GameEventVariable();
        private JsonGameEventVariable _jsonGameEventVariable = new JsonGameEventVariable();
        private string _jsonString = "";

        public GameStarter()
        {
            InitializeComponent();

            MyAppConfig.SetMyAppConfig("ClusterAgent");
            string traceMode = MyAppConfig.GetParameter("TraceMode");
            Log2.LogInit("GameStarter", "ClusterAgent", traceMode);
            Log2.Debug("GameStarter: Hello World");

            this.TransparencyKey = System.Drawing.Color.Empty;
            this.GameIdText.Text = "Game" + DateTime.Now.Ticks.ToString();
            
            labelCluster.Text = MyAppConfig.GetParameter("ClusterName");
            labelVersion.Text = MyAppConfig.GetParameter("ClusterVersion");

            // DateTime currentDateTime = DateTime.Now;
            //this.StartTimeText.Value = new DateTime(currentDateTime.Month,
            //                                            currentDateTime.Day,
            //                                            currentDateTime.Year,
            //                                            currentDateTime.Hour,
            //                                            currentDateTime.Minute,
            //                                            0);
        }

        private void creatJsonButton_Click(object sender, System.EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = "config\\ClusterAgent.app.config";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            //MyAppConfig.SetMyAppConfig("ClusterAgent");

            _gameEventVariable.GameName = this.GameNameText.Text;
            _gameEventVariable.GridZone = this.GridZoneText.Text;
            _gameEventVariable.GameType = this.GameTypeText.Text;
            _gameEventVariable.GameAwardRank = this.textBoxGameAwardRank.Text;

            _gameEventVariable.StartTime = this.StartTimeText.Value;
            _gameEventVariable.DurationInMinutes = int.Parse(this.comboBoxDurationInMinutes.Text);
            _gameEventVariable.EndTime = _gameEventVariable.StartTime.AddMinutes(_gameEventVariable.DurationInMinutes);

            string hash = Utilities.ComputeSha256Hash(_gameEventVariable.GameName + _gameEventVariable.StartTime + _gameEventVariable.DurationInMinutes);
            this.GameIdText.Text = hash;
            _gameEventVariable.GameId = this.GameIdText.Text;
            this.GameIdText.Refresh();

            _gameEventVariable.DollarPerPoint = Double.Parse(this.DollarPerPointText.Text);
            _gameEventVariable.PointsPerWatt = Double.Parse(this.PointsPerWattText.Text);
            _gameEventVariable.PointsPerPercent = Double.Parse(this.PointsPerPercentText.Text);
            _gameEventVariable.BonusPoints = Double.Parse(this.BonusPointsText.Text);
            _gameEventVariable.BonusPool = int.Parse(this.BonusPoolText.Text);

            _gameEventVariable.PreStartAlert = this.textBoxPreStartAlert.Text;


            _jsonString = _jsonGameEventVariable.GameEventVariable2Json(_gameEventVariable);
            this.JsonEventString.Text = _jsonString;
            this.JsonEventString.Refresh();
        }


        private void startGameButton_Click(object sender, System.EventArgs e)
        {
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");

            string cluster = MyAppConfig.GetParameter("ClusterName");
            string mqttCloudIpAddress = MyAppConfig.GetClusterParameter(cluster,"MqttCloudIpAddress");
            string mqttCloudPort = MyAppConfig.GetClusterParameter(cluster,"MqttCloudPort");
            string mqttCloudLoginName = MyAppConfig.GetParameter("MqttCloudLoginName");
            string mqttCloudPassword = MyAppConfig.GetParameter("MqttCloudPassword");

            Log2.Info("StartGame: {0} {1} {2} {3}", mqttCloudIpAddress,
                                         mqttCloudPort,
                                         mqttCloudLoginName,
                                         mqttCloudPassword);

            MqttCloudDriver.MqttInitializeAsync(mqttCloudIpAddress,
                                               mqttCloudLoginName,
                                               mqttCloudPassword,
                                               int.Parse(mqttCloudPort));

            MqttCloudDriver.MqttSubscribeAsync(TOPICS.GAME_START_TOPIC);
            Log2.Info("StartGame: Sending - {0}", _jsonString);

            MqttCloudDriver.MqttPublishAsync(TOPICS.GAME_START_TOPIC, _jsonString);
            
            Log2.Info("StartGame: Sending Game to Ledger - {0}", _jsonString);
            GameLedger gameLedger = new GameLedger();
            gameLedger.AddEvent(_gameEventVariable);
            //gameLedger.LogAllEvents();
            MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            MessageBox.Show("Completed", "Start", buttons1);
        }

        private void CFCLabel_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void GetLMP_Click(object sender, EventArgs e)
        {
            //MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            //MessageBox.Show("Not Implemented", "LMP", buttons1);
            this.GetLMPSync();
        }

        private void GetLMPSync()
        {
            //MessageBoxButtons buttons1 = MessageBoxButtons.OK;
            //MessageBox.Show("Not Implemented", "LMP", buttons1);

            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.GetSection("appSettings");
            try
            {
                RealTimeLMP realTimeLMP = new RealTimeLMP();
                Log2.Debug("Calling GetRealTimeLMP");
                double lmp = realTimeLMP.GetRealTimeLMP();
                Log2.Info("LMP: LMP={0}", lmp);
                string color = realTimeLMP.GetRealTimeLMPColor(lmp);
                Log2.Info("Color={0}", color);
                double ppw = realTimeLMP.GetRealTimeLMPPointsPerWatt(lmp);
                Log2.Info("PointsPerWatt={0}", ppw);

                lmp = Math.Round(lmp, 4);
                //decimal lmpDec = Convert.ToDecimal(lmp);

                this.labelLMP.Text = lmp.ToString();
                this.textBoxGameAwardRank.Text = color;
                this.PointsPerWattText.Text = ppw.ToString();
                this.Refresh();

                //MessageBoxButtons buttons1 = MessageBoxButtons.OK;
                //MessageBox.Show("Completed", "Start", buttons1);
            }
            catch (Exception ex)
            {
                Log2.Error(ex.Message);
                MessageBoxButtons buttons1 = MessageBoxButtons.OK;
                MessageBox.Show(ex.Message, "ERROR", buttons1);
            }
        }

        private void labelLMP_Click(object sender, EventArgs e)
        {

        }

        private void GameStarter_Load(object sender, EventArgs e)
        {

        }

        
    }
}
