using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class CanalAtencionDialog: ComponentDialog
    {
        protected readonly ILogger Logger;
        PersonaInfo nombre = new PersonaInfo();


        public CanalAtencionDialog (ILogger <InicialDialog> logger, SinAyudaDialog sinAyudaDialog):base(nameof(CanalAtencionDialog))
        {
            Logger = logger;


            AddDialog(sinAyudaDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                Preguntar,
                CanalesAtencion



            }));

            InitialDialogId = nameof(WaterfallDialog);

        }

        private async Task<DialogTurnResult> Preguntar(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            nombre =(PersonaInfo) stepContext.Options;
            var textoMensaje = $"{nombre.Nombre} ¡Cuéntame! ¿Qué otro tema te gustaría encontrar en este espacio ? ";
            var promptMensaje = MessageFactory.Text(textoMensaje, textoMensaje, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMensaje }, cancellationToken);
        }

        private async Task<DialogTurnResult> CanalesAtencion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Lamento no poder ayudarte en este momento con tu solicitud.Estos son nuestros canales de atención donde podrán brindarte una atención más personalizada.");
            await stepContext.Context.SendActivityAsync("E.S.E. Hospital Manuel Uribe Ángel (Sede principal) Dirección: Diagonal 31 #36A sur – 80, Antioquia, Envigado Horario de atención: 24 horas " +
                "Teléfonos: (604) 4487400 Email: atencionalusuario@hospitalmua.gov.co Sede Santa Gertrudis y Unidades Básicas de Atención Horario de atención: Lunes a jueves de 7:00 a.m.a 5:00 p.m.y " +
                "viernes de 7:00 a.m.a 4:00 p.m. Teléfonos: (604) 448 94 00 | Citas(604) 448 74 00 Email: radicacion@hospitalmua.gov.co ");
            return await stepContext.BeginDialogAsync(nameof(SinAyudaDialog),nombre,cancellationToken);
        }
    }
    
}
