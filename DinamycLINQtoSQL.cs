using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading.Tasks;
using AddToOKAreport.Models;
using System.Reflection;

namespace AddToOKAreport
{
    public class KFdata
    {
        [Key]
        public int Id { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string contacts { get; set; }
        public string analytics { get; set; }
        public string egrDetails { get; set; }
        public string licences { get; set; }
        public string buh { get; set; }
        public string taxes { get; set; }
        public string fssp { get; set; }
        public string sites { get; set; }
        public string fsa { get; set; }
        public string govPurchOfCust { get; set; }
        public string govPurchOfPartic { get; set; }
        public string beneficialOwners { get; set; }
        public string compAffiliatesReq { get; set; }
        public string compAffiliatesEgrDet { get; set; }
        public string compAffiliatesAnalit { get; set; }
        public string persAffiliatesReq { get; set; }
        public string persAffiliatesEgrDet { get; set; }
        public string persAffiliatesAnalit { get; set; }
        public string req { get; set; }
        public string bankGuarantees { get; set; }
        public string trademarks { get; set; }
        public string bankAccounts { get; set; }
        public string fnsBlockedBankAccounts { get; set; }
        public string petitionersOfArbitration { get; set; }
        public string lessee { get; set; }
        public DateTime ldm { get; set; }
    } //СУЩНОСТЬ ДЛЯ ЗАПОЛНЕНИЯ ДАННЫМИ ИЗ КФ API

    public class DataFromKF
    {
 
        ..................
        public static string GetKForDBdataByMetods(string apiMetod, KFdata kfdt)
        {
            string tmp = "";
            using (DBopendataMysqlContexts db = new DBopendataMysqlContexts())
            {
                    tmp = db.apikfimport.Where(t => (string.IsNullOrWhiteSpace(kfdt.inn) ? t.Id == -1 : t.inn == kfdt.inn) || (string.IsNullOrWhiteSpace(kfdt.ogrn) ? t.Id == -1 : t.ogrn == kfdt.ogrn))
                                        .Select(t => t.GetType().GetProperty(apiMetod).GetValue(t).ToString())
                                        .FirstOrDefault();
            }
            return tmp;
        }

    }
}

