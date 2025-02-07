using System.Net.Http;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using ZXing.QrCode.Internal;

namespace QLTB_Huong
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            CameraBarcodeReaderView cameraBarcodeReaderView = new CameraBarcodeReaderView();
            cameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.OneDimensional,
                AutoRotate = true,
                Multiple = true,
            };
        }
        
        private async void BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {   
            var rs = e.Results[0];
            if (rs != null)
            {
                var checkRs = await SendDataToGoogleSheet(rs.Value);
                if (checkRs == 1)
                {
                    //txtKetqua.Text = "Success: " + rs.Value;
                }
                else
                {
                    //txtKetqua.Text = "Fail: " + checkRs;
                }
            }
        }

        // gọi lên google sheet
        public static async Task<int> SendDataToGoogleSheet(string data)
        {
            using (HttpClient client = new HttpClient())
            {
                string ID = Guid.NewGuid().ToString();
                string DeviceName = data;
                string Datetime_Scan = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                var url = $"https://script.google.com/macros/s/AKfycbxYzLiHV2yGddiM3AuNJY0W3SrpCQOtJYXRqudykJnNAodHP8R0H-W3k6rkZR7F5hBq/exec?ID={ID}&Device_Name={DeviceName}&Datetime_Scan={Datetime_Scan}"; // URL API

                try
                {
                    // Gửi yêu cầu GET
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Kiểm tra trạng thái của phản hồi
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc nội dung phản hồi từ API
                        string content = await response.Content.ReadAsStringAsync();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
    }

}
