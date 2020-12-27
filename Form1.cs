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
        private IWebDriver driver;
        private SelectElement _selectElement;
        private IChromeDriverProvider _chromeDriverProvider = null;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            LaunchChromeDriver();
        }

        /// <summary>
        /// Function to instantiate ChromeDriver.
        /// </summary>
        private void LaunchChromeDriver()
        {
            _chromeDriverProvider = new ChromeDriverProvider();
            try
            {
                driver = _chromeDriverProvider.GetChromeDriver();
            }
            catch (Exception e)
            {
                //Killing orphan chrome driver
                KillChromeDriver();
                //Open the browser and instantiate ChromeDriver
                LaunchChrome();
                driver = _chromeDriverProvider.GetChromeDriver();
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
                Arguments = ConfigurationManager.AppSettings["url"] + "  -remote-debugging-port=9014 --user-data-dir=" + @"C:\temp\"
            };
            process.Start();
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
