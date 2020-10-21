using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TwoCaptcha.Captcha;

namespace Parser
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Parse prs = new Parse();
            prs.startParse();
        }
    }
    public class Parse
    {
        private void InputText(IWebElement Element, string Text)
        {
            Element.Clear();
            foreach (var s in Text)
            {
                Element.SendKeys(s.ToString());
                Task.Delay(50).Wait();
            }
        }

        public async void startParse()
        {
            try
            {
                string INN = "7702681265";
                string BIK = "044525225";

                var chromeOptions = new ChromeOptions();
                // chromeOptions.AddArguments(new List<string>() { "headless" });

                var chromeDriverService = ChromeDriverService.CreateDefaultService();


                while (true)
                {
                    using (var drv = new ChromeDriver(chromeDriverService, chromeOptions))
                    {
                        drv.Navigate().GoToUrl("https://service.nalog.ru/bi.do?t=1602534440423");
                        drv.FindElementById("unirad_0").Click();
                        InputText(drv.FindElementById("innPRS"), INN);
                        InputText(drv.FindElementById("bikPRS"), BIK);
                        drv.FindElementById("btnSearch").Click();

                        if (drv.PageSource.Contains("uniDialogFrame"))
                        {
                            var captchaFrame = drv.FindElementById("uniDialogFrame");
                            var offset = captchaFrame.Location;

                            drv.SwitchTo().Frame(captchaFrame);
                            var captchaInput = drv.FindElementById("captcha");

                            var solver = new TwoCaptcha.TwoCaptcha("9d1a36d87806224b38eb908766e42871");

                            var imgPath = "//div[@class='field-data']//img[1]";
                            var captchaImageTag = drv.FindElementByXPath(imgPath);
                            var imageUrl = captchaImageTag.GetAttribute("src");


                            string CaptchaCode = null;

                            while (string.IsNullOrEmpty(CaptchaCode))
                            {
                                using (var web = new WebClient())
                                {
                                    web.DownloadFile(imageUrl, "captcha.jpg");
                                }

                                //var image = GetCaptchaImage(drv, captchaImageTag, offset);
                                //image.Save("captcha.jpg", ImageFormat.Jpeg);

                                var captcha = new Normal();
                                captcha.SetFile("captcha.jpg");
                                captcha.SetMinLen(4);
                                captcha.SetMaxLen(6);
                                captcha.SetCaseSensitive(false);
                                captcha.SetLang("en");

                                string captchaId = await solver.Send(captcha);
                                Task.Delay(20 * 1000).Wait();
                                CaptchaCode = await solver.GetResult(captchaId);
                            }

                            InputText(captchaInput, CaptchaCode);
                            drv.FindElementById("btnOk").Click();
                            drv.SwitchTo().DefaultContent();
                        }
                        Task.Delay(3000).Wait();
                        var pnlResultData = drv.FindElementById("pnlResultData").Text;
                        //var result = pnlResultData.GetAttribute("innerHTML").ToString();
                        Trace.WriteLine(pnlResultData);
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private Bitmap GetCaptchaImage(IWebDriver driver, IWebElement captchaImage, Point ImageOffset)
        {
            ITakesScreenshot ssdriver = driver as ITakesScreenshot;
            Screenshot screenshot = ssdriver.GetScreenshot();

            Point point = captchaImage.Location;
            point.X += ImageOffset.X;
            point.Y += ImageOffset.Y;
            int width = captchaImage.Size.Width;
            int height = captchaImage.Size.Height;

            Rectangle section = new Rectangle(point, new Size(width, height));
            Bitmap source = new Bitmap(new MemoryStream(screenshot.AsByteArray));
            source.Save("source.jpg", ImageFormat.Jpeg);
            Bitmap finalCaptchImage = CropImage(source, section);
            return finalCaptchImage;
        }
        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            return bmp;
        }


    }
}
