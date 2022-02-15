using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class InicialDialog : ComponentDialog
    {
        protected readonly ILogger Logger;
        


        public InicialDialog(ILogger<InicialDialog> logger, TemasDialog temasDialog) : base (nameof(InicialDialog))
        {
            Logger = logger;
            AddDialog(temasDialog);
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                SaludoInicialAsync,
                TerminosYcondiciones,
                PedirNombre,
                SetNombre
                
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SaludoInicialAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("¡Hola! soy MUAbot, asistente virtual de la E.S.E.Hospital Manuel Uribe Ángel."));
            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> TerminosYcondiciones(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var textoMensaje = "Esta conversación podrá ser grabada y monitoreada de acuerdo a los Términos y Condiciones para mejorar nuestros servicios y " +
                "funcionalidades * Link de Términos y Condiciones* ¿Estás de acuerdo ? ";
            var promptMensaje = MessageFactory.Text(textoMensaje, textoMensaje, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMensaje }, cancellationToken);

        }

        private async Task<DialogTurnResult> PedirNombre(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var textoMensaje = "Para brindarte una atención personalizada, indícame tu nombre";
            var textoNoMensaje = "En este momento no puedo ayudarte, debes aceptar nuestros Términos y Condiciones para continuar con la atención";
            var promptMensaje = MessageFactory.Text(textoMensaje, textoMensaje, InputHints.ExpectingInput);

            if ((bool)stepContext.Result)
            {
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMensaje }, cancellationToken);
               
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(textoNoMensaje));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> SetNombre(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            PersonaInfo personaInfo = new PersonaInfo();
            personaInfo.Nombre = stepContext.Context.Activity.Text;
            //await stepContext.Context.SendActivityAsync($"tu nombre es: {personaInfo.Nombre}");
            return await stepContext.BeginDialogAsync(nameof(TemasDialog),personaInfo, cancellationToken);

            
        }


    }
}
