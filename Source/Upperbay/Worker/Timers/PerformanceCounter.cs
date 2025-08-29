//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System.Diagnostics;




namespace Upperbay.Worker.Timers
{
    class PerformanceCounter
    {
        public static bool CreatePerfCounter()
        {
            string categoryName = "ClarAgent";
            string counterName = "Number of Cycles";

            // Check if the category already exists or not.
            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                CounterCreationDataCollection creationData =
                    new CounterCreationDataCollection();

                // Create custom counter object.
                creationData.Add(new CounterCreationData(counterName,
                    "Number of cycle executed by  main loop",
                    PerformanceCounterType.NumberOfItems32));


                // Bind the counters to a PerformanceCounterCategory.

                //PerformanceCounterCategory myCategory =
                //    System.Diagnostics.PerformanceCounterCategory.Create(
                //    categoryName,
                //    "Upperbay's ClarAgent Library",
                //    creationData);
            }
            else
            {
            }

            return (true);
        }

        public static bool DeletePerfCounter()
        {
            string categoryName = "ClarAgent";

            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                PerformanceCounterCategory.Delete(categoryName);
            }

            return (true);
        }



        public static bool UpdatePerfCounter(int count)
        {
            //string categoryName = "ClarAgent";
            //string counterName = "Number of Cycles";
            
            //PerformanceCounter counter = new PerformanceCounter(
            //   categoryName,
            //    counterName,
            //    false);

            //counter.RawValue = count;
            return (true);
        }

    }
}
