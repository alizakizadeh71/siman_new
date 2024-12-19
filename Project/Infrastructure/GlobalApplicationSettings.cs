namespace Infrastructure
{
    public static class GlobalApplicationSettings
    {
        static GlobalApplicationSettings()
        {
        }

        public static bool Instance()
        {
            // IsSslEnabled
            return true;
        }
    }
}
