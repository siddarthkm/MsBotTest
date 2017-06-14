using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.FormFlow;


namespace LUIS_Bot_Sample.Dialogs
{
    [LuisModel("86684636-488d-48b3-a4c6-233ef496d3d1", "5f3fca65f0a64d68ab6d4d474b1b0fa6", LuisApiVersion.V2, "westus", true, false, true, true)]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            string message = $"Greetings. How may I help you?";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
    }

}