using System;
using OutBackX.Config;
using Xamarin.Forms;


[assembly: Dependency(typeof(OutBackX.iOS.Config.DbPathConfig))]
namespace OutBackX.iOS.Config
{
    public class DbPathConfig : IDbPathConfig
    {

        private String path;

        public String Path
        {
            get
            {
                if (String.IsNullOrEmpty(this.path))
                {
                    path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    path = System.IO.Path.Combine(path, "..", "Library");
                }
                return path;
            }
        }

    }
}
