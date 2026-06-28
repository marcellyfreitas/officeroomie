using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeRoomie.Database;
using OfficeRoomie.Helpers;
using OfficeRoomie.Models;
using OfficeRoomie.Models.ViewModels;

namespace OfficeRoomie.Controllers;

[Authorize]
public class AdministradoresController : Controller
{
    private readonly AppDbContext _context;

    public AdministradoresController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? pageNumber, string currentFilter, string searchString)
    {
        if (searchString != null) {
            pageNumber = 1;
        } else {
            searchString = currentFilter;
        }

        ViewData["CurrentFilter"] = searchString;

        var administradores = _context.Administradores.AsNoTracking().OrderByDescending(a => a.id);
        var admistradoresFiltrados = !String.IsNullOrEmpty(searchString) 
            ? administradores.Where(s => s.nome.ToLower().Contains(searchString.ToLower())) 
            : administradores;
        var administradoresPaginados = await ModelPaginado<Administrador>.CreateAsync(admistradoresFiltrados, pageNumber ?? 1, 5);

        return View(administradoresPaginados);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Administrador administrador)
    {
        if (ModelState.IsValid)
        {
            administrador.senha = PasswordHelper.HashPassword(administrador.senha);
            _context.Administradores.Add(administrador);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(administrador);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dados = await _context.Administradores.FindAsync(id);

        if (dados == null)
        {
            return NotFound();
        }

        var modelView = new AdministradorEdit
        {
            id = dados.id,
            nome = dados.nome,
            email = dados.email,
        };

        return View(modelView);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AdministradorEdit modelView)
    {
        
        if (id != modelView.id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var administrador = await _context.Administradores.FindAsync(id);

            if (administrador != null)
            {
                administrador.nome = modelView.nome;
                administrador.email = modelView.email;

                _context.Administradores.Update(administrador);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else {
                return NotFound();
            }
        }

        return View();
    }

    public async Task<IActionResult> Details(int? id) 
    {
        if (id == null)
        {
            return NotFound();
        }

        var dados = await _context.Administradores.FindAsync(id);

        if (dados == null)
        {
            return NotFound();
        }

        return View(dados);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dados = await _context.Administradores.FindAsync(id);

        if (dados == null)
        {
            return NotFound();
        }

        return View(dados);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dados = await _context.Administradores.FindAsync(id);

        if (dados == null)
        {
            return NotFound();
        }

        _context.Administradores.Remove(dados);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    
     }
}
