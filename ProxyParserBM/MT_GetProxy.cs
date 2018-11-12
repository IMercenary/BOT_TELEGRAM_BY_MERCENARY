using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
/* 
1:	Russia,
2:	Thailand,
3:	Indonesia,
4:	Brazil,
5:	India,
6:	Ukraine,
7:	China,
8:	Bangladesh,
9:	Colombia,
10:	Poland,
11:	Cambodia,
12:	Iran,
13:	Ecuador,
14:	Czechia,
15:	Nepal,
16:	Argentina,
17:	Spain,
18:	Bulgaria,
19:	Turkey,
20:	Italy,
21:	Canada,
22:	Mexico,
23:	France,
24:	Hungary,
25:	Romania,
26:	Germany,
27:	Albania,
28:	Nigeria,
29:	Iraq,
30:	Pakistan,
31:	Kenya,
32:	Latvia,
33:	Vietnam,
34:	Serbia,
35:	Venezuela,
36:	Greece,
37:	Georgia,
38:	Australia,
39:	Palestine,
40:	Netherlands,
41:	Kazakhstan,
42:	Singapore,
43:	Philippines,
44:	Belarus,
45:	Austria,
46:	Peru,
47:	Armenia,
48:	Panama,
49:	Lebanon,
50:	Guatemala,
51:	Egypt,
52:	Chile,
53:	Taiwan,
54:	Malaysia,
55:	Japan,
56:	Kyrgyzstan,
57:	Bolivia,
58:	Tanzania,
59:	Sweden,
60:	Malawi,
61:	Mongolia,
62:	Honduras,
63:	Portugal,
64:	Afghanistan,
65:	Paraguay,
66:	Macedonia,
67:	Nicaragua,
68:	Slovenia,
69:	Mozambique,
70:	Croatia,
71:	Cameroon,
72:	Unknown,
73:	Botswana,
74:	Mauritius,
75:	Zimbabwe,
76:	Cyprus,
77:	Norway,
78:	Ireland,
79:	Denmark,
80:	Estonia,
81:	Angola,
82:	Belgium,
83:	Switzerland,
84:	Uganda,
85:	Azerbaijan,
86:	Namibia,
87:	Somalia,
88:	Seychelles,
89:	Benin,
90:	Mali,
91:	Burundi,
92:	Rwanda,
93:	Madagascar,
94:	Zambia,
95:	Lesotho,
96:	Montenegro,
97:	Laos,
98:	Libya,
99:	Belize,
100: Yemen,
101: Israel,
102: Uzbekistan,
103: Congo,
104: Ghana,
105: Jamaica,
106: Malta,
107: Uruguay,
108: Guinea,
109: Gabon,
110: Andorra,
111: Cuba,
112: Swaziland,
113: Chad,
114: Dominica,
115: Morocco,
116: Tunisia,
117: Haiti,
118: Kosovo,
119: Tajikistan,
120: Oman,
121: Finland,
122: Guyana,
123: Syria,
124: Kuwait
 */
namespace MT_ProxyParser
{
    public abstract class MT_GetProxy
    {
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
        public static string ParseProxies(string Country)
        {

            string result = "";
            var lines = LoadPage("http://www.gatherproxy.com/proxylist/country/?c="+Country).Split(Environment.NewLine).Select(s => s.Trim()).Where(s => s.StartsWith("gp.insertPrx"));
            foreach (var line in lines)
            {
                var parts = line.Split(',').Select(s => s.Trim());
                var ipPart = parts.FirstOrDefault(s => s.Contains("PROXY_IP"));
                var portPart = parts.FirstOrDefault(s => s.Contains("PROXY_PORT"));             
                var ip = ipPart.Replace("\"PROXY_IP\":", "").Trim('"');
                var port = Convert.ToInt32(portPart.Replace("\"PROXY_PORT\":", "").Trim('"'), 16);
                result = Convert.ToString(ip + ":" + port);
                break;
            }
            return result;
        }

    }
}

