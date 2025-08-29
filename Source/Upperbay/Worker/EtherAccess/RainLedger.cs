//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;
using Upperbay.Worker.EtherAccess;
using Nethereum.Signer;
using System.Threading.Tasks;

namespace Upperbay.Worker.EtherAccess
{
    public class RainLedger
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public RainLedger()
        {

            string rainManChainEnabled = MyAppConfig.GetParameter("RainManChainEnabled");
            _ethereumExternalAddress = MyAppConfig.GetParameter("RainManChainAddress");
            _ethereumExternalPrivateKey = MyAppConfig.GetParameter("RainManChainKey");
            _ethereumServerURL = MyAppConfig.GetParameter("RainManServerURL");
            _ethereumContractAddress = MyAppConfig.GetParameter("RainManChainContract");
            string sChainId = MyAppConfig.GetParameter("RainManChainID");
            _ethereumChainId = Int32.Parse(sChainId);


        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddSoilRecord(
                                    string soilDateTime,
                                    string soilZipcode,
                                    string soilCommunity,
                                    string soilLot,
                                    string soilDeviceName,
                                    string soilTemp,
                                    string soilHumidity
                                    )
        {
            Log2.Trace("Calling AddEvent");
            Log2.Debug("Calling AddSoilRecord");
            EtherRainManAccess etherRainManAccess = new EtherRainManAccess();

            Log2.Debug("EtherAccess: {0} {1} {2} {3} {4}",
                                    _ethereumServerURL,
                                    _ethereumContractAddress,
                                    _ethereumExternalAddress,
                                    _ethereumExternalPrivateKey,
                                    _ethereumChainId);


            etherRainManAccess.ConfigureAccess(
                                    "RainMain",
                                    _ethereumServerURL,
                                    _ethereumContractAddress,
                                    _ethereumExternalAddress,
                                    _ethereumExternalPrivateKey,
                                    _ethereumChainId);

            Log2.Debug("SoilData: {0} {1} {2} {3} {4} {5} {6}",
                                     soilDateTime,
                                    soilZipcode,
                                    soilCommunity,
                                    soilLot,
                                    soilDeviceName,
                                    soilTemp,
                                    soilHumidity);


            await etherRainManAccess.AddSoilRecord(
                                    soilDateTime,
                                    soilZipcode,
                                    soilCommunity,
                                    soilLot,
                                    soilDeviceName,
                                    soilTemp,
                                    soilHumidity);

            Log2.Debug("etherRainManAccess.AddSoilRecord Returned");


            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LogSoilEvents(uint slice)
        {

            EtherRainManAccess etherRainManAccess = new EtherRainManAccess();

            Log2.Debug("EtherAccess: {0} {1} {2} {3} {4}",
                                    _ethereumServerURL,
                                    _ethereumContractAddress,
                                    _ethereumExternalAddress,
                                    _ethereumExternalPrivateKey,
                                    _ethereumChainId);


            etherRainManAccess.ConfigureAccess(
                                "RainMain",
                                _ethereumServerURL,
                                _ethereumContractAddress,
                                _ethereumExternalAddress,
                                _ethereumExternalPrivateKey,
                                _ethereumChainId);


            await etherRainManAccess.LogSoilEvents(slice);

            Log2.Debug("etherRainManAccess.LogSoilEvents Returned");

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSoilCount()
        {
            try
            {

                EtherRainManAccess etherRainManAccess = new EtherRainManAccess();

                Log2.Debug("EtherAccess: {0} {1} {2} {3} {4}",
                                        _ethereumServerURL,
                                        _ethereumContractAddress,
                                        _ethereumExternalAddress,
                                        _ethereumExternalPrivateKey,
                                        _ethereumChainId);


                etherRainManAccess.ConfigureAccess(
                                    "RainMain",
                                    _ethereumServerURL,
                                    _ethereumContractAddress,
                                    _ethereumExternalAddress,
                                    _ethereumExternalPrivateKey,
                                    _ethereumChainId);


                int i = etherRainManAccess.GetSoilCount();
               
                Log2.Debug("etherRainManAccess.GetSoilCount: {0}", i);

                return i;
            }
            catch (Exception ex)
            {
                Log2.Error("Exception: {0}", ex.ToString());
                return - 1;
            }
   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSliceCount()
        {
            try
            {

            
            EtherRainManAccess etherRainManAccess = new EtherRainManAccess();

            Log2.Debug("EtherAccess: {0} {1} {2} {3} {4}",
                                    _ethereumServerURL,
                                    _ethereumContractAddress,
                                    _ethereumExternalAddress,
                                    _ethereumExternalPrivateKey,
                                    _ethereumChainId);


            etherRainManAccess.ConfigureAccess(
                                "RainMain",
                                _ethereumServerURL,
                                _ethereumContractAddress,
                                _ethereumExternalAddress,
                                _ethereumExternalPrivateKey,
                                _ethereumChainId);


            int i = etherRainManAccess.GetSoilCount();
            int iSlice = (i / 9999) + 1;
            Log2.Debug("etherRainManAccess.GetSliceCount: {0}", iSlice);

            return iSlice;
            }
            catch (Exception ex)
            {
                Log2.Error("Exception: {0}", ex.ToString());
                return -1;
            }
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
