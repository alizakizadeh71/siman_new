namespace Models
{
    [System.Serializable]
    public abstract class BaseEntity : System.Object
    {
        #region Create Guid
        [System.Runtime.InteropServices.DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out System.Guid value);

        [System.Flags]
        private enum RetUuidCodes : int
        {
            RPC_S_OK = 0, //The call succeeded.
            RPC_S_UUID_LOCAL_ONLY = 1824, //The UUID is guaranteed to be unique to this computer only.
            RPC_S_UUID_NO_ADDRESS = 1739 //Cannot get Ethernet or token-ring hardware address for this computer.
        }
        public System.Guid CreateGuid()
        {
            System.Guid guid;
            int result = UuidCreateSequential(out guid);
            if (result == (int)RetUuidCodes.RPC_S_OK)
                return guid;
            else
                return System.Guid.NewGuid();
        }
        #endregion

        public BaseEntity()
        {
            //Id = System.Guid.NewGuid();
            Id = CreateGuid();
        }

        public System.Guid Id { get; set; }
    }
}
