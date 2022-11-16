using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project_Library.Areas.Identity.Data;
using Project_Library.Models;

namespace Project_Library.Controllers
{
    [Authorize]
    public class LibrariesController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        //public ApplicationDbContext _context;

        public LibrariesController(ApplicationDbContext context)

        {
          //  _context = context;
            _clientHandler.ServerCertificateCustomValidationCallback =
                (sender, cert, chain, sslPolicyErrors) => { return true; };
        }
        //private readonly ApplicationDbContext _context;
        // GET: Libraries/Search
        // [HttpGet]
        public async Task<ActionResult> Index()
        {
            var library = await GetLibrary();
            return View(library);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string SearchText)
        {
            var library = await GetLibrary();
            if (!String.IsNullOrEmpty(SearchText))
            {
                library = library.Where(j => j.PersonName!.Contains(SearchText)).ToList();
            }

            return View(library);
        }
        // GET: CallLibraryController
       /* public async Task<ActionResult> Index()
        {
            var library = await GetLibrary();
            return View(library);
        }*/
        [HttpGet]
        public async Task<List<Library>> GetLibrary()
        {
            List<Library> libraryList = new List<Library>();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:7157/api/CLibrary"))
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    libraryList = JsonConvert.DeserializeObject<List<Library>>(strJson);
                }
            }
            return libraryList;
        }
        // GET: CallLibraryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Library library = new Library();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:7157/api/CLibrary/Id?id=" + id))
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    library = JsonConvert.DeserializeObject<Library>(strJson);
                }
            }
            return View(library);
        }
        // GET: CallLibraryController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: CallLibraryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Library library)
        {
            try
            {
                Library sue = new Library();
                using (var httpClient = new HttpClient(_clientHandler))
                {
                    StringContent content =
                        new StringContent(JsonConvert.SerializeObject(library), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:7157/api/CLibrary", content))
                    {
                        string strJson = await response.Content.ReadAsStringAsync();
                        sue = JsonConvert.DeserializeObject<Library>(strJson);
                        if (ModelState.IsValid)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                return View(sue);
            }
            catch
            {
                return View();
            }
        }
        // GET: CallLibraryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Library library = new Library();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:7157/api/CLibrary/Id?id=" + id))
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    library = JsonConvert.DeserializeObject<Library>(strJson);
                }
            }
            return View(library);
        }
        // POST: CallLibraryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Library library)
        {
            Library sue = new Library();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content =
                       new StringContent(JsonConvert.SerializeObject(library), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7157/api/CLibrary/" + id, content))
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    library = JsonConvert.DeserializeObject<Library>(strJson);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: CallLibraryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string del = "";
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7157/api/CLibrary/" + id))
                {
                    del = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));

        }
        // POST: CallLibraryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /* private readonly ApplicationDbContext _context;
         public LibrariesController(ApplicationDbContext context)
         {
             _context = context;
         }
         // GET: Libraries
         public async Task<IActionResult> Index()
         {
               return View(await _context.Library.ToListAsync());
         }
         // GET: Libraries/Search
         public async Task<IActionResult> Search()
         {
             return View();
         }
         public async Task<IActionResult> SearchResult(string SearchText)
         {
             return View("Search", await _context.Library.Where(j => j.PersonName.Contains(SearchText)).ToListAsync());
         }
         // GET: Libraries/Details/5
         public async Task<IActionResult> Details(int? id)
         {
             if (id == null || _context.Library == null)
             {
                 return NotFound();
             }

             var library = await _context.Library
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (library == null)
             {
                 return NotFound();
             }

             return View(library);
         }
         // GET: Libraries/Create
         public IActionResult Create()
         {
             return View();
         }
         // POST: Libraries/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("Id,IdPerson,PersonName,BookID,BookName,DateBorrow,DateReturn")] Library library)
         {

             if (ModelState.IsValid)
             {
                 //Console.WriteLine("library.IdPerson = ", library.IdPerson);
                 var info = _context.Library.FirstOrDefault(user => user.IdPerson == library.IdPerson)!;
                 if (info != null)
                 {
                     // return View(await _context.Library.ToListAsync());
                 }
                 else
                 {
                     _context.Add(library);
                     await _context.SaveChangesAsync();
                     return RedirectToAction(nameof(Index));
                     Console.WriteLine("zzzzzzz");
                 }

             }
             return View(library);
         }
         // GET: Libraries/Edit/5
         public async Task<IActionResult> Edit(int? id)
         {
             if (id == null || _context.Library == null)
             {
                 return NotFound();
             }

             var library = await _context.Library.FindAsync(id);
             if (library == null)
             {
                 return NotFound();
             }
             return View(library);
         }
         // POST: Libraries/Edit/5
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, [Bind("Id,IdPerson,PersonName,BookID,BookName,DateBorrow,DateReturn")] Library library)
         {
             if (id != library.Id)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                     _context.Update(library);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!LibraryExists(library.Id))
                     {
                         return NotFound();
                     }
                     else
                     {
                         throw;
                     }
                 }
                 return RedirectToAction(nameof(Index));
             }
             return View(library);
         }
         // GET: Libraries/Delete/5
         public async Task<IActionResult> Delete(int? id)
         {
             if (id == null || _context.Library == null)
             {
                 return NotFound();
             }

             var library = await _context.Library
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (library == null)
             {
                 return NotFound();
             }

             return View(library);
         }
         // POST: Libraries/Delete/5
         [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
             if (_context.Library == null)
             {
                 return Problem("Entity set 'ApplicationDbContext.Library'  is null.");
             }
             var library = await _context.Library.FindAsync(id);
             if (library != null)
             {
                 _context.Library.Remove(library);
             }

             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
         }
         private bool LibraryExists(int id)
         {
           return _context.Library.Any(e => e.Id == id);
         }*/
    }
}
