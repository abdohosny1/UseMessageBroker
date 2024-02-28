using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UseRabbitMQ
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

       // [JsonProperty]
        public Guid Id { get; private set; }

        //[JsonProperty]
        public DateTime CreationDate { get; private set; }
    }

    public class SendMessageInQueue : IntegrationEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
