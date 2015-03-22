using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPService.Classes.Licenses
{
    class Licenses
    {
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        internal string GenerateTemporaryLicenseNumber(string emailAddress)
        {
           if(LogEvent != null) LogEvent("Generating Temporary License Number");
            string lNumberType = "t"; // temporary
            string lNumberAuthorisedFromYear = DateTime.UtcNow.Year.ToString();
            string lNumberCountryStateLanguageIDS = "zzz";
            string lNumberCounter = GetLicenseNumberCount();
            string ln = lNumberType + lNumberAuthorisedFromYear + lNumberCountryStateLanguageIDS + lNumberCounter;
            saveLicenseNumber(ln, emailAddress);
            return ln;
        }

        private void saveLicenseNumber(string ln, string emailAddress)
        {
           using(var db = new DamoclesEntities())
           {
               var lnumber = db.LicenseNumbers;
               var newlicense = new LicenseNumber();
               newlicense.emailAddress = emailAddress;
               newlicense.LicenseNumber1 = ln;
               lnumber.Add(newlicense);
               db.SaveChanges();
              if(LogEvent != null) LogEvent("Saved License Number");
           }
        }

        private string GetLicenseNumberCount()
        {
            using(var db = new DamoclesEntities())
            {
                int licenses = db.LicenseNumbers.Count();
                return (licenses + 1).ToString();

            }
        }

    }
}
