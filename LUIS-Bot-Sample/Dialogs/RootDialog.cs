using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;



namespace LUIS_Bot_Sample.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            var task = context.PostAsync($"Hello I'm Tizen LUIS. How may I serve you.");
            task.Wait();

            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            //await context.PostAsync($"Starting Luis query");

            try
            {
                //LuisService luis = new LuisService(new LuisModelAttribute("86684636-488d-48b3-a4c6-233ef496d3d1", "5f3fca65f0a64d68ab6d4d474b1b0fa6", LuisApiVersion.V2, "westus", true, false, true, true));
                LuisService luis = new LuisService(new LuisModelAttribute("86684636-488d-48b3-a4c6-233ef496d3d1", "5f3fca65f0a64d68ab6d4d474b1b0fa6"));
                var luisResult = await luis.QueryAsync(activity.Text, System.Threading.CancellationToken.None);

                //await context.PostAsync($"Luis Result received .. Processing intents");

                switch (luisResult.TopScoringIntent.Intent)
                {
                    case "":
                    case "None":
                        await context.PostAsync($"Please repeat your query. I did not understand...");
                        break;
                    case "Greeting":
                        await context.PostAsync($"Greetings to you too. Anything I can do for you?");
                        break;
                    default:
                        await context.PostAsync($"Please repeat your query. I could not recognize your intent...");
                        break;
                }
            }
            catch (Exception exc)
            {
                await context.PostAsync($"Error while processing Luis query\n{exc}");
            }

            // return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            context.Wait(MessageReceivedAsync);
            //await context.Forward(new RootLuisDialog(), MessageReceivedAsync, activity, System.Threading.CancellationToken.None);
        }
    }

    [Serializable]
    public class ErrorDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {

            context.PostAsync($"Starting Exception Bot").Wait();


            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user

            await context.PostAsync($"Received an exception. MessageRewceived func of Error Dialog");


            await context.PostAsync($"Received an exception\n{activity?.Text}" );
            context.Wait(MessageReceivedAsync);
            //await context.Forward(new RootLuisDialog(), MessageReceivedAsync, activity, System.Threading.CancellationToken.None);
        }
    }

}
