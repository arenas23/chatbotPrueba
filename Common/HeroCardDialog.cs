using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Common
{
    public class HeroCardDialog
    {
        public static async Task<DialogTurnResult> ShowOptions(WaterfallStepContext stepContext, CancellationToken cancellationToken, Activity actividad)
        {
            var option = await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions
                    {
                        Prompt = actividad
                    },
                    cancellationToken
                );
            return option;
        }
        public static Activity CreateHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "temas",
                Buttons = new List<CardAction>()
                {
                new CardAction(){Title = "portafolio de servicios", Value = '1', Type = ActionTypes.ImBack },
                new CardAction(){Title = "trabaja con nosotros", Value = '2', Type = ActionTypes.ImBack },
                new CardAction(){Title = "solicitud de citas y examenes medicos", Value = '3', Type = ActionTypes.ImBack },
                new CardAction(){Title = "otro tema", Value = '4', Type = ActionTypes.ImBack },

                }
            };
            return MessageFactory.Attachment(heroCard.ToAttachment()) as Activity;
        }

        public static Activity CreateCanalesCard()
        {


            var heroCard = new HeroCard
            {
                Title = "canales de comunicacion",
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "linea telefonica", Value = "linea telefonica", Type = ActionTypes.ImBack },
                    new CardAction(){Title = "correo electronico", Value = "correo electronico", Type = ActionTypes.ImBack },
                    new CardAction(){Title = "tramite en linea", Value = "tramite en linea", Type = ActionTypes.ImBack },
                }
            };
            return MessageFactory.Attachment(heroCard.ToAttachment()) as Activity;
        }

        public static Activity CrearCalificacion()
        {
            var reply = MessageFactory.Text("¡Genial! Por favor, califica tu experiencia");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Image= "https://w7.pngwing.com/pngs/624/18/png-transparent-cute-little-star-little-stars-star-lovely-star.png",Value="1",Type = ActionTypes.ImBack },
                    new CardAction(){ Title = "", Image= "https://w7.pngwing.com/pngs/624/18/png-transparent-cute-little-star-little-stars-star-lovely-star.png",Value="2",Type = ActionTypes.ImBack },
                    new CardAction(){ Title = "", Image= "https://w7.pngwing.com/pngs/624/18/png-transparent-cute-little-star-little-stars-star-lovely-star.png",Value="3",Type = ActionTypes.ImBack },
                    new CardAction(){ Title = "", Image= "https://w7.pngwing.com/pngs/624/18/png-transparent-cute-little-star-little-stars-star-lovely-star.png",Value="4",Type = ActionTypes.ImBack },
                    new CardAction(){ Title = "", Image= "https://w7.pngwing.com/pngs/624/18/png-transparent-cute-little-star-little-stars-star-lovely-star.png",Value="5",Type = ActionTypes.ImBack },
                }
            };
            return reply as Activity;
        }
    }



   
}
