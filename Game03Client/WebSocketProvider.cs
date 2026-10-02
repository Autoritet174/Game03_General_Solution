
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client;

/// <summary>
/// Provider for communicating with the server via SignalR.
/// </summary>
public class WebSocketProvider
{
    private static readonly Logger<WebSocketProvider> logger = new();
    private static HubConnection? connection;
    public static RetryPolicy? retryPolicy = null;

    public static bool sConnected => connection?.State == HubConnectionState.Connected;

    public static HubConnectionState state => connection?.State ?? HubConnectionState.Disconnected;

    // Pre-allocated logging delegates
    private static readonly Action<Logger<WebSocketProvider>, string, Exception> errorInvokeLogger =
        (l, method, ex) => l.LogError($"Error invoking {method}, {ex}");

    private static readonly Action<Logger<WebSocketProvider>, string, Exception> errorSendLogger =
        (l, method, ex) => l.LogError($"Error sending {method}, {ex}");

    private static readonly Action<Logger<WebSocketProvider>, string, Exception> errorDisconnectLogger =
        (l, msg, ex) => l.LogError($"Disconnect error: {msg}, {ex}");

    private static readonly Action<Logger<WebSocketProvider>, Exception> errorConnectLogger =
        (l, ex) => l.LogError($"Connection error: Exception: {ex}");

    private static readonly Action<Logger<WebSocketProvider>, string, Exception?> infoReceiveLogger =
        (l, msg, ex) => l.LogInfo($"Server log: {msg}, {ex}");

    public static async Task<bool> ConnectAsync(CancellationToken ctOpen, CancellationToken ctReceive)
    {
        if (ctOpen.IsCancellationRequested || ctReceive.IsCancellationRequested)
        {
            return false;
        }

        try
        {
            retryPolicy = new();
            await DisconnectAsync().ConfigureAwait(false);
            string url = Url.urlDomain + Parametrs.signalR_Address;

            connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    if (!string.IsNullOrWhiteSpace(Auth.accessToken))
                    {
                        options.AccessTokenProvider = () => Task.FromResult(Auth.accessToken)!;
                    }
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions = JSON.options;
                })
                .WithAutomaticReconnect(retryPolicy)
                .Build();

            RegisterServerEvents();

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ctOpen, ctReceive);
            await connection.StartAsync(linkedCts.Token).ConfigureAwait(false);

            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            errorConnectLogger(logger, ex);
            return false;
        }
    }

    /// <summary>
    /// Registers server-to-client event handlers.
    /// </summary>
    private static void RegisterServerEvents()
    {
        if (connection == null)
        {
            return;
        }

        _ = connection.On<string>("ReceiveLog", message => infoReceiveLogger(logger, message, null));

        // Register new server events here as needed:
        // _connection.On<SomeDto>("EventName", dto => { ... });
    }

    /// <summary>
    /// Invokes a server hub method with a return value.
    /// </summary>
    public static async Task<TResponse?> InvokeAsync<TResponse>(
        HubMethodNames.EMethod eMethod,
        CancellationToken ct,
        params object?[] args)
    {
        if (connection == null || !sConnected)
        {
            return default;
        }

        string methodName = HubMethodNames.GetMethod(eMethod);

        try
        {
            return await (args.Length switch
            {
                0 => connection.InvokeAsync<TResponse>(methodName, ct),
                1 => connection.InvokeAsync<TResponse>(methodName, args[0], ct),
                2 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], ct),
                3 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], ct),
                4 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], ct),
                5 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], ct),
                6 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], ct),
                7 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], ct),
                8 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], ct),
                9 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], ct),
                10 => connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], ct),
                _ => throw new ArgumentOutOfRangeException(nameof(args), "Too many arguments (max 10)")
            }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            errorInvokeLogger(logger, methodName, ex);
            return default;
        }
    }

    /// <summary>
    /// Invokes a server hub method without a return value (fire-and-forget).
    /// </summary>
    public static async Task<bool> SendAsync(string methodName, CancellationToken ct = default, params object?[] args)
    {
        if (connection == null || !sConnected)
        {
            return false;
        }

        try
        {
            await connection.InvokeAsync(methodName, args, ct).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            errorSendLogger(logger, methodName, ex);
            return false;
        }
    }

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public static async Task DisconnectAsync()
    {
        if (connection == null)
        {
            return;
        }

        retryPolicy = null;
        try
        {
            await connection.StopAsync().ConfigureAwait(false);
            await connection.DisposeAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            errorDisconnectLogger(logger, ex.Message, ex);
        }
        finally
        {
            connection = null;
        }
    }
}

/// <summary>
/// Custom retry policy for automatic reconnection.
/// </summary>
public class RetryPolicy : IRetryPolicy
{
    public long currentAttemptCount { get; private set; }
    public TimeSpan? currentDelay { get; private set; }

    // Свойство: идет ли попытка переподключения прямо сейчас
    public bool isReconnecting { get; private set; }

    private DateTime nextAttemptTime;

    public double secondsUntilNextAttempt
    {
        get
        {
            TimeSpan remaining = nextAttemptTime - DateTime.UtcNow;
            return remaining.TotalSeconds > 0 ? remaining.TotalSeconds : 0;
        }
    }

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        // Начинаем попытку переподключения
        isReconnecting = true;

        currentAttemptCount = retryContext.PreviousRetryCount + 1;

        currentDelay = retryContext.PreviousRetryCount switch
        {
            < 1 => TimeSpan.FromSeconds(0.2),
            < 10 => TimeSpan.FromSeconds(5),
            _ => TimeSpan.FromSeconds(30)
        };

        // Устанавливаем время следующей попытки
        nextAttemptTime = DateTime.UtcNow + (currentDelay ?? TimeSpan.Zero);

        return currentDelay;
    }
}
