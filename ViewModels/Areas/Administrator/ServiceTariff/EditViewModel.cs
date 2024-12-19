using System;

namespace ViewModels.Areas.Administrator.ServiceTariff
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.ServiceTariff),
           Name = Resources.Model.Strings.ServiceTariffKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(400)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region VCode
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.ServiceTariff),
           Name = Resources.Model.Strings.ServiceTariffKeys.VCode)]
        [System.ComponentModel.DataAnnotations.MaxLength(6)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string VCode { get; set; }

        #region RCode
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.ServiceTariff),
           Name = Resources.Model.Strings.ServiceTariffKeys.RCode)]
        [System.ComponentModel.DataAnnotations.MaxLength(3)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string RCode { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Amount)]
        public decimal Amount { get; set; }

        #region Unit
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Unit)]
        #endregion
        public Guid Unit { get; set; }


        #region BankAccount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.BankAccount)]
        #endregion
        public Guid BankAccount { get; set; }


        #region SubHeadLine
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.SubHeadLine)]
        #endregion
        public Guid SubHeadLine { get; set; }
    }
}
