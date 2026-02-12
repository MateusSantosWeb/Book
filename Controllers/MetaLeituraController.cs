using BookShelfAPI.Data;
using BookShelfAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShelfAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

public class MetaLeituraController :  ControllerBase
{
    private readonly BookShelfContext _context;
    
    public MetaLeituraController(BookShelfContext context)
    {
        _context = context; 
        
    }
    [HttpGet("{usuario/{usuarioId}}")]
    public async Task<ActionResult<MetaLeituraController>> GetMetaLeituras(int usuarioId)
}