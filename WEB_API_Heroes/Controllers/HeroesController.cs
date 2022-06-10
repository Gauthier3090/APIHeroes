using ASP_MVC_03_Modele.BLL.Sercices;
using Microsoft.AspNetCore.Mvc;
using WEB_API_Heroes.Mappers;
using WEB_API_Heroes.Models;

namespace WEB_API_Heroes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HeroesController : ControllerBase
{
    private readonly HeroService _repo;

    public HeroesController(HeroService repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult Get()
    {
        //renvoie une 200 + json

        return Ok(_repo.GetAll().ToArray());
    }

    [HttpGet]
    [Route("{name}")]
    public IActionResult GetByName(string name)
    {
        //renvoie une 200 + json

        return Ok(_repo.GetByName(name).Select(m => m.ToApi()));
    }

    [HttpGet]
    [Route("{endurance:min(1)}")]
    public IActionResult GetByEndurance(int endurance)
    {
        //renvoie une 200 + json

        return Ok(_repo.GetByEndurance(endurance).Select(m => m.ToApi()));
    }

    [HttpPost]
    public IActionResult AddHero(HeroApiModel monHero)
    {
        if (_repo.Insert(monHero.ToDto()))
            return Ok(monHero);
        return new BadRequestObjectResult(monHero);
    }
    [HttpPut]
    public IActionResult UpdateHero(int id, HeroApiModel hero)
    {
        if (_repo.Update(id, hero.ToDto()))
            return Ok(hero);
        return new BadRequestObjectResult(hero);
    }

    [HttpPatch]
    [Route("{id:int:min(1)}/{name:alpha}")]
    public IActionResult UpdateHeroByName(int id,string name)
    {
        if (_repo.UpdateByName(id, name))
        {
            HeroApiModel hero = _repo.GetAll().First(m => m.IdHero == id).ToApi();
            return Ok(hero);
        }
            
        return new BadRequestObjectResult(new
        {
            id, name

        });
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteHero(int id)
    {
        return Accepted(_repo.Delete(id));
    }
}