using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTaxCalculationSAPDIAPI
{
    internal static class CompanyHelper
    {
        public static Company GetCompany()
        {
            Company company = new Company();
            company.Server = "NDB@hanab1:30013";
            company.CompanyDB = "SBODEMOBR";
            company.DbUserName = "SYSTEM";
            company.DbPassword = "youpassword";
            company.UserName = "manager";
            company.Password = "youpassword";
            company.language = BoSuppLangs.ln_English;
            company.UseTrusted = false;
            company.DbServerType = BoDataServerTypes.dst_HANADB;
            return company;
        }
    }
}
