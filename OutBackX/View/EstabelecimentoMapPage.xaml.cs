using OutBackX.Model;
using OutBackX.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstabelecimentoMapPage : ContentPage
    {
        public EstabelecimentoMapPage()
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoMapViewModel(LayoutMap);
        }
        public EstabelecimentoMapPage(EstabelecimentoModel estabelecimento)
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoMapViewModel(LayoutMap, estabelecimento);
        }
    }
}