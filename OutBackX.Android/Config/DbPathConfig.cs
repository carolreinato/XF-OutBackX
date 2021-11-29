using System;
using OutBackX.Config;
using Xamarin.Forms;

[assembly: Dependency(typeof(OutBackX.Droid.Config.DbPathConfig))]
namespace OutBackX.Droid.Config
{
    public class DbPathConfig : IDbPathConfig
    {

        private String path;

        public String Path
        {
            get
            {
                if (String.IsNullOrEmpty(path))
                {
                    path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                return path;
            }
        }

    }
}
