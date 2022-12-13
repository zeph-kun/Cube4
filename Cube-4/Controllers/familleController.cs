using Cube_4.Datas;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{
    [Route("api")]
    [ApiController]
    public class FamilleController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public FamilleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("familles")]
        public IActionResult GetFamilles()
        {
            List<Famille> myFamilles = context.Familles.ToList();

            if (myFamilles.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Familles:",
                    Famille = myFamilles
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucune Familles dans la base de données !"
                });
            }
        }

        [HttpGet("familles/{familleId}")]
        public IActionResult GetFamilleById(int familleId)
        {
            Famille? findFamille = context.Familles.FirstOrDefault(x => x.Id == familleId);

            if (findFamille == null)
            {
                return NotFound(new
                {
                    Message = "Aucune famille trouvé avec cet ID !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Famille trouvée !",
                    Famille = new FamilleDTO() { Id = findFamille.Id, Nom = findFamille.Nom }
                });
            }
        }

        [HttpPost("familles")]
        public IActionResult add_familles(FamilleDTO newFamille)
        {
            Famille addFamille = new Famille()
            {
                Nom = newFamille.Nom
           
            };
            context.Familles.Add(addFamille);
            if (context.SaveChanges() > 0)
            {
                return Ok(new
                {
                    Message = "La famille a été ajouté avec succès!",
                    FamilleId = addFamille.Id
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }

        [HttpPatch("familles")]
        public IActionResult EditFamille(FamilleDTO newInfos)
        {
            Famille? findFamille = context.Familles.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findFamille != null)
            {
                findFamille.Nom = newInfos.Nom;

                context.Familles.Update(findFamille);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "La famille a bien été modifié !"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur a eu lieu durant la modification..."
                    });
                }
            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucune famille n'a été trouvé avec cet ID !"
                });
            }
        }

        [HttpDelete("familles/{familleId}")]
        public IActionResult DeleteFamille(int familleId)
        {
            Famille? findFamille = context.Familles.FirstOrDefault(x => x.Id == familleId);

            if (findFamille == null)
            {
                return NotFound(new
                {
                    Message = "Aucune famille trouvé avec cet ID !"
                });
            }
            else
            {
                context.Familles.Remove(findFamille);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "La famille a bien été supprimé",
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur est survenue..."
                    });
                }
            }
        }
    }
}