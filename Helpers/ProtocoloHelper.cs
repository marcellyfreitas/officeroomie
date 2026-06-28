namespace OfficeRoomie.Helpers;

public class ProtocoloHelper
{
    public static string GerarProtocolo()
    {
        string data = DateTime.Now.ToString("yyyyMMdd");
        string identificadorUnico = Guid.NewGuid().ToString("N").Substring(0, 8);
        string protocolo = $"RES-{data}-{identificadorUnico}".ToUpper();

        return protocolo;
    }
}
