using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class SinAyudaDialog: ComponentDialog
    {
        protected readonly ILogger Logger;

        public SinAyudaDialog(ILogger <InicialDialog> logger, CalificarDialog calificarDialog) : base(nameof(SinAyudaDialog))
        {
            Logger = logger;

            AddDialog(calificarDialog);
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                preguntar,
                redireccionar

            }));

            InitialDialogId = nameof(WaterfallDialog);

        }

        private async Task<DialogTurnResult> preguntar(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var textoMensaje = "¿Siento no poder ayudarte ¿te gustaría calificar este servicio?";
            var promptMensaje = MessageFactory.Text(textoMensaje, textoMensaje, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMensaje }, cancellationToken);
        }

        private async Task<DialogTurnResult> redireccionar(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var nombre =(PersonaInfo) stepContext.Options;
            if ((bool)stepContext.Result)
            {
                return await stepContext.BeginDialogAsync(nameof(CalificarDialog),nombre,cancellationToken);

            }
            else
            {
                await stepContext.Context.SendActivityAsync($"Gracias {nombre.Nombre} por hablar conmigo.¡Adiós!");
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }
        }

    }
}
