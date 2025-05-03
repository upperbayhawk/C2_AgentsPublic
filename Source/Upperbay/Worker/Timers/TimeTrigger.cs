//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;

//DateTimeOffset.UtcNow.ToUnixTimeSeconds()
//To get the timestamp from a DateTime:
//DateTime foo = DateTime.UtcNow;
//long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();


namespace Upperbay.Worker.Timers
{
    public class TimeTrigger
    {


        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool IsHour(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            if (currentMinute == offset)
            {
                if (_hourFlag == false)
                {
                    _hourFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _hourFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is1Min(int offset = 0)
        {
            int currentSecond = DateTime.Now.Second;

            if ((currentSecond > 0) && (currentSecond < 15))
            {
                if (_1minFlag == false)
                {
                    _1minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _1minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is2Min(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            int modulo = currentMinute % 2;
            if (modulo == offset)
            {
                if (_2minFlag == false)
                {
                    _2minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _2minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is5Min(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            int modulo = currentMinute % 5;
            if (modulo == offset)
            {
                if (_5minFlag == false)
                {
                    _5minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _5minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is10Min(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            int modulo = currentMinute % 10;
            if (modulo == offset)
            {
                if (_10minFlag == false)
                {
                    _10minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _10minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is15Min(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            int modulo = currentMinute % 15;
            if (modulo == offset)
            {
                if (_15minFlag == false)
                {
                    _15minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _15minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is20Min(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            int modulo = currentMinute % 20;
            if (modulo == offset)
            {
                if (_20minFlag == false)
                {
                    _20minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _20minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is30Min(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            int modulo = currentMinute % 30;
            if (modulo == offset)
            {
                if (_30minFlag == false)
                {
                    _30minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _30minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is45Min(int offset = 0)
        {
            Int64 currentSeconds = DateTime.Now.Ticks / ((Int64)10000000);
            Int64 currentMinutes = currentSeconds / 60;
            Int64 modulo = currentMinutes % 45;
            if (modulo == 0)
            {
                if (_45minFlag == false)
                {
                    _45minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _45minFlag = false;
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is60Min(int offset = 0)
        {
            int currentMinute = DateTime.Now.Minute;
            if (currentMinute == offset)
            {
                if (_60minFlag == false)
                {
                    _60minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _60minFlag = false;
                return false;
            }
        }

        //TEST
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is90Min(int offset = 0)
        {


            Int64 currentSeconds = DateTime.Now.Ticks / ((Int64)10000000);
            Int64 currentMinutes = currentSeconds / 60;
            Int64 modulo = currentMinutes % 90;
            if (modulo == 0)
            {
                if (_90minFlag == false)
                {
                    _90minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _90minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is120Min(int offset = 0)
        {
            int currentHour = DateTime.Now.Hour;
            int modulo = currentHour % 2;
            if (modulo == 0)
            {
                if (_120minFlag == false)
                {
                    _120minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _120minFlag = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Is240Min(int offset = 0)
        {
            int currentHour = DateTime.Now.Hour;
            int modulo = currentHour % 4;
            if (modulo == 0)
            {
                if (_240minFlag == false)
                {
                    _240minFlag = true;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                _240minFlag = false;
                return false;
            }
        }
        #endregion

        #region Private State
        private bool _hourFlag = false;
        private bool _1minFlag = false;
        private bool _2minFlag = false;
        private bool _5minFlag = false;
        private bool _10minFlag = false;
        private bool _15minFlag = false;
        private bool _20minFlag = false;
        private bool _30minFlag = false;
        private bool _45minFlag = false;
        private bool _60minFlag = false;
        private bool _90minFlag = false;
        private bool _120minFlag = false;
        private bool _240minFlag = false;
        #endregion}
    }
}

