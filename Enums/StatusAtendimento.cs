using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace API_sis_conselhotutelarv2.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusAtendimento
    {
        Ativo,
        Pendente,
        Inativo
    }
}

