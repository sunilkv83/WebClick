using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebClick
{
    public partial class Form1 : Form
    {
        private ChromeOptions _chromeOptions = new ChromeOptions();
        private IWebDriver driver;
        private SelectElement _selectElement;

        private string _websiteUrl = @"C:/Users/kvsun/Downloads/Test.html";
        private int _longRunningTaskTimeout = 10000;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            _chromeOptions.DebuggerAddress = "localhost:9014";

            LaunchChromeDriver();
        }

        /// <summary>
        /// Function to instantiate ChromeDriver.
        /// </summary>
        private void LaunchChromeDriver()
        {
            try
            {
                driver = GetChromeDriver();
            }
            catch (Exception e)
            {
                //Killing orphan chrome driver
                KillChromeDriver();
                //Open the browser and instantiate ChromeDriver
                LaunchChrome();
                driver = GetChromeDriver();
            }
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(15));
        }
        private void LaunchChrome()
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                FileName = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Google\Chrome\Application\chrome.exe ",
                Arguments = _websiteUrl + "  -remote-debugging-port=9014 --user-data-dir=" + @"C:\temp\"
            };
            process.Start();
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
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                var driver = new ChromeDriver(service, chromeOptions);
                return driver;
            }
            catch (Exception)
            {

            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var element = driver.FindElement(By.XPath("/html/body/div[3]/div[3]/div/div/div[2]/div[3]/div[2]/form/div[1]/div[5]/div[2]/select"));
                _selectElement = new SelectElement(element);
                _selectElement.SelectByIndex(1);
            }
            catch (Exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var priceElement = driver.FindElement(By.XPath("html/body/div[3]/div[3]/div/div/div[2]/div[3]/div[2]/form/div[2]/div[1]/div[2]/div[2]/div[2]/span"));

                label1.Text = _selectElement.Options[1].Text;
                label2.Text = priceElement.Text;
            }
            catch (Exception)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var buyElement = driver.FindElement(By.XPath("html/body/div[3]/div[3]/div/div/div[2]/div[3]/div[2]/form/div[2]/div[1]/div[2]/div[3]/a"));
                buyElement.Click();
            }
            catch (Exception)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            KillChromeDriver();
        }

        private void KillChromeDriver()
        {
            Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");

            foreach (var chromeDriverProcess in chromeDriverProcesses)
            {
                chromeDriverProcess.Kill();
            }
        }
    }
}
