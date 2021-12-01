using OutBackX.Model;
using OutBackX.ViewModel;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstabelecimentoInsertPage : ContentPage
    {
        public EstabelecimentoInsertPage()
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoCrudViewModel();
        }
    }
}
