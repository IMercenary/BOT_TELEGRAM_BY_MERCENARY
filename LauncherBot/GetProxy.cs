using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LauncherBot
{
    
    abstract class GetProxy
    {
        List<string> ProxyListByCountry = new List<string>()
        {
			"Russia", "Thailand", "Indonesia", "Brazil", "India", "Ukraine", "United States", "China","Bangladesh", "Colombia", "Poland", "Cambodia", "Iran", "South Africa", "Ecuador", "Czechia",
			"Nepal", "Argentina", "Spain", "Bulgaria", "Turkey", "Italy", "Canada", "Mexico", "France","Hungary", "Romania", "Germany", "Albania", "Nigeria", "Iraq", "Czech Republic", "Pakistan",
			"Kenya", "Latvia", "Vietnam", "Serbia", "Venezuela", "United Kingdom", "Greece", "Georgia","Australia", "Slovak Republic", "Palestine", "Netherlands", "Kazakhstan", "Republic of Moldova",
			"Singapore", "Philippines", "Belarus", "Austria", "Peru", "Hong Kong", "Armenia", "Panama","Lebanon", "Guatemala", "Egypt", "Republic of Korea", "Chile", "Taiwan", "Malaysia", "Republic of Lithuania","Japan", "Kyrgyzstan", "Bosnia and Herzegovina", "Bolivia", "Tanzania", "Sweden", "Malawi","Mongolia", "Puerto Rico", "Honduras", "Costa Rica", "New Zealand", "Portugal", "Afghanistan","Paraguay", "Macedonia", "Nicaragua", "Slovenia", "Mozambique", "Croatia", "Cameroon", "Unknown","Botswana", "Mauritius", "Zimbabwe", "Cyprus", "Norway", "Ireland","Denmark","Estonia","Angola","Belgium","Switzerland","Dominican Republic","Uganda","Azerbaijan","Namibia","Somalia","Seychelles",			"Benin","Mali","Burundi","Rwanda","Madagascar","Zambia","Lesotho","Montenegro","Laos","Libya","United Arab Emirates","Belize","Yemen","Israel","Uzbekistan","Congo","Sri Lanka","Ghana","Jamaica","Malta","Saudi Arabia","Republic of the Congo","Burkina Faso","South Sudan","El Salvador","Myanmar [Burma]","Uruguay",
			"Guinea","Gabon","Andorra","East Timor","Saint Martin","Trinidad and Tobago","Solomon Islands","Cuba","Swaziland","Ivory Coast","Chad","Sierra Leone","Dominica","Morocco"," U.S. Virgin Islands","Tunisia","Isle of Man","Haiti","Kosovo","Tajikistan","Sudan","Oman","New Caledonia","Papua New Guinea","Finland","Guyana","Syria","Kuwait"
        };
        static string LoadPage(string url)
        {
            var result = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();
                if (receiveStream != null)
                {
                    StreamReader readStream;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    result = readStream.ReadToEnd();
                    readStream.Close();
                }
                response.Close();
            }
            return result;
        }
        public static string ParseProxies(string Countrys)
        {

            string result = "";
            var lines = LoadPage("http://www.gatherproxy.com/").Split(Environment.NewLine).Select(s => s.Trim()).Where(s => s.StartsWith("gp.insertPrx"));
            StepFindCounty:
            foreach (var line in lines)
            {
                var parts = line.Split(',').Select(s => s.Trim());
                var CountryPart = parts.FirstOrDefault(s => s.Contains("PROXY_COUNTRY"));
                var Country = CountryPart.Replace("\"PROXY_COUNTRY\":", "").Trim('"');
                if (Country == Countrys)
                {
                    
                    var ipPart = parts.FirstOrDefault(s => s.Contains("PROXY_IP"));
                    var portPart = parts.FirstOrDefault(s => s.Contains("PROXY_PORT"));
                    var ip = ipPart.Replace("\"PROXY_IP\":", "").Trim('"');
                    var port = Convert.ToInt32(portPart.Replace("\"PROXY_PORT\":", "").Trim('"'), 16);
                    result = Convert.ToString(ip + ":" + port);
                }
                else
                {
                    goto StepFindCounty;
                }
            }
            return result;
        }

    }
}
