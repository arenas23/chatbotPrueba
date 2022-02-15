using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class TramiteEnLineaDialog: ComponentDialog
    {
        protected readonly ILogger Logger;

        public TramiteEnLineaDialog(ILogger<InicialDialog> logger) : base(nameof(TramiteEnLineaDialog))
        {
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                TramiteEnLinea
                

            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> TramiteEnLinea(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var nombre = (PersonaInfo)stepContext.Options;
            await stepContext.Context.SendActivityAsync($"{nombre.Nombre} serás redirigido a nuestro trámite en línea en portal GOV.CO de Colombia." +
                "Sigue las indicaciones y diligencia los datos solicitados. Una vez recibamos tu solicitud, uno de nuestros asesores te contactará" +
                "para programar la cita según la disponibilidad de agendas. Asignación de cita para la prestación de servicios en salud");
            return await stepContext.BeginDialogAsync(nameof(RespondioPreguntaDialog), nombre, cancellationToken);
        }

        
    }
}
