using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CoreBot1.Common
{
    public class LanzarCards
    {
        public Attachment CreateAdaptiveCardAttachment(string card)
        {

            var cardResourcePath = GetType().Assembly.GetManifestResourceNames().First(name => name.EndsWith(card));

            using (var stream = GetType().Assembly.GetManifestResourceStream(cardResourcePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    var adaptiveCard = reader.ReadToEnd();
                    return new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(adaptiveCard),
                    };
                }
            }
        }
    }
}
