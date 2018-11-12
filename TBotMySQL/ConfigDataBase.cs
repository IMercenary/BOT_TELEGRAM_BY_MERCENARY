
using System;
using System.IO;

namespace TBotMySQL
{
    public abstract class ConfigDataBase
    {
        private string HostName { get; set; }
        private string DataBaseName { get; set; }
        private string UserNameDataBase { get; set; }
        private string PasswordUserDataBase { get; set; }
        private string PrefixDataBase { get; set; }

        public string GetConfigFile()
        {
            return null; /**/
        }

        
    }
}
