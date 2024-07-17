using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PostDAO
    {
        private static PostDAO instance;
        public static PostDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PostDAO();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var _context = new PostDBContext();
            var list = await _context.Posts.ToListAsync();
            return list;
        }

        public async Task<Post> GetPostById(int id)
        {
            var _context = new PostDBContext();
            var product = await _context.Posts.SingleOrDefaultAsync(s => s.PostId == id);
            return product;
        }

        public async Task Add(Post pro)
        {
            var _db = new PostDBContext();
            _db.Posts.Add(pro);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Post pro)
        {
            var _db = new PostDBContext();
            var o = await GetPostById(pro.PostId);
            if (o != null)
            {
                _db.Posts.Update(pro);
                await _db.SaveChangesAsync();
            }

        }

        public async Task Delete(Post pro)
        {
            var _db = new PostDBContext();
            var o = await GetPostById(pro.PostId);
            if (o != null)
            {
                _db.Posts.Remove(pro);
                await _db.SaveChangesAsync();
            }

        }
    }
}

