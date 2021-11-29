using OutBackX.ViewModel;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FuncionarioCadastrarPage : ContentPage
    {
        public FuncionarioCadastrarPage()
        {
            InitializeComponent();

            BindingContext = new FuncionarioViewModel();
        }       
    }
}
