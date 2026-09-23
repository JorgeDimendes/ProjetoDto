using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjetoDto.Api.Data;
using ProjetoDto.Api.Dtos;
using ProjetoDto.Api.Models;

namespace ProjetoDto.Api.Services
{
    public class ProdutoMapsterService : IProdutoMapsterService
    {
        private readonly AppDbContext _context;

        public ProdutoMapsterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProdutoReadDto>> GetAllAsync()
        {
            //var result = await _context.Produtos.ToListAsync();
            var produtos = await _context.Produtos
                           .Where(p => p.Ativo)
                           .ToListAsync();

            //Mapster
            return produtos.Adapt<List<ProdutoReadDto>>();
        }

        public async Task<ProdutoReadDto?> GetByIdAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return null;

            //Mapster
            return produto.Adapt<ProdutoReadDto>();
        }

        public async Task<ProdutoReadDto> CreateAsync(ProdutoCreateDto produtoCreateDto)
        {
            //Mapster
            var produto = produtoCreateDto.Adapt<Produto>();

            // Campos que não vêm do DTO
            produto.DataCriacao = DateTime.Now;
            produto.Ativo = true;

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            // Retorna o DTO de leitura
            return produto.Adapt<ProdutoReadDto>();
        }

        public async Task<ProdutoReadDto> UpdateAsync(int id, ProdutoUpdateDto produtoUpdateDto)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return null;

            // Atualiza a entity com os dados do DTO
            produtoUpdateDto.Adapt(produto);   // ← forma recomendada no Update

            await _context.SaveChangesAsync();

            // Retorna o DTO atualizado
            return produto.Adapt<ProdutoReadDto>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return false;

            // Soft delete (recomendado) ou hard delete
            // Hard delete:
            // _context.Produtos.Remove(produto);

            // Soft delete (melhor prática):
            produto.Ativo = false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}