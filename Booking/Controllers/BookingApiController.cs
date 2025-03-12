using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BookingApiController : ControllerBase
{
    private readonly DbContext _context;

    public BookingApiController(DbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingApiController>>> Get()
    {

    }

    [HttpPost]
    public async Task<ActionResult<BookingApiController>> Post()
    {

    }
}

