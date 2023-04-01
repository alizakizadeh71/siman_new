namespace Models
{
    [System.Serializable]
    public abstract class BaseExtendedEntity : BaseEntity
    {
        public BaseExtendedEntity()
        {
            IsActived = true;
            IsVerified = true;
            IsDeleted = false;
            IsSystem = false;
            InsertDateTime = System.DateTime.Now;
        }

        public bool IsActived { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSystem { get; set; }
        public System.DateTime InsertDateTime { get; set; }
        public System.DateTime? UpdateDateTime { get; set; }
    }
}
