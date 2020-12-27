using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClick
{
    public interface IChromeDriverProvider
    {
        ChromeDriver GetChromeDriver();
    }
}
