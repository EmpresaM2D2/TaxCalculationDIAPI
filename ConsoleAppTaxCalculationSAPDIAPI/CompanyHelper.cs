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
            company.Server = "youserver";
            company.CompanyDB = "SBODemoBR";
            company.DbUserName = "sa";
            company.DbPassword = "youpassword";
            company.UserName = "manager";
            company.Password = "yourpassword";
            company.language = BoSuppLangs.ln_English;
            company.UseTrusted = false;
            company.DbServerType = BoDataServerTypes.dst_MSSQL2019;
            return company;
        }
    }
}
