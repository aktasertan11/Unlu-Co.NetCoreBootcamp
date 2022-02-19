using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hafta1WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        
        private static List<Task> Tasks = new List<Task>()
        {
            new Task
            {
                Id = 1,
                Title = "Patika Dersleri",
                Description = ".Net patikasını takip et",
                Status = Status.getStatus()[0],
                Date = new DateTime(2022,01,22)


            },
             new Task
             {
                Id = 2,
                Title = "Office Hours ",
                Description = "Perşembe günü office hours katıp 20:00",
                Status = Status.getStatus()[1],
                Date = new DateTime(2022,01,13)

             },
             new Task
             {
                Id = 3,
                Title = "Ödev",
                Description = "Haft 1 ödevini yap!",
                Status = Status.getStatus()[0],
                Date = new DateTime(2022,01,15)
             },
             new Task{
                Id = 4,
                Title = "Oyun",
                Description = "Oyun oyna",
                Status = Status.getStatus()[0],
                Date = new DateTime(2022,01,14)
             },
        };


        [HttpGet] // Tarihe göre Sıralama yaparak Getirir. Önce En yakın tarihli olan 
        public ActionResult<List<Task>> Get()
        {
            var tasks = Tasks.OrderBy(t => t.Date).ToList();
            return Ok(tasks);

        }
        [HttpGet("{status}")] //Task Aktif ve ya Pasif olanları getirir;
        public ActionResult<List<Task>> GetByStatus(string status)
        {
            var task = Tasks.Where(x => x.Status == status).ToList();
            if (task == null)
            {
                return NoContent();
            }
            else { return Ok(task); }
        }
        [HttpGet("search")] //filtreleme 
        public ActionResult<List<Task>> GetByFilter([FromQuery] string search)
        {
            var task = Tasks.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
            if (task == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(task);
            }
        }

        [HttpPost] // Yeni task ekleme
        public ActionResult NewTask([FromBody] Task task)
        {
            try
            {
                Tasks.Add(task);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
            return StatusCode(201);
        }

        [HttpPut] //Güncelleme

        public ActionResult UpdateTask( [FromForm] Task task)
        {
            var check = Tasks.Find(x => x.Id == task.Id);
            if (check == null) { return NoContent(); }
            else {
                check.Title = task.Title;
                check.Description = task.Description;
                check.Status = task.Status;
                check.Date = task.Date;

                return Ok(check); }
        }

        [HttpPatch("/TaskStatus/{id}")] 
        public ActionResult StatusUpdate(int id, [FromForm] string status)
        {
            var check = Tasks.Find(x => x.Id == id);
            if (check == null) { return NoContent(); }
            else
            {
                
                check.Status = status;
                

                return Ok(check);
            }
        }
        [HttpDelete("{id}")] //Task Silme işlemi 

        public ActionResult DeletTask(int id)
        {
            var check = Tasks.Find(x => x.Id == id);
            if (check == null) { return NoContent(); }
            else
            {
                Tasks.Remove(check);

                return Ok();
            }
        }









    }
}
