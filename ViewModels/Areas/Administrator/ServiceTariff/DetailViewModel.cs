using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.ServiceTariff
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.ServiceTariff),
           Name = Resources.Model.Strings.ServiceTariffKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region VCode
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.ServiceTariff),
           Name = Resources.Model.Strings.ServiceTariffKeys.VCode)]
        #endregion
        public string VCode { get; set; }

        #region RCode
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.ServiceTariff),
           Name = Resources.Model.Strings.ServiceTariffKeys.RCode)]
        #endregion
        public string RCode { get; set; }

        #region Unit
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Unit)]
        #endregion
        public string Unit { get; set; }


        #region BankAccount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.BankAccount)]
        #endregion
        public string BankAccount { get; set; }


        #region SubHeadLine
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.SubHeadLine)]
        #endregion
        public string SubHeadLine { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Amount)]
        public decimal Amount { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.ServiceTariff),
           Name = Resources.Model.Strings.ServiceTariffKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
