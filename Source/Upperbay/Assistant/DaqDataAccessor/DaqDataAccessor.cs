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
using System.Linq;
using System.Text;
using System.Runtime;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Management;
using System.Management.Instrumentation;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;


namespace Upperbay.Assistant
{
    public class DaqDataAccessor : IAgentObjectAssistant
    {
        const MccDaq.DigitalPortType PortNumA = MccDaq.DigitalPortType.FirstPortA; //  set port to use
        const MccDaq.DigitalPortType PortNumB = MccDaq.DigitalPortType.FirstPortB; //  set port to use

        const MccDaq.DigitalPortDirection DirectionA = MccDaq.DigitalPortDirection.DigitalIn; //  set direction of port to 
        const MccDaq.DigitalPortDirection DirectionB = MccDaq.DigitalPortDirection.DigitalIn; //  set direction of port to 
               
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods

        public DaqDataAccessor()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myAgentClassName"></param>
        /// <param name="myAgentObjectName"></param>
        /// <param name="myAgentObject"></param>
        public bool Initialize(string myAgentClassName, string myAgentObjectName, object myAgentObject)
        {
            try
            {

                if ((myAgentClassName != null) && (myAgentObject != null) && (myAgentObjectName != null))
                {
                    _myAgentClassName = myAgentClassName;
                    _myAgentObjectName = myAgentObjectName;
                    _myAgentObject = myAgentObject;

                    _myType = _myAgentObject.GetType();

                    Log2.Trace("{0} DaqDataAccessor: Class: {1}", _myAgentObjectName, _myType.ToString());

                    _myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    if (_myProperties != null)
                    {
                        foreach (string prop in _myProperties)
                        {
                            Log2.Trace("{0}: DaqDataAccessor Attribute: {1}", _myAgentObjectName, prop);
                        }

                        _activeState = true;
                    }
                    else
                    {
                        Log2.Trace("{0}: No DaqDataAccessor Attributes", _myAgentObjectName);
                    }
                }
                else
                {
                    Log2.Error("Start DaqDataAccessor Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start DaqDataAccessor Exception: {0}", Ex.ToString());
            }
            return true;
        }
        // ---------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public bool Start( )
        {
            //  Initiate error handling
            //   activating error handling will trap errors like
            //   bad channel numbers and non-configured conditions.
            //   Parameters:
            //     MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
            //     MccDaq.ErrorHandling.StopAll   :if an error is encountered, the program will stop

            try
            {
                MccDaq.ErrorInfo ULStat = MccDaq.MccService.ErrHandling(MccDaq.ErrorReporting.PrintAll, MccDaq.ErrorHandling.StopAll);
                
                // Create a new MccBoard object for Board 0
                DaqBoard = new MccDaq.MccBoard(0);

                //  configure FirstPortA for digital input
                //   Parameters:
                //     PortNum    :the input port
                //     Direction :sets the port for input or output
                ULStat = DaqBoard.DConfigPort(PortNumA, DirectionA);

                ULStat = DaqBoard.DConfigPort(PortNumB, DirectionB);

            }
            catch (Exception Ex)
            {
                Log2.Error("{0}: Exception in Upperbay.AgentObject.Assistant.DaqDataAccessor: {1}", _myAgentObjectName, Ex.ToString());

            }

            return true;
        }


        // ---------------------------------------------------------------------------------------------------
        public bool Fire()
        {
            if (_activeState)
            {

                //  read FirstPortA digital input and display

                //  Parameters:
                //    PortNum    :the input port
                //    DataValue  :the value read from the port
                short DataValueA = 0;
                short DataValueB = 0;
              

                MccDaq.ErrorInfo ULStatA = DaqBoard.DIn(PortNumA, out DataValueA);
                MccDaq.ErrorInfo ULStatB = DaqBoard.DIn(PortNumB, out DataValueB);

                for (int i = 0; i < 7; i++)
                {
                    iValue[i] = false;
                }

                //  parse DataValueA into bit values to indicate on/off status
                for (int i = 0; i < 8; i++)
                {
                    if ((DataValueA & (1 << i)) != 0)
                    {
                        iValue[i] = true;
                        Log2.Trace("{0}: Agent DaqDataAccessor Bit {1} = 1", _myAgentObjectName, i);
                    }
                    else
                    {
                        iValue[i] = false;
                    }
                }

                for (int i = 8; i < 16; i++)
                {
                    if ((DataValueB & (1 << i)) != 0)
                    {
                        iValue[i] = true;
                        Log2.Trace("{0}: Agent DaqDataAccessor Bit {1} = 1", _myAgentObjectName, i);
                    }
                    else
                    {
                        iValue[i] = false;
                    }
                }

                this.BitA0 = iValue[0];
                this.BitA1 = iValue[1];
                this.BitA2 = iValue[2];
                this.BitA3 = iValue[3];
                this.BitA4 = iValue[4];
                this.BitA5 = iValue[5];
                this.BitA6 = iValue[6];
                this.BitA7 = iValue[7];

                this.BitB0 = iValue[8];
                this.BitB1 = iValue[9];
                this.BitB2 = iValue[10];
                this.BitB3 = iValue[11];
                this.BitB4 = iValue[12];
                this.BitB5 = iValue[13];
                this.BitB6 = iValue[14];
                this.BitB7 = iValue[15];


                //try
                //{
                //    foreach (string prop in _myProperties)
                //    {
                //        PropertyInfo propInfo = _myType.GetProperty(prop);
                //        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                //        double currentValue = 0.0;
                //        if (Double.TryParse(var.Value, out currentValue))
                //        {
                //            currentValue = currentValue + 1;
                //            if (currentValue > 1411.0)
                //                currentValue = 0.0;
                //            var.Quality = "Good";
                //            var.Value = currentValue.ToString();
                //            var.Time = DateTime.Now;
                //            propInfo.SetValue(_myAgentObject, var, null);
                //            Log2.Trace("{0}: Agent DaqDataAccessor Property {1} = {2}", _myAgentObjectName, prop, var.Value);
                //        }
                //    }
                //}
                //catch (Exception Ex)
                //{
                //    Log2.Error("{0}: Exception in Upperbay.AgentObject.Assistant.DaqDataAccessor: {1}", _myAgentObjectName, Ex.ToString());

                //}
            }
            return true;
        }

        // ---------------------------------------------------------------------------------------------------
        public bool Stop()
        {
                        
                //try
                //{
                //    foreach (string prop in _myProperties)
                //    {
                //        PropertyInfo propInfo = _myType.GetProperty(prop);
                //        DataVariable var = (DataVariable)propInfo.GetValue(_myAgentObject, null);
                //        double currentValue = 0.0;
                //        if (Double.TryParse(var.Value, out currentValue))
                //        {
                //            var.Value = currentValue.ToString();
                //            var.Time = DateTime.Now;
                //            var.Quality = "Bad";
                //            propInfo.SetValue(_myAgentObject, var, null);
                //            Log2.Trace("{0}: Agent Simulate Quality = Bad", _myAgentObjectName);
                //        }
                //    }
                //}
                //catch (Exception Ex)
                //{
                //    Log2.Error("{0}: Exception in Upperbay.AgentObject.Assistant.DaqDataAccessor: {1}", _myAgentObjectName, Ex.ToString());

                //}
            _activeState = false;
            return true;
        }

        // ---------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Public State Variables
        /// </summary>

        //Port A Bits

        private bool _bitA0 = false;
        public bool BitA0 { get { return this._bitA0; } set { this._bitA0 = value; } }
        private bool _bitA1 = false;
        public bool BitA1 { get { return this._bitA1; } set { this._bitA1 = value; } }
        private bool _bitA2 = false;
        public bool BitA2 { get { return this._bitA2; } set { this._bitA2 = value; } }
        private bool _bitA3 = false;
        public bool BitA3 { get { return this._bitA3; } set { this._bitA3 = value; } }
        private bool _bitA4 = false;
        public bool BitA4 { get { return this._bitA4; } set { this._bitA4 = value; } }
        private bool _bitA5 = false;
        public bool BitA5 { get { return this._bitA5; } set { this._bitA5 = value; } }
        private bool _bitA6 = false;
        public bool BitA6 { get { return this._bitA6; } set { this._bitA6 = value; } }
        private bool _bitA7 = false;
        public bool BitA7 { get { return this._bitA7; } set { this._bitA7 = value; } }
  

        //Port B Bits

        private bool _bitB0 = false;
        public bool BitB0 { get { return this._bitB0; } set { this._bitB0 = value; } }
        private bool _bitB1 = false;
        public bool BitB1 { get { return this._bitB1; } set { this._bitB1 = value; } }
        private bool _bitB2 = false;
        public bool BitB2 { get { return this._bitB2; } set { this._bitB2 = value; } }
        private bool _bitB3 = false;
        public bool BitB3 { get { return this._bitB3; } set { this._bitB3 = value; } }
        private bool _bitB4 = false;
        public bool BitB4 { get { return this._bitB4; } set { this._bitB4 = value; } }
        private bool _bitB5 = false;
        public bool BitB5 { get { return this._bitB5; } set { this._bitB5 = value; } }
        private bool _bitB6 = false;
        public bool BitB6 { get { return this._bitB6; } set { this._bitB6 = value; } }
        private bool _bitB7 = false;
        public bool BitB7 { get { return this._bitB7; } set { this._bitB7 = value; } }

        #endregion

        #region Private State Variables


        private AgentPassPort _agentPassPort = null;
        private MccDaq.MccBoard DaqBoard = null;

        bool[] iValue = new bool[16];
        
        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;

        //private TraceSwitch _myTraceSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;

        private bool _activeState = false;
        private ArrayList _myProperties = null;
        private Type _myType = null;

        private string _attributeString = "mccdaq";
        #endregion
    }
}
