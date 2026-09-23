using ProjetoDto.Api.Dtos;

namespace ProjetoDto.Api.Services
{
    public interface IProdutoMapsterService
    {
        Task<List<ProdutoReadDto>> GetAllAsync();
        Task<ProdutoReadDto?> GetByIdAsync(int id);
        Task<ProdutoReadDto> CreateAsync(ProdutoCreateDto produtoCreateDto);
        Task<ProdutoReadDto> UpdateAsync(int id, ProdutoUpdateDto produtoUpdateDto);
        Task<bool> DeleteAsync(int id);
    }
}