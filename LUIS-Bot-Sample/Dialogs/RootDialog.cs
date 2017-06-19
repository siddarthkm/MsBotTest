using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;



namespace LUIS_Bot_Sample.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            //context.PostAsync($"Hello I'm Tizen LUIS. How may I serve you.");
            //task.Wait();

            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

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

                    case "HomeAutomation.TurnOn":

                        await handleHomeAutomation(context, luisResult, true);
                        break;

                    case "HomeAutomation.TurnOff":

                        await handleHomeAutomation(context, luisResult, false);
                        break;

                    case "Music.DecreaseVolume":
                        await context.PostAsync("Decreasing the volume");
                        break;

                    case "Music.IncreaseVolume":
                        await context.PostAsync("Increasing the volume");
                        break;

                    case "Music.Mute":
                        await context.PostAsync("Muting the volume");
                        break;

                    case "Music.Unmute":
                        await context.PostAsync("Unmuting the volume");
                        break;

                    case "Music.PlayMusic":
                        await context.PostAsync("Playing Music");
                        break;

                    case "Music.Pause":
                        await context.PostAsync("Pausing Music");
                        break;

                    case "Music.Repeat":
                        await context.PostAsync("Replaying Music");
                        break;

                    case "Music.Stop":
                        await context.PostAsync("Stopping Music");
                        break;

                    case "Music.SkipForward":
                        await context.PostAsync("Skipping current music track");
                        break;

                    default:
                        await context.PostAsync($"I recognized your intent as {luisResult.TopScoringIntent.Intent}...\nHowever I'm not configured to reply to it");
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

        private async Task handleHomeAutomation(IDialogContext context, LuisResult result, bool on)
        {
            var entities = result.Entities;

            bool found = false;
            string device = "";
            EntityRecommendation entity;

            found = result.TryFindEntity("HomeAutomation.Device", out entity);

            //foreach (var x in entities)
            //{
            //    if (x.Type == "HomeAutomation.Device")
            //    {
            //        found = true;
            //        device = x.Entity;
            //        break;
            //    }
            //}

            if (found)
            {
                device = entity?.Entity;
                await context.PostAsync($"Turning {(on ? "on" : "off")} the {device}");
            }
            else
            {
                await context.PostAsync($"I did not recognize a device to turn {(on ? "on" : "off")}...\nPlease repeat your command with the device name");
            }
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
