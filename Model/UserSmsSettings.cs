namespace Models
{
    class UserSmsSettings : BaseEntity
    {
        public int UserId { get; set; }
        public bool IsBalanceSmsEnabled { get; set; }

        public virtual User User { get; set; }
    }
}