using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class RespondioPreguntaDialog: ComponentDialog
    {
        protected readonly ILogger Logger;

        public RespondioPreguntaDialog(ILogger <InicialDialog> logger,CalificarDialog calificarDialog,
            SinAyudaDialog sinAyudaDialog) : base (nameof(RespondioPreguntaDialog))
        {
            AddDialog(calificarDialog);
            AddDialog(sinAyudaDialog);

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
           {
                Preguntar,
                Fin


           }));

            InitialDialogId = nameof(WaterfallDialog);

        }

        private async Task<DialogTurnResult> Preguntar(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var textoMensaje = "¿He respondido tu pregunta?";
            var promptMensaje = MessageFactory.Text(textoMensaje, textoMensaje, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMensaje }, cancellationToken);
        }

        private async Task<DialogTurnResult> Fin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var nombre = (PersonaInfo)stepContext.Options;

            if ((bool)stepContext.Result)
            {
                return await stepContext.BeginDialogAsync(nameof(CalificarDialog),nombre,cancellationToken);

            }
            else
            {
                return await stepContext.BeginDialogAsync(nameof(SinAyudaDialog), nombre, cancellationToken);
            }
        }
    }
}
