using Cube_4.Datas;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FournisseurController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public FournisseurController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet("fournisseurs")]
        public IActionResult GetFournisseurs()
        {
            List<Fournisseur> myFournisseurs = context.Fournisseurs.ToList();

            if (myFournisseurs.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Fournisseurs:",
                    Fournisseur = myFournisseurs
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun Fournisseurs dans la base de données !"
                });
            }
        }

        [HttpGet("fournisseurs/{fournisseurId}")]
        public IActionResult GetFournisseurById(int fournisseurId)
        {
            Fournisseur? findFournisseur = context.Fournisseurs.FirstOrDefault(x => x.Id == fournisseurId);

            if (findFournisseur == null)
            {
                return NotFound(new
                {
                    Message = "Aucun fournisseur trouvé avec cet ID !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Fournisseur trouvé !",
                    Article = new FournisseurDTO() { Id = findFournisseur.Id, Nom = findFournisseur.Nom}
                });
            }
        }

        [HttpPost("fournisseurs")]
        public IActionResult add_fournisseurs(FournisseurDTO newFournisseur)
        {
            Fournisseur addFournisseur= new Fournisseur()
            {
                Nom = newFournisseur.Nom
            };
            context.Fournisseurs.Add(addFournisseur);
            if (context.SaveChanges() > 0)
            {
                return Ok(new
                {
                    Message = "Le Fournisseur a été ajouté avec succès!",
                    FournisseurId = addFournisseur.Id
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

        [HttpPatch("fournisseurs")]
        public IActionResult EditFournisseur(FournisseurDTO newInfos)
        {
            Fournisseur? findFournisseur = context.Fournisseurs.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findFournisseur != null)
            {
                findFournisseur.Nom = newInfos.Nom;

                context.Fournisseurs.Update(findFournisseur);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "Le fournisseur a bien été modifié !"
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
                    Message = "Aucun fournisseur n'a été trouvé avec cet ID !"
                });
            }
        }
        [HttpDelete("fournisseurs/{fournisseurId}")]
        public IActionResult DeleteFournisseur(int fournisseurId)
        {
            Fournisseur? findFournisseur = context.Fournisseurs.FirstOrDefault(x => x.Id == fournisseurId);

            if (findFournisseur == null)
            {
                return NotFound(new
                {
                    Message = "Aucun Fournisseur trouvé avec cet ID !"
                });
            }
            else
            {
                context.Fournisseurs.Remove(findFournisseur);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "Le fournisseur a bien été supprimé",
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
