using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostDBContext _context;
        private readonly IHubContext<SignalRServer> _signalRHub;
        public PostsController(PostDBContext context, IHubContext<SignalRServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }




        public int PageSize { get; set; } = 3;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;



        public IActionResult Getposts(int? pageIndex)

        {

            CurrentPage = pageIndex ?? 1;

            var res = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Select(p => new
                {
                    p.AuthorID,
                    AuthorName = p.Author.FullName,
                    p.CreateDate,
                    p.UpdateDate,
                    p.Title,
                    p.Content,
                    p.Status,
                    Category = p.Category.CategoryName,
                    p.PostId
                })
                .ToList();

            TotalPages = (int)Math.Ceiling(res.Count() / (double)PageSize);
            res = res.Skip((CurrentPage - 1) * PageSize)
                                   .Take(PageSize)
                                   .ToList();
            return Ok(new { posts = res, TotalPages });
        }
        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var postDBContext = _context.Posts.Include(p => p.Author).Include(p => p.Category);
            return View(await postDBContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.AppUsers, "UserId", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,AuthorID,CreateDate,UpdateDate,Title,Content,Status,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                await _signalRHub.Clients.All.SendAsync("LoadPosts");
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.AppUsers, "UserId", "FullName", post.AuthorID);
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.AppUsers, "UserId", "FullName", post.AuthorID);
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int PostId, [Bind("PostId,AuthorID,CreateDate,UpdateDate,Title,Content,Status,CategoryId")] Post post)
        {
            if (PostId != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    await _signalRHub.Clients.All.SendAsync("LoadPosts");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
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
            ViewData["AuthorID"] = new SelectList(_context.AppUsers, "UserId", "FullName", post.AuthorID);
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int PostId)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'PostDBContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(PostId);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            await _signalRHub.Clients.All.SendAsync("LoadPosts");
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
