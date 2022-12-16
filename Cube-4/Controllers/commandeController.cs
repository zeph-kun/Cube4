using Cube_4.Datas;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CommandController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("commande")]
        public IActionResult GetCommande()
        {
            List<Commande> myCommands = context.Commandes.ToList();
            
            if (myCommands.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Commandes:",
                    Commande = myCommands
                });

            } else
            {
                return NotFound(new
                {
                    Message = "Aucune commandes dans la base de donnée !"
                });
            }
        }
        
        [HttpGet("commande/{commandeId}")] 
        public IActionResult GetCommandById(int commandeId)
        {
            Commande? findCommand = context.Commandes.FirstOrDefault(x => x.Id == commandeId);

            if (findCommand == null)
            {
                return NotFound(new
                {
                    Message = "Aucune commande trouvé avec cet ID !"
                });
            } else
            {
                return Ok(new
                {
                    Message = "Commande trouvée !",
                    Commande = new CommandeDTO() { Id = findCommand.Id, Quantite = findCommand.Quantite, Date = findCommand.Date, User = findCommand.User, Article = findCommand.Article}
                });
            }
        }

        [HttpPost("commande")]
        public IActionResult AddCommand(CommandeDTO newCommand, int userId, int articleId, int quantite, bool isFournisseur)
        {
            User? findUser = context.Users.FirstOrDefault(x => x.Id == userId);
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == articleId);

            if (findUser == null || findArticle == null)
            {
                return NotFound(new
                {
                    Message = "Aucun User trouvé avec cet ID !"
                });
            }
            if (findUser.IsAdmin == true && isFournisseur == true) {
                Commande addCommandFournisseur = new Commande()
                {
                    Quantite = quantite,
                    Date = newCommand.Date,
                    User = findUser,
                    Article = findArticle,
                    isFournisseur = true
                };
                Stock? findStockFournisseur = context.Stocks.FirstOrDefault(x => x.ArticleId == articleId);
                if (findStockFournisseur == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucun Article correspondant dans le stock !"
                    });
                }
                findStockFournisseur.Quantite += addCommandFournisseur.Quantite;
                context.Commandes.Add(addCommandFournisseur);
                context.Stocks.Update(findStockFournisseur);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "La commande a été ajouté avec succès!",
                        CommandeId = addCommandFournisseur.Id
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
            else
            {
                Commande addCommand = new Commande()
                {
                    Quantite = quantite,
                    Date = newCommand.Date,
                    User = findUser,
                    Article = findArticle,
                    isFournisseur = false
                };
                Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == articleId);
                if (findStock == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucun Article correspondant dans le stock !"
                    });
                }
                findStock.Quantite -= addCommand.Quantite;
                if (findStock.Quantite < 0)
                {
                    Commande addCommandFournisseur = new Commande()
                    {
                        Quantite = Math.Abs(findStock.Quantite),
                        Date = newCommand.Date,
                        User = findUser,
                        Article = findArticle,
                        isFournisseur = true
                    };
                    Stock? findStockFournisseur = context.Stocks.FirstOrDefault(x => x.ArticleId == articleId);
                    if (findStockFournisseur == null)
                    {
                        return NotFound(new
                        {
                            Message = "Aucun Article correspondant dans le stock !"
                        });
                    }
                    findStockFournisseur.Quantite += addCommandFournisseur.Quantite;
                    context.Commandes.Add(addCommandFournisseur);
                }
                context.Commandes.Add(addCommand);
                context.Stocks.Update(findStock);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "La commande a été ajouté avec succès!",
                        CommandeId = addCommand.Id
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
        
        [HttpPatch("commande")]
        public IActionResult EditCommand(CommandeDTO newInfos)
        {
            Commande? findCommand = context.Commandes.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findCommand != null )
            {
                findCommand.Quantite = newInfos.Quantite;
                findCommand.Date = newInfos.Date;
                findCommand.User = newInfos.User;
                findCommand.Article = newInfos.Article;

                context.Commandes.Update(findCommand);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "La commande a bien été modifié !"
                    });
                } else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur a eu lieu durant la modification..."
                    });
                }
            } else
            {
                return NotFound(new
                {
                    Message = "Aucune commande n'a été trouvé avec cet ID !"
                });
            }
        }
        [HttpDelete("commande/{commandeId}")]
        public IActionResult DeleteCommand(int commandeId)
        {
            Commande? findCommand = context.Commandes.FirstOrDefault(x => x.Id == commandeId);

            if (findCommand == null)
            {
                return NotFound(new
                {
                    Message = "Aucune commande trouvé avec cet ID !"
                });
            }
            else
            {
                context.Commandes.Remove(findCommand);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "La commande a bien été supprimé",
                    });
                } else
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