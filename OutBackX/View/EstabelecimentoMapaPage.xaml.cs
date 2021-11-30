using OutBackX.Model;
using OutBackX.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstabelecimentoMapaPage : ContentPage
    {
        public EstabelecimentoMapaPage()
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoMapaViewModel(LayoutMap);
        }
        public EstabelecimentoMapaPage(EstabelecimentoModel estabelecimento)
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoMapaViewModel(LayoutMap, estabelecimento);
        }
    }
}