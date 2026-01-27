using GestaoDesignerMemorias.Domain.Enums;

namespace Application.Services
{
    public interface IMessageClassifier
    {
        MessageCategory Classify(string message);
    }
}
