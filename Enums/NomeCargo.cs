using System.ComponentModel;
using System.Runtime.Serialization;

namespace API_sis_conselhotutelarv2.Enums
{
    public enum NomeCargo
    {
        [EnumMember(Value = "Administrador")]
        Administrador,
        [EnumMember(Value = "Conselheiro")]
        Conselheiro
    }
}
