using CupkekGames.TimeSystem;

namespace CupkekGames.Audio.Sonity
{
    public static class TimeBundleSonityInstaller
    {
        public static void Install(TimeBundle timeBundle)
        {
            var sonityScaler = new TimeScaleSonity(timeBundle.TimeContext);
            timeBundle.AddScaler(sonityScaler);
        }
    }
}
