﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BikeSegura.Models;

namespace BikeSegura.Controllers
{
    public class ImagensController : Controller
    {
        private Contexto db = new Contexto();

        [Authorize(Roles = "Administrador")]
        // GET: Imagens
        public ActionResult Index()
        {
            var imagens = db.Imagens.Include(i => i.Bicicletas);
            return View(imagens.ToList());
        }

        [Authorize]
        // GET: Imagens/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagens imagens = db.Imagens.Find(id);
            if (imagens == null)
            {
                return HttpNotFound();
            }
            return View(imagens);
        }

        [Authorize]
        // GET: Imagens/Create
        public ActionResult Create()
        {
            ViewBag.BicicletasId = new SelectList(db.Bicicletas.Where(w => w.Ativo == 0), "Id", "Modelo");
            return View();
        }

        // POST: Imagens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Imagem,BicicletasId")] Imagens imagens, IEnumerable<HttpPostedFileBase> arquivoimg)
        {
            if (ModelState.IsValid)
            {
                // Método upload imagem da bicicleta
                string valor = "";
                string nomearquivo = "";
                if (arquivoimg != null)
                {
                    Upload.CriarDiretorio();
                    foreach (HttpPostedFileBase a in arquivoimg) //a de arquivo
                    {
                        nomearquivo = "bicicleta" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(a.FileName);
                        valor = Upload.UploadArquivo(a, nomearquivo);
                        if (valor == "sucesso")
                        {
                            imagens.Imagem = nomearquivo;
                            db.Imagens.Add(imagens);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("ListaUsuario", "Imagens");
                }
                // Fim método upload imagem da bicicleta
            }
            ViewBag.BicicletasId = new SelectList(db.Bicicletas, "Id", "Modelo", imagens.BicicletasId);
            return View(imagens);
        }

        [Authorize]
        // GET: Imagens/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagens imagens = db.Imagens.Find(id);
            if (imagens == null)
            {
                return HttpNotFound();
            }
            ViewBag.BicicletasId = new SelectList(db.Bicicletas.Where(w => w.Ativo == 0), "Id", "Modelo", imagens.BicicletasId);
            return View(imagens);
        }

        // POST: Imagens/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Imagem,BicicletasId")] Imagens imagens, HttpPostedFileBase arquivoimg)
        {
            string valor = ""; // Faz parte do upload
            if (ModelState.IsValid)
            {
                // Método upload imagem do perfil
                if (arquivoimg != null)
                {
                    Upload.CriarDiretorio();
                    string nomearquivo = "bicicleta" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(arquivoimg.FileName);
                    valor = Upload.UploadArquivo(arquivoimg, nomearquivo);
                    if (valor == "sucesso")
                    {
                        Upload.ExcluirArquivo(Request.PhysicalApplicationPath + "Uploads\\" + imagens.Imagem);
                        imagens.Imagem = nomearquivo;
                        db.Entry(imagens).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                // Fim método upload imagem do perfil
                else
                {
                    db.Entry(imagens).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListaUsuario", "Imagens");
            }
            ViewBag.BicicletasId = new SelectList(db.Bicicletas, "Id", "Modelo", imagens.BicicletasId);
            return View(imagens);
        }

        [Authorize]
        // GET: Imagens/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagens imagens = db.Imagens.Find(id);
            if (imagens == null)
            {
                return HttpNotFound();
            }
            return View(imagens);
        }

        // POST: Imagens/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Imagens imagens = db.Imagens.Find(id);
            db.Imagens.Remove(imagens);
            db.SaveChanges();
            return RedirectToAction("ListaUsuario", "Imagens");
        }

        // Ação para excluir a imagem sem precisar confirmar Delete
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public JsonResult ExcluirFoto(string id)
        {
            Imagens f = db.Imagens.Find(Convert.ToInt32(id));
            if (f != null)
            {
                db.Imagens.Remove(f);
                db.SaveChanges();
                return Json("s");
            }
            else
            {
                return Json("n");
            }
        }

        [Authorize]
        // Index de Imagens do Usuário
        public ActionResult ListaUsuario()
        {
            var imagens = db.Imagens.Include(i => i.Bicicletas);
            return View(imagens.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
