using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("5668131568:AAHvD_WV5UmS9mwTfRRqFMsRbQakI2lmVIQ");

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    int test = 0;
    int[] mas = {100,101,102,200,201,202,203,204,206,207,300,301,302,303,304,305,307,308,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,420,421,422,423,424,425,426,429,431,444,450,451,497,498,499,500,501,502,503,504,506,507,508,509,510,511,521,522,523,525,599};
    var maks = string.Join(" ", mas);
    if (update.Message is not { } message)
        return;

    if (message.Text is not { } messageText)
        return;

    if (message.Text.ToLower().Contains("привет"))
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Привет!\n Напиши код ответа HTTP и я отправлю тебе котика \n Если не знаешь коды, напиши:\n Код");
        return;
    }
    if (message.Text.ToLower().Contains("код"))
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Доступные коды:\n" + maks);
        return;
    }

    if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
    {
        int txt = 0;
        int msg = 0;
        for (int i = 0; i < mas.Length; i++)
        {
            if (msg == (mas[i]))
            {
                var jpg = ".jpg";
                var href = "https://http.cat/";
                var chatId = message.Chat.Id;
                Message messag = await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: href + messageText + jpg,
                    caption: "Лови ;)",
                    cancellationToken: cancellationToken);
                
                return;
            }
        }
        
    }

    var chatIdd = message.Chat.Id;
    Message messa = await botClient.SendPhotoAsync(
        chatId: chatIdd,
        photo: "http.cat/404.jpg",
        caption: "ОшибОчка, не найдено :(",
        cancellationToken: cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
