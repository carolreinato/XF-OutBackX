using OutBackX.Model;
using OutBackX.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstabelecimentoListPage : ContentPage
    {
        public EstabelecimentoListPage(string paginaOrigem)
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoListViewModel(paginaOrigem);
        }       

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
    }
}
