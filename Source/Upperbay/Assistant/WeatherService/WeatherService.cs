//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Xml;


using Upperbay.Core.Logging;
using Upperbay.Core.Library;
using Upperbay.Agent.Interfaces;

using Upperbay.Worker.AstronomyClock;



namespace Upperbay.Assistant
{
    public class WeatherService : IAgentObjectAssistant
    {

       
        public AgentPassPort AgentPassPort { get { return this._agentPassPort; } set { this._agentPassPort = value; } }

        #region Methods

        public WeatherService()
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

                    Log2.Trace("{0} WeatherService: Class: {1}", _myAgentObjectName, _myType.ToString());

                    //_myProperties = Utilities.GetDecoratedProperties(_myType, _attributeString);
                    //if (_myProperties != null)
                    //{
                    //    foreach (string prop in _myProperties)
                    //    {
                    //        Log2.Trace("{0}: Simulated Attribute: {1}", _myAgentObjectName, prop);
                    //    }

                    _activeState = true;
                    //}
                    //else
                    //{
                    //    Log2.Trace("{0}: No Simulated Attributes", _myAgentObjectName);
                    //}
                }
                else
                {
                    Log2.Error("Start WeatherService Failed!");
                }
            }
            catch (Exception Ex)
            {
                Log2.Error("Start WeatherService Exception: {0}", Ex.ToString());
            }
            return true;
        }
        

        /// <summary>
        /// 
        /// </summary>
        public bool Start()
        {
            if (_activeState)
            {
                try
                {
                    _timeLastExecuted = DateTime.MinValue;
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService Start: {1}", _myAgentObjectName, Ex.ToString());

                }
}
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Fire()
        {
            if (_activeState)
            {
                try
                {
                    TimeSpan duration = DateTime.Now - _timeLastExecuted;

                    if (duration.Minutes > 10)
                    {
                        GetNOAAWeatherData();
                        GetCurrentConditions();
                        GetCurrentAlerts();
                        CalcSunriseSunset();

                        _timeLastExecuted = DateTime.Now;

                    }
                    CalculateTimes();
                    CalculateEvents();

                    return true;
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService Fire: {1}", _myAgentObjectName, Ex.ToString());
                    return false;
                }

            }
            return false;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (_activeState)
            {
                try
                {
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService Stop: {1}", _myAgentObjectName, Ex.ToString());

                }
            }
            _activeState = false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool GetCurrentConditions()
        {
            if (_activeState)
            {
                try
                {
                    System.Xml.XmlDocument xmlDoc = new XmlDocument();
                    string weatherUrl = "http://www.weather.gov/data/current_obs/" + WeatherStationID + ".xml";
                    xmlDoc.Load(weatherUrl);
                    xmlDoc.Save("WeatherServiceConditions.xml");

                    // weather values
                    this.CurrentTemperature = xmlDoc.SelectSingleNode("//temp_f").InnerText;
                    this.CurrentWeather = xmlDoc.SelectSingleNode("//weather").InnerText;
                    this.CurrentHumidity = xmlDoc.SelectSingleNode("//relative_humidity").InnerText;
                    this.CurrentWindDirection = xmlDoc.SelectSingleNode("//wind_dir").InnerText;
                    this.CurrentWindSpeed = xmlDoc.SelectSingleNode("//wind_mph").InnerText;
                    this.CurrentWindGust = xmlDoc.SelectSingleNode("//wind_gust_mph").InnerText;
                    this.CurrentHeatIndex = xmlDoc.SelectSingleNode("//heat_index_f").InnerText;
                    this.CurrentWindchill = xmlDoc.SelectSingleNode("//windchill_f").InnerText;
                    this.CurrentLastUpdated = xmlDoc.SelectSingleNode("//observation_time").InnerText;

                    
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService GetCurrentConditions: {1}", _myAgentObjectName, Ex.ToString());
                    return false;
                }

            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool GetCurrentAlerts()
        {
            if (_activeState)
            {
                try
                {
                    XmlDocument watchesRssDoc = new XmlDocument();
                    
                    string weatherAlert = "http://www.weather.gov/alerts/wwarssget.php?zone=" + WeatherAlertZone;
                    watchesRssDoc.Load(weatherAlert);
                    watchesRssDoc.Save("WeatherServiceAlerts.xml");

                    this.CurrentAlertWarning = watchesRssDoc.SelectSingleNode("//item/description").InnerText;
                    Log2.Trace("{0}: Weather Alerts: {1}", _myAgentObjectName, this.CurrentAlertWarning);
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService GetCurrentAlerts: {1}", _myAgentObjectName, Ex.ToString());
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CalculateEvents()
        {
            if (_activeState)
            {
                try
                {
                        // Process Sunrise Data

                        double mySunriseMinutes = DateTime.Parse(this.TimeOfSunrise).TimeOfDay.TotalMinutes;
                        if ((DateTime.Now.TimeOfDay.TotalMinutes > (mySunriseMinutes)) &&
                            (DateTime.Now.TimeOfDay.TotalMinutes <= (mySunriseMinutes + 10)) &&
                            (this.IsSunrise == "false"))
                        {
                            
                            this.IsSunrise = "true";
                            Log2.Event("{0}: Sunrise", _myAgentObjectName);
                        }
                        else if ((DateTime.Now.TimeOfDay.TotalMinutes > (mySunriseMinutes + 10)) &&
                            (this.IsSunrise == "true"))
                        {
                            this.IsSunrise = "false";
                            Log2.Event("{0}: Sunrise Reset", _myAgentObjectName);
                        }

                        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                        // Process Sunset Data

                        double mySunsetMinutes = DateTime.Parse(this.TimeOfSunset).TimeOfDay.TotalMinutes;
                        if ((DateTime.Now.TimeOfDay.TotalMinutes > (mySunsetMinutes)) &&
                            (DateTime.Now.TimeOfDay.TotalMinutes < (mySunsetMinutes + 10)) &&
                            (this.IsSunset == "false"))
                        {
                            //
                            this.IsSunset = "true";
                            Log2.Event("{0}: Sunset", _myAgentObjectName);
                        }
                        else if (((DateTime.Now.TimeOfDay.TotalMinutes > (mySunsetMinutes + 10)) &&
                            (this.IsSunset == "true")))
                        {
                            this.IsSunset = "false";
                            Log2.Event("{0}: Sunset Reset", _myAgentObjectName);
                        }

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService CalculateEvents: {1}", _myAgentObjectName, Ex.ToString());
                    return false;
                }

            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CalculateTimes()
        {
            if (_activeState)
            {
                try
                {
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    //Process Morning
                    if ((DateTime.Now.Hour >= 6) &&
                        (DateTime.Now.Hour < 12))
                    {
                        //
                        this.IsMorning = "true";
                    }
                    else
                    {
                        this.IsMorning = "false";
                    }
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    // Process Afternoon
                    if ((DateTime.Now.Hour >= 12) &&
                        (DateTime.Now.Hour <= 17)
                        )
                    {
                        //
                        this.IsAfternoon = "true";
                    }
                    else
                    {
                        this.IsAfternoon = "false";
                    }
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    // Process Evening
                    if ((DateTime.Now.Hour > 17) &&
                        (DateTime.Now.Hour < 21)
                        )
                    {
                        //
                        this.IsEvening = "true";
                    }
                    else
                    {
                        this.IsEvening = "false";
                    }
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    // Process Night
                    if ((DateTime.Now.Hour >= 21) ||
                        (DateTime.Now.Hour < 6)
                        )
                    {
                        //
                        this.IsNight = "true";
                        this.IsDay = "false";
                    }
                    else
                    {
                        this.IsNight = "false";
                        this.IsDay = "true";
                    }
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService CalculateTimes: {1}", _myAgentObjectName, Ex.ToString());
                    return false;
                }

            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool GetNOAAWeatherData()
        {
            ndfdXML noaaWeatherProxy = null;

            try
            {
                    //````````````````````````````````````````````````````````````````````````````````
                    // Create the proxy class
                    //wsdl.exe http://weather.gov/forecasts/xml/DWMLgen/wsdl/ndfdXML.wsdl
                    noaaWeatherProxy = new ndfdXML();

                    if (this.Url != null)
                    {
                        noaaWeatherProxy.Url = this.Url;
                    }

                    // Retrieve the XML data for a specified latitude/longitude,
                    //  start date, and number of days to forecast.

                    //Decimal latitude = 42.0869M;
                    //Decimal longitude = -71.3873M;
                    string numDays = "5";

                    string latLon = noaaWeatherProxy.LatLonListZipCode(this.WeatherZipCode);
                    Log2.Trace("{0}: Weather Data: LATLON {1}", _myAgentObjectName, latLon);

                    XmlDocument xLatLon = new XmlDocument();
                    xLatLon.LoadXml(latLon);
                    xLatLon.Save("WeatherServiceZipCode.xml");
                    XmlNodeList latLonText = xLatLon.SelectNodes("/dwml/latLonList/text()");
                    string latLonString = latLonText[0] != null ? latLonText[0].Value : string.Empty;
                    string[] latLonArray = latLonString.Split(new Char [] {','});
                    Decimal latitude = Decimal.Parse(latLonArray[0]);
                    Decimal longitude = Decimal.Parse(latLonArray[1]);

                    this.Latitude = Double.Parse(latLonArray[0]);
                    this.Longitude = Double.Parse(latLonArray[1]);

                    //----------------------------------------------------------------------------------------------------

                    string xmlData = noaaWeatherProxy.NDFDgenByDay(latitude, longitude,
                                               DateTime.Now, numDays,
                                               formatType.Item12hourly);
                    Log2.Trace("{0}: Weather Data: {1}", _myAgentObjectName, xmlData);


                    // load up the XML data...
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlData);
                    doc.Save("WeatherServiceFiveDayForecast.xml");

                    XmlNodeList twelvehourtimes = doc.SelectNodes("/dwml/data/time-layout[3]/start-valid-time/text()");
                    XmlNodeList daytimes = doc.SelectNodes("/dwml/data/time-layout[1]/start-valid-time/text()");
                    XmlNodeList highs = doc.SelectNodes("/dwml/data/parameters/temperature[@type='maximum']/value/text()");
                    XmlNodeList lows = doc.SelectNodes("/dwml/data/parameters/temperature[@type='minimum']/value/text()");
                    //XmlNodeList cloudIcon = doc.SelectNodes("/dwml/data/parameters/conditions-icon/icon-link/text()");
                    XmlNodeList precipitation = doc.SelectNodes("/dwml/data/parameters/probability-of-precipitation[@type='12 hour']/value/text()");

                    XmlNodeList weather_conditions = doc.SelectNodes("/dwml/data/parameters/weather/weather-conditions[@weather-summary]/@weather-summary");
                    //XmlNodeList weather_conditions = doc.SelectNodes("/dwml/data/parameters/weather/weather-conditions/@weather-summary");
                    //weather-conditions[attribute::weather-summary]
                    int iNumDays = Int32.Parse(numDays);

                    for (int i = 0; i < iNumDays; i++)
                    {
                        string daytime = daytimes[i] != null ? daytimes[i].Value : string.Empty;
                        string HighTempF = highs[i] != null ? highs[i].Value : string.Empty;
                        string LowTempF = lows[i] != null ? lows[i].Value : string.Empty;

                        //string CloudIconURL = cloudIcon[i] != null ? cloudIcon[i].Value : string.Empty;
                        //forecastData[i].DateTime = startDate.AddDays(i);
                        Log2.Trace("{0}: Weather Data: Time: {1} High: {2}, Low: {3}",
                            _myAgentObjectName, daytime, HighTempF, LowTempF);
                    }
                    Log2.Trace("{0}: --------------------------------------",
                        _myAgentObjectName);

                    for (int i = 0; i < iNumDays*2; i++)
                    {
                        string twelvehourtime = twelvehourtimes[i] != null ? twelvehourtimes[i].Value : string.Empty;
                        string rain = precipitation[i] != null ? precipitation[i].Value : string.Empty;
                        string weather = weather_conditions[i] != null ? weather_conditions[i].Value : string.Empty;

                        //string CloudIconURL = cloudIcon[i] != null ? cloudIcon[i].Value : string.Empty;
                        //forecastData[i].DateTime = startDate.AddDays(i);
                        Log2.Trace("{0}: Weather Data: Time: {1}, Rain: {2}, Weather: {3}",
                            _myAgentObjectName, twelvehourtime, rain, weather);
                    }

                    
                    //----------------------------------------------------------------------------------------------------
                    //weatherParametersType wweatherParametersType = new weatherParametersType();
                    //wweatherParametersType

                    weatherParametersType wparams = new weatherParametersType();
                    wparams.maxt = true;
                    wparams.mint = true;
                    wparams.temp = true;
                    wparams.dew = true;
                    wparams.pop12 = true;
                    wparams.qpf = true;
                    wparams.snow = true;
                    wparams.sky = true;
                    wparams.wspd = true;
                    wparams.wdir = true;
                    wparams.wx = true;
                    wparams.icons = false;
                    wparams.waveh = true;
                    //----------------------------
                    wparams.appt = true;
                    wparams.rh = true;
                    wparams.cumw64 = true;
                    wparams.cumw50 = true;
                    wparams.wgust = true;
                    wparams.ptornado = true;
                    wparams.phail = true;
                    wparams.ptstmwinds = true;
                    wparams.pxtornado = true;
                    wparams.pxhail = true;
                    wparams.pxtstmwinds = true;
                    wparams.ptotsvrtstm = true;
                    wparams.pxtotsvrtstm = true;
                    //----------------------------

                
                    string xmlData1 = noaaWeatherProxy.NDFDgen(latitude,
                            longitude,
                            productType.timeseries,
                            DateTime.Now, DateTime.Now,
                            wparams);
                    Log2.Trace("{0}: NDFDgen Weather Data: {1}", _myAgentObjectName, xmlData1);


                    // load up the XML data...
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(xmlData1);
                    doc1.Save("WeatherServiceDailyForecast.xml");

                    // Get Current Temperature
                    XmlNodeList tempNodes = doc1.SelectNodes("dwml/data/parameters/temperature[@type=\"hourly\"]/value/text()");
                    this.ForecastTemperature = tempNodes[0] != null ? tempNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Current Temperature: {1}", _myAgentObjectName, this.ForecastTemperature);

                    // Get Current Max Temperature
                    XmlNodeList maxtempNodes = doc1.SelectNodes("dwml/data/parameters/temperature[@type=\"maximum\"]/value/text()");
                    this.ForecastMaxTemperature = maxtempNodes[0] != null ? maxtempNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Max Temperature: {1}", _myAgentObjectName, this.ForecastMaxTemperature);

                    // Get Dew Point
                    XmlNodeList dewPointNodes = doc1.SelectNodes("dwml/data/parameters/temperature[@type=\"dew point\"]/value/text()");
                    this.ForecastDewPoint = dewPointNodes[0] != null ? dewPointNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Dew Point: {1}", _myAgentObjectName, this.ForecastDewPoint);

                    // Get Current Wind Speed
                    XmlNodeList windSpeedNodes = doc1.SelectNodes("dwml/data/parameters/wind-speed[@type=\"sustained\"]/value/text()");
                    this.ForecastWindSpeed = windSpeedNodes[0] != null ? windSpeedNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Current Wind Speed: {1}", _myAgentObjectName, this.ForecastWindSpeed);

                    // Get Current Wind Direction
                    XmlNodeList windDirectionNodes = doc1.SelectNodes("dwml/data/parameters/direction[@type=\"wind\"]/value/text()");
                    this.ForecastWindDirection = windDirectionNodes[0] != null ? windDirectionNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Current Wind Direction: {1}", _myAgentObjectName, this.ForecastWindDirection);

                    // Get Current Liquid
                    XmlNodeList liquidNodes = doc1.SelectNodes("dwml/data/parameters/precipitation[@type=\"liquid\"]/value/text()");
                    this.ForecastRain = liquidNodes[0] != null ? liquidNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Current Rain: {1}", _myAgentObjectName, this.ForecastRain);

                    //----------------------------------------------------------------------------------------------------
                    // Get Current Snow
                    XmlNodeList snowNodes = doc1.SelectNodes("dwml/data/parameters/precipitation[@type=\"snow\"]/value/text()");
                    this.ForecastSnow = snowNodes[0] != null ? snowNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Current snow: {1}", _myAgentObjectName, this.ForecastSnow);

                    //----------------------------------------------------------------------------------------------------
                    // Get Current Cloud Cover
                    XmlNodeList cloudNodes = doc1.SelectNodes("dwml/data/parameters/cloud-amount[@type=\"total\"]/value/text()");
                    this.ForecastCloudCover = cloudNodes[0] != null ? cloudNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Current clouds: {1}", _myAgentObjectName, this.ForecastCloudCover);

                    //----------------------------------------------------------------------------------------------------
                    //----------------------------------------------------------------------------------------------------
                    // Get 12 Hour Precipitation
                    XmlNodeList twelvehourNodes = doc1.SelectNodes("dwml/data/parameters/probability-of-precipitation[@type=\"12 hour\"]/value/text()");
                    this.ForecastTwelveHourPrecipitation = twelvehourNodes[0] != null ? twelvehourNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen 12 Hour Precipitation: {1}", _myAgentObjectName, this.ForecastTwelveHourPrecipitation);

                    //----------------------------------------------------------------------------------------------------
                    // Get Apparent Temperature
                    XmlNodeList apparentNodes = doc1.SelectNodes("dwml/data/parameters/temperature[@type=\"apparent\"]/value/text()");
                    this.ForecastApparentTemperature = apparentNodes[0] != null ? apparentNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Apparent Temperature: {1}", _myAgentObjectName, this.ForecastApparentTemperature);

                    //----------------------------------------------------------------------------------------------------
                    // Get Tornadoes
                    XmlNodeList tornadoNodes = doc1.SelectNodes("dwml/data/parameters/convective-hazard/severe-component[@type=\"tornadoes\"]/value/text()");
                    this.ForecastTornadoes = tornadoNodes[0] != null ? tornadoNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Tornadoes: {1}", _myAgentObjectName, this.ForecastTornadoes);

                    //----------------------------------------------------------------------------------------------------
                    // Get Hail
                    XmlNodeList hailNodes = doc1.SelectNodes("dwml/data/parameters/convective-hazard/severe-component[@type=\"hail\"]/value/text()");
                    this.ForecastHail = hailNodes[0] != null ? hailNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Hail: {1}", _myAgentObjectName, this.ForecastHail);

                    //----------------------------------------------------------------------------------------------------
                    // Get Damaging Thunderstorm Winds
                    XmlNodeList stormwindNodes = doc1.SelectNodes("dwml/data/parameters/convective-hazard/severe-component[@type=\"damaging thunderstorm winds\"]/value/text()");
                    this.ForecastStormWinds = stormwindNodes[0] != null ? stormwindNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Damaging Thunderstorm Winds: {1}", _myAgentObjectName, this.ForecastStormWinds);

                    //----------------------------------------------------------------------------------------------------
                    // Get Extreme Tornadoes
                    XmlNodeList extremeTornadoNodes = doc1.SelectNodes("dwml/data/parameters/convective-hazard/severe-component[@type=\"extreme tornadoes\"]/value/text()");
                    this.ForecastExtremeTornadoes = extremeTornadoNodes[0] != null ? extremeTornadoNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Extreme Tornadoes: {1}", _myAgentObjectName, this.ForecastExtremeTornadoes);

                    //----------------------------------------------------------------------------------------------------
                    // Get Extreme Hail
                    XmlNodeList extremeHailNodes = doc1.SelectNodes("dwml/data/parameters/convective-hazard/severe-component[@type=\"extreme hail\"]/value/text()");
                    this.ForecastExtremeHail = extremeHailNodes[0] != null ? extremeHailNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Extreme Hail: {1}", _myAgentObjectName, this.ForecastExtremeHail);

                    //----------------------------------------------------------------------------------------------------
                    // Get Extreme Storm Winds
                    XmlNodeList extremeStormWindsNodes = doc1.SelectNodes("dwml/data/parameters/convective-hazard/severe-component[@type=\"extreme thunderstorm winds\"]/value/text()");
                    this.ForecastExtremeStormWinds = extremeStormWindsNodes[0] != null ? extremeStormWindsNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Extreme Storm Winds: {1}", _myAgentObjectName, this.ForecastExtremeStormWinds);

                    //----------------------------------------------------------------------------------------------------
                    // Get Hurricane Speed Above 50 knots
                    XmlNodeList hurricaneAbove50Nodes = doc1.SelectNodes("dwml/data/parameters/wind-speed[@type=\"cumulative50\"]/value/text()");
                    this.ForecastHurricaneAbove50Knots = hurricaneAbove50Nodes[0] != null ? hurricaneAbove50Nodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Hurricane Speed Above 50 knots: {1}", _myAgentObjectName, this.ForecastHurricaneAbove50Knots);
                    //----------------------------------------------------------------------------------------------------
                    // Get Hurricane Speed Above 64 knots
                    XmlNodeList hurricaneAbove64Nodes = doc1.SelectNodes("dwml/data/parameters/wind-speed[@type=\"cumulative64\"]/value/text()");
                    this.ForecastHurricaneAbove64Knots = hurricaneAbove64Nodes[0] != null ? hurricaneAbove64Nodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Hurricane Speed Above 64 knots: {1}", _myAgentObjectName, this.ForecastHurricaneAbove64Knots);
                    //----------------------------------------------------------------------------------------------------
                    // Get Wind Gust
                    XmlNodeList gustNodes = doc1.SelectNodes("dwml/data/parameters/wind-speed[@type=\"gust\"]/value/text()");
                    this.ForecastWindGust = gustNodes[0] != null ? gustNodes[0].Value : string.Empty;
                    Log2.Trace("{0}: NDFDgen Gust: {1}", _myAgentObjectName, this.ForecastWindGust);


                    noaaWeatherProxy.Dispose();
                
               }
                catch (Exception Ex)
                {
                    Log2.Error("{0}: Exception in WeatherService Start: {1}", _myAgentObjectName, Ex.ToString());
                    if (noaaWeatherProxy != null)
                        noaaWeatherProxy.Dispose();

                }
            return true;
        }

        private bool CalcSunriseSunset()
        {

            DateTime utc = DateTime.UtcNow;
            DateTime local = DateTime.Now;

            double latitude = this.Latitude;
            double longitude = this.Longitude;
            //string longitude_ew = " ";
           // string latitude_ns = " ";

            
            double UTCyear = utc.Year;
            double UTCmonth = utc.Month;
            double UTCday = utc.Day;
            double UTChour = utc.Hour;
            double UTCminute = utc.Minute;
            double UTCsecond = utc.Second;
            double localhour = local.Hour;

            // UT in hours
            double UT = UTChour + (UTCminute / 60) + (UTCsecond / 3600);

            // Julian Date for the start of the day
            double JDSOD = NAA.calcJD((int)UTCyear, (int)UTCmonth, (int)UTCday);

            // Julian Date now
            double JD = JDSOD + UT / 24;

            // 1 January 2000 at midday corresponds to JD = 2451545.0
            double J2000 = JD - 2451545.0;

            // T2000
            double T = J2000 / 36525.0;

            double SolarDec = NAA.calcSunDeclination(T);
            double EoT = NAA.calcEquationOfTime(T);

            double ESeconds = EoT * 60;
            double EHours = EoT / 60;
            double EDegrees = EHours * 15.0;

            double UTC_S_minutes = NAA.calcSunriseUTC(JDSOD, latitude, -longitude);
            double TZdiff = UTChour - localhour;
            if (TZdiff < 0) TZdiff += 24;
            if (TZdiff > 24.0) TZdiff -= 24;
            double Local_S_Minutes = UTC_S_minutes - 60 * TZdiff;
            double Local_S_Hour = Math.Floor(Local_S_Minutes / 60.0);
            double Local_S_Min = Math.Floor(Local_S_Minutes - 60 * Local_S_Hour);

            this.TimeOfSunrise = Local_S_Hour + ":" + Local_S_Min + ":00";

            //SetControlProperty(label_SR_HM, "Text",Local_S_Hour.ToString().PadLeft(2, '0') + ':'
            //    + Local_S_Min.ToString().PadLeft(2, '0'));

            UTC_S_minutes = NAA.calcSunsetUTC(JDSOD, latitude, -longitude);
            Local_S_Minutes = UTC_S_minutes - 60 * TZdiff;
            Local_S_Hour = Math.Floor(Local_S_Minutes / 60.0);
            Local_S_Min = Math.Floor(Local_S_Minutes - 60 * Local_S_Hour);

            this.TimeOfSunset = Local_S_Hour + ":" + Local_S_Min + ":00";
            
            //SetControlProperty(label_SS_HM, "Text", Local_S_Hour.ToString().PadLeft(2, '0') + ':'
            //    + Local_S_Min.ToString().PadLeft(2, '0'));

            UTC_S_minutes = NAA.calcSolNoonUTC(T, -longitude);
            Local_S_Minutes = UTC_S_minutes - 60 * TZdiff;
            Local_S_Hour = Math.Floor(Local_S_Minutes / 60.0);
            Local_S_Min = Math.Floor(Local_S_Minutes - 60 * Local_S_Hour);

            //SetControlProperty(label_Noon_HM, "Text", Local_S_Hour.ToString().PadLeft(2, '0') + ':'
            //    + Local_S_Min.ToString().PadLeft(2, '0'));

            //SetControlProperty(label_DoY, "Text", Program.utc.DayOfYear.ToString());
            //SetControlProperty(label_UT_Date, "Text", Program.utc.Date.ToLongDateString());
            //SetControlProperty(label_local_date, "Text", Program.local.ToLongDateString());
            //SetControlProperty(label_UTC_HMS, "Text", UTChour.ToString().PadLeft(2, '0')
            //    + ':' + UTCminute.ToString().PadLeft(2, '0')
            //    + ':' + UTCsecond.ToString().PadLeft(2, '0'));
            //SetControlProperty(label_Local_HMS, "Text", Program.local.Hour.ToString().PadLeft(2, '0')
            //    + ':' + Program.local.Minute.ToString().PadLeft(2, '0')
            //    + ':' + Program.local.Second.ToString().PadLeft(2, '0'));
            //SetControlProperty(label_EoT_Secs, "Text", Math.Round(ESeconds, 4).ToString());
            //SetControlProperty(label_SolarDec, "Text", Math.Round(SolarDec, 2).ToString());

            // GM sidereal time
            double GMST = 280.46061837 + 360.98564736629 * J2000 + 0.000387933 * T * T - T * T * T / 38710000;

            // Local MST
            double LMST = GMST + longitude;

            //string GMSTstring = formatHMS(GMST);
            //SetControlProperty(label_GMST_HMS, "Text", GMSTstring);
            double GAST = GMST + EDegrees;

            //string LMSTstring = formatHMS(LMST);
            //SetControlProperty(label_LMST_HMS, "Text", LMSTstring);
            double LAST = LMST + EDegrees;

            //string GASTstring = formatHMS(GAST);
            //SetControlProperty(label_GAST_HMS, "Text", GASTstring);

            //string LASTstring = formatHMS(LAST);
            //SetControlProperty(label_LAST_HMS, "Text", LASTstring);

            //SetControlProperty(label_JD, "Text", Math.Round(JD, 5).ToString());

            return true;

        }
        // ---------------------------------------------------------------------------------------------------
        // Public Properties

        // Input Configuration Properties
        private string _weatherAlertZone = "MAZ013";
        public string WeatherAlertZone { get { return this._weatherAlertZone; } set { this._weatherAlertZone = value; } }

        private string _weatherStationID = "KTAN";
        public string WeatherStationID { get { return this._weatherStationID; } set { this._weatherStationID = value; } }

        private string _zipCode = "02038";
        public string WeatherZipCode { get { return this._zipCode; } set { this._zipCode = value; } }

        private string _url = null;
        public string Url { get { return this._url; } set { this._url = value; } }

        private double _longitude = 0;
        public double Longitude { get { return this._longitude; } set { this._longitude = value; } }

        private double _latitude = 0;
        public double Latitude { get { return this._latitude; } set { this._latitude = value; } }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Exposed Output Properties
        private string _currentAlertWarning = null;
        public string CurrentAlertWarning { get { return this._currentAlertWarning; } set { this._currentAlertWarning = value; } }


        private string _currentTemperature = null;
        public string CurrentTemperature { get { return this._currentTemperature; } set { this._currentTemperature = value; } }

        private string _currentWeather = null;
        public string CurrentWeather{ get { return this._currentWeather; } set { this._currentWeather = value; } }

        private string _currentHumidity = null;
        public string CurrentHumidity{ get { return this._currentHumidity; } set { this._currentHumidity = value; } }

        private string _currentWindDirection = null;
        public string CurrentWindDirection{ get { return this._currentWindDirection; } set { this._currentWindDirection = value; } }

        private string _currentWindSpeed = null;
        public string CurrentWindSpeed{ get { return this._currentWindSpeed; } set { this._currentWindSpeed = value; } }

        private string _currentWindGust = null;
        public string CurrentWindGust{ get { return this._currentWindGust; } set { this._currentWindGust = value; } }
        
        private string _currentHeatIndex = null;
        public string CurrentHeatIndex{ get { return this._currentHeatIndex; } set { this._currentHeatIndex = value; } }

        private string _currentWindchill = null;
        public string CurrentWindchill{ get { return this._currentWindchill; } set { this._currentWindchill = value; } }

        private string _currentLastUpdated = null;
        public string CurrentLastUpdated{ get { return this._currentLastUpdated; } set { this._currentLastUpdated = value; } }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        private string _forecastTemperature = null;
        public string ForecastTemperature { get { return this._forecastTemperature; } set { this._forecastTemperature = value; } }

        private string _forecastMaxTemperature = null;
        public string ForecastMaxTemperature { get { return this._forecastMaxTemperature; } set { this._forecastMaxTemperature = value; } }

        private string _forecastRain = null;
        public string ForecastRain { get { return this._forecastRain; } set { this._forecastRain = value; } }

        private string _forecastCloudCover = null;
        public string ForecastCloudCover { get { return this._forecastCloudCover; } set { this._forecastCloudCover = value; } }

        private string _forecastSnow = null;
        public string ForecastSnow { get { return this._forecastSnow; } set { this._forecastSnow = value; } }

        private string _forecastDewPoint = null;
        public string ForecastDewPoint { get { return this._forecastDewPoint; } set { this._forecastDewPoint = value; } }

        private string _forecastWindSpeed = null;
        public string ForecastWindSpeed { get { return this._forecastWindSpeed; } set { this._forecastWindSpeed = value; } }

        private string _forecastWindDirection = null;
        public string ForecastWindDirection { get { return this._forecastWindDirection; } set { this._forecastWindDirection = value; } }

        private string _forecastApparentTemperature = null;
        public string ForecastApparentTemperature { get { return this._forecastApparentTemperature; } set { this._forecastApparentTemperature = value; } }

        private string _forecastTwelveHourPrecipitation = null;
        public string ForecastTwelveHourPrecipitation { get { return this._forecastTwelveHourPrecipitation; } set { this._forecastTwelveHourPrecipitation = value; } }

        private string _forecastTornadoes = null;
        public string ForecastTornadoes { get { return this._forecastTornadoes; } set { this._forecastTornadoes = value; } }

        private string _forecastHail = null;
        public string ForecastHail { get { return this._forecastHail; } set { this._forecastHail = value; } }

        private string _forecastStormWinds = null;
        public string ForecastStormWinds { get { return this._forecastStormWinds; } set { this._forecastStormWinds = value; } }

        private string _forecastExtremeTornadoes = null;
        public string ForecastExtremeTornadoes { get { return this._forecastExtremeTornadoes; } set { this._forecastExtremeTornadoes = value; } }

        private string _forecastExtremeHail = null;
        public string ForecastExtremeHail { get { return this._forecastExtremeHail; } set { this._forecastExtremeHail = value; } }

        private string _forecastExtremeStormWinds = null;
        public string ForecastExtremeStormWinds { get { return this._forecastExtremeStormWinds; } set { this._forecastExtremeStormWinds = value; } }

        private string _forecastHurricaneAbove50Knots = null;
        public string ForecastHurricaneAbove50Knots { get { return this._forecastHurricaneAbove50Knots; } set { this._forecastHurricaneAbove50Knots = value; } }

        private string _forecastHurricaneAbove64Knots = null;
        public string ForecastHurricaneAbove64Knots { get { return this._forecastHurricaneAbove64Knots; } set { this._forecastHurricaneAbove64Knots = value; } }

        private string _forecastWindGust = null;
        public string ForecastWindGust { get { return this._forecastWindGust; } set { this._forecastWindGust = value; } }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private string _timeOfSunrise = null;
        public string TimeOfSunrise { get { return this._timeOfSunrise; } set { this._timeOfSunrise = value; } }

        private string _timeOfSunset = null;
        public string TimeOfSunset { get { return this._timeOfSunset; } set { this._timeOfSunset = value; } }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private string _isSunrise = "false";
        public string IsSunrise { get { return this._isSunrise; } set { this._isSunrise = value; } }

        private string _isSunset = "false";
        public string IsSunset { get { return this._isSunset; } set { this._isSunset = value; } }

        private string _isDay = "false";
        public string IsDay { get { return this._isDay; } set { this._isDay = value; } }

        private string _isNight = "false";
        public string IsNight { get { return this._isNight; } set { this._isNight = value; } }

        private string _isMorning = "false";
        public string IsMorning { get { return this._isMorning; } set { this._isMorning = value; } }

        private string _isAfternoon = "false";
        public string IsAfternoon { get { return this._isAfternoon; } set { this._isAfternoon = value; } }

        private string _isEvening = "false";
        public string IsEvening { get { return this._isEvening; } set { this._isEvening = value; } }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        #endregion


        #region Private State Variables

        private AgentPassPort _agentPassPort = null;
        private DateTime _timeLastExecuted = DateTime.MinValue;

        private string _myAgentClassName = null;
        private string _myAgentObjectName = null;
        private object _myAgentObject = null;
        //private TraceSwitch _myTraceSwitch = Upperbay.Core.Logging.Log2.AgentSwitch;

        private bool _activeState = false;
        //private ArrayList _myProperties = null;
        private Type _myType = null;
        #endregion

    }
}
