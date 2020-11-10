using AddToOKAreport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AddToOKAreport
{
    public class DataFrom1C
    {
        string connectionStr = @"Data Source=1C;Initial Catalog=*****************;User ID=**********;Password=********";
        DataTable dt;
        string parametr;
        public string inn = "", ogrn = "";
        public List<DataForReport> ldfr = new List<DataForReport>();

        public DataFrom1C(string _param) //При инициализации получаем таблицу с данными из 1С
        {
            this.parametr = _param;
            GetDataTableFrom1C();
        }
        public void GetDataTableFrom1C()
        {
            string strQuery = $"exec DataToFillWordReportSP @param";
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionStr))
                {
                    SqlParameter param = new SqlParameter("@param", SqlDbType.NChar, 9, "param");
                    param.Value = this.parametr;
                    SqlCommand sqlcomnd = new SqlCommand(strQuery, connection);
                    sqlcomnd.Parameters.Add(param);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcomnd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    this.dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception($"По заявке [{this.parametr}] в базе 1С ГФ ничего НЕ найдено");
                    }
                    this.inn = (from r in ds.Tables[0].AsEnumerable() select r.Field<string>("ИНН")).FirstOrDefault();
                    this.ogrn = (from r in ds.Tables[0].AsEnumerable() select r.Field<string>("ОГРН")).FirstOrDefault();
                }

                DateTime dtValue;
                decimal numValue;

                DataRow rw = dt.Rows[0];
                List<Task<DataForReport>> lt = new List<Task<DataForReport>>();
                foreach ( DataColumn c in dt.Columns)
                {
                    lt.Add(Task.Run(() => DataForReportCreateMetods.DataForReportCreateAsync( c.ColumnName, rw.ItemArray[c.Ordinal])));
                }
                Task.WaitAll(lt.ToArray());
                foreach(var r in lt)
                {
                    ldfr.Add(r.Result);
                }
            }
            catch (Exception e)
            {
                Program.InfMsg($"{e.Message} \r\n {e.InnerException} \r\n {e.StackTrace}");
                throw;
            }

        }
    
    public class DataForReport
    {
        public string key { get; set; }
        public string vType { get; set; }
        public object Value { get; set; }        
        public List<object> listObjValue { get; set; }
    }
    
    public class DataForReportCreateMetods
    {
        public static DataForReport DataForReportCreate(string key, object value)
        {
            DataForReport dfr = new DataForReport();
            dfr.key = key;
            if (value.GetType() == typeof(DateTime))
            {
                var v = (DateTime)value;
                dfr.vType = "DateTime";
                dfr.Value = v.ToString("dd.MM.yyyy");
            }
            else if (value.GetType() == typeof(Decimal))
            {
                var v = (decimal)value;
                dfr.vType = "Decimal";
                dfr.Value = v.ToString("#,###.##");
            }
            else if (value.GetType() == typeof(Boolean))
            {
                var v = (bool)value;
                dfr.vType = "Boolean";
                dfr.Value = v.ToString();
            }
            else
            {
                dfr.vType = "String";
                dfr.Value = value.ToString();
            }
            return dfr;
        }
        public static async Task<DataForReport> DataForReportCreateAsync(string key, object value)
        {
            return DataForReportCreate(key,value);
        }
    }
}

