using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ODMS.Models;

namespace ODMS.ControllersApi
{
    public class tbld_clusterController : ApiController
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: api/tbld_cluster
        public IQueryable<tbld_cluster> Gettbld_cluster()
        {
            return db.tbld_cluster;
        }

        // GET: api/tbld_cluster/5
        [ResponseType(typeof(tbld_cluster))]
        public IHttpActionResult Gettbld_cluster(int id)
        {
            tbld_cluster tbld_cluster = db.tbld_cluster.Find(id);
            if (tbld_cluster == null)
            {
                return NotFound();
            }

            return Ok(tbld_cluster);
        }

        // PUT: api/tbld_cluster/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbld_cluster(int id, tbld_cluster tbld_cluster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbld_cluster.id)
            {
                return BadRequest();
            }

            db.Entry(tbld_cluster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbld_clusterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tbld_cluster
        [ResponseType(typeof(tbld_cluster))]
        public IHttpActionResult Posttbld_cluster(tbld_cluster tbld_cluster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbld_cluster.Add(tbld_cluster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbld_cluster.id }, tbld_cluster);
        }

        // DELETE: api/tbld_cluster/5
        [ResponseType(typeof(tbld_cluster))]
        public IHttpActionResult Deletetbld_cluster(int id)
        {
            tbld_cluster tbld_cluster = db.tbld_cluster.Find(id);
            if (tbld_cluster == null)
            {
                return NotFound();
            }

            db.tbld_cluster.Remove(tbld_cluster);
            db.SaveChanges();

            return Ok(tbld_cluster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbld_clusterExists(int id)
        {
            return db.tbld_cluster.Count(e => e.id == id) > 0;
        }
    }
}