using CoreBot1.Common;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class CalificarDialog: ComponentDialog
    {
        protected readonly ILogger Logger;

        public CalificarDialog(ILogger <InicialDialog> logger) : base(nameof(CalificarDialog))
        {
            Logger = logger;

            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
           {
                Calificar,
                SetRespuesta,
                Fin

           }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> Calificar(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //LanzarCards Card = new LanzarCards();
            //var calificarCard = Card.CreateAdaptiveCardAttachment("cartaEleccion.json");
            //var response = MessageFactory.Attachment(calificarCard, ssml: "Welcome to Bot Framework!");
            //await stepContext.Context.SendActivityAsync(response);
            //return await stepContext.NextAsync(cancellationToken:cancellationToken);
            return await HeroCardDialog.ShowOptions(stepContext, cancellationToken, HeroCardDialog.CrearCalificacion());
        }

        private async Task<DialogTurnResult> SetRespuesta(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var nombre = (PersonaInfo)stepContext.Options;
            nombre.Calificaicon= stepContext.Context.Activity.Text;
            var textoMensaje = "gracias por tus comentarios \r ¿Puedo ayudarte con algo más? ";
            var promptMensaje = MessageFactory.Text(textoMensaje, textoMensaje, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMensaje }, cancellationToken);

        }

        private async Task<DialogTurnResult> Fin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var nombre = (PersonaInfo)stepContext.Options;
            var option = stepContext.Context.Activity.Text;
            if((bool)stepContext.Result)
            {
                await stepContext.Context.SendActivityAsync("De acuerdo, volveré a empezar.");
                return await stepContext.BeginDialogAsync(nameof(TemasDialog), nombre, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync($"Gracias {nombre.Nombre} por hablar conmigo.¡Adiós!");
                return await stepContext.EndDialogAsync(cancellationToken:cancellationToken);
            }

        }
    }
}
