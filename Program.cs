using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Chat_Server
{
    class Program
    {
        static int port = 8005;
        static Socket[] handlers = new Socket[10];
        static int global_i = 0;
        static IPEndPoint ipPoint;
        static Socket listenSocket;

        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            ipPoint = new IPEndPoint(IPAddress.Parse("10.10.10.101"), port);

            // создаем сокет
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                Thread mythread1 = new Thread(Handlers);
                mythread1.Start();
                Thread mythread2 = new Thread(Handlers);
                mythread2.Start();
                Thread mythread3 = new Thread(Handlers);
                mythread3.Start();
                Thread mythread4 = new Thread(Handlers);
                mythread4.Start();
                Thread mythread5 = new Thread(Handlers);
                mythread5.Start();
                Thread mythread6 = new Thread(Handlers);
                mythread6.Start();
                Thread mythread7 = new Thread(Handlers);
                mythread7.Start();
                Thread mythread8 = new Thread(Handlers);
                mythread8.Start();
                Thread mythread9 = new Thread(Handlers);
                mythread9.Start();
                Thread mythread10 = new Thread(Handlers);
                mythread10.Start();

                while (true) { }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Handlers() 
        {
            int i = global_i;
            global_i++;

            handlers[i] = listenSocket.Accept();

            while (true)
            {
                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[256]; // буфер для получаемых данных

                do
                {
                    bytes = handlers[i].Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (handlers[i].Available > 0);

                if (builder.ToString() == "")
                {
                    break;
                }

                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());


                File.AppendAllText(Directory.GetCurrentDirectory() + "/database.txt", DateTime.Now.ToShortTimeString() + ": " + builder.ToString() + "\n");


                // чтение из файла
                using (FileStream fstream = File.OpenRead(Directory.GetCurrentDirectory() + "/database.txt"))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string textFromFile = Encoding.Default.GetString(array);
                    data = Encoding.Unicode.GetBytes(textFromFile);
                    handlers[i].Send(data);
                }
            }
        }
    }
}
