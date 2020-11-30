﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebClick
{
    public partial class Form1 : Form
    {
        private IWebDriver driver;
        private SelectElement _selectElement;
        public Form1()
        {
            InitializeComponent();
            driver  = new ChromeDriver();
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["url"]);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(15));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var element = driver.FindElement(By.XPath("/html/body/div[3]/div[3]/div/div/div[2]/div[3]/div[2]/form/div[1]/div[5]/div[2]/select"));
            _selectElement = new SelectElement(element);
            _selectElement.SelectByIndex(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           var priceElement = driver.FindElement(By.XPath("html/body/div[3]/div[3]/div/div/div[2]/div[3]/div[2]/form/div[2]/div[1]/div[2]/div[2]/div[2]/span"));

           label1.Text = _selectElement.Options[1].Text;
           label2.Text =  priceElement.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var buyElement = driver.FindElement(By.XPath("html/body/div[3]/div[3]/div/div/div[2]/div[3]/div[2]/form/div[2]/div[1]/div[2]/div[3]/a"));
            buyElement.Click();
        }
    }
}