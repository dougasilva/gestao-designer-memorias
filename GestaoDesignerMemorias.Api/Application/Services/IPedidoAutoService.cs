using GestaoDesignerMemorias.Domain.Enums;

namespace Application.Services
{
    public interface IPedidoAutoService
    {
        Task<Guid?> CriarPedidoSeAplicavelAsync(
            Guid clienteId,
            TipoIntencaoMensagem intencao
        );
    }
}
