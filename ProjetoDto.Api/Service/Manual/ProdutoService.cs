using Microsoft.EntityFrameworkCore;
using ProjetoDto.Api.Data;
using ProjetoDto.Api.Dtos;
using ProjetoDto.Api.Models;

namespace ProjetoDto.Api.Service.Manual
{
    public class ProdutoService : IProdutoService
    {
        private readonly AppDbContext _context;

        public ProdutoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProdutoReadDto>> GetAllAsync()
        {
            //var result = await _context.Produtos.ToListAsync();
            var produtos = await _context.Produtos
                           .Where(p => p.Ativo)
                           .ToListAsync();

            // Converte Entity → DTO
            //return produtos.Select(p => new ProdutoReadDto ou
            var result = produtos.Select(p => new ProdutoReadDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Preco = p.Preco,
                Descricao = p.Descricao,
                DataCriacao = p.DataCriacao,
                Ativo = p.Ativo
            }).ToList();

            return result;
        }

        public async Task<ProdutoReadDto?> GetByIdAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return null;

            // Converte Entity → DTO
            return new ProdutoReadDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Descricao = produto.Descricao,
                DataCriacao = produto.DataCriacao,
                Ativo = produto.Ativo
            };
        }

        public async Task<ProdutoReadDto> CreateAsync(ProdutoCreateDto produtoCreateDto)
        {
            // Converte DTO → Entity
            var produto = new Produto
            {
                Nome = produtoCreateDto.Nome,
                Preco = produtoCreateDto.Preco,
                Descricao = produtoCreateDto.Descricao,
                DataCriacao = DateTime.Now,
                Ativo = true
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            // Retorna o DTO de leitura
            return new ProdutoReadDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Descricao = produto.Descricao,
                DataCriacao = produto.DataCriacao,
                Ativo = produto.Ativo
            };
        }

        public async Task<ProdutoReadDto> UpdateAsync(int id, ProdutoUpdateDto produtoUpdateDto)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return null;

            // Atualiza apenas os campos que vieram no DTO
            produto.Nome = produtoUpdateDto.Nome;
            produto.Preco = produtoUpdateDto.Preco;
            produto.Descricao = produtoUpdateDto.Descricao;
            produto.Ativo = produtoUpdateDto.Ativo;

            await _context.SaveChangesAsync();

            // Retorna o DTO atualizado
            return new ProdutoReadDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Descricao = produto.Descricao,
                DataCriacao = produto.DataCriacao,
                Ativo = produto.Ativo
            };
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