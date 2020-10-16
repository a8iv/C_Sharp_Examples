using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;


namespace tstHttpClient
{
    class Program
    {
        // HttpClient рекомендуется создавать один раз за запуск приложения.
        public static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {

            var t1 = Task.Run(() => HttpGet(new Uri(@"https://pb.nalog.ru/calculator.html?t=1602083919561")));
            t1.Wait();
            var t2 = Task.Run(() => HttpPost(new Uri(@"https://pb.nalog.ru/calculator-proc.json?c=search")));
            t2.Wait();

            Console.WriteLine(t1.Result.ToString()); 
            Console.ReadLine();
            Console.WriteLine(t2.Result.ToString());
            Console.ReadLine();
        }
        //Пример метода GET запроса с возвращением результатов
        static async Task<string> HttpGet(Uri u)
        {
            string res = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = u;
                    request.Method = HttpMethod.Get;
                    //request.Headers.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent responseContent = response.Content;
                        res = await responseContent.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception("Ошибка вызова HTTP: " + response.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "|" + e.InnerException);
            }
            return res;
        }
        //Пример метода POST запроса с возвращением результатов
        static async Task<string> HttpPost(Uri u)
        {
            string res = "";
            try
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(@"Mozilla/5.0");

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru-RU"));
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("UTF-8"));
                request.Headers.Host = "pb.nalog.ru";
                request.Headers.Add("Cache-Control", "no-cache");
                request.RequestUri = u;

                #region<ОТПРАВКА ЗАПРОСА С ПАРАМЕТРАМИ В ТИПЕ "application/json">
                //var cnt = new PbNalogContent
                //{
                //    pbCaptchaToken = "",
                //    PERIOD = "2019",
                //    OKVED = "03.1",
                //    REGION = "725",
                //    SCL = "3",
                //    TYPE = "0",
                //    CALC_4 = "",
                //    CALC_5 = "",
                //    CALC_6 = "",
                //    CALC_7 = "",
                //    CALC_8 = ""
                //};
                //string json = JsonSerializer.Serialize<PbNalogContent>(cnt);
                //var data = new StringContent(json, Encoding.UTF8, "application/json");
                #endregion

                #region<ОТПРАВКА ЗАПРОСА С ПАРАМЕТРАМИ В ТИПЕ "application/x-www-form-urlencoded">
                var pairs = new List<KeyValuePair<string, string>>
    {
                        new KeyValuePair<string, string>("vpbCaptchaToken", ""),
                        new KeyValuePair<string, string>("PERIOD", "2019"),
                        new KeyValuePair<string, string>("OKVED", "03.1"),
                        new KeyValuePair<string, string>("REGION", "725"),
                        new KeyValuePair<string, string>("SCL", "3"),
                        new KeyValuePair<string, string>("TYPE", "0"),
                        new KeyValuePair<string, string>("CALC_4", ""),
                        new KeyValuePair<string, string>("CALC_5", ""),
                        new KeyValuePair<string, string>("CALC_6", ""),
                        new KeyValuePair<string, string>("CALC_7", ""),
                        new KeyValuePair<string, string>("CALC_8", "")
                        };
                var data = new FormUrlEncodedContent(pairs);
                request.Content = data;

                //ВТОРОЙ ВАРИАНТ: МОЖНО ВЗЯТЬ СТРОКУ ПАРАМЕТРОВ И ОТПРАВИТЬ ЕЕ, ПРЕДВАРИТЕЛЬНО ПРЕОБРАЗОВАВ К ТИПУ application/x-www-form-urlencoded
                //var prmstr = "pbCaptchaToken=&PERIOD=2019&OKVED=03.1&REGION=725&SCL=3&TYPE=0&CALC_4=&CALC_5=&CALC_6=&CALC_7=&CALC_8=";
                //var data = new StringContent(prmstr, Encoding.UTF8, "application/x-www-form-urlencoded");
                #endregion

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    res = await responseContent.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception("Ошибка вызова HTTP: " + response.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "|" + e.InnerException);
            }
            return res;
        }
    }
    class PbNalogContent
    {
        public string pbCaptchaToken { get; set; }
        public string PERIOD { get; set; }
        public string OKVED { get; set; }
        public string REGION { get; set; }
        public string SCL { get; set; }
        public string TYPE { get; set; }
        public string CALC_4 { get; set; }
        public string CALC_5 { get; set; }
        public string CALC_6 { get; set; }
        public string CALC_7 { get; set; }
        public string CALC_8 { get; set; }
    }

}

