using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly StoreContext _context;

        public AdminController(StoreContext context)
        {
            _context = context;
        }

        // POST: api/Admin/agregar-producto
        [Authorize(Roles = "Administrador")]
        [HttpPost("agregar-producto")]
        public async Task<ActionResult<Product>> AgregarProducto([FromBody] Product producto)
        {
            _context.Products.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(AgregarProducto), new { id = producto.Id }, producto);
        }

        // PUT: api/Admin/actualizar-producto/1
        [HttpPut("actualizar-producto/{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Product producto)
        {
            var productoExistente = await _context.Products.FindAsync(id);
            if (productoExistente == null) return NotFound("Producto no encontrado");

            productoExistente.Name = producto.Name;
            productoExistente.Description = producto.Description;
            productoExistente.Price = producto.Price;
            productoExistente.BrandId = producto.BrandId;
            productoExistente.CategoryId = producto.CategoryId;

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto actualizado correctamente" });
        }

        // DELETE: api/Admin/eliminar-producto/1
        [HttpDelete("eliminar-producto/{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Products.FindAsync(id);
            if (producto == null) return NotFound("Producto no encontrado");

            _context.Products.Remove(producto);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto eliminado correctamente" });
        }

        // GET: api/Admin/listar-productos
        [HttpGet("listar-productos")]
        public async Task<ActionResult<IEnumerable<Product>>> ListarProductos()
        {
            var productos = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();

            return Ok(productos);
        }
        [Authorize(Roles = "Administrador")]
[HttpPost("agregar-marca")]
public async Task<IActionResult> AgregarMarca([FromBody] Brand brand)
{
    _context.Brands.Add(brand);
    await _context.SaveChangesAsync();
    return Ok(brand);
}
[Authorize(Roles = "Administrador")]
[HttpPost("agregar-categoria")]
public async Task<IActionResult> AgregarCategoria([FromBody] Category category)
{
    _context.Categories.Add(category);
    await _context.SaveChangesAsync();
    return Ok(category);
}

    }
}
