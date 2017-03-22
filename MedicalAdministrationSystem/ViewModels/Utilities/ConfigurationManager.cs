using MedicalAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Xml;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public static class ConfigurationManager
    {
        static internal ConfigurationManagerM ConfigurationManagerM { get; set; } = new ConfigurationManagerM();
        private static XmlDocument Config { get; set; } = new XmlDocument();
        private static string Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Egészségügyi Betegnyilvántartó Rendszer\\Configuration.config";
        private static Dictionary<string, string> Nodes = new Dictionary<string, string>()
        {
            { "Server", "ConfigurationProperties/ConnectionString/Server" },
            { "PortNumber", "ConfigurationProperties/ConnectionString/PortNumber" },
            { "UserId", "ConfigurationProperties/ConnectionString/UserId" },
            { "Password", "ConfigurationProperties/ConnectionString/Password" },
            { "Database", "ConfigurationProperties/ConnectionString/Database" },
            { "SecurityUsername", "ConfigurationProperties/Security/Username" },
            { "SecurityPassword", "ConfigurationProperties/Security/Password" },
            { "SecurityPasswordSalt", "ConfigurationProperties/Security/PasswordSalt" },
            { "FacilityId", "ConfigurationProperties/FacilityId" }
        };
        static ConfigurationManager()
        {
            Config.Load(Path);

            foreach (KeyValuePair<string, string> node in Nodes)
                ConfigurationManagerM.GetType().GetProperty(node.Key).SetValue(ConfigurationManagerM, Config.SelectSingleNode(node.Value).InnerText, null);

            ConfigurationManagerM.AcceptChanges();
        }
        static internal string Connect() => "persistsecurityinfo=True;server=" + ConfigurationManagerM.Server + ";port=" + ConfigurationManagerM.PortNumber +
            ";user id=" + ConfigurationManagerM.UserId + ";password=" + ConfigurationManagerM.Password + ";database=" + ConfigurationManagerM.Database;

        static internal void Save()
        {
            if (ConfigurationManagerM.IsChanged)
                foreach (KeyValuePair<string, string> node in Nodes)
                    Config.SelectSingleNode(node.Value).InnerText = ConfigurationManagerM.GetType().GetProperty(node.Key).GetValue(ConfigurationManagerM).ToString();

            Config.Save(Path);
        }
    }
}
