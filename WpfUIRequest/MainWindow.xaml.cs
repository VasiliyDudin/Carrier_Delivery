using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfUIRequest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _httpClient;
        private const string _url = "https://localhost:7273";
        private const string _errorNotFound = "В ответе от сервера нет значений !";
        private const string _json = "application/json";

        Dictionary<int, string> _couriers = new Dictionary<int, string>();

        public MainWindow()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_url)
            };

            InitializeComponent();
        }

        // Получаем все записи из БД
        private async Task<string> GetAllValuesAsync()
        { 
            string result = string.Empty;

            try
            {
                var response = await _httpClient.GetAsync("/api/Request/GetAllRequest");

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var values = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(values))
                    {
                        List<Request> requests = JsonSerializer.Deserialize<List<Request>>(values);

                        MainTable.ItemsSource = requests;
                    }
                    else
                    {
                        result = _errorNotFound;
                    }
                }
                else
                {
                    result = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        // Загружаем все заявки из БД
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OutTxt.Text = await GetAllValuesAsync();
        }

        // Ищем все заявки по введеному значению в поиске
        private async void Button_Click_Find(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/Request/GetFindRequest?findStr=" + FindTxt.Text.Trim());

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var values = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(values))
                    {
                        List<Request> requests = JsonSerializer.Deserialize<List<Request>>(values);

                        MainTable.ItemsSource = requests;
                    }
                    else
                    {
                        OutTxt.Text = _errorNotFound;
                    }
                }
                else
                {
                    OutTxt.Text = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                OutTxt.Text = ex.Message;
            }
        }

        // Удаляем заявку из БД
        private async void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (MainTable.SelectedItem == null || MainTable.SelectedItems.Count > 1)
            {
                OutTxt.Text = "Выбирите в таблице одну заявку для удаления !";
                return;
            }

            try
            {
                Request slcItm = (Request)MainTable.SelectedItem;

                var response = await _httpClient.DeleteAsync("/api/Request/Delete?id=" + slcItm.id);

                if (response != null || response.StatusCode == HttpStatusCode.OK)
                {
                    OutTxt.Text = await GetAllValuesAsync();
                }
                else
                {
                    OutTxt.Text = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                OutTxt.Text = ex.Message;
            }
        }

        // Создаем новую заявку
        private async void Button_Click_Create(object sender, RoutedEventArgs e)
        {
            try
            {
                RequestDto requestDto = new RequestDto()
                {
                    ID = MainTable.Items.Count + 1,
                    StatusID = 1,
                    StatusName = string.Empty,
                    ClientFIO = FIOTxt.Text.Trim(),
                    CourierID = _couriers.FirstOrDefault(x => x.Value == CurierCmb.SelectedItem.ToString()).Key,
                    FIO = CurierCmb.SelectedItem.ToString(),
                    Rating = 5,
                    Address = AddressTxt.Text,
                    Text = RequestTxt.Text,
                    CanceledText = string.Empty
                };

                string json = JsonSerializer.Serialize(requestDto);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, _json);
                var response = await _httpClient.PostAsync("/api/Request/Create", httpContent);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    FIOTxt.Text = string.Empty;
                    CurierCmb.Text = string.Empty;
                    AddressTxt.Text = string.Empty;
                    RequestTxt.Text = string.Empty;

                    MessageBox.Show("Новая заявка успешно создана !", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    OutTxt.Text = response.StatusCode.ToString() + "\n";
                    OutTxt.Text += await GetAllValuesAsync();
                }
                else
                {
                    OutTxt.Text = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                OutTxt.Text = ex.Message;
            }
        }

        // Изменяем выбранную заявку
        private async void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (MainTable.SelectedItem == null || MainTable.SelectedItems.Count > 1)
            {
                OutTxt.Text = "Выберите в таблице одну заявку для изменения !";
                return;
            }

            try
            {
                Request request = (Request)MainTable.SelectedItem;

                RequestDto requestDto = new RequestDto()
                {
                    ID = request.id,
                    StatusID = request.statusID,
                    StatusName = request.statusName,
                    ClientFIO = request.clientFIO,
                    CourierID = request.courierID,
                    FIO = request.fio,
                    Rating = request.rating,
                    Address = request.address,
                    Text = request.text,
                    CanceledText = request.canceledText
                };

                string json = JsonSerializer.Serialize(requestDto);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, _json);
                var response = await _httpClient.PutAsync("/api/Request/EditAll", httpContent);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    OutTxt.Text = response.StatusCode.ToString() + "\n";
                    OutTxt.Text += await GetAllValuesAsync();
                }
                else
                {
                    OutTxt.Text = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                OutTxt.Text = ex.Message;
            }
        }

        // Отображать или нет логи от сервера
        private void Log_SelectionChanged(object sender, RoutedEventArgs e)
        {
            OutTxt.Visibility = (bool)IsLog.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        // При переходе на вкладку - Создание заявки, получаем данные от сервера для подставления в выпадающие списки
        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
                return;

            TabItem selectedTab = e.AddedItems[0] as TabItem;

            if (selectedTab.Header.ToString().Equals("Создание заявки"))
            {
                try
                {
                    var response = await _httpClient.GetAsync("/api/Request/GetCouriers");
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        var values = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(values))
                        {
                            List<Courier> requests = JsonSerializer.Deserialize<List<Courier>>(values);
                            foreach (Courier cr in requests)
                                _couriers.Add(cr.id, cr.fio);

                            CurierCmb.ItemsSource = _couriers.Values;
                        }
                        else
                        {
                            OutTxt.Text = _errorNotFound;
                        }
                    }
                    else
                    {
                        OutTxt.Text = response.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    OutTxt.Text = ex.Message;
                }
            }
        }
    }
}
