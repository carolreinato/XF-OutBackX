using OutBackX.Model;
using OutBackX.ViewModel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstabelecimentoPage : MasterDetailPage
    {
        public FuncionarioModel funcionarioModel = new FuncionarioModel();
        public List<MainMenuItem> MainMenuItems { get; set; } 

        public EstabelecimentoPage()
        {

            BindingContext = this;

            MainMenuItems = new List<MainMenuItem>()
            {
                new MainMenuItem() { Title = "Cadastrar Estabelecimento", Icon = "plus.png", TargetType = typeof(EstabelecimentoInsertPage) },
                new MainMenuItem() { Title = "Atualizar Estabelecimento", Icon = "editar.png", TargetType = typeof(EstabelecimentoUpdatePage) },
                new MainMenuItem() { Title = "Excluir Estabelecimento", Icon = "trash.png", TargetType = typeof(EstabelecimentoDeletePage) },
                new MainMenuItem() { Title = "Mapa", Icon = "map.png", TargetType = typeof(EstabelecimentoMapPage) }
            };

            Detail = new NavigationPage(new EstabelecimentoListPage("paginaPrincipal"));

            InitializeComponent();            
        }

        public void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;

            if (item != null)
            {
                if (item.Title.Equals("Cadastrar Estabelecimento"))
                {
                    Detail = new NavigationPage(new EstabelecimentoInsertPage());
                }
                else if (item.Title.Equals("Atualizar Estabelecimento"))
                {
                    Detail = new NavigationPage(new EstabelecimentoUpdatePage());
                }
                else if (item.Title.Equals("Excluir Estabelecimento"))
                {
                    Detail = new NavigationPage(new EstabelecimentoDeletePage());
                }
                else if (item.Title.Equals("Mapa"))
                {
                    Detail = new NavigationPage(new EstabelecimentoMapPage());
                }
                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}