namespace ViewModels.Account
{
    public class Rechargewallet : System.Object
    {
        public Rechargewallet()
        {
        }
        #region Chargeamount
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Action),
             Name = Resources.Strings.ActionKeys.Chargeamount)]
        #endregion
        public long Chargeamount { get; set; }
        public string UserName { get; set; }

    }
}
