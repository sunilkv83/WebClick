using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClick
{
    /// <summary>
    /// This Class provides chrome driver instantiation
    /// </summary>
   public class ChromeDriverProvider : IChromeDriverProvider
    {
        private ChromeOptions _chromeOptions = new ChromeOptions();
        private int _longRunningTaskTimeout = 15000;
        public ChromeDriverProvider()
        {
            _chromeOptions.DebuggerAddress = "localhost:9014";

            if (ConfigurationManager.AppSettings["taskTimeout"] != null)
            {
                _longRunningTaskTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["taskTimeout"]);
            }
        }
        public ChromeDriver GetChromeDriver()
        {
            var task = Task.Run(() =>
            {
                return getChromeDriver(_chromeOptions);
            });
          
            task.Wait(TimeSpan.FromMilliseconds(_longRunningTaskTimeout));
            if (task.Status == TaskStatus.RanToCompletion)
            {
                return task.Result;
            }
            else if (task.Status == TaskStatus.Running)
            {
                throw new Exception("Chrome Driver still loading, looks like website/page is not open, so please launch new window");
            }

            return task.Result;
        }

        private ChromeDriver getChromeDriver(ChromeOptions chromeOptions)
        {
            try
            {
                var driver = new ChromeDriver(chromeOptions);
                return driver;
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
