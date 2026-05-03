using General;
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
    private static HubConnection? _connection;

    public static bool IsConnected => _connection?.State == HubConnectionState.Connected;

    // Pre-allocated logging delegates
    private static readonly Action<Logger<WebSocketProvider>, string, Exception?> _errorInvokeLogger =
        (l, method, ex) => l.LogError($"Error invoking {method}");

    private static readonly Action<Logger<WebSocketProvider>, string, Exception?> _errorSendLogger =
        (l, method, ex) => l.LogError($"Error sending {method}");

    private static readonly Action<Logger<WebSocketProvider>, string, Exception?> _errorDisconnectLogger =
        (l, msg, ex) => l.LogError($"Disconnect error: {msg}");

    private static readonly Action<Logger<WebSocketProvider>, Exception?> _errorConnectLogger =
        (l, ex) => l.LogError("Connection error");

    private static readonly Action<Logger<WebSocketProvider>, string, Exception?> _infoReceiveLogger =
        (l, msg, ex) => l.LogInfo($"Server log: {msg}");

    public static async Task<bool> ConnectAsync(CancellationToken ctOpen, CancellationToken ctReceive)
    {
        if (ctOpen.IsCancellationRequested || ctReceive.IsCancellationRequested)
        {
            return false;
        }

        try
        {
            await DisconnectAsync().ConfigureAwait(false);

            _connection = new HubConnectionBuilder()
                .WithUrl(Parametrs.SignalR_Address, options =>
                {
                    if (!string.IsNullOrWhiteSpace(Auth.AccessToken))
                    {
                        options.AccessTokenProvider = () => Task.FromResult(Auth.AccessToken)!;
                    }
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions = JsonSettings.JsonOptions;
                })
                .WithAutomaticReconnect(new RetryPolicy())
                .Build();

            RegisterServerEvents();

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ctOpen, ctReceive);
            await _connection.StartAsync(linkedCts.Token).ConfigureAwait(false);

            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _errorConnectLogger(logger, ex);
            return false;
        }
    }

    /// <summary>
    /// Registers server-to-client event handlers.
    /// </summary>
    private static void RegisterServerEvents()
    {
        if (_connection == null)
        {
            return;
        }

        _ = _connection.On<string>("ReceiveLog", message => _infoReceiveLogger(logger, message, null));

        // Register new server events here as needed:
        // _connection.On<SomeDto>("EventName", dto => { ... });
    }

    /// <summary>
    /// Invokes a server hub method with a return value.
    /// </summary>
    public static async Task<TResponse?> InvokeAsync<TResponse>(
        General.HubMethodNames.EMethod eMethod,
        CancellationToken ct = default,
        params object?[] args)
    {
        if (_connection == null || !IsConnected)
        {
            return default;
        }

        string methodName = General.HubMethodNames.GetMethod(eMethod);

        try
        {
            return await (args.Length switch
            {
                0 => _connection.InvokeAsync<TResponse>(methodName, ct),
                1 => _connection.InvokeAsync<TResponse>(methodName, args[0], ct),
                2 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], ct),
                3 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], ct),
                4 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], ct),
                5 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], ct),
                6 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], ct),
                7 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], ct),
                8 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], ct),
                9 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], ct),
                10 => _connection.InvokeAsync<TResponse>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], ct),
                _ => throw new ArgumentOutOfRangeException(nameof(args), "Too many arguments (max 10)")
            }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _errorInvokeLogger(logger, methodName, ex);
            return default;
        }
    }

    /// <summary>
    /// Invokes a server hub method without a return value (fire-and-forget).
    /// </summary>
    public static async Task<bool> SendAsync(string methodName, CancellationToken ct = default, params object?[] args)
    {
        if (_connection == null || !IsConnected)
        {
            return false;
        }

        try
        {
            await _connection.InvokeAsync(methodName, args, ct).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _errorSendLogger(logger, methodName, ex);
            return false;
        }
    }

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public static async Task DisconnectAsync()
    {
        if (_connection == null)
        {
            return;
        }

        try
        {
            await _connection.StopAsync().ConfigureAwait(false);
            await _connection.DisposeAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _errorDisconnectLogger(logger, ex.Message, ex);
        }
        finally
        {
            _connection = null;
        }
    }
}

/// <summary>
/// Custom retry policy for automatic reconnection.
/// </summary>
internal class RetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext) => retryContext.PreviousRetryCount switch
    {
        < 3 => TimeSpan.Zero,
        < 10 => TimeSpan.FromSeconds(5),
        _ => TimeSpan.FromSeconds(30)
    };
}
