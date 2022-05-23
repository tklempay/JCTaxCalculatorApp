using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using JCTaxCalculatorApp.ViewModels;
using JC.Common.Models;

namespace JCTaxCalculatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        public OrderPage()
        {
            InitializeComponent();
            BindingContext = App.Current.ServiceProvider.GetService<OrderViewModel>(); 

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}