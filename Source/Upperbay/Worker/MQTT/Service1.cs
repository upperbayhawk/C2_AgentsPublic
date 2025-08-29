using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Upperbay.Worker.Messaging
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class CalculatorService : ICalculator
    {
            public ServiceHost serviceHost = null;

            public CalculatorService()
            {
            }

            public void Start()
            {
                if (serviceHost != null)
                {
                    serviceHost.Close();
                }


                // Create a ServiceHost for the CalculatorService type and provide the base address.
                serviceHost = new ServiceHost(typeof(Upperbay.Worker.Messaging.CalculatorService));
               

                // Open the ServiceHostBase to create listeners and start listening for messages.
                serviceHost.Open();
            }


        public void Stop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }

        public double Add(double n1, double n2)
        {
            double result = n1 + n2;
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            double result = n1 - n2;
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            double result = n1 * n2;
            return result;
        }

        public double Divide(double n1, double n2)
        {
            double result = n1 / n2;
            return result;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}





