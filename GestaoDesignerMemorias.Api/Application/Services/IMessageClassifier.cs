using GestaoDesignerMemorias.Domain.Enums;

namespace Application.Services
{
    public interface IMessageClassifier
    {
        TipoIntencaoMensagem Classificar(string message);
    }
}
