using OutBackX.Model;
using OutBackX.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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