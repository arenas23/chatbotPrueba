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
    public class TemasDialog : ComponentDialog
    {
        protected readonly ILogger Logger;
        public PersonaInfo nombre;


        public TemasDialog(ILogger<InicialDialog> logger, TramiteEnLineaDialog tramiteEnLineaDialog,
            RespondioPreguntaDialog respondioPreguntaDialog, CanalAtencionDialog canalAtencionDialog) : base(nameof(TemasDialog))
        {
            Logger = logger;

            AddDialog(tramiteEnLineaDialog);
            AddDialog(respondioPreguntaDialog);
            AddDialog(canalAtencionDialog);
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                MostrarTemas,
                respuesta,
                CanalCita
           

            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> MostrarTemas(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            nombre = (PersonaInfo)stepContext.Options;
            await stepContext.Context.SendActivityAsync($"¡Hola, {nombre.Nombre}! Por este medio puedo ayudarte con los siguientes temas");

            return await HeroCardDialog.ShowOptions(stepContext, cancellationToken,HeroCardDialog.CreateHeroCard());
            
            
            
            //return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> respuesta(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            
            var option =Int16.Parse( stepContext.Context.Activity.Text);

            switch (option)
            {
                case 1:
                    await stepContext.Context.SendActivityAsync("En el Hospital Manuel Uribe Ángel prestamos servicios de salud de primer, " +
                        "segundo y tercer nivel de complejidad a través de un modelo de atención integral centrado en el usuario y su familia. " +
                        "Ingresando en el siguiente enlace conoce más sobre nuestro portafolio de servicios. Nuestros servicios");
                    return await stepContext.BeginDialogAsync(nameof(TramiteEnLineaDialog), nombre, cancellationToken);
                    
                case 2:
                    await stepContext.Context.SendActivityAsync("¡Gracias por elegirnos como una opción para desarrollarte profesionalmente! " +
                    "Ingresa al siguiente enlace y entérate de nuestras vacantes disponibles, si son de tu interés sigue las indicaciones " +
                    "¡Mucha suerte! Trabaje con nosotros");
                    return await stepContext.BeginDialogAsync(nameof(RespondioPreguntaDialog), nombre, cancellationToken);
                    
                case 3:
                    await stepContext.Context.SendActivityAsync($"Nombre {nombre.Nombre } en la ESE Hospital Manuel Uribe Ángel contamos con tres canales para " +
                        "la solicitud de citas.Selecciona la opción que más se adapte a tus necesidades");
                    return await HeroCardDialog.ShowOptions(stepContext, cancellationToken, HeroCardDialog.CreateCanalesCard());
                case 4:
                    return await stepContext.BeginDialogAsync(nameof(CanalAtencionDialog), nombre, cancellationToken);
            }
            //await stepContext.Context.SendActivityAsync($"elegiste la opcion {option}");
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> CanalCita(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var option = stepContext.Context.Activity.Text;
            switch (option)
            {
                case "linea telefonica":
                    await stepContext.Context.SendActivityAsync("Comunícate al +57 (604) 448 74 00 de lunes a jueves de 7:00 a.m.a 5:00 p.m.y viernes de 7:00 a.m. a 4:00 p.m. ");
                    return await stepContext.BeginDialogAsync(nameof(RespondioPreguntaDialog),nombre,cancellationToken);
                    
                case "correo electronico":
                    await stepContext.Context.SendActivityAsync("Envía un correo electrónico a citas@hospitalmua.gov.co indicando los siguientes datos: \r -Nombre completo del paciente" +
                        "\r - Documento de identidad \r - EPS  \r- Especialidad o examen médico \r - Número de autorización \r - Teléfonos de contacto. El área encargada de programación de citas médicas" +
                        "se comunicará contigo tan pronto contemos con disponibilidad de agendas");
                    return await stepContext.BeginDialogAsync(nameof(RespondioPreguntaDialog), nombre, cancellationToken);
                    
                case "tramite en linea":
                    return await stepContext.BeginDialogAsync(nameof(TramiteEnLineaDialog), nombre, cancellationToken);
                    

            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }



        //private async Task<DialogTurnResult> fin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //if ((bool)stepContext.Result)
        //{
        //return await stepContext.BeginDialogAsync(nameof(CalificarDialog));

        //}
        //else
        //{
        //return await stepContext.BeginDialogAsync(nameof(SinAyudaDialog),nombre,cancellationToken);
        //}
        //}
    }
}
