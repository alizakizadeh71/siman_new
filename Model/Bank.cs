﻿namespace Models
{
    /// <summary>
    /// بانک
    /// </summary>
    public class Bank : BaseExtendedEntity
    {

        public Bank()
        { }

        public string Name { get; set; }

        public long Balance { get; set; }
        public virtual System.Collections.Generic.IList<BankAccount> BankAccounts { get; set; }
    }
}
