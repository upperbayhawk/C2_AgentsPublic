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
using System.Threading.Tasks;

using Upperbay.Core.Logging;
using Upperbay.Core.Library;

namespace Upperbay.Worker.LMP
{
    /// <summary>
    /// 
    /// </summary>
    public class RealTimeLMP
    {
        private string slmpLowThreshold = "40.0";
        private double lmpLowThreshold = 40.0;

        private string slmpHighThreshold = "70.0";
        private double lmpHighThreshold = 70.0;

        public RealTimeLMP()
        {
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetRealTimeLMP()
        {
            double result = 0.0;

            string lmpEnable = MyAppConfig.GetParameter("LMPEnable");
            Log2.Debug("lmpEnable={0}", lmpEnable);

            string cluster = MyAppConfig.GetParameter("ClusterName");
            Log2.Debug("cluster={0}", cluster);

            string rto = MyAppConfig.GetClusterParameter(cluster, "LMPRTO");
            Log2.Debug("rto={0}", rto);

            if (lmpEnable == "true")
            {
                switch (rto)
                {
                    case "PJM":
                        PJMRealTimeLMP pjmLMP = new PJMRealTimeLMP();
                        //result = pjmLMP.GetPJMRealTimeLMPAsync().Result;
                        result = pjmLMP.GetPJMRealTimeLMP();
                        break;
                    case "NEISO":
                        result = 0.0;
                        break;
                    case "CAISO":
                        result = 0.0;
                        break;
                    case "MISO":
                        result = 0.0;
                        break;
                    default:
                        result = 0.0;
                        break;
                }
            }
            else
            {
                result = 20.0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lmp"></param>
        /// <returns></returns>
        public string GetRealTimeLMPColor(double lmp)
        {
            slmpLowThreshold = MyAppConfig.GetParameter("LMPLowThreshold");
            lmpLowThreshold = Double.Parse(slmpLowThreshold);

            slmpHighThreshold = MyAppConfig.GetParameter("LMPHighThreshold");
            lmpHighThreshold = Double.Parse(slmpHighThreshold);

            string color = "BRONZE";

            if (lmp < lmpLowThreshold)
                color = "BRONZE";
            else if ((lmp >= lmpLowThreshold) && (lmp < lmpHighThreshold))
                color = "SILVER";
            else if (lmp >= lmpHighThreshold)
                color = "GOLD";

            return color;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lmp"></param>
        /// <returns></returns>
        public string GetRealTimeLMPDollarPerPoint(double lmp)
        {
            slmpLowThreshold = MyAppConfig.GetParameter("LMPLowThreshold");
            lmpLowThreshold = Double.Parse(slmpLowThreshold);

            slmpHighThreshold = MyAppConfig.GetParameter("LMPHighThreshold");
            lmpHighThreshold = Double.Parse(slmpHighThreshold);

            double dpp = lmp / 10;

            if ((lmp > 0) && (lmp < lmpLowThreshold))
                dpp = lmp / 20;
            else if ((lmp >= lmpLowThreshold) && (lmp < lmpHighThreshold))
                dpp = lmp / 20;
            else if (lmp >= lmpHighThreshold)
                dpp = lmp / 20;

            return dpp.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lmp"></param>
        /// <returns></returns>
        public double GetRealTimeLMPPointsPerWatt(double lmp)
        {
            slmpLowThreshold = MyAppConfig.GetParameter("LMPLowThreshold");
            lmpLowThreshold = Double.Parse(slmpLowThreshold);

            slmpHighThreshold = MyAppConfig.GetParameter("LMPHighThreshold");
            lmpHighThreshold = Double.Parse(slmpHighThreshold);

            double ppw = 1.00;

            if ((lmp > 0) && (lmp < lmpLowThreshold))
                ppw = 1.00;
            else if ((lmp >= lmpLowThreshold) && (lmp < lmpHighThreshold))
                ppw = 4.00;
            else if (lmp >= lmpHighThreshold)
                ppw = 8.00;

            return ppw;
        }
    }
}
