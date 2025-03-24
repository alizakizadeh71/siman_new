namespace ViewModels.Areas.Administrator.User
{
    public class RechargewalletUser
    {
        public RechargewalletUser()
        { }

        #region PhoneNumber
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.RechargewalletUser),
            Name = Resources.ViewModel.Strings.RechargewalletUserKeys.PhoneNumber)]
        #endregion
        public string PhoneNumber { get; set; }

        #region ChargeAmount
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.RechargewalletUser),
            Name = Resources.ViewModel.Strings.RechargewalletUserKeys.ChargeAmount)]
        #endregion
        public int ChargeAmount { get; set; }
        #region Description
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.RechargewalletUser),
            Name = Resources.ViewModel.Strings.RechargewalletUserKeys.Description)]
        #endregion
        public string Description { get; set; }
    }
}