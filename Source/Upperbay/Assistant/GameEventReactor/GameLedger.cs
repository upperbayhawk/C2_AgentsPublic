//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Upperbay.Agent.Interfaces;
using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Worker.EtherAccess;
using Upperbay.Worker.PostOffice;
using Upperbay.Worker.Voice;

namespace Upperbay.Assistant
{
    public class GameLedger
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public GameLedger()
        {
            string gameLedgerEnabled = MyAppConfig.GetParameter("GameLedgerEnabled");
            if (gameLedgerEnabled != null) Boolean.TryParse(gameLedgerEnabled, out _gameLedgerEnabled);

            _gamePlayerID = MyAppConfig.GetParameter("GamePlayerID");
            _currentForCarbonWatts = MyAppConfig.GetParameter("CurrentForCarbonWatts");
            _ethereumExternalAddress = MyAppConfig.GetParameter("EthereumExternalAddress");
            _ethereumExternalPrivateKey = MyAppConfig.GetParameter("EthereumExternalPrivateKey");
            string cluster = MyAppConfig.GetParameter("ClusterName");

            string gamePlayerNumber = MyAppConfig.GetParameter("GamePlayerNumber");
            string adminOnly = MyAppConfig.GetParameter("AdminOnly");
            int playerNum = Int32.Parse(gamePlayerNumber);
            if ((playerNum == 0) && (adminOnly == "true"))
            {
                 _ethereumServerURL = MyAppConfig.GetParameter("LocalEthereumServerURL");
            }
            else
            {
                _ethereumServerURL = MyAppConfig.GetClusterParameter(cluster,"RemoteEthereumServerURL");
            }

            _ethereumContractAddress = MyAppConfig.GetClusterParameter(cluster,"EthereumContractAddress");
            string sChainId = MyAppConfig.GetClusterParameter(cluster,"EthereumChainId");

            _gamePlayerSignature = MyAppConfig.GetParameter("GamePlayerSignature");
            _ethereumOracleName = MyAppConfig.GetParameter("EthereumOracleName");
            _ethereumChainId = Int32.Parse(sChainId);


        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool AddPlayer()
        {
            if (_gameLedgerEnabled)
            {
                string _playerSignature = Utilities.ComputeSha256Hash(_currentForCarbonWatts + _ethereumExternalAddress + _gamePlayerID);
                Log2.Debug("_playerSignatures: {0} {1}", _playerSignature, _gamePlayerSignature);
                if (String.Compare(_playerSignature, _gamePlayerSignature) != 0)
                {
                    //Signatures are different!
                    Log2.Error("Player Signatures do not match! {0} {1}", _playerSignature, _gamePlayerSignature);
                }

                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                // do not wait
                etherCurrentCarbonAccess.AddPlayer(
                                        _gamePlayerID,
                                        _currentForCarbonWatts
                                         );
            }
            return true;
        }

        public bool AddPlayerFromConfidential(List<GamePlayerConfidential> gpcList)
        {
            if (_gameLedgerEnabled)
            {
                
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                // do not wait
                etherCurrentCarbonAccess.AddPlayerFromConfidential(gpcList);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gev"></param>
        /// <returns></returns>
        public bool AddEvent(GameEventVariable gev)
        {
            Log2.Trace("Calling AddEvent");
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

  

                // do not wait
                etherCurrentCarbonAccess.AddEvent(
                                        gev.GameId,
                                        gev.GameName,
                                        gev.GameType,
                                        gev.StartTime.ToString(),
                                        gev.EndTime.ToString(),
                                        gev.DurationInMinutes.ToString(),
                                        gev.DollarPerPoint.ToString(),
                                        gev.PointsPerWatt.ToString(),
                                        gev.PointsPerPercent.ToString());
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grv"></param>
        /// <returns></returns>
        public bool AddResult(GameResultsVariable grv)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                // do not wait
                etherCurrentCarbonAccess.AddResult(
                                            grv.GamePlayerId,
                                            grv.GameId,
                                            grv.GameAvgPowerInWatts.ToString(),
                                            grv.BaselineAvgPowerInWatts.ToString(),
                                            grv.DeltaPowerInWatts.ToString(),
                                            grv.PercentPoints.ToString(),
                                            grv.WattPoints.ToString(),
                                            grv.PointsAwarded.ToString(),
                                            grv.AwardValue.ToString());
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamePlayerID"></param>
        /// <returns></returns>
        //public bool LogPlayer(string gamePlayerID)
        //{
        //    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

        //    etherCurrentCarbonAccess.ConfigureAccess(
        //                            _ethereumOracleName,
        //                            _ethereumServerURL,
        //                            _ethereumContractAddress,
        //                            _ethereumExternalAddress,
        //                            _ethereumExternalPrivateKey,
        //                            _ethereumChainId);

        //    etherCurrentCarbonAccess.LogPlayer(gamePlayerID);
        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameEventID"></param>
        /// <returns></returns>
        //public bool LogEvent(string gameEventID)
        //{

        //    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

        //    etherCurrentCarbonAccess.ConfigureAccess(
        //                            _ethereumOracleName,
        //                            _ethereumServerURL,
        //                            _ethereumContractAddress,
        //                            _ethereumExternalAddress,
        //                            _ethereumExternalPrivateKey,
        //                            _ethereumChainId);

        //    etherCurrentCarbonAccess.LogEvent(gameEventID);
        //    return true;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameEventID"></param>
        /// <returns></returns>
        //public bool LogResultsForEvent(string gameEventID)
        //{

        //    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

        //    etherCurrentCarbonAccess.ConfigureAccess(
        //                            _ethereumOracleName,
        //                            _ethereumServerURL,
        //                            _ethereumContractAddress,
        //                            _ethereumExternalAddress,
        //                            _ethereumExternalPrivateKey,
        //                            _ethereumChainId);

        //    etherCurrentCarbonAccess.LogResultsForEvent(gameEventID);
        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamePlayerID"></param>
        /// <returns></returns>
        //public bool LogResultsForPlayer(string gamePlayerID)
        //{
        //    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

        //    etherCurrentCarbonAccess.ConfigureAccess(
        //                            _ethereumOracleName,
        //                            _ethereumServerURL,
        //                            _ethereumContractAddress,
        //                            _ethereumExternalAddress,
        //                            _ethereumExternalPrivateKey,
        //                            _ethereumChainId);

        //    etherCurrentCarbonAccess.LogResultsForPlayer(gamePlayerID);

        //    return true;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool LogAllEvents(uint slice)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.LogAllEvents(slice);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool LogAllPlayers(uint slice)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.LogAllPlayers(slice);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool LogAllResults(uint slice)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.LogAllResults(slice);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool LogAllCombinedEventResults(uint slice)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.LogAllCombinedEventResults(slice);
            }
            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamePlayerId"></param>
        /// <returns></returns>
        public bool LogMyResults(uint slice, string gamePlayerId)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.LogMyResults(slice,gamePlayerId);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gamePlayerId"></param>
        /// <returns></returns>
        public bool LogMyCombinedEventResults(uint slice, 
                                            string gamePlayerId)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.LogMyCombinedEventResults(slice, gamePlayerId);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSliceCount()
        {
            if (_gameLedgerEnabled)
            {
                try
                {


                    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                    etherCurrentCarbonAccess.ConfigureAccess(
                                            _ethereumOracleName,
                                            _ethereumServerURL,
                                            _ethereumContractAddress,
                                            _ethereumExternalAddress,
                                            _ethereumExternalPrivateKey,
                                            _ethereumChainId);

                    int i = etherCurrentCarbonAccess.GetSliceCount();

                    Log2.Debug("etherCurrentCarbonAccess.GetSliceCount: {0}", i);

                    return i;
                }
                catch( Exception ex)
                {
                    Log2.Error("Error: {0}",ex);
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPlayerCount()
        {
            if (_gameLedgerEnabled)
            {
                try
                {


                    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                    etherCurrentCarbonAccess.ConfigureAccess(
                                            _ethereumOracleName,
                                            _ethereumServerURL,
                                            _ethereumContractAddress,
                                            _ethereumExternalAddress,
                                            _ethereumExternalPrivateKey,
                                            _ethereumChainId);

                    int i = etherCurrentCarbonAccess.GetPlayerCount();

                    Log2.Debug("etherCurrentCarbonAccess.GetPlayerCount: {0}", i);

                    return i;
                }
                catch (Exception ex)
                {
                    Log2.Error("Error: {0}", ex);
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetEventCount()
        {
            if (_gameLedgerEnabled)
            {
                try
                {


                    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                    etherCurrentCarbonAccess.ConfigureAccess(
                                            _ethereumOracleName,
                                            _ethereumServerURL,
                                            _ethereumContractAddress,
                                            _ethereumExternalAddress,
                                            _ethereumExternalPrivateKey,
                                            _ethereumChainId);

                    int i = etherCurrentCarbonAccess.GetEventCount();

                    Log2.Debug("etherCurrentCarbonAccess.GetEventCount: {0}", i);

                    return i;
                }
                catch (Exception ex)
                {
                    Log2.Error("Error: {0}", ex);
                    return -1;
                }
            }
            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetResultCount()
        {
            if (_gameLedgerEnabled)
            {
                try
                {


                    EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                    etherCurrentCarbonAccess.ConfigureAccess(
                                            _ethereumOracleName,
                                            _ethereumServerURL,
                                            _ethereumContractAddress,
                                            _ethereumExternalAddress,
                                            _ethereumExternalPrivateKey,
                                            _ethereumChainId);

                    int i = etherCurrentCarbonAccess.GetResultCount();

                    Log2.Debug("etherCurrentCarbonAccess.GetResultCount: {0}", i);

                    return i;
                }
                catch (Exception ex)
                {
                    Log2.Error("Error: {0}", ex);
                    return -1;
                }
            }
            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromPrivateKey"></param>
        /// <param name="toAddress"></param>
        /// <param name="ethToTransfer"></param>
        /// <returns></returns>
        public bool SendMyEther(string name, string fromPrivateKey, string toAddress, string ethToTransfer)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.SendEther(name, fromPrivateKey, toAddress, ethToTransfer);
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromPrivateKey"></param>
        /// <param name="toAddress"></param>
        /// <param name="ethToTransfer"></param>
        /// <returns></returns>
        public bool TransferEther(string name, string fromPrivateKey, string toAddress, string ethToTransfer)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.TransferEther(name, fromPrivateKey, toAddress, ethToTransfer);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gpcList"></param>
        /// <returns></returns>
        public bool TransferEtherFromConfidential(List<GamePlayerConfidential> gpcList, string ether)
        {
            if (_gameLedgerEnabled)
            {

                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                // do not wait
                etherCurrentCarbonAccess.TransferEtherFromConfidential(gpcList, ether);
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromPrivateKey"></param>
        /// <param name="toAddress"></param>
        /// <returns></returns>
        public bool GetMyEther(string name, string fromPrivateKey, string toAddress)
        {
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);

                etherCurrentCarbonAccess.GetEther(name, fromPrivateKey, toAddress);
            }
            return true;
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool KillContract()
        {
            Log2.Trace("Calling KillContract");
            if (_gameLedgerEnabled)
            {
                EtherCurrentCarbonAccess etherCurrentCarbonAccess = new EtherCurrentCarbonAccess();

                etherCurrentCarbonAccess.ConfigureAccess(
                                        _ethereumOracleName,
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);



                // do not wait
                etherCurrentCarbonAccess.KillContract();
                                       
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public bool SetResultConfirmation(bool confirmed)
        {
            return true;
        }


  

        #endregion

        #region Private State
        //private GameResultsVariable _gameResult;
        //private GameEventVariable _gameEvent;

        private bool _gameLedgerEnabled;
        private string _ethereumServerURL;
        private string _gamePlayerID;
        private string _ethereumExternalAddress;
        private string _ethereumExternalPrivateKey;
        private string _ethereumContractAddress;
        private string _gamePlayerSignature;
        private string _ethereumOracleName;
        private string _currentForCarbonWatts;
        private int _ethereumChainId;
        
        #endregion
    }

}
