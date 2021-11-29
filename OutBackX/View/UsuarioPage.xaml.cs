using OutBackX.ViewModel;
using System;
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
                new MainMenuItem() { Title = "Favoritos", Icon = "favourite.png", TargetType = typeof(UsuarioEstabelecimentoFavoritoListarPage) },
                new MainMenuItem() { Title = "Próximos a mim", Icon = "location.png", TargetType = typeof(EstabelecimentoMapaPage) }
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
                    Detail = new NavigationPage(new UsuarioEstabelecimentoFavoritoListarPage(99));
                }
                else if (item.Title.Equals("Próximos a mim"))
                {
                    Detail = new NavigationPage(new EstabelecimentoMapaPage());
                }                

                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}