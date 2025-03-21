namespace ViewModels.Areas.Administrator.User
{
    public class RechargewalletUser
    {
        public RechargewalletUser()
        { }

        #region FullName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.RechargewalletUser),
            Name = Resources.ViewModel.Strings.RechargewalletUserKeys.PhoneNumber)]
        #endregion
        public string PhoneNumber { get; set; }

        #region FullName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.RechargewalletUser),
            Name = Resources.ViewModel.Strings.RechargewalletUserKeys.ChargeAmount)]
        #endregion
        public int ChargeAmount { get; set; }
    }
}