using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MobileCLientChat
{
    public partial class MainPage : ContentPage
    {
        static int port = 8005; // порт сервера
        static string address = "10.10.10.101"; // адрес сервера
        static IPEndPoint ipPointServer = new IPEndPoint(IPAddress.Parse(address), port);
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public MainPage()
        {
            InitializeComponent();

            // подключаемся к удаленному хосту
            socket.Connect(ipPointServer);
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            Entry entry = (Entry)FindByName("EntryMes");
            string message = entry.Text;

            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
            entry.Text = "";
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            // получаем ответ
            byte[] data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);

            Log.Text = builder.ToString();
        }
    }
}

