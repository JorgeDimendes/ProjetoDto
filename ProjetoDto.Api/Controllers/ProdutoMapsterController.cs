using Microsoft.AspNetCore.Mvc;
using ProjetoDto.Api.Dtos;
using ProjetoDto.Api.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoDto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoMapsterController : ControllerBase
    {
        private readonly IProdutoMapsterService _produtoService;
        public ProdutoMapsterController(IProdutoMapsterService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdutoReadDto>>> GetAllAsync()
        {
            var produtos = await _produtoService.GetAllAsync();
            if (produtos.Count == 0) return BadRequest("Nunhum produtos localizado");
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoReadDto>> GetById(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null) return NotFound($"Produto com id {id} não encontrado.");

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoReadDto>> PostAsync(ProdutoCreateDto produtoDto)
        {
            var produto = await _produtoService.CreateAsync(produtoDto);

            if (produto == null)
                return BadRequest("Erro ao cadastrar produto");

            return CreatedAtAction(
                nameof(GetById),
                new { id = produto.Id },
                produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoReadDto>> UpdateAsync(int id, ProdutoUpdateDto produtoDto)
        {
            var produto = await _produtoService.UpdateAsync(id, produtoDto);

            if (produto == null) return NotFound($"Produto com id {id} não encontrado.");

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var sucesso = await _produtoService.DeleteAsync(id);
            if (!sucesso) NotFound("Erro ao deletar produto");
            return NoContent();
        }
    }
}