using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ExpediteTool.Web.Constants
{
    public class ConfigManager
    {
        private const int _DEFAULT_PAGESIZE = 10;
        private const int _DEFAULT_PAGEBUTTONCOUNT = 5;
        private const string _DEFAULT_PASS = "CHANGEME";

        private const int _PORT = 25;
        private const string _HOST = "smtp.atmel.com";
        private const string _FROM = "no-reply-acpcron@atmel.com";
        private const string _USERNAME = "no-reply-acpcron";
        private const string _PASSWORD = "no-reply-acpcron";

        public static int PAGE_SIZE
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                int pageSize = _DEFAULT_PAGESIZE;
                try
                {
                    pageSize = (int)settingsReader.GetValue("PAGESIZE", typeof(int));
                    if (pageSize <= 0)
                        pageSize = _DEFAULT_PAGESIZE;
                }
                catch { pageSize = _DEFAULT_PAGESIZE; }

                return pageSize;
            }
        }

        public static int PAGE_BUTTON_COUNT
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                int pageButtonCount = _DEFAULT_PAGEBUTTONCOUNT;
                try
                {
                    pageButtonCount = (int)settingsReader.GetValue("PAGEBUTTONCOUNT", typeof(int));
                    if (pageButtonCount <= 0)
                        pageButtonCount = _DEFAULT_PAGEBUTTONCOUNT;
                }
                catch { pageButtonCount = _DEFAULT_PAGEBUTTONCOUNT; }

                return pageButtonCount;
            }
        }

        public static string PASS_DEFAULT
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                string passDefault = _DEFAULT_PASS;
                try
                {
                    passDefault = (string)settingsReader.GetValue("PASSDEFAULT", typeof(string));
                }
                catch { passDefault = _DEFAULT_PASS; }

                return passDefault;
            }
        }

        public static int PORT
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                int port = _PORT;
                try
                {
                    port = (int)settingsReader.GetValue("PORT", typeof(int));
                }
                catch { port = _PORT; }

                return port;
            }
        }

        public static string HOST
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                string host = _HOST;
                try
                {
                    host = (string)settingsReader.GetValue("HOST", typeof(string));
                }
                catch { host = _HOST; }

                return host;
            }
        }

        public static string FROM
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                string from = _FROM;
                try
                {
                    from = (string)settingsReader.GetValue("FROM", typeof(string));
                }
                catch { from = _FROM; }

                return from;
            }
        }

        public static string USERNAME
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                string userName = _USERNAME;
                try
                {
                    userName = (string)settingsReader.GetValue("USERNAME", typeof(string));
                }
                catch { userName = _USERNAME; }

                return userName;
            }
        }

        public static string PASSWORD
        {
            get
            {
                AppSettingsReader settingsReader = new AppSettingsReader();
                string pass = _PASSWORD;
                try
                {
                    pass = (string)settingsReader.GetValue("PASSWORD", typeof(string));
                }
                catch { pass = _PASSWORD; }

                return pass;
            }
        }
    }
}