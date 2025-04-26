using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

/// <summary>
/// Главная форма приложения.
/// </summary>
public partial class MainForm : Form {
    private readonly Uri _loginUri = new("http://localhost:5141/api/auth/login"); // URL авторизации
    private readonly Uri _wsUri = new("ws://localhost:5141/ws"); // URL WebSocket

    private string? _token = null;

    private readonly ClientWebSocket[] _sockets = new ClientWebSocket[3];
    private readonly CancellationTokenSource[] _cts = new CancellationTokenSource[3];

    /// <summary>
    /// Конструктор формы.
    /// </summary>
    public MainForm() {
        InitializeComponent();
        Text = "WebSocket Client";

        // Инициализация элементов формы
        Button button = new() { Text = "Отправить", Width = 100, Top = 10, Left = 10 };
        TextBox box = new() { Name = "TextInput", Top = 10, Left = 120, Width = 300 };
        Controls.Add(button);
        Controls.Add(box);

        button.Click += async (s, e) => {
            string text = box.Text;
            await SendToAllSocketsAsync(text);
        };

        // Запуск инициализации после загрузки
        Load += async (s, e) => {
            await AuthorizeAsync();
            await ConnectAllSocketsAsync();
        };
    }

    /// <summary>
    /// Выполняет авторизацию по логину и паролю.
    /// </summary>
    private async Task AuthorizeAsync() {
        using HttpClient client = new();
        var payload = new { Username = "testUser", Password = "testPassword" };
        string json = JsonConvert.SerializeObject(payload);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        client.Timeout = TimeSpan.FromSeconds(60);
        for (int i = 1; i <= 10; i++) {
            _ = log.AppendLine($"{i} попытка авторизоваться");
            try {
                HttpResponseMessage response = await client.PostAsync(_loginUri, content);
                i = 99999;
                if (!response.IsSuccessStatusCode) {
                    _ = MessageBox.Show("Ошибка авторизации");
                    return;
                }

                string result = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(result);
                _token = obj.token;
                _ = log.AppendLine("авторизован");
            }
            catch {

            }
        }
    }

    /// <summary>
    /// Устанавливает WebSocket соединения.
    /// </summary>
    private async Task ConnectAllSocketsAsync() {
        for (int i = 0; i < 3; i++) {
            int socketIndex = i;
            _sockets[i] = new ClientWebSocket();
            _cts[i] = new CancellationTokenSource();
            await ConnectSocketAsync(socketIndex);

            // Запуск фонового приема сообщений
            _ = Task.Run(() => ReceiveLoop(socketIndex));
        }
    }

    /// <summary>
    /// Подключает WebSocket с автоматическими попытками переподключения.
    /// </summary>
    private async Task ConnectSocketAsync(int index) {
        int attempts = 0;

        while (attempts < 10) {
            try {
                // Формируем URI с токеном в query строке
                Uri uriWithToken = new($"{_wsUri}?access_token={_token}");

                // Инициализируем WebSocket
                _sockets[index] = new ClientWebSocket();

                // Подключаемся
                await _sockets[index].ConnectAsync(uriWithToken, _cts[index].Token);

                // Успешное подключение
                return;
            }
            catch (Exception ex) {
                _ = log.AppendLine($"WebSocket[{index}] ошибка: {ex.Message}");
                attempts++;
                await Task.Delay(5000);
            }
        }

        // После 10 неудачных попыток показываем сообщение
        _ = MessageBox.Show($"WebSocket[{index}] не удалось подключиться");
    }

    /// <summary>
    /// Отправляет текст во все WebSocket подключения.
    /// </summary>
    private async Task SendToAllSocketsAsync(string message) {

        ClientWebSocket? socket = _sockets.First();
        {
            if (socket?.State == WebSocketState.Open) {
                // Создаём объект с токеном и сообщением
                var payload = new {
                    token = _token,
                    message = message
                };

                // Сериализуем объект в JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);

                // Кодируем JSON в массив байтов UTF-8
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                // Отправляем весь JSON одним сообщением
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

                //byte[] buffer = Encoding.UTF8.GetBytes(message); // затем само сообщение
                //await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        /* РАЗБИВАТЬ СООБЩЕНИЯ НА ЧАНКИ ЕСЛИ НУЖНО
        const int chunkSize = 8192; // 8 KB
        int offset = 0;

        while (offset < buffer.Length) {
            int size = Math.Min(chunkSize, buffer.Length - offset);
            bool endOfMessage = (offset + size) >= buffer.Length;

            await socket.SendAsync(
                new ArraySegment<byte>(buffer, offset, size),
                WebSocketMessageType.Text,
                endOfMessage,
                CancellationToken.None
            );

            offset += size;
        }
        */

    }

    private readonly StringBuilder log = new();

    /// <summary>
    /// Цикл получения сообщений с конкретного WebSocket.
    /// </summary>
    private async Task ReceiveLoop(int index) {
        byte[] buffer = new byte[1024];

        while (true) {
            try {
                if (_sockets[index].State != WebSocketState.Open) {
                    break;
                }

                WebSocketReceiveResult result = await _sockets[index].ReceiveAsync(new ArraySegment<byte>(buffer), _cts[index].Token);

                if (result.MessageType == WebSocketMessageType.Close) {
                    await _sockets[index].CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                //Console.WriteLine($"[{index}] => {message}");
                _ = log.AppendLine($"[{index}] => {message}");

            }
            catch {
                await ConnectSocketAsync(index);
            }
        }
    }

    private void timer1_Tick(object sender, EventArgs e) {
        textBox1.Text = log.ToString();
    }
}
