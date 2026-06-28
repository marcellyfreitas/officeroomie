using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeRoomie.Database;
using OfficeRoomie.Helpers;
using OfficeRoomie.Models;
using OfficeRoomie.Models.ViewModels;
using System;
using System.Diagnostics;


namespace OfficeRoomie.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    async public Task<IActionResult> Index(int? pageNumber)
    {
        var salas = _context.Salas.OrderByDescending(a => a.id);
        var salasPaginados = await ModelPaginado<Sala>.CreateAsync(salas, pageNumber ?? 1, 12);

        return View(salasPaginados);
    }

    public async Task<IActionResult> ClienteReserva(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sala = await _context.Salas
            .FirstOrDefaultAsync(m => m.id == id);
        if (sala == null)
        {
            return NotFound();
        }

        var viewModel = new ClienteReserva
        {
            reserva = new Reserva(),
            sala = sala,
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ClienteReserva(int id, ClienteReserva dto)
    {
        var cliente = new Cliente
        {
            nome = dto.cliente.nome,
            email = dto.cliente.email,
        };

        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();

        var reserva = new Reserva
        {
            hora_inicio = dto.reserva.hora_inicio,
            hora_fim = dto.reserva.hora_fim,
            data_reserva = dto.reserva.data_reserva,
            status = "solicitada",
            cliente_id = cliente.id,
            sala_id = id,
            cartao_id = dto.reserva.cartao_id,
            protocolo = ProtocoloHelper.GerarProtocolo(),
        };

        await _context.AddAsync(reserva);
        await _context.SaveChangesAsync();

        TempData["MensagemSucesso"] = "Reserva realizada com sucesso! - " + reserva.protocolo;

        return RedirectToAction(nameof(Index));
    }

    async public Task<IActionResult> Reserva(int? pageNumber, string SearchString, string currentFilter)
    {
        // Controle do filtro de pesquisa
        if (SearchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            SearchString = currentFilter;
        }

        ViewData["CurrentFilter"] = SearchString;

        var reservas = _context.Reservas
            .AsNoTracking()
            .OrderByDescending(a => a.id)
            .Include(r => r.cliente)
            .Include(r => r.sala);

        if (string.IsNullOrEmpty(SearchString))
        {
            var listaVazia = reservas.Where(r => false);
            var reservasPaginadasVazia = await ModelPaginado<Reserva>.CreateAsync(listaVazia, pageNumber ?? 1, 5);
            return View(reservasPaginadasVazia);
        }

        ViewBag.searchString = SearchString;

        var reservasFiltradas = reservas.Where(s => s.protocolo.ToLower().Contains(SearchString.ToLower()));
        var reservasPaginadas = await ModelPaginado<Reserva>.CreateAsync(reservasFiltradas, pageNumber ?? 1, 5);

        return View(reservasPaginadas);
    }

    [HttpPost]
    async public Task<IActionResult> Reserva(int id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var SearchString = "";
        var reserva = await _context.Reservas.FindAsync(id);
        
        if (reserva != null)
        {
            reserva.status = "cancelada";
            SearchString = reserva.protocolo;
            _context.Update(reserva);
        }

        await _context.SaveChangesAsync();


        TempData["MensagemSucesso"] = "Reserva cancelada com sucesso!";

        return RedirectToAction(nameof(Reserva), new { SearchString });
    }

    public IActionResult Sobre()
    {
        return View();
    }

    public IActionResult Ajuda()
    {
        return View();
    }

    public IActionResult Privacidade()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
