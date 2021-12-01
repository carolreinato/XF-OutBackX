using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsuarioPage : MasterDetailPage
    {
        public List<MainMenuItem> MainMenuItems { get; set; }
        public int idUsuario = 0;

        public UsuarioPage(int idUsuarioRecebido)
        {
            idUsuario = idUsuarioRecebido;

            BindingContext = this;

            MainMenuItems = new List<MainMenuItem>()
            {
                new MainMenuItem() { Title = "Favoritos", Icon = "favourite.png", TargetType = typeof(UsuarioFavoritListPage) },
                new MainMenuItem() { Title = "Próximos a mim", Icon = "location.png", TargetType = typeof(EstabelecimentoMapPage) }
            };

            Detail = new NavigationPage(new EstabelecimentoListPage("UsuarioPage"));

            InitializeComponent();
        }

        public void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;

            if (item != null)
            {
                if (item.Title.Equals("Favoritos"))
                {
                    Detail = new NavigationPage(new UsuarioFavoritListPage());
                }
                else if (item.Title.Equals("Próximos a mim"))
                {
                    Detail = new NavigationPage(new EstabelecimentoMapPage());
                }

                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}