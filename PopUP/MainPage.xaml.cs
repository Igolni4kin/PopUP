using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PopUP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        List<ContentPage> pages = new List<ContentPage>() { new PopUp_Page() };
        List<string> tekstid = new List<string> { "Ava PopUP leht" };

        public MainPage()
        {
            StackLayout st = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Aqua
            };
            for (int i = 0; i < pages.Count; i++)
            {
                Button button = new Button
                {
                    Text = tekstid[i],
                    TabIndex = i,
                    BackgroundColor = Color.Fuchsia,
                    TextColor = Color.Black
                };
                st.Children.Add(button);
                button.Clicked += Navig_funktsion;
            }
            Content = st;
        }

        private async void Navig_funktsion(object sender, EventArgs e)
        {
            Button btn = sender as Button; //(Button)sender
            await Navigation.PushAsync(pages[btn.TabIndex]);
        }
    }
}