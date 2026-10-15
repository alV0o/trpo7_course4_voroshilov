using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using trpo7.Models;

namespace trpo7.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        public string Author { get; set; }

        private HttpClient client = new HttpClient();
        public MainPage()
        {
            client.BaseAddress = new Uri("https://localhost:7210/");
            InitializeComponent();
        }

        private async void GetAll_Click(object sender, RoutedEventArgs e)
        {
            var response = await client.GetFromJsonAsync<List<Book>>("/api/v1/Book");
            //https://stackoverflow.com/questions/17096201/build-query-string-for-system-net-httpclient-get
            if (response != null)
            {
                Books.Clear();
                foreach (Book book in response)
                {
                    Books.Add(book);
                }
            }
        }
    }
}
